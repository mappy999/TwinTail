// Twin2Converter.cs

namespace Twin.Conv
{
	using System;
	using System.Text;
	using System.IO;
	using System.Collections;
	using Twin.IO;
	using Twin.Text;
	using Twin.Bbs;

	/// <summary>
	/// twintail2 �̃��O���݃R���o�[�^�[
	/// </summary>
	public class Twin2Converter : IConvertible
	{
		private bool useGzip;

		/// <summary>
		/// Twin2Converter�N���X�̃C���X�^���X��������
		/// </summary>
		public Twin2Converter()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			useGzip = false;
		}

		public void Read(string filePath, out ThreadHeader header,
			out ResSetCollection resCollection)
		{
			// .idx�t�@�C���ւ̃p�X�����߂�
			string indexPath = GetIndexPath(filePath);

			if (!File.Exists(indexPath))
				throw new FileNotFoundException("�C���f�b�N�X�t�@�C�������݂��܂���");

			// �C���f�b�N�X����ǂݍ���
			header = ThreadIndexer.Read(indexPath);
			if (header == null)
				throw new ConvertException("�C���f�b�N�X�t�@�C���̓ǂݍ��݂Ɏ��s���܂���");

			resCollection = ReadFile(filePath, header.UseGzip);
		}

		private ResSetCollection ReadFile(string filePath, bool gzip)
		{
			ResSetCollection resCollection = new ResSetCollection();
			Stream stream = StreamCreator.CreateReader(filePath, gzip);
			ThreadParser dataParser = new X2chThreadParser();

			try {
				byte[] buffer = new byte[10240];
				int readCount = 0, index = 1;

				do {
					// �o�b�t�@�Ƀf�[�^��ǂݍ���
					readCount = stream.Read(buffer, 0, buffer.Length);
					int parsed;

					// ��͂��ăR���N�V�����Ɋi�[
					ICollection collect = dataParser.Parse(buffer, readCount, out parsed);

					foreach (ResSet resSet in collect)
					{
						ResSet res = resSet;
						res.Index = index++;

						resCollection.Add(res);
					}
				} while (readCount != 0);
			}
			finally {
				if (stream != null)
					stream.Close();
			}

			return resCollection;
		}

		private string GetIndexPath(string filePath)
		{
			string indexPath = null;

			if (filePath.EndsWith(".dat.gz"))
				indexPath = filePath.Substring(0, filePath.Length - 7);

			else if (filePath.EndsWith(".dat"))
				indexPath = filePath.Substring(0, filePath.Length - 4);
			
			else { // �s���Ȋg���q
				throw new NotSupportedException(filePath + "\r\n���̃t�@�C���̊g���q�̓T�|�[�g���Ă��܂���");
			}

			indexPath += ".idx";
			return indexPath;
		}
		
		public void Write(string filePath, ThreadHeader header,
			ResSetCollection resCollection)
		{
			Stream stream = StreamCreator.CreateWriter(filePath, useGzip, true);
			header.UseGzip = useGzip;

			try {
				ThreadFormatter formatter = new X2chThreadFormatter();
				string textData = formatter.Format(resCollection);
				byte[] byteData = TwinDll.DefaultEncoding.GetBytes(textData);

				stream.Write(byteData, 0, byteData.Length);
			}
			finally {
				if (stream != null)
					stream.Close();
			}

			// �C���f�b�N�X�t�@�C�����쐬
			string indexPath = GetIndexPath(filePath);
			ThreadIndexer.Write(indexPath, header);
		}
	}
}
