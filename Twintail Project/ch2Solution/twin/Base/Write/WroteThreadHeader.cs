// WroteThreadHeader.cs

namespace Twin
{
	using System;

	/// <summary>
	/// �������ݗ����̃w�b�_���
	/// </summary>
	public class WroteThreadHeader
	{
		private BoardInfo board;
		private string key;
		private string subject;
		private int wroteCount;

		/// <summary>
		/// �����擾�܂��͐ݒ�
		/// </summary>
		public BoardInfo BoardInfo {
			set {
				if (value == null) {
					throw new ArgumentNullException("BoardInfo");
				}
				board = value;
			}
			get { return board; }
		}

		/// <summary>
		/// �X���b�h�̔ԍ����擾�܂��͐ݒ�
		/// </summary>
		public string Key {
			set {
				if (value == null) {
					throw new ArgumentNullException("Key");
				}
				key = value;
			}
			get { return key; }
		}

		/// <summary>
		/// �X���b�h�����擾�܂��͐ݒ�
		/// </summary>
		public string Subject {
			set {
				if (value == null) {
					throw new ArgumentNullException("Subject");
				}
				subject = value;
			}
			get { return subject; }
		}

		/// <summary>
		/// �������ݗ��𐔂��擾�܂��͐ݒ�
		/// </summary>
		public int WroteCount {
			set { wroteCount = value; }
			get { return wroteCount; }
		}

		/// <summary>
		/// WroteThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public WroteThreadHeader()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			key = String.Empty;
			subject = String.Empty;
			board = null;
			wroteCount = 0;
		}

		/// <summary>
		/// WroteThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header"></param>
		public WroteThreadHeader(ThreadHeader header) : this()
		{
			key = header.Key;
			board = header.BoardInfo;
			subject = header.Subject;
		}
	}
}
