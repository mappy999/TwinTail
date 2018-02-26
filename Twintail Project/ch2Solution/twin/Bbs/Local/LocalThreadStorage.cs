// LocalThreadStorage.cs

namespace Twin.IO
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections;
	using Twin.Text;
	using Twin.Bbs;

	/// <summary>
	/// ���[�J���f�B�X�N�ɃX���b�h��ۑ�����@�\���
	/// </summary>
	public class LocalThreadStorage : ThreadStorage
	{
		private Cache cache;
		private Stream baseStream;
		private ThreadParser dataParser;
		private ThreadFormatter formatter;
		private Encoding encoding;
		private StorageMode mode;
		private bool isOpen;

		private int position;
		private int index;

		/// <summary>
		/// �X�g���[�����J���Ă��邩�ǂ����𔻒f
		/// </summary>
		public override bool IsOpen {
			get { return isOpen; }
		}

		/// <summary>
		/// �L���b�V���f�[�^�̒������擾
		/// </summary>
		public override int Length {
			get {
				if (isOpen) {
					return (int)baseStream.Length;
				}
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}
		}

		/// <summary>
		/// ���݂̃X�g���[���̈ʒu���擾
		/// </summary>
		public override int Position {
			get {
				if (isOpen) {
					return position;
				}
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}
		}

		/// <summary>
		/// �o�b�t�@�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public override int BufferSize {
			set {
				if (value < 512) {
					throw new ArgumentOutOfRangeException("BufferSize");
				}
				bufferSize = value;
			}
			get { return bufferSize; }
		}

		/// <summary>
		/// �ǂݍ��݃��[�h�ŊJ����Ă��邩�ǂ�����\��
		/// </summary>
		public override bool CanRead {
			get { return (mode == StorageMode.Read); }
		}

		/// <summary>
		/// �������݃��[�h�ŊJ����Ă��邩�ǂ�����\��
		/// </summary>
		public override bool CanWrite {
			get { return (mode == StorageMode.Write); }
		}

		/// <summary>
		/// LocalThreadStorage�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache">��ɂȂ�L���b�V�����</param>
		/// <param name="parser">�f�[�^����͂���Ƃ��Ɏg�p����p�[�T</param>
		/// <param name="formatter">�������ݎ��Ɏg�p����t�H�[�}�b�^</param>
		/// <param name="enc">�������ݎ��Ɏg�p����G���R�[�_</param>
		public LocalThreadStorage(Cache cache, ThreadParser parser, ThreadFormatter formatter, Encoding enc)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (parser == null) {
				throw new ArgumentNullException("parser");
			}
			if (formatter == null) {
				throw new ArgumentNullException("formatter");
			}
			if (enc == null) {
				throw new ArgumentNullException("enc");
			}

			this.cache = cache;
			this.dataParser = parser;
			this.formatter = formatter;
			this.encoding = enc;
			this.bufferSize = 4096;
			this.isOpen = false;
		}

		/// <summary>
		/// LocalThreadStorage�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		public LocalThreadStorage(Cache cache)
			: this(cache, new X2chThreadParser(), new X2chThreadFormatter(), TwinDll.DefaultEncoding)
		{
		}

		/// <summary>
		/// LocalThreadStorage�N���X�̃C���X�^���X������������Ɠ����ɁA�X�g���[�W���J��
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header"></param>
		/// <param name="mode"></param>
		public LocalThreadStorage(Cache cache, ThreadHeader header, StorageMode mode)
			: this(cache)
		{
			Open(header, mode);
		}

		/// <summary>
		/// �o�̓X�g���[�����J��
		/// </summary>
		/// <param name="header">�������ރX���b�h�̃w�b�_���</param>
		/// <param name="mode">�X�g���[�W���J�����@</param>
		public override bool Open(ThreadHeader header, StorageMode mode)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			if (isOpen) {
				throw new InvalidCastException("���ɃX�g���[�����J����Ă��܂�");
			}

			// �������ݐ�t�@�C����
			string filePath = cache.GetDatPath(header);

			// �X�g���[�����J��
			if (mode == StorageMode.Read) {
				baseStream = StreamCreator.CreateReader(filePath, header.UseGzip);
			} else {
				baseStream = StreamCreator.CreateWriter(filePath, header.UseGzip, true);
			}

			this.mode = mode;
			this.index = 1;
			this.position = 0;
			this.isOpen = true;

			return true;
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

		/// <summary>
		/// ���X��ǂݍ���
		/// </summary>
		/// <param name="resSets"></param>
		/// <returns></returns>
		public override int Read(ResSetCollection resSets, out int byteParsed)
		{
			if (resSets == null) {
				throw new ArgumentNullException("resSets");
			}
			if (!isOpen) {
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			// �o�b�t�@�Ƀf�[�^��ǂݍ���
			int readCount = baseStream.Read(buffer, 0, buffer.Length);

			// ��͂��ăR���N�V�����Ɋi�[
			ICollection collect = dataParser.Parse(buffer, readCount, out byteParsed);

			foreach (ResSet resSet in collect)
			{
				ResSet res = resSet;
				res.Index = index++;

				resSets.Add(res);
			}

			// ���ۂɓǂݍ��܂ꂽ�o�C�g�����v�Z
			position += readCount;

			return readCount;
		}

		/// <summary>
		/// resCollection���t�@�C���ɏ�������
		/// </summary>
		/// <param name="resCollection">�X���b�h�̓��e���i�[���ꂽResSetCollection�N���X</param>
		/// <returns>���ۂɏ������܂ꂽ�o�C�g��</returns>
		public override int Write(ResSetCollection resCollection)
		{
			if (!CanWrite) {
				throw new NotSupportedException("�������ݗp�ɊJ����Ă��܂���");
			}
			if (resCollection == null) {
				throw new ArgumentNullException("resCollection");
			}
			if (!isOpen) {
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			string textData = formatter.Format(resCollection);
			byte[] byteData = encoding.GetBytes(textData);

			baseStream.Write(byteData, 0, byteData.Length);
			position += byteData.Length;

			return byteData.Length;
		}

		/// <summary>
		/// �X�g���[�������
		/// </summary>
		public override void Close()
		{
			if (isOpen)
			{
				if (baseStream != null)
					baseStream.Close();

				baseStream = null;
				buffer = null;
				isOpen = false;
				position = 0;
			}
		}

		/// <summary>
		/// �g�p���Ă��郊�\�[�X�����
		/// </summary>
		public override void Dispose()
		{
			Close();
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ԍ��̃��X�݂̂�ǂݍ���
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header"></param>
		/// <param name="indices"></param>
		/// <returns></returns>
		public static ResSetCollection ReadResSet(Cache cache, ThreadHeader header, int[] indices)
		{
			ResSetCollection items = new ResSetCollection();
			ResSetCollection temp = new ResSetCollection();

			using (LocalThreadStorage sto = new LocalThreadStorage(cache, header, StorageMode.Read))
			{
				while (sto.Read(temp) != 0);

				foreach (ResSet res in temp)
				{
					foreach (int index in indices)
					{
						if (res.Index == index)
							items.Add(res);
					}
				}

				temp.Clear();
				temp = null;
			}

			return items;
		}
	}
}
