// ServerChangeEvent.cs
// #2.0

namespace Twin
{
	using System;

	/// <summary>
	/// ThreadListReader.ServerChange �C�x���g�̃f�[�^��񋟂��܂��B
	/// </summary>
	public class ServerChangeEventArgs : EventArgs
	{
		private BoardInfo old;
		private BoardInfo _new;
		private BoardInfoCollection traceList;

		/// <summary>
		/// �ړ]���̔����擾���܂��B
		/// </summary>
		public BoardInfo OldBoard
		{
			get
			{
				return old;
			}
		}

		/// <summary>
		/// �ړ]��̔����擾���܂��B
		/// </summary>
		public BoardInfo NewBoard
		{
			get
			{
				return _new;
			}
		}

		/// <summary>
		/// ��ǐՂ����ꍇ�͂����ɒǐ՗������i�[����܂��B
		/// </summary>
		public BoardInfoCollection TraceList
		{
			get
			{
				return traceList;
			}
		}

		/// <summary>
		/// ServerChangeEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="old"></param>
		/// <param name="_new"></param>
		public ServerChangeEventArgs(BoardInfo old, BoardInfo _new)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.old = old;
			this._new = _new;
			this.traceList = null;
		}

		/// <summary>
		/// ServerChangeEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="old"></param>
		/// <param name="_new"></param>
		/// <param name="history"></param>
		public ServerChangeEventArgs(BoardInfo old, BoardInfo _new, BoardInfoCollection history)
			: this(old, _new)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.traceList = history;
		}
	}
}
