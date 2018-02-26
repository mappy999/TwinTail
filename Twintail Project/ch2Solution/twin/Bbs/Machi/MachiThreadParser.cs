// MachiThreadParser.cs

namespace Twin.Bbs
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using CSharpSamples.Text.Search;
	using Twin.Text;

	/// <summary>
	/// �܂�BBS��html�`������������
	/// </summary>
	public class MachiThreadParser : ThreadParser
	{
		/// <summary>
		/// �^�C�g�����������邽�߂̐��K�\��
		/// </summary>
		protected Regex SubjRegex =
			new Regex(@"<title>(?<subj>.*)</title>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>
		/// ���X�{�����������邽�߂̐��K�\��
		/// </summary>
		protected Regex BodyRegex =
			new Regex("<dt>(?<num>\\d+) ���O�F((<font.*?><b>\\s?(?<name>.+?)\\s?</b></font>)|(<a href=\"mailto:(?<email>.+?)\"><b>\\s?(?<name>.+?)\\s?</B></a>))\\s*?���e���F(?<date>.+?)<br><dd>\\s?(?<body>.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
		
		protected Regex IDHostRegex =
			new Regex(@"ID:(?<idhost>(?<id>[^\s]+).+?\[(?<host>.+?)])</font>", RegexOptions.Compiled);

		/// <summary>
		/// a�^�O���������鐳�K�\��
		/// </summary>
		protected Regex AHrefRegex =
			new Regex(@"</?a.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>
		/// ���X�J�n�ʒu���������邽�߂̌����N���X��������
		/// </summary>
		protected ISearchable headSrch = new BmSearch2("<dt>");

		/// <summary>
		/// ���X�I���ʒu���������邽�߂̌����N���X��������
		/// </summary>
		protected ISearchable tailSrch = new BmSearch2("<br><br>\n");

		private string subject;
		private int totalCount;

		/// <summary>
		/// �J�n�C���f�b�N�X���擾�܂��͐ݒ�
		/// </summary>
		public int StartIndex {
			set { totalCount = value; }
			get { return totalCount; }
		}

		/// <summary>
		/// �f���̌^�ƃG���R�[�f�B���O���w�肵�āA
		/// MachiThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiThreadParser(BbsType bbs, Encoding enc) : base(bbs, enc)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			subject = String.Empty;
			totalCount = 1;
		}

		/// <summary>
		/// MachiThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiThreadParser() : this(BbsType.Machi, Encoding.GetEncoding("Shift_Jis"))
		{
		}

		protected override ResSet[] ParseData(string dataText)
		{
			if (dataText == null)
				throw new ArgumentNullException("dataText");

			// 2011/06/11 Mizutama
			dataText = dataText.Replace('\0', '*');

			List<ResSet> list = new List<ResSet>(300);
			int begin = 0, index;
			
			// �X���b�h��������
			Match m = SubjRegex.Match(dataText);
			if (m.Success) subject = m.Groups["subj"].Value;

			// ���X�J�n�ʒu������
			while (begin < dataText.Length && 
				(index = headSrch.Search(dataText, begin)) != -1)
			{
				// ���X�J�n�ʒu���烌�X�I���ʒu������
				begin = index;
				int end = tailSrch.Search(dataText, begin);

				if (end != -1)
				{
					string lineData = dataText.Substring(begin, end - begin);
					begin = (end + tailSrch.Pattern.Length);

					ResSet res = ParseResSet(lineData);

					if (res.Index == 1)
						res.Tag = subject;

					// ��͂������Ǝ��ۂ̃��X�ԍ����Ⴆ�΂��ځ[�񂳂�Ă���Ǝv����̂ŁA
					// ���̌����߂Ƃ��Ă��ځ[�񃌃X��}��
					if (totalCount != res.Index)
					{
						int length = res.Index - totalCount;
						totalCount += length;

						for (int i = 0; i < length; i++)
							list.Add(ResSet.ABoneResSet);
					} 

					totalCount++;
					list.Add(res);
				}
				else break;
			}

			return list.ToArray();
		}

		protected virtual ResSet ParseResSet(string data)
		{
			ResSet resSet = new ResSet(-1, "[�������Ă܂�]",
				String.Empty, "[�������Ă܂�]", "[�������Ă܂�]");

			Match m = BodyRegex.Match(data);
			if (m.Success)
			{
				int index;
				Int32.TryParse(m.Groups["num"].Value, out index);

				resSet.Index = index;
				resSet.DateString = m.Groups["date"].Value.Trim();
				resSet.Name = m.Groups["name"].Value;
				resSet.Body = m.Groups["body"].Value;
				resSet.Email = m.Groups["email"].Value;

				// ���t�̉��s���폜
				resSet.DateString = resSet.DateString.Replace("\n", "");

				// �{���̃����N���폜���A���X�Q�ƂɃ����N�𒣂�
				resSet.Body = AHrefRegex.Replace(resSet.Body, String.Empty);
				//resSet.Body = HtmlTextUtility.RefRegex.Replace(resSet.Body, "<a href=\"/${num}\" target=\"_blank\">${ref}</a>");
			}

			return resSet;
		}

		protected override int GetEndToken(byte[] data, int index, int length, out int tokenLength)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (index < 0)
				throw new ArgumentOutOfRangeException("index");

			const string key = "<br><br>\n";

			for (int i = length - key.Length; i >= 0; i--)
			{
				int pos;

				for (pos = 0; pos < key.Length; pos++)
					if (data[i+pos] != key[pos]) break;

				if (pos == key.Length)
				{
					tokenLength = key.Length;
					return i;
				}
			}

			tokenLength = 0;
			return -1;
		}
	}
}
