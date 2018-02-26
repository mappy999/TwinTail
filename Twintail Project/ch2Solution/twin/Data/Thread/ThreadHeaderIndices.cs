// ThreadHeaderManager.cs

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using Twin;
	using CSharpSamples.Winapi;
	using Twin.Text;

	/// <summary>
	/// �C���f�b�N�X�t�@�C���̈ꗗ���Ǘ�
	/// </summary>
	public class ThreadHeaderIndices
	{
		private Cache cache;
		private List<ThreadHeader> items;
		private string fileName;

		/// <summary>
		/// ThreadHeader�̃R���N�V�������擾
		/// </summary>
		public List<ThreadHeader> Items {
			get { return items; }
		}

		/// <summary>
		/// ThreadHeaderIndices�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="fileName"></param>
		public ThreadHeaderIndices(Cache cache, string fileName)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.cache = cache;
			this.fileName = fileName;
			this.items = new List<ThreadHeader>();
		}

		/// <summary>
		/// �C���f�b�N�X�ꗗ��ǂݍ���
		/// </summary>
		public void Load()
		{
			items.Clear();

			if (File.Exists(fileName))
			{
				StreamReader sr = null;
				string text;

				try {
					sr = new StreamReader(fileName, TwinDll.DefaultEncoding);
					while ((text = sr.ReadLine()) != null)
					{
						string filePath = Path.Combine(Application.StartupPath, text);
						ThreadHeader header = ThreadIndexer.Read(filePath);

						if (header != null)
							items.Add(header);
					}
				}
				finally {
					if (sr != null)
						sr.Close();
				}
			}
		}

		/// <summary>
		/// �C���f�b�N�X�ꗗ���t�@�C���ɕۑ�
		/// </summary>
		public void Save()
		{
			StreamWriter sw = null;
			try {
				sw = new StreamWriter(fileName, false, TwinDll.DefaultEncoding);
				foreach (ThreadHeader header in items)
				{
					if (ThreadIndexer.Exists(cache, header))
					{
						// ���΃p�X�ɕϊ�
						string relative = Shlwapi.GetRelativePath(
							Application.StartupPath, cache.GetIndexPath(header));

						sw.WriteLine(relative);
					}
				}
			}
			finally {
				if (sw != null)
					sw.Close();
			}
		}
	}
}
