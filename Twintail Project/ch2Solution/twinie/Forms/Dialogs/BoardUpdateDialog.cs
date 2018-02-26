// BoardUpdateDialog.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;

	/// <summary>
	///�ꗗ���I�����C���ōX�V
	/// </summary>
	public class BoardUpdateDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonUpdate;
		private System.Windows.Forms.TextBox textBoxOutput;
		private System.Windows.Forms.ComboBox comboBoxUrls;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		private delegate void AppendTextDelegate(string text);

		private IBoardTable table;
		private Thread thread;

		private string url;
		private bool updated = false;

		/// <summary>
		/// �̐V�ǉ��܂��͈ړ]���ɔ�������C�x���g
		/// </summary>
		public event BoardUpdateEventHandler Updated;

		/// <summary>
		/// BoardUpdateDialog�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="table"></param>
		public BoardUpdateDialog(IBoardTable table)
		{
			if (table == null)
				throw new ArgumentNullException("table");
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			this.table = table;
			this.thread = null;
			this.DialogResult = DialogResult.Cancel;
			
			int index = comboBoxUrls.Items.Add(Twinie.Settings.OnlineUpdateUrl);
			comboBoxUrls.SelectedIndex = index;
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
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonUpdate = new System.Windows.Forms.Button();
			this.textBoxOutput = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxUrls = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonClose.AutoSize = true;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClose.Location = new System.Drawing.Point(170, 206);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(88, 21);
			this.buttonClose.TabIndex = 1;
			this.buttonClose.Text = "����";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonUpdate
			// 
			this.buttonUpdate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonUpdate.AutoSize = true;
			this.buttonUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonUpdate.Location = new System.Drawing.Point(78, 206);
			this.buttonUpdate.Margin = new System.Windows.Forms.Padding(2);
			this.buttonUpdate.Name = "buttonUpdate";
			this.buttonUpdate.Size = new System.Drawing.Size(88, 21);
			this.buttonUpdate.TabIndex = 0;
			this.buttonUpdate.Text = "�X�V";
			this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
			// 
			// textBoxOutput
			// 
			this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxOutput.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxOutput.Location = new System.Drawing.Point(8, 48);
			this.textBoxOutput.Margin = new System.Windows.Forms.Padding(2);
			this.textBoxOutput.Multiline = true;
			this.textBoxOutput.Name = "textBoxOutput";
			this.textBoxOutput.ReadOnly = true;
			this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxOutput.Size = new System.Drawing.Size(320, 153);
			this.textBoxOutput.TabIndex = 5;
			this.textBoxOutput.WordWrap = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 4);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(195, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "�̍X�V����ѐV�̃`�F�b�N���s���܂�";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 28);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "�X�V��URL";
			// 
			// comboBoxUrls
			// 
			this.comboBoxUrls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxUrls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxUrls.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxUrls.Items.AddRange(new object[] {
            "2ch�I�t�B�V������ http://menu.2ch.net/bbsmenu.html",
            "Azlucky����� http://azlucky.s25.xrea.com/2chboard/bbsmenu.html",
            "jikkyo.org�� http://jikkyo.sakura.ne.jp/bbsmenu.html"});
			this.comboBoxUrls.Location = new System.Drawing.Point(80, 24);
			this.comboBoxUrls.Margin = new System.Windows.Forms.Padding(2);
			this.comboBoxUrls.Name = "comboBoxUrls";
			this.comboBoxUrls.Size = new System.Drawing.Size(249, 20);
			this.comboBoxUrls.TabIndex = 4;
			// 
			// BoardUpdateDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(336, 229);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxUrls);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxOutput);
			this.Controls.Add(this.buttonUpdate);
			this.Controls.Add(this.buttonClose);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "BoardUpdateDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "�ꗗ�̍X�V";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void AppendText(string text)
		{
			textBoxOutput.AppendText(text);
			textBoxOutput.ScrollToCaret();
		}

		private void OnUpdating()
		{
			try {
				Invoke(new AppendTextDelegate(AppendText), "�ꗗ�̍X�V���J�n\r\n--------------------\r\n\r\n");

				table.OnlineUpdate(url, new BoardUpdateEventHandler(OnBoardUpdate));
			}
			catch (ThreadAbortException) {}
			catch (Exception ex) {
				TwinDll.ShowOutput(ex);
			}
			finally {
				thread = null;
				Invoke(new MethodInvoker(OnFinally));
			}
		}

		private void OnBoardUpdate(object sender, BoardUpdateEventArgs e)
		{
			Invoke(new BoardUpdateEventHandler(OnBoardUpdateInternal), new object[] {sender, e});
		}

		private void OnBoardUpdateInternal(object sender, BoardUpdateEventArgs e)
		{
			string text = null;
			
			if (e.Event == BoardUpdateEvent.New)
			{
				text = String.Format("�V���ǉ�����܂���\r\n{0} {1}",
					e.NewBoard.Name, e.NewBoard.Url);
			}
			else if (e.Event == BoardUpdateEvent.Change)
			{
				text = String.Format("��URL���ύX����܂���\r\n{0} {1} -> {2}",
					e.OldBoard.Name, e.OldBoard.Url, e.NewBoard.Url);
			}
			else
			{
				text = "�L�����Z������܂���";
			}

			textBoxOutput.AppendText(text);
			textBoxOutput.AppendText("\r\n\r\n");
			textBoxOutput.ScrollToCaret();

			if (Updated != null)
				Updated(sender, e);
		}

		private void OnFinally()
		{
			buttonClose.Text = "����";
			buttonUpdate.Enabled = true;
			AppendText("--------------------\r\n�X�V���������܂���\r\n");
			updated = true;
		}

		private void buttonUpdate_Click(object sender, System.EventArgs e)
		{
			if (thread != null)
				return;

			try {
				url = comboBoxUrls.SelectedItem.ToString();
				url = url.Substring(url.IndexOf("http://"));

				buttonUpdate.Enabled = false;
				buttonClose.Text = "���~";

				thread = new Thread(new ThreadStart(OnUpdating));
				thread.Name = "BOARD_UPDATE";
				thread.IsBackground = true;
				thread.Start();
			}
			catch (ThreadAbortException) {}
			catch (Exception ex) {
				TwinDll.ShowOutput(ex);
			}
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			if (thread != null)
			{
				try
				{
					thread.Abort();
				}
				catch {}
				finally {thread = null;}
			}
			else {
				if (updated)
					DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}
