// NGUrlEditorDialog.cs

namespace ImageViewerDll
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// NGUrlEditorDialog �̊T�v�̐����ł��B
	/// </summary>
	public class NGUrlEditorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBoxPatterns;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// NGURL�̃p�^�[���z����擾�܂��͐ݒ�
		/// </summary>
		public string[] Patterns {
			set {
				textBoxPatterns.Lines = value;
			}
			get {
				//return textBoxPatterns.Lines;

				ArrayList arrayList = new ArrayList();

				// ��s�͏���
				foreach (string word in textBoxPatterns.Lines)
					if (word != String.Empty) arrayList.Add(word);

				return (string[])arrayList.ToArray(typeof(string));
			}
		}

		/// <summary>
		/// NGUrlEditorDialog�N���X�̃C���X�^���X��������
		/// </summary>
		public NGUrlEditorDialog()
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
			this.textBoxPatterns = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxPatterns
			// 
			this.textBoxPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPatterns.Location = new System.Drawing.Point(6, 5);
			this.textBoxPatterns.Multiline = true;
			this.textBoxPatterns.Name = "textBoxPatterns";
			this.textBoxPatterns.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxPatterns.Size = new System.Drawing.Size(403, 240);
			this.textBoxPatterns.TabIndex = 0;
			this.textBoxPatterns.WordWrap = false;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(213, 250);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(106, 25);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "�L�����Z��";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(95, 250);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(107, 25);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			// 
			// NGUrlEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(414, 276);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.textBoxPatterns);
			this.Name = "NGUrlEditorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NGURL��ҏW";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}
