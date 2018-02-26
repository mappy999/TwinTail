// DatThreadParser.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using Twin.Text;

	/// <summary>
	/// 2chはdatにレス参照処理がされているが2ch互換のサイトはdatが処理されていないため、
	/// それを処理するクラス
	/// </summary>
	public class DatThreadParser : X2chThreadParser
	{
		/// <summary>
		/// DatThreadParserクラスのインスタンスを初期化
		/// </summary>
		public DatThreadParser() : base(BbsType.Dat, Encoding.GetEncoding("Shift_Jis"))
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}
