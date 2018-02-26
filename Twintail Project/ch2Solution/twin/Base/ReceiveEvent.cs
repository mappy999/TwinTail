// ReceiveEvent.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ClientBase.Receive�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void ReceiveEventHandler(object sender, ReceiveEventArgs e);

	/// <summary>
	/// ClientBase.Receive�C�x���g�̃f�[�^���
	/// </summary>
	public class ReceiveEventArgs : EventArgs
	{
		private readonly int length;
		private readonly int position;
		private readonly int receive;
		
		/// <summary>
		/// �X�g���[���̒������擾
		/// </summary>
		public int Length {
			get { return length; }
		}
		
		/// <summary>
		/// ���݂̃X�g���[���ʒu���擾
		/// </summary>
		public int Position {
			get { return position; }
		}

		/// <summary>
		/// ��M���ꂽ�T�C�Y���擾
		/// </summary>
		public int Receive {
			get { return receive; }
		}

		/// <summary>
		/// ReceiveEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="len">��M�Ώۂ̑��T�C�Y</param>
		/// <param name="pos">��M�ς݃T�C�Y</param>
		/// <param name="recv">����ǂݍ��܂ꂽ�T�C�Y</param>
		public ReceiveEventArgs(int len, int pos, int recv)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			length = len;
			position = pos;
			receive = recv;
		}
	}
}
