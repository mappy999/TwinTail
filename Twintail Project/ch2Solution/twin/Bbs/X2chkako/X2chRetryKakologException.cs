// X2chRetryKakologException.cs

using System;

namespace Twin.Bbs
{
	/// <summary>
	/// X2chRetryKakologException �̊T�v�̐����ł��B
	/// </summary>
	public class X2chRetryKakologException : ApplicationException
	{
		private BoardInfo retryBoard;

		public BoardInfo RetryBoard {
			get {
				return retryBoard;
			}
		}

		/// <summary>
		/// X2chRetryKakologException �N���X�̃C���X�^���X��������
		/// </summary>
		public X2chRetryKakologException(BoardInfo board)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.retryBoard = board;
		}
	}
}
