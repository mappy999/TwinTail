// TabUtil.cs

namespace ImageViewerDll
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	/// <summary>
	/// TabUtil の概要の説明です。
	/// </summary>
	public class TabUtil
	{
		/// <summary>
		/// 指定した位置にあるタブページを取得
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="location">スクリーン座標</param>
		/// <returns></returns>
		public static TabPage GetTabPage(TabControl ctrl, Point location)
		{
			Point pt = ctrl.PointToClient(location);

			for (int i = 0; i < ctrl.TabCount; i++)
			{
				Rectangle rect = ctrl.GetTabRect(i);
				if (rect.Contains(pt)) return ctrl.TabPages[i];
			}
			return null;
		}
	}
}
