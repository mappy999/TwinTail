// BookmarkEntryCollection.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Windows.Forms;

	/// <summary>
	/// ���C�ɓ���G���g���̃R���N�V����
	/// </summary>
	public class BookmarkEntryCollection
	{
		private ArrayList innerList;
		private BookmarkEntry parent;

		/// <summary>
		/// �R���N�V�������̗v�f�����擾
		/// </summary>
		public int Count {
			get {
				return innerList.Count;
			}
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�̂��C�ɓ���t�H���_���擾
		/// </summary>
		public BookmarkEntry this[int index] {
			get {
				return (BookmarkEntry)innerList[index];
			}
		}

		/// <summary>
		/// BookmarkCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkEntryCollection(BookmarkEntry parent)
		{
			if (parent == null) {
				throw new ArgumentNullException("parent");
			}
			if (parent.IsLeaf) {
				throw new ArgumentException("�t��e�ɂ��邱�Ƃ͏o���܂���");
			}
			this.innerList = ArrayList.Synchronized(new ArrayList());
			this.parent = parent;
		}

		/// <summary>
		/// ���C�ɓ���G���g����ǉ�
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(BookmarkEntry item)
		{
			item.Parent = parent;
			return innerList.Add(item);
		}

		/// <summary>
		/// �����̂��C�ɓ���G���g����ǉ�
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(BookmarkEntryCollection items)
		{
			foreach (BookmarkEntry entry in items)
				Add(entry);
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ɂ��C�ɓ����}��
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, BookmarkEntry item)
		{
			item.Parent = parent;
			innerList.Insert(index, item);
		}

		/// <summary>
		/// �R���N�V��������w�肵�����C�ɓ�����폜
		/// </summary>
		/// <param name="item"></param>
		public void Remove(BookmarkEntry item)
		{
			if (innerList.Contains(item))
			{
				item.Parent = null;
				innerList.Remove(item);
			}
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ɂ���v�f���폜
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= innerList.Count)
				throw new ArgumentOutOfRangeException("index");

			BookmarkEntry entry = this[index];
			entry.Parent = null;

			innerList.RemoveAt(index);
		}

		/// <summary>
		/// �w�肵���G���g�����R���N�V�������Ɋ܂܂�Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(BookmarkEntry item)
		{
			return innerList.Contains(item);
		}

		/// <summary>
		/// �w�肵���G���g���̃R���N�V�������C���f�b�N�X���擾
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(BookmarkEntry item)
		{
			return innerList.IndexOf(item);
		}

		/// <summary>
		/// ���C�ɓ�����\�[�g
		/// </summary>
		/// <param name="sorter"></param>
		public void Sort(BookmarkSorter sorter)
		{
			innerList.Sort(sorter);
		}

		/// <summary>
		/// ���C�ɓ�������ׂč폜
		/// </summary>
		public void Clear()
		{
			foreach (BookmarkEntry entry in innerList)
				entry.Parent = null;

			innerList.Clear();
		}

		/// <summary>
		/// BookmarkEntryCollection�𔽕��������邽�߂̗񋓎���Ԃ�
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return innerList.GetEnumerator();
		}
	}
}
