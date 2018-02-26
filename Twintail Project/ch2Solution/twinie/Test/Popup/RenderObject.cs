using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Twin.Test
{
	public abstract class RenderObject
	{
		public abstract List<RenderObject> ChildNodes
		{
			get;
		}

		public abstract bool HasChildNodes
		{
			get;
		}
	
		private HtmlNode node;
		public HtmlNode Node
		{
			get
			{
				return node;
			}
		}

		private List<Rectangle> bounds = new List<Rectangle>();
		public List<Rectangle> Bounds
		{
			get
			{
				return bounds;
			}
		}
	
		protected RenderStyle renderStyle = null;
		public RenderStyle Style
		{
			get
			{
				return renderStyle;
			}
		}

		public RenderObject(HtmlNode node)
		{
			this.node = node;
		}
	
	
		public virtual void Layout(Graphics g, Rectangle bounds, 
			RenderStyle style, ref Point current)
		{
			this.renderStyle = new RenderStyle(style);
			this.bounds.Clear();
		}

		public abstract void Paint(PaintEventArgs e, Point location);
	}
}
