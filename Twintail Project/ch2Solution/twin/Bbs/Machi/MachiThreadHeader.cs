// MachiThreadHeader.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// �܂�BBS�̃X���b�h�w�b�_����\��
	/// </summary>
	public class MachiThreadHeader : ThreadHeader
	{
		/// <summary>
		/// dat�t�@�C���̑��݂���URL���擾
		/// </summary>
		public override string DatUrl {
			get {
				throw new NotSupportedException("�܂�BBS��dat�ǂ݂��T�|�[�g���Ă��܂���");
			}
		}

		/// <summary>
		/// �X���b�h��URL���擾
		/// </summary>
		public override string Url {
			get {
				return String.Format("http://{0}/bbs/read.pl?BBS={1}&KEY={2}",
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
		/// MachiThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiThreadHeader() : base()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
