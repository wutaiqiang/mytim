using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;//正则匹配

namespace jiwang
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        IPAddress Server = IPAddress.Parse("166.111.140.14");
        int port = 8000;
        string Username;
        string Password;
        Socket client;
        string geshi = @"^20[0-9]{8}$";
        //个人初始定义:读取文件,填写用户名
        void Initial_self()
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //初始化端口号和地址
            IPEndPoint ip_port = new IPEndPoint(Server, port);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            //同步套接字连接，确认网络
            try
            {
                client.Connect(ip_port);
            }
            catch (SocketException)
            {
                MessageBox.Show("请检查您的网络连接！", "网络错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            Username = acco_text.Text.ToString();
            Password = pass_text.Text.ToString();

            //规范账号密码和密码
            if (Username == "")
            {
                MessageBox.Show("请输入账号！", "登陆错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else if (!(Regex.IsMatch(Username, geshi)))
            {
                MessageBox.Show("请输入符合规范的账号！", "登陆错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else if (Password == "")
            {
                MessageBox.Show("请输入密码!", "登陆错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            //连接
            string login_mess = Username + "_" + Password;
            byte[] login_byte = new byte[1024];
            login_byte = Encoding.ASCII.GetBytes(login_mess);

            try
            {
                client.Send(login_byte, login_byte.Length, 0);
            }
            catch (Exception)
            {
                MessageBox.Show("连接超时，请重新连接", "连接错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            //登录反馈
            byte[] receive_byte = new byte[1024];
            int number = client.Receive(receive_byte, receive_byte.Length, 0);

            string receive_mess = Encoding.Default.GetString(receive_byte, 0, number);
            if (receive_mess == "lol")
            {
                MessageBox.Show("欢迎使用！");
            }
            else
            {
                MessageBox.Show("账号或密码错误!", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            //登陆成功，切换到主窗口，关闭本窗口
            Main main = new Main(Username, client);
            main.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void See_pass_MouseDown(object sender, MouseEventArgs e)
        {
            pass_text.PasswordChar = '\0';
        }

        private void See_pass_MouseUp(object sender, MouseEventArgs e)
        {
            pass_text.PasswordChar = '*';
        }
    }
}
