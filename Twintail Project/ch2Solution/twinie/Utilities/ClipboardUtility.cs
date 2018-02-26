// ClipboardUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using Twin;

	/// <summary>
	/// �N���b�v�{�[�h�Ɋ֌W�������[�e�B���e�B�Q
	/// </summary>
	public class ClipboardUtility
	{
		/// <summary>
		/// ThreadHeader�̎w�肵���v�f���N���b�v�{�[�h�ɃR�s�[
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
		/// ThreadHeader�̎w�肵���v�f���N���b�v�{�[�h�ɃR�s�[
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
		/// BoardInfo�̎w�肵���v�f���N���b�v�{�[�h�ɃR�s�[
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
	/// �R�s�[�������\��
	/// </summary>
	[Flags]
	public enum CopyInfo
	{
		/// <summary>
		/// ���O���R�s�[
		/// </summary>
		Name = 1,
		/// <summary>
		/// URL���R�s�[
		/// </summary>
		Url = 2,
	}
}
