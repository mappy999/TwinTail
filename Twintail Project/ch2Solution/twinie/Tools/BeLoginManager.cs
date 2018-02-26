using System;
using System.Collections.Generic;
using System.Text;
using Twin.Forms;
using System.Net;
using Twin.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Twin.Tools
{
	public class BeLoginManager
	{
		const string LOGIN_URI = "http://be.2ch.net/test/login.php";
		const string LOGOUT_URI = "http://be.2ch.net/test/logout.html";		
		const string LOGIN_SUCCESS_MSG = "ログイン完了";
		const string LOGIN_ERROR_MSG = "パスワードかメールアドレスが正しくないようです";

		/// <summary>
		/// サーバーから返されたHTMLをプレーンテキストに変換した文字列を返します。
		/// </summary>
		public string ResponseText { get; private set; }

		/// <summary>
		/// ログインに成功したのであれば true、失敗なら false を表します。
		/// </summary>
		public bool IsSuccess { get; private set; }

		public Encoding Encoding { get; set; }

		public string DMDM { get; private set; }
		public string MDMD { get; private set; }

		public BeLoginManager()
		{
			this.Encoding = Encoding.GetEncoding("EUC-JP");
		}

		public bool Login(BeSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			if (String.IsNullOrEmpty(settings.Username) || String.IsNullOrEmpty(settings.Password))
				throw new ArgumentException("メールアドレスまたはパスワードが無効な文字列です");

			// WEBリクエスト作成
			byte[] sendData = this.Encoding.GetBytes(
				String.Format("m={0}&p={1}&submit=登録", settings.Username, settings.Password));

			WebClient w = new WebClient();
			w.Encoding = this.Encoding;
			w.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");

			// 送信
			byte[] resData = w.UploadData(LOGIN_URI, sendData);
			string resHtml = this.Encoding.GetString(resData);
			Debug.WriteLine(resHtml);

			this.ResponseText = HtmlTextUtility.RemoveTag(resHtml);
		
			// クッキー取得
			string setCookie = w.ResponseHeaders[HttpResponseHeader.SetCookie];
			Debug.WriteLine(setCookie);
			if (!String.IsNullOrEmpty(setCookie))
			{
				this.DMDM = Regex.Match(setCookie, "DMDM=(.+?);", RegexOptions.Multiline).Groups[1].Value;
				this.MDMD = Regex.Match(setCookie, "MDMD=(.+?);", RegexOptions.Multiline).Groups[1].Value;
			
				this.IsSuccess =  !String.IsNullOrEmpty(this.DMDM) &&
									!String.IsNullOrEmpty(this.MDMD) &&
									(this.ResponseText.IndexOf(LOGIN_SUCCESS_MSG) > 0);

				Debug.WriteLine("DMDM=" + this.DMDM);
				Debug.WriteLine("MDMD=" + this.MDMD);
			}

			return this.IsSuccess;
		}

		/// <summary></summary>
		public void Logout()
		{
		}

	}
}
