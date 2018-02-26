// ThreadHeaderInfo.cs

namespace Twin
{
	using System;
	using System.ComponentModel;

	/// <summary>
	/// �X���b�h�̊e�����Ǘ�
	/// </summary>
	public class ThreadHeaderInfo
	{
		private ThreadHeader header;

		/// <summary>
		/// ThreadHeaderInfo�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="h"></param>
		public ThreadHeaderInfo(ThreadHeader h)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			header = h;
		}

		/// <summary>
		/// 1��������̃��X�����擾
		/// </summary>
		public float ForceValueDay {
			get {
				TimeSpan span = DateTime.Now - header.Date.ToLocalTime();
				return (float)Math.Round(header.ResCount / span.TotalDays, 1);
			}
		}

		/// <summary>
		/// 1���Ԃ�����̃��X�����擾
		/// </summary>
		public float ForceValueHour {
			get {
				TimeSpan span = DateTime.Now - header.Date.ToLocalTime();
				return (float)Math.Round(header.ResCount / span.TotalHours, 1);
			}
		}

		/// <summary>
		/// ���̃X���b�h�̏d�v�x���v�Z
		/// </summary>
		public int Valuable {
			get {
				// �X�V�X�� = 3�A �S�����X�� = 2�A�V���X�� = 1, �ʏ�X�� = 0
				if (header.IsNewThread) return 1;
				if (header.GotResCount == 0) return 0;
				if (header.ResCount == header.GotResCount) return 2;
				if (header.SubNewResCount > 0) return 3;

				return 0;
			}
		}

		/// <summary>
		/// ���̃X���b�h��24���Ԉȓ��ɗ��Ă�ꂽ�X���b�h���ǂ���
		/// </summary>
		public bool Within24Hours {
			get { return IsWithinHours(24); }
		}

		/// <summary>
		/// ���̃X���b�h��hours���Ԉȓ��ɗ��Ă�ꂽ�X���b�h���ǂ���
		/// </summary>
		/// <param name="hours"></param>
		/// <returns></returns>
		public bool IsWithinHours(int hours)
		{
			DateTime date = header.Date.AddHours(hours);
			return (DateTime.Now <= date) ? true : false;
		}

		/// <summary>
		/// �������v�Z���A���ʂ𕶎���`���ŕԂ�
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public string GetForceValue(ForceValueOf type)
		{
			if (type == ForceValueOf.Day)
			{
				return ForceValueDay.ToString("0.0d");
			}
			else {
				return ForceValueHour.ToString("0.0h");
			}
		}

		public static string GetForceValue(ThreadHeader h, ForceValueOf valueType)
		{
			return new ThreadHeaderInfo(h).GetForceValue(valueType);
		}
	}

	
	/// <summary>
	/// �������v�Z����Ƃ��Ɏg�p����P�ʂ�\��
	/// </summary>
	public enum ForceValueOf
	{
		/// <summary>
		/// �ꎞ�Ԃ�����̐���
		/// </summary>
		[Description("1����")]
		Hour,
		/// <summary>
		/// ���������̐���
		/// </summary>
		[Description("1��")]
		Day,
	}
}
