// QuickSaveFolderItem.cs

namespace ImageViewerDll
{
	using System;
	using System.Windows.Forms;
	using System.Collections;
	using System.Runtime.Serialization;
	using System.Reflection;

	/// <summary>
	/// クイック保存フォルダを表す
	/// </summary>
	[Serializable]
	public class QuickSaveFolderItem : ISerializable
	{
		private string folderPath;
		private string title;
		private Shortcut shortcut;

		/// <summary>
		/// 保存フォルダのパスを取得または設定
		/// </summary>
		public string FolderPath {
			set {
				if (value == null)
					throw new ArgumentNullException("FolderPath");

				folderPath = value;
			}
			get { return folderPath; }
		}

		/// <summary>
		/// フォルダの別名を取得または設定
		/// </summary>
		public string Title {
			set {
				if (value == null)
					throw new ArgumentNullException("FolderPath");

				title = value;
			}
			get { return title; }
		}

		/// <summary>
		/// ショートカットキーを取得または設定
		/// </summary>
		public Shortcut Shortcut {
			set {
				if (shortcut != value)
					shortcut = value;
			}
			get { return shortcut; }
		}

		/// <summary>
		/// QuickSaveFolderItemクラスのインスタンスを初期化
		/// </summary>
		public QuickSaveFolderItem()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.folderPath = String.Empty;
			this.title = String.Empty;
			this.shortcut = Shortcut.None;
		}

		/// <summary>
		/// QuickSaveFolderItemクラスのインスタンスを初期化
		/// </summary>
		/// <param name="folderPath"></param>
		/// <param name="title"></param>
		/// <param name="expand"></param>
		/// <param name="shortcut"></param>
		public QuickSaveFolderItem(string folderPath, string title, Shortcut shortcut)
		{
			this.folderPath = folderPath;
			this.title = title;
			this.shortcut = shortcut;
		}

		public QuickSaveFolderItem(SerializationInfo info, StreamingContext context)
		{
			CSharpSamples.Serializer.Deserialize(this, info,
				BindingFlags.Instance | BindingFlags.NonPublic);
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			CSharpSamples.Serializer.Serialize(this, info,
				BindingFlags.Instance | BindingFlags.NonPublic);
		}

		/// <summary>
		/// このインスタンスを文字列形式に変換
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return title;
		}
	}
}
