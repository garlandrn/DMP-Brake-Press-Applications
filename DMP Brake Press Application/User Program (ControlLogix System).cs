using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PressBrake;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using Opc.Da;

/*
 * 
* Program: DMP Brake Press Application
* Form: User Program (ControlLogix System)
* Created By: Ryan Garland
* Last Updated on 8/28/18
* 
*/

namespace DMP_Brake_Press_Application
{
    public partial class User_Program__ControlLogix_System_ : Form
    {
        public static Form UserInterface;
        BackgroundWorker BPComputerConnect;
        BackgroundWorker ConnectToOPC;
        BackgroundWorker RunMode_OPCs;

        public User_Program__ControlLogix_System_()
        {
            InitializeComponent();
            UserInterface = this;

            // Connect to Cincinnati Brake Press Computer
            BPComputerConnect = new BackgroundWorker();
            BPComputerConnect.DoWork += new DoWorkEventHandler(ComputerConnection);
            BPComputerConnect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BPComputerConnect_RunWorkerCompleted);

            // Connect to Kepware Server
            ConnectToOPC = new BackgroundWorker();
            ConnectToOPC.DoWork += new DoWorkEventHandler(ConnectToServer_OPC);
            ConnectToOPC.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ConnectToOPC_RunWorkerCompleted);

            RunMode_OPCs = new BackgroundWorker();
            RunMode_OPCs.DoWork += new DoWorkEventHandler(PartCount_OPC);
            RunMode_OPCs.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PartCount_RunWorkerCompleted);
            ServerConnect_Timer.Enabled = true;
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
        * OPC Tag Variables 
        * 
        ********************************************************************************************************************/

        private Opc.URL OPCUrl;
        private Opc.Da.Server OPCServer;
        private OpcCom.Factory OPCFactory = new OpcCom.Factory();

        private Opc.Da.Subscription GroupRead;
        private Opc.Da.SubscriptionState GroupStateRead;

        private Opc.Da.Subscription ScanNewJob_Write;
        private Opc.Da.SubscriptionState ScanNewJob_StateWrite;

        private Opc.Da.Subscription ItemID_Write;
        private Opc.Da.SubscriptionState ItemID_StateWrite;
        private Opc.Da.Subscription OperationSelection_Write;
        private Opc.Da.SubscriptionState OperationSelection_StateWrite;
        private Opc.Da.Subscription ModeChanged_Write;
        private Opc.Da.SubscriptionState ModeChanged_StateWrite;
        //
        private Opc.Da.Subscription TeachSensor_Write;
        private Opc.Da.SubscriptionState TeachSensor_StateWrite;
        private Opc.Da.Subscription RunMode_Write;
        private Opc.Da.SubscriptionState RunMode_WriteState;
        private Opc.Da.Subscription SetupMode_Write;
        private Opc.Da.SubscriptionState SetupMode_WriteState;
        private Opc.Da.Subscription PartComplete_GroupRead;
        private Opc.Da.SubscriptionState PartComplete_StateRead;
        private Opc.Da.Subscription PartNotProgrammed_GroupRead;
        private Opc.Da.SubscriptionState PartNotProgrammed_StateRead;

        private Opc.Da.Subscription HMILabel_GroupRead;
        private Opc.Da.SubscriptionState HMILabel_StateRead;



        private Opc.Da.Subscription NotApplicable_GroupRead;
        private Opc.Da.SubscriptionState NotApplicable_StateRead;

        private List<Item> OPC_WriteSteps = new List<Item>();
        private List<Item> OPC_StepCount = new List<Item>();
        private List<Item> OPC_Not_Applicable = new List<Item>();
        private List<Item> OPC_Message_HMI = new List<Item>();
        private Opc.Da.Subscription StepCount_GroupRead;
        private Opc.Da.SubscriptionState StepCount_StateRead;
        private Opc.Da.Subscription StepNumber_GroupWrite;
        private Opc.Da.SubscriptionState StepNumber_StateWrite;

        private Opc.Da.Subscription iNspectExpressData_GroupRead;
        private Opc.Da.SubscriptionState iNspectExpressData_GroupState;
        private string iNspectVisionID_PLC;
        private string iNspectVisionID_Camera;
        private List<Item> OPC_iNspectList = new List<Item>();


        private Opc.Da.Subscription PartProgrammed_GroupRead;
        private Opc.Da.SubscriptionState PartProgrammed_GroupState;
        private string Vision_PartProgrammed;
        private string NotApplicable_PartProgrammed;
        private List<Item> OPC_PartProgrammedList = new List<Item>();



        private string HMI_App;
        private string HMI_Cycle;
        private string HMI_Pulse;

        /********************************************************************************************************************
        * 
        * Form Load Variables 
        * 
        ********************************************************************************************************************/
        private string JobStartTime = "";
        private string JobEndTime = "";
        private string LoginForm = "User Program";
        private string LoginTime = "";
        private string[] CustomerCell = { "CAT", "John Deere", "Navistar", "Paccar", "90 Ton" };
        private string[] CAT_BrakePressList = { "1107", "1139", "1177" };
        private string[] JohnDeere_BrakePressList = { "1127", "1178" };
        private string[] Navistar_BrakePressList = { "1065", "1108", "1156", "1720" };
        private string[] Paccar_BrakePressList = { "1083", "1155", "1158", "1175", "1176" };
        private string[] Ton90_BrakePressList = { "1067", "1068", "1159" };
        private string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;";

        private string Brake_Press_ID = "";
        private string Brake_Press_Computer = "";
        private string Brake_Press_OPC = "";
        private string Brake_Press_iNspect = "";
        private string Brake_Press_PartProgrammed = "";
        private string Brake_Press_Fault_OPC = "";
        private string Brake_Press_Operation_Selection = "";
        private int Brake_Press_SQL = 0;

        private static bool JobFound = false;

        // Computer_Name_TextChanged()
        string[] PrintValues = { "Solution ID: ", "Parts Formed: ", "Last PPM: ", "Current Step: ", " " };
        string[] Values = { "10", "12", "19", "35", "9" };
        string[] ValuesArray = new string[5];
        string ValueResults;

        // Clock_Timer();
        private static int ClockHour;
        private static int ClockMinute;
        private static int ClockSecond;

        // FindTotalRunTime()
        TimeSpan TimeOfOperation = new TimeSpan();
        private float ItemSetupTime;

        // ItemRunCounter()
        private static int ItemRunCount = 0;

        // OperationIDCounter()
        private static int OperationsID = 0;

        // ShowResult();
        public string CurrentItemID = "";
        public string OEEItemID = "";
        public static int CurrentItemIDOperation;

        // RunningStatistics();
        private static double CurrentParts;
        private double PartsRemaining;

        public static string ScanOutComputer = "";

        // ProgramListUpdate():
        private static string ProgramListUpdate_SQL = "";

        // RefreshSQL():
        private static string RefreshData_SQL = "";
        public static string BrakePressRefresh_SQL = "";

        // ItemOperationCalculation();
        private static string AveragePPM_String = "";
        private static string PartsManufacturedTotal_String = "";
        private static double TotalItemPartsManufactured = 0;
        private static double PartsManufacturedTotal_Double = 0;
        private float AveragePPM = 0;

        // ViewPrint_Button_Click();
        private string CurrentSetupCardID = "";
        private string PDFSetupPath = "";
        private string BrakePressNamePDFPath = "";
        private string ItemIDBrakePressPDFPath = "";
        private string ItemIDPrintPDFPath = "";
        private string BrakePressSetupPDFPath = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Brake Press\Vision\Setup Cards for Vision Solutions\";
        private string ItemPrintPDFPath = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Brake Press\Vision\Brake Press Prints\";
        private string VisionSolutionsPDFPath = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Brake Press\Vision\Pictures for Vision Solutions\";
        //private string _3DPlacardPDFPath = @"\\insidedmp.com\corporate\OH\OH Common\Engineering\Brake Press\Vision\3D Scanner Placards for Vision Solutions\";
        private string _3DPlacardPDFPath = @"M:\Brake Press\Vision\3D Scanner Placards for Vision Solutions\";
        private static float PastPPM;

        private static string CheckTime = "";

        // GroupBoxes

        // ItemData     

        Color RunModeColor = ColorTranslator.FromHtml("#32EB00");
        Color SetupModeColor = Color.Transparent;

        /********************************************************************************************************************
        * 
        * Variables In Testing Start
        * 
        ********************************************************************************************************************/

        private float Efficiency;
        private float Utilization;
        private float OEE;

        private float TimePlanned;
        private float TimeActual;

        private Stopwatch RunModeStopWatch = new Stopwatch();
        private Stopwatch SetupModeStopWatch = new Stopwatch();
        private Stopwatch SignOutTime = new Stopwatch();
        private double SignedInHours = 0;

        private static string THEPPM_String = "";
        private static double THEPPM;

        private static double LIVEPPM;
        private static string LIVEPPM_String = "";

        private static bool RunMode_Button_Clicked = false;
        private static bool SetupMode_Button_Clicked = false;

        private TextBox[] ItemDataTextBoxes;
        private TextBox[] JobIDTextBoxes;
        private TextBox[] _3DScannerTextBoxes;
        private TextBox[] JobDataTextBoxes;

        public static string Customer = "";
        public static string CustomerPartNumber = "";
        
        private string NumberOfSteps = "";
        private string StepCount = "";
        private string PartsFormed_OPC = "";

        private static int NA_Count = 0;

        public static int SelectedButton;

        /********************************************************************************************************************
        * 
        * Variables In Testing End
        * 
        *********************************************************************************************************************
        *********************************************************************************************************************
        * 
        * User Program (ControlLogix System) Start
        * 
        ********************************************************************************************************************/

        private void User_Program__ControlLogix_System__Load(object sender, EventArgs e)
        {
            Customer_ComboBox.Items.AddRange(CustomerCell);
            BrakePressID();
            SqlConnection UserLogin = new SqlConnection(SQL_Source);
            SqlCommand Login = new SqlCommand();
            Login.CommandType = System.Data.CommandType.Text;
            Login.CommandText = "INSERT INTO [dbo].[LoginData] (EmployeeName,DMPID,LoginDateTime,LoginForm,CustomerCell,BrakePress) VALUES (@EmployeeName,@DMPID,@LoginDateTime,@LoginForm,@CustomerCell,@BrakePress)";
            Login.Connection = UserLogin;
            Login.Parameters.AddWithValue("@LoginDateTime", Clock_TextBox.Text);
            Login.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            Login.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            Login.Parameters.AddWithValue("@LoginForm", LoginForm.ToString());
            Login.Parameters.AddWithValue("@CustomerCell", Customer_ComboBox.Text);
            Login.Parameters.AddWithValue("@BrakePress", BrakePress_ComboBox.Text);
            UserLogin.Open();
            Login.ExecuteNonQuery();
            UserLogin.Close();

            Clock.Enabled = true;
            LoginTime = Clock_TextBox.Text;
            ScanNewJob_Button.Focus();
            ConnectToOPC.RunWorkerAsync();
            SignOutTime.Start();
        }

        /********************************************************************************************************************
        * 
        * Buttons Region Start 
        * -- Total Buttons: 15
        * 
        * --- User Program ControlLogix System Form Buttons
        * --- Total: 4 
        * - LogOff Click
        * - ViewSchedule Click
        * - HideSchedule Click
        * - ReportError Click
        * 
        * --- ItemData GroupBox Buttons
        * --- Total: 5
        * - ViewHitImage Click
        * - CheckCardData Click
        * - ViewSetupCard Click
        * - ViewPrint Click
        * - View3DPlacard Click
        * 
        * --- JobData GroupBox Button
        * --- Total: 1
        * - ResetStepCount Click 
        * 
        * --- OPC Buttons GroupBox Buttons
        * --- Total: 5
        * - ScanNewJob Click
        * - RunMode Click
        * - SetupMode Click
        * - CancelRun Click
        * - JobEnd Click
        * 
        ********************************************************************************************************************/
        #region

        // User Program ControlLogix System Form Buttons

        private void LogOff_Button_Click(object sender, EventArgs e)
        {
            if (RunMode_Button_Clicked == false)
            {
                ScanNewJob_OPC();
                CurrentItemID = "999-99999";
                ItemIDWriteTo_OPC();
                EmployeeLogOff();
                ServerConnect_Timer.Enabled = false;
                OPCServer.Disconnect();
                DMPBrakePressLogin.Current.Focus();
                DMPBrakePressLogin.Current.Enabled = true;
                DMPBrakePressLogin.Current.WindowState = FormWindowState.Maximized;
                DMPBrakePressLogin.Current.ShowInTaskbar = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please End Job Before Logging Off");
            }
        }

        private void ViewSchedule_Button_Click(object sender, EventArgs e)
        {
            /*
            ScheduleGridView.Visible = true;
            this.ScheduleGridView.BringToFront();
            this.ScheduleGridView.Location = new System.Drawing.Point(830, 80);
            this.ScheduleGridView.Size = new System.Drawing.Size(950, 175);
            ViewSchedule_Button.Visible = false;
            HideSchedule_Button.Visible = true;
            Schedule_Timer.Start();
            */
            UserInterface.Focus();
            UserInterface.Enabled = false;
            User_Program_Schedule UserSchedule = new User_Program_Schedule(this);
            User_Program_Schedule.BrakePressRefresh_SQL = BrakePressRefresh_SQL;

            UserSchedule.Show();
        }

        private void HideSchedule_Button_Click(object sender, EventArgs e)
        {
            /*
            ScheduleGridView.Visible = false;
            this.ScheduleGridView.Location = new System.Drawing.Point(1496, 986);
            this.ScheduleGridView.Size = new System.Drawing.Size(69, 58);
            ViewSchedule_Button.Visible = true;
            HideSchedule_Button.Visible = false;
            Schedule_Timer.Stop();
            */
        }

        private void ReportError_Button_Click(object sender, EventArgs e)
        {
            Report_Error ErrorReport = new Report_Error();
            ErrorReport.Cell_TextBox.Text = Customer_TextBox.Text;
            ErrorReport.BrakePress_TextBox.Text = BrakePress_TextBox.Text;
            ErrorReport.ItemID_TextBox.Text = ItemID_TextBox.Text;
            ErrorReport.JobID_TextBox.Text = JobID_TextBox.Text;
            ErrorReport.User_TextBox.Text = User_TextBox.Text;
            ErrorReport.DMPID_TextBox.Text = DMPID_TextBox.Text;
            ErrorReport.Show();
        }

        // ItemData GroupBox Buttons

        private void ViewHitImage_Button_Click(object sender, EventArgs e)
        {
            string ViewHitItemID = CurrentItemID.Replace("-", "");
            ItemIDBrakePressPDFPath = ViewHitItemID + BrakePressNamePDFPath;
            string CompletePDFPath = Path.Combine(VisionSolutionsPDFPath, ItemIDBrakePressPDFPath);
            string ViewBrakePressPDFFile = CompletePDFPath + ".pdf";
            /*
            ViewPDF PDFViewer = new ViewPDF();
            if (File.Exists(ViewBrakePressPDFFile) == true)
            {                
                PDFViewer.AcroPDF.src = ViewBrakePressPDFFile;
                PDFViewer.AcroPDF.BringToFront();
                PDFViewer.Show();
                PDFViewer.BringToFront();
            }
            else
            {
                PDFViewer.AcroPDF.src = @"C:\Users\rgarland\Desktop\Hit Images Not.pdf";
                PDFViewer.AcroPDF.BringToFront();
                PDFViewer.Show();
                PDFViewer.BringToFront();
            }
            */
            if (File.Exists(ViewBrakePressPDFFile))
            {
                ViewPDF PDFViewer = new ViewPDF();
                PDFViewer.AcroPDF.src = ViewBrakePressPDFFile;
                PDFViewer.AcroPDF.BringToFront();
                PDFViewer.Show();
                PDFViewer.BringToFront();
            }
            else
            {
                ViewPDF PDFViewer = new ViewPDF();
                PDFViewer.AcroPDF.src = @"\\insidedmp.com\Corporate\OH\OH Common\Engineering\Brake Press\Vision\Pictures for Vision Solutions\Images_Not_Created.pdf";
                PDFViewer.AcroPDF.BringToFront();
                //PDFViewer.AcroPDF.setZoom(50);
                PDFViewer.Show();
            }
        }

        private void CheckCardData_Button_Click(object sender, EventArgs e)
        {
            Check_Card_Data_Enter CheckCardData = new Check_Card_Data_Enter();
            CheckCardData.Show();
            CheckCardData.DateTime_TextBox.Text = Clock_TextBox.Text;
            CheckCardData.ItemID_TextBox.Text = ItemID_TextBox.Text;
            CheckCardData.Sequence_TextBox.Text = Sequence_TextBox.Text;
            CheckCardData.OperatorName_TextBox.Text = User_TextBox.Text;
            CheckCardData.OperationID_TextBox.Text = OperationsID.ToString();
            CheckCardData.Customer_TextBox.Text = Customer;
            CheckCardData.CustomerPartNumber_TextBox.Text = CustomerPartNumber;
        }

        private void ViewSetupCard_Button_Click(object sender, EventArgs e)
        {
            PDFSetupPath = BrakePressSetupPDFPath + CurrentSetupCardID;
            string ViewSetupPDFFile = PDFSetupPath + ".pdf";
            ViewPDF PDFViewer = new ViewPDF();
            PDFViewer.AcroPDF.src = ViewSetupPDFFile;
            PDFViewer.AcroPDF.BringToFront();
            PDFViewer.Show();
            PDFViewer.BringToFront();
        }

        private void ViewPrint_Button_Click(object sender, EventArgs e)
        {
            CurrentItemID = CurrentItemID.Replace("-", "");
            ItemIDPrintPDFPath = CurrentItemID;
            string CompletePDFPath = Path.Combine(VisionSolutionsPDFPath, ItemPrintPDFPath);
            string ViewPrintPDFFile = CompletePDFPath + ".pdf";


            ViewPDF PDFViewer = new ViewPDF();
            PDFViewer.AcroPDF.src = ViewPrintPDFFile;
            PDFViewer.AcroPDF.BringToFront();
            PDFViewer.Show();
        }

        private void View3DPlacard_Button_Click(object sender, EventArgs e)
        {
            //CurrentItemID = CurrentItemID.Replace("-", "");
            string CompletePDFPath = Path.Combine(_3DPlacardPDFPath, CurrentItemID);
            string View3DPlacardPDFFile = CompletePDFPath + ".pdf";
            ViewPDF PDFViewer = new ViewPDF();
            PDFViewer.AcroPDF.src = View3DPlacardPDFFile;
            PDFViewer.AcroPDF.BringToFront();
            PDFViewer.Show();
            PDFViewer.BringToFront();
        }

        // JobData GroupBox Button

        private void ResetStepCount_Button_Click(object sender, EventArgs e)
        {
           HMILabels_OPC();
           HMI_Check();
        }

        // OPC Buttons GroupBox Buttons

        private void ScanNewJob_Button_Click(object sender, EventArgs e)
        {
            //SelectedButton = 1;
            //ModeChanged_OPC();
            //ScanNewJob_OPC();
            if (ItemID_TextBox.TextLength == 9 || ItemID_TextBox.TextLength == 13)
            {
                ItemOperationDataEnd();
                OperationDataEnd();
                OperationOEEData();
            }
            ClearForm();
            ItemID_TextBox.ReadOnly = false;
            ItemID_TextBox.Focus();
            PartsRunProgressBar.Value = 0;
            ScanNewJob_Button.BackColor = RunModeColor;
            RunMode_Button.BackColor = Color.Transparent;
            SetupMode_Button.BackColor = Color.Transparent;
            ReportError_Button.Enabled = false;
            ScanNewJob_OPC();
            CurrentItemID = "999-99999";
            ItemIDWriteTo_OPC();            
        }

        private void RunMode_Button_Click(object sender, EventArgs e)
        {
            //SelectedButton = 2;
            //ModeChanged_OPC();
            LIVEPPM_String = PartsFormed_TextBox.Text;
            //LIVEPPM = double.Parse(LIVEPPM_String);
            SetupModeStopWatch.Stop();
            RunModeStopWatch.Start();
            JobEnd_Button.Enabled = true;
            JobEnd_Button.Visible = true;
            //CancelRun_Button.Enabled = false;
            //CancelRun_Button.Visible = false;
            //PartsRunProgressBar.Visible = true;
            //HMI_NotActive_TextBox.Visible = false;
            //JobStartTime = Clock_TextBox.Text;
            Fault_Timer.Enabled = true;
            //Timer.Enabled = true;
            string StartingTime = Clock_TextBox.Text;
            string ReplaceTime = DateTime.Today.ToShortDateString();
            //JobStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
            RunMode_Button.BackColor = RunModeColor;
            SetupMode_Button.BackColor = Color.Transparent;
            ScanNewJob_Button.BackColor = Color.Transparent;
            ScanNewJob_Button.Enabled = false;
            RunMode_OPC();
            //HMILabels_OPC();
            if (RunMode_Button_Clicked == false)
            {
                JobStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
                RunMode_Button_Clicked = true;
            }
            RunMode_Button_Clicked = true;
        }

        private void SetupMode_Button_Click(object sender, EventArgs e)
        {
            //SelectedButton = 3;
            //ModeChanged_OPC();
            //Timer.Enabled = false;
            //Fault_Timer.Enabled = true;
            RunModeStopWatch.Stop();
            SetupModeStopWatch.Start();
            SetupMode_Button.BackColor = Color.Yellow;
            RunMode_Button.BackColor = Color.Transparent;
            ScanNewJob_Button.BackColor = Color.Transparent;
            SetupMode_Button_Clicked = true;
            SetupModeMessage_TextBox.Hide();
            //Fault_Label.Hide();
            SetupMode_OPC();
        }

        private void CancelRun_Button_Click(object sender, EventArgs e)
        {
            ClearForm();
            ScanNewJob_Button.Enabled = true;
            RunMode_Button.Enabled = false;
            SetupMode_Button.Enabled = false;
            CancelRun_Button.Enabled = false;
            CancelRun_Button.Visible = false;
        }

        private void JobEnd_Button_Click(object sender, EventArgs e)
        {
            ScanNewJob_Button.Enabled = true;
            RunMode_Button.Enabled = false;
            SetupMode_Button.Enabled = false;
            JobEnd_Button.Enabled = false;
            RunMode_Button.BackColor = Color.Transparent;
            SetupMode_Button.BackColor = Color.Transparent;
            SetupModeStopWatch.Stop();
            RunMode_Button_Clicked = false;
            SetupMode_Button_Clicked = false;
            JobEndTime = Clock_TextBox.Text;
            JobEnd_Button.Visible = false;
            PartsRunProgressBar.Visible = false;
            //HMI_NotActive_TextBox.Visible = true;
            Customer_ComboBox.Enabled = true;
            BrakePress_ComboBox.Enabled = true;
            ViewHitImage_Button.Enabled = false;
            ViewSetupCard_Button.Enabled = false;

            // Opens The Scan Out Form
            /*
            
            UserInterface.Focus();
            UserInterface.Enabled = false;
             
            User_Program_Scan_Out ScanOut = new User_Program_Scan_Out();
            ScanOut.EmployeeNumber_TextBox.Text = DMPID_TextBox.Text;
            ScanOut.JobNumber_TextBox.Text = ReferenceNumber_TextBox.Text;
            ScanOut.TotalCountQtuQtyComp_TextBox.Text = PartsFormed_TextBox.Text;
            if (ScanOut.ShowDialog(this) == DialogResult.Yes)
            {
                ScanOutComputer = "Yes";
                ScanOutCompletion();
            }
            else if (ScanOut.DialogResult == DialogResult.No)
            {
                ScanOutComputer = "No";
                ScanOutCompletion();
            }
            */

            SetupMode_OPC();

            /*
            ItemOperationCalculation();
            FindTotalRunTime();
            OperationOEECalculation();
            OperationOEEData();
            ItemOperationDataEnd();
            OperationDataEnd();
            ProgramListUpdate();
            Timer.Enabled = false;
            RefreshSQL1176();
            */
        }

        private void SytelineScanOut_Button_Click(object sender, EventArgs e)
        {
            User_Program_Scan_Out ScanOut = new User_Program_Scan_Out();
            ScanOut.EmployeeNumber_TextBox.Text = DMPID_TextBox.Text;
            ScanOut.TotalCountQtuQtyComp_TextBox.Text = PartsFormed_TextBox.Text;
            ScanOut.Show();
        }

        /*********************************************************************************************************************
        * 
        * Buttons Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * ComboBox Region Start
        * -- Total: 2
        * 
        * - Customer ComboBox SelectedIndexChanged
        * - BrakePress ComboBox SelectedIndexChanged
        * 
        *********************************************************************************************************************/
        #region

        private void Customer_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrakePress_ComboBox.Items.Clear();
            BrakePress_ComboBox.Text = "";
            ClearTextBoxes();

            if (Customer_ComboBox.Text == "CAT")
            {   // 4
                BrakePress_ComboBox.Items.AddRange(CAT_BrakePressList);

                ProgramListUpdate_SQL = "UPDATE [dbo].[CAT_Item_Data] SET PartsManufactured=@PartsManufactured,PartsPerMinute=@PartsPerMinute,TotalRuns=@TotalRuns WHERE ItemID=@ItemID";
                RefreshData_SQL = "SELECT * FROM [dbo].[CAT_Item_Data]";
                //Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[CAT_Item_Data] WHERE ItemID = '" + CurrentItemID + "'";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1176 = "SELECT * FROM [dbo].[CAT_Item_Data]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                UserProgramGridView.DataSource = Data.Tables[0];
            }
            else if (Customer_ComboBox.Text == "John Deere")
            {   // 2
                BrakePress_ComboBox.Items.AddRange(JohnDeere_BrakePressList);

                ProgramListUpdate_SQL = "UPDATE [dbo].[JohnDeere_Item_Data] SET PartsManufactured=@PartsManufactured,PartsPerMinute=@PartsPerMinute,TotalRuns=@TotalRuns WHERE ItemID=@ItemID";
                RefreshData_SQL = "SELECT * FROM [dbo].[JohnDeere_Item_Data]";
                Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[JohnDeere_Item_Data] WHERE ItemID = '" + CurrentItemID + "'";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP_JohnDeere = "SELECT * FROM [dbo].[JohnDeere_Item_Data]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP_JohnDeere, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                UserProgramGridView.DataSource = Data.Tables[0];
            }
            else if (Customer_ComboBox.Text == "Navistar")
            {   // 4
                BrakePress_ComboBox.Items.AddRange(Navistar_BrakePressList);

                ProgramListUpdate_SQL = "UPDATE [dbo].[Navistar_Item_Data] SET PartsManufactured=@PartsManufactured,PartsPerMinute=@PartsPerMinute,TotalRuns=@TotalRuns WHERE ItemID=@ItemID";
                RefreshData_SQL = "SELECT * FROM [dbo].[Navistar_Item_Data]";
                //Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[Navistar_Item_Data] WHERE ItemID = '" + CurrentItemID + "'";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1176 = "SELECT * FROM [dbo].[Navistar_Item_Data]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                UserProgramGridView.DataSource = Data.Tables[0];
            }
            else if (Customer_ComboBox.Text == "Paccar")
            {   //5
                BrakePress_ComboBox.Items.AddRange(Paccar_BrakePressList);

                ProgramListUpdate_SQL = "UPDATE [dbo].[Paccar_Item_Data] SET PartsManufactured=@PartsManufactured,PartsPerMinute=@PartsPerMinute,TotalRuns=@TotalRuns WHERE ItemID=@ItemID";
                RefreshData_SQL = "SELECT * FROM [dbo].[Paccar_Item_Data]";
                //Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[Paccar_Item_Data] WHERE ItemID = '" + CurrentItemID + "'";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1176 = "SELECT * FROM [dbo].[Paccar_Item_Data]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                UserProgramGridView.DataSource = Data.Tables[0];
                
            }            
        }

        private void BrakePress_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // CAT Brake Press
            if (BrakePress_ComboBox.Text == "1107")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1107";
                Brake_Press_Computer = "PB50093";
                Brake_Press_SQL = 19;
                //Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1107.";
                Brake_Press_ID = "OHN66OPC.BrakePress_ControlLogix.Global.B1107_";
                //Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1107.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1107_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1107_Schedule] ORDER BY RunOrder ASC";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1107 = "SELECT * FROM [dbo].[BP_1107_Schedule] ORDER BY RunOrder ASC";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1107, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
                
            }
            else if (BrakePress_ComboBox.Text == "1139")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1139";
                Brake_Press_Computer = "PB51294";
                Brake_Press_SQL = 20;
                //Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1139.";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1139.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1139_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1139_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1139 = "SELECT * FROM [dbo].[BP_1139_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1139, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            else if (BrakePress_ComboBox.Text == "1177")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1177";
                Brake_Press_Computer = "PB49568";
                Brake_Press_SQL = 21;
                //Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1177.";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1177.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1177_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1177_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1139 = "SELECT * FROM [dbo].[BP_1139_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1139, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            // John Deere Brake Press
            else if (BrakePress_ComboBox.Text == "1127")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1127";
                Brake_Press_Computer = "PB55569";
                Brake_Press_SQL = 19;
                //Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1127.";
                Brake_Press_ID = "OHN66OPC.BrakePress_ControlLogix.Global.B1127_";
                //Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1127.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1127_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1127_Schedule] ORDER BY RunOrder ASC";
                // Select BarCode Reader on Current Brake Press
            }
            else if (BrakePress_ComboBox.Text == "1178")
            {
                // Connect to SQL DataTable and Load
                Brake_Press_Computer = "PB55569";
                BrakePressNamePDFPath = "_1178";
                Brake_Press_SQL = 20;
                Brake_Press_OPC = "OHN66OPC.Brake_Press_1178.Global.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_1178.Prgm_MainProgram.";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1178_Schedule] ORDER BY RunOrder ASC";

                // Select BarCode Reader on Current Brake Press

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1178 = "SELECT * FROM [dbo].[BP_1178_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1178, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            // Navistar Brake Press
            else if (BrakePress_ComboBox.Text == "1065")
            {
                // Connect to SQL DataTable and Load
                Brake_Press_Computer = "PB846662";
                BrakePressNamePDFPath = "_1065";
                Brake_Press_SQL = 19;
                Brake_Press_OPC = "OHN66OPC.Brake_Press_1065.Global.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_1065.Prgm_MainProgram.";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1065_Schedule] ORDER BY RunOrder ASC";
                Brake_Press_ID = "OHN66OPC.Brake_Press_ControlLogix.Global.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.";

                // Select BarCode Reader on Current Brake Press

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1065 = "SELECT * FROM [dbo].[BP_1065_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1065, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            else if (BrakePress_ComboBox.Text == "1108")
            {
                // Connect to SQL DataTable and Load
                Brake_Press_Computer = "PB50208";
                BrakePressNamePDFPath = "_1108";
                Brake_Press_SQL = 20;
                Brake_Press_OPC = "OHN66OPC.Brake_Press_1108.Global.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_1108.Prgm_MainProgram.";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1108_Schedule] ORDER BY RunOrder ASC";
                Brake_Press_ID = "OHN66OPC.Brake_Press_ControlLogix.Global.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.";

                // Select BarCode Reader on Current Brake Press

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1108 = "SELECT * FROM [dbo].[BP_1108_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1108, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            else if (BrakePress_ComboBox.Text == "1156")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1156";
                Brake_Press_Computer = "PB54539";
                Brake_Press_SQL = 21;
                Brake_Press_ID = "OHN66OPC.BrakePress_ControlLogix.Global.B1156_";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1156.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1156_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1156_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1156 = "SELECT * FROM [dbo].[BP_1156_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1156, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            else if (BrakePress_ComboBox.Text == "1720")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1720";
                Brake_Press_Computer = "PB51581";
                Brake_Press_SQL = 22;
                Brake_Press_ID = "OHN66OPC.BrakePress_ControlLogix.Global.B1720_";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1720.";
                Brake_Press_iNspect = "OHN66OPC.BrakePress_ControlLogix.Global.BrakePress1720.";
                Brake_Press_PartProgrammed = "OHN66OPC.BrakePress_ControlLogix.Prgm_Brake_1720."; //Brake_Press_1720_Part_Programmed
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1720_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1720_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1720 = "SELECT * FROM [dbo].[BP_1720_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1720, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            // Paccar Brake Press
            else if (BrakePress_ComboBox.Text == "1083")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1083";
                Brake_Press_Computer = "PB48909";
                Brake_Press_SQL = 19;
                Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1083.";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1083.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1083_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1083_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1083 = "SELECT * FROM [dbo].[BP_1083_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1083, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            else if (BrakePress_ComboBox.Text == "1155")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1155";
                //Computer_Name.Text = "PB54125";
                Brake_Press_Computer = "PB54125";
                Brake_Press_SQL = 20;
                Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1155.";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1155.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1155_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1155_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1155 = "SELECT * FROM [dbo].[BP_1155_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1155, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet(); 
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            else if (BrakePress_ComboBox.Text == "1158")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1158";
                Brake_Press_Computer = "pb846574";
                Brake_Press_SQL = 21;
                Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1158.";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1158.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1158_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1158_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1158 = "SELECT * FROM [dbo].[BP_1158_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1158, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */
            }
            else if (BrakePress_ComboBox.Text == "1175")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1175";
                Brake_Press_Computer = "CI846574";
                Brake_Press_SQL = 22;
                Brake_Press_ID = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1175.";
                Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1175.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1175_";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1175_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1175 = "SELECT * FROM [dbo].[BP_1175_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1175, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                */
            }
            else if (BrakePress_ComboBox.Text == "1176")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1176";
                Brake_Press_Computer = "PB53973";
                Brake_Press_SQL = 23;
                // Select BarCode Reader on Current Brake Press
                //Brake_Press_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.";
                Brake_Press_ID = "OHN66OPC.Brake_Press_ControlLogix.Global.";
                Brake_Press_Fault_OPC = "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.";
                BrakePressRefresh_SQL = "SELECT * FROM [dbo].[BP_1176_Schedule] ORDER BY RunOrder ASC";

                /*
                 SqlConnection connection = new SqlConnection(SQL_Source);
                 string BP1176 = "SELECT * FROM [dbo].[BP_1176_Schedule] ORDER BY RunOrder ASC";
                 SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
                 SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                 DataSet Data = new DataSet();
                 dataAdapter.Fill(Data);
                 ScheduleGridView.DataSource = Data.Tables[0];
                 */

                //GroupState.Name = "1176";
                //GroupState.Active = true;
                //GroupID = (Opc.Da.Subscription)Server.CreateSubscription(GroupState);
                /*
                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1176 = "SELECT * FROM [dbo].[Paccar_Item_Data]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                UserProgramGridView.DataSource = Data.Tables[0];

                int rows = 0;
                string BP1176Count = "SELECT COUNT(*) FROM [dbo].[Paccar_Item_Data]";
                SqlConnection count = new SqlConnection(SQL_Source);
                SqlCommand countRows = new SqlCommand(BP1176Count, count);
                count.Open();
                rows = (int)countRows.ExecuteScalar();
                count.Close();

                foreach (DataGridViewRow row in UserProgramGridView.Rows)
                {
                    if (row.Index < rows)
                    {

                        ItemID_ComboBox.Items.Add(row.Cells[0].Value.ToString());
                    }
                }
                */
            }
            BPComputerConnect.RunWorkerAsync();

        }

        /*********************************************************************************************************************
        * 
        * ComboBox Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        *  
        *  SQL Data Region Start
        *  -- Total: 11
        *  
        *  - EmployeeLogOff
        *  - ItemOperationDataStart
        *  - ItemOperationDataEnd
        *  - ScanOutCompletion
        *  - ItemRunCounter
        *  - OperationIDCounter
        *  - OperationDataStart
        *  - OperationDataEnd
        *  - OperationOEEData
        *  - ProgramListUpdate
        *  - RefreshSQL
        * 
        ********************************************************************************************************************/
        #region

        private void EmployeeLogOff()
        {
            SqlConnection UserLogoff = new SqlConnection(SQL_Source);
            SqlCommand Logoff = new SqlCommand();
            Logoff.CommandType = System.Data.CommandType.Text;
            Logoff.CommandText = "UPDATE [dbo].[LoginData] SET LogoutDateTime=@LogoutDateTime WHERE LoginDateTime=@LoginDateTime";
            Logoff.Connection = UserLogoff;
            Logoff.Parameters.AddWithValue("@LoginDateTime", LoginTime.ToString());
            Logoff.Parameters.AddWithValue("@LogoutDateTime", Clock_TextBox.Text);
            UserLogoff.Open();
            Logoff.ExecuteNonQuery();
            UserLogoff.Close();
        }

        private void ItemOperationDataStart()
        {
            SqlConnection OperationStart = new SqlConnection(SQL_Source);
            SqlCommand StartOperation = new SqlCommand();
            StartOperation.CommandType = System.Data.CommandType.Text;
            StartOperation.CommandText = "INSERT INTO [dbo].[ItemOperationData] (ItemID, Sequence, OperationID, ItemRunCount, StartDateTime, EmployeeName, DMPID, BrakePress, CheckCardCompleted) VALUES (@ItemID,@Sequence,@OperationID,@ItemRunCount,@StartDateTime,@EmployeeName,@DMPID,@BrakePress,@CheckCardCompleted)";
            StartOperation.Connection = OperationStart;
            StartOperation.Parameters.AddWithValue("@ItemID", CurrentItemID);
            StartOperation.Parameters.AddWithValue("@Sequence", CurrentItemIDOperation);
            StartOperation.Parameters.AddWithValue("@OperationID", OperationsID.ToString());
            StartOperation.Parameters.AddWithValue("@ItemRunCount", ItemRunCount.ToString());
            StartOperation.Parameters.AddWithValue("@StartDateTime", Clock_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@BrakePress", BrakePress_ComboBox.Text);
            StartOperation.Parameters.AddWithValue("@CheckCardCompleted", "No");
            OperationStart.Open();
            StartOperation.ExecuteNonQuery();
            OperationStart.Close();
        }

        private void ItemOperationDataEnd()
        {
            SqlConnection OperationEnd = new SqlConnection(SQL_Source);
            SqlCommand EndOperation = new SqlCommand();
            EndOperation.CommandType = System.Data.CommandType.Text;
            //EndOperation.CommandText = "UPDATE [dbo].[ItemOperationData] SET EndDateTime=@EndDateTime,PartsManufactured=@PartsManufactured, PartsPerMinute=@PartsPerMinute WHERE OperationID=@OperationID";
            EndOperation.CommandText = "UPDATE [dbo].[ItemOperationData] SET EndDateTime=@EndDateTime WHERE OperationID=@OperationID";
            EndOperation.Connection = OperationEnd;
            EndOperation.Parameters.AddWithValue("@OperationID", OperationsID);
            EndOperation.Parameters.AddWithValue("@EndDateTime", Clock_TextBox.Text);
            //EndOperation.Parameters.AddWithValue("@PartsManufactured", PartsFormed_TextBox.Text);
            //EndOperation.Parameters.AddWithValue("@PartsPerMinute", CurrentPPM_TextBox.Text);
            OperationEnd.Open();
            EndOperation.ExecuteNonQuery();
            OperationEnd.Close();
        }

        private void ScanOutCompletion()
        {
            SqlConnection ScanOutCompletion = new SqlConnection(SQL_Source);
            SqlCommand CompletionScanOut = new SqlCommand();
            CompletionScanOut.CommandType = System.Data.CommandType.Text;
            CompletionScanOut.CommandText = "UPDATE [dbo].[ItemOperationData] SET ScanOutComputer=@ScanOutComputer WHERE OperationID=@OperationID";
            CompletionScanOut.Connection = ScanOutCompletion;
            CompletionScanOut.Parameters.AddWithValue("@OperationID", OperationsID);
            CompletionScanOut.Parameters.AddWithValue("@ScanOutComputer", ScanOutComputer);
            ScanOutCompletion.Open();
            CompletionScanOut.ExecuteNonQuery();
            ScanOutCompletion.Close();
        }

        private void ItemRunCounter()
        {
            try
            {
                string CountOperations = "SELECT COUNT(ItemID) FROM [dbo].[ItemOperationData] WHERE ItemID='" + CurrentItemID + "' AND Sequence='" + CurrentItemIDOperation + "'";
                SqlConnection OperationCount = new SqlConnection(SQL_Source);
                SqlCommand CountItemRun = new SqlCommand(CountOperations, OperationCount);
                OperationCount.Open();
                int OperationRunCount = (int)CountItemRun.ExecuteScalar();
                OperationCount.Close();
                ItemRunCount = OperationRunCount + 1;
                //ItemRunCount_TextBox.Text = ItemRunCount.ToString();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        private void OperationIDCounter()
        {
            string CountOperations = "SELECT COUNT(*) FROM [dbo].[OperationData]";
            SqlConnection OperationCount = new SqlConnection(SQL_Source);
            SqlCommand CountOperation = new SqlCommand(CountOperations, OperationCount);
            OperationCount.Open();
            int OperationCountID = (int)CountOperation.ExecuteScalar();
            OperationCount.Close();
            OperationsID = OperationCountID + 1;
        }

        private void OperationDataStart()
        {
            SqlConnection OperationStart = new SqlConnection(SQL_Source);
            SqlCommand StartOperation = new SqlCommand();
            StartOperation.CommandType = System.Data.CommandType.Text;
            StartOperation.CommandText = "INSERT INTO [dbo].[OperationData] (ItemID, Sequence, OperationID, RunDateTime, EmployeeName, DMPID, BrakePress) VALUES (@ItemID,@Sequence,@OperationID,@RunDateTime,@EmployeeName,@DMPID,@BrakePress)";
            StartOperation.Connection = OperationStart;
            StartOperation.Parameters.AddWithValue("@ItemID", CurrentItemID);
            StartOperation.Parameters.AddWithValue("@Sequence", CurrentItemIDOperation);
            StartOperation.Parameters.AddWithValue("@OperationID", OperationsID.ToString());
            StartOperation.Parameters.AddWithValue("@RunDateTime", Clock_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@BrakePress", BrakePress_ComboBox.Text);
            OperationStart.Open();
            StartOperation.ExecuteNonQuery();
            OperationStart.Close();
        }

        private void OperationDataEnd()
        {
            SqlConnection OperationEnd = new SqlConnection(SQL_Source);
            SqlCommand EndOperation = new SqlCommand();
            EndOperation.CommandType = System.Data.CommandType.Text;
            EndOperation.CommandText = "UPDATE [dbo].[OperationData] SET OperationTime=@OperationTime, PartsManufactured=@PartsManufactured,PartsPerMinute=@PartsPerMinute,SetupTime=@SetupTime WHERE OperationID=@OperationID";
            EndOperation.Connection = OperationEnd;
            EndOperation.Parameters.AddWithValue("@OperationID", OperationsID.ToString());
            EndOperation.Parameters.AddWithValue("@OperationTime", TimeActual.ToString());
            EndOperation.Parameters.AddWithValue("@PartsManufactured", PartsFormed_TextBox.Text);
            EndOperation.Parameters.AddWithValue("@PartsPerMinute", CurrentPPM_TextBox.Text);
            EndOperation.Parameters.AddWithValue("@SetupTime", ItemSetupTime.ToString());
            OperationEnd.Open();
            EndOperation.ExecuteNonQuery();
            OperationEnd.Close();
        }
        

        private void OperationOEEData()
        {
            SqlConnection OperationOEEReport = new SqlConnection(SQL_Source);
            SqlCommand OEEData = new SqlCommand();
            OEEData.CommandType = System.Data.CommandType.Text;
            OEEData.CommandText = "INSERT INTO [dbo].[OperationOEE] (ItemID, Sequence, OperationID, RunDateTime, OperationTime, PlannedTime, Efficiency, EmployeeName, DMPID, BrakePress) VALUES (@ItemID,@Sequence,@OperationID,@RunDateTime,@OperationTime,@PlannedTime,@Efficiency,@EmployeeName,@DMPID,@BrakePress)";
            OEEData.Connection = OperationOEEReport;
            OEEData.Parameters.AddWithValue("@ItemID", OEEItemID);
            OEEData.Parameters.AddWithValue("@Sequence", CurrentItemIDOperation);
            OEEData.Parameters.AddWithValue("@OperationID", OperationsID.ToString());
            OEEData.Parameters.AddWithValue("@RunDateTime", DateTime.Today.ToShortDateString());
            OEEData.Parameters.AddWithValue("@OperationTime", TimeActual.ToString());
            OEEData.Parameters.AddWithValue("@PlannedTime", TimePlanned.ToString());
            OEEData.Parameters.AddWithValue("@Efficiency", Efficiency.ToString());
            OEEData.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            OEEData.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            OEEData.Parameters.AddWithValue("@BrakePress", BrakePress_ComboBox.Text);
            OperationOEEReport.Open();
            OEEData.ExecuteNonQuery();
            OperationOEEReport.Close();
        }

        private void ProgramListUpdate()
        {
            SqlConnection ProgramUpdate = new SqlConnection(SQL_Source);
            SqlCommand UpdateItem = new SqlCommand();
            UpdateItem.CommandType = System.Data.CommandType.Text;
            UpdateItem.CommandText = ProgramListUpdate_SQL;
            UpdateItem.Connection = ProgramUpdate;
            UpdateItem.Parameters.AddWithValue("@ItemID", CurrentItemID);
            UpdateItem.Parameters.AddWithValue("@TotalRuns", ItemRunCount);
            UpdateItem.Parameters.AddWithValue("@PartsManufactured", TotalItemPartsManufactured.ToString());
            UpdateItem.Parameters.AddWithValue("@PartsPerMinute", AveragePPM.ToString());
            ProgramUpdate.Open();
            UpdateItem.ExecuteNonQuery();
            ProgramUpdate.Close();
        }

        private void RefreshSQL()
        {
            SqlConnection connection = new SqlConnection(SQL_Source);
            string BP1176 = RefreshData_SQL;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            DataSet Data = new DataSet();
            dataAdapter.Fill(Data);
            UserProgramGridView.DataSource = Data.Tables[0];
            //ItemID_ComboBox.SelectedItem = null;
        }

        /********************************************************************************************************************
        *  
        *  SQL Data Region Start
        * 
        ********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        *  
        *  OPC Region Start
        *  -- Total: 13
        *  
        *  - ConnectToServer_OPC
        *  - StepCount_DataChanged
        *  - PartCount_RunWorkerCompleted
        *  - WriteSteps_OPC
        *  - PartCount_OPC
        *  - ReadCompleteCallback
        *  - ConnectToOPC_RunWorkerCompleted
        *  - ScanNewJob_OPC
        *  - RunMode_OPC
        *  - SetupMode_OPC
        *  - ItemIDWriteTo_OPC
        *  - OperationSelect_OPC 
        *  - WriteCompleteCallback
        *  
        ********************************************************************************************************************/
        #region

        private void ConnectToServer_OPC(object sender, EventArgs e)
        {
            try
            {
                // OPC Server
                OPCServer = new Opc.Da.Server(OPCFactory, null);
                //OPCServer.Url = new Opc.URL("opcda://OHN66OPC/Matrikon.OPC.AllenBradleyPLCs.1");
                //OPCServer.Url = new Opc.URL("opcda://OHN7009/Matrikon.OPC.AllenBradleyPLCs.1");
                OPCServer.Url = new Opc.URL("opcda://OHN66OPC/Kepware.KEPServerEX.V6");
                OPCServer.Connect();

                // OPC Read Groups
                GroupStateRead = new Opc.Da.SubscriptionState();
                GroupStateRead.Name = "BrakePressConnect";
                GroupStateRead.UpdateRate = 1000;
                GroupStateRead.Active = true;
                GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(GroupStateRead);

                // 
                StepCount_StateRead = new Opc.Da.SubscriptionState();
                StepCount_StateRead.Name = "Step Count";
                StepCount_StateRead.UpdateRate = 1000;
                StepCount_StateRead.Active = false;
                StepCount_GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(StepCount_StateRead);
                //StepCount_GroupRead.DataChanged += new Opc.Da.DataChangedEventHandler(StepCount_DataChanged);

                iNspectExpressData_GroupState = new Opc.Da.SubscriptionState();
                iNspectExpressData_GroupState.Name = "iNspectData";
                iNspectExpressData_GroupState.UpdateRate = 100;
                iNspectExpressData_GroupState.Active = true;
                iNspectExpressData_GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(iNspectExpressData_GroupState);
                iNspectExpressData_GroupRead.DataChanged += new Opc.Da.DataChangedEventHandler(iNspectExpressData_DataChanged_OPC);

                PartProgrammed_GroupState = new Opc.Da.SubscriptionState();
                PartProgrammed_GroupState.Name = "PartProgrammed";
                PartProgrammed_GroupState.UpdateRate = 100;
                PartProgrammed_GroupState.Active = true;
                PartProgrammed_GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(PartProgrammed_GroupState);
                PartProgrammed_GroupRead.DataChanged += new Opc.Da.DataChangedEventHandler(PartProgrammed_DataChanged_OPC);


                // 
                StepNumber_StateWrite = new Opc.Da.SubscriptionState();
                StepNumber_StateWrite.Name = "Step Number";
                StepNumber_StateWrite.UpdateRate = 1000;
                StepNumber_StateWrite.Active = false;
                StepNumber_GroupWrite = (Opc.Da.Subscription)OPCServer.CreateSubscription(StepNumber_StateWrite);

                // PartNotProgrammedRead_OPC()

                PartNotProgrammed_StateRead = new Opc.Da.SubscriptionState();
                PartNotProgrammed_StateRead.Name = "Part Not Programmed Read";
                PartNotProgrammed_StateRead.UpdateRate = 1000;
                PartNotProgrammed_StateRead.Active = true;
                PartNotProgrammed_GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(PartNotProgrammed_StateRead);

                // RunModePartsCompleted_OPC()
                PartComplete_StateRead = new Opc.Da.SubscriptionState();
                PartComplete_StateRead.Name = "Part Complete Read";
                PartComplete_StateRead.UpdateRate = 1000;
                PartComplete_StateRead.Active = true;
                PartComplete_GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(PartComplete_StateRead);

                HMILabel_StateRead = new Opc.Da.SubscriptionState();
                HMILabel_StateRead.Name = "HMI Group";
                HMILabel_StateRead.UpdateRate = 1000;
                HMILabel_StateRead.Active = true;
                HMILabel_GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(HMILabel_StateRead);
                HMILabel_GroupRead.DataChanged += new Opc.Da.DataChangedEventHandler(HMILabel_GroupRead_DataChange);

                NotApplicable_StateRead = new Opc.Da.SubscriptionState();
                NotApplicable_StateRead.Name = "NotApplicable Group";
                NotApplicable_StateRead.UpdateRate = 1000;
                NotApplicable_StateRead.Active = true;
                NotApplicable_GroupRead = (Opc.Da.Subscription)OPCServer.CreateSubscription(NotApplicable_StateRead);
                NotApplicable_GroupRead.DataChanged += new Opc.Da.DataChangedEventHandler(NotApplicable_GroupRead_DataChange);

                // OPC Write Groups

                // ScanNewJob_OPC()
                ScanNewJob_StateWrite = new Opc.Da.SubscriptionState();
                ScanNewJob_StateWrite.Name = "StartNewJob_WriteGroup";
                ScanNewJob_StateWrite.Active = true;
                ScanNewJob_StateWrite.KeepAlive = 3600000;
                ScanNewJob_StateWrite.UpdateRate = 5000;
                ScanNewJob_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(ScanNewJob_StateWrite);

                // RunMode_OPC()
                RunMode_WriteState = new Opc.Da.SubscriptionState();
                RunMode_WriteState.Name = "RunMode_Group";
                RunMode_WriteState.Active = true;
                RunMode_WriteState.KeepAlive = 3600000;
                RunMode_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(RunMode_WriteState);

                // SetupMode_OPC()
                SetupMode_WriteState = new Opc.Da.SubscriptionState();
                SetupMode_WriteState.Name = "SetupMode_Group";
                SetupMode_WriteState.Active = true;
                SetupMode_WriteState.KeepAlive = 3600000;
                SetupMode_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(SetupMode_WriteState);

                //TeachSensor_OPC()
                TeachSensor_StateWrite = new Opc.Da.SubscriptionState();
                TeachSensor_StateWrite.Name = "TeachSensor_WriteGroup";
                TeachSensor_StateWrite.Active = false;
                TeachSensor_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(TeachSensor_StateWrite);

                //ModeButtonChanged_OPC()
                ModeChanged_StateWrite = new Opc.Da.SubscriptionState();
                ModeChanged_StateWrite.Name = "ModeChanged_WriteGroup";
                ModeChanged_StateWrite.Active = false;
                ModeChanged_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(ModeChanged_StateWrite);

                // ItemIDWriteTo_OPC()
                ItemID_StateWrite = new Opc.Da.SubscriptionState();
                ItemID_StateWrite.Name = "NewJob_WriteGroup";
                ItemID_StateWrite.Active = true;
                ItemID_StateWrite.KeepAlive = 3600000;
                ItemID_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(ItemID_StateWrite);

                // OperationSelect_OPC()
                OperationSelection_StateWrite = new Opc.Da.SubscriptionState();
                OperationSelection_StateWrite.Name = "OperationOne_WriteGroup";
                OperationSelection_StateWrite.Active = true;
                OperationSelection_StateWrite.KeepAlive = 3600000;
                OperationSelection_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(OperationSelection_StateWrite);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        
        /*
        private void ModeChanged_OPC()
        {
            try
            {
                Opc.Da.Item[] ModeChanged_OPCWrite = new Opc.Da.Item[6];
                ModeChanged_OPCWrite[0] = new Opc.Da.Item();
                ModeChanged_OPCWrite[0].ItemName = Brake_Press_OPC + "HMI_PB_RUN_MODE";
                ModeChanged_OPCWrite[1] = new Opc.Da.Item();
                ModeChanged_OPCWrite[1].ItemName = Brake_Press_OPC + "HMI_PB_Scan_New_Part";
                ModeChanged_OPCWrite[2] = new Opc.Da.Item();
                ModeChanged_OPCWrite[2].ItemName = Brake_Press_OPC + "HMI_PB_SETUP_MODE";
                ModeChanged_OPCWrite[3] = new Opc.Da.Item();
                ModeChanged_OPCWrite[3].ItemName = Brake_Press_OPC + "HMI_Operation_One_PB";
                ModeChanged_OPCWrite[4] = new Opc.Da.Item();
                ModeChanged_OPCWrite[4].ItemName = Brake_Press_OPC + "HMI_Operation_Two_PB";
                ModeChanged_OPCWrite[5] = new Opc.Da.Item();
                ModeChanged_OPCWrite[5].ItemName = Brake_Press_OPC + "HMI_Operation_Three_PB";
                ModeChanged_OPCWrite = ModeChanged_Write.AddItems(ModeChanged_OPCWrite);


                Opc.Da.ItemValue[] ModeChanged_OPCWriteValue = new Opc.Da.ItemValue[6];
                ModeChanged_OPCWriteValue[0] = new Opc.Da.ItemValue();
                ModeChanged_OPCWriteValue[1] = new Opc.Da.ItemValue();
                ModeChanged_OPCWriteValue[2] = new Opc.Da.ItemValue();
                ModeChanged_OPCWriteValue[3] = new Opc.Da.ItemValue();
                ModeChanged_OPCWriteValue[4] = new Opc.Da.ItemValue();
                ModeChanged_OPCWriteValue[5] = new Opc.Da.ItemValue();

                switch (SelectedButton)
                {
                    case 1: // Scan New Job         
                        ModeChanged_OPCWriteValue[0].ServerHandle = ModeChanged_Write.Items[0].ServerHandle; // Run Mode
                        ModeChanged_OPCWriteValue[0].Value = 0;
                        ModeChanged_OPCWriteValue[1].ServerHandle = ModeChanged_Write.Items[1].ServerHandle; // Scan New Job
                        ModeChanged_OPCWriteValue[1].Value = 1;
                        ModeChanged_OPCWriteValue[2].ServerHandle = ModeChanged_Write.Items[2].ServerHandle; //Setup Mode
                        ModeChanged_OPCWriteValue[2].Value = 1;
                        ModeChanged_OPCWriteValue[3].ServerHandle = ModeChanged_Write.Items[3].ServerHandle; // Operation One
                        ModeChanged_OPCWriteValue[3].Value = 0;
                        ModeChanged_OPCWriteValue[4].ServerHandle = ModeChanged_Write.Items[4].ServerHandle; // Operation Two
                        ModeChanged_OPCWriteValue[4].Value = 0;
                        ModeChanged_OPCWriteValue[5].ServerHandle = ModeChanged_Write.Items[5].ServerHandle; // Operation Three
                        ModeChanged_OPCWriteValue[5].Value = 0;
                        break;

                    case 2: // Run Mode
                        ModeChanged_OPCWriteValue[0].ServerHandle = ModeChanged_Write.Items[0].ServerHandle; // Run Mode
                        ModeChanged_OPCWriteValue[0].Value = 1;
                        ModeChanged_OPCWriteValue[1].ServerHandle = ModeChanged_Write.Items[1].ServerHandle; // Scan New Job
                        ModeChanged_OPCWriteValue[1].Value = 0;
                        ModeChanged_OPCWriteValue[2].ServerHandle = ModeChanged_Write.Items[2].ServerHandle; //Setup Mode
                        ModeChanged_OPCWriteValue[2].Value = 0;
                        ModeChanged_OPCWriteValue[3].ServerHandle = ModeChanged_Write.Items[3].ServerHandle; // Operation One
                        ModeChanged_OPCWriteValue[3].Value = null;
                        ModeChanged_OPCWriteValue[4].ServerHandle = ModeChanged_Write.Items[4].ServerHandle; // Operation Two
                        ModeChanged_OPCWriteValue[4].Value = null;
                        ModeChanged_OPCWriteValue[5].ServerHandle = ModeChanged_Write.Items[5].ServerHandle; // Operation Three
                        ModeChanged_OPCWriteValue[5].Value = null;
                        break;

                    case 3: // Setup Mode
                        ModeChanged_OPCWriteValue[0].ServerHandle = ModeChanged_Write.Items[0].ServerHandle; // Run Mode
                        ModeChanged_OPCWriteValue[0].Value = 0;
                        ModeChanged_OPCWriteValue[1].ServerHandle = ModeChanged_Write.Items[1].ServerHandle; // Scan New Job
                        ModeChanged_OPCWriteValue[1].Value = 0;
                        ModeChanged_OPCWriteValue[2].ServerHandle = ModeChanged_Write.Items[2].ServerHandle; //Setup Mode
                        ModeChanged_OPCWriteValue[2].Value = 1;
                        ModeChanged_OPCWriteValue[3].ServerHandle = ModeChanged_Write.Items[3].ServerHandle; // Operation One
                        ModeChanged_OPCWriteValue[3].Value = null;
                        ModeChanged_OPCWriteValue[4].ServerHandle = ModeChanged_Write.Items[4].ServerHandle; // Operation Two
                        ModeChanged_OPCWriteValue[4].Value = null;
                        ModeChanged_OPCWriteValue[5].ServerHandle = ModeChanged_Write.Items[5].ServerHandle; // Operation Three
                        ModeChanged_OPCWriteValue[5].Value = null;
                        break;
                }
                Opc.IRequest OPCRequest;
                ModeChanged_Write.Write(ModeChanged_OPCWriteValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out OPCRequest);
            }
            catch (Exception ex)
            {
                //OPCServer.Connect();
                MessageBox.Show(ex.Message);
            }
        }
        */
        
        void StepCount_DataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            if(System.Environment.MachineName == "OHN7066") // BP1083
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1083_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1083_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if(System.Environment.MachineName == "OHN7017") // BP1107
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1107_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1107_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if (System.Environment.MachineName == "OHN7120") // BP1127
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1127_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1127_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if (System.Environment.MachineName == "OHN7082") // BP1156
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1156_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1156_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if (System.Environment.MachineName == "OHN7148") // BP1720
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1720_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1720_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if (System.Environment.MachineName == "OHN7121") // BP1155
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1155_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1155_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if (System.Environment.MachineName == "OHN7067") // BP1158
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1158_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1158_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if (System.Environment.MachineName == "OHN7122") // BP1175
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1175_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1175_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            else if (System.Environment.MachineName == "OHN7009") // BP1176
            {
                foreach (ItemValueResult itemValue in values)
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1176_Sequence_Number":
                            StepCount = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Prgm_MainProgram.Brake_1176_Part_Counter":
                            PartsFormed_OPC = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
        }

        private void PartCount_RunWorkerCompleted(object sender, EventArgs e)
        {
            FaultValue();
        }            

        private void WriteSteps_OPC()
        {
                     
            Opc.Da.Item[] OPC_StepWrite = new Opc.Da.Item[1];
            OPC_StepWrite[0] = new Opc.Da.Item();
            OPC_StepWrite[0].ItemName = Brake_Press_Fault_OPC + "Step_Number";
            OPC_WriteSteps.Add(OPC_StepWrite[0]);
            StepNumber_GroupWrite.AddItems(OPC_WriteSteps.ToArray());

            Opc.Da.ItemValue[] WriteValue = new Opc.Da.ItemValue[1];
            WriteValue[0] = new Opc.Da.ItemValue();
            WriteValue[0].ServerHandle = StepNumber_GroupWrite.Items[0].ServerHandle;
            WriteValue[0].Value = NumberOfSteps;

            Opc.IRequest req;
            StepNumber_GroupWrite.Write(WriteValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out req);
        }
        
        private void PartCount_OPC(object sender, EventArgs e)
        {
            Opc.Da.Item[] OPC_CountStep = new Opc.Da.Item[3];
            OPC_CountStep[0] = new Opc.Da.Item();
            OPC_CountStep[0].ItemName = Brake_Press_Fault_OPC + "Sequence_Number";
            OPC_StepCount.Add(OPC_CountStep[0]);
            OPC_CountStep[1] = new Opc.Da.Item();
            OPC_CountStep[1].ItemName = Brake_Press_Fault_OPC + "Part_Counter";
            OPC_StepCount.Add(OPC_CountStep[1]);
            OPC_CountStep[2] = new Opc.Da.Item();
            OPC_CountStep[2].ItemName = Brake_Press_Fault_OPC + "Sequence_Fault";
            OPC_StepCount.Add(OPC_CountStep[2]);

            StepCount_GroupRead.AddItems(OPC_StepCount.ToArray());

            Opc.IRequest req;
            StepCount_GroupRead.Read(StepCount_GroupRead.Items, 123, new Opc.Da.ReadCompleteEventHandler(ReadCompleteCallback), out req);
            // Check_TeachSensor();
        }

        private void NotApplicable_OPC()
        {
            Opc.Da.Item[] OPC_NotApplicable = new Opc.Da.Item[1];
            OPC_NotApplicable[0] = new Opc.Da.Item();
            OPC_NotApplicable[0].ItemName = Brake_Press_ID + "HMI_VISION_NOT_APPLICABLE_MESSAGE";
            OPC_Not_Applicable.Add(OPC_NotApplicable[0]);

            NotApplicable_GroupRead.AddItems(OPC_Not_Applicable.ToArray());

            Opc.IRequest req;
            NotApplicable_GroupRead.Read(NotApplicable_GroupRead.Items, 123, new Opc.Da.ReadCompleteEventHandler(ReadCompleteCallback_NotApplicable), out req);
            // Check_TeachSensor();
        }

        //BrakePress1720.Vision_Part_ID_From_Camera

        private void iNspectExpressData_OPC()
        {
            Opc.Da.Item[] OPC_iNspectData = new Opc.Da.Item[2];
            OPC_iNspectData[0] = new Opc.Da.Item();
            OPC_iNspectData[0].ItemName = Brake_Press_iNspect + "Vision_Part_ID_From_Camera";
            OPC_iNspectList.Add(OPC_iNspectData[0]);
            OPC_iNspectData[1] = new Opc.Da.Item();
            OPC_iNspectData[1].ItemName = Brake_Press_iNspect + "Vision_Part_ID_From_PLC";
            OPC_iNspectList.Add(OPC_iNspectData[1]);

            iNspectExpressData_GroupRead.AddItems(OPC_iNspectList.ToArray());
            Opc.IRequest req;
            iNspectExpressData_GroupRead.Read(iNspectExpressData_GroupRead.Items, 123, new Opc.Da.ReadCompleteEventHandler(ReadCompleteCallback_iNspectExpress), out req);

            //iNspect_Timer.Enabled = true;
        }

        private void ReadCompleteCallback_iNspectExpress(object clientHandle, Opc.Da.ItemValueResult[] results)
        {
            //iNspectCamera_TextBox.Invoke(new EventHandler(delegate { iNspectCamera_TextBox.Text = (results[0].Value).ToString(); }));
            //iNspectPLC_TextBox.Invoke(new EventHandler(delegate { iNspectPLC_TextBox.Text = (results[1].Value).ToString(); }));
        }

        void iNspectExpressData_DataChanged_OPC(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            foreach (ItemValueResult itemValue in values) // 1107
            {
                switch (itemValue.ItemName)
                {
                    case "OHN66OPC.BrakePress_ControlLogix.Global.BrakePress1720.Vision_Part_ID_From_Camera":
                        iNspectVisionID_Camera = Convert.ToString(itemValue.Value);
                       // iNspectCamera_TextBox.Text = iNspectVisionID_Camera;
                        break;

                    case "OHN66OPC.BrakePress_ControlLogix.Global.BrakePress1720.Vision_Part_ID_From_PLC":
                        iNspectVisionID_PLC = Convert.ToString(itemValue.Value);
                       // iNspectPLC_TextBox.Text = iNspectVisionID_PLC;
                        break;
                }
            }
        }

        private void iNspect_Timer_Tick(object sender, EventArgs e)
        {

        }

        private void HMILabels_OPC()
        {
            Opc.Da.Item[] OPC_HMI_Message = new Opc.Da.Item[3];
            OPC_HMI_Message[0] = new Opc.Da.Item();
            OPC_HMI_Message[0].ItemName = Brake_Press_ID + "HMI_VISION_NOT_APPLICABLE_MESSAGE";
            OPC_Message_HMI.Add(OPC_HMI_Message[0]);
            OPC_HMI_Message[1] = new Opc.Da.Item();
            OPC_HMI_Message[1].ItemName = Brake_Press_ID + "HMI_OK_TO_CYCLE_MESSAGE";
            OPC_Message_HMI.Add(OPC_HMI_Message[1]);
            OPC_HMI_Message[2] = new Opc.Da.Item();
            OPC_HMI_Message[2].ItemName = Brake_Press_ID + "HMI_Message_NO_Camera_Pulse";
            OPC_Message_HMI.Add(OPC_HMI_Message[2]);

            HMILabel_GroupRead.AddItems(OPC_Message_HMI.ToArray());

            Opc.IRequest req;
            HMILabel_GroupRead.Read(HMILabel_GroupRead.Items, 123, new Opc.Da.ReadCompleteEventHandler(ReadCompleteCallback_HMI), out req);
            // Check_TeachSensor();
        }

        private void ReadCompleteCallback_HMI(object clientHandle, Opc.Da.ItemValueResult[] results)
        {
         //   HMINotApp_TextBox.Invoke(new EventHandler(delegate { HMINotApp_TextBox.Text = (results[0].Value).ToString(); }));
          //  HMICycle_TextBox.Invoke(new EventHandler(delegate { HMICycle_TextBox.Text = (results[1].Value).ToString(); }));
            //HMIPulse_TextBox.Invoke(new EventHandler(delegate { HMIPulse_TextBox.Text = (results[2].Value).ToString(); }));
        }

        private void ReadCompleteCallback_NotApplicable (object clientHandle, Opc.Da.ItemValueResult[] results)
        {
            //   HMINotApp_TextBox.Invoke(new EventHandler(delegate { HMINotApp_TextBox.Text = (results[0].Value).ToString(); }));
            //  HMICycle_TextBox.Invoke(new EventHandler(delegate { HMICycle_TextBox.Text = (results[1].Value).ToString(); }));
            //HMIPulse_TextBox.Invoke(new EventHandler(delegate { HMIPulse_TextBox.Text = (results[2].Value).ToString(); }));
        }

        void HMILabel_GroupRead_DataChange(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            // CAT Brake Press
            if (System.Environment.MachineName == "OHN7017")
            {
                foreach (ItemValueResult itemValue in values) // 1107
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1107_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1107_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1107_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "BP1139")
            {
                foreach (ItemValueResult itemValue in values) // 1139
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1139_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1139_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1139_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "BP1177")
            {
                foreach (ItemValueResult itemValue in values) // 1177
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1177_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1177_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1177_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // John Deere Brake Press
            if (System.Environment.MachineName == "OHN7120")
            {
                foreach (ItemValueResult itemValue in values) // 1127
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1127_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1127_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1127_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // Navistar Brake Press
            if (System.Environment.MachineName == "BP1065")
            {
                foreach (ItemValueResult itemValue in values) // 1065
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1065_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1065_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1065_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7052")
            {
                foreach (ItemValueResult itemValue in values) // 1108
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1108_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1108_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1108_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7082")
            {
                foreach (ItemValueResult itemValue in values) // 1156
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1156_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1156_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1156_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7148")
            {
                foreach (ItemValueResult itemValue in values) // 1720
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // Paccar Brake Press
            if (System.Environment.MachineName == "OHN7066")
            {
                foreach (ItemValueResult itemValue in values) // 1083
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1083_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1083_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1083_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7121")
            {
                foreach (ItemValueResult itemValue in values) // 1155
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1155_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1155_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1155_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7067")
            {
                foreach (ItemValueResult itemValue in values) // 1158
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1158_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1158_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1158_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7122")
            {
                foreach (ItemValueResult itemValue in values) // 1175
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1175_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1175_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1175_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7009")
            {
                foreach (ItemValueResult itemValue in values) // 1176
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.Brake_Press_ControlLogix.Global.HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Global.HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.Brake_Press_ControlLogix.Global.HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // My Laptop
            if (System.Environment.MachineName == "OHN7047NL")
            {
                foreach (ItemValueResult itemValue in values) // My Laptop
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_OK_TO_CYCLE_MESSAGE":
                            HMI_Cycle = Convert.ToString(itemValue.Value);
                            break;

                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_Message_NO_Camera_Pulse":
                            HMI_Pulse = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
        }

        void NotApplicable_GroupRead_DataChange(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            // CAT Brake Press
            if (System.Environment.MachineName == "OHN7017")
            {
                foreach (ItemValueResult itemValue in values) // 1107
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1107_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "BP1139")
            {
                foreach (ItemValueResult itemValue in values) // 1139
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1139_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "BP1177")
            {
                foreach (ItemValueResult itemValue in values) // 1107
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1177_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // John Deere Brake Press
            if (System.Environment.MachineName == "OHN7120")
            {
                foreach (ItemValueResult itemValue in values) // 1127
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1127_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // Navistar Brake Press
            if (System.Environment.MachineName == "BP1065")
            {
                foreach (ItemValueResult itemValue in values) // 1065
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1065_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7052")
            {
                foreach (ItemValueResult itemValue in values) // 1108
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1108_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7082")
            {
                foreach (ItemValueResult itemValue in values) // 1156
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1156_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7148")
            {
                foreach (ItemValueResult itemValue in values) // 1720
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // Paccar Brake Press
            if (System.Environment.MachineName == "OHN7066") 
            {
                foreach (ItemValueResult itemValue in values) // 1083
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1083_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7121")
            {
                foreach (ItemValueResult itemValue in values) // 1155
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1155_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7067")
            {
                foreach (ItemValueResult itemValue in values) // 1158
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1158_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7122")
            {
                foreach (ItemValueResult itemValue in values) // 1175
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1175_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            if (System.Environment.MachineName == "OHN7009")
            {
                foreach (ItemValueResult itemValue in values) // 1176
                {
                    switch (itemValue.ItemName)
                    {                        
                        case "OHN66OPC.Brake_Press_ControlLogix.Global.HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
            // My Laptop
            if (System.Environment.MachineName == "OHN7047NL")
            {
                foreach (ItemValueResult itemValue in values) // My Laptop
                {
                    switch (itemValue.ItemName)
                    {
                        case "OHN66OPC.BrakePress_ControlLogix.Global.B1720_HMI_VISION_NOT_APPLICABLE_MESSAGE":
                            HMI_App = Convert.ToString(itemValue.Value);
                            break;
                    }
                }
            }
        }

        private void NotApplicable_Check()
        {
            HMINotApp_TextBox.Text = HMI_App;
            NA_Count = NA_Count + 1;

            if (HMINotApp_TextBox.Text == "True")
            {
                HMI_Message_TextBox.Text = "VISION JOB NOT APPLICABLE";
                HMI_Message_TextBox.BackColor = System.Drawing.SystemColors.ControlDark;
            }
            //else if (HMINotApp_TextBox.Text == "False")
            //{
            //    HMI_Message_TextBox.Text = "";
            //    HMI_Message_TextBox.BackColor = System.Drawing.Color.LightGray;
            //}
            if(NA_Count > 10)
            {
                NotApplicable_Timer.Stop();
                NA_Count = 0;
            }
        }



        private void HMI_Check()
        {
            //HMINotApp_TextBox.Text = HMI_App;
            HMICycle_TextBox.Text = HMI_Cycle;
            HMIPulse_TextBox.Text = HMI_Pulse;
            if (HMICycle_TextBox.Text == 0.ToString())
            {
                //HMI_CheckPlacement_TextBox.Visible = false;
                //HMI_CheckPlacement_TextBox.SendToBack();
                //HMI_RunMode_TextBox.Visible = false;
                //HMI_RunMode_TextBox.SendToBack();

                HMI_Message_TextBox.Text = "";
                HMI_Message_TextBox.BackColor = System.Drawing.Color.LightGray;

            }
            else if (HMICycle_TextBox.Text == 1.ToString())
            {
                //HMI_CheckPlacement_TextBox.Visible = false;
                //HMI_CheckPlacement_TextBox.SendToBack();
                //HMI_RunMode_TextBox.Visible = true;
                //HMI_RunMode_TextBox.BringToFront();

                HMI_Message_TextBox.Text = "OK TO CYCLE";
                HMI_Message_TextBox.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);

            }
            else if (HMICycle_TextBox.Text == 2.ToString())
            {
                //HMI_CheckPlacement_TextBox.Visible = true;
                //HMI_CheckPlacement_TextBox.BringToFront();
                //HMI_RunMode_TextBox.Visible = false;
                //HMI_RunMode_TextBox.SendToBack();

                HMI_Message_TextBox.Text = "PLEASE CHECK PART PLACEMENT";
                HMI_Message_TextBox.BackColor = System.Drawing.Color.Red;

            }
            if(HMIPulse_TextBox.Text == "True")
            {
                //HMI_NotActive_TextBox.Visible = true;
                //HMI_NotActive_TextBox.BringToFront();

                HMI_Message_TextBox.Text = "CAMERA NOT ACTIVE, OK TO CYCLE PRESS";
                HMI_Message_TextBox.BackColor = System.Drawing.SystemColors.ControlDark;

            }
            else if (HMIPulse_TextBox.Text == "False")
            {
                //HMI_NotActive_TextBox.Visible = false;

                HMI_Message_TextBox.Text = "";
                HMI_Message_TextBox.BackColor = System.Drawing.Color.LightGray;
            }

        }

        private void ReadCompleteCallback(object clientHandle, Opc.Da.ItemValueResult[] results)
        {
            //CurrentSteps_TextBox.Invoke(new EventHandler(delegate { CurrentSteps_TextBox.Text = (results[0].Value).ToString(); }));
            //PartsFormed_TextBox.Invoke(new EventHandler(delegate { PartsFormed_TextBox.Text = (results[1].Value).ToString(); }));
            //FaultReset_TextBox.Invoke(new EventHandler(delegate { FaultReset_TextBox.Text = (results[2].Value).ToString(); }));
        }

        private void ConnectToOPC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ScanNewJob_Button.Enabled = true;
        }              

        private void ScanNewJob_OPC()
        {
            try
            {                
                Opc.Da.Item[] OPC_ScanNewJob = new Opc.Da.Item[7];
                OPC_ScanNewJob[0] = new Opc.Da.Item();
                OPC_ScanNewJob[0].ItemName = Brake_Press_ID + "HMI_PB_RUN_MODE_IND";
                OPC_ScanNewJob[1] = new Opc.Da.Item();
                OPC_ScanNewJob[1].ItemName = Brake_Press_ID + "HMI_PB_SCAN_NEW_PART";
                OPC_ScanNewJob[2] = new Opc.Da.Item();
                OPC_ScanNewJob[2].ItemName = Brake_Press_ID + "HMI_PB_SETUP_MODE";
                OPC_ScanNewJob[3] = new Opc.Da.Item();
                OPC_ScanNewJob[3].ItemName = Brake_Press_ID + "HMI_PB_SETUP_MODE_IND";
                OPC_ScanNewJob[4] = new Opc.Da.Item();
                OPC_ScanNewJob[4].ItemName = Brake_Press_ID + "HMI_Operation_One_PB";
                OPC_ScanNewJob[5] = new Opc.Da.Item();
                OPC_ScanNewJob[5].ItemName = Brake_Press_ID + "HMI_Operation_Two_PB";
                OPC_ScanNewJob[6] = new Opc.Da.Item();
                OPC_ScanNewJob[6].ItemName = Brake_Press_ID + "HMI_Operation_Three_PB";
                OPC_ScanNewJob = ScanNewJob_Write.AddItems(OPC_ScanNewJob);
                

                /*
                Opc.Da.Item[] OPC_ScanNewJob = new Opc.Da.Item[6];
                OPC_ScanNewJob[0] = new Opc.Da.Item();
                OPC_ScanNewJob[0].ItemName = Brake_Press_OPC + "HMI_PB_RUN_MODE";
                OPC_ScanNewJob[1] = new Opc.Da.Item();
                OPC_ScanNewJob[1].ItemName = Brake_Press_OPC + "HMI_PB_Scan_New_Part";
                OPC_ScanNewJob[2] = new Opc.Da.Item();
                OPC_ScanNewJob[2].ItemName = Brake_Press_OPC + "HMI_PB_SETUP_MODE";
                OPC_ScanNewJob[3] = new Opc.Da.Item();
                OPC_ScanNewJob[3].ItemName = Brake_Press_OPC + "HMI_Operation_One_PB";
                OPC_ScanNewJob[4] = new Opc.Da.Item();
                OPC_ScanNewJob[4].ItemName = Brake_Press_OPC + "HMI_Operation_Two_PB";
                OPC_ScanNewJob[5] = new Opc.Da.Item();
                OPC_ScanNewJob[5].ItemName = Brake_Press_OPC + "HMI_Operation_Three_PB";
                OPC_ScanNewJob = ScanNewJob_Write.AddItems(OPC_ScanNewJob);
                */
                Opc.Da.ItemValue[] OPC_ScanNewJobValue = new Opc.Da.ItemValue[7];
                OPC_ScanNewJobValue[0] = new Opc.Da.ItemValue();
                OPC_ScanNewJobValue[0].ServerHandle = ScanNewJob_Write.Items[0].ServerHandle;
                OPC_ScanNewJobValue[0].Value = 0;
                OPC_ScanNewJobValue[1] = new Opc.Da.ItemValue();
                OPC_ScanNewJobValue[1].ServerHandle = ScanNewJob_Write.Items[1].ServerHandle;
                OPC_ScanNewJobValue[1].Value = 1;
                OPC_ScanNewJobValue[2] = new Opc.Da.ItemValue();
                OPC_ScanNewJobValue[2].ServerHandle = ScanNewJob_Write.Items[2].ServerHandle;
                OPC_ScanNewJobValue[2].Value = 0;
                OPC_ScanNewJobValue[3] = new Opc.Da.ItemValue();
                OPC_ScanNewJobValue[3].ServerHandle = ScanNewJob_Write.Items[3].ServerHandle;
                OPC_ScanNewJobValue[3].Value = 0;
                OPC_ScanNewJobValue[4] = new Opc.Da.ItemValue();
                OPC_ScanNewJobValue[4].ServerHandle = ScanNewJob_Write.Items[4].ServerHandle;
                OPC_ScanNewJobValue[4].Value = 0;
                OPC_ScanNewJobValue[5] = new Opc.Da.ItemValue();
                OPC_ScanNewJobValue[5].ServerHandle = ScanNewJob_Write.Items[5].ServerHandle;
                OPC_ScanNewJobValue[5].Value = 0;
                OPC_ScanNewJobValue[6] = new Opc.Da.ItemValue();
                OPC_ScanNewJobValue[6].ServerHandle = ScanNewJob_Write.Items[6].ServerHandle;
                OPC_ScanNewJobValue[6].Value = 0;
                Opc.IRequest OPCRequest;
                ScanNewJob_Write.Write(OPC_ScanNewJobValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out OPCRequest);
            }
            catch (Exception ex)
            {
                //OPCServer.Connect();
                MessageBox.Show(ex.Message);
            }
        }

        private void RunMode_OPC()
        {
            try
            {                
                Opc.Da.Item[] OPC_RunMode = new Opc.Da.Item[4];
                OPC_RunMode[0] = new Opc.Da.Item();
                OPC_RunMode[0].ItemName = Brake_Press_ID + "HMI_PB_RUN_MODE_IND";
                OPC_RunMode[1] = new Opc.Da.Item();
                OPC_RunMode[1].ItemName = Brake_Press_ID + "HMI_PB_SCAN_NEW_PART";
                OPC_RunMode[2] = new Opc.Da.Item();
                OPC_RunMode[2].ItemName = Brake_Press_ID + "HMI_PB_SETUP_MODE";
                OPC_RunMode[3] = new Opc.Da.Item();
                OPC_RunMode[3].ItemName = Brake_Press_ID + "HMI_PB_SETUP_MODE_IND";
                /*
                Opc.Da.Item[] OPC_RunMode = new Opc.Da.Item[3];
                OPC_RunMode[0] = new Opc.Da.Item();
                OPC_RunMode[0].ItemName = Brake_Press_OPC + "HMI_PB_RUN_MODE";
                OPC_RunMode[1] = new Opc.Da.Item();
                OPC_RunMode[1].ItemName = Brake_Press_OPC + "HMI_PB_Scan_New_Part";
                OPC_RunMode[2] = new Opc.Da.Item();
                OPC_RunMode[2].ItemName = Brake_Press_OPC + "HMI_PB_SETUP_MODE";
                */

                OPC_RunMode = RunMode_Write.AddItems(OPC_RunMode);

                Opc.Da.ItemValue[] OPC_RunModeValue = new Opc.Da.ItemValue[4];
                OPC_RunModeValue[0] = new Opc.Da.ItemValue();
                OPC_RunModeValue[0].ServerHandle = RunMode_Write.Items[0].ServerHandle;
                OPC_RunModeValue[0].Value = 1;
                OPC_RunModeValue[1] = new Opc.Da.ItemValue();
                OPC_RunModeValue[1].ServerHandle = RunMode_Write.Items[1].ServerHandle;
                OPC_RunModeValue[1].Value = 0;
                OPC_RunModeValue[2] = new Opc.Da.ItemValue();
                OPC_RunModeValue[2].ServerHandle = RunMode_Write.Items[2].ServerHandle;
                OPC_RunModeValue[2].Value = 0;
                OPC_RunModeValue[3] = new Opc.Da.ItemValue();
                OPC_RunModeValue[3].ServerHandle = RunMode_Write.Items[3].ServerHandle;
                OPC_RunModeValue[3].Value = 0;

                Opc.IRequest OPCRequest;
                RunMode_Write.Write(OPC_RunModeValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out OPCRequest);
            }
            catch (Exception ex)
            {
                //OPCServer.Connect();
                MessageBox.Show(ex.Message);
            }
        }

        private void SetupMode_OPC()
        {
            try
            {                
                Opc.Da.Item[] OPC_SetupMode = new Opc.Da.Item[3];
                OPC_SetupMode[0] = new Opc.Da.Item();
                OPC_SetupMode[0].ItemName = Brake_Press_ID + "HMI_PB_RUN_MODE_IND";
                OPC_SetupMode[1] = new Opc.Da.Item();
                OPC_SetupMode[1].ItemName = Brake_Press_ID + "HMI_PB_SCAN_NEW_PART";
                OPC_SetupMode[2] = new Opc.Da.Item();
                OPC_SetupMode[2].ItemName = Brake_Press_ID + "HMI_PB_SETUP_MODE";
                OPC_SetupMode = SetupMode_Write.AddItems(OPC_SetupMode);
                /*
                Opc.Da.Item[] OPC_SetupMode = new Opc.Da.Item[3];
                OPC_SetupMode[0] = new Opc.Da.Item();
                OPC_SetupMode[0].ItemName = Brake_Press_OPC + "HMI_PB_RUN_MODE";
                OPC_SetupMode[1] = new Opc.Da.Item();
                OPC_SetupMode[1].ItemName = Brake_Press_OPC + "HMI_PB_Scan_New_Part";
                OPC_SetupMode[2] = new Opc.Da.Item();
                OPC_SetupMode[2].ItemName = Brake_Press_OPC + "HMI_PB_SETUP_MODE";
                OPC_SetupMode = SetupMode_Write.AddItems(OPC_SetupMode);
                */
                Opc.Da.ItemValue[] OPC_SetupModeValue = new Opc.Da.ItemValue[3];
                OPC_SetupModeValue[0] = new Opc.Da.ItemValue();
                OPC_SetupModeValue[0].ServerHandle = SetupMode_Write.Items[0].ServerHandle;
                OPC_SetupModeValue[0].Value = 0;
                OPC_SetupModeValue[1] = new Opc.Da.ItemValue();
                OPC_SetupModeValue[1].ServerHandle = SetupMode_Write.Items[1].ServerHandle;
                OPC_SetupModeValue[1].Value = 0;
                OPC_SetupModeValue[2] = new Opc.Da.ItemValue();
                OPC_SetupModeValue[2].ServerHandle = SetupMode_Write.Items[2].ServerHandle;
                OPC_SetupModeValue[2].Value = 1;
                Opc.IRequest OPCRequest;
                SetupMode_Write.Write(OPC_SetupModeValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out OPCRequest);
            }
            catch (Exception ex)
            {
                //OPCServer.Connect();
                MessageBox.Show(ex.Message);
            }
        }

        private void ItemIDWriteTo_OPC()
        {
            try
            {                
                Opc.Da.Item[] OPC_ItemID = new Opc.Da.Item[1];
                OPC_ItemID[0] = new Opc.Da.Item();
                OPC_ItemID[0].ItemName = Brake_Press_ID + "Barcode_Read_Results.DATA";
                OPC_ItemID = ItemID_Write.AddItems(OPC_ItemID);
                /*
                Opc.Da.Item[] OPC_ItemID = new Opc.Da.Item[1];
                OPC_ItemID[0] = new Opc.Da.Item();
                OPC_ItemID[0].ItemName = Brake_Press_OPC + "Barcode_Read_Results.DATA";
                OPC_ItemID = ItemID_Write.AddItems(OPC_ItemID);
                */
                Opc.Da.ItemValue[] OPC_ItemIDValue = new Opc.Da.ItemValue[1];
                OPC_ItemIDValue[0] = new Opc.Da.ItemValue();
                OPC_ItemIDValue[0].ServerHandle = ItemID_Write.Items[0].ServerHandle;
                OPC_ItemIDValue[0].Value = CurrentItemID.ToString();

                Opc.IRequest OPCRequest;
                ItemID_Write.Write(OPC_ItemIDValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out OPCRequest);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }

        private void OperationSelect_OPC()
        {
            try
            {                
                Opc.Da.Item[] OPC_Operation = new Opc.Da.Item[3];
                OPC_Operation[0] = new Opc.Da.Item();
                OPC_Operation[0].ItemName = Brake_Press_ID + "HMI_Operation_One_PB";
                OPC_Operation[1] = new Opc.Da.Item();
                OPC_Operation[1].ItemName = Brake_Press_ID + "HMI_Operation_Two_PB";
                OPC_Operation[2] = new Opc.Da.Item();
                OPC_Operation[2].ItemName = Brake_Press_ID + "HMI_Operation_Three_PB";
                OPC_Operation = OperationSelection_Write.AddItems(OPC_Operation);
                /*
                Opc.Da.Item[] OPC_Operation = new Opc.Da.Item[3];
                OPC_Operation[0] = new Opc.Da.Item();
                OPC_Operation[0].ItemName = Brake_Press_OPC + "HMI_Operation_One_PB";
                OPC_Operation[1] = new Opc.Da.Item();
                OPC_Operation[1].ItemName = Brake_Press_OPC + "HMI_Operation_Two_PB";
                OPC_Operation[2] = new Opc.Da.Item();
                OPC_Operation[2].ItemName = Brake_Press_OPC + "HMI_Operation_Three_PB";
                OPC_Operation = OperationSelection_Write.AddItems(OPC_Operation);
                */
                Opc.Da.ItemValue[] OPC_OperationOneValue = new Opc.Da.ItemValue[3];
                OPC_OperationOneValue[0] = new Opc.Da.ItemValue();
                OPC_OperationOneValue[0].ServerHandle = OperationSelection_Write.Items[0].ServerHandle;
                OPC_OperationOneValue[0].Value = 1;
                OPC_OperationOneValue[1] = new Opc.Da.ItemValue();
                OPC_OperationOneValue[1].ServerHandle = OperationSelection_Write.Items[1].ServerHandle;
                OPC_OperationOneValue[1].Value = 0;
                OPC_OperationOneValue[2] = new Opc.Da.ItemValue();
                OPC_OperationOneValue[2].ServerHandle = OperationSelection_Write.Items[2].ServerHandle;
                OPC_OperationOneValue[2].Value = 0;

                Opc.IRequest WriteRequest;
                OperationSelection_Write.Write(OPC_OperationOneValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out WriteRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteCompleteCallback(object clientHandle, Opc.IdentifiedResult[] results)
        {
            foreach (Opc.IdentifiedResult writeResult in results)
            {
                Console.WriteLine("\t{0} write result: {1}", writeResult.ItemName, writeResult.ResultID);
            }
        }

        /********************************************************************************************************************
        *  
        *  OPC Region End
        * 
        ********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Methods Region Start 
        * -- Total: 16
        * 
        * - RunningStatistics
        * - SearchForItemID
        * - PassOperationValue
        * - FaultValue
        * - LivePPMOEECalculation
        * - ClearTextBoxes
        * - BarcodeScanner
        * - PassValue
        * - ItemID TextBox KeyDown
        * - ClearForm
        * - SearchItemIDOperation
        * - BrakePressID
        * - CheckTime Check
        * - ItemOperationCalculation
        * - OperationOEECalculation
        * - FindTotalRunTime
        * 
        **********************************************************************************************************************/
        #region

        private void SearchForItemID()
        {
            string SearchValue = CurrentItemID;
            string OperationValue = CurrentItemIDOperation.ToString();
            UserProgramGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                foreach (DataGridViewRow Row in UserProgramGridView.Rows)
                {
                    Row.Selected = false;
                    if (Row.Cells[0].Value.ToString().Equals(SearchValue) && Row.Cells[4].Value.ToString().Equals(OperationValue) && Row.Cells[Brake_Press_SQL].Value.ToString().Equals("Yes"))
                    { 
                        Row.Selected = true;
                        ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                        Customer = Row.Cells[1].Value.ToString();
                        CustomerPartNumber = Row.Cells[2].Value.ToString();
                        JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                        Sequence_TextBox.Text = Row.Cells[4].Value.ToString();
                        Steps_TextBox.Text = Row.Cells[5].Value.ToString();
                        TotalStepsNeeded_TextBox.Text = Row.Cells[5].Value.ToString();
                        NumberOfSteps = Row.Cells[5].Value.ToString();
                        StepsUsed_TextBox.Text = Row.Cells[6].Value.ToString();
                        Scanned3D_TextBox.Text = Row.Cells[7].Value.ToString();
                        Ready3D_TextBox.Text = Row.Cells[8].Value.ToString();
                        //ItemRunCount_TextBox.Text = Row.Cells[9].Value.ToString();
                        PartsManufacturedTotal_String = Row.Cells[10].Value.ToString();
                        AveragePPM_String = Row.Cells[11].Value.ToString();
                        THEPPM_String = Row.Cells[11].Value.ToString();
                        Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                        ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                        Fixture_TextBox.Text = Row.Cells[15].Value.ToString();
                        FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                        Template_TextBox.Text = Row.Cells[17].Value.ToString();
                        TemplateLocation_TextBox.Text = Row.Cells[18].Value.ToString();

                        //UserProgramGridView.FirstDisplayedScrollingRowIndex = UserProgramGridView.SelectedRows[0].Index;
                        JobFound = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("The Following Item ID: " + SearchValue + " Was Not Found");
                //MessageBox.Show(ex.ToString());
            }
        }

        public void PassOperationValue(int SelectedOperation)
        {
            CurrentItemIDOperation = SelectedOperation;
        }

        private void FaultValue()
        {
            if (FaultReset_TextBox.Text == "True")
            {
                SetupModeMessage_TextBox.Location = new System.Drawing.Point(313, 434);
                SetupModeMessage_TextBox.Size = new System.Drawing.Size(1353, 322);
                SetupModeMessage_TextBox.Show();
                SetupModeMessage_TextBox.BringToFront();

                //Fault_Label.Show();
                //Fault_Label.BringToFront();
            }
            else if (FaultReset_TextBox.Text == "False")
            {
                SetupModeMessage_TextBox.Hide();

                //Fault_Label.Hide();
            }
        }

        private void ClearTextBoxes()
        {
            int i = 0;
            int j = 0;
            TextBox[] ItemDataTextBoxes = { ItemID_TextBox, Tooling_TextBox, ToolingLocation_TextBox, Template_TextBox, TemplateLocation_TextBox, Fixture_TextBox, FixtureLocation_TextBox };
            TextBox[] JobIDTextBoxes = { JobID_TextBox, Steps_TextBox, StepsUsed_TextBox };
            TextBox[] _3DScannerTextBoxes = { Scanned3D_TextBox, Ready3D_TextBox };
            TextBox[] JobDataTextBoxes = { PartsNeeded_TextBox, PartsFormed_TextBox, PartsRemaining_TextBox, JobStartTime_TextBox, TimeRemaining_TextBox, CurrentPPM_TextBox, LiveOEE_TextBox};
            Array[] UserProgramTextBoxes = { ItemDataTextBoxes, JobIDTextBoxes, _3DScannerTextBoxes, JobDataTextBoxes };
            for (j = 0; j < UserProgramTextBoxes.Length; j++)
            {
                foreach (TextBox CurrentGroupBox in UserProgramTextBoxes[i])
                {
                    CurrentGroupBox.Clear();
                }
                i++;
            }
        }
       
        public void PassValue(string PartsValue)
        {
            PartsNeeded_TextBox.Text = PartsValue;
        }

        public void PassReferenceNumber(string RefNumber)
        {
            ReferenceNumber_TextBox.Text = RefNumber;
        }

        private void ClearForm()
        {
            JobFound = false;
            ItemID_TextBox.Clear();
            Tooling_TextBox.Clear();
            ToolingLocation_TextBox.Clear();
            Fixture_TextBox.Clear();
            FixtureLocation_TextBox.Clear();
            Template_TextBox.Clear();
            TemplateLocation_TextBox.Clear();
            Scanned3D_TextBox.Clear();
            Ready3D_TextBox.Clear();
            JobID_TextBox.Clear();
            Steps_TextBox.Clear();
            StepsUsed_TextBox.Clear();
            CurrentItemID_TextBox.Clear();
            Sequence_TextBox.Clear();
            CurrentSteps_TextBox.Clear();
            TotalStepsNeeded_TextBox.Clear();
            HMI_Message_TextBox.Text = "";
            HMI_Message_TextBox.BackColor = System.Drawing.Color.LightGray;
        }

        private void SearchItemIDOperation()
        {
            string SearchValue = CurrentItemID;
            int OperationRows = 0;
            if (Customer_ComboBox.Text == "CAT")
            {
                Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[CAT_Item_Data] WHERE ItemID = '" + SearchValue + "'";
            }
            else if (Customer_ComboBox.Text == "John Deere")
            {
                Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[JohnDeere_Item_Data] WHERE ItemID = '" + SearchValue + "'";
            }
            else if (Customer_ComboBox.Text == "Paccar")
            {
                Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[Paccar_Item_Data] WHERE ItemID = '" + SearchValue + "'";
            }
            else if (Customer_ComboBox.Text == "Navistar")
            {
                Brake_Press_Operation_Selection = "SELECT COUNT(*) FROM [dbo].[Navistar_Item_Data] WHERE ItemID = '" + SearchValue + "'";
            }
            string OperationCount = Brake_Press_Operation_Selection;
            SqlConnection Count = new SqlConnection(SQL_Source);
            SqlCommand CountRows = new SqlCommand(OperationCount, Count);
            Count.Open();
            OperationRows = (int)CountRows.ExecuteScalar();
            Count.Close();

            if (OperationRows >= 2)
            {
                User_Program_Select_Operation OperationSelect = new User_Program_Select_Operation(this);
                if (OperationRows == 2)
                {
                    OperationSelect.Operation_1_Button.Location = new System.Drawing.Point(13, 16);
                    OperationSelect.Operation_2_Button.Location = new System.Drawing.Point(332, 16);
                    OperationSelect.Operation_3_Button.Hide();
                    OperationSelect.ClientSize = new System.Drawing.Size(625, 205);
                }
                else if (OperationRows == 3)
                {
                    OperationSelect.Operation_1_Button.Location = new System.Drawing.Point(13, 16);
                    OperationSelect.Operation_2_Button.Location = new System.Drawing.Point(332, 16);
                    OperationSelect.Operation_3_Button.Location = new System.Drawing.Point(651, 16);
                    OperationSelect.ClientSize = new System.Drawing.Size(940, 205);
                }
                if (OperationSelect.ShowDialog(this) == DialogResult.Yes)
                {
                    SearchForItemID();
                }
            }
            else
            {
                OperationSelect_OPC();
                CurrentItemIDOperation = 1;
                SearchForItemID();
            }
        }

        private void BrakePressID()
        {
            string BrakePressComputerID = System.Environment.MachineName;

            // CAT Brake Press
            if (BrakePressComputerID == "OHN7017") // Brake Press 1107
            {
                Customer_ComboBox.Text = "CAT";
                Customer_TextBox.Text = "CAT";
                BrakePress_ComboBox.Text = "1107";
                BrakePress_TextBox.Text = "1107";
            }
            else if (BrakePressComputerID == "BP1139") // Brake Press 1139
            {
                Customer_ComboBox.Text = "CAT";
                Customer_TextBox.Text = "CAT";
                BrakePress_ComboBox.Text = "1139";
                BrakePress_TextBox.Text = "1139";
            }
            else if (BrakePressComputerID == "BP1177") // Brake Press 1177
            {
                Customer_ComboBox.Text = "CAT";
                Customer_TextBox.Text = "CAT";
                BrakePress_ComboBox.Text = "1177";
                BrakePress_TextBox.Text = "1177";
            }            
            // John Deere Brake Press
            else if (BrakePressComputerID == "OHN7120") // Brake Press 1127
            {
                Customer_ComboBox.Text = "John Deere";
                Customer_TextBox.Text = "John Deere";
                BrakePress_ComboBox.Text = "1127";
                BrakePress_TextBox.Text = "1127";
                Step_Label.Visible = true;
                Of_Label.Visible = true;
                TotalStepsNeeded_TextBox.Visible = true;
                CurrentSteps_TextBox.Visible = true;
                ResetStepCount_Button.Visible = true;
            }
            else if (BrakePressComputerID == "OHN7011") // Brake Press 1178
            {
                Customer_ComboBox.Text = "John Deere";
                Customer_TextBox.Text = "John Deere";
                BrakePress_ComboBox.Text = "1178";
                BrakePress_TextBox.Text = "1178";
            }
            // Navistar Brake Press
            else if (BrakePressComputerID == "OHN7055") // Brake Press 1065
            {
                Customer_ComboBox.Text = "Navistar";
                Customer_TextBox.Text = "Navistar";
                BrakePress_ComboBox.Text = "1065";
                BrakePress_TextBox.Text = "1065";
            }
            else if (BrakePressComputerID == "OHN7052") // Brake Press 1108
            {
                Customer_ComboBox.Text = "Navistar";
                Customer_TextBox.Text = "Navistar";
                BrakePress_ComboBox.Text = "1108";
                BrakePress_TextBox.Text = "1108";
            }
            else if (BrakePressComputerID == "OHN7082") // Brake Press 1156
            {
                Customer_ComboBox.Text = "Navistar";
                Customer_TextBox.Text = "Navistar";
                BrakePress_ComboBox.Text = "1156";
                BrakePress_TextBox.Text = "1156";
            }
            else if (BrakePressComputerID == "OHN7148") // Brake Press 1720
            {
                Customer_ComboBox.Text = "Navistar";
                Customer_TextBox.Text = "Navistar";
                BrakePress_ComboBox.Text = "1720";
                BrakePress_TextBox.Text = "1720";
            }
            // Paccar Brake Press
            else if (BrakePressComputerID == "OHN7066") // Brake Press 1083
            {
                Customer_ComboBox.Text = "Paccar";
                Customer_TextBox.Text = "Paccar";
                BrakePress_ComboBox.Text = "1083";
                BrakePress_TextBox.Text = "1083";
            }
            else if (BrakePressComputerID == "OHN7121") // Brake Press 1155
            {
                Customer_ComboBox.Text = "Paccar";
                Customer_TextBox.Text = "Paccar";
                BrakePress_ComboBox.Text = "1155";
                BrakePress_TextBox.Text = "1155";
            }
            else if (BrakePressComputerID == "OHN7067") // Brake Press 1158
            {
                Customer_ComboBox.Text = "Paccar";
                Customer_TextBox.Text = "Paccar";
                BrakePress_ComboBox.Text = "1158";
                BrakePress_TextBox.Text = "1158";
            }
            else if (BrakePressComputerID == "OHN7122") // Brake Press 1175
            {
                Customer_ComboBox.Text = "Paccar";
                Customer_TextBox.Text = "Paccar";
                BrakePress_ComboBox.Text = "1175";
                BrakePress_TextBox.Text = "1175";
            }
            else if (BrakePressComputerID == "OHN7009") // Brake Press 1176
            {
                Customer_ComboBox.Text = "Paccar";
                Customer_TextBox.Text = "Paccar";
                BrakePress_ComboBox.Text = "1176";
                BrakePress_TextBox.Text = "1176";
            }
            // My Computer For Testing
            else if (BrakePressComputerID == "OHN7047NL")
            {
                Customer_ComboBox.Text = "Navistar";
                Customer_TextBox.Text = "Navistar";
                BrakePress_ComboBox.Text = "1720";
                BrakePress_TextBox.Text = "1720";
            }
            Customer_ComboBox.Enabled = false;
            BrakePress_ComboBox.Enabled = false;
        }
        
        private void CheckTime_Check()
        {
            SignedInHours = SignOutTime.Elapsed.TotalHours;
            if (SignedInHours >= 8)
            {
                SignOutTime.Reset();
                User_Program_Sign_Off SignOff = new User_Program_Sign_Off();
                UserInterface.Focus();
                UserInterface.Enabled = false;
                if (SignOff.ShowDialog(this) == DialogResult.Yes)
                {
                    LogOff_Button_Click(null, null);
                }
            }
        }           

        /*********************************************************************************************************************
        * 
        * Methods Region End
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
        * Timer Region Start
        * 
        * - Clock Tick
        * - Timer Tick
        * - Fault Timer Tick
        * - ServerConnect Timer Tick
        * - Schedule Timer Tick
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
            CheckTime = Time;
            Time += "   " + Date;
            Clock_TextBox.Text = Time;
            CheckTime_Check();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //RunningStatistics();
            //PartsRun();
            //CurrentSteps_TextBox.Text = StepCount;
            if (RunMode_OPCs.IsBusy != true)
            {
                RunMode_OPCs.RunWorkerAsync();
                //FaultValue();
            }
            //FaultValue();
            HMI_Check();
        }

        private void NotApplicable_Timer_Tick(object sender, EventArgs e)
        {
            //HMI_Check();
            NotApplicable_Check();           
        }

        private void Fault_Timer_Tick(object sender, EventArgs e)
        {
            //if (RunMode_OPCs.IsBusy != true)
            //{
            //    RunMode_OPCs.RunWorkerAsync();
                //FaultValue();
            //}
            //FaultValue();
            //HMI_Check();
        }

        private void ServerConnect_Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (OPCServer.IsConnected != true)
                {
                    ConnectToServer_OPC(null, null);
                }
            }
            catch (Exception exc)
            {
                exc.ToString();
            }

        }

        private void Schedule_Timer_Tick(object sender, EventArgs e)
        {
            SqlConnection ScheduleConnection = new SqlConnection(SQL_Source);
            SqlDataAdapter ScheduleDataAdapter = new SqlDataAdapter(BrakePressRefresh_SQL, ScheduleConnection);
            SqlCommandBuilder ScheduleCommandBuilder = new SqlCommandBuilder(ScheduleDataAdapter);
            DataSet ScheduleData = new DataSet();
            ScheduleDataAdapter.Fill(ScheduleData);
            ScheduleGridView.DataSource = ScheduleData.Tables[0];
        }

        /*********************************************************************************************************************
        * Timer Region End
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * TextBox Method Region Start
        * 
        * -- Total: 2
        * - ItemID TextBox Enter
        * - ItemID TextBox KeyDown
        * 
        *********************************************************************************************************************/
        #region

        private void ItemID_TextBox_Enter(object sender, EventArgs e)
        {
            if ((ItemID_TextBox.ReadOnly == true) && (ScanNewJob_Button.Enabled == true))
            {
                ScanNewJob_Button.Focus();
            }
            else if ((ItemID_TextBox.ReadOnly == true) && (ScanNewJob_Button.Enabled == false))
            {
                RunMode_Button.Focus();
            }
        }

        private void ItemID_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CurrentItemID = ItemID_TextBox.Text;
                OEEItemID = ItemID_TextBox.Text;
                if (CurrentItemID.Length == 9 || CurrentItemID.Length == 13)
                {
                    ItemIDWriteTo_OPC();
                    //WriteSteps_OPC();
                    SearchItemIDOperation();
                    //SearchForItemID();
                    //WriteSteps_OPC();
                    RunMode_Button.Enabled = true;
                    SetupMode_Button.Enabled = true;
                    ViewHitImage_Button.Enabled = true;
                    ViewSetupCard_Button.Enabled = true;
                    ViewPrint_Button.Enabled = true;
                    View3DPlacard_Button.Enabled = true;
                    CheckCardData_Button.Enabled = true;
                    ItemID_TextBox.ReadOnly = true;
                    ReportError_Button.Enabled = true;
                    CurrentItemID_TextBox.Text = CurrentItemID;
                    ItemRunCounter();
                    OperationIDCounter();
                    ItemOperationDataStart();
                    OperationDataStart();
                    if (JobFound == true)
                    {
                        CurrentItemID_TextBox.Text = CurrentItemID;
                        this.CurrentItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 85F);
                        this.CurrentItemID_TextBox.Size = new System.Drawing.Size(771, 129);
                        this.CurrentItemID_TextBox.Location = new System.Drawing.Point(570, 42);

                    }
                    PartProgrammed_OPC();
                    if (PartProgrammed_TextBox.Text == "False")
                    {
                        JobFound = false;
                    }
                    //else if (JobFound == false)
                    if (JobFound == false)
                    {
                        CurrentItemID = "999-99999";
                        ItemIDWriteTo_OPC();
                        //this.CurrentItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 75F);
                        //this.CurrentItemID_TextBox.Size = new System.Drawing.Size(1201, 114);
                        //this.CurrentItemID_TextBox.Location = new System.Drawing.Point(336, 37);
                        //CurrentItemID_TextBox.Text = "Job Needs Vision Program";
                        //CurrentItemID_TextBox.Text = CurrentItemID;
                        HMI_Message_TextBox.Text = "Jobs Needs Vision Program";
                    }
                    //NotApplicable_OPC();
                    //NotApplicable_Timer.Start();
                    /*
                    if (JobFound == true)
                    {
                        View3DPlacard_Button.Enabled = true;
                        ViewHitImage_Button.Enabled = true;
                        ViewPrint_Button.Enabled = true;
                        ViewSetupCard_Button.Enabled = true;
                        ItemID_TextBox.ReadOnly = true;
                        //BarcodeScanner();
                        UserInterface.Focus();
                        UserInterface.Enabled = false;
                        User_Program_Job_Data JobData = new User_Program_Job_Data(this);
                        
                        JobData.ItemID_TextBox.Text = ItemID_TextBox.Text;
                        JobData.Fixture_TextBox.Text = Fixture_TextBox.Text;
                        JobData.FixtureLocation_TextBox.Text = FixtureLocation_TextBox.Text;
                        JobData.Template_TextBox.Text = Template_TextBox.Text;
                        JobData.TemplateLocation_TextBox.Text = TemplateLocation_TextBox.Text;
                        JobData.PartsNeeded_TextBox.Focus();
                        /*
                        JobData.Show();
                        

                        if (JobData.ShowDialog(this) == DialogResult.Yes)
                        {
                            BarcodeScanner();
                            if (JobID_TextBox.Text == 0.ToString())
                            {
                                this.CurrentItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 45F);
                                CurrentItemID_TextBox.Text = "Job Needs Vision Program";
                            }
                            else if(JobID_TextBox.Text != 0.ToString())
                            {
                                this.CurrentItemID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 75F);
                                CurrentItemID_TextBox.Text = CurrentItemID;
                            }
                            SetupMode_Button.Focus();
                        }
                        else if(JobData.DialogResult == DialogResult.No)
                        {
                            ClearForm();
                        }
                    }      
                    */
                }
                else if (CurrentItemID.StartsWith("J"))
                {
                    MessageBox.Show("Please Scan Item ID. Do Not Scan Job Number.");
                }
                //SetupMode_Button_Click(null, null);
            }
        }

        private void PartProgrammed_OPC()
        {
            Opc.Da.Item[] OPC_PartProgrammed = new Opc.Da.Item[1];
            OPC_PartProgrammed[0] = new Opc.Da.Item();
            OPC_PartProgrammed[0].ItemName = Brake_Press_PartProgrammed + "Brake_Press_1720_Part_Programmed";
            OPC_PartProgrammedList.Add(OPC_PartProgrammed[0]);
            //OPC_PartProgrammed[1] = new Opc.Da.Item();
            //OPC_PartProgrammed[1].ItemName = Brake_Press_iNspect + "Vision_Part_ID_From_PLC";
            //OPC_PartProgrammedList.Add(OPC_PartProgrammed[1]);

            PartProgrammed_GroupRead.AddItems(OPC_PartProgrammedList.ToArray());
            Opc.IRequest req;
            PartProgrammed_GroupRead.Read(PartProgrammed_GroupRead.Items, 123, new Opc.Da.ReadCompleteEventHandler(ReadCompleteCallback_PartProgrammed), out req);

            //iNspect_Timer.Enabled = true;
        }

        private void ReadCompleteCallback_PartProgrammed(object clientHandle, Opc.Da.ItemValueResult[] results)
        {
            PartProgrammed_TextBox.Invoke(new EventHandler(delegate { PartProgrammed_TextBox.Text = (results[0].Value).ToString(); }));
            //iNspectPLC_TextBox.Invoke(new EventHandler(delegate { iNspectPLC_TextBox.Text = (results[1].Value).ToString(); }));
        }

        void PartProgrammed_DataChanged_OPC(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            foreach (ItemValueResult itemValue in values) // 1107
            {
                switch (itemValue.ItemName)
                {
                    case "OHN66OPC.BrakePress_ControlLogix.Prgm_Brake_1720.Brake_Press_1720_Part_Programmed":
                        Vision_PartProgrammed = Convert.ToString(itemValue.Value);
                        // iNspectCamera_TextBox.Text = iNspectVisionID_Camera;
                        break;
                }
            }
        }

        /*********************************************************************************************************************
        * TextBox Method Region End
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * TextBox Enter Region Start
        * 
        * -- Total TextBox: 28
        * 
        *********************************************************************************************************************/
        #region

        private void Tooling_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void ToolingLocation_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Template_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void TemplateLocation_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Fixture_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void FixtureLocation_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void JobID_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Steps_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void StepsUsed_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Scanned3D_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Ready3D_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void PartsNeeded_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void PartsFormed_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void PartsRemaining_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void JobStartTime_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void TimeRemaining_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void CurrentPPM_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void LiveOEE_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void LivePPM_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Clock_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void User_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void DMPID_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void CurrentItemID_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void HMI_NotActive_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void HMI_RunMode_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void HMI_Message_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void HMI_CheckPlacement_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Customer_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void BrakePress_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Status_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void SetupModeMessage_TextBox_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        /*********************************************************************************************************************
        * TextBox Enter Region End
        *********************************************************************************************************************/
        #endregion

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

        /*********************************************************************************************************************
        * 
        * Methods in Testing End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        *
        * Code Not Currently in Use Start
        * 
        *********************************************************************************************************************/
        #region

        private void RunningStatistics()
        {
            double HoursRemaining = 0;
            double MinutesRemaining = 0;
            double PartsNeeded = double.Parse(PartsNeeded_TextBox.Text);
            //string PPMString = CurrentPPM_TextBox.Text;
            string RemainingTime = "";
            CurrentParts = double.Parse(PartsFormed_TextBox.Text);
            LivePPMOEECalculation();
            //CurrentParts = 2;
            string PPMString = CurrentPPM_TextBox.Text;
            //if (PartsNeeded < CurrentParts)
            //{
            //    MessageBox.Show("Parts Needed Must be Greater than Parts Formed");
            //   SetupMode_Button_Click(null, null);

            //}
            if (PartsNeeded == CurrentParts)
            {
                SetupMode_Button_Click(null, null);
                PartsNeeded = PartsNeeded + 5;
                PartsNeeded_TextBox.Text = PartsNeeded.ToString();
            }
            else
            {
                PartsRemaining = PartsNeeded - CurrentParts;

                PartsRemaining_TextBox.Text = PartsRemaining.ToString();
            }

            double CurrentPPM = double.Parse(PPMString);
            double TimeRemaining = (PartsRemaining / CurrentPPM);

            if (TimeRemaining < 60)
            {
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 1)
                {
                    RemainingTime = MinutesRemaining + " Minute".ToString();
                }
                else
                {
                    RemainingTime = MinutesRemaining + " Minutes".ToString();
                }
            }
            else if (120 > TimeRemaining && TimeRemaining >= 60)
            {
                TimeRemaining = TimeRemaining - 60;
                HoursRemaining = 1;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hour " + MinutesRemaining + " Minutes".ToString();
            }
            else if (180 > TimeRemaining && TimeRemaining >= 120)
            {
                TimeRemaining = TimeRemaining - 120;
                HoursRemaining = 2;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (240 > TimeRemaining && TimeRemaining >= 180)
            {
                TimeRemaining = TimeRemaining - 180;
                HoursRemaining = 3;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (300 > TimeRemaining && TimeRemaining >= 240)
            {
                TimeRemaining = TimeRemaining - 240;
                HoursRemaining = 4;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (360 > TimeRemaining && TimeRemaining >= 300)
            {
                TimeRemaining = TimeRemaining - 300;
                HoursRemaining = 5;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (420 > TimeRemaining && TimeRemaining >= 360)
            {
                TimeRemaining = TimeRemaining - 360;
                HoursRemaining = 6;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (480 > TimeRemaining && TimeRemaining >= 420)
            {
                TimeRemaining = TimeRemaining - 420;
                HoursRemaining = 7;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (540 > TimeRemaining && TimeRemaining >= 480)
            {
                TimeRemaining = TimeRemaining - 480;
                HoursRemaining = 8;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (600 > TimeRemaining && TimeRemaining >= 540)
            {
                TimeRemaining = TimeRemaining - 540;
                HoursRemaining = 9;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            else if (660 > TimeRemaining && TimeRemaining >= 600)
            {
                TimeRemaining = TimeRemaining - 600;
                HoursRemaining = 10;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            TimeRemaining_TextBox.Text = RemainingTime;
            PartsRunProgressBar.Maximum = int.Parse(PartsNeeded_TextBox.Text);
            PartsRunProgressBar.Value = int.Parse(PartsFormed_TextBox.Text);
        }
        
        private void LivePPMOEECalculation()
        {
            /*
            * LivePPMCalculation
            * 
            * Current Parts Formed / TotalTime in Run Mode
            * 
            */
            THEPPM = double.Parse(THEPPM_String);
            double TotalElapsedTime = RunModeStopWatch.Elapsed.TotalMinutes;
            double CurrentRunPPM = (Math.Round((CurrentParts / TotalElapsedTime), 2));
            CurrentRunPPM = System.Math.Ceiling(CurrentRunPPM * 100) / 100;
            CurrentPPM_TextBox.Text = CurrentRunPPM.ToString("0.00");
            //LivePPM_TextBox.Text = LivePPM.ToString();
            double LiveOEE = Math.Round(CurrentRunPPM / THEPPM, 3);
            LiveOEE = LiveOEE * 100;
            LiveOEE_TextBox.Text = LiveOEE.ToString() + "%";
            /*
            try
            {
                double SUPERLIVEPPM = (Math.Round(((CurrentParts - LIVEPPM) / TotalElapsedTime), 2));
                JobEndTime_TextBox.Text = SUPERLIVEPPM.ToString();
            }
            catch
            {
                JobEndTime_TextBox.Text = "Cannot Be a Negative Number";
            }
            */
        }

        private void BarcodeScanner()
        {
            Customer_ComboBox.Enabled = false;
            BrakePress_ComboBox.Enabled = false;
            ScanNewJob_Button.Enabled = false;
            RunMode_Button.Enabled = true;
            SetupMode_Button.Enabled = true;
            CancelRun_Button.Enabled = true;
            CancelRun_Button.Visible = true;
            //JobEnd_Button.Enabled = true;
            //JobEnd_Button.Visible = true;
            //Disconnect_Button_Click(null, null);
            PartsRunProgressBar.Visible = true;
            //HMI_NotActive_TextBox.Visible = false;
            JobStartTime = Clock_TextBox.Text;
            ItemRunCounter();
            OperationIDCounter();
            ItemOperationDataStart();

            OperationDataStart();
            //Timer.Enabled = true;            
            string StartingTime = Clock_TextBox.Text;
            string ReplaceTime = DateTime.Today.ToShortDateString();
            SetupStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
            SetupMode_Button_Click(null, null);
        }


        private void ItemOperationCalculation()
        {
            /*********************************************************************************************************************
            *                                |                                       |                                           |
            *                                |                                       |                                           |   
            *            Parts Formed        |                                       |                  Parts Formed             |
            *     PPM = ----------------     |     Parts Formed = Run Time * PPM     |     Run Time = ----------------           |
            *              Run Time          |                                       |                      PPM                  |
            *                                |                                       |                                           |
            *                                |                                       |                                           |
            *________________________________|_______________________________________|___________________________________________|
            *                                |                                       |                                           |
            *                                |                                       |                                           |
            *        OverallFormedParts      |      Overall                          |                  OverallFormedParts       |   
            * PPM = --------------------     |       Parts   =  Run Time * PPM       |     Run Time = --------------------       | 
            *         OverallRunTime         |      Formed                           |                     AveragePPM            |
            *                                |                                       |                                           |
            *                                |                                       |                                           |
            *********************************************************************************************************************/

            string CurrentPartsFormed_String = PartsFormed_TextBox.Text;
            double CurrentPartsFormed_Double = double.Parse(CurrentPartsFormed_String);

            if (PartsManufacturedTotal_String != "")
            {
                PartsManufacturedTotal_Double = double.Parse(PartsManufacturedTotal_String);
            }

            TotalItemPartsManufactured = CurrentPartsFormed_Double + PartsManufacturedTotal_Double;

            //Current PPM
            string CurrentPPM_String = CurrentPPM_TextBox.Text;
            float CurrentPartsFormed_Float = (float)CurrentPartsFormed_Double;
            float CurrentPPM = float.Parse(CurrentPPM_String);
            float CurrentRunTime = CurrentPartsFormed_Float / CurrentPPM;

            //Past PPM

            if (AveragePPM_String == "")
            {
                AveragePPM = CurrentPPM;
            }
            else if (AveragePPM_String != "")
            {
                PastPPM = float.Parse(AveragePPM_String);
            }
            if (PastPPM == 0)
            {
                AveragePPM = CurrentPPM;
            }
            else if (PastPPM != 0)
            {
                float PartsManufacturedTotal_Float = (float)PartsManufacturedTotal_Double;
                float PreviousRunTime = PartsManufacturedTotal_Float / PastPPM;

                float OverallRunTime = PreviousRunTime + CurrentRunTime;
                float OverallFormedParts = PartsManufacturedTotal_Float + CurrentPartsFormed_Float;
                AveragePPM = OverallFormedParts / OverallRunTime;
                AveragePPM = (float)Math.Round(AveragePPM, 2);
            }
        }


        private void FindTotalRunTime()
        {
            TimeOfOperation = DateTime.Parse(JobEndTime).Subtract(DateTime.Parse(JobStartTime));
            LiveRunTime_TextBox.Text = TimeOfOperation.ToString();
            TimeSpan SetupMode = SetupModeStopWatch.Elapsed;
            double ItemSetupTime_Double = SetupMode.TotalMinutes;
            ItemSetupTime_Double = ItemSetupTime_Double / 60;
            ItemSetupTime = (float)ItemSetupTime_Double;
            ItemSetupTime = (float)Math.Round(ItemSetupTime, 5);
        }

        private void OperationOEECalculation()
        {
            /*********************************************************************************************************************
            *                                         OEE = Efficiency * Utilization * Quality                                   *   
            **********************************************************************************************************************                                         
            *                                       |                                         |                                  |     
            *                       Planned         |                                         |                                  |   
            *                   Operation Time      |                    Operation Time       |                Good Parts        |   
            *    Efficiency = ------------------    |    Utilization = -------------------    |    Quality = ---------------     |                 
            *                   Operation Time      |                    Availible Hours      |                  Total           |  
            *                                       |                                         |               Formed Parts       |   
            *                                       |                                         |                                  |                         
            *_______________________________________|_________________________________________|__________________________________|
            *                                       |                                         |                                  |
            *                                       |                                         |                                  |
            *                   Planned Minutes     |                   Operation Time        |                Good Parts        |
            * Efficiency = ------------------------ | Utilization = ------------------------  | Quality = ---------------------  |
            *                   Actual Minutes      |                   Availible Hours       |            Total Formed Parts    |      
            *                                       |                                         |                                  |
            *                                       |                                         |                                  |
            *********************************************************************************************************************/

            string PartsOnOrder = PartsNeeded_TextBox.Text;
            double DoublePartsOnOrder = double.Parse(PartsOnOrder);
            double RunMinutes = TimeOfOperation.TotalMinutes;
            double PlannedTime;
            if (PastPPM == 0)
            {
                PlannedTime = RunMinutes;
            }
            else
            {
                PlannedTime = (DoublePartsOnOrder / PastPPM);
            }
            try
            {
                double PlannedMinutes = PlannedTime / 60;
                double ActualMinutes = RunMinutes / 60;
                Efficiency = (float)(PlannedMinutes / ActualMinutes);
                Efficiency = Efficiency * 100;
                Efficiency = (float)Math.Round(Efficiency, 2);
                TimePlanned = (float)PlannedMinutes;
                TimePlanned = (float)Math.Round(TimePlanned, 5);
                TimeActual = (float)ActualMinutes;
                TimeActual = (float)Math.Round(TimeActual, 5);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to Calculate OEE Results");
            }
        }


        /*********************************************************************************************************************
        *
        * Code Not Currently in Use End
        * 
        *********************************************************************************************************************/
        #endregion


        /*********************************************************************************************************************
        *
        * Code No Longer Used Start
        * 
        *********************************************************************************************************************/
        #region
            
        void BPComputerConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PartsFormed_TextBox.Text = ValuesArray[1];
            CurrentPPM_TextBox.Text = ValuesArray[2];
            CurrentStep_TextBox.Text = ValuesArray[3];
            if(PartsFormed_TextBox.Text != "")
            {
                this.Status_TextBox.BackColor = RunModeColor;
            }
            else
            {
                this.Status_TextBox.BackColor = System.Drawing.Color.Red;
            }
        }


        private void ComputerConnection(object sender, EventArgs e)
        {        
            /*
            try
            {
                for (int i = 0; i < Values.Length; i++)
                {
                    Type myType = Type.GetTypeFromProgID("PressBrake.Comm", Brake_Press_Computer, true);
                    Object o = Activator.CreateInstance(myType);
                    PressBrakeComm pbClass = (PressBrakeComm)Marshal.CreateWrapperOfType(o, typeof(PressBrakeComm));
                    myType = Type.GetTypeFromProgID("PressBrake.Comm", Brake_Press_Computer, true);
                    o = Activator.CreateInstance(myType);
                    pbClass = (PressBrakeComm)Marshal.CreateWrapperOfType(o, typeof(PressBrakeComm));
                    Value_Box.Text = Values[i];
                    ValueResults = pbClass.GetValue(Convert.ToInt32(Value_Box.Text));
                    ValuesArray[i] = ValueResults;
                    Value_Box.Clear();

                    //PartsFormed_TextBox.Text = ValuesArray[1];
                    //CurrentPPM_TextBox.Text = ValuesArray[2];
                    //CurrentStep_TextBox.Text = ValuesArray[3];
                }

            }
            catch (Exception ee)
            {
                this.Status_TextBox.BackColor = System.Drawing.Color.Red;
            }
            */
        }


        private void Computer_Name_TextChanged(object sender, EventArgs e)
        {
            /*
            try
            {
                for (int i = 0; i < Values.Length; i++)
                {
                    //Type myType = Type.GetTypeFromProgID("PressBrake.Comm", Computer_Name.Text, true);
                    //Object o = Activator.CreateInstance(myType);
                    //PressBrake.PressBrakeCommClass pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
                    myType = Type.GetTypeFromProgID("PressBrake.Comm", Computer_Name.Text, true);
                    o = Activator.CreateInstance(myType);
                    pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
                    Value_Box.Text = Values[i];
                    ValueResults = pbClass.GetValue(Convert.ToInt32(Value_Box.Text));
                    ValuesArray[i] = ValueResults;
                    Value_Box.Clear();

                    PartsFormed_TextBox.Text = ValuesArray[1];
                    CurrentPPM_TextBox.Text = ValuesArray[2];
                    CurrentStep_TextBox.Text = ValuesArray[3];
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show("Unable to Connect to Brake Press");
                MessageBox.Show(ee.ToString());
            }
            */
        }

        /*********************************************************************************************************************
        *
        * No Longer Used Code Start
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        *
        * User Program (ControlLogix System) End
        * 
        *********************************************************************************************************************/

        private void User_Program_FormClosing(object sender, FormClosingEventArgs e)
        {
            ServerConnect_Timer.Enabled = false;
            this.RunMode_OPCs.CancelAsync();
            OPCServer.Disconnect();
            DMPBrakePressLogin.Current.Focus();
            DMPBrakePressLogin.Current.Enabled = true;
            DMPBrakePressLogin.Current.WindowState = FormWindowState.Maximized;
            //DMPBrakePressLogin.Current.ShowIcon = true;
            //this.RunMode_OPCs.CancelAsync();
        }

        private void iNspectCamera_TextBox_Click(object sender, EventArgs e)
        {
            iNspectExpressData_OPC();
           // iNspect_Timer.Enabled = true;
        }
    }
}
