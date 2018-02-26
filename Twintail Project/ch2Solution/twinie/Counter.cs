using System;
using System.Diagnostics;

namespace Twin
{
	/// <summary>
	/// Counter ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class _Counter
	{
		private static int tick;
		private static int total = 0;

		[Conditional("DEBUG")]
		public static void Start(bool reset)
		{
			if (reset) Reset();
			tick = Environment.TickCount;
		}

		[Conditional("DEBUG")]
		public static void Stop()
		{
			total += (Environment.TickCount - tick);
		}

		[Conditional("DEBUG")]
		public static void Reset()
		{
			total = 0;
		}

		[Conditional("DEBUG")]
		public static void Output(string name)
		{
			System.Windows.Forms.MessageBox.Show(name + ": " + total);
		}
	}
}
