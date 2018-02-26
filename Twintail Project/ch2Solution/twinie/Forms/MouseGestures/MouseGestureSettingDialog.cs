using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Twin.Forms
{
	public partial class MouseGestureSettingDialog : Form
	{
		private MouseGestureActionSettings gas;

		private List<Arrow> arrowList = new List<Arrow>();

		public MouseGestureSettingDialog(MouseGestureActionSettings settings)
		{
			InitializeComponent();
			this.gas = settings;

			foreach (string name in Enum.GetNames(typeof(MouseGestureAction)))
				comboBoxAction.Items.Add(name);

			comboBoxAction.SelectedIndex = 0;

			foreach (MouseGestureActionItem item in gas.Items)
			{
				listBox1.Items.Add(item);
			}
			numericUpDown1.Value = settings.Range;
		}

		private void AddDirection(Arrow arrow)
		{
			arrowList.Add(arrow);
			textBoxDirection.Text += MouseGesture.ArrowToString(arrow);
		}

		private void Reset()
		{
			arrowList.Clear();
			textBoxDirection.Text = String.Empty;
		}

		private void buttonUp_Click(object sender, EventArgs e)
		{
			AddDirection(Arrow.Up);
		}

		private void buttonLeft_Click(object sender, EventArgs e)
		{
			AddDirection(Arrow.Left);
		}

		private void buttonDown_Click(object sender, EventArgs e)
		{
			AddDirection(Arrow.Down);
		}

		private void buttonRight_Click(object sender, EventArgs e)
		{
			AddDirection(Arrow.Right);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			gas.Range = (int)numericUpDown1.Value;
			Close();
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			MouseGestureAction action = (MouseGestureAction)
				Enum.Parse(typeof(MouseGestureAction), comboBoxAction.SelectedItem.ToString());

			if (action != MouseGestureAction.None)
			{
				MouseGestureActionItem item = new MouseGestureActionItem(arrowList.ToArray(), action);
				gas.list.Add(item);
				listBox1.Items.Add(item);
				Reset();
			}
		}

		private void buttonReset_Click(object sender, EventArgs e)
		{
			Reset();
		}

		private void buttonDel_Click(object sender, EventArgs e)
		{
			foreach (MouseGestureActionItem item in listBox1.SelectedItems)
			{
				gas.list.Remove(item);
			}
			while (listBox1.SelectedIndices.Count > 0)
				listBox1.Items.RemoveAt(listBox1.SelectedIndices[0]);
		}
	}
}