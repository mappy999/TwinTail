// IThreadListFormatter.cs

namespace Twin.Text
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// �X���b�h�ꗗ�̏��������s����{���ۃN���X
	/// </summary>
	public abstract class ThreadListFormatter
	{
		/// <summary>
		/// �w�肵���w�b�_�[�����������ĕ�����ɕϊ�
		/// </summary>
		public abstract string Format(ThreadHeader header);

		/// <summary>
		/// �w�肵���w�b�_�[�R���N�V���������������ĕ�����ɕϊ�
		/// </summary>
		public abstract string Format(List<ThreadHeader> items);
	}
}
