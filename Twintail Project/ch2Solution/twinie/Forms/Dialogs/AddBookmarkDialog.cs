// AddBookmarkDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using Twin;

	/// <summary>
	/// ���C�ɓ���o�^���
	/// </summary>
	public class AddBookmarkDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.TreeView treeViewBookmarks;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelFolderName;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ImageList imageListIco;
		private System.Windows.Forms.Label labelIcon;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxSubject;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageBookmarks;
		private System.Windows.Forms.TabPage tabPageWarehouse;
		private System.Windows.Forms.TreeView treeViewWarehouse;
		#endregion

		private static bool bookmarkSelected = true;
		private static BookmarkFolder lastSelectedFolder = null;

		private Settings settings;
		private BookmarkView bookmarks;
		private BookmarkView warehouse;
		private ThreadHeader headerInfo;

		private BookmarkFolder selFolder;
		private Button buttonNewFolder;
		private BookmarkThread newEntry;

		/// <summary>
		/// �I������Ă���t�H���_���擾�܂��͐ݒ�
		/// </summary>
		public BookmarkFolder SelectedFolder {
			set {
				selFolder = value;
			}
			get {
				return selFolder;
			}
		}

		/// <summary>
		/// �V�����ǉ����ꂽ�G���g�����擾
		/// </summary>
		public BookmarkThread NewEntry {
			get {
				return newEntry;
			}
		}

		/// <summary>
		/// AddBookmarkDialog�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="bookmarks">���C�ɓ�����</param>
		/// <param name="warehouse">�ߋ����O���</param>
		/// <param name="header">���C�ɓ���ɓo�^����X���b�h���</param>
		public AddBookmarkDialog(BookmarkView bookmarks, BookmarkView warehouse, ThreadHeader header,
			Settings settings)
		{
			if (bookmarks == null) {
				throw new ArgumentNullException("bookmarks");
			}
			if (warehouse == null) {
				throw new ArgumentNullException("warehouse");
			}
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			this.bookmarks = bookmarks;
			this.warehouse = warehouse;
			this.settings = settings;

			headerInfo = header;
			selFolder = lastSelectedFolder;
			newEntry = null;

			// ���C�ɓ������ݒ�
			textBoxSubject.Text = headerInfo.Subject;

			if (!settings.Dialogs.AddBookmarkDialog_Location.IsEmpty)
			{
				this.StartPosition = FormStartPosition.Manual;
				this.Location = settings.Dialogs.AddBookmarkDialog_Location;
			}
			if (!settings.Dialogs.AddBookmarkDialog_Size.IsEmpty)
			{
				this.Size = settings.Dialogs.AddBookmarkDialog_Size;
			}

			this.Closed += new EventHandler(AddBookmarkDialog_Closed); 
		}

		void AddBookmarkDialog_Closed(object sender, EventArgs e)
		{
			settings.Dialogs.AddBookmarkDialog_Size = this.Size;
			settings.Dialogs.AddBookmarkDialog_Location = this.Location;
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddBookmarkDialog));
			this.treeViewBookmarks = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.buttonOK = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.labelFolderName = new System.Windows.Forms.Label();
			this.imageListIco = new System.Windows.Forms.ImageList(this.components);
			this.labelIcon = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxSubject = new System.Windows.Forms.TextBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageBookmarks = new System.Windows.Forms.TabPage();
			this.tabPageWarehouse = new System.Windows.Forms.TabPage();
			this.treeViewWarehouse = new System.Windows.Forms.TreeView();
			this.buttonNewFolder = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.tabPageBookmarks.SuspendLayout();
			this.tabPageWarehouse.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeViewBookmarks
			// 
			this.treeViewBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewBookmarks.HideSelection = false;
			this.treeViewBookmarks.HotTracking = true;
			this.treeViewBookmarks.ImageIndex = 0;
			this.treeViewBookmarks.ImageList = this.imageList;
			this.treeViewBookmarks.Location = new System.Drawing.Point(0, 0);
			this.treeViewBookmarks.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.treeViewBookmarks.Name = "treeViewBookmarks";
			this.treeViewBookmarks.SelectedImageIndex = 1;
			this.treeViewBookmarks.Size = new System.Drawing.Size(330, 111);
			this.treeViewBookmarks.TabIndex = 0;
			this.treeViewBookmarks.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeViewBookmarks.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			this.treeViewBookmarks.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(250, 28);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(92, 21);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "�ǉ�";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(13, 56);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "�I���t�H���_";
			// 
			// labelFolderName
			// 
			this.labelFolderName.AutoSize = true;
			this.labelFolderName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelFolderName.Location = new System.Drawing.Point(88, 56);
			this.labelFolderName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelFolderName.Name = "labelFolderName";
			this.labelFolderName.Size = new System.Drawing.Size(125, 12);
			this.labelFolderName.TabIndex = 6;
			this.labelFolderName.Text = "####################";
			// 
			// imageListIco
			// 
			this.imageListIco.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIco.ImageStream")));
			this.imageListIco.TransparentColor = System.Drawing.Color.Magenta;
			this.imageListIco.Images.SetKeyName(0, "");
			// 
			// labelIcon
			// 
			this.labelIcon.ImageIndex = 0;
			this.labelIcon.ImageList = this.imageListIco;
			this.labelIcon.Location = new System.Drawing.Point(0, 4);
			this.labelIcon.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelIcon.Name = "labelIcon";
			this.labelIcon.Size = new System.Drawing.Size(42, 36);
			this.labelIcon.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(50, 8);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(205, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "�ȉ��̃X���b�h�����C�ɓ���ɒǉ�����܂�";
			// 
			// textBoxSubject
			// 
			this.textBoxSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSubject.Location = new System.Drawing.Point(50, 28);
			this.textBoxSubject.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBoxSubject.Name = "textBoxSubject";
			this.textBoxSubject.ReadOnly = true;
			this.textBoxSubject.Size = new System.Drawing.Size(192, 19);
			this.textBoxSubject.TabIndex = 2;
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageBookmarks);
			this.tabControl.Controls.Add(this.tabPageWarehouse);
			this.tabControl.Location = new System.Drawing.Point(4, 72);
			this.tabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(338, 136);
			this.tabControl.TabIndex = 0;
			// 
			// tabPageBookmarks
			// 
			this.tabPageBookmarks.Controls.Add(this.treeViewBookmarks);
			this.tabPageBookmarks.Location = new System.Drawing.Point(4, 21);
			this.tabPageBookmarks.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabPageBookmarks.Name = "tabPageBookmarks";
			this.tabPageBookmarks.Size = new System.Drawing.Size(330, 111);
			this.tabPageBookmarks.TabIndex = 0;
			this.tabPageBookmarks.Text = "���C�ɓ���";
			// 
			// tabPageWarehouse
			// 
			this.tabPageWarehouse.Controls.Add(this.treeViewWarehouse);
			this.tabPageWarehouse.Location = new System.Drawing.Point(4, 21);
			this.tabPageWarehouse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabPageWarehouse.Name = "tabPageWarehouse";
			this.tabPageWarehouse.Size = new System.Drawing.Size(330, 111);
			this.tabPageWarehouse.TabIndex = 1;
			this.tabPageWarehouse.Text = "�ߋ����O�q��";
			// 
			// treeViewWarehouse
			// 
			this.treeViewWarehouse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewWarehouse.FullRowSelect = true;
			this.treeViewWarehouse.HideSelection = false;
			this.treeViewWarehouse.HotTracking = true;
			this.treeViewWarehouse.ImageIndex = 0;
			this.treeViewWarehouse.ImageList = this.imageList;
			this.treeViewWarehouse.Location = new System.Drawing.Point(0, 0);
			this.treeViewWarehouse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.treeViewWarehouse.Name = "treeViewWarehouse";
			this.treeViewWarehouse.SelectedImageIndex = 1;
			this.treeViewWarehouse.Size = new System.Drawing.Size(330, 111);
			this.treeViewWarehouse.TabIndex = 0;
			this.treeViewWarehouse.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			this.treeViewWarehouse.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			this.treeViewWarehouse.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpandCollapse);
			// 
			// buttonNewFolder
			// 
			this.buttonNewFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNewFolder.AutoSize = true;
			this.buttonNewFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonNewFolder.Location = new System.Drawing.Point(250, 53);
			this.buttonNewFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonNewFolder.Name = "buttonNewFolder";
			this.buttonNewFolder.Size = new System.Drawing.Size(92, 22);
			this.buttonNewFolder.TabIndex = 4;
			this.buttonNewFolder.Text = "�V�K�t�H���_...";
			this.buttonNewFolder.UseVisualStyleBackColor = true;
			this.buttonNewFolder.Click += new System.EventHandler(this.buttonNewFolder_Click);
			// 
			// AddBookmarkDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(348, 211);
			this.Controls.Add(this.buttonNewFolder);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.textBoxSubject);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelIcon);
			this.Controls.Add(this.labelFolderName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.buttonOK);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddBookmarkDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "���C�ɓ���ɒǉ�";
			this.Load += new System.EventHandler(this.AddBookmarkDialog_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPageBookmarks.ResumeLayout(false);
			this.tabPageWarehouse.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// �ċN�𗘗p���ăt�H���_�݂̂̃c���[�m�[�h���쐬
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="folders"></param>
		private void AppendFolderOnly(TreeView treeView, TreeNodeCollection nodes, BookmarkEntry entry)
		{
			if (entry.IsLeaf)
				return;

			TreeNode node = new TreeNode();
			node.Text = entry.Name;
			node.Tag = entry;
			nodes.Add(node);

			if (((BookmarkFolder)entry).Expanded)
				node.Expand();

			foreach (BookmarkEntry child in entry.Children)
				AppendFolderOnly(treeView, node.Nodes, child);

			if (entry.Equals(selFolder))
			{
				node.EnsureVisible();
				treeView.SelectedNode = node;
			}
		}

		// ���C�ɓ���ɒǉ�
		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			BookmarkView view;
			TreeView treeView;

			GetSelectedViewInfo(out view, out treeView);

			// �ǉ����ꂽ���C�ɓ�����
			newEntry = new BookmarkThread(headerInfo);

			// �I�����ꂽ���C�ɓ���t�H���_
			selFolder = (BookmarkFolder)treeView.SelectedNode.Tag;
			selFolder.Children.Add(newEntry);

			// �Ō�ɑI�����ꂽ���C�ɓ���t�H���_���L��
			lastSelectedFolder = selFolder;

			// ���C�ɓ���r���[���X�V
			view.RefreshBookmarks();
			view.OnBookmarkChanged();
		}

		// �t�H���_���I�����ꂽ��t�H���_�������x���ɕ\��
		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Node != null)
				labelFolderName.Text = e.Node.Text;

			buttonOK.Enabled = (e.Node != null);
		}

		private void treeView_AfterExpandCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			BookmarkFolder folder = (BookmarkFolder)e.Node.Tag;
			folder.Expanded = e.Node.IsExpanded;
		}

		private void AddBookmarkDialog_Load(object sender, System.EventArgs e)
		{
			// �c���[�m�[�h���쐬
			AppendFolderOnly(treeViewBookmarks, treeViewBookmarks.Nodes, bookmarks.Root);
			AppendFolderOnly(treeViewWarehouse, treeViewWarehouse.Nodes, warehouse.Root);

			// ���[�g�m�[�h�͏�ɓW�J�����Ƃ�
			treeViewBookmarks.Nodes[0].Expand();
			treeViewWarehouse.Nodes[0].Expand();

			if (bookmarkSelected)	tabControl.SelectedTab = tabPageBookmarks;
			else					tabControl.SelectedTab = tabPageWarehouse;

			// �I���t�H���_���w�肳��Ă��Ȃ���΂��C�ɓ��胋�[�g��I�������Ă���
			if (treeViewBookmarks.SelectedNode == null && treeViewBookmarks.Nodes.Count > 0)
				treeViewBookmarks.SelectedNode = treeViewBookmarks.Nodes[0];

			if (treeViewWarehouse.SelectedNode == null && treeViewWarehouse.Nodes.Count > 0)
				treeViewWarehouse.SelectedNode = treeViewWarehouse.Nodes[0];
		}

		private void buttonNewFolder_Click(object sender, EventArgs e)
		{
			FileNameEditorDialog dlg = new FileNameEditorDialog();
			dlg.Message = "�t�H���_������͂��Ă�������";

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				BookmarkFolder newFolder = new BookmarkFolder(dlg.FileName);

				BookmarkView view;
				TreeView treeView;

				GetSelectedViewInfo(out view, out treeView);

				// �I������Ă��邨�C�ɓ���t�H���_
				selFolder = (BookmarkFolder)treeView.SelectedNode.Tag;
				selFolder.Children.Add(newFolder);
				selFolder = newFolder;

				// �Ō�ɑI�����ꂽ���C�ɓ���t�H���_���L��
				lastSelectedFolder = newFolder;

				// ���C�ɓ���r���[���X�V
				view.RefreshBookmarks();
				view.OnBookmarkChanged();

				// �c���[�m�[�h���쐬
				treeView.Nodes.Clear();
				AppendFolderOnly(treeView, treeView.Nodes, view.Root);
				treeView.ExpandAll();

			}
		}

		private void GetSelectedViewInfo(out BookmarkView view, out TreeView treeView)
		{
			if (tabControl.SelectedTab.Equals(tabPageBookmarks))
			{
				treeView = treeViewBookmarks;
				view = bookmarks;
				bookmarkSelected = true;
			}
			else if (tabControl.SelectedTab.Equals(tabPageWarehouse))
			{
				treeView = treeViewWarehouse;
				view = warehouse;
				bookmarkSelected = false;
			}
			else
			{
				throw new ArgumentException();
			}
		}
	}
}
