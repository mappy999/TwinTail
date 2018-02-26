// ThreadUpdateChecker.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.IO;
	using Twin.Tools;
	using CSharpSamples;

	/// <summary>
	/// ����X�V�`�F�b�J�[
	/// </summary>
	public class ThreadUpdateChecker : System.Windows.Forms.Form
	{
		private CheckOnlyPatroller patroller;
		private List<ThreadHeader> items;
		private Twin2IeBrowser twinform;
		private Timer timer;

		private bool noClosing = false;
		private bool patroling = false;

		#region Designer Fields
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.ColumnHeader columnHeaderSubj;
		private System.Windows.Forms.ColumnHeader columnHeaderResCount;
		private System.Windows.Forms.ColumnHeader columnHeaderNewRes;
		private System.Windows.Forms.ToolBarButton toolBarButtonCheck;
		private System.Windows.Forms.ToolBarButton toolBarButtonDel;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep;
		private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
		private System.Windows.Forms.ToolBarButton toolBarButtonTimer;
		private System.Windows.Forms.ColumnHeader columnHeaderGotCount;
		private System.ComponentModel.IContainer components;
		#endregion

		/// <summary>
		/// ����X�V�̊Ԋu���~���b�P�ʂŎ擾�܂��͐ݒ�
		/// </summary>
		public int Interval {
			set {
				if (timer.Interval != value)
					timer.Interval = value;
			}
			get {
				return timer.Interval;
			}
		}

		/// <summary>
		/// �^�C�}�[���L�����ǂ����������l���擾�܂��͐ݒ�
		/// </summary>
		public bool TimerEnabled {
			set {
				if (timer.Enabled != value)
					timer.Enabled = false;
			}
			get {
				return timer.Enabled;
			}
		}

		/// <summary>
		/// �E�C���h�E����Ȃ��悤�ɂ��邩�ǂ����������l���擾�܂��͐ݒ�
		/// </summary>
		public bool NoClosing {
			set {
				if (noClosing != value)
					noClosing = value;
			}
			get { return noClosing; }
		}

		/// <summary>
		/// ThreadUpdateChecker�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadUpdateChecker(Twin2IeBrowser form, Cache cache)
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			items = new List<ThreadHeader>();
			Owner = form;
			twinform = form;

			patroller = new CheckOnlyPatroller(cache);
			patroller.Patroling += new PatrolEventHandler(OnPatroling);
			patroller.Updated += new PatrolEventHandler(OnUpdated);

			timer = new Timer();
			timer.Interval = 150000;	// �����l��15��
			timer.Tick += new EventHandler(OnTick);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreadUpdateChecker));
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonCheck = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonTimer = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonDel = new System.Windows.Forms.ToolBarButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.listView = new System.Windows.Forms.ListView();
			this.columnHeaderSubj = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderResCount = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderGotCount = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderNewRes = new System.Windows.Forms.ColumnHeader();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// toolBar
			// 
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonCheck,
            this.toolBarButtonTimer,
            this.toolBarButtonSep,
            this.toolBarButtonOpen,
            this.toolBarButtonDel});
			this.toolBar.Divider = false;
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(338, 26);
			this.toolBar.TabIndex = 0;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// toolBarButtonCheck
			// 
			this.toolBarButtonCheck.ImageIndex = 0;
			this.toolBarButtonCheck.Name = "toolBarButtonCheck";
			this.toolBarButtonCheck.ToolTipText = "���܂����X�V�`�F�b�N";
			// 
			// toolBarButtonTimer
			// 
			this.toolBarButtonTimer.ImageIndex = 1;
			this.toolBarButtonTimer.Name = "toolBarButtonTimer";
			this.toolBarButtonTimer.ToolTipText = "����X�V�`�F�b�N�^�C�}�[��On/Off";
			// 
			// toolBarButtonSep
			// 
			this.toolBarButtonSep.Name = "toolBarButtonSep";
			this.toolBarButtonSep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonOpen
			// 
			this.toolBarButtonOpen.ImageIndex = 2;
			this.toolBarButtonOpen.Name = "toolBarButtonOpen";
			this.toolBarButtonOpen.ToolTipText = "�I�����ڂ��J��";
			// 
			// toolBarButtonDel
			// 
			this.toolBarButtonDel.ImageIndex = 3;
			this.toolBarButtonDel.Name = "toolBarButtonDel";
			this.toolBarButtonDel.ToolTipText = "�I�����ڂ��X�V�Ώۂ���폜";
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			this.imageList.Images.SetKeyName(3, "");
			this.imageList.Images.SetKeyName(4, "");
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSubj,
            this.columnHeaderResCount,
            this.columnHeaderGotCount,
            this.columnHeaderNewRes});
			this.listView.ContextMenu = this.contextMenu;
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.FullRowSelect = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 26);
			this.listView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(338, 155);
			this.listView.TabIndex = 1;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
			// 
			// columnHeaderSubj
			// 
			this.columnHeaderSubj.Text = "�X���b�h��";
			this.columnHeaderSubj.Width = 160;
			// 
			// columnHeaderResCount
			// 
			this.columnHeaderResCount.Text = "���X��";
			this.columnHeaderResCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeaderResCount.Width = 50;
			// 
			// columnHeaderGotCount
			// 
			this.columnHeaderGotCount.Text = "����";
			this.columnHeaderGotCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeaderGotCount.Width = 50;
			// 
			// columnHeaderNewRes
			// 
			this.columnHeaderNewRes.Text = "�V��";
			this.columnHeaderNewRes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeaderNewRes.Width = 50;
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "�X���b�h���J��(&O)";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "�X�V�Ώۂ���폜(&D)";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// ThreadUpdateChecker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(338, 181);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.toolBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThreadUpdateChecker";
			this.Text = "�X���b�h����X�V�`�F�b�J�[";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ThreadUpdateChecker_Closing);
			this.Load += new System.EventHandler(this.ThreadUpdateChecker_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Private
		private void OnTick(object sender, EventArgs e)
		{
			timer.Stop();
			Check();
		}

		private void Callback(IAsyncResult ar)
		{
			patroller.EndPatrol(ar);
			patroling = false;

			if (File.Exists(Twinie.Sound.Update))
			{
				System.Media.SoundPlayer p = new System.Media.SoundPlayer(Twinie.Sound.Update);
				p.Play();
				p.Dispose();
			}
			timer.Start();
			Invoke(new MethodInvoker(UpdateToolBar));
		}

		private void OnPatroling(object sender, PatrolEventArgs e)
		{
			twinform.BookmarkPatrol_Patroling(sender, e);
		}

		private void OnUpdated(object sender, PatrolEventArgs e)
		{
			Invoke(new PatrolEventHandler(OnUpdatedInternal), new object[] {sender, e});
		}

		private void OnUpdatedInternal(object sender, PatrolEventArgs e)
		{
			int index = items.IndexOf(e.HeaderInfo);
			if (index >= 0)
			{
				ListViewItem item = listView.Items[index];
				item.SubItems[1].Text = e.HeaderInfo.ResCount.ToString();
				item.SubItems[2].Text = e.HeaderInfo.GotResCount.ToString();
				item.SubItems[3].Text = e.HeaderInfo.SubNewResCount.ToString();
			}
		}

		private void ThreadUpdateChecker_Load(object sender, System.EventArgs e)
		{
			timer.Start();
		}

		private void ThreadUpdateChecker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (noClosing)
			{
				Hide();
				e.Cancel = true;
			}
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonCheck)
			{
				Check();
			}
			else if (e.Button == toolBarButtonTimer)
			{
				timer.Enabled = !timer.Enabled;
				UpdateToolBar();
			}
			else if (e.Button == toolBarButtonOpen)
			{
				if (listView.SelectedItems.Count > 0)
				{
					ListViewItem item = listView.SelectedItems[0];
					ThreadHeader h = (ThreadHeader)item.Tag;

					twinform.ThreadOpen(h, true);
				}
			}
			else if (e.Button == toolBarButtonDel)
			{
				if (listView.SelectedItems.Count > 0)
				{
					ListViewItem item = listView.SelectedItems[0];
					Remove((ThreadHeader)item.Tag);
				}
			}
		}

		private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			if (listView.SelectedItems.Count > 0)
			{
				ThreadHeader h = (ThreadHeader)listView.SelectedItems[0].Tag;
				twinform.ThreadOpen(h, true);
			}
		}

		// Open
		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem item in listView.SelectedItems)
			{
				ThreadHeader h = (ThreadHeader)item.Tag;
				twinform.ThreadOpen(h, true);
			}
		}

		// Del
		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem item in listView.SelectedItems)
			{
				ThreadHeader h = (ThreadHeader)item.Tag;
				Remove(h);
			}
		}

		private void UpdateToolBar()
		{
			toolBarButtonCheck.Enabled = !patroling;
			toolBarButtonTimer.Pushed = timer.Enabled;
		}
		#endregion

		/// <summary>
		/// �������X�V�`�F�b�N
		/// </summary>
		public void Check()
		{
			if (items.Count > 0 && !patroling)
			{
				patroling = true;
				patroller.SetItems(items);
				patroller.BeginPatrol(new AsyncCallback(Callback), null);
				UpdateToolBar();
			}
		}

		/// <summary>
		/// �w�肵���X���b�h������X�V�ΏۂɊ܂܂�Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool IsContains(ThreadHeader header)
		{
			return items.Contains(header);
		}

		/// <summary>
		/// ���X�g�ɑ��݂��Ȃ���Βǉ��A���ɑ��݂�����폜
		/// </summary>
		/// <param name="header"></param>
		public void AddOrRemove(ThreadHeader header)
		{
			if (items.Contains(header))
			{
				Remove(header);
			}
			else {
				Add(header);
			}
		}

		/// <summary>
		/// �w�肵���X���b�h�����X�V�`�F�b�N�ɒǉ�
		/// </summary>
		/// <param name="header"></param>
		public void Add(ThreadHeader header)
		{
			if (! items.Contains(header))
			{
				ListViewItem item = new ListViewItem();
				item.Text = header.Subject;
				item.SubItems.Add(header.ResCount.ToString());
				item.SubItems.Add(header.GotResCount.ToString());
				item.SubItems.Add(String.Empty);
				item.Tag = header;

				listView.Items.Add(item);
				items.Add(header);
			}
			if (! timer.Enabled)
				timer.Start();

			UpdateToolBar();
		}

		/// <summary>
		/// �w�肵���X���b�h�����X�V�`�F�b�N����O��
		/// </summary>
		/// <param name="header"></param>
		public void Remove(ThreadHeader header)
		{
			int index = items.IndexOf(header);
			if (index >= 0)
			{
				listView.Items.RemoveAt(index);
				items.Remove(header);
			}
			if (items.Count == 0)
				timer.Stop();

			UpdateToolBar();
		}

		/// <summary>
		/// ���ׂẴX���b�h�����X�V�`�F�b�N����O��
		/// </summary>
		public void RemoveAll()
		{
			timer.Stop();
			items.Clear();
			listView.Items.Clear();
			UpdateToolBar();
		}
	}
}
