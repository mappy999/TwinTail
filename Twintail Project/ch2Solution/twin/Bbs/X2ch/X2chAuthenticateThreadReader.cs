// X2chAuthenticateThreadReader.cs

namespace Twin.Bbs
{
	using System;
	using System.Net;
	using System.IO.Compression;
	using Twin.Util;
	using System.IO;
	using System.Text;

	/// <summary>
	/// �F�؂��g���ĂQ�����˂� (www.2ch.net) �̃X���b�h��ǂݍ��ދ@�\���
	/// </summary>
	public class X2chAuthenticateThreadReader : X2chThreadReader
	{
		private HttpWebResponse _res = null;
		//private bool firstRead = true;

		public X2chRokkaResponseState RokkaResponseState { get; private set; }

		public X2chAuthenticateThreadReader()
			: base()
		{
			RokkaResponseState = X2chRokkaResponseState.None;
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

			headerInfo = header;
			//firstRead = true;
			RokkaResponseState = X2chRokkaResponseState.None;

			if (header.Pastlog)
				return false;

			X2chAuthenticator authenticator = X2chAuthenticator.GetInstance();
			if (authenticator.HasSession)
			{
				X2chThreadHeader x2chHeader = header as X2chThreadHeader;
				if (x2chHeader != null)
				{
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(x2chHeader.AuthenticateUrl);
					req.Timeout = 15000;
					req.UserAgent = UserAgent;
					req.AllowAutoRedirect = false;

					// ** 9/26 �폜 **
					// req.Headers.Add("Accept-Encoding", "gzip");

					// ** 9/26 �ǉ� **
					req.AutomaticDecompression = DecompressionMethods.GZip;

					req.Headers.Add("Pragma", "no-cache");
					req.Headers.Add("Cache-Control", "no-cache");

					if (x2chHeader.ETag != String.Empty)
						req.Headers.Add("If-None-Match", x2chHeader.ETag);

					_res = (HttpWebResponse)req.GetResponse();

					//baseStream = _res.GetResponseStream();
					baseStream = FileUtility.CreateMemoryStream(_res.GetResponseStream());
					baseStream.Position = 0;
					length = (int)baseStream.Length;

					RokkaResponseState = ParseRokkaFirstline(baseStream);

					if (_res.StatusCode == HttpStatusCode.OK)
					{
						/* 9 26�폜
						using (GZipStream gzipInp = new GZipStream(_res.GetResponseStream(), CompressionMode.Decompress))
						baseStream = FileUtility.CreateMemoryStream(gzipInp);
						*/

						x2chHeader.ETag = _res.Headers["ETag"];
						x2chHeader.LastModified = _res.LastModified;

						index = header.GotResCount + 1;
						position = 0;
						isOpen = true;
					}
					else OnPastlog(new PastlogEventArgs(header));
				}
			}
			else
				OnPastlog(new PastlogEventArgs(header));

			// �ߋ����O�Ȃ̂�dat�����ɐݒ�
			headerInfo.Pastlog = true;

			return isOpen;
		}

		//public override int Read( ResSetCollection resSets , out int byteParsed )
		//{
		// if ( firstRead )
		// {
		// byte[] buf = new byte[1];
		// int c;

		// do
		// {
		// c = baseStream.Read( buf , 0 , buf.Length );
		// // �ŏ��̉��s������܂œǂݔ�΂�
		// } while ( c != 0 && buf[0] != '\n' );

		// firstRead = false;
		// }

		// return base.Read( resSets , out byteParsed );
		//}

		public override void Close()
		{
			base.Close();
			if (_res != null)
			{
				_res.Close();
				_res = null;
			}
		}


		private X2chRokkaResponseState ParseRokkaFirstline(Stream responseStream)
		{
			if (responseStream == null)
				throw new ArgumentNullException();

			var statusbytes = new System.Collections.Generic.List<byte>();
			byte[] buf = new byte[1];
			int c;
			do
			{
				c = baseStream.Read(buf, 0, buf.Length);
				// �ŏ��̉��s������܂œǂݔ�΂�
				// 2013/09/10 ���߂̍s���������ʃR�[�h�G���A�ƒ�`���錾�����̂ŕ����Ɏ�荞��ł���
				// http://qb5.2ch.net/test/read.cgi/operate/1366640919/119
				statusbytes.Add(buf[0]);
			} while (c != 0 && buf[0] != '\n');

			var line = base.dataParser.Encoding.GetString(statusbytes.ToArray());

			//byte[] buf = new byte[256];
			//long firstpos = responseStream.Position;
			//responseStream.Read( buf , 0 , buf.Length );

			//using ( StringReader s = new StringReader( base.dataParser.Encoding.GetString( buf ) ) )
			//using ( StreamReader s = new StreamReader( responseStream ) )
			//{
			// string line = s.ReadLine();
			//responseStream.Position += base.dataParser.Encoding.GetByteCount( line ); // �ŏ���1�s�������X�g���[����i�߂Ă���

			if (line.StartsWith("Success")) return X2chRokkaResponseState.Success;
			else if (line.StartsWith("Error 8008135")) return X2chRokkaResponseState.InvalidServerOrBoardOrThread;
			else if (line.StartsWith("Error 69")) return X2chRokkaResponseState.AuthenticationError;
			else if (line.StartsWith("Error 666")) return X2chRokkaResponseState.UrlError;
			else if (line.StartsWith("Error 420")) return X2chRokkaResponseState.TimeLimitError;
			else
			{
				// �X�e�[�^�X��������Ȃ������ꍇ�́A�X�g���[���̈ʒu�����ɖ߂��Ă���
				//responseStream.Position = firstpos;
				return X2chRokkaResponseState.Unknown;
			}
			//}
		}
	}


	/*
	"Success"�@�@- The process has successfuly done. Following lines are achieved message with dat format
	Error codes:
		inputError = "Error 8008135"�@�@�@�@�@�@invalid SERVER or BOARD or THREAD
		authenticationError = "Error 69"�@�@�@�@invalid SID
		urlError = "Error 666"�@�@�@�@�@�@�@�@�@invalid OPTIONS
		timeLimitError = "Error 420"�@�@�@�@�@�@ access too fast, interval between requests required
	*/
	public enum X2chRokkaResponseState
	{
		None = -2,
		Unknown = -1, // �X�e�[�^�X���s��
		Success = 0,
		InvalidServerOrBoardOrThread = 8008135,
		AuthenticationError = 89,
		UrlError = 666,
		TimeLimitError = 420,
	}
}