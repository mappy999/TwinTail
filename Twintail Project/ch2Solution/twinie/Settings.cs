// Settings.cs

namespace Twin.Forms
{
	using System;
	using System.IO;
	using System.Text;
	using System.Net;
	using System.Drawing;
	using System.Drawing.Design;
	using System.Threading;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Windows.Forms.Design;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Soap;
	using System.Collections.Specialized;
	using System.Xml.Serialization;
	using System.Security.Cryptography;
	using System.Xml;
	using Twin.Text;
	using Twin.Forms;
	using Twin.Bbs;
	using CSharpSamples;
	using System.Collections.Generic;
	using System.Collections;
	using System.Text.RegularExpressions;

	/// <summary>
	/// twintailの共通設定情報を表す
	/// </summary>
	[Serializable]
	public class Settings : SerializableSettings
	{
		/// <summary>WebサイトへのURL</summary>
		public static readonly string WebSiteUrl =
			"http://www.geocities.jp/nullpo0/";

		/// <summary>設定ファイルのパス</summary>
		public static readonly string TPath =
			Path.Combine(Application.StartupPath, "twin.xml");

		public static readonly string TPathNew =
			Path.Combine(Application.StartupPath, "twin.ini");

		/// <summary>ログ情報を保存するファイルパス</summary>
		public static readonly string ErrorLogPath =
			Path.Combine(Application.StartupPath, "twin.log");

		/// <summary>2channel.brdの存在するファイルパス</summary>
		public static readonly string BoardTablePath =
			Path.Combine(Application.StartupPath, "2channel.brd");

		public static readonly string ImageViewUrlReplacePath =
			Path.Combine(Application.StartupPath, "ImageViewURLReplace.dat");

		/// <summary>サーバー負荷監視所のURL</summary>
		public static readonly string LoadFactorUrl = "http://ch2.ath.cx/";

		/// <summary>2ch鯖勝手な監視所</summary>
		public static readonly string ServerWatcherUrl = "http://www.ownerpet.com/";

		/// <summary>2ch鯖監視係</summary>
		public static readonly string ServerWatcher2Url = "http://sv2ch.baila6.jp/sv2ch01.html";

		/// <summary>User-Agentを表す</summary>
		public static readonly string UserAgent = TwinDll.UserAgent;

		/// <summary>スプラッシュイメージが存在するパス</summary>
		public static readonly string SlpashImagePath =
			Path.Combine(Application.StartupPath, "Splash.bmp");

		#region Window Settings
		/// <summary>最大化されているかどうか</summary>
		public bool IsMaximized = false;
		/// <summary>ウインドウの位置</summary>
		public Point WindowLocation = Point.Empty;
		/// <summary>ウインドウのサイズ</summary>
		public Size WindowSize = Size.Empty;
		#endregion

		#region ファイルまたはディレクトリのパス関係
		/// <summary>ユーザー設定情報の存在するフォルダ</summary>
		public static readonly string UserFolderPath =
			Path.Combine(Application.StartupPath, "User");

		/// <summary>NGワード設定情報の存在するフォルダ</summary>
		public static readonly string NGWordsFolderPath =
			Path.Combine(Application.StartupPath, "NGWords");

		/// <summary>Aaフォルダの存在するフォルダ</summary>
		public static readonly string AaFolderPath =
			Path.Combine(Application.StartupPath, "AA");

		/// <summary>スキンが存在する基本フォルダへのパス</summary>
		public static readonly string BaseSkinFolderPath =
			Path.Combine(Application.StartupPath, "Skin");

		/// <summary>サウンドが存在するフォルダへのパス</summary>
		public static readonly string SoundFolderPath =
			Path.Combine(Application.StartupPath, "Sound");

		/// <summary>ログ保存フォルダ</summary>
		public string CacheFolderPath =
			Path.Combine(Application.StartupPath, "Cache");

		/// <summary>スクラップ保存フォルダ</summary>
		public static readonly string ScrapFolderPath =
			Path.Combine(Application.StartupPath, "Scrap");

		/// <summary>バックアップ保存フォルダ</summary>
		public static readonly string BackupFolderPath =
			Path.Combine(Application.StartupPath, "Backup");

		/// <summary>イメージキャッシュ保存フォルダ</summary>
		public static readonly string ImageCacheDirectory =
			Path.Combine(Application.StartupPath, "ImageCache");

		/// <summary>SETTING.TXT保存フォルダ</summary>
		public static readonly string SettingTxtFolderPath =
			Path.Combine(Application.StartupPath, "SETTING_TXT");

		public static readonly string MouseGestureSettingPath =
			Path.Combine(UserFolderPath, "MouseGestureSetting.xml");

		public static readonly string GroupFolderPath =
			Path.Combine(UserFolderPath, "Group");

		/// <summary>書き込み履歴保存フォルダ</summary>
		public static readonly string KakikoFolderPath =
			Path.Combine(UserFolderPath, "kakiko");

		/// <summary>アップデータへのファイルパス</summary>
		public static string UpdateInfoUri =
			"http://www.geocities.co.jp/SiliconValley/5459/twinup.txt";

		/// <summary>ヘルプファイルの存在するパス</summary>
		public static readonly string HelpFilePath =
			Path.Combine(Application.StartupPath, "twin2help.chm");

		/// <summary>ユーザー定義の外部板ファイルのパス</summary>
		public static readonly string UserTablePath =
			Path.Combine(UserFolderPath, "User.brd");

		/// <summary>外部ツールの保存ファイルパス</summary>
		public static readonly string ToolsFilePath =
			Path.Combine(UserFolderPath, "Tools.txt");

		/// <summary>最近閉じたスレ一覧ファイルの存在するパス</summary>
		public static readonly string ClosedHistoryPath =
			Path.Combine(UserFolderPath, "Closed.txt");

		/// <summary>最近書き込んだスレッド一覧ファイルの存在するパス</summary>
		public static readonly string WrittenHistoryPath =
			Path.Combine(UserFolderPath, "Written.txt");

		/// <summary>お気に入りファイルの存在するパス</summary>
		public static readonly string BookmarkPath =
			Path.Combine(UserFolderPath, "Favorites.txt");

		/// <summary>過去ログ倉庫ファイルの存在するパス</summary>
		public static readonly string WarehousePath =
			Path.Combine(UserFolderPath, "Warehouse.txt");

		public static readonly string ItaBotanPath =
			Path.Combine(UserFolderPath, "Itabotan.txt");

		/// <summary>NGアドレス一覧ファイルのパス</summary>
		public static readonly string NGAddrsPath =
			Path.Combine(UserFolderPath, "NGAddrs.txt");

		/// <summary>メニューのショートカットキー情報が格納されているパス</summary>
		public static readonly string MenuShortcutPath =
			Path.Combine(UserFolderPath, "Shortcuts2.txt");

		/// <summary>samba24対策の秒数表へのパス</summary>
		public static readonly string Samba24Path =
			Path.Combine(Application.StartupPath, "samba.ini");

		/// <summary>コテハン設定情報のパス</summary>
		public static readonly string KotehanFilePath =
			Path.Combine(UserFolderPath, "kotehan.ini");

		public static readonly string ColorTableSettingsPath =
			Path.Combine(UserFolderPath, "ColorTableSettings.ini");

		public static readonly string ColorWordInfoSettingsPath =
			Path.Combine(UserFolderPath, "ColorWordInfoSettings.xml");

		/// <summary>使用するWebブラウザへのファイルパス</summary>
		public string WebBrowserPath = String.Empty;

		/// <summary>板一覧のオンライン更新先URL</summary>
		public string OnlineUpdateUrl = "http://menu.2ch.net/bbsmenu.html";

		/// <summary>使用するスキンフォルダの名前</summary>
		public string SkinFolderName = "default";
		#endregion


		public string AditionalAgreementField = String.Empty;
		public string AddWriteSection = String.Empty;// "&AditionalAgreementField=&kiri=tanpo";

		#region 設定全般
		/// <summary>
		/// タブを閉じたときに次に選択するタブを表す
		/// </summary>
		public TabCloseAfterSelectionMode TabCloseAfterSelection = TabCloseAfterSelectionMode.Left;

		public bool IsTasktray = false;

		/// <summary>
		/// 新しいキャッシュ構造に移行したかどうか
		/// </summary>
		public bool NewCacheStruct = false;

		public bool IsConvertedShortcutKeyFile = false;
		public bool ConnectionLimit = false;
		public bool ClosingConfirm = false;

		/// <summary>
		/// NGワードを書き込んだレスを自動でNGIDに追加するかどうか
		/// </summary>
		public bool AutoNGRes = false;
		/// <summary>NGワードのOn/Off</summary>
		public bool NGWordsOn = true;

		/// <summary>1日経つと自動でNGIDをクリアするかどうか</summary>
		public bool NGIDAutoClear = false;

		/// <summary>最後にアプリケーションを終了した日付を表す</summary>
		public int LastExitDay = 99;

		/// <summary>OverThreadを再利用するかどうか</summary>
		public bool RecycleOverThread = false;

		/// <summary>実況モード</summary>
		public bool Livemode = false;

		/// <summary>ログ保存モードかどうかを取得または設定</summary>
		public bool Caching = true;

		/// <summary>画面構成を取得または設定</summary>
		public DisplayLayout Layout = DisplayLayout.Default;

		/// <summary>起動時に更新チェックするかどうか</summary>
		public bool UpdateCheck = true;

		/// <summary>定期的にGC.Collectを実行するかどうか</summary>
		public bool GarbageCollect = true;

		/// <summary>スレッドの優先度</summary>
		public ThreadPriority Priority = ThreadPriority.Normal;

		/// <summary>
		/// 新規タブを追加する際の動作
		/// </summary>
		public NewTabPosition NewTabPosition = NewTabPosition.Last;

		/// <summary>スレッド一覧を常に新しいタブで開くかどうか</summary>
		public bool ListAlwaysNewTab = false;

		/// <summary>スレッドを常に新しいタブで開くかどうか</summary>
		public bool ThreadAlwaysNewTab = true;

		/// <summary>カテゴリを常に１つだけ開くかどうか</summary>
		public bool AlwaysSingleOpen = false;

		/// <summary>オンライン・オフラインの状態を表す</summary>
		public bool Online = true;

		/// <summary>ログのGzip圧縮を使用するかどうか</summary>
		public bool UseGzipArchive = false;

		/// <summary>
		/// 画像ビューアを使用するかどうか
		/// </summary>
		public bool ImageViewer = false;
		public bool ImageViewer_AutoOpen = false;

		/// <summary>日付のフォーマットを表す</summary>
		public string DateFormat = "yy/MM/dd HH:mm:ss";

		/// <summary>勢いを表すための単位</summary>
		public ForceValueOf ForceValueType = ForceValueOf.Day;

		/// <summary>レス参照に使用するアンカー文字列</summary>
		public string ResRefAnchor = ">>";

		/// <summary>外部ツールのインデックス</summary>
		public int SelectedToolsIndex = 0;

		/// <summary>次スレチェッカーの有効・無効</summary>
		public bool NextThreadChecker = true;
		/// <summary>
		/// 次スレ検索の精度を高くするかどうか
		/// </summary>
		public bool NextThreadChecker_HighLevelMatch = false;

		/// <summary>あぼーん情報を取得</summary>
		public ABone ABone = new ABone(true, false);

		/// <summary>次回起動時に開くURL</summary>
		public StringCollection StartupUrls = new StringCollection();

		/// <summary>起動時に前回開いていた状態を復元するかどうか</summary>
		public bool OpenStartupUrls = false;

		/// <summary>新着スレと判断される経過時間</summary>
		public TimeSpan NewThreadTimeSpan = new TimeSpan(24, 0, 0);

		/// <summary>スレッドを選択した際に自動で本文にフォーカスをあわせるかどうか</summary>
		public bool ThreadAutoFocus = false;

		public int ImageCacheClient_SizeLimit = 1024 * 1024 * 3; // 3MB

		public bool Patrol_HiddenPastlog = false;
		
		public string LastSelectedDirectoryPath = null;
		public bool RefFolderBrowserDialog_SetDefaultPathChecked = false;

		public bool EnsureVisibleBoard = true;
		public bool NG924 = false; // dat番号が924で始まるスレッドをスレッド一覧に表示するかしないか

		public bool UseVisualStyle = false;	// NTwin23.102
		#endregion

		#region 各機能ごとの設定
		public ThreadSettings Thread = new ThreadSettings();
		public PopupSettings Popup = new PopupSettings();
		public PostSettings Post = new PostSettings();
		public NetworkSettings Net = new NetworkSettings();
		public Thumbnail Thumbnail = new Thumbnail(50, 50);
		public ViewSettings View = new ViewSettings();
		public RebarSettings Rebar = new RebarSettings();
		public SearchSettings Search = new SearchSettings();
		public DialogSettings Dialogs = new DialogSettings();
		public DesignSettings Design = new DesignSettings();
		public OperationSettings Operate = new OperationSettings();
		public SoundSettings Sound = new SoundSettings();
		public AuthenticationSettings Authentication = new AuthenticationSettings();
		public BeSettings Be = new BeSettings();
		public ThreadToolPanel ThreadToolPanel = new ThreadToolPanel();
		#endregion

		/// <summary>
		/// 設定されているスキンフォルダへのパスを取得
		/// </summary>
		public string SkinFolderPath
		{
			get
			{
				return Path.Combine(BaseSkinFolderPath, SkinFolderName);
			}
		}

		/// <summary>
		/// 設定保存用のフォルダを作成
		/// </summary>
		public void CreateFolders()
		{
			if (CacheFolderPath == null)
				CacheFolderPath = Path.Combine(Application.StartupPath, "Cache");

			if (!Directory.Exists(CacheFolderPath))
				Directory.CreateDirectory(CacheFolderPath);

			if (!Directory.Exists(ScrapFolderPath))
				Directory.CreateDirectory(ScrapFolderPath);

			if (!Directory.Exists(UserFolderPath))
				Directory.CreateDirectory(UserFolderPath);

			if (!Directory.Exists(AaFolderPath))
				Directory.CreateDirectory(AaFolderPath);

			if (!Directory.Exists(NGWordsFolderPath))
				Directory.CreateDirectory(NGWordsFolderPath);

			if (!Directory.Exists(BackupFolderPath))
				Directory.CreateDirectory(BackupFolderPath);

			if (!Directory.Exists(GroupFolderPath))
				Directory.CreateDirectory(GroupFolderPath);

			if (!Directory.Exists(ImageCacheDirectory))
				Directory.CreateDirectory(ImageCacheDirectory);

			if (!Directory.Exists(SettingTxtFolderPath))
				Directory.CreateDirectory(SettingTxtFolderPath);
		}

		/// <summary>
		/// 指定したファイルに現在の設定をシリアル化
		/// </summary>
		/// <param name="filePath"></param>
		public static void Serialize(string filePath, Settings obj)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}

			//using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
			//{
			//    SoapFormatter formatter = new SoapFormatter();
			//    formatter.Serialize(fileStream, obj);
			//}

			XmlTextWriter xmlOut = new XmlTextWriter(filePath, Encoding.GetEncoding("shift_jis"));
			xmlOut.Formatting = Formatting.Indented;

			try
			{
				xmlOut.WriteStartDocument();
				xmlOut.WriteStartElement("Settings");
				xmlOut.WriteAttributeString("Version", "1.0");

				WriteToXml(xmlOut,
					typeof(Settings).GetFields(BindingFlags.Public | BindingFlags.Instance), obj);

				xmlOut.WriteEndElement();
				xmlOut.WriteEndDocument();
			}
			finally
			{
				xmlOut.Close();
			}

		}

		private static void WriteToXml(XmlTextWriter xmlOut, FieldInfo[] fieldArray, object instance)
		{

			foreach (FieldInfo field in fieldArray)
			{
				xmlOut.WriteStartElement(field.Name);

				object val = field.GetValue(instance);
				TypeConverter converter;
				
				if (val == null) converter = TypeDescriptor.GetConverter(field.FieldType);
				else converter = TypeDescriptor.GetConverter(val);

				if (converter.GetType() == typeof(TwinExpandableConverter))
				{
					WriteToXml(xmlOut, field.FieldType.GetFields(
						BindingFlags.Instance | BindingFlags.Public), val);
				}
				else
				{
					xmlOut.WriteString(converter.ConvertToString(val));
				}

				xmlOut.WriteEndElement();
			}

		}

		/// <summary>
		/// 指定したファイルから設定を逆シリアル化
		/// </summary>
		/// <param name="filePath"></param>
		public static Settings Deserialize(string filePath)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}

			if (!File.Exists(filePath))
				return new Settings();

			XmlTextReader xmlIn = new XmlTextReader(filePath);

			try
			{
				xmlIn.MoveToContent();

				// 旧式の設定ファイル
				if (xmlIn.Name == "SOAP-ENV:Envelope")
				{
					xmlIn.Close();
					xmlIn = null;

					if (File.Exists(filePath))
					{
						// 一応、旧式の設定をバックアップしておく
						File.Copy(filePath, Path.ChangeExtension(filePath, ".bak"), true);
					}

					return Deserialize_Old(filePath);
				}
				// 新しい設定ファイル
				else if (xmlIn.Name == "Settings")
				{
					return ReadXmlSettings(xmlIn, new Settings());
				}
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex.ToString());
			}
			finally
			{
				if (xmlIn != null)
					xmlIn.Close();
			}

			return new Settings();
		}

		private static Settings Deserialize_Old(string filePath)
		{
			Settings obj = null;

			if (File.Exists(filePath))
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
				{
					SoapFormatter formatter = new SoapFormatter();
					obj = formatter.Deserialize(fileStream) as Settings;
				}
			}
			else
			{
				obj = new Settings();
			}

			obj.SetCustomTypeConverter();

			return obj;
		}

		private static Settings ReadXmlSettings(XmlTextReader xmlIn, Settings settings)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(xmlIn);

			XmlNode root = doc.SelectSingleNode("Settings");

			ReadXmlSettings(root, settings);

			// 値が新しく設定すると、もう一度型コンバータをセットしなおす必要がある
			settings.SetCustomTypeConverter();

			return settings;
		}

		private static void ReadXmlSettings(XmlNode parent, object instanceObj)
		{
			Type instanceType = instanceObj.GetType();

			foreach (XmlNode node in parent.ChildNodes)
			{
				FieldInfo field = instanceType.GetField(node.Name);

				if (field != null)
				{
					object val = field.GetValue(instanceObj);
					TypeConverter converter;
					
					if (val == null) converter = TypeDescriptor.GetConverter(field.FieldType);
					else converter = TypeDescriptor.GetConverter(val);

					if (converter.GetType() == typeof(TwinExpandableConverter))
					{
						ReadXmlSettings(node, val);
					}
					else
					{
						// 新しい値を設定してしまうため、コンバータが解除されてしまう？
						field.SetValue(instanceObj, converter.ConvertFrom(node.InnerText));
					}
				}
			}
		}


		/// <summary>
		/// Settingsクラスのインスタンスを初期化
		/// </summary>
		public Settings()
		{
			SetCustomTypeConverter();
		}

		/// <summary>
		/// デシリアライズ時に呼ばれる
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public Settings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			SetCustomTypeConverter();
		}

		public void SetCustomTypeConverter()
		{
			// ちょっとメモ
			// ここで登録した各インスタンスを新しく別のインスタンスに変更した場合、シリアライズに失敗するため、
			// 再度のメソッドを呼ぶ必要がある

			TypeDescriptor.AddAttributes(StartupUrls, new TypeConverterAttribute(typeof(StringCollectionConverter)));
			TypeDescriptor.AddAttributes(Dialogs.ColorDialogCustomColors, new TypeConverterAttribute(typeof(IntArrayConverter)));
			TypeDescriptor.AddAttributes(Search.SearchHistory.Keys, new TypeConverterAttribute(typeof(ArrayListConverter<string>)));
			TypeDescriptor.AddAttributes(Post.NameHistory.Keys, new TypeConverterAttribute(typeof(ArrayListConverter<string>)));
			TypeDescriptor.AddAttributes(Post.MailHistory.Keys, new TypeConverterAttribute(typeof(ArrayListConverter<string>)));

			if (Authentication.EncryptedPassword != null)
				TypeDescriptor.AddAttributes(Authentication.EncryptedPassword, new TypeConverterAttribute(typeof(Base64Converter)));
			
			if (Be.EncryptedPassword != null)
				TypeDescriptor.AddAttributes(Be.EncryptedPassword, new TypeConverterAttribute(typeof(Base64Converter)));
		}

		internal static void ConvertingShortcutKeyFile(Settings settings)
		{
			try
			{
				string text;
				using (StreamReader sr = new StreamReader(MenuShortcutPath))
				{
					text = sr.ReadToEnd();
				}

				text = Regex.Replace(text,
					@"Version=2\.\d\.\d\.\d, Culture=neutral, PublicKeyToken=\w+",
					String.Format("Version={0}, Culture=neutral, PublicKeyToken=4bb9ff1727802d42", Twinie.Version.ToString()));

				using (StreamWriter sw = new StreamWriter(MenuShortcutPath, false))
				{
					sw.Write(text);
				}

				settings.IsConvertedShortcutKeyFile = true;
			}
			catch
			{
			}
		}
	}

	/// <summary>
	/// ダイアログ設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class DialogSettings : SerializableSettings
	{
		public Point AddBookmarkDialog_Location = Point.Empty;
		public Size AddBookmarkDialog_Size = Size.Empty;

		public int[] ColorDialogCustomColors = new int[0];

		public DialogSettings()
		{
		}
		public DialogSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	/// <summary>
	/// 検索設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class SearchSettings : SerializableSettings
	{
		#region
		/// <summary>キャッシュ検索</summary>
		public CacheSearchSettings CacheSearch = new CacheSearchSettings();
		/// <summary>スレッド一覧検索</summary>
		public ListSearchSettings ListSearch = new ListSearchSettings();
		/// <summary>スレッド検索</summary>
		public ThreadSearchSettings ThreadSearch = new ThreadSearchSettings();
		/// <summary>レス抽出</summary>
		public ResExtractSettings ResExtract = new ResExtractSettings();

		public KeywordHistory SearchHistory = new KeywordHistory();

		/// <summary>
		/// キャッシュ検索設定
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class CacheSearchSettings : SerializableSettings
		{
			/// <summary>検索キーワード履歴</summary>
			public string Keyword = String.Empty;
			/// <summary>検索対象</summary>
			public CacheSearchTarget Target = CacheSearchTarget.Body;
			/// <summary>検索対象の板</summary>
			public BoardInfoCollection SelectedBoards = new BoardInfoCollection();

			public CacheSearchSettings()
			{
			}
			public CacheSearchSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		/// <summary>
		/// スレッド一覧検索設定
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ListSearchSettings : SerializableSettings
		{
			/// <summary>検索キーワード履歴</summary>
			//public StringCollection Keywords = new StringCollection();
			public string Keyword = String.Empty;
			/// <summary>検索オプション </summary>
			public SearchOptions Options = SearchOptions.None;
			/// <summary>インクリメンタルサーチ</summary>
			public bool IncrementalSearch = false;

			public ListSearchSettings()
			{
			}
			public ListSearchSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		/// <summary>
		/// スレッド検索設定
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ThreadSearchSettings : SerializableSettings
		{
			/// <summary>検索キーワード履歴</summary>
			//public StringCollection Keywords = new StringCollection();
			public string Keyword = String.Empty;
			/// <summary>検索オプション </summary>
			public SearchOptions Options = SearchOptions.None;

			public ThreadSearchSettings()
			{
			}
			public ThreadSearchSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		/// <summary>
		/// レス抽出検索設定
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ResExtractSettings : SerializableSettings
		{
			/// <summary>検索キーワード履歴</summary>
			//public StringCollection Keywords = new StringCollection();
			public string Keyword = String.Empty;
			/// <summary>検索オプション </summary>
			public SearchOptions Options = SearchOptions.None;
			/// <summary>検索対象</summary>
			public ResSetElement SearchTarget = ResSetElement.All;
			/// <summary>ポップアップ表示</summary>
			public bool Popup = true;

			public ResExtractSettings()
			{
			}
			public ResExtractSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		public SearchSettings()
		{
		}
		public SearchSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}

	/// <summary>
	/// レバーコントロール設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class RebarSettings : SerializableSettings
	{
		#region
		/// <summary>ツールバー</summary>
		public RebarBandSettings ToolBar = new RebarBandSettings(0, true, true, 230);
		/// <summary>ツールバー</summary>
		public RebarBandSettings ListToolBar = new RebarBandSettings(1, true, false, 150);
		/// <summary>外部ツールバー</summary>
		public RebarBandSettings ToolsBar = new RebarBandSettings(2, true, false, 100);
		/// <summary>板ボタン</summary>
		public RebarBandSettings ItaButton = new RebarBandSettings(3, true, true, 200);
		/// <summary>アドレスバー</summary>
		public RebarBandSettings AddressBar = new RebarBandSettings(4, true, false, 300);

		/// <summary>
		/// レバーバンドの設定
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class RebarBandSettings : SerializableSettings
		{
			/// <summary></summary>
			public int Index = 0;
			/// <summary></summary>
			public bool NewRow = true;
			/// <summary></summary>
			public int Width = -1;
			/// <summary></summary>
			public bool Visible = true;

			/// <summary>
			/// RebarBandSettingsクラスのインスタンスを初期化
			/// </summary>
			/// <param name="index"></param>
			/// <param name="newRow"></param>
			/// <param name="width"></param>
			public RebarBandSettings(int index, bool visible, bool newRow, int width)
			{
				Index = index;
				Visible = visible;
				NewRow = newRow;
				Width = width;
			}

			public RebarBandSettings()
			{
			}
			public RebarBandSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		public RebarSettings()
		{
		}
		public RebarSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}

	/// <summary>
	/// 表示設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class ViewSettings : SerializableSettings
	{
		#region
		/// <summary>スレッド一覧からスレを開くと自動でスレを拡大するかどうか</summary>
		public bool AutoFillThread = false;
		/// <summary>ドッキング書き込みバーの表示状態</summary>
		public bool DockWriteBar = false;
		/// <summary>板一覧を右側にドッキング</summary>
		public bool TableDockRight = false;
		/// <summary>スレッドツールバーの表示状態</summary>
		public bool ThreadToolBar = true;
		/// <summary>板一覧の表示状態</summary>
		public bool HideTable = false;
		/// <summary>スレッド一覧の表示状態</summary>
		public bool FillList = false;
		/// <summary>スレッドの表示状態</summary>
		public bool FillThread = false;
		/// <summary>板一覧板ボタンを表示</summary>
		public bool TableItaBotan = true;
		/// <summary>ステータスバーの表示</summary>
		public bool StatusBar = true;
		/// <summary>スレッドビューのサイズ</summary>
		public Size ThreadView = Size.Empty;
		/// <summary>板一覧テーブルのサイズ</summary>
		public Size TableView = Size.Empty;
		/// <summary>スレッド一覧のサイズ</summary>
		public Size ListView = Size.Empty;
		/// <summary>タブサイズの固定</summary>
		public TabSizeMode ThreadTabSizeMode = TabSizeMode.Fixed;
		/// <summary>タブサイズの固定</summary>
		public TabSizeMode ListTabSizeMode = TabSizeMode.Fixed;
		/// <summary>タブサイズ</summary>
		public Size ListTabSize = new Size(90, 23);
		/// <summary>タブサイズ</summary>
		public Size ThreadTabSize = new Size(90, 23);
		/// <summary>ドッキング書き込みバーのサイズ</summary>
		public int DockWriteBarHeight = 80;
		/// <summary>カラムサイズ</summary>
		public ColumnSettings Columns = new ColumnSettings();
		public bool FixedRebarControl = false;
		public TabAppearance TabAppearance = TabAppearance.Buttons;

		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ColumnSettings : SerializableSettings
		{
			public int Info = 15;
			public int No = 30;
			public int Subject = 200;
			public int ResCount = 45;
			public int GotResCount = 45;
			public int NewResCount = 45;
			public int Force = 45;
			public int Size = 55;
			public int Date = 100;
			public int LastModified = 100;
			public int LastWritten = 100;
			public int BoardName = 100;

			// NTwin23.101
			public int ordInfo = 0;
			public int ordNo = 1;
			public int ordSubject = 2;
			public int ordResCount = 3;
			public int ordGotResCount = 4;
			public int ordNewResCount = 5;
			public int ordForce = 6;
			public int ordSize = 7;
			public int ordDate = 8;
			public int ordLastModified = 9;
			public int ordLastWritten = 10;
			public int ordBoardName = 11;
			// NTwin23.101

			public ColumnSettings()
			{
			}
			public ColumnSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		public ViewSettings()
		{
		}
		public ViewSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}

	/// <summary>
	/// 操作方法を表す
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class OperationSettings : SerializableSettings
	{
		#region
		/// <summary>板やスレッドを開く際の操作を表す</summary>
		public OpenMode OpenMode = OpenMode.SingleClick;
		/// <summary>タブをダブルクリックした際の動作を表す</summary>
		public TabOperation TabDoubleClick = TabOperation.Reload;
		/// <summary>タグをホイールクリックした際の動作を表す</summary>
		public TabOperation TabWheelClick = TabOperation.Close;
		/// <summary>スレッド一覧をホイールクリック時の動作</summary>
		public ListOperation ListWheelClick = ListOperation.None;

		public bool EnabledTabWheelScroll = false;

		public OperationSettings()
		{
		}
		public OperationSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}

	/// <summary>
	/// ポップアップ設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class PopupSettings : SerializableSettings
	{
		#region
		/// <summary>ポップアップする位置を表す</summary>
		public PopupPosition Position = PopupPosition.TopRight;
		/// <summary>ポップアップの表示スタイルを表す</summary>
		public PopupStyle Style = PopupStyle.Html;
		/// <summary>画像ポップアップの有効状態</summary>
		public PopupState ImagePopup = PopupState.Enable;
		/// <summary>URLポップアップの有効状態</summary>
		public PopupState UrlPopup = PopupState.Disable;
		/// <summary>ポップアップの最大サイズ</summary>
		public Size Maximum = new Size(500, 350);
		/// <summary>画像ポップアップのサイズ</summary>
		public Size ImagePopupSize = new Size(100, 100);

		/// <summary>拡張ポップアップに使用する文字列</summary>
		public string ExtendPopupStr = "&gt;|＞|＞＞";
		/// <summary>拡張ポップアップが有効かどうか</summary>
		public bool Extend = true;
		/// <summary>ポップアップするまでの経過時間 (ミリ秒単位)</summary>
		public int PopupInterval = 50;
		/// <summary>抽出ポップアップをクリックされるまで消さない</summary>
		public bool ClickedHide = true;
		/// <summary>レスポップアップをクリックされるまで消さない</summary>
		public bool ClickedHideResPopup = false;

		public PopupSettings()
		{
		}
		public PopupSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}

	/// <summary>
	/// ネット設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class NetworkSettings : SerializableSettings
	{
		#region
		/// <summary>受信バッファサイズ</summary>
		public int BufferSize = 65536;
		/// <summary>スレッド一覧の一括受信モードかどうか</summary>
		public bool ListPackageReception = true;
		/// <summary>スレッドの一括受信モードかどうか</summary>
		public bool PackageReception = true;
		/// <summary>受信に使用するプロキシ情報</summary>
		public WebProxyToCredential _RecvProxy = new WebProxyToCredential();
		/// <summary>送信に使用するプロキシ情報</summary>
		public WebProxyToCredential _SendProxy = new WebProxyToCredential();

		public IWebProxy RecvProxy
		{
			get
			{
				return _RecvProxy.GetWebProxy() != null ?
					_RecvProxy.GetWebProxy() : WebRequest.DefaultWebProxy;
			}
		}
		public IWebProxy SendProxy
		{
			get
			{
				return _SendProxy.GetWebProxy() != null ?
					_SendProxy.GetWebProxy() : WebRequest.DefaultWebProxy;
			}
		}

		public NetworkSettings()
		{
		}
		public NetworkSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}


	/// <summary>
	/// スレッド設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class ThreadSettings : SerializableSettings
	{
		#region
		/// <summary>デフォルトでオートスクロールOn</summary>
		public bool AutoScrollOn = false;
		/// <summary>デフォルトでオートリロードOn</summary>
		public bool AutoReloadOn = false;
		/// <summary>スムースなオートスクロール</summary>
		//public bool SmoothAutoScroll = false;
		/// <summary>新着までスクロールするかどうか</summary>
		public bool ScrollToNewRes = false;
		/// <summary>レス表示制限数</summary>
		public int ViewResCount = 50;
		/// <summary>レスの表示制限するかどうか</summary>
		public bool ViewResLimit = true;
		/// <summary>オートリロード間隔</summary>
		public int AutoReloadInterval = 30000;
		/// <summary>オートリロードで更新チェックのみ行うかどうか</summary>
		public bool AutoReloadCheckOnly = true;
		/// <summary>最近閉じたスレッドの保持数</summary>
		public int ClosedHistoryCount = 10;
		/// <summary>名前欄のポップアップ</summary>
		public bool NameNumberPopup = true;
		/// <summary>フォントサイズ</summary>
		public FontSize FontSize = FontSize.Medium;
		/// <summary>逆参照されているレスを色づけするかどうか</summary>
		public bool IsColoringBackReference = true;
		/// <summary></summary>
		public string BackReferenceChar = "*";
		public bool TabSelectedAfterReload = false;
		public bool UrlPopup = true;
		public bool UrlPopupOnCtrl = false;
		public bool UseAutoReloadAverage = false;

		public void CorrectAutoReloadInterval()
		{
			if (AutoReloadCheckOnly && AutoReloadInterval < 20000)
				AutoReloadInterval = 20000;
		}

		public ThreadSettings()
		{
		}
		public ThreadSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}

	/// <summary>
	/// 投稿設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class PostSettings : SerializableSettings
	{
		#region
		/// <summary>投稿ウインドウの位置</summary>
		public Point WindowLocation = Point.Empty;
		/// <summary>投稿ウインドウのサイズ</summary>
		public Size WindowSize = Size.Empty;
		/// <summary>sageにチェックを入れるかどうか</summary>
		public bool Sage = false;
		/// <summary>クッキー確認メッセージを表示するかどうか</summary>
		public bool ShowCookieDialog = true;
		/// <summary>書き込み後自動で閉じるかどうか</summary>
		public bool AutoClosing = true;
		/// <summary>書き込み履歴を残すかどうか</summary>
		public bool SavePostHistory = true;
		/// <summary>スレッドごとにコテハンを保存するかどうか</summary>
		public bool ThreadKotehan = true;
		/// <summary>samba24対策のために自主規制を行うかどうか</summary>
		public bool Samba24Check = true;
		/// <summary>書き込みウインドウを複数表示</summary>
		public bool MultiWriteDialog = true;
		/// <summary>書き込み時の日本語入力On/Off</summary>
		public bool ImeOn = false;
		/// <summary>書き込みダイアログを最小化するかどうか</summary>
		public bool MinimizingDialog = false;
		/// <summary>be2chの認証情報</summary>
		public Be2chCookie Be2chCookie = new Be2chCookie();

		public KeywordHistory NameHistory = new KeywordHistory();
		public KeywordHistory MailHistory = new KeywordHistory();

		public PostSettings()
		{
		}
		public PostSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		#endregion
	}

	/// <summary>
	/// デザイン設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class DesignSettings : SerializableSettings
	{
		#region
		/// <summary>板一覧のデザイン</summary>
		public TableDesignSettings Table = new TableDesignSettings();

		/// <summary>スレッド一覧のデザイン</summary>
		public ListDesignSettings List = new ListDesignSettings();

		/// <summary>書き込み画面のデザイン</summary>
		public PostDesignSettings Post = new PostDesignSettings();

		/// <summary>
		/// 選択中のタブを強調表示する場合の配色設定。
		/// </summary>
		public Color TabHighlightColor = Color.Empty;

		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class PostDesignSettings : SerializableSettings
		{
			public string FontName = Control.DefaultFont.FontFamily.Name;
			public int FontSize = (int)Control.DefaultFont.Size;

			public PostDesignSettings()
			{
			}
			public PostDesignSettings(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}

		/// <summary>
		/// 板一覧のデザイン設定
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class TableDesignSettings : SerializableSettings
		{
			/// <summary>スレ一覧の色分け</summary>
			public bool Coloring = false;

			/// <summary>アイコンを非表示にするかどうか</summary>
			public bool HideIcon = false;

			/// <summary>共通のフォント</summary>
			public string FontName = Control.DefaultFont.FontFamily.Name;
			/// <summary>フォントサイズを取得</summary>
			public int FontSize = (int)Control.DefaultFont.Size;

			/// <summary>カテゴリの背景色</summary>
			public Color CateBackColor = SystemColors.Window;
			/// <summary>板の背景色</summary>
			public Color BoardBackColor = SystemColors.Window;

			public TableDesignSettings()
			{
			}
			public TableDesignSettings(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}

		/// <summary>
		/// スレッド一覧のデザイン設定
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ListDesignSettings : SerializableSettings
		{
			/// <summary>スレ一覧の色分け</summary>
			public bool Coloring = true;

			/// <summary>共通のフォント</summary>
			public string FontName = Control.DefaultFont.FontFamily.Name;
			/// <summary>フォントサイズを取得</summary>
			public int FontSize = (int)Control.DefaultFont.Size;

			/// <summary>通常アイテム</summary>
			public ColorToFont Normal = new ColorToFont(SystemColors.ControlText);
			/// <summary>新着スレッド</summary>
			public ColorToFont NewThread = new ColorToFont(SystemColors.ControlText);
			/// <summary>最近立ったスレッド</summary>
			public ColorToFont RecentThread = new ColorToFont(SystemColors.ControlText);
			/// <summary>全既得ログ</summary>
			public ColorToFont GotThread = new ColorToFont(SystemColors.ControlText);
			/// <summary>更新スレッド</summary>
			public ColorToFont Update = new ColorToFont(SystemColors.ControlText);
			/// <summary>dat落ちログ</summary>
			public ColorToFont Pastlog = new ColorToFont(Color.Gray);
			/// <summary>最も勢いのあるすれ</summary>
			public ColorToFont MostForcible = new ColorToFont(Color.Red, false, true);

			/// <summary>背景色 (偶数)</summary>
			public Color BackColorFirst = SystemColors.Window;
			/// <summary>背景色 (奇数)</summary>
			public Color BackColorSecond = SystemColors.Window;

			internal void CreateFonts()
			{
				Normal.CreateFont(this);
				NewThread.CreateFont(this);
				RecentThread.CreateFont(this);
				GotThread.CreateFont(this);
				Update.CreateFont(this);
				Pastlog.CreateFont(this);
			}

			public ListDesignSettings()
			{
				CreateFonts();
			}

			public ListDesignSettings(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
				//CreateFonts();
			}
		}

		public DesignSettings()
		{
		}
		public DesignSettings(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
		#endregion
	}

	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class SoundSettings : SerializableSettings
	{
		public string Patrol = Path.Combine(Settings.SoundFolderPath, "Notify.wav");
		public string Update = Path.Combine(Settings.SoundFolderPath, "Update.wav");
		public string Error = Path.Combine(Settings.SoundFolderPath, "Error.wav");
		public string Denki = Path.Combine(Settings.SoundFolderPath, "Denki.wav");
		public string NewRes = String.Empty;

		#region
		[Category("サウンド設定")]
		[DisplayName("巡回完了")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string _Patrol
		{
			set
			{
				Patrol = value;
			}
			get
			{
				return Patrol;
			}
		}

		[Category("サウンド設定")]
		[DisplayName("スレッドの更新")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string _Update
		{
			set
			{
				Update = value;
			}
			get
			{
				return Update;
			}
		}

		[Category("サウンド設定")]
		[DisplayName("新着を取得")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string _NewRes
		{
			set
			{
				NewRes = value;
			}
			get
			{
				return NewRes;
			}
		}

		[Category("サウンド設定")]
		[DisplayName("エラー ")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string _Error
		{
			set
			{
				Error = value;
			}
			get
			{
				return Error;
			}
		}

		[Category("サウンド設定")]
		[DisplayName("印レスにレス")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string _Sirusi
		{
			set
			{
				Denki = value;
			}
			get
			{
				return Denki;
			}
		}

		public SoundSettings()
		{
		}
		public SoundSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}

	/// <summary>
	/// 色とフォントの設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class ColorToFont : SerializableSettings
	{
		/// <summary>文字色</summary>
		public Color Color = SystemColors.WindowText;

		/// <summary>フォントスタイル</summary>
		public FontStyle Style;

		[XmlIgnore]
		internal Font Font = null;

		internal void CreateFont(DesignSettings.ListDesignSettings design)
		{
			Font = new Font(design.FontName, design.FontSize, Style);
		}

		public ColorToFont(Color color) : this(color, false, false)
		{
		}
		public ColorToFont(Color color, bool bold, bool underline)
		{
			this.Color = color;
			if (bold) this.Style |= FontStyle.Bold;
			if (underline) this.Style |= FontStyle.Underline;
		}
		public ColorToFont(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class KeywordHistory : SerializableSettings
	{
		public ArrayList Keys = new ArrayList();

		public int itemIndex = -1;

		public KeywordHistory()
		{
		}
		public KeywordHistory(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public void Add(string newValue)
		{
			if (String.IsNullOrEmpty(newValue))
				return;

			for (int i = 0; i < Keys.Count; i++)
			{
				if (String.Compare((string)Keys[i], newValue, false) == 0)
				{
					return;
				}
			}

			Keys.Add(newValue);
		}
	}

	/// <summary>
	/// ポップアップの表示方法を表す列挙体
	/// </summary>
	public enum PopupStyle
	{
		/// <summary>
		/// twintail独自のポップアップ表示。
		/// 機能が制限される。外部スキンとの競合がないので標準スキンは使用出来る。
		/// </summary>
		Text,
		/// <summary>
		/// HTML表示が可能なポップアップ表示。
		/// スレッドと同じ様な表示が可能。外部スキンとの競合で上手く動作しない場合がある。
		/// </summary>
		Html,
	}

	/// <summary>
	/// 認証設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class AuthenticationSettings : SerializableSettings
	{
		#region
		/// <summary>認証を使ってログインするかどうか</summary>
		public bool AuthenticationOn = false;

		/// <summary>ユーザ名</summary>
		public string Username = String.Empty;

		/// <summary>パスワード（要暗号化）</summary>
		[TypeConverter(typeof(Base64Converter))]　　// 2010.07.10 
		public byte[] EncryptedPassword = new byte[0];

		[SoapIgnore]
		public string Password
		{
			get
			{
				return DecryptPassword(EncryptedPassword);
			}
			set
			{
				EncryptedPassword = EncryptPassword(value);
				TypeDescriptor.AddAttributes(EncryptedPassword, new TypeConverterAttribute(typeof(Base64Converter)));
			}
		}

		public AuthenticationSettings()
		{
		}
		public AuthenticationSettings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal static byte[] EncryptPassword(string password)
		{
			//byte[] encode = null; 　　　　// 2010.07.10 
			byte[] encode = new byte[0];　　// 2010.07.10 
			if (password != null && password.Length > 0)
			{
				byte[] input = Encoding.UTF8.GetBytes(password);
				MemoryStream output = new MemoryStream();
				DES des = new DESCryptoServiceProvider();
				CryptoStream crypto = new CryptoStream(output, des.CreateEncryptor(CreateKey(), CreateIV()), CryptoStreamMode.Write);
				crypto.Write(input, 0, input.Length);
				crypto.Close();
				encode = output.ToArray();
			}
			return encode;
		}

		internal static string DecryptPassword(byte[] encryptedPassword)
		{
			string password = String.Empty;
			if (encryptedPassword != null && encryptedPassword.Length > 0)
			{
				MemoryStream output = new MemoryStream();
				DES des = new DESCryptoServiceProvider();
				CryptoStream crypto = new CryptoStream(output, des.CreateDecryptor(CreateKey(), CreateIV()), CryptoStreamMode.Write);
				crypto.Write(encryptedPassword, 0, encryptedPassword.Length);
				crypto.FlushFinalBlock();
				crypto.Close();
				password = Encoding.UTF8.GetString(output.ToArray());
			}
			return password;
		}

		private static byte[] CreateKey()
		{
			return new byte[] { 0x35, 0x88, 0xbb, 0xcc, 0x52, 0x45, 0x60, 0x20 };
		}

		private static byte[] CreateIV()
		{
			return new byte[] { 0xfc, 0x79, 0x01, 0x60, 0x10, 0x20, 0xc4, 0xde };
		}
		#endregion
	}

	/// <summary>
	/// BE設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class BeSettings : AuthenticationSettings
	{
		public BeSettings()
		{
		}
		public BeSettings(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	/// <summary>
	/// BE設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class ThreadToolPanel : SerializableSettings
	{
		public Color BackColor = SystemColors.Control;
		public Color ForeColor = SystemColors.ControlText;

		public ThreadToolPanel()
		{
		}
		public ThreadToolPanel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	public enum NewTabPosition
	{
		[Description("常に先頭")]
		First,
		[Description("アクティブなタブの左")]
		CurrentLeft,
		[Description("アクティブなタブの右")]
		CurrentRight,
		[Description("常に最後")]
		Last,
	}

}
