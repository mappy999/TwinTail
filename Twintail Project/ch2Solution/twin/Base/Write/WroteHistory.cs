// WroteHistory.cs

namespace Twin
{
	using System;
	using System.Text;
	using System.IO;
	using System.Xml;
	using System.Xml.XPath;
	using System.Collections;

	/// <summary>
	/// �������ݗ�����\��
	/// </summary>
	public class WroteHistory
	{
		private Cache cache;

		/// <summary>
		/// WroteHistory�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache">�����̊�{�L���b�V������\��</param>
		public WroteHistory(Cache cache)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			if (cache == null)
				throw new ArgumentNullException("cache");

			this.cache = cache;
		}

		/// <summary>
		/// �w�肵���X���b�h�̗�����ǂݍ���
		/// </summary>
		/// <param name="board"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public WroteThread Load(BoardInfo board, string key)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (key == null) {
				throw new ArgumentNullException("key");
			}

			string path = GetHistoryPath(board, key);
			return Load(path);
		}

		/// <summary>
		/// �w�肵���X���b�h�̗�����ǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public WroteThread Load(string filePath)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}

			XmlDocument document = null;
			WroteThread wroteThread = null;

			try {
				if (File.Exists(filePath))
				{
					document = new XmlDocument();
					document.Load(filePath);
					wroteThread = new WroteThread();				
					
					XmlNode parent = document.DocumentElement.FirstChild;
					wroteThread.Key = parent.Attributes.GetNamedItem("key").Value;

					XmlNode subj = document.SelectSingleNode("kakikomi/thread/subject");
					wroteThread.Subject = subj.InnerText;

					XmlNode uri = document.SelectSingleNode("kakikomi/thread/url");
					wroteThread.Uri = new Uri(uri.InnerText);

					XmlNodeList resList = document.SelectNodes("kakikomi/thread/resCollection/res");
					foreach (XmlNode resNode in resList)
					{
						WroteRes item = new WroteRes();
						XmlNode date = resNode.Attributes.GetNamedItem("date");
						item.Date = DateTime.Parse(date.Value);
						item.From = resNode.SelectSingleNode("from").InnerText;
						item.Email = resNode.SelectSingleNode("email").InnerText;
						item.Message = resNode.SelectSingleNode("message").InnerText;
						wroteThread.ResItems.Add(item);
					}
				}
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}

			return wroteThread;
		}

		/// <summary>
		/// �w�肵���X���b�h�̗�������ۑ�
		/// </summary>
		/// <param name="header"></param>
		/// <returns>���𐔂�Ԃ�</returns>
		public int Save(ThreadHeader header, WroteRes res)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			if (res == null) {
				throw new ArgumentNullException("res");
			}

			// �������ݗ����̃t�@�C���p�X
			string filePath = GetHistoryPath(header.BoardInfo, header.Key);
			int wroteCount = 0;

			try {
				XmlDocument document = new XmlDocument();
				XmlNode root = null, parent = null;

				if (File.Exists(filePath))
				{
					// ���ɗ��������݂���΃t�@�C������ǂݍ���
					document.Load(filePath);
					root = document.DocumentElement;
					parent = document.SelectSingleNode("kakikomi/thread");
				}
				else {
					// ���[�g�v�f���쐬
					root = document.CreateElement("kakikomi");					
					document.AppendChild(root);

					XmlAttribute attr = document.CreateAttribute("key");
					attr.Value = header.Key.ToString();

					parent = document.CreateElement("thread");
					parent.Attributes.Append(attr);
					root.AppendChild(parent);

					// �X���b�h��URL
					XmlElement urlElement = document.CreateElement("url");
					urlElement.InnerText = header.Url;
					parent.AppendChild(urlElement);

					// �X���b�h��
					XmlElement subjElement = document.CreateElement("subject");
					subjElement.AppendChild(document.CreateCDataSection(header.Subject));
					parent.AppendChild(subjElement);
				}

				// <resCollection>
				XmlNode resCollect = document.SelectSingleNode("kakikomi/thread/resCollection");
				if (resCollect == null) {
					resCollect = document.CreateElement("resCollection");
					parent.AppendChild(resCollect);
				}

				// �ǉ�����郌�X�v�f
				XmlElement resChild = document.CreateElement("res");

				// ���t
				XmlAttribute date = document.CreateAttribute("date");
				date.Value = res.Date.ToString();
				resChild.Attributes.Append(date);

				// ���e�Җ�
				XmlElement from = document.CreateElement("from");
				from.AppendChild(document.CreateCDataSection(res.From));
				resChild.AppendChild(from);

				// ���[���A�h���X
				XmlElement email = document.CreateElement("email");
				email.AppendChild(document.CreateCDataSection(res.Email));
				resChild.AppendChild(email);

				// ���b�Z�[�W�{��
				XmlElement message = document.CreateElement("message");
				message.AppendChild(document.CreateCDataSection(res.Message));
				resChild.AppendChild(message);

				// ���X�v�f��ǉ�
				resCollect.AppendChild(resChild);

				// ���𐔂��擾
				wroteCount = resCollect.ChildNodes.Count;

				// �h�L�������g��ۑ�
				XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 4;
				document.Save(writer);
				writer.Close();
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}

			return wroteCount;
		}

		/// <summary>
		/// �w�肵���̃X���b�h���������ׂĎ擾
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public WroteThread[] Load(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			string folder = cache.GetFolderPath(board);
			ArrayList list = new ArrayList();

			if (Directory.Exists(folder))
			{
				string[] komiFiles = Directory.GetFiles(folder, "*.komi");

				foreach (string komi in komiFiles)
				{
					WroteThread wt = Load(komi);
					if (wt != null)
						list.Add(wt);
				}
			}

			return (WroteThread[])list.ToArray(typeof(WroteThread));
		}

		/// <summary>
		/// �w�肵���̎w�肵�������ւ̃t�@�C���p�X���擾
		/// </summary>
		/// <param name="board"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetHistoryPath(BoardInfo board, string key)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (key == "") {
				throw new ArgumentException("key");
			}

			string folder = cache.GetFolderPath(board);
			string filePath = Path.Combine(folder, key + ".komi");

			return filePath;
		}

		/// <summary>
		/// �w�肵���̏������ݗ��������ׂč폜
		/// </summary>
		/// <param name="board"></param>
		public void Remove(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			string folder = cache.GetFolderPath(board, false);
			if (Directory.Exists(folder))
			{
				string[] files = Directory.GetFiles(folder, "*.komi");
				foreach (string f in files)
					File.Delete(f);
			}
		}

		/// <summary>
		/// ���ׂĂ̗������폜
		/// </summary>
		public void Clear()
		{
			string[] dirs = 
				Directory.GetDirectories(cache.BaseDirectory);

			foreach (string d in dirs)
			{
				string[] files = Directory.GetFiles(d, "*.komi");
				foreach (string f in files)
					File.Delete(f);
			}
		}
	}
}
