// NumberClickEvent.cs

namespace Twin
{
	using System;
	using Twin;

	/// <summary>
	/// ThreadViewer.NumberClick�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void NumberClickEventHandler(object sender, 
					NumberClickEventArgs e);

	/// <summary>
	/// ThreadViewer.NumberClick�C�x���g�̃f�[�^���
	/// </summary>
	public class NumberClickEventArgs : EventArgs
	{
		private readonly ThreadHeader header;
		private readonly ResSet resSet;

		/// <summary>
		/// �X���b�h�̃w�b�_�����擾
		/// </summary>
		public ThreadHeader Header {
			get { return header; }
		}

		/// <summary>
		/// �I�����ꂽ���X���擾
		/// </summary>
		public ResSet ResSet {
			get { return resSet; }
		}

		/// <summary>
		/// NumberClickEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header"></param>
		/// <param name="res"></param>
		public NumberClickEventArgs(ThreadHeader header, ResSet res)
		{
			this.header = header;
			this.resSet = res;
		}
	}
}
