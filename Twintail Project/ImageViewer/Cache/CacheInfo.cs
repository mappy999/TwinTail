// ImageCache.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Text;
	using System.Drawing;
	using System.Security.Cryptography;

	/// <summary>
	/// 画像のキャッシュ情報を表す
	/// </summary>
	public class CacheInfo
	{
		private string url;
		private string hashcode;
		private int length;
		private DateTime lastmodified;

		/// <summary>
		/// 画像のURLを取得
		/// </summary>
		public string Url {
			get {
				return url;
			}
		}

		/// <summary>
		/// データの長さを取得
		/// </summary>
		public int Length {
			get {
				return length;
			}
		}

		/// <summary>
		/// 画像データのハッシュ値を取得
		/// </summary>
		public string HashCode {
			get {
				return hashcode;
			}
		}

		/// <summary>
		/// 画像の最終更新日を取得
		/// </summary>
		public DateTime LastModified {
			get {
				return lastmodified;
			}
		}

		private string fileName;
		/// <summary>
		/// キャッシュされたローカルファイルへのファイルパスを取得します。
		/// </summary>
		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				fileName = value;
			}
		}

		/// <summary>
		/// このキャッシュ画像が保存済みであれば true、そうでなければ false を表します。
		/// </summary>
		public bool IsSaved { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <param name="length"></param>
		/// <param name="hash"></param>
		/// <param name="lastmod"></param>
		public CacheInfo(string url, int length, string hash, DateTime lastmod)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			this.url = url;
			this.length = length;
			this.hashcode = hash;
			this.lastmodified = lastmod;
		}
	}
}
