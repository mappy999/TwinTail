// ColorProgressBar.cs

namespace CSharpSamples
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	/// <summary>
	/// �J���t���ȃv���O���X�o�[
	/// </summary>
	public class ColorProgressBar : ProgressCtrl
	{
		private Color[] colors;
		private Color borderColor;
		private int scaleSize;
		private int borderSize;

		/// <summary>
		/// �ڐ���̃T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public int ScaleSize {
			set {
				if (value < 1) {
					throw new ArgumentOutOfRangeException("ScaleSize��1�ȏ�̒l�łȂ���΂Ȃ�܂���");
				}
				scaleSize = value;
			}
			get { return scaleSize; }
		}

		/// <summary>
		/// ���E�̐F���擾�܂��͐ݒ�
		/// </summary>
		public Color BorderColor {
			set { borderColor = value; }
			get { return borderColor; }
		}

		/// <summary>
		/// �v���O���X�o�[�̐F�z����擾�܂��͐ݒ�
		/// </summary>
		public Color[] Colors {
			set {
				if (value == null) {
					throw new ArgumentNullException("Colors");
				}
				colors = value;
				Refresh();
			}
			get { return colors; }
		}

		/// <summary>
		/// ColorProgressBar�N���X�̃C���X�^���X��������
		/// </summary>
		public ColorProgressBar() : base()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			scaleSize = 8;
			borderSize = 1;
			borderColor = Color.Black;
			BackColor = Color.DarkGreen;
			BorderStyle = Border3DStyle.Flat;
			colors = new Color[1] { Color.Green };
		}

		/// <summary>
		/// �ڐ����`��
		/// </summary>
		/// <param name="g"></param>
		private void DrawScale(Graphics g)
		{
			Pen pen = new Pen(borderColor, borderSize);
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
			
			// �l�p��`��
			g.DrawRectangle(pen, rect);

			// �c����`��
			Point from = new Point(0, 0);
			Point to = new Point(0, this.Height);

			for (int i = Minimum; i <= Maximum; i++)
			{
				from.X = (scaleSize + borderSize) * i;
				to.X = (scaleSize + borderSize) * i;
				g.DrawLine(pen, from, to);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			// �ڐ����`��
			DrawScale(g);

			// �P�̐F�ɑ΂��Ẵ����������v�Z
			int clrCount = Maximum / colors.Length;

			// ���W
			Rectangle rect = new Rectangle(borderSize, borderSize, 
				scaleSize, this.Height - borderSize * 2);
			SolidBrush brush = null;

			for (int i = Minimum; i < Position; i++)
			{
				int clridx = i / clrCount;
				
				if (clridx >= colors.Length)
					clridx = colors.Length-1;

				Color color = colors[clridx];

				// �u���V���쐬
				if (brush == null || brush.Color != color)
					brush = new SolidBrush(color);

				g.FillRectangle(brush, rect);
				rect.X += scaleSize + borderSize;
			}

			// ���E����`��
			Rectangle bounds = new Rectangle(0, 0, Width, Height);
			ControlPaint.DrawBorder3D(g, bounds, BorderStyle);
		}

		/// <summary>
		/// �ڐ���ƕ��𒚓x�����悤�Ƀ��T�C�Y
		/// </summary>
		public void ResizeBar()
		{
			this.Width = (ScaleSize + borderSize) * Maximum + borderSize;
		}
	}

}
