// IPopup.cs

namespace Twin
{
	using System;
	using System.Drawing;
	using Twin;

	/// <summary>
	/// �|�b�v�A�b�v�C���^�[�t�F�[�X
	/// </summary>
	public interface IPopupBase
	{
		/// <summary>
		/// �q�|�b�v�A�b�v�����擾
		/// </summary>
		int Count {
			get;
		}

		/// <summary>
		/// �|�b�v�A�b�v����ʒu���擾�܂��͐ݒ�
		/// </summary>
		PopupPosition Position {
			set;
			get;
		}

		/// <summary>
		/// �|�b�v�A�b�v�̍ő�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		Size Maximum {
			set;
			get;
		}

		/// <summary>
		/// �|�b�v�A�b�v�̃t�H���g���擾�܂��͐ݒ�
		/// </summary>
		Font Font {
			set;
			get;
		}

		/// <summary>
		/// �����F���擾�܂��͐ݒ�
		/// </summary>
		Color ForeColor {
			set;
			get;
		}

		/// <summary>
		/// �w�i�F���擾�܂��͐ݒ�
		/// </summary>
		Color BackColor {
			set;
			get;
		}

		/// <summary>
		/// �|�b�v�A�b�v�̕����擾
		/// </summary>
		int Width {
			get;
		}

		/// <summary>
		/// �|�b�v�A�b�v�̍������擾
		/// </summary>
		int Height {
			get;
		}

		/// <summary>
		/// �摜�|�b�v�A�b�v�̉摜�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		Size ImageSize {
			set;
			get;
		}

		/// <summary>
		/// text���|�b�v�A�b�v�ŕ\��
		/// </summary>
		/// <param name="text"></param>
		void Show(string text);

		/// <summary>
		/// resSets���e�L�X�g�ɕϊ����ĕ\��
		/// </summary>
		/// <param name="resSets"></param>
		void Show(ResSetCollection resSets);

		/// <summary>
		/// resSet���e�L�X�g�ɕϊ����ĕ\��
		/// </summary>
		/// <param name="resSet"></param>
		void Show(ResSet resSet);

		/// <summary>
		/// �w�肵���摜��URL��\��
		/// </summary>
		/// <param name="uri"></param>
		void ShowImage(string uri);

		/// <summary>
		/// �|�b�v�A�b�v���\���ɂ���
		/// </summary>
		void Hide();

		/// <summary>
		/// �|�b�v�A�b�v�\���^��\���̏��
		/// </summary>
		bool Visible {
			get;
		}
	}

	/// <summary>
	/// �|�b�v�A�b�v����ʒu��\���񋓑�
	/// </summary>
	public enum PopupPosition
	{
		/// <summary>����ɕ\��</summary>
		TopLeft,
		/// <summary>�E��ɕ\��</summary>
		TopRight,
		/// <summary>�����ɕ\��</summary>
		BottomLeft,
		/// <summary>�E���ɕ\��</summary>
		BottomRight,
	}
}
