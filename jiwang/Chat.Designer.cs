namespace jiwang
{
    partial class Chat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.chat_rec = new System.Windows.Forms.RichTextBox();
            this.fil_send = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.win_shake = new System.Windows.Forms.Button();
            this.my_txt = new System.Windows.Forms.RichTextBox();
            this.send_mytxt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.group_mem = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // chat_rec
            // 
            this.chat_rec.Location = new System.Drawing.Point(12, 12);
            this.chat_rec.Name = "chat_rec";
            this.chat_rec.Size = new System.Drawing.Size(884, 525);
            this.chat_rec.TabIndex = 0;
            this.chat_rec.Text = "";
            // 
            // fil_send
            // 
            this.fil_send.Location = new System.Drawing.Point(738, 538);
            this.fil_send.Name = "fil_send";
            this.fil_send.Size = new System.Drawing.Size(158, 42);
            this.fil_send.TabIndex = 1;
            this.fil_send.Text = "发送文件";
            this.fil_send.UseVisualStyleBackColor = true;
            this.fil_send.Click += new System.EventHandler(this.Fil_send_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // win_shake
            // 
            this.win_shake.Location = new System.Drawing.Point(12, 541);
            this.win_shake.Name = "win_shake";
            this.win_shake.Size = new System.Drawing.Size(168, 37);
            this.win_shake.TabIndex = 2;
            this.win_shake.Text = "窗口抖动";
            this.win_shake.UseVisualStyleBackColor = true;
            this.win_shake.Click += new System.EventHandler(this.Win_shake_Click);
            // 
            // my_txt
            // 
            this.my_txt.Location = new System.Drawing.Point(12, 580);
            this.my_txt.Name = "my_txt";
            this.my_txt.Size = new System.Drawing.Size(884, 194);
            this.my_txt.TabIndex = 3;
            this.my_txt.Text = "";
            // 
            // send_mytxt
            // 
            this.send_mytxt.Location = new System.Drawing.Point(769, 705);
            this.send_mytxt.Name = "send_mytxt";
            this.send_mytxt.Size = new System.Drawing.Size(127, 69);
            this.send_mytxt.TabIndex = 4;
            this.send_mytxt.Text = "发送";
            this.send_mytxt.UseVisualStyleBackColor = true;
            this.send_mytxt.Click += new System.EventHandler(this.Send_mytxt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(978, 354);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "聊天成员";
            // 
            // group_mem
            // 
            this.group_mem.Location = new System.Drawing.Point(941, 411);
            this.group_mem.Name = "group_mem";
            this.group_mem.Size = new System.Drawing.Size(191, 259);
            this.group_mem.TabIndex = 6;
            this.group_mem.Text = "";
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 835);
            this.Controls.Add(this.group_mem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.send_mytxt);
            this.Controls.Add(this.my_txt);
            this.Controls.Add(this.win_shake);
            this.Controls.Add(this.fil_send);
            this.Controls.Add(this.chat_rec);
            this.Name = "Chat";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox chat_rec;
        private System.Windows.Forms.Button fil_send;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button win_shake;
        private System.Windows.Forms.RichTextBox my_txt;
        private System.Windows.Forms.Button send_mytxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox group_mem;
    }
}