using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace DMP_Brake_Press_Application
{
    public partial class Report_Error : Form
    {
        public Report_Error()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        // Clock 
        private static int ClockHour;
        private static int ClockMinute;
        private static int ClockSecond;

        // SQL 
        private string SQL_Source = @"Data Source = OHN7009,49172; Initial Catalog = Brake_Press_Data; Integrated Security = True; Connect Timeout = 15;";
        private static int MessageID = 0;

        private static string[] MessageOptions = { "Camera is Not Taking Pictures", "Job Needs Vision Program", "Job is Failing While Running", "Custom" };

        private static bool MessageSelected = false;

        // Form Start
        private void Report_Error_Load(object sender, EventArgs e)
        {
            Clock.Start();
            SelectMessage_ComboBox.Items.AddRange(MessageOptions);
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            Clock.Stop();
            this.Close();
        }

        private void Send_Button_Click(object sender, EventArgs e)
        {
            if(MessageSelected == true)
            {
                SendMessage();
                Clock.Stop();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Select a Message");
            }
        }

        private void SelectMessage_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageSelected = true;
            Messaging_TextBox.Clear();
            if(SelectMessage_ComboBox.Text == "Custom")
            {
                this.ClientSize = new System.Drawing.Size(1106, 508);
                Send_Button.Location = new System.Drawing.Point(816, 428);
                Cancel_Button.Location = new System.Drawing.Point(950, 428);
                Messaging_TextBox.Visible = true;
                Instruction_Label.Visible = true;
                Messaging_TextBox.Focus();
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(1106, 222);
                Send_Button.Location = new System.Drawing.Point(816, 142);
                Cancel_Button.Location = new System.Drawing.Point(950, 142);
                Messaging_TextBox.Visible = false;
                Instruction_Label.Visible = false;
                Messaging_TextBox.Text = SelectMessage_ComboBox.Text;
            }
        }

        private void SendMessage()
        {
            try
            {
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = "ClientArray.insidedmp.com";
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential("username", "password");
                objeto_mail.From = new MailAddress("BP" + BrakePress_TextBox.Text + "@defiancemetal.com");
                objeto_mail.To.Add(new MailAddress("rgarland@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("spleiman@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("dworline@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("ngill@defiancemetal.com"));
                objeto_mail.Subject =  Cell_TextBox.Text + " Brake Press: " + BrakePress_TextBox.Text;
                objeto_mail.Body = "From: " + User_TextBox.Text + "\n"+ "DMP ID: " + DMPID_TextBox.Text + "\n" + "Current Item ID: " + ItemID_TextBox.Text + "\n" + "Vision Job ID: " + JobID_TextBox.Text + "\n\n" + "Message: \n" + Messaging_TextBox.Text + "\n\nSent: " + Clock_TextBox.Text;
                client.Send(objeto_mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Clock_Tick(object sender, EventArgs e)
        {
            string AMPM = "";
            string Date = DateTime.Today.ToShortDateString();
            string Time = "";

            ClockHour = DateTime.Now.Hour;
            ClockMinute = DateTime.Now.Minute;
            ClockSecond = DateTime.Now.Second;

            if (ClockHour > 12)
            {
                ClockHour = ClockHour - 12;
                Time += ClockHour;
                AMPM = "PM";
            }
            else if (ClockHour == 12)
            {
                Time += ClockHour;
                AMPM = "PM";
            }
            else if (ClockHour < 10)
            {
                Time += "0" + ClockHour;
                AMPM = "AM";
            }
            else if (ClockHour >= 10 && ClockHour <= 12)
            {
                Time += ClockHour;
                AMPM = "AM";
            }
            Time += ":";

            if (ClockMinute < 10)
            {
                Time += "0" + ClockMinute;
            }
            else
            {
                Time += ClockMinute;
            }
            Time += ":";
            if (ClockSecond < 10)
            {
                Time += "0" + ClockSecond;
            }
            else
            {
                Time += ClockSecond;
            }
            Time += " " + AMPM;
            Time += "   " + Date;
            Clock_TextBox.Text = Time;
        }

        private void Clock_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void User_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void BrakePress_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void JobID_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void ItemID_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void DMPID_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Cell_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
