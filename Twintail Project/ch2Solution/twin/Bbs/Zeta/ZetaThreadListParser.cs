// ZetaThreadListParser.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using Twin.Text;

	/// <summary>
	/// Zetabbs�̃X���b�h�ꗗ����� (2ch�݊�)
	/// </summary>
	public class ZetaThreadListParser : X2chThreadListParser
	{
		/// <summary>
		/// ZetaThreadListParser�N���X�̃C���X�^���X��������
		/// </summary>
		public ZetaThreadListParser()
			: base(BbsType.Zeta, Encoding.GetEncoding("Shift_Jis"))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
