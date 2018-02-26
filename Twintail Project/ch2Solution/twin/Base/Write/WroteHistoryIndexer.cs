// WroteHistoryIndexer.cs

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml;
	using Twin.Util;
	using Twin.Text;

	/// <summary>
	/// �������ݗ����̃C���f�b�N�X���쐬�E�Ǘ�����
	/// </summary>
	public class WroteHistoryIndexer
	{
		/// <summary>
		/// �w�肵���̃C���f�b�N�X�ۑ���p�X���擾�܂��͐ݒ�
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		/// <returns></returns>
		public static string GetIndicesPath(Cache cache, BoardInfo board)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			string folderPath = cache.GetFolderPath(board, false);
			string listPath = Path.Combine(folderPath, "indices-komi.txt");

			return listPath;
		}

		/// <summary>
		/// �w�肵���̃C���f�b�N�X��ǂݍ���
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		/// <returns></returns>
		public static WroteThreadHeaderCollection Read(Cache cache, BoardInfo board)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			WroteThreadHeaderCollection items = new WroteThreadHeaderCollection();
			string indicesPath = GetIndicesPath(cache, board);

			if (File.Exists(indicesPath))
			{
				XmlDocument document = new XmlDocument();
				document.Load(indicesPath);

				XmlElement root = document.DocumentElement;
				XmlNodeList children = root.ChildNodes;
				
				foreach (XmlNode node in children)
				{
					WroteThreadHeader header = new WroteThreadHeader();
					header.Key = node.Attributes.GetNamedItem("key").Value;
					header.Subject = node.SelectSingleNode("subject").InnerText;
					header.WroteCount = Int32.Parse(node.SelectSingleNode("wroteCount").InnerText);
					header.BoardInfo = board;
					items.Add(header);
				}
			}

			return items;
		}

		/// <summary>
		/// �w�肵���̃C���f�b�N�X���쐬
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		/// <param name="items"></param>
		public static void Write(Cache cache, BoardInfo board, WroteThreadHeaderCollection items)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (items == null) {
				throw new ArgumentNullException("items");
			}

			string indicesPath =
				GetIndicesPath(cache, board);

			// �w�b�_�������������ăt�@�C���ɕۑ�
			WroteHistoryFormatter formatter = new WroteHistoryFormatter();
			FileUtility.Write(indicesPath, formatter.Format(items), false, Encoding.Default);
		}

		/// <summary>
		/// �w�肵���w�b�_�̃C���f�b�N�X���쐬
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header"></param>
		public static void Write(Cache cache, WroteThreadHeader header)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			XmlDocument document = new XmlDocument();
			string indicesPath = GetIndicesPath(cache, header.BoardInfo);
			bool overwrite = false;

			if (File.Exists(indicesPath))
			{
				document.Load(indicesPath);

				// �����L�[�����m�[�h������
				XmlNode node = document.SelectSingleNode("indices/item[@key=\"" + header.Key + "\"]");

				if (node != null)
				{
					// �C���f�b�N�X�����ɑ��݂���������㏑��
					node.SelectSingleNode("wroteCount").InnerText = header.WroteCount.ToString();
					overwrite = true;
				}
			}
			else {
				// ���[�g���쐬
				document.AppendChild(
					document.CreateElement("indices"));
			}

			// ���݂��Ȃ���ΐV�����쐬
			if (!overwrite)
			{
				WroteHistoryFormatter formatter = new WroteHistoryFormatter();
				formatter.AppendChild(document, document.DocumentElement, header);
			}

			// �h�L�������g��ۑ�
			XmlTextWriter writer = new XmlTextWriter(indicesPath, Encoding.Default);
			writer.Formatting = Formatting.Indented;
			document.Save(writer);
			writer.Close();
		}

		/// <summary>
		/// �w�肵���X���b�h�̃C���f�b�N�X���폜
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header"></param>
		public static void Remove(Cache cache, WroteThreadHeader header)
		{
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			XmlDocument document = new XmlDocument();
			string indicesPath = GetIndicesPath(cache, header.BoardInfo);

			if (File.Exists(indicesPath))
			{
				document.Load(indicesPath);

				XmlNode node = document.SelectSingleNode("indices/item[@key=\"" + header.Key + "\"]");

				if (node != null)
					document.DocumentElement.RemoveChild(node);

				if (document.DocumentElement.ChildNodes.Count == 0)
				{
					File.Delete(indicesPath);
				}
				else {
					// �h�L�������g��ۑ�
					XmlTextWriter writer = new XmlTextWriter(indicesPath, Encoding.Default);
					writer.Formatting = Formatting.Indented;
					document.Save(writer);
					writer.Close();
				}
			}
		}

		/// <summary>
		/// �������ݗ����̍ăC���f�b�N�X�����s��
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		public static void Indexing(Cache cache, BoardInfo board)
		{
			string folderPath = cache.GetFolderPath(board);
			string[] komidx = Directory.GetFiles(folderPath, "*.komi");

			WroteThreadHeaderCollection items =
				new WroteThreadHeaderCollection();

			foreach (string indexPath in komidx)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(indexPath);

				XmlNode root = doc.DocumentElement;

				WroteThreadHeader header = new WroteThreadHeader();
				header.BoardInfo = board;
				header.Key = root.SelectSingleNode("thread[@key]").Attributes.GetNamedItem("key").Value;
				header.Subject = root.SelectSingleNode("thread/subject").InnerText;
				header.WroteCount = root.SelectNodes("thread/resCollection/res").Count;

				items.Add(header);
				doc = null;
			}

			Write(cache, board, items);
		}
	}
}
