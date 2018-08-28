namespace DMP_Brake_Press_Application
{
    partial class DMPBrakePressLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMPBrakePressLogin));
            this.EmployeeName_TextBox = new System.Windows.Forms.TextBox();
            this.OperatorLog0n_Button = new System.Windows.Forms.Button();
            this.ListBox = new System.Windows.Forms.ListBox();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LoginGridView = new System.Windows.Forms.DataGridView();
            this.EmployeeName_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DMPID_TextBox = new System.Windows.Forms.TextBox();
            this.Clock_TextBox = new System.Windows.Forms.TextBox();
            this.AdminLogin_Button = new System.Windows.Forms.Button();
            this.Exit_Button = new System.Windows.Forms.Button();
            this.JobList_Button = new System.Windows.Forms.Button();
            this.CellControl_Button = new System.Windows.Forms.Button();
            this.ReportView_Button = new System.Windows.Forms.Button();
            this.EmployeeName_ComboBox = new System.Windows.Forms.ComboBox();
            this.Operator_Login_Button = new System.Windows.Forms.Button();
            this.Test_Button = new System.Windows.Forms.Button();
            this.CellOverview_Button = new System.Windows.Forms.Button();
            this.MessageView_Button = new System.Windows.Forms.Button();
            this.AdminButtons_GroupBox = new System.Windows.Forms.GroupBox();
            this.CheckCardData_Button = new System.Windows.Forms.Button();
            this.ViewCheckCard_Button = new System.Windows.Forms.Button();
            this.TestFormsGroupBox = new System.Windows.Forms.GroupBox();
            this.SQL_Import_Button = new System.Windows.Forms.Button();
            this.BrakePressData_Button = new System.Windows.Forms.Button();
            this.ScanOut_Button = new System.Windows.Forms.Button();
            this.Camera_Timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginGridView)).BeginInit();
            this.AdminButtons_GroupBox.SuspendLayout();
            this.TestFormsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmployeeName_TextBox
            // 
            this.EmployeeName_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F);
            this.EmployeeName_TextBox.Location = new System.Drawing.Point(769, 407);
            this.EmployeeName_TextBox.Name = "EmployeeName_TextBox";
            this.EmployeeName_TextBox.Size = new System.Drawing.Size(524, 47);
            this.EmployeeName_TextBox.TabIndex = 0;
            this.EmployeeName_TextBox.Visible = false;
            // 
            // OperatorLog0n_Button
            // 
            this.OperatorLog0n_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperatorLog0n_Button.Location = new System.Drawing.Point(6, 106);
            this.OperatorLog0n_Button.Name = "OperatorLog0n_Button";
            this.OperatorLog0n_Button.Size = new System.Drawing.Size(200, 37);
            this.OperatorLog0n_Button.TabIndex = 2;
            this.OperatorLog0n_Button.Text = "User Operator Login";
            this.OperatorLog0n_Button.UseVisualStyleBackColor = true;
            this.OperatorLog0n_Button.Click += new System.EventHandler(this.OperatorLogin_Button_Click);
            // 
            // ListBox
            // 
            this.ListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F);
            this.ListBox.FormattingEnabled = true;
            this.ListBox.ItemHeight = 29;
            this.ListBox.Location = new System.Drawing.Point(453, 565);
            this.ListBox.Name = "ListBox";
            this.ListBox.Size = new System.Drawing.Size(840, 236);
            this.ListBox.TabIndex = 10;
            this.ListBox.TabStop = false;
            // 
            // Clock
            // 
            this.Clock.Interval = 250;
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(272, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1348, 306);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // LoginGridView
            // 
            this.LoginGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LoginGridView.Location = new System.Drawing.Point(453, 807);
            this.LoginGridView.Name = "LoginGridView";
            this.LoginGridView.Size = new System.Drawing.Size(177, 154);
            this.LoginGridView.TabIndex = 19;
            this.LoginGridView.Visible = false;
            // 
            // EmployeeName_Label
            // 
            this.EmployeeName_Label.AutoSize = true;
            this.EmployeeName_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.25F, System.Drawing.FontStyle.Bold);
            this.EmployeeName_Label.Location = new System.Drawing.Point(446, 408);
            this.EmployeeName_Label.Name = "EmployeeName_Label";
            this.EmployeeName_Label.Size = new System.Drawing.Size(317, 42);
            this.EmployeeName_Label.TabIndex = 5;
            this.EmployeeName_Label.Text = "Employee Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(599, 480);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 42);
            this.label1.TabIndex = 8;
            this.label1.Text = "DMP ID:";
            // 
            // DMPID_TextBox
            // 
            this.DMPID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F);
            this.DMPID_TextBox.Location = new System.Drawing.Point(769, 475);
            this.DMPID_TextBox.Name = "DMPID_TextBox";
            this.DMPID_TextBox.Size = new System.Drawing.Size(524, 47);
            this.DMPID_TextBox.TabIndex = 1;
            this.DMPID_TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DMPID_TextBox_KeyDown);
            // 
            // Clock_TextBox
            // 
            this.Clock_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Clock_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F);
            this.Clock_TextBox.Location = new System.Drawing.Point(1490, 12);
            this.Clock_TextBox.Name = "Clock_TextBox";
            this.Clock_TextBox.ReadOnly = true;
            this.Clock_TextBox.Size = new System.Drawing.Size(402, 39);
            this.Clock_TextBox.TabIndex = 15;
            this.Clock_TextBox.TabStop = false;
            // 
            // AdminLogin_Button
            // 
            this.AdminLogin_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F, System.Drawing.FontStyle.Bold);
            this.AdminLogin_Button.Location = new System.Drawing.Point(6, 19);
            this.AdminLogin_Button.Name = "AdminLogin_Button";
            this.AdminLogin_Button.Size = new System.Drawing.Size(285, 47);
            this.AdminLogin_Button.TabIndex = 3;
            this.AdminLogin_Button.Text = "Admin Login";
            this.AdminLogin_Button.UseVisualStyleBackColor = true;
            this.AdminLogin_Button.Click += new System.EventHandler(this.AdminLogin_Button_Click);
            // 
            // Exit_Button
            // 
            this.Exit_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.25F, System.Drawing.FontStyle.Bold);
            this.Exit_Button.Location = new System.Drawing.Point(1707, 901);
            this.Exit_Button.Name = "Exit_Button";
            this.Exit_Button.Size = new System.Drawing.Size(185, 60);
            this.Exit_Button.TabIndex = 5;
            this.Exit_Button.Text = "Exit";
            this.Exit_Button.UseVisualStyleBackColor = true;
            this.Exit_Button.Click += new System.EventHandler(this.Exit_Button_Click);
            // 
            // JobList_Button
            // 
            this.JobList_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F, System.Drawing.FontStyle.Bold);
            this.JobList_Button.Location = new System.Drawing.Point(6, 171);
            this.JobList_Button.Name = "JobList_Button";
            this.JobList_Button.Size = new System.Drawing.Size(285, 47);
            this.JobList_Button.TabIndex = 4;
            this.JobList_Button.Text = "Job List";
            this.JobList_Button.UseVisualStyleBackColor = true;
            this.JobList_Button.Click += new System.EventHandler(this.JobList_Button_Click);
            // 
            // CellControl_Button
            // 
            this.CellControl_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F, System.Drawing.FontStyle.Bold);
            this.CellControl_Button.Location = new System.Drawing.Point(6, 247);
            this.CellControl_Button.Name = "CellControl_Button";
            this.CellControl_Button.Size = new System.Drawing.Size(285, 47);
            this.CellControl_Button.TabIndex = 20;
            this.CellControl_Button.Text = "Cell Control";
            this.CellControl_Button.UseVisualStyleBackColor = true;
            this.CellControl_Button.Click += new System.EventHandler(this.CellControl_Button_Click);
            // 
            // ReportView_Button
            // 
            this.ReportView_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F, System.Drawing.FontStyle.Bold);
            this.ReportView_Button.Location = new System.Drawing.Point(6, 95);
            this.ReportView_Button.Name = "ReportView_Button";
            this.ReportView_Button.Size = new System.Drawing.Size(285, 47);
            this.ReportView_Button.TabIndex = 21;
            this.ReportView_Button.Text = "Report View";
            this.ReportView_Button.UseVisualStyleBackColor = true;
            this.ReportView_Button.Click += new System.EventHandler(this.ReportView_Button_Click);
            // 
            // EmployeeName_ComboBox
            // 
            this.EmployeeName_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F);
            this.EmployeeName_ComboBox.Location = new System.Drawing.Point(769, 408);
            this.EmployeeName_ComboBox.Name = "EmployeeName_ComboBox";
            this.EmployeeName_ComboBox.Size = new System.Drawing.Size(524, 47);
            this.EmployeeName_ComboBox.TabIndex = 22;
            this.EmployeeName_ComboBox.DropDown += new System.EventHandler(this.EmployeeName_ComboBox_DropDown);
            this.EmployeeName_ComboBox.SelectedIndexChanged += new System.EventHandler(this.EmployeeName_ComboBox_SelectedIndexChanged);
            this.EmployeeName_ComboBox.DropDownClosed += new System.EventHandler(this.EmployeeName_ComboBox_DropDownClosed);
            this.EmployeeName_ComboBox.Leave += new System.EventHandler(this.EmployeeName_ComboBox_Leave);
            // 
            // Operator_Login_Button
            // 
            this.Operator_Login_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F, System.Drawing.FontStyle.Bold);
            this.Operator_Login_Button.Location = new System.Drawing.Point(1329, 408);
            this.Operator_Login_Button.Name = "Operator_Login_Button";
            this.Operator_Login_Button.Size = new System.Drawing.Size(285, 47);
            this.Operator_Login_Button.TabIndex = 24;
            this.Operator_Login_Button.Text = "Operator Login";
            this.Operator_Login_Button.UseVisualStyleBackColor = true;
            this.Operator_Login_Button.Click += new System.EventHandler(this.Operator_Login_Click);
            // 
            // Test_Button
            // 
            this.Test_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Test_Button.Location = new System.Drawing.Point(6, 63);
            this.Test_Button.Name = "Test_Button";
            this.Test_Button.Size = new System.Drawing.Size(200, 37);
            this.Test_Button.TabIndex = 25;
            this.Test_Button.Text = "Test";
            this.Test_Button.UseVisualStyleBackColor = true;
            this.Test_Button.Click += new System.EventHandler(this.Test_Button_Click);
            // 
            // CellOverview_Button
            // 
            this.CellOverview_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold);
            this.CellOverview_Button.Location = new System.Drawing.Point(6, 329);
            this.CellOverview_Button.Name = "CellOverview_Button";
            this.CellOverview_Button.Size = new System.Drawing.Size(200, 47);
            this.CellOverview_Button.TabIndex = 26;
            this.CellOverview_Button.Text = "Cell Overview";
            this.CellOverview_Button.UseVisualStyleBackColor = true;
            this.CellOverview_Button.Click += new System.EventHandler(this.CellOverview_Button_Click);
            // 
            // MessageView_Button
            // 
            this.MessageView_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold);
            this.MessageView_Button.Location = new System.Drawing.Point(6, 278);
            this.MessageView_Button.Name = "MessageView_Button";
            this.MessageView_Button.Size = new System.Drawing.Size(200, 47);
            this.MessageView_Button.TabIndex = 27;
            this.MessageView_Button.Text = "Message View";
            this.MessageView_Button.UseVisualStyleBackColor = true;
            this.MessageView_Button.Click += new System.EventHandler(this.MessageView_Button_Click);
            // 
            // AdminButtons_GroupBox
            // 
            this.AdminButtons_GroupBox.Controls.Add(this.AdminLogin_Button);
            this.AdminButtons_GroupBox.Controls.Add(this.ReportView_Button);
            this.AdminButtons_GroupBox.Controls.Add(this.JobList_Button);
            this.AdminButtons_GroupBox.Controls.Add(this.CellControl_Button);
            this.AdminButtons_GroupBox.Location = new System.Drawing.Point(1323, 461);
            this.AdminButtons_GroupBox.Name = "AdminButtons_GroupBox";
            this.AdminButtons_GroupBox.Size = new System.Drawing.Size(297, 320);
            this.AdminButtons_GroupBox.TabIndex = 28;
            this.AdminButtons_GroupBox.TabStop = false;
            this.AdminButtons_GroupBox.Visible = false;
            // 
            // CheckCardData_Button
            // 
            this.CheckCardData_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckCardData_Button.Location = new System.Drawing.Point(6, 149);
            this.CheckCardData_Button.Name = "CheckCardData_Button";
            this.CheckCardData_Button.Size = new System.Drawing.Size(200, 37);
            this.CheckCardData_Button.TabIndex = 29;
            this.CheckCardData_Button.Text = "Check Card";
            this.CheckCardData_Button.UseVisualStyleBackColor = true;
            this.CheckCardData_Button.Click += new System.EventHandler(this.CheckCardData_Button_Click);
            // 
            // ViewCheckCard_Button
            // 
            this.ViewCheckCard_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.75F, System.Drawing.FontStyle.Bold);
            this.ViewCheckCard_Button.Location = new System.Drawing.Point(6, 19);
            this.ViewCheckCard_Button.Name = "ViewCheckCard_Button";
            this.ViewCheckCard_Button.Size = new System.Drawing.Size(200, 38);
            this.ViewCheckCard_Button.TabIndex = 225;
            this.ViewCheckCard_Button.Text = "View Check Card";
            this.ViewCheckCard_Button.UseVisualStyleBackColor = true;
            this.ViewCheckCard_Button.Click += new System.EventHandler(this.ViewCheckCard_Button_Click);
            // 
            // TestFormsGroupBox
            // 
            this.TestFormsGroupBox.Controls.Add(this.SQL_Import_Button);
            this.TestFormsGroupBox.Controls.Add(this.BrakePressData_Button);
            this.TestFormsGroupBox.Controls.Add(this.MessageView_Button);
            this.TestFormsGroupBox.Controls.Add(this.CellOverview_Button);
            this.TestFormsGroupBox.Controls.Add(this.ScanOut_Button);
            this.TestFormsGroupBox.Controls.Add(this.ViewCheckCard_Button);
            this.TestFormsGroupBox.Controls.Add(this.OperatorLog0n_Button);
            this.TestFormsGroupBox.Controls.Add(this.CheckCardData_Button);
            this.TestFormsGroupBox.Controls.Add(this.Test_Button);
            this.TestFormsGroupBox.Location = new System.Drawing.Point(235, 385);
            this.TestFormsGroupBox.Name = "TestFormsGroupBox";
            this.TestFormsGroupBox.Size = new System.Drawing.Size(212, 576);
            this.TestFormsGroupBox.TabIndex = 226;
            this.TestFormsGroupBox.TabStop = false;
            this.TestFormsGroupBox.Text = "Test Forms";
            this.TestFormsGroupBox.Visible = false;
            // 
            // SQL_Import_Button
            // 
            this.SQL_Import_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQL_Import_Button.Location = new System.Drawing.Point(6, 382);
            this.SQL_Import_Button.Name = "SQL_Import_Button";
            this.SQL_Import_Button.Size = new System.Drawing.Size(200, 37);
            this.SQL_Import_Button.TabIndex = 228;
            this.SQL_Import_Button.Text = "SQL Import";
            this.SQL_Import_Button.UseVisualStyleBackColor = true;
            this.SQL_Import_Button.Click += new System.EventHandler(this.SQL_Import_Button_Click);
            // 
            // BrakePressData_Button
            // 
            this.BrakePressData_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrakePressData_Button.Location = new System.Drawing.Point(6, 235);
            this.BrakePressData_Button.Name = "BrakePressData_Button";
            this.BrakePressData_Button.Size = new System.Drawing.Size(200, 37);
            this.BrakePressData_Button.TabIndex = 227;
            this.BrakePressData_Button.Text = "BP Data";
            this.BrakePressData_Button.UseVisualStyleBackColor = true;
            this.BrakePressData_Button.Click += new System.EventHandler(this.BrakePressData_Button_Click);
            // 
            // ScanOut_Button
            // 
            this.ScanOut_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScanOut_Button.Location = new System.Drawing.Point(6, 192);
            this.ScanOut_Button.Name = "ScanOut_Button";
            this.ScanOut_Button.Size = new System.Drawing.Size(200, 37);
            this.ScanOut_Button.TabIndex = 226;
            this.ScanOut_Button.Text = "Scan Out";
            this.ScanOut_Button.UseVisualStyleBackColor = true;
            this.ScanOut_Button.Click += new System.EventHandler(this.ScanOut_Button_Click);
            // 
            // Camera_Timer
            // 
            this.Camera_Timer.Enabled = true;
            this.Camera_Timer.Interval = 1000;
            this.Camera_Timer.Tick += new System.EventHandler(this.Camera_Timer_Tick);
            // 
            // DMPBrakePressLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1054);
            this.ControlBox = false;
            this.Controls.Add(this.TestFormsGroupBox);
            this.Controls.Add(this.AdminButtons_GroupBox);
            this.Controls.Add(this.Exit_Button);
            this.Controls.Add(this.Clock_TextBox);
            this.Controls.Add(this.Operator_Login_Button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DMPID_TextBox);
            this.Controls.Add(this.EmployeeName_Label);
            this.Controls.Add(this.LoginGridView);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ListBox);
            this.Controls.Add(this.EmployeeName_ComboBox);
            this.Controls.Add(this.EmployeeName_TextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DMPBrakePressLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DMP Brake Press Login Kepware 2/28/18";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DMPBrakePressLogin_FormClosing);
            this.Load += new System.EventHandler(this.DMPBrakePressLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginGridView)).EndInit();
            this.AdminButtons_GroupBox.ResumeLayout(false);
            this.TestFormsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OperatorLog0n_Button;
        private System.Windows.Forms.ListBox ListBox;
        private System.Windows.Forms.Timer Clock;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView LoginGridView;
        private System.Windows.Forms.Label EmployeeName_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DMPID_TextBox;
        private System.Windows.Forms.TextBox Clock_TextBox;
        private System.Windows.Forms.Button AdminLogin_Button;
        private System.Windows.Forms.Button Exit_Button;
        private System.Windows.Forms.Button JobList_Button;
        private System.Windows.Forms.Button CellControl_Button;
        private System.Windows.Forms.Button ReportView_Button;
        private System.Windows.Forms.ComboBox EmployeeName_ComboBox;
        public System.Windows.Forms.TextBox EmployeeName_TextBox;
        private System.Windows.Forms.Button Operator_Login_Button;
        private System.Windows.Forms.Button Test_Button;
        private System.Windows.Forms.Button CellOverview_Button;
        private System.Windows.Forms.Button MessageView_Button;
        public System.Windows.Forms.GroupBox AdminButtons_GroupBox;
        private System.Windows.Forms.Button CheckCardData_Button;
        private System.Windows.Forms.Button ViewCheckCard_Button;
        private System.Windows.Forms.GroupBox TestFormsGroupBox;
        private System.Windows.Forms.Button ScanOut_Button;
        private System.Windows.Forms.Button BrakePressData_Button;
        private System.Windows.Forms.Button SQL_Import_Button;
        private System.Windows.Forms.Timer Camera_Timer;
    }
}