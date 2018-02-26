// X2chThreadParser.cs

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
	/// 2ch��Dat�`������������
	/// </summary>
	public class X2chThreadParser : ThreadParser
	{
		//		/// <summary>
		//		/// ���t���������邽�߂̐��K�\��
		//		/// </summary>
		//		protected static readonly Regex DateRegex =
		//			new Regex(@"(?<date>[\d\s/:\(\)\p{IsCJKUnifiedIdeographs}]+)", RegexOptions.Compiled);

		/// <summary>
		/// ���s���������邽�߂̌����N���X��������
		/// </summary>
		protected readonly ISearchable searcher = new KmpSearch("\n");
		/// <summary>
		/// <>���������邽�߂̌����N���X��������
		/// </summary>
		protected readonly ISearchable s_token = new KmpSearch("<>");

		protected string[] elements;

		protected int resCount = 0;


		//		/// <summary>
		//		/// <>���������邽�߂̌����N���X��������
		//		/// </summary>
		//		protected static readonly Regex splitRegex = new Regex("<>", RegexOptions.Compiled);

		/// <summary>
		/// �f���̌^�ƃG���R�[�f�B���O���w�肵�āA
		/// X2chThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="enc"></param>
		public X2chThreadParser(BbsType bbs, Encoding enc)
			: base(bbs, enc)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			elements = new string[5];
			resCount = 0;
		}

		/// <summary>
		/// X2chThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chThreadParser()
			: this(BbsType.X2ch, Encoding.GetEncoding("Shift_Jis"))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		protected override ResSet[] ParseData(string dataText)
		{
			if (dataText == null)
				throw new ArgumentNullException("dataText");

			//string[] lines = Regex.Split(dataText, "\r\n|\r|\n");
			List<ResSet> list = new List<ResSet>(100);
			int begin = 0, index;

			// ���X����'\0'�������܂ޏꍇ������̂ŁA�����*�ɒu��������
			if (dataText.IndexOf('\0') >= 0)
				dataText = dataText.Replace('\0', '*');

			// �������X�t�H�[�}�b�g�̏ꍇ�̓��ꏈ��
			if (dataText.IndexOf("<>") < 0)
				dataText = dataText.Replace(",", "<>");

			while ((index = searcher.Search(dataText, begin)) != -1)
			{
				string lineData = dataText.Substring(begin, index - begin);

				if (list.Count >= 200)
				{
					string s = "hoge";
					if (s == "hoge")
					{
					}
				}

				// ���̌����J�n�ʒu��ݒ�
				begin = index + searcher.Pattern.Length;

				// ���K�\���g��Ȃ���������
				//string[] elements = splitRegex.Split(lineData);
				
				for (int i = 0; i < elements.Length; i++)
					elements[i] = String.Empty;

				int ret, beg = 0, idx = 0;
				while ((ret = s_token.Search(lineData, beg)) != -1 && idx < elements.Length)
				{
					elements[idx++] = lineData.Substring(beg, ret - beg);
					beg = ret + 2; // 2 == "<>".Length
				}
				if (beg < lineData.Length && idx < elements.Length)
					elements[idx] = lineData.Substring(beg, lineData.Length - beg);
				// -----------------------

				resCount++;

				list.Add(ParseResSet(elements));
			}

			return list.ToArray();
		}

		protected virtual ResSet ParseResSet(string[] elements)
		{
			ResSet resSet = new ResSet(-1, "[�������Ă܂�]",
				String.Empty, "[�������Ă܂�]", "[�������Ă܂�]");

			try
			{
				// name=0�Aemail=1�Adate=2�Amessage=3�Asubject=4
				resSet.Name = elements[0];
				resSet.Email = elements[1];
				resSet.DateString = elements[2];
				resSet.Body = elements[3];//HtmlTextUtility.RefRegex.Replace(elements[3], "<a href=\"../${num}\" target=\"_blank\">${ref}</a>");

				if (elements.Length >= 5 &&
					elements[4] != String.Empty)
				{
					resSet.Tag = elements[4];
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Write(ex);
			}

			return resSet;
		}

		protected override int GetEndToken(byte[] data, int index, int length, out int tokenLength)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (index < 0)
				throw new ArgumentOutOfRangeException("index");

			for (int i = length - 1; i >= 0; i--)
			{
				if (data[i] == '\n')
				{
					tokenLength = 1;
					return i;
				}
			}

			tokenLength = 0;
			return -1;
		}
	}
}
