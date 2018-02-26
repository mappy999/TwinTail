// TwinUserProfile.cs

namespace Twin
{
	using System;
	using CSharpSamples;

	/// <summary>
	/// TwinUserProfile の概要の説明です。
	/// </summary>
	public class TwinUserProfile
	{
		/// <summary>
		/// 初回起動時の日付
		/// </summary>
		public DateTime Date = DateTime.Now;

		/// <summary>
		/// 初回起動時からの起動時間
		/// </summary>
		public int Tick = 0;

		/// <summary>
		/// 起動回数
		/// </summary>
		public int Startup = 0;

		/// <summary>
		/// スレッドを立てた回数
		/// </summary>
		public int NewThread = 0;

		/// <summary>
		/// 書き込んだレスの回数
		/// </summary>
		public int PostRes = 0;

		/// <summary>
		/// 取得したスレッド数
		/// </summary>
		public int GotThread = 0;

		/// <summary>
		/// 取得したレスの総数
		/// </summary>
		public int GotResCount = 0;

		/// <summary>
		/// TwinUserProfileクラスのインスタンスを初期化
		/// </summary>
		public TwinUserProfile()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}

		public void Load(string filePath)
		{
			throw new NotSupportedException("未実装");
		}

		public void Save(string filePath)
		{
			throw new NotSupportedException("未実装");
		}
	}
}
