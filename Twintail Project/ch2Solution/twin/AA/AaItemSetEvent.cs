// AaItemSetEvent.cs

namespace Twin.Aa
{
	using System;

	/// <summary>
	/// AaItemCollection.ItemSet�C�x���g���������郁�\�b�h��\��
	/// </summary>
	internal delegate void AaItemSetEventHandler(object sender, AaItemSetEventArgs e);

	/// <summary>
	/// AaItemCollection.ItemSet�C�x���g�̃f�[�^���
	/// </summary>
	internal class AaItemSetEventArgs : EventArgs
	{
		private readonly AaItem item;

		/// <summary>
		/// �V�����ǉ����ꂽAaItem�N���X�̃C���X�^���X
		/// </summary>
		public AaItem Item {
			get { return item; }
		}

		/// <summary>
		/// AaItemSetEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="aa"></param>
		public AaItemSetEventArgs(AaItem aa)
		{
			if (aa == null) {
				throw new ArgumentNullException("aa");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			item = aa;
		}
	}
}
