namespace DMP_Brake_Press_Application
{
    partial class ReportViewer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportViewer));
            this.ReportGridView = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.JobID_TextBox = new System.Windows.Forms.TextBox();
            this.ItemID_TextBox = new System.Windows.Forms.TextBox();
            this.BrakePress_ComboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.DateStartPicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SearchDMPID_TextBox = new System.Windows.Forms.TextBox();
            this.DMPID_TextBox = new System.Windows.Forms.TextBox();
            this.Clock_TextBox = new System.Windows.Forms.TextBox();
            this.User_TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LogOff_Button = new System.Windows.Forms.Button();
            this.Search_Button = new System.Windows.Forms.Button();
            this.Clear_Button = new System.Windows.Forms.Button();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.OperationID_TextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DateEndPicker = new System.Windows.Forms.DateTimePicker();
            this.DMPID_ComboBox = new System.Windows.Forms.ComboBox();
            this.LoginGridView = new System.Windows.Forms.DataGridView();
            this.BrakePress_TextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.OperationIDResults_TextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ItemIDResults_TextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.OperationTimeResults_TextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.RunDateResults_TextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.OEEResults_TextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.EfficiencyResults_TextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.DMPIDResults_TextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.BrakePressResults_TextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.PPMResults_TextBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.PartsManufacturedResults_TextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.PlannedTimeResults_TextBox = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.EmployeeResults_TextBox = new System.Windows.Forms.TextBox();
            this.Create_Button = new System.Windows.Forms.Button();
            this.OperationInfo_GroupBox = new System.Windows.Forms.GroupBox();
            this.ReferenceNumber_TextBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.OperationData_GroupBox = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.StartTime_TextBox = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.EndTime_TextBox = new System.Windows.Forms.TextBox();
            this.Part_PictureBox = new System.Windows.Forms.PictureBox();
            this.CreateExcel_Button = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ReportGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginGridView)).BeginInit();
            this.OperationInfo_GroupBox.SuspendLayout();
            this.OperationData_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Part_PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // ReportGridView
            // 
            this.ReportGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReportGridView.Location = new System.Drawing.Point(12, 147);
            this.ReportGridView.Name = "ReportGridView";
            this.ReportGridView.Size = new System.Drawing.Size(1247, 443);
            this.ReportGridView.TabIndex = 0;
            this.ReportGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ReportGridView_CellClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(19, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 29);
            this.label2.TabIndex = 15;
            this.label2.Text = "Job ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 29);
            this.label1.TabIndex = 14;
            this.label1.Text = "Item ID:";
            // 
            // JobID_TextBox
            // 
            this.JobID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.JobID_TextBox.Location = new System.Drawing.Point(120, 87);
            this.JobID_TextBox.Name = "JobID_TextBox";
            this.JobID_TextBox.Size = new System.Drawing.Size(137, 35);
            this.JobID_TextBox.TabIndex = 13;
            this.JobID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ItemID_TextBox
            // 
            this.ItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.ItemID_TextBox.Location = new System.Drawing.Point(120, 31);
            this.ItemID_TextBox.Name = "ItemID_TextBox";
            this.ItemID_TextBox.Size = new System.Drawing.Size(257, 35);
            this.ItemID_TextBox.TabIndex = 12;
            this.ItemID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BrakePress_ComboBox
            // 
            this.BrakePress_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.BrakePress_ComboBox.FormattingEnabled = true;
            this.BrakePress_ComboBox.Location = new System.Drawing.Point(689, 88);
            this.BrakePress_ComboBox.Name = "BrakePress_ComboBox";
            this.BrakePress_ComboBox.Size = new System.Drawing.Size(108, 37);
            this.BrakePress_ComboBox.TabIndex = 117;
            this.BrakePress_ComboBox.SelectedIndexChanged += new System.EventHandler(this.BrakePress_ComboBox_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(521, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(162, 29);
            this.label12.TabIndex = 116;
            this.label12.Text = "Brake Press:";
            // 
            // DateStartPicker
            // 
            this.DateStartPicker.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.DateStartPicker.Checked = false;
            this.DateStartPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.75F);
            this.DateStartPicker.Location = new System.Drawing.Point(936, 35);
            this.DateStartPicker.MaxDate = new System.DateTime(2999, 12, 31, 0, 0, 0, 0);
            this.DateStartPicker.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.DateStartPicker.Name = "DateStartPicker";
            this.DateStartPicker.Size = new System.Drawing.Size(323, 30);
            this.DateStartPicker.TabIndex = 125;
            this.DateStartPicker.DropDown += new System.EventHandler(this.DateStartPicker_DropDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(800, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 29);
            this.label3.TabIndex = 126;
            this.label3.Text = "Date Start:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(269, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 29);
            this.label4.TabIndex = 128;
            this.label4.Text = "DMP ID:";
            // 
            // SearchDMPID_TextBox
            // 
            this.SearchDMPID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchDMPID_TextBox.Location = new System.Drawing.Point(383, 91);
            this.SearchDMPID_TextBox.Name = "SearchDMPID_TextBox";
            this.SearchDMPID_TextBox.Size = new System.Drawing.Size(108, 26);
            this.SearchDMPID_TextBox.TabIndex = 127;
            this.SearchDMPID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DMPID_TextBox
            // 
            this.DMPID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.DMPID_TextBox.Location = new System.Drawing.Point(1631, 88);
            this.DMPID_TextBox.Name = "DMPID_TextBox";
            this.DMPID_TextBox.ReadOnly = true;
            this.DMPID_TextBox.Size = new System.Drawing.Size(261, 32);
            this.DMPID_TextBox.TabIndex = 133;
            this.DMPID_TextBox.TabStop = false;
            this.DMPID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Clock_TextBox
            // 
            this.Clock_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.Clock_TextBox.Location = new System.Drawing.Point(1631, 50);
            this.Clock_TextBox.Name = "Clock_TextBox";
            this.Clock_TextBox.ReadOnly = true;
            this.Clock_TextBox.Size = new System.Drawing.Size(261, 32);
            this.Clock_TextBox.TabIndex = 132;
            this.Clock_TextBox.TabStop = false;
            this.Clock_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // User_TextBox
            // 
            this.User_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.User_TextBox.Location = new System.Drawing.Point(1631, 12);
            this.User_TextBox.Name = "User_TextBox";
            this.User_TextBox.ReadOnly = true;
            this.User_TextBox.Size = new System.Drawing.Size(261, 32);
            this.User_TextBox.TabIndex = 131;
            this.User_TextBox.TabStop = false;
            this.User_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(1470, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 26);
            this.label5.TabIndex = 130;
            this.label5.Text = "Current User:";
            // 
            // LogOff_Button
            // 
            this.LogOff_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.LogOff_Button.Location = new System.Drawing.Point(1749, 126);
            this.LogOff_Button.Name = "LogOff_Button";
            this.LogOff_Button.Size = new System.Drawing.Size(143, 32);
            this.LogOff_Button.TabIndex = 129;
            this.LogOff_Button.Text = "Log Off";
            this.LogOff_Button.UseVisualStyleBackColor = true;
            this.LogOff_Button.Click += new System.EventHandler(this.LogOff_Button_Click);
            // 
            // Search_Button
            // 
            this.Search_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.Search_Button.Location = new System.Drawing.Point(1279, 142);
            this.Search_Button.Name = "Search_Button";
            this.Search_Button.Size = new System.Drawing.Size(135, 32);
            this.Search_Button.TabIndex = 134;
            this.Search_Button.Text = "Search";
            this.Search_Button.UseVisualStyleBackColor = true;
            this.Search_Button.Click += new System.EventHandler(this.Search_Button_Click);
            // 
            // Clear_Button
            // 
            this.Clear_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.Clear_Button.Location = new System.Drawing.Point(1420, 142);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(135, 32);
            this.Clear_Button.TabIndex = 135;
            this.Clear_Button.Text = "Clear";
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // Clock
            // 
            this.Clock.Interval = 250;
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(401, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 29);
            this.label6.TabIndex = 137;
            this.label6.Text = "Operation ID:";
            // 
            // OperationID_TextBox
            // 
            this.OperationID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.OperationID_TextBox.Location = new System.Drawing.Point(575, 32);
            this.OperationID_TextBox.Name = "OperationID_TextBox";
            this.OperationID_TextBox.Size = new System.Drawing.Size(222, 35);
            this.OperationID_TextBox.TabIndex = 136;
            this.OperationID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(803, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 29);
            this.label7.TabIndex = 139;
            this.label7.Text = "Date End:";
            // 
            // DateEndPicker
            // 
            this.DateEndPicker.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.DateEndPicker.Checked = false;
            this.DateEndPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.75F);
            this.DateEndPicker.Location = new System.Drawing.Point(936, 92);
            this.DateEndPicker.MaxDate = new System.DateTime(2999, 12, 31, 0, 0, 0, 0);
            this.DateEndPicker.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.DateEndPicker.Name = "DateEndPicker";
            this.DateEndPicker.Size = new System.Drawing.Size(323, 30);
            this.DateEndPicker.TabIndex = 138;
            this.DateEndPicker.DropDown += new System.EventHandler(this.DateEndPicker_DropDown);
            // 
            // DMPID_ComboBox
            // 
            this.DMPID_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.DMPID_ComboBox.FormattingEnabled = true;
            this.DMPID_ComboBox.Location = new System.Drawing.Point(383, 88);
            this.DMPID_ComboBox.Name = "DMPID_ComboBox";
            this.DMPID_ComboBox.Size = new System.Drawing.Size(108, 37);
            this.DMPID_ComboBox.TabIndex = 140;
            this.DMPID_ComboBox.SelectedIndexChanged += new System.EventHandler(this.DMPID_ComboBox_SelectedIndexChanged);
            // 
            // LoginGridView
            // 
            this.LoginGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LoginGridView.Location = new System.Drawing.Point(1244, 12);
            this.LoginGridView.Name = "LoginGridView";
            this.LoginGridView.Size = new System.Drawing.Size(11, 11);
            this.LoginGridView.TabIndex = 141;
            // 
            // BrakePress_TextBox
            // 
            this.BrakePress_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrakePress_TextBox.Location = new System.Drawing.Point(689, 93);
            this.BrakePress_TextBox.Name = "BrakePress_TextBox";
            this.BrakePress_TextBox.Size = new System.Drawing.Size(91, 26);
            this.BrakePress_TextBox.TabIndex = 142;
            this.BrakePress_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(399, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(168, 29);
            this.label8.TabIndex = 146;
            this.label8.Text = "Operation ID:";
            // 
            // OperationIDResults_TextBox
            // 
            this.OperationIDResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.OperationIDResults_TextBox.Location = new System.Drawing.Point(573, 28);
            this.OperationIDResults_TextBox.Name = "OperationIDResults_TextBox";
            this.OperationIDResults_TextBox.Size = new System.Drawing.Size(170, 35);
            this.OperationIDResults_TextBox.TabIndex = 145;
            this.OperationIDResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(17, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 29);
            this.label9.TabIndex = 144;
            this.label9.Text = "Item ID:";
            // 
            // ItemIDResults_TextBox
            // 
            this.ItemIDResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.ItemIDResults_TextBox.Location = new System.Drawing.Point(120, 28);
            this.ItemIDResults_TextBox.Name = "ItemIDResults_TextBox";
            this.ItemIDResults_TextBox.Size = new System.Drawing.Size(257, 35);
            this.ItemIDResults_TextBox.TabIndex = 143;
            this.ItemIDResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(13, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(203, 29);
            this.label10.TabIndex = 150;
            this.label10.Text = "Operation Time:";
            // 
            // OperationTimeResults_TextBox
            // 
            this.OperationTimeResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.OperationTimeResults_TextBox.Location = new System.Drawing.Point(222, 97);
            this.OperationTimeResults_TextBox.Name = "OperationTimeResults_TextBox";
            this.OperationTimeResults_TextBox.Size = new System.Drawing.Size(257, 35);
            this.OperationTimeResults_TextBox.TabIndex = 149;
            this.OperationTimeResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(774, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(127, 29);
            this.label11.TabIndex = 148;
            this.label11.Text = "Run Date:";
            // 
            // RunDateResults_TextBox
            // 
            this.RunDateResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.RunDateResults_TextBox.Location = new System.Drawing.Point(907, 85);
            this.RunDateResults_TextBox.Name = "RunDateResults_TextBox";
            this.RunDateResults_TextBox.Size = new System.Drawing.Size(312, 35);
            this.RunDateResults_TextBox.TabIndex = 147;
            this.RunDateResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(1019, 102);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 29);
            this.label13.TabIndex = 154;
            this.label13.Text = "OEE:";
            // 
            // OEEResults_TextBox
            // 
            this.OEEResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.OEEResults_TextBox.Location = new System.Drawing.Point(1099, 100);
            this.OEEResults_TextBox.Name = "OEEResults_TextBox";
            this.OEEResults_TextBox.Size = new System.Drawing.Size(120, 35);
            this.OEEResults_TextBox.TabIndex = 153;
            this.OEEResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(731, 102);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(132, 29);
            this.label14.TabIndex = 152;
            this.label14.Text = "Efficiency:";
            // 
            // EfficiencyResults_TextBox
            // 
            this.EfficiencyResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.EfficiencyResults_TextBox.Location = new System.Drawing.Point(869, 100);
            this.EfficiencyResults_TextBox.Name = "EfficiencyResults_TextBox";
            this.EfficiencyResults_TextBox.Size = new System.Drawing.Size(120, 35);
            this.EfficiencyResults_TextBox.TabIndex = 151;
            this.EfficiencyResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(459, 87);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(108, 29);
            this.label15.TabIndex = 158;
            this.label15.Text = "DMP ID:";
            // 
            // DMPIDResults_TextBox
            // 
            this.DMPIDResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.DMPIDResults_TextBox.Location = new System.Drawing.Point(573, 85);
            this.DMPIDResults_TextBox.Name = "DMPIDResults_TextBox";
            this.DMPIDResults_TextBox.Size = new System.Drawing.Size(149, 35);
            this.DMPIDResults_TextBox.TabIndex = 157;
            this.DMPIDResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(918, 43);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(162, 29);
            this.label16.TabIndex = 156;
            this.label16.Text = "Brake Press:";
            // 
            // BrakePressResults_TextBox
            // 
            this.BrakePressResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.BrakePressResults_TextBox.Location = new System.Drawing.Point(1086, 41);
            this.BrakePressResults_TextBox.Name = "BrakePressResults_TextBox";
            this.BrakePressResults_TextBox.Size = new System.Drawing.Size(133, 35);
            this.BrakePressResults_TextBox.TabIndex = 155;
            this.BrakePressResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(505, 102);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(75, 29);
            this.label17.TabIndex = 162;
            this.label17.Text = "PPM:";
            // 
            // PPMResults_TextBox
            // 
            this.PPMResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.PPMResults_TextBox.Location = new System.Drawing.Point(586, 100);
            this.PPMResults_TextBox.Name = "PPMResults_TextBox";
            this.PPMResults_TextBox.Size = new System.Drawing.Size(120, 35);
            this.PPMResults_TextBox.TabIndex = 161;
            this.PPMResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label18.Location = new System.Drawing.Point(505, 43);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(244, 29);
            this.label18.TabIndex = 160;
            this.label18.Text = "Parts Manufactured:";
            // 
            // PartsManufacturedResults_TextBox
            // 
            this.PartsManufacturedResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.PartsManufacturedResults_TextBox.Location = new System.Drawing.Point(755, 41);
            this.PartsManufacturedResults_TextBox.Name = "PartsManufacturedResults_TextBox";
            this.PartsManufacturedResults_TextBox.Size = new System.Drawing.Size(142, 35);
            this.PartsManufacturedResults_TextBox.TabIndex = 159;
            this.PartsManufacturedResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label19.Location = new System.Drawing.Point(33, 43);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(183, 29);
            this.label19.TabIndex = 166;
            this.label19.Text = "Planned Time:";
            // 
            // PlannedTimeResults_TextBox
            // 
            this.PlannedTimeResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.PlannedTimeResults_TextBox.Location = new System.Drawing.Point(222, 41);
            this.PlannedTimeResults_TextBox.Name = "PlannedTimeResults_TextBox";
            this.PlannedTimeResults_TextBox.Size = new System.Drawing.Size(257, 35);
            this.PlannedTimeResults_TextBox.TabIndex = 165;
            this.PlannedTimeResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label20.Location = new System.Drawing.Point(17, 87);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(137, 29);
            this.label20.TabIndex = 164;
            this.label20.Text = "Employee:";
            // 
            // EmployeeResults_TextBox
            // 
            this.EmployeeResults_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.EmployeeResults_TextBox.Location = new System.Drawing.Point(160, 84);
            this.EmployeeResults_TextBox.Name = "EmployeeResults_TextBox";
            this.EmployeeResults_TextBox.Size = new System.Drawing.Size(217, 35);
            this.EmployeeResults_TextBox.TabIndex = 163;
            this.EmployeeResults_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Create_Button
            // 
            this.Create_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.Create_Button.Location = new System.Drawing.Point(1279, 180);
            this.Create_Button.Name = "Create_Button";
            this.Create_Button.Size = new System.Drawing.Size(276, 32);
            this.Create_Button.TabIndex = 167;
            this.Create_Button.Text = "Create PDF Report";
            this.Create_Button.UseVisualStyleBackColor = true;
            this.Create_Button.Click += new System.EventHandler(this.Create_Button_Click);
            // 
            // OperationInfo_GroupBox
            // 
            this.OperationInfo_GroupBox.BackColor = System.Drawing.Color.LightGray;
            this.OperationInfo_GroupBox.Controls.Add(this.ReferenceNumber_TextBox);
            this.OperationInfo_GroupBox.Controls.Add(this.label21);
            this.OperationInfo_GroupBox.Controls.Add(this.label9);
            this.OperationInfo_GroupBox.Controls.Add(this.ItemIDResults_TextBox);
            this.OperationInfo_GroupBox.Controls.Add(this.OperationIDResults_TextBox);
            this.OperationInfo_GroupBox.Controls.Add(this.label8);
            this.OperationInfo_GroupBox.Controls.Add(this.label20);
            this.OperationInfo_GroupBox.Controls.Add(this.EmployeeResults_TextBox);
            this.OperationInfo_GroupBox.Controls.Add(this.RunDateResults_TextBox);
            this.OperationInfo_GroupBox.Controls.Add(this.label11);
            this.OperationInfo_GroupBox.Controls.Add(this.DMPIDResults_TextBox);
            this.OperationInfo_GroupBox.Controls.Add(this.label15);
            this.OperationInfo_GroupBox.Location = new System.Drawing.Point(12, 605);
            this.OperationInfo_GroupBox.Name = "OperationInfo_GroupBox";
            this.OperationInfo_GroupBox.Size = new System.Drawing.Size(1243, 133);
            this.OperationInfo_GroupBox.TabIndex = 168;
            this.OperationInfo_GroupBox.TabStop = false;
            this.OperationInfo_GroupBox.Text = "groupBox1";
            // 
            // ReferenceNumber_TextBox
            // 
            this.ReferenceNumber_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.ReferenceNumber_TextBox.Location = new System.Drawing.Point(1021, 28);
            this.ReferenceNumber_TextBox.Name = "ReferenceNumber_TextBox";
            this.ReferenceNumber_TextBox.Size = new System.Drawing.Size(198, 35);
            this.ReferenceNumber_TextBox.TabIndex = 169;
            this.ReferenceNumber_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label21.Location = new System.Drawing.Point(774, 30);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(241, 29);
            this.label21.TabIndex = 170;
            this.label21.Text = "Reference Number:";
            // 
            // OperationData_GroupBox
            // 
            this.OperationData_GroupBox.BackColor = System.Drawing.Color.LightGray;
            this.OperationData_GroupBox.Controls.Add(this.label22);
            this.OperationData_GroupBox.Controls.Add(this.StartTime_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.label23);
            this.OperationData_GroupBox.Controls.Add(this.EndTime_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.label19);
            this.OperationData_GroupBox.Controls.Add(this.PlannedTimeResults_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.PartsManufacturedResults_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.label16);
            this.OperationData_GroupBox.Controls.Add(this.BrakePressResults_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.label17);
            this.OperationData_GroupBox.Controls.Add(this.label18);
            this.OperationData_GroupBox.Controls.Add(this.label14);
            this.OperationData_GroupBox.Controls.Add(this.EfficiencyResults_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.label13);
            this.OperationData_GroupBox.Controls.Add(this.PPMResults_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.OEEResults_TextBox);
            this.OperationData_GroupBox.Controls.Add(this.label10);
            this.OperationData_GroupBox.Controls.Add(this.OperationTimeResults_TextBox);
            this.OperationData_GroupBox.Location = new System.Drawing.Point(12, 744);
            this.OperationData_GroupBox.Name = "OperationData_GroupBox";
            this.OperationData_GroupBox.Size = new System.Drawing.Size(1243, 254);
            this.OperationData_GroupBox.TabIndex = 169;
            this.OperationData_GroupBox.TabStop = false;
            this.OperationData_GroupBox.Text = "groupBox2";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label22.Location = new System.Drawing.Point(75, 155);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(141, 29);
            this.label22.TabIndex = 170;
            this.label22.Text = "Start Time:";
            // 
            // StartTime_TextBox
            // 
            this.StartTime_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.StartTime_TextBox.Location = new System.Drawing.Point(222, 153);
            this.StartTime_TextBox.Name = "StartTime_TextBox";
            this.StartTime_TextBox.Size = new System.Drawing.Size(257, 35);
            this.StartTime_TextBox.TabIndex = 169;
            this.StartTime_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label23.Location = new System.Drawing.Point(83, 211);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(133, 29);
            this.label23.TabIndex = 168;
            this.label23.Text = "End Time:";
            // 
            // EndTime_TextBox
            // 
            this.EndTime_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.EndTime_TextBox.Location = new System.Drawing.Point(222, 209);
            this.EndTime_TextBox.Name = "EndTime_TextBox";
            this.EndTime_TextBox.Size = new System.Drawing.Size(257, 35);
            this.EndTime_TextBox.TabIndex = 167;
            this.EndTime_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Part_PictureBox
            // 
            this.Part_PictureBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Part_PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Part_PictureBox.Location = new System.Drawing.Point(1279, 256);
            this.Part_PictureBox.Name = "Part_PictureBox";
            this.Part_PictureBox.Size = new System.Drawing.Size(613, 424);
            this.Part_PictureBox.TabIndex = 212;
            this.Part_PictureBox.TabStop = false;
            // 
            // CreateExcel_Button
            // 
            this.CreateExcel_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.CreateExcel_Button.Location = new System.Drawing.Point(1279, 218);
            this.CreateExcel_Button.Name = "CreateExcel_Button";
            this.CreateExcel_Button.Size = new System.Drawing.Size(276, 32);
            this.CreateExcel_Button.TabIndex = 213;
            this.CreateExcel_Button.Text = "Create Excel Report";
            this.CreateExcel_Button.UseVisualStyleBackColor = true;
            this.CreateExcel_Button.Click += new System.EventHandler(this.CreateExcel_Button_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(1279, 687);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(613, 300);
            this.chart1.TabIndex = 214;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1042);
            this.ControlBox = false;
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.CreateExcel_Button);
            this.Controls.Add(this.Part_PictureBox);
            this.Controls.Add(this.OperationData_GroupBox);
            this.Controls.Add(this.OperationInfo_GroupBox);
            this.Controls.Add(this.Create_Button);
            this.Controls.Add(this.LoginGridView);
            this.Controls.Add(this.DMPID_ComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.DateEndPicker);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.OperationID_TextBox);
            this.Controls.Add(this.Clear_Button);
            this.Controls.Add(this.Search_Button);
            this.Controls.Add(this.DMPID_TextBox);
            this.Controls.Add(this.Clock_TextBox);
            this.Controls.Add(this.User_TextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LogOff_Button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SearchDMPID_TextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DateStartPicker);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.JobID_TextBox);
            this.Controls.Add(this.ItemID_TextBox);
            this.Controls.Add(this.ReportGridView);
            this.Controls.Add(this.BrakePress_ComboBox);
            this.Controls.Add(this.BrakePress_TextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ReportViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReportGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginGridView)).EndInit();
            this.OperationInfo_GroupBox.ResumeLayout(false);
            this.OperationInfo_GroupBox.PerformLayout();
            this.OperationData_GroupBox.ResumeLayout(false);
            this.OperationData_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Part_PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ReportGridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox JobID_TextBox;
        private System.Windows.Forms.TextBox ItemID_TextBox;
        internal System.Windows.Forms.ComboBox BrakePress_ComboBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker DateStartPicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SearchDMPID_TextBox;
        public System.Windows.Forms.TextBox DMPID_TextBox;
        public System.Windows.Forms.TextBox Clock_TextBox;
        public System.Windows.Forms.TextBox User_TextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button LogOff_Button;
        private System.Windows.Forms.Button Search_Button;
        private System.Windows.Forms.Button Clear_Button;
        private System.Windows.Forms.Timer Clock;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox OperationID_TextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker DateEndPicker;
        private System.Windows.Forms.ComboBox DMPID_ComboBox;
        private System.Windows.Forms.DataGridView LoginGridView;
        private System.Windows.Forms.TextBox BrakePress_TextBox;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox OperationIDResults_TextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ItemIDResults_TextBox;
        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.TextBox OperationTimeResults_TextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox RunDateResults_TextBox;
        private System.Windows.Forms.Label label13;
        internal System.Windows.Forms.TextBox OEEResults_TextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox EfficiencyResults_TextBox;
        private System.Windows.Forms.Label label15;
        internal System.Windows.Forms.TextBox DMPIDResults_TextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox BrakePressResults_TextBox;
        private System.Windows.Forms.Label label17;
        internal System.Windows.Forms.TextBox PPMResults_TextBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox PartsManufacturedResults_TextBox;
        private System.Windows.Forms.Label label19;
        internal System.Windows.Forms.TextBox PlannedTimeResults_TextBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox EmployeeResults_TextBox;
        private System.Windows.Forms.Button Create_Button;
        private System.Windows.Forms.GroupBox OperationInfo_GroupBox;
        internal System.Windows.Forms.TextBox ReferenceNumber_TextBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox OperationData_GroupBox;
        private System.Windows.Forms.PictureBox Part_PictureBox;
        private System.Windows.Forms.Button CreateExcel_Button;
        private System.Windows.Forms.Label label22;
        internal System.Windows.Forms.TextBox StartTime_TextBox;
        private System.Windows.Forms.Label label23;
        internal System.Windows.Forms.TextBox EndTime_TextBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}