// MachiPost.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using System.IO;
	using System.Net;
	using System.Diagnostics;

	/// <summary>
	/// jbbs�ɓ��e����@�\���
	/// </summary>
	public class JbbsPost : PostBase
	{
		private PostResponse response;

		/// <summary>
		/// ���X�𓊍e�ł��邩�ǂ����������l���擾 (���̃v���p�e�B�͏��true��Ԃ�)
		/// </summary>
		public override bool CanPostRes {
			get { return true; }
		}

		/// <summary>
		/// �X���b�h�𓊍e�ł��邩�ǂ����������l���擾 (���̃v���p�e�B�͏��false��Ԃ�)
		/// </summary>
		public override bool CanPostThread {
			get { return true; }
		}

		/// <summary>
		/// �T�[�o�[����̉��΂��擾
		/// </summary>
		public override PostResponse Response {
			get { return response; }
		}

		/// <summary>
		/// JbbsPost�N���X�̃C���X�^���X��������
		/// </summary>
		public JbbsPost()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			Encoding = Encoding.GetEncoding("EUC-JP");
			response = PostResponse.None;
		}

		/// <summary>
		/// �V�K�X���b�h�𓊍e
		/// </summary>
		/// <param name="board">���e��̔�</param>
		/// <param name="thread">���e������e</param>
		public override void Post(BoardInfo board, PostThread thread)
		{
			try {
				// ���e�������쐬
				int time = GetTime(Time);

				string[] dirbbs = board.Path.Split('/');

				// CGI�̑��݂���URL���쐬
				string uri = String.Format("http://{0}/bbs/write.cgi/{1}/new/", board.Server, board.Path);

				// ���M�f�[�^���쐬
				StringBuilder sb = new StringBuilder();
				sb.Append("SUBJECT=" + UrlEncode(thread.Subject));
				sb.Append("&submit=" + UrlEncode("�V�K��������"));
				sb.Append("&NAME=" + UrlEncode(thread.From));
				sb.Append("&MAIL=" + UrlEncode(thread.Email));
				sb.Append("&MESSAGE=" + UrlEncode(thread.Body));
				sb.Append("&DIR=" + dirbbs[0]);
				sb.Append("&BBS=" + dirbbs[1]);
				sb.Append("&TIME=" + time);

				bool retried = false;
				byte[] bytes = Encoding.GetBytes(sb.ToString());

				Posting(board, bytes, uri, ref retried);
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
				throw ex;
			}
		}

		/// <summary>
		/// ���b�Z�[�W�𓊍e
		/// </summary>
		/// <param name="header">���e��̃X���b�h</param>
		/// <param name="res">���e������e</param>
		public override void Post(ThreadHeader header, PostRes res)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			try {
				BoardInfo board = header.BoardInfo;

				// ���e�������쐬
				int time = GetTime(header.LastModified);

				// BoardInfo.Path��BBS��DIR�ɕ���
				string[] dirbbs = board.Path.Split('/');
				Trace.Assert(dirbbs.Length == 2);

				// ���M�f�[�^���쐬
				string uri = String.Format("http://{0}/bbs/write.cgi/{1}/{2}/", board.Server, board.Path, header.Key);
				StringBuilder sb = new StringBuilder();
				sb.Append("submit=" + UrlEncode("��������"));
				sb.Append("&NAME=" + UrlEncode(res.From));
				sb.Append("&MAIL=" + UrlEncode(res.Email));
				sb.Append("&MESSAGE=" + UrlEncode(res.Body));
				sb.Append("&DIR=" + dirbbs[0]);
				sb.Append("&BBS=" + dirbbs[1]);
				sb.Append("&KEY=" + header.Key);
				sb.Append("&TIME=" + time);

				bool retried = false;
				byte[] bytes = Encoding.GetBytes(sb.ToString());

				Posting(header.BoardInfo, bytes, uri, ref retried);
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
				throw ex;
			}
		}

		protected virtual PostResponse Posting(BoardInfo board, byte[] data, string uri, ref bool retried)
		{
			HttpWebResponse res = null;
			PostResponseParser parser;

			const int timeout = 30000;
			string referer = board.Url + "index.html";

			try {
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
				req.Method = "POST";
				req.Accept = "*/*";
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = data.Length;
				req.Referer = referer;
				req.Timeout = timeout;
				req.ReadWriteTimeout = timeout;
				req.UserAgent = UserAgent;
				req.AllowAutoRedirect = false;
				req.Proxy = Proxy;

				Stream st = req.GetRequestStream();
				st.Write(data, 0, data.Length);
				st.Close();

				res = (HttpWebResponse)req.GetResponse();

				// ���X�|���X�����
				using (TextReader reader = new StreamReader(res.GetResponseStream(), Encoding))
				{
					parser = new PostResponseParser(reader.ReadToEnd());
					response = parser.Response;

					// <TITLE>302 Found</TITLE>���Ԃ��Ă����珑�����ݐ���
//					if (res.StatusCode == HttpStatusCode.Found)
//						response = PostResponse.Success;
				}

				// ���e�C�x���g�𔭐�������
				PostEventArgs e = new PostEventArgs(response, parser.Title, parser.PlainText, null, -1);
				OnPosted(this, e);

				// ���Ƀ��g���C����Ă����疳�����[�v�h�~�̂���false�ɐݒ�
				retried = retried ? false : e.Retry;
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
				OnError(this, new PostErrorEventArgs(ex));
			}
			finally {
				if (res != null)
					res.Close();
			}

			return response;
		}

		/// <summary>
		/// ���e���L�����Z������
		/// </summary>
		public override void Cancel()
		{
			throw new NotSupportedException();
		}
	}
}
