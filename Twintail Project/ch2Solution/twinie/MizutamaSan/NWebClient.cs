/****************************************************************************
	汎用ライブラリ
	使いにくいHttpWebRequestなんかよりWebClientで楽しよう

	http://neue.cc/2009/12/17_230.html
	http://stackoverflow.com/questions/153451/c-how-to-check-if-system-net-webclient-downloaddata-is-downloading-a-binary-f
	WebClientはWebRequestのラッパーにすぎないので
	overrideすればWebRequest並みの操作ができ、かついろいろ便利
	特に非同期版では元スレッドに返ってくるのでいろいろありがたいし。
	C#5.0+.NET4.5のasync/awaitは目玉だかんね、ああ早くDownloadDataTaskAsync()とか書きたい

	Copyright (C) 2012 Mizutama(水玉 ◆qHK1vdR8FRIm)
	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
****************************************************************************/
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Mizutama.Lib
{
	public class NWebClient : WebClient
	{
		// リクエスト時にカスタマイズしたいプロパティ
		public string UserAgent { get; set; }
		public int Timeout { get; set; }

		public CookieContainer Cookies { get; set; }
		public DateTime IfModifiedSinse { get; set; }
		public int Range { get; set; }

		// レスポンスプロパティが見たいじゃないですか
		public WebResponse Response { get; private set; }

		public NWebClient() : base()
		{
			Timeout = 100000; // WebRequestの規定値と同じ値
			IfModifiedSinse = DateTime.MinValue;
		}

		// GetWebRequestのところの動作をちょっと横取りして書き換える
		protected override WebRequest GetWebRequest( Uri address )
		{
			var request = base.GetWebRequest( address );
			if ( request is HttpWebRequest )
			{
				HttpWebRequest req = request as HttpWebRequest;
				req.UserAgent       = UserAgent;
				req.Timeout         = Timeout;
				req.CookieContainer = Cookies;
				if ( IfModifiedSinse != DateTime.MinValue )
				{
					req.IfModifiedSince = IfModifiedSinse;
				}
				if ( Range != 0 )
				{
					req.AddRange( Range );
				}
			}
			return request;
		}

		// GetWebResponseのところの動作をちょっと横取りして書き換える
		protected override WebResponse GetWebResponse( WebRequest req ) 
		{
			try
			{
				Response = base.GetWebResponse( req );
				return Response;
			}
			catch
			{
				// 404とかでResponseが取れないことも当然ある
				throw;
			}
		}

		// Async版を忘れずに（忘れてエラーになりまくりだったorz）
		protected override WebResponse GetWebResponse( WebRequest req , IAsyncResult ar )
		{
			try
			{
				Response = base.GetWebResponse( req , ar );
				return Response;
			}
			catch
			{
				// 404とかでResponseが取れないことも当然ある
				throw;
			}
		}

		#region ユーティリティ

		// .NET 4 Client ProfileではSystem.Webが使えないしそのためだけにFULLインストールするなんてね
		// http://d.hatena.ne.jp/kazuv3/20080605/1212656674
		public static string UrlEncode( string s , Encoding enc )
		{
			StringBuilder rt = new StringBuilder();
			foreach ( byte i in enc.GetBytes( s ) )
			{
				if ( i == 0x20 )
				{
					rt.Append( '+' );
				}
				else if ( (0x30 <= i && i <= 0x39) || (0x41 <= i && i <= 0x5a) || (0x61 <= i && i <= 0x7a) )
				{
					rt.Append( (char)i );
				}
				else
				{
					rt.Append( "%" + i.ToString( "X2" ) );
				}
			}
			return rt.ToString();
		}

		public static string UrlDecode( string s , Encoding enc )
		{
			List<byte> bytes = new List<byte>();
			for ( int i = 0; i < s.Length; i++ )
			{
				char c = s[i];
				if ( c == '%' )
				{
					bytes.Add( (byte)int.Parse( s[++i].ToString() + s[++i].ToString() , System.Globalization.NumberStyles.HexNumber ) );
				}
				else if ( c == '+' )
				{
					bytes.Add( (byte)0x20 );
				}
				else
				{
					bytes.Add( (byte)c );
				}
			}
			return enc.GetString( bytes.ToArray() , 0 , bytes.Count );
		}

		public static int GetUnixTime( DateTime baseTime )
		{
			TimeSpan t = baseTime.ToUniversalTime().Subtract( new DateTime( 1970 , 1 , 1 ) );
			return (int)(t.TotalSeconds);
		}

		// http://dot-net-expertise.blogspot.com/2009/10/cookiecontainer-domain-handling-bug-fix.html
		public static List<Cookie> GetAllCookies( CookieContainer cc )
		{
			List<Cookie> lstCookies = new List<Cookie>();

			System.Collections.Hashtable table
			 = (System.Collections.Hashtable)cc.GetType().InvokeMember
			 	(
			 		"m_domainTable" , 
			 		System.Reflection.BindingFlags.NonPublic
			 		 | System.Reflection.BindingFlags.GetField
			 		 | System.Reflection.BindingFlags.Instance , 
			 		null ,
			 		cc ,
			 		new object[] { }
			 	);

			foreach ( object pathList in table.Values )
			{
				System.Collections.SortedList lstCookieCol
				 = (System.Collections.SortedList)pathList.GetType().InvokeMember
				 	(
				 		"m_list" ,
				 		System.Reflection.BindingFlags.NonPublic
				 		 | System.Reflection.BindingFlags.GetField
				 		 | System.Reflection.BindingFlags.Instance ,
				 		null ,
				 		pathList ,
				 		new object[] { }
				 	);
				foreach ( CookieCollection colCookies in lstCookieCol.Values )
				{
					foreach ( Cookie c in colCookies )
					{
						lstCookies.Add( c );
					}
				}
			}

			return lstCookies;
		}

		#endregion ユーティリティ
	}
}
