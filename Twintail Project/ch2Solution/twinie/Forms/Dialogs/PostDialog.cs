// PostDialog.cs

namespace Twin.Forms
{
	using System;
	using System.IO;
	using System.Text;
	using System.Drawing;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Threading;
	using System.Windows.Forms;
	using System.Text.RegularExpressions;
	using System.Net;
	using AxSHDocVw;
	using mshtml;
	using Twin;
	using Twin.Bbs;
	using Twin.Tools;
	using Twin.Aa;

	/// <summary>
	/// PostDialog の概要の説明です。
	/// </summary>
	public sealed class PostDialog : System.Windows.Forms.Form
	{
		internal static readonly Samba24 samba = new Samba24(Settings.Samba24Path);

		private static AaContextMenu aaContext = null;
		public static AaContextMenu AaContext
		{
			get
			{
				if (aaContext == null)
					aaContext = new AaContextMenu(Settings.AaFolderPath);

				return aaContext;
			}
		}

		private CookieManager cookie;
		private KotehanManager koteman;

		private Cache cache;
		private ThreadHeader headerInfo;
		private BoardInfo boardInfo;

		private int tempDialogHeight = -1;		// 最小化時に元のウインドウサイズを記憶しておく変数

		// 投稿関連
		private PostType postType;
		private PostBase post;
		private bool posting;				// 投稿中かどうかを表す

		// プレビュー関連
		private bool notInit;				// ブラウザが初期化済みかどうかを表す
		private IEComPreviewControl viewer;

		// Samba秒数をタイトルバーに表示するタイマー
		private System.Windows.Forms.Timer sambaTimer;
		private int sambaCount = 0;

		private X2chServerSetting serverSetting = new X2chServerSetting();
		private SettingTxtManager settingTxtManager = new SettingTxtManager();
		private bool downloadingSettingTxt = false;

		#region Designer Fields
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TextBox textBoxSubject;
		private System.Windows.Forms.CheckBox checkBoxSage;
		private System.Windows.Forms.TextBox textBoxBody;
		private System.Windows.Forms.ToolBarButton toolBarButtonAA;
		private System.Windows.Forms.ToolBarButton toolBarButtonSave;
		private System.Windows.Forms.ToolBarButton toolBarButtonClear;
		private System.Windows.Forms.TabPage tabPageWrite;
		private System.Windows.Forms.TabPage tabPagePreview;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelEmail;
		private System.Windows.Forms.CheckBox checkBoxSendBeID;
		private ComboBox comboBoxMail;
		private ComboBox comboBoxName;
		private TableLayoutPanel tableLayoutPanel1;
		private TabPage tabPageSettingTxt;
		private TextBox textBoxSettingTxt;
		private Label labelCharInfo;
		private ToolStrip toolStrip1;
		private ToolStripLabel toolStripLabel1;
		private ToolStripTextBox toolStripTextBoxUrl;
		private ToolStripButton toolStripButtonRefresh;
		private CheckBox checkBoxOyster;
		private System.ComponentModel.IContainer components;
		#endregion

		/// <summary>
		/// 書き込み時対象の板情報を取得
		/// </summary>
		public BoardInfo BoardInfo
		{
			get
			{
				return boardInfo;
			}
		}

		/// <summary>
		/// レス投稿時、設定されているスレッドの情報を取得
		/// </summary>
		public ThreadHeader HeaderInfo
		{
			get
			{
				return headerInfo;
			}
		}

		/// <summary>
		/// スレッド名を取得または設定
		/// </summary>
		public string Subject
		{
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Subject");
				}
				textBoxSubject.Text = value;
			}
			get
			{
				return textBoxSubject.Text;
			}
		}

		/// <summary>
		/// 投稿者名を取得または設定
		/// </summary>
		public string From
		{
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("From");
				}
				comboBoxName.Text = value;
			}
			get
			{
				return comboBoxName.Text;
			}
		}

		/// <summary>
		/// 投稿Emailを取得または設定
		/// </summary>
		public string Email
		{
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Email");
				}
				comboBoxMail.Text = value;
			}
			get
			{
				return comboBoxMail.Text;
			}
		}

		/// <summary>
		/// 投稿本文を取得または設定
		/// </summary>
		public string Body
		{
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Body");
				}
				textBoxBody.Text = value;
			}
			get
			{
				return textBoxBody.Text;
			}
		}

		/// <summary>
		/// Sageのチェック状態を取得または設定
		/// </summary>
		public bool Sage
		{
			set
			{
				checkBoxSage.Checked = value;
			}
			get
			{
				return checkBoxSage.Checked;
			}
		}

		/// <summary>
		/// 日本語入力をOnにするかどうかを取得または設定
		/// </summary>
		public bool ImeOn
		{
			set
			{
				textBoxBody.ImeMode = (value) ? ImeMode.On : ImeMode.Off;
			}
			get
			{
				return (textBoxBody.ImeMode == ImeMode.On) ? true : false;
			}
		}

		/// <summary>
		/// 書き込みが開始された直後の時間を表します。
		/// </summary>
		public DateTime BeginWriteTime { get; private set; }

		/// <summary>
		/// 書き込みが完了した直後の時間を表します。
		/// </summary>
		public DateTime EndWriteTime { get; private set; }

		/// <summary>
		/// ダイアログのサイズを取得
		/// </summary>
		public new Size Size
		{
			set
			{
				base.Size = value;
			}
			get
			{
				return (tempDialogHeight != -1) ?
					new Size(Width, tempDialogHeight) : base.Size;
			}
		}

		/// <summary>
		/// 投稿成功時に発生
		/// </summary>
		public event PostedEventHandler Posted;

		private PostDialog(Cache cache, KotehanManager koteman,
			BbsType bbs, PostType type, BoardInfo board)
		{
			InitializeComponent();

			AaContext.Selected += new AaItemEventHandler(OnAaSelected);
			this.ContextMenu = AaContext.Context;

			this.cache = cache;
			this.koteman = koteman;
			this.boardInfo = board;
			this.cookie = new CookieManager(cache);

			post = TypeCreator.CreatePost(bbs);
			post.Proxy = Twinie.Settings.Net.SendProxy;
			post.UserAgent = Settings.UserAgent;
			post.Posted += new PostEventHandler(OnPosted);
			post.Error += new PostErrorEventHandler(OnPostError);
			postType = type;

			textBoxSubject.Enabled = (postType == PostType.Thread);
			notInit = true;

			// Sambaタイマーを初期化
			sambaTimer = new System.Windows.Forms.Timer();
			sambaTimer.Interval = 500;
			sambaTimer.Tick += new EventHandler(SambaCount_OnTick);

			if (!samba.IsElapsed(boardInfo.ServerName, out sambaCount))
				sambaTimer.Start();

			// 草稿機能はレス時のみ有効
			toolBarButtonSave.Enabled =
				(postType == PostType.Res) ? true : false;

			Kotehan kote = koteman.Get(board);
			//checkBoxSendBeID.Enabled = !Twinie.Settings.Post.Be2chCookie.IsEmpty;
			//checkBoxSendBeID.Checked = kote.Be;

			this.checkBoxOyster.Enabled = Twinie.OysterIsValid();
			this.checkBoxOyster.Checked = Twinie.Settings.Authentication.AuthenticationOn;

			if (type == PostType.Thread && !post.CanPostThread)
			{
				MessageBox.Show(bbs + "のスレ立ては未対応です");
				buttonOK.Enabled = false;
			}
			else if (type == PostType.Res && !post.CanPostRes)
			{
				MessageBox.Show(bbs + "のレス投稿は未対応です");
				buttonOK.Enabled = false;
			}

			// 書き込み画面のフォント設定
			Font font = new Font(Twinie.Settings.Design.Post.FontName,
				Twinie.Settings.Design.Post.FontSize);

			textBoxBody.Font =
				comboBoxMail.Font = comboBoxName.Font = font;

			// 名前とメール欄の履歴を追加
			comboBoxName.Items.AddRange(Twinie.Settings.Post.NameHistory.Keys.ToArray());
			comboBoxMail.Items.AddRange(Twinie.Settings.Post.MailHistory.Keys.ToArray());

			InitServerSettingInfo();
		}

		/// <summary>
		/// PostDialogクラスのインスタンスを初期化
		/// </summary>
		public PostDialog(Cache cache, KotehanManager koteman, BoardInfo board)
			: this(cache, koteman, board.Bbs, PostType.Thread, board)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			Kotehan kote = koteman.Get(board);
			this.From = kote.Name;
			this.Email = kote.Email;
		}

		/// <summary>
		/// PostDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="info"></param>
		/// <param name="message"></param>
		public PostDialog(Cache cache, KotehanManager koteman, BoardInfo info, PostThread message)
			: this(cache, koteman, info)
		{
			this.Subject = message.Subject;
			this.From = message.From;
			this.Email = message.Email;
			this.Body = message.Body;
		}

		/// <summary>
		/// PostDialogクラスのインスタンスを初期化
		/// </summary>
		public PostDialog(Cache cache, KotehanManager koteman, ThreadHeader header)
			: this(cache, koteman, header.BoardInfo.Bbs, PostType.Res, header.BoardInfo)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			headerInfo = header;
			textBoxSubject.Text = headerInfo.Subject;

			Kotehan kote = koteman.Get(headerInfo);
			this.From = kote.Name;
			this.Email = kote.Email;
		}

		/// <summary>
		/// PostDialogクラスのインスタンスを初期化
		/// </summary>
		public PostDialog(Cache cache, KotehanManager koteman, ThreadHeader th, PostRes message)
			: this(cache, koteman, th)
		{
			this.From = message.From;
			this.Email = message.Email;
			this.Body = message.Body;
		}

		~PostDialog()
		{
			Dispose(false);
		}

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

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostDialog));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageWrite = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.comboBoxMail = new System.Windows.Forms.ComboBox();
			this.comboBoxName = new System.Windows.Forms.ComboBox();
			this.checkBoxSage = new System.Windows.Forms.CheckBox();
			this.labelEmail = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonAA = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonClear = new System.Windows.Forms.ToolBarButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxBody = new System.Windows.Forms.TextBox();
			this.textBoxSubject = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tabPagePreview = new System.Windows.Forms.TabPage();
			this.tabPageSettingTxt = new System.Windows.Forms.TabPage();
			this.textBoxSettingTxt = new System.Windows.Forms.TextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripTextBoxUrl = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.checkBoxSendBeID = new System.Windows.Forms.CheckBox();
			this.labelCharInfo = new System.Windows.Forms.Label();
			this.checkBoxOyster = new System.Windows.Forms.CheckBox();
			this.tabControl.SuspendLayout();
			this.tabPageWrite.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tabPageSettingTxt.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageWrite);
			this.tabControl.Controls.Add(this.tabPagePreview);
			this.tabControl.Controls.Add(this.tabPageSettingTxt);
			this.tabControl.ImageList = this.imageList;
			this.tabControl.ItemSize = new System.Drawing.Size(100, 20);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(400, 215);
			this.tabControl.TabIndex = 0;
			this.tabControl.TabStop = false;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabPageWrite
			// 
			this.tabPageWrite.Controls.Add(this.tableLayoutPanel1);
			this.tabPageWrite.Controls.Add(this.toolBar);
			this.tabPageWrite.Controls.Add(this.label1);
			this.tabPageWrite.Controls.Add(this.textBoxBody);
			this.tabPageWrite.Controls.Add(this.textBoxSubject);
			this.tabPageWrite.Controls.Add(this.label4);
			this.tabPageWrite.ImageIndex = 2;
			this.tabPageWrite.Location = new System.Drawing.Point(4, 24);
			this.tabPageWrite.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageWrite.Name = "tabPageWrite";
			this.tabPageWrite.Size = new System.Drawing.Size(392, 187);
			this.tabPageWrite.TabIndex = 0;
			this.tabPageWrite.Text = "書き込み";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
			this.tableLayoutPanel1.Controls.Add(this.comboBoxMail, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxName, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxSage, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelEmail, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 28);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(375, 27);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// comboBoxMail
			// 
			this.comboBoxMail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxMail.FormattingEnabled = true;
			this.comboBoxMail.Location = new System.Drawing.Point(214, 3);
			this.comboBoxMail.Name = "comboBoxMail";
			this.comboBoxMail.Size = new System.Drawing.Size(105, 20);
			this.comboBoxMail.TabIndex = 3;
			// 
			// comboBoxName
			// 
			this.comboBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxName.FormattingEnabled = true;
			this.comboBoxName.Location = new System.Drawing.Point(53, 3);
			this.comboBoxName.Name = "comboBoxName";
			this.comboBoxName.Size = new System.Drawing.Size(105, 20);
			this.comboBoxName.TabIndex = 1;
			// 
			// checkBoxSage
			// 
			this.checkBoxSage.AllowDrop = true;
			this.checkBoxSage.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.checkBoxSage.AutoSize = true;
			this.checkBoxSage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxSage.Location = new System.Drawing.Point(324, 7);
			this.checkBoxSage.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxSage.Name = "checkBoxSage";
			this.checkBoxSage.Size = new System.Drawing.Size(49, 17);
			this.checkBoxSage.TabIndex = 4;
			this.checkBoxSage.Text = "&sage";
			this.checkBoxSage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxSage.CheckedChanged += new System.EventHandler(this.checkBoxSage_CheckedChanged);
			// 
			// labelEmail
			// 
			this.labelEmail.AllowDrop = true;
			this.labelEmail.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelEmail.AutoSize = true;
			this.labelEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelEmail.Location = new System.Drawing.Point(166, 10);
			this.labelEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelEmail.Name = "labelEmail";
			this.labelEmail.Size = new System.Drawing.Size(39, 12);
			this.labelEmail.TabIndex = 2;
			this.labelEmail.Text = "&E-mail";
			this.labelEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AllowDrop = true;
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(2, 10);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "名前(&N)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// toolBar
			// 
			this.toolBar.AllowDrop = true;
			this.toolBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.AutoSize = false;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonAA,
            this.toolBarButtonSave,
            this.toolBarButtonClear});
			this.toolBar.ButtonSize = new System.Drawing.Size(23, 20);
			this.toolBar.Divider = false;
			this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(313, 2);
			this.toolBar.Margin = new System.Windows.Forms.Padding(2);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(74, 26);
			this.toolBar.TabIndex = 2;
			this.toolBar.Wrappable = false;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			this.toolBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolBar_MouseUp);
			// 
			// toolBarButtonAA
			// 
			this.toolBarButtonAA.ImageIndex = 6;
			this.toolBarButtonAA.Name = "toolBarButtonAA";
			this.toolBarButtonAA.ToolTipText = "AA入力支援";
			// 
			// toolBarButtonSave
			// 
			this.toolBarButtonSave.ImageIndex = 4;
			this.toolBarButtonSave.Name = "toolBarButtonSave";
			this.toolBarButtonSave.ToolTipText = "草稿箱へ保存";
			// 
			// toolBarButtonClear
			// 
			this.toolBarButtonClear.ImageIndex = 5;
			this.toolBarButtonClear.Name = "toolBarButtonClear";
			this.toolBarButtonClear.ToolTipText = "クリア";
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
			// 
			// label1
			// 
			this.label1.AllowDrop = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(4, 10);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "スレッド名(&T)";
			// 
			// textBoxBody
			// 
			this.textBoxBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxBody.Location = new System.Drawing.Point(2, 60);
			this.textBoxBody.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxBody.Multiline = true;
			this.textBoxBody.Name = "textBoxBody";
			this.textBoxBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxBody.Size = new System.Drawing.Size(388, 118);
			this.textBoxBody.TabIndex = 4;
			this.textBoxBody.WordWrap = false;
			this.textBoxBody.TextChanged += new System.EventHandler(this.textBoxBody_TextChanged);
			this.textBoxBody.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxBody_KeyDown);
			// 
			// textBoxSubject
			// 
			this.textBoxSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSubject.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxSubject.Location = new System.Drawing.Point(71, 5);
			this.textBoxSubject.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxSubject.Name = "textBoxSubject";
			this.textBoxSubject.Size = new System.Drawing.Size(237, 19);
			this.textBoxSubject.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 67);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 1;
			this.label4.Text = "&B";
			// 
			// tabPagePreview
			// 
			this.tabPagePreview.ImageIndex = 0;
			this.tabPagePreview.Location = new System.Drawing.Point(4, 24);
			this.tabPagePreview.Margin = new System.Windows.Forms.Padding(2);
			this.tabPagePreview.Name = "tabPagePreview";
			this.tabPagePreview.Size = new System.Drawing.Size(392, 187);
			this.tabPagePreview.TabIndex = 1;
			this.tabPagePreview.Text = "プレビュー";
			// 
			// tabPageSettingTxt
			// 
			this.tabPageSettingTxt.Controls.Add(this.textBoxSettingTxt);
			this.tabPageSettingTxt.Controls.Add(this.toolStrip1);
			this.tabPageSettingTxt.ImageIndex = 3;
			this.tabPageSettingTxt.Location = new System.Drawing.Point(4, 24);
			this.tabPageSettingTxt.Name = "tabPageSettingTxt";
			this.tabPageSettingTxt.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSettingTxt.Size = new System.Drawing.Size(392, 187);
			this.tabPageSettingTxt.TabIndex = 2;
			this.tabPageSettingTxt.Text = "SETTING.TXT";
			this.tabPageSettingTxt.UseVisualStyleBackColor = true;
			// 
			// textBoxSettingTxt
			// 
			this.textBoxSettingTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxSettingTxt.Location = new System.Drawing.Point(3, 28);
			this.textBoxSettingTxt.Multiline = true;
			this.textBoxSettingTxt.Name = "textBoxSettingTxt";
			this.textBoxSettingTxt.ReadOnly = true;
			this.textBoxSettingTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxSettingTxt.Size = new System.Drawing.Size(386, 156);
			this.textBoxSettingTxt.TabIndex = 0;
			this.textBoxSettingTxt.WordWrap = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBoxUrl,
            this.toolStripButtonRefresh});
			this.toolStrip1.Location = new System.Drawing.Point(3, 3);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(386, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(37, 22);
			this.toolStripLabel1.Text = "URL:";
			// 
			// toolStripTextBoxUrl
			// 
			this.toolStripTextBoxUrl.Name = "toolStripTextBoxUrl";
			this.toolStripTextBoxUrl.ReadOnly = true;
			this.toolStripTextBoxUrl.Size = new System.Drawing.Size(250, 25);
			// 
			// toolStripButtonRefresh
			// 
			this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
			this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
			this.toolStripButtonRefresh.Size = new System.Drawing.Size(36, 22);
			this.toolStripButtonRefresh.Text = "更新";
			this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(202, 218);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(86, 26);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル(&C)";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonOK.AutoSize = true;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(115, 218);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(84, 26);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "書き込む(&W)";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// checkBoxSendBeID
			// 
			this.checkBoxSendBeID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxSendBeID.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxSendBeID.AutoSize = true;
			this.checkBoxSendBeID.Enabled = false;
			this.checkBoxSendBeID.Location = new System.Drawing.Point(36, 218);
			this.checkBoxSendBeID.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxSendBeID.Name = "checkBoxSendBeID";
			this.checkBoxSendBeID.Size = new System.Drawing.Size(30, 22);
			this.checkBoxSendBeID.TabIndex = 3;
			this.checkBoxSendBeID.Text = "BE";
			this.checkBoxSendBeID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxSendBeID.Visible = false;
			// 
			// labelCharInfo
			// 
			this.labelCharInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelCharInfo.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.labelCharInfo.Location = new System.Drawing.Point(313, 218);
			this.labelCharInfo.Name = "labelCharInfo";
			this.labelCharInfo.Size = new System.Drawing.Size(83, 27);
			this.labelCharInfo.TabIndex = 4;
			this.labelCharInfo.Text = "XXXXXXXX";
			// 
			// checkBoxOyster
			// 
			this.checkBoxOyster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxOyster.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxOyster.AutoSize = true;
			this.checkBoxOyster.Location = new System.Drawing.Point(4, 218);
			this.checkBoxOyster.Name = "checkBoxOyster";
			this.checkBoxOyster.Size = new System.Drawing.Size(27, 22);
			this.checkBoxOyster.TabIndex = 5;
			this.checkBoxOyster.Text = "○";
			this.checkBoxOyster.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxOyster.UseVisualStyleBackColor = true;
			this.checkBoxOyster.CheckedChanged += new System.EventHandler(this.checkBoxOyster_CheckedChanged);
			// 
			// PostDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 245);
			this.Controls.Add(this.checkBoxOyster);
			this.Controls.Add(this.labelCharInfo);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.checkBoxSendBeID);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PostDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "投稿";
			this.Activated += new System.EventHandler(this.PostDialog_Activated);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.PostDialog_Closing);
			this.Closed += new System.EventHandler(this.PostDialog_Closed);
			this.Deactivate += new System.EventHandler(this.PostDialog_Deactivate);
			this.Load += new System.EventHandler(this.PostDialog_Load);
			this.Resize += new System.EventHandler(this.PostDialog_Resize);
			this.tabControl.ResumeLayout(false);
			this.tabPageWrite.ResumeLayout(false);
			this.tabPageWrite.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tabPageSettingTxt.ResumeLayout(false);
			this.tabPageSettingTxt.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void InitServerSettingInfo()
		{
			labelCharInfo.Text = String.Empty;

			string text = settingTxtManager.LoadCacheData(boardInfo);

			if (text == String.Empty)
				return;

			serverSetting.Read(boardInfo, text);
			textBoxSettingTxt.Text = text;
		}

		/// <summary>
		/// Sambaの残り秒数を更新
		/// </summary>
		private void UpdateTitle()
		{
			string temp;

			if (postType == PostType.Thread)
			{
				temp = String.Format("新規ｽｯﾄﾞﾚ! {0}板", boardInfo.Name);
			}
			else
			{
				temp = String.Format("ﾚｽを書く! [{0}] {1}", headerInfo.BoardInfo.Name, headerInfo.Subject);
			}

			if (sambaCount > 0)
				temp = String.Format("<Samba残り: {0}秒> {1}", sambaCount, temp);

			Text = temp;
		}

		private void SambaCount_OnTick(object sender, EventArgs e)
		{
			sambaTimer.Stop();

			if (samba.IsElapsed(boardInfo.ServerName, out sambaCount))
			{
				sambaCount = 0;
			}
			else
			{
				sambaTimer.Start();
			}

			UpdateTitle();
		}

		/// <summary>
		/// エラーチェック。エラーがなければ true、エラーがあれば false を返す。
		/// </summary>
		/// <returns></returns>
		private bool IsErrorChecking()
		{
			if (textBoxBody.Text == String.Empty)
			{
				MessageBox.Show("本文を入力してください", "投稿エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return false;
			}
			if (textBoxSubject.Text == String.Empty)
			{
				MessageBox.Show("スレッド名を入力してください", "投稿エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return false;
			}

			return true;
		}

		/// <summary>
		/// 文字数制限の確認。問題がなければ true を返し、文字数制限を越えていたら false を返す。
		/// </summary>
		/// <returns></returns>
		private bool TextLengthCheckingAll()
		{
			// 2ch以外の文字数制限はしらないので何もしない
			if (boardInfo.Bbs != BbsType.X2ch)
				return true;

			// NTwin23.105
			string from = MakeConvertedName( From );
			if ( from == null )
			{
				MessageBox.Show( "トリップキーが長すぎます。#以降は半角1024文字までです。" , "入力エラー" ,
					MessageBoxButtons.OK , MessageBoxIcon.Information );
				return false;
			}
			// NTwin23.105

			if (serverSetting.IsLoaded)
			{
				// スレ立ての場合のみ、スレ名の長さをチェック
				if ((this.postType == PostType.Thread && !TextLengthChecking("スレッド名", textBoxSubject.Text, serverSetting.SubjectCount)) ||
					!TextLengthChecking("名前", from, serverSetting.NameCount) ||
					!TextLengthChecking("メール欄", Email, serverSetting.MailCount) ||
					!TextLengthChecking("本文", Body, serverSetting.MessageCount) ||
					!LineChecking())
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// data のバイト数が maxCount を超えていたらメッセージボックスを表示し、false を返す。文字制限を越えていなければ true を返す。
		/// </summary>
		private bool TextLengthChecking(string errorObj, string data, int maxCount)
		{
			if (Encoding.GetEncoding("shift_jis").GetByteCount(data) > maxCount)
			{
				MessageBox.Show(errorObj + "が長すぎます。最大" + maxCount + "文字までです。", "入力エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				return false;
			}

			return true;
		}

		private bool LineChecking()
		{
			if (textBoxBody.Lines.Length > serverSetting.LineNumber*2)
			{
				MessageBox.Show("行数が多すぎます。最大" + (serverSetting.LineNumber*2) + "行までです。", "入力エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				return false;
			}

			return true;
		}


		/// <summary>
		/// 自主規制を行う。まだカウントが残っていれば false、規制時間が経過していれば true を返す。
		/// </summary>
		/// <returns></returns>
		private bool Samba24Checking()
		{
			if (Twinie.Settings.Post.Samba24Check)
			{
				// チェック対象のサーバー名を取得 (※ホストアドレスではない)
				string server = boardInfo.ServerName;
				int result;

				if (!samba.IsElapsed(server, out result))
				{
					SambaErrorDialog dlg = new SambaErrorDialog(result);

					if (dlg.ShowDialog(this) != DialogResult.Ignore)
						return false;
				}

				samba.CountStart(server);
			}
			return true;
		}

		/// <summary>
		/// 現在のメッセージを草稿として保存
		/// </summary>
		private void SaveDraft()
		{
			PostRes res = new PostRes(From, Email, Body);
			DraftBox box = new DraftBox(cache);
			Draft draft = new Draft(headerInfo, res);

			box.Save(boardInfo, draft);
		}

		/// <summary>
		/// プレビュー画面を更新
		/// </summary>
		private void RefreshPreview()
		{
			// 日付を作成
			string date =
				DateTime.Now.ToString("yy/MM/dd HH:mm");

			// 本分を作成
			string body = Body;
			body = Regex.Replace(body, "<", "&lt;");
			body = Regex.Replace(body, ">", "&gt;");
			body = Regex.Replace(body, "\r\n", " <br> ");
			body = Regex.Replace(body, @">>(?<num>\d+\-?\d*)", "<a href=\"../${num}\">&gt;&gt;${num}</a>");


			// NTwin23.105
			//// 名前を作成 (トリップも作成)
			//string name = (From != String.Empty) ? From : "名無しさん";
			//Match m = Regex.Match(name, "#.+$");
			//if (m.Success)
			//{
			//    name = String.Format("{0}</b> ◆{1}<b>",
			//        name.Substring(0, m.Index),
			//        Twin.Tools.Trip.Create(name.Substring(m.Index + 1)));
			//}
			string name = MakeConvertedName( From );
			if ( name == null )
			{
				name = "◆トリップキーが長すぎます";
			}
			// NTwin23.105

			// メール欄があればリンクを貼る
			if (Email != String.Empty)
			{
				name = String.Format("<a href=\"mailto:{0}\">{1}</a>",
					Email, name);
			}

			// htmlを生成
			string html = "<html><head><title>" + Subject + "</title></head><body bgcolor=\"#efefef\" style=\"font-family: ＭＳ Ｐゴシック; font-size:12pt;\">" +
				"<dl><nobr><dt>" + ((postType == PostType.Thread) ? 1 : headerInfo.ResCount + 1) +
				" 名前：<font color=green><b>" + name + "</b></font> " + date + "</dt><dd>" + body + "</dd></nobr></dl></body></html>";

			viewer.Clear();
			viewer.WriteText(html);
		}

		// NTwin23.105
		private string MakeConvertedName( string from )
		{
			// 名前を作成 (トリップも作成)
			string name = (From != String.Empty) ? From : "名無しさん";
			Match m = Regex.Match( name , "^(?<name>.*?)#(?<key>.+)$" );
			if ( m.Success )
			{
				string key = m.Groups["key"].Value;
				if ( Encoding.GetEncoding( "shift_jis" ).GetByteCount( key ) > 1024 )	// 鳥キーの#を含まない長さの 
				{																		// 上限が1024なのでリミットする
					return null;														// 鳥キーの最初の#は除去が
				}																		// 保証されている
				name = String.Format( "{0}</b> ◆{1}<b>" ,
					m.Groups["name"].Value ,
					Twin.Tools.Trip.Create( m.Groups["key"].Value ) );
			}

			return name;
		}
		// NTwin23.105

		/// <summary>
		/// AA入力支援
		/// </summary>
		private void InputAa()
		{
			Point loc = toolBarButtonAA.Rectangle.Location;
			loc = toolBar.PointToScreen(loc);
			loc = PointToClient(loc);

			AaContext.Show(this, loc, new Point(0, 0));
		}

		/// <summary>
		/// レスを書き込む
		/// </summary>
		private void Write()
		{
			try
			{
				if (!Samba24Checking() || !IsErrorChecking() || !TextLengthCheckingAll())
					return;

				buttonOK.Enabled = false;
				buttonCancel.Enabled = false;
				posting = true;
				sambaTimer.Stop();

				//if (post is X2chPost)
				//{
				//    X2chPost x2ch = (X2chPost)post;
				//    x2ch.SendBeID = checkBoxSendBeID.Checked;
				//}

				// コテハンを作成 (空であればコテハンを削除)
				Kotehan newkote = new Kotehan(From, Email, checkBoxSendBeID.Checked);

				if (postType == PostType.Thread)
				{
					if (Twinie.Settings.Post.ThreadKotehan && !koteman.IsExists(boardInfo))
						koteman.Set(boardInfo, newkote);

					// クッキーを取得
					cookie.GetCookie(boardInfo);

					PostThread thread = new PostThread();
					thread.Subject = Subject;
					thread.From = From;
					thread.Email = Email;
					thread.Body = Body;

					// 板の時刻を取得
					string subject = Path.Combine(cache.GetFolderPath(boardInfo, false), "subject.txt");
					DateTime time = File.GetLastWriteTime(subject);

					post.Time = time;
					post.BeginPost(boardInfo, thread,
						new AsyncCallback(Posting), post);

				}
				else
				{
					// スレッドに書き込むごとにコテハンを保存する場合
					if (Twinie.Settings.Post.ThreadKotehan)
						koteman.Set(headerInfo, newkote);

					// クッキーを取得
					cookie.GetCookie(headerInfo.BoardInfo);

					PostRes res = new PostRes();
					res.From = From;
					res.Email = Email;
					res.Body = Body;

					post.BeginPost(headerInfo, res,
						new AsyncCallback(Posting), post);
				}

				this.BeginWriteTime = DateTime.Now;
			
				Twinie.Settings.Post.NameHistory.Add(From);
				Twinie.Settings.Post.MailHistory.Add(Email);
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		/// <summary>
		/// 本文にテキストを追加
		/// </summary>
		/// <param name="text"></param>
		public void AppendText(string text)
		{
			textBoxBody.Text += text;
			textBoxBody.ScrollToCaret();
		}

		public void CenterParent()
		{
			CenterToParent();
		}

		#region Handlers
		private void checkBoxSage_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBoxSage.Checked)
			{
				comboBoxMail.Enabled = false;
				comboBoxMail.Text = "sage";
			}
			else
			{
				comboBoxMail.Enabled = true;
				comboBoxMail.Text = String.Empty;
			}
		}

		private void toolBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
			}
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonAA)
			{
				InputAa();
			}
			else if (e.Button == toolBarButtonSave)
			{
				SaveDraft();
				Close();
			}
			else if (e.Button == toolBarButtonClear)
			{
				textBoxBody.Clear();
			}
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			Write();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void PostDialog_Load(object sender, System.EventArgs e)
		{
			textBoxBody.SelectionStart = textBoxBody.TextLength;
			textBoxBody.SelectionLength = 0;
			OnResize(new EventArgs());

			textBoxBody.Select();
			textBoxBody.Focus();

			UpdateTitle();
		}

		private void PostDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (Owner != null)
				Owner.Activate();

			if (posting)
			{
				DialogResult result =
					MessageBox.Show(this, "投稿中です。\nそれでも閉じますか？ (動作は保証しません)", "中止確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (result == DialogResult.No)
					e.Cancel = true;
			}
		}

		private void PostDialog_Closed(object sender, System.EventArgs e)
		{
			settingTxtManager.CancelAsync();

			AaContext.Selected -= new AaItemEventHandler(OnAaSelected);
			AaContext.Show(null, Point.Empty, Point.Empty);

			System.Diagnostics.Debug.WriteLine("#PostDialog_Closed");
		}

		private void PostDialog_Activated(object sender, System.EventArgs e)
		{
			if (tempDialogHeight != -1)
				Height = tempDialogHeight;
		}

		private void PostDialog_Deactivate(object sender, System.EventArgs e)
		{
			if (Twinie.Settings.Post.MinimizingDialog && !posting)
			{
				tempDialogHeight = Height;
				Height = 0;
			}
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControl.SelectedTab == tabPagePreview)
			{
				// 初期化
				if (notInit)
				{
					viewer = new IEComPreviewControl(Twinie.Cache, Twinie.Settings);
					viewer.Dock = DockStyle.Fill;
					tabPagePreview.Controls.Add(viewer);
					notInit = false;
				}
				RefreshPreview();
			}
			else if (tabControl.SelectedTab == tabPageSettingTxt)
			{
				toolStripTextBoxUrl.Text = boardInfo.Url + "SETTING.TXT";
			}
			else if (tabControl.SelectedTab == tabPageWrite)
			{
				textBoxBody.Focus();
			}
		}

		private void SettingTxtLoaded(object sender, DownloadDataCompletedEventArgs e)
		{
			downloadingSettingTxt = false;

			MethodInvoker m = delegate
			{
				string text = Regex.Replace(Encoding.GetEncoding("shift_jis").GetString(e.Result), "\n", "\r\n");

				serverSetting.Read(boardInfo, text);
				textBoxSettingTxt.Text = text;
			};

			Invoke(m);
		}

		private void Posting(IAsyncResult ar)
		{
			MethodInvoker m = delegate
			{
				try
				{
					PostBase post = (PostBase)ar.AsyncState;
					post.EndPost(ar);
					posting = false;

					// 投稿成功時に閉じる
					if (post.Response == PostResponse.Success)
					{
						this.EndWriteTime = DateTime.Now;

						if (postType == PostType.Res)
						{
							// 最終書込日を設定
							headerInfo.LastWritten = DateTime.Now;
							ThreadIndexer.SaveLastWritten(cache, headerInfo);
						}

						if (Twinie.Settings.Post.AutoClosing)
						{
							sambaTimer.Stop();
							sambaTimer.Dispose();

							Close();
						}
						else
						{
							textBoxBody.Clear();
							textBoxBody.Focus();
						}
					}

					if (!samba.IsElapsed(boardInfo.ServerName, out sambaCount))
						sambaTimer.Start();

					UpdateTitle();
				}
				catch (Exception ex)
				{
					TwinDll.ShowOutput(ex);
				}
				finally
				{
					buttonOK.Enabled = true;
					buttonCancel.Enabled = true;
				}
			};
			Invoke(m);
		}

		private void OnPosted(object sender, PostEventArgs e)
		{
			MethodInvoker m = delegate
			{
				cookie.SetCookie(boardInfo);

				switch (e.Response)
				{
					case PostResponse.Success:

						PostedEventArgs args = (postType == PostType.Thread) ?
							new PostedEventArgs(boardInfo, new PostThread(Subject, From, Email, Body)) :
							new PostedEventArgs(headerInfo, new PostRes(From, Email, Body));

						OnPostedInternal(this, args);

						break;

					case PostResponse.Cookie:
						DialogResult result = DialogResult.Yes;

						if (Twinie.Settings.Post.ShowCookieDialog ||
							Twinie.Settings.AditionalAgreementField != TwinDll.AditionalAgreementField)
						{
							result = MessageBox.Show(this, e.Text, e.Title,
								MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						}

						if (result == DialogResult.Yes)
						{
							Twinie.Settings.AditionalAgreementField = TwinDll.AditionalAgreementField;
							e.Retry = true;
						}
						break;

					// Sambaエラーの時に、Sambaテーブルの情報が古い場合は新しい情報に修正
					case PostResponse.Samba:
						if (e.SambaCount >= 0 &&
							e.SambaCount != samba[boardInfo.ServerName])
						{
							samba.Correct(boardInfo.ServerName, e.SambaCount);
						}
						goto default;

					default:
						MessageBox.Show(this, e.Text, e.Title,
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
						break;
				}
			};
			Invoke(m);
		}

		private void OnPostError(object sender, PostErrorEventArgs e)
		{
			MethodInvoker m = delegate
			{
				MessageBox.Show(e.Exception.Message, "投稿エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			};
			Invoke(m);
		}

		private void OnAaSelected(object sender, AaItemEventArgs e)
		{
			if (IsDisposed)
				return;

			if (this.Equals(sender))
			{
				string data = e.Item.Data;
				string text = textBoxBody.Text;
				int selection = textBoxBody.SelectionStart;

				textBoxBody.Text = text.Insert(selection, data);
				textBoxBody.SelectionStart = selection + data.Length;
			}
		}

		/// <summary>
		/// Postedイベントを発生させる
		/// </summary>
		private void OnPostedInternal(object sender, PostedEventArgs e)
		{
			if (Posted != null)
				Posted(this, e);
		}

		private void textBoxBody_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// Shift + Enter は書き込み
			if ((ModifierKeys & Keys.Shift) != 0 && e.KeyCode == Keys.Enter)
			{
				Write();
				e.Handled = true;
			}
			// Ctrl + Space はAA入力
			else if ((ModifierKeys & Keys.Control) != 0 && e.KeyCode == Keys.Space)
			{
				InputAa();
				e.Handled = true;
			}
			// Ctrl + L はプレビュータブ
			else if ((ModifierKeys & Keys.Control) != 0 && e.KeyCode == Keys.L)
			{
				tabControl.SelectedTab = tabPagePreview;
				e.Handled = true;
			}
			// Ctrl + Backspace は全消去
			else if ((ModifierKeys & Keys.Control) != 0 && e.KeyCode == Keys.Back)
			{
				textBoxBody.SelectAll();
				textBoxBody.Cut();
				e.Handled = true;
			}
			// Ctrl + A は全選択
			else if ((ModifierKeys & Keys.Control) != 0 && e.KeyCode == Keys.A)
			{
				textBoxBody.SelectAll();
				e.Handled = true;
			}
		}

		private void PostDialog_Resize(object sender, System.EventArgs e)
		{
		}
		#endregion

		private void textBoxBody_TextChanged(object sender, EventArgs e)
		{
			if (!serverSetting.IsLoaded || boardInfo.Bbs != BbsType.X2ch)
				return;

			int byteCount = Encoding.GetEncoding("shift_jis").GetByteCount(textBoxBody.Text);
			// 改行は \r\n から <br> に変換されるため、2バイトプラス
			byteCount += 2 * Regex.Matches(textBoxBody.Text, "\r\n").Count;

			int maxByteCount = serverSetting.MessageCount;
			int lineCount = textBoxBody.Lines.Length;
			int maxLineCount = serverSetting.LineNumber * 2;

			// 文字数 (最大文字数)
			// 行数 (最大行数)
			labelCharInfo.Text = String.Format("{0} ({1})\r\n{2} ({3})",
				byteCount, maxByteCount, lineCount, maxLineCount);

			if (byteCount > maxByteCount || lineCount > maxLineCount)
			{
				labelCharInfo.ForeColor = Color.Red;
			}
			else
			{
				labelCharInfo.ForeColor = SystemColors.ControlDark;
			}
		}

		private void toolStripButtonRefresh_Click(object sender, EventArgs e)
		{
			if (!downloadingSettingTxt)
			{
				downloadingSettingTxt = true;

				settingTxtManager.BeginDownload(boardInfo, SettingTxtLoaded);
			}
		}

		private void checkBoxOyster_CheckedChanged(object sender, EventArgs e)
		{
			bool check = checkBoxOyster.Checked;
			if (Twinie.Settings.Authentication.AuthenticationOn != check)
			{
				if (check)
				{
					checkBoxOyster.Text = "●";
					Twinie.OysterLogon();
				}
				else
				{
					checkBoxOyster.Text = "○";
					Twinie.OysterLogout();
				}
			}
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				this.Location = new Point { X = 0, Y = 0 };
				this.Size = new Size { Width = 400, Height = 300 };
			}
			return base.ProcessDialogKey(keyData);
		}

	}
}
