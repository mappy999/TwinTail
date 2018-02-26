// PartialDataParser.cs

namespace Twin.Text
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections.Generic;
	using System.Diagnostics;

	/// <summary>
	/// �����I�ȃf�[�^����͂���p�[�T
	/// </summary>
	public abstract class PartialDataParser<T>
	{
		protected readonly BbsType bbsType;
		protected readonly Encoding encoding;

		protected MemoryStream memory;
		protected int capacity;
		protected int remainderLength;

		/// <summary>
		/// PartialDataParser�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="bbs">��͂���f���̎��</param>
		/// <param name="enc">�e�L�X�g�̃G���R�[�f�B���O</param>
		/// <param name="type">����������N���X�̌^</param>
		protected PartialDataParser(BbsType bbs, Encoding enc)
		{
			encoding = enc;
			bbsType = bbs;

			memory = new MemoryStream();
			remainderLength = 0;
			capacity = 4096;
		}

		public Encoding Encoding { get { return this.encoding; } }

		/// <summary>
		/// ���������Ɏc�����]��̒������擾
		/// </summary>
		public int RemainderLength {
			get { return remainderLength; }
		}

		/// <summary>
		/// �o�b�t�@����ɂ���
		/// </summary>
		public virtual void Empty()
		{
			memory = new MemoryStream();
			remainderLength = 0;
		}

		/// <summary>
		/// data����͂��R���N�V�����Ɋi�[
		/// </summary>
		/// <param name="bytes">��͂���o�C�g�f�[�^</param>
		/// <param name="length">data�̒���</param>
		/// <param name="parsed">��͂��ꂽ�f�[�^�̒���</param>
		/// <returns></returns>
		public virtual T[] Parse(byte[] data, int length, out int parsed)
		{
			if (data == null) {
				throw new ArgumentNullException("data");
			}

			List<T> result = new List<T>();
			int index = 0;

			// �O��̗]��f�[�^��data������
			if (memory.Length > 0)
			{			
				memory.Write(data, 0, length);
				data = memory.ToArray();
				length = data.Length;
			}

			// ��͉\�ȕ����̏I����T��
			int tokenLength;
			int token = GetEndToken(data, 0, length, out tokenLength);

			if (token != -1)
			{
				// �O��̗]��f�[�^�̖����ɑ����ĂP�̃f�[�^�ɂ���
				index = token + tokenLength;
				
				// �]��̒��������߂�
				remainderLength = length - index;

				// ������ɕϊ���ɉ��
				string dataText = encoding.GetString(data, 0, index);
				T[] array = ParseData(dataText);

				if (array != null)
				{
					result.AddRange(array);
				}
				else {
					TwinDll.Output("�f�[�^�̉�͂Ɏ��s: " + dataText);
				}
			}
			else {
				// ��͉\�ȕ����f�[�^���Ȃ���΂��ׂĂ�]��f�[�^�Ƃ���
				// �O��̗]��f�[�^�ɏ�����������̉�͂Ɏg�p
				remainderLength = length;
				index = 0;
			}

			// ��͂ł��Ȃ������]��f�[�^���������ɒ��߂�
			memory.Close();
			memory = new MemoryStream(capacity);
			memory.Write(data, index, remainderLength);

			// ���ۂɉ�͂��ꂽ�������擾
			parsed = (length - remainderLength);

			return result.ToArray();
		}

		/// <summary>
		/// �f�[�^����͂��I�u�W�F�N�g���쐬
		/// </summary>
		/// <param name="lineData"></param>
		/// <returns></returns>
		protected abstract T[] ParseData(string dataText);

		/// <summary>
		/// �Ō�̃g�[�N���̈ʒu������
		/// </summary>
		/// <param name="bytes">��̓f�[�^</param>
		/// <param name="index">�����J�n�ʒu</param>
		/// <param name="length">�f�[�^�̒���</param>
		/// <param name="tokenLength">�g�[�N����������΂��̒������i�[�����</param>
		/// <returns>�g�[�N�������������ʒu</returns>
		protected abstract int GetEndToken(byte[] bytes, int index, int length, out int tokenLength);
	}
}
