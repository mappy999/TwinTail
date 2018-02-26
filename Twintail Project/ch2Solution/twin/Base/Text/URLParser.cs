using System;
using System.Text.RegularExpressions;

namespace Twin
{
	/// <summary>
	/// �X���b�h�ꗗ��X���b�h��URL����͂��邽�߂̃p�[�T
	/// </summary>
	public class URLParser
	{
		/// <summary>
		/// �X���ꗗ��URL����͂��邽�߂̐��K�\���N���X�̃C���X�^���X��\��
		/// (http://www.2ch.net/board/ �܂��� http://www.2ch.net/board/index.html �Ɉ�v
		/// </summary>
		protected static readonly Regex[] ParseListRegexArray =
			new Regex[] {
				new Regex(@"^h?ttp://(?<host>jbbs\.(shitaraba|livedoor)\.(com|jp))/(?<path>\w+/\d+)/?$", RegexOptions.Compiled),	// ������΂̔A�h���X
				new Regex(@"^h?ttp://(?<host>.+)/(?<path>[^/]+)/(index\d*\.html|\s*$)", RegexOptions.Compiled),			// �S�ʂ̔A�h���X
			};

		/// <summary>
		/// �X���b�h��URL����͂��邽�߂̐��K�\��
		/// </summary>
		protected static readonly Regex[] ParseThreadRegexArray =
			new Regex[] {
				new Regex(@"^h?ttp://(?<host>[\w\-~/_.]+)/test/(read\.cgi|r\.i)/(?<path>[^/]+)/(?<key>[^/]+)/?", RegexOptions.Compiled),	// 2ch�܂���2ch�݊���URL
				new Regex(@"^h?ttp://(?<host>[\w\-~/_.]+)/test/read\.cgi\?bbs=(?<path>\w+?)&key=(?<key>\w+)", RegexOptions.Compiled),		// ��2chURL
				new Regex(@"^h?ttp://(?<host>[^/]+)/(?<path>\w+?)/kako/[\d/]+?/(?<key>\d+)\.html", RegexOptions.Compiled),					// 2ch�ߋ����OURL
				new Regex(@"^h?ttp://(?<host>[\w\-~/_.]+)/bbs/read\.pl\?BBS=(?<path>\w+?)&KEY=(?<key>\w+)", RegexOptions.Compiled),			// �܂�BBS��URL
				new Regex(@"^h?ttp://(?<host>\w+\.(shitaraba|livedoor)\.(com|jp))/bbs/read\.cgi/(?<path>\w+/\d+)/(?<key>[^/]+)/?", RegexOptions.Compiled),		// ������΂�URL
			};

		/// <summary>
		/// URL����͂�BoardInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static BoardInfo ParseBoard(string url)
		{
			foreach (Regex regex in ParseListRegexArray)
			{
				Match m = regex.Match(url);
				if (m.Success)
				{
					BoardInfo b = new BoardInfo();
					b.Server = m.Groups["host"].Value;
					b.Path = m.Groups["path"].Value;
					b.Bbs = BoardInfo.Parse(b.Server);
					return b;
				}
			}
			return null;
		}

		/// <summary>
		/// URL����͂�ThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static ThreadHeader ParseThread(string url)
		{
			foreach (Regex regex in ParseThreadRegexArray)
			{
				Match m = regex.Match(url);
				if (m.Success)
				{
					BoardInfo board = new BoardInfo();
					board.Server = m.Groups["host"].Value;
					board.Path = m.Groups["path"].Value;

					if (board.Bbs == BbsType.None)
						board.Bbs = BbsType.Dat;

					// �ߋ����O�q�ɂ�URL�̏ꍇ
					if (ParseThreadRegexArray[2].Equals(regex))
						board.Bbs = BbsType.X2chKako;

					ThreadHeader h = TypeCreator.CreateThreadHeader(board.Bbs);
					h.Key = m.Groups["key"].Value;
					h.BoardInfo = board;

					return h;
				}
			}
			return null;
		}

		/// <summary>
		/// URL����͂�BoardInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static bool IsBoardUrl(string url)
		{
			foreach (Regex regex in ParseListRegexArray)
			{
				Match m = regex.Match(url);
				if (m.Success)
					return true;
			}
			return false;
		}

		/// <summary>
		/// URL����͂�ThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static bool IsThreadUrl(string url)
		{
			foreach (Regex regex in ParseThreadRegexArray)
			{
				Match m = regex.Match(url);
				if (m.Success)
					return true;
			}
			return false;
		}
	}
}
