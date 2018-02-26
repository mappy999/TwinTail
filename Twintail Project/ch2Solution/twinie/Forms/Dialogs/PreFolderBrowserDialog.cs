using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Twin.Forms
{
	public partial class PreFolderBrowserDialog : Form
	{
		public bool SetDefaultPathChecked
		{
			get { return checkBoxSetDefault.Checked; }
			set { checkBoxSetDefault.Checked = value; }
		}

		public string SelectedPath
		{
			get { return comboBoxPath.Text; }
			set { comboBoxPath.Text = value ?? ""; }
		}

		public PreFolderBrowserDialog()
		{
			InitializeComponent();
		}

		private void buttonRefFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog() { ShowNewFolderButton = true };
			if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				this.SelectedPath = dlg.SelectedPath;
			}
		}

		private void PreFolderBrowserDialog_Load(object sender, EventArgs e)
		{
			InitializeComboBox();
		}

		private void InitializeComboBox()
		{
			// Loadイベント時に SelectedPath プロパティが設定されていたら、
			// そのフォルダと下位フォルダ一覧を追加しておく
			if (Directory.Exists(SelectedPath))
			{
				comboBoxPath.Items.Add(SelectedPath);

				string[] subDir = Directory.GetDirectories(SelectedPath, "*.*", SearchOption.TopDirectoryOnly);
				foreach (var d in subDir) comboBoxPath.Items.Add(d);

				comboBoxPath.SelectedIndex = 0;
			}
		}

		private void comboBoxPath_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!Directory.Exists(SelectedPath))
			{
				try
				{
					if (MessageBox.Show("指定したフォルダは存在しません。作成しますか？", "フォルダ作成の確認",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						Directory.CreateDirectory(SelectedPath);
						this.DialogResult = DialogResult.OK;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, ex.Message, "フォルダを自動生成できません");
				}
			}
			else
			{
				this.DialogResult = DialogResult.OK;
			}
		}
	}
}
