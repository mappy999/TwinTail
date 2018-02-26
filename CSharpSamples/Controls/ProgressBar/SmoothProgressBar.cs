// SmoothProgressBar.cs

namespace CSharpSamples
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	/// <summary>
	/// �X���[�X�ȕ\�������̃v���O���X�o�[�B
	/// .NET 2.0 �͕W���ł���̂ŕK�v�Ȃ��Ȃ����B
	/// </summary>
	public class SmoothProgressBar : ProgressCtrl
	{
		private ProgressTextStyle style;
		private Color valueColor;

		/// <summary>
		/// �l�����̐F���擾�܂��͐ݒ�
		/// </summary>
		public Color ValueColor {
			set { valueColor = value; }
			get { return valueColor; }
		}

		/// <summary>
		/// �e�L�X�g�̕\���X�^�C�����擾�܂��͐ݒ�
		/// </summary>
		public ProgressTextStyle TextStyle {
			set {
				if (value != style) {
					style = value;
					Refresh();
				}
			}
			get { return style; }
		}

		/// <summary>
		/// SmoothProgressBar�N���X�̃C���X�^���X��������
		/// </summary>
		public SmoothProgressBar() : base()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			ValueColor = SystemColors.Highlight;
			ForeColor = Color.Black;
			style = ProgressTextStyle.Percent;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;
			Rectangle rect = e.ClipRectangle;

			// �u���V���쐬
			Brush brush = new SolidBrush(ValueColor);
			Brush blank = new SolidBrush(SystemColors.Control);

			// position(���݈ʒu)����`��͈͂��v�Z
			float range = (float)(Math.Abs(Minimum) + Math.Abs(Maximum));
			float pos = range != 0 ? ((float)Position / range) : 0;
			float right = rect.Width * pos;

			g.FillRectangle(brush, 0, 0, right, rect.Height);
			g.FillRectangle(blank, right, 0, rect.Width - right, rect.Height);

			// ������`��
			StringFormat format = StringFormat.GenericDefault;
			Brush textbrush = new SolidBrush(ForeColor);
			string text = null;

			switch (style) 
			{
			case ProgressTextStyle.Percent:
				text = Percent + "%";
				break;

			case ProgressTextStyle.Length:
				text = String.Format("{0}/{1}", Position, Maximum);
				break;

			case ProgressTextStyle.None:
				text = String.Empty;
				break;
			}

			// �S�̂̒����ɔz�u
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			// �������`��
			g.DrawString(text, this.Font, textbrush, rect, format);

			// ���E����`��
			Rectangle bounds = new Rectangle(0, 0, Width, Height);
			ControlPaint.DrawBorder3D(g, bounds, BorderStyle);
		}
	}

	/// <summary>
	/// �v���O���X�ɕ\������e�L�X�g�̕\���X�^�C����\��
	/// </summary>
	public enum ProgressTextStyle
	{
		/// <summary>
		/// �e�L�X�g��\�����Ȃ�
		/// </summary>
		None = 0,
		/// <summary>
		/// �S�����\��
		/// </summary>
		Percent,
		/// <summary>
		/// �S�̂̒�����\��
		/// </summary>
		Length,
	}
}
