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
	public partial class CacheRemovingTool : Form
	{
		private Cache cache;

		private bool cancelled = false;
		public bool IsCancelled
		{
			get
			{
				return cancelled;
			}
		}
	
		public CacheRemovingTool(Cache cache)
		{
			InitializeComponent();

			this.cache = cache;
		}

		private void CacheRemovingTool_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (backgroundWorker1.IsBusy)
			{
				if (MessageBox.Show("処理を中止しますか？", "中止の確認",
						MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					backgroundWorker1.CancelAsync();
				}
				else
				{
					e.Cancel = true;
				}
			}

			backgroundWorker1.Dispose();

		}

		private void CacheRemovingTool_Load(object sender, EventArgs e)
		{
			progressBar1.Value = 0;
			backgroundWorker1.RunWorkerAsync();
		}

		List<string> indexFileNames = new List<string>();
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			SearchIndexFiles(indexFileNames, cache.BaseDirectory);

			MethodInvoker m = delegate
			{
				progressBar1.Maximum = indexFileNames.Count;
			};
			Invoke(m);

			int progressCount = 0;

			foreach (string path in indexFileNames)
			{
				if (e.Cancel)
					return;

				try
				{
					ThreadHeader h = ThreadIndexer.Read(path);

					cache.NewStructMode = false; // 従来のdatのファイルパスを取得
					string datPath = cache.GetDatPath(h);

					cache.NewStructMode = true; // 新しい移動先ディレクトリを取得（存在しなければ作成）
					string newDir = cache.GetFolderPath(h.BoardInfo, true);

					string datFileName = Path.GetFileName(datPath);
					string indexFileName = Path.GetFileName(path);

					File.Copy(path, Path.Combine(newDir, indexFileName), true);
					File.Copy(datPath, Path.Combine(newDir, datFileName), true);

					backgroundWorker1.ReportProgress(progressCount++);
				}
				catch (Exception ex)
				{
					TwinDll.Output(ex);
				}

			}

			IBoardTable boardList = new KatjuBoardTable();
			boardList.LoadTable(Settings.BoardTablePath);
			boardList.OnlineUpdate(Twinie.Settings.OnlineUpdateUrl, null);
			boardList.SaveTable(Settings.BoardTablePath);

			if (boardList.Items.Count == 0)
			{
				e.Cancel = true;
				return;
			}

			m = delegate
			{
				progressBar1.Value = 0;
				progressBar1.Maximum = boardList.Items.Count;
			};
			Invoke(m);

			cache.NewStructMode = true;
			progressCount = 0;

			// 全ての板のインデックスを再生成
			foreach (Category cate in boardList.Items)
			{
				foreach (BoardInfo bi in cate.Children)
				{
					if (e.Cancel)
						return;
					try
					{
						ThreadIndexer.Indexing(cache, bi);
					}
					catch (Exception ex)
					{
						TwinDll.Output(ex);
					}
				}
				backgroundWorker1.ReportProgress(progressCount++);
			}
		}

		private void SearchIndexFiles(List<string> list, string directory)
		{
			string[] fileNames = Directory.GetFiles(directory, "*.idx");

			list.AddRange(fileNames);
			UpdateCount();

			foreach (string subdir in Directory.GetDirectories(directory))
			{
				SearchIndexFiles(list, subdir);
			}
		}

		private void UpdateCount()
		{
			MethodInvoker m = delegate
			{
				labelCount.Text = indexFileNames.Count.ToString();
			};
			Invoke(m);
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			cancelled = e.Cancelled;

			this.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			backgroundWorker1.CancelAsync();
			buttonCancel.Enabled = false;
		}

	}
}