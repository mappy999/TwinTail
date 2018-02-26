// AutoRefreshTimerBase.cs

namespace Twin.Tools
{
	using System;

	/// <summary>
	/// ThreadControl���^�C�}�[�Œ���I�ɍX�V�����{�N���X
	/// </summary>
	public abstract class AutoRefreshTimerBase
	{
		/// <summary>
		/// �X�V�Ԋu���~���b�P�ʂŎ擾�܂��͐ݒ�B
		/// </summary>
		public abstract int Interval {
			set;
			get;
		}

		/// <summary>
		/// AutoRefreshTimerBase�N���X�̃C���X�^���X��������
		/// </summary>
		protected AutoRefreshTimerBase()
		{}

		/// <summary>
		/// ���X�g�ɃN���C�A���g��ǉ��B
		/// ���ɓ����N���C�A���g���o�^����Ă���Ή������Ȃ��B
		/// </summary>
		/// <param name="client">�����X�V�̑ΏۂƂ���N���C�A���g</param>
		public abstract void Add(ThreadControl client);

		/// <summary>
		/// ���X�g����N���C�A���g���폜�B
		/// �w�肵���N���C�A���g�����X�g�ɑ��݂��Ȃ���Ή������Ȃ��B
		/// </summary>
		/// <param name="client">�����X�V�̑Ώۂ���O���N���C�A���g</param>
		public abstract void Remove(ThreadControl client);

		/// <summary>
		/// �w�肵���N���C�A���g�����X�g���ɑ��݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="client">��������N���C�A���g</param>
		/// <returns>���X�g���ɑ��݂����true�A���݂��Ȃ����false��Ԃ�</returns>
		public abstract bool Contains(ThreadControl client);

		/// <summary>
		/// �w�肵���N���C�A���g�̍X�V�Ԋu���擾�B
		/// </summary>
		/// <param name="client"></param>
		/// <returns>���̍X�V�܂ł̕b���Bclient ���^�C�}�[�ɓo�^����Ă��Ȃ��A
		/// �܂��̓^�C�}�[����~���Ă���ꍇ�� -1 ��Ԃ��B</returns>
		public abstract int GetInterval(ThreadControl client);

		/// <summary>
		/// �w�肵���N���C�A���g�̎��̍X�V�܂ł̎c��b����Ԃ��B
		/// </summary>
		/// <param name="client"></param>
		/// <returns>���̍X�V�܂ł̕b���Bclient ���^�C�}�[�ɓo�^����Ă��Ȃ��A
		/// �܂��̓^�C�}�[����~���Ă���ꍇ�� -1 ��Ԃ��B</returns>
		public abstract int GetTimeleft(ThreadControl client);

		public abstract ITimerObject GetTimerObject(ThreadControl client);

		/// <summary>
		/// ���ׂẴ^�C�}�[���폜
		/// </summary>
		public abstract void Clear();
	}
}
