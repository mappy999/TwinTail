// CacheClearDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using Twin.Tools;

	/// <summary>
	/// お気に入り以外のログを削除するダイアログ
	/// </summary>
	public class CacheClearDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonOK;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Timer timer;
		private Cache cache;
		private IBoardTable table;
		private BookmarkRoot without;
		private Thread thread;

		private StringCollection aaCollection;
		private int index;

		/// <summary>
		/// CacheClearDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="cache">削除するキャッシュの情報</param>
		/// <param name="without">削除対象から外すお気に入り</param>
		public CacheClearDialog(Cache cache, IBoardTable table, BookmarkRoot without)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (without == null) {
				throw new ArgumentNullException("without");
			}
			if (table == null) {
				throw new ArgumentNullException("table");
			}			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.cache = cache;
			this.without = without;
			this.table = table;
			
			thread = new Thread(new ThreadStart(Removing));
			thread.Name = "CACHE_CLEAR_DLG";
			thread.IsBackground = true;

			timer = new System.Windows.Forms.Timer();
			timer.Interval = 100;
			timer.Tick += new EventHandler(OnTimer);

			aaCollection = new StringCollection();
			aaCollection.Add("ヾ(　 　)ﾉｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("  ヾ(　ﾟд)ﾉｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("      ヾ(ﾟдﾟ)ﾉｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("          ヾ(дﾟ　)ﾉｻｸｼﾞｮﾁｭ-゛");
			aaCollection.Add("              ヾ(　　)ﾉ゛ｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("                ヾ(　ﾟд)ﾉｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("              ヾ(　　)ﾉ゛ｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("          ヾ(дﾟ　)ﾉ゛ｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("      ヾ(ﾟдﾟ)ﾉｻｸｼﾞｮﾁｭ-");
			aaCollection.Add("  ヾ(　ﾟд)ﾉｻｸｼﾞｮﾁｭ-");
			index = 0;
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(13, 8);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(169, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "お気に入り以外のログを削除します";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonOK
			// 
			this.buttonOK.AutoSize = true;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(68, 28);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(71, 21);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "開始";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// CacheClearDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(219, 60);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "CacheClearDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "お気に入り以外のログを削除";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.CacheClearDialog_Closing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void Removing()
		{
			try {
				CacheRemoveUtility remover = new CacheRemoveUtility(cache);
				remover.RemoveWithoutBookmarks(table, without);
			}
			finally {
				thread = null;
				timer.Stop();

				Invoke(new MethodInvoker(Close));
			}
		}

		private void OnTimer(object sender, EventArgs e)
		{
			label1.Text = aaCollection[index++];
			if (index == aaCollection.Count) index = 0;
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			if (thread.IsAlive)
			{
				buttonOK.Text = "閉じる";
				timer.Stop();
				thread.Abort();
				thread = null;
			}
			else if (thread != null)
			{
				buttonOK.Text = "中止";
				timer.Start();
				thread.Start();
			}
			else {
				Close();
			}
		}

		private void CacheClearDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (thread != null && thread.IsAlive)
			{
				if (MessageBox.Show("ログを削除中です。中止しますか？", "中止確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
				{
					e.Cancel = true;
					return;
				}
				timer.Stop();
				thread.Abort();
			}
		}
	}
}
