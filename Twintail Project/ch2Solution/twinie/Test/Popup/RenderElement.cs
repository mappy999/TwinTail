using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Twin.Test
{
	public class RenderElement : RenderObject
	{
		private List<RenderObject> childNodes = new List<RenderObject>();
		public override List<RenderObject> ChildNodes
		{
			get
			{
				return childNodes;
			}
		}


		public override bool HasChildNodes
		{
			get
			{
				return childNodes.Count > 0;
			}
		}

		public RenderElement(__HtmlElement element)
			: base(element)
		{
		}

		public override void Layout(Graphics g, Rectangle bounds,
			RenderStyle style, ref Point current)
		{
			base.Layout(g, bounds, style, ref current);

			if (!HasChildNodes)
				return;

			foreach (RenderObject child in ChildNodes)
			{
				child.Layout(g, bounds, style, ref current);

				base.Bounds.AddRange(child.Bounds);
			}
		}

		public override void Paint(PaintEventArgs e, Point location)
		{
			if (!HasChildNodes)
				return;

			foreach (RenderObject child in ChildNodes)
			{
				child.Paint(e, location);
			}
		}
	}
}
