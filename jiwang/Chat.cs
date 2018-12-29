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
    public partial class Chat : Form
    {
        public Chat()
        {
            InitializeComponent();
        }
        Socket[] Chat_allsocket;
        int form3_num;
        string[] Username;
        string geshi = @"^--E[0-9]{2}--$";//表情解析格式
        bool judge = false;//写字框占用标识
        private ManualResetEvent interrupt = new ManualResetEvent(false);
        private delegate void client(Socket file_send_receive);//委托线程，INVOKE调用；增加线程独立性——此处传递参数为接收发送方套接字
        //初始化
        public Chat(string Users_name, Socket[] user_socket, int con_num)
        {
            InitializeComponent();
            //分割成各个联系人,显示群聊人数
            Username = Users_name.Split(',');
            group_mem.SelectionAlignment = HorizontalAlignment.Center;
            for (int i = 0; i < Username.Length; i++)
            {             
                group_mem.AppendText(Username[i] + "\n");//追加文字              
            }
            //初始化套接字和人数
            Chat_allsocket = user_socket;
            form3_num = con_num;
            AsynRecive(Chat_allsocket);
        }
        //异步客户端接收信息
        public void AsynRecive(Socket[] tcpClient)
        {
            byte[] data = new byte[1024];
            try
            {
                foreach (Socket single_tcp in tcpClient)
                {
                    single_tcp.BeginReceive(data, 0, data.Length, SocketFlags.None,
                    asyncResult =>
                    {
                        int length = 0;
                        try
                        {
                            length = single_tcp.EndReceive(asyncResult);
                            if (length == 0)
                            {   //richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
                                //    richTextBox1.AppendText("好友已退出");
                                MessageBox.Show("对话结束");
                                this.Hide();
                                return;
                            }

                            string recieve_mess = Encoding.UTF8.GetString(data, 0, length);

                            foreach (Socket receiver in tcpClient)//本机为服务器，向其他客户端发送信息
                            {
                                if (receiver != single_tcp)
                                    AsynSend(receiver, recieve_mess);
                            }

                            //判断消息格式(未写）1:文件；2：退出

                            //if (false)//recieve_mess == "--out--"
                            //{
                            //    //richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
                            //    //richTextBox1.AppendText("好友已退出");
                            //    //button2.Enabled = false;
                            //}
                            //else 
                            //MessageBox.Show(recieve_mess);
                            if (recieve_mess == "--file--")
                            {
                                interrupt.Reset();
                                client clien = new client(FileReceive);
                                Invoke(clien, new object[] { single_tcp });
                                interrupt.WaitOne();
                            }
                            else if (recieve_mess == "--shake--")
                            {
                                Console.WriteLine(recieve_mess);
                                Window_Shake win = new Window_Shake(Window_shake);
                                Invoke(win, new object[] { });
                            }
                            else
                            {
                                //更新接受信息
                                //写字框占用
                                while (judge) { };
                                judge = true;
                               
                                SetText(recieve_mess);
                                judge = false;
                            }

                            AsynRecive(tcpClient);//恢复监听
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }, null);
                }//foreach (Socket single_tcp in tcpClient)
            }//try
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //发送信息，自己显示出来
        private void Send_mytxt_Click(object sender, EventArgs e)
        {
            try
            {
                string send = my_txt.Text;
                if (send == "") {
                    MessageBox.Show("输入不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                while (judge) {};

                //更新自己电脑信息;
                judge = true;
                //Username[0]为自己的名字
                string show = Username[0] + "  " + DateTime.Now.ToString() + "\n" + send + "\n";
                chat_rec.SelectionAlignment = HorizontalAlignment.Left;
                chat_rec.AppendText(show);
                judge = false;
                my_txt.Text = null;
                //遍历，向其他人发送
                foreach (Socket item in Chat_allsocket)
                {
                    AsynSend(item, show);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("连接已失效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
        }
        
        //线程异步发送信息
        public void AsynSend(Socket tcpClient, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpClient.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
            {
                try
                {
                    //完成发送消息
                    int length = tcpClient.EndSend(asyncResult);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "发送错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, null);

        }
        //解决跨线程设置richtxt
        delegate void SetTextHander(string recvStr);//带参数
        private void SetText(string recvStr)
        {
            if (chat_rec.InvokeRequired)//判断是否是线程在访问该控件
            {
                SetTextHander set = new SetTextHander(SetText);//委托的方法参数应和SetText一致
                chat_rec.Invoke(set, recvStr); //委托自身，递归委托，直到不是以invoke方式去访问控件
            }
            else
            {
                chat_rec.SelectionAlignment = HorizontalAlignment.Right;
                //richTextBox1.SelectionColor = Color.Red;
                //richTextBox1.AppendText(Thread.CurrentThread.ManagedThreadId + "回调结束------------" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "------------------------------------------\r\n");
                //richTextBox1.SelectionColor = Color.Blue;
                bool a = Regex.IsMatch("--E90--", geshi);
                bool a1 = Regex.IsMatch("\n--E90--\n", geshi);
                if (Regex.IsMatch(recvStr.Trim('\n'), geshi))
                {                   
                    chat_rec.AppendText(recvStr.Substring(3,2));
                }
                else
                {
                    chat_rec.AppendText(recvStr + "\n");
                }
            }
        }
        #region 退出
        //关闭连接
        public void My_Leave()
        {
            foreach (Socket item in Chat_allsocket)
            {
                if (!item.Connected) continue;
                item.Shutdown(SocketShutdown.Both);
                item.Close();
            }
            this.Hide();
        }
        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            My_Leave();
        }
        #endregion

        #region 发送文件
        private void Fil_send_Click(object sender, EventArgs e)
        {
            //选择文件
            //成功打开文件
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                byte[] file_name = Encoding.UTF8.GetBytes("\n" + filename);//由于无法较好传输文件名，用词

                byte[] data = Encoding.UTF8.GetBytes("--file--");
                //向所有对象发送
                foreach (Socket item in Chat_allsocket)
                {
                    item.Send(data);//表示要发送文件
                    item.SendFile(filename, null, null, TransmitFileOptions.UseDefaultWorkerThread);
                }
                foreach (Socket item in Chat_allsocket)
                {
                    item.Send(file_name);
                }
                chat_rec.SelectionAlignment = HorizontalAlignment.Center;
                chat_rec.AppendText("文件成功发送\n");
            }// if (openFileDialog1.ShowDialog() == DialogResult.OK)
            else
            {
                chat_rec.SelectionAlignment = HorizontalAlignment.Center;
                chat_rec.AppendText("文件选取失败，请重新检查\n");
            }
        }
        #endregion

        #region 接收文件
        public void FileReceive(Socket file_listener)
        {
            DialogResult dr = MessageBox.Show("有文件请求，是否同意？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            byte[] pass = new byte[500000];
            int single_bytes;
            int total_bytes;
            int min = 0;//同意接受且路径有效的情况下为1，其余情况为0（代表操作无效）
            if (dr == DialogResult.OK)//同意接受
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    min = 1;
                    string filepath = saveFileDialog1.FileName;//文件保存路径
                    FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                    total_bytes = 0;
                    single_bytes = 0;
                    while (true)
                    {
                        single_bytes = file_listener.Receive(pass, 500000, SocketFlags.None);
                        string get = Encoding.UTF8.GetString(pass, 0, single_bytes);
                        foreach (Socket item in Chat_allsocket)
                        {
                            if (item != file_listener)
                                AsynSend(item, get);
                        }
                        fs.Write(pass, total_bytes, single_bytes);
                        fs.Flush();
                        total_bytes = total_bytes + single_bytes;
                        if (single_bytes < 500000)//分片结束，接收完毕
                        {
                            break;
                        }
                    }
                }//if (saveFileDialog.ShowDialog() == DialogResult.OK)打开文件成功
                chat_rec.SelectionAlignment = HorizontalAlignment.Center;
                chat_rec.AppendText("文件成功接收\n");
            }//同意接受

            if (min == 0)//用户不同意接受或者没有选择合法路径，因此不储存信息，直接接受、转发即可
            {
                total_bytes = 0;
                single_bytes = 0;
                while (true)
                {
                    single_bytes = file_listener.Receive(pass, 500000, SocketFlags.None);
                    string get = Encoding.UTF8.GetString(pass, 0, single_bytes);
                    foreach (Socket item in Chat_allsocket)
                    {
                        if (item != file_listener)
                            AsynSend(item, get);
                    }
                    total_bytes = total_bytes + single_bytes;
                    if (single_bytes < 500000)//分片结束，接收完毕
                    {
                        break;
                    }
                }
            }//if (min == 0)//用户不同意接受
            interrupt.Set();
        }
        //public void FileReceive(Socket file_listener)
        #endregion

        #region 窗口抖动            
        private void Win_shake_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes("--shake--");
            foreach (Socket item in Chat_allsocket)
            {
                item.Send(data);
            }
        }
        //跨线程抖动
        public delegate void Window_Shake();
        public void Window_shake()
        {          
            timer1.Start();
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {           
            this.Location = new Point(this.Location.X + 10, this.Location.Y + 10);
            Thread.Sleep(50);
            this.Location = new Point(this.Location.X - 10, this.Location.Y - 10);
            Thread.Sleep(50);
            this.Location = new Point(this.Location.X + 10, this.Location.Y + 10);
            Thread.Sleep(50);
            this.Location = new Point(this.Location.X - 10, this.Location.Y - 10);
            Thread.Sleep(50);
            this.Location = new Point(this.Location.X + 5, this.Location.Y - 5);
            timer1.Stop();
        }
        #endregion
        #region 表情
        private void E_90_Click(object sender, EventArgs e)
        {
            my_txt.Text="--E90--";
        }
        #endregion
    }
}
