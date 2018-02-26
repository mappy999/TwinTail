// ThreadHeader.cs

namespace Twin
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// �X���b�h�̊�{�w�b�_����\��
	/// </summary>
	public abstract class ThreadHeader : IComparable
	{
		private BoardInfo board;
		private string subject;
		private string key;
		private string etag;
		private DateTime lastModified;
		private DateTime lastWritten;
		private int resCount;
		private int gotResCount;
		private int gotByteCount;
		private int newResCount;
		private int no;

		private SortedValueCollection<int> sirusi;
		private int refcount;
		private int shiori;
		private bool pastlog;
		private bool newthread;
		private float position;
		private bool useGzip;
		private object tag;

		private int hashcode;

		/// <summary>
		/// �����擾�܂��͐ݒ�
		/// </summary>
		public BoardInfo BoardInfo {
			set {
				if (value == null) {
					throw new ArgumentNullException("Board");
				}
				board = value;
				CalcHash();
			}
			get { return board; }
		}
		
		/// <summary>
		/// �X���b�h�̔ԍ����擾�܂��͐ݒ�
		/// </summary>
		public int No {
			set { no = value; }
			get { return no; }
		}

		/// <summary>
		/// �X���b�h�^�C�g�����擾�܂��͐ݒ�
		/// </summary>
		public string Subject {
			set {
				if (value == null) {
					throw new ArgumentNullException("Subject");
				}
				subject = value;
			}
			get { return subject; }
		}

		/// <summary>
		/// DAT�ԍ����擾�܂��͐ݒ�
		/// </summary>
		public string Key {
			set {
				if (value == null) {
					throw new ArgumentNullException("Key");
				}
				key = value;
				CalcHash();
			}
			get { return key; }
		}

		/// <summary>
		/// ETag���擾�܂��͐ݒ�
		/// </summary>
		public string ETag {
			set {
				etag = value;
				if (etag == null)
					etag = String.Empty;
			}
			get { return etag; }
		}

		/// <summary>
		/// DAT�̑��݂���URL���擾
		/// </summary>
		public abstract string DatUrl {
			get;
		}

		/// <summary>
		/// URL���擾
		/// </summary>
		public abstract string Url {
			get;
		}

		/// <summary>
		/// �X���b�h�����Ă�ꂽ���t���擾
		/// </summary>
		public DateTime Date {
			get { 
				DateTime result = new DateTime(1970, 1, 1);

				try {
					// �X���b�h�����Ă�ꂽ���t���v�Z
					int seconds;
					if (Int32.TryParse(Key, out seconds))
						result = result.AddSeconds(seconds);
				}
				catch (Exception) {}

				return result;
			}
		}

		/// <summary>
		/// �X���b�h�̍ŏI�X�V�����擾�܂��͐ݒ�
		/// </summary>
		public DateTime LastModified {
			set { lastModified = value; }
			get { return lastModified; }
		}

		/// <summary>
		/// �X���b�h�̍ŏI���������擾�܂��͐ݒ�
		/// </summary>
		public DateTime LastWritten {
			set { lastWritten = value; }
			get { return lastWritten; }
		}

		/// <summary>
		/// ���X�����擾�܂��͐ݒ�
		/// </summary>
		public int ResCount {
			set { resCount = value; }
			get { return resCount; }
		}

		/// <summary>
		/// �擾�ς݃��X�����擾�܂��͐ݒ�
		/// </summary>
		public int GotResCount {
			set {
				gotResCount = value; 

				if (gotResCount > resCount)
					resCount = gotResCount;
			}
			get { return gotResCount; }
		}

		/// <summary>
		/// �����ς݃o�C�g�����擾�܂��͐ݒ�
		/// </summary>
		public int GotByteCount {
			set { gotByteCount = value; }
			get { return gotByteCount; }
		}

		/// <summary>
		/// �V�����X�����擾�܂��͐ݒ�
		/// </summary>
		public int NewResCount {
			set { newResCount = value; }
			get { return newResCount; }
		}

		/// <summary>
		/// ���X����������������������擾
		/// </summary>
		public int SubNewResCount {
			get {
				if (GotResCount > 0)
					return (ResCount - GotResCount);
				return 0;
			}
		}

		/// <summary>
		/// ���X���̏�����擾
		/// </summary>
		public abstract int UpperLimitResCount {
			get;
		}

		/// <summary>
		/// �ő僌�X�����z���Ă��邩�ǂ����𔻒f
		/// </summary>
		public bool IsLimitOverThread {
			get { return (ResCount >= UpperLimitResCount); }
		}

		/// <summary>
		/// ���̃X���b�h���Q�Ƃ�����
		/// </summary>
		public int RefCount {
			set { refcount = value; }
			get { return refcount; }
		}

		/// <summary>
		/// ������ԍ����擾�܂��͐ݒ�
		/// </summary>
		public int Shiori {
			set { shiori = Math.Max(0, value); }
			get { return shiori; }
		}

		/// <summary>
		/// �X�N���[���ʒu���擾�܂��͐ݒ�
		/// </summary>
		public float Position {
			set { position = value; }
			get { return position; }
		}

		/// <summary>
		/// �ߋ����O���ǂ�����\���l���擾�܂��͐ݒ�
		/// </summary>
		public bool Pastlog {
			set { pastlog = value; }
			get { return pastlog; }
		}

		/// <summary>
		/// �V���X���b�h���ǂ����������l���擾�܂��͐ݒ�
		/// </summary>
		public bool IsNewThread {
			set { newthread = value; }
			get { return newthread; }
		}

		/// <summary>
		/// Gzip���k�𗘗p���邩�ǂ�����\���l���擾
		/// </summary>
		public bool UseGzip {
			set { useGzip = value; }
			get { return useGzip; }
		}

		/// <summary>
		/// Tag���擾�܂��͐ݒ�
		/// </summary>
		public object Tag {
			set { tag = value; }
			get { return tag; }
		}

		/// <summary>
		/// �󂳂ꂽ���X�ԍ��̃R���N�V�������擾
		/// </summary>
		public SortedValueCollection<int> Sirusi {
			get { return sirusi; }
		}

		/// <summary>
		/// ThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		public ThreadHeader()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			board = new BoardInfo();
			subject = String.Empty;
			key = String.Empty;
			lastModified = new DateTime(0);
			lastWritten = new DateTime(0);
			no = 0;
			resCount = 0;
			gotResCount = 0;
			gotByteCount = 0;
			newResCount = 0;
			position = 0;
			shiori = 0;
			refcount = 0;
			useGzip = false;
			pastlog = false;
			newthread = false;
			etag = null;
			tag = null;
			sirusi = new SortedValueCollection<int>();

			hashcode = -1;
		}

		/// <summary>
		/// ThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="board"></param>
		/// <param name="key"></param>
		public ThreadHeader(BoardInfo board, string key) : this()
		{
			this.board = board;
			this.key = key;
		}

		/// <summary>
		/// ThreadHeader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="board"></param>
		/// <param name="key"></param>
		/// <param name="subject"></param>
		public ThreadHeader(BoardInfo board, string key, string subject) : this(board, key)
		{
			this.subject = subject;
		}

		/// <summary>
		/// ���݂̃C���X�^���X�̒l��header�ɃR�s�[
		/// </summary>
		/// <param name="header"></param>
		public void CopyTo(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}

			header.No = no;
			header.Key = key;
			header.BoardInfo = board;
			header.ETag = etag;
			header.ResCount = resCount;
			header.GotByteCount = gotByteCount;
			header.GotResCount = gotResCount;
			header.LastModified = lastModified;
			header.LastWritten = lastWritten;
			header.NewResCount = newResCount;
			header.Position = position;
			header.Subject = subject;
			header.UseGzip = useGzip;
			header.Shiori = shiori;
			header.Pastlog = pastlog;
			header.newthread = newthread;
			header.refcount = refcount;
			header.Tag = tag;

			sirusi.Copy(header.sirusi);
		}

		/// <summary>
		/// �n�b�V���R�[�h���v�Z (URL�̒l��ύX�����Ƃ��ɌĂԕK�v������)
		/// </summary>
		protected void CalcHash()
		{
			hashcode = Url.GetHashCode();
		}

		/// <summary>
		/// ���̃C���X�^���X�̃n�b�V���l���擾
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return hashcode;
		}

		/// <summary>
		/// ���݂̃C���X�^���X��obj���r
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ThreadHeader);
		}

		/// <summary>
		/// ���݂̃C���X�^���X��header���r
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public bool Equals(ThreadHeader header)
		{
			if (header == null)
				return false;

			if (board.Path == header.board.Path && key == header.key)
				return true;

			else
				return false;
		}

		/// <summary>
		/// ���݂̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("No{0} {1} ({2})",
				no, subject, resCount);
		}
		
		/// <summary>
		/// ���݂̃C���X�^���X��obj���r
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			ThreadHeader h = obj as ThreadHeader;
			if (h == null) return 1;

			return String.Compare(Url, h.Url, true);
		}
	}
}
