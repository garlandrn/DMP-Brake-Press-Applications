using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

/*
 * Program: DMP Brake Press Application
 * Form: DMPBrakePressLogin
 * Created By: Ryan Garland
 * Last Updated on 3/26/17
 * 
 * Form Sections:
 *  - User Interface
 *  --- Buttons
 *  --- ComboBox
 *  --- CheckBox
 *  --- GridView
 *  --- PictureBox
 *  
 *  - SQL DataBase Methods
 *  - Methods
 *  
 * - Clock 
 * 
 * 
 * 
 */

namespace DMP_Brake_Press_Application
{
    public partial class DMPBrakePressLogin : Form
    {
        public static Form Current;

        public DMPBrakePressLogin()
        {
            InitializeComponent();
            Current = this;
        }

        /********************************************************************************************************************
        * 
        * Global Variables 
        * 
        * 
        * 
        ********************************************************************************************************************/
        /********************************************************************************************************************
        * 
        * Form Load Variables 
        * 
        ********************************************************************************************************************/

        string SQL_Connection = @"Data Source = OHN7009,49172; Initial Catalog = Brake_Press_Data; Integrated Security = True; Connect Timeout = 15;";

        // Clock_Tick();
        private static int ClockHour;
        private static int ClockMinute;
        private static int ClockSecond;

        /********************************************************************************************************************
        * 
        * Variables In Testing Start
        * 
        ********************************************************************************************************************/

        private bool _canUpdate = true;

        private bool _needUpdate = false;

        string[] names = new string[50];

        private DataSet Data_2;

        private int rows_2 = 0;

        List<string> name = new List<string>();

        /********************************************************************************************************************
        * 
        * Variables In Testing End
        * 
        *********************************************************************************************************************
        *********************************************************************************************************************
        * 
        * DMPBrakePressLogin Start
        * 
        ********************************************************************************************************************/

        private void DMPBrakePressLogin_Load(object sender, EventArgs e)
        {
            Clock.Enabled = true;
            BrakePressID();
            LoadEmployeeList();
            LoadEmployeeNames();
            LoadEmployeeNames_2();
            
        }

        /*******************************************************************************************************************
        *  
        *  [User Interface]
        *  
        *  Buttons
        *  
        *  CheckBoxes
        *  
        *  ComboBoxes
        *  
        *  DateTimePicker
        *  
        *  DataGridView
        * 
        ********************************************************************************************************************
        /********************************************************************************************************************
        * 
        * Buttons Region Start 
        * -- Total Buttons: 14
        * 
        * --- Buttons GroupBox Buttons
        * --- Total: 5
        * - Operator Login Click
        * - AdminLogin Click
        * - ReportView Click
        * - JobList Click
        * - CellControl Click
        * 
        * - Exit Click
        * 
        ********************************************************************************************************************/
        #region

        private void Operator_Login_Click(object sender, EventArgs e)
        {
            Operator_Login();
        }

        private void AdminLogin_Button_Click(object sender, EventArgs e)
        {
            AdminLogin();
        }

        private void ReportView_Button_Click(object sender, EventArgs e)
        {
            ReportViewLogin();
        }

        private void JobList_Button_Click(object sender, EventArgs e)
        {
            JobListLogin();
        }

        private void CellControl_Button_Click(object sender, EventArgs e)
        {
            CellControlLogin();
        }

        private void Exit_Button_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        /********************************************************************************************************************
        * 
        * Buttons Region End
        * 
        ********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        * 
        * Methods Region Start
        * -- Total: 9
        * 
        * - LoadEmployeeList
        * - OperatorLogin
        * - AdminLogin
        * - ReportViewLogin
        * - JobListLogin
        * - CellControlLogin
        * - BrakePressID
        * - OpenNewForm
        * 
        ********************************************************************************************************************/
        #region

        public void LoadEmployeeList()
        {
            SqlConnection connection = new SqlConnection(SQL_Connection);
            string BP1176 = "SELECT * FROM [dbo].[Employee] ORDER BY EmployeeName ASC";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            DataSet Data = new DataSet();
            dataAdapter.Fill(Data);
            LoginGridView.DataSource = Data.Tables[0];
        }

        public void LoadEmployeeNames()
        {
            int rows = 0;
            string BP1176Count = "SELECT COUNT(*) FROM [dbo].[Employee]";
            SqlConnection count = new SqlConnection(SQL_Connection);
            SqlCommand countRows = new SqlCommand(BP1176Count, count);
            count.Open();
            rows = (int)countRows.ExecuteScalar();
            rows_2 = (int)countRows.ExecuteScalar();
            count.Close();

            foreach (DataGridViewRow row in LoginGridView.Rows)
            {
                if (row.Index < rows)
                {
                    EmployeeName_ComboBox.Items.Add(row.Cells[1].Value.ToString());
                }
            }
        }

        public void LoadEmployeeNames_2()
        {
            SqlConnection connection = new SqlConnection(SQL_Connection);
            string BP1176 = "SELECT * FROM [dbo].[Employee]";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            Data_2 = new DataSet();
            dataAdapter.Fill(Data_2);
            LoginGridView.DataSource = Data_2.Tables[0];

            foreach (DataGridViewRow row2 in LoginGridView.Rows)
            {
                if (row2.Index < rows_2)
                {
                    name.Add(row2.Cells[1].Value.ToString());
                }
            }
        }

        private void Operator_Login()
        {
            if (EmployeeName_TextBox.Text == "" && DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Please Enter Your Name and DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter Employee Name to Login");
            }
            else if (DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter DMP ID to Login");
            }
            else if(EmployeeName_TextBox.Text != "" && DMPID_TextBox.Text != "")
            {
                try
                {
                    SqlConnection OperatorLogin = new SqlConnection(SQL_Connection);
                    SqlCommand Login_Command = new SqlCommand("Select * from [dbo].[Employee] where EmployeeName=@EmployeeName and DMPID=@DMPID", OperatorLogin);
                    Login_Command.Parameters.AddWithValue("@EmployeeName", EmployeeName_TextBox.Text);
                    Login_Command.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
                    OperatorLogin.Open();
                    SqlDataAdapter adapt = new SqlDataAdapter(Login_Command);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    LoginGridView.DataSource = ds.Tables[0];
                    OperatorLogin.Close();
                    int count = ds.Tables[0].Rows.Count;
                    if (count == 1)
                    {
                        User_Program__ControlLogix_System_ UserProgram = new User_Program__ControlLogix_System_();
                        UserProgram.User_TextBox.Text = EmployeeName_TextBox.Text;
                        UserProgram.Clock_TextBox.Text = Clock_TextBox.Text;
                        UserProgram.DMPID_TextBox.Text = DMPID_TextBox.Text;
                        DMPID_TextBox.Clear();
                        EmployeeName_TextBox.Clear();
                        EmployeeName_ComboBox.Text = "";
                        ListBox.Items.Clear();
                        UserProgram.Show();
                        UserProgram.Focus();
                        OpenNewForm();
                    }
                    else
                    {
                        ListBox.Items.Add("Please Check DMP ID Entered");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AdminLogin()
        {
            if (EmployeeName_TextBox.Text == "" && DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Please Enter Your Name and DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter Employee Name to Login");
            }
            else if (DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text != "" && DMPID_TextBox.Text != "")
            {
                try
                {
                    SqlConnection AdminConnection = new SqlConnection(SQL_Connection);
                    SqlCommand SQL_Admin = new SqlCommand("Select * from [dbo].[Admin] where EmployeeName=@EmployeeName and EmployeePassword=@EmployeePassword", AdminConnection);
                    SQL_Admin.Parameters.AddWithValue("@EmployeeName", EmployeeName_TextBox.Text);
                    SQL_Admin.Parameters.AddWithValue("@EmployeePassword", DMPID_TextBox.Text);
                    AdminConnection.Open();
                    SqlDataAdapter adapt = new SqlDataAdapter(SQL_Admin);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    AdminConnection.Close();
                    int count = ds.Tables[0].Rows.Count;
                    if (count == 1)
                    {
                        ListBox.Items.Clear();
                        AdminAccess Admin = new AdminAccess();
                        Admin.User_TextBox.Text = EmployeeName_TextBox.Text;
                        Admin.Clock_TextBox.Text = Clock_TextBox.Text;
                        Admin.UserNumber_TextBox.Text = DMPID_TextBox.Text;
                        Admin.Show();
                        Admin.Focus();
                        OpenNewForm();
                    }
                    else
                    {
                        ListBox.Items.Add("Access to Admin Form Denied");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ReportViewLogin()
        {
            if (EmployeeName_TextBox.Text == "" && DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Please Enter Your Name and DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter Employee Name to Login");
            }
            else if (DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text != "" && DMPID_TextBox.Text != "")
            {
                try
                {
                    SqlConnection ReportLogin = new SqlConnection(SQL_Connection);
                    SqlCommand Login_Command = new SqlCommand("Select * from [dbo].[Employee] where EmployeeName=@EmployeeName and DMPID=@DMPID", ReportLogin);
                    Login_Command.Parameters.AddWithValue("@EmployeeName", EmployeeName_TextBox.Text);
                    Login_Command.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
                    ReportLogin.Open();
                    SqlDataAdapter adapt = new SqlDataAdapter(Login_Command);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    LoginGridView.DataSource = ds.Tables[0];
                    ReportLogin.Close();
                    int count = ds.Tables[0].Rows.Count;
                    if (count == 1)
                    {
                        ReportViewer ReportView = new ReportViewer();
                        ReportView.User_TextBox.Text = EmployeeName_TextBox.Text;
                        ReportView.Clock_TextBox.Text = Clock_TextBox.Text;
                        ReportView.DMPID_TextBox.Text = DMPID_TextBox.Text;
                        ListBox.Items.Clear();
                        ReportView.Show();
                        ReportView.Focus();
                        OpenNewForm();
                    }
                    else
                    {
                        ListBox.Items.Add("Access to Report View Denied.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }

        private void JobListLogin()
        {
            if (EmployeeName_TextBox.Text == "" && DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Please Enter Your Name and DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter Employee Name to Login");
            }
            else if (DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text != "" && DMPID_TextBox.Text != "")
            {
                try
                {
                    SqlConnection JobListConnection = new SqlConnection(SQL_Connection);
                    SqlCommand SQL_JobList = new SqlCommand("Select * from [dbo].[Admin] where EmployeeName=@EmployeeName and EmployeePassword=@EmployeePassword", JobListConnection);
                    SQL_JobList.Parameters.AddWithValue("@EmployeeName", EmployeeName_TextBox.Text);
                    SQL_JobList.Parameters.AddWithValue("@EmployeePassword", DMPID_TextBox.Text);
                    JobListConnection.Open();
                    SqlDataAdapter adapt = new SqlDataAdapter(SQL_JobList);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    JobListConnection.Close();
                    int count = ds.Tables[0].Rows.Count;
                    if (count == 1)
                    {
                        JobList jobList = new JobList();
                        jobList.User_TextBox.Text = EmployeeName_TextBox.Text;
                        jobList.Clock_TextBox.Text = Clock_TextBox.Text;
                        jobList.DMPID_TextBox.Text = DMPID_TextBox.Text;
                        ListBox.Items.Clear();
                        jobList.Show();
                        jobList.Focus();
                        OpenNewForm();
                    }
                    else
                    {
                        ListBox.Items.Add("Access to Job List Denied");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }

        private void CellControlLogin()
        {
            if (EmployeeName_TextBox.Text == "" && DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Please Enter Your Name and DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter Employee Name to Login");
            }
            else if (DMPID_TextBox.Text == "")
            {
                ListBox.Items.Add("Enter DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text != "" && DMPID_TextBox.Text != "")
            {
                try
                {
                    SqlConnection CellManagerConnection = new SqlConnection(SQL_Connection);
                    SqlCommand SQL_CellManager = new SqlCommand("Select * from [dbo].[Admin] where EmployeeName=@EmployeeName and EmployeePassword=@EmployeePassword", CellManagerConnection);
                    SQL_CellManager.Parameters.AddWithValue("@EmployeeName", EmployeeName_TextBox.Text);
                    SQL_CellManager.Parameters.AddWithValue("@EmployeePassword", DMPID_TextBox.Text);
                    CellManagerConnection.Open();
                    SqlDataAdapter adapt = new SqlDataAdapter(SQL_CellManager);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    CellManagerConnection.Close();
                    int count = ds.Tables[0].Rows.Count;
                    if (count == 1)
                    {
                        Cell_Control CellControl = new Cell_Control();
                        CellControl.User_TextBox.Text = EmployeeName_TextBox.Text;
                        CellControl.Clock_TextBox.Text = Clock_TextBox.Text;
                        CellControl.DMPID_TextBox.Text = DMPID_TextBox.Text;
                        ListBox.Items.Clear();
                        CellControl.Show();
                        CellControl.Focus();
                        OpenNewForm();
                    }
                    else
                    {
                        ListBox.Items.Add("Access to Cell Control Denied");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BrakePressID()
        {
            string SpotWeldComputerID = System.Environment.MachineName;
            if (SpotWeldComputerID == "OHN7047NL")
            {
                //EmployeeName_ComboBox.Text = "Ryan Garland";
                EmployeeName_TextBox.Text = "Ryan Garland";
                DMPID_TextBox.Text = "10078";
                AdminButtons_GroupBox.Visible = true;
                TestFormsGroupBox.Visible = true;
            }
            if (SpotWeldComputerID == "OHN7125NL")
            {
                EmployeeName_ComboBox.Text = "Dale Worline";
                EmployeeName_TextBox.Text = "Dale Worline";
                DMPID_TextBox.Text = "4418";
                AdminButtons_GroupBox.Visible = true;
                TestFormsGroupBox.Visible = true;
            }
            if (SpotWeldComputerID == "OHN7070NL")
            {
                EmployeeName_ComboBox.Text = "Sean Pleiman";
                EmployeeName_TextBox.Text = "Sean Pleiman";
                DMPID_TextBox.Text = "8064";
                AdminButtons_GroupBox.Visible = true;
                TestFormsGroupBox.Visible = true;
            }
        }

        private void OpenNewForm()
        {
            Current.WindowState = FormWindowState.Minimized;
            Current.Enabled = false;
            Current.ShowInTaskbar = false;
        }

        /********************************************************************************************************************
        * 
        * Methods Region End
        * 
        ********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Events Region Start
        * -- Total: 4
        * 
        * - Clock Tick
        * - Login FormClosing
        * - EmployeeName ComboBox SelectedIndexChanged
        * - DMPID TextBox KeyDown
        * - EmployeeName ComboBox DropDown
        * 
        *********************************************************************************************************************/
        #region

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
            else if (ClockHour >= 10 && ClockHour <= 11)
            {
                Time += ClockHour;
                AMPM = "AM";
            }
            else if (ClockHour < 10)
            {
                Time += "0" + ClockHour;
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

        private void DMPBrakePressLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(System.Environment.MachineName != "OHN7047NL")
            {
                DMPBrakePressLogoff Logoff = new DMPBrakePressLogoff();

                if (Logoff.ShowDialog() == DialogResult.Yes && Logoff.Password_TextBox.Text == "DMPBP18")
                {

                }
                else
                {
                    e.Cancel = true;
                    ListBox.Items.Add("Unable to Close Application at " + Clock_TextBox.Text);
                    SendMessage();
                }
            }
        }

        private void SendAppMessage()
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
                objeto_mail.From = new MailAddress("BPLogoff@defiancemetal.com");
                objeto_mail.To.Add(new MailAddress("rgarland@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("spleiman@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("dworline@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("ngill@defiancemetal.com"));
                objeto_mail.Subject = "Camera Closed Attempt";
                objeto_mail.Body = "Camera Closed Attempt";
                client.Send(objeto_mail);
            }
            catch (Exception ex)
            {
                ListBox.Items.Add("Unable to Notify");
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
                objeto_mail.From = new MailAddress("BPLogoff@defiancemetal.com");
                objeto_mail.To.Add(new MailAddress("rgarland@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("spleiman@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("dworline@defiancemetal.com"));
                //objeto_mail.To.Add(new MailAddress("ngill@defiancemetal.com"));
                objeto_mail.Subject = "Brake Press Logoff Attempt";
                objeto_mail.Body = "Brake Press Logoff Attempt";
                client.Send(objeto_mail);
            }
            catch (Exception ex)
            {
                ListBox.Items.Add("Unable to Notify");
            }
        }

        private void Login_FormClosing(object sender, EventArgs e)
        {
            //Application.Exit();
            //this.Close();
        }

        private void EmployeeName_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DMPID_TextBox.Clear();
            if (EmployeeName_ComboBox.Text == "Ryan Garland" || EmployeeName_ComboBox.Text == "Dale Worline" || EmployeeName_ComboBox.Text == "Sean Pleiman" || EmployeeName_ComboBox.Text == "Nick Gill")
            {
                AdminButtons_GroupBox.Visible = true;
                TestFormsGroupBox.Visible = true;
            }
            else if (EmployeeName_ComboBox.Text != "Ryan Garland" || EmployeeName_ComboBox.Text != "Dale Worline" || EmployeeName_ComboBox.Text != "Sean Pleiman" || EmployeeName_ComboBox.Text != "Nick Gill")
            {
                AdminButtons_GroupBox.Visible = false;
                TestFormsGroupBox.Visible = false;
            }
            EmployeeName_TextBox.Text = EmployeeName_ComboBox.Text;
            _needUpdate = false;
        }

        private void DMPID_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && DMPID_TextBox.Focus() == true)
            {
                Operator_Login_Click(null, null);
            }
        }

        private void EmployeeName_ComboBox_DropDown(object sender, EventArgs e)
        {
            this.EmployeeName_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F);
            EmployeeName_ComboBox.Items.Clear();
            LoadEmployeeList();
            LoadEmployeeNames();
        }

        private void EmployeeName_ComboBox_MouseLeave(object sender, EventArgs e)
        {
            this.EmployeeName_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F);
        }

        /*********************************************************************************************************************
        * 
        * Events Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        * 
        * Testing Region Start
        * 
        ********************************************************************************************************************/
        #region

        private void OperatorLogin_Button_Click(object sender, EventArgs e)
        {
            OperatorLogin();
        }

        private void OperatorLogin()
        {
            if (EmployeeName_TextBox.Text == null && DMPID_TextBox.Text == null)
            {
                ListBox.Items.Add("Please Enter Your Name and DMP ID to Login");
            }
            else if (EmployeeName_TextBox.Text == null)
            {
                ListBox.Items.Add("Enter Employee Name to Login");
            }
            else if (DMPID_TextBox.Text == null)
            {
                ListBox.Items.Add("Enter DMP ID to Login");
            }
            try
            {
                SqlConnection OperatorLogin = new SqlConnection(SQL_Connection);
                SqlCommand Login_Command = new SqlCommand("Select * from [dbo].[Employee] where EmployeeName=@EmployeeName and DMPID=@DMPID", OperatorLogin);
                Login_Command.Parameters.AddWithValue("@EmployeeName", EmployeeName_TextBox.Text);
                Login_Command.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
                OperatorLogin.Open();
                SqlDataAdapter adapt = new SqlDataAdapter(Login_Command);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                LoginGridView.DataSource = ds.Tables[0];
                OperatorLogin.Close();
                int count = ds.Tables[0].Rows.Count;
                if (count == 1)
                {
                    User_Program UserProgram = new User_Program();
                    UserProgram.User_TextBox.Text = EmployeeName_TextBox.Text;
                    UserProgram.Clock_TextBox.Text = Clock_TextBox.Text;
                    //UserProgram.UserNumber_TextBox = DMPID_TextBox.Text;
                    UserProgram.User_TextBox.ReadOnly = true;
                    UserProgram.Clock_TextBox.ReadOnly = true;
                    //this.Hide();
                    DMPID_TextBox.Clear();
                    ListBox.Items.Clear();
                    foreach (DataGridViewRow number in LoginGridView.Rows)
                    {
                        UserProgram.DMPID_TextBox.Text = number.Cells[2].Value.ToString();

                        break;
                    }
                    ListBox.Items.Add("Login Successful");

                    UserProgram.Show();

                    UserProgram.Focus();

                    OpenNewForm();


                }

                else
                {
                    ListBox.Items.Add("Log In Failed. Employee Not Found.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Test_Button_Click(object sender, EventArgs e)
        {
            //User_Program_Scan_Out t = new User_Program_Scan_Out();
            //t.Show();
            User_Program_Sign_Off so = new User_Program_Sign_Off();
            so.Show();
        }
        
        private void CheckCardData_Button_Click(object sender, EventArgs e)
        {
            Check_Card_Data_Enter CCD = new Check_Card_Data_Enter();
            CCD.Show();
            CCD.DateTime_TextBox.Text = Clock_TextBox.Text;
            CCD.ItemID_TextBox.Text = "999-99999";
            CCD.Sequence_TextBox.Text = "1";
            CCD.OperatorName_TextBox.Text = "Ryan Garland";
            CCD.OperatorName_TextBox.Text = "Ryan Garland";
            CCD.Customer_TextBox.Text = "Paccar";
            CCD.CustomerPartNumber_TextBox.Text = "N10-1110-25-102R";
            CCD.GageNumber_TextBox.Text = "100689";
            CCD.BaleNumber_TextBox.Text = "123456";
            CCD.LotNumber_TextBox.Text = "888689";
            CCD.OperatorID_TextBox.Text = "10078";
            CCD.BuddyCheckID_TextBox.Text = "4418";
            CCD.A_ComboBox.Text = "Pass";
            CCD.Code_ComboBox.Text = "1";    
            CCD.B_TextBox.Text = "-.014";
            CCD.C_TextBox.Text = "-.011";
            CCD.D_TextBox.Text = "-.030";
            CCD.E_TextBox.Text = "-.125";
            CCD.F_TextBox.Text = "-.125";
            CCD.G_TextBox.Text = "-.125";
            CCD.H_TextBox.Text = "-.125";
            CCD.I_TextBox.Text = "-.125";
        }

        private void ViewCheckCard_Button_Click(object sender, EventArgs e)
        {
            Check_Card_Data_Viewer CheckCardViewer = new Check_Card_Data_Viewer();
            CheckCardViewer.ItemIDSearch_TextBox.Text = "999-99999";
            CheckCardViewer.Clock_TextBox.Text = Clock_TextBox.Text;
            CheckCardViewer.User_TextBox.Text = "Ryan Garland";
            CheckCardViewer.Show();
        }

        private void ScanOut_Button_Click(object sender, EventArgs e)
        {
            User_Program_Scan_Out s = new User_Program_Scan_Out();
            s.Show();
            s.EmployeeNumber_TextBox.Text = "10078";
        }

        private void BrakePressData_Button_Click(object sender, EventArgs e)
        {
            Test t = new Test();
            t.Show();
        }

        private void CellOverview_Button_Click(object sender, EventArgs e)
        {
            Cell_Overview CO = new Cell_Overview();
            CO.Show();
        }

        private void MessageView_Button_Click(object sender, EventArgs e)
        {
            MessagingLogin();
        }
        
        private void MessagingLogin()
        {

            if (EmployeeName_TextBox.Text == null && DMPID_TextBox.Text == null)
            {
                ListBox.Items.Add("Please Enter Your Name and Employee ID to Login.");
            }
            else if (EmployeeName_TextBox.Text == null)
            {
                ListBox.Items.Add("Enter Employee Name to Login");
            }
            else if (DMPID_TextBox.Text == null)
            {
                ListBox.Items.Add("Enter DMP ID to Login");
            }
            try
            {
                SqlConnection AdminConnection = new SqlConnection(SQL_Connection);
                SqlCommand SQL_Admin = new SqlCommand("Select * from [dbo].[Admin] where EmployeeName=@EmployeeName and EmployeePassword=@EmployeePassword", AdminConnection);
                SQL_Admin.Parameters.AddWithValue("@EmployeeName", EmployeeName_TextBox.Text);
                SQL_Admin.Parameters.AddWithValue("@EmployeePassword", DMPID_TextBox.Text);
                AdminConnection.Open();
                SqlDataAdapter adapt = new SqlDataAdapter(SQL_Admin);
                DataSet ds = new DataSet();
                adapt.Fill(ds);


                AdminConnection.Close();
                int count = ds.Tables[0].Rows.Count;
                if (count == 1)
                {

                    ListBox.Items.Add("Login Successful!");
                    // EmployeeName_Text.Clear();
                    //DMPID_TextBox.Clear();
                    ListBox.Items.Clear();
                    MessageView ViewMessage = new MessageView();
                    ViewMessage.User_TextBox.Text = EmployeeName_TextBox.Text;
                    ViewMessage.Clock_TextBox.Text = Clock_TextBox.Text;
                    ViewMessage.User_TextBox.Text = DMPID_TextBox.Text;
                    ViewMessage.User_TextBox.ReadOnly = true;
                    ViewMessage.Clock_TextBox.ReadOnly = true;
                    ViewMessage.Show();
                    ViewMessage.Show();
                    OpenNewForm();

                }
                else
                {
                    ListBox.Items.Add("Access to Admin Form Denied");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /********************************************************************************************************************
        * 
        * Testing Region Start
        * 
        ********************************************************************************************************************/
        #endregion

        private void SQL_Import_Button_Click(object sender, EventArgs e)
        {
            SQLImportTest test = new SQLImportTest();
            test.Show();
        }

        private void EmployeeName_ComboBox_Leave(object sender, EventArgs e)
        {

        }

        private void EmployeeName_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            this.EmployeeName_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F);
        }

        private void Camera_Timer_Tick(object sender, EventArgs e)
        {
            string path = @"C:\Program Files\Teledyne DALSA\iNspect Express x64\iworks.exe";
            string fileName = Path.GetFileName(path);
            // Get the precess that already running as per the exe file name.
            Process[] processName = Process.GetProcessesByName(fileName.Substring(0, fileName.LastIndexOf('.')));
            if (processName.Length > 0)
            {
            }
            else if(System.Environment.MachineName != "OHN7047NL")
            {
                Camera_Timer.Stop();
                SendAppMessage();
                ListBox.Items.Add("iNspect Express Closed at " + Clock_TextBox.Text);
            }
        }
    }
}


