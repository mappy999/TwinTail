// ThreadListIndexer.cs
// #2.0

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Collections.Generic;
	using Twin.Util;
	using Twin.Text;

	/// <summary>
	/// �����X���b�h�ꗗ�̃C���f�b�N�X���쐬�E�Ǘ�����
	/// </summary>
	public class GotThreadListIndexer
	{
		/// <summary>
		/// �w�肵���̃C���f�b�N�X�ۑ���p�X���擾�܂��͐ݒ�
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		/// <returns></returns>
		public static string GetIndicesPath(Cache cache, BoardInfo board)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			string folderPath = cache.GetFolderPath(board, false);
			string listPath = Path.Combine(folderPath, "indices.txt");

			return listPath;
		}

		/// <summary>
		/// �w�肵���̊����C���f�b�N�X��ǂݍ���
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		/// <returns></returns>
		public static List<ThreadHeader> Read(Cache cache, BoardInfo board)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			List<ThreadHeader> items = new List<ThreadHeader>();
			string indicesPath = GetIndicesPath(cache, board);

			if (File.Exists(indicesPath))
			{
				XmlDocument document = new XmlDocument();

				lock (typeof(GotThreadListIndexer))
				{
					document.Load(indicesPath);
				}

				XmlElement root = document.DocumentElement;
				XmlNodeList children = root.ChildNodes;

				foreach (XmlNode node in children)
				{
					try
					{
						ThreadHeader header = TypeCreator.CreateThreadHeader(board.Bbs);
						header.BoardInfo = board;
						header.Key = node.Attributes.GetNamedItem("key").Value;
						header.Subject = node.SelectSingleNode("subject").InnerText;

						int resCount;
						if (Int32.TryParse(node.SelectSingleNode("resCount").InnerText, out resCount))
							header.ResCount = resCount;

						items.Add(header);
					}
					catch (Exception ex)
					{
						TwinDll.Output(ex);
					}
				}
			}

			return items;
		}

		/// <summary>
		/// �w�肵���̃C���f�b�N�X���쐬
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		/// <param name="items"></param>
		public static void Write(Cache cache, BoardInfo board, List<ThreadHeader> items)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			string indicesPath =
				GetIndicesPath(cache, board);

			// �w�b�_�������������ăt�@�C���ɕۑ�
			ThreadListFormatter formatter = new GotThreadListFormatter();

			lock (typeof(GotThreadListIndexer))
			{
				FileUtility.Write(indicesPath, formatter.Format(items), false, TwinDll.DefaultEncoding);
			}
		}

		/// <summary>
		/// �w�肵���X���b�h�̃C���f�b�N�X���폜
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header"></param>
		public static void Remove(Cache cache, ThreadHeader header)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			XmlDocument document = new XmlDocument();
			string indicesPath = GetIndicesPath(cache, header.BoardInfo);

			lock (typeof(GotThreadListIndexer))
			{
				if (File.Exists(indicesPath))
				{
					document.Load(indicesPath);
					XmlNode node = document.SelectSingleNode("indices/item[@key=\"" + header.Key + "\"]");

					if (node != null)
						document.DocumentElement.RemoveChild(node);

					// �P���v�f�������Ȃ�����t�@�C�����̂��폜
					if (document.DocumentElement.ChildNodes.Count == 0)
					{
						File.Delete(indicesPath);
					}
					else
					{
						XmlTextWriter writer = new XmlTextWriter(indicesPath, TwinDll.DefaultEncoding);
						writer.Formatting = Formatting.Indented;

						document.Save(writer);
						writer.Close();
					}
				}

			}
		}

		/// <summary>
		/// �w�肵���X���b�h�̃C���f�b�N�X���쐬
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header"></param>
		public static void Write(Cache cache, ThreadHeader header)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			XmlDocument document = new XmlDocument();
			string indicesPath = GetIndicesPath(cache, header.BoardInfo);
			bool overwrite = false;

			lock (typeof(GotThreadListIndexer))
			{
				if (File.Exists(indicesPath))
				{
					document.Load(indicesPath);
					XmlNode node = document.SelectSingleNode("indices/item[@key=\"" + header.Key + "\"]");
					if (node != null)
					{
						// ���X�����X�V
						node.SelectSingleNode("resCount").InnerText = header.ResCount.ToString();
						overwrite = true;
					}
				}
				else
				{
					// ���[�g���쐬
					document.AppendChild(
						document.CreateElement("indices"));
				}

				// ���݂��Ȃ���ΐV�����쐬
				if (!overwrite)
				{
					GotThreadListFormatter formatter = new GotThreadListFormatter();
					formatter.AppendChild(document, document.DocumentElement, header);
				}

				// �h�L�������g��ۑ�
				XmlTextWriter writer = new XmlTextWriter(indicesPath, TwinDll.DefaultEncoding);
				writer.Formatting = Formatting.Indented;

				document.Save(writer);
				writer.Close();
			}
		}

		internal static void Regeneration(Cache Cache)
		{
			throw new NotImplementedException();
		}
	}
}
