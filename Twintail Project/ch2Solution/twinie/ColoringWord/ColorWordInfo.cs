using System;
using System.Collections.Generic;
using System.Text;
using Twintail3;
using System.Drawing;
using System.Xml.Serialization;

namespace Twin
{
	public class ColorWordInfoSettings : ApplicationSettingsSerializer
	{
		private List<ColorWordInfo> list = new List<ColorWordInfo>();
		
		[DefaultArraySet(0)]
		[ExpandableSerialize]
		public ColorWordInfo[] WordInfo
		{
			get
			{
				return list.ToArray();
			}
			set
			{
				list.Clear();
				list.AddRange(value);
			}
		}

		[XmlIgnore]
		public List<ColorWordInfo> WordInfoList
		{
			get { return list; }
		}

		public ColorWordInfoSettings(string path)
			: base(path, true)
		{
		}

		public void Add(ColorWordInfo[] array)
		{
			list.AddRange(array);
		}
	}

	public class ColorWordInfo
	{
		private string text;
		/// <summary>
		/// 色付けを行う文字列を表します。
		/// </summary>
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}
	
		private Color foreColor;
		/// <summary>
		/// 色付けの文字色を表します。
		/// </summary>
		public Color ForeColor
		{
			get
			{
				return foreColor;
			}
			set
			{
				foreColor = value;
			}
		}

		private Color backColor;
		/// <summary>
		/// 色付けの背景色を表します。
		/// </summary>
		public Color BackColor
		{
			get
			{
				return backColor;
			}
			set
			{
				backColor = value;
			}
		}
		private bool bold;
		/// <summary>
		/// 太字表示するかどうかを表します。
		/// </summary>
		public bool IsBold
		{
			get
			{
				return bold;
			}
			set
			{
				bold = value;
			}
		}

		private bool italic;
		/// <summary>
		/// 斜体表示するかどうかを表します。
		/// </summary>
		public bool IsItalic
		{
			get
			{
				return italic;
			}
			set
			{
				italic = value;
			}
		}
	
		private bool regex;
		/// <summary>
		/// Text プロパティは正規表現であるかどうかを表します。
		/// </summary>
		public bool IsRegex
		{
			get
			{
				return regex;
			}
			set
			{
				regex = value;
			}
		}

		private bool playSound;
		/// <summary>
		/// 一致した単語を表示したときにサウンドを再生するかどうかを表します。
		/// </summary>
		public bool IsPlaySound
		{
			get
			{
				return playSound;
			}
			set
			{
				playSound = value;
			}
		}

		private string soundFilePath;
		/// <summary>
		/// IsPlaySound プロパティが true の時、再生されるサウンドへのファイルパスを表します。
		/// </summary>
		public string SoundFilePath
		{
			get
			{
				return soundFilePath;
			}
			set
			{
				soundFilePath = value;
			}
		}
	
		private bool popup;
		/// <summary>
		/// 一致した単語を表示したときにポップアップメッセージを表示するかどうかを表します。
		/// </summary>
		public bool IsPopup
		{
			get
			{
				return popup;
			}
			set
			{
				popup = value;
			}
		}

		private string popupText;
		/// <summary>
		/// IsPopup プロパティが true の時、表示されるメッセージ文字列を表します。
		/// </summary>
		public string PopupText
		{
			get
			{
				return popupText;
			}
			set
			{
				popupText = value;
			}
		}

		public ColorWordInfo(ColorWordInfo info)
		{
			backColor = info.backColor;
			foreColor = info.foreColor;
			bold = info.bold;
			italic = info.italic;
			regex = info.regex;
			playSound = info.playSound;
			popup = info.popup;
			text = info.text;
			popupText = info.popupText;
			soundFilePath = info.soundFilePath;
		}

		public ColorWordInfo()
		{
			text = popupText = soundFilePath = String.Empty;
			bold = italic = regex = playSound = popup = false;
			backColor = SystemColors.Window;
			foreColor = SystemColors.WindowText;
		}

	}
}
