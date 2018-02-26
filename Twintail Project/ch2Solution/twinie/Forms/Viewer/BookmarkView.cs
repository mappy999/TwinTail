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
	/// お気に入りをツリー表示するコントロール
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
		/// 表示されているお気に入りのルートを取得
		/// </summary>
		public BookmarkRoot Root {
			get {
				return bookmarkRoot;
			}
		}

		/// <summary>
		/// 選択されているエントリを取得
		/// </summary>
		public BookmarkEntry SelectedEntry {
			get {
				return SelectedNode != null ?
					(SelectedNode.Tag as BookmarkEntry) : null;
			}
		}

		/// <summary>
		/// 選択されているお気に入り情報を取得
		/// </summary>
		public BookmarkFolder SelectedFolder {
			get {
				return SelectedNode != null ?
					  (SelectedNode.Tag as BookmarkFolder) : null;
			}
		}

		/// <summary>
		/// 選択されているお気に入りスレッドを取得
		/// </summary>
		public BookmarkThread SelectedThread {
			get {
				return SelectedNode != null ?
					(SelectedNode.Tag as BookmarkThread) : null;
			}
		}

		/// <summary>
		/// カテゴリを開く方法を取得または設定
		/// </summary>
		public OpenMode OpenMode {
			set { SetOpenMode(value); }
			get { return openMode; }
		}

		/// <summary>
		/// フォルダ選択時のコンテキストメニューを取得または設定
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
		/// お気に入り選択時のコンテキストメニューを取得または設定
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
		/// お気に入りが選択されたときに発生
		/// </summary>
		public event ThreadHeaderEventHandler Selected;

		/// <summary>
		/// BookmarkViewクラスのインスタンスを初期化
		/// </summary>
		public BookmarkView(BookmarkRoot bookmarks, DesignSettings.TableDesignSettings design)
		{
			if (bookmarks == null) {
				throw new ArgumentNullException("bookmarks");
			}
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
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

			// 展開状態を保存
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

		// 選択されたノードによってコンテキストメニューを切り換える
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

		// ノードがクリックされたときに発生
		// 選択ノードがカテゴリなら展開・縮小、スレッドなら開く
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
					// スレッドを開く
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
				// 右クリックされたノードを選択状態にする
				else if (e.Button == MouseButtons.Right)
				{
					SelectedNode = node;
				}
			}
		}

		// ルートフォルダ以外のラベル編集を許可する
		private void treeView_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			BookmarkEntry entry = (BookmarkEntry)e.Node.Tag;
			e.CancelEdit = (entry == bookmarkRoot || !labelEditing) ? true : false;
		}

		// お気に入りフォルダ名の変更
		private void treeView_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if (e.Label != null)
			{
				BookmarkEntry entry = (BookmarkEntry)e.Node.Tag;
				entry.Name = e.Label;
				
			}
			labelEditing = false;
		}

		#region DragDrop メソッド
		private enum DragItemType { TreeNode, ThreadColl, None };
		private DragItemType dragItemType;

		// ドラッグ開始
		private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				DoDragDrop(e.Item, DragDropEffects.Move);
		}

		// ドラッグされた
		private void treeView_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode"))
				dragItemType = DragItemType.TreeNode;

			else if (e.Data.GetDataPresent("Twin.ThreadHeader[]"))
				dragItemType = DragItemType.ThreadColl;

			else // それ以外はドロップしない
				dragItemType = DragItemType.None;
		}

		private void treeView_DragLeave(object sender, EventArgs e)
		{

		}

		private void treeView_DragOver(object sender, DragEventArgs e)
		{
			Point location = PointToClient(new Point(e.X, e.Y));
			e.Effect = DragDropEffects.None;

			// ツリービュー内のドロップ
			if (dragItemType == DragItemType.TreeNode)
			{
				TreeNode source = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
				TreeNode target = GetNodeAt(location);

				BookmarkEntry entryFrom = (BookmarkEntry)source.Tag;
				BookmarkEntry entryTo = (BookmarkEntry)target.Tag;

				// 自分の子、またはフォルダからお気に入りの移動は不可
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
					// 選択位置をユーザーに分かり易く表示
					position = TreeViewUtility.DrawDropTo(this,
						target, location, entryFrom, entryTo);

					// ルートフォルダ以外への移動は禁止
					if (entryTo == bookmarkRoot && position != DropPosition.Self)
						e.Effect = DragDropEffects.None;
				}
			}
			// スレッド一覧からのドロップ
			else if (dragItemType == DragItemType.ThreadColl)
			{
				TreeNode target = GetNodeAt(location);
				if (target != null) e.Effect = DragDropEffects.Copy;
			}
			else {
				e.Effect = DragDropEffects.None;
			}
		}

		// ドラッグ完了
		private void treeView_DragDrop(object sender, DragEventArgs e)
		{
			TreeNode target = GetNodeAt(PointToClient(new Point(e.X, e.Y)));

			if (e.Effect == DragDropEffects.Move)
			{
				TreeNode source = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

				BookmarkEntry entryFrom = (BookmarkEntry)source.Tag;
				BookmarkEntry entryTo = (BookmarkEntry)target.Tag;

				// 移動元のノードを複製
				TreeNode clone = (TreeNode)source.Clone();

				// 元を削除
				source.Remove();
				entryFrom.Remove();

				// お気に入り対お気に入りの場合
				if (entryFrom.IsLeaf && entryTo.IsLeaf)
				{
					// 移動先インデックス
					int index = target.Index - CalcFolderCount(entryTo);
					if (position == DropPosition.Lower) index += 1;
					// 移動先に挿入
					target.Parent.Nodes.Insert(index, clone);
					entryTo.Parent.Children.Insert(index, entryFrom);
				}
				// お気に入り対フォルダの場合、選択されたフォルダの最後に移動
				else if (entryFrom.IsLeaf)
				{
					target.Nodes.Add(clone);
					entryTo.Children.Add(entryFrom);
				}
				// フォルダ対フォルダの場合
				else {
					// Selfなら移動先フォルダの子として追加
					if (position == DropPosition.Self)
					{
						int folderCount = CalcFolderCount(entryTo);
						target.Nodes.Insert(folderCount, clone);
						entryTo.Children.Insert(folderCount, entryFrom);
					}
					// Upperなら移動先フォルダの１つ上に追加
					// Lowerなら移動先フォルダの１つ下へ追加
					else {
						int index = target.Index;
						if (position == DropPosition.Lower) index += 1;
						target.Parent.Nodes.Insert(index, clone);
						entryTo.Parent.Children.Insert(index, entryFrom);
					}
				}
				
			}
			// スレッド一覧からのドロップ
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
		/// entryがfolderの子要素かどうかを判断
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
		/// 指定したエントリの子に存在するフォルダ数を取得
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
		/// お気に入り情報をツリービューに設定
		/// </summary>
		/// <param name="folder">新しく設定するお気に入り</param>
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

			// お気に入りノードを作成
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

				// 子を持つ場合、再起を利用して処理を繰り返す
				foreach (BookmarkEntry child in entry.Children)
					AppendRecursive(node.Nodes, child);

				// 展開状態を復元
				if (((BookmarkFolder)entry).Expanded)
					node.Expand();
			}
		}

		/// <summary>
		/// 開く動作を設定
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
		/// Selectedイベントを発生させる
		/// </summary>
		/// <param name="item"></param>
		private void OnSelected(BookmarkThread item)
		{
			if (Selected != null)
				Selected(this, new ThreadHeaderEventArgs(item.HeaderInfo));
		}
		#endregion

		/// <summary>
		/// 指定したスレッドのお気に入り情報を検索。
		/// 見つからなければnullを返す。
		/// </summary>
		public BookmarkEntry Search(ThreadHeader header)
		{
			return bookmarkRoot.Search(header);
		}

		/// <summary>
		/// ルートフォルダにentryを追加
		/// </summary>
		/// <param name="entry"></param>
		public void AddBookmarkRoot(BookmarkEntry entry)
		{
			AddBookmarkEntry(bookmarkRoot, entry);
		}

		/// <summary>
		/// 指定したフォルダに指定したエントリを追加
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

			// 既にスレッドがお気に入りに登録されていたら一端削除してから追加する
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
		/// 選択されている位置に、新規フォルダを作成
		/// </summary>
		public void NewFolder()
		{
			if (SelectedNode != null &&
				SelectedNode.Tag is BookmarkFolder)
			{
				TreeNode node = SelectedNode;
				BookmarkEntry folder = (BookmarkEntry)node.Tag;

				BookmarkEntry newFolder = new BookmarkFolder("新規フォルダ");
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
		/// 選択されているエントリの名前を変更
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
		/// 指定したスレッドのお気に入りを削除
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
		/// 選択されているアイテムを削除
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
						MessageBox.Show("'" + entry.Name + "' を削除してもよろしいですか", "削除確認",
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
		/// 選択されているフォルダの子アイテムを名前順でソート
		/// </summary>
		public void SortChildren()
		{
			TreeNode node = SelectedNode;

			if (node != null)
			{
				BookmarkFolder folder = (BookmarkFolder)node.Tag;
				folder.Sort(BookmarkSortObject.Name);

				// ツリービューに反映
				node.Nodes.Clear();

				foreach (BookmarkEntry entry in folder.Children)
					AppendRecursive(node.Nodes, entry);

				OnBookmarkChanged();
			}
		}

		/// <summary>
		/// 指定したスレッドがお気に入り内に存在するかどうかを判断
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool Contains(ThreadHeader header)
		{
			return bookmarkRoot.Contains(header);
		}

		/// <summary>
		/// お気に入り情報を再度読み込み
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
