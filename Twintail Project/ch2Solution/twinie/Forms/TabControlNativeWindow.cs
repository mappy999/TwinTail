using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Twin
{
	/// <summary>
	/// TabControl の Appearance プロパティを Normal 以外にすると、
	/// なぜか正常に MouseDoubleClick イベントが発生しないため、直接 WndProc で処理する。
	/// </summary>
	public class TabControlNativeWindow : NativeWindow
	{
		public event MouseEventHandler MouseDoubleClick;

		private TabControl tabControl;

		public TabControlNativeWindow(TabControl tab)
		{
			tab.HandleCreated += delegate(object sender, EventArgs e)
			{
				AssignHandle(((Control)sender).Handle);
			};

			tab.HandleDestroyed += delegate
			{
				ReleaseHandle();
			};

			this.tabControl = tab;
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (int)Parameter.WM_LBUTTONDBLCLK)
			{
				MouseButtons buttons = MouseButtons.None;
				int wParam = m.WParam.ToInt32();

				if ((wParam & (int)Parameter.MK_LBUTTON) != 0)
					buttons |= MouseButtons.Left;

				if ((wParam & (int)Parameter.MK_MBUTTON) != 0)
					buttons |= MouseButtons.Middle;

				if ((wParam & (int)Parameter.MK_RBUTTON) != 0)
					buttons |= MouseButtons.Right;

				int x = m.LParam.ToInt32() & 0xFFFF;
				int y = (m.LParam.ToInt32() >> 16) & 0xFFFF;
				
				MouseEventArgs e = new MouseEventArgs(buttons, 2, x, y, 0);

				OnMouseDoubleClick(e);
			}

			base.WndProc(ref m);
		}

		private void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (MouseDoubleClick != null)
				MouseDoubleClick(tabControl, e);
		}

		private enum Parameter
		{
			WM_LBUTTONDBLCLK = 0x0203,
			MK_LBUTTON = 0x0001,
			MK_RBUTTON = 0x0002,
			MK_SHIFT = 0x0004,
			MK_CONTROL = 0x0008,
			MK_MBUTTON = 0x0010,
		}
	}
}
