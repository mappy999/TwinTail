// IExternalMethod.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// 外部呼び出し可能な関数を定義
	/// </summary>
	public interface IExternalMethod
	{
		/// <summary>
		/// 最大レス表示数を変更
		/// </summary>
		/// <param name="limitCount"></param>
		void SetLimit(int limitCount);

		/// <summary>
		/// startからendまでのレスを表示
		/// </summary>
		/// <param name="start">0から始まるレスの表示開始インデックス</param>
		/// <param name="end">0から始まるレスの表示終了インデックス</param>
		void Range(int start, int end);

		/// <summary>
		/// count数分、次のレスを表示
		/// </summary>
		/// <param name="count">表示するレス数</param>
		void Next(int count);

		/// <summary>
		/// count数分、前のレスを表示
		/// </summary>
		/// <param name="count">表示するレス数</param>
		void Prev(int count);

		/// <summary>
		/// 現在のスレッドを更新し新着レスを取得
		/// </summary>
		void Reload();

		/// <summary>
		/// 一番上までスクロール
		/// </summary>
		void ScrollTop();

		/// <summary>
		/// 一番下までスクロール
		/// </summary>
		void ScrollBottom();

		/// <summary>
		/// 現在開いているスレッドの中からキーワードを抽出
		/// </summary>
		/// <param name="obj">検索対象のオブジェクト (0=すべて、1=名前、2=Email、3=ID, 4=本文)</param>
		/// <param name="key">検索キーワード</param>
		void Extract(int obj, string key);

		/// <summary>
		/// 指定したレスを参照しているレスをポップアップ表示。
		/// </summary>
		/// <param name="index"></param>
		void BackReferences(int index);
	}
}
