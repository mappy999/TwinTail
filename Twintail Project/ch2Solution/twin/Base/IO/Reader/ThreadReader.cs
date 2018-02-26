// ThreadReader.cs

namespace Twin.IO
{
	using System;
	using System.Collections;
	using System.Net;

	/// <summary>
	/// �f���̃X���b�h��ǂݍ��ރC���^�[�t�F�[�X
	/// </summary>
	public abstract class ThreadReader
	{
		/// <summary>
		/// �f�[�^�̒������擾
		/// </summary>
		public abstract int Length
		{
			get;
		}

		/// <summary>
		/// �X�g���[���̌��݈ʒu���擾
		/// </summary>
		public abstract int Position
		{
			get;
		}

		/// <summary>
		/// �ǂݍ��݂Ɏg�p����o�b�t�@�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public abstract int BufferSize
		{
			set;
			get;
		}

		/// <summary>
		/// �t�@�C�����J����Ă��邩�ǂ������擾
		/// </summary>
		public abstract bool IsOpen
		{
			get;
		}

		/// <summary>
		/// User-Agent���擾�܂��͐ݒ�
		/// </summary>
		public abstract string UserAgent
		{
			set;
			get;
		}

		/// <summary>
		/// �����擾���ɂ��ځ[������o�����Ƃ��ɔ���
		/// </summary>
		public virtual event EventHandler ABone;

		/// <summary>
		/// �X���b�h���擾�ł��Ȃ������Ƃ��ɔ���
		/// </summary>
		public virtual event EventHandler<PastlogEventArgs> Pastlog;

		/// <summary>
		/// �X���b�h���J��
		/// </summary>
		/// <param name="header"></param>
		public abstract bool Open(ThreadHeader header);

		/// <summary>
		/// �f�[�^����͂���resCollection�Ɋi�[
		/// </summary>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g���B���[�̏ꍇ�� 0�B���ځ[������m�����ꍇ�� -1 ��Ԃ�</returns>
		public abstract int Read(ResSetCollection resCollection);

		/// <summary>
		/// �f�[�^����͂���resCollection�Ɋi�[
		/// </summary>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g���B���[�̏ꍇ�� 0�B���ځ[������m�����ꍇ�� -1 ��Ԃ�</returns>
		public abstract int Read(ResSetCollection resCollection, out int byteParsed);

		/// <summary>
		/// �ǂݍ��݃X�g���[�������
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// �ʐM�������L�����Z�����܂��B
		/// </summary>
		public abstract void Cancel();

		/// <summary>
		/// ABone�C�x���g�𔭐�������
		/// </summary>
		protected void OnABone()
		{
			if (ABone != null)
				ABone(this, EventArgs.Empty);
		}

		/// <summary>
		/// Pastlog�C�x���g�𔭐�������
		/// </summary>
		protected void OnPastlog(PastlogEventArgs argument)
		{
			if (Pastlog != null)
				Pastlog(this, argument);
		}
	}

	public delegate void KakologEventHandler(bool gzipCompress);

	public class AboneEventArgs : EventArgs
	{
		private bool reget = false;
		/// <summary>
		/// ���O���Ď擾���邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool IsReget
		{
			get
			{
				return reget;
			}
			set
			{
				reget = value;
			}
		}
	
		public AboneEventArgs()
		{
		}
	}
}
