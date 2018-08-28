namespace DMP_Brake_Press_Application
{
    partial class AdminAccess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminAccess));
            this.AddUser_Button = new System.Windows.Forms.Button();
            this.RemoveUser_Button = new System.Windows.Forms.Button();
            this.EditUser_Button = new System.Windows.Forms.Button();
            this.LogOff_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DMPID_TextBox = new System.Windows.Forms.TextBox();
            this.EmployeeName_TextBox = new System.Windows.Forms.TextBox();
            this.EmployeePassword_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AdminGridView = new System.Windows.Forms.DataGridView();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.UserNumber_TextBox = new System.Windows.Forms.TextBox();
            this.Clock_TextBox = new System.Windows.Forms.TextBox();
            this.User_TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.Confirm_Button = new System.Windows.Forms.Button();
            this.Search_Button = new System.Windows.Forms.Button();
            this.SearchName_CheckBox = new System.Windows.Forms.CheckBox();
            this.SearchDMPID_CheckBox = new System.Windows.Forms.CheckBox();
            this.Clear_Button = new System.Windows.Forms.Button();
            this.EmployeeData_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AdminGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // AddUser_Button
            // 
            this.AddUser_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddUser_Button.Location = new System.Drawing.Point(424, 263);
            this.AddUser_Button.Name = "AddUser_Button";
            this.AddUser_Button.Size = new System.Drawing.Size(148, 40);
            this.AddUser_Button.TabIndex = 0;
            this.AddUser_Button.Text = "Add User";
            this.AddUser_Button.UseVisualStyleBackColor = true;
            this.AddUser_Button.Click += new System.EventHandler(this.AddUser_Button_Click);
            // 
            // RemoveUser_Button
            // 
            this.RemoveUser_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveUser_Button.Location = new System.Drawing.Point(424, 355);
            this.RemoveUser_Button.Name = "RemoveUser_Button";
            this.RemoveUser_Button.Size = new System.Drawing.Size(148, 40);
            this.RemoveUser_Button.TabIndex = 1;
            this.RemoveUser_Button.Text = "Remove User";
            this.RemoveUser_Button.UseVisualStyleBackColor = true;
            this.RemoveUser_Button.Click += new System.EventHandler(this.RemoveUser_Button_Click);
            // 
            // EditUser_Button
            // 
            this.EditUser_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditUser_Button.Location = new System.Drawing.Point(424, 309);
            this.EditUser_Button.Name = "EditUser_Button";
            this.EditUser_Button.Size = new System.Drawing.Size(148, 40);
            this.EditUser_Button.TabIndex = 2;
            this.EditUser_Button.Text = "Edit User";
            this.EditUser_Button.UseVisualStyleBackColor = true;
            this.EditUser_Button.Click += new System.EventHandler(this.EditUser_Button_Click);
            // 
            // LogOff_Button
            // 
            this.LogOff_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogOff_Button.Location = new System.Drawing.Point(664, 493);
            this.LogOff_Button.Name = "LogOff_Button";
            this.LogOff_Button.Size = new System.Drawing.Size(120, 40);
            this.LogOff_Button.TabIndex = 7;
            this.LogOff_Button.Text = "Log Off";
            this.LogOff_Button.UseVisualStyleBackColor = true;
            this.LogOff_Button.Click += new System.EventHandler(this.LogOff_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new System.Drawing.Point(8, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "DMP ID:";
            // 
            // DMPID_TextBox
            // 
            this.DMPID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.DMPID_TextBox.Location = new System.Drawing.Point(12, 172);
            this.DMPID_TextBox.Name = "DMPID_TextBox";
            this.DMPID_TextBox.Size = new System.Drawing.Size(294, 26);
            this.DMPID_TextBox.TabIndex = 9;
            // 
            // EmployeeName_TextBox
            // 
            this.EmployeeName_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmployeeName_TextBox.Location = new System.Drawing.Point(12, 116);
            this.EmployeeName_TextBox.Name = "EmployeeName_TextBox";
            this.EmployeeName_TextBox.Size = new System.Drawing.Size(294, 26);
            this.EmployeeName_TextBox.TabIndex = 10;
            // 
            // EmployeePassword_TextBox
            // 
            this.EmployeePassword_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.EmployeePassword_TextBox.Location = new System.Drawing.Point(312, 116);
            this.EmployeePassword_TextBox.Name = "EmployeePassword_TextBox";
            this.EmployeePassword_TextBox.Size = new System.Drawing.Size(294, 26);
            this.EmployeePassword_TextBox.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label2.Location = new System.Drawing.Point(8, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = "Employee Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label3.Location = new System.Drawing.Point(308, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Employee Password:";
            // 
            // AdminGridView
            // 
            this.AdminGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AdminGridView.Location = new System.Drawing.Point(12, 263);
            this.AdminGridView.Name = "AdminGridView";
            this.AdminGridView.ReadOnly = true;
            this.AdminGridView.Size = new System.Drawing.Size(360, 270);
            this.AdminGridView.TabIndex = 14;
            this.AdminGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AdminGridView_CellClick);
            // 
            // Clock
            // 
            this.Clock.Interval = 150;
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // UserNumber_TextBox
            // 
            this.UserNumber_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNumber_TextBox.Location = new System.Drawing.Point(594, 44);
            this.UserNumber_TextBox.Name = "UserNumber_TextBox";
            this.UserNumber_TextBox.ReadOnly = true;
            this.UserNumber_TextBox.Size = new System.Drawing.Size(190, 26);
            this.UserNumber_TextBox.TabIndex = 72;
            this.UserNumber_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Clock_TextBox
            // 
            this.Clock_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clock_TextBox.Location = new System.Drawing.Point(594, 76);
            this.Clock_TextBox.Name = "Clock_TextBox";
            this.Clock_TextBox.ReadOnly = true;
            this.Clock_TextBox.Size = new System.Drawing.Size(190, 26);
            this.Clock_TextBox.TabIndex = 71;
            this.Clock_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // User_TextBox
            // 
            this.User_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.User_TextBox.Location = new System.Drawing.Point(594, 12);
            this.User_TextBox.Name = "User_TextBox";
            this.User_TextBox.ReadOnly = true;
            this.User_TextBox.Size = new System.Drawing.Size(190, 26);
            this.User_TextBox.TabIndex = 70;
            this.User_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(454, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 24);
            this.label4.TabIndex = 69;
            this.label4.Text = "Current User:";
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_Button.Location = new System.Drawing.Point(424, 493);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(148, 40);
            this.Cancel_Button.TabIndex = 73;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Visible = false;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // Confirm_Button
            // 
            this.Confirm_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Confirm_Button.Location = new System.Drawing.Point(424, 447);
            this.Confirm_Button.Name = "Confirm_Button";
            this.Confirm_Button.Size = new System.Drawing.Size(148, 40);
            this.Confirm_Button.TabIndex = 74;
            this.Confirm_Button.Text = "Confirm";
            this.Confirm_Button.UseVisualStyleBackColor = true;
            this.Confirm_Button.Visible = false;
            this.Confirm_Button.Click += new System.EventHandler(this.Confirm_Button_Click);
            // 
            // Search_Button
            // 
            this.Search_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Search_Button.Location = new System.Drawing.Point(142, 217);
            this.Search_Button.Name = "Search_Button";
            this.Search_Button.Size = new System.Drawing.Size(124, 40);
            this.Search_Button.TabIndex = 75;
            this.Search_Button.Text = "Search";
            this.Search_Button.UseVisualStyleBackColor = true;
            this.Search_Button.Click += new System.EventHandler(this.Search_Button_Click);
            // 
            // SearchName_CheckBox
            // 
            this.SearchName_CheckBox.AutoSize = true;
            this.SearchName_CheckBox.Location = new System.Drawing.Point(312, 177);
            this.SearchName_CheckBox.Name = "SearchName_CheckBox";
            this.SearchName_CheckBox.Size = new System.Drawing.Size(106, 17);
            this.SearchName_CheckBox.TabIndex = 76;
            this.SearchName_CheckBox.Text = "Search By Name";
            this.SearchName_CheckBox.UseVisualStyleBackColor = true;
            this.SearchName_CheckBox.CheckedChanged += new System.EventHandler(this.SearchName_CheckBox_CheckedChanged);
            // 
            // SearchDMPID_CheckBox
            // 
            this.SearchDMPID_CheckBox.AutoSize = true;
            this.SearchDMPID_CheckBox.Location = new System.Drawing.Point(424, 177);
            this.SearchDMPID_CheckBox.Name = "SearchDMPID_CheckBox";
            this.SearchDMPID_CheckBox.Size = new System.Drawing.Size(116, 17);
            this.SearchDMPID_CheckBox.TabIndex = 77;
            this.SearchDMPID_CheckBox.Text = "Search By DMP ID";
            this.SearchDMPID_CheckBox.UseVisualStyleBackColor = true;
            this.SearchDMPID_CheckBox.CheckedChanged += new System.EventHandler(this.SearchDMPID_CheckBox_CheckedChanged);
            // 
            // Clear_Button
            // 
            this.Clear_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clear_Button.Location = new System.Drawing.Point(12, 217);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(124, 40);
            this.Clear_Button.TabIndex = 78;
            this.Clear_Button.Text = "Clear";
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // EmployeeData_Button
            // 
            this.EmployeeData_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmployeeData_Button.Location = new System.Drawing.Point(610, 263);
            this.EmployeeData_Button.Name = "EmployeeData_Button";
            this.EmployeeData_Button.Size = new System.Drawing.Size(174, 40);
            this.EmployeeData_Button.TabIndex = 79;
            this.EmployeeData_Button.Text = "Employee Data";
            this.EmployeeData_Button.UseVisualStyleBackColor = true;
            this.EmployeeData_Button.Click += new System.EventHandler(this.EmployeeData_Button_Click);
            // 
            // AdminAccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 545);
            this.ControlBox = false;
            this.Controls.Add(this.EmployeeData_Button);
            this.Controls.Add(this.Clear_Button);
            this.Controls.Add(this.SearchDMPID_CheckBox);
            this.Controls.Add(this.SearchName_CheckBox);
            this.Controls.Add(this.Search_Button);
            this.Controls.Add(this.Confirm_Button);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.UserNumber_TextBox);
            this.Controls.Add(this.Clock_TextBox);
            this.Controls.Add(this.User_TextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AdminGridView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EmployeePassword_TextBox);
            this.Controls.Add(this.EmployeeName_TextBox);
            this.Controls.Add(this.DMPID_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogOff_Button);
            this.Controls.Add(this.EditUser_Button);
            this.Controls.Add(this.RemoveUser_Button);
            this.Controls.Add(this.AddUser_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdminAccess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Control";
            this.Load += new System.EventHandler(this.AdminAccess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AdminGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddUser_Button;
        private System.Windows.Forms.Button RemoveUser_Button;
        private System.Windows.Forms.Button EditUser_Button;
        private System.Windows.Forms.Button LogOff_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DMPID_TextBox;
        private System.Windows.Forms.TextBox EmployeeName_TextBox;
        private System.Windows.Forms.TextBox EmployeePassword_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;


        private System.Windows.Forms.DataGridView AdminGridView;

        private System.Windows.Forms.Timer Clock;
        public System.Windows.Forms.TextBox UserNumber_TextBox;
        public System.Windows.Forms.TextBox Clock_TextBox;
        public System.Windows.Forms.TextBox User_TextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Button Confirm_Button;
        private System.Windows.Forms.Button Search_Button;
        private System.Windows.Forms.CheckBox SearchName_CheckBox;
        private System.Windows.Forms.CheckBox SearchDMPID_CheckBox;
        private System.Windows.Forms.Button Clear_Button;
        private System.Windows.Forms.Button EmployeeData_Button;
    }
}


