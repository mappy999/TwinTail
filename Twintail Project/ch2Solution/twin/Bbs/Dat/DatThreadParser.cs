// DatThreadParser.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using Twin.Text;

	/// <summary>
	/// 2ch��dat�Ƀ��X�Q�Ə���������Ă��邪2ch�݊��̃T�C�g��dat����������Ă��Ȃ����߁A
	/// �������������N���X
	/// </summary>
	public class DatThreadParser : X2chThreadParser
	{
		/// <summary>
		/// DatThreadParser�N���X�̃C���X�^���X��������
		/// </summary>
		public DatThreadParser() : base(BbsType.Dat, Encoding.GetEncoding("Shift_Jis"))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
