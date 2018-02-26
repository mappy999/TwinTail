// ThreadListReader.cs
// #2.0

namespace Twin.IO
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// �X���b�h�ꗗ��ǂݍ��ރC���^�[�t�F�[�X�ł��B
	/// </summary>
	public abstract class ThreadListReader
	{
		/// <summary>
		/// ��M�p�o�b�t�@�T�C�Y���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public abstract int BufferSize
		{
			set;
			get;
		}

		/// <summary>
		/// �f�[�^�̒������擾���܂��B
		/// </summary>
		public abstract int Length
		{
			get;
		}

		/// <summary>
		/// �X�g���[���̌��݈ʒu���擾���܂��B
		/// </summary>
		public abstract int Position
		{
			get;
		}

		/// <summary>
		/// ���[�_�[���J����Ă���� true�A����ȊO�� false ��Ԃ��܂��B
		/// </summary>
		public abstract bool IsOpen
		{
			get;
		}

		/// <summary>
		/// User-Agent ���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public abstract string UserAgent
		{
			set;
			get;
		}

		/// <summary>
		/// ���ړ]���Ă����ꍇ�Ɏ����Œǔ����邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public abstract bool AutoRedirect {
			set;
			get;
		}

		/// <summary>�T�[�o�[���ړ]�����Ƃ��ɔ������܂��B</summary>
		public event EventHandler<ServerChangeEventArgs> ServerChange;

		/// <summary>
		/// �w�肵�����J���܂��B
		/// </summary>
		/// <param name="board"></param>
		public abstract bool Open(BoardInfo board);

		/// <summary>
		/// �f�[�^����M����͂��ꂽ�f�[�^�� items �ɒǉ����܂��B
		/// </summary>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g����Ԃ��܂��B</returns>
		public abstract int Read(List<ThreadHeader> items);

		/// <summary>
		/// �f�[�^����M����͂��ꂽ�f�[�^�� items �ɒǉ����܂��B��͂��ꂽ�o�C�g���� byteParsed �Ɋi�[����܂��B
		/// </summary>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g����Ԃ��܂��B</returns>
		public abstract int Read(List<ThreadHeader> items, out int byteParsed);

		/// <summary>
		/// �ʐM�������L�����Z�����܂��B
		/// </summary>
		public abstract void Cancel();

		/// <summary>
		/// �g�p���Ă��郊�\�[�X��������A�J���Ă���X�g���[������܂��B
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// ServerChange�C�x���g�𔭐������܂��B
		/// </summary>
		protected void OnServerChange(ServerChangeEventArgs e)
		{
			if (ServerChange != null)
				ServerChange(this, e);
		}
	}
}
