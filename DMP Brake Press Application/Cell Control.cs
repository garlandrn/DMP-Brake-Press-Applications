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
    public partial class Cell_Control : Form
    {
        public Cell_Control()
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

        private string LoginTime = "";
        private string LoginForm = "Cell Manager";

        private string SQL_Source = @"Data Source=OHN7009,49172;Initial Catalog=Brake_Press_Data;Integrated Security=True;Connect Timeout=15;";

        // Clock_Timer();
        private static int ClockHour;
        private static int ClockMinute;
        private static int ClockSecond;

        // Search_Button_Click();
        private static string SearchValue;
        private static int SearchColumn;

        // CellManagerGridView DragDrop[];
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private string[] cbi = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};

        // Delete Buttons Click
        private string RemoveItemID;
        /********************************************************************************************************************
        * 
        * Variables In Testing Start
        * 
        ********************************************************************************************************************/

        static int RunOrder = 0;
        static string RunOrder_String = "";

        static string RemoveItem = "";

        static string SQLRemoveCommand = "";
        static string SQLAddCommand = "";
        static string SQLUpdateCommand = "";
        static string SQLSelectCount = "";
        static string Schedule_Count = "";
        static string Refresh_Data = "";

        private static int rows;

        private string[] CustomerCell = { "CAT", "John Deere", "Navistar", "Paccar", "90 Ton" };
        private string[] CAT_BrakePressList = { "1107", "1139", "1177" };
        private string[] JohnDeere_BrakePressList = { "1127", "1178" };
        private string[] Navistar_BrakePressList = { "1065", "1108", "1156", "1720" };
        private string[] Paccar_BrakePressList = { "1083", "1155", "1158", "1175", "1176" };
        private string[] Ton90_BrakePressList = { "1067", "1068", "1159" };


        private static int[] ArrayRunOrder = new int[10];
        private static string[] ArrayItemID = new string[10];
        private static string[] ArrayJobID = new string[10];
        private static int[] ArrayRowNumber = new int[10];
        private static string RowIndexClicked = "";
        private static int RowIndexClick = 0;
        private static string RunOrder_Array = "";
        private static int RowIndex = 0;


        /********************************************************************************************************************
        * 
        * Variables In Testing End
        * 
        ********************************************************************************************************************/

        private void CellManager_Load(object sender, EventArgs e)
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
            CustomerCell_ComboBox.Items.AddRange(CustomerCell);
            SearchItemID_CheckBox.Checked = true;

            WindowState = FormWindowState.Maximized;

        }

        /********************************************************************************************************************
        * 
        * Buttons Region Start
        * 
        * -- CellManager Form Button
        * - ChangeCell Button Click
        * - LogOff Click
        * 
        * -- ItemInformation GroupBox Buttons
        * - Search Button Click
        * - Clear Button Click
        * 
        * -- OrderData GroupBox Buttons
        * - AddToQueue Button Click
        * - Calculate Button Click
        * 
        * -- RunOrderGroupBox Buttons
        * - QueueUp Button Click
        * - QueueDown Button Click
        * - Remove 1 Button Click
        * - Remove 2 Button Click
        * - Remove 3 Button Click
        * - Remove 4 Button Click
        * - Remove 5 Button Click
        * - Remove 6 Button Click
        * - Remove 7 Button Click
        * - Remove 8 Button Click
        * - Remove 9 Button Click
        * - Remove 10 Button Click
        * 
        ********************************************************************************************************************/
        #region

        private void ChangeCell_Button_Click(object sender, EventArgs e)
        {
            CustomerCell_ComboBox.Enabled = true;
            CustomerCell_ComboBox.Visible = true;
            CustomerCell_TextBox.Visible = false;
            Clear();
            //GroupBoxControlStart();
            ChangeCell_Button.Hide();
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

        private void Search_Button_Click(object sender, EventArgs e)
        {
            if (SearchItemID_CheckBox.Checked == true)
            {
                SearchCustomerItemID_CheckBox.Checked = false;
                SearchValue = ItemID_TextBox.Text;
                SearchColumn = 0;
            }
            else if (SearchCustomerItemID_CheckBox.Checked == true)
            {
                SearchItemID_CheckBox.Checked = false;
                SearchValue = CustomerItemID_TextBox.Text;
                SearchColumn = 4;
            }
            CellManagerGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow Row in CellManagerGridView.Rows)
                {
                    Row.Selected = false;
                    if (Row.Cells[SearchColumn].Value.ToString().Equals(SearchValue))
                    {
                        Row.Selected = true;
                        ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                        Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                        CustomerCell_ComboBox.Text = Row.Cells[1].Value.ToString();
                        CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                        JobID_TextBox.Text = Row.Cells[4].Value.ToString();
                        TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                        PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                        PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                        SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                        Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                        ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                        FixtureLocation_TextBox.Text = Row.Cells[15].Value.ToString();
                        CellManagerGridView.FirstDisplayedScrollingRowIndex = CellManagerGridView.SelectedRows[0].Index;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error Finding Job");
            }
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            ClearCombo();
        }

        private void AddToQueue_Button_Click(object sender, EventArgs e)
        {
            GetJobQueueRunOrder();
            AddJobToQueue();
            RefreshJobs();
        }

        private void Calculate_Button_Click(object sender, EventArgs e)
        {
            double HoursRemaining = 0;
            double MinutesRemaining = 0;
            string RemainingTime = "";
            string Parts = PartsOnOrder_TextBox.Text;
            if (Parts == "0" || Parts == "")
            {
                Parts = 1.ToString();
            }
            string PPM = PPM_TextBox.Text;
            double PartsOrdered = double.Parse(Parts);
            double AveragePPM = double.Parse(PPM);
            double TimeRemaining = (PartsOrdered / AveragePPM);

            if (TimeRemaining < 60)
            {
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 1)
                {
                    RemainingTime = MinutesRemaining + " Minute";
                }
                else
                {
                    RemainingTime = MinutesRemaining + " Minutes";
                }
            }
            else if (120 > TimeRemaining && TimeRemaining >= 60)
            {
                TimeRemaining = TimeRemaining - 60;
                HoursRemaining = 1;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if(MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hour ";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hour " + MinutesRemaining + " Minutes";
                }
            }
            else if (180 > TimeRemaining && TimeRemaining >= 120)
            {
                TimeRemaining = TimeRemaining - 120;
                HoursRemaining = 2;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (240 > TimeRemaining && TimeRemaining >= 180)
            {
                TimeRemaining = TimeRemaining - 180;
                HoursRemaining = 3;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (300 > TimeRemaining && TimeRemaining >= 240)
            {
                TimeRemaining = TimeRemaining - 240;
                HoursRemaining = 4;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (360 > TimeRemaining && TimeRemaining >= 300)
            {
                TimeRemaining = TimeRemaining - 300;
                HoursRemaining = 5;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (420 > TimeRemaining && TimeRemaining >= 360)
            {
                TimeRemaining = TimeRemaining - 360;
                HoursRemaining = 6;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (480 > TimeRemaining && TimeRemaining >= 420)
            {
                TimeRemaining = TimeRemaining - 420;
                HoursRemaining = 7;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (540 > TimeRemaining && TimeRemaining >= 480)
            {
                TimeRemaining = TimeRemaining - 480;
                HoursRemaining = 8;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (600 > TimeRemaining && TimeRemaining >= 540)
            {
                TimeRemaining = TimeRemaining - 540;
                HoursRemaining = 9;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }
            else if (660 > TimeRemaining && TimeRemaining >= 600)
            {
                TimeRemaining = TimeRemaining - 600;
                HoursRemaining = 9;
                MinutesRemaining = TimeRemaining;
                MinutesRemaining = Math.Round(MinutesRemaining, MidpointRounding.AwayFromZero);
                if (MinutesRemaining == 0)
                {
                    RemainingTime = HoursRemaining + " Hours";
                }
                else
                {
                    RemainingTime = HoursRemaining + " Hours " + MinutesRemaining + " Minutes";
                }
            }

            EstimatedRunTime_TextBox.Text = RemainingTime;
        }
        
        private void QueueUp_Button_Click(object sender, EventArgs e)
        {
            if (RowIndexClick > 0)
            {
                for (int i = 0; i < RowIndex; i++)
                {
                    if (ArrayRowNumber[i] == (RowIndexClick - 1))
                    {
                        ArrayRunOrder[i] += 1;
                        ArrayRowNumber[i] += 1;
                    }
                    else if (ArrayRowNumber[i] == RowIndexClick)
                    {
                        ArrayRunOrder[i] -= 1;
                        ArrayRowNumber[i] -= 1;
                    }
                }
                for (int m = 0; m < RowIndex; m++)
                {
                    Console.WriteLine(ArrayRunOrder[m] + " " + ArrayItemID[m] + " " + ArrayRowNumber[m]);
                }
                try
                {
                    for (int x = 0; x < rows; x++)
                    {
                        SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                        SqlCommand Edit_Job = new SqlCommand();
                        Edit_Job.CommandType = System.Data.CommandType.Text;
                        Edit_Job.CommandText = SQLUpdateCommand;
                        Edit_Job.Connection = Job_Connection;
                        Edit_Job.Parameters.AddWithValue("@RunOrder", ArrayRunOrder[x].ToString());
                        Edit_Job.Parameters.AddWithValue("@ItemID", ArrayItemID[x].ToString());
                        Edit_Job.Parameters.AddWithValue("@JobID", ArrayJobID[x].ToString());
                        Job_Connection.Open();
                        Edit_Job.ExecuteNonQuery();
                        Job_Connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                RefreshJobs();
            }
            else
            {
                MessageBox.Show("The Item Selected is Already at the Top of the Queue");
            }
        }

        private void QueueDown_Button_Click(object sender, EventArgs e)
        {
            if (RowIndexClick < (RowIndex - 1))
            {
                for (int i = 0; i < RowIndex; i++)
                {
                    if (ArrayRowNumber[i] == (RowIndexClick + 1))
                    {
                        ArrayRunOrder[i] -= 1;
                        ArrayRowNumber[i] -= 1;
                    }
                    else if (ArrayRowNumber[i] == RowIndexClick)
                    {
                        ArrayRunOrder[i] += 1;
                        ArrayRowNumber[i] += 1;
                    }
                }
                for (int m = 0; m < RowIndex; m++)
                {
                    Console.WriteLine(ArrayRunOrder[m] + " " + ArrayItemID[m] + " " + ArrayRowNumber[m]);
                }
                try
                {
                    for (int x = 0; x < rows; x++)
                    {
                        SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                        SqlCommand Edit_Job = new SqlCommand();
                        Edit_Job.CommandType = System.Data.CommandType.Text;
                        Edit_Job.CommandText = SQLUpdateCommand;
                        Edit_Job.Connection = Job_Connection;
                        Edit_Job.Parameters.AddWithValue("@RunOrder", ArrayRunOrder[x].ToString());
                        Edit_Job.Parameters.AddWithValue("@ItemID", ArrayItemID[x].ToString());
                        Edit_Job.Parameters.AddWithValue("@JobID", ArrayJobID[x].ToString());
                        Job_Connection.Open();
                        Edit_Job.ExecuteNonQuery();
                        Job_Connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                RefreshJobs();
            }
            else
            {
                MessageBox.Show("The Item Selected is Already at the Bottom of the Queue");
            }     
        }
        
        // Remove Buttons Region
        #region
        private void Remove_1_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_1_TextBox.Text;
            RowIndexClick = 0;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_2_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_2_TextBox.Text;
            RowIndexClick = 1;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_3_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_3_TextBox.Text;
            RowIndexClick = 2;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_4_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_4_TextBox.Text;
            RowIndexClick = 3;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_5_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_5_TextBox.Text;
            RowIndexClick = 4;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_6_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_6_TextBox.Text;
            RowIndexClick = 5;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_7_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_7_TextBox.Text;
            RowIndexClick = 6;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_8_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_8_TextBox.Text;
            RowIndexClick = 7;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_9_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_9_TextBox.Text;
            RowIndexClick = 8;
            DeleteCombo();
            RefreshJobs();
        }

        private void Remove_10_Button_Click(object sender, EventArgs e)
        {
            RemoveItemID = RunOrder_10_TextBox.Text;
            RowIndexClick = 9;
            DeleteCombo();
            RefreshJobs();
        }
        #endregion

        /********************************************************************************************************************
        * 
        * Buttons End
        * 
        ********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        * 
        * CheckBox Region Start
        * 
        * - SearchItemID CheckBox CheckedChanged
        * - SearchJobID CheckBox CheckedChanged
        * 
        ********************************************************************************************************************/
        #region

        private void SearchItemID_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SearchItemID_CheckBox.Checked == true)
            {
                SearchCustomerItemID_CheckBox.Checked = false;
            }
        }

        private void SearchJobID_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SearchCustomerItemID_CheckBox.Checked == true)
            {
                SearchItemID_CheckBox.Checked = false;
            }
        }

        private void RunOrder_1_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(RunOrder_1_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 0;
                RunOrder_1_TextBox.BackColor = Color.Chartreuse;
                ItemID_1_TextBox.BackColor = Color.Chartreuse;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_2_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_2_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 1;
                RunOrder_2_TextBox.BackColor = Color.Chartreuse;
                ItemID_2_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_3_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_3_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 2;
                RunOrder_3_TextBox.BackColor = Color.Chartreuse;
                ItemID_3_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_4_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_4_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 3;
                RunOrder_4_TextBox.BackColor = Color.Chartreuse;
                ItemID_4_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_5_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_5_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 4;
                RunOrder_5_TextBox.BackColor = Color.Chartreuse;
                ItemID_5_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_6_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_6_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 5;
                RunOrder_6_TextBox.BackColor = Color.Chartreuse;
                ItemID_6_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_7_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_7_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 6;
                RunOrder_7_TextBox.BackColor = Color.Chartreuse;
                ItemID_7_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_8_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_8_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 7;
                RunOrder_8_TextBox.BackColor = Color.Chartreuse;
                ItemID_8_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_9_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_9_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 8;
                RunOrder_9_TextBox.BackColor = Color.Chartreuse;
                ItemID_9_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_10_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_10_CheckBox.Checked = false;
            }
        }

        private void RunOrder_10_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunOrder_10_CheckBox.Checked == true)
            {
                Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
                Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
                RowIndex = 0;
                foreach (DataGridViewRow Row in BrakePressGridView.Rows)
                {
                    RunOrder_Array = Row.Cells[0].Value.ToString();
                    ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                    ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                    ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                    RowIndex++;
                }
                RowIndexClick = 9;
                RunOrder_10_TextBox.BackColor = Color.Chartreuse;
                ItemID_10_TextBox.BackColor = Color.Chartreuse;
                RunOrder_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_1_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_2_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_3_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_4_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_5_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_6_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_7_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_8_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                ItemID_9_TextBox.BackColor = System.Drawing.SystemColors.Window;
                RunOrder_1_CheckBox.Checked = false;
                RunOrder_2_CheckBox.Checked = false;
                RunOrder_3_CheckBox.Checked = false;
                RunOrder_4_CheckBox.Checked = false;
                RunOrder_5_CheckBox.Checked = false;
                RunOrder_6_CheckBox.Checked = false;
                RunOrder_7_CheckBox.Checked = false;
                RunOrder_8_CheckBox.Checked = false;
                RunOrder_9_CheckBox.Checked = false;
            }
        }

        /********************************************************************************************************************
        * 
        * CheckBox Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        * 
        * ComboBox Region Start
        * 
        * - CustomerCell ComboBox SelectedIndexChanged
        * - BrakePress ComboBox SelectedIndexChanged
        * 
        *********************************************************************************************************************/
        #region

        private void CustomerCell_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrakePress_ComboBox.Items.Clear();
            CustomerCell_ComboBox.Enabled = false;
            CustomerCell_ComboBox.Visible = false;
            CustomerCell_TextBox.Visible = true;
            CustomerCell_TextBox.Text = CustomerCell_ComboBox.Text;
            ChangeCell_Button.Show();

            if (CustomerCell_ComboBox.Text == "CAT")
            {
                this.CATBrakePress_GroupBox.Location = new System.Drawing.Point(12, 798);
                this.CATBrakePress_GroupBox.Size = new System.Drawing.Size(428, 231);
                CATBrakePress_GroupBox.Visible = true;
                JohnDeereBrakePress_GroupBox.Visible = false;
                NavistarBrakePress_GroupBox.Visible = false;
                PaccarBrakePress_GroupBox.Visible = false;

                BrakePress_ComboBox.Items.AddRange(CAT_BrakePressList);

                SqlConnection CATConnection = new SqlConnection(SQL_Source);
                string CATString = "SELECT * FROM [dbo].[CAT_Item_Data]";
                SqlDataAdapter CATDataAdapter = new SqlDataAdapter(CATString, CATConnection);
                SqlCommandBuilder CATCommandBuilder = new SqlCommandBuilder(CATDataAdapter);
                DataSet CATData = new DataSet();
                CATDataAdapter.Fill(CATData);
                CellManagerGridView.DataSource = CATData.Tables[0];
            }
            else if (CustomerCell_ComboBox.Text == "John Deere")
            {
                this.JohnDeereBrakePress_GroupBox.Location = new System.Drawing.Point(12, 798);
                this.JohnDeereBrakePress_GroupBox.Size = new System.Drawing.Size(428, 231);
                CATBrakePress_GroupBox.Visible = false;
                JohnDeereBrakePress_GroupBox.Visible = true;
                NavistarBrakePress_GroupBox.Visible = false;
                PaccarBrakePress_GroupBox.Visible = false;

                BrakePress_ComboBox.Items.AddRange(JohnDeere_BrakePressList);

                SqlConnection JohnDeereConnection = new SqlConnection(SQL_Source);
                string JohnDeereString = "SELECT * FROM [dbo].[JohnDeere_Item_Data]";
                SqlDataAdapter JohnDeereDataAdapter = new SqlDataAdapter(JohnDeereString, JohnDeereConnection);
                SqlCommandBuilder JohnDeereCommandBuilder = new SqlCommandBuilder(JohnDeereDataAdapter);
                DataSet JohnDeereData = new DataSet();
                JohnDeereDataAdapter.Fill(JohnDeereData);
                CellManagerGridView.DataSource = JohnDeereData.Tables[0];
            }
            else if (CustomerCell_ComboBox.Text == "Navistar")
            {
                this.NavistarBrakePress_GroupBox.Location = new System.Drawing.Point(12, 798);
                this.NavistarBrakePress_GroupBox.Size = new System.Drawing.Size(602, 231);
                CATBrakePress_GroupBox.Visible = false;
                JohnDeereBrakePress_GroupBox.Visible = false;
                NavistarBrakePress_GroupBox.Visible = true;
                PaccarBrakePress_GroupBox.Visible = false;

                BrakePress_ComboBox.Items.AddRange(Navistar_BrakePressList);

                SqlConnection NavistarConnection = new SqlConnection(SQL_Source);
                string NavistarString = "SELECT * FROM [dbo].[Navistar_Item_Data]";
                SqlDataAdapter NavistarDataAdapter = new SqlDataAdapter(NavistarString, NavistarConnection);
                SqlCommandBuilder NavistarCommandBuilder = new SqlCommandBuilder(NavistarDataAdapter);
                DataSet NavistarData = new DataSet();
                NavistarDataAdapter.Fill(NavistarData);
                CellManagerGridView.DataSource = NavistarData.Tables[0];
            }
            else if (CustomerCell_ComboBox.Text == "Paccar")
            {
                this.PaccarBrakePress_GroupBox.Location = new System.Drawing.Point(12, 798);
                this.PaccarBrakePress_GroupBox.Size = new System.Drawing.Size(602, 231);
                CATBrakePress_GroupBox.Visible = false;
                JohnDeereBrakePress_GroupBox.Visible = false;
                NavistarBrakePress_GroupBox.Visible = false;
                PaccarBrakePress_GroupBox.Visible = true;

                BrakePress_ComboBox.Items.AddRange(Paccar_BrakePressList);

                SqlConnection PaccarConnection = new SqlConnection(SQL_Source);
                string PaccarString = "SELECT * FROM [dbo].[Paccar_Item_Data]";
                //string PaccarString = "SELECT ItemID, Customer, CustomerItemID, TotalRuns, PartsManufactured, PartsPerMinute, SetupTime, BP1083, BP1155, BP1158, BP1175, BP1176 FROM [dbo].[Paccar_Item_Data]";
                SqlDataAdapter PaccarDataAdapter = new SqlDataAdapter(PaccarString, PaccarConnection);
                SqlCommandBuilder PaccarCommandBuilder = new SqlCommandBuilder(PaccarDataAdapter);
                DataSet PaccarData = new DataSet();
                PaccarDataAdapter.Fill(PaccarData);
                CellManagerGridView.DataSource = PaccarData.Tables[0];
            }
        }

        private void BrakePress_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // CAT Brake Press
            if (BrakePress_ComboBox.Text == "1107")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1107_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1107_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1107_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1107_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1107_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID = @JobID";

                SqlConnection Connection_1107 = new SqlConnection(SQL_Source);
                string BP1107 = "SELECT * FROM [dbo].[BP_1107_Schedule]";
                SqlDataAdapter DataAdapter_1107 = new SqlDataAdapter(BP1107, Connection_1107);
                SqlCommandBuilder CommandBuilder_1107 = new SqlCommandBuilder(DataAdapter_1107);
                DataSet Data_1107 = new DataSet();
                DataAdapter_1107.Fill(Data_1107);
                BrakePressGridView.DataSource = Data_1107.Tables[0];

            }
            else if (BrakePress_ComboBox.Text == "1139")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1139_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1139_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1139_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1139_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1139_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1139 = new SqlConnection(SQL_Source);
                string BP1139 = "SELECT * FROM [dbo].[BP_1139_Schedule]";
                SqlDataAdapter DataAdapter_1139 = new SqlDataAdapter(BP1139, Connection_1139);
                SqlCommandBuilder CommandBuilder_1139 = new SqlCommandBuilder(DataAdapter_1139);
                DataSet Data_1139 = new DataSet();
                DataAdapter_1139.Fill(Data_1139);
                BrakePressGridView.DataSource = Data_1139.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1177")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1177_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1177_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1177_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1177_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1177_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1177 = new SqlConnection(SQL_Source);
                string BP1177 = "SELECT * FROM [dbo].[BP_1177_Schedule]";
                SqlDataAdapter DataAdapter_1177 = new SqlDataAdapter(BP1177, Connection_1177);
                SqlCommandBuilder CommandBuilder_1177 = new SqlCommandBuilder(DataAdapter_1177);
                DataSet Data_1177 = new DataSet();
                DataAdapter_1177.Fill(Data_1177);
                BrakePressGridView.DataSource = Data_1177.Tables[0];
            }
            // John Deere Brake Press
            else if (BrakePress_ComboBox.Text == "1127")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1127_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1127_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1127_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1127_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1127_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1127 = new SqlConnection(SQL_Source);
                string BP1127 = "SELECT * FROM [dbo].[BP_1127_Schedule]";
                SqlDataAdapter DataAdapter_1127 = new SqlDataAdapter(BP1127, Connection_1127);
                SqlCommandBuilder CommandBuilder_1127 = new SqlCommandBuilder(DataAdapter_1127);
                DataSet Data_1127 = new DataSet();
                DataAdapter_1127.Fill(Data_1127);
                BrakePressGridView.DataSource = Data_1127.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1178")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1178_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1178_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1178_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1178_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1178_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1178 = new SqlConnection(SQL_Source);
                string BP1178 = "SELECT * FROM [dbo].[BP_1178_Schedule]";
                SqlDataAdapter DataAdapter_1178 = new SqlDataAdapter(BP1178, Connection_1178);
                SqlCommandBuilder CommandBuilder_1178 = new SqlCommandBuilder(DataAdapter_1178);
                DataSet Data_1178 = new DataSet();
                DataAdapter_1178.Fill(Data_1178);
                BrakePressGridView.DataSource = Data_1178.Tables[0];
            }
            // Navistar Brake Press
            else if (BrakePress_ComboBox.Text == "1065")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1065_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1065_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1065_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1065_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1065_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1065 = new SqlConnection(SQL_Source);
                string BP1065 = "SELECT * FROM [dbo].[BP_1065_Schedule]";
                SqlDataAdapter DataAdapter_1065 = new SqlDataAdapter(BP1065, Connection_1065);
                SqlCommandBuilder CommandBuilder_1065 = new SqlCommandBuilder(DataAdapter_1065);
                DataSet Data_1065 = new DataSet();
                DataAdapter_1065.Fill(Data_1065);
                BrakePressGridView.DataSource = Data_1065.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1108")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1108_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1108_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1108_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1108_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1108_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1108 = new SqlConnection(SQL_Source);
                string BP1108 = "SELECT * FROM [dbo].[BP_1108_Schedule]";
                SqlDataAdapter DataAdapter_1108 = new SqlDataAdapter(BP1108, Connection_1108);
                SqlCommandBuilder CommandBuilder_1108 = new SqlCommandBuilder(DataAdapter_1108);
                DataSet Data_1108 = new DataSet();
                DataAdapter_1108.Fill(Data_1108);
                BrakePressGridView.DataSource = Data_1108.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1156")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1156_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1156_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1156_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1156_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1156_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1156 = new SqlConnection(SQL_Source);
                string BP1156 = "SELECT * FROM [dbo].[BP_1156_Schedule]";
                SqlDataAdapter DataAdapter_1156 = new SqlDataAdapter(BP1156, Connection_1156);
                SqlCommandBuilder CommandBuilder_1156 = new SqlCommandBuilder(DataAdapter_1156);
                DataSet Data_1156 = new DataSet();
                DataAdapter_1156.Fill(Data_1156);
                BrakePressGridView.DataSource = Data_1156.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1720")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1720_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1720_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1720_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1720_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1720_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1720 = new SqlConnection(SQL_Source);
                string BP1720 = "SELECT * FROM [dbo].[BP_1720_Schedule]";
                SqlDataAdapter DataAdapter_1720 = new SqlDataAdapter(BP1720, Connection_1720);
                SqlCommandBuilder CommandBuilder_1720 = new SqlCommandBuilder(DataAdapter_1720);
                DataSet Data_1720 = new DataSet();
                DataAdapter_1720.Fill(Data_1720);
                BrakePressGridView.DataSource = Data_1720.Tables[0];
            }
            // Paccar Brake Press
            else if (BrakePress_ComboBox.Text == "1083")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1083_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1083_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1083_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1083_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1083_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1083 = new SqlConnection(SQL_Source);
                string BP1083 = "SELECT * FROM [dbo].[BP_1083_Schedule]";
                SqlDataAdapter DataAdapter_1083 = new SqlDataAdapter(BP1083, Connection_1083);
                SqlCommandBuilder CommandBuilder_1083 = new SqlCommandBuilder(DataAdapter_1083);
                DataSet Data_1083 = new DataSet();
                DataAdapter_1083.Fill(Data_1083);
                BrakePressGridView.DataSource = Data_1083.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1155")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1155_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1155_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1155_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1155_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1155_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1155 = new SqlConnection(SQL_Source);
                string BP1155 = "SELECT * FROM [dbo].[BP_1155_Schedule]";
                SqlDataAdapter DataAdapter_1155 = new SqlDataAdapter(BP1155, Connection_1155);
                SqlCommandBuilder CommandBuilder_1155 = new SqlCommandBuilder(DataAdapter_1155);
                DataSet Data_1155 = new DataSet();
                DataAdapter_1155.Fill(Data_1155);
                BrakePressGridView.DataSource = Data_1155.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1158")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1158_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1158_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1158_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1158_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1158_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1158 = new SqlConnection(SQL_Source);
                string BP1158 = "SELECT * FROM [dbo].[BP_1158_Schedule]";
                SqlDataAdapter DataAdapter_1158 = new SqlDataAdapter(BP1158, Connection_1158);
                SqlCommandBuilder CommandBuilder_1158 = new SqlCommandBuilder(DataAdapter_1158);
                DataSet Data_1158 = new DataSet();
                DataAdapter_1158.Fill(Data_1158);
                BrakePressGridView.DataSource = Data_1158.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1175")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1175_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1175_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1175_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1175_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1175_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1175 = new SqlConnection(SQL_Source);
                string BP1175 = "SELECT * FROM [dbo].[BP_1175_Schedule]";
                SqlDataAdapter DataAdapter_1175 = new SqlDataAdapter(BP1175, Connection_1175);
                SqlCommandBuilder CommandBuilder_1175 = new SqlCommandBuilder(DataAdapter_1175);
                DataSet Data_1175 = new DataSet();
                DataAdapter_1175.Fill(Data_1175);
                BrakePressGridView.DataSource = Data_1175.Tables[0];
            }
            else if (BrakePress_ComboBox.Text == "1176")
            {
                Refresh_Data = "SELECT * FROM [dbo].[BP_1176_Schedule] ORDER BY RunOrder ASC";
                Schedule_Count = "SELECT COUNT(*) FROM[dbo].[BP_1176_Schedule]";
                SQLRemoveCommand = "DELETE FROM [dbo].[BP_1176_Schedule] WHERE RunOrder=@RunOrder";
                SQLAddCommand = "INSERT INTO [dbo].[BP_1176_Schedule] (RunOrder,ItemID,JobID,Customer,CustomerItemID,Tooling,ToolingLocation,FixtureLocation,PartsOrdered,EstimatedRunTime) VALUES (@RunOrder,@ItemID,@JobID,@Customer,@CustomerItemID,@Tooling,@ToolingLocation,@FixtureLocation,@PartsOrdered,@EstimatedRunTime)";
                SQLUpdateCommand = "UPDATE [dbo].[BP_1176_Schedule] SET RunOrder=@RunOrder WHERE ItemID=@ItemID AND JobID=@JobID";

                SqlConnection Connection_1176 = new SqlConnection(SQL_Source);
                string BP1176 = "SELECT * FROM [dbo].[BP_1176_Schedule]";
                SqlDataAdapter DataAdapter_1176 = new SqlDataAdapter(BP1176, Connection_1176);
                SqlCommandBuilder CommandBuilder_1176 = new SqlCommandBuilder(DataAdapter_1176);
                DataSet Data_1176 = new DataSet();
                DataAdapter_1176.Fill(Data_1176);
                BrakePressGridView.DataSource = Data_1176.Tables[0];
            }
            RefreshJobs();
        }

        /********************************************************************************************************************
        * 
        * ComboBox Region End
        * 
        ********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        * 
        * GridView Region Start 
        * 
        * - CellManagerGridView CellClick
        * - BrakePressGridView CellClick
        * 
        *********************************************************************************************************************/
        #region

        private void CellManagerGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellManagerGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (e.RowIndex >= 0)
            {
                if(CustomerCell_ComboBox.Text == "CAT")
                {
                    DataGridViewRow Row = CellManagerGridView.Rows[e.RowIndex];
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    BP1107_TextBox.Text = Row.Cells[19].Value.ToString();
                    BP1139_TextBox.Text = Row.Cells[20].Value.ToString();
                    BP1177_TextBox.Text = Row.Cells[21].Value.ToString();
                    EstimatedRunTime_TextBox.Text = CellManagerGridView.Rows.IndexOf(Row).ToString();                    
                }
                else if(CustomerCell_ComboBox.Text == "John Deere")
                {
                    DataGridViewRow Row = CellManagerGridView.Rows[e.RowIndex];
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    BP1127_TextBox.Text = Row.Cells[19].Value.ToString();
                    BP1178_TextBox.Text = Row.Cells[20].Value.ToString();
                    EstimatedRunTime_TextBox.Text = CellManagerGridView.Rows.IndexOf(Row).ToString();
                }
                else if (CustomerCell_ComboBox.Text == "Navistar")
                {
                    DataGridViewRow Row = CellManagerGridView.Rows[e.RowIndex];
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    BP1065_TextBox.Text = Row.Cells[19].Value.ToString();
                    BP1108_TextBox.Text = Row.Cells[20].Value.ToString();
                    BP1156_TextBox.Text = Row.Cells[21].Value.ToString();
                    BP1720_TextBox.Text = Row.Cells[22].Value.ToString();
                    EstimatedRunTime_TextBox.Text = CellManagerGridView.Rows.IndexOf(Row).ToString();
                }
                else if (CustomerCell_ComboBox.Text == "Paccar")
                {
                    DataGridViewRow Row = CellManagerGridView.Rows[e.RowIndex];                    
                    ItemID_TextBox.Text = Row.Cells[0].Value.ToString();
                    Customer_TextBox.Text = Row.Cells[1].Value.ToString();
                    CustomerItemID_TextBox.Text = Row.Cells[2].Value.ToString();
                    JobID_TextBox.Text = Row.Cells[3].Value.ToString();
                    TotalRuns_TextBox.Text = Row.Cells[9].Value.ToString();
                    PartsManufactured_TextBox.Text = Row.Cells[10].Value.ToString();
                    PPM_TextBox.Text = Row.Cells[11].Value.ToString();
                    SetupTime_TextBox.Text = Row.Cells[12].Value.ToString();
                    Tooling_TextBox.Text = Row.Cells[13].Value.ToString();
                    ToolingLocation_TextBox.Text = Row.Cells[14].Value.ToString();
                    FixtureLocation_TextBox.Text = Row.Cells[16].Value.ToString();
                    BP1083_TextBox.Text = Row.Cells[19].Value.ToString();
                    BP1155_TextBox.Text = Row.Cells[20].Value.ToString();
                    BP1158_TextBox.Text = Row.Cells[21].Value.ToString();
                    BP1175_TextBox.Text = Row.Cells[22].Value.ToString();
                    BP1176_TextBox.Text = Row.Cells[23].Value.ToString();
                }
            }
        }

        private void BrakePressGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckBox[] RunOrderCheckBoxArray = { RunOrder_1_CheckBox, RunOrder_2_CheckBox, RunOrder_3_CheckBox, RunOrder_4_CheckBox, RunOrder_5_CheckBox, RunOrder_6_CheckBox, RunOrder_7_CheckBox, RunOrder_8_CheckBox, RunOrder_9_CheckBox, RunOrder_10_CheckBox };
            TextBox[] RunOrderTextBoxArray = { RunOrder_1_TextBox, RunOrder_2_TextBox, RunOrder_3_TextBox, RunOrder_4_TextBox, RunOrder_5_TextBox, RunOrder_6_TextBox, RunOrder_7_TextBox, RunOrder_8_TextBox, RunOrder_9_TextBox, RunOrder_10_TextBox };
            TextBox[] ItemIDTextBoxArray = { ItemID_1_TextBox, ItemID_2_TextBox, ItemID_3_TextBox, ItemID_4_TextBox, ItemID_5_TextBox, ItemID_6_TextBox, ItemID_7_TextBox, ItemID_8_TextBox, ItemID_9_TextBox, ItemID_10_TextBox };
            Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
            Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
            Array.Clear(ArrayJobID, 0, ArrayItemID.Length);
            Array.Clear(ArrayRowNumber, 0, ArrayItemID.Length);
            RowIndex = 0;
            BrakePressGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow DataRow = BrakePressGridView.Rows[e.RowIndex];
                RowIndexClicked = BrakePressGridView.Rows.IndexOf(DataRow).ToString();
                RowIndexClick = Int32.Parse(RowIndexClicked);
            }
            foreach (DataGridViewRow Row in BrakePressGridView.Rows)
            {
                RunOrder_Array = Row.Cells[0].Value.ToString();
                ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                ArrayJobID[RowIndex] = Row.Cells[2].Value.ToString();
                ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                RowIndex++;
            }
            for(int r = 0; r < RowIndex; ++r)
            {
                if (ArrayRowNumber[r] == RowIndexClick)
                {
                    //RunOrderTextBoxArray[r].BackColor = Color.Chartreuse;
                    //ItemIDTextBoxArray[r].BackColor = Color.Chartreuse;
                    RunOrderCheckBoxArray[r].Checked = true;
                }
                else
                {
                    //RunOrderTextBoxArray[r].BackColor = System.Drawing.SystemColors.Window;
                    //ItemIDTextBoxArray[r].BackColor = System.Drawing.SystemColors.Window;
                    RunOrderCheckBoxArray[r].Checked = false;
                }
            }
            for (int m = 0; m < RowIndex; m++)
            {
                Console.WriteLine("Run Order: " + ArrayRunOrder[m] + " Item ID: " + ArrayItemID[m] + " Row Number: " + ArrayRowNumber[m] + " Row Clicked: " + RowIndexClick + " Total Number of Rows: " + RowIndex);
            }
            BrakePressGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        /********************************************************************************************************************
        * 
        * GridView Region End
        * 
        ********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        * 
        * Methods Region Start
        * 
        * - EmployeeLogOff
        * - RefreshJobs
        * - GetJobQueueRunOrder
        * - AddJobToQueue
        * - RemoveJobFromQueue
        * - CountRows
        * 
        *********************************************************************************************************************/
        #region

        private void Clear()
        {
            // ItemInformation_GroupBox
            ItemID_TextBox.Clear();
            JobID_TextBox.Clear();
            SearchItemID_CheckBox.Checked = true;
            SearchCustomerItemID_CheckBox.Checked = false;
            Customer_TextBox.Clear();
            CustomerItemID_TextBox.Clear();

            // ItemStatistics_GroupBox
            TotalRuns_TextBox.Clear();
            PartsManufactured_TextBox.Clear();
            SetupTime_TextBox.Clear();
            PPM_TextBox.Clear();

            // OrderData_GroupBox
            PartsOnOrder_TextBox.Clear();
            EstimatedRunTime_TextBox.Clear();
            EstimatedStartTime_TextBox.Clear();
            QueuePosition_TextBox.Clear();

            // OrderData_GroupBox
            Tooling_TextBox.Clear();
            ToolingLocation_TextBox.Clear();
            FixtureLocation_TextBox.Clear();
            Scanner3D_TextBox.Clear();

            // CATBrakePress_GroupBox      
            BP1107_TextBox.Clear();
            BP1139_TextBox.Clear();
            BP1177_TextBox.Clear();

            // JohnDeereBrakePress_GroupBox      
            BP1127_TextBox.Clear();
            BP1178_TextBox.Clear();

            // NavistarBrakePress_GroupBox      
            BP1065_TextBox.Clear();
            BP1108_TextBox.Clear();
            BP1156_TextBox.Clear();
            BP1720_TextBox.Clear();

            // PaccarBrakePress_GroupBox      
            BP1083_TextBox.Clear();
            BP1155_TextBox.Clear();
            BP1158_TextBox.Clear();
            BP1175_TextBox.Clear();
            BP1176_TextBox.Clear();
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

        private void RefreshJobs()
        {
            Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
            Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
            RowIndex = 0;
            Button[] RemoveButtonArray = { Remove_1_Button, Remove_2_Button, Remove_3_Button, Remove_4_Button, Remove_5_Button, Remove_6_Button, Remove_7_Button, Remove_8_Button, Remove_9_Button, Remove_10_Button };
            CheckBox[] RunOrderCheckBoxArray = { RunOrder_1_CheckBox, RunOrder_2_CheckBox, RunOrder_3_CheckBox, RunOrder_4_CheckBox, RunOrder_5_CheckBox, RunOrder_6_CheckBox, RunOrder_7_CheckBox, RunOrder_8_CheckBox, RunOrder_9_CheckBox, RunOrder_10_CheckBox };
            TextBox[] RunOrderTextBoxArray = { RunOrder_1_TextBox, RunOrder_2_TextBox, RunOrder_3_TextBox, RunOrder_4_TextBox, RunOrder_5_TextBox, RunOrder_6_TextBox, RunOrder_7_TextBox, RunOrder_8_TextBox, RunOrder_9_TextBox, RunOrder_10_TextBox };
            TextBox[] ItemIDTextBoxArray = { ItemID_1_TextBox, ItemID_2_TextBox, ItemID_3_TextBox, ItemID_4_TextBox, ItemID_5_TextBox, ItemID_6_TextBox, ItemID_7_TextBox, ItemID_8_TextBox, ItemID_9_TextBox, ItemID_10_TextBox };
            Label[] RunLabelArray = { Run_1_Label, Run_2_Label, Run_3_Label, Run_4_Label, Run_5_Label, Run_6_Label, Run_7_Label, Run_8_Label, Run_9_Label, Run_10_Label };
            Label[] ItemLabelArray = { Item_1_Label, Item_2_Label, Item_3_Label, Item_4_Label, Item_5_Label, Item_6_Label, Item_7_Label, Item_8_Label, Item_9_Label, Item_10_Label };
            string RefreshDataString = Refresh_Data;
            SqlConnection RefreshConnection = new SqlConnection(SQL_Source);
            SqlDataAdapter RefreshDataAdapter = new SqlDataAdapter(RefreshDataString, RefreshConnection);
            SqlCommandBuilder RefreshCommandBuilder = new SqlCommandBuilder(RefreshDataAdapter);
            DataSet RefreshData = new DataSet();
            RefreshDataAdapter.Fill(RefreshData);
            BrakePressGridView.DataSource = RefreshData.Tables[0];

            ClearCombo();

            rows = 0;
            string CountString = Schedule_Count;
            SqlConnection CountConnection = new SqlConnection(SQL_Source);
            SqlCommand CountCommand = new SqlCommand(CountString, CountConnection);
            CountConnection.Open();
            rows = (int)CountCommand.ExecuteScalar();
            CountConnection.Close();

            foreach (DataGridViewRow Row in BrakePressGridView.Rows)
            {
                if (Row.Index < rows)
                {
                    RunOrderTextBoxArray[Row.Index].Text = Row.Cells[0].Value.ToString();
                    ItemIDTextBoxArray[Row.Index].Text = Row.Cells[1].Value.ToString();
                    RunOrderTextBoxArray[Row.Index].BackColor = System.Drawing.SystemColors.Window;
                    ItemIDTextBoxArray[Row.Index].BackColor = System.Drawing.SystemColors.Window;
                    RemoveButtonArray[Row.Index].Show();
                    RunOrderCheckBoxArray[Row.Index].Show();
                    RunOrderTextBoxArray[Row.Index].Show();
                    ItemIDTextBoxArray[Row.Index].Show();
                    RunLabelArray[Row.Index].Show();
                    ItemLabelArray[Row.Index].Show();
                }
            }
        }

        private void GetJobQueueRunOrder()
        {
            string ScheduleCount = Schedule_Count;
            SqlConnection count = new SqlConnection(SQL_Source);
            SqlCommand countData = new SqlCommand(ScheduleCount, count);
            count.Open();
            RunOrder = (int)countData.ExecuteScalar();
            count.Close();
            RunOrder = RunOrder + 1;
        }

        private void AddJobToQueue()
        {
            SqlConnection Job_Connection = new SqlConnection(SQL_Source);
            SqlCommand Add_Job = new SqlCommand();
            Add_Job.CommandType = System.Data.CommandType.Text;
            Add_Job.CommandText = SQLAddCommand;
            Add_Job.Connection = Job_Connection;
            Add_Job.Parameters.AddWithValue("@RunOrder", RunOrder.ToString());
            Add_Job.Parameters.AddWithValue("@ItemID", ItemID_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@JobID", JobID_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@Customer", Customer_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@CustomerItemID", CustomerItemID_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@Tooling", Tooling_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@ToolingLocation", ToolingLocation_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@FixtureLocation", FixtureLocation_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@PartsOrdered", PartsOnOrder_TextBox.Text);
            Add_Job.Parameters.AddWithValue("@EstimatedRunTime", EstimatedRunTime_TextBox.Text);
            Job_Connection.Open();
            Add_Job.ExecuteNonQuery();
            Job_Connection.Close();
        }

        private void RemoveJobFromQueue()
        {
            try
            {
                SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                SqlCommand Delete_Job = new SqlCommand();
                Delete_Job.CommandType = System.Data.CommandType.Text;
                Delete_Job.CommandText = SQLRemoveCommand;
                Delete_Job.Connection = Job_Connection;
                Delete_Job.Parameters.AddWithValue("@RunOrder", RemoveItem);
                Job_Connection.Open();
                Delete_Job.ExecuteNonQuery();
                Job_Connection.Close();
                MessageBox.Show("Job Was Successfully Removed");
            }
            catch (SqlException)
            {
                MessageBox.Show("Error Removing Job");
            }

        }        

        private void CountRows()
        {
            rows = 0;
            string CountRowsString = Schedule_Count;
            SqlConnection count = new SqlConnection(SQL_Source);
            SqlCommand countRows = new SqlCommand(CountRowsString, count);
            count.Open();
            rows = (int)countRows.ExecuteScalar();
            count.Close();
        }

        private void GetCell()
        {

        }

        /*********************************************************************************************************************
        * 
        * Methods Region End
        * 
        **********************************************************************************************************************/
        #endregion

        /********************************************************************************************************************
        * 
        * Events Region Start
        * 
        * - Clock Tick
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
            Time += "   " + Date;
            Clock_TextBox.Text = Time;
        }

        private void BP1083_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1155_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1158_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1175_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1176_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1127_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1178_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1065_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1108_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1156_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1720_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1107_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1139_TextBox_Enter(object sender, EventArgs e)
        {

        }

        private void BP1177_TextBox_Enter(object sender, EventArgs e)
        {

        }
        /********************************************************************************************************************
        * 
        * Events Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Methods in Testing Region Start 
        * 
        *********************************************************************************************************************/
        #region

        private void CountThis()
        {
            string ScheduleCount = Schedule_Count;
            SqlConnection count = new SqlConnection(SQL_Source);
            SqlCommand countData = new SqlCommand(ScheduleCount, count);
            count.Open();
            int P = (int)countData.ExecuteScalar();
            count.Close();            
        }

        private void DeleteCombo()
        {
            Array.Clear(ArrayRunOrder, 0, ArrayRunOrder.Length);
            Array.Clear(ArrayItemID, 0, ArrayItemID.Length);
            RowIndex = 0;
            foreach (DataGridViewRow Row in BrakePressGridView.Rows)
            {
                RunOrder_Array = Row.Cells[0].Value.ToString();
                ArrayRunOrder[RowIndex] = Int32.Parse(RunOrder_Array);
                ArrayItemID[RowIndex] = Row.Cells[1].Value.ToString();
                ArrayRowNumber[RowIndex] = BrakePressGridView.Rows.IndexOf(Row);
                RowIndex++;
            }
            if (RowIndexClick > 0)
            {
                for (int i = 0; i < RowIndex; i++)
                {
                    if (ArrayRowNumber[i] > RowIndexClick)
                    {
                        ArrayRunOrder[i] -= 1;
                        ArrayRowNumber[i] -= 1;
                    }
                }
            }
            try
            {
                SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                SqlCommand Delete_Job = new SqlCommand();
                Delete_Job.CommandType = System.Data.CommandType.Text;
                Delete_Job.CommandText = SQLRemoveCommand;
                Delete_Job.Connection = Job_Connection;
                Delete_Job.Parameters.AddWithValue("@RunOrder", RemoveItemID);
                Job_Connection.Open();
                Delete_Job.ExecuteNonQuery();
                Job_Connection.Close();
                MessageBox.Show("Job Was Successfully Removed");
            }
            catch (SqlException)
            {
                MessageBox.Show("Error Removing Job");
            }
            try
            {
                for (int x = 0; x < rows; x++)
                {
                    SqlConnection Job_Connection = new SqlConnection(SQL_Source);
                    SqlCommand Edit_Job = new SqlCommand();
                    Edit_Job.CommandType = System.Data.CommandType.Text;
                    Edit_Job.CommandText = SQLUpdateCommand;
                    Edit_Job.Connection = Job_Connection;
                    Edit_Job.Parameters.AddWithValue("@RunOrder", ArrayRunOrder[x].ToString());
                    Edit_Job.Parameters.AddWithValue("@ItemID", ArrayItemID[x].ToString());
                    Edit_Job.Parameters.AddWithValue("@JobID", ArrayJobID[x].ToString());
                    Job_Connection.Open();
                    Edit_Job.ExecuteNonQuery();
                    Job_Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            RefreshJobs();
        }

        private void ClearCombo()
        {
            Button[] RemoveButtonArray = { Remove_1_Button, Remove_2_Button, Remove_3_Button, Remove_4_Button, Remove_5_Button, Remove_6_Button, Remove_7_Button, Remove_8_Button, Remove_9_Button, Remove_10_Button };
            TextBox[] RunOrderTextBoxArray = { RunOrder_1_TextBox, RunOrder_2_TextBox, RunOrder_3_TextBox, RunOrder_4_TextBox, RunOrder_5_TextBox, RunOrder_6_TextBox, RunOrder_7_TextBox, RunOrder_8_TextBox, RunOrder_9_TextBox, RunOrder_10_TextBox };
            TextBox[] ItemIDTextBoxArray = { ItemID_1_TextBox, ItemID_2_TextBox, ItemID_3_TextBox, ItemID_4_TextBox, ItemID_5_TextBox, ItemID_6_TextBox, ItemID_7_TextBox, ItemID_8_TextBox, ItemID_9_TextBox, ItemID_10_TextBox };
            CheckBox[] RunOrderCheckBoxArray = { RunOrder_1_CheckBox, RunOrder_2_CheckBox, RunOrder_3_CheckBox, RunOrder_4_CheckBox, RunOrder_5_CheckBox, RunOrder_6_CheckBox, RunOrder_7_CheckBox, RunOrder_8_CheckBox, RunOrder_9_CheckBox, RunOrder_10_CheckBox };
            Label[] RunLabelArray = { Run_1_Label, Run_2_Label, Run_3_Label, Run_4_Label, Run_5_Label, Run_6_Label, Run_7_Label, Run_8_Label, Run_9_Label, Run_10_Label };
            Label[] ItemLabelArray = { Item_1_Label, Item_2_Label, Item_3_Label, Item_4_Label, Item_5_Label, Item_6_Label, Item_7_Label, Item_8_Label, Item_9_Label, Item_10_Label };
            for (int i = 0; i < 10; i++)
            {
                RunLabelArray[i].Hide();
                RunOrderTextBoxArray[i].Hide();
                ItemLabelArray[i].Hide();
                RunOrderCheckBoxArray[i].Checked = false;
                RunOrderCheckBoxArray[i].Hide();
                ItemIDTextBoxArray[i].Clear();
                ItemIDTextBoxArray[i].Hide();
                RemoveButtonArray[i].Hide();
                RunOrderTextBoxArray[i].BackColor = System.Drawing.SystemColors.Window;
                ItemIDTextBoxArray[i].BackColor = System.Drawing.SystemColors.Window;
            }
        }

        /********************************************************************************************************************
        * 
        * Methods in Testing End
        * 
        ********************************************************************************************************************/
        #endregion
               
    }
}
