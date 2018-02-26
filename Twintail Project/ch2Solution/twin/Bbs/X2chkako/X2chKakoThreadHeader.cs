// X2chKakoThreadHeader.cs

namespace Twin.Bbs
{
	using System;
	using System.IO;

	/// <summary>
	/// X2chKakoThreadHeader �̊T�v�̐����ł��B
	/// </summary>
	public class X2chKakoThreadHeader : X2chThreadHeader
	{
		/// <summary>
		/// dat�t�@�C���̑��݂���URL���擾
		/// </summary>
		public override string DatUrl {
			get {
				if (gzipCompress)
				{
					return Path.ChangeExtension(Url, ".dat.gz");
				}
				else {
					return Path.ChangeExtension(Url, ".dat");
				}
			}
		}

		/// <summary>
		/// GZip���k����Ă���X���b�h���擾����ꍇ�� true�A�����łȂ���� false�B
		/// </summary>
		public bool GzipCompress {
			set {
				if (gzipCompress != value)
					gzipCompress = value;
			}
			get {
				return gzipCompress;
			}
		}
		private bool gzipCompress = true;

		/// <summary>
		/// �X���b�h��URL���擾
		/// </summary>
		public override string Url {
			get {
				string subdir;
				int key0;
				
				Int32.TryParse(Key, out key0);
				int key1 = key0 / 1000000;
				int key2 = key0 / 100000;

				if (key1 < 1000)	subdir = key1.ToString();
				else				subdir = String.Format("{0}/{1}", key1, key2);

				return String.Format("http://{0}/{1}/kako/{2}/{3}.html",
					BoardInfo.Server, BoardInfo.Path, subdir, Key);
			}
		}

		/// <summary>
		/// X2chKakoThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chKakoThreadHeader()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// X2chKakoThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chKakoThreadHeader(X2chThreadHeader source) : base(source)
		{
			base.BoardInfo.Bbs = BbsType.X2chKako;
			base.ResCount = source.ResCount;
		}
	}
}
