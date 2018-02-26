// X2chThreadListFormatter.cs
// #2.0

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using System.Collections.Generic;
	using Twin.Text;

	/// <summary>
	/// ThreadHeader��subject.txt�`���ɕϊ�����@�\���
	/// </summary>
	public class X2chThreadListFormatter : ThreadListFormatter
	{
		/// <summary>
		/// �w�肵���w�b�_�[�����������ĕ�����ɕϊ�
		/// </summary>
		public override string Format(ThreadHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			StringBuilder sb =
				new StringBuilder(128);

			// ����: key.dat<>subject (rescount)
			sb.Append(header.Key);
			sb.Append(".dat");
			sb.Append("<>");
			sb.Append(header.Subject);
			sb.Append(" (");
			sb.Append(header.ResCount);
			sb.Append(")");

			return sb.ToString();
		}

		/// <summary>
		/// �w�肵���w�b�_�[�R���N�V���������������ĕ�����ɕϊ�
		/// </summary>
		public override string Format(List<ThreadHeader> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			StringBuilder sb =
				new StringBuilder(128 * items.Count);

			foreach (ThreadHeader header in items)
			{
				sb.Append(Format(header));
				sb.Append('\n');
			}

			return sb.ToString();
		}
	}
}
