// CSharpToolBar.cs

namespace CSharpSamples
{
	using System;
	using System.Windows.Forms;
	using System.ComponentModel;
	using System.ComponentModel.Design;
	using System.Drawing;
	using System.Drawing.Design;

	using CSharpToolBarButtonCollection = 
		CSharpToolBarButton.CSharpToolBarButtonCollection;
	using System.Drawing.Imaging;
	using System.Runtime.InteropServices;

	/// <summary>
	/// C#�ō����Windows��ToolBar���ǂ�
	/// </summary>
	[DefaultEvent("ButtonClick")]
	public class CSharpToolBar : Control
	{
		private CSharpToolBarButtonCollection buttons;
		private CSharpToolBarAppearance appearance;
		private ToolBarTextAlign textAlign;
		private Border3DStyle borderStyle;
		private ImageList imageList;
		private Size buttonSize;
		private bool autoToolBarSize;
		private bool autoAdjustSize;
		private bool wrappable;
		private bool allowDragButton;

		// �{�^���̗]���B
		private Rectangle _Margin = new Rectangle(2,2,2,4);

		private CSharpToolBarButton activeButton = null;
		private Rectangle tempDropLine = Rectangle.Empty;

		protected override Size DefaultSize {
			get {
				return new Size(100, 50);
			}
		}

		protected Rectangle ClientRect {
			get {
				Rectangle client = Bounds;
				client.X = client.Y = 0;

				return client;
			}
		}

		/// <summary>
		/// �c�[���o�[�̃{�^�����i�[����Ă���R���N�V�������擾
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public CSharpToolBarButtonCollection Buttons {
			get {
				return buttons;
			}
		}

		/// <summary>
		/// �c�[���o�[�̋��E�����擾�܂��͐ݒ�
		/// </summary>
		[DefaultValue(typeof(Border3DStyle), "Adjust")]
		public Border3DStyle BorderStyle {
			set {
				if (borderStyle != value) {
					borderStyle = value;
					UpdateButtons();
				}
			}
			get { return borderStyle; }
		}

		/// <summary>
		/// �c�[���o�[�̃{�^���X�^�C�����擾�܂��͐ݒ�
		/// </summary>
		[DefaultValue(typeof(CSharpToolBarAppearance), "Normal")]
		public CSharpToolBarAppearance Appearance {
			set {
				if (appearance != value) {
					appearance = value;
					Refresh();
				}
			}
			get { return appearance; }
		}

		/// <summary>
		/// �{�^���e�L�X�g�z�u�̈ʒu���擾�܂��͐ݒ�
		/// </summary>
		[DefaultValue(typeof(ToolBarTextAlign), "Underneath")]
		public ToolBarTextAlign TextAlign {
			set {
				if (textAlign != value) {
					textAlign = value;
					UpdateButtons();
				}
			}
			get { return textAlign; }
		}

		/// <summary>
		/// �C���[�W���X�g���擾�܂��͐ݒ�
		/// </summary>
		public ImageList ImageList {
			set {
				imageList = value;
				
				foreach (CSharpToolBarButton b in buttons) 
					b.imageList = value;

				UpdateButtons();
			}
			get {
				return imageList;
			}
		}

		/// <summary>
		/// �c�[���o�[�{�^���̌Œ�T�C�Y���擾�܂��͐ݒ�B
		/// ���̃v���p�e�B��L���ɂ���ɂ�autoAdjustSize�v���p�e�B��false�ɐݒ肳��Ă���K�v������B
		/// </summary>
		[DefaultValue(typeof(Size), "80,25")]
		public Size ButtonSize {
			set {
				buttonSize = value;
				UpdateButtons();
			}
			get { return buttonSize; }
		}

		/// <summary>
		/// �c�[���o�[�̃T�C�Y�������Œ������邩�ǂ������擾�܂��͐ݒ�
		/// </summary>
		[DefaultValue(false)]
		public bool AutoToolBarSize {
			set {
				if (autoToolBarSize != value) {
					autoToolBarSize = value;
					UpdateButtons();
				}
			}
			get { return autoToolBarSize; }

		}
		/// <summary>
		/// �{�^���̕��������Œ������邩�ǂ������擾�܂��͐ݒ�
		/// </summary>
		[DefaultValue(true)]
		public bool AutoAdjustSize {
			set {
				if (autoAdjustSize != value) {
					autoAdjustSize = value;
					UpdateButtons();
				}
			}
			get { return autoAdjustSize; }
		}

		/// <summary>
		/// �c�[���o�[�̃{�^������s�Ɏ��܂�Ȃ��Ƃ���
		/// ���̍s�ɐ܂�Ԃ����ǂ������擾�܂��͐ݒ�
		/// </summary>
		[DefaultValue(true)]
		public bool Wrappable {
			set {
				if (wrappable != value) {
					wrappable = value;
					UpdateButtons();
				}
			}
			get { return wrappable; }
		}

		/// <summary>
		/// �{�^�����h���b�O�ňړ��ł��邩�ǂ����������l���擾�܂��͐ݒ�B
		/// </summary>
		[DefaultValue(false)]
		public bool AllowDragButton {
			set {
				allowDragButton = value;
			}
			get { return allowDragButton; }
		}

		/// <summary>
		/// �c�[���o�[�̃{�^�����N���b�N���ꂽ���ɔ���
		/// </summary>
		public event CSharpToolBarButtonEventHandler ButtonClick;

		/// <summary>
		/// CSharpToolBar�N���X�̃C���X�^���X��������
		/// </summary>
		public CSharpToolBar()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//

			buttons = new CSharpToolBarButtonCollection(this);
			appearance = CSharpToolBarAppearance.Normal;
			borderStyle = Border3DStyle.Adjust;
			textAlign = ToolBarTextAlign.Underneath;
			imageList = null;
			buttonSize = new Size(80, 25);
			autoToolBarSize = false;
			autoAdjustSize = true;
			wrappable = true;
			allowDragButton = true;

			// ��������������邽�߂Ɋe�X�^�C����ݒ�
			SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint, true);

			//this.BackColor = Color.Transparent;
			//Dock = DockStyle.Top;
		}

		void UpdateRegion()
		{/*
			var region = new Region(this.ClientRectangle);
			int clientWidth = this.ClientSize.Width;
			int clientHeight = this.ClientSize.Height;

			var bg = new Bitmap(clientWidth, clientHeight);

			// bg �ɕ�����`��
			using (Graphics g = Graphics.FromImage(bg))
			{
				foreach (CSharpToolBarButton button in buttons)
					DrawButton(g, button, false, false);
			}

			BitmapData bitdata = bg.LockBits(this.ClientRectangle, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			int stride = bitdata.Stride;
			var buffer = new byte[stride * clientHeight];

			Marshal.Copy(bitdata.Scan0, buffer, 0, buffer.Length);
			bg.UnlockBits(bitdata);
			bg.Dispose();

			int line = 0;
			for (int y = 0; y < clientHeight; y++)
			{
				line = stride * y;
				for (int x = 0; x < clientWidth; x++)
				{
					if (buffer[line + x * 4 + 3] == 0)
					{
						region.Exclude(new Rectangle(x, y, 1, 1));
					}
				}
			}

			this.Region = region;*/
		}



		#region Override Events
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			Graphics g = e.Graphics;

			foreach (CSharpToolBarButton button in buttons)
				DrawButton(g, button, false, false);
			
			// ���E����`��
			Rectangle rc = Bounds;

			ControlPaint.DrawBorder3D(g, ClientRectangle, borderStyle);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			UpdateButton(activeButton);
			activeButton = null;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			CSharpToolBarButton button = ButtonFromPoint(e.X, e.Y);

			// �Z�p���[�^�̏ꍇ�͉������Ȃ�
			if (button != null && button.Style == CSharpToolBarButtonStyle.Separator)
				return;

			// �t���b�g�`���̏ꍇ�́A�����o�鋫�E����`��
			if (e.Button == MouseButtons.None)
			{
				if (button == activeButton)
					return;

				UpdateButton(activeButton);

				activeButton = button;

				if (appearance == CSharpToolBarAppearance.Flat &&
					button != null)
				{
					using (Graphics g = CreateGraphics())
						ControlPaint.DrawBorder3D(g, button.Bounds, Border3DStyle.RaisedInner);
				}
				else if (appearance == CSharpToolBarAppearance.VisualStudio &&
					button != null)
				{
					using (Graphics g = CreateGraphics())
						DrawButton(g, button, false, true);
				}

			}
			// �{�^���̃h���b�O����
			else if (e.Button == MouseButtons.Left && allowDragButton)
			{
				if (ClientRect.Contains(e.X, e.Y))
				{
					DrawHorzLine(GetDropButtonIndex(e.X, e.Y));
					Cursor = Cursors.Default;
				}
				// �N���C�A���g�̈悩��o�Ă���΃h���b�O����𒆎~
				else {
					DrawHorzLine(-1);
					Cursor = Cursors.No;
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			activeButton = ButtonFromPoint(e.X, e.Y);

			// �{�^���������ꂽ�悤�ɕ`��
			if (e.Button == MouseButtons.Left &&
				activeButton != null)
			{
				if (activeButton.Style != CSharpToolBarButtonStyle.Separator)
				{
					using (Graphics g = CreateGraphics())
						DrawButton(g, activeButton, true, true);
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			// �{�^���������ꂽ�悤�ɕ`��
			if (e.Button == MouseButtons.Left &&
				activeButton != null)
			{
				if (activeButton.Style == CSharpToolBarButtonStyle.Separator)
					return;

				CSharpToolBarButton button = ButtonFromPoint(e.X, e.Y);

				UpdateButton(activeButton);
				DrawHorzLine(-1);

				// �N���b�N���ꂽ�{�^���ƌ��݂̃}�E�X���W�ɂ���{�^�����ʂ̕��ł���΁A
				// activeButtons���ړ�
				if (activeButton != button)
				{
					if (allowDragButton && ClientRect.Contains(e.X, e.Y))
					{
						int index = GetDropButtonIndex(e.X, e.Y);

						if (index >= 0 && index <= buttons.Count)
						{
							buttons.ChangeIndex(activeButton, index);
						}
					}
					Cursor = Cursors.Default;
				}
				else
				{
					// �N���b�N�C�x���g�𔭐�������
					OnButtonClick(new CSharpToolBarButtonEventArgs(activeButton));
				}

				activeButton = null;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			UpdateButtons();
		}
		#endregion

		/// <summary>
		/// �{�^�����ŐV�̏�ԂɍX�V
		/// </summary>
		internal void UpdateButtons()
		{
			int height = 0;

			// �{�^����Rectangle���W���X�V
			using (Graphics g = CreateGraphics())
			{
				foreach (CSharpToolBarButton button in buttons)
				{
					button.bounds = GetButtonRect(g, button);
					height = Math.Max(height, button.Bounds.Bottom);
				}
			}

			// �����𒲐� (3D�{�[�_�[�̃T�C�Y������)
			if (AutoToolBarSize)
				this.Height = height + SystemInformation.Border3DSize.Height;

			UpdateRegion();
			Refresh();
		}

		/// <summary>
		/// �w�肵���{�^�����ĕ`��
		/// </summary>
		/// <param name="button">�ĕ`�悷��{�^�� (null���w�肵���ꍇ�͉������Ȃ�)</param>
		protected void UpdateButton(CSharpToolBarButton button)
		{
			if (button != null)
			{
				Invalidate(button.Bounds, false);
				Update();
			}
		}

		/// <summary>
		/// �h���b�v��̃{�^�����擾
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected int GetDropButtonIndex(int x, int y)
		{
			CSharpToolBarButton button = ButtonFromPoint(x, y);
			
			if (button != null)
			{
				int x2 = x - button.Bounds.X;

				if (x2 >= button.Bounds.Width / 2)
				{
					return button.Index + 1;
				}
				else {
					return button.Index;
				}
			}
			return -1;
		}

		/// <summary>
		/// �h���b�O���\���c�̃��C����`��
		/// </summary>
		/// <param name="index">�`�悷��{�^���̃C���f�b�N�X (-1�Ȃ��������)</param>
		protected void DrawHorzLine(int index)
		{
			if (tempDropLine != Rectangle.Empty)
				ControlPaint.FillReversibleRectangle(tempDropLine, Color.Black);

			if (index >= 0)
			{
				CSharpToolBarButton button = (index < Buttons.Count) ?
					Buttons[index] : Buttons[Buttons.Count-1];

				Rectangle rc = button.Bounds;
				rc.Width = 2;

				if (index >= Buttons.Count)
					rc.X = button.Bounds.Right - 2;

				tempDropLine = RectangleToScreen(rc);

				using (Graphics g = CreateGraphics())
					ControlPaint.FillReversibleRectangle(tempDropLine, Color.Black);
			}
			else {
				tempDropLine = Rectangle.Empty;
			}
		}

		/// <summary>
		/// �{�^����`��
		/// </summary>
		/// <param name="g"></param>
		/// <param name="button"></param>
		/// <param name="pushed"></param>
		protected void DrawButton(Graphics g, CSharpToolBarButton button, bool pushed, bool active)
		{
			if (g == null) {
				throw new ArgumentNullException("g");
			}
			if (button == null) {
				throw new ArgumentNullException("button");
			}

			StringFormat format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			
			if (textAlign == ToolBarTextAlign.Right)
				format.FormatFlags = StringFormatFlags.NoWrap;

			Rectangle bounds = button.Bounds;
			Rectangle imageRect = Rectangle.Empty, textRect = Rectangle.Empty;
			Size imgSize = (imageList != null) ? imageList.ImageSize : new Size(0, 0);

			if (button.Style == CSharpToolBarButtonStyle.Separator)
			{
				// ���E����`��
				Size border = SystemInformation.Border3DSize;
				Rectangle rect = button.Bounds;
				rect.X += rect.Width / 2 - border.Width / 2;
				rect.Y += _Margin.Y;
				rect.Height -= _Margin.Y;
				rect.Width = border.Width;
				ControlPaint.DrawBorder3D(g, rect, Border3DStyle.Etched, Border3DSide.Right);
				return;
			}

			switch (textAlign)
			{
				// �C���[�W����ӁA�e�L�X�g�����ӂɔz�u
			case ToolBarTextAlign.Underneath:
				imageRect = new Rectangle(bounds.X + bounds.Width / 2 - imgSize.Width / 2, bounds.Y+_Margin.Y, imgSize.Width, imgSize.Height);
				textRect = new Rectangle(bounds.X, imageRect.Bottom, bounds.Width, bounds.Height - imageRect.Height);
				break;
				// �C���[�W�����ӁA�e�L�X�g���E�ӂɔz�u
			case ToolBarTextAlign.Right:
				imageRect = new Rectangle(bounds.X+_Margin.X, bounds.Y + bounds.Height / 2 - imgSize.Height / 2, imgSize.Width, imgSize.Height);
				textRect = new Rectangle(imageRect.Right, bounds.Y, bounds.Width - imageRect.Width, bounds.Height);
				break;
			}

			if (appearance == CSharpToolBarAppearance.Normal)
			{
				if (pushed)
				{
					// �ʏ�̃{�^���������ꂽ��Ԃ�`��
					ControlPaint.DrawButton(g, 
						activeButton.Bounds, ButtonState.Pushed);
				}
				else {
					// �ʏ�̃{�^����`��
					ControlPaint.DrawButton(g, 
						bounds, ButtonState.Normal);
				}
			}
			else if (appearance == CSharpToolBarAppearance.Flat)
			{
				if (pushed)
				{
					// �t���b�g�{�^���������ꂽ��Ԃ�`��
					ControlPaint.DrawBorder3D(g, 
						activeButton.Bounds, Border3DStyle.SunkenOuter);
				}
			}
			else if (appearance == CSharpToolBarAppearance.VisualStudio)
			{
				if (active)
				{
					Rectangle rc = button.Bounds;

					rc.Width -= 2;
					rc.Height -= 2;

					Color color = pushed ?
						SystemColors.ControlDark : SystemColors.Highlight;

					using (Brush b = new SolidBrush(Color.FromArgb(50, color)))
						g.FillRectangle(b, rc);

					using (Pen pen = new Pen(color))
						g.DrawRectangle(pen, rc);
				}

			}

			if (imageList != null &&
				button.ImageIndex >= 0 && button.ImageIndex < imageList.Images.Count)
			{
				// �A�C�R����`��
				g.DrawImage(imageList.Images[button.ImageIndex], imageRect.X, imageRect.Y);
			}

			if (button.Text.Length > 0)
			{
				// �e�L�X�g��`��
				g.DrawString(button.Text, Font, SystemBrushes.ControlText, textRect, format);
			}
		}

		/// <summary>
		/// �w�肵�����W�ɂ���CSharpToolBarButton���擾
		/// </summary>
		/// <param name="x">�N���C�A���g���W��X��</param>
		/// <param name="y">�N���C�A���g���W��Y��</param>
		/// <returns>�w�肵�����W�ɑ��݂���CSharpToolBarButton�B������Ȃ����null��Ԃ��B</returns>
		public CSharpToolBarButton ButtonFromPoint(int x, int y)
		{
			foreach (CSharpToolBarButton button in buttons)
			{
				if (button.Bounds.Contains(x, y))
					return button;
			}

			return null;
		}

		/// <summary>
		/// �w�肵��button��Rectangle���W���v�Z
		/// </summary>
		/// <param name="g">�����񕝂̌v�Z�Ɏg�p����Graphics�N���X�̃C���X�^���X</param>
		/// <param name="button">�T�C�Y���v�Z����CSharpToolBarButton</param>
		/// <returns>button��Rectangle���W��Ԃ�</returns>
		protected Rectangle GetButtonRect(Graphics g, CSharpToolBarButton button)
		{
			Size borderSize = borderStyle == Border3DStyle.Adjust ? new Size(0,0) : SystemInformation.Border3DSize;
			Rectangle rect = new Rectangle(borderSize.Width, borderSize.Height,0,0);
			int height = 0;

			foreach (CSharpToolBarButton b in buttons)
			{
				Size size;
				
				if (b.Style == CSharpToolBarButtonStyle.Separator)
				{
					size = GetButtonSize(g, b);
				}
				else {
					size = autoAdjustSize ? GetButtonSize(g, b) : buttonSize;
				}

				rect.Width = size.Width;
				rect.Height = size.Height;
				height = Math.Max(height, size.Height);

				// ���W���c�[���o�[�̕����͂ݏo���āA
				// �Ȃ�����Wrappable�v���p�e�B��true�̏ꍇ
				if ((rect.X + rect.Width) > ClientSize.Width && Wrappable)
				{
					rect.X = borderSize.Width;
					rect.Y += height;
				}

				if (b.Equals(button))
					return rect;

				rect.X += size.Width;
			}

			return Rectangle.Empty;
		}

		/// <summary>
		/// �w�肵��button�̃T�C�Y���v�Z
		/// </summary>
		/// <param name="g">�����񕝂̌v�Z�Ɏg�p����Graphics�N���X�̃C���X�^���X</param>
		/// <param name="button">�T�C�Y���v�Z����CSharpToolBarButton</param>
		/// <returns>button�̃T�C�Y��Ԃ�</returns>
		protected Size GetButtonSize(Graphics g, CSharpToolBarButton button)
		{
			if (g == null) {
				throw new ArgumentNullException("g");
			}
			if (button == null) {
				throw new ArgumentNullException("button");
			}

			Size size, space = g.MeasureString(" ", Font).ToSize();
			
			// �Z�p���[�^
			if (button.Style == CSharpToolBarButtonStyle.Separator)
			{
				size = space;
				size.Width = SystemInformation.Border3DSize.Width;
				
				if (textAlign == ToolBarTextAlign.Underneath)
					size.Height += space.Height;
			}
			// �����A�摜�Ƃ��ɐݒ肳��Ă��Ȃ�
			else if (button.Text.Length == 0 && button.ImageIndex == -1)
			{
				size = space;
			
				if (textAlign == ToolBarTextAlign.Underneath)
					size.Height += space.Height;
			}
			// �����̂ݐݒ肳��Ă���
			else if (button.Text.Length > 0 && button.ImageIndex == -1)
			{
				size = g.MeasureString(button.Text, Font).ToSize();
			
				if (textAlign == ToolBarTextAlign.Underneath)
					size.Height += space.Height;
			}
			// �摜�̂ݐݒ肳��Ă���
			else if (button.Text.Length == 0 && button.ImageIndex != -1)
			{
				if (imageList != null)
				{
					size = imageList.ImageSize;
				}
				else {// �摜���ݒ肳��Ă���̂� ImageList �������ꍇ�͋󔒂ŃT�C�Y����
					size = space;
				}
				if (textAlign == ToolBarTextAlign.Underneath)
					size.Height += space.Height;
			}
			else {
				size = g.MeasureString(button.Text, Font).ToSize();
		
				// �A�C�R�������݂���΃A�C�R���T�C�Y�𑫂�
				if (imageList != null && button.ImageIndex != -1)
				{
					Size imageSize = imageList.ImageSize;

					switch (textAlign)
					{
						// �e�L�X�g���C���[�W�̉��ɔz�u�����
					case ToolBarTextAlign.Underneath:
						size.Width = Math.Max(size.Width, imageSize.Width);
						size.Height += imageSize.Height;
						break;
						// �e�L�X�g���C���[�W�̍��ɔz�u�����
					case ToolBarTextAlign.Right:
						size.Width += imageSize.Width;
						size.Height = Math.Max(size.Height, imageSize.Height);
						break;
					}
				}
			}

			size.Width += _Margin.X + _Margin.Width;
			size.Height += _Margin.Y + _Margin.Height;

			return size;
		}

		/// <summary>
		/// ButtonClick�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnButtonClick(CSharpToolBarButtonEventArgs e)
		{
			if (ButtonClick != null)
				ButtonClick(this, e);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);

		}
	}

	public enum CSharpToolBarAppearance
	{
		Normal,
		Flat,
		VisualStudio,
	}
}
