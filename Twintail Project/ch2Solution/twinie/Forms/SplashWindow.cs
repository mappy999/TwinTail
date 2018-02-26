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
	/// �ȒP�ȃX�v���b�V���E�C���h�E
	/// </summary>
	public class SplashWindow : System.Windows.Forms.Form
	{
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
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
		/// �\�����镶������擾�܂��͐ݒ�
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
		/// ������̔z�u�����擾�܂��͐ݒ�
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
		/// ������̍s�z�u�����擾�܂��͐ݒ�
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
		/// ������̕`��Ɏg�p����u���V���擾�܂��͐ݒ�
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
		/// SplashWindow�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="imagePath">�\������摜�t�@�C���ւ̃p�X</param>
		public SplashWindow(string imagePath)
		{
			if (imagePath == null) {
				throw new ArgumentNullException("imagePath");
			}

			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);

			this.message = String.Empty;
			this.foreBrush = Brushes.Black;
			this.alignment = StringAlignment.Far;
			this.lineAlignment = StringAlignment.Far;
			this.loaded = false;

			// �摜��ǂݍ���
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
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
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
