using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using CSharpSamples.Text.Search;

namespace Twin.Test
{
	public class RenderText : RenderObject
	{
		private ISearchable brSearch = new BmSearch2("<br>");
		private string innerText;

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

		public RenderText(HtmlText text) : base(text)
		{
			this.innerText = HtmlTextUtility.UnEscape(text.InnerText);
		}

		#region MeasureString
		public static Rectangle MeasureString(Graphics g, Font font, string text)
		{
			List<CharacterRange[]> rangesList = new List<CharacterRange[]>();
			int measureCharacterRangeLimit = 32; // 一度に 32 文字以上は計測できない
			int length = text.Length;

			while (length > 0)
			{
				int arrayCount = Math.Min(measureCharacterRangeLimit, length);
				CharacterRange[] ranges = new CharacterRange[arrayCount];

				for (int i = 0; i < arrayCount; i++)
				{
					ranges[i].First = measureCharacterRangeLimit * rangesList.Count + i;
					ranges[i].Length = 1;
				}

				rangesList.Add(ranges);
				length -= arrayCount;
			}

			StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoClip);
			RectangleF bounds = Rectangle.Empty;

			foreach (CharacterRange[] ranges in rangesList)
			{
				format.SetMeasurableCharacterRanges(ranges);

				Region[] regions = g.MeasureCharacterRanges(text, font,
					new RectangleF(0, 0, 1000, 1000), format);

				foreach (Region r in regions)
				{
					bounds = RectangleF.Union(bounds, r.GetBounds(g));
					r.Dispose();
				}
			}

			return Rectangle.Truncate(bounds);
		}
		#endregion

		public override void Layout(Graphics g, Rectangle bounds, RenderStyle style, ref Point current)
		{
			base.Layout(g, bounds, style, ref current);

			// 文字計測に使用するフォントを作成
			Font font = style.CreateFont();
			
			try
			{
				Rectangle rc = MeasureString(g, font, innerText);
				rc.Location = current;

				current.X += rc.Width;

				if (current.X > RenderRoot.scrollWidth)
					RenderRoot.scrollWidth = current.X;
					
				base.Bounds.Add(rc);
			}
			finally
			{
				style.Release();
			}
		}

		public override void Paint(PaintEventArgs e, Point location)
		{
			try
			{
				Font font = Style.CreateFont();
				StringFormat format = new StringFormat(StringFormatFlags.NoClip);

				Rectangle offset = new Rectangle(Bounds[0].X + location.X, Bounds[0].Y + location.Y,
					Bounds[0].Width, Bounds[0].Height);

				if (e.ClipRectangle.IntersectsWith(offset))
				{
					using (Brush brush = new SolidBrush(Style.ForeColor))
					{
						e.Graphics.DrawString(innerText, font, brush,
							new Point(offset.X, offset.Y), format);
					}
				}
			}
			finally
			{
				Style.Release();
			}
		}
	}
}
