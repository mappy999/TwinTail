// TabUtility.cs
// #2.0

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Collections.Generic;
	using Twin.Bbs;
	using Twin.Properties;

	/// <summary>
	/// タブ関連のユーティリティ群
	/// </summary>
	public class TabUtility
	{
		private static TabPage lastSelectedTab;
		private static bool isDraging;

		// タブを移動する先にある、強調表示されるタブ
		private static TabPage dropPositionTab = null;
		private static Rectangle prevDrawRect = Rectangle.Empty;

		/// <summary>
		/// 最後に選択されたタブを取得します。
		/// </summary>
		public static TabPage SelectedTab
		{
			get
			{
				return lastSelectedTab;
			}
		}

		public static event EventHandler ChangedPosition;

		static TabUtility()
		{
		}

		/// <summary>
		/// 指定したタブコントロールのマウスクリックを監視するイベントハンドラを設定します。
		/// </summary>
		/// <param name="tabctrl"></param>
		public static void SetTabControl(TabControl tabctrl)
		{
			tabctrl.MouseDown += new MouseEventHandler(OnMouseDown);
			tabctrl.MouseMove += new MouseEventHandler(OnMouseMove);
			tabctrl.MouseUp += new MouseEventHandler(OnMouseUp);
		}

		/// <summary>
		/// マウスがクリックされたときに座標位置にあるタブを取得します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnMouseDown(object sender, MouseEventArgs e)
		{
			TabControl tabctrl = (TabControl)sender;

			lastSelectedTab = GetItemAt(tabctrl, e.X, e.Y);

			if (lastSelectedTab != null && e.Button != MouseButtons.Left)
			{
				tabctrl.SelectedTab = lastSelectedTab;
			}
		}

		private static void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				isDraging = true;

				TabControl tabctrl = (TabControl)sender;

				// 移動先の位置をわかりやすくするため、マウス直下のタブを強調表示する
				TabPage currentTab = GetItemAt(tabctrl, e.X, e.Y);

				if (currentTab == null)
					return;

				if (lastSelectedTab == currentTab || dropPositionTab == currentTab)
					return;

				HighlightTab(tabctrl, currentTab);

				dropPositionTab = currentTab;
			}
			else if (e.Button == MouseButtons.None)
			{
			}
		}

		/// <summary>
		/// マウスがクリックされたときに座標位置にあるタブを取得
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnMouseUp(object sender, MouseEventArgs e)
		{
			TabControl tabctrl = (TabControl)sender;

			if (e.Button == MouseButtons.Left)
			{
				TabPage target = GetItemAt(tabctrl, e.X, e.Y);
				if (isDraging && target != null && lastSelectedTab != target)
				{
					ChangePosition(tabctrl,
						tabctrl.TabPages.IndexOf(lastSelectedTab),
						tabctrl.TabPages.IndexOf(target));
				}
				isDraging = false;
			}

			dropPositionTab = null;

			if (prevDrawRect != Rectangle.Empty)
			{
				tabctrl.Invalidate(prevDrawRect);
				prevDrawRect = Rectangle.Empty;
			}
		}


		/// <summary>
		/// タブの位置を変更
		/// </summary>
		/// <param name="ctrl">タブコントロール</param>
		/// <param name="from">変更するタブページのインデックス</param>
		/// <param name="to">移動先のタブページコレクション内インデックス</param>
		public static void ChangePosition(TabControl ctrl, int from, int to)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			if (from < 0 || from >= ctrl.TabCount ||
				to < 0 || to > ctrl.TabCount)
			{
				return;
			}

			TabPage target = ctrl.TabPages[from];
			ctrl.Enabled = false;
			ctrl.SuspendLayout();
			ctrl.SelectedTab = null;
			ctrl.TabPages.Remove(target);
			ctrl.TabPages.Insert(to, target);
			ctrl.SelectedTab = null;
			ctrl.SelectedTab = target;
			ctrl.ResumeLayout();
			ctrl.Enabled = true;

			OnChangedPosition(ctrl);
		}

		/// <summary>
		/// 指定した座標にあるタブページを取得
		/// </summary>
		/// <param name="tabctrl"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static TabPage GetItemAt(TabControl tabctrl, int x, int y)
		{
			if (tabctrl == null)
			{
				throw new ArgumentNullException("tabctrl");
			}
			for (int i = 0; i < tabctrl.TabCount; i++)
			{
				Rectangle rect = tabctrl.GetTabRect(i);
				if (rect.Contains(x, y))
				{
					return tabctrl.TabPages[i];
				}
			}
			return null;
		}

		public static TabPage GetItemAt(TabControl tabctrl, Point location)
		{
			return GetItemAt(tabctrl, location.X, location.Y);
		}

		/// <summary>
		/// 指定したタブを強調表示
		/// </summary>
		/// <param name="tab"></param>
		public static void HighlightTab(TabControl ctrl, TabPage tab)
		{
			if (tab == null)
				return;

			int index = ctrl.TabPages.IndexOf(tab);

			if (index == -1)
				return;

			Rectangle rc = ctrl.GetTabRect(index);

			// 前回描画したタブを再描画
			if (prevDrawRect != Rectangle.Empty)
			{
				ctrl.Invalidate(prevDrawRect);
			}

			// タブの指定した位置 (style) にラインを引く
			using (Graphics g = ctrl.CreateGraphics())
			{
				Color baseColor = Color.FromArgb(100, ControlPaint.Light(SystemColors.Highlight));
				Brush brush = new SolidBrush(baseColor);

				g.FillRectangle(brush, rc);

				brush.Dispose();
			}

			prevDrawRect = rc;
		}

		/// <summary>
		/// 指定したタブを描画
		/// </summary>
		/// <param name="control"></param>
		/// <param name="e"></param>
		public static void DrawItem<THeader, TControl>(TabControl control,
			DrawItemEventArgs e) where TControl : ClientBase//Ex<THeader>
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e.Index < 0 || e.Index >= control.TabCount)
				return;

			TabPage tab = control.TabPages[e.Index];
			Image image = null;
			int imgheight = 0;
			int imgwidth = 0;

			if (tab.Tag == null)
				throw new NullReferenceException("tab.Tag is null");

			TwinWindow<THeader, TControl> window = 
				(TwinWindow<THeader, TControl>)tab.Tag;

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags = StringFormatFlags.NoWrap;

			if (control.Alignment == TabAlignment.Left ||
				control.Alignment == TabAlignment.Right)
			{
				format.FormatFlags |= StringFormatFlags.DirectionVertical;
			}

			// なぜか描画位置がずれるので補正
			if (control.Alignment == TabAlignment.Top &&
				control.SelectedTab != tab)
			{
				imgheight = 3;
			}

			if (tab.ImageIndex != -1)
			{
				image =
					control.ImageList.Images[tab.ImageIndex];

				if (control.Alignment == TabAlignment.Left ||
					control.Alignment == TabAlignment.Right)
				{
					imgheight += image.Height + 5;
				}
				else
				{
					imgwidth = image.Width + 5;
				}
			}

			Rectangle rect = new Rectangle(
				imgwidth + e.Bounds.X, imgheight + e.Bounds.Y,
				e.Bounds.Width - imgwidth, e.Bounds.Height - imgheight);

			Brush backBrush, foreBrush;
			bool isColoring = !window.ColorSet.IsDefaultColor;

			if (e.State == DrawItemState.Selected)
			{
				backBrush = new SolidBrush(
					isColoring ? window.ColorSet.ActiveBackColor : SystemColors.Control);

				foreBrush = new SolidBrush(
					isColoring ? window.ColorSet.ActiveForeColor : SystemColors.ControlText);
			}
			else
			{
				backBrush = new SolidBrush(
					isColoring ? window.ColorSet.DeactiveBackColor : SystemColors.ControlDark);

				foreBrush = new SolidBrush(
					isColoring ? window.ColorSet.DeactiveForeColor : SystemColors.ControlLightLight);
			}

			e.Graphics.FillRectangle(backBrush, e.Bounds);
			e.Graphics.DrawString(tab.Text, e.Font, foreBrush, rect, format);

			backBrush.Dispose();
			foreBrush.Dispose();

			// 選択中のタブを強調表示する
			Color c = Twinie.Settings.Design.TabHighlightColor;
			if (! c.IsEmpty && e.State == DrawItemState.Selected)
			{
				Rectangle rc = e.Bounds;
				rc.Inflate(-1, -1);

				using (Pen pen = new Pen(c, 2))
					e.Graphics.DrawRectangle(pen, rc);
			}

			if (image != null)
			{
				int left = (image.Width + 5) / 2;

				e.Graphics.DrawImage(image,
					e.Bounds.X + left, e.Bounds.Y + left,
					image.Width, image.Height);
			}

			// 電気アイコンを描画
			if (window.Referenced)
			{
				using (var denki = (Bitmap)Resources.denki.Clone())
				{
					denki.MakeTransparent(Color.White);
					int y = e.Bounds.Top + 3;//(e.Bounds.Height - denki.Height) / 2;
					int x = e.Bounds.Left;
					e.Graphics.DrawImage(denki, x, y);
				}
			}
		}

		private static void OnChangedPosition(TabControl tab)
		{
			if (ChangedPosition != null)
				ChangedPosition(tab, EventArgs.Empty);
		}
	}

}
