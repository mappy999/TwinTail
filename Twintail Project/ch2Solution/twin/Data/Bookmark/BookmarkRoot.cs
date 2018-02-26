// BookmarkRoot.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Windows.Forms;
	using System.Xml;
	using System.Text;
	using System.IO;
	using Twin.Text;

	/// <summary>
	/// ���C�ɓ���̍ŏ�w
	/// </summary>
	public class BookmarkRoot : BookmarkFolder
	{
		/// <summary>
		/// BookmarkRoot�N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkRoot() : this("���C�ɓ���")
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// BookmarkRoot�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="name"></param>
		public BookmarkRoot(string name) : base(name, -1)
		{
			Expanded = true;
		}

		/// <summary>
		/// ���̃��\�b�h�̓T�|�[�g���Ă��܂���
		/// </summary>
		/// <returns></returns>
		public override BookmarkEntry Clone()
		{
			throw new NotSupportedException("���[�g�t�H���_�𕡐����邱�Ƃ͏o���܂���");
		}

		/// <summary>
		/// ���C�ɓ������xml�`���Ńt�@�C���ɕۑ�
		/// </summary>
		/// <param name="filePath"></param>
		public virtual void Save(string filePath)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}

			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("Bookmarks");

			// �ċN�ł��ׂĂ̍��ڂ�ǉ�
			AppendRecursive(doc, root, this);
			doc.AppendChild(root);

			// �ۑ�
			XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8);
			writer.Formatting = Formatting.Indented;

			doc.Save(writer);
			writer.Close();
		}

		/// <summary>
		/// �ċN�����𗘗p���ăT�u�t�H���_�Ƃ��C�ɓ���̗v�f��element�ɒǉ�
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="parent"></param>
		/// <param name="folders"></param>
		private void AppendRecursive(XmlDocument doc, XmlElement parent, BookmarkFolder folder)
		{
			// �t�H���_���̑������쐬
			XmlAttribute attr1 = doc.CreateAttribute("Name");
			attr1.Value = folder.Name;

			// ����ID�������쐬
			XmlAttribute attr2 = doc.CreateAttribute("ID");
			attr2.Value = folder.Id.ToString();

			// �W�J����Ă��邩�ǂ����̑������쐬
			XmlAttribute attr3 = doc.CreateAttribute("Expanded");
			attr3.Value = folder.Expanded.ToString();

			// �T�u�t�H���_�R���N�V�����m�[�h���쐬
			XmlElement children = doc.CreateElement("Children");

			foreach (BookmarkEntry entry in folder.Children)
			{
				// ���C�ɓ���̏ꍇ
				if (entry.IsLeaf)
				{
					BookmarkThread item = (BookmarkThread)entry;

					// URL�������쐬
					XmlAttribute url = doc.CreateAttribute("URL");
					url.Value = item.HeaderInfo.Url;

					//XmlAttribute id = doc.CreateAttribute("ID");
					//id.Value = entry.Id.ToString();

					// ���C�ɓ���m�[�h���쐬
					XmlElement node = doc.CreateElement("Item");
					node.Attributes.Append(url);
					//node.Attributes.Append(id);

					// beta18
					//node.InnerText = item.HeaderInfo.Subject;
					node.InnerText = item.Name;

					// ���C�ɓ���R���N�V�����m�[�h�ɒǉ�
					children.AppendChild(node);
				}
				// �t�H���_�ł���ΐ���
				else {
					AppendRecursive(doc, children, (BookmarkFolder)entry);
				}
			}

			// �Ō�Ƀt�H���_�m�[�h�̍쐬
			XmlElement folderNode = doc.CreateElement("Folder");
			folderNode.Attributes.Append(attr1);
			folderNode.Attributes.Append(attr2);
			folderNode.Attributes.Append(attr3);
			folderNode.AppendChild(children);

			// �e�m�[�h�ɒǉ�
			parent.AppendChild(folderNode);
		}

		/// <summary>
		/// ���C�ɓ���t�@�C����ǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		public virtual void Load(string filePath)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}

			// �q�����ׂč폜���Ă���ǂݍ���
			Children.Clear();

			if (File.Exists(filePath))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(filePath);

				// �ċN�𗘗p���ăm�[�h������
				XmlNode root = doc.DocumentElement.FirstChild;
				LoadRecursive(root, this);
			}
		}

		private void LoadRecursive(XmlNode node, BookmarkFolder folder)
		{
			// �t�H���_���쐬
			folder.Name = node.Attributes["Name"].Value;
			folder.Expanded = Boolean.Parse(node.Attributes["Expanded"].Value);

			XmlAttribute id = node.Attributes["ID"];
			if (id != null)
				BookmarkEntry.SetEntryId(folder, Int32.Parse(id.Value));

			// �q�m�[�h������
			foreach (XmlNode subNode in node.SelectNodes("Children/Folder"))
			{
				BookmarkFolder subFolder = new BookmarkFolder();
				folder.Children.Add(subFolder);
				LoadRecursive(subNode, subFolder);
			}

			// ���C�ɓ���R���N�V����������
			foreach (XmlNode child in node.SelectNodes("Children/Item"))
			{
				string url = child.Attributes["URL"].Value;
				ThreadHeader header = URLParser.ParseThread(url);
				
				if (header != null)
				{
					//XmlAttribute idattr = child.Attributes["ID"];
					BookmarkEntry entry = null;

					header.Subject = child.InnerText;

					//if (idattr != null)
					//	entry = new BookmarkThread(header, Int32.Parse(idattr.Value));
					//else 
						entry = new BookmarkThread(header);

					folder.Children.Add(entry);
				}
			}
		}

		/// <summary>
		/// �ċN�����Ŏw�肵�����C�ɓ��������
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public BookmarkThread Search(ThreadHeader header)
		{
			return Search(this, header);
		}

		/// <summary>
		/// header������
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="header"></param>
		/// <returns></returns>
		private BookmarkThread Search(BookmarkFolder folder, ThreadHeader header)
		{
			foreach (BookmarkEntry entry in folder.Children)
			{
				if (entry.IsLeaf)
				{
					BookmarkThread item = (BookmarkThread)entry;
					bool equal = item.HeaderInfo.Equals(header);

					if (equal)
						return item;
				}
				else {
					BookmarkThread r = Search((BookmarkFolder)entry, header);
					if (r != null) return r;
				}
			}
			return null;
		}

		/// <summary>
		/// ���C�ɓ����item���܂܂�Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool Contains(ThreadHeader header)
		{
			return Search(header) != null;
		}

		/// <summary>
		/// ���C�ɓ��肩��item���폜
		/// </summary>
		/// <param name="header"></param>
		public void Remove(ThreadHeader header)
		{
			BookmarkEntry item = Search(header);
			if (item != null) item.Remove();
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Name;
		}
	}
}
