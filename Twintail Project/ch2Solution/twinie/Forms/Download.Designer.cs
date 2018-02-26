namespace Twin.Forms
{
	partial class Download
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
			this.buttonStop = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.labelUri = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonStop
			// 
			this.buttonStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonStop.Location = new System.Drawing.Point(312, 33);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(75, 23);
			this.buttonStop.TabIndex = 1;
			this.buttonStop.Text = "中止";
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 33);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(294, 20);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 3;
			// 
			// labelUri
			// 
			this.labelUri.Location = new System.Drawing.Point(12, 0);
			this.labelUri.Name = "labelUri";
			this.labelUri.Size = new System.Drawing.Size(375, 30);
			this.labelUri.TabIndex = 4;
			this.labelUri.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Download
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonStop;
			this.ClientSize = new System.Drawing.Size(392, 63);
			this.Controls.Add(this.labelUri);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.buttonStop);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Download";
			this.Text = "一括ダウンロード";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Download_FormClosing);
			this.Load += new System.EventHandler(this.Download_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label labelUri;
	}
}