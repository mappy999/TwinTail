// ProgressCtrl.cs

namespace CSharpSamples
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	/// <summary>
	/// �v���O���X�o�[�̊�{
	/// </summary>
	public abstract class ProgressCtrl : Control
	{
		private int minimum;
		private int maximum;
		private int position;
		private int step;
		private Border3DStyle border;

		/// <summary>
		/// �v���O���X�o�[�̍ŏ��l���擾�܂��͐ݒ�
		/// </summary>
		public int Minimum {
			set {
				if (value > maximum) {
					throw new ArgumentOutOfRangeException("Minimum");
				}

				if (minimum != value)
				{
					minimum = value;
					Refresh();
				}
			}
			get { return minimum; }
		}

		/// <summary>
		/// �v���O���X�o�[�̍ő�l���擾�܂��͐ݒ�
		/// </summary>
		public int Maximum {
			set {
				if (value < minimum) {
					throw new ArgumentOutOfRangeException("Maximum");
				}

				if (maximum != value)
				{
					maximum = value;
					Refresh();
				}
			}
			get { return maximum; }
		}

		/// <summary>
		/// �v���O���X�o�[�̌��ݒl���擾�܂��͐ݒ�
		/// </summary>
		public int Position {
			set {
				if (value < 0 || value > maximum) {
					throw new ArgumentOutOfRangeException("Position");
				}

				if (position != value)
				{
					position = value;
					Refresh();
				}
			}
			get { return position; }
		}

		/// <summary>
		/// PerformStep���\�b�h���g�p�������̑��ʕ����擾�܂��͐ݒ�
		/// </summary>
		public int Step {
			set {
				if (value > maximum) {
					throw new ArgumentOutOfRangeException("Step");
				}

				if (step != value)
				{
					step = value;
				}
			}
			get { return step; }
		}

		/// <summary>
		/// �v���O���X�o�[�̋��E�����擾�܂��͐ݒ�
		/// </summary>
		public Border3DStyle BorderStyle {
			set {
				if (value != border) {
					border = value;
					Refresh();
				}
			}
			get { return border; }
		}

		/// <summary>
		/// �S�������擾
		/// </summary>
		protected int Percent {
			get {
				float range = (float)(Math.Abs(minimum) + Math.Abs(maximum));

				if (range == 0)
					return 0;
				
				float result = (float)position / range * 100.0f;
				return (int)result;
			}
		}

		/// <summary>
		/// ProgressCtrl�N���X�̃C���X�^���X��������
		/// </summary>
		public ProgressCtrl()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);

			border = Border3DStyle.SunkenOuter;
			minimum = 0;
			position = 0;
			maximum = 100;
			step = 1;
		}

		/// <summary>
		/// ���݂̈ʒu����Step�̕������i�߂�
		/// </summary>
		public virtual void PerformStep()
		{
			Increment(Step);
		}

		/// <summary>
		/// �w�肵���ʂ������݈ʒu��i�߂�
		/// </summary>
		/// <param name="value">���݈ʒu���C���N�������g�����</param>
		public virtual void Increment(int value)
		{
			if (Position + value >= Maximum)
			{
				Position = Maximum;
			}
			else {
				Position += value;
			}
		}

		/// <summary>
		/// ���݈ʒu���ŏ��l�Ƀ��Z�b�g
		/// </summary>
		public virtual void Reset()
		{
			Position = 0;
		}
	}
}
