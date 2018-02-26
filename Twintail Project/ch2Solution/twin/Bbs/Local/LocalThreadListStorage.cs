// LocalThreadListStorage.cs

namespace Twin.IO
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections.Generic;
	using Twin.Text;
	using Twin.Bbs;

	/// <summary>
	/// ���[�J���f�B�X�N�ɃX���b�h�ꗗ��ۑ�����@�\���
	/// </summary>
	public class LocalThreadListStorage : ThreadListStorage
	{
		private Cache cache;
		private Stream baseStream;
		private ThreadListParser dataParser;
		private ThreadListFormatter formatter;
		private Encoding encoding;
		private StorageMode mode;
		private BoardInfo boardInfo;

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
		/// LocalThreadListStorage�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache">��ɂȂ�L���b�V�����</param>
		/// <param name="parser">�f�[�^����͂���Ƃ��Ɏg�p����p�[�T</param>
		/// <param name="formatter">�������ݎ��Ɏg�p����t�H�[�}�b�^</param>
		/// <param name="enc">�������ݎ��Ɏg�p����G���R�[�_</param>
		public LocalThreadListStorage(Cache cache, ThreadListFormatter formatter, Encoding enc)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (formatter == null) {
				throw new ArgumentNullException("formatter");
			}
			if (enc == null) {
				throw new ArgumentNullException("enc");
			}

			this.cache = cache;
			this.formatter = formatter;
			this.encoding = enc;
			this.isOpen = false;
		}

		/// <summary>
		/// LocalThreadListStorage�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		public LocalThreadListStorage(Cache cache)
			: this(cache, new X2chThreadListFormatter(), Encoding.GetEncoding("Shift_Jis"))
		{
		}

		/// <summary>
		/// �X�g���[�����J��
		/// </summary>
		/// <param name="board">�������ޔ̃w�b�_���</param>
		/// <param name="modeRead">�X�g���[�W���J�����@</param>
		public override bool Open(BoardInfo board, StorageMode mode)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (isOpen) {
				throw new InvalidCastException("���ɃX�g���[�����J����Ă��܂�");
			}

			// �������ݐ�t�@�C����
			string filePath = Path.Combine(cache.GetFolderPath(board), "subject.txt");

			// �X�g���[�����J��
			if (mode == StorageMode.Read) {
				baseStream = StreamCreator.CreateReader(filePath, false);
			} else {
				baseStream = StreamCreator.CreateWriter(filePath, false, false);
			}

			// �p�[�T��������
			dataParser = new X2chThreadListParser(board.Bbs, encoding);

			this.index = 1;
			this.position = 0;
			this.mode = mode;
			this.isOpen = true;
			this.boardInfo = board;

			return true;
		}

		/// <summary>
		/// �f�B�X�N�ɃL���b�V�����Ȃ���X���b�h�ꗗ��ǂݍ���
		/// </summary>
		/// <param name="headers"></param>
		/// <returns></returns>
		public override int Read(List<ThreadHeader> headers)
		{
			int byteParsed;
			return Read(headers, out byteParsed);
		}

		/// <summary>
		/// �X���b�h����ǂݍ���
		/// </summary>
		/// <param name="headerCollects"></param>
		/// <returns></returns>
		public override int Read(List<ThreadHeader> headerCollects, out int byteParsed)
		{
			if (headerCollects == null) {
				throw new ArgumentNullException("headerCollects");
			}
			if (!isOpen) {
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			// �o�b�t�@�Ƀf�[�^��ǂݍ���
			int readCount = baseStream.Read(buffer, 0, buffer.Length);

			// ��͂��ăR���N�V�����Ɋi�[
			Array array = dataParser.Parse(buffer, readCount, out byteParsed);

			foreach (ThreadHeader header in array)
			{
				header.No = index++;
				header.BoardInfo = boardInfo;
				headerCollects.Add(header);
			}

			// ���ۂɓǂݍ��܂ꂽ�o�C�g�����v�Z
			position += readCount;

			return readCount;
		}

		/// <summary>
		/// resCollection���t�@�C���ɏ�������
		/// </summary>
		/// <param name="headerCollects">�X���b�h�̓��e���i�[���ꂽList<ThreadHeader>�N���X</param>
		/// <returns>���ۂɏ������܂ꂽ�o�C�g��</returns>
		public override int Write(List<ThreadHeader> headerCollects)
		{
			if (!CanWrite) {
				throw new NotSupportedException("�������ݗp�ɊJ����Ă��܂���");
			}
			if (headerCollects == null) {
				throw new ArgumentNullException("headerCollects");
			}
			if (!isOpen) {
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			string textData = formatter.Format(headerCollects);
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
			if (baseStream != null)
				baseStream.Close();

			dataParser = null;
			baseStream = null;
			boardInfo = null;
			buffer = null;
			position = 0;
			isOpen = false;
		}

		/// <summary>
		/// �g�p���Ă��郊�\�[�X�����
		/// </summary>
		public override void Dispose()
		{
			Close();
		}
	}
}
