// ThreadHeaderComparer.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Windows.Forms;

	/// <summary>
	/// ThreadHeaderComparer �̊T�v�̐����ł��B
	/// </summary>
	public class ThreadHeaderComparer : IComparer
	{
		public ThreadHeaderComparer()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		public int Compare(object x, object y)
		{
			if (x.Equals(y)) return 0;
			return (x.GetHashCode() > y.GetHashCode()) ? 1 : -1;
		}
	}
}
