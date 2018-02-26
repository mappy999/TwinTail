using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Twin.Forms
{
	public partial class TabColorChangeDialog : Form
	{
		private string tabText;
		/// <summary>
		/// タブに表示する文字列を取得または設定します。
		/// </summary>
		public string TabText
		{
			get
			{
				return tabText;
			}
			set
			{
				tabText = value;
			}
		}

		private TabColorSet newColorSet;
		/// <summary>
		/// ユーザーが選択した新しい配色設定を取得します。
		/// </summary>
		public TabColorSet ColorSet
		{
			get
			{
				return newColorSet;
			}
		}

		public TabColorChangeDialog(TabColorSet colorSet)
		{
			InitializeComponent();

			this.newColorSet = new TabColorSet(colorSet);
			this.colorDialog1.CustomColors = Twinie.Settings.Dialogs.ColorDialogCustomColors;

			OnColorChanged();
		}

		private void OnColorChanged()
		{
			tabControlSample.Refresh();
		}

		private Color ChangeColor(Color color)
		{
			colorDialog1.Color = color;

			if (colorDialog1.ShowDialog(this) == DialogResult.OK)
			{
				color = colorDialog1.Color;
			}

			return color;
		}

		private void tabControlSample_DrawItem(object sender, DrawItemEventArgs e)
		{
			TabPage tab = tabControlSample.TabPages[e.Index];

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags = StringFormatFlags.NoWrap;

			Brush fore = new SolidBrush(e.Index == 0 ?
				newColorSet.ActiveForeColor : newColorSet.DeactiveForeColor);

			Brush back = new SolidBrush(e.Index == 0 ?
				newColorSet.ActiveBackColor : newColorSet.DeactiveBackColor);
			
			e.Graphics.FillRectangle(back, e.Bounds);
			e.Graphics.DrawString(tab.Text, e.Font, fore, e.Bounds, format);

			fore.Dispose();
			back.Dispose();
		}

		private void linkLabelDefault_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			newColorSet.Reset();
			OnColorChanged();
		}

		private void linkLabelForeColor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			newColorSet.ActiveForeColor = ChangeColor(newColorSet.ActiveForeColor);
			OnColorChanged();
		}

		private void linkLabelBackColor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			newColorSet.ActiveBackColor = ChangeColor(newColorSet.ActiveBackColor);
			OnColorChanged();
		}

		private void linkLabelForeColor2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			newColorSet.DeactiveForeColor = ChangeColor(newColorSet.DeactiveForeColor);
			OnColorChanged();
		}

		private void linkLabelBackColor2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			newColorSet.DeactiveBackColor = ChangeColor(newColorSet.DeactiveBackColor);
			OnColorChanged();
		}

		private void TabColorChangeDialog_Load(object sender, EventArgs e)
		{
			tabControlSample.TabPages.Clear();
			tabControlSample.TabPages.Add(tabText);
			tabControlSample.TabPages.Add(tabText);

			tabControlSample.SelectedIndex = 0;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			Twinie.Settings.Dialogs.ColorDialogCustomColors = 
				colorDialog1.CustomColors;
		}
	}
}