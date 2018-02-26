// ThreadSearchEvent.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ThreadSearchEventHandler�f���Q�[�g
	/// </summary>
	public delegate void ThreadSearchEventHandler(object sender, ThreadSearchEventArgs e);

	/// <summary>
	/// ThreadSearchEventHandler ���\�b�h�̃f�[�^���
	/// </summary>
	public class ThreadSearchEventArgs : EventArgs
	{
		private List<ThreadHeader> items;
		private int start;
		private int end;

		/// <summary>
		/// ��v�����X���b�h�̏����擾
		/// </summary>
		public List<ThreadHeader> Items
		{
			get
			{
				return items;
			}
		}

		/// <summary>
		/// �����J�n�ʒu���擾
		/// </summary>
		public int Start
		{
			get
			{
				return start;
			}
		}

		/// <summary>
		/// �����I������擾
		/// </summary>
		public int End
		{
			get
			{
				return end;
			}
		}

		/// <summary>
		/// ThreadSearchEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="items">��v�����X���b�h�̏��</param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public ThreadSearchEventArgs(List<ThreadHeader> items, int start, int end)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.items = items;
			this.start = start;
			this.end = end;
		}
	}
}
