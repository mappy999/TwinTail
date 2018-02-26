// ItaBotan.cs

namespace Twin.Forms
{
	using System;
	using System.Collections;
	using System.Xml;
	using System.Text;
	using System.IO;
	using CSharpSamples;
	using Twin.Tools;

	/// <summary>
	/// 板ボタンを管理するクラス
	/// 板ボタンには、Categoryクラス、BoardInfoクラス、BookmarkEntryクラス、SearchBotanクラスなどを登録できる。v2.5.100
	/// 本当はインターフェース使って処理を共通化したほうがいいのかも…
	/// </summary>
	public class ItaBotan
	{
		/// <summary>
		/// 指定したツールバーの板ボタンのoldboardをnewboardに置き換える
		/// </summary>
		/// <param name="toolbar"></param>
		/// <param name="oldboard"></param>
		/// <param name="newboard"></param>
		public static void ServerChange(CSharpToolBar toolbar, BoardInfo oldboard, BoardInfo newboard)
		{
			foreach (CSharpToolBarButton but in toolbar.Buttons)
			{
				if (but.Tag is Category)
				{
					Category cate = (Category)but.Tag;
					foreach (BoardInfo board in cate.Children)
						if (oldboard.Equals(board))
							board.Server = newboard.Server;
				}
				else if (but.Tag is BoardInfo)
				{
					BoardInfo bi = (BoardInfo)but.Tag;
					if (bi.Name == oldboard.Name)
					{
						bi.Server = newboard.Server;
					}
				}
			}
		}

		/// <summary>
		/// 指定したファイルに板ボタン情報を保存
		/// </summary>
		/// <param name="filePath"></param>
		public static void Save(string filePath,
			CSharpToolBarButton.CSharpToolBarButtonCollection buttons)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}
			if (buttons == null) {
				throw new ArgumentNullException("buttons");
			}

			XmlDocument doc = new XmlDocument();
			XmlNode root = doc.CreateElement("ItaBotan");

			foreach (CSharpToolBarButton item in buttons)
			{
				object obj = item.Tag;

				if (obj is BoardInfo)
				{
					XmlNode node = CreateBoardElement(doc, (BoardInfo)obj);
					root.AppendChild(node);
				}
				else if (obj is Category)
				{
					Category cate = (Category)obj;

					XmlAttribute attr = doc.CreateAttribute("Name");
					attr.Value = cate.Name;

					XmlNode node = doc.CreateElement("Category");
					node.Attributes.Append(attr);

					foreach (BoardInfo board in cate.Children)
					{
						XmlNode child = CreateBoardElement(doc, board);
						node.AppendChild(child);
					}
					root.AppendChild(node);
				}
				else if (obj is BookmarkEntry)
				{
					BookmarkEntry entry = (BookmarkEntry)obj;

					XmlAttribute id = doc.CreateAttribute("ID");
					id.Value = entry.Id.ToString();

					XmlNode node = doc.CreateElement("Bookmark");
					node.Attributes.Append(id);
					node.InnerText = entry.Name;

					root.AppendChild(node);
				}
				// v2.5.100
				else if (obj is SearchBotan)
				{
					var botan = (SearchBotan)obj;
					var attr1 = doc.CreateAttribute("SearchString");
					attr1.Value = botan.SearchString;

					var attr2 = doc.CreateAttribute("SearchSorting");
					attr2.Value = botan.SearchSorting.ToString();

					var attr3 = doc.CreateAttribute("SearchOrder");
					attr3.Value = botan.SearchOrder.ToString();

					XmlNode node = doc.CreateElement("SearchBotan");
					node.Attributes.Append(attr1);
					node.Attributes.Append(attr2);
					node.Attributes.Append(attr3);
					node.InnerText = botan.Caption;

					root.AppendChild(node);
				}
			}

			doc.AppendChild(root);

			XmlTextWriter writer = null;
			try {
				writer = new XmlTextWriter(filePath, Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				doc.Save(writer);
			}
			finally {
				if (writer != null) 
				  writer.Close();
			}
		}

		/// <summary>
		/// 指定した板ボタン情報が格納されているファイルを読み込む
		/// </summary>
		/// <param name="filePath"></param>
		public static CSharpToolBarButton[] Load(string filePath)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}

			ArrayList buttons = new ArrayList();

			if (File.Exists(filePath))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(filePath);

				XmlNode root = doc.DocumentElement;

				foreach (XmlNode node in root.ChildNodes)
				{
					if (node.Name.ToLower() == "item")
					{
						BoardInfo board = Getboard(node);
						if (board != null) buttons.Add(board);
					}
					else if (node.Name.ToLower() == "category")
					{
						XmlAttribute attr = node.Attributes["Name"];
						Category category = new Category(attr.InnerText);
						foreach (XmlNode child in node.ChildNodes)
						{
							BoardInfo board = Getboard(child);
							if (board != null) category.Children.Add(board);
						}
						buttons.Add(category);
					}
					else if (node.Name.ToLower() == "bookmark")
					{
						XmlAttribute attr = node.Attributes["ID"];

						BookmarkEntry entry = BookmarkEntry.GetEntryOf(Int32.Parse(attr.Value));
						if (entry != null)
							buttons.Add(entry);
					}
					// v2.5.100
					else if (node.Name.ToLower() == "searchbotan")
					{
						var attrStr = node.Attributes["SearchString"];
						SubjectSearchOrder order = (SubjectSearchOrder)Enum.Parse(typeof(SubjectSearchOrder), node.Attributes["SearchOrder"].Value);
						SubjectSearchSorting sorting = (SubjectSearchSorting)Enum.Parse(typeof(SubjectSearchSorting), node.Attributes["SearchSorting"].Value);

						var botan = new SearchBotan
						{ Caption = node.InnerText, SearchString = attrStr.Value, SearchOrder = order, SearchSorting = sorting, };

						buttons.Add(botan);
					}
				}
			}

			for (int i = 0; i < buttons.Count; i++)
			{
				CSharpToolBarButton item = new CSharpToolBarButton();
				item.Text = buttons[i].ToString();
				item.Tag = buttons[i];
				buttons[i] = item;
			}

			return (CSharpToolBarButton[])buttons.ToArray(typeof(CSharpToolBarButton));
		}

		#region
		private static BoardInfo Getboard(XmlNode node)
		{
			XmlAttribute serv = node.Attributes["Server"];
			XmlAttribute path = node.Attributes["Path"];

			if (serv != null && path != null)
			{
				BoardInfo board = new BoardInfo(serv.Value, path.Value, node.InnerText);
				return board;
			}
			return null;
		}

		private static XmlNode CreateBoardElement(XmlDocument doc, BoardInfo board)
		{
			XmlAttribute serv = doc.CreateAttribute("Server");
			serv.Value = board.Server;

			XmlAttribute path = doc.CreateAttribute("Path");
			path.Value = board.Path;

			XmlNode node = doc.CreateElement("Item");
			node.Attributes.Append(serv);
			node.Attributes.Append(path);
			node.InnerText = board.Name;

			return node;
		}
		#endregion
	}
}
