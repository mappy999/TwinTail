// CacheDatSearcher.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using Twin.Text;
	using Twin.IO;
	using CSharpSamples.Text.Search;

	/// <summary>
	/// CacheDatSearcher の概要の説明です。
	/// </summary>
	public class CacheDatSearcher : CacheSearcher
	{
		private List<ThreadHeader> items;
		private ISearchable searcher;
		private int index;

		/// <summary>
		/// CacheDatSearcherクラスのインスタンスを初期化
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="targets"></param>
		public CacheDatSearcher(Cache cache, BoardInfoCollection targets, SearchOptions options)
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
				StreamReader sr = null;

				try {
					if (ThreadIndexer.Read(cache, header) != null)
					{
						// datを開く
						string path = cache.GetDatPath(header);
						sr = new StreamReader(StreamCreator.CreateReader(path, header.UseGzip), Encoding.Default);

						// 全文を検索
						int pos = searcher.Search(sr.ReadToEnd());
						if (pos != -1) {
							result = new CacheSearchResult(header, pos);
							break;
						}
					}
				}
				finally {
					if (sr != null)
						sr.Close();
					sr = null;
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
