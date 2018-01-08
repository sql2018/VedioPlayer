using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// RemoveImage.xaml 的交互逻辑
    /// </summary>
    public partial class RemoveImage : Window
    {
        private Player ply;
        //声明一个Image的集合
        /// <summary>
        /// 图片集
        /// </summary>
        /// 
        ObservableCollection<Image> Imagelist = new ObservableCollection<Image>();
        public RemoveImage(Player Player)
        {
            ply = Player;
            InitializeComponent();
            FindImagefiles();
            listview.ContextMenu= getListMenu();
        }
        /// <summary>
        /// 获取图片路径，并插入到listview中去
        /// </summary>
        private void FindImagefiles()
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "images");
            foreach (var file in di.GetFiles())
            {
                //验证是否为图片文件 
                if (file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".jpeg" || file.Extension.ToLower() == ".bmp" || file.Extension.ToLower() == ".ico" || file.Extension.ToLower() == ".gif")
                {
                    try
                    {
                        Image image = new Image();
                        image.Stretch = Stretch.UniformToFill;
                        image.Width = 100;
                        image.Height = 150;
                        image.DataContext = file.FullName;
                        image.Source = new BitmapImage(new Uri(file.FullName));
                        Imagelist.Add(image);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
            listview.ItemsSource = Imagelist;
        }
        /// <summary>
        /// 写一个contextmenu插入各个图片选项
        /// </summary>
        /// <returns></returns>
        public System.Windows.Controls.ContextMenu getListMenu()
        {
            System.Windows.Controls.MenuItem itemDelete;
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            itemDelete = new System.Windows.Controls.MenuItem();
            itemDelete.Tag = "Delete";
            itemDelete.Header = "  删   除  ";
            itemDelete.Click += new RoutedEventHandler(itemDelete_Click);
            contextMenu.Items.Add(itemDelete);

            itemDelete = new System.Windows.Controls.MenuItem();
            itemDelete.Tag = "DeleteAll";
            itemDelete.Header = "删 除 所 有";
            itemDelete.Click += new RoutedEventHandler(itemDelete_Click);
            contextMenu.Items.Add(itemDelete);

            return contextMenu;
        }
        /// <summary>
        /// 判断选项menuitem的tag,从而执行不同的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDelete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem item = (System.Windows.Controls.MenuItem)sender;
            string stringName = item.Tag.ToString();
            switch (stringName)
            {
                case "Delete":
                    RemoveImages();
                    break;
                case "DeleteAll":
                    MessageBoxResult dr = MessageBox.Show("确定要删除所有图片吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (dr == MessageBoxResult.OK)
                    {
                    RemoveAllImage();
                    }
                    break;
            }
        }
        /// <summary>
        /// 删除所有图片
        /// </summary>
        private void RemoveAllImage()
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+@"\images");
            foreach (var file in files)
            {
            File.Delete(file);
            }
            ply.data.Clear();
            this.Close();
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        private void RemoveImages(){
            int index = listview.SelectedIndex;
            if (index != -1)
            {
              //获取路径
              string path= Imagelist[index].DataContext.ToString();
              //BitmapImage img = (BitmapImage)listview.SelectedItem;
              //文件已打开，无法执行
              Imagelist.Remove(Imagelist[index]);
              File.Delete(path);
              ply.data.Clear();
              UserControlClass.ListviewAddImage(ply.data);
            }
        }
        /// <summary>
        /// 窗口关闭要执行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ply.ShowActivated = true;
            ply.Show();
            if (UserControlClass.FileName != null)
            {
                ply.ListView.SelectedValue = UserControlClass.FileName;
            }
        }

    }
}