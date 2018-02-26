// PostThread.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ���e����X���b�h��\��
	/// </summary>
	public struct PostThread
	{
		private string _subject;
		private string _from;
		private string _email;
		private string _body;

		/// <summary>
		/// ���e�҂̖��O���擾�܂��͐ݒ�
		/// </summary>
		public string From {
			set {
				if (value == null)
					throw new ArgumentNullException("From");

				_from = value;
			}
			get { return _from; }
		}

		/// <summary>
		/// ���e�҂�E-mail���擾�܂��͐ݒ�
		/// </summary>
		public string Email {
			set {
				if (value == null)
					throw new ArgumentNullException("Email");

				_email = value;
			}
			get { return _email; }
		}

		/// <summary>
		/// �{�����擾�܂��͐ݒ�
		/// </summary>
		public string Body {
			set {
				if (value == null)
					throw new ArgumentNullException("Body");

				_body = value;
			}
			get { return _body; }
		}

		/// <summary>
		/// �X���b�h�����擾�܂��͐ݒ�
		/// </summary>
		public string Subject {
			set {
				if (value == null)
					throw new ArgumentNullException("Subject");

				_subject = value;
			}
			get { return _subject; }
		}

		/// <summary>
		/// PostThread�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="subj">�V�K�X���b�h��</param>
		/// <param name="name">���e�҂̖��O</param>
		/// <param name="email">���e�҂�E-mail</param>
		/// <param name="body">�{��</param>
		public PostThread(string subj, string from, string email, string body)
		{
			_subject = subj;
			_from = from;
			_email = email;
			_body = body;
		}

		/// <summary>
		/// PostThread�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="subj">�V�K�X���b�h��</param>
		/// <param name="body">�{��</param>
		public PostThread(string subj, string body)
			: this(subj, String.Empty, String.Empty, body)
		{
		}
	}
}
