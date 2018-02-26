using System;
using System.Collections.Generic;
using System.Text;

namespace Twin.Tools
{
	using __WordSet = System.Collections.Generic.KeyValuePair<Twin.Tools.NextThreadChecker.WordType, string>;
	using WordType = Twin.Tools.NextThreadChecker.WordType;
	using System.Text.RegularExpressions;
	using System.Diagnostics;
	using Twin.Text;

	public class SelfNotify
	{
		/// <summary>
		/// items の中から target に一致すると思われるレスを探します。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="items"></param>
		/// <param name="id">空文字列を指定した場合、ほぼ自分のレスと思われるIDが見つかればそれが格納される</param>
		/// <returns></returns>
		public static ResSet Find(WroteRes target, ResSetCollection items, ref string id)
		{
			ResSet result = ResSet.Empty;
			int maxLevel = 0, sameLevelCount = 0;
			
			List<__WordSet> wordList = NextThreadChecker.GetWords(target.Message);

			foreach (ResSet res in items)
			{
				int level = 0;

				// 名前欄の一致
				if (target.From.Length > 0)
				{
					if (target.From.Contains("#") && !res.Name.Contains("◆") ||
						!target.From.Contains("#") && res.Name.Contains("◆"))
						continue;

					// トリップが入力されていたらトリップの名前部だけを判断する
					int tripIndex = target.From.IndexOf("#");
					if (tripIndex >= 0)
					{
						if (res.Name.Contains("◆"))
							level++;

						if (tripIndex > 0)
						{
							string head = target.From.Substring(0, tripIndex);
							if (res.Name.StartsWith(head)) level += 10; // ほぼ間違いなく自分のレス
						}
					}
					else if (target.From == res.Name)
						level++;
				}

				// メール欄の一致
				if (target.Email == res.Email)
					level++;

				// 本文の一致
				// 書き込んだ本文を単語に切り分け、一個ずつ検索していく
				foreach (__WordSet ws in wordList)
				{
					WordType type = ws.Key;
					string text = ws.Value;

					if (Regex.IsMatch(res.Body, Regex.Escape(text)))
					{
						level++;
					}
				}

				// タグと空白を除去した場合の文字列長が一致するかどうか
				string tmp = HtmlTextUtility.TrimTag(res.Body);
				if (tmp.Length == target.Message.Length) level++;

				// idの一致
				if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(res.ID) && res.ID.Contains(id))
					level++;

				Debug.WriteLine("No" + res.Index + " level: " + level);
				
				if (maxLevel < level)
				{
					result = res;
					maxLevel = level;
					sameLevelCount = 0;
				}
				else if (maxLevel == level)
				{
					sameLevelCount++;
				}
			}

			// もしほかに同じレベルのレスがなければ、ほぼ間違いないと思われる
			if (sameLevelCount == 0 && maxLevel > 0)
				id = result.ID;

			Debug.WriteLine("Match! No" + result.Index);
		
			return result;
		}
	}
}
