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
	/// �摜���_�E�����[�h�����[�J���ɃL���b�V������N���X
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

			indexer = new ImageCacheIndexer(this);
			
			// �f�B���N�g�������݂��Ȃ���΍쐬
			if (! Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
		}

		/// <summary>
		/// �񓯊��Ŏw�肵��URL�̉摜�����[�h
		/// </summary>
		/// <param name="url">�ǂݍ��މ摜��URL</param>
		/// <param name="loaded">�摜���ǂݍ��܂ꂽ�Ƃ��ɌĂ΂��R�[���o�b�N</param>
		/// <param name="error">�G���[�����������Ƃ��ɌĂ΂��R�[���o�b�N</param>
		public void Load(string url,
			ImageCacheEventHandler loaded,
			ImageCacheErrorEventHandler error)
		{
			lock (table)
			{
				if (table.ContainsKey(url))
					throw new ArgumentException(url + "�͊��ɓo�^����Ă��܂�");

				Thread thread = new Thread(Loading);
				thread.IsBackground = true;
				thread.Priority = ThreadPriority.Lowest;
				thread.Name = url;

				table[url] = new ImageCacheData(url, thread, loaded, error);
				thread.Start();
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

		#region TESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST

		private void Test()
		{

		}

		#endregion


		/// <summary>
		/// �摜��ǂݍ��ރX���b�h
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
				// �L���b�V����������
				CacheInfo info = indexer.Load(_data.url); 

				if (info != null)
				{
					//lastmod = info.LastModified;
					image = indexer.LoadData(info);
				}
				// �o�b�t�@���Ƀf�[�^���_�E�����[�h
				else if (DownloadData(_data.url, lastmod, out buffer, out webHeaderColl))
				{
					// �o�b�t�@����摜�f�[�^���쐬
					using (MemoryStream mem = new MemoryStream(buffer))
						image = Image.FromStream(mem);

					// �n�b�V���l���v�Z
					MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
					string hash = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "").ToLower();

					// �C���f�b�N�X����ۑ�
					info = SaveCacheInfo(_data.url, webHeaderColl, buffer, hash);
				}

				info.FileName = indexer.GetImageDataPath(info);

				// �����C�x���g���Ă�
				if (_data.loaded != null)
					_data.loaded(this, new ImageCacheEventArgs(info, image));
			}
			catch (Exception ex)
			{
				// ThreadAbortException�ȊO�̗�O�����������ꍇ�́A
				// error�C�x���g�𔭐�������
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
		/// �w�肵��URL�̃f�[�^���_�E�����[�h���o�b�t�@�Ɋi�[
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
				// �t�@�C�����X�V����Ă��Ȃ���O�͖���
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
			/// �摜���ǂݍ��܂ꂽ�Ƃ��ɌĂ΂��R�[���o�b�N
			/// </summary>
			public readonly ImageCacheEventHandler loaded;

			/// <summary>
			///�G���[�����������Ƃ��ɌĂ΂��R�[���o�b�N
			/// </summary>
			public readonly ImageCacheErrorEventHandler error;

			/// <summary>
			/// �X���b�h�̃C���X�^���X
			/// </summary>
			public readonly Thread thread;

			/// <summary>
			/// ImageCacheData�N���X�̃C���X�^���X��������
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
*/