// ThreadListStorage.cs
// #2.0

namespace Twin.IO
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ���[�J���ɃX���b�h�ꗗ��ۑ��E�Ǘ�����C���^�[�t�F�[�X�B
	/// </summary>
	public abstract class ThreadListStorage : IDisposable
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
		/// �t�@�C�����J����Ă���� true�A�����Ă���� false ��Ԃ��܂��B
		/// </summary>
		public abstract bool IsOpen
		{
			get;
		}

		/// <summary>
		/// �L���b�V���f�[�^�̃T�C�Y��Ԃ��܂��B
		/// </summary>
		public abstract int Length
		{
			get;
		}

		/// <summary>
		/// ���݂̃X�g���[���̈ʒu��Ԃ��܂��B
		/// </summary>
		public abstract int Position
		{
			get;
		}

		/// <summary>
		/// �ǂݍ��݃��[�h�ŊJ����Ă��邩�ǂ�����\���܂��B
		/// </summary>
		public abstract bool CanRead
		{
			get;
		}

		/// <summary>
		/// �������݃��[�h�ŊJ����Ă��邩�ǂ�����\���܂��B
		/// </summary>
		public abstract bool CanWrite
		{
			get;
		}

		/// <summary>
		/// �w�肵���̃L���b�V�����AStorageMode.Read �Ȃ�ǂ݂Ƃ胂�[�h�A
		/// StorageMode.Write �Ȃ珑�����݃��[�h�ŊJ���܂��B
		/// </summary>
		/// <param name="board"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public abstract bool Open(BoardInfo board, StorageMode mode);

		/// <summary>
		/// �L���b�V������X���b�h�ꗗ��ǂݍ��� headerList �Ɋi�[���܂��B
		/// �ǂ݂Ƃ胂�[�h�ŊJ����Ă���K�v������܂��B
		/// </summary>
		/// <param name="headerList"></param>
		/// <returns></returns>
		public abstract int Read(List<ThreadHeader> headerList);

		/// <summary>
		/// �L���b�V������X���b�h�ꗗ��ǂݍ��� headerList �Ɋi�[���܂��B
		/// �ǂ݂Ƃ胂�[�h�ŊJ����Ă���K�v������܂��B
		/// </summary>
		/// <param name="headerList"></param>
		/// <param name="byteParsed"></param>
		/// <returns></returns>
		public abstract int Read(List<ThreadHeader> headerList, out int byteParsed);

		/// <summary>
		/// headerList �Ɋi�[����Ă���X���b�h�ꗗ���L���b�V���ɏ������݂܂��B
		/// �������݃��[�h�ŊJ����Ă���K�v������܂��B
		/// </summary>
		public abstract int Write(List<ThreadHeader> headerList);

		/// <summary>
		/// Open ���\�b�h�ŊJ����Ă���X�g���[������܂��B
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// �g�p���Ă��郊�\�[�X��������A�J����Ă���X�g���[������܂��B
		/// </summary>
		public abstract void Dispose();
	}
}
