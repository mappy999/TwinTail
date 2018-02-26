// PatrolEvent.cs

namespace Twin.Tools
{
	using System;

	/// <summary>
	/// PatrolBase�N���X�̃C�x���g���������郁�\�b�h
	/// </summary>
	public delegate void PatrolEventHandler(object sender, PatrolEventArgs e);

	/// <summary>
	/// PatrolBase�N���X�̃C�x���g�f�[�^���
	/// </summary>
	public class PatrolEventArgs : EventArgs
	{
		private readonly ThreadHeader header;
		private bool cancel;

		/// <summary>
		/// ����Ώۂ̃X���b�h�����擾
		/// </summary>
		public ThreadHeader HeaderInfo {
			get { return header; }
		}

		/// <summary>
		/// ������L�����Z������ꍇ��true�ɐݒ�
		/// </summary>
		public bool Cancel {
			set { cancel = value; }
			get { return cancel; }
		}

		/// <summary>
		/// PatrolEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header"></param>
		public PatrolEventArgs(ThreadHeader header)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.header = header;
			this.cancel = false;
		}
	}
}
