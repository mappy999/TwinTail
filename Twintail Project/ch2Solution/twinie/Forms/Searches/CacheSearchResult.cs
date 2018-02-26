// CacheSearchResult.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// CacheSearcher�̌������ʂ�\��
	/// </summary>
	public class CacheSearchResult
	{
		private ThreadHeader header;
		private int index;

		/// <summary>
		/// ���������X���b�h�̏����擾
		/// </summary>
		public ThreadHeader HeaderInfo {
			get { return header; }
		}

		/// <summary>
		/// �������������ꍇ�A��v�C���f�b�N�X���擾�B
		/// �������s�����ꍇ�A-1��Ԃ��B
		/// </summary>
		public int Index {
			get { return index; }
		}

		/// <summary>
		/// CacheSearchResult�N���X�̃C���X�^���X��������
		/// </summary>
		public CacheSearchResult(ThreadHeader header, int index)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.header = header;
			this.index = index;
		}
	}
}
