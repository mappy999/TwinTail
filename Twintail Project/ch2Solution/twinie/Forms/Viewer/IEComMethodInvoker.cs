// IEComMethodInvoker.cs

namespace Twin
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections;
	using System.Reflection;
	using Twin.Forms;

	/// <summary>
	/// IEComMethodInvoker �̊T�v�̐����ł��B
	/// </summary>
	public class IEComMethodInvoker
	{
		private IExternalMethod iem;

		/// <summary>
		/// IEComMethodInvoker�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="iem"></param>
		public IEComMethodInvoker(IExternalMethod iem)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.iem = iem;
		}

		/// <summary>
		/// ���\�b�h���N��
		/// </summary>
		/// <param name="funcText">�֐���\��������</param>
		/// <returns></returns>
		public object Invoke(string funcText)
		{
			Match m = Regex.Match(funcText, @"(?<method>\w+)\((?<param>.*?)\)");
			if (!m.Success)
				throw new ArgumentException("func�̏������s���ł�");

			String methodName = m.Groups["method"].Value;
			String param = m.Groups["param"].Value;

			Type type = typeof(IExternalMethod);
			MethodInfo method = type.GetMethod(methodName);
			if (method == null)
				throw new ArgumentException("�w�肵���֐����擾�ł��܂���ł���");

			String[] temp = param.Split(',');
			ArrayList list = new ArrayList();

			foreach (String arg in temp)
			{
				// ���l�̏ꍇint�^�ɕϊ�
				if (Regex.IsMatch(arg, @"\$[0-9\-]+"))
				{
					int val = Int32.Parse(arg.Substring(1));
					list.Add(val);
				}
				// ����ȊO�͕�����Ƃ��Ĉ���
				else if (arg.Length > 0)
				{
					list.Add(arg);
				}
			}

			// ���\�b�h���N��
			return method.Invoke(iem, 
				(list.Count > 0) ? list.ToArray() : null);
		}
	}
}
