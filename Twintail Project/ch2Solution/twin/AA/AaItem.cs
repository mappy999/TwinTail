// AaItem.cs

namespace Twin.Aa
{
	using System;
	using System.IO;
	using System.Text;

	/// <summary>
	/// �P��AA��\��
	/// </summary>
	public class AaItem
	{
		internal AaHeader parent;
		private string text;
		private bool single;

		/// <summary>
		/// ����AA����`����Ă���w�b�_�[���擾
		/// </summary>
		public AaHeader Parent {
			get { return parent; }
		}

		/// <summary>
		/// AA�̕\�������擾
		/// </summary>
		public string Text {
			set {
				if (value == null) {
					throw new ArgumentNullException("Text");
				}
				if (value.IndexOf('\n') != -1) {
					throw new ArgumentException("Text�ɉ��s�������܂߂邱�Ƃ͏o���܂���");
				}
				text = value;
			}
			get { return text; }
		}

		/// <summary>
		/// AA�̃f�[�^���擾
		/// </summary>
		public string Data {
			set {
				#region
				if (value == null) {
					throw new ArgumentNullException("Data");
				}
				if (Single) 
				{
					if (value.IndexOf('\n') != -1) {
						throw new ArgumentException("�P�sAA�ɉ��s�������܂߂邱�Ƃ͏o���܂���");
					}
					text = value;
				}
				else {
					// �����sAA�ւ̃p�X
					string aaPath = Path.Combine(
						Path.GetDirectoryName(parent.FileName), text);

					StreamWriter sw = null;

					try {
						sw = new StreamWriter(aaPath, false, TwinDll.DefaultEncoding);
						sw.Write(value);
					}
					finally {
						if (sw != null)
							sw.Close();
					}
				}
				#endregion
			}
			get {
				#region
				if (Single)
				{
					return text;
				}
				else {
					if (parent == null) {
						throw new InvalidOperationException("Parent���ݒ肳��Ă��܂���");
					}

					// �����sAA�ւ̃p�X
					string aaPath = Path.Combine(
						Path.GetDirectoryName(parent.FileName), text);

					StreamReader sr = null;

					try {
						sr = new StreamReader(aaPath, TwinDll.DefaultEncoding);
						return sr.ReadToEnd();
					}
					catch (FileNotFoundException) {}
					finally {
						if (sr != null)
							sr.Close(); 
					}

					return String.Empty;
				}
				#endregion
			}
		}

		/// <summary>
		/// �P�sAA�̏ꍇ��true�A�����sAA�̏ꍇ��false��Ԃ�
		/// </summary>
		public bool Single {
			get { return single; }
		}

		/// <summary>
		/// AaItem�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="text">�w�b�_�[�t�@�C�����̕\����</param>
		/// <param name="single">�P�sAA�Ȃ�true�A�����sAA�Ȃ�false���w��</param>
		public AaItem(string text, bool single)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.text = text;
			this.single = single;
		}

		/// <summary>
		/// ���̃C���X�^���X���w�b�_�[����폜
		/// </summary>
		public void Remove()
		{
			if (parent != null)
				parent.Items.Remove(this);
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}{1}",
				single ? String.Empty : "#", text);
		}
	}
}
