// Be2chThreadListReader.cs

using System;
using System.Text;

namespace Twin.Bbs
{
	/// <summary>
	/// Be2chThreadListReader �̊T�v�̐����ł��B
	/// </summary>
	public class Be2chThreadListReader : X2chThreadListReader
	{
		/// <summary>
		/// Be2chThreadListReader �N���X�̃C���X�^���X��������
		/// </summary>
		public Be2chThreadListReader()
			: base(new X2chThreadListParser(BbsType.Be2ch, Encoding.GetEncoding("euc-jp")))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
