// ThreadSearchDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using Twin.Text;
	using CSharpSamples;

	/// <summary>
	/// �X���b�h���������邽�߂̃_�C�A���O
	/// </summary>
	public class ThreadSearchDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonHighlights;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkBoxMatchCase;
		private System.Windows.Forms.CheckBox checkBoxWholeWordsOnly;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonUp;
		private System.Windows.Forms.RadioButton radioButtonDown;

		private SearchSettings.ThreadSearchSettings settings;
		private ComboBox comboBox1;
		private AbstractSearcher searcher;

		/// <summary>
		/// �����L�[���[�h���擾�܂��͐ݒ�
		/// </summary>
		public string Keyword {
			set { comboBox1.Text = value; }
			get
			{
				return comboBox1.Text;
			}
		}

		/// <summary>
		/// �����I�v�V�������擾�܂��͐ݒ�
		/// </summary>
		public SearchOptions Options {
			set {
				radioButtonUp.Checked = (value & SearchOptions.RightToLeft) != 0;
				checkBoxMatchCase.Checked = (value & SearchOptions.MatchCase) != 0;
				checkBoxWholeWordsOnly.Checked = (value & SearchOptions.WholeWordsOnly) != 0;
			}
			get {
				SearchOptions flags = SearchOptions.None;
				if (radioButtonUp.Checked) flags |= SearchOptions.RightToLeft;
				if (checkBoxMatchCase.Checked) flags |= SearchOptions.MatchCase;
				if (checkBoxWholeWordsOnly.Checked) flags |= SearchOptions.WholeWordsOnly;
				return flags;
			}
		}

		/// <summary>
		/// ThreadSearchDialog�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadSearchDialog(AbstractSearcher sr)
		{
			if (sr == null) {
				throw new ArgumentNullException("sr");
			}
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			searcher = sr;
			settings = new SearchSettings.ThreadSearchSettings();

			comboBox1.Items.AddRange(Twinie.Settings.Search.SearchHistory.Keys.ToArray());
		}

		/// <summary>
		/// ThreadSearchDialog�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadSearchDialog(AbstractSearcher sr,
			SearchSettings.ThreadSearchSettings sett) : this(sr)
		{
			if (sett == null) {
				throw new ArgumentNullException("sett");
			}
			SetSettings(sett);
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
			this.buttonHighlights = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
			this.checkBoxWholeWordsOnly = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButtonUp = new System.Windows.Forms.RadioButton();
			this.radioButtonDown = new System.Windows.Forms.RadioButton();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonHighlights
			// 
			this.buttonHighlights.AutoSize = true;
			this.buttonHighlights.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonHighlights.Location = new System.Drawing.Point(298, 28);
			this.buttonHighlights.Margin = new System.Windows.Forms.Padding(2);
			this.buttonHighlights.Name = "buttonHighlights";
			this.buttonHighlights.Size = new System.Drawing.Size(83, 21);
			this.buttonHighlights.TabIndex = 3;
			this.buttonHighlights.Text = "�����\��(&H)";
			this.buttonHighlights.Click += new System.EventHandler(this.buttonHighlights_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "�����L�[���[�h(&D)";
			// 
			// buttonSearch
			// 
			this.buttonSearch.AutoSize = true;
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonSearch.Location = new System.Drawing.Point(298, 4);
			this.buttonSearch.Margin = new System.Windows.Forms.Padding(2);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(83, 21);
			this.buttonSearch.TabIndex = 2;
			this.buttonSearch.Text = "����";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(298, 60);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(83, 21);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "����";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// checkBoxMatchCase
			// 
			this.checkBoxMatchCase.AutoSize = true;
			this.checkBoxMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxMatchCase.Location = new System.Drawing.Point(4, 36);
			this.checkBoxMatchCase.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxMatchCase.Name = "checkBoxMatchCase";
			this.checkBoxMatchCase.Size = new System.Drawing.Size(151, 17);
			this.checkBoxMatchCase.TabIndex = 5;
			this.checkBoxMatchCase.Text = "�啶�������������(&C)";
			// 
			// checkBoxWholeWordsOnly
			// 
			this.checkBoxWholeWordsOnly.AutoSize = true;
			this.checkBoxWholeWordsOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxWholeWordsOnly.Location = new System.Drawing.Point(4, 52);
			this.checkBoxWholeWordsOnly.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxWholeWordsOnly.Name = "checkBoxWholeWordsOnly";
			this.checkBoxWholeWordsOnly.Size = new System.Drawing.Size(95, 17);
			this.checkBoxWholeWordsOnly.TabIndex = 6;
			this.checkBoxWholeWordsOnly.Text = "�P��P��(&W)";
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.radioButtonUp);
			this.groupBox1.Controls.Add(this.radioButtonDown);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(164, 28);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(126, 65);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "�����J�n�ʒu";
			// 
			// radioButtonUp
			// 
			this.radioButtonUp.AutoSize = true;
			this.radioButtonUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonUp.Location = new System.Drawing.Point(13, 32);
			this.radioButtonUp.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonUp.Name = "radioButtonUp";
			this.radioButtonUp.Size = new System.Drawing.Size(74, 17);
			this.radioButtonUp.TabIndex = 1;
			this.radioButtonUp.Text = "������(&E)";
			// 
			// radioButtonDown
			// 
			this.radioButtonDown.AutoSize = true;
			this.radioButtonDown.Checked = true;
			this.radioButtonDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButtonDown.Location = new System.Drawing.Point(13, 16);
			this.radioButtonDown.Margin = new System.Windows.Forms.Padding(2);
			this.radioButtonDown.Name = "radioButtonDown";
			this.radioButtonDown.Size = new System.Drawing.Size(74, 17);
			this.radioButtonDown.TabIndex = 0;
			this.radioButtonDown.TabStop = true;
			this.radioButtonDown.Text = "�ォ��(&F)";
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(103, 5);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(187, 20);
			this.comboBox1.TabIndex = 1;
			// 
			// ThreadSearchDialog
			// 
			this.AcceptButton = this.buttonSearch;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(392, 94);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkBoxMatchCase);
			this.Controls.Add(this.checkBoxWholeWordsOnly);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonSearch);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonHighlights);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThreadSearchDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "�X���b�h�̌���";
			this.Closed += new System.EventHandler(this.ThreadSearchDialog_Closed);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// �ݒ��K�p
		/// </summary>
		/// <param name="sett"></param>
		private void SetSettings(SearchSettings.ThreadSearchSettings sett)
		{
			Keyword = sett.Keyword;
			Options = sett.Options;
			settings = sett;
		}

		/// <summary>
		/// �ݒ��ۑ�
		/// </summary>
		private void SaveSettings()
		{
			settings.Options = Options;
			settings.Keyword = Keyword;
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			if (ErrorCheck())
				return;

			searcher.Options = Options;

			Twinie.Settings.Search.SearchHistory.Add(Keyword);

			if (!searcher.Search(Keyword))
			{
				searcher.Reset();

				MessageBox.Show(this, "'" + Keyword + "' �͌�����܂���ł���", "�����I��",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void buttonHighlights_Click(object sender, System.EventArgs e)
		{
			if (ErrorCheck())
				return;

			try {
				Cursor = Cursors.WaitCursor;
				searcher.Highlights(Keyword);
			} finally {
				Cursor = Cursors.Default;
			}
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// �G���[���Ȃ����`�F�b�N
		/// </summary>
		/// <returns></returns>
		private bool ErrorCheck()
		{
			if (Keyword == String.Empty)
			{
				MessageBox.Show(this, "�����L�[���[�h����͂��Ă�������", "�����ł��܂���B�B",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return true;
			}
			return false;
		}

		private void ThreadSearchDialog_Closed(object sender, System.EventArgs e)
		{
			SaveSettings();

			if (Owner != null)
				Owner.Activate();
		}
	}
}
