using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Twin.Bbs;

namespace Twin.Forms
{

	public delegate void TabDestroyEventHandler<TControl>(TControl control);
	public delegate TControl TabCreateEventHandler<THeader, TControl>(THeader header, bool newWindow);

	public partial class Twin2IeBrowser : ITwinTableControl
	{
		#region コントロール操作メソッド

		#region ITwinTableControl
		/// <summary>
		/// 選択されている板を取得または設定
		/// </summary>
		BoardInfo ITwinTableControl.Selected
		{
			set
			{
				if (value == null)
					throw new ArgumentNullException("Selected");
				tableView.SelectedItem = value;
			}
			get
			{
				return tableView.SelectedItem;
			}
		}

		/// <summary>
		/// 板が選択されているかどうかを判断
		/// </summary>
		bool ITwinTableControl.IsSelected
		{
			get
			{
				return (tableView.SelectedItem != null);
			}
		}

		/// <summary>
		/// 板名に指定した文字列を含む板を検索
		/// </summary>
		BoardInfo[] ITwinTableControl.Find(string text)
		{
			if (text == null)
				return null;

			List<BoardInfo> list = new List<BoardInfo>();
			text = text.ToLower();

			foreach (Category category in tableView.Table.Items)
			{
				foreach (BoardInfo board in category.Children)
				{
					if (board.Name.ToLower().IndexOf(text) != -1)
						list.Add(board);
				}
			}

			return list.ToArray();
		}
		#endregion

		#endregion
	}

	public class TabController<THeader, TControl> : ITwinTabController<THeader, TControl>
		where TControl : ClientBaseEx<THeader>
	{
		private TabControl tabCtrl;

		private TabCreateEventHandler<THeader, TControl> TabCreate;
		private TabDestroyEventHandler<TControl> TabDestroy;

		protected TabControl TabControl
		{
			get
			{
				return tabCtrl;
			}
		}

		public TabController(TabControl parent,
			TabCreateEventHandler<THeader, TControl> tabCreateMethod,
			TabDestroyEventHandler<TControl> tabDestroyMethod)
		{
			this.tabCtrl = parent;
			this.TabCreate = tabCreateMethod;
			this.TabDestroy = tabDestroyMethod;
		}

		#region ITwinTabController<THeader,TControl> メンバ

		public TControl Create(THeader header, bool newWindow)
		{
			return TabCreate(header, newWindow);
		}

		public void Destroy(TControl window)
		{
			TabDestroy(window);
		}

		public TControl FindControl(THeader header)
		{
			if (header == null)
				return null;

			TControl[] controls = GetControls();

			foreach (TControl ctrl in controls)
			{
				if (ctrl.HeaderInfo == null)
					TwinDll.Output("FindControl, HeaderInfo is null");

				else if (ctrl.HeaderInfo.Equals(header))
					return ctrl;
			}

			return null;
		}

		public TControl[] GetControls()
		{
			TwinWindow<THeader, TControl>[] windows = GetWindows();
			List<TControl> list = new List<TControl>();

			foreach (TwinWindow<THeader, TControl> win in windows)
				list.Add(win.Control);

			return list.ToArray();
		}

		public TwinWindow<THeader, TControl> FindWindow(THeader header)
		{
			TwinWindow<THeader, TControl>[] windows = GetWindows();

			foreach (TwinWindow<THeader, TControl> win in windows)
			{
				TControl ctrl = win.Control;
				if (ctrl.HeaderInfo.Equals(header))
					return win;
			}

			return null;
		}

		public TwinWindow<THeader, TControl>[] GetWindows()
		{
			List<TwinWindow<THeader, TControl>> list = new List<TwinWindow<THeader, TControl>>();

			foreach (TabPage tab in tabCtrl.TabPages)
			{
				TwinWindow<THeader, TControl> win = (TwinWindow<THeader, TControl>)tab.Tag;
				list.Add(win);
			}

			return list.ToArray();
		}

		public void Clear()
		{
			tabCtrl.Enabled = false;
			foreach (TabPage tab in tabCtrl.TabPages)
			{
				((ThreadControl)tab.Tag).Dispose();
				tab.Dispose();
			}

			tabCtrl.Select();
			tabCtrl.TabPages.Clear();
			tabCtrl.Enabled = true;
		}

		public void Select(bool next)
		{
			int index = tabCtrl.SelectedIndex;
			int tabc = tabCtrl.TabCount;

			if (next)
				index = (index + 1 < tabc) ? index + 1 : 0;
			else
				index = (index - 1 >= 0) ? index - 1 : tabc - 1;

			if (tabc > 0)
				tabCtrl.SelectedIndex = index;
		}

		// NTwin23
		public void Select(int index)
		{
			tabCtrl.SelectedIndex = index;
		}
		// NTwin23

		public void SetText(TControl ctrl, string text)
		{
			((TabPage)ctrl.Tag).Text = text;
		}

		public int WindowCount
		{
			get
			{
				return tabCtrl.TabCount;
			}
		}

		public TControl Control
		{
			get
			{
				TwinWindow<THeader, TControl> win = this.Window;
				return (win != null) ? (TControl)win.Control : null;
			}
		}

		public THeader HeaderInfo
		{
			get
			{
				if (this.IsSelected)
				{
					return (THeader)this.Control.HeaderInfo;
				}
				else
				{
					return default(THeader);
				}
			}
		}

		public TwinWindow<THeader, TControl> Window
		{
			get
			{
				TabPage tab = tabCtrl.SelectedTab;
				return (tab != null) ? (TwinWindow<THeader, TControl>)tab.Tag : null;
			}
		}

		public bool IsSelected
		{
			get
			{
				try
				{
					return tabCtrl.SelectedTab != null;
				}
				catch (NullReferenceException)
				{
				}
				return false;
			}
		}

		public int Index
		{
			get
			{
				TabPage tab = tabCtrl.SelectedTab;

				if (tab == null)
					return -1;

				return tabCtrl.TabPages.IndexOf(tab);
			}
		}
		#endregion
	}

	public enum TabCloseAfterSelectionMode
	{
		Left,
		Right,
	}
}
