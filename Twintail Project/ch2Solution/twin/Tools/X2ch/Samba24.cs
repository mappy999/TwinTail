// Samba24.cs

namespace Twin.Tools
{
	using System;
	using System.Collections;
	using System.IO;
	using CSharpSamples;
	using System.Net;
	using System.Text.RegularExpressions;
using System.Diagnostics;

	/// <summary>
	/// Samba24�΍�Ɏ���K�����s��
	/// </summary>
	public class Samba24
	{
		private Hashtable table;
		private CSPrivateProfile profile;
		private string filePath;

		/// <summary>
		/// �w�肵���T�[�o�[���̋K���b�����擾
		/// </summary>
		public int this[string server] {
			get {
				return profile.GetInt("samba", server, 0);
			}
		}

		/// <summary>
		/// Samba24�N���X�̃C���X�^���X��������
		/// </summary>
		public Samba24()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			table = new Hashtable();
			profile = new CSPrivateProfile();
			filePath = null;
		}

		/// <summary>
		/// Samba24�N���X�̃C���X�^���X��������
		/// </summary>
		public Samba24(string filePath) : this()
		{
			Load(filePath);
		}

		/// <summary>
		/// samba�ݒ�t�@�C����ǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath)
		{
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}
			if (File.Exists(filePath))
				profile.Read(filePath);

			this.filePath = filePath;
		}

		/// <summary>
		/// �w�肵���T�[�o�[�̃J�E���^�[�J�n
		/// </summary>
		/// <param name="server"></param>
		public void CountStart(string server)
		{
			// ���ݒl��ݒ�
			table[server] = Environment.TickCount;
		}

		/// <summary>
		/// �w�肵���T�[�o�[�̋K�����Ԃ��o�߂������ǂ����𔻒f
		/// </summary>
		/// <param name="server">�`�F�b�N����T�[�o�[��</param>
		/// <returns>�K�����Ԃ��߂��Ă�����true�A�K�����ԓ��Ȃ�false</returns>
		public bool IsElapsed(string server)
		{
			int r;
			return IsElapsed(server, out r);
		}

		/// <summary>
		/// �w�肵���T�[�o�[�̋K�����Ԃ��o�߂������ǂ����𔻒f�B
		/// �J�E���^���J�n����Ă��Ȃ���΁A���true��Ԃ��B
		/// </summary>
		/// <param name="server">�`�F�b�N����T�[�o�[��</param>
		/// <param name="result">�c��b�����i�[�����</param>
		/// <returns>�K�����Ԃ��߂��Ă�����true�A�K�����ԓ��Ȃ�false</returns>
		public bool IsElapsed(string server, out int result)
		{
			if (table.Contains(server))
			{
				int now = Environment.TickCount;	// ���ݒl
				int begin = (int)table[server];		// �J�n�l

				// �o�ߕb�����v�Z
				int count = (now - begin) / 1000;

				// �c��b�����v�Z
				result = this[server] - count;

				return (count >= this[server]) ? true : false;
			}
			else {
				//throw new ArgumentException(server + "�̊J�n�l�����݂��܂���");
				result = 0;
				return true;
			}
		}

		/// <summary>
		/// �w�肵���T�[�o�[��Samba�J�E���g���C��
		/// </summary>
		/// <param name="server"></param>
		/// <param name="newCount"></param>
		public void Correct(string server, int newCount)
		{
			// �e�[�u���ɐV�����l��ݒ肵�ĕۑ�
			profile.SetValue("samba", server, newCount);
			
			if (filePath != null)
				profile.Write(filePath);
		}

		/// <summary>
		/// ���ׂẴJ�E���^�����Z�b�g
		/// </summary>
		public void Reset()
		{
			table.Clear();
		}

		/// <summary>
		/// �̃g�b�v�y�[�W���擾���āA�ŐV��samba�l���擾�B
		/// </summary>
		/// <param name="bi"></param>
		/// <returns>�������X�V���ꂽ�ꍇ�ɂ́A�ŐV��samba24�̒l��Ԃ��܂��B����ȊO�� -1 ��Ԃ��܂��B</returns>
		public int Update(BoardInfo bi)
		{
			using (WebClient w = new WebClient())
			{
				string html = w.DownloadString(bi.Url);
				Match m = Regex.Match(html, @"\+Samba24=([0-9]+)", RegexOptions.RightToLeft);
				int newVal;
				if (Int32.TryParse(m.Groups[1].Value, out newVal))
				{
					Correct(bi.Server, newVal);
					Debug.WriteLine("Samba24, Update: " + newVal);
					return newVal;
				}
			}
			return -1;
		}
	}
}
