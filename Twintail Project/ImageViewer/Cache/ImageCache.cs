// ImageCache.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Text;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Collections;
	using System.Threading;
	using System.Diagnostics;
	using System.Security.Cryptography;
	using System.Net;
	using System.Xml;
	using Twin.Tools;
	using System.Windows.Forms;
	using System.Collections.Generic;

	/// <summary>
	/// 画像をダウンロードしローカルにキャッシュするクラス
	/// </summary>
	public class ImageCache
	{
		private static ImageViewUrlReplace imageViewUrlTool;

		private static ServerRestrictSettings _serverRestrictSettings;
		public static ServerRestrictSettings ServerRestrictSettings { get { return _serverRestrictSettings; } }

		private MultiLimitter multiLimitter;
		private ImageCacheIndexer indexer;
		private Hashtable table; 
		private string folderPath;

		/// <summary>
		/// キャッシュの保存フォルダパスを取得
		/// </summary>
		public string BaseFolderPath {
			get {
				return folderPath;
			}
		}

		public ImageCacheIndexer Indexer {
			get {
				return indexer;
			}
		}

		static ImageCache()
		{
			imageViewUrlTool = new ImageViewUrlReplace(
				Path.Combine(Application.StartupPath, "ImageViewURLReplace.dat"));

			_serverRestrictSettings = new ServerRestrictSettings(
				Path.Combine(Application.StartupPath, "ServerRestrictSettings.xml"));
			_serverRestrictSettings.Load();
		}

		/// <summary>
		/// ImageCacheクラスのインスタンスを初期化
		/// </summary>
		/// <param name="folderPath">画像のキャッシュ保存フォルダ</param>
		public ImageCache(string folderPath)
		{
			this.folderPath = folderPath;
			this.table = new Hashtable();
			this.multiLimitter = new MultiLimitter(Loading);
			this.indexer = new ImageCacheIndexer(this);
			
			// ディレクトリが存在しなければ作成
			if (! Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
		}

		/// <summary>
		/// 非同期で指定したURLの画像をロード
		/// </summary>
		/// <param name="url">読み込む画像のURL</param>
		/// <param name="callback">処理が完了した時に呼ばれるコールバック</param>
		public void Load(string url, EventHandler<ImageCacheEventArgs> callback)
		{
			lock (table)
			{
				if (table.ContainsKey(url))
					throw new ArgumentException(url + "は既に登録されています");

				var data = new ImageCacheData(url, null, callback);
				table[url] = data;

				this.multiLimitter.Enqueue(url, _serverRestrictSettings.FromUrl(url));
			}
		}

		/// <summary>
		/// 画像キャッシュを指定したファイルにコピー
		/// </summary>
		/// <param name="info"></param>
		/// <param name="filePath"></param>
		public void Copy(CacheInfo info, string filePath)
		{
			String src = indexer.GetImageDataPath(info);
			File.Copy(src, filePath, true);
		}

		/// <summary>
		/// 指定したキャッシュを削除
		/// </summary>
		/// <param name="info"></param>
		public void Delete(CacheInfo info)
		{
			if (info == null) return;
			indexer.Remove(info);
		}

		/// <summary>
		/// 指定した画像の読み込みを中止
		/// </summary>
		/// <param name="url">中止する画像のURL</param>
		public void Abort(string url)
		{
			Thread thread = null;

			lock (table)
			{
				foreach (ImageCacheData data in table.Values)
				{
					if (data.url.Equals(url))
					{
						thread = data.thread;
						break;
					}
				}
			}

			if (thread != null)
			{
				thread.Abort();
				thread.Join();
			}
		}

		/// <summary>
		/// 画像の読み込みをすべて中止
		/// </summary>
		public void Abort()
		{
			ImageCacheData data;
			int c;

			lock (table)
				c = table.Keys.Count;

			for (int i = 0; i < c; i++)
			{
				lock (table)
					data = (ImageCacheData)table[i];

				if (data != null)
				{
					Thread thread = data.thread;
					thread.Abort();
					thread.Join();
				}
			}
		}


		/// <summary>
		/// 画像を読み込むスレッド
		/// </summary>
		private void Loading(object paramUrl) // ver2.5.101
		{
			ImageCacheData _data;
			lock (table)
				_data = (ImageCacheData)table[paramUrl];

			_data.thread = Thread.CurrentThread;

			WebHeaderCollection webHeaderColl = null;
			DateTime lastmod = DateTime.MinValue;
			byte[] buffer = null;
			Exception exception = null;
			Image image = null;
			ImageCacheStatus status = ImageCacheStatus.Unknown;
			CacheInfo info = indexer.Load(_data.url);
			HttpStatusCode statusCode = HttpStatusCode.Unused;

			try {

				if (info != null && info.Length > 0)
				{
					//lastmod = info.LastModified;
					image = indexer.LoadData(info);
					status = ImageCacheStatus.CacheExist;
				}
				else 
				{
					// バッファ内にデータをダウンロード
					string hash = String.Empty;
					statusCode = DownloadData(_data.url, lastmod, out buffer, out webHeaderColl);
					if (statusCode == HttpStatusCode.OK)
					{
						status = ImageCacheStatus.Downloaded;

						// バッファから画像データを作成
						using (MemoryStream mem = new MemoryStream(buffer))
							image = Image.FromStream(mem);

						// ハッシュ値を計算
						MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
						hash = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "").ToLower();
					}
					else
					{
						status = ImageCacheStatus.Error;
					}
					info = SaveCacheInfo(_data.url, webHeaderColl, buffer, hash);
				}
				info.FileName = indexer.GetImageDataPath(info);
			}
			catch (Exception ex)
			{
				// ThreadAbortException以外の例外が発生した場合は、
				// errorイベントを発生させる
				if ((ex is ThreadAbortException) == false)
				{
					status = ImageCacheStatus.Error;
					exception = ex;
				}
			}
			finally
			{
				lock (table)
					table.Remove(paramUrl);
				buffer = null;

				if (info == null)
					info = new CacheInfo(_data.url, 0, String.Empty, DateTime.MinValue);

				// 完了イベントを呼ぶ
				if (_data.callback != null)
				{
					_data.callback(this, new ImageCacheEventArgs(info) 
					{
						Image = image,
						Exception = exception,
						StatusCode = statusCode,
						Status = status,
					});
				}

			}
		}

		/// <summary>
		/// 指定したURLのデータをダウンロードしバッファに格納
		/// </summary>
		/// <param name="url"></param>
		/// <param name="lastmod"></param>
		/// <param name="data"></param>
		/// <param name="webHeaderColl"></param>
		/// <returns></returns>
		private HttpStatusCode DownloadData(string url, DateTime lastmod,
			out byte[] data, out WebHeaderCollection webHeaderColl)
		{
			HttpWebRequest req = null;
			HttpWebResponse res = null;
			HttpStatusCode code = HttpStatusCode.Unused;

			string referer = String.Empty;
			string dataUrl = url;

			try {

				imageViewUrlTool.Replace(ref dataUrl, out referer);

				req = (HttpWebRequest)WebRequest.Create(url);
				req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows 2000; DigExt)";
				//req.AllowAutoRedirect = false;
				//req.IfModifiedSince = lastmod;
				req.Referer = referer;
				req.Timeout = 30000;
				res = (HttpWebResponse)req.GetResponse();
				Stream stream = res.GetResponseStream();
				
				using (MemoryStream mem = new MemoryStream())
				{
					byte[] buffer = new byte[Int16.MaxValue];
					int read = -1;

					while (read != 0)
					{
						read = stream.Read(buffer, 0, buffer.Length);
						mem.Write(buffer, 0, read);
					}

					data = mem.ToArray();
				}

				webHeaderColl = res.Headers;
				code = res.StatusCode;

				// アプロダによってはエラーを返さず、リダイレクトによって404ページに飛ばすところもあるための処理
				if (res.ResponseUri.AbsoluteUri.Contains("/404.htm"))
					code = HttpStatusCode.NotFound;
			}
			catch (WebException ex)
			{
				webHeaderColl = null;
				data = null;

				HttpWebResponse _res = (HttpWebResponse)ex.Response;
				code = _res.StatusCode;
			}
			finally {
				if (res != null)
					res.Close();
			}

			return code;
		}

		/// <summary>
		/// インデックス情報を保存
		/// </summary>
		/// <param name="url"></param>
		/// <param name="headers"></param>
		/// <param name="buffer"></param>
		/// <param name="hash"></param>
		/// <param name="image"></param>
		private CacheInfo SaveCacheInfo(string url, WebHeaderCollection headers, byte[] buffer, string hash)
		{
			DateTime lastmod = DateTime.Now;
			try {
				string date = headers["Last-Modified"];
				if (date != null) lastmod = DateTime.Parse(date);
			}
			catch {}

			CacheInfo info = new CacheInfo(url, buffer.Length,
				hash, lastmod);

			indexer.Add(info, buffer);

			return info;
		}

		private class ImageCacheData
		{
			/// <summary>
			/// ダウンロードする画像URLを取得
			/// </summary>
			public readonly string url;

			/// <summary>
			/// 完了時に呼ばれるコールバック
			/// </summary>
			public readonly EventHandler<ImageCacheEventArgs> callback;

			/// <summary>
			/// スレッドのインスタンス
			/// </summary>
			public Thread thread;

			/// <summary>
			/// ImageCacheDataクラスのインスタンスを初期化
			/// </summary>
			/// <param name="url"></param>
			/// <param name="thread"></param>
			/// <param name="loaded"></param>
			/// <param name="error"></param>
			public ImageCacheData(string url, Thread thread, 
				EventHandler<ImageCacheEventArgs> callback)
			{
				this.url = url;
				this.thread = thread;
				this.callback = callback;
			}
		}
	}

	/// <summary>
	/// ImageCacheException
	/// </summary>
	public class ImageCacheException : ApplicationException
	{
		/// <summary>
		/// ImageCacheExceptionクラスのインスタンスを初期化
		/// </summary>
		public ImageCacheException() : base()
		{}

		/// <summary>
		/// ImageCacheExceptionクラスのインスタンスを初期化
		/// </summary>
		public ImageCacheException(string message) : base(message)
		{}

		/// <summary>
		/// ImageCacheExceptionクラスのインスタンスを初期化
		/// </summary>
		public ImageCacheException(string message, Exception innerException)
			: base(message, innerException)
		{}
	}
}
