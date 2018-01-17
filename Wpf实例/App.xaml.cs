﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using VideoPlayer.Class;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using System.Timers;

namespace VideoPlayer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>  [StructLayout(LayoutKind.Explicit)]
    public partial class App : System.Windows.Application
    {
        public IntPtr hLib;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            /**
            * 当前用户是管理员的时候，直接启动应用程序
            * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
            */
            //获得当前登录的Windows用户标示
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            //判断当前登录用户是否为管理员
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                //如果是管理员，则直接运行
                //Player py = new Player();
                //py.Show();
                //VedioPlayer.Form1 form = new VedioPlayer.Form1();
                //form.Show();
                //UseDll();
                //USBJOY_DLL();
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Enabled = true;
                timer.Interval = 200;
                //设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
                timer.AutoReset = true;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_USBJOY_DLL);
            }
            else
            {
                //创建启动对象
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //设置运行文件
                startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
                //设置启动参数
                //startInfo.Arguments = String.Join(" ", Args);
                //设置启动动作,确保以管理员身份运行
                startInfo.Verb = "runas";
                //如果不是管理员，则启动UAC
                System.Diagnostics.Process.Start(startInfo);
                //退出
                System.Windows.Forms.Application.Exit();
            }
        }
        /// <summary>
        /// 定时器处理的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_USBJOY_DLL(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            //第一个参数为Dll包含方法的名称，第二个参数为Dll方法里面为数组的大小
            USBJOY_DLL("SQ_GET_2DOF_9P", 4);
            USBJOY_DLL("SQ_GET_2DOF", 4);
            USBJOY_DLL("SQ_GET_3DOF", 5);
            USBJOY_DLL("SQ_GET_3DOF_SWING_LINK", 5);
            USBJOY_DLL("SQ_GET_6DOF", 6);
        }
        //[DllImport(@"../../Release/CreateDLL.dll", EntryPoint = "test01", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        //extern static byte test01(ref byte a, int b, int c);
        //[DllImport(@"../../Release/CreateDLL.dll", EntryPoint = "test02", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        //extern static int test02(int a, int b);
        ////调用dll文件
        //private void UseDll()
        //{
        //    byte a=1 ;
        //    byte r1 = test01(ref a, 2, 3);
        //    int r2 = test02(5, 2);
        //    Console.WriteLine("test01结果：" + r1.ToString());
        //    Console.WriteLine("test02结果：" + r2.ToString());
        //    Console.ReadKey();
        //}

        #region
        /// <summary>
        /// 动态调用Dll以及DLL里面的方法
        /// </summary>
        /// <param name="funcname"></param>
        /// <param name="k"></param>
        private void USBJOY_DLL(string funcname,int k)
        {
            //int hModule = DllInvoke.LoadLibrary(@"D:\dll\usbjoy-alldof\vs2010\x64\Release\USBJOY_DLL.dll");
            //IntPtr intPtr = DllInvoke.GetProcAddress(hModule, "GET_2DOF");
            //1. 动态加载C++ Dll
            int hModule = DllInvoke.LoadLibrary(@"../../Release/USBJOY_DLL.dll");
            if (hModule == 0) return;
            //2. 读取函数指针SQ_GET_2DOF
            IntPtr intPtr = DllInvoke.GetProcAddress(hModule, funcname);
            //3. 将函数指针封装成委托
            PSQ_GET_2DOF addFunction = (PSQ_GET_2DOF)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(PSQ_GET_2DOF));
            //4. 测试
            byte[] a = new byte[k];
            addFunction(ref a[0]);
            //调用数组基类的静态方法Resize进行动态调整大小
            //Array.Resize<byte>(ref a, a.Length + 1);
            Console.WriteLine("结果:");
            for (int i=0;i<a.Length; i++) {
            Console.WriteLine(a[i] + "");  
            }
            Console.WriteLine("----------------------------------");
            DllInvoke.FreeLibrary(hModule);
        }





    }  
    #endregion
    /// <summary>
    /// 函数指针,声明委托
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    delegate int PSQ_GET_2DOF(ref byte a);
}
