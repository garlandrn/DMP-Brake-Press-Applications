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
    public partial class ViewPDF : Form
    {
        public ViewPDF()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        private void Close_Button_Click(object sender, EventArgs e)
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
    }
}
