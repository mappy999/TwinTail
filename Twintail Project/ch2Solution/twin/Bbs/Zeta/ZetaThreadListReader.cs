// ZetaThreadListReader.cs

namespace Twin.Bbs
{
	using System;
	using Twin.IO;

	/// <summary>
	/// Zetabbs�̃X���b�h�ꗗ��ǂݍ��ރN���X (2ch�݊�)
	/// </summary>
	public class ZetaThreadListReader : X2chThreadListReader
	{
		/// <summary>
		/// ZetaThreadListReader�N���X�̃C���X�^���X��������
		/// </summary>
		public ZetaThreadListReader() : base(new ZetaThreadListParser())
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
