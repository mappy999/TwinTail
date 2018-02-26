namespace Twin.Forms
{
	partial class SubjectSearchOptionDialog
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxKeyword = new System.Windows.Forms.TextBox();
			this.numericUpDownCount = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.comboBoxSorting = new System.Windows.Forms.ComboBox();
			this.comboBoxSortOrder = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(324, 21);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(89, 25);
			this.buttonOK.TabIndex = 10;
			this.buttonOK.Text = "登録";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(324, 54);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(89, 25);
			this.buttonCancel.TabIndex = 11;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 78);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "取得する件数";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(23, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "キーワード";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(28, 111);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 12);
			this.label3.TabIndex = 6;
			this.label3.Text = "並び替え";
			// 
			// textBoxKeyword
			// 
			this.textBoxKeyword.Location = new System.Drawing.Point(105, 44);
			this.textBoxKeyword.Name = "textBoxKeyword";
			this.textBoxKeyword.Size = new System.Drawing.Size(180, 19);
			this.textBoxKeyword.TabIndex = 3;
			// 
			// numericUpDownCount
			// 
			this.numericUpDownCount.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDownCount.Location = new System.Drawing.Point(105, 76);
			this.numericUpDownCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownCount.Name = "numericUpDownCount";
			this.numericUpDownCount.Size = new System.Drawing.Size(67, 19);
			this.numericUpDownCount.TabIndex = 5;
			this.numericUpDownCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(47, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 12);
			this.label4.TabIndex = 8;
			this.label4.Text = "順序";
			// 
			// comboBoxSorting
			// 
			this.comboBoxSorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSorting.FormattingEnabled = true;
			this.comboBoxSorting.Location = new System.Drawing.Point(105, 108);
			this.comboBoxSorting.Name = "comboBoxSorting";
			this.comboBoxSorting.Size = new System.Drawing.Size(121, 20);
			this.comboBoxSorting.TabIndex = 7;
			// 
			// comboBoxSortOrder
			// 
			this.comboBoxSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSortOrder.FormattingEnabled = true;
			this.comboBoxSortOrder.Location = new System.Drawing.Point(105, 141);
			this.comboBoxSortOrder.Name = "comboBoxSortOrder";
			this.comboBoxSortOrder.Size = new System.Drawing.Size(121, 20);
			this.comboBoxSortOrder.TabIndex = 9;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(105, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 19);
			this.textBox1.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(35, 15);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 12);
			this.label5.TabIndex = 0;
			this.label5.Text = "表示名";
			// 
			// SubjectSearchOptionDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(435, 176);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.comboBoxSortOrder);
			this.Controls.Add(this.comboBoxSorting);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.numericUpDownCount);
			this.Controls.Add(this.textBoxKeyword);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SubjectSearchOptionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "検索内容を板ボタンに登録します";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxKeyword;
		private System.Windows.Forms.NumericUpDown numericUpDownCount;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboBoxSorting;
		private System.Windows.Forms.ComboBox comboBoxSortOrder;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label5;
	}
}