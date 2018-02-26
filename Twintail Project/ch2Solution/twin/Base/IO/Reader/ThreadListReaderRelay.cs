// ThreadListReaderRelay.cs
// #2.0

namespace Twin.IO
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ���[�_�[����ǂݎ�����X���b�h�ꗗ���L���b�V�����邽�߂̒��p�N���X�ł��B
	/// </summary>
	public class ThreadListReaderRelay : ThreadListReader
	{
		private Cache cache;						// �L���b�V�����Ǘ����܂��B
		private ThreadListStorage storage;			// ���[�J���ɃL���b�V����ۑ�����N���X�ł��B
		private ThreadListReader baseReader;		// �l�b�g���[�N����ŐV�̃X���b�h�ꗗ����M����N���X�ł��B
		private List<ThreadHeader> cacheItems;		// �L���b�V������Ă���X���b�h�ꗗ�ł��B
		private BoardInfo boardInfo;
		private bool isOpen;

		/// <summary>
		/// ��ɂȂ�L���b�V�������擾���܂��B
		/// </summary>
		public Cache Cache
		{
			get
			{
				return cache;
			}
		}

		/// <summary>
		/// ���݊J����Ă���̏����擾���܂��B
		/// </summary>
		public BoardInfo BoardInfo
		{
			get
			{
				return boardInfo;
			}
		}

		public override int Length
		{
			get
			{
				return baseReader.Length;
			}
		}

		public override int Position
		{
			get
			{
				return baseReader.Position;
			}
		}

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

		public override bool IsOpen
		{
			get
			{
				return isOpen;
			}
		}

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

		public override bool AutoRedirect
		{
			set
			{
				baseReader.AutoRedirect = value;
			}
			get
			{
				return baseReader.AutoRedirect;
			}
		}

		/// <summary>
		/// �L���b�V������Ă���X���b�h�ꗗ���擾���܂��B
		/// </summary>
		public List<ThreadHeader> CacheItems
		{
			get
			{
				return cacheItems;
			}
		}

		/// <summary>
		/// ThreadListReaderRelay�N���X�̃C���X�^���X���������B
		/// </summary>
		/// <param name="cache">��ɂȂ�L���b�V�����</param>
		/// <param name="baseReader">��ɂȂ郊�[�_�[�̃C���X�^���X</param>
		public ThreadListReaderRelay(Cache cache, ThreadListReader baseReader)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}

			if (baseReader == null)
			{
				throw new ArgumentNullException("baseReader");
			}

			this.cache = cache;
			this.baseReader = baseReader;
			this.storage = new LocalThreadListStorage(cache);
			this.cacheItems = new List<ThreadHeader>();
			this.isOpen = false;
		}

		public override bool Open(BoardInfo board)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			if (baseReader.IsOpen)
			{
				throw new InvalidOperationException("���[�_�[���J����Ă��܂�");
			}

			cacheItems.Clear();
			boardInfo = board;

			// �L���b�V�������ׂēǂݍ���
			if (storage.Open(board, StorageMode.Read))
			{
				while (storage.Read(cacheItems) != 0)
					;
				storage.Close();
			}

			// �X�g���[�����J��
			if (baseReader.Open(board))
			{
				isOpen = storage.Open(board, StorageMode.Write);
			}

			return isOpen;
		}

		public override int Read(List<ThreadHeader> headers)
		{
			int byteParsed;
			return Read(headers, out byteParsed);
		}

		public override int Read(List<ThreadHeader> headers, out int byteParsed)
		{
			if (!isOpen)
			{
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			int read = baseReader.Read(headers, out byteParsed);

			// 2010/12/05 ���ʂ���thx!
			if (byteParsed > 0)
			{
				storage.Write(headers);
			} 

			return read;
		}

		public override void Cancel()
		{
			if (baseReader != null && baseReader.IsOpen)
			{
				baseReader.Cancel();
			}
		}

		public override void Close()
		{
			isOpen = false;
			baseReader.Close();
			storage.Close();
		}
	}
}
