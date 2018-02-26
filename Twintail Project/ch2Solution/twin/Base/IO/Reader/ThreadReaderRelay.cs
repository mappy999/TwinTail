// ThreadReaderRelay.cs

namespace Twin.IO
{
	using System;
	using System.IO;
	using System.Text;
	using System.Diagnostics;
	using System.Net;

	using Twin.Bbs;

	/// <summary>
	/// ���[�_�[����ǂݎ�����X���b�h���L���b�V�����邽�߂̒��p
	/// </summary>
	public class ThreadReaderRelay : ThreadReader
	{
		private Cache cache;
		private ThreadStorage storage;
		private ThreadReader baseReader;
		private ThreadHeader headerInfo;
		private bool isOpen;
		private bool readCache;

		private int length;
		private int position;

		/// <summary>
		/// ��ɂȂ�L���b�V�������擾
		/// </summary>
		public Cache Cache
		{
			get
			{
				return cache;
			}
		}

		/// <summary>
		/// �X���b�h�̃w�b�_�����擾
		/// </summary>
		public ThreadHeader Header
		{
			get
			{
				return headerInfo;
			}
		}

		/// <summary>
		/// �f�[�^�̒������擾
		/// </summary>
		public override int Length
		{
			get
			{
				return length;
			}
		}

		/// <summary>
		/// �X�g���[���̌��݈ʒu���擾
		/// </summary>
		public override int Position
		{
			get
			{
				return position;
			}
		}

		/// <summary>
		/// �X�g���[���̓ǂݍ��݂Ɏg�p����o�b�t�@�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public override int BufferSize
		{
			set
			{
				baseReader.BufferSize = value;
			}
			get
			{
				return baseReader.BufferSize;
			}
		}

		/// <summary>
		/// �t�@�C�����J����Ă��邩�ǂ������擾
		/// </summary>
		public override bool IsOpen
		{
			get
			{
				return isOpen;
			}
		}

		/// <summary>
		/// �L���b�V����ǂݍ��ނ��ǂ������擾�܂��͐ݒ�
		/// </summary>
		public bool ReadCache
		{
			set
			{
				if (readCache != value)
					readCache = value;
			}
			get
			{
				return readCache;
			}
		}

		/// <summary>
		/// User-Agent���擾�܂��͐ݒ�
		/// </summary>
		public override string UserAgent
		{
			set
			{
				baseReader.UserAgent = value;
			}
			get
			{
				return baseReader.UserAgent;
			}
		}

		/// <summary>
		/// �����擾���ɂ��ځ[������o�����Ƃ��ɔ���
		/// </summary>
		public override event EventHandler ABone
		{
			add
			{
				baseReader.ABone += value;
			}
			remove
			{
				baseReader.ABone -= value;
			}
		}

		/// <summary>
		/// dat�������Ă���Ƃ��ɔ���
		/// </summary>
		public override event EventHandler<PastlogEventArgs> Pastlog
		{
			add
			{
				baseReader.Pastlog += value;
			}
			remove
			{
				baseReader.Pastlog -= value;
			}
		}

		/// <summary>
		/// �L���b�V���̓ǂݍ��݂����������Ƃ��ɔ���
		/// </summary>
		public event EventHandler CacheComplete;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="baseReader"></param>
		/// <param name="storage"></param>
		protected ThreadReaderRelay(Cache cache,
			ThreadReader baseReader,
			ThreadStorage storage)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (baseReader == null)
			{
				throw new ArgumentNullException("baseReader");
			}
			if (storage == null)
			{
				throw new ArgumentNullException("storage");
			}

			this.cache = cache;
			this.storage = storage;
			this.baseReader = baseReader;
			this.isOpen = false;
			this.readCache = true;
		}

		/// <summary>
		/// ThreadReaderRelay�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache">��ɂȂ�L���b�V�����</param>
		/// <param name="baseReader">��ɂȂ郊�[�_�[�̃C���X�^���X</param>
		public ThreadReaderRelay(Cache cache, ThreadReader baseReader)
			: this(cache, baseReader, new LocalThreadStorage(cache))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// �X���b�h���J��
		/// </summary>
		/// <param name="th"></param>
		public override bool Open(ThreadHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

retry:

			// �w�b�_�����݂���ꍇ�̓L���b�V���X�g���[�����J��
			if (ThreadIndexer.Exists(cache, header))
			{
				int resCount = header.ResCount;

				ThreadIndexer.Read(cache, header);
				header.ResCount = resCount;

				if (readCache)
				{
					storage.Open(header, StorageMode.Read);
					storage.BufferSize = BufferSize;
					length = storage.Length;
					isOpen = storage.IsOpen;
				}
			}

			if (!storage.IsOpen)
			{
				try
				{
					isOpen = OpenBaseStream(header);
				}
				catch (X2chRetryKakologException ex)
				{
					header.BoardInfo = ex.RetryBoard;
					goto retry;
				}
			}

			headerInfo = header;
			headerInfo.NewResCount = 0;

			position = 0;

			return isOpen;
		}

		/// <summary>
		/// �f�B�X�N�ɃL���b�V�����Ȃ��烌�X��ǂݍ���
		/// </summary>
		/// <param name="resSets"></param>
		/// <param name="byteParsed">��͂��ꂽ���o�C�g�����i�[�����</param>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g����Ԃ�</returns>
		public override int Read(ResSetCollection resSets, out int byteParsed)
		{
			if (!isOpen)
			{
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			byteParsed = 0;

			ResSetCollection tempCollection = new ResSetCollection();
			int byteCount = 0;
			int writeCount = 0;

			// �L���b�V����ǂ�
			if (storage.IsOpen && storage.CanRead)
			{
				byteCount = storage.Read(tempCollection, out byteParsed);
				tempCollection.IsNew = false;

				// �f�[�^���Ȃ���΃L���b�V���X�g���[���͕���
				if (byteCount == 0)
				{
					storage.Close();
					OnCacheComplete(this, new EventArgs());
					OpenBaseStream(headerInfo);
				}
			}

			// �L���b�V�����Ȃ���Ύ��ۂɊ�{�X�g���[������ǂݍ���
			if (baseReader.IsOpen)
			{
				byteCount = baseReader.Read(tempCollection, out byteParsed);
				tempCollection.IsNew = true;

				// ���ځ[�񂪂������ꍇ�A�����𒆎~�B
				if (byteCount == -1)
					return -1;

				try
				{
					if (storage.IsOpen)
						writeCount = storage.Write(tempCollection);
				}
				finally
				{
					headerInfo.GotByteCount += byteParsed;
					headerInfo.GotResCount += tempCollection.Count;
					headerInfo.NewResCount += tempCollection.Count;
				}
			}

			resSets.AddRange(tempCollection);
			position += byteCount;

			return byteCount;
		}

		/// <summary>
		/// �f�B�X�N�ɃL���b�V�����Ȃ���X���b�h�ꗗ��ǂݍ���
		/// </summary>
		/// <param name="resSets"></param>
		/// <returns></returns>
		public override int Read(ResSetCollection resSets)
		{
			int byteParsed;
			return Read(resSets, out byteParsed);
		}

		public override void Cancel()
		{
			if (baseReader != null)
			{
				baseReader.Cancel();
			}
		}

		/// <summary>
		/// �X�g���[������C���f�b�N�X����ۑ�
		/// </summary>
		public override void Close()
		{
			try
			{
				if (isOpen)
				{
					// �X���b�h�̃C���f�b�N�X���쐬
					bool success = ThreadIndexer.Write(cache, headerInfo);
					Debug.Assert(success);

					// �����C���f�b�N�X���쐬
					GotThreadListIndexer.Write(Cache, headerInfo);
				}
			}
			finally
			{
				isOpen = false;
				length = position = 0;

				storage.Close();
				baseReader.Close();
			}
		}

		/// <summary>
		/// ��{�X�g���[�����J��
		/// </summary>
		protected bool OpenBaseStream(ThreadHeader headerInfo)
		{
			if (baseReader.IsOpen)
				return false;

			// �ߋ����O�������Ă��Ȃ��ꍇ�̂݊J��
			//0324 if (!headerInfo.Pastlog)
			{
				baseReader.Open(headerInfo);

				// �X���b�h���J���̂ɐ��������ꍇ�A�X�g���[�W���J��
				if (baseReader.IsOpen)
				{
					length = baseReader.Length;
					position = 0;
					return storage.Open(headerInfo, StorageMode.Write);
				}
			}
			return false;
		}

		/// <summary>
		/// CacheComplete�C�x���g�𔭐�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCacheComplete(object sender, EventArgs e)
		{
			if (CacheComplete != null)
				CacheComplete(sender, e);
		}
	}
}
