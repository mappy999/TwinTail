// Cache.cs

namespace Twin
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using Twin.IO;

	/// <summary>
	/// ���O�̃L���b�V�������Ǘ�
	/// </summary>
	public class Cache
	{
		private string baseDirectory;

		/// <summary>
		/// ��{�ƂȂ�f�B���N�g�����擾�܂��͐ݒ�
		/// </summary>
		public string BaseDirectory {
			set {
				if (!Directory.Exists(value))
					Directory.CreateDirectory(value);

				baseDirectory = value;
			}
			get { return baseDirectory; }
		}

		private bool newStruct = false;

		public bool NewStructMode
		{
			get
			{
				return newStruct;
			}
			set
			{
				newStruct = value;
			}
		}
	
		/// <summary>
		/// Cache�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="baseDir">��{�ƂȂ�f�B���N�g�����w��</param>
		public Cache(string baseDir)
		{
			if (baseDir == null) {
				throw new ArgumentNullException("baseDir");
			}
			baseDirectory = baseDir;
		}

		/// <summary>
		/// ���ׂẴ��O���폜
		/// </summary>
		public virtual void Clear()
		{
			string[] allFolders = 
				Directory.GetDirectories(baseDirectory);

			foreach (string folder in allFolders)
				Directory.Delete(folder, true);
		}

		/// <summary>
		/// ��̃t�H���_���폜
		/// </summary>
		public virtual void ClearEmptyFolders()
		{
			string[] subdirs = Directory.GetDirectories(baseDirectory);
			foreach (string sub in subdirs)
				ClearEmptyFolders(sub);
		}

		/// <summary>
		/// �w�肵���f�B���N�g�����̋�t�H���_���폜
		/// </summary>
		/// <param name="directory"></param>
		private void ClearEmptyFolders(string directory)
		{
			// �ċN�𗘗p���ăT�u�t�H���_������
			string[] subdirs = Directory.GetDirectories(directory);
			foreach (string sub in subdirs)
				ClearEmptyFolders(sub);

			// idx�t�@�C���ƃT�u�f�B���N�g�����P���Ȃ����
			// �t�H���_���폜
			string[] indices = Directory.GetFiles(directory, "*.idx");
			subdirs = Directory.GetDirectories(directory);

			if (indices.Length == 0 && subdirs.Length == 0)
				Directory.Delete(directory, true);
		}

		/// <summary>
		/// �w�肵���̃��O�Ə����������폜
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public virtual void Remove(BoardInfo board)
		{
			if (board != null)
			{
				try {
					string folder = GetFolderPath(board);
					Directory.Delete(folder, true);
				}
				catch (Exception ex) {
					TwinDll.Output(ex);
				}
			}
		}

		/// <summary>
		/// �w�肵���X���b�h�̃��O���폜
		/// </summary>
		/// <param name="header"></param>
		public virtual bool Remove(ThreadHeader header)
		{
			if (header != null)
			{
				// �����C���f�b�N�X�ꗗ����폜
				GotThreadListIndexer.Remove(this, header);

				// �������ݗ����C���f�b�N�X�ꗗ����폜
//				WroteHistoryIndexer.Remove(this, new WroteThreadHeader(header));

				// ���O�t�@�C�����폜
				string idx = GetIndexPath(header);
				string dat = GetDatPath(header);

				header.GotByteCount = 0;
				header.GotResCount = 0;
				header.NewResCount = 0;
				header.Position = 0;
				header.ETag = String.Empty;
				header.LastModified = new DateTime(0);

				try
				{
					File.Delete(idx);
					File.Delete(dat);
				}
				catch
				{
				}
			}
			return false;
		}

		/// <summary>
		/// ���X�����ځ[��
		/// </summary>
		/// <param name="header">���ځ[�񂷂�X���b�h���</param>
		/// <param name="indices">���ځ[�񂷂郌�X�ԍ��̔z��</param>
		/// <param name="visible">�������ځ[��̏ꍇ��false�A�����łȂ��ꍇ��true</param>
		public virtual void ResABone(ThreadHeader header, int[] indices, bool visible)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			if (indices == null) {
				throw new ArgumentNullException("indices");
			}

			ResSetCollection resSets = new ResSetCollection();
			ThreadStorage storage = null;

			try {
				storage = new LocalThreadStorage(this);
				
				// �X���b�h��ǂݍ���
				if (storage.Open(header, StorageMode.Read))
				{
					while (storage.Read(resSets) != 0);
					storage.Close();
				}

				// ���X�̍폜
				foreach (int index in indices)
					resSets.ABone(index, visible, visible ? ABoneType.Normal : ABoneType.Tomei, "");

				// ���O����[�폜
				string dat = GetDatPath(header);
				File.Delete(dat);

				// ��������
				if (storage.Open(header, StorageMode.Write))
				{
					storage.Write(resSets);
					storage.Close();
				}
				storage = null;
			}
			finally {
				if (storage != null)
					storage.Close();
			}
		}

		/// <summary>
		/// �w�肵���̃t�H���_�����݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public virtual bool Exists(BoardInfo board)
		{
			return Directory.Exists(GetFolderPath(board, false));
		}

		/// <summary>
		///	�w�肵���̌f���̃��[�g�f�B���N�g�����擾�iBbsType=X2ch: basedir/2ch.net, BbsType=bbspink: basedir/bbspink.com)
		/// </summary>
		/// <param name="bi"></param>
		/// <returns></returns>
		public virtual string GetBbsRootDirectory(BoardInfo bi)
		{
			return Path.Combine(this.BaseDirectory, bi.DomainName);
		}

		/// <summary>
		/// �w�肵���̃��[�J���t�H���_�ւ̃p�X���擾
		/// (�t�H���_�����݂��Ȃ���΍쐬)
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public virtual string GetFolderPath(BoardInfo board)
		{
			return GetFolderPath(board, true);
		}

		/// <summary>
		/// �w�肵���̃��[�J���t�H���_�ւ̃p�X���擾
		/// </summary>
		/// <param name="board"></param>
		/// <param name="create">�t�H���_���쐬����ꍇ��true�A�쐬���Ȃ��ꍇ��false</param>
		/// <returns></returns>
		public virtual string GetFolderPath(BoardInfo board, bool create)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder(32);

			if (newStruct)
			{
				sb.Append(board.DomainPath.Replace('/', '\\')); // 2ch\\path
			}
			else
			{
				sb.Append(board.Server);						// hoge.2ch.net\\path 
				sb.Append('\\');
				sb.Append(board.Path);
			}

			string result =
				Path.Combine(baseDirectory, sb.ToString());

			if (create && !Directory.Exists(result))
				Directory.CreateDirectory(result);

			return result;
		}

		/// <summary>
		/// �w�肵���X���b�h�̃��[�J����̃p�X���擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public virtual string GetDatPath(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			string extension = String.Empty;
			string folder = GetFolderPath(header.BoardInfo, false);

			if (header.UseGzip)
				extension = ".gz";

			return Path.Combine(folder, header.Key + ".dat" + extension);
		}

		/// <summary>
		///  �w�肵���X���b�h�̏�񂪑��݂���p�X���擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public virtual string GetIndexPath(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			string folder = GetFolderPath(header.BoardInfo, false);
			return Path.Combine(folder, header.Key + ".idx");
		}
	}
}
