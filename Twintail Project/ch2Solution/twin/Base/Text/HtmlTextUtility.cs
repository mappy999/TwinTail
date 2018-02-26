// HtmlTextUtility.cs

namespace Twin.Text
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Drawing;
	using System.Windows.Forms;

	// *****
	//  2012/1/15
	//  URL�����s�����ɘA�����ď�����Ă����ꍇ�ɂ����܂������N����悤�ɏC��
	//  ���ʂ���ݸ��ł��I
	// ******

	/// <summary>
	/// Html�╶���񑀍�̃��[�e�B���e�B�Q
	/// </summary>
	public class HtmlTextUtility
	{
		/// <summary>
		/// �^�O���ׂĂ��������鐳�K�\��
		/// </summary>
		public static Regex SearchTagRegex = 
			new Regex(@"</?[^>]*/?>", RegexOptions.Compiled);

		/// <summary>
		/// �������ǂ����𔻒f���邽�߂̐��K�\��
		/// </summary>
		public static readonly Regex IsDigitRegex =
			new Regex(@"^\d+$", RegexOptions.Compiled);

//		/// <summary>
//		/// <br>�^�O�ŕ������邽�߂̐��K�\��
//		/// </summary>
//		public static readonly Regex SplitRegex =
//			new Regex(@"<br>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// ���X�Q��(��: >>1-5 �`��)���������邽�߂̐��K�\��
		/// </summary>
		public static readonly Regex RefRegex =
			new Regex(@"(?<ref>&gt;&gt;(?<num>[1-9]+[\d\-\+,]*))", RegexOptions.Compiled);�@// 2011.12.16 ���ʂ���

//			new Regex(@"(?<ref>&gt;&gt;(?<num>\d+\-?\d*))", RegexOptions.Compiled);

		/// <summary>
		/// ���X�Q��(��: >>10-15,20,30,40-50�`��) ���������邽�߂̐��K�\���B������">>10-15"�̃��X�Ԃ͎��Ȃ��B
		/// </summary>
		public static readonly Regex ExRefRegex =
			new Regex(@"(?<=&gt;&gt;[\d\-\,]+?(,|\+))(?<num>\d+\-?\d*)", RegexOptions.Compiled);
		//			new Regex(@"(?<=&gt;&gt;[^\s]+?(,|\+))(?<num>\d+\-?\d*)", RegexOptions.Compiled);

		/// <summary>
		/// ttp://��URL���������鐳�K�\��
		/// </summary>
		public static readonly Regex ttpToRegex =
// 1/15		new Regex(@"(?<!h)(?<link>(ttp://[\w\.]+?/wiki/[^\s\<]+)|(ttps?://[a-zA-Z\d/_@#%&+*:;=~',.!()|?[\]\-]+))", 
			new Regex(@"((?<!h)(?<link>(ttp://[\w\.]+?/wiki/[^\s\<]+)|(ttps?://[a-zA-Z\d/_@#%&+*:;=~',.!()|?[\]\-]+))(?=h?ttp://))|((?<!h)(?<link>(ttp://[\w\.]+?/wiki/[^\s\<]+)|(ttps?://[a-zA-Z\d/_@#%&+*:;=~',.!()|?[\]\-]+)))",
				RegexOptions.Compiled);

		/// <summary>
		/// ��{�I��URL���������鐳�K�\��
		/// </summary>
		public static readonly Regex LinkRegex =
			new Regex(@"(?<link>((http://[\w\.]+?/wiki/[^\s\<]+)|((https?|ftps?|mms|rts?p)://[a-zA-Z\d/_@#%&+*:;=~',.!()|?[\]\-]+)))", 
				RegexOptions.Compiled);

		/// <summary>
		/// ��{�I��URL���������鐳�K�\�� (h����������)
		/// </summary>
		public static readonly Regex LinkRegex2 =
// 1/15		new Regex(@"(?<link>(h?ttps?|ftps?|mms|rts?p)://(?<url>[a-zA-Z\d/_@#%&+*:;=~',.!()|?[\]\-]+))", 
			new Regex(@"((?<link>(h?ttps?|ftps?|mms|rts?p)://(?<url>[a-zA-Z\d/_@#%&+*:;=~',.!|?[\]\-]+))(?=h?ttp://))|((?<link>(h?ttps?|ftps?|mms|rts?p)://(?<url>[a-zA-Z\d/_@#%&+*:;=~',.!|?[\]\-]+)))", 
				RegexOptions.Compiled);

		/// <summary>
		/// �ȗ��`��HTTP��URL���ǂ����𔻒f
		/// </summary>
		public static readonly Regex IsShortHttpUrl =
			new Regex(@"^((ttp|tp)://)|(www.[^/]+\.)", RegexOptions.Compiled);

		// ID�̐��K�\��
		public static readonly Regex IDRegex = new Regex(@"[a-zA-Z0-9+/]{8,9}");

		/// <summary>
		/// �w�肵��html����URL��A�^�O�ŋ��݃����N����
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string Linking(string html)
		{
			if (html == null) {
				throw new ArgumentNullException("html");
			}
			string r = html;
			r = LinkRegex.Replace(r, "<a href=\"${link}\" target=\"_blank\">${link}</a>");
			r = ttpToRegex.Replace(r, "<a href=\"h${link}\" target=\"_blank\">${link}</a>");

			return r;
		}

		/// <summary>
		/// ���p������S�p�����ɕϊ�
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string HanToZen(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			StringBuilder buffer = new StringBuilder(text);
			char[] zenChars = { '�O', '�P', '�Q', '�R', '�S', '�T', '�U', '�V', '�W', '�X' };
			char[] hanChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

			for (int i = 0; i < 10; i++)
				buffer.Replace(hanChars[i], zenChars[i]);

			return buffer.ToString();
		}

		/// <summary>
		/// �S�p�����𔼊p�����ɕϊ�
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string ZenToHan(string text)
		{
			if (text == null) {
				throw new ArgumentNullException("text");
			}

			StringBuilder buffer = new StringBuilder(text);
			char[] zenChars = {'�O','�P','�Q','�R','�S','�T','�U','�V','�W','�X'};
			char[] hanChars = {'0','1','2','3','4','5','6','7','8','9'};

			for (int i = 0; i < 10; i++)
				buffer.Replace(zenChars[i], hanChars[i]);

			return buffer.ToString();
		}

		/// <summary>
		/// �w�肵�������񂪐������ǂ����𔻒f
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsDigit(string text)
		{
			if (text == null)
				return false;
			return IsDigitRegex.IsMatch(text);
		}

		/// <summary>
		/// �^�O����菜��
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string RemoveTag(string html)
		{
			if (html == null)
				return String.Empty;
			return SearchTagRegex.Replace(html, "");
		}

		/// <summary>
		/// �w�肵���^�O����菜���܂��B
		/// </summary>
		/// <param name="html"></param>
		/// <param name="tagName">��������^�O�̖��O�BOR���Z�q�ɂ�蕡���w��ł��܂��B�啶���������͋�ʂ��܂���B</param>
		/// <returns></returns>
		public static string RemoveTag(string html, string tagName)
		{
			return Regex.Replace(html, "</?(" + tagName + ")[^>]*>", String.Empty, RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// HTML��������e�L�X�g�ɕϊ�
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string HtmlToText(string html)
		{
			html = Regex.Replace(html, "<br>", Environment.NewLine, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<[^>]+>", "");
			html = Regex.Replace(html, "&gt;", ">", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&lt;", "<", RegexOptions.IgnoreCase);

			return html;
		}

		/// <summary>
		/// text�̗��[�Ɋ܂܂��󔒂��폜
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string RemoveSpace(string text)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			return Regex.Replace(text, @"\s*([^\s]+)\s*", "${1}");
		}

		/// <summary>
		/// �^�O����уX�y�[�X������
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string TrimTag(string html)
		{
			return Regex.Replace(html, @"\s*|</?[^>]*/?>", String.Empty);
		}

		internal static string HanToZen(int dec)
		{
			throw new NotImplementedException();
		}
	}
}
