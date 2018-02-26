using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Twin.Tools;

namespace Twin.Forms
{
	public partial class SubjectSearchOptionDialog : Form
	{
		public string SearchCaption
		{
			get { return textBox1.Text; }
			set { textBox1.Text = value ?? String.Empty; }
		}

		public string SearchString
		{
			get { return textBoxKeyword.Text; }
			set
			{
				textBoxKeyword.Text = value ?? String.Empty;
			}
		}

		public int SearchResultCount
		{
			get { return (int)numericUpDownCount.Value; }
			set
			{
				numericUpDownCount.Value = Math.Max(1, Math.Min(value, 50));
			}
		}

		public SubjectSearchOrder SearchOrder
		{
			get { return (SubjectSearchOrder)Enum.Parse(typeof(SubjectSearchOrder), comboBoxSortOrder.SelectedItem.ToString()); }
			set { comboBoxSortOrder.SelectedItem = value.ToString(); }
		}

		public SubjectSearchSorting SearchSorting
		{
			get { return (SubjectSearchSorting)Enum.Parse(typeof(SubjectSearchSorting), comboBoxSorting.SelectedItem.ToString()); }
			set { comboBoxSorting.SelectedItem = value.ToString(); }
		}


		public SubjectSearchOptionDialog()
		{
			InitializeComponent();
			InitializeControls();
		}

		private void InitializeControls()
		{
			string[] sorting = Enum.GetNames(typeof(SubjectSearchSorting));
			comboBoxSorting.Items.AddRange(sorting);
			comboBoxSorting.SelectedText = SubjectSearchSorting.Modified.ToString();
		
			string[] order = Enum.GetNames(typeof(SubjectSearchOrder));
			comboBoxSortOrder.Items.AddRange(order);
			comboBoxSortOrder.SelectedText = SubjectSearchOrder.Ascending.ToString();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (textBox1.Text.Length == 0)
			{
				MessageBox.Show("表示名を入力してください");
				textBox1.Focus();
			}
			else if (textBoxKeyword.Text.Length == 0)
			{
				MessageBox.Show("キーワードを入力してください");
				textBoxKeyword.Focus();
			}
			else
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
	}
}
