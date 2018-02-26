// JbbsThreadReader.cs

namespace Twin.Bbs
{
	using System;
	using System.Text;
	using System.Net;
	using Twin.IO;

	/// <summary>
	/// JbbsThreadReader �̊T�v�̐����ł��B
	/// </summary>
	public class JbbsThreadReader : MachiThreadReader
	{
		private HttpWebResponse _res = null;

		private JbbsErrorStatus _errorStatus = JbbsErrorStatus.None;
		/// <summary>
		/// JBBS���L�̃G���[���b�Z�[�W���擾���܂��B
		/// </summary>
		public JbbsErrorStatus ErrorStatus
		{
			get
			{
				return _errorStatus;
			}
		}
	
		/// <summary>
		/// JbbsThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		public JbbsThreadReader()
			: base(new JbbsThreadParser())
		{
		}

		/// <summary>
		/// �X���b�h���J��
		/// </summary>
		/// <param name="th"></param>
		public override bool Open(ThreadHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (IsOpen)
			{
				throw new InvalidOperationException("���ɃX�g���[�����J����Ă��܂�");
			}

			string url = header.DatUrl;

			if (header.GotByteCount > 0)
			{
				// �����擾�p��URL�ɏC����������
				url += String.Format("{0}-n", header.GotResCount + 1);
			}

			// �l�b�g���[�N�X�g���[����������
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.Timeout = 15000;
			req.IfModifiedSince = header.LastModified;
			req.Referer = url;
			req.UserAgent = UserAgent;

			req.Headers.Add("Pragma", "no-cache");
			req.Headers.Add("Cache-Control", "no-cache");

			_res = (HttpWebResponse)req.GetResponse();

			baseStream = _res.GetResponseStream();
			headerInfo = header;

			this._errorStatus = ParseErrorStatus(_res.Headers["ERROR"]);

			// OK
			if (_res.StatusCode == HttpStatusCode.OK)
			{
				position = 0;
				length = (int)_res.ContentLength;
				index = header.GotResCount + 1;

				headerInfo.LastModified = _res.LastModified;
				isOpen = true;
			}
			else
			{
				if (_errorStatus == JbbsErrorStatus.StorageIn)
				{
					// dat���������\��
					headerInfo.Pastlog = true;
				}
				_res.Close();
				_res = null;
			}


			return isOpen;
		}

		public override void Close()
		{
			base.Close();
			if (_res != null)
			{
				_res.Close();
				_res = null;
			}
		}

		/// <summary>
		/// ������΂��Ԃ��G���[�X�e�[�^�X����͂��āAJbbsErrorStatus �񋓑̂ɕϊ����܂��B
		/// </summary>
		/// <param name="errorStatusString"></param>
		/// <returns></returns>
		protected virtual JbbsErrorStatus ParseErrorStatus(string errorStatusString)
		{
			if (!String.IsNullOrEmpty(errorStatusString))
			{
				errorStatusString = errorStatusString.Replace(" ", String.Empty);

				try
				{
					return (JbbsErrorStatus)Enum.Parse(
						typeof(JbbsErrorStatus), errorStatusString, true);
				}
				// ��`����Ă��Ȃ��l�̏ꍇ�͖���
				catch (ArgumentException)
				{
				}
			}
			return JbbsErrorStatus.None;
		}
	}

	public enum JbbsErrorStatus
	{
		/// <summary>��������܂���</summary>
		None,
		/// <summary>�f���ԍ����s�����A�܂��̓p�����[�^���Ԉ���Ă��܂�</summary>
		BBSNotFound,
		/// <summary>�X���b�h�ԍ����s�����A�܂��̓p�����[�^���Ԉ���Ă��܂�</summary>
		KeyNotFound,
		/// <summary>URL���Ԉ���Ă��邩 �ߋ����O�Ɉړ������ɍ폜����Ă��܂�</summary>
		ThreadNotFound,
		/// <summary>�Y���̃X���b�h�́A�ߋ����O�q�ɂɈړ�����Ă��܂�</summary>
		StorageIn,
	}
}

