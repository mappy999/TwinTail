using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;

namespace Twin
{
	public class UserEnvSettings : Twintail3.ApplicationSettingsSerializer
	{
		public DateTime StartTime { get; set; }
		public int CurrentRunTime { get; set; }
		public int TotalRunTime { get; set; }
		public int OpeningThreadCount { get; set; }
		public int OpenedThreadCount { get; set; }
		public int TotalPostCount { get; set; }
		public int MyBackRefCount { get; set; }

		[XmlIgnore]
		public string UseMemorySizeText
		{
			get
			{
				long memoryBytes = GC.GetTotalMemory(true);
				return String.Format("メモリ使用量: {0:#,##0} KB", memoryBytes / 1024);
			}
		}

		public UserEnvSettings()
			: base(Path.Combine(Application.ExecutablePath, "_"))
		{
			this.StartTime = DateTime.Now;
		}

	}
}
