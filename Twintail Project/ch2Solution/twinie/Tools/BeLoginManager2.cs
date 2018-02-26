// このクラスは水玉さんの作成した BeCookie-0.0.0.46 内のソースを元に作成しました。
// 水玉さん、ありがとうございました。

using System;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using Mizutama.Lib;
using Twin.Tools;
using Twin.Text;
using System.Web;

namespace Twin.Bbs
{
	public class BeLoginManager2
	{
		public string Text { get; private set; }
		public string Email { get; set; }
		public string PW { get; set; }
		public string DMDM { get; private set; }
		public string MDMD { get; private set; }
		public string UserAgnet { get; set; }
		public bool CompleteMessageBox { get; set; }

		public void Login(CookieContainer cookies)
		{
			Encoding encoding_eucjp = Encoding.GetEncoding( "euc-jp" );

			var wc = new NWebClient();
			wc.Encoding = encoding_eucjp;
			wc.UserAgent = UserAgnet;
			wc.Cookies = cookies;
			wc.Headers.Add( "Content-Type" , "application/x-www-form-urlencoded" );

			wc.UploadStringCompleted += ( s , e ) =>
			{
				StringBuilder sb = new StringBuilder();
				if ( e.Error != null )
				{
					if ( e.Error is WebException )
					{
						WebException ex = e.Error as WebException;
						HttpWebResponse error = (HttpWebResponse)ex.Response;
						if ( error != null )
						{
							// HTTPエラー
							sb.AppendFormat( "HTTPエラー：{0} {1}\n" , error.StatusCode , error.StatusDescription );
						}
						else
						{
							// たぶんサーバーとつながんなかったんじゃないかな
							sb.AppendFormat( "WebException：{0}\n" , ex.Message );
						}
					}
					else
					{
						// なんか謎の例外だ。バグか？やっべー
						sb.AppendFormat( "Exception：{0}\n" , e.Error.Message );
					}
					Text = sb.ToString();
					TwinDll.ShowOutput(Text);
				}
				else
				{
					sb.Append( "ResponseHeaders:\n" );
					sb.Append( wc.ResponseHeaders );

					foreach ( Cookie c in NWebClient.GetAllCookies( cookies ) )
					{
						if ( c.Name == "DMDM" )
						{
							DMDM = c.Value;
						}
						if ( c.Name == "MDMD" )
						{
							MDMD = c.Value;
						}
					}

					sb.Append( e.Result );
					sb.Append( "\n\n" );
				}
				Text = sb.ToString();
				Debug.WriteLine(Text);

				if (CompleteMessageBox)
					System.Windows.Forms.MessageBox.Show(HtmlTextUtility.HtmlToText(Text));
			};

			wc.UploadStringAsync
			(
				new Uri( "http://be.2ch.net/test/login.php" ) ,
				string.Format( "m={0}&p={1}&submit=登録" , Email , PW )
			);
		}

		public void Logout(CookieContainer cookies)
		{
			var wc = new NWebClient();
			wc.Encoding = Encoding.GetEncoding( "euc-jp" );
			wc.UserAgent = UserAgnet;
			wc.Cookies = cookies;
			wc.DownloadStringCompleted += ( s , e ) =>
			{
				StringBuilder sb = new StringBuilder();
				if ( e.Error != null )
				{
					if ( e.Error is WebException )
					{
						WebException ex = e.Error as WebException;
						HttpWebResponse error = (HttpWebResponse)ex.Response;
						if ( error != null )
						{
							// HTTPエラー
							sb.AppendFormat( "HTTPエラー：{0} {1}\n" , error.StatusCode , error.StatusDescription );
						}
						else
						{
							// たぶんサーバーとつながんなかったんじゃないかな
							sb.AppendFormat( "WebException：{0}\n" , ex.Message );
						}
					}
					else
					{
						// なんか謎の例外だ。バグか？やっべー
						sb.AppendFormat( "Exception：{0}\n" , e.Error.Message );
					}

					Text = sb.ToString();
					TwinDll.ShowOutput(Text);
				}
				else
				{
					sb.Append( "ResponseHeaders:\n" );
					sb.Append( wc.ResponseHeaders );

					DMDM = "";
					MDMD = "";

					sb.Append( e.Result );
					sb.Append( "\n\n" );
				}
				Text = sb.ToString();
				Debug.WriteLine(Text);

				if (CompleteMessageBox)
					System.Windows.Forms.MessageBox.Show(HtmlTextUtility.HtmlToText(Text));
			};

			wc.DownloadStringAsync( new Uri( "http://be.2ch.net/test/logout.html" ) );
		}
	}
}
