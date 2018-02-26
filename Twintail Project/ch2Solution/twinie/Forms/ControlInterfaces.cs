// ITwinControl.cs

namespace Twin.Forms
{
	using System;
	using Twin.Bbs;
	using System.Drawing;

	/// <summary>
	/// Tabコントロールを操作するインターフェース
	/// </summary>
	public interface ITwinTabController<THeader, TControl>
		where TControl : ClientBase//Ex<THeader>
	{
		/// <summary>
		/// 新しくコントロールを作成しビューアを取得
		/// </summary>
		TControl Create(THeader header, bool newWindow);

		/// <summary>
		/// 指定したスレッド情報を持つコントロールを検索
		/// </summary>
		TControl FindControl(THeader header);

		/// <summary>
		/// 表示されているすべてのコントロールを配列で取得
		/// </summary>
		TControl[] GetControls();

		/// <summary>
		/// ウインドウの情報を格納しているクラスを検索
		/// </summary>
		TwinWindow<THeader, TControl> FindWindow(THeader header);

		/// <summary>
		/// ウインドウの情報を格納しているすべてのクラスを配列で取得
		/// </summary>
		TwinWindow<THeader, TControl>[] GetWindows();

		/// <summary>
		/// 指定したウインドウを放棄
		/// </summary>
		/// <param name="window"></param>
		void Destroy(TControl window);

		/// <summary>
		/// すべてのウインドウを放棄
		/// </summary>
		void Clear();

		/// <summary>
		/// 次のウインドウを選択
		/// </summary>
		/// <param name="next"></param>
		void Select(bool next);

		/// <summary>
		/// 指定したコントロールに文字列を設定
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="text"></param>
		void SetText(TControl ctrl, string text);

		/// <summary>
		/// 選択されているウインドウを取得
		/// </summary>
		TControl Control
		{
			get;
		}

		/// <summary>
		/// 選択されているウインドウのスレッド情報を取得
		/// </summary>
		THeader HeaderInfo
		{
			get;
		}

		/// <summary>
		/// 選択されているウインドウのオブジェクトを取得
		/// </summary>
		TwinWindow<THeader, TControl> Window
		{
			get;
		}

		/// <summary>
		/// スレッドが選択されているかどうかを判断
		/// </summary>
		bool IsSelected
		{
			get;
		}

		/// <summary>
		/// 表示されているウインドウの数を取得
		/// </summary>
		int WindowCount
		{
			get;
		}

		/// <summary>
		/// 表示されているウインドウのインデックスを取得します。
		/// </summary>
		int Index
		{
			get;
		}
	}

	/// <summary>
	/// 板一覧テーブルビューのインターフェース
	/// </summary>
	public interface ITwinTableControl
	{
		/// <summary>
		/// 板名に指定した文字列を含む板を検索
		/// </summary>
		BoardInfo[] Find(string text);

		/// <summary>
		/// 選択されている板を取得または設定
		/// </summary>
		BoardInfo Selected
		{
			set;
			get;
		}

		/// <summary>
		/// 選択されているかどうかを判断
		/// </summary>
		bool IsSelected
		{
			get;
		}
	}

	/// <summary>
	/// ウインドウを表す
	/// </summary>
	public class TwinWindow<THeader, TControl>
		where TControl : ClientBase//Ex<THeader>
	{
		private TControl ctrl;
		private bool visibled;
		private object tag;

		public TControl Control
		{
			get
			{
				return ctrl;
			}
		}
	
		private TabColorSet colorSet = new TabColorSet();
		/// <summary>
		/// ウインドウの配色情報を取得または設定します。
		/// このプロパティはget/set専用で、内部プロパティやResetメソッドを呼び出してはいけない。
		/// </summary>
		public TabColorSet ColorSet
		{
			get
			{
				return colorSet;
			}
			set
			{
				colorSet = value;
			}
		}

		/// <summary>
		/// 自分のレスが参照された時には true になり、タブのアイコンを変更する。一度でもウインドウがアクティブになったり、タブがクリックされれば false。
		/// </summary>
		public bool Referenced { get; set; }

		/// <summary>
		/// 自動で画像URLを開くかどうか。本当はこのクラスに追加するべきではないだろうけど…
		/// </summary>
		public bool AutoImageOpen { get; set; }

		public bool Visibled
		{
			set
			{
				if (visibled != value)
					visibled = value;
			}
			get
			{
				return visibled;
			}
		}

		public object Tag
		{
			set
			{
				tag = value;
			}
			get
			{
				return tag;
			}
		}

		public TwinWindow(TControl viewer)
		{
			this.ctrl = viewer;
			this.visibled = false;
			this.tag = null;
		}
	}

	
}