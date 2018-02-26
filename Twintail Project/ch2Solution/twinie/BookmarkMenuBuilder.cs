// BookmarkMenuBuilder.cs

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Twin.Forms
{
	/// <summary>
	/// お気に入りメニューを生成するクラス
	/// </summary>
	public class BookmarkMenuBuilder
	{
		private ToolStripMenuItem rootmenu;
		private BookmarkRoot rootfolder;
		private IsUpdateCheckEnabled isUpdateCheckEnabled;

		/// <summary>
		/// お気に入りスレッドが選択されたときに発生
		/// </summary>
		public event ThreadHeaderEventHandler Selected;

		/// <summary>
		/// フォルダの更新チェックがクリックされたときに発生
		/// </summary>
		public event BookmarkEventHandler UpdateCheck;

		/// <summary>
		/// BookmarkMenuBuilder クラスのインスタンスを初期化
		/// </summary>
		public BookmarkMenuBuilder(ToolStripMenuItem menu, BookmarkRoot root)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.rootmenu = menu;
			this.rootfolder = root;

			menu.DropDownOpening += new EventHandler(OnPopup);
			menu.Tag = root;

			CreateSubmenu(menu, root);
		}

		// 指定したお気に入りフォルダのメニューを生成
		private void CreateSubmenu(ToolStripMenuItem parent, BookmarkFolder folder)
		{
			if (folder == null)
				throw new ArgumentNullException("folder");

			List<ToolStripItem> menuList = new List<ToolStripItem>();

			ToolStripMenuItem updateCheckMenu = new ToolStripMenuItem("更新チェック(&C)");
			updateCheckMenu.Click += new EventHandler(OnUpdateCheck);
			updateCheckMenu.Tag = folder;

			if (isUpdateCheckEnabled != null)
				updateCheckMenu.Enabled = isUpdateCheckEnabled();

			ToolStripMenuItem openUpdateThreadMenu = new ToolStripMenuItem("更新スレを開く(&N)");
			openUpdateThreadMenu.Click += new EventHandler(OnOpenThreads);
			openUpdateThreadMenu.Tag = folder;

			menuList.Add(updateCheckMenu);
			menuList.Add(openUpdateThreadMenu);
			menuList.Add(new ToolStripSeparator());

			foreach (BookmarkEntry entry in folder.Children)
			{
				ToolStripMenuItem child = new ToolStripMenuItem(entry.Name);
				child.Tag = entry;

				if (entry.IsLeaf)
				{
					child.Click += new EventHandler(OnClick);
				}
				else {
					child.DropDownItems.Add(new ToolStripMenuItem("dummy"));
					child.DropDownOpening += new EventHandler(OnPopup);
				}

				menuList.Add(child);
			}

			parent.DropDownItems.Clear();
			parent.DropDownItems.AddRange(menuList.ToArray());
		}

		private void OnPopup(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			CreateSubmenu(menu, menu.Tag as BookmarkFolder);
		}

		private void OnClick(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			BookmarkThread bookmark = menu.Tag as BookmarkThread;

			if (Selected != null)
				Selected(this, new ThreadHeaderEventArgs(bookmark.HeaderInfo));
		}

		private void OnUpdateCheck(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			BookmarkFolder folder = menu.Tag as BookmarkFolder;

			if (UpdateCheck != null)
				UpdateCheck(this, new BookmarkEventArgs(folder));
		}

		private void OnOpenThreads(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			BookmarkFolder folder = menu.Tag as BookmarkFolder;

			List<ThreadHeader> items = folder.GetBookmarks(true);

			// 更新スレのみ残す
			for (int i = items.Count-1; i >= 0; i--)
			{
				if (items[i].SubNewResCount == 0)
					items.RemoveAt(i);
			}

			if (Selected != null)
				Selected(this, new ThreadHeaderEventArgs(items));
		}

		/// <summary>
		/// 更新チェックが可能かどうかを判断するために呼ばれるハンドラを設定
		/// </summary>
		/// <param name="handler"></param>
		public void SetUpdateCheckEnabled(IsUpdateCheckEnabled method)
		{
			isUpdateCheckEnabled = method;
		}
	}

	public class BookmarkMenuItem : ToolStripMenuItem
	{
		private BookmarkEntry entry;

		public BookmarkEntry Entry {
			get {
				return entry;
			}
		}

		public BookmarkMenuItem(BookmarkEntry entry, EventHandler click, EventHandler popup)
			: base(entry.Name)
		{
			if (click != null) base.Click += click;
			if (popup != null) base.DropDownOpening += popup;
			this.entry = entry;
		}
	}

	public class BookmarkEventArgs : EventArgs
	{
		private BookmarkFolder folder;

		public BookmarkFolder Selected {
			get {
				return folder;
			}
		}

		public BookmarkEventArgs(BookmarkFolder entry)
		{
			this.folder = entry;
		}
	}

	public delegate bool IsUpdateCheckEnabled();
	public delegate void BookmarkEventHandler(object sender, BookmarkEventArgs e);
}
