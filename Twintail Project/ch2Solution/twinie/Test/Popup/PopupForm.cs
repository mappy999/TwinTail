using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Twin.Test
{
	public partial class PopupForm : Form
	{
		private HtmlControl htmlCtrl;
		private Size maximumSize = new Size(500, 300);
		private int margin = 5;

		private VScrollBar vScroll;

		private bool locked = false;

		private bool hideOnClose = true;

		public bool HideOnClose
		{
			get
			{
				return hideOnClose;
			}
			set
			{
				hideOnClose = value;
			}
		}

		public event EventHandler PopupHidden;

		public PopupForm()
		{
			InitializeComponent();

			this.TopMost = true;

			htmlCtrl = new HtmlControl();
			htmlCtrl.Location = new Point(margin, margin);
			htmlCtrl.Size = new Size(this.Width - margin * 2, this.Height - margin * 2);
			htmlCtrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

			vScroll = new VScrollBar();
			vScroll.Dock = DockStyle.Right;
			vScroll.Scroll += new ScrollEventHandler(vScroll_Scroll);
			vScroll.Width = SystemInformation.VerticalScrollBarWidth;
			vScroll.Minimum = vScroll.Maximum = vScroll.Value = 0;
			vScroll.LargeChange = 1;
			vScroll.SmallChange = 1;

			Controls.Add(vScroll);
			Controls.Add(htmlCtrl);
		}

		void vScroll_Scroll(object sender, ScrollEventArgs e)
		{
			htmlCtrl.ScrollTop = new Point(0, -e.NewValue * htmlCtrl.Font.Height);
		}

		public void Lock()
		{
			locked = true;
		}

		public void Unlock()
		{
			locked = false;
		}

		public void ShowPopup(string html, Point location)
		{
			this.Location = new Point(-999, -999);
			this.Size = maximumSize;

			htmlCtrl.SetDocument(new __HtmlDocument(html));
			htmlCtrl.LayoutHtml();

			Rectangle scroll = htmlCtrl.ScrollRectangle;

			int scrollWidth = scroll.Width + margin * 2;
			int scrollHeight = scroll.Height + margin * 2;

			if (this.Width > scrollWidth)
				this.Width = scrollWidth;

			if (this.Height > scrollHeight)
			{
				this.Height = scrollHeight;
				vScroll.Maximum = vScroll.Value = 0;
				vScroll.Visible = false;
			}
			else
			{
				vScroll.Value = 0;
				vScroll.LargeChange = this.Height / htmlCtrl.Font.Height; 
				vScroll.Maximum = scrollHeight / htmlCtrl.Font.Height + 1;
				vScroll.Visible = true;
			}

			this.Location = new Point(
				Math.Max(0, location.X - 10),
				Math.Max(0, location.Y - this.Height + 10));


		//	Form1.NotDeactivate = true;

			Show();

			Application.DoEvents();
		//	Form1.NotDeactivate = false;
		}

		public void HidePopup()
		{
			if (locked)
				return;

			if (hideOnClose)
				Hide();
			else
				Close();

			OnPopupHidden();
		}

		private void OnPopupHidden()
		{
			if (PopupHidden != null)
				PopupHidden(this, EventArgs.Empty);
		}

		private void PopupForm_MouseEnter(object sender, EventArgs e)
		{
			locked = false;
		}

		private void PopupForm_MouseLeave(object sender, EventArgs e)
		{
			HidePopup();
		}

		private void PopupForm_MouseClick(object sender, MouseEventArgs e)
		{
		}
	}
}