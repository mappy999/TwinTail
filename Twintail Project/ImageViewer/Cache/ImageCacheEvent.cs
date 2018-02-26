// ImageCacheEvent.cs

namespace ImageViewerDll
{
	using System;
	using System.Drawing;
	using System.Net;

	/// <summary>
	/// ImageCacheのイベントを処理するメソッド
	/// </summary>
	public delegate void ImageCacheEventHandler(object sender, ImageCacheEventArgs e);

	/// <summary>
	/// ImageCacheのイベントのデータ
	/// </summary>
	public class ImageCacheEventArgs : EventArgs
	{
		/// <summary>
		/// キャッシュ情報を取得
		/// </summary>
		public CacheInfo CacheInfo { get; set; }
		
		/// <summary>
		/// 読み込まれた画像データを取得
		/// </summary>
		public Image Image { get; set; }

		public ImageCacheStatus Status { get; set; }

		/// <summary>
		/// エラーの原因となった例外を取得
		/// </summary>
		public Exception Exception { get; set; }

		public HttpStatusCode StatusCode { get; set; }

		/// <summary>
		/// ImageCacheEventArgsクラスのインスタンスを初期化
		/// </summary>
		/// <param name="info"></param>
		/// <param name="image"></param>
		public ImageCacheEventArgs(CacheInfo info)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.CacheInfo = info;
		}
	}

	public enum ImageCacheStatus
	{
		Unknown, Downloaded, CacheExist, Error,
	}
}
