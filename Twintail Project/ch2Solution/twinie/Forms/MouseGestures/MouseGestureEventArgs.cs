using System;
using System.Collections.Generic;
using System.Text;

namespace Twin
{
	public class MouseGestureEventArgs : EventArgs
	{
		private Arrow[] input;
		public Arrow[] Arrows
		{
			get
			{
				return input;
			}
		}

		private bool handled = false;
		public bool Handled
		{
			get
			{
				return handled;
			}
			set
			{
				handled = value;
			}
		}
	
	
		public MouseGestureEventArgs(Arrow[] input)
		{
			this.input = input;
		}

		public bool Judge(params Arrow[] arrows)
		{
			if (input.Length != arrows.Length)
				return false;

			for (int i = 0; i < input.Length; i++)
				if (input[i] != arrows[i])
					return false;

			return true;
		}
	}
}
