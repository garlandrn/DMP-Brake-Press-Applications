using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Opc.Da;

namespace DMP_Brake_Press_Application
{
    public partial class User_Program_Select_Operation : Form
    {
        User_Program__ControlLogix_System_ Owner = null;
        public User_Program_Select_Operation(User_Program__ControlLogix_System_ Owner)
        {
            InitializeComponent();
            this.Owner = Owner;
            this.ShowInTaskbar = false;
            Operation_1_Button.DialogResult = DialogResult.Yes;
            Operation_2_Button.DialogResult = DialogResult.Yes;
            Operation_3_Button.DialogResult = DialogResult.Yes;
        }

        public int SelectedOperation;

        private Opc.URL OPCUrl;
        private Opc.Da.Server OPCServer;
        private OpcCom.Factory OPCFactory = new OpcCom.Factory();
        private Opc.Da.Subscription OperationSelection_Write;
        private Opc.Da.SubscriptionState OperationSelection_StateWrite;
        private Opc.Da.Subscription OperationSelection_Read;
        private Opc.Da.SubscriptionState OperationSelection_StateRead;
        private static string BrakePressTag_OPC = "";

        private void User_Program_Select_Operation_Load(object sender, EventArgs e)
        {
            BrakePressID();
            OPCServer = new Opc.Da.Server(OPCFactory, null);
            //.Url = new Opc.URL("opcda://OHN66OPC/Matrikon.OPC.AllenBradleyPLCs.1");
            OPCServer.Url = new Opc.URL("opcda://OHN66OPC/Kepware.KEPServerEX.V6");
            //OPCServer.Url = new Opc.URL("opcda://OHN7009/Matrikon.OPC.AllenBradleyPLCs.1");
            OPCServer.Connect();

            OperationSelection_StateRead = new Opc.Da.SubscriptionState();
            OperationSelection_StateRead.Name = "153R_Spotweld";
            OperationSelection_StateRead.UpdateRate = 200;
            OperationSelection_StateRead.Active = true;

            OperationSelection_Read = (Opc.Da.Subscription)OPCServer.CreateSubscription(OperationSelection_StateRead);

            OperationSelection_StateWrite = new Opc.Da.SubscriptionState();
            OperationSelection_StateWrite.Name = "PB_OperationSelect_WriteGroup";
            OperationSelection_StateWrite.Active = false;
            OperationSelection_Write = (Opc.Da.Subscription)OPCServer.CreateSubscription(OperationSelection_StateWrite);
        }

        private void Operation_1_Button_Click(object sender, EventArgs e)
        {
            SelectedOperation = 1;
            OperationWriteOPC();
            OPCServer.Disconnect();
            this.Owner.PassOperationValue(SelectedOperation);
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            this.Close();
        }

        private void Operation_2_Button_Click(object sender, EventArgs e)
        {
            SelectedOperation = 2;
            OperationWriteOPC();
            OPCServer.Disconnect();
            this.Owner.PassOperationValue(SelectedOperation);
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            this.Close();
        }

        private void Operation_3_Button_Click(object sender, EventArgs e)
        {
            SelectedOperation = 3;
            OperationWriteOPC();
            OPCServer.Disconnect();
            this.Owner.PassOperationValue(SelectedOperation);
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            this.Close();
        }

        private void OperationWriteOPC()
        {
            /*
            Opc.Da.Item[] OPC_ItemID = new Opc.Da.Item[3];
            OPC_ItemID[0] = new Opc.Da.Item();
            OPC_ItemID[0].ItemName = BrakePressTag_OPC + "HMI_Operation_One_PB.VALUE";
            OPC_ItemID[1] = new Opc.Da.Item();
            OPC_ItemID[1].ItemName = BrakePressTag_OPC + "HMI_Operation_Two_PB.VALUE";
            OPC_ItemID[2] = new Opc.Da.Item();
            OPC_ItemID[2].ItemName = BrakePressTag_OPC + "HMI_Operation_Three_PB.VALUE";
            OPC_ItemID = OperationSelection_Write.AddItems(OPC_ItemID);
            */

            Opc.Da.Item[] OPC_ItemID = new Opc.Da.Item[3];
            OPC_ItemID[0] = new Opc.Da.Item();
            OPC_ItemID[0].ItemName = BrakePressTag_OPC + "HMI_Operation_One_PB";
            OPC_ItemID[1] = new Opc.Da.Item();
            OPC_ItemID[1].ItemName = BrakePressTag_OPC + "HMI_Operation_Two_PB";
            OPC_ItemID[2] = new Opc.Da.Item();
            OPC_ItemID[2].ItemName = BrakePressTag_OPC + "HMI_Operation_Three_PB";
            OPC_ItemID = OperationSelection_Write.AddItems(OPC_ItemID);

            Opc.Da.ItemValue[] ItemID_OPCValue = new Opc.Da.ItemValue[3];
            ItemID_OPCValue[0] = new Opc.Da.ItemValue();
            ItemID_OPCValue[1] = new Opc.Da.ItemValue();
            ItemID_OPCValue[2] = new Opc.Da.ItemValue();

            switch (SelectedOperation)
            {
                case 1: // Operation #1
                    ItemID_OPCValue[0].ServerHandle = OperationSelection_Write.Items[0].ServerHandle;
                    ItemID_OPCValue[0].Value = 1;
                    ItemID_OPCValue[1].ServerHandle = OperationSelection_Write.Items[1].ServerHandle;
                    ItemID_OPCValue[1].Value = 0;
                    ItemID_OPCValue[2].ServerHandle = OperationSelection_Write.Items[2].ServerHandle;
                    ItemID_OPCValue[2].Value = 0;
                    break;

                case 2: // Operation #2
                    ItemID_OPCValue[0].ServerHandle = OperationSelection_Write.Items[0].ServerHandle;
                    ItemID_OPCValue[0].Value = 0;
                    ItemID_OPCValue[1].ServerHandle = OperationSelection_Write.Items[1].ServerHandle;
                    ItemID_OPCValue[1].Value = 1;
                    ItemID_OPCValue[2].ServerHandle = OperationSelection_Write.Items[2].ServerHandle;
                    ItemID_OPCValue[2].Value = 0;
                    break;

                case 3: // Operation #3
                    ItemID_OPCValue[0].ServerHandle = OperationSelection_Write.Items[0].ServerHandle;
                    ItemID_OPCValue[0].Value = 0;
                    ItemID_OPCValue[1].ServerHandle = OperationSelection_Write.Items[1].ServerHandle;
                    ItemID_OPCValue[1].Value = 0;
                    ItemID_OPCValue[2].ServerHandle = OperationSelection_Write.Items[2].ServerHandle;
                    ItemID_OPCValue[2].Value = 1;
                    break;
            }

            Opc.IRequest OPCRequest;
            OperationSelection_Write.Write(ItemID_OPCValue, 123, new Opc.Da.WriteCompleteEventHandler(WriteCompleteCallback), out OPCRequest);
        }

        private void WriteCompleteCallback(object clientHandle, Opc.IdentifiedResult[] results)
        {
            foreach (Opc.IdentifiedResult writeResult in results)
            {
                Console.WriteLine("\t{0} write result: {1}", writeResult.ItemName, writeResult.ResultID);
            }
        }

        private void BrakePressID()
        {
            string BrakePressComputerID = System.Environment.MachineName;
            // CAT Brake Press
            if (BrakePressComputerID == "OHN7017") // Brake Press 1107
            {
                //BrakePressTag_OPC = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1107.";
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1107."; 
            }
            else if (BrakePressComputerID == "BP1139") // Brake Press 1139
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1139.";
            }
            else if (BrakePressComputerID == "BP1177") // Brake Press 1177
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1177.";
            }
            // John Deere Brake Press
            else if (BrakePressComputerID == "OHN7120") // Brake Press 1127
            {
                //BrakePressTag_OPC = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1127.";
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1127.";
            }
            else if (BrakePressComputerID == "OHN7011") // Brake Press 1178
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_1178.Global.";
            }
            // Navistar Brake Press
            else if (BrakePressComputerID == "OHN7055") // Brake Press 1065
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_1065.Global.";
            }
            else if (BrakePressComputerID == "OHN7052") // Brake Press 1108
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_1108.Global.";
            }
            else if (BrakePressComputerID == "OHN7082") // Brake Press 1156
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1156.";
            }
            else if (BrakePressComputerID == "OHN7148") // Brake Press 1720
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1720.";
            }
            // Paccar Brake Press
            else if (BrakePressComputerID == "OHN7066") // Brake Press 1083
            {
                //BrakePressTag_OPC = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1083.";
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1083.";
            }
            else if (BrakePressComputerID == "OHN7121") // Brake Press 1155
            {
                //BrakePressTag_OPC = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1155.";
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1155.";
            }
            else if (BrakePressComputerID == "OHN7067") // Brake Press 1158
            {
                //BrakePressTag_OPC = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1158.";
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1158.";
            }
            else if (BrakePressComputerID == "OHN7122") // Brake Press 1175
            {
                //BrakePressTag_OPC = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1175.";
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1175.";
            }
            else if (BrakePressComputerID == "OHN7009") // Brake Press 1176
            {
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.";
            }
            // My Computer For Testing
            else if (BrakePressComputerID == "OHN7047NL")
            {
                //BrakePressTag_OPC = "Brake_Press_1176:BRAKE_PRESS_1176:BRAKE_PRESS_HANDSHAKE:BRAKE_PRESS_1107.";
                BrakePressTag_OPC = "OHN66OPC.Brake_Press_ControlLogix.Global.Brake_Press_1107.";
            }
        }

    }
}
