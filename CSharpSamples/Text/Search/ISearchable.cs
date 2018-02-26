// ISearchable.cs

namespace CSharpSamples.Text.Search
{
	using System;
	using System.Text;

	/// <summary>
	/// �����C���^�[�t�F�[�X��\��
	/// </summary>
	public interface ISearchable
	{
		/// <summary>
		/// �����p�^�[�����擾
		/// </summary>
		string Pattern {
			get;
		}

		/// <summary>
		/// ������ƍ����s��
		/// </summary>
		/// <param name="text">����������</param>
		/// <returns></returns>
		int Search(string text);

		/// <summary>
		/// �w�肵���C���f�b�N�X���當����ƍ����s��
		/// </summary>
		/// <param name="text">����������</param>
		/// <param name="index">�����J�n�C���f�b�N�X</param>
		/// <returns></returns>
		int Search(string text, int index);
	}
}
