using System;
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
                Player py = new Player();
                py.Show();
                //VedioPlayer.Form1 form = new VedioPlayer.Form1();
                //form.Show();
                //ShowChange sc = new ShowChange();
                //sc.Show();
                //启动一个线程来实时更新摇杆所反馈的数据 
                //System.Timers.Timer timer = new System.Timers.Timer();
                //timer.Enabled = true;
                //timer.Interval = 50;
                //设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
                //timer.AutoReset = true;
                //timer.Elapsed += new ElapsedEventHandler(timer_USBJOY_DLL);

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
        /// 定时器处理的事件，实时更新DLL文件获取遥感所反馈的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_USBJOY_DLL(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            //第一个参数为Dll包含方法的名称，第二个参数为Dll方法里面为数组的大小
            //方法--九座二自由度
            USBJOY_DLL("SQ_GET_2DOF_9P", 4); 
        }
        private void USBJOY_DLL(string funcname, int k)
        {
            //1. 动态加载C++ Dll
            int hModule = DllInvoke.LoadLibrary(@"../../Release/USBJOY_DLL.dll");
            if (hModule == 0) return ;
            //2. 读取函数指针SQ_GET_2DOF
            IntPtr intPtr = DllInvoke.GetProcAddress(hModule, funcname);
            //3. 将函数指针封装成委托
            PSQ_GET_2DOF addFunction = (PSQ_GET_2DOF)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(PSQ_GET_2DOF));
            //4. 测试
            byte[] a = new byte[k];
            addFunction(ref a[0]);
            for (int i=0;i<a.Length;i++) {
            Console.WriteLine(a[i]);
            }
            //DllInvoke.FreeLibrary(hModule); //释放Dll文件
        }
    }
    /// <summary>
    /// 函数指针,声明委托
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    delegate int PSQ_GET_2DOF(ref byte a);
}
