// ConfigSerializer.cs

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSharpSamples
{
	/// <summary>
	/// XmlSerializer ���g�p���ē���̌^���t�@�C���ɃV���A�����^�t�V���A�������s���N���X�ł��B
	/// </summary>
	public class ConfigSerializer
	{
		/// <summary>
		/// �V���A���C�Y���܂��B
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="type"></param>
		/// <param name="o"></param>
		public static void Serialize(string fileName, Type type, object o)
		{
			Serialize(fileName, type, o, Encoding.UTF8);
		}

		/// <summary>
		/// �V���A���C�Y���܂��B
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="type"></param>
		/// <param name="o"></param>
		/// <param name="enc"></param>
		public static void Serialize(string fileName, Type type, object o, Encoding enc)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}
			if (type == null) {
				throw new ArgumentNullException("type");
			}

			StreamWriter sw = null;
			XmlTextWriter writer = null;
			XmlSerializer serial = null;

			try {
				sw = new StreamWriter(fileName, false, enc);
				writer = new XmlTextWriter(sw);
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 4;

				serial = new XmlSerializer(type);
				serial.Serialize(writer, o);
			}
			finally {
				if (writer != null)
					writer.Close();
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// �f�V���A���C�Y���܂��B
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object Deserialize(string fileName, Type type)
		{
			return Deserialize(fileName, type, Encoding.UTF8);
		}

		/// <summary>
		/// �f�V���A���C�Y���܂��B
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="type"></param>
		/// <param name="enc"></param>
		/// <returns></returns>
		public static object Deserialize(string fileName, Type type, Encoding enc)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}
			if (type == null) {
				throw new ArgumentNullException("type");
			}

			XmlTextReader xmlreader = null;
			XmlSerializer serial = null;
			StreamReader sr = null;
			object result = null;

			try {
				sr = new StreamReader(fileName, enc);
				xmlreader = new XmlTextReader(sr);
				serial = new XmlSerializer(type);
				result = serial.Deserialize(xmlreader);
			}
			finally {
				if (xmlreader != null)
					xmlreader.Close();
				if (sr != null)
					sr.Close();
			}

			return result;
		}
	}
}
