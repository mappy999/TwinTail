// MachiPost.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using System.IO;
	using System.Net;

	/// <summary>
	/// �܂�BBS (www.machi.to) �ɓ��e����@�\���
	/// </summary>
	public class MachiPost : PostBase
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
		/// MachiPost�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiPost()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			response = PostResponse.None;
			Encoding = Encoding.GetEncoding("Shift_Jis");
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

				// CGI�̑��݂���URL���쐬
				string uri = String.Format("http://{0}/bbs/write.cgi", board.Server);

				// ���M�f�[�^���쐬
				StringBuilder sb = new StringBuilder();
				sb.Append("SUBJECT=" + UrlEncode(thread.Subject));
				sb.Append("&submit=" + UrlEncode("�V�K��������"));
				sb.Append("&NAME=" + UrlEncode(thread.From));
				sb.Append("&MAIL=" + UrlEncode(thread.Email));
				sb.Append("&MESSAGE=" + UrlEncode(thread.Body));
				sb.Append("&BBS=" + board.Path);
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
				// ���e�������쐬
				int time = GetTime(header.LastModified);

				// ���M�f�[�^���쐬
				string uri = String.Format("http://{0}/bbs/write.cgi", header.BoardInfo.Server);
				StringBuilder sb = new StringBuilder();
				sb.Append("submit=" + UrlEncode("��������"));
				sb.Append("&NAME=" + UrlEncode(res.From));
				sb.Append("&MAIL=" + UrlEncode(res.Email));
				sb.Append("&MESSAGE=" + UrlEncode(res.Body));
				sb.Append("&BBS=" + header.BoardInfo.Path);
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

		/// <summary>
		/// data���T�[�o�[�ɑ��M�����X�|���X�𓾂�
		/// </summary>
		/// <param name="data"></param>
		protected virtual PostResponse Posting(BoardInfo board, byte[] data, string uri, ref bool retried)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (data == null) {
				throw new ArgumentNullException("data");
			}

			HttpWebRequest req = null;
			HttpWebResponse res = null;
			PostResponseParser parser;

			// ���t�@�����쐬
			string referer = board.Url + "index.html";

			try {
				req = (HttpWebRequest)WebRequest.Create(uri);
				req.Method = "POST";
				req.Accept = "*/*";
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = data.Length;
				req.Referer = referer;
				req.Timeout = 30000;
				req.Proxy = Proxy;
				req.UserAgent = UserAgent;
				req.AllowAutoRedirect = false;

				Stream st = req.GetRequestStream();
				st.Write(data, 0, data.Length);
				st.Close();

				res = (HttpWebResponse)req.GetResponse();

				// ���X�|���X�����
				using (StreamReader sr = 
						   new StreamReader(res.GetResponseStream(), TwinDll.DefaultEncoding))
				{
					parser = new PostResponseParser(sr.ReadToEnd());
					response = parser.Response;
					res.Close();

					// <TITLE>302 Found</TITLE>���Ԃ��Ă����珑�����ݐ���
					if (res.StatusCode == HttpStatusCode.Found)
						response = PostResponse.Success;
				}

				// ���e�C�x���g�𔭐�������
				PostEventArgs e = new PostEventArgs(response, parser.Title, parser.PlainText, null, -1);
				OnPosted(this, e);

				// ���Ƀ��g���C����Ă����疳�����[�v�h�~�̂���false�ɐݒ�
				retried = retried ? false : e.Retry;
			}
			catch (WebException ex) {
				if (((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Found)
					OnError(this, new PostErrorEventArgs(ex));
			}
			catch (Exception ex) {
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
