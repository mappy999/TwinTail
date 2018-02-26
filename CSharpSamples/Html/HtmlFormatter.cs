// HtmlFormatter.cs

namespace CSharpSamples.Html
{
	using System;
	using System.Text;

	/// <summary>
	/// HtmlFormatter �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlFormatter
	{
		private string newline;
		private char indentChar;
		private int indentCount;

		private int indent;	// ���݂̃C���f���g����\��

		/// <summary>
		/// ���s�R�[�h���擾�܂��͐ݒ�
		/// </summary>
		public string NewLine {
			set {
				newline = value;
			}
			get {
				return newline;
			}
		}

		/// <summary>
		/// �C���f���g�Ɏg�p���镶�����擾�܂��͐ݒ�
		/// </summary>
		public char IndentChar {
			set {
				indentChar = value;
			}
			get {
				return indentChar;
			}
		}

		/// <summary>
		/// �C���f���g�����擾�܂��͐ݒ�
		/// </summary>
		public int IndentCount {
			set {
				if (value < 1)
					throw new ArgumentOutOfRangeException("IndentCount");

				indentCount = value;
			}
			get {
				return indentCount;
			}
		}

		/// <summary>
		/// HtmlFormatter�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="elem"></param>
		public HtmlFormatter()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.newline = Environment.NewLine;
			this.indentChar = ' ';
			this.indentCount = 2;
			this.indent = 0;
		}

		/// <summary>
		/// �w�肵���m�[�h�R���N�V�����̃t�H�[�}�b�g�ς�Html��Ԃ�
		/// </summary>
		/// <param name="nodeCollection"></param>
		/// <returns></returns>
		public virtual string Format(HtmlNodeCollection nodeCollection)
		{
			if (nodeCollection == null)
				throw new ArgumentNullException("nodeCollection");

			StringBuilder sb = new StringBuilder();

			foreach (HtmlNode node in nodeCollection)
			{
				if (node is HtmlText)
				{
					sb.Append(((HtmlText)node).Content);
				}
				else {
					string html = Format((HtmlElement)node);
					sb.Append(html);
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// �w�肵���m�[�h�̃t�H�[�}�b�g�ς�Html��Ԃ�
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public virtual string Format(HtmlElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");

			StringBuilder sb = new StringBuilder();
			bool format = false;

			// �J�n�^�O
			sb.Append("<").Append(element.Name);

			// ������t��
			foreach (HtmlAttribute attr in element.Attributes)
			{
				sb.Append(" ").Append(attr.Html);
			}

			if (element.Nodes.Count > 0)
			{
				sb.Append(">");

				indent++;

				// �q�m�[�h��Html�𐶐�
				foreach (HtmlNode child in element.Nodes)
				{
					if (child is HtmlText)
					{
						HtmlText text = (HtmlText)child;
						sb.Append(text.Content);

						format = false;
					}
					else {
						HtmlElement childElem = (HtmlElement)child;

						if (childElem.IsTerminated)
							format = true;

						if (format)
						{
							sb.Append(newline);
							InsertSpace(sb, indent);
						}

						string html = Format(childElem);
						sb.Append(html);
						
						format = true;
					}
				}

				--indent;

				if (format)
				{
					sb.Append(newline);
					InsertSpace(sb, indent);
				}

				sb.Append("</").Append(element.Name).Append(">");
			}
			else {
				if (element.IsEmptyElementTag)
				{
					sb.Append("/>");
				}
				else if (element.IsTerminated)
				{
					sb.Append("></").Append(element.Name).Append(">");
				}
				else {
					sb.Append(">");
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// �w�肵���������C���f���g
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="count"></param>
		private StringBuilder InsertSpace(StringBuilder sb, int indent)
		{
			return sb.Append(new String(indentChar, indent * indentCount));
		}
	}
}
