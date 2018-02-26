// BookmarkThread.cs

namespace Twin
{
	using System;

	/// <summary>
	/// �X���b�h�̂��C�ɓ����\��
	/// </summary>
	public class BookmarkThread : BookmarkEntry
	{
		private ThreadHeader header;
		private BookmarkEntry parent;
		private string name;

		/// <summary>
		/// ���̃C���X�^���X�̐e���擾�܂��͐ݒ�
		/// </summary>
		public override BookmarkEntry Parent {
			set { parent = value; }
			get { return parent; }
		}

		/// <summary>
		/// ���̃v���p�e�B�͎g�p�ł��܂���
		/// </summary>
		public override BookmarkEntryCollection Children {
			get { throw new NotSupportedException("Children�v���p�e�B�̓T�|�[�g���Ă��܂���"); }
		}

		/// <summary>
		/// ���̃v���p�e�B�͏��true��Ԃ�
		/// </summary>
		public override bool IsLeaf {
			get { return true; }
		}

		/// <summary>
		/// ���̋C�ɓ���X���b�h�̖��O���擾�܂��͐ݒ�
		/// </summary>
		public override string Name {
			set {
				if (value == null)
					throw new ArgumentNullException("Name");
				name = value;
			}
			get { return name; }
		}

		/// <summary>
		/// ���C�ɓ���̃X���b�h�����擾
		/// </summary>
		public ThreadHeader HeaderInfo {
			get { return header; }
		}

		/// <summary>
		/// BookmarkThread�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header"></param>
		/// <param name="id"></param>
		public BookmarkThread(ThreadHeader header, int id) : base(id)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.header = header;
			this.name = header.Subject;
			this.parent = null;
		}

		/// <summary>
		/// BookmarkThread�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header"></param>
		public BookmarkThread(ThreadHeader header) : this(header, -1)
		{
		}

		/// <summary>
		/// ���̃C���X�^���X��e����폜
		/// </summary>
		public override void Remove()
		{
			if (parent != null)
				parent.Children.Remove(this);
		}

		/// <summary>
		/// ���̂��C�ɓ���G���g���𕡐�
		/// </summary>
		/// <returns></returns>
		public override BookmarkEntry Clone()
		{
			BookmarkThread clone = new BookmarkThread(header);
			clone.name = this.name;

			return clone;
		}
	}
}
