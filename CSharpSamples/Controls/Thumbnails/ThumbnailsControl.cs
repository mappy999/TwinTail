// ThumbnailsControl.cs

namespace CSharpSamples
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Threading;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using System.Diagnostics;
	using CSharpSamples.Winapi;

	/// <summary>
	/// ���X�g�r���[�ɉ摜���k���\���ł���@�\��ǉ�����N���X
	/// </summary>
	public class ThumbnailsControl : Control
	{
		private ListView listView;
		private ImageList imageList;

		private Dictionary<string, int> imageIndices; // key �̓t�@�C�����Avalue �� �쐬�ς݃T���l�C���� ImageIndex�B
		private Queue<ListViewItem> queue; // �T���l�C�����쐬�� ListViewItem ���i�[����L���[�B
		private ImageFilter filter;

		private ManualResetEvent resetEvent = new ManualResetEvent(false);
		private Thread thread = null;

		private bool disposed = false;

		/// <summary>
		/// �T���l�C���̉摜�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public Size ImageSize
		{
			set
			{
				lock (imageList)
					imageList.ImageSize = value;
			}
			get
			{
				lock (imageList)
					return imageList.ImageSize;
			}
		}

		/// <summary>
		/// �摜�ɂ�����t�B���^�[���擾�܂��͐ݒ�
		/// </summary>
		public ImageFilter Filter
		{
			set
			{
				if (filter != value)
					filter = value;
			}
			get { return filter; }
		}

		/// <summary>
		/// ���ׂẴT���l�C���̌��t�@�C�������擾
		/// </summary>
		public string[] AllItems
		{
			get
			{
				string[] allItems = new string[imageIndices.Count];
				imageIndices.Keys.CopyTo(allItems, 0);

				return allItems;
			}
		}

		/// <summary>
		/// �I������Ă���T���l�C���̌��t�@�C���̃p�X���擾
		/// </summary>
		public string[] SelectedItems
		{
			get
			{
				List<string> list = new List<string>();

				foreach (ListViewItem item in listView.SelectedItems)
				{
					list.Add(item.Tag as string);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// �R���e�L�X�g���j���[���擾�܂��͐ݒ�
		/// </summary>
		public override ContextMenu ContextMenu
		{
			set
			{
				listView.ContextMenu = value;
			}
			get
			{
				return listView.ContextMenu;
			}
		}

		/// <summary>
		/// �T���l�C���摜�f�[�^���擾
		/// </summary>
		public ImageList Thumbnails
		{
			get
			{
				lock (imageList)
					return imageList;
			}
		}

		/// <summary>
		/// ThumbnailsControl�N���X�̃C���X�^���X��������
		/// </summary>
		public ThumbnailsControl()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			imageList = new ImageList();
			imageList.ColorDepth = ColorDepth.Depth32Bit;
			imageList.TransparentColor = Color.White;
			imageList.ImageSize = new Size(100, 100);

			listView = new ListView();
			listView.Dock = DockStyle.Fill;
			listView.View = View.LargeIcon;
			listView.LargeImageList = listView.SmallImageList = imageList;
			Controls.Add(listView);

			filter = ImageFilter.None;
			queue = new Queue<ListViewItem>();
			imageIndices = new Dictionary<string, int>();

			WinApi.SetWindowTheme(listView.Handle, "explorer", null);  
		}

		/// <summary>
		/// �g�p���Ă��郊�\�[�X�����
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing)
			{
				Abort();

				listView.Items.Clear();
				listView.LargeImageList = null;

				imageList.Dispose();
				imageList = null;

				disposed = true;
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// �w�肵���t�@�C���̃T���l�C�����쐬���\������
		/// </summary>
		/// <param name="fileName">�T���l�C���\������t�@�C����</param>
		public void Add(string fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");

			AddRange(new string[] { fileName });
		}

		/// <summary>
		/// �w�肵���t�@�C���̃T���l�C�����쐬���\������
		/// </summary>
		/// <param name="fileNames">�T���l�C���\������t�@�C�����̔z��</param>
		public void AddRange(string[] fileNames)
		{
			if (fileNames == null)
				throw new ArgumentNullException("fileNames");

			List<ListViewItem> list = new List<ListViewItem>();

			// ��[�A�C�e��������ǉ�
			foreach (string filename in fileNames)
			{
				ListViewItem item = new ListViewItem(Path.GetFileName(filename));
				item.ImageIndex = -1;
				item.Tag = filename;

				lock (queue)
				{
					queue.Enqueue(item);
				}

				list.Add(item);
			}

			lock (listView)
			{
				listView.Items.AddRange(list.ToArray());
			}

			ThreadRun();
		}

		/// <summary>
		/// �w�肵���t�@�C�����̃T���l�C�����폜
		/// </summary>
		/// <param name="filename"></param>
		public void Remove(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");

			for (int i = listView.Items.Count - 1; i >= 0; i--)
			{
				lock (listView)
				{
					if (filename.Equals((string)listView.Items[i].Tag))
						listView.Items.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// �T���l�C�������X���b�h���N��
		/// </summary>
		private void ThreadRun()
		{
			resetEvent.Set();
			if (thread == null)
			{
				thread = new Thread(new ThreadStart(__GenerateThread));
				thread.Name = "THUMB_CTRL";
				thread.IsBackground = true;
				thread.Priority = ThreadPriority.Lowest;
				thread.Start();
			}
		}

		/// <summary>
		/// �T���l�C���̐����𒆎~����
		/// </summary>
		private void Abort()
		{
			if (thread != null && thread.IsAlive)
				thread.Abort();
			thread = null;
		}

		/// <summary>
		/// �T���l�C�����N���A
		/// </summary>
		public void Clear()
		{
			resetEvent.Reset();
			listView.Items.Clear();
			lock (imageIndices) imageIndices.Clear();
			lock (imageList) imageList.Images.Clear();
		}

		private void __GenerateThread()
		{
			while (true)
			{
				resetEvent.WaitOne();

				if (queue.Count > 0)
				{
					ListViewItem item;

					lock (queue)
						item = (ListViewItem)queue.Dequeue();

					string filename = (string)item.Tag;

					int imageIndex;
					lock (imageIndices)
					{
						// ���ɃT���l�C�����쐬����Ă��Ȃ����ǂ������`�F�b�N
						if (imageIndices.ContainsKey(filename))
						{
							imageIndex = imageIndices[filename];
						}
						// �V�K�ɃT���l�C�����쐬���A�쐬���ꂽ�T���l�C���̃C���f�b�N�X���擾�B
						else
						{
							imageIndex = CreateThumbnail(filename);
						}
						// �쐬�ς݃T���l�C���̃C���f�b�N�X�ԍ���ۑ�
						imageIndices[filename] = imageIndex;
					}

					// ImageIndex ��ݒ�
					MethodInvoker m = delegate { item.ImageIndex = imageIndex; };
					Invoke(m);
				}
				else
				{
					resetEvent.Reset();
				}
			}
		}

		/// <summary>
		/// �w�肵���t�@�C���̃T���l�C���摜���쐬
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private int CreateThumbnail(string fileName)
		{
			int imageIndex = -1;

			byte[] bytes = LoadData(fileName);
			if (bytes == null)
			{
				lock (imageList)
				{
					MethodInvoker m = delegate
					{
						imageIndex = imageList.Images.Add(GetErrorImage(),
							imageList.TransparentColor);
					};
					Invoke(m);
				}
				return imageIndex;
			}

			MemoryStream mem = new MemoryStream(bytes);

			using (Image source = new Bitmap(mem))
			{
				float width = (float)ImageSize.Width / source.Width;
				float height = (float)ImageSize.Height / source.Height;
				float percent = Math.Min(width, height);

				width = (source.Width * percent);
				height = (source.Height * percent);

				Rectangle rect = new Rectangle(
					(int)((ImageSize.Width - width) / 2),
					(int)((ImageSize.Height - height) / 2),
					(int)width, (int)height);

				Image buffer = new Bitmap(ImageSize.Width, ImageSize.Height);

				using (Graphics g = Graphics.FromImage(buffer))
				{
					using (Image thumb = source.GetThumbnailImage((int)width, (int)height,
							delegate { return false; }, IntPtr.Zero))
					{
						_SetFilter(thumb as Bitmap);

						lock (imageList)
						{
							g.Clear(imageList.TransparentColor);
						}

						g.DrawImage(thumb, rect);
					}
				}

				lock (imageList)
				{
					MethodInvoker m = delegate
					{
						imageIndex = imageList.Images.Add(buffer,
							imageList.TransparentColor);
					};
					Invoke(m);
				}
			}

			return imageIndex;
		}

		private Image GetErrorImage()
		{
			Image image = new Bitmap(ImageSize.Width, ImageSize.Height);
			using (Graphics g = Graphics.FromImage(image))
			{
				g.DrawLine(Pens.Red, 0, 0, ImageSize.Width, ImageSize.Height);
				g.DrawLine(Pens.Red, ImageSize.Width, 0, 0, ImageSize.Height);
			}
			return image;
		}

		/// <summary>
		/// �w�肵���摜�Ƀt�B���^��������
		/// </summary>
		/// <param name="image"></param>
		private bool _SetFilter(Bitmap image)
		{
			if (image == null)
				throw new ArgumentNullException("image");

			switch (filter)
			{
				case ImageFilter.Alpha:
					return BitmapFilter.Brightness(image, 100);

				case ImageFilter.Mosaic:
					return BitmapFilter.Pixelate(image, 3, false);

				case ImageFilter.GrayScale:
					return BitmapFilter.GrayScale(image);

				default:
					return false;
			}
		}

		protected virtual byte[] LoadData(string uri)
		{
			return File.ReadAllBytes(uri);
		}
	}

	/// <summary>
	/// �摜�ɂ�����t�B���^�[��\���񋓑�
	/// </summary>
	public enum ImageFilter
	{
		/// <summary>�w��Ȃ�</summary>
		None,
		/// <summary>�A���t�@����</summary>
		Alpha,
		/// <summary>���U�C�N</summary>
		Mosaic,
		/// <summary>�O���[�X�P�[��</summary>
		GrayScale,
	}
}
