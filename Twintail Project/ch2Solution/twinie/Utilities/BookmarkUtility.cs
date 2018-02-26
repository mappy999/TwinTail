// BookmarkUtility.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// ���C�ɓ���֘A
	/// </summary>
	public class BookmarkUtility
	{
		/// <summary>
		/// �w�肵�����C�ɓ����oldboard��newboard�ɒu��������
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="oldboard"></param>
		/// <param name="newboard"></param>
		public static void ServerChange(BookmarkFolder folder, BoardInfo oldboard, BoardInfo newboard)
		{
			if (folder == null) {
				throw new ArgumentNullException("server");
			}
			if (oldboard == null) {
				throw new ArgumentNullException("oldboard");
			}
			if (newboard == null) {
				throw new ArgumentNullException("newboard");
			}

			foreach (BookmarkEntry entry in folder.Children)
			{
				if (entry.IsLeaf)
				{
					BookmarkThread item = (BookmarkThread)entry;

					// �̃A�h���X��V�����A�h���X�ɏ���������
					if (oldboard.Equals(item.HeaderInfo.BoardInfo))
					{
						if (ThreadIndexer.Read(Twinie.Cache, item.HeaderInfo) != null)
						{
							item.HeaderInfo.BoardInfo.Server = newboard.Server;
							ThreadIndexer.Write(Twinie.Cache, item.HeaderInfo);
						}
						else {
							item.HeaderInfo.BoardInfo.Server = newboard.Server;
						}
					}
				}
				else {
					ServerChange((BookmarkFolder)entry, oldboard, newboard);
				}
			}
		}
	}
}
