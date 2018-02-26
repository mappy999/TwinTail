// ExternalTool.cs

namespace Twin.Tools
{
	using System;
	using System.Diagnostics;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using System.Text;
	using Twin.Forms;
	using System.Text.RegularExpressions;

	/// <summary>
	/// 外部ツールを実行するクラス
	/// </summary>
	public class ExternalTool
	{
		private static Dictionary<string, string> hash = new Dictionary<string, string>();

		/// <summary>
		/// 外部ツールを実行
		/// </summary>
		/// <param name="item"></param>
		public static void Run(Twin2IeBrowser form, ToolItem item)
		{
			ThreadHeader header =
				form.threadTabController.IsSelected ?
				form.threadTabController.Control.HeaderInfo :
				TypeCreator.CreateThreadHeader(BbsType.X2ch);

			BoardInfo board =
				header.BoardInfo;

			IDataObject data = Clipboard.GetDataObject();
			string clipboard = data.GetDataPresent(DataFormats.Text) ? (string)data.GetData(DataFormats.Text) : String.Empty;
			string selected = (form.threadTabController.IsSelected) ? form.threadTabController.Control.SelectedText : String.Empty;
			string param = item.Parameter;

			hash["ThreadName"] = header.Subject;	// スレッド名
			hash["ThreadUrl"] = header.Url;		// スレッドURL
			hash["ThreadKey"] = header.Key;		// スレッドキー
			hash["BoardName"] = board.Name;		// 板名
			hash["BoardUrl"] = board.Url;			// 板URL
			hash["BoardServer"] = board.Server;	// 板サーバー
			hash["BoardPath"] = board.Path;		// 板パス
			hash["CacheFolder"] = form.Settings.CacheFolderPath;	// キャッシュフォルダ
			hash["DatPath"] = form.Cache.GetDatPath(header);		// datパス
			hash["Clipboard"] = clipboard;		// クリップボードの内容
			hash["Selected"] = selected;			// 選択されている文字列

			foreach (string key in hash.Keys)
			{
				// エンコードが指定されていれば取得
				string pattern = "\\{" + key + ":(?<encode>[^}]+)" + "\\}";
				string value = hash[key];

				Match m = Regex.Match(param, pattern);
				if (m.Success)
				{
					Group encodeGroup = m.Groups["encode"];

#if DEBUG
					TwinDll.Output(encodeGroup.Value);
#endif
					try
					{
						// 指定されたエンコードで値をエンコード
						Encoding encode = Encoding.GetEncoding(encodeGroup.Value);
						value = UrlEncode(value, encode);

						param = param.Remove(
							encodeGroup.Index-1, encodeGroup.Length+1);
					}
					catch (Exception ex)
					{
						TwinDll.ShowOutput(ex.Message);
					}
				}
				
				param = param.Replace("{" + key + "}", value);
			}

			Process p = new Process();
			p.StartInfo.Arguments = param;
			p.StartInfo.FileName = item.FileName;
			p.Start();
		}


		private static string UrlEncode(string text, Encoding e)
		{
			return System.Web.HttpUtility.UrlEncode(text, e);
		}
	}
}
