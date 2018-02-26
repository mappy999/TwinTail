// SortedValueCollection.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Text;
	using System.Collections.Generic;

	/// <summary>
	/// �\�[�g���ꂽ���l�^���Ǘ�����R���N�V����
	/// </summary>
	public class SortedValueCollection<T> : IEnumerable<T>
	{
		private List<T> values;

		/// <summary>
		/// �v�f�����擾
		/// </summary>
		public int Count
		{
			get
			{
				return values.Count;
			}
		}

		/// <summary>
		/// SortedValueCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public SortedValueCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			values = new List<T>();
		}

		/// <summary>
		/// �C���f�b�N�Xindex���R���N�V�����̖����ɒǉ�
		/// </summary>
		/// <param name="index"></param>
		public void Add(T index)
		{
			values.Add(index);
			values.Sort();
		}

		/// <summary>
		/// �C���f�b�N�X�z��array���R���N�V�����̖����ɒǉ�
		/// </summary>
		/// <param name="array"></param>
		public void AddRange(T[] array)
		{
			values.AddRange(array);
			values.Sort();
		}

		public void SetRange(T[] newArray)
		{
			values.Clear();
			AddRange(newArray);
		}

		/// <summary>
		/// index���R���N�V�����Ɋ܂܂�Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool Contains(T index)
		{
			//return indices.Contains(index);
			return values.BinarySearch(index) >= 0;
		}

		/// <summary>
		/// array�̒��̂����ꂩ1���R���N�V�����Ɋ܂܂�Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public bool ContainsAny(T[] array)
		{
			foreach (T index in array)
				if (Contains(index)) return true;

			return false;
		}

		/// <summary>
		/// val ���R���N�V����������폜
		/// </summary>
		/// <param name="val"></param>
		public void Remove(T val)
		{
			values.Remove(val);
		}

		/// <summary>
		/// �R���N�V���������ׂč폜
		/// </summary>
		public void Clear()
		{
			values.Clear();
		}

		public void Copy(SortedValueCollection<T> dest)
		{
			dest.Clear();
			dest.AddRange(values.ToArray());
		}

		/// <summary>
		/// �R���N�V�����ɓo�^����Ă���ԍ��𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public string ToArrayString()
		{
			StringBuilder sb = new StringBuilder();

			foreach (T val in values)
				sb.Append(val).Append(',');

			return sb.ToString();
		}

		/// <summary>
		/// ToArrayString �ŕ�����ɕϊ����ꂽ SortedValueCollection �̒l�𕜌�
		/// </summary>
		/// <param name="arrayString"></param>
		public void FromArrayString(string arrayString)
		{
			string[] indices = arrayString.Split(',');
			Clear();

			foreach (string index in indices)
			{
				if (!String.IsNullOrEmpty(index))
				{
					try
					{
						T value = (T)Convert.ChangeType(index, typeof(T));
						Add(value);
					}
					catch (InvalidCastException) {}
				}
			}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}
	}
}
