// DatThreadListReader.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// DatThreadListReader �̊T�v�̐����ł��B
	/// </summary>
	public class DatThreadListReader : X2chThreadListReader
	{
		/// <summary>
		/// DatThreadListReader�N���X�̃C���X�^���X��������
		/// </summary>
		public DatThreadListReader() : base(new DatThreadListParser())
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
