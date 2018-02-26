// HtmlSkin.cs

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
	/// 2ch��html�`���̕ϊ��������s��
	/// </summary>
	public class HtmlSkin : ThreadSkinBase
	{
		protected string headerSkin;
		protected string footerSkin;
		protected string resSkin;
		private string skinPath;

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
			get { return "htmlSkin"; }
		}

		/// <summary>
		/// HtmlSkin�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlSkin()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			headerSkin = "<html><head>" +
						"<meta http-equiv=\"Content-Type\" content=\"text/html; charset=Shift_JIS\">" +
						"<title><THREADNAME/></title>" +
						"</head>" +
						"<body bgcolor=#efefef text=black link=blue alink=red vlink=#660099>" +
						"<p><font size=+1 color=red><THREADNAME/></font>" +
						"<dl>";

			resSkin = "<dt><a name=\"<PLAINNUMBER/>\"><PLAINNUMBER/></a> <MAILNAME/> �F<DATE/></dt><dd><MESSAGE/><br><br></dd>";
			footerSkin = "</dl></body></html>";
			baseUri = "#";
			skinPath = "";
		}

		/// <summary>
		/// �X�L����ǂݍ���
		/// </summary>
		/// <param name="skinFolder"></param>
		public override void Load(string skinFolder)
		{
			headerSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "Header.html"));

			footerSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "Footer.html"));

			resSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "Res.html"));

			this.skinPath = skinFolder;

			// �Ōオ�X���b�V���L���ŏI����Ă��Ȃ���Εt������
			if (!skinPath.EndsWith("\\"))
				skinPath += "\\";
		}

		public override void Reset()
		{
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

		/// <summary>
		/// �w�肵��ResSet��
		/// �ݒ肳��Ă���X�L�����g�p���ĕ�����`���ɕϊ�
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		protected virtual string Convert(string skinhtml, ResSet resSet)
		{
			StringBuilder sb = new StringBuilder(2048);
			string name;
			string mailname;
			string dateonly, dateString;
			string body;

			#region ���O�̍쐬
			sb.Append("<b>");
			sb.Append(resSet.Name);
			sb.Append("</b>");
			name = sb.ToString();
			sb.Remove(0, sb.Length);
			#endregion

			#region Email�t�����O�̍쐬
			if (resSet.Email != String.Empty)
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
			Match m = Regex.Match(resSet.DateString, "( ID:)|(\\[)");

			if (m.Success)
			{
				dateonly = resSet.DateString.Substring(0, m.Index);
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
			body = HtmlTextUtility.RemoveTag(resSet.Body, "a");
			body = HtmlTextUtility.Linking(body);
			#endregion

			#region ���X�Q�Ƃ��쐬
			body = HtmlTextUtility.RefRegex.Replace(body, "<a href=\"" + baseUri + "${num}\" target=\"_blank\">${ref}</a>");
			body = HtmlTextUtility.ExRefRegex.Replace(body, "<a href=\"" + baseUri + "${num}\" target=\"_blank\">${num}</a>");
			#endregion

			sb.Remove(0, sb.Length);
			sb.Append(skinhtml);
			sb.Replace("<PLAINNUMBER/>", resSet.Index.ToString());
			sb.Replace("<NUMBER/>", resSet.Index.ToString());
			sb.Replace("<MAILNAME/>", mailname);
			sb.Replace("<ID/>", resSet.ID);
			sb.Replace("<BE/>", resSet.BeLink);
			sb.Replace("<NAME/>", name);
			sb.Replace("<MAIL/>", resSet.Email);
			sb.Replace("<DATE/>", dateString);
			sb.Replace("<DATEONLY/>", dateonly);
			sb.Replace("<MESSAGE/>", body);
			sb.Replace("<SKINPATH/>", skinPath);

			skinhtml = sb.ToString();

			return skinhtml;
		}

		/// <summary>
		/// �w�肵��ResSet��
		/// �ݒ肳��Ă���X�L�����g�p���ĕ�����`���ɕϊ�
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		public override string Convert(ResSet resSet)
		{
			return Convert(resSkin, resSet);
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
			StringBuilder sb = new StringBuilder(512 * resSetCollection.Count);

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
	}
}
