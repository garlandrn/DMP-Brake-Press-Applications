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
    public partial class User_Program_Schedule : Form
    {
        User_Program__ControlLogix_System_ owner = null;
        public User_Program_Schedule(User_Program__ControlLogix_System_ owner)
        {
            InitializeComponent();
            this.owner = owner;
            this.ShowInTaskbar = false;
        }

        public static string BrakePressRefresh_SQL = "";
        private string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;";

        private void Schedule_Timer_Tick(object sender, EventArgs e)
        {
            SqlConnection ScheduleConnection = new SqlConnection(SQL_Source);
            SqlDataAdapter ScheduleDataAdapter = new SqlDataAdapter(BrakePressRefresh_SQL, ScheduleConnection);
            SqlCommandBuilder ScheduleCommandBuilder = new SqlCommandBuilder(ScheduleDataAdapter);
            DataSet ScheduleData = new DataSet();
            ScheduleDataAdapter.Fill(ScheduleData);
            ScheduleGridView.DataSource = ScheduleData.Tables[0];
        }

        private void User_Program_Schedule_Load(object sender, EventArgs e)
        {
            Schedule_Timer.Start();
            this.Location = new System.Drawing.Point(820, 80);
        }

        private void HideSchedule_Button_Click(object sender, EventArgs e)
        {
            //ScheduleGridView.Visible = false;
            //this.ScheduleGridView.Location = new System.Drawing.Point(1496, 986);
            //this.ScheduleGridView.Size = new System.Drawing.Size(69, 58);
            //ViewSchedule_Button.Visible = true;
            owner.ViewSchedule_Button.Visible = true;
            //HideSchedule_Button.Visible = false;
            Schedule_Timer.Stop();
            owner.Enabled = true;
            this.Close();
        }
    }
}
