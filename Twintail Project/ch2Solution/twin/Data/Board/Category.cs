// Category.cs

namespace Twin
{
	using System;
	using System.Collections;
	using System.Runtime.Serialization;

	/// <summary>
	/// �f���̔��܂Ƃ߂�J�e�S����\��
	/// </summary>
	public class Category
	{
		private BoardInfoCollection children;
		private string name;
		private bool isExpanded;

		/// <summary>
		/// �i�[����Ă���q�G���g�������擾
		/// </summary>
		public int Count {
			get {
				return children.Count;
			}
		}

		/// <summary>
		/// �J�e�S�������擾�܂��͐ݒ�
		/// </summary>
		public string Name {
			set {
				if (value == null)
					throw new ArgumentNullException("Name");
				
				name = value;
			}
			get { return name; }
		}

		/// <summary>
		/// ���̃J�e�S���̎q�G���g�����擾
		/// </summary>
		public BoardInfoCollection Children {
			get {
				return children;
			}
		}

		/// <summary>
		/// �t�H���_���W�J����Ă��邩�ǂ������擾�܂��͐ݒ�
		/// </summary>
		public bool IsExpanded {
			set {
				if (value != isExpanded)
					isExpanded = value;
			}
			get { return isExpanded; }
		}

		/// <summary>
		/// Category�N���X�̃C���X�^���X��������
		/// </summary>
		public Category()
		{
			this.children = new BoardInfoCollection();
			this.isExpanded = false;
			this.name = String.Empty;
		}

		/// <summary>
		/// Category�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="name">�J�e�S����</param>
		public Category(string name) : this()
		{
			if (name == null) {
				throw new ArgumentNullException("name");
			}

			this.name = name;
		}

		/// <summary>
		/// �n�b�V���֐�
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// ���̃C���X�^���X��obj���r
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as Category);
		}

		/// <summary>
		/// ���̃C���X�^���X��category���r
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public bool Equals(Category category)
		{
			if (category != null &&
				category.Count == Count)
			{
				for (int i = 0; i < Count; i++)
				{
					BoardInfo board1 = category.Children[i];
					BoardInfo board2 = Children[i];
					if (!board1.Equals(board2))
						return false;
				}
				return true;
			}
			return false;
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
