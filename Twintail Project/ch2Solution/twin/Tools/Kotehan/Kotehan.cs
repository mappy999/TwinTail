// Kotehan.cs

namespace Twin.Tools
{
	using System;

	/// <summary>
	/// �P�̃R�e�n����\��
	/// </summary>
	public class Kotehan
	{
		private string name;
		private string email;
		private bool be;

		/// <summary>
		/// ���O���擾�܂��͐ݒ�
		/// </summary>
		public string Name {
			set {
				if (value == null)
					throw new ArgumentNullException("Name");
				name = value;
			}
			get { return name; }
		}

		/// <summary>
		/// E-mail�A�h���X���擾�܂��͐ݒ�
		/// </summary>
		public string Email {
			set {
				if (value == null)
					throw new ArgumentNullException("Email");
				email = value;
			}
			get { return email; }
		}

		/// <summary>
		/// BeID�𑗐M����Ȃ� true�A����ȊO�� false ��\���B
		/// </summary>
		public bool Be {
			set {
				be = value;
			}
			get { return be; }
		}

		/// <summary>
		/// ��̃R�e�n�����ǂ����𔻒f
		/// </summary>
		public bool IsEmpty {
			get {
				return (name == "" && email == "" && be == false) ? true : false;
			}
		}

		/// <summary>
		/// Kotehan�N���X�̃C���X�^���X��������
		/// </summary>
		public Kotehan()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			name = String.Empty;
			email = String.Empty;
			be = false;
		}

		/// <summary>
		/// Kotehan�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="name">���O</param>
		/// <param name="email">Email�A�h���X</param>
		/// <param name="be">Be��On/Off</param>
		public Kotehan(string name, string email, bool be)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (email == null)
				throw new ArgumentNullException("email");

			this.name = name;
			this.email = email;
			this.be = be;
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("���O: {0}, E-mail: {1}, Be={2}",
				name, email, be);
		}
	}
}
