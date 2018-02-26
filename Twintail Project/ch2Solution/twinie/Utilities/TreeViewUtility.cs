// TreeViewUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	/// <summary>
	/// ツリービューに関するユーティリティ群
	/// </summary>
	public class TreeViewUtility
	{
		// 前回描画した長方形の一時変数
		private static Rectangle drawnRect = Rectangle.Empty;
		// 前回の描画位置
		private static DropPosition position;

		/// <summary>
		/// ドロップ先の位置を描画
		/// </summary>
		/// <param name="node"></param>
		/// <param name="location"></param>
		public static DropPosition DrawDropTo(TreeView treeView,
			TreeNode node, Point pt, BookmarkEntry from, BookmarkEntry to)
		{
			// 前回の描画を消去
			treeView.Invalidate(drawnRect);
			treeView.Update();

			Rectangle rect = node.Bounds;

			int top = rect.Top + rect.Height / 3;
			int mid = rect.Top + rect.Height / 2;
			int bottom = rect.Bottom - rect.Height / 3;

			// フォルダ対フォルダ
			if (!from.IsLeaf && !to.IsLeaf)
			{
				if (pt.Y < top)
				{
					position = DropPosition.Upper;
					treeView.SelectedNode = null;
				}
				else if (pt.Y < bottom)
				{
					position = DropPosition.Self;
					treeView.SelectedNode = node;
				}
				else {
					position = DropPosition.Lower;
					treeView.SelectedNode = null;
				}
			}
			// なにか対お気に入り
			else if (to.IsLeaf)
			{
				if (pt.Y <= mid) {
					position = DropPosition.Upper;
				} else {
					position = DropPosition.Lower;
				}
				treeView.SelectedNode = null;
			}
			// なにか対フォルダ
			else {
				position = DropPosition.Self;
				treeView.SelectedNode = node;
			}

			if (treeView.SelectedNode == null)
			{
				using (Graphics g = treeView.CreateGraphics())
				{
					if (position == DropPosition.Lower)
						rect.Y += rect.Height;

					rect.Width = treeView.ClientSize.Width - rect.Left;
					rect.Height = 2;

					g.FillRectangle(Brushes.Black, rect);
					drawnRect = rect;
				}
			}

			return position;
		}

		public static void SetTable(TreeView treeView, IBoardTable table,
			int folderImageIndex, int folderSelectedImageIndex,
			int itemImageIndex, int itemSelectedImageIndex)
		{
			SetTable(treeView, table, folderImageIndex, folderSelectedImageIndex,
				itemImageIndex, itemSelectedImageIndex, SystemColors.Window, SystemColors.Window);
		}

		public static void SetTable(TreeView treeView, IBoardTable table,
			int folderImageIndex, int folderSelectedImageIndex,
			int itemImageIndex, int itemSelectedImageIndex, Color cateBack, Color boardBack)
		{
			treeView.Nodes.Clear();

			System.Collections.ArrayList arrayList =
				new System.Collections.ArrayList();

			foreach (Category cate in table.Items)
			{
				TreeNode node = new TreeNode();
				node.Text = cate.Name;
				node.BackColor = cateBack;
				node.ImageIndex = folderImageIndex;
				node.SelectedImageIndex = folderSelectedImageIndex;
				node.Tag = cate;

				foreach (BoardInfo board in cate.Children)
				{
					TreeNode child = new TreeNode();
					child.Text = board.Name;
					child.BackColor = boardBack;
					child.ImageIndex = itemImageIndex;
					child.SelectedImageIndex = itemSelectedImageIndex;
					child.Tag = board;
					node.Nodes.Add(child);
				}

				if (cate.IsExpanded)
					node.Expand();

				arrayList.Add(node);
			}

			treeView.Nodes.AddRange((TreeNode[])arrayList.ToArray(typeof(TreeNode)));
		}
	}

	/// <summary>
	/// ドロップ先の位置を表す
	/// </summary>
	public enum DropPosition
	{
		/// <summary>
		/// 上辺
		/// </summary>
		Upper,
		/// <summary>
		/// 自身
		/// </summary>
		Self,
		/// <summary>
		/// 下辺
		/// </summary>
		Lower,
	}
}
