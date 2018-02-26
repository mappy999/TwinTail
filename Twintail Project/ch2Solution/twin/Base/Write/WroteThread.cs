// WroteThread.cs

namespace Twin
{
	using System;

	/// <summary>
	/// �X���b�h�̗�����\��
	/// </summary>
	public class WroteThread
	{
		private WroteResCollection wroteResCollection;
		private string key;
		private string subject;
		private Uri uri;

		/// <summary>
		/// ���X�����̃R���N�V�������擾
		/// </summary>
		public WroteResCollection ResItems {
			get {
				return wroteResCollection;
			}
		}

		/// <summary>
		/// ���X�̃^�C�g�����擾
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
		/// ���̃X���b�h��URL���擾
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
		/// �X���b�h�̔ԍ����擾
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
		/// WroteThread�N���X�̃C���X�^���X��������
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
		/// WroteThread�N���X�̃C���X�^���X��������
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
