// ToolItemCollection.cs

namespace Twin.Tools
{
	using System;
	using System.Collections;
	using System.Xml;
	using System.IO;

	/// <summary>
	/// ToolItemCollection の概要の説明です。
	/// </summary>
	public class ToolItemCollection : CollectionBase
	{
		/// <summary>
		/// 指定したインデックスの要素を取得
		/// </summary>
		public ToolItem this[int index] {
			get {
				return (ToolItem)List[index];
			}
		}

		/// <summary>
		/// ToolItemCollectionクラスのインスタンスを初期化
		/// </summary>
		public ToolItemCollection()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}

		/// <summary>
		/// ファイルから読み込む
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
		/// ファイルに保存
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
		/// itemをコレクションの末尾に追加
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(ToolItem item)
		{
			return List.Add(item);
		}

		/// <summary>
		/// indexにitemを挿入
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, ToolItem item)
		{
			List.Insert(index, item);
		}

		/// <summary>
		/// itemをコレクションから削除
		/// </summary>
		/// <param name="item"></param>
		public void Remove(ToolItem item)
		{
			List.Remove(item);
		}
	}
}
