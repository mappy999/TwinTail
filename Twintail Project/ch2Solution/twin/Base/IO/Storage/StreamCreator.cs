// GzipStreamCreator.cs

namespace Twin.IO
{
	using System;
	using System.IO;
    using System.IO.Compression;
	using Twin.Util;

	/// <summary>
	/// Gzip���k�𗘗p�������o�̓X�g���[���̏��������s��
	/// </summary>
	public class StreamCreator
	{
		/// <summary>
		/// CreateDir
		/// </summary>
		/// <param name="filePath"></param>
		private static void CreateDir(string filePath)
		{
			string dir = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
		}

		/// <summary>
		/// �t�@�C����ǂݍ��ރ��[�_�[��������
		/// </summary>
		/// <param name="filePath">�J���t�@�C����</param>
		/// <param name="useGzip">Gzip���k����Ă���Ȃ�true�A�����łȂ����false</param>
		/// <returns></returns>
		public static Stream CreateReader(string filePath, bool useGzip)
		{
			CreateDir(filePath);

			Stream baseStream = new FileStream(
				filePath, FileMode.OpenOrCreate, FileAccess.Read);

			if (useGzip)
			{
                using (GZipStream input = new GZipStream(baseStream, CompressionMode.Decompress))
                {
                    baseStream = FileUtility.CreateMemoryStream(input);
                }
			}

			return baseStream;
		}

		/// <summary>
		/// �t�@�C���ɏ������ރX�g���[����������
		/// </summary>
		/// <param name="filePath">�������ݐ�t�@�C����</param>
		/// <param name="useGzip">�������ݎ���Gzip���k���g�p����ꍇ��true</param>
		/// <param name="append">�ǉ��������݂��s���Ȃ�true</param>
		/// <returns></returns>
		public static Stream CreateWriter(string filePath, bool useGzip, bool append)
		{
			Stream baseStream = null;
			CreateDir(filePath);

			if (useGzip)
			{
				byte[] bytes = new byte[0];

				if (append)
				{
					// ��[���ׂĉ𓀂��o�b�t�@�ɋl�߂�
					baseStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
					using (GZipStream inp = new GZipStream(baseStream, CompressionMode.Decompress))
					{
						bytes = FileUtility.ReadBytes(inp);
						//inp.Close();
					}
				}

				// ���ׂĉ𓀂��I�������ēx�X�g���[�����J���A
				// �ǂݍ��񂾃f�[�^�����k
				baseStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
				baseStream = new GZipStream(baseStream, CompressionMode.Compress);

				// �𓀂��ꂽ�o�b�t�@����������
				if (append)
				{
					baseStream.Write(bytes, 0, bytes.Length);
				}
			}
			else
			{
				baseStream =
					new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write);
			}

			return baseStream;
		}
	}
}
