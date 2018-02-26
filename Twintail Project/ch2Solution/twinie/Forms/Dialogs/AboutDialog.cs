using System;
// AboutDialog.cs

namespace Twin.Forms
{
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// バージョン情報ダイアログ
	/// </summary>
	public class AboutDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.LinkLabel linkLabelWebSite;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Label label1;
		private Label labelVersionInfo;
		private ContextMenuStrip contextMenuStrip1;
		private ToolStripMenuItem menuItemCopy;
		private IContainer components;
		private Label labelUseTotalMemory;

		private string versionText;

		/// <summary>
		/// AboutDialogクラスのインスタンスを初期化
		/// </summary>
		public AboutDialog()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			versionText = String.Empty;
			versionText += Environment.OSVersion + " CLR " + Environment.Version + "\r\n";
			versionText += "twintail.exe " + Twinie.Version.ToString() + ", ";
			versionText += "twin.dll " + TwinDll.Version.ToString() + "\r\n";
			labelVersionInfo.Text = versionText;

			linkLabelWebSite.Text = Settings.WebSiteUrl;

			long memoryBytes = GC.GetTotalMemory(true);
			labelUseTotalMemory.Text = String.Format("メモリ使用量: {0:#,##0} KB", memoryBytes / 1024);
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// 
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
			this.linkLabelWebSite = new System.Windows.Forms.LinkLabel();
			this.buttonClose = new System.Windows.Forms.Button();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.labelVersionInfo = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.labelUseTotalMemory = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// linkLabelWebSite
			// 
			this.linkLabelWebSite.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.linkLabelWebSite.Location = new System.Drawing.Point(105, 49);
			this.linkLabelWebSite.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.linkLabelWebSite.Name = "linkLabelWebSite";
			this.linkLabelWebSite.Size = new System.Drawing.Size(304, 12);
			this.linkLabelWebSite.TabIndex = 5;
			this.linkLabelWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWebSite_LinkClicked);
			// 
			// buttonClose
			// 
			this.buttonClose.AutoSize = true;
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClose.Location = new System.Drawing.Point(321, 67);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(88, 21);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "OK";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// pictureBox
			// 
			this.pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
			this.pictureBox.Location = new System.Drawing.Point(6, 10);
			this.pictureBox.Margin = new System.Windows.Forms.Padding(2);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(32, 32);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox.TabIndex = 6;
			this.pictureBox.TabStop = false;
			this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(44, 49);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 12);
			this.label1.TabIndex = 7;
			this.label1.Text = "Webサイト";
			// 
			// labelVersionInfo
			// 
			this.labelVersionInfo.ContextMenuStrip = this.contextMenuStrip1;
			this.labelVersionInfo.Location = new System.Drawing.Point(44, 4);
			this.labelVersionInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelVersionInfo.Name = "labelVersionInfo";
			this.labelVersionInfo.Size = new System.Drawing.Size(365, 43);
			this.labelVersionInfo.TabIndex = 8;
			this.labelVersionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCopy});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(128, 26);
			// 
			// menuItemCopy
			// 
			this.menuItemCopy.Name = "menuItemCopy";
			this.menuItemCopy.Size = new System.Drawing.Size(127, 22);
			this.menuItemCopy.Text = "コピー(&C)";
			this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
			// 
			// labelUseTotalMemory
			// 
			this.labelUseTotalMemory.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelUseTotalMemory.Location = new System.Drawing.Point(45, 68);
			this.labelUseTotalMemory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelUseTotalMemory.Name = "labelUseTotalMemory";
			this.labelUseTotalMemory.Size = new System.Drawing.Size(212, 15);
			this.labelUseTotalMemory.TabIndex = 9;
			// 
			// AboutDialog
			// 
			this.AcceptButton = this.buttonClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(419, 99);
			this.Controls.Add(this.labelUseTotalMemory);
			this.Controls.Add(this.labelVersionInfo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkLabelWebSite);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.buttonClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "バージョン情報";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void linkLabelWebSite_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			CommonUtility.OpenWebBrowser(Settings.WebSiteUrl);
		}

		private void pictureBox_Click(object sender, System.EventArgs e)
		{
		}

		private void menuItemCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetData(DataFormats.Text, versionText);
		}
	}
}
