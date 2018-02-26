// SplashForm.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Diagnostics;

	/// <summary>
	/// 簡単なスプラッシュウインドウ
	/// </summary>
	public class SplashWindow : System.Windows.Forms.Form
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Brush foreBrush;
		private StringAlignment alignment;
		private StringAlignment lineAlignment;
		private string message;
		private bool loaded;

		private string imagePath;
		private Image image;

		/// <summary>
		/// 表示する文字列を取得または設定
		/// </summary>
		public override string Text {
			set {
				if (value != null)
				{
					message = value;

					if (loaded) {
						Refresh();
						Application.DoEvents();
					}
				}
			}
			get {
				return message;
			}
		}

		/// <summary>
		/// 文字列の配置情報を取得または設定
		/// </summary>
		public StringAlignment TextAlignment {
			set {
				if (value != alignment)
					alignment = value;
			}
			get {
				return alignment;
			}
		}

		/// <summary>
		/// 文字列の行配置情報を取得または設定
		/// </summary>
		public StringAlignment LineAlignment {
			set {
				if (value != lineAlignment)
					lineAlignment = value;
			}
			get {
				return lineAlignment;
			}
		}

		/// <summary>
		/// 文字列の描画に使用するブラシを取得または設定
		/// </summary>
		public Brush ForeBrush {
			set {
				if (value != null)
					foreBrush = value;
			}
			get {
				return foreBrush;
			}
		}

		/// <summary>
		/// SplashWindowクラスのインスタンスを初期化
		/// </summary>
		/// <param name="imagePath">表示する画像ファイルへのパス</param>
		public SplashWindow(string imagePath)
		{
			if (imagePath == null) {
				throw new ArgumentNullException("imagePath");
			}

			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);

			this.message = String.Empty;
			this.foreBrush = Brushes.Black;
			this.alignment = StringAlignment.Far;
			this.lineAlignment = StringAlignment.Far;
			this.loaded = false;

			// 画像を読み込む
			try {
				this.imagePath = imagePath;
				this.image = new Bitmap(imagePath);
				this.Size = image.Size;
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
			}
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
			// 
			// SplashWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(100, 100);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashWindow";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SplashForm";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.SplashForm_Load);
			this.Closed += new System.EventHandler(this.SplashWindow_Closed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SplashWindow_Paint);

		}
		#endregion

		// Methods

		// Handlers
		private void SplashForm_Load(object sender, System.EventArgs e)
		{
			loaded = true;
		}

		private void SplashWindow_Closed(object sender, System.EventArgs e)
		{
			if (image != null)
				image.Dispose();
		}

		private void SplashWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			int x = (Width - ClientSize.Width) / 2;
			int y = (Height - ClientSize.Height) / 2;
			Rectangle rect = new Rectangle(new Point(x, y), ClientSize);

			if (image != null)
			{
				g.DrawImage(image, rect);
			}
		
			StringFormat format = StringFormat.GenericDefault;
			format.Alignment = alignment;
			format.LineAlignment = lineAlignment;

			g.DrawString(message, this.Font, foreBrush, rect, format);
		}
	}
}
