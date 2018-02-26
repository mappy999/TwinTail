// Twin2HtmlSkin.cs
// #define REGEX_REPLACE

namespace Twin.Forms
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using Twin.Util;
	using Twin.Text;
	using Twin.Tools;

	/// <summary>
	/// Twin2Ie��p�̃X�L��
	/// </summary>
	public class Twin2IeHtmlSkin : StandardHtmlSkin
	{
#if REGEX_REPLACE
		protected readonly Regex IndexRegex =
			new Regex("&INDEX|<INDEX/>", options);

		protected readonly Regex PathRegex =
			new Regex("&PATH|<PATH/>", options);
#endif

		private static readonly Regex AhrefRegex =
			new Regex("<a href=\"(?<url>https?://.+?)\".*?>h?ttps?://.+?</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static Regex coloringId = new Regex("<COLORINGID(?<attr>.*?)/>", RegexOptions.Compiled);
		private static Regex coloringIdAttribute = new Regex(@"n(?<key>[\d]+?)\=(?<value>[^\s]+)", RegexOptions.Compiled);

		private static Regex IDLinkRegex = new Regex("(?<!\">)ID:(?<id>[a-zA-Z0-9/\\+]{8,9})", RegexOptions.Compiled);
		// (?<!\">)ID: �Ƃ��Ă���̂́A�X�L����ID���o�p�^�O������ł���ꍇ������̂ŁA�������������

		private bool existColoringId = false;

		private Regex exPopupRegex = null;
		public Regex ExtendPopupRegex
		{
			get
			{
				return this.exPopupRegex;
			}
		}

		private string extendPopupStr = null;

		// ID���Ƃɔ����񐔂��L�^���Ă����e�[�u��
		private Dictionary<string, int> idCount = new Dictionary<string, int>();

		public Dictionary<string, int> IDCount
		{
			get
			{
				return idCount;
			}
		}

		private SortedList<int, string> newResIDColors = new SortedList<int, string>();
		private SortedList<int, string> oldResIDColors = new SortedList<int, string>();

		//		private string popHeaderSkin;
		//		private string picSkin;
		//		private string prevSkin;
		//		private string wroteSkin;

		// ���邵���ꂽ���X�ԍ��̃��X�g // v2.3.6.9
		public List<int> SirusiList { get; private set; }


		/// <summary>
		/// �g���|�b�v�A�b�v�p�̕�������擾�܂��͐ݒ�
		/// </summary>
		public string ExtendPopupStr
		{
			set
			{
				if (value == null || value == String.Empty)
				{
					extendPopupStr = null;
					exPopupRegex = null;
				}
				else
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat(@"(?<!&gt;)(?<label>(");
					sb.Append(value);
					//sb.Append(@")(?<num>\d+\-?\d*))");�@�@�@�@ // 2011.12.16
					sb.Append(@")(?<num>[1-9]+[\d\-\+,]*))");�@// 2011.12.16�@���ʂ���

					extendPopupStr = sb.ToString();
					exPopupRegex = new Regex(extendPopupStr, RegexOptions.Compiled);
				}
			}
			get
			{
				return extendPopupStr;
			}
		}

		private string colorWordSkin = String.Empty;
		public string ColorWordSkin
		{
			get
			{
				return colorWordSkin;
			}
		}


		//		/// <summary>
		//		/// ���X�|�b�v�A�b�v�p�̃w�b�_���擾
		//		/// </summary>
		//		public string PopupHeaderSkin {
		//			get { return popHeaderSkin; }
		//		}
		//
		//		/// <summary>
		//		/// �摜�p�̃X�L�����擾
		//		/// </summary>
		//		public string PictureSkin {
		//			get { return picSkin; }
		//		}
		//
		//		/// <summary>
		//		/// �v���r���[�p�̃X�L�����擾
		//		/// </summary>
		//		public string PreviewSkin {
		//			get { return prevSkin; }
		//		}
		//
		//		/// <summary>
		//		/// �������ݗ���p�̃X�L�����擾
		//		/// </summary>
		//		public string WroteSkin {
		//			get { return wroteSkin; }
		//		}

		/// <summary>
		/// Twin2IeHtmlSkin�N���X�̃C���X�^���X��������
		/// </summary>
		public Twin2IeHtmlSkin()
			: base()
		{
			this.SirusiList = new List<int>();
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			//			picSkin = String.Empty;
			//			prevSkin = String.Empty;
			//			wroteSkin = String.Empty;
			//			popHeaderSkin = headerSkin;

			//			flashSkin = String.Empty;
			//			videoSkin = String.Empty;
		}

		/// <summary>
		/// Twin2IeHtmlSkin�N���X�̃C���X�^���X��������
		/// </summary>
		public Twin2IeHtmlSkin(string skinFolder)
			: this()
		{
			Load(skinFolder);
		}

		private void MakeIDColorTable(SortedList<int, string> table, string html)
		{
			Match m = coloringId.Match(html);
			MatchCollection matches = null;

			if (m.Success)
			{
				matches = coloringIdAttribute.Matches(m.Groups["attr"].Value);
				existColoringId = true;
			}

			if (existColoringId && matches.Count > 0)
			{
				foreach (Match attr in matches)
				{
					int count = Int32.Parse(attr.Groups["key"].Value);
					string color = attr.Groups["value"].Value;

					table.Add(count, color);
				}
			}
			else
			{
				table[30] = "red";
				table[20] = "#FF00FF";
				table[15] = "orange";
				table[10] = "limegreen";
				table[5] = "blue";
				table[2] = "purple";
			}
		}

		//		/// <summary>
		//		/// �X�L����ǂݍ���
		//		/// </summary>
		//		/// <param name="skinFolder"></param>
		//		public override void Load(string skinFolder)
		//		{
		//			base.Load(skinFolder);
		//			
		//			picSkin = FileUtility.ReadToEnd(
		//				Path.Combine(skinFolder, "PicPopup.html"));
		//
		//			prevSkin = FileUtility.ReadToEnd(
		//				Path.Combine(skinFolder, "Preview.html"));
		//
		//			wroteSkin = FileUtility.ReadToEnd(
		//				Path.Combine(skinFolder, "History.html"));
		//
		//			popHeaderSkin = FileUtility.ReadToEnd(
		//				Path.Combine(skinFolder, "PopupHeader.html"));
		//		}

		public override void Load(string skinFolder)
		{
			base.Load(skinFolder);

			colorWordSkin = FileUtility.ReadToEnd(
				Path.Combine(skinFolder, "ColorWord.html"));

			if (colorWordSkin == String.Empty)
				colorWordSkin = "<font color=red><i>|</i></font>";

			MakeIDColorTable(newResIDColors, newResSkin);
			MakeIDColorTable(oldResIDColors, resSkin);
		}

		public override void Reset()
		{
			SirusiList.Clear();
			idCount.Clear();
		}

		public void IncrementIDCount(string id)
		{
			if (String.IsNullOrEmpty(id) || String.Compare(id, "???") == 0)
				return;

			if (!idCount.ContainsKey(id))
			{
				idCount.Add(id, 0);
			}

			idCount[id]++;
		}

		/// <summary>
		/// �w�肵�����X��ϊ�
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		public override string Convert(ResSet resSet)
		{
			// ���O���|�b�v�A�b�v�@�\
			base.NamePopup = Twinie.Settings.Thread.NameNumberPopup;

			// ���X��html�ɕϊ�
			string html = base.Convert(resSet);

			html = Regex.Replace(html, @"sssp://([^\s<]+)", "<img src=\"http://${1}\">");


			// �g���|�b�v�A�b�v
			if (exPopupRegex != null)
				html = exPopupRegex.Replace(html, "<a href=\"" + BaseUri + "${num}\" target=\"_blank\">${label}</a>");

			// �C���f�b�N�X�쐬
			string replacement = "<indices id=\"" + resSet.Index + "\"></indices>";

			//
			string nameSt = "";
			if (resSet.Name.Contains("��")) nameSt = "cap";
			else if (resSet.Name.Contains("��")) nameSt = "trip";

			string mailSt = "";
			if (resSet.Email.Length == 0) mailSt = "empty";

			else if (resSet.Email.Contains("sage"))
			{
				mailSt = "sage";
				if (resSet.Email.Length > 4) mailSt += "_a";
			}
			else if (resSet.Email.Contains("age"))
			{
				mailSt = "age";
				if (resSet.Email.Length > 3) mailSt += "_a";
			}

			string sirusi = String.Empty;

			if (SirusiList.Contains(resSet.Index))
			{
				sirusi = "sirusi";
			}
			//

			/*
				0 - PC 
				O - �g�� 
				P - p2 
				i,I - �h�R���{�ׂ����񂱁i��ʒm/�ʒm�j 
				o - willcom�̌g�� 
				Q - �g�уt���u���E�U
			 */
			string client = "unknown", id = resSet.ID;

			if (id.Length == 0)
			{
				// �����ɑΉ�
				Match m = Regex.Match(resSet.DateString, @".\d\d\s([a-zA-Z0])\b");
				if (m.Success)
				{
					id = m.Groups[1].Value;
				}
			}

			if (id.Length > 0 && id.Length != 8)
			{
				switch (id[id.Length - 1])
				{
					case '0': client = "pc"; break;
					case 'O': client = "ketai"; break;
					case 'P': client = "p2"; break;
					case 'I': client = "docomo"; break;
					case 'i': client = "iPhone"; break;
					case 'o': client = "willcom"; break;
					case 'Q': client = "Q"; break;
					case 'T': client = "otamesi"; break;
				}
			}
			//

#if REGEX_REPLACE
			html = IndexRegex.Replace(html, replacement);
#else
			html = html.Replace("<INDEX/>", replacement);
			html = html.Replace("<NAMESTATE/>", nameSt);
			html = html.Replace("<MAILSTATE/>", mailSt);
			html = html.Replace("<CLIENT/>", client);
			html = html.Replace("<SIRUSI/>", sirusi);
#endif
			//			// ID�|�b�v�A�b�v
			//			if (resSet.ID != String.Empty && html.IndexOf("<IDPopup/>") >= 0)
			//			{
			//				string idpopup = "<a href=\"method:Extract($3," + resSet.ID + ")\" target=\"_blank\">ID:" + resSet.ID +"</a>";
			//				html = html.Replace("<IDPopup/>", idpopup);
			//			}

			if (existColoringId)
			{
				// ID�̔����񐔂ŐF��ύX
				IDColoring(resSet, ref html);
			}


			html = BodyIDLinking(resSet, html);

			return html;
		}

		protected void IDColoring(ResSet res, ref string html)
		{
			if (String.IsNullOrEmpty(res.ID))
				return;

			// �^�O�����݂��邩�ǂ����𒲂ׂ�
			Match m = coloringId.Match(html);

			if (m.Success)
			{
				string color = GetIDColor(res.ID, res.IsNew);
				string replacement;

				if (color != null)
				{
					replacement =
						String.Concat("ID:<font color=\"", color, "\">", res.ID, "</font><font color=\"gray\"> (",
							idCount[res.ID], ")</font>");
				}
				// �z�F�����݂��Ȃ���Βʏ�̕\��
				else
				{
					replacement = "ID:" + res.ID;
				}

				// �z�X�g��������ꍇ�͒ǉ�
				if (!String.IsNullOrEmpty(res.Host))
				{
					replacement = String.Concat(replacement,
						" <font size=\"-1\">[ ", res.Host, " ]</font>");
				}

				html = coloringId.Replace(html, replacement);
			}
		}

		/// <summary>
		/// �{�����ɏ����ꂽ ID:�` �Ŏn�܂镶����𒊏o�|�b�v�A�b�v�Ƀ����N����B
		/// ���̍ۂɃX�L���Ŏw�肳�ꂽ�z�F�����f����B
		/// </summary>
		/// <param name="resSet"></param>
		/// <param name="srcHtml"></param>
		/// <returns></returns>
		private string BodyIDLinking(ResSet resSet, string srcHtml)
		{
			// ID:[8-9����] �̃��X������΁AID���o�����N��\��
			MatchEvaluator eval = delegate(Match match)
			{
				string idStr = match.Groups["id"].Value;
				string idColor = GetIDColor(idStr, resSet.IsNew);
				string colorTag;
				if (idColor == null) colorTag = " class=\"id_default_color\"";
				else colorTag = " style=\"color:" + idColor + ";\"";

				return String.Format("<a href=\"method:Extract($3,{0})\"{1} target=\"_blank\">ID:{0}</a>", idStr, colorTag);
			};

			return IDLinkRegex.Replace(srcHtml, eval);
		}

		/// <summary>
		/// �w�肵��ID�ɐݒ肳�ꂽ�F�����擾����B
		/// </summary>
		/// <param name="id"></param>
		/// <param name="isNewRes"></param>
		/// <returns></returns>
		private string GetIDColor(string id, bool isNewRes)
		{
			SortedList<int, string> dic =
					isNewRes ? newResIDColors : oldResIDColors;

			string color = null;

			// �w�肳��Ă��锭�����ȏ�̏ꍇ�̔z�F���擾�B
			for (int i = dic.Count - 1; i >= 0; i--)
			{
				int count;
				if (idCount.TryGetValue(id, out count))
				{
					if (dic.Keys[i] <= count)
					{
						color = dic.Values[i];
						break;
					}
				}
			}
			return color;
		}
	}
}
