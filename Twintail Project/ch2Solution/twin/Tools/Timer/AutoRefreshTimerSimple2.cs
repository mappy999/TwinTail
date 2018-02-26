// AutoRefreshTimerCollection2.cs
// #2.0

namespace Twin.Tools
{
	using System;
	using System.Timers;
	using System.Collections.Generic;

	/// <summary>
	/// �e�X���b�h���Ƃɏ�Ԃ�ێ����ă^�C�}�[�̊Ԋu�𒲐�����@�\�����B
	/// ������ƕύX�B
	/// </summary>
	public class AutoRefreshTimerCollection2 : AutoRefreshTimerBase
	{
		private List<TimerObject> timerList = new List<TimerObject>();
		private int interval = 30000; // 30�b

		public const int MinInterval = 10000;
		public const int MaxInterval = 60000 * 15;

		/// <summary>
		/// �X�V�Ԋu���~���b�P�ʂŎ擾�܂��͐ݒ肵�܂��B�ŏ��� 10 �b�ł��B
		/// </summary>
		public override int Interval
		{
			set
			{
				if (MinInterval > value) value = MinInterval;
				if (MaxInterval < value) value = MaxInterval;

				interval = value;
			}
			get
			{
				return interval;
			}
		}

		#region InnerClass
		/// <summary>
		/// �X���b�h���X�V����^�C�}�[���Ǘ�
		/// </summary>
		public class TimerObject : IDisposable, ITimerObject
		{
			private const int DefaultRetryCount = 12;

			private int defInterval;
			private Timer timer;
			private ThreadControl thread;
			private bool disposed = false;

			private int retryCount = DefaultRetryCount;
			private bool timerEnabled = true;

			/// <summary>
			/// �X�V�Ԋu���ő�l�ɂȂ����ꍇ�A�܂��̓^�C���A�E�g�ōX�V�ł��Ȃ������ꍇ��
			/// �Ď��s�񐔂��擾�܂��͐ݒ肵�܂��B
			/// </summary>
			public int RetryCount
			{
				set {
					if (value < 0)
						value = 0;

					retryCount = value;
				}
				get {
					return retryCount;
				}
			}

			/// <summary>
			/// ���̃X���b�h�̍X�V�Ԋu���擾�܂��͐ݒ肵�܂��B
			/// </summary>
			public int Interval
			{
				set
				{
					timer.Interval = Math.Max(defInterval, value);
				}
				get
				{
					return (int)timer.Interval;
				}
			}

			/// <summary>
			/// �^�C�}�[���L�����ǂ����������l���擾�܂��͐ݒ肵�܂��B
			/// </summary>
			public bool Enabled
			{
				set
				{
					timerEnabled = value;

					if (timerEnabled == false)
					{
						Stop();
					}
				}
				get
				{
					return timerEnabled;
				}
			}

			/// <summary>
			/// ���̃^�C�}�[���Ǘ����Ă��� ThreadControl ���擾���܂��B
			/// </summary>
			public ThreadControl Thread
			{
				get
				{
					return thread;
				}
			}

			private int startTick = -1;
			/// <summary>
			/// ���̍X�V�܂ł̎c��b�����擾���܂��B
			/// </summary>
			public int Timeleft
			{
				get
				{
					if (startTick == -1)
						return -1;

					return (Interval - (Environment.TickCount - startTick)) / 1000;
				}
			}

			public event ElapsedEventHandler Elapsed;

			private void OnTimerInternal(object sender, ElapsedEventArgs e)
			{
				if (Elapsed != null)
					Elapsed(this, e);
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
				timer.Elapsed += new ElapsedEventHandler(OnTimerInternal);
				defInterval = interval;
				thread = control;

				Elapsed += elapsed;
			}

			public void ResetRetryCount()
			{
				retryCount = DefaultRetryCount;
			}

			public void Start()
			{
				if (!disposed && timerEnabled)
				{
					timer.Start();
					startTick = Environment.TickCount;
				}
			}

			/// <summary>
			/// �^�C�}�[�̏�Ԃ����Z�b�g���ĊJ�n����
			/// </summary>
			public void ResetStart()
			{
				timerEnabled = true;
				timer.Interval = defInterval;
				Start();

			}

			public void Stop()
			{
				if (!disposed)
					timer.Stop();

				startTick = -1;
			}

			public void Dispose()
			{
				if (!disposed)
				{
					timer.Stop();
					timer.Dispose();
					startTick = -1;
				}
				timerEnabled = false;
				disposed = true;
			}
		}
		#endregion

		/// <summary>
		/// AutoRefreshTimerCollection2�N���X�̃C���X�^���X��������
		/// </summary>
		public AutoRefreshTimerCollection2()
		{
		}

		public override ITimerObject GetTimerObject(ThreadControl client)
		{
			int i = IndexOf(client);

			return i == -1 ? null : timerList[i];
		}

		public override int GetInterval(ThreadControl client)
		{
			int index = IndexOf(client);

			if (index == -1)
				return -1;

			lock (timerList)
			{
				TimerObject obj = timerList[index];
				return obj.Interval;
			}
		}

		public override int GetTimeleft(ThreadControl client)
		{
			int index = IndexOf(client);

			if (index == -1)
				return -1;

			lock (timerList)
			{
				TimerObject obj = timerList[index];
				return obj.Timeleft;
			}
		}

		/// <summary>
		/// ���X�g�ɃN���C�A���g��ǉ��B
		/// ���ɓ����N���C�A���g���o�^����Ă���Ή������Ȃ��B
		/// </summary>
		/// <param name="client">�����X�V�̑ΏۂƂ���N���C�A���g</param>
		public override void Add(ThreadControl client)
		{
			if (client == null)
			{
				throw new ArgumentNullException("client");
			}

			if (IndexOf(client) == -1)
			{
				client.Complete += new CompleteEventHandler(OnComplete);

				TimerObject timer =
					new TimerObject(client, Interval, new ElapsedEventHandler(OnTimer));

				lock (timerList)
				{
					timerList.Add(timer);
				}

				timer.Start();
			}
		}

		/// <summary>
		/// ���X�g����N���C�A���g���폜�B
		/// �w�肵���N���C�A���g�����X�g�ɑ��݂��Ȃ���Ή������Ȃ��B
		/// </summary>
		/// <param name="client">�����X�V�̑Ώۂ���O���N���C�A���g</param>
		public override void Remove(ThreadControl client)
		{
			if (client == null)
			{
				throw new ArgumentNullException("client");
			}

			int index = IndexOf(client);

			if (index != -1)
			{
				// �C�x���g���폜
				client.Complete -= new CompleteEventHandler(OnComplete);
				
				lock (timerList)
				{
					TimerObject timer = timerList[index];
					timer.Dispose();

					timerList.Remove(timer);
				}

			}
		}

		/// <summary>
		/// �w�肵���N���C�A���g�����X�g���ɑ��݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="client">��������N���C�A���g</param>
		/// <returns>���X�g���ɑ��݂����true�A���݂��Ȃ����false��Ԃ�</returns>
		public override bool Contains(ThreadControl client)
		{
			if (client == null)
			{
				throw new ArgumentNullException("client");
			}
			return IndexOf(client) != -1;
		}

		/// <summary>
		/// ���ׂẴ^�C�}�[���폜
		/// </summary>
		public override void Clear()
		{
			lock (timerList)
			{
				foreach (TimerObject timer in timerList)
					timer.Dispose();

				timerList.Clear();
			}
		}

		/// <summary>
		/// �w�肵���N���C�A���g�̃R���N�V�������̈ʒu���擾
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		private int IndexOf(ThreadControl client)
		{
			lock (timerList)
			{
				foreach (TimerObject obj in timerList)
				{
					if (obj.Thread == client)
						return timerList.IndexOf(obj);
				}
			}
			return -1;
		}

		/// <summary>
		/// �X�V�C�x���g����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTimer(object sender, ElapsedEventArgs e)
		{
			TimerObject timer = (TimerObject)sender;

			timer.Stop();
			ThreadControl thread = timer.Thread;

			if (thread.IsOpen)
				thread.Reload();
		}

		/// <summary>
		/// �X���b�h�X�V�����C�x���g����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnComplete(object sender, CompleteEventArgs e)
		{
			ThreadControl thread = (ThreadControl)sender;
			TimerObject timer = null;

			int index = IndexOf(thread);

			if (index >= 0)
			{
				lock (timerList)
				{
					timer = timerList[index];
				}

				if (e.Status == CompleteStatus.Success)
				{
					bool modified = (thread.HeaderInfo.NewResCount > 0);

					// �V���������A�ő僌�X�����z���Ă����ꍇ�A
					// �܂��͍Ď��s�񐔂� 0 ��ɂȂ��Ă���΃^�C�}�[����O��
					if (thread.HeaderInfo.IsLimitOverThread && !modified)
					{
						Remove(thread);
					}
					else if (timer.RetryCount == 0 && !modified)
					{
						Remove(thread);
					}
					else {
						// �V��������ΊԊu���k�߂�A�Ȃ���ΊԊu���̂΂�
						timer.Interval = modified ? (int)(timer.Interval / 1.5) : (int)(timer.Interval * 1.5);

						// �V�����������ꍇ�͍Ď��s�񐔂����Z�b�g�B
						if (modified)
						{
							timer.ResetRetryCount();
							timer.Interval = interval;
						}
						// �ő� 30�� ���z�����ꍇ�͍Ď��s�񐔂����炷
						else if (timer.Interval > MaxInterval)
						{
							timer.Interval = MaxInterval;
							timer.RetryCount--;
						}

						timer.Start();
					}
				}
				else {
					// �X�V�Ɏ��s�����ꍇ�́A�R�񂾂��Ď��s���Ă݂�
					if (timer.RetryCount > 0)
					{
						timer.RetryCount--;
						timer.Start();
					}
					// ����ł��_���Ȃ�Ԋu���ő�ɉ��΂��Ă���ɂR�񂾂��Ď��s�B
					else if (timer.Interval < MaxInterval)
					{
						timer.ResetRetryCount();
						timer.Interval = MaxInterval;
						timer.Start();
					}
					// ����ł��_���Ȃ�I�[�g�����[�h��~
					else
					{
						Remove(thread);
					}
				}
			}
		}
	}
}
