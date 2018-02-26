// CSharpToolBarButton.cs

namespace CSharpSamples
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing.Design;
	using System.Drawing;
	using System.Windows.Forms;

	/// <summary>
	/// CSharpToolBar �̃{�^����\��
	/// </summary>
	[DesignTimeVisible(false)]
	public class CSharpToolBarButton : Component, ICloneable
	{
		#region InnerClass
		/// <summary>
		/// CSharpToolBarButton���i�[����R���N�V����
		/// </summary>
		public class CSharpToolBarButtonCollection : ICollection, IList, IEnumerable
		{
			private CSharpToolBar toolBar;
			private ArrayList innerList;

			/// <summary>
			/// �R���N�V�����Ɋi�[����Ă���{�^�������擾
			/// </summary>
			public int Count
			{
				get
				{
					return innerList.Count;
				}
			}

			/// <summary>
			/// �w�肵���C���f�b�N�X�̃{�^�����擾�܂��͐ݒ�
			/// </summary>
			public CSharpToolBarButton this[int index]
			{
				get
				{
					return innerList[index] as CSharpToolBarButton;
				}
			}

			/// <summary>
			/// CSharpToolBarButtonCollection�N���X�̃C���X�^���X��������
			/// </summary>
			/// <param name="toolbar">���̃R���N�V�����̐e�ɂȂ�CSharpToolBar�N���X�̃C���X�^���X</param>
			internal CSharpToolBarButtonCollection(CSharpToolBar toolbar)
			{
				if (toolbar == null)
					throw new ArgumentNullException("toolbar");

				toolBar = toolbar;
				innerList = new ArrayList();
			}

			/// <summary>
			/// �c�[���o�[�̖����Ƀ{�^����ǉ�
			/// </summary>
			/// <param name="button">�c�[���o�[�ɒǉ�����{�^��</param>
			/// <returns>�{�^�����ǉ����ꂽ�R���N�V�������̃C���f�b�N�X</returns>
			public int Add(CSharpToolBarButton button)
			{
				if (button == null)
				{
					throw new ArgumentNullException("button");
				}
				if (button.toolBar != null)
				{
					throw new ArgumentException("���̃{�^���͊��ɑ��̃c�[���o�[�ɓo�^����Ă��܂�");
				}

				int index = innerList.Add(button);

				button.toolBar = toolBar;
				button.imageList = toolBar.ImageList;

				toolBar.UpdateButtons();

				return index;
			}

			/// <summary>
			/// �c�[���o�[��CSharpToolBarButton�̔z���ǉ�
			/// </summary>
			/// <param name="buttons"></param>
			public void AddRange(CSharpToolBarButton[] buttons)
			{
				foreach (CSharpToolBarButton but in buttons)
					Add(but);
			}

			/// <summary>
			/// �c�[���o�[�̎w�肵���C���f�b�N�X�̈ʒu�Ƀ{�^����}��
			/// </summary>
			/// <param name="index">button��}������0����n�܂�C���f�b�N�X�ԍ�</param>
			/// <param name="button">�}������CSharpToolBarButton</param>
			public void Insert(int index, CSharpToolBarButton button)
			{
				if (index < 0 || index > Count)
					throw new ArgumentOutOfRangeException("index");

				button.imageList = toolBar.ImageList;
				button.toolBar = toolBar;

				innerList.Insert(index, button);
				toolBar.UpdateButtons();
			}

			/// <summary>
			/// �{�^�����c�[���o�[����폜
			/// </summary>
			/// <param name="button">�c�[���o�[����폜����CSharpToolBarButton</param>
			public void Remove(CSharpToolBarButton button)
			{
				int index = innerList.IndexOf(button);
				if (index >= 0)
				{
					button.toolBar = null;
					button.imageList = null;

					innerList.Remove(button);
					toolBar.UpdateButtons();
				}
			}

			/// <summary>
			/// �c�[���o�[����w�肵���C���f�b�N�X�ɂ���{�^�����폜
			/// </summary>
			/// <param name="index">�폜����CSharpToolBarButton�̃C���f�b�N�X</param>
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException("index");

				CSharpToolBarButton button = this[index];
				button.toolBar = null;
				button.imageList = null;

				innerList.RemoveAt(index);
				toolBar.UpdateButtons();
			}

			/// <summary>
			/// �c�[���o�[�Ɋi�[����Ă���{�^�������ׂč폜
			/// </summary>
			public void Clear()
			{
				foreach (CSharpToolBarButton button in innerList)
				{
					button.toolBar = null;
					button.imageList = null;
				}
				innerList.Clear();
				toolBar.UpdateButtons();
			}

			/// <summary>
			/// button �� ���Ԃ� newIndex �ɕύX���܂��B
			/// </summary>
			/// <param name="button"></param>
			/// <param name="newIndex"></param>
			public void ChangeIndex(CSharpToolBarButton button, int newIndex)
			{
				if (button.toolBar == null)
					throw new ArgumentException("button �ɐe�����݂��܂���");

				if (newIndex < 0 || newIndex > Count)
					throw new ArgumentOutOfRangeException();

				if (button.Index == newIndex)
					return;

				if (button.Index < newIndex)
					newIndex -= 1;

				innerList.Remove(button);
				innerList.Insert(newIndex, button);

				toolBar.UpdateButtons();
			}

			public int IndexOf(CSharpToolBarButton button)
			{
				return innerList.IndexOf(button);
			}

			/// <summary>
			/// CSharpToolBarButtonCollection�̃Z�N�V�����̗񋓎q��Ԃ�
			/// </summary>
			/// <returns>IEnumerator</returns>
			public IEnumerator GetEnumerator()
			{
				return innerList.GetEnumerator();
			}

			#region ICollection
			/// <summary>
			/// �q�̃R���N�V�����ւ̃A�N�Z�X����������Ă��邩�ǂ����𔻒f
			/// </summary>
			bool ICollection.IsSynchronized
			{
				get
				{
					return innerList.IsSynchronized;
				}
			}

			/// <summary>
			/// �q�̃R���N�V�����ւ̃A�N�Z�X�𓯊����邽�߂Ɏg�p����I�u�W�F�N�g���擾
			/// </summary>
			object ICollection.SyncRoot
			{
				get
				{
					return innerList.SyncRoot;
				}
			}

			/// <summary>
			/// ���̃C���X�^���X��array�ɃR�s�[
			/// </summary>
			/// <param name="array"></param>
			/// <param name="index"></param>
			void ICollection.CopyTo(Array array, int index)
			{
				innerList.CopyTo(array, index);
			}
			#endregion

			#region IList
			bool IList.IsReadOnly
			{
				get
				{
					return innerList.IsReadOnly;
				}
			}
			bool IList.IsFixedSize
			{
				get
				{
					return innerList.IsFixedSize;
				}
			}
			object IList.this[int index]
			{
				set
				{
					throw new NotSupportedException();
				}
				get
				{
					return this[index];
				}
			}
			int IList.Add(object obj)
			{
				return this.Add((CSharpToolBarButton)obj);
			}
			bool IList.Contains(object obj)
			{
				return innerList.Contains((CSharpToolBarButton)obj);
			}
			int IList.IndexOf(object obj)
			{
				return this.IndexOf((CSharpToolBarButton)obj);
			}
			void IList.Insert(int index, object obj)
			{
				this.Insert(index, (CSharpToolBarButton)obj);
			}
			void IList.Remove(object obj)
			{
				this.Remove((CSharpToolBarButton)obj);
			}
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}
			#endregion
		}
		#endregion

		internal Rectangle bounds;
		private CSharpToolBar toolBar;

		private CSharpToolBarButtonStyle style;
		internal ImageList imageList;
		private string text;
		private int imageIndex;
		private object tag;

		/// <summary>
		/// ���̃{�^�����i�[����Ă���c�[���o�[���擾
		/// </summary>
		public CSharpToolBar ToolBar
		{
			get
			{
				return toolBar;
			}
		}

		/// <summary>
		/// ���̃{�^����Rectangle���W���擾
		/// </summary>
		public Rectangle Bounds
		{
			get
			{
				return bounds;
			}
		}

		/// <summary>
		/// ���̃{�^���̃C���f�b�N�X�ԍ����擾
		/// </summary>
		public int Index
		{
			get
			{
				if (toolBar != null)
				{
					return toolBar.Buttons.IndexOf(this);
				}
				return -1;
			}
		}

		/// <summary>
		/// �c�[���o�[�{�^���̃X�^�C���`�����擾�܂��͐ݒ�
		/// </summary>
		public CSharpToolBarButtonStyle Style
		{
			set
			{
				if (style != value)
				{
					style = value;
					Update();
				}
			}
			get
			{
				return style;
			}
		}

		/// <summary>
		/// �{�^���ɕ\�������e�L�X�g���擾�܂��͐ݒ�
		/// </summary>
		public string Text
		{
			set
			{
				if (text == null)
					throw new ArgumentNullException("Text");

				text = value;
				Update();
			}
			get
			{
				return text;
			}
		}

		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return imageList;
			}
		}

		/// <summary>
		/// �C���[�W���X�g�̃A�C�R���ԍ����擾�܂��͐ݒ�
		/// </summary>
		[DefaultValue(-1)]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof(UITypeEditor))]
		public int ImageIndex
		{
			set
			{
				if (imageIndex != value)
				{
					imageIndex = value;
					Update();
				}
			}
			get
			{
				return imageIndex;
			}
		}

		/// <summary>
		/// �^�O���擾�܂��͐ݒ�
		/// </summary>
		public object Tag
		{
			set
			{
				tag = value;
			}
			get
			{
				return tag;
			}
		}

		/// <summary>
		/// CSharpToolBarButton�N���X�̃C���X�^���X��������
		/// </summary>
		public CSharpToolBarButton()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			imageIndex = -1;
			bounds = Rectangle.Empty;
			text = String.Empty;
			style = CSharpToolBarButtonStyle.Button;
			toolBar = null;
			tag = null;
		}

		/// <summary>
		/// CSharpToolBarButton�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="text">�\�������{�^���e�L�X�g</param>
		public CSharpToolBarButton(string text)
			: this()
		{
			if (text == null)
				throw new ArgumentNullException("text");

			this.text = text;
		}

		/// <summary>
		/// CSharpToolBarButton�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="text">�\�������{�^���e�L�X�g</param>
		/// <param name="imageIndex">�C���[�W���X�g�̃A�C�R���ԍ�</param>
		public CSharpToolBarButton(string text, int imageIndex)
			: this(text)
		{
			this.imageIndex = imageIndex;
		}

		/// <summary>
		/// CSharpToolBarButton�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="button">�R�s�[���̃{�^��</param>
		private CSharpToolBarButton(CSharpToolBarButton button)
			: this()
		{
			if (button == null)
				throw new ArgumentNullException("button");

			this.text = button.text;
			this.imageIndex = button.imageIndex;
			this.style = button.style;
			this.tag = button.tag;
		}

		/// <summary>
		/// ���̃C���X�^���X�̃R�s�[���쐬
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			CSharpToolBarButton clone = new CSharpToolBarButton(this);
			return clone;
		}

		/// <summary>
		/// �e�̃c�[���o�[�ɍX�V���ꂽ���Ƃ�ʒm���܂��B
		/// </summary>
		protected void Update()
		{
			if (toolBar != null)
				toolBar.UpdateButtons();
		}
	}
}
