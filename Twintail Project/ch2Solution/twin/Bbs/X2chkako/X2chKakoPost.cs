// X2chKakoPost.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// X2chKakoPost �̊T�v�̐����ł��B
	/// </summary>
	public class X2chKakoPost : X2chPost
	{
		/// <summary>
		/// ���̃v���p�e�B�͏��false��Ԃ�
		/// </summary>
		public override bool CanPostRes {
			get { return false; }
		}

		/// <summary>
		/// ���̃v���p�e�B�͏��false��Ԃ�
		/// </summary>
		public override bool CanPostThread {
			get { return false; }
		}

		/// <summary>
		/// X2chKakoPost�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chKakoPost()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// �V�K�X���b�h�𓊍e
		/// </summary>
		/// <param name="board">���e��̔�</param>
		/// <param name="thread">���e������e</param>
		public override void Post(BoardInfo board, PostThread thread)
		{
			throw new NotSupportedException("���̃��\�b�h�̓T�|�[�g���Ă��܂���");
		}

		/// <summary>
		/// ���b�Z�[�W�𓊍e
		/// </summary>
		/// <param name="header">���e��̃X���b�h</param>
		/// <param name="res">���e������e</param>
		public override void Post(ThreadHeader header, PostRes res)
		{
			throw new NotSupportedException("�ߋ����O�ɂ͏������݂ł��܂���");
		}
	}
}
