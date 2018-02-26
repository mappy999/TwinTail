namespace Twin.Forms
{
	partial class GroupEditorDialog
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupEditorDialog));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonNewGroup = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonGroupRename = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonGroupUp = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonGroupDown = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonGroupDel = new System.Windows.Forms.ToolStripButton();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonItemUp = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonItemDown = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonItemDel = new System.Windows.Forms.ToolStripButton();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileImportExport = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileImport = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemExport = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFileClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemGroupNew = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemGroupRename = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemGroupUp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemGroupDown = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemGroupSort = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemGroupSortAc = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemGroupSortDc = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemGroupDel = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemGroupClear = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThread = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadCopyName = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadCopyURLName = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadCopyURL = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadUp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadDown = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadDel = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 26);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
			this.splitContainer1.Size = new System.Drawing.Size(707, 437);
			this.splitContainer1.SplitterDistance = 201;
			this.splitContainer1.TabIndex = 0;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.FullRowSelect = true;
			this.treeView1.HideSelection = false;
			this.treeView1.ImageIndex = 0;
			this.treeView1.ImageList = this.imageList1;
			this.treeView1.Location = new System.Drawing.Point(0, 25);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = 1;
			this.treeView1.ShowLines = false;
			this.treeView1.ShowPlusMinus = false;
			this.treeView1.ShowRootLines = false;
			this.treeView1.Size = new System.Drawing.Size(201, 412);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList1.Images.SetKeyName(0, "フォルダ1.bmp");
			this.imageList1.Images.SetKeyName(1, "フォルダ2.bmp");
			this.imageList1.Images.SetKeyName(2, "板1.bmp");
			this.imageList1.Images.SetKeyName(3, "板2.bmp");
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewGroup,
            this.toolStripButtonGroupRename,
            this.toolStripSeparator2,
            this.toolStripButtonGroupUp,
            this.toolStripButtonGroupDown,
            this.toolStripSeparator1,
            this.toolStripButtonGroupDel});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(201, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButtonNewGroup
			// 
			this.toolStripButtonNewGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonNewGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNewGroup.Image")));
			this.toolStripButtonNewGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonNewGroup.Name = "toolStripButtonNewGroup";
			this.toolStripButtonNewGroup.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonNewGroup.Text = "新規グループ作成";
			this.toolStripButtonNewGroup.Click += new System.EventHandler(this.toolStripButtonNewGroup_Click);
			// 
			// toolStripButtonGroupRename
			// 
			this.toolStripButtonGroupRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonGroupRename.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGroupRename.Image")));
			this.toolStripButtonGroupRename.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonGroupRename.Name = "toolStripButtonGroupRename";
			this.toolStripButtonGroupRename.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonGroupRename.Text = "グループの名前を変更";
			this.toolStripButtonGroupRename.Click += new System.EventHandler(this.toolStripButtonGroupRename_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			this.toolStripSeparator2.Visible = false;
			// 
			// toolStripButtonGroupUp
			// 
			this.toolStripButtonGroupUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonGroupUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGroupUp.Image")));
			this.toolStripButtonGroupUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonGroupUp.Name = "toolStripButtonGroupUp";
			this.toolStripButtonGroupUp.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonGroupUp.Text = "一つ上に移動";
			this.toolStripButtonGroupUp.Visible = false;
			this.toolStripButtonGroupUp.Click += new System.EventHandler(this.toolStripButtonGroupUp_Click);
			// 
			// toolStripButtonGroupDown
			// 
			this.toolStripButtonGroupDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonGroupDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGroupDown.Image")));
			this.toolStripButtonGroupDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonGroupDown.Name = "toolStripButtonGroupDown";
			this.toolStripButtonGroupDown.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonGroupDown.Text = "一つ下に移動";
			this.toolStripButtonGroupDown.Visible = false;
			this.toolStripButtonGroupDown.Click += new System.EventHandler(this.toolStripButtonGroupDown_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonGroupDel
			// 
			this.toolStripButtonGroupDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonGroupDel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGroupDel.Image")));
			this.toolStripButtonGroupDel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonGroupDel.Name = "toolStripButtonGroupDel";
			this.toolStripButtonGroupDel.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonGroupDel.Text = "選択されたグループを削除";
			this.toolStripButtonGroupDel.Click += new System.EventHandler(this.toolStripButtonGroupDel_Click);
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.HideSelection = false;
			this.listView1.LargeImageList = this.imageList1;
			this.listView1.Location = new System.Drawing.Point(0, 25);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(502, 412);
			this.listView1.SmallImageList = this.imageList1;
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "スレッド名";
			this.columnHeader1.Width = 176;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "板名";
			this.columnHeader2.Width = 97;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "レス数";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "サイズ";
			this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "最終更新日";
			this.columnHeader5.Width = 112;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "最終書き込み日";
			this.columnHeader6.Width = 106;
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonItemUp,
            this.toolStripButtonItemDown,
            this.toolStripSeparator3,
            this.toolStripButtonItemDel});
			this.toolStrip2.Location = new System.Drawing.Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(502, 25);
			this.toolStrip2.TabIndex = 1;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// toolStripButtonItemUp
			// 
			this.toolStripButtonItemUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonItemUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonItemUp.Image")));
			this.toolStripButtonItemUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonItemUp.Name = "toolStripButtonItemUp";
			this.toolStripButtonItemUp.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonItemUp.Text = "項目を一つ上に移動";
			this.toolStripButtonItemUp.Click += new System.EventHandler(this.toolStripButtonItemUp_Click);
			// 
			// toolStripButtonItemDown
			// 
			this.toolStripButtonItemDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonItemDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonItemDown.Image")));
			this.toolStripButtonItemDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonItemDown.Name = "toolStripButtonItemDown";
			this.toolStripButtonItemDown.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonItemDown.Text = "項目を一つ下に移動";
			this.toolStripButtonItemDown.Click += new System.EventHandler(this.toolStripButtonItemDown_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonItemDel
			// 
			this.toolStripButtonItemDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonItemDel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonItemDel.Image")));
			this.toolStripButtonItemDel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonItemDel.Name = "toolStripButtonItemDel";
			this.toolStripButtonItemDel.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonItemDel.Text = "項目を削除";
			this.toolStripButtonItemDel.Click += new System.EventHandler(this.toolStripButtonItemDel_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemGroup,
            this.menuItemThread});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(707, 26);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuItemFile
			// 
			this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileImportExport,
            this.toolStripMenuItem4,
            this.menuItemFileClose});
			this.menuItemFile.Name = "menuItemFile";
			this.menuItemFile.Size = new System.Drawing.Size(85, 22);
			this.menuItemFile.Text = "ファイル(&F)";
			// 
			// menuItemFileImportExport
			// 
			this.menuItemFileImportExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileImport,
            this.menuItemExport});
			this.menuItemFileImportExport.Name = "menuItemFileImportExport";
			this.menuItemFileImportExport.Size = new System.Drawing.Size(220, 22);
			this.menuItemFileImportExport.Text = "インポートとエクスポート";
			this.menuItemFileImportExport.Visible = false;
			// 
			// menuItemFileImport
			// 
			this.menuItemFileImport.Name = "menuItemFileImport";
			this.menuItemFileImport.Size = new System.Drawing.Size(322, 22);
			this.menuItemFileImport.Text = "ファイルからグループ情報を読み込む(&L)...";
			this.menuItemFileImport.Click += new System.EventHandler(this.menuItemFileImport_Click);
			// 
			// menuItemExport
			// 
			this.menuItemExport.Name = "menuItemExport";
			this.menuItemExport.Size = new System.Drawing.Size(322, 22);
			this.menuItemExport.Text = "現在のグループ設定を別ファイルに保存(&S)...";
			this.menuItemExport.Click += new System.EventHandler(this.menuItemExport_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(217, 6);
			this.toolStripMenuItem4.Visible = false;
			// 
			// menuItemFileClose
			// 
			this.menuItemFileClose.Name = "menuItemFileClose";
			this.menuItemFileClose.Size = new System.Drawing.Size(220, 22);
			this.menuItemFileClose.Text = "終了(&X)";
			this.menuItemFileClose.Click += new System.EventHandler(this.menuItemFileClose_Click);
			// 
			// menuItemGroup
			// 
			this.menuItemGroup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGroupNew,
            this.toolStripMenuItem3,
            this.menuItemGroupRename,
            this.menuItemGroupUp,
            this.menuItemGroupDown,
            this.toolStripMenuItem2,
            this.menuItemGroupSort,
            this.toolStripMenuItem5,
            this.menuItemGroupDel,
            this.menuItemGroupClear});
			this.menuItemGroup.Name = "menuItemGroup";
			this.menuItemGroup.Size = new System.Drawing.Size(87, 22);
			this.menuItemGroup.Text = "グループ(&G)";
			this.menuItemGroup.DropDownOpening += new System.EventHandler(this.menuItemGroup_DropDownOpening);
			// 
			// menuItemGroupNew
			// 
			this.menuItemGroupNew.Name = "menuItemGroupNew";
			this.menuItemGroupNew.Size = new System.Drawing.Size(227, 22);
			this.menuItemGroupNew.Text = "新規グループを作成(&N)...";
			this.menuItemGroupNew.Click += new System.EventHandler(this.menuItemGroupNew_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemGroupRename
			// 
			this.menuItemGroupRename.Name = "menuItemGroupRename";
			this.menuItemGroupRename.Size = new System.Drawing.Size(227, 22);
			this.menuItemGroupRename.Text = "選択項目の名前を変更(&R)...";
			this.menuItemGroupRename.Click += new System.EventHandler(this.menuItemGroupRename_Click);
			// 
			// menuItemGroupUp
			// 
			this.menuItemGroupUp.Name = "menuItemGroupUp";
			this.menuItemGroupUp.Size = new System.Drawing.Size(227, 22);
			this.menuItemGroupUp.Text = "選択項目を一つ上に移動(&U)";
			this.menuItemGroupUp.Visible = false;
			this.menuItemGroupUp.Click += new System.EventHandler(this.menuItemGroupUp_Click);
			// 
			// menuItemGroupDown
			// 
			this.menuItemGroupDown.Name = "menuItemGroupDown";
			this.menuItemGroupDown.Size = new System.Drawing.Size(227, 22);
			this.menuItemGroupDown.Text = "選択項目を一つ下に移動(&D)";
			this.menuItemGroupDown.Visible = false;
			this.menuItemGroupDown.Click += new System.EventHandler(this.menuItemGroupDown_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemGroupSort
			// 
			this.menuItemGroupSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGroupSortAc,
            this.menuItemGroupSortDc});
			this.menuItemGroupSort.Name = "menuItemGroupSort";
			this.menuItemGroupSort.Size = new System.Drawing.Size(227, 22);
			this.menuItemGroupSort.Text = "名前順で並び替え(&S)";
			this.menuItemGroupSort.Visible = false;
			// 
			// menuItemGroupSortAc
			// 
			this.menuItemGroupSortAc.Name = "menuItemGroupSortAc";
			this.menuItemGroupSortAc.Size = new System.Drawing.Size(119, 22);
			this.menuItemGroupSortAc.Text = "昇順(&U)";
			this.menuItemGroupSortAc.Click += new System.EventHandler(this.menuItemGroupSortAc_Click);
			// 
			// menuItemGroupSortDc
			// 
			this.menuItemGroupSortDc.Name = "menuItemGroupSortDc";
			this.menuItemGroupSortDc.Size = new System.Drawing.Size(119, 22);
			this.menuItemGroupSortDc.Text = "降順(&D)";
			this.menuItemGroupSortDc.Click += new System.EventHandler(this.menuItemGroupSortDc_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(224, 6);
			this.toolStripMenuItem5.Visible = false;
			// 
			// menuItemGroupDel
			// 
			this.menuItemGroupDel.Name = "menuItemGroupDel";
			this.menuItemGroupDel.Size = new System.Drawing.Size(227, 22);
			this.menuItemGroupDel.Text = "選択グループを削除(&D)";
			this.menuItemGroupDel.Click += new System.EventHandler(this.menuItemGroupDel_Click);
			// 
			// menuItemGroupClear
			// 
			this.menuItemGroupClear.Name = "menuItemGroupClear";
			this.menuItemGroupClear.Size = new System.Drawing.Size(227, 22);
			this.menuItemGroupClear.Text = "すべてのグループを削除(&X)";
			this.menuItemGroupClear.Click += new System.EventHandler(this.menuItemGroupClear_Click);
			// 
			// menuItemThread
			// 
			this.menuItemThread.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemThreadCopyName,
            this.menuItemThreadCopyURLName,
            this.menuItemThreadCopyURL,
            this.toolStripMenuItem1,
            this.menuItemThreadUp,
            this.menuItemThreadDown,
            this.toolStripMenuItem6,
            this.menuItemThreadDel});
			this.menuItemThread.Name = "menuItemThread";
			this.menuItemThread.Size = new System.Drawing.Size(85, 22);
			this.menuItemThread.Text = "アイテム(&E)";
			this.menuItemThread.DropDownOpening += new System.EventHandler(this.menuItemThread_DropDownOpening);
			// 
			// menuItemThreadCopyName
			// 
			this.menuItemThreadCopyName.Name = "menuItemThreadCopyName";
			this.menuItemThreadCopyName.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadCopyName.Text = "スレッド名をコピー(&N)";
			this.menuItemThreadCopyName.Click += new System.EventHandler(this.menuItemThreadCopyName_Click);
			// 
			// menuItemThreadCopyURLName
			// 
			this.menuItemThreadCopyURLName.Name = "menuItemThreadCopyURLName";
			this.menuItemThreadCopyURLName.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadCopyURLName.Text = "スレッド名とURLをコピー(&B)";
			this.menuItemThreadCopyURLName.Click += new System.EventHandler(this.menuItemThreadCopyURLName_Click);
			// 
			// menuItemThreadCopyURL
			// 
			this.menuItemThreadCopyURL.Name = "menuItemThreadCopyURL";
			this.menuItemThreadCopyURL.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadCopyURL.Text = "URLをコピー(&U)";
			this.menuItemThreadCopyURL.Click += new System.EventHandler(this.menuItemThreadCopyURL_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemThreadUp
			// 
			this.menuItemThreadUp.Name = "menuItemThreadUp";
			this.menuItemThreadUp.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadUp.Text = "選択項目を一つ上に移動(&U)";
			this.menuItemThreadUp.Click += new System.EventHandler(this.menuItemThreadUp_Click);
			// 
			// menuItemThreadDown
			// 
			this.menuItemThreadDown.Name = "menuItemThreadDown";
			this.menuItemThreadDown.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadDown.Text = "選択項目を一つ下に移動(&D)";
			this.menuItemThreadDown.Click += new System.EventHandler(this.menuItemThreadDown_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemThreadDel
			// 
			this.menuItemThreadDel.Name = "menuItemThreadDel";
			this.menuItemThreadDel.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadDel.Text = "選択項目を削除(&D)";
			this.menuItemThreadDel.Click += new System.EventHandler(this.menuItemThreadDel_Click);
			// 
			// GroupEditorDialog
			// 
			this.ClientSize = new System.Drawing.Size(707, 463);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "GroupEditorDialog";
			this.Text = "グループの編集";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GroupEditorDialog_FormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButtonNewGroup;
		private System.Windows.Forms.ToolStripButton toolStripButtonGroupRename;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButtonGroupUp;
		private System.Windows.Forms.ToolStripButton toolStripButtonGroupDown;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButtonGroupDel;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton toolStripButtonItemUp;
		private System.Windows.Forms.ToolStripButton toolStripButtonItemDown;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripButtonItemDel;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuItemFile;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileImportExport;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileImport;
		private System.Windows.Forms.ToolStripMenuItem menuItemExport;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroup;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupNew;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupRename;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupUp;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupDown;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupSort;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupSortAc;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupSortDc;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupDel;
		private System.Windows.Forms.ToolStripMenuItem menuItemGroupClear;
		private System.Windows.Forms.ToolStripMenuItem menuItemThread;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadCopyName;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadCopyURLName;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadCopyURL;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadUp;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadDown;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadDel;
	}
}