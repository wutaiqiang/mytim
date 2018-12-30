namespace jiwang
{
    partial class Main
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
            this.Userinfo = new System.Windows.Forms.Label();
            this.Btnlogout = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.fri_id = new System.Windows.Forms.TextBox();
            this.fri_add = new System.Windows.Forms.Button();
            this.fri_del = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.chat = new System.Windows.Forms.Button();
            this.fri_check = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Userinfo
            // 
            this.Userinfo.AutoSize = true;
            this.Userinfo.Font = new System.Drawing.Font("宋体", 20F);
            this.Userinfo.Location = new System.Drawing.Point(36, 28);
            this.Userinfo.Name = "Userinfo";
            this.Userinfo.Size = new System.Drawing.Size(185, 54);
            this.Userinfo.TabIndex = 1;
            this.Userinfo.Text = "欢迎！";
            this.Userinfo.UseWaitCursor = true;
            // 
            // Btnlogout
            // 
            this.Btnlogout.Location = new System.Drawing.Point(936, 387);
            this.Btnlogout.Name = "Btnlogout";
            this.Btnlogout.Size = new System.Drawing.Size(196, 70);
            this.Btnlogout.TabIndex = 2;
            this.Btnlogout.Text = "注销";
            this.Btnlogout.UseVisualStyleBackColor = true;
            this.Btnlogout.UseWaitCursor = true;
            this.Btnlogout.Click += new System.EventHandler(this.Btnlogout_Click);
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(35, 98);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(711, 544);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.UseWaitCursor = true;
            // 
            // fri_id
            // 
            this.fri_id.Location = new System.Drawing.Point(936, 83);
            this.fri_id.Name = "fri_id";
            this.fri_id.Size = new System.Drawing.Size(196, 35);
            this.fri_id.TabIndex = 4;
            this.fri_id.UseWaitCursor = true;
            // 
            // fri_add
            // 
            this.fri_add.Location = new System.Drawing.Point(936, 149);
            this.fri_add.Name = "fri_add";
            this.fri_add.Size = new System.Drawing.Size(196, 75);
            this.fri_add.TabIndex = 5;
            this.fri_add.Text = "添加好友";
            this.fri_add.UseVisualStyleBackColor = true;
            this.fri_add.UseWaitCursor = true;
            this.fri_add.Click += new System.EventHandler(this.Fri_add_Click);
            // 
            // fri_del
            // 
            this.fri_del.Location = new System.Drawing.Point(936, 272);
            this.fri_del.Name = "fri_del";
            this.fri_del.Size = new System.Drawing.Size(196, 69);
            this.fri_del.TabIndex = 6;
            this.fri_del.Text = "删除选中";
            this.fri_del.UseVisualStyleBackColor = true;
            this.fri_del.UseWaitCursor = true;
            this.fri_del.Click += new System.EventHandler(this.Fri_del_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(41, 688);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(106, 24);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "官方网站";
            this.linkLabel1.UseWaitCursor = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(640, 688);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(106, 24);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "用户论坛";
            this.linkLabel2.UseWaitCursor = true;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2_LinkClicked);
            // 
            // chat
            // 
            this.chat.Location = new System.Drawing.Point(936, 631);
            this.chat.Name = "chat";
            this.chat.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chat.Size = new System.Drawing.Size(196, 77);
            this.chat.TabIndex = 9;
            this.chat.Text = "开始聊天";
            this.chat.UseVisualStyleBackColor = true;
            this.chat.UseWaitCursor = true;
            this.chat.Click += new System.EventHandler(this.Chat_Click);
            // 
            // fri_check
            // 
            this.fri_check.Location = new System.Drawing.Point(936, 506);
            this.fri_check.Name = "fri_check";
            this.fri_check.Size = new System.Drawing.Size(196, 78);
            this.fri_check.TabIndex = 10;
            this.fri_check.Text = "好友状态";
            this.fri_check.UseVisualStyleBackColor = true;
            this.fri_check.UseWaitCursor = true;
            this.fri_check.Click += new System.EventHandler(this.Fri_check_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::jiwang.Properties.Resources._2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1323, 752);
            this.Controls.Add(this.fri_check);
            this.Controls.Add(this.chat);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.fri_del);
            this.Controls.Add(this.fri_add);
            this.Controls.Add(this.fri_id);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.Btnlogout);
            this.Controls.Add(this.Userinfo);
            this.Name = "Main";
            this.Text = "Main";
            this.UseWaitCursor = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Userinfo;
        private System.Windows.Forms.Button Btnlogout;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox fri_id;
        private System.Windows.Forms.Button fri_add;
        private System.Windows.Forms.Button fri_del;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Button chat;
        private System.Windows.Forms.Button fri_check;
    }
}