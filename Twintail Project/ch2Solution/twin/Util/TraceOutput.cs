// TraceOutput.cs

namespace Twin.Util
{
	using System;
	using System.IO;
	using System.Text;
	using System.Diagnostics;

	/// <summary>
	/// Trace��Debug���t�@�C���ɏo�͂��邽�߂̋@�\���
	/// </summary>
	public class TraceOutput : IDisposable
	{
		private TraceListener listener;

		/// <summary>
		/// TraceOutput�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="fileName"></param>
		public TraceOutput(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}

			listener = new TextWriterTraceListener(
				new StreamWriter(fileName, true, TwinDll.DefaultEncoding), typeof(TraceOutput).FullName);

			Trace.Listeners.Add(listener);
		}

		/// <summary>
		/// �g�p���Ă��郊�\�[�X�����
		/// </summary>
		public void Dispose()
		{
			listener.Close();
			listener.Dispose();
		}

		/// <summary>
		/// �X�g���[���ƕ��ăg���[�X�o�͂���M���Ȃ��悤�ɂ���
		/// </summary>
		public void Close()
		{
			Dispose();
		}
	}
}
