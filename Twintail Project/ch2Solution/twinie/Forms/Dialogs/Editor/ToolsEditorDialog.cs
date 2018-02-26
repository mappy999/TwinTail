// ToolsEditorDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using Twin.Tools;

	/// <summary>
	/// ToolsEditorDialog の概要の説明です。
	/// </summary>
	public class ToolsEditorDialog : System.Windows.Forms.Form
	{
		private ToolItemCollection tools;

		#region Designer Fields
		private System.Windows.Forms.ListBox listBoxTools;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonDel;
		private System.Windows.Forms.Button buttonUp;
		private System.Windows.Forms.Button buttonDown;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private Button buttonCopyItem;
		private System.ComponentModel.IContainer components = null;
		#endregion

		/// <summary>
		/// ToolsEditorDialogクラスのインスタンスを初期化
		/// </summary>
		/// <param name="items"></param>
		public ToolsEditorDialog(ToolItemCollection items)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			tools = items;

			foreach (ToolItem item in items)
				listBoxTools.Items.Add(item);

			if (items.Count > 0)
				listBoxTools.SelectedIndex = 0;
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
			this.listBoxTools = new System.Windows.Forms.ListBox();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonDel = new System.Windows.Forms.Button();
			this.buttonUp = new System.Windows.Forms.Button();
			this.buttonDown = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.buttonCopyItem = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listBoxTools
			// 
			this.listBoxTools.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxTools.IntegralHeight = false;
			this.listBoxTools.ItemHeight = 12;
			this.listBoxTools.Location = new System.Drawing.Point(8, 24);
			this.listBoxTools.Margin = new System.Windows.Forms.Padding(2);
			this.listBoxTools.Name = "listBoxTools";
			this.listBoxTools.Size = new System.Drawing.Size(278, 162);
			this.listBoxTools.TabIndex = 1;
			this.listBoxTools.SelectedIndexChanged += new System.EventHandler(this.listBoxTools_SelectedIndexChanged);
			this.listBoxTools.DoubleClick += new System.EventHandler(this.listBoxTools_DoubleClick);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAdd.AutoSize = true;
			this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAdd.Location = new System.Drawing.Point(290, 24);
			this.buttonAdd.Margin = new System.Windows.Forms.Padding(2);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(80, 21);
			this.buttonAdd.TabIndex = 2;
			this.buttonAdd.Text = "登録(&R)...";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonDel
			// 
			this.buttonDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDel.AutoSize = true;
			this.buttonDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonDel.Location = new System.Drawing.Point(290, 94);
			this.buttonDel.Margin = new System.Windows.Forms.Padding(2);
			this.buttonDel.Name = "buttonDel";
			this.buttonDel.Size = new System.Drawing.Size(80, 21);
			this.buttonDel.TabIndex = 4;
			this.buttonDel.Text = "削除(&D)";
			this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
			// 
			// buttonUp
			// 
			this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonUp.AutoSize = true;
			this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonUp.Location = new System.Drawing.Point(290, 131);
			this.buttonUp.Margin = new System.Windows.Forms.Padding(2);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(34, 21);
			this.buttonUp.TabIndex = 5;
			this.buttonUp.Text = "↑";
			this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
			// 
			// buttonDown
			// 
			this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDown.AutoSize = true;
			this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonDown.Location = new System.Drawing.Point(336, 131);
			this.buttonDown.Margin = new System.Windows.Forms.Padding(2);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(34, 21);
			this.buttonDown.TabIndex = 6;
			this.buttonDown.Text = "↓";
			this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.AutoSize = true;
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClose.Location = new System.Drawing.Point(290, 165);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(80, 21);
			this.buttonClose.TabIndex = 7;
			this.buttonClose.Text = "閉じる(&C)";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 4);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(169, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "登録されている外部ツール一覧(&A)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "すべてのファイル (*.*)|*.*|実行ファイル (*.exe;*.bat)|*.exe;*.bat";
			// 
			// buttonCopyItem
			// 
			this.buttonCopyItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCopyItem.Location = new System.Drawing.Point(291, 50);
			this.buttonCopyItem.Name = "buttonCopyItem";
			this.buttonCopyItem.Size = new System.Drawing.Size(79, 23);
			this.buttonCopyItem.TabIndex = 3;
			this.buttonCopyItem.Text = "複製(&C)";
			this.buttonCopyItem.UseVisualStyleBackColor = true;
			this.buttonCopyItem.Click += new System.EventHandler(this.buttonCopyItem_Click);
			// 
			// ToolsEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(387, 197);
			this.Controls.Add(this.buttonCopyItem);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.buttonDown);
			this.Controls.Add(this.buttonUp);
			this.Controls.Add(this.buttonDel);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.listBoxTools);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ToolsEditorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "外部ツールの登録";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ToolsEditorDialog_Closing);
			this.Load += new System.EventHandler(this.ToolsEditorDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void ToolsEditorDialog_Load(object sender, System.EventArgs e)
		{
		
		}

		private void ToolsEditorDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			tools.Clear();

			foreach (ToolItem item in listBoxTools.Items)
				tools.Add(item);
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			ToolRegistDialog dlg = new ToolRegistDialog();
			if (dlg.ShowDialog(this) == DialogResult.OK)
				listBoxTools.SelectedIndex = listBoxTools.Items.Add(dlg.Item);
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void buttonDel_Click(object sender, System.EventArgs e)
		{
			listBoxTools.Items.Remove(listBoxTools.SelectedItem);
		}

		private void buttonUp_Click(object sender, System.EventArgs e)
		{
			int index = listBoxTools.SelectedIndex;
			if (index > 0)
			{
				object obj = listBoxTools.Items[index];
				listBoxTools.Items.Remove(obj);
				listBoxTools.Items.Insert(index - 1, obj);
				listBoxTools.SelectedItem = obj;
			}
		}

		private void buttonDown_Click(object sender, System.EventArgs e)
		{
			int index = listBoxTools.SelectedIndex;
			if (index + 1 < listBoxTools.Items.Count)
			{
				object obj = listBoxTools.Items[index];
				listBoxTools.Items.Remove(obj);
				listBoxTools.Items.Insert(index + 1, obj);
				listBoxTools.SelectedItem = obj;
			}
		}

		private void listBoxTools_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			buttonDel.Enabled = buttonCopyItem.Enabled = listBoxTools.SelectedIndex != -1;
			buttonUp.Enabled = (listBoxTools.SelectedIndex-1) >= 0;
			buttonDown.Enabled = (listBoxTools.SelectedIndex+1) < listBoxTools.Items.Count;
		}

		private void listBoxTools_DoubleClick(object sender, System.EventArgs e)
		{
			ToolRegistDialog dlg = new ToolRegistDialog();
			dlg.Item = (ToolItem)listBoxTools.SelectedItem;

			int index = listBoxTools.SelectedIndex;

			if (dlg.ShowDialog(this) == DialogResult.OK)
				listBoxTools.Items[index] = dlg.Item;
		}

		private void buttonCopyItem_Click(object sender, EventArgs e)
		{
			ToolItem item = (ToolItem)listBoxTools.SelectedItem;
			listBoxTools.SelectedIndex = listBoxTools.Items.Add(new ToolItem(item.Name, item.FileName, item.Parameter));
		}
	}
}
