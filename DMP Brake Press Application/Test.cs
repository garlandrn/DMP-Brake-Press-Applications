using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PressBrake;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;

namespace DMP_Brake_Press_Application
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private static string ItemImagePath = "";
        private static string ItemID = "";
        private static string[] ItemIDSplit = ItemID.Split('-');
        private static double ItemID_Three;
        private static double ItemID_Five;
        string[] values = { "10", "12", "19", "20", "35", "9" };
        string ValueResults;

        private void Enter_Button_Click(object sender, EventArgs e)
        {
            /*
            Type myType = Type.GetTypeFromProgID("PressBrake.Comm", CPU_TextBox.Text, true);
            Object o = Activator.CreateInstance(myType);
            PressBrakeCommClass pbClass = (PressBrake.PressBrakeCommClass)System.Runtime.InteropServices.Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
            myType = Type.GetTypeFromProgID("PressBrake.Comm", CPU_TextBox.Text, true);
            o = Activator.CreateInstance(myType);
            pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
            ValueResults = pbClass.GetValue(Convert.ToInt32(Value_TextBox.Text));
            Result_ListBox.Items.Add(ValueResults);

            try
            {
                for (int i = 0; i < values.Length; i++)
                {
                    Type myType = Type.GetTypeFromProgID("PressBrake.Comm", CPU_TextBox.Text, true);
                    Object o = Activator.CreateInstance(myType);
                    PressBrake.PressBrakeCommClass pbClass = (PressBrake.PressBrakeCommClass)Marshal.CreateWrapperOfType(o, typeof(PressBrake.PressBrakeCommClass));
                    Value_TextBox.Text = values[i];
                    ValueResults = pbClass.GetValue(Convert.ToInt32(Value_TextBox.Text));
                    Value_TextBox.Clear();
                    Result_ListBox.Items.Add(ValueResults);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
                        */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ViewSetupPDFFile = @"C:\Users\rgarland\Desktop\999-99999.pdf";
            ViewPDF PDFViewer = new ViewPDF();
            PDFViewer.AcroPDF.src = ViewSetupPDFFile;
            PDFViewer.AcroPDF.BringToFront();
            PDFViewer.Show();
            PDFViewer.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp-mail.outlook.com";
                client.Timeout = 5000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("rgarland", "Felix0505");
                objeto_mail.From = new MailAddress("rgarland@DEFIANCEMETAL.com");
                objeto_mail.To.Add(new MailAddress("garlandrn05@gmail.com"));
                objeto_mail.Subject = "Password Recover";
                objeto_mail.Body = "Message";
                client.Send(objeto_mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\rgarland\Desktop\210-59415.xml");
            //doc.Load(@"\\OHN66FS01\BPprogs\1175\210-59415.pgm");

            // Owner Node
            // #document
            // xml
            // Document
            XmlNode ownerNode = doc.DocumentElement.OwnerDocument;
            Result_ListBox.Items.Add("Owner Node: " + ownerNode.Name);

            foreach (XmlNode node in ownerNode.ChildNodes)
            {
                Result_ListBox.Items.Add(ownerNode.Name + " Child: " + node.Name);
            }

            // ParentNode
            // #document
            // xml
            // Document
            XmlNode parentNode = doc.DocumentElement.ParentNode;
            Result_ListBox.Items.Add("Parent Node: " + parentNode.Name);

            foreach(XmlNode node in parentNode.ChildNodes)
            {
                Result_ListBox.Items.Add(parentNode.Name + " Child: " + node.Name);
            }
            

            // Document
            XmlNode firstChild = doc.DocumentElement.FirstChild;            
            Result_ListBox.Items.Add("First Child: " + firstChild.Name);

            // PressBrakeProgram
            // --ProgOutputGlobal
            // -- UpperToolSets
            // -- LowerToolSets
            // -- StepData
            // PressBrakePart
            foreach(XmlNode node in firstChild.ChildNodes)
            {
                Result_ListBox.Items.Add(firstChild.Name + " Child: " + node.Name);
            }


            var tmp = doc.SelectNodes("//*");
            for (int i = 0; i < tmp.Count; i++)
            {
                Result_ListBox.Items.Add(tmp.Item(i).Name);
                //if(tmp.Item(i).InnerXml != null)
                //{
                    Result_ListBox.Items.Add(" " + tmp.Item(i).InnerXml);
                //}
            }
           



            /*
            XmlNode l = doc.DocumentElement.SelectSingleNode("/Document");
            if(l.HasChildNodes == true)
            {

                Result_ListBox.Items.Add(l.FirstChild.Name);
                Result_ListBox.Items.Add(l.LastChild.Name);
            }
            

            XmlNodeList nList = doc.GetElementsByTagName("ProgOutData");
            for (int i = 0; i <nList.Count; i++)
            {
                if(nList[i].Attributes.Count != 0)
                {
                Result_ListBox.Items.Add(nList[i].Attributes["Active"].Value);
                Result_ListBox.Items.Add(nList[i].Attributes["FromType"].Value);
                Result_ListBox.Items.Add(nList[i].Attributes["ToType"].Value);

                }
            }
            */
        }

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
            
        }

        public void GetMouseClick(int xpos, int ypos)
        {
            GetMouseClick(xpos, ypos);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //LeftMouseClick(2555, 445);
            /*
            string path = @"C:\Program Files\Teledyne DALSA\iNspect Express x64\iworks.exe";
            string fileName = Path.GetFileName(path);
            // Get the precess that already running as per the exe file name.
            Process[] processName = Process.GetProcessesByName(fileName.Substring(0, fileName.LastIndexOf('.')));
            if (processName.Length > 0)
            {
                MessageBox.Show("Process already running");
            }
            else
            {
                MessageBox.Show("Not Running");
            }

    */
            Process[] running = Process.GetProcesses();
            foreach(Process p in running)
            {
                Result_ListBox.Items.Add(p.ToString());
                if(p.ProcessName == "iworks")
                {
                    MessageBox.Show("Running");
                }
            }
                        
        }

        private void Test_Click(object sender, EventArgs e)
        {
            Result_ListBox.Items.Clear();
            int x = 0;
            int y = 0;
            x = MousePosition.X;
            y = MousePosition.Y;
            Result_ListBox.Items.Add(x);
            Result_ListBox.Items.Add(y);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Result_ListBox.Items.Clear();
            int x = 0;
            int y = 0;
            x = Cursor.Position.X;
            y = Cursor.Position.Y;
            if (x >= 306 && x <= 413 && y >= 450 && y <= 490)
            {
                LeftMouseClick(500, 500);
            }
            Result_ListBox.Items.Add(x);
            Result_ListBox.Items.Add(y);
        }

        private void Test_Deactivate(object sender, EventArgs e)
        {
            
        }

        private void Test_Leave(object sender, EventArgs e)
        {

        }

        private void Test_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
