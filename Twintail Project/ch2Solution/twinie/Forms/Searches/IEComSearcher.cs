// IEComSearchResult.cs

namespace Twin.Text
{
	using System;
	using System.Drawing;
	using System.Collections;
	using mshtml;
	using Twin.Text;
	using CSharpSamples;
using System.Collections.Generic;

	/// <summary>
	/// IEコンポーネントを使用した単語の検索と結果を表す
	/// </summary>
	public sealed class IEComSearcher : AbstractSearcher
	{
		private readonly HTMLDocument document;
		private IHTMLTxtRange textRange;
		private string bookmark;
		private bool tempRightToLeft;

		private List<__Bookmark> highlights = new List<__Bookmark>();

		private class __Bookmark
		{
			public string Value;
			public object BackColor, ForeColor;
		}

		/// <summary>
		/// IHTMLTxtRange.findText用の検索オプション値を取得
		/// </summary>
		private int ieSearchOptions {
			get {
				SearchOptions flags = 0;

				if ((Options & SearchOptions.MatchCase) != 0)
					flags |= SearchOptions.MatchCase;

				if ((Options & SearchOptions.WholeWordsOnly) != 0)
					flags |= SearchOptions.WholeWordsOnly;

				return (int)flags;
			}
		}

		/// <summary>
		/// ドキュメントの後ろから検索を開始するかどうか
		/// </summary>
		private bool RightToLeft {
			get {
				return (Options & SearchOptions.RightToLeft) != 0;
			}
		}

		/// <summary>
		/// document.body.createTextRangeの戻り値を返す
		/// </summary>
		private IHTMLTxtRange htmlTextRange {
			get {
				HTMLBody body = (HTMLBody)document.body;
				return body.createTextRange();
			}
		}

		/// <summary>
		/// IEComSearcherクラスのインスタンスを初期化
		/// </summary>
		/// <param name="doc">検索対象のドキュメント</param>
		/// <param name="keyword">検索キーワード</param>
		/// <param name="options">検索オプション</param>
		public IEComSearcher(HTMLDocument doc)
		{
			if (doc == null) {
				throw new ArgumentNullException("doc");
			}
			textRange = null;
			document = doc;
		}

		/// <summary>
		/// 次の単語を検索しその位置までスクロール
		/// </summary>
		/// <returns></returns>
		public override bool Search(string text)
		{
			if (textRange == null)
				textRange = htmlTextRange;

			if (text == null || text == String.Empty)
				return false;

			// 0 = none
			// 2 = whole words only
			// 4 = match case
			int flags = ieSearchOptions;
			int count = RightToLeft ? -1 : 0;

			// 検索方向が変更されたら範囲を初期化
			if (RightToLeft != tempRightToLeft)
			{
				textRange.moveStart("Textedit", 0);
				textRange.moveEnd("Textedit", 1);
				tempRightToLeft = RightToLeft;
			}

			if (textRange.findText(text, count, flags))
			{
				bookmark = textRange.getBookmark();

				textRange.select();
				textRange.scrollIntoView(true);

				// 単語を画面の中央に来るまでスクロールする
				HTMLBody body = (HTMLBody)document.body;
				IHTMLElement elem = textRange.parentElement();

				body.scrollTop = elem.offsetTop - body.clientHeight / 2;

				if (RightToLeft) {
					textRange.moveStart("Textedit", 0);
					textRange.moveEnd("Character", -1);
				}
				else {
					textRange.moveStart("Character", 1);
					textRange.moveEnd("Textedit", 1);
				}
				return true;
			}
			else {
				return false;
			}
		}

		/// <summary>
		/// 検索単語をすべてハイライト表示
		/// </summary>
		public override void Highlights(string text)
		{
			if (text == null || text == String.Empty)
				return;

			IHTMLTxtRange range = htmlTextRange;

			while (range.findText(text, 0, ieSearchOptions))
			{
				__Bookmark b = new __Bookmark();
				b.Value = range.getBookmark();
				b.BackColor = range.queryCommandValue("BackColor");
				b.ForeColor = range.queryCommandValue("ForeColor");

				range.execCommand("BackColor", false, 
					ColorTranslator.ToHtml(SystemColors.Highlight));

				range.execCommand("ForeColor", false,
					ColorTranslator.ToHtml(SystemColors.HighlightText));

				range.moveStart("Character", 1);
				range.moveEnd("Textedit", 1);

				highlights.Add(b);
			}
		}

		/// <summary>
		/// 検索をリセットし初期状態に戻す
		/// </summary>
		public override void Reset()
		{
			if (highlights.Count > 0)
			{
				IHTMLTxtRange range = this.htmlTextRange;
				foreach (__Bookmark b in highlights)
				{
					if (range.moveToBookmark(b.Value))
					{
						range.execCommand("BackColor", false, b.BackColor);
						range.execCommand("ForeColor", false, b.ForeColor);
					}
				}
				highlights.Clear();
			}
			//document.location.reload(false);
			textRange = null;
			bookmark = null;


		}
	}
}
