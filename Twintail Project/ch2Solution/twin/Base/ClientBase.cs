// ClientBase.cs

namespace Twin.Bbs
{
	using System;
	using System.Collections;
	using System.Threading;
	using System.Diagnostics;
	using System.Windows.Forms;

	/// <summary>
	/// �f������f�[�^���擾���邽�߂̊�{�N���X�B
	/// �N���C�A���g���ʂ̋@�\�ƃC�x���g�ȂǁB
	/// </summary>
	public abstract class ClientBase : Control
	{
		public static readonly ManualResetEvent Stopper = new ManualResetEvent(true);
		public static readonly AutoResetEvent Connect = new AutoResetEvent(true);

		private static bool connectionLimitter = false;
		public static bool ConnectionLimitter
		{
			get
			{
				return connectionLimitter;
			}
			set
			{
				connectionLimitter = value;
			}
		}
	
		/// <summary>
		/// �X���b�h�̗D�揇�ʂ��擾�܂��͐ݒ�
		/// </summary>
		public static ThreadPriority Priority = ThreadPriority.Normal;

		/// <summary>
		/// �L���b�V�������Ǘ�����N���X���擾
		/// </summary>
		public readonly Cache Cache;

		/// <summary>
		/// �ǂݍ��݊J�n���ɔ���
		/// </summary>
		public event EventHandler Loading;

		/// <summary>
		/// �f�[�^��M���ɔ���
		/// </summary>
		public event ReceiveEventHandler Receive;

		/// <summary>
		/// �ǂݍ��݊������ɔ���
		/// </summary>
		public event CompleteEventHandler Complete;

		/// <summary>
		/// �X�e�[�^�X���ύX���ꂽ���ɔ���
		/// </summary>
		public event StatusTextEventHandler StatusTextChanged;

		/// <summary>
		/// ClientBase�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache">�L���b�V�����</param>
		protected ClientBase(Cache cache)
		{
			Cache = cache;
		}

		/// <summary>
		/// Loading�C�x���g�𔭐�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnLoading(EventArgs e)
		{
			Stopper.WaitOne();

			if (Loading != null)
			{
				if (InvokeRequired)
				{
					Invoke(new EventHandler(Loading), new object[] {this, e});
				}
				else {
					Loading(this, e);
				}
			}

			if (ConnectionLimitter)
				Connect.WaitOne();
		}

		/// <summary>
		/// Receive�C�x���g�𔭐�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnReceive(ReceiveEventArgs e)
		{
			Stopper.WaitOne();

			if (Receive != null)
			{
				if (InvokeRequired)
				{
					Invoke(new ReceiveEventHandler(Receive), new object[] {this, e});
				}
				else {
					Receive(this, e);
				}
			}
		}

		/// <summary>
		/// Complete�C�x���g�𔭐�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnComplete(CompleteEventArgs e)
		{
			Stopper.WaitOne();

			if (ConnectionLimitter)
				Connect.Set();

			if (Complete != null)
			{
				if (InvokeRequired)
				{
					Invoke(new CompleteEventHandler(Complete), new object[] {this, e});
				}
				else {
					Complete(this, e);
				}
			}
		}

		/// <summary>
		/// StatusTextChanged�C�x���g�𔭐�������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnStatusTextChanged(string text)
		{
			if (StatusTextChanged != null)
			{
				StatusTextEventArgs e =
					new StatusTextEventArgs(text);

				if (InvokeRequired)
				{
					Invoke(new StatusTextEventHandler(StatusTextChanged), 
						new object[] {this, e});
				}
				else {
					StatusTextChanged(this, e);
				}
			}
		}

		/// <summary>
		/// �R���g���[����I��
		/// </summary>
		public abstract void _Select();
	}
	
	public abstract class ClientBaseEx<THeader> : ClientBase
	{
		public abstract THeader HeaderInfo
		{
			get;
		}

		public ClientBaseEx(Cache cache)
			: base(cache)
		{
		}
	}
}
