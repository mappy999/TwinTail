// WroteThreadHeader.cs

namespace Twin
{
	using System;

	/// <summary>
	/// 書き込み履歴のヘッダ情報
	/// </summary>
	public class WroteThreadHeader
	{
		private BoardInfo board;
		private string key;
		private string subject;
		private int wroteCount;

		/// <summary>
		/// 板情報を取得または設定
		/// </summary>
		public BoardInfo BoardInfo {
			set {
				if (value == null) {
					throw new ArgumentNullException("BoardInfo");
				}
				board = value;
			}
			get { return board; }
		}

		/// <summary>
		/// スレッドの番号を取得または設定
		/// </summary>
		public string Key {
			set {
				if (value == null) {
					throw new ArgumentNullException("Key");
				}
				key = value;
			}
			get { return key; }
		}

		/// <summary>
		/// スレッド名を取得または設定
		/// </summary>
		public string Subject {
			set {
				if (value == null) {
					throw new ArgumentNullException("Subject");
				}
				subject = value;
			}
			get { return subject; }
		}

		/// <summary>
		/// 書き込み履歴数を取得または設定
		/// </summary>
		public int WroteCount {
			set { wroteCount = value; }
			get { return wroteCount; }
		}

		/// <summary>
		/// WroteThreadHeaderクラスのインスタンスを初期化
		/// </summary>
		public WroteThreadHeader()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			key = String.Empty;
			subject = String.Empty;
			board = null;
			wroteCount = 0;
		}

		/// <summary>
		/// WroteThreadHeaderクラスのインスタンスを初期化
		/// </summary>
		/// <param name="header"></param>
		public WroteThreadHeader(ThreadHeader header) : this()
		{
			key = header.Key;
			board = header.BoardInfo;
			subject = header.Subject;
		}
	}
}
