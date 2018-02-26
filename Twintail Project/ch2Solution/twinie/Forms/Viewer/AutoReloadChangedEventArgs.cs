using System;
using System.Collections.Generic;
using System.Text;

namespace Twin
{
	public class AutoReloadChangedEventArgs : EventArgs
	{
		private bool newValue;
		public bool NewValue
		{
			get
			{
				return newValue;
			}
		}

		private ThreadHeader target;
		public ThreadHeader Target
		{
			get
			{
				return target;
			}
		}

		private bool cancelled = false;
		public bool Cancelled
		{
			get
			{
				return cancelled;
			}
			set
			{
				cancelled = value;
			}
		}
	
	
	
		public AutoReloadChangedEventArgs(ThreadHeader target, bool newValue)
		{
			this.target = target;
			this.newValue = newValue;
		}
	}
}
