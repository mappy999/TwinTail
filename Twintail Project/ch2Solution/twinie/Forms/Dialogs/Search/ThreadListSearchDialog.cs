// ThreadListSearchDialog.cs

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
	/// スレッド一覧を検索するためのダイアログ
	/// </summary>
	public class ThreadListSearchDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.CheckBox checkBoxMatchCase;
		private System.Windows.Forms.CheckBox checkBoxRegex;
		private System.Windows.Forms.CheckBox checkBoxIncSearch;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonClose;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private SearchSettings.ListSearchSettings settings;
		private AbstractSearcher listSearcher;
		private System.Windows.Forms.Button buttonBack;
		private ComboBox comboBoxKey;
		private Timer refreshTimer;

		/// <summary>
		/// 検索キーワードを取得または設定
		/// </summary>
		public string Keyword {
			set { comboBoxKey.Text = value; }
			get
			{
				return comboBoxKey.Text;
			}
		}

		/// <summary>
		/// 検索オプションを取得または設定
		/// </summary>
		public SearchOptions Options {
			set {
				checkBoxRegex.Checked = (value & SearchOptions.Regex) != 0;
				checkBoxMatchCase.Checked = (value & SearchOptions.MatchCase) != 0;
			}
			get {
				SearchOptions flags = SearchOptions.None;
				if (checkBoxRegex.Checked) flags |= SearchOptions.Regex;
				if (checkBoxMatchCase.Checked) flags |= SearchOptions.MatchCase;
				return flags;
			}
		}

		/// <summary>
		/// ThreadListSearchDialogクラスのインスタンスを初期化
		/// </summary>
		public ThreadListSearchDialog(AbstractSearcher searcher)
		{
			if (searcher == null) {
				throw new ArgumentNullException("searcher");
			}
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			listSearcher = searcher;
			refreshTimer = new Timer();
			refreshTimer.Interval = 500;
			refreshTimer.Tick += new EventHandler(OnRefreshTimer);
			settings = new SearchSettings.ListSearchSettings();

			comboBoxKey.Items.AddRange(Twinie.Settings.Search.SearchHistory.Keys.ToArray());
		}

		/// <summary>
		/// ThreadListSearchDialogクラスのインスタンスを初期化
		/// </summary>
		public ThreadListSearchDialog(AbstractSearcher listSearcher,
			SearchSettings.ListSearchSettings sett) : this(listSearcher)
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
			this.buttonSearch = new System.Windows.Forms.Button();
			this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
			this.checkBoxRegex = new System.Windows.Forms.CheckBox();
			this.checkBoxIncSearch = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonBack = new System.Windows.Forms.Button();
			this.comboBoxKey = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// buttonSearch
			// 
			this.buttonSearch.AutoSize = true;
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonSearch.Location = new System.Drawing.Point(302, 4);
			this.buttonSearch.Margin = new System.Windows.Forms.Padding(2);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(80, 21);
			this.buttonSearch.TabIndex = 2;
			this.buttonSearch.Text = "検索";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// checkBoxMatchCase
			// 
			this.checkBoxMatchCase.AutoSize = true;
			this.checkBoxMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxMatchCase.Location = new System.Drawing.Point(21, 44);
			this.checkBoxMatchCase.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxMatchCase.Name = "checkBoxMatchCase";
			this.checkBoxMatchCase.Size = new System.Drawing.Size(151, 17);
			this.checkBoxMatchCase.TabIndex = 6;
			this.checkBoxMatchCase.Text = "大文字小文字を区別(&C)";
			// 
			// checkBoxRegex
			// 
			this.checkBoxRegex.AutoSize = true;
			this.checkBoxRegex.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxRegex.Location = new System.Drawing.Point(21, 60);
			this.checkBoxRegex.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxRegex.Name = "checkBoxRegex";
			this.checkBoxRegex.Size = new System.Drawing.Size(94, 17);
			this.checkBoxRegex.TabIndex = 7;
			this.checkBoxRegex.Text = "正規表現(&G)";
			// 
			// checkBoxIncSearch
			// 
			this.checkBoxIncSearch.AutoSize = true;
			this.checkBoxIncSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxIncSearch.Location = new System.Drawing.Point(21, 28);
			this.checkBoxIncSearch.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxIncSearch.Name = "checkBoxIncSearch";
			this.checkBoxIncSearch.Size = new System.Drawing.Size(143, 17);
			this.checkBoxIncSearch.TabIndex = 5;
			this.checkBoxIncSearch.Text = "インクリメンタルサーチ(&N)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(4, 7);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "検索キーワード(&D)";
			// 
			// buttonClose
			// 
			this.buttonClose.AutoSize = true;
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClose.Location = new System.Drawing.Point(302, 60);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(80, 21);
			this.buttonClose.TabIndex = 4;
			this.buttonClose.Text = "閉じる";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonBack
			// 
			this.buttonBack.AutoSize = true;
			this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonBack.Location = new System.Drawing.Point(302, 28);
			this.buttonBack.Margin = new System.Windows.Forms.Padding(2);
			this.buttonBack.Name = "buttonBack";
			this.buttonBack.Size = new System.Drawing.Size(80, 21);
			this.buttonBack.TabIndex = 3;
			this.buttonBack.Text = "戻す";
			this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
			// 
			// comboBoxKey
			// 
			this.comboBoxKey.FormattingEnabled = true;
			this.comboBoxKey.Location = new System.Drawing.Point(102, 4);
			this.comboBoxKey.Name = "comboBoxKey";
			this.comboBoxKey.Size = new System.Drawing.Size(181, 20);
			this.comboBoxKey.TabIndex = 1;
			this.comboBoxKey.SelectedIndexChanged += new System.EventHandler(this.textBoxKeyword_TextChanged);
			this.comboBoxKey.TextChanged += new System.EventHandler(this.textBoxKeyword_TextChanged);
			// 
			// ThreadListSearchDialog
			// 
			this.AcceptButton = this.buttonSearch;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonClose;
			this.ClientSize = new System.Drawing.Size(391, 94);
			this.Controls.Add(this.comboBoxKey);
			this.Controls.Add(this.buttonBack);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkBoxIncSearch);
			this.Controls.Add(this.checkBoxRegex);
			this.Controls.Add(this.checkBoxMatchCase);
			this.Controls.Add(this.buttonSearch);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThreadListSearchDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "スレッド一覧検索";
			this.Closed += new System.EventHandler(this.ThreadListSearchDialog_Closed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 設定を適用
		/// </summary>
		/// <param name="sett"></param>
		private void SetSettings(SearchSettings.ListSearchSettings sett)
		{
			Keyword = sett.Keyword;
			Options = sett.Options;
			checkBoxIncSearch.Checked = sett.IncrementalSearch;
			settings = sett;
		}

		/// <summary>
		/// 設定を保存
		/// </summary>
		private void SaveSettings()
		{
			settings.Options = Options;
			settings.IncrementalSearch = checkBoxIncSearch.Checked;
			settings.Keyword = Keyword;
		}

		/// <summary>
		/// インクリメンタルサーチを開始
		/// </summary>
		private void IncrementSearch()
		{
			try {
				Cursor = Cursors.WaitCursor;
				listSearcher.Options = Options;
				listSearcher.Search(Keyword);
			}
			finally { Cursor = Cursors.Default; }
		}

		private void textBoxKeyword_TextChanged(object sender, System.EventArgs e)
		{
			if (checkBoxIncSearch.Checked)
			{
				refreshTimer.Stop();
				refreshTimer.Start();
			}
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			if (Keyword == String.Empty)
			{
				listSearcher.Reset();
			}
			else {
				listSearcher.Options = Options;

				Twinie.Settings.Search.SearchHistory.Add(Keyword);

				bool found = listSearcher.Search(Keyword);

				if (found) {}

			}
		}

		private void OnRefreshTimer(object sender, EventArgs e)
		{
			refreshTimer.Stop();
			Invoke(new MethodInvoker(IncrementSearch));
		}

		private void ThreadListSearchDialog_Closed(object sender, System.EventArgs e)
		{
			SaveSettings();

			if (Owner != null)
				Owner.Activate();
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void buttonBack_Click(object sender, System.EventArgs e)
		{
			listSearcher.Reset();
		}
	}
}
