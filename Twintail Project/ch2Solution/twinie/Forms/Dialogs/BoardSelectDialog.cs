// BoardSelectDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// 板の選択ダイアログ
	/// </summary>
	public class BoardSelectDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ImageList imageList;
		private System.ComponentModel.IContainer components;

		private BoardInfo board;

		/// <summary>
		/// 選択された板を取得
		/// </summary>
		public BoardInfo Selected {
			get {
				return board;
			}
		}

		/// <summary>
		/// BoardSelectDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="table"></param>
		public BoardSelectDialog(IBoardTable table)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			board = null;
			buttonOK.Enabled = false;
			TreeViewUtility.SetTable(treeView, table, 0, 1, 2, 3);
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardSelectDialog));
			this.treeView = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.treeView.FullRowSelect = true;
			this.treeView.HotTracking = true;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new System.Drawing.Point(4, 4);
			this.treeView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.ShowLines = false;
			this.treeView.Size = new System.Drawing.Size(174, 234);
			this.treeView.TabIndex = 0;
			this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeView.Click += new System.EventHandler(this.treeView_Click);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			this.imageList.Images.SetKeyName(3, "");
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(93, 241);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(88, 21);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(0, 241);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(88, 21);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// BoardSelectDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(182, 262);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.treeView);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BoardSelectDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "板を選択してください";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			board = (e.Node.Tag as BoardInfo);
			buttonOK.Enabled = (board != null);
		}

		private void treeView_AfterExpandCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			e.Node.ImageIndex =
				e.Node.SelectedImageIndex = (e.Node.IsExpanded) ? 1 : 0;
		}

		private void treeView_Click(object sender, System.EventArgs e)
		{
			TreeNode node = treeView.GetNodeAt(
				treeView.PointToClient(MousePosition));

			if (node != null)
				treeView.SelectedNode = node;
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
