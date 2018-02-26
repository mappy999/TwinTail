// ImageUtil.cs

namespace ImageViewerDll
{
	using System;
	using System.Drawing;
	using System.Drawing.Drawing2D;

	/// <summary>
	/// ImageUtil の概要の説明です。
	/// </summary>
	public class ImageUtil
	{
		/// <summary>
		/// 縦横比が固定された画像サイズを取得
		/// </summary>
		/// <param name="imageSrc"></param>
		/// <param name="imageSize"></param>
		/// <returns></returns>
		public static Size GetThumbnailSize(Image imageSrc, Size imageSize)
		{
			float width = (float)imageSize.Width / imageSrc.Width;
			float height = (float)imageSize.Height / imageSrc.Height;
			float percent = Math.Min(width, height);

			if (percent > 1f)
				percent = 1f;

			Size newSize = new Size((int)(imageSrc.Width * percent),
				(int)(imageSrc.Height * percent));

			return newSize;
		}

		/// <summary>
		/// 縦横比が固定されたサムネイル画像を作成
		/// </summary>
		/// <param name="imageSrc"></param>
		/// <param name="imageSize"></param>
		/// <param name="transparent"></param>
		/// <returns></returns>
		public static Image GetThumbnailImage(Image imageSrc, Size imageSize, Color transparent)
		{
			Size newSize = GetThumbnailSize(imageSrc, imageSize);

			Rectangle rect = new Rectangle(
				(imageSize.Width - newSize.Width) / 2,
				(imageSize.Height - newSize.Height) / 2,
				newSize.Width, newSize.Height);

			Image buffer = new Bitmap(imageSize.Width, imageSize.Height);

			using (Graphics g = Graphics.FromImage(buffer))
			{
				using (Image thumb = new Bitmap(imageSrc, newSize))
				{
					g.Clear(transparent);
					g.DrawImage(thumb, rect);
				}
			}

			return buffer;
		}

		/// <summary>
		/// ×印のエラー用画像を取得
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static Image GetErrorImage()
		{
			Image image = new Bitmap(100, 100);

			using (Graphics g = Graphics.FromImage(image))
			{
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.DrawLine(Pens.Red, 0, 0, image.Width, image.Height);
				g.DrawLine(Pens.Red, 0, image.Height, image.Width, 0);
			}

			return image;
		}

		public static Image GetExceptionImage(Exception ex)
		{
			Image image = new Bitmap(300, 300);

			using (Graphics g = Graphics.FromImage(image))
			{
				StringFormat format = new StringFormat();
				Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

				g.DrawString(ex.Message, System.Windows.Forms.Control.DefaultFont, 
					Brushes.Black, rect, format);
			}

			return image;
		}
	}
}
