// StatusTextEvent.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ClientBase.StatusTextEventHandler�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void StatusTextEventHandler(object sender, 
								StatusTextEventArgs e);

	/// <summary>
	/// ClientBase.StatusTextEventHandler�C�x���g�̃f�[�^���
	/// </summary>
	public class StatusTextEventArgs : EventArgs
	{
		private readonly string text;

		/// <summary>
		/// �X�e�[�^�X���b�Z�[�W���擾
		/// </summary>
		public string Text {
			get { return text; }
		}

		/// <summary>
		/// StatusTextEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="text">�X�e�[�^�X���b�Z�[�W</param>
		public StatusTextEventArgs(string text)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.text = text;
		}
	}
}
