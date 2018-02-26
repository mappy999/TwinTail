// StringUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.IO;

	/// <summary>
	/// 文字列操作ユーティリティ群
	/// </summary>
	public class StringUtility
	{
		/// <summary>
		/// 先頭の余分な空白を取り除く
		/// </summary>
		/// <param name="text">操作対象の文字列</param>
		/// <returns></returns>
		public static string RemoveHeadSpace(string text)
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (!Char.IsWhiteSpace(text, i))
					return text.Substring(i);
			}

			return text;
		}

		/// <summary>
		/// 数字を文字列に変換 (0は空文字列に変換)
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string StringOf(int number)
		{
			if (number == 0) return String.Empty;
			else             return number.ToString();
		}

		public static string Unescape(string str)
		{
			StringBuilder sb = new StringBuilder(str);

			sb.Replace("&quot;", "\"");
			sb.Replace("&amp;", "&");
			sb.Replace("&lt;", "<");
			sb.Replace("&gt;", ">");

			return sb.ToString();
		}

		/// <summary>
		/// 日付を文字列に変換
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string StringOf(DateTime date, bool toLocal)
		{
			if (toLocal)
				date = date.ToLocalTime();

			if (date.Ticks == 0) return String.Empty;
			else                 return date.ToString(Twinie.Settings.DateFormat);
		}

		/// <summary>
		/// 指定した数値をアクセスキー用の文字列に変換して返す。
		/// 1から9、10は0、11以降はAからZまで。
		/// </summary>
		/// <param name="value"></param>
		/// <returns>1から9はそのまま返す。10の場合は0に変換。11以降の場合はA-Zに変換。</returns>
		public static string GetAccessKeyString(int digit)
		{
			if (digit < 1)
				throw new ArgumentException("digit");

			if (digit == 10)
				digit = 0;

			return (digit < 10) ? digit.ToString() :
				((char)('A' + (digit-11))).ToString();
		}

		/// <summary>
		/// 指定したスレッドのスレ立て用テンプレートを取得
		/// </summary>
		/// <param name="h"></param>
		/// <returns></returns>
		public static PostThread GetThreadTemplate(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
#if DEBUG
			// 本文を作成
			string template = String.Format("\r\n{0}\r\n", header.Url);
#else
			// 本文を作成
			string template = String.Format("前スレ\r\n\r\n{0}\r\n{1}\r\n",
				header.Subject, header.Url);
#endif
			// スレッド名はtwin1みたいなのはめんどくさいので空に
			PostThread ps = 
				new PostThread(String.Empty, template);
			
			return ps;
		}

		/// <summary>
		/// ポップアップ可能な文字列かどうかを判断
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsPopupString(string text)
		{
			// 20,30,40-50などの形式に対応
			return Regex.IsMatch(text, @"^[\d,\-]+$");
		}

		/// <summary>
		/// ファイル名に使用できない文字を削除して返す
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string RemoveInvalidPathChars(string text)
		{
			return ReplaceInvalidPathChars(text, String.Empty);
		}

		/// <summary>
		/// ファイル名に使用できない文字をreplacementに置き換えて返す
		/// </summary>
		/// <param name="text"></param>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public static string ReplaceInvalidPathChars(string text, string replacement)
		{
			StringBuilder sb = new StringBuilder(text);

			foreach (char ch in Path.GetInvalidPathChars())
				sb.Replace(ch.ToString(), replacement);

			foreach (char ch in Path.GetInvalidFileNameChars())
				sb.Replace(ch.ToString(), replacement);

			sb.Replace(Path.DirectorySeparatorChar.ToString(), replacement);
			sb.Replace(Path.AltDirectorySeparatorChar.ToString(), replacement);
			sb.Replace(Path.VolumeSeparatorChar.ToString(), replacement);
			sb.Replace("?", replacement);

			return sb.ToString();
		}
	}
}
