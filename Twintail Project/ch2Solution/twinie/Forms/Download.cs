using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using Twin.Tools;

namespace Twin.Forms
{
	public partial class Download : Form
	{
		private Thread thread;
		private string folderPath;
		private Queue<string> queue = new Queue<string>();
		private ImageViewUrlReplace urlRep = new ImageViewUrlReplace(Settings.ImageViewUrlReplacePath);
		private bool cancelled = false;

		public Download(string path, string[] uris)
		{
			InitializeComponent();

			folderPath = path;
			thread = new Thread(OnDownloading);
			thread.Name = "DOWNLOAD_FORM";
			thread.IsBackground = true;

			foreach (string uri in uris)
				queue.Enqueue(uri);

			progressBar1.Maximum = queue.Count;
			progressBar1.Step = 1;
			progressBar1.Value = 0;
		}

		private void OnDownloading()
		{
			WebClient client = new WebClient();
			try
			{
				foreach (string sourceUri in queue)
				{
					try
					{
						string fileName = Path.Combine(folderPath, StringUtility.ReplaceInvalidPathChars(
							Path.GetFileName(sourceUri), "_"));

						Invoke((MethodInvoker)delegate
						{
							progressBar1.PerformStep();
							labelUri.Text = String.Format("({0}/{1}) ", progressBar1.Value, progressBar1.Maximum) + sourceUri;
						});

						if (File.Exists(fileName))
							continue;

						string referer, targetUri = sourceUri;
						if (urlRep.Replace(ref targetUri, out referer))
						{
							client.Headers[HttpRequestHeader.Referer] = referer;
						}
						else
						{
							client.Headers[HttpRequestHeader.Referer] = String.Empty;
						}

						client.DownloadFile(targetUri, fileName);
					}
					catch (Exception ex)
					{
						TwinDll.Output(ex.ToString());
					}
				}
			}
			finally
			{
				client.Dispose();
				if (!cancelled)
				{
					thread = null;
					Invoke((MethodInvoker)delegate { Close(); });
				}
			}
		}

		private void Download_Load(object sender, EventArgs e)
		{
			thread.Start();
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Download_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (thread != null && thread.IsAlive)
			{
				DialogResult r = MessageBox.Show("íÜé~ÇµÇƒÇ‡ÇÊÇÎÇµÇ¢Ç≈Ç∑Ç©ÅH", "íÜé~ÇÃämîF",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				
				if (r == DialogResult.No)
				{
					e.Cancel = true;
				}
				else
				{
					cancelled = true;
					thread.Abort();
				}
			}

		}
	}
}