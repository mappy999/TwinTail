// ThreadStorage.cs

namespace Twin.IO
{
	using System;

	/// <summary>
	/// ���[�J���ɃX���b�h��ۑ��E�Ǘ�����C���^�[�t�F�[�X
	/// </summary>
	public abstract class ThreadStorage : IDisposable
	{
		private byte[] _buffer;
		protected int bufferSize = 4096;

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
					_buffer = new byte[bufferSize];

				return _buffer;
			}
		}

		/// <summary>
		/// �t�@�C�����J����Ă��邩�ǂ����𔻒f
		/// </summary>
		public abstract bool IsOpen
		{
			get;
		}

		/// <summary>
		/// �L���b�V���f�[�^�̃T�C�Y
		/// </summary>
		public abstract int Length
		{
			get;
		}

		/// <summary>
		/// ���݂̃X�g���[���̈ʒu
		/// </summary>
		public abstract int Position
		{
			get;
		}

		/// <summary>
		/// �o�b�t�@�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public abstract int BufferSize
		{
			set;
			get;
		}

		/// <summary>
		/// �ǂݍ��݃��[�h�ŊJ����Ă��邩�ǂ�����\��
		/// </summary>
		public abstract bool CanRead
		{
			get;
		}

		/// <summary>
		/// �������݃��[�h�ŊJ����Ă��邩�ǂ�����\��
		/// </summary>
		public abstract bool CanWrite
		{
			get;
		}

		/// <summary>
		/// �X���b�h���J��
		/// </summary>
		/// <param name="header"></param>
		public abstract bool Open(ThreadHeader header, StorageMode mode);

		/// <summary>
		/// resCollection�ɃL���b�V������ǂݍ���
		/// </summary>
		/// <param name="resCollection"></param>
		/// <returns></returns>
		public abstract int Read(ResSetCollection resCollection);

		/// <summary>
		/// resCollection�ɃL���b�V������ǂݍ���
		/// </summary>
		/// <param name="resCollection"></param>
		/// <returns></returns>
		public abstract int Read(ResSetCollection resCollection, out int byteParsed);

		/// <summary>
		/// resCollection���t�@�C���ɏ�������
		/// </summary>
		public abstract int Write(ResSetCollection resCollection);

		/// <summary>
		/// �X�g���[�������
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// �g�p���Ă��郊�\�[�X�����
		/// </summary>
		public abstract void Dispose();
	}
}
