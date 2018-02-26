// HtmlText.cs

namespace CSharpSamples.Html
{
	using System;

	/// <summary>
	/// Html���̃e�L�X�g��\��
	/// </summary>
	public class HtmlText : HtmlNode
	{
		private string text;

		/// <summary>
		/// �e�L�X�g�̓��e���擾�܂��͐ݒ�
		/// </summary>
		public string Content {
			set {
				text = value;
			}
			get {
				return text;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B��Content�v���p�e�B�Ɠ����l��Ԃ�
		/// </summary>
		public override string Html {
			get {
				return text;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B��Content�v���p�e�B�Ɠ����l��Ԃ�
		/// </summary>
		public override string InnerHtml {
			get {
				return text;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B��Content�v���p�e�B�Ɠ����l��Ԃ�
		/// </summary>
		public override string InnerText {
			get {
				return text;
			}
		}

		/// <summary>
		/// HtmlText�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="text"></param>
		public HtmlText(string text)
		{
			this.text = text;
		}

		/// <summary>
		/// HtmlText�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlText() : this(String.Empty)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
