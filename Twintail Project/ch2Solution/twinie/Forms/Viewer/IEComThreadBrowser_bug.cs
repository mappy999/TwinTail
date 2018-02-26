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
	///	IE�R���|�[�l���g���g�p���X���b�h��\������R���g���[��
	///	</summary>
	public class IEComThreadBrowser : ThreadControl, IExternalMethod//, IDocHostUIHandler
	{
		// �f���Q�[�g�̒�`
		private delegate void PopupMethodInvoker(string argument);

		// �I�[�g�����[�h���邽�߂̃N���X
		private readonly static AutoRefreshTimerBase autoRefreshTimers;

		// �O������֐����N�����邽�߂̃N���X
		private readonly IEComMethodInvoker methodInvoker;

		private static ImageViewUrlReplace imageViewUrlTool = null;
		public static ImageViewUrlReplace ImageViewUrlReplace
		{
			get
			{
				return imageViewUrlTool;
			}
		}

		// �摜���L���b�V������N���C�A���g
		private ImageCacheClient imageCacheClient = null;
		private AutoResetEvent iccEvent = new AutoResetEvent(false);

		// NG�����N
		public readonly static LinkInfoCollection linkCollection;
		private static readonly NGWords defNGWords;
		private NGWords nGWords;
		// ����o�߂���NGID���N���A�����̏����܂ރ��X�g
		private readonly static List<string> clearedIdList = new List<string>();

		private AxWebBrowser webBrowser;			// �X���b�h��\������R���g���[��
		private IPopupBase popInterf;				// �|�b�v�A�b�v�𐧌䂷��C���^�[�t�F�[�X
		private PopupSettings popSett;				// �|�b�v�A�b�v�̐ݒ���
		private ThreadSettings threadSett;			// �X���b�h�ݒ�
		private SkinStyle skinStyle;				// �X�L���̕\�������Ǘ�����N���X
		private ThreadSkinBase skinBase;			// �X�L���̕ϊ��������s���N���X
		private ObjectTimer popTimer;				// ���X�Q�Ƃ��|�C���^���Ă���|�b�v�A�b�v����܂ł̊Ԋu���Ǘ�����^�C�}�[
		private ABone abone;						// ���ځ[����
		private SortedValueCollection abonIndices;	// ���ځ[��ς݃��X�ԍ����R���N�V�����Ǘ� (�A�����ځ[��Ɏg�p����)
		private Stack<float> scrolled;						// �X�N���[����Ԃ��L������R���N�V����

		private string tempStatusText;				// AxWebBrowser.StatusTextChange�C�x���g���������邽�тɈ�����"e.text"���R�s�[�����ϐ�
		private bool enableAutoScroll;				// �������݊������ɃI�[�g�X�N���[�������s���邩�ǂ���
		private int resBegin, resEnd;				// �`��n�߃��X�ԍ��ƕ`��I�����X�ԍ���\��
		private int viewResCount;					// ���X�̕\����������\��

		private bool scrollToNewRes;				// �V���܂ŃX�N���[�����邩�ǂ����������l
		private bool autoReload;					// �I�[�g�����[�h���s�����ǂ���
		private bool autoScroll;					// �I�[�g�X�N���[�����s�����ǂ���

		internal bool isExtracting = false;			// ���o�����ǂ���
		internal string lastPopupRef;				// �Ō�Ƀ|�b�v�A�b�v�������X�ԍ����i�[����� (�������X���Q�d�|�b�v�A�b�v����Ă��܂��̂�h������)
		private bool disposed = false;				// Dispose���\�b�h���Ă΂ꂽ���ǂ�����\��

		internal bool clickedPopup;
		//		private	bool newPopup;		// �V�K�̃|�b�v�A�b�v

		private bool pastlogChecked = false;
		private bool kakoChecked = false;

		private object document = null;

		private Stack<_StringPair> createThumbImageUrls = new Stack<_StringPair>();

		///	<summary>
		///	�����X�V�^�C�}�[�N���X���擾
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
		/// �C���[�W�̃T���l�C�������擾���܂��B
		/// </summary>
		public Thumbnail Thumbnail
		{
			get
			{
				return thumbnail;
			}
		}

		///	<summary>
		///	�I������Ă��镶������擾
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
		/// �V���܂ŃX�N���[��
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
		///	�I�[�g�X�N���[���̗L���E����
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
		///	�I�[�g�X�����[�h�̗L���E����
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
		/// �t�H���g�T�C�Y���擾�܂��͐ݒ�
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
		///	�\�����X�����擾�܂��͐ݒ�
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
		//		///	���ځ[������擾
		//		///	</summary>
		//		public ABone ABone {
		//			get	{
		//				return abone;
		//			}
		//		}
		//
		//		///	<summary>
		//		///	�\���Ɏg�p����X�L�����擾
		//		///	</summary>
		//		public ThreadSkinBase Skin {
		//			get	{ return skinBase; }
		//		}

		///	<summary>
		///	IEComThreadBrowser
		///	</summary>
		static IEComThreadBrowser()
		{
			// �I�[�g�����[�h�^�C�}�[
			autoRefreshTimers = new AutoRefreshTimerCollection2();
			// �f�t�H���g��NG���[�h�ݒ�
			defNGWords = Twinie.NGWords.Default;

			imageViewUrlTool = new ImageViewUrlReplace(Settings.ImageViewUrlReplacePath);

			try
			{//	NG�A�h���X��ǂݍ���
				linkCollection = new LinkInfoCollection(Settings.NGAddrsPath);
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		///	<summary>
		///	IEComThreadBrowser�N���X�̃C���X�^���X��������
		///	</summary>
		public IEComThreadBrowser(Cache cache, Settings sett)
			: base(cache)
		{
			// 
			// TODO: �R���X�g���N�^	���W�b�N�������ɒǉ����Ă��������B
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

			// �O�����\�b�h
			IExternalMethod iem = this;
			methodInvoker = new IEComMethodInvoker(iem);

			// ���ځ[��֘A
			abone = sett.ABone;
			abonIndices = new SortedValueCollection();

			// �|�b�v�A�b�v�֘A
			popSett = sett.Popup;
			popTimer = new ObjectTimer();
			popTimer.Elapsed += new ObjectTimerEventHandler(OnPopup);
			popTimer.Interval = popSett.PopupInterval;

			// �X�L���֘A
			skinStyle = new SkinStyle(sett);
			skinStyle.Read(Path.Combine(sett.SkinFolderPath, "Style.ini"));

			Twin2IeHtmlSkin skin = new Twin2IeHtmlSkin(sett.SkinFolderPath);
			skinBase = skin;

			if (popSett.Extend)
				skin.ExtendPopupStr = popSett.ExtendPopupStr;

			// �T���l�C���ݒ�
			thumbnail = sett.Thumbnail;

			// �C���[�W�L���b�V��
			imageCacheClient = new ImageCacheClient();
			imageCacheClient.ImageCached += new ImageCacheEventHandler(imageCacheClient_ImageCached);
			imageCacheClient.ImageCacheDirectory = Settings.ImageCacheDirectory;


			// �|�b�v�A�b�v�N���X��������
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

			// ���̂ق��̐ݒ�
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
		///	�g�p���Ă��郊�\�[�X�����
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

			// ���ׂĂ�IMG�^�O���擾
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

			// �L���b�V�����ꂽ�摜���T���l�C���ɕϊ�
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
		///	AxWebBrowser��������
		///	</summary>
		///	<param name="owner">�e�R���g���[�� (null���w��\)</param>
		///	<returns>���������ꂽAxWebBrowser�̃C���X�^���X</returns>
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

			// Shift�L�[��������Ă���܂��̓}�E�X�{�^����������Ă���ꍇ�̓|�b�v�A�b�v���Ȃ�
			if ((ModifierKeys & Keys.Shift) != 0 || (MouseButtons != MouseButtons.None))
				return;

			// ���݃|�b�v�A�b�v���̃��X�̓|�b�v�A�b�v���Ȃ�
			if (popInterf is HtmlPopup && lastPopupRef.Equals(tempStatusText))
				return;

			// ���\�b�h���N��
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
				// �|�b�v�A�b�v�^�C�}�[�J�n
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

				// ���X�ԍ����N���b�N���ꂽ
				if (tempStatusText.StartsWith("menu:"))
				{
					int index = Int32.Parse(tempStatusText.Substring(5));
					ResSet resSet = resCollection[index - 1];

					OnNumberClick(new NumberClickEventArgs(headerInfo, resSet));
					return;
				}
				// ���\�b�h���N��
				else if (tempStatusText.StartsWith("method:"))
				{
					string funcText = tempStatusText.Substring(7);
					methodInvoker.Invoke(funcText);
					return;
				}

				// ���X�Q�Ƃ��N���b�N���ꂽ
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

							// �N���b�N���ꂽ���X�Ԃ��A�\������Ă��Ȃ���΍ă����_�����O
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
		///	that overrides the default Microsoft� Internet Explorer	registry settings.
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

		#region	�O���Ăяo���\�ȃ��\�b�h
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

		#region	Private���\�b�h
		///	<summary>
		///	�X�N���[���ʒu���擾
		///	</summary>
		///	<returns></returns>
		private float GetScrollPosition()
		{
			// ���݈�ԏ�ɕ\������Ă��郌�X�ԍ����擾
			try
			{
				HTMLBody body = (HTMLBody)GetHtmlBody();
				IHTMLElementCollection collect = (IHTMLElementCollection)body.getElementsByTagName("indices");
				IHTMLElement element = null;

				int scrollTop = body.scrollTop;
				int left = 0, right = collect.length - 1;

				// �񕪒T��
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
		///	�ĕ`��
		///	</summary>
		private void Redraw()
		{
			headerInfo.Position = GetScrollPosition();
			Range(resBegin, resEnd);
			SetScrollPosition(headerInfo.Position);
		}

		///	<summary>
		///	�������J��
		///	</summary>
		private void OpenBlank()
		{
			webBrowser.Navigate("about:blank");
		}

		///	<summary>
		///	�t�b�^�[��<div id="footer"></div>�����݂����ꍇ�A�폜����
		///	</summary>
		private void RemoveFooter()
		{
			IHTMLDocument3 doc = (IHTMLDocument3)GetDocument();
			HTMLDivElement div = (HTMLDivElement)doc.getElementById("footer");

			if (div != null)
				div.removeNode(true);
		}

		///	<summary>
		///	�w�肵�����X��NG���[�h�`�F�b�N
		///	</summary>
		///	<param name="res"></param>
		///	<returns>NG���[�h������̃��X�\����</returns>
		private ResSet CheckNGWords(ResSet res)
		{
			// �f�t�H���g��NG���[�h�A�ʂ�NG���[�h�A�A�����ځ[������s
			if (Twinie.Settings.NGWordsOn &&
				(defNGWords.IsMatch(res) || (nGWords != null && nGWords.IsMatch(res)) ||
				(abone.Chain && abonIndices.ContainsAny(res.RefIndices))))
			{
				res.Visible = abone.Visible;
				res.IsABone = true;

				// ���ځ[�񂳂ꂽ���X�g�Ƀ��X�ԍ���ǉ�
				abonIndices.Add(res.Index);

				// �R���N�V�������̃��X�����ځ[�񏈗�
				resCollection.ABone(res.Index, true);
			}
			else
			{
				res.IsABone = false;
			}

			return res;
		}

		///	<summary>
		///	text���u���E�U�ɏ�������
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
				// �X�L����HTML�ɕϊ�
				string htmlText = skinBase.Convert(resSets);

				if (thumbnail.Visible)
				{
					htmlText = CreateThumbnailHtml(htmlText, resSets);
				}

				WriteText(htmlText);

				// �t�Q�Ƃ�F�Â�
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
		///	Popup���\�b�h���Ă�
		///	</summary>
		///	<param name="argument"></param>
		private void OnPopup(object sender, ObjectTimerEventArgs e)
		{
			Invoke(new PopupMethodInvoker(PopupInternal), new object[] { e.Tag });
		}

		///	<summary>
		///	argument���g�p���ă|�b�v�A�b�v
		///	</summary>
		///	<param name="argument"></param>
		private void PopupInternal(string argument)
		{
			lastPopupRef = String.Empty;

			// ���X�|�b�v�A�b�v
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
			// ���[�����|�b�v�A�b�v
			else if (IsMailPopupable(argument))
			{
				int token = argument.IndexOf("mailto:");
				popInterf.Show(argument.Substring(token + 7));
			}
			// �摜�|�b�v�A�b�v
			else if (IsPicturePopupable(argument))
			{
				popInterf.ShowImage(argument);
			}

			if (popInterf is HtmlPopup)
				((HtmlPopup)popInterf).inPopup = true;
		}

		///	<summary>
		///	���X�|�b�v�A�b�v�\�ȕ����񂩂ǂ����𔻒f
		///	</summary>
		///	<param name="text"></param>
		///	<returns></returns>
		private bool IsResPopupable(string text)
		{
			return URLParser.IsThreadUrl(text);
		}

		///	<summary>
		///	���[���|�b�v�A�b�v�\�ȕ����񂩂ǂ����𔻒f
		///	</summary>
		///	<param name="text"></param>
		///	<returns></returns>
		private bool IsMailPopupable(string text)
		{
			// sage �̓|�b�v�A�b�v���Ȃ��悤�ɂ��Ă݂�
			return text.StartsWith("mailto:") && !text.Trim().ToLower().Equals("mailto:sage");
		}

		///	<summary>
		///	�w�肵��URL���|�b�v�A�b�v�\���ǂ����𔻒f
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
		///	�|�b�v�A�b�v������
		///	</summary>
		private void HidePopup()
		{
			popInterf.Hide();
			clickedPopup = false;
			isExtracting = false;
			//		newPopup = false;
		}

		///	<summary>
		///	�f�t�H���g�C�x���g
		///	</summary>
		[System.Runtime.InteropServices.DispId(0)]
		public void DefaultMethod()
		{
			try
			{

				HTMLBody body = GetHtmlBody();
				IHTMLWindow2 window = (IHTMLWindow2)GetDocument().parentWindow;
				IHTMLElement src = window.@event.srcElement;

				// �|�b�v�A�b�v��\��
				if (src.tagName == "A" &&
					src.innerText != null && src.innerText.Length > 0 &&
					Regex.IsMatch(src.innerText, ">>\\d+"))
				{
				}
				else if (clickedPopup && popInterf.Visible && !window.@event.type.Equals("mouseup"))
				{
				}
				// �|�b�v�A�b�v���B��
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
		///	�h�L�������g�̃C�x���g��ݒ�
		///	</summary>
		private void SetDocumentEvents()
		{
			HTMLDocument doc = GetDocument();

			
		//	DispHTMLDocument disp = doc;
		//	disp.onmouseover = this;
		//	disp.onmouseup = this;

			
		}

		///	<summary>
		///	AxWebBrowser.Document�v���p�e�B���擾
		///	</summary>
		///	<returns></returns>
		public HTMLDocument GetDocument()
		{
			return document as HTMLDocument;
		}

		///	<summary>
		///	AxWebBrowser.Document.body�v���p�e�B���擾
		///	</summary>
		///	<returns></returns>
		public HTMLBody GetHtmlBody()
		{
			return (HTMLBody)GetDocument().body;
		}
		#endregion

		#region	Override���\�b�h
		///	<summary>
		///	���ځ[������m
		///	</summary>
		protected override void OnABone()
		{
			MessageBox.Show(this, "���ځ[������m�����\���B���O���Ď擾���Ă��������B", headerInfo.Subject,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected override void OnPastlog(PastlogEventArgs e)
		{
			ThreadHeader header = null;

			if (!pastlogChecked && HeaderInfo.BoardInfo.Bbs.Equals(BbsType.X2ch) && Twinie.Settings.Authentication.AuthenticationOn)
			{
				OnStatusTextChanged(headerInfo.Subject + "��dat�������Ă��܂�");

				header = new X2chThreadHeader();
				header.Key = HeaderInfo.Key;
				header.BoardInfo = new BoardInfo(HeaderInfo.BoardInfo.Server, HeaderInfo.BoardInfo.Path, HeaderInfo.BoardInfo.Name);
				header.BoardInfo.Bbs = BbsType.X2chAuthenticate;
				header.Subject = HeaderInfo.Subject;

				pastlogChecked = true;
			}
			else if (!kakoChecked)
			{
				OnStatusTextChanged(headerInfo.Subject + ": �ߋ����O�q�ɂ𒲂ׂĂ݂�e�X�g�B");

				header = new X2chKakoThreadHeader();
				header.Key = HeaderInfo.Key;
				header.BoardInfo = new BoardInfo(HeaderInfo.BoardInfo.Server, HeaderInfo.BoardInfo.Path, HeaderInfo.BoardInfo.Name);
				header.BoardInfo.Bbs = BbsType.X2chKako;
				header.Subject = HeaderInfo.Subject;

				kakoChecked = true;
			}
			else
			{
				OnStatusTextChanged(headerInfo.Subject + "�͓ǂݍ��߂܂���ł����B");
				return;
			}

			isOpen = false;
			canceled = true;
			thread = null;

			Open(header);
		}

		protected override void Opening()
		{
			// �X�L�������Z�b�g
			skinBase.Reset();

			// �`��J�n�ƏI���ʒu���擾
			resBegin = Math.Max(1, headerInfo.GotResCount - viewResCount);
			resEnd = -1;

			// �Q�Ɖ񐔂����Z�b�g
			resCollection.ResetBackReferenceCount();

			// ���X�\���������Ȃ��ꍇ�͏��1����
			if (!Twinie.Settings.Thread.ViewResLimit)
				resBegin = 1;

			// �����肪�ݒ肳��Ă����resBegin��������ʒu�ɐݒ�
			if (headerInfo.Shiori > 0)
				resBegin = headerInfo.Shiori;

			isExtracting = false;
			popInterf.Hide();

			// �w�b�_�[����������
			Clear();

			// �C�x���g��ݒ�
			SetDocumentEvents();

			// �t�H���g�T�C�Y��ݒ�
			FontSize = threadSett.FontSize;

			WriteText(skinBase.GetHeader(headerInfo));

			RunImageCacheClient();

			Application.DoEvents();

		}

		///	<summary>
		///	�X�N���[���ʒu��ݒ�
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
		///	resSets���u���E�U�̕\���`���ɕϊ���A�������݂����Ă����B
		///	</summary>
		///	<param name="resSets"></param>
		protected override void Write(ResSetCollection resSets)
		{
			try
			{
				if (IsPackageReception)
					Cursor = Cursors.WaitCursor;

				ResSetCollection buffer = new ResSetCollection();
				bool shiori = (headerInfo.Shiori > 0); // �����肪�ݒ肳��Ă��邩�ǂ���

				for (int i = 0; i < resSets.Count; i++)
				{
					ResSet res = resSets[i];

					((Twin2IeHtmlSkin)skinBase).IncrementIDCount(res.ID);

					// ������t���O��ݒ�
					if (shiori)
						res.Bookmark = (res.Index == headerInfo.Shiori);

					// �X���b�h���J���ۂɕ\���C���f�b�N�X���w�肳��Ă���ꍇ
					if (indicesValues.Count > 0)
					{
						res.Visible = indicesValues.Contains(res.Index);
					}
					else
					{
						// resBegin����resEnd�܂ł̊Ԃ�\��	(1�͏�ɕ\��)
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
		///	�������݊J�n
		///	</summary>
		protected override void WriteBegin()
		{
			enableAutoScroll = false;

			if (autoScroll)
			{
				// ��ԉ��ɃX�N���[������Ă���΃I�[�g�X�N���[����L���ɂ���
				HTMLBody body = GetHtmlBody();
				enableAutoScroll = (body.scrollTop + body.clientHeight) == body.scrollHeight;
			}

			// �t�b�^�[���폜
			RemoveFooter();

		}

		///	<summary>
		///	�������݂��������t�b�^�[��t��
		///	</summary>
		protected override void WriteEnd()
		{
			// �V�����X������ꍇ�݂̂Ƀt�b�^�[��t��
			if (headerInfo.NewResCount > 0)
				WriteText(skinBase.GetFooter(headerInfo));

			// ���ꂵ�Ȃ��Ə������݂��I���Ȃ������ɃX�N���[�����Ă��܂��ׁA
			// �����܂ŃX�N���[���ł��Ȃ��B
			Application.DoEvents();

			// �V���܂ŃX�N���[��
			if (scrollToNewRes)
				ScrollTo(headerInfo.GotResCount - headerInfo.NewResCount + 1);

			// �I�[�g�X�N���[�������s
			else if (enableAutoScroll)
				ScrollTo(ScrollPosition.Bottom);

			// �t�Q�Ƃ̃J���[�����O�����s
			if (threadSett.IsColoringBackReference)
			{
				FlushColoringBackReference();
			}

			FlushCreateThumbnailStack();
		}
		#endregion

		///	<summary>
		///	�X���b�h���J��
		///	</summary>
		///	<param name="header"></param>
		public override void Open(ThreadHeader header)
		{
			base.Open(header);

			// �X�L���̊�{URL��ݒ�
			skinBase.BaseUri = header.Url;

			// NG���[�h���擾
			nGWords = Twinie.NGWords.Get(header.BoardInfo, false);

			// ����o�߂��Ă���NG���[�h���폜
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

			// �I�[�g�����[�h�̑Ώۂɐݒ�
			if (autoReload)
				autoRefreshTimers.Add(this);
		}

		///	<summary>
		///	�X���b�h�����
		///	</summary>
		public override void Close()
		{
			if (IsOpen)
			{
				kakoChecked = false;
				pastlogChecked = false;

				// �X�N���[���ʒu��ۑ�
				headerInfo.Position = GetScrollPosition();
				ThreadIndexer.SavePosition(Cache, headerInfo);

				popInterf.Hide();
			}

			CloseImageCacheClient();

			// ���ځ[��ς݃��X�g���N���A
			abonIndices.Clear();
			// �X�N���[���������N���A
			scrolled.Clear();

			// �I�[�g�����[�h�̑Ώۂ���O��
			autoRefreshTimers.Remove(this);

			nGWords = null;

			base.Close();
		}

		///	<summary>
		///	�w�肵��1����n�܂郌�X�ԍ��̔z����|�b�v�A�b�v�\��
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
		///	�w�肵�����X�R���N�V�������|�b�v�A�b�v�ŕ\��
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
		///	�X���b�h�����
		///	</summary>
		public override void Print()
		{
			object o = null;

			webBrowser.ExecWB(OLECMDID.OLECMDID_PRINT,
				OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
		}

		///	<summary>
		///	�p����ŃI�[�o�[���C�h�����΁A�w�肵���ʒu�ɂ���������ށB
		///	���ɓ����ԍ��ɂ����肪�ݒ肳��Ă�����A������������B
		///	</summary>
		///	<param name="shiori">�����������1����n�܂郌�X�ԍ�</param>
		public override void Bookmark(int shiori)
		{
			// �����ԍ��ɂ����肪�ݒ肳��Ă�����A��������O��
			headerInfo.Shiori = (headerInfo.Shiori == shiori) ? 0 : shiori;
			ThreadIndexer.SaveBookmark(Cache, headerInfo);
			Redraw();
		}

		/// <summary>
		/// �w�肵�� sirusi �ԍ��̃��X�Ɉ��t����B
		/// �����ԍ��Ɉ󂷂�ƁA�󂪉��������B
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
		///	��������J��
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
		/// �󂳂ꂽ���X��\��
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
				MessageBox.Show("���邵�����݂��܂���");
		}

		///	<summary>
		///	�w�肵���ʒu�ɃX�N���[��
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
		///	�w�肵�����X�ԍ��܂ŃX�N���[��
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
		///	�w�肵���͈͂̃��X��\��
		///	</summary>
		///	<param name="begin"></param>
		///	<param name="end"></param>
		public override void Range(int begin, int end)
		{
			if (IsReading)
				throw new ApplicationException("�X����ǂݍ��ݒ��ł�");

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
		///	�w�肵���͈͂Ɉړ�
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
		///	�\������[�N���A����items����������
		///	</summary>
		///	<param name="items"></param>
		public override void WriteResColl(ResSetCollection items)
		{
			if (IsReading)
				throw new ApplicationException("�X����ǂݍ��ݒ��ł�");

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
		///	text���u���E�U�ɏ�������
		///	</summary>
		///	<param name="text"></param>
		public override void WriteText(string text)
		{
			IHTMLDocument2 doc = (IHTMLDocument2)GetDocument();
			doc.write(text);
		}

		///	<summary>
		///	�\�����N���A
		///	</summary>
		public override void Clear()
		{
			IHTMLDocument2 doc = (IHTMLDocument2)GetDocument();
			doc.body.innerHTML = String.Empty;
		}

		///	<summary>
		///	�X���b�h���J���w�肵���L�[���[�h���܂ރ��X�݂̂�\��
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

			// �����N���X��������
			BmSearch2 bmSearch = new BmSearch2(info.Keyword);
			ResSetCollection resSets = new ResSetCollection();
			ResSetCollection matches = new ResSetCollection();

			ThreadIndexer.Read(Cache, header);
			headerInfo = header;

			// ���X��ǂݍ���
			ThreadStorage storage = new LocalThreadStorage(Cache);
			storage.Open(header, StorageMode.Read);

			while (storage.Read(resSets) != 0)
				;
			storage.Close();

			// ���X����������v���Ă��郌�X�݂̂�matches�ɓ����
			foreach (ResSet res in resSets)
			{
				string str = res.ToString(ResSetElement.All);
				if (bmSearch.Search(str) >= 0)
					matches.Add(res);
			}

			// ��v�������X�݂̂�\��
			resCollection.AddRange(resSets);
			Opening();

			bool temp = scrollToNewRes;
			scrollToNewRes = false;

			WriteBegin();
			WriteText(skinBase.Convert(matches));
			WriteEnd();

			scrollToNewRes = temp;

			OnStatusTextChanged("���o�L�[���[�h: " + info.Keyword);
			return matches.Count;
		}

		///	<summary>
		///	�X���b�h�����������邽�߂̃N���X��������
		///	</summary>
		///	<param name="keyword">��������P��</param>
		///	<param name="flags">�����I�v�V����</param>
		///	<returns></returns>
		public override AbstractSearcher BeginSearch()
		{
			HTMLDocument document = GetDocument();
			IEComSearcher r = new IEComSearcher(document);
			return r;
		}

		///	<summary>
		///	���X�𒊏o���邽�߂̃N���X��������
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
		///	�X���b�h�Ƀt�H�[�J�X���ړ�
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

					// �摜�ւ̃����N������
					int index = html.LastIndexOf("<a href=\"" + url, start);

					if (index < 0)
						break;

					// �摜�����N�̐擪�Ƀ^�O��}��
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
		/// �w�肵���摜url�̃T���l�C���^�O���쐬
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
		/// newItems ���Q�Ƃ��Ă��郌�X�̔ԍ���F�Â����Ă���
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
				// �Q�Ƃ�50�ȏ�̃��X�̓J�E���g���Ȃ�
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
		/// items �̒��ŁA�t�Q�Ƃ���Ă��郌�X (IsBackReferenced=true) �̔ԍ���F�Â�����
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
		/// �w�肵���C���f�b�N�X���Q�Ƃ��Ă��郌�X���|�b�v�A�b�v���܂��B
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
