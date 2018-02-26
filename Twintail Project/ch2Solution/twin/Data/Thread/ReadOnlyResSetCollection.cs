// ReadOnlyResSetCollection.cs

namespace Twin
{
	using System;
	using System.Collections;

	/// <summary>
	/// �ǂݎ���p�̃R���N�V������\��
	/// </summary>
	public class ReadOnlyResSetCollection
	{
		private ResSetCollection collection;

		public object SyncRoot {
			get {
				return collection.SyncRoot;
			}
		}

		public int Count {
			get {
				return collection.Count;
			}
		}

		public ResSet this[int index] {
			get {
				return (ResSet)collection[index];
			}
		}

		/// <summary>
		/// ReadOnlyResSetCollection�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="items"></param>
		public ReadOnlyResSetCollection(ResSetCollection items)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			collection = items;
			//list = ArrayList.ReadOnly(list);
		}

		public IEnumerator GetEnumerator()
		{
			return collection.GetEnumerator();
		}


	}
}
