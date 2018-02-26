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
	/// Twinie �̊T�v�̐����ł��B
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
		/// �A�v���P�[�V�����̋N���f�B���N�g�����擾
		/// </summary>
		public static readonly string StartupPath = Application.StartupPath;

		/// <summary>
		/// �o�[�W�����ԍ����擾
		/// </summary>
		public static Version Version
		{
			get
			{
				return Assembly.GetAssembly(typeof(Twinie)).GetName().Version;
			}
		}

		/// <summary>
		/// Version�v���p�e�B���e�L�X�g�ɕϊ������`�����擾
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
		/// twintail�̃t�H�[�����擾
		/// </summary>
		internal static Form Form
		{
			get
			{
				return form;
			}
		}

		/// <summary>
		/// �ꗗ�e�[�u�����擾
		/// </summary>
		internal static Twin.IBoardTable BBSTable
		{
			get
			{
				return form.BBSTable;
			}
		}

		/// <summary>
		/// �T�E���h�����擾
		/// </summary>
		internal static SoundSettings Sound
		{
			get
			{
				return settings.Sound;
			}
		}

		/// <summary>
		/// ���O�̃L���b�V�������擾
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
		/// NG���[�h�ݒ���擾
		/// </summary>
		internal static NGWordsManager NGWords
		{
			get
			{
				return nGWords;
			}
		}

		/// <summary>
		/// �ݒ�����Ǘ�����N���X���擾
		/// </summary>
		internal static Twin.Forms.Settings Settings
		{
			get
			{
				return settings;
			}
		}

		/// <summary>
		/// �V���v���ȃu���E�U���擾
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
		/// �����O�I�����܂�
		/// </summary>
		internal static void OysterLogon()
		{
			settings.Authentication.AuthenticationOn = true;
			if (X2chAuthenticator.GetInstance().HasSession == false)
				X2chAuthenticator.Enable(settings.Authentication.Username, settings.Authentication.Password);
		}

		/// <summary>
		/// �����O�A�E�g���܂�
		/// </summary>
		internal static void OysterLogout()
		{
			settings.Authentication.AuthenticationOn = false;
			X2chAuthenticator.Disable();
		}

		/// <summary>
		/// �����[�U�[���ƃp�X���[�h���L�����ǂ����𔻒f���܂�
		/// </summary>
		/// <returns></returns>
		internal static bool OysterIsValid()
		{
			return X2chAuthenticator.IsValidUsernamePassword(
				Twinie.Settings.Authentication.Username,
				Twinie.Settings.Authentication.Password);
		}

		/// <summary>
		/// �A�v���P�[�V�����̃��C�� �G���g�� �|�C���g�ł��B
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				// twintail.exe �̃f�B���N�g�����Ⴆ�Α��d�N����������
				DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath));
				
				string mutexName = Path.Combine(dir.Name, Path.GetFileNameWithoutExtension(Application.ExecutablePath));
				mutexName = mutexName.Replace('\\', '_');


				bool created;
				mutex = new Mutex(true, mutexName, out created);

				// �쐬�ɐ���
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

						// ����N�����͔ꗗ���Â���������Ȃ��̂ŁA�X�V�����߂�
						MessageBox.Show("����N�����̏ꍇ�́A�ꗗ���Â��\��������܂��̂ŁA\r\n" +
										"�t�@�C�����j���[����ꗗ���X�V���Ă��炲�g�p���������B", "���m�点", MessageBoxButtons.OK, MessageBoxIcon.Information);

						settings = new Settings();
						settings.CreateFolders();
						settings.NewCacheStruct = true; // ����N�����Ȃ�Â��L���b�V������������K�v�͂Ȃ�
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

					// ���肽���
					TwinDll.AddWriteSection = settings.AddWriteSection;
					TwinDll.AditionalAgreementField = settings.AditionalAgreementField;

					// ���΃p�X�ŕۑ�����Ă���p�X�����΃p�X�ɕϊ��B
					PathRelativeToFullPath();

					Application.ThreadException +=
						new ThreadExceptionEventHandler(OnThreadException);

					AppDomain.CurrentDomain.UnhandledException +=
						new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

					// �L���b�V������������
					Cache cache = new Cache(settings.CacheFolderPath);

					if (settings.NewCacheStruct)
					{
						cache.NewStructMode = true;
					}
					else
					{
						RemovingCache(cache);
					}

					// NG���[�h�ݒ��ǂݍ���
					nGWords = new NGWordsManager(Settings.NGWordsFolderPath);
					nGWords.Load();

					// NTwin 2011/05/31
					Twin.Tools.CookieManager.LoadCookie();

					// �t�H�[����������
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

					// NG���[�h�ݒ��ۑ�
					nGWords.Save();

					// �ꎞ�I�ɃJ�����g�f�B���N�g����exe�̑��݂���t�H���_�ɐݒ�
					string current = Directory.GetCurrentDirectory();
					Directory.SetCurrentDirectory(StartupPath);

					// twintail.exe �Ɠ����h���C�u�ɕۑ�����Ă���ꍇ�̂ݑ��΃p�X�ɕϊ����ĕۑ�
					string root1 = Path.GetPathRoot(Application.ExecutablePath);
					string root2 = Path.GetPathRoot(Settings.CacheFolderPath);

					if (root1.StartsWith(root2))
					{
						// ��΃p�X�𑊑΃p�X�ɕϊ�
						string cachef = Shlwapi.GetRelativePath(StartupPath, Settings.CacheFolderPath);
						Settings.CacheFolderPath = cachef;
					}

					// �ꉞ�J�����g�f�B���N�g�������ɖ߂�
					Directory.SetCurrentDirectory(current);

					SerializingSettings(form.Settings);

					tracer.Close();
				}
				// �N�����Ɉ������n���ꂽ�ꍇ
				else if (args.Length > 0)
				{
					mutex = null;
					NewInstance(args);
				}
				// ��d�N���h�~
				else
				{
					mutex = null;

					// �őO�ʂɕ\��
					Process process = GetPrevProcess();
					if (process != null)
						WinApi.SetForegroundWindow(process.MainWindowHandle);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "�N���G���[",
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
			if (MessageBox.Show("���O�`�����V�����Ȃ�܂����B�������O�̈ڍs�������s���܂��B\r\n"
				+ "���O�̗e�ʂɂ���Ă͏����Ɏ��Ԃ�������ꍇ������܂��B\r\n\r\n" +
				"���������s���܂����H (��������I�ԂƏ����������ɋN�����܂�)", "twintail",
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
			// �ݒ���V���A���C�Y
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

				// ����ɓǂݍ��߂��ꍇ�́A�o�b�N�A�b�v������Ă���
				backupUtil.Backup(Settings.TPath);
			}
			// �ݒ�t�@�C�������Ă���ꍇ
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
			// ���傤���Ȃ��̂ŏ�����
			settings = new Settings();
			settings.CreateFolders();

			MessageBox.Show("�ݒ�t�@�C����ǂݍ��߂܂���ł����B\r\n���ēx�A���ݒ肪�K�v�ł��B",
				"�ݒ�t�@�C�������Ă���\��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private static void PathRelativeToFullPath()
		{
			// �p�X����΃p�X�ŕۑ�����Ă���ꍇ�͉������Ȃ�
			if (Path.IsPathRooted(Settings.CacheFolderPath))
				return;

			// �ꎞ�I�ɃJ�����g�f�B���N�g����exe�̑��݂���t�H���_�ɐݒ�
			string current = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory(StartupPath);

			// ���΃p�X���΃p�X�ɕϊ�
			Settings.CacheFolderPath = Shlwapi.GetFullPath(Settings.CacheFolderPath);

			// �ꉞ�J�����g�f�B���N�g�������ɖ߂�
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
		/// �N������twintail�̃v���Z�X���擾
		/// </summary>
		/// <returns>����twintail���N�����Ă���΂��̃v���Z�X��Ԃ��B���݂��Ȃ����null</returns>
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
		/// �V�����C���X�^���X���쐬���ꂽ���Ƃ��N������twintail�ɒʒm���A������n��
		/// </summary>
		/// <param name="args"></param>
		private static void NewInstance(string[] args)
		{
			// �������P�̕�����ɕϊ�
			StringBuilder sb = new StringBuilder();
			foreach (string uri in args)
				sb.Append(uri).Append('|');

			// �Ō�̗]�v��|���폜
			sb.Remove(sb.Length - 1, 1);

			int atom = GlobalAtom.Add(sb.ToString());

			Process process = GetPrevProcess();
			if (process != null)
				WinApi.SendMessage(process.MainWindowHandle, Twin2IeBrowser.WM_NEWINSTANCE, atom, 0);
		}
	}
}
