namespace GPMI_Laser_Marking
{
    partial class SetupIP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupIP));
            this.LSIP = new System.Windows.Forms.TextBox();
            this.IPtxt = new System.Windows.Forms.Label();
            this.LSPort = new System.Windows.Forms.TextBox();
            this.Porttxt = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.BIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.savebtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.BarcodeIP_Beforetxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BarcodePort_Befor_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.PLC_Combobox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // LSIP
            // 
            this.LSIP.Location = new System.Drawing.Point(50, 24);
            this.LSIP.Margin = new System.Windows.Forms.Padding(4);
            this.LSIP.Name = "LSIP";
            this.LSIP.Size = new System.Drawing.Size(125, 24);
            this.LSIP.TabIndex = 0;
            this.LSIP.Text = "192.168.0.72";
            // 
            // IPtxt
            // 
            this.IPtxt.AutoSize = true;
            this.IPtxt.Location = new System.Drawing.Point(4, 27);
            this.IPtxt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.IPtxt.Name = "IPtxt";
            this.IPtxt.Size = new System.Drawing.Size(21, 18);
            this.IPtxt.TabIndex = 1;
            this.IPtxt.Text = "IP";
            // 
            // LSPort
            // 
            this.LSPort.Location = new System.Drawing.Point(50, 57);
            this.LSPort.Margin = new System.Windows.Forms.Padding(4);
            this.LSPort.Name = "LSPort";
            this.LSPort.Size = new System.Drawing.Size(125, 24);
            this.LSPort.TabIndex = 0;
            this.LSPort.Text = "2500";
            // 
            // Porttxt
            // 
            this.Porttxt.AutoSize = true;
            this.Porttxt.Location = new System.Drawing.Point(4, 61);
            this.Porttxt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Porttxt.Name = "Porttxt";
            this.Porttxt.Size = new System.Drawing.Size(36, 18);
            this.Porttxt.TabIndex = 1;
            this.Porttxt.Text = "Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.LSIP);
            this.groupBox1.Controls.Add(this.Porttxt);
            this.groupBox1.Controls.Add(this.LSPort);
            this.groupBox1.Controls.Add(this.IPtxt);
            this.groupBox1.Location = new System.Drawing.Point(12, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 102);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Laser F20E";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(182, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(58, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox2);
            this.groupBox2.Controls.Add(this.BIP);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.PPort);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(313, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 102);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Barcode Check After Laser Marking";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.InitialImage")));
            this.pictureBox2.Location = new System.Drawing.Point(184, 24);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(58, 58);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // BIP
            // 
            this.BIP.Location = new System.Drawing.Point(52, 25);
            this.BIP.Margin = new System.Windows.Forms.Padding(4);
            this.BIP.Name = "BIP";
            this.BIP.Size = new System.Drawing.Size(125, 24);
            this.BIP.TabIndex = 0;
            this.BIP.Text = "192.168.0.72";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            // 
            // PPort
            // 
            this.PPort.Location = new System.Drawing.Point(52, 58);
            this.PPort.Margin = new System.Windows.Forms.Padding(4);
            this.PPort.Name = "PPort";
            this.PPort.Size = new System.Drawing.Size(125, 24);
            this.PPort.TabIndex = 0;
            this.PPort.Text = "27110";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP";
            // 
            // savebtn
            // 
            this.savebtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.savebtn.Location = new System.Drawing.Point(225, 250);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(163, 64);
            this.savebtn.TabIndex = 3;
            this.savebtn.Text = "Save";
            this.savebtn.UseVisualStyleBackColor = true;
            this.savebtn.Click += new System.EventHandler(this.Savebtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBox3);
            this.groupBox3.Controls.Add(this.BarcodeIP_Beforetxt);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.BarcodePort_Befor_txt);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(8, 133);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(289, 102);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Barcode Check Before Laser Marking";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.InitialImage")));
            this.pictureBox3.Location = new System.Drawing.Point(184, 24);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(58, 58);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // BarcodeIP_Beforetxt
            // 
            this.BarcodeIP_Beforetxt.Location = new System.Drawing.Point(52, 25);
            this.BarcodeIP_Beforetxt.Margin = new System.Windows.Forms.Padding(4);
            this.BarcodeIP_Beforetxt.Name = "BarcodeIP_Beforetxt";
            this.BarcodeIP_Beforetxt.Size = new System.Drawing.Size(125, 24);
            this.BarcodeIP_Beforetxt.TabIndex = 0;
            this.BarcodeIP_Beforetxt.Text = "192.168.0.72";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 62);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Port";
            // 
            // BarcodePort_Befor_txt
            // 
            this.BarcodePort_Befor_txt.Location = new System.Drawing.Point(52, 58);
            this.BarcodePort_Befor_txt.Margin = new System.Windows.Forms.Padding(4);
            this.BarcodePort_Befor_txt.Name = "BarcodePort_Befor_txt";
            this.BarcodePort_Befor_txt.Size = new System.Drawing.Size(125, 24);
            this.BarcodePort_Befor_txt.TabIndex = 0;
            this.BarcodePort_Befor_txt.Text = "27110";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 28);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "IP";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.PLC_Combobox);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(313, 133);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(280, 102);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PLC Connect";
            // 
            // PLC_Combobox
            // 
            this.PLC_Combobox.FormattingEnabled = true;
            this.PLC_Combobox.Location = new System.Drawing.Point(92, 42);
            this.PLC_Combobox.Name = "PLC_Combobox";
            this.PLC_Combobox.Size = new System.Drawing.Size(125, 26);
            this.PLC_Combobox.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 45);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 18);
            this.label6.TabIndex = 1;
            this.label6.Text = "COM";
            // 
            // SetupIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(605, 334);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.savebtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SetupIP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup ";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupIP_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox LSIP;
        private System.Windows.Forms.Label IPtxt;
        private System.Windows.Forms.TextBox LSPort;
        private System.Windows.Forms.Label Porttxt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox BIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox BarcodeIP_Beforetxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BarcodePort_Befor_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox PLC_Combobox;
        private System.Windows.Forms.Label label6;
    }
}