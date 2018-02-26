// BoardTableEvent.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// BoardTableEventHandler �f���Q�[�g
	/// </summary>
	public delegate void BoardTableEventHandler(object sender, BoardTableEventArgs e);

	/// <summary>
	/// BoardTableEventArgs �̊T�v�̐����ł��B
	/// </summary>
	public class BoardTableEventArgs : EventArgs
	{
		private readonly BoardInfo board;
		private readonly bool isNewOpen;

		/// <summary>
		/// �I�����ꂽ���擾
		/// </summary>
		public BoardInfo Item {
			get {
				return board;
			}
		}

		/// <summary>
		/// �V�����E�C���h�E�ŊJ�����ǂ������擾
		/// </summary>
		public bool IsNewOpen {
			get {
				return isNewOpen;
			}
		}

		/// <summary>
		/// BoardTableEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		public BoardTableEventArgs(BoardInfo info, bool newOpen)
		{
			if (info == null) {
				throw new ArgumentNullException("info");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			board = info;
			isNewOpen = newOpen;
		}
	}
}
