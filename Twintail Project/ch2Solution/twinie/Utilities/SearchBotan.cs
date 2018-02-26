using System;
using System.Collections.Generic;
using System.Text;
using Twin.Tools;

namespace Twin.Forms
{
	/// <summary>
	/// 板ボタンにスレッドタイトル検索のキーワードを登録できるようにするクラス。
	/// ItaBotan関連のクラス全部修正。v2.5.100
	/// </summary>
	internal class SearchBotan
	{
		/// <summary>
		/// ボタンの表示名を表す
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// 検索する文字列を表す
		/// </summary>
		public string SearchString { get; set; }

		public SubjectSearchSorting SearchSorting { get; set; }
		public SubjectSearchOrder SearchOrder { get; set; }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || this.Caption == null) return false;
			return this.Caption == obj.ToString();
		}

		public override string ToString()
		{
			return this.Caption;
		}
	}
}
