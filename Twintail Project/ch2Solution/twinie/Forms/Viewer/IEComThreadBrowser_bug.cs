// ThreadBrowser.cs

namespace Twin.Forms
{
	using System;
	using System.Windows.Forms;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Threading;
	using System.Web;
	using mshtml;
	using AxSHDocVw;
	using SHDocVw;
	using CSharpSamples;
	using CSharpSamples.Text.Search;

	using Twin.Bbs;
	using Twin.Tools;
	using Twin.Text;
	using Twin.Util;
	using Twin.IO;
	using PopupTest;

	using _StringPair = System.Collections.Generic.KeyValuePair<string, string>;

	///	<summary>
	///	IEコンポーネントを使用しスレッドを表示するコントロール
	///	</summary>
	public class IEComThreadBrowser : ThreadControl, IExternalMethod//, IDocHostUIHandler
	{
		// デリゲートの定義
		private delegate void PopupMethodInvoker(string argument);

		// オートリロードするためのクラス
		private readonly static AutoRefreshTimerBase autoRefreshTimers;

		// 外部から関数を起動するためのクラス
		private readonly IEComMethodInvoker methodInvoker;

		private static ImageViewUrlReplace imageViewUrlTool = null;
		public static ImageViewUrlReplace ImageViewUrlReplace
		{
			get
			{
				return imageViewUrlTool;
			}
		}

		// 画像をキャッシュするクライアント
		private ImageCacheClient imageCacheClient = null;
		private AutoResetEvent iccEvent = new AutoResetEvent(false);

		// NGリンク
		public readonly static LinkInfoCollection linkCollection;
		private static readonly NGWords defNGWords;
		private NGWords nGWords;
		// 一日経過したNGIDをクリアした板の情報を含むリスト
		private readonly static List<string> clearedIdList = new List<string>();

		private AxWebBrowser webBrowser;			// スレッドを表示するコントロール
		private IPopupBase popInterf;				// ポップアップを制御するインターフェース
		private PopupSettings popSett;				// ポップアップの設定情報
		private ThreadSettings threadSett;			// スレッド設定
		private SkinStyle skinStyle;				// スキンの表示情報を管理するクラス
		private ThreadSkinBase skinBase;			// スキンの変換処理を行うクラス
		private ObjectTimer popTimer;				// レス参照をポインタしてからポップアップするまでの間隔を管理するタイマー
		private ABone abone;						// あぼーん情報
		private SortedValueCollection abonIndices;	// あぼーん済みレス番号をコレクション管理 (連鎖あぼーんに使用する)
		private Stack<float> scrolled;						// スクロール状態を記憶するコレクション

		private string tempStatusText;				// AxWebBrowser.StatusTextChangeイベントが発生するたびに引数の"e.text"がコピーされる変数
		private bool enableAutoScroll;				// 書き込み完了時にオートスクロールを実行するかどうか
		private int resBegin, resEnd;				// 描画始めレス番号と描画終了レス番号を表す
		private int viewResCount;					// レスの表示制限数を表す

		private bool scrollToNewRes;				// 新着までスクロールするかどうかを示す値
		private bool autoReload;					// オートリロードを行うかどうか
		private bool autoScroll;					// オートスクロールを行うかどうか

		internal bool isExtracting = false;			// 抽出中かどうか
		internal string lastPopupRef;				// 最後にポップアップしたレス番号が格納される (同じレスが２重ポップアップされてしまうのを防ぐため)
		private bool disposed = false;				// Disposeメソッドが呼ばれたかどうかを表す

		internal bool clickedPopup;
		//		private	bool newPopup;		// 新規のポップアップ

		private bool pastlogChecked = false;
		private bool kakoChecked = false;

		private object document = null;

		private Stack<_StringPair> createThumbImageUrls = new Stack<_StringPair>();

		///	<summary>
		///	自動更新タイマークラスを取得
		///	</summary>
		public static AutoRefreshTimerBase AutoRefreshTimers
		{
			get
			{
				return autoRefreshTimers;
			}
		}


		private Thumbnail thumbnail = new Thumbnail();
		/// <summary>
		/// イメージのサムネイル情報を取得します。
		/// </summary>
		public Thumbnail Thumbnail
		{
			get
			{
				return thumbnail;
			}
		}

		///	<summary>
		///	選択されている文字列を取得
		///	</summary>
		public override string SelectedText
		{
			get
			{
				if (IsOpen)
				{
					HTMLDocument doc = GetDocument();
					IHTMLTxtRange range = (IHTMLTxtRange)doc.selection.createRange();
					return (range.text != null) ? range.text : String.Empty;
				}
				return String.Empty;
			}
		}

		/// <summary>
		/// 新着までスクロール
		/// </summary>
		public override bool ScrollToNewRes
		{
			set
			{
				if (scrollToNewRes != value)
				{
					scrollToNewRes = value;
					autoScroll = false;
				}
			}
			get
			{
				return scrollToNewRes;
			}
		}

		///	<summary>
		///	オートスクロールの有効・無効
		///	</summary>
		public override bool AutoScroll
		{
			set
			{
				if (autoScroll != value)
				{
					autoScroll = value;
					scrollToNewRes = false;
				}
			}
			get
			{
				return autoScroll;
			}
		}

		///	<summary>
		///	オートスリロードの有効・無効
		///	</summary>
		public override bool AutoReload
		{
			set
			{
				if (autoReload != value)
				{
					autoReload = value;
					if (value)
						autoRefreshTimers.Add(this);
					else
						autoRefreshTimers.Remove(this);
				}
			}
			get
			{
				return autoReload;
			}
		}

		/// <summary>
		/// フォントサイズを取得または設定
		/// </summary>
		public override FontSize FontSize
		{
			set
			{
				object val = (int)value, nul = null;

				webBrowser.ExecWB(OLECMDID.OLECMDID_ZOOM, OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT,
					ref val, ref nul);
			}
			get
			{
				object val = null, nul = null;

				webBrowser.ExecWB(OLECMDID.OLECMDID_ZOOM, OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT,
					ref nul, ref val);

				return (FontSize)Enum.Parse(typeof(FontSize), val.ToString());
			}
		}

		///	<summary>
		///	表示レス数を取得または設定
		///	</summary>
		public override int ViewResCount
		{
			set
			{
				if (value < 1)
					throw new ArgumentOutOfRangeException("ViewResCount");
				viewResCount = value;
				Range(headerInfo.GotResCount - value, -1);
			}
			get
			{
				return viewResCount;
			}
		}

		//		///	<summary>
		//		///	あぼーん情報を取得
		//		///	</summary>
		//		public ABone ABone {
		//			get	{
		//				return abone;
		//			}
		//		}
		//
		//		///	<summary>
		//		///	表示に使用するスキンを取得
		//		///	</summary>
		//		public ThreadSkinBase Skin {
		//			get	{ return skinBase; }
		//		}

		///	<summary>
		///	IEComThreadBrowser
		///	</summary>
		static IEComThreadBrowser()
		{
			// オートリロードタイマー
			autoRefreshTimers = new AutoRefreshTimerCollection2();
			// デフォルトのNGワード設定
			defNGWords = Twinie.NGWords.Default;

			imageViewUrlTool = new ImageViewUrlReplace(Settings.ImageViewUrlReplacePath);

			try
			{//	NGアドレスを読み込む
				linkCollection = new LinkInfoCollection(Settings.NGAddrsPath);
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		///	<summary>
		///	IEComThreadBrowserクラスのインスタンスを初期化
		///	</summary>
		public IEComThreadBrowser(Cache cache, Settings sett)
			: base(cache)
		{
			// 
			// TODO: コンストラクタ	ロジックをここに追加してください。
			//

			TabStop = false;

			#region	Initialize AxWebBrowser
			webBrowser = CreateWebBrowser(this);
			webBrowser.NewWindow2 += new AxSHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(OnNewWindow2);
			webBrowser.StatusTextChange += new AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler(OnStatusTextChange);
			OpenBlank();

			while (webBrowser.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE)
			{
				Application.DoEvents();
			}

			document = webBrowser.Document;

			object ocx = webBrowser.GetOcx();
			SHDocVw.WebBrowser_V1 wb = ocx as SHDocVw.WebBrowser_V1;
			if (wb != null)
				wb.BeforeNavigate += new SHDocVw.DWebBrowserEvents_BeforeNavigateEventHandler(OnBeforeNavigate);
			//			InstallIDocHostUIHandler((ICustomDoc)webBrowser.Document);
			#endregion

			tempStatusText =
				lastPopupRef = String.Empty;

			scrolled = new Stack<float>();

			// 外部メソッド
			IExternalMethod iem = this;
			methodInvoker = new IEComMethodInvoker(iem);

			// あぼーん関連
			abone = sett.ABone;
			abonIndices = new SortedValueCollection();

			// ポップアップ関連
			popSett = sett.Popup;
			popTimer = new ObjectTimer();
			popTimer.Elapsed += new ObjectTimerEventHandler(OnPopup);
			popTimer.Interval = popSett.PopupInterval;

			// スキン関連
			skinStyle = new SkinStyle(sett);
			skinStyle.Read(Path.Combine(sett.SkinFolderPath, "Style.ini"));

			Twin2IeHtmlSkin skin = new Twin2IeHtmlSkin(sett.SkinFolderPath);
			skinBase = skin;

			if (popSett.Extend)
				skin.ExtendPopupStr = popSett.ExtendPopupStr;

			// サムネイル設定
			thumbnail = sett.Thumbnail;

			// イメージキャッシュ
			imageCacheClient = new ImageCacheClient();
			imageCacheClient.ImageCached += new ImageCacheEventHandler(imageCacheClient_ImageCached);
			imageCacheClient.ImageCacheDirectory = Settings.ImageCacheDirectory;


			// ポップアップクラスを初期化
			if (skinStyle.style == PopupStyle.Text)
				popInterf = new SimplePopup(Twinie.Form);
			else if (skinStyle.style == PopupStyle.Html)
				popInterf = new HtmlPopup(this, skinBase);

			popInterf.ImageSize = popSett.ImagePopupSize;
			popInterf.Position = popSett.Position;
			popInterf.Maximum = popSett.Maximum;
			popInterf.BackColor = skinStyle.backColor;
			popInterf.ForeColor = skinStyle.foreColor;
			popInterf.Font = skinStyle.font;

			// そのほかの設定
			threadSett = sett.Thread;
			viewResCount = threadSett.ViewResCount;
			scrollToNewRes = threadSett.ScrollToNewRes;
			autoScroll = threadSett.AutoScrollOn;
			autoReload = threadSett.AutoReloadOn;

			bufferSize = sett.Net.BufferSize;
			UseGzip = sett.UseGzipArchive;
			IsPackageReception = sett.Net.PackageReception;
		}

		///	<summary>
		///	使用しているリソースを解放
		///	</summary>
		///	<param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				try
				{
					if (disposing)
					{
						autoRefreshTimers.Remove(this);
						webBrowser.Dispose();
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
			disposed = true;
		}


		void imageCacheClient_ImageCached(object sender, ImageCacheEventArgs e)
		{

			// すべてのIMGタグを取得
			MethodInvoker m = delegate
			{
				IHTMLElementCollection tags =
					GetDocument().getElementsByName(e.SourceUri.GetHashCode().ToString());

				foreach (IHTMLElement element in tags)
				{
					if (String.Compare(element.tagName, "IMG", true) == 0)
					{
						IHTMLImgElement img = element as IHTMLImgElement;
						if (img != null && String.IsNullOrEmpty(img.src))
						{
							img.src = e.FileName;
						}
					}
				}
			};

			// キャッシュされた画像をサムネイルに変換
			lock (typeof(ImageThumbnail))
			{
				ImageThumbnail.Convert(e.FileName, thumbnail.Size);
			}

			if (iccEvent.WaitOne(1000, false))
			{
				Invoke(m);

				iccEvent.Set();
			}
		}

		///	<summary>
		///	AxWebBrowserを初期化
		///	</summary>
		///	<param name="owner">親コントロール (nullを指定可能)</param>
		///	<returns>初期化されたAxWebBrowserのインスタンス</returns>
		public static AxWebBrowser CreateWebBrowser(Control owner)
		{
			AxWebBrowser wb = new AxWebBrowser();
			wb.BeginInit();
			if (owner != null)
				owner.SuspendLayout();
			wb.Enabled = true;
			wb.ImeMode = 0;
			wb.TabIndex = 0;
			wb.Dock = DockStyle.Fill;
			if (owner != null)
				owner.Controls.Add(wb);
			wb.EndInit();
			if (owner != null)
				owner.ResumeLayout(true);

			return wb;
		}

		#region	WebBrowser Events
		protected virtual void OnBeforeNavigate(string uRL,
			int Flags, string TargetFrameName, ref object PostData, string Headers, ref	bool Processed)
		{
			Processed = true;
			//			OnUriClick(new UriClickEventArgs(uRL, info));
		}

		protected virtual void OnStatusTextChange(object sender,
			AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEvent e)
		{

			popTimer.Stop();
			tempStatusText = e.text;

			// Shiftキーが押されているまたはマウスボタンが押されている場合はポップアップしない
			if ((ModifierKeys & Keys.Shift) != 0 || (MouseButtons != MouseButtons.None))
				return;

			// 現在ポップアップ中のレスはポップアップしない
			if (popInterf is HtmlPopup && lastPopupRef.Equals(tempStatusText))
				return;

			// メソッドを起動
			if (tempStatusText.StartsWith("mouseover:"))
			{
				string funcText = tempStatusText.Substring(10);
				methodInvoker.Invoke(funcText);
				return;
			}

			if (IsResPopupable(e.text) ||
				IsMailPopupable(e.text) ||
				IsPicturePopupable(e.text))
			{
				// ポップアップタイマー開始
				popTimer.Start(e.text);
			}
			else if (popInterf is SimplePopup)
			{
				HidePopup();
			}
		}

		protected virtual void OnNewWindow2(object sender,
			AxSHDocVw.DWebBrowserEvents2_NewWindow2Event e)
		{
			try
			{
				e.cancel = true;

				// レス番号がクリックされた
				if (tempStatusText.StartsWith("menu:"))
				{
					int index = Int32.Parse(tempStatusText.Substring(5));
					ResSet resSet = resCollection[index - 1];

					OnNumberClick(new NumberClickEventArgs(headerInfo, resSet));
					return;
				}
				// メソッドを起動
				else if (tempStatusText.StartsWith("method:"))
				{
					string funcText = tempStatusText.Substring(7);
					methodInvoker.Invoke(funcText);
					return;
				}

				// レス参照がクリックされた
				else if (IsResPopupable(tempStatusText))
				{
					ThreadHeader header = URLParser.ParseThread(tempStatusText);
					int[] num = ResReference.GetArray(tempStatusText);

					if (HeaderInfo.Equals(header))
					{
						if (num.Length > 0)
						{
							scrolled.Push(GetScrollPosition());
							HidePopup();

							// beta27
							ScrollTo(num[0]);

							// クリックされたレス番が、表示されていなければ再レンダリング
							//							if (num[0] < resBegin)
							//								Range(num[0], num[0] + viewResCount);
							//
							//							SetScrollPosition(num[0]);
						}
						return;
					}
				}

				string linkText = GetLinkText((AxWebBrowser)sender, tempStatusText);
				LinkInfo info = linkCollection.IndexOf(linkText);
				OnUriClick(new UriClickEventArgs(linkText, info));
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		private string GetLinkText(AxSHDocVw.AxWebBrowser webBrowser, string defaultText)
		{
			string linkText = defaultText;
			try
			{
				Uri uri = new Uri(linkText);

				if (uri.Host.Contains("wikipedia"))
				{
					linkText = HttpUtility.UrlDecode(uri.AbsoluteUri);
				}
			}
			catch
			{
			}

			return linkText;
		}
		#endregion

		#region	IDocHostUIHandler Implementation
		/*		///	<summary>
		///	Called by MSHTML to	display	a shortcut menu.
		///	</summary>
		///	<param name="dwID"></param>
		///	<param name="ppt"></param>
		///	<param name="pcmdtReserved"></param>
		///	<param name="pdispReserved"></param>
		///	<returns></returns>
		HRESULT	IDocHostUIHandler.ShowContextMenu(uint dwID, ref tagPOINT ppt, object pcmdtReserved, object	pdispReserved)
		{
			HRESULT hr = HRESULT.S_FALSE;
			if (popInterf.Visible && newPopup)
			{
				hr = HRESULT.S_OK;
				newPopup = false;
			}
			return hr;
		}

		///	<summary>
		///	Called by MSHTML to	retrieve the user interface	(UI) capabilities
		///	of the application that	is hosting MSHTML.
		///	</summary>
		///	<param name="pInfo"></param>
		HRESULT IDocHostUIHandler.GetHostInfo(ref DOCHOSTUIINFO pInfo)
		{
			return HRESULT.S_OK;
		}

		///	<summary>
		///	Called by MSHTML to	enable the host	to replace MSHTML menus	and	toolbars.
		///	</summary>
		///	<param name="dwID"></param>
		///	<param name="pActiveObject"></param>
		///	<param name="pCommandTarget"></param>
		///	<param name="pFrame"></param>
		///	<param name="pDoc"></param>
		///	<returns>
		///	S_OK: Host displayed its own UI. MSHTML	will not display its UI.
		///	S_FALSE: Host did not display its own UI. MSHTML will display its UI.
		///	DOCHOST_E_UNKNOWN:	Host did not recognize the UI identifier.
		///						MSHTML will	either try an alternative identifier
		///						for	compatibility with a previous version or display its own UI. 
		///	</returns>
		HRESULT	IDocHostUIHandler.ShowUI(uint dwID,	IntPtr pActiveObject, IntPtr pCommandTarget, IntPtr	pFrame,	IntPtr pDoc)
		{
			return HRESULT.S_FALSE;
		}

		///	<summary>
		///	Called when	MSHTML removes its menus and toolbars.
		///	</summary>
		HRESULT IDocHostUIHandler.HideUI()
		{
			return HRESULT.S_OK;
		}

		///	<summary>
		///	Called by MSHTML to	notify the host	that the command state has changed.
		///	</summary>
		HRESULT IDocHostUIHandler.UpdateUI()
		{
			return HRESULT.S_OK;
		}

		///	<summary>
		///	Called by the MSHTML implementation	of IOleInPlaceActiveObject::EnableModeless.
		///	Also called	when MSHTML	displays a modal UI.
		///	</summary>
		///	<param name="fEnable"></param>
		HRESULT IDocHostUIHandler.EnableModeless(int fEnable)
		{
			return HRESULT.S_OK;
		}

		///	<summary>
		///	Called by the MSHTML implementation	of IOleInPlaceActiveObject::OnDocWindowActivate.
		///	</summary>
		///	<param name="fActivate"></param>
		HRESULT IDocHostUIHandler.OnDocWindowActivate(int fActivate)
		{
			return HRESULT.E_NOTIMPL;
		}

		///	<summary>
		///	Called by the MSHTML implementation	of IOleInPlaceActiveObject::OnFrameWindowActivate.
		///	</summary>
		///	<param name="fActivate"></param>
		HRESULT IDocHostUIHandler.OnFrameWindowActivate(int fActivate)
		{
			return HRESULT.E_NOTIMPL;
		}

		///	<summary>
		///	Called by the MSHTML implementation	of IOleInPlaceActiveObject::ResizeBorder.
		///	</summary>
		///	<param name="prcBorder"></param>
		///	<param name="pUIWindow"></param>
		///	<param name="fRameWindow"></param>
		HRESULT IDocHostUIHandler.ResizeBorder(ref tagRECT prcBorder, IntPtr pUIWindow, int fRameWindow)
		{
			return HRESULT.E_NOTIMPL;
		}

		///	<summary>
		///	Called by MSHTML when IOleInPlaceActiveObject::TranslateAccelerator
		///	or IOleControlSite::TranslateAccelerator is	called.
		///	</summary>
		///	<param name="lpmsg"></param>
		///	<param name="pguidCmdGroup"></param>
		///	<param name="nCmdID"></param>
		///	<returns>
		///	Returns	S_OK if	successful,	or an error	value otherwise.
		///	</returns>
		HRESULT IDocHostUIHandler.TranslateAccelerator(ref tagMSG lpmsg, ref Guid pguidCmdGroup, uint nCmdID)
		{
			if (((lpmsg.wParam & 0xFF) == 'F') && ((ModifierKeys & Keys.Control) != 0))
			{
				if (Twinie.Form.GetType().Equals(typeof(Twin2IeBrowser)))
				{
					((Twin2IeBrowser)Twinie.Form).ThreadFind();
					return HRESULT.S_OK;
				}
			}
			return HRESULT.S_FALSE;
		}

		///	<summary>
		///	Called by the WebBrowser Control to	retrieve a registry	subkey path
		///	that overrides the default Microsoftｮ Internet Explorer	registry settings.
		///	</summary>
		///	<param name="pchKey"></param>
		///	<param name="dw"></param>
		HRESULT IDocHostUIHandler.GetOptionKeyPath(out string pchKey, uint dw)
		{
			pchKey = null;
			return HRESULT.S_OK;
		}

		///	<summary>
		///	Called by MSHTML when it is	used as	a drop target. This	method enables
		///	the	host to	supply an alternative IDropTarget interface.
		///	</summary>
		///	<param name="pDropTarget">
		///	[in] Pointer to	an IDropTarget interface for the current drop target
		///	object supplied	by MSHTML.
		///	</param>
		///	<param name="ppDropTarget">
		///	[out] Address of a pointer variable	that receives an IDropTarget
		///	interface pointer for the alternative drop target object supplied by
		///	the	host.
		///	</param>
		///	<returns>
		///	Returns	S_OK if	successful,	or an error	value otherwise.
		///	</returns>
		HRESULT IDocHostUIHandler.GetDropTarget(IntPtr pDropTarget, out	IntPtr ppDropTarget)
		{
			ppDropTarget = IntPtr.Zero;
			return HRESULT.E_NOTIMPL;
		}

		///	<summary>
		///	Called by MSHTML to	obtain the host's IDispatch	interface.
		///	</summary>
		///	<param name="ppDispatch">
		///	out] Address of	a pointer to a variable	that receives an IDispatch
		///	interface pointer for the host application.
		///	</param>
		///	<returns>
		///	Returns	S_OK if	successful,	or an error	value otherwise. 
		///	</returns>
		HRESULT IDocHostUIHandler.GetExternal(out object ppDispatch)
		{
			ppDispatch = IntPtr.Zero;
			return HRESULT.E_NOTIMPL;
		}

		///	<summary>
		///	Called by MSHTML to	give the host an opportunity to	modify the URL to be loaded.
		///	</summary>
		///	<param name="dwTranslate"></param>
		///	<param name="pchURLIn"></param>
		///	<param name="ppchURLOut"></param>
		///	<returns>
		///	Returns	S_OK if	the	URL	was	translated,	or S_FALSE if the URL was not translated.
		///	</returns>
		HRESULT IDocHostUIHandler.TranslateUrl(uint dwTranslate, ref ushort pchURLIn, IntPtr ppchURLOut)
		{
			return HRESULT.E_NOTIMPL;
		}

		///	<summary>
		///	Called by MSHTML to	allow the host to replace the MSHTML data object.
		///	</summary>
		///	<param name="pDO"></param>
		///	<param name="ppDORet"></param>
		///	<returns>
		///	Returns	S_OK if	the	data object	is replaced, or	S_FALSE	if it's	not	replaced.
		///	</returns>
		HRESULT IDocHostUIHandler.FilterDataObject(IntPtr pDO, out IntPtr ppDORet)
		{
			ppDORet	= IntPtr.Zero;
			return HRESULT.E_NOTIMPL;
		}

		///	<summary>
		///	Install	IDocHostUIHandler
		///	</summary>
		private	void InstallIDocHostUIHandler(ICustomDoc customDoc)
		{
			if (customDoc != null)
			{
				try
				{
					customDoc.SetUIHandler((IDocHostUIHandler)this);
				}
				catch (Exception) {}
			}
		}*/
		#endregion

		#region	外部呼び出し可能なメソッド
		void IExternalMethod.BackReferences(int index)
		{
			PopupBackReferences(index);
		}

		void IExternalMethod.SetLimit(int limit)
		{
			ViewResCount = limit;
		}

		void IExternalMethod.Extract(int obj, string key)
		{
			ResSetElement element = (ResSetElement)obj;
			AbstractExtractor extractor = BeginExtract();
			extractor.NewWindow = true;
			extractor.InnerExtract(key, element);
		}

		void IExternalMethod.ScrollTop()
		{
			ScrollTo(ScrollPosition.Top);
		}

		void IExternalMethod.ScrollBottom()
		{
			ScrollTo(ScrollPosition.Bottom);
		}

		void IExternalMethod.Reload()
		{
			Reload();
		}

		void IExternalMethod.Range(int start, int end)
		{
			Range(start, end);
		}

		void IExternalMethod.Next(int count)
		{
			viewResCount = count;
			Range(RangeMovement.Forward);
		}

		void IExternalMethod.Prev(int count)
		{
			viewResCount = count;
			Range(RangeMovement.Back);
		}
		#endregion

		#region	Privateメソッド
		///	<summary>
		///	スクロール位置を取得
		///	</summary>
		///	<returns></returns>
		private float GetScrollPosition()
		{
			// 現在一番上に表示されているレス番号を取得
			try
			{
				HTMLBody body = (HTMLBody)GetHtmlBody();
				IHTMLElementCollection collect = (IHTMLElementCollection)body.getElementsByTagName("indices");
				IHTMLElement element = null;

				int scrollTop = body.scrollTop;
				int left = 0, right = collect.length - 1;

				// 二分探索
				while (left < right)
				{
					int mid = (left + right) / 2;
					element = (IHTMLElement)collect.item(mid, mid);

					if (scrollTop < element.offsetTop)
						right = mid - 1;
					else if (scrollTop > element.offsetTop)
						left = mid + 1;
					else
					{
						left = mid;
						break;
					}
				}

				int index = 0, sub = 0;

				if (element != null)
				{
					if (scrollTop > element.offsetTop && ++left < collect.length)
						element = (IHTMLElement)collect.item(left, left);

					index = Int32.Parse(element.id);
					sub = Math.Abs(element.offsetTop - scrollTop);
				}
				return Single.Parse(index + "." + sub);
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
			return 0.0f;
		}

		///	<summary>
		///	再描画
		///	</summary>
		private void Redraw()
		{
			headerInfo.Position = GetScrollPosition();
			Range(resBegin, resEnd);
			SetScrollPosition(headerInfo.Position);
		}

		///	<summary>
		///	白紙を開く
		///	</summary>
		private void OpenBlank()
		{
			webBrowser.Navigate("about:blank");
		}

		///	<summary>
		///	フッターに<div id="footer"></div>が存在した場合、削除する
		///	</summary>
		private void RemoveFooter()
		{
			IHTMLDocument3 doc = (IHTMLDocument3)GetDocument();
			HTMLDivElement div = (HTMLDivElement)doc.getElementById("footer");

			if (div != null)
				div.removeNode(true);
		}

		///	<summary>
		///	指定したレスのNGワードチェック
		///	</summary>
		///	<param name="res"></param>
		///	<returns>NGワード処理後のレス構造体</returns>
		private ResSet CheckNGWords(ResSet res)
		{
			// デフォルトのNGワード、個別のNGワード、連鎖あぼーんを実行
			if (Twinie.Settings.NGWordsOn &&
				(defNGWords.IsMatch(res) || (nGWords != null && nGWords.IsMatch(res)) ||
				(abone.Chain && abonIndices.ContainsAny(res.RefIndices))))
			{
				res.Visible = abone.Visible;
				res.IsABone = true;

				// あぼーんされたリストにレス番号を追加
				abonIndices.Add(res.Index);

				// コレクション内のレスもあぼーん処理
				resCollection.ABone(res.Index, true);
			}
			else
			{
				res.IsABone = false;
			}

			return res;
		}

		///	<summary>
		///	textをブラウザに書き込む
		///	</summary>
		///	<param name="resSets"></param>
		private void WriteResInternal(ResSetCollection resSets)
		{
			if (resSets == null)
			{
				throw new ArgumentNullException("resSets");
			}

			try
			{
				// スキンでHTMLに変換
				string htmlText = skinBase.Convert(resSets);

				if (thumbnail.Visible)
				{
					htmlText = CreateThumbnailHtml(htmlText, resSets);
				}

				WriteText(htmlText);

				// 逆参照を色づけ
				if (threadSett.IsColoringBackReference)
				{
					ColoringReferencedResNumber(resSets);
				}
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
		}

		///	<summary>
		///	Popupメソッドを呼ぶ
		///	</summary>
		///	<param name="argument"></param>
		private void OnPopup(object sender, ObjectTimerEventArgs e)
		{
			Invoke(new PopupMethodInvoker(PopupInternal), new object[] { e.Tag });
		}

		///	<summary>
		///	argumentを使用してポップアップ
		///	</summary>
		///	<param name="argument"></param>
		private void PopupInternal(string argument)
		{
			lastPopupRef = String.Empty;

			// レスポップアップ
			if (IsResPopupable(argument))
			{
				ThreadHeader header = URLParser.ParseThread(argument);
				if (HeaderInfo.Equals(header))
				{
					int[] numbers = ResReference.GetArray(argument);
					lastPopupRef = argument;

					Popup(numbers);
				}
			}
			// メール欄ポップアップ
			else if (IsMailPopupable(argument))
			{
				int token = argument.IndexOf("mailto:");
				popInterf.Show(argument.Substring(token + 7));
			}
			// 画像ポップアップ
			else if (IsPicturePopupable(argument))
			{
				popInterf.ShowImage(argument);
			}

			if (popInterf is HtmlPopup)
				((HtmlPopup)popInterf).inPopup = true;
		}

		///	<summary>
		///	レスポップアップ可能な文字列かどうかを判断
		///	</summary>
		///	<param name="text"></param>
		///	<returns></returns>
		private bool IsResPopupable(string text)
		{
			return URLParser.IsThreadUrl(text);
		}

		///	<summary>
		///	メールポップアップ可能な文字列かどうかを判断
		///	</summary>
		///	<param name="text"></param>
		///	<returns></returns>
		private bool IsMailPopupable(string text)
		{
			// sage はポップアップしないようにしてみる
			return text.StartsWith("mailto:") && !text.Trim().ToLower().Equals("mailto:sage");
		}

		///	<summary>
		///	指定したURLがポップアップ可能かどうかを判断
		///	</summary>
		///	<param name="url"></param>
		private bool IsPicturePopupable(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if ((skinStyle.imagePopup & PopupState.Disable) != 0)
				return false;

			if ((skinStyle.imagePopup & PopupState.KeySwitch) != 0 && (ModifierKeys & Keys.Control) == 0)
				return false;

			return Regex.IsMatch(url, @"(\.jpg$)|(\.jpeg$)|(\.gif$)|(\.png$)|(\.bmp$)",
				RegexOptions.IgnoreCase);
		}

		///	<summary>
		///	ポップアップを消す
		///	</summary>
		private void HidePopup()
		{
			popInterf.Hide();
			clickedPopup = false;
			isExtracting = false;
			//		newPopup = false;
		}

		///	<summary>
		///	デフォルトイベント
		///	</summary>
		[System.Runtime.InteropServices.DispId(0)]
		public void DefaultMethod()
		{
			try
			{

				HTMLBody body = GetHtmlBody();
				IHTMLWindow2 window = (IHTMLWindow2)GetDocument().parentWindow;
				IHTMLElement src = window.@event.srcElement;

				// ポップアップを表示
				if (src.tagName == "A" &&
					src.innerText != null && src.innerText.Length > 0 &&
					Regex.IsMatch(src.innerText, ">>\\d+"))
				{
				}
				else if (clickedPopup && popInterf.Visible && !window.@event.type.Equals("mouseup"))
				{
				}
				// ポップアップを隠す
				else
				{
					HidePopup();
				}
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		///	<summary>
		///	ドキュメントのイベントを設定
		///	</summary>
		private void SetDocumentEvents()
		{
			HTMLDocument doc = GetDocument();

			
		//	DispHTMLDocument disp = doc;
		//	disp.onmouseover = this;
		//	disp.onmouseup = this;

			
		}

		///	<summary>
		///	AxWebBrowser.Documentプロパティを取得
		///	</summary>
		///	<returns></returns>
		public HTMLDocument GetDocument()
		{
			return document as HTMLDocument;
		}

		///	<summary>
		///	AxWebBrowser.Document.bodyプロパティを取得
		///	</summary>
		///	<returns></returns>
		public HTMLBody GetHtmlBody()
		{
			return (HTMLBody)GetDocument().body;
		}
		#endregion

		#region	Overrideメソッド
		///	<summary>
		///	あぼーんを検知
		///	</summary>
		protected override void OnABone()
		{
			MessageBox.Show(this, "あぼーんを検知した予感。ログを再取得してください。", headerInfo.Subject,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected override void OnPastlog(PastlogEventArgs e)
		{
			ThreadHeader header = null;

			if (!pastlogChecked && HeaderInfo.BoardInfo.Bbs.Equals(BbsType.X2ch) && Twinie.Settings.Authentication.AuthenticationOn)
			{
				OnStatusTextChanged(headerInfo.Subject + "はdat落ちしています");

				header = new X2chThreadHeader();
				header.Key = HeaderInfo.Key;
				header.BoardInfo = new BoardInfo(HeaderInfo.BoardInfo.Server, HeaderInfo.BoardInfo.Path, HeaderInfo.BoardInfo.Name);
				header.BoardInfo.Bbs = BbsType.X2chAuthenticate;
				header.Subject = HeaderInfo.Subject;

				pastlogChecked = true;
			}
			else if (!kakoChecked)
			{
				OnStatusTextChanged(headerInfo.Subject + ": 過去ログ倉庫を調べてみるテスト。");

				header = new X2chKakoThreadHeader();
				header.Key = HeaderInfo.Key;
				header.BoardInfo = new BoardInfo(HeaderInfo.BoardInfo.Server, HeaderInfo.BoardInfo.Path, HeaderInfo.BoardInfo.Name);
				header.BoardInfo.Bbs = BbsType.X2chKako;
				header.Subject = HeaderInfo.Subject;

				kakoChecked = true;
			}
			else
			{
				OnStatusTextChanged(headerInfo.Subject + "は読み込めませんでした。");
				return;
			}

			isOpen = false;
			canceled = true;
			thread = null;

			Open(header);
		}

		protected override void Opening()
		{
			// スキンをリセット
			skinBase.Reset();

			// 描画開始と終了位置を取得
			resBegin = Math.Max(1, headerInfo.GotResCount - viewResCount);
			resEnd = -1;

			// 参照回数をリセット
			resCollection.ResetBackReferenceCount();

			// レス表示制限しない場合は常に1から
			if (!Twinie.Settings.Thread.ViewResLimit)
				resBegin = 1;

			// しおりが設定されていればresBeginをしおり位置に設定
			if (headerInfo.Shiori > 0)
				resBegin = headerInfo.Shiori;

			isExtracting = false;
			popInterf.Hide();

			// ヘッダーを書き込む
			Clear();

			// イベントを設定
			SetDocumentEvents();

			// フォントサイズを設定
			FontSize = threadSett.FontSize;

			WriteText(skinBase.GetHeader(headerInfo));

			RunImageCacheClient();

			Application.DoEvents();

		}

		///	<summary>
		///	スクロール位置を設定
		///	</summary>
		///	<param name="value"></param>
		protected override void SetScrollPosition(float value)
		{
			try
			{
				float pos = value;

				if (popInterf.Visible)
					return;

				if (pos < 0)
				{
					HTMLBody body = GetHtmlBody();
					ScrollInternal(body.scrollHeight);
				}
				else if (pos == 0)
				{
					ScrollInternal(0);
				}
				else
				{
					string[] values = Convert.ToString(pos).Split('.');
					int index = Int32.Parse(values[0]);
					int sub = (values.Length == 2) ? Int32.Parse(values[1]) : 0;

					HTMLDocument doc = GetDocument();
					IHTMLElement element = doc.getElementById(index.ToString());

					if (element != null)
					{
						//element.scrollIntoView(true);
						ScrollInternal(element.offsetTop - sub);
					}
				}
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
		}

		private void ScrollInternal(int scroll)
		{
			HTMLBody body = GetHtmlBody();
			body.scrollTop = scroll;
		}

		///	<summary>
		///	resSetsをブラウザの表示形式に変換後、書き込みをしていく。
		///	</summary>
		///	<param name="resSets"></param>
		protected override void Write(ResSetCollection resSets)
		{
			try
			{
				if (IsPackageReception)
					Cursor = Cursors.WaitCursor;

				ResSetCollection buffer = new ResSetCollection();
				bool shiori = (headerInfo.Shiori > 0); // しおりが設定されているかどうか

				for (int i = 0; i < resSets.Count; i++)
				{
					ResSet res = resSets[i];

					((Twin2IeHtmlSkin)skinBase).IncrementIDCount(res.ID);

					// しおりフラグを設定
					if (shiori)
						res.Bookmark = (res.Index == headerInfo.Shiori);

					// スレッドを開く際に表示インデックスが指定されている場合
					if (indicesValues.Count > 0)
					{
						res.Visible = indicesValues.Contains(res.Index);
					}
					else
					{
						// resBeginからresEndまでの間を表示	(1は常に表示)
						res.Visible = (res.Index >= resBegin) &&
									(res.Index <= resEnd || resEnd == -1) || (res.Index == 1);
					}

					if (res.Visible)
						res = CheckNGWords(res);

					buffer.Add(res);
				}

				WriteResInternal(buffer);
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
			finally
			{
				Cursor = Cursors.Default;
				//				Application.DoEvents();
			}
		}

		///	<summary>
		///	書き込み開始
		///	</summary>
		protected override void WriteBegin()
		{
			enableAutoScroll = false;

			if (autoScroll)
			{
				// 一番下にスクロールされていればオートスクロールを有効にする
				HTMLBody body = GetHtmlBody();
				enableAutoScroll = (body.scrollTop + body.clientHeight) == body.scrollHeight;
			}

			// フッターを削除
			RemoveFooter();

		}

		///	<summary>
		///	書き込みを完了しフッターを付加
		///	</summary>
		protected override void WriteEnd()
		{
			// 新着レスがある場合のみにフッターを付加
			if (headerInfo.NewResCount > 0)
				WriteText(skinBase.GetFooter(headerInfo));

			// これしないと書き込みが終わらないうちにスクロールしてしまう為、
			// 末尾までスクロールできない。
			Application.DoEvents();

			// 新着までスクロール
			if (scrollToNewRes)
				ScrollTo(headerInfo.GotResCount - headerInfo.NewResCount + 1);

			// オートスクロールを実行
			else if (enableAutoScroll)
				ScrollTo(ScrollPosition.Bottom);

			// 逆参照のカラーリングを実行
			if (threadSett.IsColoringBackReference)
			{
				FlushColoringBackReference();
			}

			FlushCreateThumbnailStack();
		}
		#endregion

		///	<summary>
		///	スレッドを開く
		///	</summary>
		///	<param name="header"></param>
		public override void Open(ThreadHeader header)
		{
			base.Open(header);

			// スキンの基本URLを設定
			skinBase.BaseUri = header.Url;

			// NGワードを取得
			nGWords = Twinie.NGWords.Get(header.BoardInfo, false);

			// 一日経過しているNGワードを削除
			if (nGWords != null &&
				Twinie.Settings.NGIDAutoClear &&
				Twinie.Settings.LastExitDay != DateTime.Now.Day)
			{
				if (!clearedIdList.Contains(header.BoardInfo.DomainPath))
				{
					nGWords.ID.Clear();
					Twinie.NGWords.Save(header.BoardInfo);

					clearedIdList.Add(header.BoardInfo.DomainPath);
				}
			}

			// オートリロードの対象に設定
			if (autoReload)
				autoRefreshTimers.Add(this);
		}

		///	<summary>
		///	スレッドを閉じる
		///	</summary>
		public override void Close()
		{
			if (IsOpen)
			{
				kakoChecked = false;
				pastlogChecked = false;

				// スクロール位置を保存
				headerInfo.Position = GetScrollPosition();
				ThreadIndexer.SavePosition(Cache, headerInfo);

				popInterf.Hide();
			}

			CloseImageCacheClient();

			// あぼーん済みリストをクリア
			abonIndices.Clear();
			// スクロール履歴をクリア
			scrolled.Clear();

			// オートリロードの対象から外す
			autoRefreshTimers.Remove(this);

			nGWords = null;

			base.Close();
		}

		///	<summary>
		///	指定した1から始まるレス番号の配列をポップアップ表示
		///	</summary>
		///	<param name="indices"></param>
		public override void Popup(int[] indices)
		{
			if (indices == null)
				throw new ArgumentNullException("indices");

			if (indices.Length == 0)
				return;

			ResSetCollection resSets = resCollection.GetRange(indices);
			resSets.Visible = true;

			// Popup
			popInterf.Show(resSets);
		}

		private void ClickedPopup(int[] indices)
		{
			Popup(indices);
			if (popInterf is HtmlPopup)
				((HtmlPopup)popInterf).inPopup = true;
			clickedPopup = true;
			//			newPopup = true;
		}

		///	<summary>
		///	指定したレスコレクションをポップアップで表示
		///	</summary>
		///	<param name="resSets"></param>
		public override void Popup(ResSetCollection resSets)
		{
			if (resSets == null)
				throw new ArgumentNullException("resSets");

			if (resSets.Count == 0)
				return;

			popInterf.Show(resSets);
		}

		///	<summary>
		///	スレッドを印刷
		///	</summary>
		public override void Print()
		{
			object o = null;

			webBrowser.ExecWB(OLECMDID.OLECMDID_PRINT,
				OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
		}

		///	<summary>
		///	継承先でオーバーライドされれば、指定した位置にしおりを挟む。
		///	既に同じ番号にしおりが設定されていたら、しおりを解除。
		///	</summary>
		///	<param name="shiori">しおりを挟む1から始まるレス番号</param>
		public override void Bookmark(int shiori)
		{
			// 同じ番号にしおりが設定されていたら、しおりを外す
			headerInfo.Shiori = (headerInfo.Shiori == shiori) ? 0 : shiori;
			ThreadIndexer.SaveBookmark(Cache, headerInfo);
			Redraw();
		}

		/// <summary>
		/// 指定した sirusi 番号のレスに印を付ける。
		/// 同じ番号に印すると、印が解除される。
		/// </summary>
		/// <param name="sirusi"></param>
		public override void Sirusi(int sirusi)
		{
			if (headerInfo.Sirusi.Contains(sirusi))
			{
				headerInfo.Sirusi.Remove(sirusi);
			}
			else
			{
				headerInfo.Sirusi.Add(sirusi);
			}
			ThreadIndexer.SaveSirusi(Cache, headerInfo);
		}

		///	<summary>
		///	しおりを開く
		///	</summary>
		public override void OpenBookmark()
		{
			if (headerInfo.Shiori != -1)
			{
				resBegin = headerInfo.Shiori;
				resEnd = viewResCount;

				Redraw();
			}
		}

		/// <summary>
		/// 印されたレスを表示
		/// </summary>
		public override void OpenSirusi()
		{
			if (headerInfo.Sirusi.Count > 0)
			{
				ResSetCollection newResSets = new ResSetCollection();

				foreach (ResSet res in resCollection)
				{
					if (headerInfo.Sirusi.Contains(res.Index))
						newResSets.Add(res);
				}

				WriteResColl(newResSets);
			}
			else
				MessageBox.Show("しるしが存在しません");
		}

		///	<summary>
		///	指定した位置にスクロール
		///	</summary>
		///	<param name="position"></param>
		public override void ScrollTo(ScrollPosition position)
		{
			switch (position)
			{
				case ScrollPosition.Top:
					SetScrollPosition(0);
					break;

				case ScrollPosition.Bottom:
					SetScrollPosition(-1);
					break;

				case ScrollPosition.Prev:
					if (scrolled.Count > 0)
						SetScrollPosition(scrolled.Pop());
					break;
			}
		}

		///	<summary>
		///	指定したレス番号までスクロール
		///	</summary>
		///	<param name="resNumber"></param>
		public override void ScrollTo(int resNumber)
		{
			if (resNumber < resBegin)
				Range(resNumber, resEnd);

			else if (resEnd > 0 && resNumber > resEnd)
				Range(resBegin, resNumber);

			SetScrollPosition(resNumber);
		}

		///	<summary>
		///	指定した範囲のレスを表示
		///	</summary>
		///	<param name="begin"></param>
		///	<param name="end"></param>
		public override void Range(int begin, int end)
		{
			if (IsReading)
				throw new ApplicationException("スレを読み込み中です");

			if (begin < 1)
				begin = 1;
			if (end > resCollection.Count)
				end = -1;

			Opening();
			resBegin = begin;
			resEnd = end;

			bool tempValue = scrollToNewRes;
			scrollToNewRes = false;

			WriteBegin();
			Write(resCollection);
			WriteEnd();

			scrollToNewRes = tempValue;
		}

		///	<summary>
		///	指定した範囲に移動
		///	</summary>
		///	<param name="movement"></param>
		public override void Range(RangeMovement movement)
		{
			int begin;
			switch (movement)
			{
				case RangeMovement.Back:
					begin = resBegin - viewResCount;
					break;
				case RangeMovement.Forward:
					begin = resBegin + viewResCount;
					break;
				default:
					return;
			}
			Range(begin, begin + viewResCount);
		}

		///	<summary>
		///	表示を一端クリアしてitemsを書き込む
		///	</summary>
		///	<param name="items"></param>
		public override void WriteResColl(ResSetCollection items)
		{
			if (IsReading)
				throw new ApplicationException("スレを読み込み中です");

			Opening();

			resBegin = items.Count > 0 ? items[0].Index : 0;
			resEnd = items.Count > 0 ? items[items.Count - 1].Index : 0;

			bool tempValue = scrollToNewRes;
			scrollToNewRes = false;

			WriteBegin();
			Write(items);
			WriteEnd();

			scrollToNewRes = tempValue;
		}

		///	<summary>
		///	textをブラウザに書き込む
		///	</summary>
		///	<param name="text"></param>
		public override void WriteText(string text)
		{
			IHTMLDocument2 doc = (IHTMLDocument2)GetDocument();
			doc.write(text);
		}

		///	<summary>
		///	表示をクリア
		///	</summary>
		public override void Clear()
		{
			IHTMLDocument2 doc = (IHTMLDocument2)GetDocument();
			doc.body.innerHTML = String.Empty;
		}

		///	<summary>
		///	スレッドを開き指定したキーワードを含むレスのみを表示
		///	</summary>
		///	<param name="header"></param>
		///	<param name="info"></param>
		public override int OpenExtract(ThreadHeader header, ThreadExtractInfo info)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			Close();

			// 検索クラスを初期化
			BmSearch2 bmSearch = new BmSearch2(info.Keyword);
			ResSetCollection resSets = new ResSetCollection();
			ResSetCollection matches = new ResSetCollection();

			ThreadIndexer.Read(Cache, header);
			headerInfo = header;

			// レスを読み込む
			ThreadStorage storage = new LocalThreadStorage(Cache);
			storage.Open(header, StorageMode.Read);

			while (storage.Read(resSets) != 0)
				;
			storage.Close();

			// レスを検索し一致しているレスのみをmatchesに入れる
			foreach (ResSet res in resSets)
			{
				string str = res.ToString(ResSetElement.All);
				if (bmSearch.Search(str) >= 0)
					matches.Add(res);
			}

			// 一致したレスのみを表示
			resCollection.AddRange(resSets);
			Opening();

			bool temp = scrollToNewRes;
			scrollToNewRes = false;

			WriteBegin();
			WriteText(skinBase.Convert(matches));
			WriteEnd();

			scrollToNewRes = temp;

			OnStatusTextChanged("抽出キーワード: " + info.Keyword);
			return matches.Count;
		}

		///	<summary>
		///	スレッド内を検索するためのクラスを初期化
		///	</summary>
		///	<param name="keyword">検索する単語</param>
		///	<param name="flags">検索オプション</param>
		///	<returns></returns>
		public override AbstractSearcher BeginSearch()
		{
			HTMLDocument document = GetDocument();
			IEComSearcher r = new IEComSearcher(document);
			return r;
		}

		///	<summary>
		///	レスを抽出するためのクラスを初期化
		///	</summary>
		///	<param name="keyword"></param>
		///	<param name="options"></param>
		///	<param name="modePopup"></param>
		public override AbstractExtractor BeginExtract()
		{
			IEComExtractor r = new IEComExtractor(this);
			return r;
		}

		///	<summary>
		///	スレッドにフォーカスを移動
		///	</summary>
		public override void _Select()
		{
			HTMLDocument doc = GetDocument();
			IHTMLWindow2 win = doc.parentWindow;
			win.focus();
		}

		internal string CreateThumbnailHtml(string html, ResSetCollection resItems)
		{
			StringBuilder sb = new StringBuilder(html);
			int start = html.Length - 1;

			for (int i = resItems.Count - 1; i >= 0; i--)
			{
				string[] links = resItems[i].Links["jpg|jpeg|gif|png|bmp|jpg.html"];

				for (int n = links.Length - 1; n >= 0; n--)
				{
					string url = links[n];

					// 画像へのリンクを検索
					int index = html.LastIndexOf("<a href=\"" + url, start);

					if (index < 0)
						break;

					// 画像リンクの先頭にタグを挿入
					string tag = CreateThumbnailLink(url);
					sb.Insert(index, tag);
					start = index;

				}

				if (start <= 0)
					break;
			}

			return sb.ToString();
		}

		/// <summary>
		/// 指定した画像urlのサムネイルタグを作成
		/// </summary>
		/// <param name="urls"></param>
		/// <returns></returns>
		protected string CreateThumbnailLink(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			StringBuilder sb = new StringBuilder();

			if (thumbnail.IsLightMode)
			{
				try
				{
					string referer, srcUrl = url;
					imageViewUrlTool.Replace(ref url, out referer);

					sb.AppendFormat(
						"<a href=\"{0}\" target=\"_blank\"><img name=\"{1}\" height=\"{2}\" border=1></a>",
						srcUrl, url.GetHashCode(), thumbnail.Height);

					createThumbImageUrls.Push(
						new _StringPair(url, referer));
				}
				catch (ArgumentException)
				{
				}
			}
			else
			{
				string referer, srcUrl = url;
				imageViewUrlTool.Replace(ref url, out referer);

				sb.AppendFormat("<a href=\"{0}\" target=\"_blank\">" +
								"<img src=\"{1}\" height=\"{2}\" border=1></a> ", srcUrl, url, thumbnail.Height);
			}

			return sb.ToString();
		}

		internal void FlushCreateThumbnailStack()
		{
			if (thumbnail.Visible)
			{
				foreach (_StringPair kv in createThumbImageUrls.ToArray())
				{
					imageCacheClient.AddDownloadList(kv.Key, kv.Value);
				}
			}
			createThumbImageUrls.Clear();
		}

		public void CloseImageCacheClient()
		{
			imageCacheClient.Close();
			iccEvent.Reset();
		}

		public void RunImageCacheClient()
		{
			iccEvent.Set();
			imageCacheClient.ClearList();
			imageCacheClient.Run();
		}

		private Queue<KeyValuePair<int, IHTMLElement>> numberColoringQueue =
			new Queue<KeyValuePair<int, IHTMLElement>>();

		/// <summary>
		/// newItems が参照しているレスの番号を色づけしていく
		/// </summary>
		/// <param name="newItems"></param>
		private void ColoringReferencedResNumber(ResSetCollection newItems)
		{
			int tick = Environment.TickCount;

			IHTMLElementCollection tags = GetDocument().getElementsByTagName("A");

			if (tags.length == 0)
				return;

			List<int> enQueuedIndices = new List<int>();

			foreach (ResSet res in newItems)
			{
				// 参照が50個以上のレスはカウントしない
				int[] refIndicesArray = res.RefIndices;

				if (refIndicesArray.Length >= 50)
					continue;

				foreach (int refIndex in refIndicesArray)
				{
					if (1 <= refIndex && refIndex <= resCollection.Count)
					{
						ResSet temp = resCollection[refIndex - 1];
						temp.BackReferencedCount++;

						resCollection[refIndex - 1] = temp;

						if (!enQueuedIndices.Contains(refIndex))
						{
							IHTMLElement element = (IHTMLElement)tags.item(refIndex.ToString(), 0);

							if (element != null)
							{
								numberColoringQueue.Enqueue(
									new KeyValuePair<int, IHTMLElement>(refIndex, element));

								enQueuedIndices.Add(refIndex);
							}
						}
					}
				}

			}

			Console.WriteLine("ColoringReferencedResNumber#{0}ms",
				Environment.TickCount - tick);

		}

		/// <summary>
		/// items の中で、逆参照されているレス (IsBackReferenced=true) の番号を色づけする
		/// </summary>
		/// <param name="items"></param>
		internal void ColoringBackReference(ResSetCollection items)
		{
			int tick = Environment.TickCount;

			IHTMLElementCollection tags = GetDocument().getElementsByTagName("A");

			if (tags.length == 0)
				return;

			List<int> coloringResIndices = new List<int>();

			foreach (ResSet res in items)
			{
				if (res.IsBackReferenced)
				{
					IHTMLElement element = (IHTMLElement)tags.item(res.Index.ToString(), 0);

					if (element != null)
						ColoringHtmlElement(res, element);
				}
			}

			Console.WriteLine("ColoringBackReference#{0}ms",
				Environment.TickCount - tick);

		}

		/// <summary>
		/// 指定したインデックスを参照しているレスをポップアップします。
		/// </summary>
		/// <param name="index"></param>
		public override void PopupBackReferences(int index)
		{
			List<int> indices = new List<int>();

			foreach (ResSet res in resCollection)
			{
				if (res.RefIndices.Length >= 50)
					continue;

				foreach (int n in res.RefIndices)
				{
					if (n == index && !indices.Contains(res.Index))
					{
						indices.Add(res.Index);
					}
				}
			}

			Popup(indices.ToArray());
		}

		private void FlushColoringBackReference()
		{
			foreach (KeyValuePair<int, IHTMLElement> kv in numberColoringQueue)
			{
				if (kv.Key >= 0 && (kv.Key - 1) < resCollection.Count)
				{
					ColoringHtmlElement(resCollection[kv.Key - 1], kv.Value);
				}
			}
		}

		private void ColoringHtmlElement(ResSet res, IHTMLElement element)
		{
			if (element.style.color == null)
			{
				element.style.color = "red";

				IHTMLElement target = GetDocument().createElement("A");
				target.setAttribute("href", String.Format("method:BackReferences(${0})", res.Index), 0);
				target.setAttribute("target", "_blank", 0);

				target.style.textDecorationNone = true;
				target.style.color = "blue";
				target.style.fontWeight = "normal";
				target.innerText = threadSett.BackReferenceChar;

				IHTMLElement2 e2 = (IHTMLElement2)element;
				e2.insertAdjacentElement("afterEnd", target);
			}
		}

	}

}
