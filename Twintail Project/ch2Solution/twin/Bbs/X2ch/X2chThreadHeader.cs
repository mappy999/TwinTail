// X2chThreadHeader.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using System.Web;

	/// <summary>
	/// �Q�����˂�̃X���b�h�w�b�_����\��
	/// </summary>
	public class X2chThreadHeader : ThreadHeader
	{
		/// <summary>
		/// dat�t�@�C���̑��݂���URL���擾
		/// </summary>
		public override string DatUrl {
			get {
				return String.Format("http://{0}/{1}/dat/{2}.dat",
					BoardInfo.Server, BoardInfo.Path, Key);
			}
		}

		/// <summary>
		/// �X���b�h��URL���擾
		/// </summary>
		public override string Url {
			get {
				return String.Format("http://{0}/test/read.cgi/{1}/{2}/",
					BoardInfo.Server, BoardInfo.Path, Key);
			}
		}

		/// <summary>
		/// �F�؎g�p���̃X���b�h��URL���擾
		/// </summary>
		public string AuthenticateUrl
		{
			get
			{
				// ** ���ʂ���ɂ��� **
				// 2013/09/10 ��dat�擾�d�l�ύX
				// http://qb5.2ch.net/test/read.cgi/operate/1366640919/87-88 
				// http://stream.bbspink.com/update.txt 
				//�@�@http://rokka.<SITENAME>.<COM or NET>/<SERVER NAME>/<BOARD NAME>/<DAT NUMBER>/<OPTIONS>?raw=0.0&sid=<SID>
				//return AddSessionId( String.Format( "http://{0}/test/offlaw.cgi/{1}/{2}/" ,
				//�@�@BoardInfo.Server , BoardInfo.Path , Key ) );
				var site = BoardInfo.DomainPath.Substring(0, BoardInfo.DomainPath.IndexOf("/"));
				return AddSessionId(string.Format("http://rokka.{3}/{0}/{1}/{2}/",
				  BoardInfo.ServerName, BoardInfo.Path, Key, site));
			}
		}

		private string AddSessionId(string url)
		{
			StringBuilder urlOutput = new StringBuilder();
			urlOutput.Append(url);

			X2chAuthenticator authenticator = X2chAuthenticator.GetInstance();
			if (authenticator.HasSession)
			{
				urlOutput.AppendFormat("?raw=0.0&sid={0}", HttpUtility.UrlEncode(authenticator.SessionId, Encoding.GetEncoding("shift_jis")));
			}
			return urlOutput.ToString();
		}

		/// <summary>
		/// �������݉\�ȍő僌�X�����擾
		/// </summary>
		public override int UpperLimitResCount {
			get {
				return 1000;
			}
		}

		/// <summary>
		/// X2chThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chThreadHeader()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		public X2chThreadHeader(X2chThreadHeader source)
			: base(source.BoardInfo, source.Key, source.Subject)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
