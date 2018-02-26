// LocalRuleViewer.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using System.Net;
	using System.IO;

	/// <summary>
	/// ローカルルールを表示するためのビューア
	/// </summary>
	public class LocalRuleViewer : System.Windows.Forms.Form
	{
		private Thread thread;
		private BoardInfo boardInfo;
		private string tempFileName;

		private AxSHDocVw.AxWebBrowser axWebBrowser;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>
		/// LocalRuleViewerクラスのインスタンスを初期化
		/// </summary>
		public LocalRuleViewer()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalRuleViewer));
			this.axWebBrowser = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).BeginInit();
			this.SuspendLayout();
			// 
			// axWebBrowser
			// 
			this.axWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser.Enabled = true;
			this.axWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser.Margin = new System.Windows.Forms.Padding(2);
			this.axWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser.OcxState")));
			this.axWebBrowser.Size = new System.Drawing.Size(534, 362);
			this.axWebBrowser.TabIndex = 0;
			// 
			// LocalRuleViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 362);
			this.Controls.Add(this.axWebBrowser);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "LocalRuleViewer";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ローカルルール";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.LocalRuleViewer_Closing);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// ローカルルールの取得処理
		/// </summary>
		private void Process()
		{

			string body = String.Empty;

			try
			{
				// ローカルルールをダウンロード
				using (WebClient client = new WebClient())
				{
					client.Headers.Add("User-Agent", Settings.UserAgent);
					byte[] buffer = client.DownloadData(boardInfo.Url + "head.txt");
					body = TwinDll.DefaultEncoding.GetString(buffer);
				}
			}
			catch (Exception ex)
			{
				body = ex.Message;
			}

			// 一時ファイルを作成
			tempFileName = Path.ChangeExtension(Path.GetTempFileName(), ".html");

			using (StreamWriter w = new StreamWriter(tempFileName, false, TwinDll.DefaultEncoding))
			{
				w.Write("<html><head><base href=\"{0}\" /></head><body>{1}</body></html>",
					boardInfo.Url, body);
			}

			// 開く
			Invoke(new MethodInvoker(OpenLocalRule));

		}

		/// <summary>
		/// Webブラウザで開く
		/// </summary>
		private void OpenLocalRule()
		{
			object o = null;
			axWebBrowser.Navigate(tempFileName, ref o, ref o, ref o, ref o);
		}

		private void LocalRuleViewer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// スレッドを中止
			if (thread != null && thread.IsAlive)
				thread.Abort();

			// 作成した一時ファイルを削除
			if (File.Exists(tempFileName))
				File.Delete(tempFileName);
		}

		/// <summary>
		/// コントロールを表示すると同時にローカルルールも表示
		/// </summary>
		/// <param name="board"></param>
		public void Show(BoardInfo board)
		{
			Show();
			Open(board);
		}

		/// <summary>
		/// 指定した板のローカルルールを表示
		/// </summary>
		/// <param name="board"></param>
		public void Open(BoardInfo board)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			if (thread != null && thread.IsAlive)
				thread.Abort();

			// タイトルを設定
			this.Text = String.Format("ローカルルール [{0}板]", board.Name);
			this.boardInfo = board;

			if (File.Exists(tempFileName))
				File.Delete(tempFileName);

			thread = new Thread(new ThreadStart(Process));
			thread.Name = "LOCAL_RURLE_VIEW";
			thread.IsBackground = true;
			thread.Start();
		}
	}
}
