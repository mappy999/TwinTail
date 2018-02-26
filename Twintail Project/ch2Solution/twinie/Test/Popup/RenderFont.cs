using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Twin.Test
{
	public class RenderFont : RenderElement
	{
		private Color fontColor = Color.Empty;
		private string fontName = null;

		public RenderFont(__HtmlElement e)
			: base(e)
		{
			fontName = e.Attributes["FACE"];

			// COLORëÆê´Ç©ÇÁ Color ç\ë¢ëÃÇèâä˙âª
			string attrColor = e.Attributes["COLOR"];

			if (!String.IsNullOrEmpty(attrColor))
				fontColor = HtmlTextUtility.ColorFromHtml(attrColor);
		}

		public override void Layout(Graphics g, Rectangle bounds, RenderStyle style, ref Point current)
		{
			RenderStyle newStyle = new RenderStyle(style);

			if (!fontColor.IsEmpty)
			{
				newStyle.ForeColor = fontColor;
			}
			if (!String.IsNullOrEmpty(fontName))
			{
				newStyle.FontFamily = new FontFamily(fontName);
			}

			base.Layout(g, bounds, newStyle, ref current);
		}
	}
}
