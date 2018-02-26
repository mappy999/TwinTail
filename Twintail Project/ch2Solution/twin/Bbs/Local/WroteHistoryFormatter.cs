// WroteHistoryFormatter.cs

namespace Twin.Text
{
	using System;
	using System.Text;
	using System.IO;
	using System.Xml;

	/// <summary>
	/// �������ݗ������C���f�b�N�X������
	/// </summary>
	public class WroteHistoryFormatter /*: IThreadListFormatter*/
	{
		/// <summary>
		/// WroteHistoryFormatter�N���X�̃C���X�^���X��������
		/// </summary>
		public WroteHistoryFormatter()
		{
		}

		/// <summary>
		/// �q�m�[�h���쐬
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public void AppendChild(XmlDocument doc, XmlElement root, WroteThreadHeader header)
		{
			XmlAttribute attr = doc.CreateAttribute("key");
			attr.Value = header.Key;

			XmlElement child = doc.CreateElement("item");
			child.Attributes.Append(attr);

			XmlElement subj = doc.CreateElement("subject");
			subj.AppendChild(doc.CreateCDataSection(header.Subject));

			XmlElement wrote = doc.CreateElement("wroteCount");
			wrote.InnerText = header.WroteCount.ToString();

			child.AppendChild(subj);
			child.AppendChild(wrote);
			root.AppendChild(child);
		}

		/// <summary>
		/// �w�肵���w�b�_�[�����������ĕ�����ɕϊ�
		/// </summary>
		public string Format(WroteThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			WroteThreadHeaderCollection temp = new WroteThreadHeaderCollection();
			temp.Add(header);

			return Format(temp);
		}

		/// <summary>
		/// �w�肵���w�b�_�[�R���N�V���������������ĕ�����ɕϊ�
		/// </summary>
		public string Format(WroteThreadHeaderCollection headerCollection)
		{
			if (headerCollection == null) {
				throw new ArgumentNullException("headerCollection");
			}

			/*	header�̊ȈՏ���xml�`���ɕϊ�
			 *	<indices>
			 *		<item key="">
			 *			<subject></subject>
			 *			<wroteCount></wroteCount>
			 *		</item>
			 * </indices>
			 */

			XmlDocument document = new XmlDocument();
			XmlElement root = document.CreateElement("indices");
			document.AppendChild(root);

			foreach (WroteThreadHeader header in headerCollection)
				AppendChild(document, root, header);

			MemoryStream memory = new MemoryStream();
			XmlTextWriter writer = new XmlTextWriter(memory, Encoding.Default); // UTF8�ɂ���ƂȂ����擪�ɃS�~���t���c

			writer.Formatting = Formatting.Indented;
			document.Save(writer);
			writer.Close();

			// ������ɕϊ�
			return Encoding.Default.GetString(memory.ToArray());
		}
	}
}
