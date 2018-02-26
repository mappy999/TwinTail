// CryptoConfig.cs

namespace CSharpSamples
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections;
	using System.Security.Cryptography;
	using System.Diagnostics;

	/// <summary>
	/// �f�[�^���ȒP��XOR�Í������ĕۑ��A�܂��͈Í������ꂽ�f�[�^�𕜌����܂��B
	/// </summary>
	public class XorCryptoConfig
	{
		private Hashtable _hash;
		private byte[] _key;

		/// <summary>
		/// CryptoConfig�N���X�̃C���X�^���X���������B
		/// </summary>
		public XorCryptoConfig() : 
			this("mc$r[olUa`![c|k5yt:]SW@#{xx,6!dt=8sGq]n#xl)5CFf(<Hd")
		{
		}

		/// <summary>
		/// CryptoConfig�N���X�̃C���X�^���X���������B
		/// </summary>
		public XorCryptoConfig(string key)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			_hash = new Hashtable();
			_key = Encoding.Default.GetBytes(key);
		}

		protected string GetBase(string key)
		{
			if (_hash.Contains(key))
			{
				return (string)_hash[key];
			}
			else {
				return null;
			}
		}

		/// <summary>
		/// ������`���̒l���擾���܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <param name="def"></param>
		/// <returns>�L�[�����݂���Ύ擾����������A�G���[�Ȃ� def ��Ԃ��܂��B</returns>
		public string Get(string key, string def)
		{
			string ret = GetBase(key);
			return (ret != null) ? ret : def;
		}

		/// <summary>
		/// ���l�`���̒l���擾���܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <param name="def"></param>
		/// <returns>�L�[�����݂���Ύ擾�������l�A�G���[�Ȃ� def ��Ԃ��܂��B</returns>
		public int GetInt(string key, int def)
		{
			string ret = GetBase(key);
			return (ret != null) ? Int32.Parse(ret) : def;
		}

		/// <summary>
		/// �w�肵���L�[�ɒl��ݒ肵�܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(string key, object value)
		{
			if (_hash.Contains(key))
			{
				_hash[key] = value;
			}
			else {
				_hash.Add(key, value);
			}
		}

		/// <summary>
		/// �ݒ�����ׂč폜���܂��B
		/// </summary>
		public void Clear()
		{
			_hash.Clear();
		}

		/// <summary>
		/// �t�@�C������ݒ��ǂݍ��݂܂��B
		/// </summary>
		/// <param name="fileName"></param>
		public void Save(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}

			// ������ �� �Í��� �� �t�@�C���X�g���[��

			Stream fileStream = null;
			StringBuilder sb = new StringBuilder();

			try {
				fileStream = new FileStream(
					fileName, FileMode.Create, FileAccess.Write);

				foreach (string key in _hash.Keys)
				{
					string data = String.Format("{0}={1}\r\n", key, _hash[key]);
					sb.Append(data);
				}
				
				byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
				bytes = Xor(bytes, _key);
				
				fileStream.Write(bytes, 0, bytes.Length);
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
			}
			finally {
				if (fileStream != null)
					fileStream.Close();
			}
		}

		/// <summary>
		/// �ݒ���t�@�C���ɕۑ����܂��B
		/// </summary>
		/// <param name="fileName"></param>
		public void Load(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}

			// �t�@�C���X�g���[�� �� ������ �� �������X�g���[�� �� �X�g���[�����[�_�[

			Stream fileStream = null;
			StreamReader reader = null;
			MemoryStream mem = new MemoryStream();

			try {
				fileStream = new FileStream(
					fileName, FileMode.OpenOrCreate, FileAccess.Read);
				
				byte[] buffer = new byte[fileStream.Length];
				int read = fileStream.Read(buffer, 0, buffer.Length);

				buffer = Xor(buffer, _key);
				mem.Write(buffer, 0, buffer.Length);
				mem.Seek(0L, SeekOrigin.Begin);

				reader = new StreamReader(mem, Encoding.Default);

				string text;
				while ((text = reader.ReadLine()) != null)
				{
					Match m = Regex.Match(text, "(?<key>\\w+?)=(?<value>.+)");
					if (m.Success)
					{
						string key = m.Groups["key"].Value;
						string val = m.Groups["value"].Value;

						Set(key, val);
					}
				}
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
			}
			finally {
				if (reader != null)
					reader.Close();

				if (fileStream != null)
					fileStream.Close();
			}
		}

		/// <summary>
		/// data ���w�肵�� key ���g�p���� xor ���]���s���܂��B
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] Xor(byte[] data, byte[] key)
		{
			if (data == null) {
				throw new ArgumentNullException("data");
			}
			if (key == null) {
				throw new ArgumentNullException("key");
			}

			byte[] result = new byte[data.Length];
			
			for (int i = 0, k = 0; i < data.Length; i++)
			{
				result[i] = (byte)(data[i] ^ key[k++]);
				
				if (k >= key.Length)
					k = 0;
			}
			
			return result;
		}
	}
}
