// EditQuickSaveFolderDialog.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using CSharpSamples;

	/// <summary>
	/// EditQuickSaveFolderDialog �̊T�v�̐����ł��B
	/// </summary>
	public class EditQuickSaveFolderDialog : System.Windows.Forms.Form
	{
		#region Designer Fields
		private System.Windows.Forms.Button buttonRefFolderPath;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.TextBox textBoxFolderPath;
		private System.Windows.Forms.TextBox textBoxFolderName;
		private System.Windows.Forms.ComboBox comboBoxShortcuts;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		private QuickSaveFolderItem item;

		/// <summary>
		/// QuickFolderItem���擾�܂��͐ݒ�
		/// </summary>
		public QuickSaveFolderItem Item {
			set {
				if (value == null)
					throw new ArgumentNullException("Item");

				item = value;
			}
			get { return item; }
		}

		/// <summary>
		/// EditQuickSaveFolderDialog�N���X�̃C���X�^���X��������
		/// </summary>
		public EditQuickSaveFolderDialog()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			item = new QuickSaveFolderItem();

			// Shortcut�񋓑̂��R���{�{�b�N�X�ɒǉ�
			Array array = Enum.GetValues(typeof(Shortcut));
			
			foreach (object key in array)
				comboBoxShortcuts.Items.Add(key);
			comboBoxShortcuts.SelectedItem = Shortcut.None;
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
			this.buttonRefFolderPath = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.textBoxFolderPath = new System.Windows.Forms.TextBox();
			this.textBoxFolderName = new System.Windows.Forms.TextBox();
			this.comboBoxShortcuts = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonRefFolderPath
			// 
			this.buttonRefFolderPath.Location = new System.Drawing.Point(264, 20);
			this.buttonRefFolderPath.Name = "buttonRefFolderPath";
			this.buttonRefFolderPath.Size = new System.Drawing.Size(64, 20);
			this.buttonRefFolderPath.TabIndex = 2;
			this.buttonRefFolderPath.Text = "�Q��...";
			this.buttonRefFolderPath.Click += new System.EventHandler(this.buttonRefFolderPath_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(352, 8);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(76, 20);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(352, 32);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(76, 20);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "�L�����Z��";
			// 
			// textBoxFolderPath
			// 
			this.textBoxFolderPath.Location = new System.Drawing.Point(12, 20);
			this.textBoxFolderPath.Name = "textBoxFolderPath";
			this.textBoxFolderPath.Size = new System.Drawing.Size(244, 19);
			this.textBoxFolderPath.TabIndex = 1;
			this.textBoxFolderPath.Text = "";
			// 
			// textBoxFolderName
			// 
			this.textBoxFolderName.Location = new System.Drawing.Point(12, 60);
			this.textBoxFolderName.Name = "textBoxFolderName";
			this.textBoxFolderName.Size = new System.Drawing.Size(244, 19);
			this.textBoxFolderName.TabIndex = 4;
			this.textBoxFolderName.Text = "";
			// 
			// comboBoxShortcuts
			// 
			this.comboBoxShortcuts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxShortcuts.Location = new System.Drawing.Point(12, 100);
			this.comboBoxShortcuts.Name = "comboBoxShortcuts";
			this.comboBoxShortcuts.Size = new System.Drawing.Size(108, 20);
			this.comboBoxShortcuts.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 84);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 12);
			this.label1.TabIndex = 5;
			this.label1.Text = "�V���[�g�J�b�g�L�[";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "�ۑ��t�H���_";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 12);
			this.label3.TabIndex = 3;
			this.label3.Text = "�\����";
			// 
			// EditQuickSaveFolderDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(434, 126);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.comboBoxShortcuts,
																		  this.textBoxFolderName,
																		  this.textBoxFolderPath,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.buttonRefFolderPath});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "EditQuickSaveFolderDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EditQuickSaveFolderDialog";
			this.Load += new System.EventHandler(this.EditQuickSaveFolderDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void EditQuickSaveFolderDialog_Load(object sender, System.EventArgs e)
		{
			textBoxFolderPath.Text = item.FolderPath;
			textBoxFolderName.Text = item.Title;
			comboBoxShortcuts.SelectedItem = item.Shortcut;
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			// �p�X�����������ǂ����𒲂ׂ�
			try {Uri uri = new Uri(textBoxFolderPath.Text);}
			catch (Exception ex) { MessageBox.Show(ex.ToString(), "�w�肵���t�H���_�p�X�͕s���ł�"); return; }

			item.FolderPath = textBoxFolderPath.Text;
			item.Title = textBoxFolderName.Text;
			item.Shortcut = (Shortcut)comboBoxShortcuts.SelectedItem;

			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonRefFolderPath_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.Description = "�ۑ��t�H���_�����w�肵�Ă�������";

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				textBoxFolderPath.Text = dlg.SelectedPath;

				// �t�H���_�̕ʖ�����̏ꍇ�̓t�H���_����ݒ�
				if (textBoxFolderName.Text == String.Empty)
					textBoxFolderName.Text = Path.GetFileName(dlg.SelectedPath);
			}
		}
	}
}
