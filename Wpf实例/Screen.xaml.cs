using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using DevComponents.WpfRibbon;

namespace VideoPlayer
{
    /// <summary>
    /// Screen.xaml 的交互逻辑
    /// </summary>
    public partial class Screen : Window
    {
        public Screen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenWin_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 点击浏览按钮打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLocation_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog FBDFile = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(this.tbLocation.Text))
            {
                FBDFile.SelectedPath = this.tbLocation.Text;
            }
            FBDFile.ShowDialog();
            tbLocation.Text = FBDFile.SelectedPath;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 更改写入XML文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FileInfo finfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
            if (finfo.Exists)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
                XmlNode childNodes = xmlDoc.SelectSingleNode("Screen");
                XmlElement element = (XmlElement)childNodes;
                element["Location"].InnerText = tbLocation.Text;
                element["Format"].InnerText = cbFormat.Text;
                xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
            }
            this.Close();
        }


        /// <summary>
        /// 加载窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenWin_Loaded(object sender, RoutedEventArgs e)
        {
            FileInfo finfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
            if (finfo.Exists)
            {
                XmlDocument ScreenXml = new XmlDocument();
                ScreenXml.Load(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Screen.xml");
                XmlNode ScreenNode = ScreenXml.SelectSingleNode("Screen");
                XmlElement element = (XmlElement)ScreenNode;
                tbLocation.Text = element["Location"].InnerText;
                cbFormat.Text = element["Format"].InnerText;
            }
        }
    }
}
