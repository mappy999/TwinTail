// AuthenticateSettingDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using Twin;
	using Twin.Bbs;

	/// <summary>
	/// Summary description for AuthenticateSettingDialog.
	/// </summary>
	public class AuthenticateSettingDialog : System.Windows.Forms.Form
	{
		private Settings settings;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox usernameText;
		private System.Windows.Forms.TextBox passwordText;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonTest;
		private System.Windows.Forms.CheckBox authenticationON;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AuthenticateSettingDialog(Settings settings)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.settings = settings;
		}

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.usernameText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.passwordText = new System.Windows.Forms.TextBox();
			this.authenticationON = new System.Windows.Forms.CheckBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonTest = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(16, 52);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "ユーザーID(&L)";
			// 
			// usernameText
			// 
			this.usernameText.Location = new System.Drawing.Point(92, 44);
			this.usernameText.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.usernameText.Name = "usernameText";
			this.usernameText.Size = new System.Drawing.Size(219, 19);
			this.usernameText.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(16, 82);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "パスワード(&P)";
			// 
			// passwordText
			// 
			this.passwordText.Location = new System.Drawing.Point(92, 74);
			this.passwordText.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.passwordText.Name = "passwordText";
			this.passwordText.PasswordChar = '*';
			this.passwordText.Size = new System.Drawing.Size(219, 19);
			this.passwordText.TabIndex = 4;
			// 
			// authenticationON
			// 
			this.authenticationON.AutoSize = true;
			this.authenticationON.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.authenticationON.Location = new System.Drawing.Point(16, 22);
			this.authenticationON.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.authenticationON.Name = "authenticationON";
			this.authenticationON.Size = new System.Drawing.Size(98, 17);
			this.authenticationON.TabIndex = 0;
			this.authenticationON.Text = "認証を行う(&A)";
			this.authenticationON.CheckedChanged += new System.EventHandler(this.authenticationON_CheckedChanged);
			// 
			// buttonOK
			// 
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(152, 124);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(79, 21);
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(244, 124);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(79, 21);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonTest
			// 
			this.buttonTest.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonTest.Location = new System.Drawing.Point(8, 124);
			this.buttonTest.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonTest.Name = "buttonTest";
			this.buttonTest.Size = new System.Drawing.Size(110, 21);
			this.buttonTest.TabIndex = 7;
			this.buttonTest.Text = "認証の確認(&T)";
			this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.usernameText);
			this.groupBox1.Controls.Add(this.passwordText);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 7);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Size = new System.Drawing.Size(320, 109);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// AuthenticateSettingDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(339, 151);
			this.Controls.Add(this.authenticationON);
			this.Controls.Add(this.buttonTest);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AuthenticateSettingDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "認証設定";
			this.Load += new System.EventHandler(this.AuthenticateSettingDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void AuthenticateSettingDialog_Load(object sender, System.EventArgs e)
		{
			this.authenticationON.Checked = this.settings.Authentication.AuthenticationOn;
			this.usernameText.Text = this.settings.Authentication.Username;
			this.passwordText.Text = this.settings.Authentication.Password;
			EnableUsernameAndPassword(this.authenticationON.Checked);
		}

		private void EnableUsernameAndPassword(bool enableAuthentication)
		{
			this.usernameText.Enabled = enableAuthentication;
			this.passwordText.Enabled = enableAuthentication;
			this.buttonTest.Enabled = enableAuthentication;
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			this.settings.Authentication.AuthenticationOn = this.authenticationON.Checked;
			if (this.settings.Authentication.AuthenticationOn)
			{
				this.settings.Authentication.Username = this.usernameText.Text;
				this.settings.Authentication.Password = this.passwordText.Text;
			}
			this.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void authenticationON_CheckedChanged(object sender, System.EventArgs e)
		{
			EnableUsernameAndPassword(this.authenticationON.Checked);
		}

		private void buttonTest_Click(object sender, System.EventArgs e)
		{
			if (X2chAuthenticator.IsValidUsernamePassword(this.usernameText.Text, this.passwordText.Text))
			{
				MessageBox.Show(this, "認証に成功しました", "認証テスト");
			}
			else
			{
				MessageBox.Show(this, "認証に失敗しました。ユーザーIDとパスワードが正しいか確認してください。", "認証テスト");
			}
		}
	}
}
