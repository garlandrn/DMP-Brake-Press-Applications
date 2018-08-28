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
    public partial class JobList_RemoveJob : Form
    {
        public JobList_RemoveJob()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            Confirm_Button.DialogResult = DialogResult.Yes;
            Cancel_Button.DialogResult = DialogResult.No;
        }
    }
}
