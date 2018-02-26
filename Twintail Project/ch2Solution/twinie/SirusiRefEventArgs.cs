using System;
using System.Collections.Generic;
using System.Text;

namespace Twin
{
	public class SirusiRefEventArgs : EventArgs
	{
		public ThreadHeader HeaderInfo { get; private set; }
		public ResSet ResSet { get; private set; }

		public SirusiRefEventArgs(ThreadHeader h, ResSet r)
		{
			if (h == null) throw new ArgumentNullException();
			this.HeaderInfo = h;
			this.ResSet = r;
		}
	}
}
