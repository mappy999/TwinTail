using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Twin.Tools;

namespace Twin.Forms
{
	/// <summary>
	/// BEユーザー情報の入力と、ログインを行うダイアログ
	/// </summary>
	public partial class BeLoginDialog : Form
	{
		public BeSettings BeSettings { get; set; }

		public string DMDM { get; private set; }
		public string MDMD { get; private set; }

		public BeLoginDialog(BeSettings settings)
		{
			InitializeComponent();

			this.textBoxMailAddress.Text = settings.Username;
			this.textBoxPassword.Text = settings.Password;

			this.BeSettings = settings;
			this.DMDM = this.MDMD = String.Empty;
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

			try
			{
				BeLoginManager be = new BeLoginManager();

				if (be.Login(this.BeSettings))
				{
					this.MDMD = be.MDMD;
					this.DMDM = be.DMDM;
					this.BeSettings.AuthenticationOn = true;
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
					//this.Close();
				}
				else
				{
					MessageBox.Show(be.ResponseText);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ログイン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				TwinDll.Output(ex.ToString());
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{

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
