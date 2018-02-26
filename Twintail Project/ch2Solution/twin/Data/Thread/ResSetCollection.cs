// ResSetCollection.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// ResSet�\���̂��R���N�V�����Ǘ�
	/// </summary>
	public class ResSetCollection : CollectionBase, IEnumerable<ResSet>
	{
		/// <summary>
		/// �R���N�V�����𓯊������邽�߂̃I�u�W�F�N�g���擾
		/// </summary>
		public object SyncRoot {
			get {
				return List.SyncRoot;
			}
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�̃��X���擾�܂��͐ݒ�
		/// </summary>
		public ResSet this[int index] {
			set {
				List[index] = value;
			}
			get {
				return (ResSet)List[index];
			}
		}

		/// <summary>
		/// ���ׂĂ�Visible�t���O��ݒ�
		/// </summary>
		public bool Visible {
			set {
				for (int i = 0; i < Count; i++)
				{
					ResSet res = this[i];
					res.Visible = value;

					this[i] = res;
				}
			}
		}

		/// <summary>
		/// ���ׂĂ̐V���t���O��ݒ�
		/// </summary>
		public bool IsNew {
			set {
				for (int i = 0; i < Count; i++)
				{
					ResSet res = this[i];
					res.IsNew = value;

					this[i] = res;
				}
			}
		}

		/// <summary>
		/// ResSetCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public ResSetCollection()
		{
			InnerList.Capacity = 1000;
		}

		public ResSetCollection(ResSetCollection coll)
		{
			InnerList.Capacity = coll.Count;
			AddRange(coll);
		}

		/// <summary>
		/// ���X���R���N�V�����ɒǉ�
		/// </summary>
		/// <param name="resSet">�ǉ�����ResSet�\����</param>
		/// <returns>�ǉ������C���f�b�N�X�l</returns>
		public int Add(ResSet resSet)
		{
			return List.Add(resSet);
		}

		/// <summary>
		/// ResSet�\���̂̔z����R���N�V�����ɒǉ�
		/// </summary>
		public void AddRange(ResSet[] items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// ResSet�R���N�V�������R���N�V�����ɒǉ�
		/// </summary>
		public void AddRange(ResSetCollection resCollection)
		{
			InnerList.AddRange(resCollection);
		}

		/// <summary>
		/// ���ׂẴ��X�̋t�Q�Ɖ� (BackReferenceCount �v���p�e�B) �����Z�b�g���܂��B
		/// </summary>
		public void ResetBackReferenceCount()
		{
			for (int i = 0; i < Count; i++)
			{
				ResSet r = this[i];
				r.BackReferencedList.Clear();

				this[i] = r;
			}
		}

		/// <summary>
		/// �w�肵�� 1 ����n�܂郌�X�ԍ����w�肵�ă��X���擾�B
		/// ���̃��\�b�h�́A�R���N�V�����̏����ƃ��X�ԍ��̏�������v���Ă���ꍇ�݂̂Ɏg�p�B
		/// �����łȂ��ꍇ�́Aindices �Ŏw�肵�Ă��A�Ԃ����R���N�V�����̃��X�ԍ��ƈ�v���Ă���ۏ؂͖����B
		/// �w�肵�����X�ԍ��̃f�[�^���~�����ꍇ�́AGetRangeOfNumber ���\�b�h���g�p����B
		/// </summary>
		/// <param name="indices"></param>
		/// <returns></returns>
		public ResSetCollection GetRange(int[] indices)
		{
			if (indices == null) {
				throw new ArgumentNullException("indices");
			}

			ResSetCollection result = 
				new ResSetCollection();

			foreach (int index in indices)
			{
				if (index > 0 && index <= Count)
				{
					result.Add(this[index - 1]);
				}
			}

			return result;
		}

		/// <summary>
		/// 1 ����n�܂郌�X�ԍ��̎w�肵���͈͂��擾�B
		/// ���̃��\�b�h�́A�R���N�V�����̏����ƃ��X�ԍ��̏�������v���Ă���ꍇ�݂̂Ɏg�p�B
		/// �����łȂ��ꍇ�́AbeginIndex �� endIndex �Ŏw�肵�Ă��A�Ԃ����R���N�V�����̃��X�ԍ��ƈ�v���Ă���ۏ؂͖����B
		/// �w�肵�����X�ԍ��̃f�[�^���~�����ꍇ�́AGetRangeOfNumber ���\�b�h���g�p����B
		/// </summary>
		/// <param name="beginIndex"></param>
		/// <param name="endIndex"></param>
		/// <returns></returns>
		public ResSetCollection GetRange(int beginIndex, int endIndex)
		{
			if (beginIndex < 1 || endIndex < 1 || beginIndex > endIndex)
				throw new ArgumentOutOfRangeException();

			var result = new ResSetCollection();
			for (int i = beginIndex; i <= endIndex; i++)
				result.Add(this[i - 1]);

			return result;
		}

		public ResSetCollection GetRangeOfNumber(int beginResNumber, int endResNumber)
		{
			if (beginResNumber < 1 || endResNumber < 1 || beginResNumber > endResNumber)
				throw new ArgumentOutOfRangeException();

			var num = new List<int>();
			for (int i = beginResNumber; i <= endResNumber; i++) num.Add(i);
			return GetRangeOfNumber(num.ToArray());
		}

		public ResSetCollection GetRangeOfNumber(int[] resNumbers)
		{
			var result = new ResSetCollection();

			foreach (ResSet res in List)
			{
				foreach (int n in resNumbers)
				{
					if (res.Index == n)
						result.Add(res);
				}
			}

			return result;
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�����ځ[��
		/// </summary>
		/// <param name="index">�폜���郌�X�ԍ�</param>
		/// <param name="visible">�����ځ[��Ȃ�true</param>
		public ResSet ABone(int index, bool visible, ABoneType type, string description)
		{
			int st = 0;
			int ed = Count-1;

			while (st <= ed)
			{
				int mid = (st + ed) / 2;
				ResSet res = this[mid];

				if (res.Index > index)
					ed = mid - 1;
				else if (res.Index < index)
					st = mid + 1;
				else {
					st = mid;
					break;
				}
			}

			// ���ځ[��
			this[st] = ResSet.ABone(this[st], visible, type, description);

			return this[st];
		}

		public int[] GetBackReferences(int index)
		{
			List<int> indices = new List<int>();

			foreach (ResSet res in this)
			{
				if (res.RefIndices.Length >= 50)
					continue;

				foreach (int n in res.RefIndices)
				{
					if (n == index && !indices.Contains(res.Index))
					{
						indices.Add(res.Index);
					}
				}
			}
			return indices.ToArray();
		}

		IEnumerator<ResSet> IEnumerable<ResSet>.GetEnumerator()
		{
			for (int i = 0; i < List.Count; i++)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return List.GetEnumerator();
		}

		public int[] IndicesFromID(string idstr)
		{
			var indices = new List<int>();
			foreach (ResSet res in this)
				if (res.ID == idstr) indices.Add(res.Index);
			return indices.ToArray();
		}
	}
}
