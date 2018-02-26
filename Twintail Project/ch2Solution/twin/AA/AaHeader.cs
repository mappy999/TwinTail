// AaHeader.cs

namespace Twin.Aa
{
	using System;
	using System.Text;
	using System.IO;

	/// <summary>
	/// *.aa�t�@�C���̏���\��
	/// </summary>
	public class AaHeader
	{
		private AaItemCollection  items;
		private string fileName;

		/// <summary>
		/// ���̃w�b�_�[��񂪕ۑ�����Ă���t�@�C�������擾
		/// </summary>
		public string FileName {
			get { return fileName; }
		}

		/// <summary>
		/// AaItem���i�[����Ă���R���N�V�������擾
		/// </summary>
		public AaItemCollection Items {
			get { return items; }
		}

		/// <summary>
		/// AaHeader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="filename">�쐬����aa�t�@�C���ւ̃p�X</param>
		public AaHeader(string filename)
		{
			if (filename == null) {
				throw new ArgumentNullException("filename");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			items = new AaItemCollection();
			items.ItemSet += new AaItemSetEventHandler(OnItemSet);
			fileName = filename;
		}

		/// <summary>
		/// �w�b�_�[����ǂݍ��ށB�t�@�C�������݂��Ȃ��ꍇ�͗�O���o��
		/// </summary>
		public void Load()
		{
			StreamReader sr = null;
			string data = null;

			try {
				sr = new StreamReader(fileName, TwinDll.DefaultEncoding);
				items.Clear();

				while ((data = sr.ReadLine()) != null)
				{
					bool single = !data.StartsWith("#");
					string text = single ? data : data.Substring(1);

					AaItem aa = new AaItem(text, single);
					items.Add(aa);
				}
			}
			finally {
				if (sr != null)
					sr.Close();
			}
		}

		/// <summary>
		/// ���݂̃w�b�_�[�����t�@�C���ɕۑ�
		/// </summary>
		public void Save()
		{
			StreamWriter sw = null;

			try {
				sw = new StreamWriter(fileName, false, TwinDll.DefaultEncoding);

				foreach (AaItem aa in items)
				{
					sw.WriteLine(aa.ToString());
				}
			}
			finally {
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// �R���N�V�����ɒǉ����ꂽ�A�C�e���̐e�������ɐݒ肷��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnItemSet(object sender, AaItemSetEventArgs e)
		{
			e.Item.parent = this;
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Path.GetFileNameWithoutExtension(fileName);
		}
	}
}
