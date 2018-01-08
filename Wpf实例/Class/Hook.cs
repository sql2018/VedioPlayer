using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
namespace VideoPlayer.Class
{
    public class Hook
    {

        public delegate int KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        static int hKeyboardHook = 0;
        KeyboardProc KeyboardHookProcedure;
        public const int WH_KEYBOARD = 13;
        public const int WH_MOUSE_LL = 14;
        public struct KeyboardMSG
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
            public int VK_CONTROL;
            public int VK_MENU;
            public int VK_DELETE;
        }
        [DllImport("kernel32")]
        public static extern int GetCurrentThreadId();
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        private int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KeyboardMSG m = (KeyboardMSG)Marshal.PtrToStructure(lParam, typeof(KeyboardMSG));
            if ((int)m.vkCode == 91 || (int)m.vkCode == 92 || (int)m.vkCode == 10)
            {
                return 1;
            }
            if (((int)m.vkCode == 46) && ((int)m.vkCode == 17) && ((int)m.vkCode == 18))
            {
                return 2;
            }
            if (m.vkCode == (int)Keys.Escape && (int)Control.ModifierKeys == (int)Keys.Control) //截获Ctrl+Esc 
            {
                return 1;
            }

            if (m.vkCode == (int)Keys.Escape && (int)Control.ModifierKeys == (int)Keys.Alt) //截获Alt+Esc 
            {
                return 1;
            }
            if (m.vkCode == (int)Keys.F4 && (int)Control.ModifierKeys == (int)Keys.Alt) //截获alt+f4 
            {
                return 1;
            }
            if (m.vkCode == (int)Keys.Tab && (int)Control.ModifierKeys == (int)Keys.Alt) //截获alt+tab
            {
                return 1;
            }
            if (m.vkCode == (int)Keys.Escape && (int)Control.ModifierKeys == (int)Keys.Control + (int)Keys.Shift) //截获Ctrl+Shift+Esc
            {
                return 1;
            }
            if (m.vkCode == (int)Keys.Space && (int)Control.ModifierKeys == (int)Keys.Alt) //截获alt+空格 
            {
                return 1;
            }
            if (m.vkCode == (int)Keys.Delete && (int)Control.ModifierKeys == (int)Keys.Control + (int)Keys.Alt)      //截获Ctrl+Alt+Delete 
            {
                return 1;
            }
            if (m.vkCode == (int)Keys.Space && (int)Control.ModifierKeys == (int)Keys.Control + (int)Keys.Alt) //截获Ctrl+Alt+空格 
            {
                return 1;
            }
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
        // 安装钩子
        public void KeyMaskStart()
        {
            if (hKeyboardHook == 0)
            {
                // 创建HookProc实例
                KeyboardHookProcedure = new KeyboardProc(KeyboardHookProc);
                // 设置线程钩子
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD, KeyboardHookProcedure,
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                // 如果设置钩子失败
                if (hKeyboardHook == 0)
                {
                    KeyMaskStop();
                    throw new Exception("SetWindowsHookEx failed.");
                }
            }
        }
        // 卸载钩子
        public void KeyMaskStop()
        {
            bool retKeyboard = true;
            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            if (!(retKeyboard))
            {
                throw new Exception("UnhookWindowsHookEx  failed.");
            }
        }




    }
}
