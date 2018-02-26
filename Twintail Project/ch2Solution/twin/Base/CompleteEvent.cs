// CompleteEvent.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ClientBase.Complete�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void CompleteEventHandler(object sender,
					CompleteEventArgs e);

	/// <summary>
	/// ClientBase.Complete�C�x���g�̃f�[�^���
	/// </summary>
	public class CompleteEventArgs : EventArgs
	{
		private CompleteStatus status;

		/// <summary>
		/// ������Ԃ�\��
		/// </summary>
		public CompleteStatus Status {
			get { return status; }
		}

		/// <summary>
		/// CompleteEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="status">�N���C�A���g�̊������</param>
		public CompleteEventArgs(CompleteStatus status)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.status = status;
		}
	}
}
