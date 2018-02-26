// GotThreadListReader.cs
// #2.0

namespace Twin.IO
{
	using System;
	using System.Text;
	using System.Collections.Generic;
	using System.IO;
	using Twin;
	using Twin.Text;

	/// <summary>
	/// �����ς݃X���b�h�̈ꗗ��ǂݍ��ދ@�\���
	/// (OfflineThreadListReader��葁��)
	/// </summary>
	public class GotThreadListReader : ThreadListReaderBase
	{
		private Cache cache;
		private BoardInfo boardInfo;
		private List<ThreadHeader> items;

		/// <summary>
		/// GotThreadListReader�N���X�̃C���X�^���X��������
		/// </summary>
		public GotThreadListReader(Cache log)
			: base(null)
		{
			if (log == null)
			{
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
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}
			if (isOpen)
			{
				throw new InvalidOperationException("�ꗗ��ǂݍ��ݒ��ł�");
			}

			// ���O�f�B���N�g��
			string folder = cache.GetFolderPath(board);

			// �C���f�b�N�X�t�@�C����ǂݍ���
			items = GotThreadListIndexer.Read(cache, board);
			length = items.Count;

			position = 0;
			boardInfo = board;
			isOpen = true;

			return isOpen;
		}

		/// <summary>
		/// �f�B�X�N�ɃL���b�V�����Ȃ���X���b�h�ꗗ��ǂݍ���
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
		/// <param name="headers">��͂��ꂽ�w�b�_���i�[�����R���N�V����</param>
		/// <param name="cntParsed">��͂��ꂽ�w�b�_�����i�[����� (���̒l�͖߂�l�Ɠ����ɂȂ�)</param>
		/// <returns>�ǂݍ��܂ꂽ�w�b�_����Ԃ�</returns>
		public override int Read(List<ThreadHeader> headers, out int cntParsed)
		{
			if (!isOpen)
			{
				throw new InvalidOperationException("�J����Ă��܂���");
			}
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}

			int max = Math.Min(position + 32, length);
			int temp = position;

			while (position < max)
			{
				headers.Add(items[position]);
				position++;
			}

			cntParsed = (position - temp);
			return cntParsed;
		}

		/// <summary>
		/// �w�肵���e�[�u���̂��ׂĂ̔̊����X���b�h�����擾
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public static List<ThreadHeader> GetAllThreads(Cache cache, IBoardTable table)
		{
			List<ThreadHeader> items = new List<ThreadHeader>();

			foreach (Category categ in table.Items)
			{
				foreach (BoardInfo board in categ.Children)
				{
					items.AddRange(GotThreadListIndexer.Read(cache, board));
				}
			}

			return items;
		}
	}
}
