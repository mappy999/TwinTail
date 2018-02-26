using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Twin.Test
{
	public class RenderBold : RenderElement
	{
		public RenderBold(__HtmlElement e)
			: base(e)
		{
		}

		public override void Layout(Graphics g, Rectangle bounds, RenderStyle style, ref Point current)
		{
			RenderStyle newStyle = new RenderStyle(style);

			newStyle.FontStyle |= FontStyle.Bold;

			base.Layout(g, bounds, newStyle, ref current);
		}
	}
}
