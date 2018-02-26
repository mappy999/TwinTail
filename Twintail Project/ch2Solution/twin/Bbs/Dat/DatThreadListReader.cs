// DatThreadListReader.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// DatThreadListReader の概要の説明です。
	/// </summary>
	public class DatThreadListReader : X2chThreadListReader
	{
		/// <summary>
		/// DatThreadListReaderクラスのインスタンスを初期化
		/// </summary>
		public DatThreadListReader() : base(new DatThreadListParser())
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}
