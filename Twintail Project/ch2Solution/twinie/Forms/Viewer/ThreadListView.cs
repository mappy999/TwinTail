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
	/// ���X�g�r���[���g�p���ăX���b�h�ꗗ��\�����邽�߂̃R���g���[��
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
		/// ���̃N���X�����ڂ�\������̂Ɏg�p���Ă���R���g���[�����擾
		/// </summary>
		public ListView InnerView
		{
			get
			{
				return listView;
			}
		}

		/// <summary>
		/// �X���b�h���J���ۂ̃}�E�X������擾�܂��͐ݒ�
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
		/// dat�ԍ���924�Ŏn�܂�X���b�h��NG�ɂ��邩�ǂ���
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
		/// ���X�g�r���[�Ɏg�p����C���[�W���X�g���擾�܂��͐ݒ�B
		/// �e�C���f�b�N�X�ԍ�: �S����(0) �V��(1)
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
		/// �I������Ă��鍀�ڂ��擾
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
		/// ���ړ]�����Ƃ��ɔ���
		/// </summary>
		public event EventHandler<ServerChangeEventArgs> ServerChanged;

		/// <summary>
		/// �C���[�W�C���f�b�N�X�̔ԍ���\��
		/// </summary>
		protected struct ImageIndices
		{
			/// <summary>�Ȃ�</summary>
			public const int None = -1;

			/// <summary>�S�����X���b�h</summary>
			public const int Complete = 0;
			/// <summary>�X�V�X���b�h</summary>
			public const int Update = 1;
			/// <summary>���C�ɓ���X���b�h</summary>
			public const int Bookmark = 2;
			/// <summary>���C�ɓ���X�V�X���b�h</summary>
			public const int BookmarkUp = 3;
		}

		/// <summary>
		/// �J�����w�b�_�[�̍��ڔԍ���\��
		/// </summary>
		public struct ColumnNumbers
		{
			/// <summary>
			/// �X���b�h�̊e����\��
			/// </summary>
			public const int Info = 0;
			/// <summary>
			/// �X���b�h�̕��ԏ���\��
			/// </summary>
			public const int No = 1;
			/// <summary>
			/// �X���b�h�̃^�C�g����\��
			/// </summary>
			public const int Subject = 2;
			/// <summary>
			/// ���X����\��
			/// </summary>
			public const int ResCount = 3;
			/// <summary>
			/// �����ς݃��X����\��
			/// </summary>
			public const int GotResCount = 4;
			/// <summary>
			/// �V�����X����\��
			/// </summary>
			public const int NewResCount = 5;
			/// <summary>
			/// �X���b�h�̐�����\��
			/// </summary>
			public const int Force = 6;
			/// <summary>
			/// �X���b�h�̃T�C�Y��\��
			/// </summary>
			public const int Size = 7;
			/// <summary>
			/// �X���b�h�����Ă�ꂽ���t��\��
			/// </summary>
			public const int Date = 8;
			/// <summary>
			/// �ŏI�X�V����\��
			/// </summary>
			public const int LastModified = 9;
			/// <summary>
			/// �ŏI�������ݎ��Ԃ�\��
			/// </summary>
			public const int LastWritten = 10;
			/// <summary>
			/// ����\��
			/// </summary>
			public const int BoardName = 11;
		}

		static ThreadListView()
		{
			defNGWords = Twinie.NGWords.Default;
		}

		/// <summary>
		/// ThreadListView�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadListView(Cache cache, Settings settings, BookmarkRoot bookmark)
			: base(cache)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
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

			// �C���f�b�N�X�����č����Ɋ������X�g��ǂݍ��ރ��[�_�[�ɕύX
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
			// �X�V�X���b�h���ǂ���
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
			// �ł������̂��邷��
			else if (header == mostForcibleItem)
			{
				color = listDesign.MostForcible;
			}
			// �V���X��
			else if (header.IsNewThread)
			{
				color = listDesign.NewThread;
			}
			// dat�����X��
			else if (header.Pastlog)
			{
				color = listDesign.Pastlog;
			}
			// �S�����X��
			else if (header.ResCount == header.GotResCount)
			{
				color = listDesign.GotThread;
			}
			// �V���X���b�h
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
			// �`�悷�鍀�ڂ̃T�C�Y�B�E���̃T�u���ڂƂ��Ԃ�Ȃ��悤�ɏ���(10px)�]�������
			Rectangle bounds = e.Bounds;	// NTwin23.101
			bounds.Width -= 10;

			// �F�����\�����s���ꍇ
			if (listDesign.Coloring)
			{
				ThreadHeader header = (ThreadHeader)e.Item.Tag;
				ColorToFont color = GetColorInfo(header, e.Item);
				Color backColor = SystemColors.Window;

				// �����s�܂��͊�s�̐F���������擾
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
			// �F�������Ȃ��ꍇ�̑I��F�̓V�X�e���W���ŕ`�悷��
			else
			{
				if (e.Item.Selected)
				{
					e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Item.Bounds);
				}
			}

			// �t�H�[�J�X�̕`��́A���ڂ̃T�C�Y���㉺���E��1px���傫���`�悷��
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

			// ������̕`��Ɏg�p����u���V�ƃt�H���g
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

				// �ݒ肳��Ă���t�H���g������t�H���g�ƈႤ�ꍇ�͐V���ɍ쐬����
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

				// �F�����\�����s���ꍇ
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
				// �F�������Ȃ��ꍇ�̑I��F�̓V�X�e���W���ŕ`�悷��
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
					// �J�����ɐݒ肳��Ă���e�L�X�g�z�u�ʒu�Ɠ����������g�p����
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

					// �`�悷�鍀�ڂ̃T�C�Y�B�E���̃T�u���ڂƂ��Ԃ�Ȃ��悤�ɏ���(10px)�]�������
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
		/// �g�p���Ă��郊�\�[�X�����
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
		/// �h���b�O���J�n����B����Ƀh���b�v�ł����true��Ԃ��B
		/// </summary>
		/// <returns></returns>
		private bool BeginDrag()
		{
			// �h���b�O�J�n
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
				// �Ȃ����ACacheSearchDialog�ɂ���ĊJ���ꂽ�������ʂ�\�����郊�X�g�r���[�̂݁A�}�E�X�C�x���g��2�񔭐����Ă��܂��s�������A
				// �X���b�h���J��������2�d�ɂȂ��Ă��܂��B�����͓�c�B
				// ����ȃt���O�����邱�Ƃł��������ł��Ȃ������B�B
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

			// �\�[�g�����s
			listView.BeginUpdate();

			listView.Items.Clear();
			System.Collections.IComparer comparer = new ListViewItemComparer(sortOrder, e.Column);

			Array.Sort(keys, comparer);

			for (int i = 0; i < keys.Length; i++)
				SetItemBackColor(i, keys[i]);

			listView.Items.AddRange(keys);

			listView.EndUpdate();

			// �\�[�g�̕��@�i�����ƍ~���j��؂�ւ���
			sortOrder = (sortOrder == SortOrder.Ascending) ?
				SortOrder.Descending : SortOrder.Ascending;

			//			listView.Sorting = sortOrder;
			//			listView.ListViewItemSorter = comparer;
		}


		void listView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
		{			
		}

		#endregion

		#region Private���\�b�h
		/// <summary>
		/// �w�肵�����X�g�r���[�A�C�e���̐F��ݒ�
		/// </summary>
		/// <param name="item"></param>
		private void SetItemDesign(ThreadHeader header, ListViewItem item)
		{
			// �V���X��
			if (header.IsNewThread)
			{
//				item.Font = listDesign.NewThread.Font;
//				item.ForeColor = listDesign.NewThread.Color;
				item.ImageIndex = ImageIndices.None;
			}
			// dat�����X��
			else if (header.Pastlog)
			{
//				item.Font = listDesign.Pastlog.Font;
//				item.ForeColor = listDesign.Pastlog.Color;
				item.ImageIndex = ImageIndices.Complete;
			}
			// �S�����X��
			else if (header.ResCount == header.GotResCount)
			{
//				item.Font = listDesign.GotThread.Font;
//				item.ForeColor = listDesign.GotThread.Color;
				item.ImageIndex = ImageIndices.Complete;
			}
			// �X�V�X��
			else if (header.SubNewResCount > 0)
			{
//				item.Font = listDesign.Update.Font;
//				item.ForeColor = listDesign.Update.Color;
				item.ImageIndex = ImageIndices.Update;
			}
			// �V���X���b�h
			else if (Within24Hours(header))
			{
//				item.Font = listDesign.RecentThread.Font;
//				item.ForeColor = listDesign.RecentThread.Color;
				item.ImageIndex = ImageIndices.None;
			}
			// �ʏ�X��
			else
			{
//				item.Font = listDesign.Normal.Font;
//				item.ForeColor = listDesign.Normal.Color;
				item.ImageIndex = ImageIndices.None;
			}

			// ���C�ɓ���X�����ǂ����𔻒f
			if (bookmarkRoot.Contains(header))
			{
				item.ImageIndex = (header.SubNewResCount > 0) ?
					ImageIndices.BookmarkUp : ImageIndices.Bookmark;
			}
		}

		/// <summary>
		/// item�̔w�i�F��ݒ�
		/// </summary>
		/// <param name="item"></param>
		private void SetItemBackColor(int index, ListViewItem item)
		{
			if (listDesign.Coloring)
			{
				// �w�i�F�̐ݒ�
//				item.BackColor = (index % 2) == 0 ?
//					listDesign.BackColorFirst : listDesign.BackColorSecond;
			}
			//			else {
			//				item.BackColor = ListView.DefaultBackColor;
			//			}
		}

		/// <summary>
		/// date��24���Ԉȓ���\���Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private bool Within24Hours(ThreadHeader h)
		{
			ThreadHeaderInfo info = new ThreadHeaderInfo(h);
			return info.Within24Hours;
		}

		/// <summary>
		/// �J������������
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

			columnSubj.Text = "�^�C�g��";
			columnSubj.Width = 250;

			columnRes.Text = "���X��";
			columnRes.Width = 45;
			columnRes.TextAlign = HorizontalAlignment.Right;

			columnGot.Text = "����";
			columnGot.Width = 40;
			columnGot.TextAlign = HorizontalAlignment.Right;

			columnNew.Text = "�V��";
			columnNew.Width = 40;
			columnNew.TextAlign = HorizontalAlignment.Right;

			columnForce.Text = "����";
			columnForce.Width = 40;
			columnForce.TextAlign = HorizontalAlignment.Right;

			columnSize.Text = "�T�C�Y";
			columnSize.Width = 50;
			columnSize.TextAlign = HorizontalAlignment.Right;

			columnDate.Text = "���t";
			columnDate.Width = 120;

			columnLastModified.Text = "�ŏI�X�V��";
			columnLastModified.Width = 120;

			columnLastWritten.Text = "�ŏI������";
			columnLastWritten.Width = 120;

			columnBoard.Text = "��";
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
		/// �J�����T�C�Y��ݒ�
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
		/// ���݂̃J�����T�C�Y���擾
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
		/// ���J��
		/// </summary>
		/// <param name="board"></param>
		public override void Open(BoardInfo board)
		{
			// NG���[�h�ݒ���擾
			nGWords = Twinie.NGWords.Get(board, false);

			baseFolder = Cache.GetFolderPath(board);
			prevColumn = -1;
			base.Open(board);
		}

		/// <summary>
		/// ���X�g���X�V
		/// </summary>
		public override void Reload()
		{
			listView.Items.Clear();
			base.Reload();
		}

		/// <summary>
		/// �\�����N���A
		/// </summary>
		public override void Close()
		{
			// �J�����T�C�Y��ۑ�
			GetColumnSize(columnSize);
			listView.Items.Clear();
			base.Close();
		}

		/// <summary>
		/// �w�肵���w�b�_���ŐV�̏�ԂɍX�V
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
		/// �������݊J�n
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
		/// �������݊���
		/// </summary>
		protected override void WriteEnd()
		{
		}

		/// <summary>
		/// ��������
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
			// NG���[�h����
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

			// �V���X�����ǂ����𔻒f
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
		/// �w�肵�����X�g�R���N�V������ǉ�
		/// </summary>
		/// <param name="items"></param>
		public override void AddItems(List<ThreadHeader> items)
		{
			base.AddItems(items);
		}

		/// <summary>
		/// �\������Ă��郊�X�g�ꗗ����w�肳�ꂽ�X���b�h���폜
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
				throw new InvalidOperationException("���X�g��ǂݍ��ݒ��ł�");
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
		/// �w�肵�����X�g�R���N�V������ݒ�
		/// </summary>
		/// <param name="items"></param>
		public override void SetItems(BoardInfo board, List<ThreadHeader> items)
		{
			listView.Items.Clear();
			base.SetItems(board, items);
		}

		/// <summary>
		/// ���݂̃��X�g����āA�w�肵�����X�g�ݒ�B
		/// ���X�g��ǂݍ���ł���Œ��ɂ��̃��\�b�h���ĂԂƗ�O�𓊂���B
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
		/// �������邽�߂̃I�u�W�F�N�g��������
		/// </summary>
		/// <returns></returns>
		public override AbstractSearcher BeginSearch()
		{
			return new ThreadListSearcher(this);
		}

		/// <summary>
		/// �ړ]�C�x���g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnServerChange(object sender, ServerChangeEventArgs e)
		{
			if (ServerChanged != null)
				ServerChanged(this, e);
		}

		/// <summary>
		/// �R���g���[����I��
		/// </summary>
		public override void _Select()
		{
			listView.Select();
		}
	}
}