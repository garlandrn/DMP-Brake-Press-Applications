namespace DMP_Brake_Press_Application
{
    partial class User_Program_Sign_Off
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
            this.LogOff_Button = new System.Windows.Forms.Button();
            this.StaySignedIn_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.Countdown_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LogOff_Button
            // 
            this.LogOff_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogOff_Button.Location = new System.Drawing.Point(288, 106);
            this.LogOff_Button.Name = "LogOff_Button";
            this.LogOff_Button.Size = new System.Drawing.Size(270, 50);
            this.LogOff_Button.TabIndex = 0;
            this.LogOff_Button.Text = "Log Off";
            this.LogOff_Button.UseVisualStyleBackColor = true;
            this.LogOff_Button.Click += new System.EventHandler(this.LogOff_Button_Click);
            // 
            // StaySignedIn_Button
            // 
            this.StaySignedIn_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StaySignedIn_Button.Location = new System.Drawing.Point(12, 106);
            this.StaySignedIn_Button.Name = "StaySignedIn_Button";
            this.StaySignedIn_Button.Size = new System.Drawing.Size(270, 50);
            this.StaySignedIn_Button.TabIndex = 1;
            this.StaySignedIn_Button.Text = "Stay Signed In";
            this.StaySignedIn_Button.UseVisualStyleBackColor = true;
            this.StaySignedIn_Button.Click += new System.EventHandler(this.StaySignedIn_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F);
            this.label1.Location = new System.Drawing.Point(26, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(392, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "You Will Be Signed Off In:";
            this.label1.Visible = false;
            // 
            // Timer
            // 
            this.Timer.Interval = 1000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // Countdown_TextBox
            // 
            this.Countdown_TextBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Countdown_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Countdown_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Countdown_TextBox.Location = new System.Drawing.Point(12, 42);
            this.Countdown_TextBox.Name = "Countdown_TextBox";
            this.Countdown_TextBox.Size = new System.Drawing.Size(543, 37);
            this.Countdown_TextBox.TabIndex = 3;
            this.Countdown_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // User_Program_Sign_Off
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 168);
            this.Controls.Add(this.Countdown_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StaySignedIn_Button);
            this.Controls.Add(this.LogOff_Button);
            this.MaximumSize = new System.Drawing.Size(590, 206);
            this.MinimumSize = new System.Drawing.Size(590, 206);
            this.Name = "User_Program_Sign_Off";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Program Sign Off";
            this.Load += new System.EventHandler(this.User_Program_Sign_Off_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LogOff_Button;
        private System.Windows.Forms.Button StaySignedIn_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.TextBox Countdown_TextBox;
    }
}