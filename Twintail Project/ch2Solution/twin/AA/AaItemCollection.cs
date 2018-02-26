// AaItemCollection.cs

namespace Twin.Aa
{
	using System;
	using System.Collections;

	/// <summary>
	/// AaItem���R���N�V�����Ǘ�
	/// </summary>
	public class AaItemCollection : CollectionBase
	{
		/// <summary>
		/// �w�肵��index�ʒu�̃A�C�e�����擾�܂��͐ݒ�
		/// </summary>
		public AaItem this[int index] {
			set {
				List[index] = value;
			}
			get { return (AaItem)List[index]; }
		}

		/// <summary>
		/// �A�C�e�����ǉ����ꂽ�Ƃ��ɔ���
		/// </summary>
		internal event AaItemSetEventHandler ItemSet;

		/// <summary>
		/// AaItemCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public AaItemCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			InnerList.Capacity = 50;
		}

		/// <summary>
		/// item���R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(AaItem item)
		{
			OnSetItemEvent(this, new AaItemSetEventArgs(item));
			return List.Add(item);
		}

		/// <summary>
		/// items���R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(AaItemCollection items)
		{
			foreach (AaItem item in items)
				Add(item);
		}

		/// <summary>
		/// items���R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(AaItem[] items)
		{
			foreach (AaItem item in items)
				Add(item);
			//InnerList.AddRange(items);
		}

		/// <summary>
		/// �R���N�V�����̎w�肵��index��item��}��
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, AaItem item)
		{
			List.Insert(index, item);
			OnSetItemEvent(this, new AaItemSetEventArgs(item));
		}

		/// <summary>
		/// item���R���N�V��������폜
		/// </summary>
		/// <param name="item"></param>
		public void Remove(AaItem item)
		{
			List.Remove(item);
			item.parent = null;
		}

		/// <summary>
		/// item���������C���f�b�N�X�l���擾
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(AaItem item)
		{
			return List.IndexOf(item);
		}

		/// <summary>
		/// �R���N�V��������AaItem���\�[�g
		/// </summary>
		public void Sort()
		{
			InnerList.Sort(new AaComparer.AaItemComparer());
		}

//		�Ȃ񂩌Ă΂�Ȃ��c
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			base.OnSetComplete(index, oldValue, newValue);
			OnSetItemEvent(this, new AaItemSetEventArgs((AaItem)newValue));
		}

		private void OnSetItemEvent(object sender, AaItemSetEventArgs e)
		{
			if (ItemSet != null)
				ItemSet(sender, e);
		}
	}
}
