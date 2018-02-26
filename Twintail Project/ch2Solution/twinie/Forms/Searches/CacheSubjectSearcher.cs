// CacheSubjectSearcher.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;
	using Twin.Text;
	using CSharpSamples.Text.Search;

	/// <summary>
	/// �L���b�V�����̊����X���b�h�ꗗ����������N���X
	/// </summary>
	public class CacheSubjectSearcher : CacheSearcher
	{
		private List<ThreadHeader> items;
		private ISearchable searcher;
		private int index;

		/// <summary>
		/// CacheSubjectSearcher�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="targets"></param>
		/// <param name="options"></param>
		public CacheSubjectSearcher(Cache cache, BoardInfoCollection targets, SearchOptions options)
			: base(cache, targets, options)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// �w�肵���L�[���[�h�ŃL���b�V����������
		/// </summary>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public override int Search(string keyword)
		{
			if (keyword == null) {
				throw new ArgumentNullException("keyword");
			}
			if (keyword == String.Empty) {
				throw new ArgumentException("keyword", "keyword�͋󕶎��ɏo���܂���");
			}

			searcher = new BmSearch2(keyword);
			items = GetTargetThreads();
			index = 0;

			return items.Count;
		}

		/// <summary>
		/// ��������
		/// </summary>
		/// <returns></returns>
		public override CacheSearchResult Next()
		{
			CacheSearchResult result = null;

			while (index < items.Count)
			{
				ThreadHeader header = items[index++];
				int position = searcher.Search(header.Subject);

				if (position != -1) {
					result = new CacheSearchResult(header, position);
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// ���ׂẴX���b�h�ꗗ������
		/// </summary>
		/// <returns></returns>
		public override CacheSearchResult[] Matches()
		{
			List<CacheSearchResult> list = new List<CacheSearchResult>();
			CacheSearchResult r = null;

			while ((r = Next()) != null)
				list.Add(r);

			return list.ToArray();
		}
	}
}
