using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Twin.Forms
{
	public partial class Twin2IeBrowser
	{
		#region Designer Fields
		private System.Windows.Forms.ToolBar toolBarMain;
		private System.Windows.Forms.TextBox textBoxAddress;
		private System.Windows.Forms.ToolBar toolBarGo;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolBarButton toolBarButtonGo;
		private System.Windows.Forms.Panel addressPanel;
		private RebarDotNet.RebarWrapper rebarWrapper;
		private RebarDotNet.BandWrapper bandWrapperMain;
		private RebarDotNet.BandWrapper bandWrapperAddress;
		private RebarDotNet.BandWrapper bandWrapperIButton;
		private System.Windows.Forms.Panel treePanel;
		private System.Windows.Forms.Panel listPanel;
		private System.Windows.Forms.Panel threadPanel;
		private System.Windows.Forms.Splitter splitterLeft;
		private System.Windows.Forms.Splitter splitterTop;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel statusText;
		private System.Windows.Forms.StatusBarPanel statusProgress;
		private System.Windows.Forms.StatusBarPanel statusSize;
		private System.Windows.Forms.TabControl tabControlTable;
		private System.Windows.Forms.TabControl listTabCtrl;
		private System.Windows.Forms.TabPage tabPageBoards;
		private System.Windows.Forms.TabPage tabPageBookmarks;
		private System.Windows.Forms.ImageList imageListSmallIcons;
		private System.Windows.Forms.TabControl threadTabCtrl;
		private System.Windows.Forms.Label labelBoardName;
		private System.Windows.Forms.Label labelThreadSubject;
		private System.Windows.Forms.Panel threadToolPanel;
		private System.Windows.Forms.ToolBar toolBarThread;
		private System.Windows.Forms.ToolBarButton toolBarButton7;
		private System.Windows.Forms.ToolBarButton toolBarButton12;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ToolBarButton toolBarButtonReload;
		private System.Windows.Forms.ToolBarButton toolBarButtonStop;
		private System.Windows.Forms.ToolBarButton toolBarButtonWriteRes;
		private System.Windows.Forms.ToolBarButton toolBarButtonAutoReload;
		private System.Windows.Forms.ToolBarButton toolBarButtonFind;
		private System.Windows.Forms.ToolBarButton toolBarButtonDelete;
		private System.Windows.Forms.ToolBarButton toolBarButtonClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemTools;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsInetOption;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsOption;
		private System.Windows.Forms.ToolStripMenuItem menuItemFile;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileExit;
		private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
		private System.Windows.Forms.ToolStripMenuItem menuItemView;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindow;
		private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
		private System.Windows.Forms.ToolBarButton toolBarButtonOnline;
		private System.Windows.Forms.ToolBarButton toolBarButtonSettings;
		private System.Windows.Forms.ToolStripMenuItem menuItemFilePrint;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileCloseHistory;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileOnline;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileBoardUpdate;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewToolBars;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewStatusBar;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewToolBar;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewAddressBar;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewIButton;
		private System.Windows.Forms.ToolStripMenuItem menuItemHelpOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemHelpAbout;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowThreadCloseAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowThreadCloseNotActive;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowListCloseAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowListCloseNotActive;
		private System.Windows.Forms.ToolStripMenuItem menuItemThread;
		private System.Windows.Forms.ToolStripMenuItem menuItemList;
		private System.Windows.Forms.ToolStripMenuItem menuItemListReload;
		private System.Windows.Forms.ToolStripMenuItem menuItemListStop;
		private System.Windows.Forms.ToolStripMenuItem menuItemListClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadReload;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadStop;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadPostRes;
		private System.Windows.Forms.ToolStripMenuItem menuItemNewThread;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadResExtract;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadDeleteClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadReget;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadAutoReload;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadAutoScroll;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditSearchCache;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditRegistBoard;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditNGWords;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadFind;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileHistoryClear;
		private System.Windows.Forms.ToolBarButton toolBarButton6;
		private System.Windows.Forms.ToolBarButton toolBarButtonViewChange;
		private System.Windows.Forms.ToolBarButton toolBarButtonScroll;
		private System.Windows.Forms.ContextMenuStrip contextMenuViewChange;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewChangePrev;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewChangeNext;
		private System.Windows.Forms.ContextMenuStrip contextMenuScroll;
		private System.Windows.Forms.ToolStripMenuItem menuItemScrollSetAutoScroll;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowSelectNext;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowSelectPrev;
		private System.Windows.Forms.ContextMenuStrip contextMenuRead;
		private System.Windows.Forms.ToolStripMenuItem menuItemReadReget;
		private System.Windows.Forms.ToolStripMenuItem menuItemReadReloadAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditFindBoard;
		private System.Windows.Forms.ContextMenuStrip contextMenuRes;
		private System.Windows.Forms.ToolStripSeparator menuItem6;
		private System.Windows.Forms.ToolStripSeparator menuItem16;
		private System.Windows.Forms.ToolStripSeparator menuItem21;
		private System.Windows.Forms.ToolStripMenuItem menuItemResWrite;
		private System.Windows.Forms.ToolStripMenuItem menuItemResCopy;
		private System.Windows.Forms.ToolStripMenuItem menuItemResRefCopy;
		private System.Windows.Forms.ToolStripMenuItem menuItemResIDPopup;
		private System.Windows.Forms.ToolStripMenuItem menuItemResAddNG;
		private System.Windows.Forms.ToolStripMenuItem menuItemResABone;
		private System.Windows.Forms.ToolStripMenuItem menuItemResHideABone;
		private System.Windows.Forms.ContextMenuStrip contextMenuListView;
		private System.Windows.Forms.ToolStripMenuItem menuItemListOpenNewTab;
		private System.Windows.Forms.ToolStripSeparator menuItem15;
		private System.Windows.Forms.ToolStripMenuItem menuItemListCopyURL;
		private System.Windows.Forms.ToolStripMenuItem menuItemListCopyURLName;
		private System.Windows.Forms.ToolStripSeparator menuItem22;
		private System.Windows.Forms.ToolStripMenuItem menuItemListDeleteLog;
		private System.Windows.Forms.ToolStripMenuItem menuItemListNewResPopup;
		private System.Windows.Forms.ToolStripMenuItem menuItemListHeadPopup;
		private System.Windows.Forms.ToolStripSeparator menuItem29;
		private System.Windows.Forms.ToolStripMenuItem menuItemListABone;
		private System.Windows.Forms.ToolStripMenuItem menuItemListOpenWebBrowser;
		private System.Windows.Forms.ToolStripSeparator menuItem33;
		private System.Windows.Forms.ImageList imageListLv;
		private System.Windows.Forms.ToolStripSeparator menuItem3;
		private System.Windows.Forms.ImageList imageListTable;
		private System.Windows.Forms.ContextMenuStrip contextMenuTable;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableNewOpen;
		private System.Windows.Forms.ToolStripSeparator menuItem18;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableCopyURL;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableCopyURLName;
		private System.Windows.Forms.ToolStripSeparator menuItem25;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableOpenWebBrowser;
		private System.Windows.Forms.ToolStripSeparator menuItem30;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableShowLocalRule;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableShowPicture;
		private System.Windows.Forms.ToolStripSeparator menuItem35;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableDeleteLog;
		private System.Windows.Forms.ToolStripMenuItem menuItemListCache;
		private System.Windows.Forms.ToolStripMenuItem menuItemListCacheOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemListCacheClear;
		private System.Windows.Forms.ToolStripMenuItem menuItemListShowPicture;
		private System.Windows.Forms.ToolStripMenuItem menuItemListShowLocalRule;
		private System.Windows.Forms.ToolBarButton toolBarButtonResExtract;
		private System.Windows.Forms.ToolStripMenuItem menuItemHelpExit;
		private System.Windows.Forms.ToolStripMenuItem menuItemHelpOpenLoadFactor;
		private System.Windows.Forms.ToolStripMenuItem menuItemHelpOpenWeb;
		private RebarDotNet.BandWrapper bandWrapperList;
		private System.Windows.Forms.ToolBar toolBarList;
		private System.Windows.Forms.ToolBarButton toolBarButtonListSearch;
		private System.Windows.Forms.ToolBarButton toolBarButtonListNewThread;
		private System.Windows.Forms.ToolBarButton toolBarButtonListReload;
		private System.Windows.Forms.ToolBarButton toolBarButtonListStop;
		private System.Windows.Forms.ToolBarButton toolBarButtonListClose;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditSearchList;
		private System.Windows.Forms.ToolStripMenuItem menuItemHelpTest;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButtonSearchCache;
		private System.Windows.Forms.ToolBarButton toolBarButtonSearchBoard;
		private System.Windows.Forms.ToolStripMenuItem menuItemResBookmark;
		private System.Windows.Forms.ToolStripSeparator menuItem28;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadBookmark;
		private System.Windows.Forms.ToolStripMenuItem menuItemScrollToBottom;
		private System.Windows.Forms.ToolBarButton toolBarButtonBookmark;
		private System.Windows.Forms.ContextMenuStrip contextMenuBookmarkFolder;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkPatrol;
		private System.Windows.Forms.ContextMenuStrip contextMenuBookmarkItem;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkNewOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkRemove;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkUpdateCheck;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewListBar;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemListHistoryOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemListDraftOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkNewFolder;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkDel;
		private System.Windows.Forms.ToolStripSeparator menuItem36;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkSort;
		private System.Windows.Forms.ToolStripSeparator menuItem32;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileLogManager;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileRemoveLogs;
		private System.Windows.Forms.TabPage tabPageWareHouse;
		private System.Windows.Forms.ContextMenuStrip contextMenuWareHouseFolder;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseOpen;
		private System.Windows.Forms.ToolStripSeparator menuItem34;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseNewFolder;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseSort;
		private System.Windows.Forms.ToolStripSeparator menuItem44;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseRemove;
		private System.Windows.Forms.ContextMenuStrip contextMenuWareHouseItem;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseItem_NewOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseItem_Remove;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkToWareHouse;
		private System.Windows.Forms.ToolStripSeparator menuItem39;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkItemToWareHouse;
		private System.Windows.Forms.ToolStripSeparator menuItem37;
		private CSharpSamples.CSharpToolBar cSharpToolBar;
		private System.Windows.Forms.ToolStripMenuItem menuItemTableSetItaBotan;
		private System.Windows.Forms.ContextMenuStrip contextMenuItaBotan;
		private System.Windows.Forms.ToolStripMenuItem menuItemItaBotanRemove;
		private System.Windows.Forms.StatusBarPanel statusResCount;
		private System.Windows.Forms.StatusBarPanel statusForce;
		private System.Windows.Forms.ToolStripMenuItem menuItemListPastlog;
		private System.Windows.Forms.ToolStripSeparator menuItem40;
		private System.Windows.Forms.ContextMenuStrip contextMenuThreadTab;
		private System.Windows.Forms.ToolStripSeparator menuItem43;
		private System.Windows.Forms.ToolStripSeparator menuItem47;
		private System.Windows.Forms.ContextMenuStrip contextMenuListTab;
		private System.Windows.Forms.ToolStripSeparator menuItem53;
		private System.Windows.Forms.ToolStripMenuItem menuItemListUpdateCheck;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabClose2;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabCloseAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabOpenUpThreads;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabOpenWebBrowsre;
		private System.Windows.Forms.ToolStripSeparator menuItem50;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabCacheOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabCacheClear;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabClose2;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabCloseAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabCopyURL;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabCopyURLAndName;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabNewThread;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabOpenWebBrowser;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkRename;
		private System.Windows.Forms.ToolStripSeparator menuItem41;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseItem_Rename;
		private System.Windows.Forms.ToolStripSeparator menuItem42;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseFolderRename;
		private System.Windows.Forms.ToolStripSeparator menuItem8;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkItemRename;
		private System.Windows.Forms.ToolStripSeparator menuItem46;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewTableItaBotan;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditItaBtan;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ToolBarButton toolBarButtonPatrol;
		private System.Windows.Forms.ToolStripMenuItem menuItemListAllThreads;
		private System.Windows.Forms.ToolStripMenuItem menuItemListWrittenThreads;
		private System.Windows.Forms.ToolStripMenuItem menuItemLayout;
		private System.Windows.Forms.ToolStripMenuItem menuItemLayoutTate3;
		private System.Windows.Forms.ToolStripMenuItem menuItemLayoutStd;
		private System.Windows.Forms.ToolStripMenuItem menuItemLayoutYoko3;
		private System.Windows.Forms.ToolStripMenuItem menuItemLayoutTateYoko2;
		private System.Windows.Forms.ToolBarButton toolBarButtonCaching;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.ToolBarButton toolBarButtonViewList;
		private System.Windows.Forms.ToolBarButton toolBarButtonViewThread;
		private System.Windows.Forms.ToolBarButton toolBarButtonViewTable;
		private System.Windows.Forms.ToolBarButton toolBarButtonListOpenUp;
		private System.Windows.Forms.ToolBarButton toolBarButton5;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewTableDockRight;
		private System.Windows.Forms.ToolStripMenuItem menuItemSaveScrap;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsScrapEditor;
		private System.Windows.Forms.ToolStripMenuItem menuItemResBackReference;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokokara;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokomade;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeWrite;
		private System.Windows.Forms.ToolStripSeparator menuItem57;
		private System.Windows.Forms.ToolStripMenuItem menuItemKokoMadeCopy;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeRefCopy;
		private System.Windows.Forms.ToolStripSeparator menuItem61;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeAddNGID;
		private System.Windows.Forms.ToolStripSeparator menuItem63;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeABone;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeHideABone;
		private System.Windows.Forms.ToolStripSeparator menuItem66;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeCancel;
		private System.Windows.Forms.ToolStripSeparator menuItem54;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolBar toolBarTools;
		private System.Windows.Forms.ToolBarButton toolBarButtonRunTool;
		private System.Windows.Forms.ComboBox comboBoxTools;
		private RebarDotNet.BandWrapper bandWrapperTools;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewTools;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsSub;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsRegist;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileSave;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileSaveDat;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileSaveHtml;
		private System.Windows.Forms.ToolStripMenuItem menuItemOpenDat;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewLiveMode;
		private System.Windows.Forms.ToolStripMenuItem menuItemSearch;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadLinkExtract;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditShortcut;
		private System.Windows.Forms.ToolStripMenuItem menuItemFocus;
		private System.Windows.Forms.ToolStripMenuItem menuItemFocusTable;
		private System.Windows.Forms.ToolStripMenuItem menuItemFocusList;
		private System.Windows.Forms.ToolStripMenuItem menuItemFocusThread;
		private System.Windows.Forms.ToolStripMenuItem menuItemFocusBookmark;
		private System.Windows.Forms.ToolStripMenuItem menuItemFocusWare;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewDockWriteBar;
		private System.Windows.Forms.Panel threadInnerPanel;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowListNext;
		private System.Windows.Forms.ToolStripMenuItem menuItemWindowListPrev;
		private System.Windows.Forms.ToolStripSeparator menuItem1;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadSetUpChecker;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewUpChecker;
		private System.Windows.Forms.ToolStripMenuItem menuItemListIndexing;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkSetItabotan;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseSetItabotan;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileSaveMonalog;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileOpenMonalog;
		private System.Windows.Forms.ToolStripMenuItem menuItemRefResWrite;
		private System.Windows.Forms.ToolBarButton toolBarButton8;
		private System.Windows.Forms.ToolBarButton toolBarButton9;
		private System.Windows.Forms.ToolBarButton toolBarButtonNGWords;
		private System.Windows.Forms.ToolBarButton toolBarButtonLive;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewSwitch;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewHideTable;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewFillList;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewFillThread;
		private System.Windows.Forms.ToolStripMenuItem menuItemLayoutExtend01;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabOpenDraft;
		private System.Windows.Forms.ToolStripSeparator menuItem65;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ToolStripMenuItem menuItemListCopyName;
		private System.Windows.Forms.ToolStripMenuItem menuItemListSetBookmark;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabCopyName;
		private System.Windows.Forms.ToolStripMenuItem menuItemScrollBack;
		private System.Windows.Forms.ToolStripSeparator menuItem67;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadTabDelClose;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewThreadToolBar;
		private System.Windows.Forms.ToolStripMenuItem menuItemItemViewFirst;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitFirst50;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitFirst100;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitFirst250;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitFirst500;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitFirst1000;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewNewResOnly;
		private System.Windows.Forms.ToolStripMenuItem menuItemItemViewLast;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitLast50;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitLast100;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitLast250;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitLast500;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitLast1000;
		private System.Windows.Forms.ToolStripSeparator menuItem80;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewShiori;
		private System.Windows.Forms.ToolStripMenuItem menuItemScrollToTop;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsImageViewer;
		private System.Windows.Forms.ToolStripMenuItem menuItemResCopyUrl;
		private System.Windows.Forms.ToolStripMenuItem menuItemResCopyNameUrl;
		private System.Windows.Forms.ToolStripSeparator menuItem69;
		private System.Windows.Forms.ToolStripSeparator menuItem64;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeCopyUrl;
		private System.Windows.Forms.ToolStripMenuItem menuItemResKokoMadeCopyNameUrl;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitFirstXXX;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetLimitLastXXX;
		private System.Windows.Forms.ToolStripMenuItem menuItemFontSize;
		private System.Windows.Forms.ToolStripMenuItem menuItemFontSizeLarge;
		private System.Windows.Forms.ToolStripMenuItem menuItemFontSizeMedium;
		private System.Windows.Forms.ToolStripMenuItem menuItemFontSizeSmall;
		private System.Windows.Forms.ToolStripMenuItem menuItemFontSizeXSmall;
		private System.Windows.Forms.ToolStripMenuItem menuItemFontSizeXLarge;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsSaveWindowUrls;
		private System.Windows.Forms.ToolStripMenuItem menuItemToolsOpenStartupUrls;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarks;
		private System.Windows.Forms.ToolStripMenuItem menuItemSaveAa;
		private System.Windows.Forms.ToolStripMenuItem menuItemBookmarkOpenIncludeSubChildren;
		private System.Windows.Forms.ToolStripMenuItem menuItemWareHouseOpenIncludeSubChildren;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadNextThreadCheck;
		private System.Windows.Forms.ToolStripSeparator menuItem71;
		private System.Windows.Forms.ToolStripMenuItem menuItemListTabUpdateCheck;
		private System.Windows.Forms.ToolStripMenuItem menuItemFileHistoryUpdateCheck;
		private System.Windows.Forms.ToolStripMenuItem menuItemResOpenLinks;
		private System.Windows.Forms.ToolStripSeparator menuItem73;
		private System.Windows.Forms.ToolStripMenuItem menuItemListSetWarehouse;
		private System.Windows.Forms.ToolStripSeparator menuItem74;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewSirusi;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadSirusiManager;
		private System.Windows.Forms.ToolStripMenuItem menuItemResSirusi;
		private System.Windows.Forms.ToolStripSeparator menuItem72;
		private System.Windows.Forms.ToolStripMenuItem menuItemViewAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemThreadReloadAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemScrollSetNewScroll;
		private System.Windows.Forms.ToolStripMenuItem menuItemView_FixedRebarBands;
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Twin2IeBrowser));
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileBoardUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileLogManager = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileRemoveLogs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFileLogIndexing = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileLogDeleteImageCache = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem59 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemOpenDat = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileOpenMonalog = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFileSave = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileSaveDat = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileSaveHtml = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileSaveMonalog = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemSaveSelectedAllToDat = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSaveSelectedAllToHtml = new System.Windows.Forms.ToolStripMenuItem();
			this.miSaveSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFileCloseHistory = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFileHistoryClear = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileHistoryUpdateCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.履歴を削除HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileClearNameHistory = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileClearSearchHistory = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemClearAllHistory = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFilePrint = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFileOnline = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditNGWords = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditColoringWord = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditItaBtan = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditShortcut = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemEditRegistBoard = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem56 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemSaveScrap = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSaveAa = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewToolBars = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewToolBar = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewThreadToolBar = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewListBar = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewIButton = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewTools = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewAddressBar = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem17 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemView_FixedRebarBands = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewStatusBar = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem48 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLayoutStd = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem58 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemLayoutTate3 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLayoutYoko3 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLayoutTateYoko2 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem45 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemLayoutExtend01 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewSwitch = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewHideTable = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewFillList = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewFillThread = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFocus = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFocusTable = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFocusBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFocusWare = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFocusList = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFocusThread = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFontSizeXLarge = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFontSizeLarge = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFontSizeMedium = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFontSizeSmall = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFontSizeXSmall = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem51 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemViewTableItaBotan = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewTableDockRight = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem55 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemViewDockWriteBar = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewUpChecker = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemViewPatrol = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemPatrolHiddenPastlog = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemViewLiveMode = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSearch = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditSearchCache = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditSearchList = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemEditFindBoard = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemEditSearchSubjectBotanAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBookmarks = new System.Windows.Forms.ToolStripMenuItem();
			this.miGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.miGroupAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.miGroupEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemList = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListReload = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListStop = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListShowLocalRule = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListShowPicture = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemShowSettingTxt = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem27 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListAllThreads = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListWrittenThreads = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListCache = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListCacheOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListCacheClear = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListIndexing = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListHistoryOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListDraftOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem49 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThread = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadReload = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadReloadAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadStop = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemNewThread = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadPostRes = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem77 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadSetUpChecker = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem38 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadFind = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadLinkExtract = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadResExtract = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadNextThreadCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem26 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadResetPastlogFlags = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadSirusiManager = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem20 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemScrollToTop = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemScrollToBottom = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadAutoReload = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadAutoFocus = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem75 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadDeleteClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadReget = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolsSub = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolsRegist = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem60 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItem68 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemToolsScrapEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolsImageViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.miMouseGestureSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem70 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemToolsSaveWindowUrls = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolsOpenStartupUrls = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem52 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemToolOyster = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolOysterEnable = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolOysterDisable = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolBe = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolBeLogin = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolBeLogout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemToolsInetOption = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemToolsOption = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWindowThreadCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWindowThreadCloseNotActive = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem31 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWindowSelectPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWindowSelectNext = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem24 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWindowListCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWindowListCloseNotActive = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem62 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWindowListPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWindowListNext = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHelpOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHelpOpenWeb = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHelpOpenErrorLog = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem19 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemHelpOpenLoadFactor = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHelpOpenServerWatch2 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHelpExit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHelpTest = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolBarMain = new System.Windows.Forms.ToolBar();
			this.toolBarButtonOnline = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCaching = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSearchCache = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSearchBoard = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonViewTable = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonViewList = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonViewThread = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonNGWords = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonLive = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPatrol = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSettings = new System.Windows.Forms.ToolBarButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.addressPanel = new System.Windows.Forms.Panel();
			this.toolBarGo = new System.Windows.Forms.ToolBar();
			this.toolBarButtonGo = new System.Windows.Forms.ToolBarButton();
			this.textBoxAddress = new System.Windows.Forms.TextBox();
			this.rebarWrapper = new RebarDotNet.RebarWrapper();
			this.bandWrapperMain = new RebarDotNet.BandWrapper();
			this.bandWrapperList = new RebarDotNet.BandWrapper();
			this.toolBarList = new System.Windows.Forms.ToolBar();
			this.toolBarButtonListReload = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonListStop = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonListNewThread = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonListSearch = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonListOpenUp = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonListClose = new System.Windows.Forms.ToolBarButton();
			this.bandWrapperTools = new RebarDotNet.BandWrapper();
			this.panel1 = new System.Windows.Forms.Panel();
			this.comboBoxTools = new System.Windows.Forms.ComboBox();
			this.toolBarTools = new System.Windows.Forms.ToolBar();
			this.toolBarButtonRunTool = new System.Windows.Forms.ToolBarButton();
			this.bandWrapperIButton = new RebarDotNet.BandWrapper();
			this.cSharpToolBar = new CSharpSamples.CSharpToolBar();
			this.contextMenuItaBotan = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemItaBotanRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.bandWrapperAddress = new RebarDotNet.BandWrapper();
			this.treePanel = new System.Windows.Forms.Panel();
			this.tabControlTable = new System.Windows.Forms.TabControl();
			this.tabPageBoards = new System.Windows.Forms.TabPage();
			this.tabPageBookmarks = new System.Windows.Forms.TabPage();
			this.tabPageWareHouse = new System.Windows.Forms.TabPage();
			this.listPanel = new System.Windows.Forms.Panel();
			this.listTabCtrl = new System.Windows.Forms.TabControl();
			this.contextMenuListTab = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemListTabClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListTabClose2 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListTabCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem53 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListTabOpenUpThreads = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListTabOpenWebBrowsre = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem50 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListTabCopyURL = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListTabCopyNameURL = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListTabCacheOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListTabCacheClear = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListTabRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem71 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListTabUpdateCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListTabWithout1000Res = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListTabWithoutPastlog = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListTabWithoutKakolog = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListTabColoring = new System.Windows.Forms.ToolStripMenuItem();
			this.imageListSmallIcons = new System.Windows.Forms.ImageList(this.components);
			this.threadPanel = new System.Windows.Forms.Panel();
			this.threadInnerPanel = new System.Windows.Forms.Panel();
			this.threadTabCtrl = new System.Windows.Forms.TabControl();
			this.contextMenuThreadTab = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemThreadTabClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabClose2 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabCloseLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabCloseRight = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabCloseWithoutThis = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem67 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabDelClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem43 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabCopyURL = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabCopyURLAndName = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabCopyName = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem47 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabOpenWebBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabOpenDraft = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem65 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabNewThread = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabNextThreadCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabAllOpenImageViewer = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSaveImages = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThreadTabReget = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThreadTabColoring = new System.Windows.Forms.ToolStripMenuItem();
			this.threadToolPanel = new System.Windows.Forms.Panel();
			this.labelBoardName = new System.Windows.Forms.Label();
			this.labelThreadSubject = new System.Windows.Forms.Label();
			this.toolBarThread = new System.Windows.Forms.ToolBar();
			this.toolBarButtonViewChange = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonScrollTop = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonScroll = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonReload = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonWriteRes = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAutoReload = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonFind = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonResExtract = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonBookmark = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton12 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonDelete = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonClose = new System.Windows.Forms.ToolBarButton();
			this.splitterLeft = new System.Windows.Forms.Splitter();
			this.splitterTop = new System.Windows.Forms.Splitter();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusText = new System.Windows.Forms.StatusBarPanel();
			this.statusProgress = new System.Windows.Forms.StatusBarPanel();
			this.statusSize = new System.Windows.Forms.StatusBarPanel();
			this.statusResCount = new System.Windows.Forms.StatusBarPanel();
			this.statusForce = new System.Windows.Forms.StatusBarPanel();
			this.statusSamba24 = new System.Windows.Forms.StatusBarPanel();
			this.statusTimerCount = new System.Windows.Forms.StatusBarPanel();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuViewChange = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemItemViewFirst = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitFirst50 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitFirst100 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitFirst250 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitFirst500 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitFirst1000 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitFirstXXX = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemItemViewLast = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitLast50 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitLast100 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitLast250 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitLast500 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitLast1000 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetLimitLastXXX = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemViewChangePrev = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewChangeNext = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem80 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemViewNewResOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemViewSirusi = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemViewShiori = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemRemoveShiori = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuScroll = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemScrollSetAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemScrollSetNewScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemScrollBack = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuRead = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemReadReget = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemReadReloadAll = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuRes = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemResWrite = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemRefResWrite = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResRefCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResCopyID = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem69 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResCopyUrl = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResCopyNameUrl = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResIDPopup = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResBackReference = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem54 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResAddNG = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem28 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResSirusi = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem72 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResKokokara = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResKokomade = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResKokoMadeWrite = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem57 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemKokoMadeCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResKokoMadeRefCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem64 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResKokoMadeCopyUrl = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResKokoMadeCopyNameUrl = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem61 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResKokoMadeAddNGID = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResKokoMadeLink = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem63 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResKokoMadeABone = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResKokoMadeHideABone = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem66 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResKokoMadeCancel = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResOpenLinks = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem73 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemResABone = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemResHideABone = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuListView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemListOpenNewTab = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListHeadPopup = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListNewResPopup = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem29 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListCopyURL = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListCopyURLName = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListCopyName = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem33 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListOpenWebBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem22 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListSetWarehouse = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem74 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListSearchNext = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListUpdateCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListPastlog = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem40 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemListABone = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemListDeleteLog = new System.Windows.Forms.ToolStripMenuItem();
			this.imageListLv = new System.Windows.Forms.ImageList(this.components);
			this.imageListTable = new System.Windows.Forms.ImageList(this.components);
			this.contextMenuTable = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemTableNewOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem18 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemTableCopyURL = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTableCopyURLName = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem25 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemTableOpenWebBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem30 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemTableSetItaBotan = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTableShowLocalRule = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTableShowPicture = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTableShowSettingTxt = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem35 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemTableDeleteLog = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuBookmarkFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemBookmarkOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBookmarkOpenIncludeSubChildren = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem32 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemBookmarkUpdateCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBookmarkPatrol = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem36 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemBookmarkNewFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBookmarkToWareHouse = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBookmarkSetItabotan = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem41 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemBookmarkSort = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBookmarkRename = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem39 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemBookmarkDel = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuBookmarkItem = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemBookmarkNewOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem46 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemBookmarkItemToWareHouse = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBookmarkItemRename = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem37 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemBookmarkRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuWareHouseFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemWareHouseOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWareHouseOpenIncludeSubChildren = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem34 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWareHouseNewFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWareHouseSetItabotan = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWareHouseSort = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWareHouseFolderRename = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem44 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWareHouseRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuWareHouseItem = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemWareHouseItem_NewOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWareHouseItem_Rename = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem42 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWareHouseItem_Remove = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStripNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemNotifyIconExit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.contextMenuStripAutoReload = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemAutoImageOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu.SuspendLayout();
			this.addressPanel.SuspendLayout();
			this.rebarWrapper.SuspendLayout();
			this.panel1.SuspendLayout();
			this.contextMenuItaBotan.SuspendLayout();
			this.treePanel.SuspendLayout();
			this.tabControlTable.SuspendLayout();
			this.listPanel.SuspendLayout();
			this.contextMenuListTab.SuspendLayout();
			this.threadPanel.SuspendLayout();
			this.threadInnerPanel.SuspendLayout();
			this.contextMenuThreadTab.SuspendLayout();
			this.threadToolPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusProgress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusResCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusForce)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusSamba24)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusTimerCount)).BeginInit();
			this.contextMenuViewChange.SuspendLayout();
			this.contextMenuScroll.SuspendLayout();
			this.contextMenuRead.SuspendLayout();
			this.contextMenuRes.SuspendLayout();
			this.contextMenuListView.SuspendLayout();
			this.contextMenuTable.SuspendLayout();
			this.contextMenuBookmarkFolder.SuspendLayout();
			this.contextMenuBookmarkItem.SuspendLayout();
			this.contextMenuWareHouseFolder.SuspendLayout();
			this.contextMenuWareHouseItem.SuspendLayout();
			this.contextMenuStripNotifyIcon.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.contextMenuStripAutoReload.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Dock = System.Windows.Forms.DockStyle.None;
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemEdit,
            this.menuItemView,
            this.menuItemSearch,
            this.menuItemBookmarks,
            this.miGroup,
            this.menuItemList,
            this.menuItemThread,
            this.menuItemTools,
            this.menuItemWindow,
            this.menuItemHelp});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
			this.mainMenu.Size = new System.Drawing.Size(896, 26);
			this.mainMenu.TabIndex = 13;
			// 
			// menuItemFile
			// 
			this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileBoardUpdate,
            this.menuItemFileLogManager,
            this.menuItem59,
            this.menuItemFileOpen,
            this.menuItemFileSave,
            this.miSaveSettings,
            this.menuItem9,
            this.menuItemFileCloseHistory,
            this.menuItem13,
            this.履歴を削除HToolStripMenuItem,
            this.toolStripMenuItem4,
            this.menuItemFilePrint,
            this.menuItemFileOnline,
            this.menuItem12,
            this.menuItemFileExit});
			this.menuItemFile.Name = "menuItemFile";
			this.menuItemFile.Size = new System.Drawing.Size(85, 22);
			this.menuItemFile.Text = "ファイル(&F)";
			this.menuItemFile.DropDownOpening += new System.EventHandler(this.menuItemFile_Popup);
			// 
			// menuItemFileBoardUpdate
			// 
			this.menuItemFileBoardUpdate.Name = "menuItemFileBoardUpdate";
			this.menuItemFileBoardUpdate.Size = new System.Drawing.Size(274, 22);
			this.menuItemFileBoardUpdate.Text = "板一覧を更新(&U)";
			this.menuItemFileBoardUpdate.Click += new System.EventHandler(this.menuItemFileBoardUpdate_Click);
			// 
			// menuItemFileLogManager
			// 
			this.menuItemFileLogManager.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileRemoveLogs,
            this.toolStripMenuItem9,
            this.menuItemFileLogIndexing,
            this.menuItemFileLogDeleteImageCache});
			this.menuItemFileLogManager.Name = "menuItemFileLogManager";
			this.menuItemFileLogManager.Size = new System.Drawing.Size(274, 22);
			this.menuItemFileLogManager.Text = "ログ管理(&L)";
			// 
			// menuItemFileRemoveLogs
			// 
			this.menuItemFileRemoveLogs.Name = "menuItemFileRemoveLogs";
			this.menuItemFileRemoveLogs.Size = new System.Drawing.Size(288, 22);
			this.menuItemFileRemoveLogs.Text = "お気に入り以外のログを削除(&D)";
			this.menuItemFileRemoveLogs.Click += new System.EventHandler(this.menuItemFileRemoveLogs_Click);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(285, 6);
			// 
			// menuItemFileLogIndexing
			// 
			this.menuItemFileLogIndexing.Name = "menuItemFileLogIndexing";
			this.menuItemFileLogIndexing.Size = new System.Drawing.Size(288, 22);
			this.menuItemFileLogIndexing.Text = "ログインデックス生成(&R)";
			this.menuItemFileLogIndexing.Click += new System.EventHandler(this.menuItemFileLogIndexing_Click);
			// 
			// menuItemFileLogDeleteImageCache
			// 
			this.menuItemFileLogDeleteImageCache.Name = "menuItemFileLogDeleteImageCache";
			this.menuItemFileLogDeleteImageCache.Size = new System.Drawing.Size(288, 22);
			this.menuItemFileLogDeleteImageCache.Text = "画像サムネイルのキャッシュを削除(&M)";
			this.menuItemFileLogDeleteImageCache.Click += new System.EventHandler(this.menuItemFileLogDeleteImageCache_Click);
			// 
			// menuItem59
			// 
			this.menuItem59.Name = "menuItem59";
			this.menuItem59.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemFileOpen
			// 
			this.menuItemFileOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpenDat,
            this.menuItemFileOpenMonalog,
            this.toolStripMenuItem14});
			this.menuItemFileOpen.Name = "menuItemFileOpen";
			this.menuItemFileOpen.Size = new System.Drawing.Size(274, 22);
			this.menuItemFileOpen.Text = "ファイルを開く(&O)";
			// 
			// menuItemOpenDat
			// 
			this.menuItemOpenDat.Name = "menuItemOpenDat";
			this.menuItemOpenDat.Size = new System.Drawing.Size(216, 22);
			this.menuItemOpenDat.Text = "DAT形式を開く(&D)...";
			this.menuItemOpenDat.Click += new System.EventHandler(this.menuItemOpenDat_Click);
			// 
			// menuItemFileOpenMonalog
			// 
			this.menuItemFileOpenMonalog.Name = "menuItemFileOpenMonalog";
			this.menuItemFileOpenMonalog.Size = new System.Drawing.Size(216, 22);
			this.menuItemFileOpenMonalog.Text = "Monalog形式を開く(&M)...";
			this.menuItemFileOpenMonalog.Click += new System.EventHandler(this.menuItemFileOpenMonalog_Click);
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(213, 6);
			// 
			// menuItemFileSave
			// 
			this.menuItemFileSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileSaveDat,
            this.menuItemFileSaveHtml,
            this.menuItemFileSaveMonalog,
            this.toolStripMenuItem13,
            this.menuItemSaveSelectedAllToDat,
            this.menuItemSaveSelectedAllToHtml});
			this.menuItemFileSave.Name = "menuItemFileSave";
			this.menuItemFileSave.Size = new System.Drawing.Size(274, 22);
			this.menuItemFileSave.Text = "ファイルに保存(&S)";
			// 
			// menuItemFileSaveDat
			// 
			this.menuItemFileSaveDat.Name = "menuItemFileSaveDat";
			this.menuItemFileSaveDat.Size = new System.Drawing.Size(392, 22);
			this.menuItemFileSaveDat.Text = "DAT形式で保存(&D)...";
			this.menuItemFileSaveDat.Click += new System.EventHandler(this.menuItemFileSaveDat_Click);
			// 
			// menuItemFileSaveHtml
			// 
			this.menuItemFileSaveHtml.Name = "menuItemFileSaveHtml";
			this.menuItemFileSaveHtml.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
			this.menuItemFileSaveHtml.Size = new System.Drawing.Size(392, 22);
			this.menuItemFileSaveHtml.Text = "HTML形式で保存(&H)...";
			this.menuItemFileSaveHtml.Click += new System.EventHandler(this.menuItemFileSaveHtml_Click);
			// 
			// menuItemFileSaveMonalog
			// 
			this.menuItemFileSaveMonalog.Name = "menuItemFileSaveMonalog";
			this.menuItemFileSaveMonalog.Size = new System.Drawing.Size(392, 22);
			this.menuItemFileSaveMonalog.Text = "Monalog形式で保存(&M)...";
			this.menuItemFileSaveMonalog.Click += new System.EventHandler(this.menuItemFileSaveMonalog_Click);
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(389, 6);
			// 
			// menuItemSaveSelectedAllToDat
			// 
			this.menuItemSaveSelectedAllToDat.Name = "menuItemSaveSelectedAllToDat";
			this.menuItemSaveSelectedAllToDat.Size = new System.Drawing.Size(392, 22);
			this.menuItemSaveSelectedAllToDat.Text = "選択されているスレッドをまとめてDAT形式で保存(&A)...";
			this.menuItemSaveSelectedAllToDat.Click += new System.EventHandler(this.menuItemSaveSelectedAllToDat_Click);
			// 
			// menuItemSaveSelectedAllToHtml
			// 
			this.menuItemSaveSelectedAllToHtml.Name = "menuItemSaveSelectedAllToHtml";
			this.menuItemSaveSelectedAllToHtml.Size = new System.Drawing.Size(392, 22);
			this.menuItemSaveSelectedAllToHtml.Text = "選択されているスレッドをまとめてHTML形式で保存(&T)...";
			this.menuItemSaveSelectedAllToHtml.Click += new System.EventHandler(this.menuItemSaveSelectedAllToHtml_Click);
			// 
			// miSaveSettings
			// 
			this.miSaveSettings.Name = "miSaveSettings";
			this.miSaveSettings.Size = new System.Drawing.Size(274, 22);
			this.miSaveSettings.Text = "すべての設定情報を保存(&W)";
			this.miSaveSettings.Click += new System.EventHandler(this.miSaveSettings_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Name = "menuItem9";
			this.menuItem9.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemFileCloseHistory
			// 
			this.menuItemFileCloseHistory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem5,
            this.menuItemFileHistoryClear,
            this.menuItemFileHistoryUpdateCheck});
			this.menuItemFileCloseHistory.Name = "menuItemFileCloseHistory";
			this.menuItemFileCloseHistory.Size = new System.Drawing.Size(274, 22);
			this.menuItemFileCloseHistory.Text = "最近閉じたスレッド(&R)";
			this.menuItemFileCloseHistory.DropDownOpening += new System.EventHandler(this.menuItemFileCloseHistory_Popup);
			// 
			// menuItem5
			// 
			this.menuItem5.Name = "menuItem5";
			this.menuItem5.Size = new System.Drawing.Size(163, 6);
			// 
			// menuItemFileHistoryClear
			// 
			this.menuItemFileHistoryClear.Name = "menuItemFileHistoryClear";
			this.menuItemFileHistoryClear.Size = new System.Drawing.Size(166, 22);
			this.menuItemFileHistoryClear.Text = "履歴をクリア(&Z)";
			this.menuItemFileHistoryClear.Click += new System.EventHandler(this.menuItemFileHistoryClear_Click);
			// 
			// menuItemFileHistoryUpdateCheck
			// 
			this.menuItemFileHistoryUpdateCheck.Name = "menuItemFileHistoryUpdateCheck";
			this.menuItemFileHistoryUpdateCheck.Size = new System.Drawing.Size(166, 22);
			this.menuItemFileHistoryUpdateCheck.Text = "更新チェック(&X)";
			this.menuItemFileHistoryUpdateCheck.Click += new System.EventHandler(this.menuItemFileHistoryUpdateCheck_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Name = "menuItem13";
			this.menuItem13.Size = new System.Drawing.Size(271, 6);
			// 
			// 履歴を削除HToolStripMenuItem
			// 
			this.履歴を削除HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileClearNameHistory,
            this.menuItemFileClearSearchHistory,
            this.toolStripMenuItem5,
            this.menuItemClearAllHistory});
			this.履歴を削除HToolStripMenuItem.Name = "履歴を削除HToolStripMenuItem";
			this.履歴を削除HToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
			this.履歴を削除HToolStripMenuItem.Text = "履歴を削除(&H)";
			// 
			// menuItemFileClearNameHistory
			// 
			this.menuItemFileClearNameHistory.Name = "menuItemFileClearNameHistory";
			this.menuItemFileClearNameHistory.Size = new System.Drawing.Size(287, 22);
			this.menuItemFileClearNameHistory.Text = "名前欄およびメール欄の履歴を削除(&N)";
			this.menuItemFileClearNameHistory.Click += new System.EventHandler(this.menuItemFileClearNameHistory_Click);
			// 
			// menuItemFileClearSearchHistory
			// 
			this.menuItemFileClearSearchHistory.Name = "menuItemFileClearSearchHistory";
			this.menuItemFileClearSearchHistory.Size = new System.Drawing.Size(287, 22);
			this.menuItemFileClearSearchHistory.Text = "検索履歴を削除(&S)";
			this.menuItemFileClearSearchHistory.Click += new System.EventHandler(this.menuItemFileClearSearchHistory_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(284, 6);
			// 
			// menuItemClearAllHistory
			// 
			this.menuItemClearAllHistory.Name = "menuItemClearAllHistory";
			this.menuItemClearAllHistory.Size = new System.Drawing.Size(287, 22);
			this.menuItemClearAllHistory.Text = "すべての履歴を削除(&A)";
			this.menuItemClearAllHistory.Click += new System.EventHandler(this.menuItemClearAllHistory_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemFilePrint
			// 
			this.menuItemFilePrint.Name = "menuItemFilePrint";
			this.menuItemFilePrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.menuItemFilePrint.Size = new System.Drawing.Size(274, 22);
			this.menuItemFilePrint.Text = "印刷(&P)...";
			this.menuItemFilePrint.Click += new System.EventHandler(this.menuItemFilePrint_Click);
			// 
			// menuItemFileOnline
			// 
			this.menuItemFileOnline.Name = "menuItemFileOnline";
			this.menuItemFileOnline.Size = new System.Drawing.Size(274, 22);
			this.menuItemFileOnline.Text = "オンライン(&C)";
			this.menuItemFileOnline.Click += new System.EventHandler(this.menuItemFileOnline_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Name = "menuItem12";
			this.menuItem12.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemFileExit
			// 
			this.menuItemFileExit.Name = "menuItemFileExit";
			this.menuItemFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.menuItemFileExit.Size = new System.Drawing.Size(274, 22);
			this.menuItemFileExit.Text = "アプリケーションを終了(&X)";
			this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEditNGWords,
            this.menuItemEditColoringWord,
            this.menuItemEditItaBtan,
            this.menuItemEditShortcut,
            this.menuItem4,
            this.menuItemEditRegistBoard,
            this.menuItem56,
            this.menuItemSaveScrap,
            this.menuItemSaveAa});
			this.menuItemEdit.Name = "menuItemEdit";
			this.menuItemEdit.Size = new System.Drawing.Size(61, 22);
			this.menuItemEdit.Text = "編集(&E)";
			this.menuItemEdit.DropDownOpening += new System.EventHandler(this.menuItemEdit_Popup);
			// 
			// menuItemEditNGWords
			// 
			this.menuItemEditNGWords.Name = "menuItemEditNGWords";
			this.menuItemEditNGWords.Size = new System.Drawing.Size(260, 22);
			this.menuItemEditNGWords.Text = "NGワードを編集(&G)...";
			this.menuItemEditNGWords.Click += new System.EventHandler(this.menuItemEditNGWords_Click);
			// 
			// menuItemEditColoringWord
			// 
			this.menuItemEditColoringWord.Name = "menuItemEditColoringWord";
			this.menuItemEditColoringWord.Size = new System.Drawing.Size(260, 22);
			this.menuItemEditColoringWord.Text = "単語ハイライトを編集(&H)...";
			this.menuItemEditColoringWord.Click += new System.EventHandler(this.menuItemEditColoringWord_Click);
			// 
			// menuItemEditItaBtan
			// 
			this.menuItemEditItaBtan.Name = "menuItemEditItaBtan";
			this.menuItemEditItaBtan.Size = new System.Drawing.Size(260, 22);
			this.menuItemEditItaBtan.Text = "板ボタンを編集(&O)...";
			this.menuItemEditItaBtan.Click += new System.EventHandler(this.menuItemEditItaBtan_Click);
			// 
			// menuItemEditShortcut
			// 
			this.menuItemEditShortcut.Name = "menuItemEditShortcut";
			this.menuItemEditShortcut.Size = new System.Drawing.Size(260, 22);
			this.menuItemEditShortcut.Text = "ショートカットキーを編集(&S)";
			this.menuItemEditShortcut.Click += new System.EventHandler(this.menuItemEditShortcut_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Name = "menuItem4";
			this.menuItem4.Size = new System.Drawing.Size(257, 6);
			// 
			// menuItemEditRegistBoard
			// 
			this.menuItemEditRegistBoard.Name = "menuItemEditRegistBoard";
			this.menuItemEditRegistBoard.Size = new System.Drawing.Size(260, 22);
			this.menuItemEditRegistBoard.Text = "外部板を登録(&R)...";
			this.menuItemEditRegistBoard.Click += new System.EventHandler(this.menuItemEditRegistBoard_Click);
			// 
			// menuItem56
			// 
			this.menuItem56.Name = "menuItem56";
			this.menuItem56.Size = new System.Drawing.Size(257, 6);
			// 
			// menuItemSaveScrap
			// 
			this.menuItemSaveScrap.Name = "menuItemSaveScrap";
			this.menuItemSaveScrap.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.menuItemSaveScrap.Size = new System.Drawing.Size(260, 22);
			this.menuItemSaveScrap.Text = "選択範囲をメモとして保存(&C)";
			this.menuItemSaveScrap.Click += new System.EventHandler(this.menuItemSaveScrap_Click);
			// 
			// menuItemSaveAa
			// 
			this.menuItemSaveAa.Name = "menuItemSaveAa";
			this.menuItemSaveAa.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.menuItemSaveAa.Size = new System.Drawing.Size(260, 22);
			this.menuItemSaveAa.Text = "選択範囲をAAとして保存(&A)";
			this.menuItemSaveAa.Click += new System.EventHandler(this.menuItemSaveAa_Click);
			// 
			// menuItemView
			// 
			this.menuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewToolBars,
            this.menuItemViewStatusBar,
            this.menuItem48,
            this.menuItemLayout,
            this.menuItemViewSwitch,
            this.menuItemFocus,
            this.menuItemFontSize,
            this.menuItem51,
            this.menuItemViewTableItaBotan,
            this.menuItemViewTableDockRight,
            this.menuItem55,
            this.menuItemViewDockWriteBar,
            this.menuItemViewUpChecker,
            this.toolStripMenuItem10,
            this.menuItemViewPatrol,
            this.menuItem23,
            this.menuItemViewLiveMode});
			this.menuItemView.Name = "menuItemView";
			this.menuItemView.Size = new System.Drawing.Size(62, 22);
			this.menuItemView.Text = "表示(&V)";
			this.menuItemView.DropDownOpening += new System.EventHandler(this.menuItemView_Popup);
			// 
			// menuItemViewToolBars
			// 
			this.menuItemViewToolBars.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewToolBar,
            this.menuItemViewThreadToolBar,
            this.menuItemViewListBar,
            this.menuItemViewIButton,
            this.menuItemViewTools,
            this.menuItemViewAddressBar,
            this.menuItem17,
            this.menuItemView_FixedRebarBands});
			this.menuItemViewToolBars.Name = "menuItemViewToolBars";
			this.menuItemViewToolBars.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewToolBars.Text = "ツールバー(&T)";
			// 
			// menuItemViewToolBar
			// 
			this.menuItemViewToolBar.Name = "menuItemViewToolBar";
			this.menuItemViewToolBar.Size = new System.Drawing.Size(203, 22);
			this.menuItemViewToolBar.Text = "メインツールバー(&T)";
			this.menuItemViewToolBar.Click += new System.EventHandler(this.menuItemViewToolBar_Click);
			// 
			// menuItemViewThreadToolBar
			// 
			this.menuItemViewThreadToolBar.Name = "menuItemViewThreadToolBar";
			this.menuItemViewThreadToolBar.Size = new System.Drawing.Size(203, 22);
			this.menuItemViewThreadToolBar.Text = "スレッドツールバー(&D)";
			this.menuItemViewThreadToolBar.Click += new System.EventHandler(this.menuItemViewThreadToolBar_Click);
			// 
			// menuItemViewListBar
			// 
			this.menuItemViewListBar.Name = "menuItemViewListBar";
			this.menuItemViewListBar.Size = new System.Drawing.Size(203, 22);
			this.menuItemViewListBar.Text = "板ツールバー(&R)";
			this.menuItemViewListBar.Click += new System.EventHandler(this.menuItemViewListBar_Click);
			// 
			// menuItemViewIButton
			// 
			this.menuItemViewIButton.Name = "menuItemViewIButton";
			this.menuItemViewIButton.Size = new System.Drawing.Size(203, 22);
			this.menuItemViewIButton.Text = "板ボタン(&B)";
			this.menuItemViewIButton.Click += new System.EventHandler(this.menuItemViewIButton_Click);
			// 
			// menuItemViewTools
			// 
			this.menuItemViewTools.Name = "menuItemViewTools";
			this.menuItemViewTools.Size = new System.Drawing.Size(203, 22);
			this.menuItemViewTools.Text = "外部ツールバー(&F)";
			this.menuItemViewTools.Click += new System.EventHandler(this.menuItemViewTools_Click);
			// 
			// menuItemViewAddressBar
			// 
			this.menuItemViewAddressBar.Name = "menuItemViewAddressBar";
			this.menuItemViewAddressBar.Size = new System.Drawing.Size(203, 22);
			this.menuItemViewAddressBar.Text = "アドレスバー(&A)";
			this.menuItemViewAddressBar.Click += new System.EventHandler(this.menuItemViewAddressBar_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Name = "menuItem17";
			this.menuItem17.Size = new System.Drawing.Size(200, 6);
			// 
			// menuItemView_FixedRebarBands
			// 
			this.menuItemView_FixedRebarBands.Name = "menuItemView_FixedRebarBands";
			this.menuItemView_FixedRebarBands.Size = new System.Drawing.Size(203, 22);
			this.menuItemView_FixedRebarBands.Text = "ツールバーを固定(&L)";
			this.menuItemView_FixedRebarBands.Click += new System.EventHandler(this.menuItemView_FixedRebarBands_Click);
			// 
			// menuItemViewStatusBar
			// 
			this.menuItemViewStatusBar.Name = "menuItemViewStatusBar";
			this.menuItemViewStatusBar.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewStatusBar.Text = "ステータスバー(&S)";
			this.menuItemViewStatusBar.Click += new System.EventHandler(this.menuItemViewStatusBar_Click);
			// 
			// menuItem48
			// 
			this.menuItem48.Name = "menuItem48";
			this.menuItem48.Size = new System.Drawing.Size(311, 6);
			// 
			// menuItemLayout
			// 
			this.menuItemLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLayoutStd,
            this.menuItem58,
            this.menuItemLayoutTate3,
            this.menuItemLayoutYoko3,
            this.menuItemLayoutTateYoko2,
            this.menuItem45,
            this.menuItemLayoutExtend01});
			this.menuItemLayout.Name = "menuItemLayout";
			this.menuItemLayout.Size = new System.Drawing.Size(314, 22);
			this.menuItemLayout.Text = "画面構成(&D)";
			// 
			// menuItemLayoutStd
			// 
			this.menuItemLayoutStd.Name = "menuItemLayoutStd";
			this.menuItemLayoutStd.Size = new System.Drawing.Size(149, 22);
			this.menuItemLayoutStd.Text = "標準(&S)";
			this.menuItemLayoutStd.Click += new System.EventHandler(this.menuItemLayoutStd_Click);
			// 
			// menuItem58
			// 
			this.menuItem58.Name = "menuItem58";
			this.menuItem58.Size = new System.Drawing.Size(146, 6);
			// 
			// menuItemLayoutTate3
			// 
			this.menuItemLayoutTate3.Name = "menuItemLayoutTate3";
			this.menuItemLayoutTate3.Size = new System.Drawing.Size(149, 22);
			this.menuItemLayoutTate3.Text = "縦3分割(&A)";
			this.menuItemLayoutTate3.Click += new System.EventHandler(this.menuItemLayoutTate3_Click);
			// 
			// menuItemLayoutYoko3
			// 
			this.menuItemLayoutYoko3.Name = "menuItemLayoutYoko3";
			this.menuItemLayoutYoko3.Size = new System.Drawing.Size(149, 22);
			this.menuItemLayoutYoko3.Text = "横3分割(&B)";
			this.menuItemLayoutYoko3.Click += new System.EventHandler(this.menuItemLayoutYoko3_Click);
			// 
			// menuItemLayoutTateYoko2
			// 
			this.menuItemLayoutTateYoko2.Name = "menuItemLayoutTateYoko2";
			this.menuItemLayoutTateYoko2.Size = new System.Drawing.Size(149, 22);
			this.menuItemLayoutTateYoko2.Text = "縦横2分割(&C)";
			this.menuItemLayoutTateYoko2.Click += new System.EventHandler(this.menuItemLayoutTateYoko2_Click);
			// 
			// menuItem45
			// 
			this.menuItem45.Name = "menuItem45";
			this.menuItem45.Size = new System.Drawing.Size(146, 6);
			// 
			// menuItemLayoutExtend01
			// 
			this.menuItemLayoutExtend01.Name = "menuItemLayoutExtend01";
			this.menuItemLayoutExtend01.Size = new System.Drawing.Size(149, 22);
			this.menuItemLayoutExtend01.Text = "特殊分割(&D)";
			this.menuItemLayoutExtend01.Click += new System.EventHandler(this.menuItemLayoutExtend01_Click);
			// 
			// menuItemViewSwitch
			// 
			this.menuItemViewSwitch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewHideTable,
            this.menuItemViewFillList,
            this.menuItemViewFillThread});
			this.menuItemViewSwitch.Name = "menuItemViewSwitch";
			this.menuItemViewSwitch.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewSwitch.Text = "表示切り換え(&V)";
			// 
			// menuItemViewHideTable
			// 
			this.menuItemViewHideTable.Name = "menuItemViewHideTable";
			this.menuItemViewHideTable.Size = new System.Drawing.Size(201, 22);
			this.menuItemViewHideTable.Text = "板一覧を非表示(&B)";
			this.menuItemViewHideTable.Click += new System.EventHandler(this.menuItemViewHideTable_Click);
			// 
			// menuItemViewFillList
			// 
			this.menuItemViewFillList.Name = "menuItemViewFillList";
			this.menuItemViewFillList.Size = new System.Drawing.Size(201, 22);
			this.menuItemViewFillList.Text = "スレッド一覧を拡大(&L)";
			this.menuItemViewFillList.Click += new System.EventHandler(this.menuItemViewFillList_Click);
			// 
			// menuItemViewFillThread
			// 
			this.menuItemViewFillThread.Name = "menuItemViewFillThread";
			this.menuItemViewFillThread.Size = new System.Drawing.Size(201, 22);
			this.menuItemViewFillThread.Text = "スレッドを拡大(&T)";
			this.menuItemViewFillThread.Click += new System.EventHandler(this.menuItemViewFillThread_Click);
			// 
			// menuItemFocus
			// 
			this.menuItemFocus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFocusTable,
            this.menuItemFocusBookmark,
            this.menuItemFocusWare,
            this.menuItemFocusList,
            this.menuItemFocusThread});
			this.menuItemFocus.Name = "menuItemFocus";
			this.menuItemFocus.Size = new System.Drawing.Size(314, 22);
			this.menuItemFocus.Text = "フォーカス(&F)";
			// 
			// menuItemFocusTable
			// 
			this.menuItemFocusTable.Name = "menuItemFocusTable";
			this.menuItemFocusTable.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
			this.menuItemFocusTable.Size = new System.Drawing.Size(252, 22);
			this.menuItemFocusTable.Text = "板一覧(&B)";
			this.menuItemFocusTable.Click += new System.EventHandler(this.menuItemFocusTable_Click);
			// 
			// menuItemFocusBookmark
			// 
			this.menuItemFocusBookmark.Name = "menuItemFocusBookmark";
			this.menuItemFocusBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
			this.menuItemFocusBookmark.Size = new System.Drawing.Size(252, 22);
			this.menuItemFocusBookmark.Text = "お気に入り(&F)";
			this.menuItemFocusBookmark.Click += new System.EventHandler(this.menuItemFocusBookmark_Click);
			// 
			// menuItemFocusWare
			// 
			this.menuItemFocusWare.Name = "menuItemFocusWare";
			this.menuItemFocusWare.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.H)));
			this.menuItemFocusWare.Size = new System.Drawing.Size(252, 22);
			this.menuItemFocusWare.Text = "過去ログ倉庫(&H)";
			this.menuItemFocusWare.Click += new System.EventHandler(this.menuItemFocusWare_Click);
			// 
			// menuItemFocusList
			// 
			this.menuItemFocusList.Name = "menuItemFocusList";
			this.menuItemFocusList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.L)));
			this.menuItemFocusList.Size = new System.Drawing.Size(252, 22);
			this.menuItemFocusList.Text = "スレッド一覧(&L)";
			this.menuItemFocusList.Click += new System.EventHandler(this.menuItemFocusList_Click);
			// 
			// menuItemFocusThread
			// 
			this.menuItemFocusThread.Name = "menuItemFocusThread";
			this.menuItemFocusThread.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
			this.menuItemFocusThread.Size = new System.Drawing.Size(252, 22);
			this.menuItemFocusThread.Text = "スレッド(&R)";
			this.menuItemFocusThread.Click += new System.EventHandler(this.menuItemFocusThread_Click);
			// 
			// menuItemFontSize
			// 
			this.menuItemFontSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFontSizeXLarge,
            this.menuItemFontSizeLarge,
            this.menuItemFontSizeMedium,
            this.menuItemFontSizeSmall,
            this.menuItemFontSizeXSmall});
			this.menuItemFontSize.Name = "menuItemFontSize";
			this.menuItemFontSize.Size = new System.Drawing.Size(314, 22);
			this.menuItemFontSize.Text = "文字サイズ(&N)";
			this.menuItemFontSize.DropDownOpening += new System.EventHandler(this.menuItemFontSize_Popup);
			// 
			// menuItemFontSizeXLarge
			// 
			this.menuItemFontSizeXLarge.Name = "menuItemFontSizeXLarge";
			this.menuItemFontSizeXLarge.Size = new System.Drawing.Size(118, 22);
			this.menuItemFontSizeXLarge.Text = "極大(&Z)";
			this.menuItemFontSizeXLarge.Click += new System.EventHandler(this.menuItemFontSizeChange_Click);
			// 
			// menuItemFontSizeLarge
			// 
			this.menuItemFontSizeLarge.Name = "menuItemFontSizeLarge";
			this.menuItemFontSizeLarge.Size = new System.Drawing.Size(118, 22);
			this.menuItemFontSizeLarge.Text = "大(&L)";
			this.menuItemFontSizeLarge.Click += new System.EventHandler(this.menuItemFontSizeChange_Click);
			// 
			// menuItemFontSizeMedium
			// 
			this.menuItemFontSizeMedium.Name = "menuItemFontSizeMedium";
			this.menuItemFontSizeMedium.Size = new System.Drawing.Size(118, 22);
			this.menuItemFontSizeMedium.Text = "中(&M)";
			this.menuItemFontSizeMedium.Click += new System.EventHandler(this.menuItemFontSizeChange_Click);
			// 
			// menuItemFontSizeSmall
			// 
			this.menuItemFontSizeSmall.Name = "menuItemFontSizeSmall";
			this.menuItemFontSizeSmall.Size = new System.Drawing.Size(118, 22);
			this.menuItemFontSizeSmall.Text = "小(&S)";
			this.menuItemFontSizeSmall.Click += new System.EventHandler(this.menuItemFontSizeChange_Click);
			// 
			// menuItemFontSizeXSmall
			// 
			this.menuItemFontSizeXSmall.Name = "menuItemFontSizeXSmall";
			this.menuItemFontSizeXSmall.Size = new System.Drawing.Size(118, 22);
			this.menuItemFontSizeXSmall.Text = "極小(&X)";
			this.menuItemFontSizeXSmall.Click += new System.EventHandler(this.menuItemFontSizeChange_Click);
			// 
			// menuItem51
			// 
			this.menuItem51.Name = "menuItem51";
			this.menuItem51.Size = new System.Drawing.Size(311, 6);
			// 
			// menuItemViewTableItaBotan
			// 
			this.menuItemViewTableItaBotan.Name = "menuItemViewTableItaBotan";
			this.menuItemViewTableItaBotan.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewTableItaBotan.Text = "板一覧の板ボタンを表示(&B)";
			this.menuItemViewTableItaBotan.Click += new System.EventHandler(this.menuItemViewTableItaBotan_Click);
			// 
			// menuItemViewTableDockRight
			// 
			this.menuItemViewTableDockRight.Name = "menuItemViewTableDockRight";
			this.menuItemViewTableDockRight.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewTableDockRight.Text = "板一覧を右に配置(&R)";
			this.menuItemViewTableDockRight.Click += new System.EventHandler(this.menuItemViewTableDockRight_Click);
			// 
			// menuItem55
			// 
			this.menuItem55.Name = "menuItem55";
			this.menuItem55.Size = new System.Drawing.Size(311, 6);
			// 
			// menuItemViewDockWriteBar
			// 
			this.menuItemViewDockWriteBar.Name = "menuItemViewDockWriteBar";
			this.menuItemViewDockWriteBar.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
			this.menuItemViewDockWriteBar.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewDockWriteBar.Text = "ドッキング書き込みバー(&W)";
			this.menuItemViewDockWriteBar.Click += new System.EventHandler(this.menuItemViewDockWriteBar_Click);
			// 
			// menuItemViewUpChecker
			// 
			this.menuItemViewUpChecker.Name = "menuItemViewUpChecker";
			this.menuItemViewUpChecker.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewUpChecker.Text = "定期更新チェッカー(&C)";
			this.menuItemViewUpChecker.Click += new System.EventHandler(this.menuItemViewUpChecker_Click);
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			this.toolStripMenuItem10.Size = new System.Drawing.Size(311, 6);
			// 
			// menuItemViewPatrol
			// 
			this.menuItemViewPatrol.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPatrolHiddenPastlog});
			this.menuItemViewPatrol.Name = "menuItemViewPatrol";
			this.menuItemViewPatrol.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewPatrol.Text = "お気に入り巡回時(&P)";
			this.menuItemViewPatrol.DropDownOpening += new System.EventHandler(this.menuItemViewPatrol_DropDownOpening);
			// 
			// menuItemPatrolHiddenPastlog
			// 
			this.menuItemPatrolHiddenPastlog.Name = "menuItemPatrolHiddenPastlog";
			this.menuItemPatrolHiddenPastlog.Size = new System.Drawing.Size(258, 22);
			this.menuItemPatrolHiddenPastlog.Text = "dat落ちスレッドを表示しない(&H)";
			this.menuItemPatrolHiddenPastlog.Click += new System.EventHandler(this.menuItemPatrolHiddenPastlog_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Name = "menuItem23";
			this.menuItem23.Size = new System.Drawing.Size(311, 6);
			// 
			// menuItemViewLiveMode
			// 
			this.menuItemViewLiveMode.Name = "menuItemViewLiveMode";
			this.menuItemViewLiveMode.Size = new System.Drawing.Size(314, 22);
			this.menuItemViewLiveMode.Text = "実況モード(&L)";
			this.menuItemViewLiveMode.Click += new System.EventHandler(this.menuItemViewLiveMode_Click);
			// 
			// menuItemSearch
			// 
			this.menuItemSearch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEditSearchCache,
            this.menuItemEditSearchList,
            this.menuItemEditFindBoard,
            this.toolStripMenuItem15,
            this.menuItemEditSearchSubjectBotanAdd});
			this.menuItemSearch.Name = "menuItemSearch";
			this.menuItemSearch.Size = new System.Drawing.Size(62, 22);
			this.menuItemSearch.Text = "検索(&S)";
			this.menuItemSearch.DropDownOpening += new System.EventHandler(this.menuItemSearch_Popup);
			// 
			// menuItemEditSearchCache
			// 
			this.menuItemEditSearchCache.Name = "menuItemEditSearchCache";
			this.menuItemEditSearchCache.Size = new System.Drawing.Size(310, 22);
			this.menuItemEditSearchCache.Text = "既得ログを検索(&S)...";
			this.menuItemEditSearchCache.Click += new System.EventHandler(this.menuItemEditSearchCache_Click);
			// 
			// menuItemEditSearchList
			// 
			this.menuItemEditSearchList.Name = "menuItemEditSearchList";
			this.menuItemEditSearchList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.menuItemEditSearchList.Size = new System.Drawing.Size(310, 22);
			this.menuItemEditSearchList.Text = "スレッド一覧を絞り込み検索(&L)...";
			this.menuItemEditSearchList.Click += new System.EventHandler(this.menuItemEditSearchList_Click);
			// 
			// menuItemEditFindBoard
			// 
			this.menuItemEditFindBoard.Name = "menuItemEditFindBoard";
			this.menuItemEditFindBoard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.menuItemEditFindBoard.Size = new System.Drawing.Size(310, 22);
			this.menuItemEditFindBoard.Text = "板一覧から板を検索(&B)";
			this.menuItemEditFindBoard.Click += new System.EventHandler(this.menuItemEditFindBoard_Click);
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			this.toolStripMenuItem15.Size = new System.Drawing.Size(307, 6);
			// 
			// menuItemEditSearchSubjectBotanAdd
			// 
			this.menuItemEditSearchSubjectBotanAdd.Name = "menuItemEditSearchSubjectBotanAdd";
			this.menuItemEditSearchSubjectBotanAdd.Size = new System.Drawing.Size(310, 22);
			this.menuItemEditSearchSubjectBotanAdd.Text = "スレタイ検索の条件を板ボタンに追加(&A)...";
			this.menuItemEditSearchSubjectBotanAdd.Click += new System.EventHandler(this.menuItemEditSearchSubjectBotanAdd_Click);
			// 
			// menuItemBookmarks
			// 
			this.menuItemBookmarks.Name = "menuItemBookmarks";
			this.menuItemBookmarks.Size = new System.Drawing.Size(98, 22);
			this.menuItemBookmarks.Text = "お気に入り(&B)";
			// 
			// miGroup
			// 
			this.miGroup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGroupAdd,
            this.miGroupEdit,
            this.toolStripMenuItem3});
			this.miGroup.Name = "miGroup";
			this.miGroup.Size = new System.Drawing.Size(87, 22);
			this.miGroup.Text = "グループ(&G)";
			this.miGroup.DropDownOpening += new System.EventHandler(this.miGroup_DropDownOpening);
			// 
			// miGroupAdd
			// 
			this.miGroupAdd.Name = "miGroupAdd";
			this.miGroupAdd.Size = new System.Drawing.Size(190, 22);
			this.miGroupAdd.Text = "グループに追加(&A)...";
			this.miGroupAdd.Click += new System.EventHandler(this.miGroupAdd_Click);
			// 
			// miGroupEdit
			// 
			this.miGroupEdit.Name = "miGroupEdit";
			this.miGroupEdit.Size = new System.Drawing.Size(190, 22);
			this.miGroupEdit.Text = "グループを編集(&E)...";
			this.miGroupEdit.Click += new System.EventHandler(this.miGroupEdit_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(187, 6);
			// 
			// menuItemList
			// 
			this.menuItemList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemListReload,
            this.menuItemListStop,
            this.menuItem7,
            this.menuItemListShowLocalRule,
            this.menuItemListShowPicture,
            this.menuItemShowSettingTxt,
            this.menuItem27,
            this.menuItemListAllThreads,
            this.menuItemListWrittenThreads,
            this.menuItem10,
            this.menuItemListCache,
            this.menuItemListHistoryOpen,
            this.menuItemListDraftOpen,
            this.menuItem49,
            this.menuItemListClose});
			this.menuItemList.Name = "menuItemList";
			this.menuItemList.Size = new System.Drawing.Size(49, 22);
			this.menuItemList.Text = "板(&L)";
			this.menuItemList.DropDownOpening += new System.EventHandler(this.menuItemList_Popup);
			// 
			// menuItemListReload
			// 
			this.menuItemListReload.Name = "menuItemListReload";
			this.menuItemListReload.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
			this.menuItemListReload.Size = new System.Drawing.Size(274, 22);
			this.menuItemListReload.Text = "最新の状態に更新(&R)";
			this.menuItemListReload.Click += new System.EventHandler(this.menuItemListReload_Click);
			// 
			// menuItemListStop
			// 
			this.menuItemListStop.Name = "menuItemListStop";
			this.menuItemListStop.Size = new System.Drawing.Size(274, 22);
			this.menuItemListStop.Text = "読み込み中止(&P)";
			this.menuItemListStop.Click += new System.EventHandler(this.menuItemListStop_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Name = "menuItem7";
			this.menuItem7.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemListShowLocalRule
			// 
			this.menuItemListShowLocalRule.Name = "menuItemListShowLocalRule";
			this.menuItemListShowLocalRule.Size = new System.Drawing.Size(274, 22);
			this.menuItemListShowLocalRule.Text = "ローカルルールを表示(&L)";
			this.menuItemListShowLocalRule.Click += new System.EventHandler(this.menuItemListShowLocalRule_Click);
			// 
			// menuItemListShowPicture
			// 
			this.menuItemListShowPicture.Name = "menuItemListShowPicture";
			this.menuItemListShowPicture.Size = new System.Drawing.Size(274, 22);
			this.menuItemListShowPicture.Text = "看板を表示(&T)";
			this.menuItemListShowPicture.Click += new System.EventHandler(this.menuItemListShowPicture_Click);
			// 
			// menuItemShowSettingTxt
			// 
			this.menuItemShowSettingTxt.Name = "menuItemShowSettingTxt";
			this.menuItemShowSettingTxt.Size = new System.Drawing.Size(274, 22);
			this.menuItemShowSettingTxt.Text = "SETTING.TXT を見る(&S)...";
			this.menuItemShowSettingTxt.Click += new System.EventHandler(this.menuItemShowSettingTxt_Click);
			// 
			// menuItem27
			// 
			this.menuItem27.Name = "menuItem27";
			this.menuItem27.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemListAllThreads
			// 
			this.menuItemListAllThreads.Name = "menuItemListAllThreads";
			this.menuItemListAllThreads.Size = new System.Drawing.Size(274, 22);
			this.menuItemListAllThreads.Text = "全既得スレッドを表示(&A)";
			this.menuItemListAllThreads.Click += new System.EventHandler(this.menuItemListAllThreads_Click);
			// 
			// menuItemListWrittenThreads
			// 
			this.menuItemListWrittenThreads.Name = "menuItemListWrittenThreads";
			this.menuItemListWrittenThreads.Size = new System.Drawing.Size(274, 22);
			this.menuItemListWrittenThreads.Text = "最近書き込んだスレッドを表示(&W)";
			this.menuItemListWrittenThreads.Click += new System.EventHandler(this.menuItemListWrittenThreads_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Name = "menuItem10";
			this.menuItem10.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemListCache
			// 
			this.menuItemListCache.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemListCacheOpen,
            this.menuItemListCacheClear,
            this.menuItem2,
            this.menuItemListIndexing});
			this.menuItemListCache.Name = "menuItemListCache";
			this.menuItemListCache.Size = new System.Drawing.Size(274, 22);
			this.menuItemListCache.Text = "この板の既得ログ(&G)";
			// 
			// menuItemListCacheOpen
			// 
			this.menuItemListCacheOpen.Name = "menuItemListCacheOpen";
			this.menuItemListCacheOpen.Size = new System.Drawing.Size(213, 22);
			this.menuItemListCacheOpen.Text = "すべて表示(&S)";
			this.menuItemListCacheOpen.Click += new System.EventHandler(this.menuItemListCacheOpen_Click);
			// 
			// menuItemListCacheClear
			// 
			this.menuItemListCacheClear.Name = "menuItemListCacheClear";
			this.menuItemListCacheClear.Size = new System.Drawing.Size(213, 22);
			this.menuItemListCacheClear.Text = "すべて削除(&D)";
			this.menuItemListCacheClear.Click += new System.EventHandler(this.menuItemListCacheClear_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Name = "menuItem2";
			this.menuItem2.Size = new System.Drawing.Size(210, 6);
			// 
			// menuItemListIndexing
			// 
			this.menuItemListIndexing.Name = "menuItemListIndexing";
			this.menuItemListIndexing.Size = new System.Drawing.Size(213, 22);
			this.menuItemListIndexing.Text = "インデックスを再生成(&L)";
			this.menuItemListIndexing.Click += new System.EventHandler(this.menuItemListIndexing_Click);
			// 
			// menuItemListHistoryOpen
			// 
			this.menuItemListHistoryOpen.Name = "menuItemListHistoryOpen";
			this.menuItemListHistoryOpen.Size = new System.Drawing.Size(274, 22);
			this.menuItemListHistoryOpen.Text = "書き込み履歴を表示(&H)";
			this.menuItemListHistoryOpen.Click += new System.EventHandler(this.menuItemListHistoryOpen_Click);
			// 
			// menuItemListDraftOpen
			// 
			this.menuItemListDraftOpen.Name = "menuItemListDraftOpen";
			this.menuItemListDraftOpen.Size = new System.Drawing.Size(274, 22);
			this.menuItemListDraftOpen.Text = "草稿を表示(&D)";
			this.menuItemListDraftOpen.Click += new System.EventHandler(this.menuItemListDraftOpen_Click);
			// 
			// menuItem49
			// 
			this.menuItem49.Name = "menuItem49";
			this.menuItem49.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemListClose
			// 
			this.menuItemListClose.Name = "menuItemListClose";
			this.menuItemListClose.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
			this.menuItemListClose.Size = new System.Drawing.Size(274, 22);
			this.menuItemListClose.Text = "閉じる(&C)";
			this.menuItemListClose.Click += new System.EventHandler(this.menuItemListClose_Click);
			// 
			// menuItemThread
			// 
			this.menuItemThread.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemThreadReload,
            this.menuItemThreadReloadAll,
            this.menuItemThreadStop,
            this.menuItem14,
            this.menuItemNewThread,
            this.menuItemThreadPostRes,
            this.menuItem77,
            this.menuItemThreadBookmark,
            this.menuItemThreadSetUpChecker,
            this.menuItem38,
            this.menuItemThreadFind,
            this.menuItemThreadLinkExtract,
            this.menuItemThreadResExtract,
            this.menuItemThreadNextThreadCheck,
            this.menuItem26,
            this.menuItemThreadResetPastlogFlags,
            this.menuItemThreadSirusiManager,
            this.menuItem20,
            this.menuItemScrollToTop,
            this.menuItemScrollToBottom,
            this.toolStripMenuItem2,
            this.menuItemThreadAutoScroll,
            this.menuItemThreadAutoReload,
            this.menuItemThreadAutoFocus,
            this.menuItem75,
            this.menuItemThreadDeleteClose,
            this.menuItemThreadReget,
            this.menuItem1,
            this.menuItemThreadClose});
			this.menuItemThread.Name = "menuItemThread";
			this.menuItemThread.Size = new System.Drawing.Size(86, 22);
			this.menuItemThread.Text = "スレッド(&R)";
			this.menuItemThread.DropDownOpening += new System.EventHandler(this.menuItemThread_Popup);
			// 
			// menuItemThreadReload
			// 
			this.menuItemThreadReload.Name = "menuItemThreadReload";
			this.menuItemThreadReload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.menuItemThreadReload.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadReload.Text = "最新の状態に更新(&R)";
			this.menuItemThreadReload.Click += new System.EventHandler(this.menuItemThreadReload_Click);
			// 
			// menuItemThreadReloadAll
			// 
			this.menuItemThreadReloadAll.Name = "menuItemThreadReloadAll";
			this.menuItemThreadReloadAll.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadReloadAll.Text = "すべてのスレッドを更新(&V)";
			this.menuItemThreadReloadAll.Click += new System.EventHandler(this.menuItemThreadReloadAll_Click);
			// 
			// menuItemThreadStop
			// 
			this.menuItemThreadStop.Name = "menuItemThreadStop";
			this.menuItemThreadStop.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadStop.Text = "読み込み中止(&P)";
			this.menuItemThreadStop.Click += new System.EventHandler(this.menuItemThreadStop_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Name = "menuItem14";
			this.menuItem14.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemNewThread
			// 
			this.menuItemNewThread.Name = "menuItemNewThread";
			this.menuItemNewThread.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.menuItemNewThread.Size = new System.Drawing.Size(272, 22);
			this.menuItemNewThread.Text = "新規スレッドを立てる(&N)...";
			this.menuItemNewThread.Click += new System.EventHandler(this.menuItemNewThread_Click);
			// 
			// menuItemThreadPostRes
			// 
			this.menuItemThreadPostRes.Name = "menuItemThreadPostRes";
			this.menuItemThreadPostRes.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
			this.menuItemThreadPostRes.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadPostRes.Text = "レスを投稿(&W)...";
			this.menuItemThreadPostRes.Click += new System.EventHandler(this.menuItemThreadPostRes_Click);
			// 
			// menuItem77
			// 
			this.menuItem77.Name = "menuItem77";
			this.menuItem77.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemThreadBookmark
			// 
			this.menuItemThreadBookmark.Name = "menuItemThreadBookmark";
			this.menuItemThreadBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
			this.menuItemThreadBookmark.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadBookmark.Text = "お気に入り(&B)...";
			this.menuItemThreadBookmark.Click += new System.EventHandler(this.menuItemThreadBookmark_Click);
			// 
			// menuItemThreadSetUpChecker
			// 
			this.menuItemThreadSetUpChecker.Name = "menuItemThreadSetUpChecker";
			this.menuItemThreadSetUpChecker.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadSetUpChecker.Text = "定期更新チェック(&H)";
			this.menuItemThreadSetUpChecker.Click += new System.EventHandler(this.menuItemThreadSetUpChecker_Click);
			// 
			// menuItem38
			// 
			this.menuItem38.Name = "menuItem38";
			this.menuItem38.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemThreadFind
			// 
			this.menuItemThreadFind.Name = "menuItemThreadFind";
			this.menuItemThreadFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.menuItemThreadFind.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadFind.Text = "スレッド内を検索(&F)...";
			this.menuItemThreadFind.Click += new System.EventHandler(this.menuItemThreadFind_Click);
			// 
			// menuItemThreadLinkExtract
			// 
			this.menuItemThreadLinkExtract.Name = "menuItemThreadLinkExtract";
			this.menuItemThreadLinkExtract.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.menuItemThreadLinkExtract.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadLinkExtract.Text = "リンクを抽出(&L)...";
			this.menuItemThreadLinkExtract.Click += new System.EventHandler(this.menuItemThreadLinkExtract_Click);
			// 
			// menuItemThreadResExtract
			// 
			this.menuItemThreadResExtract.Name = "menuItemThreadResExtract";
			this.menuItemThreadResExtract.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.menuItemThreadResExtract.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadResExtract.Text = "レスを抽出(&E)...";
			this.menuItemThreadResExtract.Click += new System.EventHandler(this.menuItemThreadResExtract_Click);
			// 
			// menuItemThreadNextThreadCheck
			// 
			this.menuItemThreadNextThreadCheck.Name = "menuItemThreadNextThreadCheck";
			this.menuItemThreadNextThreadCheck.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadNextThreadCheck.Text = "次スレを検索(&N)";
			this.menuItemThreadNextThreadCheck.Click += new System.EventHandler(this.menuItemThreadNextThreadCheck_Click);
			// 
			// menuItem26
			// 
			this.menuItem26.Name = "menuItem26";
			this.menuItem26.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemThreadResetPastlogFlags
			// 
			this.menuItemThreadResetPastlogFlags.Name = "menuItemThreadResetPastlogFlags";
			this.menuItemThreadResetPastlogFlags.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadResetPastlogFlags.Text = "dat落ちフラグを全て解除(&O)";
			this.menuItemThreadResetPastlogFlags.Click += new System.EventHandler(this.menuItemThreadResetPastlogFlags_Click);
			// 
			// menuItemThreadSirusiManager
			// 
			this.menuItemThreadSirusiManager.Name = "menuItemThreadSirusiManager";
			this.menuItemThreadSirusiManager.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadSirusiManager.Text = "しるしマネージャ(&U)...";
			this.menuItemThreadSirusiManager.Click += new System.EventHandler(this.menuItemThreadSirusiManager_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Name = "menuItem20";
			this.menuItem20.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemScrollToTop
			// 
			this.menuItemScrollToTop.Name = "menuItemScrollToTop";
			this.menuItemScrollToTop.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.menuItemScrollToTop.Size = new System.Drawing.Size(272, 22);
			this.menuItemScrollToTop.Text = "最上部へスクロール(&T)";
			this.menuItemScrollToTop.Click += new System.EventHandler(this.menuItemScrollToTop_Click);
			// 
			// menuItemScrollToBottom
			// 
			this.menuItemScrollToBottom.Name = "menuItemScrollToBottom";
			this.menuItemScrollToBottom.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.menuItemScrollToBottom.Size = new System.Drawing.Size(272, 22);
			this.menuItemScrollToBottom.Text = "最下部へスクロール(&D)";
			this.menuItemScrollToBottom.Click += new System.EventHandler(this.menuItemScrollToBottom_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemThreadAutoScroll
			// 
			this.menuItemThreadAutoScroll.Name = "menuItemThreadAutoScroll";
			this.menuItemThreadAutoScroll.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadAutoScroll.Text = "オートスクロール(&S)";
			this.menuItemThreadAutoScroll.Click += new System.EventHandler(this.menuItemThreadAutoScroll_Click);
			// 
			// menuItemThreadAutoReload
			// 
			this.menuItemThreadAutoReload.Name = "menuItemThreadAutoReload";
			this.menuItemThreadAutoReload.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadAutoReload.Text = "オートリロード(&A)";
			this.menuItemThreadAutoReload.Click += new System.EventHandler(this.menuItemThreadAutoReload_Click);
			// 
			// menuItemThreadAutoFocus
			// 
			this.menuItemThreadAutoFocus.Name = "menuItemThreadAutoFocus";
			this.menuItemThreadAutoFocus.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadAutoFocus.Text = "オートフォーカス(&Y)";
			this.menuItemThreadAutoFocus.Click += new System.EventHandler(this.menuItemThreadAutoFocus_Click);
			// 
			// menuItem75
			// 
			this.menuItem75.Name = "menuItem75";
			this.menuItem75.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemThreadDeleteClose
			// 
			this.menuItemThreadDeleteClose.Name = "menuItemThreadDeleteClose";
			this.menuItemThreadDeleteClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
			this.menuItemThreadDeleteClose.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadDeleteClose.Text = "ログを削除して閉じる(&X)";
			this.menuItemThreadDeleteClose.Click += new System.EventHandler(this.menuItemThreadDeleteClose_Click);
			// 
			// menuItemThreadReget
			// 
			this.menuItemThreadReget.Name = "menuItemThreadReget";
			this.menuItemThreadReget.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.menuItemThreadReget.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadReget.Text = "ログを再取得(&G)";
			this.menuItemThreadReget.Click += new System.EventHandler(this.menuItemThreadReget_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Name = "menuItem1";
			this.menuItem1.Size = new System.Drawing.Size(269, 6);
			// 
			// menuItemThreadClose
			// 
			this.menuItemThreadClose.Name = "menuItemThreadClose";
			this.menuItemThreadClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.menuItemThreadClose.Size = new System.Drawing.Size(272, 22);
			this.menuItemThreadClose.Text = "閉じる(&C)";
			this.menuItemThreadClose.Click += new System.EventHandler(this.menuItemThreadClose_Click);
			// 
			// menuItemTools
			// 
			this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsSub,
            this.menuItem68,
            this.menuItemToolsScrapEditor,
            this.menuItemToolsImageViewer,
            this.miMouseGestureSetting,
            this.menuItem70,
            this.menuItemToolsSaveWindowUrls,
            this.menuItemToolsOpenStartupUrls,
            this.menuItem52,
            this.menuItemToolOyster,
            this.menuItemToolBe,
            this.toolStripMenuItem16,
            this.menuItemToolsInetOption,
            this.menuItemToolsOption});
			this.menuItemTools.Name = "menuItemTools";
			this.menuItemTools.Size = new System.Drawing.Size(74, 22);
			this.menuItemTools.Text = "ツール(&T)";
			this.menuItemTools.DropDownOpening += new System.EventHandler(this.menuItemTools_Popup);
			// 
			// menuItemToolsSub
			// 
			this.menuItemToolsSub.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsRegist,
            this.menuItem60});
			this.menuItemToolsSub.Name = "menuItemToolsSub";
			this.menuItemToolsSub.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolsSub.Text = "外部ツール(&T)";
			// 
			// menuItemToolsRegist
			// 
			this.menuItemToolsRegist.Name = "menuItemToolsRegist";
			this.menuItemToolsRegist.Size = new System.Drawing.Size(190, 22);
			this.menuItemToolsRegist.Text = "外部ツールを登録(&R)";
			this.menuItemToolsRegist.Click += new System.EventHandler(this.menuItemToolsRegist_Click);
			// 
			// menuItem60
			// 
			this.menuItem60.Name = "menuItem60";
			this.menuItem60.Size = new System.Drawing.Size(187, 6);
			// 
			// menuItem68
			// 
			this.menuItem68.Name = "menuItem68";
			this.menuItem68.Size = new System.Drawing.Size(249, 6);
			// 
			// menuItemToolsScrapEditor
			// 
			this.menuItemToolsScrapEditor.Name = "menuItemToolsScrapEditor";
			this.menuItemToolsScrapEditor.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolsScrapEditor.Text = "メモ帳(&V)...";
			this.menuItemToolsScrapEditor.Click += new System.EventHandler(this.menuItemToolsScrapEditor_Click);
			// 
			// menuItemToolsImageViewer
			// 
			this.menuItemToolsImageViewer.Name = "menuItemToolsImageViewer";
			this.menuItemToolsImageViewer.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolsImageViewer.Text = "画像ビューア(&P)...";
			this.menuItemToolsImageViewer.Click += new System.EventHandler(this.menuItemToolsImageViewer_Click);
			// 
			// miMouseGestureSetting
			// 
			this.miMouseGestureSetting.Name = "miMouseGestureSetting";
			this.miMouseGestureSetting.Size = new System.Drawing.Size(252, 22);
			this.miMouseGestureSetting.Text = "マウスジェスチャーの設定(&M)...";
			this.miMouseGestureSetting.Click += new System.EventHandler(this.miMouseGestureSetting_Click);
			// 
			// menuItem70
			// 
			this.menuItem70.Name = "menuItem70";
			this.menuItem70.Size = new System.Drawing.Size(249, 6);
			// 
			// menuItemToolsSaveWindowUrls
			// 
			this.menuItemToolsSaveWindowUrls.Name = "menuItemToolsSaveWindowUrls";
			this.menuItemToolsSaveWindowUrls.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolsSaveWindowUrls.Text = "開いている状態を保存(&W)";
			this.menuItemToolsSaveWindowUrls.Click += new System.EventHandler(this.menuItemToolsSaveWindowUrls_Click);
			// 
			// menuItemToolsOpenStartupUrls
			// 
			this.menuItemToolsOpenStartupUrls.Name = "menuItemToolsOpenStartupUrls";
			this.menuItemToolsOpenStartupUrls.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolsOpenStartupUrls.Text = "保存されている状態を復元(&O)";
			this.menuItemToolsOpenStartupUrls.Click += new System.EventHandler(this.menuItemToolsOpenStartupUrls_Click);
			// 
			// menuItem52
			// 
			this.menuItem52.Name = "menuItem52";
			this.menuItem52.Size = new System.Drawing.Size(249, 6);
			// 
			// menuItemToolOyster
			// 
			this.menuItemToolOyster.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolOysterEnable,
            this.menuItemToolOysterDisable});
			this.menuItemToolOyster.Name = "menuItemToolOyster";
			this.menuItemToolOyster.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolOyster.Text = "●認証(&Y)";
			this.menuItemToolOyster.DropDownOpening += new System.EventHandler(this.menuItemToolOyster_DropDownOpening);
			// 
			// menuItemToolOysterEnable
			// 
			this.menuItemToolOysterEnable.Name = "menuItemToolOysterEnable";
			this.menuItemToolOysterEnable.Size = new System.Drawing.Size(119, 22);
			this.menuItemToolOysterEnable.Text = "有効(&E)";
			this.menuItemToolOysterEnable.Click += new System.EventHandler(this.menuItemToolOysterEnable_Click);
			// 
			// menuItemToolOysterDisable
			// 
			this.menuItemToolOysterDisable.Name = "menuItemToolOysterDisable";
			this.menuItemToolOysterDisable.Size = new System.Drawing.Size(119, 22);
			this.menuItemToolOysterDisable.Text = "無効(&D)";
			this.menuItemToolOysterDisable.Click += new System.EventHandler(this.menuItemToolOysterDisable_Click);
			// 
			// menuItemToolBe
			// 
			this.menuItemToolBe.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolBeLogin,
            this.menuItemToolBeLogout});
			this.menuItemToolBe.Name = "menuItemToolBe";
			this.menuItemToolBe.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolBe.Text = "BE認証(&B)";
			// 
			// menuItemToolBeLogin
			// 
			this.menuItemToolBeLogin.Name = "menuItemToolBeLogin";
			this.menuItemToolBeLogin.Size = new System.Drawing.Size(155, 22);
			this.menuItemToolBeLogin.Text = "ログイン(&L)...";
			this.menuItemToolBeLogin.Click += new System.EventHandler(this.menuItemToolBeLogin_Click);
			// 
			// menuItemToolBeLogout
			// 
			this.menuItemToolBeLogout.Name = "menuItemToolBeLogout";
			this.menuItemToolBeLogout.Size = new System.Drawing.Size(155, 22);
			this.menuItemToolBeLogout.Text = "ログアウト(&O)";
			this.menuItemToolBeLogout.Click += new System.EventHandler(this.menuItemToolBeLogout_Click);
			// 
			// toolStripMenuItem16
			// 
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			this.toolStripMenuItem16.Size = new System.Drawing.Size(249, 6);
			// 
			// menuItemToolsInetOption
			// 
			this.menuItemToolsInetOption.Name = "menuItemToolsInetOption";
			this.menuItemToolsInetOption.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolsInetOption.Text = "インターネットオプション(&N)...";
			this.menuItemToolsInetOption.Click += new System.EventHandler(this.menuItemToolsInetOption_Click);
			// 
			// menuItemToolsOption
			// 
			this.menuItemToolsOption.Name = "menuItemToolsOption";
			this.menuItemToolsOption.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.menuItemToolsOption.Size = new System.Drawing.Size(252, 22);
			this.menuItemToolsOption.Text = "環境設定(&S)...";
			this.menuItemToolsOption.Click += new System.EventHandler(this.menuItemToolsOption_Click);
			// 
			// menuItemWindow
			// 
			this.menuItemWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemWindowThreadCloseAll,
            this.menuItemWindowThreadCloseNotActive,
            this.menuItem31,
            this.menuItemWindowSelectPrev,
            this.menuItemWindowSelectNext,
            this.menuItem24,
            this.menuItemWindowListCloseAll,
            this.menuItemWindowListCloseNotActive,
            this.menuItem62,
            this.menuItemWindowListPrev,
            this.menuItemWindowListNext});
			this.menuItemWindow.Name = "menuItemWindow";
			this.menuItemWindow.Size = new System.Drawing.Size(98, 22);
			this.menuItemWindow.Text = "ウインドウ(&Y)";
			this.menuItemWindow.DropDownOpening += new System.EventHandler(this.menuItemWindow_Popup);
			// 
			// menuItemWindowThreadCloseAll
			// 
			this.menuItemWindowThreadCloseAll.Name = "menuItemWindowThreadCloseAll";
			this.menuItemWindowThreadCloseAll.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowThreadCloseAll.Text = "すべてのスレッドを閉じる(&E)";
			this.menuItemWindowThreadCloseAll.Click += new System.EventHandler(this.menuItemWindowThreadCloseAll_Click);
			// 
			// menuItemWindowThreadCloseNotActive
			// 
			this.menuItemWindowThreadCloseNotActive.Name = "menuItemWindowThreadCloseNotActive";
			this.menuItemWindowThreadCloseNotActive.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowThreadCloseNotActive.Text = "アクティブなスレッド以外を閉じる(&D)";
			this.menuItemWindowThreadCloseNotActive.Click += new System.EventHandler(this.menuItemWindowThreadCloseNotActive_Click);
			// 
			// menuItem31
			// 
			this.menuItem31.Name = "menuItem31";
			this.menuItem31.Size = new System.Drawing.Size(284, 6);
			// 
			// menuItemWindowSelectPrev
			// 
			this.menuItemWindowSelectPrev.Name = "menuItemWindowSelectPrev";
			this.menuItemWindowSelectPrev.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.menuItemWindowSelectPrev.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowSelectPrev.Text = "前のスレッドを表示(&P)";
			this.menuItemWindowSelectPrev.Click += new System.EventHandler(this.menuItemWindowSelectPrev_Click);
			// 
			// menuItemWindowSelectNext
			// 
			this.menuItemWindowSelectNext.Name = "menuItemWindowSelectNext";
			this.menuItemWindowSelectNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.menuItemWindowSelectNext.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowSelectNext.Text = "次のスレッドを表示(&N)";
			this.menuItemWindowSelectNext.Click += new System.EventHandler(this.menuItemWindowSelectNext_Click);
			// 
			// menuItem24
			// 
			this.menuItem24.Name = "menuItem24";
			this.menuItem24.Size = new System.Drawing.Size(284, 6);
			// 
			// menuItemWindowListCloseAll
			// 
			this.menuItemWindowListCloseAll.Name = "menuItemWindowListCloseAll";
			this.menuItemWindowListCloseAll.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowListCloseAll.Text = "すべてのスレ一覧を閉じる(&R)";
			this.menuItemWindowListCloseAll.Click += new System.EventHandler(this.menuItemWindowListCloseAll_Click);
			// 
			// menuItemWindowListCloseNotActive
			// 
			this.menuItemWindowListCloseNotActive.Name = "menuItemWindowListCloseNotActive";
			this.menuItemWindowListCloseNotActive.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowListCloseNotActive.Text = "アクティブなスレ一覧以外を閉じる(&F)";
			this.menuItemWindowListCloseNotActive.Click += new System.EventHandler(this.menuItemWindowListCloseNotActive_Click);
			// 
			// menuItem62
			// 
			this.menuItem62.Name = "menuItem62";
			this.menuItem62.Size = new System.Drawing.Size(284, 6);
			// 
			// menuItemWindowListPrev
			// 
			this.menuItemWindowListPrev.Name = "menuItemWindowListPrev";
			this.menuItemWindowListPrev.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowListPrev.Text = "前のスレ一覧を表示(&G)";
			this.menuItemWindowListPrev.Click += new System.EventHandler(this.menuItemWindowListPrev_Click);
			// 
			// menuItemWindowListNext
			// 
			this.menuItemWindowListNext.Name = "menuItemWindowListNext";
			this.menuItemWindowListNext.Size = new System.Drawing.Size(287, 22);
			this.menuItemWindowListNext.Text = "次のスレ一覧を表示(&H)";
			this.menuItemWindowListNext.Click += new System.EventHandler(this.menuItemWindowListNext_Click);
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemHelpOpen,
            this.menuItemHelpOpenWeb,
            this.menuItemHelpOpenErrorLog,
            this.menuItem19,
            this.menuItemHelpOpenLoadFactor,
            this.menuItemHelpOpenServerWatch2,
            this.menuItemHelpExit,
            this.menuItemHelpTest,
            this.menuItem11,
            this.menuItemHelpAbout});
			this.menuItemHelp.Name = "menuItemHelp";
			this.menuItemHelp.Size = new System.Drawing.Size(75, 22);
			this.menuItemHelp.Text = "ヘルプ(&H)";
			this.menuItemHelp.DropDownOpening += new System.EventHandler(this.menuItemHelp_Popup);
			// 
			// menuItemHelpOpen
			// 
			this.menuItemHelpOpen.Name = "menuItemHelpOpen";
			this.menuItemHelpOpen.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.menuItemHelpOpen.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpOpen.Text = "目次(&C)";
			this.menuItemHelpOpen.Click += new System.EventHandler(this.menuItemHelpOpen_Click);
			// 
			// menuItemHelpOpenWeb
			// 
			this.menuItemHelpOpenWeb.Name = "menuItemHelpOpenWeb";
			this.menuItemHelpOpenWeb.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpOpenWeb.Text = "Webページを開く(&W)";
			this.menuItemHelpOpenWeb.Click += new System.EventHandler(this.menuItemHelpOpenWeb_Click);
			// 
			// menuItemHelpOpenErrorLog
			// 
			this.menuItemHelpOpenErrorLog.Name = "menuItemHelpOpenErrorLog";
			this.menuItemHelpOpenErrorLog.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpOpenErrorLog.Text = "エラーログァイルを開く(&G)";
			this.menuItemHelpOpenErrorLog.Click += new System.EventHandler(this.menuItemHelpOpenErrorLog_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Name = "menuItem19";
			this.menuItem19.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemHelpOpenLoadFactor
			// 
			this.menuItemHelpOpenLoadFactor.Name = "menuItemHelpOpenLoadFactor";
			this.menuItemHelpOpenLoadFactor.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpOpenLoadFactor.Text = "2chサーバ負荷監視所(&L)";
			this.menuItemHelpOpenLoadFactor.Click += new System.EventHandler(this.menuItemHelpOpenLoadFactor_Click);
			// 
			// menuItemHelpOpenServerWatch2
			// 
			this.menuItemHelpOpenServerWatch2.Name = "menuItemHelpOpenServerWatch2";
			this.menuItemHelpOpenServerWatch2.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpOpenServerWatch2.Text = "2ch鯖監視係(&K)";
			this.menuItemHelpOpenServerWatch2.Click += new System.EventHandler(this.menuItemHelpOpenServerWatch2_Click);
			// 
			// menuItemHelpExit
			// 
			this.menuItemHelpExit.Name = "menuItemHelpExit";
			this.menuItemHelpExit.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.X)));
			this.menuItemHelpExit.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpExit.Text = "強制終了(&X)";
			this.menuItemHelpExit.Visible = false;
			this.menuItemHelpExit.Click += new System.EventHandler(this.menuItemHelpExit_Click);
			// 
			// menuItemHelpTest
			// 
			this.menuItemHelpTest.Name = "menuItemHelpTest";
			this.menuItemHelpTest.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpTest.Text = "テスト(&T)";
			this.menuItemHelpTest.Visible = false;
			this.menuItemHelpTest.Click += new System.EventHandler(this.menuItemHelpTest_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Name = "menuItem11";
			this.menuItem11.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemHelpAbout
			// 
			this.menuItemHelpAbout.Name = "menuItemHelpAbout";
			this.menuItemHelpAbout.Size = new System.Drawing.Size(227, 22);
			this.menuItemHelpAbout.Text = "バージョン情報(&A)...";
			this.menuItemHelpAbout.Click += new System.EventHandler(this.menuItemHelpAbout_Click);
			// 
			// toolBarMain
			// 
			this.toolBarMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarMain.AutoSize = false;
			this.toolBarMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonOnline,
            this.toolBarButtonCaching,
            this.toolBarButton2,
            this.toolBarButtonSearchCache,
            this.toolBarButtonSearchBoard,
            this.toolBarButton3,
            this.toolBarButtonViewTable,
            this.toolBarButtonViewList,
            this.toolBarButtonViewThread,
            this.toolBarButton4,
            this.toolBarButtonNGWords,
            this.toolBarButtonLive,
            this.toolBarButton10,
            this.toolBarButtonPatrol,
            this.toolBarButton9,
            this.toolBarButtonSettings});
			this.toolBarMain.ButtonSize = new System.Drawing.Size(16, 16);
			this.toolBarMain.Divider = false;
			this.toolBarMain.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarMain.DropDownArrows = true;
			this.toolBarMain.ImageList = this.imageList;
			this.toolBarMain.Location = new System.Drawing.Point(11, 2);
			this.toolBarMain.Margin = new System.Windows.Forms.Padding(2);
			this.toolBarMain.Name = "toolBarMain";
			this.toolBarMain.ShowToolTips = true;
			this.toolBarMain.Size = new System.Drawing.Size(299, 22);
			this.toolBarMain.TabIndex = 0;
			this.toolBarMain.Wrappable = false;
			this.toolBarMain.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarMain_ButtonClick);
			this.toolBarMain.ButtonDropDown += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarMain_ButtonDropDown);
			// 
			// toolBarButtonOnline
			// 
			this.toolBarButtonOnline.ImageIndex = 5;
			this.toolBarButtonOnline.Name = "toolBarButtonOnline";
			this.toolBarButtonOnline.ToolTipText = "オンライン・オフライン切り替え";
			// 
			// toolBarButtonCaching
			// 
			this.toolBarButtonCaching.ImageIndex = 35;
			this.toolBarButtonCaching.Name = "toolBarButtonCaching";
			this.toolBarButtonCaching.ToolTipText = "ログ保存モードの切り替え";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Name = "toolBarButton2";
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonSearchCache
			// 
			this.toolBarButtonSearchCache.ImageIndex = 25;
			this.toolBarButtonSearchCache.Name = "toolBarButtonSearchCache";
			this.toolBarButtonSearchCache.ToolTipText = "キャッシュ内を検索";
			// 
			// toolBarButtonSearchBoard
			// 
			this.toolBarButtonSearchBoard.ImageIndex = 26;
			this.toolBarButtonSearchBoard.Name = "toolBarButtonSearchBoard";
			this.toolBarButtonSearchBoard.ToolTipText = "板一覧を検索";
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.Name = "toolBarButton3";
			this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonViewTable
			// 
			this.toolBarButtonViewTable.ImageIndex = 38;
			this.toolBarButtonViewTable.Name = "toolBarButtonViewTable";
			this.toolBarButtonViewTable.ToolTipText = "板一覧を隠します";
			// 
			// toolBarButtonViewList
			// 
			this.toolBarButtonViewList.ImageIndex = 37;
			this.toolBarButtonViewList.Name = "toolBarButtonViewList";
			this.toolBarButtonViewList.ToolTipText = "スレッド一覧を拡大表示します";
			// 
			// toolBarButtonViewThread
			// 
			this.toolBarButtonViewThread.ImageIndex = 36;
			this.toolBarButtonViewThread.Name = "toolBarButtonViewThread";
			this.toolBarButtonViewThread.ToolTipText = "スレッドを拡大表示します";
			// 
			// toolBarButton4
			// 
			this.toolBarButton4.Name = "toolBarButton4";
			this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonNGWords
			// 
			this.toolBarButtonNGWords.ImageIndex = 46;
			this.toolBarButtonNGWords.Name = "toolBarButtonNGWords";
			this.toolBarButtonNGWords.ToolTipText = "NGワードのOn/Off";
			// 
			// toolBarButtonLive
			// 
			this.toolBarButtonLive.ImageIndex = 44;
			this.toolBarButtonLive.Name = "toolBarButtonLive";
			this.toolBarButtonLive.ToolTipText = "実況モード";
			// 
			// toolBarButton10
			// 
			this.toolBarButton10.Name = "toolBarButton10";
			this.toolBarButton10.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonPatrol
			// 
			this.toolBarButtonPatrol.ImageIndex = 30;
			this.toolBarButtonPatrol.Name = "toolBarButtonPatrol";
			this.toolBarButtonPatrol.ToolTipText = "お気に入りの更新チェック";
			// 
			// toolBarButton9
			// 
			this.toolBarButton9.Name = "toolBarButton9";
			this.toolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonSettings
			// 
			this.toolBarButtonSettings.ImageIndex = 7;
			this.toolBarButtonSettings.Name = "toolBarButtonSettings";
			this.toolBarButtonSettings.ToolTipText = "環境設定";
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			this.imageList.Images.SetKeyName(3, "");
			this.imageList.Images.SetKeyName(4, "");
			this.imageList.Images.SetKeyName(5, "");
			this.imageList.Images.SetKeyName(6, "");
			this.imageList.Images.SetKeyName(7, "");
			this.imageList.Images.SetKeyName(8, "");
			this.imageList.Images.SetKeyName(9, "");
			this.imageList.Images.SetKeyName(10, "");
			this.imageList.Images.SetKeyName(11, "");
			this.imageList.Images.SetKeyName(12, "");
			this.imageList.Images.SetKeyName(13, "");
			this.imageList.Images.SetKeyName(14, "");
			this.imageList.Images.SetKeyName(15, "");
			this.imageList.Images.SetKeyName(16, "");
			this.imageList.Images.SetKeyName(17, "");
			this.imageList.Images.SetKeyName(18, "");
			this.imageList.Images.SetKeyName(19, "");
			this.imageList.Images.SetKeyName(20, "");
			this.imageList.Images.SetKeyName(21, "");
			this.imageList.Images.SetKeyName(22, "");
			this.imageList.Images.SetKeyName(23, "");
			this.imageList.Images.SetKeyName(24, "");
			this.imageList.Images.SetKeyName(25, "");
			this.imageList.Images.SetKeyName(26, "");
			this.imageList.Images.SetKeyName(27, "");
			this.imageList.Images.SetKeyName(28, "");
			this.imageList.Images.SetKeyName(29, "");
			this.imageList.Images.SetKeyName(30, "");
			this.imageList.Images.SetKeyName(31, "");
			this.imageList.Images.SetKeyName(32, "");
			this.imageList.Images.SetKeyName(33, "");
			this.imageList.Images.SetKeyName(34, "");
			this.imageList.Images.SetKeyName(35, "");
			this.imageList.Images.SetKeyName(36, "");
			this.imageList.Images.SetKeyName(37, "");
			this.imageList.Images.SetKeyName(38, "");
			this.imageList.Images.SetKeyName(39, "");
			this.imageList.Images.SetKeyName(40, "");
			this.imageList.Images.SetKeyName(41, "");
			this.imageList.Images.SetKeyName(42, "");
			this.imageList.Images.SetKeyName(43, "");
			this.imageList.Images.SetKeyName(44, "");
			this.imageList.Images.SetKeyName(45, "");
			this.imageList.Images.SetKeyName(46, "");
			this.imageList.Images.SetKeyName(47, "");
			this.imageList.Images.SetKeyName(48, "");
			this.imageList.Images.SetKeyName(49, "");
			this.imageList.Images.SetKeyName(50, "");
			// 
			// addressPanel
			// 
			this.addressPanel.BackColor = System.Drawing.Color.Transparent;
			this.addressPanel.Controls.Add(this.toolBarGo);
			this.addressPanel.Controls.Add(this.textBoxAddress);
			this.addressPanel.Location = new System.Drawing.Point(325, 30);
			this.addressPanel.Margin = new System.Windows.Forms.Padding(0);
			this.addressPanel.Name = "addressPanel";
			this.addressPanel.Size = new System.Drawing.Size(571, 22);
			this.addressPanel.TabIndex = 1;
			// 
			// toolBarGo
			// 
			this.toolBarGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.toolBarGo.AutoSize = false;
			this.toolBarGo.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonGo});
			this.toolBarGo.ButtonSize = new System.Drawing.Size(51, 20);
			this.toolBarGo.Divider = false;
			this.toolBarGo.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarGo.DropDownArrows = true;
			this.toolBarGo.ImageList = this.imageList;
			this.toolBarGo.Location = new System.Drawing.Point(517, -1);
			this.toolBarGo.Margin = new System.Windows.Forms.Padding(2);
			this.toolBarGo.Name = "toolBarGo";
			this.toolBarGo.ShowToolTips = true;
			this.toolBarGo.Size = new System.Drawing.Size(56, 22);
			this.toolBarGo.TabIndex = 1;
			this.toolBarGo.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBarGo.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarGo_ButtonClick);
			// 
			// toolBarButtonGo
			// 
			this.toolBarButtonGo.ImageIndex = 6;
			this.toolBarButtonGo.Name = "toolBarButtonGo";
			this.toolBarButtonGo.Text = "移動";
			this.toolBarButtonGo.ToolTipText = "指定したURLを開く";
			// 
			// textBoxAddress
			// 
			this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxAddress.Location = new System.Drawing.Point(0, 1);
			this.textBoxAddress.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxAddress.Name = "textBoxAddress";
			this.textBoxAddress.Size = new System.Drawing.Size(514, 19);
			this.textBoxAddress.TabIndex = 0;
			this.textBoxAddress.Text = "http://";
			this.textBoxAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxAddress_KeyPress);
			// 
			// rebarWrapper
			// 
			this.rebarWrapper.Bands.Add(this.bandWrapperMain);
			this.rebarWrapper.Bands.Add(this.bandWrapperList);
			this.rebarWrapper.Bands.Add(this.bandWrapperTools);
			this.rebarWrapper.Bands.Add(this.bandWrapperIButton);
			this.rebarWrapper.Bands.Add(this.bandWrapperAddress);
			this.rebarWrapper.Controls.Add(this.toolBarList);
			this.rebarWrapper.Controls.Add(this.toolBarMain);
			this.rebarWrapper.Controls.Add(this.addressPanel);
			this.rebarWrapper.Controls.Add(this.cSharpToolBar);
			this.rebarWrapper.Controls.Add(this.panel1);
			this.rebarWrapper.Dock = System.Windows.Forms.DockStyle.Top;
			this.rebarWrapper.Location = new System.Drawing.Point(0, 0);
			this.rebarWrapper.Margin = new System.Windows.Forms.Padding(2);
			this.rebarWrapper.Name = "rebarWrapper";
			this.rebarWrapper.Size = new System.Drawing.Size(896, 54);
			this.rebarWrapper.TabIndex = 0;
			this.rebarWrapper.Text = "rebarWrapper1";
			// 
			// bandWrapperMain
			// 
			this.bandWrapperMain.Child = this.toolBarMain;
			this.bandWrapperMain.GripSettings = RebarDotNet.GripperSettings.Always;
			this.bandWrapperMain.Header = -1;
			this.bandWrapperMain.IdealWidth = -1;
			this.bandWrapperMain.Integral = 1;
			this.bandWrapperMain.Key = "ToolBar";
			this.bandWrapperMain.MaxHeight = 0;
			this.bandWrapperMain.MinHeight = 22;
			this.bandWrapperMain.Width = 312;
			// 
			// bandWrapperList
			// 
			this.bandWrapperList.Child = this.toolBarList;
			this.bandWrapperList.GripSettings = RebarDotNet.GripperSettings.Always;
			this.bandWrapperList.Header = -1;
			this.bandWrapperList.IdealWidth = -1;
			this.bandWrapperList.Integral = 1;
			this.bandWrapperList.Key = "ListToolBar";
			this.bandWrapperList.MaxHeight = 0;
			this.bandWrapperList.MinHeight = 22;
			this.bandWrapperList.NewRow = false;
			this.bandWrapperList.Width = 189;
			// 
			// toolBarList
			// 
			this.toolBarList.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarList.AutoSize = false;
			this.toolBarList.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonListReload,
            this.toolBarButtonListStop,
            this.toolBarButton5,
            this.toolBarButtonListNewThread,
            this.toolBarButton8,
            this.toolBarButtonListSearch,
            this.toolBarButtonListOpenUp,
            this.toolBarButton1,
            this.toolBarButtonListClose});
			this.toolBarList.ButtonSize = new System.Drawing.Size(16, 16);
			this.toolBarList.Divider = false;
			this.toolBarList.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarList.DropDownArrows = true;
			this.toolBarList.ImageList = this.imageList;
			this.toolBarList.Location = new System.Drawing.Point(327, 2);
			this.toolBarList.Margin = new System.Windows.Forms.Padding(2);
			this.toolBarList.Name = "toolBarList";
			this.toolBarList.ShowToolTips = true;
			this.toolBarList.Size = new System.Drawing.Size(176, 22);
			this.toolBarList.TabIndex = 7;
			this.toolBarList.Wrappable = false;
			this.toolBarList.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarList_ButtonClick);
			// 
			// toolBarButtonListReload
			// 
			this.toolBarButtonListReload.ImageIndex = 8;
			this.toolBarButtonListReload.Name = "toolBarButtonListReload";
			this.toolBarButtonListReload.ToolTipText = "スレッド一覧を最新の状態に更新";
			// 
			// toolBarButtonListStop
			// 
			this.toolBarButtonListStop.ImageIndex = 9;
			this.toolBarButtonListStop.Name = "toolBarButtonListStop";
			this.toolBarButtonListStop.ToolTipText = "スレッド一覧の読み込みを中止";
			// 
			// toolBarButton5
			// 
			this.toolBarButton5.Name = "toolBarButton5";
			this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonListNewThread
			// 
			this.toolBarButtonListNewThread.ImageIndex = 24;
			this.toolBarButtonListNewThread.Name = "toolBarButtonListNewThread";
			this.toolBarButtonListNewThread.ToolTipText = "新規スレッドを立てる";
			// 
			// toolBarButton8
			// 
			this.toolBarButton8.Name = "toolBarButton8";
			this.toolBarButton8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonListSearch
			// 
			this.toolBarButtonListSearch.ImageIndex = 14;
			this.toolBarButtonListSearch.Name = "toolBarButtonListSearch";
			this.toolBarButtonListSearch.ToolTipText = "スレッド一覧を検索";
			// 
			// toolBarButtonListOpenUp
			// 
			this.toolBarButtonListOpenUp.ImageIndex = 49;
			this.toolBarButtonListOpenUp.Name = "toolBarButtonListOpenUp";
			this.toolBarButtonListOpenUp.ToolTipText = "更新スレッドを開く";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Name = "toolBarButton1";
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonListClose
			// 
			this.toolBarButtonListClose.ImageIndex = 11;
			this.toolBarButtonListClose.Name = "toolBarButtonListClose";
			this.toolBarButtonListClose.ToolTipText = "スレッド一覧を閉じる";
			// 
			// bandWrapperTools
			// 
			this.bandWrapperTools.Caption = "ツール";
			this.bandWrapperTools.Child = this.panel1;
			this.bandWrapperTools.GripSettings = RebarDotNet.GripperSettings.Always;
			this.bandWrapperTools.Header = -1;
			this.bandWrapperTools.IdealWidth = -1;
			this.bandWrapperTools.Integral = 1;
			this.bandWrapperTools.Key = "Tools";
			this.bandWrapperTools.MaxHeight = 0;
			this.bandWrapperTools.MinHeight = 22;
			this.bandWrapperTools.NewRow = false;
			this.bandWrapperTools.UseChevron = false;
			this.bandWrapperTools.Width = 385;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.comboBoxTools);
			this.panel1.Controls.Add(this.toolBarTools);
			this.panel1.Location = new System.Drawing.Point(560, 2);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(336, 22);
			this.panel1.TabIndex = 4;
			// 
			// comboBoxTools
			// 
			this.comboBoxTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxTools.Location = new System.Drawing.Point(0, 0);
			this.comboBoxTools.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxTools.Name = "comboBoxTools";
			this.comboBoxTools.Size = new System.Drawing.Size(308, 20);
			this.comboBoxTools.TabIndex = 1;
			this.comboBoxTools.SelectedIndexChanged += new System.EventHandler(this.comboBoxTools_SelectedIndexChanged);
			this.comboBoxTools.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxTools_KeyPress);
			// 
			// toolBarTools
			// 
			this.toolBarTools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.toolBarTools.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarTools.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonRunTool});
			this.toolBarTools.Divider = false;
			this.toolBarTools.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarTools.DropDownArrows = true;
			this.toolBarTools.ImageList = this.imageList;
			this.toolBarTools.Location = new System.Drawing.Point(311, -1);
			this.toolBarTools.Margin = new System.Windows.Forms.Padding(0);
			this.toolBarTools.Name = "toolBarTools";
			this.toolBarTools.ShowToolTips = true;
			this.toolBarTools.Size = new System.Drawing.Size(24, 26);
			this.toolBarTools.TabIndex = 0;
			this.toolBarTools.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarRun_ButtonClick);
			// 
			// toolBarButtonRunTool
			// 
			this.toolBarButtonRunTool.ImageIndex = 43;
			this.toolBarButtonRunTool.Name = "toolBarButtonRunTool";
			this.toolBarButtonRunTool.ToolTipText = "ツールを実行します";
			// 
			// bandWrapperIButton
			// 
			this.bandWrapperIButton.BackColor = System.Drawing.Color.Transparent;
			this.bandWrapperIButton.Child = this.cSharpToolBar;
			this.bandWrapperIButton.GripSettings = RebarDotNet.GripperSettings.Always;
			this.bandWrapperIButton.Header = -1;
			this.bandWrapperIButton.IdealWidth = -1;
			this.bandWrapperIButton.Integral = 1;
			this.bandWrapperIButton.Key = "IButton";
			this.bandWrapperIButton.MaxHeight = 0;
			this.bandWrapperIButton.MinHeight = 20;
			this.bandWrapperIButton.UseChevron = false;
			this.bandWrapperIButton.Width = 258;
			// 
			// cSharpToolBar
			// 
			this.cSharpToolBar.AllowDragButton = true;
			this.cSharpToolBar.Appearance = CSharpSamples.CSharpToolBarAppearance.VisualStudio;
			this.cSharpToolBar.ContextMenuStrip = this.contextMenuItaBotan;
			this.cSharpToolBar.ImageList = null;
			this.cSharpToolBar.Location = new System.Drawing.Point(11, 31);
			this.cSharpToolBar.Margin = new System.Windows.Forms.Padding(2);
			this.cSharpToolBar.Name = "cSharpToolBar";
			this.cSharpToolBar.Size = new System.Drawing.Size(245, 20);
			this.cSharpToolBar.TabIndex = 7;
			this.cSharpToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.cSharpToolBar.Wrappable = false;
			this.cSharpToolBar.ButtonClick += new CSharpSamples.CSharpToolBarButtonEventHandler(this.ItaToolBar_ButtonClick);
			// 
			// contextMenuItaBotan
			// 
			this.contextMenuItaBotan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemItaBotanRemove});
			this.contextMenuItaBotan.Name = "contextMenuItaBotan";
			this.contextMenuItaBotan.Size = new System.Drawing.Size(120, 26);
			this.contextMenuItaBotan.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuItaBotan_Popup);
			// 
			// menuItemItaBotanRemove
			// 
			this.menuItemItaBotanRemove.Name = "menuItemItaBotanRemove";
			this.menuItemItaBotanRemove.Size = new System.Drawing.Size(119, 22);
			this.menuItemItaBotanRemove.Text = "削除(&D)";
			this.menuItemItaBotanRemove.Click += new System.EventHandler(this.menuItemItaBotanRemove_Click);
			// 
			// bandWrapperAddress
			// 
			this.bandWrapperAddress.Caption = "アドレス";
			this.bandWrapperAddress.Child = this.addressPanel;
			this.bandWrapperAddress.GripSettings = RebarDotNet.GripperSettings.Always;
			this.bandWrapperAddress.Header = -1;
			this.bandWrapperAddress.IdealWidth = -1;
			this.bandWrapperAddress.Integral = 1;
			this.bandWrapperAddress.Key = "Address";
			this.bandWrapperAddress.MaxHeight = 0;
			this.bandWrapperAddress.MinHeight = 22;
			this.bandWrapperAddress.NewRow = false;
			this.bandWrapperAddress.UseChevron = false;
			this.bandWrapperAddress.Width = 632;
			// 
			// treePanel
			// 
			this.treePanel.Controls.Add(this.tabControlTable);
			this.treePanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.treePanel.Location = new System.Drawing.Point(0, 54);
			this.treePanel.Margin = new System.Windows.Forms.Padding(2);
			this.treePanel.Name = "treePanel";
			this.treePanel.Size = new System.Drawing.Size(160, 456);
			this.treePanel.TabIndex = 1;
			// 
			// tabControlTable
			// 
			this.tabControlTable.Controls.Add(this.tabPageBoards);
			this.tabControlTable.Controls.Add(this.tabPageBookmarks);
			this.tabControlTable.Controls.Add(this.tabPageWareHouse);
			this.tabControlTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlTable.ItemSize = new System.Drawing.Size(60, 24);
			this.tabControlTable.Location = new System.Drawing.Point(0, 0);
			this.tabControlTable.Margin = new System.Windows.Forms.Padding(2);
			this.tabControlTable.Name = "tabControlTable";
			this.tabControlTable.SelectedIndex = 0;
			this.tabControlTable.Size = new System.Drawing.Size(160, 456);
			this.tabControlTable.TabIndex = 0;
			// 
			// tabPageBoards
			// 
			this.tabPageBoards.Location = new System.Drawing.Point(4, 28);
			this.tabPageBoards.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageBoards.Name = "tabPageBoards";
			this.tabPageBoards.Size = new System.Drawing.Size(152, 424);
			this.tabPageBoards.TabIndex = 0;
			this.tabPageBoards.Text = "板一覧";
			// 
			// tabPageBookmarks
			// 
			this.tabPageBookmarks.Location = new System.Drawing.Point(4, 28);
			this.tabPageBookmarks.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageBookmarks.Name = "tabPageBookmarks";
			this.tabPageBookmarks.Size = new System.Drawing.Size(152, 424);
			this.tabPageBookmarks.TabIndex = 1;
			this.tabPageBookmarks.Text = "お気に入り";
			// 
			// tabPageWareHouse
			// 
			this.tabPageWareHouse.Location = new System.Drawing.Point(4, 28);
			this.tabPageWareHouse.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageWareHouse.Name = "tabPageWareHouse";
			this.tabPageWareHouse.Size = new System.Drawing.Size(152, 424);
			this.tabPageWareHouse.TabIndex = 2;
			this.tabPageWareHouse.Text = "過去ログ";
			// 
			// listPanel
			// 
			this.listPanel.Controls.Add(this.listTabCtrl);
			this.listPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.listPanel.Location = new System.Drawing.Point(163, 54);
			this.listPanel.Margin = new System.Windows.Forms.Padding(2);
			this.listPanel.Name = "listPanel";
			this.listPanel.Size = new System.Drawing.Size(733, 138);
			this.listPanel.TabIndex = 2;
			// 
			// listTabCtrl
			// 
			this.listTabCtrl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.listTabCtrl.ContextMenuStrip = this.contextMenuListTab;
			this.listTabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listTabCtrl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.listTabCtrl.ImageList = this.imageListSmallIcons;
			this.listTabCtrl.ItemSize = new System.Drawing.Size(85, 20);
			this.listTabCtrl.Location = new System.Drawing.Point(0, 0);
			this.listTabCtrl.Margin = new System.Windows.Forms.Padding(2);
			this.listTabCtrl.Multiline = true;
			this.listTabCtrl.Name = "listTabCtrl";
			this.listTabCtrl.SelectedIndex = 0;
			this.listTabCtrl.Size = new System.Drawing.Size(733, 138);
			this.listTabCtrl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.listTabCtrl.TabIndex = 0;
			this.listTabCtrl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabControl_DrawItem);
			this.listTabCtrl.SelectedIndexChanged += new System.EventHandler(this.listTabCtrl_SelectedIndexChanged);
			// 
			// contextMenuListTab
			// 
			this.contextMenuListTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemListTabClose,
            this.menuItemListTabClose2,
            this.menuItemListTabCloseAll,
            this.menuItem53,
            this.menuItemListTabOpenUpThreads,
            this.menuItemListTabOpenWebBrowsre,
            this.menuItem50,
            this.menuItemListTabCopyURL,
            this.menuItemListTabCopyNameURL,
            this.toolStripMenuItem11,
            this.menuItemListTabCacheOpen,
            this.menuItemListTabCacheClear,
            this.toolStripMenuItem8,
            this.menuItemListTabRefresh,
            this.menuItem71,
            this.menuItemListTabUpdateCheck,
            this.toolStripSeparator1,
            this.menuItemListTabWithout1000Res,
            this.menuItemListTabWithoutPastlog,
            this.menuItemListTabWithoutKakolog,
            this.toolStripSeparator4,
            this.menuItemListTabColoring});
			this.contextMenuListTab.Name = "contextMenuListTab";
			this.contextMenuListTab.Size = new System.Drawing.Size(240, 376);
			this.contextMenuListTab.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuListTab_Popup);
			// 
			// menuItemListTabClose
			// 
			this.menuItemListTabClose.Name = "menuItemListTabClose";
			this.menuItemListTabClose.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabClose.Text = "閉じる(&C)";
			this.menuItemListTabClose.Click += new System.EventHandler(this.menuItemListTabClose_Click);
			// 
			// menuItemListTabClose2
			// 
			this.menuItemListTabClose2.Name = "menuItemListTabClose2";
			this.menuItemListTabClose2.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabClose2.Text = "これ以外のタブを閉じる(&O)";
			this.menuItemListTabClose2.Click += new System.EventHandler(this.menuItemListTabClose2_Click);
			// 
			// menuItemListTabCloseAll
			// 
			this.menuItemListTabCloseAll.Name = "menuItemListTabCloseAll";
			this.menuItemListTabCloseAll.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabCloseAll.Text = "すべて閉じる(&A)";
			this.menuItemListTabCloseAll.Click += new System.EventHandler(this.menuItemListTabCloseAll_Click);
			// 
			// menuItem53
			// 
			this.menuItem53.Name = "menuItem53";
			this.menuItem53.Size = new System.Drawing.Size(236, 6);
			// 
			// menuItemListTabOpenUpThreads
			// 
			this.menuItemListTabOpenUpThreads.Name = "menuItemListTabOpenUpThreads";
			this.menuItemListTabOpenUpThreads.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabOpenUpThreads.Text = "更新スレッドをすべて開く(&U)";
			this.menuItemListTabOpenUpThreads.Click += new System.EventHandler(this.menuItemListTabOpenUpThreads_Click);
			// 
			// menuItemListTabOpenWebBrowsre
			// 
			this.menuItemListTabOpenWebBrowsre.Name = "menuItemListTabOpenWebBrowsre";
			this.menuItemListTabOpenWebBrowsre.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabOpenWebBrowsre.Text = "Webブラウザで開く(&W)";
			this.menuItemListTabOpenWebBrowsre.Click += new System.EventHandler(this.menuItemListTabOpenWebBrowsre_Click);
			// 
			// menuItem50
			// 
			this.menuItem50.Name = "menuItem50";
			this.menuItem50.Size = new System.Drawing.Size(236, 6);
			// 
			// menuItemListTabCopyURL
			// 
			this.menuItemListTabCopyURL.Name = "menuItemListTabCopyURL";
			this.menuItemListTabCopyURL.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabCopyURL.Text = "URLをコピー(&U)";
			this.menuItemListTabCopyURL.Click += new System.EventHandler(this.menuItemListTabCopyURL_Click);
			// 
			// menuItemListTabCopyNameURL
			// 
			this.menuItemListTabCopyNameURL.Name = "menuItemListTabCopyNameURL";
			this.menuItemListTabCopyNameURL.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabCopyNameURL.Text = "板名とURLをコピー(&Y)";
			this.menuItemListTabCopyNameURL.Click += new System.EventHandler(this.menuItemListTabCopyNameURL_Click);
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(236, 6);
			// 
			// menuItemListTabCacheOpen
			// 
			this.menuItemListTabCacheOpen.Name = "menuItemListTabCacheOpen";
			this.menuItemListTabCacheOpen.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabCacheOpen.Text = "この板の既得ログを表示(&G)";
			this.menuItemListTabCacheOpen.Click += new System.EventHandler(this.menuItemListTabCacheOpen_Click);
			// 
			// menuItemListTabCacheClear
			// 
			this.menuItemListTabCacheClear.Name = "menuItemListTabCacheClear";
			this.menuItemListTabCacheClear.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabCacheClear.Text = "この板のログを削除(&D)";
			this.menuItemListTabCacheClear.Click += new System.EventHandler(this.menuItemListTabCacheClear_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(236, 6);
			// 
			// menuItemListTabRefresh
			// 
			this.menuItemListTabRefresh.Name = "menuItemListTabRefresh";
			this.menuItemListTabRefresh.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabRefresh.Text = "スレッド一覧を更新(&R)";
			this.menuItemListTabRefresh.Click += new System.EventHandler(this.menuItemListTabRefresh_Click);
			// 
			// menuItem71
			// 
			this.menuItem71.Name = "menuItem71";
			this.menuItem71.Size = new System.Drawing.Size(236, 6);
			// 
			// menuItemListTabUpdateCheck
			// 
			this.menuItemListTabUpdateCheck.Name = "menuItemListTabUpdateCheck";
			this.menuItemListTabUpdateCheck.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabUpdateCheck.Text = "更新チェック(&P)";
			this.menuItemListTabUpdateCheck.Click += new System.EventHandler(this.menuItemListTabUpdateCheck_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(236, 6);
			// 
			// menuItemListTabWithout1000Res
			// 
			this.menuItemListTabWithout1000Res.Name = "menuItemListTabWithout1000Res";
			this.menuItemListTabWithout1000Res.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabWithout1000Res.Text = "1000レスのスレッドを除外";
			this.menuItemListTabWithout1000Res.Click += new System.EventHandler(this.menuItemListTabWithout1000Res_Click);
			// 
			// menuItemListTabWithoutPastlog
			// 
			this.menuItemListTabWithoutPastlog.Name = "menuItemListTabWithoutPastlog";
			this.menuItemListTabWithoutPastlog.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabWithoutPastlog.Text = "dat落ちスレを除外";
			this.menuItemListTabWithoutPastlog.Click += new System.EventHandler(this.menuItemListTabWithoutPastlog_Click);
			// 
			// menuItemListTabWithoutKakolog
			// 
			this.menuItemListTabWithoutKakolog.Name = "menuItemListTabWithoutKakolog";
			this.menuItemListTabWithoutKakolog.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabWithoutKakolog.Text = "過去ログスレッドを除外";
			this.menuItemListTabWithoutKakolog.Click += new System.EventHandler(this.menuItemListTabWithoutKakolog_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(236, 6);
			// 
			// menuItemListTabColoring
			// 
			this.menuItemListTabColoring.Name = "menuItemListTabColoring";
			this.menuItemListTabColoring.Size = new System.Drawing.Size(239, 22);
			this.menuItemListTabColoring.Text = "この板タブを色づけ...";
			this.menuItemListTabColoring.ToolTipText = "この板を開いたときのタブの背景色を変更します";
			this.menuItemListTabColoring.Click += new System.EventHandler(this.menuItemListTabColoring_Click);
			// 
			// imageListSmallIcons
			// 
			this.imageListSmallIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmallIcons.ImageStream")));
			this.imageListSmallIcons.TransparentColor = System.Drawing.Color.Magenta;
			this.imageListSmallIcons.Images.SetKeyName(0, "");
			this.imageListSmallIcons.Images.SetKeyName(1, "");
			this.imageListSmallIcons.Images.SetKeyName(2, "");
			this.imageListSmallIcons.Images.SetKeyName(3, "");
			this.imageListSmallIcons.Images.SetKeyName(4, "whiteblock.bmp");
			this.imageListSmallIcons.Images.SetKeyName(5, "");
			this.imageListSmallIcons.Images.SetKeyName(6, "");
			this.imageListSmallIcons.Images.SetKeyName(7, "denki.bmp");
			// 
			// threadPanel
			// 
			this.threadPanel.Controls.Add(this.threadInnerPanel);
			this.threadPanel.Controls.Add(this.threadToolPanel);
			this.threadPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.threadPanel.Location = new System.Drawing.Point(163, 195);
			this.threadPanel.Margin = new System.Windows.Forms.Padding(2);
			this.threadPanel.Name = "threadPanel";
			this.threadPanel.Size = new System.Drawing.Size(733, 315);
			this.threadPanel.TabIndex = 3;
			// 
			// threadInnerPanel
			// 
			this.threadInnerPanel.Controls.Add(this.threadTabCtrl);
			this.threadInnerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.threadInnerPanel.Location = new System.Drawing.Point(0, 26);
			this.threadInnerPanel.Margin = new System.Windows.Forms.Padding(2);
			this.threadInnerPanel.Name = "threadInnerPanel";
			this.threadInnerPanel.Size = new System.Drawing.Size(733, 289);
			this.threadInnerPanel.TabIndex = 4;
			// 
			// threadTabCtrl
			// 
			this.threadTabCtrl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.threadTabCtrl.ContextMenuStrip = this.contextMenuThreadTab;
			this.threadTabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.threadTabCtrl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.threadTabCtrl.ImageList = this.imageListSmallIcons;
			this.threadTabCtrl.ItemSize = new System.Drawing.Size(85, 20);
			this.threadTabCtrl.Location = new System.Drawing.Point(0, 0);
			this.threadTabCtrl.Margin = new System.Windows.Forms.Padding(2);
			this.threadTabCtrl.Multiline = true;
			this.threadTabCtrl.Name = "threadTabCtrl";
			this.threadTabCtrl.SelectedIndex = 0;
			this.threadTabCtrl.Size = new System.Drawing.Size(733, 289);
			this.threadTabCtrl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.threadTabCtrl.TabIndex = 0;
			this.threadTabCtrl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabControl_DrawItem);
			this.threadTabCtrl.SelectedIndexChanged += new System.EventHandler(this.threadTabCtrl_SelectedIndexChanged);
			// 
			// contextMenuThreadTab
			// 
			this.contextMenuThreadTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemThreadTabClose,
            this.menuItemThreadTabClose2,
            this.menuItemThreadTabCloseAll,
            this.menuItem67,
            this.menuItemThreadTabDelClose,
            this.menuItem43,
            this.menuItemThreadTabCopyURL,
            this.menuItemThreadTabCopyURLAndName,
            this.menuItemThreadTabCopyName,
            this.menuItem47,
            this.menuItemThreadTabRefresh,
            this.toolStripMenuItem6,
            this.menuItemThreadTabOpenWebBrowser,
            this.menuItemThreadTabOpenDraft,
            this.menuItem65,
            this.menuItemThreadTabNewThread,
            this.menuItemThreadTabNextThreadCheck,
            this.toolStripMenuItem7,
            this.menuItemThreadTabAllOpenImageViewer,
            this.toolStripMenuItemSaveImages,
            this.menuItemThreadTabReget,
            this.toolStripSeparator3,
            this.menuItemThreadTabColoring});
			this.contextMenuThreadTab.Name = "contextMenuThreadTab";
			this.contextMenuThreadTab.Size = new System.Drawing.Size(326, 398);
			this.contextMenuThreadTab.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuThreadTab_Popup);
			// 
			// menuItemThreadTabClose
			// 
			this.menuItemThreadTabClose.Name = "menuItemThreadTabClose";
			this.menuItemThreadTabClose.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabClose.Text = "このタブを閉じる(&C)";
			this.menuItemThreadTabClose.Click += new System.EventHandler(this.menuItemThreadTabClose_Click);
			// 
			// menuItemThreadTabClose2
			// 
			this.menuItemThreadTabClose2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemThreadTabCloseLeft,
            this.menuItemThreadTabCloseRight,
            this.toolStripMenuItem1,
            this.menuItemThreadTabCloseWithoutThis});
			this.menuItemThreadTabClose2.Name = "menuItemThreadTabClose2";
			this.menuItemThreadTabClose2.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabClose2.Text = "これ以外のタブを閉じる(&O)";
			// 
			// menuItemThreadTabCloseLeft
			// 
			this.menuItemThreadTabCloseLeft.Name = "menuItemThreadTabCloseLeft";
			this.menuItemThreadTabCloseLeft.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadTabCloseLeft.Text = "これより左のタブを閉じる(&L)";
			this.menuItemThreadTabCloseLeft.Click += new System.EventHandler(this.menuItemThreadTabCloseLeft_Click);
			// 
			// menuItemThreadTabCloseRight
			// 
			this.menuItemThreadTabCloseRight.Name = "menuItemThreadTabCloseRight";
			this.menuItemThreadTabCloseRight.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadTabCloseRight.Text = "これより右のタブを閉じる(&R)";
			this.menuItemThreadTabCloseRight.Click += new System.EventHandler(this.menuItemThreadTabCloseRight_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemThreadTabCloseWithoutThis
			// 
			this.menuItemThreadTabCloseWithoutThis.Name = "menuItemThreadTabCloseWithoutThis";
			this.menuItemThreadTabCloseWithoutThis.Size = new System.Drawing.Size(238, 22);
			this.menuItemThreadTabCloseWithoutThis.Text = "これ以外のタブを閉じる(&A)";
			this.menuItemThreadTabCloseWithoutThis.Click += new System.EventHandler(this.menuItemThreadTabClose2_Click);
			// 
			// menuItemThreadTabCloseAll
			// 
			this.menuItemThreadTabCloseAll.Name = "menuItemThreadTabCloseAll";
			this.menuItemThreadTabCloseAll.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabCloseAll.Text = "すべて閉じる(&A)";
			this.menuItemThreadTabCloseAll.Click += new System.EventHandler(this.menuItemThreadTabCloseAll_Click);
			// 
			// menuItem67
			// 
			this.menuItem67.Name = "menuItem67";
			this.menuItem67.Size = new System.Drawing.Size(322, 6);
			// 
			// menuItemThreadTabDelClose
			// 
			this.menuItemThreadTabDelClose.Name = "menuItemThreadTabDelClose";
			this.menuItemThreadTabDelClose.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabDelClose.Text = "ログを削除して閉じる(&D)";
			this.menuItemThreadTabDelClose.Click += new System.EventHandler(this.menuItemThreadTabDelClose_Click);
			// 
			// menuItem43
			// 
			this.menuItem43.Name = "menuItem43";
			this.menuItem43.Size = new System.Drawing.Size(322, 6);
			// 
			// menuItemThreadTabCopyURL
			// 
			this.menuItemThreadTabCopyURL.Name = "menuItemThreadTabCopyURL";
			this.menuItemThreadTabCopyURL.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabCopyURL.Text = "URLをコピー(&U)";
			this.menuItemThreadTabCopyURL.Click += new System.EventHandler(this.menuItemThreadTabCopyURL_Click);
			// 
			// menuItemThreadTabCopyURLAndName
			// 
			this.menuItemThreadTabCopyURLAndName.Name = "menuItemThreadTabCopyURLAndName";
			this.menuItemThreadTabCopyURLAndName.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabCopyURLAndName.Text = "URLとスレッド名をコピー(&N)";
			this.menuItemThreadTabCopyURLAndName.Click += new System.EventHandler(this.menuItemThreadTabCopyURLAndName_Click);
			// 
			// menuItemThreadTabCopyName
			// 
			this.menuItemThreadTabCopyName.Name = "menuItemThreadTabCopyName";
			this.menuItemThreadTabCopyName.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabCopyName.Text = "スレッド名をコピー(&S)";
			this.menuItemThreadTabCopyName.Click += new System.EventHandler(this.menuItemThreadTabCopyName_Click);
			// 
			// menuItem47
			// 
			this.menuItem47.Name = "menuItem47";
			this.menuItem47.Size = new System.Drawing.Size(322, 6);
			// 
			// menuItemThreadTabRefresh
			// 
			this.menuItemThreadTabRefresh.Name = "menuItemThreadTabRefresh";
			this.menuItemThreadTabRefresh.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabRefresh.Text = "スレッドを更新(&R)";
			this.menuItemThreadTabRefresh.Click += new System.EventHandler(this.menuItemThreadReload_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(322, 6);
			// 
			// menuItemThreadTabOpenWebBrowser
			// 
			this.menuItemThreadTabOpenWebBrowser.Name = "menuItemThreadTabOpenWebBrowser";
			this.menuItemThreadTabOpenWebBrowser.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabOpenWebBrowser.Text = "Webブラウザで開く(&W)";
			this.menuItemThreadTabOpenWebBrowser.Click += new System.EventHandler(this.menuItemThreadTabOpenWebBrowser_Click);
			// 
			// menuItemThreadTabOpenDraft
			// 
			this.menuItemThreadTabOpenDraft.Name = "menuItemThreadTabOpenDraft";
			this.menuItemThreadTabOpenDraft.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabOpenDraft.Text = "草稿を表示(&F)";
			this.menuItemThreadTabOpenDraft.Click += new System.EventHandler(this.menuItemThreadTabOpenDraft_Click);
			// 
			// menuItem65
			// 
			this.menuItem65.Name = "menuItem65";
			this.menuItem65.Size = new System.Drawing.Size(322, 6);
			// 
			// menuItemThreadTabNewThread
			// 
			this.menuItemThreadTabNewThread.Name = "menuItemThreadTabNewThread";
			this.menuItemThreadTabNewThread.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabNewThread.Text = "次スレを立てる(&T)";
			this.menuItemThreadTabNewThread.Click += new System.EventHandler(this.menuItemThreadTabNewThread_Click);
			// 
			// menuItemThreadTabNextThreadCheck
			// 
			this.menuItemThreadTabNextThreadCheck.Name = "menuItemThreadTabNextThreadCheck";
			this.menuItemThreadTabNextThreadCheck.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabNextThreadCheck.Text = "次スレを検索(&E)";
			this.menuItemThreadTabNextThreadCheck.Click += new System.EventHandler(this.menuItemThreadNextThreadCheck_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(322, 6);
			// 
			// menuItemThreadTabAllOpenImageViewer
			// 
			this.menuItemThreadTabAllOpenImageViewer.Name = "menuItemThreadTabAllOpenImageViewer";
			this.menuItemThreadTabAllOpenImageViewer.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabAllOpenImageViewer.Text = "スレッド内の画像をImageViewerで開く(&I)...";
			this.menuItemThreadTabAllOpenImageViewer.Click += new System.EventHandler(this.menuItemThreadTabAllOpenImageViewer_Click);
			// 
			// toolStripMenuItemSaveImages
			// 
			this.toolStripMenuItemSaveImages.Name = "toolStripMenuItemSaveImages";
			this.toolStripMenuItemSaveImages.Size = new System.Drawing.Size(325, 22);
			this.toolStripMenuItemSaveImages.Text = "スレッド内の画像を一括保存(&M)...";
			this.toolStripMenuItemSaveImages.Click += new System.EventHandler(this.toolStripMenuItemSaveImages_Click);
			// 
			// menuItemThreadTabReget
			// 
			this.menuItemThreadTabReget.Name = "menuItemThreadTabReget";
			this.menuItemThreadTabReget.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabReget.Text = "ログを再取得(&G)";
			this.menuItemThreadTabReget.Click += new System.EventHandler(this.menuItemThreadTabReget_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(322, 6);
			this.toolStripSeparator3.Click += new System.EventHandler(this.toolStripSeparator3_Click);
			// 
			// menuItemThreadTabColoring
			// 
			this.menuItemThreadTabColoring.Name = "menuItemThreadTabColoring";
			this.menuItemThreadTabColoring.Size = new System.Drawing.Size(325, 22);
			this.menuItemThreadTabColoring.Text = "このスレタブを色づけ...";
			this.menuItemThreadTabColoring.ToolTipText = "このスレッドを開いたときのタブの背景色を変更します";
			this.menuItemThreadTabColoring.Click += new System.EventHandler(this.menuItemThreadTabColoring_Click);
			// 
			// threadToolPanel
			// 
			this.threadToolPanel.Controls.Add(this.labelBoardName);
			this.threadToolPanel.Controls.Add(this.labelThreadSubject);
			this.threadToolPanel.Controls.Add(this.toolBarThread);
			this.threadToolPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.threadToolPanel.Location = new System.Drawing.Point(0, 0);
			this.threadToolPanel.Margin = new System.Windows.Forms.Padding(2);
			this.threadToolPanel.Name = "threadToolPanel";
			this.threadToolPanel.Size = new System.Drawing.Size(733, 26);
			this.threadToolPanel.TabIndex = 3;
			this.threadToolPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.threadToolPanel_Paint);
			// 
			// labelBoardName
			// 
			this.labelBoardName.AutoSize = true;
			this.labelBoardName.Cursor = System.Windows.Forms.Cursors.Hand;
			this.labelBoardName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelBoardName.Location = new System.Drawing.Point(4, 7);
			this.labelBoardName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelBoardName.Name = "labelBoardName";
			this.labelBoardName.Size = new System.Drawing.Size(29, 12);
			this.labelBoardName.TabIndex = 1;
			this.labelBoardName.Text = "板名";
			this.toolTip.SetToolTip(this.labelBoardName, "クリックすると板のスレッド一覧を取得します");
			this.labelBoardName.Click += new System.EventHandler(this.labelBoardName_Click);
			// 
			// labelThreadSubject
			// 
			this.labelThreadSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelThreadSubject.Cursor = System.Windows.Forms.Cursors.Hand;
			this.labelThreadSubject.Location = new System.Drawing.Point(42, 7);
			this.labelThreadSubject.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelThreadSubject.Name = "labelThreadSubject";
			this.labelThreadSubject.Size = new System.Drawing.Size(296, 12);
			this.labelThreadSubject.TabIndex = 2;
			this.labelThreadSubject.Text = "スレッド名";
			this.toolTip.SetToolTip(this.labelThreadSubject, "クリックすると関連キーワード一覧を取得します");
			this.labelThreadSubject.Click += new System.EventHandler(this.labelThreadSubject_Click);
			// 
			// toolBarThread
			// 
			this.toolBarThread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.toolBarThread.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarThread.AutoSize = false;
			this.toolBarThread.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonViewChange,
            this.toolBarButtonScrollTop,
            this.toolBarButtonScroll,
            this.toolBarButton6,
            this.toolBarButtonReload,
            this.toolBarButtonStop,
            this.toolBarButton7,
            this.toolBarButtonWriteRes,
            this.toolBarButtonAutoReload,
            this.toolBarButtonFind,
            this.toolBarButtonResExtract,
            this.toolBarButtonBookmark,
            this.toolBarButton12,
            this.toolBarButtonDelete,
            this.toolBarButtonClose});
			this.toolBarThread.ButtonSize = new System.Drawing.Size(39, 22);
			this.toolBarThread.Divider = false;
			this.toolBarThread.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarThread.DropDownArrows = true;
			this.toolBarThread.ImageList = this.imageList;
			this.toolBarThread.Location = new System.Drawing.Point(425, 2);
			this.toolBarThread.Margin = new System.Windows.Forms.Padding(2);
			this.toolBarThread.Name = "toolBarThread";
			this.toolBarThread.ShowToolTips = true;
			this.toolBarThread.Size = new System.Drawing.Size(302, 22);
			this.toolBarThread.TabIndex = 3;
			this.toolBarThread.Wrappable = false;
			this.toolBarThread.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarThread_ButtonClick);
			this.toolBarThread.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolBarThread_MouseUp);
			// 
			// toolBarButtonViewChange
			// 
			this.toolBarButtonViewChange.ImageIndex = 21;
			this.toolBarButtonViewChange.Name = "toolBarButtonViewChange";
			this.toolBarButtonViewChange.ToolTipText = "再描画";
			// 
			// toolBarButtonScrollTop
			// 
			this.toolBarButtonScrollTop.ImageIndex = 50;
			this.toolBarButtonScrollTop.Name = "toolBarButtonScrollTop";
			this.toolBarButtonScrollTop.ToolTipText = "一番上にスクロールします";
			// 
			// toolBarButtonScroll
			// 
			this.toolBarButtonScroll.ImageIndex = 22;
			this.toolBarButtonScroll.Name = "toolBarButtonScroll";
			this.toolBarButtonScroll.ToolTipText = "スクロール (右クリックで別メニュー)";
			// 
			// toolBarButton6
			// 
			this.toolBarButton6.Name = "toolBarButton6";
			this.toolBarButton6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonReload
			// 
			this.toolBarButtonReload.ImageIndex = 8;
			this.toolBarButtonReload.Name = "toolBarButtonReload";
			this.toolBarButtonReload.ToolTipText = "スレッドを更新 (右クリックで別メニュー)";
			// 
			// toolBarButtonStop
			// 
			this.toolBarButtonStop.ImageIndex = 9;
			this.toolBarButtonStop.Name = "toolBarButtonStop";
			this.toolBarButtonStop.ToolTipText = "読み込み中止";
			// 
			// toolBarButton7
			// 
			this.toolBarButton7.Name = "toolBarButton7";
			this.toolBarButton7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonWriteRes
			// 
			this.toolBarButtonWriteRes.ImageIndex = 15;
			this.toolBarButtonWriteRes.Name = "toolBarButtonWriteRes";
			this.toolBarButtonWriteRes.ToolTipText = "レスを書く";
			// 
			// toolBarButtonAutoReload
			// 
			this.toolBarButtonAutoReload.ImageIndex = 10;
			this.toolBarButtonAutoReload.Name = "toolBarButtonAutoReload";
			this.toolBarButtonAutoReload.ToolTipText = "オートリロード (右クリックで別メニュー)";
			// 
			// toolBarButtonFind
			// 
			this.toolBarButtonFind.ImageIndex = 14;
			this.toolBarButtonFind.Name = "toolBarButtonFind";
			this.toolBarButtonFind.ToolTipText = "スレッド内を検索";
			// 
			// toolBarButtonResExtract
			// 
			this.toolBarButtonResExtract.ImageIndex = 13;
			this.toolBarButtonResExtract.Name = "toolBarButtonResExtract";
			this.toolBarButtonResExtract.ToolTipText = "レスを抽出";
			// 
			// toolBarButtonBookmark
			// 
			this.toolBarButtonBookmark.ImageIndex = 27;
			this.toolBarButtonBookmark.Name = "toolBarButtonBookmark";
			this.toolBarButtonBookmark.ToolTipText = "お気に入りの登録・解除";
			// 
			// toolBarButton12
			// 
			this.toolBarButton12.Name = "toolBarButton12";
			this.toolBarButton12.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonDelete
			// 
			this.toolBarButtonDelete.ImageIndex = 12;
			this.toolBarButtonDelete.Name = "toolBarButtonDelete";
			this.toolBarButtonDelete.ToolTipText = "ログを削除して閉じる";
			// 
			// toolBarButtonClose
			// 
			this.toolBarButtonClose.ImageIndex = 11;
			this.toolBarButtonClose.Name = "toolBarButtonClose";
			this.toolBarButtonClose.ToolTipText = "スレッドを閉じる";
			// 
			// splitterLeft
			// 
			this.splitterLeft.Location = new System.Drawing.Point(160, 54);
			this.splitterLeft.Margin = new System.Windows.Forms.Padding(2);
			this.splitterLeft.Name = "splitterLeft";
			this.splitterLeft.Size = new System.Drawing.Size(3, 456);
			this.splitterLeft.TabIndex = 4;
			this.splitterLeft.TabStop = false;
			// 
			// splitterTop
			// 
			this.splitterTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitterTop.Location = new System.Drawing.Point(163, 192);
			this.splitterTop.Margin = new System.Windows.Forms.Padding(2);
			this.splitterTop.Name = "splitterTop";
			this.splitterTop.Size = new System.Drawing.Size(733, 3);
			this.splitterTop.TabIndex = 5;
			this.splitterTop.TabStop = false;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 510);
			this.statusBar.Margin = new System.Windows.Forms.Padding(2);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusText,
            this.statusProgress,
            this.statusSize,
            this.statusResCount,
            this.statusForce,
            this.statusSamba24,
            this.statusTimerCount});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(896, 17);
			this.statusBar.TabIndex = 6;
			this.statusBar.DrawItem += new System.Windows.Forms.StatusBarDrawItemEventHandler(this.statusBar_DrawItem);
			this.statusBar.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.statusBar_PanelClick);
			// 
			// statusText
			// 
			this.statusText.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusText.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
			this.statusText.Name = "statusText";
			this.statusText.Width = 429;
			// 
			// statusProgress
			// 
			this.statusProgress.Name = "statusProgress";
			this.statusProgress.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusProgress.ToolTipText = "データ受信の進行状態を表します";
			this.statusProgress.Width = 80;
			// 
			// statusSize
			// 
			this.statusSize.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.statusSize.Name = "statusSize";
			this.statusSize.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusSize.ToolTipText = "スレッドのサイズを表します";
			this.statusSize.Width = 60;
			// 
			// statusResCount
			// 
			this.statusResCount.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusResCount.Name = "statusResCount";
			this.statusResCount.ToolTipText = "レス数を表します (新着/既得)";
			this.statusResCount.Width = 60;
			// 
			// statusForce
			// 
			this.statusForce.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusForce.Name = "statusForce";
			this.statusForce.ToolTipText = "スレッドの勢いを表します";
			this.statusForce.Width = 60;
			// 
			// statusSamba24
			// 
			this.statusSamba24.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusSamba24.Name = "statusSamba24";
			this.statusSamba24.Text = "samba24";
			this.statusSamba24.ToolTipText = "アクティブなスレッドの連投規制の秒数を表します。クリックで最新の値に更新します。";
			// 
			// statusTimerCount
			// 
			this.statusTimerCount.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusTimerCount.Name = "statusTimerCount";
			this.statusTimerCount.ToolTipText = "オートリロードがOnの場合、次の更新までの秒数を表します";
			this.statusTimerCount.Width = 90;
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 8000;
			this.toolTip.InitialDelay = 100;
			this.toolTip.ReshowDelay = 100;
			// 
			// contextMenuViewChange
			// 
			this.contextMenuViewChange.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemItemViewFirst,
            this.menuItemSetLimitFirstXXX,
            this.menuItemItemViewLast,
            this.menuItemSetLimitLastXXX,
            this.menuItem3,
            this.menuItemViewChangePrev,
            this.menuItemViewChangeNext,
            this.menuItemViewAll,
            this.menuItem80,
            this.menuItemViewNewResOnly,
            this.menuItemViewSirusi,
            this.toolStripSeparator2,
            this.menuItemViewShiori,
            this.menuItemRemoveShiori});
			this.contextMenuViewChange.Name = "contextMenuViewChange";
			this.contextMenuViewChange.ShowImageMargin = false;
			this.contextMenuViewChange.Size = new System.Drawing.Size(167, 264);
			this.contextMenuViewChange.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuViewChange_Popup);
			// 
			// menuItemItemViewFirst
			// 
			this.menuItemItemViewFirst.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSetLimitFirst50,
            this.menuItemSetLimitFirst100,
            this.menuItemSetLimitFirst250,
            this.menuItemSetLimitFirst500,
            this.menuItemSetLimitFirst1000});
			this.menuItemItemViewFirst.Name = "menuItemItemViewFirst";
			this.menuItemItemViewFirst.Size = new System.Drawing.Size(166, 22);
			this.menuItemItemViewFirst.Text = "始めから(&F)";
			// 
			// menuItemSetLimitFirst50
			// 
			this.menuItemSetLimitFirst50.Name = "menuItemSetLimitFirst50";
			this.menuItemSetLimitFirst50.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitFirst50.Text = "50";
			this.menuItemSetLimitFirst50.Click += new System.EventHandler(this.menuItemSetLimitFirst_Click);
			// 
			// menuItemSetLimitFirst100
			// 
			this.menuItemSetLimitFirst100.Name = "menuItemSetLimitFirst100";
			this.menuItemSetLimitFirst100.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitFirst100.Text = "100";
			this.menuItemSetLimitFirst100.Click += new System.EventHandler(this.menuItemSetLimitFirst_Click);
			// 
			// menuItemSetLimitFirst250
			// 
			this.menuItemSetLimitFirst250.Name = "menuItemSetLimitFirst250";
			this.menuItemSetLimitFirst250.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitFirst250.Text = "250";
			this.menuItemSetLimitFirst250.Click += new System.EventHandler(this.menuItemSetLimitFirst_Click);
			// 
			// menuItemSetLimitFirst500
			// 
			this.menuItemSetLimitFirst500.Name = "menuItemSetLimitFirst500";
			this.menuItemSetLimitFirst500.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitFirst500.Text = "500";
			this.menuItemSetLimitFirst500.Click += new System.EventHandler(this.menuItemSetLimitFirst_Click);
			// 
			// menuItemSetLimitFirst1000
			// 
			this.menuItemSetLimitFirst1000.Name = "menuItemSetLimitFirst1000";
			this.menuItemSetLimitFirst1000.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitFirst1000.Text = "1000";
			this.menuItemSetLimitFirst1000.Click += new System.EventHandler(this.menuItemSetLimitFirst_Click);
			// 
			// menuItemSetLimitFirstXXX
			// 
			this.menuItemSetLimitFirstXXX.Name = "menuItemSetLimitFirstXXX";
			this.menuItemSetLimitFirstXXX.Size = new System.Drawing.Size(166, 22);
			this.menuItemSetLimitFirstXXX.Text = "    dummy";
			this.menuItemSetLimitFirstXXX.Click += new System.EventHandler(this.menuItemSetLimitFirst_Click);
			// 
			// menuItemItemViewLast
			// 
			this.menuItemItemViewLast.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSetLimitLast50,
            this.menuItemSetLimitLast100,
            this.menuItemSetLimitLast250,
            this.menuItemSetLimitLast500,
            this.menuItemSetLimitLast1000});
			this.menuItemItemViewLast.Name = "menuItemItemViewLast";
			this.menuItemItemViewLast.Size = new System.Drawing.Size(166, 22);
			this.menuItemItemViewLast.Text = "終わりから(&L)";
			// 
			// menuItemSetLimitLast50
			// 
			this.menuItemSetLimitLast50.Name = "menuItemSetLimitLast50";
			this.menuItemSetLimitLast50.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitLast50.Text = "50";
			this.menuItemSetLimitLast50.Click += new System.EventHandler(this.menuItemSetLimitLast_Click);
			// 
			// menuItemSetLimitLast100
			// 
			this.menuItemSetLimitLast100.Name = "menuItemSetLimitLast100";
			this.menuItemSetLimitLast100.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitLast100.Text = "100";
			this.menuItemSetLimitLast100.Click += new System.EventHandler(this.menuItemSetLimitLast_Click);
			// 
			// menuItemSetLimitLast250
			// 
			this.menuItemSetLimitLast250.Name = "menuItemSetLimitLast250";
			this.menuItemSetLimitLast250.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitLast250.Text = "250";
			this.menuItemSetLimitLast250.Click += new System.EventHandler(this.menuItemSetLimitLast_Click);
			// 
			// menuItemSetLimitLast500
			// 
			this.menuItemSetLimitLast500.Name = "menuItemSetLimitLast500";
			this.menuItemSetLimitLast500.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitLast500.Text = "500";
			this.menuItemSetLimitLast500.Click += new System.EventHandler(this.menuItemSetLimitLast_Click);
			// 
			// menuItemSetLimitLast1000
			// 
			this.menuItemSetLimitLast1000.Name = "menuItemSetLimitLast1000";
			this.menuItemSetLimitLast1000.Size = new System.Drawing.Size(104, 22);
			this.menuItemSetLimitLast1000.Text = "1000";
			this.menuItemSetLimitLast1000.Click += new System.EventHandler(this.menuItemSetLimitLast_Click);
			// 
			// menuItemSetLimitLastXXX
			// 
			this.menuItemSetLimitLastXXX.Name = "menuItemSetLimitLastXXX";
			this.menuItemSetLimitLastXXX.Size = new System.Drawing.Size(166, 22);
			this.menuItemSetLimitLastXXX.Text = "    dummy";
			this.menuItemSetLimitLastXXX.Click += new System.EventHandler(this.menuItemSetLimitLast_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Name = "menuItem3";
			this.menuItem3.Size = new System.Drawing.Size(163, 6);
			// 
			// menuItemViewChangePrev
			// 
			this.menuItemViewChangePrev.Name = "menuItemViewChangePrev";
			this.menuItemViewChangePrev.Size = new System.Drawing.Size(166, 22);
			this.menuItemViewChangePrev.Text = "前のXXXレス(&P)";
			this.menuItemViewChangePrev.Click += new System.EventHandler(this.menuItemViewChangePrev_Click);
			// 
			// menuItemViewChangeNext
			// 
			this.menuItemViewChangeNext.Name = "menuItemViewChangeNext";
			this.menuItemViewChangeNext.Size = new System.Drawing.Size(166, 22);
			this.menuItemViewChangeNext.Text = "次のXXXレス(&N)";
			this.menuItemViewChangeNext.Click += new System.EventHandler(this.menuItemViewChangeNext_Click);
			// 
			// menuItemViewAll
			// 
			this.menuItemViewAll.Name = "menuItemViewAll";
			this.menuItemViewAll.Size = new System.Drawing.Size(166, 22);
			this.menuItemViewAll.Text = "すべてのﾚｽを表示(&A)";
			this.menuItemViewAll.Click += new System.EventHandler(this.menuItemViewAll_Click);
			// 
			// menuItem80
			// 
			this.menuItem80.Name = "menuItem80";
			this.menuItem80.Size = new System.Drawing.Size(163, 6);
			// 
			// menuItemViewNewResOnly
			// 
			this.menuItemViewNewResOnly.Name = "menuItemViewNewResOnly";
			this.menuItemViewNewResOnly.Size = new System.Drawing.Size(166, 22);
			this.menuItemViewNewResOnly.Text = "新着レスのみ表示(&R)";
			this.menuItemViewNewResOnly.Click += new System.EventHandler(this.menuItemViewNewResOnly_Click);
			// 
			// menuItemViewSirusi
			// 
			this.menuItemViewSirusi.Name = "menuItemViewSirusi";
			this.menuItemViewSirusi.Size = new System.Drawing.Size(166, 22);
			this.menuItemViewSirusi.Text = "しるしレスを表示(&U)";
			this.menuItemViewSirusi.Click += new System.EventHandler(this.menuItemViewSirusi_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(163, 6);
			// 
			// menuItemViewShiori
			// 
			this.menuItemViewShiori.Name = "menuItemViewShiori";
			this.menuItemViewShiori.Size = new System.Drawing.Size(166, 22);
			this.menuItemViewShiori.Text = "しおりを開く(&S)";
			this.menuItemViewShiori.Click += new System.EventHandler(this.menuItemViewShiori_Click);
			// 
			// menuItemRemoveShiori
			// 
			this.menuItemRemoveShiori.Name = "menuItemRemoveShiori";
			this.menuItemRemoveShiori.Size = new System.Drawing.Size(166, 22);
			this.menuItemRemoveShiori.Text = "しおりを削除(&D)";
			this.menuItemRemoveShiori.Click += new System.EventHandler(this.menuItemRemoveShiori_Click);
			// 
			// contextMenuScroll
			// 
			this.contextMenuScroll.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemScrollSetAutoScroll,
            this.menuItemScrollSetNewScroll,
            this.menuItemScrollBack});
			this.contextMenuScroll.Name = "contextMenuScroll";
			this.contextMenuScroll.Size = new System.Drawing.Size(204, 70);
			this.contextMenuScroll.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuScroll_Popup);
			// 
			// menuItemScrollSetAutoScroll
			// 
			this.menuItemScrollSetAutoScroll.Name = "menuItemScrollSetAutoScroll";
			this.menuItemScrollSetAutoScroll.Size = new System.Drawing.Size(203, 22);
			this.menuItemScrollSetAutoScroll.Text = "オートスクロール(&A)";
			this.menuItemScrollSetAutoScroll.Click += new System.EventHandler(this.menuItemThreadAutoScroll_Click);
			// 
			// menuItemScrollSetNewScroll
			// 
			this.menuItemScrollSetNewScroll.Name = "menuItemScrollSetNewScroll";
			this.menuItemScrollSetNewScroll.Size = new System.Drawing.Size(203, 22);
			this.menuItemScrollSetNewScroll.Text = "新着までスクロール(&N)";
			this.menuItemScrollSetNewScroll.Click += new System.EventHandler(this.menuItemScrollSetNewScroll_Click);
			// 
			// menuItemScrollBack
			// 
			this.menuItemScrollBack.Name = "menuItemScrollBack";
			this.menuItemScrollBack.Size = new System.Drawing.Size(203, 22);
			this.menuItemScrollBack.Text = "前に戻る(&B)";
			this.menuItemScrollBack.Click += new System.EventHandler(this.menuItemScrollBack_Click);
			// 
			// contextMenuRead
			// 
			this.contextMenuRead.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReadReget,
            this.menuItemReadReloadAll});
			this.contextMenuRead.Name = "contextMenuRead";
			this.contextMenuRead.ShowImageMargin = false;
			this.contextMenuRead.Size = new System.Drawing.Size(202, 48);
			this.contextMenuRead.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuRead_Popup);
			// 
			// menuItemReadReget
			// 
			this.menuItemReadReget.Name = "menuItemReadReget";
			this.menuItemReadReget.Size = new System.Drawing.Size(201, 22);
			this.menuItemReadReget.Text = "ログを再取得(&R)";
			this.menuItemReadReget.Click += new System.EventHandler(this.menuItemThreadReget_Click);
			// 
			// menuItemReadReloadAll
			// 
			this.menuItemReadReloadAll.Name = "menuItemReadReloadAll";
			this.menuItemReadReloadAll.Size = new System.Drawing.Size(201, 22);
			this.menuItemReadReloadAll.Text = "すべてのスレッドを更新(&A)";
			this.menuItemReadReloadAll.Click += new System.EventHandler(this.menuItemReadReloadAll_Click);
			// 
			// contextMenuRes
			// 
			this.contextMenuRes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemResWrite,
            this.menuItemRefResWrite,
            this.menuItem6,
            this.menuItemResCopy,
            this.menuItemResRefCopy,
            this.menuItemResCopyID,
            this.menuItem69,
            this.menuItemResCopyUrl,
            this.menuItemResCopyNameUrl,
            this.menuItem16,
            this.menuItemResIDPopup,
            this.menuItemResBackReference,
            this.menuItem54,
            this.menuItemResAddNG,
            this.menuItem28,
            this.menuItemResBookmark,
            this.menuItemResSirusi,
            this.menuItem72,
            this.menuItemResKokokara,
            this.menuItemResKokomade,
            this.menuItem21,
            this.menuItemResOpenLinks,
            this.menuItem73,
            this.menuItemResABone,
            this.menuItemResHideABone});
			this.contextMenuRes.Name = "contextMenuRes";
			this.contextMenuRes.ShowImageMargin = false;
			this.contextMenuRes.Size = new System.Drawing.Size(250, 426);
			this.contextMenuRes.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuRes_Popup);
			// 
			// menuItemResWrite
			// 
			this.menuItemResWrite.Name = "menuItemResWrite";
			this.menuItemResWrite.Size = new System.Drawing.Size(249, 22);
			this.menuItemResWrite.Text = "これにレス(&R)...";
			this.menuItemResWrite.Click += new System.EventHandler(this.menuItemResWrite_Click);
			// 
			// menuItemRefResWrite
			// 
			this.menuItemRefResWrite.Name = "menuItemRefResWrite";
			this.menuItemRefResWrite.Size = new System.Drawing.Size(249, 22);
			this.menuItemRefResWrite.Text = "引用符を付加してレス(&H)";
			this.menuItemRefResWrite.Click += new System.EventHandler(this.menuItemRefResWrite_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Name = "menuItem6";
			this.menuItem6.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResCopy
			// 
			this.menuItemResCopy.Name = "menuItemResCopy";
			this.menuItemResCopy.Size = new System.Drawing.Size(249, 22);
			this.menuItemResCopy.Text = "レスをコピー(&C)";
			this.menuItemResCopy.Click += new System.EventHandler(this.menuItemResCopy_Click);
			// 
			// menuItemResRefCopy
			// 
			this.menuItemResRefCopy.Name = "menuItemResRefCopy";
			this.menuItemResRefCopy.Size = new System.Drawing.Size(249, 22);
			this.menuItemResRefCopy.Text = "引用符を付加してコピー(&O)";
			this.menuItemResRefCopy.Click += new System.EventHandler(this.menuItemResRefCopy_Click);
			// 
			// menuItemResCopyID
			// 
			this.menuItemResCopyID.Name = "menuItemResCopyID";
			this.menuItemResCopyID.Size = new System.Drawing.Size(249, 22);
			this.menuItemResCopyID.Text = "IDをコピー(&I)";
			this.menuItemResCopyID.Click += new System.EventHandler(this.menuItemResCopyID_Click);
			// 
			// menuItem69
			// 
			this.menuItem69.Name = "menuItem69";
			this.menuItem69.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResCopyUrl
			// 
			this.menuItemResCopyUrl.Name = "menuItemResCopyUrl";
			this.menuItemResCopyUrl.Size = new System.Drawing.Size(249, 22);
			this.menuItemResCopyUrl.Text = "レスのURLをコピー(&H)";
			this.menuItemResCopyUrl.Click += new System.EventHandler(this.menuItemResCopyUrl_Click);
			// 
			// menuItemResCopyNameUrl
			// 
			this.menuItemResCopyNameUrl.Name = "menuItemResCopyNameUrl";
			this.menuItemResCopyNameUrl.Size = new System.Drawing.Size(249, 22);
			this.menuItemResCopyNameUrl.Text = "スレッド名とレスのURLをコピー(&T)";
			this.menuItemResCopyNameUrl.Click += new System.EventHandler(this.menuItemResCopyNameUrl_Click);
			// 
			// menuItem16
			// 
			this.menuItem16.Name = "menuItem16";
			this.menuItem16.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResIDPopup
			// 
			this.menuItemResIDPopup.Name = "menuItemResIDPopup";
			this.menuItemResIDPopup.Size = new System.Drawing.Size(249, 22);
			this.menuItemResIDPopup.Text = "このIDのレスを抽出表示(&D)";
			this.menuItemResIDPopup.Click += new System.EventHandler(this.menuItemResIDPopup_Click);
			// 
			// menuItemResBackReference
			// 
			this.menuItemResBackReference.Name = "menuItemResBackReference";
			this.menuItemResBackReference.Size = new System.Drawing.Size(249, 22);
			this.menuItemResBackReference.Text = "これを参照しているレスの表示(&F)";
			this.menuItemResBackReference.Click += new System.EventHandler(this.menuItemResBackReference_Click);
			// 
			// menuItem54
			// 
			this.menuItem54.Name = "menuItem54";
			this.menuItem54.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResAddNG
			// 
			this.menuItemResAddNG.Name = "menuItemResAddNG";
			this.menuItemResAddNG.Size = new System.Drawing.Size(249, 22);
			this.menuItemResAddNG.Text = "IDをNGワードに追加(&N)";
			this.menuItemResAddNG.Click += new System.EventHandler(this.menuItemResAddNG_Click);
			// 
			// menuItem28
			// 
			this.menuItem28.Name = "menuItem28";
			this.menuItem28.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResBookmark
			// 
			this.menuItemResBookmark.Name = "menuItemResBookmark";
			this.menuItemResBookmark.Size = new System.Drawing.Size(249, 22);
			this.menuItemResBookmark.Text = "ココまで読んだの(&B)";
			this.menuItemResBookmark.Click += new System.EventHandler(this.menuItemResBookmark_Click);
			// 
			// menuItemResSirusi
			// 
			this.menuItemResSirusi.Name = "menuItemResSirusi";
			this.menuItemResSirusi.Size = new System.Drawing.Size(249, 22);
			this.menuItemResSirusi.Text = "コレをしるしする(&M)";
			this.menuItemResSirusi.Click += new System.EventHandler(this.menuItemResSirusi_Click);
			// 
			// menuItem72
			// 
			this.menuItem72.Name = "menuItem72";
			this.menuItem72.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResKokokara
			// 
			this.menuItemResKokokara.Name = "menuItemResKokokara";
			this.menuItemResKokokara.Size = new System.Drawing.Size(249, 22);
			this.menuItemResKokokara.Text = "ココから連続して(&E)";
			this.menuItemResKokokara.Click += new System.EventHandler(this.menuItemResKokokara_Click);
			// 
			// menuItemResKokomade
			// 
			this.menuItemResKokomade.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemResKokoMadeWrite,
            this.menuItem57,
            this.menuItemKokoMadeCopy,
            this.menuItemResKokoMadeRefCopy,
            this.menuItem64,
            this.menuItemResKokoMadeCopyUrl,
            this.menuItemResKokoMadeCopyNameUrl,
            this.menuItem61,
            this.menuItemResKokoMadeAddNGID,
            this.toolStripSeparator5,
            this.menuItemResKokoMadeLink,
            this.menuItem63,
            this.menuItemResKokoMadeABone,
            this.menuItemResKokoMadeHideABone,
            this.menuItem66,
            this.menuItemResKokoMadeCancel});
			this.menuItemResKokomade.Name = "menuItemResKokomade";
			this.menuItemResKokomade.Size = new System.Drawing.Size(249, 22);
			this.menuItemResKokomade.Text = "ココまで連続して(&E)";
			this.menuItemResKokomade.Visible = false;
			// 
			// menuItemResKokoMadeWrite
			// 
			this.menuItemResKokoMadeWrite.Name = "menuItemResKokoMadeWrite";
			this.menuItemResKokoMadeWrite.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeWrite.Text = "これらにレス(&R)";
			this.menuItemResKokoMadeWrite.Click += new System.EventHandler(this.menuItemResKokoMadeWrite_Click);
			// 
			// menuItem57
			// 
			this.menuItem57.Name = "menuItem57";
			this.menuItem57.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemKokoMadeCopy
			// 
			this.menuItemKokoMadeCopy.Name = "menuItemKokoMadeCopy";
			this.menuItemKokoMadeCopy.Size = new System.Drawing.Size(274, 22);
			this.menuItemKokoMadeCopy.Text = "レスをコピー(&C)";
			this.menuItemKokoMadeCopy.Click += new System.EventHandler(this.menuItemKokoMadeCopy_Click);
			// 
			// menuItemResKokoMadeRefCopy
			// 
			this.menuItemResKokoMadeRefCopy.Name = "menuItemResKokoMadeRefCopy";
			this.menuItemResKokoMadeRefCopy.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeRefCopy.Text = "引用符を付加してコピー(&O)";
			this.menuItemResKokoMadeRefCopy.Click += new System.EventHandler(this.menuItemResKokoMadeRefCopy_Click);
			// 
			// menuItem64
			// 
			this.menuItem64.Name = "menuItem64";
			this.menuItem64.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemResKokoMadeCopyUrl
			// 
			this.menuItemResKokoMadeCopyUrl.Name = "menuItemResKokoMadeCopyUrl";
			this.menuItemResKokoMadeCopyUrl.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeCopyUrl.Text = "レスのURLをコピー(&H)";
			this.menuItemResKokoMadeCopyUrl.Click += new System.EventHandler(this.menuItemResKokoMadeCopyUrl_Click);
			// 
			// menuItemResKokoMadeCopyNameUrl
			// 
			this.menuItemResKokoMadeCopyNameUrl.Name = "menuItemResKokoMadeCopyNameUrl";
			this.menuItemResKokoMadeCopyNameUrl.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeCopyNameUrl.Text = "スレッド名とレスのURLをコピー(&T)";
			this.menuItemResKokoMadeCopyNameUrl.Click += new System.EventHandler(this.menuItemResKokoMadeCopyNameUrl_Click);
			// 
			// menuItem61
			// 
			this.menuItem61.Name = "menuItem61";
			this.menuItem61.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemResKokoMadeAddNGID
			// 
			this.menuItemResKokoMadeAddNGID.Name = "menuItemResKokoMadeAddNGID";
			this.menuItemResKokoMadeAddNGID.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeAddNGID.Text = "IDをNGワードに追加(&N)";
			this.menuItemResKokoMadeAddNGID.Click += new System.EventHandler(this.menuItemResKokoMadeAddNGID_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemResKokoMadeLink
			// 
			this.menuItemResKokoMadeLink.Name = "menuItemResKokoMadeLink";
			this.menuItemResKokoMadeLink.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeLink.Text = "すべてのリンクを開く(&L)";
			this.menuItemResKokoMadeLink.Click += new System.EventHandler(this.menuItemResKokoMadeLink_Click);
			// 
			// menuItem63
			// 
			this.menuItem63.Name = "menuItem63";
			this.menuItem63.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemResKokoMadeABone
			// 
			this.menuItemResKokoMadeABone.Name = "menuItemResKokoMadeABone";
			this.menuItemResKokoMadeABone.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeABone.Text = "あぼーん(&A)";
			this.menuItemResKokoMadeABone.Click += new System.EventHandler(this.menuItemResKokoMadeABone_Click);
			// 
			// menuItemResKokoMadeHideABone
			// 
			this.menuItemResKokoMadeHideABone.Name = "menuItemResKokoMadeHideABone";
			this.menuItemResKokoMadeHideABone.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeHideABone.Text = "透明あぼーん(&Z)";
			this.menuItemResKokoMadeHideABone.Click += new System.EventHandler(this.menuItemResKokoMadeHideABone_Click);
			// 
			// menuItem66
			// 
			this.menuItem66.Name = "menuItem66";
			this.menuItem66.Size = new System.Drawing.Size(271, 6);
			// 
			// menuItemResKokoMadeCancel
			// 
			this.menuItemResKokoMadeCancel.Name = "menuItemResKokoMadeCancel";
			this.menuItemResKokoMadeCancel.Size = new System.Drawing.Size(274, 22);
			this.menuItemResKokoMadeCancel.Text = "キャンセル(&C)";
			this.menuItemResKokoMadeCancel.Click += new System.EventHandler(this.menuItemResKokoMadeCancel_Click);
			// 
			// menuItem21
			// 
			this.menuItem21.Name = "menuItem21";
			this.menuItem21.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResOpenLinks
			// 
			this.menuItemResOpenLinks.Name = "menuItemResOpenLinks";
			this.menuItemResOpenLinks.Size = new System.Drawing.Size(249, 22);
			this.menuItemResOpenLinks.Text = "すべてのリンクを開く(&L)";
			this.menuItemResOpenLinks.Click += new System.EventHandler(this.menuItemResOpenLinks_Click);
			// 
			// menuItem73
			// 
			this.menuItem73.Name = "menuItem73";
			this.menuItem73.Size = new System.Drawing.Size(246, 6);
			// 
			// menuItemResABone
			// 
			this.menuItemResABone.Name = "menuItemResABone";
			this.menuItemResABone.Size = new System.Drawing.Size(249, 22);
			this.menuItemResABone.Text = "あぼーん(&A)";
			this.menuItemResABone.Click += new System.EventHandler(this.menuItemResABone_Click);
			// 
			// menuItemResHideABone
			// 
			this.menuItemResHideABone.Name = "menuItemResHideABone";
			this.menuItemResHideABone.Size = new System.Drawing.Size(249, 22);
			this.menuItemResHideABone.Text = "透明あぼーん(&T)";
			this.menuItemResHideABone.Click += new System.EventHandler(this.menuItemResHideABone_Click);
			// 
			// contextMenuListView
			// 
			this.contextMenuListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemListOpenNewTab,
            this.menuItem15,
            this.menuItemListHeadPopup,
            this.menuItemListNewResPopup,
            this.menuItem29,
            this.menuItemListCopyURL,
            this.menuItemListCopyURLName,
            this.menuItemListCopyName,
            this.menuItem33,
            this.menuItemListOpenWebBrowser,
            this.menuItem22,
            this.menuItemListSetBookmark,
            this.menuItemListSetWarehouse,
            this.menuItem74,
            this.menuItemListSearchNext,
            this.toolStripMenuItem12,
            this.menuItemListUpdateCheck,
            this.menuItemListPastlog,
            this.menuItem40,
            this.menuItemListABone,
            this.menuItemListDeleteLog});
			this.contextMenuListView.Name = "contextMenuListView";
			this.contextMenuListView.Size = new System.Drawing.Size(239, 354);
			this.contextMenuListView.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuListView_Popup);
			// 
			// menuItemListOpenNewTab
			// 
			this.menuItemListOpenNewTab.Name = "menuItemListOpenNewTab";
			this.menuItemListOpenNewTab.Size = new System.Drawing.Size(238, 22);
			this.menuItemListOpenNewTab.Text = "新しいタブで開く(&N)";
			this.menuItemListOpenNewTab.Click += new System.EventHandler(this.menuItemListOpenNewTab_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Name = "menuItem15";
			this.menuItem15.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemListHeadPopup
			// 
			this.menuItemListHeadPopup.Enabled = false;
			this.menuItemListHeadPopup.Name = "menuItemListHeadPopup";
			this.menuItemListHeadPopup.Size = new System.Drawing.Size(238, 22);
			this.menuItemListHeadPopup.Text = "1をポップアップ(&H)";
			this.menuItemListHeadPopup.Click += new System.EventHandler(this.menuItemListHeadPopup_Click);
			// 
			// menuItemListNewResPopup
			// 
			this.menuItemListNewResPopup.Enabled = false;
			this.menuItemListNewResPopup.Name = "menuItemListNewResPopup";
			this.menuItemListNewResPopup.Size = new System.Drawing.Size(238, 22);
			this.menuItemListNewResPopup.Text = "新着ポップアップ(&P)";
			this.menuItemListNewResPopup.Click += new System.EventHandler(this.menuItemListNewResPopup_Click);
			// 
			// menuItem29
			// 
			this.menuItem29.Name = "menuItem29";
			this.menuItem29.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemListCopyURL
			// 
			this.menuItemListCopyURL.Name = "menuItemListCopyURL";
			this.menuItemListCopyURL.Size = new System.Drawing.Size(238, 22);
			this.menuItemListCopyURL.Text = "URLをコピー(&U)";
			this.menuItemListCopyURL.Click += new System.EventHandler(this.menuItemListCopyURL_Click);
			// 
			// menuItemListCopyURLName
			// 
			this.menuItemListCopyURLName.Name = "menuItemListCopyURLName";
			this.menuItemListCopyURLName.Size = new System.Drawing.Size(238, 22);
			this.menuItemListCopyURLName.Text = "URLとスレッド名をコピー(&C)";
			this.menuItemListCopyURLName.Click += new System.EventHandler(this.menuItemListCopyURLName_Click);
			// 
			// menuItemListCopyName
			// 
			this.menuItemListCopyName.Name = "menuItemListCopyName";
			this.menuItemListCopyName.Size = new System.Drawing.Size(238, 22);
			this.menuItemListCopyName.Text = "スレッド名をコピー(&E)";
			this.menuItemListCopyName.Click += new System.EventHandler(this.menuItemListCopyName_Click);
			// 
			// menuItem33
			// 
			this.menuItem33.Name = "menuItem33";
			this.menuItem33.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemListOpenWebBrowser
			// 
			this.menuItemListOpenWebBrowser.Name = "menuItemListOpenWebBrowser";
			this.menuItemListOpenWebBrowser.Size = new System.Drawing.Size(238, 22);
			this.menuItemListOpenWebBrowser.Text = "Webブラウザで開く(&O)";
			this.menuItemListOpenWebBrowser.Click += new System.EventHandler(this.menuItemListOpenWebBrowser_Click);
			// 
			// menuItem22
			// 
			this.menuItem22.Name = "menuItem22";
			this.menuItem22.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemListSetBookmark
			// 
			this.menuItemListSetBookmark.Name = "menuItemListSetBookmark";
			this.menuItemListSetBookmark.Size = new System.Drawing.Size(238, 22);
			this.menuItemListSetBookmark.Text = "お気に入り(&B)";
			this.menuItemListSetBookmark.Click += new System.EventHandler(this.menuItemListSetBookmark_Click);
			// 
			// menuItemListSetWarehouse
			// 
			this.menuItemListSetWarehouse.Name = "menuItemListSetWarehouse";
			this.menuItemListSetWarehouse.Size = new System.Drawing.Size(238, 22);
			this.menuItemListSetWarehouse.Text = "過去ログ倉庫(&W)";
			this.menuItemListSetWarehouse.Click += new System.EventHandler(this.menuItemListSetWarehouse_Click);
			// 
			// menuItem74
			// 
			this.menuItem74.Name = "menuItem74";
			this.menuItem74.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemListSearchNext
			// 
			this.menuItemListSearchNext.Name = "menuItemListSearchNext";
			this.menuItemListSearchNext.Size = new System.Drawing.Size(238, 22);
			this.menuItemListSearchNext.Text = "次スレを検索(&S)";
			this.menuItemListSearchNext.Click += new System.EventHandler(this.menuItemListSearchNext_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemListUpdateCheck
			// 
			this.menuItemListUpdateCheck.Name = "menuItemListUpdateCheck";
			this.menuItemListUpdateCheck.Size = new System.Drawing.Size(238, 22);
			this.menuItemListUpdateCheck.Text = "定期更新チェック(&L)";
			this.menuItemListUpdateCheck.Click += new System.EventHandler(this.menuItemListUpdateCheck_Click);
			// 
			// menuItemListPastlog
			// 
			this.menuItemListPastlog.Name = "menuItemListPastlog";
			this.menuItemListPastlog.Size = new System.Drawing.Size(238, 22);
			this.menuItemListPastlog.Text = "dat落ちログ(&A)";
			this.menuItemListPastlog.Click += new System.EventHandler(this.menuItemListPastlog_Click);
			// 
			// menuItem40
			// 
			this.menuItem40.Name = "menuItem40";
			this.menuItem40.Size = new System.Drawing.Size(235, 6);
			// 
			// menuItemListABone
			// 
			this.menuItemListABone.Name = "menuItemListABone";
			this.menuItemListABone.Size = new System.Drawing.Size(238, 22);
			this.menuItemListABone.Text = "透明あぼーん(&X)";
			this.menuItemListABone.Click += new System.EventHandler(this.menuItemListABone_Click);
			// 
			// menuItemListDeleteLog
			// 
			this.menuItemListDeleteLog.Name = "menuItemListDeleteLog";
			this.menuItemListDeleteLog.Size = new System.Drawing.Size(238, 22);
			this.menuItemListDeleteLog.Text = "ログを削除(&D)";
			this.menuItemListDeleteLog.Click += new System.EventHandler(this.menuItemListDeleteLog_Click);
			// 
			// imageListLv
			// 
			this.imageListLv.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLv.ImageStream")));
			this.imageListLv.TransparentColor = System.Drawing.Color.Magenta;
			this.imageListLv.Images.SetKeyName(0, "");
			this.imageListLv.Images.SetKeyName(1, "");
			this.imageListLv.Images.SetKeyName(2, "");
			this.imageListLv.Images.SetKeyName(3, "");
			this.imageListLv.Images.SetKeyName(4, "");
			// 
			// imageListTable
			// 
			this.imageListTable.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTable.ImageStream")));
			this.imageListTable.TransparentColor = System.Drawing.Color.Magenta;
			this.imageListTable.Images.SetKeyName(0, "");
			this.imageListTable.Images.SetKeyName(1, "");
			this.imageListTable.Images.SetKeyName(2, "");
			this.imageListTable.Images.SetKeyName(3, "");
			this.imageListTable.Images.SetKeyName(4, "");
			this.imageListTable.Images.SetKeyName(5, "");
			// 
			// contextMenuTable
			// 
			this.contextMenuTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemTableNewOpen,
            this.menuItem18,
            this.menuItemTableCopyURL,
            this.menuItemTableCopyURLName,
            this.menuItem25,
            this.menuItemTableOpenWebBrowser,
            this.menuItem30,
            this.menuItemTableSetItaBotan,
            this.menuItemTableShowLocalRule,
            this.menuItemTableShowPicture,
            this.menuItemTableShowSettingTxt,
            this.menuItem35,
            this.menuItemTableDeleteLog});
			this.contextMenuTable.Name = "contextMenuTable";
			this.contextMenuTable.Size = new System.Drawing.Size(228, 226);
			this.contextMenuTable.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTable_Popup);
			// 
			// menuItemTableNewOpen
			// 
			this.menuItemTableNewOpen.Name = "menuItemTableNewOpen";
			this.menuItemTableNewOpen.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableNewOpen.Text = "新しいウインドウで開く(&N)";
			this.menuItemTableNewOpen.Click += new System.EventHandler(this.menuItemTableNewOpen_Click);
			// 
			// menuItem18
			// 
			this.menuItem18.Name = "menuItem18";
			this.menuItem18.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemTableCopyURL
			// 
			this.menuItemTableCopyURL.Name = "menuItemTableCopyURL";
			this.menuItemTableCopyURL.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableCopyURL.Text = "URLをコピー(&U)";
			this.menuItemTableCopyURL.Click += new System.EventHandler(this.menuItemTableCopyURL_Click);
			// 
			// menuItemTableCopyURLName
			// 
			this.menuItemTableCopyURLName.Name = "menuItemTableCopyURLName";
			this.menuItemTableCopyURLName.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableCopyURLName.Text = "URLと名前をコピー(&C)";
			this.menuItemTableCopyURLName.Click += new System.EventHandler(this.menuItemTableCopyURLName_Click);
			// 
			// menuItem25
			// 
			this.menuItem25.Name = "menuItem25";
			this.menuItem25.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemTableOpenWebBrowser
			// 
			this.menuItemTableOpenWebBrowser.Name = "menuItemTableOpenWebBrowser";
			this.menuItemTableOpenWebBrowser.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableOpenWebBrowser.Text = "Webブラウザで開く(&W)";
			this.menuItemTableOpenWebBrowser.Click += new System.EventHandler(this.menuItemTableOpenWebBrowser_Click);
			// 
			// menuItem30
			// 
			this.menuItem30.Name = "menuItem30";
			this.menuItem30.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemTableSetItaBotan
			// 
			this.menuItemTableSetItaBotan.Name = "menuItemTableSetItaBotan";
			this.menuItemTableSetItaBotan.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableSetItaBotan.Text = "板ボタンの追加と削除(&T)";
			this.menuItemTableSetItaBotan.Click += new System.EventHandler(this.menuItemTableSetItaBotan_Click);
			// 
			// menuItemTableShowLocalRule
			// 
			this.menuItemTableShowLocalRule.Name = "menuItemTableShowLocalRule";
			this.menuItemTableShowLocalRule.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableShowLocalRule.Text = "ローカルルールを表示(&L)";
			this.menuItemTableShowLocalRule.Click += new System.EventHandler(this.menuItemTableShowLocalRule_Click);
			// 
			// menuItemTableShowPicture
			// 
			this.menuItemTableShowPicture.Name = "menuItemTableShowPicture";
			this.menuItemTableShowPicture.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableShowPicture.Text = "看板を表示(&P)";
			this.menuItemTableShowPicture.Click += new System.EventHandler(this.menuItemTableShowPicture_Click);
			// 
			// menuItemTableShowSettingTxt
			// 
			this.menuItemTableShowSettingTxt.Name = "menuItemTableShowSettingTxt";
			this.menuItemTableShowSettingTxt.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableShowSettingTxt.Text = "SETTING.TXTを見る(&S)...";
			this.menuItemTableShowSettingTxt.Click += new System.EventHandler(this.menuItemTableShowSettingTxt_Click);
			// 
			// menuItem35
			// 
			this.menuItem35.Name = "menuItem35";
			this.menuItem35.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemTableDeleteLog
			// 
			this.menuItemTableDeleteLog.Name = "menuItemTableDeleteLog";
			this.menuItemTableDeleteLog.Size = new System.Drawing.Size(227, 22);
			this.menuItemTableDeleteLog.Text = "ログを削除(&D)";
			this.menuItemTableDeleteLog.Click += new System.EventHandler(this.menuItemTableDeleteLog_Click);
			// 
			// contextMenuBookmarkFolder
			// 
			this.contextMenuBookmarkFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemBookmarkOpen,
            this.menuItemBookmarkOpenIncludeSubChildren,
            this.menuItem32,
            this.menuItemBookmarkUpdateCheck,
            this.menuItemBookmarkPatrol,
            this.menuItem36,
            this.menuItemBookmarkNewFolder,
            this.menuItemBookmarkToWareHouse,
            this.menuItemBookmarkSetItabotan,
            this.menuItem41,
            this.menuItemBookmarkSort,
            this.menuItemBookmarkRename,
            this.menuItem39,
            this.menuItemBookmarkDel});
			this.contextMenuBookmarkFolder.Name = "contextMenuBookmarkFolder";
			this.contextMenuBookmarkFolder.ShowImageMargin = false;
			this.contextMenuBookmarkFolder.Size = new System.Drawing.Size(213, 248);
			this.contextMenuBookmarkFolder.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuBookmarkFolder_Popup);
			// 
			// menuItemBookmarkOpen
			// 
			this.menuItemBookmarkOpen.Name = "menuItemBookmarkOpen";
			this.menuItemBookmarkOpen.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkOpen.Text = "開く(&O)";
			this.menuItemBookmarkOpen.Click += new System.EventHandler(this.menuItemBookmarkOpen_Click);
			// 
			// menuItemBookmarkOpenIncludeSubChildren
			// 
			this.menuItemBookmarkOpenIncludeSubChildren.Name = "menuItemBookmarkOpenIncludeSubChildren";
			this.menuItemBookmarkOpenIncludeSubChildren.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkOpenIncludeSubChildren.Text = "サブフォルダも含めて開く(&L)";
			this.menuItemBookmarkOpenIncludeSubChildren.Click += new System.EventHandler(this.menuItemBookmarkOpenIncludeSubChildren_Click);
			// 
			// menuItem32
			// 
			this.menuItem32.Name = "menuItem32";
			this.menuItem32.Size = new System.Drawing.Size(209, 6);
			// 
			// menuItemBookmarkUpdateCheck
			// 
			this.menuItemBookmarkUpdateCheck.Name = "menuItemBookmarkUpdateCheck";
			this.menuItemBookmarkUpdateCheck.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkUpdateCheck.Text = "更新チェック(&C)";
			this.menuItemBookmarkUpdateCheck.Click += new System.EventHandler(this.menuItemBookmarkUpdateCheck_Click);
			// 
			// menuItemBookmarkPatrol
			// 
			this.menuItemBookmarkPatrol.Name = "menuItemBookmarkPatrol";
			this.menuItemBookmarkPatrol.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkPatrol.Text = "巡回(&P)";
			this.menuItemBookmarkPatrol.Click += new System.EventHandler(this.menuItemBookmarkPatrol_Click);
			// 
			// menuItem36
			// 
			this.menuItem36.Name = "menuItem36";
			this.menuItem36.Size = new System.Drawing.Size(209, 6);
			// 
			// menuItemBookmarkNewFolder
			// 
			this.menuItemBookmarkNewFolder.Name = "menuItemBookmarkNewFolder";
			this.menuItemBookmarkNewFolder.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkNewFolder.Text = "新規フォルダ(&F)";
			this.menuItemBookmarkNewFolder.Click += new System.EventHandler(this.menuItemBookmarkNewFolder_Click);
			// 
			// menuItemBookmarkToWareHouse
			// 
			this.menuItemBookmarkToWareHouse.Name = "menuItemBookmarkToWareHouse";
			this.menuItemBookmarkToWareHouse.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkToWareHouse.Text = "過去ログ倉庫へ移動(&W)";
			this.menuItemBookmarkToWareHouse.Click += new System.EventHandler(this.menuItemBookmarkToWareHouse_Click);
			// 
			// menuItemBookmarkSetItabotan
			// 
			this.menuItemBookmarkSetItabotan.Name = "menuItemBookmarkSetItabotan";
			this.menuItemBookmarkSetItabotan.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkSetItabotan.Text = "板ボタン(&B)";
			this.menuItemBookmarkSetItabotan.Click += new System.EventHandler(this.menuItemBookmarkSetItabotan_Click);
			// 
			// menuItem41
			// 
			this.menuItem41.Name = "menuItem41";
			this.menuItem41.Size = new System.Drawing.Size(209, 6);
			// 
			// menuItemBookmarkSort
			// 
			this.menuItemBookmarkSort.Name = "menuItemBookmarkSort";
			this.menuItemBookmarkSort.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkSort.Text = "名前順で並び替え(&S)";
			this.menuItemBookmarkSort.Click += new System.EventHandler(this.menuItemBookmarkSort_Click);
			// 
			// menuItemBookmarkRename
			// 
			this.menuItemBookmarkRename.Name = "menuItemBookmarkRename";
			this.menuItemBookmarkRename.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkRename.Text = "名前を変更(&N)";
			this.menuItemBookmarkRename.Click += new System.EventHandler(this.menuItemBookmarkRename_Click);
			// 
			// menuItem39
			// 
			this.menuItem39.Name = "menuItem39";
			this.menuItem39.Size = new System.Drawing.Size(209, 6);
			// 
			// menuItemBookmarkDel
			// 
			this.menuItemBookmarkDel.Name = "menuItemBookmarkDel";
			this.menuItemBookmarkDel.Size = new System.Drawing.Size(212, 22);
			this.menuItemBookmarkDel.Text = "削除(&D)";
			this.menuItemBookmarkDel.Click += new System.EventHandler(this.menuItemBookmarkRemove_Click);
			// 
			// contextMenuBookmarkItem
			// 
			this.contextMenuBookmarkItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemBookmarkNewOpen,
            this.menuItem46,
            this.menuItemBookmarkItemToWareHouse,
            this.menuItemBookmarkItemRename,
            this.menuItem37,
            this.menuItemBookmarkRemove});
			this.contextMenuBookmarkItem.Name = "contextMenuBookmarkItem";
			this.contextMenuBookmarkItem.Size = new System.Drawing.Size(228, 104);
			this.contextMenuBookmarkItem.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuBookmarkItem_Popup);
			// 
			// menuItemBookmarkNewOpen
			// 
			this.menuItemBookmarkNewOpen.Name = "menuItemBookmarkNewOpen";
			this.menuItemBookmarkNewOpen.Size = new System.Drawing.Size(227, 22);
			this.menuItemBookmarkNewOpen.Text = "新しいウインドウで開く(&N)";
			this.menuItemBookmarkNewOpen.Click += new System.EventHandler(this.menuItemBookmarkNewOpen_Click);
			// 
			// menuItem46
			// 
			this.menuItem46.Name = "menuItem46";
			this.menuItem46.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemBookmarkItemToWareHouse
			// 
			this.menuItemBookmarkItemToWareHouse.Name = "menuItemBookmarkItemToWareHouse";
			this.menuItemBookmarkItemToWareHouse.Size = new System.Drawing.Size(227, 22);
			this.menuItemBookmarkItemToWareHouse.Text = "過去ログ倉庫へ移動(&W)";
			this.menuItemBookmarkItemToWareHouse.Click += new System.EventHandler(this.menuItemBookmarkToWareHouse_Click);
			// 
			// menuItemBookmarkItemRename
			// 
			this.menuItemBookmarkItemRename.Name = "menuItemBookmarkItemRename";
			this.menuItemBookmarkItemRename.Size = new System.Drawing.Size(227, 22);
			this.menuItemBookmarkItemRename.Text = "名前を変更(&R)";
			this.menuItemBookmarkItemRename.Click += new System.EventHandler(this.menuItemBookmarkRename_Click);
			// 
			// menuItem37
			// 
			this.menuItem37.Name = "menuItem37";
			this.menuItem37.Size = new System.Drawing.Size(224, 6);
			// 
			// menuItemBookmarkRemove
			// 
			this.menuItemBookmarkRemove.Name = "menuItemBookmarkRemove";
			this.menuItemBookmarkRemove.Size = new System.Drawing.Size(227, 22);
			this.menuItemBookmarkRemove.Text = "削除(&D)";
			this.menuItemBookmarkRemove.Click += new System.EventHandler(this.menuItemBookmarkRemove_Click);
			// 
			// contextMenuWareHouseFolder
			// 
			this.contextMenuWareHouseFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemWareHouseOpen,
            this.menuItemWareHouseOpenIncludeSubChildren,
            this.menuItem34,
            this.menuItemWareHouseNewFolder,
            this.menuItemWareHouseSetItabotan,
            this.menuItem8,
            this.menuItemWareHouseSort,
            this.menuItemWareHouseFolderRename,
            this.menuItem44,
            this.menuItemWareHouseRemove});
			this.contextMenuWareHouseFolder.Name = "contextMenuWareHouseFolder";
			this.contextMenuWareHouseFolder.ShowImageMargin = false;
			this.contextMenuWareHouseFolder.Size = new System.Drawing.Size(213, 176);
			this.contextMenuWareHouseFolder.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuWareHouse_Popup);
			// 
			// menuItemWareHouseOpen
			// 
			this.menuItemWareHouseOpen.Name = "menuItemWareHouseOpen";
			this.menuItemWareHouseOpen.Size = new System.Drawing.Size(212, 22);
			this.menuItemWareHouseOpen.Text = "開く(&O)";
			this.menuItemWareHouseOpen.Click += new System.EventHandler(this.menuItemWareHouseOpen_Click);
			// 
			// menuItemWareHouseOpenIncludeSubChildren
			// 
			this.menuItemWareHouseOpenIncludeSubChildren.Name = "menuItemWareHouseOpenIncludeSubChildren";
			this.menuItemWareHouseOpenIncludeSubChildren.Size = new System.Drawing.Size(212, 22);
			this.menuItemWareHouseOpenIncludeSubChildren.Text = "サブフォルダも含めて開く(&L)";
			this.menuItemWareHouseOpenIncludeSubChildren.Click += new System.EventHandler(this.menuItemWareHouseOpenIncludeSubChildren_Click);
			// 
			// menuItem34
			// 
			this.menuItem34.Name = "menuItem34";
			this.menuItem34.Size = new System.Drawing.Size(209, 6);
			// 
			// menuItemWareHouseNewFolder
			// 
			this.menuItemWareHouseNewFolder.Name = "menuItemWareHouseNewFolder";
			this.menuItemWareHouseNewFolder.Size = new System.Drawing.Size(212, 22);
			this.menuItemWareHouseNewFolder.Text = "新規フォルダ(&F)";
			this.menuItemWareHouseNewFolder.Click += new System.EventHandler(this.menuItemWareHouseNewFolder_Click);
			// 
			// menuItemWareHouseSetItabotan
			// 
			this.menuItemWareHouseSetItabotan.Name = "menuItemWareHouseSetItabotan";
			this.menuItemWareHouseSetItabotan.Size = new System.Drawing.Size(212, 22);
			this.menuItemWareHouseSetItabotan.Text = "板ボタン(&B)";
			this.menuItemWareHouseSetItabotan.Click += new System.EventHandler(this.menuItemWareHouseSetItabotan_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Name = "menuItem8";
			this.menuItem8.Size = new System.Drawing.Size(209, 6);
			// 
			// menuItemWareHouseSort
			// 
			this.menuItemWareHouseSort.Name = "menuItemWareHouseSort";
			this.menuItemWareHouseSort.Size = new System.Drawing.Size(212, 22);
			this.menuItemWareHouseSort.Text = "名前順で並び替え(&S)";
			this.menuItemWareHouseSort.Click += new System.EventHandler(this.menuItemWareHouseSort_Click);
			// 
			// menuItemWareHouseFolderRename
			// 
			this.menuItemWareHouseFolderRename.Name = "menuItemWareHouseFolderRename";
			this.menuItemWareHouseFolderRename.Size = new System.Drawing.Size(212, 22);
			this.menuItemWareHouseFolderRename.Text = "名前を変更(&N)";
			this.menuItemWareHouseFolderRename.Click += new System.EventHandler(this.menuItemWareHouseFolderRename_Click);
			// 
			// menuItem44
			// 
			this.menuItem44.Name = "menuItem44";
			this.menuItem44.Size = new System.Drawing.Size(209, 6);
			// 
			// menuItemWareHouseRemove
			// 
			this.menuItemWareHouseRemove.Name = "menuItemWareHouseRemove";
			this.menuItemWareHouseRemove.Size = new System.Drawing.Size(212, 22);
			this.menuItemWareHouseRemove.Text = "削除(&D)";
			this.menuItemWareHouseRemove.Click += new System.EventHandler(this.menuItemWareHouseRemove_Click);
			// 
			// contextMenuWareHouseItem
			// 
			this.contextMenuWareHouseItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemWareHouseItem_NewOpen,
            this.menuItemWareHouseItem_Rename,
            this.menuItem42,
            this.menuItemWareHouseItem_Remove});
			this.contextMenuWareHouseItem.Name = "contextMenuWareHouseItem";
			this.contextMenuWareHouseItem.ShowImageMargin = false;
			this.contextMenuWareHouseItem.Size = new System.Drawing.Size(203, 76);
			this.contextMenuWareHouseItem.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuWareHouseItem_Popup);
			// 
			// menuItemWareHouseItem_NewOpen
			// 
			this.menuItemWareHouseItem_NewOpen.Name = "menuItemWareHouseItem_NewOpen";
			this.menuItemWareHouseItem_NewOpen.Size = new System.Drawing.Size(202, 22);
			this.menuItemWareHouseItem_NewOpen.Text = "新しいウインドウで開く(&N)";
			this.menuItemWareHouseItem_NewOpen.Click += new System.EventHandler(this.menuItemWareHouseNewOpen_Click);
			// 
			// menuItemWareHouseItem_Rename
			// 
			this.menuItemWareHouseItem_Rename.Name = "menuItemWareHouseItem_Rename";
			this.menuItemWareHouseItem_Rename.Size = new System.Drawing.Size(202, 22);
			this.menuItemWareHouseItem_Rename.Text = "名前を変更(&R)";
			this.menuItemWareHouseItem_Rename.Click += new System.EventHandler(this.menuItemWareHouseFolderRename_Click);
			// 
			// menuItem42
			// 
			this.menuItem42.Name = "menuItem42";
			this.menuItem42.Size = new System.Drawing.Size(199, 6);
			// 
			// menuItemWareHouseItem_Remove
			// 
			this.menuItemWareHouseItem_Remove.Name = "menuItemWareHouseItem_Remove";
			this.menuItemWareHouseItem_Remove.Size = new System.Drawing.Size(202, 22);
			this.menuItemWareHouseItem_Remove.Text = "削除(&D)";
			this.menuItemWareHouseItem_Remove.Click += new System.EventHandler(this.menuItemWareHouseRemove_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "All Files (*.*)|*.*|Html Files (*.html)|*.html;*.htm|Dat Files (*.dat)|*.dat;*.da" +
    "t.gz|Monalog Files (*.xml)|*.xml";
			this.openFileDialog.ReadOnlyChecked = true;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.FileName = "doc1";
			this.saveFileDialog.Filter = "All Files (*.*)|*.*|Html Files (*.html)|*.html;*.htm|Dat Files (*.dat)|*.dat;*.da" +
    "t.gz|Monalog Files (*.xml)|*.xml";
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStripNotifyIcon;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "twintail";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
			// 
			// contextMenuStripNotifyIcon
			// 
			this.contextMenuStripNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNotifyIconExit});
			this.contextMenuStripNotifyIcon.Name = "contextMenuStripNotifyIcon";
			this.contextMenuStripNotifyIcon.ShowImageMargin = false;
			this.contextMenuStripNotifyIcon.Size = new System.Drawing.Size(202, 26);
			// 
			// menuItemNotifyIconExit
			// 
			this.menuItemNotifyIconExit.Name = "menuItemNotifyIconExit";
			this.menuItemNotifyIconExit.Size = new System.Drawing.Size(201, 22);
			this.menuItemNotifyIconExit.Text = "アプリケーションを終了(&X)";
			this.menuItemNotifyIconExit.Click += new System.EventHandler(this.menuItemNotifyIconExit_Click);
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.AutoScroll = true;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.threadPanel);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitterTop);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.listPanel);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitterLeft);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.treePanel);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.statusBar);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.rebarWrapper);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(896, 527);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(896, 553);
			this.toolStripContainer1.TabIndex = 14;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.mainMenu);
			// 
			// contextMenuStripAutoReload
			// 
			this.contextMenuStripAutoReload.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAutoImageOpen});
			this.contextMenuStripAutoReload.Name = "contextMenuStripAutoReload";
			this.contextMenuStripAutoReload.Size = new System.Drawing.Size(276, 26);
			this.contextMenuStripAutoReload.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripAutoReload_Opening);
			// 
			// menuItemAutoImageOpen
			// 
			this.menuItemAutoImageOpen.Name = "menuItemAutoImageOpen";
			this.menuItemAutoImageOpen.Size = new System.Drawing.Size(275, 22);
			this.menuItemAutoImageOpen.Text = "自動で新着レスの画像URLを開く(&U)";
			this.menuItemAutoImageOpen.Click += new System.EventHandler(this.menuItemAutoImageOpen_Click);
			// 
			// Twin2IeBrowser
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(896, 553);
			this.Controls.Add(this.toolStripContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenu;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Twin2IeBrowser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Twin2IeBrowser";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Twin2IeBrowser_Closing);
			this.Load += new System.EventHandler(this.Twin2IeBrowser_Load);
			this.Shown += new System.EventHandler(this.Twin2IeBrowser_Shown);
			this.LocationChanged += new System.EventHandler(this.Twin2IeBrowser_LocationSizeChanged);
			this.SizeChanged += new System.EventHandler(this.Twin2IeBrowser_LocationSizeChanged);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Twin2IeBrowser_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Twin2IeBrowser_DragEnter);
			this.Resize += new System.EventHandler(this.Twin2IeBrowser_Resize);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.addressPanel.ResumeLayout(false);
			this.addressPanel.PerformLayout();
			this.rebarWrapper.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.contextMenuItaBotan.ResumeLayout(false);
			this.treePanel.ResumeLayout(false);
			this.tabControlTable.ResumeLayout(false);
			this.listPanel.ResumeLayout(false);
			this.contextMenuListTab.ResumeLayout(false);
			this.threadPanel.ResumeLayout(false);
			this.threadInnerPanel.ResumeLayout(false);
			this.contextMenuThreadTab.ResumeLayout(false);
			this.threadToolPanel.ResumeLayout(false);
			this.threadToolPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusProgress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusResCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusForce)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusSamba24)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusTimerCount)).EndInit();
			this.contextMenuViewChange.ResumeLayout(false);
			this.contextMenuScroll.ResumeLayout(false);
			this.contextMenuRead.ResumeLayout(false);
			this.contextMenuRes.ResumeLayout(false);
			this.contextMenuListView.ResumeLayout(false);
			this.contextMenuTable.ResumeLayout(false);
			this.contextMenuBookmarkFolder.ResumeLayout(false);
			this.contextMenuBookmarkItem.ResumeLayout(false);
			this.contextMenuWareHouseFolder.ResumeLayout(false);
			this.contextMenuWareHouseItem.ResumeLayout(false);
			this.contextMenuStripNotifyIcon.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.contextMenuStripAutoReload.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ToolStripSeparator menuItem59;
		private ToolStripSeparator menuItem9;
		private ToolStripSeparator menuItem5;
		private ToolStripSeparator menuItem13;
		private ToolStripSeparator menuItem12;
		private ToolStripSeparator menuItem4;
		private ToolStripSeparator menuItem56;
		private ToolStripSeparator menuItem17;
		private ToolStripSeparator menuItem48;
		private ToolStripSeparator menuItem51;
		private ToolStripSeparator menuItem55;
		private ToolStripSeparator menuItem23;
		private ToolStripSeparator menuItem7;
		private ToolStripSeparator menuItem27;
		private ToolStripSeparator menuItem10;
		private ToolStripSeparator menuItem2;
		private ToolStripSeparator menuItem49;
		private ToolStripSeparator menuItem14;
		private ToolStripSeparator menuItem77;
		private ToolStripSeparator menuItem38;
		private ToolStripSeparator menuItem26;
		private ToolStripSeparator menuItem20;
		private ToolStripSeparator menuItem75;
		private ToolStripSeparator menuItem58;
		private ToolStripSeparator menuItem45;
		private ToolStripSeparator menuItem60;
		private ToolStripSeparator menuItem68;
		private ToolStripSeparator menuItem70;
		private ToolStripSeparator menuItem52;
		private ToolStripSeparator menuItem31;
		private ToolStripSeparator menuItem24;
		private ToolStripSeparator menuItem62;
		private ToolStripSeparator menuItem19;
		private ToolStripSeparator menuItem11;
		private ToolStripMenuItem menuItemThreadTabCloseLeft;
		private ToolStripMenuItem menuItemThreadTabCloseRight;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem menuItemThreadTabCloseWithoutThis;
		private ToolStripMenuItem menuItemHelpOpenServerWatch2;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem menuItemThreadAutoFocus;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem menuItemListTabWithout1000Res;
		private ToolStripMenuItem menuItemListTabWithoutPastlog;
		private ToolStripMenuItem menuItemListTabWithoutKakolog;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem menuItemRemoveShiori;
		private ToolStripMenuItem miSaveSettings;
		private ToolStripMenuItem miGroup;
		private ToolStripMenuItem miGroupAdd;
		private ToolStripMenuItem miGroupEdit;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem menuItemThreadTabColoring;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripMenuItem menuItemListTabColoring;
		private ToolStripMenuItem menuItemShowSettingTxt;
		private ToolStripMenuItem menuItemTableShowSettingTxt;
		private ToolStripMenuItem menuItemThreadTabNextThreadCheck;
		private ToolStripMenuItem 履歴を削除HToolStripMenuItem;
		private ToolStripMenuItem menuItemFileClearNameHistory;
		private ToolStripMenuItem menuItemFileClearSearchHistory;
		private ToolStripSeparator toolStripMenuItem5;
		private ToolStripMenuItem menuItemClearAllHistory;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripMenuItem menuItemResCopyID;
		private StatusBarPanel statusTimerCount;
		private ToolStripMenuItem menuItemThreadTabRefresh;
		private ToolStripSeparator toolStripMenuItem6;
		private ToolBarButton toolBarButtonScrollTop;
		private ToolStripSeparator toolStripMenuItem7;
		private ToolStripMenuItem menuItemThreadTabReget;
		private ToolStripSeparator toolStripMenuItem8;
		private ToolStripMenuItem menuItemListTabRefresh;
		private ToolBarButton toolBarButton10;
		private ToolStripSeparator toolStripMenuItem9;
		private ToolStripMenuItem menuItemFileLogIndexing;
		private ToolStripSeparator toolStripMenuItem10;
		private ToolStripMenuItem menuItemViewPatrol;
		private ToolStripMenuItem menuItemPatrolHiddenPastlog;
		private ToolStripMenuItem menuItemThreadResetPastlogFlags;
		private ToolStripMenuItem miMouseGestureSetting;
		private ToolStripMenuItem toolStripMenuItemSaveImages;
		private ToolStripMenuItem menuItemListTabCopyURL;
		private ToolStripMenuItem menuItemListTabCopyNameURL;
		private ToolStripSeparator toolStripMenuItem11;
		private ToolStripMenuItem menuItemListSearchNext;
		private ToolStripSeparator toolStripMenuItem12;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripMenuItem menuItemResKokoMadeLink;
		private NotifyIcon notifyIcon1;
		private ContextMenuStrip contextMenuStripNotifyIcon;
		private ToolStripMenuItem menuItemNotifyIconExit;
		private ToolStripContainer toolStripContainer1;
		private ToolStripMenuItem menuItemFileLogDeleteImageCache;
		private ToolStripSeparator toolStripMenuItem14;
		private ToolStripSeparator toolStripMenuItem13;
		private ToolStripMenuItem menuItemSaveSelectedAllToDat;
		private ToolStripMenuItem menuItemSaveSelectedAllToHtml;
		private ToolStripMenuItem menuItemHelpOpenErrorLog;
		private ToolStripMenuItem menuItemToolOyster;
		private ToolStripMenuItem menuItemToolOysterEnable;
		private ToolStripMenuItem menuItemToolOysterDisable;
		private ToolStripSeparator toolStripMenuItem16;
		private ToolStripMenuItem menuItemEditColoringWord;
		private ToolStripMenuItem menuItemToolBe;
		private ToolStripMenuItem menuItemToolBeLogin;
		private ToolStripMenuItem menuItemToolBeLogout;
		private ToolStripMenuItem menuItemThreadTabAllOpenImageViewer;
		private ContextMenuStrip contextMenuStripAutoReload;
		private ToolStripMenuItem menuItemAutoImageOpen;
		private StatusBarPanel statusSamba24;
		private ToolStripSeparator toolStripMenuItem15;
		private ToolStripMenuItem menuItemEditSearchSubjectBotanAdd;

	}
}
