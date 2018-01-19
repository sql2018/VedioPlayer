using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VideoPlayer.Class;
namespace VideoPlayer
{
    /// <summary>
    /// ShowChange.xaml 的交互逻辑
    /// </summary>
    public partial class ShowChange : Window
    {
        byte[] a = new byte[2];
        byte[] b = new byte[2];
        byte[] c = new byte[3];
        byte[] d = new byte[3];
        byte[] f = new byte[6];
        public ShowChange()
        {
            InitializeComponent();
            Timer_Dll();
        }     
        private void Timer_Dll()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 50;
            //设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(timer_USBJOY_DLL);
            //if (a.Length > 1)
            //{ 
            //    lab_one_Box1.SetBinding(TextBox.TextProperty, new Binding("/") { Source = a[0].ToString() });
            //    lab_one_Box2.SetBinding(TextBox.TextProperty, new Binding("/") { Source = a[1].ToString() });
            //}
        }
        /// <summary>
        /// 定时器处理的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void timer_USBJOY_DLL(object sender, ElapsedEventArgs e)
        {
            //第一个参数为Dll包含方法的名称，第二个参数为Dll方法里面为数组的大小
            a=USBJOY_DLL("SQ_GET_2DOF_9P", 2);
            //MessageBox.Show(a[0].ToString()+"----"+a[1].ToString());
            if (a.Length > 0)
            {
                this.lab_one_Box1.Dispatcher.Invoke(new Action(() => { this.lab_one_Box1.Text = a[0].ToString(); }));
                this.lab_one_Box2.Dispatcher.Invoke(new Action(() => { this.lab_one_Box2.Text = a[1].ToString(); }));
            }
            b = USBJOY_DLL("SQ_GET_2DOF", 2);  
            if (b.Length > 0)
            {
                this.lab_two_Box1.Dispatcher.Invoke(new Action(() => { this.lab_two_Box1.Text = b[0].ToString(); }));
                this.lab_two_Box2.Dispatcher.Invoke(new Action(() => { this.lab_two_Box2.Text = b[1].ToString(); }));
            }
            c =USBJOY_DLL("SQ_GET_3DOF", 3);
            if (c.Length > 0)
            {
                this.lab_three_Box1.Dispatcher.Invoke(new Action(() => { this.lab_three_Box1.Text = c[0].ToString(); }));
                this.lab_three_Box2.Dispatcher.Invoke(new Action(() => { this.lab_three_Box2.Text = c[1].ToString(); }));
                this.lab_three_Box3.Dispatcher.Invoke(new Action(() => { this.lab_three_Box3.Text = c[2].ToString(); }));
            }
            d=USBJOY_DLL("SQ_GET_3DOF_SWING_LINK", 3);
            if (d.Length > 0)
            {
                this.lab_four_Box1.Dispatcher.Invoke(new Action(() => { this.lab_four_Box1.Text = d[0].ToString(); }));
                this.lab_four_Box2.Dispatcher.Invoke(new Action(() => { this.lab_four_Box2.Text = d[1].ToString(); }));
                this.lab_four_Box3.Dispatcher.Invoke(new Action(() => { this.lab_four_Box3.Text = d[2].ToString(); }));
            }
           f=USBJOY_DLL("SQ_GET_6DOF", 6);
            if (f.Length > 0)
            {
                this.lab_five_Box1.Dispatcher.Invoke(new Action(() => { this.lab_five_Box1.Text = f[0].ToString(); }));
                this.lab_five_Box2.Dispatcher.Invoke(new Action(() => { this.lab_five_Box2.Text = f[1].ToString(); }));
                this.lab_five_Box3.Dispatcher.Invoke(new Action(() => { this.lab_five_Box3.Text = f[2].ToString(); }));
                this.lab_five_Box4.Dispatcher.Invoke(new Action(() => { this.lab_five_Box4.Text = f[3].ToString(); }));
                this.lab_five_Box5.Dispatcher.Invoke(new Action(() => { this.lab_five_Box5.Text = f[4].ToString(); }));
                this.lab_five_Box6.Dispatcher.Invoke(new Action(() => { this.lab_five_Box6.Text = f[5].ToString(); }));
            }
        }
        /// <summary>
        /// 动态调用Dll以及DLL里面的方法
        /// </summary>
        /// <param name="funcname"></param>
        /// <param name="k"></param>
        private byte[] USBJOY_DLL(string funcname, int k)
        {
            //int hModule = DllInvoke.LoadLibrary(@"D:\dll\usbjoy-alldof\vs2010\x64\Release\USBJOY_DLL.dll");
            //IntPtr intPtr = DllInvoke.GetProcAddress(hModule, "GET_2DOF");
            //1. 动态加载C++ Dll
          
            int hModule = DllInvoke.LoadLibrary(@"../../Release/USBJOY_DLL.dll");
            if (hModule == 0) return null ;
            //2. 读取函数指针SQ_GET_2DOF
            IntPtr intPtr = DllInvoke.GetProcAddress(hModule, funcname);
            //3. 将函数指针封装成委托
            PSQ_GET_2DOF addFunction = (PSQ_GET_2DOF)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(PSQ_GET_2DOF));
            //4. 测试
            byte[] a = new byte[k];
            //addFunction(ref a[0]);
            this.Dispatcher.Invoke(new Action(() => {addFunction(ref a[0]);}));
            //DllInvoke.FreeLibrary(hModule);
            return a;
            
        }

    }  
}
