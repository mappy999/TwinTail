// ErrorWriter.cs

namespace Twin.Util
{
	using System;
	using System.IO;
	using System.Text;
	using System.Diagnostics;
	using System.Windows.Forms;

	/// <summary>
	/// �G���[���t�@�C���ɏ�������
	/// </summary>
	public class ErrorWriter
	{
		private int[] ticks;
		private string[] names;
		private int position;

		private readonly string ListenerName = DateTime.Now.Millisecond.ToString();
		private TraceListener listener;
		private string fileName;

		/// <summary>
		/// �G���[��ۑ�����t�@�C����
		/// </summary>
		public string FileName {
			set {
				SetListener(value);
			}
			get {
				return fileName;
			}
		}

		public ErrorWriter() : this(Application.StartupPath + "\\error.log")
		{
		}

		public ErrorWriter(string fileName)
		{
			position = 0;
			ticks = new int[256];
			names = new string[256];

			listener = null;
			SetListener(fileName);
		}

		~ErrorWriter()
		{
			Close();
		}

		private void SetListener(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName", "fileName��null�Q�Ƃł�");
			}

			Close();

			this.fileName = fileName;
			listener = new TextWriterTraceListener(
				new StreamWriter(fileName, true, TwinDll.DefaultEncoding), ListenerName);

			Trace.Listeners.Add(listener);
		}

		[Conditional("DEBUG")]
		public void CountStart(string name)
		{
			lock (this)
			{
				ticks[position] = Environment.TickCount;
				names[position] = name;
				position++;
			}
		}

		[Conditional("DEBUG")]
		public void CountStop(bool msgBox)
		{
			lock (this)
			{
				position--;

				int count = Environment.TickCount - ticks[position];
				Write("{0}\t{1}ms", names[position], count);

				if (msgBox)
					MessageBox.Show(count.ToString() + "ms");
			}
		}

		/// <summary>
		/// ����
		/// </summary>
		public void Close()
		{
			if (listener != null) {
				listener.Close();
				listener = null;
				Trace.Listeners.Remove(ListenerName);
			}
		}

		/// <summary>
		/// ���b�Z�[�W�{�b�N�X��\��
		/// </summary>
		/// <param name="owner">�I�[�i�[�E�C���h�E</param>
		/// <param name="message">�\�����郁�b�Z�[�W</param>
		/// <param name="caption">�L���v�V����</param>
		/// <param name="buttons">�{�^��</param>
		/// <param name="icon">�A�C�R��</param>
		/// <returns>�����ꂽ�{�^���̎��</returns>
		public DialogResult Show(IWin32Window owner, 
										string message, 
										string caption, 
										MessageBoxButtons buttons, 
										MessageBoxIcon icon)
		{
			Write(message);

			return MessageBox.Show(owner, message,
				caption, buttons, icon);
		}

		/// <summary>
		/// ���b�Z�[�W�{�b�N�X��\��
		/// </summary>
		/// <param name="message">�\�����郁�b�Z�[�W</param>
		public void Show(string message)
		{
			Show(null, message, "�A�v���P�[�V�����G���[",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// ���b�Z�[�W�����������ĕ\��
		/// </summary>
		/// <param name="format">����</param>
		/// <param name="arguments">����</param>
		public void Show(string format, params object[] arguments)
		{
			string text = String.Format(format, arguments);
			Show(text);
		}

		/// <summary>
		/// ���b�Z�[�W�{�b�N�X��\��
		/// </summary>
		/// <param name="owner">�I�[�i�[�E�C���h�E</param>
		/// <param name="message">�\�����郁�b�Z�[�W</param>
		public void Show(IWin32Window owner, string message)
		{
			Show(owner, message, "�A�v���P�[�V�����G���[",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// ���b�Z�[�W�{�b�N�X�ŗ�O��\��
		/// </summary>
		/// <param name="ex">�\�������O</param>
		public void Show(Exception ex)
		{
			Show(null, ex);
		}

		/// <summary>
		/// ���b�Z�[�W�{�b�N�X��\��
		/// </summary>
		/// <param name="owner">�I�[�i�[�E�C���h�E</param>
		/// <param name="ex">�\�������O</param>
		public void Show(IWin32Window owner, Exception ex)
		{
			Write(ex.ToString());

			MessageBox.Show(owner, ex.Message, "��O���������܂���",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// ���b�Z�[�W���t�@�C���ɏ�������
		/// </summary>
		/// <param name="message">�������ރ��b�Z�[�W</param>
		public void Write(string message)
		{
			listener.WriteLine(String.Format("ver{0} ({1})", TwinDll.Version, DateTime.Now));
			listener.WriteLine(String.Format("{0} CLR {1}", Environment.OSVersion, Environment.Version));
			listener.WriteLine(message);
			listener.WriteLine(String.Empty);
		}

		/// <summary>
		/// ����������������ď�������
		/// </summary>
		/// <param name="format">����</param>
		/// <param name="arguments">����</param>
		public void Write(string format, params object[] arguments)
		{
			Write(String.Format(format, arguments));
		}

		/// <summary>
		/// �I�u�W�F�N�g�𕶎���ɕϊ����ď�������
		/// </summary>
		/// <param name="obj">�������ރI�u�W�F�N�g</param>
		public void Write(object obj)
		{
			Write(obj.ToString());
		}
	}
}
