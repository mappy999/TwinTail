// ImageCacheEvent.cs

namespace ImageViewerDll
{
	using System;
	using System.Drawing;
	using System.Net;

	/// <summary>
	/// ImageCache�̃C�x���g���������郁�\�b�h
	/// </summary>
	public delegate void ImageCacheEventHandler(object sender, ImageCacheEventArgs e);

	/// <summary>
	/// ImageCache�̃C�x���g�̃f�[�^
	/// </summary>
	public class ImageCacheEventArgs : EventArgs
	{
		/// <summary>
		/// �L���b�V�������擾
		/// </summary>
		public CacheInfo CacheInfo { get; set; }
		
		/// <summary>
		/// �ǂݍ��܂ꂽ�摜�f�[�^���擾
		/// </summary>
		public Image Image { get; set; }

		public ImageCacheStatus Status { get; set; }

		/// <summary>
		/// �G���[�̌����ƂȂ�����O���擾
		/// </summary>
		public Exception Exception { get; set; }

		public HttpStatusCode StatusCode { get; set; }

		/// <summary>
		/// ImageCacheEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="info"></param>
		/// <param name="image"></param>
		public ImageCacheEventArgs(CacheInfo info)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.CacheInfo = info;
		}
	}

	public enum ImageCacheStatus
	{
		Unknown, Downloaded, CacheExist, Error,
	}
}
