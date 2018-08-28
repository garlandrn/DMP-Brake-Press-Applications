namespace DMP_Brake_Press_Application
{
    partial class MessageView
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
            this.MessageData_GridView = new System.Windows.Forms.DataGridView();
            this.DMPID_TextBox = new System.Windows.Forms.TextBox();
            this.Clock_TextBox = new System.Windows.Forms.TextBox();
            this.User_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Customer_TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ItemID_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.JobID_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrakePress_TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DateTime_TextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.EmployeeName_TextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DMPID_Employee_TextBox = new System.Windows.Forms.TextBox();
            this.Messaging_TextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Exit_Button = new System.Windows.Forms.Button();
            this.MessageID_TextBox = new System.Windows.Forms.TextBox();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.Delete_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MessageData_GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // MessageData_GridView
            // 
            this.MessageData_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MessageData_GridView.Location = new System.Drawing.Point(12, 12);
            this.MessageData_GridView.Name = "MessageData_GridView";
            this.MessageData_GridView.Size = new System.Drawing.Size(739, 227);
            this.MessageData_GridView.TabIndex = 0;
            this.MessageData_GridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MessageData_GridView_CellClick);
            this.MessageData_GridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.MessageData_GridView_DataError);
            // 
            // DMPID_TextBox
            // 
            this.DMPID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.DMPID_TextBox.Location = new System.Drawing.Point(934, 88);
            this.DMPID_TextBox.Name = "DMPID_TextBox";
            this.DMPID_TextBox.ReadOnly = true;
            this.DMPID_TextBox.Size = new System.Drawing.Size(279, 32);
            this.DMPID_TextBox.TabIndex = 204;
            this.DMPID_TextBox.TabStop = false;
            this.DMPID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DMPID_TextBox.Visible = false;
            // 
            // Clock_TextBox
            // 
            this.Clock_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.Clock_TextBox.Location = new System.Drawing.Point(934, 12);
            this.Clock_TextBox.Name = "Clock_TextBox";
            this.Clock_TextBox.Size = new System.Drawing.Size(279, 32);
            this.Clock_TextBox.TabIndex = 203;
            this.Clock_TextBox.TabStop = false;
            this.Clock_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Clock_TextBox.Enter += new System.EventHandler(this.Clock_TextBox_Enter);
            // 
            // User_TextBox
            // 
            this.User_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.User_TextBox.Location = new System.Drawing.Point(934, 50);
            this.User_TextBox.Name = "User_TextBox";
            this.User_TextBox.Size = new System.Drawing.Size(279, 32);
            this.User_TextBox.TabIndex = 202;
            this.User_TextBox.TabStop = false;
            this.User_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.User_TextBox.Enter += new System.EventHandler(this.User_TextBox_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(773, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 26);
            this.label3.TabIndex = 201;
            this.label3.Text = "Current User:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(12, 256);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 26);
            this.label6.TabIndex = 212;
            this.label6.Text = "Customer:";
            // 
            // Customer_TextBox
            // 
            this.Customer_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.Customer_TextBox.Location = new System.Drawing.Point(140, 256);
            this.Customer_TextBox.Name = "Customer_TextBox";
            this.Customer_TextBox.ReadOnly = true;
            this.Customer_TextBox.Size = new System.Drawing.Size(194, 32);
            this.Customer_TextBox.TabIndex = 211;
            this.Customer_TextBox.TabStop = false;
            this.Customer_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(332, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 26);
            this.label4.TabIndex = 210;
            this.label4.Text = "Item ID:";
            // 
            // ItemID_TextBox
            // 
            this.ItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.ItemID_TextBox.Location = new System.Drawing.Point(435, 295);
            this.ItemID_TextBox.Name = "ItemID_TextBox";
            this.ItemID_TextBox.ReadOnly = true;
            this.ItemID_TextBox.Size = new System.Drawing.Size(158, 32);
            this.ItemID_TextBox.TabIndex = 209;
            this.ItemID_TextBox.TabStop = false;
            this.ItemID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(599, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 26);
            this.label2.TabIndex = 208;
            this.label2.Text = "Job ID:";
            // 
            // JobID_TextBox
            // 
            this.JobID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.JobID_TextBox.Location = new System.Drawing.Point(693, 295);
            this.JobID_TextBox.Name = "JobID_TextBox";
            this.JobID_TextBox.ReadOnly = true;
            this.JobID_TextBox.Size = new System.Drawing.Size(109, 32);
            this.JobID_TextBox.TabIndex = 207;
            this.JobID_TextBox.TabStop = false;
            this.JobID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 295);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 26);
            this.label1.TabIndex = 206;
            this.label1.Text = "Brake Press:";
            // 
            // BrakePress_TextBox
            // 
            this.BrakePress_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.BrakePress_TextBox.Location = new System.Drawing.Point(167, 295);
            this.BrakePress_TextBox.Name = "BrakePress_TextBox";
            this.BrakePress_TextBox.ReadOnly = true;
            this.BrakePress_TextBox.Size = new System.Drawing.Size(159, 32);
            this.BrakePress_TextBox.TabIndex = 205;
            this.BrakePress_TextBox.TabStop = false;
            this.BrakePress_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(340, 256);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 26);
            this.label5.TabIndex = 214;
            this.label5.Text = "Date and Time:";
            // 
            // DateTime_TextBox
            // 
            this.DateTime_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.DateTime_TextBox.Location = new System.Drawing.Point(520, 256);
            this.DateTime_TextBox.Name = "DateTime_TextBox";
            this.DateTime_TextBox.ReadOnly = true;
            this.DateTime_TextBox.Size = new System.Drawing.Size(282, 32);
            this.DateTime_TextBox.TabIndex = 213;
            this.DateTime_TextBox.TabStop = false;
            this.DateTime_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(808, 256);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 26);
            this.label7.TabIndex = 216;
            this.label7.Text = "Employee:";
            // 
            // EmployeeName_TextBox
            // 
            this.EmployeeName_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.EmployeeName_TextBox.Location = new System.Drawing.Point(939, 256);
            this.EmployeeName_TextBox.Name = "EmployeeName_TextBox";
            this.EmployeeName_TextBox.ReadOnly = true;
            this.EmployeeName_TextBox.Size = new System.Drawing.Size(255, 32);
            this.EmployeeName_TextBox.TabIndex = 215;
            this.EmployeeName_TextBox.TabStop = false;
            this.EmployeeName_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(831, 295);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 26);
            this.label8.TabIndex = 218;
            this.label8.Text = "DMP ID:";
            // 
            // DMPID_Employee_TextBox
            // 
            this.DMPID_Employee_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.DMPID_Employee_TextBox.Location = new System.Drawing.Point(939, 295);
            this.DMPID_Employee_TextBox.Name = "DMPID_Employee_TextBox";
            this.DMPID_Employee_TextBox.ReadOnly = true;
            this.DMPID_Employee_TextBox.Size = new System.Drawing.Size(255, 32);
            this.DMPID_Employee_TextBox.TabIndex = 217;
            this.DMPID_Employee_TextBox.TabStop = false;
            this.DMPID_Employee_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Messaging_TextBox
            // 
            this.Messaging_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.Messaging_TextBox.Location = new System.Drawing.Point(17, 374);
            this.Messaging_TextBox.Multiline = true;
            this.Messaging_TextBox.Name = "Messaging_TextBox";
            this.Messaging_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Messaging_TextBox.Size = new System.Drawing.Size(1061, 230);
            this.Messaging_TextBox.TabIndex = 219;
            this.Messaging_TextBox.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(12, 334);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 26);
            this.label9.TabIndex = 220;
            this.label9.Text = "Message:";
            // 
            // Exit_Button
            // 
            this.Exit_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Exit_Button.Location = new System.Drawing.Point(1085, 552);
            this.Exit_Button.Name = "Exit_Button";
            this.Exit_Button.Size = new System.Drawing.Size(128, 52);
            this.Exit_Button.TabIndex = 221;
            this.Exit_Button.Text = "Exit";
            this.Exit_Button.UseVisualStyleBackColor = true;
            this.Exit_Button.Click += new System.EventHandler(this.Exit_Button_Click);
            // 
            // MessageID_TextBox
            // 
            this.MessageID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.MessageID_TextBox.Location = new System.Drawing.Point(132, 334);
            this.MessageID_TextBox.Name = "MessageID_TextBox";
            this.MessageID_TextBox.ReadOnly = true;
            this.MessageID_TextBox.Size = new System.Drawing.Size(109, 32);
            this.MessageID_TextBox.TabIndex = 222;
            this.MessageID_TextBox.TabStop = false;
            this.MessageID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MessageID_TextBox.Visible = false;
            // 
            // Clock
            // 
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // Delete_Button
            // 
            this.Delete_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Delete_Button.Location = new System.Drawing.Point(1085, 374);
            this.Delete_Button.Name = "Delete_Button";
            this.Delete_Button.Size = new System.Drawing.Size(128, 52);
            this.Delete_Button.TabIndex = 223;
            this.Delete_Button.Text = "Delete";
            this.Delete_Button.UseVisualStyleBackColor = true;
            this.Delete_Button.Click += new System.EventHandler(this.Delete_Button_Click);
            // 
            // MessageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 616);
            this.Controls.Add(this.Delete_Button);
            this.Controls.Add(this.MessageID_TextBox);
            this.Controls.Add(this.Exit_Button);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Messaging_TextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DMPID_Employee_TextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.EmployeeName_TextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DateTime_TextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Customer_TextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ItemID_TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.JobID_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BrakePress_TextBox);
            this.Controls.Add(this.DMPID_TextBox);
            this.Controls.Add(this.Clock_TextBox);
            this.Controls.Add(this.User_TextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MessageData_GridView);
            this.Name = "MessageView";
            this.Text = "Message View";
            this.Load += new System.EventHandler(this.MessageView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MessageData_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView MessageData_GridView;
        public System.Windows.Forms.TextBox DMPID_TextBox;
        public System.Windows.Forms.TextBox Clock_TextBox;
        public System.Windows.Forms.TextBox User_TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox Customer_TextBox;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox ItemID_TextBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox JobID_TextBox;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox BrakePress_TextBox;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox DateTime_TextBox;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox EmployeeName_TextBox;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox DMPID_Employee_TextBox;
        public System.Windows.Forms.TextBox Messaging_TextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button Exit_Button;
        public System.Windows.Forms.TextBox MessageID_TextBox;
        private System.Windows.Forms.Timer Clock;
        private System.Windows.Forms.Button Delete_Button;
    }
}