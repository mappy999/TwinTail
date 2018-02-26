// FileUtility.cs

namespace Twin.Util
{
	using System;
	using System.IO;
	using System.Text;

	/// <summary>
	/// �t�@�C���֌W�̃��[�e�B���e�B�Q
	/// </summary>
	public class FileUtility
	{
		/// <summary>
		/// �t�@�C����ǂݍ���
		/// �i�t�@�C�������݂��Ȃ���΋󕶎����Ԃ��j
		/// </summary>
		/// <param name="filePath">�ǂݍ��ރt�@�C���̃p�X</param>
		/// <returns></returns>
		public static string ReadToEnd(string filePath)
		{
			return ReadToEnd(filePath, TwinDll.DefaultEncoding);
		}

		/// <summary>
		/// �t�@�C����ǂݍ���
		/// �i�t�@�C�������݂��Ȃ���΋󕶎����Ԃ��j
		/// </summary>
		/// <param name="filePath">�ǂݍ��ރt�@�C���̃p�X</param>
		/// <returns></returns>
		public static string ReadToEnd(string filePath, Encoding enc)
		{
			string result = String.Empty;

			if (File.Exists(filePath))
			{
				using (StreamReader sr = new StreamReader(filePath, TwinDll.DefaultEncoding))
					result = sr.ReadToEnd();
			}

			return result;
		}

		/// <summary>
		/// �w�肵���t�@�C���Ƀe�L�X�g��ۑ�
		/// </summary>
		/// <param name="filePath">�ۑ���t�@�C����</param>
		/// <param name="text">�������ޕ�����</param>
		/// <param name="append">�ǉ��������݂��s���ꍇ��true�A�����łȂ����false</param>
		public static void Write(string filePath, string text, bool append)
		{
			Write(filePath, text, append, TwinDll.DefaultEncoding);
		}

		/// <summary>
		/// �w�肵���t�@�C���Ƀe�L�X�g��ۑ�
		/// </summary>
		/// <param name="filePath">�ۑ���t�@�C����</param>
		/// <param name="text">�������ޕ�����</param>
		/// <param name="append">�ǉ��������݂��s���ꍇ��true�A�����łȂ����false</param>
		public static void Write(string filePath, string text, bool append, Encoding enc)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}

			string directory =
				Path.GetDirectoryName(filePath);

			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			using (StreamWriter sw = new StreamWriter(filePath, append, enc))
			{
				sw.Write(text);
				sw.Flush();
			}
		}

		/// <summary>
		/// ���ׂĂ�ǂݍ��݃������X�g���[�����쐬
		/// </summary>
		/// <param name="baseStream"></param>
		/// <returns></returns>
		public static MemoryStream CreateMemoryStream(Stream baseStream)
		{
			if (baseStream == null) {
				throw new ArgumentNullException("baseStream");
			}

			MemoryStream memory = new MemoryStream();
			byte[] buffer = new byte[4096];
			int read = -1, allCount = 0;

			while (read != 0)
			{
				read = baseStream.Read(buffer, 0, buffer.Length);
				memory.Write(buffer, 0, read);
				allCount += read;
			}

			buffer = null;
			memory.Position = 0;
			memory.SetLength(allCount);

			return memory;
		}

		/// <summary>
		/// ���ׂĂ�ǂݍ���byte�z��Ɋi�[
		/// </summary>
		/// <param name="baseStream"></param>
		/// <returns></returns>
		public static byte[] ReadBytes(Stream baseStream)
		{
			return CreateMemoryStream(baseStream).ToArray();
		}
	}
}
