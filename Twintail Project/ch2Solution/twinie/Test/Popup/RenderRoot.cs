using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Twin.Test
{
	public class RenderRoot : RenderElement
	{
		public static int scrollWidth = 0;

		public RenderRoot(__HtmlElement root) : base(root)
		{
		}

		public override void Layout(Graphics g, Rectangle bounds,
			RenderStyle style, ref Point current)
		{
			scrollWidth = 0;

			base.Layout(g, bounds, style, ref current);
			base.Bounds.Clear();

			base.Bounds.Add(
				new Rectangle(0, 0, scrollWidth, current.Y));
		}

		public override void Paint(PaintEventArgs e, Point location)
		{
			base.Paint(e, location);
		}
	}
}
