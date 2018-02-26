// OfflineThreadListReader.cs

namespace Twin.IO
{
	using System;
	using System.IO;
	using System.Collections.Generic;

	/// <summary>
	/// �����ς݃X���b�h�̈ꗗ��ǂݍ��ދ@�\���
	/// </summary>
	public class OfflineThreadListReader : ThreadListReaderBase
	{
		private Cache cache;
		private string[] indexFiles;

		/// <summary>
		/// OfflineThreadListReader�N���X�̃C���X�^���X��������
		/// </summary>
		public OfflineThreadListReader(Cache log) : base(null)
		{
			if (log == null) {
				throw new ArgumentNullException("log");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			cache = log;
		}

		/// <summary>
		/// ���[�J���ɕۑ����ꂽ�ꗗ������
		/// </summary>
		/// <param name="board"></param>
		public override bool Open(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (isOpen) {
				throw new InvalidOperationException("�ꗗ��ǂݍ��ݒ��ł�");
			}

			// ���O�f�B���N�g��
			string folder = cache.GetFolderPath(board);

			// ���ׂẴC���f�b�N�X�t�@�C��������
			indexFiles = Directory.GetFiles(folder, "*.idx");
			length = indexFiles.Length;
			boardinfo = board;
			position = 0;
			isOpen = true;

			return isOpen;
		}

		/// <summary>
		/// ���[�J���ɑ��݂��邷�ׂẴX���b�h��������
		/// </summary>
		/// <param name="headers"></param>
		/// <returns></returns>
		public override int Read(List<ThreadHeader> headers)
		{
			int temp;
			return Read(headers, out temp);
		}

		/// <summary>
		/// ���[�J���ɑ��݂��邷�ׂẴX���b�h��������
		/// </summary>
		/// <param name="headers"></param>
		/// <returns></returns>
		public override int Read(List<ThreadHeader> headers, out int cntParsed)
		{
			if (!isOpen) {
				throw new InvalidOperationException("�J����Ă��܂���");
			}
			if (headers == null) {
				throw new ArgumentNullException("headers");
			}

			int max = Math.Min(position + 16, length);
			int temp = position;

			while (position < max)
			{
				ThreadHeader head = TypeCreator.CreateThreadHeader(boardinfo.Bbs);
				head.BoardInfo = boardinfo;
				head.Key = Path.GetFileNameWithoutExtension(indexFiles[position]);

				ThreadIndexer.Read(cache, head);
				headers.Add(head);
				position++;
			}

			cntParsed = (position - temp);
			return cntParsed;
		}
	}
}
