// ThreadExtractInfo.cs

namespace Twin.Text
{
	using System;
	using CSharpSamples;

	/// <summary>
	/// ThreadExtractInfo �̊T�v�̐����ł��B
	/// </summary>
	public class ThreadExtractInfo
	{
		private string keyword;
		private SearchOptions options;

		/// <summary>
		/// ���o�L�[���[�h���擾
		/// </summary>
		public string Keyword {
			get { return keyword; }
		}

		/// <summary>
		/// ���o�L�[���[�h���擾
		/// </summary>
		public SearchOptions Options {
			get { return options; }
		}


		/// <summary>
		/// ThreadExtractInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="keyword"></param>
		public ThreadExtractInfo(string keyword, SearchOptions options)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.keyword = keyword;
			this.options = options;
		}
	}
}
