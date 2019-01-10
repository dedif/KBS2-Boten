using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, UnmanagedCode = true)]

namespace System.Windows.Forms
{
    //Deze klasse wordt gebruikt om een timer aan de messageBox toe tevoegen, waardoor de messagebox na een bepaalde tijd automatisch sluit.
    public class MessageBoxEx
    {


        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, uint uTimeout)
        {
            Setup(caption, uTimeout);
            return MessageBox.Show(text, caption, buttons);
        }


        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

        public const int WH_CALLWNDPROCRET = 12;
        public const int WM_DESTROY = 0x0002;
        public const int WM_INITDIALOG = 0x0110;
        public const int WM_TIMER = 0x0113;
        public const int WM_USER = 0x400;
        public const int DM_GETDEFID = WM_USER + 0;

      
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        private const int TimerID = 42;
        private static HookProc hookProc;
        private static TimerProc hookTimer;
        private static uint hookTimeout;
        private static string hookCaption;
        private static IntPtr hHook;


        private static void Setup(string caption, uint uTimeout)
        {
            if (hHook != IntPtr.Zero)
                throw new NotSupportedException("multiple calls are not supported");

            hookTimeout = uTimeout;
            hookCaption = caption != null ? caption : "";
            hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
        }

     
    }
}