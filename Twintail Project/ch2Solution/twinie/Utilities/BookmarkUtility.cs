// BookmarkUtility.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// お気に入り関連
	/// </summary>
	public class BookmarkUtility
	{
		/// <summary>
		/// 指定したお気に入りのoldboardをnewboardに置き換える
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

					// 板のアドレスを新しいアドレスに書き換える
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
