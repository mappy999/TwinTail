// AaCompare.cs

namespace Twin.Aa
{
	using System;
	using System.Collections;
	using System.IO;

	/// <summary>
	/// AaItem���r����N���X�̒�`
	/// </summary>
	public class AaComparer
	{
		#region Inner Class
		/// <summary>
		/// AaHeader���r����N���X
		/// </summary>
		public class AaHeaderComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				AaHeader item1 = x as AaHeader;
				AaHeader item2 = y as AaHeader;

				if (item1 == null || item2 == null) {
					throw new ArgumentException("x�܂���y��AaItem�^�ł͂���܂���");
				}

				string fn1 = Path.GetFileNameWithoutExtension(item1.FileName);
				string fn2 = Path.GetFileNameWithoutExtension(item2.FileName);

				return fn1.CompareTo(fn2);
			}
		}

		/// <summary>
		/// AaItem���r����N���X
		/// </summary>
		public class AaItemComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				AaItem item1 = x as AaItem;
				AaItem item2 = y as AaItem;

				if (item1 == null || item2 == null) {
					throw new ArgumentException("x�܂���y��AaItem�^�ł͂���܂���");
				}

				return item1.Text.CompareTo(item2.Text);
			}
		}
		#endregion
	}
}
