// ToolItem.cs
// #2.0

namespace Twin.Tools
{
	using System;

	/// <summary>
	/// �O���c�[�����N�����邽�߂̏����Ǘ����܂��B
	/// </summary>
	public class ToolItem
	{
		private string name = String.Empty;
		private string parameter = String.Empty;
		private string fileName = String.Empty;

		/// <summary>
		/// �\����̃c�[�������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string Name
		{
			set
			{
				name = value;
			}
			get
			{
				return name;
			}
		}

		/// <summary>
		/// �N������O���c�[���̃t�@�C�������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string FileName
		{
			set
			{
				fileName = value;
			}
			get
			{
				return fileName;
			}
		}

		/// <summary>
		/// FileName �v���p�e�B�Őݒ肳�ꂽ�t�@�C���ɓn���p�����[�^���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string Parameter
		{
			set
			{
				parameter = value;
			}
			get
			{
				return parameter;
			}
		}

		public ToolItem()
		{
		}

		public ToolItem(string name, string filename, string param)
		{
			this.name = name;
			this.fileName = filename;
			this.parameter = param;
		}

		public override string ToString()
		{
			return name;
		}
	}
}
