// IPatrolable.cs

namespace Twin.Tools
{
	using System;
	using System.Net;
	using System.Collections.Generic;

	/// <summary>
	/// ���񂷂邽�߂̊�{�N���X��\��
	/// </summary>
	public abstract class PatrolBase
	{
		/// <summary>
		/// �񓯊�����p�̃f���Q�[�g
		/// </summary>
		protected delegate void PatrolInvoker();

		private List<ThreadHeader> itemColleciton;

		private PatrolInvoker method;
		private Cache cache;

		/// <summary>
		/// �L���b�V�������擾
		/// </summary>
		protected Cache Cache {
			get { return cache; }
		}

		/// <summary>
		/// ����Ώۂ̃X���b�h�R���N�V�������擾
		/// </summary>
		public List<ThreadHeader> Items {
			get { return itemColleciton; }
		}

		private bool patrolling;
		public bool IsPatrolling
		{
			get
			{
				return patrolling;
			}
		}
	

		/// <summary>
		/// ��Ԃ��X�V���ꂽ�Ƃ��ɔ���
		/// </summary>
		public event StatusTextEventHandler StatusTextChanged;

		/// <summary>
		/// �A�C�e�������񒆂ɔ���
		/// </summary>
		public event PatrolEventHandler Patroling;

		/// <summary>
		/// �X�V���ꂽ�A�C�e�����������Ƃ��ɔ���
		/// </summary>
		public event PatrolEventHandler Updated;

		/// <summary>
		/// PatrolBase�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cacheInfo"></param>
		protected PatrolBase(Cache cacheInfo)
		{
			itemColleciton = new List<ThreadHeader>();
			cache = cacheInfo;
			method = null;
			patrolling = false;
		}

		/// <summary>
		/// ����Ώۂ̃X���b�h�A�C�e����ݒ�
		/// </summary>
		/// <param name="items"></param>
		public void SetItems(List<ThreadHeader> items)
		{
			if (items == null) {
				throw new ArgumentNullException("items");
			}
			itemColleciton.Clear();
			itemColleciton.AddRange(items);

			// �X���b�h�����ŐV�̏�Ԃɂ���
			for (int i = 0; i < itemColleciton.Count; i++)
				ThreadIndexer.Read(cache, itemColleciton[i]);
		}

		/// <summary>
		/// ����J�n
		/// </summary>
		public abstract void Patrol();

		/// <summary>
		/// �񓯊��ŏ��񂷂�
		/// </summary>
		public IAsyncResult BeginPatrol(AsyncCallback callback, object stateObject)
		{
			method = new PatrolInvoker(Patrol);
			patrolling = true;
			return method.BeginInvoke(callback, stateObject);
		}

		/// <summary>
		/// �񓯊��ȏ�����I������܂Ńu���b�N
		/// </summary>
		public void EndPatrol(IAsyncResult ar)
		{
			if (ar == null) {
				throw new ArgumentNullException("ar");
			}
			if (method == null) {
				throw new InvalidOperationException("�񓯊��ŏ��񂪊J�n����Ă��܂���");
			}

			method.EndInvoke(ar);
			patrolling = false;
			method = null;
		}

		/// <summary>
		/// StatusTextChanged�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnStatusTextChanged(string text)
		{
			if (StatusTextChanged != null)
				StatusTextChanged(this, new StatusTextEventArgs(text));
		}

		/// <summary>
		/// Patroling�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnPatroling(PatrolEventArgs e)
		{
			if (Patroling != null)
				Patroling(this, e);
		}

		/// <summary>
		/// Updated�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnUpdated(PatrolEventArgs e)
		{
			if (Updated != null)
				Updated(this, e);
		}
	}
}
