// PostResponse.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ���e���̃T�[�o�[����̉��΂�\��
	/// </summary>
	public enum PostResponse
	{
		/// <summary>�w��Ȃ�</summary>
		None,
		/// <summary>���e�ɐ���</summary>
		Success,
		/// <summary>�N�b�L�[�m�F</summary>
		Cookie,
		/// <summary>�^�C���A�E�g�œ��e�ł��Ȃ�</summary>
		Timeout,
		/// <summary>�������݂͏o���������ӕt��</summary>
		Attention,
		/// <summary>���炩�̌x��������</summary>
		Warning,
		/// <summary>���炩�̃G���[������</summary>
		Error,
		/// <summary>Samba�G���[</summary>
		Samba,
	}
}
