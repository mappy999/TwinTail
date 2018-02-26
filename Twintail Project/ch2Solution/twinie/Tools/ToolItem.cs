// ToolItem.cs
// #2.0

namespace Twin.Tools
{
	using System;

	/// <summary>
	/// 外部ツールを起動するための情報を管理します。
	/// </summary>
	public class ToolItem
	{
		private string name = String.Empty;
		private string parameter = String.Empty;
		private string fileName = String.Empty;

		/// <summary>
		/// 表示上のツール名を取得または設定します。
		/// </summary>
		public string Name
		{
			set
			{
				name = value;
			}
			get
			{
				return name;
			}
		}

		/// <summary>
		/// 起動する外部ツールのファイル名を取得または設定します。
		/// </summary>
		public string FileName
		{
			set
			{
				fileName = value;
			}
			get
			{
				return fileName;
			}
		}

		/// <summary>
		/// FileName プロパティで設定されたファイルに渡すパラメータを取得または設定します。
		/// </summary>
		public string Parameter
		{
			set
			{
				parameter = value;
			}
			get
			{
				return parameter;
			}
		}

		public ToolItem()
		{
		}

		public ToolItem(string name, string filename, string param)
		{
			this.name = name;
			this.fileName = filename;
			this.parameter = param;
		}

		public override string ToString()
		{
			return name;
		}
	}
}
