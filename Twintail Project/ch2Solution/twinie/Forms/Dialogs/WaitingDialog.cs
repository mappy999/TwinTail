using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Twin.Forms
{
	public partial class WaitingDialog : Form
	{
		public WaitingDialog(string message)
		{
			InitializeComponent();

			this.label1.Text = message;
		}
	}
}