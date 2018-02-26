// CacheSearcher.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;
	using Twin.Text;
	using Twin.IO;
	using CSharpSamples;

	/// <summary>
	/// �L���b�V�������������邽�߂̊�{�N���X
	/// </summary>
	public abstract class CacheSearcher
	{
		protected readonly Cache cache;
		protected readonly BoardInfoCollection targets;
		protected readonly SearchOptions options;

		/// <summary>
		/// �����I�v�V�������擾
		/// </summary>
		public SearchOptions Options
		{
			get
			{
				return options;
			}
		}

		/// <summary>
		/// CacheSearcher�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="targets"></param>
		/// <param name="options"></param>
		protected CacheSearcher(Cache cache, BoardInfoCollection targets, SearchOptions options)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			if (targets == null)
			{
				throw new ArgumentNullException("targets");
			}
			this.cache = cache;
			this.targets = targets;
			this.options = options;
		}

		/// <summary>
		/// �����Ώۂ̔̃X���b�h�������ׂĎ擾
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		protected List<ThreadHeader> GetTargetThreads()
		{
			GotThreadListReader listReader = new GotThreadListReader(cache);
			List<ThreadHeader> items = new List<ThreadHeader>();

			foreach (BoardInfo board in targets)
			{
				if (cache.Exists(board))
				{
					if (listReader.Open(board))
					{
						while (listReader.Read(items) != 0)
							;
						listReader.Close();
					}
				}
			}

			return items;
		}

		/// <summary>
		/// �w�肵���L�[���[�h�Ō����������Ώۂ̃X���b�h����Ԃ�
		/// </summary>
		/// <param name="keyword">�����L�[���[�h</param>
		/// <returns>�����Ώۂ̔ɑ��݂���X���b�h�̑���</returns>
		public abstract int Search(string keyword);

		/// <summary>
		/// ���̃X���b�h���������A�������ʂ�Ԃ�
		/// </summary>
		/// <returns>��v�����X���b�h�����݂���Ό������ʂ�Ԃ��A��v���Ȃ����null��Ԃ��B</returns>
		public abstract CacheSearchResult Next();

		/// <summary>
		/// ���ׂẴX���b�h���������A�������ʂ̔z���Ԃ�
		/// </summary>
		/// <returns></returns>
		public abstract CacheSearchResult[] Matches();
	}
}
