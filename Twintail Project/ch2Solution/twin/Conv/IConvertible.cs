// IConvertible.cs

namespace Twin.Conv
{
	using System;

	/// <summary>
	/// ���O�̑��݃R���o�[�^�̃C���^�[�t�F�[�X
	/// </summary>
	public interface IConvertible
	{
		/// <summary>
		/// �w�肵���t�@�C���̃��O��ǂݍ���
		/// </summary>
		/// <param name="filePath">�ǂݍ��ރ��O�ւ̃t�@�C���p�X</param>
		/// <param name="header">�ǂݍ��񂾃��O�̃X���b�h��@���i�[�����</param>
		/// <param name="resCollection">�ǂݍ��񂾃��O�{�����i�[�����</param>
		/// <returns>�ǂݍ��݂ɐ��������true�A���s�����false��Ԃ�</returns>
		void Read(string filePath, out ThreadHeader header, out ResSetCollection resCollection);
		
		/// <summary>
		/// �w�肵���t�@�C���Ƀ��O����������
		/// </summary>
		/// <param name="filePath">�ۑ��惍�O�t�@�C���ւ̃p�X</param>
		/// <param name="header">�X���b�h���</param>
		/// <param name="resCollection">�ۑ����郍�O�{��</param>
		/// <returns>�������݂ɐ��������true�A���s�����false��Ԃ�</returns>
		void Write(string filePath, ThreadHeader header, ResSetCollection resCollection);
	}
}
