using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Twin
{
	public class ThreadGroup
	{
		private string dir, ext;

		/// <summary>
		/// このグループリストの保存先ファイル名を取得します。
		/// </summary>
		public string FileName
		{
			get
			{
				return Path.Combine(dir, name + ext);
			}
		}
	
		private string name;
		/// <summary>
		/// グループリストの名前を取得または設定します。
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				string oldFileName = this.FileName;
				name = value;
				string newFileName = this.FileName;

				File.Move(oldFileName, newFileName);
			}
		}
	
		private ThreadHeaderIndices indices;
		/// <summary>
		/// グループ化されたスレッド一覧を取得します。
		/// </summary>
		public ThreadHeaderIndices ThreadList
		{
			get
			{
				return indices;
			}
		}

		public ThreadGroup(Cache cache, string fileName)
		{
			this.dir = Path.GetDirectoryName(fileName);
			this.ext = Path.GetExtension(fileName);
			this.name = Path.GetFileNameWithoutExtension(fileName);
			this.indices = new ThreadHeaderIndices(cache, fileName);
		}

		public void ChangeCache(Cache cache)
		{
			ThreadHeaderIndices newIndices = new ThreadHeaderIndices(cache, this.FileName);
			newIndices.Items.AddRange(this.indices.Items);

			this.indices = newIndices;
		}

		public void Save()
		{
			indices.Save();
		}

		public void Load()
		{
			indices.Load();
		}

	}
}
