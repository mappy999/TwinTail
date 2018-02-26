// BoardPictureViewer.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Threading;
	using System.Text.RegularExpressions;
	using System.Net;
	using System.IO;
	using Twin.Tools;

	/// <summary>
	/// �Q�����˂�̊Ŕ�\������r���[�A
	/// </summary>
	public class BoardPictureViewer : System.Windows.Forms.Form
	{
		private delegate void SetImageMethodInvoker(Image image);

		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ImageViewerDll.ImagePanel imagePanel1;
		private X2chServerSetting x2chSettings;
		private Thread thread;
		private BoardInfo board;
		private bool gif;

		/// <summary>
		/// BoardPictureViewer�N���X�̃C���X�^���X��������
		/// </summary>
		public BoardPictureViewer()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			x2chSettings = new X2chServerSetting();
			imagePanel1.Mosaic = false;
			gif = false;
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
			this.imagePanel1 = new ImageViewerDll.ImagePanel();
			this.SuspendLayout();
			// 
			// imagePanel1
			// 
			this.imagePanel1.BackColor = System.Drawing.Color.White;
			this.imagePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imagePanel1.IsOriginalSize = false;
			this.imagePanel1.Location = new System.Drawing.Point(0, 0);
			this.imagePanel1.Mosaic = false;
			this.imagePanel1.Name = "imagePanel1";
			this.imagePanel1.Size = new System.Drawing.Size(411, 176);
			this.imagePanel1.TabIndex = 0;
			this.imagePanel1.TabStop = false;
			this.imagePanel1.Text = "imagePanel1";
			// 
			// BoardPictureViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(411, 176);
			this.Controls.Add(this.imagePanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "BoardPictureViewer";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "�Ŕ� (�摜���_�E�����[�h���ł�...)";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BoardPictureViewer_Closing);
			this.Load += new System.EventHandler(this.BoardPictureViewer_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.BoardPictureViewer_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		private void Downloading()
		{
			Image image = null;
			try {
				x2chSettings.Download(board);
				WebClient webClient = new WebClient();
				byte[] data = webClient.DownloadData(x2chSettings.TitlePicture);

				MemoryStream memory = new MemoryStream();
				memory.Write(data, 0, data.Length);
				memory.Position = 0;

				gif = Regex.IsMatch(x2chSettings.TitlePicture, "\\.gif$",
					RegexOptions.IgnoreCase);

				image = Image.FromStream(memory);
			}
			catch (Exception ex)
			{
				image = new Bitmap(300, 50);
				StringFormat format = new StringFormat();

				using (Graphics g = Graphics.FromImage(image))
				{
					g.DrawString(ex.Message, this.Font, Brushes.Black, 
						new Rectangle(0, 0, image.Width, image.Height), format);
				}
			}
	
			Invoke(new SetImageMethodInvoker(SetImage), new object[] {image});
		}

		private void SetImage(Image image)
		{
			if (image == null)
				return;

			imagePanel1.Load(image);

			Screen sc = Screen.PrimaryScreen;
			Size imgSize = imagePanel1.Image.Size;
			
			this.Top = (sc.Bounds.Height / 2) - (imgSize.Height / 2);
			this.Left = (sc.Bounds.Width / 2) - (imgSize.Width / 2);
			this.ClientSize = imgSize;

			if (imagePanel1.CanAnimate) 
				imagePanel1.Animate();

			this.Text = "�Ŕ� (�ǂݍ��݊���)";
		}

		private void BoardPictureViewer_Load(object sender, System.EventArgs e)
		{

		}

		private void BoardPictureViewer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (thread != null && thread.IsAlive)
				thread.Abort();

			imagePanel1.Unload();
		}

		private void BoardPictureViewer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		}

		/// <summary>
		/// �R���g���[����\�����A�w�肵���Ŕ��J��
		/// </summary>
		/// <param name="info"></param>
		public void Show(BoardInfo info)
		{
			Show();
			Open(info);
		}

		/// <summary>
		/// �w�肵���̊Ŕ��J��
		/// </summary>
		public void Open(BoardInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			if (thread != null && thread.IsAlive)
				thread.Abort();

			this.imagePanel1.Unload();
			this.board = info;
			this.Text = String.Format("�Ŕ� [{0}��] �_�E�����[�h��...", board.Name);

			thread = new Thread(new ThreadStart(Downloading));
			thread.Name = "BOARD_PIC_VIEW";
			thread.IsBackground = true;
			thread.Start();
		}

	}
}
