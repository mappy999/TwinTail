// ThreadSearchEvent.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ThreadSearchEventHandlerデリゲート
	/// </summary>
	public delegate void ThreadSearchEventHandler(object sender, ThreadSearchEventArgs e);

	/// <summary>
	/// ThreadSearchEventHandler メソッドのデータを提供
	/// </summary>
	public class ThreadSearchEventArgs : EventArgs
	{
		private List<ThreadHeader> items;
		private int start;
		private int end;

		/// <summary>
		/// 一致したスレッドの情報を取得
		/// </summary>
		public List<ThreadHeader> Items
		{
			get
			{
				return items;
			}
		}

		/// <summary>
		/// 検索開始位置を取得
		/// </summary>
		public int Start
		{
			get
			{
				return start;
			}
		}

		/// <summary>
		/// 検索終了一を取得
		/// </summary>
		public int End
		{
			get
			{
				return end;
			}
		}

		/// <summary>
		/// ThreadSearchEventArgsクラスのインスタンスを初期化
		/// </summary>
		/// <param name="items">一致したスレッドの情報</param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public ThreadSearchEventArgs(List<ThreadHeader> items, int start, int end)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.items = items;
			this.start = start;
			this.end = end;
		}
	}
}
