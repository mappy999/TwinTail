// LocalThreadReader.cs

namespace Twin.IO
{
	using System;
	using System.IO;
	using System.Text;
	using Twin.Text;

	/// <summary>
	/// LocalThreadReader �̊T�v�̐����ł��B
	/// </summary>
	public class LocalThreadReader : ThreadReaderBase
	{
		/// <summary>
		/// LocalThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="dataParser"></param>
		public LocalThreadReader(ThreadParser dataParser)
			: base(dataParser)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		public bool __Open(string path)
		{
			if (File.Exists(path))
			{
				baseStream = new FileStream(path, FileMode.Open);
				base.length = (int)baseStream.Length;
				base.isOpen = true;
				base.index = 1;
				return true;
			}
			return false;
		}

		public override bool Open(ThreadHeader header)
		{
			throw new NotImplementedException("���̃��\�b�h�͎g�p�ł��܂���B");
		}
	}
}
