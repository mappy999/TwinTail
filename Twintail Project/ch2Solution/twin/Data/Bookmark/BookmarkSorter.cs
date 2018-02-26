// BookmarkSorter.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Windows.Forms;

	/// <summary>
	/// BookmarkCollection�̗v�f���\�[�g����N���X
	/// </summary>
	public class BookmarkSorter : IComparer
	{
		private BookmarkSortObject obj;
		private SortOrder order;

		/// <summary>
		/// BookmarkSorter�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="order"></param>
		public BookmarkSorter(BookmarkSortObject obj, SortOrder order)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.obj = obj;
			this.order = order;
		}

		/// <summary>
		/// x��y���r
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y)
		{
			BookmarkEntry item_x = (BookmarkEntry)x;
			BookmarkEntry item_y = (BookmarkEntry)y;

			switch (obj)
			{
				// ���O���Ń\�[�g
			case BookmarkSortObject.Name:
				return CompareInternal(item_x, item_y);
			}

			throw new Exception();
		}

		/// <summary>
		/// x��y���r
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private int CompareInternal(BookmarkEntry entry1, BookmarkEntry entry2)
		{
			// �t�H���_�΂��C�ɓ���A�܂��͂��C�ɓ���΃t�H���_�̏ꍇ�́A
			// �t�H���_��D�悷��B
			if (entry1 is BookmarkFolder && entry2 is BookmarkThread)
				return -1;

			if (entry1 is BookmarkThread && entry2 is BookmarkFolder)
				return 1;
			// ---------------------------------------

			// ���C�ɓ���΂��C�ɓ���
			return (order == SortOrder.Ascending) ?
				String.Compare(entry1.Name, entry2.Name) :
				String.Compare(entry2.Name, entry1.Name);
		}
	}
}
