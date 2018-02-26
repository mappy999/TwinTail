namespace Twin
{
	partial class ColorWordEditDialog
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeaderWord = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderIsRegex = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderForeColor = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderBackColor = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderIsBold = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderIsItalic = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderPlaySound = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderIsPopup = new System.Windows.Forms.ColumnHeader();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditNew = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditSelection = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemEditDel = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderWord,
            this.columnHeaderIsRegex,
            this.columnHeaderForeColor,
            this.columnHeaderBackColor,
            this.columnHeaderIsBold,
            this.columnHeaderIsItalic,
            this.columnHeaderPlaySound,
            this.columnHeaderIsPopup});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.Location = new System.Drawing.Point(0, 24);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(703, 309);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
			// 
			// columnHeaderWord
			// 
			this.columnHeaderWord.Text = "単語";
			this.columnHeaderWord.Width = 136;
			// 
			// columnHeaderIsRegex
			// 
			this.columnHeaderIsRegex.Text = "正規表現";
			// 
			// columnHeaderForeColor
			// 
			this.columnHeaderForeColor.Text = "文字色";
			// 
			// columnHeaderBackColor
			// 
			this.columnHeaderBackColor.Text = "背景色";
			// 
			// columnHeaderIsBold
			// 
			this.columnHeaderIsBold.Text = "太字";
			this.columnHeaderIsBold.Width = 43;
			// 
			// columnHeaderIsItalic
			// 
			this.columnHeaderIsItalic.Text = "斜体";
			this.columnHeaderIsItalic.Width = 44;
			// 
			// columnHeaderPlaySound
			// 
			this.columnHeaderPlaySound.DisplayIndex = 7;
			this.columnHeaderPlaySound.Text = "サウンド";
			this.columnHeaderPlaySound.Width = 206;
			// 
			// columnHeaderIsPopup
			// 
			this.columnHeaderIsPopup.DisplayIndex = 6;
			this.columnHeaderIsPopup.Text = "ポップアップ";
			this.columnHeaderIsPopup.Width = 83;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemEdit});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(703, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuItemFile
			// 
			this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileExit});
			this.menuItemFile.Name = "menuItemFile";
			this.menuItemFile.Size = new System.Drawing.Size(66, 20);
			this.menuItemFile.Text = "ファイル(&F)";
			// 
			// menuItemFileExit
			// 
			this.menuItemFileExit.Name = "menuItemFileExit";
			this.menuItemFileExit.Size = new System.Drawing.Size(114, 22);
			this.menuItemFileExit.Text = "終了(&X)";
			this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEditNew,
            this.menuItemEditSelection,
            this.toolStripMenuItem1,
            this.menuItemEditDel});
			this.menuItemEdit.Name = "menuItemEdit";
			this.menuItemEdit.Size = new System.Drawing.Size(56, 20);
			this.menuItemEdit.Text = "編集(&E)";
			this.menuItemEdit.DropDownOpening += new System.EventHandler(this.menuItemEdit_DropDownOpening);
			// 
			// menuItemEditNew
			// 
			this.menuItemEditNew.Name = "menuItemEditNew";
			this.menuItemEditNew.Size = new System.Drawing.Size(177, 22);
			this.menuItemEditNew.Text = "新規登録(&A)...";
			this.menuItemEditNew.Click += new System.EventHandler(this.menuItemEditNew_Click);
			// 
			// menuItemEditSelection
			// 
			this.menuItemEditSelection.Name = "menuItemEditSelection";
			this.menuItemEditSelection.Size = new System.Drawing.Size(177, 22);
			this.menuItemEditSelection.Text = "選択項目を編集(&E)...";
			this.menuItemEditSelection.Click += new System.EventHandler(this.menuItemEditSelection_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(174, 6);
			// 
			// menuItemEditDel
			// 
			this.menuItemEditDel.Name = "menuItemEditDel";
			this.menuItemEditDel.Size = new System.Drawing.Size(177, 22);
			this.menuItemEditDel.Text = "選択項目を削除(&D)";
			this.menuItemEditDel.Click += new System.EventHandler(this.menuItemEditDel_Click);
			// 
			// ColorWordEditDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 333);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorWordEditDialog";
			this.Text = "単語の強調表示リスト";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeaderWord;
		private System.Windows.Forms.ColumnHeader columnHeaderIsRegex;
		private System.Windows.Forms.ColumnHeader columnHeaderForeColor;
		private System.Windows.Forms.ColumnHeader columnHeaderBackColor;
		private System.Windows.Forms.ColumnHeader columnHeaderIsBold;
		private System.Windows.Forms.ColumnHeader columnHeaderIsItalic;
		private System.Windows.Forms.ColumnHeader columnHeaderPlaySound;
		private System.Windows.Forms.ColumnHeader columnHeaderIsPopup;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuItemFile;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileExit;
		private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditNew;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditSelection;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditDel;
	}
}