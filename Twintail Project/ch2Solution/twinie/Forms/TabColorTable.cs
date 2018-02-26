using System;
using System.Collections.Generic;
using System.Text;
using CSharpSamples;
using System.Drawing;
using System.IO;
using Twin.Forms;

namespace Twin
{
	/// <summary>
	/// �^�u�̔z�F���ƃ^�u��������֘A�Â���e�[�u���ł��B
	/// </summary>
	public class TabColorTable
	{
		private const string __SectionName = "Colors"; // ����ver�̐ݒ�̂ݎg�p

		private string fileName;

		private Dictionary<string, TabColorSet> colorDic =
			new Dictionary<string, TabColorSet>();

		public TabColorTable(string fileName)
		{
			this.fileName = fileName;
		}

		private string TrimKey(string key)
		{
			return key.Trim().Replace("]", "");
		}

		/// <summary>
		/// �e�[�u������ key ���܂ސݒ肪���݂��邩�ǂ����𒲂ׂ܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainKey(string key)
		{
			return colorDic.ContainsKey(TrimKey(key));
		}

		/// <summary>
		/// �w�肵�� key �� colorSet ���֘A�Â��܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <param name="colorSet"></param>
		public void Set(string key, TabColorSet colorSet)
		{
			if (colorSet.IsDefaultColor)
			{
				colorDic.Remove(TrimKey(key));
			}
			else
			{
				colorDic[TrimKey(key)] = colorSet;
			}
		}

		/// <summary>
		/// �w�肵�� key �����񂩂�z�F�ݒ�����o���܂��B
		/// key �����݂��Ȃ��ꍇ�͋�� TabColorSet ��Ԃ��܂��B
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public TabColorSet FromKey(string key)
		{
			key = TrimKey(key);

			if (colorDic.ContainsKey(key))
			{
				return colorDic[key];
			}
			else
			{
				return new TabColorSet();
			}
		}

		/// <summary>
		/// �e�[�u�����̏����t�@�C���ɕۑ����܂��B
		/// </summary>
		public void Save()
		{
			CSPrivateProfile p = new CSPrivateProfile();

			foreach (KeyValuePair<string, TabColorSet> kv in colorDic)
			{
				CSPrivateProfileSection section = new CSPrivateProfileSection(kv.Key);

				section["ActiveBackColor"] = ColorTranslator.ToHtml(kv.Value.ActiveBackColor);
				section["ActiveForeColor"] = ColorTranslator.ToHtml(kv.Value.ActiveForeColor);
				section["DeactiveBackColor"] = ColorTranslator.ToHtml(kv.Value.DeactiveBackColor);
				section["DeactiveForeColor"] = ColorTranslator.ToHtml(kv.Value.DeactiveForeColor);

				p.Sections.Add(section);
			}

			p.Write(fileName);
		}

		/// <summary>
		/// �t�@�C������z�F����ǂݍ��݂܂��B
		/// </summary>
		public void Load()
		{
			if (!File.Exists(fileName))
				return;

			colorDic.Clear();

			CSPrivateProfile p = new CSPrivateProfile();
			p.Read(fileName);

			foreach (CSPrivateProfileSection section in p.Sections)
			{
				TabColorSet set = new TabColorSet();

				set.ActiveBackColor =
					ColorTranslator.FromHtml(section["ActiveBackColor"]);

				set.ActiveForeColor =
					ColorTranslator.FromHtml(section["ActiveForeColor"]);

				set.DeactiveBackColor =
					ColorTranslator.FromHtml(section["DeactiveBackColor"]);

				set.DeactiveForeColor =
					ColorTranslator.FromHtml(section["DeactiveForeColor"]);

				colorDic.Add(section.Name, set);
			}

		}
	}

	/// <summary>
	/// �^�u�̔z�F����\���܂��B
	/// </summary>
	public class TabColorSet
	{
		private Color activeForeColor;
		/// <summary>
		/// �A�N�e�B�u�ȃ^�u�̕����F�ł��B
		/// </summary>
		public Color ActiveForeColor
		{
			get
			{
				return activeForeColor;
			}
			set
			{
				activeForeColor = value;
			}
		}

		private Color activeBackColor;
		/// <summary>
		/// �A�N�e�B�u�ȃ^�u�̔w�i�F�ł��B
		/// </summary>
		public Color ActiveBackColor
		{
			get
			{
				return activeBackColor;
			}
			set
			{
				activeBackColor = value;
			}
		}

		private Color deactiveForeColor;
		/// <summary>
		/// ��A�N�e�B�u�ȃ^�u�̕����F�ł��B
		/// </summary>
		public Color DeactiveForeColor
		{
			get
			{
				return deactiveForeColor;
			}
			set
			{
				deactiveForeColor = value;
			}
		}

		private Color deactiveBackColor;
		/// <summary>
		/// ��A�N�e�B�u�ȃ^�u�̔w�i�F�ł��B
		/// </summary>
		public Color DeactiveBackColor
		{
			get
			{
				return deactiveBackColor;
			}
			set
			{
				deactiveBackColor = value;
			}
		}

		/// <summary>
		/// ���ׂĂ̐F���f�t�H���g�F�ł���� true�A
		/// �����ꂩ��̐F���ύX����Ă���� false ��Ԃ��܂��B
		/// </summary>
		public bool IsDefaultColor
		{
			get
			{
				if (activeForeColor == SystemColors.ControlText &&
					activeBackColor == SystemColors.Control &&
					deactiveForeColor == SystemColors.ControlLightLight &&
					deactiveBackColor == SystemColors.ControlDark)
				{
					return true;
				}
				else
					return false;
			}
		}

		/// <summary>
		/// �f�t�H���g�̔z�F�ݒ���擾���܂��B
		/// </summary>
		public static readonly TabColorSet Default = new TabColorSet();

		/// <summary>
		/// ���ׂĂ̐F�����Z�b�g���A�f�t�H���g�F�ɖ߂��܂��B
		/// </summary>
		public void Reset()
		{
			activeForeColor = SystemColors.ControlText;
			activeBackColor = SystemColors.Control;
			deactiveForeColor = SystemColors.ControlLightLight;
			deactiveBackColor = SystemColors.ControlDark;
		}

		public TabColorSet()
		{
			Reset();
		}

		public TabColorSet(TabColorSet source)
		{
			this.activeForeColor = source.activeForeColor;
			this.activeBackColor = source.activeBackColor;
			this.deactiveForeColor = source.deactiveForeColor;
			this.deactiveBackColor = source.deactiveBackColor;
		}
	}
}
