// WroteHistory.cs

namespace Twin
{
	using System;
	using System.Text;
	using System.IO;
	using System.Xml;
	using System.Xml.XPath;
	using System.Collections;

	/// <summary>
	/// 書き込み履歴を表す
	/// </summary>
	public class WroteHistory
	{
		private Cache cache;

		/// <summary>
		/// WroteHistoryクラスのインスタンスを初期化
		/// </summary>
		/// <param name="cache">履歴の基本キャッシュ情報を表す</param>
		public WroteHistory(Cache cache)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			if (cache == null)
				throw new ArgumentNullException("cache");

			this.cache = cache;
		}

		/// <summary>
		/// 指定したスレッドの履歴を読み込む
		/// </summary>
		/// <param name="board"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public WroteThread Load(BoardInfo board, string key)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (key == null) {
				throw new ArgumentNullException("key");
			}

			string path = GetHistoryPath(board, key);
			return Load(path);
		}

		/// <summary>
		/// 指定したスレッドの履歴を読み込む
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public WroteThread Load(string filePath)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}

			XmlDocument document = null;
			WroteThread wroteThread = null;

			try {
				if (File.Exists(filePath))
				{
					document = new XmlDocument();
					document.Load(filePath);
					wroteThread = new WroteThread();				
					
					XmlNode parent = document.DocumentElement.FirstChild;
					wroteThread.Key = parent.Attributes.GetNamedItem("key").Value;

					XmlNode subj = document.SelectSingleNode("kakikomi/thread/subject");
					wroteThread.Subject = subj.InnerText;

					XmlNode uri = document.SelectSingleNode("kakikomi/thread/url");
					wroteThread.Uri = new Uri(uri.InnerText);

					XmlNodeList resList = document.SelectNodes("kakikomi/thread/resCollection/res");
					foreach (XmlNode resNode in resList)
					{
						WroteRes item = new WroteRes();
						XmlNode date = resNode.Attributes.GetNamedItem("date");
						item.Date = DateTime.Parse(date.Value);
						item.From = resNode.SelectSingleNode("from").InnerText;
						item.Email = resNode.SelectSingleNode("email").InnerText;
						item.Message = resNode.SelectSingleNode("message").InnerText;
						wroteThread.ResItems.Add(item);
					}
				}
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}

			return wroteThread;
		}

		/// <summary>
		/// 指定したスレッドの履歴情報を保存
		/// </summary>
		/// <param name="header"></param>
		/// <returns>履歴数を返す</returns>
		public int Save(ThreadHeader header, WroteRes res)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			if (res == null) {
				throw new ArgumentNullException("res");
			}

			// 書き込み履歴のファイルパス
			string filePath = GetHistoryPath(header.BoardInfo, header.Key);
			int wroteCount = 0;

			try {
				XmlDocument document = new XmlDocument();
				XmlNode root = null, parent = null;

				if (File.Exists(filePath))
				{
					// 既に履歴が存在すればファイルから読み込む
					document.Load(filePath);
					root = document.DocumentElement;
					parent = document.SelectSingleNode("kakikomi/thread");
				}
				else {
					// ルート要素を作成
					root = document.CreateElement("kakikomi");					
					document.AppendChild(root);

					XmlAttribute attr = document.CreateAttribute("key");
					attr.Value = header.Key.ToString();

					parent = document.CreateElement("thread");
					parent.Attributes.Append(attr);
					root.AppendChild(parent);

					// スレッドのURL
					XmlElement urlElement = document.CreateElement("url");
					urlElement.InnerText = header.Url;
					parent.AppendChild(urlElement);

					// スレッド名
					XmlElement subjElement = document.CreateElement("subject");
					subjElement.AppendChild(document.CreateCDataSection(header.Subject));
					parent.AppendChild(subjElement);
				}

				// <resCollection>
				XmlNode resCollect = document.SelectSingleNode("kakikomi/thread/resCollection");
				if (resCollect == null) {
					resCollect = document.CreateElement("resCollection");
					parent.AppendChild(resCollect);
				}

				// 追加されるレス要素
				XmlElement resChild = document.CreateElement("res");

				// 日付
				XmlAttribute date = document.CreateAttribute("date");
				date.Value = res.Date.ToString();
				resChild.Attributes.Append(date);

				// 投稿者名
				XmlElement from = document.CreateElement("from");
				from.AppendChild(document.CreateCDataSection(res.From));
				resChild.AppendChild(from);

				// メールアドレス
				XmlElement email = document.CreateElement("email");
				email.AppendChild(document.CreateCDataSection(res.Email));
				resChild.AppendChild(email);

				// メッセージ本分
				XmlElement message = document.CreateElement("message");
				message.AppendChild(document.CreateCDataSection(res.Message));
				resChild.AppendChild(message);

				// レス要素を追加
				resCollect.AppendChild(resChild);

				// 履歴数を取得
				wroteCount = resCollect.ChildNodes.Count;

				// ドキュメントを保存
				XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 4;
				document.Save(writer);
				writer.Close();
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}

			return wroteCount;
		}

		/// <summary>
		/// 指定した板のスレッド履歴をすべて取得
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public WroteThread[] Load(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			string folder = cache.GetFolderPath(board);
			ArrayList list = new ArrayList();

			if (Directory.Exists(folder))
			{
				string[] komiFiles = Directory.GetFiles(folder, "*.komi");

				foreach (string komi in komiFiles)
				{
					WroteThread wt = Load(komi);
					if (wt != null)
						list.Add(wt);
				}
			}

			return (WroteThread[])list.ToArray(typeof(WroteThread));
		}

		/// <summary>
		/// 指定した板の指定した履歴へのファイルパスを取得
		/// </summary>
		/// <param name="board"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetHistoryPath(BoardInfo board, string key)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (key == "") {
				throw new ArgumentException("key");
			}

			string folder = cache.GetFolderPath(board);
			string filePath = Path.Combine(folder, key + ".komi");

			return filePath;
		}

		/// <summary>
		/// 指定した板の書き込み履歴をすべて削除
		/// </summary>
		/// <param name="board"></param>
		public void Remove(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			string folder = cache.GetFolderPath(board, false);
			if (Directory.Exists(folder))
			{
				string[] files = Directory.GetFiles(folder, "*.komi");
				foreach (string f in files)
					File.Delete(f);
			}
		}

		/// <summary>
		/// すべての履歴を削除
		/// </summary>
		public void Clear()
		{
			string[] dirs = 
				Directory.GetDirectories(cache.BaseDirectory);

			foreach (string d in dirs)
			{
				string[] files = Directory.GetFiles(d, "*.komi");
				foreach (string f in files)
					File.Delete(f);
			}
		}
	}
}
