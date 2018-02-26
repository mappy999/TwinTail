// HtmlParser.cs

namespace CSharpSamples.Html
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections;
	using System.Diagnostics;

	/// <summary>
	/// HtmlParser �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlParser
	{
		private readonly Regex rexTagName = new Regex("[a-zA-Z0-9]+", RegexOptions.Compiled);
		private readonly char[] QuoteChars = ("\"\'").ToCharArray();

		/// <summary>
		/// HtmlParser�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlParser()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// Html����͂��ăm�[�h�R���N�V�����𐶐�
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public HtmlNodeCollection Parse(string html)
		{
			if (html == null)
				throw new ArgumentNullException("html");

			// ���s�����Ȃǂ��폜
			html = RemoveWhiteSpace(html);

			HtmlNodeCollection root = new HtmlNodeCollection(null);
			int index = 0;

			while (index < html.Length)
			{
				// �R�����g�͖�������
				if (is_match(html, index, "<!--"))
				{
					index += 4;

					// �R�����g���I���܂�index��i�߂�
					while (index < html.Length)
					{
						if (is_match(html, index, "-->"))
						{
							index += 3;
							break;
						}
						index++;
					}
				}
				// ����^�O�̏ꍇ
				else if (is_match(html, index, "</"))
				{
					index += 2;

					// �󔒓ǂݔ�΂�
					SkipWhiteSpace(html, ref index);

					// �^�O�����擾
					Match m = rexTagName.Match(html, index);
					if (m.Success)
					{
						// �������O�̊J�n�^�O���擾
						int nodeidx = BackFindOpenElement(root, m.Value);
						if (nodeidx != -1)
						{
							NodesDown(root, nodeidx+1, (HtmlElement)root[nodeidx]);
						}
						else {
							// throw new HtmlException();
						}
						index += m.Length;
					}

					// �I���^�O������܂ŃX�L�b�v
					SkipChars(html, ref index, "<>".ToCharArray());

					if (is_match(html, index, ">"))
						index++;
				}
				// �J�n�^�O�̏ꍇ
				else if (is_match(html, index, "<"))
				{
					index += 1;

					// �󔒓ǂݔ�΂�
					SkipWhiteSpace(html, ref index);

					// �^�O�����擾
					Match m = rexTagName.Match(html, index);
					if (m.Success)
					{
						HtmlElement e = new HtmlElement(m.Value.ToLower());
						root.Add(e);

						index = m.Index + m.Length;

						// �󔒓ǂݔ�΂�
						SkipWhiteSpace(html, ref index);

						// �����̓ǂݍ���
						if (not_match(html, index, "/>") &&
							not_match(html, index, ">"))
						{
							ParseAttributes(html, ref index, e.Attributes);
						}

						// �P�Ŋ�������^�O�̏���
						if (is_match(html, index, "/>"))
						{
							e.IsEmptyElementTag = true;
							index += 2;
						}
						// �I���^�O�̏ꍇ
						else if (is_match(html, index, ">"))
						{
							index++;
						}
					}
				}
				// �e�L�X�g����
				else {

					// �J�n�^�O���������A�����܂ł��e�L�X�g�Ƃ���
					int next = html.IndexOf("<", index);
					HtmlText text;

					if (next == -1)
					{
						text = new HtmlText(html.Substring(index));
						index = html.Length;
					}
					else {
						text = new HtmlText(html.Substring(index, next - index));
						index = next;
					}

					root.Add(text);
				}
			}

			return root;
		}

		/// <summary>
		/// �w�肵�����O�Ɠ����^�O�v�f������
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private int BackFindOpenElement(HtmlNodeCollection nodes, string name)
		{
			for (int i = nodes.Count-1; i >= 0; i--)
			{
				HtmlElement e = nodes[i] as HtmlElement;

				if (e != null && !e.IsTerminated)
				{
					// �啶���������͋�ʂ��Ȃ�
					if (e.Name.ToLower() == name.ToLower())
						return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// ��������͂�attributes�ϐ��Ɋi�[
		/// </summary>
		/// <param name="html"></param>
		/// <param name="index"></param>
		/// <param name="attributes"></param>
		private void ParseAttributes(string html, ref int index,
			HtmlAttributeCollection attributes)
		{
			while (index < html.Length)
			{
				// �󔒓ǂݔ�΂�
				SkipWhiteSpace(html, ref index);

				// �^�O�̏I���Ȃ�I��
				if (is_match(html, index, "/>") || is_match(html, index, ">"))
					break;

				int attrSt = index;
				
				// ������������
				if (SkipChars(html, ref index, "=".ToCharArray()))
				{
					string attrName, attrVal = String.Empty;
					
					attrName = html.Substring(attrSt, index - attrSt).ToLower();
					index++;

					// �󔒓ǂݔ�΂�
					SkipWhiteSpace(html, ref index);

					// �N�I�[�e�[�V��������n�܂�ꍇ�́A�����N�I�[�e�[�V�������o��܂ł�l�Ƃ���
					if (index < html.Length && IsQuote(html[index]))
					{
						char quote = html[index];
						int st = index++;

						SkipChars(html, ref index, new char[] {quote, '>'});

						attrVal = html.Substring(st, index - st).Trim(QuoteChars);
						
						if (is_match(html, index, quote.ToString()))
							index++;
					}
					// �����ɒl���n�܂��Ă���ꍇ
					else {
						// �󔒂��^�O�̏I��肪����܂�index��i�߂�
						for (int i = index; i < html.Length; i++)
						{
							if (is_match(html, i, " ") ||
								is_match(html, i, ">") ||
								is_match(html, i, "/>"))
							{
								attrVal = html.Substring(index, i - index);
								index = i;
								break;
							}
						}
					}

					// �l������
					HtmlAttribute attr = new HtmlAttribute(attrName, attrVal);
					attributes.Add(attr);
				}
				else {
					break;
				}
			}
		}

		/// <summary>
		/// �w�肵���������N�I�[�e�[�V�������ǂ����𔻒f
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		private bool IsQuote(char ch)
		{
			foreach (char quote in QuoteChars)
			{
				if (quote == ch)
					return true;
			}
			return false;
		}

		/// <summary>
		/// input��index����stopChars�̂ǂꂩ������܂�index���C���N�������g
		/// </summary>
		/// <param name="input"></param>
		/// <param name="index"></param>
		/// <param name="stopChars"></param>
		/// <returns></returns>
		private bool SkipChars(string input, ref int index, char[] stopChars)
		{
			if (index < 0 || index >= input.Length)
				throw new ArgumentOutOfRangeException("index");

			int r = input.IndexOfAny(stopChars, index);
			if (r != -1) index = r;

			return (r != -1) ? true : false;
		}

		/// <summary>
		/// �󔒕����������Ȃ�܂�index���C���N�������g
		/// </summary>
		/// <param name="input"></param>
		/// <param name="index"></param>
		private void SkipWhiteSpace(string input, ref int index)
		{
			while (index < input.Length)
			{
				if (!Char.IsWhiteSpace(input[index]))
					break;

				index++;
			}
		}

		/// <summary>
		/// element�ȍ~�̃m�[�h��element�̎q�ɐݒ肷��
		/// </summary>
		/// <param name="element"></param>
		private void NodesDown(HtmlNodeCollection nodes, int index, HtmlElement parent)
		{
			ArrayList children = new ArrayList();

			while (index < nodes.Count)
			{
				children.Add(nodes[index]);
				nodes.RemoveAt(index);
			}

			foreach (HtmlNode node in children)
				parent.Nodes.Add(node);

			parent.IsTerminated = true;
		}

		/// <summary>
		/// �󔒕������폜����Html��Ԃ�
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		private string RemoveWhiteSpace(string html)
		{
			StringBuilder sb = new StringBuilder(html);
			sb.Replace("\r", "");
			sb.Replace("\n", "");
			sb.Replace("\t", " ");

			return sb.ToString();
		}

		/// <summary>
		/// input��index����key����v���邩�ǂ����𔻒f
		/// </summary>
		/// <param name="input"></param>
		/// <param name="index"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool is_match(string input, int index, string key)
		{
			if (index < 0 || index >= input.Length)
				throw new ArgumentOutOfRangeException("index");

			if ((input.Length - index) < key.Length)
				return false;

			string s = input.Substring(index, key.Length);
			return s.Equals(key);
		}

		/// <summary>
		/// input��index����key����v���Ȃ����ǂ����𔻒f
		/// </summary>
		/// <param name="input"></param>
		/// <param name="index"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool not_match(string input, int index, string key)
		{
			return !is_match(input, index, key);
		}
	}
}
