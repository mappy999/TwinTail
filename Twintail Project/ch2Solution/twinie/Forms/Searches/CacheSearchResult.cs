// CacheSearchResult.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// CacheSearcherの検索結果を表す
	/// </summary>
	public class CacheSearchResult
	{
		private ThreadHeader header;
		private int index;

		/// <summary>
		/// 検索したスレッドの情報を取得
		/// </summary>
		public ThreadHeader HeaderInfo {
			get { return header; }
		}

		/// <summary>
		/// 検索成功した場合、一致インデックスを取得。
		/// 検索失敗した場合、-1を返す。
		/// </summary>
		public int Index {
			get { return index; }
		}

		/// <summary>
		/// CacheSearchResultクラスのインスタンスを初期化
		/// </summary>
		public CacheSearchResult(ThreadHeader header, int index)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.header = header;
			this.index = index;
		}
	}
}
