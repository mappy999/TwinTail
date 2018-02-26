// JbbsThreadListReader.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using Twin.IO;

	/// <summary>
	/// JbbsThreadListReader �̊T�v�̐����ł��B
	/// </summary>
	public class JbbsThreadListReader : MachiThreadListReader
	{
		/// <summary>
		/// JbbsThreadListReader�N���X�̃C���X�^���X��������
		/// </summary>
		public JbbsThreadListReader()
			: base(new MachiThreadListParser(BbsType.Jbbs, Encoding.GetEncoding("EUC-JP")))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
