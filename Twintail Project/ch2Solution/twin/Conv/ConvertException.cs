// ConvertException.cs

namespace Twin.Conv
{
	using System;

	/// <summary>
	/// ���O�̕ϊ��Ɏ��s�����Ƃ��ɔ��������O
	/// </summary>
	public class ConvertException : ApplicationException
	{
		/// <summary>
		/// ConvertException�N���X�̃C���X�^���X��������
		/// </summary>
		public ConvertException() : base()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// ConvertException�N���X�̃C���X�^���X��������
		/// </summary>
		public ConvertException(string message) : base(message)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// ConvertException�N���X�̃C���X�^���X��������
		/// </summary>
		public ConvertException(string message, Exception innerException) 
			: base(message, innerException)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		} 
	}
}
