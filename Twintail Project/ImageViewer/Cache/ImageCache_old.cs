// ImageCache.cs
/*
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

			indexer = new ImageCacheIndexer(this);
			
			// ディレクトリが存在しなければ作成
			if (! Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
		}

		/// <summary>
		/// 非同期で指定したURLの画像をロード
		/// </summary>
		/// <param name="url">読み込む画像のURL</param>
		/// <param name="loaded">画像が読み込まれたときに呼ばれるコールバック</param>
		/// <param name="error">エラーが発生したときに呼ばれるコールバック</param>
		public void Load(string url,
			ImageCacheEventHandler loaded,
			ImageCacheErrorEventHandler error)
		{
			lock (table)
			{
				if (table.ContainsKey(url))
					throw new ArgumentException(url + "は既に登録されています");

				Thread thread = new Thread(Loading);
				thread.IsBackground = true;
				thread.Priority = ThreadPriority.Lowest;
				thread.Name = url;

				table[url] = new ImageCacheData(url, thread, loaded, error);
				thread.Start();
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

		#region TESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST

		private void Test()
		{

		}

		#endregion


		/// <summary>
		/// 画像を読み込むスレッド
		/// </summary>
		private void Loading()
		{
			ImageCacheData _data;
			
			lock (table)
				_data = (ImageCacheData)table[Thread.CurrentThread.Name];

			WebHeaderCollection webHeaderColl = null;
			DateTime lastmod = DateTime.MinValue;
			byte[] buffer = null;

			Image image = null;

			try {
				// キャッシュ内を検索
				CacheInfo info = indexer.Load(_data.url); 

				if (info != null)
				{
					//lastmod = info.LastModified;
					image = indexer.LoadData(info);
				}
				// バッファ内にデータをダウンロード
				else if (DownloadData(_data.url, lastmod, out buffer, out webHeaderColl))
				{
					// バッファから画像データを作成
					using (MemoryStream mem = new MemoryStream(buffer))
						image = Image.FromStream(mem);

					// ハッシュ値を計算
					MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
					string hash = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "").ToLower();

					// インデックス情報を保存
					info = SaveCacheInfo(_data.url, webHeaderColl, buffer, hash);
				}

				info.FileName = indexer.GetImageDataPath(info);

				// 完了イベントを呼ぶ
				if (_data.loaded != null)
					_data.loaded(this, new ImageCacheEventArgs(info, image));
			}
			catch (Exception ex)
			{
				// ThreadAbortException以外の例外が発生した場合は、
				// errorイベントを発生させる
				if ((ex is ThreadAbortException) == false)
				{
					Debug.WriteLine(ex.ToString());

					if (_data.error != null)
						_data.error(this, new ImageCacheErrorEventArgs(_data.url, ex));
				}
			}
			finally {
				lock (table)
					table.Remove(Thread.CurrentThread.Name);

				buffer = null;
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
		private bool DownloadData(string url, DateTime lastmod,
			out byte[] data, out WebHeaderCollection webHeaderColl)
		{
			HttpWebRequest req = null;
			HttpWebResponse res = null;
			bool result = false;

			string referer = String.Empty;
			string dataUrl = url;

			try {

				imageViewUrlTool.Replace(ref dataUrl, out referer);

				req = (HttpWebRequest)WebRequest.Create(url);
				req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows 2000; DigExt)";
				//req.IfModifiedSince = lastmod;
				req.Referer = referer;

				res = (HttpWebResponse)req.GetResponse();
				Stream stream = res.GetResponseStream();
				
				using (MemoryStream mem = new MemoryStream())
				{
					byte[] buffer = new byte[4096];
					int read = -1;

					while (read != 0)
					{
						read = stream.Read(buffer, 0, buffer.Length);
						mem.Write(buffer, 0, read);
					}

					data = mem.ToArray();
				}

				webHeaderColl = res.Headers;
				result = true;
			}
			catch (WebException ex)
			{
				webHeaderColl = null;
				data = null;

				HttpWebResponse _res = (HttpWebResponse)ex.Response;
				// ファイルが更新されていない例外は無視
				if (_res.StatusCode != HttpStatusCode.NotModified)
					throw;
			}
			finally {
				if (res != null)
					res.Close();
			}

			return result;
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
			/// 画像が読み込まれたときに呼ばれるコールバック
			/// </summary>
			public readonly ImageCacheEventHandler loaded;

			/// <summary>
			///エラーが発生したときに呼ばれるコールバック
			/// </summary>
			public readonly ImageCacheErrorEventHandler error;

			/// <summary>
			/// スレッドのインスタンス
			/// </summary>
			public readonly Thread thread;

			/// <summary>
			/// ImageCacheDataクラスのインスタンスを初期化
			/// </summary>
			/// <param name="url"></param>
			/// <param name="thread"></param>
			/// <param name="loaded"></param>
			/// <param name="error"></param>
			public ImageCacheData(string url, Thread thread, 
				ImageCacheEventHandler loaded,
				ImageCacheErrorEventHandler error)
			{
				this.url = url;
				this.thread = thread;
				this.loaded = loaded;
				this.error = error;
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
*/