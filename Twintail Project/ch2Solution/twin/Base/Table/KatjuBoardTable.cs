// KatjuBoardTable.cs
// #2.0

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using Twin.Text;
	using Twin.Tools;
	using System.Threading;

	/// <summary>
	/// ������`����݊� (2channel.brd�`��) �{�[�h�e�[�u��
	/// </summary>
	public class KatjuBoardTable : IBoardTable
	{
		private List<Category> items = new List<Category>();

		public List<Category> Items
		{
			get
			{
				return items;
			}
		}

		public KatjuBoardTable()
		{
		}

		public void Add(IBoardTable table)
		{
			if (table == null)
			{
				throw new ArgumentNullException("table");
			}

			foreach (Category cate in table.Items)
				items.Add(cate);
		}

		public void Clear()
		{
			items.Clear();
		}

		/// <summary>
		/// �I�����C���Ŕꗗ���X�V ([BBS MENU for 2ch]�ɑΉ�)
		/// </summary>
		/// <param name="url">�X�V��URL</param>
		/// <param name="callback">���ړ]���Ă����ꍇ�ɌĂ΂��R�[���o�b�N</param>
		public void OnlineUpdate(string url, BoardUpdateEventHandler callback)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.Headers.Add("Pragma", "no-cache");
			req.Headers.Add("Cache-Control", "no-cache");

			HttpWebResponse res = (HttpWebResponse)req.GetResponse();

			try
			{
				IBoardTable newTable = new KatjuBoardTable();
				string htmlData;

				using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("Shift_Jis")))
					htmlData = sr.ReadToEnd();

				res.Close();
				res = null;

				// 2012/12/05 Mizutama����
				// ���𒊏o
				// ��BR����BR����B���J�e�S������/B����BR��
				// ��A HREF=http://[�T�[�o�[]/[��]/�����O��/A��
				MatchCollection cats = Regex.Matches
									   (
										 htmlData,
										 @"<BR><BR><B>(?<cat>.+?)</B><BR>(?<brds>.+?)(?=\<BR\>\<BR\>\<B\>)",
										 RegexOptions.Singleline | RegexOptions.IgnoreCase
									   );
				foreach (Match m in cats)
				{
					Category category = new Category(m.Groups["cat"].Value);

					MatchCollection brds = Regex.Matches
										   (
											 m.Groups["brds"].Value,
											 @"<A HREF=(?<url>[^\s>]+).*?>(?<subj>.+?)</A>",
											 RegexOptions.Singleline | RegexOptions.IgnoreCase
											);
					foreach (Match matchBrd in brds)
					{
						// �{�[�h�����쐬
						BoardInfo newBoard = URLParser.ParseBoard(matchBrd.Groups["url"].Value);
						if (newBoard != null)
						{
							newBoard.Name = matchBrd.Groups["subj"].Value;
							category.Children.Add(newBoard);

							if (callback != null)
							{
								// �V���ړ]�`�F�b�N
								BoardInfo old = FromName(newBoard.Name, newBoard.DomainPath);
								BoardUpdateEventArgs args = null;

								// ������Ȃ���ΐV�Ɣ��f
								if (old == null)
								{
									args = new BoardUpdateEventArgs(BoardUpdateEvent.New, null, newBoard);
								}
								// ������������URL���Ⴄ�ꍇ�͈ړ]�Ɣ��f
								else if (old.Server != newBoard.Server)
								{
									args = new BoardUpdateEventArgs(BoardUpdateEvent.Change, old, newBoard);
								}

								if (args != null)
									callback(this, args);
							}
						}
					}

					if (category.Children.Count > 0)
					{
						newTable.Items.Add(category);
					}
				}

				if (newTable.Items.Count > 0)
				{
					// �V�����ꗗ��ݒ�
					Items.Clear();
					Items.AddRange(newTable.Items);
				}
				else
				{
					throw new ApplicationException("�ꗗ�̍X�V�Ɏ��s���܂���");
				}
			}
			catch (ThreadAbortException)
			{
				if (callback != null)
					callback(this, new BoardUpdateEventArgs(BoardUpdateEvent.Cancelled, null, null));
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
			finally
			{
				if (res != null)
					res.Close();
			}
		}

		public void SaveTable(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName", "fileName��null�Q�Ƃł�");
			}

			StreamWriter sw = null;

			try
			{
				sw = new StreamWriter(fileName, false, TwinDll.DefaultEncoding);
				sw.WriteLine("2");

				foreach (Category cate in items)
				{
					sw.WriteLine("{0}\t{1}",
						cate.Name, cate.IsExpanded ? 1 : 0);

					foreach (BoardInfo board in cate.Children)
					{
						sw.WriteLine("\t{0}\t{1}\t{2}",
							board.Server, board.Path, board.Name);
					}
				}
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// 2�����˂�{�[�h�e�[�u��(2channel.brd�^��)��ǂݍ���
		/// </summary>
		/// <param name="fileName">�ǂݍ��ރt�@�C����</param>
		/// <exception cref="System.ArgumentNullException">fileName��null�Q�Ƃł�</exception>
		/// <exception cref="System.IO.FileNotFoundException">fileName�͑��݂��܂���</exception>
		public void LoadTable(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName", "fileName��null�Q�Ƃł�");
			}

			StreamReader sr = null;
			Category category = null;
			string text;

			try
			{
				sr = new StreamReader(fileName, TwinDll.DefaultEncoding);

				while ((text = sr.ReadLine()) != null)
				{
					string[] elems = text.Split('\t');

					if (elems.Length < 2)
					{

					}
					// �J�e�S���J�n
					else if (elems[0] != String.Empty)
					{
						int expandedBool;

						category = new Category(elems[0]);
						if (Int32.TryParse(elems[1], out expandedBool))
							category.IsExpanded = Convert.ToBoolean(expandedBool);
						items.Add(category);
					}
					// �{�[�h�ǉ�
					else if (elems.Length >= 4)
					{
						//if (BoardInfo.IsSupport(elems[1]))
						{
							string serv = elems[1];
							string dir = elems[2];
							string name = elems[3];
							BoardInfo item = new BoardInfo(serv, dir, name);
							category.Children.Add(item);
						}
					}
				}
			}
			finally
			{
				if (sr != null)
					sr.Close();
			}
		}

		public void Replace(BoardInfo oldBoard, BoardInfo newBoard)
		{
			if (oldBoard == null)
			{
				throw new ArgumentNullException("oldBoard");
			}
			if (newBoard == null)
			{
				throw new ArgumentNullException("newBoard");
			}

			foreach (Category cate in items)
			{
				int index = cate.Children.IndexOf(oldBoard);

				if (index != -1)
				{
					BoardInfo a = (BoardInfo)cate.Children[index];
					a.Path = newBoard.Path;
					a.Server = newBoard.Server;
				}
			}
		}

		public bool Contains(BoardInfo board)
		{
			if (board == null)
			{
				throw new ArgumentNullException("board");
			}

			return Items.Exists(new Predicate<Category>(delegate(Category category)
			{
				return category.Children.Contains(board);
			}));
		}

		/// <summary>
		/// �ꗗ�̒�����w�肵�� URL ���������������܂��B
		/// </summary>
		/// <param name="url"></param>
		/// <returns>��v��������Ԃ��܂��B������Ȃ���� null �ł��B</returns>
		public BoardInfo FromUrl(string url)
		{
			foreach (Category category in Items)
			{
				int index = category.Children.IndexOfUrl(url);

				if (index >= 0)
				{
					return category.Children[index];
				}
			}
			return null;
		}

		/// <summary>
		/// �w�肵��������������
		/// </summary>
		/// <param name="name"></param>
		/// <param name="domainPath"></param>
		/// <returns>������Ȃ����null��Ԃ�</returns>
		public BoardInfo FromName(string name, string domainPath)
		{
			foreach (Category cate in Items)
			{
				int index = cate.Children.IndexOfName(name);
				if (index >= 0)
				{
					BoardInfo brd = cate.Children[index];
					if (brd.DomainPath.Equals(domainPath))
						return brd;
				}
			}
			return null;
		}

		public BoardInfo[] ToArray()
		{
			List<BoardInfo> list = new List<BoardInfo>();

			foreach (Category cate in items)
			{
				foreach (BoardInfo board in cate.Children)
				{
					list.Add(board);
				}
			}

			return list.ToArray();
		}
	}
}
