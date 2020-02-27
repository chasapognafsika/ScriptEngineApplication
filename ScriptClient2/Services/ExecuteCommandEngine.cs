using NLog;
using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ScriptClient.Services
{
    class ExecuteCommandEngine
    {
        private static readonly ILogger ErrorLogger = LogManager.GetLogger("fileErrorLogger");
        private IntPtr _selectedHWND = IntPtr.Zero;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_SHOWWINDOW = 0x0040;

        public bool SendKeys(string str)
        {
            try
            {
                System.Windows.Forms.SendKeys.SendWait(str);
                Console.WriteLine($"SendKeys({str})");
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex);
                return false;
            }
        }

        public bool SetCursor(int X, int Y)
        {
            try
            {
                Win32.POINT p = new Win32.POINT();
                p.x = X;
                p.y = Y;
                Win32.ClientToScreen(_selectedHWND, ref p);
                Win32.SetCursorPos(p.x, p.y);
                Console.WriteLine($"SetCursor({X},{Y})");
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex);
                return false;
            }
        }

        // returns the HWND of the window (if found), otherwise IntPtr.Zero
        public bool SelectWindow(string win_title)
        {
            try
            {
                StringBuilder sb = new StringBuilder(win_title);
                if (win_title.Contains("\""))
                    sb.Replace("\"", "");
                string transformedString = sb.ToString();

                _selectedHWND = Win32.FindWindow(null, transformedString);
                if (_selectedHWND != IntPtr.Zero)
                {
                    Win32.SetWindowPos(_selectedHWND, new IntPtr(0), 0, 0, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW);
                    Win32.ShowWindow(_selectedHWND, 5);
                    Win32.SetForegroundWindow(_selectedHWND);
                    Console.WriteLine($"SelectWindow({win_title}): SUCCESS :: _selectedHWND={_selectedHWND}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex);
                return false;
            }
            Console.WriteLine($"SelectWindow({win_title}): Failure :: _selectedHWND={_selectedHWND}");
            return false;
        }

        public bool DoMouseClick()
        {
            try
            {
                //Call the imported function with the cursor's current position
                uint X = (uint)Cursor.Position.X;
                uint Y = (uint)Cursor.Position.Y;
                Win32.mouse_event(Win32.MOUSEEVENTF_LEFTDOWN | Win32.MOUSEEVENTF_LEFTUP, X, Y, 0, UIntPtr.Zero);
                Console.WriteLine($"DoMouseClick");
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex);
                return false;
            }
        }

    }

    public class Win32
    {
        [DllImport("user32.Dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_MOVE = 0x0001;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string sClass, string sWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int X);

    }
}
