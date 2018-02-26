// DatThreadReader.cs

namespace Twin.Bbs
{
	using System;

	/// <summary>
	/// DatThreadReader の概要の説明です。
	/// </summary>
	public class DatThreadReader : X2chThreadReader
	{
		public DatThreadReader() : base(new DatThreadParser())
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}
