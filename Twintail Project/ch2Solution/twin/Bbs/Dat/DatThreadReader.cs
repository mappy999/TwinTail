// DatThreadReader.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// DatThreadReader �̊T�v�̐����ł��B
	/// </summary>
	public class DatThreadReader : X2chThreadReader
	{
		public DatThreadReader() : base(new DatThreadParser())
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
