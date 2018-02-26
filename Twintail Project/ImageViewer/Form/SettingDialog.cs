// SettingDialog.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using CSharpSamples;
	using System.Collections.Generic;

	/// <summary>
	/// SettingDialog の概要の説明です。
	/// </summary>
	public class SettingDialog : System.Windows.Forms.Form
	{
		private FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
		private ImageViewerSettings settings;
		private ServerRestrictSettings serverRestrictSettings;
		#region Designer Fields
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private System.Windows.Forms.TextBox textBoxCacheFolder;
		private System.Windows.Forms.Button buttonRefCacheFolder;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabPageSaveFolder;
		private System.Windows.Forms.ListBox listBoxSaveFolders;
		private System.Windows.Forms.Button buttonRemoveSaveFolder;
		private System.Windows.Forms.Button buttonAddSaveFolder;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.CheckBox checkBoxMosaic;
		private System.Windows.Forms.Button buttonEditSaveFolder;
		private System.Windows.Forms.CheckBox checkBoxActivate;
		private System.Windows.Forms.TextBox textBoxWebBrowserPath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonRefWebBrowserPath;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkBoxAutoHide;
		private System.Windows.Forms.CheckBox checkBoxNoOverwrite;
		private CheckBox checkBoxViewOriginalSize;
		private CheckBox checkBoxSetuyaku;
		private GroupBox groupBox3;
		private GroupBox groupBox2;
		private GroupBox groupBox1;
		private TabPage tabPage1;
		private GroupBox groupBox4;
		private Label label8;
		private Label label7;
		private Label label6;
		private Label label5;
		private TextBox textBoxServer;
		private NumericUpDown numericUpDownInterval;
		private NumericUpDown numericUpDownLimit;
		private Button buttonUpdate;
		private Button buttonAdd;
		private ListView listView1;
		private ColumnHeader columnHeaderServer;
		private ColumnHeader columnHeaderLimit;
		private ColumnHeader columnHeaderInterval;
		private Button buttonDel;
		private Label label4;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		/// <summary>
		/// SettingDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="settings"></param>
		public SettingDialog(ImageViewerSettings settings, ServerRestrictSettings restrict)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			SetSettings(settings);
			SetRestrictSettings(restrict);
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				folderBrowser.Dispose();

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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkBoxActivate = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoHide = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBoxNoOverwrite = new System.Windows.Forms.CheckBox();
			this.checkBoxSetuyaku = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxMosaic = new System.Windows.Forms.CheckBox();
			this.checkBoxViewOriginalSize = new System.Windows.Forms.CheckBox();
			this.buttonRefWebBrowserPath = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxWebBrowserPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonRefCacheFolder = new System.Windows.Forms.Button();
			this.textBoxCacheFolder = new System.Windows.Forms.TextBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.buttonDel = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxServer = new System.Windows.Forms.TextBox();
			this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownLimit = new System.Windows.Forms.NumericUpDown();
			this.buttonUpdate = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeaderServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderInterval = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tabPageSaveFolder = new System.Windows.Forms.TabPage();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonEditSaveFolder = new System.Windows.Forms.Button();
			this.buttonAddSaveFolder = new System.Windows.Forms.Button();
			this.buttonRemoveSaveFolder = new System.Windows.Forms.Button();
			this.listBoxSaveFolders = new System.Windows.Forms.ListBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.tabControl.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimit)).BeginInit();
			this.tabPageSaveFolder.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageGeneral);
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPageSaveFolder);
			this.tabControl.ItemSize = new System.Drawing.Size(96, 22);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(483, 330);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl.TabIndex = 0;
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.Controls.Add(this.groupBox3);
			this.tabPageGeneral.Controls.Add(this.groupBox2);
			this.tabPageGeneral.Controls.Add(this.groupBox1);
			this.tabPageGeneral.Controls.Add(this.buttonRefWebBrowserPath);
			this.tabPageGeneral.Controls.Add(this.label2);
			this.tabPageGeneral.Controls.Add(this.textBoxWebBrowserPath);
			this.tabPageGeneral.Controls.Add(this.label1);
			this.tabPageGeneral.Controls.Add(this.buttonRefCacheFolder);
			this.tabPageGeneral.Controls.Add(this.textBoxCacheFolder);
			this.tabPageGeneral.Location = new System.Drawing.Point(4, 26);
			this.tabPageGeneral.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.Size = new System.Drawing.Size(475, 300);
			this.tabPageGeneral.TabIndex = 0;
			this.tabPageGeneral.Text = "全般設定";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkBoxActivate);
			this.groupBox3.Controls.Add(this.checkBoxAutoHide);
			this.groupBox3.Location = new System.Drawing.Point(18, 100);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(440, 70);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "表示";
			// 
			// checkBoxActivate
			// 
			this.checkBoxActivate.AutoSize = true;
			this.checkBoxActivate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxActivate.Location = new System.Drawing.Point(11, 19);
			this.checkBoxActivate.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxActivate.Name = "checkBoxActivate";
			this.checkBoxActivate.Size = new System.Drawing.Size(223, 17);
			this.checkBoxActivate.TabIndex = 5;
			this.checkBoxActivate.Text = "画像を開くたびにビューアをアクティブにする";
			// 
			// checkBoxAutoHide
			// 
			this.checkBoxAutoHide.AutoSize = true;
			this.checkBoxAutoHide.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAutoHide.Location = new System.Drawing.Point(11, 40);
			this.checkBoxAutoHide.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAutoHide.Name = "checkBoxAutoHide";
			this.checkBoxAutoHide.Size = new System.Drawing.Size(257, 17);
			this.checkBoxAutoHide.TabIndex = 9;
			this.checkBoxAutoHide.Text = "すべての画像を閉じたときビューアを非表示にする";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBoxNoOverwrite);
			this.groupBox2.Controls.Add(this.checkBoxSetuyaku);
			this.groupBox2.Location = new System.Drawing.Point(185, 176);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(273, 71);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "その他";
			// 
			// checkBoxNoOverwrite
			// 
			this.checkBoxNoOverwrite.AutoSize = true;
			this.checkBoxNoOverwrite.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxNoOverwrite.Location = new System.Drawing.Point(8, 19);
			this.checkBoxNoOverwrite.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxNoOverwrite.Name = "checkBoxNoOverwrite";
			this.checkBoxNoOverwrite.Size = new System.Drawing.Size(244, 17);
			this.checkBoxNoOverwrite.TabIndex = 10;
			this.checkBoxNoOverwrite.Text = "同名のファイルが存在したとき別名で保存する";
			// 
			// checkBoxSetuyaku
			// 
			this.checkBoxSetuyaku.AutoSize = true;
			this.checkBoxSetuyaku.Location = new System.Drawing.Point(8, 41);
			this.checkBoxSetuyaku.Name = "checkBoxSetuyaku";
			this.checkBoxSetuyaku.Size = new System.Drawing.Size(195, 16);
			this.checkBoxSetuyaku.TabIndex = 12;
			this.checkBoxSetuyaku.Text = "メモリを節約 (次回起動時から有効)";
			this.checkBoxSetuyaku.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBoxMosaic);
			this.groupBox1.Controls.Add(this.checkBoxViewOriginalSize);
			this.groupBox1.Location = new System.Drawing.Point(18, 176);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(159, 71);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "デフォルトの動作";
			// 
			// checkBoxMosaic
			// 
			this.checkBoxMosaic.AutoSize = true;
			this.checkBoxMosaic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxMosaic.Location = new System.Drawing.Point(11, 19);
			this.checkBoxMosaic.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxMosaic.Name = "checkBoxMosaic";
			this.checkBoxMosaic.Size = new System.Drawing.Size(123, 17);
			this.checkBoxMosaic.TabIndex = 4;
			this.checkBoxMosaic.Text = "画像にモザイク処理";
			// 
			// checkBoxViewOriginalSize
			// 
			this.checkBoxViewOriginalSize.AutoSize = true;
			this.checkBoxViewOriginalSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxViewOriginalSize.Location = new System.Drawing.Point(11, 40);
			this.checkBoxViewOriginalSize.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxViewOriginalSize.Name = "checkBoxViewOriginalSize";
			this.checkBoxViewOriginalSize.Size = new System.Drawing.Size(133, 17);
			this.checkBoxViewOriginalSize.TabIndex = 11;
			this.checkBoxViewOriginalSize.Text = "画像を原寸大で表示";
			this.checkBoxViewOriginalSize.UseVisualStyleBackColor = true;
			// 
			// buttonRefWebBrowserPath
			// 
			this.buttonRefWebBrowserPath.AutoSize = true;
			this.buttonRefWebBrowserPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRefWebBrowserPath.Location = new System.Drawing.Point(294, 72);
			this.buttonRefWebBrowserPath.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRefWebBrowserPath.Name = "buttonRefWebBrowserPath";
			this.buttonRefWebBrowserPath.Size = new System.Drawing.Size(80, 26);
			this.buttonRefWebBrowserPath.TabIndex = 8;
			this.buttonRefWebBrowserPath.Text = "参照...";
			this.buttonRefWebBrowserPath.Click += new System.EventHandler(this.buttonRefWebBrowserPath_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(16, 60);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(200, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "画像を開くときに使用するブラウザへのパス";
			// 
			// textBoxWebBrowserPath
			// 
			this.textBoxWebBrowserPath.Location = new System.Drawing.Point(16, 76);
			this.textBoxWebBrowserPath.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxWebBrowserPath.Name = "textBoxWebBrowserPath";
			this.textBoxWebBrowserPath.Size = new System.Drawing.Size(274, 19);
			this.textBoxWebBrowserPath.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(255, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "画像キャッシュの保存フォルダ (次回起動時から有効)";
			// 
			// buttonRefCacheFolder
			// 
			this.buttonRefCacheFolder.AutoSize = true;
			this.buttonRefCacheFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRefCacheFolder.Location = new System.Drawing.Point(294, 28);
			this.buttonRefCacheFolder.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRefCacheFolder.Name = "buttonRefCacheFolder";
			this.buttonRefCacheFolder.Size = new System.Drawing.Size(80, 26);
			this.buttonRefCacheFolder.TabIndex = 2;
			this.buttonRefCacheFolder.Text = "参照...";
			this.buttonRefCacheFolder.Click += new System.EventHandler(this.buttonRefCacheFolder_Click);
			// 
			// textBoxCacheFolder
			// 
			this.textBoxCacheFolder.Location = new System.Drawing.Point(16, 32);
			this.textBoxCacheFolder.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxCacheFolder.Name = "textBoxCacheFolder";
			this.textBoxCacheFolder.Size = new System.Drawing.Size(274, 19);
			this.textBoxCacheFolder.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.groupBox4);
			this.tabPage1.Controls.Add(this.listView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 26);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(475, 300);
			this.tabPage1.TabIndex = 2;
			this.tabPage1.Text = "接続制限";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(28, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(324, 12);
			this.label4.TabIndex = 3;
			this.label4.Text = "※このタブの設定は、一度変更したらキャンセルしても元に戻せません";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.buttonDel);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.textBoxServer);
			this.groupBox4.Controls.Add(this.numericUpDownInterval);
			this.groupBox4.Controls.Add(this.numericUpDownLimit);
			this.groupBox4.Controls.Add(this.buttonUpdate);
			this.groupBox4.Controls.Add(this.buttonAdd);
			this.groupBox4.Location = new System.Drawing.Point(10, 147);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(457, 121);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "登録情報";
			// 
			// buttonDel
			// 
			this.buttonDel.Location = new System.Drawing.Point(376, 78);
			this.buttonDel.Name = "buttonDel";
			this.buttonDel.Size = new System.Drawing.Size(75, 23);
			this.buttonDel.TabIndex = 9;
			this.buttonDel.Text = "削除";
			this.buttonDel.UseVisualStyleBackColor = true;
			this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(148, 81);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(31, 12);
			this.label8.TabIndex = 8;
			this.label8.Text = "ミリ秒";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(16, 81);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(55, 12);
			this.label7.TabIndex = 7;
			this.label7.Text = "接続間隔:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(4, 56);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 12);
			this.label6.TabIndex = 6;
			this.label6.Text = "同時接続数:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(22, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(49, 12);
			this.label5.TabIndex = 5;
			this.label5.Text = "サーバ名:";
			// 
			// textBoxServer
			// 
			this.textBoxServer.Location = new System.Drawing.Point(77, 22);
			this.textBoxServer.Name = "textBoxServer";
			this.textBoxServer.Size = new System.Drawing.Size(278, 19);
			this.textBoxServer.TabIndex = 4;
			// 
			// numericUpDownInterval
			// 
			this.numericUpDownInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownInterval.Location = new System.Drawing.Point(77, 77);
			this.numericUpDownInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.numericUpDownInterval.Name = "numericUpDownInterval";
			this.numericUpDownInterval.Size = new System.Drawing.Size(65, 19);
			this.numericUpDownInterval.TabIndex = 3;
			this.numericUpDownInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// numericUpDownLimit
			// 
			this.numericUpDownLimit.Location = new System.Drawing.Point(77, 52);
			this.numericUpDownLimit.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.numericUpDownLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownLimit.Name = "numericUpDownLimit";
			this.numericUpDownLimit.ReadOnly = true;
			this.numericUpDownLimit.Size = new System.Drawing.Size(65, 19);
			this.numericUpDownLimit.TabIndex = 2;
			this.numericUpDownLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// buttonUpdate
			// 
			this.buttonUpdate.Location = new System.Drawing.Point(376, 49);
			this.buttonUpdate.Name = "buttonUpdate";
			this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
			this.buttonUpdate.TabIndex = 1;
			this.buttonUpdate.Text = "更新";
			this.buttonUpdate.UseVisualStyleBackColor = true;
			this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(376, 20);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(75, 23);
			this.buttonAdd.TabIndex = 0;
			this.buttonAdd.Text = "追加";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderServer,
            this.columnHeaderLimit,
            this.columnHeaderInterval});
			this.listView1.FullRowSelect = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(8, 29);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(459, 114);
			this.listView1.TabIndex = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// columnHeaderServer
			// 
			this.columnHeaderServer.Text = "サーバ名";
			this.columnHeaderServer.Width = 180;
			// 
			// columnHeaderLimit
			// 
			this.columnHeaderLimit.Text = "同時接続数";
			this.columnHeaderLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeaderLimit.Width = 75;
			// 
			// columnHeaderInterval
			// 
			this.columnHeaderInterval.Text = "接続間隔";
			this.columnHeaderInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tabPageSaveFolder
			// 
			this.tabPageSaveFolder.Controls.Add(this.label3);
			this.tabPageSaveFolder.Controls.Add(this.buttonEditSaveFolder);
			this.tabPageSaveFolder.Controls.Add(this.buttonAddSaveFolder);
			this.tabPageSaveFolder.Controls.Add(this.buttonRemoveSaveFolder);
			this.tabPageSaveFolder.Controls.Add(this.listBoxSaveFolders);
			this.tabPageSaveFolder.Location = new System.Drawing.Point(4, 26);
			this.tabPageSaveFolder.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageSaveFolder.Name = "tabPageSaveFolder";
			this.tabPageSaveFolder.Size = new System.Drawing.Size(475, 300);
			this.tabPageSaveFolder.TabIndex = 1;
			this.tabPageSaveFolder.Text = "保存フォルダ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(14, 10);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(209, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "ドラッグ＆ドロップするとまとめて登録できます";
			// 
			// buttonEditSaveFolder
			// 
			this.buttonEditSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonEditSaveFolder.AutoSize = true;
			this.buttonEditSaveFolder.Enabled = false;
			this.buttonEditSaveFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonEditSaveFolder.Location = new System.Drawing.Point(286, 248);
			this.buttonEditSaveFolder.Margin = new System.Windows.Forms.Padding(2);
			this.buttonEditSaveFolder.Name = "buttonEditSaveFolder";
			this.buttonEditSaveFolder.Size = new System.Drawing.Size(82, 27);
			this.buttonEditSaveFolder.TabIndex = 2;
			this.buttonEditSaveFolder.Text = "編集";
			this.buttonEditSaveFolder.Click += new System.EventHandler(this.buttonEditSaveFolder_Click);
			// 
			// buttonAddSaveFolder
			// 
			this.buttonAddSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddSaveFolder.AutoSize = true;
			this.buttonAddSaveFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAddSaveFolder.Location = new System.Drawing.Point(196, 248);
			this.buttonAddSaveFolder.Margin = new System.Windows.Forms.Padding(2);
			this.buttonAddSaveFolder.Name = "buttonAddSaveFolder";
			this.buttonAddSaveFolder.Size = new System.Drawing.Size(82, 27);
			this.buttonAddSaveFolder.TabIndex = 1;
			this.buttonAddSaveFolder.Text = "追加";
			this.buttonAddSaveFolder.Click += new System.EventHandler(this.buttonAddSaveFolder_Click);
			// 
			// buttonRemoveSaveFolder
			// 
			this.buttonRemoveSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRemoveSaveFolder.AutoSize = true;
			this.buttonRemoveSaveFolder.Enabled = false;
			this.buttonRemoveSaveFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRemoveSaveFolder.Location = new System.Drawing.Point(375, 248);
			this.buttonRemoveSaveFolder.Margin = new System.Windows.Forms.Padding(2);
			this.buttonRemoveSaveFolder.Name = "buttonRemoveSaveFolder";
			this.buttonRemoveSaveFolder.Size = new System.Drawing.Size(83, 27);
			this.buttonRemoveSaveFolder.TabIndex = 3;
			this.buttonRemoveSaveFolder.Text = "削除";
			this.buttonRemoveSaveFolder.Click += new System.EventHandler(this.buttonRemoveSaveFolder_Click);
			// 
			// listBoxSaveFolders
			// 
			this.listBoxSaveFolders.AllowDrop = true;
			this.listBoxSaveFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxSaveFolders.IntegralHeight = false;
			this.listBoxSaveFolders.ItemHeight = 12;
			this.listBoxSaveFolders.Location = new System.Drawing.Point(16, 30);
			this.listBoxSaveFolders.Margin = new System.Windows.Forms.Padding(2);
			this.listBoxSaveFolders.Name = "listBoxSaveFolders";
			this.listBoxSaveFolders.Size = new System.Drawing.Size(442, 212);
			this.listBoxSaveFolders.TabIndex = 0;
			this.listBoxSaveFolders.SelectedIndexChanged += new System.EventHandler(this.listBoxSaveFolders_SelectedIndexChanged);
			this.listBoxSaveFolders.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxSaveFolders_DragDrop);
			this.listBoxSaveFolders.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxSaveFolders_DragEnter);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(243, 330);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(88, 25);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(151, 330);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(88, 25);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "実行ファイル (*.exe)|*.exe";
			// 
			// SettingDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(483, 356);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "SettingDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "環境設定";
			this.tabControl.ResumeLayout(false);
			this.tabPageGeneral.ResumeLayout(false);
			this.tabPageGeneral.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimit)).EndInit();
			this.tabPageSaveFolder.ResumeLayout(false);
			this.tabPageSaveFolder.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void SetSettings(ImageViewerSettings sett)
		{
			this.settings = sett;

			// 全般設定
			textBoxCacheFolder.Text = sett.ImageCacheFolder;
			textBoxWebBrowserPath.Text = sett.WebBrowserPath;
			checkBoxActivate.Checked = sett.Activate;
			checkBoxMosaic.Checked = sett.Mosaic;
			checkBoxAutoHide.Checked = sett.AutoHide;
			checkBoxNoOverwrite.Checked = sett.NoOverwirte;
			checkBoxViewOriginalSize.Checked = sett.ViewOriginalSize;
			checkBoxSetuyaku.Checked = sett.SavingMemory;

			// 保存フォルダ設定
			foreach (QuickSaveFolderItem item in sett.QuickSaveFolders)
				listBoxSaveFolders.Items.Add(item);
		}

		private void GetSettings(ImageViewerSettings sett)
		{
			// 全般設定
			sett.ImageCacheFolder = textBoxCacheFolder.Text;
			sett.WebBrowserPath = textBoxWebBrowserPath.Text;
			sett.Activate = checkBoxActivate.Checked;
			sett.Mosaic = checkBoxMosaic.Checked;
			sett.AutoHide = checkBoxAutoHide.Checked;
			sett.NoOverwirte = checkBoxNoOverwrite.Checked;
			sett.ViewOriginalSize = checkBoxViewOriginalSize.Checked;
			sett.SavingMemory = checkBoxSetuyaku.Checked;

			sett.QuickSaveFolders.Clear();

			// 保存フォルダ設定
			foreach (QuickSaveFolderItem item in listBoxSaveFolders.Items)
				sett.QuickSaveFolders.Add(item);
		}

		private void SetRestrictSettings(ServerRestrictSettings restrict)
		{
			this.serverRestrictSettings = restrict;
			var lvi = new List<ListViewItem>();

			foreach (ServerRestrictInfo info in restrict.RestrictList)
			{
				ListViewItem t = new ListViewItem();
				t.Text = info.ServerAddress;
				t.SubItems.Add(info.ConnectionLimit.ToString());
				t.SubItems.Add(info.Interval.ToString());
				t.SubItems.Add(info.Referer);
				t.Tag = info;
				lvi.Add(t);
			}
			listView1.Items.AddRange(lvi.ToArray());
		}

		private void buttonRefCacheFolder_Click(object sender, System.EventArgs e)
		{
			folderBrowser.Description = "画像キャッシュの保存先フォルダを指定してください";

			if (folderBrowser.ShowDialog(this) == DialogResult.OK)
				textBoxCacheFolder.Text = folderBrowser.SelectedPath;
		}

		private void buttonAddSaveFolder_Click(object sender, System.EventArgs e)
		{
			EditQuickSaveFolderDialog dlg = new EditQuickSaveFolderDialog();

			if (dlg.ShowDialog(this) == DialogResult.OK)
				listBoxSaveFolders.Items.Add(dlg.Item);
		}

		private void buttonEditSaveFolder_Click(object sender, System.EventArgs e)
		{
			EditQuickSaveFolderDialog dlg = new EditQuickSaveFolderDialog();
			dlg.Item = (QuickSaveFolderItem)listBoxSaveFolders.SelectedItem;

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				int index = listBoxSaveFolders.SelectedIndex;
				listBoxSaveFolders.Items.RemoveAt(index);
				listBoxSaveFolders.Items.Insert(index, dlg.Item);
			}
		}

		private void buttonRemoveSaveFolder_Click(object sender, System.EventArgs e)
		{
			int index = listBoxSaveFolders.SelectedIndex;

			if (index >= 0)
			{
				listBoxSaveFolders.Items.RemoveAt(index);
				listBoxSaveFolders.SelectedIndex = Math.Min(listBoxSaveFolders.Items.Count-1, index);
			}
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			GetSettings(settings);

			// すぐに設定ファイルを保存してみる
			ImageViewerSettings.Save(
				ImageViewerSettings.SettingPath, settings);

			Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void listBoxSaveFolders_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			buttonRemoveSaveFolder.Enabled =
				buttonEditSaveFolder.Enabled = (listBoxSaveFolders.SelectedIndex != -1);
		}

		private void buttonRefWebBrowserPath_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
				textBoxWebBrowserPath.Text = openFileDialog.FileName;
		}

		private void listBoxSaveFolders_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ?
				DragDropEffects.Copy : DragDropEffects.None;
		}

		private void listBoxSaveFolders_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			string[] dropData = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach (string filePath in dropData)
			{
				// ディレクトリのみ許可
				if (Directory.Exists(filePath))
				{
					QuickSaveFolderItem item = new QuickSaveFolderItem(filePath,
						Path.GetFileName(filePath), Shortcut.None);

					listBoxSaveFolders.Items.Add(item);
				}
			}
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			if (textBoxServer.Text == String.Empty)
				MessageBox.Show("サーバ名を入力してください");
			else
			{
				var newInfo = new ServerRestrictInfo();
				ListViewItem item = GetUpdateInfo(null, newInfo);
				listView1.Items.Add(item);
				listView1.SelectedItems.Clear();
				item.Selected = true;
				this.serverRestrictSettings.RestrictList.Add(newInfo);
			}
		}

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count < 1)
				return;

			if (textBoxServer.Text == String.Empty)
				MessageBox.Show("サーバ名を入力してください");
			else
			{
				var item = listView1.SelectedItems[0];
				var info = (ServerRestrictInfo)item.Tag;

				GetUpdateInfo(item, info);
				Console.Beep(926, 100);
			}
		}

		private ListViewItem GetUpdateInfo(ListViewItem item, ServerRestrictInfo info)
		{
			info.ServerAddress = textBoxServer.Text;
			//info.Referer = textBoxReferer.Text;
			info.ConnectionLimit = (int)numericUpDownLimit.Value;
			info.Interval = (int)numericUpDownInterval.Value;

			if (item == null)
			{
				item = new ListViewItem() { Tag = info };
				item.SubItems.Add(String.Empty);
				item.SubItems.Add(String.Empty);
				item.SubItems.Add(String.Empty);
			}
			item.Text = textBoxServer.Text;
			item.SubItems[1].Text = numericUpDownLimit.Value.ToString();
			item.SubItems[2].Text = numericUpDownInterval.Value.ToString();
		//	item.SubItems[3].Text = textBoxReferer.Text;

			return item;
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count < 1)
				return;
			var info = (ServerRestrictInfo)listView1.SelectedItems[0].Tag;
			textBoxServer.Text = info.ServerAddress;
			numericUpDownInterval.Value = info.Interval;
			numericUpDownLimit.Value = info.ConnectionLimit;
		}

		private void buttonDel_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count < 1)
				return;
			
			int index = Math.Min(listView1.SelectedIndices[0], listView1.Items.Count-2);
			
			var info = (ServerRestrictInfo)listView1.SelectedItems[0].Tag;
			listView1.Items.Remove(listView1.SelectedItems[0]);
			this.serverRestrictSettings.RestrictList.Remove(info);
		
			if (listView1.Items.Count > 0)
				listView1.SelectedIndices.Add(index);
		}

	}
}
