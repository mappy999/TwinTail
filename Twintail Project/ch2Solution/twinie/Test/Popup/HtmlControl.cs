using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Twin.Test
{
	/// <summary>
	/// HTML から描画情報を生成するクラス。
	/// </summary>
	public class HtmlControl : Control
	{
		private Dictionary<string, Type> __table = new Dictionary<string, Type>();

		private __HtmlDocument document;
		private RenderRoot renderRoot = null;

		public Rectangle ScrollRectangle
		{
			get
			{
				Rectangle rc = renderRoot.Bounds[0];
				return rc;
			}
		}

		private Point scrollTop = Point.Empty;

		public Point ScrollTop
		{
			get
			{
				return scrollTop;
			}
			set
			{
				if (scrollTop != value)
				{
					scrollTop = value;
					Refresh();
				}
			}
		}
	
	

		private bool painting = false;
		private bool layouted = false;
		private bool layouting = false;

		public HtmlControl()
		{
			this.DoubleBuffered = true;
			this.Paint += new System.Windows.Forms.PaintEventHandler(OnPaint);

			RegisterElements();
		}

		private void RegisterElements()
		{
			__table.Add("HEAD", typeof(RenderHead));
			__table.Add("DL", typeof(RenderDL));
			__table.Add("DT", typeof(RenderDT));
			__table.Add("DD", typeof(RenderDD));
			__table.Add("A", typeof(RenderAnchor));
			__table.Add("FONT", typeof(RenderFont));
			__table.Add("B", typeof(RenderBold));
		}

		private RenderElement CreateElement(string tagName, __HtmlElement element)
		{
			Type type;
			
			if (__table.ContainsKey(tagName))
			{
				type = __table[tagName];
			}
			else
			{
				type = typeof(RenderElement);
			}

			return (RenderElement)Activator.CreateInstance(type, element);
		}

		private void CreateRenderingInfo()
		{
			Stack<HtmlNode> nodeStack = new Stack<HtmlNode>();
			Stack<RenderObject> renderStack = new Stack<RenderObject>();

			HtmlNode node = renderRoot.Node;
			RenderObject render = renderRoot;

			while (true)
			{
				if (node == null)
				{
					if (nodeStack.Count == 0)
						break;

					node = nodeStack.Pop().NextSibling;
					render = renderStack.Pop();
				}
				else
				{
					if (node.FirstChild == null)
					{
						RenderObject obj;

						switch (node.Name)
						{
						case "#text":
							obj = new RenderText((HtmlText)node);
							break;
						case "BR":
							obj = new RenderBr((__HtmlElement)node);
							break;
						default:
							obj = new RenderElement((__HtmlElement)node);
							break;
						}

						render.ChildNodes.Add(obj);

						node = node.NextSibling;
					}
					else
					{
						RenderObject obj = CreateElement(node.Name, (__HtmlElement)node);

						render.ChildNodes.Add(obj);

						renderStack.Push(render);
						render = obj;

						nodeStack.Push(node);
						node = node.FirstChild;
					}
				}
			}

		}

		void OnPaint(object sender, PaintEventArgs e)
		{
			if (painting)
				return;

			if (renderRoot == null)
				return;

			try
			{
				Stopwatch watch = Stopwatch.StartNew();

				painting = true;

				if (layouted)
					renderRoot.Paint(e, scrollTop);

				watch.Stop();
				Console.WriteLine("+renderRoot.Paint {0}ms", watch.ElapsedMilliseconds);

			}
			finally
			{
				painting = false;
			}
		}

		public void SetDocument(__HtmlDocument document)
		{
			__HtmlElement[] elements = document.GetElementsByName("HTML");

			if (elements.Length == 0)
				throw new ArgumentNullException("ルート要素 <HTML> が見つかりませんでした");

			this.renderRoot = new RenderRoot(elements[0]);
			this.document = document;
			this.scrollTop = Point.Empty;

			CreateRenderingInfo();
		}

		public void LayoutHtml()
		{
			if (layouting)
				return;

			if (renderRoot == null)
				return;

			layouting = true;

			try
			{
				Point location = scrollTop;

				using (Graphics g = CreateGraphics())
				{
					renderRoot.Layout(g, Bounds,
						RenderStyle.Default, ref location);
				}
			}
			finally
			{
				layouting = false;
				layouted = true;
			}

		}
	}
}
