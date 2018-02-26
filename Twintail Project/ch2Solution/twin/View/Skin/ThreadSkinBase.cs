// ThreadSkinBase.cs.cs

namespace Twin
{
	using System;

	/// <summary>
	/// �X�L���̊�{�N���X
	/// </summary>
	public abstract class ThreadSkinBase
	{
		/// <summary>
		/// ���X�Q�Ƃ̊�{�ƂȂ�URL���擾�܂��͐ݒ�
		/// </summary>
		public abstract string BaseUri {
			set;
			get;
		}
	
		// �X�L�������擾
		public abstract string Name {
			get;
		}

		/// <summary>
		/// �w�b�_�[���擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public abstract string GetHeader(ThreadHeader header);

		/// <summary>
		/// �t�b�^�[���擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public abstract string GetFooter(ThreadHeader header);

		/// <summary>
		/// ResSet���X�L���𗘗p���ĕ�����ɕϊ�
		/// </summary>
		/// <param name="resSet"></param>
		/// <returns></returns>
		public abstract string Convert(ResSet resSet);

		/// <summary>
		/// ResSet����x�ɕϊ�
		/// </summary>
		/// <param name="resSetCollection"></param>
		/// <returns></returns>
		public abstract string Convert(ResSetCollection resSetCollection);

		/// <summary>
		/// �X���b�h���J���ꂽ���A��x�����Ă΂�܂��B
		/// </summary>
		public abstract void Reset();

		/// <summary>
		/// �X�L����ǂݍ���
		/// </summary>
		/// <param name="skinFolder"></param>
		public abstract void Load(string skinFolder);
	}
}
