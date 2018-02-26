namespace Twin.Forms
{
	partial class CacheRemovingTool
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelCount = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(305, 43);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(83, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "中止";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(19, 49);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(226, 14);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 7;
			this.progressBar1.Value = 50;
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerReportsProgress = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(371, 12);
			this.label1.TabIndex = 8;
			this.label1.Text = "ログの移行処理を実行しています。完了すると自動的にtwintailを起動します。";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(146, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(47, 12);
			this.label2.TabIndex = 9;
			this.label2.Text = "ログ総数";
			// 
			// labelCount
			// 
			this.labelCount.Location = new System.Drawing.Point(199, 66);
			this.labelCount.Name = "labelCount";
			this.labelCount.Size = new System.Drawing.Size(46, 23);
			this.labelCount.TabIndex = 10;
			this.labelCount.Text = "0";
			this.labelCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CacheRemovingTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(413, 88);
			this.Controls.Add(this.labelCount);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CacheRemovingTool";
			this.Text = "ログの移行処理";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CacheRemovingTool_FormClosing);
			this.Load += new System.EventHandler(this.CacheRemovingTool_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelCount;
	}
}