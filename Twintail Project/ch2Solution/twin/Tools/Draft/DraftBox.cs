// DraftBox.cs

namespace Twin.Tools
{
	using System;
	using System.Collections;
	using System.Xml;
	using System.Xml.XPath;
	using System.IO;

	/// <summary>
	/// ���������̃��b�Z�[�W���ꎞ�I�ɕۑ����Ă������e��
	/// </summary>
	public class DraftBox
	{
		private const string DraftFileName = "draft.txt";
		private Cache cache;

		/// <summary>
		/// DraftBox�N���X�̃C���X�^���X��������
		/// </summary>
		public DraftBox(Cache cache)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.cache = cache;
		}

		/// <summary>
		/// �w�肵�����e��xml�v�f���쐬
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="draft"></param>
		/// <returns></returns>
		private XmlNode CreateDraftElement(XmlDocument doc, Draft draft)
		{
			// �X���b�h�ԍ���\���������쐬
			XmlAttribute key = doc.CreateAttribute("key");
			key.Value = draft.HeaderInfo.Key;

			// ���e�Җ�
			XmlElement from = doc.CreateElement("from");
			from.AppendChild(doc.CreateCDataSection(draft.PostRes.From));

			// email
			XmlElement email = doc.CreateElement("email");
			email.AppendChild(doc.CreateCDataSection(draft.PostRes.Email));

			// �{��
			XmlElement body = doc.CreateElement("message");
			body.AppendChild(doc.CreateCDataSection(draft.PostRes.Body));

			// ���e�̗v�f���쐬���A�e�ɒǉ�
			XmlElement child = doc.CreateElement("thread");
			child.Attributes.Append(key);
			child.AppendChild(from);
			child.AppendChild(email);
			child.AppendChild(body);

			return child;
		}

		/// <summary>
		/// �w�肵���̑��e���폜
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="draft"></param>
		public void Remove(BoardInfo board, Draft draft)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (draft == null) {
				throw new ArgumentNullException("draft");
			}

			XmlDocument doc = new XmlDocument();
			XmlElement root;

			string filePath = cache.GetFolderPath(board);
			filePath = Path.Combine(filePath, DraftFileName);

			if (File.Exists(filePath))
			{
				doc.Load(filePath);
				root = doc.DocumentElement;

				// �����X���b�h�ԍ��������e�v�f���������邽�߂�XPath
				string xpath = String.Format("draft/thread[@key=\"{0}\"]", draft.HeaderInfo.Key);

				// ���ɓ������e�����݂����ꍇ�A��[�폜���Ă���V�����v�f��ǉ�
				XmlNode node = doc.SelectSingleNode(xpath);
				if (node != null)
					root.RemoveChild(node);

				doc.Save(filePath);
			}
		}

		/// <summary>
		/// ���݂̑��e���ɏ㏑���ۑ�
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="draft"></param>
		public void Save(BoardInfo board, Draft draft)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (draft == null) {
				throw new ArgumentNullException("draft");
			}

			XmlDocument doc = new XmlDocument();
			XmlElement root;

			string filePath = cache.GetFolderPath(board);
			filePath = Path.Combine(filePath, DraftFileName);

			// �t�@�C�������݂��Ȃ���΃��[�g�v�f���쐬���Ă���
			if (!File.Exists(filePath))
			{
				root = doc.CreateElement("draft");
				doc.AppendChild(root);
			}
			else {
				doc.Load(filePath);
				root = doc.DocumentElement;
			}

			// �����X���b�h�ԍ��������e�v�f���������邽�߂�XPath
			string xpath = String.Format("draft/thread[@key=\"{0}\"]", draft.HeaderInfo.Key);

			// �V�������e�̗v�f
			XmlNode newChild = CreateDraftElement(doc, draft);

			// ���ɓ������e�����݂����ꍇ�A��[�폜���Ă���V�����v�f��ǉ�
			XmlNode node = doc.SelectSingleNode(xpath);
			if (node != null)
				root.RemoveChild(node);

			root.AppendChild(newChild);
			doc.Save(filePath);
		}

		/// <summary>
		/// �w�肵���X���b�h�̑��e���擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public Draft Load(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			Draft draft = null;

			string filePath = cache.GetFolderPath(header.BoardInfo);
			filePath = Path.Combine(filePath, DraftFileName);

			if (File.Exists(filePath))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(filePath);
				
				// �����X���b�h�ԍ��������e�v�f���������邽�߂�XPath
				string xpath = String.Format("draft/thread[@key=\"{0}\"]", header.Key);
				XmlNode node = doc.SelectSingleNode(xpath);

				if (node != null && ThreadIndexer.Read(cache, header) != null)
				{
					string name = node.SelectSingleNode("from").InnerText;
					string email = node.SelectSingleNode("email").InnerText;
					string body = node.SelectSingleNode("message").InnerText;

					// ���e�����쐬
					PostRes res = new PostRes(name, email, body);
					draft = new Draft(header, res);
				}
			}

			return draft;
		}

		/// <summary>
		/// �w�肵���ɑ��݂��鑐�e���擾
		/// </summary>
		/// <param name="filePath"></param>
		public Draft[] Load(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			XmlDocument doc = new XmlDocument();
			ArrayList arrayList = new ArrayList();

			string filePath = cache.GetFolderPath(board);
			filePath = Path.Combine(filePath, DraftFileName);

			try {
				doc.Load(filePath);
				
				// ���e�v�f�����ׂĎ擾
				XmlNodeList nodeList = doc.SelectNodes("draft/thread");
				
				foreach (XmlNode node in nodeList)
				{					
					ThreadHeader header = TypeCreator.CreateThreadHeader(board.Bbs);
					header.BoardInfo = board;
					header.Key = node.Attributes.GetNamedItem("key").Value;

					if (ThreadIndexer.Read(cache, header) != null)
					{
						string name = node.SelectSingleNode("from").InnerText;
						string email = node.SelectSingleNode("email").InnerText;
						string body = node.SelectSingleNode("message").InnerText;

						// ���e�����쐬
						PostRes res = new PostRes(name, email, body);
						Draft draft = new Draft(header, res);
						arrayList.Add(draft);
					}
				}
			}
			catch (FileNotFoundException) {}

			return (Draft[])arrayList.ToArray(typeof(Draft));
		}
	}
}
