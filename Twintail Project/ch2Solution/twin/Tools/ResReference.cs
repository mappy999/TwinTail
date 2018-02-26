// ResReference.cs

namespace Twin.Tools
{
	using System;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Collections;
	using Twin.Text;

	/// <summary>
	/// ���X�Q�ƂɊ֘A�����@�\���
	/// </summary>
	public class ResReference
	{
		/// <summary>
		/// �Q�ƃA���J�[ >>XX ���������鐳�K�\��
		/// </summary>
		public static readonly Regex RefAnchor =
			new Regex(@"(��|&gt;)(?<num>[,\d\-\+]+)", RegexOptions.Compiled);

		/// <summary>
		/// ���X�Q�Ƃ̔ԍ�(��: 1,2,3,4-5,6+7 �`��)���������邽�߂̐��K�\��
		/// </summary>
		public static readonly Regex RefRegex =
			new Regex(@"(?<num>[,\d\-\+]+)n?$", RegexOptions.Compiled);

		public static readonly Regex ParseRegex =
			new Regex(@"(?<num>\d+\-?\d*)$", RegexOptions.Compiled);

		/// <summary>
		/// text���Ɋ܂܂�Ă��郌�X�Q�Ƃ̔ԍ������ׂĐ��l�z��ɕϊ�
		/// (��: http://.../10-20 �� 10,11,12...20)
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static int[] GetArray(string text)
		{
			if (text == null) {
				throw new ArgumentNullException("text");
			}

			if (Regex.IsMatch(text, @"/\d{5,}/?$") || Regex.IsMatch(text, "(=|l)\\d+$"))// "/50l" �Ȃǂ͖�������
				return (new int[0]);

			ArrayList list = new ArrayList();

			Match m = RefRegex.Match(
				HtmlTextUtility.ZenToHan(text));

			if (m.Success)
			{
				string[] numbers = m.Groups["num"].Value.Split(",+".ToCharArray());

				foreach (string num in numbers)
				{
					string[] array = num.Split('-');
					if (array.Length == 2)
					{
						int st=0, ed=0;

						// �������ǂ������`�F�b�N
						if (Int32.TryParse(array[0], out st))
						{
							// "100-" (100�Ԗڈȍ~) �Ƃ��������̏ꍇ�Aarray[1] �ɂ͋󕶎��񂪊i�[�����B
							// ���̌`���̏ꍇ�� 100�Ԗڂ���Ō�̃��X(1001�Ԗ�)�܂ł��܂߂�悤�ɂ���
							if (array[1] == String.Empty)
								ed = 1001;
							else
								Int32.TryParse(array[1], out ed);

							if (st >= 1 && (ed - st) <= 1000)
							{
								for (int i = st; i <= ed; i++)
									list.Add(i);
							}
						}
					}
					else if (array.Length == 1)
					{
						if (HtmlTextUtility.IsDigit(array[0]))
						{
							int n;
							if (Int32.TryParse(array[0], out n))
								list.Add(n);
						}
					}
				}
			}
			return (int[])list.ToArray(typeof(int));
		}
	}
}
