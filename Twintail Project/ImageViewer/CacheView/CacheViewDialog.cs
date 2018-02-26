// CacheViewDialog.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Collections.Generic;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using System.Xml;
	using Twin;

	/// <summary>
	/// キャッシュ一覧ビューア
	/// </summary>
	public class CacheViewDialog : System.Windows.Forms.Form
	{
		private int totalCount;
		private float totalBytes;

		private ImageViewer imageViewer;
		private SortOrder sorting;
		private Thread thread;

		#region Designer Fields
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxUrl;
		private System.Windows.Forms.TextBox textBoxLastmod;
		private System.Windows.Forms.TextBox textBoxHash;
		private System.Windows.Forms.TextBox textBoxLength;
		private System.Windows.Forms.ColumnHeader columnHeaderFileName;
		private System.Windows.Forms.ColumnHeader columnHeaderUrl;
		private System.Windows.Forms.ColumnHeader columnHeaderHash;
		private System.Windows.Forms.ColumnHeader columnHeaderLength;
		private System.Windows.Forms.ColumnHeader columnHeaderLastmod;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel statusBarPanelFileName;
		private System.Windows.Forms.StatusBarPanel statusBarPanelTotalCount;
		private System.Windows.Forms.StatusBarPanel statusBarPanelTotalBytes;
		private System.Windows.Forms.StatusBarPanel statusBarPanelImageSize;
		private System.Windows.Forms.MenuItem menuItemFileClose;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.Windows.Forms.MenuItem menuItemFileRefresh;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemFileOpen;
		#endregion
		private ColumnHeader columnHeaderDisplayed;
		private ColumnHeader columnHeaderGetdate;
		private IContainer components;

		/// <summary>
		/// CacheViewDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="imageViewer"></param>
		public CacheViewDialog(ImageViewer imageViewer)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.imageViewer = imageViewer;
			this.sorting = SortOrder.Ascending;
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
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.listView = new System.Windows.Forms.ListView();
			this.columnHeaderFileName = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderUrl = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderHash = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderLength = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderLastmod = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderGetdate = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderDisplayed = new System.Windows.Forms.ColumnHeader();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemFileRefresh = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemFileClose = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textBoxLength = new System.Windows.Forms.TextBox();
			this.textBoxHash = new System.Windows.Forms.TextBox();
			this.textBoxLastmod = new System.Windows.Forms.TextBox();
			this.textBoxUrl = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarPanelFileName = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanelTotalCount = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanelTotalBytes = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanelImageSize = new System.Windows.Forms.StatusBarPanel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelFileName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelTotalCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelTotalBytes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelImageSize)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox.Location = new System.Drawing.Point(12, 12);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(120, 120);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderUrl,
            this.columnHeaderHash,
            this.columnHeaderLength,
            this.columnHeaderLastmod,
            this.columnHeaderGetdate,
            this.columnHeaderDisplayed});
			this.listView.ContextMenu = this.contextMenu;
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(548, 157);
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
			// 
			// columnHeaderFileName
			// 
			this.columnHeaderFileName.Text = "ファイル名";
			this.columnHeaderFileName.Width = 116;
			// 
			// columnHeaderUrl
			// 
			this.columnHeaderUrl.Text = "URL";
			this.columnHeaderUrl.Width = 137;
			// 
			// columnHeaderHash
			// 
			this.columnHeaderHash.Text = "ハッシュ値";
			this.columnHeaderHash.Width = 119;
			// 
			// columnHeaderLength
			// 
			this.columnHeaderLength.Text = "サイズ";
			this.columnHeaderLength.Width = 58;
			// 
			// columnHeaderLastmod
			// 
			this.columnHeaderLastmod.Text = "最終更新日";
			this.columnHeaderLastmod.Width = 97;
			// 
			// columnHeaderGetdate
			// 
			this.columnHeaderGetdate.DisplayIndex = 6;
			this.columnHeaderGetdate.Text = "取得日";
			this.columnHeaderGetdate.Width = 100;
			// 
			// columnHeaderDisplayed
			// 
			this.columnHeaderDisplayed.DisplayIndex = 5;
			this.columnHeaderDisplayed.Text = "表示";
			this.columnHeaderDisplayed.Width = 100;
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpen});
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Text = "ビューアで開く(&O)";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFileRefresh,
            this.menuItemFileOpen,
            this.menuItem2,
            this.menuItemFileClose});
			this.menuItemFile.Text = "ファイル(&F)";
			// 
			// menuItemFileRefresh
			// 
			this.menuItemFileRefresh.Index = 0;
			this.menuItemFileRefresh.Text = "表示を更新(&R)";
			this.menuItemFileRefresh.Click += new System.EventHandler(this.menuItemFileRefresh_Click);
			// 
			// menuItemFileOpen
			// 
			this.menuItemFileOpen.Index = 1;
			this.menuItemFileOpen.Text = "選択項目をビューアで開く(&O)";
			this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.Text = "-";
			// 
			// menuItemFileClose
			// 
			this.menuItemFileClose.Index = 3;
			this.menuItemFileClose.Text = "閉じる(&X)";
			this.menuItemFileClose.Click += new System.EventHandler(this.menuItemFileClose_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 157);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(548, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.textBoxLength);
			this.panel1.Controls.Add(this.textBoxHash);
			this.panel1.Controls.Add(this.textBoxLastmod);
			this.panel1.Controls.Add(this.textBoxUrl);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.pictureBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 160);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(548, 140);
			this.panel1.TabIndex = 1;
			// 
			// textBoxLength
			// 
			this.textBoxLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLength.BackColor = System.Drawing.SystemColors.Control;
			this.textBoxLength.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxLength.Location = new System.Drawing.Point(208, 104);
			this.textBoxLength.Name = "textBoxLength";
			this.textBoxLength.ReadOnly = true;
			this.textBoxLength.Size = new System.Drawing.Size(312, 12);
			this.textBoxLength.TabIndex = 7;
			this.textBoxLength.WordWrap = false;
			// 
			// textBoxHash
			// 
			this.textBoxHash.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxHash.BackColor = System.Drawing.SystemColors.Control;
			this.textBoxHash.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxHash.Location = new System.Drawing.Point(208, 76);
			this.textBoxHash.Name = "textBoxHash";
			this.textBoxHash.ReadOnly = true;
			this.textBoxHash.Size = new System.Drawing.Size(312, 12);
			this.textBoxHash.TabIndex = 5;
			this.textBoxHash.WordWrap = false;
			// 
			// textBoxLastmod
			// 
			this.textBoxLastmod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLastmod.BackColor = System.Drawing.SystemColors.Control;
			this.textBoxLastmod.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxLastmod.Location = new System.Drawing.Point(208, 48);
			this.textBoxLastmod.Name = "textBoxLastmod";
			this.textBoxLastmod.ReadOnly = true;
			this.textBoxLastmod.Size = new System.Drawing.Size(312, 12);
			this.textBoxLastmod.TabIndex = 3;
			this.textBoxLastmod.WordWrap = false;
			// 
			// textBoxUrl
			// 
			this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUrl.BackColor = System.Drawing.SystemColors.Control;
			this.textBoxUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxUrl.Location = new System.Drawing.Point(208, 20);
			this.textBoxUrl.Name = "textBoxUrl";
			this.textBoxUrl.ReadOnly = true;
			this.textBoxUrl.Size = new System.Drawing.Size(312, 12);
			this.textBoxUrl.TabIndex = 1;
			this.textBoxUrl.WordWrap = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(160, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(28, 12);
			this.label4.TabIndex = 0;
			this.label4.Text = "URL";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(156, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 12);
			this.label3.TabIndex = 6;
			this.label3.Text = "サイズ";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(156, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "ハッシュ値";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(140, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "最終更新日";
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 300);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanelFileName,
            this.statusBarPanelTotalCount,
            this.statusBarPanelTotalBytes,
            this.statusBarPanelImageSize});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(548, 17);
			this.statusBar.TabIndex = 2;
			// 
			// statusBarPanelFileName
			// 
			this.statusBarPanelFileName.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanelFileName.Name = "statusBarPanelFileName";
			this.statusBarPanelFileName.ToolTipText = "ファイル名です";
			this.statusBarPanelFileName.Width = 252;
			// 
			// statusBarPanelTotalCount
			// 
			this.statusBarPanelTotalCount.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.statusBarPanelTotalCount.Name = "statusBarPanelTotalCount";
			this.statusBarPanelTotalCount.ToolTipText = "キャッシュの総ファイル数です";
			this.statusBarPanelTotalCount.Width = 60;
			// 
			// statusBarPanelTotalBytes
			// 
			this.statusBarPanelTotalBytes.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.statusBarPanelTotalBytes.Name = "statusBarPanelTotalBytes";
			this.statusBarPanelTotalBytes.ToolTipText = "キャッシュの総サイズです";
			this.statusBarPanelTotalBytes.Width = 70;
			// 
			// statusBarPanelImageSize
			// 
			this.statusBarPanelImageSize.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusBarPanelImageSize.Name = "statusBarPanelImageSize";
			this.statusBarPanelImageSize.ToolTipText = "イメージの縦幅です";
			this.statusBarPanelImageSize.Width = 150;
			// 
			// CacheViewDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(548, 317);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusBar);
			this.Menu = this.mainMenu;
			this.Name = "CacheViewDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "CacheViewDialog";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.CacheViewDialog_Closing);
			this.Load += new System.EventHandler(this.CacheViewDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelFileName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelTotalCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelTotalBytes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelImageSize)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void RefreshIndices()
		{
			if (thread == null)
			{
				listView.Items.Clear();

				thread = new Thread(new ThreadStart(CacheSearching));
				thread.Name = "CACHE_VIEW_DLG";
				thread.IsBackground = true;
				thread.Priority = ThreadPriority.Lowest;
				thread.Start();

				Text = "キャッシュ一覧を読み込み中です．．．";
			}
		}

		private void CacheViewDialog_Load(object sender, System.EventArgs e)
		{
			RefreshIndices();
		}

		private void CacheViewDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (thread != null && thread.IsAlive)
				thread.Abort();
		}

		private void menuItemFileClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listView.SelectedItems.Count == 1)
			{
				ListViewItem item = listView.SelectedItems[0];
				Size srcSize;

				CacheInfo info = item.Tag as CacheInfo;

				string fileName = Path.Combine(imageViewer.folderPath,
						String.Format("{0:x}.ich", info.Url.GetHashCode()));

				using (FileStream fs = new FileStream(fileName, FileMode.Open))
				{
					if (pictureBox.Image != null)
						pictureBox.Image.Dispose();

					Image image = Image.FromStream(fs);
					srcSize = image.Size;
					
					image = ImageUtil.GetThumbnailImage(image, pictureBox.Size, Color.Transparent);
					pictureBox.Image = image;
				}

				textBoxUrl.Text = info.Url;
				textBoxHash.Text = info.HashCode;
				textBoxLength.Text = info.Length.ToString();
				textBoxLastmod.Text = info.LastModified.ToLongDateString();

				statusBarPanelFileName.Text = item.Text;
				statusBarPanelImageSize.Text = String.Format("Width={0},Height={1}", srcSize.Width, srcSize.Height);
			}
		}

		private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			listView.BeginUpdate();

			ListViewItem[] items = new ListViewItem[listView.Items.Count];
		
			listView.Items.CopyTo(items, 0);
			listView.Items.Clear();

			Array.Sort(items, new ListViewItemComparer(e.Column, sorting));

			listView.Items.AddRange(items);

			sorting = (SortOrder.Ascending == sorting) ?
				SortOrder.Descending : SortOrder.Ascending;

			listView.EndUpdate();
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			if (listView.SelectedItems.Count > 0)
				imageViewer.OpenUrl(listView.SelectedItems[0].SubItems[1].Text, true);
		}

		private void CacheSearching()
		{
			try {
				string filePath = Path.Combine(imageViewer.folderPath, "indices.txt");

				if (! File.Exists(filePath))
					return;

				XmlDocument doc = new XmlDocument();
				doc.Load(filePath);

				totalCount = 0;
				totalBytes = 0;

				List<ListViewItem> lvitems = new List<ListViewItem>();

				foreach (XmlNode node in doc.SelectNodes("Indices/Cache"))
				{
					XmlAttribute url = node.Attributes["Url"];
					XmlAttribute hash = node.Attributes["Hash"];
					XmlAttribute length = node.Attributes["Length"];
					XmlAttribute lastmod = node.Attributes["Lastmod"];

					ListViewItem item = new ListViewItem();
					item.Text = Path.GetFileName(PathUtil.ReplaceInvalidPathChars(url.Value, "_"));
					item.SubItems.Add(url.Value);
					item.SubItems.Add(hash.Value);
					item.SubItems.Add(length.Value);
					item.SubItems.Add(lastmod.Value);
					
					item.Tag = new CacheInfo(url.Value, Int32.Parse(length.Value), hash.Value, 
						DateTime.Parse(lastmod.Value));

					totalCount++;
					totalBytes += Int32.Parse(length.Value);

					lvitems.Add(item);
				}

				ListViewItem[] array = lvitems.ToArray();
				Invoke(new FlushDelegate(WriteListViewItem), new object[] {array});
			}
			finally {
				thread = null;

				Invoke(new MethodInvoker(delegate {Text = "キャッシュ一覧表示完了";}));
			}
		}

		private delegate void FlushDelegate(ListViewItem[] array);

		private void WriteListViewItem(ListViewItem[] array)
		{
			float k,m;
			string totalBytesStr = (k = totalBytes / 1024f) < 1024f ?
				k.ToString("0.0KB") : (m = k / 1024f) < 1024 ?
				m.ToString("0.00MB") : (m / 1024).ToString("0.000GB");

			listView.Items.AddRange(array);

			statusBarPanelTotalCount.Text = totalCount.ToString() + "枚";
			statusBarPanelTotalBytes.Text = totalBytesStr;
		}

		private void menuItemOpen_Click(object sender, System.EventArgs e)
		{
			menuItemFileOpen_Click(null, null);
		}

		private void menuItemFileOpen_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem item in listView.SelectedItems)
				imageViewer.OpenUrl(item.SubItems[1].Text, false);
		}

		private void menuItemFileRefresh_Click(object sender, System.EventArgs e)
		{
			RefreshIndices();
		}

		#region InnerClass ListViewItemComparer
		private class ListViewItemComparer : IComparer
		{
			private SortOrder sorting;
			private int column;

			public ListViewItemComparer(int column, SortOrder sort)
			{
				this.column = column;
				this.sorting = sort;
			}

			public int Compare(object x, object y)
			{
				ListViewItem item_x = (ListViewItem)x, item_y = (ListViewItem)y;
				string strA, strB;

				if (sorting == SortOrder.Ascending)
				{
					strA = item_x.SubItems[column].Text;
					strB = item_y.SubItems[column].Text;
				}
				else {
					strA = item_y.SubItems[column].Text;
					strB = item_x.SubItems[column].Text;
				}

				switch (column)
				{
					// ファイル名、URL、ハッシュ
				case 0:
				case 1:
				case 2:
					return String.Compare(strA, strB, false);

					// ファイルサイズ
				case 3:
					int valA = Int32.Parse(strA), valB = Int32.Parse(strB);
					return (valA - valB);

					// 日付
				case 4:
					DateTime dateA = DateTime.Parse(strA), dateB = DateTime.Parse(strB);
					return DateTime.Compare(dateA, dateB);

				default:
					throw new ApplicationException("カラム値が不正です");
				}
			}
		}
		#endregion

		//private void menuItem3_Click(object sender, EventArgs e)
		//{
		//    DialogResult r = MessageBox.Show(listView.SelectedItems.Count + "個のキャッシュを削除します。よろしいですか？", "削除の確認",
		//        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

		//    if (r == DialogResult.No)
		//        return;

		//    foreach (ListViewItem item in listView.SelectedItems)
		//    {
		//        CacheInfo info = item.Tag as CacheInfo;
		//        imageViewer.imageCache.Delete(info);
		//    }
		//}
	}
}
