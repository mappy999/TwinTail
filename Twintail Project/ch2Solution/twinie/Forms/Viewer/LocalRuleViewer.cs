// LocalRuleViewer.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using System.Net;
	using System.IO;

	/// <summary>
	/// ���[�J�����[����\�����邽�߂̃r���[�A
	/// </summary>
	public class LocalRuleViewer : System.Windows.Forms.Form
	{
		private Thread thread;
		private BoardInfo boardInfo;
		private string tempFileName;

		private AxSHDocVw.AxWebBrowser axWebBrowser;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>
		/// LocalRuleViewer�N���X�̃C���X�^���X��������
		/// </summary>
		public LocalRuleViewer()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalRuleViewer));
			this.axWebBrowser = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).BeginInit();
			this.SuspendLayout();
			// 
			// axWebBrowser
			// 
			this.axWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser.Enabled = true;
			this.axWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser.Margin = new System.Windows.Forms.Padding(2);
			this.axWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser.OcxState")));
			this.axWebBrowser.Size = new System.Drawing.Size(534, 362);
			this.axWebBrowser.TabIndex = 0;
			// 
			// LocalRuleViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 362);
			this.Controls.Add(this.axWebBrowser);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "LocalRuleViewer";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "���[�J�����[��";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.LocalRuleViewer_Closing);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// ���[�J�����[���̎擾����
		/// </summary>
		private void Process()
		{

			string body = String.Empty;

			try
			{
				// ���[�J�����[�����_�E�����[�h
				using (WebClient client = new WebClient())
				{
					client.Headers.Add("User-Agent", Settings.UserAgent);
					byte[] buffer = client.DownloadData(boardInfo.Url + "head.txt");
					body = TwinDll.DefaultEncoding.GetString(buffer);
				}
			}
			catch (Exception ex)
			{
				body = ex.Message;
			}

			// �ꎞ�t�@�C�����쐬
			tempFileName = Path.ChangeExtension(Path.GetTempFileName(), ".html");

			using (StreamWriter w = new StreamWriter(tempFileName, false, TwinDll.DefaultEncoding))
			{
				w.Write("<html><head><base href=\"{0}\" /></head><body>{1}</body></html>",
					boardInfo.Url, body);
			}

			// �J��
			Invoke(new MethodInvoker(OpenLocalRule));

		}

		/// <summary>
		/// Web�u���E�U�ŊJ��
		/// </summary>
		private void OpenLocalRule()
		{
			object o = null;
			axWebBrowser.Navigate(tempFileName, ref o, ref o, ref o, ref o);
		}

		private void LocalRuleViewer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// �X���b�h�𒆎~
			if (thread != null && thread.IsAlive)
				thread.Abort();

			// �쐬�����ꎞ�t�@�C�����폜
			if (File.Exists(tempFileName))
				File.Delete(tempFileName);
		}

		/// <summary>
		/// �R���g���[����\������Ɠ����Ƀ��[�J�����[�����\��
		/// </summary>
		/// <param name="board"></param>
		public void Show(BoardInfo board)
		{
			Show();
			Open(board);
		}

		/// <summary>
		/// �w�肵���̃��[�J�����[����\��
		/// </summary>
		/// <param name="board"></param>
		public void Open(BoardInfo board)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			if (thread != null && thread.IsAlive)
				thread.Abort();

			// �^�C�g����ݒ�
			this.Text = String.Format("���[�J�����[�� [{0}��]", board.Name);
			this.boardInfo = board;

			if (File.Exists(tempFileName))
				File.Delete(tempFileName);

			thread = new Thread(new ThreadStart(Process));
			thread.Name = "LOCAL_RURLE_VIEW";
			thread.IsBackground = true;
			thread.Start();
		}
	}
}
