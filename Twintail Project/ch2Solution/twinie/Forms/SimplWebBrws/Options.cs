// Options.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// Option �̊T�v�̐����ł��B
	/// </summary>
	public class Options
	{
		/// <summary>
		/// �^�u������ۂ̓���
		/// </summary>
		public Activation Activation = Activation.Right;
		/// <summary>
		/// �^�u���J�����Ƃ��ɐV�����^�u���A�N�e�B�u�ɂ��邩�ǂ���
		/// </summary>
		public bool Activate = true;
		/// <summary>
		/// �����Ɏg�p����URI
		/// </summary>
		public string SearchUri = "http://www.google.co.jp/search?q={0}&lr=lang_ja";

		public Options()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}

	/// <summary>
	/// �A�N�e�B�u�̕��@
	/// </summary>
	public enum Activation
	{
		/// <summary>
		/// �����^�u�̍����A�N�e�B�u�ɂ���
		/// </summary>
		Left,
		/// <summary>
		/// �����^�u�̉E���A�N�e�B�u�ɂ���
		/// </summary>
		Right,
	}
}
