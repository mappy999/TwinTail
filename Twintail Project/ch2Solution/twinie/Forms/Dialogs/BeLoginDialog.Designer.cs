namespace Twin.Forms
{
	partial class BeLoginDialog
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
			this.textBoxMailAddress = new System.Windows.Forms.TextBox();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(98, 119);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(85, 25);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "ログイン";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(199, 119);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(85, 25);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// textBoxMailAddress
			// 
			this.textBoxMailAddress.Location = new System.Drawing.Point(125, 29);
			this.textBoxMailAddress.Name = "textBoxMailAddress";
			this.textBoxMailAddress.Size = new System.Drawing.Size(181, 19);
			this.textBoxMailAddress.TabIndex = 1;
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.Location = new System.Drawing.Point(125, 58);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.Size = new System.Drawing.Size(181, 19);
			this.textBoxPassword.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "メールアドレス(&A):";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(40, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "パスワード(&P):";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBoxMailAddress);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textBoxPassword);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(350, 93);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ユーザー情報";
			// 
			// BeLoginDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(382, 156);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BeLoginDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BEログイン";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BeLoginDialog_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BeLoginDialog_FormClosed);
			this.Load += new System.EventHandler(this.BeLoginDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.TextBox textBoxMailAddress;
		private System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}