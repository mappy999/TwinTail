using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Twin.IO;
using Twin.Bbs;
using Twin.Test;
using System.Threading;

namespace Twin
{
	public class PopupTest
	{
		#region parameter

		class Parameter
		{
			public Cache cache;
			public ReadOnlyCollection<ThreadHeader> items;

			public Parameter(Cache c, ReadOnlyCollection<ThreadHeader> i)
			{
				this.cache = c;
				this.items = i;
			}
		}
		#endregion

		private static PopupForm popup = new PopupForm();
		
		private static Thread thread = null;
		private static bool cancelled = false;
		private static object syncObject = new object();

		static PopupTest()
		{
			popup.PopupHidden += delegate
			{
				cancelled = true;
			};
		}

		public static void Popup1(Cache cache, ReadOnlyCollection<ThreadHeader> headerItems)
		{
			RunThread(cache, headerItems, OnPopup1);
		}

		private static void OnPopup1(object parameter)
		{
			try
			{
				Parameter param = (Parameter)parameter;
				Cache cache = param.cache;

				StringBuilder sb = new StringBuilder();
				StandardHtmlSkin skin = new StandardHtmlSkin();

				sb.Append("<html><body><dl>");
				string headerHtml = "<b><font color=red><THREADNAME/></font></b><br><br>";

				foreach (ThreadHeader header in param.items)
				{
					ResSetCollection buf = new ResSetCollection();

					if (ThreadIndexer.Exists(cache, header))
					{
						ThreadIndexer.Read(cache, header);

						// 既得スレッドの場合、ログの1だけを読み込む
						using (LocalThreadStorage storage =
							new LocalThreadStorage(cache, header, StorageMode.Read))
						{

							if (storage.Read(buf) >= 1)
							{
								sb.Append(headerHtml.Replace("<THREADNAME/>", header.Subject));
								sb.Append(skin.Convert(buf[0]));
							}
						}
					}
					else
					{
						// 未取得の場合、新しく取得
						ThreadReader reader = TypeCreator.CreateThreadReader(header.BoardInfo.Bbs);
						reader.BufferSize = 1024;

						try
						{
							if (!reader.Open(header))
								return;

							if (reader.Read(buf) == 0)
								return;

							sb.Append(headerHtml.Replace("<THREADNAME/>", header.Subject));
							sb.Append(skin.Convert(buf[0]));

							// 既得情報を設定
							X2chThreadFormatter formatter = new X2chThreadFormatter();
							int byteCount = Encoding.GetEncoding("shift_jis").GetByteCount(formatter.Format(buf[0]));

							header.GotByteCount = byteCount;
							header.NewResCount = 1;
							header.GotResCount = 1;
							header.ETag = String.Empty;

							// インデックスに保存
							ThreadIndexer.Write(cache, header);
							GotThreadListIndexer.Write(cache, header);

							// 一応、1だけ既得として保存しておく
							using (LocalThreadStorage storage = new LocalThreadStorage(cache, header, StorageMode.Write))
							{
								ResSetCollection tmp = new ResSetCollection();
								tmp.Add(buf[0]);

								storage.Write(tmp);
							}
						}
						finally
						{
							reader.Close();
						}
					}
				}

				sb.Append("</dl></body></html>");

				InvokePopup(sb.ToString());
			}
			finally
			{
				thread = null;
			}
		}

		public static void PopupNewRes(Cache cache, ReadOnlyCollection<ThreadHeader> headerItems)
		{
			RunThread(cache, headerItems, OnPopupNewRes);
		}

		private static void OnPopupNewRes(object parameter)
		{
			try
			{
				Parameter param = (Parameter)parameter;
				Cache cache = param.cache;

				StringBuilder sb = new StringBuilder();
				StandardHtmlSkin skin = new StandardHtmlSkin();

				int maxNewResLimit = 32;
				sb.Append("<html><body><dl>");
				string headerHtml = "<b><font color=red><THREADNAME/></font></b><br><br>";

				foreach (ThreadHeader header in param.items)
				{
					ThreadReaderRelay reader =
						new ThreadReaderRelay(cache, TypeCreator.CreateThreadReader(header.BoardInfo.Bbs));

					reader.ReadCache = false; // キャッシュは読み込まない
					reader.BufferSize = 512;

					try
					{
						ResSetCollection buffer = new ResSetCollection();

						ThreadIndexer.Read(cache, header);

						if (!reader.Open(header))
							return;

						while (reader.Read(buffer) != 0 && buffer.Count < maxNewResLimit)
							;

						if (buffer.Count > 0)
						{
							sb.Append(headerHtml.Replace("<THREADNAME/>", header.Subject));
							sb.Append(skin.Convert(buffer));
						}
					}
					finally
					{
						reader.Close();
					}
				}

				sb.Append("</dl></body></html>");


				InvokePopup(sb.ToString());
			}
			finally
			{
				thread = null;
			}
		}

		private static void RunThread(Cache cache, ReadOnlyCollection<ThreadHeader> headerItems, ParameterizedThreadStart callback)
		{
			if (thread != null)
				return;

			cancelled = false;
			popup.ShowPopup("<html><body>取得中．．．</body></html>", Control.MousePosition);

			thread = new Thread(callback);
			thread.IsBackground = true;
			thread.Start(new Parameter(cache, headerItems));
		}

		private static void InvokePopup(string html)
		{
			if (!cancelled)
			{
				MethodInvoker m = delegate
				{
					popup.ShowPopup(html, Control.MousePosition);
				};

				popup.Invoke(m);
			}
		}
	}
}
