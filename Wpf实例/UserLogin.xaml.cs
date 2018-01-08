using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Data.SqlClient;
using VideoPlayer.Class;
namespace VideoPlayer
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class UserLogin : Window
    {
        private Player playerWin;
        public static string connstr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 11) + @"\Database1.mdf;Integrated Security=True";

        public Player PlayerWin
        {
            get
            {
                return playerWin;
            }

            set
            {
                playerWin = value;
            }
        }

        public UserLogin(Player win)
        {
            PlayerWin = win;
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            this.Close();
        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName != null && txtUserName.Text != "" && txtPwd.Password != null && txtPwd.Password != "")
            {
                SqlConnection conn = new SqlConnection(connstr);
                conn.Open();
                string sql = "select UserPwd from Users where Username='" + txtUserName.Text + "'";//Table是关键字，要加[]，查询数据库的表结构
                //对数据库执行的sql语句命令
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //使用 SqlDataReader来读取数据库 
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //如果查到用户名
                        if (sdr.Read())
                        {
                            //则把对应的密码读取出来
                            string pwd = sdr.GetString(0).Trim();
                            //把文本框输入的密码和取出的密码相等，则跳转
                            if (pwd == txtPwd.Password)
                            {
                                UserControlClass.username = txtUserName.Text;
                                this.Close();
                                playerWin.JugUser();  
                            }
                            //密码错误重新输入焦点集中在密码文本框并清空密码文本框
                            else
                            {
                                MessageBox.Show("密码错误，重新输入密码!\n\r\n\rWrong password, re - enter the password!");
                                txtPwd.Clear();
                                txtPwd.Focus();
                            }
                        }
                        //用户名不存在重新输入并把文本框清空
                        else
                        {
                            MessageBox.Show("用户名不存在,重新输入!\n\r\n\rThe username does not exist, reinput!");
                            txtUserName.Text = "";
                            txtUserName.Focus();
                        }
                    }
                }
                conn.Close();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            UserUpdate uup = new UserUpdate(playerWin);
            uup.ShowDialog();
        }
        /// <summary>
        /// 窗体按Esc键退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)//Esc键  
            {
                this.Close();
            }
        }

    }  
}
