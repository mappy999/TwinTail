// ThreadParser.cs

namespace Twin.Text
{
	using System;
	using System.Text;

	/// <summary>
	/// �X���b�h�̃��X����͂���p�[�T
	/// </summary>
	public abstract class ThreadParser : PartialDataParser<ResSet>
	{
		/// <summary>
		/// ThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="enc"></param>
		public ThreadParser(BbsType bbs, Encoding enc)
			: base(bbs, enc)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
