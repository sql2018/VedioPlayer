using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Win32;
using VideoPlayer.Class;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace VideoPlayer.Class
{
     /// <summary>
     /// 自定义公共类
     /// </summary>
     public  class  UserControlClass
    { 
        /// <summary>
        /// 登录的用户名
        /// </summary>
        public static string username="";
        /// <summary>
        /// 播放器实体类
        /// </summary>
        public static MediaPlayer MPPlayer = new MediaPlayer();
        /// <summary>
        /// 播放器状态
        /// </summary>
        public static MediaStatus MSStatus;
        /// <summary>
        /// Player的实例
        /// </summary>
        public static Player pp = new Player();
        /// <summary>
        /// SecondScreen的实例
        /// </summary>
        public static SecondScreen sc2 = new SecondScreen();
        /// <summary>
        /// 当前播放文件名
        /// </summary>
        public static string FileName;
        /// <summary>
        /// 得到程序集debug里面的images里面所有的图片名称
        /// </summary>
        /// <returns></returns>
        public static List<string> Getimages(){
            DirectoryInfo path = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory.ToString() + "images");
            FileInfo[] files = path.GetFiles();
            //"*.jpg|*.jpeg|*.png|*.bmp|*.gif,*.ico"
            List<string> lists = new List<string>();
            foreach (FileInfo file in files)
            {
                //筛选出图片文件添加到集合中去
                if (file.Name.EndsWith(".jpg") || file.Name.EndsWith(".png") || file.Name.EndsWith(".jpeg") || file.Name.EndsWith(".bmp") || file.Name.EndsWith(".gif") || file.Name.EndsWith(".ico"))
                {
                    //System.Windows.Forms.MessageBox.Show(file.Name);
                    lists.Add(file.Name);
                }
            }
            return lists;
        }
        /// <summary>
        /// 添加图片到效果图ListBox里面
        /// </summary>
        /// <param name=""></param>
        public static void Addimage(ObservableCollection<ImageInfo> data,string imagepath) {
                FileInfo picFile = new FileInfo(imagepath);
                string dest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "images\\" + Path.GetFileName(imagepath));
                //复制文件到指定目录下（1）为当前路径（2）为指定路径
                if (UserControlClass.Getimages().Contains(Path.GetFileName(imagepath)))
                {
                }
                else
                {
                if (data != null) {
                    File.Copy(imagepath, dest, true);
                    data.Add(new ImageInfo(dest));
                    // data.Remove(new ImageInfo(@"D:\Wpf-实例\bin\Debug\images\gd.jpeg"));
                    //Refresh.DoEvents();
                    System.Windows.Forms.MessageBox.Show("添 加 成 功 !");
                }
                }
        }
        /// <summary>
        /// 给ObservableCollection的data添加数据
        /// </summary>
        public static void ListviewAddImage(ObservableCollection<ImageInfo> data)
        {
            if (data!=null) {
                //获取程序所在目录中的images文件夹
                DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "images");
                //添加此目录中的图片文件到data中
                foreach (var file in di.GetFiles())
                {
                    //验证是否为图片文件
                    if (file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".jpeg" || file.Extension.ToLower() == ".bmp" || file.Extension.ToLower() == ".ico" || file.Extension.ToLower() == ".gif")
                    {
                        data.Add(new ImageInfo(file.FullName));
                    }
                }
            }
        }
        /// <summary>
        /// 当前显示器屏幕无边框最大化
        /// </summary>
        /// <param name="the"></param>
        public static void NullBorderWin(Window the) {
            var handle = new WindowInteropHelper(the).Handle;
            //获取当前显示器屏幕
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(handle);
            //调整窗口最大化
            the.Left = 0;
            the.Top = 0;
            the.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            the.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        }
        /// <summary>
        /// 开机自动启动
        /// </summary>
        /// <param name="started">设置开机启动，或取消开机启动</param>
        /// <param name="exeName">注册表中的名称</param>
        /// <returns>开启或停用是否成功</returns>
        public static void RegRun(string appName, bool f)
        {
            RegistryKey HKCU = Registry.CurrentUser;
           // RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            RegistryKey Run = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//打开注册表子项
            bool b = false;
            foreach (string i in Run.GetValueNames())
            {
                if (i == appName)
                {
                    b = true;
                    break;
                }
            }
            try
            {
                if (f)
                {
                    Run.SetValue(appName, System.Windows.Forms.Application.ExecutablePath);
                }
                else
                {
                    Run.DeleteValue(appName);
                }
            }
            catch
            { }
            HKCU.Close();
        }

    }
}
