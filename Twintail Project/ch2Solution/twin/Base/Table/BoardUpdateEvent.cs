// BoardUpdateEvent.cs

namespace Twin
{
	using System;

	/// <summary>
	/// IBoardTable.OnlineUpdate���\�b�h�Ɏg�p����f���Q�[�g
	/// </summary>
	public delegate void BoardUpdateEventHandler(object sender, BoardUpdateEventArgs e);

	/// <summary>
	/// BoardUpdateEventHandler���\�b�h�̃C�x���g�f�[�^
	/// </summary>
	public class BoardUpdateEventArgs : EventArgs
	{
		private BoardUpdateEvent _event;
		private BoardInfo oldBoard;
		private BoardInfo newBoard;

		/// <summary>
		/// �ꗗ�C�x���g�̓��e���擾
		/// </summary>
		public BoardUpdateEvent Event
		{
			get
			{
				return _event;
			}
		}

		/// <summary>
		/// �X�V�O�̔����擾 (Event��Change�̎��̂�)
		/// </summary>
		public BoardInfo OldBoard
		{
			get
			{
				return oldBoard;
			}
		}

		/// <summary>
		/// �V���������擾
		/// </summary>
		public BoardInfo NewBoard
		{
			get
			{
				return newBoard;
			}
		}

		/// <summary>
		/// BoardUpdateEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="_event">�C�x���g�̓��e</param>
		/// <param name="old">���ړ]�����Ƃ��݈̂ړ]�O�̔����w��</param>
		/// <param name="_new">�V�������</param>
		public BoardUpdateEventArgs(BoardUpdateEvent _event, BoardInfo old, BoardInfo _new)
		{
			if (_event == BoardUpdateEvent.Change &&
				old == null)
			{
				throw new ArgumentNullException("old");
			}

			this._event = _event;
			this.oldBoard = old;
			this.newBoard = _new;
		}
	}

	/// <summary>
	/// �X�V�C�x���g�̓��e
	/// </summary>
	public enum BoardUpdateEvent
	{
		/// <summary>
		/// ��URL���ύX���ꂽ
		/// </summary>
		Change,
		/// <summary>
		/// �V�����������ǉ����ꂽ
		/// </summary>
		New,
		/// <summary>
		/// �̍X�V���L�����Z�����ꂽ
		/// </summary>
		Cancelled,
	}
}
