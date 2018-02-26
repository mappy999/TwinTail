namespace Twin.Forms
{
	partial class PreFolderBrowserDialog
	{
		/// <summary>
		/// 必要なデザイナー変数です。
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

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonRefFolder = new System.Windows.Forms.Button();
			this.comboBoxPath = new System.Windows.Forms.ComboBox();
			this.checkBoxSetDefault = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonRefFolder
			// 
			this.buttonRefFolder.Location = new System.Drawing.Point(279, 46);
			this.buttonRefFolder.Name = "buttonRefFolder";
			this.buttonRefFolder.Size = new System.Drawing.Size(101, 27);
			this.buttonRefFolder.TabIndex = 0;
			this.buttonRefFolder.Text = "フォルダの参照...";
			this.buttonRefFolder.UseVisualStyleBackColor = true;
			this.buttonRefFolder.Click += new System.EventHandler(this.buttonRefFolder_Click);
			// 
			// comboBoxPath
			// 
			this.comboBoxPath.FormattingEnabled = true;
			this.comboBoxPath.Location = new System.Drawing.Point(18, 20);
			this.comboBoxPath.Name = "comboBoxPath";
			this.comboBoxPath.Size = new System.Drawing.Size(362, 20);
			this.comboBoxPath.TabIndex = 2;
			this.comboBoxPath.SelectedIndexChanged += new System.EventHandler(this.comboBoxPath_SelectedIndexChanged);
			// 
			// checkBoxSetDefault
			// 
			this.checkBoxSetDefault.AutoSize = true;
			this.checkBoxSetDefault.Location = new System.Drawing.Point(18, 52);
			this.checkBoxSetDefault.Name = "checkBoxSetDefault";
			this.checkBoxSetDefault.Size = new System.Drawing.Size(216, 16);
			this.checkBoxSetDefault.TabIndex = 3;
			this.checkBoxSetDefault.Text = "選択されたフォルダをデフォルトに指定する";
			this.checkBoxSetDefault.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboBoxPath);
			this.groupBox1.Controls.Add(this.checkBoxSetDefault);
			this.groupBox1.Controls.Add(this.buttonRefFolder);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(393, 83);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "フォルダパス";
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(213, 101);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(93, 26);
			this.buttonOK.TabIndex = 6;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(312, 101);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(93, 26);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// PreFolderBrowserDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(415, 133);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PreFolderBrowserDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "フォルダ名の指定";
			this.Load += new System.EventHandler(this.PreFolderBrowserDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonRefFolder;
		private System.Windows.Forms.ComboBox comboBoxPath;
		private System.Windows.Forms.CheckBox checkBoxSetDefault;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
	}
}