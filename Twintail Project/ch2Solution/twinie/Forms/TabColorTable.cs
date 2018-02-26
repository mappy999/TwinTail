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
	/// タブの配色情報とタブ文字列を関連づけるテーブルです。
	/// </summary>
	public class TabColorTable
	{
		private const string __SectionName = "Colors"; // 初期verの設定のみ使用

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
		/// テーブル内に key を含む設定が存在するかどうかを調べます。
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainKey(string key)
		{
			return colorDic.ContainsKey(TrimKey(key));
		}

		/// <summary>
		/// 指定した key と colorSet を関連づけます。
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
		/// 指定した key 文字列から配色設定を取り出します。
		/// key が存在しない場合は空の TabColorSet を返します。
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
		/// テーブル内の情報をファイルに保存します。
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
		/// ファイルから配色情報を読み込みます。
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
	/// タブの配色情報を表します。
	/// </summary>
	public class TabColorSet
	{
		private Color activeForeColor;
		/// <summary>
		/// アクティブなタブの文字色です。
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
		/// アクティブなタブの背景色です。
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
		/// 非アクティブなタブの文字色です。
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
		/// 非アクティブなタブの背景色です。
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
		/// すべての色がデフォルト色であれば true、
		/// いずれか一つの色が変更されていれば false を返します。
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
		/// デフォルトの配色設定を取得します。
		/// </summary>
		public static readonly TabColorSet Default = new TabColorSet();

		/// <summary>
		/// すべての色をリセットし、デフォルト色に戻します。
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
