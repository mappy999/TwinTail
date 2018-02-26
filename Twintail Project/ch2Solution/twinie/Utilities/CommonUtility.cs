// CommonUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Diagnostics;
	using System.Windows.Forms;
	using System.Text.RegularExpressions;

	/// <summary>
	/// ���낢��ȃ��[�e�B���e�B�Q
	/// </summary>
	public class CommonUtility
	{

		/// <summary>
		/// Web�u���E�U�Ŏw�肵��URL���J��
		/// </summary>
		/// <param name="url"></param>
		public static void OpenWebBrowser(string url)
		{
			if (url != null)
			{
				try
				{
					if (Twinie.Settings.WebBrowserPath.Equals("SimpleWebBrowser"))
					{
						Twinie.SimpleWeb.OpenUri(url, true);
					}
					else if (Twinie.Settings.WebBrowserPath != String.Empty)
					{
						Process.Start(Twinie.Settings.WebBrowserPath, url);
					}
					else
					{
						Process.Start(url);
					}
				}
				catch (ArgumentException ex)
				{
					TwinDll.Output(ex.ToString() + "\r\n�p�����[�^: " + url); 
					if (ex.Message.Contains("�w�肵���t�@�C����������܂���"))
					{
						MessageBox.Show("�u���E�U���N���ł��܂���ł����B\r\n�W���̃u���E�U���ݒ肳��Ă��Ȃ��\��������܂��B");
					}
				}
			}
		}

		/// <summary>
		/// �c�[���o�[�̕����v�Z
		/// </summary>
		/// <param name="toolbar">�����v�Z����c�[���o�[</param>
		/// <returns></returns>
		public static int CalcToolBarWidth(ToolBar toolbar)
		{
			if (toolbar == null)
			{
				throw new ArgumentNullException("toolbar");
			}

			int width = 0;

			foreach (ToolBarButton tb in toolbar.Buttons)
				width += tb.Rectangle.Width;

			return width;
		}
	}
}
