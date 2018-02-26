// ToolItemCollection.cs

namespace Twin.Tools
{
	using System;
	using System.Collections;
	using System.Xml;
	using System.IO;

	/// <summary>
	/// ToolItemCollection �̊T�v�̐����ł��B
	/// </summary>
	public class ToolItemCollection : CollectionBase
	{
		/// <summary>
		/// �w�肵���C���f�b�N�X�̗v�f���擾
		/// </summary>
		public ToolItem this[int index] {
			get {
				return (ToolItem)List[index];
			}
		}

		/// <summary>
		/// ToolItemCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public ToolItemCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// �t�@�C������ǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath)
		{
			Clear();

			if (File.Exists(filePath))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(filePath);

				XmlNodeList list = doc.SelectNodes("Tools/Item");
				foreach (XmlNode node in list)
				{
					ToolItem item = new ToolItem();
					item.Name = node.Attributes.GetNamedItem("Name").Value;
					item.Parameter = node.Attributes.GetNamedItem("Parameter").Value;

					XmlNode attr = node.Attributes.GetNamedItem("FileName");
					if (attr != null)
					{
						item.FileName = attr.Value;
					}

					Add(item);
				}
			}
		}

		/// <summary>
		/// �t�@�C���ɕۑ�
		/// </summary>
		/// <param name="filePath"></param>
		public void Save(string filePath)
		{
			XmlDocument doc = new XmlDocument();
			XmlNode root = doc.CreateElement("Tools");

			foreach (ToolItem item in List)
			{
				XmlAttribute attr1 = doc.CreateAttribute("Name");
				attr1.Value = item.Name;

				XmlAttribute attr2 = doc.CreateAttribute("Parameter");
				attr2.Value = item.Parameter;

				XmlAttribute attr3 = doc.CreateAttribute("FileName");
				attr3.Value = item.FileName;

				XmlNode node = doc.CreateElement("Item");
				node.Attributes.Append(attr1);
				node.Attributes.Append(attr2);
				node.Attributes.Append(attr3);

				root.AppendChild(node);
			}

			doc.AppendChild(root);
			doc.Save(filePath);
		}

		/// <summary>
		/// item���R���N�V�����̖����ɒǉ�
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(ToolItem item)
		{
			return List.Add(item);
		}

		/// <summary>
		/// index��item��}��
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, ToolItem item)
		{
			List.Insert(index, item);
		}

		/// <summary>
		/// item���R���N�V��������폜
		/// </summary>
		/// <param name="item"></param>
		public void Remove(ToolItem item)
		{
			List.Remove(item);
		}
	}
}
