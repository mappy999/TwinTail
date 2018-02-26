// IBoardTable.cs
// #2.0

namespace Twin
{
	using System;
	using System.Collections.Generic;
	using System.Net;

	/// <summary>
	/// �ꗗ���Ǘ�����C���^�[�t�F�[�X
	/// </summary>
	public interface IBoardTable
	{
		/// <summary>
		/// �o�^����Ă��邷�ׂĂ� Category ���i�[���� List ��Ԃ��܂��B
		/// </summary>
		List<Category> Items
		{
			get;
		}

		/// <summary>
		/// �I�����C���ōŐV�̔ꗗ�ɍX�V���܂��B
		/// </summary>
		/// <param name="url">�X�V��URL</param>
		/// <param name="callback">���ړ]���Ă����ꍇ�ɌĂ΂��R�[���o�b�N</param>
		void OnlineUpdate(string url, BoardUpdateEventHandler callback);

		/// <summary>
		/// �w�肵���ꗗ�����݂̃e�[�u���ɒǉ����܂��B
		/// </summary>
		/// <param name="table">�ǉ����� IBoardTable�B</param>
		void Add(IBoardTable table);

		/// <summary>
		/// �ꗗ�����ׂč폜���A��̏�Ԃɂ��܂��B
		/// </summary>
		void Clear();

		/// <summary>
		/// �ꗗ���w�肵���t�@�C���ɕۑ����܂��B
		/// </summary>
		/// <param name="fileName">�ۑ��t�@�C�����B</param>
		void SaveTable(string fileName);

		/// <summary>
		/// �w�肵���t�@�C������ꗗ��ǂݍ��݂܂��B
		/// </summary>
		/// <param name="fileName">�ǂݍ��ރt�@�C�����B</param>
		void LoadTable(string fileName);

		/// <summary>
		/// ��ʂ̔֒u��������
		/// </summary>
		/// <param name="oldBoard">�Â���</param>
		/// <param name="newBoard">�V������</param>
		void Replace(BoardInfo oldBoard, BoardInfo newBoard);

		/// <summary>
		/// board �����݂̔ꗗ���ɑ��݂��邩�ǂ����𔻒f���܂��B
		/// </summary>
		/// <param name="board">���������</param>
		/// <returns>board ���ꗗ�ɑ��݂���� true�A����ȊO�� false �ł��B</returns>
		bool Contains(BoardInfo board);

		/// <summary>
		/// ���݂̔ꗗ����w�肵�� URL ���܂ލŏ��Ɍ������� BoardInfo ��Ԃ��܂��B
		/// </summary>
		BoardInfo FromUrl(string url);

		/// <summary>
		/// ���݂̔ꗗ����w�肵��������уh���C���p�X���������A�ŏ��Ɍ������� BoardInfo ��Ԃ��܂��B
		/// </summary>
		BoardInfo FromName(string name, string domainPath);

		/// <summary>
		/// ���ׂĂ̔��� BoardInfo[] �^�ɕϊ����ĕԂ��܂��B
		/// </summary>
		BoardInfo[] ToArray();
	}
}
