// Startup.cs

using System;
using System.IO;
using System.Windows.Forms;

namespace ImageViewerDll
{
	/// <summary>
	/// Startup �̊T�v�̐����ł��B
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
