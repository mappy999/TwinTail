// QuickSaveFolderCollection.cs

namespace ImageViewerDll
{
	using System;
	using System.Collections;
	using System.Runtime.Serialization;

	[Serializable]
	public class QuickSaveFolderCollection : CollectionBase, ISerializable
	{
		/// <summary>
		/// �w�肵���C���f�b�N�X�̗v�f���擾
		/// </summary>
		public QuickSaveFolderItem this[int index] {
			set {
				List[index] = value;
			}
			get {
				return (QuickSaveFolderItem)List[index];
			}
		}

		/// <summary>
		/// QuickSaveFolderCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public QuickSaveFolderCollection()
		{
		}

		public QuickSaveFolderCollection(SerializationInfo info, StreamingContext context)
		{
			ArrayList arrayList =
				(ArrayList)info.GetValue(GetType().Name, typeof(ArrayList));

			foreach (object obj in arrayList)
				if (obj != null) InnerList.Add(obj);
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(GetType().Name, InnerList);
		}

		/// <summary>
		/// item���R���N�V�����̖����ɒǉ�
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(QuickSaveFolderItem item)
		{
			return List.Add(item);
		}

		/// <summary>
		/// item���R���N�V����������폜
		/// </summary>
		/// <param name="item"></param>
		public void Remove(QuickSaveFolderItem item)
		{
			List.Remove(item);
		}
	}
}
