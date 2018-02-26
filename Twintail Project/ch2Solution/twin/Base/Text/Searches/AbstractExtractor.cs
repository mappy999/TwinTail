// AbstractExtractor.cs

namespace Twin.Text
{
	using System;
	using Twin.Text;
	using CSharpSamples;

	/// <summary>
	/// ���X���o�@�\�����������{�N���X
	/// </summary>
	public abstract class AbstractExtractor
	{
		private SearchOptions options;
		private bool newWindow;

		/// <summary>
		/// �����I�v�V�������擾�܂��͐ݒ�
		/// </summary>
		public SearchOptions Options {
			set { options = value; }
			get { return options; }
		}

		/// <summary>
		/// ���o���ʂ�V�����E�C���h�E�ŕ\�����邩�ǂ���
		/// </summary>
		public bool NewWindow {
			set {
				if (newWindow != value)
					newWindow = value;
			}
			get { return newWindow; }
		}

		/// <summary>
		/// AbstractExtractor�N���X�̃C���X�^���X��������
		/// </summary>
		public AbstractExtractor()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			options = SearchOptions.None;
			newWindow = false;
		}

		
		/// <summary>
		/// �w�肵���L�[���[�h���܂ރ��X�𒊏o
		/// </summary>
		/// <param name="keyword">�����L�[���[�h</param>
		/// <param name="element">�����Ώۂ̗v�f</param>
		/// <returns></returns>
		public abstract ResSetCollection Extract(string keyword, ResSetElement element);

		/// <summary>
		/// �w�肵���L�[���[�h���܂ރ��X�𒊏o���\��
		/// </summary>
		/// <param name="keyword">�����L�[���[�h</param>
		/// <param name="element">�����Ώۂ̗v�f</param>
		public abstract bool InnerExtract(string keyword, ResSetElement element);

		/// <summary>
		/// ���ׂẴ����N���擾
		/// </summary>
		/// <returns></returns>
		public abstract LinkCollection GetLinks();

		public abstract void Reset();
	}
}
