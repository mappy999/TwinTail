// WroteHistoryFormatter.cs

namespace Twin.Text
{
	using System;
	using System.Text;
	using System.IO;
	using System.Xml;

	/// <summary>
	/// 書き込み履歴をインデックス化する
	/// </summary>
	public class WroteHistoryFormatter /*: IThreadListFormatter*/
	{
		/// <summary>
		/// WroteHistoryFormatterクラスのインスタンスを初期化
		/// </summary>
		public WroteHistoryFormatter()
		{
		}

		/// <summary>
		/// 子ノードを作成
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
		/// 指定したヘッダーを書式化して文字列に変換
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
		/// 指定したヘッダーコレクションを書式化して文字列に変換
		/// </summary>
		public string Format(WroteThreadHeaderCollection headerCollection)
		{
			if (headerCollection == null) {
				throw new ArgumentNullException("headerCollection");
			}

			/*	headerの簡易情報をxml形式に変換
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
			XmlTextWriter writer = new XmlTextWriter(memory, Encoding.Default); // UTF8にするとなぜか先頭にゴミが付く…

			writer.Formatting = Formatting.Indented;
			document.Save(writer);
			writer.Close();

			// 文字列に変換
			return Encoding.Default.GetString(memory.ToArray());
		}
	}
}
