// ThreadListSearchResult.cs

namespace Twin.Text
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using Twin.Forms;
	using CSharpSamples;

	/// <summary>
	/// ThreadListViewクラスのスレ一覧を検索するための機能を提供
	/// </summary>
	public class ThreadListSearcher : AbstractSearcher
	{
		private List<ThreadHeader> matches;
		private List<ThreadHeader> temporary;
		private ThreadListView listview;

		/// <summary>
		/// ThreadListSearcherクラスのインスタンスを初期化
		/// </summary>
		public ThreadListSearcher(ThreadListView view)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			listview = view;
			matches = new List<ThreadHeader>();
			matches.AddRange(view.Items);
			temporary = new List<ThreadHeader>();
			temporary.AddRange(view.Items);
		}

		/// <summary>
		/// スレッド一覧を初期状態に戻す
		/// </summary>
		public override void Reset()
		{
			matches.Clear();
			matches.AddRange(temporary);
			listview.SetItems(matches);
		}

		/// <summary>
		/// 前回の検索結果から、絞り込み検査ｋ
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public override bool Search(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}

			// 検索結果を入れるコレクション
			List<ThreadHeader> result = new List<ThreadHeader>();

			if (key.Length > 0)
			{
				// 正規表現のオプション
				RegexOptions regopt = RegexOptions.Compiled;

				if ((Options & SearchOptions.MatchCase) == 0)
					regopt = RegexOptions.IgnoreCase;

				if ((Options & SearchOptions.Regex) == 0)
					key = Regex.Escape(key);

				// 正規表現をコンパイル
				Regex regex = new Regex(key, regopt);

				foreach (ThreadHeader h in matches)
				{
					if (regex.IsMatch(h.Subject))
						result.Add(h);
				}
			}
			else
			{
				// 空のキーワードを指定された場合はすべてのアイテムを設定
				result.AddRange(temporary);
			}
			matches.Clear();
			matches.AddRange(result);
			listview.SetItems(matches);

			return (matches.Count > 0);
		}

		/// <summary>
		/// このメソッドはサポートしていません
		/// </summary>
		public override void Highlights(string key)
		{
			throw new NotSupportedException("Highlightsメソッドはサポートしていません");
		}
	}
}
