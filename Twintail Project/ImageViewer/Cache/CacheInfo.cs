// ImageCache.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Text;
	using System.Drawing;
	using System.Security.Cryptography;

	/// <summary>
	/// �摜�̃L���b�V������\��
	/// </summary>
	public class CacheInfo
	{
		private string url;
		private string hashcode;
		private int length;
		private DateTime lastmodified;

		/// <summary>
		/// �摜��URL���擾
		/// </summary>
		public string Url {
			get {
				return url;
			}
		}

		/// <summary>
		/// �f�[�^�̒������擾
		/// </summary>
		public int Length {
			get {
				return length;
			}
		}

		/// <summary>
		/// �摜�f�[�^�̃n�b�V���l���擾
		/// </summary>
		public string HashCode {
			get {
				return hashcode;
			}
		}

		/// <summary>
		/// �摜�̍ŏI�X�V�����擾
		/// </summary>
		public DateTime LastModified {
			get {
				return lastmodified;
			}
		}

		private string fileName;
		/// <summary>
		/// �L���b�V�����ꂽ���[�J���t�@�C���ւ̃t�@�C���p�X���擾���܂��B
		/// </summary>
		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				fileName = value;
			}
		}

		/// <summary>
		/// ���̃L���b�V���摜���ۑ��ς݂ł���� true�A�����łȂ���� false ��\���܂��B
		/// </summary>
		public bool IsSaved { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <param name="length"></param>
		/// <param name="hash"></param>
		/// <param name="lastmod"></param>
		public CacheInfo(string url, int length, string hash, DateTime lastmod)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			this.url = url;
			this.length = length;
			this.hashcode = hash;
			this.lastmodified = lastmod;
		}
	}
}
