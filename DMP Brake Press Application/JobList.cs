using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDataReader;

/*
 * Program: DMP Brake Press Application
 * Form: JobList
 * Created By: Ryan Garland
 * Last Updated on 4/27/18
 *  
 */

namespace DMP_Brake_Press_Application
{
    public partial class JobList : Form
    {
        BackgroundWorker PartImage;
        BackgroundWorker CreateExcel;
        BackgroundWorker CreateExcelSQL_Test;
        BackgroundWorker SaveExcelSQL_Test;
        public JobList()
        {
            InitializeComponent();
            PartImage = new BackgroundWorker();
            PartImage.DoWork += new DoWorkEventHandler(PartImage_RunWorker);
            PartImage.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PartImage_RunWorkerCompleted);
            CreateExcel = new BackgroundWorker();
            CreateExcel.WorkerReportsProgress = true;
            CreateExcel.DoWork += new DoWorkEventHandler(CreateExcelFile);
            CreateExcel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CreateExcelFile_RunWorkerComplete);
            CreateExcel.ProgressChanged += new ProgressChangedEventHandler(CreateExcelFile_ProgressChanged);

            CreateExcelSQL_Test = new BackgroundWorker();
            CreateExcelSQL_Test.WorkerReportsProgress = true;
            CreateExcelSQL_Test.DoWork += new DoWorkEventHandler(CreateExcelFileSQL_Test);
            CreateExcelSQL_Test.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CreateExcelFileSQL_Test_RunWorkerComplete);
            CreateExcelSQL_Test.ProgressChanged += new ProgressChangedEventHandler(CreateExcelFileSQL_Test_ProgressChanged);

            SaveExcelSQL_Test = new BackgroundWorker();
            SaveExcelSQL_Test.WorkerReportsProgress = true;
            SaveExcelSQL_Test.DoWork += new DoWorkEventHandler(SaveExcelSQL_TestRun);
            SaveExcelSQL_Test.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SaveExcelSQL_Test_RunWorkerComplete);
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

        private string LoginTime = "";
        private string LoginForm = "Job List";
        private bool AddJob_ButtonWasClicked = false;
        private bool EditJob_ButtonWasClicked = false;
        private bool RemoveJob_ButtonWasClicked = false;
        string[] Companies = { "CAT", "HINO", "JLG", "John Deere", "Navistar", "Paccar", "" };
        string[] Steps = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        string[] SequenceNumber = { "1", "2", "3", "4" };
        string[] ComboSelections = { "Yes", "No", "Not Feasible", "" };
        string[] BrakePressComboSelections = { "Yes", "No"};

        string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;";

        // Clock_Tick();
        private static int ClockHour;
        private static int ClockMinute;
        private static int ClockSecond;

        // Search_Button_Click();
        private static int SearchColumn;
        private static string SearchValue;


        // FindItemImage();
        private static string ItemImagePath = "";
        private static string ItemID = "";
        private static string[] ItemIDSplit = ItemID.Split('-');
        private static double ItemID_Three;
        private static double ItemID_Five;

        private static string SQLAddCommand = "";
        private static string SQLEditCommand = "";
        private static string SQLRemoveCommand = "";
        private static string Refresh_Data = "";

        // Excel File Creation
        private static Excel._Workbook ReportWB;
        private static Excel.Application ReportApp;
        private static Excel._Worksheet ReportWS;
        private static Excel.Range ReportRange;
        private static string ExcelFileLocation;
        private DataSet ReportDataSet;
        private static string ReportCell = "";
        private static int RowCount;
        private static string RowCountString = "";

        /********************************************************************************************************************
        * 
        * Variables In Testing Start
        * 
        ********************************************************************************************************************/
        private int ImportCurrentRows = 0;
        private int ImportUpdatedRows = 0;

        // Starting Arrays
        private static string[] ItemID_CurrentArray = new string[1000];
        private static string[] JobID_CurrentArray = new string[1000];
        private static string[] Sequence_CurrentArray = new string[1000];

        // Update Arrays
        private static string[] ItemID_UpdatedArray = new string[1000];
        private static string[] JobID_UpdatedArray = new string[1000];
        private static string[] Customer_UpdatedArray = new string[1000];
        private static string[] CustomerItemID_UpdatedArray = new string[1000];
        private static string[] Sequence_UpdatedArray = new string[1000];
        private static string[] Steps_UpdatedArray = new string[1000];
        private static string[] StepsUsed_UpdatedArray = new string[1000];
        private static string[] Sample3D_UpdatedArray = new string[1000];
        private static string[] Ready3D_UpdatedArray = new string[1000];
        private static string[] TotalRuns_UpdatedArray = new string[1000];
        private static string[] PartsManufactured_UpdatedArray = new string[1000];
        private static string[] PartsPerMinute_UpdatedArray = new string[1000];
        private static string[] SetupTime_UpdatedArray = new string[1000];
        private static string[] Tooling_UpdatedArray = new string[1000];
        private static string[] ToolingLocation_UpdatedArray = new string[1000];
        private static string[] Fixture_UpdatedArray = new string[1000];
        private static string[] FixtureLocation_UpdatedArray = new string[1000];
        private static string[] Template_UpdatedArray = new string[1000];
        private static string[] TemplateLocation_UpdatedArray = new string[1000];

        // CAT Brake Press Arrays
        private static string[] BP1107_UpdatedArray = new string[1000];
        private static string[] BP1139_UpdatedArray = new string[1000];
        private static string[] BP1177_UpdatedArray = new string[1000];

        // John Deere Brake Press Arrays
        private static string[] BP1127_UpdatedArray = new string[1000];
        private static string[] BP1178_UpdatedArray = new string[1000];

        // Navistar Brake Press Arrays
        private static string[] BP1065_UpdatedArray = new string[1000];
        private static string[] BP1108_UpdatedArray = new string[1000];
        private static string[] BP1156_UpdatedArray = new string[1000];
        private static string[] BP1720_UpdatedArray = new string[1000];

        // Paccar Brake Press Arrays
        private static string[] BP1083_UpdatedArray = new string[1000];
        private static string[] BP1155_UpdatedArray = new string[1000];
        private static string[] BP1158_UpdatedArray = new string[1000];
        private static string[] BP1175_UpdatedArray = new string[1000];
        private static string[] BP1176_UpdatedArray = new string[1000];

        private static string CellName = "";

        /********************************************************************************************************************
        * 
        * Variables In Testing End
        * 
        *********************************************************************************************************************
        *********************************************************************************************************************
        * 
        * JobList Start
        * 
        ********************************************************************************************************************/

        private void JobList_Load(object sender, EventArgs e)
        {
            SqlConnection JobListLogin = new SqlConnection(SQL_Source);
            SqlCommand LoginJobList = new SqlCommand();
            LoginJobList.CommandType = System.Data.CommandType.Text;
            LoginJobList.CommandText = "INSERT INTO [dbo].[LoginData] (EmployeeName,DMPID,LoginDateTime,LoginForm) VALUES (@EmployeeName,@DMPID,@LoginDateTime,@LoginForm)";
            LoginJobList.Connection = JobListLogin;
            LoginJobList.Parameters.AddWithValue("@LoginDateTime", Clock_TextBox.Text);
            LoginJobList.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            LoginJobList.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            LoginJobList.Parameters.AddWithValue("@LoginForm", LoginForm.ToString());
            JobListLogin.Open();
            LoginJobList.ExecuteNonQuery();
            JobListLogin.Close();

            LoginTime = Clock_TextBox.Text;
            Clock.Enabled = true;
            CustomerCell_ComboBox.Items.Add("CAT");
            BP1107_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1139_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1177_ComboBox.Items.AddRange(BrakePressComboSelections);
            CustomerCell_ComboBox.Items.Add("John Deere");
            BP1127_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1178_ComboBox.Items.AddRange(BrakePressComboSelections);
            CustomerCell_ComboBox.Items.Add("Navistar");
            BP1065_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1108_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1156_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1720_ComboBox.Items.AddRange(BrakePressComboSelections);
            CustomerCell_ComboBox.Items.Add("Paccar");
            BP1083_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1155_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1158_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1175_ComboBox.Items.AddRange(BrakePressComboSelections);
            BP1176_ComboBox.Items.AddRange(BrakePressComboSelections);
            Customer_ComboBox.Items.AddRange(Companies);
            Sequence_ComboBox.Items.AddRange(SequenceNumber);
            Steps_ComboBox.Items.AddRange(Steps);
            Sample_ComboBox.Items.AddRange(ComboSelections);
            ProductionReady_ComboBox.Items.AddRange(ComboSelections);

            SearchItemID_CheckBox.Checked = true;
            CustomerCell_ComboBox.Focus();
        }

        /********************************************************************************************************************
        * 
        * Buttons Region Start 
        * -- Total Buttons: 15
        * 
        * --- JobList Form Buttons
        * --- Total: 2
        * - ChangeCell Button
        * - Exit Button
        * 
        * --- ItemInfo GroupBox Buttons
        * --- Total: 8
        * - Search Click
        * - Clear Click
        * - AddJob Click
        * - EditJob Click
        * - RemoveJob Click
        * - Confirm Click
        * - Cancel Click
        * - Refresh Click
        * 
        * --- Folder GroupBox Button
        * --- Total: 1
        * - ViewCheckCard Click
        * 
        ********************************************************************************************************************/
        #region

        //JobList Form Buttons

        private void ChangeCell_Button_Click(object sender, EventArgs e)
        {
            if (CreateExcel.IsBusy == true)
            {
                //CreateExcel.CancelAsync();
            }
            CustomerCell_ComboBox.Enabled = true;
            ClearForm();
            GroupBoxControlStart();
            ChangeCell_Button.Hide();
        }

        private void Exit_Button_Click(object sender, EventArgs e)
        {
            if (CreateExcel.IsBusy != true)
            {
                EmployeeLogOff_SQL();
                DMPBrakePressLogin.Current.Focus();
                DMPBrakePressLogin.Current.Enabled = true;
                DMPBrakePressLogin.Current.WindowState = FormWindowState.Maximized;
                DMPBrakePressLogin.Current.ShowInTaskbar = true;
                this.Close();
            }
        }

        // ItemInfo GroupBox Buttons

        private void Search_Button_Click(object sender, EventArgs e)
        {
            SearchForItem();
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            if (CreateExcel.IsBusy == true)
            {
                //CreateExcel.CancelAsync();
            }
            ClearForm();
            GroupBoxControlStart();
        }

        private void AddJob_Button_Click(object sender, EventArgs e)
        {
            if(CustomerCell_ComboBox.Text != "")
            {
                AddJob_ButtonWasClicked = true;
                //EditJob_Button.Hide();
                //RemoveJob_Button.Hide();
                //EditJob_Button.Enabled = false;
                //RemoveJob_Button.Enabled = false;
                ClearForm();
                GroupBoxControl_AddJob();
                JobHideShow();
            }
        }

        private void EditJob_Button_Click(object sender, EventArgs e)
        {
            if (ItemID_TextBox.Text == "")
            {
                MessageBox.Show("Please Select a Job to Edit");
            }
            else
            {
                ItemID_TextBox.ReadOnly = true;
                EditJob_ButtonWasClicked = true;
                //AddJob_Button.Hide();
                //Exit_Button.Hide();
                //RemoveJob_Button.Hide();
                DateOfCompletionPicker.Text = CompletionDate_TextBox.Text;
                Customer_ComboBox.Text = Customer_TextBox.Text;
                Steps_ComboBox.SelectedItem = Steps_TextBox.Text;
                ButtonsControl_Edit();
                GroupBoxControlActive();
                JobHideShow();
            }
        }

        private void RemoveJob_Button_Click(object sender, EventArgs e)
        {
            if (ItemID_TextBox.Text == "")
            {
                MessageBox.Show("Please Select a Job to Remove");
            }
            else
            {
                RemoveJob_ButtonWasClicked = true;
                //EditJob_Button.Hide();
                //AddJob_Button.Hide();
                //Exit_Button.Hide();
                //ItemID_TextBox.ReadOnly = true;
                //GroupBoxControlInactive();
                //JobHideShow();
                Confirm_Button_Click(null, null);
            }
        }
        
        private void Confirm_Button_Click(object sender, EventArgs e)
        {
            if (AddJob_ButtonWasClicked == true)
            {
                JobList_AddJob AddConfirm = new JobList_AddJob();
                if (CustomerCell_ComboBox.Text == "CAT")
                {
                    AddConfirm.CATBrakePress_GroupBox.Location = new System.Drawing.Point(1042, 12);
                    AddConfirm.CATBrakePress_GroupBox.Size = new System.Drawing.Size(290, 226);
                    AddConfirm.CATBrakePress_GroupBox.Visible = true;

                    AddConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    AddConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    AddConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    AddConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    AddConfirm.Sequence_TextBox.Text = Sequence_ComboBox.Text;
                    AddConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    AddConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    AddConfirm.Sample_TextBox.Text = Sample_ComboBox.Text;
                    AddConfirm.Ready_TextBox.Text = ProductionReady_ComboBox.Text;
                    AddConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    AddConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    AddConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    AddConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    AddConfirm.Template_TextBox.Text = Template_TextBox.Text;
                    AddConfirm.TemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    AddConfirm.BP1107_TextBox.Text = BP1107_ComboBox.Text;
                    AddConfirm.BP1139_TextBox.Text = BP1139_ComboBox.Text;
                    AddConfirm.BP1177_TextBox.Text = BP1177_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "John Deere")
                {
                    AddConfirm.JohnDeereBrakePress_GroupBox.Location = new System.Drawing.Point(1042, 12);
                    AddConfirm.JohnDeereBrakePress_GroupBox.Size = new System.Drawing.Size(290, 226);
                    AddConfirm.JohnDeereBrakePress_GroupBox.Visible = true;

                    AddConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    AddConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    AddConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    AddConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    AddConfirm.Sequence_TextBox.Text = Sequence_ComboBox.Text;
                    AddConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    AddConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    AddConfirm.Sample_TextBox.Text = Sample_ComboBox.Text;
                    AddConfirm.Ready_TextBox.Text = ProductionReady_ComboBox.Text;
                    AddConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    AddConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    AddConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    AddConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    AddConfirm.Template_TextBox.Text = Template_TextBox.Text;
                    AddConfirm.TemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    AddConfirm.BP1127_TextBox.Text = BP1127_ComboBox.Text;
                    AddConfirm.BP1178_TextBox.Text = BP1178_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "Navistar")
                {
                    AddConfirm.NavistarBrakePress_GroupBox.Location = new System.Drawing.Point(1042, 12);
                    AddConfirm.NavistarBrakePress_GroupBox.Size = new System.Drawing.Size(343, 226);
                    AddConfirm.NavistarBrakePress_GroupBox.Visible = true;

                    AddConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    AddConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    AddConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    AddConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    AddConfirm.Sequence_TextBox.Text = Sequence_ComboBox.Text;
                    AddConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    AddConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    AddConfirm.Sample_TextBox.Text = Sample_ComboBox.Text;
                    AddConfirm.Ready_TextBox.Text = ProductionReady_ComboBox.Text;
                    AddConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    AddConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    AddConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    AddConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    AddConfirm.Template_TextBox.Text = Template_TextBox.Text;
                    AddConfirm.TemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    AddConfirm.BP1065_TextBox.Text = BP1065_ComboBox.Text;
                    AddConfirm.BP1108_TextBox.Text = BP1108_ComboBox.Text;
                    AddConfirm.BP1156_TextBox.Text = BP1156_ComboBox.Text;
                    AddConfirm.BP1720_TextBox.Text = BP1720_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "Paccar")
                {
                    AddConfirm.PaccarBrakePress_GroupBox.Location = new System.Drawing.Point(1042, 12);
                    AddConfirm.PaccarBrakePress_GroupBox.Size = new System.Drawing.Size(343, 226);
                    AddConfirm.PaccarBrakePress_GroupBox.Visible = true;

                    AddConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    AddConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    AddConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    AddConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    AddConfirm.Sequence_TextBox.Text = Sequence_ComboBox.Text;
                    AddConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    AddConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    AddConfirm.Sample_TextBox.Text = Sample_ComboBox.Text;
                    AddConfirm.Ready_TextBox.Text = ProductionReady_ComboBox.Text;
                    AddConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    AddConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    AddConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    AddConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    AddConfirm.Template_TextBox.Text = Template_TextBox.Text;
                    AddConfirm.TemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    AddConfirm.BP1083_TextBox.Text = BP1083_ComboBox.Text;
                    AddConfirm.BP1155_TextBox.Text = BP1155_ComboBox.Text;
                    AddConfirm.BP1158_TextBox.Text = BP1158_ComboBox.Text;
                    AddConfirm.BP1175_TextBox.Text = BP1175_ComboBox.Text;
                    AddConfirm.BP1176_TextBox.Text = BP1176_ComboBox.Text;
                }
                if (AddConfirm.ShowDialog(this) == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                        SqlCommand Add_Job = new SqlCommand();
                        Add_Job.CommandType = System.Data.CommandType.Text;
                        Add_Job.CommandText = SQLAddCommand;
                        Add_Job.Connection = Job_Connection;
                        if (CustomerCell_ComboBox.Text == "CAT")
                        {
                            Add_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@TotalRuns", 0);
                            Add_Job.Parameters.AddWithValue("@PartsManufactured", 0);
                            Add_Job.Parameters.AddWithValue("@PartsPerMinute", 0);
                            Add_Job.Parameters.AddWithValue("@SetupTime", 0);
                            Add_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1107", BP1107_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1139", BP1139_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1177", BP1177_ComboBox.Text);
                        }
                        else if (CustomerCell_ComboBox.Text == "John Deere")
                        {
                            Add_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@TotalRuns", 0);
                            Add_Job.Parameters.AddWithValue("@PartsManufactured", 0);
                            Add_Job.Parameters.AddWithValue("@PartsPerMinute", 0);
                            Add_Job.Parameters.AddWithValue("@SetupTime", 0);
                            Add_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1127", BP1127_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1178", BP1178_ComboBox.Text);
                        }
                        else if (CustomerCell_ComboBox.Text == "Navistar")
                        {
                            Add_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@TotalRuns", 0);
                            Add_Job.Parameters.AddWithValue("@PartsManufactured", 0);
                            Add_Job.Parameters.AddWithValue("@PartsPerMinute", 0);
                            Add_Job.Parameters.AddWithValue("@SetupTime", 0);
                            Add_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1065", BP1065_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1108", BP1108_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1156", BP1156_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1720", BP1720_ComboBox.Text);
                        }
                        else if (CustomerCell_ComboBox.Text == "Paccar")
                        {
                            Add_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@TotalRuns", 0);
                            Add_Job.Parameters.AddWithValue("@PartsManufactured", 0);
                            Add_Job.Parameters.AddWithValue("@PartsPerMinute", 0);
                            Add_Job.Parameters.AddWithValue("@SetupTime", 0);
                            Add_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1083", BP1065_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1155", BP1155_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1158", BP1158_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1175", BP1175_ComboBox.Text);
                            Add_Job.Parameters.AddWithValue("@BP1176", BP1176_ComboBox.Text);
                        }
                        Job_Connection.Open();
                        Add_Job.ExecuteNonQuery();
                        Job_Connection.Close();
                    }
                    catch (SqlException ExceptionValue)
                    {
                        int ErrorNumber = ExceptionValue.Number;
                        if (ErrorNumber.Equals(2627))
                        {
                            MessageBox.Show("Item ID: " + ItemID_TextBox.Text + " is Already on this List");
                        }
                        else if (ErrorNumber.Equals(245))
                        {
                            MessageBox.Show("Item ID Can Only Contain Numbers");
                        }
                        else
                        {
                            MessageBox.Show("Unable to Add Job. Please Try Again." + "\n" + "Error Code: " + ErrorNumber.ToString());
                        }
                    }
                    ConfirmFinished();
                    Refresh_SQL();
                    CreateExcel_Start();
                    if (CreateExcel.IsBusy != true)
                    {
                        CreateExcel.RunWorkerAsync();
                    }
                    ClearForm();
                    GroupBoxControlStart();
                }
                else
                {
                    ConfirmFinished();
                    ClearForm();
                    GroupBoxControlStart();
                }
            }
            else if (EditJob_ButtonWasClicked == true)
            {
                JobList_EditJob EditConfirm = new JobList_EditJob();
                if (CustomerCell_ComboBox.Text == "CAT")
                {
                    EditConfirm.CATBrakePress_GroupBox.Location = new System.Drawing.Point(1027, 40);
                    EditConfirm.CATBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.CATBrakePress_GroupBox.Visible = true;
                    EditConfirm.UpdatedCATBrakePress_GroupBox.Location = new System.Drawing.Point(1029, 513);
                    EditConfirm.UpdatedCATBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.UpdatedCATBrakePress_GroupBox.Visible = true;

                    EditConfirm.UpdatedItemID_TextBox.Text = ItemID_TextBox.Text;
                    EditConfirm.UpdatedCustomer_TextBox.Text = Customer_ComboBox.Text;
                    EditConfirm.UpdatedCustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    EditConfirm.UpdatedJobID_TextBox.Text = JobID_TextBox.Text;
                    EditConfirm.UpdatedSequence_TextBox.Text = Sequence_ComboBox.Text;
                    EditConfirm.UpdatedNumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    EditConfirm.UpdatedStepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    EditConfirm.UpdatedSample_TextBox.Text = Sample_ComboBox.Text;
                    EditConfirm.UpdatedReady_TextBox.Text = ProductionReady_ComboBox.Text;
                    EditConfirm.UpdatedTooling_TextBox.Text = Tooling_TextBox.Text;
                    EditConfirm.UpdatedToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    EditConfirm.UpdatedFixture_TextBox.Text = Fixture_TextBox.Text;
                    EditConfirm.UpdatedFixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    EditConfirm.UpdatedTemplate_TextBox.Text = Template_TextBox.Text;
                    EditConfirm.UpdatedTemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    EditConfirm.UpdatedBP1107_TextBox.Text = BP1107_ComboBox.Text;
                    EditConfirm.UpdatedBP1139_TextBox.Text = BP1139_ComboBox.Text;
                    EditConfirm.UpdatedBP1177_TextBox.Text = BP1177_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "John Deere")
                {
                    EditConfirm.JohnDeereBrakePress_GroupBox.Location = new System.Drawing.Point(1027, 40);
                    EditConfirm.JohnDeereBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.JohnDeereBrakePress_GroupBox.Visible = true;
                    EditConfirm.UpdatedJohnDeereBrakePress_GroupBox.Location = new System.Drawing.Point(1029, 513);
                    EditConfirm.UpdatedJohnDeereBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.UpdatedJohnDeereBrakePress_GroupBox.Visible = true;

                    EditConfirm.UpdatedItemID_TextBox.Text = ItemID_TextBox.Text;
                    EditConfirm.UpdatedCustomer_TextBox.Text = Customer_ComboBox.Text;
                    EditConfirm.UpdatedCustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    EditConfirm.UpdatedJobID_TextBox.Text = JobID_TextBox.Text;
                    EditConfirm.UpdatedSequence_TextBox.Text = Sequence_ComboBox.Text;
                    EditConfirm.UpdatedNumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    EditConfirm.UpdatedStepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    EditConfirm.UpdatedSample_TextBox.Text = Sample_ComboBox.Text;
                    EditConfirm.UpdatedReady_TextBox.Text = ProductionReady_ComboBox.Text;
                    EditConfirm.UpdatedTooling_TextBox.Text = Tooling_TextBox.Text;
                    EditConfirm.UpdatedToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    EditConfirm.UpdatedFixture_TextBox.Text = Fixture_TextBox.Text;
                    EditConfirm.UpdatedFixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    EditConfirm.UpdatedTemplate_TextBox.Text = Template_TextBox.Text;
                    EditConfirm.UpdatedTemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    EditConfirm.UpdatedBP1127_TextBox.Text = BP1127_ComboBox.Text;
                    EditConfirm.UpdatedBP1178_TextBox.Text = BP1178_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "Navistar")
                {
                    EditConfirm.NavistarBrakePress_GroupBox.Location = new System.Drawing.Point(1027, 40);
                    EditConfirm.NavistarBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.NavistarBrakePress_GroupBox.Visible = true;
                    EditConfirm.UpdatedNavistarBrakePress_GroupBox.Location = new System.Drawing.Point(1029, 513);
                    EditConfirm.UpdatedNavistarBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.UpdatedNavistarBrakePress_GroupBox.Visible = true;

                    EditConfirm.UpdatedItemID_TextBox.Text = ItemID_TextBox.Text;
                    EditConfirm.UpdatedCustomer_TextBox.Text = Customer_ComboBox.Text;
                    EditConfirm.UpdatedCustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    EditConfirm.UpdatedJobID_TextBox.Text = JobID_TextBox.Text;
                    EditConfirm.UpdatedSequence_TextBox.Text = Sequence_ComboBox.Text;
                    EditConfirm.UpdatedNumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    EditConfirm.UpdatedStepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    EditConfirm.UpdatedSample_TextBox.Text = Sample_ComboBox.Text;
                    EditConfirm.UpdatedReady_TextBox.Text = ProductionReady_ComboBox.Text;
                    EditConfirm.UpdatedTooling_TextBox.Text = Tooling_TextBox.Text;
                    EditConfirm.UpdatedToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    EditConfirm.UpdatedFixture_TextBox.Text = Fixture_TextBox.Text;
                    EditConfirm.UpdatedFixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    EditConfirm.UpdatedTemplate_TextBox.Text = Template_TextBox.Text;
                    EditConfirm.UpdatedTemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    EditConfirm.UpdatedBP1065_TextBox.Text = BP1065_ComboBox.Text;
                    EditConfirm.UpdatedBP1108_TextBox.Text = BP1108_ComboBox.Text;
                    EditConfirm.UpdatedBP1156_TextBox.Text = BP1156_ComboBox.Text;
                    EditConfirm.UpdatedBP1720_TextBox.Text = BP1720_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "Paccar")
                {
                    EditConfirm.PaccarBrakePress_GroupBox.Location = new System.Drawing.Point(1027, 40);
                    EditConfirm.PaccarBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.PaccarBrakePress_GroupBox.Visible = true;
                    EditConfirm.UpdatedPaccarBrakePress_GroupBox.Location = new System.Drawing.Point(1029, 513);
                    EditConfirm.UpdatedPaccarBrakePress_GroupBox.Size = new System.Drawing.Size(343, 200);
                    EditConfirm.UpdatedPaccarBrakePress_GroupBox.Visible = true;

                    EditConfirm.UpdatedItemID_TextBox.Text = ItemID_TextBox.Text;
                    EditConfirm.UpdatedCustomer_TextBox.Text = Customer_ComboBox.Text;
                    EditConfirm.UpdatedCustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    EditConfirm.UpdatedJobID_TextBox.Text = JobID_TextBox.Text;
                    EditConfirm.UpdatedSequence_TextBox.Text = Sequence_ComboBox.Text;
                    EditConfirm.UpdatedNumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                    EditConfirm.UpdatedStepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    EditConfirm.UpdatedSample_TextBox.Text = Sample_ComboBox.Text;
                    EditConfirm.UpdatedReady_TextBox.Text = ProductionReady_ComboBox.Text;
                    EditConfirm.UpdatedTooling_TextBox.Text = Tooling_TextBox.Text;
                    EditConfirm.UpdatedToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    EditConfirm.UpdatedFixture_TextBox.Text = Fixture_TextBox.Text;
                    EditConfirm.UpdatedFixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    EditConfirm.UpdatedTemplate_TextBox.Text = Template_TextBox.Text;
                    EditConfirm.UpdatedTemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                    EditConfirm.UpdatedBP1083_TextBox.Text = BP1083_ComboBox.Text;
                    EditConfirm.UpdatedBP1155_TextBox.Text = BP1155_ComboBox.Text;
                    EditConfirm.UpdatedBP1158_TextBox.Text = BP1158_ComboBox.Text;
                    EditConfirm.UpdatedBP1175_TextBox.Text = BP1175_ComboBox.Text;
                    EditConfirm.UpdatedBP1176_TextBox.Text = BP1176_ComboBox.Text;
                }
                JobListGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                try
                {
                    SearchValue = ItemID_TextBox.Text;
                    foreach (DataGridViewRow Row in JobListGridView.Rows)
                    {
                        Row.Selected = false;
                        if (Row.Cells[0].Value.ToString().Equals(SearchValue))
                        {
                            if (CustomerCell_ComboBox.Text == "CAT")
                            {
                                EditConfirm.ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                                EditConfirm.Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                                EditConfirm.CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                                EditConfirm.JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                                //EditConfirm.DateOfCompletion_TextBox.Text = row.Cells[4].Value.ToString();
                                EditConfirm.Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                                EditConfirm.NumberOfSteps_TextBox.Text = Row.Cells[5].Value.ToString();
                                EditConfirm.StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                                EditConfirm.Sample_TextBox.Text = Row.Cells[7].Value.ToString();
                                EditConfirm.Ready_TextBox.Text = Row.Cells[8].Value.ToString();

                                EditConfirm.Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                                EditConfirm.ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                                EditConfirm.Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                                EditConfirm.FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                                EditConfirm.Template_TextBox.Text = Row.Cells[17].Value.ToString();
                                EditConfirm.TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                                EditConfirm.BP1107_TextBox.Text = Row.Cells[19].Value.ToString();
                                EditConfirm.BP1139_TextBox.Text = Row.Cells[20].Value.ToString();
                                EditConfirm.BP1177_TextBox.Text = Row.Cells[21].Value.ToString();
                            }
                            else if (CustomerCell_ComboBox.Text == "John Deere")
                            {
                                EditConfirm.ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                                EditConfirm.Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                                EditConfirm.CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                                EditConfirm.JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                                //EditConfirm.DateOfCompletion_TextBox.Text = row.Cells[4].Value.ToString();
                                EditConfirm.Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                                EditConfirm.NumberOfSteps_TextBox.Text = Row.Cells[5].Value.ToString();
                                EditConfirm.StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                                EditConfirm.Sample_TextBox.Text = Row.Cells[7].Value.ToString();
                                EditConfirm.Ready_TextBox.Text = Row.Cells[8].Value.ToString();

                                EditConfirm.Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                                EditConfirm.ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                                EditConfirm.Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                                EditConfirm.FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                                EditConfirm.Template_TextBox.Text = Row.Cells[17].Value.ToString();
                                EditConfirm.TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                                EditConfirm.BP1127_TextBox.Text = Row.Cells[19].Value.ToString();
                                EditConfirm.BP1178_TextBox.Text = Row.Cells[20].Value.ToString();
                            }
                            else if (CustomerCell_ComboBox.Text == "Navistar")
                            {
                                EditConfirm.ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                                EditConfirm.Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                                EditConfirm.CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                                EditConfirm.JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                                //EditConfirm.DateOfCompletion_TextBox.Text = row.Cells[4].Value.ToString();
                                EditConfirm.Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                                EditConfirm.NumberOfSteps_TextBox.Text = Row.Cells[5].Value.ToString();
                                EditConfirm.StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                                EditConfirm.Sample_TextBox.Text = Row.Cells[7].Value.ToString();
                                EditConfirm.Ready_TextBox.Text = Row.Cells[8].Value.ToString();

                                EditConfirm.Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                                EditConfirm.ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                                EditConfirm.Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                                EditConfirm.FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                                EditConfirm.Template_TextBox.Text = Row.Cells[17].Value.ToString();
                                EditConfirm.TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                                EditConfirm.BP1065_TextBox.Text = Row.Cells[19].Value.ToString();
                                EditConfirm.BP1108_TextBox.Text = Row.Cells[20].Value.ToString();
                                EditConfirm.BP1156_TextBox.Text = Row.Cells[21].Value.ToString();
                                EditConfirm.BP1720_TextBox.Text = Row.Cells[22].Value.ToString();
                            }
                            else if (CustomerCell_ComboBox.Text == "Paccar")
                            {
                                EditConfirm.ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                                EditConfirm.Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                                EditConfirm.CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                                EditConfirm.JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                                //EditConfirm.DateOfCompletion_TextBox.Text = row.Cells[4].Value.ToString();
                                EditConfirm.Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                                EditConfirm.NumberOfSteps_TextBox.Text = Row.Cells[5].Value.ToString();
                                EditConfirm.StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                                EditConfirm.Sample_TextBox.Text = Row.Cells[7].Value.ToString();
                                EditConfirm.Ready_TextBox.Text = Row.Cells[8].Value.ToString();

                                EditConfirm.Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                                EditConfirm.ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                                EditConfirm.Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                                EditConfirm.FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                                EditConfirm.Template_TextBox.Text = Row.Cells[17].Value.ToString();
                                EditConfirm.TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                                EditConfirm.BP1083_TextBox.Text = Row.Cells[19].Value.ToString();
                                EditConfirm.BP1155_TextBox.Text = Row.Cells[20].Value.ToString();
                                EditConfirm.BP1158_TextBox.Text = Row.Cells[21].Value.ToString();
                                EditConfirm.BP1175_TextBox.Text = Row.Cells[22].Value.ToString();
                                EditConfirm.BP1176_TextBox.Text = Row.Cells[23].Value.ToString();
                            }
                            string dateValue = CompletionDate_TextBox.Text;
                            dateValue = dateValue.Replace(" 12:00:00 AM", "");
                            EditConfirm.DateOfCompletion_TextBox.Text = dateValue;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error Finding Job");
                }
                if (EditConfirm.ShowDialog(this) == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                        SqlCommand Edit_Job = new SqlCommand();
                        Edit_Job.CommandType = System.Data.CommandType.Text;
                        Edit_Job.CommandText = SQLEditCommand;
                        Edit_Job.Connection = Job_Connection;

                        if (CustomerCell_ComboBox.Text == "CAT")
                        {
                            Edit_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1107", BP1107_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1139", BP1139_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1177", BP1177_ComboBox.Text);
                        }
                        else if (CustomerCell_ComboBox.Text == "John Deere")
                        {
                            Edit_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1127", BP1127_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1178", BP1178_ComboBox.Text);
                        }
                        else if (CustomerCell_ComboBox.Text == "Navistar")
                        {
                            Edit_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1065", BP1065_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1108", BP1108_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1156", BP1156_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1720", BP1720_ComboBox.Text);
                        }
                        else if (CustomerCell_ComboBox.Text == "Paccar")
                        {
                            Edit_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Customer", Customer_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Steps", Steps_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Sample3D", Sample_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Ready3D", ProductionReady_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Fixture", Fixture_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@Template", Template_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_TextBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1083", BP1083_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1155", BP1155_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1158", BP1158_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1175", BP1175_ComboBox.Text);
                            Edit_Job.Parameters.AddWithValue("@BP1176", BP1176_ComboBox.Text);
                        }
                        Job_Connection.Open();
                        Edit_Job.ExecuteNonQuery();
                        Job_Connection.Close();
                    }
                    catch (SqlException ExceptionValue)
                    {
                        int ErrorNumber = ExceptionValue.Number;
                        MessageBox.Show("Unable to Edit Job" + "\n" + "Error Code: " + ErrorNumber.ToString());
                    }
                    ConfirmFinished();
                    //Refresh_SQL();
                    //CreateExcelFile(null, null
                    GroupBoxControlStart();
                    CreateExcel_Start();
                    if (CreateExcel.IsBusy != true)
                    {
                        CreateExcel.RunWorkerAsync();
                    }
                    //ClearForm();
                    //GroupBoxControlStart();
                }
                else
                {
                    ConfirmFinished();
                    //ClearForm();
                    GroupBoxControlStart();
                }
            }
            else if (RemoveJob_ButtonWasClicked == true)
            {
                JobList_RemoveJob RemoveConfirm = new JobList_RemoveJob();
                if (CustomerCell_ComboBox.Text == "CAT")
                {
                    RemoveConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    RemoveConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    RemoveConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    RemoveConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    RemoveConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    RemoveConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    RemoveConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    RemoveConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    RemoveConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    RemoveConfirm.DateOfCompletion_TextBox.Text = CompletionDate_TextBox.Text;
                    RemoveConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "John Deere")
                {
                    RemoveConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    RemoveConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    RemoveConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    RemoveConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    RemoveConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    RemoveConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    RemoveConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    RemoveConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    RemoveConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    RemoveConfirm.DateOfCompletion_TextBox.Text = CompletionDate_TextBox.Text;
                    RemoveConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "Navistar")
                {
                    RemoveConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    RemoveConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    RemoveConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    RemoveConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    RemoveConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    RemoveConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    RemoveConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    RemoveConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    RemoveConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    RemoveConfirm.DateOfCompletion_TextBox.Text = CompletionDate_TextBox.Text;
                    RemoveConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                }
                else if (CustomerCell_ComboBox.Text == "Paccar")
                {
                    RemoveConfirm.ItemID_TextBox.Text = ItemID_TextBox.Text;
                    RemoveConfirm.JobID_TextBox.Text = JobID_TextBox.Text;
                    RemoveConfirm.Customer_TextBox.Text = Customer_ComboBox.Text;
                    RemoveConfirm.CustomerItemID_TextBox.Text = CustomerItemID_TextBox.Text;
                    RemoveConfirm.StepsUsed_TextBox.Text = StepsUsed_TextBox.Text;
                    RemoveConfirm.Tooling_TextBox.Text = Tooling_TextBox.Text;
                    RemoveConfirm.ToolingLocation_TextBox.Text = ToolingLocation_TextBox.Text;
                    RemoveConfirm.Fixture_TextBox.Text = Fixture_TextBox.Text;
                    RemoveConfirm.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                    RemoveConfirm.DateOfCompletion_TextBox.Text = CompletionDate_TextBox.Text;
                    RemoveConfirm.NumberOfSteps_TextBox.Text = Steps_ComboBox.Text;
                }
                if (RemoveConfirm.ShowDialog(this) == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                        SqlCommand Delete_Job = new SqlCommand();
                        Delete_Job.CommandType = System.Data.CommandType.Text;
                        Delete_Job.CommandText = SQLRemoveCommand;
                        Delete_Job.Connection = Job_Connection;
                        Delete_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
                        Delete_Job.Parameters.AddWithValue("@Sequence", Sequence_ComboBox.Text);
                        Job_Connection.Open();
                        Delete_Job.ExecuteNonQuery();
                        Job_Connection.Close();
                        MessageBox.Show("Job Was Successfully Removed");
                    }
                    catch (SqlException ExceptionValue)
                    {
                        int ErrorNumber = ExceptionValue.Number;
                        MessageBox.Show("Error Removing Job" + "\n" + "Error Code: " + ErrorNumber.ToString());
                    }
                    ConfirmFinished();
                    Refresh_SQL();
                    CreateExcel_Start();
                    if (CreateExcel.IsBusy != true)
                    {
                        CreateExcel.RunWorkerAsync();
                    }
                    ClearForm();
                    GroupBoxControlStart();
                }
                else
                {
                    ConfirmFinished();
                    ClearForm();
                    GroupBoxControlStart();
                }
            }
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            AddJob_Button.Show();
            EditJob_Button.Show();
            RemoveJob_Button.Show();
            Exit_Button.Show();
            Confirm_Button.Hide();
            Cancel_Button.Hide();
            DateOfCompletionPicker.Hide();
            Customer_ComboBox.Hide();
            Steps_ComboBox.Hide();
            SearchItemID_CheckBox.Show();
            SearchJobID_CheckBox.Show();
            CompletionDate_TextBox.Show();
            Steps_TextBox.Show();
            Customer_TextBox.Show();
            ItemID_TextBox.ReadOnly = false;
            AddJob_ButtonWasClicked = false;
            EditJob_ButtonWasClicked = false;
            RemoveJob_ButtonWasClicked = false;
            ClearForm();
            GroupBoxControlStart();
            ButtonsControl_Cancel();
        }

        private void Refresh_Button_Click(object sender, EventArgs e)
        {
            Refresh_SQL();
        }
        
        private void ViewCheckCard_Button_Click(object sender, EventArgs e)
        {
            Check_Card_Data_Viewer CheckCardViewer = new Check_Card_Data_Viewer();
            CheckCardViewer.ItemID_TextBox.Text = ItemID_TextBox.Text;
            CheckCardViewer.Clock_TextBox.Text = Clock_TextBox.Text;
            CheckCardViewer.User_TextBox.Text = User_TextBox.Text;
            CheckCardViewer.Show();
        }

        /********************************************************************************************************************
        * 
        * Buttons Region End
        * 
        ********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * CheckBox Region Start
        * -- Total: 2
        * 
        * - SearchItemID CheckedChanged
        * - SearchJobID CheckedChanged
        * 
        *********************************************************************************************************************/
        #region

        private void SearchItemID_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SearchItemID_CheckBox.Checked == true)
            {
                SearchJobID_CheckBox.Checked = false;
            }
        }

        private void SearchJobID_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SearchJobID_CheckBox.Checked == true)
            {
                SearchItemID_CheckBox.Checked = false;
            }
        }

        private void NotApplicable_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CustomerCell_ComboBox.Text == "CAT")
            {
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string JohnDeereSelect = "SELECT * FROM [dbo].[CAT_NotApplicable_Item_Data] ORDER BY ItemID DESC";
                SqlDataAdapter JohnDeereDataAdapter = new SqlDataAdapter(JohnDeereSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(JohnDeereDataAdapter);
                DataSet JohnDeereData = new DataSet();
                JohnDeereDataAdapter.Fill(JohnDeereData);
                JobListGridView.DataSource = JohnDeereData.Tables[0];
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string JohnDeereSelect = "SELECT * FROM [dbo].[Navistar_NotApplicable_Item_Data] ORDER BY ItemID DESC";
                SqlDataAdapter JohnDeereDataAdapter = new SqlDataAdapter(JohnDeereSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(JohnDeereDataAdapter);
                DataSet JohnDeereData = new DataSet();
                JohnDeereDataAdapter.Fill(JohnDeereData);
                JobListGridView.DataSource = JohnDeereData.Tables[0];
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string JohnDeereSelect = "SELECT * FROM [dbo].[JohnDeere_NotApplicable_Item_Data] ORDER BY ItemID DESC";
                SqlDataAdapter JohnDeereDataAdapter = new SqlDataAdapter(JohnDeereSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(JohnDeereDataAdapter);
                DataSet JohnDeereData = new DataSet();
                JohnDeereDataAdapter.Fill(JohnDeereData);
                JobListGridView.DataSource = JohnDeereData.Tables[0];
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string JohnDeereSelect = "SELECT * FROM [dbo].[Paccar_NotApplicable_Item_Data] ORDER BY ItemID DESC";
                SqlDataAdapter JohnDeereDataAdapter = new SqlDataAdapter(JohnDeereSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(JohnDeereDataAdapter);
                DataSet JohnDeereData = new DataSet();
                JohnDeereDataAdapter.Fill(JohnDeereData);
                JobListGridView.DataSource = JohnDeereData.Tables[0];
            }
        }

        /*********************************************************************************************************************
        * 
        * CheckBox Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * ComboBox Region Start
        * -- Total: 2
        * 
        * - Customer SelectedIndexChanged
        * - BrakePress SelectedIndexChanged
        * 
        *********************************************************************************************************************/
        #region

        private void CustomerCell_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrakePress_ComboBox.Items.Clear();
            CustomerCell_ComboBox.Enabled = false;
            ChangeCell_Button.Show();

            // Buttons Enabled
            Search_Button.Enabled = true;
            Clear_Button.Enabled = true;
            Refresh_Button.Enabled = true;
            AddJob_Button.Enabled = true;
            EditJob_Button.Enabled = true;
            RemoveJob_Button.Enabled = true;
            DatabaseImport_Button.Enabled = true;

            if (CustomerCell_ComboBox.Text == "CAT")
            {
                ReportCell = "CAT";
                this.CATBrakePress_GroupBox.Location = new System.Drawing.Point(922, 784);
                this.CATBrakePress_GroupBox.Size = new System.Drawing.Size(602, 231);
                CATBrakePress_GroupBox.Visible = true;
                JohnDeereBrakePress_GroupBox.Visible = false;
                NavistarBrakePress_GroupBox.Visible = false;
                PaccarBrakePress_GroupBox.Visible = false;

                // Connect to SQL DataTable and Load
                
                Refresh_Data = "SELECT * FROM [dbo].[CAT_Item_Data] ORDER BY JobID DESC";
                SQLAddCommand = "INSERT INTO [dbo].[CAT_Item_Data] (ItemID,Customer,CustomerItemID,JobID,Sequence,Steps,StepsUsed,Sample3D,Ready3D,TotalRuns,PartsManufactured,PartsPerMinute,SetupTime,Tooling,ToolingLocation,Fixture,FixtureLocation,Template,TemplateLocation,BP1107,BP1139,BP1177) VALUES (@ItemID,@Customer,@CustomerItemID,@JobID,@Sequence,@Steps,@StepsUsed,@Sample3D,@Ready3D,@TotalRuns,@PartsManufactured,@PartsPerMinute,@SetupTime,@Tooling,@ToolingLocation,@Fixture,@FixtureLocation,@Template,@TemplateLocation,@BP1107,@BP1139,@BP1177)";
                SQLEditCommand = "UPDATE [dbo].[CAT_Item_Data] SET Customer=@Customer,CustomerItemID=@CustomerItemID,JobID=@JobID,Steps=@Steps,StepsUsed=@StepsUsed,Sample3D=@Sample3D,Ready3D=@Ready3D,Tooling=@Tooling,ToolingLocation=@ToolingLocation,Fixture=@Fixture,FixtureLocation=@FixtureLocation,Template=@Template,TemplateLocation=@TemplateLocation,BP1107=@BP1107,BP1139=@BP1139,BP1177=@BP1177 WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SQLRemoveCommand = "DELETE FROM [dbo].[CAT_Item_Data] WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string CATSelect = "SELECT * FROM [dbo].[CAT_Item_Data] ORDER BY JobID DESC";
                /*
                Refresh_Data = "SELECT * FROM [dbo].[Test_Table] ORDER BY JobID DESC";
                SQLAddCommand = "INSERT INTO [dbo].[Test_Table] (ItemID,Customer,CustomerItemID,JobID,Sequence,Steps,StepsUsed,Sample3D,Ready3D,TotalRuns,PartsManufactured,PartsPerMinute,SetupTime,Tooling,ToolingLocation,Fixture,FixtureLocation,Template,TemplateLocation,BP1107,BP1139,BP1177) VALUES (@ItemID,@Customer,@CustomerItemID,@JobID,@Sequence,@Steps,@StepsUsed,@Sample3D,@Ready3D,@TotalRuns,@PartsManufactured,@PartsPerMinute,@SetupTime,@Tooling,@ToolingLocation,@Fixture,@FixtureLocation,@Template,@TemplateLocation,@BP1107,@BP1139,@BP1177)";
                SQLEditCommand = "UPDATE [dbo].[Test_Table] SET Customer=@Customer,CustomerItemID=@CustomerItemID,JobID=@JobID,Steps=@Steps,StepsUsed=@StepsUsed,Sample3D=@Sample3D,Ready3D=@Ready3D,Tooling=@Tooling,ToolingLocation=@ToolingLocation,Fixture=@Fixture,FixtureLocation=@FixtureLocation,Template=@Template,TemplateLocation=@TemplateLocation,BP1107=@BP1107,BP1139=@BP1139,BP1177=@BP1177 WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SQLRemoveCommand = "DELETE FROM [dbo].[Test_Table] WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string CATSelect = "SELECT * FROM [dbo].[Test_Table] ORDER BY JobID DESC";
                */
                SqlDataAdapter CATDataAdapter = new SqlDataAdapter(CATSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(CATDataAdapter);
                DataSet CATData = new DataSet();
                CATDataAdapter.Fill(CATData);
                JobListGridView.DataSource = CATData.Tables[0];
            }            
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                ReportCell = "John Deere";
                this.JohnDeereBrakePress_GroupBox.Location = new System.Drawing.Point(922, 784);
                this.JohnDeereBrakePress_GroupBox.Size = new System.Drawing.Size(602, 231);
                CATBrakePress_GroupBox.Visible = false;
                JohnDeereBrakePress_GroupBox.Visible = true;
                NavistarBrakePress_GroupBox.Visible = false;
                PaccarBrakePress_GroupBox.Visible = false;

                // Connect to SQL DataTable and Load
                Refresh_Data = "SELECT * FROM [dbo].[JohnDeere_Item_Data] ORDER BY JobID DESC";
                SQLAddCommand = "INSERT INTO [dbo].[JohnDeere_Item_Data] (ItemID,Customer,CustomerItemID,JobID,Sequence,Steps,StepsUsed,Sample3D,Ready3D,TotalRuns,PartsManufactured,PartsPerMinute,SetupTime,Tooling,ToolingLocation,Fixture,FixtureLocation,Template,TemplateLocation,BP1127,BP1178) VALUES (@ItemID,@Customer,@CustomerItemID,@JobID,@Sequence,@Steps,@StepsUsed,@Sample3D,@Ready3D,@TotalRuns,@PartsManufactured,@PartsPerMinute,@SetupTime,@Tooling,@ToolingLocation,@Fixture,@FixtureLocation,@Template,@TemplateLocation,@BP1127,@BP1178)";
                SQLEditCommand = "UPDATE [dbo].[JohnDeere_Item_Data] SET Customer=@Customer,CustomerItemID=@CustomerItemID,JobID=@JobID,Steps=@Steps,StepsUsed=@StepsUsed,Sample3D=@Sample3D,Ready3D=@Ready3D,Tooling=@Tooling,ToolingLocation=@ToolingLocation,Fixture=@Fixture,FixtureLocation=@FixtureLocation,Template=@Template,TemplateLocation=@TemplateLocation,BP1127=@BP1127,BP1178=@BP1178 WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SQLRemoveCommand = "DELETE FROM [dbo].[JohnDeere_Item_Data] WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string JohnDeereSelect = "SELECT * FROM [dbo].[JohnDeere_Item_Data] ORDER BY JobID DESC";
                SqlDataAdapter JohnDeereDataAdapter = new SqlDataAdapter(JohnDeereSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(JohnDeereDataAdapter);
                DataSet JohnDeereData = new DataSet();
                JohnDeereDataAdapter.Fill(JohnDeereData);
                JobListGridView.DataSource = JohnDeereData.Tables[0];

            } else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                ReportCell = "Navistar";
                this.NavistarBrakePress_GroupBox.Location = new System.Drawing.Point(922, 784);
                this.NavistarBrakePress_GroupBox.Size = new System.Drawing.Size(602, 231);
                CATBrakePress_GroupBox.Visible = false;
                JohnDeereBrakePress_GroupBox.Visible = false;
                NavistarBrakePress_GroupBox.Visible = true;
                PaccarBrakePress_GroupBox.Visible = false;

                // Connect to SQL DataTable and Load
                Refresh_Data = "SELECT * FROM [dbo].[Navistar_Item_Data] ORDER BY JobID DESC";
                SQLAddCommand = "INSERT INTO [dbo].[Navistar_Item_Data] (ItemID,Customer,CustomerItemID,JobID,Sequence,Steps,StepsUsed,Sample3D,Ready3D,TotalRuns,PartsManufactured,PartsPerMinute,SetupTime,Tooling,ToolingLocation,Fixture,FixtureLocation,Template,TemplateLocation,BP1065,BP1108,BP1156,BP1720) VALUES (@ItemID,@Customer,@CustomerItemID,@JobID,@Sequence,@Steps,@StepsUsed,@Sample3D,@Ready3D,@TotalRuns,@PartsManufactured,@PartsPerMinute,@SetupTime,@Tooling,@ToolingLocation,@Fixture,@FixtureLocation,@Template,@TemplateLocation,@BP1065,@BP1108,@BP1156,@BP1720)";
                SQLEditCommand = "UPDATE [dbo].[Navistar_Item_Data] SET Customer=@Customer,CustomerItemID=@CustomerItemID,JobID=@JobID,Steps=@Steps,StepsUsed=@StepsUsed,Sample3D=@Sample3D,Ready3D=@Ready3D,Tooling=@Tooling,ToolingLocation=@ToolingLocation,Fixture=@Fixture,FixtureLocation=@FixtureLocation,Template=@Template,TemplateLocation=@TemplateLocation,BP1065=@BP1065,BP1108=@BP1108,BP1156=@BP1156,BP1720=@BP1720 WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SQLRemoveCommand = "DELETE FROM [dbo].[Navistar_Item_Data] WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string NavistarSelect = "SELECT * FROM [dbo].[Navistar_Item_Data] ORDER BY JobID DESC";
                SqlDataAdapter NavistarDataAdapter = new SqlDataAdapter(NavistarSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(NavistarDataAdapter);
                DataSet NavistarData = new DataSet();
                NavistarDataAdapter.Fill(NavistarData);
                JobListGridView.DataSource = NavistarData.Tables[0];

            } else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                ReportCell = "Paccar";
                CATBrakePress_GroupBox.Visible = false;
                JohnDeereBrakePress_GroupBox.Visible = false;
                NavistarBrakePress_GroupBox.Visible = false;
                PaccarBrakePress_GroupBox.Visible = true;

                // Connect to SQL DataTable and Load
                Refresh_Data = "SELECT * FROM [dbo].[Paccar_Item_Data] ORDER BY JobID DESC";
                SQLAddCommand = "INSERT INTO [dbo].[Paccar_Item_Data] (ItemID,Customer,CustomerItemID,JobID,Sequence,Steps,StepsUsed,Sample3D,Ready3D,TotalRuns,PartsManufactured,PartsPerMinute,SetupTime,Tooling,ToolingLocation,Fixture,FixtureLocation,Template,TemplateLocation,BP1083,BP1155,BP1158,BP1175,BP1176) VALUES (@ItemID,@Customer,@CustomerItemID,@JobID,@Sequence,@Steps,@StepsUsed,@Sample3D,@Ready3D,@TotalRuns,@PartsManufactured,@PartsPerMinute,@SetupTime,@Tooling,@ToolingLocation,@Fixture,@FixtureLocation,@Template,@TemplateLocation,@BP1083,@BP1155,@BP1158,@BP1175,@BP1176)";
                SQLEditCommand = "UPDATE [dbo].[Paccar_Item_Data] SET Customer=@Customer,CustomerItemID=@CustomerItemID,JobID=@JobID,Steps=@Steps,StepsUsed=@StepsUsed,Sample3D=@Sample3D,Ready3D=@Ready3D,Tooling=@Tooling,ToolingLocation=@ToolingLocation,Fixture=@Fixture,FixtureLocation=@FixtureLocation,Template=@Template,TemplateLocation=@TemplateLocation,BP1083=@BP1083,BP1155=@BP1155,BP1158=@BP1158,BP1175=@BP1175,BP1176=@BP1176 WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SQLRemoveCommand = "DELETE FROM [dbo].[Paccar_Item_Data] WHERE ItemID=@ItemID AND Sequence=@Sequence";
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string PaccarSelect = "SELECT * FROM [dbo].[Paccar_Item_Data] ORDER BY JobID DESC";
                SqlDataAdapter PaccarDataAdapter = new SqlDataAdapter(PaccarSelect, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(PaccarDataAdapter);
                DataSet PaccarData = new DataSet();
                PaccarDataAdapter.Fill(PaccarData);
                JobListGridView.DataSource = PaccarData.Tables[0];
            }
            Row_TotalCount_SQL();
            ClearForm();
            GroupBoxControlStart();
            FirstLoadTest();
        }
        
        private void BrakePress_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BrakePress_ComboBox.Text == "1107")
            {

            }
            else if (BrakePress_ComboBox.Text == "1139")
            {

            }
            else if (BrakePress_ComboBox.Text == "1178")
            {

            }
            else if (BrakePress_ComboBox.Text == "1065")
            {
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string Navistar1065 = "SELECT * FROM [dbo].[BP1065]";
                SqlDataAdapter Data1065 = new SqlDataAdapter(Navistar1065, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(Data1065);
                DataSet Data = new DataSet();
                Data1065.Fill(Data);
                JobListGridView.DataSource = Data.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1108")
            {

            }
            else if (BrakePress_ComboBox.Text == "1156")
            {

            }
            else if (BrakePress_ComboBox.Text == "1720")
            {

            }
            else if (BrakePress_ComboBox.Text == "1176")
            {
                // Connect to SQL DataTable and Load 
                /*
                SqlConnection BrakePressConnect = new SqlConnection(SQL_Source);
                string Paccar1176 = "SELECT * FROM [dbo].[Paccar_Item_Data]";
                SqlDataAdapter Data1176 = new SqlDataAdapter(Paccar1176, BrakePressConnect);
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(Data1176);
                DataSet PaccarData = new DataSet();
                Data1176.Fill(PaccarData);
                JobListGridView.DataSource = PaccarData.Tables[0];
                */
            }
            else if (BrakePress_ComboBox.Text == "1083")
            {

            }
            else if (BrakePress_ComboBox.Text == "1155")
            {

            }
            else if (BrakePress_ComboBox.Text == "1158")
            {

            }
            else if (BrakePress_ComboBox.Text == "1175")
            {

            }
        }

        /*********************************************************************************************************************
        * 
        * ComboBox Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * GridView Region Start
        * 
        * - JobListGridView CellClick
        * - JobListGridView DataError
        * 
        *********************************************************************************************************************/
        #region

        private void JobListGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (CustomerCell_ComboBox.Text == "CAT")
                {
                    DataGridViewRow Row = JobListGridView.Rows[e.RowIndex];
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1107_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1139_ComboBox.Text = Row.Cells[20].Value.ToString();
                    BP1177_ComboBox.Text = Row.Cells[21].Value.ToString();
                }
                else if (CustomerCell_ComboBox.Text == "John Deere")
                {
                    DataGridViewRow Row = JobListGridView.Rows[e.RowIndex];
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1127_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1178_ComboBox.Text = Row.Cells[20].Value.ToString();
                }
                else if (CustomerCell_ComboBox.Text == "Navistar")
                {
                    DataGridViewRow Row = JobListGridView.Rows[e.RowIndex];
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1065_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1108_ComboBox.Text = Row.Cells[20].Value.ToString();
                    BP1156_ComboBox.Text = Row.Cells[21].Value.ToString();
                    BP1720_ComboBox.Text = Row.Cells[22].Value.ToString();
                }
                else if(CustomerCell_ComboBox.Text == "Paccar")
                {
                    DataGridViewRow Row = JobListGridView.Rows[e.RowIndex];
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1083_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1155_ComboBox.Text = Row.Cells[20].Value.ToString();
                    BP1158_ComboBox.Text = Row.Cells[21].Value.ToString();
                    BP1175_ComboBox.Text = Row.Cells[22].Value.ToString();
                    BP1176_ComboBox.Text = Row.Cells[23].Value.ToString();
                }
                DateParse();
                //FindItemImage(null,null);             
                if (PartImage.IsBusy != true)
                {
                    PartImage.RunWorkerAsync();
                }
                JobListGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                GroupBoxControlStart();
            }
        }

        private void JobListGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                if (CustomerCell_ComboBox.Text == "CAT")
                {
                    DataGridViewRow Row = JobListGridView.CurrentRow;
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1107_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1139_ComboBox.Text = Row.Cells[20].Value.ToString();
                    BP1177_ComboBox.Text = Row.Cells[21].Value.ToString();
                }
                else if (CustomerCell_ComboBox.Text == "John Deere")
                {
                    DataGridViewRow Row = JobListGridView.CurrentRow;
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1127_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1178_ComboBox.Text = Row.Cells[20].Value.ToString();
                }
                else if (CustomerCell_ComboBox.Text == "Navistar")
                {
                    DataGridViewRow Row = JobListGridView.CurrentRow;
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1065_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1108_ComboBox.Text = Row.Cells[20].Value.ToString();
                    BP1156_ComboBox.Text = Row.Cells[21].Value.ToString();
                    BP1720_ComboBox.Text = Row.Cells[22].Value.ToString();
                }
                else if (CustomerCell_ComboBox.Text == "Paccar")
                {
                    DataGridViewRow Row = JobListGridView.CurrentRow;
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                    Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                    Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                    StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                    Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                    ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    Template_TextBox.Text = Row.Cells[17].Value.ToString();
                    TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                    BP1083_ComboBox.Text = Row.Cells[19].Value.ToString();
                    BP1155_ComboBox.Text = Row.Cells[20].Value.ToString();
                    BP1158_ComboBox.Text = Row.Cells[21].Value.ToString();
                    BP1175_ComboBox.Text = Row.Cells[22].Value.ToString();
                    BP1176_ComboBox.Text = Row.Cells[23].Value.ToString();
                }
                DateParse();
                //FindItemImage(null,null);             
                if (PartImage.IsBusy != true)
                {
                    PartImage.RunWorkerAsync();
                }
                JobListGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                GroupBoxControlStart();
            }
        }

        private void JobListGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        /*********************************************************************************************************************
        * 
        * GridView Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Methods Region Start 
        * -- Inner Regions: 3
        * 
        * - Standard 
        * - Excel Creation
        * - Import Operation
        * 
        * - SearchForItemID
        * - SearchItemIDOperation
        * - ShowComponents
        * - OperationInitialize
        * - OperationOEECalculation
        * - FindTotalRunTime
        * - ItemOperationCalculation
        * - RunningStatistics
        * - LivePPMOEECalculation
        * - PDFFileCheck
        * - SpotWeldID
        * - ClearForm
        * - PassOperationValue
        * - PassReferenceNumber
        * - PassValue
        * 
        **********************************************************************************************************************/
        #region

        /*********************************************************************************************************************
        * 
        * Standard Region Start
        * 
        * - 
        * - 
        * 
        *********************************************************************************************************************/
        #region

        private void ClearForm()
        {
            // Item Information GroupBox
            Customer_ComboBox.Text = "";
            Sequence_ComboBox.Text = "";

            ItemID_TextBox.Clear();
            JobID_TextBox.Clear();
            Customer_TextBox.Clear();
            CustomerItemID_TextBox.Clear();
            Sequence_TextBox.Clear();
            Saving_Label.Hide();

            // Folder Items GroupBox
            Tooling_TextBox.Clear();
            ToolingLocation_TextBox.Clear();
            Fixture_TextBox.Clear();
            FixtureLocation_TextBox.Clear();
            Template_TextBox.Clear();
            TemplateLocation_TextBox.Clear();

            // Vision System GroupBox
            Steps_ComboBox.Text = "";

            CompletionDate_TextBox.Clear();
            Steps_TextBox.Clear();
            StepsUsed_TextBox.Clear();

            // Item Statistics GroupBox
            TotalRuns_TextBox.Clear();
            PartsManufactured_TextBox.Clear();
            SetupTime_TextBox.Clear();
            PPM_TextBox.Clear();

            // 3D Scanner GroupBox
            Sample_ComboBox.Text = "";
            ProductionReady_ComboBox.Text = "";

            Part_PictureBox.Image = null;
            Refresh_SQL();

            // Brake Press Program GroupBox
            if (CustomerCell_ComboBox.Text == "CAT")
            {
                BP1107_ComboBox.Text = "";
                BP1139_ComboBox.Text = "";
                BP1177_ComboBox.Text = "";
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                BP1127_ComboBox.Text = "";
                BP1178_ComboBox.Text = "";
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                BP1065_ComboBox.Text = "";
                BP1108_ComboBox.Text = "";
                BP1156_ComboBox.Text = "";
                BP1720_ComboBox.Text = "";
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                BP1083_ComboBox.Text = "";
                BP1155_ComboBox.Text = "";
                BP1158_ComboBox.Text = "";
                BP1175_ComboBox.Text = "";
                BP1176_ComboBox.Text = "";
            }
        }

        private void SearchForItem()
        {
            bool found = false;
            if (SearchItemID_CheckBox.Checked == true)
            {
                SearchJobID_CheckBox.Checked = false;
                SearchValue = ItemID_TextBox.Text;
                SearchColumn = 0;
            }
            else if (SearchJobID_CheckBox.Checked == true)
            {
                SearchItemID_CheckBox.Checked = false;
                SearchValue = JobID_TextBox.Text;
                SearchColumn = 3;
            }
            JobListGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow Row in JobListGridView.Rows)
                {
                    Row.Selected = false;
                    if (Row.Cells[SearchColumn].Value.ToString().Equals(SearchValue))
                    {
                        Row.Selected = true;
                        //
                        if (CustomerCell_ComboBox.Text == "CAT")
                        {
                            ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                            Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                            Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                            CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                            JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                            Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                            Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                            Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                            StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                            Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                            ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                            TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                            PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                            PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                            SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                            Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                            ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                            Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                            FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                            Template_TextBox.Text = Row.Cells[17].Value.ToString();
                            TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                            BP1107_ComboBox.Text = Row.Cells[19].Value.ToString();
                            BP1139_ComboBox.Text = Row.Cells[20].Value.ToString();
                            BP1177_ComboBox.Text = Row.Cells[21].Value.ToString();
                        }
                        else if (CustomerCell_ComboBox.Text == "John Deere")
                        {
                            ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                            Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                            Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                            CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                            JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                            Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                            Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                            Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                            StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                            Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                            ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                            TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                            PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                            PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                            SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                            Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                            ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                            Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                            FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                            Template_TextBox.Text = Row.Cells[17].Value.ToString();
                            TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                            BP1127_ComboBox.Text = Row.Cells[19].Value.ToString();
                            BP1178_ComboBox.Text = Row.Cells[20].Value.ToString();
                        }
                        else if (CustomerCell_ComboBox.Text == "Navistar")
                        {
                            ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                            Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                            Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                            CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                            JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                            Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                            Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                            Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                            StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                            Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                            ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                            TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                            PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                            PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                            SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                            Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                            ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                            Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                            FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                            Template_TextBox.Text = Row.Cells[17].Value.ToString();
                            TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                            BP1065_ComboBox.Text = Row.Cells[19].Value.ToString();
                            BP1108_ComboBox.Text = Row.Cells[20].Value.ToString();
                            BP1156_ComboBox.Text = Row.Cells[21].Value.ToString();
                            BP1720_ComboBox.Text = Row.Cells[22].Value.ToString();
                        }
                        else if (CustomerCell_ComboBox.Text == "Paccar")
                        {
                            ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                            Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                            Customer_ComboBox.Text = Row.Cells[1].Value.ToString();
                            CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                            JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                            Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                            Sequence_ComboBox.Text = Row.Cells[4].Value.ToString();
                            Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                            StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                            Sample_ComboBox.Text = Row.Cells[7].Value.ToString();
                            ProductionReady_ComboBox.Text = Row.Cells[8].Value.ToString();
                            TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                            PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                            PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                            SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                            Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                            ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                            Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                            FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                            Template_TextBox.Text = Row.Cells[17].Value.ToString();
                            TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();
                            BP1083_ComboBox.Text = Row.Cells[19].Value.ToString();
                            BP1155_ComboBox.Text = Row.Cells[20].Value.ToString();
                            BP1158_ComboBox.Text = Row.Cells[21].Value.ToString();
                            BP1175_ComboBox.Text = Row.Cells[22].Value.ToString();
                            BP1176_ComboBox.Text = Row.Cells[23].Value.ToString();
                        }
                        DateParse();
                        //FindItemImage(null,null);
                        if (PartImage.IsBusy != true)
                        {
                            PartImage.RunWorkerAsync();
                        }
                        JobListGridView.FirstDisplayedScrollingRowIndex = JobListGridView.SelectedRows[0].Index;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                if (found == false)
                {
                    MessageBox.Show("Item ID: " + SearchValue + " Was Not Found");
                }
                else if (SearchColumn == 3)
                {
                    MessageBox.Show("Brake Press Vision Job ID: " + SearchValue + " Was Not Found");
                }
            }
        }

        private void ConfirmFinished()
        {
            AddJob_Button.Show();
            EditJob_Button.Show();
            RemoveJob_Button.Show();
            Exit_Button.Show();
            Confirm_Button.Hide();
            Cancel_Button.Hide();
            Customer_ComboBox.Hide();
            Steps_ComboBox.Hide();
            DateOfCompletionPicker.Hide();
            CompletionDate_TextBox.Show();
            Steps_TextBox.Show();
            SearchItemID_CheckBox.Show();
            SearchJobID_CheckBox.Show();
            Customer_TextBox.Show();
            AddJob_ButtonWasClicked = false;
            EditJob_ButtonWasClicked = false;
            RemoveJob_ButtonWasClicked = false;
            ItemID_TextBox.ReadOnly = false;
            ClearForm();
            Refresh_SQL();
        }

        private void DateParse()
        {
            string DateValue = CompletionDate_TextBox.Text;
            DateValue = DateValue.Replace(" 12:00:00 AM", "");
            CompletionDate_TextBox.Text = DateValue;
        }

        private void PartImage_RunWorker(object sender, EventArgs e)
        {
            ItemID = ItemID_TextBox.Text;
            ItemIDSplit = ItemID.Split('-');
            ItemID_Three = double.Parse(ItemIDSplit[0]);
            ItemID_Five = double.Parse(ItemIDSplit[1]);

            if (ItemID_Five >= 1 && ItemID_Five <= 10000)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\1-9999\";
            }
            else if (ItemID_Five >= 10000 && ItemID_Five <= 14999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\10000-14999\";
            }
            else if (ItemID_Five >= 15000 && ItemID_Five <= 19999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\15000-19999\";
            }
            else if (ItemID_Five >= 20000 && ItemID_Five <= 24999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\20000-24999\";
            }
            else if (ItemID_Five >= 25000 && ItemID_Five <= 29999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\25000-29999\";
            }
            else if (ItemID_Five >= 30000 && ItemID_Five <= 34999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\30000-34999\";
            }
            else if (ItemID_Five >= 35000 && ItemID_Five <= 39999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\35000-39999\";
            }
            else if (ItemID_Five >= 40000 && ItemID_Five <= 44999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\40000-44999\";
            }
            else if (ItemID_Five >= 45000 && ItemID_Five <= 49999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\45000-49999\";
            }
            else if (ItemID_Five >= 50000 && ItemID_Five <= 54999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\50000-54999\";
            }
            else if (ItemID_Five >= 55000 && ItemID_Five <= 59999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\55000-59999\";
            }
            else if (ItemID_Five >= 60000 && ItemID_Five <= 69999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\60000-69999\";
            }
            else if (ItemID_Five >= 70000 && ItemID_Five <= 79999)
            {
                ItemImagePath = @"\\insidedmp.com\Corporate\OH\OH Common\Part Pictures\70000-79999\";
            }

            string ImageName = ItemID + ".JPG";

            try
            {
                if (File.Exists(ItemImagePath + ImageName))
                {
                    Part_PictureBox.Image = Image.FromFile(ItemImagePath + ImageName);
                    Part_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else if (File.Exists(ItemImagePath + ItemIDSplit[1] + ".JPG"))
                {
                    Part_PictureBox.Image = Image.FromFile(ItemImagePath + ItemIDSplit[1] + ".JPG");
                    Part_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                }
                else if (File.Exists(ItemImagePath + ItemIDSplit[0] + '-' + ItemIDSplit[1] + ".JPG"))
                {
                    Part_PictureBox.Image = Image.FromFile(ItemImagePath + ItemIDSplit[0] + '-' + ItemIDSplit[1] + ".JPG");
                    Part_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                }
                else
                {
                    Part_PictureBox.Image = null;
                }
            }
            catch
            {
                MessageBox.Show("Error Finding Image");
            }
        }

        void PartImage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void JobHideShow()
        {
            //Confirm_Button.Show();
            //Cancel_Button.Show();
            DateOfCompletionPicker.Show();
            Customer_ComboBox.Show();
            Steps_ComboBox.Show();
            Sequence_ComboBox.Show();
            SearchItemID_CheckBox.Hide();
            SearchJobID_CheckBox.Hide();
            CompletionDate_TextBox.Hide();
            //Exit_Button.Hide();
            Steps_TextBox.Hide();
            Customer_TextBox.Hide();
            Sequence_TextBox.Hide();
        }

        private void GroupBoxControl_AddJob()
        {
            ItemID_TextBox.ReadOnly = false;
            JobID_TextBox.ReadOnly = false;
            Customer_TextBox.ReadOnly = false;
            CustomerItemID_TextBox.ReadOnly = false;
            Tooling_TextBox.ReadOnly = false;
            ToolingLocation_TextBox.ReadOnly = false;
            Fixture_TextBox.ReadOnly = false;
            FixtureLocation_TextBox.ReadOnly = false;
            Template_TextBox.ReadOnly = false;
            TemplateLocation_TextBox.ReadOnly = false;
            TotalRuns_TextBox.ReadOnly = false;
            PartsManufactured_TextBox.ReadOnly = false;
            SetupTime_TextBox.ReadOnly = false;
            PPM_TextBox.ReadOnly = false;
            CompletionDate_TextBox.ReadOnly = false;
            Steps_TextBox.ReadOnly = false;
            StepsUsed_TextBox.ReadOnly = false;
            Sequence_ComboBox.Enabled = true;

            // Buttons Enabled
            Search_Button.Enabled = false;
            Clear_Button.Enabled = false;
            Refresh_Button.Enabled = false;
            Exit_Button.Enabled = false;
            // AddJob_Button.Enabled = false;
            EditJob_Button.Enabled = false;
            RemoveJob_Button.Enabled = false;
            DatabaseImport_Button.Enabled = false;
            // Buttons Visible
            Confirm_Button.Visible = true;
            Cancel_Button.Visible = true;


            ProductionReady_ComboBox.Enabled = true;
            Sample_ComboBox.Enabled = true;
            DateOfCompletionPicker.Enabled = true;
            Steps_ComboBox.Enabled = true;

            if (CustomerCell_ComboBox.Text == "CAT")
            {
                BP1107_ComboBox.Enabled = true;
                BP1139_ComboBox.Enabled = true;
                BP1177_ComboBox.Enabled = true;
                BP1107_ComboBox.Text = "No";
                BP1139_ComboBox.Text = "No";
                BP1177_ComboBox.Text = "No";
                Template_TextBox.Text = "N/A";
                TemplateLocation_TextBox.Text = "N/A";
                Sample_ComboBox.Text = "No";
                ProductionReady_ComboBox.Text = "No";
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                BP1127_ComboBox.Enabled = true;
                BP1178_ComboBox.Enabled = true;
                BP1127_ComboBox.Text = "No";
                BP1178_ComboBox.Text = "No";
                Template_TextBox.Text = "N/A";
                TemplateLocation_TextBox.Text = "N/A";
                Sample_ComboBox.Text = "No";
                ProductionReady_ComboBox.Text = "No";
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                BP1065_ComboBox.Enabled = true;
                BP1108_ComboBox.Enabled = true;
                BP1156_ComboBox.Enabled = true;
                BP1720_ComboBox.Enabled = true;
                BP1065_ComboBox.Text = "No";
                BP1108_ComboBox.Text = "No";
                BP1156_ComboBox.Text = "No";
                BP1720_ComboBox.Text = "No";
                Template_TextBox.Text = "N/A";
                TemplateLocation_TextBox.Text = "N/A";
                Sample_ComboBox.Text = "No";
                ProductionReady_ComboBox.Text = "No";
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                BP1083_ComboBox.Enabled = true;
                BP1155_ComboBox.Enabled = true;
                BP1158_ComboBox.Enabled = true;
                BP1175_ComboBox.Enabled = true;
                BP1176_ComboBox.Enabled = true;
                BP1083_ComboBox.Text = "No";
                BP1155_ComboBox.Text = "No";
                BP1158_ComboBox.Text = "No";
                BP1175_ComboBox.Text = "No";
                BP1176_ComboBox.Text = "No";
                Template_TextBox.Text = "N/A";
                TemplateLocation_TextBox.Text = "N/A";
                Sample_ComboBox.Text = "No";
                ProductionReady_ComboBox.Text = "No";
            }
        }

        private void ButtonsControl_Edit()
        {
            // Buttons Enabled
            Search_Button.Enabled = false;
            Clear_Button.Enabled = false;
            Refresh_Button.Enabled = false;
            Exit_Button.Enabled = false;
            AddJob_Button.Enabled = false;
            // EditJob_Button.Enabled = false;
            RemoveJob_Button.Enabled = false;
            DatabaseImport_Button.Enabled = false;
            // Buttons Visible
            Confirm_Button.Visible = true;
            Cancel_Button.Visible = true;
        }

        private void ButtonsControl_Cancel()
        {
            // Buttons Enabled
            Search_Button.Enabled = true;
            Clear_Button.Enabled = true;
            Refresh_Button.Enabled = true;
            Exit_Button.Enabled = true;
            AddJob_Button.Enabled = true;
            EditJob_Button.Enabled = true;
            RemoveJob_Button.Enabled = true;
            DatabaseImport_Button.Enabled = true;
            // Buttons Visible
            Confirm_Button.Visible = false;
            Cancel_Button.Visible = false;
        }

        private void GroupBoxControlActive()
        {
            ItemID_TextBox.ReadOnly = false;
            JobID_TextBox.ReadOnly = false;
            Customer_TextBox.ReadOnly = false;
            CustomerItemID_TextBox.ReadOnly = false;
            Tooling_TextBox.ReadOnly = false;
            ToolingLocation_TextBox.ReadOnly = false;
            Fixture_TextBox.ReadOnly = false;
            FixtureLocation_TextBox.ReadOnly = false;
            Template_TextBox.ReadOnly = false;
            TemplateLocation_TextBox.ReadOnly = false;
            TotalRuns_TextBox.ReadOnly = false;
            PartsManufactured_TextBox.ReadOnly = false;
            SetupTime_TextBox.ReadOnly = false;
            PPM_TextBox.ReadOnly = false;
            CompletionDate_TextBox.ReadOnly = false;
            Steps_TextBox.ReadOnly = false;
            StepsUsed_TextBox.ReadOnly = false;
            Sequence_ComboBox.Enabled = true;

            //Clear_Button.Enabled = false;

            ProductionReady_ComboBox.Enabled = true;
            Sample_ComboBox.Enabled = true;
            DateOfCompletionPicker.Enabled = true;
            Steps_ComboBox.Enabled = true;

            if (CustomerCell_ComboBox.Text == "CAT")
            {
                BP1107_ComboBox.Enabled = true;
                BP1139_ComboBox.Enabled = true;
                BP1177_ComboBox.Enabled = true;
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                BP1127_ComboBox.Enabled = true;
                BP1178_ComboBox.Enabled = true;
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                BP1065_ComboBox.Enabled = true;
                BP1108_ComboBox.Enabled = true;
                BP1156_ComboBox.Enabled = true;
                BP1720_ComboBox.Enabled = true;
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                BP1083_ComboBox.Enabled = true;
                BP1155_ComboBox.Enabled = true;
                BP1158_ComboBox.Enabled = true;
                BP1175_ComboBox.Enabled = true;
                BP1176_ComboBox.Enabled = true;
            }
        }

        private void GroupBoxControlInactive()
        {
            ItemID_TextBox.ReadOnly = true;
            JobID_TextBox.ReadOnly = true;
            Customer_TextBox.ReadOnly = true;
            CustomerItemID_TextBox.ReadOnly = true;
            Tooling_TextBox.ReadOnly = true;
            ToolingLocation_TextBox.ReadOnly = true;
            Fixture_TextBox.ReadOnly = true;
            FixtureLocation_TextBox.ReadOnly = true;
            Template_TextBox.ReadOnly = true;
            TemplateLocation_TextBox.ReadOnly = true;
            TotalRuns_TextBox.ReadOnly = true;
            PartsManufactured_TextBox.ReadOnly = true;
            SetupTime_TextBox.ReadOnly = true;
            PPM_TextBox.ReadOnly = true;
            CompletionDate_TextBox.ReadOnly = true;
            Steps_TextBox.ReadOnly = true;
            StepsUsed_TextBox.ReadOnly = true;
            Sequence_ComboBox.Enabled = false;

            ProductionReady_ComboBox.Enabled = false;
            Sample_ComboBox.Enabled = false;
            DateOfCompletionPicker.Enabled = false;
            Steps_ComboBox.Enabled = false;

            if (CustomerCell_ComboBox.Text == "CAT")
            {
                BP1107_ComboBox.Enabled = false;
                BP1139_ComboBox.Enabled = false;
                BP1177_ComboBox.Enabled = false;
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                BP1127_ComboBox.Enabled = false;
                BP1178_ComboBox.Enabled = false;
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                BP1065_ComboBox.Enabled = false;
                BP1108_ComboBox.Enabled = false;
                BP1156_ComboBox.Enabled = false;
                BP1720_ComboBox.Enabled = false;
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                BP1083_ComboBox.Enabled = false;
                BP1155_ComboBox.Enabled = false;
                BP1158_ComboBox.Enabled = false;
                BP1175_ComboBox.Enabled = false;
                BP1176_ComboBox.Enabled = false;
            }
        }

        private void GroupBoxControlStart()
        {
            // GroupBox and TextBox 
            // Item Information GroupBox
            ItemID_TextBox.ReadOnly = false;
            JobID_TextBox.ReadOnly = false;
            Customer_TextBox.ReadOnly = true;
            CustomerItemID_TextBox.ReadOnly = true;
            Sequence_ComboBox.Enabled = false;

            // Folder Items GroupBox
            Tooling_TextBox.ReadOnly = true;
            ToolingLocation_TextBox.ReadOnly = true;
            Fixture_TextBox.ReadOnly = true;
            FixtureLocation_TextBox.ReadOnly = true;
            Template_TextBox.ReadOnly = true;
            TemplateLocation_TextBox.ReadOnly = true;

            // Vision System GroupBox
            DateOfCompletionPicker.Enabled = false;
            CompletionDate_TextBox.ReadOnly = true;
            Steps_ComboBox.Enabled = false;
            Steps_TextBox.ReadOnly = true;
            StepsUsed_TextBox.ReadOnly = true;

            // Item Statistics GroupBox
            TotalRuns_TextBox.ReadOnly = true;
            PartsManufactured_TextBox.ReadOnly = true;
            SetupTime_TextBox.ReadOnly = true;
            PPM_TextBox.ReadOnly = true;

            // 3D Scanner GroupBox
            ProductionReady_ComboBox.Enabled = false;
            Sample_ComboBox.Enabled = false;

            // Brake Press GroupBoxes
            if (CustomerCell_ComboBox.Text == "CAT")
            {
                BP1107_ComboBox.Enabled = false;
                BP1139_ComboBox.Enabled = false;
                BP1177_ComboBox.Enabled = false;
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                BP1127_ComboBox.Enabled = false;
                BP1178_ComboBox.Enabled = false;
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                BP1065_ComboBox.Enabled = false;
                BP1108_ComboBox.Enabled = false;
                BP1156_ComboBox.Enabled = false;
                BP1720_ComboBox.Enabled = false;
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                BP1083_ComboBox.Enabled = false;
                BP1155_ComboBox.Enabled = false;
                BP1158_ComboBox.Enabled = false;
                BP1175_ComboBox.Enabled = false;
                BP1176_ComboBox.Enabled = false;
            }

            // Button
            Clear_Button.Enabled = true;
        }

        /*********************************************************************************************************************
        * 
        * Standard Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Excel Region Start
        * 
        * - 
        * - 
        * 
        *********************************************************************************************************************/
        #region


        /*********************************************************************************************************************
        * 
        * Excel Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Import Region Start
        * 
        * - 
        * - 
        * 
        *********************************************************************************************************************/
        #region


        /*********************************************************************************************************************
        * 
        * Import Region End
        * 
        *********************************************************************************************************************/
        #endregion

        private void CreateExcelFile(object sender, EventArgs e)
        {
            string ReportName = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Millisecond + "_";
            ReportName = ReportName.Replace("/", "_");
            ReportName = ReportName.Replace(":", "_");

            string ExcelFileLocation2 = "";

            // Excel Initialize
            ReportApp = new Excel.Application();
            ReportApp.Visible = false;
            ReportWB = (Excel._Workbook)(ReportApp.Workbooks.Add(""));
            ReportWS = (Excel._Worksheet)ReportWB.ActiveSheet;

            string[] ColumnNames = new string[JobListGridView.Columns.Count];
            int ExcelColumns = 1;

            foreach (DataGridViewColumn dc in JobListGridView.Columns)
            {
                ReportWS.Cells[1, ExcelColumns] = dc.Name;
                ExcelColumns++;
            }


            if (ReportCell == "CAT")
            {
                ReportWS.get_Range("A1", "V1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "V1");
                ReportWS.get_Range("A1", "V1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "V" + RowCount.ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in ReportRange.Cells)
                {
                    cell.BorderAround2();
                }
            }
            else if (ReportCell == "John Deere")
            {
                ReportWS.get_Range("A1", "U1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "U1");
                ReportWS.get_Range("A1", "U1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "U" + RowCount.ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in ReportRange.Cells)
                {
                    cell.BorderAround2();
                }
            }
            else if (ReportCell == "Navistar")
            {
                ReportWS.get_Range("A1", "W1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "W1");
                ReportWS.get_Range("A1", "W1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "W" + RowCount.ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in ReportRange.Cells)
                {
                    cell.BorderAround2();
                }
            }
            else if (ReportCell == "Paccar")
            {
                ReportWS.get_Range("A1", "X1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "X1");
                ReportWS.get_Range("A1", "X1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "X" + RowCount.ToString());
            }

            for (int i = 0; i < ReportDataSet.Tables[0].Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < ReportDataSet.Tables[0].Columns.Count; j++)
                {
                    ReportWS.Cells[(i + 2), (j + 1)] = ReportDataSet.Tables[0].Rows[i][j];
                    CreateExcel.ReportProgress(i);
                }
            }
            ReportRange = ReportWS.get_Range("A1", "X1");
            ReportRange.EntireColumn.AutoFit();
            string ReportPDFName = ReportCell + "_Brake_Press_SQL" + ".xlsx";

            /*
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
            */
            ExcelFileLocation = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Job List\SQL Data Tables\" + ReportPDFName;
            if (File.Exists(ExcelFileLocation))
            {
                bool tryAgain = true;
                try
                {
                    File.Delete(ExcelFileLocation);
                }
                catch (IOException)
                {
                    tryAgain = false;
                }
                if (tryAgain == false)
                {
                    ExcelFileLocation = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Job List\SQL Data Tables\" + ReportName + "_" + ReportPDFName;
                }
            }

            ReportWS.SaveAs(ExcelFileLocation, Excel.XlFileFormat.xlOpenXMLWorkbook, null, null, false, false, false, false, false, false);
            //ReportWS.SaveAs(ExcelFileLocation);
            ReportWB.Close();
        }

        private void CreateExcelFile_RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            // Process.Start(ExcelFileLocation);
            // MessageBox.Show("Saved");
            //Saving_Label.Text = "Saved";
            Saving_ProgressBar.Value = 0;
            Saving_ProgressBar.Hide();
            Saving_Label.Hide();
            ChangeCell_Button.Enabled = true;
            Search_Button.Enabled = true;
            Clear_Button.Enabled = true;
            Refresh_Button.Enabled = true;
            AddJob_Button.Enabled = true;
            EditJob_Button.Enabled = true;
            RemoveJob_Button.Enabled = true;
            DatabaseImport_Button.Enabled = true;

            Exit_Button.Enabled = true;
        }

        private void CreateExcelFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           Saving_ProgressBar.Value = e.ProgressPercentage;
        }

        private void CreateExcel_Start()
        {
            Clear_Button.Enabled = false;
            ChangeCell_Button.Enabled = false;
            Exit_Button.Enabled = false;
            Refresh_Button.Enabled = false;
            Saving_Label.Show();
            Saving_Label.Text = "Saving";
            Saving_ProgressBar.Show();
        }

        private void CreateExcel_End()
        {
            Saving_Label.Hide();
            Saving_ProgressBar.Hide();
        }


        /*********************************************************************************************************************
        * 
        * Methods Region End
        * 
        **********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        *  
        *  SQL Data Region Start
        *  -- Total: 3
        *  
        *  - ItemOperationDataStart_SQL
        *  - ItemOperationDataEnd_SQL
        *  - ItemRunCounter_SQL
        *  - OperationIDCounter_SQL
        *  - OperationDataStart_SQL
        *  - OperationDataEnd_SQL
        *  - OperationOEEData_SQL
        *  - ProgramListUpdate_SQL
        *  - RefreshItemData_SQL
        *  - ScanOutCompletion_SQL
        *  - EmployeeLogOff_SQL  
        * 
        *********************************************************************************************************************/
        #region

        private void EmployeeLogOff_SQL()
        {
            SqlConnection JobListLogoff = new SqlConnection(SQL_Source);
            SqlCommand Logoff = new SqlCommand();
            Logoff.CommandType = System.Data.CommandType.Text;
            Logoff.CommandText = "UPDATE [dbo].[LoginData] SET LogoutDateTime=@LogoutDateTime WHERE LoginDateTime=@LoginDateTime";
            Logoff.Connection = JobListLogoff;
            Logoff.Parameters.AddWithValue("@LoginDateTime", LoginTime.ToString());
            Logoff.Parameters.AddWithValue("@LogoutDateTime", Clock_TextBox.Text);
            JobListLogoff.Open();
            Logoff.ExecuteNonQuery();
            JobListLogoff.Close();
        }

        private void Refresh_SQL()
        {
            SqlConnection connection = new SqlConnection(SQL_Source);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(Refresh_Data, connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            ReportDataSet = new DataSet();
            dataAdapter.Fill(ReportDataSet);
            JobListGridView.DataSource = ReportDataSet.Tables[0];
        }

        private void Row_TotalCount_SQL()
        {
            if (CustomerCell_ComboBox.Text == "CAT")
            {
                RowCountString = "SELECT COUNT(*) FROM [dbo].[CAT_Item_Data]";
            }
            if (CustomerCell_ComboBox.Text == "John Deere")
            {
                RowCountString = "SELECT COUNT(*) FROM [dbo].[JohnDeere_Item_Data]";
            }
            if (CustomerCell_ComboBox.Text == "Navistar")
            {
                RowCountString = "SELECT COUNT(*) FROM [dbo].[Navistar_Item_Data]";
            }
            if (CustomerCell_ComboBox.Text == "Paccar")
            {
                RowCountString = "SELECT COUNT(*) FROM [dbo].[Paccar_Item_Data]";
            }
            try
            {
                string CheckCardCountString = RowCountString;
                SqlConnection CheckCountTotalConnection = new SqlConnection(SQL_Source);
                SqlCommand CheckCountTotalCommand = new SqlCommand(CheckCardCountString, CheckCountTotalConnection);
                CheckCountTotalConnection.Open();
                int CheckCardCountOperationTotal = (int)CheckCountTotalCommand.ExecuteScalar();
                CheckCountTotalConnection.Close();
                RowCount = CheckCardCountOperationTotal + 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            Saving_ProgressBar.Maximum = RowCount + 1;
        }

        /*********************************************************************************************************************
        * 
        * SQL Data Region End
        * 
        **********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Events Region Start
        * -- Inner Regions: 3
        * 
        * - Timer Region
        * - TextBox Method Region
        * - TextBox Enter Region 
        * 
        *********************************************************************************************************************/
        #region 

        /*********************************************************************************************************************
        * 
        * Events Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Methods in Testing Start
        * 
        *********************************************************************************************************************/
        #region

        private void DatabaseImport_Button_Click(object sender, EventArgs e)
        {
            DataSet ImportResult;
            IExcelDataReader ExcelReader;
            OpenFileDialog OpenExcelFile = new OpenFileDialog();
            try
            {
                OpenExcelFile.FileName = CustomerCell_ComboBox.Text + "*";
                if (OpenExcelFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileStream ExcelFileStream = File.Open(OpenExcelFile.FileName, FileMode.Open, FileAccess.Read);
                    ExcelReader = ExcelReaderFactory.CreateOpenXmlReader(ExcelFileStream);
                    ImportResult = ExcelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                            ReadHeaderRow = (rowReader) => {
                            }
                        }
                    });
                    ExcelReader.Close();
                    JobListGridView.DataSource = ImportResult.Tables["Sheet1"];
                    //SaveImport_Button.Show();
                    //CancelImport_Button.Show();
                    //DatabaseImport_Button.Hide();
                    DatabaseImportStart_Controls();
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File: " + OpenExcelFile.FileName.ToString() + " is \ncurrently opened by another user and must be closed before import");
            }
            SecondLoadTest();
        }

        private void DatabaseImportStart_Controls()
        {
            SaveImport_Button.Show();
            CancelImport_Button.Show();
            DatabaseImport_Button.Enabled = false;

            ChangeCell_Button.Enabled = false;
            Search_Button.Enabled = false;
            Clear_Button.Enabled = false;
            Refresh_Button.Enabled = false;
            AddJob_Button.Enabled = false;
            EditJob_Button.Enabled = false;
            RemoveJob_Button.Enabled = false;
            Exit_Button.Enabled = false;
        }

        private void DatabaseImportEnd_Controls()
        {
            SaveImport_Button.Hide();
            CancelImport_Button.Hide();
            DatabaseImport_Button.Enabled = true;

            ChangeCell_Button.Enabled = true;
            Search_Button.Enabled = true;
            Clear_Button.Enabled = true;
            Refresh_Button.Enabled = true;
            AddJob_Button.Enabled = true;
            EditJob_Button.Enabled = true;
            RemoveJob_Button.Enabled = true;
            Exit_Button.Enabled = true;
        }

        private void FirstLoadTest()
        {
            try
            {
                string CheckCardCountString = RowCountString;
                SqlConnection CheckCountTotalConnection = new SqlConnection(SQL_Source);
                SqlCommand CheckCountTotalCommand = new SqlCommand(CheckCardCountString, CheckCountTotalConnection);
                CheckCountTotalConnection.Open();
                int CheckCardCountOperationTotal = (int)CheckCountTotalCommand.ExecuteScalar();
                CheckCountTotalConnection.Close();
                RowCount = CheckCardCountOperationTotal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            Array.Clear(ItemID_CurrentArray, 0, RowCount);
            Array.Clear(JobID_CurrentArray, 0, RowCount);
            Array.Clear(Sequence_CurrentArray, 0, RowCount);
            ImportCurrentRows = 0;
            foreach (DataGridViewRow Row in JobListGridView.Rows)
            {
                ItemID_CurrentArray[ImportCurrentRows] = Row.Cells[0].Value.ToString();
                JobID_CurrentArray[ImportCurrentRows] = Row.Cells[3].Value.ToString();
                Sequence_CurrentArray[ImportCurrentRows] = Row.Cells[4].Value.ToString();
                ImportCurrentRows++;
            }
        }

        private void SecondLoadTest()
        {
            try
            {
                string CheckCardCountString = RowCountString;
                SqlConnection CheckCountTotalConnection = new SqlConnection(SQL_Source);
                SqlCommand CheckCountTotalCommand = new SqlCommand(CheckCardCountString, CheckCountTotalConnection);
                CheckCountTotalConnection.Open();
                int CheckCardCountOperationTotal = (int)CheckCountTotalCommand.ExecuteScalar();
                CheckCountTotalConnection.Close();
                RowCount = CheckCardCountOperationTotal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            Array.Clear(ItemID_UpdatedArray, 0, ItemID_UpdatedArray.Length);
            Array.Clear(JobID_UpdatedArray, 0, JobID_UpdatedArray.Length);
            Array.Clear(Customer_UpdatedArray, 0, Customer_UpdatedArray.Length);
            Array.Clear(CustomerItemID_UpdatedArray, 0, CustomerItemID_UpdatedArray.Length);
            Array.Clear(Sequence_UpdatedArray, 0, Sequence_UpdatedArray.Length);
            Array.Clear(Steps_UpdatedArray, 0, Steps_UpdatedArray.Length);
            Array.Clear(StepsUsed_UpdatedArray, 0, StepsUsed_UpdatedArray.Length);
            Array.Clear(Sample3D_UpdatedArray, 0, Sample3D_UpdatedArray.Length);
            Array.Clear(Ready3D_UpdatedArray, 0, Ready3D_UpdatedArray.Length);
            Array.Clear(TotalRuns_UpdatedArray, 0, TotalRuns_UpdatedArray.Length);
            Array.Clear(PartsManufactured_UpdatedArray, 0, PartsManufactured_UpdatedArray.Length);
            Array.Clear(PartsPerMinute_UpdatedArray, 0, PartsPerMinute_UpdatedArray.Length);
            Array.Clear(SetupTime_UpdatedArray, 0, SetupTime_UpdatedArray.Length);
            Array.Clear(Tooling_UpdatedArray, 0, Tooling_UpdatedArray.Length);
            Array.Clear(ToolingLocation_UpdatedArray, 0, ToolingLocation_UpdatedArray.Length);
            Array.Clear(Fixture_UpdatedArray, 0, Fixture_UpdatedArray.Length);
            Array.Clear(FixtureLocation_UpdatedArray, 0, FixtureLocation_UpdatedArray.Length);
            Array.Clear(Template_UpdatedArray, 0, Template_UpdatedArray.Length);
            Array.Clear(TemplateLocation_UpdatedArray, 0, TemplateLocation_UpdatedArray.Length);

            // CAT Brake Press Arrays
            Array.Clear(BP1107_UpdatedArray, 0, BP1107_UpdatedArray.Length);
            Array.Clear(BP1139_UpdatedArray, 0, BP1139_UpdatedArray.Length);
            Array.Clear(BP1177_UpdatedArray, 0, BP1177_UpdatedArray.Length);

            // John Deere Brake Press Arrays
            Array.Clear(BP1127_UpdatedArray, 0, BP1127_UpdatedArray.Length);
            Array.Clear(BP1178_UpdatedArray, 0, BP1178_UpdatedArray.Length);

            // Navistar Brake Press Arrays
            Array.Clear(BP1065_UpdatedArray, 0, BP1065_UpdatedArray.Length);
            Array.Clear(BP1108_UpdatedArray, 0, BP1108_UpdatedArray.Length);
            Array.Clear(BP1156_UpdatedArray, 0, BP1156_UpdatedArray.Length);
            Array.Clear(BP1720_UpdatedArray, 0, BP1720_UpdatedArray.Length);

            // Paccar Brake Press Arrays
            Array.Clear(BP1083_UpdatedArray, 0, BP1083_UpdatedArray.Length);
            Array.Clear(BP1155_UpdatedArray, 0, BP1155_UpdatedArray.Length);
            Array.Clear(BP1158_UpdatedArray, 0, BP1158_UpdatedArray.Length);
            Array.Clear(BP1175_UpdatedArray, 0, BP1175_UpdatedArray.Length);
            Array.Clear(BP1176_UpdatedArray, 0, BP1176_UpdatedArray.Length);

            ImportUpdatedRows = 0;
            foreach (DataGridViewRow Row in JobListGridView.Rows)
            {
                if (CustomerCell_ComboBox.Text == "CAT")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1107_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1139_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    BP1177_UpdatedArray[ImportUpdatedRows] = Row.Cells[21].Value.ToString();
                    ImportUpdatedRows++;
                }
                else if (CustomerCell_ComboBox.Text == "John Deere")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1127_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1178_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    ImportUpdatedRows++;
                }
                if (CustomerCell_ComboBox.Text == "Navistar")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1065_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1108_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    BP1156_UpdatedArray[ImportUpdatedRows] = Row.Cells[21].Value.ToString();
                    BP1720_UpdatedArray[ImportUpdatedRows] = Row.Cells[22].Value.ToString();
                    ImportUpdatedRows++;
                }
                if (CustomerCell_ComboBox.Text == "Paccar")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1083_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1155_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    BP1158_UpdatedArray[ImportUpdatedRows] = Row.Cells[21].Value.ToString();
                    BP1175_UpdatedArray[ImportUpdatedRows] = Row.Cells[22].Value.ToString();
                    BP1176_UpdatedArray[ImportUpdatedRows] = Row.Cells[23].Value.ToString();
                    ImportUpdatedRows++;
                }
            }
        }

        private void DeleteTest()
        {
            try
            {
                for (int i = 0; i < ImportCurrentRows; i++)
                {
                    SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                    SqlCommand Delete_Job = new SqlCommand();
                    Delete_Job.CommandType = System.Data.CommandType.Text;
                    Delete_Job.CommandText = SQLRemoveCommand;
                    Delete_Job.Connection = Job_Connection;
                    Delete_Job.Parameters.AddWithValue("@ItemID", ItemID_CurrentArray[i].ToString());
                    Delete_Job.Parameters.AddWithValue("@JobID", JobID_CurrentArray[i].ToString());
                    Delete_Job.Parameters.AddWithValue("@Sequence", Sequence_CurrentArray[i].ToString());
                    Job_Connection.Open();
                    Delete_Job.ExecuteNonQuery();
                    Job_Connection.Close();
                }
            }
            catch (SqlException ExceptionValue)
            {
                int ErrorNumber = ExceptionValue.Number;
                MessageBox.Show("Error Importing Job" + "\n" + "Error Code: " + ErrorNumber.ToString());
            }
            FirstLoadTest();
        }

        private void WriteTest()
        {
            try
            {
                for (int i = 0; i < ImportUpdatedRows; i++)
                {
                    SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                    SqlCommand Add_Job = new SqlCommand();
                    Add_Job.CommandType = System.Data.CommandType.Text;
                    Add_Job.CommandText = SQLAddCommand;
                    Add_Job.Connection = Job_Connection;
                    if (CustomerCell_ComboBox.Text == "CAT")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1107", BP1107_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1139", BP1139_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1177", BP1177_UpdatedArray[i].ToString());
                    }
                    else if (CustomerCell_ComboBox.Text == "John Deere")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1127", BP1127_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1178", BP1178_UpdatedArray[i].ToString());
                    }
                    else if (CustomerCell_ComboBox.Text == "Navistar")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1065", BP1065_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1108", BP1108_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1156", BP1156_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1720", BP1720_UpdatedArray[i].ToString());
                    }
                    else if (CustomerCell_ComboBox.Text == "Paccar")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1083", BP1083_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1155", BP1155_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1158", BP1158_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1175", BP1175_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1176", BP1176_UpdatedArray[i].ToString());
                    }
                    Job_Connection.Open();
                    Add_Job.ExecuteNonQuery();
                    Job_Connection.Close();
                }
                MessageBox.Show("Import Successful");
                //SaveImport_Button.Hide();
                //CancelImport_Button.Hide();
                //DatabaseImport_Button.Show();
                DatabaseImportEnd_Controls();
            }
            catch (SqlException ExceptionValue)
            {
                int ErrorNumber = ExceptionValue.Number;
                if (ErrorNumber.Equals(2627))
                {
                    MessageBox.Show("Item ID: " + ItemID_TextBox.Text + " is Already on this List");
                }
                else if (ErrorNumber.Equals(245))
                {
                    MessageBox.Show("Item ID Can Only Contain Numbers");
                }
                else
                {
                    MessageBox.Show("Unable to Add Job. Please Try Again." + "\n" + "Error Code: " + ErrorNumber.ToString());
                }
            }
        }

        private void SaveImport_Button_Click(object sender, EventArgs e)
        {
            // Buttons
            SaveImport_Button.Enabled = false;
            CancelImport_Button.Enabled = false;
            Saving_Label.Text = "Importing";
            Saving_Label.Visible = true;

            CellName = "";
            if (CustomerCell_ComboBox.Text == "CAT")
            {
                CellName = "CAT";
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                CellName = "John Deere";
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                CellName = "Navistar";
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                CellName = "Paccar";
            }
            SaveExcelSQL_Test.RunWorkerAsync();
            //DeleteTest();
            //CreateExcel_Start();
            //CreateExcelSQL_Test.RunWorkerAsync();
            //DeleteTest();
            //WriteTest_Test();
            //Refresh_SQL();
        }

        private void WriteTest_Test()
        {
            try
            {
                for (int i = 0; i < ImportUpdatedRows; i++)
                {
                    SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                    SqlCommand Add_Job = new SqlCommand();
                    Add_Job.CommandType = System.Data.CommandType.Text;
                    Add_Job.CommandText = SQLAddCommand;
                    Add_Job.Connection = Job_Connection;
                    if (CellName == "CAT")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1107", BP1107_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1139", BP1139_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1177", BP1177_UpdatedArray[i].ToString());
                    }
                    else if (CellName == "John Deere")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1127", BP1127_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1178", BP1178_UpdatedArray[i].ToString());
                    }
                    else if (CellName == "Navistar")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1065", BP1065_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1108", BP1108_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1156", BP1156_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1720", BP1720_UpdatedArray[i].ToString());
                    }
                    else if (CellName == "Paccar")
                    {
                        Add_Job.Parameters.AddWithValue("@ItemID", ItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Customer", Customer_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@JobID", JobID_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sequence", Sequence_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Steps", Steps_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@StepsUsed", StepsUsed_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Sample3D", Sample3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Ready3D", Ready3D_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TotalRuns", TotalRuns_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsManufactured", PartsManufactured_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@PartsPerMinute", PartsPerMinute_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@SetupTime", SetupTime_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Tooling", Tooling_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Fixture", Fixture_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@Template", Template_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@TemplateLocation", TemplateLocation_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1083", BP1083_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1155", BP1155_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1158", BP1158_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1175", BP1175_UpdatedArray[i].ToString());
                        Add_Job.Parameters.AddWithValue("@BP1176", BP1176_UpdatedArray[i].ToString());
                    }
                    Job_Connection.Open();
                    Add_Job.ExecuteNonQuery();
                    Job_Connection.Close();
                }
                //MessageBox.Show("Import Successful");
                //SaveImport_Button.Hide();
                //CancelImport_Button.Hide();
                //DatabaseImport_Button.Show();
                //DatabaseImportEnd_Controls();
            }
            catch (SqlException ExceptionValue)
            {
                int ErrorNumber = ExceptionValue.Number;
                if (ErrorNumber.Equals(2627))
                {
                    MessageBox.Show("Item ID: " + ItemID_TextBox.Text + " is Already on this List");
                }
                else if (ErrorNumber.Equals(245))
                {
                    MessageBox.Show("Item ID Can Only Contain Numbers");
                }
                else
                {
                    MessageBox.Show("Unable to Add Job. Please Try Again." + "\n" + "Error Code: " + ErrorNumber.ToString());
                }
            }
        }

        private void SecondLoadTest_Test()
        {
            RowCount = 0;
            try
            {
                string CheckCardCountString = RowCountString;
                SqlConnection CheckCountTotalConnection = new SqlConnection(SQL_Source);
                SqlCommand CheckCountTotalCommand = new SqlCommand(CheckCardCountString, CheckCountTotalConnection);
                CheckCountTotalConnection.Open();
                int CheckCardCountOperationTotal = (int)CheckCountTotalCommand.ExecuteScalar();
                CheckCountTotalConnection.Close();
                RowCount = CheckCardCountOperationTotal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            Array.Clear(ItemID_UpdatedArray, 0, ItemID_UpdatedArray.Length);
            Array.Clear(JobID_UpdatedArray, 0, JobID_UpdatedArray.Length);
            Array.Clear(Customer_UpdatedArray, 0, Customer_UpdatedArray.Length);
            Array.Clear(CustomerItemID_UpdatedArray, 0, CustomerItemID_UpdatedArray.Length);
            Array.Clear(Sequence_UpdatedArray, 0, Sequence_UpdatedArray.Length);
            Array.Clear(Steps_UpdatedArray, 0, Steps_UpdatedArray.Length);
            Array.Clear(StepsUsed_UpdatedArray, 0, StepsUsed_UpdatedArray.Length);
            Array.Clear(Sample3D_UpdatedArray, 0, Sample3D_UpdatedArray.Length);
            Array.Clear(Ready3D_UpdatedArray, 0, Ready3D_UpdatedArray.Length);
            Array.Clear(TotalRuns_UpdatedArray, 0, TotalRuns_UpdatedArray.Length);
            Array.Clear(PartsManufactured_UpdatedArray, 0, PartsManufactured_UpdatedArray.Length);
            Array.Clear(PartsPerMinute_UpdatedArray, 0, PartsPerMinute_UpdatedArray.Length);
            Array.Clear(SetupTime_UpdatedArray, 0, SetupTime_UpdatedArray.Length);
            Array.Clear(Tooling_UpdatedArray, 0, Tooling_UpdatedArray.Length);
            Array.Clear(ToolingLocation_UpdatedArray, 0, ToolingLocation_UpdatedArray.Length);
            Array.Clear(Fixture_UpdatedArray, 0, Fixture_UpdatedArray.Length);
            Array.Clear(FixtureLocation_UpdatedArray, 0, FixtureLocation_UpdatedArray.Length);
            Array.Clear(Template_UpdatedArray, 0, Template_UpdatedArray.Length);
            Array.Clear(TemplateLocation_UpdatedArray, 0, TemplateLocation_UpdatedArray.Length);

            // CAT Brake Press Arrays
            Array.Clear(BP1107_UpdatedArray, 0, BP1107_UpdatedArray.Length);
            Array.Clear(BP1139_UpdatedArray, 0, BP1139_UpdatedArray.Length);
            Array.Clear(BP1177_UpdatedArray, 0, BP1177_UpdatedArray.Length);

            // John Deere Brake Press Arrays
            Array.Clear(BP1127_UpdatedArray, 0, BP1127_UpdatedArray.Length);
            Array.Clear(BP1178_UpdatedArray, 0, BP1178_UpdatedArray.Length);

            // Navistar Brake Press Arrays
            Array.Clear(BP1065_UpdatedArray, 0, BP1065_UpdatedArray.Length);
            Array.Clear(BP1108_UpdatedArray, 0, BP1108_UpdatedArray.Length);
            Array.Clear(BP1156_UpdatedArray, 0, BP1156_UpdatedArray.Length);
            Array.Clear(BP1720_UpdatedArray, 0, BP1720_UpdatedArray.Length);

            // Paccar Brake Press Arrays
            Array.Clear(BP1083_UpdatedArray, 0, BP1083_UpdatedArray.Length);
            Array.Clear(BP1155_UpdatedArray, 0, BP1155_UpdatedArray.Length);
            Array.Clear(BP1158_UpdatedArray, 0, BP1158_UpdatedArray.Length);
            Array.Clear(BP1175_UpdatedArray, 0, BP1175_UpdatedArray.Length);
            Array.Clear(BP1176_UpdatedArray, 0, BP1176_UpdatedArray.Length);

            ImportUpdatedRows = 0;
            foreach (DataGridViewRow Row in JobListGridView.Rows)
            {
                if (CellName == "CAT")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1107_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1139_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    BP1177_UpdatedArray[ImportUpdatedRows] = Row.Cells[21].Value.ToString();
                    ImportUpdatedRows++;
                }
                else if (CellName == "John Deere")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1127_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1178_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    ImportUpdatedRows++;
                }
                if (CellName == "Navistar")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1065_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1108_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    BP1156_UpdatedArray[ImportUpdatedRows] = Row.Cells[21].Value.ToString();
                    BP1720_UpdatedArray[ImportUpdatedRows] = Row.Cells[22].Value.ToString();
                    ImportUpdatedRows++;
                }
                if (CellName == "Paccar")
                {
                    ItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[0].Value.ToString();
                    Customer_UpdatedArray[ImportUpdatedRows] = Row.Cells[1].Value.ToString();
                    CustomerItemID_UpdatedArray[ImportUpdatedRows] = Row.Cells[2].Value.ToString();
                    JobID_UpdatedArray[ImportUpdatedRows] = Row.Cells[3].Value.ToString();
                    Sequence_UpdatedArray[ImportUpdatedRows] = Row.Cells[4].Value.ToString();
                    Steps_UpdatedArray[ImportUpdatedRows] = Row.Cells[5].Value.ToString();
                    StepsUsed_UpdatedArray[ImportUpdatedRows] = Row.Cells[6].Value.ToString();
                    Sample3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[7].Value.ToString();
                    Ready3D_UpdatedArray[ImportUpdatedRows] = Row.Cells[8].Value.ToString();
                    TotalRuns_UpdatedArray[ImportUpdatedRows] = Row.Cells[9].Value.ToString();
                    PartsManufactured_UpdatedArray[ImportUpdatedRows] = Row.Cells[10].Value.ToString();
                    PartsPerMinute_UpdatedArray[ImportUpdatedRows] = Row.Cells[11].Value.ToString();
                    SetupTime_UpdatedArray[ImportUpdatedRows] = Row.Cells[12].Value.ToString();
                    Tooling_UpdatedArray[ImportUpdatedRows] = Row.Cells[13].Value.ToString();
                    ToolingLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[14].Value.ToString();
                    Fixture_UpdatedArray[ImportUpdatedRows] = Row.Cells[15].Value.ToString();
                    FixtureLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[16].Value.ToString();
                    Template_UpdatedArray[ImportUpdatedRows] = Row.Cells[17].Value.ToString();
                    TemplateLocation_UpdatedArray[ImportUpdatedRows] = Row.Cells[18].Value.ToString();
                    BP1083_UpdatedArray[ImportUpdatedRows] = Row.Cells[19].Value.ToString();
                    BP1155_UpdatedArray[ImportUpdatedRows] = Row.Cells[20].Value.ToString();
                    BP1158_UpdatedArray[ImportUpdatedRows] = Row.Cells[21].Value.ToString();
                    BP1175_UpdatedArray[ImportUpdatedRows] = Row.Cells[22].Value.ToString();
                    BP1176_UpdatedArray[ImportUpdatedRows] = Row.Cells[23].Value.ToString();
                    ImportUpdatedRows++;
                }
            }
        }
        
        private void SaveExcelSQL_TestRun(object sender, EventArgs e)
        {
            DeleteTest();
            WriteTest_Test();
            SecondLoadTest_Test();
            //Refresh_SQL();
        }

        private void SaveExcelSQL_Test_RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Saving_Label.Text = "Import Complete";
            Refresh_SQL();

            CreateExcel_Start();
            CreateExcelSQL_Test.RunWorkerAsync();
        }


        private void CreateExcelFileSQL_Test(object sender, EventArgs e)
        {
            string ReportName = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Millisecond + "_";
            ReportName = ReportName.Replace("/", "_");
            ReportName = ReportName.Replace(":", "_");

            string ExcelFileLocation2 = "";

            // Excel Initialize
            ReportApp = new Excel.Application();
            ReportApp.Visible = false;
            ReportWB = (Excel._Workbook)(ReportApp.Workbooks.Add(""));
            ReportWS = (Excel._Worksheet)ReportWB.ActiveSheet;

            string[] ColumnNames = new string[JobListGridView.Columns.Count];
            int ExcelColumns = 1;

            foreach (DataGridViewColumn dc in JobListGridView.Columns)
            {
                ReportWS.Cells[1, ExcelColumns] = dc.Name;
                ExcelColumns++;
            }
            if (ReportCell == "CAT")
            {
                ReportWS.get_Range("A1", "V1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "V1");
                ReportWS.get_Range("A1", "V1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "V" + RowCount.ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in ReportRange.Cells)
                {
                    cell.BorderAround2();
                }
            }
            else if (ReportCell == "John Deere")
            {
                ReportWS.get_Range("A1", "U1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "U1");
                ReportWS.get_Range("A1", "U1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "U" + RowCount.ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in ReportRange.Cells)
                {
                    cell.BorderAround2();
                }
            }
            else if (ReportCell == "Navistar")
            {
                ReportWS.get_Range("A1", "W1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "W1");
                ReportWS.get_Range("A1", "W1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "W" + RowCount.ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in ReportRange.Cells)
                {
                    cell.BorderAround2();
                }
            }
            else if (ReportCell == "Paccar")
            {
                ReportWS.get_Range("A1", "X1").Font.Bold = true;
                ReportRange = ReportWS.get_Range("A1", "X1");
                ReportWS.get_Range("A1", "X1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                ReportRange.EntireColumn.AutoFit();
                ReportRange = ReportWS.get_Range("A1", "X" + RowCount.ToString());
            }

            for (int i = 0; i < ReportDataSet.Tables[0].Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < ReportDataSet.Tables[0].Columns.Count; j++)
                {
                    ReportWS.Cells[(i + 2), (j + 1)] = ReportDataSet.Tables[0].Rows[i][j];
                    CreateExcelSQL_Test.ReportProgress(i);
                }
            }
            ReportRange = ReportWS.get_Range("A1", "X1");
            ReportRange.EntireColumn.AutoFit();
            string ReportPDFName = ReportCell + "_Brake_Press_SQL" + ".xlsx";

            /*
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
            */
            ExcelFileLocation = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Job List\SQL Data Tables\" + ReportPDFName;
            if (File.Exists(ExcelFileLocation))
            {
                bool tryAgain = true;
                try
                {
                    File.Delete(ExcelFileLocation);
                }
                catch (IOException)
                {
                    tryAgain = false;
                }
                if (tryAgain == false)
                {
                    ExcelFileLocation = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Job List\SQL Data Tables\" + ReportName + "_" + ReportPDFName;
                }
            }

            ReportWS.SaveAs(ExcelFileLocation, Excel.XlFileFormat.xlOpenXMLWorkbook, null, null, false, false, false, false, false, false);
            //ReportWS.SaveAs(ExcelFileLocation);
            ReportWB.Close();
        }

        private void CreateExcelFileSQL_Test_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Saving_ProgressBar.Value = e.ProgressPercentage;
        }

        private void CreateExcelFileSQL_Test_RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            //DeleteTest();
            //WriteTest();
            //Refresh_SQL();
            // Process.Start(ExcelFileLocation);
            // MessageBox.Show("Saved");
            //Saving_Label.Text = "Saved";
            Saving_ProgressBar.Value = 0;
            Saving_ProgressBar.Hide();
            Saving_Label.Hide();
            ChangeCell_Button.Enabled = true;
            Search_Button.Enabled = true;
            Clear_Button.Enabled = true;
            Refresh_Button.Enabled = true;
            AddJob_Button.Enabled = true;
            EditJob_Button.Enabled = true;
            RemoveJob_Button.Enabled = true;
            DatabaseImport_Button.Enabled = true;
            SaveImport_Button.Enabled = true;
            CancelImport_Button.Enabled = true;
            SaveImport_Button.Hide();
            CancelImport_Button.Hide();
            Exit_Button.Enabled = true;
            //MessageBox.Show("Complete");
        }

        private void CancelImport_Button_Click(object sender, EventArgs e)
        {
            ClearForm();
            GroupBoxControlStart();
            DatabaseImportEnd_Controls();
        }

        /*********************************************************************************************************************
        * 
        * Methods in Testing End
        * 
        *********************************************************************************************************************/
        #endregion

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




        /********************************************************************************************************************
        * 
        * JobList End
        * 
        ********************************************************************************************************************/

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
