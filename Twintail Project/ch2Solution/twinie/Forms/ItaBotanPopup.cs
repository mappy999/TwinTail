// ItaBotanPopup.cs

namespace Twin.Forms
{
	using System;
	using System.Windows.Forms;
	using System.ComponentModel;

	/// <summary>
	/// 板ボタンのカテゴリをポップアップするメニュー
	/// </summary>
	public class ItaBotanPopup : ContextMenuStrip
	{
		private IBoardTable table;
		private Category category;

		/// <summary>
		/// ポップアップしたメニューがクリックされたときに発生
		/// </summary>
		public new event BoardTableEventHandler Click;

		/// <summary>
		/// ItaBotanPopupクラスのインスタンスを初期化
		/// </summary>
		/// <param name="cate"></param>
		public ItaBotanPopup(Category cate)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
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
		/// ItaBotanPopupクラスのインスタンスを初期化
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
		/// メニューアイテムがクリックされた
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
		/// このインスタンスのメニューがポップアップされた
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
		/// カテゴリメニューがポップアップされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CateMenuItem_Select(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			category = menu.Tag as Category;
		}

		/// <summary>
		/// Clickイベントを発生させる
		/// </summary>
		/// <param name="board"></param>
		private void OnClick(BoardInfo board, bool shiftPushed)
		{
			if (Click != null)
				Click(this, new BoardTableEventArgs(board, shiftPushed));
		}
	}
}
