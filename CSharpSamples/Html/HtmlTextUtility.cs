// HtmlTextUtility.cs

namespace CSharpSamples.Html
{
	using System;
	using System.Text;
	using System.Drawing;

	/// <summary>
	/// HtmlUtility �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlTextUtility
	{
		//private static string[] esc_replacement = null;
		private static string[] unesc_replacement = null;

		/// <summary>
		/// ���ꕶ�����G�X�P�[�v
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string Escape(string html)
		{
			throw new NotSupportedException("������");
			//StringBuilder sb = new StringBuilder(html);
			//return sb.ToString();
		}

		/// <summary>
		/// �X�P�[�v���ꂽ���������ɕϊ�
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string UnEscape(string html)
		{
			if (unesc_replacement == null)
			{
				unesc_replacement = new string[]
				{
					"&copy;", new String((char)169, 1),
					"&reg;", new String((char)174, 1),
					"&yen;", "\\",
					"&nbsp;", " ",
					"&lt;", "<",
					"&gt;", ">",
				};
			}

			StringBuilder sb = new StringBuilder(html);

			for (int i = 0; i < unesc_replacement.Length; i += 2)
				sb.Replace(unesc_replacement[i], unesc_replacement[i+1]);

			return sb.ToString();
		}

		/// <summary>
		/// Html�`���̐F����Color�\���̂ɕϊ�
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static Color ColorFromHtml(string html)
		{
			if (html == null)
				throw new ArgumentNullException("html");

			return html.StartsWith("#") ?
				ColorTranslator.FromHtml(html) : Color.FromName(html);
		}
	}
}
