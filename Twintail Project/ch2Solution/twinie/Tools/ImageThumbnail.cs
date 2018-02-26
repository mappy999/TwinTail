using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace Twin.Tools
{
	class ImageThumbnail
	{
		public static bool Convert(string fileName, Size resultSize)
		{
			if (!File.Exists(fileName))
				return false;

			Image source = null;
			MemoryStream memory = new MemoryStream();

			try
			{
				try
				{
					using (FileStream fs = new FileStream(fileName, FileMode.Open))
					{
						byte[] buff = new byte[fs.Length];
						fs.Read(buff, 0, buff.Length);
						memory.Write(buff, 0, buff.Length);
						memory.Position = 0;
					}

					source = new Bitmap(memory);
				}
				catch
				{
					return false;
				}

				float width = (float)resultSize.Width / source.Width;
				float height = (float)resultSize.Height / source.Height;

				//float percent = Math.Min(width, height);
				float percent = height;

				width = (source.Width * percent);
				height = (source.Height * percent);

				using (Image thumb = new Bitmap(source, new Size((int)width, (int)height)))
				{
					thumb.Save(fileName, GetImageFormat(fileName));
				}
			}
			finally
			{
				if (source != null)
					source.Dispose();
			}

			return true;
		}

		private static ImageFormat GetImageFormat(string fileName)
		{
			switch (Path.GetExtension(fileName))
			{
			case ".jpg":
			case ".jpeg":
			case ".jpe":
				return ImageFormat.Jpeg;

			case ".gif":
				return ImageFormat.Gif;

			case ".png":
				return ImageFormat.Png;

			default:
				return ImageFormat.Bmp;
			}
		}
	}
}
