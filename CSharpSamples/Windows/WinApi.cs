// WinApi.cs

namespace CSharpSamples.Winapi
{
	using System;
	using System.Runtime.InteropServices;
	using System.Drawing;
	using System.Diagnostics;
	using System.Text;
	using System.IO;

	/// <summary>
	/// shlwapi.dll��Path�n�֐��Q
	/// �Q�lURL: http://nienie.com/~masapico/
	/// </summary>
	public class Shlwapi
	{
		/// <summary>
		/// fromPath����toPath�ւ̑��΃p�X���擾
		/// </summary>
		/// <param name="fromPath">�Q�ƌ��̃p�X</param>
		/// <param name="toPath">�Q�Ɛ�̃p�X</param>
		/// <returns>���΃p�X (�G���[�ł����null��Ԃ�)</returns>
		public static string GetRelativePath(
			string fromPath, string toPath)
		{
			StringBuilder sb = new StringBuilder(512);
			FileAttributes attrfrom = File.GetAttributes(fromPath);
			FileAttributes attrto = File.GetAttributes(toPath);

			if (PathRelativePathTo(sb, fromPath, (uint)attrfrom, toPath, (uint)attrto) != 0)
				return sb.ToString();

			// ���s
			return null;
		}

		/// <summary>
		/// ���������w�肵�ăp�X���k�߂�
		/// </summary>
		/// <param name="path">�k�߂�p�X</param>
		/// <param name="count">�������ɏk�߂邩���w��</param>
		/// <returns>count�����ɏk�߂�ꂽ�p�X</returns>
		public static string GetCompactPath(string path, int count)
		{
			StringBuilder sb = new StringBuilder(512);

			if (PathCompactPathEx(sb, path, (uint)count, '\\') != 0)
				return sb.ToString();

			return null;
		}

		/// <summary>
		/// ..\ �Ȃǂ��܂ރp�X�̕ϊ�
		/// </summary>
		/// <param name="path">�ϊ��Ώۂ̃p�X</param>
		/// <returns>�ϊ���̃p�X</returns>
		public static string Canonicalize(string path)
		{
			StringBuilder sb = new StringBuilder(512);

			if (PathCanonicalize(sb, path) != 0)
				return sb.ToString();

			return null;
		}

		/// <summary>
		/// path�̃t���p�X�����߂�
		/// </summary>
		/// <param name="path">�t���p�X�����߂�p�X</param>
		/// <returns>���߂��t���p�X</returns>
		public static string GetFullPath(string path)
		{
			StringBuilder sb = new StringBuilder(512);

			if (PathSearchAndQualify(path, sb, 512) != 0)
				return sb.ToString();

			return null;
		}

		[DllImport("shlwapi.dll")]
		private static extern int PathRelativePathTo(
			StringBuilder sb, string from, uint attrfrom, string to, uint attrto);

		[DllImport("shlwapi.dll")]
		private static extern int PathCompactPathEx(
			StringBuilder dest, string src, uint count, ulong flags);

		[DllImport("shlwapi.dll")]
		private static extern int PathCanonicalize(StringBuilder dest, string src);

		[DllImport("shlwapi.dll")]
		private static extern int PathSearchAndQualify(
			string path, StringBuilder fullyQualifiedPath, uint fullyQualifiedPathSize);
	}

	/// <summary>
	/// WinAPI �̊T�v�̐����ł��B
	/// </summary>
	public class WinApi
	{		
		/// <summary>�A�v���P�[�V�����p��`���b�Z�[�W</summary>
		public const int WM_APP = 0x8000;

		[DllImport("user32.dll")]
		public static extern int SetForegroundWindow(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern int IsIconic(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern int OpenIcon(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern int MessageBeep(uint type);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hwnd, uint uMsg, uint wParam, long lParam);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hwnd, uint uMsg, int wParam, long lParam);

		[DllImport("user32.dll")]
		public static extern long GetWindowLong(IntPtr hwnd, int index);

		[DllImport("user32.dll")]
		public static extern long SetWindowLong(IntPtr hwnd, int index, ulong newLong);

		[DllImport("kernel32.dll")]
		public static extern int GlobalFindAtom(String atomString);

		[DllImport("kernel32.dll")]
		public static extern int GlobalAddAtom(String atomString);

		[DllImport("kernel32.dll")]
		public static extern int GlobalDeleteAtom(int atom);

		[DllImport("kernel32.dll")]
		public static extern int GlobalGetAtomName(int atom, StringBuilder buffer, int bufferSize);

		[DllImport("gdi32.dll")]
		public static extern int GetTextExtentPoint32(IntPtr hdc, string lpString, int cbString, ref Size size);

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]  
		public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);  

	}

	/// <summary>
	/// WindowsAPI�̂�����Ƃ������b�v
	/// </summary>
	public class WrapApi
	{
		/// <summary>
		/// �r�[�v����炷
		/// </summary>
		/// <param name="type">�Đ�����r�[�v�̎�ނ�\��BeepSound�񋓑�</param>
		/// <returns>�Đ��ɐ���������true�A���s������false</returns>
		public static bool Beep(BeepSound type)
		{
			return WinApi.MessageBeep((uint)type) != 0 ? true : false;
		}

		/// <summary>
		/// �w�肵���E�C���h�E�n���h�����őO�ʂɕ\���B
		/// �ŏ�������Ă����ꍇ�͌��̃T�C�Y�ɖ߂��B
		/// </summary>
		/// <param name="hwnd">�őO�ʂɕ\������E�C���h�E�̃n���h��</param>
		public static void SetForegroundWindow(IntPtr hwnd)
		{
			// �Ȃ������s����
			if (WinApi.IsIconic(hwnd) != 0)	Debug.Assert(WinApi.OpenIcon(hwnd) != 0);
			else							Debug.Assert(WinApi.SetForegroundWindow(hwnd) != 0);
		}
	}

	/// <summary>
	/// �r�[�v���̎�ނ�\��
	/// </summary>
	public enum BeepSound : long
	{
		/// <summary>
		/// �R���s���[�^�̃X�s�[�J���甭������W���I�ȃr�[�v��
		/// </summary>
		Default = 0xFFFFFFFF,
		/// <summary>
		/// ��ʂ̌x����
		/// </summary>
		OK = 0x00000000L,
		/// <summary>
		/// �V�X�e���G���[
		/// </summary>
		Hand = 0x00000010L,
		/// <summary>
		/// ���b�Z�[�W�i�₢���킹�j
		/// </summary>
		Question = 0x00000020L,
		/// <summary>
		/// ���b�Z�[�W�i�x���j
		/// </summary>
		Exclamation = 0x00000030L,
		/// <summary>
		/// ���b�Z�[�W�i���j
		/// </summary>
		Asterisk = 0x00000040L,
	}

	/// <summary>
	/// Windows���b�Z�[�W��\��
	/// </summary>
	public enum Wm
	{
		User = 0x0400,
		App = 0x8000,
	}

	/// <summary>
	/// �R�����R���g���[�����b�Z�[�W
	/// </summary>
	public enum Ccm
	{
		First       = 0x2000,
		SetBkColor  = 0x2000 + 1,
	}

	/// <summary>
	/// WindowLong
	/// </summary>
	public enum WindowLong
	{
		WndProc        = (-4),
		hInstance      = (-6),
		HwndParent     = (-8),
		Style          = (-16),
		ExStyle        = (-20),
		UserData       = (-21),
		ID             = (-12),
	}
}
