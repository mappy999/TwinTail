// TreeViewUtility.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	/// <summary>
	/// �c���[�r���[�Ɋւ��郆�[�e�B���e�B�Q
	/// </summary>
	public class TreeViewUtility
	{
		// �O��`�悵�������`�̈ꎞ�ϐ�
		private static Rectangle drawnRect = Rectangle.Empty;
		// �O��̕`��ʒu
		private static DropPosition position;

		/// <summary>
		/// �h���b�v��̈ʒu��`��
		/// </summary>
		/// <param name="node"></param>
		/// <param name="location"></param>
		public static DropPosition DrawDropTo(TreeView treeView,
			TreeNode node, Point pt, BookmarkEntry from, BookmarkEntry to)
		{
			// �O��̕`�������
			treeView.Invalidate(drawnRect);
			treeView.Update();

			Rectangle rect = node.Bounds;

			int top = rect.Top + rect.Height / 3;
			int mid = rect.Top + rect.Height / 2;
			int bottom = rect.Bottom - rect.Height / 3;

			// �t�H���_�΃t�H���_
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
			// �Ȃɂ��΂��C�ɓ���
			else if (to.IsLeaf)
			{
				if (pt.Y <= mid) {
					position = DropPosition.Upper;
				} else {
					position = DropPosition.Lower;
				}
				treeView.SelectedNode = null;
			}
			// �Ȃɂ��΃t�H���_
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
	/// �h���b�v��̈ʒu��\��
	/// </summary>
	public enum DropPosition
	{
		/// <summary>
		/// ���
		/// </summary>
		Upper,
		/// <summary>
		/// ���g
		/// </summary>
		Self,
		/// <summary>
		/// ����
		/// </summary>
		Lower,
	}
}
