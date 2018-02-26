// IArchivable.cs

using System;

namespace CSharpSamples
{
	/// <summary>
	/// IArchivable �C���^�[�t�F�[�X�ł��B
	/// </summary>
	public interface IArchivable
	{
		/// <summary>
		/// �𓀂ɑΉ����Ă���� true�A���Ή��Ȃ� false ��Ԃ��܂��B
		/// </summary>
		bool CanExtract {
			get;
		}

		/// <summary>
		/// ���k�ɑΉ����Ă���� true�A���Ή��Ȃ� false ��Ԃ��܂��B
		/// </summary>
		bool CanCompress {
			get;
		}
	}
}
