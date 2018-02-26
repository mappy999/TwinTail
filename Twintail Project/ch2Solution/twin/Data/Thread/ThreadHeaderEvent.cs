// ThreadHeaderEvent.cs

namespace Twin
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ThreadHeaderEventHandler�f���Q�[�g
	/// </summary>
	public delegate void ThreadHeaderEventHandler(object sender, ThreadHeaderEventArgs e);

	/// <summary>
	/// ThreadHeaderEvent �̊T�v�̐����ł��B
	/// </summary>
	public class ThreadHeaderEventArgs : EventArgs
	{
		private readonly List<ThreadHeader> headerCollection;

		/// <summary>
		/// ThreadHeader�̃R���N�V�������擾
		/// </summary>
		public List<ThreadHeader> Items {
			get {
				return headerCollection;
			}
		}

		/// <summary>
		/// ThreadHeaderEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadHeaderEventArgs(List<ThreadHeader> items)
		{
			if (items == null) {
				throw new ArgumentNullException("items");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			headerCollection = items;
		}

		/// <summary>
		/// ThreadHeaderEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadHeaderEventArgs(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			headerCollection = new List<ThreadHeader>();
			headerCollection.Add(header);
		}
	}
}
