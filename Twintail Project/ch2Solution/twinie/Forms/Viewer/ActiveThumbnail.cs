using System;
using System.Collections.Generic;
using System.Text;
using mshtml;
using System.Windows.Forms;
using System.Drawing;

namespace Twin.Forms
{/*
	class ActiveThumbnail
	{
		private WebBrowser browser;
		private bool attached = false;

		private Dictionary<int, HtmlElement> dic = new Dictionary<int, ResSet>();

		public ActiveThumbnail(WebBrowser b)
		{
			browser = b;
		}

		public void Written(ResSetCollection resColl)
		{
			foreach (ResSet res in resColl)
			{
				HtmlElement e = browser.Document.GetElementById(res.Index.ToString());
				if (e != null && !dic.ContainsKey(res.Index))
					dic.Add(res.Index, e);
			}
		}

		void Window_Scroll(object sender, HtmlElementEventArgs e)
		{
			List<HtmlElement> targets = new List<HtmlElement>();
			Rectangle offset = browser.Document.Body.OffsetRectangle;

			foreach (KeyValuePair<int, HtmlElement> pair in dic)
			{
				HtmlElement e = pair.Value;
				if (offset.Contains(e.OffsetRectangle))
					targets.Add(pair);
			}
			
			// targetsÇ…âÊëúÉäÉìÉNÇñÑÇﬂçûÇÒÇ≈Ç¢Ç≠
			foreach (HtmlElement element in targets)
			{
			}
		}

		public void Attach()
		{
			if (attached)
				return;

			browser.Document.Window.Scroll += new HtmlElementEventHandler(Window_Scroll);
			attached = true;
		}

		public void Dettach()
		{
			if (attached)
			{
				browser.Document.Window.Scroll -= new HtmlElementEventHandler(Window_Scroll);
			}
		}
	}*/
}
