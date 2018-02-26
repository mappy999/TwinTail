using System;
using System.Collections.Generic;
using System.Text;
using CSharpSamples;
using System.Runtime.Serialization;
using System.ComponentModel;
using Twintail3;

namespace Twin
{
	public class MouseGestureActionSettings : ApplicationSettingsSerializer
	{
		internal List<MouseGestureActionItem> list = new List<MouseGestureActionItem>();

		[DefaultArraySet(0)]
		[ExpandableSerialize]
		public MouseGestureActionItem[] Items
		{
			get { return list.ToArray(); }
			set
			{
				list.Clear();
				list.AddRange(value);
			}
		}

		private int range;
		[DefaultValue(30)]
		public int Range
		{
			get
			{
				return range;
			}
			set
			{
				range = value;
			}
		}

		public MouseGestureActionSettings(string path) : base(path, true)
		{
		}
	}

	public class MouseGestureActionItem
	{
		private Arrow[] arrows;
		[DefaultArraySet(0)]
		public Arrow[] Arrows
		{
			get { return arrows; }
			set { arrows = value; }
		}

		private MouseGestureAction action;
		[DefaultValue("None")]
		public MouseGestureAction Action
		{
			get { return action; }
			set { action = value; }
		}

		public MouseGestureActionItem()
		{
		}
		public MouseGestureActionItem(Arrow[] arrows, MouseGestureAction action)
		{
			this.arrows = arrows;
			this.action = action;
		}
		public override string ToString()
		{
			return "(" + Action.ToString() + ") " + MouseGesture.ArrowToString(Arrows);
		}
	}

	public enum MouseGestureAction
	{
		None,

		Reload,
		ReloadAll,
		ReloadSubject,

		FillThread,
		FillSubject,

		ScrollTop,
		ScrollBottom,

		Close,
		CloseLeft,
		CloseRight,
		CloseAll,

		SelectNextTab,
		SelectPrevTab,

		ShowExtractDialog,
		ShowSearchDialog,
		ShowWriteDialog,
		ShowDockWriteBar,

		SetBookmark,
		UpdateBookmark,
	}
}
