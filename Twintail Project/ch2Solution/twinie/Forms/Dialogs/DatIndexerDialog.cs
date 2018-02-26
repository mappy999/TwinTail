using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Twin
{
	public partial class DatIndexerDialog : Form
	{
		string dir, name, path,server;
		List<IndexParameter> parameterList = new List<IndexParameter>();

		public DatIndexerDialog(Cache cache)
		{
			InitializeComponent();

			this.progressBar1.Value = 0;
		}

		private void buttonAddList_Click(object sender, EventArgs e)
		{
			dir = textBoxDir.Text;
			name = textBoxBoardName.Text;
			path = textBoxBoardPath.Text;
			server = textBoxServer.Text;

			if (String.IsNullOrEmpty(dir))
			{
				MessageBox.Show("�ϊ����̃t�H���_������͂��Ă�������");
				textBoxDir.Focus();
				return;
			}
			if (!Directory.Exists(dir))
			{
				MessageBox.Show(dir + "�͑��݂��܂���");
				return;
			}
			if (String.IsNullOrEmpty(server))
			{
				MessageBox.Show("�T�[�o�[������͂��Ă�������");
				textBoxServer.Focus();
				return;
			}
			if (String.IsNullOrEmpty(name))
			{
				MessageBox.Show("������͂��Ă�������");
				textBoxBoardName.Focus();
				return;
			}
			if (String.IsNullOrEmpty(path))
			{
				MessageBox.Show("�̃A�h���X����͂��Ă�������");
				textBoxBoardPath.Focus();
				return;
			}

			IndexParameter p = new IndexParameter();
			p.files = Directory.GetFiles(dir, "*.dat");
			p.path = path;
			p.name = name;
			p.server = server;

			parameterList.Add(p);

			ListViewItem item = new ListViewItem(Path.GetFileName(dir));
			item.SubItems.Add(p.files.Length.ToString());
			item.SubItems.Add(name);
			item.SubItems.Add("http://" + server + "/" + path + "/");

			listView1.Items.Add(item);

			textBoxBoardName.Text =
				textBoxBoardPath.Text = textBoxDir.Text = textBoxServer.Text = String.Empty;
		}

		private void buttonRef_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
			{
				textBoxDir.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void buttonRun_Click(object sender, EventArgs e)
		{
			this.buttonAddList.Enabled = false;
			this.buttonRef.Enabled = false;
			this.buttonRun.Enabled = false;

			this.progressBar1.Value = 0;
			this.backgroundWorker1.RunWorkerAsync();
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			if (backgroundWorker1.IsBusy)
				Cancelling();
			else
				Close();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			CreateDatIndexer indexer = new CreateDatIndexer();

			foreach (IndexParameter p in parameterList)
			{
				indexer.Indexing(new BoardInfo(p.server, p.path, p.name), p.files, delegate(float per)
				{
					backgroundWorker1.ReportProgress((int)per);
				});
			}
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBar1.Value = e.ProgressPercentage;
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.buttonAddList.Enabled = true;
			this.buttonRef.Enabled = true;
			this.buttonRun.Enabled = true;
		}

		private void DatIndexerDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (backgroundWorker1.IsBusy)
			{
				if (!Cancelling())
					e.Cancel = true;
			}
		}

		private bool Cancelling()
		{
			DialogResult r = MessageBox.Show("�����𒆎~���܂����H", "�m�F",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (r == DialogResult.Yes)
			{
				backgroundWorker1.CancelAsync();
				return true;
			}
			else
				return false;
		}
	}

	class IndexParameter
	{
		public string[] files;
		public string name;
		public string path;
		public string server;
	}
}