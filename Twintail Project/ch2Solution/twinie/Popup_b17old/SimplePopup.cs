// SimplePopup.cs

namespace PopupTest
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.IO;
	using System.Threading;
	using System.Text;
	using System.Net;
	using Twin;

	/// <summary>
	/// ToolTip�N���X���ۂ��ȒP�ȃ|�b�v�A�b�v
	/// </summary>
	public class SimplePopup : IDisposable, IPopupBase
	{
		#region PopupControl
		/// <summary>
		/// PopupControl �̊T�v�̐����ł��B
		/// </summary>
		public class PopupControl : System.Windows.Forms.Form
		{
			/// <summary>
			/// �K�v�ȃf�U�C�i�ϐ��ł��B
			/// </summary>
			private System.ComponentModel.Container components = null;
			private System.Windows.Forms.HScrollBar hScrollBar;
			private System.Windows.Forms.VScrollBar vScrollBar;
			
			private StringBuilder drawStr = new StringBuilder(512);

			private Size maximum = new Size(400, 200);	// �|�b�v�A�b�v�̍ő�T�C�Y
			private Size imageSize = new Size(0, 0);
			private Point marg = new Point(3, 3);
			private Brush foreBrush = null;

			/// <summary>
			/// �����F���擾�܂��͐ݒ�
			/// </summary>
			public override Color ForeColor {
				set {
					base.ForeColor = value;
					foreBrush = new SolidBrush(value);
				}
			}

			/// <summary>
			/// �|�b�v�A�b�v�̍ő�T�C�Y���擾�܂��͐ݒ�
			/// </summary>
			public Size Maximum {
				set {
					maximum = value;
				}
				get {
					return maximum;
				}
			}

			public override Font Font {
				set {
					base.Font = value;
					Util.label.Font = value;
				}
			}

			/// <summary>
			/// �摜�|�b�v�A�b�v�̉摜�T�C�Y���擾�܂��͐ݒ�
			/// </summary>
			public Size ImageSize {
				set {
					imageSize = new Size(
						Math.Max(0, value.Width),
						Math.Max(0, value.Height));
				}
				get { return imageSize; }
			}

			/// <summary>
			/// PopupControl�N���X�̃C���X�^���X��������
			/// </summary>
			public PopupControl()
			{
				//
				// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
				//
				InitializeComponent();

				//
				// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
				//
				SetStyle(ControlStyles.DoubleBuffer |
					ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

				foreBrush = new SolidBrush(ForeColor);
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
				this.hScrollBar = new System.Windows.Forms.HScrollBar();
				this.vScrollBar = new System.Windows.Forms.VScrollBar();
				this.SuspendLayout();
				// 
				// hScrollBar
				// 
				this.hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
					| System.Windows.Forms.AnchorStyles.Right);
				this.hScrollBar.LargeChange = 11;
				this.hScrollBar.Location = new System.Drawing.Point(3, 121);
				this.hScrollBar.Maximum = 10;
				this.hScrollBar.Name = "hScrollBar";
				this.hScrollBar.Size = new System.Drawing.Size(220, 12);
				this.hScrollBar.TabIndex = 1;
				this.hScrollBar.Visible = false;
				this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
				// 
				// vScrollBar
				// 
				this.vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
					| System.Windows.Forms.AnchorStyles.Right);
				this.vScrollBar.LargeChange = 11;
				this.vScrollBar.Location = new System.Drawing.Point(224, 3);
				this.vScrollBar.Maximum = 10;
				this.vScrollBar.Name = "vScrollBar";
				this.vScrollBar.Size = new System.Drawing.Size(12, 118);
				this.vScrollBar.TabIndex = 2;
				this.vScrollBar.Visible = false;
				this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
				// 
				// PopupControl
				// 
				this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
				this.BackColor = System.Drawing.SystemColors.Info;
				this.ClientSize = new System.Drawing.Size(240, 136);
				this.ControlBox = false;
				this.Controls.AddRange(new System.Windows.Forms.Control[] {
																			  this.vScrollBar,
																			  this.hScrollBar});
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				this.Name = "PopupControl";
				this.ShowInTaskbar = false;
				this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
				this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
				this.Text = "PopupControl";
				this.TopMost = true;
				this.Closing += new System.ComponentModel.CancelEventHandler(this.PopupControl_Closing);
				this.Paint += new System.Windows.Forms.PaintEventHandler(this.PopupControl_Paint);
				this.ResumeLayout(false);

			}
			#endregion

			private void vScrollBar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
			{
				Invalidate();
			}

			private void hScrollBar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
			{
				Invalidate();
			}

			private void PopupControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
			{
			}

			private void PopupControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
			{
				Graphics g = e.Graphics;
				DrawBorder(g);

				Size chrSize = Util.Calc("X");

				int x = -(hScrollBar.Value * chrSize.Width);
				int y = -(vScrollBar.Value * chrSize.Height);
				Point location = new Point(x + marg.X, y + marg.Y);

				// �`��̈��ݒ�
				int region_w = Width - marg.X * 2;
				if (vScrollBar.Visible) region_w -= vScrollBar.Width;

				int region_h = Height - marg.Y * 2;
				if (hScrollBar.Visible) region_h -= hScrollBar.Height;

				Rectangle rect = new Rectangle(marg.X, marg.Y, region_w, region_h);
				g.Clip = new Region(rect);

				// ��s���ǂݏo���b�`�悵�Ă���
				StringReader r = new StringReader(Text);
				Point pt = location;
				string line;

				while ((line = r.ReadLine()) != null)
				{
					if (pt.Y >= marg.Y)
					{
						drawStr.Append(line);
						drawStr.Append(Environment.NewLine);
					}

					pt.Y += chrSize.Height;

					if (pt.Y > Height)
						break;
				}

				// ������ݒ�
				g.DrawString(drawStr.ToString(), Font, foreBrush, location.X, marg.Y);
				drawStr.Remove(0, drawStr.Length);
			}

			/// <summary>
			/// �g��`��
			/// </summary>
			/// <param name="g"></param>
			private void DrawBorder(Graphics g)
			{
				g.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height -1);
				g.DrawRectangle(Pens.LightGray, 1, 1, Width - 3, Height - 3);
			}

			/// <summary>
			/// �|�b�v�A�b�v���镶�����ݒ肵�A���ƍ����������Œ���
			/// </summary>
			/// <param name="text"></param>
			public void Set(string text)
			{
				Text = text;

				Size size = Util.Calc(text);
				Size chrSize = Util.Calc("X");

				vScrollBar.Visible = hScrollBar.Visible = false;
				vScrollBar.Value = hScrollBar.Value = 0;

				int width = size.Width + marg.X * 2;
				int height = size.Height + marg.Y * 2;

				if (width > maximum.Width)
				{
					hScrollBar.Maximum = width / chrSize.Width;
					hScrollBar.LargeChange = maximum.Width / chrSize.Width;
					hScrollBar.Visible = true;
					width = maximum.Width;
				}
				if (height > maximum.Height)
				{
					vScrollBar.Maximum = height / chrSize.Height + 1;
					vScrollBar.LargeChange = maximum.Height / chrSize.Height;
					vScrollBar.Visible = true;
					height = maximum.Height;
				}

				if (vScrollBar.Visible)	width += vScrollBar.Width;
				if (hScrollBar.Visible) height += hScrollBar.Height;

				Width = width;
				Height = height;
				AdjustScrollBarPos();
			}

			/// <summary>
			/// �X�N���[���o�[�̈ʒu�𒲐�
			/// </summary>
			private void AdjustScrollBarPos()
			{
				// ���ăX�N���[���o�[�̂ݕ\������Ă���ꍇ
				if (vScrollBar.Visible && !hScrollBar.Visible)
				{
					vScrollBar.Height = Height - marg.Y * 2;
				}
				// ���X�N���[���o�[�̂ݕ\������Ă���ꍇ
				else if (hScrollBar.Visible && !vScrollBar.Visible)
				{
					hScrollBar.Width = Width - marg.X * 2;
				}
				// �����\������Ă���ꍇ
				else if (vScrollBar.Visible && hScrollBar.Visible)
				{
					vScrollBar.Height = Height - SystemInformation.HorizontalScrollBarHeight - marg.Y * 2;
					hScrollBar.Width = vScrollBar.Left - hScrollBar.Left;
				}
			}
		}

		public class Util
		{
			internal static readonly Label label;

			static Util()
			{
				label = new Label();
				label.AutoSize = true;
			}

			public static Size Calc(string str)
			{
				StringReader r = new StringReader(str);
				string line;

				int lineCount = 0;
				int width = 0, height = 0;

				while ((line = r.ReadLine()) != null)
				{
					// ��s�͖�������
					if (line != String.Empty)
					{
						label.Text = line;

						if (width < label.Width)
							width = label.Width;

						if (height < label.Height)
							height = label.Height;
					}
					lineCount++;
				}

				return new Size(width, height * lineCount);
			}
		}
		#endregion

		private PopupControl pop = new PopupControl();
		private PlainTextSkin skin = new PlainTextSkin();
		private PopupPosition position;
		private Point adjust = new Point(10, 10);

		private Thread thread = null;
		private string imageUri;

		private bool visible;
		private bool disposed = false;

		/// <summary>
		/// �|�b�v�A�b�v������W���擾
		/// </summary>
		protected Point PopupLocation {
			get {
				Point pt = Control.MousePosition;

				switch (position)
				{
				case PopupPosition.TopLeft:
					return new Point(pt.X - Width + adjust.X, pt.Y - Height + adjust.Y);

				case PopupPosition.TopRight:
					return new Point(pt.X - adjust.X, pt.Y - Height + adjust.Y);

				case PopupPosition.BottomLeft:
					return new Point(pt.X - Width + adjust.X, pt.Y - adjust.Y);

				case PopupPosition.BottomRight:
					return new Point(pt.X - adjust.X, pt.Y - adjust.Y);
				}
				throw new ApplicationException("PopupPosition���s���ł�");
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v����ʒu���擾�܂��͐ݒ�
		/// </summary>
		public PopupPosition Position {
			set {
				if (position != value)
					position = value;
			}
			get { return position; }
		}

		/// <summary>
		/// �|�b�v�A�b�v�̍ő�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public Size Maximum {
			set {
				pop.Maximum = value;
			}
			get {
				return pop.Maximum;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v���镶���̃t�H���g���擾�܂��͐ݒ�
		/// </summary>
		public Font Font {
			set {
				pop.Font = value;
			}
			get {
				return pop.Font;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v���镶���F���擾�܂��͐ݒ�
		/// </summary>
		public Color ForeColor {
			set {
				pop.ForeColor = value;
			}
			get {
				return pop.ForeColor;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�̔w�i�F���擾�܂��͐ݒ�
		/// </summary>
		public Color BackColor {
			set {
				pop.BackColor = value;
			}
			get {
				return pop.BackColor;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�̕����擾
		/// </summary>
		public int Width {
			get {
				return pop.Width;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�̍������擾
		/// </summary>
		public int Height {
			get {
				return pop.Height;
			}
		}

		/// <summary>
		/// �摜�|�b�v�A�b�v�̉摜�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public Size ImageSize {
			set {
				pop.ImageSize = value;
			}
			get { return pop.ImageSize; }
		}

		/// <summary>
		/// SimplePopup�N���X�̃C���X�^���X��������
		/// </summary>
		public SimplePopup()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			position = PopupPosition.TopRight;
			visible = false;

			pop.Location = new Point(-1000, -1000);
			pop.Size = new Size(0, 0);
			pop.Show();
		}

		/// <summary>
		/// SimplePopup�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="owner">�|�b�v�A�b�v�̐e�Ɏw�肷��t�H�[��</param>
		public SimplePopup(Form owner) : this()
		{
			if (owner == null)
				throw new ArgumentNullException("owner");

			pop.Owner = owner;

			if (owner != null)
				owner.Activate();
		}

		/// <summary>
		/// �|�b�v�A�b�v��\������
		/// </summary>
		/// <param name="text"></param>
		public void Show(string text)
		{
			if (!visible)
			{
				pop.Set(text);

				// ��ʂ̗̈���͂ݏo���Ȃ��悤�ɒ���
				Rectangle rect = Screen.PrimaryScreen.WorkingArea;
				Point pt = PopupLocation;

				if (pt.X + pop.Width > rect.Width)
					pt.X = rect.Width - pop.Width;

				if (pt.Y + pop.Height > rect.Height)
					pt.Y = rect.Height - pop.Height;

				pop.Location = pt;
				visible = true;
			}
		}

		/// <summary>
		/// resSets���|�b�v�A�b�v�ŕ\��
		/// </summary>
		/// <param name="resSets"></param>
		public virtual void Show(ResSetCollection resSets)
		{
			StringBuilder sb = new StringBuilder(128);

			foreach (ResSet res in resSets)
			{
				string text = skin.Convert(res);
				sb.Append(text);
				sb.Append("\r\n");
			}

			Show(sb.ToString());
		}

		/// <summary>
		/// res���|�b�v�A�b�v�ŕ\��
		/// </summary>
		/// <param name="res"></param>
		public virtual void Show(ResSet res)
		{
			ResSetCollection items = new ResSetCollection();
			items.Add(res);

			Show(items);
		}

		/// <summary>
		/// �w�肵��URL�̉摜���_�E�����[�h���ĕ\��
		/// </summary>
		/// <param name="uri"></param>
		public void ShowImage(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			// �_�E�����[�h���Ȃ璆�~
			if (thread != null && thread.IsAlive)
				thread.Abort();

			// �_�E�����[�h���Ă���Ԃ̃��b�Z�[�W��\��
			Show(uri + "\r\n�܂��摜�|�b�v�A�b�v�͖������ł��c");

			// �摜���_�E�����[�h
			imageUri = uri;
			thread = new Thread(new ThreadStart(DownloadImage));
			thread.IsBackground = true;
			thread.Start();
		}

		private void DownloadImage()
		{
			WebClient webClient = new WebClient();
			MemoryStream memory = new MemoryStream();
			Image image = null;
			byte[] data = null;

			try {
				data = webClient.DownloadData(imageUri);
				memory.Write(data, 0, data.Length);
				memory.Seek(0, SeekOrigin.Begin);

				image = Image.FromStream(memory);
				// ...
			}
			finally {
				webClient = null;
				memory = null;
				image = null;
				data = null;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v���B��
		/// </summary>
		public void Hide()
		{
			if (visible)
			{
				pop.Location = new Point(-1000, -1000);
				pop.Size = new Size(0, 0);
				visible = false;
			}
		}

		/// <summary>
		/// �g�p���Ă��郊�\�[�X�����
		/// </summary>
		public void Dispose()
		{
			if (!disposed)
				pop.Dispose();

			disposed = true;
		}
	}
}
