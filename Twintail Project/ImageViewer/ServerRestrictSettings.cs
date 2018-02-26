using System;
using System.Collections.Generic;
using System.Text;
using CSharpSamples;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Twintail3;

namespace ImageViewerDll
{
	[Serializable]
	public class ServerRestrictSettings : ApplicationSettingsSerializer
	{
		private List<ServerRestrictInfo> restrictInfoList = new List<ServerRestrictInfo>();
		[XmlIgnore]
		internal List<ServerRestrictInfo> RestrictList
		{
			get { return restrictInfoList; }
		}

		[ExpandableSerialize]
		public ServerRestrictInfo[] ServerRestrictInfos
		{
			get { return restrictInfoList.ToArray(); }
			set { restrictInfoList.Clear(); restrictInfoList.AddRange(value); }
		}

		public ServerRestrictSettings(string fileName)
			: base(fileName, false)
		{
		}

		public ServerRestrictInfo FromUrl(string url)
		{
			var uri = new Uri(url);
			foreach (ServerRestrictInfo info in RestrictList)
			{
				if (info.ServerAddress == uri.Host)
					return info;
			}
			return ServerRestrictInfo.Empty;
		}
	}

	[Serializable]
	public class ServerRestrictInfo
	{
		public string ServerAddress { get; set; }
		public string Referer { get; set; }
		public int ConnectionLimit { get; set; }
		public int Interval { get; set; }

		public static readonly ServerRestrictInfo Empty;

		static ServerRestrictInfo()
		{
			Empty = new ServerRestrictInfo() 
			{ ServerAddress = "*", ConnectionLimit = 5, Interval = 300, Referer = "index.html" };
		}
	}
}
