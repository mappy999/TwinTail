// WroteResCollection.cs

namespace Twin
{
	using System;
	using System.Collections;

	/// <summary>
	/// WroteRes�̃R���N�V������\��
	/// </summary>
	public class WroteResCollection : CollectionBase
	{
		private int totalSize;

		/// <summary>
		/// �w�肵���C���f�b�N�X�̃��X�������擾
		/// </summary>
		public WroteRes this[int index] {
			get {
				return (WroteRes)List[index];
			}
		}

		/// <summary>
		/// ���X�̍��v�T�C�Y���擾
		/// </summary>
		public int TotalSize {
			get {
				return totalSize;
			}
		}

		/// <summary>
		/// WroteResCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public WroteResCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			totalSize = 0;
		}

		/// <summary>
		/// �w�肵��res���R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="res"></param>
		/// <returns></returns>
		public int Add(WroteRes res)
		{
			totalSize += res.Length;
			return List.Add(res);
		}

		/// <summary>
		/// WroteRes�R���N�V�������R���N�V�����ɒǉ�
		/// </summary>
		public void AddRange(WroteResCollection resCollection)
		{
			foreach (WroteRes r in resCollection)
				totalSize += r.Length;
			InnerList.AddRange(resCollection);
		}

		/// <summary>
		/// ���ׂĂ̗v�f���R���N�V��������폜
		/// </summary>
		public new void Clear()
		{
			totalSize = 0;
			List.Clear();
		}
	}
}
