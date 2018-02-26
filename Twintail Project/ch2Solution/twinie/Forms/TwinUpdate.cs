
namespace Twin.Forms
{
	using System;
	using System.Net;
	using System.Xml;
	using System.Windows.Forms;
	using System.Text;
	using System.Threading;
	using System.Diagnostics;

	/// <summary>
	/// TwinUpdate �̊T�v�̐����ł��B
	/// </summary>
	public class TwinUpdate
	{
		public static void Check()
		{
			Thread thread = new Thread(new ThreadStart(CheckInternal));
			thread.Name = "UPDATE_CHECK";
			thread.IsBackground = true;
			thread.Start();
		}

		private static void CheckInternal()
		{
			try {
			// �X�V���̃t�@�C�����擾
			WebClient webClient = new WebClient();
			byte[] data = webClient.DownloadData(Settings.UpdateInfoUri);
			string text = Encoding.GetEncoding("Shift_Jis").GetString(data);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(text);

			XmlNode root = doc.DocumentElement;

			// �o�[�W�������V������΍X�V���b�Z�[�W���o��
			Version newver = new Version(root.SelectSingleNode("Version").InnerText);

			if (Twinie.Version < newver)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("����ā[�邪�X�V����Ă��܂�\r\n");
				sb.AppendFormat("�o�[�W����: {0}\r\n\r\n", newver);
				sb.Append(root.SelectSingleNode("Information").InnerText);
				sb.Append("\r\n�V�����o�[�W�����ɍX�V���܂����H");

				// �X�V�m�F�̃_�C�A���O��\��
				DialogResult r = MessageBox.Show(sb.ToString(), "�V�����o�[�W�����̊m�F",
					MessageBoxButtons.YesNo, MessageBoxIcon.Information);

				if (r == DialogResult.Yes)
					Process.Start(Settings.WebSiteUrl);
			}
			}catch{}
		}
	}
}
