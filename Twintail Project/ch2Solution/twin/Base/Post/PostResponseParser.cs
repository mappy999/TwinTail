// PostResponseParser.cs

namespace Twin
{
	using System;
	using System.Text.RegularExpressions;
	using System.Diagnostics;
using System.Collections.Generic;

	/// <summary>
	/// �T�[�o�[����̃��X�|���X���b�Z�[�W����͂���@�\���
	/// </summary>
	public class PostResponseParser
	{
		protected string title;
		protected string plainText;
		protected string htmlText;
		protected int sambaCount;
		protected PostResponse response;

		/// <summary>
		/// �^�C�g�����擾
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
		}

		/// <summary>
		/// Html�`���̖{�����擾
		/// </summary>
		public string HtmlText
		{
			get
			{
				return htmlText;
			}
		}

		/// <summary>
		/// �e�L�X�g�`���̖{�����擾
		/// </summary>
		public string PlainText
		{
			get
			{
				return plainText;
			}
		}

		/// <summary>
		/// �T�[�o�[��Samba�b�����擾
		/// </summary>
		public int SambaCount
		{
			get
			{
				return sambaCount;
			}
		}

		/// <summary>
		/// �T�[�o�[����̉��Ώ�Ԃ��擾
		/// </summary>
		public PostResponse Response
		{
			get
			{
				return response;
			}
		}

		private Dictionary<string,string> hiddenParamsDic = new Dictionary<string,string>();
		public Dictionary<string,string> HiddenParams
		{
			get
			{
				return hiddenParamsDic;
			}
		}
	

		/// <summary>
		/// PostResponseParser�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="htmlData">��͂���f�[�^</param>
		public PostResponseParser(string htmlData)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			htmlText = htmlData;
			response = PostResponse.Error;
			sambaCount = -1;
			plainText = Parse(htmlData);
		}

		/// <summary>
		/// text�����
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		protected virtual string Parse(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			try
			{
				// 2ch_X���������鐳�K�\��
				Regex regex2chx = new Regex(@"<!--\s*?(2ch_X:\w+)\s*?-->",
					RegexOptions.IgnoreCase | RegexOptions.Singleline);

				// �^�C�g�����������鐳�K�\��
				Regex regext = new Regex(@"<title>(?<t>.+?)</title>",
					RegexOptions.IgnoreCase | RegexOptions.Singleline);

				// �{�����������鐳�K�\��
				Regex regexb = new Regex(@"(<body.*?>(?<b>.+)</body>)|(</head>(?<b>.+)</body>)",
					RegexOptions.IgnoreCase | RegexOptions.Singleline);

				// �^�C�g�����擾
				Match match0 = regext.Match(text);
				if (match0.Success)
				{
					title = match0.Groups["t"].Value;
					response = TitleToStatus(title);
				}

				// 2ch_X���擾
				Match match1 = regex2chx.Match(text);
				if (match1.Success)
				{
					response = x2chXToStatus(match1.Value);
				}

				// �{�����擾�i�^�O����菜���j
				Match match2 = regexb.Match(text);
				if (match2.Success)
				{
					string result = match2.Groups["b"].Value;

					if (result.IndexOf("�������݊m�F") != -1)
						response = PostResponse.Cookie;

					else if (result.IndexOf("�N�b�L�[���I���ɂ��Ă���") != -1)
						response = PostResponse.Cookie;

					else if (result.IndexOf("�d�q�q�n�q - 593") != -1)
					{
						response = PostResponse.Samba;

						Match m = Regex.Match(result, "593 (?<cnt>\\d+) sec");
						if (m.Success)
							Int32.TryParse(m.Groups["cnt"].Value, out sambaCount);
					}

					result = Regex.Replace(result, "<br>", "\r\n", RegexOptions.IgnoreCase);
					result = Regex.Replace(result, "<hr>", "\r\n", RegexOptions.IgnoreCase);
					result = Regex.Replace(result, "</?[^>]+>", "");
					result = Regex.Replace(result, "\t", "");
					result = Regex.Replace(result, "(\r\n|\r|\n){2,}", "\r\n", RegexOptions.Singleline);
					plainText = result;


					if (response == PostResponse.Cookie)
					{
						MatchCollection matches = Regex.Matches(text, "<input type=hidden name=\"?(\\w+?)\"? value=\"?(\\w+?)\"?>", RegexOptions.IgnoreCase);

						foreach (Match m in matches)
						{
							string name = m.Groups[1].Value;
							string value = m.Groups[2].Value;

							hiddenParamsDic.Add(name, value);
#if DEBUG
							Console.WriteLine("{0}={1}", name, value);
#endif
						}
					}
				}

				if (plainText == String.Empty)
					plainText = htmlText;
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex.ToString());
			}

			return plainText;
		}

		/// <summary>
		/// 2ch_X��PostResponse�񋓑̂Ŏ擾
		/// </summary>
		/// <param name="x2chx"></param>
		/// <returns></returns>
		public static PostResponse x2chXToStatus(string x2chx)
		{
			/* http://members.jcom.home.ne.jp/monazilla/document/2ch_x.html
			�P�A�G���[�\���Ƀ^�O�����܂����B 
�@				<html>�̒����<!-- 2ch_X:***** -->�Ƃ����`�œ����Ă��܂��B 
�@				��ʂɂ��Ă͈ȉ��̒ʂ�ł��B 
�@�@�@			����I���F<!-- 2ch_X:true -->�i����ɏ������݂��I���j 
�@�@�@			���ӏI���F<!-- 2ch_X:false -->�i�������݂͂��������ӂ��j 
�@�@�@			�G���[�\���F<!-- 2ch_X:error -->�i���͂d�q�q�n�q�I�̃^�C�g���j 
�@�@�@			�������݊m�F�F<!-- 2ch_X:check -->�i�X�����ĂȂǏ������ݕʉ�ʁj 
�@�@�@			�N�b�L�[�m�F�F<!-- 2ch_X:cookie -->�i�N�b�L�[��H�ׂ������ʁj 
			�Q�A���ʋK���𒍈Ӕ����̏ꍇ��������܂����B 
�@				�����b���Œ��ӂ���̂ł͂Ȃ��A���̋K������������قǍ���ł��鎞�ԑтɁA 
�@				�`�`�Ȃǂ̑�ʏ������݂�A���������݂ȂǁA���ׂ��グ��s�ׂ������ꍇ�A 
�@				�������݃��O���������A�G���[�ŏ������߂܂���B 
�@				���O�C�����Ă���l�͊ɘa�����̂ŁA���܂�c�[���ɂ͊֌W����܂��񂪁A 
�@				�G���[�ɂȂ����ꍇ�̃A�N�Z�X�K�������͓����Ȃ̂ŁA 
�@				�K�����ӂ�\�����Ă���������΂Ǝv���܂��B 
�@				�֘A���Ă���͈̂ȉ��̂Q�ł��B 
�@�@�@			<!-- 2ch_X:check -->�F����ȏ㏑���ƃA�N�Z�X�֎~�ɂȂ�܂��B�B�B 
�@�@�@			<!-- 2ch_X:false -->�F�������݂͊������܂������A�ȉ��̒��ӂ��o�Ă��܂��B�B�B 
			*/

			Match m = Regex.Match(x2chx, "(?<a>\\w+:\\w+)");
			if (m.Success)
			{
				string a = m.Groups["a"].Value;

				switch (a)
				{
					case "2ch_X:true":
						return PostResponse.Success;
					case "2ch_X:false":
						return PostResponse.Attention;
					case "2ch_X:error":
						return PostResponse.Error;
					case "2ch_X:check":
						return PostResponse.Warning;
					case "2ch_X:cookie":
						return PostResponse.Cookie;
				}
			}
			return PostResponse.None;
		}

		/// <summary>
		/// �^�C�g����PostResponse�񋓑̂Ŏ擾
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public static PostResponse TitleToStatus(string title)
		{
			if (title.IndexOf("�������݂܂���") >= 0)
			{
				return PostResponse.Success;
			}

			if (title.IndexOf("�������݊m�F") >= 0 ||
				title.IndexOf("�N�b�L�[�m�F") >= 0 ||
				title.IndexOf("���e�m�F") >= 0)
			{
				return PostResponse.Cookie;
			}

			if (title.IndexOf("�d�q�q�n�q") >= 0 ||
				title.IndexOf("�����ł����݂܂��傤") >= 0)
			{
				return PostResponse.Error;
			}

			return PostResponse.None;

			/*
			switch (title)
			{
			case "�������݂܂����B":
				return PostResponse.Success;

			case "�� �������݊m�F ��":
			case "�N�b�L�[�m�F�I":
			case "�������݊m�F":
			case "�������݊m�F�B":
			case "���e�m�F":
				return PostResponse.Cookie;

			case "�d�q�q�n�q�I":
			case "�����ł����݂܂��傤�B":
				return PostResponse.Error;

			default:
				return PostResponse.None;
			}
			*/
		}
	}
}
