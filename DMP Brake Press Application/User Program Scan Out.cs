using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using PressBrake;

namespace DMP_Brake_Press_Application
{
    public partial class User_Program_Scan_Out : Form
    {
        User_Program__ControlLogix_System_ Owner = null;
        BackgroundWorker BPComputerConnect;
        public User_Program_Scan_Out()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            BPComputerConnect = new BackgroundWorker();
            BPComputerConnect.DoWork += new DoWorkEventHandler(ComputerConnection);
            BPComputerConnect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BPComputerConnect_RunWorkerCompleted);
            Complete_Button.DialogResult = DialogResult.Yes;
            Cancel_Button.DialogResult = DialogResult.No;
        }

        private static string ComputerName = "";
        private static string LoginUsername = "";
        private static string LoginPassword = "WIPINV1";
        private static string LoginConfig = "OH";
        private static string DMPResID = "";
        private static bool FormCompleted = false;

        private static int LogOffValue = 0;

        private void User_Program_Scan_Out_Load(object sender, EventArgs e)
        {
            BrakePressID();
            BPComputerConnect.RunWorkerAsync();
        }

        /*********************************************************************************************************************
        * 
        * Buttons Region Start
        * -- Total: 3
        * 
        * - Completed Button Click
        * - Submit Button Click
        * - Cancel Button Click
        * 
        *********************************************************************************************************************/
        #region

        private void Complete_Button_Click(object sender, EventArgs e)
        {
            FormCompleted = false;
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            this.Close();
        }

        private void Submit_Button_Click(object sender, EventArgs e)
        {
            HtmlElement EmpNum_TextBox = ScanOutBrowser.Document.GetElementById("EmpNum");
            HtmlElement JobNum_TextBox = ScanOutBrowser.Document.GetElementById("JobNum");
            HtmlElement OperNum_TextBox = ScanOutBrowser.Document.GetElementById("OperNum");
            HtmlElement DMPResID_TextBox = ScanOutBrowser.Document.GetElementById("DMPResID");
            HtmlElement TcQtuQtyComp_TextBox = ScanOutBrowser.Document.GetElementById("TcQtuQtyComp");
            HtmlElement TcQtuQtyScrap_TextBox = ScanOutBrowser.Document.GetElementById("TcQtuQtyScrap");
            HtmlElement TcQtuQtyMove_TextBox = ScanOutBrowser.Document.GetElementById("TcQtuQtyMove");
            HtmlElement Complete_TextBox = ScanOutBrowser.Document.GetElementById("Complete");
            HtmlElement Close_TextBox = ScanOutBrowser.Document.GetElementById("Close");
            if (JobNum_TextBox.GetAttribute("value") != "" && TcQtuQtyComp_TextBox.GetAttribute("value") != "" && TcQtuQtyScrap_TextBox.GetAttribute("value") != "" && TcQtuQtyMove_TextBox.GetAttribute("value") != "" && Complete_TextBox.GetAttribute("value") != "" && Close_TextBox.GetAttribute("value") != "")
            {
                ScanOutBrowser.Focus();
                TcQtuQtyScrap_TextBox.Focus();
                SendKeys.Send("{ENTER}");
                Submit_Button.Visible = false;
                Complete_Button.Visible = true;
                FormCompleted = true;
            }
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            try
            {
                User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            }
            catch
            {

            }
            this.Close();
        }



        /*********************************************************************************************************************
        * 
        * Buttons Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * WebBrowser Region Start
        * -- Total: 1
        * 
        * - ScanOutBrowser_DocumentCompleted
        * 
        *********************************************************************************************************************/
        #region

        private void ScanOutBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            LogOffValue = 0;
            if (ScanOutBrowser.Url.AbsoluteUri == ("http://ohsenslu803/fsdatacollection/Login.asp"))
            {
                HtmlElement UserID_TextBox = ScanOutBrowser.Document.GetElementById("UserID");
                HtmlElement Password_TextBox = ScanOutBrowser.Document.GetElementById("Password");
                HtmlElement Config_TextBox = ScanOutBrowser.Document.GetElementById("Config");
                UserID_TextBox.SetAttribute("value", LoginUsername);
                Password_TextBox.SetAttribute("value", LoginPassword);
                Config_TextBox.SetAttribute("value", LoginConfig);
                foreach (HtmlElement Button in this.ScanOutBrowser.Document.GetElementsByTagName("input"))
                {
                    if (Button.GetAttribute("value").Equals("Start"))
                    {
                        Button.InvokeMember("click");
                    }
                }
            }
            if (ScanOutBrowser.Url.AbsoluteUri == ("http://ohsenslu803/fsdatacollection/default.asp"))
            {
                HtmlElement MenuSelect_TextBox = ScanOutBrowser.Document.GetElementById("MenuSelect");
                MenuSelect_TextBox.SetAttribute("value", "1");
            }
            if (ScanOutBrowser.Url.AbsoluteUri == ("http://ohsenslu803/fsdatacollection/DMPJobMove.asp") && FormCompleted == true)
            {
                HtmlElement EmpNum_TextBox = ScanOutBrowser.Document.GetElementById("EmpNum");
                HtmlElement Complete_TextBox = ScanOutBrowser.Document.GetElementById("Complete");
                try
                {
                    Complete_TextBox.Focus();
                    SendKeys.Send("{ENTER}");
                }
                catch
                {

                } //
                if (EmpNum_TextBox.GetAttribute("value") == "" && FormCompleted == true)
                {
                    FormCompleted = false;
                }
            }
            if (ScanOutBrowser.Url.AbsoluteUri == ("http://ohsenslu803/fsdatacollection/DMPJobMove.asp") && FormCompleted == false)
            {
                HtmlElement EmpNum_TextBox = ScanOutBrowser.Document.GetElementById("EmpNum");
                HtmlElement JobNum_TextBox = ScanOutBrowser.Document.GetElementById("JobNum");
                HtmlElement OperNum_TextBox = ScanOutBrowser.Document.GetElementById("OperNum");
                HtmlElement DMPResID_TextBox = ScanOutBrowser.Document.GetElementById("DMPResID");
                HtmlElement TcQtuQtyComp_TextBox = ScanOutBrowser.Document.GetElementById("TcQtuQtyComp");
                HtmlElement TcQtuQtyScrap_TextBox = ScanOutBrowser.Document.GetElementById("TcQtuQtyScrap");
                HtmlElement TcQtuQtyMove_TextBox = ScanOutBrowser.Document.GetElementById("TcQtuQtyMove");
                HtmlElement Complete_TextBox = ScanOutBrowser.Document.GetElementById("Complete");
                HtmlElement Close_TextBox = ScanOutBrowser.Document.GetElementById("Close");


               // TcQtuQtyComp_TextBox.KeyDown += new HtmlElementEventHandler(PartsCompleted_Enter);
                
                
                try
                {
                    EmpNum_TextBox.SetAttribute("value", EmployeeNumber_TextBox.Text);
                    if (JobNum_TextBox.GetAttribute("value") == "")
                    {
                        JobNum_TextBox.SetAttribute("value", JobNumber_TextBox.Text);
                    }
                    if (TcQtuQtyComp_TextBox.GetAttribute("value") == "")
                    {
                        TcQtuQtyComp_TextBox.SetAttribute("value", TotalCountQtuQtyComp_TextBox.Text);
                    }
                    DMPResID_TextBox.SetAttribute("value", DMPResID);

                    if (JobNum_TextBox.GetAttribute("value") != "" && TcQtuQtyComp_TextBox.GetAttribute("value") != "" && TcQtuQtyScrap_TextBox.GetAttribute("value") != "" && TcQtuQtyMove_TextBox.GetAttribute("value") != "" && Complete_TextBox.GetAttribute("value") != "" && Close_TextBox.GetAttribute("value") != "" && FormCompleted == false)
                    {
                        //if(TcQtuQtyScrap_TextBox != true)
                        //Close_TextBox.Focus();
                        Submit_Button.Visible = true;
                        FormCompleted = true;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }
            }
        }

        void PartsCompleted_Enter(object sender, HtmlElementEventArgs e)
        {
            HtmlElement TcQtuQtyComp_TextBox = ScanOutBrowser.Document.GetElementById("TcQtuQtyComp");
            if (e.KeyPressedCode == 13)
            {
                TotalCountQtuQtyComp_TextBox.Text = TcQtuQtyComp_TextBox.GetAttribute("value");
            }
        }

        /*********************************************************************************************************************
        * 
        * WebBrowser Region End
        * 
        *********************************************************************************************************************/
        #endregion

        /*********************************************************************************************************************
        * 
        * Method Region Start
        * -- Total: 1
        * 
        * - BrakePressID
        * - ComputerConnection
        * - BPComputerConnect RunWorkerCompleted
        * 
        *********************************************************************************************************************/
        #region

        private void BrakePressID()
        {
            string BrakePressComputerID = System.Environment.MachineName;

            // CAT Brake Press
            if (BrakePressComputerID == "OHN7017") // Brake Press 1107
            {
                ComputerName = "PB50093";
                LoginUsername = "DC1107";
                DMPResID = "BP-1107";
            }
            if (BrakePressComputerID == "BP1139") // Brake Press 1139
            {
                ComputerName = "PB51294";
                LoginUsername = "DC1139";
                DMPResID = "BP-1139";
            }
            if (BrakePressComputerID == "BP1177") // Brake Press 1177
            {
                ComputerName = "PB49568";
                LoginUsername = "DC1177";
                DMPResID = "BP-1177";
            }
            // John Deere Brake Press
            if (BrakePressComputerID == "OHN7120") // Brake Press 1127
            {
                ComputerName = "PB50093";
                LoginUsername = "DC1127";
                DMPResID = "BP-1127";
            }
            if (BrakePressComputerID == "OHN7011") // Brake Press 1178
            {
                ComputerName = "PB50093";
                LoginUsername = "DC1178";
                DMPResID = "BP-1178";
            }
            // Navistar Brake Press
            if (BrakePressComputerID == "OHN7055") // Brake Press 1065
            {
                ComputerName = "PB846662";
                LoginUsername = "DC1065";
                DMPResID = "BP-1065";
            }
            if (BrakePressComputerID == "OHN7052") // Brake Press 1108
            {
                ComputerName = "PB50208";
                LoginUsername = "DC1108";
                DMPResID = "BP-1108";
            }
            if (BrakePressComputerID == "BP1156") // Brake Press 1156
            {
                ComputerName = "PB54539";
                LoginUsername = "DC1156";
                DMPResID = "BP-1156";
            }
            if (BrakePressComputerID == "BP1720") // Brake Press 1720
            {
                ComputerName = "PB51581";
                LoginUsername = "DC1720";
                DMPResID = "BP-1720";
            }
            // Paccar Brake Press
            if (BrakePressComputerID == "OHN7066") // Brake Press 1083
            {
                ComputerName = "PB48909";
                LoginUsername = "DC1083";
                DMPResID = "BP-1083";
            }
            if (BrakePressComputerID == "OHN7121") // Brake Press 1155
            {
                ComputerName = "PB54125";
                LoginUsername = "DC1155";
                DMPResID = "BP-1155";
            }
            if (BrakePressComputerID == "OHN7067") // Brake Press 1158
            {
                ComputerName = "pb846574";
                LoginUsername = "DC1158";
                DMPResID = "BP-1158";
            }
            if (BrakePressComputerID == "OHN7122") // Brake Press 1175
            {
                ComputerName = "CI846574";
                LoginUsername = "DC1175";
                DMPResID = "BP-1175";
            }
            if (BrakePressComputerID == "OHN7009") // Brake Press 1176
            {
                ComputerName = "PB53973";
                LoginUsername = "DC1176";
                DMPResID = "BP-1176";
            }
            // My Computer For Testing
            if (BrakePressComputerID == "OHN7047NL")
            {
                ComputerName = "PB53973";
                LoginUsername = "DC1176";
                DMPResID = "BP-1176";
            }
        }


        private void ComputerConnection(object sender, EventArgs e)
        {/*
            try
            {
                Type myType = Type.GetTypeFromProgID("PressBrake.Comm", ComputerName, true);
                Object o = Activator.CreateInstance(myType);
                PressBrake.PressBrakeCommClass pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
                myType = Type.GetTypeFromProgID("PressBrake.Comm", ComputerName, true);
                o = Activator.CreateInstance(myType);
                pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
                TotalCountQtuQtyComp_TextBox.Text = pbClass.GetValue(Convert.ToInt32(12));
            }
            catch (Exception ee)
            {
                //MessageBox.Show("Unable to Connect to Brake Press" + "Error Code: " + ee.ToString());
            }*/
        }

        void BPComputerConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        /*********************************************************************************************************************
        * 
        * Method Region End
        * 
        *********************************************************************************************************************/
        #endregion

        private void Close_Timer_Tick(object sender, EventArgs e)
        {
            LogOffValue = 1 + LogOffValue;
            if (LogOffValue > 300)
            {
                this.Close();
            }
        }
    }
}
