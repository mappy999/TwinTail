// BookmarkView.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Threading;
	using Twin.Bbs;
	using Twin.Tools;

	/// <summary>
	/// ���C�ɓ�����c���[�\������R���g���[��
	/// </summary>
	public class BookmarkView : TreeView//, ITwinBookmarkControl//, IPatrolable
	{
		private DesignSettings.TableDesignSettings tableDesign;

		private BookmarkRoot bookmarkRoot;
		private ContextMenuStrip folderContext;
		private ContextMenuStrip bookmarkContext;
		private OpenMode openMode;
		private DropPosition position;
		private bool leftClicked;
		private bool labelEditing;

		/// <summary>
		/// �\������Ă��邨�C�ɓ���̃��[�g���擾
		/// </summary>
		public BookmarkRoot Root {
			get {
				return bookmarkRoot;
			}
		}

		/// <summary>
		/// �I������Ă���G���g�����擾
		/// </summary>
		public BookmarkEntry SelectedEntry {
			get {
				return SelectedNode != null ?
					(SelectedNode.Tag as BookmarkEntry) : null;
			}
		}

		/// <summary>
		/// �I������Ă��邨�C�ɓ�������擾
		/// </summary>
		public BookmarkFolder SelectedFolder {
			get {
				return SelectedNode != null ?
					  (SelectedNode.Tag as BookmarkFolder) : null;
			}
		}

		/// <summary>
		/// �I������Ă��邨�C�ɓ���X���b�h���擾
		/// </summary>
		public BookmarkThread SelectedThread {
			get {
				return SelectedNode != null ?
					(SelectedNode.Tag as BookmarkThread) : null;
			}
		}

		/// <summary>
		/// �J�e�S�����J�����@���擾�܂��͐ݒ�
		/// </summary>
		public OpenMode OpenMode {
			set { SetOpenMode(value); }
			get { return openMode; }
		}

		/// <summary>
		/// �t�H���_�I�����̃R���e�L�X�g���j���[���擾�܂��͐ݒ�
		/// </summary>
		public ContextMenuStrip FolderContextMenu
		{
			set {
				if (value == null) {
					throw new ArgumentNullException("FolderContextMenu");
				}
				folderContext = value;
			}
			get { return folderContext; }
		}

		/// <summary>
		/// ���C�ɓ���I�����̃R���e�L�X�g���j���[���擾�܂��͐ݒ�
		/// </summary>
		public ContextMenuStrip BookmarkContextMenu
		{
			set {
				if (value == null) {
					throw new ArgumentNullException("BookmarkContext");
				}
				bookmarkContext = value;
			}
			get { return bookmarkContext; }
		}

		/// <summary>
		/// ���C�ɓ��肪�I�����ꂽ�Ƃ��ɔ���
		/// </summary>
		public event ThreadHeaderEventHandler Selected;

		/// <summary>
		/// BookmarkView�N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkView(BookmarkRoot bookmarks, DesignSettings.TableDesignSettings design)
		{
			if (bookmarks == null) {
				throw new ArgumentNullException("bookmarks");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			tableDesign = design;
			bookmarkRoot = bookmarks;
			AppendRecursive(Nodes, bookmarks);

			leftClicked = false;
			labelEditing = false;

			AllowDrop = true;
			LabelEdit = true;
			HotTracking = true;
			HideSelection = false;
			FullRowSelect = true;
			Font = new Font(design.FontName, design.FontSize);

			Click += new EventHandler(treeView_Click);
			KeyPress += new KeyPressEventHandler(treeView_KeyPress);
			MouseDown += new MouseEventHandler(treeView_MouseDown);
			AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
			AfterExpand += new TreeViewEventHandler(treeView_AfterExpandCollapse);
			AfterCollapse += new TreeViewEventHandler(treeView_AfterExpandCollapse);
			BeforeLabelEdit += new NodeLabelEditEventHandler(treeView_BeforeLabelEdit);
			AfterLabelEdit += new NodeLabelEditEventHandler(treeView_AfterLabelEdit);
			ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);
			DragEnter += new DragEventHandler(treeView_DragEnter);
			DragOver += new DragEventHandler(treeView_DragOver);
			DragLeave += new EventHandler(treeView_DragLeave);
			DragDrop += new DragEventHandler(treeView_DragDrop);
			SetOpenMode(OpenMode.SingleClick);
		}

		#region TreeView Events
		private void treeView_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				BookmarkThread thread = SelectedNode.Tag as BookmarkThread;
				if (thread != null)
					OnSelected(thread);

				e.Handled = true;
			}
		}

		private void treeView_AfterExpandCollapse(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			BookmarkFolder folder = (node.Tag as BookmarkFolder);

			// �W�J��Ԃ�ۑ�
			if (folder != null)
				folder.Expanded = node.IsExpanded;

			if (node.IsExpanded) 
			{
				node.ImageIndex = 
					node.SelectedImageIndex = Icons.FolderOpen;
			}
			else {
				node.ImageIndex = 
					node.SelectedImageIndex = Icons.FolderNormal;
			}
		}

		// �I�����ꂽ�m�[�h�ɂ���ăR���e�L�X�g���j���[��؂芷����
		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag is BookmarkFolder && folderContext != null) {
				ContextMenuStrip = folderContext;
			}
			else if (e.Node.Tag is BookmarkThread && bookmarkContext != null) {
				ContextMenuStrip = bookmarkContext;
			}
			else {
				ContextMenuStrip = null;
			}
		}

		// �m�[�h���N���b�N���ꂽ�Ƃ��ɔ���
		// �I���m�[�h���J�e�S���Ȃ�W�J�E�k���A�X���b�h�Ȃ�J��
		private void treeView_Click(object sender, EventArgs e)
		{
			Point location = PointToClient(MousePosition);
			TreeNode node = GetNodeAt(location);

			if (node != null && leftClicked)
			{
				if (node.Tag is BookmarkFolder)
				{
					if (openMode == OpenMode.SingleClick)
						node.Toggle();
				}
				else if (node.Tag is BookmarkThread)
				{
					// �X���b�h���J��
					BookmarkThread item = (BookmarkThread)node.Tag;
					OnSelected(item);
				}
			}
		}

		private void treeView_MouseDown(object sender, MouseEventArgs e)
		{
			TreeNode node = GetNodeAt(e.X, e.Y);
			leftClicked = false;

			if (node != null)
			{
				if (e.Button == MouseButtons.Left)
				{
					leftClicked = true;
				}
				// �E�N���b�N���ꂽ�m�[�h��I����Ԃɂ���
				else if (e.Button == MouseButtons.Right)
				{
					SelectedNode = node;
				}
			}
		}

		// ���[�g�t�H���_�ȊO�̃��x���ҏW��������
		private void treeView_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			BookmarkEntry entry = (BookmarkEntry)e.Node.Tag;
			e.CancelEdit = (entry == bookmarkRoot || !labelEditing) ? true : false;
		}

		// ���C�ɓ���t�H���_���̕ύX
		private void treeView_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if (e.Label != null)
			{
				BookmarkEntry entry = (BookmarkEntry)e.Node.Tag;
				entry.Name = e.Label;
				
			}
			labelEditing = false;
		}

		#region DragDrop ���\�b�h
		private enum DragItemType { TreeNode, ThreadColl, None };
		private DragItemType dragItemType;

		// �h���b�O�J�n
		private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				DoDragDrop(e.Item, DragDropEffects.Move);
		}

		// �h���b�O���ꂽ
		private void treeView_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode"))
				dragItemType = DragItemType.TreeNode;

			else if (e.Data.GetDataPresent("Twin.ThreadHeader[]"))
				dragItemType = DragItemType.ThreadColl;

			else // ����ȊO�̓h���b�v���Ȃ�
				dragItemType = DragItemType.None;
		}

		private void treeView_DragLeave(object sender, EventArgs e)
		{

		}

		private void treeView_DragOver(object sender, DragEventArgs e)
		{
			Point location = PointToClient(new Point(e.X, e.Y));
			e.Effect = DragDropEffects.None;

			// �c���[�r���[���̃h���b�v
			if (dragItemType == DragItemType.TreeNode)
			{
				TreeNode source = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
				TreeNode target = GetNodeAt(location);

				BookmarkEntry entryFrom = (BookmarkEntry)source.Tag;
				BookmarkEntry entryTo = (BookmarkEntry)target.Tag;

				// �����̎q�A�܂��̓t�H���_���炨�C�ɓ���̈ړ��͕s��
				if (IsChildren(entryFrom, entryTo) ||
					(!entryFrom.IsLeaf && entryTo.IsLeaf))
				{
					e.Effect = DragDropEffects.None;
				}
				else {
					e.Effect = (source != target) ? 
						DragDropEffects.Move : DragDropEffects.None;
				}

				if (e.Effect != DragDropEffects.None)
				{
					// �I���ʒu�����[�U�[�ɕ�����Ղ��\��
					position = TreeViewUtility.DrawDropTo(this,
						target, location, entryFrom, entryTo);

					// ���[�g�t�H���_�ȊO�ւ̈ړ��͋֎~
					if (entryTo == bookmarkRoot && position != DropPosition.Self)
						e.Effect = DragDropEffects.None;
				}
			}
			// �X���b�h�ꗗ����̃h���b�v
			else if (dragItemType == DragItemType.ThreadColl)
			{
				TreeNode target = GetNodeAt(location);
				if (target != null) e.Effect = DragDropEffects.Copy;
			}
			else {
				e.Effect = DragDropEffects.None;
			}
		}

		// �h���b�O����
		private void treeView_DragDrop(object sender, DragEventArgs e)
		{
			TreeNode target = GetNodeAt(PointToClient(new Point(e.X, e.Y)));

			if (e.Effect == DragDropEffects.Move)
			{
				TreeNode source = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

				BookmarkEntry entryFrom = (BookmarkEntry)source.Tag;
				BookmarkEntry entryTo = (BookmarkEntry)target.Tag;

				// �ړ����̃m�[�h�𕡐�
				TreeNode clone = (TreeNode)source.Clone();

				// �����폜
				source.Remove();
				entryFrom.Remove();

				// ���C�ɓ���΂��C�ɓ���̏ꍇ
				if (entryFrom.IsLeaf && entryTo.IsLeaf)
				{
					// �ړ���C���f�b�N�X
					int index = target.Index - CalcFolderCount(entryTo);
					if (position == DropPosition.Lower) index += 1;
					// �ړ���ɑ}��
					target.Parent.Nodes.Insert(index, clone);
					entryTo.Parent.Children.Insert(index, entryFrom);
				}
				// ���C�ɓ���΃t�H���_�̏ꍇ�A�I�����ꂽ�t�H���_�̍Ō�Ɉړ�
				else if (entryFrom.IsLeaf)
				{
					target.Nodes.Add(clone);
					entryTo.Children.Add(entryFrom);
				}
				// �t�H���_�΃t�H���_�̏ꍇ
				else {
					// Self�Ȃ�ړ���t�H���_�̎q�Ƃ��Ēǉ�
					if (position == DropPosition.Self)
					{
						int folderCount = CalcFolderCount(entryTo);
						target.Nodes.Insert(folderCount, clone);
						entryTo.Children.Insert(folderCount, entryFrom);
					}
					// Upper�Ȃ�ړ���t�H���_�̂P��ɒǉ�
					// Lower�Ȃ�ړ���t�H���_�̂P���֒ǉ�
					else {
						int index = target.Index;
						if (position == DropPosition.Lower) index += 1;
						target.Parent.Nodes.Insert(index, clone);
						entryTo.Parent.Children.Insert(index, entryFrom);
					}
				}
				
			}
			// �X���b�h�ꗗ����̃h���b�v
			else if (e.Effect == DragDropEffects.Copy)
			{
				ThreadHeader[] array = 
					(ThreadHeader[])e.Data.GetData("Twin.ThreadHeader[]");

				BookmarkEntry entryTo = (BookmarkEntry)target.Tag;
				BookmarkEntry folder = entryTo.IsLeaf ? entryTo.Parent : entryTo;

				foreach (ThreadHeader header in array)
				{
					BookmarkEntry entry = new BookmarkThread(header);
					AddBookmarkEntry((BookmarkFolder)folder, entry);
				}
			}

			OnBookmarkChanged();
		}
		#endregion
		#endregion

		#region Private Methods

		/// <summary>
		/// entry��folder�̎q�v�f���ǂ����𔻒f
		/// </summary>
		/// <param name="entry1"></param>
		/// <param name="entry2"></param>
		/// <returns></returns>
		private bool IsChildren(BookmarkEntry folder, BookmarkEntry entry)
		{
			if (folder.IsLeaf)
				return false;

			while (entry != null)
			{
				if (folder.Children.Contains(entry))
					return true;
				entry = entry.Parent;
			}

			return false;
		}

		/// <summary>
		/// �w�肵���G���g���̎q�ɑ��݂���t�H���_�����擾
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		private int CalcFolderCount(BookmarkEntry folder)
		{
			if (folder.IsLeaf)
				return 0;

			int c = 0;
			foreach (BookmarkEntry entry in folder.Children)
				if (!entry.IsLeaf)
					c++;

			return c;
		}

		/// <summary>
		/// ���C�ɓ�������c���[�r���[�ɐݒ�
		/// </summary>
		/// <param name="folder">�V�����ݒ肷�邨�C�ɓ���</param>
		private void AppendRecursive(TreeNodeCollection nodeCollection, BookmarkEntry entry)
		{
			if (entry == null) {
				throw new ArgumentNullException("entry");
			}

			TreeNode node = new TreeNode();
			node.Text = entry.Name;
			node.Tag = entry;
			entry.Tag = node;
			nodeCollection.Add(node);

			// ���C�ɓ���m�[�h���쐬
			if (entry.IsLeaf)
			{
				node.ImageIndex = Icons.ItemNormal;
				node.SelectedImageIndex = Icons.ItemSelected;

				if (tableDesign.Coloring)
					node.BackColor = tableDesign.BoardBackColor;
			}
			else {
				node.ImageIndex = 
					node.SelectedImageIndex = Icons.FolderNormal;

				if (tableDesign.Coloring)
					node.BackColor = tableDesign.CateBackColor;

				// �q�����ꍇ�A�ċN�𗘗p���ď������J��Ԃ�
				foreach (BookmarkEntry child in entry.Children)
					AppendRecursive(node.Nodes, child);

				// �W�J��Ԃ𕜌�
				if (((BookmarkFolder)entry).Expanded)
					node.Expand();
			}
		}

		/// <summary>
		/// �J�������ݒ�
		/// </summary>
		/// <param name="mode"></param>
		private void SetOpenMode(OpenMode mode)
		{
			ShowPlusMinus = 
				ShowRootLines = 
				ShowLines = (mode == OpenMode.DoubleClick) ? true : false;

			openMode = mode;
		}

		/// <summary>
		/// Selected�C�x���g�𔭐�������
		/// </summary>
		/// <param name="item"></param>
		private void OnSelected(BookmarkThread item)
		{
			if (Selected != null)
				Selected(this, new ThreadHeaderEventArgs(item.HeaderInfo));
		}
		#endregion

		/// <summary>
		/// �w�肵���X���b�h�̂��C�ɓ�����������B
		/// ������Ȃ����null��Ԃ��B
		/// </summary>
		public BookmarkEntry Search(ThreadHeader header)
		{
			return bookmarkRoot.Search(header);
		}

		/// <summary>
		/// ���[�g�t�H���_��entry��ǉ�
		/// </summary>
		/// <param name="entry"></param>
		public void AddBookmarkRoot(BookmarkEntry entry)
		{
			AddBookmarkEntry(bookmarkRoot, entry);
		}

		/// <summary>
		/// �w�肵���t�H���_�Ɏw�肵���G���g����ǉ�
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="entry"></param>
		private void AddBookmarkEntry(BookmarkFolder folder, BookmarkEntry entry)
		{
			if (folder == null) {
				throw new ArgumentNullException("folder");
			}
			if (entry == null) {
				throw new ArgumentNullException("entry");
			}

			// ���ɃX���b�h�����C�ɓ���ɓo�^����Ă������[�폜���Ă���ǉ�����
			if (entry is BookmarkThread)
			{
				BookmarkThread bookmark = (BookmarkThread)entry;
			
				if (Contains(bookmark.HeaderInfo))
					RemoveBookmark(bookmark.HeaderInfo);
			}

			folder.Children.Add(entry);

			TreeNode parent = (TreeNode)folder.Tag;
			TreeNode child = new TreeNode();
			child.Text = entry.Name;
			child.Tag = entry;
			entry.Tag = child;

			if (entry is BookmarkFolder) 
			{
				foreach (BookmarkEntry sub in entry.Children)
				{
					TreeNode node = new TreeNode();
					node.Text = sub.Name;
					node.Tag = sub;
					node.ImageIndex = Icons.ItemNormal;
					node.SelectedImageIndex = Icons.ItemSelected;
					child.Nodes.Add(node);
				}
				child.ImageIndex = Icons.FolderNormal;
				child.SelectedImageIndex = Icons.FolderNormal;
				
				if (tableDesign.Coloring)
					child.BackColor = tableDesign.CateBackColor;

				parent.Nodes.Insert(0, child);
			}
			else {
				child.ImageIndex = Icons.ItemNormal;
				child.SelectedImageIndex = Icons.ItemSelected;
				
				if (tableDesign.Coloring)
					child.BackColor = tableDesign.BoardBackColor;

				parent.Nodes.Add(child);
			}

			SelectedNode = child;

			OnBookmarkChanged();
		}

		/// <summary>
		/// �I������Ă���ʒu�ɁA�V�K�t�H���_���쐬
		/// </summary>
		public void NewFolder()
		{
			if (SelectedNode != null &&
				SelectedNode.Tag is BookmarkFolder)
			{
				TreeNode node = SelectedNode;
				BookmarkEntry folder = (BookmarkEntry)node.Tag;

				BookmarkEntry newFolder = new BookmarkFolder("�V�K�t�H���_");
				TreeNode newNode = new TreeNode();
				newNode.ImageIndex = newNode.SelectedImageIndex = Icons.FolderNormal;
				newNode.Text = newFolder.Name;
				newNode.Tag = newFolder;
				newFolder.Tag = newNode;

				folder.Children.Insert(0, newFolder);
				node.Nodes.Insert(0, newNode);

				newNode.EnsureVisible();
				newNode.BeginEdit();

				OnBookmarkChanged();
			}

			OnBookmarkChanged();
		}

		/// <summary>
		/// �I������Ă���G���g���̖��O��ύX
		/// </summary>
		public void Rename()
		{
			if (SelectedNode != null)
			{
				labelEditing = true;
				SelectedNode.BeginEdit();

				OnBookmarkChanged();
			}
		}

		/// <summary>
		/// �w�肵���X���b�h�̂��C�ɓ�����폜
		/// </summary>
		/// <param name="header"></param>
		public void RemoveBookmark(ThreadHeader header)
		{
			BookmarkEntry entry = Search(header);
			if (entry != null)
			{
				TreeNode node = (TreeNode)entry.Tag;
				node.Remove();
				entry.Remove();

				OnBookmarkChanged();
			}
		}

		/// <summary>
		/// �I������Ă���A�C�e�����폜
		/// </summary>
		public void RemoveSelected(bool msgbox)
		{
			TreeNode node = SelectedNode;
			if (node != null)
			{
				BookmarkEntry entry = (BookmarkEntry)node.Tag;
				if (entry != bookmarkRoot)
				{
					if (!msgbox ||
						MessageBox.Show("'" + entry.Name + "' ���폜���Ă���낵���ł���", "�폜�m�F",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						entry.Remove();
						node.Remove();

						OnBookmarkChanged();
					}
				}
			}
		}

		/// <summary>
		/// �I������Ă���t�H���_�̎q�A�C�e���𖼑O���Ń\�[�g
		/// </summary>
		public void SortChildren()
		{
			TreeNode node = SelectedNode;

			if (node != null)
			{
				BookmarkFolder folder = (BookmarkFolder)node.Tag;
				folder.Sort(BookmarkSortObject.Name);

				// �c���[�r���[�ɔ��f
				node.Nodes.Clear();

				foreach (BookmarkEntry entry in folder.Children)
					AppendRecursive(node.Nodes, entry);

				OnBookmarkChanged();
			}
		}

		/// <summary>
		/// �w�肵���X���b�h�����C�ɓ�����ɑ��݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool Contains(ThreadHeader header)
		{
			return bookmarkRoot.Contains(header);
		}

		/// <summary>
		/// ���C�ɓ�������ēx�ǂݍ���
		/// </summary>
		public void RefreshBookmarks()
		{
			Nodes.Clear();
			AppendRecursive(Nodes, bookmarkRoot);
		}

		public event EventHandler BookmarkChanged;

		internal void OnBookmarkChanged()
		{
			if (BookmarkChanged != null)
				BookmarkChanged(this, EventArgs.Empty);
		}
	}
}
