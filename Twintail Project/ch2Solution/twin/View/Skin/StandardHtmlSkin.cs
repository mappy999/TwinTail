// StandardHtmlSkin.cs

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections;
	using System.Diagnostics;
	using Twin.Util;
	using Twin.Text;

	/// <summary>
	/// �X�L���W�����v���W�F�N�g (http://dtao.cside.com/stdskin/)�݊��X�L����\��
	/// (REGEX_REPLACE���`�����ꍇ�̂݁A������X�L���ɂ��݊�)
	/// </summary>
	public class StandardHtmlSkin : ThreadSkinBase
	{
		private static Regex rexBRSpace = new Regex(" <br> ", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		protected const int Capacity = 512;

		protected string skinPath;
		protected string bookmarkSkin;
		protected string headerSkin;
		protected string footerSkin;
		protected string newResSkin;
		protected string resSkin;

		private bool namePopup;
		private string baseUri;

		/// <summary>
		/// ���X�Q�Ƃ̊�{�ƂȂ�URL���擾�܂��͐ݒ�
		/// </summary>
		public override string BaseUri {
			set {
				if (value == null)
					throw new ArgumentNullException("BaseUri");

				baseUri = value;

				if (!baseUri.EndsWith("/"))
					baseUri += "/";
			}
			get { return baseUri; }
		}

		// �X�L�������擾
		public override string Name {
			get { return "stdskin"; }
		}

		/// <summary>
		/// ���O���|�b�v�A�b�v
		/// </summary>
		public bool NamePopup {
			set {
				if (namePopup != value)
					namePopup = value;
			}
			get { return namePopup; }
		}

		/// <summary>
		/// StandardHtmlSkin�N���X�̃C���X�^���X��������
		/// </summary>
		public StandardHtmlSkin()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			skinPath = String.Empty;
			headerSkin = "<html><head><title><THREADNAME/></title></head><b><font color=red><THREADNAME/></font></b><br><br><dl>";
			footerSkin = "</dl></body></html>";
			newResSkin = "<dt><b><NUMBER/></b> ���O�F<font color=\"forestgreen\"><MAILNAME/></font> ���e���F<DATE/></dt><dd><MESSAGE/><br><br></dd></dt>";
			resSkin = "<dt><NUMBER/> ���O�F<font color=\"forestgreen\"><MAILNAME/></font> ���e���F<DATE/></dt><dd><MESSAGE/><br><br></dd></dt>";
			bookmarkSkin = "</dl><span style=\"font-size:8pt; ; text-align: center; color: #ffffff; background-color: #70cc70; border: 1px solid forestgreen; width: 100%;\">�R�R�܂œǂ񂾂�</span><dl>";
			baseUri = String.Empty;
		}

		/// <summary>
		/// HtmlThreadSkin�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="folder">�ǂݍ��ރX�L�������݂���t�H���_�p�X</param>
		public StandardHtmlSkin(string folder) : this()
		{
			Load(folder);
		}

		/// <summary>
		/// �X�L����ǂݍ���
		/// </summary>
		/// <param name="skinFolder"></param>
		public override void Load(string skinFolder)
		{
			if (skinFolder == null) {
				throw new ArgumentNullException("skinFolder");
			}

			headerSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "Header.html"));

			footerSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "Footer.html"));

			resSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "Res.html"));

			newResSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "NewRes.html"));

			bookmarkSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "Bookmark.html"));

			// �Ōオ�X���b�V���L���ŏI����Ă��Ȃ���Εt������
			if (!skinFolder.EndsWith("\\"))
				skinFolder += "\\";

			skinPath = skinFolder;
		}

		/// <summary>
		/// �w�b�_�[�ƃt�b�^�[���ʂ̒u�������֐�
		/// </summary>
		/// <param name="skinhtml"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string ReplaceHeaderFooter(string skinhtml, ThreadHeader header)
		{
			ThreadHeader h = header;
			BoardInfo b = h.BoardInfo;
			string result = skinhtml;

			StringBuilder sb = new StringBuilder(512);
			sb.Append(result);
			sb.Replace("<BOARDNAME/>", b.Name);
			sb.Replace("<BOARDURL/>", b.Url);
			sb.Replace("<THREADNAME/>", h.Subject);
			sb.Replace("<THREADURL/>", h.Url);
			sb.Replace("<ALLRESCOUNT/>", h.ResCount.ToString());
			sb.Replace("<NEWRESCOUNT/>", h.NewResCount.ToString());
			sb.Replace("<GETRESCOUNT/>", (h.GotByteCount - h.NewResCount).ToString());
			sb.Replace("<SKINPATH/>", skinPath);
			sb.Replace("<LASTMODIFIED/>", h.LastModified.ToString());
			sb.Replace("<SIZEKB/>", (h.GotByteCount / 1024).ToString());
			sb.Replace("<SIZE/>", h.GotByteCount.ToString());
			result = sb.ToString();

			return result;
		}

		/// <summary>
		/// �w�b�_�[�X�L�����擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public override string GetHeader(ThreadHeader header)
		{
			return ReplaceHeaderFooter(headerSkin, header);
		}

		/// <summary>
		/// �t�b�^�[�X�L�����擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public override string GetFooter(ThreadHeader header)
		{
			return ReplaceHeaderFooter(footerSkin, header);
		}

		private StringBuilder CreateHtml(string skinhtml, ResSet resSet)
		{
			StringBuilder sb = new StringBuilder(2048);
			string number;
			string name;
			string mailname;
			string dateonly, dateString;
			string body;

			#region ���j���[�t���ԍ��̍쐬
			int iPos = -1;

			sb.Append("<a href=\"menu:");
			sb.Append(resSet.Index);
			sb.Append("\" name=\"");
			sb.Append(resSet.Index);
			sb.Append("\" target=\"_blank\">");
			iPos = sb.Length;
			sb.Append(resSet.Index);

			if (resSet.IsServerAboned)
			{
				sb.Insert(iPos, "<font color=lime><i>");
				sb.Append("</i></font>");
			}

			sb.Append("</a>");

			number = sb.ToString();
			sb.Remove(0, sb.Length);
			#endregion

			#region ���O�̍쐬
			sb.Append("<b>");
			sb.Append(resSet.Name);
			sb.Append("</b>");
			name = sb.ToString();
			sb.Remove(0, sb.Length);
			#endregion

			#region ���O���|�b�v�A�b�v
			string _resName = String.Empty;
			if (!String.IsNullOrEmpty(resSet.Name))
			{
				if (namePopup && Char.IsDigit(resSet.Name[resSet.Name.Length - 1]))
					_resName = Regex.Replace(resSet.Name, ">|��|&gt;", String.Empty);
			}

			// ���O���������Ȃ烊���N��\��
			if (namePopup && HtmlTextUtility.IsDigit(_resName)) // �Ō�̕����������̂Ƃ��̂ݏ�������
			{
				sb.Append("<a href=\"");
				sb.Append(baseUri);
				sb.Append(HtmlTextUtility.ZenToHan(_resName));
				sb.Append("\" target=\"_blank\">");
				sb.Append(name);
				sb.Append("</a>");
				name = sb.ToString();
				mailname = name;
				sb.Remove(0, sb.Length);
			}
			#endregion

			#region Email�t�����O�̍쐬
			else if (resSet.Email != String.Empty)
			{
				sb.Append("<a href=\"mailto:");
				sb.Append(resSet.Email);
				sb.Append("\">");
				sb.Append(name);
				sb.Append("</a>");
				mailname = sb.ToString();
				sb.Remove(0, sb.Length);
			}
			else
			{
				mailname = name;
			}
			#endregion

			#region ���t��ID���쐬
			dateString = resSet.DateString;
			dateonly = resSet.DateString;
			Match m = Regex.Match(resSet.DateString, @"\d{2,4}/\d{2}/\d{2}(\(\w\))?\s\d{2}:\d{2}(:\d{2}.\d{2}|:\d{2})?(\s[0-9a-zA-GJ-Z])?");

			if (m.Success)
			{
				dateonly = m.Value;
			}
			#endregion

			#region Be2chID�̃����N��\��
			// BE:0123456-# �܂��� <BE:0123456:0> �`���̓����݂���
			dateString =
				Regex.Replace(dateString, @"BE:(?<id>\d+)\-(?<rank>.+)",
				"<a href=\"http://be.2ch.net/test/p.php?i=${id}\" target=\"_blank\">?${rank}</a>", RegexOptions.IgnoreCase);

			dateString =// �ʔ��l�^news�`��
				Regex.Replace(dateString, @"<BE:(?<id>\d+):(?<rank>.+)>",
				"<a href=\"http://be.2ch.net/test/p.php?i=${id}\" target=\"_blank\">Lv.${rank}</a>", RegexOptions.IgnoreCase);
			#endregion

			#region �{�����쐬
			body = HtmlTextUtility.RemoveTag(resSet.Body, "a|font");
			body = rexBRSpace.Replace(body, "<BR>");
			body = HtmlTextUtility.Linking(body);
			#endregion

			#region ���X�Q�Ƃ��쐬
			body = HtmlTextUtility.RefRegex.Replace(body, "<a href=\"" + baseUri + "${num}\" target=\"_blank\" name=\"res" + resSet.Index + "_ref${num}\">${ref}</a>");
			// body = HtmlTextUtility.ExRefRegex.Replace(body, "<a href=\"" + baseUri + "${num}\" target=\"_blank\">${num}</a>"); �@// 2011.12.16 ���ʂ���
			#endregion

			#region ���̂ق��̒u����������
			sb.Remove(0, sb.Length);
			sb.Append(skinhtml);
			sb.Replace("<PLAINNUMBER/>", resSet.Index.ToString());
			sb.Replace("<MAILNAME/>", mailname);
			sb.Replace("<NUMBER/>", number);
			sb.Replace("<ID/>", resSet.ID);
			sb.Replace("<BE/>", resSet.BeLink);
			sb.Replace("<NAME/>", name);
			sb.Replace("<MAIL/>", resSet.Email);
			sb.Replace("<DATE/>", dateString);
			sb.Replace("<DATEONLY/>", dateonly);
			sb.Replace("<MESSAGE/>", body);
			sb.Replace("<SKINPATH/>", skinPath);
			sb.Replace("<HOST/>", resSet.Host);
			#endregion

			return sb;
		}

		/// <summary>
		/// �w�肵��ResSet��
		/// �ݒ肳��Ă���X�L�����g�p���ĕ�����`���ɕϊ�
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		protected virtual string Convert(string skinhtml, ResSet resSet)
		{
			StringBuilder sb = null;

			if (!resSet.Visible || resSet.DateString == "�������ځ[��")
			{
				sb = new StringBuilder();
			}
			else
			{
				sb = CreateHtml(skinhtml, resSet);
			}			

			// �����菈��
			if (resSet.Bookmark)
				sb.Append(bookmarkSkin);

			return sb.ToString();
		}

		/// <summary>
		/// �w�肵��ResSet��
		/// �ݒ肳��Ă���X�L�����g�p���ĕ�����`���ɕϊ�
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		public override string Convert(ResSet resSet)
		{
			string skinHtml = resSet.IsNew ? newResSkin : resSkin;
			return Convert(skinHtml, resSet);
		}

		/// <summary>
		/// �w�肵��ResSet�R���N�V������
		/// �ݒ肳��Ă���X�L�����g�p���ĕ�����`���ɕϊ�
		/// </summary>
		/// <param name="resSetCollection"></param>
		/// <returns></returns>
		public override string Convert(ResSetCollection resSetCollection)
		{
			if (resSetCollection == null) {
				throw new ArgumentNullException("resSetCollection");
			}

			// �w�肵�����X���̏����z������蓖�Ă�
			StringBuilder sb = new StringBuilder(Capacity * resSetCollection.Count);

			foreach (ResSet resSet in resSetCollection)
			{
				if (resSet.Visible)
				{
					string result = Convert(resSet);
					sb.Append(result);
				}
			}

			return sb.ToString();
		}

		public override void Reset()
		{
			
		}
	}
}
