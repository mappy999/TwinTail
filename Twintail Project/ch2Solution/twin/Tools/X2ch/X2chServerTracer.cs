// X2chServerTracer.cs

namespace Twin.Tools
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Net;
	using Twin.Text;

	/// <summary>
	/// 2ch�̃T�[�o�[�ړ]�ǐ�
	/// </summary>
	public class X2chServerTracer
	{
		private BoardInfoCollection traceList;
		private BoardInfo result;

		/// <summary>
		/// �̒ǐ՗������擾
		/// </summary>
		public BoardInfoCollection TraceList {
			get { return traceList; }
		}

		/// <summary>
		/// �ړ]����擾
		/// </summary>
		public BoardInfo Result {
			get { return result; }
		}

		/// <summary>
		/// �̒ǐՂɐ��������Ƃ��ɔ�������C�x���g
		/// </summary>
		public event EventHandler<ServerChangeEventArgs> Tracing;

		/// <summary>
		/// X2chServerTracer�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chServerTracer()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			traceList = new BoardInfoCollection();
			result = null;
		}

		/// <summary>
		/// �w�肵���̈ړ]��ǐ�
		/// </summary>
		/// <param name="board">�ǐՂ����</param>
		/// <param name="recursive">�ړ]�悪����Ɉړ]���Ă����ꍇ�A�ċN�ǐՂ��邩�ǂ���</param>
		/// <returns>�ǐՂł����true�A���s�����false��Ԃ�</returns>
		public bool Trace(BoardInfo board, bool recursive)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			traceList.Clear();
			result = null;
Check:
			// Html�f�[�^���擾
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(board.Url);
			req.UserAgent = TwinDll.UserAgent;
			req.AddRange(0, 499);

			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			string html;
			
			using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("Shift_Jis")))
				html = sr.ReadToEnd();

			res.Close();
			
			// �T�[�o�[�ړ]
			if (html.IndexOf("2chbbs..") >= 0)
			{
				TwinDll.Output("{0} ���ړ]���Ă��܂��B", board.Url);

				// �ړ]���URL���擾
				Match m = Regex.Match(html, "<a href=\"(?<url>.+?)\">GO !</a>", RegexOptions.IgnoreCase);
				if (m.Success)
				{
					string newUrl = m.Groups["url"].Value;

					TwinDll.Output("�ړ]�� {0} �𔭌����܂����B", newUrl);

					result = URLParser.ParseBoard(m.Groups["url"].Value);
					if (result != null)
					{
						result.Name = board.Name;
						traceList.Add(result);

						OnTracing(new ServerChangeEventArgs(board, result));

						if (recursive)
						{
							board = result;
							html = null;

							goto Check;
						}
					}
				}
			}
			// �ǐՏI�������ꍇ�͔����擾
			else if (result != null)
			{
				if (String.IsNullOrEmpty(result.Name))
				{
					Match m = Regex.Match(html, "<title>(?<t>.+?)</title>", RegexOptions.IgnoreCase);
					if (m.Success)
					{
						result.Name = m.Groups["t"].Value;
					}
				}
				TwinDll.Output("{0} �̒ǐՂɐ������܂����B", result.Name);
			}

			return (result != null) ? true : false;
		}

		/// <summary>
		/// Tracing�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnTracing(ServerChangeEventArgs e)
		{
			if (Tracing != null)
				Tracing(this, e);
		}
	}
}
