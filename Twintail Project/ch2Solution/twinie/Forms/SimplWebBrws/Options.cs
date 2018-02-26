// Options.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// Option の概要の説明です。
	/// </summary>
	public class Options
	{
		/// <summary>
		/// タブを閉じた際の動作
		/// </summary>
		public Activation Activation = Activation.Right;
		/// <summary>
		/// タブを開いたときに新しいタブをアクティブにするかどうか
		/// </summary>
		public bool Activate = true;
		/// <summary>
		/// 検索に使用するURI
		/// </summary>
		public string SearchUri = "http://www.google.co.jp/search?q={0}&lr=lang_ja";

		public Options()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}

	/// <summary>
	/// アクティブの方法
	/// </summary>
	public enum Activation
	{
		/// <summary>
		/// 閉じたタブの左をアクティブにする
		/// </summary>
		Left,
		/// <summary>
		/// 閉じたタブの右をアクティブにする
		/// </summary>
		Right,
	}
}
