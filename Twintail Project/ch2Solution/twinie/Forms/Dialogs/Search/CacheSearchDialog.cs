// CacheSearchDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using Twin.Text;
	using CSharpSamples;

	using ITwinListControl = ITwinTabController<BoardInfo, ThreadListControl>;

	/// <summary>
	/// キャッシュを検索するダイアログ
	/// </summary>
	public class CacheSearchDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.Button buttonCancel;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonBody;
		private System.Windows.Forms.RadioButton radioButtonTitle;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckedListBox checkedListBoxBoards;
		private System.Windows.Forms.ContextMenu contextMenuBoard;
		private System.Windows.Forms.MenuItem menuItemCheckAll;
		private System.Windows.Forms.MenuItem menuItemUncheckAll;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		#endregion

		private SearchSettings.CacheSearchSettings settings;	// 検索オプション
		private BoardInfoCollection boards;						// 検索対象の板コレクション
		private ITwinListControl listControl;
		private IBoardTable table;
		private Cache cache;

		private int lastSearchIndex = 0;

		// 検索時に使う変数
		private ThreadListControl _listView;
		private ComboBox comboBoxKey;
		private ToolStrip toolStrip1;
		private ToolStripLabel toolStripLabel1;
		private ToolStripTextBox toolStripTextBoxKey;
		private ToolStripSplitButton toolStripSplitButtonSearchNext;
		private ToolStripMenuItem toolStripMenuItemCheckAll;
		private ToolStripMenuItem toolStripMenuItemUncheckAll;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton toolStripButtonCheckActive;
		private Thread thread;

		/// <summary>
		/// CacheSearchDialogクラスのインスタンスを初期化
		/// </summary>
		public CacheSearchDialog(ITwinListControl listControl, Cache cache, IBoardTable table) 
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.cache = cache;
			this.table = table;
			this.listControl = listControl;
			this.boards = new BoardInfoCollection();

			SetBoardTable(cache, table);
			LoadSettings(Twinie.Settings.Search.CacheSearch);

			comboBoxKey.Items.AddRange(Twinie.Settings.Search.SearchHistory.Keys.ToArray());
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CacheSearchDialog));
			this.buttonSearch = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButtonBody = new System.Windows.Forms.RadioButton();
			this.radioButtonTitle = new System.Windows.Forms.RadioButton();
			this.checkedListBoxBoards = new System.Windows.Forms.CheckedListBox();
			this.contextMenuBoard = new System.Windows.Forms.ContextMenu();
			this.menuItemCheckAll = new System.Windows.Forms.MenuItem();
			this.menuItemUncheckAll = new System.Windows.Forms.MenuItem();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.comboBoxKey = new System.Windows.Forms.ComboBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripTextBoxKey = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSplitButtonSearchNext = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStripMenuItemCheckAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemUncheckAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonCheckActive = new System.Windows.Forms.ToolStripButton();
			this.groupBox1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonSearch
			// 
			this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSearch.AutoSize = true;
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonSearch.Location = new System.Drawing.Point(276, 232);
			this.buttonSearch.Margin = new System.Windows.Forms.Padding(2);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(84, 21);
			this.buttonSearch.TabIndex = 2;
			this.buttonSearch.Text = "検索";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(365, 232);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(84, 21);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(159, 41);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "検索キーワード(&D)";
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.radioButtonBody);
			this.groupBox1.Controls.Add(this.radioButtonTitle);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(159, 65);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(290, 49);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "検索対象";
			// 
			// radioButtonBody
			// 
			this.radioButtonBody.AutoSize = true;
			this.radioButtonBody.Checked = true;
			this.radioButtonBody.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonBody.Location = new System.Drawing.Point(147, 16);
			this.radioButtonBody.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonBody.Name = "radioButtonBody";
			this.radioButtonBody.Size = new System.Drawing.Size(103, 17);
			this.radioButtonBody.TabIndex = 1;
			this.radioButtonBody.TabStop = true;
			this.radioButtonBody.Text = "スレッド全体(&B)";
			this.toolTip.SetToolTip(this.radioButtonBody, "スレッドのDAT全体を検索します");
			// 
			// radioButtonTitle
			// 
			this.radioButtonTitle.AutoSize = true;
			this.radioButtonTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonTitle.Location = new System.Drawing.Point(16, 16);
			this.radioButtonTitle.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonTitle.Name = "radioButtonTitle";
			this.radioButtonTitle.Size = new System.Drawing.Size(113, 17);
			this.radioButtonTitle.TabIndex = 0;
			this.radioButtonTitle.Text = "スレッドタイトル(&T)";
			this.toolTip.SetToolTip(this.radioButtonTitle, "スレッド名のみを検索します");
			// 
			// checkedListBoxBoards
			// 
			this.checkedListBoxBoards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.checkedListBoxBoards.ContextMenu = this.contextMenuBoard;
			this.checkedListBoxBoards.IntegralHeight = false;
			this.checkedListBoxBoards.Location = new System.Drawing.Point(11, 27);
			this.checkedListBoxBoards.Margin = new System.Windows.Forms.Padding(2);
			this.checkedListBoxBoards.Name = "checkedListBoxBoards";
			this.checkedListBoxBoards.Size = new System.Drawing.Size(135, 226);
			this.checkedListBoxBoards.TabIndex = 7;
			this.toolTip.SetToolTip(this.checkedListBoxBoards, "検索対象の板をチェックしてください");
			this.checkedListBoxBoards.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxBoards_ItemCheck);
			// 
			// contextMenuBoard
			// 
			this.contextMenuBoard.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCheckAll,
            this.menuItemUncheckAll});
			// 
			// menuItemCheckAll
			// 
			this.menuItemCheckAll.Index = 0;
			this.menuItemCheckAll.Text = "すべてチェックする(&C)";
			this.menuItemCheckAll.Click += new System.EventHandler(this.menuItemCheckAll_Click);
			// 
			// menuItemUncheckAll
			// 
			this.menuItemUncheckAll.Index = 1;
			this.menuItemUncheckAll.Text = "すべてチェックをはずす(&U)";
			this.menuItemUncheckAll.Click += new System.EventHandler(this.menuItemUncheckAll_Click);
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.InitialDelay = 100;
			this.toolTip.ReshowDelay = 100;
			// 
			// comboBoxKey
			// 
			this.comboBoxKey.FormattingEnabled = true;
			this.comboBoxKey.Location = new System.Drawing.Point(257, 38);
			this.comboBoxKey.Name = "comboBoxKey";
			this.comboBoxKey.Size = new System.Drawing.Size(192, 20);
			this.comboBoxKey.TabIndex = 1;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBoxKey,
            this.toolStripSplitButtonSearchNext,
            this.toolStripSeparator1,
            this.toolStripButtonCheckActive});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(455, 25);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(86, 22);
			this.toolStripLabel1.Text = "対象の板を検索";
			// 
			// toolStripTextBoxKey
			// 
			this.toolStripTextBoxKey.AcceptsReturn = true;
			this.toolStripTextBoxKey.Name = "toolStripTextBoxKey";
			this.toolStripTextBoxKey.Size = new System.Drawing.Size(100, 25);
			this.toolStripTextBoxKey.TextChanged += new System.EventHandler(this.toolStripTextBoxKey_TextChanged);
			// 
			// toolStripSplitButtonSearchNext
			// 
			this.toolStripSplitButtonSearchNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripSplitButtonSearchNext.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCheckAll,
            this.toolStripMenuItemUncheckAll});
			this.toolStripSplitButtonSearchNext.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonSearchNext.Image")));
			this.toolStripSplitButtonSearchNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButtonSearchNext.Name = "toolStripSplitButtonSearchNext";
			this.toolStripSplitButtonSearchNext.Size = new System.Drawing.Size(68, 22);
			this.toolStripSplitButtonSearchNext.Text = "次を検索";
			this.toolStripSplitButtonSearchNext.ButtonClick += new System.EventHandler(this.toolStripSplitButtonSearchNext_ButtonClick);
			// 
			// toolStripMenuItemCheckAll
			// 
			this.toolStripMenuItemCheckAll.Name = "toolStripMenuItemCheckAll";
			this.toolStripMenuItemCheckAll.Size = new System.Drawing.Size(255, 22);
			this.toolStripMenuItemCheckAll.Text = "キーワードを含むすべての板をチェック";
			this.toolStripMenuItemCheckAll.Click += new System.EventHandler(this.toolStripMenuItemCheckAll_Click);
			// 
			// toolStripMenuItemUncheckAll
			// 
			this.toolStripMenuItemUncheckAll.Name = "toolStripMenuItemUncheckAll";
			this.toolStripMenuItemUncheckAll.Size = new System.Drawing.Size(255, 22);
			this.toolStripMenuItemUncheckAll.Text = "すべてのチェックを外す";
			this.toolStripMenuItemUncheckAll.Click += new System.EventHandler(this.toolStripMenuItemUncheckAll_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonCheckActive
			// 
			this.toolStripButtonCheckActive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonCheckActive.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCheckActive.Image")));
			this.toolStripButtonCheckActive.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonCheckActive.Name = "toolStripButtonCheckActive";
			this.toolStripButtonCheckActive.Size = new System.Drawing.Size(115, 22);
			this.toolStripButtonCheckActive.Text = "アクティブな板をチェック";
			this.toolStripButtonCheckActive.Click += new System.EventHandler(this.toolStripButtonCheckActive_Click);
			// 
			// CacheSearchDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(455, 264);
			this.Controls.Add(this.checkedListBoxBoards);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.comboBoxKey);
			this.Controls.Add(this.buttonSearch);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CacheSearchDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "キャッシュ検索";
			this.Closed += new System.EventHandler(this.CacheSearchDialog_Closed);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// チェックリストボックスに検索可能な板を追加
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="table"></param>
		private void SetBoardTable(Cache cache, IBoardTable table)
		{
			// 板を追加
			List<BoardInfo> list = new List<BoardInfo>();

			foreach (BoardInfo board in table.ToArray())
			{
				if (cache.Exists(board))
					list.Add(board);
			}
			checkedListBoxBoards.Items.AddRange(list.ToArray());
		}

		/// <summary>
		/// すべてのチェック状態を設定
		/// </summary>
		/// <param name="check"></param>
		private void SetChecked(bool check)
		{
			for (int i = 0; i < checkedListBoxBoards.Items.Count; i++)
				checkedListBoxBoards.SetItemChecked(i, check);
		}

		/// <summary>
		/// 設定情報を復元
		/// </summary>
		/// <param name="csc"></param>
		private void LoadSettings(SearchSettings.CacheSearchSettings csc)
		{
			settings = csc;

			// 検索キーワードを設定
			comboBoxKey.Text = csc.Keyword;

			// 検索対象を設定
			radioButtonTitle.Checked = 
				(settings.Target == CacheSearchTarget.Title) ? true : false;

			// 検索対象の板を設定
			foreach (BoardInfo board in csc.SelectedBoards)
			{
				int index = checkedListBoxBoards.Items.IndexOf(board);
				if (index >= 0) checkedListBoxBoards.SetItemChecked(index, true);
			}
		}

		/// <summary>
		/// 設定情報を保存
		/// </summary>
		private void SaveSettings()
		{
			// 検索キーワード
			settings.Keyword = comboBoxKey.Text;
			// 検索対象を取得
			settings.Target = (radioButtonTitle.Checked) ? 
				CacheSearchTarget.Title : CacheSearchTarget.Body;

			// 検索対象の板を取得
			settings.SelectedBoards.Clear();
			settings.SelectedBoards.AddRange(boards);

			Twinie.Settings.Search.SearchHistory.Add(comboBoxKey.Text);
		}

		/// <summary>
		/// 検索メソッド
		/// </summary>
		private void Searching()
		{
			try {
				// 検索開始
				Invoke(new MethodInvoker(OnSearchBegin));

				CacheSearcher searcher = null;
				CacheSearchResult result = null;

				switch (settings.Target)
				{
					// スレッドタイトルを検索
				case CacheSearchTarget.Title:
					searcher = new CacheSubjectSearcher(cache, boards, SearchOptions.None);
					break;

					// スレッドの本文を検索
				case CacheSearchTarget.Body:
					searcher = new CacheDatSearcher(cache, boards, SearchOptions.None);
					break;

				default:
					throw new ApplicationException();
				}

				// 検索開始
				searcher.Search(settings.Keyword);

				while ((result = searcher.Next()) != null)
				{
					// 一致アイテムがあれば書き出す
					Invoke(new ThreadHeaderEventHandler(OnSearched),
						new object[] {this, new ThreadHeaderEventArgs(result.HeaderInfo)});
				}
			}
			catch (ThreadAbortException) {}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}
			finally {
				// 検索完了
				Invoke(new MethodInvoker(OnSearchComplete));
			}
		}

		/// <summary>
		/// 検索開始時に呼ばれる
		/// </summary>
		private void OnSearchBegin()
		{
			buttonSearch.Enabled = false;
			buttonCancel.Text = "中止";
			buttonCancel.DialogResult = DialogResult.None;

			// 検索結果を表示するコントロールを作成
			_listView = listControl.Create(Twin2IeBrowser.dummySearchBoardInfo, true);
			_listView.SetItems(Twin2IeBrowser.dummySearchBoardInfo, new List<ThreadHeader>());
		}

		/// <summary>
		/// 検索完了時に呼ばれる
		/// </summary>
		private void OnSearched(object sender, ThreadHeaderEventArgs e)
		{
			foreach (ThreadHeader h in e.Items)
				h.Tag = new ThreadExtractInfo(settings.Keyword, SearchOptions.None);

			_listView.AddItems(e.Items);
		}

		/// <summary>
		/// 検索完了時に呼ばれる
		/// </summary>
		private void OnSearchComplete()
		{
			buttonSearch.Enabled = true;
			buttonCancel.Text = "閉じる";
			buttonCancel.DialogResult = DialogResult.Cancel;
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			if (comboBoxKey.Text == String.Empty)
			{
				MessageBox.Show("キーワードが入力されていません", "検索できません。。",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else if (boards.Count == 0)
			{
				MessageBox.Show("検索対象の板が選択されていません", "検索できません。。",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				SaveSettings();

				thread = new Thread(new ThreadStart(Searching));
				thread.IsBackground = true;
				thread.Start();
			}
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			if (thread != null && thread.IsAlive)
			{
				thread.Abort();
				thread = null;
			}
			else {
				Close();
			}
		}

		private void CacheSearchDialog_Closed(object sender, System.EventArgs e)
		{
			if (thread != null && thread.IsAlive)
				thread.Abort();

			if (Owner != null)
				Owner.Activate();
		}

		// すべてチェック
		private void menuItemCheckAll_Click(object sender, System.EventArgs e)
		{
			SetChecked(true);
		}

		// すべてチェック解除
		private void menuItemUncheckAll_Click(object sender, System.EventArgs e)
		{
			SetChecked(false);	
		}

		private void checkedListBoxBoards_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			BoardInfo board = 
				(BoardInfo)checkedListBoxBoards.Items[e.Index];

			if (e.NewValue == CheckState.Checked)
			{
				boards.Add(board);
			}
			else {
				boards.Remove(board);
			}
		}

		private int SearchBoard(string key, bool next)
		{
			if (!next)
				lastSearchIndex = 0;

			while (lastSearchIndex < checkedListBoxBoards.Items.Count)
			{
				int idx = lastSearchIndex++;
				BoardInfo bi = (BoardInfo)checkedListBoxBoards.Items[idx];

				if (bi.Name.Contains(key))
					return idx;
			}

			System.Media.SystemSounds.Question.Play();
			
			lastSearchIndex = 0;

			return -1;
		}

		private void toolStripMenuItemCheckAll_Click(object sender, EventArgs e)
		{
			string text = toolStripTextBoxKey.Text;
			int index;

			lastSearchIndex = 0;

			while ((index = SearchBoard(text, true)) != -1)
			{
				checkedListBoxBoards.SetItemChecked(index, true);
			}
		}

		private void toolStripMenuItemUncheckAll_Click(object sender, EventArgs e)
		{
			SetChecked(false);
		}

		private void toolStripButtonCheckActive_Click(object sender, EventArgs e)
		{
			SetChecked(false);

			if (listControl.IsSelected)
			{
				int index = checkedListBoxBoards.Items.IndexOf(listControl.HeaderInfo);

				if (index >= 0)
				{
					checkedListBoxBoards.SetItemChecked(index, true);
					checkedListBoxBoards.TopIndex = index;
				}
			}
		}

		private void toolStripSplitButtonSearchNext_ButtonClick(object sender, EventArgs e)
		{
			int index = SearchBoard(toolStripTextBoxKey.Text, true);
			if (index != -1)
			{
				checkedListBoxBoards.SetSelected(index, true);
			}
		}

		private void toolStripTextBoxKey_TextChanged(object sender, EventArgs e)
		{
			lastSearchIndex = 0;
		}

		private void toolStripTextBoxKey_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				toolStripSplitButtonSearchNext_ButtonClick(null, EventArgs.Empty);
			}
		}
	}

	/// <summary>
	/// キャッシュ検索の対象を表す
	/// </summary>
	public enum CacheSearchTarget
	{
		/// <summary>
		/// タイトルを検索
		/// </summary>
		Title = 0,
		/// <summary>
		/// 本文を検索
		/// </summary>
		Body = 1,
	}
}
