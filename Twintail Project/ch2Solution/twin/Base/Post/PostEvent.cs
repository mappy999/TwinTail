// PostEvent.cs

namespace Twin
{
	using System;

	/// <summary>
	/// IPost.BeginPost���\�b�h�̔񓯊����������邽�߂̃f���Q�[�g��\��
	/// </summary>
	internal delegate void PostThreadHandler(BoardInfo board, PostThread thread);

	/// <summary>
	/// IPost.BeginPost���\�b�h�̔񓯊����������邽�߂̃f���Q�[�g��\��
	/// </summary>
	internal delegate void PostResHandler(ThreadHeader header, PostRes res);

	/// <summary>
	/// IPost.Posted�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void PostEventHandler(object sender, PostEventArgs e);

	/// <summary>
	/// IPost.Error�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void PostErrorEventHandler(object sender, PostErrorEventArgs e);

	/// <summary>
	/// IPost.Posted�C�x���g�̃f�[�^���
	/// </summary>
	public class PostEventArgs : EventArgs
	{
		private readonly PostResponse response;
		private readonly string text;
		private readonly string title;
		private readonly string cookie;
		private readonly int sambaCount;
		private bool retry;

		/// <summary>
		/// �ēx���e����ꍇ��true�ɐݒ�
		/// </summary>
		public bool Retry
		{
			set
			{
				if (retry != value)
					retry = value;
			}
			get
			{
				return retry;
			}
		}

		/// <summary>
		/// �T�[�o�[����Ԃ��ꂽ�N�b�L�[���擾
		/// </summary>
		public string Cookie
		{
			get
			{
				return cookie;
			}
		}

		/// <summary>
		/// Samba�G���[���̎��̂݁A�T�[�o�[��Samba�b�����擾
		/// </summary>
		public int SambaCount
		{
			get
			{
				return sambaCount;
			}
		}

		/// <summary>
		/// ���e���ɃT�[�o�[����A���Ă�����Ԃ��擾
		/// </summary>
		public PostResponse Response
		{
			get
			{
				return response;
			}
		}

		/// <summary>
		/// �^�C�g�����擾
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
		}

		/// <summary>
		/// ���e���ɃT�[�o�[����A���Ă������b�Z�[�W���擾
		/// </summary>
		public string Text
		{
			get
			{
				return text;
			}
		}

		/// <summary>
		/// PostEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="res">�T�[�o�[����̉��Ώ�Ԃ�\��</param>
		/// <param name="title">���b�Z�[�W�̃^�C�g����\��</param>
		/// <param name="message">���b�Z�[�W�̖{����\��</param>
		public PostEventArgs(PostResponse res, string title, string message, string cookie, int samba)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.retry = false;
			this.title = title;
			this.cookie = cookie;
			this.response = res;
			this.text = message;
			this.sambaCount = samba;
		}
	}

	/// <summary>
	/// IPost.Error�C�x���g�̃f�[�^���
	/// </summary>
	public class PostErrorEventArgs : EventArgs
	{
		private readonly Exception exception;

		/// <summary>
		/// ����������O���擾
		/// </summary>
		public Exception Exception
		{
			get
			{
				return exception;
			}
		}

		/// <summary>
		/// PostErrorEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="ex">��O�N���X�̃C���X�^���X</param>
		public PostErrorEventArgs(Exception ex)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.exception = ex;
		}
	}
}
