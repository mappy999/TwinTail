// MachiThreadReader.cs

namespace Twin.Bbs
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.Net;
	using System.Diagnostics;
	using Twin.Text;
	using Twin.Util;
	using Twin.IO;

	/// <summary>
	/// �܂�BBS (www.machi.to) �̃X���b�h��ǂݍ��ދ@�\���
	/// </summary>
	public class MachiThreadReader : ThreadReaderBase
	{
		protected ThreadHeader headerInfo;
		private HttpWebResponse _res = null;
		private int prevIndex = -1;

		/// <summary>
		/// �p�[�T���w�肵��MachiThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiThreadReader(ThreadParser dataParser)
			: base(dataParser)
		{
		}

		/// <summary>
		/// MachiThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		public MachiThreadReader()
			: base(new MachiThreadParser())
		{
		}

		/// <summary>
		/// MachiThreadReader�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="header"></param>
		public MachiThreadReader(ThreadHeader header) : this()
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
			if (header == null) {
				throw new ArgumentNullException("header");
			}
			if (IsOpen) {
				throw new InvalidOperationException("���ɃX�g���[�����J����Ă��܂�");
			}

			string url = header.Url;

			if (header.GotByteCount > 0)
			{
				// �����擾�p��URL�ɏC����������
				url += String.Format("&START={0}&NOFIRST=TRUE", header.GotResCount + 1);
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

			// OK
			if (_res.StatusCode == HttpStatusCode.OK)
			{
				position = 0;
				length = (int)_res.ContentLength;
				index = header.GotResCount + 1;

				// �J�n�C���f�b�N�X��ݒ�
				((MachiThreadParser)dataParser).StartIndex = index;

				headerInfo.LastModified = _res.LastModified;
				isOpen = true;
			}
			// dat���������\��
			else {
				headerInfo.Pastlog = true;
				_res.Close();
				_res = null;
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
				if (res.Index <= 0)
					res.Index = index++;

				// JBBS���ځ[��΍�
				//int aboneCount = (res.Index - prevIndex) - 1; // ���O�̃��X�ԍ��Ɣ�r���A���ł��郌�X�������߂�
				//if (prevIndex != -1 && aboneCount > 1)
				//{
				//    // �܂�BBS�AJBBS�ł̓��X��dat���̂���폜���邠�ځ[�񂪂��邽�߁A
				//    // ��͂̍ۂɃ��X�ԍ������ł��܂����X�ԍ��������Ă��܂��B
				//    // �Ȃ̂Ń_�~�[�̂��ځ[�񃌃X��}�����Ă���
				//    for (int i = 0; i < aboneCount; i++)
				//        resSets.Add(ResSet.ABoneResSet);
				//}
				prevIndex = res.Index;

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

			prevIndex = -1;
			if (_res != null)
			{
				_res.Close();
				_res = null;
			}
		}
	}
}
