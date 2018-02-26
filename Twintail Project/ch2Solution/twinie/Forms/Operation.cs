// Operation.cs

namespace Twin.Forms
{
	using System;
	using System.ComponentModel;

	/// <summary>
	/// 開く際の共通操作を表す
	/// </summary>
	public enum OpenMode
	{
		[Description("シングルクリック")]
		SingleClick,
		[Description("ダブルクリック")]
		DoubleClick,
	}

	/// <summary>
	/// タブの操作を表す
	/// </summary>
	public enum TabOperation
	{
		[Description("なし")]
		None,
		[Description("更新")]
		Reload,
		[Description("閉じる")]
		Close,
	}

	/// <summary>
	/// スレッド一覧の操作
	/// </summary>
	public enum ListOperation
	{
		[Description("なし")]
		None,
		[Description("開く")]
		Open,
		[Description("新規タブで開く")]
		NewOpen,
		[Description("1をポップアップ")]
		Popup1,
		[Description("新着ポップアップ")]
		NewResPopup,
		[Description("ログを削除")]
		Delete,
	}
}
