// TwinListView.cs

namespace Twin.Forms
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	using ListViewSubItem =
		System.Windows.Forms.ListViewItem.ListViewSubItem;

	public class TwinListView : ListView
	{
		public TwinListView()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			DoubleBuffered = true;
			ShowItemToolTips = true;
		}
	}
}
