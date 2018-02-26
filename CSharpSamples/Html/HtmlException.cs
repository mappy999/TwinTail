// HtmlException.cs

namespace CSharpSamples.Html
{
	using System;

	/// <summary>
	/// HtmlException �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlException : ApplicationException
	{
		/// <summary>
		/// HtmlException�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlException() : base()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// HtmlException�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="message"></param>
		public HtmlException(string message) : base(message)
		{
		}

		/// <summary>
		/// HtmlException�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public HtmlException(string message, Exception exception) 
			: base(message, exception)
		{
		}
	}
}
