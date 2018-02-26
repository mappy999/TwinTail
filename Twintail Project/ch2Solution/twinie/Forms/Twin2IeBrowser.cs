// Twin2IeBrowser.cs

namespace Twin.Forms
{
	using System;
	using System.IO;
	using System.Data;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Drawing;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Diagnostics;
	using System.Net;
	using CSharpSamples;
	using CSharpSamples.Winapi;
	using ImageViewerDll;

	#region Twin
	using Twin;
	using Twin.Bbs;
	using Twin.Util;
	using Twin.Tools;
	using Twin.Text;
	#endregion

	using ThreadTabController = TabController<ThreadHeader, ThreadControl>;
	using ListTabController = TabController<BoardInfo, ThreadListControl>;

	using TwinListWindow = TwinWindow<BoardInfo, ThreadListControl>;
	using TwinThreadWindow = TwinWindow<ThreadHeader, ThreadControl>;
	using System.Net.Configuration;
	using Twintail.Serialization;
	using RebarDotNet;
	using System.Threading;
	using Twin.Properties;

	/// <summary>
	/// IE�R���|�[�l���g���g�p����twintail
	/// </summary>
	public partial class Twin2IeBrowser : Form
	{
		private const int setAutoReloadLimit = 7;

		#region Callbacks
		private delegate void SetStatusBarInfoCallback(int index, string text);
		#endregion

		internal ThreadTabController threadTabController;
		internal ListTabController listTabController;

		#region Fields
		private Cache cache;
		private Settings settings;
		private Rectangle normalWindowRect;
		private System.Windows.Forms.Timer timerGarbageCollect;
		private string[] arguments;
		private ToolItem lastSelectedToolItem = null;
		private bool initializing = true;

		private NumberClickEventArgs numberClickEventArgs;
		private NumberClickEventArgs numberClickEventArgsSt;

		private NextThreadChecker nextThreadChecker;

		private IBoardTable _2chTable;			// 2channel.brd�̃f�[�^
		private IBoardTable userTable;			// user.brd�̃f�[�^
		private IBoardTable allTable;			// ���̓�����킹����

		private DisplayUtil display;			// ��ʍ\����ύX����N���X
		private PatrolBase patroller;			// ����N���X (���񒆂̓C���X�^���X�A����ȊO��null)
		private ToolItemCollection tools;		// �O���c�[���R���N�V����
		//private WroteHistory wroteHistory;	// �������ݗ���
		private KakikomiRireki kakikomi;		// �������ݗ���
		private KotehanManager koteman;			// �R�e�n���Ǘ�
		private ThreadHeaderIndices closedThreadHistory;	// �ŋߊJ�����X���b�h
		private ThreadHeaderIndices writtenThreadHistory;	// �ŋߏ������񂾃X���b�h
		private List<ThreadGroup> threadGroupList;	// �X���b�h�O���[�v���X�g
		private BookmarkRoot bookmarkRoot;		// ���C�ɓ���
		private BookmarkRoot warehouseRoot;		// �ߋ����O�q��
		private TabColorTable tabColorTable;	// �^�u�̌ʔz�F���

		// �R���g���[���A�_�C�A���O�֌W
		private ThreadUpdateChecker threadUpdateChecker;
		private DockWriteBar dockWriteBar;
		private PostDialog writeDialog;
		private BoardTableView tableView;
		private BookmarkView bookmarkView;
		private BookmarkView warehouseView;
		private BookmarkMenuBuilder bookmarkMenu;		// ���C�ɓ��胁�j���[�̐����N���X
		private SmoothProgressBar progress;
		private ResExtractDialog resExtract;
		private CacheSearchDialog cacheSearcher;
		private ThreadSearchDialog threadSearcher;
		private ThreadListSearchDialog listSearcher;
		private ThreadListControl listBookmarkControl;	// ���C�ɓ��菄�񎞂Ɏg�p����ꎞ�ϐ�

		private ImageViewer imageViewer;
		private bool __closing = false;

		private System.Windows.Forms.Timer autoReloadTimerCounter = null;
		private TabControlNativeWindow tabnative1, tabnative2;

		private Dictionary<string, int> autoReloadServerDic = new Dictionary<string, int>();
		private Dictionary<string, string> selfID = new Dictionary<string, string>(); // �e�X���b�h�ɏ������񂾎�����ID�Ǝv���镶������i�[ (key=URL, Value=ID)

		private FormWindowState prevWindowState = FormWindowState.Normal;

		private ColorWordInfoSettings colorWordInfoSett = null;

		private object lockObj = new object();
		private WroteRes lastWroteRes = null;	// �Ō�ɏ������񂾃��X�̃R�s�[
		#endregion

		#region Dummy
		/// <summary>���X���\���p�̃_�~�[�̔��</summary>
		internal static readonly BoardInfo dummyNextThreadBoardInfo = new BoardInfo("dummy.addr", "next", "���X���ē�");
		/// <summary>�������ݗ���\���p�̃_�~�[�̔��</summary>
		internal static readonly BoardInfo dummyWrittenBoardInfo = new BoardInfo("dummy.addr", "write", "�������ݗ���");
		/// <summary>�S�����X���\���p�̃_�~�[�̔��</summary>
		internal static readonly BoardInfo dummyAllThreadsBoardInfo = new BoardInfo("dummy.addr", "all", "�S�����X��");
		/// <summary>�������ʕ\���p�̃_�~�[�̔��</summary>
		internal static readonly BoardInfo dummySearchBoardInfo = new BoardInfo("dummy.addr", "search", "��������");
		/// <summary>���C�ɓ���\���p�̃_�~�[�̔��</summary>
		internal static readonly BoardInfo dummyBookmarkBoardInfo = new BoardInfo("dummy.addr", "bookmark", "���C�ɓ���");

		// �X�V�`�F�b�N�\�Ȕ̎�ނ��`
		private BoardInfo[] updateCheckableTypes = new BoardInfo[] {
			dummyAllThreadsBoardInfo, dummyBookmarkBoardInfo, dummyWrittenBoardInfo};
		#endregion

		#region Properties
		/// <summary>
		/// �t�H�[���̃f�t�H���g�T�C�Y���擾
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(700, 550);
			}
		}

		/// <summary>
		/// ���ׂĂ̔ꗗ�e�[�u�����擾
		/// </summary>
		public IBoardTable BBSTable
		{
			get
			{
				return allTable;
			}
		}

		/// <summary>
		/// ���O�̃L���b�V�������擾
		/// </summary>
		public Cache Cache
		{
			get
			{
				return cache;
			}
		}

		/// <summary>
		/// �ݒ�����擾�܂��͐ݒ�
		/// </summary>
		public Settings Settings
		{
			get
			{
				return settings;
			}
		}

		/// <summary>
		/// �I�t���C�����ǂ������f
		/// </summary>
		public bool IsOnline
		{
			set
			{
				settings.Online = value;
				menuItemFileOnline.Checked = value;
				toolBarButtonOnline.Pushed = value;
				toolBarButtonOnline.ImageIndex = value ? Icons.Online : Icons.Offline;
			}
			get
			{
				return settings.Online;
			}
		}

		/// <summary>
		/// �ꗗ�e�[�u���̃C���^�[�t�F�[�X���擾
		/// </summary>
		public ITwinTableControl TableInterface
		{
			get
			{
				return this;
			}
		}

		//		/// <summary>
		//		/// ���C�ɓ���R���g���[���̃C���^�[�t�F�[�X���擾
		//		/// </summary>
		//		public ITwinBookmarkControl BookmarkInterface {
		//			get {
		//				return bookmarkView;
		//			}
		//		}

		public bool FixedRebarBandSize
		{
			set
			{
				settings.View.FixedRebarControl = value;

				foreach (RebarDotNet.BandWrapper band in rebarWrapper.Bands)
				{
					SetRebarBandGripStyle(band);
				}
			}
			get
			{
				return settings.View.FixedRebarControl;
			}
		}
		#endregion

		#region Structs
		/// <summary>
		/// �X�e�[�^�X�o�[�̃p�l���C���f�N�b�X��\��
		/// </summary>
		internal struct StatusBarPanelIndex
		{
			public const int Text = 0;
			public const int Progress = 1;
			public const int Size = 2;
			public const int ResCount = 3;
			public const int Force = 4;
			public const int SambaCount = 5;
			public const int TimerCount = 6;
		}

		/// <summary>
		/// �^�u�C���[�W�̃C���f�b�N�X��\��
		/// </summary>
		internal struct TabImageIndex
		{
			public const int None = -1;
			public const int Complete = 0;
			public const int Loading = 1;
			public const int Error = 2;
			public const int Pastlog = 3;
			public const int Over1000Res = 4;
			public const int AutoReload = 5;
			public const int Tugi = 6;
			public const int Denki = 7;
		}

		internal const int WM_NEWINSTANCE = WinApi.WM_APP + 1;
		#endregion

		#region Events
		/// <summary>
		/// �t�H�[�����\�����ꂽ�Ƃ��ɔ���
		/// </summary>
		public event EventHandler Loaded;
		#endregion

		#region InnerClass
		/// <summary>
		/// ��ʍ\����ύX����N���X
		/// </summary>
		private class DisplayUtil
		{
			private Twin2IeBrowser form;

			/// <summary>
			/// ���ݐݒ肳��Ă����ʍ\�����擾
			/// </summary>
			public DisplayLayout Layout
			{
				get
				{
					return form.settings.Layout;
				}
			}

			/// <summary>
			/// ControlLayoutUtility�N���X�̃C���X�^���X��������
			/// </summary>
			/// <param name="ctrl"></param>
			public DisplayUtil(Twin2IeBrowser ctrl)
			{
				if (ctrl == null)
					throw new ArgumentNullException("ctrl");

				this.form = ctrl;
			}

			/// <summary>
			/// �w�肵����ʍ\���ɕύX
			/// </summary>
			/// <param name="layout"></param>
			public void SetLayout(DisplayLayout newlayout)
			{
				bool tableRight = form.settings.View.TableDockRight;
				DockStyle tableDockStyle = (tableRight) ? DockStyle.Right : DockStyle.Left;

				form.SuspendLayout();

				form.treePanel.Visible =
					form.listPanel.Visible =
					form.threadPanel.Visible = true;

				//				form.settings.View.HideTable =
				//					form.settings.View.FillList =
				//					form.settings.View.FillThread = false;

				switch (newlayout)
				{
					case DisplayLayout.Default:
						form.treePanel.Dock = tableDockStyle;
						form.treePanel.BringToFront();
						form.splitterLeft.Dock = tableDockStyle;
						form.splitterLeft.BringToFront();
						form.listPanel.Dock = DockStyle.Top;
						form.listPanel.BringToFront();
						form.splitterTop.Dock = DockStyle.Top;
						form.splitterTop.BringToFront();
						form.threadPanel.Dock = DockStyle.Fill;
						form.threadPanel.BringToFront();

						form.treePanel.Width = 180;
						form.listPanel.Height = 170;
						break;

					case DisplayLayout.Extend1:
						form.listPanel.Dock = DockStyle.Top;
						form.listPanel.BringToFront();
						form.splitterTop.Dock = DockStyle.Top;
						form.splitterTop.BringToFront();
						form.treePanel.Dock = tableDockStyle;
						form.treePanel.BringToFront();
						form.splitterLeft.Dock = tableDockStyle;
						form.splitterLeft.BringToFront();
						form.threadPanel.Dock = DockStyle.Fill;
						form.threadPanel.BringToFront();

						form.treePanel.Width = 180;
						form.listPanel.Height = 170;
						break;

					case DisplayLayout.Tate3:
						form.treePanel.Dock = tableDockStyle;
						form.treePanel.BringToFront();
						form.splitterLeft.Dock = tableDockStyle;
						form.splitterLeft.BringToFront();

						if (tableRight)
						{
							form.listPanel.Dock = DockStyle.Left;
							form.listPanel.BringToFront();
							form.splitterTop.Dock = DockStyle.Left;
							form.splitterTop.BringToFront();
						}
						else
						{
							form.listPanel.Dock = tableDockStyle;
							form.listPanel.BringToFront();
							form.splitterTop.Dock = tableDockStyle;
							form.splitterTop.BringToFront();
						}

						form.threadPanel.Dock = DockStyle.Fill;
						form.threadPanel.BringToFront();
						form.treePanel.Width = 200;
						form.listPanel.Width = 200;
						break;

					case DisplayLayout.Yoko3:
						form.treePanel.Dock = DockStyle.Top;
						form.treePanel.BringToFront();
						form.splitterLeft.Dock = DockStyle.Top;
						form.splitterLeft.BringToFront();
						form.listPanel.Dock = DockStyle.Top;
						form.listPanel.BringToFront();
						form.splitterTop.Dock = DockStyle.Top;
						form.splitterTop.BringToFront();
						form.threadPanel.Dock = DockStyle.Fill;
						form.threadPanel.BringToFront();

						form.treePanel.Height = 150;
						form.listPanel.Height = 150;
						break;

					case DisplayLayout.TateYoko2:
						DockStyle dock = (tableRight) ? DockStyle.Left : DockStyle.Right;
						form.threadPanel.Dock = dock;
						form.threadPanel.BringToFront();
						form.splitterLeft.Dock = dock;
						form.splitterLeft.BringToFront();
						form.treePanel.Dock = DockStyle.Top;
						form.treePanel.BringToFront();
						form.splitterTop.Dock = DockStyle.Top;
						form.splitterTop.BringToFront();
						form.listPanel.Dock = DockStyle.Fill;
						form.listPanel.BringToFront();

						form.treePanel.Height = 200;
						form.treePanel.Width = 150;
						form.listPanel.Width = 150;
						break;
				}

				form.settings.Layout = newlayout;
				form.ResumeLayout();
				form.UpdateLayout();
			}
		}
		#endregion

		/// <summary>
		/// Twin2IeBrowser�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="args"></param>
		public Twin2IeBrowser(Cache cache, Settings settings, string[] args)
		{
			// ����
			this.arguments = args;
			this.settings = settings;
			this.cache = cache;

			// Windows�̏I���C�x���g��o�^
			Microsoft.Win32.SystemEvents.SessionEnding +=
				new Microsoft.Win32.SessionEndingEventHandler(OnSessionEnding);

			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			new HttpWebRequestElement().UseUnsafeHeaderParsing = true;

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			toolBarButtonCaching.Visible = false;
			menuItemThreadLinkExtract.Visible = false;
			this.notifyIcon1.Visible = false;
			this.labelThreadSubject.UseMnemonic = false;
			SetHeaderInfo(null);
			SetBoardInfo(null);
			AdjustToolBar();

			InitializeItems();
			InitializeControls();
			LoadSettings();
		}


		#region Dispose
		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				autoReloadTimerCounter.Dispose();
				timerGarbageCollect.Dispose();
				threadUpdateChecker.Dispose();
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Settings
		/// <summary>
		/// �ݒ��ǂݍ���
		/// </summary>
		private void LoadSettings()
		{
			// ���j���[�𔽉f
			if (File.Exists(Settings.MenuShortcutPath))
			{
				Settings.ConvertingShortcutKeyFile(settings);
				MenuSerializer2.Deserialize(Settings.MenuShortcutPath, this);
			}

			ClientBase.ConnectionLimitter = settings.ConnectionLimit;

			// ���ʂ̎�M�p�v���L�V��ݒ�
			WebRequest.DefaultWebProxy = settings.Net.RecvProxy;

			// �t�H���g���쐬
			settings.Design.List.CreateFonts();

			// �ꗗ�ɐV�����ݒ�𔽉f
			if (settings.Design.Table.HideIcon)
				tableView.ImageList = null;

			settings.Thread.CorrectAutoReloadInterval();
			// �I�[�g�����[�h�Ԋu��ݒ�
			IEComThreadBrowser.AutoRefreshTimers.Interval =
				settings.Thread.AutoReloadInterval;

			// ���̑��̐ݒ���
			IsOnline = settings.Online;
			timerGarbageCollect.Enabled = settings.GarbageCollect;

			// �O���c�[���̑I���A�C�e����ݒ�
			if (0 <= settings.SelectedToolsIndex && settings.SelectedToolsIndex < comboBoxTools.Items.Count)
			{
				comboBoxTools.SelectedIndex = settings.SelectedToolsIndex;
			}

			// ���o�[�R���g���[���̏�Ԃ��X�V
			LoadRebarControlState();

			mgSett.Load();
		}

		private void LoadWindowPosition()
		{
			// �E�C���h�E��񂪋�Ȃ��ʒ����ɐݒ�
			// beta21
			//			if (settings.WindowSize.IsEmpty ||
			//				settings.WindowLocation.IsEmpty)
			if (settings.WindowSize.IsEmpty ||
				IsOutOfScreen(settings.WindowLocation, settings.WindowSize))
			{
				this.Size = DefaultSize;
				this.CenterToScreen();
			}
			else
			{
				this.Location = settings.WindowLocation;
				this.ClientSize = settings.WindowSize;
				this.WindowState = settings.IsMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
			}

			if (!settings.View.ThreadView.IsEmpty)
				// �X���b�h�̃T�C�Y��ݒ�
				this.threadPanel.Size = settings.View.ThreadView;

			if (!settings.View.TableView.IsEmpty)
				// �ꗗ�e�[�u���̃T�C�Y��ݒ�
				this.treePanel.Size = settings.View.TableView;

			if (!settings.View.ListView.IsEmpty)
				// �X���ꗗ�̃T�C�Y��ݒ�
				this.listPanel.Size = settings.View.ListView;

			this.threadToolPanel.Visible = settings.View.ThreadToolBar;
			this.statusBar.Visible = settings.View.StatusBar;
		}

		/// <summary>
		///�@�E�C���h�E����ʂɌ����Ȃ��ʒu�ɂ���Ȃ�true��Ԃ�
		/// </summary>
		private bool IsOutOfScreen(Point location, Size size)
		{
			Rectangle screen = Screen.PrimaryScreen.WorkingArea;

			bool inScreen =
				location.X + size.Width >= 0 || location.Y + size.Height >= 0 ||
				location.X <= screen.Width || location.Y <= screen.Height;

			return !inScreen;
		}

		/// <summary>
		/// �ݒ��ۑ�
		/// </summary>
		private Settings SaveSettings()
		{
			// �E�C���h�E�T�C�Y�̕ۑ�
			settings.WindowLocation = normalWindowRect.Location;
			settings.WindowSize = normalWindowRect.Size;
			settings.IsMaximized = this.WindowState == FormWindowState.Maximized;

			// �e�r���[�̏�Ԃ����ɖ߂�
			//			settings.View.HideTable =
			//				settings.View.FillList =
			//				settings.View.FillThread = false;
			//			UpdateLayout();

			// �X���b�h�r���[�̃T�C�Y
			settings.View.ThreadView = threadPanel.Size;
			// �ꗗ�e�[�u���̃T�C�Y��ۑ�
			settings.View.TableView = treePanel.Size;
			// �X���ꗗ�̃T�C�Y��ۑ�
			settings.View.ListView = listPanel.Size;
			// �h�b�L���O�������݃o�[�̃T�C�Y��ۑ�
			settings.View.DockWriteBarHeight = dockWriteBar.Height;

			// �I������Ă���O���c�[���̃C���f�b�N�X��ۑ�
			settings.SelectedToolsIndex = lastSelectedToolItem == null ? -1 :
				comboBoxTools.Items.IndexOf(lastSelectedToolItem);

			// ���o�[�R���g���[���̏�Ԃ�ۑ�
			SaveRebarControlState();
			Twinie.SerializingSettings(settings);

			SaveItaBotan();

			return settings;
		}

		/// <summary>
		/// Rebar�R���g���[���̏�Ԃ�ǂݍ���
		/// </summary>
		private void LoadRebarControlState()
		{
			RebarDotNet.BandWrapper[] bands = new RebarDotNet.BandWrapper[5];
			RebarDotNet.BandWrapper band = null;
			RebarSettings rebarSett = settings.Rebar;
			int index = -1;

			try
			{
				index = rebarSett.ToolBar.Index;
				band = rebarWrapper.Bands["ToolBar"];
				SetRebarBandGripStyle(band);
				band.NewRow = rebarSett.ToolBar.NewRow;
				band.Width = rebarSett.ToolBar.Width;
				bands[index] = band;

				index = rebarSett.ListToolBar.Index;
				band = rebarWrapper.Bands["ListToolBar"];
				SetRebarBandGripStyle(band);
				band.NewRow = rebarSett.ListToolBar.NewRow;
				band.Width = rebarSett.ListToolBar.Width;
				bands[index] = band;

				index = rebarSett.ToolsBar.Index;
				band = rebarWrapper.Bands["Tools"];
				SetRebarBandGripStyle(band);
				band.NewRow = rebarSett.ToolsBar.NewRow;
				band.Width = rebarSett.ToolsBar.Width;
				bands[index] = band;

				index = rebarSett.ItaButton.Index;
				band = rebarWrapper.Bands["IButton"];
				SetRebarBandGripStyle(band);
				band.NewRow = rebarSett.ItaButton.NewRow;
				band.Width = rebarSett.ItaButton.Width;
				bands[index] = band;

				index = rebarSett.AddressBar.Index;
				band = rebarWrapper.Bands["Address"];
				SetRebarBandGripStyle(band);
				band.NewRow = rebarSett.AddressBar.NewRow;
				band.Width = rebarSett.AddressBar.Width;
				bands[index] = band;

				rebarWrapper.SuspendLayout();
				rebarWrapper.Bands.Clear();
				for (int i = 0; i < bands.Length; i++)
				{
					rebarWrapper.Bands.Add(bands[i]);
				}
				rebarWrapper.ResumeLayout(true);
			}
			catch (IndexOutOfRangeException)
			{
			}

		}

		private void SetRebarBandGripStyle(RebarDotNet.BandWrapper band)
		{
			if (settings.View.FixedRebarControl)
			{
				band.GripSettings = RebarDotNet.GripperSettings.Never;
			}
			else
			{
				band.GripSettings = RebarDotNet.GripperSettings.Always;
			}
		}

		/// <summary>
		/// ���o�[�R���g���[���̕\����Ԃ��X�V
		/// </summary>
		private void LoadRebarVisibleState()
		{
			rebarWrapper.Visible = true;
			RebarSettings rebar = settings.Rebar;

			bandWrapperMain.Visible = rebar.ToolBar.Visible;
			bandWrapperList.Visible = rebar.ListToolBar.Visible;
			bandWrapperAddress.Visible = rebar.AddressBar.Visible;
			bandWrapperIButton.Visible = rebar.ItaButton.Visible;
			bandWrapperTools.Visible = rebar.ToolsBar.Visible;
		}

		/// <summary>
		/// ���o�[�R���g���[���̏�Ԃ�ۑ�
		/// </summary>
		private void SaveRebarControlState()
		{
			// �ŏ������͈ʒu��T�C�Y��񂪐���ł͂Ȃ��̂ŏ������Ȃ�
			if (WindowState == FormWindowState.Minimized)
				return;

			RebarSettings rebar = settings.Rebar;
			rebar.ToolBar.NewRow = bandWrapperMain.Left < 4 ? true : false;// NTwin23.102
			rebar.ToolBar.Width = bandWrapperMain.Width;
			rebar.ToolBar.Index = bandWrapperMain.BandIndex;
			rebar.ToolBar.Visible = bandWrapperMain.Visible;

			rebar.ListToolBar.NewRow = bandWrapperList.Left < 4 ? true : false;// NTwin23.102
			rebar.ListToolBar.Width = bandWrapperList.Width;
			rebar.ListToolBar.Index = bandWrapperList.BandIndex;
			rebar.ListToolBar.Visible = bandWrapperList.Visible;

			rebar.AddressBar.NewRow = bandWrapperAddress.Left < 4 ? true : false;// NTwin23.102
			rebar.AddressBar.Width = bandWrapperAddress.Width;
			rebar.AddressBar.Index = bandWrapperAddress.BandIndex;
			rebar.AddressBar.Visible = bandWrapperAddress.Visible;

			rebar.ItaButton.NewRow = bandWrapperIButton.Left < 4 ? true : false;// NTwin23.102
			rebar.ItaButton.Width = bandWrapperIButton.Width;
			rebar.ItaButton.Index = bandWrapperIButton.BandIndex;
			rebar.ItaButton.Visible = bandWrapperIButton.Visible;

			rebar.ToolsBar.NewRow = bandWrapperTools.Left < 4 ? true : false;// NTwin23.102
			rebar.ToolsBar.Width = bandWrapperTools.Width;
			rebar.ToolsBar.Index = bandWrapperTools.BandIndex;
			rebar.ToolsBar.Visible = bandWrapperTools.Visible;
		}
		#endregion

		#region Events
		/// <summary>
		/// Loaded�C�x���g�𔭐�������
		/// </summary>
		private void OnLoaded()
		{
			if (Loaded != null)
				Loaded(this, new EventArgs());
		}
		#endregion

		#region Handlers
		// Windows�I���C�x���g
		private void OnSessionEnding(object sender, Microsoft.Win32.SessionEndingEventArgs e)
		{
			this.Close();
		}

		// �ꗗ�X�V�C�x���g
		private void OnBoardUpdate(object sender, BoardUpdateEventArgs e)
		{
			if (e.Event == BoardUpdateEvent.Change)
				OnServerChanged(null, new ServerChangeEventArgs(e.OldBoard, e.NewBoard));
		}

		//���ړ]�������ɃL���b�V���Ɣ{�^�����ړ]
		private void OnServerChanged(object sender, ServerChangeEventArgs e)
		{
			try
			{
				SetStatusBarInfo(0, e.OldBoard.Name + "���ړ]���܂����B�������Ă��܂�...");

				// �S�X���b�h�̃����[�h���ꎞ��~
				ClientBase.Stopper.Reset();

				// �S�X���b�h���ҋ@��ԂɂȂ�܂ł܂�
				//foreach (ThreadControl c in threadTabController.GetControls())
				//{
				//    while (c.IsReading && !c.IsWaiting)
				//        System.Threading.Thread.Sleep(500);
				//}

				// �L���b�V�����ړ]
				BoardLinker linker = new BoardLinker(cache);
				linker.Replace(e.OldBoard, e.NewBoard);

				// �{�^���Ƃ��C�ɓ���ɂ����f������
				ItaBotan.ServerChange(cSharpToolBar, e.OldBoard, e.NewBoard);
				SaveItaBotan();

				BookmarkUtility.ServerChange(bookmarkRoot, e.OldBoard, e.NewBoard);
				BookmarkUtility.ServerChange(warehouseRoot, e.OldBoard, e.NewBoard);
				SaveBookmarks();

				// �ꗗ������������
				_2chTable.Replace(e.OldBoard, e.NewBoard);
				userTable.Replace(e.OldBoard, e.NewBoard);

				// �R�e�n����������������
				koteman.ServerChange(e.OldBoard, e.NewBoard);
				koteman.Save(Settings.KotehanFilePath);

				// �J���Ă���X���b�h�����ׂ�
				OnServerChanged_CheckOpened(e.OldBoard, e.NewBoard);
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
			finally
			{
				// �S�X���b�h�̃����[�h���ĊJ
				ClientBase.Stopper.Set();
			}
		}

		// �J���Ă���X���b�h�̈ړ]����
		private void OnServerChanged_CheckOpened(BoardInfo old, BoardInfo _new)
		{
			foreach (ThreadControl thread in threadTabController.GetControls())
			{
				BoardInfo bi = thread.HeaderInfo.BoardInfo;

				if (old.Server == bi.Server)
				{
					thread.HeaderInfo.BoardInfo.Server = _new.Server;
					thread.HeaderInfo.Pastlog = false;
				}
			}
		}

		// �L���b�V������������ꂽ�C�x���g
		private void CacheSearcher_Closed(object sender, EventArgs e)
		{
			cacheSearcher = null;
		}

		// �X���b�h������ꂽ���Ɍ����_�C�A���O���J����Ă���Ε���
		private void ThreadControl_Closed(object sender, EventArgs e)
		{
			if (threadSearcher != null)
				threadSearcher.Close();
			threadSearcher = null;

			if (resExtract != null)
				resExtract.Close();
			resExtract = null;
		}

		// �X���b�h�ꗗ������ꂽ���Ɍ����_�C�A���O���J����Ă���Ε���
		private void ThreadListControl_Closed(object sender, EventArgs e)
		{
			if (listSearcher != null)
				listSearcher.Close();
			listSearcher = null;
		}

		// timerGarbageCollect.Tick�C�x���g
		private void Timer_GarbageCollect(object sender, EventArgs e)
		{
			GC.Collect();
		}
		#endregion

		#region ���������\�b�h
		/// <summary>
		/// ���������Ƀp�����[�^���n���ꂽ��Ă΂��
		/// </summary>
		/// <param name="parameters">�p�����[�^�z��</param>
		private void ParseParameter(string[] parameters)
		{
			foreach (string param in parameters)
			{
				if (param.StartsWith("http://"))
				{
					// �A�h���X���J��
					OpenAddress(param);
				}
				else if (param.StartsWith("/patrol"))
				{
					// ����
					bool checkOnly = param.StartsWith("/patrol:checkonly");
					BookmarkPatrol(bookmarkRoot, checkOnly, true);
				}
				else if (param == "/open")
				{
					// �O��J�����E�C���h�E���J��
					OpenStartup();
				}
			}

		}

		/// <summary>
		/// Form.Load�ŌĂ΂�鏉�������\�b�h
		/// </summary>
		private void Initialize()
		{
			// ���ʂ̊J�����@��ݒ�
			tableView.OpenMode =
				bookmarkView.OpenMode =
				warehouseView.OpenMode = settings.Operate.OpenMode;

			// �J�e�S���͏�ɂP�����J���Ȃ��ݒ�
			tableView.AlwaysSingleOpen = settings.AlwaysSingleOpen;

			// �ꗗ�{�^����\������ꍇ�̏���
			if (settings.View.TableItaBotan)
				SetTableItaBotan(allTable);

			// �F�؂Ɋւ��鏉����
			if (settings.Authentication.AuthenticationOn)
				X2chAuthenticator.Enable(settings.Authentication.Username, settings.Authentication.Password);
			else
				X2chAuthenticator.Disable();

			// ��ʍ\����ύX
			display.SetLayout(settings.Layout);

			LoadWindowPosition();

			UpdateTitleBar();
			initializing = false;
		}

		/// <summary>
		/// �R���X�g���N�^�ŌĂ΂�鏉�������\�b�h
		/// </summary>
		private void InitializeItems()
		{
			_2chTable = new KatjuBoardTable();
			userTable = new KatjuBoardTable();
			allTable = new KatjuBoardTable();

			// �ꗗ��ǂݍ���
			try
			{
				_2chTable.LoadTable(Settings.BoardTablePath);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("2channel.brd�����݂��܂���B" +
					"\r\n�t�@�C�����j���[����[�ꗗ���X�V]���Ă�������");
			}

			// ���[�U�[��`�t�@�C����ǂݍ���
			if (File.Exists(Settings.UserTablePath))
				userTable.LoadTable(Settings.UserTablePath);

			// ��̔ꗗ�f�[�^������
			allTable.Add(userTable);
			allTable.Add(_2chTable);

			// �������ݗ���
			kakikomi = new KakikomiRireki(Settings.KakikoFolderPath);
			//wroteHistory = new WroteHistory(cache);

			// �ŋߕ���������ǂݍ���
			closedThreadHistory = new ThreadHeaderIndices(cache, Settings.ClosedHistoryPath);
			closedThreadHistory.Load();

			// �ŋߏ������񂾗�����ǂݍ���
			writtenThreadHistory = new ThreadHeaderIndices(cache, Settings.WrittenHistoryPath);
			writtenThreadHistory.Load();


			// GC���s�^�C�}�[��������
			timerGarbageCollect = new System.Windows.Forms.Timer();
			timerGarbageCollect.Interval = 60000 * 10;
			timerGarbageCollect.Tick += new EventHandler(Timer_GarbageCollect);

			autoReloadTimerCounter = new System.Windows.Forms.Timer();
			autoReloadTimerCounter.Interval = 500;
			autoReloadTimerCounter.Tick += new EventHandler(autoReloadTimerCounter_Tick);

			// �X���b�h�O���[�v��ǂݍ���
			LoadThreadGroupList();

			LoadBookmarkFiles();

			LoadItaBotan();

			// �O���c�[����ǂݍ���
			tools = new ToolItemCollection();
			tools.Load(Settings.ToolsFilePath);
			UpdateToolsComboBox();

			// �R�e�n����ǂݍ���
			koteman = new KotehanManager();
			koteman.Load(Settings.KotehanFilePath);

			// ��ʍ\���ύX�N���X
			display = new DisplayUtil(this);

			// Be2ch�̔F�؃N���X�̃C���X�^���X��ݒ�
			TwinDll.Be2chCookie = settings.Post.Be2chCookie;

			// �^�u�z�F���̏�����
			tabColorTable = new TabColorTable(Settings.ColorTableSettingsPath);
			tabColorTable.Load();

			colorWordInfoSett = new ColorWordInfoSettings(Settings.ColorWordInfoSettingsPath);
			colorWordInfoSett.Load();

			writeDialog = null;
			nextThreadChecker = null;
		}

		private void LoadThreadGroupList()
		{
			threadGroupList = new List<ThreadGroup>();
			string[] groupList = Directory.GetFiles(Settings.GroupFolderPath, "*.grp");

			foreach (string path in groupList)
			{
				ThreadGroup g = new ThreadGroup(cache, path);
				g.Load();

				threadGroupList.Add(g);
			}
		}

		private void SaveThreadGroupList()
		{
			foreach (ThreadGroup g in threadGroupList)
			{
				g.Save();
			}
		}

		private void LoadItaBotan()
		{
			// �{�^���𕜌�
			cSharpToolBar.Buttons.AddRange(
				ItaBotan.Load(Settings.ItaBotanPath));
		}

		private void LoadBookmarkFiles()
		{
			Dictionary<BookmarkRoot, string> bookmarks =
				new Dictionary<BookmarkRoot, string>();

			bookmarkRoot = new BookmarkRoot("���C�ɓ���");
			warehouseRoot = new BookmarkRoot("�ߋ����O");

			bookmarks.Add(bookmarkRoot, Settings.BookmarkPath);
			bookmarks.Add(warehouseRoot, Settings.WarehousePath);

			foreach (KeyValuePair<BookmarkRoot, string> item in bookmarks)
			{
				try
				{
					item.Key.Load(item.Value);
					// ����ɓǂݍ��߂���o�b�N�A�b�v
					Twinie.BackupUtil.Backup(item.Value);
				}
				catch (Exception)
				{
					if (Twinie.BackupUtil.Restore(item.Value))
					{
						item.Key.Load(item.Value);
						MessageBox.Show(item.Key.Name + "�t�@�C�������Ă����̂Ńo�b�N�A�b�v���畜�����܂����B");
					}
					else
					{
						MessageBox.Show(item.Key.Name + "�t�@�C�������Ă��ēǂݍ��߂܂���ł���");
					}
				}
			}
		}

		/// <summary>
		/// �R���X�g���N�^�ŌĂ΂��R���g���[�������������郁�\�b�h
		/// </summary>
		private void InitializeControls()
		{

			// �v���O���X�o�[��������
			progress = new SmoothProgressBar();
			progress.ForeColor = SystemColors.ControlLightLight;
			progress.TextStyle = ProgressTextStyle.Percent;

			statusProgress.BorderStyle = StatusBarPanelBorderStyle.None;
			statusBar.Controls.Add(progress);
			statusBar.DoubleClick += new EventHandler(statusBar_DoubleClick);

			// �ꗗ�c���[�r���[��������
			tableView = new BoardTableView(settings.Design.Table);
			tableView.Dock = DockStyle.Fill;
			tableView.Selected += new BoardTableEventHandler(boardTableView_Selected);
			tableView.Table = allTable;
			tableView.ContextMenuStrip = contextMenuTable;
			tableView.ImageList = imageListTable;
			tabPageBoards.Controls.Add(tableView);

			// ���C�ɓ���c���[�r���[��������
			bookmarkView = new BookmarkView(bookmarkRoot, settings.Design.Table);
			bookmarkView.Dock = DockStyle.Fill;
			bookmarkView.Selected += new ThreadHeaderEventHandler(bookmarkView_Selected);
			bookmarkView.ImageList = imageListTable;
			bookmarkView.FolderContextMenu = contextMenuBookmarkFolder;
			bookmarkView.BookmarkContextMenu = contextMenuBookmarkItem;
			bookmarkView.BookmarkChanged += OnBookmarkChanged;
			tabPageBookmarks.Text = bookmarkRoot.Name;
			tabPageBookmarks.Controls.Add(bookmarkView);

			// �ߋ����O�q�Ƀc���[�r���[��������
			warehouseView = new BookmarkView(warehouseRoot, settings.Design.Table);
			warehouseView.Dock = DockStyle.Fill;
			warehouseView.Selected += new ThreadHeaderEventHandler(bookmarkView_Selected);
			warehouseView.ImageList = imageListTable;
			warehouseView.FolderContextMenu = contextMenuWareHouseFolder;
			warehouseView.BookmarkContextMenu = contextMenuWareHouseItem;
			warehouseView.BookmarkChanged += OnBookmarkChanged;
			tabPageWareHouse.Text = warehouseRoot.Name;
			tabPageWareHouse.Controls.Add(warehouseView);

			// �h�b�L���O�������݃o�[���쐬
			Splitter splitter = new Splitter();
			splitter.Dock = DockStyle.Bottom;

			dockWriteBar = new DockWriteBar(cache, koteman);
			dockWriteBar.Dock = DockStyle.Bottom;
			dockWriteBar.Posted += new PostedEventHandler(PostDialog_Posted);
			dockWriteBar.Visible = settings.View.DockWriteBar;
			dockWriteBar.Height = settings.View.DockWriteBarHeight;
			dockWriteBar.Sage = settings.Post.Sage;
			//dockWriteBar.ImeOn = settings.Post.ImeOn;

			threadInnerPanel.Controls.Add(splitter);
			threadInnerPanel.Controls.Add(dockWriteBar);

			dockWriteBar.BringToFront();
			splitter.BringToFront();
			threadTabCtrl.BringToFront();

			// �^�u�R���g���[���̃}�E�X�C�x���g���Ď�
			TabUtility.ChangedPosition += new EventHandler(TabUtility_ChangedPosition);
			TabUtility.SetTabControl(listTabCtrl);
			TabUtility.SetTabControl(threadTabCtrl);

			listTabCtrl.MouseClick += new MouseEventHandler(TabControl_MouseDown);
			threadTabCtrl.MouseClick += new MouseEventHandler(TabControl_MouseDown);

			listTabCtrl.ShowToolTips = true;
			listTabCtrl.Appearance = settings.View.TabAppearance;
			listTabCtrl.SizeMode = settings.View.ListTabSizeMode;
			listTabCtrl.ItemSize = settings.View.ListTabSize;

			threadTabCtrl.ShowToolTips = true;
			threadTabCtrl.SizeMode = settings.View.ThreadTabSizeMode;
			threadTabCtrl.ItemSize = settings.View.ThreadTabSize;
			threadTabCtrl.Appearance = settings.View.TabAppearance;
			threadTabCtrl.MouseDown += new MouseEventHandler(threadTabCtrl_MouseDown);
			threadTabCtrl.MouseUp += new MouseEventHandler(threadTabCtrl_MouseUp);
			
			tabnative1 = new TabControlNativeWindow(listTabCtrl);
			tabnative1.MouseDoubleClick += TabControl_DoubleClick;

			tabnative2 = new TabControlNativeWindow(threadTabCtrl);
			tabnative2.MouseDoubleClick += TabControl_DoubleClick;

			// ����X�V�`�F�b�J���쐬
			threadUpdateChecker = new ThreadUpdateChecker(this, cache);
			threadUpdateChecker.NoClosing = true;

			// ���C�ɓ��胁�j���[�����N���X��������
			bookmarkMenu = new BookmarkMenuBuilder(menuItemBookmarks, bookmarkRoot);
			bookmarkMenu.Selected += new ThreadHeaderEventHandler(bookmarkMenu_Selected);
			bookmarkMenu.UpdateCheck += new BookmarkEventHandler(bookmarkMenu_UpdateCheck);
			bookmarkMenu.SetUpdateCheckEnabled(new IsUpdateCheckEnabled(bookmarkMenu_IsUpdateCheckEnabled));

			// initialize controller
			threadTabController = new ThreadTabController(threadTabCtrl,
				this.threadTabController_OnCreate,
				this.threadTabController_OnDestroy);

			listTabController = new ListTabController(listTabCtrl,
				this.listTabController_OnCreate,
				this.listTabController_OnDestroy);

			//SetDefaultFont();

			if (settings.Operate.EnabledTabWheelScroll)
			{
				// �}�E�X�z�C�[���Ń^�u�̈ړ����������邽�߁A�}�E�X���O���[�o���t�b�N
				MouseHook.MouseWheel += new EventHandler<MouseHook.MouseHookEventArgs>(MouseHook_MouseWheel);
			}

			// 2012/3/1
			threadToolPanel.BackColor = settings.ThreadToolPanel.BackColor;
			threadToolPanel.ForeColor = settings.ThreadToolPanel.ForeColor;
		}

		void MouseHook_MouseWheel(object sender, MouseHook.MouseHookEventArgs e)
		{
			bool next = e.Delta < 0;
			if (TabUtility.GetItemAt(threadTabCtrl, threadTabCtrl.PointToClient(e.Location)) != null)
			{
				threadTabController.Select(next);
			}
			else if (TabUtility.GetItemAt(listTabCtrl, listTabCtrl.PointToClient(e.Location)) != null)
			{
				listTabController.Select(next);
			}
		}

		private void SetDefaultFont()
		{
			// �e�R���g���[���̃t�H���g��Windows�W���ɐݒ�
			threadTabCtrl.Font = listTabCtrl.Font = cSharpToolBar.Font = tabControlTable.Font = statusBar.Font =
				dockWriteBar.Font = labelBoardName.Font = labelThreadSubject.Font = cSharpToolBar.Font =
				comboBoxTools.Font = textBoxAddress.Font = rebarWrapper.Font = (Font)SystemFonts.MessageBoxFont.Clone();
		}

		void statusBar_DoubleClick(object sender, EventArgs e)
		{
			dockWriteBar.Visible = settings.View.DockWriteBar =
				   !settings.View.DockWriteBar;

		}




		#region TabController Events

		private void AppendNewTab(TabControl tabctrl, TabPage tab)
		{
			int index = tabctrl.SelectedIndex;

			if (index == -1)
				index = 0;

			switch (settings.NewTabPosition)
			{
				case NewTabPosition.First:
					tabctrl.TabPages.Insert(0, tab);
					break;

				case NewTabPosition.CurrentLeft:
					tabctrl.TabPages.Insert(index, tab);
					break;

				case NewTabPosition.CurrentRight:
					tabctrl.TabPages.Insert(
						Math.Min(index + 1, tabctrl.TabCount), tab);
					break;

				case NewTabPosition.Last:
					tabctrl.TabPages.Add(tab);
					break;
			}
		}


		ThreadControl threadTabController_OnCreate(ThreadHeader header, bool newWindow)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			ThreadControl viewer = threadTabController.FindControl(header);

			if (viewer != null)
			{
				threadTabCtrl.SelectedTab = (TabPage)viewer.Tag;
				return viewer;
			}
			else
			{
				if (threadTabCtrl.TabCount == 0)
					newWindow = true;

				else
				{
					viewer = threadTabController.Control;
					if (viewer != null && viewer.IsReading)
						newWindow = true;
				}
			}



			TwinThreadWindow win = null;

			TabPage tab = null;
			int tabCount = threadTabCtrl.TabCount;

			// �V�K�E�C���h�E���쐬���邩�ǂ���
			if (tabCount == 0 || newWindow)
			{
				// �r���[�A���쐬
				IEComThreadBrowser browser = new IEComThreadBrowser(cache, settings);
				browser.Dock = DockStyle.Fill;
				browser.NumberClick += new NumberClickEventHandler(threadView_NumberClick);
				browser.UriClick += new UriClickEventHandler(threadView_UriClick);
				browser.StatusTextChanged += new StatusTextEventHandler(ClientBase_StatusTextChanged);
				browser.AutoReloadChanged += new EventHandler<AutoReloadChangedEventArgs>(browser_AutoReloadChanged);
				browser.SirusiReferenced += new EventHandler<SirusiRefEventArgs>(browser_SirusiReferenced);
				browser.MouseGestureTest += new EventHandler<MouseGestureEventArgs>(browser_MouseGestureTest);
				browser.BeginWrite += new EventHandler<ResSetEventArgs>(browser_BeginWrite);
				browser.F += delegate { ThreadFind(); };
				browser.MouseGestureRange = mgSett.Range;
				browser.ColorWordInfoSettings = colorWordInfoSett;
				viewer = browser;

				// �������[�h�ł̓I�[�g�X�N���[�����I�[�g�����[�h��On�ɂ���
				if (settings.Livemode)
					browser.AutoReload = browser.AutoScroll = true;

				ClientAddEvents(browser);

				win = new TwinThreadWindow(viewer);
				win.AutoImageOpen = settings.ImageViewer_AutoOpen;

				tab = new TabPage();
				tab.Text = StringUtility.Unescape(StringUtility.RemoveHeadSpace(header.Subject));
				tab.ToolTipText = StringUtility.Unescape(header.Subject);
				tab.Controls.Add(viewer);
				tab.Tag = win;
				viewer.Tag = tab;
				// NTwin23.102
				if (settings.UseVisualStyle)
				{
					tab.BorderStyle = BorderStyle.FixedSingle;	// NTwin23
				}
				// NTwin23.102
				AppendNewTab(threadTabCtrl, tab);

				// ��[null�������Ȃ���SelectedIndexChanged�C�x���g���������Ȃ�
				if (tabCount == 0)
					threadTabCtrl.SelectedTab = null;

				threadTabCtrl.SelectedTab = tab;
			}
			else
			{
				tab = threadTabCtrl.SelectedTab;
				tab.Text = StringUtility.Unescape(StringUtility.RemoveHeadSpace(header.Subject));
				tab.ToolTipText = StringUtility.Unescape(header.Subject);

				win = (TwinThreadWindow)tab.Tag;
				win.ColorSet = TabColorSet.Default;

				viewer = (ThreadControl)win.Control;
			}

			UpdateToolBar();

			return viewer;
		}

		/// <summary>
		/// �󂳂ꂽ���X�����̃��X�ɎQ�Ƃ��ꂽ�ꍇ�ɂ��̃C�x���g����������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void browser_SirusiReferenced(object sender, SirusiRefEventArgs e)
		{
			if (e.ResSet.IsNew)
			{
				if (File.Exists(settings.Sound._Sirusi))
				{
					using (var p = new System.Media.SoundPlayer(settings.Sound._Sirusi))
						p.Play();
				}
				TwinThreadWindow win = threadTabController.FindWindow(e.HeaderInfo);
				win.Referenced = true;
			}
		}

		void browser_BeginWrite(object sender, ResSetEventArgs e)
		{
			lock (lockObj)
			{
				IEComThreadBrowser browser = (IEComThreadBrowser)sender;
				if (lastWroteRes != null)
				{
					// �Ō�ɏ������񂾃��X�Ǝ�M�������X����v������������
					if (browser.HeaderInfo.Url == lastWroteRes.HeaderInfo.Url)
					{
						string id;
						selfID.TryGetValue(browser.HeaderInfo.Url, out id);
						ResSet selfRes = SelfNotify.Find(lastWroteRes, e.Items, ref id);
						if (selfRes.Index >= 1)
						{
							selfID[browser.HeaderInfo.Url] = id;
							browser.Sirusi(selfRes.Index, false);
						}
					}
					lastWroteRes = null;
				}

				// �������܂�郌�X�̒�����摜�����N��T���A�C���[�W�r���[�A�Ŏ����I�ɊJ��
				TwinThreadWindow win = threadTabController.FindWindow(browser.HeaderInfo);
				if (win != null && win.AutoImageOpen)
				{
					ResSet[] resArray = new List<ResSet>(e.Items).ToArray();
					ImageViewer_OpenImageAsync(browser.HeaderInfo, resArray, true);
				}
			}
		}

		#region ImageViewr_OpenImageAsync
		private void ImageViewer_OpenImageAsync(ThreadHeader header, ResSet[] resArray, bool newResOnly)
		{
			WaitCallback callback = delegate
			{
				var newUrls = new List<string>();

				foreach (ResSet res in resArray)
				{
					if (newResOnly == false || newResOnly && res.IsNew)
					{
						string[] link = res.Links[".jpg|.jpe|.jpeg|.gif|.png|.bmp"];
						newUrls.AddRange(link);
					}
				}

				if (newUrls.Count == 0)
					return;

				MethodInvoker m1 = delegate
				{
					if (settings.ImageViewer)
					{
						ImageViewerOpen(newUrls.ToArray());
					}
					else
					{
						foreach (string url in newUrls)
							CommonUtility.OpenWebBrowser(url);
					}
				};
				Invoke(m1);
			};
			ThreadPool.QueueUserWorkItem(callback);
		}
		/*
		private void Thumbnails_OpenImageAsync(ThreadHeader header, ResSet[] resArray)
		{
			WaitCallback callback = delegate
			{
				var allUrls = new List<string>();

				foreach (ResSet res in resArray)
				{
					string[] link = res.Links[".jpg|.jpe|.jpeg|.gif|.png|.bmp"];
					allUrls.AddRange(link);
				}

				MethodInvoker m = delegate
				{
					// �T���l�C���r���[�A
					TwinThreadWindow win = threadTabController.FindWindow(header);
					if (threadTabController.Window == win)
					{
						if (thumbnailViewer == null)
							thumbnailViewer = new ThumbnailViewer();
						thumbnailViewer.AddImageFromUrlRange(allUrls.ToArray());
						thumbnailViewer.Show();
					}
				};
				Invoke(m);
			};
			ThreadPool.QueueUserWorkItem(callback);
		}*/
		#endregion

		void browser_AutoReloadChanged(object sender, AutoReloadChangedEventArgs e)
		{
			string key = e.Target.BoardInfo.Path;

			// �L�[���Ȃ���Βǉ�
			if (!autoReloadServerDic.ContainsKey(key))
			{
				autoReloadServerDic.Add(key, 0);
			}

			if (e.NewValue)
			{
				if (autoReloadServerDic[key] >= setAutoReloadLimit)
				{
					SetStatusBarInfo(StatusBarPanelIndex.Text, "�I�[�g�����[�h�͓�����" + setAutoReloadLimit + "�܂ł����ݒ�ł��܂���B");
					System.Media.SystemSounds.Beep.Play();
					e.Cancelled = true;
				}
				else
				{
					autoReloadServerDic[key]++;
#if DEBUG
					Console.WriteLine("AutoReload set! {0}={1}, {2}",
						key, autoReloadServerDic[key], e.Target.Subject);
#endif
				}
			}
			else
			{
				autoReloadServerDic[key]--;

#if DEBUG
				Console.WriteLine("AutoReload removed! {0}={1}, {2}",
					key, autoReloadServerDic[key], e.Target.Subject);

				if (autoReloadServerDic[key] < 0)
				{
					MessageBox.Show("���������ł��B");
				}
#endif
			}


		}

		void threadTabController_OnDestroy(ThreadControl window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}

			TabPage tab = (TabPage)window.Tag;
			int index = threadTabCtrl.TabPages.IndexOf(tab);

			threadTabCtrl.Select();
			threadTabCtrl.SelectedTab = null;
			threadTabCtrl.TabPages.Remove(tab);

			TabCloseAfterSelection(threadTabCtrl, index, settings.TabCloseAfterSelection);

			window.Dispose();
			window.Tag = null;

			tab.Dispose();
			tab.Tag = null;

			if (threadTabCtrl.TabCount == 0)
			{
				SetHeaderInfo(null);
			}

			UpdateToolBar();
		}

		// �^�u��I�������Ƃ��ɃX���b�h�������ōX�V���鏈��
		// �^�u�̈ړ������Ƃ��ɓ��삵�Ȃ��悤�ɂ��Ă���
		private Rectangle activeTabRect = Rectangle.Empty;
		void threadTabCtrl_MouseDown(object sender, MouseEventArgs e)
		{
			activeTabRect = Rectangle.Empty;
			if (e.Clicks == 1 && e.Button == MouseButtons.Left)
			{
				TabPage page = TabUtility.GetItemAt(threadTabCtrl, e.Location.X, e.Location.Y);
				if (page == null)
					return;
				int index = threadTabCtrl.TabPages.IndexOf(page);
				if (index >= 0)
					activeTabRect = threadTabCtrl.GetTabRect(index);
			}
		}
		void threadTabCtrl_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Clicks == 1 && e.Button == MouseButtons.Left &&
				activeTabRect.Contains(e.Location) && threadTabController.IsSelected)
			{
				ThreadControl client = threadTabController.Control;

				// �����ŃX���b�h���X�V
				if (settings.Thread.TabSelectedAfterReload)
				{
					TimeSpan t = DateTime.Now - client.LastCompletedDateTime;
					if (t.Seconds > 10)
						client.Reload();
				}
			}
		}


		ThreadListControl listTabController_OnCreate(BoardInfo board, bool newWindow)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			ThreadListControl viewer = listTabController.FindControl(board);

			if (viewer != null)
			{
				listTabCtrl.SelectedTab = (TabPage)viewer.Tag;
				return viewer;
			}
			else
			{
				if (listTabCtrl.TabCount == 0)
					newWindow = true;
				else
				{
					viewer = listTabController.Control;
					if (viewer != null && viewer.IsReading)
						newWindow = true;
				}
			}



			TabPage tab = null;
			int tabCount = listTabCtrl.TabCount;

			TwinListWindow win = null;

			// �V�K�E�C���h�E���쐬���邩�ǂ���
			if (tabCount == 0 || newWindow)
			{
				// �r���[�A���쐬
				ThreadListView listview = new ThreadListView(cache, settings, bookmarkRoot);
				listview.Selected += new EventHandler<ThreadListEventArgs>(threadListView_Selected);
				listview.ServerChanged += new EventHandler<ServerChangeEventArgs>(OnServerChanged);
				listview.InnerView.MouseUp += new MouseEventHandler(threadListView_MouseUp);
				listview.StatusTextChanged += new StatusTextEventHandler(ClientBase_StatusTextChanged);
				listview.Dock = DockStyle.Fill;
				listview.ContextMenuStrip = contextMenuListView;
				listview.ImageList = imageListLv;
				listview.NG924 = settings.NG924;
				ClientAddEvents(listview);
				viewer = listview;

				win = new TwinListWindow(viewer);

				tab = new TabPage();
				tab.Text = tab.ToolTipText = board.Name;
				tab.Controls.Add(viewer);
				tab.Tag = win;
				viewer.Tag = tab;

				AppendNewTab(listTabCtrl, tab);

				// ��[null�������Ȃ���SelectedIndexChanged�C�x���g���������Ȃ�
				if (tabCount == 0)
					listTabCtrl.SelectedTab = null;

				listTabCtrl.SelectedTab = tab;
			}
			else
			{
				tab = listTabCtrl.SelectedTab;
				tab.Text = tab.ToolTipText = board.Name;

				win = (TwinListWindow)tab.Tag;
				viewer = (ThreadListControl)win.Control;
			}

			// �^�u�̐F�Â�
			if (tabColorTable.ContainKey(board.Name))
			{
				win.ColorSet = tabColorTable.FromKey(board.Name);
			}
			else
			{
				win.ColorSet = TabColorSet.Default;
			}

			return viewer;
		}

		void listTabController_OnDestroy(ThreadListControl window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}

			TabPage tab = (TabPage)window.Tag;
			int index = listTabCtrl.TabPages.IndexOf(tab);

			listTabCtrl.Select();
			listTabCtrl.SelectedTab = null;
			listTabCtrl.TabPages.Remove(tab);

			TabCloseAfterSelection(listTabCtrl, index, settings.TabCloseAfterSelection);

			window.Dispose();
			window.Tag = null;

			tab.Dispose();
			tab.Tag = null;

			UpdateToolBar();
		}

		#endregion

		void TabUtility_ChangedPosition(object sender, EventArgs e)
		{
			UpdateToolBar();
		}

		/// <summary>
		/// �^�u������Ƃ��A���ɑI������^�u�����肷��
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="index">�����^�u�̃C���f�b�N�X</param>
		/// <param name="sel"></param>
		private void TabCloseAfterSelection(TabControl tab, int index, TabCloseAfterSelectionMode sel)
		{
			if (sel == TabCloseAfterSelectionMode.Left)
			{
				tab.SelectedIndex = Math.Max(index - 1, 0);
			}
			else if (sel == TabCloseAfterSelectionMode.Right)
			{
				tab.SelectedIndex = Math.Min(index, tab.TabCount - 1);
			}
		}


		#region ImageViewer
		private void ImageViewerInitialize()
		{
			if (imageViewer != null)
				throw new InvalidOperationException();

			// �摜�r���[�A
			imageViewer = new ImageViewer();
			imageViewer.Closing += new CancelEventHandler(imageViewer_Closing);
		}

		private void ImageViewerClose()
		{
			__closing = true;

			if (imageViewer != null)
				imageViewer.Close();
		}

		private void ImageViewerOpen(string url)
		{
			ImageViewerOpen(new string[] { url });
		}

		private void ImageViewerOpen(string[] urlArray)
		{
			if (imageViewer == null)
				ImageViewerInitialize();

			if (!imageViewer.Visible)
				imageViewer.Show();

			if (urlArray != null)
			{
				if (urlArray.Length == 1)
					imageViewer.OpenUrl(urlArray[0], true);
				else
					imageViewer.OpenUrls(urlArray);
			}
		}
		#endregion
		#endregion

		#region Private���\�b�h

		/// <summary>
		/// �w�肵���X���b�h�̊֘A�L�[���[�h���R���e�L�X�g���j���[�ŕ\�����܂��B
		/// </summary>
		/// <param name="header"></param>
		private void ShowRelatedKeywordContextMenu(ThreadHeader header)
		{
			try
			{
				string[] keywords = RelatedKeyword.GetKeywords(header);

				ContextMenuStrip context = new ContextMenuStrip();
				context.ShowImageMargin = context.ShowCheckMargin = false;

				foreach (string word in keywords)
				{
					ToolStripMenuItem menu = new ToolStripMenuItem(word);
					menu.Click += new EventHandler(RelatedKeyword_Clicked);

					context.Items.Add(menu);
				}

				context.Show(MousePosition);
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
				SetStatusBarInfo(0, ex.Message);
			}
		}

		private void RelatedKeyword_Clicked(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem)sender;

			X2chSubjectSearcher s = new X2chSubjectSearcher();
			SubjectSearchResult result = s.Search(item.Text);

			ThreadListControl list = listTabController.Create(dummySearchBoardInfo, true);
			list.SetItems(dummySearchBoardInfo, result.MatchThreads);
		}

		/// <summary>
		/// �O��̏I�����̏�Ԃ��J�����\�b�h
		/// </summary>
		private void OpenStartup()
		{
			string[] array = new string[settings.StartupUrls.Count];
			settings.StartupUrls.CopyTo(array, 0);

			foreach (string url in array)
				OpenAddress(url);
		}

		/// <summary>
		/// �����A�b�v�f�[�^
		/// </summary>
		private void UpdateCheck()
		{
			if (settings.UpdateCheck)
				TwinUpdate.Check();
		}

		/// <summary>
		/// �ꗗ��{�^���ɒǉ�
		/// </summary>
		/// <param name="table"></param>
		private void SetTableItaBotan(IBoardTable table)
		{
			if (table == null)
				return;

			CSharpToolBarButton button = ItaBotanFind(table);

			if (button == null)
			{
				ItaBotanSet(table, "�Q�����˂�", 0);
				settings.View.TableItaBotan = true;
			}
			else
			{
				ItaBotanRemove(button);
				settings.View.TableItaBotan = false;
			}
		}

		private void SaveSettingsAll()
		{
			settings.LastExitDay = DateTime.Now.Day;
			_2chTable.SaveTable(Settings.BoardTablePath);		// �ꗗ����ۑ�
			userTable.SaveTable(Settings.UserTablePath);		// ���[�U�[��`��ۑ�
			SaveSettings();		// ���ׂĂ̐ݒ��ۑ�
		}

		private void SaveItaBotan()
		{
			ItaBotan.Save(Settings.ItaBotanPath, cSharpToolBar.Buttons);	// �{�^����ۑ�
		}
		private void SaveTools()
		{
			tools.Save(Settings.ToolsFilePath);					// �O���c�[����ۑ�
		}

		private void SaveSettings2()
		{
			if (initializing)
				return;

			SaveWindowsUrl();
			Twinie.SerializingSettings(settings);
		}

		/// <summary>
		/// ���݊J���Ă���URL��ۑ�
		/// </summary>
		private void SaveWindowsUrl()
		{
			List<string> urls = new List<string>();

			try
			{
				// �J���Ă��郊�X�g��URL
				ThreadListControl[] listItems = listTabController.GetControls();
				foreach (ThreadListControl list in listItems)
				{
					// ���C�ɓ���܂��͌������ʃ^�u�͑Ώۂ���O��
					if (list.BoardInfo.Server == "dummy.addr")
						continue;

					urls.Add(list.BoardInfo.Url);
				}

				// �J���Ă���X���b�h��URL
				ThreadControl[] threadItems = threadTabController.GetControls();
				foreach (ThreadControl thread in threadItems)
				{
					if (thread.IsOpen)
						urls.Add(thread.HeaderInfo.Url);
				}
				settings.StartupUrls.Clear();
				settings.StartupUrls.AddRange(urls.ToArray());
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
		}

		/// <summary>
		/// �X���b�h�����������\�b�h
		/// </summary>
		/// <param name="thread">����E�C���h�E</param>
		/// <param name="delete">���O���폜���邩�ǂ���</param>
		private void ThreadCloseInternal(ThreadControl thread, bool delete)
		{
			if (thread == null)
			{
				throw new ArgumentNullException("thread");
			}
			if (thread.IsReading)
			{
				MessageBox.Show("�X���b�h����M���ɕ��邱�Ƃ͏o���܂���", "�����܂���",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ThreadHeader header = thread.HeaderInfo;
			header.NewResCount = 0;

			// ���������ɒǉ�
			AddClosedHistory(header);

			// �h�b�L���O�������݃o�[���烌�X�����폜
			dockWriteBar.Remove(header);

			// �X���b�h�����
			thread.Close();

			threadTabController.Destroy(thread);

			// ���O���폜
			if (delete)
				cache.Remove(header);

			SaveSettings2();

			// �X���b�h�ꗗ���X�V
			UpdateThreadInfo(header);
			UpdateTitleBar();
		}

		/// <summary>
		/// �N���C�A���g�C�x���g��o�^
		/// </summary>
		/// <param name="client"></param>
		private void ClientAddEvents(ClientBase client)
		{
			client.Loading += new EventHandler(ClientBase_Loading);
			client.Receive += new ReceiveEventHandler(ClientBase_Receive);
			client.Complete += new CompleteEventHandler(ClientBase_Complete);
		}

		/// <summary>
		/// �N���C�A���g�C�x���g���폜
		/// </summary>
		/// <param name="client"></param>
		private void ClientRemoveEvents(ClientBase client)
		{
			client.Loading -= new EventHandler(ClientBase_Loading);
			client.Complete -= new CompleteEventHandler(ClientBase_Complete);
			client.Receive -= new ReceiveEventHandler(ClientBase_Receive);
		}

		/// <summary>
		/// �w�肵���c�[�������s
		/// </summary>
		/// <param name="item"></param>
		private void RunTool(ToolItem item, string text)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item.FileName.StartsWith("$"))
			{
				InternalTool.Run(this, item, text);
			}
			else
			{
				ExternalTool.Run(this, item);
			}
		}

		/// <summary>
		/// ���ׂẴX���b�h�̃t�H���g�T�C�Y���X�V
		/// </summary>
		private void UpdateFontSize()
		{
			foreach (ThreadControl ctrl in threadTabController.GetControls())
				ctrl.FontSize = settings.Thread.FontSize;
		}

		/// <summary>
		/// �O���c�[���̃R���{�{�b�N�X���X�V
		/// </summary>
		private void UpdateToolsComboBox()
		{
			comboBoxTools.Items.Clear();
			comboBoxTools.Items.Add(String.Empty);

			// ����o�^����Ă��Ȃ��ꍇ�A�W���ŃX���b�h�^�C�g��������ǉ�
			if (tools.Count == 0)
			{
				tools.Add(new ToolItem("�X���^�C����", "$Find2ch", ""));
			}

			foreach (ToolItem item in tools)
				comboBoxTools.Items.Add(item);

			comboBoxTools.SelectedIndex = 0;
		}

		/// <summary>
		/// �c�[���o�[�̏�Ԃ��X�V
		/// </summary>
		private void UpdateToolBar()
		{
			// ���C���c�[���o�[
			toolBarButtonListClose.Enabled =
				toolBarButtonListNewThread.Enabled =
				toolBarButtonListReload.Enabled =
				toolBarButtonListStop.Enabled =
				toolBarButtonListSearch.Enabled =
				toolBarButtonListOpenUp.Enabled = listTabController.IsSelected;

			toolBarButtonViewTable.Pushed = settings.View.HideTable;
			toolBarButtonViewList.Pushed = settings.View.FillList;
			toolBarButtonViewThread.Pushed = settings.View.FillThread;

			toolBarButtonCaching.Pushed = settings.Caching;

			toolBarButtonLive.Pushed = settings.Livemode;
			toolBarButtonLive.ImageIndex = settings.Livemode ? Icons.LivemodeOn : Icons.LivemodeOff;

			toolBarButtonPatrol.Enabled = (patroller == null);

			toolBarButtonNGWords.Pushed = settings.NGWordsOn;
			toolBarButtonNGWords.ImageIndex = settings.NGWordsOn ? Icons.NGOn : Icons.NGOff;

			// �X���b�h�c�[���o�[
			toolBarButtonReload.Enabled =
				toolBarButtonStop.Enabled =
				toolBarButtonAutoReload.Enabled =
				toolBarButtonResExtract.Enabled =
				toolBarButtonDelete.Enabled =
				toolBarButtonWriteRes.Enabled =
				toolBarButtonViewChange.Enabled =
				toolBarButtonScroll.Enabled =
				toolBarButtonFind.Enabled =
				toolBarButtonClose.Enabled =
				toolBarButtonScrollTop.Enabled =
				toolBarButtonBookmark.Enabled = threadTabController.IsSelected;

			toolBarButtonViewTable.Enabled =
				toolBarButtonViewList.Enabled =
				toolBarButtonViewThread.Enabled = (display.Layout != DisplayLayout.TateYoko2);


			toolBarButtonBookmark.ToolTipText = "���C�ɓ���̓o�^�E����";

			if (threadTabController.IsSelected)
			{
				ThreadControl thread = threadTabController.Control;

				// ���C�ɓ���̏��
				bool contains = IsBookmarked(thread.HeaderInfo);
				toolBarButtonBookmark.Pushed = contains;
				toolBarButtonBookmark.ImageIndex = contains ? Icons.BookmarkOn : Icons.BookmarkOff;

				if (contains)
					toolBarButtonBookmark.ToolTipText += String.Format(" [{0}]", GetBookmarkFolderName(thread.HeaderInfo));

				toolBarButtonAutoReload.Pushed = thread.AutoReload;
				toolBarButtonAutoReload.ImageIndex = thread.AutoReload ? Icons.AutoReloadOn : Icons.AutoReloadOff;
				toolBarButtonScroll.ImageIndex = thread.AutoScroll ? Icons.AutoScrollOn : Icons.AutoScroll;

				// �X���b�h����M���̓��O�폜�܂��͕����Ȃ��悤�ɂ���
				toolBarButtonClose.Enabled =
					toolBarButtonDelete.Enabled = !thread.IsReading;
			}

			if (listTabController.IsSelected)
			{
				toolBarButtonListClose.Enabled =
					!listTabController.Control.IsReading;
			}
		}

		// ��ʍ\�����X�V
		private void UpdateLayout()
		{
			ViewSettings view = settings.View;

			if (display.Layout != DisplayLayout.TateYoko2)
			{
				treePanel.Visible = !view.HideTable;
				threadPanel.Visible = !view.FillList;
				listPanel.Visible = !view.FillThread;

				// �X���b�h�ꗗ���g�傷��Ȃ�X���b�h���\����
				if (view.FillList)
				{
					listPanel.Dock = DockStyle.Fill;
					threadPanel.Dock = DockStyle.None;

					// ���ꕪ�����Ɣ���Ă��܂��̂ŁA��\���ɂ���
					if (display.Layout == DisplayLayout.Extend1)
						treePanel.Visible = false;
				}
				// �X���b�h���g�傷��Ȃ�X���ꗗ���\����
				else if (view.FillThread)
				{
					listPanel.Dock = DockStyle.None;
					threadPanel.Dock = DockStyle.Fill;
				}
				// �ǂ���̏�Ԃł��Ȃ���Ό��ɖ߂�
				else
				{
					// �ʏ�܂��͉������Ȃ�DockStyle.Top�A�c�����Ȃ�DockStyle.Left					
					listPanel.Dock = (display.Layout != DisplayLayout.Tate3) ? DockStyle.Top : DockStyle.Left;
					threadPanel.Dock = DockStyle.Fill;
				}
			}
		}

		/// <summary>
		/// �X���b�h�ꗗ�ɕ\������Ă���X���b�h�����X�V
		/// </summary>
		/// <param name="header"></param>
		private void UpdateThreadInfo(ThreadHeader header)
		{
			// �X���b�h�̔ƁA���X���^�u�A���C�ɓ���^�u������
			BoardInfo[] boards = new BoardInfo[] { header.BoardInfo, dummyNextThreadBoardInfo, dummyBookmarkBoardInfo };
			foreach (BoardInfo board in boards)
			{
				ThreadListControl list = listTabController.FindControl(board);
				if (list != null)
					list.UpdateItem(header);
			}
		}

		/// <summary>
		/// �w�肵���C���q�L�[��������Ă����Ԃ��ǂ����𒲂ׂ�
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool KeyPushed(Keys key)
		{
			return (ModifierKeys & key) != 0;
		}

		/// <summary>
		/// �w�肵��client���A�N�e�B�u���ǂ����𔻒f
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		private bool ClientIsActive(ClientBase client)
		{
			if (threadTabController.IsSelected &&
				threadTabController.Control.Equals(client))
				return true;

			if (listTabController.IsSelected &&
				listTabController.Control.Equals(client))
				return true;

			return false;
		}

		// �X���b�h�c�[���o�[�̘g��`��
		private void threadToolPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			ControlPaint.DrawBorder3D(g, 0, 0,
				threadToolPanel.Width, threadToolPanel.Height, Border3DStyle.Etched);
		}

		/// <summary>
		/// �X���b�h�c�[���o�[�̈ʒu�ƃT�C�Y�𒲐�
		/// </summary>
		private void AdjustToolBar()
		{
			int width = threadPanel.Width;

			// �c�[���o�[�𒲐�
			int tbw = CommonUtility.CalcToolBarWidth(toolBarThread);
			toolBarThread.Left = width - tbw - 3;
			toolBarThread.Width = tbw;

			// �X���b�h����\�����郉�x���𒲐�
			labelThreadSubject.Left = labelBoardName.Width + 3;
			labelThreadSubject.Width = (toolBarThread.Left - 3) - labelThreadSubject.Left;
		}

		/// <summary>
		/// ����ݒ�
		/// </summary>
		/// <param name="board"></param>
		private void SetBoardInfo(BoardInfo board)
		{
			if (board != null)
			{
				textBoxAddress.Text = board.Url;
			}
			else
			{
				textBoxAddress.Text = String.Empty;
				SetStatusBarInfo(StatusBarPanelIndex.Text, String.Empty);
			}
		}

		/// <summary>
		/// �w�b�_����ݒ�
		/// </summary>
		/// <param name="header"></param>
		private void SetHeaderInfo(ThreadHeader header)
		{
			if (header != null)
			{
				if (header.BoardInfo.Name == String.Empty)
					header.BoardInfo.Name = FindBoardName(header.BoardInfo);
				
				labelThreadSubject.Width = 0;
				labelBoardName.Text = String.Format("[{0}]", header.BoardInfo.Name);
				labelThreadSubject.Text = String.Format("{0} ({1})", StringUtility.Unescape(header.Subject), header.ResCount);
				toolTip.SetToolTip(labelThreadSubject, labelThreadSubject.Text);
				threadTabCtrl.SelectedTab.Text = threadTabCtrl.SelectedTab.ToolTipText = StringUtility.Unescape(StringUtility.RemoveHeadSpace(header.Subject));
				textBoxAddress.Text = header.Url;
				SetStatusBarSize(header.GotByteCount);
				SetStatusBarResCount(header);
				SetStatusBarForce(header);
				SetStatusBarSamba24(header.BoardInfo);
				AdjustToolBar();
			}
			else
			{
				labelBoardName.Text =
					labelThreadSubject.Text =
					textBoxAddress.Text = String.Empty;

				toolTip.SetToolTip(labelThreadSubject, String.Empty);
				SetStatusBarInfo(StatusBarPanelIndex.Size, String.Empty);
				SetStatusBarInfo(StatusBarPanelIndex.Text, String.Empty);
				SetStatusBarInfo(StatusBarPanelIndex.ResCount, String.Empty);
				SetStatusBarInfo(StatusBarPanelIndex.Force, String.Empty);
				SetStatusBarInfo(StatusBarPanelIndex.TimerCount, String.Empty);
				SetStatusBarInfo(StatusBarPanelIndex.SambaCount, String.Empty);
				AdjustToolBar();
			}
		}

		/// <summary>
		/// �w�肵��board�Ɠ���URL���������݂̔ꗗ�̒����猟�����A�����擾
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		private string FindBoardName(BoardInfo board)
		{
			foreach (Category category in allTable.Items)
			{
				int index = category.Children.IndexOf(board);
				if (index >= 0)
					return category.Children[index].Name;
			}
			return "���s��";
		}

		/// <summary>
		/// ��M�T�C�Y���X�e�[�^�X�o�[�ɕ\��
		/// </summary>
		/// <param name="size"></param>
		private void SetStatusBarSize(int size)
		{
			SetStatusBarInfo(StatusBarPanelIndex.Size,
				String.Format("{0}KB", size / 1024));
		}

		/// <summary>
		/// �X���b�h�̐������X�e�[�^�X�o�[�ɕ\��
		/// </summary>
		/// <param name="h"></param>
		private void SetStatusBarForce(ThreadHeader h)
		{
			ThreadHeaderInfo info = new ThreadHeaderInfo(h);
			SetStatusBarInfo(StatusBarPanelIndex.Force, info.GetForceValue(settings.ForceValueType));
		}

		/// <summary>
		/// �X���b�h�̃��X�����X�e�[�^�X�o�[�ɕ\��
		/// </summary>
		/// <param name="h"></param>
		private void SetStatusBarResCount(ThreadHeader h)
		{
			SetStatusBarInfo(StatusBarPanelIndex.ResCount,
				String.Format("{0}/{1}", h.NewResCount, h.ResCount));
		}

		private void SetStatusBarSamba24(BoardInfo bi)
		{
			int sambaValue = (bi == null) ? 0 : PostDialog.samba[bi.Server];
			SetStatusBarInfo(StatusBarPanelIndex.SambaCount, "Samba: " + sambaValue.ToString() + "�b");
		}

		/// <summary>
		/// �X�e�[�^�X�o�[�̏���ݒ�
		/// </summary>
		/// <param name="index"></param>
		/// <param name="obj"></param>
		private void SetStatusBarInfo(int index, string text)
		{
			MethodInvoker m = delegate
			{
				statusBar.Panels[index].Text = text;
			};

			if (InvokeRequired)
				Invoke(m);
			else
				m();
		}

		/// <summary>
		/// �ŋߕ���������ǉ�
		/// </summary>
		/// <param name="header"></param>
		private void AddClosedHistory(ThreadHeader header)
		{
			if (header == null)
				return;

			if (closedThreadHistory.Items.Count >=
				settings.Thread.ClosedHistoryCount)
			{
				closedThreadHistory.Items.RemoveAt(
					closedThreadHistory.Items.Count - 1);
			}
			closedThreadHistory.Items.Remove(header);
			closedThreadHistory.Items.Insert(0, header);
			closedThreadHistory.Save();
		}

		/// <summary>
		/// �w�肵���̔����擾
		/// </summary>
		/// <param name="board">���������URL��񂪊i�[���ꂽBoardInfo�N���X</param>
		/// <returns></returns>
		private string GetBoardName(BoardInfo board)
		{
			foreach (Category cate in allTable.Items)
			{
				int index = cate.Children.IndexOf(board);
				if (index >= 0)
					return cate.Children[index].Name;
			}
			return "���s��";
		}

		/// <summary>
		/// �X�N���b�v�G�f�B�^��\��
		/// </summary>
		private void ShowScrapEditor()
		{
			ScrapEditorDialog dlg = new ScrapEditorDialog(Settings.ScrapFolderPath);
			dlg.Show();
		}

		/// <summary>
		/// �ꗗ���\���ɂ���
		/// </summary>
		/// <param name="hide"></param>
		private void ViewHideTable(bool hide)
		{
			settings.View.HideTable = hide;
			UpdateLayout();
			UpdateToolBar();
		}

		/// <summary>
		/// �X���b�h�ꗗ���g�傷��
		/// </summary>
		/// <param name="fill"></param>
		private void ViewFillList(bool fill)
		{
			settings.View.FillList = fill;
			settings.View.FillThread = false;
			UpdateLayout();
			UpdateToolBar();
		}

		/// <summary>
		/// �X���b�h���g�傷��
		/// </summary>
		/// <param name="fill"></param>
		private void ViewFillThread(bool fill)
		{
			settings.View.FillThread = fill;
			settings.View.FillList = false;
			UpdateLayout();
			UpdateToolBar();
		}

		/// <summary>
		/// �擾��̃X�����Ⴄ�T�[�o�[�ŁA
		/// </summary>
		/// <param name="currentHeader"></param>
		/// <param name="newHeader"></param>
		/// <returns></returns>
		private BoardInfo GetRetryServer(ThreadHeader currentHeader, ThreadHeader newHeader)
		{
			if (currentHeader != null)
			{
				if (currentHeader.BoardInfo.Path == newHeader.BoardInfo.Path &&
					currentHeader.BoardInfo.Server != newHeader.BoardInfo.Server)
				{
					return currentHeader.BoardInfo;
				}
			}
			return null;
		}

		// �w�肵���X���b�h�̎��X��������
		private void BeginNextThreadCheck(ThreadHeader header)
		{
			if (nextThreadChecker == null)
			{
				nextThreadChecker = new NextThreadChecker() { 
					HighLevelMatching = settings.NextThreadChecker_HighLevelMatch };
				nextThreadChecker.Success += new ThreadHeaderEventHandler(NextThreadChecker_Success);
				nextThreadChecker.CheckBegin(header);
			}
			else
			{
				MessageBox.Show("���̃X���b�h�̎��X�����������ł�", "������Ƒ҂��Ă�������",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		// �X�V�`�F�b�N�\�Ȕ��ǂ����𔻒f
		// board�����C�ɓ���A�S�����X���A�������ݗ����̂ǂꂩ�ł����true��Ԃ�
		private bool IsUpdateCheckable(BoardInfo board)
		{
			foreach (BoardInfo type in updateCheckableTypes)
			{
				if (board.Equals(type))
					return true;
			}
			return false;
		}

		/// <summary>
		/// �{�^���ɃX���b�h�^�C�g��������o�^
		/// </summary>
		/// <param name="searchString">����������</param>
		private void RegistSearchBotan(string searchString)
		{
			SubjectSearchOptionDialog dlg = new SubjectSearchOptionDialog
			{
				SearchOrder = SubjectSearchOrder.Ascending,
				SearchSorting = SubjectSearchSorting.Modified,
				SearchResultCount = 50,
				SearchString = searchString,
				SearchCaption = searchString,
			};
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				ItaBotanSet(new SearchBotan
				{
					SearchString = dlg.SearchString,
					Caption = dlg.SearchCaption,
					SearchOrder = dlg.SearchOrder,
					SearchSorting = dlg.SearchSorting
				});
			}
		}

		// ���ׂẴ����N���J��
		private void OpenLinks(LinkCollection links)
		{
			LinkInfoCollection lic = IEComThreadBrowser.linkCollection;

			foreach (string uri in links)
				OpenLink(uri, lic.IndexOf(uri));
		}

		private bool OpenLink(string uri, LinkInfo linkInfo)
		{
			if (linkInfo != null)
			{
				string text = String.Format("{0} ({1})\r\n���̃����N���J���܂����H",
					linkInfo.Uri, linkInfo.Text);

				DialogResult r = MessageBox.Show(text, "NG�w�肳�ꂽURL",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (r == DialogResult.No)
					return false;
			}

			OpenAddress(uri);
			return true;
		}

		// ���ׂẴX���b�h������ꂽ�ꍇ�AautoReloadServerDic�̒l�͂��ׂ�0�ɂȂ�͂�
		private void CheckAutoReloadServerDic()
		{
			if (threadTabController.WindowCount > 0)
				return;

#if DEBUG
			foreach (KeyValuePair<string, int> kv in autoReloadServerDic)
			{
				if (kv.Value != 0)
					MessageBox.Show(kv.Key + "=" + kv.Value, "Error");
			}
#else
			autoReloadServerDic.Clear();
#endif
		}
		#endregion

		#region Public���\�b�h
		/// <summary>
		/// �w�肵��URL���J��
		/// </summary>
		/// <param name="url">�J���X���b�h��URL</param>
		public void OpenAddress(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url == String.Empty)
				return;

			BoardInfo board = null;
			ThreadHeader header = null;

			if ((header = URLParser.ParseThread(url)) != null)
			{
				header.BoardInfo.Name = GetBoardName(header.BoardInfo);

				int[] indices = ResReference.GetArray(url);
				ThreadOpen(header, true, indices);
				return;
			}
			else if ((board = URLParser.ParseBoard(url)) != null && allTable.Contains(board))
			{
				board.Name = GetBoardName(board);
				ListOpen(board, true);
				return;
			}

			// �摜�r���[�A�ŊJ���Ă݂�
			if (settings.ImageViewer &&
				Regex.IsMatch(url, @"(\.jpg|\.jpeg|\.gif|\.bmp|\.png|\.jpg\.html)$", RegexOptions.IgnoreCase))
			{
				ImageViewerOpen(url);
				return;
			}

			CommonUtility.OpenWebBrowser(url);
		}

		/// <summary>
		/// 2ch�݊���dat�t�@�C�����J��
		/// </summary>
		public void OpenDat()
		{
			OpenThreadDialog dlg = new OpenThreadDialog(cache, allTable);

			if (dlg.ShowDialog(this) == DialogResult.OK)
				ThreadOpen(dlg.HeaderInfo, true);
		}

		/// <summary>
		/// monalog�t�@�C�����J��
		/// </summary>
		public void OpenMonalog()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "monalog �t�@�C�� (*.xml)|*.xml";

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				ThreadHeader header = ThreadUtil.OpenMonalog(cache, dlg.FileName);
				ThreadOpen(header, true);
			}
		}

		private string GetThreadSaveFileName(ThreadHeader h)
		{
			// �X���b�h�� #key
			return StringUtility.ReplaceInvalidPathChars(h.Subject, "-") + " #" + h.Key;
		}

		/// <summary>
		/// 2ch��html�t�@�C���ɂ��ĕۑ�
		/// </summary>
		public void SaveHtml()
		{
			if (threadTabController.IsSelected)
			{
				ThreadHeader header = threadTabController.HeaderInfo;
				saveFileDialog.FileName = GetThreadSaveFileName(header);
				saveFileDialog.FilterIndex = 2;
				saveFileDialog.Title = header.Subject + "��html�`���ŕۑ�";

				if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
				{
					SelectSkinDialog dlg = new SelectSkinDialog();
					HtmlSkin htmlSkin = new HtmlSkin();

					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						htmlSkin.Load(dlg.SelectedSkinFolderPath);
						ThreadUtil.SaveHtml(cache, header, saveFileDialog.FileName, htmlSkin);
					}
				}
			}
		}

		/// <summary>
		/// 2ch��dat�t�@�C���ɂ��ĕۑ�
		/// </summary>
		public void SaveDat()
		{
			if (threadTabController.IsSelected)
			{
				ThreadHeader header = threadTabController.HeaderInfo;
				saveFileDialog.FileName = GetThreadSaveFileName(header);
				saveFileDialog.FilterIndex = 3;
				saveFileDialog.Title = header.Subject + "��dat�`���ŕۑ�";

				if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
					ThreadUtil.SaveDat(cache, header, saveFileDialog.FileName);
			}
		}

		/// <summary>
		/// monalog�`���ŃX���b�h��ۑ�
		/// </summary>
		public void SaveMonalog()
		{
			if (threadTabController.IsSelected)
			{
				ThreadHeader header = threadTabController.HeaderInfo;
				saveFileDialog.FileName = GetThreadSaveFileName(header);
				saveFileDialog.FilterIndex = 4;
				saveFileDialog.Title = header.Subject + "��monalog�`���ŕۑ�";

				if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
					ThreadUtil.SaveMonalog(cache, header, saveFileDialog.FileName);
			}
		}

		/// <summary>
		/// NG���[�h��On/Off�؂芷��
		/// </summary>
		/// <param name="enable">�L���ɂ���Ȃ�true�A�����ɂ���Ȃ�false</param>
		public void NGWordsSwitch(bool enable)
		{
			settings.NGWordsOn = enable;
			UpdateToolBar();

		}

		/// <summary>
		/// �������[�h
		/// </summary>
		public void Livemode()
		{
			if (settings.Livemode)
			{
				// �ꗗ�A�X���ꗗ�A�X���b�h��\���A�h�b�L���O�������݃o�[���\��
				settings.View.HideTable = false;
				settings.View.FillList = false;
				settings.View.FillThread = false;
				dockWriteBar.Visible = settings.View.DockWriteBar;

				// �I�[�g�����[�hOff�A�I�[�g�X�N���[��Off
				if (threadTabController.IsSelected)
				{
					threadTabController.Control.AutoReload =
						threadTabController.Control.AutoScroll = false;
				}
			}
			else
			{
				// �ꗗ�A�X���ꗗ���\���A�X���b�h�A�h�b�L���O�������݃o�[��\��
				settings.View.HideTable = true;
				settings.View.FillList = false;
				settings.View.FillThread = true;
				dockWriteBar.Visible = true;

				// �I�[�g�����[�hOn�A�I�[�g�X�N���[��On
				if (threadTabController.IsSelected)
				{
					threadTabController.Control.AutoReload =
						threadTabController.Control.AutoScroll = true;
				}
			}

			settings.Livemode = !settings.Livemode;

			UpdateLayout();
			UpdateToolBar();
		}

		/// <summary>
		/// ���ݒ�_�C�A���O��\��
		/// </summary>
		public void ShowOption()
		{
			OptionDialog dlg = new OptionDialog(allTable, koteman, settings);

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				settings.SetCustomTypeConverter();
				settings.Thread.CorrectAutoReloadInterval();

				IEComThreadBrowser.AutoRefreshTimers.Interval =
					settings.Thread.AutoReloadInterval;

				if (String.Compare(cache.BaseDirectory, settings.CacheFolderPath, true) != 0)
				{
					cache.BaseDirectory = settings.CacheFolderPath;
					SaveThreadGroupList();
				}

				TwinDll.AddWriteSection = settings.AddWriteSection;
				WebRequest.DefaultWebProxy = settings.Net.RecvProxy;

				UpdateToolBar();
			}
		}

		/// <summary>
		/// ���ׂẴE�C���h�E�����
		/// </summary>
		public bool CloseAllWindow()
		{
			// �I�[�g�����[�h���~�߂�
			IEComThreadBrowser.AutoRefreshTimers.Clear();

			// �X���ꗗ
			ThreadListControl[] lists = listTabController.GetControls();
			foreach (ThreadListControl window in lists)
				window.Close();

			//	bool ignore = false;

			// �X���b�h
			ThreadControl[] threads = threadTabController.GetControls();
			foreach (ThreadControl window in threads)
			{
				/*
				while (window.IsReading && !ignore)
				{
					DialogResult r = MessageBox.Show("�X���b�h��ǂݍ��ݒ��ł����A�I�����Ă���낵���ł����H",
						window.HeaderInfo.Subject, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

					if (r == DialogResult.Ignore)
					{
						ignore = true;
						break;
					}
					else if (r == DialogResult.Abort)
					{
						return false;
					}
				}
				*/

				AddClosedHistory(window.HeaderInfo);
				window.Close();
			}
			//listTabController.Clear();
			//threadTabController.Clear();

			return true;
		}

		/// <summary>
		/// �w�肵���X���b�h�����C�ɓ���ɓo�^����Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool IsBookmarked(ThreadHeader header)
		{
			return bookmarkRoot.Contains(header) || warehouseRoot.Contains(header);
		}

		/// <summary>
		/// �w�肵���X���b�h���o�^����Ă��邨�C�ɓ���̃t�H���_�����擾���܂��B
		/// ���C�ɓ���ɑ��݂��Ȃ���� null ��Ԃ��܂��B
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public string GetBookmarkFolderName(ThreadHeader header)
		{
			BookmarkThread t = bookmarkRoot.Search(header);

			if (t != null)
				return t.Parent.Name;

			t = warehouseRoot.Search(header);

			if (t != null)
				return t.Parent.Name;

			return null;
		}

		/// <summary>
		/// �����O�I�����܂�
		/// </summary>
		public void OysterLogon()
		{
			Twinie.OysterLogon();
			UpdateTitleBar();
		}

		/// <summary>
		/// �����O�A�E�g���܂�
		/// </summary>
		public void OysterLogout()
		{
			Twinie.OysterLogout();
			UpdateTitleBar();
		}
		#endregion

		#region �X���b�h�ꗗ���상�\�b�h
		/// <summary>
		/// �w�肵�����J���X���b�h�ꗗ���擾
		/// </summary>
		/// <param name="info">�J���̏��</param>
		/// <param name="newTab">�V�����^�u�ŊJ���ꍇ��true�A�����łȂ����false���w��</param>
		public void ListOpen(BoardInfo info, bool newTab)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			// ���̃X���b�h�̔��ړ]���Ă��Ȃ����ǂ������`�F�b�N
			//			BoardLinker linker = new BoardLinker(cache);
			//			BoardInfo moveto = linker.GetLinked(info, true);
			//
			//			if (moveto != null) // ���ړ]���Ă����ꍇ�͔�������������
			//				info = moveto;

			// �r���[�A���쐬
			ThreadListControl client = listTabController.Create(info, newTab);
			if (client.IsReading)
			{
				return;
			}

			client.Online = IsOnline;
			client.Open(info);
			UpdateToolBar();

			SaveSettings2();
		}

		/// <summary>
		/// �A�N�e�B�u�ȃX���b�h�ꗗ�������[�h���V�����X���擾
		/// </summary>
		public void ListReload()
		{
			if (listTabController.IsSelected)
			{
				BoardInfo bi = listTabController.HeaderInfo;
				if (bi.Server == "dummy.addr")
				{
					if (bi.Path == dummyBookmarkBoardInfo.Path)
						BookmarkPatrol(bookmarkRoot, true, true);
				}
				else
				{
					listTabController.Control.Online = IsOnline;
					listTabController.Control.Reload();
				}
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȃX���b�h�ꗗ�̓ǂݍ��݂𒆎~
		/// </summary>
		public void ListStop()
		{
			if (listTabController.IsSelected)
				listTabController.Control.Stop();
		}

		private bool CanListClose(ThreadListControl list)
		{
			if (list == null || list.IsReading)
				return false;

			if (list == listBookmarkControl && patroller != null && patroller.IsPatrolling)
			{
				if (MessageBox.Show("���񒆂̂��C�ɓ���^�u����Ă���낵���ł����H", "�m�F",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// �A�N�e�B�u�ȃX���b�h�ꗗ�����
		/// </summary>
		public void ListClose()
		{
			if (listTabController.IsSelected)
			{
				ThreadListControl list = listTabController.Control;

				if (list.IsReading)
				{
					MessageBox.Show("�X���b�h�ꗗ����M���ɕ��邱�Ƃ͏o���܂���", "�����܂���",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				if (!CanListClose(list))
					return;

				list.Close();
				listTabController.Destroy(list);

				SaveSettings2();
			}
		}

		/// <summary>
		/// �X���b�h�ꗗ�����_�C�A���O��\��
		/// </summary>
		public void ListSearch()
		{
			if (listTabController.IsSelected)
			{
				ThreadListControl list = listTabController.Control;

				if (!CanListClose(list))
					return;

				// ���ɊJ����Ă���Έ�[����
				if (listSearcher != null)
				{
					listSearcher.Close();
					((ThreadListControl)listSearcher.Tag).Closed -= new EventHandler(ThreadControl_Closed);
				}

				// �����N���X��������
				list.Closed += new EventHandler(ThreadListControl_Closed);
				listSearcher = new ThreadListSearchDialog(list.BeginSearch(), settings.Search.ListSearch);
				listSearcher.Tag = list;
				listSearcher.Owner = this;
				listSearcher.Show();
			}
		}

		/// <summary>
		/// ���ׂẴX���b�h�ꗗ�����
		/// </summary>
		public void ListCloseAll()
		{
			if (!ClosingConfirmDialog())
				return;

			ThreadListControl[] controls = listTabController.GetControls();
			foreach (ThreadListControl ctrl in controls)
			{
				if (CanListClose(ctrl))
				{
					ctrl.Close();
					listTabController.Destroy(ctrl);
				}
			}
		}

		/// <summary>
		/// �A�N�e�B�u�łȂ��X���b�h�ꗗ�����
		/// </summary>
		public void ListCloseNotActive()
		{
			if (!ClosingConfirmDialog())
				return;

			ListTabController list = listTabController;

			if (list.WindowCount > 0)
			{
				ThreadListControl active = list.Control;
				ThreadListControl[] controls = list.GetControls();

				foreach (ThreadListControl ctrl in controls)
				{
					if (active != ctrl && CanListClose(ctrl))
					{
						ctrl.Close();
						list.Destroy(ctrl);
					}
				}
			}
		}

		/// <summary>
		/// �A�N�e�B�u�Ȕ̍X�V�X�����J��
		/// </summary>
		public void ListOpenUpThreads()
		{
			if (listTabController.IsSelected)
			{
				int cnt = 0;

				foreach (ThreadHeader h in listTabController.Control.Items)
				{
					if (h.SubNewResCount > 0)
					{
						if (++cnt == 15)
						{
							/*
							if (MessageBox.Show("�X���b�h�J�������ł��B�B\r\n�J��������Ɠ��삪�s����ɂȂ�܂����A����ł��J���܂����H", "�J����������",
								MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
							{
								break;
							}*/
						}
						ThreadOpen(h, true);
					}
				}
			}
		}

		/// <summary>
		/// ���ׂĂ̊����X���b�h���ꗗ�\��
		/// </summary>
		public void ListAllThreads()
		{
			List<ThreadHeader> items = Twin.IO.GotThreadListReader.GetAllThreads(cache, allTable);
			ThreadListControl list = listTabController.Create(dummyAllThreadsBoardInfo, true);
			list.SetItems(dummyAllThreadsBoardInfo, items);
		}

		/// <summary>
		/// �ŋߏ������񂾃X���b�h���ꗗ�\��
		/// </summary>
		public void ListWrittenThreads()
		{
			ThreadListControl list = listTabController.Create(dummyWrittenBoardInfo, true);

			// �ŋߏ������񂾓��t���Ƀ\�[�g
			IComparer comparer = new ListViewItemComparer(SortOrder.Descending, ThreadListView.ColumnNumbers.LastWritten);
			ThreadHeader[] array = new ThreadHeader[writtenThreadHistory.Items.Count];
			writtenThreadHistory.Items.CopyTo(array, 0);
			Array.Sort(array, comparer);

			List<ThreadHeader> sortedList = new List<ThreadHeader>();
			sortedList.AddRange(array);

			list.SetItems(dummyWrittenBoardInfo, sortedList);
		}

		/// <summary>
		/// �����C���f�b�N�X���Đ���
		/// </summary>
		private void ListIndexing()
		{
			if (listTabController.IsSelected)
			{
				try
				{
					ClientBase.Stopper.Reset();
					ThreadIndexer.Indexing(cache, listTabController.HeaderInfo);
					//				WroteHistoryIndexer.Indexing(cache, listTabController.HeaderInfo);
				}
				finally
				{
					ClientBase.Stopper.Set();
				}
			}
		}

		/// <summary>
		/// �w�肵����������s����
		/// </summary>
		/// <param name="operate"></param>
		public void ListOperator(TabOperation operate)
		{
			switch (operate)
			{
				case TabOperation.Close:
					ListClose();
					break;
				case TabOperation.Reload:
					ListReload();
					break;
			}
		}

		/// <summary>
		/// �w�肵����������s����
		/// </summary>
		/// <param name="operate"></param>
		public void ListOperator(ListOperation operate)
		{
			ReadOnlyCollection<ThreadHeader> items =
				listTabController.Control.SelectedItems;

			if (items.Count == 0)
				return;

			switch (operate)
			{
				case ListOperation.Open:
					ThreadBeforeOpen(items[0], false);
					break;
				case ListOperation.NewOpen:
					ThreadBeforeOpen(items[0], true);
					break;
				case ListOperation.Delete:
					ThreadDelete(items[0]);
					break;
				case ListOperation.NewResPopup:
					PopupTest.PopupNewRes(cache, items);
					break;
				case ListOperation.Popup1:
					PopupTest.Popup1(cache, items);
					break;
			}
		}
		#endregion

		#region �L���b�V�����𑀍상�\�b�h
		/// <summary>
		/// �L���b�V��������
		/// </summary>
		public void CacheSearch()
		{
			if (cacheSearcher == null)
			{
				SearchSettings.CacheSearchSettings css =
					settings.Search.CacheSearch;

				cacheSearcher = new CacheSearchDialog(listTabController, cache, allTable);
				cacheSearcher.Closed += new EventHandler(CacheSearcher_Closed);
				cacheSearcher.Owner = this;
				cacheSearcher.Show();
			}
		}

		/// <summary>
		/// �w�肵���̃L���b�V�����J��
		/// </summary>
		/// <param name="board"></param>
		public void CacheOpen(BoardInfo board)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			// �I�t���C�����[�h�ŊJ��
			ThreadListControl list = listTabController.Create(board, true);
			list.Stop();
			list.Online = false;
			list.Open(board);
		}

		/// <summary>
		/// �w�肵���̃��O�����ׂč폜
		/// </summary>
		/// <param name="board">���O���폜����̏�� (null���w�肷��Ƃ��ׂẴ��O���폜)</param>
		public void CacheClear(BoardInfo board)
		{
			string name =
				(board != null) ? board.Name : "���ׂĂ�";

			DialogResult r =
				MessageBox.Show(name + "�̊������O�����ׂč폜���܂� (�������ݗ������폜����܂�)�B\r\n��낵���ł����H", "�폜�m�F",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (r == DialogResult.Yes)
			{
				if (board != null)
					cache.Remove(board);
				else
					cache.Clear();
			}
		}

		/// <summary>
		/// �w�肵���̑��e���ꗗ�\��
		/// </summary>
		/// <param name="board"></param>
		public void DraftOpen(BoardInfo board)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			List<ThreadHeader> items = new List<ThreadHeader>();
			DraftBox box = new DraftBox(cache);
			Draft[] draftArray = box.Load(board);

			foreach (Draft draft in draftArray)
			{
				ThreadHeader h = draft.HeaderInfo;
				h.Tag = draft;
				items.Add(h);
			}

			ThreadListControl listControl = listTabController.Create(board, true);
			listControl.SetItems(board, items);
		}

		/// <summary>
		/// �w�肵�����e��ҏW
		/// </summary>
		/// <param name="draft"></param>
		public void DraftEdit(Draft draft)
		{
			if (draft == null)
			{
				throw new ArgumentNullException("draft");
			}

			// �ҏW���鑐�e�����X�g����폜
			DraftBox box = new DraftBox(cache);
			box.Remove(draft.HeaderInfo.BoardInfo, draft);

			ThreadPostRes(draft);
		}

		/// <summary>
		/// �w�肵���̏������ݗ����ꗗ��\��
		/// </summary>
		/// <param name="board">�������ݗ�����\�������</param>
		public void HistoryOpen(BoardInfo board)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			string filePath = kakikomi.GetKomiPath(board);

			if (File.Exists(filePath))
				Process.Start(filePath);

			//			ThreadListControl list = listTabController.Create(board, settings.ListAlwaysNewTab);
			//			list.OpenHistory(board);
			//			SetBoardInfo(board);
		}
		//
		//		/// <summary>
		//		/// �w�肵���̏������ݗ�����\��
		//		/// </summary>
		//		/// <param name="board">�������ݗ�����\������X���b�h</param>
		//		public void HistoryOpen(ThreadHeader header)
		//		{
		//			if (header == null) {
		//				throw new ArgumentNullException("header");
		//			}
		//
		//			MessageBox.Show("������Ƒ҂���");
		//			/*
		//			ThreadControl thread = threadTabController.Create(header, true);
		//			thread.OpenHistory(header);
		//			SetHeaderInfo(header);
		//			*/
		//		}

		/// <summary>
		/// �w�肵���̏������ݗ������폜
		/// </summary>
		/// <param name="board">�폜�Ώۂ̔� (null���w�肷��Ƃ��ׂĂ̔��폜)</param>
		public void HistoryClear(BoardInfo board)
		{
			string name =
				(board != null) ? board.Name : "���ׂĂ�";

			DialogResult r =
				MessageBox.Show(board.Name + "�̏������ݗ��������ׂč폜���܂��B��낵���ł����H", "�폜�m�F",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (r == DialogResult.Yes)
			{
				if (board != null)
					kakikomi.Delete(board);
				else
					Directory.Delete(Settings.KakikoFolderPath);
			}
		}
		#endregion

		#region ���상�\�b�h
		/// <summary>
		/// �����_�C�A���O��\��
		/// </summary>
		public void BoardSearch()
		{
			BoardSearchDialog dlg = new BoardSearchDialog(this);
			dlg.ShowDialog(this);
		}
		#endregion

		#region �X���b�h���상�\�b�h
		/// <summary>
		/// �X���b�h���J���O�̕��ʏ���
		/// </summary>
		/// <param name="header"></param>
		/// <param name="newTab"></param>
		private void ThreadBeforeOpen(ThreadHeader header, bool newTab)
		{
			// ���X���o
			if (header.Tag is ThreadExtractInfo)
			{
				ThreadOpen(header, (ThreadExtractInfo)header.Tag, newTab);
			}
			// ���e
			else if (header.Tag is Draft)
			{
				DraftEdit((Draft)header.Tag);
				return;
			}
			else
			{
				ThreadOpen(header, newTab);
			}

			if (settings.View.AutoFillThread)
				ViewFillThread(true);
		}

		/// <summary>
		/// �w�肵���X���b�h���J���\��
		/// </summary>
		/// <param name="header"></param>
		/// <param name="newTab"></param>
		public void ThreadOpen(ThreadHeader header, bool newTab)
		{
			ThreadOpen(header, newTab, null);
		}

		/// <summary>
		/// �w�肵���X���b�h���J���\��
		/// </summary>
		/// <param name="header"></param>
		/// <param name="newTab"></param>
		/// <param name="indices"></param>
		public void ThreadOpen(ThreadHeader header, bool newTab, int[] indices)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			BoardInfo retryServer = null;

			if (threadTabController.IsSelected)
			{
				retryServer = GetRetryServer(
					threadTabController.Control.HeaderInfo, header);
			}

			// �r���[�A���쐬
			ThreadControl ctrl = threadTabController.Create(header, newTab);
			ThreadHeader prevheader = ctrl.HeaderInfo;

			if (ctrl.IsReading)
			{
				return;
			}

			// �X���b�h���J����Ă���΁A���̃X���b�h����������ɒǉ�
			if (prevheader != null)
			{
				prevheader.NewResCount = 0;
				UpdateThreadInfo(prevheader);
				AddClosedHistory(prevheader);
			}

			ctrl.RetryServer = retryServer;
			ctrl.Open(header, indices);

			UpdateTitleBar();
			SaveSettings2();
		}

		/// <summary>
		/// �w�肵���X���b�h���J���\��
		/// </summary>
		/// <param name="header"></param>
		/// <param name="newTab"></param>
		public void ThreadOpen(ThreadHeader header, ThreadExtractInfo info, bool newTab)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			// �r���[�A���쐬
			ThreadControl ctrl = threadTabController.Create(header, newTab);

			// ���������ɒǉ�
			if (ctrl.HeaderInfo != null)
				AddClosedHistory(header);

			ctrl.Stop();
			ctrl.OpenExtract(header, info);
			SetHeaderInfo(header);
		}

		/// <summary>
		/// �A�N�e�B�u�ȃX���b�h�������[�h���V�����X���擾
		/// </summary>
		public void ThreadReload()
		{
			if (threadTabController.IsSelected)
				threadTabController.Control.Reload();
		}

		/// <summary>
		/// ���ׂẴX���b�h���X�V
		/// </summary>
		public void ThreadReloadAll()
		{
			ThreadControl[] controls = threadTabController.GetControls();

			foreach (ThreadControl thread in controls)
				thread.Reload();
		}

		/// <summary>
		/// �A�N�e�B�u�ȃX���b�h�̓ǂݍ��݂𒆎~
		/// </summary>
		public void ThreadStop()
		{
			if (threadTabController.IsSelected)
				threadTabController.Control.Stop();
		}

		/// <summary>
		/// �X���b�h�����������邽�߂̃_�C�A���O��\��
		/// </summary>
		public void ThreadFind()
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl control = threadTabController.Control;

				// ���ɊJ����Ă���Έ�[����
				if (threadSearcher != null)
				{
					threadSearcher.Close();
					((ThreadControl)threadSearcher.Tag).Closed -= new EventHandler(ThreadControl_Closed);
				}

				// �����N���X��������
				control.Closed += new EventHandler(ThreadControl_Closed);
				threadSearcher = new ThreadSearchDialog(control.BeginSearch(), settings.Search.ThreadSearch);
				threadSearcher.Tag = control;
				threadSearcher.Owner = this;
				threadSearcher.Show();
			}
		}

		/// <summary>
		/// ���X�𒊏o���邽�߂̃_�C�A���O��\��
		/// </summary>
		public void ThreadExtract()
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl control = threadTabController.Control;
				AbstractExtractor extractor = control.BeginExtract();

				// �����񂪑I������Ă���ꍇ�A�����ɒ��o�\��
				if (control.SelectedText != String.Empty)
				{
					extractor.NewWindow = true;
					extractor.InnerExtract(control.SelectedText, ResSetElement.All);
					return;
				}

				// ���ɊJ����Ă���Έ�[����
				if (resExtract != null)
				{
					resExtract.Close();
					((ThreadControl)resExtract.Tag).Closed -= new EventHandler(ThreadControl_Closed);
				}

				// �����N���X��������
				control.Closed += new EventHandler(ThreadControl_Closed);
				resExtract = new ResExtractDialog(extractor, settings.Search.ResExtract);
				resExtract.Tag = control;
				resExtract.Owner = this;
				resExtract.Show();
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȃX���b�h�����
		/// </summary>
		/// <param name="delete">���O���폜���ĕ��邩�ǂ���</param>
		public void ThreadClose(bool delete)
		{
			if (threadTabController.IsSelected)
				ThreadCloseInternal(threadTabController.Control, delete);
		}

		/// <summary>
		/// �w�肵���X���b�h�̃��O���폜
		/// </summary>
		/// <param name="header"></param>
		public void ThreadDelete(ThreadHeader header)
		{
			ThreadControl control = threadTabController.FindControl(header);
			if (control != null)
				ThreadCloseInternal(control, true);
			else
			{
				cache.Remove(header);
				ThreadListControl list = listTabController.FindControl(header.BoardInfo);
				if (list != null)
					list.UpdateItem(header);
			}
		}

		/// <summary>
		/// �I�[�g�����[�h��L���܂��͉���
		/// </summary>
		/// <param name="enable">�L���ɂ���ꍇ��true�A�����ɂ���ꍇ��false</param>
		public void ThreadSetAutoReload(bool enable)
		{
			if (threadTabController.IsSelected)
			{
				threadTabController.Control.AutoReload = enable;
				UpdateToolBar();
			}
			else // �X���b�h���I������Ă��Ȃ��ꍇ�͖���
				enable = false;

			autoReloadTimerCounter.Enabled = enable;
		}

		/// <summary>
		/// �I�[�g�X�N���[����L���܂��͉���
		/// </summary>
		/// <param name="enable">�L���ɂ���ꍇ��true�A�����ɂ���ꍇ��false</param>
		public void ThreadSetAutoScroll(bool enable)
		{
			if (threadTabController.IsSelected)
			{
				threadTabController.Control.AutoScroll = enable;
				UpdateToolBar();
			}
		}

		/// <summary>
		/// �X���b�h���X�N���[��
		/// </summary>
		/// <param name="down">���փX�N���[������ꍇ��true�A��փX�N���[������ꍇ��false</param>
		public void ThreadScroll(bool down)
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl t = threadTabController.Control;
				if (down)
					t.ScrollTo(ScrollPosition.Bottom);
				else
					t.ScrollTo(ScrollPosition.Top);
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȃX���b�h�̃��O���Ď擾
		/// </summary>
		public void ThreadReget()
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl control = threadTabController.Control;
				ThreadHeader header = control.HeaderInfo;

				cache.Remove(header);
				control.Close();
				control.Open(header);
			}
		}

		/// <summary>
		/// ���ׂẴX���b�h�����
		/// </summary>
		public void ThreadCloseAll()
		{
			if (!ClosingConfirmDialog())
				return;

			ThreadTabController thread = threadTabController;
			ThreadControl[] controls = thread.GetControls();

			foreach (ThreadControl ctrl in controls)
				ThreadCloseInternal(ctrl, false);
		}

		/// <summary>
		/// �A�N�e�B�u�łȂ��X���b�h�����
		/// </summary>
		public void ThreadCloseNotActive()
		{
			if (!ClosingConfirmDialog())
				return;

			ThreadTabController thread = threadTabController;

			if (thread.WindowCount > 0)
			{
				thread.Control.SuspendLayout();
				thread.Control.Enabled = false;

				ThreadControl active = thread.Control;
				ThreadControl[] controls = thread.GetControls();

				foreach (ThreadControl ctrl in controls)
				{
					if (active != ctrl)
						ThreadCloseInternal(ctrl, false);
				}

				thread.Control.Enabled = true;
				thread.Control.ResumeLayout();
			}
		}

		public void ThreadCloseLeft()
		{
			if (!ClosingConfirmDialog())
				return;

			ThreadControl active = threadTabController.Control;
			ThreadControl[] controls = threadTabController.GetControls();

			foreach (ThreadControl c in controls)
			{
				if (c == active)
					break;

				ThreadCloseInternal(c, false);
			}
		}

		public void ThreadCloseRight()
		{
			if (!ClosingConfirmDialog())
				return;

			ThreadTabController thread = threadTabController;
			ThreadControl[] controls = thread.GetControls();

			int activeIndex = thread.Index;

			for (int i = thread.WindowCount - 1; i > activeIndex; i--)
			{
				ThreadCloseInternal(controls[i], false);
			}
		}

		/// <summary>
		/// �w�肵����������s����
		/// </summary>
		/// <param name="operate"></param>
		public void ThreadOperator(TabOperation operate)
		{
			switch (operate)
			{
				case TabOperation.Close:
					ThreadClose(false);
					break;
				case TabOperation.Reload:
					ThreadReload();
					break;
			}
		}

		public void ThreadResetPastlogFlags()
		{
			foreach (ThreadControl ctrl in threadTabController.GetControls())
			{
				if (ctrl.HeaderInfo.Pastlog)
				{

					ThreadIndexer.SavePastlog(cache, ctrl.HeaderInfo, false);
					ctrl.HeaderInfo.Pastlog = false;

					TabPage tab = (TabPage)ctrl.Tag;
					tab.ImageIndex = TabImageIndex.Complete;
				}
			}
		}

		/// <summary>
		/// �X���b�h�𓊍e
		/// </summary>
		public void PostThread(BoardInfo board)
		{
			PostThread(board, new PostThread(String.Empty, String.Empty));
		}

		/// <summary>
		/// �X���b�h�𓊍e
		/// </summary>
		public void PostThread(BoardInfo board, PostThread thread)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			// �ꗗ�ɑ��݂��邩�ǂ����𔻒f
			if (allTable.Contains(board))
			{
				PostDialog dlg = new PostDialog(cache, koteman, board, thread);
				ShowPostDialog(dlg);
			}
			else
			{
				MessageBox.Show("���݂̔͖��Ή��ł�");
			}
		}

		/// <summary>
		/// �t���[�g�������݃_�C�A���O��\��
		/// </summary>
		public void ThreadPostFloatDialog()
		{
			ThreadPostRes(String.Empty, true);
		}

		/// <summary>
		/// �A�N�e�B�u�̃X���b�h�Ƀ��X�𓊍e
		/// </summary>
		public void ThreadPostRes()
		{
			ThreadPostRes(String.Empty, !dockWriteBar.Visible);
		}
		/// <summary>
		/// �A�N�e�B�u�̃X���b�h�Ƀ��X�𓊍e
		/// </summary>
		public void ThreadPostRes(string text)
		{
			ThreadPostRes(text, !dockWriteBar.Visible);
		}

		/// <summary>
		/// �A�N�e�B�u�̃X���b�h�Ƀ��X�𓊍e
		/// </summary>
		public void ThreadPostRes(string text, bool floatmode)
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl ctrl = threadTabController.Control;
				PostSettings ps = settings.Post;

				if (floatmode)
				{
					// ���Ƀ_�C�A���O���\������Ă���΍ė��p����
					bool recycle = (writeDialog != null);

					PostDialog dlg = recycle ? writeDialog :
						new PostDialog(cache, koteman, ctrl.HeaderInfo);

					// �_�C�A���O���ė��p����ꍇ��sage��Ԃ�ύX���Ȃ�
					if (!recycle)
						dlg.Sage = ps.Sage;

					dlg.AppendText(text);
					//dlg.ImeOn = ps.ImeOn;
					ShowPostDialog(dlg);
				}
				else
				{
					dockWriteBar.AppendText(text);
					//dockWriteBar.Sage = ps.Sage;	// �h�b�L���O�o�[��sage��Ԃ͋N�����Ɉ�񂾂���������悤�ɕύX
					//dockWriteBar.Focus();
				}
			}
		}

		/// <summary>
		/// �w�肵�����e��ҏW���邽�߂̃��X���e��� (�t���[�g��������) ��\��
		/// </summary>
		public void ThreadPostRes(Draft draft)
		{
			if (draft == null)
			{
				throw new ArgumentNullException("draft");
			}

			PostDialog dlg = (writeDialog != null) ? writeDialog :
				new PostDialog(cache, koteman, draft.HeaderInfo, draft.PostRes);

			ShowPostDialog(dlg);
		}

		/// <summary>
		/// ���X���e�_�C�A���O��\����������֐��B
		/// �C�x���g�̂ƃE�C���h�E�ʒu�̐ݒ���s���B
		/// </summary>
		/// <param name="dlg"></param>
		private void ShowPostDialog(PostDialog dlg)
		{
			PostSettings ps = settings.Post;
			dlg.Closed += new EventHandler(PostDialog_Closed);
			dlg.Posted += new PostedEventHandler(PostDialog_Posted);
			if (ps.ImeOn) dlg.ImeOn = true;
			dlg.Owner = this;

			// �������݃_�C�A���O���ė��p
			if (!ps.MultiWriteDialog)
				writeDialog = dlg;

			if (ps.WindowLocation != Point.Empty &&
				ps.WindowSize != Size.Empty)
			{
				dlg.Location = ps.WindowLocation;
				dlg.Size = ps.WindowSize;
			}
			else
			{
				dlg.CenterParent();
			}
			dlg.Show();
		}


		// ���e�_�C�A���O������ꂽ��E�C���h�E�ʒu��sage��Ԃ�ۑ�
		private void PostDialog_Closed(object sender, EventArgs e)
		{
			PostDialog dlg = (PostDialog)sender;
			settings.Post.WindowLocation = dlg.Location;
			settings.Post.WindowSize = dlg.Size;
			settings.Post.Sage = dlg.Sage;
			writeDialog = null;
		}

		// ���e���������烊���[�h�ƁA�������ݗ����̕ۑ����s��
		private void PostDialog_Posted(object sender, PostedEventArgs e)
		{
			if (e.Type == PostType.Res)
			{
				ThreadHeader header = e.HeaderInfo;

				// �������ݗ�����ۑ�
				WroteRes wrote = new WroteRes(header, DateTime.Now, e.From, e.Email, e.Body);
				lock (lockObj)
					lastWroteRes = wrote;

				if (settings.Post.SavePostHistory)
				{
					// �e�L�X�g�`���̏������ݗ����ɕۑ�
					kakikomi.Append(header, wrote);
				}

				koteman.Save(Settings.KotehanFilePath);				// �R�e�n����ۑ�

				// �ŋߏ������񂾃X�������ɒǉ�
				writtenThreadHistory.Items.Remove(header);
				writtenThreadHistory.Items.Add(header);
				writtenThreadHistory.Save();

				// �X���b�h���X�V
				ThreadControl ctrl = threadTabController.FindControl(header);
				if (ctrl != null)
					ctrl.Reload();
			}
			// �X���b�h�𗧂Ă��ꍇ�A���J��
			else if (e.Type == PostType.Thread)
			{
				ListOpen(e.BoardInfo, true);
			}
		}

		private bool ClosingConfirmDialog()
		{
			if (settings.ClosingConfirm)
			{
				return MessageBox.Show("�����̃^�u�������悤�Ƃ��Ă��܂��B���Ă�낵���ł���", "�m�F",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
			}
			else
				return true;
		}

		#endregion

		#region ���C�ɓ���֘A���\�b�h

		void OnBookmarkChanged(object sender, EventArgs e)
		{
			SaveBookmarks();
		}

		void SaveBookmarks()
		{
			bookmarkRoot.Save(Settings.BookmarkPath);			// ���C�ɓ����ۑ�
			warehouseRoot.Save(Settings.WarehousePath);			// �ߋ����O�q�ɂ�ۑ�
		}

		/// <summary>
		/// ���C�ɓ���^�u���쐬
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		private ThreadListControl CreateBookmarkWindow(List<ThreadHeader> items)
		{
			ThreadListControl listControl = listTabController.Create(dummyBookmarkBoardInfo, true);
			listControl.SetItems(dummyBookmarkBoardInfo, items);

			return listControl;
		}

		/// <summary>
		/// �w�肵���t�H���_���̂��C�ɓ�������X�g�\��
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="includeSubChildren"></param>
		private void BookmarkOpen(BookmarkFolder folder, bool includeSubChildren)
		{
			if (folder == null)
			{
				throw new ArgumentNullException("folder");
			}

			List<ThreadHeader> items = folder.GetBookmarks(includeSubChildren);
			listBookmarkControl = CreateBookmarkWindow(items);
		}

		/// <summary>
		/// �I������Ă���t�H���_�̂��C�ɓ��������
		/// </summary>
		private void BookmarkPatrol(bool checkOnly, bool includeSubChildren)
		{
			// ����ΏۃX���b�h���擾
			BookmarkFolder folder = bookmarkView.SelectedFolder;
			if (folder == null)
				folder = bookmarkRoot;

			BookmarkPatrol(folder, checkOnly, includeSubChildren);
		}

		/// <summary>
		/// �w�肵���t�H���_������
		/// </summary>
		/// <param name="folder">���񂷂�t�H���_</param>
		/// <param name="checkOnly">�X�V�`�F�b�N����ꍇ��true�A���񂷂�ꍇ��false</param>
		/// <param name="includeSubChildren">�T�u�t�H���_���܂߂�ꍇ��true</param>
		private void BookmarkPatrol(BookmarkFolder folder, bool checkOnly, bool includeSubChildren)
		{
			BookmarkPatrol(folder.GetBookmarks(includeSubChildren), checkOnly);
		}

		/// <summary>
		/// �w�肵���t�H���_������
		/// </summary>
		/// <param name="items">���񂷂�X���b�h���X�g</param>
		/// <param name="checkOnly">�X�V�`�F�b�N����ꍇ��true�A���񂷂�ꍇ��false</param>
		private void BookmarkPatrol(List<ThreadHeader> items, bool checkOnly)
		{
			if (patroller == null)
			{
				if (checkOnly)
					patroller = new CheckOnlyPatroller(cache);
				else
					patroller = new DefaultPatroller(cache);
				UpdateToolBar();

				// ���C�ɓ���^�u���쐬
				listBookmarkControl = CreateBookmarkWindow(new List<ThreadHeader>());
				SetStatusBarInfo(StatusBarPanelIndex.Text, "���C�ɓ�������񂵂Ă��܂�...");

				patroller.StatusTextChanged += new StatusTextEventHandler(BookmarkPatrol_StatusTextChanged);
				patroller.SetItems(items);
				patroller.Patroling += new PatrolEventHandler(BookmarkPatrol_Patroling);
				patroller.Updated += new PatrolEventHandler(BookmarkPatrol_Updated);
				patroller.BeginPatrol(new AsyncCallback(BookmarkPatrol_Patroling), null);
			}
		}

		/// <summary>
		/// ���C�ɓ���ɓo�^�܂��͉����B
		/// </summary>
		/// <param name="header"></param>
		private void BookmarkSet(ThreadHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			if (IsBookmarked(header))
			{
				bookmarkView.RemoveBookmark(header);
				warehouseView.RemoveBookmark(header);
			}
			else
			{
				AddBookmarkDialog dlg = new AddBookmarkDialog(bookmarkView, warehouseView, header, settings);

				dlg.ShowDialog(this);
			}


			UpdateToolBar();
		}

		private void BookmarkPatrol_StatusTextChanged(object sender, StatusTextEventArgs e)
		{
			SetStatusBarInfo(StatusBarPanelIndex.Text, e.Text);
		}

		internal void BookmarkPatrol_Patroling(object sender, PatrolEventArgs e)
		{
			Invoke(new PatrolEventHandler(BookmarkPatrol_PatrolingInternal),
				new object[] { sender, e });
		}

		private void BookmarkPatrol_PatrolingInternal(object sender, PatrolEventArgs e)
		{
			ThreadControl ctrl = threadTabController.FindControl(e.HeaderInfo);
			if (ctrl != null)
			{
				ctrl.Reload();
				e.Cancel = true;
			}
		}

		internal void BookmarkPatrol_Updated(object sender, PatrolEventArgs e)
		{
			Invoke(new PatrolEventHandler(BookmarkPatrol_UpdatedInternal),
				new object[] { sender, e });
		}

		private void BookmarkPatrol_UpdatedInternal(object sender, PatrolEventArgs e)
		{
			if (settings.Patrol_HiddenPastlog && e.HeaderInfo.Pastlog)
				return;

			List<ThreadHeader> items = new List<ThreadHeader>();
			items.Add(e.HeaderInfo);

			if (listBookmarkControl != null && listBookmarkControl.IsOpen)
				listBookmarkControl.AddItems(items);
		}

		private void BookmarkPatrol_Patroling(IAsyncResult ar)
		{
			patroller.EndPatrol(ar);
			patroller = null;

			MethodInvoker m = delegate
			{
				if (File.Exists(Twinie.Sound.Patrol))
				{
					System.Media.SoundPlayer p = new System.Media.SoundPlayer(Twinie.Sound.Patrol);
					p.Play();
					p.Dispose();
				}
				SetStatusBarInfo(StatusBarPanelIndex.Text, "���C�ɓ���̏�����������܂����B");
				UpdateToolBar();
			};
			if (InvokeRequired)
				Invoke(m);
			else
				m();
		}
		#endregion

		#region �{�^��
		private void ItaBotanSet(object tag)
		{
			ItaBotanSet(tag, tag.ToString());
		}

		private void ItaBotanSet(object tag, string text)
		{
			ItaBotanSet(tag, text, -1);
		}

		/// <summary>
		/// �w�肵��tag�����{�^����ǉ�
		/// </summary>
		/// <param name="obj"></param>
		private void ItaBotanSet(object tag, string text, int index)
		{
			CSharpToolBarButton button = ItaBotanFind(tag);
			if (button == null)
			{
				// �{�^���ɑ��݂��Ȃ���ΐV�����ǉ�
				button = new CSharpToolBarButton();
				button.Text = text;
				button.Tag = tag;

				if (index >= 0 && index < cSharpToolBar.Buttons.Count)
				{
					cSharpToolBar.Buttons.Insert(index, button);
				}
				else
				{
					cSharpToolBar.Buttons.Add(button);
				}
			}
			else
			{
				// ���ɑ��݂�����폜
				cSharpToolBar.Buttons.Remove(button);
			}

			if (initializing == false)
				SaveItaBotan();
		}

		/// <summary>
		/// �{�^������button���폜
		/// </summary>
		/// <param name="obj"></param>
		private void ItaBotanRemove(CSharpToolBarButton button)
		{
			if (button != null)
			{
				cSharpToolBar.Buttons.Remove(button);
				SaveItaBotan();
			}
		}

		/// <summary>
		/// �w�肵��tag�����{�^��������
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		private CSharpToolBarButton ItaBotanFind(object tag)
		{
			foreach (CSharpToolBarButton button in cSharpToolBar.Buttons)
				if (tag.Equals(button.Tag))
					return button;

			return null;
		}
		#endregion

		#region NG���[�h�֘A
		/// <summary>
		/// NG���[�h��ҏW����_�C�A���O��\��
		/// </summary>
		public void ShowNGWordsEditor()
		{
			NGWordEditorDialog dlg = new NGWordEditorDialog(allTable);
			dlg.ShowDialog(this);

			Twinie.NGWords.Save();

		}
		#endregion

		#region �R���g���[���C�x���g
		#region WndProc
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_NEWINSTANCE:
					{
						int atom = 0;
						try
						{
							atom = m.WParam.ToInt32();
							string[] parameters = GlobalAtom.Get(atom, 1024).Split('|');
							ParseParameter(parameters);
						}
						finally
						{
							GlobalAtom.Delete(atom);
						}
						m.Result = (IntPtr)0;
					}
					break;

				default:
					base.WndProc(ref m);
					break;
			}
		}
		#endregion

		#region Form Events
		private void Twin2IeBrowser_Load(object sender, System.EventArgs e)
		{
			Initialize();

			LoadRebarVisibleState();
			UpdateToolBar();

		}

		private void Twin2IeBrowser_Shown(object sender, System.EventArgs e)
		{
			// �O��̏�Ԃ𕜌�����ꍇ�̏���
			if (settings.OpenStartupUrls)
			{
				OpenStartup();
			}

			ParseParameter(arguments);
			UpdateCheck();

			OnLoaded(); // NTwin23.102
		}

		private void Twin2IeBrowser_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!ClosingInternal())
				e.Cancel = true;
		}

		/// <summary>
		/// �I���������s���܂��B
		/// </summary>
		/// <returns>����ɏI���ł���ꍇ�� true�A�L�����Z�����ꂽ�ꍇ�� false �ł��B</returns>
		private bool ClosingInternal()
		{
			try
			{
				// ���ׂĂ̐ڑ����~
				ClientBase.Stopper.Reset();

				if (!CloseAllWindow())
				{
					return false;
				}

				SaveSettingsAll();
				Dispose(true);		// �g�p���Ă��郊�\�[�X�����

				// �摜�r���[�A�����
				ImageViewerClose();

			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
			return true;
		}

		private void Twin2IeBrowser_LocationSizeChanged(object sender, System.EventArgs e)
		{
			Twin2IeBrowser_Resize(sender, e);
		}

		private void Twin2IeBrowser_Resize(object sender, System.EventArgs e)
		{

			if (this.WindowState != FormWindowState.Maximized &&
				this.WindowState != FormWindowState.Minimized)
			{
				normalWindowRect = new Rectangle(Location, ClientSize);
			}

			if (this.WindowState != FormWindowState.Minimized)
				this.prevWindowState = this.WindowState;

			// �^�X�N�g���C�ɓ����ꍇ�A�ŏ������ꂽ�Ƃ������^�X�N�o�[�ɕ\�������Ȃ�
			if (settings.IsTasktray)
			{
				this.Visible = (this.WindowState != FormWindowState.Minimized);
				this.notifyIcon1.Visible = !this.Visible;
			}
			else
			{
				this.notifyIcon1.Visible = false;
			}

			AdjustToolBar();

			if (progress != null)
			{
				// �v���O���X�o�[���ĕ`��
				progress.Refresh();
			}
			if (rebarWrapper != null && this.WindowState != FormWindowState.Minimized)
			{
				// �ŏ������A�T�C�Y�����ɖ߂����Ƃ��ɕ\�������������Ȃ�̂ŁA
				// Rebar��Resize�C�x���g�𔭐�������
				rebarWrapper.UpdateSize();
			}
		}
		#endregion

		#region StatusBar Events
		private void statusBar_DrawItem(object sender, System.Windows.Forms.StatusBarDrawItemEventArgs sbdevent)
		{
			// �v���O���X�o�[
			if (sbdevent.Index == statusBar.Panels.IndexOf(statusProgress))
			{
				Rectangle rect = sbdevent.Bounds;
				rect.X -= 1;
				rect.Y -= 1;
				rect.Width += 2;
				rect.Height += 2;

				progress.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
			}
			// �T�C�Y��512KB��������Ԃ��\��
			else if (sbdevent.Index == statusBar.Panels.IndexOf(statusSize))
			{
				Brush back = SystemBrushes.Control, fore = SystemBrushes.ControlText;

				Match m = Regex.Match(sbdevent.Panel.Text, @"(\d+)KB");
				if (m.Success)
				{
					int result;
					if (Int32.TryParse(m.Groups[1].Value, out result))
					{
						if (result >= 500)
						{
							back = Brushes.Red;
							fore = Brushes.White;
						}
					}
				}

				StringFormat format = new StringFormat();
				format.Alignment = StringAlignment.Center;

				sbdevent.Graphics.FillRectangle(back, sbdevent.Bounds);

				sbdevent.Graphics.DrawString(sbdevent.Panel.Text, sbdevent.Font,
					fore, sbdevent.Bounds, format);

				format.Dispose();

			}
		}

		private void statusBar_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
		{
			if (e.StatusBarPanel == this.statusSamba24)
			{
				UpdateSamba24(threadTabController.HeaderInfo.BoardInfo);
			}
		}
		#endregion

		#region AddressBar Events
		private void textBoxAddress_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				e.Handled = true;

				if (!textBoxAddress.Text.StartsWith("http://"))
				{
					// ���Ɉ�v����΂��̔��J��
					BoardInfo[] boards = TableInterface.Find(textBoxAddress.Text);
					if (boards.Length > 0)
					{
						ListOpen(boards[0], true);
						return;
					}
				}
				OpenAddress(textBoxAddress.Text);
			}
		}
		#endregion

		#region TabControl Events
		private void TabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			try
			{
				if (sender.Equals(threadTabCtrl))
				{
					TabUtility.DrawItem<ThreadHeader, ThreadControl>((TabControl)sender, e);
				}
				else if (sender.Equals(listTabCtrl))
				{
					TabUtility.DrawItem<BoardInfo, ThreadListControl>((TabControl)sender, e);
				}
			}
			catch (NullReferenceException ex)
			{
				TwinDll.Output(ex);
			}
		}

		void TabControl_DoubleClick(object sender, MouseEventArgs e)
		{
			OperationSettings op = settings.Operate;

			if (e.Clicks == 2 &&
				e.Button == MouseButtons.Left)
			{
				if (sender == listTabCtrl)
					ListOperator(op.TabDoubleClick);

				else if (sender == threadTabCtrl)
					ThreadOperator(op.TabDoubleClick);
			}
		}

		private void TabControl_MouseDown(object sender, MouseEventArgs e)
		{
			OperationSettings op = settings.Operate;

			if (e.Clicks == 2)
			{
				MessageBox.Show("DoubleClick");
			}
			else if (e.Clicks == 1)
			{
				if (e.Button == MouseButtons.Middle)
				{
					if (sender == listTabCtrl)
						ListOperator(op.TabWheelClick);

					else if (sender == threadTabCtrl)
						ThreadOperator(op.TabWheelClick);
				}
				else if (e.Button == MouseButtons.Left)
				{
					if (sender == threadTabCtrl && threadTabController.IsSelected)
					{
						// �������I������Ă�����|�b�v�A�b�v
						string sel = HtmlTextUtility.RemoveSpace(threadTabController.Control.SelectedText);

						if (StringUtility.IsPopupString(sel))
						{
							int[] numbers = ResReference.GetArray(sel);

							if (numbers.Length > 0)
								threadTabController.Control.Popup(numbers);
						}

						threadTabController.Window.Referenced = false;
					}
				}
			}
		}

		private void listTabCtrl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listTabController.IsSelected)
			{
				// �J����Ă���T�[�o�[�����擾
				ThreadListControl control = listTabController.Control;

				if (control.BoardInfo != null)
					SetBoardInfo(control.BoardInfo);
			}
			UpdateToolBar();
		}

		private void threadTabCtrl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl client = threadTabController.Control;
				ThreadHeader header = client.HeaderInfo;

				if (header != null)
				{
					TabPage tab = (TabPage)client.Tag;
					if (tab.ImageIndex == TabImageIndex.Complete)
						tab.ImageIndex = TabImageIndex.None;


					SetHeaderInfo(header);

					// �h�b�L���O�������݃o�[�ɏ���ݒ�
					dockWriteBar.Select(header);

				}



				/*
				 * 
				 * 
				 * */
				threadTabController.Window.Visibled = true;
				threadTabController.Window.Referenced = false;

				if (settings.ThreadAutoFocus)
				{
					threadTabController.Control._Select();
				}

				IEComThreadBrowser brows = client as IEComThreadBrowser;
				if (brows != null && brows.AutoReload)
				{
					autoReloadTimerCounter.Enabled = true;

					// �X�V�`�F�b�N�ς݂ŁA�I�[�g�����[�h���ꎞ��~���Ă���ꍇ��
					// �Ԋu�����Z�b�g���čĊJ����
					if (settings.Thread.AutoReloadCheckOnly && !settings.Thread.UseAutoReloadAverage &&
						brows.TimerObject != null && !brows.TimerObject.Enabled)
					{
						brows.TimerObject.ResetStart();
					}
				}


			}
			else
			{
				if (threadTabController.WindowCount == 0)
					dockWriteBar.Clear();

				CheckAutoReloadServerDic();
			}
			UpdateToolBar();
		}
		#endregion

		#region ToolBar Events
		private void toolBarMain_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonSettings)
			{
				ShowOption();
			}
			else if (e.Button == toolBarButtonOnline)
			{
				IsOnline = !IsOnline;
			}
			else if (e.Button == toolBarButtonSearchCache)
			{
				CacheSearch();
			}
			else if (e.Button == toolBarButtonSearchBoard)
			{
				BoardSearch();
			}
			else if (e.Button == toolBarButtonPatrol)
			{
				BookmarkPatrol(bookmarkRoot, true, true);
			}
			//			else if (e.Button == toolBarButtonCache)
			//			{
			//				SetWebCache(!settings.WebCache);
			//			}
			//			else if (e.Button == toolBarButtonCaching)
			//			{
			//				settings.Caching = !settings.Caching;
			//				UpdateToolBar();
			//			}
			else if (e.Button == toolBarButtonLive)
			{
				Livemode();
			}
			else if (e.Button == toolBarButtonNGWords)
			{
				NGWordsSwitch(!settings.NGWordsOn);
			}
			else if (e.Button == toolBarButtonViewTable)
			{
				ViewHideTable(!settings.View.HideTable);
			}
			else if (e.Button == toolBarButtonViewList)
			{
				ViewFillList(!settings.View.FillList);
			}
			else if (e.Button == toolBarButtonViewThread)
			{
				ViewFillThread(!settings.View.FillThread);
			}
		}

		private void toolBarList_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonListClose)
			{
				ListClose();
			}
			else if (e.Button == toolBarButtonListReload)
			{
				ListReload();
			}
			else if (e.Button == toolBarButtonListStop)
			{
				ListStop();
			}
			else if (e.Button == toolBarButtonListSearch)
			{
				ListSearch();
			}
			else if (e.Button == toolBarButtonListNewThread)
			{
				if (listTabController.IsSelected)
					PostThread(listTabController.HeaderInfo);
			}
			else if (e.Button == toolBarButtonListOpenUp)
			{
				ListOpenUpThreads();
			}
		}

		private void toolBarIButton_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{

		}

		private void toolBarRun_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonRunTool)
			{
				if ((Control.ModifierKeys & Keys.Control) > 0)
				{
					RegistSearchBotan(comboBoxTools.Text);
				}
				else if (lastSelectedToolItem != null)
					RunTool(lastSelectedToolItem, comboBoxTools.Text);
			}
		}


		private void toolBarGo_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonGo)
				OpenAddress(textBoxAddress.Text);
		}

		private void toolBarThread_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonReload)
			{
				ThreadReload();
			}
			else if (e.Button == toolBarButtonStop)
			{
				ThreadStop();
			}
			else if (e.Button == toolBarButtonClose)
			{
				ThreadClose(false);
			}
			else if (e.Button == toolBarButtonDelete)
			{
				ThreadClose(true);
			}
			else if (e.Button == toolBarButtonAutoReload)
			{
				ThreadSetAutoReload(
					!threadTabController.Control.AutoReload);
			}
			else if (e.Button == toolBarButtonWriteRes)
			{
				ThreadPostFloatDialog();
			}
			else if (e.Button == toolBarButtonFind)
			{
				ThreadFind();
			}
			else if (e.Button == toolBarButtonResExtract)
			{
				ThreadExtract();
			}
			else if (e.Button == toolBarButtonBookmark)
			{
				BookmarkSet(threadTabController.HeaderInfo);
			}
			else if (e.Button == toolBarButtonViewChange)
			{
				contextMenuViewChange.Show(this,
					PointToClient(MousePosition));
			}
			else if (e.Button == toolBarButtonScroll)
			{
				ThreadScroll(true);
			}
			else if (e.Button == toolBarButtonScrollTop)
			{
				ThreadScroll(false);
			}
		}

		private void toolBarThread_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{

			Point pt = new Point(e.X, e.Y);

			if (toolBarButtonViewChange.Rectangle.Contains(pt))
			{
				if (e.Button == MouseButtons.Middle)
				{
					if (threadTabController.IsSelected)
						threadTabController.Control.Bookmark(1001);
				}
			}
			else if (toolBarButtonScroll.Rectangle.Contains(pt))
			{
				if (e.Button == MouseButtons.Right)
				{
					contextMenuScroll.Show(this,
						PointToClient(MousePosition));
				}
				else if (e.Button == MouseButtons.Middle)
				{
					ThreadSetAutoScroll(
						!threadTabController.Control.AutoScroll);
				}
			}
			else if (toolBarButtonReload.Rectangle.Contains(pt))
			{
				if (e.Button == MouseButtons.Right)
				{
					contextMenuRead.Show(this,
						PointToClient(MousePosition));
				}
				else if (e.Button == MouseButtons.Middle)
				{
					ThreadReloadAll();
				}
			}
			else if (toolBarButtonAutoReload.Rectangle.Contains(pt))
			{
				if (e.Button == MouseButtons.Right)
				{
					contextMenuStripAutoReload.Show(this,
						PointToClient(MousePosition));
				}
			}
			else if (toolBarButtonClose.Rectangle.Contains(pt))
			{

			}
		}
		#endregion

		#region CSharpToolBar Events
		private void ItaToolBar_ButtonClick(object sender, CSharpToolBarButtonEventArgs e)
		{
			if (e.Button.Tag is BoardInfo)
			{
				BoardInfo board = (BoardInfo)e.Button.Tag;
				if (settings.EnsureVisibleBoard)
					TableInterface.Selected = board;
				ListOpen(board, KeyPushed(Keys.Shift) ? true : settings.ListAlwaysNewTab);
			}
			else if (e.Button.Tag is BookmarkEntry)
			{
				if (e.Button.Tag is BookmarkThread)
					ThreadOpen(((BookmarkThread)e.Button.Tag).HeaderInfo, true);

				else if (e.Button.Tag is BookmarkFolder)
					BookmarkOpen((BookmarkFolder)e.Button.Tag, false);
			}
			// v2.5.100
			else if (e.Button.Tag is SearchBotan)
			{
				SearchBotan btn = (SearchBotan)e.Button.Tag;
				InternalTool.FindSubject(this, btn.SearchString);
			}
			else
			{
				ItaBotanPopup popup = null;

				if (e.Button.Tag is Category)
					popup = new ItaBotanPopup((Category)e.Button.Tag);

				else if (e.Button.Tag is IBoardTable)
					popup = new ItaBotanPopup((IBoardTable)e.Button.Tag);

				if (popup != null)
				{
					// �{�^���̍����Ƀ��j���[��\������
					Point location = new Point(
						e.Button.Bounds.Left,
						e.Button.Bounds.Bottom);

					popup.Click += new BoardTableEventHandler(ItaBotan_Click);
					popup.Show(cSharpToolBar, location);
				}
			}

		}

		private void ItaBotan_Click(object sender, BoardTableEventArgs e)
		{
			TableInterface.Selected = e.Item;
			ListOpen(e.Item, e.IsNewOpen ? true : settings.ListAlwaysNewTab);
		}
		#endregion

		#region TreeView Events
		private void boardTableView_Selected(object sender, BoardTableEventArgs e)
		{
			ListOpen(e.Item,
				e.IsNewOpen ? true : settings.ListAlwaysNewTab);
		}

		private void bookmarkView_Selected(object sender, ThreadHeaderEventArgs e)
		{
			bool newTab = KeyPushed(Keys.Shift);

			foreach (ThreadHeader h in e.Items)
				ThreadOpen(h, newTab ? true : settings.ThreadAlwaysNewTab);
		}
		#endregion

		#region Label Events
		private void labelBoardName_Click(object sender, System.EventArgs e)
		{
			if (threadTabController.IsSelected)
				ListOpen(threadTabController.HeaderInfo.BoardInfo,
					KeyPushed(Keys.Shift) ? true : settings.ListAlwaysNewTab);
		}

		private void labelThreadSubject_Click(object sender, EventArgs e)
		{
			if (threadTabController.IsSelected)
			{
				ShowRelatedKeywordContextMenu(threadTabController.HeaderInfo);
			}
		}

		#endregion

		#endregion

		#region �N���C�A���g �C�x���g�n���h��
		private void ClientBase_StatusTextChanged(object sender, StatusTextEventArgs e)
		{
			// 404�G���[�����������ꍇ�A���ړ]���Ă���\��������̂ŁA
			// �ꗗ�̍X�V�����߂�
			string text = e.Text;

			if (e.Text.IndexOf("404") >= 0)
				text += " (���ړ]������������Ȃ��̂Ŕꗗ���X�V���Ă݂�)";

			SetStatusBarInfo(StatusBarPanelIndex.Text, text);
		}

		private void ClientBase_Loading(object sender, EventArgs e)
		{
			try
			{
				ClientBase client = (ClientBase)sender;
				TabPage tab = (TabPage)client.Tag;

				tab.ImageIndex = TabImageIndex.Loading;

				UpdateToolBar();

				if (ClientIsActive(client))
					progress.Reset();

				if (sender is ThreadControl)
				{
					OnThreadLoading((ThreadControl)sender);
				}
				else if (sender is ThreadListControl)
				{
					OnListLoading((ThreadListControl)sender);
				}
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex.ToString());
			}

		}

		private void ClientBase_Receive(object sender, ReceiveEventArgs e)
		{
			if (ClientIsActive((ClientBase)sender))
			{
				if (e.Length > 0)
				{
					progress.Maximum = e.Length;
					progress.Position = e.Position;
				}
			}
		}


		private void ClientBase_Complete(object sender, CompleteEventArgs e)
		{
			ClientBase client = (ClientBase)sender;
			TabPage tab = (TabPage)client.Tag;


			if (sender is ThreadControl)
			{
				ThreadControl c = (ThreadControl)sender;
				if (c.IsDisposed)
					return;
				SetThreadImageIndex(tab, c, e.Status);
			}
			else
			{
				tab.ImageIndex = (e.Status == CompleteStatus.Success) ?
					TabImageIndex.Complete : TabImageIndex.Error;
			}

			if (ClientIsActive(client))
				progress.Reset();

			if (sender is ThreadControl)
			{
				OnThreadComplete((ThreadControl)sender, e);
			}
			else if (sender is ThreadListControl)
			{
				OnListComplete((ThreadListControl)sender, e);
			}

			// �G���[�T�E���h��炷
			if (e.Status == CompleteStatus.Error)
			{
				if (File.Exists(Twinie.Sound.Error))
				{
					System.Media.SoundPlayer p = new System.Media.SoundPlayer(Twinie.Sound.Error);
					p.Play();
					p.Dispose();
				}
			}

			UpdateToolBar();
		}

		/// <summary>
		/// �X���b�h�̏�Ԃ�\���C���[�W�̃C���f�b�N�X��ݒ�
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="ctrl"></param>
		/// <param name="cs"></param>
		private void SetThreadImageIndex(TabPage tab, ThreadControl ctrl, CompleteStatus status)
		{
			if (status == CompleteStatus.Success)
			{
				// ���̃X���b�h�̃E�C���h�E�����擾
				//TwinWindow<THeader, TControl> win = (TwinWindow<THeader, TControl>)tab.Tag;
				TwinThreadWindow win = threadTabController.FindWindow(ctrl.HeaderInfo);

				// ��Ԃ��ƂɃA�C�R����ύX
				if (ctrl.HeaderInfo.Pastlog)
				{
					tab.ImageIndex = TabImageIndex.Pastlog;
				}
				else if (ctrl.HeaderInfo.NewResCount == 0 && win.Visibled)
				{
					tab.ImageIndex = TabImageIndex.None;
				}
				else if (ctrl.HeaderInfo.IsLimitOverThread)
				{
					tab.ImageIndex = TabImageIndex.Over1000Res;
				}
				else
				{
					tab.ImageIndex = TabImageIndex.Complete;
					win.Visibled = false;
				}

				if (ClientIsActive(ctrl))
					win.Visibled = true;
			}
			else
			{
				tab.ImageIndex = TabImageIndex.Error;
			}
		}

		// �X���b�h�ꗗ�̓ǂݍ��݊J�n���ɌĂ΂��
		private void OnListLoading(ThreadListControl control)
		{
			if (ClientIsActive(control))
				SetBoardInfo(control.BoardInfo);
		}

		/// <summary>
		/// �X���b�h�ꗗ�̊������ɌĂ΂��
		/// </summary>
		/// <param name="control"></param>
		private void OnListComplete(ThreadListControl control, CompleteEventArgs e)
		{
		}

		// �X���b�h�̓ǂݍ��݊J�n���ɌĂ΂��
		private void OnThreadLoading(ThreadControl control)
		{
			if (ClientIsActive(control))
			{
				// �X���b�h�̃T�C�Y���X�V
				ThreadHeader header = control.HeaderInfo;
				SetHeaderInfo(header);
				dockWriteBar.Select(header);
			}
		}

		// �X���b�h�̊������ɌĂ΂��
		private void OnThreadComplete(ThreadControl control, CompleteEventArgs e)
		{
			// �X���b�h�̃T�C�Y���X�V
			ThreadHeader header = control.HeaderInfo;

			// �X���b�h�ꗗ�̏����X�V
			UpdateThreadInfo(header);

			// �^�u�̏��A�^�u�F���X�V
			threadTabController.SetText(control, 
				StringUtility.Unescape(StringUtility.RemoveHeadSpace(header.Subject)));

			TabPage tab = (TabPage)control.Tag;
			TwinThreadWindow win = (TwinThreadWindow)tab.Tag;

			if (tabColorTable.ContainKey(header.Subject))
			{
				win.ColorSet = tabColorTable.FromKey(header.Subject);
			}
			else if (tabColorTable.ContainKey(header.BoardInfo.Name + "*"))
			{
				win.ColorSet = tabColorTable.FromKey(header.BoardInfo.Name + "*");
			}
			else
			{
				win.ColorSet = TabColorSet.Default;
			}

			if (ClientIsActive(control))
			{
				SetHeaderInfo(header);

				if (e.Status == CompleteStatus.Success)
				{
					// ���X���`�F�b�J�[
					if (settings.NextThreadChecker &&
						control.HeaderInfo.IsLimitOverThread)
					{
						BeginNextThreadCheck(control.HeaderInfo);
					}
				}
			}
			else
			{
			}

			// ��A�N�e�B�u�X���b�h�ŁA�X�V�`�F�b�N�݂̂̃I�[�g�����[�h�̏ꍇ�A
			// �V�������������_�Ń^�C�}�[���ꎞ��~�B

			// �^�u���A�N�e�B�u�ɂȂ����Ƃ��Ƀ^�C�}�[���ĊJ����

			if (settings.Thread.AutoReloadCheckOnly && !settings.Thread.UseAutoReloadAverage && control.AutoReload)
			{
				IEComThreadBrowser brows = control as IEComThreadBrowser;
				if (brows != null && brows.TimerObject != null)
				{
					if (ClientIsActive(control) || brows.HeaderInfo.NewResCount == 0)
					{
						brows.TimerObject.ResetStart();
					}
					else
					{
						brows.TimerObject.Enabled = false;
					}
				}
			}

			// �V��������Ή���炷
			if (header.NewResCount > 0 && File.Exists(Twinie.Sound.NewRes))
			{
				System.Media.SoundPlayer p = new System.Media.SoundPlayer(Twinie.Sound.NewRes);
				p.Play();
				p.Dispose();
			}
		}
		#endregion

		#region �X���b�h �C�x���g�n���h��
		private void threadView_NumberClick(object sender, NumberClickEventArgs e)
		{
			numberClickEventArgs = e;

			contextMenuRes.Show(this,
				PointToClient(MousePosition));
		}

		private void threadView_UriClick(object sender, UriClickEventArgs e)
		{
			if (settings.RecycleOverThread)
			{
				// ���̃X���b�h��OverThread��e.Uri��2ch�̃X���b�h�Ȃ�A�����^�u�ŊJ��
				// 2004/04/04 beta11
				ThreadControl ctrl = (ThreadControl)sender;
				ThreadHeader header = URLParser.ParseThread(e.Uri);

				if (header != null && ctrl.HeaderInfo.IsLimitOverThread)
				{
					ctrl.Open(header);
					return;
				}
			}

			OpenLink(e.Uri, e.Information);
		}

		#endregion

		#region �X���b�h�ꗗ �C�x���g�n���h��
		private void threadListView_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle &&
				ModifierKeys == Keys.None)
			{
				ListOperator(settings.Operate.ListWheelClick);
			}
		}

		private void threadListView_Selected(object sender, ThreadListEventArgs e)
		{
			try
			{
				foreach (ThreadHeader h in e.Items)
					ThreadBeforeOpen(h, settings.ThreadAlwaysNewTab);
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}
		#endregion

		#region ���X���ē� �C�x���g�n���h��
		private void NextThreadChecker_Success(object sender, ThreadHeaderEventArgs e)
		{
			Invoke(new ThreadHeaderEventHandler(ntcSuccess),
				new object[] { sender, e });
		}

		private bool autoNextThreadOpen = false;
		private void ntcSuccess(object sender, ThreadHeaderEventArgs e)
		{
			if (e.Items.Count > 0)
			{
				ThreadListControl list = listTabController.Create(dummyNextThreadBoardInfo, true);
				list.SetItems(dummyNextThreadBoardInfo, e.Items);
			}
			SetStatusBarInfo(0, "���X���Ǝv����X���b�h��: " + e.Items.Count);
			nextThreadChecker = null;

			if (autoNextThreadOpen)
			{
				foreach (ThreadHeader h in e.Items)
				{
					ThreadOpen(h, true);
				}
			}
			// ���X�����ŃX���ɊJ���Ă���X��������΍X�V
			else if (e.Items.Count > 0)
			{
				foreach (ThreadHeader h in e.Items)
				{
					ThreadControl c = threadTabController.FindControl(h);
					if (c != null && h.IsLimitOverThread == false)
						c.Reload();
				}
			}
		}
		#endregion

		#region �摜�r���[�A
		private void imageViewer_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = !__closing;

			if (e.Cancel)
				imageViewer.Hide();
		}
		#endregion

		#region �}�E�X�W�F�X�`��
		private MouseGestureActionSettings mgSett = new MouseGestureActionSettings(Settings.MouseGestureSettingPath);
		private void ShowMouseGestureSetting()
		{
			MouseGestureSettingDialog dlg = new MouseGestureSettingDialog(mgSett);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				mgSett.Save();
			}
		}


		void browser_MouseGestureTest(object sender, MouseGestureEventArgs e)
		{
			IEComThreadBrowser brows = sender as IEComThreadBrowser;

			foreach (MouseGestureActionItem item in mgSett.Items)
			{
				if (e.Judge(item.Arrows))
				{
					RunAction(brows, item.Action);
				}
			}
		}

		private void RunAction(IEComThreadBrowser sender, MouseGestureAction action)
		{
			switch (action)
			{
				case MouseGestureAction.Reload:
					ThreadReload();
					break;
				case MouseGestureAction.ReloadAll:
					ThreadReloadAll();
					break;
				case MouseGestureAction.ReloadSubject:
					ListReload();
					break;
				case MouseGestureAction.Close:
					ThreadClose(false);
					break;
				case MouseGestureAction.CloseAll:
					ThreadCloseAll();
					break;
				case MouseGestureAction.CloseLeft:
					ThreadCloseLeft();
					break;
				case MouseGestureAction.CloseRight:
					ThreadCloseRight();
					break;
				case MouseGestureAction.FillSubject:
					ViewFillList(!settings.View.FillList);
					break;
				case MouseGestureAction.FillThread:
					ViewFillThread(!settings.View.FillThread);
					break;
				case MouseGestureAction.ScrollTop:
					ThreadScroll(false);
					break;
				case MouseGestureAction.ScrollBottom:
					ThreadScroll(true);
					break;
				case MouseGestureAction.SelectNextTab:
					threadTabController.Select(true);
					break;
				case MouseGestureAction.SelectPrevTab:
					threadTabController.Select(false);
					break;
				case MouseGestureAction.SetBookmark:
					BookmarkSet(sender.HeaderInfo);
					break;
				case MouseGestureAction.UpdateBookmark:
					BookmarkPatrol(true, true);
					break;
				case MouseGestureAction.ShowExtractDialog:
					ThreadExtract();
					break;
				case MouseGestureAction.ShowSearchDialog:
					ThreadFind();
					break;
				case MouseGestureAction.ShowDockWriteBar:
					dockWriteBar.Visible = !dockWriteBar.Visible;
					break;
				case MouseGestureAction.ShowWriteDialog:
					ThreadPostRes(String.Empty, true);
					break;

			}
		}
		#endregion

		private void toolStripSeparator3_Click(object sender, EventArgs e)
		{

		}



		void autoReloadTimerCounter_Tick(object sender, EventArgs e)
		{
			IEComThreadBrowser brows = threadTabController.Control as IEComThreadBrowser;
			int timeleft = -1, interval = -1;

			if (brows != null && brows.AutoReload)
			{
				timeleft = brows.AutoReloadTimeleft;
				interval = brows.AutoReloadInterval;
			}
			else
				autoReloadTimerCounter.Stop();

			string text = String.Format("{0}/{1}�b",
				timeleft < 0 ? String.Empty : timeleft.ToString(),	// �c��b��
				interval / 1000);										// �X�V�Ԋu

			SetStatusBarInfo(StatusBarPanelIndex.TimerCount, text);
		}

		private void comboBoxTools_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToolItem tool = comboBoxTools.SelectedItem as ToolItem;

			if (tool != null)
				lastSelectedToolItem = tool;

		}


		private void comboBoxTools_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				e.Handled = true;

				if (lastSelectedToolItem != null)
				{
					RunTool(lastSelectedToolItem, comboBoxTools.Text);
				}
			}
		}

		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.notifyIcon1.Visible = false;
				this.Visible = !this.Visible;
				this.WindowState = this.prevWindowState;
			}
		}

		private void menuItemFileLogDeleteImageCache_Click(object sender, EventArgs e)
		{
			if (threadTabController.WindowCount > 0)
			{
				MessageBox.Show("���̑�������s����ɂ́A�S�ẴX���b�h�����K�v������܂��B");
				return;
			}

			if (MessageBox.Show("�ᕉ�חp�̉摜�T���l�C���L���b�V����S�č폜���܂��B\r\n��낵���ł����H", "�폜�̊m�F",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					Directory.Delete(Settings.ImageCacheDirectory, true);
					Directory.CreateDirectory(Settings.ImageCacheDirectory);
				}
				catch (Exception ex)
				{
					TwinDll.ShowOutput(ex);
				}
			}
		}

		private void menuItemSaveSelectedAllToDat_Click(object sender, EventArgs e)
		{
			SaveSelectedItemsAll(false);
		}

		private void menuItemSaveSelectedAllToHtml_Click(object sender, EventArgs e)
		{
			SaveSelectedItemsAll(true);
		}

		private void SaveSelectedItemsAll(bool isConvertHtml)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.Description = "�ۑ���̃t�H���_���w�肵�Ă��������B";

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				HtmlSkin htmlSkin = new HtmlSkin();
				if (isConvertHtml)
				{
					SelectSkinDialog selectSkin = new SelectSkinDialog();
					if (selectSkin.ShowDialog(this) == DialogResult.OK)
					{
						htmlSkin.Load(selectSkin.SelectedSkinFolderPath);
					}
					else
						return;
				}

				int count = 0;
				foreach (ThreadHeader header in listTabController.Control.SelectedItems)
				{
					string fileName = StringUtility.ReplaceInvalidPathChars(header.Subject, "_") + "#" + header.Key;
					string fullPath = Path.Combine(dlg.SelectedPath, fileName);
					if (isConvertHtml)
					{
						ThreadUtil.SaveHtml(cache, header, fullPath + ".html", htmlSkin);
					}
					else
					{
						ThreadUtil.SaveDat(cache, header, fullPath + ".dat");
					}
					count++;
				}
				MessageBox.Show(count + "�̃X���b�h��ۑ����܂����B");
			}
		}

		private void Twin2IeBrowser_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				BoardInfo bi = listTabController.HeaderInfo;
				if (!allTable.Contains(bi)) return;

				string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string fn in fileNames)
				{
					if (Regex.IsMatch(Path.GetFileName(fn), "^[0-9]+\\.dat$"))
					{
						e.Effect = DragDropEffects.Copy;
						break;
					}
				}
			}
		}

		private void Twin2IeBrowser_DragDrop(object sender, DragEventArgs e)
		{
			if (listTabController.IsSelected &&
				e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				BoardInfo bi = listTabController.HeaderInfo;

				string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
				int copiedCount = 0;

				foreach (string fn in fileNames)
				{
					Match m = Regex.Match(Path.GetFileName(fn), "^([0-9]+)\\.dat$");
					if (m.Success)
					{
						// �����t�@�C�������݂��Ȃ����m�F
						ThreadHeader h = TypeCreator.CreateThreadHeader(bi.Bbs);
						h.BoardInfo = bi;
						h.Key = m.Groups[1].Value;

						string destPath = this.cache.GetDatPath(h);
						if (File.Exists(destPath))
						{
							DialogResult r = MessageBox.Show("�t�@�C��: " + fn + "�͊��ɓ�����dat�����݂��܂��B�㏑�����܂����H",
								"�㏑���m�F", MessageBoxButtons.YesNoCancel);
							if (r == DialogResult.Cancel)
								return;
							else if (r == DialogResult.No)
								continue;
						}
						ThreadUtil.OpenDat(this.cache, bi, fn, Path.GetFileNameWithoutExtension(fn), false);
						copiedCount++;
					}
				}

				MessageBox.Show(copiedCount + "��dat�t�@�C����" + bi.Name + "�Ɋ֘A�t���܂����B");

				if (MessageBox.Show("�V�����ǉ����ꂽdat�𔽉f�����邽�߂ɃC���f�b�N�X���č\�z���܂����H\r\n(�ォ��ł����̑���͏o���܂�)", "�C���f�b�N�X�̍č\�z",
									MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					ListIndexing();
				}
			}
		}

		private void toolBarMain_ButtonDropDown(object sender, ToolBarButtonClickEventArgs e)
		{
			MessageBox.Show(e.Button.Text);
		}

		private void menuItemEditColoringWord_Click(object sender, EventArgs e)
		{
			ColorWordEditDialog dlg = new ColorWordEditDialog(colorWordInfoSett);
			dlg.ShowDialog(this);
			colorWordInfoSett.Save();
		}

		// BE�̎d�l�ύX�ɑΉ�����e�X�g
		private void menuItemToolBeLogin_Click(object sender, EventArgs e)
		{
			//BeLoginDialog dlg = new BeLoginDialog(this.settings.Be);
			//if (dlg.ShowDialog(this) == DialogResult.OK)
			//{
			//    this.settings.Post.Be2chCookie.Dmdm = dlg.DMDM;
			//    this.settings.Post.Be2chCookie.Mdmd = dlg.MDMD;
			//}
			BeLoginDialog2 dlg = new BeLoginDialog2(this.settings.Be);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.settings.Be.AuthenticationOn = true;
				UpdateTitleBar();
			}
		}

		private void menuItemToolBeLogout_Click(object sender, EventArgs e)
		{
			//this.settings.Post.Be2chCookie.SetEmpty();
			BeLoginManager2 b = new BeLoginManager2() { UserAgnet = TwinDll.UserAgent, CompleteMessageBox = true, };
			b.Logout(CookieManager.gCookies);

			this.settings.Be.AuthenticationOn = false;
			UpdateTitleBar();
		}

		private void UpdateTitleBar()
		{
			this.Text = Twinie.VersionText;

			if (settings.Be.AuthenticationOn)
				this.Text += " *BE";

			if (settings.Authentication.AuthenticationOn)
				this.Text += " ��";

			int wc = threadTabController.WindowCount;
			if (wc > 0) this.Text += " [" + wc + "]";

		}

		private void UpdateSamba24(BoardInfo bi)
		{
			PostDialog.samba.Update(bi);
			SetStatusBarSamba24(bi);
			Console.Beep(926, 300);
		}

		// BE


	}

	/// <summary>
	/// �A�C�R���̔ԍ���\��
	/// </summary>
	internal struct Icons
	{
		public const int FolderNormal = 0;
		public const int FolderOpen = 1;
		public const int ItemNormal = 2;
		public const int ItemSelected = 3;

		public const int Offline = 4;
		public const int Online = 5;
		public const int AutoScroll = 22;
		public const int AutoScrollOn = 23;
		public const int BookmarkOff = 27;
		public const int BookmarkOn = 28;
		public const int LivemodeOff = 44;
		public const int LivemodeOn = 45;
		public const int NGOff = 46;
		public const int NGOn = 47;
		public const int AutoReloadOff = 48;
		public const int AutoReloadOn = 10;

	}
}
