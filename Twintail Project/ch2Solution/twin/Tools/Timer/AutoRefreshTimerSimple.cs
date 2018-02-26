// AutoRefreshTimerSimple.cs

namespace Twin.Tools
{
	using System;
	using System.Timers;
	using System.Collections;

	/// <summary>
	/// ThreadControl���^�C�}�[�Œ���I�ɍX�V����@�\��񋟁B
	/// ���ɏ��׍H�Ȃ��̃V���v���ȍ\���B
	/// </summary>
	public class AutoRefreshTimerSimple : AutoRefreshTimerBase
	{
		private ArrayList list;
		private Timer timer;
		private int interval;

		/// <summary>
		/// �X�V�Ԋu���~���b�P�ʂŎ擾�܂��͐ݒ�B
		/// �ŏ��l��5000 (5�b)�B
		/// </summary>
		public override int Interval {
			set {
				interval = Math.Max(5000, value);
				timer.Interval = interval;
			}
			get { return interval; }
		}

		/// <summary>
		/// AutoRefreshTimerSimple�N���X�̃C���X�^���X��������
		/// </summary>
		public AutoRefreshTimerSimple()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			list = ArrayList.Synchronized(new ArrayList());
			timer = new Timer();
			timer.Elapsed += new ElapsedEventHandler(OnTimer);

			// �����l��10�b
			Interval = 10000;
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

			if (list.IndexOf(client) == -1)
			{
				// �����C�x���g�ɓo�^
				client.Complete += new CompleteEventHandler(OnComplete);
				list.Add(client);
			}

			if (! timer.Enabled)
				timer.Start();
		}

		/// <summary>
		/// ���X�g����N���C�A���g���폜�B
		/// �w�肵���N���C�A���g�����X�g�ɑ��݂��Ȃ���Ή������Ȃ��B
		/// </summary>
		/// <param name="client">�����X�V�̑Ώۂ���O���N���C�A���g</param>
		public override void Remove(ThreadControl client)
		{
			if (list.Contains(client))
			{
				list.Remove(client);
				client.Complete -= new CompleteEventHandler(OnComplete);
			}

			if (list.Count == 0)
				timer.Stop();
		}

		/// <summary>
		/// �w�肵���N���C�A���g�����X�g���ɑ��݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="client">��������N���C�A���g</param>
		/// <returns>���X�g���ɑ��݂����true�A���݂��Ȃ����false��Ԃ�</returns>
		public override bool Contains(ThreadControl client)
		{
			return list.Contains(client);
		}

		/// <summary>
		/// ���ׂẴ^�C�}�[���폜
		/// </summary>
		public override void Clear()
		{
			timer.Dispose();
			list.Clear();
		}

		/// <summary>
		/// �^�C�}�[������������L���[������o���X�V
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTimer(object sender, ElapsedEventArgs e)
		{
			timer.Stop();

			if (list.Count > 0)
			{
				// �X�V�Ώۂ̃A�C�e�����擾
				ThreadControl thread = (ThreadControl)list[0];

				// �X���b�h���J����Ă��āA�ǂݍ��ݒ��łȂ��ꍇ�̂ݍX�V
				if (thread.IsOpen)
				{
					thread.Reload();
				}
				// �X���b�h���J����Ă��Ȃ���΍폜
				else {
					list.Remove(thread);
				}
			}
		}

		/// <summary>
		/// �X�V������������ēx�L���[�ɒǉ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnComplete(object sender, CompleteEventArgs e)
		{
			// �ǂݍ��݊��������疖���ɒǉ�
			list.Remove(sender);
			list.Add(sender);
			timer.Start();
		}
	}
}
