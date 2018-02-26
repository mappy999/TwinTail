// CommonUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Diagnostics;
	using System.Windows.Forms;
	using System.Text.RegularExpressions;

	/// <summary>
	/// いろいろなユーティリティ群
	/// </summary>
	public class CommonUtility
	{

		/// <summary>
		/// Webブラウザで指定したURLを開く
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
					TwinDll.Output(ex.ToString() + "\r\nパラメータ: " + url); 
					if (ex.Message.Contains("指定したファイルが見つかりません"))
					{
						MessageBox.Show("ブラウザを起動できませんでした。\r\n標準のブラウザが設定されていない可能性があります。");
					}
				}
			}
		}

		/// <summary>
		/// ツールバーの幅を計算
		/// </summary>
		/// <param name="toolbar">幅を計算するツールバー</param>
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
