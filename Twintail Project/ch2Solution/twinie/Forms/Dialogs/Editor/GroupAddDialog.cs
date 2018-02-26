using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Twin.Forms
{
	public partial class GroupAddDialog : Form
	{
		public string FileName
		{
			get
			{
				string f = comboBoxGroupName.Text;
				return StringUtility.ReplaceInvalidPathChars(f, "_");
			}
		}
	
		public List<ThreadHeader> CheckedItems
		{
			get
			{
				List<ThreadHeader> checkedItems = new List<ThreadHeader>();

				foreach (ThreadHeader h in checkedListBox1.CheckedItems)
				{
					checkedItems.Add(h);
				}

				return checkedItems;
			}
		}
	
		public GroupAddDialog(List<ThreadGroup> groupList, List<ThreadHeader> headerList)
		{
			InitializeComponent();

			comboBoxGroupName.Items.Add(DateTime.Now.ToString("新規グループ yyyyMMddHHss"));
			comboBoxGroupName.SelectedIndex = 0;

			foreach (ThreadGroup gp in groupList)
				comboBoxGroupName.Items.Add(gp.Name);

			foreach (ThreadHeader h in headerList)
			{
				checkedListBox1.Items.Add(h, CheckState.Checked);
			}
		}

		private void buttonCheckAll_Click(object sender, EventArgs e)
		{
			SetCheckedState(true);
		}

		private void buttonUncheckAll_Click(object sender, EventArgs e)
		{
			SetCheckedState(false);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void SetCheckedState(bool checkedState)
		{
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
				checkedListBox1.SetItemChecked(i, checkedState);
		}
	}
}