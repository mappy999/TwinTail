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
	/// �O���c�[�������s����N���X
	/// </summary>
	public class ExternalTool
	{
		private static Dictionary<string, string> hash = new Dictionary<string, string>();

		/// <summary>
		/// �O���c�[�������s
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

			hash["ThreadName"] = header.Subject;	// �X���b�h��
			hash["ThreadUrl"] = header.Url;		// �X���b�hURL
			hash["ThreadKey"] = header.Key;		// �X���b�h�L�[
			hash["BoardName"] = board.Name;		// ��
			hash["BoardUrl"] = board.Url;			// ��URL
			hash["BoardServer"] = board.Server;	// �T�[�o�[
			hash["BoardPath"] = board.Path;		// �p�X
			hash["CacheFolder"] = form.Settings.CacheFolderPath;	// �L���b�V���t�H���_
			hash["DatPath"] = form.Cache.GetDatPath(header);		// dat�p�X
			hash["Clipboard"] = clipboard;		// �N���b�v�{�[�h�̓��e
			hash["Selected"] = selected;			// �I������Ă��镶����

			foreach (string key in hash.Keys)
			{
				// �G���R�[�h���w�肳��Ă���Ύ擾
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
						// �w�肳�ꂽ�G���R�[�h�Œl���G���R�[�h
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
