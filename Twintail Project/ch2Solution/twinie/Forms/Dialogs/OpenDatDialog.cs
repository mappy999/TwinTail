// OpenDatDialog.cs

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using Twin.Util;

namespace Twin.Forms
{
	/// <summary>
	/// OpenDatDialog �̊T�v�̐����ł��B
	/// </summary>
	public class OpenThreadDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBoxFilePath;
		private System.Windows.Forms.Button buttonRefFilePath;
		private System.Windows.Forms.TextBox textBoxDatNumber;
		private System.Windows.Forms.ComboBox comboBoxBoards;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Fields
		private Cache cacheInfo;
		private ThreadHeader headerInfo = null;
		#endregion

		#region Properties
		/// <summary>
		/// �L���b�V���ɍ쐬���ꂽ�X���b�h�̃w�b�_�����擾
		/// </summary>
		public ThreadHeader HeaderInfo {
			get {
				return headerInfo;
			}
		}
		#endregion

		/// <summary>
		/// OpenDatDialog �N���X�̃C���X�^���X��������
		/// </summary>
		public OpenThreadDialog(Cache cache, IBoardTable table)
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			cacheInfo = cache;
			comboBoxBoards.Items.AddRange(table.ToArray());
			comboBoxBoards.SelectedIndex = 0;
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
			this.textBoxFilePath = new System.Windows.Forms.TextBox();
			this.buttonRefFilePath = new System.Windows.Forms.Button();
			this.textBoxDatNumber = new System.Windows.Forms.TextBox();
			this.comboBoxBoards = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// textBoxFilePath
			// 
			this.textBoxFilePath.Location = new System.Drawing.Point(6, 35);
			this.textBoxFilePath.Name = "textBoxFilePath";
			this.textBoxFilePath.Size = new System.Drawing.Size(403, 22);
			this.textBoxFilePath.TabIndex = 1;
			// 
			// buttonRefFilePath
			// 
			this.buttonRefFilePath.AutoSize = true;
			this.buttonRefFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRefFilePath.Location = new System.Drawing.Point(414, 35);
			this.buttonRefFilePath.Name = "buttonRefFilePath";
			this.buttonRefFilePath.Size = new System.Drawing.Size(34, 25);
			this.buttonRefFilePath.TabIndex = 2;
			this.buttonRefFilePath.Text = "...";
			this.buttonRefFilePath.Click += new System.EventHandler(this.buttonRefFilePath_Click);
			// 
			// textBoxDatNumber
			// 
			this.textBoxDatNumber.Location = new System.Drawing.Point(6, 90);
			this.textBoxDatNumber.Name = "textBoxDatNumber";
			this.textBoxDatNumber.Size = new System.Drawing.Size(156, 22);
			this.textBoxDatNumber.TabIndex = 4;
			// 
			// comboBoxBoards
			// 
			this.comboBoxBoards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxBoards.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxBoards.Location = new System.Drawing.Point(6, 155);
			this.comboBoxBoards.MaxDropDownItems = 16;
			this.comboBoxBoards.Name = "comboBoxBoards";
			this.comboBoxBoards.Size = new System.Drawing.Size(224, 23);
			this.comboBoxBoards.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(6, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(178, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "�t�@�C���p�X���w�肵�Ă�������";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(6, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(216, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "�X���b�h��dat�ԍ�����͂��Ă�������";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(6, 130);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(267, 15);
			this.label3.TabIndex = 5;
			this.label3.Text = "�J���X���b�h�Ɗ֘A�Â����I�����Ă�������";
			// 
			// buttonOK
			// 
			this.buttonOK.AutoSize = true;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(386, 125);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(124, 25);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "�J��";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(386, 155);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(124, 25);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "�L�����Z��";
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "dat �t�@�C�� (*.dat)|*.dat|���ׂẴt�@�C�� (*.*)|*.*";
			// 
			// OpenThreadDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(519, 195);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBoxBoards);
			this.Controls.Add(this.textBoxDatNumber);
			this.Controls.Add(this.buttonRefFilePath);
			this.Controls.Add(this.textBoxFilePath);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OpenThreadDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "�X���b�h���J��";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Methods

		#endregion

		#region Event Handlers

		private void buttonRefFilePath_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
				textBoxFilePath.Text = openFileDialog.FileName;
		}

		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			if (ErrorCheck())
				return;

			try {
				headerInfo =
					ThreadUtil.OpenDat(cacheInfo,
						(BoardInfo)comboBoxBoards.SelectedItem,
						textBoxFilePath.Text, textBoxDatNumber.Text, false);

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		// ���͂��ꂽ��񂪐��������ǂ������`�F�b�N
		// �s���Ȓl�����݂���� true ��Ԃ�
		private bool ErrorCheck()
		{
			string message = String.Empty;
			bool result = true;

			if (textBoxDatNumber.Text == String.Empty)
			{
				message = "dat�ԍ�����͂��Ă�������";
			}
			else if (!File.Exists(textBoxFilePath.Text))
			{
				message = "�w�肳�ꂽ�t�@�C���͑��݂��܂���";
			}
			else if (comboBoxBoards.SelectedIndex == -1)
			{
				message = "�֘A�Â�����I������Ă��܂���";
			}
			else {
				result = false;
			}

			if (result)
				MessageBox.Show(message, "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Warning);

			return result;
		}
	}
}
