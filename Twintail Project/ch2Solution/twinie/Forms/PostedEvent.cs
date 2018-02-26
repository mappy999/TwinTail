// PostedEvent.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// ���e�����ނ�\��
	/// </summary>
	public enum PostType
	{
		Thread,
		Res,
	}

	/// <summary>
	/// PostedEventHandler �f���Q�[�g
	/// </summary>
	public delegate void PostedEventHandler(object sender, PostedEventArgs e);

	/// <summary>
	/// PostEvent �̊T�v�̐����ł��B
	/// </summary>
	public class PostedEventArgs : EventArgs
	{
		private PostType type;
		private BoardInfo boardInfo;
		private ThreadHeader headerInfo;

		private string title;
		private string from;
		private string email;
		private string body;

		/// <summary>
		/// �X�����Ă����X���������l���擾
		/// </summary>
		public PostType Type {
			get { return type; }
		}

		/// <summary>
		/// �X�����Ď��͂����ɔ�񂪊i�[�����
		/// </summary>
		public BoardInfo BoardInfo {
			get { return boardInfo; }
		}

		/// <summary>
		/// ���X�������ݎ��ɂ͂����ɃX���b�h������i�[�����
		/// </summary>
		public ThreadHeader HeaderInfo {
			get { return headerInfo; }
		}

		/// <summary>
		/// �������񂾃X���b�h�����擾
		/// </summary>
		public string Subject {
			get { return title; }
		}

		/// <summary>
		/// ���e�Җ����擾
		/// </summary>
		public string From {
			get { return from; }
		}

		/// <summary>
		/// E-mail�A�h���X���擾
		/// </summary>
		public string Email {
			get { return email; }
		}

		/// <summary>
		/// ���e�����{�����擾
		/// </summary>
		public string Body {
			get { return body; }
		}

		/// <summary>
		/// PostedEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="board"></param>
		/// <param name="thread"></param>
		public PostedEventArgs(BoardInfo board, PostThread thread)
		{
			title = thread.Subject;
			from = thread.From;
			email = thread.Email;
			body = thread.Body;
			boardInfo = board;
			type = PostType.Thread;
		}

		/// <summary>
		/// PostedEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header"></param>
		/// <param name="res"></param>
		public PostedEventArgs(ThreadHeader header, PostRes res)
		{
			title = header.Subject;
			from = res.From;
			email = res.Email;
			body = res.Body;
			headerInfo = header;
			type = PostType.Res;
		}
	}
}
