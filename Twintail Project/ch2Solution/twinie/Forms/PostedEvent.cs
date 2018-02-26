// PostedEvent.cs

namespace Twin.Forms
{
	using System;

	/// <summary>
	/// 投稿する種類を表す
	/// </summary>
	public enum PostType
	{
		Thread,
		Res,
	}

	/// <summary>
	/// PostedEventHandler デリゲート
	/// </summary>
	public delegate void PostedEventHandler(object sender, PostedEventArgs e);

	/// <summary>
	/// PostEvent の概要の説明です。
	/// </summary>
	public class PostedEventArgs : EventArgs
	{
		private PostType type;
		private BoardInfo boardInfo;
		private ThreadHeader headerInfo;

		private string title;
		private string from;
		private string email;
		private string body;

		/// <summary>
		/// スレ立てかレスかを示す値を取得
		/// </summary>
		public PostType Type {
			get { return type; }
		}

		/// <summary>
		/// スレ立て時はここに板情報が格納される
		/// </summary>
		public BoardInfo BoardInfo {
			get { return boardInfo; }
		}

		/// <summary>
		/// レス書き込み時にはここにスレッド上方が格納される
		/// </summary>
		public ThreadHeader HeaderInfo {
			get { return headerInfo; }
		}

		/// <summary>
		/// 書き込んだスレッド名を取得
		/// </summary>
		public string Subject {
			get { return title; }
		}

		/// <summary>
		/// 投稿者名を取得
		/// </summary>
		public string From {
			get { return from; }
		}

		/// <summary>
		/// E-mailアドレスを取得
		/// </summary>
		public string Email {
			get { return email; }
		}

		/// <summary>
		/// 投稿した本文を取得
		/// </summary>
		public string Body {
			get { return body; }
		}

		/// <summary>
		/// PostedEventArgsクラスのインスタンスを初期化
		/// </summary>
		/// <param name="board"></param>
		/// <param name="thread"></param>
		public PostedEventArgs(BoardInfo board, PostThread thread)
		{
			title = thread.Subject;
			from = thread.From;
			email = thread.Email;
			body = thread.Body;
			boardInfo = board;
			type = PostType.Thread;
		}

		/// <summary>
		/// PostedEventArgsクラスのインスタンスを初期化
		/// </summary>
		/// <param name="header"></param>
		/// <param name="res"></param>
		public PostedEventArgs(ThreadHeader header, PostRes res)
		{
			title = header.Subject;
			from = res.From;
			email = res.Email;
			body = res.Body;
			headerInfo = header;
			type = PostType.Res;
		}
	}
}
