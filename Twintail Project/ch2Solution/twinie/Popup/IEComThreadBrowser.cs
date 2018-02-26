// ThreadBrowser.cs

namespace Twin.Forms
{
	using System;
	using System.Windows.Forms;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Collections;
	using System.Text.RegularExpressions;
	using mshtml;
	using AxSHDocVw;
	using SHDocVw;

	using Twin.Bbs;
	using Twin.Tools;
	using Twin.Text;
	using Twin.Util;
	using Twin.IO;
	using PopupTest;

	/// <summary>
	/// IEコンポーネントを使用しスレッドを表示するコントロール
	/// </summary>
	public sealed class IEComThreadBrowser : ThreadControl, IExternalMethod, IDocHostUIHandler
	{
		// デリゲートの定義
		private delegate void PopupMethodInvoker(string argument);

		// オートリロードするためのクラス
		private readonly static AutoRefreshTimerBase autoRefreshTimers;

//		// キャッシュするクラス
//		private readonly static WebCacheClient webCacheClient;

		// 外部から関数を起動するためのクラス
		private readonly IEComMethodInvoker methodInvoker;

		// NGリンク
		public readonly static LinkInfoCollection linkCollection;
		private static readonly NGWords defNGWords;
		private NGWords nGWords;

		private AxWebBrowser webBrowser;			// スレッドを表示するコントロール
		private IPopupBase popInterf;				// ポップアップを制御するインターフェース
		private PopupSettings popSett;				// ポップアップの設定情報
		private SkinStyle skinStyle;				// スキンの表示情報を管理するクラス
		private ThreadSkinBase skinBase;			// スキンの変換処理を行うクラス
		private ObjectTimer popTimer;				// レス参照をポインタしてからポップアップするまでの間隔を管理するタイマー
		private ABone abone;						// あぼーん情報
		private SortedValueCollection abonIndices;	// あぼーん済みレス番号をコレクション管理 (連鎖あぼーんに使用する)
		private Stack scrolled;						// スクロール状態を記憶するコレクション

		private string tempStatusText;				// AxWebBrowser.StatusTextChangeイベントが発生するたびに引数の"e.text"がコピーされる変数
		private bool enableAutoScroll;				// 書き込み完了時にオートスクロールを実行するかどうか
		private int resBegin, resEnd;				// 描画始めレス番号と描画終了レス番号を表す
		private int viewResCount;					// レスの表示制限数を表す

		private bool scrollToNewRes;				// 新着までスクロールするかどうかを示す値
		private bool autoReload;					// オートリロードを行うかどうか
		private bool autoScroll;					// オートスクロールを行うかどうか

		internal string lastPopupRef;				// 最後にポップアップしたレス番号が格納される (同じレスが２重ポップアップされてしまうのを防ぐため)
		private bool disposed = false;				// Disposeメソッドが呼ばれたかどうかを表す

		private bool clickedPopup;

		/// <summary>
		/// 自動更新タイマークラスを取得
		/// </summary>
		public static AutoRefreshTimerBase AutoRefreshTimers {
			get {
				return autoRefreshTimers;
			}
		}

		/// <summary>
		/// 選択されている文字列を取得
		/// </summary>
		public override string SelectedText {
			get {
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
		/// オートスクロールの有効・無効
		/// </summary>
		public override bool AutoScroll {
			set {
				if (autoScroll != value)
					autoScroll = value;
			}
			get { return autoScroll; }
		}

		/// <summary>
		/// オートスリロードの有効・無効
		/// </summary>
		public override bool AutoReload {
			set {
				if (autoReload != value)
				{
					autoReload = value;
					if (value)	autoRefreshTimers.Add(this);
					else		autoRefreshTimers.Remove(this);
				}
			}
			get { return autoReload; }
		}

		/// <summary>
		/// 表示レス数を取得または設定
		/// </summary>
		public override int ViewResCount {
			set {
				if (value < 1)
					throw new ArgumentOutOfRangeException("ViewResCount");
				viewResCount = value;
				Range(headerInfo.GotResCount - value, -1);
			}
			get { return viewResCount; }
		}

//		/// <summary>
//		/// あぼーん情報を取得
//		/// </summary>
//		public ABone ABone {
//			get {
//				return abone;
//			}
//		}
//
//		/// <summary>
//		/// 表示に使用するスキンを取得
//		/// </summary>
//		public ThreadSkinBase Skin {
//			get { return skinBase; }
//		}

		/// <summary>
		/// IEComThreadBrowser
		/// </summary>
		static IEComThreadBrowser()
		{
			// オートリロードタイマー
			autoRefreshTimers = new AutoRefreshTimerCollection2();
			// デフォルトのNGワード設定
			defNGWords = Twinie.NGWords.Default;

			try {// NGアドレスを読み込む
				linkCollection = new LinkInfoCollection(Settings.NGAddrsPath);
			} catch (Exception ex) {
				TwinDll.ShowOutput(ex);
			}

			// キャッシュクラス
//			webCacheClient = new WebCacheClient(Settings.WebCacheFolderPath);
//			webCacheClient.Filters.Add("2ch.net/");
//			webCacheClient.Filters.Add("ogrish");
//			webCacheClient.Filters.Add("rotten");

//			foreach (LinkInfo link in linkCollection)
//				webCacheClient.Filters.Add(link.Uri);
		}

		/// <summary>
		/// IEComThreadBrowserクラスのインスタンスを初期化
		/// </summary>
		public IEComThreadBrowser(Cache cache, Settings sett)
			: base(cache)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//

			TabStop = false;

			#region Initialize AxWebBrowser
			webBrowser = CreateWebBrowser(this);
			webBrowser.NewWindow2 += new AxSHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(OnNewWindow2);
			webBrowser.StatusTextChange += new AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler(OnStatusTextChange);
			OpenBlank();

			while (webBrowser.Document == null) {
				Application.DoEvents();
				System.Threading.Thread.Sleep(50);
			}
			
			object ocx = webBrowser.GetOcx();
			SHDocVw.WebBrowser_V1 wb = ocx as SHDocVw.WebBrowser_V1;
			if (wb != null)
				wb.BeforeNavigate += new SHDocVw.DWebBrowserEvents_BeforeNavigateEventHandler(OnBeforeNavigate);
			InstallIDocHostUIHandler((ICustomDoc)webBrowser.Document);
			#endregion

			tempStatusText =
				lastPopupRef = String.Empty;

			scrolled = new Stack();

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
			Thumbnail thumb = sett.Thumbnail;
			thumb.Visible = skinStyle.thumb;
			skin.Thumbnail = thumb;

			// ポップアップクラスを初期化
			if (skinStyle.style == PopupStyle.Text) popInterf = new SimplePopup(Twinie.Form);
			else if (skinStyle.style == PopupStyle.Html) popInterf = new HtmlPopup(this, skinBase);

			popInterf.ImageSize = popSett.ImagePopupSize;
			popInterf.Position = popSett.Position;
			popInterf.Maximum = popSett.Maximum;
			popInterf.BackColor = skinStyle.backColor;
			popInterf.ForeColor = skinStyle.foreColor;
			popInterf.Font = skinStyle.font;
			
			// そのほかの設定
			ThreadSettings thread = sett.Thread;
			viewResCount = thread.ViewResCount;
			scrollToNewRes = thread.ScrollToNewRes;
			autoScroll = thread.AutoScrollOn;
			autoReload = thread.AutoReloadOn;
			bufferSize = sett.Net.BufferSize;
			UseGzip = sett.UseGzipArchive;
			IsPackageReception = sett.Net.PackageReception;
			Proxy = sett.Net.RecvProxy;
		}

		/// <summary>
		/// 使用しているリソースを解放
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				try {
					autoRefreshTimers.Remove(this);
					webBrowser.Dispose();
					webBrowser = null;
				}
				finally {
					base.Dispose(disposing);
				}
			}
			disposed = true;
		}

		/// <summary>
		/// AxWebBrowserを初期化
		/// </summary>
		/// <param name="owner">親コントロール (nullを指定可能)</param>
		/// <returns>初期化されたAxWebBrowserのインスタンス</returns>
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

		#region WebBrowser Events
		private void OnBeforeNavigate(string uRL,
			int Flags, string TargetFrameName, ref object PostData, string Headers, ref bool Processed)
		{
			Processed = true;
//			OnUriClick(new UriClickEventArgs(uRL, info));
		}

		private void OnStatusTextChange(object sender,
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

		private void OnNewWindow2(object sender,
			AxSHDocVw.DWebBrowserEvents2_NewWindow2Event e)
		{
			try {
				e.cancel = true;

				// レス番号がクリックされた
				if (tempStatusText.StartsWith("menu:"))
				{
					int index = Int32.Parse(tempStatusText.Substring(5));
					ResSet resSet = resCollection[index-1];

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

							// クリックされたレス番が、表示されていなければ再レンダリング
							if (num[0] < resBegin)
								Range(num[0], num[0] + viewResCount);

							HidePopup();
							SetScrollPosition(num[0]);
						}
						return;
					}
				}
				
				LinkInfo info = linkCollection.IndexOf(tempStatusText);
				OnUriClick(new UriClickEventArgs(tempStatusText, info));
			}
			catch (Exception ex) {
				TwinDll.ShowOutput(ex);
			}
		}
		#endregion

		#region IDocHostUIHandler Implementation
		/// <summary>
		/// Called by MSHTML to display a shortcut menu.
		/// </summary>
		/// <param name="dwID"></param>
		/// <param name="ppt"></param>
		/// <param name="pcmdtReserved"></param>
		/// <param name="pdispReserved"></param>
		/// <returns></returns>
		HRESULT IDocHostUIHandler.ShowContextMenu(uint dwID, ref tagPOINT ppt, object pcmdtReserved, object pdispReserved)
		{
			HRESULT hr = HRESULT.S_FALSE;
			if (popInterf.Visible)
			{
				hr = HRESULT.S_OK;
			}
			return hr;
		}

		/// <summary>
		/// Called by MSHTML to retrieve the user interface (UI) capabilities
		/// of the application that is hosting MSHTML.
		/// </summary>
		/// <param name="pInfo"></param>
		HRESULT IDocHostUIHandler.GetHostInfo(ref DOCHOSTUIINFO pInfo)
		{
			return HRESULT.S_OK;
		}

		/// <summary>
		/// Called by MSHTML to enable the host to replace MSHTML menus and toolbars.
		/// </summary>
		/// <param name="dwID"></param>
		/// <param name="pActiveObject"></param>
		/// <param name="pCommandTarget"></param>
		/// <param name="pFrame"></param>
		/// <param name="pDoc"></param>
		/// <returns>
		/// S_OK: Host displayed its own UI. MSHTML will not display its UI.
		/// S_FALSE: Host did not display its own UI. MSHTML will display its UI.
		/// DOCHOST_E_UNKNOWN:	Host did not recognize the UI identifier.
		///						MSHTML will either try an alternative identifier
		///						for compatibility with a previous version or display its own UI. 
		/// </returns>
		HRESULT IDocHostUIHandler.ShowUI(uint dwID, IntPtr pActiveObject, IntPtr pCommandTarget, IntPtr pFrame, IntPtr pDoc)
		{
			return HRESULT.S_FALSE;
		}

		/// <summary>
		/// Called when MSHTML removes its menus and toolbars.
		/// </summary>
		HRESULT IDocHostUIHandler.HideUI()
		{
			return HRESULT.S_OK;
		}

		/// <summary>
		/// Called by MSHTML to notify the host that the command state has changed.
		/// </summary>
		HRESULT IDocHostUIHandler.UpdateUI()
		{
			return HRESULT.S_OK;
		}

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::EnableModeless.
		/// Also called when MSHTML displays a modal UI.
		/// </summary>
		/// <param name="fEnable"></param>
		HRESULT IDocHostUIHandler.EnableModeless(int fEnable)
		{
			return HRESULT.S_OK;
		}

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::OnDocWindowActivate.
		/// </summary>
		/// <param name="fActivate"></param>
		HRESULT IDocHostUIHandler.OnDocWindowActivate(int fActivate)
		{
			return HRESULT.E_NOTIMPL;
		}

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::OnFrameWindowActivate.
		/// </summary>
		/// <param name="fActivate"></param>
		HRESULT IDocHostUIHandler.OnFrameWindowActivate(int fActivate)
		{
			return HRESULT.E_NOTIMPL;
		}

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::ResizeBorder.
		/// </summary>
		/// <param name="prcBorder"></param>
		/// <param name="pUIWindow"></param>
		/// <param name="fRameWindow"></param>
		HRESULT IDocHostUIHandler.ResizeBorder(ref tagRECT prcBorder, IntPtr pUIWindow, int fRameWindow)
		{
			return HRESULT.E_NOTIMPL;
		}

		/// <summary>
		/// Called by MSHTML when IOleInPlaceActiveObject::TranslateAccelerator
		/// or IOleControlSite::TranslateAccelerator is called.
		/// </summary>
		/// <param name="lpmsg"></param>
		/// <param name="pguidCmdGroup"></param>
		/// <param name="nCmdID"></param>
		/// <returns>
		/// Returns S_OK if successful, or an error value otherwise.
		/// </returns>
		HRESULT IDocHostUIHandler.TranslateAccelerator(ref tagMSG lpmsg, ref Guid pguidCmdGroup, uint nCmdID)
		{
			return HRESULT.S_FALSE;
		}

		/// <summary>
		/// Called by the WebBrowser Control to retrieve a registry subkey path
		/// that overrides the default Microsoftｮ Internet Explorer registry settings.
		/// </summary>
		/// <param name="pchKey"></param>
		/// <param name="dw"></param>
		HRESULT IDocHostUIHandler.GetOptionKeyPath(out string pchKey, uint dw)
		{
			pchKey = null;
			return HRESULT.S_OK;
		}

		/// <summary>
		/// Called by MSHTML when it is used as a drop target. This method enables
		/// the host to supply an alternative IDropTarget interface.
		/// </summary>
		/// <param name="pDropTarget">
		/// [in] Pointer to an IDropTarget interface for the current drop target
		/// object supplied by MSHTML.
		/// </param>
		/// <param name="ppDropTarget">
		/// [out] Address of a pointer variable that receives an IDropTarget
		/// interface pointer for the alternative drop target object supplied by
		/// the host.
		/// </param>
		/// <returns>
		/// Returns S_OK if successful, or an error value otherwise.
		/// </returns>
		HRESULT IDocHostUIHandler.GetDropTarget(IntPtr pDropTarget, out IntPtr ppDropTarget)
		{
			ppDropTarget = IntPtr.Zero;
			return HRESULT.E_NOTIMPL;
		}

		/// <summary>
		/// Called by MSHTML to obtain the host's IDispatch interface.
		/// </summary>
		/// <param name="ppDispatch">
		/// out] Address of a pointer to a variable that receives an IDispatch
		/// interface pointer for the host application.
		/// </param>
		/// <returns>
		/// Returns S_OK if successful, or an error value otherwise. 
		/// </returns>
		HRESULT IDocHostUIHandler.GetExternal(out object ppDispatch)
		{
			ppDispatch = IntPtr.Zero;
			return HRESULT.E_NOTIMPL;
		}

		/// <summary>
		/// Called by MSHTML to give the host an opportunity to modify the URL to be loaded.
		/// </summary>
		/// <param name="dwTranslate"></param>
		/// <param name="pchURLIn"></param>
		/// <param name="ppchURLOut"></param>
		/// <returns>
		/// Returns S_OK if the URL was translated, or S_FALSE if the URL was not translated.
		/// </returns>
		HRESULT IDocHostUIHandler.TranslateUrl(uint dwTranslate, ref ushort pchURLIn, IntPtr ppchURLOut)
		{
			return HRESULT.E_NOTIMPL;
		}

		/// <summary>
		/// Called by MSHTML to allow the host to replace the MSHTML data object.
		/// </summary>
		/// <param name="pDO"></param>
		/// <param name="ppDORet"></param>
		/// <returns>
		/// Returns S_OK if the data object is replaced, or S_FALSE if it's not replaced.
		/// </returns>
		HRESULT IDocHostUIHandler.FilterDataObject(IntPtr pDO, out IntPtr ppDORet)
		{
			ppDORet = IntPtr.Zero;
			return HRESULT.E_NOTIMPL;
		}

		/// <summary>
		/// Install IDocHostUIHandler
		/// </summary>
		private void InstallIDocHostUIHandler(ICustomDoc customDoc)
		{
			if (customDoc != null)
			{
				try
				{
					customDoc.SetUIHandler((IDocHostUIHandler)this);
				}
				catch (Exception) {}
			}
		}
		#endregion

		#region 外部呼び出し可能なメソッド
		void IExternalMethod.SetLimit(int limit)
		{
			ViewResCount = limit;
		}

		void IExternalMethod.Extract(int obj, string key)
		{
			ResSetElement element = (ResSetElement)obj;
			AbstractExtractor extractor = BeginExtract();
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

		#region Privateメソッド
		/// <summary>
		/// スクロール位置を取得
		/// </summary>
		/// <returns></returns>
		private float GetScrollPosition()
		{
			// 現在一番上に表示されているレス番号を取得
			try {
				HTMLBody body = (HTMLBody)GetHtmlBody();
				IHTMLElementCollection collect = (IHTMLElementCollection)body.getElementsByTagName("indices");
				IHTMLElement element = null;

				int scrollTop = body.scrollTop;
				int left = 0, right = collect.length-1;

				// 二分探索
				while (left < right)
				{
					int mid = (left + right) / 2;
					element = (IHTMLElement)collect.item(mid, mid);
					
					if (scrollTop < element.offsetTop)
						right = mid - 1;
					else if (scrollTop > element.offsetTop)
						left = mid + 1;
					else {
						left = mid;
						break;
					}
				}

				int index=0,sub=0;

				if (element != null)
				{
					if (scrollTop > element.offsetTop && ++left < collect.length)
						element = (IHTMLElement)collect.item(left, left);

					index = Int32.Parse(element.id);
					sub = Math.Abs(element.offsetTop - scrollTop);
				}
				return Single.Parse(index + "." + sub);
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}
			return 0.0f;
		}

		/// <summary>
		/// 再描画
		/// </summary>
		private void Redraw()
		{
			headerInfo.Position = GetScrollPosition();
			Range(resBegin, resEnd);
			SetScrollPosition(headerInfo.Position);
		}

		/// <summary>
		/// 白紙を開く
		/// </summary>
		private void OpenBlank()
		{
			object o = null;
			webBrowser.Navigate("about:blank", ref o, ref o, ref o, ref o);
		}

		/// <summary>
		/// フッターに<div id="footer"></div>が存在した場合、削除する
		/// </summary>
		private void RemoveFooter()
		{
			IHTMLDocument3 doc = (IHTMLDocument3)GetDocument();
			HTMLDivElement div = (HTMLDivElement)doc.getElementById("footer");

			if (div != null)
				div.removeNode(true);
		}

		/// <summary>
		/// 指定したレスのNGワードチェック
		/// </summary>
		/// <param name="res"></param>
		/// <returns>NGワード処理後のレス構造体</returns>
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
			else {
				res.IsABone = false;
			}

			return res;
		}

		/// <summary>
		/// textをブラウザに書き込む
		/// </summary>
		/// <param name="resSets"></param>
		private void WriteResInternal(ResSetCollection resSets)
		{
			if (resSets == null) {
				throw new ArgumentNullException("resSets");
			}

			try {
				// スキンでHTMLに変換
				string htmlText = skinBase.Convert(resSets);

				WriteText(htmlText);
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}
		}

		/// <summary>
		/// Popupメソッドを呼ぶ
		/// </summary>
		/// <param name="argument"></param>
		private void OnPopup(object sender, ObjectTimerEventArgs e)
		{
			Invoke(new PopupMethodInvoker(PopupInternal), new object[] {e.Tag});
		}

		/// <summary>
		/// argumentを使用してポップアップ
		/// </summary>
		/// <param name="argument"></param>
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

		/// <summary>
		/// レスポップアップ可能な文字列かどうかを判断
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private bool IsResPopupable(string text)
		{
			return URLParser.IsThreadUrl(text);
		}

		/// <summary>
		/// メールポップアップ可能な文字列かどうかを判断
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private bool IsMailPopupable(string text)
		{
			return text.StartsWith("mailto:");
		}

		/// <summary>
		/// 指定したURLがポップアップ可能かどうかを判断
		/// </summary>
		/// <param name="url"></param>
		private bool IsPicturePopupable(string url)
		{
			if (url == null) {
				throw new ArgumentNullException("url");
			}
			if ((skinStyle.imagePopup & PopupState.Disable) != 0)
				return false;

			if ((skinStyle.imagePopup & PopupState.KeySwitch) != 0 && (ModifierKeys & Keys.Control) == 0)
				return false;

			return Regex.IsMatch(url, @"(\.jpg$)|(\.jpeg$)|(\.gif$)|(\.png$)|(\.bmp$)",
				RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// ポップアップを消す
		/// </summary>
		private void HidePopup()
		{
			popInterf.Hide();
			clickedPopup = false;
		}

		/// <summary>
		/// デフォルトイベント
		/// </summary>
		[System.Runtime.InteropServices.DispId(0)]
		public void DefaultMethod()
		{
			try {
				HTMLBody body = GetHtmlBody();
				IHTMLWindow2 window = (IHTMLWindow2)GetDocument().parentWindow;
				IHTMLElement src = window.@event.srcElement;
				
				if (window.@event.button == 2)
				{
					IHTMLTxtRange range = (IHTMLTxtRange)GetDocument().selection.createRange();
					if (HtmlTextUtility.IsDigit(range.text))
					{
						ClickedPopup(ResReference.GetArray(range.text));
						window.@event.cancelBubble = true;
					}
				}
					// ポップアップを表示
				else if (src.tagName == "A" &&
					Regex.IsMatch(src.innerText, ">>\\d+")) 
				{
				}
				else if (clickedPopup && popInterf.Visible && !window.@event.type.Equals("mouseup")) 
				{
				}
					// ポップアップを隠す
				else {
					HidePopup();
				}
			}
			catch (Exception ex) {
				TwinDll.ShowOutput(ex);
			}
		}

		/// <summary>
		/// ドキュメントのイベントを設定
		/// </summary>
		private void SetDocumentEvents()
		{
			HTMLDocument doc = GetDocument();
			doc.onmouseover = this;
			doc.onmouseup = this;
		}

		/// <summary>
		/// AxWebBrowser.Documentプロパティを取得
		/// </summary>
		/// <returns></returns>
		public HTMLDocument GetDocument()
		{
			try {
				object doc = webBrowser.Document;
				Debug.Assert(doc != null, "ドキュメントの取得に失敗しました");

				return (HTMLDocument)doc;
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
			}
			return null;
		}

		/// <summary>
		/// AxWebBrowser.Document.bodyプロパティを取得
		/// </summary>
		/// <returns></returns>
		public HTMLBody GetHtmlBody()
		{
			return (HTMLBody)GetDocument().body;
		}
		#endregion

		#region Overrideメソッド
		/// <summary>
		/// あぼーんを検知
		/// </summary>
		protected override void OnABone()
		{
			MessageBox.Show(this, "あぼーんを検知した予感。ログを再取得してください。", headerInfo.Subject,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected override void OnPastlog()
		{
			OnStatusTextChanged(headerInfo.Subject + "はdat落ちしています");
		}

		protected override void OnKakolog()
		{
			OnStatusTextChanged(headerInfo.Subject + "を過去ログ倉庫で発見しました。");

			ThreadHeader header = new X2chKakoThreadHeader();
			header.Key = HeaderInfo.Key;
			header.BoardInfo = HeaderInfo.BoardInfo;
			header.BoardInfo.Bbs = BbsType.X2chKako;
			header.Subject = HeaderInfo.Subject;

			isOpen = false;
			thread = null;

			Open(header);
		}

		protected override void Opening()
		{
			// 描画開始と終了位置を取得
			resBegin = Math.Max(1, headerInfo.GotResCount - viewResCount);
			resEnd = -1;

			// レス表示制限しない場合は常に1から
			if (! Twinie.Settings.Thread.ViewResLimit)
				resBegin = 1;

			// しおりが設定されていればresBeginをしおり位置に設定
			if (headerInfo.Shiori > 0)
				resBegin = headerInfo.Shiori;

			// ヘッダーを書き込む
			Clear();
			
			// イベントを設定
			SetDocumentEvents();

			WriteText(skinBase.GetHeader(headerInfo));
		}

		/// <summary>
		/// スクロール位置を設定
		/// </summary>
		/// <param name="value"></param>
		protected override void SetScrollPosition(float value)
		{
			try {
				float pos = value;

				if (pos < 0)
				{
					HTMLBody body = GetHtmlBody();
					ScrollInternal(body.scrollHeight);
				}
				else if (pos == 0)
				{
					ScrollInternal(0);
				}
				else {
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
			catch (Exception ex) {
				TwinDll.Output(ex);
			}
		}

		private void ScrollInternal(int scroll)
		{			
			HTMLBody body = GetHtmlBody();
			body.scrollTop = scroll;
		}

		/// <summary>
		/// resSetsをブラウザの表示形式に変換後、書き込みをしていく。
		/// </summary>
		/// <param name="resSets"></param>
		protected override void Write(ResSetCollection resSets)
		{
			try {
				if (IsPackageReception)
					Cursor = Cursors.WaitCursor;

				ResSetCollection buffer = new ResSetCollection();
				bool shiori = (headerInfo.Shiori > 0); // しおりが設定されているかどうか

				for (int i = 0; i < resSets.Count; i++)
				{
					ResSet res = resSets[i];

					// しおりフラグを設定
					if (shiori)
						res.Bookmark = (res.Index == headerInfo.Shiori);

					// スレッドを開く際に表示インデックスが指定されている場合
					if (indicesValues.Count > 0)
					{
						res.Visible = indicesValues.Contains(res.Index);
					}
					else {
						// resBeginからresEndまでの間を表示 (1は常に表示)
						res.Visible = (res.Index >= resBegin) &&
									(res.Index <= resEnd || resEnd == -1) || (res.Index == 1);
					}

					if (res.Visible)
						res = CheckNGWords(res);

					buffer.Add(res);
				}

				WriteResInternal(buffer);
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}
			finally {
				Cursor = Cursors.Default;
//				Application.DoEvents();
			}
		}

		/// <summary>
		/// 書き込み開始
		/// </summary>
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

		/// <summary>
		/// 書き込みを完了しフッターを付加
		/// </summary>
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
		}
		#endregion

		/// <summary>
		/// スレッドを開く
		/// </summary>
		/// <param name="header"></param>
		public override void Open(ThreadHeader header)
		{
			base.Open(header);

			// スキンの基本URLを設定
			skinBase.BaseUri = header.Url;

			// NGワードを取得
			nGWords = Twinie.NGWords.Get(header.BoardInfo, false);

			// オートリロードの対象に設定
			if (autoReload)
				autoRefreshTimers.Add(this);
		}

		/// <summary>
		/// スレッドを閉じる
		/// </summary>
		public override void Close()
		{
			if (IsOpen)
			{
				// スクロール位置を保存
				headerInfo.Position = GetScrollPosition();
				ThreadIndexer.SavePosition(Cache, headerInfo);

				// あぼーん済みリストをクリア
				abonIndices.Clear();
				// スクロール履歴をクリア
				scrolled.Clear();
			}

			// オートリロードの対象から外す
			autoRefreshTimers.Remove(this);

			nGWords = null;

			base.Close();
		}

		/// <summary>
		/// 指定した1から始まるレス番号の配列をポップアップ表示
		/// </summary>
		/// <param name="indices"></param>
		public override void Popup(int[] indices)
		{
			if (indices == null)
				throw new ArgumentNullException("indices");

			clickedPopup = false;

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
		}

		/// <summary>
		/// 指定したレスコレクションをポップアップで表示
		/// </summary>
		/// <param name="resSets"></param>
		public override void Popup(ResSetCollection resSets)
		{
			if (resSets == null)
				throw new ArgumentNullException("resSets");

			clickedPopup = false;

			if (resSets.Count == 0)
				return;

			popInterf.Show(resSets);
		}

		/// <summary>
		/// スレッドを印刷
		/// </summary>
		public override void Print()
		{
			object o = null;
			webBrowser.ExecWB(OLECMDID.OLECMDID_PRINT,
				OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
		}

		/// <summary>
		/// 継承先でオーバーライドされれば、指定した位置にしおりを挟む。
		/// 既に同じ番号にしおりが設定されていたら、しおりを解除。
		/// </summary>
		/// <param name="shiori">しおりを挟む1から始まるレス番号</param>
		public override void Bookmark(int shiori)
		{
			// 同じ番号にしおりが設定されていたら、しおりを外す
			headerInfo.Shiori = (headerInfo.Shiori == shiori) ? 0 : shiori;
			ThreadIndexer.SaveBookmark(Cache, headerInfo);
			Redraw();
		}

		/// <summary>
		/// しおりを開く
		/// </summary>
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
		/// 指定した位置にスクロール
		/// </summary>
		/// <param name="position"></param>
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
					SetScrollPosition((float)scrolled.Pop());
				break;
			}
		}

		/// <summary>
		/// 指定したレス番号までスクロール
		/// </summary>
		/// <param name="resNumber"></param>
		public override void ScrollTo(int resNumber)
		{
			SetScrollPosition(resNumber);
		}

		/// <summary>
		/// 指定した範囲のレスを表示
		/// </summary>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		public override void Range(int begin, int end)
		{
			if (IsReading)
				throw new ApplicationException("スレを読み込み中です");

			if (begin < 1)					begin = 1;
			if (end > resCollection.Count)	end = -1;

			Opening();
			resBegin = begin;
			resEnd = end;

			WriteBegin();
			Write(resCollection);
			WriteEnd();
		}

		/// <summary>
		/// 指定した範囲に移動
		/// </summary>
		/// <param name="movement"></param>
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

		/// <summary>
		/// 表示を一端クリアしてitemsを書き込む
		/// </summary>
		/// <param name="items"></param>
		public override void WriteResColl(ResSetCollection items)
		{
			if (IsReading)
				throw new ApplicationException("スレを読み込み中です");

			Opening();

			resBegin = items.Count > 0 ? items[0].Index : 0;
			resEnd = items.Count > 0 ? items[items.Count-1].Index : 0;

			WriteBegin();
			Write(items);
			WriteEnd();
		}

		/// <summary>
		/// textをブラウザに書き込む
		/// </summary>
		/// <param name="text"></param>
		public override void WriteText(string text)
		{
			IHTMLDocument2 doc = (IHTMLDocument2)GetDocument();
			doc.write(text);
		}

		/// <summary>
		/// 表示をクリア
		/// </summary>
		public override void Clear()
		{
			IHTMLDocument2 doc = (IHTMLDocument2)GetDocument();
			doc.open("about:blank", null, null, null);
		}

		/// <summary>
		/// スレッドを開き指定したキーワードを含むレスのみを表示
		/// </summary>
		/// <param name="header"></param>
		/// <param name="info"></param>
		public override int OpenExtract(ThreadHeader header, ThreadExtractInfo info)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			if (info == null) {
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

			while (storage.Read(resSets) != 0);
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
			WriteBegin();
			WriteText(skinBase.Convert(matches));
			WriteEnd();

			OnStatusTextChanged("抽出キーワード: " + info.Keyword);
			return matches.Count;
		}
		
		/// <summary>
		/// スレッド内を検索するためのクラスを初期化
		/// </summary>
		/// <param name="keyword">検索する単語</param>
		/// <param name="flags">検索オプション</param>
		/// <returns></returns>
		public override AbstractSearcher BeginSearch()
		{
			HTMLDocument document = GetDocument();
			IEComSearcher r = new IEComSearcher(document);
			return r;
		}

		/// <summary>
		/// レスを抽出するためのクラスを初期化
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="options"></param>
		/// <param name="modePopup"></param>
		public override AbstractExtractor BeginExtract()
		{
			IEComExtractor r = new IEComExtractor(this);
			return r;
		}

		/// <summary>
		/// スレッドにフォーカスを移動
		/// </summary>
		public override void _Select()
		{
			HTMLDocument doc = GetDocument();
			IHTMLWindow2 win = doc.parentWindow;
			win.focus();
		}
	}

}
