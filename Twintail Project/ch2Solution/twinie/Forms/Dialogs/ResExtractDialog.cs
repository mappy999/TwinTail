// ResExtractDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using Twin.Text;
	using CSharpSamples;

	/// <summary>
	/// レスを抽出するためのダイアログ
	/// </summary>
	public class ResExtractDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonExtract;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.CheckBox checkBoxPopup;
		private System.Windows.Forms.CheckBox checkBoxRegex;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBoxElement;

		private SearchSettings.ResExtractSettings settings;
		private ComboBox comboBox1;
		private AbstractExtractor extractor;

		/// <summary>
		/// 検索キーワードを取得または設定
		/// </summary>
		public string Keyword {
			set { comboBox1.Text = value; }
			get { return comboBox1.Text; }
		}

//		/// <summary>
//		/// 検索対象の要素を取得または設定
//		/// </summary>
//		public ResSetElement SearchTarget {
//			set { comboBoxElement.SelectedItem = value; }
//			get { return (ResSetElement)comboBoxElement.SelectedItem; }
//		}

		/// <summary>
		/// 検索対象の要素を取得または設定
		/// </summary>
		public ResSetElement SearchTarget {
			set {
				comboBoxElement.SelectedIndex = (int)value;
			}
			get {
				return (ResSetElement)Enum.Parse(typeof(ResSetElement), 
					comboBoxElement.SelectedIndex.ToString());
			}
		}

		/// <summary>
		/// 検索オプションを取得または設定
		/// </summary>
		public SearchOptions Options {
			set {
				checkBoxRegex.Checked = (value & SearchOptions.Regex) != 0;
			}
			get {
				SearchOptions flags = SearchOptions.None;
				if (checkBoxRegex.Checked) flags |= SearchOptions.Regex;
				return flags;
			}
		}

		/// <summary>
		/// ResExtractDialogクラスのインスタンスを初期化
		/// </summary>
		public ResExtractDialog(AbstractExtractor rer)
		{
			if (rer == null) {
				throw new ArgumentNullException("rer");
			}
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			extractor = rer;
			comboBoxElement.SelectedIndex = 0;
			settings = new SearchSettings.ResExtractSettings();

//			Array array = Enum.GetValues(typeof(ResSetCollection));
//			foreach (Enum e in array)
//				comboBoxElement.Items.Add(e);

			comboBox1.Items.AddRange(Twinie.Settings.Search.SearchHistory.Keys.ToArray());
		}

		/// <summary>
		/// ResExtractDialogクラスのインスタンスを初期化
		/// </summary>
		public ResExtractDialog(AbstractExtractor rer, 
			SearchSettings.ResExtractSettings sett) : this(rer)
		{
			if (sett == null) {
				throw new ArgumentNullException("sett");
			}
			SetSettings(sett);
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
			this.label1 = new System.Windows.Forms.Label();
			this.buttonExtract = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.checkBoxPopup = new System.Windows.Forms.CheckBox();
			this.checkBoxRegex = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxElement = new System.Windows.Forms.ComboBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "検索キーワード(&D)";
			// 
			// buttonExtract
			// 
			this.buttonExtract.AutoSize = true;
			this.buttonExtract.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonExtract.Location = new System.Drawing.Point(284, 4);
			this.buttonExtract.Name = "buttonExtract";
			this.buttonExtract.Size = new System.Drawing.Size(75, 21);
			this.buttonExtract.TabIndex = 2;
			this.buttonExtract.Text = "抽出";
			this.buttonExtract.Click += new System.EventHandler(this.buttonExtract_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(284, 28);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(74, 21);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "閉じる";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// checkBoxPopup
			// 
			this.checkBoxPopup.AutoSize = true;
			this.checkBoxPopup.Checked = true;
			this.checkBoxPopup.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxPopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxPopup.Location = new System.Drawing.Point(16, 56);
			this.checkBoxPopup.Name = "checkBoxPopup";
			this.checkBoxPopup.Size = new System.Drawing.Size(120, 17);
			this.checkBoxPopup.TabIndex = 6;
			this.checkBoxPopup.Text = "ポップアップ表示(&P)";
			// 
			// checkBoxRegex
			// 
			this.checkBoxRegex.AutoSize = true;
			this.checkBoxRegex.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxRegex.Location = new System.Drawing.Point(16, 72);
			this.checkBoxRegex.Name = "checkBoxRegex";
			this.checkBoxRegex.Size = new System.Drawing.Size(94, 17);
			this.checkBoxRegex.TabIndex = 7;
			this.checkBoxRegex.Text = "正規表現(&G)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(16, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "検索対象(&T)";
			// 
			// comboBoxElement
			// 
			this.comboBoxElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxElement.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxElement.Items.AddRange(new object[] {
            "すべて",
            "名前",
            "メール欄",
            "ID",
            "本文"});
			this.comboBoxElement.Location = new System.Drawing.Point(100, 28);
			this.comboBoxElement.Name = "comboBoxElement";
			this.comboBoxElement.Size = new System.Drawing.Size(100, 20);
			this.comboBoxElement.TabIndex = 5;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(100, 2);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(171, 20);
			this.comboBox1.TabIndex = 1;
			// 
			// ResExtractDialog
			// 
			this.AcceptButton = this.buttonExtract;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(375, 104);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxElement);
			this.Controls.Add(this.checkBoxRegex);
			this.Controls.Add(this.checkBoxPopup);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonExtract);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ResExtractDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "レスの抽出";
			this.Closed += new System.EventHandler(this.ResExtractDialog_Closed);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ResExtractDialog_Closing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 設定を適用
		/// </summary>
		/// <param name="sett"></param>
		private void SetSettings(SearchSettings.ResExtractSettings sett)
		{
			Keyword = sett.Keyword;
			Options = sett.Options;
			checkBoxPopup.Checked = sett.Popup;
			SearchTarget = sett.SearchTarget;
			settings = sett;
		}

		/// <summary>
		/// 設定を保存
		/// </summary>
		private void SaveSettings()
		{
			settings.Keyword = Keyword;
			settings.Options = Options;
			settings.Popup = checkBoxPopup.Checked;
			settings.SearchTarget = SearchTarget;
		}

		private void buttonExtract_Click(object sender, System.EventArgs e)
		{
			if (Keyword == String.Empty)
			{
				MessageBox.Show(this, "検索キーワードを入力してください", "検索できません。。",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try {
				Cursor = Cursors.WaitCursor;
				extractor.Options = Options;
				extractor.NewWindow = checkBoxPopup.Checked;

				Twinie.Settings.Search.SearchHistory.Add(Keyword);

				bool r = extractor.InnerExtract(Keyword, SearchTarget);

				if (r) this.Close();
				else {
					MessageBox.Show(this, "'" + Keyword + "' は見つかりませんでした", "抽出終了",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			finally { Cursor = Cursors.Default; }
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ResExtractDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		private void ResExtractDialog_Closed(object sender, System.EventArgs e)
		{
			SaveSettings();

			if (Owner != null)
				Owner.Activate();
		}
	}
}
