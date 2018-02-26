// ThreadUtil.cs

namespace Twin.Util
{
	using System;
	using System.IO;
	using System.Text;
	using Twin.IO;
	using Twin.Bbs;
	using Twin.Conv;
	using Twin.Text;

	/// <summary>
	/// ThreadUtil �̊T�v�̐����ł��B
	/// </summary>
	public class ThreadUtil
	{
		/// <summary>
		/// �X���b�h��dat�`���ŕۑ�
		/// </summary>
		/// <param name="cache">�L���b�V�����</param>
		/// <param name="header">�ۑ�����X���b�h</param>
		/// <param name="filePath">�ۑ���t�@�C���p�X</param>
		public static void SaveDat(Cache cache, ThreadHeader header, string filePath)
		{
			if (cache == null)
				throw new ArgumentNullException("cache");
		
			if (header == null)
				throw new ArgumentNullException("header");

			if (filePath == null)
				throw new ArgumentNullException("filePath");

			// dat�̑��݂���p�X���擾
			string fromPath = cache.GetDatPath(header);

			ThreadStorage reader = null;
			StreamWriter writer = null;
			
			ResSetCollection items = new ResSetCollection();
			X2chThreadFormatter formatter = new X2chThreadFormatter();

			try {
				// �ǂݍ��݃X�g���[�����J��
				reader = new LocalThreadStorage(cache, header, StorageMode.Read);
				// �������݃X�g���[�����J��
				writer = new StreamWriter(filePath, false, Encoding.GetEncoding("Shift_Jis"));

				// ���ׂēǂݍ���
				while (reader.Read(items) != 0);
				writer.Write(formatter.Format(items));
			}
			finally {
				if (reader != null) reader.Close();
				if (writer != null) writer.Close();
			}
		}

		/// <summary>
		/// �X���b�h��html�`���ŕۑ�
		/// </summary>
		/// <param name="cache">�L���b�V�����</param>
		/// <param name="header">�ۑ�����X���b�h</param>
		/// <param name="filePath">�ۑ���t�@�C���p�X</param>
		public static void SaveHtml(Cache cache, ThreadHeader header, string filePath, ThreadSkinBase skin)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");
				
			if (cache == null)
				throw new ArgumentNullException("cache");
		
			if (header == null)
				throw new ArgumentNullException("header");

			// dat�̑��݂���p�X���擾
			string fromPath = cache.GetDatPath(header);

			ThreadStorage reader = null;
			StreamWriter writer = null;
			
			try {
				// �ǂݍ��݃X�g���[�����J��
				reader = new LocalThreadStorage(cache, header, StorageMode.Read);
				// �������݃X�g���[�����J��
				writer = new StreamWriter(filePath, false, TwinDll.DefaultEncoding);
				
				ResSetCollection items = new ResSetCollection();
				
				if (skin == null)
					skin = new HtmlSkin();

				// �w�b�_����������
				writer.WriteLine(skin.GetHeader(header));

				// �{������������
				while (reader.Read(items) != 0);
				writer.WriteLine(skin.Convert(items));

				// �t�b�^����������
				writer.WriteLine(skin.GetFooter(header));
			}
			finally {
				if (reader != null) reader.Close();
				if (writer != null) writer.Close();
			}
		}

		/// <summary>
		/// �X���b�h��monalog�`���ŕۑ�
		/// </summary>
		/// <param name="cache">�L���b�V�����</param>
		/// <param name="header">�ۑ�����X���b�h</param>
		/// <param name="filePath">�ۑ���t�@�C���p�X</param>
		public static void SaveMonalog(Cache cache, ThreadHeader header, string filePath)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");
				
			if (cache == null)
				throw new ArgumentNullException("cache");
		
			if (header == null)
				throw new ArgumentNullException("header");

			// dat�̑��݂���p�X���擾
			string fromPath = cache.GetDatPath(header);
			MonalogConverter conv = new MonalogConverter();

			ThreadStorage reader = null;
			ResSetCollection items = new ResSetCollection();

			try {
				reader = new LocalThreadStorage(cache, header, StorageMode.Read);
				while (reader.Read(items) != 0);
				conv.Write(filePath, header, items);
			}
			finally {
				if (reader != null)
					reader.Close();
			}
		}

		/// <summary>
		/// �P�Ƃ�dat�t�@�C���J���A�������L���b�V���Ƃ��ĕۑ�
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="target">�P�Ƃ�dat�Ɗ֘A�Â����</param>
		/// <param name="filePath">dat�t�@�C���̃t�@�C���p�X</param>
		/// <param name="datNumber">dat�ԍ�</param>
		/// <param name="gzip">dat�t�@�C����gzip���k����Ă����true�A�����łȂ����false���w�肷��</param>
		/// <returns>�L���b�V�����ꂽ�X���b�h�̃w�b�_����Ԃ�</returns>
		public static ThreadHeader OpenDat(Cache cache, BoardInfo target,
			string filePath, string datNumber, bool gzip)
		{
			// �w�b�_�[�����쐬
			ThreadHeader header = TypeCreator.CreateThreadHeader(target.Bbs);
			header.BoardInfo = target;
			header.Key = datNumber;
			header.UseGzip = gzip;
			header.Subject = String.Empty;

			ResSetCollection resItems = new ResSetCollection();

			using (Stream stream = StreamCreator.CreateReader(filePath, gzip))
			{
				X2chThreadParser parser = new X2chThreadParser();

				byte[] buffer = new byte[4096];
				bool first = true;
				int offset = 0, read, parsed;

				do {
					// �o�b�t�@�ɓǂݍ���
					read = stream.Read(buffer, 0, buffer.Length);
					offset += read;
					
					// ��͂�ResSet�\���̂̔z����쐬
					ResSet[] array = parser.Parse(buffer, read, out parsed);
					resItems.AddRange(array);

					// �X���^�C���擾���Ă���
					if (first && array.Length > 0)
					{
						header.Subject = array[0].Tag as String;
						first = false;
					}

				} while (read != 0);

				// �����o�C�g���ƃ��X����ݒ�
				header.GotByteCount = offset;
				header.GotResCount = resItems.Count;
			}

			// dat�t�@�C���̍ŏI�X�V�����擾
			header.LastModified = File.GetLastWriteTime(filePath);
			
			// �ǂݍ��񂾃��X���L���b�V���Ƃ��ĕۑ�
			using (LocalThreadStorage storage = 
					   new LocalThreadStorage(cache, header, StorageMode.Write))
			{
				storage.Write(resItems);
			}

			// �C���f�b�N�X���𐶐�
			ThreadIndexer.Write(cache, header);

			return header;
		}

		/// <summary>
		/// monalog�`���̃X���b�h���L���b�V���Ƃ��ĕۑ�
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static ThreadHeader OpenMonalog(Cache cache, string filePath)
		{
			ResSetCollection resItems = null;
			ThreadHeader header = null;

			MonalogConverter conv = new MonalogConverter();
			conv.Read(filePath, out header, out resItems);

			using (LocalThreadStorage storage = 
					   new LocalThreadStorage(cache, header, StorageMode.Write))
			{
				storage.Write(resItems);
			}

			ThreadIndexer.Write(cache, header);

			return header;
		}
	}
}
