// LinkInfo.cs

namespace Twin
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// �����N�Ƃ��̐�����\��
	/// </summary>
	[Serializable]
	public class LinkInfo : ISerializable
	{
		private string uri;
		private string text;

		/// <summary>
		/// �����N��URI���擾�܂��͐ݒ�
		/// </summary>
		public string Uri {
			set {
				if (value == null) {
					throw new ArgumentNullException("Uri");
				}
				uri = value;
			}
			get { return uri; }
		}

		/// <summary>
		/// �����N�Ɋւ��Ă̐������擾�܂��͐ݒ�
		/// </summary>
		public string Text {
			set {
				if (value == null) {
					throw new ArgumentNullException("Text");
				}
				text = value;
			}
			get { return text; }
		}

		/// <summary>
		/// LinkInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="text"></param>
		public LinkInfo()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.uri = String.Empty;
			this.text = String.Empty;
		}

		/// <summary>
		/// LinkInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="text"></param>
		public LinkInfo(string uri, string text)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.uri = uri;
			this.text = text;
		}

		public LinkInfo(SerializationInfo info, StreamingContext context)
		{
			uri = info.GetString("Uri");
			text = info.GetString("Text");
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{			
			info.AddValue("Uri", uri);
			info.AddValue("Text", text);
		}
	}
}
