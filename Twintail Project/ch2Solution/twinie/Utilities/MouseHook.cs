using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Twin
{
	static class MouseHook
	{
		#region Inner Class
		public sealed class MouseHookEventArgs : EventArgs
		{
			#region Properties

			private bool cancel;
			public bool Cancel
			{
				get
				{
					return cancel;
				}
				set
				{
					cancel = value;
				}
			}

			private IntPtr nativeWParam;
			public IntPtr NativeWParam
			{
				get
				{
					return nativeWParam;
				}
			}

			private IntPtr nativeLParam;
			public IntPtr NativeLParam
			{
				get
				{
					return nativeLParam;
				}
			}

			private MouseHookStruct mhs;
			public MouseHookStruct MouseHookStruct
			{
				get
				{
					return mhs;
				}
			}
	

			private MouseButtons buttons;
			public MouseButtons MouseButtons
			{
				get
				{
					return buttons;
				}
			}

			private Point pt;
			public Point Location
			{
				get
				{
					return pt;
				}
			}

			private int delta;
			public int Delta
			{
				get
				{
					return delta;
				}
			}
			#endregion

			internal MouseHookEventArgs(IntPtr wParam, IntPtr lParam)
			{
				this.nativeWParam = wParam;
				this.nativeLParam = lParam;
				this.delta = 0;
				this.cancel = false;

				this.mhs = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
				this.pt = new Point(mhs.pt.x, mhs.pt.y);

				switch (wParam.ToInt32())
				{
					case WM_LBUTTONDOWN:
					case WM_LBUTTONUP:
						this.buttons = MouseButtons.Left;
						break;

					case WM_RBUTTONDOWN:
					case WM_RBUTTONUP:
						this.buttons = MouseButtons.Right;
						break;

					case WM_MBUTTONDOWN:
					case WM_MBUTTONUP:
						this.buttons = MouseButtons.Middle;
						break;

					default:
						this.buttons = MouseButtons.None;
						break;
				}

				if (wParam.ToInt32() == WM_MOUSEWHEEL)
					this.delta = (int)(short)(mhs.mouseData >> 16);
			}
		}
		#endregion

		#region Public Events
		//public static event EventHandler<MouseHookEventArgs> MouseDown;
		//public static event EventHandler<MouseHookEventArgs> MouseUp;
		//public static event EventHandler<MouseHookEventArgs> MouseMove;

		public static event EventHandler<MouseHookEventArgs> MouseWheel;
		#endregion

		#region Public Properties
		public static bool IsHook
		{
			get
			{
				return hHook != IntPtr.Zero;
			}
		}
		#endregion

		private static IntPtr hHook;
		private static HookProc mouseHookProc;

		static MouseHook()
		{
			mouseHookProc = new HookProc(MouseHookProc);

			IntPtr module = Marshal.GetHINSTANCE(typeof(MouseHook).Module);

			hHook = SetWindowsHookEx(WH_MOUSE_LL, mouseHookProc, module, 0);

			AppDomain.CurrentDomain.DomainUnload += delegate
			{
				if (hHook != IntPtr.Zero)
					UnhookWindowsHookEx(hHook);
			};
		}

		static IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			MouseHookEventArgs eventArgs = new MouseHookEventArgs(wParam, lParam);

			if (nCode == HC_ACTION)
			{
				switch (wParam.ToInt32())
				{
					//case WM_LBUTTONDOWN:
					//case WM_MBUTTONDOWN:
					//case WM_RBUTTONDOWN:
					//    CallEvent(MouseDown, eventArgs);
					//    break;

					//case WM_LBUTTONUP:
					//case WM_MBUTTONUP:
					//case WM_RBUTTONUP:
					//    CallEvent(MouseUp, eventArgs);
					//    break;

					//case WM_MOUSEMOVE:
					//    CallEvent(MouseMove, eventArgs);
					//    break;

					case WM_MOUSEWHEEL:
						CallEvent(MouseWheel, eventArgs);
						break;
				}

				if (eventArgs.Cancel)
					return (IntPtr)1;
			}
			
			return (IntPtr)CallNextHookEx(hHook, nCode, wParam, lParam);
		}

		private static void CallEvent(EventHandler<MouseHookEventArgs> handler, MouseHookEventArgs args)
		{
			if (handler != null)
				handler(null, args);
		}

		#region P/Invoke
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MouseHookStruct
		{
			public POINT pt;
			public int mouseData;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}

		[System.Runtime.InteropServices.UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
		
		[DllImport("kernel32.dll")]
		static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll")]
		static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr idHook);

		[DllImport("user32.dll")]
		static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll")]
		static extern int GetCurrentThreadId();

		const int HC_ACTION = 0;
		const int WHEEL_DELTA = 120;
		const int WH_MOUSE = 7;
		const int WH_MOUSE_LL = 14;
		const int WM_MOUSEMOVE = 0x200;
		const int WM_MOUSEWHEEL = 0x20A;
		const int WM_LBUTTONDOWN = 0x201;
		const int WM_LBUTTONUP = 0x202;
		const int WM_LBUTTONDBLCLK = 0x0203;
		const int WM_RBUTTONDBLCLK = 0x204;
		const int WM_RBUTTONDOWN = 0x205;
		const int WM_RBUTTONUP = 0x206;
		const int WM_MBUTTONDOWN = 0x207;
		const int WM_MBUTTONUP = 0x208;
		const int WM_MBUTTONDBLCLK = 0x0209;
		#endregion
	}

}