// TwinUserProfile.cs

namespace Twin
{
	using System;
	using CSharpSamples;

	/// <summary>
	/// TwinUserProfile �̊T�v�̐����ł��B
	/// </summary>
	public class TwinUserProfile
	{
		/// <summary>
		/// ����N�����̓��t
		/// </summary>
		public DateTime Date = DateTime.Now;

		/// <summary>
		/// ����N��������̋N������
		/// </summary>
		public int Tick = 0;

		/// <summary>
		/// �N����
		/// </summary>
		public int Startup = 0;

		/// <summary>
		/// �X���b�h�𗧂Ă���
		/// </summary>
		public int NewThread = 0;

		/// <summary>
		/// �������񂾃��X�̉�
		/// </summary>
		public int PostRes = 0;

		/// <summary>
		/// �擾�����X���b�h��
		/// </summary>
		public int GotThread = 0;

		/// <summary>
		/// �擾�������X�̑���
		/// </summary>
		public int GotResCount = 0;

		/// <summary>
		/// TwinUserProfile�N���X�̃C���X�^���X��������
		/// </summary>
		public TwinUserProfile()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		public void Load(string filePath)
		{
			throw new NotSupportedException("������");
		}

		public void Save(string filePath)
		{
			throw new NotSupportedException("������");
		}
	}
}
