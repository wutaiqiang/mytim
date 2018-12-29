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
            //监听他人发来的信息
            Listen_s();
        }

        #region 更新表单
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
                    info[3] = "Offline";
                }
                else if (flag=="Incorrect No."){
                    info[2] = flag;
                    info[3] = "Offline";
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
        #endregion

        #region logout
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
            //Login tmp = new Login();
            //tmp.Show();
    
        }
        //关闭窗口注销
        private void Main_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Logout();
            this.Hide();
            //Login tmp = new Login();
            //tmp.Show();
        }
        #endregion

        #region 好友增删
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
        #endregion

        #region 广告位招租
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.huanqiu.com");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://lanyue.tanwan.com/");
        }
        #endregion

        #region 监听
       
        public void Listen_s()
        {
            int listenport = 50000;
            Socket tcp_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverIP = new IPEndPoint(User_ip, listenport);
            tcp_server.Bind(serverIP);//阻塞模式
            tcp_server.Listen(20);//最多处理20个排队
            Console.WriteLine("异步开启监听...");
            AsynAccept(tcp_server);//接受连接套接字
        }
        //接受连接,继续监听
        public void AsynAccept(Socket tcpServer)
        {
            tcpServer.BeginAccept(asyncResult =>
            {
                Socket tcpClient = tcpServer.EndAccept(asyncResult);
                Console.WriteLine("server<--<--{0}", tcpClient.RemoteEndPoint.ToString());
                //AsynSend(tcpClient, "收到连接...");//发送消息
                AsynAccept(tcpServer);//继续监听其余连接
                AsynRecive(tcpClient);//监听信息
            }, null);
        }
        //启动线程，界面跳转，接受信息
        public void AsynRecive(Socket tcpClient)
        {
            byte[] data = new byte[1024];
            try
            {
                tcpClient.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    int length = tcpClient.EndReceive(asyncResult);                    
                    string recieve_mess = Encoding.UTF8.GetString(data, 0, data.Length);
                    Console.WriteLine("server<--<--client:{0}",recieve_mess);
                    Socket[] connected_socket = new Socket[1];//配合群聊使用，本来是单个套接字
                    connected_socket[0] = tcpClient;//发起对话的对象
                    //Thread sever_client = new Thread()
                    //开启对话窗口，开始对话，传递参数
                    //button3.Visible = true;
                    string pass_mess;
                    pass_mess = User_name + "," + recieve_mess;
                    //与一个用户单聊
                    Thread server_client_connection = new Thread(() => Application.Run(new Chat(pass_mess, connected_socket, 1)));
                    server_client_connection.SetApartmentState(System.Threading.ApartmentState.STA);//单线程监听控制
                    server_client_connection.Start();
                }, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //查找对应的IP，发送广播信息
        public Socket Chat_group(string userid, string broad)
        {
            string ip = IsOnline(userid);
            IPEndPoint user_ip = new IPEndPoint(IPAddress.Parse(ip), 50000);
            Socket user_tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                user_tcp.Connect(user_ip);
            }
            catch (SocketException)
            {
                MessageBox.Show("对方没有响应！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return user_tcp;
            }
            byte[] data = Encoding.UTF8.GetBytes(broad);
            user_tcp.Send(data);
            return user_tcp;
        }
        #endregion
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
                //选中人是否在线
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    if (item.SubItems[3].Text == "Offline")
                    {
                        MessageBox.Show("部分选中的好友不在线", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                int connect_num = 0;
                string broad_mess;//广播信息(学号)，包括本机地址;不同的对象信息不同
                string headpart = User_name;//将自己的信息放入
                Socket[] clients = new Socket[listView1.SelectedItems.Count];//建立套接字对象
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    //所有人的学号，对每一个人广播群聊里其他人的信息
                    broad_mess = User_name;
                    foreach (ListViewItem item1 in listView1.SelectedItems)
                    {
                        if (item1.SubItems[1].Text != item.SubItems[1].Text)
                            broad_mess = broad_mess + "," + item1.SubItems[1].Text;
                    }
                    //所有参与对话者，用来建立线程
                    headpart = headpart + "," + item.SubItems[1].Text;
                    //发送board信息
                    clients[connect_num] = Chat_group(item.SubItems[1].Text, broad_mess);//群聊
                    connect_num++;
                }
                
                //开启对话框，多线程，开始对话
                Thread server_client_connection = new Thread(() => Application.Run(new Chat(headpart, clients, connect_num)));
                server_client_connection.SetApartmentState(System.Threading.ApartmentState.STA);//单线程监听控制
                server_client_connection.Start();
            }
        }
    }
}
