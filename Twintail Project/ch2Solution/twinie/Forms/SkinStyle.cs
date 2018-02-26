// SkinStyle.cs

namespace Twin.Forms
{
	using System;
	using System.Drawing;
	using CSharpSamples;

	/// <summary>
	/// SkinStyle �̊T�v�̐����ł��B
	/// </summary>
	public class SkinStyle
	{
		/// <summary>
		/// �w�i�F���擾�܂��͐ݒ�
		/// </summary>
		public Color backColor;

		/// <summary>
		/// �����F���擾�܂��͐ݒ�
		/// </summary>
		public Color foreColor;

		/// <summary>
		/// �|�b�v�A�b�v�̉摜�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public Size imageSize;

		/// <summary>
		/// �t�H���g���擾�܂��͐ݒ�
		/// </summary>
		public Font font;

		/// <summary>
		/// �|�b�v�A�b�v�̕\���X�^�C�����擾�܂��͐ݒ�
		/// </summary>
		public PopupStyle style;

		/// <summary>
		/// �摜�|�b�v�A�b�v�̗L����Ԃ��擾�܂��͐ݒ�
		/// </summary>
		public PopupState imagePopup;

		/// <summary>
		/// �摜�|�b�v�A�b�v�̗L����Ԃ��擾�܂��͐ݒ�
		/// </summary>
		public PopupState urlPopup;

		/// <summary>
		/// �摜�T���l�C���@�\�̗L����Ԃ��擾�܂��͐ݒ�
		/// </summary>
		public bool thumb;

		/// <summary>
		/// SkinStyle�N���X�̃C���X�^���X��������
		/// </summary>
		public SkinStyle(Settings sett)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			backColor = Color.White;
			foreColor = Color.Black;
			imageSize = new Size(100, 100);
			font = new Font("�l�r �o�S�V�b�N", 9);
			style = sett.Popup.Style;
			imagePopup = sett.Popup.ImagePopup;
			urlPopup = sett.Popup.UrlPopup;
			thumb = sett.Thumbnail.Visible;
		}

		/// <summary>
		/// �X�L������ǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		public void Read(string filePath)
		{
			CSPrivateProfile p = new CSPrivateProfile();
			p.Read(filePath);

			// [Popup]�Z�N�V����
			font = new Font(p.GetString("Popup", "FontFace", font.Name), p.GetFloat("Popup", "FontSize", font.Size));
			style = (PopupStyle)Enum.Parse(typeof(PopupStyle), p.GetString("Popup", "Style", style.ToString()));
			backColor = ColorTranslator.FromHtml(p.GetString("Popup", "BackColor", ColorTranslator.ToHtml(backColor)));
			foreColor = ColorTranslator.FromHtml(p.GetString("Popup", "ForeColor", ColorTranslator.ToHtml(foreColor)));
			// [Option]�Z�N�V����
			imagePopup = (PopupState)Enum.Parse(typeof(PopupState), p.GetString("Option", "ImagePopup", imagePopup.ToString()));
			urlPopup = (PopupState)Enum.Parse(typeof(PopupState), p.GetString("Option", "UrlPopup", urlPopup.ToString()));
			thumb = p.GetBool("Option", "Thumb", thumb);
		}

		/// <summary>
		/// �X�L������ۑ�
		/// </summary>
		/// <param name="filePath"></param>
		public void Write(string filePath)
		{
			CSPrivateProfile p = new CSPrivateProfile();
			p.SetValue("Popup", "Style", style);
			p.SetValue("Popup", "BackColor", ColorTranslator.ToHtml(backColor));
			p.SetValue("Popup", "ForeColor", ColorTranslator.ToHtml(foreColor));
			p.SetValue("Popup", "FontFace", font.Name);
			p.SetValue("Popup", "FontSize", font.Size);
			p.SetValue("Option", "ImagePopup", imagePopup);
			p.SetValue("Option", "UrlPopup", urlPopup);
			p.SetValue("Option", "Thumb", thumb);
			p.Write(filePath);
		}
	}
}
