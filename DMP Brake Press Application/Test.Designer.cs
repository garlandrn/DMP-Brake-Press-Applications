namespace DMP_Brake_Press_Application
{
    partial class Test
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Test));
            this.Value_TextBox = new System.Windows.Forms.TextBox();
            this.CPU_TextBox = new System.Windows.Forms.TextBox();
            this.Result_ListBox = new System.Windows.Forms.ListBox();
            this.Enter_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Item_Label = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ItemID_TextBox = new System.Windows.Forms.TextBox();
            this.AcroPDF = new AxAcroPDFLib.AxAcroPDF();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AcroPDF)).BeginInit();
            this.SuspendLayout();
            // 
            // Value_TextBox
            // 
            this.Value_TextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Value_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.Value_TextBox.Location = new System.Drawing.Point(87, 54);
            this.Value_TextBox.Name = "Value_TextBox";
            this.Value_TextBox.Size = new System.Drawing.Size(109, 32);
            this.Value_TextBox.TabIndex = 234;
            this.Value_TextBox.TabStop = false;
            this.Value_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CPU_TextBox
            // 
            this.CPU_TextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CPU_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.CPU_TextBox.Location = new System.Drawing.Point(87, 6);
            this.CPU_TextBox.Name = "CPU_TextBox";
            this.CPU_TextBox.Size = new System.Drawing.Size(194, 32);
            this.CPU_TextBox.TabIndex = 233;
            this.CPU_TextBox.TabStop = false;
            this.CPU_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Result_ListBox
            // 
            this.Result_ListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.Result_ListBox.FormattingEnabled = true;
            this.Result_ListBox.ItemHeight = 24;
            this.Result_ListBox.Location = new System.Drawing.Point(12, 151);
            this.Result_ListBox.Name = "Result_ListBox";
            this.Result_ListBox.Size = new System.Drawing.Size(988, 364);
            this.Result_ListBox.TabIndex = 235;
            // 
            // Enter_Button
            // 
            this.Enter_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.Enter_Button.Location = new System.Drawing.Point(287, 6);
            this.Enter_Button.Name = "Enter_Button";
            this.Enter_Button.Size = new System.Drawing.Size(86, 35);
            this.Enter_Button.TabIndex = 236;
            this.Enter_Button.Text = "Enter";
            this.Enter_Button.UseVisualStyleBackColor = true;
            this.Enter_Button.Click += new System.EventHandler(this.Enter_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(1, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 26);
            this.label1.TabIndex = 238;
            this.label1.Text = "Value:";
            // 
            // Item_Label
            // 
            this.Item_Label.AutoSize = true;
            this.Item_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.Item_Label.Location = new System.Drawing.Point(12, 9);
            this.Item_Label.Name = "Item_Label";
            this.Item_Label.Size = new System.Drawing.Size(69, 26);
            this.Item_Label.TabIndex = 237;
            this.Item_Label.Text = "CPU:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(287, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 35);
            this.button1.TabIndex = 239;
            this.button1.Text = "Enter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(17, 115);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 240;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(6, 529);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(107, 35);
            this.button3.TabIndex = 241;
            this.button3.Text = "Search";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ItemID_TextBox
            // 
            this.ItemID_TextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.ItemID_TextBox.Location = new System.Drawing.Point(112, 647);
            this.ItemID_TextBox.Name = "ItemID_TextBox";
            this.ItemID_TextBox.Size = new System.Drawing.Size(194, 32);
            this.ItemID_TextBox.TabIndex = 242;
            this.ItemID_TextBox.TabStop = false;
            this.ItemID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AcroPDF
            // 
            this.AcroPDF.Enabled = true;
            this.AcroPDF.Location = new System.Drawing.Point(635, 6);
            this.AcroPDF.Name = "AcroPDF";
            this.AcroPDF.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("AcroPDF.OcxState")));
            this.AcroPDF.Size = new System.Drawing.Size(1600, 685);
            this.AcroPDF.TabIndex = 243;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(206, 115);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 244;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1042);
            this.Controls.Add(this.Result_ListBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.AcroPDF);
            this.Controls.Add(this.ItemID_TextBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Item_Label);
            this.Controls.Add(this.Enter_Button);
            this.Controls.Add(this.Value_TextBox);
            this.Controls.Add(this.CPU_TextBox);
            this.Name = "Test";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Deactivate += new System.EventHandler(this.Test_Deactivate);
            this.Click += new System.EventHandler(this.Test_Click);
            this.Leave += new System.EventHandler(this.Test_Leave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Test_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.AcroPDF)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox Value_TextBox;
        public System.Windows.Forms.TextBox CPU_TextBox;
        private System.Windows.Forms.ListBox Result_ListBox;
        private System.Windows.Forms.Button Enter_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Item_Label;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.TextBox ItemID_TextBox;
        protected internal AxAcroPDFLib.AxAcroPDF AcroPDF;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Timer timer1;
    }
}