// NGURLCollection.cs

namespace ImageViewerDll
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using CSharpSamples.Text.Search;

	/// <summary>
	/// NGURLCollection �̊T�v�̐����ł��B
	/// </summary>
	public class NGURLCollection
	{
		private ArrayList searcher;

		/// <summary>
		/// �o�^����Ă��邷�ׂẴp�^�[�����擾�܂��͐ݒ�
		/// </summary>
		public string[] Patterns {
			set {
				searcher.Clear();

				foreach (string pattern in value)
					Add(pattern);
			}
			get {
				ArrayList arrayList = new ArrayList();

				foreach (ISearchable s in searcher)
					arrayList.Add(s.Pattern);

				return (string[])arrayList.ToArray(typeof(string));
			}
		}

		/// <summary>
		/// NGURLCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public NGURLCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			searcher = new ArrayList();
		}

		/// <summary>
		/// �p�^�[����ǉ�
		/// </summary>
		/// <param name="pattern"></param>
		public void Add(string pattern)
		{
			searcher.Add(new BmSearch2(pattern));
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�̗v�f���폜
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			searcher.RemoveAt(index);
		}

		/// <summary>
		/// ���ׂẴp�^�[�����폜
		/// </summary>
		public void Clear()
		{
			searcher.Clear();
		}

		/// <summary>
		/// �w�肵��URL���o�^����Ă���p�^�[���Ɉ�v���邩�ǂ����𔻒f
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public bool IsMatch(string url)
		{
			foreach (ISearchable s in searcher)
			{
				if (s.Search(url) >= 0)
					return true;
			}
			return false;
		}
	}
}
