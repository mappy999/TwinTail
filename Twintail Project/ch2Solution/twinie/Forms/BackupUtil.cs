using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Twin
{
	public class BackupUtil
	{
		private DirectoryInfo backupDir;

		public BackupUtil(string folderPath)
		{
			this.backupDir = new DirectoryInfo(folderPath);

			if (!backupDir.Exists)
			{
				backupDir.Create();
			}
		}

		public bool Backup(string sourceFilePath)
		{
			if (!File.Exists(sourceFilePath))
				return false;

			string fileName = Path.GetFileName(sourceFilePath);
			string backupFilePath = Path.Combine(backupDir.FullName, fileName);

			//　一応、バックアップのバックアップを取っとく
			if (File.Exists(backupFilePath))
				File.Copy(backupFilePath, backupFilePath + ".bak", true);

			File.Copy(sourceFilePath, backupFilePath, true);

			return true;
		}

		public bool Restore(string sourceFilePath)
		{
			string fileName = Path.GetFileName(sourceFilePath);
			string backupFilePath = Path.Combine(backupDir.FullName, fileName);

			if (!File.Exists(backupFilePath))
				return false;

			File.Copy(backupFilePath, sourceFilePath, true);

			return true;
		}
	}
}
