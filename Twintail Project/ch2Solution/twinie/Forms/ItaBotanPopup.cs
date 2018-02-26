// ItaBotanPopup.cs

namespace Twin.Forms
{
	using System;
	using System.Windows.Forms;
	using System.ComponentModel;

	/// <summary>
	/// �{�^���̃J�e�S�����|�b�v�A�b�v���郁�j���[
	/// </summary>
	public class ItaBotanPopup : ContextMenuStrip
	{
		private IBoardTable table;
		private Category category;

		/// <summary>
		/// �|�b�v�A�b�v�������j���[���N���b�N���ꂽ�Ƃ��ɔ���
		/// </summary>
		public new event BoardTableEventHandler Click;

		/// <summary>
		/// ItaBotanPopup�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cate"></param>
		public ItaBotanPopup(Category cate)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.category = cate;
			this.table = null;

			foreach (BoardInfo board in cate.Children)
			{
				ToolStripMenuItem menu = new ToolStripMenuItem(board.Name);
				menu.Click += new EventHandler(MenuItem_Click);
				menu.Tag = board;

				Items.Add(menu);
			}
		}

		/// <summary>
		/// ItaBotanPopup�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="table"></param>
		public ItaBotanPopup(IBoardTable table)
		{
			this.table = table;
			this.category = null;
			this.Opening += new CancelEventHandler(This_Popup);
			this.Closed += new ToolStripDropDownClosedEventHandler(This_Closed);

			Items.Add(new ToolStripMenuItem("dummy"));
		}

		/// <summary>
		/// ���j���[�A�C�e�����N���b�N���ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			
			OnClick(menu.Tag as BoardInfo,
				(Control.ModifierKeys & Keys.Shift) != 0);
		}

		/// <summary>
		/// ���̃C���X�^���X�̃��j���[���|�b�v�A�b�v���ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void This_Popup(object sender, CancelEventArgs e)
		{
			Items.Clear();

			foreach (Category category in table.Items)
			{
				ToolStripMenuItem menu = new ToolStripMenuItem();
				menu.Text = category.Name;
				menu.MouseHover += new EventHandler(CateMenuItem_Select);
				menu.Tag = category;

				foreach (BoardInfo board in category.Children)
				{
					ToolStripMenuItem child = new ToolStripMenuItem();
					child.Text = board.Name;
					child.Click += new EventHandler(MenuItem_Click);
					child.Tag = board;

					menu.DropDownItems.Add(child);
				}

				Items.Add(menu);
			}
		}

		void This_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			Items.Clear();
			Items.Add(new ToolStripMenuItem("dummy"));
		}

		/// <summary>
		/// �J�e�S�����j���[���|�b�v�A�b�v���ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CateMenuItem_Select(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			category = menu.Tag as Category;
		}

		/// <summary>
		/// Click�C�x���g�𔭐�������
		/// </summary>
		/// <param name="board"></param>
		private void OnClick(BoardInfo board, bool shiftPushed)
		{
			if (Click != null)
				Click(this, new BoardTableEventArgs(board, shiftPushed));
		}
	}
}
