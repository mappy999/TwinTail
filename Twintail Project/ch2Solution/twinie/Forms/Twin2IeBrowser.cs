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
	/// IEコンポーネントを使用したtwintail
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

		private IBoardTable _2chTable;			// 2channel.brdのデータ
		private IBoardTable userTable;			// user.brdのデータ
		private IBoardTable allTable;			// ↑の二つを合わせた物

		private DisplayUtil display;			// 画面構成を変更するクラス
		private PatrolBase patroller;			// 巡回クラス (巡回中はインスタンス、それ以外はnull)
		private ToolItemCollection tools;		// 外部ツールコレクション
		//private WroteHistory wroteHistory;	// 書き込み履歴
		private KakikomiRireki kakikomi;		// 書き込み履歴
		private KotehanManager koteman;			// コテハン管理
		private ThreadHeaderIndices closedThreadHistory;	// 最近開いたスレッド
		private ThreadHeaderIndices writtenThreadHistory;	// 最近書き込んだスレッド
		private List<ThreadGroup> threadGroupList;	// スレッドグループリスト
		private BookmarkRoot bookmarkRoot;		// お気に入り
		private BookmarkRoot warehouseRoot;		// 過去ログ倉庫
		private TabColorTable tabColorTable;	// タブの個別配色情報

		// コントロール、ダイアログ関係
		private ThreadUpdateChecker threadUpdateChecker;
		private DockWriteBar dockWriteBar;
		private PostDialog writeDialog;
		private BoardTableView tableView;
		private BookmarkView bookmarkView;
		private BookmarkView warehouseView;
		private BookmarkMenuBuilder bookmarkMenu;		// お気に入りメニューの生成クラス
		private SmoothProgressBar progress;
		private ResExtractDialog resExtract;
		private CacheSearchDialog cacheSearcher;
		private ThreadSearchDialog threadSearcher;
		private ThreadListSearchDialog listSearcher;
		private ThreadListControl listBookmarkControl;	// お気に入り巡回時に使用する一時変数

		private ImageViewer imageViewer;
		private bool __closing = false;

		private System.Windows.Forms.Timer autoReloadTimerCounter = null;
		private TabControlNativeWindow tabnative1, tabnative2;

		private Dictionary<string, int> autoReloadServerDic = new Dictionary<string, int>();
		private Dictionary<string, string> selfID = new Dictionary<string, string>(); // 各スレッドに書き込んだ自分のIDと思われる文字列を格納 (key=URL, Value=ID)

		private FormWindowState prevWindowState = FormWindowState.Normal;

		private ColorWordInfoSettings colorWordInfoSett = null;

		private object lockObj = new object();
		private WroteRes lastWroteRes = null;	// 最後に書き込んだレスのコピー
		#endregion

		#region Dummy
		/// <summary>次スレ表示用のダミーの板情報</summary>
		internal static readonly BoardInfo dummyNextThreadBoardInfo = new BoardInfo("dummy.addr", "next", "次スレ案内");
		/// <summary>書き込み履歴表示用のダミーの板情報</summary>
		internal static readonly BoardInfo dummyWrittenBoardInfo = new BoardInfo("dummy.addr", "write", "書き込み履歴");
		/// <summary>全既得スレ表示用のダミーの板情報</summary>
		internal static readonly BoardInfo dummyAllThreadsBoardInfo = new BoardInfo("dummy.addr", "all", "全既得スレ");
		/// <summary>検索結果表示用のダミーの板情報</summary>
		internal static readonly BoardInfo dummySearchBoardInfo = new BoardInfo("dummy.addr", "search", "検索結果");
		/// <summary>お気に入り表示用のダミーの板情報</summary>
		internal static readonly BoardInfo dummyBookmarkBoardInfo = new BoardInfo("dummy.addr", "bookmark", "お気に入り");

		// 更新チェック可能な板の種類を定義
		private BoardInfo[] updateCheckableTypes = new BoardInfo[] {
			dummyAllThreadsBoardInfo, dummyBookmarkBoardInfo, dummyWrittenBoardInfo};
		#endregion

		#region Properties
		/// <summary>
		/// フォームのデフォルトサイズを取得
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(700, 550);
			}
		}

		/// <summary>
		/// すべての板一覧テーブルを取得
		/// </summary>
		public IBoardTable BBSTable
		{
			get
			{
				return allTable;
			}
		}

		/// <summary>
		/// ログのキャッシュ情報を取得
		/// </summary>
		public Cache Cache
		{
			get
			{
				return cache;
			}
		}

		/// <summary>
		/// 設定情報を取得または設定
		/// </summary>
		public Settings Settings
		{
			get
			{
				return settings;
			}
		}

		/// <summary>
		/// オフラインかどうか判断
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
		/// 板一覧テーブルのインターフェースを取得
		/// </summary>
		public ITwinTableControl TableInterface
		{
			get
			{
				return this;
			}
		}

		//		/// <summary>
		//		/// お気に入りコントロールのインターフェースを取得
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
		/// ステータスバーのパネルインデクッスを表す
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
		/// タブイメージのインデックスを表す
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
		/// フォームが表示されたときに発生
		/// </summary>
		public event EventHandler Loaded;
		#endregion

		#region InnerClass
		/// <summary>
		/// 画面構成を変更するクラス
		/// </summary>
		private class DisplayUtil
		{
			private Twin2IeBrowser form;

			/// <summary>
			/// 現在設定されている画面構成を取得
			/// </summary>
			public DisplayLayout Layout
			{
				get
				{
					return form.settings.Layout;
				}
			}

			/// <summary>
			/// ControlLayoutUtilityクラスのインスタンスを初期化
			/// </summary>
			/// <param name="ctrl"></param>
			public DisplayUtil(Twin2IeBrowser ctrl)
			{
				if (ctrl == null)
					throw new ArgumentNullException("ctrl");

				this.form = ctrl;
			}

			/// <summary>
			/// 指定した画面構成に変更
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
		/// Twin2IeBrowserクラスのインスタンスを初期化
		/// </summary>
		/// <param name="args"></param>
		public Twin2IeBrowser(Cache cache, Settings settings, string[] args)
		{
			// 引数
			this.arguments = args;
			this.settings = settings;
			this.cache = cache;

			// Windowsの終了イベントを登録
			Microsoft.Win32.SystemEvents.SessionEnding +=
				new Microsoft.Win32.SessionEndingEventHandler(OnSessionEnding);

			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			new HttpWebRequestElement().UseUnsafeHeaderParsing = true;

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
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
		/// 使用されているリソースに後処理を実行します。
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
		/// 設定を読み込む
		/// </summary>
		private void LoadSettings()
		{
			// メニューを反映
			if (File.Exists(Settings.MenuShortcutPath))
			{
				Settings.ConvertingShortcutKeyFile(settings);
				MenuSerializer2.Deserialize(Settings.MenuShortcutPath, this);
			}

			ClientBase.ConnectionLimitter = settings.ConnectionLimit;

			// 共通の受信用プロキシを設定
			WebRequest.DefaultWebProxy = settings.Net.RecvProxy;

			// フォントを作成
			settings.Design.List.CreateFonts();

			// 板一覧に新しい設定を反映
			if (settings.Design.Table.HideIcon)
				tableView.ImageList = null;

			settings.Thread.CorrectAutoReloadInterval();
			// オートリロード間隔を設定
			IEComThreadBrowser.AutoRefreshTimers.Interval =
				settings.Thread.AutoReloadInterval;

			// その他の設定情報
			IsOnline = settings.Online;
			timerGarbageCollect.Enabled = settings.GarbageCollect;

			// 外部ツールの選択アイテムを設定
			if (0 <= settings.SelectedToolsIndex && settings.SelectedToolsIndex < comboBoxTools.Items.Count)
			{
				comboBoxTools.SelectedIndex = settings.SelectedToolsIndex;
			}

			// レバーコントロールの状態を更新
			LoadRebarControlState();

			mgSett.Load();
		}

		private void LoadWindowPosition()
		{
			// ウインドウ情報が空なら画面中央に設定
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
				// スレッドのサイズを設定
				this.threadPanel.Size = settings.View.ThreadView;

			if (!settings.View.TableView.IsEmpty)
				// 板一覧テーブルのサイズを設定
				this.treePanel.Size = settings.View.TableView;

			if (!settings.View.ListView.IsEmpty)
				// スレ一覧のサイズを設定
				this.listPanel.Size = settings.View.ListView;

			this.threadToolPanel.Visible = settings.View.ThreadToolBar;
			this.statusBar.Visible = settings.View.StatusBar;
		}

		/// <summary>
		///　ウインドウが画面に見えない位置にあるならtrueを返す
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
		/// 設定を保存
		/// </summary>
		private Settings SaveSettings()
		{
			// ウインドウサイズの保存
			settings.WindowLocation = normalWindowRect.Location;
			settings.WindowSize = normalWindowRect.Size;
			settings.IsMaximized = this.WindowState == FormWindowState.Maximized;

			// 各ビューの状態を元に戻す
			//			settings.View.HideTable =
			//				settings.View.FillList =
			//				settings.View.FillThread = false;
			//			UpdateLayout();

			// スレッドビューのサイズ
			settings.View.ThreadView = threadPanel.Size;
			// 板一覧テーブルのサイズを保存
			settings.View.TableView = treePanel.Size;
			// スレ一覧のサイズを保存
			settings.View.ListView = listPanel.Size;
			// ドッキング書き込みバーのサイズを保存
			settings.View.DockWriteBarHeight = dockWriteBar.Height;

			// 選択されている外部ツールのインデックスを保存
			settings.SelectedToolsIndex = lastSelectedToolItem == null ? -1 :
				comboBoxTools.Items.IndexOf(lastSelectedToolItem);

			// レバーコントロールの状態を保存
			SaveRebarControlState();
			Twinie.SerializingSettings(settings);

			SaveItaBotan();

			return settings;
		}

		/// <summary>
		/// Rebarコントロールの状態を読み込む
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
		/// レバーコントロールの表示状態を更新
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
		/// レバーコントロールの状態を保存
		/// </summary>
		private void SaveRebarControlState()
		{
			// 最小化時は位置やサイズ情報が正常ではないので処理しない
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
		/// Loadedイベントを発生させる
		/// </summary>
		private void OnLoaded()
		{
			if (Loaded != null)
				Loaded(this, new EventArgs());
		}
		#endregion

		#region Handlers
		// Windows終了イベント
		private void OnSessionEnding(object sender, Microsoft.Win32.SessionEndingEventArgs e)
		{
			this.Close();
		}

		// 板一覧更新イベント
		private void OnBoardUpdate(object sender, BoardUpdateEventArgs e)
		{
			if (e.Event == BoardUpdateEvent.Change)
				OnServerChanged(null, new ServerChangeEventArgs(e.OldBoard, e.NewBoard));
		}

		//板が移転した時にキャッシュと板ボタンも移転
		private void OnServerChanged(object sender, ServerChangeEventArgs e)
		{
			try
			{
				SetStatusBarInfo(0, e.OldBoard.Name + "板が移転しました。処理しています...");

				// 全スレッドのリロードを一時停止
				ClientBase.Stopper.Reset();

				// 全スレッドが待機状態になるまでまつ
				//foreach (ThreadControl c in threadTabController.GetControls())
				//{
				//    while (c.IsReading && !c.IsWaiting)
				//        System.Threading.Thread.Sleep(500);
				//}

				// キャッシュを移転
				BoardLinker linker = new BoardLinker(cache);
				linker.Replace(e.OldBoard, e.NewBoard);

				// 板ボタンとお気に入りにも反映させる
				ItaBotan.ServerChange(cSharpToolBar, e.OldBoard, e.NewBoard);
				SaveItaBotan();

				BookmarkUtility.ServerChange(bookmarkRoot, e.OldBoard, e.NewBoard);
				BookmarkUtility.ServerChange(warehouseRoot, e.OldBoard, e.NewBoard);
				SaveBookmarks();

				// 板一覧も書き換える
				_2chTable.Replace(e.OldBoard, e.NewBoard);
				userTable.Replace(e.OldBoard, e.NewBoard);

				// コテハン情報も書き換える
				koteman.ServerChange(e.OldBoard, e.NewBoard);
				koteman.Save(Settings.KotehanFilePath);

				// 開いているスレッドも調べる
				OnServerChanged_CheckOpened(e.OldBoard, e.NewBoard);
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
			finally
			{
				// 全スレッドのリロードを再開
				ClientBase.Stopper.Set();
			}
		}

		// 開いているスレッドの移転処理
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

		// キャッシュ検索が閉じられたイベント
		private void CacheSearcher_Closed(object sender, EventArgs e)
		{
			cacheSearcher = null;
		}

		// スレッドが閉じられた時に検索ダイアログが開かれていれば閉じる
		private void ThreadControl_Closed(object sender, EventArgs e)
		{
			if (threadSearcher != null)
				threadSearcher.Close();
			threadSearcher = null;

			if (resExtract != null)
				resExtract.Close();
			resExtract = null;
		}

		// スレッド一覧が閉じられた時に検索ダイアログが開かれていれば閉じる
		private void ThreadListControl_Closed(object sender, EventArgs e)
		{
			if (listSearcher != null)
				listSearcher.Close();
			listSearcher = null;
		}

		// timerGarbageCollect.Tickイベント
		private void Timer_GarbageCollect(object sender, EventArgs e)
		{
			GC.Collect();
		}
		#endregion

		#region 初期化メソッド
		/// <summary>
		/// 初期化時にパラメータが渡されたら呼ばれる
		/// </summary>
		/// <param name="parameters">パラメータ配列</param>
		private void ParseParameter(string[] parameters)
		{
			foreach (string param in parameters)
			{
				if (param.StartsWith("http://"))
				{
					// アドレスを開く
					OpenAddress(param);
				}
				else if (param.StartsWith("/patrol"))
				{
					// 巡回
					bool checkOnly = param.StartsWith("/patrol:checkonly");
					BookmarkPatrol(bookmarkRoot, checkOnly, true);
				}
				else if (param == "/open")
				{
					// 前回開いたウインドウを開く
					OpenStartup();
				}
			}

		}

		/// <summary>
		/// Form.Loadで呼ばれる初期化メソッド
		/// </summary>
		private void Initialize()
		{
			// 共通の開く方法を設定
			tableView.OpenMode =
				bookmarkView.OpenMode =
				warehouseView.OpenMode = settings.Operate.OpenMode;

			// カテゴリは常に１つしか開かない設定
			tableView.AlwaysSingleOpen = settings.AlwaysSingleOpen;

			// 板一覧板ボタンを表示する場合の処理
			if (settings.View.TableItaBotan)
				SetTableItaBotan(allTable);

			// 認証に関する初期化
			if (settings.Authentication.AuthenticationOn)
				X2chAuthenticator.Enable(settings.Authentication.Username, settings.Authentication.Password);
			else
				X2chAuthenticator.Disable();

			// 画面構成を変更
			display.SetLayout(settings.Layout);

			LoadWindowPosition();

			UpdateTitleBar();
			initializing = false;
		}

		/// <summary>
		/// コンストラクタで呼ばれる初期化メソッド
		/// </summary>
		private void InitializeItems()
		{
			_2chTable = new KatjuBoardTable();
			userTable = new KatjuBoardTable();
			allTable = new KatjuBoardTable();

			// 板一覧を読み込む
			try
			{
				_2chTable.LoadTable(Settings.BoardTablePath);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("2channel.brdが存在しません。" +
					"\r\nファイルメニューから[板一覧を更新]してください");
			}

			// ユーザー定義ファイルを読み込む
			if (File.Exists(Settings.UserTablePath))
				userTable.LoadTable(Settings.UserTablePath);

			// 二つの板一覧データを結合
			allTable.Add(userTable);
			allTable.Add(_2chTable);

			// 書き込み履歴
			kakikomi = new KakikomiRireki(Settings.KakikoFolderPath);
			//wroteHistory = new WroteHistory(cache);

			// 最近閉じた履歴を読み込む
			closedThreadHistory = new ThreadHeaderIndices(cache, Settings.ClosedHistoryPath);
			closedThreadHistory.Load();

			// 最近書き込んだ履歴を読み込む
			writtenThreadHistory = new ThreadHeaderIndices(cache, Settings.WrittenHistoryPath);
			writtenThreadHistory.Load();


			// GC実行タイマーを初期化
			timerGarbageCollect = new System.Windows.Forms.Timer();
			timerGarbageCollect.Interval = 60000 * 10;
			timerGarbageCollect.Tick += new EventHandler(Timer_GarbageCollect);

			autoReloadTimerCounter = new System.Windows.Forms.Timer();
			autoReloadTimerCounter.Interval = 500;
			autoReloadTimerCounter.Tick += new EventHandler(autoReloadTimerCounter_Tick);

			// スレッドグループを読み込む
			LoadThreadGroupList();

			LoadBookmarkFiles();

			LoadItaBotan();

			// 外部ツールを読み込む
			tools = new ToolItemCollection();
			tools.Load(Settings.ToolsFilePath);
			UpdateToolsComboBox();

			// コテハンを読み込む
			koteman = new KotehanManager();
			koteman.Load(Settings.KotehanFilePath);

			// 画面構成変更クラス
			display = new DisplayUtil(this);

			// Be2chの認証クラスのインスタンスを設定
			TwinDll.Be2chCookie = settings.Post.Be2chCookie;

			// タブ配色情報の初期化
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
			// 板ボタンを復元
			cSharpToolBar.Buttons.AddRange(
				ItaBotan.Load(Settings.ItaBotanPath));
		}

		private void LoadBookmarkFiles()
		{
			Dictionary<BookmarkRoot, string> bookmarks =
				new Dictionary<BookmarkRoot, string>();

			bookmarkRoot = new BookmarkRoot("お気に入り");
			warehouseRoot = new BookmarkRoot("過去ログ");

			bookmarks.Add(bookmarkRoot, Settings.BookmarkPath);
			bookmarks.Add(warehouseRoot, Settings.WarehousePath);

			foreach (KeyValuePair<BookmarkRoot, string> item in bookmarks)
			{
				try
				{
					item.Key.Load(item.Value);
					// 正常に読み込めたらバックアップ
					Twinie.BackupUtil.Backup(item.Value);
				}
				catch (Exception)
				{
					if (Twinie.BackupUtil.Restore(item.Value))
					{
						item.Key.Load(item.Value);
						MessageBox.Show(item.Key.Name + "ファイルが壊れていたのでバックアップから復元しました。");
					}
					else
					{
						MessageBox.Show(item.Key.Name + "ファイルが壊れていて読み込めませんでした");
					}
				}
			}
		}

		/// <summary>
		/// コンストラクタで呼ばれるコントロールを初期化するメソッド
		/// </summary>
		private void InitializeControls()
		{

			// プログレスバーを初期化
			progress = new SmoothProgressBar();
			progress.ForeColor = SystemColors.ControlLightLight;
			progress.TextStyle = ProgressTextStyle.Percent;

			statusProgress.BorderStyle = StatusBarPanelBorderStyle.None;
			statusBar.Controls.Add(progress);
			statusBar.DoubleClick += new EventHandler(statusBar_DoubleClick);

			// 板一覧ツリービューを初期化
			tableView = new BoardTableView(settings.Design.Table);
			tableView.Dock = DockStyle.Fill;
			tableView.Selected += new BoardTableEventHandler(boardTableView_Selected);
			tableView.Table = allTable;
			tableView.ContextMenuStrip = contextMenuTable;
			tableView.ImageList = imageListTable;
			tabPageBoards.Controls.Add(tableView);

			// お気に入りツリービューを初期化
			bookmarkView = new BookmarkView(bookmarkRoot, settings.Design.Table);
			bookmarkView.Dock = DockStyle.Fill;
			bookmarkView.Selected += new ThreadHeaderEventHandler(bookmarkView_Selected);
			bookmarkView.ImageList = imageListTable;
			bookmarkView.FolderContextMenu = contextMenuBookmarkFolder;
			bookmarkView.BookmarkContextMenu = contextMenuBookmarkItem;
			bookmarkView.BookmarkChanged += OnBookmarkChanged;
			tabPageBookmarks.Text = bookmarkRoot.Name;
			tabPageBookmarks.Controls.Add(bookmarkView);

			// 過去ログ倉庫ツリービューを初期化
			warehouseView = new BookmarkView(warehouseRoot, settings.Design.Table);
			warehouseView.Dock = DockStyle.Fill;
			warehouseView.Selected += new ThreadHeaderEventHandler(bookmarkView_Selected);
			warehouseView.ImageList = imageListTable;
			warehouseView.FolderContextMenu = contextMenuWareHouseFolder;
			warehouseView.BookmarkContextMenu = contextMenuWareHouseItem;
			warehouseView.BookmarkChanged += OnBookmarkChanged;
			tabPageWareHouse.Text = warehouseRoot.Name;
			tabPageWareHouse.Controls.Add(warehouseView);

			// ドッキング書き込みバーを作成
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

			// タブコントロールのマウスイベントを監視
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

			// 定期更新チェッカを作成
			threadUpdateChecker = new ThreadUpdateChecker(this, cache);
			threadUpdateChecker.NoClosing = true;

			// お気に入りメニュー生成クラスを初期化
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
				// マウスホイールでタブの移動を処理するため、マウスをグローバルフック
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
			// 各コントロールのフォントをWindows標準に設定
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

			// 新規ウインドウを作成するかどうか
			if (tabCount == 0 || newWindow)
			{
				// ビューアを作成
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

				// 実況モードではオートスクロール＆オートリロードをOnにする
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

				// 一端nullを代入しないとSelectedIndexChangedイベントが発生しない
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
		/// 印されたレスが他のレスに参照された場合にこのイベントが発生する
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
					// 最後に書き込んだレスと受信したレスが一致したら印をつける
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

				// 書き込まれるレスの中から画像リンクを探し、イメージビューアで自動的に開く
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
					// サムネイルビューア
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

			// キーがなければ追加
			if (!autoReloadServerDic.ContainsKey(key))
			{
				autoReloadServerDic.Add(key, 0);
			}

			if (e.NewValue)
			{
				if (autoReloadServerDic[key] >= setAutoReloadLimit)
				{
					SetStatusBarInfo(StatusBarPanelIndex.Text, "オートリロードは同じ板で" + setAutoReloadLimit + "つまでしか設定できません。");
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
					MessageBox.Show("おかしいです。");
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

		// タブを選択したときにスレッドを自動で更新する処理
		// タブの移動や閉じるときに動作しないようにしてある
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

				// 自動でスレッドを更新
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

			// 新規ウインドウを作成するかどうか
			if (tabCount == 0 || newWindow)
			{
				// ビューアを作成
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

				// 一端nullを代入しないとSelectedIndexChangedイベントが発生しない
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

			// タブの色づけ
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
		/// タブを閉じたとき、次に選択するタブを決定する
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="index">閉じたタブのインデックス</param>
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

			// 画像ビューア
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

		#region Privateメソッド

		/// <summary>
		/// 指定したスレッドの関連キーワードをコンテキストメニューで表示します。
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
		/// 前回の終了時の状態を開くメソッド
		/// </summary>
		private void OpenStartup()
		{
			string[] array = new string[settings.StartupUrls.Count];
			settings.StartupUrls.CopyTo(array, 0);

			foreach (string url in array)
				OpenAddress(url);
		}

		/// <summary>
		/// 自動アップデータ
		/// </summary>
		private void UpdateCheck()
		{
			if (settings.UpdateCheck)
				TwinUpdate.Check();
		}

		/// <summary>
		/// 板一覧を板ボタンに追加
		/// </summary>
		/// <param name="table"></param>
		private void SetTableItaBotan(IBoardTable table)
		{
			if (table == null)
				return;

			CSharpToolBarButton button = ItaBotanFind(table);

			if (button == null)
			{
				ItaBotanSet(table, "２ちゃんねる", 0);
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
			_2chTable.SaveTable(Settings.BoardTablePath);		// 板一覧情報を保存
			userTable.SaveTable(Settings.UserTablePath);		// ユーザー定義板を保存
			SaveSettings();		// すべての設定を保存
		}

		private void SaveItaBotan()
		{
			ItaBotan.Save(Settings.ItaBotanPath, cSharpToolBar.Buttons);	// 板ボタンを保存
		}
		private void SaveTools()
		{
			tools.Save(Settings.ToolsFilePath);					// 外部ツールを保存
		}

		private void SaveSettings2()
		{
			if (initializing)
				return;

			SaveWindowsUrl();
			Twinie.SerializingSettings(settings);
		}

		/// <summary>
		/// 現在開いているURLを保存
		/// </summary>
		private void SaveWindowsUrl()
		{
			List<string> urls = new List<string>();

			try
			{
				// 開いているリストのURL
				ThreadListControl[] listItems = listTabController.GetControls();
				foreach (ThreadListControl list in listItems)
				{
					// お気に入りまたは検索結果タブは対象から外す
					if (list.BoardInfo.Server == "dummy.addr")
						continue;

					urls.Add(list.BoardInfo.Url);
				}

				// 開いているスレッドのURL
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
		/// スレッドを閉じる内部メソッド
		/// </summary>
		/// <param name="thread">閉じるウインドウ</param>
		/// <param name="delete">ログを削除するかどうか</param>
		private void ThreadCloseInternal(ThreadControl thread, bool delete)
		{
			if (thread == null)
			{
				throw new ArgumentNullException("thread");
			}
			if (thread.IsReading)
			{
				MessageBox.Show("スレッドを受信中に閉じることは出来ません", "閉じられません",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ThreadHeader header = thread.HeaderInfo;
			header.NewResCount = 0;

			// 閉じた履歴に追加
			AddClosedHistory(header);

			// ドッキング書き込みバーからレス情報を削除
			dockWriteBar.Remove(header);

			// スレッドを閉じる
			thread.Close();

			threadTabController.Destroy(thread);

			// ログを削除
			if (delete)
				cache.Remove(header);

			SaveSettings2();

			// スレッド一覧を更新
			UpdateThreadInfo(header);
			UpdateTitleBar();
		}

		/// <summary>
		/// クライアントイベントを登録
		/// </summary>
		/// <param name="client"></param>
		private void ClientAddEvents(ClientBase client)
		{
			client.Loading += new EventHandler(ClientBase_Loading);
			client.Receive += new ReceiveEventHandler(ClientBase_Receive);
			client.Complete += new CompleteEventHandler(ClientBase_Complete);
		}

		/// <summary>
		/// クライアントイベントを削除
		/// </summary>
		/// <param name="client"></param>
		private void ClientRemoveEvents(ClientBase client)
		{
			client.Loading -= new EventHandler(ClientBase_Loading);
			client.Complete -= new CompleteEventHandler(ClientBase_Complete);
			client.Receive -= new ReceiveEventHandler(ClientBase_Receive);
		}

		/// <summary>
		/// 指定したツールを実行
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
		/// すべてのスレッドのフォントサイズを更新
		/// </summary>
		private void UpdateFontSize()
		{
			foreach (ThreadControl ctrl in threadTabController.GetControls())
				ctrl.FontSize = settings.Thread.FontSize;
		}

		/// <summary>
		/// 外部ツールのコンボボックスを更新
		/// </summary>
		private void UpdateToolsComboBox()
		{
			comboBoxTools.Items.Clear();
			comboBoxTools.Items.Add(String.Empty);

			// 一つも登録されていない場合、標準でスレッドタイトル検索を追加
			if (tools.Count == 0)
			{
				tools.Add(new ToolItem("スレタイ検索", "$Find2ch", ""));
			}

			foreach (ToolItem item in tools)
				comboBoxTools.Items.Add(item);

			comboBoxTools.SelectedIndex = 0;
		}

		/// <summary>
		/// ツールバーの状態を更新
		/// </summary>
		private void UpdateToolBar()
		{
			// メインツールバー
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

			// スレッドツールバー
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


			toolBarButtonBookmark.ToolTipText = "お気に入りの登録・解除";

			if (threadTabController.IsSelected)
			{
				ThreadControl thread = threadTabController.Control;

				// お気に入りの状態
				bool contains = IsBookmarked(thread.HeaderInfo);
				toolBarButtonBookmark.Pushed = contains;
				toolBarButtonBookmark.ImageIndex = contains ? Icons.BookmarkOn : Icons.BookmarkOff;

				if (contains)
					toolBarButtonBookmark.ToolTipText += String.Format(" [{0}]", GetBookmarkFolderName(thread.HeaderInfo));

				toolBarButtonAutoReload.Pushed = thread.AutoReload;
				toolBarButtonAutoReload.ImageIndex = thread.AutoReload ? Icons.AutoReloadOn : Icons.AutoReloadOff;
				toolBarButtonScroll.ImageIndex = thread.AutoScroll ? Icons.AutoScrollOn : Icons.AutoScroll;

				// スレッドを受信中はログ削除または閉じられないようにする
				toolBarButtonClose.Enabled =
					toolBarButtonDelete.Enabled = !thread.IsReading;
			}

			if (listTabController.IsSelected)
			{
				toolBarButtonListClose.Enabled =
					!listTabController.Control.IsReading;
			}
		}

		// 画面構成を更新
		private void UpdateLayout()
		{
			ViewSettings view = settings.View;

			if (display.Layout != DisplayLayout.TateYoko2)
			{
				treePanel.Visible = !view.HideTable;
				threadPanel.Visible = !view.FillList;
				listPanel.Visible = !view.FillThread;

				// スレッド一覧を拡大するならスレッドを非表示に
				if (view.FillList)
				{
					listPanel.Dock = DockStyle.Fill;
					threadPanel.Dock = DockStyle.None;

					// 特殊分割だと被ってしまうので、非表示にする
					if (display.Layout == DisplayLayout.Extend1)
						treePanel.Visible = false;
				}
				// スレッドを拡大するならスレ一覧を非表示に
				else if (view.FillThread)
				{
					listPanel.Dock = DockStyle.None;
					threadPanel.Dock = DockStyle.Fill;
				}
				// どちらの状態でもなければ元に戻す
				else
				{
					// 通常または横分割ならDockStyle.Top、縦分割ならDockStyle.Left					
					listPanel.Dock = (display.Layout != DisplayLayout.Tate3) ? DockStyle.Top : DockStyle.Left;
					threadPanel.Dock = DockStyle.Fill;
				}
			}
		}

		/// <summary>
		/// スレッド一覧に表示されているスレッド情報を更新
		/// </summary>
		/// <param name="header"></param>
		private void UpdateThreadInfo(ThreadHeader header)
		{
			// スレッドの板と、次スレタブ、お気に入りタブを検索
			BoardInfo[] boards = new BoardInfo[] { header.BoardInfo, dummyNextThreadBoardInfo, dummyBookmarkBoardInfo };
			foreach (BoardInfo board in boards)
			{
				ThreadListControl list = listTabController.FindControl(board);
				if (list != null)
					list.UpdateItem(header);
			}
		}

		/// <summary>
		/// 指定した修飾子キーが押されている状態かどうかを調べる
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool KeyPushed(Keys key)
		{
			return (ModifierKeys & key) != 0;
		}

		/// <summary>
		/// 指定したclientがアクティブかどうかを判断
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

		// スレッドツールバーの枠を描画
		private void threadToolPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			ControlPaint.DrawBorder3D(g, 0, 0,
				threadToolPanel.Width, threadToolPanel.Height, Border3DStyle.Etched);
		}

		/// <summary>
		/// スレッドツールバーの位置とサイズを調整
		/// </summary>
		private void AdjustToolBar()
		{
			int width = threadPanel.Width;

			// ツールバーを調整
			int tbw = CommonUtility.CalcToolBarWidth(toolBarThread);
			toolBarThread.Left = width - tbw - 3;
			toolBarThread.Width = tbw;

			// スレッド名を表示するラベルを調整
			labelThreadSubject.Left = labelBoardName.Width + 3;
			labelThreadSubject.Width = (toolBarThread.Left - 3) - labelThreadSubject.Left;
		}

		/// <summary>
		/// 板情報を設定
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
		/// ヘッダ情報を設定
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
		/// 指定したboardと同じURLを持つ板を現在の板一覧の中から検索し、板名を取得
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
			return "板名不明";
		}

		/// <summary>
		/// 受信サイズをステータスバーに表示
		/// </summary>
		/// <param name="size"></param>
		private void SetStatusBarSize(int size)
		{
			SetStatusBarInfo(StatusBarPanelIndex.Size,
				String.Format("{0}KB", size / 1024));
		}

		/// <summary>
		/// スレッドの勢いをステータスバーに表示
		/// </summary>
		/// <param name="h"></param>
		private void SetStatusBarForce(ThreadHeader h)
		{
			ThreadHeaderInfo info = new ThreadHeaderInfo(h);
			SetStatusBarInfo(StatusBarPanelIndex.Force, info.GetForceValue(settings.ForceValueType));
		}

		/// <summary>
		/// スレッドのレス数をステータスバーに表示
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
			SetStatusBarInfo(StatusBarPanelIndex.SambaCount, "Samba: " + sambaValue.ToString() + "秒");
		}

		/// <summary>
		/// ステータスバーの情報を設定
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
		/// 最近閉じた履歴を追加
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
		/// 指定した板の板名を取得
		/// </summary>
		/// <param name="board">検索する板のURL情報が格納されたBoardInfoクラス</param>
		/// <returns></returns>
		private string GetBoardName(BoardInfo board)
		{
			foreach (Category cate in allTable.Items)
			{
				int index = cate.Children.IndexOf(board);
				if (index >= 0)
					return cate.Children[index].Name;
			}
			return "板名不明";
		}

		/// <summary>
		/// スクラップエディタを表示
		/// </summary>
		private void ShowScrapEditor()
		{
			ScrapEditorDialog dlg = new ScrapEditorDialog(Settings.ScrapFolderPath);
			dlg.Show();
		}

		/// <summary>
		/// 板一覧を非表示にする
		/// </summary>
		/// <param name="hide"></param>
		private void ViewHideTable(bool hide)
		{
			settings.View.HideTable = hide;
			UpdateLayout();
			UpdateToolBar();
		}

		/// <summary>
		/// スレッド一覧を拡大する
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
		/// スレッドを拡大する
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
		/// 取得先のスレが違うサーバーで、
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

		// 指定したスレッドの次スレを検索
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
				MessageBox.Show("他のスレッドの次スレを検索中です", "ちょっと待ってください",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		// 更新チェック可能な板かどうかを判断
		// boardがお気に入り、全既得スレ、書き込み履歴のどれかであればtrueを返す
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
		/// 板ボタンにスレッドタイトル検索を登録
		/// </summary>
		/// <param name="searchString">検索文字列</param>
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

		// すべてのリンクを開く
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
				string text = String.Format("{0} ({1})\r\nこのリンクを開きますか？",
					linkInfo.Uri, linkInfo.Text);

				DialogResult r = MessageBox.Show(text, "NG指定されたURL",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (r == DialogResult.No)
					return false;
			}

			OpenAddress(uri);
			return true;
		}

		// すべてのスレッドが閉じられた場合、autoReloadServerDicの値はすべて0になるはず
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

		#region Publicメソッド
		/// <summary>
		/// 指定したURLを開く
		/// </summary>
		/// <param name="url">開くスレッドのURL</param>
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

			// 画像ビューアで開いてみる
			if (settings.ImageViewer &&
				Regex.IsMatch(url, @"(\.jpg|\.jpeg|\.gif|\.bmp|\.png|\.jpg\.html)$", RegexOptions.IgnoreCase))
			{
				ImageViewerOpen(url);
				return;
			}

			CommonUtility.OpenWebBrowser(url);
		}

		/// <summary>
		/// 2ch互換のdatファイルを開く
		/// </summary>
		public void OpenDat()
		{
			OpenThreadDialog dlg = new OpenThreadDialog(cache, allTable);

			if (dlg.ShowDialog(this) == DialogResult.OK)
				ThreadOpen(dlg.HeaderInfo, true);
		}

		/// <summary>
		/// monalogファイルを開く
		/// </summary>
		public void OpenMonalog()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "monalog ファイル (*.xml)|*.xml";

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				ThreadHeader header = ThreadUtil.OpenMonalog(cache, dlg.FileName);
				ThreadOpen(header, true);
			}
		}

		private string GetThreadSaveFileName(ThreadHeader h)
		{
			// スレッド名 #key
			return StringUtility.ReplaceInvalidPathChars(h.Subject, "-") + " #" + h.Key;
		}

		/// <summary>
		/// 2chのhtmlファイルにして保存
		/// </summary>
		public void SaveHtml()
		{
			if (threadTabController.IsSelected)
			{
				ThreadHeader header = threadTabController.HeaderInfo;
				saveFileDialog.FileName = GetThreadSaveFileName(header);
				saveFileDialog.FilterIndex = 2;
				saveFileDialog.Title = header.Subject + "をhtml形式で保存";

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
		/// 2chのdatファイルにして保存
		/// </summary>
		public void SaveDat()
		{
			if (threadTabController.IsSelected)
			{
				ThreadHeader header = threadTabController.HeaderInfo;
				saveFileDialog.FileName = GetThreadSaveFileName(header);
				saveFileDialog.FilterIndex = 3;
				saveFileDialog.Title = header.Subject + "をdat形式で保存";

				if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
					ThreadUtil.SaveDat(cache, header, saveFileDialog.FileName);
			}
		}

		/// <summary>
		/// monalog形式でスレッドを保存
		/// </summary>
		public void SaveMonalog()
		{
			if (threadTabController.IsSelected)
			{
				ThreadHeader header = threadTabController.HeaderInfo;
				saveFileDialog.FileName = GetThreadSaveFileName(header);
				saveFileDialog.FilterIndex = 4;
				saveFileDialog.Title = header.Subject + "をmonalog形式で保存";

				if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
					ThreadUtil.SaveMonalog(cache, header, saveFileDialog.FileName);
			}
		}

		/// <summary>
		/// NGワードのOn/Off切り換え
		/// </summary>
		/// <param name="enable">有効にするならtrue、無効にするならfalse</param>
		public void NGWordsSwitch(bool enable)
		{
			settings.NGWordsOn = enable;
			UpdateToolBar();

		}

		/// <summary>
		/// 実況モード
		/// </summary>
		public void Livemode()
		{
			if (settings.Livemode)
			{
				// 板一覧、スレ一覧、スレッドを表示、ドッキング書き込みバーを非表示
				settings.View.HideTable = false;
				settings.View.FillList = false;
				settings.View.FillThread = false;
				dockWriteBar.Visible = settings.View.DockWriteBar;

				// オートリロードOff、オートスクロールOff
				if (threadTabController.IsSelected)
				{
					threadTabController.Control.AutoReload =
						threadTabController.Control.AutoScroll = false;
				}
			}
			else
			{
				// 板一覧、スレ一覧を非表示、スレッド、ドッキング書き込みバーを表示
				settings.View.HideTable = true;
				settings.View.FillList = false;
				settings.View.FillThread = true;
				dockWriteBar.Visible = true;

				// オートリロードOn、オートスクロールOn
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
		/// 環境設定ダイアログを表示
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
		/// すべてのウインドウを閉じる
		/// </summary>
		public bool CloseAllWindow()
		{
			// オートリロードを止める
			IEComThreadBrowser.AutoRefreshTimers.Clear();

			// スレ一覧
			ThreadListControl[] lists = listTabController.GetControls();
			foreach (ThreadListControl window in lists)
				window.Close();

			//	bool ignore = false;

			// スレッド
			ThreadControl[] threads = threadTabController.GetControls();
			foreach (ThreadControl window in threads)
			{
				/*
				while (window.IsReading && !ignore)
				{
					DialogResult r = MessageBox.Show("スレッドを読み込み中ですが、終了してもよろしいですか？",
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
		/// 指定したスレッドがお気に入りに登録されているかどうかを判断
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool IsBookmarked(ThreadHeader header)
		{
			return bookmarkRoot.Contains(header) || warehouseRoot.Contains(header);
		}

		/// <summary>
		/// 指定したスレッドが登録されているお気に入りのフォルダ名を取得します。
		/// お気に入りに存在しなければ null を返します。
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
		/// ●ログオンします
		/// </summary>
		public void OysterLogon()
		{
			Twinie.OysterLogon();
			UpdateTitleBar();
		}

		/// <summary>
		/// ●ログアウトします
		/// </summary>
		public void OysterLogout()
		{
			Twinie.OysterLogout();
			UpdateTitleBar();
		}
		#endregion

		#region スレッド一覧操作メソッド
		/// <summary>
		/// 指定した板を開きスレッド一覧を取得
		/// </summary>
		/// <param name="info">開く板の情報</param>
		/// <param name="newTab">新しいタブで開く場合はtrue、そうでなければfalseを指定</param>
		public void ListOpen(BoardInfo info, bool newTab)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			// このスレッドの板が移転していないかどうかをチェック
			//			BoardLinker linker = new BoardLinker(cache);
			//			BoardInfo moveto = linker.GetLinked(info, true);
			//
			//			if (moveto != null) // 板が移転していた場合は板情報を書き換える
			//				info = moveto;

			// ビューアを作成
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
		/// アクティブなスレッド一覧をリロードし新着レスを取得
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
		/// アクティブなスレッド一覧の読み込みを中止
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
				if (MessageBox.Show("巡回中のお気に入りタブを閉じてもよろしいですか？", "確認",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// アクティブなスレッド一覧を閉じる
		/// </summary>
		public void ListClose()
		{
			if (listTabController.IsSelected)
			{
				ThreadListControl list = listTabController.Control;

				if (list.IsReading)
				{
					MessageBox.Show("スレッド一覧を受信中に閉じることは出来ません", "閉じられません",
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
		/// スレッド一覧検索ダイアログを表示
		/// </summary>
		public void ListSearch()
		{
			if (listTabController.IsSelected)
			{
				ThreadListControl list = listTabController.Control;

				if (!CanListClose(list))
					return;

				// 既に開かれていれば一端閉じる
				if (listSearcher != null)
				{
					listSearcher.Close();
					((ThreadListControl)listSearcher.Tag).Closed -= new EventHandler(ThreadControl_Closed);
				}

				// 検索クラスを初期化
				list.Closed += new EventHandler(ThreadListControl_Closed);
				listSearcher = new ThreadListSearchDialog(list.BeginSearch(), settings.Search.ListSearch);
				listSearcher.Tag = list;
				listSearcher.Owner = this;
				listSearcher.Show();
			}
		}

		/// <summary>
		/// すべてのスレッド一覧を閉じる
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
		/// アクティブでないスレッド一覧を閉じる
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
		/// アクティブな板の更新スレを開く
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
							if (MessageBox.Show("スレッド開きすぎです。。\r\n開きすぎると動作が不安定になりますが、それでも開きますか？", "開きすぎ注意",
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
		/// すべての既得スレッドを一覧表示
		/// </summary>
		public void ListAllThreads()
		{
			List<ThreadHeader> items = Twin.IO.GotThreadListReader.GetAllThreads(cache, allTable);
			ThreadListControl list = listTabController.Create(dummyAllThreadsBoardInfo, true);
			list.SetItems(dummyAllThreadsBoardInfo, items);
		}

		/// <summary>
		/// 最近書き込んだスレッドを一覧表示
		/// </summary>
		public void ListWrittenThreads()
		{
			ThreadListControl list = listTabController.Create(dummyWrittenBoardInfo, true);

			// 最近書き込んだ日付順にソート
			IComparer comparer = new ListViewItemComparer(SortOrder.Descending, ThreadListView.ColumnNumbers.LastWritten);
			ThreadHeader[] array = new ThreadHeader[writtenThreadHistory.Items.Count];
			writtenThreadHistory.Items.CopyTo(array, 0);
			Array.Sort(array, comparer);

			List<ThreadHeader> sortedList = new List<ThreadHeader>();
			sortedList.AddRange(array);

			list.SetItems(dummyWrittenBoardInfo, sortedList);
		}

		/// <summary>
		/// 既得インデックスを再生成
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
		/// 指定した操作を実行する
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
		/// 指定した操作を実行する
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

		#region キャッシュ履歴操作メソッド
		/// <summary>
		/// キャッシュを検索
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
		/// 指定した板のキャッシュを開く
		/// </summary>
		/// <param name="board"></param>
		public void CacheOpen(BoardInfo board)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			// オフラインモードで開く
			ThreadListControl list = listTabController.Create(board, true);
			list.Stop();
			list.Online = false;
			list.Open(board);
		}

		/// <summary>
		/// 指定した板のログをすべて削除
		/// </summary>
		/// <param name="board">ログを削除する板の情報 (nullを指定するとすべてのログを削除)</param>
		public void CacheClear(BoardInfo board)
		{
			string name =
				(board != null) ? board.Name : "すべての";

			DialogResult r =
				MessageBox.Show(name + "板の既得ログをすべて削除します (書き込み履歴も削除されます)。\r\nよろしいですか？", "削除確認",
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
		/// 指定した板の草稿を一覧表示
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
		/// 指定した草稿を編集
		/// </summary>
		/// <param name="draft"></param>
		public void DraftEdit(Draft draft)
		{
			if (draft == null)
			{
				throw new ArgumentNullException("draft");
			}

			// 編集する草稿をリストから削除
			DraftBox box = new DraftBox(cache);
			box.Remove(draft.HeaderInfo.BoardInfo, draft);

			ThreadPostRes(draft);
		}

		/// <summary>
		/// 指定した板の書き込み履歴一覧を表示
		/// </summary>
		/// <param name="board">書き込み履歴を表示する板</param>
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
		//		/// 指定した板の書き込み履歴を表示
		//		/// </summary>
		//		/// <param name="board">書き込み履歴を表示するスレッド</param>
		//		public void HistoryOpen(ThreadHeader header)
		//		{
		//			if (header == null) {
		//				throw new ArgumentNullException("header");
		//			}
		//
		//			MessageBox.Show("ちょっと待って");
		//			/*
		//			ThreadControl thread = threadTabController.Create(header, true);
		//			thread.OpenHistory(header);
		//			SetHeaderInfo(header);
		//			*/
		//		}

		/// <summary>
		/// 指定した板の書き込み履歴を削除
		/// </summary>
		/// <param name="board">削除対象の板 (nullを指定するとすべての板を削除)</param>
		public void HistoryClear(BoardInfo board)
		{
			string name =
				(board != null) ? board.Name : "すべての";

			DialogResult r =
				MessageBox.Show(board.Name + "板の書き込み履歴をすべて削除します。よろしいですか？", "削除確認",
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

		#region 板操作メソッド
		/// <summary>
		/// 板検索ダイアログを表示
		/// </summary>
		public void BoardSearch()
		{
			BoardSearchDialog dlg = new BoardSearchDialog(this);
			dlg.ShowDialog(this);
		}
		#endregion

		#region スレッド操作メソッド
		/// <summary>
		/// スレッドを開く前の分別処理
		/// </summary>
		/// <param name="header"></param>
		/// <param name="newTab"></param>
		private void ThreadBeforeOpen(ThreadHeader header, bool newTab)
		{
			// レス抽出
			if (header.Tag is ThreadExtractInfo)
			{
				ThreadOpen(header, (ThreadExtractInfo)header.Tag, newTab);
			}
			// 草稿
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
		/// 指定したスレッドを開き表示
		/// </summary>
		/// <param name="header"></param>
		/// <param name="newTab"></param>
		public void ThreadOpen(ThreadHeader header, bool newTab)
		{
			ThreadOpen(header, newTab, null);
		}

		/// <summary>
		/// 指定したスレッドを開き表示
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

			// ビューアを作成
			ThreadControl ctrl = threadTabController.Create(header, newTab);
			ThreadHeader prevheader = ctrl.HeaderInfo;

			if (ctrl.IsReading)
			{
				return;
			}

			// スレッドが開かれていれば、そのスレッドを閉じた履歴に追加
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
		/// 指定したスレッドを開き表示
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

			// ビューアを作成
			ThreadControl ctrl = threadTabController.Create(header, newTab);

			// 閉じた履歴に追加
			if (ctrl.HeaderInfo != null)
				AddClosedHistory(header);

			ctrl.Stop();
			ctrl.OpenExtract(header, info);
			SetHeaderInfo(header);
		}

		/// <summary>
		/// アクティブなスレッドをリロードし新着レスを取得
		/// </summary>
		public void ThreadReload()
		{
			if (threadTabController.IsSelected)
				threadTabController.Control.Reload();
		}

		/// <summary>
		/// すべてのスレッドを更新
		/// </summary>
		public void ThreadReloadAll()
		{
			ThreadControl[] controls = threadTabController.GetControls();

			foreach (ThreadControl thread in controls)
				thread.Reload();
		}

		/// <summary>
		/// アクティブなスレッドの読み込みを中止
		/// </summary>
		public void ThreadStop()
		{
			if (threadTabController.IsSelected)
				threadTabController.Control.Stop();
		}

		/// <summary>
		/// スレッド内を検索するためのダイアログを表示
		/// </summary>
		public void ThreadFind()
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl control = threadTabController.Control;

				// 既に開かれていれば一端閉じる
				if (threadSearcher != null)
				{
					threadSearcher.Close();
					((ThreadControl)threadSearcher.Tag).Closed -= new EventHandler(ThreadControl_Closed);
				}

				// 検索クラスを初期化
				control.Closed += new EventHandler(ThreadControl_Closed);
				threadSearcher = new ThreadSearchDialog(control.BeginSearch(), settings.Search.ThreadSearch);
				threadSearcher.Tag = control;
				threadSearcher.Owner = this;
				threadSearcher.Show();
			}
		}

		/// <summary>
		/// レスを抽出するためのダイアログを表示
		/// </summary>
		public void ThreadExtract()
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl control = threadTabController.Control;
				AbstractExtractor extractor = control.BeginExtract();

				// 文字列が選択されている場合、すぐに抽出表示
				if (control.SelectedText != String.Empty)
				{
					extractor.NewWindow = true;
					extractor.InnerExtract(control.SelectedText, ResSetElement.All);
					return;
				}

				// 既に開かれていれば一端閉じる
				if (resExtract != null)
				{
					resExtract.Close();
					((ThreadControl)resExtract.Tag).Closed -= new EventHandler(ThreadControl_Closed);
				}

				// 検索クラスを初期化
				control.Closed += new EventHandler(ThreadControl_Closed);
				resExtract = new ResExtractDialog(extractor, settings.Search.ResExtract);
				resExtract.Tag = control;
				resExtract.Owner = this;
				resExtract.Show();
			}
		}

		/// <summary>
		/// アクティブなスレッドを閉じる
		/// </summary>
		/// <param name="delete">ログを削除して閉じるかどうか</param>
		public void ThreadClose(bool delete)
		{
			if (threadTabController.IsSelected)
				ThreadCloseInternal(threadTabController.Control, delete);
		}

		/// <summary>
		/// 指定したスレッドのログを削除
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
		/// オートリロードを有効または解除
		/// </summary>
		/// <param name="enable">有効にする場合はtrue、無効にする場合はfalse</param>
		public void ThreadSetAutoReload(bool enable)
		{
			if (threadTabController.IsSelected)
			{
				threadTabController.Control.AutoReload = enable;
				UpdateToolBar();
			}
			else // スレッドが選択されていない場合は無効
				enable = false;

			autoReloadTimerCounter.Enabled = enable;
		}

		/// <summary>
		/// オートスクロールを有効または解除
		/// </summary>
		/// <param name="enable">有効にする場合はtrue、無効にする場合はfalse</param>
		public void ThreadSetAutoScroll(bool enable)
		{
			if (threadTabController.IsSelected)
			{
				threadTabController.Control.AutoScroll = enable;
				UpdateToolBar();
			}
		}

		/// <summary>
		/// スレッドをスクロール
		/// </summary>
		/// <param name="down">下へスクロールする場合はtrue、上へスクロールする場合はfalse</param>
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
		/// アクティブなスレッドのログを再取得
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
		/// すべてのスレッドを閉じる
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
		/// アクティブでないスレッドを閉じる
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
		/// 指定した操作を実行する
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
		/// スレッドを投稿
		/// </summary>
		public void PostThread(BoardInfo board)
		{
			PostThread(board, new PostThread(String.Empty, String.Empty));
		}

		/// <summary>
		/// スレッドを投稿
		/// </summary>
		public void PostThread(BoardInfo board, PostThread thread)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			// 板一覧に存在するかどうかを判断
			if (allTable.Contains(board))
			{
				PostDialog dlg = new PostDialog(cache, koteman, board, thread);
				ShowPostDialog(dlg);
			}
			else
			{
				MessageBox.Show("現在の板は未対応です");
			}
		}

		/// <summary>
		/// フロート書き込みダイアログを表示
		/// </summary>
		public void ThreadPostFloatDialog()
		{
			ThreadPostRes(String.Empty, true);
		}

		/// <summary>
		/// アクティブのスレッドにレスを投稿
		/// </summary>
		public void ThreadPostRes()
		{
			ThreadPostRes(String.Empty, !dockWriteBar.Visible);
		}
		/// <summary>
		/// アクティブのスレッドにレスを投稿
		/// </summary>
		public void ThreadPostRes(string text)
		{
			ThreadPostRes(text, !dockWriteBar.Visible);
		}

		/// <summary>
		/// アクティブのスレッドにレスを投稿
		/// </summary>
		public void ThreadPostRes(string text, bool floatmode)
		{
			if (threadTabController.IsSelected)
			{
				ThreadControl ctrl = threadTabController.Control;
				PostSettings ps = settings.Post;

				if (floatmode)
				{
					// 既にダイアログが表示されていれば再利用する
					bool recycle = (writeDialog != null);

					PostDialog dlg = recycle ? writeDialog :
						new PostDialog(cache, koteman, ctrl.HeaderInfo);

					// ダイアログを再利用する場合はsage状態を変更しない
					if (!recycle)
						dlg.Sage = ps.Sage;

					dlg.AppendText(text);
					//dlg.ImeOn = ps.ImeOn;
					ShowPostDialog(dlg);
				}
				else
				{
					dockWriteBar.AppendText(text);
					//dockWriteBar.Sage = ps.Sage;	// ドッキングバーのsage状態は起動時に一回だけ復元するように変更
					//dockWriteBar.Focus();
				}
			}
		}

		/// <summary>
		/// 指定した草稿を編集するためのレス投稿画面 (フロート書き込み) を表示
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
		/// レス投稿ダイアログを表示する内部関数。
		/// イベントのとウインドウ位置の設定を行う。
		/// </summary>
		/// <param name="dlg"></param>
		private void ShowPostDialog(PostDialog dlg)
		{
			PostSettings ps = settings.Post;
			dlg.Closed += new EventHandler(PostDialog_Closed);
			dlg.Posted += new PostedEventHandler(PostDialog_Posted);
			if (ps.ImeOn) dlg.ImeOn = true;
			dlg.Owner = this;

			// 書き込みダイアログを再利用
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


		// 投稿ダイアログが閉じられたらウインドウ位置とsage状態を保存
		private void PostDialog_Closed(object sender, EventArgs e)
		{
			PostDialog dlg = (PostDialog)sender;
			settings.Post.WindowLocation = dlg.Location;
			settings.Post.WindowSize = dlg.Size;
			settings.Post.Sage = dlg.Sage;
			writeDialog = null;
		}

		// 投稿成功したらリロードと、書き込み履歴の保存を行う
		private void PostDialog_Posted(object sender, PostedEventArgs e)
		{
			if (e.Type == PostType.Res)
			{
				ThreadHeader header = e.HeaderInfo;

				// 書き込み履歴を保存
				WroteRes wrote = new WroteRes(header, DateTime.Now, e.From, e.Email, e.Body);
				lock (lockObj)
					lastWroteRes = wrote;

				if (settings.Post.SavePostHistory)
				{
					// テキスト形式の書き込み履歴に保存
					kakikomi.Append(header, wrote);
				}

				koteman.Save(Settings.KotehanFilePath);				// コテハンを保存

				// 最近書き込んだスレ履歴に追加
				writtenThreadHistory.Items.Remove(header);
				writtenThreadHistory.Items.Add(header);
				writtenThreadHistory.Save();

				// スレッドを更新
				ThreadControl ctrl = threadTabController.FindControl(header);
				if (ctrl != null)
					ctrl.Reload();
			}
			// スレッドを立てた場合、板を開く
			else if (e.Type == PostType.Thread)
			{
				ListOpen(e.BoardInfo, true);
			}
		}

		private bool ClosingConfirmDialog()
		{
			if (settings.ClosingConfirm)
			{
				return MessageBox.Show("複数のタブが閉じられようとしています。閉じてよろしいですか", "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
			}
			else
				return true;
		}

		#endregion

		#region お気に入り関連メソッド

		void OnBookmarkChanged(object sender, EventArgs e)
		{
			SaveBookmarks();
		}

		void SaveBookmarks()
		{
			bookmarkRoot.Save(Settings.BookmarkPath);			// お気に入りを保存
			warehouseRoot.Save(Settings.WarehousePath);			// 過去ログ倉庫を保存
		}

		/// <summary>
		/// お気に入りタブを作成
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
		/// 指定したフォルダ内のお気に入りをリスト表示
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
		/// 選択されているフォルダのお気に入りを巡回
		/// </summary>
		private void BookmarkPatrol(bool checkOnly, bool includeSubChildren)
		{
			// 巡回対象スレッドを取得
			BookmarkFolder folder = bookmarkView.SelectedFolder;
			if (folder == null)
				folder = bookmarkRoot;

			BookmarkPatrol(folder, checkOnly, includeSubChildren);
		}

		/// <summary>
		/// 指定したフォルダを巡回
		/// </summary>
		/// <param name="folder">巡回するフォルダ</param>
		/// <param name="checkOnly">更新チェックする場合はtrue、巡回する場合はfalse</param>
		/// <param name="includeSubChildren">サブフォルダも含める場合はtrue</param>
		private void BookmarkPatrol(BookmarkFolder folder, bool checkOnly, bool includeSubChildren)
		{
			BookmarkPatrol(folder.GetBookmarks(includeSubChildren), checkOnly);
		}

		/// <summary>
		/// 指定したフォルダを巡回
		/// </summary>
		/// <param name="items">巡回するスレッドリスト</param>
		/// <param name="checkOnly">更新チェックする場合はtrue、巡回する場合はfalse</param>
		private void BookmarkPatrol(List<ThreadHeader> items, bool checkOnly)
		{
			if (patroller == null)
			{
				if (checkOnly)
					patroller = new CheckOnlyPatroller(cache);
				else
					patroller = new DefaultPatroller(cache);
				UpdateToolBar();

				// お気に入りタブを作成
				listBookmarkControl = CreateBookmarkWindow(new List<ThreadHeader>());
				SetStatusBarInfo(StatusBarPanelIndex.Text, "お気に入りを巡回しています...");

				patroller.StatusTextChanged += new StatusTextEventHandler(BookmarkPatrol_StatusTextChanged);
				patroller.SetItems(items);
				patroller.Patroling += new PatrolEventHandler(BookmarkPatrol_Patroling);
				patroller.Updated += new PatrolEventHandler(BookmarkPatrol_Updated);
				patroller.BeginPatrol(new AsyncCallback(BookmarkPatrol_Patroling), null);
			}
		}

		/// <summary>
		/// お気に入りに登録または解除。
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
				SetStatusBarInfo(StatusBarPanelIndex.Text, "お気に入りの巡回を完了しました。");
				UpdateToolBar();
			};
			if (InvokeRequired)
				Invoke(m);
			else
				m();
		}
		#endregion

		#region 板ボタン
		private void ItaBotanSet(object tag)
		{
			ItaBotanSet(tag, tag.ToString());
		}

		private void ItaBotanSet(object tag, string text)
		{
			ItaBotanSet(tag, text, -1);
		}

		/// <summary>
		/// 指定したtagを持つ板ボタンを追加
		/// </summary>
		/// <param name="obj"></param>
		private void ItaBotanSet(object tag, string text, int index)
		{
			CSharpToolBarButton button = ItaBotanFind(tag);
			if (button == null)
			{
				// 板ボタンに存在しなければ新しく追加
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
				// 既に存在したら削除
				cSharpToolBar.Buttons.Remove(button);
			}

			if (initializing == false)
				SaveItaBotan();
		}

		/// <summary>
		/// 板ボタンからbuttonを削除
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
		/// 指定したtagを持つ板ボタンを検索
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

		#region NGワード関連
		/// <summary>
		/// NGワードを編集するダイアログを表示
		/// </summary>
		public void ShowNGWordsEditor()
		{
			NGWordEditorDialog dlg = new NGWordEditorDialog(allTable);
			dlg.ShowDialog(this);

			Twinie.NGWords.Save();

		}
		#endregion

		#region コントロールイベント
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
			// 前回の状態を復元する場合の処理
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
		/// 終了処理を行います。
		/// </summary>
		/// <returns>正常に終了できる場合は true、キャンセルされた場合は false です。</returns>
		private bool ClosingInternal()
		{
			try
			{
				// すべての接続を停止
				ClientBase.Stopper.Reset();

				if (!CloseAllWindow())
				{
					return false;
				}

				SaveSettingsAll();
				Dispose(true);		// 使用しているリソースを解放

				// 画像ビューアを閉じる
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

			// タスクトレイに入れる場合、最小化されたときだけタスクバーに表示させない
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
				// プログレスバーを再描画
				progress.Refresh();
			}
			if (rebarWrapper != null && this.WindowState != FormWindowState.Minimized)
			{
				// 最小化し、サイズを元に戻したときに表示がおかしくなるので、
				// RebarのResizeイベントを発生させる
				rebarWrapper.UpdateSize();
			}
		}
		#endregion

		#region StatusBar Events
		private void statusBar_DrawItem(object sender, System.Windows.Forms.StatusBarDrawItemEventArgs sbdevent)
		{
			// プログレスバー
			if (sbdevent.Index == statusBar.Panels.IndexOf(statusProgress))
			{
				Rectangle rect = sbdevent.Bounds;
				rect.X -= 1;
				rect.Y -= 1;
				rect.Width += 2;
				rect.Height += 2;

				progress.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
			}
			// サイズが512KB超えたら赤く表示
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
					// 板名に一致すればその板を開く
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
						// 数字が選択されていたらポップアップ
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
				// 開かれているサーバー名を取得
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

					// ドッキング書き込みバーに情報を設定
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

					// 更新チェック済みで、オートリロードが一時停止している場合は
					// 間隔をリセットして再開する
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
					// 板ボタンの左下にメニューを表示する
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

		#region クライアント イベントハンドラ
		private void ClientBase_StatusTextChanged(object sender, StatusTextEventArgs e)
		{
			// 404エラーが発生した場合、板が移転している可能性があるので、
			// 板一覧の更新を奨める
			string text = e.Text;

			if (e.Text.IndexOf("404") >= 0)
				text += " (板が移転したかもしれないので板一覧を更新してみて)";

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

			// エラーサウンドを鳴らす
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
		/// スレッドの状態を表すイメージのインデックスを設定
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="ctrl"></param>
		/// <param name="cs"></param>
		private void SetThreadImageIndex(TabPage tab, ThreadControl ctrl, CompleteStatus status)
		{
			if (status == CompleteStatus.Success)
			{
				// このスレッドのウインドウ情報を取得
				//TwinWindow<THeader, TControl> win = (TwinWindow<THeader, TControl>)tab.Tag;
				TwinThreadWindow win = threadTabController.FindWindow(ctrl.HeaderInfo);

				// 状態ごとにアイコンを変更
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

		// スレッド一覧の読み込み開始時に呼ばれる
		private void OnListLoading(ThreadListControl control)
		{
			if (ClientIsActive(control))
				SetBoardInfo(control.BoardInfo);
		}

		/// <summary>
		/// スレッド一覧の完了時に呼ばれる
		/// </summary>
		/// <param name="control"></param>
		private void OnListComplete(ThreadListControl control, CompleteEventArgs e)
		{
		}

		// スレッドの読み込み開始時に呼ばれる
		private void OnThreadLoading(ThreadControl control)
		{
			if (ClientIsActive(control))
			{
				// スレッドのサイズを更新
				ThreadHeader header = control.HeaderInfo;
				SetHeaderInfo(header);
				dockWriteBar.Select(header);
			}
		}

		// スレッドの完了時に呼ばれる
		private void OnThreadComplete(ThreadControl control, CompleteEventArgs e)
		{
			// スレッドのサイズを更新
			ThreadHeader header = control.HeaderInfo;

			// スレッド一覧の情報を更新
			UpdateThreadInfo(header);

			// タブの情報、タブ色を更新
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
					// 次スレチェッカー
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

			// 非アクティブスレッドで、更新チェックのみのオートリロードの場合、
			// 新着があった時点でタイマーを一時停止。

			// タブがアクティブになったときにタイマーを再開する

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

			// 新着があれば音を鳴らす
			if (header.NewResCount > 0 && File.Exists(Twinie.Sound.NewRes))
			{
				System.Media.SoundPlayer p = new System.Media.SoundPlayer(Twinie.Sound.NewRes);
				p.Play();
				p.Dispose();
			}
		}
		#endregion

		#region スレッド イベントハンドラ
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
				// このスレッドがOverThreadでe.Uriが2chのスレッドなら、同じタブで開く
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

		#region スレッド一覧 イベントハンドラ
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

		#region 次スレ案内 イベントハンドラ
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
			SetStatusBarInfo(0, "次スレと思われるスレッド数: " + e.Items.Count);
			nextThreadChecker = null;

			if (autoNextThreadOpen)
			{
				foreach (ThreadHeader h in e.Items)
				{
					ThreadOpen(h, true);
				}
			}
			// 次スレ候補でスレに開いているスレがあれば更新
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

		#region 画像ビューア
		private void imageViewer_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = !__closing;

			if (e.Cancel)
				imageViewer.Hide();
		}
		#endregion

		#region マウスジェスチャ
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

			string text = String.Format("{0}/{1}秒",
				timeleft < 0 ? String.Empty : timeleft.ToString(),	// 残り秒数
				interval / 1000);										// 更新間隔

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
				MessageBox.Show("この操作を実行するには、全てのスレッドを閉じる必要があります。");
				return;
			}

			if (MessageBox.Show("低負荷用の画像サムネイルキャッシュを全て削除します。\r\nよろしいですか？", "削除の確認",
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
			dlg.Description = "保存先のフォルダを指定してください。";

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
				MessageBox.Show(count + "個のスレッドを保存しました。");
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
						// 同名ファイルが存在しないか確認
						ThreadHeader h = TypeCreator.CreateThreadHeader(bi.Bbs);
						h.BoardInfo = bi;
						h.Key = m.Groups[1].Value;

						string destPath = this.cache.GetDatPath(h);
						if (File.Exists(destPath))
						{
							DialogResult r = MessageBox.Show("ファイル: " + fn + "は既に同名のdatが存在します。上書きしますか？",
								"上書き確認", MessageBoxButtons.YesNoCancel);
							if (r == DialogResult.Cancel)
								return;
							else if (r == DialogResult.No)
								continue;
						}
						ThreadUtil.OpenDat(this.cache, bi, fn, Path.GetFileNameWithoutExtension(fn), false);
						copiedCount++;
					}
				}

				MessageBox.Show(copiedCount + "個のdatファイルを" + bi.Name + "板に関連付けました。");

				if (MessageBox.Show("新しく追加されたdatを反映させるためにインデックスを再構築しますか？\r\n(後からでもこの操作は出来ます)", "インデックスの再構築",
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

		// BEの仕様変更に対応するテスト
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
				this.Text += " ●";

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
	/// アイコンの番号を表す
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
