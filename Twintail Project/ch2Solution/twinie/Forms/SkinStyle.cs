// SkinStyle.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using CSharpSamples;

	/// <summary>
	/// SkinStyle の概要の説明です。
	/// </summary>
	public class SkinStyle
	{
		/// <summary>
		/// 背景色を取得または設定
		/// </summary>
		public Color backColor;

		/// <summary>
		/// 文字色を取得または設定
		/// </summary>
		public Color foreColor;

		/// <summary>
		/// ポップアップの画像サイズを取得または設定
		/// </summary>
		public Size imageSize;

		/// <summary>
		/// フォントを取得または設定
		/// </summary>
		public Font font;

		/// <summary>
		/// ポップアップの表示スタイルを取得または設定
		/// </summary>
		public PopupStyle style;

		/// <summary>
		/// 画像ポップアップの有効状態を取得または設定
		/// </summary>
		public PopupState imagePopup;

		/// <summary>
		/// 画像ポップアップの有効状態を取得または設定
		/// </summary>
		public PopupState urlPopup;

		/// <summary>
		/// 画像サムネイル機能の有効状態を取得または設定
		/// </summary>
		public bool thumb;

		/// <summary>
		/// SkinStyleクラスのインスタンスを初期化
		/// </summary>
		public SkinStyle(Settings sett)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			backColor = Color.White;
			foreColor = Color.Black;
			imageSize = new Size(100, 100);
			font = new Font("ＭＳ Ｐゴシック", 9);
			style = sett.Popup.Style;
			imagePopup = sett.Popup.ImagePopup;
			urlPopup = sett.Popup.UrlPopup;
			thumb = sett.Thumbnail.Visible;
		}

		/// <summary>
		/// スキン情報を読み込む
		/// </summary>
		/// <param name="filePath"></param>
		public void Read(string filePath)
		{
			CSPrivateProfile p = new CSPrivateProfile();
			p.Read(filePath);

			// [Popup]セクション
			font = new Font(p.GetString("Popup", "FontFace", font.Name), p.GetFloat("Popup", "FontSize", font.Size));
			style = (PopupStyle)Enum.Parse(typeof(PopupStyle), p.GetString("Popup", "Style", style.ToString()));
			backColor = ColorTranslator.FromHtml(p.GetString("Popup", "BackColor", ColorTranslator.ToHtml(backColor)));
			foreColor = ColorTranslator.FromHtml(p.GetString("Popup", "ForeColor", ColorTranslator.ToHtml(foreColor)));
			// [Option]セクション
			imagePopup = (PopupState)Enum.Parse(typeof(PopupState), p.GetString("Option", "ImagePopup", imagePopup.ToString()));
			urlPopup = (PopupState)Enum.Parse(typeof(PopupState), p.GetString("Option", "UrlPopup", urlPopup.ToString()));
			thumb = p.GetBool("Option", "Thumb", thumb);
		}

		/// <summary>
		/// スキン情報を保存
		/// </summary>
		/// <param name="filePath"></param>
		public void Write(string filePath)
		{
			CSPrivateProfile p = new CSPrivateProfile();
			p.SetValue("Popup", "Style", style);
			p.SetValue("Popup", "BackColor", ColorTranslator.ToHtml(backColor));
			p.SetValue("Popup", "ForeColor", ColorTranslator.ToHtml(foreColor));
			p.SetValue("Popup", "FontFace", font.Name);
			p.SetValue("Popup", "FontSize", font.Size);
			p.SetValue("Option", "ImagePopup", imagePopup);
			p.SetValue("Option", "UrlPopup", urlPopup);
			p.SetValue("Option", "Thumb", thumb);
			p.Write(filePath);
		}
	}
}
