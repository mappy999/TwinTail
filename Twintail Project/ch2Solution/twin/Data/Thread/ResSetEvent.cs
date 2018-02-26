// ResSetEvent.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ResSetEventHandler�f���Q�[�g
	/// </summary>
	public delegate void ResSetEventHandler(object sender, ResSetEventArgs e);

	/// <summary>
	/// ResSetEventHandler���\�b�h�̃f�[�^���
	/// </summary>
	public class ResSetEventArgs : EventArgs
	{
		private readonly ResSetCollection resSets;

		/// <summary>
		/// ResSet�R���N�V�������擾
		/// </summary>
		public ResSetCollection Items {
			get { return resSets; }
		}

		/// <summary>
		/// ResSetEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="items"></param>
		public ResSetEventArgs(ResSetCollection items)
		{
			if (items == null) {
				throw new ArgumentNullException("items");
			}

			this.resSets = items;
		}

		public ResSetEventArgs(ResSet res)
		{
			this.resSets = new ResSetCollection();
			this.resSets.Add(res);
		}
	}
}
