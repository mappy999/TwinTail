using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Twin.Test
{
	public class RenderBr : RenderElement
	{
		public override List<RenderObject> ChildNodes
		{
			get
			{
				return null;
			}
		}

		public override bool HasChildNodes
		{
			get
			{
				return false;
			}
		}

		public RenderBr(__HtmlElement e)
			: base(e)
		{
		}

		public override void Paint(System.Windows.Forms.PaintEventArgs e, Point location)
		{
		}

		public override void Layout(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, RenderStyle style, ref System.Drawing.Point current)
		{
			try
			{
				Rectangle rc = RenderText.MeasureString(g, style.CreateFont(), "M");

				current.X = 0;
				current.Y += rc.Height + 1;
			}
			finally
			{
				style.Release();
			}
		}
	}
}
