// AaHeaderCollection.cs

namespace Twin.Aa
{
	using System;
	using System.Collections;

	/// <summary>
	/// AaHeader���R���N�V�����Ǘ�
	/// </summary>
	public class AaHeaderCollection : CollectionBase
	{
		/// <summary>
		/// �w�肵��index�ʒu�̃A�C�e�����擾�܂��͐ݒ�
		/// </summary>
		public AaHeader this[int index] {
			set {
				List[index] = value;
			}
			get { return (AaHeader)List[index]; }
		}

		/// <summary>
		/// AaHeaderCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public AaHeaderCollection()
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
		public int Add(AaHeader item)
		{
			return List.Add(item);
		}

		/// <summary>
		/// items���R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(AaHeaderCollection items)
		{
			foreach (AaHeader item in items)
				List.Add(item);
		}

		/// <summary>
		/// items���R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(AaHeader[] items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// �R���N�V�����̎w�肵��index��item��}��
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, AaHeader item)
		{
			List.Insert(index, item);
		}

		/// <summary>
		/// item���R���N�V��������폜
		/// </summary>
		/// <param name="item"></param>
		public void Remove(AaHeader item)
		{
			List.Remove(item);
		}

		/// <summary>
		/// �R���N�V��������AaItem���\�[�g
		/// </summary>
		public void Sort()
		{
			InnerList.Sort(new AaComparer.AaHeaderComparer());
		}
	}
}
