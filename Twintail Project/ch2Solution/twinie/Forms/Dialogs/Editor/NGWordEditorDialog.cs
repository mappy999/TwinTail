// NGWordEditorDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Text;
	using System.IO;

	/// <summary>
	/// NGワードを編集するためのダイアログ
	/// </summary>
	public class NGWordEditorDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageName;
		private System.Windows.Forms.TabPage tabPageEmail;
		private System.Windows.Forms.TabPage tabPageID;
		private System.Windows.Forms.TextBox textBoxEmail;
		private System.Windows.Forms.TextBox textBoxBody;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.TextBox textBoxID;
		private System.Windows.Forms.TabPage tabPageLinks;
		private System.Windows.Forms.ListView listViewLinks;
		private System.Windows.Forms.Button buttonRegistLink;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader columnHeaderUri;
		private System.Windows.Forms.ColumnHeader columnHeaderLabel;
		private System.Windows.Forms.ContextMenu contextMenuLinks;
		private System.Windows.Forms.MenuItem menuItemLinkDel;
		private System.Windows.Forms.MenuItem menuItemLinkSelectAll;
		private System.Windows.Forms.TextBox textBoxLabel;
		private System.Windows.Forms.TextBox textBoxUri;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabPage tabPageThread;
		private System.Windows.Forms.TextBox textBoxSubject;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ToolBarButton toolBarButtonNew;
		private System.Windows.Forms.ToolBarButton toolBarButtonSave;
		private System.Windows.Forms.ToolBarButton toolBarButtonDel;
		private System.Windows.Forms.ToolBarButton toolBarButtonClose;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep;
		#endregion

		private LinkInfoCollection linkCollection;
		private IBoardTable table;
		private NGWords editing;
		private System.Windows.Forms.TabPage tabPageBody;		// 編集中のNGワード設定が格納される
		private bool modified;			// 設定が変更されたかどうかを表す

		/// <summary>
		/// NGWordEditorDialogクラスのインスタンスを初期化
		/// </summary>
		public NGWordEditorDialog(IBoardTable table)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.table = table;
			this.editing = null;

			// 重複を防ぐためのコレクション
			ArrayList exists = new ArrayList();

			// 存在するNGワード設定をすべて読み込みツリービューに反映
			TreeNode all = new TreeNode("すべてに適用");
			treeView.Nodes.Add(all);

			// すべてに適用を選択させる
			treeView.SelectedNode = all;

			// そのほかの個別設定
			foreach (BoardInfo board in table.ToArray())
			{
				if (Twinie.NGWords.Exists(board) && !exists.Contains(board))
				{
					TreeNode node = new TreeNode(board.Name);
					node.Tag = board;
					treeView.Nodes.Add(node);
					exists.Add(board);
				}
			}

			// NGアドレスを取得
			linkCollection = new LinkInfoCollection(Settings.NGAddrsPath);
		
			foreach (LinkInfo link in linkCollection)
				LinkInfoToListView(link);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NGWordEditorDialog));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageBody = new System.Windows.Forms.TabPage();
			this.textBoxBody = new System.Windows.Forms.TextBox();
			this.tabPageName = new System.Windows.Forms.TabPage();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.tabPageEmail = new System.Windows.Forms.TabPage();
			this.textBoxEmail = new System.Windows.Forms.TextBox();
			this.tabPageID = new System.Windows.Forms.TabPage();
			this.textBoxID = new System.Windows.Forms.TextBox();
			this.tabPageThread = new System.Windows.Forms.TabPage();
			this.textBoxSubject = new System.Windows.Forms.TextBox();
			this.tabPageLinks = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonRegistLink = new System.Windows.Forms.Button();
			this.textBoxLabel = new System.Windows.Forms.TextBox();
			this.textBoxUri = new System.Windows.Forms.TextBox();
			this.listViewLinks = new System.Windows.Forms.ListView();
			this.columnHeaderUri = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderLabel = new System.Windows.Forms.ColumnHeader();
			this.contextMenuLinks = new System.Windows.Forms.ContextMenu();
			this.menuItemLinkDel = new System.Windows.Forms.MenuItem();
			this.menuItemLinkSelectAll = new System.Windows.Forms.MenuItem();
			this.treeView = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonNew = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonDel = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonClose = new System.Windows.Forms.ToolBarButton();
			this.tabControl.SuspendLayout();
			this.tabPageBody.SuspendLayout();
			this.tabPageName.SuspendLayout();
			this.tabPageEmail.SuspendLayout();
			this.tabPageID.SuspendLayout();
			this.tabPageThread.SuspendLayout();
			this.tabPageLinks.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageBody);
			this.tabControl.Controls.Add(this.tabPageName);
			this.tabControl.Controls.Add(this.tabPageEmail);
			this.tabControl.Controls.Add(this.tabPageID);
			this.tabControl.Controls.Add(this.tabPageThread);
			this.tabControl.Controls.Add(this.tabPageLinks);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.ItemSize = new System.Drawing.Size(60, 18);
			this.tabControl.Location = new System.Drawing.Point(138, 26);
			this.tabControl.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(406, 308);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl.TabIndex = 0;
			// 
			// tabPageBody
			// 
			this.tabPageBody.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageBody.Controls.Add(this.textBoxBody);
			this.tabPageBody.Location = new System.Drawing.Point(4, 22);
			this.tabPageBody.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageBody.Name = "tabPageBody";
			this.tabPageBody.Size = new System.Drawing.Size(398, 282);
			this.tabPageBody.TabIndex = 0;
			this.tabPageBody.Text = "本文";
			// 
			// textBoxBody
			// 
			this.textBoxBody.AcceptsReturn = true;
			this.textBoxBody.AcceptsTab = true;
			this.textBoxBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxBody.Location = new System.Drawing.Point(0, 0);
			this.textBoxBody.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxBody.Multiline = true;
			this.textBoxBody.Name = "textBoxBody";
			this.textBoxBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxBody.Size = new System.Drawing.Size(398, 282);
			this.textBoxBody.TabIndex = 0;
			this.textBoxBody.WordWrap = false;
			this.textBoxBody.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// tabPageName
			// 
			this.tabPageName.Controls.Add(this.textBoxName);
			this.tabPageName.Location = new System.Drawing.Point(4, 22);
			this.tabPageName.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageName.Name = "tabPageName";
			this.tabPageName.Size = new System.Drawing.Size(398, 282);
			this.tabPageName.TabIndex = 1;
			this.tabPageName.Text = "名前";
			// 
			// textBoxName
			// 
			this.textBoxName.AcceptsReturn = true;
			this.textBoxName.AcceptsTab = true;
			this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxName.Location = new System.Drawing.Point(0, 0);
			this.textBoxName.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxName.Multiline = true;
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxName.Size = new System.Drawing.Size(398, 282);
			this.textBoxName.TabIndex = 1;
			this.textBoxName.WordWrap = false;
			this.textBoxName.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// tabPageEmail
			// 
			this.tabPageEmail.Controls.Add(this.textBoxEmail);
			this.tabPageEmail.Location = new System.Drawing.Point(4, 22);
			this.tabPageEmail.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageEmail.Name = "tabPageEmail";
			this.tabPageEmail.Size = new System.Drawing.Size(398, 282);
			this.tabPageEmail.TabIndex = 2;
			this.tabPageEmail.Text = "Email";
			// 
			// textBoxEmail
			// 
			this.textBoxEmail.AcceptsReturn = true;
			this.textBoxEmail.AcceptsTab = true;
			this.textBoxEmail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxEmail.Location = new System.Drawing.Point(0, 0);
			this.textBoxEmail.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxEmail.Multiline = true;
			this.textBoxEmail.Name = "textBoxEmail";
			this.textBoxEmail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxEmail.Size = new System.Drawing.Size(398, 282);
			this.textBoxEmail.TabIndex = 0;
			this.textBoxEmail.WordWrap = false;
			this.textBoxEmail.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// tabPageID
			// 
			this.tabPageID.Controls.Add(this.textBoxID);
			this.tabPageID.Location = new System.Drawing.Point(4, 22);
			this.tabPageID.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageID.Name = "tabPageID";
			this.tabPageID.Size = new System.Drawing.Size(398, 282);
			this.tabPageID.TabIndex = 3;
			this.tabPageID.Text = "ID";
			// 
			// textBoxID
			// 
			this.textBoxID.AcceptsReturn = true;
			this.textBoxID.AcceptsTab = true;
			this.textBoxID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxID.Location = new System.Drawing.Point(0, 0);
			this.textBoxID.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxID.Multiline = true;
			this.textBoxID.Name = "textBoxID";
			this.textBoxID.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxID.Size = new System.Drawing.Size(398, 282);
			this.textBoxID.TabIndex = 1;
			this.textBoxID.WordWrap = false;
			this.textBoxID.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// tabPageThread
			// 
			this.tabPageThread.Controls.Add(this.textBoxSubject);
			this.tabPageThread.Location = new System.Drawing.Point(4, 22);
			this.tabPageThread.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageThread.Name = "tabPageThread";
			this.tabPageThread.Size = new System.Drawing.Size(398, 282);
			this.tabPageThread.TabIndex = 5;
			this.tabPageThread.Text = "スレッド名";
			// 
			// textBoxSubject
			// 
			this.textBoxSubject.AcceptsReturn = true;
			this.textBoxSubject.AcceptsTab = true;
			this.textBoxSubject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxSubject.Location = new System.Drawing.Point(0, 0);
			this.textBoxSubject.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxSubject.Multiline = true;
			this.textBoxSubject.Name = "textBoxSubject";
			this.textBoxSubject.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxSubject.Size = new System.Drawing.Size(398, 282);
			this.textBoxSubject.TabIndex = 0;
			this.textBoxSubject.WordWrap = false;
			this.textBoxSubject.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// tabPageLinks
			// 
			this.tabPageLinks.Controls.Add(this.label2);
			this.tabPageLinks.Controls.Add(this.label1);
			this.tabPageLinks.Controls.Add(this.buttonRegistLink);
			this.tabPageLinks.Controls.Add(this.textBoxLabel);
			this.tabPageLinks.Controls.Add(this.textBoxUri);
			this.tabPageLinks.Controls.Add(this.listViewLinks);
			this.tabPageLinks.Location = new System.Drawing.Point(4, 22);
			this.tabPageLinks.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageLinks.Name = "tabPageLinks";
			this.tabPageLinks.Size = new System.Drawing.Size(398, 282);
			this.tabPageLinks.TabIndex = 4;
			this.tabPageLinks.Text = "アドレス";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(26, 211);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "説明文";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 187);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "登録するURL";
			// 
			// buttonRegistLink
			// 
			this.buttonRegistLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRegistLink.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRegistLink.Location = new System.Drawing.Point(286, 191);
			this.buttonRegistLink.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRegistLink.Name = "buttonRegistLink";
			this.buttonRegistLink.Size = new System.Drawing.Size(71, 28);
			this.buttonRegistLink.TabIndex = 3;
			this.buttonRegistLink.Text = "登録";
			this.buttonRegistLink.Click += new System.EventHandler(this.buttonRegistLink_Click);
			// 
			// textBoxLabel
			// 
			this.textBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLabel.Location = new System.Drawing.Point(84, 207);
			this.textBoxLabel.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxLabel.Name = "textBoxLabel";
			this.textBoxLabel.Size = new System.Drawing.Size(191, 19);
			this.textBoxLabel.TabIndex = 2;
			// 
			// textBoxUri
			// 
			this.textBoxUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUri.Location = new System.Drawing.Point(84, 183);
			this.textBoxUri.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxUri.Name = "textBoxUri";
			this.textBoxUri.Size = new System.Drawing.Size(191, 19);
			this.textBoxUri.TabIndex = 1;
			this.textBoxUri.Text = "http://";
			// 
			// listViewLinks
			// 
			this.listViewLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listViewLinks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUri,
            this.columnHeaderLabel});
			this.listViewLinks.ContextMenu = this.contextMenuLinks;
			this.listViewLinks.Location = new System.Drawing.Point(4, 4);
			this.listViewLinks.Margin = new System.Windows.Forms.Padding(2);
			this.listViewLinks.Name = "listViewLinks";
			this.listViewLinks.Size = new System.Drawing.Size(372, 172);
			this.listViewLinks.TabIndex = 0;
			this.listViewLinks.UseCompatibleStateImageBehavior = false;
			this.listViewLinks.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderUri
			// 
			this.columnHeaderUri.Text = "URL";
			this.columnHeaderUri.Width = 193;
			// 
			// columnHeaderLabel
			// 
			this.columnHeaderLabel.Text = "説明文";
			this.columnHeaderLabel.Width = 147;
			// 
			// contextMenuLinks
			// 
			this.contextMenuLinks.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLinkDel,
            this.menuItemLinkSelectAll});
			// 
			// menuItemLinkDel
			// 
			this.menuItemLinkDel.Index = 0;
			this.menuItemLinkDel.Text = "削除(&D)";
			this.menuItemLinkDel.Click += new System.EventHandler(this.menuItemLinkDel_Click);
			// 
			// menuItemLinkSelectAll
			// 
			this.menuItemLinkSelectAll.Index = 1;
			this.menuItemLinkSelectAll.Text = "すべて選択(&A)";
			this.menuItemLinkSelectAll.Click += new System.EventHandler(this.menuItemLinkSelectAll_Click);
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView.FullRowSelect = true;
			this.treeView.HideSelection = false;
			this.treeView.HotTracking = true;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new System.Drawing.Point(0, 26);
			this.treeView.Margin = new System.Windows.Forms.Padding(2);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 1;
			this.treeView.ShowLines = false;
			this.treeView.ShowPlusMinus = false;
			this.treeView.ShowRootLines = false;
			this.treeView.Size = new System.Drawing.Size(135, 308);
			this.treeView.TabIndex = 3;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			this.treeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeSelect);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			this.imageList.Images.SetKeyName(3, "");
			this.imageList.Images.SetKeyName(4, "");
			this.imageList.Images.SetKeyName(5, "");
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(135, 26);
			this.splitter1.Margin = new System.Windows.Forms.Padding(2);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 308);
			this.splitter1.TabIndex = 5;
			this.splitter1.TabStop = false;
			// 
			// toolBar
			// 
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonNew,
            this.toolBarButtonSave,
            this.toolBarButtonDel,
            this.toolBarButtonSep,
            this.toolBarButtonClose});
			this.toolBar.Divider = false;
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Margin = new System.Windows.Forms.Padding(2);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(544, 26);
			this.toolBar.TabIndex = 6;
			this.toolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// toolBarButtonNew
			// 
			this.toolBarButtonNew.ImageIndex = 2;
			this.toolBarButtonNew.Name = "toolBarButtonNew";
			this.toolBarButtonNew.Text = "新規作成";
			this.toolBarButtonNew.ToolTipText = "新しいNGワード設定を追加";
			// 
			// toolBarButtonSave
			// 
			this.toolBarButtonSave.ImageIndex = 3;
			this.toolBarButtonSave.Name = "toolBarButtonSave";
			this.toolBarButtonSave.Text = "保存";
			this.toolBarButtonSave.ToolTipText = "変更されたNGワード設定を保存";
			// 
			// toolBarButtonDel
			// 
			this.toolBarButtonDel.ImageIndex = 4;
			this.toolBarButtonDel.Name = "toolBarButtonDel";
			this.toolBarButtonDel.Text = "削除";
			this.toolBarButtonDel.ToolTipText = "選択されているNGワード設定を削除";
			// 
			// toolBarButtonSep
			// 
			this.toolBarButtonSep.Name = "toolBarButtonSep";
			this.toolBarButtonSep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonClose
			// 
			this.toolBarButtonClose.ImageIndex = 5;
			this.toolBarButtonClose.Name = "toolBarButtonClose";
			this.toolBarButtonClose.Text = "閉じる";
			this.toolBarButtonClose.ToolTipText = "NGワードを保存しウインドウを閉じる";
			// 
			// NGWordEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(544, 334);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.treeView);
			this.Controls.Add(this.toolBar);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NGWordEditorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NGワード編集";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.NGWordEditorDialog_Closing);
			this.Load += new System.EventHandler(this.NGWordEditorDialog_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPageBody.ResumeLayout(false);
			this.tabPageBody.PerformLayout();
			this.tabPageName.ResumeLayout(false);
			this.tabPageName.PerformLayout();
			this.tabPageEmail.ResumeLayout(false);
			this.tabPageEmail.PerformLayout();
			this.tabPageID.ResumeLayout(false);
			this.tabPageID.PerformLayout();
			this.tabPageThread.ResumeLayout(false);
			this.tabPageThread.PerformLayout();
			this.tabPageLinks.ResumeLayout(false);
			this.tabPageLinks.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 新しいNGワード設定を作成
		/// </summary>
		private void NewNGWordsSetting()
		{
			// NGワード設定を追加する板を選択させる
			BoardSelectDialog dlg = new BoardSelectDialog(table);
			dlg.Text = "NGワードを追加する板を選択";

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				BoardInfo board = dlg.Selected;
				TreeNode node = new TreeNode(board.Name);
				node.Tag = board;

				treeView.Nodes.Add(node);
				treeView.SelectedNode = node;

				node.EnsureVisible();
			}
		}

		/// <summary>
		/// すべてのNGワード設定をファイルに保存
		/// </summary>
		private void SaveSettings()
		{
			if (modified)
			{
				// 現在編集中の設定を取得
				UpdateNGWords(editing);

				// ディスクに保存
				Twinie.NGWords.Save();
				linkCollection.SaveToXml(Settings.NGAddrsPath);
			}
			modified = false;
		}

		private bool modifiedEvent = true;
		/// <summary>
		/// 指定したNGワード設定をテキストボックスに設定する
		/// </summary>
		private void SetNGWords(NGWords nGWords)
		{
			modifiedEvent = false;
			textBoxID.Lines = nGWords.ID.GetPatterns();
			textBoxBody.Lines = nGWords.Body.GetPatterns();
			textBoxName.Lines = nGWords.Name.GetPatterns();
			textBoxEmail.Lines = nGWords.Email.GetPatterns();
			textBoxSubject.Lines = nGWords.Subject.GetPatterns();
			modifiedEvent = true;
			editing = nGWords;
		}

		/// <summary>
		/// 現在のテキストボックスの状態をnGWordsに反映させる
		/// </summary>
		/// <param name="nGWords"></param>
		private void UpdateNGWords(NGWords nGWords)
		{
			if (modified)
			{
				nGWords.ID.SetRange(textBoxID.Lines);
				nGWords.Body.SetRange(textBoxBody.Lines);
				nGWords.Name.SetRange(textBoxName.Lines);
				nGWords.Email.SetRange(textBoxEmail.Lines);
				nGWords.Subject.SetRange(textBoxSubject.Lines);
			}
		}

		/// <summary>
		/// 選択されているNGワード設定を削除
		/// </summary>
		private void RemoveSelected()
		{
			TreeNode node = treeView.SelectedNode;
			if (node != null)
			{
				if (MessageBox.Show(node.Text + "板のNGワード設定を削除してもいいですか？", "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					BoardInfo board = (BoardInfo)node.Tag;
					modified = true;

					// デフォルトの設定は削除せずに空にするだけ
					if (board == null)
					{
						Twinie.NGWords.Default.Clear();
						SetNGWords(Twinie.NGWords.Default);
					}
					else {
						Twinie.NGWords.Remove(board);
						editing = null;

						treeView.SelectedNode = node.PrevNode;
						node.Remove();
					}
				}
			}
		}

		/// <summary>
		/// LinkInfoクラスをリストビューに追加
		/// </summary>
		/// <param name="info"></param>
		private void LinkInfoToListView(LinkInfo info)
		{
			ListViewItem item = new ListViewItem();
			item.Tag = info;
			item.Text = info.Uri;
			item.SubItems.Add(info.Text);

			listViewLinks.Items.Add(item);
		}

		private void textBox_TextChanged(object sender, System.EventArgs e)
		{
			if (modifiedEvent)
				modified = true;
		}

		private void NGWordEditorDialog_Load(object sender, System.EventArgs e)
		{
		}

		private void NGWordEditorDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (modified)
			{
				DialogResult r = MessageBox.Show("ファイルが変更されています。保存しますか？", "保存確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (r == DialogResult.Yes)
					SaveSettings();
			}
		}

		private void buttonRegistLink_Click(object sender, System.EventArgs e)
		{
			LinkInfo info = new LinkInfo(textBoxUri.Text, textBoxLabel.Text);
			LinkInfoToListView(info);
			linkCollection.Add(info);

			textBoxUri.Text = textBoxLabel.Text = String.Empty;
			modified = true;
		}

		private void menuItemLinkDel_Click(object sender, System.EventArgs e)
		{
			while (listViewLinks.SelectedItems.Count > 0)
			{
				ListViewItem item = listViewLinks.SelectedItems[0];
				LinkInfo link = (LinkInfo)item.Tag;
				linkCollection.Remove(link);
				item.Remove();
				modified = true;
			}
		}

		private void menuItemLinkSelectAll_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem item in listViewLinks.Items)
				item.Selected = true;
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			// 新しいNGワード設定を作成
			if (e.Button == toolBarButtonNew)
			{
				// 新規作成
				NewNGWordsSetting();
			}
			// 設定されているNGワード設定を保存
			if (e.Button == toolBarButtonSave)
			{
				SaveSettings();
			}
			// 選択されているNGワード設定を削除
			else if (e.Button == toolBarButtonDel)
			{
				RemoveSelected();
			}
			// 変更された設定を保存しウインドウを閉じる
			else if (e.Button == toolBarButtonClose)
			{
				SaveSettings();
				Close();
			}
		}

		// 選択が切り換える前に現在設定されているテキストボックスのNGワード設定を取得
		private void treeView_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if (editing != null)
				UpdateNGWords(editing);
		}

		// 選択されたNGワード設定をテキストボックスに設定
		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			BoardInfo board = (BoardInfo)e.Node.Tag;

			if (board == null)	SetNGWords(Twinie.NGWords.Default);
			else				SetNGWords(Twinie.NGWords.Get(board, true));

			Text = String.Format("NGワード編集 [{0}]",
				(board == null) ? "すべてに適用" : board.Name);
		}
	}
}
