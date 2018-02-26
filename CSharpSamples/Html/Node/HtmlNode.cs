// HtmlNode.cs

namespace CSharpSamples.Html
{
	using System;

	/// <summary>
	/// HtmlNode �̊T�v�̐����ł��B
	/// </summary>
	public abstract class HtmlNode
	{
		private HtmlElement parent;

		/// <summary>
		/// ���̃m�[�h���ŏ�w���ǂ������擾
		/// </summary>
		public bool IsRoot {
			get {
				return (parent == null);
			}
		}

		/// <summary>
		/// ���̃m�[�h���e�m�[�h���ǂ������擾
		/// </summary>
		public bool IsParent {
			get {
				HtmlElement e = this as HtmlElement;
				return (e != null && e.Nodes.Count > 0) ? true : false;
			}
		}

		/// <summary>
		/// ���̃m�[�h���q�m�[�h���ǂ������擾
		/// </summary>
		public bool IsChild {
			get {
				return (parent != null);
			}
		}

		/// <summary>
		/// �e�m�[�h���擾
		/// </summary>
		public HtmlElement Parent {
			get {
				return parent;
			}
		}

		/// <summary>
		/// �O�̌Z��m�[�h���擾
		/// </summary>
		public HtmlNode Prev {
			get {
				if (Index != -1 && Parent != null)
				{
					if (Index > 0)
						return Parent.Nodes[Index-1];
				}
				return null;
			}
		}

		/// <summary>
		/// ���̌Z��m�[�h���擾
		/// </summary>
		public HtmlNode Next {
			get {
				if (Index != -1 && Parent != null)
				{
					if (Index+1 < Parent.Nodes.Count)
						return Parent.Nodes[Index+1];
				}
				return null;
			}
		}

		/// <summary>
		/// �ŏ��̎q�m�[�h���擾 (�q�m�[�h�����݂��Ȃ����null��Ԃ�)
		/// </summary>
		public HtmlNode FirstChild {
			get {
				HtmlElement e = this as HtmlElement;
				
				if (e == null || e.Nodes.Count == 0)
				{
					return null;
				}
				else {
					return e.Nodes[0];
				}
			}
		}

		/// <summary>
		/// �Ō�̎q�m�[�h���擾 (�q�m�[�h�����݂��Ȃ����null��Ԃ�)
		/// </summary>
		public HtmlNode LastChild {
			get {
				HtmlElement e = this as HtmlElement;

				if (e == null || e.Nodes.Count == 0)
				{
					return null;
				}
				else {
					return e.Nodes[e.Nodes.Count-1];
				}
			}
		}

		/// <summary>
		/// �m�[�h�R���N�V�������̈ʒu���擾
		/// </summary>
		public int Index {
			get {
				return (Parent != null) ?
					Parent.Nodes.IndexOf(this) : -1;
			}
		}

		/// <summary>
		/// ���̃m�[�h��Html�`���̕�����Ŏ擾
		/// </summary>
		public abstract string Html {
			get;
		}

		/// <summary>
		/// ���̃m�[�h�̓���Html���擾
		/// </summary>
		public abstract string InnerHtml {
			get;
		}

		/// <summary>
		/// ���̃m�[�h�̓����e�L�X�g���擾
		/// </summary>
		public abstract string InnerText {
			get;
		}

		/// <summary>
		/// HtmlNode�N���X�̃C���X�^���X��������
		/// </summary>
		protected HtmlNode()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.parent = null;
		}

		/// <summary>
		/// ���̃m�[�h�ɐV�����e��ݒ�
		/// </summary>
		/// <param name="newParent"></param>
		internal void SetParent(HtmlElement newParent)
		{
			parent = newParent;
		}

		/// <summary>
		/// ���̃C���X�^���X��e�m�[�h����폜
		/// </summary>
		public void Remove()
		{
			if (Parent != null)
				Parent.Nodes.Remove(this);
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns>Html�v���p�e�B�̒l��Ԃ�</returns>
		public override string ToString()
		{
			return this.Html;
		}
	}
}
