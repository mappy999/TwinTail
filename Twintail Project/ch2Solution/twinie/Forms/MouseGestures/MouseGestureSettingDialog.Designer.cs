namespace Twin.Forms
{
	partial class MouseGestureSettingDialog
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
			this.buttonLeft = new System.Windows.Forms.Button();
			this.buttonDown = new System.Windows.Forms.Button();
			this.buttonUp = new System.Windows.Forms.Button();
			this.buttonRight = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.textBoxDirection = new System.Windows.Forms.TextBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.comboBoxAction = new System.Windows.Forms.ComboBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonDel = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonReset = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonLeft
			// 
			this.buttonLeft.Location = new System.Drawing.Point(299, 52);
			this.buttonLeft.Name = "buttonLeft";
			this.buttonLeft.Size = new System.Drawing.Size(33, 23);
			this.buttonLeft.TabIndex = 4;
			this.buttonLeft.Text = "←";
			this.buttonLeft.UseVisualStyleBackColor = true;
			this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
			// 
			// buttonDown
			// 
			this.buttonDown.Location = new System.Drawing.Point(338, 67);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(33, 23);
			this.buttonDown.TabIndex = 7;
			this.buttonDown.Text = "↓";
			this.buttonDown.UseVisualStyleBackColor = true;
			this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
			// 
			// buttonUp
			// 
			this.buttonUp.Location = new System.Drawing.Point(338, 38);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(33, 23);
			this.buttonUp.TabIndex = 5;
			this.buttonUp.Text = "↑";
			this.buttonUp.UseVisualStyleBackColor = true;
			this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
			// 
			// buttonRight
			// 
			this.buttonRight.Location = new System.Drawing.Point(377, 52);
			this.buttonRight.Name = "buttonRight";
			this.buttonRight.Size = new System.Drawing.Size(33, 23);
			this.buttonRight.TabIndex = 6;
			this.buttonRight.Text = "→";
			this.buttonRight.UseVisualStyleBackColor = true;
			this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(423, 173);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 12;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// textBoxDirection
			// 
			this.textBoxDirection.Location = new System.Drawing.Point(139, 54);
			this.textBoxDirection.Name = "textBoxDirection";
			this.textBoxDirection.ReadOnly = true;
			this.textBoxDirection.Size = new System.Drawing.Size(148, 19);
			this.textBoxDirection.TabIndex = 3;
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(12, 96);
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBox1.Size = new System.Drawing.Size(388, 100);
			this.listBox1.TabIndex = 10;
			// 
			// comboBoxAction
			// 
			this.comboBoxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxAction.FormattingEnabled = true;
			this.comboBoxAction.Location = new System.Drawing.Point(12, 52);
			this.comboBoxAction.Name = "comboBoxAction";
			this.comboBoxAction.Size = new System.Drawing.Size(121, 20);
			this.comboBoxAction.TabIndex = 2;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDown1.Location = new System.Drawing.Point(123, 12);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.ReadOnly = true;
			this.numericUpDown1.Size = new System.Drawing.Size(57, 19);
			this.numericUpDown1.TabIndex = 1;
			this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown1.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(27, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "移動量（ピクセル）";
			// 
			// buttonDel
			// 
			this.buttonDel.Location = new System.Drawing.Point(12, 205);
			this.buttonDel.Name = "buttonDel";
			this.buttonDel.Size = new System.Drawing.Size(185, 23);
			this.buttonDel.TabIndex = 11;
			this.buttonDel.Text = "選択項目を削除";
			this.buttonDel.UseVisualStyleBackColor = true;
			this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(423, 38);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(75, 23);
			this.buttonAdd.TabIndex = 8;
			this.buttonAdd.Text = "追加";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonReset
			// 
			this.buttonReset.Location = new System.Drawing.Point(423, 67);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(75, 23);
			this.buttonReset.TabIndex = 9;
			this.buttonReset.Text = "リセット";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// MouseGestureSettingDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(510, 240);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.buttonDel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.comboBoxAction);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.textBoxDirection);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonRight);
			this.Controls.Add(this.buttonUp);
			this.Controls.Add(this.buttonDown);
			this.Controls.Add(this.buttonLeft);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MouseGestureSettingDialog";
			this.Text = "MouseGestureSettingDialog";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonLeft;
		private System.Windows.Forms.Button buttonDown;
		private System.Windows.Forms.Button buttonUp;
		private System.Windows.Forms.Button buttonRight;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TextBox textBoxDirection;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ComboBox comboBoxAction;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonDel;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonReset;
	}
}