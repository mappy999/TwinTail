// KakikomiRireki.cs

namespace Twin.Tools
{
	using System;
	using System.IO;
	using System.Text;
	using Twin.Util;

	/// <summary>
	/// �������ݗ������쐬�E�Ǘ�
	/// </summary>
	public class KakikomiRireki
	{
		private string baseDir;
		
		/// <summary>
		/// KakikomiRireki�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="dir">�������ݗ���ۑ��t�H���_</param>
		public KakikomiRireki(string dir)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			baseDir = dir;
		}

		/// <summary>
		/// �w�肵���̏������ݗ����t�@�C���̃p�X���擾
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public string GetKomiPath(BoardInfo board)
		{
			return Path.Combine(baseDir, board.Name + ".txt");
		}

		/// <summary>
		/// �w�肵���̏������ݗ�����ǂݍ���
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public string Load(BoardInfo board)
		{
			string filePath = GetKomiPath(board);
			return FileUtility.ReadToEnd(filePath);
		}

		/// <summary>
		/// �w�肵���̏������ݗ����ɒǉ�
		/// </summary>
		/// <param name="board"></param>
		/// <param name="res"></param>
		public void Append(ThreadHeader header, WroteRes res)
		{
			try {
				string filePath = GetKomiPath(header.BoardInfo);

				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Date: {0}\r\nSubject: {1}\r\nURL: {2}\r\nFrom: {3}\r\nEmail: {4}\r\n\r\n{5}\r\n------------------------\r\n",
					res.Date.ToString(), header.Subject, header.Url, res.From, res.Email, res.Message);

				FileUtility.Write(filePath, sb.ToString(), true);
			}
			catch (Exception ex) {
				TwinDll.Output(ex);
			}
		}

		/// <summary>
		/// �w�肵���̏������ݗ������폜
		/// </summary>
		/// <param name="board"></param>
		public void Delete(BoardInfo board)
		{
			string filePath = GetKomiPath(board);
			File.Delete(filePath);
		}

		/// <summary>
		/// �w�肵���̏������ݗ��������݂��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public bool IsExists(BoardInfo board)
		{
			string filePath = GetKomiPath(board);
			return File.Exists(filePath);
		}
	}
}
