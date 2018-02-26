namespace ImageViewerDll
{
	partial class ThumbnailViewer
	{
		/// <summary>
		/// 必要なデザイナー変数です。
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

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.webThumbnailsControl1 = new CSharpSamples.WebThumbnailsControl();
			this.SuspendLayout();
			// 
			// webThumbnailsControl1
			// 
			this.webThumbnailsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webThumbnailsControl1.Filter = CSharpSamples.ImageFilter.None;
			this.webThumbnailsControl1.ImageSize = new System.Drawing.Size(80, 80);
			this.webThumbnailsControl1.Location = new System.Drawing.Point(0, 0);
			this.webThumbnailsControl1.Name = "webThumbnailsControl1";
			this.webThumbnailsControl1.Size = new System.Drawing.Size(284, 262);
			this.webThumbnailsControl1.TabIndex = 0;
			this.webThumbnailsControl1.Text = "webThumbnailsControl1";
			// 
			// ThumbnailViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.webThumbnailsControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ThumbnailViewer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "サムネイルビューア";
			this.ResumeLayout(false);

		}

		#endregion

		private CSharpSamples.WebThumbnailsControl webThumbnailsControl1;

	}
}