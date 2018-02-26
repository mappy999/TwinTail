// HtmlAttributeCollection.cs

namespace CSharpSamples.Html
{
	using System;
	using System.Collections;

	/// <summary>
	/// HtmlAttribute�N���X���R���N�V�����Ǘ�
	/// </summary>
	public class HtmlAttributeCollection
	{
		/// <summary>
		/// �������Ǘ�����R���N�V����
		/// </summary>
		private ArrayList attributes;

		// ���̑����R���N�V�����̃C���X�^���X��ێ����Ă���e�v�f
		private HtmlElement parent;

		/// <summary>
		/// �R���N�V�����ɓo�^����Ă��鑮���̐���Ԃ�
		/// </summary>
		public int Count {
			get {
				return attributes.Count;
			}
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ɂ��鑮�����擾�܂��͐ݒ�
		/// </summary>
		public HtmlAttribute this[int index] {
			set {
				attributes[index] = value;
			}
			get {
				return (HtmlAttribute)attributes[index];
			}
		}

		/// <summary>
		/// �w�肵�����O���������̒l���擾�܂��͐ݒ�
		/// </summary>
		public string this[string name] {
			set {
				HtmlAttribute attr = FindByName(name);

				if (attr == null)
				{
					Add(new HtmlAttribute(name, value));
				}
				else {
					attr.Value = value;
				}
			}
			get {
				HtmlAttribute attr = FindByName(name);
				return (attr != null) ? attr.Value : null;
			}
		}

		/// <summary>
		/// HtmlAttributeCollection�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="parent"></param>
		public HtmlAttributeCollection(HtmlElement parent)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.attributes = new ArrayList();
			this.parent = parent;
		}

		/// <summary>
		/// �R���N�V�����̖����ɑ�����ǉ�
		/// </summary>
		/// <param name="attr"></param>
		/// <returns></returns>
		public int Add(HtmlAttribute attr)
		{
			if (attributes.Contains(attr))
				throw new HtmlException();	// ����C���X�^���X�𕡐��o�^���邱�Ƃ͏o���Ȃ�

			return attributes.Add(attr);
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X��attr��}��
		/// </summary>
		/// <param name="index"></param>
		/// <param name="attr"></param>
		public void Insert(int index, HtmlAttribute attr)
		{
			if (attributes.Contains(attr))
				throw new HtmlException();	// ����C���X�^���X�𕡐��o�^���邱�Ƃ͏o���Ȃ�

			attributes.Insert(index, attr);
		}

		/// <summary>
		/// attr���R���N�V��������폜
		/// </summary>
		/// <param name="attr"></param>
		public void Remove(HtmlAttribute attr)
		{
			attributes.Remove(attr);
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ɂ��鑮�����R���N�V��������폜
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			attributes.RemoveAt(index);
		}

		/// <summary>
		/// ���ׂĂ̑������R���N�V��������폜
		/// </summary>
		public void RemoveAll()
		{
			attributes.Clear();
		}

		/// <summary>
		/// �w�肵�����O����������Ԃ�
		/// </summary>
		/// <param name="name">�������鑮�����Bnull���w�肷���ArgumentNullException�B</param>
		/// <returns>������΂��̑����̃C���X�^���X�A������Ȃ����null��Ԃ�</returns>
		public HtmlAttribute FindByName(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			foreach (HtmlAttribute attr in attributes)
			{
				if (attr.Name.ToLower().Equals(name))
					return attr;
			}

			return null;
		}

		/// <summary>
		/// HtmlAttributeCollection�𔽕���������񋓎q��Ԃ�
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			return attributes.GetEnumerator();
		}
	}
}
