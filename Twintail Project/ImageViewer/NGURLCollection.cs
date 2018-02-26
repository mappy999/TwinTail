// NGURLCollection.cs

namespace ImageViewerDll
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using CSharpSamples.Text.Search;

	/// <summary>
	/// NGURLCollection の概要の説明です。
	/// </summary>
	public class NGURLCollection
	{
		private ArrayList searcher;

		/// <summary>
		/// 登録されているすべてのパターンを取得または設定
		/// </summary>
		public string[] Patterns {
			set {
				searcher.Clear();

				foreach (string pattern in value)
					Add(pattern);
			}
			get {
				ArrayList arrayList = new ArrayList();

				foreach (ISearchable s in searcher)
					arrayList.Add(s.Pattern);

				return (string[])arrayList.ToArray(typeof(string));
			}
		}

		/// <summary>
		/// NGURLCollectionクラスのインスタンスを初期化
		/// </summary>
		public NGURLCollection()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			searcher = new ArrayList();
		}

		/// <summary>
		/// パターンを追加
		/// </summary>
		/// <param name="pattern"></param>
		public void Add(string pattern)
		{
			searcher.Add(new BmSearch2(pattern));
		}

		/// <summary>
		/// 指定したインデックスの要素を削除
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			searcher.RemoveAt(index);
		}

		/// <summary>
		/// すべてのパターンを削除
		/// </summary>
		public void Clear()
		{
			searcher.Clear();
		}

		/// <summary>
		/// 指定したURLが登録されているパターンに一致するかどうかを判断
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public bool IsMatch(string url)
		{
			foreach (ISearchable s in searcher)
			{
				if (s.Search(url) >= 0)
					return true;
			}
			return false;
		}
	}
}
