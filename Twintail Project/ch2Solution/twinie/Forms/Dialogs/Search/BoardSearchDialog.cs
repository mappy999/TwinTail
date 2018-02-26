// BoardSearchDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// ����������_�C�A���O
	/// </summary>
	public class BoardSearchDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ITwinTableControl tableControl;
		private BoardInfo[] boards;
		private ComboBox comboBoxKey;
		private int index;

		/// <summary>
		/// BoardSearchDialog�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="table"></param>
		public BoardSearchDialog(ITwinTableControl table)
		{
			if (table == null) {
				throw new ArgumentNullException("table");
			}
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			tableControl = table;
			boards = null;

			comboBoxKey.Items.AddRange(Twinie.Settings.Search.SearchHistory.Keys.ToArray());
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
			this.buttonSearch = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxKey = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// buttonSearch
			// 
			this.buttonSearch.AutoSize = true;
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonSearch.Location = new System.Drawing.Point(236, 4);
			this.buttonSearch.Margin = new System.Windows.Forms.Padding(2);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(67, 21);
			this.buttonSearch.TabIndex = 2;
			this.buttonSearch.Text = "��������";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(4, 8);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "����������";
			// 
			// comboBoxKey
			// 
			this.comboBoxKey.FormattingEnabled = true;
			this.comboBoxKey.Location = new System.Drawing.Point(79, 5);
			this.comboBoxKey.Name = "comboBoxKey";
			this.comboBoxKey.Size = new System.Drawing.Size(147, 20);
			this.comboBoxKey.TabIndex = 1;
			this.comboBoxKey.TextChanged += new System.EventHandler(this.comboBoxKey_TextChanged);
			// 
			// BoardSearchDialog
			// 
			this.AcceptButton = this.buttonSearch;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(311, 33);
			this.Controls.Add(this.comboBoxKey);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonSearch);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BoardSearchDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "������";
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BoardSearchDialog_KeyUp);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			if (boards == null)
			{
				boards = tableControl.Find(comboBoxKey.Text);
				index = 0;
			}

			if (boards == null || index >= boards.Length)
			{
				MessageBox.Show("'" + comboBoxKey.Text + "' �͌�����܂���ł���", "�����I��",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				// ����������I��
				tableControl.Selected = boards[index++];
			}
		}

		private void BoardSearchDialog_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				e.Handled = true;
				Close();
			}
		}

		private void comboBoxKey_TextChanged(object sender, EventArgs e)
		{
			boards = null;
		}
	}
}
