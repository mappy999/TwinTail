// WindowInfo.cs
// #2.0

namespace ImageViewerDll
{
	using System;

	/// <summary>
	/// WindowInfo �̊T�v�̐����ł��B
	/// </summary>
	public class WindowInfo
	{
		private ImagePanel imagePanel;
		private string url;

		private CacheInfo cacheInfo = null;
		private string statusText = String.Empty;
		private bool error = false;
		private bool isSelected = false;

		/// <summary>
		/// ImagePanel �R���g���[���̃C���X�^���X���擾���܂��B
		/// </summary>
		public ImagePanel ImagePanel
		{
			get
			{
				return imagePanel;
			}
		}

		/// <summary>
		/// �摜�̃L���b�V�������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public CacheInfo CacheInfo
		{
			set
			{
				cacheInfo = value;
			}
			get
			{
				return cacheInfo;
			}
		}

		/// <summary>
		/// �摜���\�[�X�ւ� URL ���擾���܂��B
		/// </summary>
		public string Url
		{
			get
			{
				return url;
			}
		}

		/// <summary>
		/// �X�e�[�^�X�o�[�ɕ\������e�L�X�g���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string StatusText
		{
			set
			{
				statusText = value;
			}
			get
			{
				return statusText;
			}
		}

		/// <summary>
		/// �E�C���h�E���I����Ԃł���� true�A����ȊO�� false ��Ԃ��܂��B
		/// </summary>
		public bool Selected
		{
			set
			{
				if (isSelected != value)
				{
					isSelected = value;

					if (SelectedChanged != null)
						SelectedChanged(this, new EventArgs());
				}
			}
			get
			{
				return isSelected;
			}
		}

		/// <summary>
		/// �摜�̓ǂݍ��݂Ɏ��s�������ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool Error
		{
			set
			{
				error = value;
			}
			get
			{
				return error;
			}
		}

		/// <summary>
		/// ���݂� ImagePanel �ɉ摜���ǂݍ��܂�Ă��邩�ǂ����������l���擾���܂��B
		/// </summary>
		public bool Loaded
		{
			get
			{
				return imagePanel.IsLoaded;
			}
		}

		/// <summary>
		/// Selected �v���p�e�B���ύX���ꂽ�甭�����܂��B
		/// </summary>
		public event EventHandler SelectedChanged;

		public WindowInfo(ImagePanel panel, string url)
		{
			if (panel == null)
				throw new ArgumentNullException("panel");

			if (url == null)
				throw new ArgumentNullException("url");

			this.imagePanel = panel;
			this.url = url;
		}
	}
}
