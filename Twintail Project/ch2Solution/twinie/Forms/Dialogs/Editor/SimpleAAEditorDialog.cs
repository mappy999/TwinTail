// SimpleAAEditorDialog.cs

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Twin.Aa;

namespace Twin.Forms
{
	/// <summary>
	/// SimpleAAEditorDialog �̊T�v�̐����ł��B
	/// </summary>
	public class SimpleAAEditorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonRegist;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxCategory;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Fields
		private AaHeaderCollection headerColl = new AaHeaderCollection(); 
		#endregion

		#region Properties
		
		#endregion

		/// <summary>
		/// SimpleAAEditorDialog �N���X�̃C���X�^���X��������
		/// </summary>
		public SimpleAAEditorDialog(string aafolder, string aatext)
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			
			foreach (string filename in Directory.GetFiles(aafolder, "*.aa"))
			{
				AaHeader header = new AaHeader(filename);
				headerColl.Add(header);
				comboBoxCategory.Items.Add(header);
			}

			if (comboBoxCategory.Items.Count > 0)
				comboBoxCategory.SelectedIndex = 0;

			else {
				buttonRegist.Enabled = false;
			}

			textBox.Text = aatext;
		}

		#region Auto Generated Code
		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonRegist = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxCategory = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.AcceptsReturn = true;
			this.textBox.AcceptsTab = true;
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Font = new System.Drawing.Font("�l�r �o�S�V�b�N", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textBox.Location = new System.Drawing.Point(8, 36);
			this.textBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox.Size = new System.Drawing.Size(332, 182);
			this.textBox.TabIndex = 0;
			this.textBox.WordWrap = false;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(256, 8);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(79, 22);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "�L�����Z��(&C)";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonRegist
			// 
			this.buttonRegist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRegist.AutoSize = true;
			this.buttonRegist.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonRegist.Location = new System.Drawing.Point(172, 8);
			this.buttonRegist.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.buttonRegist.Name = "buttonRegist";
			this.buttonRegist.Size = new System.Drawing.Size(79, 22);
			this.buttonRegist.TabIndex = 1;
			this.buttonRegist.Text = "�o�^(&R)";
			this.buttonRegist.Click += new System.EventHandler(this.buttonRegist_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "�o�^�J�e�S��";
			// 
			// comboBoxCategory
			// 
			this.comboBoxCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCategory.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxCategory.Location = new System.Drawing.Point(88, 8);
			this.comboBoxCategory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.comboBoxCategory.Name = "comboBoxCategory";
			this.comboBoxCategory.Size = new System.Drawing.Size(75, 20);
			this.comboBoxCategory.TabIndex = 4;
			// 
			// SimpleAAEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(344, 222);
			this.Controls.Add(this.comboBoxCategory);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonRegist);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.textBox);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "SimpleAAEditorDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AA��o�^";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Methods

		#endregion

		#region Event Handlers
		private void buttonRegist_Click(object sender, System.EventArgs e)
		{
			FileNameEditorDialog dlg = new FileNameEditorDialog();
			AaItem newItem = null;

			AaHeader header = comboBoxCategory.SelectedItem as AaHeader;
			header.Load();

			if (textBox.Lines.Length > 1)
			{
				if (dlg.ShowDialog(this) != DialogResult.OK)
					return;

				newItem = new AaItem(dlg.FileName, false);
				header.Items.Add(newItem);

				newItem.Data = textBox.Text;
			}
			else {
				newItem = new AaItem(textBox.Text, true);
				header.Items.Add(newItem);
			}

			header.Save();

			Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}
		#endregion
	}
}
