using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;


/*
 * Program: DMP Brake Press Application
 * Form: ReportViewer
 * Created By: Ryan Garland
 * Last Updated on 4/25/17
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
    public partial class ReportViewer : Form
    {
        public ReportViewer()
        {
            InitializeComponent();
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

        private string LoginForm = "Report Viewer";
        private string LoginTime = "";
        //private string[] BrakePressID = { "1065", "1083", "1107", "1108", "1127", "1155", "1158", "1175", "1176", "1178" };
        private string[] BrakePressID = {"1083", "1107", "1127", "1155", "1158", "1175"};
        string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;";
                
        // Clock_Tick();
        private static int ClockHour;
        private static int ClockMinute;
        private static int ClockSecond;
        
        // Search_Button_Click()
        private static string SearchCommand;
        private static string ReportItemID;
        private static string ReportBrakePress;
        private static string ReportEmployee;
        private static string ReportDate;

        // Excel File Creation
        private static Excel._Workbook ReportWB;
        private static Excel.Application ReportApp;
        private static Excel._Worksheet ReportWS;
        private static Excel.Range ReportRange;
        private static string ExcelFileLocation;
        private DataSet ReportDataSet;

        // PDF FileLocation
        private static string PDFFileLocation;


        /********************************************************************************************************************
        * 
        * Variables In Testing Start
        * 
        ********************************************************************************************************************/

        List<String> list = new List<String>();
        string[] names;

        private static string[] itemID;
        private static int[] itemCount;

        /********************************************************************************************************************
        * 
        * Variables In Testing End
        * 
        *********************************************************************************************************************
        *********************************************************************************************************************
        * 
        * ReportViewer Start
        * 
        ********************************************************************************************************************/

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            SqlConnection ReportLogin = new SqlConnection(SQL_Source);
            SqlCommand Login = new SqlCommand();
            Login.CommandType = System.Data.CommandType.Text;
            Login.CommandText = "INSERT INTO [dbo].[LoginData] (EmployeeName,DMPID,LoginDateTime,LoginForm) VALUES (@EmployeeName,@DMPID,@LoginDateTime,@LoginForm)";
            Login.Connection = ReportLogin;
            Login.Parameters.AddWithValue("@LoginDateTime", Clock_TextBox.Text);
            Login.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            Login.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            Login.Parameters.AddWithValue("@LoginForm", LoginForm.ToString());
            ReportLogin.Open();
            Login.ExecuteNonQuery();
            ReportLogin.Close();

            Clock.Enabled = true;
            LoginTime = Clock_TextBox.Text;
            SqlConnection connection = new SqlConnection(SQL_Source);
            string EmployeeReport = "SELECT * FROM [dbo].[Employee]";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(EmployeeReport, connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            DataSet Data = new DataSet();
            dataAdapter.Fill(Data);
            LoginGridView.DataSource = Data.Tables[0];

            int rows = 0;
            string EmployeeCount = "SELECT COUNT(*) FROM [dbo].[Employee]";
            SqlConnection count = new SqlConnection(SQL_Source);
            SqlCommand countRows = new SqlCommand(EmployeeCount, count);
            count.Open();
            rows = (int)countRows.ExecuteScalar();
            count.Close();
            foreach (DataGridViewRow row in LoginGridView.Rows)
            {
                if (row.Index < rows)
                {
                    DMPID_ComboBox.Items.Add(row.Cells[0].Value.ToString());
                }
            }

            BrakePress_ComboBox.Items.AddRange(BrakePressID);

            //chart1.ChartAreas[0].AxisX.Maximum = 100;
            //chart1.ChartAreas[0].AxisX.Minimum = 1;

        }

        // Buttons Region
        #region

        /********************************************************************************************************************
        * [Buttons]
        * 
        * -------------------------------------------------------[Clear]----------------------------------------------------
        * --Method:
        *   Clear();
        *  
        * ------------------------------------------------------[Create]----------------------------------------------------
        * --
        * 
        * -------------------------------------------------------[Search]---------------------------------------------------
        * --Method:
        *   CommandCreator();
        *   
        * -------------------------------------------------------[LogOff]-------------_-------------------------------------
        * --SQL Method:
        *   EmployeeLogoff();
        *    
        ********************************************************************************************************************/

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Create_Button_Click(object sender, EventArgs e)
        {
            if(SearchCommand == null)
            {
                MessageBox.Show("Please Create a DataTable Before Creating a PDF");
            }
            else
            {
                PDFFileCreate();
            }
        }

        private void CreateExcel_Button_Click(object sender, EventArgs e)
        {
            if (SearchCommand == null)
            {
                MessageBox.Show("Please Create a DataTable Before Creating an Excel File");
            }
            else
            {
                CreateExcelFile();
            }
        }

        private void Search_Button_Click(object sender, EventArgs e)
        {
            CommandCreator();
        }
        
        private void LogOff_Button_Click(object sender, EventArgs e)
        {
            EmployeeLogOff();
            DMPBrakePressLogin.Current.Focus();
            DMPBrakePressLogin.Current.Enabled = true;
            DMPBrakePressLogin.Current.WindowState = FormWindowState.Maximized;
            DMPBrakePressLogin.Current.ShowInTaskbar = true;
            this.Close();
        }

        /********************************************************************************************************************
        * 
        * Buttons End
        * 
        ********************************************************************************************************************/
        #endregion // Buttons Region

        // Methods Region
        #region

        /********************************************************************************************************************
        *  (Methods)
        *  
        * -------------------------------------------------------(Clear)------------------------------------------------------
        *  --Method Variables:
        *    BrakePress_TextBox.Clear();
        *    BrakePress_ComboBox.Text = "";
        *    DateStartPicker.Checked = false;
        *    DateStartPicker.Font;
        *    DateStartPicker.ResetText();
        *    DateEndPicker.Font;
        *    DateEndPicker.ResetText();
        *    DMPID_ComboBox.Text = "";
        *    ItemID_TextBox.Clear();
        *    OperationID_TextBox.Clear();
        *    ReportGridView.DataSource = null;
        *    SearchDMPID_TextBox.Clear();
        *    
        *  -------------------------------------------------(CommandCreator)--------------------------------------------------
        *  --Global Variables:
        *    SearchCommand;
        *    ReportItemID;
        *    ReportBrakePress;
        *    ReportEmployee;
        *    ReportDate;
        *    
        *  --Method:   
        *    CreateReport();
        *            
        ********************************************************************************************************************/

        private void Clear()
        {
            BrakePress_TextBox.Clear();
            BrakePress_ComboBox.Text = "";
            DateStartPicker.Checked = false;
            DateStartPicker.Size = new System.Drawing.Size(323, 30);
            DateStartPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DateStartPicker.ResetText();
            DateEndPicker.Size = new System.Drawing.Size(323, 30);
            DateEndPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DateEndPicker.ResetText();
            DMPID_ComboBox.Text = "";
            ItemID_TextBox.Clear();
            OperationID_TextBox.Clear();
            ReportGridView.DataSource = null;
            SearchDMPID_TextBox.Clear();
        }

        private void CommandCreator()
        {
            // #1
            // Item ID: Yes
            // Brake Press: Yes
            // DMP: Yes
            // Date: Yes
            if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // #2
            // Item ID: Yes
            // Brake Press: Yes
            // DMP: Yes
            // Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
            // #3
            // Item ID: Yes
            // Brake Press: Yes
            // DMP: No
            // Date: Yes
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.ItemID='" + ItemID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // #4
            // Item ID: Yes
            // Brake Press: Yes
            // DMP: No
            // Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }
            // #5
            // Item ID: Yes
            // Brake Press: No
            // DMP: Yes
            // Date: Yes
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // #6
            // Item ID: Yes
            // Brake Press: No
            // DMP: Yes
            // Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
            // #7
            // Item ID: No
            // Brake Press: Yes
            // DMP: Yes
            // Date: Yes
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // #8
            // Item ID: No
            // Brake Press: Yes
            // DMP: Yes
            // Date: No
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
            // #9
            // Item ID: No
            // Brake Press: Yes
            // DMP: No
            // Date: Yes
            if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // #10
            // Item ID: No
            // Brake Press: Yes
            // DMP: No
            // Date: No
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }
            // #11
            // Item ID: Yes
            // Brake Press: No
            // DMP: No
            // Date: Yes
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // #12
            // Item ID: Yes
            // Brake Press: No
            // DMP: No
            // Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                //SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[ItemOperationData] as D INNER JOIN [dbo].[OperationOEE] as O ON D.OperationID = O.OperationID WHERE D.ItemID='" + ItemID_TextBox.Text + "' AND D.DMPID='" + SearchDMPID_TextBox.Text + "'";
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }
            // #13
            // Item ID: No
            // Brake Press: No
            // DMP: Yes
            // Date: Yes
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // #14
            // Item ID: No
            // Brake Press: No
            // DMP: Yes
            // Date: No
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
            // #15
            // Item ID: No
            // Brake Press: No
            // DMP: No
            // Date: Yes
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                //SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, D.StartDateTime as StartDateTime, D.EndDateTime as EndDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE D.StartDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Value.ToShortDateString() + " - " + DateEndPicker.Value.ToShortDateString();
                CreateReport();
            }
            // Last
            // Excecutes When Item ID, Operation ID, DMP ID, and DateStartPicker are all Empty
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                MessageBox.Show("Please Select a Date, DMP ID, or Item Number to Search Data");
            }
        }

        private void CreateReport()
        {
            try
            {
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string Paccar1176 = SearchCommand;
                SqlDataAdapter Data1176 = new SqlDataAdapter(Paccar1176, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(Data1176);
                ReportDataSet = new DataSet();
                Data1176.Fill(ReportDataSet);
                ReportGridView.DataSource = ReportDataSet.Tables[0];
            }
            catch (SqlException ExceptionValue)
            {
                int ErrorNumber = ExceptionValue.Number;
                if (ErrorNumber.Equals(2627))
                {
                    MessageBox.Show("This DMP ID Belongs to Another User");
                }
                else if (ErrorNumber.Equals(245))
                {
                    MessageBox.Show("DMP ID Can Only Contain Numbers");
                }
                else
                {
                    MessageBox.Show("Unable to Add User. Please Try Again." + "\n" + "Error Code: " + ErrorNumber.ToString());
                }
            }
        }

        private void PDFFileCreate()
        {
            // New PDF Document
            PdfDocument BrakePressReport = new PdfDocument();
            PdfPage ReportPage = BrakePressReport.AddPage();
            ReportPage.Size = PdfSharp.PageSize.Letter;
            ReportPage.Orientation = PdfSharp.PageOrientation.Landscape;
            ReportPage.Rotate = 0;
            XGraphics ReportGraph = XGraphics.FromPdfPage(ReportPage);

            // Fonts
            XFont ReportDataHeader = new XFont("Verdana", 12, XFontStyle.Bold);
            XFont ColumnHeader = new XFont("Verdana", 10, XFontStyle.Bold | XFontStyle.Underline);
            XFont RowFont = new XFont("Verdana", 8, XFontStyle.Regular);
            XFont PageFooterFont = new XFont("Verdana", 8, XFontStyle.Regular);

            int PointY = 0;
            int CurrentRow = 0;

            // PDF Report Name

            string ReportName = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Millisecond;
            ReportName = ReportName.Replace("/", "_");
            ReportName = ReportName.Replace(":", "_");
            string ReportFooter = " | Report Created On: " + DateTime.Now.ToShortDateString() + " | Created By: " + User_TextBox.Text;

            // PDF Header First Page Only

            ReportGraph.DrawImage(XImage.FromFile(@"\\OHN66FS01\BPprogs\Brake Press Vision\Applications\DMPLogo700.jpg"), 35, 5);
            ReportGraph.DrawString(ReportItemID, ReportDataHeader, XBrushes.Black, new XRect(400, 15, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString(ReportBrakePress, ReportDataHeader, XBrushes.Black, new XRect(400, 33, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString(ReportEmployee, ReportDataHeader, XBrushes.Black, new XRect(400, 51, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString(ReportDate, ReportDataHeader, XBrushes.Black, new XRect(400, 69, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            PointY = PointY + 100;

            // Column Headers

            ReportGraph.DrawString("Item ID", ColumnHeader, XBrushes.Black, new XRect(10, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("Operation", ColumnHeader, XBrushes.Black, new XRect(80, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("Date", ColumnHeader, XBrushes.Black, new XRect(170, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("Operation", ColumnHeader, XBrushes.Black, new XRect(240, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("Planned", ColumnHeader, XBrushes.Black, new XRect(310, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("Efficiency", ColumnHeader, XBrushes.Black, new XRect(380, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("Brake Press", ColumnHeader, XBrushes.Black, new XRect(450, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("PPM", ColumnHeader, XBrushes.Black, new XRect(550, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("Employee", ColumnHeader, XBrushes.Black, new XRect(600, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
            ReportGraph.DrawString("DMP ID", ColumnHeader, XBrushes.Black, new XRect(700, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);

            // Report Footer 
            string PageNumber = "Page: " + BrakePressReport.PageCount + ReportFooter;
            ReportGraph.DrawString(PageNumber, PageFooterFont, XBrushes.Black, new XRect(5, 585, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);

            PointY = PointY + 25;
            try
            {
                SqlConnection CreatePDF = new SqlConnection(SQL_Source);
                string PDFCommand = SearchCommand;
                SqlDataAdapter PDFAdapter = new SqlDataAdapter(PDFCommand, CreatePDF);
                DataSet PDFData = new DataSet();
                PDFAdapter.Fill(PDFData);

                for (int i = 0; i <= PDFData.Tables[0].Rows.Count - 1; i++)
                {
                    string ItemIDResults = PDFData.Tables[0].Rows[i].ItemArray[0].ToString();
                    string OperationIDResults = PDFData.Tables[0].Rows[i].ItemArray[1].ToString();
                    string RunDateResults = PDFData.Tables[0].Rows[i].ItemArray[2].ToString();
                    string OperationTimeResults = PDFData.Tables[0].Rows[i].ItemArray[5].ToString();
                    string PlannedTimeResults = PDFData.Tables[0].Rows[i].ItemArray[6].ToString();
                    string EfficiencyResults = PDFData.Tables[0].Rows[i].ItemArray[7].ToString();
                    string OEEResults = PDFData.Tables[0].Rows[i].ItemArray[8].ToString();
                    string BrakePressResults = PDFData.Tables[0].Rows[i].ItemArray[9].ToString();
                    string PartsManufacturedResults = PDFData.Tables[0].Rows[i].ItemArray[10].ToString();
                    string PPMResults = PDFData.Tables[0].Rows[i].ItemArray[11].ToString();
                    string EmployeeResults = PDFData.Tables[0].Rows[i].ItemArray[12].ToString();
                    string DMPIDResults = PDFData.Tables[0].Rows[i].ItemArray[13].ToString();

                    RunDateResults = RunDateResults.Replace("12:00:00 AM", "");

                    // Report Row Data

                    ReportGraph.DrawString(ItemIDResults, RowFont, XBrushes.Black, new XRect(10, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(OperationIDResults, RowFont, XBrushes.Black, new XRect(97, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(RunDateResults, RowFont, XBrushes.Black, new XRect(163, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(OperationTimeResults, RowFont, XBrushes.Black, new XRect(248, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(PlannedTimeResults, RowFont, XBrushes.Black, new XRect(315, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(EfficiencyResults, RowFont, XBrushes.Black, new XRect(395, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(BrakePressResults, RowFont, XBrushes.Black, new XRect(470, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(PPMResults, RowFont, XBrushes.Black, new XRect(554, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(EmployeeResults, RowFont, XBrushes.Black, new XRect(600, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    ReportGraph.DrawString(DMPIDResults, RowFont, XBrushes.Black, new XRect(705, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                    PointY = PointY + 20;
                    CurrentRow = CurrentRow + 1;

                    // Report Creates Adds Another Page If Data is Larger than 22 Rows
                    // Only 22 Entries on Page One Due To Report Header
                    if (CurrentRow == 22 && BrakePressReport.PageCount == 1)
                    {
                        PointY = 0;
                        ReportPage = BrakePressReport.AddPage();
                        ReportGraph = XGraphics.FromPdfPage(ReportPage);
                        ReportPage.Size = PdfSharp.PageSize.Letter;
                        ReportPage.Orientation = PdfSharp.PageOrientation.Landscape;
                        ReportPage.Rotate = 0;
                        PointY = PointY + 50;

                        // Column Headers For Second Page

                        ReportGraph.DrawString("Item ID", ColumnHeader, XBrushes.Black, new XRect(10, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Operation", ColumnHeader, XBrushes.Black, new XRect(80, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Date", ColumnHeader, XBrushes.Black, new XRect(170, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Operation", ColumnHeader, XBrushes.Black, new XRect(240, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Planned", ColumnHeader, XBrushes.Black, new XRect(310, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Efficiency", ColumnHeader, XBrushes.Black, new XRect(380, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Brake Press", ColumnHeader, XBrushes.Black, new XRect(450, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("PPM", ColumnHeader, XBrushes.Black, new XRect(550, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Employee", ColumnHeader, XBrushes.Black, new XRect(600, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("DMP ID", ColumnHeader, XBrushes.Black, new XRect(700, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        PageNumber = "Page: " + BrakePressReport.PageCount + ReportFooter;
                        ReportGraph.DrawString(PageNumber, PageFooterFont, XBrushes.Black, new XRect(5, 585, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft); PointY = PointY + 25;
                        CurrentRow = 0;
                    }
                    else if (CurrentRow == 25 && BrakePressReport.PageCount >= 2)
                    {
                        PointY = 0;
                        ReportPage = BrakePressReport.AddPage();
                        ReportGraph = XGraphics.FromPdfPage(ReportPage);
                        ReportPage.Size = PdfSharp.PageSize.Letter;
                        ReportPage.Orientation = PdfSharp.PageOrientation.Landscape;
                        ReportPage.Rotate = 0;
                        PointY = PointY + 50;

                        // Column Headers For Any Page After Two

                        ReportGraph.DrawString("Item ID", ColumnHeader, XBrushes.Black, new XRect(10, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Operation", ColumnHeader, XBrushes.Black, new XRect(80, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Date", ColumnHeader, XBrushes.Black, new XRect(170, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Operation", ColumnHeader, XBrushes.Black, new XRect(240, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Planned", ColumnHeader, XBrushes.Black, new XRect(310, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Efficiency", ColumnHeader, XBrushes.Black, new XRect(380, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Brake Press", ColumnHeader, XBrushes.Black, new XRect(450, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("PPM", ColumnHeader, XBrushes.Black, new XRect(550, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("Employee", ColumnHeader, XBrushes.Black, new XRect(600, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        ReportGraph.DrawString("DMP ID", ColumnHeader, XBrushes.Black, new XRect(700, PointY, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft);
                        PageNumber = "Page: " + BrakePressReport.PageCount + ReportFooter;
                        ReportGraph.DrawString(PageNumber, PageFooterFont, XBrushes.Black, new XRect(5, 585, ReportPage.Width.Point, ReportPage.Height.Point), XStringFormats.TopLeft); PointY = PointY + 25;
                        CurrentRow = 0;
                    }
                }




                //string ReportPDFName = "Brake_Press_Report_" + ReportName + ".pdf";
                //BrakePressReport.Save(ReportPDFName);
                //BrakePressReport.Save(@"C:\Users\rgarland\Desktop\"+ReportPDFName);
                //Process.Start(ReportPDFName);

                string ReportPDFName = "Brake_Press_Report_" + ReportName + ".pdf";
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf|All files (*.*)|*.*";
                saveFile.FileName = ReportPDFName;
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    PDFFileLocation = saveFile.FileName;
                    BrakePressReport.Save(PDFFileLocation);
                    //BrakePressReport.Save(ReportPDFName);
                    //BrakePressReport.Save(@"C:\Users\rgarland\Desktop\"+ReportPDFName);
                    Process.Start(PDFFileLocation);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CreateExcelFile()
        {
            
            
            string ReportName = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Millisecond;
            ReportName = ReportName.Replace("/", "_");
            ReportName = ReportName.Replace(":", "_");

            // Excel Initialize
            ReportApp = new Excel.Application();
            ReportApp.Visible = false;
            ReportWB = (Excel._Workbook)(ReportApp.Workbooks.Add(""));
            ReportWS = (Excel._Worksheet)ReportWB.ActiveSheet;

            /*
                            ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportSpotWelder = "Spotweld: " + Spotweld_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate
                */
            ReportRange = ReportWS.get_Range("A1", "E1");
            ReportRange.get_Range("A1", "E1").Merge();
            ReportRange.get_Range("A2", "E2").Merge();
            ReportRange.get_Range("A3", "E3").Merge();
            ReportRange.get_Range("A4", "E4").Merge();
            ReportWS.Shapes.AddPicture(@"\\OHN66FS01\BPprogs\Brake Press Vision\Applications\DMPLogo700.jpg", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 0, 0, 325, 60);
            //ReportWS.Range["F1", "H1"].Merge();
            string Name = User_TextBox.Text;
            ReportWS.Cells[1, 6] = ReportItemID;
            ReportWS.Cells[2, 6] = ReportBrakePress;
            ReportWS.Cells[3, 6] = ReportEmployee;
            ReportWS.Cells[4, 6] = ReportDate;
            ReportWS.get_Range("F1", "F4").Font.Bold = true;
            ReportWS.get_Range("F1", "F4").Font.Size = 14;
            ReportRange.EntireColumn.AutoFit();

            string[] ColumnNames = new string[ReportGridView.Columns.Count];
            int ExcelColumns = 1;
            foreach (DataGridViewColumn dc in ReportGridView.Columns)
            {
                ReportWS.Cells[5, ExcelColumns] = dc.Name;
                ExcelColumns++;
            }
            ReportWS.get_Range("A5", "N5").Font.Bold = true;
            ReportRange = ReportWS.get_Range("A5", "N5");
            ReportRange.EntireColumn.AutoFit();

            for (int i = 0; i < ReportDataSet.Tables[0].Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < ReportDataSet.Tables[0].Columns.Count; j++)
                {
                    ReportWS.Cells[(i + 6), (j + 1)] = ReportDataSet.Tables[0].Rows[i][j];
                }
            }
            ReportRange = ReportWS.get_Range("A6", "N6");
            ReportRange.EntireColumn.AutoFit();
            string ReportPDFName = "Brake_Press_Report_" + ReportName + ".xls";
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel Files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFile.FileName = ReportPDFName;
            if (saveFile.ShowDialog() != DialogResult.OK)
            {
                ReportWS.Delete();
                ReportWB.Close();
            }
            else
            {
                ExcelFileLocation = saveFile.FileName;
                ReportWS.SaveAs(ExcelFileLocation);
                //ExcelOpen.StartInfo.FileName = ExcelFileLocation;
                //ExcelOpen.Start();
            }
            //ReportWB.Close();

            /*
            ReportWS.SaveAs(@"C:\Users\rgarland\Desktop\ExcelTest.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing);
            */

        }

        private void EmployeeLogOff()
        {
            SqlConnection ReportLogoff = new SqlConnection(SQL_Source);
            SqlCommand Logoff = new SqlCommand();
            Logoff.CommandType = System.Data.CommandType.Text;
            Logoff.CommandText = "UPDATE [dbo].[LoginData] SET LogoutDateTime=@LogoutDateTime WHERE LoginDateTime=@LoginDateTime";
            Logoff.Connection = ReportLogoff;
            Logoff.Parameters.AddWithValue("@LoginDateTime", LoginTime.ToString());
            Logoff.Parameters.AddWithValue("@LogoutDateTime", Clock_TextBox.Text);
            ReportLogoff.Open();
            Logoff.ExecuteNonQuery();
            ReportLogoff.Close();
        }

        /********************************************************************************************************************
        * 
        * Methods End
        * 
        ********************************************************************************************************************/
        #endregion // Methods Region

        // Events Region
        #region
        /********************************************************************************************************************
        * 
        * Events
        * 
        * --------------------------------------(BrakePress_ComboBox_SelectedIndexChanged)------------------------------------
        * 
        * ----------------------------------------(DMPID_ComboBox_SelectedIndexChanged)---------------------------------------
        * 
        * ----------------------------------------------(DateStartPicker_DropDown)--------------------------------------------
        * 
        * -----------------------------------------------(DateEndPicker_DropDown)---------------------------------------------
        * 
        * ----------------------------------------------(ReportGridView_CellClick)--------------------------------------------
        * 
        * -----------------------------------------------------(Clock_Tick)---------------------------------------------------
        * 
        ********************************************************************************************************************/

        private void BrakePress_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrakePress_TextBox.Text = BrakePress_ComboBox.Text;
        }

        private void DMPID_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchDMPID_TextBox.Text = DMPID_ComboBox.Text;
        }

        private void DateStartPicker_DropDown(object sender, EventArgs e)
        {
            DateStartPicker.Checked = true;
            DateStartPicker.Size = new System.Drawing.Size(357, 30);
            DateStartPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void DateEndPicker_DropDown(object sender, EventArgs e)
        {
            DateEndPicker.Checked = true;
            DateEndPicker.Size = new System.Drawing.Size(357, 30);
            DateEndPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void ReportGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow Row = ReportGridView.Rows[e.RowIndex];
                ItemIDResults_TextBox.Text = Row.Cells[0].Value.ToString();
                OperationIDResults_TextBox.Text = Row.Cells[1].Value.ToString();
                RunDateResults_TextBox.Text = Row.Cells[2].Value.ToString();
                StartTime_TextBox.Text = Row.Cells[3].Value.ToString();
                EndTime_TextBox.Text = Row.Cells[4].Value.ToString();
                OperationTimeResults_TextBox.Text = Row.Cells[5].Value.ToString();
                PlannedTimeResults_TextBox.Text = Row.Cells[6].Value.ToString();
                EfficiencyResults_TextBox.Text = Row.Cells[7].Value.ToString();
                OEEResults_TextBox.Text = Row.Cells[8].Value.ToString();
                BrakePressResults_TextBox.Text = Row.Cells[9].Value.ToString();
                PartsManufacturedResults_TextBox.Text = Row.Cells[10].Value.ToString();
                PPMResults_TextBox.Text = Row.Cells[11].Value.ToString();
                EmployeeResults_TextBox.Text = Row.Cells[12].Value.ToString();
                DMPIDResults_TextBox.Text = Row.Cells[13].Value.ToString();
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

        private void chart1_Click(object sender, EventArgs e)
        {
            /*
            int rows = 0;
            string EmployeeCount = "SELECT COUNT(*) FROM [dbo].[Employee]";
            SqlConnection count = new SqlConnection(SQL_Source);
            SqlCommand countRows = new SqlCommand(EmployeeCount, count);
            count.Open();
            rows = (int)countRows.ExecuteScalar();
            count.Close();


            foreach(DataGridViewRow row in ReportGridView.Rows)
            {
                list.Add(row.Cells[0].Value.ToString());
                //names[row.Index].Equals(row.Cells[0].Value.ToString());
            }


            for (int i = 0; i < 6; i++)
            {
                Series series = chart1.Series.Add(list.ToArray().ToString());
                series.Points.Add(rows);
            }
            */

            for (int i = 0; i < ReportDataSet.Tables[0].Rows.Count; i++)
            {

            }

        }

        #endregion //Events Region

        /********************************************************************************************************************
        * 
        * Methods in Testing Start
        * 
        ********************************************************************************************************************/

        /********************************************************************************************************************
        * 
        * Methods in Testing End
        * 
        ********************************************************************************************************************/

    }
}
