// Draft.cs

namespace Twin.Tools
{
	using System;

	/// <summary>
	/// ���b�Z�[�W�̑��e��\��
	/// </summary>
	public class Draft
	{
		private ThreadHeader headerInfo;
		private PostRes postRes;

		/// <summary>
		/// ���e��̃X���b�h�����擾
		/// </summary>
		public ThreadHeader HeaderInfo {
			get { return headerInfo; }
		}

		/// <summary>
		/// ���e���郁�b�Z�[�W���擾
		/// </summary>
		public PostRes PostRes {
			get { return postRes; }
		}

		/// <summary>
		/// Draft�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header">���e��̃X���b�h���</param>
		/// <param name="res">���e���b�Z�[�W</param>
		public Draft(ThreadHeader header, PostRes res)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.headerInfo = header;
			this.postRes = res;
		}
	}
}
