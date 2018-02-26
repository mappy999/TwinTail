// JbbsThreadParser.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;

	/// <summary>
	/// JbbsThreadParser �̊T�v�̐����ł��B
	/// </summary>
	public class JbbsThreadParser : X2chThreadParser
	{
		/// <summary>
		/// JbbsThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		public JbbsThreadParser()
			: base(BbsType.Jbbs, Encoding.GetEncoding("EUC-JP"))
		{
			base.elements = new string[7];

			// rawmode.cgi �t�H�[�}�b�g
			// http://blog.livedoor.jp/bbsnews/archives/50283526.html
			// [���X�ԍ�]<>[���O]<>[���[��]<>[���t]<>[�{��]<>[�X���b�h�^�C�g��]<>[ID]

			// BodyRegex =
			//	new Regex("(?<num>.+?)<>(?<name>.+?)<>(?<email>.+?)<>(?<date>.+?)<>(?<body>.+?)<>(?<threadname>.*?)<>(?<id>.+?)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);


			// read.cgi �t�H�[�}�b�g
			//BodyRegex =
			//    new Regex("<dt><a.*?>(?<num>\\d+)</a> ���O�F((<font.*?><b>\\s?(?<name>.+?)\\s?</b></font>)|(<a href=\"mailto:(?<email>.+?)\"><b>\\s?(?<name>.+?)\\s?</B></a>))\\s*?���e���F\\s*(?<date>.+?)<dd>(?<body>.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
		}

		protected override ResSet ParseResSet(string[] elements)
		{
			ResSet resSet = new ResSet(-1, "[�������Ă܂�]",
				String.Empty, "[�������Ă܂�]", "[�������Ă܂�]");

			// [���X�ԍ�]<>[���O]<>[���[��]<>[���t]<>[�{��]<>[�X���b�h�^�C�g��]<>[ID]

			int index;
			Int32.TryParse(elements[0], out index);

			resSet.Index = index;
			resSet.Name = elements[1];
			resSet.Email = elements[2];
			resSet.DateString = String.Concat(elements[3], " ID:", elements[6]);
			resSet.Body = elements[4];
			resSet.Tag = elements[5];
			resSet.ID = elements[6];

			return resSet;
		}
	}
}
