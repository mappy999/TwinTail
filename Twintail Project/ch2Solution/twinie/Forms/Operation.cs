// Operation.cs

namespace Twin.Forms
{
	using System;
	using System.ComponentModel;

	/// <summary>
	/// �J���ۂ̋��ʑ����\��
	/// </summary>
	public enum OpenMode
	{
		[Description("�V���O���N���b�N")]
		SingleClick,
		[Description("�_�u���N���b�N")]
		DoubleClick,
	}

	/// <summary>
	/// �^�u�̑����\��
	/// </summary>
	public enum TabOperation
	{
		[Description("�Ȃ�")]
		None,
		[Description("�X�V")]
		Reload,
		[Description("����")]
		Close,
	}

	/// <summary>
	/// �X���b�h�ꗗ�̑���
	/// </summary>
	public enum ListOperation
	{
		[Description("�Ȃ�")]
		None,
		[Description("�J��")]
		Open,
		[Description("�V�K�^�u�ŊJ��")]
		NewOpen,
		[Description("1���|�b�v�A�b�v")]
		Popup1,
		[Description("�V���|�b�v�A�b�v")]
		NewResPopup,
		[Description("���O���폜")]
		Delete,
	}
}
