// ThreadListReaderBase.cs
// #2.0

namespace Twin.IO
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using Twin.Text;
	using System.Threading;

	/// <summary>
	/// �X���b�h�ꗗ��ǂݍ��ނ��߂̊�{�N���X�ł��B
	/// </summary>
	public abstract class ThreadListReaderBase : ThreadListReader
	{
		private string uagent;
		private bool autoRedirect;

		protected Stream baseStream;
		protected ThreadListParser dataParser;
		protected BoardInfo boardinfo;

		private byte[] _buffer;
		private int buffSize;

		protected bool isOpen;
		protected int index;
		protected int length;
		protected int position;

		protected bool canceled = false;

		protected byte[] buffer
		{
			set
			{
				_buffer = null;
				_buffer = value;
			}
			get
			{
				if (_buffer == null)
					_buffer = new byte[BufferSize];

				return _buffer;
			}
		}

		public override int Length
		{
			get
			{
				return length;
			}
		}

		public override int Position
		{
			get
			{
				return position;
			}
		}

		/// <summary>
		/// �X�g���[���̎�M�o�b�t�@�T�C�Y���擾�܂��͐ݒ肵�܂��B
		/// �ŏ��l�� 1024 byte �ł��B
		/// </summary>
		public override int BufferSize
		{
			set
			{
				if (value < 1024)
				{
#if DEBUG
					throw new ArgumentException("BufferSize=" + value);
#else
					value = 1024;
#endif
				}

				buffSize = value;
			}
			get
			{
				return buffSize;
			}
		}

		public override bool IsOpen
		{
			get
			{
				return isOpen;
			}
		}

		public override string UserAgent
		{
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("UserAgent");
				}

				uagent = value;
			}
			get
			{
				return uagent;
			}
		}

		public override bool AutoRedirect
		{
			set
			{
				autoRedirect = value;
			}
			get
			{
				return autoRedirect;
			}
		}

		/// <summary>
		/// ThreadListReaderBase
		/// </summary>
		private ThreadListReaderBase()
		{
			uagent = TwinDll.UserAgent;
			buffSize = 32768;
			isOpen = false;
			autoRedirect = true;
			index = 1;
		}

		/// <summary>
		/// ThreadListReaderBase�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="parser">�X���b�h�ꗗ�̉�͂Ɏg�p����p�[�T�[</param>
		public ThreadListReaderBase(ThreadListParser parser)
			: this()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			dataParser = parser;
		}

		public override int Read(List<ThreadHeader> headers)
		{
			int temp;
			return Read(headers, out temp);
		}

		public override int Read(List<ThreadHeader> headers, out int byteParsed)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}

			if (!isOpen)
			{
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			// �o�b�t�@�Ƀf�[�^��ǂݍ���
			int readCount = baseStream.Read(buffer, 0, buffer.Length);

			//IAsyncResult ar =
			//    baseStream.BeginRead(buffer, 0, buffer.Length, null, null);

			//while (!ar.IsCompleted)
			//{
			//    if (canceled)
			//    {
			//        return byteParsed = 0;
			//    }

			//    Thread.Sleep(100);
			//}
			//int readCount = baseStream.EndRead(ar);

			// ��͂��ăR���N�V�����Ɋi�[
			ThreadHeader[] items = dataParser.Parse(buffer, readCount, out byteParsed);
			headers.AddRange(items);

			// �l��ݒ�
			foreach (ThreadHeader h in items)
			{
				h.No = index++;
				h.BoardInfo = boardinfo;
			}

			// ���ۂɓǂݍ��܂ꂽ�o�C�g�����v�Z
			position += readCount;

			return readCount;
		}

		public override void Cancel()
		{
			canceled = true;
		}

		public override void Close()
		{
			if (baseStream != null)
				baseStream.Close();

			if (dataParser != null)
				dataParser.Empty();

			isOpen = false;
			canceled = false;

			baseStream = null;
			boardinfo = null;
			_buffer = null;
			position = 0;
			length = 0;
			index = 1;
		}
	}
}
