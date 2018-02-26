// AaItemEvent.cs

namespace Twin.Aa
{
	using System;

	/// <summary>
	/// AaItemEventHandler �f���Q�[�g
	/// </summary>
	public delegate void AaItemEventHandler(object sender, AaItemEventArgs e);

	/// <summary>
	/// AaItemEventArgs �̊T�v�̐����ł��B
	/// </summary>
	public class AaItemEventArgs : EventArgs
	{
		private readonly AaItem item;

		/// <summary>
		/// AaItem���擾
		/// </summary>
		public AaItem Item {
			get { return item; }
		}

		/// <summary>
		/// AaItemEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="aa"></param>
		public AaItemEventArgs(AaItem aa)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			item = aa;
		}
	}
}
