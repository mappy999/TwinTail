using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Twin.Bbs;
using System.Windows.Forms;

namespace Twin.Tools
{
	/*
	 * 
	 * 
	 * 
	 */

	/// <summary>
	/// �֘A�L�[���[�h�Ɋւ��Ă̑�����s���N���X�ł��B
	/// </summary>
	public class RelatedKeyword
	{
		/// <summary>
		/// �w�肵���X���b�h�Ɋ֘A���Ă���L�[���[�h�ꗗ���擾���܂��B
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string[] GetKeywords(ThreadHeader header)
		{
			string uri = "http://p2.2ch.io/getf.cgi?" + header.Url;

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
			req.Timeout = 5000;
			req.ReadWriteTimeout = 5000;
			
			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			
			try
			{
				return Parsing(res.GetResponseStream());
			}
			finally
			{
				res.Close();
			}
		}

		private static string[] Parsing(Stream responseStream)
		{
			string text;

			using (StreamReader sr =
				new StreamReader(responseStream, Encoding.GetEncoding("shift_jis")))
			{
				text = sr.ReadToEnd();
			}

			List<string> words = new List<string>();

			foreach (Match m in Regex.Matches(text, "<a[^>]+?>([^<]+)</a>"))
			{
				words.Add(m.Groups[1].Value);
			}

			return words.ToArray();

		}
	}

	public class X2chSubjectSearcher
	{
		internal string keyword;
		internal int startIndex;

		private SubjectSearchSorting sorting = SubjectSearchSorting.Modified;
		/// <summary>
		/// �������ʂ̃\�[�g���@���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public SubjectSearchSorting Sorting
		{
			get
			{
				return sorting;
			}
			set
			{
				sorting = value;
			}
		}

		private SubjectSearchBbs bbsType = SubjectSearchBbs.ALL;

		public SubjectSearchBbs BbsType
		{
			get
			{
				return bbsType;
			}
			set
			{
				bbsType = value;
			}
		}

		private SortOrder sortOrder = SortOrder.Ascending;

		public SortOrder SortOrder
		{
			get
			{
				return sortOrder;
			}
			set
			{
				sortOrder = value;
			}
		}
	

		private int count = 10;
		/// <summary>
		/// ��x�Ɏ擾���錟�����ʂ̐����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public int ViewCount
		{
			get
			{
				return count;
			}
			set
			{
				count = value;
			}
		}
	
		public X2chSubjectSearcher()
		{
		}

		public SubjectSearchResult Search(string keyword)
		{
			return Search(keyword, 0);
		}

		public SubjectSearchResult Search(string keyword, int startIndex)
		{
			this.keyword = keyword;
			this.startIndex = startIndex;

			string order = (this.sortOrder == SortOrder.Ascending)	? "A" : "D";
			string bbsType = (this.bbsType == SubjectSearchBbs._2ch) ? "2ch" : this.bbsType.ToString();


			Encoding encoding = Encoding.GetEncoding("euc-jp");

			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("STR={0}", HttpUtility.UrlEncode(keyword, encoding));
			sb.AppendFormat("&SCEND={0}&SORT={1}&COUNT={2}&TYPE=TITLE&OFFSET={3}&BBS={4}",
				order, Sorting.ToString().ToUpper(), ViewCount, startIndex, bbsType);

			string requestUri = "http://find.2ch.net/index.php?" + sb.ToString();

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
			req.UserAgent = TwinDll.IEUserAgent;
			req.ContentType = "application/x-www-form-urlencoded";

			/*HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri);
			req.ReadWriteTimeout = 10000;
			req.Timeout = 10000;
			req.Method = "POST";
			req.ContentLength = bytes.Length;
			req.ContentType = "application/x-www-form-urlencoded";
			req.CookieContainer = cookies;

			Stream requestStream = req.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			*/

			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			
			try
			{
				if (res.StatusCode == HttpStatusCode.OK)
				{
					return ParsingResponseData(res.GetResponseStream());
				}
				else
				{
					return new SubjectSearchResult(this, 0, startIndex, 0, new List<ThreadHeader>());
				}
			}
			finally
			{
				res.Close();
			}
		}

		private SubjectSearchResult ParsingResponseData(Stream responseStream)
		{
			string text;

			using (StreamReader sr =
				new StreamReader(responseStream, Encoding.GetEncoding("euc-jp")))
			{
				text = sr.ReadToEnd();
			}

			// ��v��(1)�A��������(2)
			Regex regexh = new Regex("([0-9]+)�X����.*?�X����\\s*([0-9\\.]+)�b");

			Match m1 = regexh.Match(text);
			int totalCount;
			float interval;

			Int32.TryParse(m1.Groups[1].Value, out totalCount);
			Single.TryParse(m1.Groups[2].Value, out interval);

			// �z�X�g��(host)�A��key(board)�A�X���b�h��key(key)�A�X���b�h��(subject)�A���X��(res)�A��(bname)
			Regex regexb = new Regex("<dt><a href=\"http://(?<host>[^/]+)/test/read.cgi/(?<board>[^/]+)/(?<key>[0-9]+)/.*?\">(?<subject>.+?)</a>\\s*?\\((?<res>[0-9]+?)\\).*?<a.*?>(?<bname>.+?)</a>��",
				RegexOptions.Compiled);

			List<ThreadHeader> list = new List<ThreadHeader>();

			foreach (Match m in regexb.Matches(text))
			{
				ThreadHeader h = new X2chThreadHeader();
				h.BoardInfo.Server = m.Groups["host"].Value;
				h.BoardInfo.Path = m.Groups["board"].Value;
				h.BoardInfo.Name = m.Groups["bname"].Value;
				h.Subject = m.Groups["subject"].Value;
				h.Key = m.Groups["key"].Value;

				int res;
				if (Int32.TryParse(m.Groups["res"].Value, out res))
				{
					h.ResCount = res;
				}

				list.Add(h);
			}

			return new SubjectSearchResult(this,
				totalCount, startIndex, interval, list);
		}
	}
	
	public class SubjectSearchResult
	{
		protected X2chSubjectSearcher searcher;

		private int totalCount;
		/// <summary>
		/// �������ʂ̑S��v�X���b�h�����擾���܂��B
		/// </summary>
		public int TotalCount
		{
			get
			{
				return totalCount;
			}
		}

		private int startIndex;
		/// <summary>
		/// �������ʂ̊J�n�C���f�b�N�X���擾���܂��B
		/// </summary>
		public int StartIndex
		{
			get
			{
				return startIndex;
			}
		}

		private float interval;
		/// <summary>
		/// �����ɂ����������Ԃ��擾���܂��B
		/// </summary>
		public float Interval
		{
			get
			{
				return interval;
			}
		}

		private List<ThreadHeader> matchThreads = new List<ThreadHeader>();
		/// <summary>
		/// �������ʂɊ܂܂���v�X���b�h���擾���܂��B
		/// </summary>
		public List<ThreadHeader> MatchThreads
		{
			get
			{
				return matchThreads;
			}
		}
	
		internal SubjectSearchResult(X2chSubjectSearcher searcher,
			int totalCount, int startIndex, float interval, List<ThreadHeader> matches)
		{
			this.searcher = searcher;
			this.totalCount = totalCount;
			this.startIndex = startIndex;
			this.interval = interval;
			this.matchThreads = matches;
		}

		public SubjectSearchResult Next()
		{
			return Next(startIndex + searcher.ViewCount);
		}

		public SubjectSearchResult Next(int offset)
		{
			return searcher.Search(searcher.keyword, offset);
		}
	}

	/// <summary>
	/// �Q�����˂�X���b�h�����Ń\�[�g������@��\���񋓃^�C�ł��B
	/// </summary>
	public enum SubjectSearchSorting
	{
		/// <summary>
		/// ���X�̓��e���Ń\�[�g���܂��B
		/// </summary>
		NPosts,
		/// <summary>
		/// �X���b�h���쐬���ꂽ�����Ń\�[�g���܂��B
		/// </summary>
		Created,
		/// <summary>
		/// �ŐV�����Ń\�[�g���܂��B
		/// </summary>
		Modified,
	}

	public enum SubjectSearchOrder
	{
		/// <summary> ���� </summary>
		Ascending,
		/// <summary> �~�� </summary>
		Dscending,
	}

	public enum SubjectSearchBbs
	{
		ALL,
		_2ch,
		bbspink,
	}
}
