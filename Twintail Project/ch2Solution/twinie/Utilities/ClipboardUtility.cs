// ClipboardUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using Twin;

	/// <summary>
	/// クリップボードに関係したユーティリティ群
	/// </summary>
	public class ClipboardUtility
	{
		/// <summary>
		/// ThreadHeaderの指定した要素をクリップボードにコピー
		/// </summary>
		/// <param name="item"></param>
		/// <param name="copy"></param>
		public static void Copy(ThreadHeader item, CopyInfo copy)
		{
			List<ThreadHeader> temp = new List<ThreadHeader>();
			temp.Add(item);

			Copy(temp, copy);
		}

		public static void Copy(List<ThreadHeader> items, CopyInfo copy)
		{
			Copy(new ReadOnlyCollection<ThreadHeader>(items), copy);
		}

		/// <summary>
		/// ThreadHeaderの指定した要素をクリップボードにコピー
		/// </summary>
		/// <param name="items"></param>
		/// <param name="copy"></param>
		public static void Copy(ReadOnlyCollection<ThreadHeader> items, CopyInfo copy)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			string text = String.Empty;
			bool added = false;

			foreach (ThreadHeader item in items)
			{
				if ((copy & CopyInfo.Name) > 0)
				{
					text += (added) ? ("\r\n" + item.Subject) : item.Subject;
					added = true;
				}
				if ((copy & CopyInfo.Url) > 0)
				{
					text += (added) ? ("\r\n" + item.Url) : item.Url;
					added = true;
				}
			}

			try
			{
				Clipboard.SetDataObject(text, true);
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
		}

		/// <summary>
		/// BoardInfoの指定した要素をクリップボードにコピー
		/// </summary>
		/// <param name="item"></param>
		/// <param name="copy"></param>
		public static void Copy(BoardInfo item, CopyInfo copy)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			string text = String.Empty;
			bool added = false;

			if ((copy & CopyInfo.Name) > 0)
			{
				text += item.Name;
				added = true;
			}
			if ((copy & CopyInfo.Url) > 0)
			{
				text += (added) ? ("\r\n" + item.Url) : item.Url;
				added = true;
			}

			try
			{
				Clipboard.SetDataObject(text, true);
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
		}
	}

	/// <summary>
	/// コピーする情報を表す
	/// </summary>
	[Flags]
	public enum CopyInfo
	{
		/// <summary>
		/// 名前をコピー
		/// </summary>
		Name = 1,
		/// <summary>
		/// URLをコピー
		/// </summary>
		Url = 2,
	}
}
