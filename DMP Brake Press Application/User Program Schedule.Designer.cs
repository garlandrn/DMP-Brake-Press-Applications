namespace DMP_Brake_Press_Application
{
    partial class User_Program_Schedule
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
            this.ScheduleGridView = new System.Windows.Forms.DataGridView();
            this.HideSchedule_Button = new System.Windows.Forms.Button();
            this.Schedule_Timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ScheduleGridView
            // 
            this.ScheduleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScheduleGridView.Location = new System.Drawing.Point(12, 12);
            this.ScheduleGridView.Name = "ScheduleGridView";
            this.ScheduleGridView.Size = new System.Drawing.Size(1048, 148);
            this.ScheduleGridView.TabIndex = 190;
            // 
            // HideSchedule_Button
            // 
            this.HideSchedule_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.75F, System.Drawing.FontStyle.Bold);
            this.HideSchedule_Button.Location = new System.Drawing.Point(836, 166);
            this.HideSchedule_Button.Name = "HideSchedule_Button";
            this.HideSchedule_Button.Size = new System.Drawing.Size(224, 50);
            this.HideSchedule_Button.TabIndex = 227;
            this.HideSchedule_Button.Text = "Hide Schedule";
            this.HideSchedule_Button.UseVisualStyleBackColor = true;
            this.HideSchedule_Button.Click += new System.EventHandler(this.HideSchedule_Button_Click);
            // 
            // Schedule_Timer
            // 
            this.Schedule_Timer.Interval = 10000;
            this.Schedule_Timer.Tick += new System.EventHandler(this.Schedule_Timer_Tick);
            // 
            // User_Program_Schedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 218);
            this.ControlBox = false;
            this.Controls.Add(this.HideSchedule_Button);
            this.Controls.Add(this.ScheduleGridView);
            this.Location = new System.Drawing.Point(830, 80);
            this.Name = "User_Program_Schedule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.User_Program_Schedule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ScheduleGridView;
        private System.Windows.Forms.Button HideSchedule_Button;
        private System.Windows.Forms.Timer Schedule_Timer;
    }
}