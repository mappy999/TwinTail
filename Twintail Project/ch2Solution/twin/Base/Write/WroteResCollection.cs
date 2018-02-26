// WroteResCollection.cs

namespace Twin
{
	using System;
	using System.Collections;

	/// <summary>
	/// WroteResのコレクションを表す
	/// </summary>
	public class WroteResCollection : CollectionBase
	{
		private int totalSize;

		/// <summary>
		/// 指定したインデックスのレス履歴を取得
		/// </summary>
		public WroteRes this[int index] {
			get {
				return (WroteRes)List[index];
			}
		}

		/// <summary>
		/// レスの合計サイズを取得
		/// </summary>
		public int TotalSize {
			get {
				return totalSize;
			}
		}

		/// <summary>
		/// WroteResCollectionクラスのインスタンスを初期化
		/// </summary>
		public WroteResCollection()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			totalSize = 0;
		}

		/// <summary>
		/// 指定したresをコレクションに追加
		/// </summary>
		/// <param name="res"></param>
		/// <returns></returns>
		public int Add(WroteRes res)
		{
			totalSize += res.Length;
			return List.Add(res);
		}

		/// <summary>
		/// WroteResコレクションをコレクションに追加
		/// </summary>
		public void AddRange(WroteResCollection resCollection)
		{
			foreach (WroteRes r in resCollection)
				totalSize += r.Length;
			InnerList.AddRange(resCollection);
		}

		/// <summary>
		/// すべての要素をコレクションから削除
		/// </summary>
		public new void Clear()
		{
			totalSize = 0;
			List.Clear();
		}
	}
}
