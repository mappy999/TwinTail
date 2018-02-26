// PostRes.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ���e���郌�X�̓��e��\��
	/// </summary>
	public struct PostRes
	{
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
		/// PostRes�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="name">���e�҂̖��O</param>
		/// <param name="email">���e�҂�E-mail</param>
		/// <param name="body">�{��</param>
		public PostRes(string from, string email, string body)
		{
			_from = from;
			_email = email;
			_body = body;
		}

		/// <summary>
		/// PostRes�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="body">�{��</param>
		public PostRes(string body)
			: this(String.Empty, String.Empty, body)
		{
		}
	}
}
