// BoardPictureViewer.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using System.Text.RegularExpressions;
	using System.Net;
	using System.IO;
	using Twin.Tools;

	/// <summary>
	/// ２ちゃんねるの看板を表示するビューア
	/// </summary>
	public class BoardPictureViewer : System.Windows.Forms.Form
	{
		private delegate void SetImageMethodInvoker(Image image);

		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ImageViewerDll.ImagePanel imagePanel1;
		private X2chServerSetting x2chSettings;
		private Thread thread;
		private BoardInfo board;
		private bool gif;

		/// <summary>
		/// BoardPictureViewerクラスのインスタンスを初期化
		/// </summary>
		public BoardPictureViewer()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			x2chSettings = new X2chServerSetting();
			imagePanel1.Mosaic = false;
			gif = false;
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
			this.imagePanel1 = new ImageViewerDll.ImagePanel();
			this.SuspendLayout();
			// 
			// imagePanel1
			// 
			this.imagePanel1.BackColor = System.Drawing.Color.White;
			this.imagePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imagePanel1.IsOriginalSize = false;
			this.imagePanel1.Location = new System.Drawing.Point(0, 0);
			this.imagePanel1.Mosaic = false;
			this.imagePanel1.Name = "imagePanel1";
			this.imagePanel1.Size = new System.Drawing.Size(411, 176);
			this.imagePanel1.TabIndex = 0;
			this.imagePanel1.TabStop = false;
			this.imagePanel1.Text = "imagePanel1";
			// 
			// BoardPictureViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(411, 176);
			this.Controls.Add(this.imagePanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "BoardPictureViewer";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "看板 (画像をダウンロード中です...)";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BoardPictureViewer_Closing);
			this.Load += new System.EventHandler(this.BoardPictureViewer_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.BoardPictureViewer_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		private void Downloading()
		{
			Image image = null;
			try {
				x2chSettings.Download(board);
				WebClient webClient = new WebClient();
				byte[] data = webClient.DownloadData(x2chSettings.TitlePicture);

				MemoryStream memory = new MemoryStream();
				memory.Write(data, 0, data.Length);
				memory.Position = 0;

				gif = Regex.IsMatch(x2chSettings.TitlePicture, "\\.gif$",
					RegexOptions.IgnoreCase);

				image = Image.FromStream(memory);
			}
			catch (Exception ex)
			{
				image = new Bitmap(300, 50);
				StringFormat format = new StringFormat();

				using (Graphics g = Graphics.FromImage(image))
				{
					g.DrawString(ex.Message, this.Font, Brushes.Black, 
						new Rectangle(0, 0, image.Width, image.Height), format);
				}
			}
	
			Invoke(new SetImageMethodInvoker(SetImage), new object[] {image});
		}

		private void SetImage(Image image)
		{
			if (image == null)
				return;

			imagePanel1.Load(image);

			Screen sc = Screen.PrimaryScreen;
			Size imgSize = imagePanel1.Image.Size;
			
			this.Top = (sc.Bounds.Height / 2) - (imgSize.Height / 2);
			this.Left = (sc.Bounds.Width / 2) - (imgSize.Width / 2);
			this.ClientSize = imgSize;

			if (imagePanel1.CanAnimate) 
				imagePanel1.Animate();

			this.Text = "看板 (読み込み完了)";
		}

		private void BoardPictureViewer_Load(object sender, System.EventArgs e)
		{

		}

		private void BoardPictureViewer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (thread != null && thread.IsAlive)
				thread.Abort();

			imagePanel1.Unload();
		}

		private void BoardPictureViewer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		}

		/// <summary>
		/// コントロールを表示し、指定した看板を開く
		/// </summary>
		/// <param name="info"></param>
		public void Show(BoardInfo info)
		{
			Show();
			Open(info);
		}

		/// <summary>
		/// 指定した板の看板を開く
		/// </summary>
		public void Open(BoardInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			if (thread != null && thread.IsAlive)
				thread.Abort();

			this.imagePanel1.Unload();
			this.board = info;
			this.Text = String.Format("看板 [{0}板] ダウンロード中...", board.Name);

			thread = new Thread(new ThreadStart(Downloading));
			thread.Name = "BOARD_PIC_VIEW";
			thread.IsBackground = true;
			thread.Start();
		}

	}
}
