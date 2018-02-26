// CacheRemoveUtility.cs
// #2.0

namespace Twin.Tools
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Windows.Forms;

	/// <summary>
	/// �L���b�V�����폜���郆�[�e�B���e�B
	/// </summary>
	public class CacheRemoveUtility
	{
		private Cache cache;

		/// <summary>
		/// CacheRemoveUtility�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		public CacheRemoveUtility(Cache cache)
		{
			this.cache = cache;
		}

		/// <summary>
		/// ���C�ɓ���ȊO�̊����X���b�h���폜���܂��B
		/// </summary>
		/// <param name="bookmarkRoot"></param>
		public void RemoveWithoutBookmarks(IBoardTable table, BookmarkRoot bookmarkRoot)
		{
			if (bookmarkRoot == null) {
				throw new ArgumentNullException("bookmarkRoot");
			}

			// ���ׂĂ̔̊����C���f�b�N�X��ǂݍ���
			List<ThreadHeader> targets = Twin.IO.GotThreadListReader.GetAllThreads(cache, table);
			int count1 = targets.Count;

			// �ǂݍ��܂ꂽ�C���f�b�N�X�ŁA���C�ɓ���ɓo�^����Ă��镨�͏��O
			for (int i = 0; i < targets.Count;)
			{
				ThreadHeader h = targets[i];
				BookmarkThread match = bookmarkRoot.Search(h);

				if (match != null)
					targets.RemoveAt(i);

				else i++;
			}

			int count2 = count1 - targets.Count;

			foreach (ThreadHeader h in targets)
				cache.Remove(h);

			cache.ClearEmptyFolders();

#if DEBUG
			int count3 = Twin.IO.GotThreadListReader.GetAllThreads(cache, table).Count;
			MessageBox.Show(String.Format("�폜�O={0}, ���C�ɓ���̐�={1}, �폜��̃��O��={2}", count1, count2, count3));
#endif
		}
	}
}
