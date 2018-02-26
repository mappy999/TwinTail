// CacheInfo.cs

namespace Twin
{
	using System;
	using System.IO;

	/// <summary>
	/// �̃L���b�V������\��
	/// </summary>
	public class CacheInfo
	{
		private string folderPath;
		private long totalSize;
		private int totalCount;

		/// <summary>
		/// �L���b�V���t�H���_�ւ̃p�X���擾
		/// </summary>
		public string FolderPath {
			get { return folderPath; }
		}

		/// <summary>
		/// �L���b�V���̍��v�T�C�Y���擾
		/// </summary>
		public long TotalSize {
			get { return totalSize; }
		}

		/// <summary>
		/// �L���b�V���̍��v�����擾
		/// </summary>
		public int TotalCount {
			get { return totalCount; }
		}

		/// <summary>
		/// CacheInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="board"></param>
		public CacheInfo(Cache cache, BoardInfo board)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			if (cache == null) {
				throw new ArgumentNullException("cache");
			}
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			// �ւ̃p�X���擾
			folderPath = cache.GetFolderPath(board);

			DirectoryInfo dir = new DirectoryInfo(folderPath);
			FileInfo[] fileInfoArray = dir.GetFiles("*.idx");

			// ���v�����擾
			totalCount = fileInfoArray.Length;
			totalSize = 0;

			// ���v�T�C�Y���擾
			foreach (FileInfo info in fileInfoArray)
				totalSize += info.Length;
		}
	}
}
