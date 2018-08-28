using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMP_Brake_Press_Application
{
    public partial class DMPBrakePressLogoff : Form
    {
        public DMPBrakePressLogoff()
        {
            InitializeComponent();
            Confirm_Button.DialogResult = DialogResult.Yes;
            Cancel_Button.DialogResult = DialogResult.No;
        }

        private void Confirm_Button_Click(object sender, EventArgs e)
        {

        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {

        }

        private void Password_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Confirm_Button.PerformClick();
            }
        }
    }
}
