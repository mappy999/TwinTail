// NGWordsManager.cs

namespace Twin
{
	using System;
	using System.IO;
	using System.Collections;
	using CSharpSamples;

	/// <summary>
	/// NGWordsクラスを使用して板ごとにNGワード設定を管理するクラス
	/// </summary>
	public class NGWordsManager
	{
		private const string default_key = "default";
		private string path;

		private Hashtable hash;
		private NGWords defwords;

		/// <summary>
		/// デフォルトのNGワード設定を取得
		/// </summary>
		public NGWords Default {
			get {
				// デフォルトのNGワードが存在しない場合は作成
				if (defwords == null)
				{
					defwords = new NGWords();
					hash.Add(default_key, defwords);
				}

				return defwords;
			}
		}

		/// <summary>
		/// NGWordsManagerクラスのインスタンスを初期化
		/// </summary>
		/// <param name="folderPath">NGワード設定フォルダ</param>
		public NGWordsManager(string folderPath)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			hash = new Hashtable();
			defwords = null;
			path = folderPath;
		}

		/// <summary>
		/// 指定した板のキー名を取得
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		private string GetKey(BoardInfo board)
		{
			if (board != null)
			{
				return board.DomainPath.Replace("/", "-");
			}
			else {
				return default_key;
			}
		}

		/// <summary>
		/// すべてのNGワードを読み込む
		/// </summary>
		public void Load()
		{
			string[] settings = Directory.GetFiles(path);
			hash.Clear();

			foreach (string sett in settings)
			{
				NGWords n = new NGWords(sett);
				string key = Path.GetFileName(sett);

				hash[key] = n;
			}

			if (hash.Contains(default_key))
				defwords = (NGWords)hash[default_key];
		}

		/// <summary>
		/// すべてのNGワードを保存
		/// </summary>
		public void Save()
		{
			foreach (string key in hash.Keys)
			{
				NGWords n = (NGWords)hash[key];
				n.Save(Path.Combine(path, key));
			}
		}

		/// <summary>
		/// 指定した板のNGワードのみ保存
		/// </summary>
		public void Save(BoardInfo bi)
		{
			if (bi == null)
				throw new ArgumentNullException("bi");

			string key = GetKey(bi);

			NGWords n = (NGWords)hash[key];
			n.Save(Path.Combine(path, key));
		}

		/// <summary>
		/// 指定した板のNGワード設定を追加
		/// </summary>
		/// <param name="board">wordを追加する板</param>
		public NGWords Add(BoardInfo board)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			string key = GetKey(board);

			if (!hash.ContainsKey(key))
			{
				NGWords n = new NGWords();
				hash[key] = n;
			}

			return (NGWords)hash[key];
		}

		/// <summary>
		/// 指定した板のNGワード設定を削除
		/// </summary>
		/// <param name="board">NGワード設定を削除する板</param>
		public void Remove(BoardInfo board)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			string key = GetKey(board);
			File.Delete(Path.Combine(path, key));
			hash.Remove(key);
		}

		/// <summary>
		/// 指定した板にNGワード設定が存在するかどうかを判断
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public bool Exists(BoardInfo board)
		{
			string key = GetKey(board);
			return hash.ContainsKey(key);
		}

		/// <summary>
		/// 指定した板のNGワード設定を取得。
		/// </summary>
		/// <param name="board"></param>
		/// <param name="create">設定が存在しない場合に空のNGWordsを作成するかどうか</param>
		/// <returns>指定した板に設定されているNGワード</returns>
		public NGWords Get(BoardInfo board, bool create)
		{
			if (board == null)
				throw new ArgumentNullException("board");

			string key = GetKey(board);
			NGWords nGWords = hash.ContainsKey(key) ? (NGWords)hash[key] : null;

			// 存在しない場合は空の設定を作成
			if (nGWords == null && create)
				nGWords = Add(board);

			return nGWords;
		}

		/// <summary>
		/// すべてのNGワード設定を削除
		/// </summary>
		public void Clear()
		{
			hash.Clear();
		}
	}
}
