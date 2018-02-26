// MachiThreadListReader.cs

namespace Twin.Bbs
{
	using System;
	using System.Net;
	using System.Text;
	using Twin.Text;
	using Twin.IO;

	/// <summary>
	/// �܂�BBS (www.machi.to) �̃X���b�h�ꗗ��ǂݍ��ދ@�\���
	/// </summary>
	public class MachiThreadListReader : ThreadListReaderBase
	{
		private HttpWebResponse _res = null;

		/// <summary>
		/// �p�[�T���w�肵��MachiThreadListReader�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiThreadListReader(ThreadListParser dataParser) 
			: base(dataParser)
		{
		}

		/// <summary>
		/// MachiThreadListReader�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiThreadListReader() : this(new MachiThreadListParser())
		{
		}

		/// <summary>
		/// ���J��
		/// </summary>
		/// <param name="info"></param>
		public override bool Open(BoardInfo info)
		{
			if (info == null) {
				throw new ArgumentNullException("info");
			}
			if (isOpen) {
				throw new InvalidOperationException("���ɃX�g���[�����J����Ă��܂�");
			}

			// �l�b�g���[�N�X�g���[����������
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(info.Url + "subject.txt");
			req.Timeout = 30000;
			req.UserAgent = UserAgent;
			req.Referer = info.Url;

			req.Headers.Add("Pragma", "no-cache");
			req.Headers.Add("Cache-Control", "no-cache");
			
			_res = (HttpWebResponse)req.GetResponse();
			baseStream = _res.GetResponseStream();

			if (_res.StatusCode == HttpStatusCode.OK)
			{
				position = 0;
				length = (int)_res.ContentLength;

				boardinfo = info;
				isOpen = true;
			}
			else {
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
	}
}
