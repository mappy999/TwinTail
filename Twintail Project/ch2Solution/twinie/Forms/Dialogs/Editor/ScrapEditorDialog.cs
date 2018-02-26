// ScrapEditorDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.IO;
	using System.Text;
	using Twin.Util;

	/// <summary>
	/// スクラップを編集するダイアログ
	/// </summary>
	public class ScrapEditorDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TextBox textBox;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ToolBarButton toolBarButtonNew;
		private System.Windows.Forms.ToolBarButton toolBarButtonSave;
		private System.Windows.Forms.ToolBarButton toolBarButtonAppend;
		private System.Windows.Forms.ToolBarButton toolBarButtonCopy;
		private System.Windows.Forms.ToolBarButton toolBarButtonPaste;
		private System.Windows.Forms.ToolBarButton toolBarButtonDel;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		#endregion

		private string folderPath;
		private TreeNode select;

		/// <summary>
		/// ScrapEditorDialogクラスのインスタンスを初期化
		/// </summary>
		public ScrapEditorDialog(string folderPath)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.folderPath = folderPath;

			string[] fileNames = Directory.GetFiles(folderPath);

			foreach (string filePath in fileNames)
			{
				TreeNode node = new TreeNode();
				node.Text = Path.GetFileName(filePath);
				node.Tag = filePath;

				treeView.Nodes.Add(node);
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrapEditorDialog));
			this.treeView = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.textBox = new System.Windows.Forms.TextBox();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonNew = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAppend = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCopy = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPaste = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonDel = new System.Windows.Forms.ToolBarButton();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView.FullRowSelect = true;
			this.treeView.HideSelection = false;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new System.Drawing.Point(0, 26);
			this.treeView.Margin = new System.Windows.Forms.Padding(2);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.ShowLines = false;
			this.treeView.ShowPlusMinus = false;
			this.treeView.ShowRootLines = false;
			this.treeView.Size = new System.Drawing.Size(152, 304);
			this.treeView.Sorted = true;
			this.treeView.TabIndex = 0;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
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
			this.imageList.Images.SetKeyName(5, "");
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(152, 26);
			this.splitter1.Margin = new System.Windows.Forms.Padding(2);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 304);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// textBox
			// 
			this.textBox.AcceptsReturn = true;
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point(155, 26);
			this.textBox.Margin = new System.Windows.Forms.Padding(2);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox.Size = new System.Drawing.Size(363, 304);
			this.textBox.TabIndex = 2;
			this.textBox.WordWrap = false;
			// 
			// toolBar
			// 
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonNew,
            this.toolBarButtonSave,
            this.toolBarButtonAppend,
            this.toolBarButton1,
            this.toolBarButtonCopy,
            this.toolBarButtonPaste,
            this.toolBarButtonDel});
			this.toolBar.ButtonSize = new System.Drawing.Size(22, 22);
			this.toolBar.Divider = false;
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Margin = new System.Windows.Forms.Padding(2);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(518, 26);
			this.toolBar.TabIndex = 3;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// toolBarButtonNew
			// 
			this.toolBarButtonNew.ImageIndex = 0;
			this.toolBarButtonNew.Name = "toolBarButtonNew";
			// 
			// toolBarButtonSave
			// 
			this.toolBarButtonSave.ImageIndex = 1;
			this.toolBarButtonSave.Name = "toolBarButtonSave";
			// 
			// toolBarButtonAppend
			// 
			this.toolBarButtonAppend.ImageIndex = 3;
			this.toolBarButtonAppend.Name = "toolBarButtonAppend";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Name = "toolBarButton1";
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonCopy
			// 
			this.toolBarButtonCopy.ImageIndex = 4;
			this.toolBarButtonCopy.Name = "toolBarButtonCopy";
			// 
			// toolBarButtonPaste
			// 
			this.toolBarButtonPaste.ImageIndex = 5;
			this.toolBarButtonPaste.Name = "toolBarButtonPaste";
			// 
			// toolBarButtonDel
			// 
			this.toolBarButtonDel.ImageIndex = 2;
			this.toolBarButtonDel.Name = "toolBarButtonDel";
			// 
			// ScrapEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(518, 330);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.treeView);
			this.Controls.Add(this.toolBar);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "ScrapEditorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "メモを編集";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			string filePath = (string)e.Node.Tag;
			textBox.Text = FileUtility.ReadToEnd(filePath);
			select = e.Node;
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolBarButtonNew)
			{
				textBox.Clear();
				select = null;
			}
			else if (e.Button == toolBarButtonSave)
			{
				OverWrite();
			}
			else if (e.Button == toolBarButtonAppend)
			{
				Append();
			}
			else if (e.Button == toolBarButtonCopy)
			{
				textBox.Copy();
			}
			else if (e.Button == toolBarButtonPaste)
			{
				textBox.Paste();
			}
			else if (e.Button == toolBarButtonDel)
			{
				DeleteSel();
			}
		}

		/// <summary>
		/// 選択されている項目を削除
		/// </summary>
		private void DeleteSel()
		{
			if (treeView.SelectedNode != null)
			{
				string filePath = (string)treeView.SelectedNode.Tag;
				File.Delete(filePath);

				treeView.SelectedNode.Remove();
				select = null;
			}
		}

		/// <summary>
		/// 現在の内容を保存
		/// </summary>
		private void Append()
		{
			FileNameEditorDialog dlg = new FileNameEditorDialog();
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				string filePath =
					Path.Combine(folderPath, dlg.FileName);

				TreeNode node = new TreeNode(dlg.FileName);
				node.Tag = filePath;
				treeView.Nodes.Add(node);
				treeView.SelectedNode = node;

				FileUtility.Write(filePath, textBox.Text, false);
			}
		}

		/// <summary>
		/// 現在の内容を上書き保存
		/// </summary>
		private void OverWrite()
		{
			if (select != null)
			{
				string filePath = (string)select.Tag;
				FileUtility.Write(filePath, textBox.Text, false);
			}
			else {
				Append();
			}
		}
	}

	/// <summary>
	/// スクラップをファイルに保存するクラス
	/// </summary>
	public class Scrap
	{
		/// <summary>
		/// 指定したテキストをファイルに保存
		/// </summary>
		/// <param name="text"></param>
		public static void Save(string text)
		{
			if (text != String.Empty)
			{
				FileNameEditorDialog dlg = new FileNameEditorDialog();
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					string filePath = Path.Combine(Settings.ScrapFolderPath, dlg.FileName);
					FileUtility.Write(filePath, text, false);
				}
			}
		}
	}
}
