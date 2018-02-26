// ToolRegistDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using Twin.Tools;

	/// <summary>
	/// 外部ツール登録ダイアログ
	/// </summary>
	public class ToolRegistDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.Button buttonParameter;
		private System.Windows.Forms.Button buttonRefFilePath;
		private System.Windows.Forms.TextBox textBoxParameter;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ContextMenu contextMenuArgs;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItemThreadName;
		private System.Windows.Forms.MenuItem menuItemThreadUrl;
		private System.Windows.Forms.MenuItem menuItemBoardName;
		private System.Windows.Forms.MenuItem menuItemBoardUrl;
		private System.Windows.Forms.MenuItem menuItemSelectedText;
		private System.Windows.Forms.MenuItem menuItemClipText;
		private System.Windows.Forms.MenuItem menuItemBoardServer;
		private System.Windows.Forms.MenuItem menuItemBoardPath;
		private System.Windows.Forms.MenuItem menuItemThreadKey;
		private System.Windows.Forms.MenuItem menuItemDatPath;
		private System.Windows.Forms.MenuItem menuItemCacheFolder;
		private Label label1;
		private TextBox textBoxFileName;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		/// <summary>
		/// ツール情報を取得または設定
		/// </summary>
		public ToolItem Item {
			set {
				textBoxName.Text = value.Name;
				textBoxFileName.Text = value.FileName;
				textBoxParameter.Text = value.Parameter;
			}
			get {
				ToolItem item = new ToolItem(
					textBoxName.Text, 
					textBoxFileName.Text,
					textBoxParameter.Text);

				return item;
			}
		}

		/// <summary>
		/// ToolRegistDialogクラスのインスタンスを初期化
		/// </summary>
		public ToolRegistDialog()
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
			this.buttonParameter = new System.Windows.Forms.Button();
			this.buttonRefFilePath = new System.Windows.Forms.Button();
			this.textBoxParameter = new System.Windows.Forms.TextBox();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.contextMenuArgs = new System.Windows.Forms.ContextMenu();
			this.menuItemThreadName = new System.Windows.Forms.MenuItem();
			this.menuItemThreadUrl = new System.Windows.Forms.MenuItem();
			this.menuItemThreadKey = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItemCacheFolder = new System.Windows.Forms.MenuItem();
			this.menuItemDatPath = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemBoardName = new System.Windows.Forms.MenuItem();
			this.menuItemBoardUrl = new System.Windows.Forms.MenuItem();
			this.menuItemBoardServer = new System.Windows.Forms.MenuItem();
			this.menuItemBoardPath = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItemClipText = new System.Windows.Forms.MenuItem();
			this.menuItemSelectedText = new System.Windows.Forms.MenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxFileName = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// buttonParameter
			// 
			this.buttonParameter.AutoSize = true;
			this.buttonParameter.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonParameter.Location = new System.Drawing.Point(313, 103);
			this.buttonParameter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonParameter.Name = "buttonParameter";
			this.buttonParameter.Size = new System.Drawing.Size(84, 21);
			this.buttonParameter.TabIndex = 7;
			this.buttonParameter.Text = "パラメータ(&P)";
			this.buttonParameter.Click += new System.EventHandler(this.buttonParameter_Click);
			// 
			// buttonRefFilePath
			// 
			this.buttonRefFilePath.AutoSize = true;
			this.buttonRefFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRefFilePath.Location = new System.Drawing.Point(313, 61);
			this.buttonRefFilePath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonRefFilePath.Name = "buttonRefFilePath";
			this.buttonRefFilePath.Size = new System.Drawing.Size(84, 21);
			this.buttonRefFilePath.TabIndex = 3;
			this.buttonRefFilePath.Text = "参照(&R)...";
			this.buttonRefFilePath.Click += new System.EventHandler(this.buttonRefFilePath_Click);
			// 
			// textBoxParameter
			// 
			this.textBoxParameter.Location = new System.Drawing.Point(4, 104);
			this.textBoxParameter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBoxParameter.Name = "textBoxParameter";
			this.textBoxParameter.Size = new System.Drawing.Size(303, 19);
			this.textBoxParameter.TabIndex = 6;
			// 
			// textBoxName
			// 
			this.textBoxName.Location = new System.Drawing.Point(4, 20);
			this.textBoxName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(303, 19);
			this.textBoxName.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(4, 88);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 12);
			this.label4.TabIndex = 5;
			this.label4.Text = "パラメータ(&E)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(4, 4);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "ツールの名前(&N)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonOK
			// 
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(313, 4);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(84, 21);
			this.buttonOK.TabIndex = 8;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(313, 28);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(84, 21);
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
			// 
			// contextMenuArgs
			// 
			this.contextMenuArgs.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemThreadName,
            this.menuItemThreadUrl,
            this.menuItemThreadKey,
            this.menuItem12,
            this.menuItemCacheFolder,
            this.menuItemDatPath,
            this.menuItem3,
            this.menuItemBoardName,
            this.menuItemBoardUrl,
            this.menuItemBoardServer,
            this.menuItemBoardPath,
            this.menuItem6,
            this.menuItemClipText,
            this.menuItemSelectedText});
			// 
			// menuItemThreadName
			// 
			this.menuItemThreadName.Index = 0;
			this.menuItemThreadName.Text = "スレッド名(&N)";
			this.menuItemThreadName.Click += new System.EventHandler(this.menuItemThreadName_Click);
			// 
			// menuItemThreadUrl
			// 
			this.menuItemThreadUrl.Index = 1;
			this.menuItemThreadUrl.Text = "スレッドURL(&U)";
			this.menuItemThreadUrl.Click += new System.EventHandler(this.menuItemThreadUrl_Click);
			// 
			// menuItemThreadKey
			// 
			this.menuItemThreadKey.Index = 2;
			this.menuItemThreadKey.Text = "スレッドDAT番号(&K)";
			this.menuItemThreadKey.Click += new System.EventHandler(this.menuItemThreadKey_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 3;
			this.menuItem12.Text = "-";
			// 
			// menuItemCacheFolder
			// 
			this.menuItemCacheFolder.Index = 4;
			this.menuItemCacheFolder.Text = "キャッシュフォルダ(&F)";
			this.menuItemCacheFolder.Click += new System.EventHandler(this.menuItemCacheFolder_Click);
			// 
			// menuItemDatPath
			// 
			this.menuItemDatPath.Index = 5;
			this.menuItemDatPath.Text = "DATへのファイルパス(&P)";
			this.menuItemDatPath.Click += new System.EventHandler(this.menuItemDatPath_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 6;
			this.menuItem3.Text = "-";
			// 
			// menuItemBoardName
			// 
			this.menuItemBoardName.Index = 7;
			this.menuItemBoardName.Text = "板名(&B)";
			this.menuItemBoardName.Click += new System.EventHandler(this.menuItemBoardName_Click);
			// 
			// menuItemBoardUrl
			// 
			this.menuItemBoardUrl.Index = 8;
			this.menuItemBoardUrl.Text = "板URL(&R)";
			this.menuItemBoardUrl.Click += new System.EventHandler(this.menuItemBoardUrl_Click);
			// 
			// menuItemBoardServer
			// 
			this.menuItemBoardServer.Index = 9;
			this.menuItemBoardServer.Text = "板のサーバーアドレス(&V)";
			this.menuItemBoardServer.Click += new System.EventHandler(this.menuItemBoardServer_Click);
			// 
			// menuItemBoardPath
			// 
			this.menuItemBoardPath.Index = 10;
			this.menuItemBoardPath.Text = "板のディレクトリ(&D)";
			this.menuItemBoardPath.Click += new System.EventHandler(this.menuItemBoardPath_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 11;
			this.menuItem6.Text = "-";
			// 
			// menuItemClipText
			// 
			this.menuItemClipText.Index = 12;
			this.menuItemClipText.Text = "クリップボードの内容(&C)";
			this.menuItemClipText.Click += new System.EventHandler(this.menuItemClipText_Click);
			// 
			// menuItemSelectedText
			// 
			this.menuItemSelectedText.Index = 13;
			this.menuItemSelectedText.Text = "選択文字列(&S)";
			this.menuItemSelectedText.Click += new System.EventHandler(this.menuItemSelectedText_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(4, 47);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "ファイルパス(&F)";
			// 
			// textBoxFileName
			// 
			this.textBoxFileName.Location = new System.Drawing.Point(4, 62);
			this.textBoxFileName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBoxFileName.Name = "textBoxFileName";
			this.textBoxFileName.Size = new System.Drawing.Size(303, 19);
			this.textBoxFileName.TabIndex = 4;
			// 
			// ToolRegistDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(404, 133);
			this.Controls.Add(this.textBoxFileName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonParameter);
			this.Controls.Add(this.buttonRefFilePath);
			this.Controls.Add(this.textBoxParameter);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ToolRegistDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "外部ツールを登録";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// パラメータを追加
		/// </summary>
		/// <param name="param"></param>
		private void AppendParameter(string param)
		{
			int index = textBoxParameter.SelectionStart;
			textBoxParameter.Text = textBoxParameter.Text.Insert(index, "{" + param + "}");
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void buttonRefFilePath_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				textBoxFileName.Text = openFileDialog.FileName;

				if (textBoxName.Text == String.Empty)
				{
					textBoxName.Text = 
						System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
				}
			}
		}

		private void buttonParameter_Click(object sender, System.EventArgs e)
		{
			contextMenuArgs.Show(this,
				PointToClient(MousePosition));	
		}

		private void menuItemThreadName_Click(object sender, System.EventArgs e)
		{
			AppendParameter("ThreadName");
		}

		private void menuItemThreadUrl_Click(object sender, System.EventArgs e)
		{
			AppendParameter("ThreadUrl");
		}

		private void menuItemThreadKey_Click(object sender, System.EventArgs e)
		{
			AppendParameter("ThreadKey");
		}

		private void menuItemCacheFolder_Click(object sender, System.EventArgs e)
		{
			AppendParameter("CacheFolder");
		}

		private void menuItemDatPath_Click(object sender, System.EventArgs e)
		{
			AppendParameter("DatPath");
		}

		private void menuItemBoardName_Click(object sender, System.EventArgs e)
		{
			AppendParameter("BoardName");
		}

		private void menuItemBoardUrl_Click(object sender, System.EventArgs e)
		{
			AppendParameter("BoardUrl");
		}

		private void menuItemBoardServer_Click(object sender, System.EventArgs e)
		{
			AppendParameter("BoardServer");
		}

		private void menuItemBoardPath_Click(object sender, System.EventArgs e)
		{
			AppendParameter("BoardPath");
		}

		private void menuItemClipText_Click(object sender, System.EventArgs e)
		{
			AppendParameter("Clipboard");
		}

		private void menuItemSelectedText_Click(object sender, System.EventArgs e)
		{
			AppendParameter("Selected");
		}
	}
}
