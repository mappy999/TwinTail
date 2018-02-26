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
		/// �F�t�����s���������\���܂��B
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
		/// �F�t���̕����F��\���܂��B
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
		/// �F�t���̔w�i�F��\���܂��B
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
		/// �����\�����邩�ǂ�����\���܂��B
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
		/// �Α̕\�����邩�ǂ�����\���܂��B
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
		/// Text �v���p�e�B�͐��K�\���ł��邩�ǂ�����\���܂��B
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
		/// ��v�����P���\�������Ƃ��ɃT�E���h���Đ����邩�ǂ�����\���܂��B
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
		/// IsPlaySound �v���p�e�B�� true �̎��A�Đ������T�E���h�ւ̃t�@�C���p�X��\���܂��B
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
		/// ��v�����P���\�������Ƃ��Ƀ|�b�v�A�b�v���b�Z�[�W��\�����邩�ǂ�����\���܂��B
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
		/// IsPopup �v���p�e�B�� true �̎��A�\������郁�b�Z�[�W�������\���܂��B
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
