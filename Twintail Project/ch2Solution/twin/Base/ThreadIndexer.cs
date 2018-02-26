// ThreadIndexer.cs
// #2.0

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Diagnostics;
	using System.Collections.Generic;
	using CSharpSamples;
	using Twin.IO;

	/// <summary>
	/// �X���b�h�̃C���f�b�N�X�����Ǘ��E�쐬
	/// </summary>
	public class ThreadIndexer
	{

		public static void SavePastlog(Cache cache, ThreadHeader header, bool newValue)
		{
			SaveValue(cache, header, "Option", "Pastlog", newValue);
		}

		public static void SaveServerInfo(Cache cache, ThreadHeader header)
		{
			SaveValue(cache, header, "Board", "Server", header.BoardInfo.Server);
		}

		public static void SaveSirusi(Cache cache, ThreadHeader header)
		{
			SaveValue(cache, header, "Option", "Sirusi", header.Sirusi.ToArrayString());
		}

		public static void SaveBookmark(Cache cache, ThreadHeader header)
		{
			SaveValue(cache, header, "Option", "Shiori", header.Shiori);
		}

		public static void SavePosition(Cache cache, ThreadHeader header)
		{
			SaveValue(cache, header, "Option", "Position", header.Position);
		}

		public static void SaveLastWritten(Cache cache, ThreadHeader header)
		{
			SaveValue(cache, header, "Thread", "LastWritten", header.LastWritten);
		}

		public static void IncrementRefCount(Cache cache, ThreadHeader header)
		{
			string filePath = cache.GetIndexPath(header);
			int refCount = 0;

			if (File.Exists(filePath))
			{
				refCount = CSPrivateProfile.GetInt("Option", "RefCount", 0, filePath);
			}

			SaveValue(cache, header, "Option", "RefCount", ++refCount);
		}

		private static void SaveValue(Cache cache, ThreadHeader header,
			string section, string key, object value)
		{
			lock (typeof(ThreadIndexer))
			{
				string filePath = cache.GetIndexPath(header);

				if (File.Exists(filePath))
				{
					CSPrivateProfile.SetValue(section, key,
						value, filePath);
				}
			}
		}

		/// <summary>
		/// �X���b�h�̊��������L�^����C���f�b�N�X���쐬���܂��B
		/// </summary>
		/// <param name="filePath">�쐬����C���f�b�N�X���ւ̃p�X</param>
		/// <param name="header">�쐬����C���f�b�N�X��񂪊i�[���ꂽThreadHeader�N���X</param>
		/// <returns>�쐬�ɐ��������true�A���s�����false��Ԃ�</returns>
		public static bool Write(string filePath, ThreadHeader header)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}

			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			lock (typeof(ThreadIndexer))
			{
				CSPrivateProfile profile = new CSPrivateProfile();

				// ���
				profile.SetValue("Board", "Server", header.BoardInfo.Server);
				profile.SetValue("Board", "Path", header.BoardInfo.Path);
				profile.SetValue("Board", "Name", header.BoardInfo.Name);
				profile.SetValue("Board", "BBS", header.BoardInfo.Bbs);

				// �X���b�h���
				profile.SetValue("Thread", "ETag", header.ETag);
				profile.SetValue("Thread", "LastModified", header.LastModified);
				profile.SetValue("Thread", "LastWritten", header.LastWritten);
				profile.SetValue("Thread", "Subject", header.Subject);
				profile.SetValue("Thread", "ResCount", header.ResCount);
				profile.SetValue("Thread", "GotResCount", header.GotResCount);
				profile.SetValue("Thread", "GotByteCount", header.GotByteCount);
				profile.SetValue("Thread", "NewResCount", header.NewResCount);
				profile.SetValue("Thread", "Key", header.Key);

				// �g�����
				profile.SetValue("Option", "UseGzip", header.UseGzip);
				profile.SetValue("Option", "Pastlog", header.Pastlog);
				profile.SetValue("Option", "Position", header.Position);
				profile.SetValue("Option", "Shiori", header.Shiori);
				profile.SetValue("Option", "RefCount", header.RefCount);
				profile.SetValue("Option", "Sirusi", header.Sirusi.ToArrayString());

				profile.Write(filePath);
			}

			return true;
		}

		/// <summary>
		/// �C���f�b�N�X���쐬
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header">�쐬����C���f�b�N�X��񂪊i�[���ꂽThreadHeader�N���X</param>
		/// <returns>�쐬�ɐ��������true�A���s�����false��Ԃ�</returns>
		public static bool Write(Cache cache, ThreadHeader header)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}

			string filePath = cache.GetIndexPath(header);

			// �f�B���N�g�������݂��Ȃ���΍쐬
			string dir = Path.GetDirectoryName(filePath);

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			return Write(filePath, header);
		}

		/// <summary>
		/// �C���f�b�N�X��ǂݍ���
		/// </summary>
		/// <param name="header">��{�I�ȏ�񂪊i�[���ꂽThreadHeader</param>
		/// <returns>�ǂݍ��݂ɐ��������ThreadHeader�̃C���X�^���X�A���s�����null</returns>
		public static ThreadHeader Read(Cache cache, ThreadHeader header)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}

			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			// �C���f�b�N�X�t�@�C���ւ̃p�X
			string filePath = cache.GetIndexPath(header);
			ThreadHeader result = Read(filePath);

			if (result != null)
			{
				// �Q�Ƃ͂��̂܂܂Œl�������R�s�[����
				result.Tag = header.Tag;
				result.CopyTo(header);
			}
			else
			{
				header = null;
			}

			return header;
		}

		/// <summary>
		/// �C���f�b�N�X��ǂݍ���
		/// </summary>
		/// <param name="filePath">�C���f�b�N�X�t�@�C���ւ̃t�@�C���p�X</param>
		/// <returns>�ǂݍ��݂ɐ��������ThreadHeader�̃C���X�^���X�A���s�����null</returns>
		public static ThreadHeader Read(string filePath)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}

			ThreadHeader result = null;

			lock (typeof(ThreadIndexer))
			{
				// �C���f�b�N�X�t�@�C���ւ̃p�X

				if (File.Exists(filePath))
				{
					try
					{
						CSPrivateProfile profile = new CSPrivateProfile();
						profile.Read(filePath);

						// �d�v�ȃZ�N�V�������Ȃ���΃G���[
						if (!profile.Sections.ContainsSection("Board") ||
							!profile.Sections.ContainsSection("Thread"))
						{
							return null;
						}

						BbsType bbs = (BbsType)Enum.Parse(typeof(BbsType), profile.GetString("Board", "BBS", "X2ch"));

						// ���
						result = TypeCreator.CreateThreadHeader(bbs);
						result.BoardInfo.Server = profile.GetString("Board", "Server", "Error");
						result.BoardInfo.Path = profile.GetString("Board", "Path", "Error");
						result.BoardInfo.Name = profile.GetString("Board", "Name", "Error");
						result.BoardInfo.Bbs = bbs;

						// �X���b�h���
						result.ETag = profile.GetString("Thread", "ETag", String.Empty);
						result.LastWritten = profile.GetDateTime("Thread", "LastWritten");
						result.LastModified = profile.GetDateTime("Thread", "LastModified");
						result.Subject = profile.GetString("Thread", "Subject", "Error");
						result.ResCount = profile.GetInt("Thread", "ResCount", 0);
						result.GotResCount = profile.GetInt("Thread", "GotResCount", 0);
						result.GotByteCount = profile.GetInt("Thread", "GotByteCount", 0);
						result.NewResCount = profile.GetInt("Thread", "NewResCount", 0);
						result.Key = profile.GetString("Thread", "Key", "Error");

						// ���̂ق��̏��
						result.Position = profile.GetFloat("Option", "Position", 0);
						result.Pastlog = profile.GetBool("Option", "Pastlog", false);
						result.UseGzip = profile.GetBool("Option", "UseGzip", false);
						result.Shiori = profile.GetInt("Option", "Shiori", 0);
						result.RefCount = profile.GetInt("Option", "RefCount", 0);
						result.Sirusi.FromArrayString(profile.GetString("Option", "Sirusi", ""));
					}
					catch (Exception ex)
					{
						TwinDll.Output(ex);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// �w�肵���X���b�h�̃C���f�b�N�X���폜
		/// </summary>
		public static void Delete(Cache cache, ThreadHeader header)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			lock (typeof(ThreadIndexer))
			{
				// �C���f�b�N�X�t�@�C���ւ̃p�X
				string filePath = cache.GetIndexPath(header);
				File.Delete(filePath);
			}
		}

		/// <summary>
		/// �C���f�b�N�X�����݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="header">��{�I�ȏ�񂪊i�[���ꂽThreadHeader</param>
		/// <returns>���݂����true�A���݂��Ȃ����false��Ԃ�</returns>
		public static bool Exists(Cache cache, ThreadHeader header)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			bool exists = false;

			lock (typeof(ThreadIndexer))
			{
				// �C���f�b�N�X�t�@�C���ւ̃p�X
				string filePath = cache.GetIndexPath(header);
				exists = File.Exists(filePath);
			}

			return exists;
		}

		/// <summary>
		/// �w�肵���̃C���f�b�N�X���쐬������
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		public static void Indexing(Cache cache, BoardInfo board)
		{
			OfflineThreadListReader reader = new OfflineThreadListReader(cache);
			List<ThreadHeader> items = new List<ThreadHeader>();

			if (reader.Open(board))
			{
				// ���[�J���ɑ��݂��邷�ׂẴt�@�C����ǂݍ���
				while (reader.Read(items) != 0)
					;
				reader.Close();

				// �T�[�o�[�������ׂď���������
				foreach (ThreadHeader h in items)
				{
					h.BoardInfo.Server = board.Server;
					ThreadIndexer.SaveServerInfo(cache, h);
				}

				// �ǂݍ��񂾃X���b�h�������Ɋ����C���f�b�N�X�ꗗ���쐬
				GotThreadListIndexer.Write(cache, board, items);
			}
		}
	}
}
