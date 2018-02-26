
namespace Twin.Forms
{
	using System;
	using System.Net;
	using System.Runtime.Serialization;
	using CSharpSamples;
	using System.ComponentModel;

	/// <summary>
	/// �v���L�V���ƔF�؏���\��
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class WebProxyToCredential : ISerializable
	{
		/// <summary>
		/// �v���L�V��URI
		/// </summary>
		public Uri Uri;

		/// <summary>
		/// �F�؂��邩�ǂ�����\��
		/// </summary>
		public bool Credential;

		/// <summary>
		/// �F�ؗp�̃��[�U�[��
		/// </summary>
		public string UserName;

		/// <summary>
		/// �F�ؗp�̃p�X���[�h
		/// </summary>
		public string Password;

		/// <summary>
		/// WebProxyToCredential�N���X�̃C���X�^���X��������
		/// </summary>
		public WebProxyToCredential()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			Uri = null;
			Credential = false;
			UserName = String.Empty;
			Password = String.Empty;
		}

		/// <summary>
		/// �f�V���A���C�Y��
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public WebProxyToCredential(SerializationInfo info, StreamingContext context)
		{
			Serializer.Deserialize(this, info);
		}

		/// <summary>
		/// �V���A���C�Y��
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Serializer.Serialize(this, info);
		}

		/// <summary>
		/// ���̃C���X�^���X��IWebProxy���擾
		/// </summary>
		/// <returns></returns>
		public IWebProxy GetWebProxy()
		{
			IWebProxy webProxy = null;

			if (Uri != null && Uri.IsAbsoluteUri)
			{
				webProxy = new WebProxy(Uri);
				webProxy.Credentials = new NetworkCredential(UserName, Password);
			}

			return webProxy;
		}
	}
}
