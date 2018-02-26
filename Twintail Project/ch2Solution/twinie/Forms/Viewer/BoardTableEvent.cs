// BoardTableEvent.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// BoardTableEventHandler デリゲート
	/// </summary>
	public delegate void BoardTableEventHandler(object sender, BoardTableEventArgs e);

	/// <summary>
	/// BoardTableEventArgs の概要の説明です。
	/// </summary>
	public class BoardTableEventArgs : EventArgs
	{
		private readonly BoardInfo board;
		private readonly bool isNewOpen;

		/// <summary>
		/// 選択された板を取得
		/// </summary>
		public BoardInfo Item {
			get {
				return board;
			}
		}

		/// <summary>
		/// 新しいウインドウで開くかどうかを取得
		/// </summary>
		public bool IsNewOpen {
			get {
				return isNewOpen;
			}
		}

		/// <summary>
		/// BoardTableEventArgsクラスのインスタンスを初期化
		/// </summary>
		public BoardTableEventArgs(BoardInfo info, bool newOpen)
		{
			if (info == null) {
				throw new ArgumentNullException("info");
			}
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			board = info;
			isNewOpen = newOpen;
		}
	}
}
