// ThreadListControl.cs
// #2.0

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Threading;
	using System.Net;
	using Twin.Bbs;
	using Twin.IO;
	using Twin.Text;

	/// <summary>
	/// �X���b�h�ꗗ�𑀍�E�\�����邽�߂̊�{�R���g���[���N���X
	/// </summary>
	public abstract class ThreadListControl : ClientBaseEx<BoardInfo>
	{
		private Thread thread;					// �l�b�g���[�N����f�[�^����M���邽�߂̃X���b�h�ł��B

		private BbsType bbsType;				// ���݊J���Ă���f���̎�ނł��B
		private bool online;					// true �̏ꍇ�͍ŐV�̃X���b�h�ꗗ���擾���Afalse �̏ꍇ�̓L���b�V�����ꂽ�ꗗ��ǂݍ��݂܂��B

		protected List<ThreadHeader> headerList;	// ��M�����ŐV�̃X���b�h�ꗗ���i�[���܂��B

		protected ThreadHeader[] oldItems;			// �L���b�V�����ꂽ�O��̃X���b�h�ꗗ���i�[���܂��B

		// ��{�ƂȂ郊�[�_�[�ł��B
		// online �̒l�� true �̏ꍇ�� networkReader�Afalse �̏ꍇ�� offlineReader ���i�[����܂��B
		protected ThreadListReader baseReader;

		protected ThreadListReader offlineReader;	// �L���b�V�����ꂽ�X���b�h�ꗗ��ǂݍ��ނ��߂̃��[�_�[�̃C���X�^���X�ł��B
		protected ThreadListReader networkReader;	// �ŐV�̃X���b�h�ꗗ��ǂݍ��ނ��߂̃��[�_�[�̃C���X�^���X�ł��B

		protected BoardInfo boardInfo;				// ���݊J���Ă���̏��ł��B���J����Ă��Ȃ��ꍇ�� null �ł��B
		protected bool isOpen;						// ���J����Ă���� true�A����ȊO�� false �ł��B
		protected int bufferSize;					// ���[�_�[�̎�M�o�b�t�@�T�C�Y�ł��B

		private bool canceled = false;				// �X���b�h�ꗗ�̎擾���L�����Z�������ꍇ�� true �ł��B

		/// <summary>
		/// ���݊J���Ă���̏���Ԃ��܂��B�J����Ă��Ȃ��ꍇ�� null �ł��B
		/// </summary>
		public BoardInfo BoardInfo
		{
			get {
				return boardInfo;
			}
		}

		public override BoardInfo HeaderInfo
		{
			get
			{
				return boardInfo;
			}
		}

		/// <summary>
		/// �X���b�h�ꗗ��ǂݍ��ݒ��ł���� true�A����ȊO�� false ��Ԃ��܂��B
		/// </summary>
		public bool IsReading
		{
			get {
				if (thread == null)
					return false;

				lock (thread)
					return thread.IsAlive;
			}
		}

		/// <summary>
		/// �X���b�h�ꗗ���J����Ă���� true�A����ȊO�� false ��Ԃ��܂��B
		/// </summary>
		public bool IsOpen
		{
			get {
				return isOpen;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B�̓T�|�[�g���Ă��܂���B
		/// </summary>
		public bool IsPackageReception
		{
			set {
				//throw new NotSupportedException();
			}
			get {
				return false;
			}
		}

		/// <summary>
		/// �ŐV�̃X���b�h�ꗗ���擾����ꍇ�� true�A
		/// �O��L���b�V�����ꂽ�X���b�h�ꗗ��ǂݍ��ޏꍇ�� false ��ݒ肵�܂��B
		/// </summary>
		public bool Online
		{
			set {
				online = value;
			}
			get {
				return online;
			}
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���ύX�s�ȃX���b�h�ꗗ�̃R���N�V�������擾���܂��B
		/// </summary>
		public ReadOnlyCollection<ThreadHeader> Items
		{
			get {
				return new ReadOnlyCollection<ThreadHeader>(headerList);
			}
		}

		/// <summary>
		/// �O��ǂݍ��܂ꂽ�X���b�h�ꗗ�̔z����擾���܂��B
		/// </summary>
		public ThreadHeader[] OldItems {
			get {
				return oldItems;
			}
		}

		/// <summary>
		/// �I������Ă���A�C�e���R���N�V�������擾���܂��B
		/// </summary>
		public abstract ReadOnlyCollection<ThreadHeader> SelectedItems
		{
			get;
		}

		/// <summary>
		/// ���ڂ��I�����ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		public event EventHandler<ThreadListEventArgs> Selected;

		/// <summary>
		/// �X���b�h������ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		public event EventHandler Closed;

		/// <summary>
		/// ThreadListControl�N���X�̃C���X�^���X���������B
		/// </summary>
		/// <param name="cache"></param>
		protected ThreadListControl(Cache cache) : base(cache)
		{
			headerList = new List<ThreadHeader>();

			// �I�t���C���p�̃��[�_�[���쐬
			offlineReader = new OfflineThreadListReader(cache);

			oldItems = new ThreadHeader[0];

			bbsType = BbsType.None;
			bufferSize = 4096;

			isOpen = false;
			online = true;
		}

		/// <summary>
		/// �w�肵���ɑΉ����郊�[�_�[���쐬���A�J���܂��B
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		private bool OpenReader(BoardInfo board)
		{
			if (board.Bbs != bbsType && online)
			{
				bbsType = board.Bbs;
				networkReader = TypeCreator.CreateThreadListReader(bbsType);
				networkReader.ServerChange += new EventHandler<ServerChangeEventArgs>(OnServerChange);
			}

			baseReader = online ?
				new ThreadListReaderRelay(Cache, networkReader) : offlineReader;

			baseReader.BufferSize = bufferSize;
			baseReader.AutoRedirect = true;
			baseReader.Open(board);

			if (online)
			{
				oldItems = ((ThreadListReaderRelay)baseReader).CacheItems.ToArray();
			}

			return baseReader.IsOpen;
		}

		/// <summary>
		/// �f�[�^��ǂݍ��݁A�i�s��Ԃ�ʒm���܂��B
		/// </summary>
		private List<ThreadHeader> Reading()
		{
			List<ThreadHeader> items = new List<ThreadHeader>();
			int read = -1;

			while (read != 0)
			{
				if (canceled)
					break;

				read = baseReader.Read(items);

				OnReceive(new ReceiveEventArgs(
					baseReader.Length, baseReader.Position, read));

				OnStatusTextChanged(
					String.Format("{0}�� ��M�� ({1}/{2})",
						boardInfo.Name, baseReader.Position, baseReader.Length));
			}

			return items;
		}

		/// <summary>
		/// �ʃX���b�h�Ƃ��Ď�M�������s�����\�b�h�ł��B
		/// </summary>
		private void OpenInternal()
		{
			// ������Ԃ�\��
			CompleteStatus status = CompleteStatus.Success;

			List<ThreadHeader> items = null;

			try {
				OnLoading(new EventArgs());
				
				if (OpenReader(boardInfo))
				{
					items = Reading();

					if (canceled)
						return;

					headerList.AddRange(items);
					Invoke(new WriteListMethodInvoker(WriteInternal), new object[] {items});	
				}
			}
			catch (Exception ex) 
			{
				status = CompleteStatus.Error;
				OnStatusTextChanged(ex.Message);
				TwinDll.Output(ex);
			}
			finally {

				if (canceled)
					status = CompleteStatus.Error;
					
				if (baseReader != null)
					baseReader.Close();

				canceled = false;
				
				if (thread != null)
				{
					lock (thread)
						thread = null;
				}

				OnComplete(new CompleteEventArgs(status));

				if (status == CompleteStatus.Success)
				{
					OnStatusTextChanged(
						String.Format("{0}�̓ǂݍ��݂����� (����: {1})",
							boardInfo.Name, headerList.Count));
				}
			}
		}

		private void WriteInternal(List<ThreadHeader> items)
		{
			WriteBegin();

			Write(items);

			WriteEnd();
		}

		/// <summary>
		/// Closed�C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e"></param>
		protected void OnClosed(EventArgs e)
		{
			if (Closed != null)
				Closed(this, e);
		}

		/// <summary>
		/// �X���b�h�ꗗ�̎�M�p�X���b�h���N�����܂��B
		/// </summary>
		protected void ThreadRun()
		{
			thread = new Thread(OpenInternal);
			thread.Name = "TLC_" + this.boardInfo.Path;
			thread.Priority = Priority;
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// Selected�C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e"></param>
		protected void OnSelected(ThreadListEventArgs e)
		{
			if (Selected != null)
				Selected(this, e);
		}

		/// <summary>
		/// ���ړ]����Ă����ۂɌĂ΂�܂��B
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnServerChange(object sender, ServerChangeEventArgs e)
		{
		}

		/// <summary>
		/// �w�肵���w�b�_�����A�C�e�����X�V���܂��B
		/// </summary>
		/// <param name="header"></param>
		public abstract void UpdateItem(ThreadHeader header);

		/// <summary>
		/// �������݊J�n�O�ɌĂ΂��֐��ł��B
		/// </summary>
		protected virtual void WriteBegin()
		{}

		/// <summary>
		/// �������݊������ɌĂ΂��֐��ł��B
		/// </summary>
		protected virtual void WriteEnd()
		{}

		/// <summary>
		/// �p����ŁAitems ��\�����鏈�����L�q���܂��B
		/// </summary>
		/// <param name="items"></param>
		protected abstract void Write(List<ThreadHeader> items);

		/// <summary>
		/// �w�肵�����J���A�X���b�h�ꗗ���擾���܂��B
		/// </summary>
		/// <param name="board"></param>
		public virtual void Open(BoardInfo board)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (IsReading)
				throw new InvalidOperationException("�X���b�h�ꗗ��ǂݍ��ݒ��ł�");

			if (IsOpen)
				Close();

			isOpen = true;
			boardInfo = board;

			// �J���������s��
			OnStatusTextChanged(board.Name + "���J���Ă��܂�");

			ThreadRun();
		}

		/// <summary>
		/// �X���b�h�ꗗ���ŐV�̏�ԂɍX�V���܂��B
		/// </summary>
		public virtual void Reload()
		{
			if (IsReading)
				return;

			if (boardInfo != null)
				Open(boardInfo);
		}

		/// <summary>
		/// �X���b�h�ꗗ�̓ǂݍ��݂𒆎~���܂��B
		/// </summary>
		public virtual void Stop()
		{
			if (IsReading)
			{
				canceled = true;
				isOpen = false;

				if (baseReader != null)
					baseReader.Cancel();

				thread = null;
				OnStatusTextChanged(boardInfo.Name + "�̓Ǎ��𒆎~");
			}
		}

		/// <summary>
		/// �X���b�h�ꗗ����܂��B
		/// </summary>
		public virtual void Close()
		{
			if (IsOpen)
			{
				Stop();

				OnClosed(new EventArgs());
				OnStatusTextChanged(String.Empty);
			}

			boardInfo = null;
			headerList.Clear();

			isOpen = false;
			baseReader = null;

			oldItems = null;
			oldItems = new ThreadHeader[0];
		}

		//-------------------------------

		/// <summary>
		/// ���݂̃��X�g�ɃA�C�e����ǉ��B
		/// ���X�g��ǂݍ���ł���Œ��ɂ��̃��\�b�h���ĂԂƗ�O�𓊂���B
		/// </summary>
		/// <param name="items"></param>
		public virtual void AddItems(List<ThreadHeader> items)
		{
			if (boardInfo == null) {
				throw new InvalidOperationException("���J����Ă��܂���");
			}
			if (items == null) {
				throw new ArgumentNullException("items");
			}
			if (IsReading) {
				throw new InvalidOperationException("���X�g��ǂݍ��ݒ��ł�");
			}

			headerList.AddRange(items);
			Write(items);
		}

		/// <summary>
		/// �\������Ă��郊�X�g�ꗗ����w�肳�ꂽ�X���b�h���폜
		/// </summary>
		/// <param name="items"></param>
		public virtual void RemoveItems(List<ThreadHeader> items)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// ���݂̃��X�g����āA�w�肵�����X�g�ݒ�B
		/// ���X�g��ǂݍ���ł���Œ��ɂ��̃��\�b�h���ĂԂƗ�O�𓊂���B
		/// </summary>
		/// <param name="items"></param>
		public virtual void SetItems(BoardInfo board, List<ThreadHeader> items)
		{
			if (board == null) {
				throw new ArgumentNullException("board");
			}
			if (items == null) {
				throw new ArgumentNullException("items");
			}
			if (IsReading) {
				throw new InvalidOperationException("���X�g��ǂݍ��ݒ��ł�");
			}

			if (IsOpen)
				Close();

			isOpen = true;
			boardInfo = board;
			headerList.AddRange(items);

			WriteBegin();
			Write(items);
			WriteEnd();
		}

		/// <summary>
		/// �p����ŃI�[�o�[���C�h�����΁A�\�����̃X���b�h�ꗗ�����
		/// </summary>
		public virtual void Print()
		{
			throw new NotSupportedException("����̓T�|�[�g����Ă��܂���");
		}

//		/// <summary>
//		/// �w�肵���̏������ݗ����ꗗ��\��
//		/// </summary>
//		/// <param name="board"></param>
//		public abstract void OpenHistory(BoardInfo board);

		/// <summary>
		/// �������邽�߂̃I�u�W�F�N�g��Ԃ�
		/// </summary>
		/// <returns></returns>
		public abstract AbstractSearcher BeginSearch();
	}
}
