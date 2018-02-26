// X2chThreadListParser.cs

namespace Twin.Bbs
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using Twin.Text;

	/// <summary>
	/// 2ch��subject.txt����������
	/// </summary>
	public class X2chThreadListParser : ThreadListParser
	{
		// �X���b�h�ꗗ����͂��鐳�K�\��
		protected static readonly Regex Pattern =
			new Regex(@"^(?<key>.+?)<>(?<subj>.+?)\((?<res>\d+)\)\r?$",
			RegexOptions.Multiline | RegexOptions.Compiled);

		/// <summary>
		/// �f���̌^�ƃG���R�[�f�B���O���w�肵�āA
		/// X2chThreadListParser�N���X�̃C���X�^���X���������B
		/// </summary>
		/// <param name="bbs"></param>
		/// <param name="enc"></param>
		public X2chThreadListParser(BbsType bbs, Encoding enc) : base(bbs, enc)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// X2chThreadListParser�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chThreadListParser() 
			: base(BbsType.X2ch, Encoding.GetEncoding("Shift_Jis"))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		protected override ThreadHeader[] ParseData(string dataText)
		{
			List<ThreadHeader> list = new List<ThreadHeader>(100);

			try {
				MatchCollection matches = Pattern.Matches(dataText);

				foreach (Match m in matches)
				{
					ThreadHeader header = TypeCreator.CreateThreadHeader(bbsType);
					header.Key = Path.GetFileNameWithoutExtension(m.Groups["key"].Value);
					header.Subject = m.Groups["subj"].Value;
					
					int resCnt;
					if (Int32.TryParse(m.Groups["res"].Value, out resCnt))
						header.ResCount = resCnt;

					list.Add(header);
				}
			}
			catch {}

			return list.ToArray();
		}

		protected override int GetEndToken(byte[] data, int index, int length, out int tokenLength)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (index < 0)
				throw new ArgumentOutOfRangeException("index");

			for (int i = length-1; i >= 0; i--)
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
