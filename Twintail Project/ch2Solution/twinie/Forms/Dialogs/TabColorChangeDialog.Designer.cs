namespace Twin.Forms
{
	partial class TabColorChangeDialog
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.tabControlSample = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.linkLabelBackColor = new System.Windows.Forms.LinkLabel();
			this.linkLabelForeColor = new System.Windows.Forms.LinkLabel();
			this.linkLabelDefault = new System.Windows.Forms.LinkLabel();
			this.linkLabelForeColor2 = new System.Windows.Forms.LinkLabel();
			this.linkLabelBackColor2 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControlSample.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(270, 111);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(270, 82);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// tabControlSample
			// 
			this.tabControlSample.Controls.Add(this.tabPage1);
			this.tabControlSample.Controls.Add(this.tabPage2);
			this.tabControlSample.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControlSample.Location = new System.Drawing.Point(12, 18);
			this.tabControlSample.Name = "tabControlSample";
			this.tabControlSample.SelectedIndex = 0;
			this.tabControlSample.Size = new System.Drawing.Size(320, 20);
			this.tabControlSample.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControlSample.TabIndex = 2;
			this.tabControlSample.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControlSample_DrawItem);
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 21);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
			this.tabPage1.Size = new System.Drawing.Size(312, 0);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 21);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
			this.tabPage2.Size = new System.Drawing.Size(312, 0);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// colorDialog1
			// 
			this.colorDialog1.AnyColor = true;
			this.colorDialog1.FullOpen = true;
			// 
			// linkLabelBackColor
			// 
			this.linkLabelBackColor.AutoSize = true;
			this.linkLabelBackColor.Location = new System.Drawing.Point(24, 115);
			this.linkLabelBackColor.Name = "linkLabelBackColor";
			this.linkLabelBackColor.Size = new System.Drawing.Size(74, 12);
			this.linkLabelBackColor.TabIndex = 6;
			this.linkLabelBackColor.TabStop = true;
			this.linkLabelBackColor.Text = "背景色を変更";
			this.linkLabelBackColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBackColor_LinkClicked);
			// 
			// linkLabelForeColor
			// 
			this.linkLabelForeColor.AutoSize = true;
			this.linkLabelForeColor.Location = new System.Drawing.Point(24, 93);
			this.linkLabelForeColor.Name = "linkLabelForeColor";
			this.linkLabelForeColor.Size = new System.Drawing.Size(74, 12);
			this.linkLabelForeColor.TabIndex = 7;
			this.linkLabelForeColor.TabStop = true;
			this.linkLabelForeColor.Text = "文字色を変更";
			this.linkLabelForeColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForeColor_LinkClicked);
			// 
			// linkLabelDefault
			// 
			this.linkLabelDefault.AutoSize = true;
			this.linkLabelDefault.Location = new System.Drawing.Point(10, 48);
			this.linkLabelDefault.Name = "linkLabelDefault";
			this.linkLabelDefault.Size = new System.Drawing.Size(60, 12);
			this.linkLabelDefault.TabIndex = 8;
			this.linkLabelDefault.TabStop = true;
			this.linkLabelDefault.Text = "標準に戻す";
			this.linkLabelDefault.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDefault_LinkClicked);
			// 
			// linkLabelForeColor2
			// 
			this.linkLabelForeColor2.AutoSize = true;
			this.linkLabelForeColor2.Location = new System.Drawing.Point(128, 93);
			this.linkLabelForeColor2.Name = "linkLabelForeColor2";
			this.linkLabelForeColor2.Size = new System.Drawing.Size(74, 12);
			this.linkLabelForeColor2.TabIndex = 10;
			this.linkLabelForeColor2.TabStop = true;
			this.linkLabelForeColor2.Text = "文字色を変更";
			this.linkLabelForeColor2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForeColor2_LinkClicked);
			// 
			// linkLabelBackColor2
			// 
			this.linkLabelBackColor2.AutoSize = true;
			this.linkLabelBackColor2.Location = new System.Drawing.Point(128, 115);
			this.linkLabelBackColor2.Name = "linkLabelBackColor2";
			this.linkLabelBackColor2.Size = new System.Drawing.Size(74, 12);
			this.linkLabelBackColor2.TabIndex = 9;
			this.linkLabelBackColor2.TabStop = true;
			this.linkLabelBackColor2.Text = "背景色を変更";
			this.linkLabelBackColor2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBackColor2_LinkClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(10, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 12);
			this.label1.TabIndex = 12;
			this.label1.Text = "アクティブタブ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(112, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 12);
			this.label2.TabIndex = 13;
			this.label2.Text = "非アクティブタブ";
			// 
			// TabColorChangeDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(359, 151);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkLabelDefault);
			this.Controls.Add(this.linkLabelBackColor);
			this.Controls.Add(this.linkLabelForeColor);
			this.Controls.Add(this.linkLabelForeColor2);
			this.Controls.Add(this.linkLabelBackColor2);
			this.Controls.Add(this.tabControlSample);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "TabColorChangeDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "タブの配色設定";
			this.Load += new System.EventHandler(this.TabColorChangeDialog_Load);
			this.tabControlSample.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TabControl tabControlSample;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.LinkLabel linkLabelBackColor;
		private System.Windows.Forms.LinkLabel linkLabelForeColor;
		private System.Windows.Forms.LinkLabel linkLabelDefault;
		private System.Windows.Forms.LinkLabel linkLabelForeColor2;
		private System.Windows.Forms.LinkLabel linkLabelBackColor2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}