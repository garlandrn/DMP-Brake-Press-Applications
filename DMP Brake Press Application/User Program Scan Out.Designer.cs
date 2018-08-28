namespace DMP_Brake_Press_Application
{
    partial class User_Program_Scan_Out
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(User_Program_Scan_Out));
            this.ScanOutBrowser = new System.Windows.Forms.WebBrowser();
            this.Complete_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.EmployeeNumber_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.JobNumber_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TotalCountQtuQtyComp_TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ComputerName_TextBox = new System.Windows.Forms.TextBox();
            this.Submit_Button = new System.Windows.Forms.Button();
            this.Close_Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ScanOutBrowser
            // 
            this.ScanOutBrowser.Dock = System.Windows.Forms.DockStyle.Top;
            this.ScanOutBrowser.Location = new System.Drawing.Point(0, 0);
            this.ScanOutBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.ScanOutBrowser.Name = "ScanOutBrowser";
            this.ScanOutBrowser.Size = new System.Drawing.Size(764, 822);
            this.ScanOutBrowser.TabIndex = 0;
            this.ScanOutBrowser.Url = new System.Uri("http://ohsenslu803/fsdatacollection/Login.asp", System.UriKind.Absolute);
            this.ScanOutBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.ScanOutBrowser_DocumentCompleted);
            // 
            // Complete_Button
            // 
            this.Complete_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 38.25F);
            this.Complete_Button.Location = new System.Drawing.Point(167, 828);
            this.Complete_Button.Name = "Complete_Button";
            this.Complete_Button.Size = new System.Drawing.Size(585, 72);
            this.Complete_Button.TabIndex = 1;
            this.Complete_Button.Text = "Complete";
            this.Complete_Button.UseVisualStyleBackColor = true;
            this.Complete_Button.Click += new System.EventHandler(this.Complete_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.25F);
            this.Cancel_Button.Location = new System.Drawing.Point(12, 828);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(149, 72);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // EmployeeNumber_TextBox
            // 
            this.EmployeeNumber_TextBox.Location = new System.Drawing.Point(68, 909);
            this.EmployeeNumber_TextBox.Name = "EmployeeNumber_TextBox";
            this.EmployeeNumber_TextBox.ReadOnly = true;
            this.EmployeeNumber_TextBox.Size = new System.Drawing.Size(123, 20);
            this.EmployeeNumber_TextBox.TabIndex = 3;
            this.EmployeeNumber_TextBox.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 912);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "EmpNum";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 912);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "JobNum";
            this.label2.Visible = false;
            // 
            // JobNumber_TextBox
            // 
            this.JobNumber_TextBox.Location = new System.Drawing.Point(253, 909);
            this.JobNumber_TextBox.Name = "JobNumber_TextBox";
            this.JobNumber_TextBox.ReadOnly = true;
            this.JobNumber_TextBox.Size = new System.Drawing.Size(123, 20);
            this.JobNumber_TextBox.TabIndex = 5;
            this.JobNumber_TextBox.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 912);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "TcQtuQtyComp";
            this.label3.Visible = false;
            // 
            // TotalCountQtuQtyComp_TextBox
            // 
            this.TotalCountQtuQtyComp_TextBox.Location = new System.Drawing.Point(468, 909);
            this.TotalCountQtuQtyComp_TextBox.Name = "TotalCountQtuQtyComp_TextBox";
            this.TotalCountQtuQtyComp_TextBox.ReadOnly = true;
            this.TotalCountQtuQtyComp_TextBox.Size = new System.Drawing.Size(72, 20);
            this.TotalCountQtuQtyComp_TextBox.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(546, 912);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Computer Name";
            this.label4.Visible = false;
            // 
            // ComputerName_TextBox
            // 
            this.ComputerName_TextBox.Location = new System.Drawing.Point(632, 909);
            this.ComputerName_TextBox.Name = "ComputerName_TextBox";
            this.ComputerName_TextBox.ReadOnly = true;
            this.ComputerName_TextBox.Size = new System.Drawing.Size(72, 20);
            this.ComputerName_TextBox.TabIndex = 15;
            this.ComputerName_TextBox.Visible = false;
            // 
            // Submit_Button
            // 
            this.Submit_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 38.25F);
            this.Submit_Button.Location = new System.Drawing.Point(167, 828);
            this.Submit_Button.Name = "Submit_Button";
            this.Submit_Button.Size = new System.Drawing.Size(585, 72);
            this.Submit_Button.TabIndex = 17;
            this.Submit_Button.Text = "Submit";
            this.Submit_Button.UseVisualStyleBackColor = true;
            this.Submit_Button.Click += new System.EventHandler(this.Submit_Button_Click);
            // 
            // Close_Timer
            // 
            this.Close_Timer.Interval = 1000;
            this.Close_Timer.Tick += new System.EventHandler(this.Close_Timer_Tick);
            // 
            // User_Program_Scan_Out
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 934);
            this.ControlBox = false;
            this.Controls.Add(this.Submit_Button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ComputerName_TextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TotalCountQtuQtyComp_TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.JobNumber_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EmployeeNumber_TextBox);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.Complete_Button);
            this.Controls.Add(this.ScanOutBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(780, 950);
            this.MinimumSize = new System.Drawing.Size(780, 950);
            this.Name = "User_Program_Scan_Out";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.User_Program_Scan_Out_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Complete_Button;
        private System.Windows.Forms.Button Cancel_Button;
        public System.Windows.Forms.WebBrowser ScanOutBrowser;
        public System.Windows.Forms.TextBox EmployeeNumber_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox JobNumber_TextBox;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox TotalCountQtuQtyComp_TextBox;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox ComputerName_TextBox;
        private System.Windows.Forms.Button Submit_Button;
        private System.Windows.Forms.Timer Close_Timer;
    }
}