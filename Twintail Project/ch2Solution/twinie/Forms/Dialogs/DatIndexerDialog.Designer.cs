namespace Twin
{
	partial class DatIndexerDialog
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
			this.buttonRef = new System.Windows.Forms.Button();
			this.buttonRun = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonAddList = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.textBoxDir = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxBoardPath = new System.Windows.Forms.TextBox();
			this.textBoxBoardName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.textBoxServer = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonRef
			// 
			this.buttonRef.Location = new System.Drawing.Point(319, 22);
			this.buttonRef.Name = "buttonRef";
			this.buttonRef.Size = new System.Drawing.Size(108, 23);
			this.buttonRef.TabIndex = 3;
			this.buttonRef.Text = "フォルダを選択...";
			this.buttonRef.UseVisualStyleBackColor = true;
			this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
			// 
			// buttonRun
			// 
			this.buttonRun.Location = new System.Drawing.Point(111, 353);
			this.buttonRun.Name = "buttonRun";
			this.buttonRun.Size = new System.Drawing.Size(108, 25);
			this.buttonRun.TabIndex = 11;
			this.buttonRun.Text = "生成開始";
			this.buttonRun.UseVisualStyleBackColor = true;
			this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
			// 
			// buttonStop
			// 
			this.buttonStop.Location = new System.Drawing.Point(227, 355);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(108, 23);
			this.buttonStop.TabIndex = 12;
			this.buttonStop.Text = "中止";
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// buttonAddList
			// 
			this.buttonAddList.Location = new System.Drawing.Point(143, 178);
			this.buttonAddList.Name = "buttonAddList";
			this.buttonAddList.Size = new System.Drawing.Size(160, 23);
			this.buttonAddList.TabIndex = 8;
			this.buttonAddList.Text = "↓リストに追加↓";
			this.buttonAddList.UseVisualStyleBackColor = true;
			this.buttonAddList.Click += new System.EventHandler(this.buttonAddList_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(14, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "変換元フォルダ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(14, 137);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(133, 12);
			this.label2.TabIndex = 6;
			this.label2.Text = "板のアドレス（例：news）";
			// 
			// listView1
			// 
			this.listView1.BackColor = System.Drawing.Color.LavenderBlush;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.listView1.Location = new System.Drawing.Point(16, 217);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(411, 107);
			this.listView1.TabIndex = 9;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "フォルダ名";
			this.columnHeader1.Width = 99;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "dat総数";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader2.Width = 76;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "板名";
			this.columnHeader3.Width = 93;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "アドレス";
			this.columnHeader4.Width = 79;
			// 
			// progressBar1
			// 
			this.progressBar1.BackColor = System.Drawing.Color.LavenderBlush;
			this.progressBar1.Cursor = System.Windows.Forms.Cursors.Help;
			this.progressBar1.ForeColor = System.Drawing.Color.LightPink;
			this.progressBar1.Location = new System.Drawing.Point(16, 330);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(411, 17);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 10;
			this.progressBar1.Value = 50;
			// 
			// textBoxDir
			// 
			this.textBoxDir.BackColor = System.Drawing.Color.Pink;
			this.textBoxDir.Location = new System.Drawing.Point(111, 51);
			this.textBoxDir.Name = "textBoxDir";
			this.textBoxDir.ReadOnly = true;
			this.textBoxDir.Size = new System.Drawing.Size(316, 19);
			this.textBoxDir.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(14, 19);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(301, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "2ch互換の*.datファイルのインデックス情報を生成します。";
			// 
			// textBoxBoardPath
			// 
			this.textBoxBoardPath.BackColor = System.Drawing.Color.LavenderBlush;
			this.textBoxBoardPath.Location = new System.Drawing.Point(227, 134);
			this.textBoxBoardPath.Name = "textBoxBoardPath";
			this.textBoxBoardPath.Size = new System.Drawing.Size(108, 19);
			this.textBoxBoardPath.TabIndex = 7;
			// 
			// textBoxBoardName
			// 
			this.textBoxBoardName.BackColor = System.Drawing.Color.LavenderBlush;
			this.textBoxBoardName.Location = new System.Drawing.Point(227, 82);
			this.textBoxBoardName.Name = "textBoxBoardName";
			this.textBoxBoardName.Size = new System.Drawing.Size(200, 19);
			this.textBoxBoardName.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(14, 85);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(131, 12);
			this.label4.TabIndex = 4;
			this.label4.Text = "板名（例：ニュース速報）";
			// 
			// folderBrowserDialog1
			// 
			this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerReportsProgress = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			// 
			// textBoxServer
			// 
			this.textBoxServer.BackColor = System.Drawing.Color.LavenderBlush;
			this.textBoxServer.Location = new System.Drawing.Point(227, 108);
			this.textBoxServer.Name = "textBoxServer";
			this.textBoxServer.Size = new System.Drawing.Size(200, 19);
			this.textBoxServer.TabIndex = 14;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label5.Location = new System.Drawing.Point(14, 111);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(202, 12);
			this.label5.TabIndex = 13;
			this.label5.Text = "サーバー名 (例：namidame.2ch.net)";
			// 
			// DatIndexerDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Pink;
			this.ClientSize = new System.Drawing.Size(447, 386);
			this.Controls.Add(this.textBoxServer);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxBoardName);
			this.Controls.Add(this.textBoxBoardPath);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxDir);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonAddList);
			this.Controls.Add(this.buttonStop);
			this.Controls.Add(this.buttonRun);
			this.Controls.Add(this.buttonRef);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DatIndexerDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "インデックス生成ツール";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DatIndexerDialog_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonRef;
		private System.Windows.Forms.Button buttonRun;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Button buttonAddList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TextBox textBoxDir;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxBoardPath;
		private System.Windows.Forms.TextBox textBoxBoardName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.TextBox textBoxServer;
		private System.Windows.Forms.Label label5;
	}
}