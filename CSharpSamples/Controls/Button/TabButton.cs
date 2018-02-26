// TabButton.cs

using System;
using System.Drawing;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CSharpSamples
{
	/// <summary>
	/// TabButtonControl �̃^�u�{�^����\���N���X�ł��B
	/// </summary>
	[DesignTimeVisible(false)]
	public class TabButton : Component, ICloneable
	{
		#region inner class
		/// <summary>
		/// TabButton ���i�[����R���N�V�����ł��B
		/// </summary>
		public class TabButtonCollection : ICollection, IList, IEnumerable
		{
			private TabButtonControl parent;
			private ArrayList innerList;

			/// <summary>
			/// �R���N�V�����Ɋi�[����Ă���^�u�����擾���܂��B
			/// </summary>
			public int Count {
				get {
					return innerList.Count;
				}
			}

			/// <summary>
			/// �w�肵���C���f�b�N�X�̃^�u���擾�܂��͐ݒ肵�܂��B
			/// </summary>
			public TabButton this[int index]
			{
				get {
					return (TabButton)innerList[index];
				}
			}

			/// <summary>
			/// TabButtonCollection�N���X�̃C���X�^���X���������B
			/// </summary>
			internal TabButtonCollection(TabButtonControl parent)
			{
				this.parent = parent;
				this.innerList = new ArrayList();
			}

			/// <summary>
			/// �R���N�V�����̖����� button ��ǉ����܂��B
			/// </summary>
			/// <param name="button"></param>
			/// <returns></returns>
			public int Add(TabButton button)
			{
				if (button == null)
					throw new ArgumentNullException("button");
				
				if (button.parent != null)
					throw new ArgumentException("���̃{�^���͊��ɑ��̃^�u�R���g���[���ɓo�^����Ă��܂��B");

				int index = innerList.Add(button);

				button.parent = parent;
				button.imageList = parent.ImageList;
				parent.UpdateButtons();

				return index;
			}

			/// <summary>
			/// �R���N�V�����̖����� array ��ǉ����܂��B
			/// </summary>
			/// <param name="array"></param>
			public void AddRange(TabButton[] array)
			{
				foreach (TabButton button in array)
					Add(button);
			}

			/// <summary>
			/// �R���N�V�������� index �Ԗڂ� button ��}�����܂��B
			/// </summary>
			/// <param name="index">0 ����n�܂�R���N�V�������C���f�b�N�X�B</param>
			/// <param name="button">index �Ԗڂɑ}�������{�^���B</param>
			public void Insert(int index, TabButton button)
			{
				if (index < 0 || index > Count)
					throw new ArgumentOutOfRangeException("index");

				if (button.parent != null)
					throw new ArgumentException("���̃{�^���͊��ɑ��̃^�u�R���g���[���ɓo�^����Ă��܂��B");

				innerList.Insert(index, button);
				
				button.parent = parent;
				button.imageList = parent.ImageList;

				parent.UpdateButtons();
			}

			/// <summary>
			/// button ���R���N�V��������폜���܂��B
			/// </summary>
			/// <param name="button">�R���N�V��������폜����{�^���B</param>
			public void Remove(TabButton button)
			{
				if (innerList.Contains(button))
				{
					button.parent = null;
					button.imageList = null;
				
					innerList.Remove(button);
					parent.UpdateButtons();
				}
			}

			/// <summary>
			/// �R���N�V�������� index �Ԗڂ̃{�^�����폜���܂��B
			/// </summary>
			/// <param name="index">�폜����{�^���̃C���f�b�N�X�B</param>
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException("index");

				TabButton button = this[index];
				button.parent = null;
				button.imageList = null;

				innerList.RemoveAt(index);
				parent.UpdateButtons();
			}

			public bool Contains(TabButton button)
			{
				return innerList.Contains(button);
			}

			public int IndexOf(TabButton button)
			{
				return innerList.IndexOf(button);
			}

			/// <summary>
			/// �o�^����Ă���{�^�������ׂč폜���܂��B
			/// </summary>
			public void Clear()
			{
				foreach (TabButton button in innerList)
				{
					button.parent = null;
					button.imageList = null;
				}
				innerList.Clear();
				parent.UpdateButtons();
			}

			/// <summary>
			/// button �̈ʒu�� target �̑O�Ɉړ����܂��B
			/// </summary>
			/// <param name="target"></param>
			/// <param name="button"></param>
			public void InsertBefore(TabButton target, TabButton button)
			{
				if (target == button)
					return;

				if (target.parent == null)
					throw new ArgumentException("target �ɐe�����݂��܂���B");

				if (button.parent == null)
					throw new ArgumentException("button �ɐe�����݂��܂���");

				if (target.parent != button.parent)
					throw new ArgumentException("target �� button �̐e���Ⴂ�܂��B");

				int newIndex;

				if (target.Index < button.Index)
				{
					newIndex = target.Index;
				}
				else {
					newIndex = target.Index-1;
				}

				innerList.Remove(button);
				innerList.Insert(newIndex, button);

				parent.UpdateButtons();
			}

			/// <summary>
			/// TabButtonCollection �̃Z�N�V�����̗񋓎q��Ԃ��܂��B
			/// </summary>
			/// <returns>IEnumerator</returns>
			public IEnumerator GetEnumerator()
			{
				return innerList.GetEnumerator();
			}

			#region ICollection
			/// <summary>
			/// �q�̃R���N�V�����ւ̃A�N�Z�X����������Ă��邩�ǂ����𔻒f���܂��B
			/// </summary>
			bool ICollection.IsSynchronized {
				get {
					return innerList.IsSynchronized;
				}
			}

			/// <summary>
			/// �q�̃R���N�V�����ւ̃A�N�Z�X�𓯊����邽�߂Ɏg�p����I�u�W�F�N�g���擾���܂��B
			/// </summary>
			object ICollection.SyncRoot {
				get {
					return innerList.SyncRoot;
				}
			}

			/// <summary>
			/// ���̃C���X�^���X�� array �ɃR�s�[���܂��B
			/// </summary>
			/// <param name="array"></param>
			/// <param name="index"></param>
			void ICollection.CopyTo(Array array, int index)
			{
				innerList.CopyTo(array, index);
			}
			#endregion

			#region IList
			bool IList.IsReadOnly {
				get {
					return innerList.IsReadOnly;
				}
			}
			bool IList.IsFixedSize {
				get {
					return innerList.IsFixedSize;
				}
			}
			object IList.this[int index] {
				set {
					throw new NotSupportedException();
				}
				get {
					return this[index];
				}
			}
			int IList.Add(object obj)
			{
				return Add((TabButton)obj);
			}
			bool IList.Contains(object obj)
			{
				return innerList.Contains(obj);
			}
			int IList.IndexOf(object obj)
			{
				return innerList.IndexOf(obj);
			}
			void IList.Insert(int index, object obj)
			{
				Insert(index, (TabButton)obj);
			}
			void IList.Remove(object obj)
			{
				Remove((TabButton)obj);
			}
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}
			#endregion
		}
		#endregion

		internal ImageList imageList;
		private TabButtonControl parent = null;
		private string text = String.Empty;
		private int imageIndex = -1;
		private object tag;

		private Color activeForeColor = SystemColors.ControlText;
		private Color activeBackColor = SystemColors.ControlLightLight;
		private FontStyle activeFontStyle = FontStyle.Regular;
		private FontFamily activeFontFamily = Control.DefaultFont.FontFamily;

		private Color inactiveForeColor = SystemColors.ControlText;
		private Color inactiveBackColor = SystemColors.Control;
		private FontStyle inactiveFontStyle = FontStyle.Regular;
		private FontFamily inactiveFontFamily = Control.DefaultFont.FontFamily;

		internal Rectangle bounds = Rectangle.Empty;

		/// <summary>
		/// �^�u�̕\���e�L�X�g���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue("")]
		public string Text {
			set {
				text = value;
				Update(true);
			}
			get {
				return text;
			}
		}

		/// <summary>
		/// ���̃{�^���̃C���f�b�N�X���擾���܂��B
		/// </summary>
		[Browsable(false)]
		public int Index {
			get {
				if (parent != null)
					return parent.Buttons.IndexOf(this);

				return -1;
			}
		}

		[Browsable(false)]
		public ImageList ImageList {
			get {
				return imageList;
			}
		}

		/// <summary>
		/// ImageList �̃C���f�b�N�X���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(-1)]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof(UITypeEditor))]   
		public int ImageIndex {
			set {
				if (imageIndex != value)
				{
					imageIndex = value;
					Update(false);
				}
			}
			get {
				return imageIndex;
			}
		}

		/// <summary>
		/// ���̃{�^�����I������Ă���� true�A�����łȂ���� false ��Ԃ��܂��B
		/// </summary>
		[Browsable(false)]
		public bool IsSelected {
			get {
				if (parent != null)
					return parent.Selected.Equals(this);

				return false;
			}
		}

		/// <summary>
		/// ���̃{�^���� Rectangle ���W���擾���܂��B
		/// </summary>
		[Browsable(false)]
		public Rectangle Bounds {
			get {
				return bounds;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȃ^�u�̕����F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "ControlText")]
		public Color ActiveForeColor {
			set {
				activeForeColor = value;
				Update(false);
			}
			get {
				return activeForeColor;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȃ^�u�̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "ControlLightLight")]
		public Color ActiveBackColor {
			set {
				activeBackColor = value;
				Update(false);
			}
			get {
				return activeBackColor;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȃ^�u�̕����F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "ControlText")]
		public Color InactiveForeColor {
			set {
				inactiveForeColor = value;
				Update(false);
			}
			get {
				return inactiveForeColor;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȃ^�u�̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "Control")]
		public Color InactiveBackColor {
			set {
				inactiveBackColor = value;
				Update(false);
			}
			get {
				return inactiveBackColor;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�t�@�~���[���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public FontFamily ActiveFontFamily {
			set {
				activeFontFamily = value;
				Update(true);
			}
			get {
				return activeFontFamily;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�X�^�C�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle ActiveFontStyle {
			set {
				activeFontStyle = value;
				Update(true);
			}
			get {
				return activeFontStyle;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�t�@�~���[���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public FontFamily InactiveFontFamily {
			set {
				inactiveFontFamily = value;
				Update(true);
			}
			get {
				return inactiveFontFamily;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�X�^�C�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle InactiveFontStyle {
			set {
				inactiveFontStyle = value;
				Update(true);
			}
			get {
				return inactiveFontStyle;
			}
		}

		/// <summary>
		/// �^�O���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public object Tag {
			set {
				tag = value;
			}
			get {
				return tag;
			}
		}

		public TabButton()
		{
		}

		public TabButton(string text)
		{
			Text = text;
		}

		public TabButton(string text, int imageIndex)
		{
			Text = text;
			ImageIndex = imageIndex;
		}

		public TabButton(TabButton button)
		{
			Text = button.Text;
			ImageIndex = button.ImageIndex;

			ActiveForeColor = button.ActiveForeColor;
			ActiveBackColor = button.ActiveBackColor;
			ActiveFontFamily = button.ActiveFontFamily;
			ActiveFontStyle = button.ActiveFontStyle;

			InactiveForeColor = button.InactiveForeColor;
			InactiveBackColor = button.InactiveBackColor;
			InactiveFontFamily = button.InactiveFontFamily;
			InactiveFontStyle = button.InactiveFontStyle;
		}

		/// <summary>
		/// �{�^���̏�Ԃ��X�V���ꂽ���Ƃ�e�R���g���[���ɒʒm���܂��B
		/// </summary>
		/// <param name="all"></param>
		private void Update(bool all)
		{
			if (parent == null)
				return;

			if (all) parent.UpdateButtons();
			else     parent.UpdateButton(this);
		}

		/// <summary>
		/// �C���X�^���X�̊ȈՃR�s�[���쐬���܂��B
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return new TabButton(this);
		}
	}
}
