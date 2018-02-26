// BookmarkEntry.cs

namespace Twin
{
	using System;
	using System.Collections;

	/// <summary>
	/// ���C�ɓ���̊�{���ۃN���X
	/// </summary>
	public abstract class BookmarkEntry
	{
		/// <summary>
		/// ���ׂĂ̂��C�ɓ��肪�o�^����Ă���e�[�u��
		/// </summary>
		private static readonly Hashtable hash;
		private static readonly Random random;

		private int id;
		private object tag;

		/// <summary>
		/// ���̃G���g�����i�[����Ă���e�t�H���_���擾�܂��͐ݒ�
		/// </summary>
		public abstract BookmarkEntry Parent {
			set;
			get;
		}

		/// <summary>
		/// ���̃G���g���̎q�R���N�V�������擾
		/// </summary>
		public abstract BookmarkEntryCollection Children {
			get;
		}

		/// <summary>
		/// ���̃G���g���̖��O���擾�܂��͐ݒ�
		/// </summary>
		public abstract string Name {
			set;
			get;
		}

		/// <summary>
		/// ���̃C���X�^���X���t���ǂ����𔻒f
		/// </summary>
		public abstract bool IsLeaf {
			get;
		}

		/// <summary>
		/// ���̂��C�ɓ�������ʂ���ID���擾
		/// </summary>
		public int Id {
			get {
				return id;
			}
		}

		/// <summary>
		/// �^�O���擾�܂��͐ݒ�
		/// </summary>
		public object Tag {
			set { tag = value; }
			get { return tag; }
		}

		static BookmarkEntry()
		{
			hash = new Hashtable();
			random = new Random();
		}

		/// <summary>
		/// BookmarkEntry�N���X�̃C���X�^���X��������
		/// </summary>
		protected BookmarkEntry()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			id = GetRandomId();
			hash[id] = this;
		}

		/// <summary>
		/// BookmarkEntry�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="entryid">���̂��C�ɓ���Ƃ��Ԃ�Ȃ�Id���w��B-1�ɂ���ƃ����_���ɐݒ肳���B</param>
		protected BookmarkEntry(int entryid)
		{
			if (entryid == -1)
				entryid = GetRandomId();

			if (hash.Contains(entryid))
				throw new ArgumentException("Id:" + entryid + "�͑��̂��C�ɓ����Id�Əd�����Ă��܂�");

			id = entryid;
			hash[id] = this;
		}

		/// <summary>
		/// ���̃G���g����e����폜
		/// </summary>
		public abstract void Remove();

		/// <summary>
		/// ���̃G���g���𕡐�
		/// </summary>
		/// <returns></returns>
		public abstract BookmarkEntry Clone();

		/// <summary>
		/// ���ɓo�^����Ă��邨�C�ɓ���ɏd�����Ȃ������_����ID���擾
		/// </summary>
		/// <returns></returns>
		protected static int GetRandomId()
		{
			int id;
			do {
				id = random.Next();
			}
			while (hash.ContainsKey(id) || id == -1);

			return id;
		}

		/// <summary>
		/// �w�肵��ID�������C�ɓ��肪�o�^����Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool Contains(int id)
		{
			return hash.ContainsKey(id);
		}

		/// <summary>
		/// �w�肵��ID�����G���g�����擾
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static BookmarkEntry GetEntryOf(int id)
		{
			return hash[id] as BookmarkEntry;
		}

		/// <summary>
		/// �w�肵�����C�ɓ���ɐV����ID��ݒ肷��
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="newid"></param>
		public static void SetEntryId(BookmarkEntry entry, int newid)
		{
			if (entry.id == newid)
				return;

			if (hash.ContainsKey(newid))
				throw new ArgumentException("Id:" + newid + "�͑��̂��C�ɓ���Əd�����Ă��܂�");

			entry.id = newid;

			hash.Remove(entry.id);
			hash[newid] = entry;
		}

		/// <summary>
		/// ���̃C���X�^���X�̃n�b�V���l��Ԃ�
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return id;
		}

		/// <summary>
		/// ���̃C���X�^���X��obj�����������ǂ����𔻒f
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			BookmarkEntry entry = obj as BookmarkEntry;
			return (entry != null) ? (this.id == entry.id) : false;
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Name;
		}
	}
}
