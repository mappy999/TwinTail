// Be2chThreadReader.cs

using System;
using System.Text;

namespace Twin.Bbs
{
	/// <summary>
	/// Be2chThreadReader �̊T�v�̐����ł��B
	/// </summary>
	public class Be2chThreadReader : X2chThreadReader
	{
		/// <summary>
		/// Be2chThreadReader �N���X�̃C���X�^���X��������
		/// </summary>
		public Be2chThreadReader()
			: base(new X2chThreadParser(BbsType.Be2ch, Encoding.GetEncoding("euc-jp")))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
