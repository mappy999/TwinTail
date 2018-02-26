// X2chKakoThreadReader.cs

namespace Twin.Bbs
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.Net;
	using System.Diagnostics;
	using System.IO.Compression;
	using Twin.Text;
	using Twin.Util;
	using Twin.IO;

	/// <summary>
	/// �Q�����̉ߋ����O��ǂݍ���
	/// </summary>
	public class X2chKakoThreadReader : X2chThreadReader
	{
		private HttpWebResponse _res = null;
		private BoardInfo[] retryServers = null;
		private int retryCount = 0;

		/// <summary>
		/// �ߋ����O���擾�ł��Ȃ������ꍇ�A�Ď��s���s���T�[�o���擾�܂��͐ݒ�
		/// </summary>
		public BoardInfo[] RetryServers {
			set {
				retryServers = value;
			}
			get {
				return retryServers;
			}
		}

		/// <summary>
		/// X2chThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chKakoThreadReader() : base()
		{
		}

		/// <summary>
		/// �X���b�h���J��
		/// </summary>
		/// <param name="th"></param>
		public override bool Open(ThreadHeader header)
		{
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			if (IsOpen) {
				throw new InvalidOperationException("���ɃX�g���[�����J����Ă��܂�");
			}

			X2chKakoThreadHeader kakoheader = header as X2chKakoThreadHeader;
			bool retried = false;

			if (kakoheader != null)
				kakoheader.GzipCompress = true;

Retry:
			// �l�b�g���[�N�X�g���[����������
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(header.DatUrl);
			req.Timeout = 30000;
			req.AllowAutoRedirect = false;
			req.Referer = header.Url;
			req.UserAgent = UserAgent;
			req.Headers.Add("Accept-Encoding", "gzip");

			req.Headers.Add("Pragma", "no-cache");
			req.Headers.Add("Cache-Control", "no-cache");
			
			if (header.GotByteCount > 0)
				req.AddRange(header.GotByteCount-1);

			if (header.ETag != String.Empty)
				req.Headers.Add("If-None-Match", header.ETag);

			_res = (HttpWebResponse)req.GetResponse();
			baseStream = _res.GetResponseStream();
			headerInfo = header;

			// OK
			if (_res.StatusCode == HttpStatusCode.OK ||
				_res.StatusCode == HttpStatusCode.PartialContent)
			{
				bool encGzip = _res.ContentEncoding.EndsWith("gzip");

				// Gzip���g�p����ꍇ�͂��ׂēǂݍ���
				if (encGzip)
				{
					using (GZipStream gzipInp = new GZipStream(_res.GetResponseStream(), CompressionMode.Decompress))
						baseStream = FileUtility.CreateMemoryStream(gzipInp);

					baseStream.Position = 0;
					length = (int)baseStream.Length;
				}
				else {
					length = aboneCheck ?
						(int)_res.ContentLength - 1 : (int)_res.ContentLength;
				}

				headerInfo.LastModified = _res.LastModified;
				headerInfo.ETag = _res.Headers["ETag"];

				index = header.GotResCount + 1;
				position = 0;
				isOpen = true;
			}
			else if (!retried)
			{
				_res.Close();
				_res = null;

				if (kakoheader != null)
					kakoheader.GzipCompress = !kakoheader.GzipCompress;
				retried = true;
				goto Retry;
			}
			else if (_res.StatusCode == HttpStatusCode.Found)
			{
				if (retryServers != null && retryCount < retryServers.Length)
				{
					BoardInfo retryBoard = retryServers[retryCount++];
					_res.Close();
					_res = null;
				
					if (retryBoard != null)
						throw new X2chRetryKakologException(retryBoard);
				}
			}

			// �ߋ����O�Ȃ̂�dat�����ɐݒ�
			//0324 headerInfo.Pastlog = true;

			retryCount = 0;

			return isOpen;
		}

		/// <summary>
		/// ���X��ǂݍ���
		/// </summary>
		/// <param name="resSets"></param>
		/// <param name="byteParsed"></param>
		/// <returns></returns>
		public override int Read(ResSetCollection resSets)
		{
			int temp;
			return Read(resSets, out temp);
		}

		/// <summary>
		/// ���X��ǂݍ���
		/// </summary>
		/// <param name="resSets"></param>
		/// <param name="byteParsed"></param>
		/// <returns></returns>
		public override int Read(ResSetCollection resSets, out int byteParsed)
		{
			if (resSets == null) {
				throw new ArgumentNullException("resSets");
			}
			if (!isOpen) {
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			// �o�b�t�@�Ƀf�[�^��ǂݍ���
			int byteCount = baseStream.Read(buffer, 0, buffer.Length);

			// ��͂��ăR���N�V�����Ɋi�[
			ICollection collect = dataParser.Parse(buffer, byteCount, out byteParsed);

			foreach (ResSet resSet in collect)
			{
				ResSet res = resSet;
				res.Index = index++;
				resSets.Add(res);

				if (res.Index == 1 && res.Tag != null)
					headerInfo.Subject = (string)res.Tag;
			}

			// ���ۂɓǂݍ��܂ꂽ�o�C�g�����v�Z
			position += byteCount;

			return byteCount;
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
