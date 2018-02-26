// ZetaThreadHeader.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// Zetabbs�p�̃X���b�h�����i�[����N���X
	/// </summary>
	public class ZetaThreadHeader : ThreadHeader
	{
		/// <summary>
		/// dat�̑��݂���p�X���擾
		/// </summary>
		public override string DatUrl {
			get {
				return String.Format("http://{0}/{1}/dat/{2}.dat",
					BoardInfo.Server, BoardInfo.Path, Key);
			}
		}

		/// <summary>
		/// �X���b�h��URL���擾
		/// </summary>
		public override string Url {
			get {
				return String.Format("http://{0}/cgi-bin/test/read.cgi/{1}/{2}/",
					BoardInfo.Server, BoardInfo.Path, Key);
			}
		}

		/// <summary>
		/// �������݉\�ȍő僌�X�����擾
		/// </summary>
		public override int UpperLimitResCount {
			get {
				return 1000;
			}
		}

		/// <summary>
		/// ZetaThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public ZetaThreadHeader()
		{
		}
	}
}
