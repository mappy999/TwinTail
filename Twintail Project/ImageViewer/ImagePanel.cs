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
	using System.Drawing.Imaging;	// FrameDimension�̂��߂ɒǉ�

	/// <summary>
	/// �摜��\������p�l��
	/// </summary>
//	public class ImagePanel : Panel
	public class ImagePanel : ScrollableControl	// �����͌y���Ȃ�Ǝv��
	{
		private Image imageSrc;
		private Image buffer;
		private bool mosaic;
		private bool viewOriginalSize;

//		private bool animation = false;	// ���W�b�N��ς����̂ŕs�v��
		private bool stopped = true;
		private AniGIF mAgif;			// �A�j��GIF�����s����C���X�^���X

		/// <summary>
		/// �\������Ă���摜���擾
		/// </summary>
		public Image Image {
			get {
				return imageSrc;
			}
		}

		/// <summary>
		/// �摜���ǂݍ��܂�Ă��邩�ǂ����������l���擾
		/// </summary>
		public bool IsLoaded {
			get {
				return (imageSrc != null) ? true : false;
			}
		}

		/// <summary>
		/// �摜��������ŕ\�����邩�ǂ����������l���擾�܂��͐ݒ�
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
		/// ���U�C�N�\�����邩�ǂ������擾�܂��͐ݒ�
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
		/// �A�j���[�V�����\���ǂ����𔻒f
		/// </summary>
		public bool CanAnimate {
			get {
				return IsLoaded && AniGIF.CanAnimate(imageSrc);	// ImageAnimator�ł���������
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
		/// ImagePanel�N���X�̃C���X�^���X��������
		/// </summary>
		public ImagePanel()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);

			TabStop = false;//���ꂵ�Ȃ��Ɖ摜���J�����тɂȂ����r���[�A���A�N�e�B�u�ɂȂ��Ă��܂�
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
					// �c���䂪�Œ肳�ꂽ�摜�T�C�Y���擾
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
		/// �摜��ǂݍ��ݕ\������
		/// </summary>
		/// <param name="filename"></param>
		public void Load(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");

//			StopAnimation();	// �~�߂Ȃ��Ă����v�̂悤��

			Unload();
			imageSrc = new Bitmap( filename );

			UpdateAutoScrollMode();
			Invalidate();
		}

		/// <summary>
		/// �摜��ǂݍ��ݕ\������
		/// </summary>
		/// <param name="image"></param>
		public void Load(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image");

//			StopAnimation();	// �~�߂Ȃ��Ă����v�̂悤��

			Unload();
			imageSrc = image;

			UpdateAutoScrollMode();
			Invalidate();
		}

		/// <summary>
		/// ���ݕ\������Ă���摜���������
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
		/// �A�j���[�V�������J�n
		/// </summary>
		public void Animate()
		{
			if (imageSrc == null)
				throw new InvalidOperationException("�C���[�W���ǂݍ��܂�Ă��܂���");

			if (AniGIF.CanAnimate(imageSrc))
			{
				if ( mAgif == null )
				{
					mAgif = new AniGIF( imageSrc , new EventHandler( OnAnimateFrameChanged ) );
				}
				mAgif.Animating = !mAgif.Animating;	// ���j���[�u�A�j���[�V�������J�n�v�Ńg�O������悤�ɂ��Ă݂�
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
				mAgif.Index++;	// �f�N�������g���Z�q�ɂ���Ƌt�Đ����ł���

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
	/// ���OImageAnimator
	/// </summary>
	public class AniGIF : IDisposable
	{
		#region �v���C�x�[�g�����o�[

		private Image mImage;						// �A�j��GIF�ێ�
		private int[] mIntervals;					// �t���[���C���^�o���Amsec
		private System.Windows.Forms.Timer mTimer;	// �A�j�������邽�߂̃^�C�}�[�AUI�X���b�h�Ȃ̂��L��
		private bool mDisposed;

		#endregion �v���C�x�[�g�����o�[

		#region �v���p�e�B

		/// <summary>
		/// �C���X�^���X���A�j���[�V�����\��
		/// </summary>
		public bool Animatable
		{
			get
			{
				return _Frames > 1;
			}
		}

		/// <summary>
		/// �A�j���[�V�������s����
		/// ���s�̊J�n���~���ł���
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
		/// �A�j��GIF���t���[����
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
		/// ���݃A�N�e�B�u�ȃA�j��GIF���t���[���̃C���f�b�N�X
		/// �I�����ł���
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
		/// �h�d��FireFox�ł̓t���[���C���^�[�o���̒l���T�ȉ�����
		/// �����I��10�A�܂�100msec�C���^�[�o�[���ɂ��Ă��܂��悤��
		/// �����Ă��̂��Ƃ𓖂č���ŃC���^�[�o�[���l���ς�GIF�������ς�����̂�
		/// �؂�ւ�����悤�ɂ��Ă݂�
		///�Ƃ����̂͂����Ə����Ȓl�����҂��Ă���f�h�e�������ς�����̂ŁB
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

		#endregion �v���p�e�B

		#region �C�x���g

		/// <summary>
		/// �t���[���ω��^�C�~���O�ŌĂяo���C�x���g�n���h��
		/// �t���[���������I�ɐi�߂���͂��Ȃ��̂ŃC�x���g�n���h�����ł��ׂ�
		/// </summary>
		public event EventHandler FrameChanging;

		#endregion �C�x���g

		#region �������֌W

		/// <summary>
		/// �R���X�g���N�^
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
			// GIF���C���^�[�o���l�����o���ă^�C�}�[���Ԕz��Ɋi�[
			//
			mIntervals = new int[_Frames];
			PropertyItem p = img.GetPropertyItem(20736);	// ���̃v���p�e�B���t���[���C���^�[�o��
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
					mIntervals[i] = 5;	// �[���Ӗ��͂Ȃ��A10�̂��Ó����H
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

		#endregion �������֌W

		#region ���\�b�h

		/// <summary>
		/// �A�j���[�V�����\�����ׂ�static�֐�
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

		#endregion ���\�b�h

		#region ����

		/// <summary>
		/// �t���[���C���^�[�o�����Ԃ����������̂ŃC�x���g�n���h�����Ăяo��
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
		/// �t���[���C���^�[�o�����Ԃ��Z�b�g���ă^�C�}�[���X�^�[�g
		/// </summary>
		private void StartTimer()
		{
			int interval = mIntervals[_Index];
			if (BrowserCompatible)
			{
				if (interval <= 5)
				{
					interval = 10; // �u���E�U�i���IE�j�ł͂����Ȃ�炵���BQT����1�ł�OK�炵��
				}
			}
			interval = interval * 10; // to MilliSeconds
			if (interval == 0)
			{
				interval = 5;	// 0�̓^�C�}�[�ɐݒ�ł��Ȃ��̂�5msec�ɂ��Ă�����
			}
			mTimer.Interval = interval;
			mTimer.Enabled = true;
		}

		#endregion ����
	}
}
