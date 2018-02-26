// CacheSearcher.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;
	using Twin.Text;
	using Twin.IO;
	using CSharpSamples;

	/// <summary>
	/// キャッシュ情報を検索するための基本クラス
	/// </summary>
	public abstract class CacheSearcher
	{
		protected readonly Cache cache;
		protected readonly BoardInfoCollection targets;
		protected readonly SearchOptions options;

		/// <summary>
		/// 検索オプションを取得
		/// </summary>
		public SearchOptions Options
		{
			get
			{
				return options;
			}
		}

		/// <summary>
		/// CacheSearcherクラスのインスタンスを初期化
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="targets"></param>
		/// <param name="options"></param>
		protected CacheSearcher(Cache cache, BoardInfoCollection targets, SearchOptions options)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (targets == null)
			{
				throw new ArgumentNullException("targets");
			}
			this.cache = cache;
			this.targets = targets;
			this.options = options;
		}

		/// <summary>
		/// 検索対象の板のスレッド情報をすべて取得
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		protected List<ThreadHeader> GetTargetThreads()
		{
			GotThreadListReader listReader = new GotThreadListReader(cache);
			List<ThreadHeader> items = new List<ThreadHeader>();

			foreach (BoardInfo board in targets)
			{
				if (cache.Exists(board))
				{
					if (listReader.Open(board))
					{
						while (listReader.Read(items) != 0)
							;
						listReader.Close();
					}
				}
			}

			return items;
		}

		/// <summary>
		/// 指定したキーワードで検索し検索対象のスレッド数を返す
		/// </summary>
		/// <param name="keyword">検索キーワード</param>
		/// <returns>検索対象の板に存在するスレッドの総数</returns>
		public abstract int Search(string keyword);

		/// <summary>
		/// 次のスレッドを検索し、検索結果を返す
		/// </summary>
		/// <returns>一致したスレッドが存在すれば検索結果を返し、一致しなければnullを返す。</returns>
		public abstract CacheSearchResult Next();

		/// <summary>
		/// すべてのスレッドを検索し、検索結果の配列を返す
		/// </summary>
		/// <returns></returns>
		public abstract CacheSearchResult[] Matches();
	}
}
