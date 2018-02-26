using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.ComponentModel;
using Twin.Forms;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Twin.Tools
{
	public class SettingTxtManager
	{
		private DownloadDataCompletedEventHandler callback;
		private WebClient webClient = null;

		public SettingTxtManager()
		{
		}

		private string GetCacheFileName(BoardInfo bi)
		{
			return Path.Combine(Settings.SettingTxtFolderPath, bi.Name + ".txt");
		}

		/// <summary>
		/// 指定した板の SETTING.TXT を取得し、ファイル名を返します。
		/// </summary>
		/// <param name="bi"></param>
		/// <returns></returns>
		public void BeginDownload(BoardInfo bi, DownloadDataCompletedEventHandler completed)
		{
			if (webClient != null)
				throw new InvalidOperationException();

			string uri = bi.Url + "SETTING.TXT";
			FileInfo fi = new FileInfo(GetCacheFileName(bi));

			callback = completed;

			webClient = new WebClient();

			if (fi.Exists)
			{
				webClient.Headers.Add(HttpRequestHeader.LastModified, fi.LastWriteTime.ToString("R"));
			}
			webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(w_DownloadFileCompleted);
			webClient.DownloadDataAsync(new Uri(uri), fi);
		}

		public string LoadCacheData(BoardInfo bi)
		{
			string fileName = GetCacheFileName(bi);

			if (!File.Exists(fileName))
				return String.Empty;

			using (StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("shift_jis")))
			{
				return sr.ReadToEnd();
			}
		}

		public void CancelAsync()
		{
			if (webClient == null)
				return;

			webClient.CancelAsync();
		}

		private void w_DownloadFileCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			if (e.Cancelled)
				return;

			// ファイルに書き込む
			FileInfo fi = (FileInfo)e.UserState;

			// エンコードを変換して保存
			using (StreamWriter w = new StreamWriter(fi.FullName, false, Encoding.GetEncoding("shift_jis")))
			{
				string text = Encoding.GetEncoding("shift_jis").GetString(e.Result);
				w.Write(Regex.Replace(text, "\n", "\r\n"));
			}

			fi.LastWriteTime = DateTime.ParseExact(webClient.ResponseHeaders[HttpResponseHeader.LastModified],
				"R", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);

			try
			{

				if (callback != null)
					callback(sender, e);
			}
			finally
			{
				webClient.Dispose();
				webClient = null;
				callback = null;
			}
		}
	}
}
