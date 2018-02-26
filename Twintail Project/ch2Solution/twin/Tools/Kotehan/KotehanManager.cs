// KotehanManager.cs

namespace Twin.Tools
{
	using System;
	using System.IO;
	using System.Collections;
	using CSharpSamples;
	using CSharpSamples.Text.Search;

	/// <summary>
	/// �R�e�n�����Ǘ�
	/// </summary>
	public class KotehanManager
	{
		private CSPrivateProfile profile;

		/// <summary>
		/// �f�t�H���g�̃R�e�n�����擾�܂��͐ݒ�
		/// </summary>
		public Kotehan Default {
			set {
				Kotehan kote = (value != null) ? value : new Kotehan();
				profile.SetValue("Default", "Name", kote.Name);
				profile.SetValue("Default", "Email", kote.Email);
				profile.SetValue("Default", "Be", kote.Be);
			}
			get {
				Kotehan kote = new Kotehan();
				kote.Name = profile.GetString("Default", "Name", String.Empty);
				kote.Email = profile.GetString("Default", "Email", String.Empty);
				kote.Be = profile.GetBool("Default", "Be", false);

				return kote;
			}
		}

		/// <summary>
		/// �ݒ肳��Ă���R�e�n�������ׂĎ擾
		/// </summary>
		public Kotehan[] All {
			get {
				ArrayList list = new ArrayList();

				foreach (CSPrivateProfileSection sec in profile.Sections)
				{
					Kotehan kote = new Kotehan(sec["Name"], sec["Email"], Boolean.Parse(sec["Be"]));
					if (!kote.IsEmpty) list.Add(kote);
				}

				return (Kotehan[])list.ToArray(typeof(Kotehan));
			}
		}

		/// <summary>
		/// KotehanManager�N���X�̃C���X�^���X��������
		/// </summary>
		public KotehanManager()
		{
			profile = new CSPrivateProfile();
		}

		/// <summary>
		/// �w�肵���̃Z�N�V���������擾
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		private string GetSection(BoardInfo board)
		{
			return board.DomainPath;
		}

		/// <summary>
		/// �w�肵���X���b�h�̃Z�N�V���������擾
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		private string GetSection(ThreadHeader header)
		{
			return header.BoardInfo.DomainPath + "#" + header.Key;
		}

		/// <summary>
		/// �w�肵���Z�N�V�����̃R�e�n�����擾�B���݂��Ȃ����null��Ԃ��B
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		private Kotehan GetInternal(string key)
		{
			Kotehan kote = new Kotehan(
				profile.GetString(key, "Name", String.Empty),
				profile.GetString(key, "Email", String.Empty),
				profile.GetBool(key, "Be", false));

			return (kote.IsEmpty) ? null : kote;
		}

		/// <summary>
		/// �w�肵���Z�N�V�����ɃR�e�n����ݒ�
		/// </summary>
		/// <param name="section"></param>
		/// <param name="kotehan">null�܂��͋�̃R�e�n�����w�肷��ƃZ�N�V�������폜</param>
		private void SetInternal(string section, Kotehan kotehan)
		{
			if (kotehan == null || kotehan.IsEmpty)
			{
				profile.Sections.Remove(section);
				return;
			}

			profile.SetValue(section, "Name", kotehan.Name);
			profile.SetValue(section, "Email", kotehan.Email);
			profile.SetValue(section, "Be", kotehan.Be);
		}

		/// <summary>
		/// �R�e�n���ɐݒ肳��Ă���̃T�[�o�[�������������܂��B
		/// </summary>
		/// <param name="oldBoard"></param>
		/// <param name="newBoard"></param>
		public void ServerChange(BoardInfo oldBoard, BoardInfo newBoard)
		{
			ISearchable s = new BmSearch2(oldBoard.DomainPath);

			foreach (CSPrivateProfileSection sec in profile.Sections)
			{
				if (s.Search(sec.Name) >= 0)
				{
					sec.Name.Replace(oldBoard.DomainPath, newBoard.DomainPath);
				}
			}
		}
		
		/// <summary>
		/// �w�肵���t�@�C������R�e�n������ǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public void Load(string filePath)
		{
			profile.RemoveAll();

			if (File.Exists(filePath))
				profile.Read(filePath);
		}

		/// <summary>
		/// �w�肵���t�@�C���ɃR�e�n������ۑ�
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="kotehan"></param>
		public void Save(string filePath)
		{
			profile.Write(filePath);
		}

		/// <summary>
		/// �w�肵���ɃR�e�n�����ݒ肳��Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public bool IsExists(BoardInfo board)
		{
			string key = GetSection(board);
			return profile.Sections.ContainsSection(key);
		}

		/// <summary>
		/// �w�肵���X���b�h�ɃR�e�n�����ݒ肳��Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool IsExists(ThreadHeader header)
		{
			string key = GetSection(header);
			return profile.Sections.ContainsSection(key);
		}

		/// <summary>
		/// �w�肵���ɐݒ肳��Ă���R�e�n�����擾
		/// </summary>
		/// <param name="board">�R�e�n�����擾�����</param>
		/// <returns>���݂����Kotehan�N���X�̃C���X�^���X��Ԃ��B���݂��Ȃ���΃f�t�H���g�l</returns>
		public Kotehan Get(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			string key = GetSection(board);
			Kotehan kote = GetInternal(key);

			if (kote == null)
				kote = Default;

			return kote;
		}

		/// <summary>
		/// �w�肵���X���b�h�ɐݒ肳��Ă���R�e�n�����擾
		/// </summary>
		/// <param name="header">�X���b�h���</param>
		/// <returns>���݂����Kotehan�N���X�̃C���X�^���X��Ԃ��B���݂��Ȃ���΃f�t�H���g�l</returns>
		public Kotehan Get(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			string key = GetSection(header);
			Kotehan kote = GetInternal(key);

			if (kote == null)
				kote = Get(header.BoardInfo);

			return kote;
		}

		/// <summary>
		/// �w�肵���̃R�e�n����ݒ�
		/// </summary>
		/// <param name="board">�R�e�n����ݒ肷���</param>
		/// <param name="kotehan">�ݒ肷��l���i�[���ꂽ�R�e�n��</param>
		public void Set(BoardInfo board, Kotehan kotehan)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}

			string section = GetSection(board);
			SetInternal(section, kotehan);
		}

		/// <summary>
		/// Set�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header">�R�e�n����ݒ肷��X���b�h���</param>
		/// <param name="kotehan">�ݒ肷��l���i�[���ꂽ�R�e�n��</param>
		public void Set(ThreadHeader header, Kotehan kotehan)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			string section = GetSection(header);
			SetInternal(section, kotehan);
		}
	}
}
