// CacheDatSearcher.cs

namespace Twin.Forms
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using Twin.Text;
	using Twin.IO;
	using CSharpSamples.Text.Search;

	/// <summary>
	/// CacheDatSearcher �̊T�v�̐����ł��B
	/// </summary>
	public class CacheDatSearcher : CacheSearcher
	{
		private List<ThreadHeader> items;
		private ISearchable searcher;
		private int index;

		/// <summary>
		/// CacheDatSearcher�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="targets"></param>
		public CacheDatSearcher(Cache cache, BoardInfoCollection targets, SearchOptions options)
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
				StreamReader sr = null;

				try {
					if (ThreadIndexer.Read(cache, header) != null)
					{
						// dat���J��
						string path = cache.GetDatPath(header);
						sr = new StreamReader(StreamCreator.CreateReader(path, header.UseGzip), Encoding.Default);

						// �S��������
						int pos = searcher.Search(sr.ReadToEnd());
						if (pos != -1) {
							result = new CacheSearchResult(header, pos);
							break;
						}
					}
				}
				finally {
					if (sr != null)
						sr.Close();
					sr = null;
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
