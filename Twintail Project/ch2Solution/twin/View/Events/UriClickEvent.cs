// UriClickEvent.cs

namespace Twin
{
	using System;

	/// <summary>
	/// ThreadViewer.UriClick�C�x���g���������郁�\�b�h��\��
	/// </summary>
	public delegate void UriClickEventHandler(object sender, UriClickEventArgs e);

	/// <summary>
	/// ThreadViewer.UriClick�C�x���g�̃f�[�^���
	/// </summary>
	public class UriClickEventArgs : EventArgs
	{
		private readonly string uri;
		private readonly LinkInfo info;

		/// <summary>
		/// �N���b�N���ꂽURI���擾
		/// </summary>
		public string Uri {
			get { return uri; }
		}

		/// <summary>
		/// ���̃����N�Ɋ֘A�Â����Ă�������擾
		/// </summary>
		public LinkInfo Information {
			get { return info; }
		}

		/// <summary>
		/// UriClickEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		public UriClickEventArgs(string uri)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.uri = uri;
			this.info = null;
		}

		/// <summary>
		/// UriClickEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		public UriClickEventArgs(string uri, LinkInfo info)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.uri = uri;
			this.info = info;
		}
	}
}
