// MonalogConverter.cs

namespace Twin.Conv
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Diagnostics;

	/// <summary>
	/// Monalog/1.0 �̒��Ԍ`���̑��݃R���o�[�^�[
	/// �Q�lURL: http://logconvert.s28.xrea.com/
	/// </summary>
	public class MonalogConverter : IConvertible
	{
		/// <summary>
		/// MonalogConverter�N���X�̃C���X�^���X��������
		/// </summary>
		public MonalogConverter()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		public void Read(string filePath, out ThreadHeader header, 
			out ResSetCollection resCollection)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			header = TypeCreator.CreateThreadHeader(BbsType.X2ch);
			resCollection = new ResSetCollection();

			// ���[�g�v�f���擾
			XmlNode root = doc.DocumentElement;

			// <thread>�v�f�̏���
			XmlNode thread = root.SelectSingleNode("thread");
			if (thread == null)
				throw new ConvertException("<thread>�v�f�����݂��܂���");

			XmlNode serv = thread.Attributes.GetNamedItem("server");
			if (serv == null)
				throw new ConvertException("server���������݂��܂���");

			XmlNode board = thread.Attributes.GetNamedItem("board");
			if (board == null)
				throw new ConvertException("board���������݂��܂���");

			XmlNode key = thread.Attributes.GetNamedItem("key");
			if (key == null)
				throw new ConvertException("key���������݂��܂���");

			// �X���b�h�^�C�g�����擾
			XmlNode title = thread.SelectSingleNode("title");
			if (title == null)
				throw new ConvertException("<title>�v�f�����݂��܂���");

			// <info>�v�f�̏���
			XmlNode info = thread.SelectSingleNode("info");
			if (info == null)
				throw new ConvertException("<info>�v�f�����݂��܂���");

			XmlNode lastmod = info.Attributes.GetNamedItem("last-modified");
			if (lastmod == null)
				throw new ConvertException("last-modified���������݂��܂���");

			XmlNode size = info.Attributes.GetNamedItem("size");
			if (size == null)
				throw new ConvertException("size���������݂��܂���");

			// �X���b�h�̏���ݒ�
			header.Key = key.Value;
			header.BoardInfo.Server = serv.Value;
			header.BoardInfo.Path = board.Value;
			header.Subject = title.InnerText;
			header.LastModified = DateTime.Parse(lastmod.Value);
			
			int gotByteCount;
			if (Int32.TryParse(size.Value, out gotByteCount))
				header.GotByteCount = gotByteCount;

			// <res-set>�v�f�̏���
			XmlNode children = thread.SelectSingleNode("res-set");
			if (children == null)
				throw new ConvertException("<res-set>�v�f�����݂��܂���");

			resCollection = GetResCollection(doc, children);
			header.GotResCount = resCollection.Count;
		}

		private ResSetCollection GetResCollection(XmlDocument doc, XmlNode resset)
		{
			ResSetCollection collection = new ResSetCollection();

			foreach (XmlNode child in resset.SelectNodes("res"))
			{
				XmlNode num = child.Attributes.GetNamedItem("num");
				if (num == null)
					throw new ConvertException("num���������݂��܂���");

				// state�����͖���
				// ...

				XmlNode name = child.SelectSingleNode("name");
				if (name == null)
					throw new ConvertException("<name>�v�f�����݂��܂���");

				XmlNode email = child.SelectSingleNode("email");
				if (email == null)
					throw new ConvertException("<email>�v�f�����݂��܂���");

				XmlNode timestamp = child.SelectSingleNode("timestamp");
				if (timestamp == null)
					throw new ConvertException("<timestamp>�v�f�����݂��܂���");

				XmlNode message = child.SelectSingleNode("message");
				if (message == null)
					throw new ConvertException("<message>�v�f�����݂��܂���");

				int index;
				Int32.TryParse(num.Value, out index);

				ResSet res = new ResSet(
					index,
					name.InnerText,
					email.InnerText,
					timestamp.InnerText,
					message.InnerText);

				collection.Add(res);
			}

			return collection;
		}
		
		public void Write(string filePath, ThreadHeader header,
			ResSetCollection resCollection)
		{
			XmlDocument doc = new XmlDocument();

			// �o�[�W�����������쐬
			XmlAttribute version = doc.CreateAttribute("version");
			version.Value = "1.0";

			// ���[�g���쐬
			XmlNode root = doc.CreateElement("monalog", "http://www.monazilla.org/Monalog/1.0");
			root.Attributes.Append(version);

			// �X���b�h�v�f���쐬
			XmlNode thread = CreateThreadElement(doc, header, resCollection);
			root.AppendChild(thread);
			doc.AppendChild(root);

			// �G���R�[�_��Shift_Jis�ŕۑ�
			XmlTextWriter writer = null;
			try {
				writer = new XmlTextWriter(filePath, Encoding.GetEncoding("Shift_Jis"));
				writer.Formatting = Formatting.Indented;
				doc.Save(writer);
			}
			finally {
				if (writer != null)
					writer.Close();
			}
		}

		private XmlNode CreateThreadElement(XmlDocument doc,
			ThreadHeader header, ResSetCollection resCollection)
		{
			// �T�[�o�[�������쐬
			XmlAttribute serv = doc.CreateAttribute("server");
			serv.Value = header.BoardInfo.Server;

			// �������쐬
			XmlAttribute board = doc.CreateAttribute("board");
			board.Value = header.BoardInfo.Path;

			// key�������쐬
			XmlAttribute key = doc.CreateAttribute("key");
			key.Value = header.Key;

			// <thread></thread>�v�f���쐬
			XmlNode thread = doc.CreateElement("thread");
			thread.Attributes.Append(serv);
			thread.Attributes.Append(board);
			thread.Attributes.Append(key);

			// �^�C�g���v�f���쐬
			XmlNode title = doc.CreateElement("title");
			title.AppendChild(doc.CreateCDataSection(header.Subject));
			thread.AppendChild(title);

			// info�v�f���쐬
			XmlAttribute lastmod = doc.CreateAttribute("last-modified");
			lastmod.Value = header.LastModified.ToString("R"); // RFC1123 �p�^�[��

			XmlAttribute size = doc.CreateAttribute("size");
			size.Value = header.GotByteCount.ToString();

			XmlNode info = doc.CreateElement("info");
			info.Attributes.Append(lastmod);
			info.Attributes.Append(size);
			thread.AppendChild(info);

			// <res-set></res-set>�v�f���쐬
			XmlNode children = CreateResSetChildren(doc, resCollection);
			thread.AppendChild(children);

			return thread;
		}

		private XmlNode CreateResSetChildren(XmlDocument doc, ResSetCollection resCollection)
		{
			// ���X�R���N�V�����v�f���쐬
			XmlNode children = doc.CreateElement("res-set");

			foreach (ResSet res in resCollection)
			{
				// ���X�ԍ���\���Anum�������쐬
				XmlAttribute num = doc.CreateAttribute("num");
				num.Value = res.Index.ToString();

				// ���X�̏�Ԃ�\���Astate�������쐬
				XmlAttribute state = doc.CreateAttribute("state");
				state.Value = "normal";

				// ���X�v�f���쐬
				XmlNode child = doc.CreateElement("res");
				child.Attributes.Append(num);
				child.Attributes.Append(state);

				// ���O
				XmlNode name = doc.CreateElement("name");
				name.AppendChild(doc.CreateCDataSection(res.Name));
				
				// Email
				XmlNode email = doc.CreateElement("email");
				email.AppendChild(doc.CreateCDataSection(res.Email));

				// ���t
				XmlNode timestamp = doc.CreateElement("timestamp");
				timestamp.AppendChild(doc.CreateCDataSection(res.DateString));

				// ���b�Z�[�W
				XmlNode message = doc.CreateElement("message");
				message.AppendChild(doc.CreateCDataSection(res.Body));

				child.AppendChild(name);
				child.AppendChild(email);
				child.AppendChild(timestamp);
				child.AppendChild(message);
				children.AppendChild(child);
			}

			return children;
		}
	}
}
