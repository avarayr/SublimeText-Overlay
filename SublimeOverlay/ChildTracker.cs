using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SublimeOverlay
{
    class ChildTracker
    {
        public delegate void ChildMinimizedHandler();
        public static event ChildMinimizedHandler ChildMinimized;

        static IntPtr hhook = IntPtr.Zero;
        static IntPtr windowHWND = IntPtr.Zero;

        const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
        const uint WINEVENT_OUTOFCONTEXT = 0;
        static NativeMethods.WinEventDelegate procDelegate = new NativeMethods.WinEventDelegate(WinEventProc);

        public static void Hook(IntPtr hWnd)
        {
            hhook = NativeMethods.SetWinEventHook(EVENT_SYSTEM_MINIMIZESTART, EVENT_SYSTEM_MINIMIZESTART, IntPtr.Zero,
                    procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
            windowHWND = hWnd;
        }
        public static void Unhook()
        {
            if (hhook != IntPtr.Zero)
                NativeMethods.UnhookWinEvent(hhook);
        }
        static void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd != windowHWND)
            {
                return;
            }
            NativeMethods.WINDOWPLACEMENT windowPlacement = new NativeMethods.WINDOWPLACEMENT();
            NativeMethods.GetWindowPlacement(hwnd, ref windowPlacement);
            windowPlacement.showCmd = 9; /* SW_RESTORE */
            NativeMethods.SetWindowPlacement(hwnd, ref windowPlacement);
            ChildMinimized();
        }
    }
}

