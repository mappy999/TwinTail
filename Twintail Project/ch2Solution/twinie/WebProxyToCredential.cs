
namespace Twin.Forms
{
	using System;
	using System.Net;
	using System.Runtime.Serialization;
	using CSharpSamples;
	using System.ComponentModel;

	/// <summary>
	/// プロキシ情報と認証情報を表す
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class WebProxyToCredential : ISerializable
	{
		/// <summary>
		/// プロキシのURI
		/// </summary>
		public Uri Uri;

		/// <summary>
		/// 認証するかどうかを表す
		/// </summary>
		public bool Credential;

		/// <summary>
		/// 認証用のユーザー名
		/// </summary>
		public string UserName;

		/// <summary>
		/// 認証用のパスワード
		/// </summary>
		public string Password;

		/// <summary>
		/// WebProxyToCredentialクラスのインスタンスを初期化
		/// </summary>
		public WebProxyToCredential()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			Uri = null;
			Credential = false;
			UserName = String.Empty;
			Password = String.Empty;
		}

		/// <summary>
		/// デシリアライズ化
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public WebProxyToCredential(SerializationInfo info, StreamingContext context)
		{
			Serializer.Deserialize(this, info);
		}

		/// <summary>
		/// シリアライズ化
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Serializer.Serialize(this, info);
		}

		/// <summary>
		/// このインスタンスのIWebProxyを取得
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
