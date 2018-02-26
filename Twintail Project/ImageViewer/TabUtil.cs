// TabUtil.cs

namespace ImageViewerDll
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	/// <summary>
	/// TabUtil �̊T�v�̐����ł��B
	/// </summary>
	public class TabUtil
	{
		/// <summary>
		/// �w�肵���ʒu�ɂ���^�u�y�[�W���擾
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="location">�X�N���[�����W</param>
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
