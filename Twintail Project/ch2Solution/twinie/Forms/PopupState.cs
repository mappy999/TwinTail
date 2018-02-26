// PopupState.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ポップアップ機能のフラグ列挙体
	/// </summary>
	[Flags]
	public enum PopupState
	{
		/// <summary>
		/// ポップアップ機能は無効
		/// </summary>
		Disable = 1,
		/// <summary>
		/// ポップアップ機能は有効
		/// </summary>
		Enable = 2,
		/// <summary>
		/// キーが押されている間のみ有効
		/// </summary>
		KeySwitch = 4,
	}
}
