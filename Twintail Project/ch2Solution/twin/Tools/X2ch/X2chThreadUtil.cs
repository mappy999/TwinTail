// X2chThreadUtil.cs

namespace Twin
{
	using System;
	using System.Net;
	using System.Text;
	using Twin.Bbs;
	using Twin.Text;
	using Twin.Util;
	using System.IO;

	/// <summary>
	/// X2chThreadUtil �̊T�v�̐����ł��B
	/// </summary>
	public class X2chThreadUtil
	{
		public X2chThreadUtil()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		public static string GetResponseHtml(ThreadHeader header)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(header.Url);
			req.UserAgent = String.Empty;

			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			try
			{

				using (StreamReader r = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("shift_jis")))
				{
					return r.ReadToEnd();
				}
			}
			finally
			{
				res.Close();
			}
		}

		/// <summary>
		/// �ߋ����O�����݂��邩�ǂ����𒲂ׂĂ݂�
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static bool KakologIsExist(ThreadHeader header, out bool gzipCompress)
		{
			X2chKakoThreadHeader kako = new X2chKakoThreadHeader();
			header.CopyTo(kako);

			// �܂�.dat.gz���擾���Ă݂āA���߂Ȃ�.dat���擾����B
			// ����ł����߂Ȃ���߂�B
			kako.GzipCompress = true;
			bool retried = false;
Retry:
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(kako.DatUrl);
			req.UserAgent = TwinDll.UserAgent;
			req.Headers.Add("Accept-Encoding", "gzip");
			req.Method = "HEAD";
			req.AllowAutoRedirect = false;

			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			res.Close();

			if (res.StatusCode == HttpStatusCode.OK)
			{
				gzipCompress = kako.GzipCompress;
				return true;
			}
			else if (!retried)
			{
				kako.GzipCompress = false;
				retried = true;
				goto Retry;
			}

			gzipCompress = false;

			return false;
		}

		/*
		/// <summary>
		/// �w�肵���X���b�h�̏�Ԃ��m�F
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static ThreadState CheckState(ThreadHeader header)
		{
			if (header == null)
				throw new ArgumentNullException("header");

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(header.Url);
			req.UserAgent = TwinDll.IEUserAgent;
			HttpWebResponse res = (HttpWebResponse)req.GetResponse();

			byte[] data = FileUtility.ReadBytes(res.GetResponseStream());
			string html = Encoding.GetEncoding("Shift_Jis").GetString(data);

			if (html.IndexOf("����Ȕ�or�X���b�h�Ȃ��ł�") >= 0)
			{
				if (html.IndexOf("����! �ߋ����O�q�ɂ�") >= 0)
					return ThreadState.Kakolog;

				if (html.IndexOf("�ߋ����O�q�ɂɂ�����܂���ł���") >= 0)
					return ThreadState.NotExists;
			}
			else if (html.IndexOf("���̃X���b�h�͉ߋ����O�q�ɂɊi�[����Ă��܂�") >= 0 ||
					html.IndexOf("���������Ɛl�吙") >= 0)
			{
				return ThreadState.Pastlog;
			}

			return ThreadState.None;
		}*/

		/// <summary>
		/// �w�肵���X���b�h��resStart����resEnd�܂ł͈̔͂��擾�B
		/// �������O�ɑ��݂���΃��[�J������ǂݍ��ށB
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="header"></param>
		/// <param name="resStart"></param>
		/// <param name="resEnd"></param>
		/// <returns></returns>
		public static ResSetCollection GetRange(Cache cache, ThreadHeader header, int resStart, int resEnd)
		{
			if (cache == null)
				throw new ArgumentNullException("cache");

			if (header == null)
				throw new ArgumentNullException("header");

			if (resStart > resEnd)
				throw new ArgumentException("resStart��resEnd�ȉ��ɂ��Ă�������", "resStart");

			string address = header.Url + ((resStart == resEnd) ? 
			resStart.ToString() : String.Format("{0}-{1}", resStart, resEnd));

			// �T�[�o�[����f�[�^���_�E�����[�h
			WebClient webClient = new WebClient();
			webClient.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT; DigExt)");
			
			byte[] data = webClient.DownloadData(address);
			int byteParsed;

			// ResSet[]�^�ɉ��
			ThreadParser parser = new X2chHtmlThreadParser(BbsType.X2ch, Encoding.GetEncoding("Shift_Jis"));
			ResSet[] array = parser.Parse(data, data.Length, out byteParsed);

			ResSetCollection items = new ResSetCollection();
			items.AddRange(array);

			return items;
		}
	}

	/// <summary>
	/// �X���b�h�̏�Ԃ�\���񋓑�
	/// </summary>
	public enum ThreadState
	{
		/// <summary>
		/// �w��Ȃ�
		/// </summary>
		None,
		/// <summary>
		/// �X���b�h�͑��݂���
		/// </summary>
		Exists,
		/// <summary>
		/// �X���b�h��������Ȃ�����
		/// </summary>
		NotExists,
		/// <summary>
		/// �X���b�h�͉ߋ����O�Ɋi�[����Ă���
		/// </summary>
		Kakolog,
		/// <summary>
		/// �X���b�h��dat�������Ă���
		/// </summary>
		Pastlog,
	}
}
