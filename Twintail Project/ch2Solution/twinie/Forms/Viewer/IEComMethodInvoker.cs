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
	/// IEComMethodInvoker の概要の説明です。
	/// </summary>
	public class IEComMethodInvoker
	{
		private IExternalMethod iem;

		/// <summary>
		/// IEComMethodInvokerクラスのインスタンスを初期化
		/// </summary>
		/// <param name="iem"></param>
		public IEComMethodInvoker(IExternalMethod iem)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.iem = iem;
		}

		/// <summary>
		/// メソッドを起動
		/// </summary>
		/// <param name="funcText">関数を表す文字列</param>
		/// <returns></returns>
		public object Invoke(string funcText)
		{
			Match m = Regex.Match(funcText, @"(?<method>\w+)\((?<param>.*?)\)");
			if (!m.Success)
				throw new ArgumentException("funcの書式が不正です");

			String methodName = m.Groups["method"].Value;
			String param = m.Groups["param"].Value;

			Type type = typeof(IExternalMethod);
			MethodInfo method = type.GetMethod(methodName);
			if (method == null)
				throw new ArgumentException("指定した関数を取得できませんでした");

			String[] temp = param.Split(',');
			ArrayList list = new ArrayList();

			foreach (String arg in temp)
			{
				// 数値の場合int型に変換
				if (Regex.IsMatch(arg, @"\$[0-9\-]+"))
				{
					int val = Int32.Parse(arg.Substring(1));
					list.Add(val);
				}
				// それ以外は文字列として扱う
				else if (arg.Length > 0)
				{
					list.Add(arg);
				}
			}

			// メソッドを起動
			return method.Invoke(iem, 
				(list.Count > 0) ? list.ToArray() : null);
		}
	}
}
