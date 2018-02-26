// BookmarkFormatter.cs
// #2.0

namespace Twin.Text
{
	using System;
	using System.Text;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;

	/// <summary>
	/// ���C�ɓ�������C���f�b�N�X������
	/// </summary>
	public class BookmarkFormatter : ThreadListFormatter
	{
		/// <summary>
		/// BookmarkFormatter�N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkFormatter()
		{
		}

		/// <summary>
		/// �q�m�[�h���쐬
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public void AppendChild(XmlDocument doc, XmlElement root, ThreadHeader header)
		{
			XmlAttribute attr = doc.CreateAttribute("key");
			attr.Value = header.Key;

			XmlElement child = doc.CreateElement("item");
			child.Attributes.Append(attr);

			XmlElement subj = doc.CreateElement("subject");
			subj.AppendChild(doc.CreateCDataSection(header.Subject));

			XmlElement resc = doc.CreateElement("resCount");
			resc.InnerText = header.ResCount.ToString();

			child.AppendChild(subj);
			child.AppendChild(resc);

			root.AppendChild(child);
		}

		/// <summary>
		/// �w�肵���w�b�_�[�����������ĕ�����ɕϊ�
		/// </summary>
		public override string Format(ThreadHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			List<ThreadHeader> temp = new List<ThreadHeader>();
			temp.Add(header);

			return Format(temp);
		}

		/// <summary>
		/// �w�肵���w�b�_�[�R���N�V���������������ĕ�����ɕϊ�
		/// </summary>
		public override string Format(List<ThreadHeader> headerList)
		{
			if (headerList == null)
			{
				throw new ArgumentNullException("headerList");
			}

			XmlDocument document = new XmlDocument();
			XmlElement root = document.CreateElement("Bookmarks");
			document.AppendChild(root);

			foreach (ThreadHeader header in headerList)
			{
				AppendChild(document, root, header);
			}

			MemoryStream memory = new MemoryStream();
			XmlTextWriter writer = new XmlTextWriter(memory, TwinDll.DefaultEncoding); // UTF8�ɂ���ƂȂ����擪�ɃS�~���t���c

			writer.Formatting = Formatting.Indented;
			document.Save(writer);
			writer.Close();

			// ������ɕϊ�
			return TwinDll.DefaultEncoding.GetString(memory.ToArray());
		}
	}
}

