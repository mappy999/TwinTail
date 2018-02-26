// SirusiDialog.cs

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CSharpSamples.Text.Search;


namespace Twin.Forms
{
	using ITwinThreadControl = ITwinTabController<ThreadHeader, ThreadControl>;

	/// <summary>
	/// SirusiDialog の概要の説明です。
	/// </summary>
	public class SirusiDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelSubject;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.TextBox textBoxKey;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.MenuItem menuItemCopy;
		private System.Windows.Forms.MenuItem menuItemDel;
		private System.ComponentModel.IContainer components;

		#region Fields
		private ITwinThreadControl threadCtrl; 
		private ReadOnlyResSetCollection resSetCollection;
		private System.Windows.Forms.MenuItem menuItemJump;
		private System.Windows.Forms.MenuItem menuItem2;
		private SortedValueCollection<int> sirusiCollection;
		#endregion

		#region Properties
		
		#endregion

		/// <summary>
		/// SirusiDialog クラスのインスタンスを初期化
		/// </summary>
		public SirusiDialog(ITwinThreadControl thread)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			threadCtrl = thread;
			resSetCollection = thread.Control.ResSets;
			sirusiCollection = thread.HeaderInfo.Sirusi;
			labelSubject.Text =thread.HeaderInfo.Subject;
		}

		#region Auto Generated Code
		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.buttonClose = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.labelSubject = new System.Windows.Forms.Label();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemCopy = new System.Windows.Forms.MenuItem();
			this.menuItemJump = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemDel = new System.Windows.Forms.MenuItem();
			this.treeView = new System.Windows.Forms.TreeView();
			this.textBoxKey = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.AutoSize = true;
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClose.Location = new System.Drawing.Point(118, 169);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(109, 21);
			this.buttonClose.TabIndex = 1;
			this.buttonClose.Text = "閉じる";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(29, 8);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "スレッド:";
			// 
			// labelSubject
			// 
			this.labelSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelSubject.AutoSize = true;
			this.labelSubject.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelSubject.Location = new System.Drawing.Point(97, 8);
			this.labelSubject.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelSubject.Name = "labelSubject";
			this.labelSubject.Size = new System.Drawing.Size(197, 12);
			this.labelSubject.TabIndex = 3;
			this.labelSubject.Text = "********************************";
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCopy,
            this.menuItemJump,
            this.menuItem2,
            this.menuItemDel});
			this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
			// 
			// menuItemCopy
			// 
			this.menuItemCopy.Index = 0;
			this.menuItemCopy.Text = "内容をクリップボードにコピー(&C)";
			this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
			// 
			// menuItemJump
			// 
			this.menuItemJump.Index = 1;
			this.menuItemJump.Text = "このレスにジャンプ(&J)";
			this.menuItemJump.Click += new System.EventHandler(this.menuItemJump_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.Text = "-";
			// 
			// menuItemDel
			// 
			this.menuItemDel.Index = 3;
			this.menuItemDel.Text = "このしるしを削除(&D)";
			this.menuItemDel.Click += new System.EventHandler(this.menuItemDel_Click);
			// 
			// treeView
			// 
			this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.treeView.ContextMenu = this.contextMenu;
			this.treeView.FullRowSelect = true;
			this.treeView.HideSelection = false;
			this.treeView.HotTracking = true;
			this.treeView.Location = new System.Drawing.Point(8, 52);
			this.treeView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(359, 114);
			this.treeView.TabIndex = 0;
			this.treeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseMove);
			this.treeView.MouseHover += new System.EventHandler(this.treeView_MouseHover);
			this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
			// 
			// textBoxKey
			// 
			this.textBoxKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxKey.Location = new System.Drawing.Point(97, 24);
			this.textBoxKey.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBoxKey.Name = "textBoxKey";
			this.textBoxKey.Size = new System.Drawing.Size(220, 19);
			this.textBoxKey.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(4, 28);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "検索キーワード";
			// 
			// buttonSearch
			// 
			this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSearch.AutoSize = true;
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonSearch.Location = new System.Drawing.Point(320, 24);
			this.buttonSearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(46, 21);
			this.buttonSearch.TabIndex = 6;
			this.buttonSearch.Text = "検索";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 30000;
			this.toolTip.InitialDelay = 500;
			this.toolTip.ReshowDelay = 100;
			// 
			// SirusiDialog
			// 
			this.AcceptButton = this.buttonClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(374, 194);
			this.Controls.Add(this.buttonSearch);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxKey);
			this.Controls.Add(this.treeView);
			this.Controls.Add(this.labelSubject);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonClose);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "SirusiDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "しるしマネージャ";
			this.Load += new System.EventHandler(this.SirusiDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Methods
		private void Search(TreeNodeCollection collection, ISearchable searcher)
		{
			foreach (TreeNode node in collection)
			{
				ResSet res = (ResSet)node.Tag;
				string text = res.ToString(ResSetElement.All);

				if (searcher.Search(text) >= 0)
				{
					node.BackColor = SystemColors.Highlight;
					node.ForeColor = SystemColors.HighlightText;
				}
				else {
					node.BackColor = treeView.BackColor;
					node.ForeColor = treeView.ForeColor;
				}

				//Search(node.Nodes, searcher);
			}
		}

		private void AddSirusi(TreeNode parent, ResSet resSet)
		{
			string text = resSet.Index.ToString() + ": " + resSet.Name;

			// ノードを作成
			TreeNode node = new TreeNode(text);
			node.Tag = resSet;

			if (parent != null)
			{
				parent.Nodes.Add(node);
			}
			else {
				treeView.Nodes.Add(node);
			}
		}
		#endregion

		#region Event Handlers
		private void SirusiDialog_Load(object sender, System.EventArgs e)
		{
			// 印と一致したレスをツリーに追加
			foreach (ResSet resSet in resSetCollection)
			{
				if (sirusiCollection.Contains(resSet.Index))
					AddSirusi(null, resSet);
			}

			// 印を参照しているレスを子として追加
			foreach (ResSet resSet in resSetCollection)
			{
				foreach (TreeNode sirusi in treeView.Nodes)
				{
					int[] reference = resSet.RefIndices;
					Array.Sort(reference);

					if (Array.BinarySearch(reference, ((ResSet)sirusi.Tag).Index) >= 0)
						AddSirusi(sirusi, resSet);
				}
			}
		}

		private void menuItemCopy_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.SelectedNode;

			if (node != null)
			{
				ResSet res = (ResSet)node.Tag;
				Clipboard.SetDataObject(res.ToString(new PlainTextSkin()), true);
			}
		}
		
		private void menuItemJump_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.SelectedNode;

			if (node != null)
			{
				ResSet res = (ResSet)node.Tag;
				threadCtrl.Control.ScrollTo(res.Index);
			}
		}

		private void menuItemDel_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.SelectedNode;

			if (node != null)
			{
				ThreadHeader h = threadCtrl.HeaderInfo;
				ResSet res = (ResSet)node.Tag;

				if (h.Sirusi.Contains(res.Index))
				{
					threadCtrl.Control.Sirusi(res.Index, true);
					node.Remove();
				}
			}
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			if (textBoxKey.Text == String.Empty)
			{
				MessageBox.Show("キーワードが入力されていません");
			}
			else {
				Search(treeView.Nodes, new BmSearch2(textBoxKey.Text));
			}
		}

		private void treeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode node = treeView.GetNodeAt(e.X, e.Y);
				if (node != null) treeView.SelectedNode = node;
			}
		}

		private void contextMenu_Popup(object sender, System.EventArgs e)
		{
			menuItemDel.Enabled = false;

			TreeNode node = treeView.SelectedNode;

			if (node != null)
			{
				ThreadHeader h = threadCtrl.HeaderInfo;
				ResSet res = (ResSet)node.Tag;

				if (h.Sirusi.Contains(res.Index))
					menuItemDel.Enabled = true;
			}
		}

		private void treeView_MouseHover(object sender, System.EventArgs e)
		{
		}

		private void treeView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point pt = treeView.PointToClient(MousePosition);
			TreeNode node = treeView.GetNodeAt(pt.X, pt.Y);

			if (node != null)
			{
				if (node != tempNode)
				{
					ResSet res = (ResSet)node.Tag;
					toolTip.SetToolTip(treeView, res.ToString(new PlainTextSkin()));
					tempNode = node;
				}
			}
			else {
				toolTip.RemoveAll();
				tempNode = null;
			}
		}
		private TreeNode tempNode = null;
		#endregion

	}
}
