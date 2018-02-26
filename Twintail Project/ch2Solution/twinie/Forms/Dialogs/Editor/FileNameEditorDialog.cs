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
	/// ���[�U�[�Ƀt�@�C��������͂����邽�߂̃_�C�A���O
	/// </summary>
	public class FileNameEditorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button buttonOK;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// �\������郁�b�Z�[�W���擾�܂��͐ݒ肵�܂��B
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
		/// ���͂��ꂽ�t�@�C�������擾
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
		/// FileNameEditorDialog�N���X�̃C���X�^���X��������
		/// </summary>
		public FileNameEditorDialog()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
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
			this.label1.Text = "�t�@�C��������͂��Ă�������";
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
			// �t�@�C�����Ɏg�p�o���Ȃ��������܂܂�Ă��Ȃ����ǂ������`�F�b�N
			int index = textBox1.Text.IndexOfAny(Path.GetInvalidFileNameChars());
			if (index == -1) index = textBox1.Text.IndexOfAny(new char[] {Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar, Path.PathSeparator, Path.VolumeSeparatorChar});

			if (index >= 0)
			{
				MessageBox.Show(++index + "�����ڂɎg�p�ł��Ȃ��������܂܂�Ă��܂�", "���̓G���[",
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
