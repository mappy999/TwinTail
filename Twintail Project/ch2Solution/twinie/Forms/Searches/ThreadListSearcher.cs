// ThreadListSearchResult.cs

namespace Twin.Text
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using Twin.Forms;
	using CSharpSamples;

	/// <summary>
	/// ThreadListView�N���X�̃X���ꗗ���������邽�߂̋@�\���
	/// </summary>
	public class ThreadListSearcher : AbstractSearcher
	{
		private List<ThreadHeader> matches;
		private List<ThreadHeader> temporary;
		private ThreadListView listview;

		/// <summary>
		/// ThreadListSearcher�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadListSearcher(ThreadListView view)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			listview = view;
			matches = new List<ThreadHeader>();
			matches.AddRange(view.Items);
			temporary = new List<ThreadHeader>();
			temporary.AddRange(view.Items);
		}

		/// <summary>
		/// �X���b�h�ꗗ��������Ԃɖ߂�
		/// </summary>
		public override void Reset()
		{
			matches.Clear();
			matches.AddRange(temporary);
			listview.SetItems(matches);
		}

		/// <summary>
		/// �O��̌������ʂ���A�i�荞�݌�����
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public override bool Search(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}

			// �������ʂ�����R���N�V����
			List<ThreadHeader> result = new List<ThreadHeader>();

			if (key.Length > 0)
			{
				// ���K�\���̃I�v�V����
				RegexOptions regopt = RegexOptions.Compiled;

				if ((Options & SearchOptions.MatchCase) == 0)
					regopt = RegexOptions.IgnoreCase;

				if ((Options & SearchOptions.Regex) == 0)
					key = Regex.Escape(key);

				// ���K�\�����R���p�C��
				Regex regex = new Regex(key, regopt);

				foreach (ThreadHeader h in matches)
				{
					if (regex.IsMatch(h.Subject))
						result.Add(h);
				}
			}
			else
			{
				// ��̃L�[���[�h���w�肳�ꂽ�ꍇ�͂��ׂẴA�C�e����ݒ�
				result.AddRange(temporary);
			}
			matches.Clear();
			matches.AddRange(result);
			listview.SetItems(matches);

			return (matches.Count > 0);
		}

		/// <summary>
		/// ���̃��\�b�h�̓T�|�[�g���Ă��܂���
		/// </summary>
		public override void Highlights(string key)
		{
			throw new NotSupportedException("Highlights���\�b�h�̓T�|�[�g���Ă��܂���");
		}
	}
}
