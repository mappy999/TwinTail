// ZetaThreadReader.cs

namespace Twin.Bbs
{
	using System;
	using Twin.IO;

	/// <summary>
	/// Zetabbs�̃X���b�h��ǂݍ��ރN���X (2ch�݊�)
	/// </summary>
	public class ZetaThreadReader : X2chThreadReader
	{
		/// <summary>
		/// ZetaThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		public ZetaThreadReader() : base(new ZetaThreadParser())
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
