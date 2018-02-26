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
	/// 画像を順次ローカルにキャッシュしていくクラスです。
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
		/// キャッシュした画像データを保存するディレクトリを取得または設定します。
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
		/// キャッシュする画像の最大サイズ（バイト数）を取得または設定します。
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
		/// 接続が閉じられているかどうかを示す値を取得します。
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
		/// キャッシュ処理が動作中かどうかを取得または設定します。
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
		/// イメージがキャッシュされる前に呼ばれます。
		/// </summary>
		public event ImageCacheEventHandler ImageCaching;

		/// <summary>
		/// イメージのキャッシュに成功すると呼ばれます。
		/// </summary>
		public event ImageCacheEventHandler ImageCached;

		public ImageCacheClient()
		{

		}

		/// <summary>
		/// キャッシュスレッドを実行します。
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
		/// クライアントを終了します。
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
			// url の形式が正しくない場合はリストに追加しない
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
			// ファイル名に使用できない文字や、拡張子のドット以外はすべてアンダーバーに置換
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
				// 既にキャッシュ済み。
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
						// でかすぎる画像の場合、エラー用の画像を作る
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
						// 他のインスタンスが既に同じファイルをダウンロードしていると思われる					
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
		/// ローカルに保存されたキャッシュへのファイル名を取得します。
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
		/// このイメージのキャッシュを要求したオブジェクトを取得します。
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
		/// リソース元の URI を取得します。
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
		/// イメージのキャッシュ（ダウンロード）をキャンセルするかどうかを示す値を取得または設定します。
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
