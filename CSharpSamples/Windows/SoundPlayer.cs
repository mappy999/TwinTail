// PlaySound.cs

namespace CSharpSamples
{
	using System;
	using System.Runtime.InteropServices;

	// PlaySound�֐�
	// ms-help://MS.VSCC/MS.MSDNVS.1041/jpmltimd/html/_win32_playsound.htm

	/// /// <summary>
	/// �T�E���h���Đ�����N���X
	/// </summary>
	[Obsolete("System.Media.SoundPlayer �N���X���g�p���Ă��������B")]
	public class SoundPlayer
	{
		/// /// <summary>
		/// �T�E���h���Đ�
		/// </summary>
		/// <param name="fileName">�Đ�����T�E���h�̃t�@�C����</param>
		/// <param name="flags">�Đ��t���O</param>
		/// <returns>�Đ��ɐ���������true�A���s������false</returns>
		public static bool Play(string fileName, SoundFlags flags)
		{
			return PlaySound(fileName, IntPtr.Zero, (ulong)flags) != 0 ? true : false;
		}

		[DllImport("winmm.dll")]
		private static extern int PlaySound(string pszSound, IntPtr handle, ulong fdwSound);
	}

	/// /// <summary>
	/// PlaySound�̃t���O
	/// </summary>
	[Flags]
	public enum SoundFlags : ulong
	{
		/// <summary> �T�E���h�C�x���g�𓯊��Đ����܂��BPlaySound �֐��́A�T�E���h�̍Đ�������������Ő����Ԃ��܂��B (default) </summary>
		Sync = 0x0000, 
		/// <summary> �T�E���h��񓯊��Đ����A�T�E���h���J�n�����ƁAPlaySound �֐��͑����ɐ����Ԃ��܂��B�񓯊��Đ�����Ă���T�E���h���~����ɂ́ApszSound �p�����[�^�� NULL ���w�肵�� PlaySound �֐����Ăяo���Ă��������B </summary>
		Async = 0x0001,  
		/// <summary>����̃T�E���h�C�x���g���g���܂���B�w�肳�ꂽ�T�E���h��������Ȃ������ꍇ�APlaySound �֐��́A����̃T�E���h�i��ʂ̌x�����j���Đ������ɐÂ��ɐ����Ԃ��܂��B </summary>
		NoDefault = 0x0002, 
		/// <summary> �T�E���h�C�x���g�̃t�@�C���́A���������Ɋ��Ƀ��[�h����Ă��܂��BpszSound �p�����[�^�́A���������̃T�E���h�C���[�W�ւ̃|�C���^��\���܂��B  </summary>
		Memory = 0x0004,  
		/// <summary> �T�E���h���J��Ԃ��Đ����܂��BpszSound �p�����[�^�� NULL ���w�肵�� PlaySound �֐����Ăяo���ƁA�T�E���h����~���܂��B�T�E���h�C�x���g��񓯊��Đ�����悤�w�����邽�߂ɁASND_ASYNC �Ɠ����Ɏw�肵�Ȃ���΂Ȃ�܂���B  </summary>
		Loop = 0x0008,  
		/// <summary> ���ɂق��̃T�E���h���Đ�����Ă���ꍇ�A�w�肳�ꂽ�T�E���h���Đ����܂���B�w�肳�ꂽ�T�E���h���Đ����邽�߂ɕK�v�ȃ��\�[�X���A�ق��̃T�E���h���Đ����Ă��ăr�W�[�ł���A�w�肳�ꂽ�T�E���h���Đ��ł��Ȃ��ꍇ�A���̊֐��͎w�肳�ꂽ�T�E���h���Đ������ɁA������ FALSE ��Ԃ��܂��B  </summary>
		NoStop = 0x0010,  
		/// <summary> �Ăяo�����^�X�N�Ɋ֌W����T�E���h�̍Đ����~���܂��BpszSound �p�����[�^�� NULL �ł͂Ȃ��ꍇ�A�w�肵���T�E���h�̂��ׂẴC���X�^���X���~���܂��BpszSound �p�����[�^�� NULL �̏ꍇ�A�Ăяo�����^�X�N�Ɋ֌W���邷�ׂẴT�E���h���~���܂��B  </summary>
		Purge = 0x0040,  
		/// <summary> �A�v���P�[�V�������L�̊֘A�t�����g���ăT�E���h���Đ����܂��B </summary>
		Application = 0x0080,  
		/// <summary> �h���C�o���r�W�[��Ԃ̏ꍇ�A�w�肳�ꂽ�T�E���h���Đ������ɑ����ɐ����Ԃ��܂��B  </summary>
		NoWait = 0x00002000L, 
		/// <summary> pszSound �p�����[�^�́A���W�X�g���܂��� WIN.INI �t�@�C���ɋL�q����Ă���V�X�e���C�x���g�̕ʖ��i�G�C���A�X�j�ł��BSND_FILENAME �� SND_RESOURCE �Ɠ����Ɏw�肷�邱�Ƃ͂ł��܂���B  </summary>
		Alias = 0x00010000L,
		/// <summary> pszSound �p�����[�^�́A��`�ς݂̃T�E���h���ʎq�i"SystemStart"�A"SystemExit" �Ȃǁj�ł��B  </summary>
		AliasID = 0x00110000L, 
		/// <summary> pszSound �p�����[�^�́A�t�@�C������\���܂� </summary>
		FileName = 0x00020000L, 
		/// <summary> �p�����[�^�Ŏw�肵���T�E���h�C�x���g���~������ꍇ�́Ahmod �p�����[�^�ŃC���X�^���X�n���h�����w�肵�Ȃ���΂Ȃ�܂���B   </summary>
		Resource = 0x00040004L,
	}
}
