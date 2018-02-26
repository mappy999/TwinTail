// ImageViewer.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Text;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using System.Windows.Forms;
	using System.Diagnostics;
	using CSharpSamples;
	using System.Net.Configuration;
	using Twin;
	using System.Threading;
	using System.Collections.Generic;

	/// <summary>
	/// ImageViewer の概要の説明です。
	/// </summary>
	public class ImageViewer : System.Windows.Forms.Form
	{
		private const int maxRecentUrl = 15;

		internal readonly ImageCache imageCache;
		internal readonly string folderPath;

		private ImageList imageList;
		private ArrayList windowList;

		private NGURLCollection nGUrls;
		private ImageViewerSettings settings;

		private WindowInfo prevActiveWindow = null;
		private bool isSavingMemory = true;

		#region Designer Fields

		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.MenuItem menuItem31;
		private System.Windows.Forms.MenuItem menuItem35;
		private System.Windows.Forms.MenuItem menuItem37;
		private System.Windows.Forms.MenuItem menuItem40;
		private System.Windows.Forms.MenuItem menuItem41;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem44;
		private System.Windows.Forms.MenuItem menuItem54;
		private System.Windows.Forms.MenuItem menuItem55;
		private System.Windows.Forms.MenuItem menuItem56;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.StatusBarPanel statusBarUrl;
		private System.Windows.Forms.StatusBarPanel statusBarByteCount;
		private System.Windows.Forms.StatusBarPanel statusBarSize;
		private System.Windows.Forms.MenuItem menuItem58;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem menuItemFileOpenUrl;
		private System.Windows.Forms.MenuItem menuItemFileExit;
		private System.Windows.Forms.MenuItem menuItemEditCancelSelected;
		private System.Windows.Forms.MenuItem menuItemViewCloseActive;
		private System.Windows.Forms.MenuItem menuItemViewCloseAll;
		private System.Windows.Forms.ContextMenu contextMenuImage;
		private System.Windows.Forms.ContextMenu contextMenuTab;
		private System.Windows.Forms.MenuItem menuItem52;
		private System.Windows.Forms.MenuItem menuItem53;
		private System.Windows.Forms.MenuItem menuItemTabClose;
		private System.Windows.Forms.MenuItem menuItemTabCloseExceptThis;
		private System.Windows.Forms.MenuItem menuItemTabCloseAll;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.MenuItem menuItemFileOpenWebBrowser;
		private System.Windows.Forms.MenuItem menuItemFileQuickSave;
		private System.Windows.Forms.MenuItem menuItemFileSave;
		private System.Windows.Forms.MenuItem menuItemFileSavePackage;
		private System.Windows.Forms.MenuItem menuItemFileCacheView;
		private System.Windows.Forms.MenuItem menuItemFileCacheDel;
		private System.Windows.Forms.MenuItem menuItemFileRecent;
		private System.Windows.Forms.MenuItem menuItemViewMosaic;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemViewSettings;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemFileRecentClear;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemEditCopyImage;
		private System.Windows.Forms.MenuItem menuItemEditCopyUrl;
		private System.Windows.Forms.MenuItem menuItemEditSetNGUrl;
		private System.Windows.Forms.MenuItem menuItemEditNGUrl;
		private System.Windows.Forms.MenuItem menuItemEditSelectAll;
		private System.Windows.Forms.MenuItem menuItemView;
		private System.Windows.Forms.MenuItem menuItemViewCancelMosaicAll;
		private System.Windows.Forms.MenuItem menuItemViewGifAnime;
		private System.Windows.Forms.MenuItem menuItemViewCloseError;
		private System.Windows.Forms.MenuItem menuItemViewDelCache;
		private System.Windows.Forms.MenuItem menuItemViewPrev;
		private System.Windows.Forms.MenuItem menuItemViewNext;
		private System.Windows.Forms.MenuItem menuItemViewTopMost;
		private System.Windows.Forms.MenuItem menuItemTabDelCache;
		private System.Windows.Forms.MenuItem menuItemTabCloseError;
		private System.Windows.Forms.MenuItem menuItemImageOpenWebBrowser;
		private System.Windows.Forms.MenuItem menuItemImageQuickSave;
		private System.Windows.Forms.MenuItem menuItemImageSave;
		private System.Windows.Forms.MenuItem menuItemImageSavePackage;
		private System.Windows.Forms.MenuItem menuItemImageCopy;
		private System.Windows.Forms.MenuItem menuItemImageCopyUrl;
		private System.Windows.Forms.MenuItem menuItemImageMosaic;
		private System.Windows.Forms.MenuItem menuItemImageNGUrl;
		private System.Windows.Forms.MenuItem menuItem3;


		#endregion
		private StatusBarPanel statusBarMessage;
		private MenuItem menuItem8;
		private MenuItem menuItemViewOriginalSize;
		private MenuItem menuItemViewReget;
		private MenuItem menuItemErrorTabRefresh;
		private MenuItem menuItem11;
		private MenuItem menuItem5;
		private MenuItem menuItemImageReget;
		private MenuItem menuItemViewClose404Tab;
		private MenuItem menuItem12;
		private StatusBarPanel statusBarIsSaved;

		private IContainer components;

		/// <summary>
		/// 選択されているタブの情報を取得
		/// </summary>
		private WindowInfo SelectedWindow {
			get {
				return (tabControl.SelectedTab != null) ?
					(WindowInfo)tabControl.SelectedTab.Tag : null;
			}
		}

		/// <summary>
		/// 選択されているすべてのウインドウを取得
		/// </summary>
		private WindowInfo[] Selections {
			get {
				ArrayList sel = new ArrayList();

				foreach (WindowInfo info in windowList)
					if (info.Selected) sel.Add(info);

				return (WindowInfo[])sel.ToArray(typeof(WindowInfo));
			}
		}

		/// <summary>
		/// ビューアの設定情報を取得
		/// </summary>
		public ImageViewerSettings Settings {
			get {
				return settings;
			}
		}

		/// <summary>
		/// ImageViewerクラスのインスタンスを初期化
		/// </summary>
		public ImageViewer()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//

			// 設定情報を初期化
			settings = ImageViewerSettings.Load(
				ImageViewerSettings.SettingPath);

			if (settings.WindowBounds != Rectangle.Empty)
			{
				this.StartPosition = FormStartPosition.Manual;
				this.Location = settings.WindowBounds.Location;
				this.ClientSize = settings.WindowBounds.Size;
			}

			folderPath = settings.ImageCacheFolder;
			new HttpWebRequestElement().UseUnsafeHeaderParsing = true;

			imageList = new ImageList();
			imageList.ImageSize = settings.TabImageSize;
			imageList.ColorDepth = ColorDepth.Depth16Bit;

			// エラー用の画像を作成しておく
			imageList.Images.Add(ImageUtil.GetErrorImage());

			imageCache = new ImageCache(folderPath);
			windowList = new ArrayList();

			nGUrls = new NGURLCollection();
			nGUrls.Patterns = settings.NGUrlPattern;

			tabControl.ItemSize = imageList.ImageSize;
			TopMost = settings.TopMost;
			isSavingMemory = settings.SavingMemory;
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
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
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpenUrl = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpenWebBrowser = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemFileQuickSave = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItemFileSave = new System.Windows.Forms.MenuItem();
			this.menuItemFileSavePackage = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItemFileCacheView = new System.Windows.Forms.MenuItem();
			this.menuItemFileCacheDel = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItemFileRecent = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemFileRecentClear = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItemFileExit = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemEditCopyImage = new System.Windows.Forms.MenuItem();
			this.menuItemEditCopyUrl = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItemEditSetNGUrl = new System.Windows.Forms.MenuItem();
			this.menuItemEditNGUrl = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuItemEditCancelSelected = new System.Windows.Forms.MenuItem();
			this.menuItemEditSelectAll = new System.Windows.Forms.MenuItem();
			this.menuItemView = new System.Windows.Forms.MenuItem();
			this.menuItemViewMosaic = new System.Windows.Forms.MenuItem();
			this.menuItemViewCancelMosaicAll = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItemViewReget = new System.Windows.Forms.MenuItem();
			this.menuItemErrorTabRefresh = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItemViewOriginalSize = new System.Windows.Forms.MenuItem();
			this.menuItem29 = new System.Windows.Forms.MenuItem();
			this.menuItemViewGifAnime = new System.Windows.Forms.MenuItem();
			this.menuItem31 = new System.Windows.Forms.MenuItem();
			this.menuItemViewCloseActive = new System.Windows.Forms.MenuItem();
			this.menuItemViewCloseAll = new System.Windows.Forms.MenuItem();
			this.menuItem35 = new System.Windows.Forms.MenuItem();
			this.menuItemViewClose404Tab = new System.Windows.Forms.MenuItem();
			this.menuItemViewCloseError = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItemViewDelCache = new System.Windows.Forms.MenuItem();
			this.menuItem37 = new System.Windows.Forms.MenuItem();
			this.menuItemViewPrev = new System.Windows.Forms.MenuItem();
			this.menuItemViewNext = new System.Windows.Forms.MenuItem();
			this.menuItem58 = new System.Windows.Forms.MenuItem();
			this.menuItemViewTopMost = new System.Windows.Forms.MenuItem();
			this.menuItemViewSettings = new System.Windows.Forms.MenuItem();
			this.menuItem40 = new System.Windows.Forms.MenuItem();
			this.menuItem41 = new System.Windows.Forms.MenuItem();
			this.contextMenuImage = new System.Windows.Forms.ContextMenu();
			this.menuItemImageOpenWebBrowser = new System.Windows.Forms.MenuItem();
			this.menuItem54 = new System.Windows.Forms.MenuItem();
			this.menuItemImageQuickSave = new System.Windows.Forms.MenuItem();
			this.menuItem44 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemImageSave = new System.Windows.Forms.MenuItem();
			this.menuItemImageSavePackage = new System.Windows.Forms.MenuItem();
			this.menuItem55 = new System.Windows.Forms.MenuItem();
			this.menuItemImageCopy = new System.Windows.Forms.MenuItem();
			this.menuItemImageCopyUrl = new System.Windows.Forms.MenuItem();
			this.menuItem56 = new System.Windows.Forms.MenuItem();
			this.menuItemImageMosaic = new System.Windows.Forms.MenuItem();
			this.menuItemImageNGUrl = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemImageReget = new System.Windows.Forms.MenuItem();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarUrl = new System.Windows.Forms.StatusBarPanel();
			this.statusBarMessage = new System.Windows.Forms.StatusBarPanel();
			this.statusBarIsSaved = new System.Windows.Forms.StatusBarPanel();
			this.statusBarByteCount = new System.Windows.Forms.StatusBarPanel();
			this.statusBarSize = new System.Windows.Forms.StatusBarPanel();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.contextMenuTab = new System.Windows.Forms.ContextMenu();
			this.menuItemTabClose = new System.Windows.Forms.MenuItem();
			this.menuItemTabCloseExceptThis = new System.Windows.Forms.MenuItem();
			this.menuItem52 = new System.Windows.Forms.MenuItem();
			this.menuItemTabCloseError = new System.Windows.Forms.MenuItem();
			this.menuItemTabDelCache = new System.Windows.Forms.MenuItem();
			this.menuItem53 = new System.Windows.Forms.MenuItem();
			this.menuItemTabCloseAll = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.statusBarUrl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMessage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarIsSaved)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarByteCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarSize)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemEdit,
            this.menuItemView,
            this.menuItem40});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFileOpenUrl,
            this.menuItemFileOpenWebBrowser,
            this.menuItem4,
            this.menuItemFileQuickSave,
            this.menuItem7,
            this.menuItemFileSave,
            this.menuItemFileSavePackage,
            this.menuItem10,
            this.menuItemFileCacheView,
            this.menuItemFileCacheDel,
            this.menuItem13,
            this.menuItemFileRecent,
            this.menuItem15,
            this.menuItemFileExit});
			this.menuItemFile.Text = "ファイル(&F)";
			this.menuItemFile.Popup += new System.EventHandler(this.menuItemFile_Popup);
			// 
			// menuItemFileOpenUrl
			// 
			this.menuItemFileOpenUrl.Index = 0;
			this.menuItemFileOpenUrl.Text = "URLを指定して画像を開く(&O)...";
			this.menuItemFileOpenUrl.Click += new System.EventHandler(this.menuItemFileOpenUrl_Click);
			// 
			// menuItemFileOpenWebBrowser
			// 
			this.menuItemFileOpenWebBrowser.Index = 1;
			this.menuItemFileOpenWebBrowser.Text = "画像をWebブラウザで開く(&W)";
			this.menuItemFileOpenWebBrowser.Click += new System.EventHandler(this.menuItemFileOpenWebBrowser_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "-";
			// 
			// menuItemFileQuickSave
			// 
			this.menuItemFileQuickSave.Index = 3;
			this.menuItemFileQuickSave.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem6});
			this.menuItemFileQuickSave.Text = "指定フォルダ(&H)";
			this.menuItemFileQuickSave.Popup += new System.EventHandler(this.menuItemFileQuickSave_Popup);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 0;
			this.menuItem6.Text = "dummy";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 4;
			this.menuItem7.Text = "-";
			// 
			// menuItemFileSave
			// 
			this.menuItemFileSave.Index = 5;
			this.menuItemFileSave.Text = "別名で保存(&S)...";
			this.menuItemFileSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
			// 
			// menuItemFileSavePackage
			// 
			this.menuItemFileSavePackage.Index = 6;
			this.menuItemFileSavePackage.Text = "まとめてフォルダに保存(&F)...";
			this.menuItemFileSavePackage.Click += new System.EventHandler(this.menuItemFileSavePackage_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 7;
			this.menuItem10.Text = "-";
			// 
			// menuItemFileCacheView
			// 
			this.menuItemFileCacheView.Index = 8;
			this.menuItemFileCacheView.Text = "キャッシュの一覧表示(&L)...";
			this.menuItemFileCacheView.Click += new System.EventHandler(this.menuItemFileCacheView_Click);
			// 
			// menuItemFileCacheDel
			// 
			this.menuItemFileCacheDel.Index = 9;
			this.menuItemFileCacheDel.Text = "すべてのキャッシュを削除(&D)";
			this.menuItemFileCacheDel.Click += new System.EventHandler(this.menuItemFileCacheDel_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 10;
			this.menuItem13.Text = "-";
			// 
			// menuItemFileRecent
			// 
			this.menuItemFileRecent.Index = 11;
			this.menuItemFileRecent.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItemFileRecentClear});
			this.menuItemFileRecent.Text = "最近閉じたURL(&R)";
			this.menuItemFileRecent.Popup += new System.EventHandler(this.menuItemFileRecent_Popup);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "dummy";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// menuItemFileRecentClear
			// 
			this.menuItemFileRecentClear.Index = 2;
			this.menuItemFileRecentClear.Text = "履歴をクリア(&C)";
			this.menuItemFileRecentClear.Click += new System.EventHandler(this.menuItemFileRecentClear_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 12;
			this.menuItem15.Text = "-";
			// 
			// menuItemFileExit
			// 
			this.menuItemFileExit.Index = 13;
			this.menuItemFileExit.Text = "閉じる(&X)";
			this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEditCopyImage,
            this.menuItemEditCopyUrl,
            this.menuItem20,
            this.menuItemEditSetNGUrl,
            this.menuItemEditNGUrl,
            this.menuItem23,
            this.menuItemEditCancelSelected,
            this.menuItemEditSelectAll});
			this.menuItemEdit.Text = "編集(&E)";
			this.menuItemEdit.Popup += new System.EventHandler(this.menuItemEdit_Popup);
			// 
			// menuItemEditCopyImage
			// 
			this.menuItemEditCopyImage.Index = 0;
			this.menuItemEditCopyImage.Text = "画像をクリップボードにコピー(&C)";
			this.menuItemEditCopyImage.Click += new System.EventHandler(this.menuItemEditCopyImage_Click);
			// 
			// menuItemEditCopyUrl
			// 
			this.menuItemEditCopyUrl.Index = 1;
			this.menuItemEditCopyUrl.Text = "URLをクリップボードにコピー(&U)";
			this.menuItemEditCopyUrl.Click += new System.EventHandler(this.menuItemEditCopyUrl_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 2;
			this.menuItem20.Text = "-";
			// 
			// menuItemEditSetNGUrl
			// 
			this.menuItemEditSetNGUrl.Index = 3;
			this.menuItemEditSetNGUrl.Text = "NGURLに指定(&N)";
			this.menuItemEditSetNGUrl.Click += new System.EventHandler(this.menuItemEditSetNGUrl_Click);
			// 
			// menuItemEditNGUrl
			// 
			this.menuItemEditNGUrl.Index = 4;
			this.menuItemEditNGUrl.Text = "NGURLを編集(&E)";
			this.menuItemEditNGUrl.Click += new System.EventHandler(this.menuItemEditNGUrl_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 5;
			this.menuItem23.Text = "-";
			// 
			// menuItemEditCancelSelected
			// 
			this.menuItemEditCancelSelected.Index = 6;
			this.menuItemEditCancelSelected.Text = "選択をキャンセル(&C)";
			this.menuItemEditCancelSelected.Click += new System.EventHandler(this.menuItemEditCancelSelected_Click);
			// 
			// menuItemEditSelectAll
			// 
			this.menuItemEditSelectAll.Index = 7;
			this.menuItemEditSelectAll.Text = "すべて選択(&S)";
			this.menuItemEditSelectAll.Click += new System.EventHandler(this.menuItemEditSelectAll_Click);
			// 
			// menuItemView
			// 
			this.menuItemView.Index = 2;
			this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemViewMosaic,
            this.menuItemViewCancelMosaicAll,
            this.menuItem8,
            this.menuItemViewReget,
            this.menuItemErrorTabRefresh,
            this.menuItem11,
            this.menuItemViewOriginalSize,
            this.menuItem29,
            this.menuItemViewGifAnime,
            this.menuItem31,
            this.menuItemViewCloseActive,
            this.menuItemViewCloseAll,
            this.menuItem35,
            this.menuItemViewClose404Tab,
            this.menuItemViewCloseError,
            this.menuItem12,
            this.menuItemViewDelCache,
            this.menuItem37,
            this.menuItemViewPrev,
            this.menuItemViewNext,
            this.menuItem58,
            this.menuItemViewTopMost,
            this.menuItemViewSettings});
			this.menuItemView.Text = "表示(&V)";
			this.menuItemView.Popup += new System.EventHandler(this.menuItemView_Popup);
			// 
			// menuItemViewMosaic
			// 
			this.menuItemViewMosaic.Index = 0;
			this.menuItemViewMosaic.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
			this.menuItemViewMosaic.Text = "モザイク(&M)";
			this.menuItemViewMosaic.Click += new System.EventHandler(this.menuItemViewMosaic_Click);
			// 
			// menuItemViewCancelMosaicAll
			// 
			this.menuItemViewCancelMosaicAll.Index = 1;
			this.menuItemViewCancelMosaicAll.Text = "すべてのモザイクを解除(&C)";
			this.menuItemViewCancelMosaicAll.Click += new System.EventHandler(this.menuItemViewCancelMosaicAll_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 2;
			this.menuItem8.Text = "-";
			// 
			// menuItemViewReget
			// 
			this.menuItemViewReget.Index = 3;
			this.menuItemViewReget.Text = "選択されている画像を再取得(&G)";
			this.menuItemViewReget.Click += new System.EventHandler(this.menuItemFileReget_Click);
			// 
			// menuItemErrorTabRefresh
			// 
			this.menuItemErrorTabRefresh.Index = 4;
			this.menuItemErrorTabRefresh.Text = "すべてのエラータブを再取得(&Y)";
			this.menuItemErrorTabRefresh.Click += new System.EventHandler(this.menuItemErrorTabRefresh_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 5;
			this.menuItem11.Text = "-";
			// 
			// menuItemViewOriginalSize
			// 
			this.menuItemViewOriginalSize.Index = 6;
			this.menuItemViewOriginalSize.Text = "原寸大表示(&O)";
			this.menuItemViewOriginalSize.Click += new System.EventHandler(this.menuItemViewOriginalSize_Click);
			// 
			// menuItem29
			// 
			this.menuItem29.Index = 7;
			this.menuItem29.Text = "-";
			// 
			// menuItemViewGifAnime
			// 
			this.menuItemViewGifAnime.Index = 8;
			this.menuItemViewGifAnime.Text = "GIFアニメーションを開始(&G)";
			this.menuItemViewGifAnime.Click += new System.EventHandler(this.menuItemViewGifAnime_Click);
			// 
			// menuItem31
			// 
			this.menuItem31.Index = 9;
			this.menuItem31.Text = "-";
			// 
			// menuItemViewCloseActive
			// 
			this.menuItemViewCloseActive.Index = 10;
			this.menuItemViewCloseActive.Text = "選択されているタブを閉じる(&A)";
			this.menuItemViewCloseActive.Click += new System.EventHandler(this.menuItemViewCloseSelected_Click);
			// 
			// menuItemViewCloseAll
			// 
			this.menuItemViewCloseAll.Index = 11;
			this.menuItemViewCloseAll.Text = "すべてのタブを閉じる(&W)";
			this.menuItemViewCloseAll.Click += new System.EventHandler(this.menuItemViewCloseAll_Click);
			// 
			// menuItem35
			// 
			this.menuItem35.Index = 12;
			this.menuItem35.Text = "-";
			// 
			// menuItemViewClose404Tab
			// 
			this.menuItemViewClose404Tab.Index = 13;
			this.menuItemViewClose404Tab.Text = "404エラーのタブを閉じる(&U)";
			this.menuItemViewClose404Tab.Click += new System.EventHandler(this.menuItemViewClose404Tab_Click);
			// 
			// menuItemViewCloseError
			// 
			this.menuItemViewCloseError.Index = 14;
			this.menuItemViewCloseError.Text = "エラータブをすべて閉じる(&E)";
			this.menuItemViewCloseError.Click += new System.EventHandler(this.menuItemViewCloseError_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 15;
			this.menuItem12.Text = "-";
			// 
			// menuItemViewDelCache
			// 
			this.menuItemViewDelCache.Index = 16;
			this.menuItemViewDelCache.Text = "キャッシュを削除して閉じる(&D)";
			this.menuItemViewDelCache.Click += new System.EventHandler(this.menuItemViewDelCache_Click);
			// 
			// menuItem37
			// 
			this.menuItem37.Index = 17;
			this.menuItem37.Text = "-";
			// 
			// menuItemViewPrev
			// 
			this.menuItemViewPrev.Index = 18;
			this.menuItemViewPrev.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.menuItemViewPrev.Text = "前のタブを表示(&P)";
			this.menuItemViewPrev.Click += new System.EventHandler(this.menuItemViewPrev_Click);
			// 
			// menuItemViewNext
			// 
			this.menuItemViewNext.Index = 19;
			this.menuItemViewNext.Shortcut = System.Windows.Forms.Shortcut.F3;
			this.menuItemViewNext.Text = "次のタブを表示(&N)";
			this.menuItemViewNext.Click += new System.EventHandler(this.menuItemViewNext_Click);
			// 
			// menuItem58
			// 
			this.menuItem58.Index = 20;
			this.menuItem58.Text = "-";
			// 
			// menuItemViewTopMost
			// 
			this.menuItemViewTopMost.Index = 21;
			this.menuItemViewTopMost.Text = "常に最前面に表示(&T)";
			this.menuItemViewTopMost.Click += new System.EventHandler(this.menuItemViewTopMost_Click);
			// 
			// menuItemViewSettings
			// 
			this.menuItemViewSettings.Index = 22;
			this.menuItemViewSettings.Text = "環境設定(&O)...";
			this.menuItemViewSettings.Click += new System.EventHandler(this.menuItemViewSettings_Click);
			// 
			// menuItem40
			// 
			this.menuItem40.Index = 3;
			this.menuItem40.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem41});
			this.menuItem40.Text = "ヘルプ(&H)";
			// 
			// menuItem41
			// 
			this.menuItem41.Index = 0;
			this.menuItem41.Text = "v1.0";
			// 
			// contextMenuImage
			// 
			this.contextMenuImage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemImageOpenWebBrowser,
            this.menuItem54,
            this.menuItemImageQuickSave,
            this.menuItem3,
            this.menuItemImageSave,
            this.menuItemImageSavePackage,
            this.menuItem55,
            this.menuItemImageCopy,
            this.menuItemImageCopyUrl,
            this.menuItem56,
            this.menuItemImageMosaic,
            this.menuItemImageNGUrl,
            this.menuItem5,
            this.menuItemImageReget});
			this.contextMenuImage.Popup += new System.EventHandler(this.contextMenuImage_Popup);
			// 
			// menuItemImageOpenWebBrowser
			// 
			this.menuItemImageOpenWebBrowser.Index = 0;
			this.menuItemImageOpenWebBrowser.Text = "Webブラウザで開く(&W)";
			this.menuItemImageOpenWebBrowser.Click += new System.EventHandler(this.menuItemImageOpenWebBrowser_Click);
			// 
			// menuItem54
			// 
			this.menuItem54.Index = 1;
			this.menuItem54.Text = "-";
			// 
			// menuItemImageQuickSave
			// 
			this.menuItemImageQuickSave.Index = 2;
			this.menuItemImageQuickSave.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem44});
			this.menuItemImageQuickSave.Text = "指定フォルダ(&H)";
			this.menuItemImageQuickSave.Popup += new System.EventHandler(this.menuItemImageQuickSave_Popup);
			// 
			// menuItem44
			// 
			this.menuItem44.Index = 0;
			this.menuItem44.Text = "dummy";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "-";
			// 
			// menuItemImageSave
			// 
			this.menuItemImageSave.Index = 4;
			this.menuItemImageSave.Text = "別名で保存(&S)...";
			this.menuItemImageSave.Click += new System.EventHandler(this.menuItemImageSave_Click);
			// 
			// menuItemImageSavePackage
			// 
			this.menuItemImageSavePackage.Index = 5;
			this.menuItemImageSavePackage.Text = "まとめてフォルダに保存(&F)...";
			this.menuItemImageSavePackage.Click += new System.EventHandler(this.menuItemImageSavePackage_Click);
			// 
			// menuItem55
			// 
			this.menuItem55.Index = 6;
			this.menuItem55.Text = "-";
			// 
			// menuItemImageCopy
			// 
			this.menuItemImageCopy.Index = 7;
			this.menuItemImageCopy.Text = "画像をクリップボードにコピー(&C)";
			this.menuItemImageCopy.Click += new System.EventHandler(this.menuItemImageCopy_Click);
			// 
			// menuItemImageCopyUrl
			// 
			this.menuItemImageCopyUrl.Index = 8;
			this.menuItemImageCopyUrl.Text = "URLをクリップボードにコピー(&U)";
			this.menuItemImageCopyUrl.Click += new System.EventHandler(this.menuItemImageCopyUrl_Click);
			// 
			// menuItem56
			// 
			this.menuItem56.Index = 9;
			this.menuItem56.Text = "-";
			// 
			// menuItemImageMosaic
			// 
			this.menuItemImageMosaic.Index = 10;
			this.menuItemImageMosaic.Text = "モザイク(&M)";
			this.menuItemImageMosaic.Click += new System.EventHandler(this.menuItemImageMosaic_Click);
			// 
			// menuItemImageNGUrl
			// 
			this.menuItemImageNGUrl.Index = 11;
			this.menuItemImageNGUrl.Text = "NGURLに指定(&N)";
			this.menuItemImageNGUrl.Click += new System.EventHandler(this.menuItemImageNGUrl_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 12;
			this.menuItem5.Text = "-";
			// 
			// menuItemImageReget
			// 
			this.menuItemImageReget.Index = 13;
			this.menuItemImageReget.Text = "画像を再取得(&D)";
			this.menuItemImageReget.Click += new System.EventHandler(this.menuItemItemImageReget_Click);
			// 
			// tabControl
			// 
			this.tabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(385, 130);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl.TabIndex = 0;
			this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl_KeyDown);
			this.tabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl_MouseDown);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 130);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarUrl,
            this.statusBarMessage,
            this.statusBarIsSaved,
            this.statusBarByteCount,
            this.statusBarSize});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(385, 21);
			this.statusBar.TabIndex = 1;
			// 
			// statusBarUrl
			// 
			this.statusBarUrl.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarUrl.Name = "statusBarUrl";
			this.statusBarUrl.Width = 169;
			// 
			// statusBarMessage
			// 
			this.statusBarMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarMessage.Name = "statusBarMessage";
			this.statusBarMessage.Width = 169;
			// 
			// statusBarIsSaved
			// 
			this.statusBarIsSaved.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusBarIsSaved.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.statusBarIsSaved.Name = "statusBarIsSaved";
			this.statusBarIsSaved.Width = 10;
			// 
			// statusBarByteCount
			// 
			this.statusBarByteCount.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusBarByteCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.statusBarByteCount.Name = "statusBarByteCount";
			this.statusBarByteCount.ToolTipText = "画像のデータサイズを表します";
			this.statusBarByteCount.Width = 10;
			// 
			// statusBarSize
			// 
			this.statusBarSize.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusBarSize.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.statusBarSize.Name = "statusBarSize";
			this.statusBarSize.ToolTipText = "画像のサイズ(幅,高さ)を表します";
			this.statusBarSize.Width = 10;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.FileName = "image1";
			this.saveFileDialog.Filter = "Image Files (*.jpg,*.gif,*.png,*.bmp)|*.jpg;*.jpeg;*.jpe;*.gif;*.png;*.bmp|All Fi" +
    "les (*.*)|*.*";
			// 
			// contextMenuTab
			// 
			this.contextMenuTab.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTabClose,
            this.menuItemTabCloseExceptThis,
            this.menuItem52,
            this.menuItemTabCloseError,
            this.menuItemTabDelCache,
            this.menuItem53,
            this.menuItemTabCloseAll});
			this.contextMenuTab.Popup += new System.EventHandler(this.contextMenuTab_Popup);
			// 
			// menuItemTabClose
			// 
			this.menuItemTabClose.Index = 0;
			this.menuItemTabClose.Text = "このタブを閉じる(&A)";
			this.menuItemTabClose.Click += new System.EventHandler(this.menuItemTabClose_Click);
			// 
			// menuItemTabCloseExceptThis
			// 
			this.menuItemTabCloseExceptThis.Index = 1;
			this.menuItemTabCloseExceptThis.Text = "これ以外のタブを閉じる(&O)";
			this.menuItemTabCloseExceptThis.Click += new System.EventHandler(this.menuItemTabCloseExceptThis_Click);
			// 
			// menuItem52
			// 
			this.menuItem52.Index = 2;
			this.menuItem52.Text = "-";
			// 
			// menuItemTabCloseError
			// 
			this.menuItemTabCloseError.Index = 3;
			this.menuItemTabCloseError.Text = "エラータブをすべて閉じる(&E)";
			this.menuItemTabCloseError.Click += new System.EventHandler(this.menuItemTabCloseError_Click);
			// 
			// menuItemTabDelCache
			// 
			this.menuItemTabDelCache.Index = 4;
			this.menuItemTabDelCache.Text = "キャッシュを削除して閉じる(&D)";
			this.menuItemTabDelCache.Click += new System.EventHandler(this.menuItemTabDelCache_Click);
			// 
			// menuItem53
			// 
			this.menuItem53.Index = 5;
			this.menuItem53.Text = "-";
			// 
			// menuItemTabCloseAll
			// 
			this.menuItemTabCloseAll.Index = 6;
			this.menuItemTabCloseAll.Text = "すべてのタブを閉じる(&X)";
			this.menuItemTabCloseAll.Click += new System.EventHandler(this.menuItemTabCloseAll_Click);
			// 
			// ImageViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(385, 151);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.statusBar);
			this.Menu = this.mainMenu;
			this.Name = "ImageViewer";
			this.Text = "ImageViewer";
			this.Closed += new System.EventHandler(this.ImageViewer_Closed);
			((System.ComponentModel.ISupportInitialize)(this.statusBarUrl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMessage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarIsSaved)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarByteCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarSize)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// ステータスバーの状態を更新
		/// </summary>
		private void UpdateStatusBar()
		{
			WindowInfo info = SelectedWindow;

			if (info != null)
			{
				SetStatusText(info.StatusText);
			}

			if (info != null && info.CacheInfo != null)
			{
				this.Text = "ImageViewer [" + Path.GetFileName(info.Url) + "]";
				if (info.ImagePanel.CanAnimate) this.Text += " $";
				// 画像の元URL
				statusBarUrl.Text = info.Url;
				// 画像のファイルサイズ
				string sizeText;
				float sizeKB = info.CacheInfo.Length / 1024f;
				if (sizeKB < 1024) sizeText = ((int)sizeKB) + "KB";
				else sizeText = sizeText =  Math.Round(sizeKB / 1024, 1) + "MB";
				statusBarByteCount.Text = sizeText;
				// 画像のサイズ
				Size size = info.Loaded ? info.ImagePanel.Image.Size : Size.Empty;
				statusBarSize.Text = String.Format("{0},{1}", size.Width, size.Height);
				statusBarIsSaved.Text = info.CacheInfo.IsSaved ? "保存済み" : "未保存";
			}
			else {
				this.Text = "ImageViewer";
				statusBarByteCount.Text =
					statusBarSize.Text =
					statusBarUrl.Text = statusBarIsSaved.Text = String.Empty;
			}
		}

		private void SetStatusText(string text)
		{
			statusBarMessage.Text = String.IsNullOrEmpty(text) ? String.Empty : text;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void OpenUrls(string[] urls)
		{
			if (urls == null)
				throw new ArgumentNullException("urls");

			foreach (string u in urls)
				OpenUrl(u, false);
		}

		/// <summary>
		/// 指定したURLの画像を開く
		/// </summary>
		/// <param name="url"></param>
		/// <param name="selection">作成されたタブを選択するかどうか</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void OpenUrl(string url, bool selection)
		{
			if (String.IsNullOrEmpty(url))
				return;

			// NGパターンチェック
			if (nGUrls.IsMatch(url) && MessageBox.Show(url + "\r\nこのURLは登録されているNGパターンに一致します。本当に開きますか？", "確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}

			// 既に指定されたURLが開かれている場合は、
			// そのタブをアクティブにする
			int index = FindWindow(url);

			if (index >= 0)
			{
				tabControl.SelectedIndex = index;
			}
			// 新規タブを作成
			else {
				ImagePanel panel = new ImagePanel();
				panel.Dock = DockStyle.Fill;
				panel.ContextMenu = contextMenuImage;
				panel.Mosaic = settings.Mosaic;
				panel.IsOriginalSize = settings.ViewOriginalSize;

				WindowInfo info = new WindowInfo(panel, url);
				info.SelectedChanged += new EventHandler(OnWindowSelectedChanged);

				TabPage tab = new TabPage();
				tab.Controls.Add(panel);
				tab.Tag = info;

				tabControl.TabPages.Add(tab);
				windowList.Add(info);
				
				if (selection)
				{
					tabControl.SelectedTab = tab;
					SingleSelection(info);
				}

				imageCache.Load(url, OnImageCompleted);
			}

			if (settings.Activate)
				BringToFront();
		}

		/// <summary>
		/// 選択されている画像を別名で保存
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void SaveImage()
		{
			foreach (WindowInfo info in Selections)
			{
				if (info.CacheInfo != null && !info.Error)
				{
					saveFileDialog.FileName =
						PathUtil.ReplaceInvalidPathChars(Path.GetFileName(info.Url), "_");

					if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
					{
						imageCache.Copy(info.CacheInfo, saveFileDialog.FileName);
						info.CacheInfo.IsSaved = true;
						imageCache.Indexer.SaveCacheInfo(info.CacheInfo);
					}
				}
			}
			UpdateStatusBar();
		}

		/// <summary>
		/// 指定したフォルダに画像を保存
		/// </summary>
		/// <param name="folder"></param>
		private void SaveImage(QuickSaveFolderItem folder)
		{
			SaveImageInternal(folder.FolderPath, true);
		}

		/// <summary>
		/// 画像をまとめて保存
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void SavePackage()
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.Description = "保存先のフォルダを指定してください";

			if (dlg.ShowDialog(this) == DialogResult.OK)
				SaveImageInternal(dlg.SelectedPath, false);
		}

		/// <summary>
		/// 選択されている画像を指定したフォルダに保存
		/// </summary>
		/// <param name="folderPath"></param>
		/// <param name="selonly">選択されている画像のみ保存する場合はtrue、すべて保存する場合はfalse</param>
		private void SaveImageInternal(string folderPath, bool selonly)
		{
			ICollection collection = selonly ? 
				(ICollection)Selections : (ICollection)windowList;

			int cnt = 0;
			foreach (WindowInfo info in collection)
			{
				if (info.CacheInfo != null && !info.Error)
				{
					string fileName = PathUtil.ReplaceInvalidPathChars(Path.GetFileName(info.Url), "_");
					string filePath = Path.Combine(folderPath, fileName);

					imageCache.Copy(info.CacheInfo, Rename(filePath));
					info.CacheInfo.IsSaved = true;
					imageCache.Indexer.SaveCacheInfo(info.CacheInfo);
					cnt++;
				}
			}

			UpdateStatusBar();
			SetStatusText(cnt + " 個の画像を保存しました。");
		}

		/// <summary>
		/// 選択されている画像のモザイクを切り換える
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Mosaic()
		{
			foreach (WindowInfo info in Selections)
				info.ImagePanel.Mosaic = !info.ImagePanel.Mosaic;
		}

		/// <summary>
		/// 原寸大で表示するかどうかを切り替える
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void ViewOriginalSize()
		{
			foreach (WindowInfo info in Selections)
				info.ImagePanel.IsOriginalSize = !info.ImagePanel.IsOriginalSize;
		}

		/// <summary>
		/// すべてのモザイクを解除
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void CancelMosaic()
		{
			foreach (WindowInfo info in windowList)
			{
				info.ImagePanel.Mosaic = false;
			}
		}

		/// <summary>
		/// 画像のアニメーションを開始
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void GifAnimate()
		{
			try {
				foreach (WindowInfo info in Selections)
					if (info.ImagePanel.CanAnimate)
						info.ImagePanel.Animate();
			}
			catch (Exception ex) 
			{
				MessageBox.Show("GIFアニメーションに失敗しました\r\n\r\n" + ex.ToString(), "GIFアニメーションエラー",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// NGURLのパターンを追加
		/// </summary>
		/// <param name="pattern"></param>
		private void AddNGUrl(string pattern)
		{
			nGUrls.Add(pattern);
		}

		/// <summary>
		/// NGURLを編集するダイアログを表示
		/// </summary>
		private void EditNGUrl()
		{
			NGUrlEditorDialog dlg = new NGUrlEditorDialog();
			dlg.Patterns =  nGUrls.Patterns;

			if (dlg.ShowDialog(this) == DialogResult.OK)
				nGUrls.Patterns = dlg.Patterns;
		}

		/// <summary>
		/// 指定したURLを開いているタブのインデックスを取得
		/// </summary>
		/// <param name="url"></param>
		/// <returns>見つかればタブのインデックス、見つからなければ-1を返す</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public int FindWindow(string url)
		{
			for (int i = 0; i < windowList.Count; i++)
			{
				WindowInfo info = (WindowInfo)windowList[i];
				// 大文字小文字は区別しないで比較
				if (String.Compare(info.Url, url, true) == 0)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// アクティブな画像を閉じる
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void CloseImage(bool del)
		{
			if (windowList.Count > 0)
			{
				WindowInfo info = SelectedWindow;
				CloseImageInternal(info, del);
			}
		}

		/// <summary>
		/// 選択されている画像を閉じる
		/// </summary>
		/// <param name="del"></param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void CloseSelectedImages(bool del)
		{
			WindowInfo[] selection = Selections;

			for (int i = 0; i < selection.Length; i++)
				CloseImageInternal(selection[i], del);
		}

		/// <summary>
		/// すべての画像を閉じる
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void CloseImageAll()
		{
			foreach (WindowInfo info in windowList)
			{
				info.ImagePanel.Dispose();
				if (info.CacheInfo != null && !info.Error)
					AddRecentUrl(info.CacheInfo.Url);
			}

			windowList.Clear();
			tabControl.TabPages.Clear();

			foreach (Image image in imageList.Images)
				image.Dispose();

			imageList.Images.Clear();
			imageList.Images.Add(ImageUtil.GetErrorImage());

			imageCache.Abort();

			if (settings.AutoHide)
				Hide();
		}

		/// <summary>
		/// エラーな画像をすべて閉じる
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void CloseErrorImages()
		{
			ArrayList arrayList = new ArrayList();

			foreach (WindowInfo info in windowList)
			{
				if (info.Error)
					arrayList.Add(info);
			}

			tabControl.Visible = false;

			foreach (WindowInfo info in arrayList)
			{
				int index = windowList.IndexOf(info);
				TabPage tab = tabControl.TabPages[index];

				tabControl.TabPages.RemoveAt(index);
				windowList.RemoveAt(index);

				tab.Dispose();
			}

			tabControl.Visible = true;
		}

		/// <summary>
		/// 404エラーな画像をすべて閉じる
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Close404ErrorTabs()
		{
			var list = new List<WindowInfo>();
			foreach (WindowInfo info in windowList)
			{
				if (info.StatusText.Contains("404") || info.StatusText.Contains("NotFound"))
					list.Add(info);
			}

			tabControl.Visible = false;

			foreach (WindowInfo info in list)
			{
				int index = windowList.IndexOf(info);
				TabPage tab = tabControl.TabPages[index];		
				tabControl.TabPages.RemoveAt(index);
				windowList.RemoveAt(index);
				tab.Dispose();
			}

			tabControl.Visible = true;
		}

		/// <summary>
		/// 指定したウインドウ以外を閉じる
		/// </summary>
		private void CloseImageExceptThis(WindowInfo except)
		{
			tabControl.Visible = false;
			int idx = windowList.Count-1;

			while (idx >= 0)
			{
				TabPage tab = tabControl.TabPages[idx];
				WindowInfo info = (WindowInfo)tab.Tag;

				if (info != except)
				{
					windowList.Remove(info);
					tabControl.TabPages.Remove(tab);

					if (tab.ImageIndex > 0)
					{
						Image image = imageList.Images[tab.ImageIndex];
						image.Dispose();
					}

					tab.Dispose();
					info.ImagePanel.Dispose();

					imageCache.Abort(info.Url);
				}
				idx--;
			}
			tabControl.Visible = true;
		}

		/// <summary>
		/// 指定したウインドウを閉じる
		/// </summary>
		/// <param name="info"></param>
		/// <param name="del"></param>
		private void CloseImageInternal(WindowInfo info, bool del)
		{
			int index = windowList.IndexOf(info);

#if DEBUG
			if (index == -1)
				throw new ArgumentException("指定したウインドウは存在しません");
#else
			if (index == -1)
				return;
#endif

			info.ImagePanel.StopAnimation();

			TabPage tab = tabControl.TabPages[index];
			TabPage sel = tabControl.SelectedTab;

			int imageIndex = tab.ImageIndex;

			// 画像のサムネイルを放棄
			// (RemoveしてしまうとタブのImageIndexがずれてしまうので放棄するだけ)
			if (imageIndex > 0)
				imageList.Images[imageIndex].Dispose();

			windowList.RemoveAt(index);
			tabControl.TabPages.RemoveAt(index);

			tab.Dispose();
			info.ImagePanel.Dispose();

			imageCache.Abort(info.Url);

			// キャッシュを削除する
			if (del)
				imageCache.Delete(info.CacheInfo);

			// 閉じる前に選択されていたタブを再度選択する
			if (tab != sel)
				tabControl.SelectedTab = sel;

			if (windowList.Count == 0 && settings.AutoHide)
				Hide();

			if (info.CacheInfo != null && !info.Error)
				AddRecentUrl(info.CacheInfo.Url);
		}

		/// <summary>
		/// 他の選択をキャンセルしてから選択
		/// </summary>
		/// <param name="info"></param>
		private void SingleSelection(WindowInfo info)
		{
			CancelSelection();
			info.Selected = true;
		}

		/// <summary>
		/// 選択をすべて解除
		/// </summary>
		private void CancelSelection()
		{
			foreach (WindowInfo info in windowList)
				info.Selected = false;
		}

		/// <summary>
		/// すべてを選択
		/// </summary>
		private void SelectAll()
		{
			foreach (WindowInfo info in windowList)
				info.Selected = true;
		}

		private void CreateQuickSaveMenues(MenuItem parent)
		{
			parent.MenuItems.Clear();

			foreach (QuickSaveFolderItem item in settings.QuickSaveFolders)
			{
				MenuItem menu = new MenuItem(item.Title);
				menu.Click += new EventHandler(menuItemFileQuickSave_Click);
				menu.Shortcut = item.Shortcut;
				parent.MenuItems.Add(menu);
			}

			if (parent.MenuItems.Count == 0)
			{
				MenuItem nashi = new MenuItem("なし");
				nashi.Enabled = false;
				parent.MenuItems.Add(nashi);
			}
		}

		/// <summary>
		/// アクティブな画像のデータをコピー
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void CopyImage()
		{
			WindowInfo info = SelectedWindow;

			if (info.Loaded)
				Clipboard.SetDataObject(info.ImagePanel.Image, true);
		}

		/// <summary>
		/// 選択されている画像のURLをコピー
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void CopyUrl()
		{
			StringBuilder sb = new StringBuilder();

			foreach (WindowInfo info in Selections) {
				sb.Append(info.Url);
				sb.Append(Environment.NewLine);
			}

			Clipboard.SetDataObject(sb.ToString(), true);
		}

		/// <summary>
		/// すべてのキャッシュを削除
		/// </summary>
		private void ClearCache()
		{
			if (windowList.Count > 0)
				throw new InvalidOperationException();

			imageCache.Indexer.Clear();
		}

		/// <summary>
		/// oldFileName と同名のファイルが存在したら別名にリネームして返す
		/// </summary>
		/// <param name="oldFileName"></param>
		/// <returns></returns>
		private string Rename(string oldFileName)
		{
			if (settings.NoOverwirte && File.Exists(oldFileName))
			{
				string dir = Path.GetDirectoryName(oldFileName);
				string fileNameOnly = Path.GetFileNameWithoutExtension(oldFileName);
				string extension = Path.GetExtension(oldFileName);

				for (int cnt = 1;; cnt++)
				{
					string newFileName = String.Format("{0}[{1}]{2}",
						fileNameOnly, cnt, extension);

					newFileName = Path.Combine(dir, newFileName);

					if (!File.Exists(newFileName))
					{
						return newFileName;
					}
				}
			}
			else {
				return oldFileName;
			}
		}

		private void AddRecentUrl(string url)
		{
			settings.RecentUrl.Remove(url);
			settings.RecentUrl.Insert(0, url);

			if (settings.RecentUrl.Count > maxRecentUrl)
				settings.RecentUrl.RemoveAt(maxRecentUrl);
		}

		private void RegetSelectionImages()
		{
			foreach (WindowInfo info in Selections)
			{
				if (info.CacheInfo != null && (info.Error || info.Loaded))
				{
					info.ImagePanel.Unload();
					imageCache.Delete(info.CacheInfo);
					imageCache.Load(info.CacheInfo.Url, OnImageCompleted);
				}
			}
		}

		private void RegetErrorImages()
		{
			foreach (WindowInfo info in windowList)
			{
				if (info.CacheInfo != null && info.Error &&
					!info.StatusText.Contains("404"))
				{
					info.ImagePanel.Unload();
					imageCache.Delete(info.CacheInfo);
					imageCache.Load(info.CacheInfo.Url, OnImageCompleted);
				}
			}
		}


		#region Formイベント
		private void ImageViewer_Closed(object sender, System.EventArgs e)
		{
			settings.NGUrlPattern = nGUrls.Patterns;
			settings.WindowBounds = new Rectangle(Location, ClientSize);

			ImageViewerSettings.Save(ImageViewerSettings.SettingPath, settings);
		}
		#endregion

		#region WindowInfoイベント
		private void OnWindowSelectedChanged(object sender, EventArgs e)
		{
			WindowInfo info = (WindowInfo)sender;
			int index = windowList.IndexOf(info);

			if (index >= 0 && index < tabControl.TabCount)
			{
				tabControl.Invalidate(tabControl.GetTabRect(index));
				tabControl.Update();
			}
		}
		#endregion

		#region ImageCacheイベント

		// 読み込みに失敗したときは×印の画像を設定
		private void OnImageCompleted(object sender, ImageCacheEventArgs e)
		{
			MethodInvoker m = delegate
			{
				if (e.Status == ImageCacheStatus.Downloaded ||
					e.Status == ImageCacheStatus.CacheExist)
				{
					ImageCache_Success(e);
				}

				else if (e.Status == ImageCacheStatus.Error)
				{
					ImageCache_Error(e);
				}
				UpdateStatusBar();
			};
			Invoke(m);
		}

		private void ImageCache_Success(ImageCacheEventArgs e)
		{
			int index = FindWindow(e.CacheInfo.Url);
			if (index >= 0)
			{
				// 画像のサムネイルを作成
				Image thumb = ImageUtil.GetThumbnailImage(e.Image,
					settings.TabImageSize, Color.White);

				// イメージリストに追加
				int imageIndex = imageList.Images.Add(thumb, Color.White);

				// タブに作成した画像を設定
				TabPage tab = tabControl.TabPages[index];
				tab.ImageIndex = imageIndex;

				// 画像パネルに読み込む
				WindowInfo info = (WindowInfo)tab.Tag;
				info.CacheInfo = e.CacheInfo;
				info.Error = false;

				if (e.Status == ImageCacheStatus.CacheExist)
					info.StatusText = "キャッシュから読み込みました。";

				else if (e.Status == ImageCacheStatus.Downloaded)
					info.StatusText = "新規ダウンロード画像です。";
				
				// アクティブでなければ、メモリ節約のために読み込まない
				if (isSavingMemory && index == tabControl.SelectedIndex ||
					isSavingMemory == false && info.CacheInfo.FileName != null)
				{
					info.ImagePanel.Load(info.CacheInfo.FileName);
				}
			}
		}

		private void ImageCache_Error(ImageCacheEventArgs e)
		{
			int index = FindWindow(e.CacheInfo.Url);
			if (index >= 0)
			{
				// エラー用のサムネイル画像を設定
				TabPage tab = tabControl.TabPages[index];
				tab.ImageIndex = 0;

				WindowInfo info = (WindowInfo)tab.Tag;
				if (e.Exception != null)
					info.StatusText = e.StatusCode + ", " + e.Exception.Message;
				else
					info.StatusText = e.StatusCode.ToString();

				info.CacheInfo = e.CacheInfo;
				info.Error = true;
			}
		}

		#endregion

		#region TabControlイベント
		private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			TabPage tab = tabControl.TabPages[e.Index];
			WindowInfo info = (WindowInfo)tab.Tag;

			if (tab.ImageIndex != -1)
			{
				Image image = imageList.Images[tab.ImageIndex];
				g.FillRectangle(SystemBrushes.Control, e.Bounds);
				g.DrawImage(image, e.Bounds);

				if (info.Selected)
				{
					using (Brush brush = new SolidBrush(Color.FromArgb(100, SystemColors.Highlight)))
						g.FillRectangle(brush, e.Bounds);
				}
			}
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WindowInfo info = SelectedWindow;
			
			if (isSavingMemory && prevActiveWindow != null)
			{
				if (prevActiveWindow.CacheInfo != null && !prevActiveWindow.Error)
					prevActiveWindow.ImagePanel.Unload();
			}

			prevActiveWindow = info;

			if (info != null)
			{
				if ((ModifierKeys & Keys.Control) != 0)
				{
					info.Selected = !info.Selected;
				}
				else {
					SingleSelection(info);
				}

				if (isSavingMemory && info.CacheInfo != null && !info.Error)
				{
					info.ImagePanel.Load(info.CacheInfo.FileName);
				}
			}
			UpdateStatusBar();
		}

		private void tabControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TabPage tab = TabUtil.GetTabPage(tabControl, MousePosition);

			if (tab != null)
			{
				WindowInfo info = (WindowInfo)tab.Tag;

				// ホイールクリックなら閉じる
				if (e.Button == MouseButtons.Middle)
				{
					CloseImageInternal(info, false);
				}
				// 右クリックならコンテキストメニューを表示
				else if (e.Button == MouseButtons.Right)
				{
					contextMenuTab.Show(tabControl, new Point(e.X, e.Y));
				}
			}
		}
		#endregion

		#region ファイルメニュー
		private void menuItemFile_Popup(object sender, System.EventArgs e)
		{
			bool isSelect = tabControl.SelectedTab != null;

			menuItemFileOpenWebBrowser.Enabled =
				menuItemFileQuickSave.Enabled =
				menuItemFileSave.Enabled =
				menuItemFileSavePackage.Enabled = isSelect;
		}

		private void menuItemFileOpenUrl_Click(object sender, System.EventArgs e)
		{
			OpenUrlDialog dlg = new OpenUrlDialog();
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				foreach (string url in dlg.Urls)
					OpenUrl(url.Trim(), true);
			}
		}

		private void menuItemFileOpenWebBrowser_Click(object sender, System.EventArgs e)
		{
			if (File.Exists(settings.WebBrowserPath))
			{
				foreach (WindowInfo info in Selections)
					Process.Start(settings.WebBrowserPath, info.Url);
			}
			else {
				MessageBox.Show("使用するブラウザが設定されていません");
			}
		}

		private void menuItemFileQuickSave_Popup(object sender, System.EventArgs e)
		{
			CreateQuickSaveMenues(menuItemFileQuickSave);
		}

		private void menuItemFileQuickSave_Click(object sender, EventArgs e)
		{
			MenuItem menu = (MenuItem)sender;
			QuickSaveFolderItem item = (QuickSaveFolderItem)settings.QuickSaveFolders[menu.Index];

			SaveImage(item);
		}

		private void menuItemFileSave_Click(object sender, System.EventArgs e)
		{
			SaveImage();
		}

		private void menuItemFileSavePackage_Click(object sender, System.EventArgs e)
		{
			SavePackage();
		}

		private void menuItemFileCacheView_Click(object sender, System.EventArgs e)
		{
			CacheViewDialog dlg = new CacheViewDialog(this);
			dlg.Owner = this;
			dlg.Show();
		}

		private void menuItemFileCacheDel_Click(object sender, System.EventArgs e)
		{
			if (windowList.Count > 0)
			{
				DialogResult r = MessageBox.Show("その前にすべての画像を閉じますがよろしいですか", "よろしいですか",
					MessageBoxButtons.YesNo, MessageBoxIcon.Information);

				if (r == DialogResult.No)
					return;

				CloseImageAll();
			}

			if (MessageBox.Show("削除したキャッシュは復元できません。よろしいですか？", "削除確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				ClearCache();
			}
		}

		private void menuItemFileRecent_Popup(object sender, System.EventArgs e)
		{
			menuItemFileRecent.MenuItems.Clear();

			foreach (string url in settings.RecentUrl)
			{
				MenuItem menu = new MenuItem(url);
				menu.Click += new EventHandler(menuItemFileRecent_Click);
				menuItemFileRecent.MenuItems.Add(menu);
			}

			menuItemFileRecent.MenuItems.Add(new MenuItem("-"));
			menuItemFileRecent.MenuItems.Add(menuItemFileRecentClear);
		}

		private void menuItemFileRecent_Click(object sender, EventArgs e)
		{
			MenuItem menu = (MenuItem)sender;
			string url = (string)settings.RecentUrl[menu.Index];

			OpenUrl(url, true);
		}

		private void menuItemFileRecentClear_Click(object sender, System.EventArgs e)
		{
			settings.RecentUrl.Clear();
		}

		private void menuItemFileExit_Click(object sender, System.EventArgs e)
		{
			Close();
		}
		#endregion

		#region 編集メニュー
		private void menuItemEdit_Popup(object sender, System.EventArgs e)
		{
			bool isSelected = tabControl.SelectedTab != null;

			menuItemEditCopyImage.Enabled =
				menuItemEditCopyUrl.Enabled =
				menuItemEditSetNGUrl.Enabled = 
				menuItemEditSelectAll.Enabled =
				menuItemEditCancelSelected.Enabled = isSelected;
		}

		private void menuItemEditCopyImage_Click(object sender, System.EventArgs e)
		{
			CopyImage();
		}

		private void menuItemEditCopyUrl_Click(object sender, System.EventArgs e)
		{
			CopyUrl();
		}

		private void menuItemEditSetNGUrl_Click(object sender, System.EventArgs e)
		{
			AddNGUrl(SelectedWindow.Url);
		}

		private void menuItemEditNGUrl_Click(object sender, System.EventArgs e)
		{
			EditNGUrl();
		}

		private void menuItemEditCancelSelected_Click(object sender, System.EventArgs e)
		{
			CancelSelection();
		}

		private void menuItemEditSelectAll_Click(object sender, System.EventArgs e)
		{
			SelectAll();
		}
		#endregion

		#region 表示メニュー
		private void menuItemView_Popup(object sender, System.EventArgs e)
		{
			bool isSelected = tabControl.SelectedTab != null;

			foreach (MenuItem menu in menuItemView.MenuItems)
				menu.Enabled = isSelected;

			if (isSelected)
			{
				WindowInfo info = SelectedWindow;
				menuItemViewGifAnime.Enabled = info.ImagePanel.CanAnimate;
				menuItemViewMosaic.Checked = info.ImagePanel.Mosaic;
				menuItemViewOriginalSize.Checked = info.ImagePanel.IsOriginalSize;
			}

			menuItemViewSettings.Enabled =
				menuItemViewTopMost.Enabled = true;

			menuItemViewTopMost.Checked = settings.TopMost;
		}

		private void menuItemViewMosaic_Click(object sender, System.EventArgs e)
		{
			Mosaic();
		}

		private void menuItemViewOriginalSize_Click(object sender, System.EventArgs e)
		{
			ViewOriginalSize();
		}

		private void menuItemViewCancelMosaicAll_Click(object sender, System.EventArgs e)
		{
			CancelMosaic();
		}

		private void menuItemViewGifAnime_Click(object sender, System.EventArgs e)
		{
			GifAnimate();
		}

		private void menuItemViewCloseSelected_Click(object sender, System.EventArgs e)
		{
			CloseSelectedImages(false);
		}

		private void menuItemViewCloseAll_Click(object sender, System.EventArgs e)
		{
			CloseImageAll();
		}

		private void menuItemViewCloseError_Click(object sender, System.EventArgs e)
		{
			CloseErrorImages();
		}

		private void menuItemViewDelCache_Click(object sender, System.EventArgs e)
		{
			CloseSelectedImages(true);
		}

		private void menuItemViewPrev_Click(object sender, System.EventArgs e)
		{
			int index = tabControl.SelectedIndex;

			if (index > 0) index -= 1;
			else           index = tabControl.TabCount - 1;

			tabControl.SelectedIndex = index;
		}

		private void menuItemViewNext_Click(object sender, System.EventArgs e)
		{
			int index = tabControl.SelectedIndex;

			if (index < tabControl.TabCount-1) index += 1;
			else           index = 0;

			tabControl.SelectedIndex = index;
		}

		private void menuItemViewTopMost_Click(object sender, System.EventArgs e)
		{
			TopMost = settings.TopMost = !settings.TopMost;
		}

		private void menuItemViewSettings_Click(object sender, System.EventArgs e)
		{
			SettingDialog dlg = new SettingDialog(settings, ImageCache.ServerRestrictSettings);
			if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				ImageViewerSettings.Save(ImageViewerSettings.SettingPath, settings);				
				ImageCache.ServerRestrictSettings.Save();
			}

		}

		private void menuItemViewClose404Tab_Click(object sender, EventArgs e)
		{
			Close404ErrorTabs();
		}

		private void menuItemViewCloseSavedImages_Click(object sender, EventArgs e)
		{

		}
		#endregion

		#region タブコンテキストメニュー
		private TabPage __tabPageCurrent = null;

		private void contextMenuTab_Popup(object sender, System.EventArgs e)
		{
			__tabPageCurrent = TabUtil.GetTabPage(tabControl, MousePosition);
			Trace.Assert(__tabPageCurrent != null);
		}

		private void menuItemTabClose_Click(object sender, System.EventArgs e)
		{
			CloseImageInternal((WindowInfo)__tabPageCurrent.Tag, false);
		}

		private void menuItemTabCloseExceptThis_Click(object sender, System.EventArgs e)
		{
			CloseImageExceptThis((WindowInfo)__tabPageCurrent.Tag);
		}

		private void menuItemTabDelCache_Click(object sender, System.EventArgs e)
		{
			CloseImage(true);
		}

		private void menuItemTabCloseError_Click(object sender, System.EventArgs e)
		{
			CloseErrorImages();
		}

		private void menuItemTabCloseAll_Click(object sender, System.EventArgs e)
		{
			CloseImageAll();
		}
		#endregion

		#region ImagePanelコンテキストメニュー
		private void contextMenuImage_Popup(object sender, System.EventArgs e)
		{
			ImagePanel p = SelectedWindow.ImagePanel;

			foreach (MenuItem menu in contextMenuImage.MenuItems)
				menu.Enabled = p.IsLoaded;

			menuItemImageMosaic.Checked = p.Mosaic;
		}

		private void menuItemImageOpenWebBrowser_Click(object sender, System.EventArgs e)
		{
			menuItemFileOpenWebBrowser_Click(null, null);
		}

		private void menuItemImageQuickSave_Popup(object sender, System.EventArgs e)
		{
			CreateQuickSaveMenues(menuItemImageQuickSave);
		}

		private void menuItemImageSave_Click(object sender, System.EventArgs e)
		{
			SaveImage();
		}

		private void menuItemImageSavePackage_Click(object sender, System.EventArgs e)
		{
			SavePackage();
		}

		private void menuItemImageCopy_Click(object sender, System.EventArgs e)
		{
			CopyImage();
		}

		private void menuItemImageCopyUrl_Click(object sender, System.EventArgs e)
		{
			CopyUrl();
		}

		private void menuItemImageMosaic_Click(object sender, System.EventArgs e)
		{
			Mosaic();
		}

		private void menuItemImageNGUrl_Click(object sender, System.EventArgs e)
		{
			AddNGUrl(SelectedWindow.Url);
		}
		#endregion

		private void menuItemFileReget_Click(object sender, EventArgs e)
		{
			RegetSelectionImages();
		}

		// なにかでビューアが画面外に出てしまった時の復元処理
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				this.Location = new Point(0, 0);
				this.Size = new Size(300, 300);
			}
			return base.ProcessDialogKey(keyData);
		}

		private void menuItemErrorTabRefresh_Click(object sender, EventArgs e)
		{
			RegetErrorImages();
		}

		private void menuItemItemImageReget_Click(object sender, EventArgs e)
		{
			RegetSelectionImages();
		}

		private void tabControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
				GifAnimate();
		}

		
	}
}
