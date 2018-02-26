// BookmarkMenuBuilder.cs

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Twin.Forms
{
	/// <summary>
	/// ���C�ɓ��胁�j���[�𐶐�����N���X
	/// </summary>
	public class BookmarkMenuBuilder
	{
		private ToolStripMenuItem rootmenu;
		private BookmarkRoot rootfolder;
		private IsUpdateCheckEnabled isUpdateCheckEnabled;

		/// <summary>
		/// ���C�ɓ���X���b�h���I�����ꂽ�Ƃ��ɔ���
		/// </summary>
		public event ThreadHeaderEventHandler Selected;

		/// <summary>
		/// �t�H���_�̍X�V�`�F�b�N���N���b�N���ꂽ�Ƃ��ɔ���
		/// </summary>
		public event BookmarkEventHandler UpdateCheck;

		/// <summary>
		/// BookmarkMenuBuilder �N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkMenuBuilder(ToolStripMenuItem menu, BookmarkRoot root)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.rootmenu = menu;
			this.rootfolder = root;

			menu.DropDownOpening += new EventHandler(OnPopup);
			menu.Tag = root;

			CreateSubmenu(menu, root);
		}

		// �w�肵�����C�ɓ���t�H���_�̃��j���[�𐶐�
		private void CreateSubmenu(ToolStripMenuItem parent, BookmarkFolder folder)
		{
			if (folder == null)
				throw new ArgumentNullException("folder");

			List<ToolStripItem> menuList = new List<ToolStripItem>();

			ToolStripMenuItem updateCheckMenu = new ToolStripMenuItem("�X�V�`�F�b�N(&C)");
			updateCheckMenu.Click += new EventHandler(OnUpdateCheck);
			updateCheckMenu.Tag = folder;

			if (isUpdateCheckEnabled != null)
				updateCheckMenu.Enabled = isUpdateCheckEnabled();

			ToolStripMenuItem openUpdateThreadMenu = new ToolStripMenuItem("�X�V�X�����J��(&N)");
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

			// �X�V�X���̂ݎc��
			for (int i = items.Count-1; i >= 0; i--)
			{
				if (items[i].SubNewResCount == 0)
					items.RemoveAt(i);
			}

			if (Selected != null)
				Selected(this, new ThreadHeaderEventArgs(items));
		}

		/// <summary>
		/// �X�V�`�F�b�N���\���ǂ����𔻒f���邽�߂ɌĂ΂��n���h����ݒ�
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
