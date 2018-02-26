// ThreadListEvent.cs

namespace Twin
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;

	/// <summary>
	/// ThreadListEventArgs �̊T�v�̐����ł��B
	/// </summary>
	public class ThreadListEventArgs : EventArgs
	{
		private ReadOnlyCollection<ThreadHeader> collection;

		/// <summary>
		/// ThreadHeader�̃R���N�V�������擾
		/// </summary>
		public ReadOnlyCollection<ThreadHeader> Items
		{
			get {
				return collection;
			}
		}

		public ThreadListEventArgs(ReadOnlyCollection<ThreadHeader> collection)
		{
			this.collection = collection;
		}

		/// <summary>
		/// ThreadListEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadListEventArgs(List<ThreadHeader> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			collection = new ReadOnlyCollection<ThreadHeader>(list);
		}

		/// <summary>
		/// ThreadListEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadListEventArgs(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			List<ThreadHeader> list = new List<ThreadHeader>();
			list.Add(header);

			collection = new ReadOnlyCollection<ThreadHeader>(list);
		}
	}
}
