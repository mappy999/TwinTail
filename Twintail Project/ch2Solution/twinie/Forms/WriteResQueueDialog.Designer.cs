namespace Twin.Forms
{
	partial class WriteResQueueDialog
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuStripItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStripItemCancel = new System.Windows.Forms.ToolStripMenuItem();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.menuStripItemView = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
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
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.Location = new System.Drawing.Point(0, 24);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(455, 188);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(455, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuStripItem
			// 
			this.menuStripItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripItemView,
            this.toolStripMenuItem1,
            this.menuStripItemCancel});
			this.menuStripItem.Name = "menuStripItem";
			this.menuStripItem.Size = new System.Drawing.Size(59, 20);
			this.menuStripItem.Text = "項目(&M)";
			this.menuStripItem.DropDownOpening += new System.EventHandler(this.menuStripItem_DropDownOpening);
			// 
			// menuStripItemCancel
			// 
			this.menuStripItemCancel.Name = "menuStripItemCancel";
			this.menuStripItemCancel.Size = new System.Drawing.Size(251, 22);
			this.menuStripItemCancel.Text = "選択されている投稿をキャンセル(&C)";
			this.menuStripItemCancel.Click += new System.EventHandler(this.menuStripItemCancel_Click);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "板名";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "名前";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "メール欄";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "本文";
			this.columnHeader4.Width = 133;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Samba値";
			this.columnHeader5.Width = 59;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "状態";
			// 
			// menuStripItemView
			// 
			this.menuStripItemView.Name = "menuStripItemView";
			this.menuStripItemView.Size = new System.Drawing.Size(251, 22);
			this.menuStripItemView.Text = "表示(&V)...";
			this.menuStripItemView.Click += new System.EventHandler(this.menuStripItemView_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(248, 6);
			// 
			// WriteResQueueDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(455, 212);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "WriteResQueueDialog";
			this.Text = "待機中の書き込みリスト";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WriteResQueueDialog_FormClosing);
			this.Load += new System.EventHandler(this.WriteResQueueDialog_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuStripItem;
		private System.Windows.Forms.ToolStripMenuItem menuStripItemView;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem menuStripItemCancel;
	}
}