// PopupState.cs

namespace Twin
{
	using System;

	/// <summary>
	/// �|�b�v�A�b�v�@�\�̃t���O�񋓑�
	/// </summary>
	[Flags]
	public enum PopupState
	{
		/// <summary>
		/// �|�b�v�A�b�v�@�\�͖���
		/// </summary>
		Disable = 1,
		/// <summary>
		/// �|�b�v�A�b�v�@�\�͗L��
		/// </summary>
		Enable = 2,
		/// <summary>
		/// �L�[��������Ă���Ԃ̂ݗL��
		/// </summary>
		KeySwitch = 4,
	}
}
