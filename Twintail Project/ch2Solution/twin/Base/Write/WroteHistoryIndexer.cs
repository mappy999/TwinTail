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
	/// 書き込み履歴のインデックスを作成・管理する
	/// </summary>
	public class WroteHistoryIndexer
	{
		/// <summary>
		/// 指定した板のインデックス保存先パスを取得または設定
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
		/// 指定した板のインデックスを読み込む
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
		/// 指定した板のインデックスを作成
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

			// ヘッダ情報を書式化してファイルに保存
			WroteHistoryFormatter formatter = new WroteHistoryFormatter();
			FileUtility.Write(indicesPath, formatter.Format(items), false, Encoding.Default);
		}

		/// <summary>
		/// 指定したヘッダのインデックスを作成
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

				// 同じキーを持つノードを検索
				XmlNode node = document.SelectSingleNode("indices/item[@key=\"" + header.Key + "\"]");

				if (node != null)
				{
					// インデックスが既に存在したら情報を上書き
					node.SelectSingleNode("wroteCount").InnerText = header.WroteCount.ToString();
					overwrite = true;
				}
			}
			else {
				// ルートを作成
				document.AppendChild(
					document.CreateElement("indices"));
			}

			// 存在しなければ新しく作成
			if (!overwrite)
			{
				WroteHistoryFormatter formatter = new WroteHistoryFormatter();
				formatter.AppendChild(document, document.DocumentElement, header);
			}

			// ドキュメントを保存
			XmlTextWriter writer = new XmlTextWriter(indicesPath, Encoding.Default);
			writer.Formatting = Formatting.Indented;
			document.Save(writer);
			writer.Close();
		}

		/// <summary>
		/// 指定したスレッドのインデックスを削除
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
					// ドキュメントを保存
					XmlTextWriter writer = new XmlTextWriter(indicesPath, Encoding.Default);
					writer.Formatting = Formatting.Indented;
					document.Save(writer);
					writer.Close();
				}
			}
		}

		/// <summary>
		/// 書き込み履歴の再インデックス化を行う
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
