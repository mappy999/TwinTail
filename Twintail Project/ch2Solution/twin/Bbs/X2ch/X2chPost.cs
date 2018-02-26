// X2chPost.cs

namespace Twin.Bbs
{
	using System;
	using System.Net;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Diagnostics;
	using System.Threading;
	using Twin.Tools;
	using System.Collections.Generic;

	/// <summary>
	/// �Q�����˂�ɓ��e����@�\���
	/// </summary>
	public class X2chPost : PostBase
	{
		private PostResponse response;
		private bool sendBeID;
		protected readonly string postCGIPath;
		protected readonly string postSubCGIPath;

		public virtual bool SendBeID {
			set { sendBeID = value; }
			get { return sendBeID; }
		}

		/// <summary>
		/// �V�K�X���b�h�̓��e�ɑΉ����Ă��邩�ǂ������擾
		/// </summary>
		public override bool CanPostThread {
			get { return true; }
		}

		/// <summary>
		/// ���X�̓��e�ɑΉ����Ă��邩�ǂ������擾
		/// </summary>
		public override bool CanPostRes {
			get { return true; }
		}

		/// <summary>
		/// ���e���̃T�[�o�[����̃��X�|���X��\���l���擾
		/// </summary>
		public override PostResponse Response {
			get { return response; }
		}

		/// <summary>
		/// X2chPost�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chPost()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			sendBeID = false;
			postCGIPath = "test/bbs.cgi";
			response = PostResponse.None;
			Encoding = Encoding.GetEncoding("Shift_Jis");
		}

		protected X2chPost(Encoding encoding) : this()
		{
			Encoding = encoding;
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

				// ���M�f�[�^���쐬
				bool retried = false;
Retry:
				if (retried)
					Thread.Sleep(1000);

				// CGI�̑��݂���URL���쐬
				string uri = String.Format("http://{0}/{1}", board.Server, postCGIPath);

				StringBuilder sb = new StringBuilder();
				sb.Append("subject=" + UrlEncode(thread.Subject));
//				sb.Append("&submit=" + (retried ? UrlEncode("�S�ӔC�𕉂����Ƃ��������ď�������") : UrlEncode("�V�K�X���b�h�쐬")));
				sb.Append("&submit=" + UrlEncode("�V�K�X���b�h�쐬"));
				sb.Append("&FROM=" + UrlEncode(thread.From));
				sb.Append("&mail=" + UrlEncode(thread.Email));
				sb.Append("&MESSAGE=" + UrlEncode(thread.Body));
				sb.Append("&bbs=" + board.Path);
				sb.Append("&time=" + time);

				sb.Append(TwinDll.AditionalAgreementField);
				sb.Append(TwinDll.AddWriteSection);

				AddSessionId(sb);

				byte[] bytes = Encoding.GetBytes(sb.ToString());
				Posting(board, bytes, uri, ref retried);

				if (retried)
					goto Retry;
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
				throw ex;
			}
		}

		private void AddSessionId(StringBuilder sb)
		{
			X2chAuthenticator authenticator = X2chAuthenticator.GetInstance();
			if (authenticator.HasSession)
			{
				sb.AppendFormat("&sid={0}", UrlEncode(authenticator.SessionId));
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
				bool retried = false;
Retry:
				if (retried)
					Thread.Sleep(100);

				string uri = String.Format("http://{0}/{1}", header.BoardInfo.Server, postCGIPath);
				StringBuilder sb = new StringBuilder();
//				sb.Append("submit=" + (retried ? UrlEncode("�S�ӔC�𕉂����Ƃ��������ď�������") : UrlEncode("��������")));
				sb.Append("submit=" + UrlEncode("��������"));
				sb.Append("&FROM=" + UrlEncode(res.From));
				sb.Append("&mail=" + UrlEncode(res.Email));
				sb.Append("&MESSAGE=" + UrlEncode(res.Body));
				sb.Append("&bbs=" + header.BoardInfo.Path);
				sb.Append("&key=" + header.Key);
				sb.Append("&time=" + time);
//				sb.Append("&hana=mogera");

				sb.Append(TwinDll.AditionalAgreementField);
				sb.Append(TwinDll.AddWriteSection);

				AddSessionId(sb);

				byte[] bytes = Encoding.GetBytes(sb.ToString());
				Posting(header.BoardInfo, bytes, uri, ref retried);

				if (retried)
					goto Retry;
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

			HttpWebResponse res = null;
			PostResponseParser parser = null;

			// �^�C���A�E�g�l
			const int timeout = 15000; // 15�b

			// �Ď��s���s�����ǂ���
			bool is_retry = true;

			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
				req.Method = "POST";
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = data.Length;
				req.Referer = board.Url + "index.html";
				req.UserAgent = UserAgent;
				req.Timeout = timeout;
				req.ReadWriteTimeout = timeout;
				req.Proxy = Proxy;
				//				req.Accept = "text/html, */*";
				//				req.Expect = null;
				//				req.AllowAutoRedirect = false;
				//				req.ProtocolVersion = HttpVersion.Version10;

				// NTwin 2011/05/31
				//req.CookieContainer = GetCookie(board);

				req.CookieContainer = CookieManager.gCookies;
				
#if DEBUG
				foreach (Cookie c in req.CookieContainer.GetCookies(req.RequestUri))
					Console.WriteLine("{0}={1}", c.Name, c.Value);
#endif
				SetHttpWebRequest(req);

				Stream st = req.GetRequestStream();
				st.Write(data, 0, data.Length);
				st.Close();

				res = (HttpWebResponse)req.GetResponse();

				// ���X�|���X����͂��邽�߂̃p�[�T���������B
				using (TextReader reader = new StreamReader(res.GetResponseStream(), Encoding))
				{
					parser = new PostResponseParser(reader.ReadToEnd());
					response = parser.Response;

					if (response == PostResponse.Cookie)
					{
						foreach (KeyValuePair<string, string> kv in parser.HiddenParams)
						{
							if (Regex.IsMatch(kv.Key, "subject|FROM|mail|MESSAGE|bbs|time|key") == false)
							{
								TwinDll.AditionalAgreementField = String.Format("&{0}={1}",
									kv.Key, kv.Value);

								Console.WriteLine(TwinDll.AditionalAgreementField);
								break;
							}
						}
					}
				}

				if (res.StatusCode == HttpStatusCode.Found)
					response = PostResponse.Success;

				/*
				board.CookieContainer = new CookieContainer();

				bool ponIsExist = false;

				foreach (Cookie c in req.CookieContainer.GetCookies(req.RequestUri))
				{
#if DEBUG
					Console.WriteLine("{0}={1}", c.Name, c.Value);
#endif
					if (c.Name == "PON")
						ponIsExist = true;
					board.CookieContainer.Add(c);
				}

				if (!ponIsExist && response == PostResponse.Cookie)
				{
					board.CookieContainer.Add(res.Cookies);
				}*/

				// ���e�C�x���g�𔭐�������
				PostEventArgs e = new PostEventArgs(response, parser.Title,
					parser.PlainText, null, parser.SambaCount);

				OnPosted(this, e);

				is_retry = e.Retry;
			}
			catch (Exception ex)
			{
#if DEBUG
				WebException webex = ex as WebException;
				if (webex != null)
				{
					TwinDll.ShowOutput("Status " + webex.Status + ", " + webex.ToString());
				}
				else
				{
					TwinDll.ShowOutput(ex);
				}
#endif
				// �^�C���A�E�g�₻��ȊO�̗�O�����������疳�����Ń��g���C�𒆎~
				//is_retry = false;
				OnError(this, new PostErrorEventArgs(ex));
			}
			finally {
				if (res != null)
					res.Close();

				// �N�b�L�[�m�F�Ȃǂł̍Ď��s����
				// �����ɍĎ��s����Ă����疳�����[�v�h�~�̂���false�ɐݒ�
				if (retried)
				{
					retried = false;
				}
				else {
					retried = is_retry;
				}
			}

			return response;
		}
		
		/// <summary>
		/// ���e���L�����Z�� (���Ή�)
		/// </summary>
		public override void Cancel()
		{
			throw new NotSupportedException();
		}

		protected virtual void SetHttpWebRequest(HttpWebRequest req)
		{
		}

		//protected CookieContainer SetBeCookie(Uri reqUri, CookieContainer container)
		//{
		//    if (SendBeID && !TwinDll.Be2chCookie.IsEmpty)
		//    {
		//        container.SetCookies(reqUri, "DMDM=" + TwinDll.Be2chCookie.Dmdm);
		//        container.SetCookies(reqUri, "MDMD=" + TwinDll.Be2chCookie.Mdmd);
		//    }
		//    else
		//    {
		//        container.SetCookies(reqUri, "DMDM=");
		//        container.SetCookies(reqUri, "MDMD=");
		//    }

		//    return container;
		//}
		/*
		protected virtual CookieContainer GetCookie(BoardInfo board)
		{
			Uri uri = new Uri("http://" + board.Server + "/");		

			CookieContainer cContainer = new CookieContainer();
			CookieCollection others = new CookieCollection();
			CookieCollection cookies = board.CookieContainer.GetCookies(uri);

			bool dmdm=false, mdmd=false;

			foreach (Cookie c in cookies)
			{
				if (c.Name == "DMDM") dmdm = true;
				else if (c.Name == "MDMD") mdmd = true;
				else						others.Add(c);
			}

			if (SendBeID)
			{
				if (dmdm && mdmd)
				{
					cContainer.Add(cookies);
				}
				else if (!TwinDll.Be2chCookie.IsEmpty)
				{
					cContainer.Add(new Cookie("DMDM", TwinDll.Be2chCookie.Dmdm, "/", board.Server));
					cContainer.Add(new Cookie("MDMD", TwinDll.Be2chCookie.Mdmd, "/", board.Server));
					cContainer.Add(others);
				}
			}
			else {
				cContainer.Add(others);
			}

			return cContainer;
		}*/
	}
}
