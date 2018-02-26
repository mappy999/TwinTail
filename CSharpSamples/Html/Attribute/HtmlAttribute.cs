// HtmlAttribute.cs

namespace CSharpSamples.Html
{
	using System;

	/// <summary>
	/// ������\���N���X
	/// </summary>
	public class HtmlAttribute
	{
		private string name;
		private string _value;

		/// <summary>
		/// �����̖��O���擾�܂��͐ݒ�
		/// </summary>
		public string Name {
			set {
				name = value;
			}
			get {
				return name;
			}
		}

		/// <summary>
		/// �����̒l���擾�܂��͐ݒ�
		/// </summary>
		public string Value {
			set {
				_value = value;
			}
			get {
				return _value;
			}
		}

		/// <summary>
		/// ������Html�`���Ŏ擾
		/// </summary>
		public string Html {
			get {
				return String.Format("{0}=\"{1}\"", name, _value);
			}
		}

		/// <summary>
		/// HtmlAttribute�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="name">������</param>
		/// <param name="val">�����l</param>
		public HtmlAttribute(string name, string val)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.name = name;
			this._value = val;
		}

		/// <summary>
		/// HtmlAttribute�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlAttribute() : this(String.Empty, String.Empty)
		{
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
