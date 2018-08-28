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
/*
 * Program: DMP Brake Press Application
 * Form: User_Program
 * Created By: Ryan Garland
 * Last Updated on 5/4/17
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
    public partial class User_Program : Form
    {
        public User_Program()
        {
            InitializeComponent();
            syncContext = WindowsFormsSynchronizationContext.Current;
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
        * Barcode Scanner Variables 
        * 
        ********************************************************************************************************************/

        public SynchronizationContext syncContext = null;
        //public EthSystemDiscoverer ethSystemDiscoverer = null;
        //public ISystemConnector connector = null;
        //public DataManSystem DMSystem = null;
        public object currentResultInfoSyncLock = new object();
        public ResultInfo currentResultInfo = new ResultInfo(-1, null, null, null);
        public bool Closing = false;
        public bool autoConnect = false;
        public object listAddItemLock = new object();

        /********************************************************************************************************************
        * 
        * OPC Tag Variables 
        * 
        ********************************************************************************************************************/
        /*

        Opc.URL BP_Url = new Opc.URL("opcda://localhost/Matrikon.OPC.AllenBradleyPLCs.1");

        Opc.Da.Server Server = null;
        OpcCom.Factory Factory = new OpcCom.Factory();
        Opc.Da.Subscription GroupID;
        Opc.Da.SubscriptionState GroupState = new Opc.Da.SubscriptionState();
        Opc.Da.Item[] GroupItems = new Opc.Da.Item[3];
        */
        private string BPGroup = "";
        private string BPItem = "";

        /********************************************************************************************************************
        * 
        * Form Load Variables 
        * 
        ********************************************************************************************************************/

        private string JobStartTime = "";
        private string JobEndTime = "";
        private string LoginForm = "User Program";
        private string LoginTime = "";
        //private string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;";
        private string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;";


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
        private string CurrentItemID = "";

        // RunningStatistics();
        private static double CurrentParts;
        private double PartsRemaining;

        // ItemOperationCalculation();
        private static string AveragePPM_String = "";
        private static string PartsManufacturedTotal_String = "";
        private static double TotalItemPartsManufactured = 0;
        private static double PartsManufacturedTotal_Double = 0;
        private float AveragePPM = 0;

        // ViewPrint_Button_Click();
        private string PDFPrintPath = "";
        private string PDFImagePath = "";
        private string CurrentSetupCardID = "";
        private string PDFSetupPath = "";
        private string BrakePressNamePDFPath = "";
        private string ItemIDBrakePressPDFPath = "";
        private string ItemIDPrintPDFPath = "";
        private string BrakePressSetupPDFPath = @"M:\OH\OH Common\Engineering\Brake Press\Vision\Setup Cards for Vision Solutions\";
        private string ItemPrintPDFPath = @"M:\OH\OH Common\Engineering\Brake Press\Vision\Brake Press Prints\";
        private string VisionSolutionsPDFPath = @"M:\OH\OH Common\Engineering\Brake Press\Vision\Pictures for Vision Solutions\";

        private static float PastPPM;

        Color RunModeColor = ColorTranslator.FromHtml("#32EB00");
        Color SetupModeColor = Color.Silver;

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

        private int j = 0;

        private static string BarCodeReaderID = "";

        private static string THEPPM_String = "";
        private static double THEPPM;

        private static double LIVEPPM;
        private static string LIVEPPM_String = "";

        private static bool RunMode_Button_Clicked = false;
        private static bool SetupMode_Button_Clicked = false;
        
        /********************************************************************************************************************
        * 
        * Variables In Testing End
        * 
        *********************************************************************************************************************
        *********************************************************************************************************************
        * 
        * User_Program Start
        * 
        ********************************************************************************************************************/

        private void User_Program_Load(object sender, EventArgs e)
        {
            SqlConnection UserLogin = new SqlConnection(SQL_Source);
            SqlCommand Login = new SqlCommand();
            Login.CommandType = System.Data.CommandType.Text;
            Login.CommandText = "INSERT INTO [dbo].[LoginData] (EmployeeName,DMPID,LoginDateTime,LoginForm) VALUES (@EmployeeName,@DMPID,@LoginDateTime,@LoginForm)";
            Login.Connection = UserLogin;
            Login.Parameters.AddWithValue("@LoginDateTime", Clock_TextBox.Text);
            Login.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            Login.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            Login.Parameters.AddWithValue("@LoginForm", LoginForm.ToString());
            UserLogin.Open();
            Login.ExecuteNonQuery();
            UserLogin.Close();

            Clock.Enabled = true;
            LoginTime = Clock_TextBox.Text;
            Customer_ComboBox.Items.Add("CAT");
            Customer_ComboBox.Items.Add("John Deere");
            Customer_ComboBox.Items.Add("Navistar");
            Customer_ComboBox.Items.Add("Paccar");
            PartsNeeded_TextBox.Text = 200.ToString();

            // Barcode Scanner Initialization
            //ethSystemDiscoverer = new EthSystemDiscoverer();
            //ethSystemDiscoverer.SystemDiscovered += new EthSystemDiscoverer.SystemDiscoveredHandler(OnEthSystemDiscovered);
            //ethSystemDiscoverer.Discover();
            //RefreshBarcodeScanner();

            //Server = new Opc.Da.Server(Factory, null);
            //Server.Connect(BP_Url, new Opc.ConnectData(new System.Net.NetworkCredential()));

        }

        /********************************************************************************************************************
         *  
         *  [User Interface]
         *  
         *  Buttons
         *  
         *  ComboBoxes
         * 
         ********************************************************************************************************************
         ********************************************************************************************************************
         * [Buttons]
         * 
         * ---------------------------------------------------------[Scan]---------------------------------------------------
         * -- 
         * 
         * -------------------------------------------------------[Job End]--------------------------------------------------
         * --Global Variables:
         *   JobEndTime
         *   
         * --Methods:
         *   ItemOperationCalculation();
         *   ItemOperationDataEnd();
         *   OperationDataEnd();
         *   ProgramListUpdate();
         *   FindTotalRunTime();
         *
         * ------------------------------------------------------[Disconnect]------------------------------------------------
         * --Methods:
         *   CleanupConnection();
         *   
         * ------------------------------------------------------ [ViewPrint]------------------------------------------------
         * 
         * ------------------------------------------------------[ViewHitImage]----------------------------------------------
         * --Global Variables:
         *   CurrentItemID = CurrentItemID.Replace("-", "");
         *   ItemIDBrakePressPDFPath = CurrentItemID + BrakePressNamePDFPath;
         * 
         * --Method Variables:
         *  string CompletePDFPath = Path.Combine(VisionSolutionsPDFPath, ItemIDBrakePressPDFPath);
         *  string ViewBrakePressPDFFile = CompletePDFPath + ".pdf";
         *   
         * ---------------------------------------------------------[Cancel]-------------------------------------------------
         * --Global Variables:
         *   JobFound = false;
         * 
         * 
         * ---------------------------------------------------------[LogOff]-------------------------------------------------
         * --Methods:
         *   EmployeeLogOff();
         * 
         ********************************************************************************************************************/

        private void Scan_Button_Click(object sender, EventArgs e)
        {/*
            try
            {
                if (BarcodeListBox.SelectedIndex != -1 && BarcodeListBox.Items.Count >= BarcodeListBox.SelectedIndex)
                {
                    var System_Info = BarcodeListBox.Items[BarcodeListBox.SelectedIndex];
                    if (System_Info is EthSystemDiscoverer.SystemInfo)
                    {
                        EthSystemDiscoverer.SystemInfo eth_System_Info = System_Info as EthSystemDiscoverer.SystemInfo;
                        EthSystemConnector Connection_Eth = new EthSystemConnector(eth_System_Info.IPAddress);

                        connector = Connection_Eth;

                    }
                    else if (System_Info is SerSystemDiscoverer.SystemInfo)
                    {
                        SerSystemDiscoverer.SystemInfo ser_system_info = System_Info as SerSystemDiscoverer.SystemInfo;
                        SerSystemConnector conn = new SerSystemConnector(ser_system_info.PortName, ser_system_info.Baudrate);

                        connector = conn;
                    }
                }


                //connector.Logger = new GuiLogger(tbLog, LoggingEnabledCB.Checked, ref Closing);

                DMSystem = new DataManSystem(connector);
                DMSystem.DefaultTimeout = 5000;

                DMSystem.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
                DMSystem.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);

                //DMSystem.SystemWentOnline += new SystemWentOnlineHandler(OnSystemWentOnline);
                //DMSystem.SystemWentOffline += new SystemWentOfflineHandler(OnSystemWentOffline);

                DMSystem.ReadStringArrived += new ReadStringArrivedHandler(OnReadStringArrived);
                DMSystem.XmlResultArrived += new XmlResultArrivedHandler(OnXmlResultArrived);
                DMSystem.ImageArrived += new ImageArrivedHandler(OnImageArrived);
                DMSystem.ImageGraphicsArrived += new ImageGraphicsArrivedHandler(OnImageGraphicsArrived);
                //DMSystem.BinaryDataTransferProgress += new BinaryDataTransferProgressHandler(OnBinaryDataTransferProgress);

                ResultTypes requested_result_types = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;
                ResultCollector _results = new ResultCollector(DMSystem, requested_result_types);
                _results.ComplexResultCompleted += Results_ComplexResultCompleted;
                _results.SimpleResultDropped += Results_SimpleResultDropped;


                DMSystem.Connect();

                try
                {
                    DMSystem.SetResultTypes(requested_result_types);
                }
                catch
                { }

            }
            catch (Exception ex)
            {
                CleanupConnection();
                AddListItem("Failed to Connect: " + ex.ToString());
                Scan_Button.Enabled = true;
            }
            autoConnect = true;
            */
        }

        private void JobEnd_Button_Click(object sender, EventArgs e)
        {
            Scan_Button.Enabled = true;
            RunMode_Button.Enabled = false;
            SetupMode_Button.Enabled = false;
            JobEnd_Button.Enabled = false;
            RunMode_Button.BackColor = Color.Silver;
            SetupMode_Button.BackColor = Color.Silver;
            SetupModeStopWatch.Stop();
            RunMode_Button_Clicked = false;
            SetupMode_Button_Clicked = false;
            JobEndTime = Clock_TextBox.Text;
            JobEnd_Button.Visible = false;
            ItemOperationCalculation();
            FindTotalRunTime();
            OperationOEECalculation();
            OperationOEEData();
            ItemOperationDataEnd();
            OperationDataEnd();
            ProgramListUpdate();
            Timer.Enabled = false;
            PartsRunProgressBar.Visible = false;
            HMI_NotActive_TextBox.Visible = true;
            Connect_Button.Visible = true;
            RefreshSQL1176();
        }

        private void Disconnect_Button_Click(object sender, EventArgs e)
        {/*
            DMSystem.Disconnect();
            DMSystem.SystemConnected -= new SystemConnectedHandler(OnSystemConnected);
            DMSystem.SystemDisconnected -= new SystemDisconnectedHandler(OnSystemDisconnected);


            DMSystem.ReadStringArrived -= new ReadStringArrivedHandler(OnReadStringArrived);
            DMSystem.XmlResultArrived -= new XmlResultArrivedHandler(OnXmlResultArrived);
            DMSystem.ImageArrived -= new ImageArrivedHandler(OnImageArrived);
            DMSystem.ImageGraphicsArrived -= new ImageGraphicsArrivedHandler(OnImageGraphicsArrived);
            //DMSystem.BinaryDataTransferProgress -= new BinaryDataTransferProgressHandler(OnBinaryDataTransferProgress);

            CleanupConnection();
            */
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

        private void ViewHitImage_Button_Click(object sender, EventArgs e)
        {
            CurrentItemID = CurrentItemID.Replace("-", "");
            ItemIDBrakePressPDFPath = CurrentItemID + BrakePressNamePDFPath;
            string CompletePDFPath = Path.Combine(VisionSolutionsPDFPath, ItemIDBrakePressPDFPath);
            string ViewBrakePressPDFFile = CompletePDFPath + ".pdf";
            ViewPDF PDFViewer = new ViewPDF();
            PDFViewer.AcroPDF.src = ViewBrakePressPDFFile;
            PDFViewer.AcroPDF.BringToFront();
            PDFViewer.Show();
            PDFViewer.BringToFront();
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

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            JobFound = false;
            ItemID_ComboBox.Text = "";
        }

        private void LogOff_Button_Click(object sender, EventArgs e)
        {
            EmployeeLogOff();
            DMPBrakePressLogin.Current.Focus();
            DMPBrakePressLogin.Current.Enabled = true;
            DMPBrakePressLogin.Current.WindowState = FormWindowState.Normal;
            this.Close();
        }

        /*********************************************************************************************************************
        * 
        * Buttons End
        * 
        *********************************************************************************************************************/
        /*********************************************************************************************************************
        * 
        * ComboBoxes Start
        * 
        *********************************************************************************************************************/

        private void Customer_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrakePress_ComboBox.Items.Clear();
            BrakePress_ComboBox.Text = "";
            listBox1.Items.Clear();
            
            {
                if(Customer_ComboBox.Text == "CAT")
                {   // 4
                    BrakePress_ComboBox.Items.Add("1107");
                    BrakePress_ComboBox.Items.Add("1139");
                }
                else if (Customer_ComboBox.Text == "John Deere")
                {   // 2
                    BrakePress_ComboBox.Items.Add("1127");
                    BrakePress_ComboBox.Items.Add("1178");
                }
                else if (Customer_ComboBox.Text == "Navistar")
                {   // 4
                    BrakePress_ComboBox.Items.Add("1065"); // 1065
                    BrakePress_ComboBox.Items.Add("1108"); // 1108
                    //BrakePress_ComboBox.Items.Add("1156"); // 1156
                    //BrakePress_ComboBox.Items.Add("1720"); // 1720
                }
                else if (Customer_ComboBox.Text == "Paccar")
                {   //5
                    /*
                    BrakePress_ComboBox.Items.Add("1083"); // 1083
                    BrakePress_ComboBox.Items.Add("1155"); // 1155
                    BrakePress_ComboBox.Items.Add("1158"); // 1158
                    BrakePress_ComboBox.Items.Add("1175"); // 1175
                    BrakePress_ComboBox.Items.Add("1176"); // 1176
                    */
                    BrakePress_ComboBox.Items.Add("1083"); // 1083
                    BrakePress_ComboBox.Items.Add("1155"); // 1155
                    BrakePress_ComboBox.Items.Add("1175"); // 1175                    
                    BrakePress_ComboBox.Items.Add("1176"); // 1176
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
                }
            }
        }

        private void BrakePress_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // CAT Cell

            if (BrakePress_ComboBox.Text == "1107")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1107";
                Computer_Name.Text = "PB50093";                                

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1107 = "SELECT * FROM [dbo].[BP_1107_Schedule]";
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
                Computer_Name.Text = "PB51294";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1139 = "SELECT * FROM [dbo].[BP_1139_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1139, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }

            // John Deere Cell

            else if (BrakePress_ComboBox.Text == "1178")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1178";
                Computer_Name.Text = "PB55569";
                // Select BarCode Reader on Current Brake Press
                BarcodeListBox.Text = "1178_Bar_Code_Reader";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1178 = "SELECT * FROM [dbo].[BP_1178_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1178, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }

            // Navistar Cell

            else if (BrakePress_ComboBox.Text == "1065")
            {
                // Connect to SQL DataTable and Load
                //Computer_Name.Text = "PB846662";
                BrakePressNamePDFPath = "_1065";
                // Select BarCode Reader on Current Brake Press
                BarcodeListBox.Text = "1065_Bar_Code_Reader";
                
                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1065 = "SELECT * FROM [dbo].[BP_1065_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1065, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];                
            }
            else if (BrakePress_ComboBox.Text == "1108")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1108";
                Computer_Name.Text = "PB50208";
                // Select BarCode Reader on Current Brake Press
                BarcodeListBox.Text = "1108_Bar_Code_Reader";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1108 = "SELECT * FROM [dbo].[BP_1108_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1108, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1156")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1156";
                Computer_Name.Text = "PB54539";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1156 = "SELECT * FROM [dbo].[BP_1156_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1156, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1720")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1720";
                Computer_Name.Text = "PB51581";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1720 = "SELECT * FROM [dbo].[BP_1720_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1720, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }

            // Paccar Cell

            else if (BrakePress_ComboBox.Text == "1083")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1083";
                Computer_Name.Text = "PB48909";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1083 = "SELECT * FROM [dbo].[BP_1083_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1083, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1155")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1155";
                Computer_Name.Text = "PB50093";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1155 = "SELECT * FROM [dbo].[BP_1155_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1155, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1158")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1158";
                Computer_Name.Text = "pb846574";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1158 = "SELECT * FROM [dbo].[BP_1158_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1158, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1175")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1175";
                Computer_Name.Text = "CI846574";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1175 = "SELECT * FROM [dbo].[BP_1175_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1175, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1176")
            {
                // Connect to SQL DataTable and Load
                BrakePressNamePDFPath = "_1176";
                Computer_Name.Text = "PB53973";
                // Select BarCode Reader on Current Brake Press
                BarcodeListBox.Text = "1176_Bar_Code_Reader";

                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1176 = "SELECT * FROM [dbo].[BP_1176_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];

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
            
        }

        private void ItemID_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SearchValue = ItemID_ComboBox.Text;
            CurrentItemID = ItemID_ComboBox.Text;
            CurrentSetupCardID = ItemID_ComboBox.Text;
            CurrentItemID_TextBox.Text = ItemID_ComboBox.Text;
            UserProgramGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                foreach (DataGridViewRow row in UserProgramGridView.Rows)
                {
                    row.Selected = false;
                    if (row.Cells[0].Value.ToString().Equals(SearchValue))
                    {
                        row.Selected = true;
                        ItemID_TextBox.Text = row.Cells[0].Value.ToString();
                        Steps_TextBox.Text = row.Cells[3].Value.ToString();
                        JobID_TextBox.Text = row.Cells[4].Value.ToString();
                        StepsUsed_TextBox.Text = row.Cells[5].Value.ToString();
                        //ItemRunCount_TextBox.Text = row.Cells[9].Value.ToString();
                        PartsManufacturedTotal_String = row.Cells[10].Value.ToString();
                        AveragePPM_String = row.Cells[11].Value.ToString();
                        THEPPM_String = row.Cells[12].Value.ToString();
                        UserProgramGridView.FirstDisplayedScrollingRowIndex = UserProgramGridView.SelectedRows[0].Index;
                        JobFound = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Computer_Name_TextChanged(object sender, EventArgs e)
        {/*
            try
            {
                for (int i = 0; i < Values.Length; i++)
                {
                    Type myType = Type.GetTypeFromProgID("PressBrake.Comm", Computer_Name.Text, true);
                    Object o = Activator.CreateInstance(myType);
                    PressBrake.PressBrakeCommClass pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
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


        /********************************************************************************************************************
        * 
        * ComboBoxes End
        * 
        ********************************************************************************************************************/
        /********************************************************************************************************************
        *  
        * [Barcode Scanner]
        * 
        * Classes
        * 
        * Methods
        *  
        * 
        ********************************************************************************************************************/
        /*********************************************************************************************************************
        * (Methods)
        * 
        * ----------------------------------------------(OnEthSystemDiscovered)-----------------------------------------------
        * 
        * ---------------------------------------------------(AddListItem)----------------------------------------------------
        * 
        * -------------------------------------------------(OnSystemConnected)------------------------------------------------
        * 
        * ------------------------------------------------(OnSystemDisconnected)----------------------------------------------
        * --Global Variables:
        *   JobFound - private static bool 
        * 
        * -------------------------------------------------(OnReadStringArrived)----------------------------------------------
        * 
        * -------------------------------------------------(OnXmlResultArrived)-----------------------------------------------
        * 
        * ---------------------------------------------------(OnImageArrived)-------------------------------------------------
        * 
        * ------------------------------------------------(OnImageGraphicsArrived)--------------------------------------------
        * 
        * ----------------------------------------------------(ShowResult)----------------------------------------------------
        * --Global Variables:
        *   JobStartTime = Clock_TextBox.Text;
        *   
        * --Variables:
        *   string StartingTime = Clock_TextBox.Text;
        *   string ReplaceTime = DateTime.Today.ToShortDateString();
        *   JobStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
        *     
        * --Methods:
        *   Disconnect_Button_Click(null, null);
        *   ItemRunCounter();
        *   OperationIDCounter();
        *   ItemOperationDataStart();
        *   OperationDataStart();
        * 
        *  
        * 
        * ---------------------------------------------(GetReadStringFromResultXml)-------------------------------------------
        * 
        * --------------------------------------------(Results_ComplexResultCompleted)----------------------------------------
        * 
        * ---------------------------------------------(Results_SimpleResultDropped)------------------------------------------
        * 
        * ------------------------------------------------(ReportDroppedResult)-----------------------------------------------
        * 
        * -------------------------------------------------(CleanupConnection)------------------------------------------------
        * 
        ********************************************************************************************************************/

        public class ResultInfo
        {
            public ResultInfo(int resultId, Image image, string imageGraphics, string readString)
            {
                ResultId = resultId;
                Image = image;
                ImageGraphics = imageGraphics;
                ReadString = readString;
            }
            public int ResultId { get; set; }
            public Image Image { get; set; }
            public string ImageGraphics { get; set; }
            public string ReadString { get; set; }

        }

        private void AddListItem(object item)
        {
            lock (listAddItemLock)
            {
                StatusListBox.Items.Add(item);

                if (StatusListBox.Items.Count > 500)
                    StatusListBox.Items.RemoveAt(0);

                if (StatusListBox.Items.Count > 0)
                    StatusListBox.SelectedIndex = StatusListBox.Items.Count - 1;
            }
        }

        private void RefreshBarcodeScanner()
        {
            try
            {
                BarcodeListBox.Items.Clear();
                //ethSystemDiscoverer.Discover();
            }
            catch
            {
                MessageBox.Show("Unable to Refresh Scanners");
            }

        }
    
        private void OnSystemConnected(object sender, EventArgs args)
        {
            syncContext.Post(
                delegate
                {

                    //ItemID_Label.Visible = false;
                    ItemID_ComboBox.Visible = false;
                    //DeviceIP_TextBox.Visible = true;
                    StatusListBox.Visible = true;
                    Cancel_Button.Visible = true;
                    Barcode_PictureBox.Visible = true;
                    //Scan_Button.Visible = false;
                    Trigger_Button.Visible = true;
                    Disconnect_Button.Visible = true;
                    ItemID_TextBox.Visible = true;
                    Scan_Button.Enabled = false;
                    Trigger_Button.Enabled = true;
                    Disconnect_Button.Enabled = true;
                    StatusListBox.Visible = true;
                    AddListItem("System Connected");

                },
                null);
        }

        private void OnSystemDisconnected(object sender, EventArgs args)
        {
            syncContext.Post(
                delegate
                {
                    AddListItem("System Disconnected");
                    Item_Label.Visible = true;
                    ItemID_ComboBox.Visible = true;
                    ItemID_TextBox.Visible = false;
                    Cancel_Button.Visible = false;
                    Disconnect_Button.Visible = false;
                    Trigger_Button.Visible = false;
                    Barcode_PictureBox.Visible = false;
                    ItemID_ComboBox.Enabled = true;
                    Scan_Button.Enabled = true;
                    Disconnect_Button.Enabled = false;
                    Trigger_Button.Enabled = false;
                    JobFound = false;
                    StatusListBox.Visible = false;
                },
                null);
        }

        

        /********************************************************************************************************************
        *  
        *  Barcode Scanner End
        * 
        ********************************************************************************************************************/

        /********************************************************************************************************************
        *  
        *  Methods
        *
        *  ---------------------------------------------------(Clock Tick)---------------------------------------------------
        *  --Global Variables:
        *    ClockHour = DateTime.Now.Hour;
        *    ClockMinute = DateTime.Now.Minute;
        *    ClockSecond = DateTime.Now.Second;
        *  
        *  --Method Variables:
        *    string AMPM = "";
        *    string Date = DateTime.Today.ToShortDateString();
        *    string Time = "";
        *    
        *  ---------------------------------------------------(EmployeeLogOff)-----------------------------------------------
        *  --Method SQL Data:
        *    ("@LoginDateTime", LoginTime.ToString());
        *    ("@LogoutDateTime", Clock_TextBox.Text);
        *    
        *  ---------------------------------------------------(FindTotalRunTime)---------------------------------------------
        *  --GlobalVariables:
        *    JobEndTime
        *    JobStartTime
        *    TimeOfOperation = DateTime.Parse(JobEndTime).Subtract(DateTime.Parse(JobStartTime));
        *    
        *  ---------------------------------------------------(ItemOperationCalculation)-------------------------------------
        *  --Global Variables:
        *    AveragePPM = CurrentPPM;
        *    or
        *    AveragePPM = CurrentPPM;
        *    AveragePPM = OverallFormedParts / OverallRunTime;
        *    AveragePPM = (float)Math.Round(AveragePPM, 2);
        *   
        *  --Method Variables:
        *    string CurrentPartsFormed_String = PartsFormed_TextBox.Text;
        *    double CurrentPartsFormed_Double = double.Parse(CurrentPartsFormed_String);
        *    or
        *    PartsManufacturedTotal_Double = double.Parse(PartsManufacturedTotal_String);
        *    TotalItemPartsManufactured = CurrentPartsFormed_Double + PartsManufacturedTotal_Double;
        *    string CurrentPPM_String = CurrentPPM_TextBox.Text;
        *    float CurrentPartsFormed_Float = (float)CurrentPartsFormed_Double;
        *    float CurrentPPM = float.Parse(CurrentPPM_String);
        *    float CurrentRunTime = CurrentPartsFormed_Float / CurrentPPM;
        *    string AveragePPM_String = AveragePPM_TextBox.Text;
        *    PastPPM = float.Parse(AveragePPM_String);
        *    float PartsManufacturedTotal_Float = (float)PartsManufacturedTotal_Double;
        *    float PreviousRunTime = PartsManufacturedTotal_Float / PastPPM;
        *    float OverallRunTime = PreviousRunTime + CurrentRunTime;
        *    float OverallFormedParts = PartsManufacturedTotal_Float + CurrentPartsFormed_Float;
        *    
        *  ---------------------------------------------------(ItemOperationDataStart)---------------------------------------
        *  --Method SQL Data:
        *    ("@ItemID", CurrentItemID);
        *    ("@OperationID", OperationsID.ToString());
        *    ("@ItemRunCount", ItemRunCount.ToString());
        *    ("@StartDateTime", Clock_TextBox.Text);
        *    ("@EmployeeName", User_TextBox.Text);
        *    ("@DMPID", DMPID_TextBox.Text);
        *    ("@BrakePress", BrakePress_ComboBox.Text);
        *  
        *  ---------------------------------------------------(ItemOperationDataEnd)-----------------------------------------
        *  --Method SQL Data:
        *    ("@OperationID", OperationsID);
        *    ("@EndDateTime", Clock_TextBox.Text);
        *    ("@PartsManufactured", PartsFormed_TextBox.Text);
        *    ("@PartsPerMinute", CurrentPPM_TextBox.Text);
        *  
        *  ---------------------------------------------------(ItemRunCounter)-----------------------------------------------
        *  --Global Variables:
        *    ItemRunCount = OperationRunCount + 1;
        *  
        *  --Method Variables:
        *    int OperationRunCount = (int)CountItemRun.ExecuteScalar();
        *    
        *  --Method SQL Data:  
        *    
        *  
        *  ---------------------------------------------------(LoadImage)----------------------------------------------------
        *  --Global Variables:
        *  
        *  
        *  --Method Variables:
        *    PictureBox ImageBox = Part_PictureBox;
        *    int i = 0;
        *    var DataImage = (Byte[])(ImageData.Tables[0].Rows[0][i]);
        *    var ImageStream = new MemoryStream(DataImage);
        *    
        *  ---------------------------------------------------(OperationIDCounter)-------------------------------------------
        *  --Global Variables:
        *    OperationsID = OperationCountID + 1;
        *    
        *  --Method Variables:
        *    int OperationCountID = (int)countOperation.ExecuteScalar();
        *    
        *  ---------------------------------------------------(OperationDataStart)-------------------------------------------
        *  --Global Variables:
        *    JobStartTime;
        *    
        *  --Method SQL Data:
        *    ("@ItemID", CurrentItemID);
        *    ("@OperationID", OperationsID.ToString());
        *    ("@RunDateTime", Clock_TextBox.Text);
        *    ("@EmployeeName", User_TextBox.Text);
        *    ("@DMPID", DMPID_TextBox.Text);
        *    ("@BrakePress", BrakePress_ComboBox.Text);
        *    
        *  ---------------------------------------------------(OperationDataEnd)---------------------------------------------
        *  --Global Variables:
        *  
        *  
        *  --Method Variables:
        *  
        *  --Method SQL Data:
        *    ("@OperationID", OperationsID.ToString());
        *    ("@PartsManufactured", PartsFormed_TextBox.Text);
        *    ("@PartsPerMinute", CurrentPPM_TextBox.Text);
        *    
        *  ---------------------------------------------------(ProgramListUpdate)--------------------------------------------
        *   --Global Variables:
        *  
        *  
        *  --Method Variables:
        *  
        *  --Method SQL Data:
        *    ("@ItemID", CurrentItemID);
        *    ("@PartsManufactured", PartsManufacturedTotal_String);
        *    ("@PartsPerMinute", PPMAverage.ToString());
        *    ("@TotalRuns", ItemRunCount);
        *    
        *  ---------------------------------------------------(RemoveSolution)-----------------------------------------------
        *  --Global Variables:
        *  
        *  
        *  --Method Variables:
        *    string ItemID = BP_Array[0];
        *    
        *  ---------------------------------------------------(RemoveSolution)-----------------------------------------------
        *      
        *  ---------------------------------------------------(RunningStatistics)--------------------------------------------
        *  --Global Variables:
        *    CurrentParts = int.Parse(PartsFormed_TextBox.Text);
        *    PartsRemaining = PartsNeeded - CurrentParts;
        *    
        *  --Method Variables:
        *    double HoursRemaining = 0;
        *    double MinutesRemaining = 0;
        *    string RemainingTime = "";
        *    int PartsNeeded = int.Parse(PartsNeeded_TextBox.Text);
        *    
        *  ---------------------------------------------------(RunMode)------------------------------------------------------
        *  --Global Variables:
        *  
        *  
        *  --Method Variables:
        *  ---------------------------------------------------(SearchForItemID)-----------------------------------------------
        *  --Global Variables:
        *    JobFound = true;
        *  
        *  --Method Variables:
        *    string SearchValue = CurrentItemID;
        *    
        *  --Methods:
        *    
        *    
        *  ---------------------------------------------------(Timer_Tick)---------------------------------------------------
        *  --Methods:
        *    RunMode();
        *    RunningStatistics();
        * 
        ********************************************************************************************************************/

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

        private void ItemOperationCalculation()
        {
        /*********************************************************************************************************************
        *                                |                                       |
        *                                |                                       |
        *            Parts Formed        |                                       |                  Parts Formed
        *     PPM = ----------------     |     Parts Formed = Run Time * PPM     |     Run Time = ----------------
        *              Run Time          |                                       |                      PPM
        *                                |                                       |
        *                                |                                       |
        *________________________________|_______________________________________|__________________________________________
        *                                |                                       |
        *                                |                                       |
        *        OverallFormedParts      |      Overall                          |                  OverallFormedParts
        * PPM = --------------------     |       Parts   =  Run Time * PPM       |     Run Time = --------------------
        *         OverallRunTime         |      Formed                           |                     AveragePPM
        *                                |                                       |
        *                                |                                       |
        *********************************************************************************************************************/

            string CurrentPartsFormed_String = PartsFormed_TextBox.Text;
            double CurrentPartsFormed_Double = double.Parse(CurrentPartsFormed_String);

            if (PartsManufacturedTotal_String != "")
            {
                PartsManufacturedTotal_Double = double.Parse(PartsManufacturedTotal_String);
            }

            TotalItemPartsManufactured = CurrentPartsFormed_Double + PartsManufacturedTotal_Double;

            //Current PPM
            string CurrentPPM_String = LivePPM_TextBox.Text;
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

        private void ItemOperationDataStart()
        {
            SqlConnection OperationStart = new SqlConnection(SQL_Source);
            SqlCommand StartOperation = new SqlCommand();
            StartOperation.CommandType = System.Data.CommandType.Text;
            StartOperation.CommandText = "INSERT INTO [dbo].[ItemOperationData] (ItemID, OperationID, ItemRunCount, StartDateTime, EmployeeName, DMPID, BrakePress) VALUES (@ItemID,@OperationID,@ItemRunCount,@StartDateTime,@EmployeeName,@DMPID,@BrakePress)";
            StartOperation.Connection = OperationStart;
            StartOperation.Parameters.AddWithValue("@ItemID", CurrentItemID);
            StartOperation.Parameters.AddWithValue("@OperationID", OperationsID.ToString());
            StartOperation.Parameters.AddWithValue("@ItemRunCount", ItemRunCount.ToString());
            StartOperation.Parameters.AddWithValue("@StartDateTime", Clock_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@EmployeeName", User_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@DMPID", DMPID_TextBox.Text);
            StartOperation.Parameters.AddWithValue("@BrakePress", BrakePress_ComboBox.Text);
            OperationStart.Open();
            StartOperation.ExecuteNonQuery();
            OperationStart.Close();            
        }

        private void ItemOperationDataEnd()
        {
            SqlConnection OperationEnd = new SqlConnection(SQL_Source);
            SqlCommand EndOperation = new SqlCommand();
            EndOperation.CommandType = System.Data.CommandType.Text;
            EndOperation.CommandText = "UPDATE [dbo].[ItemOperationData] SET EndDateTime=@EndDateTime,PartsManufactured=@PartsManufactured, PartsPerMinute=@PartsPerMinute WHERE OperationID=@OperationID";
            EndOperation.Connection = OperationEnd;
            EndOperation.Parameters.AddWithValue("@OperationID", OperationsID);
            EndOperation.Parameters.AddWithValue("@EndDateTime", Clock_TextBox.Text);
            EndOperation.Parameters.AddWithValue("@PartsManufactured", PartsFormed_TextBox.Text);
            EndOperation.Parameters.AddWithValue("@PartsPerMinute", LivePPM_TextBox.Text);
            OperationEnd.Open();
            EndOperation.ExecuteNonQuery();
            OperationEnd.Close();

        }

        private void ItemRunCounter()
        {
            try
            {
                string CountOperations = "SELECT COUNT(ItemID) FROM [dbo].[ItemOperationData] WHERE ItemID='" + CurrentItemID + "'";
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
                MessageBox.Show(e.ToString());
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
            StartOperation.CommandText = "INSERT INTO [dbo].[OperationData] (ItemID, OperationID, RunDateTime, EmployeeName, DMPID, BrakePress) VALUES (@ItemID,@OperationID,@RunDateTime,@EmployeeName,@DMPID,@BrakePress)";
            StartOperation.Connection = OperationStart;
            StartOperation.Parameters.AddWithValue("@ItemID", CurrentItemID);
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
            EndOperation.Parameters.AddWithValue("@PartsPerMinute", LivePPM_TextBox.Text);
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
            OEEData.CommandText = "INSERT INTO [dbo].[OperationOEE] (ItemID, OperationID, RunDateTime, OperationTime, PlannedTime, Efficiency, EmployeeName, DMPID, BrakePress) VALUES (@ItemID,@OperationID,@RunDateTime,@OperationTime,@PlannedTime,@Efficiency,@EmployeeName,@DMPID,@BrakePress)";
            OEEData.Connection = OperationOEEReport;
            OEEData.Parameters.AddWithValue("@ItemID", CurrentItemID);
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

        /*********************************************************************************************************************
         *                                         OEE = Efficiency * Utilization * Quality                                  *   
         *********************************************************************************************************************                                         
         *                                       |                                         |                                 |     
         *                       Planned         |                                         |                                 |   
         *                   Operation Time      |                    Operation Time       |                Good Parts       |   
         *    Efficiency = ------------------    |    Utilization = -------------------    |    Quality = ---------------    |                 
         *                   Operation Time      |                    Availible Hours      |                  Total          |  
         *                                       |                                         |               Formed Parts      |  
         *                                       |                                         |                                 |                                        |                                         |                                   
         *_______________________________________|_________________________________________|_________________________________|
         *                                       |                                         |                                 |
         *                                       |                                         |                                 |
         *                   Planned Minutes     |                   Operation Time        |                Good Parts       |
         * Efficiency = ------------------------ | Utilization = ------------------------  | Quality = --------------------- |
         *                   Actual Minutes      |                   Availible Hours       |            Total Formed Parts   |     
         *                                       |                                         |                                 |
         *                                       |                                         |                                 |
         *********************************************************************************************************************/


        private void OperationOEECalculation()
        {
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

        private void ProgramListUpdate()
        {
            SqlConnection ProgramUpdate = new SqlConnection(SQL_Source);
            SqlCommand UpdateItem = new SqlCommand();
            UpdateItem.CommandType = System.Data.CommandType.Text;
            UpdateItem.CommandText = "UPDATE [dbo].[Paccar_Item_Data] SET PartsManufactured=@PartsManufactured,PartsPerMinute=@PartsPerMinute,TotalRuns=@TotalRuns WHERE ItemID=@ItemID";
            UpdateItem.Connection = ProgramUpdate;
            UpdateItem.Parameters.AddWithValue("@ItemID", CurrentItemID);
            UpdateItem.Parameters.AddWithValue("@TotalRuns", ItemRunCount);
            UpdateItem.Parameters.AddWithValue("@PartsManufactured", TotalItemPartsManufactured.ToString());
            UpdateItem.Parameters.AddWithValue("@PartsPerMinute", AveragePPM.ToString());
            ProgramUpdate.Open();
            UpdateItem.ExecuteNonQuery();
            ProgramUpdate.Close();
        }

        private void RefreshSQL1176()
        {
            SqlConnection connection = new SqlConnection(SQL_Source);
            string BP1176 = "SELECT * FROM [dbo].[Paccar_Item_Data]";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            DataSet Data = new DataSet();
            dataAdapter.Fill(Data);
            UserProgramGridView.DataSource = Data.Tables[0];

            //ItemID_ComboBox.SelectedItem = null;
        }

        public void RemoveSolution()
        {
            string ItemID = ValuesArray[0];

            ItemID = ItemID.Replace("1st", "");
            ItemID = ItemID.Replace("1ST", "");
            ItemID = ItemID.Replace("2nd", "");
            ItemID = ItemID.Replace("3ND", "");
            ItemID = ItemID.Replace("3rd", "");
            ItemID = ItemID.Replace("3RD", "");
            // ItemID = ItemID.Replace("200-", "");
            //  ItemID = ItemID.Replace("210-", "");
            //  ItemID = ItemID.Replace("220-", "");

            // Remove Spaces and Symbols 
            ItemID = ItemID.Replace(" ", "");
            ItemID = ItemID.Replace("(", "");
            ItemID = ItemID.Replace(")", "");
            ItemID = ItemID.Replace(".", "");
            ItemID = ItemID.Replace(",", "");
            //ItemID = ItemID.Replace("-", "");

            // Remove Company
            ItemID = ItemID.Replace("AMG", "");
            ItemID = ItemID.Replace("amg", "");
            ItemID = ItemID.Replace("ash", "");
            ItemID = ItemID.Replace("ASH", "");
            ItemID = ItemID.Replace(" ", "");
            ItemID = ItemID.Replace("Cat", "");
            ItemID = ItemID.Replace("CAT", "");
            ItemID = ItemID.Replace("JD", "");
            ItemID = ItemID.Replace("JLG", "");
            ItemID = ItemID.Replace("jlg", "");
            ItemID = ItemID.Replace("pac", "");
            ItemID = ItemID.Replace("Pac", ""); ;
            ItemID = ItemID.Replace("JohnDeere", "");
            ItemID = ItemID.Replace("mack", "");
            ItemID = ItemID.Replace("NAV", "");
            ItemID = ItemID.Replace("nav", "");

            // Remove Random String
            ItemID = ItemID.Replace("NUTS", "");
            ItemID = ItemID.Replace("(Off set tooling)", "");
            ItemID = ItemID.Replace("pgm", "");
            ItemID = ItemID.Replace("2 rows", "");
            ItemID = ItemID.Replace("op", "");
            ItemID = ItemID.Replace("opp", "");
            ItemID = ItemID.Replace("OPP", "");
            ItemID = ItemID.Replace("opp1", "");
            ItemID = ItemID.Replace("opp2", "");
            ItemID = ItemID.Replace("2HITS", "");
            ItemID = ItemID.Replace("form", "");
            ItemID = ItemID.Replace("hits 4-6", "");
            ItemID = ItemID.Replace("4hits", "");
            ItemID = ItemID.Replace("ALL", "");
            ItemID = ItemID.Replace("b", "");
            ItemID = ItemID.Replace("B", "");
            ItemID = ItemID.Replace("barfold", "");
            ItemID = ItemID.Replace("Both", "");
            ItemID = ItemID.Replace("Bothhits", "");
            ItemID = ItemID.Replace("BUMP", "");
            ItemID = ItemID.Replace("button", "");
            ItemID = ItemID.Replace("curl", "");
            ItemID = ItemID.Replace("CURL", "");
            ItemID = ItemID.Replace("D", "");
            ItemID = ItemID.Replace("dual stage", "");
            ItemID = ItemID.Replace("dw", "");
            ItemID = ItemID.Replace("E", "");
            ItemID = ItemID.Replace("final", "");
            ItemID = ItemID.Replace("FINAL", "");
            ItemID = ItemID.Replace("first", "");
            ItemID = ItemID.Replace("fix", "");
            ItemID = ItemID.Replace("FLAT", "");
            ItemID = ItemID.Replace("flatten", "");
            ItemID = ItemID.Replace("form", "");
            ItemID = ItemID.Replace("FORM", "");
            ItemID = ItemID.Replace("forms", "");
            ItemID = ItemID.Replace("formtool", "");
            ItemID = ItemID.Replace("formtooling", "");
            ItemID = ItemID.Replace("FT", "");
            ItemID = ItemID.Replace("gage", "");
            ItemID = ItemID.Replace("good", "");
            ItemID = ItemID.Replace("half", "");
            ItemID = ItemID.Replace("hit", "");
            ItemID = ItemID.Replace("hit1-3", "");
            ItemID = ItemID.Replace("hits", "");
            ItemID = ItemID.Replace("Hits", "");
            ItemID = ItemID.Replace("JN", "");
            ItemID = ItemID.Replace("junk", "");
            ItemID = ItemID.Replace("plate", "");
            ItemID = ItemID.Replace("L", "");
            ItemID = ItemID.Replace("louver", "");
            ItemID = ItemID.Replace("nd", "");
            ItemID = ItemID.Replace("NEW", "");
            ItemID = ItemID.Replace("New", "");
            ItemID = ItemID.Replace("new", "");
            ItemID = ItemID.Replace("no", "");
            ItemID = ItemID.Replace("NUT", "");
            ItemID = ItemID.Replace("nut", "");
            ItemID = ItemID.Replace("nuts", "");
            ItemID = ItemID.Replace("offset", "");
            ItemID = ItemID.Replace("Offset", "");
            ItemID = ItemID.Replace("OFFSET", "");
            ItemID = ItemID.Replace("ext", "");


            ItemID = ItemID.Replace("palm", "");
            ItemID = ItemID.Replace("palmbuttons", "");
            ItemID = ItemID.Replace("pb", "");
            ItemID = ItemID.Replace("PGM", "");
            ItemID = ItemID.Replace("PIERCE", "");
            ItemID = ItemID.Replace("PRESSROOM", "");
            ItemID = ItemID.Replace("-rcs-1127", "");
            ItemID = ItemID.Replace("REG", "");
            ItemID = ItemID.Replace("rehit", "");
            ItemID = ItemID.Replace("rivet", "");
            ItemID = ItemID.Replace("s", ""); // Remove Spaces
            ItemID = ItemID.Replace("side", "");
            ItemID = ItemID.Replace("stamp", "");
            ItemID = ItemID.Replace("strap", "");
            ItemID = ItemID.Replace("STUD", "");
            ItemID = ItemID.Replace("-stud", "");
            ItemID = ItemID.Replace("T", "");
            ItemID = ItemID.Replace("tool", "");
            ItemID = ItemID.Replace("Tooling", "");
            ItemID = ItemID.Replace("Untitled", "");
            ItemID = ItemID.Replace("wc", "");
            ItemID = ItemID.Replace("wilson", "");
            ItemID = ItemID.Replace("with", "");
            ItemID = ItemID.Replace("xxx", "");
            ItemID = ItemID.Replace("TOOLING", "");
            ItemID_TextBox.Text = ItemID;
        }

        private void RunningStatistics()
        {
            double HoursRemaining = 0;
            double MinutesRemaining = 0;
            double PartsNeeded = double.Parse(PartsNeeded_TextBox.Text);
            //string PPMString = CurrentPPM_TextBox.Text;
            string RemainingTime = "";
            CurrentParts = double.Parse(PartsFormed_TextBox.Text);
            LivePPMOEECalculation();
            string PPMString = LivePPM_TextBox.Text;
            if (PartsNeeded < CurrentParts)
            {
               MessageBox.Show("Parts Needed Must be Greater than Parts Formed");
               SetupMode_Button_Click(null, null);

            } else if(PartsNeeded == CurrentParts)
            {
                SetupMode_Button_Click(null, null);
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
                HoursRemaining = 9;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes".ToString();
            }
            TimeRemaining_TextBox.Text = RemainingTime;
            PartsRunProgressBar.Maximum = int.Parse(PartsNeeded_TextBox.Text);
            PartsRunProgressBar.Value = int.Parse(PartsFormed_TextBox.Text);

        }

        private void RunMode()
        {/*
            try
            {
                for (int i = 0; i < Values.Length; i++)
                {
                    Type myType = Type.GetTypeFromProgID("PressBrake.Comm", Computer_Name.Text, true);
                    Object o = Activator.CreateInstance(myType);
                    PressBrakeCommClass pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
                    Value_Box.Text = Values[i];
                    ValueResults = pbClass.GetValue(Convert.ToInt32(Value_Box.Text));
                    ValuesArray[i] = ValueResults;
                    Value_Box.Clear();
                    PartsFormed_TextBox.Text = ValuesArray[1];
                    CurrentPPM_TextBox.Text = ValuesArray[2];
                    CurrentStep_TextBox.Text = ValuesArray[3];
                }
                RemoveSolution();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to Connect to Brake Press");
                Timer.Enabled = false;
            }
            */
        }

        private void SearchForItemID()
        {
            string SearchValue = CurrentItemID;

            UserProgramGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                foreach (DataGridViewRow row in UserProgramGridView.Rows)
                {
                    row.Selected = false;
                    if (row.Cells[0].Value.ToString().Equals(SearchValue))
                    {
                        row.Selected = true;
                        ItemID_TextBox.Text = row.Cells[0].Value.ToString();
                        Steps_TextBox.Text = row.Cells[3].Value.ToString();
                        JobID_TextBox.Text = row.Cells[4].Value.ToString();
                        StepsUsed_TextBox.Text = row.Cells[5].Value.ToString();
                        //ItemRunCount_TextBox.Text = row.Cells[9].Value.ToString();
                        PartsManufacturedTotal_String = row.Cells[10].Value.ToString();
                        AveragePPM_String = row.Cells[11].Value.ToString();
                        THEPPM_String = row.Cells[12].Value.ToString();
                        UserProgramGridView.FirstDisplayedScrollingRowIndex = UserProgramGridView.SelectedRows[0].Index;
                        JobFound = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The Following Item ID: " + SearchValue + " Was Not Found");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {            
            RunMode();
            RunningStatistics();
            
            if (j < 20)
            {
                j++;
                Refresh_TextBox.Text = j.ToString();
            }
            else if(j == 20)
            {
                SqlConnection connection = new SqlConnection(SQL_Source);
                string BP1176 = "SELECT * FROM [dbo].[BP_1176_Schedule]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BP1176, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet Data = new DataSet();
                dataAdapter.Fill(Data);
                ScheduleGridView.DataSource = Data.Tables[0];
                j = 0;

                Refresh_TextBox.Text = j.ToString();
            }
            
        }

        /*********************************************************************************************************************
        * 
        * Methods End
        * 
        *********************************************************************************************************************/

        private void User_Program_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        /*********************************************************************************************************************
        * 
        * User_Program End
        * 
        *********************************************************************************************************************/        
        /*********************************************************************************************************************
        * 
        * Methods in Testing
        * 
        * 
        * 
        * 
        * 
        * 
        * 
        *********************************************************************************************************************/

        private void Connect_Button_Click(object sender, EventArgs e)
        {
            string Read_Result = ItemID_ComboBox.Text;

            if (Read_Result != null)
            {
                ItemID_TextBox.Text = Read_Result.ToString();
                CurrentItemID = Read_Result.ToString();

            }
            if (ItemID_TextBox.TextLength == 9 && JobFound != true)
            {
                SearchForItemID();
            }
            if (JobFound == true)
            {// current 
             /*
             //Disconnect_Button_Click(null, null);
             PartsRunProgressBar.Visible = true;
             HMI_NotActive_TextBox.Visible = false;
             Connect_Button.Visible = false;
             JobStartTime = Clock_TextBox.Text;
             ItemRunCounter();
             OperationIDCounter();
             ItemOperationDataStart();
             OperationDataStart();
             Timer.Enabled = true;
             JobEnd_Button.Visible = true;
             string StartingTime = Clock_TextBox.Text;
             string ReplaceTime = DateTime.Today.ToShortDateString();
             JobStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
             */
             //Disconnect_Button_Click(null, null);
                Scan_Button.Enabled = false;
                RunMode_Button.Enabled = true;
                SetupMode_Button.Enabled = true;
                JobEnd_Button.Enabled = true;
                PartsRunProgressBar.Visible = true;
                HMI_NotActive_TextBox.Visible = false;
                JobStartTime = Clock_TextBox.Text;
                ItemRunCounter();
                OperationIDCounter();
                ItemOperationDataStart();
                OperationDataStart();
                //Timer.Enabled = true;
                JobEnd_Button.Visible = true;
                string StartingTime = Clock_TextBox.Text;
                string ReplaceTime = DateTime.Today.ToShortDateString();
                SetupStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
                SetupMode_Button_Click(null, null);
            }            
        }

        private void Calculate_Button_Click(object sender, EventArgs e)
        {
            Timer.Enabled = true;
        }

        private void RunMode_Button_Click(object sender, EventArgs e)
        {
            LIVEPPM_String = PartsFormed_TextBox.Text;
            LIVEPPM = double.Parse(LIVEPPM_String);
            SetupModeStopWatch.Stop();
            RunModeStopWatch.Start();
            PartsRunProgressBar.Visible = true;
            HMI_NotActive_TextBox.Visible = false;
            //JobStartTime = Clock_TextBox.Text;
            Timer.Enabled = true;
            Connect_Button.Visible = false;
            string StartingTime = Clock_TextBox.Text;
            string ReplaceTime = DateTime.Today.ToShortDateString();
            //JobStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
            RunMode_Button.BackColor = RunModeColor;
            SetupMode_Button.BackColor = Color.Silver;

            if(RunMode_Button_Clicked == false)
            {
                JobStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
                RunMode_Button_Clicked = true;
            }
            RunMode_Button_Clicked = true;
        }

        private void SetupMode_Button_Click(object sender, EventArgs e)
        {
            Timer.Enabled = false;
            RunModeStopWatch.Stop();
            SetupModeStopWatch.Start();
            SetupMode_Button.BackColor = Color.Yellow;
            RunMode_Button.BackColor = Color.Silver;
            SetupMode_Button_Clicked = true;
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
            double LivePPM = (Math.Round((CurrentParts / TotalElapsedTime), 2));
            LivePPM_TextBox.Text = LivePPM.ToString();
            double LiveOEE = Math.Round(LivePPM / THEPPM, 3);
            LiveOEE = LiveOEE * 100;
            LiveOEE_TextBox.Text = LiveOEE.ToString() + "%";

            try
            {
                double SUPERLIVEPPM = (Math.Round(((CurrentParts-LIVEPPM) / TotalElapsedTime), 2));
                JobEndTime_TextBox.Text = SUPERLIVEPPM.ToString();
            }
            catch
            {
                JobEndTime_TextBox.Text = "Cannot Be a Negative Number";
            }
        }

        private void HoneywellScanner_Button_Click(object sender, EventArgs e)
        {
            ItemID_TextBox.Focus();
        }

        private void BarcodeScanner()
        {
            Scan_Button.Enabled = false;
            RunMode_Button.Enabled = true;
            SetupMode_Button.Enabled = true;
            JobEnd_Button.Enabled = true;
            //Disconnect_Button_Click(null, null);
            PartsRunProgressBar.Visible = true;
            HMI_NotActive_TextBox.Visible = false;
            JobStartTime = Clock_TextBox.Text;
            ItemRunCounter();
            OperationIDCounter();
            ItemOperationDataStart();
            OperationDataStart();
            //Timer.Enabled = true;
            JobEnd_Button.Visible = true;
            string StartingTime = Clock_TextBox.Text;
            string ReplaceTime = DateTime.Today.ToShortDateString();
            SetupStartTime_TextBox.Text = StartingTime.Replace("   " + ReplaceTime, "");
            SetupMode_Button_Click(null, null);

        }

        private void ItemID_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                CurrentItemID = ItemID_TextBox.Text;
                if(CurrentItemID.Length == 9)
                {
                    SearchForItemID();
                    if(JobFound == true)
                    {
                        BarcodeScanner();
                    }
                }
            }
        }

        private void ItemID_TextBox_Enter(object sender, EventArgs e)
        {
            ItemID_TextBox.ReadOnly = false;
        }



        //
        //
        //
        //

    }
}

