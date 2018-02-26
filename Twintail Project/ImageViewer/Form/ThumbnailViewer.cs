using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageViewerDll
{
	public partial class ThumbnailViewer : Form
	{
		public ThumbnailViewer()
		{
			InitializeComponent();

			webThumbnailsControl1.ImageSize = new Size(50, 50);
			webThumbnailsControl1.CacheDataFolderPath =
				Path.Combine(Application.StartupPath, "Images");
		}


		public void AddImageFromUrlRange(string[] urlArran)
		{
			InternalAddRangeImage(urlArran, false);
		}

		public void SetRangeImageFromUrl(string[] urlArray)
		{
			InternalAddRangeImage(urlArray, true);
		}

		private void InternalAddRangeImage(string[] pathArray, bool reset)
		{
			if (reset) webThumbnailsControl1.Clear();
			webThumbnailsControl1.AddRange(pathArray);
		}

		public void ClearThumbnails()
		{
			webThumbnailsControl1.Clear();
		}
	}
}
