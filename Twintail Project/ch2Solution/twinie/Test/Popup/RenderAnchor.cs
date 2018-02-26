using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Twin.Test
{
	public class RenderAnchor : RenderElement
	{
		public RenderAnchor(__HtmlElement e) : base(e)
		{
		}

		public override void Layout(Graphics g, Rectangle bounds, RenderStyle style, ref System.Drawing.Point current)
		{
			RenderStyle newStyle = new RenderStyle(style);

			newStyle.FontStyle |= FontStyle.Underline;
			newStyle.ForeColor = Color.Blue;

			base.Layout(g, bounds, newStyle, ref current);
		}
	}
}
