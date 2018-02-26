// WroteThreadHeaderCollection.cs

namespace Twin
{
	using System;
	using System.Collections;

	/// <summary>
	/// WroteThreadHeaderCollection �̊T�v�̐����ł��B
	/// </summary>
	public class WroteThreadHeaderCollection : CollectionBase
	{
		/// <summary>
		/// �w�肵���C���f�b�N�X�̗v�f���擾�܂��͐ݒ�
		/// </summary>
		public WroteThreadHeader this[int index] {
			set {
				if (value == null)
					throw new ArgumentNullException("Indexer");
				List[index] = value;
			}
			get { return (WroteThreadHeader)List[index]; }
		}

		/// <summary>
		/// WroteThreadHeaderCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public WroteThreadHeaderCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			InnerList.Capacity = 100;
		}

		/// <summary>
		/// �R���N�V�����ɗv�f��ǉ�
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public int Add(WroteThreadHeader header)
		{
			return List.Add(header);
		}

		/// <summary>
		/// �R���N�V�����ɗv�f��ǉ�
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(WroteThreadHeaderCollection items)
		{
			InnerList.AddRange(items);
		}
	}
}
