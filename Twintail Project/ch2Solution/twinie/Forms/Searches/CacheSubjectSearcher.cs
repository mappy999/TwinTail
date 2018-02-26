// CacheSubjectSearcher.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;
	using Twin.Text;
	using CSharpSamples.Text.Search;

	/// <summary>
	/// キャッシュ内の既得スレッド一覧を検索するクラス
	/// </summary>
	public class CacheSubjectSearcher : CacheSearcher
	{
		private List<ThreadHeader> items;
		private ISearchable searcher;
		private int index;

		/// <summary>
		/// CacheSubjectSearcherクラスのインスタンスを初期化
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="targets"></param>
		/// <param name="options"></param>
		public CacheSubjectSearcher(Cache cache, BoardInfoCollection targets, SearchOptions options)
			: base(cache, targets, options)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}

		/// <summary>
		/// 指定したキーワードでキャッシュ内を検索
		/// </summary>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public override int Search(string keyword)
		{
			if (keyword == null) {
				throw new ArgumentNullException("keyword");
			}
			if (keyword == String.Empty) {
				throw new ArgumentException("keyword", "keywordは空文字に出来ません");
			}

			searcher = new BmSearch2(keyword);
			items = GetTargetThreads();
			index = 0;

			return items.Count;
		}

		/// <summary>
		/// 次を検索
		/// </summary>
		/// <returns></returns>
		public override CacheSearchResult Next()
		{
			CacheSearchResult result = null;

			while (index < items.Count)
			{
				ThreadHeader header = items[index++];
				int position = searcher.Search(header.Subject);

				if (position != -1) {
					result = new CacheSearchResult(header, position);
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// すべてのスレッド一覧を検索
		/// </summary>
		/// <returns></returns>
		public override CacheSearchResult[] Matches()
		{
			List<CacheSearchResult> list = new List<CacheSearchResult>();
			CacheSearchResult r = null;

			while ((r = Next()) != null)
				list.Add(r);

			return list.ToArray();
		}
	}
}
