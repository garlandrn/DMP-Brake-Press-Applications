using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DMP_Brake_Press_Application
{
    public partial class MessageView : Form
    {
        public MessageView()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        // Clock_Tick();
        private static int ClockHour;
        private static int ClockMinute;
        private static int ClockSecond;

        string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;";

        private void MessageView_Load(object sender, EventArgs e)
        {
            ConnectToMessages();
        }

        private void ConnectToMessages()
        {
            SqlConnection MessageConnect = new SqlConnection(SQL_Source);
            string ConnectMessage = "SELECT * FROM [dbo].[ErrorReport] ORDER BY MessageID DESC";
            SqlDataAdapter Message = new SqlDataAdapter(ConnectMessage, MessageConnect);
            SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(Message);
            DataSet MessageData = new DataSet();
            Message.Fill(MessageData);
            MessageData_GridView.DataSource = MessageData.Tables[0];
        }

        private void DeleteMessage()
        {
            try
            {             
            SqlConnection DeleteMessage = new SqlConnection(SQL_Source);
            SqlCommand Delete_Message = new SqlCommand();
            Delete_Message.CommandType = System.Data.CommandType.Text;
            Delete_Message.CommandText = "DELETE FROM [dbo].[ErrorReport] WHERE DateTime=@DateTime"; ;
            Delete_Message.Connection = DeleteMessage;
            Delete_Message.Parameters.AddWithValue("@DateTime", DateTime_TextBox.Text);
            DeleteMessage.Open();
            Delete_Message.ExecuteNonQuery();
            DeleteMessage.Close();
            MessageBox.Show("Message Was Successfully Removed");
            }
            catch (SqlException ExceptionValue)
            {
                int ErrorNumber = ExceptionValue.Number;
                MessageBox.Show("Error Removing Message" + "\n" + "Error Code: " + ErrorNumber.ToString());
            }
            ConnectToMessages();
        }

        private void MessageData_GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow Row = MessageData_GridView.Rows[e.RowIndex];
                DateTime_TextBox.Text = Row.Cells[0].Value.ToString();
                Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                BrakePress_TextBox.Text = Row.Cells[2].Value.ToString();
                ItemID_TextBox.Text = Row.Cells[3].Value.ToString();
                JobID_TextBox.Text = Row.Cells[4].Value.ToString();
                Messaging_TextBox.Text = Row.Cells[5].Value.ToString();
                EmployeeName_TextBox.Text = Row.Cells[6].Value.ToString();
                DMPID_Employee_TextBox.Text = Row.Cells[7].Value.ToString();
                MessageID_TextBox.Text = Row.Cells[8].Value.ToString();
            }
        }

        private void MessageData_GridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
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

        private void Exit_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Delete_Button_Click(object sender, EventArgs e)
        {
            DeleteMessage();
        }
    }
}
