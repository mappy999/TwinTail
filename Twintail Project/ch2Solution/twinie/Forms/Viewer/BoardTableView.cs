// BoardTableView.cs

namespace Twin.Forms
{
	using System;
	using System.ComponentModel;
	using System.Collections;
	using System.Diagnostics;
	using System.Windows.Forms;
	using System.Drawing;
	using Twin;
	using Twin.Forms;
	using Twin.Tools;
	using System.Net;
	using System.IO;

	/// <summary>
	/// 板一覧を表示するツリービュー
	/// </summary>
	public class BoardTableView : TreeView
	{
		private DesignSettings.TableDesignSettings tableDesign;

		private LocalRuleViewer localrule = null;
		private BoardPictureViewer pictview = null;
		private OpenMode openMode;
		private IBoardTable table;
		private BoardInfo selected;
		private bool alwaysSingleOpen;

		/// <summary>
		/// 選択されている板を取得または設定
		/// </summary>
		public BoardInfo SelectedItem {
			set {
				if (value == null) {
					throw new ArgumentNullException("SelectedItem");
				}
				Select(value);
			}
			get {
				return selected;
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
		/// カテゴリを常に１つだけ開く場合はtrue
		/// </summary>
		public bool AlwaysSingleOpen {
			set { alwaysSingleOpen = value; }
			get { return alwaysSingleOpen; }
		}

		/// <summary>
		/// 表示する板一覧を取得または設定
		/// </summary>
		public IBoardTable Table {
			set {
				if (value == null)
					throw new ArgumentNullException("Table");

				SetTable(value);
			}
			get { return table; }
		}

		/// <summary>
		/// 板が選択されたときに発生
		/// </summary>
		public event BoardTableEventHandler Selected;

		/// <summary>
		/// BoardViewクラスのインスタンスを初期化
		/// </summary>
		public BoardTableView(DesignSettings.TableDesignSettings design)
		{
			tableDesign = design;

			AlwaysSingleOpen = false;
			HotTracking = true;
			HideSelection = false;
			FullRowSelect = true;
			Font = new Font(design.FontName, design.FontSize);

			KeyPress += new KeyPressEventHandler(TreeView_KeyPress);
			MouseUp += new MouseEventHandler(TreeView_MouseUp);
			MouseDown += new MouseEventHandler(BoardTableView_MouseDown);
			AfterExpand += new TreeViewEventHandler(TreeView_AfterExpandCollapse);
			AfterCollapse += new TreeViewEventHandler(TreeView_AfterExpandCollapse);
			SetOpenMode(OpenMode.SingleClick);
		}

		private TreeNode clickedNode = null;
		void BoardTableView_MouseDown(object sender, MouseEventArgs e)
		{
			clickedNode = GetNodeAt(e.X, e.Y);
		}

		#region Private メソッド
		private void OnViewerClosed(object sender, EventArgs e)
		{
			if (sender is BoardPictureViewer)
				pictview = null;
			else if (sender is LocalRuleViewer)
				localrule = null;
		}

		private void TreeView_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				BoardInfo board = SelectedNode.Tag as BoardInfo;
				if (board != null)
					OnSelected(board, (ModifierKeys == Keys.Shift));

				e.Handled  = true;
			}
		}

		private void TreeView_AfterExpandCollapse(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			((Category)node.Tag).IsExpanded = node.IsExpanded;

			if (openMode == OpenMode.DoubleClick)
			{
				SelectedNode = node;
			}

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

		private void TreeView_MouseUp(object sender, MouseEventArgs e)
		{
			// 右クリックされたノードを選択状態にする
			if (e.Button == MouseButtons.Right)
			{
				TreeNode node = GetNodeAt(e.X, e.Y);
				if (node == null) return;
				
				SelectedNode = node;

				selected = node.Tag as BoardInfo;
			}
			else if (e.Button == MouseButtons.Left)
			{
				if (clickedNode != null)
				{
					// カテゴリが選択されたら開くまたは閉じる
					if (clickedNode.Tag is Category)
					{
						if (openMode == OpenMode.SingleClick)
						{
							if (alwaysSingleOpen)
								CollapseNode(clickedNode);

							clickedNode.Toggle();
						}
						selected = null;
					}
					// 板が選択されたらSelectedイベントを発生させる
					else if (clickedNode.Tag is BoardInfo)
					{
						selected = (BoardInfo)clickedNode.Tag;
						OnSelected(selected, (ModifierKeys == Keys.Shift));
					}
				}
			}
		}

		/// <summary>
		/// 板一覧をツリービューに設定
		/// </summary>
		/// <param name="newTable">新しく設定する板一覧</param>
		private void SetTable(IBoardTable newTable)
		{
			if (newTable == null) {
				throw new ArgumentNullException("newTable");
			}

			if (tableDesign.Coloring)
			{
				TreeViewUtility.SetTable(this, newTable,
					Icons.FolderNormal, Icons.FolderNormal,
					Icons.ItemNormal, Icons.ItemSelected, tableDesign.CateBackColor, tableDesign.BoardBackColor);
			}
			else {
				TreeViewUtility.SetTable(this, newTable,
					Icons.FolderNormal, Icons.FolderNormal, Icons.ItemNormal, Icons.ItemSelected);
			}

			table = newTable;
		}

		/// <summary>
		/// 指定した板のノードを選択
		/// </summary>
		/// <param name="board"></param>
		private void Select(BoardInfo board)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			foreach (TreeNode category in Nodes)
			{
				foreach (TreeNode node in category.Nodes)
				{
					BoardInfo item = (BoardInfo)node.Tag;
					if (item.Equals(board))
					{
						if (alwaysSingleOpen)
							CollapseNode(category);

						SelectedNode = node;

						node.EnsureVisible();
						selected = item;
						return;
					}
				}
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
		/// 指定したノード以外のカテゴリを縮小
		/// </summary>
		/// <param name="without">縮小しないノード</param>
		private void CollapseNode(TreeNode without)
		{
			foreach (TreeNode temp in Nodes)
				if (without != temp) temp.Collapse();
		}

		/// <summary>
		/// Selectedイベントを発生させる
		/// </summary>
		/// <param name="board"></param>
		/// <param name="isNewOpen"></param>
		private void OnSelected(BoardInfo board, bool isNewOpen)
		{
			if (Selected != null)
				Selected(this, new BoardTableEventArgs(board, isNewOpen));
		}
		#endregion
		
		/// <summary>
		/// 指定した板のローカルルールを表示
		/// </summary>
		public void ShowLocalRule(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (localrule == null) {
				localrule = new LocalRuleViewer();
				localrule.Closed += new EventHandler(OnViewerClosed);
				localrule.TopMost = true;
			}

			localrule.Show(board);
		}

		/// <summary>
		/// 指定した板の看板を表示
		/// </summary>
		public void ShowPicture(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (pictview == null) {
				pictview = new BoardPictureViewer();
				pictview.Closed += new EventHandler(OnViewerClosed);
				pictview.TopMost = true;
			}

			pictview.Show(board);
		}

		public void ShowSettingTxt(BoardInfo board)
		{
			if (board == null)
				return;

			SettingTxtManager st = new SettingTxtManager();

			st.BeginDownload(board,
				delegate (object sender, DownloadDataCompletedEventArgs e)
			{
				Process.Start("notepad.exe", ((FileInfo)e.UserState).FullName);
			});
		}

		/// <summary>
		/// 選択されている板のログを削除
		/// </summary>
		public void DeleteLog()
		{
			if (selected != null)
			{
				DialogResult r = MessageBox.Show(selected.Name + "板の全既得ログを削除します。よろしいですか？", "削除確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (r == DialogResult.Yes)
					Twinie.Cache.Remove(selected);
			}
		}
	}
}
