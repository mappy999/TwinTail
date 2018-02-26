using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ImageViewerDll
{
	internal class MultiLimitter
	{
		private Dictionary<string, Limitter> limitters = new Dictionary<string, Limitter>();
		private ParameterizedThreadStart callback;

		public MultiLimitter(ParameterizedThreadStart callback)
		{
			this.callback = callback;
		}

		public void Enqueue(string url, ServerRestrictInfo restrictInfo)
		{
			if (! limitters.ContainsKey(restrictInfo.ServerAddress))
			{
				limitters.Add(restrictInfo.ServerAddress, 
					new Limitter(restrictInfo.ServerAddress, restrictInfo, this.callback));
			}

			Limitter limitter = limitters[restrictInfo.ServerAddress];
			limitter.Enqueue(url);
		}

		public void ClearQueues()
		{
			foreach (Limitter l in limitters.Values)
			{
				l.ClearQueue();
			}
		}
	}

	internal class Limitter
	{
		private object syncObj = new object();
		private Thread thread = null;
		private ManualResetEvent resetEvent = new ManualResetEvent(true);
		private Queue<string> queue = new Queue<string>();
		private ParameterizedThreadStart downloadCallback;
		private string threadName;
		private bool running = true;
		private int current = 0;

		public ServerRestrictInfo RestrictInfo { get; private set; }

		public Limitter(string name, ServerRestrictInfo info, ParameterizedThreadStart callback)
		{
			this.threadName = name;
			this.RestrictInfo = info;
			this.downloadCallback = callback;
		}

		public void Enqueue(string url)
		{
			lock (syncObj)
			{
				if (thread == null)
				{
					thread = new Thread(Processing) { IsBackground = true };
					thread.Name = "LMTR_" + threadName;
					thread.Start();
				}
				lock (queue)
					queue.Enqueue(url);

				resetEvent.Set();
			}
		}

		/// <summary>
		/// 穏便にスレッドを終了させる。待機スレッドも終了。
		/// </summary>
		public void Stop()
		{
			lock (syncObj)
			{
				this.running = false;
				this.thread = null;
			}
		}

		/// <summary>
		/// 強制的にスレッドを終了する。待機スレッドも終了。
		/// </summary>
		public void Abort()
		{
			lock (syncObj)
			{
				if (thread != null)
					thread.Abort();
				thread = null;
			}
		}

		/// <summary>
		/// 現在残っているキューを全てクリア。待機スレッドは終了させない。
		/// </summary>
		public void ClearQueue()
		{
			lock (syncObj)
			{
				lock (queue)
					queue.Clear();
			}
		}

		private void Processing()
		{
			while (running)
			{
				int queueCount = queue.Count;

				if (current < this.RestrictInfo.ConnectionLimit && queueCount > 0)
				{
					current++;

					string url;
					lock (queue)
						url = queue.Dequeue();
					
					Thread thread = new Thread(delegate()
						{
							try
							{
								if (running)
									downloadCallback(url);
							}
							finally
							{
								current--;
								resetEvent.Set();
							}
						});
					thread.Name = this.threadName + "-" + current;
					thread.Priority = ThreadPriority.Lowest;
					thread.IsBackground = true;
					thread.Start();

					if (queue.Count > 0)
						Thread.Sleep(this.RestrictInfo.Interval);
				}
				else
				{
					resetEvent.Reset();

					if (queueCount == 0)
					{
						resetEvent.WaitOne();
					}
					else
					{
						resetEvent.WaitOne(2000);
					}
				}
			}

			resetEvent.Close();
		}

	}
}
