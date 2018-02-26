// AutoRefreshTimerCollection.cs

namespace Twin.Tools
{
	using System;
	using System.Timers;
	using System.Collections;

	/// <summary>
	/// ThreadControl���^�C�}�[�Ŏ����X�V����@�\�����B
	/// AutoRefreshTimerSimple�Ƃ͈Ⴂ�A�X���̑����ɂ���čX�V�Ԋu�𑝌����邱�ƂŖ��ʂȍX�V�������B
	/// </summary>
	public class AutoRefreshTimerCollection : AutoRefreshTimerBase
	{
		private ArrayList timerCollection;
		private TimerObject current;
		private bool running;
		private int interval;
		private int index;

		/// <summary>
		/// �X�V�Ԋu���~���b�P�ʂŎ擾�܂��͐ݒ�B
		/// </summary>
		public override int Interval {
			set { interval = Math.Max(5000, value); }
			get { return interval; }
		}

		#region InnerClass
		/// <summary>
		/// �X���b�h���X�V����^�C�}�[���Ǘ�
		/// </summary>
		internal class TimerObject : IDisposable
		{
			private int defInterval;
			private Timer timer;
			private ThreadControl thread;
			private bool disposed = false;

			/// <summary>
			/// ���̃X���b�h�̍X�V�Ԋu���擾�܂��͐ݒ�
			/// </summary>
			public int Interval {
				set { timer.Interval = Math.Max(defInterval, value); }
				get { return (int)timer.Interval; }
			}

			/// <summary>
			/// �^�C�}�[���L�����ǂ����𔻒f
			/// </summary>
			public bool Enabled {
				get { return timer.Enabled; }
			}

			/// <summary>
			/// �X���b�h�R���g���[�����擾
			/// </summary>
			public ThreadControl Thread {
				get { return thread; }
			}

			/// <summary>
			/// TimerObject�N���X�̃C���X�^���X��������
			/// </summary>
			/// <param name="control">�X�V�Ώۂ̃X���b�h�R���g���[��</param>
			/// <param name="interval">�X�V�Ԋu�̏����l (�~���b)</param>
			/// <param name="elapsed">���Ԍo�ߎ��̃C�x���g�n���h��</param>
			public TimerObject(ThreadControl control, int interval, ElapsedEventHandler elapsed)
			{
				timer = new Timer();
				timer.Interval = interval;
				timer.Elapsed += elapsed;
				defInterval = interval;
				thread = control;
			}

			/// <summary>
			/// �^�C�}�[���J�n
			/// </summary>
			public void Start()
			{
				timer.Start();
			}

			/// <summary>
			/// �^�C�}�[���~
			/// </summary>
			public void Stop()
			{
				timer.Stop();
			}

			/// <summary>
			/// �g�p���Ă��郊�\�[�X�����
			/// </summary>
			public void Dispose()
			{
				if (!disposed)
				{
					timer.Stop();
					timer.Dispose();
					GC.SuppressFinalize(this);
				}
				disposed = true;
			}
		}
		#endregion

		/// <summary>
		/// AutoRefreshTimerCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public AutoRefreshTimerCollection()
		{
			timerCollection = ArrayList.Synchronized(new ArrayList());
			running = false;
			current = null;
			interval = 10000;
			index = 0;
		}

		/// <summary>
		/// ���̃^�C�}�[�̈ʒu�֐i�߂�
		/// </summary>
		private void Increment()
		{
			if (timerCollection.Count > 0)
			{
				if (index >= timerCollection.Count)
					index = 0;

				current = (TimerObject)timerCollection[index++];
				current.Start();
				running = true;
			}
			else {
				running = false;
			}
		}

		public override int GetInterval(ThreadControl client)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		public override int GetTimeleft(ThreadControl client)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		public override ITimerObject GetTimerObject(ThreadControl client)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// ���X�g�ɃN���C�A���g��ǉ��B
		/// ���ɓ����N���C�A���g���o�^����Ă���Ή������Ȃ��B
		/// </summary>
		/// <param name="client">�����X�V�̑ΏۂƂ���N���C�A���g</param>
		public override void Add(ThreadControl client)
		{
			if (client == null) {
				throw new ArgumentNullException("client");
			}

			if (IndexOf(client) == -1)
			{
				// �C�x���g��o�^
				client.Complete += new CompleteEventHandler(OnComplete);

				timerCollection.Add(new TimerObject(client, Interval,
					new ElapsedEventHandler(OnTimer)));
			}

			if (!running)
				Increment();
		}

		/// <summary>
		/// ���X�g����N���C�A���g���폜�B
		/// �w�肵���N���C�A���g�����X�g�ɑ��݂��Ȃ���Ή������Ȃ��B
		/// </summary>
		/// <param name="client">�����X�V�̑Ώۂ���O���N���C�A���g</param>
		public override void Remove(ThreadControl client)
		{
			if (client  == null) {
				throw new ArgumentNullException("client");
			}

			int index = IndexOf(client);

			if (index != -1)
			{
				// �C�x���g���폜
				client.Complete -= new CompleteEventHandler(OnComplete);

				TimerObject timer = (TimerObject)timerCollection[index];
				timerCollection.Remove(timer);

				if (timer.Enabled)
				{
					timer.Stop();
					Increment();
				}

				timer.Dispose();
			}

			if (timerCollection.Count == 0)
				running = false;
		}

		/// <summary>
		/// �w�肵���N���C�A���g�����X�g���ɑ��݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="client">��������N���C�A���g</param>
		/// <returns>���X�g���ɑ��݂����true�A���݂��Ȃ����false��Ԃ�</returns>
		public override bool Contains(ThreadControl client)
		{
			if (client  == null) {
				throw new ArgumentNullException("client");
			}
			return IndexOf(client) != -1;
		}

		/// <summary>
		/// ���ׂẴ^�C�}�[���폜
		/// </summary>
		public override void Clear()
		{
			if (current != null)
			{
				current.Dispose();
				current = null;

				timerCollection.Clear();
			}
		}

		/// <summary>
		/// �w�肵���N���C�A���g�̃R���N�V�������̈ʒu���擾
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		private int IndexOf(ThreadControl client)
		{
			foreach (TimerObject obj in timerCollection)
				if (obj.Thread == client)
					return timerCollection.IndexOf(obj);

			return -1;
		}

		/// <summary>
		/// �X�V�C�x���g����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTimer(object sender, ElapsedEventArgs e)
		{
//			Timer timer = (Timer)sender;
//			timer.Stop();

			try {
				current.Stop();
				ThreadControl thread = current.Thread;

				if (thread.IsOpen)
					thread.Reload();
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}
		}

		/// <summary>
		/// �X���b�h�X�V�����C�x���g����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnComplete(object sender, CompleteEventArgs e)
		{
			if (current == null)
				return;

			if (e.Status == CompleteStatus.Success)
			{
				// �����C���X�^���X���ǂ������`�F�b�N
				if (current.Thread.Equals(sender))
				{
					ThreadControl thread = current.Thread;

					// �V�������������ǂ���
					bool notmodified = (thread.HeaderInfo.NewResCount == 0);

					// �V��������ΊԊu���k�߂�A�Ȃ���ΊԊu���̂΂�
					current.Interval += notmodified ? Interval : -Interval;

					// �ő僌�X�����z���Ă��ĐV�����Ȃ���΃^�C�}�[����O��
					if (thread.HeaderInfo.IsLimitOverThread && notmodified)
						Remove(current.Thread);

					current = null;

					// ���̃X���b�h��
					Increment();
				}
			}
		}
	}
}
