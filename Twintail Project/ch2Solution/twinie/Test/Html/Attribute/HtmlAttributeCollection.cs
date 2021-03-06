// HtmlAttributeCollection.cs

namespace Twin.Test
{
	using System;
	using System.Collections;

	/// <summary>
	/// HtmlAttributeクラスをコレクション管理
	/// </summary>
	public class HtmlAttributeCollection
	{
		/// <summary>
		/// 属性を管理するコレクション
		/// </summary>
		private ArrayList attributes;

		// この属性コレクションのインスタンスを保持している親要素
		private __HtmlElement parent;

		/// <summary>
		/// コレクションに登録されている属性の数を返す
		/// </summary>
		public int Count {
			get {
				return attributes.Count;
			}
		}

		/// <summary>
		/// 指定したインデックスにある属性を取得または設定
		/// </summary>
		public HtmlAttribute this[int index] {
			set {
				attributes[index] = value;
			}
			get {
				return (HtmlAttribute)attributes[index];
			}
		}

		/// <summary>
		/// 指定した名前を持つ属性の値を取得または設定
		/// </summary>
		public string this[string name] {
			set {
				HtmlAttribute attr = FindByName(name);

				if (attr == null)
				{
					Add(new HtmlAttribute(name, value));
				}
				else {
					attr.Value = value;
				}
			}
			get {
				HtmlAttribute attr = FindByName(name);
				return (attr != null) ? attr.Value : null;
			}
		}

		/// <summary>
		/// HtmlAttributeCollectionクラスのインスタンスを初期化
		/// </summary>
		/// <param name="parent"></param>
		public HtmlAttributeCollection(__HtmlElement parent)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.attributes = new ArrayList();
			this.parent = parent;
		}

		/// <summary>
		/// コレクションの末尾に属性を追加
		/// </summary>
		/// <param name="attr"></param>
		/// <returns></returns>
		public int Add(HtmlAttribute attr)
		{
			if (attributes.Contains(attr))
				throw new HtmlException();	// 同一インスタンスを複数登録することは出来ない

			return attributes.Add(attr);
		}

		/// <summary>
		/// 指定したインデックスにattrを挿入
		/// </summary>
		/// <param name="index"></param>
		/// <param name="attr"></param>
		public void Insert(int index, HtmlAttribute attr)
		{
			if (attributes.Contains(attr))
				throw new HtmlException();	// 同一インスタンスを複数登録することは出来ない

			attributes.Insert(index, attr);
		}

		/// <summary>
		/// attrをコレクションから削除
		/// </summary>
		/// <param name="attr"></param>
		public void Remove(HtmlAttribute attr)
		{
			attributes.Remove(attr);
		}

		/// <summary>
		/// 指定したインデックスにある属性をコレクションから削除
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			attributes.RemoveAt(index);
		}

		/// <summary>
		/// すべての属性をコレクションから削除
		/// </summary>
		public void RemoveAll()
		{
			attributes.Clear();
		}

		/// <summary>
		/// 指定した名前を持つ属性を返す
		/// </summary>
		/// <param name="name">検索する属性名。nullを指定するとArgumentNullException。</param>
		/// <returns>見つかればその属性のインスタンス、見つからなければnullを返す</returns>
		public HtmlAttribute FindByName(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			foreach (HtmlAttribute attr in attributes)
			{
				if (attr.Name.ToUpper().Equals(name))
					return attr;
			}

			return null;
		}

		/// <summary>
		/// HtmlAttributeCollectionを反復処理する列挙子を返す
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			return attributes.GetEnumerator();
		}
	}
}
