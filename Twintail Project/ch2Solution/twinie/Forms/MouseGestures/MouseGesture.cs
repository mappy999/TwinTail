using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Twin
{
	public class MouseGesture
	{
		class Direction
		{
			public bool enabled;
			public int length;

			public Direction()
			{
				Reset();
			}
			public void Reset()
			{
				enabled = true;
				length = 0;
			}
		}

		private Control target;

		private Direction[] direction;
		private Point startPos = Point.Empty;

		private bool enabled = false;
		public bool Enabled
		{
			get
			{
				return enabled;
			}
		}

		private int range = 30;
		public int Range
		{
			get
			{
				return range;
			}
			set
			{
				range = Math.Max(15, value);
			}
		}

		public MouseGesture()
		{
			direction = new Direction[4];

			for (int i = 0; i < direction.Length; i++)
				direction[i] = new Direction();
		}

		private void Reset()
		{
			foreach (Direction d in direction)
				d.Reset();
		}

		public void Start(Control control, Point location)
		{
			this.enabled = true;
			this.startPos = location;
			this.target = control;

			Reset();
		//	target.Capture = true;
		}

		public void End()
		{
			if (enabled)
			{
		//		target.Capture = false;
				enabled = false;
			}
		}

		public Arrow Test(Point newPos)
		{
			if (!enabled)
				return Arrow.None;

			Arrow arrow = CheckDirection(this.startPos, newPos);

			switch (arrow)
			{
				case Arrow.Up:
					direction[(int)Arrow.Up].length += startPos.Y - newPos.Y;
					direction[(int)Arrow.Down].length = 0;
					break;

				case Arrow.Down:
					direction[(int)Arrow.Down].length += newPos.Y - startPos.Y;
					direction[(int)Arrow.Up].length = 0;
					break;

				case Arrow.Left:
					direction[(int)Arrow.Left].length += startPos.X - newPos.X;
					direction[(int)Arrow.Right].length = 0;
					break;

				case Arrow.Right:
					direction[(int)Arrow.Right].length += newPos.X - startPos.X;
					direction[(int)Arrow.Left].length = 0;
					break;
			}

			startPos = newPos;

			if (arrow != Arrow.None)
			{
				if (direction[(int)arrow].enabled &&
					direction[(int)arrow].length > range)
				{
					Reset();
					direction[(int)arrow].enabled = false;

					return arrow;
				}
			}

			return Arrow.None;
		}

		private Arrow CheckDirection(Point oldPos, Point newPos)
		{
			int vectorX = newPos.X - oldPos.X;
			int vectorY = newPos.Y - oldPos.Y;

			if (Math.Abs(vectorX) > Math.Abs(vectorY))
			{
				if (vectorX < 0)
					return Arrow.Left;

				else if (vectorX > 0)
					return Arrow.Right;
			}
			else
			{
				if (vectorY < 0)
					return Arrow.Up;

				else if (vectorY > 0)
					return Arrow.Down;
			}

			return Arrow.None;
		}

		public static string ArrowToString(params Arrow[] args)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Arrow arrow in args)
			{
				if (arrow == Arrow.Up)
					sb.Append("Å™");
				else if (arrow == Arrow.Down)
					sb.Append("Å´");
				else if (arrow == Arrow.Left)
					sb.Append("Å©");
				else if (arrow == Arrow.Right)
					sb.Append("Å®");
			}
			return sb.ToString();
		}
	}

	public enum Arrow
	{
		None = -1,
		Up = 0,
		Down,
		Left,
		Right,
	}
}
