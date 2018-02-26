// WroteThreadHeaderCollection.cs

namespace Twin
{
	using System;
	using System.Collections;

	/// <summary>
	/// WroteThreadHeaderCollection の概要の説明です。
	/// </summary>
	public class WroteThreadHeaderCollection : CollectionBase
	{
		/// <summary>
		/// 指定したインデックスの要素を取得または設定
		/// </summary>
		public WroteThreadHeader this[int index] {
			set {
				if (value == null)
					throw new ArgumentNullException("Indexer");
				List[index] = value;
			}
			get { return (WroteThreadHeader)List[index]; }
		}

		/// <summary>
		/// WroteThreadHeaderCollectionクラスのインスタンスを初期化
		/// </summary>
		public WroteThreadHeaderCollection()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			InnerList.Capacity = 100;
		}

		/// <summary>
		/// コレクションに要素を追加
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public int Add(WroteThreadHeader header)
		{
			return List.Add(header);
		}

		/// <summary>
		/// コレクションに要素を追加
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(WroteThreadHeaderCollection items)
		{
			InnerList.AddRange(items);
		}
	}
}
