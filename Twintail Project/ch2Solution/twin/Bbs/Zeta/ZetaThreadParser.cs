// ZetaThreadListParser.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using Twin.Text;

	/// <summary>
	/// Zetabbs�̃X���b�h����� (2ch�݊�)
	/// </summary>
	public class ZetaThreadParser : X2chThreadParser
	{
		/// <summary>
		/// ID���������邽�߂̐��K�\��
		/// </summary>
		private readonly Regex ZetaIDRegex =
			new Regex(@"(TC:(?<id>[^\s]+))", RegexOptions.Compiled);

//		/// <summary>
//		/// ���t���������邽�߂̐��K�\��
//		/// </summary>
//		private readonly Regex ZetaDateRegex =
//			new Regex(@"(?<date>\d+/\d+/\d+ \(.+?\) \d+:\d+:\d+)", RegexOptions.Compiled);

		/// <summary>
		/// ZetaThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		public ZetaThreadParser()
			: base(BbsType.Zeta, Encoding.GetEncoding("Shift_Jis"))
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
			List<ResSet> list = new List<ResSet>(300);
			int begin = 0, index;

			while ((index = searcher.Search(dataText, begin)) != -1)
			{
				string lineData = dataText.Substring(begin, index - begin);
				begin = index + searcher.Pattern.Length;
		
				ResSet resSet = new ResSet(-1, "[�������Ă܂�]",
					String.Empty, "[�������Ă܂�]", "[�������Ă܂�]");

				string[] elements = Regex.Split(lineData, "<>");

				if (elements.Length >= 4)
				{
					try {
						// ID���擾
						Match idmatch = ZetaIDRegex.Match(elements[2]);
						string id = idmatch.Success ? idmatch.Groups["id"].Value : "";

						// name=0�Aemail=1�Adate=2�Amessage=3�Asubject=4
						resSet.ID = id;
						resSet.Name = elements[0];
						resSet.Email = elements[1];
						resSet.DateString = elements[2];
						resSet.Body = elements[3];

						if (elements.Length >= 5 &&
							elements[4] != String.Empty)
						{
							resSet.Tag = elements[4];
						}
						list.Add(resSet);
					}
					catch (Exception ex) {
						System.Diagnostics.Debug.Write(ex);
					}
				}
			}

			return list.ToArray();
		}
	}
}
