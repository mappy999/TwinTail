// TabButtonControl.cs

using System;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace CSharpSamples
{
	using TabButtonCollection = TabButton.TabButtonCollection;

	/// <summary>
	/// TabControl �̃^�u���������̋@�\�����N���X�ł��B
	/// </summary>
	[DefaultEvent("SelectedChanged")]
	public class TabButtonControl : Control
	{
		private TabButtonCollection buttons;

		private TabButtonStyle buttonStyle;
		private Border3DStyle borderStyle;
		private ImageList imageList;
		private Size buttonSize;
		private Color hotTrackColor;
		private bool hotTrack;
		private bool autoAdjustControlHeight;
		private bool autoAdjustButtonSize;
		private bool wrappable;

		private TabButton selected = null;		// ���ݑI������Ă��� TabButton
		private TabButton focused = null;		// ���O�ɃN���b�N���ꂽ TabButton
		private TabButton hot = null;			// �}�E�X�����ɂ��� TabButton

		private bool disposed = false;

		protected override Size DefaultSize {
			get {
				return new Size(200, 50);
			}
		}

		/// <summary>
		/// �o�^����Ă���{�^���̃R���N�V������Ԃ��܂��B
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public TabButtonCollection Buttons {
			get {
				return buttons;
			}
		}

		/// <summary>
		/// �{�^���ɕ\������ ImageList ���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public ImageList ImageList {
			set {
				imageList = value;

				foreach (TabButton b in buttons)
					b.imageList = value;

				UpdateButtons();
			}
			get { return imageList; }
		}

		/// <summary>
		/// �{�^���̃X�^�C����\�� TabButtonStyle �񋓑̂��擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(TabButtonStyle), "Button")]
		public TabButtonStyle Style {
			set {
				buttonStyle = value;
				Refresh();
			}
			get { return buttonStyle; }
		}

		/// <summary>
		/// �^�u�R���g���[���̋��E�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Border3DStyle), "Adjust")]
		public Border3DStyle BorderStyle {
			set {
				if (borderStyle != value) {
					borderStyle = value;
					Refresh();
				}
			}
			get { return borderStyle; }
		}

		/// <summary>
		/// �^�u����s�Ɏ��܂�Ȃ��Ƃ��ɁA
		/// ���̍s�ɐ܂�Ԃ����ǂ����������l���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(true)]
		public bool Wrappable {
			set {
				if (wrappable != value) 
				{
					wrappable = value;
					throw new NotImplementedException();
				}
			}
			get { return wrappable; }
		}

		/// <summary>
		/// �c�[���o�[�{�^���̌Œ�T�C�Y���擾�܂��͐ݒ肵�܂��B
		/// ���̃v���p�e�B��L���ɂ���ɂ� AutoAdjustButtonSize �v���p�e�B�� false �ɐݒ肳��Ă���K�v������܂��B
		/// </summary>
		[DefaultValue(typeof(Size), "80, 20")]
		public Size ButtonSize {
			set {
				buttonSize = value;
				UpdateButtons();
			}
			get { return buttonSize; }
		}

		/// <summary>
		/// HotTrack �v���p�e�B���L���ɂȂ��Ă���ꍇ�ɁA�ω�����F��ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color HotTrackColor {
			set {
				hotTrackColor = value;
			}
			get {
				return hotTrackColor;
			}
		}

		/// <summary>
		/// �}�E�X���^�u�̏�ɒu�����Ƃ��A
		/// �O�ς��ω����邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(true)]
		public bool HotTrack {
			set {
				hotTrack = value;
			}
			get { return hotTrack; }
		}
	
		/// <summary>
		/// �R���g���[���̍����������Œ������邩�ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(true)]
		public bool AutoAdjustControlHeight {
			set {
				if (autoAdjustControlHeight != value)
				{
					autoAdjustControlHeight = value;
					UpdateButtons();
				}
			}
			get { return autoAdjustControlHeight; }

		}
		/// <summary>
		/// �^�u�{�^���̃T�C�Y�������Œ������邩�ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(true)]
		public bool AutoAdjustButtonSize {
			set {
				if (autoAdjustButtonSize != value)
				{
					autoAdjustButtonSize = value;
					UpdateButtons();
				}
			}
			get { return autoAdjustButtonSize; }
		}

		/// <summary>
		/// �I������Ă��� TabButton ���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(false)]
		public TabButton Selected {
			set {
				if (value == null)
					throw new ArgumentNullException();

				SetSelected(value);
			}
			get {
				return selected;
			}
		}

		/// <summary>
		/// �I������Ă���^�u���ύX���ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		public event TabButtonEventHandler SelectedChanged;

		/// <summary>
		/// TabButtonControl �N���X�̃C���X�^���X���������B
		/// </summary>	
		public TabButtonControl()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			buttons = new TabButtonCollection(this);
			borderStyle = Border3DStyle.Adjust;
			buttonStyle = TabButtonStyle.Flat;
			autoAdjustButtonSize = true;
			autoAdjustControlHeight = true;
			wrappable = true;
			imageList = null;
			hotTrack = true;
			hotTrackColor = SystemColors.ControlDark;
			buttonSize = new Size(80, 20);
			
			// ��������������邽�߂Ɋe�X�^�C����ݒ�
			SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint, true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!disposed)
				{
					disposed = true;
				}
			}
			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			foreach (TabButton button in buttons)
			{
				if (button.Bounds.IntersectsWith(e.ClipRectangle))
					DrawButton(e.Graphics, button);
			}

			ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, borderStyle);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			UpdateButtons();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				TabButton button = GetButtonAt(new Point(e.X, e.Y));

				if (button != null)
				{
					focused = button;
					SetSelected(focused);
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			TabButton button = GetButtonAt(new Point(e.X, e.Y));

			if (button != null)
			{
				Region region = new Region(button.Bounds);
				if (hot != null) region.Union(hot.Bounds);

				hot = button;

				Invalidate(region);
				Update();
			}
			else if (hot != null)
			{
				Rectangle rc = hot.Bounds;
				hot = null;

				Invalidate(rc);
				Update();
			}

		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (e.Button == MouseButtons.Left)
			{
				TabButton button = GetButtonAt(new Point(e.X, e.Y));

				if (button != null && focused != null && focused != button)
					buttons.InsertBefore(focused, button);
			}
			focused = null;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			if (hot != null)
			{
				Rectangle rc = hot.Bounds;
				hot = null;

				Invalidate(rc);
				Update();
			}
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
		}

		/// <summary>
		/// �w�肵���{�^���̏�Ԃ��X�V���܂��B
		/// </summary>
		/// <param name="button"></param>
		internal void UpdateButton(TabButton button)
		{
			if (button != null)
			{
				Invalidate(button.Bounds);
				Update();
			}
		}

		/// <summary>
		/// ���ׂẴ{�^���̏�Ԃ��X�V���܂��B
		/// </summary>
		internal void UpdateButtons()
		{
			if (buttons.Count > 0 && selected == null)
				selected = buttons[0];

			else if (buttons.Count == 0)
				selected = null;

			using (Graphics g = CreateGraphics())
			{
				// ���ׂẴ{�^���̍��W���X�V
				UpdateButtonRect(g);
			}

			int height = 0;

			foreach (TabButton button in buttons)
				height = Math.Max(height, button.Bounds.Bottom);	

			if (AutoAdjustControlHeight)
			{
				this.Height = height + SystemInformation.Border3DSize.Height;
			}

			Refresh();
		}

		/// <summary>
		/// SelectedChanged �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="button"></param>
		private void OnSelectedChanged(TabButton button)
		{
			if (SelectedChanged != null)
				SelectedChanged(this, new TabButtonEventArgs(button));
		}

		private void SetSelected(TabButton button)
		{
			if (selected != null)
			{
				Region region = new Region(selected.Bounds);
				region.Union(button.Bounds);

				selected = button;
				Invalidate(region);
			}
			else {
				selected = button;
				Invalidate(selected.Bounds);
			}

			Update();

			OnSelectedChanged(selected);
		}

		private void DrawButton(Graphics g, TabButton button)
		{
			StringFormat format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags = StringFormatFlags.NoWrap;

			Rectangle bounds = button.Bounds;
			Size imgSize = (imageList != null) ? imageList.ImageSize : new Size(0, 0);

			int margin = (button.ImageIndex != -1) ? 5 : 0;
			Rectangle imageRect = new Rectangle(bounds.X + margin, bounds.Y + bounds.Height / 2 - imgSize.Height / 2, imgSize.Width, imgSize.Height);
			Rectangle textRect = new Rectangle(imageRect.Right, bounds.Y, bounds.Width - imageRect.Width - margin, bounds.Height);

			// ���E����`��
			DrawButtonBorder(g, button);
			
			// �z�b�g�Ȕw�i�F��`��
			if (hotTrack && button.Equals(hot))
			{
				Rectangle rc = button.Bounds;
				Size size = SystemInformation.Border3DSize;

				rc.Width -= 2;
				rc.Height -= 2;

				using (Brush b = new SolidBrush(Color.FromArgb(50, HotTrackColor)))
					g.FillRectangle(b, rc);

				using (Pen pen = new Pen(HotTrackColor))
					g.DrawRectangle(pen, rc);
			}
			else {
				// �ʏ�̔w�i�F��`��			
				using (Brush brush = new SolidBrush(button.IsSelected ? 
						button.ActiveBackColor : button.InactiveBackColor))
				{
					Rectangle rc = button.Bounds;
					Size border = SystemInformation.Border3DSize;

					rc.Inflate(-border.Width, -border.Height);

					g.FillRectangle(brush, rc);
				}
			}

			// �A�C�R����`��
			if (imageList != null &&
				button.ImageIndex >= 0 && button.ImageIndex < imageList.Images.Count)
			{
				g.DrawImage(imageList.Images[button.ImageIndex], imageRect);
			}

			// �e�L�X�g��`��
			Brush foreBrush = new SolidBrush(button.IsSelected ?
					button.ActiveForeColor : button.InactiveForeColor);

			Font textFont = button.IsSelected ? 
				new Font(button.ActiveFontFamily, Font.Size, button.ActiveFontStyle) :
				new Font(button.InactiveFontFamily, Font.Size, button.InactiveFontStyle);

			g.DrawString(button.Text, textFont, foreBrush, textRect, format);
			
			textFont.Dispose();
			foreBrush.Dispose();
		}

		private void DrawButtonBorder(Graphics g, TabButton button)
		{
			switch (buttonStyle)
			{
			case TabButtonStyle.Flat:
				// ����ȃ^�u�R���g���[��
				if (button.IsSelected)
				{
					ControlPaint.DrawBorder3D(g, button.Bounds, Border3DStyle.Sunken);
				}

				// �^�u�ƃ^�u�̊Ԃɋ��E����`��
				Size border = SystemInformation.Border3DSize;
				Rectangle rect = button.Bounds;
				rect.X = rect.Right + border.Width / 2;
				rect.Width = border.Width;
				ControlPaint.DrawBorder3D(g, rect, Border3DStyle.Etched, Border3DSide.Right);
				break;

			case TabButtonStyle.Button:
				// �{�^�����^�u�R���g���[��
				if (button.IsSelected)
				{
					ControlPaint.DrawBorder3D(g, button.Bounds, Border3DStyle.Sunken);
				}
				else {
					ControlPaint.DrawButton(g,
						button.Bounds, ButtonState.Normal);
				}
				break;
			}
		}

		/// <summary>
		/// ���ׂẴ{�^���� Rectangle ���W���X�V���܂��B
		/// </summary>
		/// <param name="g"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		private void UpdateButtonRect(Graphics g)
		{
			Size borderSize = SystemInformation.Border3DSize;
			Rectangle rect = new Rectangle(borderSize.Width, borderSize.Height,0,0);
			int height = 0;

			foreach (TabButton button in buttons)
			{
				Size size = AutoAdjustButtonSize ?
					GetButtonSize(g, button) : ButtonSize;

				rect.Width = size.Width;
				rect.Height = size.Height;

				height = Math.Max(height, size.Height);

				if (rect.Right > Width)
				{
					if (Wrappable)
					{
						rect.X = borderSize.Width;
						rect.Y += height + 2;
					}
				}

				button.bounds = rect;

				rect.X += size.Width + SystemInformation.Border3DSize.Width * 2;
			}
		}

		/// <summary>
		/// �w�肵���{�^���̃T�C�Y��Ԃ��܂��B
		/// </summary>
		/// <param name="g"></param>
		/// <param name="button"></param>
		/// <returns></returns>
		private Size GetButtonSize(Graphics g, TabButton button)
		{
			Font tempFont = new Font(
				button.IsSelected ? button.ActiveFontFamily : button.InactiveFontFamily, Font.Size,
				button.IsSelected ? button.ActiveFontStyle : button.InactiveFontStyle);

			Size itemSize = g.MeasureString(button.Text, tempFont).ToSize();

			tempFont.Dispose();

			// ������Ƃ����̂œK���ɒ���
			itemSize.Width += 8; 
			itemSize.Height += 5;

			// �A�C�R�������݂���΃A�C�R���T�C�Y�𑫂�
			if (button.ImageIndex != -1 && imageList != null)
			{
				Size imageSize = imageList.ImageSize;
				itemSize.Width += imageSize.Width;
				itemSize.Height = Math.Max(itemSize.Height, imageSize.Height);
			}

			return itemSize;
		}

		/// <summary>
		/// �w�肵�����W�ɗL�� TabButton ���擾���܂��B
		/// ���݂��Ȃ���� null ��Ԃ��܂��B
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public TabButton GetButtonAt(Point pt)
		{
			foreach (TabButton b in buttons)
			{
				if (b.Bounds.Contains(pt))
					return b;
			}
			return null;
		}
	}

	/// <summary>
	/// TabButton �̊O�ς�\���񋓑̂ł��B
	/// </summary>
	public enum TabButtonStyle
	{
		/// <summary>����ł��B</summary>
		Flat,
		/// <summary>�{�^���ł��B</summary>
		Button,
	}
}
