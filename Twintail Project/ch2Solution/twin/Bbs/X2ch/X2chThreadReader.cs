// X2chThreadReader.cs

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
	using Twin.Tools;
	using System.Threading;

	/// <summary>
	/// �Q�����˂� (www.2ch.net) �̃X���b�h��ǂݍ��ދ@�\���
	/// </summary>
	public class X2chThreadReader : ThreadReaderBase
	{
		protected ThreadHeader headerInfo;
		protected bool aboneCheck;
		protected bool getGzip;

		private HttpWebResponse _res = null;

		/// <summary>
		/// �w�肵���p�[�T���g�p����X2chThreadReader�N���X�̃C���X�^���X���������B
		/// </summary>
		/// <param name="dataParser"></param>
		public X2chThreadReader(ThreadParser dataParser)
			: base(dataParser)
		{
			getGzip = true;
		}

		/// <summary>
		/// X2chThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		public X2chThreadReader()
			: this(new X2chThreadParser())
		{
		}

		/// <summary>
		/// X2chThreadReader�N���X�̃C���X�^���X���������Ɠ����ɁA
		/// �w�肵���X���b�h���J���B
		/// </summary>
		/// <param name="header">�������Ɠ����ɊJ���X���b�h�̏��</param>
		public X2chThreadReader(ThreadHeader header)
			: this()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			Open(header);
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

			// �����擾���ǂ���
			aboneCheck = (header.GotByteCount > 0) ? true : false;
//		Retry:
			_res = null;

			try
			{

				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(header.DatUrl);
				req.Timeout = 60000;
				req.AllowAutoRedirect = false;
				req.UserAgent = UserAgent;

				//req.Headers.Add("Pragma", "no-cache");
				//req.Headers.Add("Cache-Control", "no-cache");

				if (!String.IsNullOrEmpty(header.ETag))
					req.Headers.Add("If-None-Match", header.ETag);

				if (header.GotByteCount > 0)
					req.AddRange(header.GotByteCount - 1);

				//if (!aboneCheck && getGzip)
				//	req.Headers.Add("Accept-Encoding", "gzip");

				_res = (HttpWebResponse)req.GetResponse();

				baseStream = _res.GetResponseStream();
				headerInfo = header;

				bool encGzip = _res.ContentEncoding.EndsWith("gzip");

				if (encGzip)
				{

					using (GZipStream gzipInp = new GZipStream(baseStream, CompressionMode.Decompress))
						baseStream = FileUtility.CreateMemoryStream(gzipInp);

					length = (int)baseStream.Length;
				}
				else
				{
					length = aboneCheck ?
						(int)_res.ContentLength - 1 : (int)_res.ContentLength;
				}

				// OK
				if (_res.StatusCode == HttpStatusCode.OK ||
					_res.StatusCode == HttpStatusCode.PartialContent)
				{
					headerInfo.LastModified = _res.LastModified;
					headerInfo.ETag = _res.Headers["ETag"];

					index = header.GotResCount + 1;
					position = 0;
					isOpen = true;
				}
				// dat���������\��
				else
				{
					_res.Close();

					if (_res.StatusCode == HttpStatusCode.Found)
					{
						// 10/05 �ړ]�ǔ�������Ɖߋ����O�q�ɂ��ǂ߂Ȃ��Ȃ��Ă��܂������킩�����̂ŁA�ꎞ�I�ɊO���Ă݂�
						//if (IsServerChanged(headerInfo))
						//{
						//    // �T�[�o�[���ړ]������V�������Ń��g���C�B
						//    goto Retry;
						//}
						//else
						{
							// �����łȂ���� dat�����Ƃ��Ĕ��f
							//0324 headerInfo.Pastlog = true;

							PastlogEventArgs argument = new PastlogEventArgs(headerInfo);
							OnPastlog(argument);
						}
					}

					_res = null;
				}
			}
			catch (WebException ex)
			{
				HttpWebResponse res = (HttpWebResponse)ex.Response;

				// ���ځ[��̗\��
				if (res != null &&
					res.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
				{
					OnABone();
				}
				else
				{
					throw ex;
				}
			}

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
			if (resSets == null)
			{
				throw new ArgumentNullException("resSets");
			}
			if (!isOpen)
			{
				throw new InvalidOperationException("�X�g���[�����J����Ă��܂���");
			}

			// �o�b�t�@�Ƀf�[�^��ǂݍ���
			int byteCount = baseStream.Read(buffer, 0, buffer.Length);

			// ���ځ[��`�F�b�N
			if (aboneCheck && byteCount > 0)
			{
				if (buffer[0] != '\n')
				{
					OnABone();
					byteParsed = 0;

					headerInfo.ETag = String.Empty;
					headerInfo.LastModified = DateTime.MinValue;

					return -1;
				}

				buffer = RemoveHeader(buffer, byteCount, 1);
				byteCount -= 1;
				aboneCheck = false;
			}

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

		/// <summary>
		/// buffer�̐擪��������菜��
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private static byte[] RemoveHeader(byte[] buffer, int length, int count)
		{
			byte[] result = new byte[length - count];
			Array.Copy(buffer, count, result, 0, result.Length);

			return result;
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



		private bool IsServerChanged(ThreadHeader h)
		{
			X2chServerTracer server = new X2chServerTracer();

			if (server.Trace(h.BoardInfo, true))
			{
				h.BoardInfo = server.Result;
				return true;
			}


			return false;
		}
	}

}
