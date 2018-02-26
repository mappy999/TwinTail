// AbstractSearcher.cs

namespace Twin.Text
{
	using System;
	using CSharpSamples;

	/// <summary>
	/// �����@�\�����������{�N���X
	/// </summary>
	public abstract class AbstractSearcher
	{
		private SearchOptions options;

		/// <summary>
		/// �����I�v�V�������擾�܂��͐ݒ�
		/// </summary>
		public SearchOptions Options {
			set {
				options = value;
			}
			get {
				return options;
			}
		}

		/// <summary>
		/// AbstractSearcher�N���X�̃C���X�^���X��������
		/// </summary>
		protected AbstractSearcher()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			options = SearchOptions.None;
		}
		/// <summary>
		/// ���������Z�b�g
		/// </summary>
		public abstract void Reset();

		/// <summary>
		/// �����J�n
		/// </summary>
		/// <param name="keyword"></param>
		public abstract bool Search(string keyword);

		/// <summary>
		/// �P������ׂăn�C���C�g�\��
		/// </summary>
		public abstract void Highlights(string keyword);
	}
}
