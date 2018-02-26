// JbbsThreadHeader.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// JbbsThreadHeader �̊T�v�̐����ł��B
	/// </summary>
	public class JbbsThreadHeader : MachiThreadHeader
	{
		/// <summary>
		/// �X���b�h��URL���擾
		/// </summary>
		public override string Url {
			get {
				return String.Format("http://{0}/bbs/read.cgi/{1}/{2}/",
					BoardInfo.Server, BoardInfo.Path, Key);
			}
		}

		public override string DatUrl
		{
			get
			{
				return String.Format("http://{0}/bbs/rawmode.cgi/{1}/{2}/",
					BoardInfo.Server, BoardInfo.Path, Key);
			}
		}

		/// <summary>
		/// �������݉\�ȍő僌�X�����擾
		/// </summary>
		public override int UpperLimitResCount {
			get {
				return 10000; // ������΂̍ő僌�X�����킩��Ȃ��̂œK���Ɂc
			}
		}

		/// <summary>
		/// JbbsThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public JbbsThreadHeader()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
