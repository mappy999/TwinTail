// SelectSkinDialog.cs

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Twin.Forms
{
	/// <summary>
	/// SelectSkinDialog �̊T�v�̐����ł��B
	/// </summary>
	public class SelectSkinDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox comboBoxSkins;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static string lastSelectedSkinName = null;

		/// <summary>
		/// �I�����ꂽ�X�L���t�H���_�̃p�X��Ԃ��B
		/// </summary>
		public string SelectedSkinFolderPath {
			get {
				string name = comboBoxSkins.SelectedItem as string;
				if (name == null)
					return null;

				return Path.Combine(Settings.BaseSkinFolderPath, name);
			}
		}

		public SelectSkinDialog()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//

			foreach (string path in Directory.GetDirectories(Settings.BaseSkinFolderPath))
				comboBoxSkins.Items.Add(Path.GetFileName(path));

			if (comboBoxSkins.Items.Count == 0)
			{
				MessageBox.Show("�X�L����������܂���ł����B");
				button1.PerformClick(); // Cancel �{�^���������ĕ���
			}
			else if (lastSelectedSkinName != null)
			{
				comboBoxSkins.SelectedItem = lastSelectedSkinName;
			}
			else
			{
				int index = comboBoxSkins.FindString(Twinie.Settings.SkinFolderName);
				comboBoxSkins.SelectedIndex = index;
			}

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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.comboBoxSkins = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.AutoSize = true;
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(134, 60);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 21);
			this.button1.TabIndex = 3;
			this.button1.Text = "�L�����Z��";
			// 
			// button2
			// 
			this.button2.AutoSize = true;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(54, 60);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 21);
			this.button2.TabIndex = 2;
			this.button2.Text = "OK";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// comboBoxSkins
			// 
			this.comboBoxSkins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSkins.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxSkins.Location = new System.Drawing.Point(10, 30);
			this.comboBoxSkins.Name = "comboBoxSkins";
			this.comboBoxSkins.Size = new System.Drawing.Size(245, 20);
			this.comboBoxSkins.TabIndex = 1;
			this.comboBoxSkins.SelectedIndexChanged += new System.EventHandler(this.comboBoxSkins_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(10, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(231, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "HTML�ϊ��Ɏg�p����X�L����I�����Ă��������B";
			// 
			// SelectSkinDialog
			// 
			this.AcceptButton = this.button2;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.button1;
			this.ClientSize = new System.Drawing.Size(279, 93);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBoxSkins);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SelectSkinDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "�X�L���̑I��";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void comboBoxSkins_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lastSelectedSkinName = comboBoxSkins.SelectedItem as string;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int index = comboBoxSkins.SelectedIndex;

			if (index == -1)
			{
				MessageBox.Show("�ϊ��Ɏg�p����X�L����I�����Ă�������");
			}
			else
			{
				this.DialogResult = DialogResult.OK;
			}
		}
	}
}
