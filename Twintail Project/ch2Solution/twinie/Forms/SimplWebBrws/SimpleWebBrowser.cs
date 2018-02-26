// SimpleWebBrowser.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.IO;
	using AxSHDocVw;
	using SHDocVw;
	using mshtml;
	using CSharpSamples;
	using CSharpSamples.Winapi;

	/// <summary>
	/// シンプルなブラウザ
	/// </summary>
	public class SimpleWebBrowser : System.Windows.Forms.Form
	{
		public const int WM_NEWINSTANCE = WinApi.WM_APP + 1;

		private readonly string SettingPath = Path.Combine(
			Application.StartupPath, "twinweb.ini");

		private SmoothProgressBar progress;
		private Options opt;
//		private bool disposed;

		#region Designer Fields
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolBarButton toolBarButtonBack;
		private System.Windows.Forms.ToolBarButton toolBarButtonForward;
		private System.Windows.Forms.ToolBarButton toolBarButtonFav;
		private System.Windows.Forms.ToolBarButton toolBarButtonHome;
		private System.Windows.Forms.ToolBarButton toolBarButtonPaste;
		private System.Windows.Forms.ToolBarButton toolBarButtonRefresh;
		private System.Windows.Forms.ToolBarButton toolBarButtonOption;
		private System.Windows.Forms.ToolBarButton toolBarButtonPrint;
		private System.Windows.Forms.ToolBarButton toolBarButtonCopy;
		private System.Windows.Forms.ToolBarButton toolBarButtonStop;
		private System.Windows.Forms.ToolBarButton toolBarButtonGoogle;
		private RebarDotNet.RebarWrapper rebarWrapper;
		private System.Windows.Forms.Panel addrPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxAddress;
		private RebarDotNet.BandWrapper bandWrapperToolBar;
		private RebarDotNet.BandWrapper bandWrapperAddress;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep1;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep3;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep4;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep2;
		private System.Windows.Forms.StatusBarPanel statusBarPanelText;
		private System.Windows.Forms.StatusBarPanel statusBarPanelProg;
		private System.Windows.Forms.ToolBarButton toolBarButtonFind;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemClose;
		private System.Windows.Forms.MenuItem menuItemClose2;
		private System.Windows.Forms.MenuItem menuItemCloseAll;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep5;
		private System.ComponentModel.IContainer components;
		#endregion

		#region InnerClass WebTabPage
		private class WebTabPage : TabPage
		{
			private AxWebBrowser wb;

			/// <summary>
			/// ブラウザを取得
			/// </summary>
			public AxWebBrowser WebBrowser {
				get { return wb; }
			}

			public WebTabPage(SimpleWebBrowser form)
			{
				wb = new AxWebBrowser();
				wb.BeginInit();
				wb.Enabled = true;
				wb.ImeMode = 0;
				wb.TabIndex = 0;
				wb.Dock = DockStyle.Fill;
				Controls.Add(wb);
				wb.EndInit();

				object ocx = wb.GetOcx();
				SHDocVw.WebBrowser_V1 webv1 = ocx as SHDocVw.WebBrowser_V1;
				if (webv1 != null)
					webv1.BeforeNavigate += new SHDocVw.DWebBrowserEvents_BeforeNavigateEventHandler(form.OnBeforeNavigate);

				wb.StatusTextChange += new AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler(form.OnStatusTextChange);
				wb.ProgressChange += new AxSHDocVw.DWebBrowserEvents2_ProgressChangeEventHandler(form.OnProgressChange);
				wb.NewWindow2 += new AxSHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(form.OnNewWindow);
				wb.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(form.OnNavigateComplete);
				wb.TitleChange += new AxSHDocVw.DWebBrowserEvents2_TitleChangeEventHandler(form.OnTitleChange);
				wb.CommandStateChange += new AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEventHandler(form.OnCommandStateChange);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					wb.Dispose();
				}

				base.Dispose(disposing);
			}
		}
		#endregion

		/// <summary>
		/// ブラウザが選択されているかどうかを判断
		/// </summary>
		public bool IsSelected {
			get {
				WebTabPage tab = tabControl.SelectedTab as WebTabPage;
				return (tab != null) ? true : false;
			}
		}

		/// <summary>
		/// 選択されているブラウザを取得
		/// </summary>
		public AxWebBrowser WebBrowser {
			get {
				if (IsSelected)
				{
					WebTabPage tab = (WebTabPage)tabControl.SelectedTab;
					return tab.WebBrowser;
				}
				// 選択されていなければ新しく作成
				else {
					return CreateWebBrowser();
				}
			}
		}

		/// <summary>
		/// SimpleWebBrowserクラスのインスタンスを初期化
		/// </summary>
		public SimpleWebBrowser()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			opt = new Options();
			progress = new SmoothProgressBar();
			progress.TextStyle = ProgressTextStyle.None;
			statusBar.Controls.Add(progress);

			// 設定を読み込む
			CSPrivateProfile pp = new CSPrivateProfile();
			pp.Read(SettingPath);

			Point pt = new Point(
				pp.GetInt("Window", "X", Point.Empty.X),
				pp.GetInt("Window", "Y", Point.Empty.Y));

			Size sz = new Size(
				pp.GetInt("Window", "Width", Size.Empty.Width),
				pp.GetInt("Window", "Height", Size.Empty.Height));

			if (!pt.IsEmpty) Location = pt;
			if (!sz.IsEmpty) ClientSize = sz;

			opt.SearchUri = pp.GetString("Options", "SearchUri", opt.SearchUri);
			opt.Activate = pp.GetBool("Options", "Activate", opt.Activate);
			opt.Activation = (Activation)Enum.Parse(typeof(Activation), pp.GetString("Options", "Activation", opt.Activation.ToString()));
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				foreach (WebTabPage tab in tabControl.TabPages)
					tab.WebBrowser.Dispose();

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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SimpleWebBrowser));
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonBack = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonForward = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonRefresh = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonHome = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonFav = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep3 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCopy = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPaste = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep4 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonFind = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonGoogle = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep5 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPrint = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonOption = new System.Windows.Forms.ToolBarButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.tabControl = new System.Windows.Forms.TabControl();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemClose = new System.Windows.Forms.MenuItem();
			this.menuItemClose2 = new System.Windows.Forms.MenuItem();
			this.menuItemCloseAll = new System.Windows.Forms.MenuItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarPanelText = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanelProg = new System.Windows.Forms.StatusBarPanel();
			this.rebarWrapper = new RebarDotNet.RebarWrapper();
			this.bandWrapperToolBar = new RebarDotNet.BandWrapper();
			this.bandWrapperAddress = new RebarDotNet.BandWrapper();
			this.addrPanel = new System.Windows.Forms.Panel();
			this.textBoxAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelProg)).BeginInit();
			this.rebarWrapper.SuspendLayout();
			this.addrPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolBar
			// 
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.AutoSize = false;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.toolBarButtonBack,
																					   this.toolBarButtonForward,
																					   this.toolBarButtonSep1,
																					   this.toolBarButtonStop,
																					   this.toolBarButtonRefresh,
																					   this.toolBarButtonHome,
																					   this.toolBarButtonSep2,
																					   this.toolBarButtonFav,
																					   this.toolBarButtonSep3,
																					   this.toolBarButtonCopy,
																					   this.toolBarButtonPaste,
																					   this.toolBarButtonSep4,
																					   this.toolBarButtonFind,
																					   this.toolBarButtonGoogle,
																					   this.toolBarButtonSep5,
																					   this.toolBarButtonPrint,
																					   this.toolBarButtonOption});
			this.toolBar.Divider = false;
			this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(9, 2);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(435, 22);
			this.toolBar.TabIndex = 0;
			this.toolBar.Wrappable = false;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// toolBarButtonBack
			// 
			this.toolBarButtonBack.ImageIndex = 0;
			this.toolBarButtonBack.ToolTipText = "前に戻る";
			// 
			// toolBarButtonForward
			// 
			this.toolBarButtonForward.ImageIndex = 1;
			this.toolBarButtonForward.ToolTipText = "次に進む";
			// 
			// toolBarButtonSep1
			// 
			this.toolBarButtonSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonStop
			// 
			this.toolBarButtonStop.ImageIndex = 11;
			this.toolBarButtonStop.ToolTipText = "読み込みを中止";
			// 
			// toolBarButtonRefresh
			// 
			this.toolBarButtonRefresh.ImageIndex = 14;
			this.toolBarButtonRefresh.ToolTipText = "最新の状態に更新";
			// 
			// toolBarButtonHome
			// 
			this.toolBarButtonHome.ImageIndex = 7;
			this.toolBarButtonHome.ToolTipText = "ホームへ";
			// 
			// toolBarButtonSep2
			// 
			this.toolBarButtonSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonFav
			// 
			this.toolBarButtonFav.ImageIndex = 6;
			this.toolBarButtonFav.ToolTipText = "お気に入り";
			// 
			// toolBarButtonSep3
			// 
			this.toolBarButtonSep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonCopy
			// 
			this.toolBarButtonCopy.ImageIndex = 4;
			this.toolBarButtonCopy.ToolTipText = "選択テキストをクリップボードにコピー";
			// 
			// toolBarButtonPaste
			// 
			this.toolBarButtonPaste.ImageIndex = 9;
			this.toolBarButtonPaste.ToolTipText = "クリップボードの内容を貼り付ける";
			// 
			// toolBarButtonSep4
			// 
			this.toolBarButtonSep4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonFind
			// 
			this.toolBarButtonFind.ImageIndex = 10;
			this.toolBarButtonFind.ToolTipText = "ページ内を検索";
			// 
			// toolBarButtonGoogle
			// 
			this.toolBarButtonGoogle.ImageIndex = 17;
			this.toolBarButtonGoogle.ToolTipText = "選択文字列をGoogleで検索";
			// 
			// toolBarButtonSep5
			// 
			this.toolBarButtonSep5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonPrint
			// 
			this.toolBarButtonPrint.ImageIndex = 16;
			this.toolBarButtonPrint.ToolTipText = "現在のページを印刷";
			// 
			// toolBarButtonOption
			// 
			this.toolBarButtonOption.ImageIndex = 15;
			this.toolBarButtonOption.ToolTipText = "インターネットオプション";
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// tabControl
			// 
			this.tabControl.ContextMenu = this.contextMenu;
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl.ItemSize = new System.Drawing.Size(85, 17);
			this.tabControl.Location = new System.Drawing.Point(0, 58);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(452, 251);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl.TabIndex = 1;
			this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
			this.tabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl_MouseDown);
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuItemClose,
																						this.menuItemClose2,
																						this.menuItemCloseAll});
			// 
			// menuItemClose
			// 
			this.menuItemClose.Index = 0;
			this.menuItemClose.Shortcut = System.Windows.Forms.Shortcut.F4;
			this.menuItemClose.Text = "閉じる(&C)";
			this.menuItemClose.Click += new System.EventHandler(this.menuItemClose_Click);
			// 
			// menuItemClose2
			// 
			this.menuItemClose2.Index = 1;
			this.menuItemClose2.Shortcut = System.Windows.Forms.Shortcut.ShiftF4;
			this.menuItemClose2.Text = "これ以外のタブを閉じる(&E)";
			this.menuItemClose2.Click += new System.EventHandler(this.menuItemClose2_Click);
			// 
			// menuItemCloseAll
			// 
			this.menuItemCloseAll.Index = 2;
			this.menuItemCloseAll.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF4;
			this.menuItemCloseAll.Text = "すべて閉じる(&A)";
			this.menuItemCloseAll.Click += new System.EventHandler(this.menuItemCloseAll_Click);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 309);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarPanelText,
																						 this.statusBarPanelProg});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(452, 16);
			this.statusBar.TabIndex = 2;
			this.statusBar.DrawItem += new System.Windows.Forms.StatusBarDrawItemEventHandler(this.statusBar_DrawItem);
			// 
			// statusBarPanelText
			// 
			this.statusBarPanelText.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanelText.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
			this.statusBarPanelText.Width = 356;
			// 
			// statusBarPanelProg
			// 
			this.statusBarPanelProg.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
			this.statusBarPanelProg.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanelProg.Width = 80;
			// 
			// rebarWrapper
			// 
			this.rebarWrapper.Bands.Add(this.bandWrapperToolBar);
			this.rebarWrapper.Bands.Add(this.bandWrapperAddress);
			this.rebarWrapper.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.toolBar,
																					   this.addrPanel});
			this.rebarWrapper.Dock = System.Windows.Forms.DockStyle.Top;
			this.rebarWrapper.Name = "rebarWrapper";
			this.rebarWrapper.Size = new System.Drawing.Size(452, 58);
			this.rebarWrapper.TabIndex = 3;
			this.rebarWrapper.Text = "rebarWrapper1";
			// 
			// bandWrapperToolBar
			// 
			this.bandWrapperToolBar.Child = this.toolBar;
			this.bandWrapperToolBar.GripSettings = RebarDotNet.GripperSettings.Always;
			this.bandWrapperToolBar.Header = -1;
			this.bandWrapperToolBar.IdealWidth = -1;
			this.bandWrapperToolBar.Integral = 1;
			this.bandWrapperToolBar.Key = "ToolBar";
			this.bandWrapperToolBar.MaxHeight = 0;
			this.bandWrapperToolBar.MinHeight = 22;
			this.bandWrapperToolBar.UseChevron = false;
			this.bandWrapperToolBar.Width = 448;
			// 
			// bandWrapperAddress
			// 
			this.bandWrapperAddress.Child = this.addrPanel;
			this.bandWrapperAddress.GripSettings = RebarDotNet.GripperSettings.Always;
			this.bandWrapperAddress.Header = -1;
			this.bandWrapperAddress.IdealWidth = -1;
			this.bandWrapperAddress.Integral = 1;
			this.bandWrapperAddress.Key = "Address";
			this.bandWrapperAddress.MaxHeight = 0;
			this.bandWrapperAddress.MinHeight = 22;
			this.bandWrapperAddress.UseChevron = false;
			this.bandWrapperAddress.Width = 448;
			// 
			// addrPanel
			// 
			this.addrPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.textBoxAddress,
																					this.label1});
			this.addrPanel.Location = new System.Drawing.Point(9, 30);
			this.addrPanel.Name = "addrPanel";
			this.addrPanel.Size = new System.Drawing.Size(435, 22);
			this.addrPanel.TabIndex = 4;
			// 
			// textBoxAddress
			// 
			this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxAddress.Location = new System.Drawing.Point(64, 1);
			this.textBoxAddress.Name = "textBoxAddress";
			this.textBoxAddress.Size = new System.Drawing.Size(368, 19);
			this.textBoxAddress.TabIndex = 1;
			this.textBoxAddress.Text = "";
			this.textBoxAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxAddress_KeyPress);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "アドレス(&D)";
			// 
			// SimpleWebBrowser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(452, 325);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl,
																		  this.rebarWrapper,
																		  this.statusBar});
			this.Name = "SimpleWebBrowser";
			this.Text = "SimpleWebBrowser";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SimpleWebBrowser_Closing);
			this.Load += new System.EventHandler(this.SimpleWebBrowser_Load);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelProg)).EndInit();
			this.rebarWrapper.ResumeLayout(false);
			this.addrPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region AxWebBrowser Events
		private void OnBeforeNavigate(string uRL,
				int Flags, string TargetFrameName, ref object PostData, string Headers, ref bool Processed)
		{
			textBoxAddress.Text = uRL;
		}

		private void OnStatusTextChange(object sender, AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEvent e)
		{
			statusBarPanelText.Text = e.text;
		}

		private void OnTitleChange(object sender, AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
		{
			AxWebBrowser web = (AxWebBrowser)sender;
			((WebTabPage)web.Tag).Text = e.text;
		}

		private void OnCommandStateChange(object sender, AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEvent e)
		{
			if (e.command == (int)CommandStateChangeConstants.CSC_NAVIGATEBACK)
			{
				toolBarButtonBack.Enabled = e.enable;
			}
			else if (e.command == (int)CommandStateChangeConstants.CSC_NAVIGATEFORWARD)
			{
				toolBarButtonForward.Enabled = e.enable;
			}
			else if (e.command == (int)CommandStateChangeConstants.CSC_UPDATECOMMANDS)
			{
			}
		}

		private void OnNavigateComplete(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			
		}

		private void OnNewWindow(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow2Event e)
		{
			AxWebBrowser wb = CreateWebBrowser();
			e.ppDisp = wb.Application;
		}

		private void OnProgressChange(object sender, AxSHDocVw.DWebBrowserEvents2_ProgressChangeEvent e)
		{
			if (WebBrowser.Equals(sender))
			{
				progress.Maximum = e.progressMax;
				progress.Position = e.progress;
			}
		}
		#endregion

		#region Form Events
		private void SimpleWebBrowser_Load(object sender, System.EventArgs e)
		{
			UpdateToolBar();
		}

		private void SimpleWebBrowser_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// 設定を保存
			CSPrivateProfile pp = new CSPrivateProfile();
			pp.Read(SettingPath);
			pp.SetValue("Window", "X", Location.X);
			pp.SetValue("Window", "Y", Location.Y);
			pp.SetValue("Window", "Width", ClientSize.Width);
			pp.SetValue("Window", "Height", ClientSize.Height);
			pp.Write(SettingPath);
		}

		private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			TabControl control = (TabControl)sender;
			TabPage tab = control.TabPages[e.Index];

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags = StringFormatFlags.NoWrap;

			if (e.State == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(
					SystemBrushes.Control, e.Bounds);

				e.Graphics.DrawString(tab.Text, e.Font, 
					SystemBrushes.ControlText, e.Bounds, format);
			}
			else {
				e.Graphics.FillRectangle(
					SystemBrushes.ControlDark, e.Bounds);

				e.Graphics.DrawString(tab.Text, e.Font, 
					SystemBrushes.ControlLightLight, e.Bounds, format);
			}
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonBack)
			{
				WebBrowser.GoBack();
			}
			else if (e.Button == toolBarButtonForward)
			{
				WebBrowser.GoForward();
			}
			else if (e.Button == toolBarButtonStop)
			{
				WebBrowser.Stop();
			}
			else if (e.Button == toolBarButtonRefresh)
			{
				object o = null;
				WebBrowser.Refresh2(ref o);
			}
			else if (e.Button == toolBarButtonHome)
			{
				WebBrowser.GoHome();
			}
			else if (e.Button == toolBarButtonFav)
			{
				Favorites();
			}
			else if (e.Button == toolBarButtonCopy)
			{
				Copy();
			}
			else if (e.Button == toolBarButtonPaste)
			{
				Paste();
			}
			else if (e.Button == toolBarButtonGoogle)
			{
				GoogleSearch();
			}
			else if (e.Button == toolBarButtonFind)
			{
				Find();
			}
			else if (e.Button == toolBarButtonPrint)
			{
				Print();
			}
			else if (e.Button == toolBarButtonOption)
			{
				InternetOption();
			}
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (IsSelected)
				textBoxAddress.Text = WebBrowser.LocationURL;

			UpdateToolBar();
		}

		private void textBoxAddress_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				e.Handled = true;
				OpenUri(textBoxAddress.Text, (ModifierKeys & Keys.Shift) != 0);
			}
		}

		private void statusBar_DrawItem(object sender, System.Windows.Forms.StatusBarDrawItemEventArgs sbdevent)
		{
			if (sbdevent.Panel == statusBarPanelProg)
			{
				Rectangle rc = sbdevent.Bounds;
				progress.SetBounds(rc.X, rc.Y - 2, rc.Width, rc.Height + 4);
			}
		}
		
		private void menuItemClose_Click(object sender, System.EventArgs e)
		{
			CloseWebBrowser();
		}

		private void menuItemClose2_Click(object sender, System.EventArgs e)
		{
			WebTabPage tab = (WebTabPage)tabControl.SelectedTab;
			tabControl.Select();

			for (int i = 0; i < tabControl.TabCount;)
			{
				if (tab == tabControl.TabPages[i]) i++;
				else tabControl.TabPages.RemoveAt(i);
			}
		}

		private void menuItemCloseAll_Click(object sender, System.EventArgs e)
		{
			CloseAll();
		}

		private void tabControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			WebTabPage cur = GetTabPage(e.X, e.Y);

			if (cur != null)
			{
				if (e.Button == MouseButtons.Left && e.Clicks > 1)
				{
					object o = null;
					cur.WebBrowser.Refresh2(ref o);
				}
				else if (e.Button == MouseButtons.Middle)
				{
					CloseWebBrowser(cur);
				}
			}
		}
		#endregion

		#region WndProc
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
			case WM_NEWINSTANCE:
				{
					int atom = m.WParam.ToInt32();
					GlobalAtom.Delete(atom);

					string[] args = GlobalAtom.Get(atom, 1024).Split('|');

					foreach (string uri in args)
						OpenUri(uri, true);

					m.Result = (IntPtr)0;
				}
				break;

			default:
				base.WndProc(ref m);
				break;
			}
		}
		#endregion

		#region Private
		private AxWebBrowser CreateWebBrowser()
		{
			WebTabPage tab = new WebTabPage(this);

			tab.Tag = tab.WebBrowser;
			tab.WebBrowser.Tag = tab;

			tabControl.TabPages.Add(tab);

			if (opt.Activate)
				tabControl.SelectedTab = tab;

			return tab.WebBrowser;
		}

		private WebTabPage GetTabPage(int x, int y)
		{
			for (int i = 0; i < tabControl.TabCount; i++)
			{
				if (tabControl.GetTabRect(i).Contains(x, y))
					return (WebTabPage)tabControl.TabPages[i];
			}
			return null;
		}

		private void UpdateToolBar()
		{
			foreach (ToolBarButton b in toolBar.Buttons)
				b.Enabled = IsSelected;

			toolBarButtonHome.Enabled =
				toolBarButtonFav.Enabled =
				toolBarButtonOption.Enabled = true;

			if (IsSelected)
				WebBrowser.QueryStatusWB(OLECMDID.OLECMDID_UPDATECOMMANDS);
		}

		/// <summary>
		/// 指定したタブを閉じる
		/// </summary>
		/// <param name="tab"></param>
		private void CloseWebBrowser(WebTabPage tab)
		{
			int index = Math.Min(tabControl.TabPages.IndexOf(tab),
				tabControl.TabCount-2);

			tabControl.Select();
			tabControl.TabPages.Remove(tab);

			if (opt.Activation == Activation.Right && tabControl.TabCount > 0)
				tabControl.SelectedIndex = index;

			tab.WebBrowser.Dispose();
			tab.Dispose();
		}
		#endregion

		/// <summary>
		/// uriをブラウザで開く
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="newWindow">新しいウインドウを開くかどうか</param>
		public void OpenUri(string uri, bool newWindow)
		{
			object o = null;
			AxWebBrowser web = null;

			web = newWindow ? CreateWebBrowser() : WebBrowser;
			web.Navigate(uri, ref o, ref o, ref o, ref o);

			if (! Visible)
				Show();
		}

		/// <summary>
		/// アクティブなブラウザを閉じる
		/// </summary>
		public void CloseWebBrowser()
		{
			if (IsSelected)
			{
				WebTabPage tab = (WebTabPage)tabControl.SelectedTab;
				CloseWebBrowser(tab);
			}
		}

		/// <summary>
		/// すべてのブラウザを閉じる
		/// </summary>
		public void CloseAll()
		{
			foreach (WebTabPage tab in tabControl.TabPages)
				tab.Dispose();

			tabControl.Select();
			tabControl.TabPages.Clear();
		}

		/// <summary>
		/// 選択文字列をGoogleで検索
		/// </summary>
		public void GoogleSearch()
		{
			if (IsSelected)
			{
				HTMLDocument doc = (HTMLDocument)WebBrowser.Document;
				IHTMLTxtRange range = (IHTMLTxtRange)doc.selection.createRange();

				string uri = String.Format("http://www.google.co.jp/search?q={0}&lr=lang_ja", range.text);
				OpenUri(uri, true);
			}
		}

		/// <summary>
		/// ページ内を検索
		/// </summary>
		public void Find()
		{
			if (IsSelected)
			{}
		}

		/// <summary>
		/// お気に入りメニューを表示
		/// </summary>
		public void Favorites()
		{
		}

		/// <summary>
		/// 選択されている文字列をコピー
		/// </summary>
		public void Copy()
		{
			if (IsSelected) {
				object o = null;
				WebBrowser.ExecWB(OLECMDID.OLECMDID_COPY, OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
			}
		}

		/// <summary>
		/// クリップボードの内容を貼り付ける
		/// </summary>
		public void Paste()
		{
			if (IsSelected) {
				object o = null;
				WebBrowser.ExecWB(OLECMDID.OLECMDID_PASTE, OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
			}
		}

		/// <summary>
		/// ページを印刷する
		/// </summary>
		public void Print()
		{
			if (IsSelected) {
				object o = null;
				WebBrowser.ExecWB(OLECMDID.OLECMDID_PRINT, OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
			}
		}

		/// <summary>
		/// インターネットオプションを開く
		/// </summary>
		public void InternetOption()
		{
			System.Diagnostics.Process.Start("inetcpl.cpl");
		}
	}
}
