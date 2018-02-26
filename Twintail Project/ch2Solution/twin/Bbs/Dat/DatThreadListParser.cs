// DatThreadListParser.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;

	/// <summary>
	/// DatThreadListParser の概要の説明です。
	/// </summary>
	public class DatThreadListParser : X2chThreadListParser
	{
		/// <summary>
		/// DatThreadListParserクラスのインスタンスを初期化
		/// </summary>
		public DatThreadListParser() : base(BbsType.Dat, Encoding.GetEncoding("Shift_Jis"))
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}
