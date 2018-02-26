using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WindowsUtilities;
using System.Runtime.InteropServices;

namespace RebarDotNet
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	
	[ToolboxItem(true),
	DefaultProperty("Bands"),
	Designer(typeof(RebarDotNet.RebarDesigner)),
	DesignTimeVisible(true)]
	public class RebarWrapper : System.Windows.Forms.Control
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.Windows.Forms.ImageList _imageList;
		private NativeRebar _rebar = null;
		private System.ComponentModel.Container components = null;
		private bool _autoSize = true;
		private BandCollection _bands;
		private bool _bandBorders = true;
		private Color _embossHighlight;
		private bool _embossPicture = false;
		private Color _embossShadow;
		private bool _fixedOrder = false;
		private Orientation _orientation = Orientation.Horizontal;
		private bool _showBackgroundImage = true;
		private bool _resizing = false;
		private bool _throwExceptions = true;
		private bool _variantHeight = true;

		/// <summary>
		/// シェブロンの項目がクリックされたときに発生
		/// </summary>
		public event ToolBarButtonClickEventHandler ChevronClick;

		/// <summary>
		/// シェブロンがポップアップされる時に発生
		/// </summary>
		public event EventHandler ChevronPopup;

/*
		public event RebarEventHandler AddBand;
		public event RebarEventHandler RemoveBand;
*/
		public RebarWrapper()
		{
			SetStyle(ControlStyles.StandardClick, true);
			SetStyle(ControlStyles.StandardDoubleClick,true);
			
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			//SetStyle(ControlStyles.ResizeRedraw, true);

			_embossHighlight = SystemColors.ControlLightLight;
			_embossShadow = SystemColors.ControlDark;
			_bands = new BandCollection(this);
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		[Category("Behavior"),
		DefaultValue(true),
		NotifyParentProperty(true)]
		public new bool AutoSize
		{
			get
			{
				return _autoSize;
			}
			set
			{
				if(value != _autoSize)
				{
					_autoSize = value;
					UpdateStyle();
				}
			}
		}

		[Category("Appearance"),
		DefaultValue(typeof(Color), "Control"),
		NotifyParentProperty(true)]
		public override System.Drawing.Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				UpdateColors();
			}	
		}

		[Category("Appearance"),
		DefaultValue(null),
		NotifyParentProperty(true)]
		public new Bitmap BackgroundImage
		{
			get
			{
				return (Bitmap) base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
				foreach(BandWrapper band in _bands)
				{
					if(band.UseCoolbarPicture & band.FixedBackground)band.BackgroundImage = (Bitmap)base.BackgroundImage;

				}
			}
		}

		[Category("Behavior"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(RebarDotNet.BandCollectionEditor), 
		typeof(System.Drawing.Design.UITypeEditor)),
		NotifyParentProperty(true)]
		public BandCollection Bands
		{
			get
			{
				return _bands;
			}
		}

		[Category("Appearance"),
		NotifyParentProperty(true),
		DefaultValue(true)]
		public bool BandBorders
		{
			get
			{
				return _bandBorders;
			}
			set
			{
				if(value != _bandBorders)
				{
					_bandBorders = value;
					UpdateStyle();
				}
			}
		}

		public BandWrapper BandHitTest(Point pt)
		{
			foreach (BandWrapper band in _bands)
			{
				if(band.Bounds.Contains(pt))
					return band;
			}
			return null;
		}

		[Category("Appearance"),
		DefaultValue(typeof(Color), "ControlLightLight"),
		NotifyParentProperty(true)]
		public Color EmbossHighlight
		{
			get
			{
				return _embossHighlight;
			}
			set
			{
				if(value != _embossHighlight)
				{
					//Code to set EmbossHighlight
					_embossHighlight = value;
					UpdateColors();
				}
			}
		}

		[Category("Appearance"),
		DefaultValue(false),
		NotifyParentProperty(true)]
		public bool EmbossPicture
		{
			get
			{
				return _embossPicture;
			}
			set
			{
				if(value != _embossPicture)
				{
					_embossPicture = value;
					UpdateStyle();
				}
			}
		}

		[Category("Appearance"),
		DefaultValue(typeof(Color), "ControlDark"),
		NotifyParentProperty(true)]
		public Color EmbossShadow
		{
			get
			{
				return _embossShadow;
			}
			set
			{
				if(value != _embossShadow)
				{
					//Code to set EmbossShadow
					_embossShadow = value;
					UpdateColors();
				}
			}
		}
		
		[Category("Appearance"),
		DefaultValue(false),
		NotifyParentProperty(true)]
		public bool FixedOrder
		{
			get
			{
				return _fixedOrder;
			}
			set
			{
				if(value != _fixedOrder)
				{
					_fixedOrder = value;
					UpdateStyle();
				}
			}
		}

		[Category("Appearance"),
		DefaultValue(typeof(Color), "ControlText"),
		NotifyParentProperty(true)]
		public override System.Drawing.Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				UpdateColors();
			}
		}

		[Browsable(false),
		System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
		public BandWrapper HitTest(Point pt)
		{//RB_HITTEST
				return null;
		}

		[Category("Appearance"),
		NotifyParentProperty(true),
		DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return _imageList;
			}
			set
			{
				if(value != _imageList)
				{
					_imageList = value;
					UpdateImageList();
				}
			}
		}

		[Category("Behavior"),
		NotifyParentProperty(true),
		DefaultValue(Orientation.Horizontal)]
		public Orientation Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				if(value != _orientation)
				{
					_orientation = value;
					_resizing = true;
					UpdateStyle();
					_resizing = false;
				}
			}
		}
		
			internal NativeRebar Rebar
		{
			get
			{
				return _rebar;
			}
		}

		internal IntPtr RebarHwnd
		{
			get
			{
				if (_rebar == null)
				{
					return IntPtr.Zero;
				}
				else
				{
					return _rebar.Handle;
				}
			}
		}

		[Browsable(false),
		System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
		public int RowCount
		{//RB_GETROWCOUNT
			get
			{
				return 0;
			}
		}

		[Category("Appearance"),
		Browsable(true),
		DefaultValue(true)]
		public bool ShowBackgroundImage
		{
			get
			{
				return _showBackgroundImage;
			}
			set
			{
				if(value != _showBackgroundImage)
				{
					_showBackgroundImage = value;
					foreach(BandWrapper band in _bands)
					{
						if(band.UseCoolbarPicture & band.FixedBackground)band.BackgroundImage = (_showBackgroundImage)?BackgroundImage:null;
					}
				}
			}
		}

		protected int Style
		{
			get
			{
				int style = (int)(WindowsStyleConstants.WS_BORDER | 
					WindowsStyleConstants.WS_CHILD | 
					WindowsStyleConstants.WS_CLIPCHILDREN |
					WindowsStyleConstants.WS_CLIPSIBLINGS | 
					WindowsStyleConstants.CCS_NODIVIDER | 
					WindowsStyleConstants.CCS_NOPARENTALIGN
					//|WindowsStyleConstants.CCS_NORESIZE
					);
				if(_autoSize)
					style |= (int)WindowsStyleConstants.RBS_AUTOSIZE;
				if(_bandBorders)
					style |= (int)WindowsStyleConstants.RBS_BANDBORDERS;
				if(_orientation == Orientation.Vertical)
				{
					style |= (int)WindowsStyleConstants.CCS_LEFT;
				}
				else
				{
					style |= (int)WindowsStyleConstants.CCS_TOP;
				}
				if(this._variantHeight)
					style |= (int)WindowsStyleConstants.RBS_VARHEIGHT;
				if(this.Visible)
					style |= (int)WindowsStyleConstants.WS_VISIBLE;

				return style;
			}
		}

		[Browsable(false),
		EditorBrowsable(EditorBrowsableState.Never)]
		private new string Text
		{
			get
			{
				return base.Text;
			}
		}

		[Category("Behavior"),
		Browsable(true),
		DefaultValue(true)]
		public bool ThrowExceptions
		{
			get
			{
				return _throwExceptions;
			}
			set
			{
				_throwExceptions = value;
			}
		}

		protected void UpdateColors()
		{
			if(Created && _rebar != null)
			{
				COLORSCHEME CSInfo = new COLORSCHEME();
				CSInfo.dwSize = (uint) Marshal.SizeOf(CSInfo);
				CSInfo.clrBtnHighlight = new COLORREF(_embossHighlight);
				CSInfo.clrBtnShadow = new COLORREF(_embossShadow);
				User32Dll.SendMessage(_rebar.Handle, (int)WindowsMessages.RB_SETCOLORSCHEME,0,ref CSInfo);
				
				COLORREF color = new COLORREF(this.ForeColor);
				User32Dll.SendMessage(_rebar.Handle, (int)WindowsMessages.RB_SETTEXTCOLOR, 0, color);
				color = new COLORREF(this.BackColor);
				User32Dll.SendMessage(_rebar.Handle, (int)WindowsMessages.RB_SETBKCOLOR, 0, color);
			}
		}
		
		protected void UpdateStyle()
		{
			if(_rebar != null)
			{
				if(User32Dll.SetWindowLongPtr(_rebar.Handle, (int)WindowLongConstants.GWL_STYLE, (IntPtr)Style).ToInt32() == 0)
				{
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						System.Diagnostics.Debug.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Updating Styles.", ex));
					}
				}
				else
				{
					this.Invalidate();
					this.Refresh();
				}
			}
		}

		protected void UpdateImageList()
		{
			if(_rebar != null)
			{
				REBARINFO RBInfo = new REBARINFO();
				RBInfo.cbSize = (uint)Marshal.SizeOf(RBInfo);
				RBInfo.fMask = (uint)RebarImageListConstants.RBIM_IMAGELIST;
				if(_imageList == null)
				{
					RBInfo.himl = IntPtr.Zero;
				}
				else
				{
					RBInfo.himl = _imageList.Handle;
				}
				if(User32Dll.SendMessage(_rebar.Handle, 
					(int)WindowsMessages.RB_SETBARINFO, 
					0,ref RBInfo)==0)
				{
				
					int LastErr = Marshal.GetHRForLastWin32Error();
					try 
					{
						Marshal.ThrowExceptionForHR(LastErr);
					}
					catch (Exception ex) 
					{
						System.Diagnostics.Debug.WriteLine(LastErr + " " + ex.Message);
						if (_throwExceptions) throw(new Exception("Error Setting Imagelist.", ex));
					}
				}
			}
		}

		[Category("Appearance"),
		Browsable(true),
		DefaultValue(true),
		NotifyParentProperty(true)]
		public bool VariantHeight
		{
			get
			{
				return _variantHeight;
			}
			set
			{
				if(value != _variantHeight)
				{
					//Code to set Style
					_variantHeight= value;
					UpdateStyle();
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( _rebar != null )
			{
				_rebar.DestroyHandle();
			}
			if( _imageList != null )
				_imageList.Dispose();
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ReBarWrapper
			// 
			this.Name = "ReBarWrapper";
			this.Size = new System.Drawing.Size(464, 64);
		}
		#endregion
		
		public void UpdateSize()
		{
			this.OnResize(new EventArgs());
		}
		
		protected override void OnBackColorChanged(System.EventArgs e)
		{
			base.OnBackColorChanged(e);
			UpdateColors();
		}

		protected override void OnClick(System.EventArgs e)
		{
			base.OnClick(e);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			INITCOMMONCONTROLSEX ComCtls = new INITCOMMONCONTROLSEX();
			ComCtls.dwICC = (uint)InitWindowsCommonControlsConstants.ICC_COOL_CLASSES;
			ComCtls.dwSize = (uint)Marshal.SizeOf(ComCtls);
			bool Result = ComCtl32Dll.InitCommonControlsEx(ref ComCtls);
			if(!Result) MessageBox.Show("There was a tragic error.  InitCommControlsEx Failed!");
			else
			{
				
				_rebar = new NativeRebar(User32Dll.CreateWindowEx(
					0U,
					"ReBarWindow32",
					null,
					(uint)Style,
					0,0,this.Width, this.Height,
					this.Handle,
					(IntPtr)1,//This will always be the only child window
					IntPtr.Zero,
					0
					));
				User32Dll.SetWindowLongPtr(_rebar.Handle, (int)WindowLongConstants.GWL_EXSTYLE, (IntPtr)0x80);
				_rebar.WindowPosChanging += new NativeRebarEventHandler(OnWindowPosChanging);
				_rebar.WindowsMessageRecieved += new NativeRebarEventHandler(OnWindowsMessageRecieved);
				UpdateImageList();
				UpdateColors();
				foreach(BandWrapper band in _bands)
				{
					band.CreateBand();
				}
			}
		}

		protected override void OnDoubleClick(System.EventArgs e)
		{
			base.OnDoubleClick(e);
		}

		protected override void OnForeColorChanged(System.EventArgs e)
		{
			base.OnForeColorChanged(e);
			UpdateColors();
		}
		
		protected override void OnHandleCreated(System.EventArgs e)
		{
			base.OnHandleCreated(e);
	
		}

		protected override void OnLayout(System.Windows.Forms.LayoutEventArgs levent)
		{
			base.OnLayout(levent);
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDown(e);
			BandWrapper band = BandHitTest(new Point(e.X, e.Y));
			if (band != null)
			{
				band.OnMouseDown(e);
			}
		}

		protected override void OnMouseEnter(System.EventArgs e)
		{
			base.OnMouseEnter(e);
		}

		protected override void OnMouseHover(System.EventArgs e)
		{
			base.OnMouseHover(e);
		}

		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			BandWrapper band = BandHitTest(new Point(e.X, e.Y));
			if (band != null)
			{
				band.OnMouseMove(e);
			}
		}

		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseUp(e);
			BandWrapper band = BandHitTest(new Point(e.X, e.Y));
			if (band != null)
			{
				band.OnMouseUp(e);
			}
		}

		protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			BandWrapper band = BandHitTest(new Point(e.X, e.Y));
			if (band != null)
			{
				band.OnMouseWheel(e);
			}
		}

		protected override void OnMove(System.EventArgs e)
		{
			base.OnMove(e);
		}

		protected override void OnNotifyMessage(System.Windows.Forms.Message m)
		{
			System.Diagnostics.Debug.WriteLine("Notify Message");
			base.OnNotifyMessage(m);
		}


		protected override void OnResize(System.EventArgs e)
		{
			bool ResizeSetting = _resizing;
			_resizing = true;
			if(_rebar != null)
			{
				if(_orientation == Orientation.Horizontal)
				{
					int height = _rebar.BarHeight;
					if(Height != height)
					{
						Height = height;
					}
					if(Width != _rebar.BarWidth)
					{
						if (User32Dll.MoveWindow(_rebar.Handle, 0, 0, this.Width, Height, true) == 0)
						{
							System.Diagnostics.Debug.WriteLine(Marshal.GetLastWin32Error());
							try
							{
								Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
							}
							catch(Exception ex)
							{
								System.Diagnostics.Debug.WriteLine("Something went wrong resizing the window: " + ex.Message);
								if (_throwExceptions) throw(new Exception("Error Resizing.", ex));
							}
						}
						else
						{
							Refresh();
						}
					}
				}
				else
				{//Vertical
					int width = _rebar.BarWidth;
					if(Width != width)
					{
						Width = width;
					}
					if(Height != _rebar.BarHeight)
					{
						if(User32Dll.MoveWindow(_rebar.Handle, 0, 0, width, this.Height, true) == 0)
						{
							try
							{
								Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
							}
							catch(Exception ex)
							{
								System.Diagnostics.Debug.WriteLine("Something went wrong resizing the window: " + ex.Message);
								if (_throwExceptions) throw(new Exception("Error Updating Styles.", ex));
							
							}
						}
						else
						{
							Refresh();
						}
					}
				}	
			}
			base.OnResize(e);
			_resizing = ResizeSetting;
		}

		internal void OnWindowPosChanging(object sender, NativeRebarEventArgs e)
		{
			if(_resizing) return;
			if(_orientation == Orientation.Horizontal)
			{
				//System.Diagnostics.Debug.WriteLine(_rebar.BarHeight);
				if(Height != _rebar.BarHeight)
				{
					this.OnResize(new EventArgs());
				}
			}
			else
			{ //Vertical
				if(Width != _rebar.BarWidth)
				{
					this.OnResize(new EventArgs());
				}
			}
		}

		internal void OnWindowsMessageRecieved(object sender, NativeRebarEventArgs e)
		{
			//Send mouse messages to the parent window
			if(e.m.Msg == (int)WindowsMessages.WM_LBUTTONDBLCLK |
				e.m.Msg == (int) WindowsMessages.WM_LBUTTONDOWN |
				e.m.Msg == (int) WindowsMessages.WM_LBUTTONUP |
				e.m.Msg == (int) WindowsMessages.WM_MBUTTONDBLCLK |
				e.m.Msg == (int) WindowsMessages.WM_MBUTTONDOWN |
				e.m.Msg == (int) WindowsMessages.WM_MBUTTONUP |	
				e.m.Msg == (int)WindowsMessages.WM_RBUTTONDBLCLK |
				e.m.Msg == (int) WindowsMessages.WM_RBUTTONDOWN |
				e.m.Msg == (int) WindowsMessages.WM_RBUTTONUP |
				e.m.Msg == (int) WindowsMessages.WM_MOUSEHOVER |
				e.m.Msg == (int) WindowsMessages.WM_MOUSEMOVE |
				e.m.Msg == (int) WindowsMessages.WM_MOUSEWHEEL)
			{
				User32Dll.SendMessage(this.Handle, e.m.Msg, e.m.WParam, e.m.LParam);
			}
		}

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if(_rebar != null && m.Msg == (int)WindowsMessages.WM_NOTIFY)
			{
				NMHDR Notify = (NMHDR)Marshal.PtrToStructure(m.LParam,typeof(NMHDR));

				if(Notify.idFrom == 1)
				{
					switch((int)Notify.code)
					{
						// 2003/08/29
						// pluto@utyuu.zzn.com
						case ((int)WindowsNotifyConstants.RBN_CHEVRONPUSHED):
						{
							NMREBARCHEVRON chevron = (NMREBARCHEVRON)Marshal.PtrToStructure(m.LParam, typeof(NMREBARCHEVRON));
							ShowChevron(chevron, new EventHandler(ChevronButton_Click));
							m.Result = new IntPtr(0);
							return;
						}
					
						case ((int)WindowsNotifyConstants.RBN_LAYOUTCHANGED):
						{
							//System.Diagnostics.Debug.WriteLine("Layout Changed");
							break;
						}
						case ((int)WindowsNotifyConstants.RBN_AUTOSIZE):
						{
							//System.Diagnostics.Debug.WriteLine("Autosized");
							break;
						}
						case ((int)WindowsNotifyConstants.RBN_BEGINDRAG):
						{
							//System.Diagnostics.Debug.WriteLine("Begin Drag");
							break;
						}
						case ((int)WindowsNotifyConstants.RBN_ENDDRAG):
						{
							//System.Diagnostics.Debug.WriteLine("End Drag");
							break;
						}
						case ((int)WindowsNotifyConstants.RBN_DELETEDBAND):
						{
							//System.Diagnostics.Debug.WriteLine("Delete band");
							break;
						}
						case ((int)WindowsNotifyConstants.RBN_DELETINGBAND):
						{
							//System.Diagnostics.Debug.WriteLine("Deleting Band");
							break;
						}
						case ((int)WindowsNotifyConstants.RBN_CHILDSIZE):
						{
							NMREBARCHILDSIZE ChildSize = (NMREBARCHILDSIZE)Marshal.PtrToStructure(m.LParam,typeof(NMREBARCHILDSIZE));
							foreach(BandWrapper band in _bands)
							{
								if (band.ID == ChildSize.wID)
								{
									band.OnResize(new EventArgs());
									continue;
								}
							}
							//System.Diagnostics.Debug.WriteLine("Child Sized");
							break;
						}

						default:
						{
							//System.Diagnostics.Debug.WriteLine("Other Notify code recieved " + Notify.code);
							break;
						}
					}
				}
				//System.Diagnostics.Debug.WriteLine("Control WndProc Notified");
			}
			base.WndProc(ref m);
		}

		internal class NativeRebar: NativeWindow
		{
			public event NativeRebarEventHandler WindowPosChanging;
			public event NativeRebarEventHandler WindowsMessageRecieved;

			internal NativeRebar(IntPtr handle)
			{
				base.AssignHandle(handle);
			}

			internal int BarHeight
			{
				get
				{
					RECT rect = new RECT();
					User32Dll.GetWindowRect(Handle, ref rect);
					return rect.bottom - rect.top;
					//return (int)User32Dll.SendMessage(Handle,(int)WindowsMessages.RB_GETBARHEIGHT, 0U, 0U);
				}
			}

			internal int BarWidth
			{
				get
				{
					RECT rect = new RECT();
					User32Dll.GetWindowRect(Handle, ref rect);
					return rect.right - rect.left;
				}
			}

			internal bool WidthHeightMatch(int Width, int Height)
			{
				return (Height == BarHeight && Width == BarWidth);
			}

			protected override void WndProc(ref System.Windows.Forms.Message m)
			{
				if(m.Msg == (int)WindowsMessages.WM_WINDOWPOSCHANGING)
				{
					OnWindowPosChanging(new NativeRebarEventArgs(m));
				}
				//System.Diagnostics.Debug.WriteLine("Message: " + m.Msg);
				OnWindowsMessageRecieved(new NativeRebarEventArgs(m));
				this.DefWndProc(ref m);
				
			}

			protected virtual void OnWindowPosChanging(NativeRebarEventArgs e)
			{
				if (WindowPosChanging != null)
				{
					WindowPosChanging(this, e);
				}
			}

			protected virtual void OnWindowsMessageRecieved(NativeRebarEventArgs e)
			{
				if (WindowsMessageRecieved != null)
				{
					WindowsMessageRecieved(this, e);
				}
			}
		}

		// =============================================================
		// 2003/08/29
		// pluto@utyuu.zzn.com

		/// <summary>
		/// ChevronMenuItemクラス
		/// </summary>
		private class ChevronMenuItem : MenuItem
		{
			private readonly BandWrapper band;
			private readonly ToolBarButton button;

			/// <summary>
			/// シェブロンを表示しているバンドを取得
			/// </summary>
			public BandWrapper Band {
				get {
					return band;
				}
			}

			/// <summary>
			/// クリックされたボタンを取得
			/// </summary>
			public ToolBarButton Button {
				get {
					return button;
				}
			}

			/// <summary>
			/// ChevronMenuItemクラスのインスタンスを初期化
			/// </summary>
			/// <param name="text">メニュー項目のキャプション</param>
			/// <param name="onClick">クリックされたときに呼ばれるハンドラ</param>
			/// <param name="band">シェブロンを表示しているBandWrapperクラス</param>
			/// <param name="button">クリックされたToolBarButtonクラス</param>
			public ChevronMenuItem(BandWrapper band, ToolBarButton button)
			{
				this.band = band;
				this.button = button;
			}
		}

		// シェブロンを表示しているBandWrapperクラスを一時的に格納する変数
		private BandWrapper chevronBand;

		/// <summary>
		/// シェブロンを表示
		/// 参考URL: http://www.kumei.jp/c_lang/sdk4/sdk_308.htm
		/// </summary>
		/// <param name="chevron">NMREBARCHEVRON構造体</param>
		private void ShowChevron(NMREBARCHEVRON chevron, EventHandler onClick)
		{
			try {
				BandWrapper band = null;

				foreach (BandWrapper b in Bands)
				{
					if (b.ID == chevron.wID)
					{
						band = b;
						break;
					}
				}

				if (band.Child is ToolBar)
				{
					ToolBar toolbar = (ToolBar)band.Child;
					ToolBarButton hideButton = null;

					// 隠されているボタンを調べる
					foreach (ToolBarButton button in toolbar.Buttons)
					{
						// ツールバーボタンの矩形
						Rectangle rcbtn = toolbar.RectangleToScreen(button.Rectangle);

						// バンドの矩形
						Rectangle rcband = this.RectangleToScreen(band.Bounds);

						// ２つの重なり合う矩形を取得
						Rectangle result = Rectangle.Intersect(rcbtn, rcband);

						if (!result.Equals(rcbtn))
						{
							hideButton = button;
							break;
						}
					}

					if (hideButton != null)
					{
						// 隠されているボタンのポップアップメニューを作成
						ContextMenu context = new ContextMenu();
						Point location = PointToClient(MousePosition);
						
						// シェブロンの項目開始位置
						int index = toolbar.Buttons.IndexOf(hideButton);

						for (int i = index; i < toolbar.Buttons.Count; i++)
						{
							ToolBarButton tbb = toolbar.Buttons[i];

							if (tbb.Style == ToolBarButtonStyle.Separator)
							{
								MenuItem separator = new MenuItem("-");
								context.MenuItems.Add(separator);
							}
							else {
								string text = tbb.Text != "" ? tbb.Text : tbb.ToolTipText;

								MenuItem item = new ChevronMenuItem(band, tbb);
								item.Text = text;
								item.Click += onClick;
								item.Visible = tbb.Visible;
								item.Checked = tbb.Pushed;
								item.Enabled = tbb.Enabled;

								if (tbb.DropDownMenu != null)
									AddMenuItems(item, tbb.DropDownMenu.MenuItems);

								context.MenuItems.Add(item);
							}
						}

						chevronBand = band;
						context.Popup += new EventHandler(OnChevronPopup);
						context.Show(this, location);
					}
				}

				chevronBand = null;
			}
			catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
		}

		private void AddMenuItems(MenuItem parent, Menu.MenuItemCollection menuItems)
		{
			foreach (MenuItem menu in menuItems)
			{
				MenuItem item = menu.CloneMenu();
				parent.MenuItems.Add(item);
			}
		}

		// シェブロンで表示された項目がクリックされた時に発生
		private void ChevronButton_Click(object sender, EventArgs e)
		{
			ChevronMenuItem menu = sender as ChevronMenuItem;
			
			if (menu != null)
				OnChevronClick(menu.Band, menu.Button);
		}

		/// <summary>
		/// ChevronPopupイベントを発生させる
		/// </summary>
		private void OnChevronPopup(object sender, EventArgs e)
		{
			if (ChevronPopup != null)
				ChevronPopup(chevronBand, new EventArgs());
		}

		/// <summary>
		/// ChevronClickイベントを発生させる
		/// </summary>
		/// <param name="band">シェブロンを表示しているバンド</param>
		/// <param name="button">クリックされたツールバーボタン</param>
		private void OnChevronClick(BandWrapper band, ToolBarButton button)
		{
			if (ChevronClick != null)
				ChevronClick(band, new ToolBarButtonClickEventArgs(button));
		}
		// =============================================================
	}

	internal class NativeRebarEventArgs: EventArgs
	{
		Message _m;

		public NativeRebarEventArgs(Message m)
		{
			_m = m;
		}

		public Message m
		{
			get
			{
				return _m;
			}
		}
	}

	internal delegate void NativeRebarEventHandler(object sender, NativeRebarEventArgs e);
}
