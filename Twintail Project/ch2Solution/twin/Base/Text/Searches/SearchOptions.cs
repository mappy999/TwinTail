// SearchOptions.cs

namespace Twin.Text
{
	using System;

	/// <summary>
	/// �����I�v�V������\��
	/// </summary>
	[Flags]
	public enum SearchOptions
	{
		None = 0x000,
		/// <summary>
		/// �h�L�������g�̏I��肩�猟�����J�n����
		/// </summary>
		RightToLeft = 0x001,
		/// <summary>
		/// �P��P�ʂŌ���
		/// </summary>
		WholeWordsOnly = 0x002,
		/// <summary>
		/// �啶���Ə������̋��
		/// </summary>
		MatchCase = 0x004,
		/// <summary>
		/// ���K�\������
		/// </summary>
		Regex = 0x008,
	}
}
