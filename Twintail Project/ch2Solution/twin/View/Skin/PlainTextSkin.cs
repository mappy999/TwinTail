// PlainTextSkin.cs

namespace Twin
{
	using System;
	using System.Text.RegularExpressions;
	using System.Text;
	using Twin.Text;

	/// <summary>
	/// �e�L�X�g�`���̃X�L����\��
	/// </summary>
	public class PlainTextSkin : StandardHtmlSkin
	{
		/// <summary>
		/// �f�t�H���g�̃e�L�X�g�X�L����\��
		/// </summary>
		public static readonly PlainTextSkin Default = new PlainTextSkin();

		private StringBuilder buffer = new StringBuilder(128);

		/// <summary>
		/// �X�L�������擾
		/// </summary>
		public override string Name {
			get { return "plainskin"; }
		}

		/// <summary>
		/// PlainTextSkin�N���X�̃C���X�^���X��������
		/// </summary>
		public PlainTextSkin() : base()
		{
			headerSkin = String.Empty;
			footerSkin = String.Empty;
			resSkin = "<PLAINNUMBER/> ���O�F<NAME/> [<MAIL/>] <DATE/>\r\n<MESSAGE/>";
			newResSkin = "<PLAINNUMBER/> ���O�F<NAME/> [<MAIL/>] <DATE/>\r\n<MESSAGE/>";
		}

		/// <summary>
		/// �w�肵��ResSet��
		/// �ݒ肳��Ă���X�L�����g�p���ĕ�����`���ɕϊ�
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		public override string Convert(ResSet resSet)
		{
			if (!resSet.Visible)
				return String.Empty;

			if (resSet.DateString == "�������ځ[��")
				return String.Empty;

			/*
			if (resSet.IsABone) {
				resSet = ResSet.ABone(resSet, ABoneType.NG, "");
				resSet.Email = String.Empty;
			}*/

			// �g�p����X�L��
			string skinhtml = resSet.IsNew ? newResSkin : resSkin;
			string dateonly, body;

			// �{������tag����菜��
			body = resSet.Body;
			body = Regex.Replace(body, "<a[^>]+>(?<uri>[^<]*)</a>", "${uri}", RegexOptions.IgnoreCase);
			body = Regex.Replace(body, "<(?!br|hr)[^>]+>", "");

			buffer.Append(body);
			buffer.Replace("<br>", "\r\n");
			buffer.Replace("<hr>", "\r\n �\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\\r\n");
			buffer.Replace("&gt;", ">");
			buffer.Replace("&lt;", "<");
			body = buffer.ToString();
			buffer.Remove(0, buffer.Length);

			#region ���t��ID���쐬
			dateonly = resSet.DateString;
			Match m = Regex.Match(resSet.DateString, "( ID:)|(\\[)");

			if (m.Success)
				dateonly = resSet.DateString.Substring(0, m.Index);
			#endregion

#if REGEX_REPLACE
			skinhtml = PlainNumberRegex.Replace(skinhtml, resSet.Index.ToString());
			skinhtml = IDRegex.Replace(skinhtml, resSet.ID);
			skinhtml = NameRegex.Replace(skinhtml, resSet.Name);
			skinhtml = EmailRegex.Replace(skinhtml, resSet.Email);
			skinhtml = DateRegex.Replace(skinhtml, resSet.DateString);
			skinhtml = DateOnlyRegex.Replace(skinhtml, dateonly);
			skinhtml = BodyRegex.Replace(skinhtml, body);
#else
			buffer.Append(skinhtml);
			buffer.Replace("<PLAINNUMBER/>", resSet.Index.ToString());
			buffer.Replace("<ID/>", resSet.ID);
			buffer.Replace("<NAME/>", HtmlTextUtility.RemoveTag(resSet.Name));
			buffer.Replace("<MAIL/>", resSet.Email);
			buffer.Replace("<DATE/>", resSet.DateString);
			buffer.Replace("<DATEONLY/>", dateonly);
			buffer.Replace("<MESSAGE/>", body);
			skinhtml = buffer.ToString();
			buffer.Remove(0, buffer.Length);
#endif

			return skinhtml;
		}
	}
}
