// MenuSerializer.cs

namespace CSharpSamples
{
	using System;
	using System.IO;
	using System.Xml;
	using System.Text;
	using System.Windows.Forms;
	using System.Reflection;
	using System.Collections;

	/// <summary>
	/// ���j���[�̏����t�@�C���ɃV���A�����܂��͋t�V���A�������s���N���X
	/// </summary>
	public class MenuSerializer
	{
		/// <summary>
		/// obj���Ɋ܂܂��MenuItem�t�B�[���h�̏���XML�ɃV���A����
		/// </summary>
		/// <param name="filePath">�ۑ���t�@�C���p�X</param>
		/// <param name="obj">�V���A�����Ώۂ̃I�u�W�F�N�g</param>
		public static void Serialize(string filePath, object obj)
		{
			XmlDocument doc = new XmlDocument();

			// menu�̌^���𑮐���
			XmlAttribute attrType = doc.CreateAttribute("Type");
			attrType.Value = obj.GetType().FullName;

			// ���j���[�̃��[�g���
			XmlElement root = doc.CreateElement("Menu");
			root.Attributes.Append(attrType);

			// obj�ɑ��݂��邷�ׂĂ�MenuItem��xml�ɏ����o��
			FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance |
				BindingFlags.NonPublic | BindingFlags.Public);

			foreach (FieldInfo field in fields)
			{
				if (field.FieldType != typeof(ToolStripMenuItem))
					continue;

				ToolStripMenuItem menu = (ToolStripMenuItem)field.GetValue(obj);

				if (menu != null)
				{
					// Text����
					XmlAttribute attrText = doc.CreateAttribute("Text");
					attrText.Value = menu.Text;

					// Visible����
					//XmlAttribute attrVisible = doc.CreateAttribute("Visible");
					//attrVisible.Value = menu.Visible.ToString();

					// Shortcut����
					XmlAttribute attrShortcut = doc.CreateAttribute("Shortcut");
					attrShortcut.Value = menu.ShortcutKeys.ToString();

					XmlNode node = doc.CreateElement("MenuItem");
					node.Attributes.Append(attrText);
					//node.Attributes.Append(attrVisible);
					node.Attributes.Append(attrShortcut);
					node.InnerText = field.Name;

					root.AppendChild(node);
				}
			}

			doc.AppendChild(root);

			XmlTextWriter writer = null;
			try {
				writer = new XmlTextWriter(filePath, Encoding.UTF8);
				writer.Indentation = 1;
				writer.IndentChar = '\t';
				writer.Formatting = Formatting.Indented;
				doc.Save(writer);
			}
			finally {
				if (writer != null)
					writer.Close();
			}
		}

		/// <summary>
		/// �w�肵���t�@�C���ɕۑ�����Ă���XML���f�V���A������obj�ɓǂݍ���
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="obj"></param>
		public static void Deserialize(string filePath, object obj)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.DocumentElement;

			XmlAttribute attrType = root.Attributes["Type"];
			if (attrType == null)
				throw new ApplicationException("���[�g�Ɍ^��񂪑��݂��܂���");

			string typeName = attrType.Value;
			if (typeName.Contains("Version"))
			{
				int token = typeName.IndexOf(",");
				if (token >= 0)
					typeName = typeName.Substring(0, token);
			}

			Type type = Type.GetType(typeName);
			XmlNodeList menuItems = root.SelectNodes("MenuItem");

			foreach (XmlNode node in menuItems)
			{
				XmlAttribute attrText = node.Attributes["Text"];
				XmlAttribute attrVisible = node.Attributes["Visible"];
				XmlAttribute attrShortcut = node.Attributes["Shortcut"];

				FieldInfo field = type.GetField(node.InnerText, 
					BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

				if (field != null)
				{
					ToolStripMenuItem menu = (ToolStripMenuItem)field.GetValue(obj);
					menu.Text = attrText.Value;
					menu.ShortcutKeys = (Keys)Enum.Parse(typeof(Keys), attrShortcut.Value);
					
					if (attrVisible != null)
					{
						menu.Visible = Boolean.Parse(attrVisible.Value);
					}
				}
			}
		}
	}
}
