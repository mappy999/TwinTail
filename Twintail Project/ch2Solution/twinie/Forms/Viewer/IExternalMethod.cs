// IExternalMethod.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// �O���Ăяo���\�Ȋ֐����`
	/// </summary>
	public interface IExternalMethod
	{
		/// <summary>
		/// �ő僌�X�\������ύX
		/// </summary>
		/// <param name="limitCount"></param>
		void SetLimit(int limitCount);

		/// <summary>
		/// start����end�܂ł̃��X��\��
		/// </summary>
		/// <param name="start">0����n�܂郌�X�̕\���J�n�C���f�b�N�X</param>
		/// <param name="end">0����n�܂郌�X�̕\���I���C���f�b�N�X</param>
		void Range(int start, int end);

		/// <summary>
		/// count�����A���̃��X��\��
		/// </summary>
		/// <param name="count">�\�����郌�X��</param>
		void Next(int count);

		/// <summary>
		/// count�����A�O�̃��X��\��
		/// </summary>
		/// <param name="count">�\�����郌�X��</param>
		void Prev(int count);

		/// <summary>
		/// ���݂̃X���b�h���X�V���V�����X���擾
		/// </summary>
		void Reload();

		/// <summary>
		/// ��ԏ�܂ŃX�N���[��
		/// </summary>
		void ScrollTop();

		/// <summary>
		/// ��ԉ��܂ŃX�N���[��
		/// </summary>
		void ScrollBottom();

		/// <summary>
		/// ���݊J���Ă���X���b�h�̒�����L�[���[�h�𒊏o
		/// </summary>
		/// <param name="obj">�����Ώۂ̃I�u�W�F�N�g (0=���ׂāA1=���O�A2=Email�A3=ID, 4=�{��)</param>
		/// <param name="key">�����L�[���[�h</param>
		void Extract(int obj, string key);

		/// <summary>
		/// �w�肵�����X���Q�Ƃ��Ă��郌�X���|�b�v�A�b�v�\���B
		/// </summary>
		/// <param name="index"></param>
		void BackReferences(int index);
	}
}
