// OpenUrlDialog.cs

namespace ImageViewerDll
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// OpenUrlDialog の概要の説明です。
	/// </summary>
	public class OpenUrlDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBoxUrls;
		private System.Windows.Forms.Button buttonOK;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 入力されたURLを取得
		/// </summary>
		public string[] Urls {
			get {
				return textBoxUrls.Lines;
			}
		}

		/// <summary>
		/// OpenUrlDialogクラスのインスタンスを初期化
		/// </summary>
		public OpenUrlDialog()
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
			this.textBoxUrls = new System.Windows.Forms.TextBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxUrls
			// 
			this.textBoxUrls.AcceptsReturn = true;
			this.textBoxUrls.AllowDrop = true;
			this.textBoxUrls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUrls.Location = new System.Drawing.Point(4, 4);
			this.textBoxUrls.Multiline = true;
			this.textBoxUrls.Name = "textBoxUrls";
			this.textBoxUrls.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxUrls.Size = new System.Drawing.Size(332, 88);
			this.textBoxUrls.TabIndex = 0;
			this.textBoxUrls.WordWrap = false;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(268, 95);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(68, 21);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "開く";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// OpenUrlDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(340, 116);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.textBoxUrls);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "OpenUrlDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "OpenUrlDialog";
			this.Shown += new System.EventHandler(this.OpenUrlDialog_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void OpenUrlDialog_Shown(object sender, EventArgs e)
		{
			string text = Clipboard.GetText();
			
			if (text == null)
				return;

			textBoxUrls.Text = text;
		}
	}
}
