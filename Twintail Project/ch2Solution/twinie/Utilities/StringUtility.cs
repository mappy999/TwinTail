// StringUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.IO;

	/// <summary>
	/// �����񑀍샆�[�e�B���e�B�Q
	/// </summary>
	public class StringUtility
	{
		/// <summary>
		/// �擪�̗]���ȋ󔒂���菜��
		/// </summary>
		/// <param name="text">����Ώۂ̕�����</param>
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
		/// �����𕶎���ɕϊ� (0�͋󕶎���ɕϊ�)
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
		/// ���t�𕶎���ɕϊ�
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
		/// �w�肵�����l���A�N�Z�X�L�[�p�̕�����ɕϊ����ĕԂ��B
		/// 1����9�A10��0�A11�ȍ~��A����Z�܂ŁB
		/// </summary>
		/// <param name="value"></param>
		/// <returns>1����9�͂��̂܂ܕԂ��B10�̏ꍇ��0�ɕϊ��B11�ȍ~�̏ꍇ��A-Z�ɕϊ��B</returns>
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
		/// �w�肵���X���b�h�̃X�����ėp�e���v���[�g���擾
		/// </summary>
		/// <param name="h"></param>
		/// <returns></returns>
		public static PostThread GetThreadTemplate(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
#if DEBUG
			// �{�����쐬
			string template = String.Format("\r\n{0}\r\n", header.Url);
#else
			// �{�����쐬
			string template = String.Format("�O�X��\r\n\r\n{0}\r\n{1}\r\n",
				header.Subject, header.Url);
#endif
			// �X���b�h����twin1�݂����Ȃ̂͂߂�ǂ������̂ŋ��
			PostThread ps = 
				new PostThread(String.Empty, template);
			
			return ps;
		}

		/// <summary>
		/// �|�b�v�A�b�v�\�ȕ����񂩂ǂ����𔻒f
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsPopupString(string text)
		{
			// 20,30,40-50�Ȃǂ̌`���ɑΉ�
			return Regex.IsMatch(text, @"^[\d,\-]+$");
		}

		/// <summary>
		/// �t�@�C�����Ɏg�p�ł��Ȃ��������폜���ĕԂ�
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string RemoveInvalidPathChars(string text)
		{
			return ReplaceInvalidPathChars(text, String.Empty);
		}

		/// <summary>
		/// �t�@�C�����Ɏg�p�ł��Ȃ�������replacement�ɒu�������ĕԂ�
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
