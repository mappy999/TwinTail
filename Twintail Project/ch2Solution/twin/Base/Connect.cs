// Connect.cs

namespace Twin.Bbs
{
	using System;
	using System.Collections;
	using System.Threading;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using Twin.IO;

	/// <summary>
	/// �ڑ�����
	/// </summary>
	public sealed class Connect
	{
		private int queue;
		private int interval;

		/// <summary>
		/// ���݊Ǘ����Ă���ڑ������擾
		/// </summary>
		public int Count {
			get {
				return queue;
			}
		}

		/// <summary>
		/// Connect�N���X�̃C���X�^���X��������
		/// </summary>
		public Connect()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			interval = 1000;
			queue = 0;
		}

		/// <summary>
		/// �V�����ڑ���ǉ����A�������܂őҋ@
		/// </summary>
		public void Wait(object id)
		{
			// ���̃L���[���I������܂ő҂�
			while (queue > 0)
				Thread.Sleep(interval);

			Interlocked.Increment(ref queue);
		}

		/// <summary>
		/// ���݂̐ڑ����������
		/// </summary>
		//[MethodImpl(MethodImplOptions.Synchronized)]
		public void Release(object id)
		{
			Interlocked.Decrement(ref queue);

			if (queue < 0)
				queue = 0;
		}
	}
}
