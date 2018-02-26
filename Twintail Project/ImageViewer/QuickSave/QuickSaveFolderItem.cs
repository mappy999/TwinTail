// QuickSaveFolderItem.cs

namespace ImageViewerDll
{
	using System;
	using System.Windows.Forms;
	using System.Collections;
	using System.Runtime.Serialization;
	using System.Reflection;

	/// <summary>
	/// �N�C�b�N�ۑ��t�H���_��\��
	/// </summary>
	[Serializable]
	public class QuickSaveFolderItem : ISerializable
	{
		private string folderPath;
		private string title;
		private Shortcut shortcut;

		/// <summary>
		/// �ۑ��t�H���_�̃p�X���擾�܂��͐ݒ�
		/// </summary>
		public string FolderPath {
			set {
				if (value == null)
					throw new ArgumentNullException("FolderPath");

				folderPath = value;
			}
			get { return folderPath; }
		}

		/// <summary>
		/// �t�H���_�̕ʖ����擾�܂��͐ݒ�
		/// </summary>
		public string Title {
			set {
				if (value == null)
					throw new ArgumentNullException("FolderPath");

				title = value;
			}
			get { return title; }
		}

		/// <summary>
		/// �V���[�g�J�b�g�L�[���擾�܂��͐ݒ�
		/// </summary>
		public Shortcut Shortcut {
			set {
				if (shortcut != value)
					shortcut = value;
			}
			get { return shortcut; }
		}

		/// <summary>
		/// QuickSaveFolderItem�N���X�̃C���X�^���X��������
		/// </summary>
		public QuickSaveFolderItem()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.folderPath = String.Empty;
			this.title = String.Empty;
			this.shortcut = Shortcut.None;
		}

		/// <summary>
		/// QuickSaveFolderItem�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="folderPath"></param>
		/// <param name="title"></param>
		/// <param name="expand"></param>
		/// <param name="shortcut"></param>
		public QuickSaveFolderItem(string folderPath, string title, Shortcut shortcut)
		{
			this.folderPath = folderPath;
			this.title = title;
			this.shortcut = shortcut;
		}

		public QuickSaveFolderItem(SerializationInfo info, StreamingContext context)
		{
			CSharpSamples.Serializer.Deserialize(this, info,
				BindingFlags.Instance | BindingFlags.NonPublic);
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			CSharpSamples.Serializer.Serialize(this, info,
				BindingFlags.Instance | BindingFlags.NonPublic);
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return title;
		}
	}
}
