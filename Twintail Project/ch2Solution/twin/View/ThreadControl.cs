// ThreadControl.cs

namespace Twin
{
	using System;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using System.Threading;
	using System.Net;
//	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Twin.Bbs;
	using Twin.IO;
	using Twin.Text;
using System.Xml;

	/// <summary>
	/// �X���b�h�𑀍�E�\�����邽�߂̊�{�R���g���[���N���X
	/// </summary>
	public abstract class ThreadControl : ClientBaseEx<ThreadHeader>
	{
		private BbsType bbsType;
		private BoardInfo retryServer;

		private bool isPackageReception;
		private bool useGzip;

		protected ResSetCollection resCollection;
		protected SortedValueCollection<int> indicesValues;

		protected Thread thread;
		protected ThreadReader reader;
		protected ThreadStorage storage;
		protected ThreadHeader headerInfo;
		protected bool modeOpen;				// true=Open, false=Reload
		protected int bufferSize;
		protected bool canceled = false;		// �ʐM�������L�����Z�����ꂽ���� true �ɂȂ�
		protected bool retried = false;

		private object syncObject = new object();


		private DateTime lastCompletedDateTime = DateTime.MinValue;
		/// <summary>
		/// �O�񏈗��������̎��Ԃ��擾���܂��B
		/// </summary>
		public DateTime LastCompletedDateTime
		{
			get
			{
				return lastCompletedDateTime;
			}
		}
	

		private bool aboneDetected = false;
		/// <summary>
		/// �X���b�h�̎�M���ɂ��ځ[������m�����ꍇ�Atrue ��Ԃ��܂��B����ȊO�͏�� false �ł��B
		/// </summary>
		public bool AboneDetected
		{
			get
			{
				return aboneDetected;
			}
		}

	
		/// <summary>
		/// ���݊J���Ă���X���b�h�̃w�b�_�����擾
		/// (�X���b�h���J����Ă��Ȃ����null��Ԃ�)
		/// </summary>
		public override ThreadHeader HeaderInfo
		{
			get
			{
				return headerInfo;
			}
		}

		private bool __isReading = false;
		/// <summary>
		/// �X���b�h�̓ǂݍ��ݏ������̏ꍇ�� true�B���̎��̓X���b�h���N�����Ă��āA�f�[�^�ɕύX���������Ă����ԁB
		/// </summary>
		public bool IsReading
		{
			protected set
			{
				if (value && IsOpen == false)
					throw new InvalidOperationException("�܂��X���b�h���J����Ă��܂���");
				__isReading = value;
			}
			get
			{
				return __isReading;
			}
		}

		private bool isWaiting = false;
		/// <summary>
		/// �ǂݍ��ݏ�����ҋ@�����ǂ����𔻒f�BOpen���\�b�h���Ă΂�Ă��� OnLoad���\�b�h���Ă΂��܂ł̊Ԃ� true�B����ȊO�� false�B
		/// </summary>
		public bool IsWaiting
		{
			get
			{
				return isWaiting;
			}
		}

		private bool __isOpen = false;
		/// <summary>
		/// �X���b�h���J����Ă��邩�ǂ����𔻒f�BOpen���\�b�h���Ă΂�Ă���AClose���\�b�h���Ă΂��܂ł̊Ԃ� true�B����ȊO�� false�B
		/// </summary>
		public bool IsOpen
		{
			protected set
			{
#if DEBUG
				if (IsReading)
					throw new InvalidOperationException("IsReading == true �̎��ɂ��̕ϐ���ύX����̂��������ł�");
#endif
				__isOpen = value;
			}
			get
			{
				return __isOpen;
			}
		}

		/// <summary>
		/// �ꊇ��M���s�����ǂ������擾�܂��͐ݒ�
		/// </summary>
		public bool IsPackageReception
		{
			set
			{
				if (isPackageReception != value)
					isPackageReception = value;
			}
			get
			{
				return isPackageReception;
			}
		}

		/// <summary>
		/// �L���b�V����Gzip���k���邩�ǂ������擾�܂��͐ݒ�
		/// </summary>
		public bool UseGzip
		{
			set
			{
				if (useGzip != value)
					useGzip = value;
			}
			get
			{
				return useGzip;
			}
		}

		/// <summary>
		/// �ߋ����O�擾���s���ɍĎ擾�����݂�T�[�o�[�����擾�܂��͐ݒ�
		/// </summary>
		public BoardInfo RetryServer
		{
			set
			{
				retryServer = value;
			}
			get
			{
				return retryServer;
			}
		}

		/// <summary>
		/// �ǂݎ���p�̃��X�R���N�V�������擾
		/// </summary>
		public ReadOnlyResSetCollection ResSets
		{
			get
			{
				return new ReadOnlyResSetCollection(resCollection);
			}
		}

		/// <summary>
		/// �����T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public abstract FontSize FontSize
		{
			set;
			get;
		}

		/// <summary>
		/// �I������Ă��镶������擾
		/// </summary>
		public abstract string SelectedText
		{
			get;
		}

		/// <summary>
		/// �V���܂ŃX�N���[����On�Ȃ� true�AOff�Ȃ� false ��\���B
		/// </summary>
		public abstract bool ScrollToNewRes
		{
			set;
			get;
		}

		/// <summary>
		/// �I�[�g�X�N���[�����L�����ǂ������擾�܂��͐ݒ�
		/// </summary>
		public abstract bool AutoScroll
		{
			set;
			get;
		}

		/// <summary>
		/// �I�[�g�����[�h���L�����ǂ������擾�܂��͐ݒ�
		/// </summary>
		public abstract bool AutoReload
		{
			set;
			get;
		}

		/// <summary>
		/// �\�����X�����擾�܂��͐ݒ�
		/// </summary>
		public abstract int ViewResCount
		{
			set;
			get;
		}

		/// <summary>
		/// ���X�ԍ����N���b�N���ꂽ�Ƃ��ɔ���
		/// </summary>
		public event NumberClickEventHandler NumberClick;

		/// <summary>
		/// URI���N���b�N���ꂽ�Ƃ��ɔ���
		/// </summary>
		public event UriClickEventHandler UriClick;

		/// <summary>
		/// �X���b�h������ꂽ�Ƃ��ɔ���
		/// </summary>
		public event EventHandler Closed;

		/// <summary>
		/// ThreadControl�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		protected ThreadControl(Cache cache)
			: base(cache)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			resCollection = new ResSetCollection();
			indicesValues = new SortedValueCollection<int>();
			bbsType = BbsType.None;
			bufferSize = 4096;
			useGzip = false;
			isPackageReception = false;
		}

		#region Private���\�b�h
		/// <summary>
		/// ���ځ[�񂪔��������Ƃ��ɌĂ΂��
		/// </summary>
		private void OnABoneInternal(object sender, EventArgs e)
		{
			Invoke(new MethodInvoker(OnABone));
		}

		/// <summary>
		/// dat������
		/// </summary>
		private void OnPastlogInternal(object sender, PastlogEventArgs e)
		{
			MethodInvoker m = delegate
			{
				OnPastlog(e);
			};

			Invoke(m);

			if (e.Retry)
			{
				this.retried = true;
			}
		}

		/// <summary>
		/// newbbs�ɑΉ��������[�_�[���쐬
		/// (���ɍ쐬�ς݂ł���Ή������Ȃ�)
		/// </summary>
		/// <param name="newbbs"></param>
		private ThreadReader CreateBaseReader(BbsType newbbs)
		{
			if (newbbs != bbsType)
			{
				bbsType = newbbs;

				reader = TypeCreator.CreateThreadReader(newbbs);

				if (reader is X2chKakoThreadReader)
				{
					X2chKakoThreadReader kako = (X2chKakoThreadReader)reader;
					kako.RetryServers = new BoardInfo[] { retryServer };
				}

				reader.ABone += new EventHandler(OnABoneInternal);
				reader.Pastlog += new EventHandler<PastlogEventArgs>(OnPastlogInternal);
			}
			return reader;
		}

		private void ReadCache(ResSetCollection buff)
		{
			// �V�K�ɊJ���ꍇ�̂݃L���b�V����ǂݍ���
			if (modeOpen)
			{
				if (ThreadIndexer.Exists(Cache, headerInfo))
				{
					ThreadIndexer.Read(Cache, headerInfo);

					try
					{
						storage = new LocalThreadStorage(Cache, headerInfo, StorageMode.Read);
						storage.BufferSize = bufferSize;

						// ���ׂẴ��X��ǂݍ��ݕ\��
						while (storage.Read(buff) != 0)
							;
					}
					finally
					{
						if (storage != null)
						{
							storage.Close();
							storage = null;
						}
					}

					buff.IsNew = false;
				}
			}
		}

		/// <summary>
		/// ���[�_�[���J��
		/// </summary>
		private bool OpenReader()
		{
			aboneDetected = false;
			retried = false;

			// ���擾�X���b�h�Ȃ猻�݂̐ݒ�𔽉f������
			if (!ThreadIndexer.Exists(Cache, headerInfo))
			{
				headerInfo.UseGzip = useGzip;
			}
			// ��{���[�_�[���쐬
			reader = CreateBaseReader(headerInfo.BoardInfo.Bbs);

			// ���[�_�[���J��
			reader.BufferSize = bufferSize;
			reader.Open(headerInfo);

			return reader.IsOpen;
		}

		/// <summary>
		/// �f�[�^��ǂݍ��ށ���������
		/// </summary>
		private void Reading()
		{
			ResSetCollection items = new ResSetCollection(),
				buffer = new ResSetCollection();
			int read = -1, byteParsed, totalByteCount = 0;

			while (read != 0)
			{
				if (canceled)
					return;

				read = reader.Read(buffer, out byteParsed);

				// ���ځ[������m�����ꍇ�A�����𒆎~�B
				if (read == -1)
				{
					aboneDetected = true;
					return;
				}

				totalByteCount += byteParsed;

				items.AddRange(buffer);

				// ������M�̏ꍇ�̓r���[�A�ɏ�������
				if (!isPackageReception)
				{
					if (canceled)
						return;

					Invoke(new WriteResMethodInvoker(WriteInternal), new object[] { buffer });
				}
				buffer.Clear();

				OnReceive(new ReceiveEventArgs(
					reader.Length, reader.Position, read));

				OnStatusTextChanged(
					String.Format("{0} ��M�� ({1}/{2})",
						headerInfo.Subject, reader.Position, reader.Length));
			}

			// �ꊇ��M�̏ꍇ�͂����ň�C�Ƀt���b�V��
			if (isPackageReception)
			{
				if (canceled)
					return;

				Invoke(new WriteResMethodInvoker(WriteInternal), new object[] { items });
			}

			try
			{
				// �X���b�h�̃C���f�b�N�X����ۑ�
				storage = new LocalThreadStorage(Cache, headerInfo, StorageMode.Write);
				storage.BufferSize = bufferSize;
				storage.Write(items);

				headerInfo.GotByteCount += totalByteCount;
				headerInfo.GotResCount += items.Count;
				headerInfo.NewResCount = items.Count;
				ThreadIndexer.Write(Cache, headerInfo);
			}
			catch (Exception ex)
			{
				TwinDll.Output(ex);
			}
			finally
			{
				storage.Close();
			}

			SaveThreadListIndices();
		}

		// �X���b�h�ꗗ�̃C���f�b�N�X��ۑ��B���Ă��邱�Ƃ����܂ɂ���̂ŁA�Đ����ł���悤�ɂ���
		private void SaveThreadListIndices()
		{
			try
			{
				GotThreadListIndexer.Write(Cache, headerInfo);
			}
			catch (XmlException ex)
			{
				if (ex.Message.IndexOf("���[�g�v�f��������܂���") >= 0)
				{
					DialogResult r = MessageBox.Show(
						headerInfo.BoardInfo.Name + "�̃C���f�b�N�X�����Ă��܂��B�������Đ������܂����H\r\n(�₽�玞�Ԃ�����ꍇ������܂�)",
						"�C���f�b�N�X�Ȃ񂾂�`", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (r == DialogResult.Yes)
					{
						try
						{
							ClientBase.Stopper.Reset();
							ThreadIndexer.Indexing(Cache, headerInfo.BoardInfo);
						}
						finally
						{
							ClientBase.Stopper.Set();
						}
					}
				}
			}
		}

		private void WriteInternal(ResSetCollection items)
		{
			resCollection.AddRange(items);
			Write(items);
		}

		private void OpenInternal()
		{
			CompleteStatus status = CompleteStatus.Success;
			try
			{
				try
				{
					isWaiting = true;
					OnLoading(new EventArgs());
				}
				finally
				{
					isWaiting = false;
				}
				if (canceled)
					return;

				// �J���������s��
				if (modeOpen)
					Invoke(new MethodInvoker(Opening));

				// �L���b�V����ǂݍ��ݕ\��
				ReadCache(resCollection);

				if (canceled)
					return;

				Invoke(new MethodInvoker(WriteBegin));

				if (canceled)
					return;

				if (modeOpen)
				{
					Invoke(new WriteResMethodInvoker(Write), new object[] { resCollection });

					// �X�N���[���ʒu�𕜌� (�l��0�̏ꍇ�͕������Ȃ�)
					if (modeOpen && headerInfo.Position != 0.0f)
					{
						Invoke(new PositionMethodInvoker(SetScrollPosition),
							new object[] { headerInfo.Position });
					}
				}

Retry:
				try
				{
					// �T�[�o�[�ɐڑ�
					if (OpenReader())
					{
						if (canceled)
							return;

						Reading();

						// ���ځ[������m�����ꍇ
						if (aboneDetected)
						{
						}
					}
					else
					{
						headerInfo.NewResCount = 0;
						ThreadIndexer.Write(Cache, headerInfo);
					}
				}
				finally
				{
					if (reader != null)
						reader.Close();
				}

				// �Ď��s���v�����ꂽ�ꍇ�A�ŏ�����
				if (retried)
					goto Retry;

				if (canceled)
					return;


				Invoke(new MethodInvoker(WriteEnd));
			}
			catch (Exception ex)
			{
				status = CompleteStatus.Error;
	//			isOpen = false; #10/15
				TwinDll.Output(ex);
				OnStatusTextChanged(ex.Message);
			}
			finally
			{

				// ���~���ꂽ�ꍇ��ETag�����Z�b�g
				if (canceled)
					headerInfo.ETag = String.Empty;

				indicesValues.Clear();

				canceled = false;

				lock (syncObject)
					thread = null;

				if (status == CompleteStatus.Success)
				{
					OnStatusTextChanged(
						String.Format("{0}�̓ǂݍ��݂����� (�V��: {1}��)",
							headerInfo.Subject, headerInfo.NewResCount));
				}
				/** 9/26 �ǉ� **/
				else if (reader is X2chAuthenticateThreadReader)
				{
					X2chRokkaResponseState rokkaState = ((X2chAuthenticateThreadReader)reader).RokkaResponseState;
					if (rokkaState != X2chRokkaResponseState.Success) TwinDll.Output("RokkaResponseState: {0}, URL: {1}, ", rokkaState, headerInfo.Url);
					OnStatusTextChanged(String.Format("RokkaResponseState: {0}", rokkaState));
				}
				/****/

				IsReading = false;
				lastCompletedDateTime = DateTime.Now;

				OnComplete(new CompleteEventArgs(status));
			}
		}
		#endregion

		#region Protected���\�b�h
		/// <summary>
		/// Closed�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnClosed(EventArgs e)
		{
			if (Closed != null)
				Closed(this, e);
		}

		/// <summary>
		/// �ǂݍ��݃X���b�h���J�n
		/// </summary>
		protected void ThreadRun()
		{
			if (IsOpen == false || IsReading == false)
				throw new InvalidOperationException("IsOpen == false || IsReading == false");

			canceled = false;
			thread = new Thread(OpenInternal);
			thread.Name = "TC_" + this.headerInfo.Key;
			thread.Priority = Priority;
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// NumberClick�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnNumberClick(NumberClickEventArgs e)
		{
			if (NumberClick != null)
				NumberClick(this, e);
		}

		/// <summary>
		/// UriClick�C�x���g�𔭐�������
		/// </summary>
		/// <param name="e"></param>
		protected void OnUriClick(UriClickEventArgs e)
		{
			if (UriClick != null)
				UriClick(this, e);
		}

		/// <summary>
		/// �X�N���[���ʒu��ݒ���s��
		/// </summary>
		/// <param name="value"></param>
		protected virtual void SetScrollPosition(float value)
		{
		}

		/// <summary>
		/// ���ځ[�񂪔��������Ƃ��ɌĂ΂��
		/// </summary>
		protected virtual void OnABone()
		{
		}

		/// <summary>
		/// �X���b�h��dat�������Ă���Ƃ��ɌĂ΂��
		/// </summary>
		protected virtual void OnPastlog(PastlogEventArgs e)
		{
		}

		/// <summary>
		/// ���[�_�[��������ɌĂ΂��֐� (Open���\�b�h�ŃX���b�h���J���ꂽ�Ƃ��̂�)
		/// </summary>
		protected virtual void Opening()
		{
		}

		/// <summary>
		/// �������݊J�n���ɌĂ΂��
		/// </summary>
		protected virtual void WriteBegin()
		{
		}

		/// <summary>
		/// �������݊������ɌĂ΂��
		/// </summary>
		protected virtual void WriteEnd()
		{
		}

		/// <summary>
		/// items����������
		/// </summary>
		/// <param name="items"></param>
		protected abstract void Write(ResSetCollection items);
		#endregion

		/// <summary>
		/// �X���b�h���J��
		/// </summary>
		/// <param name="header"></param>
		public virtual void Open(ThreadHeader header, int[] indices)
		{
			indicesValues.Clear();
			if (indices != null)
				indicesValues.AddRange(indices);

			Open(header);
		}

		/// <summary>
		/// �X���b�h���J��
		/// </summary>
		/// <param name="header"></param>
		public virtual void Open(ThreadHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (IsReading)
				throw new InvalidOperationException("�X���b�h��ǂݍ��ݒ��ł�");

			Close();

			// �e�t���O��ݒ�
			IsOpen = true;
			IsReading = true;

			modeOpen = true;
			headerInfo = header;

			// �C���f�b�N�X����ǂݍ���
			if (ThreadIndexer.Exists(Cache, header))
				ThreadIndexer.Read(Cache, header);

			string subj = (header.Subject != String.Empty) ? header.Subject : "[�X���b�h���s��]";
			OnStatusTextChanged(subj + "���J���Ă��܂�");

			ThreadRun();
		}

		/// <summary>
		/// �X���b�h���X�V (�X���b�h��ǂݍ��ݒ��Ȃ牽�����Ȃ�)
		/// </summary>
		public virtual void Reload()
		{
			if (IsReading)
				return;

			if (IsOpen)
			{
				IsReading = true;
				modeOpen = false;	// �X�V����ꍇ��false
				OnStatusTextChanged(headerInfo.Subject + "���X�V���܂�");

				ThreadRun();
			}
			else if (headerInfo != null)
			{
				Open(headerInfo);
			}
		}

		/// <summary>
		/// �X���b�h�̓ǂݍ��݂𒆎~ (�ǂݍ��ݒ��łȂ���Ή������Ȃ�)
		/// </summary>
		public virtual void Stop()
		{
			if (IsReading && !canceled)
			{
				canceled = true;

				if (reader != null)
					reader.Cancel();

				//lock (syncObject)
				//	thread = null;

				if (headerInfo != null)
					OnStatusTextChanged(headerInfo.Subject + "�̓ǂݍ��݂𒆎~");
			}
			IsReading = false;
		}

		/// <summary>
		/// �X���b�h�����
		/// </summary>
		public virtual void Close()
		{
			if (IsOpen)
			{
				Stop();

				ThreadIndexer.IncrementRefCount(Cache, headerInfo);
				OnClosed(new EventArgs());
			}

			IsOpen = false;
			headerInfo = null;
			resCollection.Clear();
		}

		/// <summary>
		/// �w�肵��1����n�܂郌�X�ԍ��̔z����|�b�v�A�b�v�ŕ\��
		/// </summary>
		/// <param name="indices"></param>
		public abstract void Popup(int[] indices);

		/// <summary>
		/// �w�肵�����X�R���N�V�������|�b�v�A�b�v�ŕ\��
		/// </summary>
		/// <param name="resSets"></param>
		public abstract void Popup(ResSetCollection resSets);

		/// <summary>
		/// �w�肵�� index �ԍ����Q�Ƃ��Ă��郌�X���|�b�v�A�b�v�ŕ\���B
		/// </summary>
		/// <param name="index"></param>
		public abstract void PopupBackReferences(int index);

		/// <summary>
		/// �\�����̃X���b�h���������悤�Ɍp����ŃI�[�o�[���C�h
		/// </summary>
		public abstract void Print();

		/// <summary>
		/// �w�肵���ʒu�ɂ���������ނ悤�Ɍp����ŃI�[�o�[���C�h�B
		/// ���ɓ����ԍ��ɂ����肪�ݒ肳��Ă�����A������������B
		/// </summary>
		/// <param name="shiroi"></param>
		public abstract void Bookmark(int shiroi);

		/// <summary>
		/// ��������J��
		/// </summary>
		public abstract void OpenBookmark();

		/// <summary>
		/// �w�肵�� sirusi �ԍ��̃��X�Ɉ��t����B
		/// �����ԍ��Ɉ󂷂�ƁA�󂪉��������B
		/// </summary>
		/// <param name="sirusi"></param>
		public abstract void Sirusi(int sirusi, bool redraw);

		/// <summary>
		/// �󂳂ꂽ���X��\��
		/// </summary>
		public abstract void OpenSirusi();

		/// <summary>
		/// �w�肵���ʒu�ɃX�N���[������悤�Ɍp����ŃI�[�o�[���C�h
		/// </summary>
		/// <param name="position"></param>
		public abstract void ScrollTo(ScrollPosition position);

		/// <summary>
		/// �w�肵�����X�ԍ��܂ŃX�N���[������悤�Ɍp����ŃI�[�o�[���C�h
		/// </summary>
		/// <param name="resNumber"></param>
		public abstract void ScrollTo(int resNumber);

		/// <summary>
		/// �w�肵���͈݂͂̂�\������悤�Ɍp����ŃI�[�o�[���C�h
		/// </summary>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		public abstract void Range(int begin, int end);

		/// <summary>
		/// �w�肵���ʒu�Ɉړ�����悤�Ɍp����ŃI�[�o�[���C�h
		/// </summary>
		/// <param name="movement"></param>
		public abstract void Range(RangeMovement movement);

		/// <summary>
		/// �\������[�N���A���Ďw�肵��items����������
		/// </summary>
		/// <param name="items"></param>
		public abstract void WriteResColl(ResSetCollection items);

		/// <summary>
		/// �w�肵�����������������
		/// </summary>
		/// <param name="html"></param>
		public abstract void WriteText(string text);

		/// <summary>
		/// �\�����N���A
		/// </summary>
		public abstract void Clear();

		/// <summary>
		/// �X���b�h���J�����X�𒊏o
		/// </summary>
		/// <param name="hezder"></param>
		/// <param name="info"></param>
		public abstract int OpenExtract(ThreadHeader hezder, ThreadExtractInfo info);

		/// <summary>
		/// �������邽�߂̃N���X��������
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public abstract AbstractSearcher BeginSearch();

		/// <summary>
		/// ���o���邽�߂̃N���X��������
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="flags"></param>
		/// <param name="modePopup"></param>
		/// <returns></returns>
		public abstract AbstractExtractor BeginExtract();
	}

	/// <summary>
	/// �͈͐ݒ莞�̈ʒu��\��
	/// </summary>
	public enum RangeMovement
	{
		/// <summary>���ֈړ�</summary>
		Back,
		/// <summary>�O�ֈړ�</summary>
		Forward,
	}

	/// <summary>
	/// �X�N���[������ʒu��\��
	/// </summary>
	public enum ScrollPosition
	{
		/// <summary>��ԏ�փX�N���[��</summary>
		Top,
		/// <summary>��ԉ��փX�N���[��</summary>
		Bottom,
		/// <summary>��O�̈ʒu�ɖ߂�</summary>
		Prev,
	}
}
