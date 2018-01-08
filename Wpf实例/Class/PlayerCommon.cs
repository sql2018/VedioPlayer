using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AxShockwaveFlashObjects;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms.Integration;
using System.Drawing;
using System.Xml;
namespace VideoPlayer.Class
{
    /// <summary>
    /// 截图类
    /// </summary>
    public class PlayerCommon
    {
       
        #region Flash调用控件方法
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, long dwRop);
        private const Int32 SRCCOPY = 0xCC0020;
        #endregion
         SecondScreen sc2 = new SecondScreen();
       
        ///<summary>
        ///flash截取图片操作
        /// </summary>
        public static string SaveFlashImage(AxShockwaveFlash control)
        {
            var flashW = control.Width;
            var flashH = control.Height;  
            string flashPath = "";
            string flashFormat = "";
            FileInfo finfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
            if (finfo.Exists)
            {
                //System.Windows.MessageBox.Show(flashPath + "");
                //读取XML文档信息
                XmlDocument flashXml = new XmlDocument();
                //从指定的Url加载xml文档
                flashXml.Load(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
                //找出xml中的Screen.xml中的Screen单个节点
                XmlNode flashNode = flashXml.SelectSingleNode("Screen");
                //获取根节点信息
                XmlElement flashElement = (XmlElement)flashNode;
                //获取保存图片位置
                flashPath = flashElement["Location"].InnerText + "\\";
                //获取保存图片格式
                //System.Windows.Forms.MessageBox.Show(flashPath+"");
                flashFormat = flashElement["Format"].InnerText; 
            }
            if (string.IsNullOrEmpty(flashPath))
            {
                flashPath = "C:\\";
            }
            if (string.IsNullOrEmpty(flashFormat))
            {
                flashFormat = ".bmp";
            }
            //GUID（全局统一标识符）
            string reValue = string.Format("{0}{1}" + flashFormat + "", flashPath, Guid.NewGuid().ToString());
            Graphics g0fCtrl = control.CreateGraphics();
            var bmp = new Bitmap(flashW, flashH, g0fCtrl);
            Graphics g0fBbmp = Graphics.FromImage(bmp);
            IntPtr dc1 = g0fCtrl.GetHdc();
            IntPtr dc2 = g0fBbmp.GetHdc();
            BitBlt(dc2, 0, 0, flashW, flashH, dc1, 0, 0, SRCCOPY);
            g0fCtrl.ReleaseHdc(dc1);
            g0fBbmp.ReleaseHdc(dc2);
            g0fCtrl.Dispose();
            g0fBbmp.Dispose();
            bmp.Save(reValue);
            return reValue;
        }

        /// <summary>
        /// 视频截取图片操作
        /// </summary>
        public static string SaveVideoImage(SecondScreen sc2)
        {
            Player PP = new Player();
            double PlayW = UserControlClass.sc2.FInkCanvas_Player.ActualWidth;
            double PlayH = UserControlClass.sc2.FInkCanvas_Player.ActualHeight;

            string PlayPath = "";
            string PlayFormat = "";
            FileInfo finfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
            if (finfo.Exists)
            {
                //读取XML文档信息
                XmlDocument PlayXml = new XmlDocument();
                PlayXml.Load(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
                XmlNode PlayNode = PlayXml.SelectSingleNode("Screen");
                XmlElement PlayElement = (XmlElement)PlayNode;
                //获取保存图片位置
                PlayPath = PlayElement["Location"].InnerText + "\\";
                //获取保存图片格式
                PlayFormat = PlayElement["Format"].InnerText; 
            }
            if (string.IsNullOrEmpty(PlayPath))
            {
                PlayPath = "C:\\";
            }
            if (string.IsNullOrEmpty(PlayFormat))
            {
                PlayFormat = ".bmp";
            }

            //GUID（全局统一标识符）
            string reValue = string.Format("{0}{1}" + PlayFormat + "", PlayPath, Guid.NewGuid().ToString());
            RenderTargetBitmap RenderBit = new RenderTargetBitmap(Convert.ToInt32(PlayW), Convert.ToInt32(PlayH), 1 / 200, 1 / 200, PixelFormats.Pbgra32);

            DrawingVisual PlayVisual = new DrawingVisual();
            DrawingContext PlayContext = PlayVisual.RenderOpen();
            PlayContext.DrawVideo(UserControlClass.MPPlayer, new Rect(0, 0, PlayW, PlayH));
            PlayContext.Close();
            RenderBit.Render(PlayVisual);

            if (PlayFormat == ".bmp")
            {
                BmpBitmapEncoder bmpEncoder = new BmpBitmapEncoder();
                bmpEncoder.Frames.Add(BitmapFrame.Create(RenderBit));
                FileStream fileStream = new FileStream(reValue, FileMode.Create, FileAccess.ReadWrite);
                bmpEncoder.Save(fileStream);
                fileStream.Close();
                System.Windows.Forms.MessageBox.Show("保存位置:" + reValue);
                return reValue;
            }
            else if (PlayFormat == ".jpg")
             {
                JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
                jpgEncoder.Frames.Add(BitmapFrame.Create(RenderBit));
                FileStream fileStream = new FileStream(reValue, FileMode.Create, FileAccess.ReadWrite);
                jpgEncoder.Save(fileStream);
                fileStream.Close();
                System.Windows.Forms.MessageBox.Show("保存位置:" + reValue);
                return reValue;
            }
            else
            {
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(RenderBit));
                FileStream fileStream = new FileStream(reValue, FileMode.Create, FileAccess.ReadWrite);
                pngEncoder.Save(fileStream);
                fileStream.Close();
                System.Windows.Forms.MessageBox.Show("保存位置:" + reValue);
                return reValue;
            }
        }
        ///<summary>
        ///截取摄像头文件图片
        ///</summary>
        public static string SaveCameraImage(SecondScreen win)
        {
            double CameraW = win.FInkCanvas_Camera.ActualWidth;
            double CameraH = win.FInkCanvas_Camera.ActualHeight;
            string CameraPath = "";
            string CameraFormat = "";
            FileInfo finfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
            if (finfo.Exists)
            {
                //读取XML文档信息
                XmlDocument CameraXml = new XmlDocument();
                CameraXml.Load(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
                XmlNode CameraNode = CameraXml.SelectSingleNode("Screen");
                XmlElement CameraElement = (XmlElement)CameraNode;
                //获取保存图片位置
                CameraPath = CameraElement["Location"].InnerText + "\\";
                //获取保存图片格式
                CameraFormat = CameraElement["Format"].InnerText; 
            }

            if (string.IsNullOrEmpty(CameraPath))
            {
                CameraPath = "C:\\";
            }
            if (string.IsNullOrEmpty(CameraFormat))
            {
                CameraFormat = ".bmp";
            }

            //GUID（全局统一标识符）
            string reValue = string.Format("{0}{1}" + CameraFormat + "", CameraPath, Guid.NewGuid().ToString());
            RenderTargetBitmap RenderBit = new RenderTargetBitmap(Convert.ToInt32(CameraW), Convert.ToInt32(CameraH), 1 / 200, 1 / 200, PixelFormats.Pbgra32);

            DrawingVisual CameraVisual = new DrawingVisual();
            CameraVisual.Transform = new RotateTransform(180, CameraW / 2.0, CameraH / 2.0);
            DrawingContext CameraContext = CameraVisual.RenderOpen();
           // CameraContext.DrawImage(win.FInkCanvas_Camera.Source, new Rect(0, 0, CameraW, CameraH));
            CameraContext.PushTransform(new RotateTransform(180, CameraW / 2.0, CameraH / 2.0));
            CameraContext.Close();
            RenderBit.Render(CameraVisual);

            if (CameraFormat == ".bmp")
            {
                BmpBitmapEncoder bmpEncoder = new BmpBitmapEncoder();
                bmpEncoder.Frames.Add(BitmapFrame.Create(RenderBit));
                FileStream fileStream = new FileStream(reValue, FileMode.Create, FileAccess.ReadWrite);
                bmpEncoder.Save(fileStream);
                fileStream.Close();
                System.Windows.Forms.MessageBox.Show("保存位置:" + reValue);
                return reValue;
            }
            else if (CameraFormat == ".jpg")
            {
                JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
                jpgEncoder.Frames.Add(BitmapFrame.Create(RenderBit));
                FileStream fileStream = new FileStream(reValue, FileMode.Create, FileAccess.ReadWrite);
                jpgEncoder.Save(fileStream);
                fileStream.Close();
                System.Windows.Forms.MessageBox.Show("保存位置:" + reValue);
                return reValue;
            }
            else
            {
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(RenderBit));
                FileStream fileStream = new FileStream(reValue, FileMode.Create, FileAccess.ReadWrite);
                pngEncoder.Save(fileStream);
                fileStream.Close();
                System.Windows.Forms.MessageBox.Show("保存位置:" + reValue);
                return reValue;
            }
        }

        ///<summary>
        ///加载flash文件信息
        ///</summary>
        ///<param name="FlashShow">显示flash的画板</param>
        ///<param name="FlashPath">flash的文件路径</param>
        public static void OpenFlash(SecondScreen win, string flashPath)
        {
            //AxShockwaveFlash成员函数用法 https://www.cnblogs.com/yuankexiong313/archive/2010/06/04/1751166.html
            // stage.scaleMode = StageScaleMode.NO_SCALE
            //初始化 WindowsFormsHost 类的新实例
            win.FlashFormHost = new WindowsFormsHost();
            
            //初始化 AxShockwaveFlash 类的新实例
            win.FlashShock = new AxShockwaveFlash();
            
            //缩放模式，‘0’--在控件内显示全部影片区域，保持影片 长宽比例不变，影片的大小决定于控件长或宽中较小的一 边 。 
            win.FlashShock.ScaleMode = 0;

            ((System.ComponentModel.ISupportInitialize)(win.FlashShock)).BeginInit();

            //设置flash文件播放的宽高
            //win.FlashFormHost.Width = win.InkCanvas_Player.ActualWidth;
            //win.FlashFormHost.Height = win.InkCanvas_Player.ActualHeight;

            //装载AxShockwaveFlash
            win.FlashFormHost.Child = win.FlashShock;

            //将host对象嵌入FlashGrid
            UserControlClass.sc2.FInkCanvas_Player.Children.Add(win.FlashFormHost);
            ((System.ComponentModel.ISupportInitialize)(win.FlashShock)).EndInit();

            //给flashshock赋值路径
            win.FlashShock.Movie = flashPath;
        }
        internal static void OpenFlash()
        {
            throw new NotImplementedException();
        }
        internal static void OpenFlash(Player player, Player player_2, string p)
        {
            throw new NotImplementedException();
        }

    }
}
