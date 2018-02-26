// ObjectTimer.cs

namespace Twin.Bbs
{
	using System;
	using System.Timers;

	/// <summary>
	/// �^�C�}�[�C�x���g��object�^��n���N���X
	/// </summary>
	public class ObjectTimer
	{
		private Timer timer;
		private object tag;

		/// <summary>
		/// �^�C�}�[�Ԋu���擾�܂��͐ݒ�
		/// </summary>
		public int Interval {
			set {
				if (value < 1) {
					throw new ArgumentOutOfRangeException("Interval");
				}
				timer.Interval = value;
			}
			get { return (int)timer.Interval; }
		}

		/// <summary>
		/// ���Ԃ��o�߂����甭��
		/// </summary>
		public event ObjectTimerEventHandler Elapsed;

		/// <summary>
		/// ObjectTimer�N���X�̃C���X�^���X��������
		/// </summary>
		public ObjectTimer()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			timer = new Timer();
			timer.Interval = 250;
			timer.Elapsed += new ElapsedEventHandler(OnElapsed);
		}

		/// <summary>
		/// �^�C�}�[�J�n
		/// </summary>
		public void Start(object obj)
		{
			tag = obj;
			timer.Start();
		}

		/// <summary>
		/// �^�C�}�[�I��
		/// </summary>
		public void Stop()
		{
			timer.Stop();
		}

		private void OnElapsed(object sender, ElapsedEventArgs e)
		{
			timer.Stop();

			if (Elapsed != null)
				Elapsed(this, new ObjectTimerEventArgs(tag));
		}
	}

	/// <summary>
	/// ObjectTimer.Elapsed�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void ObjectTimerEventHandler(object sender, ObjectTimerEventArgs e);

	/// <summary>
	/// ObjectTimer.Elapsed�C�x���g�̃f�[�^���
	/// </summary>
	public class ObjectTimerEventArgs : EventArgs
	{
		private readonly object tag;

		/// <summary>
		/// �^�O���擾
		/// </summary>
		public object Tag {
			get { return tag; }
		}

		/// <summary>
		/// ObjectTimerEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="obj"></param>
		public ObjectTimerEventArgs(object obj)
		{
			tag = obj;
		}
	}
}
