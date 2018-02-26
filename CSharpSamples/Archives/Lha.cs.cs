// Lha.cs

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace CSharpSamples
{
	/// <summary>
	/// Lha�^���A�t�@�C�����𓀁^���k����@�\��񋟂��܂��B
	/// ���̃N���X�̓X���b�h�A���Z�[�t�ł��B
	/// </summary>
	public class Lha : IArchivable
	{
		/// <summary>
		/// ���k�ɑΉ����Ă��邩�ǂ����������l��Ԃ��܂��B
		/// ���̃v���p�e�B�͏�� true �ł��B
		/// </summary>
		public bool CanCompress {
			get {
				return true;
			}
		}

		/// <summary>
		/// �𓀂ɑΉ����Ă��邩�ǂ����������l��Ԃ��܂��B
		/// ���̃v���p�e�B�͏�� true �ł��B
		/// </summary>
		public bool CanExtract {
			get {
				return true;
			}
		}

		/// <summary>
		/// Unlha32.dll �̃o�[�W�������擾���܂��B
		/// </summary>
		public float Version {
			get {
				int ver = UnlhaGetVersion();
				return Convert.ToSingle(ver / 100);
			}
		}

		/// <summary>
		/// ���� Unlha32.dll �����쒆���ǂ����𔻒f���܂��B
		/// </summary>
		public bool IsRunning {
			get {
				int ret = UnlhaGetRunning();
				return Convert.ToBoolean(ret);
			}
		}

		/// <summary>
		/// �o�b�N�O���E���h���[�h���ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool IsBackground {
			set {
				int boolean = Convert.ToInt32(value);
				UnlhaSetBackGroundMode(boolean);
			}
			get {
				int ret = UnlhaGetBackGroundMode();
				return Convert.ToBoolean(ret);
			}
		}

		/// <summary>
		/// Unlha32.dll �̓��쒆�ɃJ�[�\����\�����郂�[�h���ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool CursorMode {
			set {
				int boolean = Convert.ToInt32(value);
				UnlhaSetCursorMode(boolean);
			}
			get {
				int ret = UnlhaGetCursorMode();
				return Convert.ToBoolean(ret);
			}
		}

		/// <summary>
		/// Lha �N���X�̃C���X�^���X�����������܂��B
		/// </summary>
		public Lha()
		{
		}

		/// <summary>
		/// Unlha �֐��𒼐ڌĂяo���܂��B
		/// </summary>
		/// <param name="format"></param>
		/// <param name="objects"></param>
		/// <returns></returns>
		public int Unlha(string format, params object[] objects)
		{
			string cmdline = String.Format(format, objects);
			return Unlha(cmdline);
		}

		/// <summary>
		/// Unlha �֐��𒼐ڌĂяo���܂��B
		/// </summary>
		/// <param name="cmdline"></param>
		/// <returns></returns>
		public int Unlha(string cmdline)
		{
			if (cmdline == null) {
				throw new ArgumentNullException("cmdline");
			}

			return Unlha(IntPtr.Zero, cmdline, null, 0);
		}

		/// <summary>
		/// �t�@�C�������k���܂��B
		/// </summary>
		/// <param name="archive">�쐬����A�[�J�C�u���B</param>
		/// <param name="fileName">���k����t�@�C�����B</param>
		/// <returns>����I���Ȃ� 0 ��Ԃ��A�ȏ�I���̏ꍇ�� 0 �ȊO�̒l��Ԃ��܂��B</returns>
		public int Compress(string archive, string fileName)
		{
			if (archive == null) {
				throw new ArgumentNullException("archive");
			}
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}

			string cmdline = String.Format("a \"{0}\" \"{1}\"", archive, fileName);

			return Unlha(IntPtr.Zero, cmdline, null, 0);
		}

		/// <summary>
		/// �����̃t�@�C�����܂Ƃ߂Ĉ��k���܂��B
		/// </summary>
		/// <param name="archive">�쐬����A�[�J�C�u���B</param>
		/// <param name="fileName">���k����t�@�C�����̔z��B</param>
		/// <returns>����I���Ȃ� 0 ��Ԃ��A�ȏ�I���̏ꍇ�� 0 �ȊO�̒l��Ԃ��܂��B</returns>
		public int Compress(string archive, string[] fileNames)
		{
			if (archive == null) {
				throw new ArgumentNullException("archive");
			}
			if (fileNames == null) {
				throw new ArgumentNullException("fileNames");
			}

			int ret = 0;

			foreach (string fileName in fileNames)
			{
				string cmdline = String.Format("a \"{0}\" \"{1}\"", archive, fileName);
				ret = Unlha(IntPtr.Zero, cmdline, null, 0);

				if (ret != 0)
					break;
			}

			return ret;
		}

		/// <summary>
		/// �A�[�J�C�u���𓀂��܂��B
		/// </summary>
		/// <param name="archive">�𓀂���A�[�J�C�u�B</param>
		/// <param name="directory">�o�̓f�B���N�g���B</param>
		/// <param name="fileName">�𓀂���t�@�C����</param>
		/// <returns>����I���Ȃ� 0 ��Ԃ��A�ȏ�I���̏ꍇ�� 0 �ȊO�̒l��Ԃ��܂��B</returns>
		public int Extract(string archive, string directory, string fileName)
		{
			if (archive == null) {
				throw new ArgumentNullException("archive");
			}
			if (directory == null) {
				throw new ArgumentNullException("directory");
			}
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}

			string cmdline = String.Format("e \"{0}\" \"{1}\" \"{2}\"",
				archive, directory, fileName);

			return Unlha(IntPtr.Zero, cmdline, null, 0);
		}

		/// <summary>
		/// �A�[�J�C�u���𓀂��܂��B
		/// </summary>
		/// <param name="archive">�𓀂���A�[�J�C�u�B</param>
		/// <param name="directory">�o�̓f�B���N�g���B</param>
		/// <returns>����I���Ȃ� 0 ��Ԃ��A�ȏ�I���̏ꍇ�� 0 �ȊO�̒l��Ԃ��B</returns>
		public int Extract(string archive, string directory)
		{
			if (archive == null) {
				throw new ArgumentNullException("archive");
			}
			if (directory == null) {
				throw new ArgumentNullException("directory");
			}

			string cmdline = String.Format("e \"{0}\" -c -y \"{1}\" *.*",
				archive, directory);

			return Unlha(IntPtr.Zero, cmdline, null, 0);
		}

		[DllImport("Unlha32.dll")]
		private static extern int UnlhaGetCursorMode();

		[DllImport("Unlha32.dll")]
		private static extern int UnlhaSetCursorMode(int bCursorMode);

		[DllImport("Unlha32.dll")]
		private static extern int UnlhaGetBackGroundMode();
		
		[DllImport("Unlha32.dll")]
		private static extern int UnlhaSetBackGroundMode(int backGroundMode);

		[DllImport("Unlha32.dll")]
		private static extern int UnlhaGetVersion();

		[DllImport("Unlha32.dll")]
		private static extern int UnlhaGetRunning();

		[DllImport("Unlha32.dll")]
		private static extern int Unlha(IntPtr hwnd, string szCmdLine, string buffer, int buffSize);

	}
}
