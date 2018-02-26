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
	/// ImageCacheIndexer �̊T�v�̐����ł��B
	/// </summary>
	public class ImageCacheIndexer
	{
		private string folderPath;
		private string indexPath;

		/// <summary>
		/// ImageCacheIndexer�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="imageCache"></param>
		internal ImageCacheIndexer(ImageCache imageCache)
		{
			this.folderPath = imageCache.BaseFolderPath;
			this.indexPath = Path.Combine(folderPath, "indices.txt");
		}
	
		/// <summary>
		/// url �Ɏw�肳�ꂽ�摜�̃L���b�V�������擾
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public CacheInfo Load(string url)
		{
			XmlDocument doc = new XmlDocument();
			CacheInfo info = null;

			if (File.Exists(indexPath))
			{
				doc.Load(indexPath);

				// �L���b�V����������
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
		/// info �Ŏw�肳�ꂽ�L���b�V�����A����� buffer �Ɏw�肳�ꂽ�摜�f�[�^��
		/// �L���b�V���f�[�^�ɒǉ��B
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

			// �����L���b�V����������
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
			else if (node.Attributes["IsSaved"] == null) // �ォ��ǉ����������Ȃ̂ŁA�̂̃o�[�W�����ł� null �ɂȂ�
			{
				node.Attributes.Append(doc.CreateAttribute("IsSaved"));
			}
			
			node.Attributes["Lastmod"].Value = info.LastModified.ToString("R");
			node.Attributes["Length"].Value = info.Length.ToString();
			node.Attributes["Hash"].Value = info.HashCode;
			node.Attributes["Url"].Value = info.Url;
			node.Attributes["IsSaved"].Value = info.IsSaved.ToString();

			// �t�@�C���ɕۑ�
			XmlTextWriter w = new XmlTextWriter(indexPath, Encoding.GetEncoding("Shift_Jis"));
			w.Formatting = Formatting.Indented;

			doc.Save(w);
			w.Close();

			if (buffer != null)
			{
				// �摜���t�@�C���ɕۑ�
				using (FileStream fs = new FileStream(GetImageDataPath(info), FileMode.Create))
					fs.Write(buffer, 0, buffer.Length);
			}
		}

		/// <summary>
		/// info �Ŏw�肳�ꂽ�L���b�V����񂨂�щ摜�f�[�^���폜
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

				// �����L���b�V����������
				XmlNode node = root.SelectSingleNode("Cache[@Url=\"" + info.Url + "\"]");

				if (node != null)
				{
					// ������΃m�[�h���폜
					root.RemoveChild(node);

					// �t�@�C���ɕۑ�
					XmlTextWriter w = new XmlTextWriter(indexPath, Encoding.GetEncoding("Shift_Jis"));
					w.Formatting = Formatting.Indented;

					doc.Save(w);
					w.Close();

					// �摜�f�[�^���폜
					File.Delete(GetImageDataPath(info));
				}
			}
		}

		/// <summary>
		/// �L���b�V���C���f�b�N�X�A����ёS�摜�L���b�V�����폜
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Clear()
		{
			// �C���f�b�N�X�t�@�C�����폜
			File.Delete(indexPath);

			foreach (string file in Directory.GetFiles(this.folderPath, "*.ich", SearchOption.TopDirectoryOnly))
			{
				try { File.Delete(file); }
				catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			}
		}

		/// <summary>
		/// info �Ŏw�肵���L���b�V���̉摜�f�[�^��ǂݍ���
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
		/// info �Ŏw�肵���L���b�V���̃f�[�^�����݂���p�X���擾
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public string GetImageDataPath(CacheInfo info)
		{
			if (! Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			// URL�̃n�b�V���l��16�i���ɂ������� + �g���q.ich
			return Path.Combine(folderPath,
				String.Format("{0:x}.ich", info.Url.GetHashCode()));
		}
	}
}
