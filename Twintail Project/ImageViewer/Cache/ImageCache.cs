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
	/// �摜���_�E�����[�h�����[�J���ɃL���b�V������N���X
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
		/// �L���b�V���̕ۑ��t�H���_�p�X���擾
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
		/// ImageCache�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="folderPath">�摜�̃L���b�V���ۑ��t�H���_</param>
		public ImageCache(string folderPath)
		{
			this.folderPath = folderPath;
			this.table = new Hashtable();
			this.multiLimitter = new MultiLimitter(Loading);
			this.indexer = new ImageCacheIndexer(this);
			
			// �f�B���N�g�������݂��Ȃ���΍쐬
			if (! Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
		}

		/// <summary>
		/// �񓯊��Ŏw�肵��URL�̉摜�����[�h
		/// </summary>
		/// <param name="url">�ǂݍ��މ摜��URL</param>
		/// <param name="callback">�����������������ɌĂ΂��R�[���o�b�N</param>
		public void Load(string url, EventHandler<ImageCacheEventArgs> callback)
		{
			lock (table)
			{
				if (table.ContainsKey(url))
					throw new ArgumentException(url + "�͊��ɓo�^����Ă��܂�");

				var data = new ImageCacheData(url, null, callback);
				table[url] = data;

				this.multiLimitter.Enqueue(url, _serverRestrictSettings.FromUrl(url));
			}
		}

		/// <summary>
		/// �摜�L���b�V�����w�肵���t�@�C���ɃR�s�[
		/// </summary>
		/// <param name="info"></param>
		/// <param name="filePath"></param>
		public void Copy(CacheInfo info, string filePath)
		{
			String src = indexer.GetImageDataPath(info);
			File.Copy(src, filePath, true);
		}

		/// <summary>
		/// �w�肵���L���b�V�����폜
		/// </summary>
		/// <param name="info"></param>
		public void Delete(CacheInfo info)
		{
			if (info == null) return;
			indexer.Remove(info);
		}

		/// <summary>
		/// �w�肵���摜�̓ǂݍ��݂𒆎~
		/// </summary>
		/// <param name="url">���~����摜��URL</param>
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
		/// �摜�̓ǂݍ��݂����ׂĒ��~
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
		/// �摜��ǂݍ��ރX���b�h
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
					// �o�b�t�@���Ƀf�[�^���_�E�����[�h
					string hash = String.Empty;
					statusCode = DownloadData(_data.url, lastmod, out buffer, out webHeaderColl);
					if (statusCode == HttpStatusCode.OK)
					{
						status = ImageCacheStatus.Downloaded;

						// �o�b�t�@����摜�f�[�^���쐬
						using (MemoryStream mem = new MemoryStream(buffer))
							image = Image.FromStream(mem);

						// �n�b�V���l���v�Z
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
				// ThreadAbortException�ȊO�̗�O�����������ꍇ�́A
				// error�C�x���g�𔭐�������
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

				// �����C�x���g���Ă�
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
		/// �w�肵��URL�̃f�[�^���_�E�����[�h���o�b�t�@�Ɋi�[
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

				// �A�v���_�ɂ���Ă̓G���[��Ԃ����A���_�C���N�g�ɂ����404�y�[�W�ɔ�΂��Ƃ�������邽�߂̏���
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
		/// �C���f�b�N�X����ۑ�
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
			/// �_�E�����[�h����摜URL���擾
			/// </summary>
			public readonly string url;

			/// <summary>
			/// �������ɌĂ΂��R�[���o�b�N
			/// </summary>
			public readonly EventHandler<ImageCacheEventArgs> callback;

			/// <summary>
			/// �X���b�h�̃C���X�^���X
			/// </summary>
			public Thread thread;

			/// <summary>
			/// ImageCacheData�N���X�̃C���X�^���X��������
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
		/// ImageCacheException�N���X�̃C���X�^���X��������
		/// </summary>
		public ImageCacheException() : base()
		{}

		/// <summary>
		/// ImageCacheException�N���X�̃C���X�^���X��������
		/// </summary>
		public ImageCacheException(string message) : base(message)
		{}

		/// <summary>
		/// ImageCacheException�N���X�̃C���X�^���X��������
		/// </summary>
		public ImageCacheException(string message, Exception innerException)
			: base(message, innerException)
		{}
	}
}
