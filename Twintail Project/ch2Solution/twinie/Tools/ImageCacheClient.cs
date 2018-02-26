using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Net;
using Twin.Forms;
using System.Drawing;

namespace Twin.Tools
{
	/// <summary>
	/// �摜���������[�J���ɃL���b�V�����Ă����N���X�ł��B
	/// </summary>
	class ImageCacheClient
	{
		private AutoResetEvent autoResetEvent = null;
		private Thread thread = null;

		private Queue<string> urlQueue = new Queue<string>();
		private Dictionary<string, string> urlDic = new Dictionary<string,string>();

		private bool threadExit = false;

		private string imageCacheDir;
		/// <summary>
		/// �L���b�V�������摜�f�[�^��ۑ�����f�B���N�g�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string ImageCacheDirectory
		{
			get
			{
				return imageCacheDir;
			}
			set
			{
				imageCacheDir = value;
			}
		}

		private int imageSizeLimit = 0;
		/// <summary>
		/// �L���b�V������摜�̍ő�T�C�Y�i�o�C�g���j���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public int ImageSizeLimit
		{
			get
			{
				return imageSizeLimit;
			}
			set
			{
				imageSizeLimit = value;
			}
		}
	

		/// <summary>
		/// �ڑ��������Ă��邩�ǂ����������l���擾���܂��B
		/// </summary>
		public bool IsClosed
		{
			get
			{
				return threadExit;
			}
		}

		private bool suspend = false;
		/// <summary>
		/// �L���b�V�����������쒆���ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool IsSuspend
		{
			get
			{
				lock (this)
					return suspend;
			}
			set
			{
				lock (this)
				{
					suspend = value;

					if (!suspend && thread != null)
					{
						autoResetEvent.Set();
					}
				}
			}
		}
	
	

		/// <summary>
		/// �C���[�W���L���b�V�������O�ɌĂ΂�܂��B
		/// </summary>
		public event ImageCacheEventHandler ImageCaching;

		/// <summary>
		/// �C���[�W�̃L���b�V���ɐ�������ƌĂ΂�܂��B
		/// </summary>
		public event ImageCacheEventHandler ImageCached;

		public ImageCacheClient()
		{

		}

		/// <summary>
		/// �L���b�V���X���b�h�����s���܂��B
		/// </summary>
		public void Run()
		{
			if (thread == null)
			{
				autoResetEvent = new AutoResetEvent(false);
				threadExit = false;

				thread = new Thread(CachingThread);
				thread.Name = "IMG_CACHE_CLIENT";
				thread.IsBackground = true;
				thread.Priority = ThreadPriority.Lowest;
				thread.Start();

				autoResetEvent.Set();
			}
		}

		/// <summary>
		/// �N���C�A���g���I�����܂��B
		/// </summary>
		public void Close()
		{
			if (threadExit || thread == null)
				return;

			ClearList();
			threadExit = true;
			autoResetEvent.Set();
		}

		public void ClearList()
		{
			lock (urlQueue)
			{
				urlQueue.Clear();
				urlDic.Clear();
			}
		}

		public void AddDownloadList(string url)
		{
			AddDownloadList(url, String.Empty);
		}

		public void AddDownloadList(string url, string referer)
		{
			// url �̌`�����������Ȃ��ꍇ�̓��X�g�ɒǉ����Ȃ�
			if (! Uri.IsWellFormedUriString(url, UriKind.Absolute))
				return;

			if (!url.ToLower().StartsWith("http://"))
				return;

			lock (urlQueue)
			{
				if (!urlQueue.Contains(url))
				{
					urlQueue.Enqueue(url);
					urlDic.Add(url, referer);
				}
			}

			if (thread == null)
				return;

			if (urlQueue.Count > 0)
			{
				autoResetEvent.Set();
			}
		}

		private void CachingThread()
		{
			try
			{
				while (!threadExit)
				{
					if (urlQueue.Count == 0 || IsSuspend)
					{
						autoResetEvent.Reset();
						autoResetEvent.WaitOne();
					}
					else
					{
						string url, referer;

						lock (urlQueue)
						{
							url = urlQueue.Dequeue();
							referer = urlDic[url];

							urlDic.Remove(url);
						}

						DownloadImage(url, referer);
					}
				}
			}
			finally
			{
				thread = null;
				autoResetEvent.Close();
			}
		}

		private void DownloadImage(string url, string referer)
		{
			// �t�@�C�����Ɏg�p�ł��Ȃ�������A�g���q�̃h�b�g�ȊO�͂��ׂăA���_�[�o�[�ɒu��
			string fileName = StringUtility.ReplaceInvalidPathChars(Path.GetFileName(url), "_");
			string dir = StringUtility.ReplaceInvalidPathChars(Path.GetDirectoryName(url), "_");

			fileName = dir.Replace('.', '_') + "_" + fileName;
			fileName = Path.Combine(imageCacheDir, fileName);

			ImageCacheEventArgs argument =
				new ImageCacheEventArgs(null, url, fileName);

			OnImageCaching(argument);

			if (argument.Cancel)
				return;

			if (File.Exists(fileName))
			{
				// ���ɃL���b�V���ς݁B
			}
			else
			{
				DownloadData(url, referer, fileName);
			}

			OnImageCached(argument);
		}

		private void DownloadData(string url, string referer, string fileName)
		{


			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.Timeout = req.ReadWriteTimeout = 15000;
			req.Referer = referer;

			HttpWebResponse res = null;
			try
			{
				res = (HttpWebResponse)req.GetResponse();

				if (res.StatusCode == HttpStatusCode.OK)
				{
					if (imageSizeLimit > 0 &&
						res.ContentLength > imageSizeLimit)
					{
						// �ł�������摜�̏ꍇ�A�G���[�p�̉摜�����
						Image i = new Bitmap(50, 50);
						Graphics g = Graphics.FromImage(i);
						g.FillRectangle(Brushes.Yellow, 0, 0, i.Width, i.Height);
						g.Dispose();
						i.Save(fileName);
						i.Dispose();
						return;
					}

					Stream stream = res.GetResponseStream();
					byte[] buffer = new byte[32768];

					try
					{
						using (FileStream fs = new FileStream(fileName, FileMode.Create))
						{
							int read = -1;

							while (read != 0)
							{
								read = stream.Read(buffer, 0, buffer.Length);
								fs.Write(buffer, 0, read);
							}
						}
					}
					catch (ArgumentException ex)
					{
						TwinDll.Output("{0}\r\nfileName = {1}",
							ex.ToString(), fileName);
					}
					catch (System.Security.SecurityException)
					{
						// ���̃C���X�^���X�����ɓ����t�@�C�����_�E�����[�h���Ă���Ǝv����					
					}
				}

			}
			catch (WebException)
			{
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex.ToString());
			}
			finally
			{
				if (res != null)
					res.Close();
			}
		}

		private void OnImageCaching(ImageCacheEventArgs e)
		{
			if (threadExit)
				return;

			if (ImageCaching != null)
				ImageCaching(this, e);
		}

		private void OnImageCached(ImageCacheEventArgs e)
		{
			if (threadExit)
				return;

			if (ImageCached != null)
				ImageCached(this, e);
		}
	}


	public delegate void ImageCacheEventHandler(object sender, ImageCacheEventArgs e);

	public class ImageCacheEventArgs : EventArgs
	{
		private string fileName;
		/// <summary>
		/// ���[�J���ɕۑ����ꂽ�L���b�V���ւ̃t�@�C�������擾���܂��B
		/// </summary>
		public string FileName
		{
			get
			{
				return fileName;
			}
		}

		private object sourceObject;
		/// <summary>
		/// ���̃C���[�W�̃L���b�V����v�������I�u�W�F�N�g���擾���܂��B
		/// </summary>
		public object SourceObject
		{
			get
			{
				return sourceObject;
			}
		}


		private string sourceUri;
		/// <summary>
		/// ���\�[�X���� URI ���擾���܂��B
		/// </summary>
		public string SourceUri
		{
			get
			{
				return sourceUri;
			}
		}

		private bool cancel = false;
		/// <summary>
		/// �C���[�W�̃L���b�V���i�_�E�����[�h�j���L�����Z�����邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool Cancel
		{
			get
			{
				return cancel;
			}
			set
			{
				cancel = value;
			}
		}

		public ImageCacheEventArgs(object srcObject, string uri, string filename)
		{
			this.sourceObject = srcObject;
			this.sourceUri = uri;
			this.fileName = filename;
		}
	}
}
