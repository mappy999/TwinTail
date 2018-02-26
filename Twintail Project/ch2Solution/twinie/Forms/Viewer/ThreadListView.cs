// ThreadListView.cs

namespace Twin.Forms
{
	using System;
	using System.Windows.Forms;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using System.IO;
	using System.Drawing;
	using Twin.IO;
	using Twin.Text;
	using System.Diagnostics;

	/// <summary>
	/// リストビューを使用してスレッド一覧を表示するためのコントロール
	/// </summary>
	public class ThreadListView : ThreadListControl
	{
		private static readonly NGWords defNGWords;
		private NGWords nGWords;

		private DesignSettings.ListDesignSettings listDesign;
		private ViewSettings.ColumnSettings columnSize;
		private BookmarkRoot bookmarkRoot;
		private ForceValueOf forceValueType;
		private OpenMode openmode;
		private SortOrder sortOrder;
		private int prevColumn;

		private ThreadHeader[] sortedOldItems = null;
		private ThreadHeader mostForcibleItem = null;

		protected ListView listView;
		protected string baseFolder;

		private bool disposed = false;

		/// <summary>
		/// このクラスが項目を表示するのに使用しているコントロールを取得
		/// </summary>
		public ListView InnerView
		{
			get
			{
				return listView;
			}
		}

		/// <summary>
		/// スレッドを開く際のマウス操作を取得または設定
		/// </summary>
		public OpenMode OpenMode
		{
			set
			{
				if (openmode != value)
					openmode = value;
			}
			get
			{
				return openmode;
			}
		}

		private bool _ng924 = false;
		/// <summary>
		/// dat番号が924で始まるスレッドをNGにするかどうか
		/// </summary>
		public bool NG924
		{
			get
			{
				return _ng924;
			}
			set
			{
				_ng924 = value;
			}
		}
	

		/// <summary>
		/// リストビューに使用するイメージリストを取得または設定。
		/// 各インデックス番号: 全既得(0) 新着(1)
		/// </summary>
		public ImageList ImageList
		{
			set
			{
				if (value == null)
					throw new ArgumentNullException("ImageList");
				listView.SmallImageList = value;
			}
			get
			{
				return listView.SmallImageList;
			}
		}

		/// <summary>
		/// 選択されている項目を取得
		/// </summary>
		public override ReadOnlyCollection<ThreadHeader> SelectedItems
		{
			get
			{
				List<ThreadHeader> result = new List<ThreadHeader>();
				foreach (ListViewItem lv in listView.SelectedItems)
				{
					ThreadHeader header = (ThreadHeader)lv.Tag;
					result.Add(header);
				}
				return new ReadOnlyCollection<ThreadHeader>(result);
			}
		}

		/// <summary>
		/// 板が移転したときに発生
		/// </summary>
		public event EventHandler<ServerChangeEventArgs> ServerChanged;

		/// <summary>
		/// イメージインデックスの番号を表す
		/// </summary>
		protected struct ImageIndices
		{
			/// <summary>なし</summary>
			public const int None = -1;

			/// <summary>全既得スレッド</summary>
			public const int Complete = 0;
			/// <summary>更新スレッド</summary>
			public const int Update = 1;
			/// <summary>お気に入りスレッド</summary>
			public const int Bookmark = 2;
			/// <summary>お気に入り更新スレッド</summary>
			public const int BookmarkUp = 3;
		}

		/// <summary>
		/// カラムヘッダーの項目番号を表す
		/// </summary>
		public struct ColumnNumbers
		{
			/// <summary>
			/// スレッドの各情報を表す
			/// </summary>
			public const int Info = 0;
			/// <summary>
			/// スレッドの並ぶ順を表す
			/// </summary>
			public const int No = 1;
			/// <summary>
			/// スレッドのタイトルを表す
			/// </summary>
			public const int Subject = 2;
			/// <summary>
			/// レス数を表す
			/// </summary>
			public const int ResCount = 3;
			/// <summary>
			/// 既得済みレス数を表す
			/// </summary>
			public const int GotResCount = 4;
			/// <summary>
			/// 新着レス数を表す
			/// </summary>
			public const int NewResCount = 5;
			/// <summary>
			/// スレッドの勢いを表す
			/// </summary>
			public const int Force = 6;
			/// <summary>
			/// スレッドのサイズを表す
			/// </summary>
			public const int Size = 7;
			/// <summary>
			/// スレッドが立てられた日付を表す
			/// </summary>
			public const int Date = 8;
			/// <summary>
			/// 最終更新日を表す
			/// </summary>
			public const int LastModified = 9;
			/// <summary>
			/// 最終書き込み時間を表す
			/// </summary>
			public const int LastWritten = 10;
			/// <summary>
			/// 板名を表す
			/// </summary>
			public const int BoardName = 11;
		}

		static ThreadListView()
		{
			defNGWords = Twinie.NGWords.Default;
		}

		/// <summary>
		/// ThreadListViewクラスのインスタンスを初期化
		/// </summary>
		public ThreadListView(Cache cache, Settings settings, BookmarkRoot bookmark)
			: base(cache)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//

			DoubleBuffered = true;

			columnSize = settings.View.Columns;
			listDesign = settings.Design.List;

			bookmarkRoot = bookmark;
			openmode = settings.Operate.OpenMode;
			sortOrder = SortOrder.Ascending;
			forceValueType = settings.ForceValueType;
			prevColumn = -1;

			Online = settings.Online;
			IsPackageReception = settings.Net.ListPackageReception;
			bufferSize = settings.Net.BufferSize;

			TabStop = false;

			InitializeListView();
			InitializeColumns();
			SetColumnSize(settings.View.Columns);

			// インデックス化して高速に既得リストを読み込むリーダーに変更
			offlineReader = new GotThreadListReader(cache);

			listView.OwnerDraw = true;
			listView.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(listView_DrawColumnHeader);
			listView.DrawItem += new DrawListViewItemEventHandler(listView_DrawItem);
			listView.DrawSubItem += new DrawListViewSubItemEventHandler(listView_DrawSubItem);
		}

		void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			drawingItem = null;
		}

		void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
		}

		private ColorToFont GetColorInfo(ThreadHeader header, ListViewItem item)
		{
			bool isUpdate = false;
			int resC, gotResC;
			// 更新スレッドかどうか
			if (Int32.TryParse(item.SubItems[ColumnNumbers.ResCount].Text, out resC) &&
				Int32.TryParse(item.SubItems[ColumnNumbers.GotResCount].Text, out gotResC))
			{
				if (resC - gotResC > 0)
					isUpdate = true;
			}				

			ColorToFont color = listDesign.Normal;
			if (isUpdate)
			{
				color = listDesign.Update;
			}
			// 最も勢いのあるすれ
			else if (header == mostForcibleItem)
			{
				color = listDesign.MostForcible;
			}
			// 新着スレ
			else if (header.IsNewThread)
			{
				color = listDesign.NewThread;
			}
			// dat落ちスレ
			else if (header.Pastlog)
			{
				color = listDesign.Pastlog;
			}
			// 全既得スレ
			else if (header.ResCount == header.GotResCount)
			{
				color = listDesign.GotThread;
			}
			// 新着スレッド
			else if (Within24Hours(header))
			{
				color = listDesign.RecentThread;
			}
			return color;
		}

		private ListViewItem drawingItem = null;
		void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			if (drawingItem == e.Item)
				return;

			drawingItem = e.Item;

			#region draw background
			// 描画する項目のサイズ。右側のサブ項目とかぶらないように少し(10px)余白を取る
			Rectangle bounds = e.Bounds;	// NTwin23.101
			bounds.Width -= 10;

			// 色分け表示を行う場合
			if (listDesign.Coloring)
			{
				ThreadHeader header = (ThreadHeader)e.Item.Tag;
				ColorToFont color = GetColorInfo(header, e.Item);
				Color backColor = SystemColors.Window;

				// 偶数行または奇数行の色分け情報を取得
				if (e.ItemIndex % 2 == 0)
				{
					backColor = listDesign.BackColorFirst;
				}
				else
				{
					backColor = listDesign.BackColorSecond;
				}

				if (e.Item.Selected)
				{
					e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Item.Bounds);
				}
				else
				{
					if (SystemColors.Window != backColor)
					{
						using (Brush b = new SolidBrush(backColor))
							e.Graphics.FillRectangle(b, e.Item.Bounds);
					}
					else
					{
						e.Graphics.FillRectangle(SystemBrushes.Window, e.Item.Bounds);
					}
				}
			}
			// 色分けしない場合の選択色はシステム標準で描画する
			else
			{
				if (e.Item.Selected)
				{
					e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Item.Bounds);
				}
			}

			// フォーカスの描画は、項目のサイズより上下左右に1pxずつ大きく描画する
			if (e.Item.Focused)
			{
				Rectangle rc = e.Item.Bounds;
				rc.X--;
				rc.Y--;
				rc.Width += 2;
				rc.Height += 2;
				e.DrawFocusRectangle(rc);
			}
			#endregion

			#region draw foreground

			// 文字列の描画に使用するブラシとフォント
			Brush foreBrush = SystemBrushes.WindowText;
			bool shouldDisposeBrush = false;
		
			Font font = e.Item.Font;
			bool shouldDisposeFont = false;

			StringFormat format = new StringFormat(StringFormat.GenericTypographic);
			format.LineAlignment = StringAlignment.Center;

			try
			{
				ThreadHeader header = (ThreadHeader)e.Item.Tag;
				ColorToFont color = GetColorInfo(header, e.Item);

				// 設定されているフォントが既定フォントと違う場合は新たに作成する
				if (e.Item.Font.Name != listDesign.FontName ||
					e.Item.Font.Height != listDesign.FontSize ||
					e.Item.Font.Style != color.Style)
				{
					color.CreateFont(listDesign);
					font = color.Font;
					shouldDisposeFont = true;
				}
				else
				{
					font = e.Item.Font;
				}

				// 色分け表示を行う場合
				if (listDesign.Coloring)
				{

					if (e.Item.Selected)
					{
						foreBrush = SystemBrushes.HighlightText;
					}
					else
					{
						if (SystemColors.WindowText != color.Color)
						{
							foreBrush = new SolidBrush(color.Color);
							shouldDisposeBrush = true;
						}
						else
						{
							foreBrush = SystemBrushes.WindowText;
						}
					}
				}
				// 色分けしない場合の選択色はシステム標準で描画する
				else
				{
					if (e.Item.Selected)
					{
						foreBrush = SystemBrushes.HighlightText;
					}
				}

				if (e.Item.ImageIndex != -1)
				{
					Image image = e.Item.ImageList.Images[e.Item.ImageIndex];
					Point location = new Point(e.Item.Position.X, e.Bounds.Y);
					location.Y += (e.Bounds.Height - image.Height) / 2;
					e.Graphics.DrawImage(image, location);
				}

				int columnIndex = 0;
				foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
				{
					// カラムに設定されているテキスト配置位置と同じ書式を使用する
					switch (listView.Columns[columnIndex++].TextAlign)
					{
						case HorizontalAlignment.Center:
							format.Alignment = StringAlignment.Center;
							break;
						case HorizontalAlignment.Left:
							format.Alignment = StringAlignment.Near;
							break;
						case HorizontalAlignment.Right:
							format.Alignment = StringAlignment.Far;
							break;
					}

					// 描画する項目のサイズ。右側のサブ項目とかぶらないように少し(10px)余白を取る
					bounds = subItem.Bounds;
					bounds.Width -= 10;

					e.Graphics.DrawString(subItem.Text, font, foreBrush, bounds, format);
				}
			}
			finally
			{
				format.Dispose();
				if (shouldDisposeFont)
					font.Dispose();
				if (shouldDisposeBrush)
					foreBrush.Dispose();
			}
			#endregion
		}

		/// <summary>
		/// 使用しているリソースを解放
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				try
				{
					if (disposing)
					{
						offlineReader = null;
						listView.Dispose();
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
			disposed = true;
		}

		#region Initialize ListView
		private void InitializeListView()
		{
			listView = new TwinListView();
			listView.FullRowSelect = true;
			listView.HideSelection = false;
			listView.MouseDown += new MouseEventHandler(listView_MouseDown);
			listView.MouseMove += new MouseEventHandler(listView_MouseMove);
			listView.MouseUp += new MouseEventHandler(listView_MouseUp);
			listView.KeyPress += new KeyPressEventHandler(listView_KeyPress);
			listView.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
			listView.Dock = DockStyle.Fill;
			listView.View = View.Details;
			listView.BackColor = listDesign.BackColorFirst;
			listView.Font = new Font(listDesign.FontName, listDesign.FontSize);
			listView.TabIndex = 0;
			listView.AllowColumnReorder = true;
			listView.ColumnReordered += new ColumnReorderedEventHandler(listView_ColumnReordered);
			Controls.Add(listView);
		}
		#endregion

		#region ListView Events
		/// <summary>
		/// ドラッグを開始する。正常にドロップできればtrueを返す。
		/// </summary>
		/// <returns></returns>
		private bool BeginDrag()
		{
			// ドラッグ開始
			ThreadHeader[] array = new ThreadHeader[SelectedItems.Count];
			SelectedItems.CopyTo(array, 0);

			DragDropEffects effects = DoDragDrop(array, DragDropEffects.Copy);
			return (effects == DragDropEffects.Copy) ? true : false;
		}

		private void listView_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				OnSelected(new ThreadListEventArgs(SelectedItems));
				e.Handled = true;
			}
		}

		bool __lockMouseEvent = false;
		private void listView_MouseDownUpInternal(MouseEventArgs e)
		{
			if (__lockMouseEvent == false)
			{
				// 2012/1/16
				// なぜか、CacheSearchDialogによって開かれた検索結果を表示するリストビューのみ、マウスイベントが2回発生してしまう不具合があり、
				// スレッドを開く処理が2重になってしまう。原因は謎…。
				// こんなフラグを入れることでしか解決できなかった。。
				__lockMouseEvent = true;

				try
				{
					if (e.Button != MouseButtons.Right && ModifierKeys == Keys.None)
					{
						listView.SelectedItems.Clear();
						ListViewItem item = listView.GetItemAt(e.X, e.Y);

						if (item != null)
						{
							item.Selected = true;

							if (e.Button == MouseButtons.Left)
							{
								OnSelected(new ThreadListEventArgs(SelectedItems));
							}
						}
					}
				}
				finally
				{
					__lockMouseEvent = false;
				}
			}
		}

		protected virtual void listView_MouseDown(object sender, MouseEventArgs e)
		{
			if ((openmode == OpenMode.DoubleClick && e.Clicks >= 2))
				listView_MouseDownUpInternal(e);
		}

		protected virtual void listView_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && (ModifierKeys & Keys.Control) != 0)
			{
				BeginDrag();
			}
		}

		protected virtual void listView_MouseUp(object sender, MouseEventArgs e)
		{
			if ((openmode == OpenMode.SingleClick && e.Clicks == 1))
				listView_MouseDownUpInternal(e);
		}

		protected virtual void listView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column != prevColumn)
			{
				sortOrder = SortOrder.Ascending;
				prevColumn = e.Column;
			}

			ListViewItem[] keys = new ListViewItem[listView.Items.Count];
			listView.Items.CopyTo(keys, 0);

			// ソートを実行
			listView.BeginUpdate();

			listView.Items.Clear();
			System.Collections.IComparer comparer = new ListViewItemComparer(sortOrder, e.Column);

			Array.Sort(keys, comparer);

			for (int i = 0; i < keys.Length; i++)
				SetItemBackColor(i, keys[i]);

			listView.Items.AddRange(keys);

			listView.EndUpdate();

			// ソートの方法（昇順と降順）を切り替える
			sortOrder = (sortOrder == SortOrder.Ascending) ?
				SortOrder.Descending : SortOrder.Ascending;

			//			listView.Sorting = sortOrder;
			//			listView.ListViewItemSorter = comparer;
		}


		void listView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
		{			
		}

		#endregion

		#region Privateメソッド
		/// <summary>
		/// 指定したリストビューアイテムの色を設定
		/// </summary>
		/// <param name="item"></param>
		private void SetItemDesign(ThreadHeader header, ListViewItem item)
		{
			// 新着スレ
			if (header.IsNewThread)
			{
//				item.Font = listDesign.NewThread.Font;
//				item.ForeColor = listDesign.NewThread.Color;
				item.ImageIndex = ImageIndices.None;
			}
			// dat落ちスレ
			else if (header.Pastlog)
			{
//				item.Font = listDesign.Pastlog.Font;
//				item.ForeColor = listDesign.Pastlog.Color;
				item.ImageIndex = ImageIndices.Complete;
			}
			// 全既得スレ
			else if (header.ResCount == header.GotResCount)
			{
//				item.Font = listDesign.GotThread.Font;
//				item.ForeColor = listDesign.GotThread.Color;
				item.ImageIndex = ImageIndices.Complete;
			}
			// 更新スレ
			else if (header.SubNewResCount > 0)
			{
//				item.Font = listDesign.Update.Font;
//				item.ForeColor = listDesign.Update.Color;
				item.ImageIndex = ImageIndices.Update;
			}
			// 新着スレッド
			else if (Within24Hours(header))
			{
//				item.Font = listDesign.RecentThread.Font;
//				item.ForeColor = listDesign.RecentThread.Color;
				item.ImageIndex = ImageIndices.None;
			}
			// 通常スレ
			else
			{
//				item.Font = listDesign.Normal.Font;
//				item.ForeColor = listDesign.Normal.Color;
				item.ImageIndex = ImageIndices.None;
			}

			// お気に入りスレかどうかを判断
			if (bookmarkRoot.Contains(header))
			{
				item.ImageIndex = (header.SubNewResCount > 0) ?
					ImageIndices.BookmarkUp : ImageIndices.Bookmark;
			}
		}

		/// <summary>
		/// itemの背景色を設定
		/// </summary>
		/// <param name="item"></param>
		private void SetItemBackColor(int index, ListViewItem item)
		{
			if (listDesign.Coloring)
			{
				// 背景色の設定
//				item.BackColor = (index % 2) == 0 ?
//					listDesign.BackColorFirst : listDesign.BackColorSecond;
			}
			//			else {
			//				item.BackColor = ListView.DefaultBackColor;
			//			}
		}

		/// <summary>
		/// dateが24時間以内を表しているかどうかを判断
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private bool Within24Hours(ThreadHeader h)
		{
			ThreadHeaderInfo info = new ThreadHeaderInfo(h);
			return info.Within24Hours;
		}

		/// <summary>
		/// カラムを初期化
		/// </summary>
		private void InitializeColumns()
		{
			#region
			ColumnHeader columnInfo = new ColumnHeader();
			ColumnHeader columnNo = new ColumnHeader();
			ColumnHeader columnSubj = new ColumnHeader();
			ColumnHeader columnRes = new ColumnHeader();
			ColumnHeader columnGot = new ColumnHeader();
			ColumnHeader columnNew = new ColumnHeader();
			ColumnHeader columnForce = new ColumnHeader();
			ColumnHeader columnSize = new ColumnHeader();
			ColumnHeader columnDate = new ColumnHeader();
			ColumnHeader columnLastModified = new ColumnHeader();
			ColumnHeader columnLastWritten = new ColumnHeader();
			ColumnHeader columnBoard = new ColumnHeader();

			columnInfo.Text = "!";
			columnInfo.Width = 15;

			columnNo.Text = "No.";
			columnNo.Width = 30;
			columnNo.TextAlign = HorizontalAlignment.Right;

			columnSubj.Text = "タイトル";
			columnSubj.Width = 250;

			columnRes.Text = "レス数";
			columnRes.Width = 45;
			columnRes.TextAlign = HorizontalAlignment.Right;

			columnGot.Text = "既得";
			columnGot.Width = 40;
			columnGot.TextAlign = HorizontalAlignment.Right;

			columnNew.Text = "新着";
			columnNew.Width = 40;
			columnNew.TextAlign = HorizontalAlignment.Right;

			columnForce.Text = "勢い";
			columnForce.Width = 40;
			columnForce.TextAlign = HorizontalAlignment.Right;

			columnSize.Text = "サイズ";
			columnSize.Width = 50;
			columnSize.TextAlign = HorizontalAlignment.Right;

			columnDate.Text = "日付";
			columnDate.Width = 120;

			columnLastModified.Text = "最終更新日";
			columnLastModified.Width = 120;

			columnLastWritten.Text = "最終書込日";
			columnLastWritten.Width = 120;

			columnBoard.Text = "板名";
			columnBoard.Width = 80;

			listView.Clear();
			listView.Columns.Add(columnInfo);
			listView.Columns.Add(columnNo);
			listView.Columns.Add(columnSubj);
			listView.Columns.Add(columnRes);
			listView.Columns.Add(columnGot);
			listView.Columns.Add(columnNew);
			listView.Columns.Add(columnForce);
			listView.Columns.Add(columnSize);
			listView.Columns.Add(columnDate);
			listView.Columns.Add(columnLastModified);
			listView.Columns.Add(columnLastWritten);
			listView.Columns.Add(columnBoard);
			#endregion
		}

		/// <summary>
		/// カラムサイズを設定
		/// </summary>
		/// <param name="colSize"></param>
		private void SetColumnSize(ViewSettings.ColumnSettings colSize)
		{
			listView.Columns[ColumnNumbers.Info].Width = colSize.Info;
			listView.Columns[ColumnNumbers.No].Width = colSize.No;
			listView.Columns[ColumnNumbers.Subject].Width = colSize.Subject;
			listView.Columns[ColumnNumbers.ResCount].Width = colSize.ResCount;
			listView.Columns[ColumnNumbers.GotResCount].Width = colSize.GotResCount;
			listView.Columns[ColumnNumbers.NewResCount].Width = colSize.NewResCount;
			listView.Columns[ColumnNumbers.Force].Width = colSize.Force;
			listView.Columns[ColumnNumbers.Size].Width = colSize.Size;
			listView.Columns[ColumnNumbers.Date].Width = colSize.Date;
			listView.Columns[ColumnNumbers.LastModified].Width = colSize.LastModified;
			listView.Columns[ColumnNumbers.LastWritten].Width = colSize.LastWritten;
			listView.Columns[ColumnNumbers.BoardName].Width = colSize.BoardName;

			// NTwin23.101
			listView.Columns[ColumnNumbers.Info].DisplayIndex = colSize.ordInfo;
			listView.Columns[ColumnNumbers.No].DisplayIndex = colSize.ordNo;
			listView.Columns[ColumnNumbers.Subject].DisplayIndex = colSize.ordSubject;
			listView.Columns[ColumnNumbers.ResCount].DisplayIndex = colSize.ordResCount;
			listView.Columns[ColumnNumbers.GotResCount].DisplayIndex = colSize.ordGotResCount;
			listView.Columns[ColumnNumbers.NewResCount].DisplayIndex = colSize.ordNewResCount;
			listView.Columns[ColumnNumbers.Force].DisplayIndex = colSize.ordForce;
			listView.Columns[ColumnNumbers.Size].DisplayIndex = colSize.ordSize;
			listView.Columns[ColumnNumbers.Date].DisplayIndex = colSize.ordDate;
			listView.Columns[ColumnNumbers.LastModified].DisplayIndex = colSize.ordLastModified;
			listView.Columns[ColumnNumbers.LastWritten].DisplayIndex = colSize.ordLastWritten;
			listView.Columns[ColumnNumbers.BoardName].DisplayIndex = colSize.ordBoardName;
			// NTwin23.101
		}

		/// <summary>
		/// 現在のカラムサイズを取得
		/// </summary>
		/// <param name="colSize"></param>
		private void GetColumnSize(ViewSettings.ColumnSettings colSize)
		{
			colSize.Info = listView.Columns[ColumnNumbers.Info].Width;
			colSize.No = listView.Columns[ColumnNumbers.No].Width;
			colSize.Subject = listView.Columns[ColumnNumbers.Subject].Width;
			colSize.ResCount = listView.Columns[ColumnNumbers.ResCount].Width;
			colSize.GotResCount = listView.Columns[ColumnNumbers.GotResCount].Width;
			colSize.NewResCount = listView.Columns[ColumnNumbers.NewResCount].Width;
			colSize.Force = listView.Columns[ColumnNumbers.Force].Width;
			colSize.Size = listView.Columns[ColumnNumbers.Size].Width;
			colSize.Date = listView.Columns[ColumnNumbers.Date].Width;
			colSize.LastModified = listView.Columns[ColumnNumbers.LastModified].Width;
			colSize.LastWritten = listView.Columns[ColumnNumbers.LastWritten].Width;
			colSize.BoardName = listView.Columns[ColumnNumbers.BoardName].Width;

			// NTwin23.101
			colSize.ordInfo = listView.Columns[ColumnNumbers.Info].DisplayIndex;
			colSize.ordNo = listView.Columns[ColumnNumbers.No].DisplayIndex;
			colSize.ordSubject = listView.Columns[ColumnNumbers.Subject].DisplayIndex;
			colSize.ordResCount = listView.Columns[ColumnNumbers.ResCount].DisplayIndex;
			colSize.ordGotResCount = listView.Columns[ColumnNumbers.GotResCount].DisplayIndex;
			colSize.ordNewResCount = listView.Columns[ColumnNumbers.NewResCount].DisplayIndex;
			colSize.ordForce = listView.Columns[ColumnNumbers.Force].DisplayIndex;
			colSize.ordSize = listView.Columns[ColumnNumbers.Size].DisplayIndex;
			colSize.ordDate = listView.Columns[ColumnNumbers.Date].DisplayIndex;
			colSize.ordLastModified = listView.Columns[ColumnNumbers.LastModified].DisplayIndex;
			colSize.ordLastWritten = listView.Columns[ColumnNumbers.LastWritten].DisplayIndex;
			colSize.ordBoardName = listView.Columns[ColumnNumbers.BoardName].DisplayIndex;
			// NTwin23.101
		}
		#endregion

		/// <summary>
		/// 板を開く
		/// </summary>
		/// <param name="board"></param>
		public override void Open(BoardInfo board)
		{
			// NGワード設定を取得
			nGWords = Twinie.NGWords.Get(board, false);

			baseFolder = Cache.GetFolderPath(board);
			prevColumn = -1;
			base.Open(board);
		}

		/// <summary>
		/// リストを更新
		/// </summary>
		public override void Reload()
		{
			listView.Items.Clear();
			base.Reload();
		}

		/// <summary>
		/// 表示をクリア
		/// </summary>
		public override void Close()
		{
			// カラムサイズを保存
			GetColumnSize(columnSize);
			listView.Items.Clear();
			base.Close();
		}

		/// <summary>
		/// 指定したヘッダを最新の状態に更新
		/// </summary>
		/// <param name="header"></param>
		public override void UpdateItem(ThreadHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			ListViewItem lvItem = null;

			foreach (ListViewItem lv in listView.Items)
			{
				if (header.Equals(lv.Tag))
				{
					lvItem = lv;
					break;
				}
			}

			if (lvItem != null)
			{
				int t;
				SetItemDesign(header, lvItem);

				lvItem.SubItems[ColumnNumbers.ResCount].Text =
					StringUtility.StringOf(header.ResCount);

				lvItem.SubItems[ColumnNumbers.GotResCount].Text =
					StringUtility.StringOf(header.GotResCount);

				lvItem.SubItems[ColumnNumbers.NewResCount].Text =
					StringUtility.StringOf(header.NewResCount);

				lvItem.SubItems[ColumnNumbers.Force].Text =
					ThreadHeaderInfo.GetForceValue(header, forceValueType);

				lvItem.SubItems[ColumnNumbers.Size].Text =
					(t = header.GotByteCount / 1024) > 0 ? t + "KB" : String.Empty;

				lvItem.SubItems[ColumnNumbers.LastModified].Text =
					StringUtility.StringOf(header.LastModified, false);

				lvItem.SubItems[ColumnNumbers.LastWritten].Text =
					StringUtility.StringOf(header.LastWritten, false);
			}
		}

		/// <summary>
		/// 書き込み開始
		/// </summary>
		protected override void WriteBegin()
		{
			listView.Items.Clear();

			sortedOldItems = OldItems;
			mostForcibleItem = null;
			//sortedOldItems = new ThreadHeader[oldItems.Length];

			//Array.Copy(OldItems, sortedOldItems, OldItems.Length);
			Array.Sort(sortedOldItems);
		}

		/// <summary>
		/// 書き込み完了
		/// </summary>
		protected override void WriteEnd()
		{
		}

		/// <summary>
		/// 書き込み
		/// </summary>
		/// <param name="items"></param>
		protected override void Write(List<ThreadHeader> items)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				//listView.BeginUpdate();

				List<ListViewItem> lvitems = new List<ListViewItem>(items.Count);
				int itemCount = listView.Items.Count;

				for (int i = 0; i < items.Count; i++)
				{
					ThreadHeader h = items[i];
					ListViewItem lv = NewListViewItem(h);

					if (lv != null)
					{
						SetItemDesign(h, lv);
						SetItemBackColor(itemCount++, lv);

						lvitems.Add(lv);
					}
				}

				listView.Items.AddRange(lvitems.ToArray());
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
			finally
			{
				//listView.EndUpdate();
				Cursor = Cursors.Default;
			}
		}

		private ListViewItem NewListViewItem(ThreadHeader h)
		{
			// NGワード処理
			if (Twinie.Settings.NGWordsOn)
			{
				if (defNGWords.IsMatchSubject(h.Subject) ||
					(nGWords != null && nGWords.IsMatchSubject(h.Subject)))
				{
					return null;
				}
				else if (NG924 && h.Key.StartsWith("924"))
				{
					return null;
				}
			}

			// 新着スレかどうかを判断
			if (sortedOldItems.Length > 0 && Array.BinarySearch(sortedOldItems, h) < 0)
			{
				h.IsNewThread = true;
			}

			ThreadHeader temp = ThreadIndexer.Read(Cache.GetIndexPath(h));

			if (temp != null)
			{
				h.GotResCount = temp.GotResCount;
				h.GotByteCount = temp.GotByteCount;
				h.LastModified = temp.LastModified;
				h.LastWritten = temp.LastWritten;
				h.Pastlog = temp.Pastlog;
			}

			ListViewItem lv = new ListViewItem();
			int size;

			lv.Text = String.Empty;
			lv.SubItems.Add(StringUtility.StringOf(h.No));
			lv.SubItems.Add(StringUtility.Unescape(h.Subject));
			lv.SubItems.Add(h.ResCount.ToString());
			lv.SubItems.Add(StringUtility.StringOf(h.GotResCount));
			lv.SubItems.Add(StringUtility.StringOf(h.SubNewResCount != 0 ? h.SubNewResCount : h.NewResCount));
			lv.SubItems.Add(ThreadHeaderInfo.GetForceValue(h, forceValueType));
			lv.SubItems.Add((size = h.GotByteCount / 1024) > 0 ? (size + "KB") : String.Empty);
			lv.SubItems.Add(StringUtility.StringOf(h.Date, true));
			lv.SubItems.Add(StringUtility.StringOf(h.LastModified, false));
			lv.SubItems.Add(StringUtility.StringOf(h.LastWritten, false));
			lv.SubItems.Add(h.BoardInfo.Name);
			lv.Name = h.Url;
			lv.Tag = h;

			lv.SubItems[ColumnNumbers.Info].Name = "Info";

			MarkMostForcibleItem(h);
			
			return lv;
		}

		private void MarkMostForcibleItem(ThreadHeader h)
		{
			if (h.GotResCount == h.ResCount && h.ResCount >= h.UpperLimitResCount)
				return;

			if (mostForcibleItem != null)
			{
				float a = new ThreadHeaderInfo(h).ForceValueDay,
					b = new ThreadHeaderInfo(mostForcibleItem).ForceValueDay;

				if (a > b)
					mostForcibleItem = h;
			}
			else
			{
				mostForcibleItem = h;
			}
		}

		/// <summary>
		/// 指定したリストコレクションを追加
		/// </summary>
		/// <param name="items"></param>
		public override void AddItems(List<ThreadHeader> items)
		{
			base.AddItems(items);
		}

		/// <summary>
		/// 表示されているリスト一覧から指定されたスレッドを削除
		/// </summary>
		/// <param name="items"></param>
		public override void RemoveItems(List<ThreadHeader> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (IsReading)
			{
				throw new InvalidOperationException("リストを読み込み中です");
			}

			foreach (ThreadHeader header in items)
			{
				ListViewItem removeItem = null;

				foreach (ListViewItem item in listView.Items)
				{
					if (header.Equals(item.Tag))
					{
						removeItem = item;
						break;
					}
				}

				headerList.Remove(header);
				removeItem.Remove();
			}
		}

		/// <summary>
		/// 指定したリストコレクションを設定
		/// </summary>
		/// <param name="items"></param>
		public override void SetItems(BoardInfo board, List<ThreadHeader> items)
		{
			listView.Items.Clear();
			base.SetItems(board, items);
		}

		/// <summary>
		/// 現在のリストを閉じて、指定したリスト設定。
		/// リストを読み込んでいる最中にこのメソッドを呼ぶと例外を投げる。
		/// </summary>
		/// <param name="items"></param>
		public void SetItems(List<ThreadHeader> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (IsReading)
			{
				throw new InvalidOperationException();
			}

			isOpen = true;

			headerList.Clear();
			headerList.AddRange(items);

			WriteBegin();
			Write(items);
			WriteEnd();
		}

		/// <summary>
		/// 検索するためのオブジェクトを初期化
		/// </summary>
		/// <returns></returns>
		public override AbstractSearcher BeginSearch()
		{
			return new ThreadListSearcher(this);
		}

		/// <summary>
		/// 板移転イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnServerChange(object sender, ServerChangeEventArgs e)
		{
			if (ServerChanged != null)
				ServerChanged(this, e);
		}

		/// <summary>
		/// コントロールを選択
		/// </summary>
		public override void _Select()
		{
			listView.Select();
		}
	}
}