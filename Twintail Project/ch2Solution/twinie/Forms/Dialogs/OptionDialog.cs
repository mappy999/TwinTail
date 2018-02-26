// OptionDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Drawing.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using System.Net;
	using System.IO;
	using CSharpSamples;
	using Twin.Tools;
	using System.Collections.Generic;
	using System.Reflection;

	/// <summary>
	/// 環境設定ダイアログ
	/// </summary>
	public class OptionDialog : System.Windows.Forms.Form
	{
		// デザインタブの色情報を格納しておく変数
		private Settings settings;
		private Label[] labelColors;
		private Label[] labelTableColors;
		private CheckBox[] checkBoxesBold;
		private CheckBox[] checkBoxesUnderLine;
		private bool initializing;

		// コテハン設定に使用
		private IBoardTable table;
		private KotehanManager koteman;

		#region Designer Fields
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxWebBrowserPath;
		private System.Windows.Forms.Button buttonRefWebBrowserPath;
		private System.Windows.Forms.TextBox textBoxOnlineUpdateUrl;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.CheckBox checkBoxUseGzipArchive;
		private System.Windows.Forms.Button buttonRefLogFolder;
		private System.Windows.Forms.TextBox textBoxLogFolder;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private System.Windows.Forms.CheckBox checkBoxAutoClose;
		private System.Windows.Forms.CheckBox checkBoxShowCookieDialog;
		private System.Windows.Forms.NumericUpDown numericUpDownViewResCount;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox checkBoxAutoScrollOn;
		private System.Windows.Forms.CheckBox checkBoxAutoReloadOn;
		private System.Windows.Forms.TabPage tabPageAction;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboBoxPriority;
		private System.Windows.Forms.TabPage tabPageThread;
		private System.Windows.Forms.TabPage tabPageFunction;
		private System.Windows.Forms.TabPage tabPagePost;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBoxDateFormat;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkBoxSavePostHistory;
		private System.Windows.Forms.CheckBox checkBoxNextThreadChecker;
		private System.Windows.Forms.CheckBox checkBoxImageThumb;
		private System.Windows.Forms.CheckBox checkBoxVisibleNGAbone;
		private System.Windows.Forms.TextBox textBoxResRefAnchor;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.NumericUpDown numericUpDownAutoReloadInterval;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TabPage tabPagePopup;
		private System.Windows.Forms.CheckBox checkBoxExPopup;
		private System.Windows.Forms.CheckBox checkBoxImagePopup;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDownRecvBuffSize;
		private System.Windows.Forms.CheckBox checkBoxImagePopupCtrlSwitch;
		private System.Windows.Forms.TextBox textBoxPopupRegex;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.NumericUpDown numericUpDownClosedHistory;
		private System.Windows.Forms.NumericUpDown numericUpDownThumSize;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckBox checkBoxThreadKotehan;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBoxOpenStartupUrls;
		private System.Windows.Forms.CheckBox checkBoxScrollToNewRes;
		private System.Windows.Forms.CheckBox checkBoxSamba24Check;
		private System.Windows.Forms.CheckBox checkBoxMultiWriteDialog;
		private System.Windows.Forms.TabPage tabPageProxy;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.TextBox textBoxRecvProxyHost;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.NumericUpDown numericUpDownRecvProxyPort;
		private System.Windows.Forms.TextBox textBoxRecvProxyUserID;
		private System.Windows.Forms.TextBox textBoxRecvProxyPass;
		private System.Windows.Forms.CheckBox checkBoxRecvProxyCredential;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.CheckBox checkBoxSendProxyCredential;
		private System.Windows.Forms.TextBox textBoxSendProxyPass;
		private System.Windows.Forms.TextBox textBoxSendProxyUserID;
		private System.Windows.Forms.NumericUpDown numericUpDownSendProxyPort;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.TextBox textBoxSendProxyHost;
		private System.Windows.Forms.TabPage tabPageDesign;
		private System.Windows.Forms.TabControl tabControlDesign;
		private System.Windows.Forms.TabPage tabPageTableDesign;
		private System.Windows.Forms.TabPage tabPageListDesign;
		private System.Windows.Forms.TabPage tabPageThreadDesign;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Label labelColorListBack1;
		private System.Windows.Forms.Label labelColorListBack0;
		private System.Windows.Forms.Label labelColorListDef;
		private System.Windows.Forms.Label labelColorListNew;
		private System.Windows.Forms.Label labelColorListDat;
		private System.Windows.Forms.Label labelColorListUp;
		private System.Windows.Forms.Label labelColorListGot;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.FontDialog fontDialog;
		private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
		private System.Windows.Forms.TabPage tabPageDesignCommon;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.NumericUpDown numericUpDownTabSizeWidth;
		private System.Windows.Forms.NumericUpDown numericUpDownTabSizeHeight;
		private System.Windows.Forms.CheckBox checkBoxImeOn;
		private System.Windows.Forms.CheckBox checkBoxAlwaysSingleOpen;
		private System.Windows.Forms.TabPage tabPageMouse;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.ComboBox comboBoxOpenMode;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.ComboBox comboBoxListWheelClick;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.ComboBox comboBoxWheelClick;
		private System.Windows.Forms.ComboBox comboBoxDoubleClick;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.CheckBox checkBoxChainABone;
		private System.Windows.Forms.CheckBox checkBoxListColoring;
		private System.Windows.Forms.Label labelColorListRecent;
		private System.Windows.Forms.ComboBox comboBoxListFonts;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.NumericUpDown numericUpDownListFontSize;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.CheckBox checkBoxListDef_Bold;
		private System.Windows.Forms.CheckBox checkBoxListDef_UnderLine;
		private System.Windows.Forms.CheckBox checkBoxListNew_UnderLine;
		private System.Windows.Forms.CheckBox checkBoxListNew_Bold;
		private System.Windows.Forms.CheckBox checkBoxListUp_UnderLine;
		private System.Windows.Forms.CheckBox checkBoxListUp_Bold;
		private System.Windows.Forms.CheckBox checkBoxListGot_UnderLine;
		private System.Windows.Forms.CheckBox checkBoxListGot_Bold;
		private System.Windows.Forms.CheckBox checkBoxListDat_UnderLine;
		private System.Windows.Forms.CheckBox checkBoxListDat_Bold;
		private System.Windows.Forms.CheckBox checkBoxListRecent_UnderLine;
		private System.Windows.Forms.CheckBox checkBoxListRecent_Bold;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.NumericUpDown numericUpDownPopupInterval;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Button buttonAuthenticateOption;
		private System.Windows.Forms.Button buttonClearProxy;
		private System.Windows.Forms.CheckBox checkBoxTableHideIcon;
		private System.Windows.Forms.CheckBox checkBoxTableColoring;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.NumericUpDown numericUpDownTableFontSize;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.ComboBox comboBoxTableFonts;
		private System.Windows.Forms.Label labelColorTableBoard;
		private System.Windows.Forms.Label labelColorTableCate;
		private System.Windows.Forms.CheckBox checkBoxPostDlgMinimize;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.RadioButton radioButtonBrwsStd;
		private System.Windows.Forms.RadioButton radioButtonBrwsSimpl;
		private System.Windows.Forms.RadioButton radioButtonBrwsRef;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.TrackBar trackBarAutoReload;
		private System.Windows.Forms.TrackBar trackBarHistory;
		private System.Windows.Forms.CheckBox checkBoxOrigPopup;
		private System.Windows.Forms.CheckBox checkBoxUrlPopup;
		private System.Windows.Forms.CheckBox checkBoxUrlPopupCtrlKeySwitch;
		private System.Windows.Forms.CheckBox checkBoxRecycleOverThread;
		private System.Windows.Forms.RadioButton radioButtonViewLimitResCount;
		private System.Windows.Forms.RadioButton radioButtonViewAllRes;
		private System.Windows.Forms.CheckBox checkBoxThreadAlwaysNewTab;
		private System.Windows.Forms.RadioButton radioButtonThreadNotPackage;
		private System.Windows.Forms.RadioButton radioButtonThreadPackageReception;
		private System.Windows.Forms.CheckBox checkBoxListAlwaysNewTab;
		private System.Windows.Forms.RadioButton radioButtonListNotPackage;
		private System.Windows.Forms.RadioButton radioButtonListPackageReception;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Label label44;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TabPage tabPageKotehan;
		private System.Windows.Forms.ComboBox comboBoxBoardList;
		private System.Windows.Forms.Button buttonDeleteKote;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.TextBox textBoxPostName;
		private System.Windows.Forms.Label label48;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.Button buttonRegistKote;
		private System.Windows.Forms.TextBox textBoxPostEmail;
		private System.Windows.Forms.ComboBox comboBoxSkins;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.RadioButton radioButtonTabAutoSize;
		private System.Windows.Forms.RadioButton radioButtonTabFixedSize;
		private System.Windows.Forms.Label label50;
		private System.Windows.Forms.Label label51;
		private System.Windows.Forms.ListView listViewColorList;
		private System.Windows.Forms.Label label52;
		private System.Windows.Forms.ColumnHeader columnHeaderName;
		private System.Windows.Forms.ColumnHeader columnHeaderType;
		private System.Windows.Forms.ColumnHeader columnHeaderForeColor;
		private System.Windows.Forms.ColumnHeader columnHeaderBackColor;
		private System.Windows.Forms.Button buttonAddColor;
		private System.Windows.Forms.Button buttonRemoveColor;
		private System.Windows.Forms.RadioButton radioButtonTabFillRight;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Label label54;
		private System.Windows.Forms.TreeView treeViewKotehan;
		private System.Windows.Forms.Button buttonTripPreview;
		private System.Windows.Forms.GroupBox groupBoxKotehan;
		private System.Windows.Forms.Label label57;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.RadioButton radioButtonThreadTabFillRight;
		private System.Windows.Forms.Label label55;
		private System.Windows.Forms.Label label56;
		private System.Windows.Forms.RadioButton radioButtonThreadTabFixed;
		private System.Windows.Forms.RadioButton radioButtonThreadTabAutoSize;
		private System.Windows.Forms.NumericUpDown numericUpDownThreadTabSizeHeight;
		private System.Windows.Forms.NumericUpDown numericUpDownThreadTabSizeWidth;
		private System.Windows.Forms.Label label62;
		private System.Windows.Forms.Label label63;
		private System.Windows.Forms.NumericUpDown numericUpDownPopupMaxWidth;
		private System.Windows.Forms.NumericUpDown numericUpDownPopupMaxHeight;
		private System.Windows.Forms.Label label64;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.TabPage tabPageSound;
		private System.Windows.Forms.PropertyGrid propertyGridSound;
		private System.Windows.Forms.ComboBox comboBoxPopupPos;
		private System.Windows.Forms.Label label58;
		private System.Windows.Forms.NumericUpDown numericUpDownImagePopupHeight;
		private System.Windows.Forms.Label label59;
		private System.Windows.Forms.NumericUpDown numericUpDownImagePopupWidth;
		private System.Windows.Forms.Label label60;
		private System.Windows.Forms.CheckBox checkBoxNamePopup;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.GroupBox groupBox12;
		private System.Windows.Forms.GroupBox groupBox13;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.ComboBox comboBoxForceValueOf;
		private System.Windows.Forms.CheckBox checkBoxImageViewer;
		private System.Windows.Forms.CheckBox checkBoxAutoFillThread;
		private System.Windows.Forms.CheckBox checkBoxClickedHide;
		private System.Windows.Forms.TextBox textBoxDmdm;
		private System.Windows.Forms.TextBox textBoxMdmd;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.Label label61;
		private System.Windows.Forms.Label label65;
		private System.Windows.Forms.Label label66;
		private System.Windows.Forms.CheckBox checkBoxSendBeID;
		private System.Windows.Forms.TabPage tabPageWrite;
		private System.Windows.Forms.Label label67;
		private System.Windows.Forms.NumericUpDown numericUpDownWriteFontSize;
		private System.Windows.Forms.Label label68;
		private System.Windows.Forms.ComboBox comboBoxWriteFonts;
		private System.Windows.Forms.Label label69;
		private CheckBox checkBoxThumbnailIsLightMode;
		private GroupBox groupBox14;
		private CheckBox checkBoxColoringBackReference;
		private Label label71;
		private NumericUpDown numericUpDownImageSizeLimit;
		private Label label70;
		private CheckBox checkBoxAutoNGRes;
		private GroupBox groupBox15;
		private Label label72;
		private RadioButton radioButtonTabCloseAfterSelectionLeft;
		private RadioButton radioButtonTabCloseAfterSelectionRight;
		private CheckBox checkBoxClickedHideResPopup;
		private Label label73;
		private ComboBox comboBoxTabAppearance;
		private CheckBox checkBoxIsHighlightActiveTab;
		private Label labelHighlightActiveColor;
		private CheckBox checkBoxAutoReloadCheckOnly;
		private CheckBox checkBoxCloseMsgBox;
		private CheckBox checkBoxConnectionLimit;
		private ComboBox comboBoxNewTabPos;
		private Label label74;
		private CheckBox checkBoxTabSelectedAfterReload;
		private TextBox textBoxAddWriteSection;
		private Label label75;
		private CheckBox checkBoxAutoReloadAverage;
		private CheckBox checkBoxTaskTray;
		private CheckBox checkBoxEnsureVisibleBoard;
		private Label label76;
		private CheckBox checkBoxUseVisualStyle;
		private CheckBox checkBoxWheelScroll;
		private CheckBox checkBoxNG924;
		private LinkLabel linkLabel1;
		private CheckBox checkBoxListForce_Underline;
		private CheckBox checkBoxListForce_Bold;
		private Label labelColorListForcible;
		private CheckBox checkBoxAutoOpenImage;
		private CheckBox checkBoxHighLevelMatch;
		private System.ComponentModel.IContainer components = null;

		#endregion

		/// <summary>
		/// OptionDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="settings"></param>
		public OptionDialog(IBoardTable table, KotehanManager koteman, Settings settings)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//

			
			this.initializing = true;
			this.table = table;
			this.settings = settings;
			this.koteman = koteman;
			this.colorDialog.CustomColors = settings.Dialogs.ColorDialogCustomColors;

			// 使用可能なスキンを列挙
			foreach (string path in Directory.GetDirectories(Settings.BaseSkinFolderPath))
				comboBoxSkins.Items.Add(Path.GetFileName(path));

			Set(settings);

			radioButtonListNotPackage.Enabled = false;
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
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionDialog));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.checkBoxUseVisualStyle = new System.Windows.Forms.CheckBox();
			this.comboBoxForceValueOf = new System.Windows.Forms.ComboBox();
			this.label40 = new System.Windows.Forms.Label();
			this.radioButtonBrwsRef = new System.Windows.Forms.RadioButton();
			this.radioButtonBrwsSimpl = new System.Windows.Forms.RadioButton();
			this.radioButtonBrwsStd = new System.Windows.Forms.RadioButton();
			this.label7 = new System.Windows.Forms.Label();
			this.textBoxResRefAnchor = new System.Windows.Forms.TextBox();
			this.textBoxDateFormat = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.buttonRefLogFolder = new System.Windows.Forms.Button();
			this.checkBoxUseGzipArchive = new System.Windows.Forms.CheckBox();
			this.textBoxLogFolder = new System.Windows.Forms.TextBox();
			this.textBoxWebBrowserPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxOnlineUpdateUrl = new System.Windows.Forms.TextBox();
			this.buttonRefWebBrowserPath = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPageFunction = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxTaskTray = new System.Windows.Forms.CheckBox();
			this.checkBoxImageViewer = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
			this.checkBoxOpenStartupUrls = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.checkBoxNamePopup = new System.Windows.Forms.CheckBox();
			this.checkBoxNextThreadChecker = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label71 = new System.Windows.Forms.Label();
			this.numericUpDownImageSizeLimit = new System.Windows.Forms.NumericUpDown();
			this.label70 = new System.Windows.Forms.Label();
			this.checkBoxThumbnailIsLightMode = new System.Windows.Forms.CheckBox();
			this.label66 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.numericUpDownThumSize = new System.Windows.Forms.NumericUpDown();
			this.checkBoxImageThumb = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBoxNG924 = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoNGRes = new System.Windows.Forms.CheckBox();
			this.checkBoxChainABone = new System.Windows.Forms.CheckBox();
			this.checkBoxVisibleNGAbone = new System.Windows.Forms.CheckBox();
			this.tabPageAction = new System.Windows.Forms.TabPage();
			this.groupBox15 = new System.Windows.Forms.GroupBox();
			this.comboBoxNewTabPos = new System.Windows.Forms.ComboBox();
			this.label74 = new System.Windows.Forms.Label();
			this.checkBoxCloseMsgBox = new System.Windows.Forms.CheckBox();
			this.checkBoxConnectionLimit = new System.Windows.Forms.CheckBox();
			this.label72 = new System.Windows.Forms.Label();
			this.radioButtonTabCloseAfterSelectionLeft = new System.Windows.Forms.RadioButton();
			this.radioButtonTabCloseAfterSelectionRight = new System.Windows.Forms.RadioButton();
			this.comboBoxPriority = new System.Windows.Forms.ComboBox();
			this.numericUpDownRecvBuffSize = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.groupBox13 = new System.Windows.Forms.GroupBox();
			this.checkBoxAutoFillThread = new System.Windows.Forms.CheckBox();
			this.checkBoxThreadAlwaysNewTab = new System.Windows.Forms.CheckBox();
			this.label44 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.radioButtonThreadNotPackage = new System.Windows.Forms.RadioButton();
			this.radioButtonThreadPackageReception = new System.Windows.Forms.RadioButton();
			this.checkBoxRecycleOverThread = new System.Windows.Forms.CheckBox();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.label41 = new System.Windows.Forms.Label();
			this.checkBoxListAlwaysNewTab = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.radioButtonListNotPackage = new System.Windows.Forms.RadioButton();
			this.radioButtonListPackageReception = new System.Windows.Forms.RadioButton();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.checkBoxEnsureVisibleBoard = new System.Windows.Forms.CheckBox();
			this.checkBoxAlwaysSingleOpen = new System.Windows.Forms.CheckBox();
			this.tabPageMouse = new System.Windows.Forms.TabPage();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.label39 = new System.Windows.Forms.Label();
			this.comboBoxOpenMode = new System.Windows.Forms.ComboBox();
			this.label30 = new System.Windows.Forms.Label();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.label29 = new System.Windows.Forms.Label();
			this.comboBoxListWheelClick = new System.Windows.Forms.ComboBox();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.checkBoxWheelScroll = new System.Windows.Forms.CheckBox();
			this.comboBoxWheelClick = new System.Windows.Forms.ComboBox();
			this.comboBoxDoubleClick = new System.Windows.Forms.ComboBox();
			this.label27 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.tabPageThread = new System.Windows.Forms.TabPage();
			this.checkBoxAutoReloadAverage = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoReloadCheckOnly = new System.Windows.Forms.CheckBox();
			this.groupBox14 = new System.Windows.Forms.GroupBox();
			this.checkBoxColoringBackReference = new System.Windows.Forms.CheckBox();
			this.label46 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.checkBoxAutoOpenImage = new System.Windows.Forms.CheckBox();
			this.checkBoxTabSelectedAfterReload = new System.Windows.Forms.CheckBox();
			this.checkBoxScrollToNewRes = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoReloadOn = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoScrollOn = new System.Windows.Forms.CheckBox();
			this.trackBarHistory = new System.Windows.Forms.TrackBar();
			this.trackBarAutoReload = new System.Windows.Forms.TrackBar();
			this.radioButtonViewLimitResCount = new System.Windows.Forms.RadioButton();
			this.radioButtonViewAllRes = new System.Windows.Forms.RadioButton();
			this.numericUpDownClosedHistory = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.numericUpDownAutoReloadInterval = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.numericUpDownViewResCount = new System.Windows.Forms.NumericUpDown();
			this.tabPagePopup = new System.Windows.Forms.TabPage();
			this.label76 = new System.Windows.Forms.Label();
			this.checkBoxClickedHideResPopup = new System.Windows.Forms.CheckBox();
			this.checkBoxClickedHide = new System.Windows.Forms.CheckBox();
			this.numericUpDownImagePopupHeight = new System.Windows.Forms.NumericUpDown();
			this.label59 = new System.Windows.Forms.Label();
			this.numericUpDownImagePopupWidth = new System.Windows.Forms.NumericUpDown();
			this.label60 = new System.Windows.Forms.Label();
			this.label58 = new System.Windows.Forms.Label();
			this.comboBoxPopupPos = new System.Windows.Forms.ComboBox();
			this.numericUpDownPopupMaxHeight = new System.Windows.Forms.NumericUpDown();
			this.label64 = new System.Windows.Forms.Label();
			this.numericUpDownPopupMaxWidth = new System.Windows.Forms.NumericUpDown();
			this.label63 = new System.Windows.Forms.Label();
			this.label62 = new System.Windows.Forms.Label();
			this.checkBoxUrlPopupCtrlKeySwitch = new System.Windows.Forms.CheckBox();
			this.checkBoxUrlPopup = new System.Windows.Forms.CheckBox();
			this.checkBoxOrigPopup = new System.Windows.Forms.CheckBox();
			this.label37 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.numericUpDownPopupInterval = new System.Windows.Forms.NumericUpDown();
			this.textBoxPopupRegex = new System.Windows.Forms.TextBox();
			this.checkBoxExPopup = new System.Windows.Forms.CheckBox();
			this.checkBoxImagePopupCtrlSwitch = new System.Windows.Forms.CheckBox();
			this.checkBoxImagePopup = new System.Windows.Forms.CheckBox();
			this.tabPagePost = new System.Windows.Forms.TabPage();
			this.textBoxAddWriteSection = new System.Windows.Forms.TextBox();
			this.label75 = new System.Windows.Forms.Label();
			this.label65 = new System.Windows.Forms.Label();
			this.label61 = new System.Windows.Forms.Label();
			this.label45 = new System.Windows.Forms.Label();
			this.textBoxMdmd = new System.Windows.Forms.TextBox();
			this.textBoxDmdm = new System.Windows.Forms.TextBox();
			this.label38 = new System.Windows.Forms.Label();
			this.checkBoxPostDlgMinimize = new System.Windows.Forms.CheckBox();
			this.checkBoxImeOn = new System.Windows.Forms.CheckBox();
			this.checkBoxMultiWriteDialog = new System.Windows.Forms.CheckBox();
			this.checkBoxSamba24Check = new System.Windows.Forms.CheckBox();
			this.checkBoxThreadKotehan = new System.Windows.Forms.CheckBox();
			this.label20 = new System.Windows.Forms.Label();
			this.checkBoxSavePostHistory = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoClose = new System.Windows.Forms.CheckBox();
			this.checkBoxShowCookieDialog = new System.Windows.Forms.CheckBox();
			this.tabPageKotehan = new System.Windows.Forms.TabPage();
			this.buttonTripPreview = new System.Windows.Forms.Button();
			this.treeViewKotehan = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.label54 = new System.Windows.Forms.Label();
			this.comboBoxBoardList = new System.Windows.Forms.ComboBox();
			this.label47 = new System.Windows.Forms.Label();
			this.groupBoxKotehan = new System.Windows.Forms.GroupBox();
			this.checkBoxSendBeID = new System.Windows.Forms.CheckBox();
			this.label48 = new System.Windows.Forms.Label();
			this.textBoxPostEmail = new System.Windows.Forms.TextBox();
			this.label49 = new System.Windows.Forms.Label();
			this.textBoxPostName = new System.Windows.Forms.TextBox();
			this.buttonRegistKote = new System.Windows.Forms.Button();
			this.buttonDeleteKote = new System.Windows.Forms.Button();
			this.tabPageProxy = new System.Windows.Forms.TabPage();
			this.buttonClearProxy = new System.Windows.Forms.Button();
			this.buttonAuthenticateOption = new System.Windows.Forms.Button();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label21 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.checkBoxRecvProxyCredential = new System.Windows.Forms.CheckBox();
			this.textBoxRecvProxyPass = new System.Windows.Forms.TextBox();
			this.textBoxRecvProxyUserID = new System.Windows.Forms.TextBox();
			this.numericUpDownRecvProxyPort = new System.Windows.Forms.NumericUpDown();
			this.label18 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.textBoxRecvProxyHost = new System.Windows.Forms.TextBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.checkBoxSendProxyCredential = new System.Windows.Forms.CheckBox();
			this.textBoxSendProxyPass = new System.Windows.Forms.TextBox();
			this.textBoxSendProxyUserID = new System.Windows.Forms.TextBox();
			this.numericUpDownSendProxyPort = new System.Windows.Forms.NumericUpDown();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.textBoxSendProxyHost = new System.Windows.Forms.TextBox();
			this.tabPageSound = new System.Windows.Forms.TabPage();
			this.propertyGridSound = new System.Windows.Forms.PropertyGrid();
			this.tabPageDesign = new System.Windows.Forms.TabPage();
			this.tabControlDesign = new System.Windows.Forms.TabControl();
			this.tabPageDesignCommon = new System.Windows.Forms.TabPage();
			this.checkBoxIsHighlightActiveTab = new System.Windows.Forms.CheckBox();
			this.labelHighlightActiveColor = new System.Windows.Forms.Label();
			this.label73 = new System.Windows.Forms.Label();
			this.comboBoxTabAppearance = new System.Windows.Forms.ComboBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.radioButtonThreadTabFillRight = new System.Windows.Forms.RadioButton();
			this.label55 = new System.Windows.Forms.Label();
			this.label56 = new System.Windows.Forms.Label();
			this.radioButtonThreadTabFixed = new System.Windows.Forms.RadioButton();
			this.radioButtonThreadTabAutoSize = new System.Windows.Forms.RadioButton();
			this.numericUpDownThreadTabSizeHeight = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownThreadTabSizeWidth = new System.Windows.Forms.NumericUpDown();
			this.panel3 = new System.Windows.Forms.Panel();
			this.radioButtonTabFillRight = new System.Windows.Forms.RadioButton();
			this.label51 = new System.Windows.Forms.Label();
			this.numericUpDownTabSizeWidth = new System.Windows.Forms.NumericUpDown();
			this.radioButtonTabFixedSize = new System.Windows.Forms.RadioButton();
			this.numericUpDownTabSizeHeight = new System.Windows.Forms.NumericUpDown();
			this.radioButtonTabAutoSize = new System.Windows.Forms.RadioButton();
			this.label50 = new System.Windows.Forms.Label();
			this.label57 = new System.Windows.Forms.Label();
			this.label53 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.tabPageTableDesign = new System.Windows.Forms.TabPage();
			this.buttonRemoveColor = new System.Windows.Forms.Button();
			this.buttonAddColor = new System.Windows.Forms.Button();
			this.label52 = new System.Windows.Forms.Label();
			this.listViewColorList = new System.Windows.Forms.ListView();
			this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderForeColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderBackColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label42 = new System.Windows.Forms.Label();
			this.numericUpDownTableFontSize = new System.Windows.Forms.NumericUpDown();
			this.label43 = new System.Windows.Forms.Label();
			this.comboBoxTableFonts = new System.Windows.Forms.ComboBox();
			this.labelColorTableBoard = new System.Windows.Forms.Label();
			this.labelColorTableCate = new System.Windows.Forms.Label();
			this.checkBoxTableColoring = new System.Windows.Forms.CheckBox();
			this.checkBoxTableHideIcon = new System.Windows.Forms.CheckBox();
			this.tabPageListDesign = new System.Windows.Forms.TabPage();
			this.checkBoxListForce_Underline = new System.Windows.Forms.CheckBox();
			this.checkBoxListForce_Bold = new System.Windows.Forms.CheckBox();
			this.labelColorListForcible = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.checkBoxListRecent_UnderLine = new System.Windows.Forms.CheckBox();
			this.checkBoxListRecent_Bold = new System.Windows.Forms.CheckBox();
			this.checkBoxListDat_UnderLine = new System.Windows.Forms.CheckBox();
			this.checkBoxListDat_Bold = new System.Windows.Forms.CheckBox();
			this.checkBoxListGot_UnderLine = new System.Windows.Forms.CheckBox();
			this.checkBoxListGot_Bold = new System.Windows.Forms.CheckBox();
			this.checkBoxListUp_UnderLine = new System.Windows.Forms.CheckBox();
			this.checkBoxListUp_Bold = new System.Windows.Forms.CheckBox();
			this.checkBoxListNew_UnderLine = new System.Windows.Forms.CheckBox();
			this.checkBoxListNew_Bold = new System.Windows.Forms.CheckBox();
			this.checkBoxListDef_UnderLine = new System.Windows.Forms.CheckBox();
			this.checkBoxListDef_Bold = new System.Windows.Forms.CheckBox();
			this.label32 = new System.Windows.Forms.Label();
			this.numericUpDownListFontSize = new System.Windows.Forms.NumericUpDown();
			this.label31 = new System.Windows.Forms.Label();
			this.comboBoxListFonts = new System.Windows.Forms.ComboBox();
			this.labelColorListRecent = new System.Windows.Forms.Label();
			this.checkBoxListColoring = new System.Windows.Forms.CheckBox();
			this.labelColorListGot = new System.Windows.Forms.Label();
			this.labelColorListUp = new System.Windows.Forms.Label();
			this.labelColorListDat = new System.Windows.Forms.Label();
			this.labelColorListDef = new System.Windows.Forms.Label();
			this.labelColorListNew = new System.Windows.Forms.Label();
			this.labelColorListBack0 = new System.Windows.Forms.Label();
			this.labelColorListBack1 = new System.Windows.Forms.Label();
			this.tabPageThreadDesign = new System.Windows.Forms.TabPage();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.label12 = new System.Windows.Forms.Label();
			this.comboBoxSkins = new System.Windows.Forms.ComboBox();
			this.tabPageWrite = new System.Windows.Forms.TabPage();
			this.label69 = new System.Windows.Forms.Label();
			this.label67 = new System.Windows.Forms.Label();
			this.numericUpDownWriteFontSize = new System.Windows.Forms.NumericUpDown();
			this.label68 = new System.Windows.Forms.Label();
			this.comboBoxWriteFonts = new System.Windows.Forms.ComboBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonReset = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.fontDialog = new System.Windows.Forms.FontDialog();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.checkBoxHighLevelMatch = new System.Windows.Forms.CheckBox();
			this.tabControl.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.tabPageFunction.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownImageSizeLimit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThumSize)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.tabPageAction.SuspendLayout();
			this.groupBox15.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecvBuffSize)).BeginInit();
			this.groupBox13.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.tabPageMouse.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.tabPageThread.SuspendLayout();
			this.groupBox14.SuspendLayout();
			this.groupBox10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarHistory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarAutoReload)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownClosedHistory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoReloadInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownViewResCount)).BeginInit();
			this.tabPagePopup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownImagePopupHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownImagePopupWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopupMaxHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopupMaxWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopupInterval)).BeginInit();
			this.tabPagePost.SuspendLayout();
			this.tabPageKotehan.SuspendLayout();
			this.groupBoxKotehan.SuspendLayout();
			this.tabPageProxy.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecvProxyPort)).BeginInit();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownSendProxyPort)).BeginInit();
			this.tabPageSound.SuspendLayout();
			this.tabPageDesign.SuspendLayout();
			this.tabControlDesign.SuspendLayout();
			this.tabPageDesignCommon.SuspendLayout();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreadTabSizeHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreadTabSizeWidth)).BeginInit();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabSizeWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabSizeHeight)).BeginInit();
			this.tabPageTableDesign.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableFontSize)).BeginInit();
			this.tabPageListDesign.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownListFontSize)).BeginInit();
			this.tabPageThreadDesign.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.tabPageWrite.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteFontSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageGeneral);
			this.tabControl.Controls.Add(this.tabPageFunction);
			this.tabControl.Controls.Add(this.tabPageAction);
			this.tabControl.Controls.Add(this.tabPageMouse);
			this.tabControl.Controls.Add(this.tabPageThread);
			this.tabControl.Controls.Add(this.tabPagePopup);
			this.tabControl.Controls.Add(this.tabPagePost);
			this.tabControl.Controls.Add(this.tabPageKotehan);
			this.tabControl.Controls.Add(this.tabPageProxy);
			this.tabControl.Controls.Add(this.tabPageSound);
			this.tabControl.Controls.Add(this.tabPageDesign);
			this.tabControl.ImageList = this.imageList;
			this.tabControl.ItemSize = new System.Drawing.Size(90, 23);
			this.tabControl.Location = new System.Drawing.Point(2, 0);
			this.tabControl.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(575, 426);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl.TabIndex = 0;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.Controls.Add(this.linkLabel1);
			this.tabPageGeneral.Controls.Add(this.checkBoxUseVisualStyle);
			this.tabPageGeneral.Controls.Add(this.comboBoxForceValueOf);
			this.tabPageGeneral.Controls.Add(this.label40);
			this.tabPageGeneral.Controls.Add(this.radioButtonBrwsRef);
			this.tabPageGeneral.Controls.Add(this.radioButtonBrwsSimpl);
			this.tabPageGeneral.Controls.Add(this.radioButtonBrwsStd);
			this.tabPageGeneral.Controls.Add(this.label7);
			this.tabPageGeneral.Controls.Add(this.textBoxResRefAnchor);
			this.tabPageGeneral.Controls.Add(this.textBoxDateFormat);
			this.tabPageGeneral.Controls.Add(this.label6);
			this.tabPageGeneral.Controls.Add(this.label5);
			this.tabPageGeneral.Controls.Add(this.buttonRefLogFolder);
			this.tabPageGeneral.Controls.Add(this.checkBoxUseGzipArchive);
			this.tabPageGeneral.Controls.Add(this.textBoxLogFolder);
			this.tabPageGeneral.Controls.Add(this.textBoxWebBrowserPath);
			this.tabPageGeneral.Controls.Add(this.label2);
			this.tabPageGeneral.Controls.Add(this.textBoxOnlineUpdateUrl);
			this.tabPageGeneral.Controls.Add(this.buttonRefWebBrowserPath);
			this.tabPageGeneral.Controls.Add(this.label3);
			this.tabPageGeneral.ImageIndex = 6;
			this.tabPageGeneral.Location = new System.Drawing.Point(4, 50);
			this.tabPageGeneral.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.Size = new System.Drawing.Size(567, 372);
			this.tabPageGeneral.TabIndex = 0;
			this.tabPageGeneral.Text = "基本設定";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(184, 139);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(170, 12);
			this.linkLabel1.TabIndex = 19;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "*フォルダ変更に関しての注意事項!";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// checkBoxUseVisualStyle
			// 
			this.checkBoxUseVisualStyle.AutoSize = true;
			this.checkBoxUseVisualStyle.Location = new System.Drawing.Point(107, 304);
			this.checkBoxUseVisualStyle.Name = "checkBoxUseVisualStyle";
			this.checkBoxUseVisualStyle.Size = new System.Drawing.Size(134, 16);
			this.checkBoxUseVisualStyle.TabIndex = 18;
			this.checkBoxUseVisualStyle.Text = "VisualStyleを適用する";
			this.checkBoxUseVisualStyle.UseVisualStyleBackColor = true;
			// 
			// comboBoxForceValueOf
			// 
			this.comboBoxForceValueOf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxForceValueOf.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxForceValueOf.Location = new System.Drawing.Point(424, 235);
			this.comboBoxForceValueOf.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxForceValueOf.Name = "comboBoxForceValueOf";
			this.comboBoxForceValueOf.Size = new System.Drawing.Size(60, 20);
			this.comboBoxForceValueOf.TabIndex = 17;
			this.toolTip.SetToolTip(this.comboBoxForceValueOf, "勢いを計算するときに使用する時間の単位を設定してください");
			// 
			// label40
			// 
			this.label40.AutoSize = true;
			this.label40.Location = new System.Drawing.Point(257, 237);
			this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(137, 12);
			this.label40.TabIndex = 16;
			this.label40.Text = "勢いの計算に使用する単位";
			// 
			// radioButtonBrwsRef
			// 
			this.radioButtonBrwsRef.AutoSize = true;
			this.radioButtonBrwsRef.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonBrwsRef.Location = new System.Drawing.Point(327, 35);
			this.radioButtonBrwsRef.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonBrwsRef.Name = "radioButtonBrwsRef";
			this.radioButtonBrwsRef.Size = new System.Drawing.Size(99, 17);
			this.radioButtonBrwsRef.TabIndex = 3;
			this.radioButtonBrwsRef.Text = "指定のブラウザ";
			this.toolTip.SetToolTip(this.radioButtonBrwsRef, "使用するブラウザを指定します");
			this.radioButtonBrwsRef.CheckedChanged += new System.EventHandler(this.radioButtonSetBrowser_CheckedChanged);
			// 
			// radioButtonBrwsSimpl
			// 
			this.radioButtonBrwsSimpl.AutoSize = true;
			this.radioButtonBrwsSimpl.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonBrwsSimpl.Location = new System.Drawing.Point(210, 35);
			this.radioButtonBrwsSimpl.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonBrwsSimpl.Name = "radioButtonBrwsSimpl";
			this.radioButtonBrwsSimpl.Size = new System.Drawing.Size(113, 17);
			this.radioButtonBrwsSimpl.TabIndex = 2;
			this.radioButtonBrwsSimpl.Text = "簡易内部ブラウザ";
			this.toolTip.SetToolTip(this.radioButtonBrwsSimpl, "ついんてーるに内蔵されている簡易タブブラウザを使用します");
			this.radioButtonBrwsSimpl.CheckedChanged += new System.EventHandler(this.radioButtonSetBrowser_CheckedChanged);
			// 
			// radioButtonBrwsStd
			// 
			this.radioButtonBrwsStd.AutoSize = true;
			this.radioButtonBrwsStd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonBrwsStd.Location = new System.Drawing.Point(107, 35);
			this.radioButtonBrwsStd.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonBrwsStd.Name = "radioButtonBrwsStd";
			this.radioButtonBrwsStd.Size = new System.Drawing.Size(99, 17);
			this.radioButtonBrwsStd.TabIndex = 1;
			this.radioButtonBrwsStd.Text = "標準のブラウザ";
			this.toolTip.SetToolTip(this.radioButtonBrwsStd, "システムに設定されている標準のブラウザを使用します");
			this.radioButtonBrwsStd.CheckedChanged += new System.EventHandler(this.radioButtonSetBrowser_CheckedChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.BackColor = System.Drawing.SystemColors.Control;
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label7.Location = new System.Drawing.Point(316, 266);
			this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(83, 12);
			this.label7.TabIndex = 14;
			this.label7.Text = "レス参照文字列";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textBoxResRefAnchor
			// 
			this.textBoxResRefAnchor.Location = new System.Drawing.Point(423, 264);
			this.textBoxResRefAnchor.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxResRefAnchor.Name = "textBoxResRefAnchor";
			this.textBoxResRefAnchor.Size = new System.Drawing.Size(60, 19);
			this.textBoxResRefAnchor.TabIndex = 15;
			this.toolTip.SetToolTip(this.textBoxResRefAnchor, "“これにレス”時に付加される参照文字列です");
			// 
			// textBoxDateFormat
			// 
			this.textBoxDateFormat.Location = new System.Drawing.Point(107, 247);
			this.textBoxDateFormat.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxDateFormat.Name = "textBoxDateFormat";
			this.textBoxDateFormat.Size = new System.Drawing.Size(127, 19);
			this.textBoxDateFormat.TabIndex = 13;
			this.toolTip.SetToolTip(this.textBoxDateFormat, "スレッド一覧に表示される日付の書式を設定してください");
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.SystemColors.Control;
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label6.Location = new System.Drawing.Point(73, 223);
			this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(63, 12);
			this.label6.TabIndex = 12;
			this.label6.Text = "日付の書式";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.SystemColors.Control;
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label5.Location = new System.Drawing.Point(75, 139);
			this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(104, 12);
			this.label5.TabIndex = 8;
			this.label5.Text = "ログの保存先フォルダ";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonRefLogFolder
			// 
			this.buttonRefLogFolder.AutoSize = true;
			this.buttonRefLogFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRefLogFolder.Location = new System.Drawing.Point(384, 159);
			this.buttonRefLogFolder.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRefLogFolder.Name = "buttonRefLogFolder";
			this.buttonRefLogFolder.Size = new System.Drawing.Size(71, 24);
			this.buttonRefLogFolder.TabIndex = 10;
			this.buttonRefLogFolder.Text = "参照...";
			this.buttonRefLogFolder.Click += new System.EventHandler(this.buttonRefLogFolder_Click);
			// 
			// checkBoxUseGzipArchive
			// 
			this.checkBoxUseGzipArchive.AutoSize = true;
			this.checkBoxUseGzipArchive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxUseGzipArchive.Location = new System.Drawing.Point(254, 187);
			this.checkBoxUseGzipArchive.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxUseGzipArchive.Name = "checkBoxUseGzipArchive";
			this.checkBoxUseGzipArchive.Size = new System.Drawing.Size(145, 17);
			this.checkBoxUseGzipArchive.TabIndex = 11;
			this.checkBoxUseGzipArchive.Text = "ログをGzip圧縮して保存";
			this.toolTip.SetToolTip(this.checkBoxUseGzipArchive, "ログを圧縮するとディスクスペースを節約できます");
			// 
			// textBoxLogFolder
			// 
			this.errorProvider.SetIconAlignment(this.textBoxLogFolder, System.Windows.Forms.ErrorIconAlignment.BottomLeft);
			this.textBoxLogFolder.Location = new System.Drawing.Point(107, 161);
			this.textBoxLogFolder.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxLogFolder.Name = "textBoxLogFolder";
			this.textBoxLogFolder.Size = new System.Drawing.Size(274, 19);
			this.textBoxLogFolder.TabIndex = 9;
			this.textBoxLogFolder.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLogFolder_Validating);
			this.textBoxLogFolder.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// textBoxWebBrowserPath
			// 
			this.textBoxWebBrowserPath.Location = new System.Drawing.Point(107, 56);
			this.textBoxWebBrowserPath.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxWebBrowserPath.Name = "textBoxWebBrowserPath";
			this.textBoxWebBrowserPath.Size = new System.Drawing.Size(274, 19);
			this.textBoxWebBrowserPath.TabIndex = 4;
			this.textBoxWebBrowserPath.Text = "null";
			this.textBoxWebBrowserPath.TextChanged += new System.EventHandler(this.textBoxBrowser_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.SystemColors.Control;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label2.Location = new System.Drawing.Point(73, 21);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "使用するブラウザ";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textBoxOnlineUpdateUrl
			// 
			this.textBoxOnlineUpdateUrl.Location = new System.Drawing.Point(107, 107);
			this.textBoxOnlineUpdateUrl.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxOnlineUpdateUrl.Name = "textBoxOnlineUpdateUrl";
			this.textBoxOnlineUpdateUrl.Size = new System.Drawing.Size(274, 19);
			this.textBoxOnlineUpdateUrl.TabIndex = 7;
			this.toolTip.SetToolTip(this.textBoxOnlineUpdateUrl, "板一覧をオンラインで更新するためのURLを指定してください");
			// 
			// buttonRefWebBrowserPath
			// 
			this.buttonRefWebBrowserPath.AutoSize = true;
			this.buttonRefWebBrowserPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRefWebBrowserPath.Location = new System.Drawing.Point(384, 54);
			this.buttonRefWebBrowserPath.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRefWebBrowserPath.Name = "buttonRefWebBrowserPath";
			this.buttonRefWebBrowserPath.Size = new System.Drawing.Size(71, 24);
			this.buttonRefWebBrowserPath.TabIndex = 5;
			this.buttonRefWebBrowserPath.Text = "参照...";
			this.buttonRefWebBrowserPath.Click += new System.EventHandler(this.buttonRefWebBrowserPath_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label3.Location = new System.Drawing.Point(73, 85);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(119, 12);
			this.label3.TabIndex = 6;
			this.label3.Text = "板一覧の更新先のURL";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPageFunction
			// 
			this.tabPageFunction.Controls.Add(this.groupBox1);
			this.tabPageFunction.Controls.Add(this.groupBox4);
			this.tabPageFunction.Controls.Add(this.groupBox3);
			this.tabPageFunction.Controls.Add(this.groupBox2);
			this.tabPageFunction.ImageIndex = 10;
			this.tabPageFunction.Location = new System.Drawing.Point(4, 50);
			this.tabPageFunction.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageFunction.Name = "tabPageFunction";
			this.tabPageFunction.Size = new System.Drawing.Size(567, 372);
			this.tabPageFunction.TabIndex = 6;
			this.tabPageFunction.Text = "機能";
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.checkBoxTaskTray);
			this.groupBox1.Controls.Add(this.checkBoxImageViewer);
			this.groupBox1.Controls.Add(this.checkBoxAutoUpdate);
			this.groupBox1.Controls.Add(this.checkBoxOpenStartupUrls);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(69, 4);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(429, 91);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "全般";
			// 
			// checkBoxTaskTray
			// 
			this.checkBoxTaskTray.AutoSize = true;
			this.checkBoxTaskTray.Location = new System.Drawing.Point(13, 58);
			this.checkBoxTaskTray.Name = "checkBoxTaskTray";
			this.checkBoxTaskTray.Size = new System.Drawing.Size(173, 16);
			this.checkBoxTaskTray.TabIndex = 3;
			this.checkBoxTaskTray.Text = "最小化時にタスクトレイに入れる";
			this.checkBoxTaskTray.UseVisualStyleBackColor = true;
			// 
			// checkBoxImageViewer
			// 
			this.checkBoxImageViewer.AutoSize = true;
			this.checkBoxImageViewer.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxImageViewer.Location = new System.Drawing.Point(206, 16);
			this.checkBoxImageViewer.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxImageViewer.Name = "checkBoxImageViewer";
			this.checkBoxImageViewer.Size = new System.Drawing.Size(141, 17);
			this.checkBoxImageViewer.TabIndex = 2;
			this.checkBoxImageViewer.Text = "画像ビューアを使用する";
			// 
			// checkBoxAutoUpdate
			// 
			this.checkBoxAutoUpdate.AutoSize = true;
			this.checkBoxAutoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAutoUpdate.Location = new System.Drawing.Point(13, 36);
			this.checkBoxAutoUpdate.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
			this.checkBoxAutoUpdate.Size = new System.Drawing.Size(145, 17);
			this.checkBoxAutoUpdate.TabIndex = 1;
			this.checkBoxAutoUpdate.Text = "起動時に最新版の確認";
			this.toolTip.SetToolTip(this.checkBoxAutoUpdate, "起動時に最新のバージョンを確認します");
			// 
			// checkBoxOpenStartupUrls
			// 
			this.checkBoxOpenStartupUrls.AutoSize = true;
			this.checkBoxOpenStartupUrls.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxOpenStartupUrls.Location = new System.Drawing.Point(13, 16);
			this.checkBoxOpenStartupUrls.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxOpenStartupUrls.Name = "checkBoxOpenStartupUrls";
			this.checkBoxOpenStartupUrls.Size = new System.Drawing.Size(178, 17);
			this.checkBoxOpenStartupUrls.TabIndex = 0;
			this.checkBoxOpenStartupUrls.Text = "起動時に終了前の状態を復元";
			this.toolTip.SetToolTip(this.checkBoxOpenStartupUrls, "起動時に前回終了時に開いていたスレッドを開き直します");
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.checkBoxHighLevelMatch);
			this.groupBox4.Controls.Add(this.checkBoxNamePopup);
			this.groupBox4.Controls.Add(this.checkBoxNextThreadChecker);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(69, 99);
			this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox4.Size = new System.Drawing.Size(428, 68);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "スレッド";
			// 
			// checkBoxNamePopup
			// 
			this.checkBoxNamePopup.AutoSize = true;
			this.checkBoxNamePopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxNamePopup.Location = new System.Drawing.Point(205, 22);
			this.checkBoxNamePopup.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxNamePopup.Name = "checkBoxNamePopup";
			this.checkBoxNamePopup.Size = new System.Drawing.Size(117, 17);
			this.checkBoxNamePopup.TabIndex = 1;
			this.checkBoxNamePopup.Text = "名前欄ポップアップ";
			// 
			// checkBoxNextThreadChecker
			// 
			this.checkBoxNextThreadChecker.AutoSize = true;
			this.checkBoxNextThreadChecker.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxNextThreadChecker.Location = new System.Drawing.Point(13, 22);
			this.checkBoxNextThreadChecker.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxNextThreadChecker.Name = "checkBoxNextThreadChecker";
			this.checkBoxNextThreadChecker.Size = new System.Drawing.Size(108, 17);
			this.checkBoxNextThreadChecker.TabIndex = 0;
			this.checkBoxNextThreadChecker.Text = "次スレ案内機能";
			this.toolTip.SetToolTip(this.checkBoxNextThreadChecker, "レスが1000を越えたときに次スレを探し一覧表示します");
			this.checkBoxNextThreadChecker.CheckedChanged += new System.EventHandler(this.checkBoxNextThreadChecker_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.AutoSize = true;
			this.groupBox3.Controls.Add(this.label71);
			this.groupBox3.Controls.Add(this.numericUpDownImageSizeLimit);
			this.groupBox3.Controls.Add(this.label70);
			this.groupBox3.Controls.Add(this.checkBoxThumbnailIsLightMode);
			this.groupBox3.Controls.Add(this.label66);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.numericUpDownThumSize);
			this.groupBox3.Controls.Add(this.checkBoxImageThumb);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(70, 265);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox3.Size = new System.Drawing.Size(428, 97);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "スキン出力";
			// 
			// label71
			// 
			this.label71.AutoSize = true;
			this.label71.Location = new System.Drawing.Point(271, 61);
			this.label71.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label71.Name = "label71";
			this.label71.Size = new System.Drawing.Size(20, 12);
			this.label71.TabIndex = 7;
			this.label71.Text = "KB";
			// 
			// numericUpDownImageSizeLimit
			// 
			this.numericUpDownImageSizeLimit.AutoSize = true;
			this.numericUpDownImageSizeLimit.Enabled = false;
			this.numericUpDownImageSizeLimit.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownImageSizeLimit.Location = new System.Drawing.Point(204, 57);
			this.numericUpDownImageSizeLimit.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownImageSizeLimit.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
			this.numericUpDownImageSizeLimit.Name = "numericUpDownImageSizeLimit";
			this.numericUpDownImageSizeLimit.Size = new System.Drawing.Size(67, 19);
			this.numericUpDownImageSizeLimit.TabIndex = 6;
			this.toolTip.SetToolTip(this.numericUpDownImageSizeLimit, "指定した画像サイズを超えるデータは読み込まないようにします");
			this.numericUpDownImageSizeLimit.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			// 
			// label70
			// 
			this.label70.AutoSize = true;
			this.label70.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label70.Location = new System.Drawing.Point(43, 59);
			this.label70.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label70.Name = "label70";
			this.label70.Size = new System.Drawing.Size(149, 12);
			this.label70.TabIndex = 5;
			this.label70.Text = "読み込む画像サイズの上限値";
			// 
			// checkBoxThumbnailIsLightMode
			// 
			this.checkBoxThumbnailIsLightMode.AutoSize = true;
			this.checkBoxThumbnailIsLightMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxThumbnailIsLightMode.Location = new System.Drawing.Point(29, 40);
			this.checkBoxThumbnailIsLightMode.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxThumbnailIsLightMode.Name = "checkBoxThumbnailIsLightMode";
			this.checkBoxThumbnailIsLightMode.Size = new System.Drawing.Size(120, 17);
			this.checkBoxThumbnailIsLightMode.TabIndex = 4;
			this.checkBoxThumbnailIsLightMode.Text = "低負荷・メモリ節約";
			this.toolTip.SetToolTip(this.checkBoxThumbnailIsLightMode, "一度サムネイルをローカルにキャッシュしてから表示するので低負荷でメモリもあまり使用しません。");
			this.checkBoxThumbnailIsLightMode.UseVisualStyleBackColor = true;
			this.checkBoxThumbnailIsLightMode.CheckedChanged += new System.EventHandler(this.checkBoxThumbnailIsLightMode_CheckedChanged);
			// 
			// label66
			// 
			this.label66.AutoSize = true;
			this.label66.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label66.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.label66.Location = new System.Drawing.Point(231, 20);
			this.label66.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label66.Name = "label66";
			this.label66.Size = new System.Drawing.Size(117, 12);
			this.label66.TabIndex = 3;
			this.label66.Text = "※動作が重いので注意";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label13.Location = new System.Drawing.Point(189, 20);
			this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(29, 12);
			this.label13.TabIndex = 2;
			this.label13.Text = "pixel";
			// 
			// numericUpDownThumSize
			// 
			this.numericUpDownThumSize.AutoSize = true;
			this.numericUpDownThumSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownThumSize.Location = new System.Drawing.Point(122, 16);
			this.numericUpDownThumSize.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownThumSize.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.numericUpDownThumSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownThumSize.Name = "numericUpDownThumSize";
			this.numericUpDownThumSize.Size = new System.Drawing.Size(60, 19);
			this.numericUpDownThumSize.TabIndex = 1;
			this.numericUpDownThumSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownThumSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericUpDownThumSize.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownThumSize.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// checkBoxImageThumb
			// 
			this.checkBoxImageThumb.AutoSize = true;
			this.checkBoxImageThumb.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxImageThumb.Location = new System.Drawing.Point(13, 20);
			this.checkBoxImageThumb.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxImageThumb.Name = "checkBoxImageThumb";
			this.checkBoxImageThumb.Size = new System.Drawing.Size(103, 17);
			this.checkBoxImageThumb.TabIndex = 0;
			this.checkBoxImageThumb.Text = "画像サムネイル";
			this.toolTip.SetToolTip(this.checkBoxImageThumb, "スレッド内の画像へのリンクをｻﾑﾈｲﾙ表示するかどうかの設定です。生の画像データを表示するので、動作が重く不安定になる場合もあります。");
			this.checkBoxImageThumb.CheckedChanged += new System.EventHandler(this.checkBoxImageThumb_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBoxNG924);
			this.groupBox2.Controls.Add(this.checkBoxAutoNGRes);
			this.groupBox2.Controls.Add(this.checkBoxChainABone);
			this.groupBox2.Controls.Add(this.checkBoxVisibleNGAbone);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(70, 171);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox2.Size = new System.Drawing.Size(427, 85);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "NGワード";
			// 
			// checkBoxNG924
			// 
			this.checkBoxNG924.AutoSize = true;
			this.checkBoxNG924.Location = new System.Drawing.Point(13, 64);
			this.checkBoxNG924.Name = "checkBoxNG924";
			this.checkBoxNG924.Size = new System.Drawing.Size(261, 16);
			this.checkBoxNG924.TabIndex = 3;
			this.checkBoxNG924.Text = "dat番号が924で始まるスレッドを一覧に表示しない";
			this.checkBoxNG924.UseVisualStyleBackColor = true;
			// 
			// checkBoxAutoNGRes
			// 
			this.checkBoxAutoNGRes.AutoSize = true;
			this.checkBoxAutoNGRes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAutoNGRes.Location = new System.Drawing.Point(13, 42);
			this.checkBoxAutoNGRes.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAutoNGRes.Name = "checkBoxAutoNGRes";
			this.checkBoxAutoNGRes.Size = new System.Drawing.Size(295, 17);
			this.checkBoxAutoNGRes.TabIndex = 2;
			this.checkBoxAutoNGRes.Text = "NGワードを書き込んだ投稿者のIDを自動でNGIDに追加";
			this.checkBoxAutoNGRes.UseVisualStyleBackColor = true;
			// 
			// checkBoxChainABone
			// 
			this.checkBoxChainABone.AutoSize = true;
			this.checkBoxChainABone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxChainABone.Location = new System.Drawing.Point(126, 21);
			this.checkBoxChainABone.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxChainABone.Name = "checkBoxChainABone";
			this.checkBoxChainABone.Size = new System.Drawing.Size(94, 17);
			this.checkBoxChainABone.TabIndex = 1;
			this.checkBoxChainABone.Text = "連鎖あぼーん";
			// 
			// checkBoxVisibleNGAbone
			// 
			this.checkBoxVisibleNGAbone.AutoSize = true;
			this.checkBoxVisibleNGAbone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxVisibleNGAbone.Location = new System.Drawing.Point(13, 21);
			this.checkBoxVisibleNGAbone.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxVisibleNGAbone.Name = "checkBoxVisibleNGAbone";
			this.checkBoxVisibleNGAbone.Size = new System.Drawing.Size(94, 17);
			this.checkBoxVisibleNGAbone.TabIndex = 0;
			this.checkBoxVisibleNGAbone.Text = "可視あぼーん";
			this.toolTip.SetToolTip(this.checkBoxVisibleNGAbone, "NGあぼーん時にあぼーんしたことを表示するかどうかの設定です");
			// 
			// tabPageAction
			// 
			this.tabPageAction.Controls.Add(this.groupBox15);
			this.tabPageAction.Controls.Add(this.groupBox13);
			this.tabPageAction.Controls.Add(this.groupBox12);
			this.tabPageAction.Controls.Add(this.groupBox11);
			this.tabPageAction.ImageIndex = 9;
			this.tabPageAction.Location = new System.Drawing.Point(4, 50);
			this.tabPageAction.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageAction.Name = "tabPageAction";
			this.tabPageAction.Size = new System.Drawing.Size(567, 372);
			this.tabPageAction.TabIndex = 4;
			this.tabPageAction.Text = "基本動作";
			// 
			// groupBox15
			// 
			this.groupBox15.AutoSize = true;
			this.groupBox15.Controls.Add(this.comboBoxNewTabPos);
			this.groupBox15.Controls.Add(this.label74);
			this.groupBox15.Controls.Add(this.checkBoxCloseMsgBox);
			this.groupBox15.Controls.Add(this.checkBoxConnectionLimit);
			this.groupBox15.Controls.Add(this.label72);
			this.groupBox15.Controls.Add(this.radioButtonTabCloseAfterSelectionLeft);
			this.groupBox15.Controls.Add(this.radioButtonTabCloseAfterSelectionRight);
			this.groupBox15.Controls.Add(this.comboBoxPriority);
			this.groupBox15.Controls.Add(this.numericUpDownRecvBuffSize);
			this.groupBox15.Controls.Add(this.label4);
			this.groupBox15.Controls.Add(this.label1);
			this.groupBox15.Controls.Add(this.label14);
			this.groupBox15.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox15.Location = new System.Drawing.Point(50, 221);
			this.groupBox15.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox15.Name = "groupBox15";
			this.groupBox15.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox15.Size = new System.Drawing.Size(448, 140);
			this.groupBox15.TabIndex = 23;
			this.groupBox15.TabStop = false;
			this.groupBox15.Text = "スレッド一覧、スレッドの共通設定";
			// 
			// comboBoxNewTabPos
			// 
			this.comboBoxNewTabPos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxNewTabPos.FormattingEnabled = true;
			this.comboBoxNewTabPos.Location = new System.Drawing.Point(111, 103);
			this.comboBoxNewTabPos.Name = "comboBoxNewTabPos";
			this.comboBoxNewTabPos.Size = new System.Drawing.Size(130, 20);
			this.comboBoxNewTabPos.TabIndex = 20;
			// 
			// label74
			// 
			this.label74.AutoSize = true;
			this.label74.Location = new System.Drawing.Point(5, 108);
			this.label74.Name = "label74";
			this.label74.Size = new System.Drawing.Size(104, 12);
			this.label74.TabIndex = 19;
			this.label74.Text = "新規タブの追加位置";
			// 
			// checkBoxCloseMsgBox
			// 
			this.checkBoxCloseMsgBox.AutoSize = true;
			this.checkBoxCloseMsgBox.Location = new System.Drawing.Point(228, 74);
			this.checkBoxCloseMsgBox.Name = "checkBoxCloseMsgBox";
			this.checkBoxCloseMsgBox.Size = new System.Drawing.Size(164, 16);
			this.checkBoxCloseMsgBox.TabIndex = 18;
			this.checkBoxCloseMsgBox.Text = "複数のタブを閉じるときに確認";
			this.checkBoxCloseMsgBox.UseVisualStyleBackColor = true;
			// 
			// checkBoxConnectionLimit
			// 
			this.checkBoxConnectionLimit.AutoSize = true;
			this.checkBoxConnectionLimit.Location = new System.Drawing.Point(228, 53);
			this.checkBoxConnectionLimit.Name = "checkBoxConnectionLimit";
			this.checkBoxConnectionLimit.Size = new System.Drawing.Size(215, 16);
			this.checkBoxConnectionLimit.TabIndex = 3;
			this.checkBoxConnectionLimit.Text = "接続を1つに制限(次回起動時から有効)";
			this.checkBoxConnectionLimit.UseVisualStyleBackColor = true;
			// 
			// label72
			// 
			this.label72.AutoSize = true;
			this.label72.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label72.Location = new System.Drawing.Point(19, 24);
			this.label72.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label72.Name = "label72";
			this.label72.Size = new System.Drawing.Size(112, 12);
			this.label72.TabIndex = 2;
			this.label72.Text = "タブを閉じたときの動作";
			// 
			// radioButtonTabCloseAfterSelectionLeft
			// 
			this.radioButtonTabCloseAfterSelectionLeft.AutoSize = true;
			this.radioButtonTabCloseAfterSelectionLeft.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonTabCloseAfterSelectionLeft.Location = new System.Drawing.Point(151, 23);
			this.radioButtonTabCloseAfterSelectionLeft.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonTabCloseAfterSelectionLeft.Name = "radioButtonTabCloseAfterSelectionLeft";
			this.radioButtonTabCloseAfterSelectionLeft.Size = new System.Drawing.Size(101, 17);
			this.radioButtonTabCloseAfterSelectionLeft.TabIndex = 1;
			this.radioButtonTabCloseAfterSelectionLeft.TabStop = true;
			this.radioButtonTabCloseAfterSelectionLeft.Text = "左のタブを選択";
			this.radioButtonTabCloseAfterSelectionLeft.UseVisualStyleBackColor = true;
			// 
			// radioButtonTabCloseAfterSelectionRight
			// 
			this.radioButtonTabCloseAfterSelectionRight.AutoSize = true;
			this.radioButtonTabCloseAfterSelectionRight.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonTabCloseAfterSelectionRight.Location = new System.Drawing.Point(273, 23);
			this.radioButtonTabCloseAfterSelectionRight.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonTabCloseAfterSelectionRight.Name = "radioButtonTabCloseAfterSelectionRight";
			this.radioButtonTabCloseAfterSelectionRight.Size = new System.Drawing.Size(101, 17);
			this.radioButtonTabCloseAfterSelectionRight.TabIndex = 0;
			this.radioButtonTabCloseAfterSelectionRight.TabStop = true;
			this.radioButtonTabCloseAfterSelectionRight.Text = "右のタブを選択";
			this.radioButtonTabCloseAfterSelectionRight.UseVisualStyleBackColor = true;
			// 
			// comboBoxPriority
			// 
			this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxPriority.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxPriority.Location = new System.Drawing.Point(109, 44);
			this.comboBoxPriority.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxPriority.Name = "comboBoxPriority";
			this.comboBoxPriority.Size = new System.Drawing.Size(110, 20);
			this.comboBoxPriority.TabIndex = 14;
			// 
			// numericUpDownRecvBuffSize
			// 
			this.numericUpDownRecvBuffSize.AutoSize = true;
			this.numericUpDownRecvBuffSize.Increment = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.numericUpDownRecvBuffSize.Location = new System.Drawing.Point(109, 76);
			this.numericUpDownRecvBuffSize.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownRecvBuffSize.Maximum = new decimal(new int[] {
            512000,
            0,
            0,
            0});
			this.numericUpDownRecvBuffSize.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.numericUpDownRecvBuffSize.Name = "numericUpDownRecvBuffSize";
			this.numericUpDownRecvBuffSize.Size = new System.Drawing.Size(75, 19);
			this.numericUpDownRecvBuffSize.TabIndex = 16;
			this.numericUpDownRecvBuffSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownRecvBuffSize.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.numericUpDownRecvBuffSize.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownRecvBuffSize.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.SystemColors.Control;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(17, 50);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 12);
			this.label4.TabIndex = 13;
			this.label4.Text = "プロセスの優先度";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label1.Location = new System.Drawing.Point(17, 78);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 12);
			this.label1.TabIndex = 15;
			this.label1.Text = "受信バッファサイズ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label14.Location = new System.Drawing.Point(192, 83);
			this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(27, 12);
			this.label14.TabIndex = 17;
			this.label14.Text = "byte";
			// 
			// groupBox13
			// 
			this.groupBox13.AutoSize = true;
			this.groupBox13.Controls.Add(this.checkBoxAutoFillThread);
			this.groupBox13.Controls.Add(this.checkBoxThreadAlwaysNewTab);
			this.groupBox13.Controls.Add(this.label44);
			this.groupBox13.Controls.Add(this.panel2);
			this.groupBox13.Controls.Add(this.checkBoxRecycleOverThread);
			this.groupBox13.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox13.Location = new System.Drawing.Point(50, 124);
			this.groupBox13.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox13.Name = "groupBox13";
			this.groupBox13.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox13.Size = new System.Drawing.Size(448, 93);
			this.groupBox13.TabIndex = 22;
			this.groupBox13.TabStop = false;
			this.groupBox13.Text = "スレッド";
			// 
			// checkBoxAutoFillThread
			// 
			this.checkBoxAutoFillThread.AutoSize = true;
			this.checkBoxAutoFillThread.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAutoFillThread.Location = new System.Drawing.Point(21, 60);
			this.checkBoxAutoFillThread.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAutoFillThread.Name = "checkBoxAutoFillThread";
			this.checkBoxAutoFillThread.Size = new System.Drawing.Size(189, 17);
			this.checkBoxAutoFillThread.TabIndex = 20;
			this.checkBoxAutoFillThread.Text = "スレッドを開いたら自動で拡大する";
			// 
			// checkBoxThreadAlwaysNewTab
			// 
			this.checkBoxThreadAlwaysNewTab.AutoSize = true;
			this.checkBoxThreadAlwaysNewTab.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxThreadAlwaysNewTab.Location = new System.Drawing.Point(21, 16);
			this.checkBoxThreadAlwaysNewTab.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxThreadAlwaysNewTab.Name = "checkBoxThreadAlwaysNewTab";
			this.checkBoxThreadAlwaysNewTab.Size = new System.Drawing.Size(127, 17);
			this.checkBoxThreadAlwaysNewTab.TabIndex = 8;
			this.checkBoxThreadAlwaysNewTab.Text = "常に新しいタブで開く";
			// 
			// label44
			// 
			this.label44.AutoSize = true;
			this.label44.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label44.Location = new System.Drawing.Point(235, 17);
			this.label44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(57, 12);
			this.label44.TabIndex = 10;
			this.label44.Text = "レンダリング";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.radioButtonThreadNotPackage);
			this.panel2.Controls.Add(this.radioButtonThreadPackageReception);
			this.panel2.Location = new System.Drawing.Point(328, 16);
			this.panel2.Margin = new System.Windows.Forms.Padding(2);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(46, 32);
			this.panel2.TabIndex = 19;
			// 
			// radioButtonThreadNotPackage
			// 
			this.radioButtonThreadNotPackage.AutoSize = true;
			this.radioButtonThreadNotPackage.Checked = true;
			this.radioButtonThreadNotPackage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonThreadNotPackage.Location = new System.Drawing.Point(0, 0);
			this.radioButtonThreadNotPackage.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonThreadNotPackage.Name = "radioButtonThreadNotPackage";
			this.radioButtonThreadNotPackage.Size = new System.Drawing.Size(53, 17);
			this.radioButtonThreadNotPackage.TabIndex = 11;
			this.radioButtonThreadNotPackage.TabStop = true;
			this.radioButtonThreadNotPackage.Text = "逐次";
			this.toolTip.SetToolTip(this.radioButtonThreadNotPackage, "ナローバンドな人用です");
			this.radioButtonThreadNotPackage.Click += new System.EventHandler(this.radioButtonRendering_Click);
			// 
			// radioButtonThreadPackageReception
			// 
			this.radioButtonThreadPackageReception.AutoSize = true;
			this.radioButtonThreadPackageReception.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonThreadPackageReception.Location = new System.Drawing.Point(0, 16);
			this.radioButtonThreadPackageReception.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonThreadPackageReception.Name = "radioButtonThreadPackageReception";
			this.radioButtonThreadPackageReception.Size = new System.Drawing.Size(53, 17);
			this.radioButtonThreadPackageReception.TabIndex = 12;
			this.radioButtonThreadPackageReception.Text = "一括";
			this.toolTip.SetToolTip(this.radioButtonThreadPackageReception, "ブロードバンドな人用です");
			this.radioButtonThreadPackageReception.Click += new System.EventHandler(this.radioButtonRendering_Click);
			// 
			// checkBoxRecycleOverThread
			// 
			this.checkBoxRecycleOverThread.AutoSize = true;
			this.checkBoxRecycleOverThread.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxRecycleOverThread.Location = new System.Drawing.Point(21, 44);
			this.checkBoxRecycleOverThread.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxRecycleOverThread.Name = "checkBoxRecycleOverThread";
			this.checkBoxRecycleOverThread.Size = new System.Drawing.Size(251, 17);
			this.checkBoxRecycleOverThread.TabIndex = 9;
			this.checkBoxRecycleOverThread.Text = "1000ストッパー済みのスレは新規タブを開かない";
			// 
			// groupBox12
			// 
			this.groupBox12.AutoSize = true;
			this.groupBox12.Controls.Add(this.label41);
			this.groupBox12.Controls.Add(this.checkBoxListAlwaysNewTab);
			this.groupBox12.Controls.Add(this.panel1);
			this.groupBox12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox12.Location = new System.Drawing.Point(50, 59);
			this.groupBox12.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox12.Size = new System.Drawing.Size(448, 64);
			this.groupBox12.TabIndex = 21;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "スレッド一覧";
			// 
			// label41
			// 
			this.label41.AutoSize = true;
			this.label41.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label41.Location = new System.Drawing.Point(235, 17);
			this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(57, 12);
			this.label41.TabIndex = 4;
			this.label41.Text = "レンダリング";
			// 
			// checkBoxListAlwaysNewTab
			// 
			this.checkBoxListAlwaysNewTab.AutoSize = true;
			this.checkBoxListAlwaysNewTab.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListAlwaysNewTab.Location = new System.Drawing.Point(21, 20);
			this.checkBoxListAlwaysNewTab.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListAlwaysNewTab.Name = "checkBoxListAlwaysNewTab";
			this.checkBoxListAlwaysNewTab.Size = new System.Drawing.Size(127, 17);
			this.checkBoxListAlwaysNewTab.TabIndex = 3;
			this.checkBoxListAlwaysNewTab.Text = "常に新しいタブで開く";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.radioButtonListNotPackage);
			this.panel1.Controls.Add(this.radioButtonListPackageReception);
			this.panel1.Location = new System.Drawing.Point(328, 16);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(50, 32);
			this.panel1.TabIndex = 18;
			// 
			// radioButtonListNotPackage
			// 
			this.radioButtonListNotPackage.AutoSize = true;
			this.radioButtonListNotPackage.Checked = true;
			this.radioButtonListNotPackage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonListNotPackage.Location = new System.Drawing.Point(0, 0);
			this.radioButtonListNotPackage.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonListNotPackage.Name = "radioButtonListNotPackage";
			this.radioButtonListNotPackage.Size = new System.Drawing.Size(53, 17);
			this.radioButtonListNotPackage.TabIndex = 5;
			this.radioButtonListNotPackage.TabStop = true;
			this.radioButtonListNotPackage.Text = "逐次";
			this.toolTip.SetToolTip(this.radioButtonListNotPackage, "ナローバンドな人用です");
			this.radioButtonListNotPackage.Click += new System.EventHandler(this.radioButtonRendering_Click);
			// 
			// radioButtonListPackageReception
			// 
			this.radioButtonListPackageReception.AutoSize = true;
			this.radioButtonListPackageReception.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonListPackageReception.Location = new System.Drawing.Point(0, 16);
			this.radioButtonListPackageReception.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonListPackageReception.Name = "radioButtonListPackageReception";
			this.radioButtonListPackageReception.Size = new System.Drawing.Size(53, 17);
			this.radioButtonListPackageReception.TabIndex = 6;
			this.radioButtonListPackageReception.Text = "一括";
			this.toolTip.SetToolTip(this.radioButtonListPackageReception, "ブロードバンドな人用です");
			this.radioButtonListPackageReception.Click += new System.EventHandler(this.radioButtonRendering_Click);
			// 
			// groupBox11
			// 
			this.groupBox11.AutoSize = true;
			this.groupBox11.Controls.Add(this.checkBoxEnsureVisibleBoard);
			this.groupBox11.Controls.Add(this.checkBoxAlwaysSingleOpen);
			this.groupBox11.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox11.Location = new System.Drawing.Point(50, 6);
			this.groupBox11.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox11.Size = new System.Drawing.Size(448, 49);
			this.groupBox11.TabIndex = 20;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "板一覧";
			// 
			// checkBoxEnsureVisibleBoard
			// 
			this.checkBoxEnsureVisibleBoard.AutoSize = true;
			this.checkBoxEnsureVisibleBoard.Location = new System.Drawing.Point(173, 16);
			this.checkBoxEnsureVisibleBoard.Name = "checkBoxEnsureVisibleBoard";
			this.checkBoxEnsureVisibleBoard.Size = new System.Drawing.Size(200, 16);
			this.checkBoxEnsureVisibleBoard.TabIndex = 2;
			this.checkBoxEnsureVisibleBoard.Text = "開かれた板を板一覧で選択表示する";
			this.checkBoxEnsureVisibleBoard.UseVisualStyleBackColor = true;
			// 
			// checkBoxAlwaysSingleOpen
			// 
			this.checkBoxAlwaysSingleOpen.AutoSize = true;
			this.checkBoxAlwaysSingleOpen.Enabled = false;
			this.checkBoxAlwaysSingleOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAlwaysSingleOpen.Location = new System.Drawing.Point(21, 16);
			this.checkBoxAlwaysSingleOpen.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAlwaysSingleOpen.Name = "checkBoxAlwaysSingleOpen";
			this.checkBoxAlwaysSingleOpen.Size = new System.Drawing.Size(151, 17);
			this.checkBoxAlwaysSingleOpen.TabIndex = 1;
			this.checkBoxAlwaysSingleOpen.Text = "カテゴリを１つしか開かない";
			// 
			// tabPageMouse
			// 
			this.tabPageMouse.Controls.Add(this.groupBox9);
			this.tabPageMouse.Controls.Add(this.groupBox8);
			this.tabPageMouse.Controls.Add(this.groupBox7);
			this.tabPageMouse.ImageIndex = 12;
			this.tabPageMouse.Location = new System.Drawing.Point(4, 50);
			this.tabPageMouse.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageMouse.Name = "tabPageMouse";
			this.tabPageMouse.Size = new System.Drawing.Size(567, 372);
			this.tabPageMouse.TabIndex = 10;
			this.tabPageMouse.Text = "マウス動作";
			// 
			// groupBox9
			// 
			this.groupBox9.AutoSize = true;
			this.groupBox9.Controls.Add(this.label39);
			this.groupBox9.Controls.Add(this.comboBoxOpenMode);
			this.groupBox9.Controls.Add(this.label30);
			this.groupBox9.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox9.Location = new System.Drawing.Point(58, 21);
			this.groupBox9.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox9.Size = new System.Drawing.Size(366, 68);
			this.groupBox9.TabIndex = 2;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "カテゴリやスレを開く方法";
			// 
			// label39
			// 
			this.label39.AutoSize = true;
			this.label39.BackColor = System.Drawing.SystemColors.Control;
			this.label39.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label39.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label39.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.label39.Location = new System.Drawing.Point(8, 16);
			this.label39.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(141, 11);
			this.label39.TabIndex = 2;
			this.label39.Text = "次回起動時から有効になります";
			// 
			// comboBoxOpenMode
			// 
			this.comboBoxOpenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxOpenMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxOpenMode.Location = new System.Drawing.Point(152, 28);
			this.comboBoxOpenMode.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxOpenMode.Name = "comboBoxOpenMode";
			this.comboBoxOpenMode.Size = new System.Drawing.Size(106, 20);
			this.comboBoxOpenMode.TabIndex = 1;
			this.comboBoxOpenMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpenMode_SelectedIndexChanged);
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label30.Location = new System.Drawing.Point(8, 32);
			this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(80, 12);
			this.label30.TabIndex = 0;
			this.label30.Text = "選択してください";
			// 
			// groupBox8
			// 
			this.groupBox8.AutoSize = true;
			this.groupBox8.Controls.Add(this.label29);
			this.groupBox8.Controls.Add(this.comboBoxListWheelClick);
			this.groupBox8.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox8.Location = new System.Drawing.Point(58, 210);
			this.groupBox8.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox8.Size = new System.Drawing.Size(366, 60);
			this.groupBox8.TabIndex = 1;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "スレッド一覧のマウス操作";
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label29.Location = new System.Drawing.Point(8, 24);
			this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(74, 12);
			this.label29.TabIndex = 0;
			this.label29.Text = "ホイールクリック";
			// 
			// comboBoxListWheelClick
			// 
			this.comboBoxListWheelClick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxListWheelClick.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxListWheelClick.Location = new System.Drawing.Point(152, 20);
			this.comboBoxListWheelClick.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxListWheelClick.Name = "comboBoxListWheelClick";
			this.comboBoxListWheelClick.Size = new System.Drawing.Size(106, 20);
			this.comboBoxListWheelClick.TabIndex = 1;
			// 
			// groupBox7
			// 
			this.groupBox7.AutoSize = true;
			this.groupBox7.Controls.Add(this.checkBoxWheelScroll);
			this.groupBox7.Controls.Add(this.comboBoxWheelClick);
			this.groupBox7.Controls.Add(this.comboBoxDoubleClick);
			this.groupBox7.Controls.Add(this.label27);
			this.groupBox7.Controls.Add(this.label17);
			this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox7.Location = new System.Drawing.Point(58, 94);
			this.groupBox7.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox7.Size = new System.Drawing.Size(366, 104);
			this.groupBox7.TabIndex = 0;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "タブ上のマウス操作";
			// 
			// checkBoxWheelScroll
			// 
			this.checkBoxWheelScroll.AutoSize = true;
			this.checkBoxWheelScroll.Location = new System.Drawing.Point(22, 71);
			this.checkBoxWheelScroll.Name = "checkBoxWheelScroll";
			this.checkBoxWheelScroll.Size = new System.Drawing.Size(317, 16);
			this.checkBoxWheelScroll.TabIndex = 4;
			this.checkBoxWheelScroll.Text = "ホイールスクロールでタブの切り替えを行う (再起動後から有効)";
			this.checkBoxWheelScroll.UseVisualStyleBackColor = true;
			// 
			// comboBoxWheelClick
			// 
			this.comboBoxWheelClick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxWheelClick.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxWheelClick.Location = new System.Drawing.Point(152, 40);
			this.comboBoxWheelClick.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxWheelClick.Name = "comboBoxWheelClick";
			this.comboBoxWheelClick.Size = new System.Drawing.Size(106, 20);
			this.comboBoxWheelClick.TabIndex = 3;
			// 
			// comboBoxDoubleClick
			// 
			this.comboBoxDoubleClick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxDoubleClick.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxDoubleClick.Location = new System.Drawing.Point(152, 16);
			this.comboBoxDoubleClick.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxDoubleClick.Name = "comboBoxDoubleClick";
			this.comboBoxDoubleClick.Size = new System.Drawing.Size(106, 20);
			this.comboBoxDoubleClick.TabIndex = 1;
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label27.Location = new System.Drawing.Point(8, 44);
			this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(74, 12);
			this.label27.TabIndex = 2;
			this.label27.Text = "ホイールクリック";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label17.Location = new System.Drawing.Point(8, 24);
			this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(63, 12);
			this.label17.TabIndex = 0;
			this.label17.Text = "ダブルクリック";
			// 
			// tabPageThread
			// 
			this.tabPageThread.Controls.Add(this.checkBoxAutoReloadAverage);
			this.tabPageThread.Controls.Add(this.checkBoxAutoReloadCheckOnly);
			this.tabPageThread.Controls.Add(this.groupBox14);
			this.tabPageThread.Controls.Add(this.label46);
			this.tabPageThread.Controls.Add(this.label15);
			this.tabPageThread.Controls.Add(this.groupBox10);
			this.tabPageThread.Controls.Add(this.trackBarHistory);
			this.tabPageThread.Controls.Add(this.trackBarAutoReload);
			this.tabPageThread.Controls.Add(this.radioButtonViewLimitResCount);
			this.tabPageThread.Controls.Add(this.radioButtonViewAllRes);
			this.tabPageThread.Controls.Add(this.numericUpDownClosedHistory);
			this.tabPageThread.Controls.Add(this.label10);
			this.tabPageThread.Controls.Add(this.label11);
			this.tabPageThread.Controls.Add(this.numericUpDownAutoReloadInterval);
			this.tabPageThread.Controls.Add(this.label8);
			this.tabPageThread.Controls.Add(this.label9);
			this.tabPageThread.Controls.Add(this.numericUpDownViewResCount);
			this.tabPageThread.ImageIndex = 2;
			this.tabPageThread.Location = new System.Drawing.Point(4, 50);
			this.tabPageThread.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageThread.Name = "tabPageThread";
			this.tabPageThread.Size = new System.Drawing.Size(567, 372);
			this.tabPageThread.TabIndex = 3;
			this.tabPageThread.Text = "スレッド";
			// 
			// checkBoxAutoReloadAverage
			// 
			this.checkBoxAutoReloadAverage.AutoSize = true;
			this.checkBoxAutoReloadAverage.Location = new System.Drawing.Point(105, 127);
			this.checkBoxAutoReloadAverage.Name = "checkBoxAutoReloadAverage";
			this.checkBoxAutoReloadAverage.Size = new System.Drawing.Size(357, 16);
			this.checkBoxAutoReloadAverage.TabIndex = 19;
			this.checkBoxAutoReloadAverage.Text = "過去10レスの平均間隔を元に自動で設定する (次回起動時から有効)";
			this.checkBoxAutoReloadAverage.UseVisualStyleBackColor = true;
			this.checkBoxAutoReloadAverage.CheckedChanged += new System.EventHandler(this.checkBoxAutoReloadAverage_CheckedChanged);
			// 
			// checkBoxAutoReloadCheckOnly
			// 
			this.checkBoxAutoReloadCheckOnly.AutoSize = true;
			this.checkBoxAutoReloadCheckOnly.Location = new System.Drawing.Point(397, 106);
			this.checkBoxAutoReloadCheckOnly.Name = "checkBoxAutoReloadCheckOnly";
			this.checkBoxAutoReloadCheckOnly.Size = new System.Drawing.Size(100, 16);
			this.checkBoxAutoReloadCheckOnly.TabIndex = 18;
			this.checkBoxAutoReloadCheckOnly.Text = "更新チェックのみ";
			this.toolTip.SetToolTip(this.checkBoxAutoReloadCheckOnly, "オートリロードで更新チェックのみを行います。");
			this.checkBoxAutoReloadCheckOnly.UseVisualStyleBackColor = true;
			this.checkBoxAutoReloadCheckOnly.CheckedChanged += new System.EventHandler(this.checkBoxAutoReloadCheckOnly_CheckedChanged);
			// 
			// groupBox14
			// 
			this.groupBox14.AutoSize = true;
			this.groupBox14.Controls.Add(this.checkBoxColoringBackReference);
			this.groupBox14.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox14.Location = new System.Drawing.Point(51, 294);
			this.groupBox14.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox14.Name = "groupBox14";
			this.groupBox14.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox14.Size = new System.Drawing.Size(408, 55);
			this.groupBox14.TabIndex = 17;
			this.groupBox14.TabStop = false;
			this.groupBox14.Text = "表示関連";
			// 
			// checkBoxColoringBackReference
			// 
			this.checkBoxColoringBackReference.AutoSize = true;
			this.checkBoxColoringBackReference.Location = new System.Drawing.Point(14, 18);
			this.checkBoxColoringBackReference.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxColoringBackReference.Name = "checkBoxColoringBackReference";
			this.checkBoxColoringBackReference.Size = new System.Drawing.Size(120, 16);
			this.checkBoxColoringBackReference.TabIndex = 0;
			this.checkBoxColoringBackReference.Text = "被参照レスの色づけ";
			this.toolTip.SetToolTip(this.checkBoxColoringBackReference, "他から参照されているレスの番号を赤く表示します。");
			this.checkBoxColoringBackReference.UseVisualStyleBackColor = true;
			// 
			// label46
			// 
			this.label46.AutoSize = true;
			this.label46.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label46.Location = new System.Drawing.Point(385, 180);
			this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(17, 12);
			this.label46.TabIndex = 15;
			this.label46.Text = "件";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label15.Location = new System.Drawing.Point(384, 39);
			this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(17, 12);
			this.label15.TabIndex = 7;
			this.label15.Text = "件";
			// 
			// groupBox10
			// 
			this.groupBox10.AutoSize = true;
			this.groupBox10.Controls.Add(this.checkBoxAutoOpenImage);
			this.groupBox10.Controls.Add(this.checkBoxTabSelectedAfterReload);
			this.groupBox10.Controls.Add(this.checkBoxScrollToNewRes);
			this.groupBox10.Controls.Add(this.checkBoxAutoReloadOn);
			this.groupBox10.Controls.Add(this.checkBoxAutoScrollOn);
			this.groupBox10.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox10.Location = new System.Drawing.Point(51, 214);
			this.groupBox10.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox10.Size = new System.Drawing.Size(408, 77);
			this.groupBox10.TabIndex = 16;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "デフォルトの動作";
			// 
			// checkBoxAutoOpenImage
			// 
			this.checkBoxAutoOpenImage.AutoSize = true;
			this.checkBoxAutoOpenImage.Location = new System.Drawing.Point(210, 44);
			this.checkBoxAutoOpenImage.Name = "checkBoxAutoOpenImage";
			this.checkBoxAutoOpenImage.Size = new System.Drawing.Size(175, 16);
			this.checkBoxAutoOpenImage.TabIndex = 5;
			this.checkBoxAutoOpenImage.Text = "URLを自動的に開く (画像のみ)";
			this.checkBoxAutoOpenImage.UseVisualStyleBackColor = true;
			// 
			// checkBoxTabSelectedAfterReload
			// 
			this.checkBoxTabSelectedAfterReload.AutoSize = true;
			this.checkBoxTabSelectedAfterReload.Location = new System.Drawing.Point(17, 44);
			this.checkBoxTabSelectedAfterReload.Name = "checkBoxTabSelectedAfterReload";
			this.checkBoxTabSelectedAfterReload.Size = new System.Drawing.Size(183, 16);
			this.checkBoxTabSelectedAfterReload.TabIndex = 3;
			this.checkBoxTabSelectedAfterReload.Text = "タブの選択と同時にスレッドを更新";
			this.toolTip.SetToolTip(this.checkBoxTabSelectedAfterReload, "オートリロードが設定されているスレッドでは動作しません");
			this.checkBoxTabSelectedAfterReload.UseVisualStyleBackColor = true;
			// 
			// checkBoxScrollToNewRes
			// 
			this.checkBoxScrollToNewRes.AutoSize = true;
			this.checkBoxScrollToNewRes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxScrollToNewRes.Location = new System.Drawing.Point(17, 17);
			this.checkBoxScrollToNewRes.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxScrollToNewRes.Name = "checkBoxScrollToNewRes";
			this.checkBoxScrollToNewRes.Size = new System.Drawing.Size(119, 17);
			this.checkBoxScrollToNewRes.TabIndex = 0;
			this.checkBoxScrollToNewRes.Text = "新着までスクロール";
			this.checkBoxScrollToNewRes.CheckedChanged += new System.EventHandler(this.scrollSettings_CheckedChanged);
			// 
			// checkBoxAutoReloadOn
			// 
			this.checkBoxAutoReloadOn.AutoSize = true;
			this.checkBoxAutoReloadOn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAutoReloadOn.Location = new System.Drawing.Point(279, 17);
			this.checkBoxAutoReloadOn.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAutoReloadOn.Name = "checkBoxAutoReloadOn";
			this.checkBoxAutoReloadOn.Size = new System.Drawing.Size(106, 17);
			this.checkBoxAutoReloadOn.TabIndex = 1;
			this.checkBoxAutoReloadOn.Text = "オートリロードOn";
			this.checkBoxAutoReloadOn.CheckedChanged += new System.EventHandler(this.scrollSettings_CheckedChanged);
			// 
			// checkBoxAutoScrollOn
			// 
			this.checkBoxAutoScrollOn.AutoSize = true;
			this.checkBoxAutoScrollOn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAutoScrollOn.Location = new System.Drawing.Point(151, 17);
			this.checkBoxAutoScrollOn.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAutoScrollOn.Name = "checkBoxAutoScrollOn";
			this.checkBoxAutoScrollOn.Size = new System.Drawing.Size(117, 17);
			this.checkBoxAutoScrollOn.TabIndex = 2;
			this.checkBoxAutoScrollOn.Text = "オートスクロールOn";
			// 
			// trackBarHistory
			// 
			this.trackBarHistory.Location = new System.Drawing.Point(105, 174);
			this.trackBarHistory.Margin = new System.Windows.Forms.Padding(2);
			this.trackBarHistory.Maximum = 100;
			this.trackBarHistory.Minimum = 1;
			this.trackBarHistory.Name = "trackBarHistory";
			this.trackBarHistory.Size = new System.Drawing.Size(126, 45);
			this.trackBarHistory.TabIndex = 13;
			this.trackBarHistory.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackBarHistory.Value = 1;
			this.trackBarHistory.Scroll += new System.EventHandler(this.trackBarHistory_Scroll);
			// 
			// trackBarAutoReload
			// 
			this.trackBarAutoReload.LargeChange = 15;
			this.trackBarAutoReload.Location = new System.Drawing.Point(95, 96);
			this.trackBarAutoReload.Margin = new System.Windows.Forms.Padding(2);
			this.trackBarAutoReload.Maximum = 180;
			this.trackBarAutoReload.Minimum = 10;
			this.trackBarAutoReload.Name = "trackBarAutoReload";
			this.trackBarAutoReload.Size = new System.Drawing.Size(126, 45);
			this.trackBarAutoReload.TabIndex = 9;
			this.trackBarAutoReload.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackBarAutoReload.Value = 15;
			this.trackBarAutoReload.Scroll += new System.EventHandler(this.trackBarAutoReload_Scroll);
			// 
			// radioButtonViewLimitResCount
			// 
			this.radioButtonViewLimitResCount.AutoSize = true;
			this.radioButtonViewLimitResCount.Checked = true;
			this.radioButtonViewLimitResCount.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonViewLimitResCount.Location = new System.Drawing.Point(182, 38);
			this.radioButtonViewLimitResCount.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonViewLimitResCount.Name = "radioButtonViewLimitResCount";
			this.radioButtonViewLimitResCount.Size = new System.Drawing.Size(53, 17);
			this.radioButtonViewLimitResCount.TabIndex = 5;
			this.radioButtonViewLimitResCount.TabStop = true;
			this.radioButtonViewLimitResCount.Text = "制限";
			this.radioButtonViewLimitResCount.CheckedChanged += new System.EventHandler(this.radioButtonResViewMode_CheckedChanged);
			// 
			// radioButtonViewAllRes
			// 
			this.radioButtonViewAllRes.AutoSize = true;
			this.radioButtonViewAllRes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonViewAllRes.Location = new System.Drawing.Point(104, 38);
			this.radioButtonViewAllRes.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonViewAllRes.Name = "radioButtonViewAllRes";
			this.radioButtonViewAllRes.Size = new System.Drawing.Size(82, 17);
			this.radioButtonViewAllRes.TabIndex = 4;
			this.radioButtonViewAllRes.Text = "すべて表示";
			this.radioButtonViewAllRes.CheckedChanged += new System.EventHandler(this.radioButtonResViewMode_CheckedChanged);
			// 
			// numericUpDownClosedHistory
			// 
			this.numericUpDownClosedHistory.AutoSize = true;
			this.numericUpDownClosedHistory.Location = new System.Drawing.Point(315, 174);
			this.numericUpDownClosedHistory.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownClosedHistory.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownClosedHistory.Name = "numericUpDownClosedHistory";
			this.numericUpDownClosedHistory.Size = new System.Drawing.Size(64, 19);
			this.numericUpDownClosedHistory.TabIndex = 14;
			this.numericUpDownClosedHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownClosedHistory.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownClosedHistory.ValueChanged += new System.EventHandler(this.numericUpDownClosedHistory_ValueChanged);
			this.numericUpDownClosedHistory.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownClosedHistory.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label10.Location = new System.Drawing.Point(48, 158);
			this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(129, 12);
			this.label10.TabIndex = 12;
			this.label10.Text = "最近閉じたスレッド保持数";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label11.Location = new System.Drawing.Point(375, 109);
			this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(17, 12);
			this.label11.TabIndex = 11;
			this.label11.Text = "秒";
			// 
			// numericUpDownAutoReloadInterval
			// 
			this.numericUpDownAutoReloadInterval.AutoSize = true;
			this.numericUpDownAutoReloadInterval.Location = new System.Drawing.Point(305, 103);
			this.numericUpDownAutoReloadInterval.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownAutoReloadInterval.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numericUpDownAutoReloadInterval.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownAutoReloadInterval.Name = "numericUpDownAutoReloadInterval";
			this.numericUpDownAutoReloadInterval.Size = new System.Drawing.Size(64, 19);
			this.numericUpDownAutoReloadInterval.TabIndex = 10;
			this.numericUpDownAutoReloadInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownAutoReloadInterval.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numericUpDownAutoReloadInterval.ValueChanged += new System.EventHandler(this.numericUpDownAutoReloadInterval_ValueChanged);
			this.numericUpDownAutoReloadInterval.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownAutoReloadInterval.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.BackColor = System.Drawing.SystemColors.Control;
			this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label8.Location = new System.Drawing.Point(48, 73);
			this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(101, 12);
			this.label8.TabIndex = 8;
			this.label8.Text = "オートリロードの間隔";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.BackColor = System.Drawing.SystemColors.Control;
			this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label9.Location = new System.Drawing.Point(48, 16);
			this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(93, 12);
			this.label9.TabIndex = 3;
			this.label9.Text = "レス表示数の指定";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// numericUpDownViewResCount
			// 
			this.numericUpDownViewResCount.AutoSize = true;
			this.numericUpDownViewResCount.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericUpDownViewResCount.Location = new System.Drawing.Point(314, 36);
			this.numericUpDownViewResCount.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownViewResCount.Maximum = new decimal(new int[] {
            1001,
            0,
            0,
            0});
			this.numericUpDownViewResCount.Name = "numericUpDownViewResCount";
			this.numericUpDownViewResCount.Size = new System.Drawing.Size(64, 19);
			this.numericUpDownViewResCount.TabIndex = 6;
			this.numericUpDownViewResCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownViewResCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericUpDownViewResCount.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownViewResCount.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// tabPagePopup
			// 
			this.tabPagePopup.Controls.Add(this.label76);
			this.tabPagePopup.Controls.Add(this.checkBoxClickedHideResPopup);
			this.tabPagePopup.Controls.Add(this.checkBoxClickedHide);
			this.tabPagePopup.Controls.Add(this.numericUpDownImagePopupHeight);
			this.tabPagePopup.Controls.Add(this.label59);
			this.tabPagePopup.Controls.Add(this.numericUpDownImagePopupWidth);
			this.tabPagePopup.Controls.Add(this.label60);
			this.tabPagePopup.Controls.Add(this.label58);
			this.tabPagePopup.Controls.Add(this.comboBoxPopupPos);
			this.tabPagePopup.Controls.Add(this.numericUpDownPopupMaxHeight);
			this.tabPagePopup.Controls.Add(this.label64);
			this.tabPagePopup.Controls.Add(this.numericUpDownPopupMaxWidth);
			this.tabPagePopup.Controls.Add(this.label63);
			this.tabPagePopup.Controls.Add(this.label62);
			this.tabPagePopup.Controls.Add(this.checkBoxUrlPopupCtrlKeySwitch);
			this.tabPagePopup.Controls.Add(this.checkBoxUrlPopup);
			this.tabPagePopup.Controls.Add(this.checkBoxOrigPopup);
			this.tabPagePopup.Controls.Add(this.label37);
			this.tabPagePopup.Controls.Add(this.label26);
			this.tabPagePopup.Controls.Add(this.numericUpDownPopupInterval);
			this.tabPagePopup.Controls.Add(this.textBoxPopupRegex);
			this.tabPagePopup.Controls.Add(this.checkBoxExPopup);
			this.tabPagePopup.Controls.Add(this.checkBoxImagePopupCtrlSwitch);
			this.tabPagePopup.Controls.Add(this.checkBoxImagePopup);
			this.tabPagePopup.ImageIndex = 4;
			this.tabPagePopup.Location = new System.Drawing.Point(4, 50);
			this.tabPagePopup.Margin = new System.Windows.Forms.Padding(2);
			this.tabPagePopup.Name = "tabPagePopup";
			this.tabPagePopup.Size = new System.Drawing.Size(567, 372);
			this.tabPagePopup.TabIndex = 7;
			this.tabPagePopup.Text = "ポップアップ";
			// 
			// label76
			// 
			this.label76.AutoSize = true;
			this.label76.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label76.Location = new System.Drawing.Point(236, 163);
			this.label76.Name = "label76";
			this.label76.Size = new System.Drawing.Size(291, 12);
			this.label76.TabIndex = 25;
			this.label76.Text = "幅または高さのどちらかを 0 に指定すると縦横比を維持します";
			// 
			// checkBoxClickedHideResPopup
			// 
			this.checkBoxClickedHideResPopup.AutoSize = true;
			this.checkBoxClickedHideResPopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxClickedHideResPopup.Location = new System.Drawing.Point(53, 261);
			this.checkBoxClickedHideResPopup.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxClickedHideResPopup.Name = "checkBoxClickedHideResPopup";
			this.checkBoxClickedHideResPopup.Size = new System.Drawing.Size(225, 17);
			this.checkBoxClickedHideResPopup.TabIndex = 24;
			this.checkBoxClickedHideResPopup.Text = "レスポップアップをクリックされるまで消さない";
			this.checkBoxClickedHideResPopup.UseVisualStyleBackColor = true;
			// 
			// checkBoxClickedHide
			// 
			this.checkBoxClickedHide.AutoSize = true;
			this.checkBoxClickedHide.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxClickedHide.Location = new System.Drawing.Point(53, 241);
			this.checkBoxClickedHide.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxClickedHide.Name = "checkBoxClickedHide";
			this.checkBoxClickedHide.Size = new System.Drawing.Size(243, 17);
			this.checkBoxClickedHide.TabIndex = 23;
			this.checkBoxClickedHide.Text = "抽出ポップアップ時にクリックされるまで消さない";
			// 
			// numericUpDownImagePopupHeight
			// 
			this.numericUpDownImagePopupHeight.AutoSize = true;
			this.numericUpDownImagePopupHeight.Enabled = false;
			this.numericUpDownImagePopupHeight.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownImagePopupHeight.Location = new System.Drawing.Point(322, 181);
			this.numericUpDownImagePopupHeight.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownImagePopupHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownImagePopupHeight.Name = "numericUpDownImagePopupHeight";
			this.numericUpDownImagePopupHeight.Size = new System.Drawing.Size(52, 19);
			this.numericUpDownImagePopupHeight.TabIndex = 22;
			this.numericUpDownImagePopupHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownImagePopupHeight.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownImagePopupHeight.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			// 
			// label59
			// 
			this.label59.AutoSize = true;
			this.label59.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label59.Location = new System.Drawing.Point(292, 185);
			this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label59.Name = "label59";
			this.label59.Size = new System.Drawing.Size(25, 12);
			this.label59.TabIndex = 21;
			this.label59.Text = "高さ";
			// 
			// numericUpDownImagePopupWidth
			// 
			this.numericUpDownImagePopupWidth.AutoSize = true;
			this.numericUpDownImagePopupWidth.Enabled = false;
			this.numericUpDownImagePopupWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownImagePopupWidth.Location = new System.Drawing.Point(238, 181);
			this.numericUpDownImagePopupWidth.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownImagePopupWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownImagePopupWidth.Name = "numericUpDownImagePopupWidth";
			this.numericUpDownImagePopupWidth.Size = new System.Drawing.Size(52, 19);
			this.numericUpDownImagePopupWidth.TabIndex = 20;
			this.numericUpDownImagePopupWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownImagePopupWidth.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownImagePopupWidth.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			// 
			// label60
			// 
			this.label60.AutoSize = true;
			this.label60.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label60.Location = new System.Drawing.Point(217, 185);
			this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label60.Name = "label60";
			this.label60.Size = new System.Drawing.Size(17, 12);
			this.label60.TabIndex = 19;
			this.label60.Text = "幅";
			// 
			// label58
			// 
			this.label58.AutoSize = true;
			this.label58.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label58.Location = new System.Drawing.Point(51, 12);
			this.label58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label58.Name = "label58";
			this.label58.Size = new System.Drawing.Size(80, 12);
			this.label58.TabIndex = 17;
			this.label58.Text = "ポップアップ位置";
			// 
			// comboBoxPopupPos
			// 
			this.comboBoxPopupPos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxPopupPos.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxPopupPos.Location = new System.Drawing.Point(164, 10);
			this.comboBoxPopupPos.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxPopupPos.Name = "comboBoxPopupPos";
			this.comboBoxPopupPos.Size = new System.Drawing.Size(119, 20);
			this.comboBoxPopupPos.TabIndex = 16;
			// 
			// numericUpDownPopupMaxHeight
			// 
			this.numericUpDownPopupMaxHeight.AutoSize = true;
			this.numericUpDownPopupMaxHeight.Enabled = false;
			this.numericUpDownPopupMaxHeight.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownPopupMaxHeight.Location = new System.Drawing.Point(322, 84);
			this.numericUpDownPopupMaxHeight.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownPopupMaxHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownPopupMaxHeight.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownPopupMaxHeight.Name = "numericUpDownPopupMaxHeight";
			this.numericUpDownPopupMaxHeight.Size = new System.Drawing.Size(52, 19);
			this.numericUpDownPopupMaxHeight.TabIndex = 6;
			this.numericUpDownPopupMaxHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownPopupMaxHeight.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numericUpDownPopupMaxHeight.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownPopupMaxHeight.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// label64
			// 
			this.label64.AutoSize = true;
			this.label64.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label64.Location = new System.Drawing.Point(292, 88);
			this.label64.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label64.Name = "label64";
			this.label64.Size = new System.Drawing.Size(25, 12);
			this.label64.TabIndex = 5;
			this.label64.Text = "高さ";
			// 
			// numericUpDownPopupMaxWidth
			// 
			this.numericUpDownPopupMaxWidth.AutoSize = true;
			this.numericUpDownPopupMaxWidth.Enabled = false;
			this.numericUpDownPopupMaxWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownPopupMaxWidth.Location = new System.Drawing.Point(238, 84);
			this.numericUpDownPopupMaxWidth.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownPopupMaxWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownPopupMaxWidth.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownPopupMaxWidth.Name = "numericUpDownPopupMaxWidth";
			this.numericUpDownPopupMaxWidth.Size = new System.Drawing.Size(52, 19);
			this.numericUpDownPopupMaxWidth.TabIndex = 4;
			this.numericUpDownPopupMaxWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownPopupMaxWidth.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
			this.numericUpDownPopupMaxWidth.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownPopupMaxWidth.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// label63
			// 
			this.label63.AutoSize = true;
			this.label63.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label63.Location = new System.Drawing.Point(217, 88);
			this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label63.Name = "label63";
			this.label63.Size = new System.Drawing.Size(17, 12);
			this.label63.TabIndex = 3;
			this.label63.Text = "幅";
			// 
			// label62
			// 
			this.label62.AutoSize = true;
			this.label62.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label62.Location = new System.Drawing.Point(74, 88);
			this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label62.Name = "label62";
			this.label62.Size = new System.Drawing.Size(119, 12);
			this.label62.TabIndex = 2;
			this.label62.Text = "ポップアップの最大サイズ";
			// 
			// checkBoxUrlPopupCtrlKeySwitch
			// 
			this.checkBoxUrlPopupCtrlKeySwitch.AutoSize = true;
			this.checkBoxUrlPopupCtrlKeySwitch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxUrlPopupCtrlKeySwitch.Location = new System.Drawing.Point(70, 304);
			this.checkBoxUrlPopupCtrlKeySwitch.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxUrlPopupCtrlKeySwitch.Name = "checkBoxUrlPopupCtrlKeySwitch";
			this.checkBoxUrlPopupCtrlKeySwitch.Size = new System.Drawing.Size(160, 17);
			this.checkBoxUrlPopupCtrlKeySwitch.TabIndex = 12;
			this.checkBoxUrlPopupCtrlKeySwitch.Text = "Ctrlキーを押している時のみ";
			// 
			// checkBoxUrlPopup
			// 
			this.checkBoxUrlPopup.AutoSize = true;
			this.checkBoxUrlPopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxUrlPopup.Location = new System.Drawing.Point(53, 288);
			this.checkBoxUrlPopup.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxUrlPopup.Name = "checkBoxUrlPopup";
			this.checkBoxUrlPopup.Size = new System.Drawing.Size(103, 17);
			this.checkBoxUrlPopup.TabIndex = 11;
			this.checkBoxUrlPopup.Text = "URLポップアップ";
			this.checkBoxUrlPopup.CheckedChanged += new System.EventHandler(this.checkBoxUrlPopup_CheckedChanged);
			// 
			// checkBoxOrigPopup
			// 
			this.checkBoxOrigPopup.AutoSize = true;
			this.checkBoxOrigPopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxOrigPopup.Location = new System.Drawing.Point(53, 68);
			this.checkBoxOrigPopup.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxOrigPopup.Name = "checkBoxOrigPopup";
			this.checkBoxOrigPopup.Size = new System.Drawing.Size(390, 17);
			this.checkBoxOrigPopup.TabIndex = 1;
			this.checkBoxOrigPopup.Text = "独自ポップアップを使用する (多段ポップアップと画像ポップアップは出来ません)";
			this.checkBoxOrigPopup.CheckedChanged += new System.EventHandler(this.checkBoxOrigPopup_CheckedChanged);
			// 
			// label37
			// 
			this.label37.AutoSize = true;
			this.label37.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label37.Location = new System.Drawing.Point(286, 42);
			this.label37.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(39, 12);
			this.label37.TabIndex = 15;
			this.label37.Text = "(ミリ秒)";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label26.Location = new System.Drawing.Point(51, 42);
			this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(152, 12);
			this.label26.TabIndex = 13;
			this.label26.Text = "ポップアップするまでの経過時間";
			// 
			// numericUpDownPopupInterval
			// 
			this.numericUpDownPopupInterval.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericUpDownPopupInterval.Location = new System.Drawing.Point(223, 38);
			this.numericUpDownPopupInterval.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownPopupInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numericUpDownPopupInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownPopupInterval.Name = "numericUpDownPopupInterval";
			this.numericUpDownPopupInterval.ReadOnly = true;
			this.numericUpDownPopupInterval.Size = new System.Drawing.Size(60, 19);
			this.numericUpDownPopupInterval.TabIndex = 14;
			this.numericUpDownPopupInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownPopupInterval.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			// 
			// textBoxPopupRegex
			// 
			this.textBoxPopupRegex.Enabled = false;
			this.textBoxPopupRegex.Location = new System.Drawing.Point(70, 132);
			this.textBoxPopupRegex.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxPopupRegex.Name = "textBoxPopupRegex";
			this.textBoxPopupRegex.Size = new System.Drawing.Size(304, 19);
			this.textBoxPopupRegex.TabIndex = 8;
			// 
			// checkBoxExPopup
			// 
			this.checkBoxExPopup.AutoSize = true;
			this.checkBoxExPopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxExPopup.Location = new System.Drawing.Point(53, 111);
			this.checkBoxExPopup.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxExPopup.Name = "checkBoxExPopup";
			this.checkBoxExPopup.Size = new System.Drawing.Size(246, 17);
			this.checkBoxExPopup.TabIndex = 7;
			this.checkBoxExPopup.Text = "拡張ポップアップ (|で区切って指定してください)";
			this.checkBoxExPopup.CheckedChanged += new System.EventHandler(this.checkBoxExPopup_CheckedChanged);
			// 
			// checkBoxImagePopupCtrlSwitch
			// 
			this.checkBoxImagePopupCtrlSwitch.AutoSize = true;
			this.checkBoxImagePopupCtrlSwitch.Enabled = false;
			this.checkBoxImagePopupCtrlSwitch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxImagePopupCtrlSwitch.Location = new System.Drawing.Point(70, 202);
			this.checkBoxImagePopupCtrlSwitch.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxImagePopupCtrlSwitch.Name = "checkBoxImagePopupCtrlSwitch";
			this.checkBoxImagePopupCtrlSwitch.Size = new System.Drawing.Size(160, 17);
			this.checkBoxImagePopupCtrlSwitch.TabIndex = 10;
			this.checkBoxImagePopupCtrlSwitch.Text = "Ctrlキーを押している時のみ";
			// 
			// checkBoxImagePopup
			// 
			this.checkBoxImagePopup.AutoSize = true;
			this.checkBoxImagePopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxImagePopup.Location = new System.Drawing.Point(53, 181);
			this.checkBoxImagePopup.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxImagePopup.Name = "checkBoxImagePopup";
			this.checkBoxImagePopup.Size = new System.Drawing.Size(105, 17);
			this.checkBoxImagePopup.TabIndex = 9;
			this.checkBoxImagePopup.Text = "画像ポップアップ";
			this.checkBoxImagePopup.CheckedChanged += new System.EventHandler(this.checkBoxImagePopup_CheckedChanged);
			// 
			// tabPagePost
			// 
			this.tabPagePost.Controls.Add(this.textBoxAddWriteSection);
			this.tabPagePost.Controls.Add(this.label75);
			this.tabPagePost.Controls.Add(this.label65);
			this.tabPagePost.Controls.Add(this.label61);
			this.tabPagePost.Controls.Add(this.label45);
			this.tabPagePost.Controls.Add(this.textBoxMdmd);
			this.tabPagePost.Controls.Add(this.textBoxDmdm);
			this.tabPagePost.Controls.Add(this.label38);
			this.tabPagePost.Controls.Add(this.checkBoxPostDlgMinimize);
			this.tabPagePost.Controls.Add(this.checkBoxImeOn);
			this.tabPagePost.Controls.Add(this.checkBoxMultiWriteDialog);
			this.tabPagePost.Controls.Add(this.checkBoxSamba24Check);
			this.tabPagePost.Controls.Add(this.checkBoxThreadKotehan);
			this.tabPagePost.Controls.Add(this.label20);
			this.tabPagePost.Controls.Add(this.checkBoxSavePostHistory);
			this.tabPagePost.Controls.Add(this.checkBoxAutoClose);
			this.tabPagePost.Controls.Add(this.checkBoxShowCookieDialog);
			this.tabPagePost.ImageIndex = 8;
			this.tabPagePost.Location = new System.Drawing.Point(4, 50);
			this.tabPagePost.Margin = new System.Windows.Forms.Padding(2);
			this.tabPagePost.Name = "tabPagePost";
			this.tabPagePost.Size = new System.Drawing.Size(567, 372);
			this.tabPagePost.TabIndex = 1;
			this.tabPagePost.Text = "投稿";
			// 
			// textBoxAddWriteSection
			// 
			this.textBoxAddWriteSection.Location = new System.Drawing.Point(35, 249);
			this.textBoxAddWriteSection.Name = "textBoxAddWriteSection";
			this.textBoxAddWriteSection.Size = new System.Drawing.Size(339, 19);
			this.textBoxAddWriteSection.TabIndex = 16;
			this.textBoxAddWriteSection.Text = "&AditionalAgreementField=&kiri=tanpo";
			// 
			// label75
			// 
			this.label75.AutoSize = true;
			this.label75.Location = new System.Drawing.Point(33, 234);
			this.label75.Name = "label75";
			this.label75.Size = new System.Drawing.Size(175, 12);
			this.label75.TabIndex = 15;
			this.label75.Text = "投稿時に追加するパラメータ文字列:";
			// 
			// label65
			// 
			this.label65.AutoSize = true;
			this.label65.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label65.Location = new System.Drawing.Point(256, 64);
			this.label65.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label65.Name = "label65";
			this.label65.Size = new System.Drawing.Size(39, 12);
			this.label65.TabIndex = 14;
			this.label65.Text = "MDMD";
			// 
			// label61
			// 
			this.label61.AutoSize = true;
			this.label61.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label61.Location = new System.Drawing.Point(256, 40);
			this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label61.Name = "label61";
			this.label61.Size = new System.Drawing.Size(39, 12);
			this.label61.TabIndex = 13;
			this.label61.Text = "DMDM";
			// 
			// label45
			// 
			this.label45.AutoSize = true;
			this.label45.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label45.Location = new System.Drawing.Point(248, 16);
			this.label45.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(85, 12);
			this.label45.TabIndex = 12;
			this.label45.Text = "Be2ch認証情報";
			// 
			// textBoxMdmd
			// 
			this.textBoxMdmd.Location = new System.Drawing.Point(302, 60);
			this.textBoxMdmd.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxMdmd.Name = "textBoxMdmd";
			this.textBoxMdmd.Size = new System.Drawing.Size(132, 19);
			this.textBoxMdmd.TabIndex = 11;
			// 
			// textBoxDmdm
			// 
			this.textBoxDmdm.Location = new System.Drawing.Point(302, 36);
			this.textBoxDmdm.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxDmdm.Name = "textBoxDmdm";
			this.textBoxDmdm.Size = new System.Drawing.Size(132, 19);
			this.textBoxDmdm.TabIndex = 10;
			// 
			// label38
			// 
			this.label38.AutoSize = true;
			this.label38.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label38.Location = new System.Drawing.Point(13, 120);
			this.label38.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(29, 12);
			this.label38.TabIndex = 6;
			this.label38.Text = "表示";
			// 
			// checkBoxPostDlgMinimize
			// 
			this.checkBoxPostDlgMinimize.AutoSize = true;
			this.checkBoxPostDlgMinimize.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxPostDlgMinimize.Location = new System.Drawing.Point(29, 168);
			this.checkBoxPostDlgMinimize.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxPostDlgMinimize.Name = "checkBoxPostDlgMinimize";
			this.checkBoxPostDlgMinimize.Size = new System.Drawing.Size(239, 17);
			this.checkBoxPostDlgMinimize.TabIndex = 9;
			this.checkBoxPostDlgMinimize.Text = "非アクティブ時に書き込みダイアログを最小化";
			// 
			// checkBoxImeOn
			// 
			this.checkBoxImeOn.AutoSize = true;
			this.checkBoxImeOn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxImeOn.Location = new System.Drawing.Point(29, 48);
			this.checkBoxImeOn.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxImeOn.Name = "checkBoxImeOn";
			this.checkBoxImeOn.Size = new System.Drawing.Size(210, 17);
			this.checkBoxImeOn.TabIndex = 2;
			this.checkBoxImeOn.Text = "自動でIMEをオン (日本語入力モード)";
			// 
			// checkBoxMultiWriteDialog
			// 
			this.checkBoxMultiWriteDialog.AutoSize = true;
			this.checkBoxMultiWriteDialog.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxMultiWriteDialog.Location = new System.Drawing.Point(29, 152);
			this.checkBoxMultiWriteDialog.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxMultiWriteDialog.Name = "checkBoxMultiWriteDialog";
			this.checkBoxMultiWriteDialog.Size = new System.Drawing.Size(176, 17);
			this.checkBoxMultiWriteDialog.TabIndex = 8;
			this.checkBoxMultiWriteDialog.Text = "書き込みウインドウを複数表示";
			// 
			// checkBoxSamba24Check
			// 
			this.checkBoxSamba24Check.AutoSize = true;
			this.checkBoxSamba24Check.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxSamba24Check.Location = new System.Drawing.Point(29, 96);
			this.checkBoxSamba24Check.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxSamba24Check.Name = "checkBoxSamba24Check";
			this.checkBoxSamba24Check.Size = new System.Drawing.Size(213, 17);
			this.checkBoxSamba24Check.TabIndex = 5;
			this.checkBoxSamba24Check.Text = "書き込みの自主規制 (samba24対策)";
			// 
			// checkBoxThreadKotehan
			// 
			this.checkBoxThreadKotehan.AutoSize = true;
			this.checkBoxThreadKotehan.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxThreadKotehan.Location = new System.Drawing.Point(29, 32);
			this.checkBoxThreadKotehan.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxThreadKotehan.Name = "checkBoxThreadKotehan";
			this.checkBoxThreadKotehan.Size = new System.Drawing.Size(159, 17);
			this.checkBoxThreadKotehan.TabIndex = 1;
			this.checkBoxThreadKotehan.Text = "スレッドごとにコテハンを保存";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.BackColor = System.Drawing.SystemColors.Control;
			this.label20.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label20.ForeColor = System.Drawing.SystemColors.WindowText;
			this.label20.Location = new System.Drawing.Point(16, 12);
			this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(48, 12);
			this.label20.TabIndex = 0;
			this.label20.Text = "オプション";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// checkBoxSavePostHistory
			// 
			this.checkBoxSavePostHistory.AutoSize = true;
			this.checkBoxSavePostHistory.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxSavePostHistory.Location = new System.Drawing.Point(29, 80);
			this.checkBoxSavePostHistory.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxSavePostHistory.Name = "checkBoxSavePostHistory";
			this.checkBoxSavePostHistory.Size = new System.Drawing.Size(129, 17);
			this.checkBoxSavePostHistory.TabIndex = 4;
			this.checkBoxSavePostHistory.Text = "書き込み履歴を残す";
			// 
			// checkBoxAutoClose
			// 
			this.checkBoxAutoClose.AutoSize = true;
			this.checkBoxAutoClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAutoClose.Location = new System.Drawing.Point(29, 64);
			this.checkBoxAutoClose.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAutoClose.Name = "checkBoxAutoClose";
			this.checkBoxAutoClose.Size = new System.Drawing.Size(159, 17);
			this.checkBoxAutoClose.TabIndex = 3;
			this.checkBoxAutoClose.Text = "書き込み後に自動で閉じる";
			// 
			// checkBoxShowCookieDialog
			// 
			this.checkBoxShowCookieDialog.AutoSize = true;
			this.checkBoxShowCookieDialog.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxShowCookieDialog.Location = new System.Drawing.Point(29, 136);
			this.checkBoxShowCookieDialog.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxShowCookieDialog.Name = "checkBoxShowCookieDialog";
			this.checkBoxShowCookieDialog.Size = new System.Drawing.Size(167, 17);
			this.checkBoxShowCookieDialog.TabIndex = 7;
			this.checkBoxShowCookieDialog.Text = "クッキー確認ダイアログを表示";
			// 
			// tabPageKotehan
			// 
			this.tabPageKotehan.Controls.Add(this.buttonTripPreview);
			this.tabPageKotehan.Controls.Add(this.treeViewKotehan);
			this.tabPageKotehan.Controls.Add(this.label54);
			this.tabPageKotehan.Controls.Add(this.comboBoxBoardList);
			this.tabPageKotehan.Controls.Add(this.label47);
			this.tabPageKotehan.Controls.Add(this.groupBoxKotehan);
			this.tabPageKotehan.Controls.Add(this.buttonRegistKote);
			this.tabPageKotehan.Controls.Add(this.buttonDeleteKote);
			this.tabPageKotehan.ImageIndex = 13;
			this.tabPageKotehan.Location = new System.Drawing.Point(4, 50);
			this.tabPageKotehan.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageKotehan.Name = "tabPageKotehan";
			this.tabPageKotehan.Size = new System.Drawing.Size(567, 372);
			this.tabPageKotehan.TabIndex = 11;
			this.tabPageKotehan.Text = "コテハン";
			// 
			// buttonTripPreview
			// 
			this.buttonTripPreview.AutoSize = true;
			this.buttonTripPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonTripPreview.Location = new System.Drawing.Point(225, 226);
			this.buttonTripPreview.Margin = new System.Windows.Forms.Padding(2);
			this.buttonTripPreview.Name = "buttonTripPreview";
			this.buttonTripPreview.Size = new System.Drawing.Size(100, 24);
			this.buttonTripPreview.TabIndex = 10;
			this.buttonTripPreview.Text = "トリップ確認...";
			this.toolTip.SetToolTip(this.buttonTripPreview, "名前欄にトリップが入力されている場合、トリップをプレビューします");
			this.buttonTripPreview.Click += new System.EventHandler(this.buttonTripPreview_Click);
			// 
			// treeViewKotehan
			// 
			this.treeViewKotehan.FullRowSelect = true;
			this.treeViewKotehan.HideSelection = false;
			this.treeViewKotehan.HotTracking = true;
			this.treeViewKotehan.ImageIndex = 15;
			this.treeViewKotehan.ImageList = this.imageList;
			this.treeViewKotehan.Location = new System.Drawing.Point(85, 48);
			this.treeViewKotehan.Margin = new System.Windows.Forms.Padding(2);
			this.treeViewKotehan.Name = "treeViewKotehan";
			this.treeViewKotehan.SelectedImageIndex = 16;
			this.treeViewKotehan.ShowLines = false;
			this.treeViewKotehan.ShowPlusMinus = false;
			this.treeViewKotehan.ShowRootLines = false;
			this.treeViewKotehan.Size = new System.Drawing.Size(123, 199);
			this.treeViewKotehan.TabIndex = 9;
			this.treeViewKotehan.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewKotehan_AfterSelect);
			this.treeViewKotehan.Click += new System.EventHandler(this.treeViewKotehan_Click);
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
			// 
			// label54
			// 
			this.label54.AutoSize = true;
			this.label54.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label54.Location = new System.Drawing.Point(86, 31);
			this.label54.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label54.Name = "label54";
			this.label54.Size = new System.Drawing.Size(112, 12);
			this.label54.TabIndex = 8;
			this.label54.Text = "登録済みコテハン一覧";
			// 
			// comboBoxBoardList
			// 
			this.comboBoxBoardList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxBoardList.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxBoardList.Location = new System.Drawing.Point(225, 48);
			this.comboBoxBoardList.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxBoardList.Name = "comboBoxBoardList";
			this.comboBoxBoardList.Size = new System.Drawing.Size(148, 20);
			this.comboBoxBoardList.TabIndex = 1;
			this.comboBoxBoardList.SelectedIndexChanged += new System.EventHandler(this.comboBoxBoardList_SelectedIndexChanged);
			// 
			// label47
			// 
			this.label47.AutoSize = true;
			this.label47.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label47.Location = new System.Drawing.Point(225, 31);
			this.label47.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(189, 12);
			this.label47.TabIndex = 0;
			this.label47.Text = "コテハンを登録する板を選択してください";
			// 
			// groupBoxKotehan
			// 
			this.groupBoxKotehan.AutoSize = true;
			this.groupBoxKotehan.Controls.Add(this.checkBoxSendBeID);
			this.groupBoxKotehan.Controls.Add(this.label48);
			this.groupBoxKotehan.Controls.Add(this.textBoxPostEmail);
			this.groupBoxKotehan.Controls.Add(this.label49);
			this.groupBoxKotehan.Controls.Add(this.textBoxPostName);
			this.groupBoxKotehan.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBoxKotehan.Location = new System.Drawing.Point(225, 89);
			this.groupBoxKotehan.Margin = new System.Windows.Forms.Padding(2);
			this.groupBoxKotehan.Name = "groupBoxKotehan";
			this.groupBoxKotehan.Padding = new System.Windows.Forms.Padding(2);
			this.groupBoxKotehan.Size = new System.Drawing.Size(231, 104);
			this.groupBoxKotehan.TabIndex = 5;
			this.groupBoxKotehan.TabStop = false;
			this.groupBoxKotehan.Text = "コテハン情報";
			// 
			// checkBoxSendBeID
			// 
			this.checkBoxSendBeID.AutoSize = true;
			this.checkBoxSendBeID.Enabled = false;
			this.checkBoxSendBeID.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxSendBeID.Location = new System.Drawing.Point(80, 66);
			this.checkBoxSendBeID.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxSendBeID.Name = "checkBoxSendBeID";
			this.checkBoxSendBeID.Size = new System.Drawing.Size(89, 17);
			this.checkBoxSendBeID.TabIndex = 4;
			this.checkBoxSendBeID.Text = "BeIDの送信";
			this.checkBoxSendBeID.Visible = false;
			// 
			// label48
			// 
			this.label48.AutoSize = true;
			this.label48.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label48.Location = new System.Drawing.Point(21, 24);
			this.label48.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(29, 12);
			this.label48.TabIndex = 0;
			this.label48.Text = "名前";
			// 
			// textBoxPostEmail
			// 
			this.textBoxPostEmail.Location = new System.Drawing.Point(80, 44);
			this.textBoxPostEmail.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxPostEmail.Name = "textBoxPostEmail";
			this.textBoxPostEmail.Size = new System.Drawing.Size(132, 19);
			this.textBoxPostEmail.TabIndex = 3;
			this.textBoxPostEmail.TextChanged += new System.EventHandler(this.textBoxKotehan_TextChanged);
			// 
			// label49
			// 
			this.label49.AutoSize = true;
			this.label49.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label49.Location = new System.Drawing.Point(21, 48);
			this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label49.Name = "label49";
			this.label49.Size = new System.Drawing.Size(33, 12);
			this.label49.TabIndex = 2;
			this.label49.Text = "メール";
			// 
			// textBoxPostName
			// 
			this.textBoxPostName.Location = new System.Drawing.Point(80, 20);
			this.textBoxPostName.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxPostName.Name = "textBoxPostName";
			this.textBoxPostName.Size = new System.Drawing.Size(132, 19);
			this.textBoxPostName.TabIndex = 1;
			this.textBoxPostName.TextChanged += new System.EventHandler(this.textBoxKotehan_TextChanged);
			// 
			// buttonRegistKote
			// 
			this.buttonRegistKote.AutoSize = true;
			this.buttonRegistKote.Enabled = false;
			this.buttonRegistKote.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRegistKote.Location = new System.Drawing.Point(334, 226);
			this.buttonRegistKote.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRegistKote.Name = "buttonRegistKote";
			this.buttonRegistKote.Size = new System.Drawing.Size(58, 24);
			this.buttonRegistKote.TabIndex = 4;
			this.buttonRegistKote.Text = "登録";
			this.buttonRegistKote.Click += new System.EventHandler(this.buttonRegistKote_Click);
			// 
			// buttonDeleteKote
			// 
			this.buttonDeleteKote.AutoSize = true;
			this.buttonDeleteKote.Enabled = false;
			this.buttonDeleteKote.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonDeleteKote.Location = new System.Drawing.Point(397, 226);
			this.buttonDeleteKote.Margin = new System.Windows.Forms.Padding(2);
			this.buttonDeleteKote.Name = "buttonDeleteKote";
			this.buttonDeleteKote.Size = new System.Drawing.Size(58, 24);
			this.buttonDeleteKote.TabIndex = 2;
			this.buttonDeleteKote.Text = "削除";
			this.buttonDeleteKote.Click += new System.EventHandler(this.buttonDeleteKote_Click);
			// 
			// tabPageProxy
			// 
			this.tabPageProxy.Controls.Add(this.buttonClearProxy);
			this.tabPageProxy.Controls.Add(this.buttonAuthenticateOption);
			this.tabPageProxy.Controls.Add(this.groupBox5);
			this.tabPageProxy.Controls.Add(this.groupBox6);
			this.tabPageProxy.ImageIndex = 5;
			this.tabPageProxy.Location = new System.Drawing.Point(4, 50);
			this.tabPageProxy.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageProxy.Name = "tabPageProxy";
			this.tabPageProxy.Size = new System.Drawing.Size(567, 372);
			this.tabPageProxy.TabIndex = 8;
			this.tabPageProxy.Text = "プロキシ";
			// 
			// buttonClearProxy
			// 
			this.buttonClearProxy.AutoSize = true;
			this.buttonClearProxy.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClearProxy.Location = new System.Drawing.Point(342, 222);
			this.buttonClearProxy.Margin = new System.Windows.Forms.Padding(2);
			this.buttonClearProxy.Name = "buttonClearProxy";
			this.buttonClearProxy.Size = new System.Drawing.Size(138, 24);
			this.buttonClearProxy.TabIndex = 3;
			this.buttonClearProxy.Text = "プロキシ情報をクリア";
			this.buttonClearProxy.Click += new System.EventHandler(this.buttonClearProxy_Click);
			// 
			// buttonAuthenticateOption
			// 
			this.buttonAuthenticateOption.AutoSize = true;
			this.buttonAuthenticateOption.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAuthenticateOption.Location = new System.Drawing.Point(90, 222);
			this.buttonAuthenticateOption.Margin = new System.Windows.Forms.Padding(2);
			this.buttonAuthenticateOption.Name = "buttonAuthenticateOption";
			this.buttonAuthenticateOption.Size = new System.Drawing.Size(135, 24);
			this.buttonAuthenticateOption.TabIndex = 2;
			this.buttonAuthenticateOption.Text = "認証設定を行う(&A)";
			this.buttonAuthenticateOption.Click += new System.EventHandler(this.buttonAuthenticateOption_Click);
			// 
			// groupBox5
			// 
			this.groupBox5.AutoSize = true;
			this.groupBox5.Controls.Add(this.label21);
			this.groupBox5.Controls.Add(this.label19);
			this.groupBox5.Controls.Add(this.checkBoxRecvProxyCredential);
			this.groupBox5.Controls.Add(this.textBoxRecvProxyPass);
			this.groupBox5.Controls.Add(this.textBoxRecvProxyUserID);
			this.groupBox5.Controls.Add(this.numericUpDownRecvProxyPort);
			this.groupBox5.Controls.Add(this.label18);
			this.groupBox5.Controls.Add(this.label16);
			this.groupBox5.Controls.Add(this.textBoxRecvProxyHost);
			this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox5.Location = new System.Drawing.Point(90, 34);
			this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox5.Size = new System.Drawing.Size(382, 90);
			this.groupBox5.TabIndex = 0;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "受信用プロキシ";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label21.Location = new System.Drawing.Point(197, 52);
			this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(52, 12);
			this.label21.TabIndex = 7;
			this.label21.Text = "パスワード";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label19.Location = new System.Drawing.Point(197, 24);
			this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(56, 12);
			this.label19.TabIndex = 5;
			this.label19.Text = "ユーザーID";
			// 
			// checkBoxRecvProxyCredential
			// 
			this.checkBoxRecvProxyCredential.AutoSize = true;
			this.checkBoxRecvProxyCredential.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxRecvProxyCredential.Location = new System.Drawing.Point(122, 52);
			this.checkBoxRecvProxyCredential.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxRecvProxyCredential.Name = "checkBoxRecvProxyCredential";
			this.checkBoxRecvProxyCredential.Size = new System.Drawing.Size(54, 17);
			this.checkBoxRecvProxyCredential.TabIndex = 4;
			this.checkBoxRecvProxyCredential.Text = "認証";
			this.checkBoxRecvProxyCredential.CheckedChanged += new System.EventHandler(this.checkBoxRecvProxyCredential_CheckedChanged);
			// 
			// textBoxRecvProxyPass
			// 
			this.textBoxRecvProxyPass.Enabled = false;
			this.textBoxRecvProxyPass.Location = new System.Drawing.Point(265, 48);
			this.textBoxRecvProxyPass.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxRecvProxyPass.Name = "textBoxRecvProxyPass";
			this.textBoxRecvProxyPass.PasswordChar = '*';
			this.textBoxRecvProxyPass.Size = new System.Drawing.Size(106, 19);
			this.textBoxRecvProxyPass.TabIndex = 8;
			// 
			// textBoxRecvProxyUserID
			// 
			this.textBoxRecvProxyUserID.Enabled = false;
			this.textBoxRecvProxyUserID.Location = new System.Drawing.Point(265, 20);
			this.textBoxRecvProxyUserID.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxRecvProxyUserID.Name = "textBoxRecvProxyUserID";
			this.textBoxRecvProxyUserID.Size = new System.Drawing.Size(106, 19);
			this.textBoxRecvProxyUserID.TabIndex = 6;
			// 
			// numericUpDownRecvProxyPort
			// 
			this.numericUpDownRecvProxyPort.AutoSize = true;
			this.numericUpDownRecvProxyPort.Location = new System.Drawing.Point(55, 48);
			this.numericUpDownRecvProxyPort.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownRecvProxyPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
			this.numericUpDownRecvProxyPort.Name = "numericUpDownRecvProxyPort";
			this.numericUpDownRecvProxyPort.Size = new System.Drawing.Size(67, 19);
			this.numericUpDownRecvProxyPort.TabIndex = 3;
			this.numericUpDownRecvProxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownRecvProxyPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label18.Location = new System.Drawing.Point(13, 52);
			this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(33, 12);
			this.label18.TabIndex = 2;
			this.label18.Text = "ポート";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label16.Location = new System.Drawing.Point(13, 24);
			this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(32, 12);
			this.label16.TabIndex = 0;
			this.label16.Text = "ホスト";
			// 
			// textBoxRecvProxyHost
			// 
			this.textBoxRecvProxyHost.Location = new System.Drawing.Point(55, 20);
			this.textBoxRecvProxyHost.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxRecvProxyHost.Name = "textBoxRecvProxyHost";
			this.textBoxRecvProxyHost.Size = new System.Drawing.Size(106, 19);
			this.textBoxRecvProxyHost.TabIndex = 1;
			// 
			// groupBox6
			// 
			this.groupBox6.AutoSize = true;
			this.groupBox6.Controls.Add(this.label22);
			this.groupBox6.Controls.Add(this.label23);
			this.groupBox6.Controls.Add(this.checkBoxSendProxyCredential);
			this.groupBox6.Controls.Add(this.textBoxSendProxyPass);
			this.groupBox6.Controls.Add(this.textBoxSendProxyUserID);
			this.groupBox6.Controls.Add(this.numericUpDownSendProxyPort);
			this.groupBox6.Controls.Add(this.label24);
			this.groupBox6.Controls.Add(this.label25);
			this.groupBox6.Controls.Add(this.textBoxSendProxyHost);
			this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox6.Location = new System.Drawing.Point(90, 130);
			this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox6.Size = new System.Drawing.Size(382, 90);
			this.groupBox6.TabIndex = 1;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "書き込み用プロキシ";
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label22.Location = new System.Drawing.Point(197, 52);
			this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(52, 12);
			this.label22.TabIndex = 7;
			this.label22.Text = "パスワード";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label23.Location = new System.Drawing.Point(197, 24);
			this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(56, 12);
			this.label23.TabIndex = 5;
			this.label23.Text = "ユーザーID";
			// 
			// checkBoxSendProxyCredential
			// 
			this.checkBoxSendProxyCredential.AutoSize = true;
			this.checkBoxSendProxyCredential.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxSendProxyCredential.Location = new System.Drawing.Point(122, 52);
			this.checkBoxSendProxyCredential.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxSendProxyCredential.Name = "checkBoxSendProxyCredential";
			this.checkBoxSendProxyCredential.Size = new System.Drawing.Size(54, 17);
			this.checkBoxSendProxyCredential.TabIndex = 4;
			this.checkBoxSendProxyCredential.Text = "認証";
			this.checkBoxSendProxyCredential.CheckedChanged += new System.EventHandler(this.checkBoxSendProxyCredential_CheckedChanged);
			// 
			// textBoxSendProxyPass
			// 
			this.textBoxSendProxyPass.Enabled = false;
			this.textBoxSendProxyPass.Location = new System.Drawing.Point(265, 48);
			this.textBoxSendProxyPass.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxSendProxyPass.Name = "textBoxSendProxyPass";
			this.textBoxSendProxyPass.PasswordChar = '*';
			this.textBoxSendProxyPass.Size = new System.Drawing.Size(106, 19);
			this.textBoxSendProxyPass.TabIndex = 8;
			// 
			// textBoxSendProxyUserID
			// 
			this.textBoxSendProxyUserID.Enabled = false;
			this.textBoxSendProxyUserID.Location = new System.Drawing.Point(265, 20);
			this.textBoxSendProxyUserID.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxSendProxyUserID.Name = "textBoxSendProxyUserID";
			this.textBoxSendProxyUserID.Size = new System.Drawing.Size(106, 19);
			this.textBoxSendProxyUserID.TabIndex = 6;
			// 
			// numericUpDownSendProxyPort
			// 
			this.numericUpDownSendProxyPort.AutoSize = true;
			this.numericUpDownSendProxyPort.Location = new System.Drawing.Point(55, 48);
			this.numericUpDownSendProxyPort.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownSendProxyPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
			this.numericUpDownSendProxyPort.Name = "numericUpDownSendProxyPort";
			this.numericUpDownSendProxyPort.Size = new System.Drawing.Size(67, 19);
			this.numericUpDownSendProxyPort.TabIndex = 3;
			this.numericUpDownSendProxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownSendProxyPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label24.Location = new System.Drawing.Point(13, 52);
			this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(33, 12);
			this.label24.TabIndex = 2;
			this.label24.Text = "ポート";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label25.Location = new System.Drawing.Point(13, 24);
			this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(32, 12);
			this.label25.TabIndex = 0;
			this.label25.Text = "ホスト";
			// 
			// textBoxSendProxyHost
			// 
			this.textBoxSendProxyHost.Location = new System.Drawing.Point(55, 20);
			this.textBoxSendProxyHost.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxSendProxyHost.Name = "textBoxSendProxyHost";
			this.textBoxSendProxyHost.Size = new System.Drawing.Size(106, 19);
			this.textBoxSendProxyHost.TabIndex = 1;
			// 
			// tabPageSound
			// 
			this.tabPageSound.Controls.Add(this.propertyGridSound);
			this.tabPageSound.ImageIndex = 17;
			this.tabPageSound.Location = new System.Drawing.Point(4, 50);
			this.tabPageSound.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageSound.Name = "tabPageSound";
			this.tabPageSound.Size = new System.Drawing.Size(567, 372);
			this.tabPageSound.TabIndex = 12;
			this.tabPageSound.Text = "サウンド";
			// 
			// propertyGridSound
			// 
			this.propertyGridSound.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGridSound.HelpVisible = false;
			this.propertyGridSound.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGridSound.Location = new System.Drawing.Point(0, 0);
			this.propertyGridSound.Margin = new System.Windows.Forms.Padding(2);
			this.propertyGridSound.Name = "propertyGridSound";
			this.propertyGridSound.Size = new System.Drawing.Size(567, 372);
			this.propertyGridSound.TabIndex = 0;
			this.propertyGridSound.ToolbarVisible = false;
			this.propertyGridSound.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridSound_PropertyValueChanged);
			// 
			// tabPageDesign
			// 
			this.tabPageDesign.Controls.Add(this.tabControlDesign);
			this.tabPageDesign.ImageIndex = 3;
			this.tabPageDesign.Location = new System.Drawing.Point(4, 50);
			this.tabPageDesign.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageDesign.Name = "tabPageDesign";
			this.tabPageDesign.Size = new System.Drawing.Size(567, 372);
			this.tabPageDesign.TabIndex = 9;
			this.tabPageDesign.Text = "デザイン";
			// 
			// tabControlDesign
			// 
			this.tabControlDesign.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControlDesign.Controls.Add(this.tabPageDesignCommon);
			this.tabControlDesign.Controls.Add(this.tabPageTableDesign);
			this.tabControlDesign.Controls.Add(this.tabPageListDesign);
			this.tabControlDesign.Controls.Add(this.tabPageThreadDesign);
			this.tabControlDesign.Controls.Add(this.tabPageWrite);
			this.tabControlDesign.ImageList = this.imageList;
			this.tabControlDesign.ItemSize = new System.Drawing.Size(85, 23);
			this.tabControlDesign.Location = new System.Drawing.Point(54, 15);
			this.tabControlDesign.Margin = new System.Windows.Forms.Padding(2);
			this.tabControlDesign.Name = "tabControlDesign";
			this.tabControlDesign.SelectedIndex = 0;
			this.tabControlDesign.Size = new System.Drawing.Size(462, 298);
			this.tabControlDesign.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControlDesign.TabIndex = 0;
			this.tabControlDesign.SelectedIndexChanged += new System.EventHandler(this.tabControlDesign_SelectedIndexChanged);
			// 
			// tabPageDesignCommon
			// 
			this.tabPageDesignCommon.Controls.Add(this.checkBoxIsHighlightActiveTab);
			this.tabPageDesignCommon.Controls.Add(this.labelHighlightActiveColor);
			this.tabPageDesignCommon.Controls.Add(this.label73);
			this.tabPageDesignCommon.Controls.Add(this.comboBoxTabAppearance);
			this.tabPageDesignCommon.Controls.Add(this.panel4);
			this.tabPageDesignCommon.Controls.Add(this.panel3);
			this.tabPageDesignCommon.Controls.Add(this.label57);
			this.tabPageDesignCommon.Controls.Add(this.label53);
			this.tabPageDesignCommon.Controls.Add(this.label28);
			this.tabPageDesignCommon.ImageIndex = 11;
			this.tabPageDesignCommon.Location = new System.Drawing.Point(4, 27);
			this.tabPageDesignCommon.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageDesignCommon.Name = "tabPageDesignCommon";
			this.tabPageDesignCommon.Size = new System.Drawing.Size(454, 267);
			this.tabPageDesignCommon.TabIndex = 3;
			this.tabPageDesignCommon.Text = "共通設定";
			// 
			// checkBoxIsHighlightActiveTab
			// 
			this.checkBoxIsHighlightActiveTab.AutoSize = true;
			this.checkBoxIsHighlightActiveTab.Location = new System.Drawing.Point(18, 241);
			this.checkBoxIsHighlightActiveTab.Name = "checkBoxIsHighlightActiveTab";
			this.checkBoxIsHighlightActiveTab.Size = new System.Drawing.Size(140, 16);
			this.checkBoxIsHighlightActiveTab.TabIndex = 8;
			this.checkBoxIsHighlightActiveTab.Text = "アクティブタブを強調表示";
			this.checkBoxIsHighlightActiveTab.UseVisualStyleBackColor = true;
			this.checkBoxIsHighlightActiveTab.CheckedChanged += new System.EventHandler(this.checkBoxIsHighlightActiveTab_CheckedChanged);
			// 
			// labelHighlightActiveColor
			// 
			this.labelHighlightActiveColor.AutoSize = true;
			this.labelHighlightActiveColor.BackColor = System.Drawing.Color.White;
			this.labelHighlightActiveColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelHighlightActiveColor.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelHighlightActiveColor.Location = new System.Drawing.Point(173, 239);
			this.labelHighlightActiveColor.Name = "labelHighlightActiveColor";
			this.labelHighlightActiveColor.Size = new System.Drawing.Size(31, 14);
			this.labelHighlightActiveColor.TabIndex = 7;
			this.labelHighlightActiveColor.Text = "　　　";
			this.toolTip.SetToolTip(this.labelHighlightActiveColor, "クリックすると色を変更できます");
			this.labelHighlightActiveColor.Click += new System.EventHandler(this.labelHightlightActiveColor_Click);
			// 
			// label73
			// 
			this.label73.AutoSize = true;
			this.label73.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label73.Location = new System.Drawing.Point(9, 36);
			this.label73.Name = "label73";
			this.label73.Size = new System.Drawing.Size(70, 12);
			this.label73.TabIndex = 6;
			this.label73.Text = "タブのスタイル:";
			// 
			// comboBoxTabAppearance
			// 
			this.comboBoxTabAppearance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTabAppearance.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxTabAppearance.FormattingEnabled = true;
			this.comboBoxTabAppearance.Location = new System.Drawing.Point(88, 33);
			this.comboBoxTabAppearance.Name = "comboBoxTabAppearance";
			this.comboBoxTabAppearance.Size = new System.Drawing.Size(136, 20);
			this.comboBoxTabAppearance.TabIndex = 5;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.radioButtonThreadTabFillRight);
			this.panel4.Controls.Add(this.label55);
			this.panel4.Controls.Add(this.label56);
			this.panel4.Controls.Add(this.radioButtonThreadTabFixed);
			this.panel4.Controls.Add(this.radioButtonThreadTabAutoSize);
			this.panel4.Controls.Add(this.numericUpDownThreadTabSizeHeight);
			this.panel4.Controls.Add(this.numericUpDownThreadTabSizeWidth);
			this.panel4.Location = new System.Drawing.Point(88, 159);
			this.panel4.Margin = new System.Windows.Forms.Padding(2);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(273, 64);
			this.panel4.TabIndex = 4;
			// 
			// radioButtonThreadTabFillRight
			// 
			this.radioButtonThreadTabFillRight.AutoSize = true;
			this.radioButtonThreadTabFillRight.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonThreadTabFillRight.Location = new System.Drawing.Point(6, 24);
			this.radioButtonThreadTabFillRight.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonThreadTabFillRight.Name = "radioButtonThreadTabFillRight";
			this.radioButtonThreadTabFillRight.Size = new System.Drawing.Size(130, 17);
			this.radioButtonThreadTabFillRight.TabIndex = 1;
			this.radioButtonThreadTabFillRight.Text = "右端までタブで埋める";
			// 
			// label55
			// 
			this.label55.AutoSize = true;
			this.label55.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label55.Location = new System.Drawing.Point(100, 44);
			this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label55.Name = "label55";
			this.label55.Size = new System.Drawing.Size(19, 12);
			this.label55.TabIndex = 3;
			this.label55.Text = "幅:";
			// 
			// label56
			// 
			this.label56.AutoSize = true;
			this.label56.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label56.Location = new System.Drawing.Point(181, 44);
			this.label56.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label56.Name = "label56";
			this.label56.Size = new System.Drawing.Size(27, 12);
			this.label56.TabIndex = 5;
			this.label56.Text = "高さ:";
			// 
			// radioButtonThreadTabFixed
			// 
			this.radioButtonThreadTabFixed.AutoSize = true;
			this.radioButtonThreadTabFixed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonThreadTabFixed.Location = new System.Drawing.Point(6, 40);
			this.radioButtonThreadTabFixed.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonThreadTabFixed.Name = "radioButtonThreadTabFixed";
			this.radioButtonThreadTabFixed.Size = new System.Drawing.Size(82, 17);
			this.radioButtonThreadTabFixed.TabIndex = 2;
			this.radioButtonThreadTabFixed.Text = "固定サイズ";
			this.radioButtonThreadTabFixed.CheckedChanged += new System.EventHandler(this.radioButtonTabFixedSize_CheckedChanged);
			// 
			// radioButtonThreadTabAutoSize
			// 
			this.radioButtonThreadTabAutoSize.AutoSize = true;
			this.radioButtonThreadTabAutoSize.Checked = true;
			this.radioButtonThreadTabAutoSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonThreadTabAutoSize.Location = new System.Drawing.Point(6, 4);
			this.radioButtonThreadTabAutoSize.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonThreadTabAutoSize.Name = "radioButtonThreadTabAutoSize";
			this.radioButtonThreadTabAutoSize.Size = new System.Drawing.Size(159, 17);
			this.radioButtonThreadTabAutoSize.TabIndex = 0;
			this.radioButtonThreadTabAutoSize.TabStop = true;
			this.radioButtonThreadTabAutoSize.Text = "自動で文字の幅に合わせる";
			// 
			// numericUpDownThreadTabSizeHeight
			// 
			this.numericUpDownThreadTabSizeHeight.AutoSize = true;
			this.numericUpDownThreadTabSizeHeight.BackColor = System.Drawing.SystemColors.Window;
			this.numericUpDownThreadTabSizeHeight.Enabled = false;
			this.numericUpDownThreadTabSizeHeight.Location = new System.Drawing.Point(214, 40);
			this.numericUpDownThreadTabSizeHeight.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownThreadTabSizeHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownThreadTabSizeHeight.Name = "numericUpDownThreadTabSizeHeight";
			this.numericUpDownThreadTabSizeHeight.Size = new System.Drawing.Size(51, 19);
			this.numericUpDownThreadTabSizeHeight.TabIndex = 6;
			this.numericUpDownThreadTabSizeHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.numericUpDownThreadTabSizeHeight, "この設定は次回起動後有効になります");
			this.numericUpDownThreadTabSizeHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// numericUpDownThreadTabSizeWidth
			// 
			this.numericUpDownThreadTabSizeWidth.AutoSize = true;
			this.numericUpDownThreadTabSizeWidth.BackColor = System.Drawing.SystemColors.Window;
			this.numericUpDownThreadTabSizeWidth.Enabled = false;
			this.numericUpDownThreadTabSizeWidth.Location = new System.Drawing.Point(126, 40);
			this.numericUpDownThreadTabSizeWidth.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownThreadTabSizeWidth.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numericUpDownThreadTabSizeWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownThreadTabSizeWidth.Name = "numericUpDownThreadTabSizeWidth";
			this.numericUpDownThreadTabSizeWidth.Size = new System.Drawing.Size(51, 19);
			this.numericUpDownThreadTabSizeWidth.TabIndex = 4;
			this.numericUpDownThreadTabSizeWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.numericUpDownThreadTabSizeWidth, "この設定は次回起動後有効になります");
			this.numericUpDownThreadTabSizeWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.radioButtonTabFillRight);
			this.panel3.Controls.Add(this.label51);
			this.panel3.Controls.Add(this.numericUpDownTabSizeWidth);
			this.panel3.Controls.Add(this.radioButtonTabFixedSize);
			this.panel3.Controls.Add(this.numericUpDownTabSizeHeight);
			this.panel3.Controls.Add(this.radioButtonTabAutoSize);
			this.panel3.Controls.Add(this.label50);
			this.panel3.Location = new System.Drawing.Point(88, 70);
			this.panel3.Margin = new System.Windows.Forms.Padding(2);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(273, 68);
			this.panel3.TabIndex = 2;
			// 
			// radioButtonTabFillRight
			// 
			this.radioButtonTabFillRight.AutoSize = true;
			this.radioButtonTabFillRight.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonTabFillRight.Location = new System.Drawing.Point(4, 28);
			this.radioButtonTabFillRight.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonTabFillRight.Name = "radioButtonTabFillRight";
			this.radioButtonTabFillRight.Size = new System.Drawing.Size(130, 17);
			this.radioButtonTabFillRight.TabIndex = 1;
			this.radioButtonTabFillRight.Text = "右端までタブで埋める";
			// 
			// label51
			// 
			this.label51.AutoSize = true;
			this.label51.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label51.Location = new System.Drawing.Point(100, 52);
			this.label51.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label51.Name = "label51";
			this.label51.Size = new System.Drawing.Size(19, 12);
			this.label51.TabIndex = 3;
			this.label51.Text = "幅:";
			// 
			// numericUpDownTabSizeWidth
			// 
			this.numericUpDownTabSizeWidth.AutoSize = true;
			this.numericUpDownTabSizeWidth.BackColor = System.Drawing.SystemColors.Window;
			this.numericUpDownTabSizeWidth.Enabled = false;
			this.numericUpDownTabSizeWidth.Location = new System.Drawing.Point(126, 48);
			this.numericUpDownTabSizeWidth.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownTabSizeWidth.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numericUpDownTabSizeWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownTabSizeWidth.Name = "numericUpDownTabSizeWidth";
			this.numericUpDownTabSizeWidth.Size = new System.Drawing.Size(51, 19);
			this.numericUpDownTabSizeWidth.TabIndex = 4;
			this.numericUpDownTabSizeWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.numericUpDownTabSizeWidth, "この設定は次回起動後有効になります");
			this.numericUpDownTabSizeWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownTabSizeWidth.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownTabSizeWidth.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// radioButtonTabFixedSize
			// 
			this.radioButtonTabFixedSize.AutoSize = true;
			this.radioButtonTabFixedSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonTabFixedSize.Location = new System.Drawing.Point(4, 44);
			this.radioButtonTabFixedSize.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonTabFixedSize.Name = "radioButtonTabFixedSize";
			this.radioButtonTabFixedSize.Size = new System.Drawing.Size(82, 17);
			this.radioButtonTabFixedSize.TabIndex = 2;
			this.radioButtonTabFixedSize.Text = "固定サイズ";
			this.radioButtonTabFixedSize.CheckedChanged += new System.EventHandler(this.radioButtonTabFixedSize_CheckedChanged);
			// 
			// numericUpDownTabSizeHeight
			// 
			this.numericUpDownTabSizeHeight.AutoSize = true;
			this.numericUpDownTabSizeHeight.BackColor = System.Drawing.SystemColors.Window;
			this.numericUpDownTabSizeHeight.Enabled = false;
			this.numericUpDownTabSizeHeight.Location = new System.Drawing.Point(214, 48);
			this.numericUpDownTabSizeHeight.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownTabSizeHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownTabSizeHeight.Name = "numericUpDownTabSizeHeight";
			this.numericUpDownTabSizeHeight.Size = new System.Drawing.Size(51, 19);
			this.numericUpDownTabSizeHeight.TabIndex = 6;
			this.numericUpDownTabSizeHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.numericUpDownTabSizeHeight, "この設定は次回起動後有効になります");
			this.numericUpDownTabSizeHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownTabSizeHeight.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownTabSizeHeight.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// radioButtonTabAutoSize
			// 
			this.radioButtonTabAutoSize.AutoSize = true;
			this.radioButtonTabAutoSize.Checked = true;
			this.radioButtonTabAutoSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonTabAutoSize.Location = new System.Drawing.Point(4, 8);
			this.radioButtonTabAutoSize.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonTabAutoSize.Name = "radioButtonTabAutoSize";
			this.radioButtonTabAutoSize.Size = new System.Drawing.Size(159, 17);
			this.radioButtonTabAutoSize.TabIndex = 0;
			this.radioButtonTabAutoSize.TabStop = true;
			this.radioButtonTabAutoSize.Text = "自動で文字の幅に合わせる";
			// 
			// label50
			// 
			this.label50.AutoSize = true;
			this.label50.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label50.Location = new System.Drawing.Point(181, 52);
			this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label50.Name = "label50";
			this.label50.Size = new System.Drawing.Size(27, 12);
			this.label50.TabIndex = 5;
			this.label50.Text = "高さ:";
			// 
			// label57
			// 
			this.label57.AutoSize = true;
			this.label57.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label57.Location = new System.Drawing.Point(21, 159);
			this.label57.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label57.Name = "label57";
			this.label57.Size = new System.Drawing.Size(58, 12);
			this.label57.TabIndex = 3;
			this.label57.Text = "スレッドタブ:";
			this.toolTip.SetToolTip(this.label57, "この設定は次回起動後有効になります");
			// 
			// label53
			// 
			this.label53.AutoSize = true;
			this.label53.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label53.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label53.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
			this.label53.Location = new System.Drawing.Point(16, 12);
			this.label53.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(153, 12);
			this.label53.TabIndex = 0;
			this.label53.Text = "次回起動時から有効になります";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label28.Location = new System.Drawing.Point(16, 78);
			this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(66, 12);
			this.label28.TabIndex = 1;
			this.label28.Text = "スレ一覧タブ:";
			this.toolTip.SetToolTip(this.label28, "この設定は次回起動後有効になります");
			// 
			// tabPageTableDesign
			// 
			this.tabPageTableDesign.Controls.Add(this.buttonRemoveColor);
			this.tabPageTableDesign.Controls.Add(this.buttonAddColor);
			this.tabPageTableDesign.Controls.Add(this.label52);
			this.tabPageTableDesign.Controls.Add(this.listViewColorList);
			this.tabPageTableDesign.Controls.Add(this.label42);
			this.tabPageTableDesign.Controls.Add(this.numericUpDownTableFontSize);
			this.tabPageTableDesign.Controls.Add(this.label43);
			this.tabPageTableDesign.Controls.Add(this.comboBoxTableFonts);
			this.tabPageTableDesign.Controls.Add(this.labelColorTableBoard);
			this.tabPageTableDesign.Controls.Add(this.labelColorTableCate);
			this.tabPageTableDesign.Controls.Add(this.checkBoxTableColoring);
			this.tabPageTableDesign.Controls.Add(this.checkBoxTableHideIcon);
			this.tabPageTableDesign.ImageIndex = 0;
			this.tabPageTableDesign.Location = new System.Drawing.Point(4, 27);
			this.tabPageTableDesign.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageTableDesign.Name = "tabPageTableDesign";
			this.tabPageTableDesign.Size = new System.Drawing.Size(454, 267);
			this.tabPageTableDesign.TabIndex = 0;
			this.tabPageTableDesign.Text = "板一覧";
			// 
			// buttonRemoveColor
			// 
			this.buttonRemoveColor.AutoSize = true;
			this.buttonRemoveColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRemoveColor.Location = new System.Drawing.Point(298, 152);
			this.buttonRemoveColor.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRemoveColor.Name = "buttonRemoveColor";
			this.buttonRemoveColor.Size = new System.Drawing.Size(58, 24);
			this.buttonRemoveColor.TabIndex = 0;
			this.buttonRemoveColor.Text = "削除";
			this.buttonRemoveColor.Visible = false;
			this.buttonRemoveColor.Click += new System.EventHandler(this.buttonRemoveColor_Click);
			// 
			// buttonAddColor
			// 
			this.buttonAddColor.AutoSize = true;
			this.buttonAddColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAddColor.Location = new System.Drawing.Point(298, 128);
			this.buttonAddColor.Margin = new System.Windows.Forms.Padding(2);
			this.buttonAddColor.Name = "buttonAddColor";
			this.buttonAddColor.Size = new System.Drawing.Size(58, 24);
			this.buttonAddColor.TabIndex = 11;
			this.buttonAddColor.Text = "追加";
			this.buttonAddColor.Visible = false;
			this.buttonAddColor.Click += new System.EventHandler(this.buttonAddColor_Click);
			// 
			// label52
			// 
			this.label52.Location = new System.Drawing.Point(16, 84);
			this.label52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label52.Name = "label52";
			this.label52.Size = new System.Drawing.Size(76, 12);
			this.label52.TabIndex = 9;
			this.label52.Text = "個別に配色";
			this.label52.Visible = false;
			// 
			// listViewColorList
			// 
			this.listViewColorList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listViewColorList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderType,
            this.columnHeaderForeColor,
            this.columnHeaderBackColor});
			this.listViewColorList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewColorList.Location = new System.Drawing.Point(13, 100);
			this.listViewColorList.Margin = new System.Windows.Forms.Padding(2);
			this.listViewColorList.MultiSelect = false;
			this.listViewColorList.Name = "listViewColorList";
			this.listViewColorList.Size = new System.Drawing.Size(282, 72);
			this.listViewColorList.SmallImageList = this.imageList;
			this.listViewColorList.TabIndex = 10;
			this.listViewColorList.UseCompatibleStateImageBehavior = false;
			this.listViewColorList.View = System.Windows.Forms.View.Details;
			this.listViewColorList.Visible = false;
			// 
			// columnHeaderName
			// 
			this.columnHeaderName.Text = "文字列";
			this.columnHeaderName.Width = 84;
			// 
			// columnHeaderType
			// 
			this.columnHeaderType.Text = "種類";
			// 
			// columnHeaderForeColor
			// 
			this.columnHeaderForeColor.Text = "文字色";
			this.columnHeaderForeColor.Width = 48;
			// 
			// columnHeaderBackColor
			// 
			this.columnHeaderBackColor.Text = "背景色";
			this.columnHeaderBackColor.Width = 47;
			// 
			// label42
			// 
			this.label42.AutoSize = true;
			this.label42.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label42.Location = new System.Drawing.Point(210, 60);
			this.label42.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(34, 12);
			this.label42.TabIndex = 6;
			this.label42.Text = "サイズ";
			// 
			// numericUpDownTableFontSize
			// 
			this.numericUpDownTableFontSize.AutoSize = true;
			this.numericUpDownTableFontSize.BackColor = System.Drawing.SystemColors.Window;
			this.numericUpDownTableFontSize.ForeColor = System.Drawing.SystemColors.WindowText;
			this.numericUpDownTableFontSize.Location = new System.Drawing.Point(256, 56);
			this.numericUpDownTableFontSize.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownTableFontSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numericUpDownTableFontSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.numericUpDownTableFontSize.Name = "numericUpDownTableFontSize";
			this.numericUpDownTableFontSize.Size = new System.Drawing.Size(55, 19);
			this.numericUpDownTableFontSize.TabIndex = 7;
			this.numericUpDownTableFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownTableFontSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.numericUpDownTableFontSize.ValueChanged += new System.EventHandler(this.TableFontUpdate);
			this.numericUpDownTableFontSize.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownTableFontSize.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// label43
			// 
			this.label43.AutoSize = true;
			this.label43.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label43.Location = new System.Drawing.Point(16, 60);
			this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(38, 12);
			this.label43.TabIndex = 4;
			this.label43.Text = "フォント";
			// 
			// comboBoxTableFonts
			// 
			this.comboBoxTableFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTableFonts.Location = new System.Drawing.Point(68, 56);
			this.comboBoxTableFonts.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxTableFonts.Name = "comboBoxTableFonts";
			this.comboBoxTableFonts.Size = new System.Drawing.Size(131, 20);
			this.comboBoxTableFonts.TabIndex = 5;
			this.comboBoxTableFonts.SelectedIndexChanged += new System.EventHandler(this.TableFontUpdate);
			// 
			// labelColorTableBoard
			// 
			this.labelColorTableBoard.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorTableBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorTableBoard.Location = new System.Drawing.Point(236, 28);
			this.labelColorTableBoard.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorTableBoard.Name = "labelColorTableBoard";
			this.labelColorTableBoard.Size = new System.Drawing.Size(93, 19);
			this.labelColorTableBoard.TabIndex = 3;
			this.labelColorTableBoard.Text = "板の背景色";
			this.labelColorTableBoard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorTableBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_BackColorChange);
			// 
			// labelColorTableCate
			// 
			this.labelColorTableCate.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorTableCate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorTableCate.Location = new System.Drawing.Point(134, 28);
			this.labelColorTableCate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorTableCate.Name = "labelColorTableCate";
			this.labelColorTableCate.Size = new System.Drawing.Size(93, 19);
			this.labelColorTableCate.TabIndex = 2;
			this.labelColorTableCate.Text = "カテゴリ背景色";
			this.labelColorTableCate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorTableCate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_BackColorChange);
			// 
			// checkBoxTableColoring
			// 
			this.checkBoxTableColoring.AutoSize = true;
			this.checkBoxTableColoring.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxTableColoring.Location = new System.Drawing.Point(26, 28);
			this.checkBoxTableColoring.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxTableColoring.Name = "checkBoxTableColoring";
			this.checkBoxTableColoring.Size = new System.Drawing.Size(98, 17);
			this.checkBoxTableColoring.TabIndex = 1;
			this.checkBoxTableColoring.Text = "一覧の色分け";
			// 
			// checkBoxTableHideIcon
			// 
			this.checkBoxTableHideIcon.AutoSize = true;
			this.checkBoxTableHideIcon.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxTableHideIcon.Location = new System.Drawing.Point(26, 8);
			this.checkBoxTableHideIcon.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxTableHideIcon.Name = "checkBoxTableHideIcon";
			this.checkBoxTableHideIcon.Size = new System.Drawing.Size(127, 17);
			this.checkBoxTableHideIcon.TabIndex = 0;
			this.checkBoxTableHideIcon.Text = "アイコンを表示しない";
			// 
			// tabPageListDesign
			// 
			this.tabPageListDesign.Controls.Add(this.checkBoxListForce_Underline);
			this.tabPageListDesign.Controls.Add(this.checkBoxListForce_Bold);
			this.tabPageListDesign.Controls.Add(this.labelColorListForcible);
			this.tabPageListDesign.Controls.Add(this.label35);
			this.tabPageListDesign.Controls.Add(this.label36);
			this.tabPageListDesign.Controls.Add(this.label34);
			this.tabPageListDesign.Controls.Add(this.label33);
			this.tabPageListDesign.Controls.Add(this.checkBoxListRecent_UnderLine);
			this.tabPageListDesign.Controls.Add(this.checkBoxListRecent_Bold);
			this.tabPageListDesign.Controls.Add(this.checkBoxListDat_UnderLine);
			this.tabPageListDesign.Controls.Add(this.checkBoxListDat_Bold);
			this.tabPageListDesign.Controls.Add(this.checkBoxListGot_UnderLine);
			this.tabPageListDesign.Controls.Add(this.checkBoxListGot_Bold);
			this.tabPageListDesign.Controls.Add(this.checkBoxListUp_UnderLine);
			this.tabPageListDesign.Controls.Add(this.checkBoxListUp_Bold);
			this.tabPageListDesign.Controls.Add(this.checkBoxListNew_UnderLine);
			this.tabPageListDesign.Controls.Add(this.checkBoxListNew_Bold);
			this.tabPageListDesign.Controls.Add(this.checkBoxListDef_UnderLine);
			this.tabPageListDesign.Controls.Add(this.checkBoxListDef_Bold);
			this.tabPageListDesign.Controls.Add(this.label32);
			this.tabPageListDesign.Controls.Add(this.numericUpDownListFontSize);
			this.tabPageListDesign.Controls.Add(this.label31);
			this.tabPageListDesign.Controls.Add(this.comboBoxListFonts);
			this.tabPageListDesign.Controls.Add(this.labelColorListRecent);
			this.tabPageListDesign.Controls.Add(this.checkBoxListColoring);
			this.tabPageListDesign.Controls.Add(this.labelColorListGot);
			this.tabPageListDesign.Controls.Add(this.labelColorListUp);
			this.tabPageListDesign.Controls.Add(this.labelColorListDat);
			this.tabPageListDesign.Controls.Add(this.labelColorListDef);
			this.tabPageListDesign.Controls.Add(this.labelColorListNew);
			this.tabPageListDesign.Controls.Add(this.labelColorListBack0);
			this.tabPageListDesign.Controls.Add(this.labelColorListBack1);
			this.tabPageListDesign.ImageIndex = 1;
			this.tabPageListDesign.Location = new System.Drawing.Point(4, 27);
			this.tabPageListDesign.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageListDesign.Name = "tabPageListDesign";
			this.tabPageListDesign.Size = new System.Drawing.Size(454, 267);
			this.tabPageListDesign.TabIndex = 1;
			this.tabPageListDesign.Text = "スレッド一覧";
			// 
			// checkBoxListForce_Underline
			// 
			this.checkBoxListForce_Underline.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListForce_Underline.Location = new System.Drawing.Point(208, 164);
			this.checkBoxListForce_Underline.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListForce_Underline.Name = "checkBoxListForce_Underline";
			this.checkBoxListForce_Underline.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListForce_Underline.TabIndex = 31;
			this.checkBoxListForce_Underline.CheckedChanged += new System.EventHandler(this.DesignUnderLine_CheckedChanged);
			// 
			// checkBoxListForce_Bold
			// 
			this.checkBoxListForce_Bold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListForce_Bold.Location = new System.Drawing.Point(187, 164);
			this.checkBoxListForce_Bold.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListForce_Bold.Name = "checkBoxListForce_Bold";
			this.checkBoxListForce_Bold.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListForce_Bold.TabIndex = 30;
			this.checkBoxListForce_Bold.CheckedChanged += new System.EventHandler(this.DesignBold_CheckedChanged);
			// 
			// labelColorListForcible
			// 
			this.labelColorListForcible.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListForcible.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListForcible.Location = new System.Drawing.Point(25, 162);
			this.labelColorListForcible.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListForcible.Name = "labelColorListForcible";
			this.labelColorListForcible.Size = new System.Drawing.Size(148, 19);
			this.labelColorListForcible.TabIndex = 29;
			this.labelColorListForcible.Text = "最も勢いのあるスレッド";
			this.labelColorListForcible.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListForcible.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ColorChange);
			// 
			// label35
			// 
			this.label35.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label35.Location = new System.Drawing.Point(307, 68);
			this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(16, 12);
			this.label35.TabIndex = 19;
			this.label35.Text = "線";
			// 
			// label36
			// 
			this.label36.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label36.Location = new System.Drawing.Point(286, 68);
			this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(16, 12);
			this.label36.TabIndex = 18;
			this.label36.Text = "太";
			// 
			// label34
			// 
			this.label34.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label34.Location = new System.Drawing.Point(147, 68);
			this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(16, 12);
			this.label34.TabIndex = 8;
			this.label34.Text = "線";
			// 
			// label33
			// 
			this.label33.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label33.Location = new System.Drawing.Point(126, 68);
			this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(16, 12);
			this.label33.TabIndex = 7;
			this.label33.Text = "太";
			// 
			// checkBoxListRecent_UnderLine
			// 
			this.checkBoxListRecent_UnderLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListRecent_UnderLine.Location = new System.Drawing.Point(307, 110);
			this.checkBoxListRecent_UnderLine.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListRecent_UnderLine.Name = "checkBoxListRecent_UnderLine";
			this.checkBoxListRecent_UnderLine.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListRecent_UnderLine.TabIndex = 28;
			this.checkBoxListRecent_UnderLine.CheckedChanged += new System.EventHandler(this.DesignUnderLine_CheckedChanged);
			// 
			// checkBoxListRecent_Bold
			// 
			this.checkBoxListRecent_Bold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListRecent_Bold.Location = new System.Drawing.Point(286, 110);
			this.checkBoxListRecent_Bold.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListRecent_Bold.Name = "checkBoxListRecent_Bold";
			this.checkBoxListRecent_Bold.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListRecent_Bold.TabIndex = 27;
			this.checkBoxListRecent_Bold.CheckedChanged += new System.EventHandler(this.DesignBold_CheckedChanged);
			// 
			// checkBoxListDat_UnderLine
			// 
			this.checkBoxListDat_UnderLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListDat_UnderLine.Location = new System.Drawing.Point(307, 132);
			this.checkBoxListDat_UnderLine.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListDat_UnderLine.Name = "checkBoxListDat_UnderLine";
			this.checkBoxListDat_UnderLine.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListDat_UnderLine.TabIndex = 25;
			this.checkBoxListDat_UnderLine.CheckedChanged += new System.EventHandler(this.DesignUnderLine_CheckedChanged);
			// 
			// checkBoxListDat_Bold
			// 
			this.checkBoxListDat_Bold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListDat_Bold.Location = new System.Drawing.Point(286, 132);
			this.checkBoxListDat_Bold.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListDat_Bold.Name = "checkBoxListDat_Bold";
			this.checkBoxListDat_Bold.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListDat_Bold.TabIndex = 24;
			this.checkBoxListDat_Bold.CheckedChanged += new System.EventHandler(this.DesignBold_CheckedChanged);
			// 
			// checkBoxListGot_UnderLine
			// 
			this.checkBoxListGot_UnderLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListGot_UnderLine.Location = new System.Drawing.Point(147, 132);
			this.checkBoxListGot_UnderLine.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListGot_UnderLine.Name = "checkBoxListGot_UnderLine";
			this.checkBoxListGot_UnderLine.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListGot_UnderLine.TabIndex = 22;
			this.checkBoxListGot_UnderLine.CheckedChanged += new System.EventHandler(this.DesignUnderLine_CheckedChanged);
			// 
			// checkBoxListGot_Bold
			// 
			this.checkBoxListGot_Bold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListGot_Bold.Location = new System.Drawing.Point(126, 132);
			this.checkBoxListGot_Bold.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListGot_Bold.Name = "checkBoxListGot_Bold";
			this.checkBoxListGot_Bold.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListGot_Bold.TabIndex = 21;
			this.checkBoxListGot_Bold.CheckedChanged += new System.EventHandler(this.DesignBold_CheckedChanged);
			// 
			// checkBoxListUp_UnderLine
			// 
			this.checkBoxListUp_UnderLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListUp_UnderLine.Location = new System.Drawing.Point(147, 110);
			this.checkBoxListUp_UnderLine.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListUp_UnderLine.Name = "checkBoxListUp_UnderLine";
			this.checkBoxListUp_UnderLine.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListUp_UnderLine.TabIndex = 17;
			this.checkBoxListUp_UnderLine.CheckedChanged += new System.EventHandler(this.DesignUnderLine_CheckedChanged);
			// 
			// checkBoxListUp_Bold
			// 
			this.checkBoxListUp_Bold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListUp_Bold.Location = new System.Drawing.Point(126, 110);
			this.checkBoxListUp_Bold.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListUp_Bold.Name = "checkBoxListUp_Bold";
			this.checkBoxListUp_Bold.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListUp_Bold.TabIndex = 16;
			this.checkBoxListUp_Bold.CheckedChanged += new System.EventHandler(this.DesignBold_CheckedChanged);
			// 
			// checkBoxListNew_UnderLine
			// 
			this.checkBoxListNew_UnderLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListNew_UnderLine.Location = new System.Drawing.Point(307, 88);
			this.checkBoxListNew_UnderLine.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListNew_UnderLine.Name = "checkBoxListNew_UnderLine";
			this.checkBoxListNew_UnderLine.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListNew_UnderLine.TabIndex = 14;
			this.checkBoxListNew_UnderLine.CheckedChanged += new System.EventHandler(this.DesignUnderLine_CheckedChanged);
			// 
			// checkBoxListNew_Bold
			// 
			this.checkBoxListNew_Bold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListNew_Bold.Location = new System.Drawing.Point(286, 88);
			this.checkBoxListNew_Bold.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListNew_Bold.Name = "checkBoxListNew_Bold";
			this.checkBoxListNew_Bold.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListNew_Bold.TabIndex = 13;
			this.checkBoxListNew_Bold.CheckedChanged += new System.EventHandler(this.DesignBold_CheckedChanged);
			// 
			// checkBoxListDef_UnderLine
			// 
			this.checkBoxListDef_UnderLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListDef_UnderLine.Location = new System.Drawing.Point(147, 88);
			this.checkBoxListDef_UnderLine.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListDef_UnderLine.Name = "checkBoxListDef_UnderLine";
			this.checkBoxListDef_UnderLine.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListDef_UnderLine.TabIndex = 11;
			this.checkBoxListDef_UnderLine.CheckedChanged += new System.EventHandler(this.DesignUnderLine_CheckedChanged);
			// 
			// checkBoxListDef_Bold
			// 
			this.checkBoxListDef_Bold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListDef_Bold.Location = new System.Drawing.Point(126, 88);
			this.checkBoxListDef_Bold.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListDef_Bold.Name = "checkBoxListDef_Bold";
			this.checkBoxListDef_Bold.Size = new System.Drawing.Size(16, 16);
			this.checkBoxListDef_Bold.TabIndex = 10;
			this.checkBoxListDef_Bold.CheckedChanged += new System.EventHandler(this.DesignBold_CheckedChanged);
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label32.Location = new System.Drawing.Point(210, 44);
			this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(34, 12);
			this.label32.TabIndex = 5;
			this.label32.Text = "サイズ";
			// 
			// numericUpDownListFontSize
			// 
			this.numericUpDownListFontSize.AutoSize = true;
			this.numericUpDownListFontSize.BackColor = System.Drawing.SystemColors.Window;
			this.numericUpDownListFontSize.Location = new System.Drawing.Point(252, 40);
			this.numericUpDownListFontSize.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownListFontSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numericUpDownListFontSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.numericUpDownListFontSize.Name = "numericUpDownListFontSize";
			this.numericUpDownListFontSize.Size = new System.Drawing.Size(51, 19);
			this.numericUpDownListFontSize.TabIndex = 6;
			this.numericUpDownListFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownListFontSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.numericUpDownListFontSize.ValueChanged += new System.EventHandler(this.FontUpdate);
			this.numericUpDownListFontSize.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDownCtrl_Validating);
			this.numericUpDownListFontSize.Validated += new System.EventHandler(this.CommonValidated);
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label31.Location = new System.Drawing.Point(26, 44);
			this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(38, 12);
			this.label31.TabIndex = 3;
			this.label31.Text = "フォント";
			// 
			// comboBoxListFonts
			// 
			this.comboBoxListFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxListFonts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxListFonts.Location = new System.Drawing.Point(71, 40);
			this.comboBoxListFonts.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxListFonts.Name = "comboBoxListFonts";
			this.comboBoxListFonts.Size = new System.Drawing.Size(132, 20);
			this.comboBoxListFonts.TabIndex = 4;
			this.comboBoxListFonts.SelectedIndexChanged += new System.EventHandler(this.FontUpdate);
			// 
			// labelColorListRecent
			// 
			this.labelColorListRecent.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListRecent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListRecent.Location = new System.Drawing.Point(184, 108);
			this.labelColorListRecent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListRecent.Name = "labelColorListRecent";
			this.labelColorListRecent.Size = new System.Drawing.Size(94, 19);
			this.labelColorListRecent.TabIndex = 26;
			this.labelColorListRecent.Text = "最近立ったスレ";
			this.labelColorListRecent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListRecent.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ColorChange);
			// 
			// checkBoxListColoring
			// 
			this.checkBoxListColoring.AutoSize = true;
			this.checkBoxListColoring.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxListColoring.Location = new System.Drawing.Point(34, 12);
			this.checkBoxListColoring.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxListColoring.Name = "checkBoxListColoring";
			this.checkBoxListColoring.Size = new System.Drawing.Size(110, 17);
			this.checkBoxListColoring.TabIndex = 0;
			this.checkBoxListColoring.Text = "背景色の色分け";
			// 
			// labelColorListGot
			// 
			this.labelColorListGot.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListGot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListGot.Location = new System.Drawing.Point(26, 132);
			this.labelColorListGot.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListGot.Name = "labelColorListGot";
			this.labelColorListGot.Size = new System.Drawing.Size(93, 19);
			this.labelColorListGot.TabIndex = 20;
			this.labelColorListGot.Text = "全既得スレッド";
			this.labelColorListGot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListGot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ColorChange);
			// 
			// labelColorListUp
			// 
			this.labelColorListUp.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListUp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListUp.Location = new System.Drawing.Point(26, 108);
			this.labelColorListUp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListUp.Name = "labelColorListUp";
			this.labelColorListUp.Size = new System.Drawing.Size(93, 19);
			this.labelColorListUp.TabIndex = 15;
			this.labelColorListUp.Text = "更新スレッド";
			this.labelColorListUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ColorChange);
			// 
			// labelColorListDat
			// 
			this.labelColorListDat.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListDat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListDat.Location = new System.Drawing.Point(184, 132);
			this.labelColorListDat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListDat.Name = "labelColorListDat";
			this.labelColorListDat.Size = new System.Drawing.Size(94, 19);
			this.labelColorListDat.TabIndex = 23;
			this.labelColorListDat.Text = "dat落ちスレッド";
			this.labelColorListDat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListDat.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ColorChange);
			// 
			// labelColorListDef
			// 
			this.labelColorListDef.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListDef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListDef.Location = new System.Drawing.Point(26, 84);
			this.labelColorListDef.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListDef.Name = "labelColorListDef";
			this.labelColorListDef.Size = new System.Drawing.Size(93, 19);
			this.labelColorListDef.TabIndex = 9;
			this.labelColorListDef.Text = "通常文字";
			this.labelColorListDef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListDef.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ColorChange);
			// 
			// labelColorListNew
			// 
			this.labelColorListNew.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListNew.Location = new System.Drawing.Point(184, 84);
			this.labelColorListNew.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListNew.Name = "labelColorListNew";
			this.labelColorListNew.Size = new System.Drawing.Size(94, 19);
			this.labelColorListNew.TabIndex = 12;
			this.labelColorListNew.Text = "新着スレッド";
			this.labelColorListNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListNew.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ColorChange);
			// 
			// labelColorListBack0
			// 
			this.labelColorListBack0.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListBack0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListBack0.Location = new System.Drawing.Point(155, 12);
			this.labelColorListBack0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListBack0.Name = "labelColorListBack0";
			this.labelColorListBack0.Size = new System.Drawing.Size(81, 19);
			this.labelColorListBack0.TabIndex = 1;
			this.labelColorListBack0.Text = "奇数行";
			this.labelColorListBack0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListBack0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_BackColorChange);
			// 
			// labelColorListBack1
			// 
			this.labelColorListBack1.BackColor = System.Drawing.SystemColors.Window;
			this.labelColorListBack1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorListBack1.Location = new System.Drawing.Point(244, 12);
			this.labelColorListBack1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelColorListBack1.Name = "labelColorListBack1";
			this.labelColorListBack1.Size = new System.Drawing.Size(84, 19);
			this.labelColorListBack1.TabIndex = 2;
			this.labelColorListBack1.Text = "偶数行";
			this.labelColorListBack1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelColorListBack1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_BackColorChange);
			// 
			// tabPageThreadDesign
			// 
			this.tabPageThreadDesign.Controls.Add(this.pictureBox);
			this.tabPageThreadDesign.Controls.Add(this.label12);
			this.tabPageThreadDesign.Controls.Add(this.comboBoxSkins);
			this.tabPageThreadDesign.ImageIndex = 2;
			this.tabPageThreadDesign.Location = new System.Drawing.Point(4, 27);
			this.tabPageThreadDesign.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageThreadDesign.Name = "tabPageThreadDesign";
			this.tabPageThreadDesign.Size = new System.Drawing.Size(454, 267);
			this.tabPageThreadDesign.TabIndex = 2;
			this.tabPageThreadDesign.Text = "スレッド";
			// 
			// pictureBox
			// 
			this.pictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox.Location = new System.Drawing.Point(29, 44);
			this.pictureBox.Margin = new System.Windows.Forms.Padding(2);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(316, 131);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox.TabIndex = 2;
			this.pictureBox.TabStop = false;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label12.Location = new System.Drawing.Point(29, 16);
			this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(119, 12);
			this.label12.TabIndex = 0;
			this.label12.Text = "スキンを選択してください:";
			// 
			// comboBoxSkins
			// 
			this.comboBoxSkins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSkins.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxSkins.Location = new System.Drawing.Point(184, 12);
			this.comboBoxSkins.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxSkins.Name = "comboBoxSkins";
			this.comboBoxSkins.Size = new System.Drawing.Size(161, 20);
			this.comboBoxSkins.TabIndex = 1;
			this.comboBoxSkins.SelectedIndexChanged += new System.EventHandler(this.comboBoxSkins_SelectedIndexChanged);
			// 
			// tabPageWrite
			// 
			this.tabPageWrite.Controls.Add(this.label69);
			this.tabPageWrite.Controls.Add(this.label67);
			this.tabPageWrite.Controls.Add(this.numericUpDownWriteFontSize);
			this.tabPageWrite.Controls.Add(this.label68);
			this.tabPageWrite.Controls.Add(this.comboBoxWriteFonts);
			this.tabPageWrite.ImageIndex = 8;
			this.tabPageWrite.Location = new System.Drawing.Point(4, 27);
			this.tabPageWrite.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageWrite.Name = "tabPageWrite";
			this.tabPageWrite.Size = new System.Drawing.Size(454, 267);
			this.tabPageWrite.TabIndex = 4;
			this.tabPageWrite.Text = "書き込み";
			// 
			// label69
			// 
			this.label69.Location = new System.Drawing.Point(37, 35);
			this.label69.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label69.Name = "label69";
			this.label69.Size = new System.Drawing.Size(147, 14);
			this.label69.TabIndex = 11;
			this.label69.Text = "書き込み本文のフォント";
			// 
			// label67
			// 
			this.label67.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label67.Location = new System.Drawing.Point(231, 60);
			this.label67.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label67.Name = "label67";
			this.label67.Size = new System.Drawing.Size(42, 12);
			this.label67.TabIndex = 9;
			this.label67.Text = "サイズ";
			// 
			// numericUpDownWriteFontSize
			// 
			this.numericUpDownWriteFontSize.BackColor = System.Drawing.SystemColors.Window;
			this.numericUpDownWriteFontSize.Location = new System.Drawing.Point(273, 55);
			this.numericUpDownWriteFontSize.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDownWriteFontSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numericUpDownWriteFontSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.numericUpDownWriteFontSize.Name = "numericUpDownWriteFontSize";
			this.numericUpDownWriteFontSize.Size = new System.Drawing.Size(51, 19);
			this.numericUpDownWriteFontSize.TabIndex = 10;
			this.numericUpDownWriteFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownWriteFontSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
			// 
			// label68
			// 
			this.label68.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label68.Location = new System.Drawing.Point(47, 60);
			this.label68.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label68.Name = "label68";
			this.label68.Size = new System.Drawing.Size(42, 12);
			this.label68.TabIndex = 7;
			this.label68.Text = "フォント";
			// 
			// comboBoxWriteFonts
			// 
			this.comboBoxWriteFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxWriteFonts.Location = new System.Drawing.Point(94, 55);
			this.comboBoxWriteFonts.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxWriteFonts.Name = "comboBoxWriteFonts";
			this.comboBoxWriteFonts.Size = new System.Drawing.Size(132, 20);
			this.comboBoxWriteFonts.TabIndex = 8;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(482, 429);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(88, 21);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(390, 429);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(88, 21);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			// 
			// buttonReset
			// 
			this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonReset.AutoSize = true;
			this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonReset.Location = new System.Drawing.Point(8, 427);
			this.buttonReset.Margin = new System.Windows.Forms.Padding(2);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(118, 21);
			this.buttonReset.TabIndex = 3;
			this.buttonReset.Text = "初期値に設定";
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
			// 
			// colorDialog
			// 
			this.colorDialog.FullOpen = true;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			this.errorProvider.DataMember = "";
			// 
			// checkBoxHighLevelMatch
			// 
			this.checkBoxHighLevelMatch.AutoSize = true;
			this.checkBoxHighLevelMatch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxHighLevelMatch.Location = new System.Drawing.Point(30, 43);
			this.checkBoxHighLevelMatch.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxHighLevelMatch.Name = "checkBoxHighLevelMatch";
			this.checkBoxHighLevelMatch.Size = new System.Drawing.Size(118, 17);
			this.checkBoxHighLevelMatch.TabIndex = 2;
			this.checkBoxHighLevelMatch.Text = "一致精度を高める";
			this.toolTip.SetToolTip(this.checkBoxHighLevelMatch, "次スレ検索時に一致レベルの低いスレッドは検索結果に含めないようにします");
			// 
			// OptionDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(575, 452);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "環境設定";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.OptionDialog_Closing);
			this.tabControl.ResumeLayout(false);
			this.tabPageGeneral.ResumeLayout(false);
			this.tabPageGeneral.PerformLayout();
			this.tabPageFunction.ResumeLayout(false);
			this.tabPageFunction.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownImageSizeLimit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThumSize)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPageAction.ResumeLayout(false);
			this.tabPageAction.PerformLayout();
			this.groupBox15.ResumeLayout(false);
			this.groupBox15.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecvBuffSize)).EndInit();
			this.groupBox13.ResumeLayout(false);
			this.groupBox13.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.groupBox12.ResumeLayout(false);
			this.groupBox12.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox11.ResumeLayout(false);
			this.groupBox11.PerformLayout();
			this.tabPageMouse.ResumeLayout(false);
			this.tabPageMouse.PerformLayout();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.tabPageThread.ResumeLayout(false);
			this.tabPageThread.PerformLayout();
			this.groupBox14.ResumeLayout(false);
			this.groupBox14.PerformLayout();
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarHistory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarAutoReload)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownClosedHistory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoReloadInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownViewResCount)).EndInit();
			this.tabPagePopup.ResumeLayout(false);
			this.tabPagePopup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownImagePopupHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownImagePopupWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopupMaxHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopupMaxWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopupInterval)).EndInit();
			this.tabPagePost.ResumeLayout(false);
			this.tabPagePost.PerformLayout();
			this.tabPageKotehan.ResumeLayout(false);
			this.tabPageKotehan.PerformLayout();
			this.groupBoxKotehan.ResumeLayout(false);
			this.groupBoxKotehan.PerformLayout();
			this.tabPageProxy.ResumeLayout(false);
			this.tabPageProxy.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecvProxyPort)).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownSendProxyPort)).EndInit();
			this.tabPageSound.ResumeLayout(false);
			this.tabPageDesign.ResumeLayout(false);
			this.tabControlDesign.ResumeLayout(false);
			this.tabPageDesignCommon.ResumeLayout(false);
			this.tabPageDesignCommon.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreadTabSizeHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreadTabSizeWidth)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabSizeWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabSizeHeight)).EndInit();
			this.tabPageTableDesign.ResumeLayout(false);
			this.tabPageTableDesign.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableFontSize)).EndInit();
			this.tabPageListDesign.ResumeLayout(false);
			this.tabPageListDesign.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownListFontSize)).EndInit();
			this.tabPageThreadDesign.ResumeLayout(false);
			this.tabPageThreadDesign.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.tabPageWrite.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteFontSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Form Events


		private void OptionDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
				Get(settings);
		}
		#endregion

		// 設定を初期化
		private void buttonReset_Click(object sender, System.EventArgs e)
		{
			Settings newSettings = new Settings();

			Set(newSettings);
		}

		/// <summary>
		/// 指定した列挙型の値をコンボボックスに設定
		/// </summary>
		/// <param name="cb"></param>
		/// <param name="enumType"></param>
		private void EnumToComboBoxItems(ComboBox cb, Enum enumeration)
		{
			List<DictionaryEntry> list = new List<DictionaryEntry>();

			foreach (FieldInfo field in 
				enumeration.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				object[] attributes = 
					field.GetCustomAttributes(typeof(DescriptionAttribute), true);

				string displayName;

				if (attributes.Length == 0)
				{
					displayName = field.Name;
				}
				else
				{
					displayName = (attributes[0] as DescriptionAttribute).Description;
				}

				list.Add(new DictionaryEntry(displayName, Enum.Parse(enumeration.GetType(), field.Name)));
			}

			cb.DisplayMember = "Key";
			cb.ValueMember = "Value";
			cb.DataSource = list.ToArray();
		}

		#region Setメソッド、Getメソッド
		private void Set(Settings settings)
		{
			try
			{
				ThreadSettings thread = settings.Thread;
				PopupSettings popup = settings.Popup;
				NetworkSettings net = settings.Net;
				ViewSettings view = settings.View;
				PostSettings post = settings.Post;

				// 基本設定
				EnumToComboBoxItems(comboBoxForceValueOf, new ForceValueOf());
				comboBoxForceValueOf.SelectedValue = settings.ForceValueType;
				checkBoxUseGzipArchive.Checked = settings.UseGzipArchive;
				textBoxOnlineUpdateUrl.Text = settings.OnlineUpdateUrl;
				textBoxWebBrowserPath.Text = settings.WebBrowserPath;
				textBoxLogFolder.Text = settings.CacheFolderPath;
				textBoxDateFormat.Text = settings.DateFormat;
				textBoxResRefAnchor.Text = settings.ResRefAnchor;
				checkBoxTaskTray.Checked = settings.IsTasktray;

				checkBoxUseVisualStyle.Checked = settings.UseVisualStyle;

				// 投稿設定
				textBoxAddWriteSection.Text = settings.AddWriteSection;

				checkBoxAutoClose.Checked = post.AutoClosing;
				checkBoxShowCookieDialog.Checked = post.ShowCookieDialog;
				checkBoxSavePostHistory.Checked = post.SavePostHistory;
				checkBoxThreadKotehan.Checked = post.ThreadKotehan;
				checkBoxSamba24Check.Checked = post.Samba24Check;
				checkBoxMultiWriteDialog.Checked = post.MultiWriteDialog;
				checkBoxPostDlgMinimize.Checked = post.MinimizingDialog;
				checkBoxImeOn.Checked = post.ImeOn;

				if (!post.Be2chCookie.IsEmpty)
				{
					textBoxMdmd.Text = post.Be2chCookie.Mdmd;
					textBoxDmdm.Text = post.Be2chCookie.Dmdm;
				}

				// スレッド設定
				checkBoxAutoFillThread.Checked = view.AutoFillThread;
				checkBoxScrollToNewRes.Checked = thread.ScrollToNewRes;
				checkBoxAutoScrollOn.Checked = thread.AutoScrollOn;
				checkBoxAutoReloadOn.Checked = thread.AutoReloadOn;
				radioButtonViewAllRes.Checked = !thread.ViewResLimit;
				numericUpDownViewResCount.Value = thread.ViewResCount;
				numericUpDownClosedHistory.Value = trackBarHistory.Value = thread.ClosedHistoryCount;
				checkBoxColoringBackReference.Checked = thread.IsColoringBackReference;
				checkBoxAutoReloadCheckOnly.Checked = thread.AutoReloadCheckOnly;
				checkBoxTabSelectedAfterReload.Checked = thread.TabSelectedAfterReload;
				checkBoxAutoReloadAverage.Checked = thread.UseAutoReloadAverage;

				// 基本動作
				EnumToComboBoxItems(comboBoxPriority, new ThreadPriority());
				comboBoxPriority.SelectedValue = settings.Priority;
				checkBoxRecycleOverThread.Checked = settings.RecycleOverThread;
				checkBoxAlwaysSingleOpen.Checked = settings.AlwaysSingleOpen;
				checkBoxListAlwaysNewTab.Checked = settings.ListAlwaysNewTab;
				checkBoxThreadAlwaysNewTab.Checked = settings.ThreadAlwaysNewTab;
				radioButtonThreadPackageReception.Checked = net.PackageReception;
				numericUpDownRecvBuffSize.Value = net.BufferSize;

				EnumToComboBoxItems(comboBoxNewTabPos, new NewTabPosition());
				comboBoxNewTabPos.SelectedValue = settings.NewTabPosition;

				//radioButtonListPackageReception.Checked = net.ListPackageReception;
				radioButtonListPackageReception.Checked = true;

				checkBoxCloseMsgBox.Checked = settings.ClosingConfirm;
				checkBoxConnectionLimit.Checked = settings.ConnectionLimit;

				// オートリロードの単位は秒で表す
				numericUpDownAutoReloadInterval.Value =
					trackBarAutoReload.Value = thread.AutoReloadInterval / 1000;

				if (settings.TabCloseAfterSelection == TabCloseAfterSelectionMode.Left)
					radioButtonTabCloseAfterSelectionLeft.Checked = true;
				else
					radioButtonTabCloseAfterSelectionRight.Checked = true;

				// マウス動作
				EnumToComboBoxItems(comboBoxDoubleClick, new TabOperation());
				EnumToComboBoxItems(comboBoxWheelClick, new TabOperation());
				EnumToComboBoxItems(comboBoxListWheelClick, new ListOperation());
				EnumToComboBoxItems(comboBoxOpenMode, new OpenMode());
				comboBoxDoubleClick.SelectedValue = settings.Operate.TabDoubleClick;
				comboBoxWheelClick.SelectedValue = settings.Operate.TabWheelClick;
				comboBoxListWheelClick.SelectedValue = settings.Operate.ListWheelClick;
				comboBoxOpenMode.SelectedValue = settings.Operate.OpenMode;
				checkBoxWheelScroll.Checked = settings.Operate.EnabledTabWheelScroll;

				// 機能
				checkBoxImageViewer.Checked = settings.ImageViewer;
				checkBoxAutoOpenImage.Checked = settings.ImageViewer_AutoOpen;
				checkBoxNamePopup.Checked = settings.Thread.NameNumberPopup;
				checkBoxAutoUpdate.Checked = settings.UpdateCheck;
				checkBoxOpenStartupUrls.Checked = settings.OpenStartupUrls;
				checkBoxNextThreadChecker.Checked = settings.NextThreadChecker;
				checkBoxHighLevelMatch.Checked = settings.NextThreadChecker_HighLevelMatch;
				checkBoxVisibleNGAbone.Checked = settings.ABone.Visible;
				checkBoxChainABone.Checked = settings.ABone.Chain;
				checkBoxImageThumb.Checked = settings.Thumbnail.Visible;
				numericUpDownThumSize.Value = settings.Thumbnail.Height;
				checkBoxThumbnailIsLightMode.Checked = settings.Thumbnail.IsLightMode;
				numericUpDownImageSizeLimit.Value = settings.ImageCacheClient_SizeLimit / 1024; // 扱いやすいようにKB単位にする
				checkBoxAutoNGRes.Checked = settings.AutoNGRes;
				checkBoxEnsureVisibleBoard.Checked = settings.EnsureVisibleBoard;
				checkBoxNG924.Checked = settings.NG924;

				// ポップアップ
				EnumToComboBoxItems(comboBoxPopupPos, new PopupPosition());
				comboBoxPopupPos.SelectedValue = popup.Position;
				checkBoxOrigPopup.Checked = (popup.Style == PopupStyle.Text);
				checkBoxUrlPopup.Checked = (popup.UrlPopup & PopupState.Enable) != 0;
				checkBoxUrlPopupCtrlKeySwitch.Checked = (popup.UrlPopup & PopupState.KeySwitch) != 0;
				checkBoxImagePopup.Checked = (popup.ImagePopup & PopupState.Enable) != 0;
				checkBoxImagePopupCtrlSwitch.Checked = (popup.ImagePopup & PopupState.KeySwitch) != 0;
				checkBoxExPopup.Checked = popup.Extend;
				checkBoxClickedHide.Checked = popup.ClickedHide;
				checkBoxClickedHideResPopup.Checked = popup.ClickedHideResPopup;
				checkBoxUrlPopup.Checked = thread.UrlPopup;
				checkBoxUrlPopupCtrlKeySwitch.Checked = thread.UrlPopupOnCtrl;
				textBoxPopupRegex.Text = popup.ExtendPopupStr;
				numericUpDownPopupInterval.Value = popup.PopupInterval;
				numericUpDownPopupMaxHeight.Value = popup.Maximum.Height;
				numericUpDownPopupMaxWidth.Value = popup.Maximum.Width;
				numericUpDownImagePopupWidth.Value = popup.ImagePopupSize.Width;
				numericUpDownImagePopupHeight.Value = popup.ImagePopupSize.Height;

				// サウンド
				propertyGridSound.SelectedObject = settings.Sound;

				// デザイン

				EnumToComboBoxItems(comboBoxTabAppearance, new TabAppearance());
				comboBoxTabAppearance.SelectedValue = view.TabAppearance;

				radioButtonTabFixedSize.Checked = (view.ListTabSizeMode == TabSizeMode.Fixed);
				radioButtonTabFillRight.Checked = (view.ListTabSizeMode == TabSizeMode.FillToRight);
				radioButtonTabAutoSize.Checked = (view.ListTabSizeMode == TabSizeMode.Normal);

				radioButtonThreadTabFixed.Checked = (view.ThreadTabSizeMode == TabSizeMode.Fixed);
				radioButtonThreadTabFillRight.Checked = (view.ThreadTabSizeMode == TabSizeMode.FillToRight);
				radioButtonThreadTabAutoSize.Checked = (view.ThreadTabSizeMode == TabSizeMode.Normal);

				numericUpDownTabSizeWidth.Value = view.ListTabSize.Width;
				numericUpDownTabSizeHeight.Value = view.ListTabSize.Height;

				numericUpDownThreadTabSizeWidth.Value = view.ThreadTabSize.Width;
				numericUpDownThreadTabSizeHeight.Value = view.ThreadTabSize.Height;

				checkBoxTableHideIcon.Checked = settings.Design.Table.HideIcon;
				checkBoxTableColoring.Checked = settings.Design.Table.Coloring;
				checkBoxListColoring.Checked = settings.Design.List.Coloring;

				checkBoxIsHighlightActiveTab.Checked = !settings.Design.TabHighlightColor.IsEmpty;
				labelHighlightActiveColor.BackColor = settings.Design.TabHighlightColor;


				SetColors(settings.Design);

				// プロキシ
				#region
				if (net._RecvProxy.Uri != null && net._RecvProxy.Uri.IsAbsoluteUri)
				{
					textBoxRecvProxyHost.Text = net._RecvProxy.Uri.Host;
					numericUpDownRecvProxyPort.Value = net._RecvProxy.Uri.Port;

					checkBoxRecvProxyCredential.Checked = net._RecvProxy.Credential;
					textBoxRecvProxyUserID.Text = net._RecvProxy.UserName;
					textBoxRecvProxyPass.Text = net._RecvProxy.Password;
				}
				if (net._SendProxy.Uri != null && net._SendProxy.Uri.IsAbsoluteUri)
				{
					textBoxSendProxyHost.Text = net._SendProxy.Uri.Host;
					numericUpDownSendProxyPort.Value = net._SendProxy.Uri.Port;

					checkBoxSendProxyCredential.Checked = net._SendProxy.Credential;
					textBoxSendProxyUserID.Text = net._SendProxy.UserName;
					textBoxSendProxyPass.Text = net._SendProxy.Password;
				}
				#endregion
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		private void Get(Settings settings)
		{
			PostSettings post = settings.Post;
			ViewSettings view = settings.View;
			NetworkSettings net = settings.Net;
			PopupSettings popup = settings.Popup;
			ThreadSettings thread = settings.Thread;
			DesignSettings design = settings.Design;

			// 基本設定
			settings.ForceValueType = (ForceValueOf)comboBoxForceValueOf.SelectedValue;
			settings.UseGzipArchive = checkBoxUseGzipArchive.Checked;
			settings.OnlineUpdateUrl = textBoxOnlineUpdateUrl.Text;
			settings.WebBrowserPath = textBoxWebBrowserPath.Text;
			settings.DateFormat = textBoxDateFormat.Text;
			settings.ResRefAnchor = textBoxResRefAnchor.Text;
			settings.CacheFolderPath = textBoxLogFolder.Text;
			settings.IsTasktray = checkBoxTaskTray.Checked;
			settings.UseVisualStyle = checkBoxUseVisualStyle.Checked;

			// 投稿設定
			settings.AddWriteSection = textBoxAddWriteSection.Text;

			post.AutoClosing = checkBoxAutoClose.Checked;
			post.ShowCookieDialog = checkBoxShowCookieDialog.Checked;
			post.SavePostHistory = checkBoxSavePostHistory.Checked;
			post.ThreadKotehan = checkBoxThreadKotehan.Checked;
			post.Samba24Check = checkBoxSamba24Check.Checked;
			post.MultiWriteDialog = checkBoxMultiWriteDialog.Checked;
			post.ImeOn = checkBoxImeOn.Checked;
			post.MinimizingDialog = checkBoxPostDlgMinimize.Checked;

			post.Be2chCookie.Dmdm = textBoxDmdm.Text;
			post.Be2chCookie.Mdmd = textBoxMdmd.Text;

			// スレッド設定
			view.AutoFillThread = checkBoxAutoFillThread.Checked;
			thread.ScrollToNewRes = checkBoxScrollToNewRes.Checked;
			thread.AutoReloadOn = checkBoxAutoReloadOn.Checked;
			thread.AutoScrollOn = checkBoxAutoScrollOn.Checked;
			thread.ViewResCount = (int)numericUpDownViewResCount.Value;
			thread.ViewResLimit = !radioButtonViewAllRes.Checked;
			thread.ClosedHistoryCount = (int)numericUpDownClosedHistory.Value;
			thread.IsColoringBackReference = checkBoxColoringBackReference.Checked;
			thread.AutoReloadCheckOnly = checkBoxAutoReloadCheckOnly.Checked;
			thread.TabSelectedAfterReload = checkBoxTabSelectedAfterReload.Checked;
			thread.UseAutoReloadAverage = checkBoxAutoReloadAverage.Checked;

			// 基本動作
			settings.AlwaysSingleOpen = checkBoxAlwaysSingleOpen.Checked;
			settings.Priority = (ThreadPriority)comboBoxPriority.SelectedValue;
			settings.ListAlwaysNewTab = checkBoxListAlwaysNewTab.Checked;
			settings.ThreadAlwaysNewTab = checkBoxThreadAlwaysNewTab.Checked;
			settings.RecycleOverThread = checkBoxRecycleOverThread.Checked;
			net.ListPackageReception = radioButtonListPackageReception.Checked;
			net.PackageReception = radioButtonThreadPackageReception.Checked;
			net.BufferSize = (int)numericUpDownRecvBuffSize.Value;

			settings.ConnectionLimit = checkBoxConnectionLimit.Checked;
			settings.ClosingConfirm = checkBoxCloseMsgBox.Checked;

			settings.NewTabPosition = (NewTabPosition)comboBoxNewTabPos.SelectedValue;

			// 秒単位をミリ秒単位に直す
			thread.AutoReloadInterval = (int)numericUpDownAutoReloadInterval.Value * 1000;

			settings.TabCloseAfterSelection = radioButtonTabCloseAfterSelectionLeft.Checked ?
				TabCloseAfterSelectionMode.Left : TabCloseAfterSelectionMode.Right;

			// マウス動作
			settings.Operate.OpenMode = (OpenMode)comboBoxOpenMode.SelectedValue;
			settings.Operate.TabDoubleClick = (TabOperation)comboBoxDoubleClick.SelectedValue;
			settings.Operate.TabWheelClick = (TabOperation)comboBoxWheelClick.SelectedValue;
			settings.Operate.ListWheelClick = (ListOperation)comboBoxListWheelClick.SelectedValue;
			settings.Operate.EnabledTabWheelScroll = checkBoxWheelScroll.Checked;

			// 機能
			settings.ImageViewer = checkBoxImageViewer.Checked;
			settings.ImageViewer_AutoOpen = checkBoxAutoOpenImage.Checked;
			settings.Thread.NameNumberPopup = checkBoxNamePopup.Checked;
			settings.UpdateCheck = checkBoxAutoUpdate.Checked;
			settings.OpenStartupUrls = checkBoxOpenStartupUrls.Checked;
			settings.NextThreadChecker = checkBoxNextThreadChecker.Checked;
			settings.NextThreadChecker_HighLevelMatch = checkBoxHighLevelMatch.Checked;
			settings.ABone.Visible = checkBoxVisibleNGAbone.Checked;
			settings.ABone.Chain = checkBoxChainABone.Checked;
			settings.Thumbnail.Width = settings.Thumbnail.Height = (int)numericUpDownThumSize.Value;
			settings.Thumbnail.Visible = checkBoxImageThumb.Checked;
			settings.Thumbnail.IsLightMode = checkBoxThumbnailIsLightMode.Checked;
			settings.AutoNGRes = checkBoxAutoNGRes.Checked;
			settings.ImageCacheClient_SizeLimit = (int)numericUpDownImageSizeLimit.Value * 1024;
			settings.EnsureVisibleBoard = checkBoxEnsureVisibleBoard.Checked;
			settings.NG924 = checkBoxNG924.Checked;

			// ポップアップ
			popup.UrlPopup = checkBoxUrlPopup.Checked ? PopupState.Enable : PopupState.Disable;
			if (checkBoxUrlPopupCtrlKeySwitch.Checked)
				popup.UrlPopup |= PopupState.KeySwitch;

			popup.ImagePopup = checkBoxImagePopup.Checked ? PopupState.Enable : PopupState.Disable;
			if (checkBoxImagePopupCtrlSwitch.Checked)
				popup.ImagePopup |= PopupState.KeySwitch;

			popup.Position = (PopupPosition)comboBoxPopupPos.SelectedValue;
			popup.Style = checkBoxOrigPopup.Checked ? PopupStyle.Text : PopupStyle.Html;
			popup.Extend = checkBoxExPopup.Checked;
			popup.ClickedHide = checkBoxClickedHide.Checked;
			popup.ClickedHideResPopup = checkBoxClickedHideResPopup.Checked;
			popup.ExtendPopupStr = textBoxPopupRegex.Text;
			popup.PopupInterval = (int)numericUpDownPopupInterval.Value;
			popup.Maximum = new Size((int)numericUpDownPopupMaxWidth.Value, (int)numericUpDownPopupMaxHeight.Value);
			popup.ImagePopupSize = new Size((int)numericUpDownImagePopupWidth.Value, (int)numericUpDownImagePopupHeight.Value);
			thread.UrlPopup = checkBoxUrlPopup.Checked;
			thread.UrlPopupOnCtrl = checkBoxUrlPopupCtrlKeySwitch.Checked;

			// サウンド
			settings.Sound = (SoundSettings)propertyGridSound.SelectedObject;

			// 共通デザイン
			view.TabAppearance = (TabAppearance)comboBoxTabAppearance.SelectedValue;

			view.ListTabSizeMode = GetListTabSizeMode();
			view.ListTabSize.Width = (int)numericUpDownTabSizeWidth.Value;
			view.ListTabSize.Height = (int)numericUpDownTabSizeHeight.Value;

			view.ThreadTabSizeMode = GetThreadTabSizeMode();
			view.ThreadTabSize.Width = (int)numericUpDownThreadTabSizeWidth.Value;
			view.ThreadTabSize.Height = (int)numericUpDownThreadTabSizeHeight.Value;

			design.TabHighlightColor = checkBoxIsHighlightActiveTab.Checked ?
				labelHighlightActiveColor.BackColor : Color.Empty;

			// 板一覧デザイン
			design.Table.Coloring = checkBoxTableColoring.Checked;
			design.Table.HideIcon = checkBoxTableHideIcon.Checked;
			// スレ一覧デザイン
			design.List.Coloring = checkBoxListColoring.Checked;
			// そのほかの配色を取得
			GetColors(design);

			settings.Dialogs.ColorDialogCustomColors = colorDialog.CustomColors;

			// プロキシ
			net._RecvProxy = new WebProxyToCredential();
			net._SendProxy = new WebProxyToCredential();

			#region
			if (textBoxRecvProxyHost.Text != String.Empty)
			{
				net._RecvProxy.Uri =
					new Uri("http://" + textBoxRecvProxyHost.Text + ":" + numericUpDownRecvProxyPort.Value + "/");

				net._RecvProxy.Credential = checkBoxRecvProxyCredential.Checked;
				net._RecvProxy.UserName = textBoxRecvProxyUserID.Text;
				net._RecvProxy.Password = textBoxRecvProxyPass.Text;
			}
			if (textBoxSendProxyHost.Text != String.Empty)
			{
				net._SendProxy.Uri =
					new Uri("http://" + textBoxSendProxyHost.Text + ":" + numericUpDownSendProxyPort.Value + "/");

				net._SendProxy.Credential = checkBoxSendProxyCredential.Checked;
				net._SendProxy.UserName = textBoxSendProxyUserID.Text;
				net._SendProxy.Password = textBoxSendProxyPass.Text;
			}
			#endregion
		}

		private TabSizeMode GetListTabSizeMode()
		{
			if (radioButtonTabAutoSize.Checked)
				return TabSizeMode.Normal;
			if (radioButtonTabFillRight.Checked)
				return TabSizeMode.FillToRight;
			else
				return TabSizeMode.Fixed;
		}

		private TabSizeMode GetThreadTabSizeMode()
		{
			if (radioButtonThreadTabAutoSize.Checked)
				return TabSizeMode.Normal;
			if (radioButtonThreadTabFillRight.Checked)
				return TabSizeMode.FillToRight;
			else
				return TabSizeMode.Fixed;
		}
		#endregion

		#region 配色設定
		private void SetColors(DesignSettings design)
		{
			// 板一覧のラベル
			labelColorTableBoard.Tag = design.Table.BoardBackColor;
			labelColorTableCate.Tag = design.Table.CateBackColor;

			labelColorTableBoard.BackColor = design.Table.BoardBackColor;
			labelColorTableCate.BackColor = design.Table.CateBackColor;

			labelTableColors = new Label[] {
				   labelColorTableBoard, labelColorTableCate};

			// スレ一覧の色ラベル
			labelColorListDef.Tag = design.List.Normal;
			labelColorListRecent.Tag = design.List.RecentThread;
			labelColorListGot.Tag = design.List.GotThread;
			labelColorListNew.Tag = design.List.NewThread;
			labelColorListDat.Tag = design.List.Pastlog;
			labelColorListUp.Tag = design.List.Update;
			labelColorListForcible.Tag = design.List.MostForcible;

			labelColorListBack0.Tag = design.List.BackColorFirst;
			labelColorListBack1.Tag = design.List.BackColorSecond;

			labelColorListBack0.BackColor = design.List.BackColorFirst;
			labelColorListBack1.BackColor = design.List.BackColorSecond;

			labelColors = new Label[] {
				labelColorListDat, labelColorListDef,
				labelColorListGot, labelColorListNew,
				labelColorListUp, labelColorListRecent,
				labelColorListForcible};

			// スレ一覧のチェックボックス
			checkBoxListDat_Bold.Tag =
				checkBoxListDat_UnderLine.Tag = labelColorListDat;

			checkBoxListDef_Bold.Tag =
				checkBoxListDef_UnderLine.Tag = labelColorListDef;

			checkBoxListGot_Bold.Tag =
				checkBoxListGot_UnderLine.Tag = labelColorListGot;

			checkBoxListNew_Bold.Tag =
				checkBoxListNew_UnderLine.Tag = labelColorListNew;

			checkBoxListRecent_Bold.Tag =
				checkBoxListRecent_UnderLine.Tag = labelColorListRecent;

			checkBoxListUp_Bold.Tag =
				checkBoxListUp_UnderLine.Tag = labelColorListUp;

			checkBoxListForce_Bold.Tag =
				checkBoxListForce_Underline.Tag = labelColorListForcible;

			checkBoxesBold = new CheckBox[] {
				checkBoxListDat_Bold, checkBoxListDef_Bold,
				checkBoxListGot_Bold, checkBoxListNew_Bold,
				checkBoxListUp_Bold, checkBoxListRecent_Bold,
				checkBoxListForce_Bold,
			};

			checkBoxesUnderLine = new CheckBox[] {
				checkBoxListDat_UnderLine, checkBoxListDef_UnderLine,
				checkBoxListGot_UnderLine, checkBoxListNew_UnderLine,
				checkBoxListUp_UnderLine, checkBoxListRecent_UnderLine,
				checkBoxListForce_Underline,
			};

			// インストールされているフォントを列挙
			InstalledFontCollection ifc = new InstalledFontCollection();
			ArrayList arrayList = new ArrayList();

			foreach (FontFamily family in ifc.Families)
				arrayList.Add(family.Name);

			string[] families = (string[])arrayList.ToArray(typeof(string));

			// スレ一覧のフォント設定
			comboBoxListFonts.Items.AddRange(families);
			comboBoxListFonts.SelectedItem = design.List.FontName;
			numericUpDownListFontSize.Value = design.List.FontSize;

			// 板一覧のフォント設定
			comboBoxTableFonts.Items.AddRange(families);
			comboBoxTableFonts.SelectedItem = design.Table.FontName;
			numericUpDownTableFontSize.Value = design.Table.FontSize;

			// 書き込みフォント設定
			comboBoxWriteFonts.Items.AddRange(families);
			comboBoxWriteFonts.SelectedItem = design.Post.FontName;
			numericUpDownWriteFontSize.Value = design.Post.FontSize;

			initializing = false;

			TableFontUpdate(null, null);
			FontUpdate(null, null);
			UpdateLabelColors();
		}

		private void GetColors(DesignSettings design)
		{
			design.List.CreateFonts();

			design.Post.FontName = comboBoxWriteFonts.SelectedItem as string;
			design.Post.FontSize = (int)numericUpDownWriteFontSize.Value;
		}

		private void UpdateLabelColors()
		{
			for (int i = 0; i < labelColors.Length; i++)
			{
				Label label = labelColors[i];
				ColorToFont ctf = (ColorToFont)label.Tag;

				label.Font = new Font(label.Font, ctf.Style);
				label.ForeColor = ctf.Color;

				CheckBox bold = checkBoxesBold[i];
				bold.Checked = (ctf.Style & FontStyle.Bold) != 0;

				CheckBox underline = checkBoxesUnderLine[i];
				underline.Checked = (ctf.Style & FontStyle.Underline) != 0;
			}
		}

		// デザインタブのすべてのLabel.MouseUpイベント (文字色用)
		private void label_ColorChange(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Label label = (Label)sender;
			ColorToFont ctf = (ColorToFont)label.Tag;
			Color color = ctf.Color;

			if (ChangeColor(ref color))
			{
				label.ForeColor = color;
				ctf.Color = color;
			}
		}

		// デザインタブのすべてのLabel.MouseUpイベント (背景色用)
		private void label_BackColorChange(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Label label = (Label)sender;
			Color color = (Color)label.Tag;

			if (ChangeColor(ref color))
			{
				label.Tag = label.BackColor = color;

				if (label.Equals(labelColorListBack0))
					settings.Design.List.BackColorFirst = color;

				else if (label.Equals(labelColorListBack1))
					settings.Design.List.BackColorSecond = color;

				else if (label.Equals(labelColorTableBoard))
					settings.Design.Table.BoardBackColor = color;

				else if (label.Equals(labelColorTableCate))
					settings.Design.Table.CateBackColor = color;
			}
		}

		private bool ChangeColor(ref Color color)
		{
			colorDialog.Color = color;

			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				color = colorDialog.Color;
				return true;
			}

			return false;
		}

		private void FontUpdate(object sender, System.EventArgs e)
		{
			if (initializing)
				return;

			try
			{
				Font font = new Font((string)comboBoxListFonts.SelectedItem, (int)numericUpDownListFontSize.Value);
				settings.Design.List.FontName = font.FontFamily.Name;
				settings.Design.List.FontSize = (int)font.Size;

				foreach (Label label in labelColors)
				{
					ColorToFont ctf = (ColorToFont)label.Tag;
					label.Font = new Font(font, ctf.Style);
				}
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		private void DesignBold_CheckedChanged(object sender, System.EventArgs e)
		{
			if (initializing)
				return;
			CheckBox checkbox = (CheckBox)sender;
			Label label = (Label)checkbox.Tag;
			ColorToFont ctf = ((ColorToFont)label.Tag);

			SetFontStyle(ctf, FontStyle.Bold, checkbox.Checked);
			label.Font = new Font(settings.Design.List.FontName, settings.Design.List.FontSize, ctf.Style);
		}

		private void DesignUnderLine_CheckedChanged(object sender, System.EventArgs e)
		{
			if (initializing)
				return;
			CheckBox checkbox = (CheckBox)sender;
			Label label = (Label)checkbox.Tag;
			ColorToFont ctf = ((ColorToFont)label.Tag);

			SetFontStyle(ctf, FontStyle.Underline, checkbox.Checked);
			label.Font = new Font(settings.Design.List.FontName, settings.Design.List.FontSize, ctf.Style);
		}

		private void SetFontStyle(ColorToFont ctf, FontStyle style, bool _set)
		{
			if (_set)
				ctf.Style |= style;
			else
				ctf.Style ^= style;
		}

		private void TableFontUpdate(object sender, System.EventArgs e)
		{
			if (initializing)
				return;

			try
			{
				Font font = new Font((string)comboBoxTableFonts.SelectedItem, (int)numericUpDownTableFontSize.Value);
				settings.Design.Table.FontName = font.FontFamily.Name;
				settings.Design.Table.FontSize = (int)font.Size;

				foreach (Label label in labelTableColors)
					label.Font = font;
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}
		#endregion

		#region Reference Path
		private void buttonRefWebBrowserPath_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
				textBoxWebBrowserPath.Text = openFileDialog.FileName;
		}

		private void buttonRefLogFolder_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog picker = new FolderBrowserDialog();
			picker.Description = "ログ保存先フォルダを指定してください";

			if (picker.ShowDialog() == DialogResult.OK)
				textBoxLogFolder.Text = picker.SelectedPath;
		}
		#endregion

		#region プロキシ設定
		private void buttonClearProxy_Click(object sender, System.EventArgs e)
		{
			textBoxRecvProxyHost.Text =
				textBoxRecvProxyPass.Text =
				textBoxRecvProxyUserID.Text =
				textBoxSendProxyHost.Text =
				textBoxSendProxyPass.Text =
				textBoxSendProxyUserID.Text = String.Empty;

			numericUpDownRecvProxyPort.Value =
				numericUpDownSendProxyPort.Value = 0;
		}
		#endregion

		#region 認証設定
		private void buttonAuthenticateOption_Click(object sender, System.EventArgs e)
		{
			AuthenticateSettingDialog authentication = new AuthenticateSettingDialog(this.settings);
			if (authentication.ShowDialog().Equals(DialogResult.OK))
			{
				if (this.settings.Authentication.AuthenticationOn)
					Twin.Bbs.X2chAuthenticator.Enable(this.settings.Authentication.Username, this.settings.Authentication.Password);
				else
					Twin.Bbs.X2chAuthenticator.Disable();
			}
		}
		#endregion

		#region 使用するブラウザの選択イベント
		private bool textBoxFirstTextChangedEvents = true;
		private void textBoxBrowser_TextChanged(object sender, System.EventArgs e)
		{
			if (textBoxFirstTextChangedEvents)
			{
				textBoxFirstTextChangedEvents = false;

				switch (textBoxWebBrowserPath.Text)
				{
					case "":
						radioButtonBrwsStd.Checked = true;
						break;
					case "SimpleWebBrowser":
						radioButtonBrwsSimpl.Checked = true;
						break;
					default:
						radioButtonBrwsRef.Checked = true;
						break;
				}
			}
		}

		private void radioButtonSetBrowser_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonBrwsSimpl.Equals(sender))
			{
				textBoxWebBrowserPath.Text = "SimpleWebBrowser";
				textBoxWebBrowserPath.Enabled = false;
			}
			else if (radioButtonBrwsStd.Equals(sender))
			{
				textBoxWebBrowserPath.Text = String.Empty;
				textBoxWebBrowserPath.Enabled = false;
			}
			else
			{
				textBoxWebBrowserPath.Text = settings.WebBrowserPath;
				textBoxWebBrowserPath.Enabled = true;
			}
		}
		#endregion

		#region TrackBarとNumericUpDownの値を同期させる
		private void numericUpDownAutoReloadInterval_ValueChanged(object sender, System.EventArgs e)
		{
			trackBarAutoReload.Value = (int)numericUpDownAutoReloadInterval.Value;
		}

		private void numericUpDownClosedHistory_ValueChanged(object sender, System.EventArgs e)
		{
			trackBarHistory.Value = (int)numericUpDownClosedHistory.Value;
		}

		private void trackBarAutoReload_Scroll(object sender, System.EventArgs e)
		{
			numericUpDownAutoReloadInterval.Value =
				trackBarAutoReload.Value;
		}

		private void trackBarHistory_Scroll(object sender, System.EventArgs e)
		{
			numericUpDownClosedHistory.Value =
				trackBarHistory.Value;
		}
		#endregion

		#region 各コントロール同士を同期
		private void checkBoxUrlPopup_CheckedChanged(object sender, System.EventArgs e)
		{
			checkBoxUrlPopupCtrlKeySwitch.Enabled =
				checkBoxUrlPopup.Checked;
		}

		private void checkBoxOrigPopup_CheckedChanged(object sender, System.EventArgs e)
		{
			numericUpDownPopupMaxHeight.Enabled =
				numericUpDownPopupMaxWidth.Enabled = checkBoxOrigPopup.Checked;
		}

		private void radioButtonResViewMode_CheckedChanged(object sender, System.EventArgs e)
		{
			numericUpDownViewResCount.Enabled =
				radioButtonViewLimitResCount.Checked;
		}

		private void radioButtonRendering_Click(object sender, System.EventArgs e)
		{
			//RadioButton radio = (RadioButton)sender;
		}

		private void radioButtonTabFixedSize_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonTabFixedSize.Equals(sender))
			{
				numericUpDownTabSizeHeight.Enabled =
					numericUpDownTabSizeWidth.Enabled = radioButtonTabFixedSize.Checked;
			}
			else if (radioButtonThreadTabFixed.Equals(sender))
			{
				numericUpDownThreadTabSizeHeight.Enabled =
					numericUpDownThreadTabSizeWidth.Enabled = radioButtonThreadTabFixed.Checked;
			}
		}

		private void checkBoxExPopup_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxPopupRegex.Enabled = checkBoxExPopup.Checked;
		}

		private void checkBoxImagePopup_CheckedChanged(object sender, System.EventArgs e)
		{
			numericUpDownImagePopupWidth.Enabled =
				numericUpDownImagePopupHeight.Enabled =
				checkBoxImagePopupCtrlSwitch.Enabled = checkBoxImagePopup.Checked;
		}

		private void checkBoxImageThumb_CheckedChanged(object sender, System.EventArgs e)
		{
			numericUpDownThumSize.Enabled = checkBoxImageThumb.Checked;
		}

		private void checkBoxRecvProxyCredential_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxRecvProxyUserID.Enabled =
				textBoxRecvProxyPass.Enabled = checkBoxRecvProxyCredential.Checked;
		}

		private void checkBoxSendProxyCredential_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxSendProxyUserID.Enabled =
				textBoxSendProxyPass.Enabled = checkBoxSendProxyCredential.Checked;
		}

		private void comboBoxOpenMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			OpenMode mode = (OpenMode)comboBoxOpenMode.SelectedValue;
			checkBoxAlwaysSingleOpen.Enabled = (mode == OpenMode.SingleClick);
		}

		private void checkBoxThumbnailIsLightMode_CheckedChanged(object sender, EventArgs e)
		{
			this.numericUpDownImageSizeLimit.Enabled =
				checkBoxThumbnailIsLightMode.Checked;
		}
		#endregion

		#region コテハン設定部
		private bool koteini = true;

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabPageKotehan.Equals(tabControl.SelectedTab) && koteini)
			{
				BoardInfo[] array = table.ToArray();
				ArrayList arrayList = new ArrayList();
				ArrayList exists = new ArrayList();

				// 登録済みコテハン一覧をツリービューに追加
				if (!koteman.Default.IsEmpty)
				{
					TreeNode all = new TreeNode("すべてに適用");
					treeViewKotehan.Nodes.Add(all);
				}

				foreach (BoardInfo board in array)
				{
					if (koteman.IsExists(board) && !exists.Contains(board))
					{
						TreeNode node = new TreeNode(board.Name);
						node.Tag = board;
						treeViewKotehan.Nodes.Add(node);
						exists.Add(board);
					}
					arrayList.Add(board);
				}

				// コテハン未登録の板一覧をコンボボックスに追加
				comboBoxBoardList.Items.Add("すべてに適用");
				comboBoxBoardList.Items.AddRange(arrayList.ToArray());
				comboBoxBoardList.SelectedIndex = 0;

				koteini = false;
			}
		}

		private void treeViewKotehan_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
		}

		// 登録済みコテハンが選択されたらコテハン情報を入力欄に設定
		private void treeViewKotehan_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeViewKotehan.GetNodeAt(
				treeViewKotehan.PointToClient(MousePosition));

			if (node != null)
			{
				Kotehan kote = null;
				BoardInfo board = node.Tag as BoardInfo;

				// 特定の板のコテハン
				if (board != null)
				{
					kote = koteman.Get(board);
					comboBoxBoardList.SelectedItem = board;
				}
				// すべてに適用するコテハン
				else
				{
					kote = koteman.Default;
					comboBoxBoardList.SelectedIndex = 0;
				}

				textBoxPostName.Text = kote.Name;
				textBoxPostEmail.Text = kote.Email;
				buttonDeleteKote.Enabled = true;
			}
		}

		private void comboBoxBoardList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void buttonDeleteKote_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeViewKotehan.SelectedNode;

			// 登録済みコテハンを削除
			if (node != null)
			{
				node.Remove();
				BoardInfo board = node.Tag as BoardInfo;

				// 特定の板のコテハンを削除
				if (board != null)
				{
					koteman.Set(board, null);
				}
				// すべてに適用のコテハンを削除
				else
				{
					koteman.Default = null;
				}
			}

			textBoxPostName.Text =
				textBoxPostEmail.Text = String.Empty;

			buttonDeleteKote.Enabled = false;
		}

		private void buttonRegistKote_Click(object sender, System.EventArgs e)
		{
			// 登録するコテハン
			Kotehan kotehan =
				new Kotehan(textBoxPostName.Text, textBoxPostEmail.Text, checkBoxSendBeID.Checked);

			// 登録する板
			BoardInfo board =
				comboBoxBoardList.SelectedItem as BoardInfo;

			// 未登録の板であれば登録済み一覧に追加
			if (board == null)
			{
				if (koteman.Default.IsEmpty)
				{
					TreeNode node = new TreeNode();
					node.Text = "すべてに適用";
					node.Tag = null;
					treeViewKotehan.Nodes.Add(node);
				}
			}
			else if (!koteman.IsExists(board))
			{
				TreeNode node = new TreeNode();
				node.Text = board.Name;
				node.Tag = board;
				treeViewKotehan.Nodes.Add(node);
			}

			// 特定の板にコテハンを設定
			if (board != null)
			{
				koteman.Set(board, kotehan);
			}
			// すべての板にコテハンを設定
			else
			{
				koteman.Default = kotehan;
			}

			// 入力情報を空に設定
			textBoxPostName.Text =
				textBoxPostEmail.Text = String.Empty;
		}

		private void buttonTripPreview_Click(object sender, System.EventArgs e)
		{
			int index = textBoxPostName.Text.IndexOf("#");
			if (index >= 0)
			{
				MessageBox.Show(textBoxPostName.Text.Substring(0, index) + " ◆" +
					Trip.Create(textBoxPostName.Text.Substring(index + 1)), "トリップ確認");
			}
			else
			{
				MessageBox.Show("トリップが入力されていません", "トリップ確認");
			}
		}

		private void textBoxKotehan_TextChanged(object sender, System.EventArgs e)
		{
			buttonRegistKote.Enabled =
				(textBoxPostName.Text == String.Empty) &&
				(textBoxPostEmail.Text == String.Empty) ? false : true;
		}
		#endregion

		#region 板一覧の個別デザイン設定
		private void buttonAddColor_Click(object sender, System.EventArgs e)
		{

		}

		private void buttonRemoveColor_Click(object sender, System.EventArgs e)
		{

		}
		#endregion

		#region スキン設定
		private void comboBoxSkins_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string name = (string)comboBoxSkins.SelectedItem;
			settings.SkinFolderName = name;

			string imagePath = Path.Combine(settings.SkinFolderPath, "Preview.bmp");
			if (File.Exists(imagePath))
			{
				using (FileStream fs = new FileStream(imagePath, FileMode.Open))
					pictureBox.Image = Image.FromStream(fs);
			}
		}

		private bool init = true;
		private void tabControlDesign_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (init && tabPageThreadDesign.Equals(tabControlDesign.SelectedTab))
			{
				comboBoxSkins.SelectedItem = settings.SkinFolderName;
				init = false;
			}
		}
		#endregion

		#region サウンド設定
		private void propertyGridSound_PropertyValueChanged(object sender, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
		}
		#endregion

		#region スレッド設定
		private bool __tmp = false;
		private void scrollSettings_CheckedChanged(object sender, System.EventArgs e)
		{
			if (__tmp)
				return;
			__tmp = true;

			if (sender.Equals(checkBoxScrollToNewRes))
			{
				if (checkBoxScrollToNewRes.Checked)
					checkBoxAutoScrollOn.Checked = false;
			}
			else if (sender.Equals(checkBoxAutoScrollOn))
			{
				if (checkBoxAutoScrollOn.Checked)
					checkBoxScrollToNewRes.Checked = false;
			}

			__tmp = false;
		}
		#endregion

		#region Validating
		private void textBoxLogFolder_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel =
				Directory.Exists(textBoxLogFolder.Text) ||
				Path.IsPathRooted(textBoxLogFolder.Text) ? false : true;

			if (e.Cancel)
				errorProvider.SetError((Control)sender, "指定されたログ保存フォルダは無効です");
		}

		private void numericUpDownCtrl_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			NumericUpDown ctrl = (NumericUpDown)sender;
			e.Cancel = (ctrl.Minimum > ctrl.Value || ctrl.Maximum < ctrl.Value) ? true : false;

			if (e.Cancel)
				errorProvider.SetError(ctrl, String.Format("指定された値は範囲を超えています。({0}～{1}まで", ctrl.Minimum, ctrl.Maximum));
		}

		private void CommonValidated(object sender, System.EventArgs e)
		{
			errorProvider.SetError((Control)sender, String.Empty);
		}
		#endregion

		private void checkBoxIsHighlightActiveTab_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void labelHightlightActiveColor_Click(object sender, EventArgs e)
		{
			if (!checkBoxIsHighlightActiveTab.Checked)
				return;

			colorDialog.Color = labelHighlightActiveColor.BackColor;

			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				labelHighlightActiveColor.BackColor = colorDialog.Color;
			}
		}

		private void checkBoxAutoReloadCheckOnly_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void checkBoxAutoReloadAverage_CheckedChanged(object sender, EventArgs e)
		{
			trackBarAutoReload.Enabled = numericUpDownAutoReloadInterval.Enabled = checkBoxAutoReloadCheckOnly.Enabled =
				!checkBoxAutoReloadAverage.Checked;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show("ログの保存先フォルダを変更した場合、既に登録してあるスレッドグループや、各履歴が消えてしまいます！\r\n" +
				"これを回避するには、先に既存のログフォルダを変更先へ移動してから、パスを変更してください。", "注意！",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private void checkBoxImageViewer_CheckedChanged(object sender, EventArgs e)
		{
			//checkBoxAutoOpenImage.Enabled = checkBoxImageViewer.Checked;
		}

		private void checkBoxNextThreadChecker_CheckedChanged(object sender, EventArgs e)
		{
			checkBoxHighLevelMatch.Enabled = checkBoxNextThreadChecker.Checked;
		}
	}
}
