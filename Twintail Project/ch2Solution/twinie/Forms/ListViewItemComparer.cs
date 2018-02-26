// ListViewItemComparer.cs

namespace Twin.Forms
{
	using System;
	using System.Collections;
	using System.Windows.Forms;
	using Columns = ThreadListView.ColumnNumbers;

	/// <summary>
	/// ListViewItemComparer �̊T�v�̐����ł��B
	/// </summary>
	public class ListViewItemComparer : IComparer
	{
		private int column;
		private SortOrder order;

		/// <summary>
		/// ListViewItemComparer�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="order">���ڂ̕��ёւ����@</param>
		/// <param name="column">�\�[�g�Ώۂ̃I�u�W�F�N�g�̔ԍ�</param>
		public ListViewItemComparer(SortOrder order, int column)
		{
			this.order = order;
			this.column = column;
		}

		/// <summary>
		/// �ݒ肳��Ă���^�ɕϊ���Ax��y���r
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y)
		{
			if (order == SortOrder.None)
				return 0;

			ThreadHeader hx, hy;

			ListViewItem item1 = x as ListViewItem;
			ListViewItem item2 = y as ListViewItem;

			if (item1 != null && item2 != null)
			{
				if (order == SortOrder.Ascending)
				{
					hx = (ThreadHeader)item1.Tag;
					hy = (ThreadHeader)item2.Tag;
				}
				else {
					hy = (ThreadHeader)item1.Tag;
					hx = (ThreadHeader)item2.Tag;
				}
			}
			else {
				hx = x as ThreadHeader;
				hy = y as ThreadHeader;

				if (hx == null || hy == null)
					return 0;
			}

			ThreadHeaderInfo infox = new ThreadHeaderInfo(hx);
			ThreadHeaderInfo infoy = new ThreadHeaderInfo(hy);

			switch (column)
			{
			case Columns.Subject:
				int length = Math.Min(hx.Subject.Length, hy.Subject.Length);
				return String.CompareOrdinal(hx.Subject, 0, hy.Subject, 0, length);

			case Columns.GotResCount:
				return NumberCompare(hy.GotResCount, hx.GotResCount);

			case Columns.NewResCount:
				return NumberCompare(hy.SubNewResCount, hx.SubNewResCount);

			case Columns.ResCount:
				return NumberCompare(hy.ResCount, hx.ResCount);

			case Columns.Info:
				return NumberCompare(infoy.Valuable, infox.Valuable);

			case Columns.Force:
				return NumberCompare(infoy.ForceValueDay, infox.ForceValueDay);

			case Columns.Size:
				return NumberCompare(hy.GotByteCount, hx.GotByteCount);

			case Columns.No:
				return NumberCompare(hx.No, hy.No);

			case Columns.Date:
				return DateTime.Compare(hy.Date, hx.Date);

			case Columns.LastModified:
				return DateTime.Compare(hy.LastModified, hx.LastModified);

			case Columns.LastWritten:
				return DateTime.Compare(hy.LastWritten, hx.LastWritten);

			case Columns.BoardName:
				return  String.CompareOrdinal(hy.BoardInfo.Name, hx.BoardInfo.Name);

			default:
				throw new Exception("�J�����ԍ����ُ�ł�");
			}
		}

		/// <summary>
		/// ���l���r
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static int NumberCompare(double x, double y)
		{
			if (x.Equals(y))
				return 0;

			return (x > y) ? 1 : -1;
		}
	}
}
