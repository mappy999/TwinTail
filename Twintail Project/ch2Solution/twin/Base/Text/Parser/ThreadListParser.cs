// ThreadListParser.cs

namespace Twin.Text
{
	using System;
	using System.Text;

	/// <summary>
	/// �X���b�h�ꗗ����͂���p�[�T
	/// </summary>
	public abstract class ThreadListParser : PartialDataParser<ThreadHeader>
	{
		/// <summary>
		/// ThreadListParser�N���X�̃C���X�^���X��������
		/// </summary>
		protected ThreadListParser(BbsType bbs, Encoding encoding)
			: base(bbs, encoding)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
