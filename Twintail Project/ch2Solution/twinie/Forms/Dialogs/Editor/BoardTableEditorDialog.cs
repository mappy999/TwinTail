// BoardTableEditorDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// 板一覧を編集するためのダイアログ
	/// </summary>
	public class BoardTableEditorDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Button buttonAddCategory;
		private System.Windows.Forms.TextBox textBoxCategory;
		private System.Windows.Forms.TextBox textBoxBoardName;
		private System.Windows.Forms.Button buttonAddBoard;
		private System.Windows.Forms.Button buttonRemoveSelected;
		private System.Windows.Forms.Button buttonClose;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button buttonOwCategory;
		private System.Windows.Forms.Button buttonOwBoard;
		private System.Windows.Forms.TextBox textBoxBoardServer;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxBoardPath;
		#endregion

		private IBoardTable table;
		private System.Windows.Forms.Label label5;
		private bool modified;

		/// <summary>
		/// BoardTableEditorDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="table">編集する板一覧テーブル</param>
		public BoardTableEditorDialog(IBoardTable table)
		{
			if (table == null) {
				throw new ArgumentNullException("table");
			}
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.table = table;
			this.modified = false;

			TreeViewUtility.SetTable(treeView, table, 0, 0, 2, 3);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardTableEditorDialog));
			this.treeView = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.buttonAddCategory = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxCategory = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxBoardName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxBoardServer = new System.Windows.Forms.TextBox();
			this.buttonAddBoard = new System.Windows.Forms.Button();
			this.buttonRemoveSelected = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonOwCategory = new System.Windows.Forms.Button();
			this.buttonOwBoard = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxBoardPath = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.treeView.FullRowSelect = true;
			this.treeView.HideSelection = false;
			this.treeView.HotTracking = true;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new System.Drawing.Point(8, 8);
			this.treeView.Margin = new System.Windows.Forms.Padding(2);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.ShowLines = false;
			this.treeView.Size = new System.Drawing.Size(174, 246);
			this.treeView.TabIndex = 0;
			this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
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
			// buttonAddCategory
			// 
			this.buttonAddCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddCategory.AutoSize = true;
			this.buttonAddCategory.Enabled = false;
			this.buttonAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAddCategory.Location = new System.Drawing.Point(344, 32);
			this.buttonAddCategory.Margin = new System.Windows.Forms.Padding(2);
			this.buttonAddCategory.Name = "buttonAddCategory";
			this.buttonAddCategory.Size = new System.Drawing.Size(88, 21);
			this.buttonAddCategory.TabIndex = 4;
			this.buttonAddCategory.Text = "カテゴリを追加";
			this.buttonAddCategory.Click += new System.EventHandler(this.buttonAddCategory_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(197, 12);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "カテゴリ名";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxCategory
			// 
			this.textBoxCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxCategory.Location = new System.Drawing.Point(260, 8);
			this.textBoxCategory.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxCategory.Name = "textBoxCategory";
			this.textBoxCategory.Size = new System.Drawing.Size(174, 19);
			this.textBoxCategory.TabIndex = 2;
			this.textBoxCategory.TextChanged += new System.EventHandler(this.textBoxCategory_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(206, 68);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "板名";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxBoardName
			// 
			this.textBoxBoardName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxBoardName.Location = new System.Drawing.Point(260, 64);
			this.textBoxBoardName.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxBoardName.Name = "textBoxBoardName";
			this.textBoxBoardName.Size = new System.Drawing.Size(174, 19);
			this.textBoxBoardName.TabIndex = 6;
			this.textBoxBoardName.TextChanged += new System.EventHandler(this.textBoxBoardName_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(189, 92);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "ホストアドレス";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxBoardServer
			// 
			this.textBoxBoardServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxBoardServer.Location = new System.Drawing.Point(260, 88);
			this.textBoxBoardServer.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxBoardServer.Name = "textBoxBoardServer";
			this.textBoxBoardServer.Size = new System.Drawing.Size(174, 19);
			this.textBoxBoardServer.TabIndex = 8;
			this.textBoxBoardServer.TextChanged += new System.EventHandler(this.textBoxBoardName_TextChanged);
			// 
			// buttonAddBoard
			// 
			this.buttonAddBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddBoard.AutoSize = true;
			this.buttonAddBoard.Enabled = false;
			this.buttonAddBoard.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAddBoard.Location = new System.Drawing.Point(344, 136);
			this.buttonAddBoard.Margin = new System.Windows.Forms.Padding(2);
			this.buttonAddBoard.Name = "buttonAddBoard";
			this.buttonAddBoard.Size = new System.Drawing.Size(88, 21);
			this.buttonAddBoard.TabIndex = 12;
			this.buttonAddBoard.Text = "板を追加";
			this.buttonAddBoard.Click += new System.EventHandler(this.buttonAddBoard_Click);
			// 
			// buttonRemoveSelected
			// 
			this.buttonRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRemoveSelected.AutoSize = true;
			this.buttonRemoveSelected.Enabled = false;
			this.buttonRemoveSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRemoveSelected.Location = new System.Drawing.Point(328, 160);
			this.buttonRemoveSelected.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRemoveSelected.Name = "buttonRemoveSelected";
			this.buttonRemoveSelected.Size = new System.Drawing.Size(105, 21);
			this.buttonRemoveSelected.TabIndex = 13;
			this.buttonRemoveSelected.Text = "選択項目を削除";
			this.buttonRemoveSelected.Click += new System.EventHandler(this.buttonRemoveSelected_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.AutoSize = true;
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClose.Location = new System.Drawing.Point(328, 245);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(105, 21);
			this.buttonClose.TabIndex = 14;
			this.buttonClose.Text = "閉じる";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonOwCategory
			// 
			this.buttonOwCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOwCategory.AutoSize = true;
			this.buttonOwCategory.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOwCategory.Location = new System.Drawing.Point(260, 32);
			this.buttonOwCategory.Margin = new System.Windows.Forms.Padding(2);
			this.buttonOwCategory.Name = "buttonOwCategory";
			this.buttonOwCategory.Size = new System.Drawing.Size(80, 21);
			this.buttonOwCategory.TabIndex = 3;
			this.buttonOwCategory.Text = "上書き";
			this.buttonOwCategory.Click += new System.EventHandler(this.buttonOwCategory_Click);
			// 
			// buttonOwBoard
			// 
			this.buttonOwBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOwBoard.AutoSize = true;
			this.buttonOwBoard.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOwBoard.Location = new System.Drawing.Point(260, 136);
			this.buttonOwBoard.Margin = new System.Windows.Forms.Padding(2);
			this.buttonOwBoard.Name = "buttonOwBoard";
			this.buttonOwBoard.Size = new System.Drawing.Size(80, 21);
			this.buttonOwBoard.TabIndex = 11;
			this.buttonOwBoard.Text = "上書き";
			this.buttonOwBoard.Click += new System.EventHandler(this.buttonOwBoard_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(189, 116);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 12);
			this.label4.TabIndex = 9;
			this.label4.Text = "板のアドレス";
			// 
			// textBoxBoardPath
			// 
			this.textBoxBoardPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxBoardPath.Location = new System.Drawing.Point(260, 112);
			this.textBoxBoardPath.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxBoardPath.Name = "textBoxBoardPath";
			this.textBoxBoardPath.Size = new System.Drawing.Size(90, 19);
			this.textBoxBoardPath.TabIndex = 10;
			this.textBoxBoardPath.TextChanged += new System.EventHandler(this.textBoxBoardName_TextChanged);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.AutoSize = true;
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(9, 256);
			this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(185, 12);
			this.label5.TabIndex = 15;
			this.label5.Text = "項目をダブルクリックすると編集できます";
			// 
			// BoardTableEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 271);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBoxBoardPath);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.buttonOwBoard);
			this.Controls.Add(this.buttonOwCategory);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.buttonRemoveSelected);
			this.Controls.Add(this.buttonAddBoard);
			this.Controls.Add(this.textBoxBoardServer);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxBoardName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxCategory);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonAddCategory);
			this.Controls.Add(this.treeView);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BoardTableEditorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "板一覧の編集";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BoardTableEditorDialog_Closing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 板の状態を保存
		/// </summary>
		private void SaveBoards()
		{
			table.Items.Clear();

			foreach (TreeNode node in treeView.Nodes)
			{
				Category cate = (Category)node.Tag;
				cate.Children.Clear();

				foreach (TreeNode child in node.Nodes)
				{
					BoardInfo board = (BoardInfo)child.Tag;
					cate.Children.Add(board);
				}

				table.Items.Add(cate);
			}
		}

		/// <summary>
		/// ボタンの有効状態を更新
		/// </summary>
		private void UpdateButtonEnable()
		{
			bool sel = (treeView.SelectedNode != null);

			bool e = (textBoxBoardName.Text != String.Empty && 
				textBoxBoardServer.Text != String.Empty &&
				textBoxBoardPath.Text != String.Empty);

			TreeNode node = treeView.SelectedNode;

			// カテゴリを追加ボタンの有効状態を設定
			buttonAddCategory.Enabled = 
				(textBoxCategory.Text != String.Empty);

			// 板を追加ボタンの有効状態を設定
			buttonAddBoard.Enabled =
				(sel && node.Tag is Category) && e;

			// 選択項目を削除ボタン
			buttonRemoveSelected.Enabled = sel ? true : false;

			// 上書きボタン
			buttonOwBoard.Enabled = sel ? (node.Tag is BoardInfo) && e : false;
			buttonOwCategory.Enabled = sel ? (node.Tag is Category) && (textBoxCategory.Text != String.Empty) : false;
		}

		// カテゴリを追加
		private void buttonAddCategory_Click(object sender, System.EventArgs e)
		{
			TreeNode node = new TreeNode(textBoxCategory.Text);
			node.Tag = new Category(textBoxCategory.Text);
			node.ImageIndex = node.SelectedImageIndex = 0;

			treeView.Nodes.Add(node);
			treeView.SelectedNode = node;

			textBoxCategory.Text = String.Empty;
			modified = true;
		}

		private void buttonAddBoard_Click(object sender, System.EventArgs e)
		{
			TreeNode node = new TreeNode(textBoxBoardName.Text);
			node.ImageIndex = node.SelectedImageIndex = 2;
			node.Tag = new BoardInfo(textBoxBoardServer.Text, textBoxBoardPath.Text, textBoxBoardName.Text);

			TreeNode selected = treeView.SelectedNode;
			selected.Nodes.Add(node);

			treeView.SelectedNode = selected;
			modified = true;

			textBoxBoardName.Text =
				textBoxBoardServer.Text = textBoxBoardPath.Text = String.Empty;
		}

		private void buttonRemoveSelected_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.SelectedNode;
			node.Remove();
			modified = true;
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			UpdateButtonEnable();
		}

		private void treeView_AfterExpandCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Node.Tag is Category)
			{
				e.Node.ImageIndex = e.Node.SelectedImageIndex = 
					(e.Node.IsExpanded) ? 1 : 0;
			}
		}

		private void textBoxCategory_TextChanged(object sender, System.EventArgs e)
		{
			UpdateButtonEnable();
		}

		private void textBoxBoardName_TextChanged(object sender, System.EventArgs e)
		{
			UpdateButtonEnable();
		}

		private void BoardTableEditorDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (modified)
				SaveBoards();
		}

		private void treeView_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.GetNodeAt(
								treeView.PointToClient(MousePosition));

			if (node != null)
			{
				if (node.Tag is BoardInfo)
				{
					BoardInfo board = (BoardInfo)node.Tag;
					textBoxBoardName.Text = board.Name;
					textBoxBoardServer.Text = board.Server;
					textBoxBoardPath.Text = board.Path;
				}
				else if (node.Tag is Category)
				{
					Category cate = (Category)node.Tag;
					textBoxCategory.Text = cate.Name;
				}
			}
		}

		private void buttonOwCategory_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.SelectedNode;
			if (node != null && node.Tag is Category)
			{
				Category cate = (Category)node.Tag;
				cate.Name = node.Text = textBoxCategory.Text;
				modified = true;

				textBoxCategory.Text = String.Empty;
			}
		}

		private void buttonOwBoard_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.SelectedNode;
			if (node != null && node.Tag is BoardInfo)
			{
				BoardInfo board = new BoardInfo(textBoxBoardServer.Text, textBoxBoardPath.Text, textBoxBoardName.Text);
				node.Tag = board;
				node.Text = board.Name;
				modified = true;

				textBoxBoardName.Text =
					textBoxBoardPath.Text =
					textBoxBoardServer.Text = String.Empty;
			}		
		}
	}
}
