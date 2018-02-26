
namespace Twin.Forms
{
	using System;
	using System.Net;
	using System.Xml;
	using System.Windows.Forms;
	using System.Text;
	using System.Threading;
	using System.Diagnostics;

	/// <summary>
	/// TwinUpdate の概要の説明です。
	/// </summary>
	public class TwinUpdate
	{
		public static void Check()
		{
			Thread thread = new Thread(new ThreadStart(CheckInternal));
			thread.Name = "UPDATE_CHECK";
			thread.IsBackground = true;
			thread.Start();
		}

		private static void CheckInternal()
		{
			try {
			// 更新情報のファイルを取得
			WebClient webClient = new WebClient();
			byte[] data = webClient.DownloadData(Settings.UpdateInfoUri);
			string text = Encoding.GetEncoding("Shift_Jis").GetString(data);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(text);

			XmlNode root = doc.DocumentElement;

			// バージョンが新しければ更新メッセージを出す
			Version newver = new Version(root.SelectSingleNode("Version").InnerText);

			if (Twinie.Version < newver)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("ついんてーるが更新されています\r\n");
				sb.AppendFormat("バージョン: {0}\r\n\r\n", newver);
				sb.Append(root.SelectSingleNode("Information").InnerText);
				sb.Append("\r\n新しいバージョンに更新しますか？");

				// 更新確認のダイアログを表示
				DialogResult r = MessageBox.Show(sb.ToString(), "新しいバージョンの確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Information);

				if (r == DialogResult.Yes)
					Process.Start(Settings.WebSiteUrl);
			}
			}catch{}
		}
	}
}
