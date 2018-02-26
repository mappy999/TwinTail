// BoardInfo.cs
// #2.0

namespace Twin
{
	using System;
	using System.Text;
	using System.Xml.Serialization;
	using System.Runtime.Serialization;
	using System.Text.RegularExpressions;
	using System.Net;
	using Twin.Text;

	/// <summary>
	/// �f���̔���ێ�����N���X�ł��B
	/// </summary>
	[Serializable]
	public class BoardInfo : ISerializable, IComparable
	{
		private CookieContainer cookie = new CookieContainer();
		private string server;
		private string path;
		private string name;
		private object tag;
		private BbsType bbs;

		/// <summary>
		/// �ւ� URL ���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[XmlIgnore]
		public string Url
		{
			get {
				StringBuilder sb = new StringBuilder();
				return "http://" + server + "/" + path + "/";
			}
		}

		/// <summary>
		/// �T�[�o�[�̖��O�����݂̂��擾���܂��B
		/// </summary>
		public string ServerName
		{
			get {
				int token = server.IndexOf('.');
				return server.Substring(0, token);
			}
		}

		public string DomainName
		{
			get
			{
				int dot = this.Server.IndexOf('.');
				if (this.Server.IndexOf('.', dot + 1) >= 0)
					return this.Server.Substring(dot + 1);
				else
					return this.Server;
			}
		}

		/// <summary>
		/// �h���C�����Ɣւ̃p�X���擾���܂��B
		/// </summary>
		public string DomainPath
		{
			get {
				int token = server.IndexOf('.');

				if (server.IndexOf('.', token + 1) >= 0)
				{
					return server.Substring(token + 1) + "/" + path;
				}
				else
				{
					return server + "/" + path;
				}
			}
		}

		/// <summary>
		/// �T�[�o�[�A�h���X���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string Server
		{
			set {
				if (value == null)
				{
					throw new ArgumentNullException("Server");
				}
				server = value;
				bbs = Parse(server);
			}
			get {
				return server;
			}
		}

		/// <summary>
		/// �ւ̃p�X���擾�܂��͐ݒ�
		/// </summary>
		public string Path
		{
			set {
				if (value == null)
				{
					throw new ArgumentNullException("Path");
				}
				path = value;
			}
			get {
				return path;
			}
		}

		/// <summary>
		/// �����擾�܂��͐ݒ�
		/// </summary>
		public string Name
		{
			set {
				if (value == null)
				{
					throw new ArgumentNullException("Name");
				}
				name = HtmlTextUtility.RemoveTag(value);
			}
			get {
				return name;
			}
		}

		/// <summary>
		/// �N�b�L�[���擾�܂��͐ݒ�
		/// </summary>
		[XmlIgnore]
		public CookieContainer CookieContainer
		{
			set {
				if (value == null)
					cookie = new CookieContainer();

				else
					cookie = value;
			}
			get {
				return cookie;
			}
		}

		/// <summary>
		/// �f���̎�ނ��擾�܂��͐ݒ�
		/// </summary>
		[XmlIgnore]
		public BbsType Bbs
		{
			set {
				bbs = value;
			}
			get {
				return bbs;
			}
		}

		/// <summary>
		/// Tag���擾�܂��͐ݒ�
		/// </summary>
		[XmlIgnore]
		public object Tag
		{
			set {
				tag = value;
			}
			get {
				return tag;
			}
		}

		/// <summary>
		/// BoardInfo�N���X�̃C���X�^���X��������
		/// </summary>
		public BoardInfo()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			server = String.Empty;
			path = String.Empty;
			name = String.Empty;
			bbs = BbsType.X2ch;
			tag = null;
		}

		/// <summary>
		/// BoardInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="server">�T�[�o�[�A�h���X</param>
		/// <param name="path">�ւ̃p�X</param>
		/// <param name="name">��</param>
		public BoardInfo(string server, string path, string name)
			: this()
		{
			this.server = server;
			this.path = path;
			this.name = HtmlTextUtility.RemoveTag(name);
			this.bbs = Parse(server);
		}

		/// <summary>
		/// �f�V���A���C�Y���ɌĂ΂��R���X�g���N�^
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public BoardInfo(SerializationInfo info, StreamingContext context)
		{
			this.bbs = (BbsType)info.GetValue("Bbs", typeof(BbsType));
			this.name = HtmlTextUtility.RemoveTag(info.GetString("Name"));
			this.path = info.GetString("Path");
			this.server = info.GetString("Server");
		}

		/// <summary>
		/// ���̃C���X�^���X���V���A���C�Y
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Bbs", bbs);
			info.AddValue("Name", name);
			info.AddValue("Path", path);
			info.AddValue("Server", server);
		}

		/// <summary>
		/// �w�肵��server����f���̎�ނ���͂���
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static BbsType Parse(string url)
		{
			#region
			if (Regex.IsMatch(url, @"^(dempa|i|isp|irc|info)\.2ch\.net"))
			{
				return BbsType.None;
			}
			if (url.IndexOf("be.2ch.net") != -1)
			{
				return BbsType.Be2ch;
			}
			if (url.IndexOf("2ch.net") != -1 || url.IndexOf("bbspink.com") != -1)
			{
				return BbsType.X2ch;
			}
			if (url.IndexOf("machi.to") != -1)
			{
				return BbsType.Machi;
			}
			if (url.IndexOf("jbbs.shitaraba.com") != -1 || url.IndexOf("jbbs.livedoor.com") != -1 ||
				url.IndexOf("jbbs.livedoor.jp") != -1)
			{
				return BbsType.Jbbs;
			}
			if (url.IndexOf("www.shitaraba.com") != -1)
			{
				return BbsType.Shita;
			}
			if (url.IndexOf("zetabbs.org") != -1)
			{
				return BbsType.Zeta;
			}
			if (url.IndexOf("milkcafe.net") != -1)
			{
				return BbsType.MilkCafe;
			}
			#endregion

			return BbsType.Dat;
		}

		/// <summary>
		/// �n�b�V���֐�
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return Url.GetHashCode();
		}

		/// <summary>
		/// ���݂̃C���X�^���X��entry���r
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as BoardInfo);
		}

		/// <summary>
		/// ���݂̃C���X�^���X��board���r
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public bool Equals(BoardInfo board)
		{
			if (board == null)
				return false;

			return (this.GetHashCode() == board.GetHashCode()) ? true : false;
		}

		/// <summary>
		/// ���݂̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// ���݂̃C���X�^���X��obj���r
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			BoardInfo b = obj as BoardInfo;
			if (b == null)
				return 1;

			return String.Compare(Url, b.Url);
		}
	}
}
