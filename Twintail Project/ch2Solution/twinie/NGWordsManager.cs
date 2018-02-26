// NGWordsManager.cs

namespace Twin
{
	using System;
	using System.IO;
	using System.Collections;
	using CSharpSamples;

	/// <summary>
	/// NGWords�N���X���g�p���Ĕ��Ƃ�NG���[�h�ݒ���Ǘ�����N���X
	/// </summary>
	public class NGWordsManager
	{
		private const string default_key = "default";
		private string path;

		private Hashtable hash;
		private NGWords defwords;

		/// <summary>
		/// �f�t�H���g��NG���[�h�ݒ���擾
		/// </summary>
		public NGWords Default {
			get {
				// �f�t�H���g��NG���[�h�����݂��Ȃ��ꍇ�͍쐬
				if (defwords == null)
				{
					defwords = new NGWords();
					hash.Add(default_key, defwords);
				}

				return defwords;
			}
		}

		/// <summary>
		/// NGWordsManager�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="folderPath">NG���[�h�ݒ�t�H���_</param>
		public NGWordsManager(string folderPath)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			hash = new Hashtable();
			defwords = null;
			path = folderPath;
		}

		/// <summary>
		/// �w�肵���̃L�[�����擾
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		private string GetKey(BoardInfo board)
		{
			if (board != null)
			{
				return board.DomainPath.Replace("/", "-");
			}
			else {
				return default_key;
			}
		}

		/// <summary>
		/// ���ׂĂ�NG���[�h��ǂݍ���
		/// </summary>
		public void Load()
		{
			string[] settings = Directory.GetFiles(path);
			hash.Clear();

			foreach (string sett in settings)
			{
				NGWords n = new NGWords(sett);
				string key = Path.GetFileName(sett);

				hash[key] = n;
			}

			if (hash.Contains(default_key))
				defwords = (NGWords)hash[default_key];
		}

		/// <summary>
		/// ���ׂĂ�NG���[�h��ۑ�
		/// </summary>
		public void Save()
		{
			foreach (string key in hash.Keys)
			{
				NGWords n = (NGWords)hash[key];
				n.Save(Path.Combine(path, key));
			}
		}

		/// <summary>
		/// �w�肵����NG���[�h�̂ݕۑ�
		/// </summary>
		public void Save(BoardInfo bi)
		{
			if (bi == null)
				throw new ArgumentNullException("bi");

			string key = GetKey(bi);

			NGWords n = (NGWords)hash[key];
			n.Save(Path.Combine(path, key));
		}

		/// <summary>
		/// �w�肵����NG���[�h�ݒ��ǉ�
		/// </summary>
		/// <param name="board">word��ǉ������</param>
		public NGWords Add(BoardInfo board)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			string key = GetKey(board);

			if (!hash.ContainsKey(key))
			{
				NGWords n = new NGWords();
				hash[key] = n;
			}

			return (NGWords)hash[key];
		}

		/// <summary>
		/// �w�肵����NG���[�h�ݒ���폜
		/// </summary>
		/// <param name="board">NG���[�h�ݒ���폜�����</param>
		public void Remove(BoardInfo board)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			string key = GetKey(board);
			File.Delete(Path.Combine(path, key));
			hash.Remove(key);
		}

		/// <summary>
		/// �w�肵����NG���[�h�ݒ肪���݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public bool Exists(BoardInfo board)
		{
			string key = GetKey(board);
			return hash.ContainsKey(key);
		}

		/// <summary>
		/// �w�肵����NG���[�h�ݒ���擾�B
		/// </summary>
		/// <param name="board"></param>
		/// <param name="create">�ݒ肪���݂��Ȃ��ꍇ�ɋ��NGWords���쐬���邩�ǂ���</param>
		/// <returns>�w�肵���ɐݒ肳��Ă���NG���[�h</returns>
		public NGWords Get(BoardInfo board, bool create)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			string key = GetKey(board);
			NGWords nGWords = hash.ContainsKey(key) ? (NGWords)hash[key] : null;

			// ���݂��Ȃ��ꍇ�͋�̐ݒ���쐬
			if (nGWords == null && create)
				nGWords = Add(board);

			return nGWords;
		}

		/// <summary>
		/// ���ׂĂ�NG���[�h�ݒ���폜
		/// </summary>
		public void Clear()
		{
			hash.Clear();
		}
	}
}
