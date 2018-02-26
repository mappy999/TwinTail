// BookmarkFolder.cs

namespace Twin
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Forms;

	/// <summary>
	/// ���C�ɓ���t�H���_���Ǘ�
	/// </summary>
	public class BookmarkFolder : BookmarkEntry
	{
		private BookmarkEntryCollection children;
		private BookmarkEntry parent;
		private SortOrder sortorder;
		private string name;
		private bool expanded;

		/// <summary>
		/// ���̃C���X�^���X�̐e�t�H���_���擾�܂��͐ݒ�
		/// </summary>
		public override BookmarkEntry Parent
		{
			set
			{
				parent = value;
			}
			get
			{
				return parent;
			}
		}

		/// <summary>
		/// ���̃G���g���Ɋi�[����Ă���R���N�V�������擾
		/// </summary>
		public override BookmarkEntryCollection Children
		{
			get
			{
				return children;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B�͏��false��Ԃ�
		/// </summary>
		public override bool IsLeaf
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// ���̋C�ɓ���t�H���_�̖��O���擾�܂��͐ݒ�
		/// </summary>
		public override string Name
		{
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Name");
				}
				name = value;
			}
			get
			{
				return name;
			}
		}

		/// <summary>
		/// ���̃t�H���_���W�J��Ԃ��ǂ�����\���l���擾�܂��͐ݒ�
		/// </summary>
		public bool Expanded
		{
			set
			{
				if (expanded != value)
					expanded = value;
			}
			get
			{
				return expanded;
			}
		}

		/// <summary>
		/// BookmarkFolder�N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkFolder()
			: this(String.Empty, -1)
		{
		}

		/// <summary>
		/// BookmarkFolder�N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkFolder(string name, int id)
			: base(id)
		{
			this.children = new BookmarkEntryCollection(this);
			this.sortorder = SortOrder.Ascending;
			this.expanded = false;
			this.parent = null;
			this.name = name;
		}

		/// <summary>
		/// BookmarkFolder�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="id"></param>
		public BookmarkFolder(int id)
			: this(String.Empty, id)
		{
		}

		/// <summary>
		/// BookmarkFolder�N���X�̃C���X�^���X��������
		/// </summary>
		public BookmarkFolder(string name)
			: this(name, -1)
		{
		}

		/// <summary>
		/// ���̃C���X�^���X��e����폜
		/// </summary>
		public override void Remove()
		{
			if (parent == null)
			{
				throw new InvalidOperationException("���̃C���X�^���X�ɐe���ݒ肳��Ă��܂���");
			}
			parent.Children.Remove(this);
		}

		/// <summary>
		/// ���̃t�H���_�𕡐�
		/// </summary>
		/// <returns></returns>
		public override BookmarkEntry Clone()
		{
			BookmarkFolder clone = new BookmarkFolder(name);
			clone.Children.AddRange(children);

			return clone;
		}

		/// <summary>
		/// �w�肵���\�[�g���@�ŏ����\�[�g
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="order"></param>
		public void Sort(BookmarkSortObject obj)
		{
			children.Sort(new BookmarkSorter(obj, sortorder));
			sortorder = (sortorder != SortOrder.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
		}

		/// <summary>
		/// �w�肵���\�[�g���@�ŏ����\�[�g
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="order"></param>
		public void Sort(BookmarkSortObject obj, SortOrder order)
		{
			sortorder = order;
			Sort(obj);
		}

		/// <summary>
		/// ���̃t�H���_�Ɋ܂܂�Ă���q�̐����擾
		/// </summary>
		/// <param name="includeSubChilren">�T�u�t�H���_���܂߂�ꍇ��true</param>
		/// <returns></returns>
		public int GetChildCount(bool includeSubChildren)
		{
			int result = children.Count;

			if (includeSubChildren)
				foreach (BookmarkEntry entry in children)
					if (!entry.IsLeaf)
						result += ((BookmarkFolder)entry).GetChildCount(true);

			return result;
		}

		/// <summary>
		/// ���̃t�H���_�̂��C�ɓ�����擾
		/// </summary>
		/// <param name="includeSubChildren">�T�u�t�H���_���܂߂�ꍇ��true</param>
		/// <returns></returns>
		public List<ThreadHeader> GetBookmarks(bool includeSubChildren)
		{
			List<ThreadHeader> items =
				new List<ThreadHeader>();

			foreach (BookmarkEntry entry in children)
			{
				if (entry.IsLeaf)
				{
					BookmarkThread thread = (BookmarkThread)entry;
					items.Add(thread.HeaderInfo);
				}
				else if (includeSubChildren)
				{
					BookmarkFolder folder = (BookmarkFolder)entry;
					items.AddRange(folder.GetBookmarks(includeSubChildren));
				}
			}
			return items;
		}
	}
}
