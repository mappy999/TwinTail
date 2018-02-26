using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Twin
{
	public partial class ColorWordEditDialog : Form
	{
		private ColorWordInfoSettings settings;

		private bool IsSelected
		{
			get { return listView1.SelectedItems.Count > 0; }
		}

		public ColorWordEditDialog(ColorWordInfoSettings sett)
		{
			InitializeComponent();
			settings = sett;

			foreach (ColorWordInfo newInfo in settings.WordInfo)
				SetListViewItem(null, newInfo);
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{

		}

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			Edit(true);
		}

		private void menuItemFileExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void menuItemEditNew_Click(object sender, EventArgs e)
		{
			Edit(false);
		}

		private void menuItemEditSelection_Click(object sender, EventArgs e)
		{
			Edit(true);
		}

		private void menuItemEditDel_Click(object sender, EventArgs e)
		{
			while (listView1.SelectedItems.Count > 0)
			{
				ListViewItem item = listView1.SelectedItems[0];
				ColorWordInfo info = (ColorWordInfo)item.Tag;

				settings.WordInfoList.Remove(info);
				item.Remove();
			}
		}

		private void menuItemEdit_DropDownOpening(object sender, EventArgs e)
		{
			menuItemEditDel.Enabled = menuItemEditSelection.Enabled = IsSelected;
		}

		private void Edit(bool editing)
		{
			ColorWordInfo info = null;

			if (editing && IsSelected)
			{
				info = (ColorWordInfo)listView1.SelectedItems[0].Tag;
			}
			else
			{
				info = new ColorWordInfo();
			}

			using (ColorWordRegistDialog dlg = new ColorWordRegistDialog(info))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					if (editing)
					{
						ColorWordInfo newInfo = dlg.ResultColorWordInfoArray[0];
						int idx = settings.WordInfoList.IndexOf(info);
						settings.WordInfoList[idx] = newInfo;
						SetListViewItem(listView1.SelectedItems[0], newInfo);
					}
					else
					{
						foreach (ColorWordInfo newInfo in dlg.ResultColorWordInfoArray)
						{
							settings.WordInfoList.Add(newInfo);
							SetListViewItem(null, newInfo);
						}
					}
				}
			}
		}

		private void SetListViewItem(ListViewItem item, ColorWordInfo info)
		{
			if (item == null)
			{
				item = new ListViewItem();
				listView1.Items.Add(item);
			}
			else
			{
				item.SubItems.Clear();
			}

			item.Text = info.Text;
			item.SubItems.Add(info.IsRegex ? "‚ ‚è" : String.Empty);
			item.SubItems.Add(ColorTranslator.ToHtml(info.ForeColor));
			item.SubItems.Add(ColorTranslator.ToHtml(info.BackColor));
			item.SubItems.Add(info.IsBold ? "‚ ‚è" : String.Empty);
			item.SubItems.Add(info.IsItalic ? "‚ ‚è" : String.Empty);
			item.SubItems.Add(info.IsPlaySound ? info.SoundFilePath : String.Empty);
			item.SubItems.Add(info.IsPopup ? info.PopupText : String.Empty);
			item.Tag = info;
		}
	}
}