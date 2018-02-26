// ImagePanel.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Windows.Forms;
	using System.Threading;
	using System.Net;
	using CSharpSamples;
	using System.Drawing.Imaging;	// FrameDimensionのために追加

	/// <summary>
	/// 画像を表示するパネル
	/// </summary>
//	public class ImagePanel : Panel
	public class ImagePanel : ScrollableControl	// 少しは軽くなると思う
	{
		private Image imageSrc;
		private Image buffer;
		private bool mosaic;
		private bool viewOriginalSize;

//		private bool animation = false;	// ロジックを変えたので不要に
		private bool stopped = true;
		private AniGIF mAgif;			// アニメGIFを実行するインスタンス

		/// <summary>
		/// 表示されている画像を取得
		/// </summary>
		public Image Image {
			get {
				return imageSrc;
			}
		}

		/// <summary>
		/// 画像が読み込まれているかどうかを示す値を取得
		/// </summary>
		public bool IsLoaded {
			get {
				return (imageSrc != null) ? true : false;
			}
		}

		/// <summary>
		/// 画像を原寸大で表示するかどうかを示す値を取得または設定
		/// </summary>
		public bool IsOriginalSize
		{
			set {
				if (viewOriginalSize != value)
				{
					viewOriginalSize = value;
					UpdateAutoScrollMode();

					buffer = null;
					Invalidate();
				}
			}
			get {
				return viewOriginalSize;
			}
		}

		/// <summary>
		/// モザイク表示するかどうかを取得または設定
		/// </summary>
		public bool Mosaic {
			set {
				if (mosaic != value)
				{
					mosaic = value;
					buffer = null;
					Invalidate();
				}
			}
			get { return mosaic; }
		}

		/// <summary>
		/// アニメーション可能かどうかを判断
		/// </summary>
		public bool CanAnimate {
			get {
				return IsLoaded && AniGIF.CanAnimate(imageSrc);	// ImageAnimatorでもいいけど
			}
		}

		public bool AnimationStopped
		{
			get
			{
				return this.stopped;
			}
		}

		/// <summary>
		/// ImagePanelクラスのインスタンスを初期化
		/// </summary>
		public ImagePanel()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);

			TabStop = false;//これしないと画像を開くたびになぜかビューアがアクティブになってしまう
			Dock = DockStyle.Fill;
			BackColor = Color.White;

			imageSrc = null;
			buffer = null;
			viewOriginalSize = false;
			mosaic = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Unload();
			}
			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (ClientSize.Width < 10 || ClientSize.Height < 10)
				return;

			Graphics g = e.Graphics;

			if (IsLoaded)
			{
				Image image = imageSrc;
				Size border = SystemInformation.Border3DSize;
				Size newSize;
				
				if (viewOriginalSize)
				{
					newSize = image.Size;
				}
				else
				{
					// 縦横比が固定された画像サイズを取得
					newSize = ImageUtil.GetThumbnailSize(image,
						new Size(Width - border.Width * 2, Height - border.Height * 2));
				}

				if (mosaic)
				{
					if (buffer == null)
					{
						buffer = new Bitmap(imageSrc, newSize);
						BitmapFilter.Pixelate((Bitmap)buffer, 8, false);
					}
					image = buffer;
				}

				if (viewOriginalSize)
				{
					g.DrawImage(image, AutoScrollPosition.X, AutoScrollPosition.Y,
						newSize.Width, newSize.Height);
				}
				else
				{
					g.DrawImage(image, border.Width, border.Height, newSize.Width, newSize.Height);
					
					ControlPaint.DrawBorder3D(g, 
						new Rectangle(0, 0, Width, Height), Border3DStyle.Sunken);
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			buffer = null;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (!IsLoaded)
				return;

			if (e.Button == System.Windows.Forms.MouseButtons.Middle)
			{
				Mosaic = !Mosaic;
			}
			else if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks > 1)
			{
				IsOriginalSize = !IsOriginalSize;
			}
		}

		/// <summary>
		/// 画像を読み込み表示する
		/// </summary>
		/// <param name="filename"></param>
		public void Load(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");

//			StopAnimation();	// 止めなくても大丈夫のようだ

			Unload();
			imageSrc = new Bitmap( filename );

			UpdateAutoScrollMode();
			Invalidate();
		}

		/// <summary>
		/// 画像を読み込み表示する
		/// </summary>
		/// <param name="image"></param>
		public void Load(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image");

//			StopAnimation();	// 止めなくても大丈夫のようだ

			Unload();
			imageSrc = image;

			UpdateAutoScrollMode();
			Invalidate();
		}

		/// <summary>
		/// 現在表示されている画像を解放する
		/// </summary>
		public void Unload()
		{
			if (imageSrc != null)
				imageSrc.Dispose();
			imageSrc = null;

			if (mAgif != null)
				mAgif.Dispose();
			mAgif = null;

			if (buffer != null)
				buffer.Dispose();
			buffer = null;

			Invalidate();
		}

		/// <summary>
		/// アニメーションを開始
		/// </summary>
		public void Animate()
		{
			if (imageSrc == null)
				throw new InvalidOperationException("イメージが読み込まれていません");

			if (AniGIF.CanAnimate(imageSrc))
			{
				if ( mAgif == null )
				{
					mAgif = new AniGIF( imageSrc , new EventHandler( OnAnimateFrameChanged ) );
				}
				mAgif.Animating = !mAgif.Animating;	// メニュー「アニメーションを開始」でトグルするようにしてみた
				this.stopped = mAgif.Animating;
			}
		}

		public void StopAnimation()
		{
			if ( mAgif != null )
			{
				mAgif.Animating = false;
				this.stopped = mAgif.Animating;
			}
		}

		private void OnAnimateFrameChanged(object sender, EventArgs e)
		{
			if ( mAgif != null )
			{
				mAgif.Index++;	// デクリメント演算子にすると逆再生もできる

				if (Mosaic)
				{
					buffer.Dispose();
					buffer = null;
				}

				Invalidate();
			}
		}

		private void UpdateAutoScrollMode()
		{
			base.AutoScroll = viewOriginalSize;

			if (viewOriginalSize)
			{
				if (imageSrc != null)
				{
					base.AutoScrollMinSize = imageSrc.Size;
				}
			}
		}
	}

	/// <summary>
	/// 自前ImageAnimator
	/// </summary>
	public class AniGIF : IDisposable
	{
		#region プライベートメンバー

		private Image mImage;						// アニメGIF保持
		private int[] mIntervals;					// フレームインタバル、msec
		private System.Windows.Forms.Timer mTimer;	// アニメをするためのタイマー、UIスレッドなのがキモ
		private bool mDisposed;

		#endregion プライベートメンバー

		#region プロパティ

		/// <summary>
		/// インスタンスがアニメーション可能か
		/// </summary>
		public bool Animatable
		{
			get
			{
				return _Frames > 1;
			}
		}

		/// <summary>
		/// アニメーション実行中か
		/// 実行の開始･停止もできる
		/// </summary>
		public bool Animating
		{
			get
			{
				return _Animating;
			}
			set
			{
				if (!Animatable)
				{
					return;
				}
				if (value == _Animating)
				{
					return;
				}
				ThrowExceptionIfDisposed();
				if (value)
				{
					StartTimer();
				}
				else
				{
					mTimer.Enabled = false;
				}
				_Animating = value;
			}
		}
		private bool _Animating;

		/// <summary>
		/// アニメGIF内フレーム数
		/// </summary>
		public int Frames
		{
			get
			{
				return _Frames;
			}
		}
		private int _Frames;

		/// <summary>
		/// 現在アクティブなアニメGIF内フレームのインデックス
		/// 選択もできる
		/// </summary>
		public int Index
		{
			get
			{
				return _Index;
			}
			set
			{
				if (!Animatable)
				{
					return;
				}
				if (value < 0)
				{
					value = _Frames - 1;
				}
				if (value >= _Frames)
				{
					value = 0;
				}
				_Index = value;
				if (mImage != null)
				{
					try
					{
						mImage.SelectActiveFrame(FrameDimension.Time, _Index);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}
				}
			}
		}
		private int _Index;

		/// <summary>
		/// ＩＥやFireFoxではフレームインターバルの値が５以下だと
		/// 自動的に10、つまり100msecインターバールにしてしまうようだ
		/// そしてそのことを当て込んでインターバール値が変なGIFもいっぱいあるので
		/// 切り替えられるようにしてみた
		///というのはちゃんと小さな値を期待しているＧＩＦもいっぱいあるので。
		/// </summary>
		public bool BrowserCompatible
		{
			get
			{
				return _BrowserCompatible;
			}
			set
			{
				_BrowserCompatible = value;
			}
		}
		private bool _BrowserCompatible = true;

		#endregion プロパティ

		#region イベント

		/// <summary>
		/// フレーム変化タイミングで呼び出すイベントハンドラ
		/// フレームを自動的に進めたりはしないのでイベントハンドラ側ですべし
		/// </summary>
		public event EventHandler FrameChanging;

		#endregion イベント

		#region 初期化関係

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public AniGIF(Image img, EventHandler evh)
		{
			mImage = img;
			FrameChanging += evh;

			try
			{
				_Frames = img.GetFrameCount(FrameDimension.Time);
			}
			catch (Exception)
			{
				_Frames = 0;
			}

			if (!Animatable)
			{
				return;
			}

			//
			// GIF内インターバル値を取り出してタイマー時間配列に格納
			//
			mIntervals = new int[_Frames];
			PropertyItem p = img.GetPropertyItem(20736);	// このプロパティがフレームインターバル
			byte[] b = (byte[])p.Value;

			for (int i = 0; i < _Frames; i++)
			{
				try
				{
					Int32 v = BitConverter.ToInt32(b, i * 4);
					mIntervals[i] = v;
				}
				catch (Exception)
				{
					mIntervals[i] = 5;	// 深い意味はない、10のが妥当か？
				}
			}

			mTimer = new System.Windows.Forms.Timer();
			mTimer.Tick += new EventHandler(mTimer_Tick);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		~AniGIF()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
			{
				return;
			}
			mDisposed = true;
			if (disposing)
			{
				mImage = null;
				if (mTimer != null)
				{
					mTimer.Dispose();
					mTimer = null;
				}
			}
		}

		protected void ThrowExceptionIfDisposed()
		{
			if (mDisposed)
			{
				throw new ObjectDisposedException(this.GetType().ToString());
			}
		}

		#endregion 初期化関係

		#region メソッド

		/// <summary>
		/// アニメーション可能か調べるstatic関数
		/// </summary>
		public static bool CanAnimate(Image img)
		{
			bool can = false;
			try
			{
				can = img.GetFrameCount(FrameDimension.Time) > 1;
			}
			catch (Exception)
			{
			}
			return can;
		}

		#endregion メソッド

		#region 実装

		/// <summary>
		/// フレームインターバル時間を消化したのでイベントハンドラを呼び出す
		/// </summary>
		private void mTimer_Tick(object sender, EventArgs e)
		{
			if (FrameChanging != null)
			{
				FrameChanging(this, e);
				StartTimer();
			}
		}

		/// <summary>
		/// フレームインターバル時間をセットしてタイマーをスタート
		/// </summary>
		private void StartTimer()
		{
			int interval = mIntervals[_Index];
			if (BrowserCompatible)
			{
				if (interval <= 5)
				{
					interval = 10; // ブラウザ（主にIE）ではこうなるらしい。QTだと1でもOKらしい
				}
			}
			interval = interval * 10; // to MilliSeconds
			if (interval == 0)
			{
				interval = 5;	// 0はタイマーに設定できないので5msecにしておくか
			}
			mTimer.Interval = interval;
			mTimer.Enabled = true;
		}

		#endregion 実装
	}
}
