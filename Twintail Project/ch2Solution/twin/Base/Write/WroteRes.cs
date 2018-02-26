// WroteRes.cs

namespace Twin
{
	using System;
	using System.Text;

	/// <summary>
	/// �������ݗ�����\��
	/// </summary>
	public class WroteRes
	{
		private DateTime date;
		private string from;
		private string email;
		private string message;

		public ThreadHeader HeaderInfo { get; set; }

		/// <summary>
		/// ���̃��X�����������t�E�����Ŏ擾
		/// </summary>
		public DateTime Date {
			set { date = value; }
			get { return date; }
		}

		/// <summary>
		/// ���e�҂̖��O���擾�܂��͐ݒ�
		/// </summary>
		public string From {
			set {
				if (value == null) {
					throw new ArgumentNullException("From");
				}
				from = value;
			}
			get { return from; }
		}

		/// <summary>
		/// ���e�҂�Email���擾�܂��͐ݒ�
		/// </summary>
		public string Email {
			set {
				if (value == null) {
					throw new ArgumentNullException("Email");
				}
				email = value;
			}
			get { return email; }
		}

		/// <summary>
		/// ���b�Z�[�W���擾�܂��͐ݒ�
		/// </summary>
		public string Message {
			set {
				if (value == null) {
					throw new ArgumentNullException("Message");
				}
				message = value;
			}
			get { return message; }
		}

		/// <summary>
		/// Message�v���p�e�B�̒������o�C�g�P�ʂŎ擾
		/// </summary>
		public int Length {
			get {
				return TwinDll.DefaultEncoding.GetByteCount(message);
			}
		}

		/// <summary>
		/// WroteRes�N���X�̃C���X�^���X��������
		/// </summary>
		public WroteRes()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.HeaderInfo = null;
			this.from = String.Empty;
			this.email = String.Empty;
			this.message = String.Empty;
		}

		/// <summary>
		/// WroteRes�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="date">���e��</param>
		/// <param name="from">���e�Җ�</param>
		/// <param name="email">���e�҂�email</param>
		/// <param name="msg">���b�Z�[�W</param>
		public WroteRes(ThreadHeader header, DateTime date, string from, string email, string msg) : this()
		{
			this.HeaderInfo = header;
			this.Date = date;
			this.From = from;
			this.Email = email;
			this.Message = msg;
		}

		public WroteRes(DateTime date, string from, string email, string msg)
			: this(null, date, from, email, msg)
		{
		}
	}
}
