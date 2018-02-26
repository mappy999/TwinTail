// HtmlElement.cs

namespace CSharpSamples.Html
{
	using System;
	using System.Text;
	using System.Collections;

	/// <summary>
	/// HtmlElement �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlElement : HtmlNode
	{
		private HtmlNodeCollection nodes;
		private HtmlAttributeCollection attributes;

		private bool isTerminated;
		private bool isEmptyElementTag;
		private string name;

		/// <summary>
		/// �q�m�[�h�R���N�V�������擾
		/// </summary>
		public HtmlNodeCollection Nodes {
			get {
				return nodes;
			}
		}

		/// <summary>
		/// �����R���N�V�������擾
		/// </summary>
		public HtmlAttributeCollection Attributes {
			get {
				return attributes;
			}
		}

		/// <summary>
		/// �P�̃^�O�Ŋ�������^�O���ǂ������擾�܂��͐ݒ�
		/// </summary>
		public bool IsEmptyElementTag {
			set {
				isEmptyElementTag = value;
			}
			get {
				return isEmptyElementTag && nodes.Count == 0;
			}
		}

		/// <summary>
		/// ���ތ`���̃^�O���ǂ������擾�܂��͐ݒ�
		/// </summary>
		public bool IsTerminated {
			set {
				isTerminated = value;
			}
			get {
				return isTerminated || Nodes.Count > 0;
			}
		}

		/// <summary>
		/// �^�O�̖��O���擾�܂��͐ݒ�
		/// </summary>
		public string Name {
			set {
				if (value == null)
					throw new ArgumentNullException("Name");

				name = value;
			}
			get {
				return name;
			}
		}

		/// <summary>
		/// ���̃m�[�h���q�m�[�h���܂�Html�`���ɕϊ�
		/// </summary>
		public override string Html {
			get {
				StringBuilder sb = new StringBuilder();

				sb.Append("<").Append(Name);

				foreach (HtmlAttribute attr in attributes)
				{
					sb.Append(" ").Append(attr.Html);
				}

				if (nodes.Count > 0)
				{
					sb.Append(">");

					foreach (HtmlNode child in nodes)
						sb.Append(child.Html);

					sb.Append("</").Append(Name).Append(">");
				}
				else {
					if (IsEmptyElementTag)
					{
						sb.Append("/>");
					}
					else if (IsTerminated)
					{
						sb.Append("></").Append(Name).Append(">");
					}
					else {
						sb.Append(">");
					}
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// ���̃m�[�h�̓���Html���擾
		/// </summary>
		public override string InnerHtml {
			get {
				StringBuilder sb = new StringBuilder();

				foreach (HtmlNode child in nodes)
					sb.Append(child.InnerHtml);

				return sb.ToString();
			}
		}

		/// <summary>
		/// ���̃m�[�h�̓����e�L�X�g���擾
		/// </summary>
		public override string InnerText {
			get {
				StringBuilder sb = new StringBuilder();
				
				foreach (HtmlNode child in nodes)
					sb.Append(child.InnerText);

				return sb.ToString();
			}
		}

		/// <summary>
		/// HtmlElement�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="name"></param>
		public HtmlElement(string name)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.nodes = new HtmlNodeCollection(this);
			this.attributes = new HtmlAttributeCollection(this);
			this.isTerminated = false;
			this.isEmptyElementTag = false;
			this.name = name;
		}

		/// <summary>
		/// HtmlElement�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlElement() : this(String.Empty)
		{
		}

		/// <summary>
		/// ���̃G�������g��node�̐e���ǂ����𔻒f
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsAncestor(HtmlNode node)
		{
			while (node != null)
			{
				if (this == node.Parent)
					return true;

				node = node.Parent;
			}	
			return false;
		}

		/// <summary>
		/// ���̃m�[�h��node�̎q�����ǂ����𔻒f
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsDescendant(HtmlNode node)
		{
			HtmlElement e = node as HtmlElement;
			return e != null ? e.IsAncestor(this) : false;
		}

		/// <summary>
		/// �w�肵�����O�̃^�O�����ׂĎ擾
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		public HtmlElement[] GetElementsByName(string tagName)
		{
			return Sta_GetElementsByName(nodes, tagName);
		}

		/// <summary>
		/// �w�肵��ID�����G�������g���擾
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public HtmlElement GetElementById(string id)
		{
			return Sta_GetElementById(nodes, id);
		}

		/// <summary>
		/// �m�[�h�R���N�V�������̎w�肵���^�O���O�������ׂẴG�������g���擾
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="tagName"></param>
		/// <returns></returns>
		internal static HtmlElement[] Sta_GetElementsByName(HtmlNodeCollection nodes, string tagName)
		{
			ArrayList arrayList = new ArrayList();

			foreach (HtmlNode node in nodes)
			{
				HtmlElement elem = node as HtmlElement;

				if (elem != null)
				{
					if (String.Compare(elem.name, tagName, true) == 0)
						arrayList.Add(elem);

					HtmlElement[] array = elem.GetElementsByName(tagName);
					arrayList.AddRange(array);
				}
			}

			return (HtmlElement[])arrayList.ToArray(typeof(HtmlElement));
		}

		/// <summary>
		/// �m�[�h�R���N�V�������̎w�肵��ID�����G�������g���擾
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		internal static HtmlElement Sta_GetElementById(HtmlNodeCollection nodes, string id)
		{
			foreach (HtmlNode node in nodes)
			{
				HtmlElement element = node as HtmlElement;

				if (element != null)
				{
					string val = element.Attributes["id"];

					if (String.Compare(id, val, true) == 0)
						return element;

					HtmlElement sub = element.GetElementById(id);
					if (sub != null) return sub;
				}
			}

			return null;
		}
	}
}
