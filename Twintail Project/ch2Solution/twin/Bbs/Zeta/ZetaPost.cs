// ZetaPost.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;

	/// <summary>
	/// Zetabbs�ɓ��e����N���X
	/// </summary>
	public class ZetaPost : X2chPost
	{
		/// <summary>
		/// ZetaPost�N���X�̃C���X�^���X��������
		/// </summary>
		public ZetaPost()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
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

				// CGI�̑��݂���URL���쐬
				string uri = String.Format("http://{0}/cgi-bin/test/bbs.cgi", board.Server);

				StringBuilder sb = new StringBuilder();
				sb.Append("subject=" + UrlEncode(thread.Subject));
				sb.Append("&submit=" + UrlEncode("�V�K�X���b�h�쐬"));
				sb.Append("&FROM=" + UrlEncode(thread.From));
				sb.Append("&mail=" + UrlEncode(thread.Email));
				sb.Append("&MESSAGE=" + UrlEncode(thread.Body));
				sb.Append("&bbs=" + board.Path);
				sb.Append("&time=" + time);

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
				bool retried = false;

				string uri = String.Format("http://{0}/cgi-bin/test/bbs.cgi", header.BoardInfo.Server);
				StringBuilder sb = new StringBuilder();
				sb.Append("submit=" + UrlEncode("������"));
				sb.Append("&FROM=" + UrlEncode(res.From));
				sb.Append("&mail=" + UrlEncode(res.Email));
				sb.Append("&MESSAGE=" + UrlEncode(res.Body));
				sb.Append("&bbs=" + header.BoardInfo.Path);
				sb.Append("&key=" + header.Key);
				sb.Append("&time=" + time);

				byte[] bytes = Encoding.GetBytes(sb.ToString());
				Posting(header.BoardInfo, bytes, uri, ref retried);
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
				throw ex;
			}
		}
	}
}
