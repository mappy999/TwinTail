// WroteThread.cs

namespace Twin
{
	using System;

	/// <summary>
	/// スレッドの履歴を表す
	/// </summary>
	public class WroteThread
	{
		private WroteResCollection wroteResCollection;
		private string key;
		private string subject;
		private Uri uri;

		/// <summary>
		/// レス履歴のコレクションを取得
		/// </summary>
		public WroteResCollection ResItems {
			get {
				return wroteResCollection;
			}
		}

		/// <summary>
		/// レスのタイトルを取得
		/// </summary>
		public string Subject {
			set {
				if (value == null) {
					throw new ArgumentNullException("Subject");
				}
				subject = value;
			}
			get {
				return subject;
			}
		}

		/// <summary>
		/// このスレッドのURLを取得
		/// </summary>
		public Uri Uri {
			set {
				if (value == null) {
					throw new ArgumentNullException("Uri");
				}
				uri = value;
			}
			get {
				return uri;
			}
		}

		/// <summary>
		/// スレッドの番号を取得
		/// </summary>
		public string Key {
			set {
				if (key == null) {
					throw new ArgumentNullException("Key");
				}
				key = value;
			}
			get {
				return key;
			}
		}

		/// <summary>
		/// WroteThreadクラスのインスタンスを初期化
		/// </summary>
		public WroteThread(ThreadHeader thread) : this()
		{
			if (thread == null) {
				throw new ArgumentNullException("thread");
			}
			
			wroteResCollection = new WroteResCollection();
			subject = thread.Subject;
			key = thread.Key;
			uri = new Uri(thread.Url);
		}

		/// <summary>
		/// WroteThreadクラスのインスタンスを初期化
		/// </summary>
		public WroteThread()
		{			
			wroteResCollection = new WroteResCollection();
			subject = String.Empty;
			key = String.Empty;
			uri = null;
		}
	}
}
