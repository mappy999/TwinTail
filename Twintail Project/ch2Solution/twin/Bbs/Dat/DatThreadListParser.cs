// DatThreadListParser.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;

	/// <summary>
	/// DatThreadListParser �̊T�v�̐����ł��B
	/// </summary>
	public class DatThreadListParser : X2chThreadListParser
	{
		/// <summary>
		/// DatThreadListParser�N���X�̃C���X�^���X��������
		/// </summary>
		public DatThreadListParser() : base(BbsType.Dat, Encoding.GetEncoding("Shift_Jis"))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
