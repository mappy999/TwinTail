// KeyValueCollection.cs

namespace CSharpSamples
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Runtime.Serialization;

	/// <summary>
	/// KeyValues �N���X���R���N�V�����Ǘ��B
	/// 
	/// ��
	/// [section1]
	/// key1
	/// key2
	/// key3
	/// ...
	/// </summary>
	public class KeyValuesCollection : IEnumerable
	{
		private Hashtable hash;

		/// <summary>
		/// �w�肵���L�[�����l�̃R���N�V�������擾
		/// </summary>
		public StringCollection this[string key] {
			get {
				if (hash.ContainsKey(key))
				{
					KeyValues kv = (KeyValues)hash[key];
					return kv.Values;
				}
				return null;
			}
		}

		/// <summary>
		/// KeyValuesCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public KeyValuesCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			hash = new Hashtable();
		}

		/// <summary>
		/// �w�肵���t�@�C������f�[�^��ǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		public void Read(string filePath)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");

			StreamReader sr = null;
			KeyValues keyset = null;
			string text;

			try {
				sr = new StreamReader(filePath, Encoding.Default);
				Clear();

				while ((text = sr.ReadLine()) != null)
				{
					// �󕶎��͖���
					if (text == String.Empty)
						continue;

					if (text.StartsWith("[") && text.EndsWith("]"))
					{
						keyset = new KeyValues(text.Substring(1, text.Length-2));
						Add(keyset);
					}
					else if (keyset != null)
					{
						keyset.Values.Add(text);
					}
				}
			}
			finally {
				if (sr != null)
					sr.Close();
			}
		}

		/// <summary>
		/// �w�肵���t�@�C���Ƀf�[�^��ۑ�
		/// </summary>
		/// <param name="filePath"></param>
		public void Write(string filePath)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");

			StreamWriter sw = null;
			try {
				sw = new StreamWriter(filePath, false, Encoding.Default);

				foreach (KeyValues _set in hash.Values)
				{
					sw.Write('[');
					sw.Write(_set.Key);
					sw.Write(']');

					sw.WriteLine();

					foreach (string val in _set.Values)
						sw.WriteLine(val);

					sw.WriteLine();
				}
			}
			finally {
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// �L�[��ǉ�
		/// </summary>
		/// <param name="obj"></param>
		public void Add(KeyValues obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			hash.Add(obj.Key, obj);
		}

		/// <summary>
		/// �w�肵���L�[�����ݒ���폜
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			hash.Remove(key);
		}

		/// <summary>
		/// �l�����ׂĎ擾
		/// </summary>
		public void Clear()
		{
			hash.Clear();
		}

		/// <summary>
		/// KeyValuesCollection�𔽕���������񋓎q��Ԃ�
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return hash.Values.GetEnumerator();
		}
	}

	/// <summary>
	/// �L�[�ƒl�R���N�V�����̃Z�b�g
	/// </summary>
	[Serializable]
	public class KeyValues// : ISerializable
	{
		private string key;
		private StringCollection values;

		/// <summary>
		/// �L�[�����擾�܂��͐ݒ�
		/// </summary>
		public string Key {
			set {
				if (value == null)
					throw new ArgumentNullException("Key");

				key = value;
			}
			get { return key; }
		}

		/// <summary>
		/// �l�̃R���N�V�������擾
		/// </summary>
		public StringCollection Values {
			get {
				return values;
			}
		}

		/// <summary>
		/// KeyValues�N���X�̃C���X�^���X��������
		/// </summary>
		public KeyValues()
		{
			key = null;
			values = new StringCollection();
		}

		/// <summary>
		/// KeyValues�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="key"></param>
		public KeyValues(string key) : this()
		{
			if (key == null)
				throw new ArgumentNullException("key");

			this.key = key;
		}

		/// <summary>
		/// KeyValues�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="key"></param>
		/// <param name="array"></param>
		public KeyValues(string key, string[] array) : this()
		{
			if (key == null)
				throw new ArgumentNullException("key");
			if (array == null)
				throw new ArgumentNullException("array");

			this.key = key;
			this.values.AddRange(array);
		}
	}
}
