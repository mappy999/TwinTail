// CategoryCollection.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Runtime.Serialization;

	/// <summary>
	/// Category�N���X���R���N�V�����Ǘ�
	/// </summary>
	public class CategoryCollection : CollectionBase
	{
		/// <summary>
		/// �w�肵���C���f�b�N�X�̃{�[�h�A�C�e�����擾
		/// </summary>
		public Category this[int index] {
			get {
				return (Category)List[index];
			}
		}

		/// <summary>
		/// CategoryCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public CategoryCollection() 
		{
		}

		/// <summary>
		/// �R���N�V�����ɃJ�e�S����ǉ�
		/// </summary>
		/// <param name="item">�ǉ�����Category�N���X</param>
		/// <returns>�ǉ����ꂽ�ʒu</returns>
		public int Add(Category item)
		{
			return List.Add(item);
		}

		/// <summary>
		/// �����̃J�e�S����ǉ�
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(CategoryCollection items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ɃJ�e�S����}��
		/// </summary>
		/// <param name="index">�}������C���f�b�N�X</param>
		/// <param name="item">�}������Category�N���X</param>
		public void Insert(int index, Category item)
		{
			List.Insert(index, item);
		}

		/// <summary>
		/// �w�肵���J�e�S�����폜
		/// </summary>
		/// <param name="item">�폜����J�e�S��</param>
		public void Remove(Category item)
		{
			List.Remove(item);
		}
	}
}
