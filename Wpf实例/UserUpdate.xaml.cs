using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// UpdateUser.xaml 的交互逻辑
    /// </summary>
    public partial class UserUpdate : Window
    {
        private Player playerWin;

        public UserUpdate()
        {
            InitializeComponent();
        }

        public UserUpdate(Player playerWin)
        {
            InitializeComponent();
            PlayerWin = playerWin;
        }

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

        private void btn_ResetAll_Click(object sender, RoutedEventArgs e)
        {
            txtUserName.Text = "";
            txtOldPwd.Password = "";
            txtNewPwd.Password = "";
            txtNewRptPwd.Password = "";
        }

        private void btn_SureUpd_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text != null && txtOldPwd.Password != null && txtNewPwd.Password != null && txtNewRptPwd.Password != null && txtUserName.Text != "" && txtOldPwd.Password != "" && txtNewPwd.Password != "" && txtNewRptPwd.Password != "")
            {
                if (txtNewPwd.Password != txtNewRptPwd.Password)
                {
                    MessageBox.Show("两次输入的密码不一致!\n\r\n\rTwo inputted password inconsistencies!");
                    txtNewRptPwd.Focus();
                }
                else if (txtOldPwd.Password == txtNewPwd.Password)
                {
                    MessageBox.Show("原密码和新密码相同!\n\r\n\rThe original code is the same as the new one!");
                    txtNewPwd.Focus();
                }
                else if (txtNewPwd.Password == txtNewRptPwd.Password)
                {
                    SqlConnection conn = new SqlConnection(UserLogin.connstr);
                    conn.Open();
                    string sql = "select UserPwd from Users where Username='" + txtUserName.Text + "' and UserPwd= '"+ txtOldPwd.Password+"'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        //使用 SqlDataReader来读取数据库 
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //如果查到用户名
                            if (sdr.Read())
                            {
                                try
                                {
                                    sdr.Close();
                                    string MyUpdate = "Update Users set UserPwd='" + txtNewPwd.Password + "'where Username = '" + txtUserName.Text + "'";
                                    SqlCommand MyCommand = new SqlCommand(MyUpdate, conn);
                                    MyCommand.ExecuteNonQuery();
                                    MessageBox.Show("密码已修改，请重新登录!!\n\r\n\rThe password has been modified. Please log in again!");
                                    UserControlClass.username = "";
                                    PlayerWin.JugUser();
                                    this.Close();
                                }
                                catch {
                                }
                            }
                            else
                            {
                                MessageBox.Show("您输入的用户名和密码有误!\n\r\n\rThe username and password you entered were incorrect!");
                                txtOldPwd.Clear();
                                txtOldPwd.Focus();
                            }
                        }
                    }
                    conn.Close();
                } 
            }else
             {
             MessageBox.Show("请把信息填写完整!\n\r\n\rPlease fill in the information!");
             }
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();

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
