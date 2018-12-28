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
using System.IO;//命名空间
using System.Text.RegularExpressions;//正则匹配

namespace jiwang
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        public string User_name;
        public string List_dir;
        Socket Socket_user;//本机套接字
        IPAddress User_ip;

        string geshi = @"^20[0-9]{8}$";
        //初始化
        public Main(string username, Socket client)
        {
            InitializeComponent();
            User_name = username;
            Socket_user = client;
            List_dir = "Data/"+ username+ ".txt";

            //得到本机IP地址
            IPAddress[] ip_list = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipAddress in ip_list)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    User_ip = ipAddress;
                }
            }
            Userinfo.Text = "欢迎！@"+User_name;
            
            //设置listview的显示样式,标题(隐藏第一列解决居中问题)
            this.listView1.View = View.Details;
            ColumnHeader Ch0 = new ColumnHeader();    
            Ch0.Width = 0;       
            ColumnHeader Ch1 = new ColumnHeader();
            Ch1.Text = "    好友名称    ";
            Ch1.Width = -2;
            Ch1.TextAlign = HorizontalAlignment.Center;
            ColumnHeader Ch2 = new ColumnHeader();
            Ch2.Text = "    IP地址    ";
            Ch2.Width = -2;
            Ch2.TextAlign = HorizontalAlignment.Center;
            ColumnHeader Ch3 = new ColumnHeader();
            Ch3.Text = "    是否在线    ";
            Ch3.Width = -2;
            Ch3.TextAlign = HorizontalAlignment.Center;
            this.listView1.Columns.AddRange(new ColumnHeader[] { Ch0,Ch1, Ch2, Ch3 });

            Updata_list();
        }

        /**根据文件生成list**/
        private void Updata_list()
        {
            this.listView1.Items.Clear();

            if (!File.Exists(List_dir))
            {
                //如果没有就新建文件
                FileStream fs1 = new FileStream(List_dir, FileMode.Create);
                fs1.Close();
            }
            StreamReader f = new StreamReader(List_dir, Encoding.Default);
            String line;
            this.listView1.BeginUpdate();
            while ((line = f.ReadLine()) != null)
            {
                
                if (line =="") break;

                string[] info = new string[4];
                info[0] = "";
                info[1] = line;
                string flag;
                flag = IsOnline(line);
                if (flag == "n"||flag=="")
                {
                    info[2] = "Unknown";
                    info[3] = "OffLine";
                }
                else if (flag=="Incorrect No."){
                    info[2] = flag;
                    info[3] = "OffLine";
                }
                else{
                    info[2] = flag;
                    info[3] = "Online";
                }
                //初始化每一行的内容与颜色
                ListViewItem mid_list = new ListViewItem(info);
                if (info[3] == "Online")
                {
                    mid_list.BackColor = Color.White;               
                }
                else { mid_list.BackColor = Color.LightGray; }

                this.listView1.Items.Add(mid_list);
            }
            this.listView1.EndUpdate();
            f.Close();

            fri_id.Text = "";
        }
 
        public string IsOnline(string IDnumber)
        {
            string IP_target = "q" + IDnumber;//向服务器发送的查询信息
            byte[] IP_echo = new byte[1024];
            IP_echo = Encoding.ASCII.GetBytes(IP_target);
            try
            {
                Socket_user.Send(IP_echo);
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接服务器!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ("");
            }
            //接收用户信息,从中截取部分
            byte[] IP_receive_byte = new byte[1024];
            int number;
            try
            {
                number = Socket_user.Receive(IP_receive_byte);
            }
            catch (ArgumentOutOfRangeException)
            {
                return "fuck";
            }         
            string IP_receive_mess = Encoding.Default.GetString(IP_receive_byte, 0, number);
            return IP_receive_mess;
                       
        }

        /****注销****/
        private void Logout()
        {
            string outmess = "logout" + User_name;
            //开始建立连接
            byte[] logout_byte = new byte[1024];
            logout_byte = Encoding.ASCII.GetBytes(outmess);
            try
            {
                Socket_user.Send(logout_byte, logout_byte.Length, 0);
            }
            catch (Exception)
            {
                MessageBox.Show("网络错误！", "注销错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            //接收注销信息,从1024字节中截取一部分
            byte[] receive_byte = new byte[1024];
            int number = Socket_user.Receive(receive_byte, receive_byte.Length, 0);
            //MessageBox.Show(number.ToString());

            string receive_mess = Encoding.Default.GetString(receive_byte, 0, number);
            if (receive_mess == "loo")
            {
                MessageBox.Show("下线成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("下线失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
        }
        //按退出键注销
        private void Btnlogout_Click(object sender, EventArgs e)
        {
            Logout();
            this.Hide();
            Login tmp = new Login();
            tmp.Show();
        }
        //关闭窗口注销
        private void Main_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Logout();
            this.Hide();
            Login tmp = new Login();
            tmp.Show();
        }
        //添加好友
        private void Fri_add_Click(object sender, EventArgs e)
        {
            string fri_name;
            fri_name = fri_id.Text.ToString();
            if (fri_name == "")
            {
                MessageBox.Show("输入的学号为空！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else if (!(Regex.IsMatch(fri_name, geshi)))
            {
                MessageBox.Show("学号不符合规范！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            {
                if (!File.Exists(List_dir))
                {
                    //如果没有就新建文件
                    FileStream f1 = new FileStream(List_dir, FileMode.Create);
                    f1.Close();
                }
                //读取每一行
                StreamReader fr = new StreamReader(List_dir, Encoding.Default);
                String line;
                bool Exi = false;              
                while ((line = fr.ReadLine()) != null)
                {
                    if (line == fri_name)
                    {
                        Exi = true;
                    }
                }
                fr.Close();
                if (!Exi)
                {
                    StreamWriter f = new StreamWriter(List_dir, true);
                    
                    f.WriteLine(fri_name);
                    f.Flush();
                    f.Close();
                }              
                //更新
                Updata_list();
            }
        }
        //删除选中好友
        private void Fri_del_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的好友!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                DialogResult dr=MessageBox.Show("真的要删除这些好友？", "警告", MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
                if (dr != DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    List<string> type = new List<string>();
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        type.Add(listView1.SelectedItems[i].SubItems[1].Text);
                    }
                    List<string> lns = new List<string>();
                    using (StreamReader str = new StreamReader(List_dir))
                    {
                        string ln;
                        while ((ln = str.ReadLine()) != null)
                        {
                            if (!type.Contains(ln))
                            {
                                lns.Add(ln);
                            }
                        }
                        str.Close();
                    }
                    //lns=all-type;将lns写回List_dir,覆盖性写回
                    StreamWriter f = new StreamWriter(List_dir);
                    for (int i = 0; i < lns.Count; i++)
                    {
                        string a = lns[i];
                        f.WriteLine(a);
                    }
                    f.Flush();
                    f.Close();

                    Updata_list();
                }
            }
        }

        //广告位招租
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.huanqiu.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://lanyue.tanwan.com/");
        }
        //跳转到聊天界面
        private void Chat_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要聊天的好友!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                //MessageBox.Show("乐享沟通！", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
            }
        }
    }
}
