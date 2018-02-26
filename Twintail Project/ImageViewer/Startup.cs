// Startup.cs

using System;
using System.IO;
using System.Windows.Forms;

namespace ImageViewerDll
{
	/// <summary>
	/// Startup の概要の説明です。
	/// </summary>
	public class Startup
	{
		[STAThread]
		static void Main()
		{
			Application.Run(new ImageViewer());
		}
	}
}
