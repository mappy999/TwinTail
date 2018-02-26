using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Twin.Tools;
using Twin.Bbs;

namespace Twin.Forms
{
	/// <summary>
	/// BEユーザー情報の入力と、ログインを行うダイアログ
	/// </summary>
	public partial class BeLoginDialog2 : Form
	{
		public BeSettings BeSettings { get; set; }

		public BeLoginDialog2(BeSettings settings)
		{
			InitializeComponent();
			this.textBoxMailAddress.Text = settings.Username;
			this.textBoxPassword.Text = settings.Password;
			this.BeSettings = settings;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.BeSettings.Username = this.textBoxMailAddress.Text;
			this.BeSettings.Password = this.textBoxPassword.Text;

			if (this.textBoxMailAddress.TextLength == 0)
			{
				MessageBox.Show("メールアドレスが入力されていません...?");
				return;
			}
			else if (this.textBoxPassword.TextLength == 0)
			{
				MessageBox.Show("パスワードが入力されていません...??");
				return;
			}

			BeLoginManager2 be = new BeLoginManager2() { 
				UserAgnet = TwinDll.UserAgent,
				CompleteMessageBox = true,
				Email = textBoxMailAddress.Text,
				PW = textBoxPassword.Text,
			};
			be.Login(CookieManager.gCookies);

			this.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void BeLoginDialog_Load(object sender, EventArgs e)
		{
		}

		private void BeLoginDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		private void BeLoginDialog_FormClosed(object sender, FormClosedEventArgs e)
		{
		}
	}

}
