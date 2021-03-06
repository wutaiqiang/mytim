﻿using System;
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
            //显示群聊的所有成员
            group_mem.AppendText("聊天成员\n");         
            for (int i = 0; i < Username.Length; i++)
            {             
                group_mem.AppendText(Username[i] + "\n");//追加文字              
            }
            //初始化套接字和人数
            Chat_allsocket = user_socket;
            form3_num = con_num;
            AsynRecive(Chat_allsocket);
        }
        //接收信息，监听所有的tcp连接
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
                                MyHide();
                                //this.Hide();
                                //System.Environment.Exit(0);
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
                                //调用函数显示出来
                                SetText(recieve_mess);
                                judge = false;
                            }

                            AsynRecive(tcpClient);//恢复监听
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //发送信息，自己显示出来（中心节点的发送）
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
               
                if (Regex.IsMatch(send, geshi))
                {
                    //显示日期                
                    chat_rec.AppendText(Username[0] + "  " + DateTime.Now.ToString() + "\n");
                    Bitmap bmp = jiwang.Properties.Resources.face__90_;
                    switch (send)
                    {
                        case "--E76--": bmp = jiwang.Properties.Resources.face__76_; break;
                        case "--E77--": bmp = jiwang.Properties.Resources.face__77_; break;
                        case "--E78--": bmp = jiwang.Properties.Resources.face__78_; break;
                        case "--E79--": bmp = jiwang.Properties.Resources.face__79_; break;
                        case "--E80--": bmp = jiwang.Properties.Resources.face__80_; break;
                        case "--E81--": bmp = jiwang.Properties.Resources.face__81_; break;
                        case "--E82--": bmp = jiwang.Properties.Resources.face__82_; break;
                        case "--E83--": bmp = jiwang.Properties.Resources.face__83_; break;
                        case "--E84--": bmp = jiwang.Properties.Resources.face__84_; break;
                        case "--E86--": bmp = jiwang.Properties.Resources.face__86_; break;
                        case "--E87--": bmp = jiwang.Properties.Resources.face__87_; break;
                        case "--E88--": bmp = jiwang.Properties.Resources.face__88_; break;
                        case "--E90--": bmp = jiwang.Properties.Resources.face__90_; break;
                        case "--E91--": bmp = jiwang.Properties.Resources.face__91_; break;
                        default: break;
                    }

                    Clipboard.SetDataObject(bmp, false);//将图片放在剪贴板中

                    if (chat_rec.CanPaste(DataFormats.GetFormat(DataFormats.Bitmap)))

                        chat_rec.Paste();//粘贴数据

                    chat_rec.AppendText("\n");
                }
                else
                {
                    chat_rec.AppendText(show + "\n");
                }

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
        
        //利用tcp连接发送信息
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
                int len = recvStr.Length;
                string a = recvStr.Substring(len - 8, 7);

                if (Regex.IsMatch(a, geshi))
                {   //显示日期                
                    chat_rec.AppendText(recvStr.Substring(0,len-8));
                    Bitmap bmp = jiwang.Properties.Resources.face__90_;
                    switch (a)
                    {
                        case "--E76--": bmp = jiwang.Properties.Resources.face__76_; break;
                        case "--E77--": bmp = jiwang.Properties.Resources.face__77_; break;
                        case "--E78--": bmp = jiwang.Properties.Resources.face__78_; break;
                        case "--E79--": bmp = jiwang.Properties.Resources.face__79_; break;
                        case "--E80--": bmp = jiwang.Properties.Resources.face__80_;break;
                        case "--E81--": bmp = jiwang.Properties.Resources.face__81_; break;
                        case "--E82--": bmp = jiwang.Properties.Resources.face__82_; break;
                        case "--E83--": bmp = jiwang.Properties.Resources.face__83_; break;
                        case "--E84--": bmp = jiwang.Properties.Resources.face__84_; break;
                        case "--E86--": bmp = jiwang.Properties.Resources.face__86_; break;
                        case "--E87--": bmp = jiwang.Properties.Resources.face__87_; break;
                        case "--E88--": bmp = jiwang.Properties.Resources.face__88_; break;
                        case "--E90--": bmp = jiwang.Properties.Resources.face__90_; break;
                        case "--E91--": bmp = jiwang.Properties.Resources.face__91_; break;
                        default:break;
                    }
                    
                    Clipboard.SetDataObject(bmp, false);//将图片放在剪贴板中

                    if (chat_rec.CanPaste(DataFormats.GetFormat(DataFormats.Bitmap)))

                        chat_rec.Paste();//粘贴数据

                    chat_rec.AppendText("\n");
                }
                else
                {
                    chat_rec.AppendText(recvStr + "\n");
                }
            }
        }
        //跨线程最小化窗口
        delegate void MyHideHander();//带参数
        private void MyHide()
        {
            if (chat_rec.InvokeRequired)//判断是否是线程在访问该控件
            {
                MyHideHander set = new MyHideHander(MyHide);//委托的方法参数应和SetText一致
                chat_rec.Invoke(set); //委托自身，递归委托，直到不是以invoke方式去访问控件
            }
            else
            {
                foreach (Socket single_tcp in Chat_allsocket)
                {
                   
                    single_tcp.Close();
                }
                this.Close();
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
                byte[] file_name = Encoding.UTF8.GetBytes("\n" + filename);//由于无法较好传输文件名，用

                byte[] data = Encoding.UTF8.GetBytes("--file--");
                //经过所有tcp连接来发送文件
                foreach (Socket item in Chat_allsocket)
                {
                    item.Send(data);//表示要发送文件,发送一个标识

                    byte[] buffer = new byte[500000];
                    int send = 0; //发送的字节数  
                    FileStream fsRead = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read);                    

                    while (true)  //大文件断点多次传输
                    {
                        int r = fsRead.Read(buffer,0, buffer.Length);
                        if (r == 0)
                        {
                            break;
                        }
                        item.Send(buffer, 0, r, SocketFlags.None);
                        send += r;                        
                    }
                    //item.SendFile(filename, null, null, TransmitFileOptions.UseDefaultWorkerThread);
                }
                foreach (Socket item in Chat_allsocket)
                {
                    //发送文件的名字
                    item.Send(file_name);
                }
                chat_rec.SelectionAlignment = HorizontalAlignment.Center;
                chat_rec.AppendText("文件正在发送\n");
            }// if (openFileDialog1.ShowDialog() == DialogResult.OK)
            else
            {
                chat_rec.SelectionAlignment = HorizontalAlignment.Center;
                chat_rec.AppendText("文件选取失败，请重新检查\n");
            }
        }
        #endregion

        #region "服务器"接收文件
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
                saveFileDialog1.Title = "保存文件";
                saveFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
                saveFileDialog1.Filter = "文本文件|*.txt|图片文件|*.jpg|视频文件|*.avi|所有文件|*.*";
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
                        /*
                        foreach (Socket item in Chat_allsocket)
                        {
                            if (item != file_listener)
                                AsynSend(item, get);
                        }
                        */
                        fs.Write(pass, total_bytes, single_bytes);
                        fs.Flush();
                        total_bytes = total_bytes + single_bytes;
                        if (single_bytes < 500000)//分片结束，接收完毕
                        {
                            break;
                        }
                    }
                    fs.Close();
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
                    /*
                    foreach (Socket item in Chat_allsocket)
                    {
                        if (item != file_listener)
                            AsynSend(item, get);
                    }
                    */
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
        private void E_86_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E86--";
        }
        private void E_76_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E76--";
        }
        private void E_80_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E80--";
        }
        private void E_82_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E82--";
        }
        private void E_87_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E87--";
        }
        private void E_88_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E88--";
        }
        private void E_91_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E91--";
        }
        private void E_77_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E77--";
        }
        private void E_84_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E84--";
        }
        private void E_83_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E83--";
        }
        private void E_81_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E81--";
        }
        private void E_79_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E79--";
        }
        private void E_78_Click(object sender, EventArgs e)
        {
            my_txt.Text = "--E78--";
        }
        #endregion


    }
}
