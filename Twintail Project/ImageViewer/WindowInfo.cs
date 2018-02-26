// WindowInfo.cs
// #2.0

namespace ImageViewerDll
{
	using System;

	/// <summary>
	/// WindowInfo の概要の説明です。
	/// </summary>
	public class WindowInfo
	{
		private ImagePanel imagePanel;
		private string url;

		private CacheInfo cacheInfo = null;
		private string statusText = String.Empty;
		private bool error = false;
		private bool isSelected = false;

		/// <summary>
		/// ImagePanel コントロールのインスタンスを取得します。
		/// </summary>
		public ImagePanel ImagePanel
		{
			get
			{
				return imagePanel;
			}
		}

		/// <summary>
		/// 画像のキャッシュ情報を取得または設定します。
		/// </summary>
		public CacheInfo CacheInfo
		{
			set
			{
				cacheInfo = value;
			}
			get
			{
				return cacheInfo;
			}
		}

		/// <summary>
		/// 画像リソースへの URL を取得します。
		/// </summary>
		public string Url
		{
			get
			{
				return url;
			}
		}

		/// <summary>
		/// ステータスバーに表示するテキストを取得または設定します。
		/// </summary>
		public string StatusText
		{
			set
			{
				statusText = value;
			}
			get
			{
				return statusText;
			}
		}

		/// <summary>
		/// ウインドウが選択状態であれば true、それ以外は false を返します。
		/// </summary>
		public bool Selected
		{
			set
			{
				if (isSelected != value)
				{
					isSelected = value;

					if (SelectedChanged != null)
						SelectedChanged(this, new EventArgs());
				}
			}
			get
			{
				return isSelected;
			}
		}

		/// <summary>
		/// 画像の読み込みに失敗したかどうかを取得または設定します。
		/// </summary>
		public bool Error
		{
			set
			{
				error = value;
			}
			get
			{
				return error;
			}
		}

		/// <summary>
		/// 現在の ImagePanel に画像が読み込まれているかどうかを示す値を取得します。
		/// </summary>
		public bool Loaded
		{
			get
			{
				return imagePanel.IsLoaded;
			}
		}

		/// <summary>
		/// Selected プロパティが変更されたら発生します。
		/// </summary>
		public event EventHandler SelectedChanged;

		public WindowInfo(ImagePanel panel, string url)
		{
			if (panel == null)
				throw new ArgumentNullException("panel");

			if (url == null)
				throw new ArgumentNullException("url");

			this.imagePanel = panel;
			this.url = url;
		}
	}
}
