// IEComExtractResult.cs

namespace Twin.Text
{
	using System;
	using System.Text.RegularExpressions;
	using System.IO;
	using Twin.Text;
	using Twin.Forms;
	using Twin.IO;
	using CSharpSamples;
	using mshtml;

	/// <summary>
	/// IEComThreadBrowser�p�̃��X���o����������N���X
	/// </summary>
	public class IEComExtractor : AbstractExtractor
	{
		private readonly IEComThreadBrowser thread;
		private IEComSearcher searcher;

		/// <summary>
		/// IEComExtractor�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="tb"></param>
		/// <param name="modePopup"></param>
		public IEComExtractor(IEComThreadBrowser tb)
		{
			if (tb == null) {
				throw new ArgumentNullException("tb");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			thread = tb;
			searcher = new IEComSearcher(tb.GetDomDocument());
		}

		/// <summary>
		/// �w�肵���L�[���[�h���܂ރ��X�𒊏o
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="element"></param>
		/// <returns></returns>
		public override ResSetCollection Extract(string keyword, ResSetElement element)
		{
			if (keyword == null || keyword == String.Empty)
				throw new ArgumentException("keyword���s���ł�");

			ReadOnlyResSetCollection resItems = thread.ResSets;
			ResSetCollection matches = new ResSetCollection();

			RegexOptions regopt = RegexOptions.None;

			if ((Options & SearchOptions.MatchCase) == 0)
				regopt |= RegexOptions.IgnoreCase;

			if ((Options & SearchOptions.Regex) == 0)
				keyword = Regex.Escape(keyword);

			lock (resItems.SyncRoot)
			{
				foreach (ResSet item in resItems)
				{
					string obj = item.ToString(element);
					if (Regex.IsMatch(obj, keyword, regopt))
					{
							matches.Add(item);
					}
				}
			}

			return matches;
		}

		/// <summary>
		/// �w�肵���L�[���[�h���܂ރ��X�𒊏o���\��
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="element"></param>
		/// <returns></returns>
		public override bool InnerExtract(string keyword, ResSetElement element)
		{
			if (keyword == String.Empty)
				return false;

			ResSetCollection matches = Extract(keyword, element);
			bool matched = matches.Count > 0;

			if (matched)
			{
				if (NewWindow)
				{
					thread.Popup(matches);

					if (Twinie.Settings.Popup.ClickedHide)
						thread.clickedPopup = true;
				}
				else
				{
					thread.isExtracting = true;

					thread.Clear();
					thread.WriteText(thread.Skin.GetHeader(thread.HeaderInfo));
					thread.WriteText(String.Format("�y���X���o�z<br>�ΏۃX���F {0}<br>�L�[���[�h�F {1}<br>���o���X���F {2}<br><br>",
						thread.HeaderInfo.Subject, keyword, matches.Count));

					thread.WriteText(thread.Skin.Convert(matches));
					thread.WriteText(thread.Skin.GetFooter(thread.HeaderInfo));
					thread.isExtracting = false;
				}
/*
				if (matches.Count <= 32)
				{
					// ��v�����P��������\��
					searcher.Options = this.Options;
					searcher.Highlights(keyword);

					thread.highlighting = true;
				}*/
			}

			return matched;
		}

		/// <summary>
		/// �����N�����ׂĎ擾
		/// </summary>
		/// <returns></returns>
		public override LinkCollection GetLinks()
		{
			// dat�t�@�C�����璼�ړǂݍ���
			LinkCollection links = new LinkCollection();
			string filePath = thread.Cache.GetDatPath(thread.HeaderInfo);
			string data = null;

			using (StreamReader sr = new StreamReader(
					   StreamCreator.CreateReader(filePath, thread.HeaderInfo.UseGzip)))
			{
				data = sr.ReadToEnd();
			}

			MatchCollection matches = 
				HtmlTextUtility.LinkRegex2.Matches(data);

			foreach (Match m in matches)
				links.Add(m.Value);

			return links;
		}

		public override void Reset()
		{
			searcher.Reset();
		}
	}
}
