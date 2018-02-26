namespace Twin.Forms
{
	using System;
	using System.Runtime.InteropServices;

	#region HRESULT definition
	/// <summary>
	/// HRESULT definition
	/// </summary>
//	[CLSCompliant(false)]
	public enum HRESULT : uint
	{
		/// <summary></summary>
		S_OK = 0,

		/// <summary></summary>
		S_FALSE = 1,

		/// <summary></summary>
		E_UNEXPECTED = 0x8000ffff,

		/// <summary></summary>
		E_NOTIMPL = 0x80004001,

		/// <summary></summary>
		E_OUTOFMEMORY = 0x8007000e,

		/// <summary></summary>
		E_INVALIDARG = 0x80070057,

		/// <summary></summary>
		E_NOINTERFACE = 0x80004002,

		/// <summary></summary>
		E_POINTER = 0x80004003,

		/// <summary></summary>
		E_HANDLE = 0x80070006,

		/// <summary></summary>
		E_ABORT = 0x80004004,

		/// <summary></summary>
		E_FAIL = 0x80004005,

		/// <summary></summary>
		E_ACCESSDENIED = 0x80074005
	}
	#endregion

	/// <summary>
	/// DOCHOSTUIINFO
	/// </summary>
	[ StructLayout( LayoutKind.Sequential )]
	public struct DOCHOSTUIINFO
	{
		uint cbSize;
		uint dwFlags;
		uint dwDoubleClick;
		[MarshalAs(UnmanagedType.BStr)] string pchHostCss;
		[MarshalAs(UnmanagedType.BStr)] string pchHostNS;
	}

	/// <summary>
	/// tagPOINT
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	public struct tagPOINT
	{
		int x;
		int y;
	} 

	/// <summary>
	/// tagMSG
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	public struct tagMSG 
	{
		public IntPtr hwnd;
		public uint message;
		public uint wParam;
		public uint lParam;
		public uint time;
		public tagPOINT pt;
	} 

	/// <summary>
	/// tagRECT
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	public struct tagRECT
	{
		/// <summary></summary>
		public int left;

		/// <summary></summary>
		public int top;

		/// <summary></summary>
		public int right;

		/// <summary></summary>
		public int bottom;
	} 

	/// <summary>
	/// Summary description for IDocHostUIHandler.
	/// </summary>
//	[CLSCompliant(false)]
	[ComImport, Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDocHostUIHandler
	{
		/// <summary>
		/// Called by MSHTML to display a shortcut menu.
		/// </summary>
		[PreserveSig]
		HRESULT ShowContextMenu([In] uint dwID, [In] ref tagPOINT ppt,
			[In, MarshalAs(UnmanagedType.IUnknown)] object pcmdtReserved,
			[In, MarshalAs(UnmanagedType.IDispatch)] object pdispReserved);

		/// <summary>
		/// Called by MSHTML to retrieve the user interface (UI) capabilities of
		/// the application that is hosting MSHTML.
		/// </summary>
		[PreserveSig]
		HRESULT GetHostInfo([In, Out] ref DOCHOSTUIINFO pInfo);

		/// <summary>
		/// Called by MSHTML to enable the host to replace MSHTML menus and toolbars.
		/// </summary>
		[PreserveSig]
		HRESULT ShowUI([In] uint dwID,
			[In, MarshalAs(UnmanagedType.Interface)] IntPtr pActiveObject,
			[In, MarshalAs(UnmanagedType.Interface)] IntPtr pCommandTarget,
			[In, MarshalAs(UnmanagedType.Interface)] IntPtr pFrame,
			[In, MarshalAs(UnmanagedType.Interface)] IntPtr pDoc);

		/// <summary>
		/// Called when MSHTML removes its menus and toolbars.
		/// </summary>
		[PreserveSig]
		HRESULT HideUI();

		/// <summary>
		/// Called by MSHTML to notify the host that the command state has changed.
		/// </summary>
		[PreserveSig]
		HRESULT UpdateUI();

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::EnableModeless.
		/// Also called when MSHTML displays a modal UI.
		/// </summary>
		[PreserveSig]
		HRESULT EnableModeless([In] int fEnable);

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::OnDocWindowActivate.
		/// </summary>
		[PreserveSig]
		HRESULT OnDocWindowActivate([In] int fActivate);

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::OnFrameWindowActivate.
		/// </summary>
		[PreserveSig]
		HRESULT OnFrameWindowActivate([In] int fActivate);

		/// <summary>
		/// Called by the MSHTML implementation of IOleInPlaceActiveObject::ResizeBorder.
		/// </summary>
		[PreserveSig]
		HRESULT ResizeBorder([In] ref tagRECT prcBorder,
			[In, MarshalAs(UnmanagedType.Interface)] IntPtr pUIWindow,
			[In] int fRameWindow);

		/// <summary>
		/// Called by MSHTML when IOleInPlaceActiveObject::TranslateAccelerator
		/// or IOleControlSite::TranslateAccelerator is called.
		/// </summary>
		[PreserveSig]
		HRESULT TranslateAccelerator([In] ref tagMSG lpmsg, [In] ref Guid pguidCmdGroup, [In] uint nCmdID);

		/// <summary>
		/// Called by the WebBrowser Control to retrieve a registry subkey path
		/// that overrides the default Microsoft® Internet Explorer registry settings.
		/// </summary>
		[PreserveSig]
		HRESULT GetOptionKeyPath([Out, MarshalAs(UnmanagedType.LPWStr)] out string pchKey, [In] uint dw);

		/// <summary>
		/// Called by MSHTML when it is used as a drop target. This method enables
		/// the host to supply an alternative IDropTarget interface.
		/// </summary>
		[PreserveSig]
		HRESULT GetDropTarget([In, MarshalAs(UnmanagedType.Interface)] IntPtr pDropTarget,
			[Out, MarshalAs(UnmanagedType.Interface)] out IntPtr ppDropTarget);

		/// <summary>
		/// Called by MSHTML to obtain the host's IDispatch interface.
		/// </summary>
		[PreserveSig]
		HRESULT GetExternal([Out, MarshalAs(UnmanagedType.IDispatch)] out object ppDispatch);

		/// <summary>
		/// Called by MSHTML to give the host an opportunity to modify the URL to be loaded.
		/// </summary>
		[PreserveSig]
		HRESULT TranslateUrl([In] uint dwTranslate,
			[In] ref ushort pchURLIn,
			[Out] IntPtr ppchURLOut);

		/// <summary>
		/// Called by MSHTML to allow the host to replace the MSHTML data object.
		/// </summary>
		[PreserveSig]
		HRESULT FilterDataObject([In, MarshalAs(UnmanagedType.Interface)] IntPtr pDO,
			[Out, MarshalAs(UnmanagedType.Interface)] out IntPtr ppDORet);
	}

	/// <summary>
	/// ICustomDoc Interface
	/// </summary>
//	[CLSCompliant(false)]
	[ComImport, Guid("3050F3F0-98B5-11CF-BB82-00AA00BDCE0B"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICustomDoc
	{
		/// <summary>
		/// SetUIHandler
		/// </summary>
		[PreserveSig]
		uint SetUIHandler(IDocHostUIHandler pUIHandler);
	}
}
