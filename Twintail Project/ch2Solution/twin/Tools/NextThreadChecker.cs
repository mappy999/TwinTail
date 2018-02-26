// NextThreadChecker.cs
// #2.0

namespace Twin.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Net;
	using Twin.IO;

	using __WordSet = System.Collections.Generic.KeyValuePair<Twin.Tools.NextThreadChecker.WordType, string>;
	using System.Diagnostics;
	using Twin.Text;

	/// <summary>
	/// ���X���ē��@�\
	/// </summary>
	public class NextThreadChecker
	{
		private ThreadHeader header;
		private List<ThreadHeader> matchItems;
		private Thread thread;
		private int matchLevel;

		private static readonly Regex alpha = new Regex("\\p{Lu}|\\p{Ll}", RegexOptions.Compiled);
		private static readonly Regex space = new Regex("\\s", RegexOptions.Compiled);
		private static readonly Regex dec = new Regex("\\p{Nd}", RegexOptions.Compiled);
		private static readonly Regex hira = new Regex("\\p{IsHiragana}", RegexOptions.Compiled);
		private static readonly Regex kata = new Regex("\\p{IsKatakana}", RegexOptions.Compiled);
		private static readonly Regex kanji = new Regex("\\p{IsCJKUnifiedIdeographs}", RegexOptions.Compiled);
		private static readonly Regex hankaku = new Regex("[\uFF61-\uFF9F]", RegexOptions.Compiled);

		/// <summary>
		/// �O�X���̏����擾���܂��B
		/// </summary>
		public ThreadHeader Item
		{
			get
			{
				return header;
			}
		}

		/// <summary>
		/// ��v���f�P�ꐔ���擾�܂��͐ݒ肵�܂��B0�ȉ��̏ꍇ�A�����Ŕ��f�B
		/// </summary>
		public int MatchLevel
		{
			set
			{
				matchLevel = Math.Max(0, value);
			}
			get
			{
				return matchLevel;
			}
		}

		/// <summary>
		/// �p�^�[���Ɉ�v�������X���Ǝv����X���b�h�������ׂĊi�[���� List ���擾���܂��B
		/// </summary>
		public List<ThreadHeader> MatchItems
		{
			get {
				return matchItems;
			}
		}

		/// <summary>
		/// ���X�����`�F�b�N���ł���� true�A����ȊO�� false ��Ԃ��܂��B
		/// </summary>
		public bool IsChecking
		{
			get
			{
				return (thread != null) ? thread.IsAlive : false;
			}
		}

		/// <summary>
		/// MatchLevel �v���p�e�B�̒l�� 0 (����) �ɂ����ꍇ�̂݁A��v���x����������ꍇ�� true�A��ł���v�����X�����܂߂�Ȃ� false�B
		/// </summary>
		public bool HighLevelMatching { get; set; }

		/// <summary>
		/// ���X���̌����ɐ��������Ƃ��ɔ������܂��B
		/// </summary>
		public event ThreadHeaderEventHandler Success;

		/// <summary>
		/// NextThreadChecker
		/// </summary>
		public NextThreadChecker()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			matchItems = new List<ThreadHeader>();
			matchLevel = 0;
			thread = null;
		}

		/// <summary>
		/// �ŐV�̃X���b�h�ꗗ���擾���Aitem �̎��X���Ǝv����X���b�h���������܂��B
		/// </summary>
		/// <param name="item">���X�����`�F�b�N����X���b�h�B</param>
		public void Check(ThreadHeader item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (IsChecking)
			{
				throw new InvalidOperationException("�`�F�b�N���ł�");
			}

			header = item;
			matchItems.Clear();

			Checking();
		}

		/// <summary>
		/// �񓯊��Ń`�F�b�N���n�߂�
		/// </summary>
		/// <param name="item"></param>
		public void CheckBegin(ThreadHeader item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (IsChecking)
			{
				throw new InvalidOperationException("�`�F�b�N���ł�");
			}

			header = item;
			header.BoardInfo.Bbs = EnsureCurrentBbs(header.BoardInfo.Bbs);
			matchItems.Clear();

			thread = new Thread(Checking);
			thread.Name = "NST_" + item.Key;
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// ��Ɍ��s�X��������悤�ɂ���
		/// </summary>
		private BbsType EnsureCurrentBbs(BbsType bbs)
		{
			return bbs.Equals(BbsType.X2chAuthenticate) ? BbsType.X2ch : bbs;
		}

		/// <summary>
		/// �`�F�b�N���I�������������ɌĂ�
		/// </summary>
		public void CheckEnd()
		{
			if (IsChecking && thread != null)
				thread.Abort();
		}

		/// <summary>
		/// items ���������Aheader �̎��X���Ǝv����X���b�h�����ׂĕԂ��܂��B
		/// </summary>
		/// <param name="header"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public List<ThreadHeader> Check(ThreadHeader header,
			List<ThreadHeader> items)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			List<ThreadHeader> result = new List<ThreadHeader>();
			List<__WordSet> wordList = GetWords(header.Subject);

			bool isAutoLevel = (MatchLevel <= 0);
			int currentLevel = isAutoLevel ? 5 : this.MatchLevel;
			int maxLevel = 0;

			foreach (__WordSet s in wordList)
				Console.Write("{0}:{1}, ", s.Key, s.Value);
			Console.WriteLine();

			do
			{
				Console.WriteLine("Level={0}", currentLevel);
				foreach (ThreadHeader item in items)
				{
					int matchCount = currentLevel;

					if (IsMatch(wordList, item, ref matchCount))
					{
						maxLevel = Math.Max(maxLevel, matchCount);

						item.Tag = matchCount;
						result.Add(item);
					}
				}
			}
			while (isAutoLevel && result.Count == 0 && --currentLevel >= 2);

			// �ō����x���𒲂ׂāA���̔����ɖ����Ȃ���v�X��������
			int removeLevel = maxLevel / 2;
			result.RemoveAll((s) => (((int)s.Tag) < removeLevel));

			// ��v���x���Ń\�[�g
			ThreadHeader[] temp = result.ToArray();
			Array.Sort(temp, new LevelComparer());

			result.Clear();
			result.AddRange(temp);

			return result;
		}

		/// <summary>
		/// ���X���`�F�b�N�X���b�h
		/// </summary>
		protected virtual void Checking()
		{
			ThreadListReader listReader = null;
			List<ThreadHeader> tempItems = new List<ThreadHeader>();
			BoardInfo board = header.BoardInfo;

			try
			{
				matchItems.Clear();
				listReader = TypeCreator.CreateThreadListReader(board.Bbs);

				if (listReader.Open(board))
				{
					while (listReader.Read(tempItems) != 0)
						;
					matchItems = Check(header, tempItems);
				}
				OnSuccess(this, new ThreadHeaderEventArgs(matchItems));
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
			finally
			{
				if (listReader != null)
					listReader.Close();
			}
		}

		/// <summary>
		/// ���X�����ǂ����𔻒f
		/// </summary>
		/// <param name="sourceItem">�O�X�����</param>
		/// <param name="checkItem">���f����X���b�h�A�C�e��</param>
		/// <param name="level">��v���f�P�ꐔ�i��v�����ꍇ�͈�v���x������������j</param>
		/// <returns>��v�����Ȃ�true</returns>
		protected virtual bool IsMatch(ThreadHeader sourceItem, ThreadHeader checkItem, ref int level)
		{
			return IsMatch(GetWords(sourceItem.Subject), checkItem, ref level);
		}
			
		protected virtual bool IsMatch(List<__WordSet> wordList, ThreadHeader checkItem, ref int level)
		{
			if (checkItem.IsLimitOverThread)
				return false;

			float matchCount = 0;
			string input = checkItem.Subject;
			string backWord = String.Empty;

			foreach (__WordSet ws in wordList)
			{
				WordType type = ws.Key;
				string key = ws.Value;

				// �Ђ炪��1�����͖���
				if (key.Length == 1 && type == WordType.Hira)
					continue;

				if (Regex.IsMatch(input, Regex.Escape(key), RegexOptions.IgnoreCase))
				{
					if (type == WordType.Kanji || type == WordType.Kata || type == WordType.Hankaku || type == WordType.Alpha)
					{
						matchCount += key.Length * 2.0f;
					}
					else
					{
						matchCount += 1;
					}
				}

				// key �𒼑O�̒P�� backWord �ƌ������A�ЂƂ̃��[�h�Ƃ��Č����B
				// "BS11 3691" �� "BS�t�W 1125" �������Ƃ� "BS" �� "11" ����v���A������v���x���Ƃ���Ă��܂���肪������������
				if (backWord != "" && Regex.IsMatch(input, Regex.Escape(backWord + key), RegexOptions.IgnoreCase))
				{
					string longKey = backWord + key;
					matchCount += longKey.Length * 1.5f;
				}
			
				// �X���̃J�E���g+1�@�̐��������݂�����L�͌��c�H
				if (type == WordType.Decimal)
				{
					int dec;
					if (Int32.TryParse(key, out dec))
					{
						dec += 1;
						if (Regex.IsMatch(input, dec + "|" + HtmlTextUtility.HanToZen(dec.ToString())))
							matchCount += 3;
					}
				}

				backWord = key;
			}
		
			// �X���̐���������΂���ق�
			matchCount += new ThreadHeaderInfo(checkItem).ForceValueDay / 10000;

			if (level - matchCount <= 0)
			{
				level = (int)Math.Round(matchCount, MidpointRounding.AwayFromZero);
				Console.WriteLine("{0} level of {1}", level, input);
				return true;
			}

			return false;
		}

		/// <summary>
		/// �A���t�@�x�b�g�A�����A�����A�J�^�J�i�A�Ђ炪�Ȋe�P�ʂ��Ƃ�
		/// �P���؂�o���Ĕz��Ɋi�[
		/// </summary>
		/// <param name="text">�P���؂�o��������</param>
		/// <returns>�P�ꂪ�i�[���ꂽstring�N���X�̔z��</returns>
		public static List<__WordSet> GetWords(string text)
		{
			List<__WordSet> words = new List<KeyValuePair<WordType, string>>();
			WordType wordType = WordType.None;
			int index = 0;

			for (int i = 0; i < text.Length; i++)
			{
				WordType type = WordType.None;
				string ch = text[i].ToString();

				if (alpha.IsMatch(ch))
					type = WordType.Alpha;

				else if (space.IsMatch(ch))
					type = WordType.Space;

				else if (dec.IsMatch(ch))
					type = WordType.Decimal;

				else if (hira.IsMatch(ch))
					type = WordType.Hira;

				else if (kata.IsMatch(ch))
					type = WordType.Kata;

				else if (kanji.IsMatch(ch))
					type = WordType.Kanji;

				else if (hankaku.IsMatch(ch))
					type = WordType.Hankaku;
				
				else if (Char.IsPunctuation(ch[0]) || Char.IsSymbol(ch[0]))
					type = WordType.Symbol;

				if (i == 0)
				{
					wordType = type;
				}
				else if (wordType != type)
				{
					if (wordType != WordType.Space &&
						wordType != WordType.None &&
						wordType != WordType.Symbol)
					{
						string word = text.Substring(index, i - index);
						words.Add(new __WordSet(wordType, word));
					}

					index = i;
					wordType = type;
				}
			}

			if (index < text.Length &&
				wordType != WordType.Space &&
				wordType != WordType.None)
			{
				words.Add(new __WordSet(wordType, text.Substring(index, text.Length - index)));
			}

			return words;
		}

		protected void OnSuccess(object sender, ThreadHeaderEventArgs e)
		{
			if (Success != null)
				Success(sender, e);
		}

		public enum WordType
		{
			None = -1,
			Space = 0,
			Alpha,
			Decimal,
			Hira,
			Kata,
			Kanji,
			Hankaku,
			Symbol,
		}

		private class LevelComparer : IComparer
		{
			public int Compare(object a, object b)
			{
				ThreadHeader itemA = (ThreadHeader)a;
				ThreadHeader itemB = (ThreadHeader)b;

				int levela = (int)itemA.Tag;
				int levelb = (int)itemB.Tag;

				// itemA �� itemB ����ɕ\��������Ȃ�}�C�i�X�̒l�AitemB �̕�����ɕ\��������Ȃ�v���X�̒l��Ԃ��c�H
				int ret = (levelb - levela);
				if (ret == 0)
				{
					// ���x��������̏ꍇ�A���X���ƃX���̐����Ŕ��肷��
					int a_resCount = itemA.ResCount, b_resCount = itemB.ResCount,
						a_no = itemA.No, b_no = itemB.No;
					float a_force = new ThreadHeaderInfo(itemA).ForceValueDay,  b_force = new ThreadHeaderInfo(itemB).ForceValueDay;

					// ���X���͑����ق����D��
					int compareValue = 0;
					if (a_resCount != b_resCount)
						compareValue += (a_resCount - b_resCount) < 0 ? 1 : -1;
					// �����͑����ق����D��
					if (a_force != b_force)
						compareValue += (a_force - b_force) < 0 ? 1 : -1;
					// �X��NO�͏������ق����D��
					if (a_no != b_no)
						compareValue += (a_no - b_no) < 0 ? -1 : 1;

					compareValue += String.Compare(itemA.Subject, itemB.Subject);

					return compareValue;
				}
				else
					return ret;
			}
		}
	}
}
