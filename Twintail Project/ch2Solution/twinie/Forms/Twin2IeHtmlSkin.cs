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
	/// Twin2Ie専用のスキン
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
		// (?<!\">)ID: としているのは、スキンでID抽出用タグを挟んでいる場合があるので、それを除くため

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

		// IDごとに発言回数を記録しておくテーブル
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

		// しるしされたレス番号のリスト // v2.3.6.9
		public List<int> SirusiList { get; private set; }


		/// <summary>
		/// 拡張ポップアップ用の文字列を取得または設定
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
					//sb.Append(@")(?<num>\d+\-?\d*))");　　　　 // 2011.12.16
					sb.Append(@")(?<num>[1-9]+[\d\-\+,]*))");　// 2011.12.16　水玉さん

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
		//		/// レスポップアップ用のヘッダを取得
		//		/// </summary>
		//		public string PopupHeaderSkin {
		//			get { return popHeaderSkin; }
		//		}
		//
		//		/// <summary>
		//		/// 画像用のスキンを取得
		//		/// </summary>
		//		public string PictureSkin {
		//			get { return picSkin; }
		//		}
		//
		//		/// <summary>
		//		/// プレビュー用のスキンを取得
		//		/// </summary>
		//		public string PreviewSkin {
		//			get { return prevSkin; }
		//		}
		//
		//		/// <summary>
		//		/// 書き込み履歴用のスキンを取得
		//		/// </summary>
		//		public string WroteSkin {
		//			get { return wroteSkin; }
		//		}

		/// <summary>
		/// Twin2IeHtmlSkinクラスのインスタンスを初期化
		/// </summary>
		public Twin2IeHtmlSkin()
			: base()
		{
			this.SirusiList = new List<int>();
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			//			picSkin = String.Empty;
			//			prevSkin = String.Empty;
			//			wroteSkin = String.Empty;
			//			popHeaderSkin = headerSkin;

			//			flashSkin = String.Empty;
			//			videoSkin = String.Empty;
		}

		/// <summary>
		/// Twin2IeHtmlSkinクラスのインスタンスを初期化
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
		//		/// スキンを読み込む
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
		/// 指定したレスを変換
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		public override string Convert(ResSet resSet)
		{
			// 名前欄ポップアップ機能
			base.NamePopup = Twinie.Settings.Thread.NameNumberPopup;

			// レスをhtmlに変換
			string html = base.Convert(resSet);

			html = Regex.Replace(html, @"sssp://([^\s<]+)", "<img src=\"http://${1}\">");


			// 拡張ポップアップ
			if (exPopupRegex != null)
				html = exPopupRegex.Replace(html, "<a href=\"" + BaseUri + "${num}\" target=\"_blank\">${label}</a>");

			// インデックス作成
			string replacement = "<indices id=\"" + resSet.Index + "\"></indices>";

			//
			string nameSt = "";
			if (resSet.Name.Contains("★")) nameSt = "cap";
			else if (resSet.Name.Contains("◆")) nameSt = "trip";

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
				O - 携帯 
				P - p2 
				i,I - ドコモ＋べっかんこ（非通知/通知） 
				o - willcomの携帯 
				Q - 携帯フルブラウザ
			 */
			string client = "unknown", id = resSet.ID;

			if (id.Length == 0)
			{
				// モ娘板に対応
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
			//			// IDポップアップ
			//			if (resSet.ID != String.Empty && html.IndexOf("<IDPopup/>") >= 0)
			//			{
			//				string idpopup = "<a href=\"method:Extract($3," + resSet.ID + ")\" target=\"_blank\">ID:" + resSet.ID +"</a>";
			//				html = html.Replace("<IDPopup/>", idpopup);
			//			}

			if (existColoringId)
			{
				// IDの発言回数で色を変更
				IDColoring(resSet, ref html);
			}


			html = BodyIDLinking(resSet, html);

			return html;
		}

		protected void IDColoring(ResSet res, ref string html)
		{
			if (String.IsNullOrEmpty(res.ID))
				return;

			// タグが存在するかどうかを調べる
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
				// 配色が存在しなければ通常の表示
				else
				{
					replacement = "ID:" + res.ID;
				}

				// ホスト名がある場合は追加
				if (!String.IsNullOrEmpty(res.Host))
				{
					replacement = String.Concat(replacement,
						" <font size=\"-1\">[ ", res.Host, " ]</font>");
				}

				html = coloringId.Replace(html, replacement);
			}
		}

		/// <summary>
		/// 本文内に書かれた ID:～ で始まる文字列を抽出ポップアップにリンクする。
		/// その際にスキンで指定された配色も反映する。
		/// </summary>
		/// <param name="resSet"></param>
		/// <param name="srcHtml"></param>
		/// <returns></returns>
		private string BodyIDLinking(ResSet resSet, string srcHtml)
		{
			// ID:[8-9文字] のレスがあれば、ID抽出リンクを貼る
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
		/// 指定したIDに設定された色名を取得する。
		/// </summary>
		/// <param name="id"></param>
		/// <param name="isNewRes"></param>
		/// <returns></returns>
		private string GetIDColor(string id, bool isNewRes)
		{
			SortedList<int, string> dic =
					isNewRes ? newResIDColors : oldResIDColors;

			string color = null;

			// 指定されている発言数以上の場合の配色を取得。
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
