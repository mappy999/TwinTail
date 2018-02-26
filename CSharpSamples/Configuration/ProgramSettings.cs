// ProgramSettingscs

namespace CSharpSamples
{
	using System;
	using System.IO;
	using System.Xml;
	using System.Diagnostics;
	using System.ComponentModel;

	/// <summary>
	/// �v���O�����ݒ�Ȃǂ̏��� Xml �`���ŊǗ�����@�\��񋟂��܂��B
	/// </summary>
	public class ProgramSettings
	{
		private string fileName;
		private XmlDocument document;

		/// <summary>
		/// ProgramSettings�N���X�̃C���X�^���X���������B
		/// </summary>
		/// <param name="fileName">�ݒ���̃t�@�C�����B</param>
		public ProgramSettings(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.fileName = fileName;
			this.document = new XmlDocument();

			try {
				if (File.Exists(fileName))
				{
					document.Load(fileName);
				}
				else {
					CreateRoot();
				}
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
				CreateRoot();
			}
		}

		/// <summary>
		/// ���[�g���쐬���܂��B
		/// </summary>
		private void CreateRoot()
		{
			XmlNode root = document.CreateElement("Settings");
			document.AppendChild(root);
		}

		/// <summary>
		/// �w�肵���L�[�̒l���擾���܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public object Get(string key, Type type)
		{
			if (key == null) {
				throw new ArgumentNullException("key");
			}

			XmlElement element = document.DocumentElement;
			XmlNode node = element.SelectSingleNode(key);
			
			if (node != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				
				if (converter == null)
					throw new ArgumentException(type + "�^�ɑΉ����� TypeConverter ��������܂���B", "type");

				return converter.ConvertFromString(node.InnerText);
			}

			return null;
		}

		/// <summary>
		/// �w�肵���Z�N�V�����ƃL�[�ɐݒ肳��Ă���l���擾���܂��B
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Get(string section, string key, Type type)
		{
			if (section == null) {
				throw new ArgumentNullException("section");
			}
			if (key == null) {
				throw new ArgumentNullException("key");
			}

			XmlElement element = document.DocumentElement;
			XmlNode node = element.SelectSingleNode(section + "/" + key);
			
			if (node != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				
				if (converter == null)
					throw new ArgumentException(type + "�^�ɑΉ����� TypeConverter ��������܂���B", "type");

				return converter.ConvertFromString(node.InnerText);
			}

			return null;
		}

		/// <summary>
		/// �w�肵���L�[�ɒl��ݒ肵�܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(string key, object value)
		{
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (value == null) {
				throw new ArgumentNullException("value");
			}

			TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
			
			if (converter == null)
				throw new ArgumentException("�w�肵���I�u�W�F�N�g�ɑΉ����� TypeConverter ��������܂���B", "value");

			// ���[�g�v�f���擾
			XmlElement root = document.DocumentElement;

			// �Z�N�V���������݂���m�[�h���擾
			XmlNode node = root.SelectSingleNode(key);
			if (node == null)
			{
				node = root.AppendChild(
					document.CreateElement(key));
			}

			node.InnerText = converter.ConvertToString(value);
		}

		/// <summary>
		/// �w�肵���Z�N�V�����̎w�肵���L�[�ɒl��ݒ肵�܂��B
		/// </summary>
		/// <param name="section">�Z�N�V������</param>
		/// <param name="key">�L�[��</param>
		/// <param name="value">�ݒ肷��l</param>
		public void Set(string section, string key, object value)
		{
			if (section == null) {
				throw new ArgumentNullException("section");
			}
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (value == null) {
				throw new ArgumentNullException("value");
			}

			TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
			
			if (converter == null)
				throw new ArgumentException("�w�肵���I�u�W�F�N�g�ɑΉ����� TypeConverter ��������܂���B", "value");

			// ���[�g�v�f���擾
			XmlElement root = document.DocumentElement;

			// �Z�N�V���������݂���m�[�h���擾
			XmlNode parent = root.SelectSingleNode(section);
			if (parent == null)
			{
				parent = root.AppendChild(
					document.CreateElement(section));
			}

			// �l���i�[����m�[�h���擾
			XmlNode child = parent.SelectSingleNode(key);
			if (child == null)
			{
				child = parent.AppendChild(
					document.CreateElement(key));
			}

			child.InnerText = converter.ConvertToString(value);
		}

		/// <summary>
		/// �ݒ�����t�@�C���ɕۑ����܂��B
		/// </summary>
		public void Save()
		{
			document.Save(fileName);
		}
	}
}
