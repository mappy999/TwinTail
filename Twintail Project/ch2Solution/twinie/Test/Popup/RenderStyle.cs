using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Twin.Test
{
	public class RenderStyle
	{
		private List<IDisposable> disposableResources = new List<IDisposable>();

		private FontFamily fontFamily;
		public FontFamily FontFamily
		{
			get
			{
				return fontFamily;
			}
			set
			{
				fontFamily = value;
			}
		}
	
		private FontStyle fontStyle;
		public FontStyle FontStyle
		{
			get
			{
				return fontStyle;
			}
			set
			{
				fontStyle = value;
			}
		}

		private Color foreColor;

		public Color ForeColor
		{
			get
			{
				return foreColor;
			}
			set
			{
				foreColor = value;
			}
		}
	

		private float fontSize;
		public float FontSize
		{
			get
			{
				return fontSize;
			}
		}

		private static RenderStyle defaultStyle = null;
		public static RenderStyle Default
		{
			get
			{
				if (defaultStyle == null)
				{
					defaultStyle = new RenderStyle(
						Control.DefaultFont.FontFamily, 
						Control.DefaultFont.Style, 
						Control.DefaultFont.SizeInPoints);
				}

				return defaultStyle;
			}
		}

		

		public RenderStyle(FontFamily family, FontStyle style, float size)
		{
			this.fontFamily = family;
			this.fontStyle = style;
			this.fontSize = size;
			this.foreColor = SystemColors.WindowText;
		}

		public RenderStyle(RenderStyle style)
		{
			this.fontSize = style.FontSize;
			this.fontFamily = style.FontFamily;
			this.fontStyle = style.FontStyle;
			this.foreColor = style.foreColor;
		}

		public Font CreateFont()
		{
			Font f = new Font(fontFamily, fontSize, fontStyle);
			disposableResources.Add(f);

			return f;
		}

		public void Release()
		{
			foreach (IDisposable d in disposableResources)
			{
				d.Dispose();
			}
			disposableResources.Clear();
		}
	}
}
