// IPopup.cs

namespace Twin
{
	using System;
	using System.Drawing;
	using Twin;

	/// <summary>
	/// ポップアップインターフェース
	/// </summary>
	public interface IPopupBase
	{
		/// <summary>
		/// 子ポップアップ数を取得
		/// </summary>
		int Count {
			get;
		}

		/// <summary>
		/// ポップアップする位置を取得または設定
		/// </summary>
		PopupPosition Position {
			set;
			get;
		}

		/// <summary>
		/// ポップアップの最大サイズを取得または設定
		/// </summary>
		Size Maximum {
			set;
			get;
		}

		/// <summary>
		/// ポップアップのフォントを取得または設定
		/// </summary>
		Font Font {
			set;
			get;
		}

		/// <summary>
		/// 文字色を取得または設定
		/// </summary>
		Color ForeColor {
			set;
			get;
		}

		/// <summary>
		/// 背景色を取得または設定
		/// </summary>
		Color BackColor {
			set;
			get;
		}

		/// <summary>
		/// ポップアップの幅を取得
		/// </summary>
		int Width {
			get;
		}

		/// <summary>
		/// ポップアップの高さを取得
		/// </summary>
		int Height {
			get;
		}

		/// <summary>
		/// 画像ポップアップの画像サイズを取得または設定
		/// </summary>
		Size ImageSize {
			set;
			get;
		}

		/// <summary>
		/// textをポップアップで表示
		/// </summary>
		/// <param name="text"></param>
		void Show(string text);

		/// <summary>
		/// resSetsをテキストに変換して表示
		/// </summary>
		/// <param name="resSets"></param>
		void Show(ResSetCollection resSets);

		/// <summary>
		/// resSetをテキストに変換して表示
		/// </summary>
		/// <param name="resSet"></param>
		void Show(ResSet resSet);

		/// <summary>
		/// 指定した画像のURLを表示
		/// </summary>
		/// <param name="uri"></param>
		void ShowImage(string uri);

		/// <summary>
		/// ポップアップを非表示にする
		/// </summary>
		void Hide();

		/// <summary>
		/// ポップアップ表示／非表示の状態
		/// </summary>
		bool Visible {
			get;
		}
	}

	/// <summary>
	/// ポップアップする位置を表す列挙体
	/// </summary>
	public enum PopupPosition
	{
		/// <summary>左上に表示</summary>
		TopLeft,
		/// <summary>右上に表示</summary>
		TopRight,
		/// <summary>左下に表示</summary>
		BottomLeft,
		/// <summary>右下に表示</summary>
		BottomRight,
	}
}
