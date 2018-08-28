namespace DMP_Brake_Press_Application
{
    partial class Report_Error
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
            this.Clock_TextBox = new System.Windows.Forms.TextBox();
            this.User_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Send_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.BrakePress_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.JobID_TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ItemID_TextBox = new System.Windows.Forms.TextBox();
            this.Instruction_Label = new System.Windows.Forms.Label();
            this.Messaging_TextBox = new System.Windows.Forms.TextBox();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.DMPID_TextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Cell_TextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SelectMessage_ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Clock_TextBox
            // 
            this.Clock_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clock_TextBox.Location = new System.Drawing.Point(799, 12);
            this.Clock_TextBox.Name = "Clock_TextBox";
            this.Clock_TextBox.Size = new System.Drawing.Size(279, 31);
            this.Clock_TextBox.TabIndex = 188;
            this.Clock_TextBox.TabStop = false;
            this.Clock_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Clock_TextBox.Enter += new System.EventHandler(this.Clock_TextBox_Enter);
            // 
            // User_TextBox
            // 
            this.User_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.User_TextBox.Location = new System.Drawing.Point(799, 50);
            this.User_TextBox.Name = "User_TextBox";
            this.User_TextBox.Size = new System.Drawing.Size(279, 32);
            this.User_TextBox.TabIndex = 187;
            this.User_TextBox.TabStop = false;
            this.User_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.User_TextBox.Enter += new System.EventHandler(this.User_TextBox_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(638, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 26);
            this.label3.TabIndex = 186;
            this.label3.Text = "Current User:";
            // 
            // Send_Button
            // 
            this.Send_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Send_Button.Location = new System.Drawing.Point(816, 142);
            this.Send_Button.Name = "Send_Button";
            this.Send_Button.Size = new System.Drawing.Size(128, 52);
            this.Send_Button.TabIndex = 189;
            this.Send_Button.Text = "Send";
            this.Send_Button.UseVisualStyleBackColor = true;
            this.Send_Button.Click += new System.EventHandler(this.Send_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_Button.Location = new System.Drawing.Point(950, 142);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(128, 52);
            this.Cancel_Button.TabIndex = 190;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // BrakePress_TextBox
            // 
            this.BrakePress_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.BrakePress_TextBox.Location = new System.Drawing.Point(170, 88);
            this.BrakePress_TextBox.Name = "BrakePress_TextBox";
            this.BrakePress_TextBox.ReadOnly = true;
            this.BrakePress_TextBox.Size = new System.Drawing.Size(103, 32);
            this.BrakePress_TextBox.TabIndex = 191;
            this.BrakePress_TextBox.TabStop = false;
            this.BrakePress_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BrakePress_TextBox.Enter += new System.EventHandler(this.BrakePress_TextBox_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 26);
            this.label1.TabIndex = 192;
            this.label1.Text = "Brake Press:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(549, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 26);
            this.label2.TabIndex = 194;
            this.label2.Text = "Job ID:";
            // 
            // JobID_TextBox
            // 
            this.JobID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.JobID_TextBox.Location = new System.Drawing.Point(643, 88);
            this.JobID_TextBox.Name = "JobID_TextBox";
            this.JobID_TextBox.ReadOnly = true;
            this.JobID_TextBox.Size = new System.Drawing.Size(109, 32);
            this.JobID_TextBox.TabIndex = 193;
            this.JobID_TextBox.TabStop = false;
            this.JobID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JobID_TextBox.Enter += new System.EventHandler(this.JobID_TextBox_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(282, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 26);
            this.label4.TabIndex = 196;
            this.label4.Text = "Item ID:";
            // 
            // ItemID_TextBox
            // 
            this.ItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.ItemID_TextBox.Location = new System.Drawing.Point(385, 88);
            this.ItemID_TextBox.Name = "ItemID_TextBox";
            this.ItemID_TextBox.ReadOnly = true;
            this.ItemID_TextBox.Size = new System.Drawing.Size(158, 32);
            this.ItemID_TextBox.TabIndex = 195;
            this.ItemID_TextBox.TabStop = false;
            this.ItemID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ItemID_TextBox.Enter += new System.EventHandler(this.ItemID_TextBox_Enter);
            // 
            // Instruction_Label
            // 
            this.Instruction_Label.AutoSize = true;
            this.Instruction_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.Instruction_Label.Location = new System.Drawing.Point(15, 212);
            this.Instruction_Label.Name = "Instruction_Label";
            this.Instruction_Label.Size = new System.Drawing.Size(335, 26);
            this.Instruction_Label.TabIndex = 198;
            this.Instruction_Label.Text = "Please Describe The Problem:";
            this.Instruction_Label.Visible = false;
            // 
            // Messaging_TextBox
            // 
            this.Messaging_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.Messaging_TextBox.Location = new System.Drawing.Point(17, 241);
            this.Messaging_TextBox.Multiline = true;
            this.Messaging_TextBox.Name = "Messaging_TextBox";
            this.Messaging_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Messaging_TextBox.Size = new System.Drawing.Size(1061, 181);
            this.Messaging_TextBox.TabIndex = 199;
            this.Messaging_TextBox.TabStop = false;
            this.Messaging_TextBox.Visible = false;
            // 
            // Clock
            // 
            this.Clock.Interval = 300;
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // DMPID_TextBox
            // 
            this.DMPID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.DMPID_TextBox.Location = new System.Drawing.Point(799, 88);
            this.DMPID_TextBox.Name = "DMPID_TextBox";
            this.DMPID_TextBox.ReadOnly = true;
            this.DMPID_TextBox.Size = new System.Drawing.Size(279, 32);
            this.DMPID_TextBox.TabIndex = 200;
            this.DMPID_TextBox.TabStop = false;
            this.DMPID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DMPID_TextBox.Visible = false;
            this.DMPID_TextBox.Enter += new System.EventHandler(this.DMPID_TextBox_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(15, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 26);
            this.label6.TabIndex = 202;
            this.label6.Text = "Cell:";
            // 
            // Cell_TextBox
            // 
            this.Cell_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.Cell_TextBox.Location = new System.Drawing.Point(82, 50);
            this.Cell_TextBox.Name = "Cell_TextBox";
            this.Cell_TextBox.ReadOnly = true;
            this.Cell_TextBox.Size = new System.Drawing.Size(194, 32);
            this.Cell_TextBox.TabIndex = 201;
            this.Cell_TextBox.TabStop = false;
            this.Cell_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Cell_TextBox.Enter += new System.EventHandler(this.Cell_TextBox_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(15, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(188, 26);
            this.label7.TabIndex = 203;
            this.label7.Text = "Select Message:";
            // 
            // SelectMessage_ComboBox
            // 
            this.SelectMessage_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.SelectMessage_ComboBox.FormattingEnabled = true;
            this.SelectMessage_ComboBox.Location = new System.Drawing.Point(209, 153);
            this.SelectMessage_ComboBox.Name = "SelectMessage_ComboBox";
            this.SelectMessage_ComboBox.Size = new System.Drawing.Size(382, 33);
            this.SelectMessage_ComboBox.TabIndex = 204;
            this.SelectMessage_ComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectMessage_ComboBox_SelectedIndexChanged);
            // 
            // Report_Error
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 206);
            this.ControlBox = false;
            this.Controls.Add(this.SelectMessage_ComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Cell_TextBox);
            this.Controls.Add(this.DMPID_TextBox);
            this.Controls.Add(this.Messaging_TextBox);
            this.Controls.Add(this.Instruction_Label);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ItemID_TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.JobID_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BrakePress_TextBox);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.Send_Button);
            this.Controls.Add(this.Clock_TextBox);
            this.Controls.Add(this.User_TextBox);
            this.Controls.Add(this.label3);
            this.MaximumSize = new System.Drawing.Size(1106, 508);
            this.MinimumSize = new System.Drawing.Size(1106, 222);
            this.Name = "Report_Error";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Report_Error_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox Clock_TextBox;
        public System.Windows.Forms.TextBox User_TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Send_Button;
        private System.Windows.Forms.Button Cancel_Button;
        public System.Windows.Forms.TextBox BrakePress_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox JobID_TextBox;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox ItemID_TextBox;
        private System.Windows.Forms.Label Instruction_Label;
        public System.Windows.Forms.TextBox Messaging_TextBox;
        private System.Windows.Forms.Timer Clock;
        public System.Windows.Forms.TextBox DMPID_TextBox;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox Cell_TextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox SelectMessage_ComboBox;
    }
}