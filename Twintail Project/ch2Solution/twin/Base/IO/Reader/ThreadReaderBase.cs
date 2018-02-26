// ThreadReaderBase.cs

namespace Twin.IO
{
	using System;
	using System.Text;
	using System.IO;
	using System.Collections;
	using System.Net;
	using Twin.Text;
	using System.Threading;

	/// <summary>
	/// �X���b�h��ǂݍ��ނ��߂̊�{�N���X
	/// </summary>
	public abstract class ThreadReaderBase : ThreadReader
	{
		private string uagent;

		protected Stream baseStream;
		protected ThreadParser dataParser;

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

		/// <summary>
		/// �X���b�h�̃f�[�^�T�C�Y
		/// </summary>
		public override int Length
		{
			get
			{
				return length;
			}
		}

		/// <summary>
		/// �X�g���[���̌��݈ʒu
		/// </summary>
		public override int Position
		{
			get
			{
				return position;
			}
		}

		/// <summary>
		/// �X�g���[���̓ǂݏ����Ɏg�p����o�b�t�@�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public override int BufferSize
		{
			set
			{
				buffSize = Math.Max(4096, value);
			}
			get
			{
				return buffSize;
			}
		}

		/// <summary>
		/// �ǂݍ��ݒ����ǂ����𔻒f
		/// </summary>
		public override bool IsOpen
		{
			get
			{
				return isOpen;
			}
		}

		/// <summary>
		/// User-Agent���擾�܂��͐ݒ�
		/// </summary>
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

		/// <summary>
		/// ThreadReaderBase
		/// </summary>
		private ThreadReaderBase()
		{
			uagent = TwinDll.UserAgent;
			buffSize = 32768;
			index = 1;
			isOpen = false;
		}

		/// <summary>
		/// ThreadReaderBase�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="parser">�ǂݍ��ݎ��ɉ�͂��s�����߂̃p�[�T�[</param>
		protected ThreadReaderBase(ThreadParser parser)
			: this()
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			dataParser = parser;
		}

		/// <summary>
		/// ���X��ǂݍ���
		/// </summary>
		/// <param name="resSets"></param>
		/// <returns>�ǂݍ��܂ꂽ��byte����Ԃ�</returns>
		public override int Read(ResSetCollection resSets)
		{
			int temp;
			return Read(resSets, out temp);
		}

		/// <summary>
		/// ���X��ǂݍ���
		/// </summary>
		/// <param name="resSets"></param>
		/// <param name="byteParsed">��͂��ꂽ�o�C�g�����i�[�����</param>
		/// <returns>�ǂݍ��܂ꂽ��byte����Ԃ�</returns>
		public override int Read(ResSetCollection resSets, out int byteParsed)
		{
			if (resSets == null)
			{
				throw new ArgumentNullException("resSets");
			}
			if (!isOpen)
			{
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			// �o�b�t�@�Ƀf�[�^��ǂݍ���
			int readCount = baseStream.Read(buffer, 0, buffer.Length);

			// ��͂��ăR���N�V�����Ɋi�[
			ResSet[] items = dataParser.Parse(buffer, readCount, out byteParsed);

			foreach (ResSet resSet in items)
			{
				ResSet res = resSet;
				res.Index = index++;
				resSets.Add(res);
			}

			// ���ۂɓǂݍ��܂ꂽ�o�C�g�����v�Z
			position += readCount;

			return readCount;
		}

		public override void Cancel()
		{
			canceled = true;
		}

		/// <summary>
		/// �X�g���[�������
		/// </summary>
		public override void Close()
		{
			canceled = false;

			if (isOpen)
			{
				isOpen = false;

				if (dataParser != null)
				{
					// ����I������Ƃ��͕K��0�ɂȂ�͂�
					//System.Diagnostics.Debug.Assert(dataParser.RemainderLength == 0);
					dataParser.Empty();
				}

				if (baseStream != null)
					baseStream.Close();

				position = 0;
				length = 0;
				index = 1;
				baseStream = null;
				_buffer = null;
			}
		}
	}
}
