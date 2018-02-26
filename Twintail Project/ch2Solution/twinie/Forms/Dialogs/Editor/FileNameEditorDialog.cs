// FileNameEditorDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.IO;
	using System.Text;

	/// <summary>
	/// ユーザーにファイル名を入力させるためのダイアログ
	/// </summary>
	public class FileNameEditorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button buttonOK;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 表示されるメッセージを取得または設定します。
		/// </summary>
		public string Message
		{
			set
			{
				label1.Text = value;
			}
			get
			{
				return label1.Text;
			}
		}
	
		/// <summary>
		/// 入力されたファイル名を取得
		/// </summary>
		public string FileName {
			set
			{
				textBox1.Text = value;
			}
			get {
				return textBox1.Text;
			}
		}

		/// <summary>
		/// FileNameEditorDialogクラスのインスタンスを初期化
		/// </summary>
		public FileNameEditorDialog()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
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
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "ファイル名を入力してください";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 28);
			this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(249, 19);
			this.textBox1.TabIndex = 1;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// buttonOK
			// 
			this.buttonOK.AutoSize = true;
			this.buttonOK.Enabled = false;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(265, 28);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(71, 21);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// FileNameEditorDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(346, 57);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FileNameEditorDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FileNameEditorDialog";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			// ファイル名に使用出来ない文字が含まれていないかどうかをチェック
			int index = textBox1.Text.IndexOfAny(Path.GetInvalidFileNameChars());
			if (index == -1) index = textBox1.Text.IndexOfAny(new char[] {Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar, Path.PathSeparator, Path.VolumeSeparatorChar});

			if (index >= 0)
			{
				MessageBox.Show(++index + "文字目に使用できない文字が含まれています", "入力エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			else {
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			buttonOK.Enabled = (textBox1.Text.Length > 0) ? true : false;
		}
	}
}
