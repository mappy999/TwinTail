// ImageCacheIndexer.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Text;
	using System.Drawing;
	using System.Runtime.CompilerServices;
	using System.Xml;

	/// <summary>
	/// ImageCacheIndexer の概要の説明です。
	/// </summary>
	public class ImageCacheIndexer
	{
		private string folderPath;
		private string indexPath;

		/// <summary>
		/// ImageCacheIndexerクラスのインスタンスを初期化
		/// </summary>
		/// <param name="imageCache"></param>
		internal ImageCacheIndexer(ImageCache imageCache)
		{
			this.folderPath = imageCache.BaseFolderPath;
			this.indexPath = Path.Combine(folderPath, "indices.txt");
		}
	
		/// <summary>
		/// url に指定された画像のキャッシュ情報を取得
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public CacheInfo Load(string url)
		{
			XmlDocument doc = new XmlDocument();
			CacheInfo info = null;

			if (File.Exists(indexPath))
			{
				doc.Load(indexPath);

				// キャッシュ情報を検索
				XmlNode root = doc.DocumentElement;
				XmlNode node = root.SelectSingleNode("Cache[@Url=\"" + url + "\"]");

				if (node != null)
				{
					XmlAttribute length = node.Attributes["Length"];
					XmlAttribute lastmod = node.Attributes["Lastmod"];
					XmlAttribute hash = node.Attributes["Hash"];
					XmlAttribute saved = node.Attributes["IsSaved"];

					info = new CacheInfo(url,
						Int32.Parse(length.Value),
						hash.Value,
						DateTime.Parse(lastmod.Value));

					if (saved != null)
					{
						bool b;
						if (Boolean.TryParse(saved.Value, out b))
							info.IsSaved = b;
					}
				}
			}
			return info;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void SaveCacheInfo(CacheInfo info)
		{
			Add(info, null);
		}

		/// <summary>
		/// info で指定されたキャッシュ情報、および buffer に指定された画像データを
		/// キャッシュデータに追加。
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Add(CacheInfo info, byte[] buffer)
		{
			XmlDocument doc = new XmlDocument();
			XmlNode root = null;

			if (File.Exists(indexPath))
			{
				doc.Load(indexPath);
				root = doc.DocumentElement;
			}
			else {
				root = doc.CreateElement("Indices");
				doc.AppendChild(root);
			}

			// 同じキャッシュ情報を検索
			XmlNode node = root.SelectSingleNode("Cache[@Url=\"" + info.Url + "\"]");

			if (node == null)
			{
				node = doc.CreateElement("Cache");
				root.AppendChild(node);

				node.Attributes.Append(doc.CreateAttribute("Url"));
				node.Attributes.Append(doc.CreateAttribute("Length"));
				node.Attributes.Append(doc.CreateAttribute("Hash"));
				node.Attributes.Append(doc.CreateAttribute("Lastmod"));
				node.Attributes.Append(doc.CreateAttribute("IsSaved"));
			}
			else if (node.Attributes["IsSaved"] == null) // 後から追加した属性なので、昔のバージョンでは null になる
			{
				node.Attributes.Append(doc.CreateAttribute("IsSaved"));
			}
			
			node.Attributes["Lastmod"].Value = info.LastModified.ToString("R");
			node.Attributes["Length"].Value = info.Length.ToString();
			node.Attributes["Hash"].Value = info.HashCode;
			node.Attributes["Url"].Value = info.Url;
			node.Attributes["IsSaved"].Value = info.IsSaved.ToString();

			// ファイルに保存
			XmlTextWriter w = new XmlTextWriter(indexPath, Encoding.GetEncoding("Shift_Jis"));
			w.Formatting = Formatting.Indented;

			doc.Save(w);
			w.Close();

			if (buffer != null)
			{
				// 画像をファイルに保存
				using (FileStream fs = new FileStream(GetImageDataPath(info), FileMode.Create))
					fs.Write(buffer, 0, buffer.Length);
			}
		}

		/// <summary>
		/// info で指定されたキャッシュ情報および画像データを削除
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Remove(CacheInfo info)
		{
			XmlDocument doc = new XmlDocument();
			XmlNode root = null;

			if (File.Exists(indexPath))
			{
				doc.Load(indexPath);
				root = doc.DocumentElement;

				// 同じキャッシュ情報を検索
				XmlNode node = root.SelectSingleNode("Cache[@Url=\"" + info.Url + "\"]");

				if (node != null)
				{
					// 見つかればノードを削除
					root.RemoveChild(node);

					// ファイルに保存
					XmlTextWriter w = new XmlTextWriter(indexPath, Encoding.GetEncoding("Shift_Jis"));
					w.Formatting = Formatting.Indented;

					doc.Save(w);
					w.Close();

					// 画像データも削除
					File.Delete(GetImageDataPath(info));
				}
			}
		}

		/// <summary>
		/// キャッシュインデックス、および全画像キャッシュを削除
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Clear()
		{
			// インデックスファイルを削除
			File.Delete(indexPath);

			foreach (string file in Directory.GetFiles(this.folderPath, "*.ich", SearchOption.TopDirectoryOnly))
			{
				try { File.Delete(file); }
				catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			}
		}

		/// <summary>
		/// info で指定したキャッシュの画像データを読み込む
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public Image LoadData(CacheInfo info)
		{
			string filePath = GetImageDataPath(info);
			Image image = null;
			
			if (File.Exists(filePath))
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Open))
					image = Image.FromStream(fs);
			}

			return image;
		}

		/// <summary>
		/// info で指定したキャッシュのデータが存在するパスを取得
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public string GetImageDataPath(CacheInfo info)
		{
			if (! Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			// URLのハッシュ値を16進数にしたもの + 拡張子.ich
			return Path.Combine(folderPath,
				String.Format("{0:x}.ich", info.Url.GetHashCode()));
		}
	}
}
