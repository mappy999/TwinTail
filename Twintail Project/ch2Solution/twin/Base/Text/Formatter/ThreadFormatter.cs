// IThreadFormatter.cs

namespace Twin.Text
{
	using System;

	/// <summary>
	/// �X���b�h�̏��������s����{���ۃN���X
	/// </summary>
	public abstract class ThreadFormatter
	{
		/// <summary>
		/// �w�肵�����X�����������ĕ�����ɕϊ�
		/// </summary>
		public abstract string Format(ResSet resSet);

		/// <summary>
		/// �w�肵�����X�R���N�V���������������ĕ�����ɕϊ�
		/// </summary>
		public abstract string Format(ResSetCollection resCollection);
	}
}
