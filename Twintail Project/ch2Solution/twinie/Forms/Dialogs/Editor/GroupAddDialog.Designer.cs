namespace Twin.Forms
{
	partial class GroupAddDialog
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonUncheckAll = new System.Windows.Forms.Button();
			this.buttonCheckAll = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxGroupName = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(344, 268);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(93, 23);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "キャンセル(&C)";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonUncheckAll
			// 
			this.buttonUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonUncheckAll.AutoSize = true;
			this.buttonUncheckAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonUncheckAll.Location = new System.Drawing.Point(140, 268);
			this.buttonUncheckAll.Name = "buttonUncheckAll";
			this.buttonUncheckAll.Size = new System.Drawing.Size(92, 23);
			this.buttonUncheckAll.TabIndex = 7;
			this.buttonUncheckAll.Text = "すべて解除(&U)";
			this.buttonUncheckAll.UseVisualStyleBackColor = true;
			this.buttonUncheckAll.Click += new System.EventHandler(this.buttonUncheckAll_Click);
			// 
			// buttonCheckAll
			// 
			this.buttonCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCheckAll.AutoSize = true;
			this.buttonCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCheckAll.Location = new System.Drawing.Point(41, 268);
			this.buttonCheckAll.Name = "buttonCheckAll";
			this.buttonCheckAll.Size = new System.Drawing.Size(92, 23);
			this.buttonCheckAll.TabIndex = 6;
			this.buttonCheckAll.Text = "すべて選択(&F)";
			this.buttonCheckAll.UseVisualStyleBackColor = true;
			this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(244, 268);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(93, 23);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "追加(&S)";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.Location = new System.Drawing.Point(12, 70);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(426, 172);
			this.checkedListBox1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(10, 53);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(105, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "追加されるスレッド(&L)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(12, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(201, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "追加先のグループ名を入力してください(&N)";
			// 
			// comboBoxGroupName
			// 
			this.comboBoxGroupName.FormattingEnabled = true;
			this.comboBoxGroupName.Location = new System.Drawing.Point(12, 22);
			this.comboBoxGroupName.Name = "comboBoxGroupName";
			this.comboBoxGroupName.Size = new System.Drawing.Size(220, 20);
			this.comboBoxGroupName.TabIndex = 1;
			// 
			// GroupAddDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(449, 298);
			this.Controls.Add(this.comboBoxGroupName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkedListBox1);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCheckAll);
			this.Controls.Add(this.buttonUncheckAll);
			this.Controls.Add(this.buttonCancel);
			this.MaximizeBox = false;
			this.Name = "GroupAddDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "グループに追加";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonUncheckAll;
		private System.Windows.Forms.Button buttonCheckAll;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBoxGroupName;
	}
}