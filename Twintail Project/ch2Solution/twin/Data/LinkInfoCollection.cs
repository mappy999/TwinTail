// LinkInfoCollection.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Soap;
	using System.Text;
	using System.IO;
	using System.Xml;
	using CSharpSamples;
	using System.Text.RegularExpressions;

	/// <summary>
	/// LinkInfo�N���X���R���N�V�����Ǘ�
	/// </summary>
	[Serializable]
	public class LinkInfoCollection : CollectionBase, ISerializable
	{
		private string filePath;

		/// <summary>
		/// �w�肵���C���f�b�N�X�̃A�C�e�����擾
		/// </summary>
		public LinkInfo this[int index] {
			get {
				return (LinkInfo)List[index];
			}
		}

		/// <summary>
		/// LinkInfoCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public LinkInfoCollection() 
		{
			filePath = String.Empty;
		}

		/// <summary>
		/// LinkInfoCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public LinkInfoCollection(string fileName) 
		{
			LoadFromXml(fileName);
		}

		public LinkInfoCollection(SerializationInfo info, StreamingContext context)
		{
			ArrayList arrayList = (ArrayList)info.GetValue("AddressList", typeof(ArrayList));
			foreach (LinkInfo link in arrayList)
			{
				if (link != null)
					Add(link);
			}
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("AddressList", InnerList);
		}

		/// <summary>
		/// �R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="item">�ǉ�����LinkInfo�N���X</param>
		/// <returns>�ǉ����ꂽ�ʒu</returns>
		public int Add(LinkInfo item)
		{
			return List.Add(item);
		}

		/// <summary>
		/// �R���N�V������ǉ�
		/// </summary>
		/// <param name="items">�ǉ�����LinkInfoCollection�N���X</param>
		/// <returns>�ǉ����ꂽ�ʒu</returns>
		public void AddRange(LinkInfoCollection items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ɑ}��
		/// </summary>
		/// <param name="index">�}������C���f�b�N�X</param>
		/// <param name="item">�}������LinkInfo�N���X</param>
		public void Insert(int index, LinkInfo item)
		{
			List.Insert(index, item);
		}

		/// <summary>
		/// �w�肵��LinkInfo���폜
		/// </summary>
		/// <param name="item">�폜����LinkInfo</param>
		public void Remove(LinkInfo item)
		{
			List.Remove(item);
		}

		/// <summary>
		/// �C���f�b�N�X���擾
		/// </summary>
		/// <param name="item">��������A�C�e��</param>
		/// <returns>���݂���΂��̃C���f�b�N�X (������Ȃ����-1)</returns>
		public int IndexOf(LinkInfo item)
		{
			return List.IndexOf(item);
		}

		/// <summary>
		/// �w�肵��URI�Ƀ����N�����擾
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public LinkInfo IndexOf(string uri)
		{
			uri = uri.ToLower();

			foreach (LinkInfo link in List)
			{
				if (uri.IndexOf(link.Uri.ToLower()) >= 0)
					return link;
			}
			return null;
		}

		/// <summary>
		/// �����N�R���N�V������ǂݍ��� (���o�[�W�����̃t�@�C���`��)
		/// </summary>
		/// <param name="fileName"></param>
		public void LoadFromSoap(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}

			Clear();
			filePath = fileName;

			if (File.Exists(fileName))
			{
				
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
				{
					SoapFormatter soap = new SoapFormatter();
					LinkInfo[] linkInfos = soap.Deserialize(fileStream) as LinkInfo[];
					if (linkInfos != null)
					{
						foreach (LinkInfo linkInfo in linkInfos)
							Add(linkInfo);
					}
				}
			}
		}

		public void LoadFromXml(string fileName)
		{
			Clear();
			filePath = fileName;

			if (File.Exists(fileName))
			{
				XmlTextReader xmlIn = new XmlTextReader(fileName);
				
				try
				{
					xmlIn.MoveToContent();

					if (xmlIn.Name == "SOAP-ENV:Envelope")
					{
						// �����̃t�@�C���̏ꍇ
						xmlIn.Close();
						xmlIn = null;

						LoadFromSoap(fileName);
					}
					else if (xmlIn.Name == "LinkInfoCollection")
					{
						string version = xmlIn.GetAttribute("Version");

						if (version != "1.0")
							throw new ArgumentException();

						ReadXml(xmlIn);
					}

				}
				finally
				{
					if (xmlIn != null)
						xmlIn.Close();
				}
			}
		}

		private void ReadXml(XmlTextReader xmlIn)
		{
			while (xmlIn.Read())
			{
				if (xmlIn.NodeType != XmlNodeType.Element)
					continue;

				if (xmlIn.Name == "Item")
				{
					LinkInfo item = new LinkInfo();
					item.Uri = xmlIn.GetAttribute("Uri");
					item.Text = xmlIn.ReadString();

					List.Add(item);
				}
			}
		}

		/// <summary>
		/// �����N�R���N�V������ۑ�
		/// </summary>
		/// <param name="fileName"></param>
		public void SaveToXml(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}

			XmlTextWriter xmlOut = new XmlTextWriter(fileName, Encoding.GetEncoding("shift_jis"));
			xmlOut.Formatting = Formatting.Indented;

			filePath = fileName;

			try
			{
				xmlOut.WriteStartDocument();

				xmlOut.WriteStartElement("LinkInfoCollection");
				xmlOut.WriteAttributeString("Version", "1.0");

				foreach (LinkInfo item in this.List)
				{
					xmlOut.WriteStartElement("Item");
					xmlOut.WriteAttributeString("Uri", item.Uri);

					xmlOut.WriteString(item.Text);

					xmlOut.WriteEndElement();
				}

				xmlOut.WriteEndElement();
				xmlOut.WriteEndDocument();
			}
			finally
			{
				xmlOut.Close();
			}
	
			//using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			//{
			//    SoapFormatter soap = new SoapFormatter();
			//    soap.Serialize(fileStream, this.InnerList.ToArray(typeof(LinkInfo)));
			//}
			//			ConfigSerializer.Serialize(fileName, typeof(LinkInfoCollection), this);

		}
	}
}
