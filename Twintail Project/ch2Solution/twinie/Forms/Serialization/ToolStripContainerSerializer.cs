using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Twintail3;

namespace Twintail.Serialization
{
	/// <summary>
	/// �Ȃ񂩕W����ToolStripManager.LoadSettings�������ƕ�������Ȃ��̂Ŏ��O�ŕۑ��E��������N���X�ł��B
	/// </summary>
	public class ToolStripContainerSerializer
	{
		private static Dictionary<string, ToolStripPanel> GetToolStripPanelDictionary(ToolStripContainer Container)
		{
			Dictionary<string, ToolStripPanel> allPanels = new Dictionary<string,ToolStripPanel>();
			allPanels.Add("Top", Container.TopToolStripPanel);
			allPanels.Add("Left", Container.LeftToolStripPanel);
			allPanels.Add("Right", Container.RightToolStripPanel);
			allPanels.Add("Bottom", Container.BottomToolStripPanel);

			return allPanels;
		}

		/// <summary>
		/// �w�肵�� ToolStripContainer ���̂��ׂẴp�l�����Ɏq�Ƃ��đ��݂���  ToolStrip �R���g���[���̈ʒu�����t�@�C���ɕۑ����܂��B
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="fileName"></param>
		public static void Save(ToolStripContainer Container, string fileName)
		{
			List<ToolStripPanelSetting> panelsSetting = new List<ToolStripPanelSetting>();
			foreach (KeyValuePair<string, ToolStripPanel> panel in GetToolStripPanelDictionary(Container))
			{
				List<ToolStripSetting> rows = new List<ToolStripSetting>();
				foreach (ToolStripPanelRow row in panel.Value.Rows)
				{
					foreach (Control c in row.Controls)
					{
						ToolStrip item = c as ToolStrip;

						if (item == null)
							return;

						rows.Add(new ToolStripSetting(item.Name, item.Location));
					}
				}

				rows.Sort();
				panelsSetting.Add(new ToolStripPanelSetting(panel.Key, rows.ToArray()));
			}

			ToolStripdockContainerSetting setting = new ToolStripdockContainerSetting(fileName, panelsSetting.ToArray());
			setting.Save();
		}

		/// <summary>
		/// �ݒ�t�@�C����ǂݍ��݁A�w�肵���R���e�i�̏�Ԃ𕜌����܂��B
		/// </summary>
		/// <param name="parentCtrl">Container���q�Ɏ��ŏ�ʂ̐e�R���g���[���A�܂��̓��C���t�H�[���B</param>
		/// <param name="Container">��Ԃ𕜌�����R���e�i</param>
		/// <param name="fileName">�ݒ�t�@�C���̕ۑ���t�@�C���p�X</param>
		public static void Load(Control parentCtrl, ToolStripContainer Container, string fileName)
		{		
			ToolStripdockContainerSetting dockContainerSetting = new ToolStripdockContainerSetting(fileName);
			dockContainerSetting.Load();

			if (!dockContainerSetting.IsDeserialized)
				return;

			Dictionary<string, ToolStripPanel> allPanels = GetToolStripPanelDictionary(Container);
			Dictionary<string, ToolStrip> tsDic = new Dictionary<string, ToolStrip>();

			Dictionary<ToolStrip, ToolStripPanel> remainds = new Dictionary<ToolStrip, ToolStripPanel>();

			// �S�p�l������ToolStrip�R���g���[�������ׂ� tsDic �ɓ���A��x�R���g���[���R���N�V�������N���A����
			foreach (ToolStripPanel panel in allPanels.Values)
			{
				foreach (Control c in panel.Controls)
				{
					if (c is ToolStrip)
					{
						tsDic.Add(c.Name, (ToolStrip)c);
						remainds.Add((ToolStrip)c, panel);
					}
				}
				panel.Controls.Clear();
			}

			// �ۑ�����Ă��鏇�ɍēx�p�l����ToolStrip��ǉ����Ă���
			foreach (ToolStripPanelSetting panelSetting in dockContainerSetting.Panels)
			{
				if (!allPanels.ContainsKey(panelSetting.Alignment))
					continue;

				ToolStripPanel panel = allPanels[panelSetting.Alignment];
				foreach (ToolStripSetting setting in panelSetting.Rows)
				{
					ToolStrip item = null;

					if (tsDic.ContainsKey(setting.Name))
					{
						item = tsDic[setting.Name];
					}
					else if (parentCtrl != null)
					{
						// ���݂�ToolStripContainer���ɃR���g���[����������Ȃ��ꍇ�͐e�̃R���g���[��������T��
						foreach (Control c in parentCtrl.Controls.Find(setting.Name, true))
						{
							if (c is ToolStrip)
							{
								item = (ToolStrip)c;
								break;
							}
						}
					}

					if (item != null)
					{
						panel.Join(item, setting.Location);
						remainds.Remove(item);
					}
				}
			}

			// �ǂ��ɂ��ǉ����ꂸ�Ɏc����ToolStrip�����Ƃ��Ƃ������p�l���ɒǉ�����
			foreach (KeyValuePair<ToolStrip, ToolStripPanel> pair in remainds)
				pair.Value.Join(pair.Key, Point.Empty);
		}
	}

	public class ToolStripdockContainerSetting : ApplicationSettingsSerializer
	{
		ToolStripPanelSetting[] panels;
		[ExpandableSerialize,DefaultArrayLength(0)]
		public ToolStripPanelSetting[] Panels
		{
			get
			{
				return panels;
			}
			set
			{
				panels = value;
			}
		}
		
		public ToolStripdockContainerSetting(string fileName)
			: this(fileName, new ToolStripPanelSetting[] {})
		{
		}

		public ToolStripdockContainerSetting(string fileName, ToolStripPanelSetting[] panels)
			: base(fileName, true)
		{
			this.Panels = panels;
		}
	}

	[Serializable]
	public class ToolStripPanelSetting
	{
		string alignment;
		public string Alignment
		{
			get
			{
				return alignment;
			}
			set
			{
				alignment = value;
			}
		}

		ToolStripSetting[] rows;
		[ExpandableSerialize,DefaultArrayLength(0)]
		public ToolStripSetting[] Rows
		{
			get
			{
				return rows;
			}
			set
			{
				rows = value;
			}
		}

		public ToolStripPanelSetting()
			: this(String.Empty, new ToolStripSetting[] { })
		{
		}

		public ToolStripPanelSetting(string alignment, ToolStripSetting[] rows)
		{
			this.Alignment = alignment;
			this.Rows = rows;
		}
	}

	[Serializable]
	public class ToolStripSetting : IComparable<ToolStripSetting>
	{
		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		private Point location;
		public Point Location
		{
			get
			{
				return location;
			}
			set
			{
				location = value;
			}
		}


		public ToolStripSetting()
			: this(String.Empty, Point.Empty)
		{
		}

		public ToolStripSetting(string name, Point location)
		{
			this.Name = name;
			this.Location = location;
		}

		#region IComparable<ToolStripSettings> �����o

		public int CompareTo(ToolStripSetting item)
		{
			int ret = this.Location.X - item.Location.X;
			return ret != 0 ? ret : this.Location.Y - item.Location.Y;
		}

		#endregion
	}
}
