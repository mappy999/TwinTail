using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Twin.Forms
{
	/// <summary>
	/// SambaErrorDialog の概要の説明です。
	/// </summary>
	public class SambaErrorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label labelCount;
		private System.Windows.Forms.Button buttonIgnore;
		private System.Windows.Forms.Button buttonOK;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SambaErrorDialog(int count)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			labelCount.Text = count.ToString();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SambaErrorDialog));
			this.label1 = new System.Windows.Forms.Label();
			this.labelCount = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonIgnore = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "あと";
			// 
			// labelCount
			// 
			this.labelCount.AutoSize = true;
			this.labelCount.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelCount.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelCount.ForeColor = System.Drawing.Color.Black;
			this.labelCount.Location = new System.Drawing.Point(24, 0);
			this.labelCount.Name = "labelCount";
			this.labelCount.Size = new System.Drawing.Size(26, 12);
			this.labelCount.TabIndex = 3;
			this.labelCount.Text = "123";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(58, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "秒 待ってください。";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.labelCount);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Location = new System.Drawing.Point(97, 20);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(148, 12);
			this.panel1.TabIndex = 5;
			// 
			// buttonIgnore
			// 
			this.buttonIgnore.AutoSize = true;
			this.buttonIgnore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
			this.buttonIgnore.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonIgnore.Location = new System.Drawing.Point(50, 54);
			this.buttonIgnore.Name = "buttonIgnore";
			this.buttonIgnore.Size = new System.Drawing.Size(70, 21);
			this.buttonIgnore.TabIndex = 6;
			this.buttonIgnore.Text = "無視";
			// 
			// buttonOK
			// 
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(130, 54);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(70, 21);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "はい";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(20, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// SambaErrorDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonOK;
			this.ClientSize = new System.Drawing.Size(259, 87);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonIgnore);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SambaErrorDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Samba 確認ダイアログ";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}
