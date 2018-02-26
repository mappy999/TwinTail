// BoardLinker.cs

namespace Twin
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.IO;
	using System.Xml;
	using Twin.IO;

	/// <summary>
	/// �Ɣ������N���֘A�Â���N���X
	/// </summary>
	public class BoardLinker
	{
		private Cache cache;

		/// <summary>
		/// BoardLinker�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		public BoardLinker(Cache cache)
		{
			if (cache == null)
				throw new ArgumentNullException("cache");

			this.cache = cache;
		}

		/// <summary>
		/// ���O�̈ړ]�ƁA�Â��Ɉړ]�������c��
		/// </summary>
		/// <param name="oldBoard�ړ]���̔�</param>
		/// <param name="newBoard">�ړ]��̔�</param>
		public void Replace(BoardInfo oldBoard, BoardInfo newBoard)
		{
			if (oldBoard == null) {
				throw new ArgumentNullException("oldBoard");
			}
			if (newBoard == null) {
				throw new ArgumentNullException("newBoard");
			}

			// dat�����X���Ɛ����Ă���X���𕪗�
			//List<ThreadHeader> leaveItems, liveItems;
			//Separate(oldBoard, newBoard, out leaveItems, out liveItems);

			//// �����C���f�b�N�X���X�V
			//GotThreadListIndexer.Write(cache, oldBoard, leaveItems);
			//GotThreadListIndexer.Write(cache, newBoard, liveItems);

			ThreadIndexer.Indexing(cache, newBoard);
/*
			// �ړ]�O�̊������O���擾
			List<ThreadHeader> gotItems = GotThreadListIndexer.Read(cache, oldBoard);
			
			// �V�������ɕύX
			foreach (ThreadHeader h in gotItems)
				h.BoardInfo = newBoard;

			// �V�����Ɋ����C���f�b�N�X���쐬
			GotThreadListIndexer.Write(cache, newBoard, gotItems);

			// ���O���ړ�
			CopyDatFiles(oldBoard, newBoard);
*/
		}


		/// <summary>
		/// �����Ă���X����dat�������Ă���X���𕪗�
		/// </summary>
		/// <param name="oldBoard">���O�����݂����</param>
		/// <param name="newBoard">�����Ă���X���̈ړ]��</param>
		/// <param name="leaveItems">dat�������Ă���X�����i�[�����</param>
		/// <param name="liveItems">�����Ă���X�����i�[�����</param>
		private void Separate(BoardInfo oldBoard, BoardInfo newBoard,
			out List<ThreadHeader> leaveItems, out List<ThreadHeader> liveItems)
		{
			leaveItems = new List<ThreadHeader>();
			liveItems = new List<ThreadHeader>();

			if (cache.Exists(oldBoard))
			{
				List<ThreadHeader> oldItems = GotThreadListIndexer.Read(cache, oldBoard);
				List<ThreadHeader> newItems = new List<ThreadHeader>();
				ThreadListReader listReader = null;

				if (oldItems.Count > 0)
				{
					try {
						listReader = TypeCreator.CreateThreadListReader(newBoard.Bbs);
						listReader.Open(newBoard);

						while (listReader.Read(newItems) != 0);

						// �ړ]��̃X���ꗗ�ɑ��݂��郍�O�݈̂ړ] (dat�������Ă���X���͈ړ]���Ȃ�)
						foreach (ThreadHeader header in oldItems)
						{
							Predicate<ThreadHeader> p = new Predicate<ThreadHeader>(delegate (ThreadHeader h)
							{
								return h.Key == header.Key;
							});

							if (newItems.Exists(p))
							{
								// �����Ă���X���̔����ړ]��ɏ���������
								if  (ThreadIndexer.Read(cache, header) != null)
								{
									ThreadIndexer.Delete(cache, header);

									liveItems.Add(header);
									header.BoardInfo = newBoard;

									ThreadIndexer.Write(cache, header);
								}
							}
							else {
								leaveItems.Add(header);
							}
						}
					}
					finally {
						if (listReader != null)
							listReader.Close();
					}
				}
			}
		}

		/// <summary>
		/// oldBoard����newBoard�Ƀ��O���ړ�
		/// </summary>
		/// <param name="oldItems"></param>
		/// <param name="newBoard"></param>
		private void CopyDatFiles(BoardInfo oldItems, BoardInfo newBoard)
		{
			string fromFolder = cache.GetFolderPath(oldItems);
			string toFolder = cache.GetFolderPath(newBoard, true);

			string[] fileNames = Directory.GetFiles(fromFolder, "*.dat*");// .dat .dat.gz ������

			foreach (string fromPath in fileNames)
			{
				try {
					string fromName = Path.GetFileName(fromPath);
					string destPath = Path.Combine(toFolder, fromName);

					File.Copy(fromPath, destPath, true);
					File.Delete(fromPath);

					int token = fromName.IndexOf('.');
					if (token != -1)
					{
						string key = fromName.Substring(0, token);
						string fromIndexFile = Path.Combine(fromFolder, key + ".idx");
						string toIndexFile = Path.Combine(toFolder, key + ".idx");

						ThreadHeader h = ThreadIndexer.Read(fromIndexFile);

						if (h != null)
						{
							h.BoardInfo.Server = newBoard.Server;

							ThreadIndexer.Write(toIndexFile, h);

							File.Delete(fromIndexFile);
						}
					}
				}
				catch (IOException ex) 
				{
					TwinDll.Output(ex);
				}
			}
		}

//		/// <summary>
//		/// board�Ƀ����N�����쐬
//		/// </summary>
//		/// <param name="cache"></param>
//		/// <param name="board"></param>
//		private void CreateLinkInfo(Cache cache, BoardInfo oldBoard, BoardInfo newBoard)
//		{
//			try {
//				// �ړ]���Ƀ����N�����c��
//				string filePath = 
//					Path.Combine(cache.GetFolderPath(oldBoard), "moved.txt");
//
//				XmlDocument doc = new XmlDocument();
//				XmlNode root = doc.CreateElement("Link");
//					
//				XmlAttribute serv = doc.CreateAttribute("Server");
//				serv.Value = newBoard.Server;
//
//				XmlAttribute path = doc.CreateAttribute("Path");
//				path.Value = newBoard.Path;
//
//				XmlNode node = doc.CreateElement("Item");
//				node.Attributes.Append(serv);
//				node.Attributes.Append(path);
//				node.InnerText = newBoard.Name;
//				root.AppendChild(node);
//
//				doc.AppendChild(root);
//				doc.Save(filePath);
//			}
//			catch (Exception ex) {
//				throw new ApplicationException("�����N���̍쐬�Ɏ��s���܂���", ex);
//			}
//		}

//		/// <summary>
//		/// �w�肵���������N����Ă�����擾
//		/// </summary>
//		/// <param name="board"></param>
//		/// <param name="recursive"></param>
//		/// <returns></returns>
//		public BoardInfo GetLinked(BoardInfo board, bool recursive)
//		{
//			if (board == null) {
//				throw new ArgumentNullException("board");
//			}
//
//			string filePath = 
//				Path.Combine(cache.GetFolderPath(board), "moved.txt");
//
//			if (File.Exists(filePath))
//			{
//				XmlDocument doc = new XmlDocument();
//				doc.Load(filePath);
//
//				XmlNode node = doc.SelectSingleNode("link/item");
//
//				BoardInfo result = new BoardInfo(
//					node.Attributes["Server"].Value, 
//					node.Attributes["Path"].Value,
//					node.InnerText);
//
//				if (recursive)
//				{
//					BoardInfo sub = GetLinked(result, true);
//					if (sub != null) result = sub;
//				}
//
//				return result;
//			}
//			return null;
//		}
	}
}
