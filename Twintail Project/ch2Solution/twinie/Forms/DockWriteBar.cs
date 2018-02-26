// DockWriteBar.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Data;
	using System.Windows.Forms;
	using Twin.Aa;
	using Twin.Tools;
	using Twin.Bbs;

	/// <summary>
	/// ドッキング書き込みバー
	/// </summary>
	public class DockWriteBar : System.Windows.Forms.UserControl
	{
		private Dictionary<string,PostState> resItemTable;			// スレッドごとのレスデータ
		private ThreadHeader activeThread;	// 現在アクティブなスレッドの情報
		private bool noTextChangedEvent;

		private readonly Samba24 samba = PostDialog.samba;
		private CookieManager cookie;
		private KotehanManager koteman;

		private Cache cache;
		private ThreadHeader postThreadHeader;

		// 投稿関連
		private bool posting;				// 投稿中かどうかを表す

		#region Designer Fields
		private System.Windows.Forms.TextBox textBoxBody;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolBarButton toolBarButtonAA;
		private System.Windows.Forms.ToolBarButton toolBarButtonClear;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button buttonWrite;
		private System.Windows.Forms.CheckBox checkBoxSage;
		private System.Windows.Forms.CheckBox checkBoxBe2ch;
		private ComboBox comboBoxName;
		private ComboBox comboBoxEmail;
		private Label labelThreadTitle;
		private System.ComponentModel.IContainer components;
		#endregion

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
				comboBoxEmail.Text = value;
			}
			get
			{
				return comboBoxEmail.Text;
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
		/// コテハンが空かどうかを判断
		/// </summary>
		public bool KoteIsEmpty
		{
			get
			{
				// 名前かEmailのどちらかに値が設定されていればtrue
				if (From == String.Empty &&
					(Email == String.Empty || Sage))
					return true;
				return false;
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

		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				comboBoxEmail.Font = comboBoxName.Font = value;
			}
		}
	

		/// <summary>
		/// 書き込み後に発生
		/// </summary>
		public event PostedEventHandler Posted;

		#region InnerClass
		private class PostState
		{
			public PostBase PostBase;
			public PostRes Res;
			public ThreadHeader HeaderInfo;
			public bool Be;

			public PostState(ThreadHeader header, bool be)
			{
				HeaderInfo = header;
				Res = new PostRes();
				PostBase = null;
				Be = be;
			}
		}
		#endregion

		/// <summary>
		/// DockWriteBarクラスのインスタンスを初期化
		/// </summary>
		public DockWriteBar(Cache cache, KotehanManager koteman)
		{
			// この呼び出しは、Windows.Forms フォーム デザイナで必要です。
			InitializeComponent();

			// TODO: InitForm を呼び出しの後に初期化処理を追加します。
			PostDialog.AaContext.Selected += new AaItemEventHandler(OnAaSelected);
			this.ContextMenu = PostDialog.AaContext.Context;

			textBoxBody.KeyDown += new KeyEventHandler(textBoxBody_KeyDown);

			this.cache = cache;
			this.koteman = koteman;
			this.cookie = new CookieManager(cache);

			resItemTable = new Dictionary<string, PostState>();
			activeThread = null;

			noTextChangedEvent = false;
			posting = false;

			comboBoxName.Items.AddRange(Twinie.Settings.Post.NameHistory.Keys.ToArray());
			comboBoxEmail.Items.AddRange(Twinie.Settings.Post.MailHistory.Keys.ToArray());

			// 書き込み画面のフォント設定
			Font font = new Font(Twinie.Settings.Design.Post.FontName,
				Twinie.Settings.Design.Post.FontSize);

			textBoxBody.Font =
				comboBoxName.Font = comboBoxEmail.Font = font;
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
				PostDialog.AaContext.Selected -= new AaItemEventHandler(OnAaSelected);
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockWriteBar));
			this.textBoxBody = new System.Windows.Forms.TextBox();
			this.buttonWrite = new System.Windows.Forms.Button();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonAA = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonClear = new System.Windows.Forms.ToolBarButton();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBoxSage = new System.Windows.Forms.CheckBox();
			this.checkBoxBe2ch = new System.Windows.Forms.CheckBox();
			this.comboBoxName = new System.Windows.Forms.ComboBox();
			this.comboBoxEmail = new System.Windows.Forms.ComboBox();
			this.labelThreadTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBoxBody
			// 
			this.textBoxBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxBody.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxBody.Enabled = false;
			this.textBoxBody.ImeMode = System.Windows.Forms.ImeMode.On;
			this.textBoxBody.Location = new System.Drawing.Point(3, 27);
			this.textBoxBody.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxBody.Multiline = true;
			this.textBoxBody.Name = "textBoxBody";
			this.textBoxBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxBody.Size = new System.Drawing.Size(656, 75);
			this.textBoxBody.TabIndex = 0;
			this.textBoxBody.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// buttonWrite
			// 
			this.buttonWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonWrite.AutoSize = true;
			this.buttonWrite.Enabled = false;
			this.buttonWrite.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonWrite.Location = new System.Drawing.Point(579, 2);
			this.buttonWrite.Margin = new System.Windows.Forms.Padding(2);
			this.buttonWrite.Name = "buttonWrite";
			this.buttonWrite.Size = new System.Drawing.Size(79, 21);
			this.buttonWrite.TabIndex = 1;
			this.buttonWrite.Text = "書き込む(&W)";
			this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 10000;
			this.toolTip.InitialDelay = 500;
			this.toolTip.ReshowDelay = 100;
			this.toolTip.ShowAlways = true;
			// 
			// toolBar
			// 
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.AutoSize = false;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonAA,
            this.toolBarButtonClear});
			this.toolBar.Divider = false;
			this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBar.DropDownArrows = true;
			this.toolBar.Enabled = false;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(408, 2);
			this.toolBar.Margin = new System.Windows.Forms.Padding(2);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(57, 24);
			this.toolBar.TabIndex = 8;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// toolBarButtonAA
			// 
			this.toolBarButtonAA.ImageIndex = 1;
			this.toolBarButtonAA.Name = "toolBarButtonAA";
			this.toolBarButtonAA.ToolTipText = "AA入力支援";
			// 
			// toolBarButtonClear
			// 
			this.toolBarButtonClear.ImageIndex = 2;
			this.toolBarButtonClear.Name = "toolBarButtonClear";
			this.toolBarButtonClear.ToolTipText = "本文をクリア";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "名前(&N)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(159, 7);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "&Email";
			// 
			// checkBoxSage
			// 
			this.checkBoxSage.AutoSize = true;
			this.checkBoxSage.Enabled = false;
			this.checkBoxSage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxSage.Location = new System.Drawing.Point(302, 6);
			this.checkBoxSage.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxSage.Name = "checkBoxSage";
			this.checkBoxSage.Size = new System.Drawing.Size(54, 17);
			this.checkBoxSage.TabIndex = 6;
			this.checkBoxSage.Text = "&sage";
			this.checkBoxSage.CheckedChanged += new System.EventHandler(this.checkBoxSage_CheckedChanged);
			// 
			// checkBoxBe2ch
			// 
			this.checkBoxBe2ch.AutoSize = true;
			this.checkBoxBe2ch.Enabled = false;
			this.checkBoxBe2ch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxBe2ch.Location = new System.Drawing.Point(360, 6);
			this.checkBoxBe2ch.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxBe2ch.Name = "checkBoxBe2ch";
			this.checkBoxBe2ch.Size = new System.Drawing.Size(44, 17);
			this.checkBoxBe2ch.TabIndex = 7;
			this.checkBoxBe2ch.Text = "Be";
			this.checkBoxBe2ch.CheckedChanged += new System.EventHandler(this.checkBoxBe2ch_CheckedChanged);
			// 
			// comboBoxName
			// 
			this.comboBoxName.Enabled = false;
			this.comboBoxName.FormattingEnabled = true;
			this.comboBoxName.Location = new System.Drawing.Point(51, 4);
			this.comboBoxName.Name = "comboBoxName";
			this.comboBoxName.Size = new System.Drawing.Size(100, 20);
			this.comboBoxName.TabIndex = 3;
			this.comboBoxName.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// comboBoxEmail
			// 
			this.comboBoxEmail.Enabled = false;
			this.comboBoxEmail.FormattingEnabled = true;
			this.comboBoxEmail.Location = new System.Drawing.Point(197, 4);
			this.comboBoxEmail.Name = "comboBoxEmail";
			this.comboBoxEmail.Size = new System.Drawing.Size(100, 20);
			this.comboBoxEmail.TabIndex = 5;
			this.comboBoxEmail.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// labelThreadTitle
			// 
			this.labelThreadTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelThreadTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelThreadTitle.Location = new System.Drawing.Point(470, 7);
			this.labelThreadTitle.Name = "labelThreadTitle";
			this.labelThreadTitle.Size = new System.Drawing.Size(104, 17);
			this.labelThreadTitle.TabIndex = 9;
			this.labelThreadTitle.Text = "スレッド名";
			this.labelThreadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DockWriteBar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.labelThreadTitle);
			this.Controls.Add(this.comboBoxEmail);
			this.Controls.Add(this.comboBoxName);
			this.Controls.Add(this.checkBoxBe2ch);
			this.Controls.Add(this.checkBoxSage);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.toolBar);
			this.Controls.Add(this.buttonWrite);
			this.Controls.Add(this.textBoxBody);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "DockWriteBar";
			this.Size = new System.Drawing.Size(663, 101);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Private
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

		private void OnAaSelected(object sender, AaItemEventArgs e)
		{
			if (sender.Equals(this))
			{
				string data = e.Item.Data;
				string text = textBoxBody.Text;
				int selection = textBoxBody.SelectionStart;

				textBoxBody.Text = text.Insert(selection, data);
				textBoxBody.SelectionStart = selection + data.Length;
			}
		}

		private void checkBoxBe2ch_CheckedChanged(object sender, System.EventArgs e)
		{
			if (activeThread != null)
			{
				PostState state = resItemTable[activeThread.Url];
				if (state != null)
					state.Be = checkBoxBe2ch.Checked;
			}
		}

		private void checkBoxSage_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBoxSage.Checked)
			{
				comboBoxEmail.Text = "sage";
				comboBoxEmail.Enabled = false;
			}
			else
			{
				comboBoxEmail.Text = String.Empty;
				comboBoxEmail.Enabled = true;
			}
		}

		private void textBox_TextChanged(object sender, System.EventArgs e)
		{
			if (!noTextChangedEvent && activeThread != null)
			{
				PostState state = resItemTable[activeThread.Url];
				state.Res = new PostRes(From, Email, Body);
			}
			if (textBoxBody.Equals(sender))
				toolTip.SetToolTip(buttonWrite, textBoxBody.Text);
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonAA)
			{
				InputAa();
			}
			else if (e.Button == toolBarButtonClear)
			{
				textBoxBody.Clear();
			}
		}

		private void buttonWrite_Click(object sender, System.EventArgs e)
		{
			Write();
		}

		private void OnPosting(IAsyncResult ar)
		{
			MethodInvoker m = delegate
			{
				PostState state = (PostState)ar.AsyncState;
				PostBase post = state.PostBase;
				ThreadHeader header = state.HeaderInfo;

				post.EndPost(ar);

				posting = false;

				// 投稿成功時に閉じる
				if (post.Response == PostResponse.Success)
				{
					// 最終書込日を設定
					header.LastWritten = DateTime.Now;
					ThreadIndexer.SaveLastWritten(cache, header);

					textBoxBody.Text = String.Empty;
					textBoxBody.Focus();
				}

				buttonWrite.Enabled = true;
			};
			Invoke(m);
		}

		private void OnPosted(object sender, PostEventArgs e)
		{
			MethodInvoker m = delegate
			{
				cookie.SetCookie(postThreadHeader.BoardInfo);

				switch (e.Response)
				{
				case PostResponse.Success:
					if (Posted != null)
					{
						Posted(this, new PostedEventArgs(postThreadHeader, new PostRes(From, Email, Body)));
					}
					break;

				case PostResponse.Cookie:
					DialogResult result = DialogResult.Yes;

					if (Twinie.Settings.Post.ShowCookieDialog)
					{
						result = MessageBox.Show(this, e.Text, e.Title,
							MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					}

					if (result == DialogResult.Yes)
						e.Retry = true;
					break;

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

		/// <summary>
		/// エラーチェック
		/// </summary>
		/// <returns></returns>
		private bool IsErrorChecking()
		{
			if (textBoxBody.Text == String.Empty)
			{
				MessageBox.Show("本文を入力してください", "投稿エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return true;
			}
			return false;
		}

		/// <summary>
		/// 自主規制を行う
		/// </summary>
		/// <returns></returns>
		private bool Samba24Checking()
		{
			if (Twinie.Settings.Post.Samba24Check)
			{
				// チェック対象のサーバー名を取得 (※ホストアドレスではない)
				string server = activeThread.BoardInfo.ServerName;
				int result;

				if (!samba.IsElapsed(server, out result))
				{
					MessageBox.Show(
						String.Format("あと{0}秒待ってください。。", result),
						"samba24 自主規制", MessageBoxButtons.OK, MessageBoxIcon.Information);

					return true;
				}

				samba.CountStart(server);
			}
			return false;
		}

		/// <summary>
		/// 書き込みバーの有効状態を設定
		/// </summary>
		/// <param name="enabled"></param>
		private void EnableDockWriteBar(bool enabled)
		{
			comboBoxName.Enabled = comboBoxEmail.Enabled = textBoxBody.Enabled =
				toolBar.Enabled = buttonWrite.Enabled = checkBoxSage.Enabled = enabled;

			checkBoxSage.Checked = 
				comboBoxEmail.Text.Equals("sage") ? true : false;

			comboBoxEmail.Enabled = !checkBoxSage.Checked;

			checkBoxBe2ch.Enabled = !TwinDll.Be2chCookie.IsEmpty;
		}

		/// <summary>
		/// メッセージを投稿
		/// </summary>
		private void Write()
		{
			if (activeThread == null)
				throw new ApplicationException("アクティブなスレッドが設定されていません");

			if (Samba24Checking() || IsErrorChecking())
				return;

			buttonWrite.Enabled = false;
			posting = true;

			// クッキーを取得
			cookie.GetCookie(activeThread.BoardInfo);

			// コテハンを作成
			Kotehan newkote = new Kotehan(From, Email, checkBoxBe2ch.Checked);

			UpdateInputHistory();

			// スレッドに書き込むごとにコテハンを保存する場合
			if (Twinie.Settings.Post.ThreadKotehan)
				koteman.Set(activeThread, newkote);

			PostState state = resItemTable[activeThread.Url];
			postThreadHeader = activeThread;

			if (state.PostBase == null)
			{
				BbsType bbs = activeThread.BoardInfo.Bbs;
				PostBase post = TypeCreator.CreatePost(bbs);

				post.Posted += new PostEventHandler(OnPosted);
				post.Error += new PostErrorEventHandler(OnPostError);
				post.Proxy = Twinie.Settings.Net.SendProxy;
				post.UserAgent = Settings.UserAgent;
				state.PostBase = post;
			}

			if (state.PostBase is X2chPost)
			{
				((X2chPost)state.PostBase).SendBeID = state.Be;
			}

			state.PostBase.BeginPost(activeThread,
				state.Res, new AsyncCallback(OnPosting), state);
		}

		// コンボボックスに入力履歴を追加
		private void UpdateInputHistory()
		{
			Twinie.Settings.Post.NameHistory.Add(From);
			Twinie.Settings.Post.MailHistory.Add(Email);

			UpdateComboBoxItems(comboBoxName, From);
			UpdateComboBoxItems(comboBoxEmail, Email);
		}

		private void UpdateComboBoxItems(ComboBox cb, string addText)
		{
			if (addText == String.Empty)
				return;

			for (int i = 0; i < cb.Items.Count; i++)
			{
				if (String.Compare(cb.Items[i].ToString(), addText) == 0)
				{
					return;
				}
			}
			cb.Items.Add(addText);
		}

		#endregion

		/// <summary>
		/// 本文にテキストを追加
		/// </summary>
		/// <param name="text"></param>
		public void AppendText(string text)
		{
			textBoxBody.Text += text;
			textBoxBody.ScrollToCaret();
		}

		/// <summary>
		/// AA入力支援を表示
		/// </summary>
		public void InputAa()
		{
			Point loc = toolBarButtonAA.Rectangle.Location;
			loc = toolBar.PointToScreen(loc);
			loc = PointToClient(loc);

			PostDialog.AaContext.Show(this, loc, new Point(0, 0));
		}

		/// <summary>
		/// 書き込み先を指定したスレッドに設定
		/// </summary>
		/// <param name="header"></param>
		public void Select(ThreadHeader header)
		{
			if (header == null)
				throw new ArgumentNullException("header");

			activeThread = header;
			noTextChangedEvent = true;
			labelThreadTitle.Text = String.Format("[{0}] {1}", header.BoardInfo.Name, header.Subject);

			if (resItemTable.ContainsKey(header.Url))
			{
				// 指定したスレッドのレスを復元
				PostState state = resItemTable[header.Url];
				textBoxBody.Text = state.Res.Body;
				checkBoxSage.Checked = state.Res.Email.Equals("sage");
				checkBoxBe2ch.Checked = state.Be;
				From = state.Res.From;
				Email = state.Res.Email;
			}
			else
			{
				Kotehan kote = koteman.Get(header);
				PostState state = new PostState(header, kote.Be);
				state.Res = new PostRes(kote.Name, kote.Email, String.Empty);
				resItemTable.Add(header.Url, state);

				Empty();

				From = kote.Name;
				Email = kote.Email;
				checkBoxBe2ch.Checked = kote.Be;

			}

			noTextChangedEvent = false;
			checkBoxSage.Enabled = true;
			buttonWrite.Enabled = !posting;

			EnableDockWriteBar(true);
		}

		/// <summary>
		/// 指定したスレッドのレスを削除
		/// </summary>
		/// <param name="header"></param>
		public void Remove(ThreadHeader header)
		{
			// アクティブなスレッドを削除しようとしたら入力欄を空にしてから
			if (header.Equals(activeThread))
				Empty();

			resItemTable.Remove(header.Url);

			if (resItemTable.Count == 0)
				EnableDockWriteBar(false);
		}

		/// <summary>
		/// 入力欄を空に設定
		/// </summary>
		public void Empty()
		{
			checkBoxSage.Checked = false;
			From = Email = Body = String.Empty;
		}

		/// <summary>
		/// すべてのスレッドのレスを削除
		/// </summary>
		public void Clear()
		{
			noTextChangedEvent = true;
			{
				activeThread = null;
				resItemTable.Clear();
				Empty();
				EnableDockWriteBar(false);
			}
			noTextChangedEvent = false;
		}
	}
}
