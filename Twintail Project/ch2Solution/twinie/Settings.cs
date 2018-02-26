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
	/// twintail�̋��ʐݒ����\��
	/// </summary>
	[Serializable]
	public class Settings : SerializableSettings
	{
		/// <summary>Web�T�C�g�ւ�URL</summary>
		public static readonly string WebSiteUrl =
			"http://www.geocities.jp/nullpo0/";

		/// <summary>�ݒ�t�@�C���̃p�X</summary>
		public static readonly string TPath =
			Path.Combine(Application.StartupPath, "twin.xml");

		public static readonly string TPathNew =
			Path.Combine(Application.StartupPath, "twin.ini");

		/// <summary>���O����ۑ�����t�@�C���p�X</summary>
		public static readonly string ErrorLogPath =
			Path.Combine(Application.StartupPath, "twin.log");

		/// <summary>2channel.brd�̑��݂���t�@�C���p�X</summary>
		public static readonly string BoardTablePath =
			Path.Combine(Application.StartupPath, "2channel.brd");

		public static readonly string ImageViewUrlReplacePath =
			Path.Combine(Application.StartupPath, "ImageViewURLReplace.dat");

		/// <summary>�T�[�o�[���׊Ď�����URL</summary>
		public static readonly string LoadFactorUrl = "http://ch2.ath.cx/";

		/// <summary>2ch�I����ȊĎ���</summary>
		public static readonly string ServerWatcherUrl = "http://www.ownerpet.com/";

		/// <summary>2ch�I�Ď��W</summary>
		public static readonly string ServerWatcher2Url = "http://sv2ch.baila6.jp/sv2ch01.html";

		/// <summary>User-Agent��\��</summary>
		public static readonly string UserAgent = TwinDll.UserAgent;

		/// <summary>�X�v���b�V���C���[�W�����݂���p�X</summary>
		public static readonly string SlpashImagePath =
			Path.Combine(Application.StartupPath, "Splash.bmp");

		#region Window Settings
		/// <summary>�ő剻����Ă��邩�ǂ���</summary>
		public bool IsMaximized = false;
		/// <summary>�E�C���h�E�̈ʒu</summary>
		public Point WindowLocation = Point.Empty;
		/// <summary>�E�C���h�E�̃T�C�Y</summary>
		public Size WindowSize = Size.Empty;
		#endregion

		#region �t�@�C���܂��̓f�B���N�g���̃p�X�֌W
		/// <summary>���[�U�[�ݒ���̑��݂���t�H���_</summary>
		public static readonly string UserFolderPath =
			Path.Combine(Application.StartupPath, "User");

		/// <summary>NG���[�h�ݒ���̑��݂���t�H���_</summary>
		public static readonly string NGWordsFolderPath =
			Path.Combine(Application.StartupPath, "NGWords");

		/// <summary>Aa�t�H���_�̑��݂���t�H���_</summary>
		public static readonly string AaFolderPath =
			Path.Combine(Application.StartupPath, "AA");

		/// <summary>�X�L�������݂����{�t�H���_�ւ̃p�X</summary>
		public static readonly string BaseSkinFolderPath =
			Path.Combine(Application.StartupPath, "Skin");

		/// <summary>�T�E���h�����݂���t�H���_�ւ̃p�X</summary>
		public static readonly string SoundFolderPath =
			Path.Combine(Application.StartupPath, "Sound");

		/// <summary>���O�ۑ��t�H���_</summary>
		public string CacheFolderPath =
			Path.Combine(Application.StartupPath, "Cache");

		/// <summary>�X�N���b�v�ۑ��t�H���_</summary>
		public static readonly string ScrapFolderPath =
			Path.Combine(Application.StartupPath, "Scrap");

		/// <summary>�o�b�N�A�b�v�ۑ��t�H���_</summary>
		public static readonly string BackupFolderPath =
			Path.Combine(Application.StartupPath, "Backup");

		/// <summary>�C���[�W�L���b�V���ۑ��t�H���_</summary>
		public static readonly string ImageCacheDirectory =
			Path.Combine(Application.StartupPath, "ImageCache");

		/// <summary>SETTING.TXT�ۑ��t�H���_</summary>
		public static readonly string SettingTxtFolderPath =
			Path.Combine(Application.StartupPath, "SETTING_TXT");

		public static readonly string MouseGestureSettingPath =
			Path.Combine(UserFolderPath, "MouseGestureSetting.xml");

		public static readonly string GroupFolderPath =
			Path.Combine(UserFolderPath, "Group");

		/// <summary>�������ݗ���ۑ��t�H���_</summary>
		public static readonly string KakikoFolderPath =
			Path.Combine(UserFolderPath, "kakiko");

		/// <summary>�A�b�v�f�[�^�ւ̃t�@�C���p�X</summary>
		public static string UpdateInfoUri =
			"http://www.geocities.co.jp/SiliconValley/5459/twinup.txt";

		/// <summary>�w���v�t�@�C���̑��݂���p�X</summary>
		public static readonly string HelpFilePath =
			Path.Combine(Application.StartupPath, "twin2help.chm");

		/// <summary>���[�U�[��`�̊O���t�@�C���̃p�X</summary>
		public static readonly string UserTablePath =
			Path.Combine(UserFolderPath, "User.brd");

		/// <summary>�O���c�[���̕ۑ��t�@�C���p�X</summary>
		public static readonly string ToolsFilePath =
			Path.Combine(UserFolderPath, "Tools.txt");

		/// <summary>�ŋߕ����X���ꗗ�t�@�C���̑��݂���p�X</summary>
		public static readonly string ClosedHistoryPath =
			Path.Combine(UserFolderPath, "Closed.txt");

		/// <summary>�ŋߏ������񂾃X���b�h�ꗗ�t�@�C���̑��݂���p�X</summary>
		public static readonly string WrittenHistoryPath =
			Path.Combine(UserFolderPath, "Written.txt");

		/// <summary>���C�ɓ���t�@�C���̑��݂���p�X</summary>
		public static readonly string BookmarkPath =
			Path.Combine(UserFolderPath, "Favorites.txt");

		/// <summary>�ߋ����O�q�Ƀt�@�C���̑��݂���p�X</summary>
		public static readonly string WarehousePath =
			Path.Combine(UserFolderPath, "Warehouse.txt");

		public static readonly string ItaBotanPath =
			Path.Combine(UserFolderPath, "Itabotan.txt");

		/// <summary>NG�A�h���X�ꗗ�t�@�C���̃p�X</summary>
		public static readonly string NGAddrsPath =
			Path.Combine(UserFolderPath, "NGAddrs.txt");

		/// <summary>���j���[�̃V���[�g�J�b�g�L�[��񂪊i�[����Ă���p�X</summary>
		public static readonly string MenuShortcutPath =
			Path.Combine(UserFolderPath, "Shortcuts2.txt");

		/// <summary>samba24�΍�̕b���\�ւ̃p�X</summary>
		public static readonly string Samba24Path =
			Path.Combine(Application.StartupPath, "samba.ini");

		/// <summary>�R�e�n���ݒ���̃p�X</summary>
		public static readonly string KotehanFilePath =
			Path.Combine(UserFolderPath, "kotehan.ini");

		public static readonly string ColorTableSettingsPath =
			Path.Combine(UserFolderPath, "ColorTableSettings.ini");

		public static readonly string ColorWordInfoSettingsPath =
			Path.Combine(UserFolderPath, "ColorWordInfoSettings.xml");

		/// <summary>�g�p����Web�u���E�U�ւ̃t�@�C���p�X</summary>
		public string WebBrowserPath = String.Empty;

		/// <summary>�ꗗ�̃I�����C���X�V��URL</summary>
		public string OnlineUpdateUrl = "http://menu.2ch.net/bbsmenu.html";

		/// <summary>�g�p����X�L���t�H���_�̖��O</summary>
		public string SkinFolderName = "default";
		#endregion


		public string AditionalAgreementField = String.Empty;
		public string AddWriteSection = String.Empty;// "&AditionalAgreementField=&kiri=tanpo";

		#region �ݒ�S��
		/// <summary>
		/// �^�u������Ƃ��Ɏ��ɑI������^�u��\��
		/// </summary>
		public TabCloseAfterSelectionMode TabCloseAfterSelection = TabCloseAfterSelectionMode.Left;

		public bool IsTasktray = false;

		/// <summary>
		/// �V�����L���b�V���\���Ɉڍs�������ǂ���
		/// </summary>
		public bool NewCacheStruct = false;

		public bool IsConvertedShortcutKeyFile = false;
		public bool ConnectionLimit = false;
		public bool ClosingConfirm = false;

		/// <summary>
		/// NG���[�h���������񂾃��X��������NGID�ɒǉ����邩�ǂ���
		/// </summary>
		public bool AutoNGRes = false;
		/// <summary>NG���[�h��On/Off</summary>
		public bool NGWordsOn = true;

		/// <summary>1���o�Ǝ�����NGID���N���A���邩�ǂ���</summary>
		public bool NGIDAutoClear = false;

		/// <summary>�Ō�ɃA�v���P�[�V�������I���������t��\��</summary>
		public int LastExitDay = 99;

		/// <summary>OverThread���ė��p���邩�ǂ���</summary>
		public bool RecycleOverThread = false;

		/// <summary>�������[�h</summary>
		public bool Livemode = false;

		/// <summary>���O�ۑ����[�h���ǂ������擾�܂��͐ݒ�</summary>
		public bool Caching = true;

		/// <summary>��ʍ\�����擾�܂��͐ݒ�</summary>
		public DisplayLayout Layout = DisplayLayout.Default;

		/// <summary>�N�����ɍX�V�`�F�b�N���邩�ǂ���</summary>
		public bool UpdateCheck = true;

		/// <summary>����I��GC.Collect�����s���邩�ǂ���</summary>
		public bool GarbageCollect = true;

		/// <summary>�X���b�h�̗D��x</summary>
		public ThreadPriority Priority = ThreadPriority.Normal;

		/// <summary>
		/// �V�K�^�u��ǉ�����ۂ̓���
		/// </summary>
		public NewTabPosition NewTabPosition = NewTabPosition.Last;

		/// <summary>�X���b�h�ꗗ����ɐV�����^�u�ŊJ�����ǂ���</summary>
		public bool ListAlwaysNewTab = false;

		/// <summary>�X���b�h����ɐV�����^�u�ŊJ�����ǂ���</summary>
		public bool ThreadAlwaysNewTab = true;

		/// <summary>�J�e�S������ɂP�����J�����ǂ���</summary>
		public bool AlwaysSingleOpen = false;

		/// <summary>�I�����C���E�I�t���C���̏�Ԃ�\��</summary>
		public bool Online = true;

		/// <summary>���O��Gzip���k���g�p���邩�ǂ���</summary>
		public bool UseGzipArchive = false;

		/// <summary>
		/// �摜�r���[�A���g�p���邩�ǂ���
		/// </summary>
		public bool ImageViewer = false;
		public bool ImageViewer_AutoOpen = false;

		/// <summary>���t�̃t�H�[�}�b�g��\��</summary>
		public string DateFormat = "yy/MM/dd HH:mm:ss";

		/// <summary>������\�����߂̒P��</summary>
		public ForceValueOf ForceValueType = ForceValueOf.Day;

		/// <summary>���X�Q�ƂɎg�p����A���J�[������</summary>
		public string ResRefAnchor = ">>";

		/// <summary>�O���c�[���̃C���f�b�N�X</summary>
		public int SelectedToolsIndex = 0;

		/// <summary>���X���`�F�b�J�[�̗L���E����</summary>
		public bool NextThreadChecker = true;
		/// <summary>
		/// ���X�������̐��x���������邩�ǂ���
		/// </summary>
		public bool NextThreadChecker_HighLevelMatch = false;

		/// <summary>���ځ[������擾</summary>
		public ABone ABone = new ABone(true, false);

		/// <summary>����N�����ɊJ��URL</summary>
		public StringCollection StartupUrls = new StringCollection();

		/// <summary>�N�����ɑO��J���Ă�����Ԃ𕜌����邩�ǂ���</summary>
		public bool OpenStartupUrls = false;

		/// <summary>�V���X���Ɣ��f�����o�ߎ���</summary>
		public TimeSpan NewThreadTimeSpan = new TimeSpan(24, 0, 0);

		/// <summary>�X���b�h��I�������ۂɎ����Ŗ{���Ƀt�H�[�J�X�����킹�邩�ǂ���</summary>
		public bool ThreadAutoFocus = false;

		public int ImageCacheClient_SizeLimit = 1024 * 1024 * 3; // 3MB

		public bool Patrol_HiddenPastlog = false;
		
		public string LastSelectedDirectoryPath = null;
		public bool RefFolderBrowserDialog_SetDefaultPathChecked = false;

		public bool EnsureVisibleBoard = true;
		public bool NG924 = false; // dat�ԍ���924�Ŏn�܂�X���b�h���X���b�h�ꗗ�ɕ\�����邩���Ȃ���

		public bool UseVisualStyle = false;	// NTwin23.102
		#endregion

		#region �e�@�\���Ƃ̐ݒ�
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
		/// �ݒ肳��Ă���X�L���t�H���_�ւ̃p�X���擾
		/// </summary>
		public string SkinFolderPath
		{
			get
			{
				return Path.Combine(BaseSkinFolderPath, SkinFolderName);
			}
		}

		/// <summary>
		/// �ݒ�ۑ��p�̃t�H���_���쐬
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
		/// �w�肵���t�@�C���Ɍ��݂̐ݒ���V���A����
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
		/// �w�肵���t�@�C������ݒ���t�V���A����
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

				// �����̐ݒ�t�@�C��
				if (xmlIn.Name == "SOAP-ENV:Envelope")
				{
					xmlIn.Close();
					xmlIn = null;

					if (File.Exists(filePath))
					{
						// �ꉞ�A�����̐ݒ���o�b�N�A�b�v���Ă���
						File.Copy(filePath, Path.ChangeExtension(filePath, ".bak"), true);
					}

					return Deserialize_Old(filePath);
				}
				// �V�����ݒ�t�@�C��
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

			// �l���V�����ݒ肷��ƁA������x�^�R���o�[�^���Z�b�g���Ȃ����K�v������
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
						// �V�����l��ݒ肵�Ă��܂����߁A�R���o�[�^����������Ă��܂��H
						field.SetValue(instanceObj, converter.ConvertFrom(node.InnerText));
					}
				}
			}
		}


		/// <summary>
		/// Settings�N���X�̃C���X�^���X��������
		/// </summary>
		public Settings()
		{
			SetCustomTypeConverter();
		}

		/// <summary>
		/// �f�V���A���C�Y���ɌĂ΂��
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public Settings(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			SetCustomTypeConverter();
		}

		public void SetCustomTypeConverter()
		{
			// ������ƃ���
			// �����œo�^�����e�C���X�^���X��V�����ʂ̃C���X�^���X�ɕύX�����ꍇ�A�V���A���C�Y�Ɏ��s���邽�߁A
			// �ēx�̃��\�b�h���ĂԕK�v������

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
	/// �_�C�A���O�ݒ�
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
	/// �����ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class SearchSettings : SerializableSettings
	{
		#region
		/// <summary>�L���b�V������</summary>
		public CacheSearchSettings CacheSearch = new CacheSearchSettings();
		/// <summary>�X���b�h�ꗗ����</summary>
		public ListSearchSettings ListSearch = new ListSearchSettings();
		/// <summary>�X���b�h����</summary>
		public ThreadSearchSettings ThreadSearch = new ThreadSearchSettings();
		/// <summary>���X���o</summary>
		public ResExtractSettings ResExtract = new ResExtractSettings();

		public KeywordHistory SearchHistory = new KeywordHistory();

		/// <summary>
		/// �L���b�V�������ݒ�
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class CacheSearchSettings : SerializableSettings
		{
			/// <summary>�����L�[���[�h����</summary>
			public string Keyword = String.Empty;
			/// <summary>�����Ώ�</summary>
			public CacheSearchTarget Target = CacheSearchTarget.Body;
			/// <summary>�����Ώۂ̔�</summary>
			public BoardInfoCollection SelectedBoards = new BoardInfoCollection();

			public CacheSearchSettings()
			{
			}
			public CacheSearchSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		/// <summary>
		/// �X���b�h�ꗗ�����ݒ�
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ListSearchSettings : SerializableSettings
		{
			/// <summary>�����L�[���[�h����</summary>
			//public StringCollection Keywords = new StringCollection();
			public string Keyword = String.Empty;
			/// <summary>�����I�v�V���� </summary>
			public SearchOptions Options = SearchOptions.None;
			/// <summary>�C���N�������^���T�[�`</summary>
			public bool IncrementalSearch = false;

			public ListSearchSettings()
			{
			}
			public ListSearchSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		/// <summary>
		/// �X���b�h�����ݒ�
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ThreadSearchSettings : SerializableSettings
		{
			/// <summary>�����L�[���[�h����</summary>
			//public StringCollection Keywords = new StringCollection();
			public string Keyword = String.Empty;
			/// <summary>�����I�v�V���� </summary>
			public SearchOptions Options = SearchOptions.None;

			public ThreadSearchSettings()
			{
			}
			public ThreadSearchSettings(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		/// <summary>
		/// ���X���o�����ݒ�
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ResExtractSettings : SerializableSettings
		{
			/// <summary>�����L�[���[�h����</summary>
			//public StringCollection Keywords = new StringCollection();
			public string Keyword = String.Empty;
			/// <summary>�����I�v�V���� </summary>
			public SearchOptions Options = SearchOptions.None;
			/// <summary>�����Ώ�</summary>
			public ResSetElement SearchTarget = ResSetElement.All;
			/// <summary>�|�b�v�A�b�v�\��</summary>
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
	/// ���o�[�R���g���[���ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class RebarSettings : SerializableSettings
	{
		#region
		/// <summary>�c�[���o�[</summary>
		public RebarBandSettings ToolBar = new RebarBandSettings(0, true, true, 230);
		/// <summary>�c�[���o�[</summary>
		public RebarBandSettings ListToolBar = new RebarBandSettings(1, true, false, 150);
		/// <summary>�O���c�[���o�[</summary>
		public RebarBandSettings ToolsBar = new RebarBandSettings(2, true, false, 100);
		/// <summary>�{�^��</summary>
		public RebarBandSettings ItaButton = new RebarBandSettings(3, true, true, 200);
		/// <summary>�A�h���X�o�[</summary>
		public RebarBandSettings AddressBar = new RebarBandSettings(4, true, false, 300);

		/// <summary>
		/// ���o�[�o���h�̐ݒ�
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
			/// RebarBandSettings�N���X�̃C���X�^���X��������
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
	/// �\���ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class ViewSettings : SerializableSettings
	{
		#region
		/// <summary>�X���b�h�ꗗ����X�����J���Ǝ����ŃX�����g�傷�邩�ǂ���</summary>
		public bool AutoFillThread = false;
		/// <summary>�h�b�L���O�������݃o�[�̕\�����</summary>
		public bool DockWriteBar = false;
		/// <summary>�ꗗ���E���Ƀh�b�L���O</summary>
		public bool TableDockRight = false;
		/// <summary>�X���b�h�c�[���o�[�̕\�����</summary>
		public bool ThreadToolBar = true;
		/// <summary>�ꗗ�̕\�����</summary>
		public bool HideTable = false;
		/// <summary>�X���b�h�ꗗ�̕\�����</summary>
		public bool FillList = false;
		/// <summary>�X���b�h�̕\�����</summary>
		public bool FillThread = false;
		/// <summary>�ꗗ�{�^����\��</summary>
		public bool TableItaBotan = true;
		/// <summary>�X�e�[�^�X�o�[�̕\��</summary>
		public bool StatusBar = true;
		/// <summary>�X���b�h�r���[�̃T�C�Y</summary>
		public Size ThreadView = Size.Empty;
		/// <summary>�ꗗ�e�[�u���̃T�C�Y</summary>
		public Size TableView = Size.Empty;
		/// <summary>�X���b�h�ꗗ�̃T�C�Y</summary>
		public Size ListView = Size.Empty;
		/// <summary>�^�u�T�C�Y�̌Œ�</summary>
		public TabSizeMode ThreadTabSizeMode = TabSizeMode.Fixed;
		/// <summary>�^�u�T�C�Y�̌Œ�</summary>
		public TabSizeMode ListTabSizeMode = TabSizeMode.Fixed;
		/// <summary>�^�u�T�C�Y</summary>
		public Size ListTabSize = new Size(90, 23);
		/// <summary>�^�u�T�C�Y</summary>
		public Size ThreadTabSize = new Size(90, 23);
		/// <summary>�h�b�L���O�������݃o�[�̃T�C�Y</summary>
		public int DockWriteBarHeight = 80;
		/// <summary>�J�����T�C�Y</summary>
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
	/// ������@��\��
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class OperationSettings : SerializableSettings
	{
		#region
		/// <summary>��X���b�h���J���ۂ̑����\��</summary>
		public OpenMode OpenMode = OpenMode.SingleClick;
		/// <summary>�^�u���_�u���N���b�N�����ۂ̓����\��</summary>
		public TabOperation TabDoubleClick = TabOperation.Reload;
		/// <summary>�^�O���z�C�[���N���b�N�����ۂ̓����\��</summary>
		public TabOperation TabWheelClick = TabOperation.Close;
		/// <summary>�X���b�h�ꗗ���z�C�[���N���b�N���̓���</summary>
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
	/// �|�b�v�A�b�v�ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class PopupSettings : SerializableSettings
	{
		#region
		/// <summary>�|�b�v�A�b�v����ʒu��\��</summary>
		public PopupPosition Position = PopupPosition.TopRight;
		/// <summary>�|�b�v�A�b�v�̕\���X�^�C����\��</summary>
		public PopupStyle Style = PopupStyle.Html;
		/// <summary>�摜�|�b�v�A�b�v�̗L�����</summary>
		public PopupState ImagePopup = PopupState.Enable;
		/// <summary>URL�|�b�v�A�b�v�̗L�����</summary>
		public PopupState UrlPopup = PopupState.Disable;
		/// <summary>�|�b�v�A�b�v�̍ő�T�C�Y</summary>
		public Size Maximum = new Size(500, 350);
		/// <summary>�摜�|�b�v�A�b�v�̃T�C�Y</summary>
		public Size ImagePopupSize = new Size(100, 100);

		/// <summary>�g���|�b�v�A�b�v�Ɏg�p���镶����</summary>
		public string ExtendPopupStr = "&gt;|��|����";
		/// <summary>�g���|�b�v�A�b�v���L�����ǂ���</summary>
		public bool Extend = true;
		/// <summary>�|�b�v�A�b�v����܂ł̌o�ߎ��� (�~���b�P��)</summary>
		public int PopupInterval = 50;
		/// <summary>���o�|�b�v�A�b�v���N���b�N�����܂ŏ����Ȃ�</summary>
		public bool ClickedHide = true;
		/// <summary>���X�|�b�v�A�b�v���N���b�N�����܂ŏ����Ȃ�</summary>
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
	/// �l�b�g�ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class NetworkSettings : SerializableSettings
	{
		#region
		/// <summary>��M�o�b�t�@�T�C�Y</summary>
		public int BufferSize = 65536;
		/// <summary>�X���b�h�ꗗ�̈ꊇ��M���[�h���ǂ���</summary>
		public bool ListPackageReception = true;
		/// <summary>�X���b�h�̈ꊇ��M���[�h���ǂ���</summary>
		public bool PackageReception = true;
		/// <summary>��M�Ɏg�p����v���L�V���</summary>
		public WebProxyToCredential _RecvProxy = new WebProxyToCredential();
		/// <summary>���M�Ɏg�p����v���L�V���</summary>
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
	/// �X���b�h�ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class ThreadSettings : SerializableSettings
	{
		#region
		/// <summary>�f�t�H���g�ŃI�[�g�X�N���[��On</summary>
		public bool AutoScrollOn = false;
		/// <summary>�f�t�H���g�ŃI�[�g�����[�hOn</summary>
		public bool AutoReloadOn = false;
		/// <summary>�X���[�X�ȃI�[�g�X�N���[��</summary>
		//public bool SmoothAutoScroll = false;
		/// <summary>�V���܂ŃX�N���[�����邩�ǂ���</summary>
		public bool ScrollToNewRes = false;
		/// <summary>���X�\��������</summary>
		public int ViewResCount = 50;
		/// <summary>���X�̕\���������邩�ǂ���</summary>
		public bool ViewResLimit = true;
		/// <summary>�I�[�g�����[�h�Ԋu</summary>
		public int AutoReloadInterval = 30000;
		/// <summary>�I�[�g�����[�h�ōX�V�`�F�b�N�̂ݍs�����ǂ���</summary>
		public bool AutoReloadCheckOnly = true;
		/// <summary>�ŋߕ����X���b�h�̕ێ���</summary>
		public int ClosedHistoryCount = 10;
		/// <summary>���O���̃|�b�v�A�b�v</summary>
		public bool NameNumberPopup = true;
		/// <summary>�t�H���g�T�C�Y</summary>
		public FontSize FontSize = FontSize.Medium;
		/// <summary>�t�Q�Ƃ���Ă��郌�X��F�Â����邩�ǂ���</summary>
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
	/// ���e�ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class PostSettings : SerializableSettings
	{
		#region
		/// <summary>���e�E�C���h�E�̈ʒu</summary>
		public Point WindowLocation = Point.Empty;
		/// <summary>���e�E�C���h�E�̃T�C�Y</summary>
		public Size WindowSize = Size.Empty;
		/// <summary>sage�Ƀ`�F�b�N�����邩�ǂ���</summary>
		public bool Sage = false;
		/// <summary>�N�b�L�[�m�F���b�Z�[�W��\�����邩�ǂ���</summary>
		public bool ShowCookieDialog = true;
		/// <summary>�������݌㎩���ŕ��邩�ǂ���</summary>
		public bool AutoClosing = true;
		/// <summary>�������ݗ������c�����ǂ���</summary>
		public bool SavePostHistory = true;
		/// <summary>�X���b�h���ƂɃR�e�n����ۑ����邩�ǂ���</summary>
		public bool ThreadKotehan = true;
		/// <summary>samba24�΍�̂��߂Ɏ���K�����s�����ǂ���</summary>
		public bool Samba24Check = true;
		/// <summary>�������݃E�C���h�E�𕡐��\��</summary>
		public bool MultiWriteDialog = true;
		/// <summary>�������ݎ��̓��{�����On/Off</summary>
		public bool ImeOn = false;
		/// <summary>�������݃_�C�A���O���ŏ������邩�ǂ���</summary>
		public bool MinimizingDialog = false;
		/// <summary>be2ch�̔F�؏��</summary>
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
	/// �f�U�C���ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class DesignSettings : SerializableSettings
	{
		#region
		/// <summary>�ꗗ�̃f�U�C��</summary>
		public TableDesignSettings Table = new TableDesignSettings();

		/// <summary>�X���b�h�ꗗ�̃f�U�C��</summary>
		public ListDesignSettings List = new ListDesignSettings();

		/// <summary>�������݉�ʂ̃f�U�C��</summary>
		public PostDesignSettings Post = new PostDesignSettings();

		/// <summary>
		/// �I�𒆂̃^�u�������\������ꍇ�̔z�F�ݒ�B
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
		/// �ꗗ�̃f�U�C���ݒ�
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class TableDesignSettings : SerializableSettings
		{
			/// <summary>�X���ꗗ�̐F����</summary>
			public bool Coloring = false;

			/// <summary>�A�C�R�����\���ɂ��邩�ǂ���</summary>
			public bool HideIcon = false;

			/// <summary>���ʂ̃t�H���g</summary>
			public string FontName = Control.DefaultFont.FontFamily.Name;
			/// <summary>�t�H���g�T�C�Y���擾</summary>
			public int FontSize = (int)Control.DefaultFont.Size;

			/// <summary>�J�e�S���̔w�i�F</summary>
			public Color CateBackColor = SystemColors.Window;
			/// <summary>�̔w�i�F</summary>
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
		/// �X���b�h�ꗗ�̃f�U�C���ݒ�
		/// </summary>
		[Serializable]
		[TypeConverter(typeof(TwinExpandableConverter))]
		public class ListDesignSettings : SerializableSettings
		{
			/// <summary>�X���ꗗ�̐F����</summary>
			public bool Coloring = true;

			/// <summary>���ʂ̃t�H���g</summary>
			public string FontName = Control.DefaultFont.FontFamily.Name;
			/// <summary>�t�H���g�T�C�Y���擾</summary>
			public int FontSize = (int)Control.DefaultFont.Size;

			/// <summary>�ʏ�A�C�e��</summary>
			public ColorToFont Normal = new ColorToFont(SystemColors.ControlText);
			/// <summary>�V���X���b�h</summary>
			public ColorToFont NewThread = new ColorToFont(SystemColors.ControlText);
			/// <summary>�ŋߗ������X���b�h</summary>
			public ColorToFont RecentThread = new ColorToFont(SystemColors.ControlText);
			/// <summary>�S�������O</summary>
			public ColorToFont GotThread = new ColorToFont(SystemColors.ControlText);
			/// <summary>�X�V�X���b�h</summary>
			public ColorToFont Update = new ColorToFont(SystemColors.ControlText);
			/// <summary>dat�������O</summary>
			public ColorToFont Pastlog = new ColorToFont(Color.Gray);
			/// <summary>�ł������̂��邷��</summary>
			public ColorToFont MostForcible = new ColorToFont(Color.Red, false, true);

			/// <summary>�w�i�F (����)</summary>
			public Color BackColorFirst = SystemColors.Window;
			/// <summary>�w�i�F (�)</summary>
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
		[Category("�T�E���h�ݒ�")]
		[DisplayName("���񊮗�")]
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

		[Category("�T�E���h�ݒ�")]
		[DisplayName("�X���b�h�̍X�V")]
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

		[Category("�T�E���h�ݒ�")]
		[DisplayName("�V�����擾")]
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

		[Category("�T�E���h�ݒ�")]
		[DisplayName("�G���[ ")]
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

		[Category("�T�E���h�ݒ�")]
		[DisplayName("�󃌃X�Ƀ��X")]
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
	/// �F�ƃt�H���g�̐ݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class ColorToFont : SerializableSettings
	{
		/// <summary>�����F</summary>
		public Color Color = SystemColors.WindowText;

		/// <summary>�t�H���g�X�^�C��</summary>
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
	/// �|�b�v�A�b�v�̕\�����@��\���񋓑�
	/// </summary>
	public enum PopupStyle
	{
		/// <summary>
		/// twintail�Ǝ��̃|�b�v�A�b�v�\���B
		/// �@�\�����������B�O���X�L���Ƃ̋������Ȃ��̂ŕW���X�L���͎g�p�o����B
		/// </summary>
		Text,
		/// <summary>
		/// HTML�\�����\�ȃ|�b�v�A�b�v�\���B
		/// �X���b�h�Ɠ����l�ȕ\�����\�B�O���X�L���Ƃ̋����ŏ�肭���삵�Ȃ��ꍇ������B
		/// </summary>
		Html,
	}

	/// <summary>
	/// �F�ؐݒ�
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class AuthenticationSettings : SerializableSettings
	{
		#region
		/// <summary>�F�؂��g���ă��O�C�����邩�ǂ���</summary>
		public bool AuthenticationOn = false;

		/// <summary>���[�U��</summary>
		public string Username = String.Empty;

		/// <summary>�p�X���[�h�i�v�Í����j</summary>
		[TypeConverter(typeof(Base64Converter))]�@�@// 2010.07.10 
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
			//byte[] encode = null; �@�@�@�@// 2010.07.10 
			byte[] encode = new byte[0];�@�@// 2010.07.10 
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
	/// BE�ݒ�
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
	/// BE�ݒ�
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
		[Description("��ɐ擪")]
		First,
		[Description("�A�N�e�B�u�ȃ^�u�̍�")]
		CurrentLeft,
		[Description("�A�N�e�B�u�ȃ^�u�̉E")]
		CurrentRight,
		[Description("��ɍŌ�")]
		Last,
	}

}
