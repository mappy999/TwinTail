// HtmlNode.cs

namespace Twin.Test
{
	using System;

	/// <summary>
	/// HtmlNode の概要の説明です。
	/// </summary>
	public abstract class HtmlNode
	{
		private __HtmlElement parent;

		/// <summary>
		/// このノードが最上層かどうかを取得
		/// </summary>
		public bool IsRoot
		{
			get
			{
				return (parent == null);
			}
		}

		/// <summary>
		/// このノードが子ノードを持つかどうかを示す値を取得します。
		/// </summary>
		public bool HasChildNodes
		{
			get
			{
				__HtmlElement e = this as __HtmlElement;
				return (e != null && e.Nodes.Count > 0) ? true : false;
			}
		}

		/// <summary>
		/// 親ノードを取得
		/// </summary>
		public __HtmlElement Parent
		{
			get
			{
				return parent;
			}
		}

		/// <summary>
		/// 前の兄弟ノードを取得
		/// </summary>
		public HtmlNode PrevSibling
		{
			get
			{
				if (Index != -1 && Parent != null)
				{
					if (Index > 0)
						return Parent.Nodes[Index - 1];
				}
				return null;
			}
		}

		/// <summary>
		/// 次の兄弟ノードを取得
		/// </summary>
		public HtmlNode NextSibling
		{
			get
			{
				if (Index != -1 && Parent != null)
				{
					if (Index + 1 < Parent.Nodes.Count)
						return Parent.Nodes[Index + 1];
				}
				return null;
			}
		}

		/// <summary>
		/// 最初の子ノードを取得 (子ノードが存在しなければnullを返す)
		/// </summary>
		public HtmlNode FirstChild
		{
			get
			{
				__HtmlElement e = this as __HtmlElement;

				if (e == null || e.Nodes.Count == 0)
				{
					return null;
				}
				else
				{
					return e.Nodes[0];
				}
			}
		}

		/// <summary>
		/// 最後の子ノードを取得 (子ノードが存在しなければnullを返す)
		/// </summary>
		public HtmlNode LastChild
		{
			get
			{
				__HtmlElement e = this as __HtmlElement;

				if (e == null || e.Nodes.Count == 0)
				{
					return null;
				}
				else
				{
					return e.Nodes[e.Nodes.Count - 1];
				}
			}
		}

		/// <summary>
		/// ノードコレクション内の位置を取得
		/// </summary>
		public int Index
		{
			get
			{
				return (Parent != null) ?
					Parent.Nodes.IndexOf(this) : -1;
			}
		}

		/// <summary>
		/// このノードを含む内部文字列を取得
		/// </summary>
		public abstract string OuterHtml
		{
			get;
		}

		/// <summary>
		/// このノードの内部文字列を取得
		/// </summary>
		public abstract string InnerHtml
		{
			get;
		}

		/// <summary>
		/// このノードの内部テキストを取得
		/// </summary>
		public abstract string InnerText
		{
			get;
		}

		/// <summary>
		/// このノードの名前を取得します。
		/// </summary>
		public abstract string Name
		{
			get;
		}

		/// <summary>
		/// HtmlNodeクラスのインスタンスを初期化
		/// </summary>
		protected HtmlNode()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.parent = null;
		}

		/// <summary>
		/// このノードに新しい親を設定
		/// </summary>
		/// <param name="newParent"></param>
		internal void SetParent(__HtmlElement newParent)
		{
			parent = newParent;
		}

		/// <summary>
		/// このインスタンスを親ノードから削除
		/// </summary>
		public void Remove()
		{
			if (Parent != null)
				Parent.Nodes.Remove(this);
		}

		/// <summary>
		/// このインスタンスを文字列形式に変換
		/// </summary>
		/// <returns>Htmlプロパティの値を返す</returns>
		public override string ToString()
		{
			return this.OuterHtml;
		}
	}
}
