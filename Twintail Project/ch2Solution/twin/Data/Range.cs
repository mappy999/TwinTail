// Range.cs

namespace Twin
{
	using System;

	/// <summary>
	/// Range �̊T�v�̐����ł��B
	/// </summary>
	public struct Range
	{
		public int Start;
		public int End;

		/// <summary>
		/// Range�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public Range(int start, int end)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			Start = start;
			End = end;
		}
	}
}
