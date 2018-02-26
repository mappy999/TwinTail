using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Twin.Test
{
	public class RenderDL : RenderElement
	{
		public RenderDL(__HtmlElement e) : base(e) {}
	}

	public class RenderDT : RenderElement
	{
		public RenderDT(__HtmlElement e) : base(e) {}
		public override void Layout(Graphics g, Rectangle bounds, RenderStyle style, ref System.Drawing.Point current)
		{
			current.X = 0;

			base.Layout(g, bounds, style, ref current);
		}
	}

	public class RenderDD : RenderElement
	{
		public RenderDD(__HtmlElement e) : base(e) {}
		public override void Layout(Graphics g, Rectangle bounds, RenderStyle style, ref System.Drawing.Point current)
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

			base.Layout(g, bounds, style, ref current);
		}
	}
}
