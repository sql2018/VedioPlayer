using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using VideoPlayer.Class;
namespace VideoPlayer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        
        public IntPtr hLib;
        public delegate IntPtr Test(ref byte pdate);
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
                Player py = new Player();
                py.Show();
                //UseDll();
                USBJOY_DLL();
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
        private void USBJOY_DLL()
        {
           // int hModule = DllInvoke.LoadLibrary(@"D:\dll\usbjoy-alldof\vs2010\x64\Release\USBJOY_DLL.dll");
           //IntPtr intPtr = DllInvoke.GetProcAddress(hModule, "GET_2DOF");
           //1. 动态加载C++ Dll
            int hModule = DllInvoke.LoadLibrary(@"../../Release/USBJOY_DLL.dll");
            if (hModule == 0) return;
           //2. 读取函数指针
            Console.WriteLine("test01结果");
            IntPtr intPtr = DllInvoke.GetProcAddress(hModule, "SQ_GET_2DOF");
           //3. 将函数指针封装成委托
            PSQ_GET_2DOF addFunction = (PSQ_GET_2DOF)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(PSQ_GET_2DOF));
            //4. 测试
            byte a=1;
            addFunction(ref a);
            Console.WriteLine();
            Console.Read();
            DllInvoke.FreeLibrary(hModule);
         }
        /// <summary>
        /// 函数指针
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate int PSQ_GET_2DOF(ref byte a);
    }
}
