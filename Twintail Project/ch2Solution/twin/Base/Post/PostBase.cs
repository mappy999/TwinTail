// PostBase.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Net;
	using System.Text;
	using System.Web;

	/// <summary>
	/// ���e�N���X�̊�{�N���X
	/// </summary>
	public abstract class PostBase
	{
		private PostResHandler methodR;
		private PostThreadHandler methodT;

		private DateTime time;
		private Encoding encoding;
		private IWebProxy proxy;
		private string userAgent;

		/// <summary>
		/// �V�K�X���b�h�̓��e�ɑΉ����Ă��邩�ǂ������擾
		/// </summary>
		public abstract bool CanPostThread
		{
			get;
		}

		/// <summary>
		/// ���X�̓��e�ɑΉ����Ă��邩�ǂ������擾
		/// </summary>
		public abstract bool CanPostRes
		{
			get;
		}

		/// <summary>
		/// �T�[�o�[����̉��΂��擾
		/// </summary>
		public abstract PostResponse Response
		{
			get;
		}

		/// <summary>
		/// ���b�Z�[�W�̃G���R�[�f�B���O���擾�܂��͐ݒ�
		/// </summary>
		public Encoding Encoding
		{
			set
			{
				if (value == null)
					throw new ArgumentNullException("Encoding");
				encoding = value;
			}
			get
			{
				return encoding;
			}
		}

		/// <summary>
		/// �g�p����v���L�V���擾�܂��͐ݒ�
		/// </summary>
		public IWebProxy Proxy
		{
			set
			{
				proxy = (value != null) ?
					value : WebRequest.DefaultWebProxy;
			}
			get
			{
				return proxy;
			}
		}

		/// <summary>
		/// �g�p����User-Agent�w�b�_���擾�܂��͐ݒ�
		/// </summary>
		public string UserAgent
		{
			set
			{
				if (value == null)
					throw new ArgumentNullException("UserAgent");
				userAgent = value;
			}
			get
			{
				return userAgent;
			}
		}

		/// <summary>
		/// �X�����Ď��Ɏg�p���鎞�����擾�܂��͐ݒ�
		/// </summary>
		public DateTime Time
		{
			set
			{
				time = value;
			}
			get
			{
				return time;
			}
		}

		/// <summary>
		/// ���e�����Ƃ��ɔ���
		/// </summary>
		public event PostEventHandler Posted;

		/// <summary>
		/// ���e�G���[�����������Ƃ��ɔ���
		/// </summary>
		public event PostErrorEventHandler Error;

		/// <summary>
		/// PostBase�N���X�̃C���X�^���X��������
		/// </summary>
		protected PostBase()
		{
			userAgent = TwinDll.UserAgent;
			encoding = TwinDll.DefaultEncoding;
			proxy = WebRequest.DefaultWebProxy;
			time = DateTime.MinValue;
		}

		/// <summary>
		/// �V�K�X���b�h�𓊍e
		/// </summary>
		/// <param name="board">���e��̔�</param>
		/// <param name="thread">���e������e</param>
		public abstract void Post(BoardInfo board, PostThread thread);

		/// <summary>
		/// ���b�Z�[�W�𓊍e
		/// </summary>
		/// <param name="header">���e��̃X���b�h</param>
		/// <param name="res">���e������e</param>
		public abstract void Post(ThreadHeader header, PostRes res);

		/// <summary>
		/// ���e���L�����Z��
		/// </summary>
		public abstract void Cancel();

		/// <summary>
		/// �񓯊��ŐV�K�X���b�h�𓊍e
		/// </summary>
		/// <param name="board">���e��̔�</param>
		/// <param name="thread">���e������e</param>
		/// <param name="callback">���e�������ɌĂ΂��R�[���o�b�N</param>
		/// <param name="state">���[�U�[�w��̃I�u�W�F�N�g</param>
		/// <returns></returns>
		public virtual IAsyncResult BeginPost(BoardInfo board, PostThread thread,
			AsyncCallback callback, object state)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			if (methodR != null ||
				methodT != null)
			{
				throw new InvalidOperationException("��x�ɕ����̔񓯊��Ăяo���͏o���܂���");
			}

			methodT = new PostThreadHandler(Post);
			return methodT.BeginInvoke(board, thread, callback, state);
		}

		/// <summary>
		/// �񓯊��Ń��b�Z�[�W�𓊍e
		/// </summary>
		/// <param name="header">���e��̃X���b�h</param>
		/// <param name="res">���e������e</param>
		/// <param name="callback">���e�������ɌĂ΂��R�[���o�b�N</param>
		/// <param name="state">���[�U�[�w��̃I�u�W�F�N�g</param>
		/// <returns></returns>
		public virtual IAsyncResult BeginPost(ThreadHeader header, PostRes res,
			AsyncCallback callback, object state)
		{
			if (header == null)
				throw new ArgumentNullException("header");

			if (methodR != null ||
				methodT != null)
			{
				throw new InvalidOperationException("��x�ɕ����̔񓯊��Ăяo���͏o���܂���");
			}

			methodR = new PostResHandler(Post);
			return methodR.BeginInvoke(header, res, callback, state);
		}

		/// <summary>
		/// ���e����������܂őҋ@
		/// </summary>
		/// <param name="ar"></param>
		public virtual void EndPost(IAsyncResult ar)
		{
			if (ar == null)
			{
				throw new ArgumentNullException("ar");
			}

			if (methodR != null)
				methodR.EndInvoke(ar);

			else if (methodT != null)
				methodT.EndInvoke(ar);

			else
			{
				throw new InvalidOperationException("�񓯊��Ăяo�����s���Ă��܂���");
			}

			methodR = null;
			methodT = null;
		}

		/// <summary>
		/// Posted�C�x���g�𔭐�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnPosted(object sender, PostEventArgs e)
		{
			if (Posted != null)
				Posted(sender, e);
		}

		/// <summary>
		/// Error�C�x���g�𔭐�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnError(object sender, PostErrorEventArgs e)
		{
			if (Error != null)
				Error(sender, e);
		}

		/// <summary>
		/// HttpUtility.UrlEncode���g�p����text���G���R�[�h����
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		protected string UrlEncode(string text)
		{
			return HttpUtility.UrlEncode(text, encoding);
		}

		/// <summary>
		/// ���e����time�l���擾
		/// </summary>
		/// <param name="baseTime">��ɂȂ����</param>
		/// <returns></returns>
		public static int GetTime(DateTime baseTime)
		{
			TimeSpan t = baseTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1));
			return (int)(t.TotalSeconds);
		}
	}
}
