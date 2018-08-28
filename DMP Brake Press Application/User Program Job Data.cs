using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DMP_Brake_Press_Application
{
    public partial class User_Program_Job_Data : Form
    {
        User_Program__ControlLogix_System_ owner = null;
        public User_Program_Job_Data(User_Program__ControlLogix_System_ owner)
        {
            InitializeComponent();
            this.owner = owner;
            this.ShowInTaskbar = false;
            Run_Button.DialogResult = DialogResult.Yes;
            Cancel_Button.DialogResult = DialogResult.No;
        }


        private static string ReferenceNumber = "";

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
        //private string SWGroup = "";
        //private string SWItem = "";

        /********************************************************************************************************************
        * 
        * Form Load Variables 
        * 
        ********************************************************************************************************************/

        private static int QuantityOfParts;
        private static string ScannerString = "";
        private static string CurrentFixture = "";


        private void User_Program_Job_Data_Load(object sender, EventArgs e)
        {
            JobData_ListBox.Items.Add("Please Enter the Total Parts Needed");
            ItemID_TextBox.ReadOnly = true;
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            owner.ScanNewJob_Button.Focus();
            this.Close();
        }

        private void Enter_Button_Click(object sender, EventArgs e)
        {
            try
            {
                JobData_ListBox.Items.Clear();
                PartsNeeded_TextBox.ReadOnly = true;
                Enter_Button.Enabled = false;
                QuantityOfParts = Int32.Parse(PartsNeeded_TextBox.Text);
                if (QuantityOfParts >= 1)
                {
                    ShowFormData();
                    if (Fixture_TextBox.Text != "N/A")
                    {
                        Scan_TextBox.Focus();
                        JobData_ListBox.Items.Add("Please Scan Fixture");

                    }
                    else if (Fixture_TextBox.Text == "N/A")
                    {
                        Run_Button.Show();
                        Run_Button.Focus();
                        JobData_ListBox.Items.Add("No Fixture For Item");
                    }

                }
                else if (QuantityOfParts == 0)
                {
                    MessageBox.Show("Please Enter a Value Greater Than 0");
                }
            }
            catch
            {
                MessageBox.Show("Please Enter a Value Greater Than 0");
            }
        }

        /********************************************************************************************************************
        * 
        * Variables In Testing Start
        * 
        ********************************************************************************************************************/

        private void ShowFormData()
        {
            JobData_ListBox.Show();
            Fixture_Label.Show();
            Fixture_TextBox.Show();
            FixtureLocation_Label.Show();
            FixtureLocation_TextBox.Show();
            //_Label.Show();
            Fixture_TextBox.Show();
            FixtureLocation_Label.Show();
            FixtureLocation_TextBox.Show();
            Scan_Label.Show();
            Scan_TextBox.Show();
        }

        private void Scan_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {                
                ScannerString = Scan_TextBox.Text;
                ScanFixture();
                
            }
        }
        
        private void ScanFixture()
        {
            if (ScannerString == Fixture_TextBox.Text)
            {
                JobData_ListBox.Items.Add("Fixture: " + ScannerString + " Scanned" + "\n");
                this.Fixture_TextBox.BackColor = Color.Chartreuse;
                Scan_TextBox.Clear();
                Run_Button.Show();
                Run_Button.Focus();
                Scan_Label.Hide();
                Scan_TextBox.Hide();
            }
            else if (ScannerString != Fixture_TextBox.Text)
            {
                JobData_ListBox.Items.Add("Scanned Data: " + ScannerString + " Does Not Match");
                JobData_ListBox.Items.Add("Scan Fixture Again");
                Scan_TextBox.Clear();
            }
        }

        private void StartRun_Button_Click(object sender, EventArgs e)
        {
            this.owner.PassReferenceNumber(ReferenceNumber_TextBox.Text);
            this.owner.PassValue(PartsNeeded_TextBox.Text);
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            this.Close();
        }

        private void PartsNeeded_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && PartsNeeded_TextBox.ReadOnly == false)
            {
                Enter_Button_Click(null, null);
            }
        }

        private void ReferenceEnter_Button_Click(object sender, EventArgs e)
        {
            try
            {
                string JobNumber = "";
                ReferenceNumber = ReferenceNumber_TextBox.Text;
                JobNumber = ReferenceNumber.Substring(0, 11);
                if (JobNumber.Length == 11)
                {
                    PartsNeeded_TextBox.Visible = true;
                    PartsNeeded_Label.Visible = true;
                    PartsNeeded_TextBox.Focus();
                    ReferenceNumber_TextBox.ReadOnly = true;
                    ReferenceEnter_Button.Visible = false;
                    Enter_Button.Visible = true;
                }
                else if (JobNumber.Length != 11)
                {
                    MessageBox.Show("Please Scan Reference Number Again");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please Scan Reference Number Again");
            }
        }

        private void ReferenceNumber_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReferenceEnter_Button_Click(null, null);
            }
        }
    }
}
