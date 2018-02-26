// ImageViewerSettings.cs

namespace ImageViewerDll
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Soap;
	using CSharpSamples;

	/// <summary>
	/// ImageViewerSettings の概要の説明です。
	/// </summary>
	[Serializable]
	public class ImageViewerSettings : SerializableSettings
	{
		/// <summary>
		/// ImageViewerSettingsクラスのインスタンスを初期化
		/// </summary>
		public ImageViewerSettings()
		{
		}

		/// <summary>
		/// ImageViewerSettingsクラスのインスタンスを初期化
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public ImageViewerSettings(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// 設定を保存
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="ivs"></param>
		public static void Save(string filePath, ImageViewerSettings ivs)
		{
			//using (FileStream fs = new FileStream(filePath, FileMode.Create))
			//{
			//    SoapFormatter soap = new SoapFormatter();
			//    soap.Serialize(fs, ivs);
			//}
			CSPrivateProfile p = new CSPrivateProfile();

			p.SetValue("ImageViewerSettings", "TopMost", ivs.TopMost);
			p.SetValue("ImageViewerSettings", "NoOverwrite", ivs.NoOverwirte);
			p.SetValue("ImageViewerSettings", "Activate", ivs.Activate);
			p.SetValue("ImageViewerSettings", "AutoHide", ivs.AutoHide);
			p.SetValue("ImageViewerSettings", "Mosaic", ivs.Mosaic);
			p.SetValue("ImageViewerSettings", "ImageCacheFolder", ivs.ImageCacheFolder);
			p.SetValue("ImageViewerSettings", "WebBrowserPath", ivs.WebBrowserPath);
			p.SetValue("ImageViewerSettings", "TabImageSize", ivs.TabImageSize);
			p.SetValue("ImageViewerSettings", "WindowBounds", ivs.WindowBounds);
			p.SetValue("ImageViewerSettings", "ViewOriginalSize", ivs.ViewOriginalSize);
			p.SetValue("ImageViewerSettings", "SavingMemory", ivs.SavingMemory);

			int i = 0;

			foreach (QuickSaveFolderItem f in ivs.QuickSaveFolders)
			{
				p.SetValue("QuickSaveFolder" + i, "Title", f.Title);
				p.SetValue("QuickSaveFolder" + i, "Shortcut", f.Shortcut.ToString());
				p.SetValue("QuickSaveFolder" + i, "FolderPath", f.FolderPath);
				i++;
			}

			i = 0;

			foreach (string uri in ivs.RecentUrl)
			{
				p.SetValue("RecentUrl", "Item" + i, uri);
				i++;
			}

			i = 0;

			foreach (string pattern in ivs.NGUrlPattern)
			{
				p.SetValue("NGUrlPattern", "Pattern" + i, pattern);
				i++;
			}

			p.Write(filePath);
		}

		/// <summary>
		/// 設定を読み込む
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static ImageViewerSettings Load(string filePath)
		{
			//SoapFormatter soap = new SoapFormatter();
			//ImageViewerSettings ivs = null;

			//try {
			//    if (File.Exists(filePath))
			//    {
			//        using (FileStream fs = new FileStream(filePath, FileMode.Open))
			//            ivs = (ImageViewerSettings)soap.Deserialize(fs);
			//    }
			//}
			//catch (Exception ex) 
			//{
			//    MessageBox.Show(ex.ToString());
			//}
			//finally {
			//    if (ivs == null)
			//        ivs = new ImageViewerSettings();
			//}

			//return ivs;

			ImageViewerSettings ivs = new ImageViewerSettings();

			CSPrivateProfile p = new CSPrivateProfile();
			p.Read(filePath);

			ivs.TopMost = p.GetBool("ImageViewerSettings", "TopMost", ivs.TopMost);
			ivs.NoOverwirte = p.GetBool("ImageViewerSettings", "NoOverwrite", ivs.NoOverwirte);
			ivs.Activate = p.GetBool("ImageViewerSettings", "Activate", ivs.Activate);
			ivs.AutoHide = p.GetBool("ImageViewerSettings", "AutoHide", ivs.AutoHide);
			ivs.Mosaic = p.GetBool("ImageViewerSettings", "Mosaic", ivs.Mosaic);
			ivs.ImageCacheFolder = p.GetString("ImageViewerSettings", "ImageCacheFolder", ivs.ImageCacheFolder);
			ivs.WebBrowserPath = p.GetString("ImageViewerSettings", "WebBrowserPath", ivs.WebBrowserPath);
			ivs.TabImageSize = p.GetSize("ImageViewerSettings", "TabImageSize", ivs.TabImageSize);
			ivs.WindowBounds = p.GetRect("ImageViewerSettings", "WindowBounds", Rectangle.Empty);
			ivs.ViewOriginalSize = p.GetBool("ImageViewerSettings", "ViewOriginalSize", false);
			ivs.SavingMemory = p.GetBool("ImageViewerSettings", "SavingMemory", true);

			int i = 0;

			while (true)
			{
				QuickSaveFolderItem item = new QuickSaveFolderItem();

				item.Title = p.GetString("QuickSaveFolder" + i, "Title", String.Empty);
				item.FolderPath = p.GetString("QuickSaveFolder" + i, "FolderPath", String.Empty);
				item.Shortcut = (Shortcut)p.GetEnum("QuickSaveFolder" + i, "Shortcut", Shortcut.None);

				if (item.FolderPath == String.Empty)
					break;

				ivs.QuickSaveFolders.Add(item);
				i++;
			}

			i = 0;

			while (true)
			{
				string uri = p.GetString("RecentUrl", "Item" + i, null);

				if (uri == null)
					break;

				ivs.RecentUrl.Add(uri);
				i++;
			}

			i = 0;
			List<string> list = new List<string>();

			while (true)
			{
				string pattern = p.GetString("NGUrlPattern", "Pattern" + i, null);

				if (pattern == null)
					break;

				list.Add(pattern);
				i++;
			}
			ivs.NGUrlPattern = list.ToArray();

			return ivs;
		}

		/// <summary>
		/// 設定ファイルのパス
		/// </summary>
		public static readonly string SettingPath =
			Path.Combine(Application.StartupPath, "ImageViewerSettings.ini");

		/// <summary>
		/// 画像キャッシュ保存フォルダ
		/// </summary>
		public string ImageCacheFolder =
			Path.Combine(Application.StartupPath, "Images");

		/// <summary>
		/// クイック保存フォルダ
		/// </summary>
		public QuickSaveFolderCollection QuickSaveFolders =
			new QuickSaveFolderCollection();

		/// <summary>
		/// タブの画像サイズを取得または設定
		/// </summary>
		public Size TabImageSize = new Size(32, 32);

		/// <summary>最近開いたURL一覧</summary>
		public List<string> RecentUrl = new List<string>();

		/// <summary>NGURLのパターン配列</summary>
		public string[] NGUrlPattern = new string[0];

		/// <summary>使用するブラウザのパス</summary>
		public string WebBrowserPath = String.Empty;

		/// <summary>画像を開くたびにアクティブにするかどうか</summary>
		public bool Activate = false;

		/// <summary>モザイク表示</summary>
		public bool Mosaic = true;

		/// <summary>常に最全面に表示</summary>
		public bool TopMost = false;

		/// <summary>すべての画像を閉じたとき自動的に非表示にするかどうか</summary>
		public bool AutoHide = false;

		/// <summary>同名のファイルが存在したとき別名で保存</summary>
		public bool NoOverwirte = true;

		/// <summary>画像を表示する際に、リサイズせずにオリジナルのサイズで表示するかどうか</summary>
		public bool ViewOriginalSize = false;

		/// <summary>メモリ節約</summary>
		public bool SavingMemory = true;

		/// <summary>画像ビューアの位置情報</summary>
		public Rectangle WindowBounds = Rectangle.Empty;
	}
}
