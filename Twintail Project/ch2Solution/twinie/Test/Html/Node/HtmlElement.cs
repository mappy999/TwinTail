// HtmlElement.cs

namespace Twin.Test
{
	using System;
	using System.Text;
	using System.Collections;

	/// <summary>
	/// HtmlElement の概要の説明です。
	/// </summary>
	public class __HtmlElement : HtmlNode
	{
		private HtmlNodeCollection nodes;
		private HtmlAttributeCollection attributes;

		private bool isTerminated;
		private bool isEmptyElementTag;
		private string name;

		/// <summary>
		/// 子ノードコレクションを取得
		/// </summary>
		public HtmlNodeCollection Nodes {
			get {
				return nodes;
			}
		}

		/// <summary>
		/// 属性コレクションを取得
		/// </summary>
		public HtmlAttributeCollection Attributes {
			get {
				return attributes;
			}
		}

		/// <summary>
		/// １つのタグで完結するタグかどうかを取得または設定
		/// </summary>
		public bool IsEmptyElementTag {
			set {
				isEmptyElementTag = value;
			}
			get {
				return isEmptyElementTag && nodes.Count == 0;
			}
		}

		/// <summary>
		/// 挟む形式のタグかどうかを取得または設定
		/// </summary>
		public bool IsTerminated {
			set {
				isTerminated = value;
			}
			get {
				return isTerminated || Nodes.Count > 0;
			}
		}

		/// <summary>
		/// タグの名前を取得
		/// </summary>
		public override string Name
		{
			get {
				return name;
			}
		}

		/// <summary>
		/// このノードを子ノードも含めHtml形式に変換
		/// </summary>
		public override string OuterHtml {
			get {
				StringBuilder sb = new StringBuilder();

				sb.Append("<").Append(Name);

				foreach (HtmlAttribute attr in attributes)
				{
					sb.Append(" ").Append(attr.Html);
				}

				if (nodes.Count > 0)
				{
					sb.Append(">");

					foreach (HtmlNode child in nodes)
						sb.Append(child.OuterHtml);

					sb.Append("</").Append(Name).Append(">");
				}
				else {
					if (IsEmptyElementTag)
					{
						sb.Append("/>");
					}
					else if (IsTerminated)
					{
						sb.Append("></").Append(Name).Append(">");
					}
					else {
						sb.Append(">");
					}
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// このノードの内部Htmlを取得
		/// </summary>
		public override string InnerHtml {
			get {
				StringBuilder sb = new StringBuilder();

				foreach (HtmlNode child in nodes)
					sb.Append(child.InnerHtml);

				return sb.ToString();
			}
		}

		/// <summary>
		/// このノードの内部テキストを取得
		/// </summary>
		public override string InnerText {
			get {
				StringBuilder sb = new StringBuilder();
				
				foreach (HtmlNode child in nodes)
					sb.Append(child.InnerText);

				return sb.ToString();
			}
		}

		/// <summary>
		/// HtmlElementクラスのインスタンスを初期化
		/// </summary>
		/// <param name="name"></param>
		public __HtmlElement(string name)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.nodes = new HtmlNodeCollection(this);
			this.attributes = new HtmlAttributeCollection(this);
			this.isTerminated = false;
			this.isEmptyElementTag = false;
			this.name = name.ToUpper();
		}

		/// <summary>
		/// HtmlElementクラスのインスタンスを初期化
		/// </summary>
		public __HtmlElement() : this(String.Empty)
		{
		}

		/// <summary>
		/// このエレメントがnodeの親かどうかを判断
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsAncestor(HtmlNode node)
		{
			while (node != null)
			{
				if (this == node.Parent)
					return true;

				node = node.Parent;
			}	
			return false;
		}

		/// <summary>
		/// このノードがnodeの子孫かどうかを判断
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsDescendant(HtmlNode node)
		{
			__HtmlElement e = node as __HtmlElement;
			return e != null ? e.IsAncestor(this) : false;
		}

		/// <summary>
		/// 指定した名前のタグをすべて取得
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		public __HtmlElement[] GetElementsByName(string tagName)
		{
			return Sta_GetElementsByName(nodes, tagName.ToUpper());
		}

		/// <summary>
		/// 指定したIDを持つエレメントを取得
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public __HtmlElement GetElementById(string id)
		{
			return Sta_GetElementById(nodes, id);
		}

		/// <summary>
		/// ノードコレクション内の指定したタグ名前を持つすべてのエレメントを取得
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="tagName"></param>
		/// <returns></returns>
		internal static __HtmlElement[] Sta_GetElementsByName(HtmlNodeCollection nodes, string tagName)
		{
			ArrayList arrayList = new ArrayList();

			foreach (HtmlNode node in nodes)
			{
				__HtmlElement elem = node as __HtmlElement;

				if (elem != null)
				{
					if (String.Compare(elem.Name, tagName, true) == 0)
						arrayList.Add(elem);

					__HtmlElement[] array = elem.GetElementsByName(tagName);
					arrayList.AddRange(array);
				}
			}

			return (__HtmlElement[])arrayList.ToArray(typeof(__HtmlElement));
		}

		/// <summary>
		/// ノードコレクション内の指定したIDを持つエレメントを取得
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		internal static __HtmlElement Sta_GetElementById(HtmlNodeCollection nodes, string id)
		{
			foreach (HtmlNode node in nodes)
			{
				__HtmlElement element = node as __HtmlElement;

				if (element != null)
				{
					string val = element.Attributes["id"];

					if (String.Compare(id, val, true) == 0)
						return element;

					__HtmlElement sub = element.GetElementById(id);
					if (sub != null) return sub;
				}
			}

			return null;
		}
	}
}
