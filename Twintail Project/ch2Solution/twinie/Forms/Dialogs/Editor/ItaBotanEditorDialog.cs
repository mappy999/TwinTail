// ItaBotanEditorDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using CSharpSamples;

	/// <summary>
	/// 板ボタンを編集するためのダイアログ
	/// </summary>
	public class ItaBotanEditorDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.TreeView treeViewItab;
		private System.Windows.Forms.TreeView treeViewSrc;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonDel;
		private System.Windows.Forms.Button buttonUp;
		private System.Windows.Forms.Button buttonDown;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonNewCategory;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button buttonLabelEdit;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemAddFolder;
		private System.Windows.Forms.MenuItem menuItemAppend;
		private System.Windows.Forms.ImageList imageList;
		#endregion

		private IBoardTable table;
		private CSharpToolBar itabotan;
		private bool labelEditing;

		/// <summary>
		/// ItaBotanEditorDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="table"></param>
		/// <param name="itabotan"></param>
		public ItaBotanEditorDialog(IBoardTable table, CSharpToolBar itabotan)
		{
			if (table == null) {
				throw new ArgumentNullException("table");
			}
			if (itabotan == null) {
				throw new ArgumentNullException("itabotan");
			}
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.table = table;
			this.itabotan = itabotan;
			this.labelEditing = false;
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItaBotanEditorDialog));
			this.treeViewItab = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.treeViewSrc = new System.Windows.Forms.TreeView();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonDel = new System.Windows.Forms.Button();
			this.buttonUp = new System.Windows.Forms.Button();
			this.buttonDown = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonNewCategory = new System.Windows.Forms.Button();
			this.buttonLabelEdit = new System.Windows.Forms.Button();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemAddFolder = new System.Windows.Forms.MenuItem();
			this.menuItemAppend = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// treeViewItab
			// 
			this.treeViewItab.FullRowSelect = true;
			this.treeViewItab.HideSelection = false;
			this.treeViewItab.ImageIndex = 0;
			this.treeViewItab.ImageList = this.imageList;
			this.treeViewItab.LabelEdit = true;
			this.treeViewItab.Location = new System.Drawing.Point(278, 20);
			this.treeViewItab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.treeViewItab.Name = "treeViewItab";
			this.treeViewItab.SelectedImageIndex = 0;
			this.treeViewItab.Size = new System.Drawing.Size(169, 217);
			this.treeViewItab.TabIndex = 9;
			this.treeViewItab.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeViewItab.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewItab_AfterLabelEdit);
			this.treeViewItab.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewItab_AfterSelect);
			this.treeViewItab.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewItab_BeforeLabelEdit);
			this.treeViewItab.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			this.imageList.Images.SetKeyName(3, "");
			// 
			// treeViewSrc
			// 
			this.treeViewSrc.FullRowSelect = true;
			this.treeViewSrc.HideSelection = false;
			this.treeViewSrc.ImageIndex = 0;
			this.treeViewSrc.ImageList = this.imageList;
			this.treeViewSrc.Location = new System.Drawing.Point(4, 20);
			this.treeViewSrc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.treeViewSrc.Name = "treeViewSrc";
			this.treeViewSrc.SelectedImageIndex = 0;
			this.treeViewSrc.Size = new System.Drawing.Size(169, 217);
			this.treeViewSrc.TabIndex = 1;
			this.treeViewSrc.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeViewSrc.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewSrc_AfterSelect);
			this.treeViewSrc.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			// 
			// buttonAdd
			// 
			this.buttonAdd.AutoSize = true;
			this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAdd.Location = new System.Drawing.Point(181, 20);
			this.buttonAdd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(88, 21);
			this.buttonAdd.TabIndex = 4;
			this.buttonAdd.Text = ">>追加(&A)";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonDel
			// 
			this.buttonDel.AutoSize = true;
			this.buttonDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonDel.Location = new System.Drawing.Point(181, 44);
			this.buttonDel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonDel.Name = "buttonDel";
			this.buttonDel.Size = new System.Drawing.Size(88, 21);
			this.buttonDel.TabIndex = 5;
			this.buttonDel.Text = "<<削除(&D)";
			this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
			// 
			// buttonUp
			// 
			this.buttonUp.AutoSize = true;
			this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonUp.Location = new System.Drawing.Point(454, 72);
			this.buttonUp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(100, 21);
			this.buttonUp.TabIndex = 6;
			this.buttonUp.Text = "上へ(&U)";
			this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
			// 
			// buttonDown
			// 
			this.buttonDown.AutoSize = true;
			this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonDown.Location = new System.Drawing.Point(454, 96);
			this.buttonDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(100, 21);
			this.buttonDown.TabIndex = 7;
			this.buttonDown.Text = "下へ(&N)";
			this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.AutoSize = true;
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClose.Location = new System.Drawing.Point(454, 216);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(105, 21);
			this.buttonClose.TabIndex = 10;
			this.buttonClose.Text = "閉じる(&C)";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 4);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "板一覧(&T)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(281, 4);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "追加された板ボタン(&B)";
			// 
			// buttonNewCategory
			// 
			this.buttonNewCategory.AutoSize = true;
			this.buttonNewCategory.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonNewCategory.Location = new System.Drawing.Point(454, 20);
			this.buttonNewCategory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonNewCategory.Name = "buttonNewCategory";
			this.buttonNewCategory.Size = new System.Drawing.Size(102, 21);
			this.buttonNewCategory.TabIndex = 2;
			this.buttonNewCategory.Text = "カテゴリを作成(&N)";
			this.buttonNewCategory.Click += new System.EventHandler(this.buttonNewCategory_Click);
			// 
			// buttonLabelEdit
			// 
			this.buttonLabelEdit.AutoSize = true;
			this.buttonLabelEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonLabelEdit.Location = new System.Drawing.Point(454, 44);
			this.buttonLabelEdit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonLabelEdit.Name = "buttonLabelEdit";
			this.buttonLabelEdit.Size = new System.Drawing.Size(100, 21);
			this.buttonLabelEdit.TabIndex = 3;
			this.buttonLabelEdit.Text = "名前を編集(&E)";
			this.buttonLabelEdit.Click += new System.EventHandler(this.buttonLabelEdit_Click);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAddFolder,
            this.menuItemAppend});
			this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
			// 
			// menuItemAddFolder
			// 
			this.menuItemAddFolder.Index = 0;
			this.menuItemAddFolder.Text = "選択フォルダに追加(&F)";
			this.menuItemAddFolder.Click += new System.EventHandler(this.menuItemAddFolder_Click);
			// 
			// menuItemAppend
			// 
			this.menuItemAppend.Index = 1;
			this.menuItemAppend.Text = "末尾に追加(&A)";
			this.menuItemAppend.Click += new System.EventHandler(this.menuItemAppend_Click);
			// 
			// ItaBotanEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(569, 243);
			this.Controls.Add(this.buttonLabelEdit);
			this.Controls.Add(this.buttonNewCategory);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.buttonDown);
			this.Controls.Add(this.buttonUp);
			this.Controls.Add(this.buttonDel);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.treeViewSrc);
			this.Controls.Add(this.treeViewItab);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ItaBotanEditorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "板ボタンの編集";
			this.Load += new System.EventHandler(this.ItaBotanEditorDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 板ボタンを保存
		/// </summary>
		private void SaveItaBotan()
		{
			ArrayList list = new ArrayList();

			foreach (TreeNode node in treeViewItab.Nodes)
			{
				if (node.Tag is Category)
				{
					Category cate = (Category)node.Tag;
					cate.Children.Clear();

					CSharpToolBarButton button = new CSharpToolBarButton(cate.Name);
					button.Tag = cate;
					
					foreach (TreeNode child in node.Nodes)
					{
						BoardInfo board = (BoardInfo)child.Tag;
						cate.Children.Add(board);
					}

					list.Add(button);
				}
				else if (node.Tag is BoardInfo)
				{
					BoardInfo board = (BoardInfo)node.Tag;
					CSharpToolBarButton button = new CSharpToolBarButton(board.Name);
					button.Tag = board;
					list.Add(button);
				}
				else if (node.Tag is BookmarkEntry)
				{
					BookmarkEntry entry = (BookmarkEntry)node.Tag;
					CSharpToolBarButton button = new CSharpToolBarButton(entry.Name);
					button.Tag = entry;
					list.Add(button);
				}
			}

			CSharpToolBarButton[] buttons = 
				(CSharpToolBarButton[])list.ToArray(typeof(CSharpToolBarButton));

			itabotan.Buttons.Clear();
			itabotan.Buttons.AddRange(buttons);
		}

		/// <summary>
		/// 新規カテゴリを作成
		/// </summary>
		/// <param name="name"></param>
		private TreeNode NewCategory(string name)
		{
			Category cate = new Category(name);
			return AddCategory(cate);
		}

		/// <summary>
		/// カテゴリを板ボタンに追加
		/// </summary>
		/// <param name="category"></param>
		private TreeNode AddCategory(Category category)
		{
			Category cate = new Category();
			cate.Children.AddRange(category.Children);
			cate.Name = category.Name;

			TreeNode node = new TreeNode(cate.Name);
			node.ImageIndex = node.SelectedImageIndex = 0;
			node.Tag = cate;

			foreach (BoardInfo board in cate.Children)
			{
				TreeNode child = new TreeNode(board.Name);
				child.ImageIndex = 2;
				child.SelectedImageIndex = 3;
				child.Tag = board;
				node.Nodes.Add(child);
			}

			treeViewItab.Nodes.Add(node);
			treeViewItab.SelectedNode = node;
			node.EnsureVisible();

			return node;
		}

		/// <summary>
		/// 板を板ボタンに追加
		/// </summary>
		/// <param name="board"></param>
		private void AddBoard(TreeNode parent, BoardInfo board)
		{
			// ノードが選択されていてそれがカテゴリであれば子として追加
			if (parent != null && parent.Tag is Category)
			{
				TreeNode child = new TreeNode(board.Name);
				child.ImageIndex = 2;
				child.SelectedImageIndex = 3;
				child.Tag = board;
				parent.Nodes.Add(child);

				treeViewItab.SelectedNode = parent;
				child.EnsureVisible();
			}
			// 単独の板ボタンとして追加
			else {
				TreeNode node = new TreeNode(board.Name);
				node.ImageIndex = 2;
				node.SelectedImageIndex = 3;
				node.Tag = board;

				treeViewItab.Nodes.Add(node);
				treeViewItab.SelectedNode = node;
				node.EnsureVisible();
			}
		}

		/// <summary>
		/// お気に入りを板ボタンに追加
		/// </summary>
		/// <param name="entry"></param>
		private void AddBookmarkEntry(BookmarkEntry entry)
		{
			// 単独の板ボタンとして追加
			TreeNode node = new TreeNode(entry.Name);
			node.ImageIndex = 2;
			node.SelectedImageIndex = 3;
			node.Tag = entry;

			treeViewItab.Nodes.Add(node);
			treeViewItab.SelectedNode = node;
			node.EnsureVisible();
		}

		private void treeViewSrc_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			buttonAdd.Enabled = (e.Node != null);
		}

		private void treeViewItab_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			buttonDel.Enabled = 
				buttonUp.Enabled =
				buttonDown.Enabled = (e.Node != null);

			buttonLabelEdit.Enabled =
				(treeViewItab.SelectedNode != null && treeViewItab.SelectedNode.Tag is Category);
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			contextMenu.Show(this,
				PointToClient(MousePosition));
		}

		private void buttonDel_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeViewItab.SelectedNode;
			node.Remove();
		}

		private void buttonUp_Click(object sender, System.EventArgs e)
		{
			TreeNode from = treeViewItab.SelectedNode;
			TreeNode to = from.PrevNode;

			if (to != null)
			{
				TreeNodeCollection collection =
					(from.Parent == null) ? treeViewItab.Nodes : from.Parent.Nodes;

				from.Remove();
				collection.Insert(to.Index, from);
				treeViewItab.SelectedNode = from;
			}
		}

		private void buttonDown_Click(object sender, System.EventArgs e)
		{
			TreeNode from = treeViewItab.SelectedNode;
			TreeNode to = from.NextNode;

			if (to != null)
			{
				TreeNodeCollection collection =
					(from.Parent == null) ? treeViewItab.Nodes : from.Parent.Nodes;

				from.Remove();
				collection.Insert(to.Index+1, from);
				treeViewItab.SelectedNode = from;
			}
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			SaveItaBotan();
			Close();
		}

		private void ItaBotanEditorDialog_Load(object sender, System.EventArgs e)
		{
			// 板一覧をツリービューに追加
			TreeViewUtility.SetTable(treeViewSrc, table, 0, 0, 2, 3);

			// 板ボタンをツリービューに追加
			foreach (CSharpToolBarButton but in itabotan.Buttons)
			{
				if (but.Tag is Category)	AddCategory((Category)but.Tag);
				else if (but.Tag is BoardInfo)	AddBoard(null, (BoardInfo)but.Tag);
				else if (but.Tag is BookmarkEntry) AddBookmarkEntry((BookmarkEntry)but.Tag);
			}
		}

		// 新規カテゴリを作成
		private void buttonNewCategory_Click(object sender, System.EventArgs e)
		{
			labelEditing = true;
			NewCategory("新規カテゴリ").BeginEdit();
		}

		private void treeView_AfterExpandCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			e.Node.ImageIndex =
				e.Node.SelectedImageIndex = e.Node.IsExpanded ? 1 : 0;
		}

		private void treeViewItab_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			e.CancelEdit = (e.Node.Tag is Category) && labelEditing ? false : true;
		}

		private void treeViewItab_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if (e.Label != null)
			{
				TreeNode node = e.Node;

				if (node.Tag is Category)
					((Category)node.Tag).Name = e.Label;
			}
			labelEditing = false;
		}

		// カテゴリ名を編集
		private void buttonLabelEdit_Click(object sender, System.EventArgs e)
		{
			labelEditing = true;
			treeViewItab.SelectedNode.BeginEdit();
		}

		// 選択されているカテゴリに板を追加
		private void menuItemAddFolder_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeViewSrc.SelectedNode;
			AddBoard(treeViewItab.SelectedNode, (BoardInfo)node.Tag);			
		}

		// 末尾にカテゴリまたは板を追加
		private void menuItemAppend_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeViewSrc.SelectedNode;

			if (node.Tag is Category) AddCategory((Category)node.Tag);
			else if (node.Tag is BoardInfo) AddBoard(null, (BoardInfo)node.Tag);
		}

		// 板一覧で板が選択されてなおかつ板ボタンのカテゴリが選択されているときのみ、
		// フォルダに追加メニューを有効にする。
		private void contextMenu_Popup(object sender, System.EventArgs e)
		{
			TreeNode node1 = treeViewSrc.SelectedNode;
			TreeNode node2 = treeViewItab.SelectedNode;

			menuItemAddFolder.Enabled =
				(node1 != null && node1.Tag is BoardInfo) &&
				(node2 != null && node2.Tag is Category);
		}
	}
}
