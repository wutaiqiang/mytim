namespace jiwang
{
    partial class Login
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.acco_text = new System.Windows.Forms.TextBox();
            this.pass_text = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.see_pass = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(915, 628);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 82);
            this.button1.TabIndex = 0;
            this.button1.Text = "登录";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // acco_text
            // 
            this.acco_text.Location = new System.Drawing.Point(508, 350);
            this.acco_text.Name = "acco_text";
            this.acco_text.Size = new System.Drawing.Size(238, 35);
            this.acco_text.TabIndex = 1;
            this.acco_text.Text = "2016011419";
            // 
            // pass_text
            // 
            this.pass_text.Location = new System.Drawing.Point(508, 452);
            this.pass_text.Name = "pass_text";
            this.pass_text.PasswordChar = '*';
            this.pass_text.Size = new System.Drawing.Size(238, 35);
            this.pass_text.TabIndex = 2;
            this.pass_text.Text = "net2018";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(370, 353);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "账号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 455);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "密码：";
            // 
            // see_pass
            // 
            this.see_pass.Location = new System.Drawing.Point(797, 441);
            this.see_pass.Name = "see_pass";
            this.see_pass.Size = new System.Drawing.Size(141, 53);
            this.see_pass.TabIndex = 5;
            this.see_pass.Text = "查看密码";
            this.see_pass.UseVisualStyleBackColor = true;
            this.see_pass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.See_pass_MouseDown);
            this.see_pass.MouseUp += new System.Windows.Forms.MouseEventHandler(this.See_pass_MouseUp);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 764);
            this.Controls.Add(this.see_pass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pass_text);
            this.Controls.Add(this.acco_text);
            this.Controls.Add(this.button1);
            this.Name = "Login";
            this.Text = "登陆";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox acco_text;
        private System.Windows.Forms.TextBox pass_text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button see_pass;
    }
}

