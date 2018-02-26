using System;
using System.Collections.Generic;
using System.Text;
using Twin.Forms;
using Twin.Text;
using Twin.Tools;
using System.Windows.Forms;
using System.Threading;

namespace Twin
{
	public static class InternalTool
	{
		delegate bool Func(Parameter parameter);

		class Parameter
		{
			public Twin2IeBrowser form;
			public string inputText;
		}

		static Dictionary<string, Func> dic = new Dictionary<string,Func>();
		static bool processing = false;

		static WaitingDialog waitingDialog = null;

		static InternalTool()
		{
			dic["$ResPopupFromIndex"] = ResPopupFromIndex;
			dic["$ResExtract"] = ResExtract;
			dic["$Find2ch"] = FindSubject;
		}

		public static bool Run(Twin2IeBrowser form, ToolItem item, string inputText)
		{
			string key = item.FileName;

			if (!dic.ContainsKey(key))
			{
				MessageBox.Show(key + "というパラメータは存在しません");
				return false;
			}

			Parameter p = new Parameter();
			p.form = form;
			p.inputText = inputText;

			Func f = dic[key];

			return f(p);
		}

		static bool ResPopupFromIndex(Parameter param)
		{
			int[] array = ResReference.GetArray(param.inputText);

			if (array.Length == 0)
			{
				MessageBox.Show("レス番号が入力されていないか、または正しいレス番号ではありません");
				return false;
			}

			if (!param.form.threadTabController.IsSelected)
			{
				MessageBox.Show("スレッドが選択されていません");
				return false;
			}


			param.form.threadTabController.Control.Popup(array);

			return true;
		}

		static bool ResExtract(Parameter param)
		{
			if (!param.form.threadTabController.IsSelected)
			{
				MessageBox.Show("スレッドが選択されていません");
				return false;
			}

			if (String.IsNullOrEmpty(param.inputText))
				return false;


			AbstractExtractor extractor = param.form.threadTabController.Control.BeginExtract();

			extractor.NewWindow = true;
			extractor.Options = SearchOptions.Regex;

			return extractor.InnerExtract(param.inputText, ResSetElement.All);
		}

		static bool FindSubject(Parameter param)
		{
			if (processing)
				return false;

			processing = true;
			
			X2chSubjectSearcher searcher = new X2chSubjectSearcher();
			searcher.Sorting = SubjectSearchSorting.Modified;
			searcher.ViewCount = 100;

			ThreadStart startMethod = delegate
			{
				try
				{
					SubjectSearchResult r = searcher.Search(param.inputText);

					MethodInvoker m = delegate
					{
						waitingDialog.Dispose();

						ThreadListControl list = param.form.listTabController.Create(Twin2IeBrowser.dummySearchBoardInfo, true);
						list.SetItems(Twin2IeBrowser.dummySearchBoardInfo, r.MatchThreads);
					};

					param.form.Invoke(m);
				}
				finally
				{
					processing = false;
					waitingDialog = null;
				}

			};

			Thread t = new Thread(startMethod);
			t.IsBackground = true;
			t.Start();

			waitingDialog = new WaitingDialog("検索結果を取得しています．．．");
			waitingDialog.Show(param.form);

			return true;
		}

		// v2.5.100
		public static bool FindSubject(Twin2IeBrowser owner, string searchString)
		{
			return FindSubject(new Parameter { form = owner, inputText = searchString });
		}
	}
}
