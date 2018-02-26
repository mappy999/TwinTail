// Twinie.cs

namespace Twin.Forms
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Windows.Forms;
	using System.Diagnostics;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Runtime.InteropServices;
	using System.ComponentModel;
	using System.Text;
	using CSharpSamples.Winapi;
	using Twin.Util;
	using System.Xml;
using Twin.Bbs;

	/// <summary>
	/// Twinie の概要の説明です。
	/// </summary>
	public class Twinie
	{
		private static bool ShowSplash = false;

		private static Twin2IeBrowser form;
		private static SimpleWebBrowser simplweb;

		private static NGWordsManager nGWords;

		private static TraceOutput tracer;
		private static SplashWindow splash;
		private static Settings settings;
		private static BackupUtil backupUtil;
		private static Mutex mutex = null;

		/// <summary>
		/// アプリケーションの起動ディレクトリを取得
		/// </summary>
		public static readonly string StartupPath = Application.StartupPath;

		/// <summary>
		/// バージョン番号を取得
		/// </summary>
		public static Version Version
		{
			get
			{
				return Assembly.GetAssembly(typeof(Twinie)).GetName().Version;
			}
		}

		/// <summary>
		/// Versionプロパティをテキストに変換した形式を取得
		/// </summary>
		public static string VersionText
		{
			get
			{
				string ver = String.Format("twintail ver{0}", Version);
#if DEBUG
				ver += " DEBUG";
#endif

				//if (Version.Build > 0)
				//    ver += String.Format(" build{1}", Version.Build);

				return ver;
			}
		}

		/// <summary>
		/// twintailのフォームを取得
		/// </summary>
		internal static Form Form
		{
			get
			{
				return form;
			}
		}

		/// <summary>
		/// 板一覧テーブルを取得
		/// </summary>
		internal static Twin.IBoardTable BBSTable
		{
			get
			{
				return form.BBSTable;
			}
		}

		/// <summary>
		/// サウンド情報を取得
		/// </summary>
		internal static SoundSettings Sound
		{
			get
			{
				return settings.Sound;
			}
		}

		/// <summary>
		/// ログのキャッシュ情報を取得
		/// </summary>
		internal static Twin.Cache Cache
		{
			get
			{
				return form.Cache;
			}
		}

		internal static BackupUtil BackupUtil
		{
			get
			{
				return backupUtil;
			}
		}

		/// <summary>
		/// NGワード設定を取得
		/// </summary>
		internal static NGWordsManager NGWords
		{
			get
			{
				return nGWords;
			}
		}

		/// <summary>
		/// 設定情報を管理するクラスを取得
		/// </summary>
		internal static Twin.Forms.Settings Settings
		{
			get
			{
				return settings;
			}
		}

		/// <summary>
		/// シンプルなブラウザを取得
		/// </summary>
		internal static Twin.Forms.SimpleWebBrowser SimpleWeb
		{
			get
			{
				if (simplweb == null)
				{
					simplweb = new SimpleWebBrowser();
					simplweb.Closing += new CancelEventHandler(SimpleWeb_Closing);
					simplweb.Owner = form;
				}
				return simplweb;
			}
		}

		/// <summary>
		/// ●ログオンします
		/// </summary>
		internal static void OysterLogon()
		{
			settings.Authentication.AuthenticationOn = true;
			if (X2chAuthenticator.GetInstance().HasSession == false)
				X2chAuthenticator.Enable(settings.Authentication.Username, settings.Authentication.Password);
		}

		/// <summary>
		/// ●ログアウトします
		/// </summary>
		internal static void OysterLogout()
		{
			settings.Authentication.AuthenticationOn = false;
			X2chAuthenticator.Disable();
		}

		/// <summary>
		/// ●ユーザー名とパスワードが有効かどうかを判断します
		/// </summary>
		/// <returns></returns>
		internal static bool OysterIsValid()
		{
			return X2chAuthenticator.IsValidUsernamePassword(
				Twinie.Settings.Authentication.Username,
				Twinie.Settings.Authentication.Password);
		}

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				// twintail.exe のディレクトリが違えば多重起動を許可する
				DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath));
				
				string mutexName = Path.Combine(dir.Name, Path.GetFileNameWithoutExtension(Application.ExecutablePath));
				mutexName = mutexName.Replace('\\', '_');


				bool created;
				mutex = new Mutex(true, mutexName, out created);

				// 作成に成功
				if (created)
				{
					//Application.EnableVisualStyles();
					//Application.SetCompatibleTextRenderingDefault(false);

					Control.CheckForIllegalCrossThreadCalls = true;

					tracer = new TraceOutput(Settings.ErrorLogPath);
					backupUtil = new BackupUtil(Settings.BackupFolderPath);

					if (File.Exists(Settings.TPath))
					{
						LoadXmlSettings();
					}
					else
					{

						// 初回起動時は板一覧が古いかもしれないので、更新を奨める
						MessageBox.Show("初回起動時の場合は、板一覧が古い可能性がありますので、\r\n" +
										"ファイルメニューから板一覧を更新してからご使用ください。", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Information);

						settings = new Settings();
						settings.CreateFolders();
						settings.NewCacheStruct = true; // 初回起動時なら古いキャッシュを処理する必要はない
					}

					// NTwin23.102
					if (settings.UseVisualStyle)
					{
						Application.EnableVisualStyles();
						Application.SetCompatibleTextRenderingDefault(false);
					}

					if (ShowSplash)
					{
						splash = new SplashWindow(Settings.SlpashImagePath);
						splash.Show();
						Application.DoEvents();
					}

					// きりたんぽ
					TwinDll.AddWriteSection = settings.AddWriteSection;
					TwinDll.AditionalAgreementField = settings.AditionalAgreementField;

					// 相対パスで保存されているパス情報を絶対パスに変換。
					PathRelativeToFullPath();

					Application.ThreadException +=
						new ThreadExceptionEventHandler(OnThreadException);

					AppDomain.CurrentDomain.UnhandledException +=
						new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

					// キャッシュ情報を初期化
					Cache cache = new Cache(settings.CacheFolderPath);

					if (settings.NewCacheStruct)
					{
						cache.NewStructMode = true;
					}
					else
					{
						RemovingCache(cache);
					}

					// NGワード設定を読み込む
					nGWords = new NGWordsManager(Settings.NGWordsFolderPath);
					nGWords.Load();

					// NTwin 2011/05/31
					Twin.Tools.CookieManager.LoadCookie();

					// フォームを初期化
					form = new Twin2IeBrowser(cache, settings, args);
					form.Loaded += new EventHandler(OnLoaded);
					form.Closed += new EventHandler(OnClosed);
					form.Text = VersionText;

					Application.Run(form);

					/*
					StreamWriter s = new StreamWriter(Settings.TPath, false, System.Text.Encoding.Default);
					s.Close();
					*/

					// NTwin 2011/05/31
					Twin.Tools.CookieManager.SaveCookie();

					// NGワード設定を保存
					nGWords.Save();

					// 一時的にカレントディレクトリをexeの存在するフォルダに設定
					string current = Directory.GetCurrentDirectory();
					Directory.SetCurrentDirectory(StartupPath);

					// twintail.exe と同じドライブに保存されている場合のみ相対パスに変換して保存
					string root1 = Path.GetPathRoot(Application.ExecutablePath);
					string root2 = Path.GetPathRoot(Settings.CacheFolderPath);

					if (root1.StartsWith(root2))
					{
						// 絶対パスを相対パスに変換
						string cachef = Shlwapi.GetRelativePath(StartupPath, Settings.CacheFolderPath);
						Settings.CacheFolderPath = cachef;
					}

					// 一応カレントディレクトリを元に戻す
					Directory.SetCurrentDirectory(current);

					SerializingSettings(form.Settings);

					tracer.Close();
				}
				// 起動中に引数が渡された場合
				else if (args.Length > 0)
				{
					mutex = null;
					NewInstance(args);
				}
				// 二重起動防止
				else
				{
					mutex = null;

					// 最前面に表示
					Process process = GetPrevProcess();
					if (process != null)
						WinApi.SetForegroundWindow(process.MainWindowHandle);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "起動エラー",
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				if (mutex != null)
					mutex.ReleaseMutex();
			}
		}

		private static void RemovingCache(Cache cache)
		{
			if (MessageBox.Show("ログ形式が新しくなりました。既得ログの移行処理を行います。\r\n"
				+ "ログの容量によっては処理に時間がかかる場合があります。\r\n\r\n" +
				"処理を実行しますか？ (いいえを選ぶと処理をせずに起動します)", "twintail",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				CacheRemovingTool tool = new CacheRemovingTool(cache);
				tool.ShowDialog();

				cache.NewStructMode = 
					settings.NewCacheStruct = !tool.IsCancelled;
			}
			else
			{
				cache.NewStructMode = false;
				settings.NewCacheStruct = false;
			}
		}

		public static void SerializingSettings(Settings settings)
		{
			// 設定をシリアライズ
			Settings.Serialize(Settings.TPath, settings);
		}

		private static void LoadXmlSettings()
		{
			bool retried = false;

			Stopwatch sw = new Stopwatch();

			sw.Start();
Retry:
			try
			{
				settings = Settings.Deserialize(Settings.TPath);
				settings.CreateFolders();

				sw.Stop();

//				MessageBox.Show(sw.ElapsedMilliseconds.ToString());

				// 正常に読み込めた場合は、バックアップを取っておく
				backupUtil.Backup(Settings.TPath);
			}
			// 設定ファイルが壊れている場合
			catch (Exception ex)
			{
				if (ex is SerializationException ||
					ex is XmlException)
				{
					if (!retried && backupUtil.Restore(Settings.TPath))
					{
						retried = true;
						goto Retry;
					}
					else
					{
						SettingsLoadError();
					}
				}
				else
					throw ex;
			}

		}

		private static void SettingsLoadError()
		{
			// しょうがないので初期化
			settings = new Settings();
			settings.CreateFolders();

			MessageBox.Show("設定ファイルを読み込めませんでした。\r\n※再度、環境設定が必要です。",
				"設定ファイルが壊れている予感", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private static void PathRelativeToFullPath()
		{
			// パスが絶対パスで保存されている場合は何もしない
			if (Path.IsPathRooted(Settings.CacheFolderPath))
				return;

			// 一時的にカレントディレクトリをexeの存在するフォルダに設定
			string current = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory(StartupPath);

			// 相対パスを絶対パスに変換
			Settings.CacheFolderPath = Shlwapi.GetFullPath(Settings.CacheFolderPath);

			// 一応カレントディレクトリを元に戻す
			Directory.SetCurrentDirectory(current);
		}

		private static void OnLoaded(object sender, EventArgs e)
		{
			if (splash != null)
			{
				splash.Hide();
				splash = null;
			}
		}

		private static void OnClosed(object sender, EventArgs e)
		{
			if (simplweb != null)
			{
				simplweb.Closing -= new CancelEventHandler(SimpleWeb_Closing);
				simplweb.Close();
			}
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			TwinDll.ShowOutput(e.ExceptionObject.ToString());
		}

		private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			TwinDll.ShowOutput(e.Exception);
		}

		private static void SimpleWeb_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			simplweb.CloseAll();
			simplweb.Hide();
		}

		/// <summary>
		/// 起動中のtwintailのプロセスを取得
		/// </summary>
		/// <returns>既にtwintailが起動していればそのプロセスを返す。存在しなければnull</returns>
		private static Process GetPrevProcess()
		{
			Process this_ = Process.GetCurrentProcess();
			Process[] array = Process.GetProcessesByName(this_.ProcessName);

			foreach (Process p in array)
			{
				if (this_.Id != p.Id)
				{
					return p;
				}
			}
			return null;
		}

		/// <summary>
		/// 新しいインスタンスが作成されたことを起動中のtwintailに通知し、引数を渡す
		/// </summary>
		/// <param name="args"></param>
		private static void NewInstance(string[] args)
		{
			// 引数を１つの文字列に変換
			StringBuilder sb = new StringBuilder();
			foreach (string uri in args)
				sb.Append(uri).Append('|');

			// 最後の余計な|を削除
			sb.Remove(sb.Length - 1, 1);

			int atom = GlobalAtom.Add(sb.ToString());

			Process process = GetPrevProcess();
			if (process != null)
				WinApi.SendMessage(process.MainWindowHandle, Twin2IeBrowser.WM_NEWINSTANCE, atom, 0);
		}
	}
}
