using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DMP_Brake_Press_Application
{
    public partial class User_Program_Sign_Off : Form
    {
        public User_Program_Sign_Off()
        {
            InitializeComponent();
            LogOff_Button.DialogResult = DialogResult.Yes;
            StaySignedIn_Button.DialogResult = DialogResult.No;
        }

        private static int CountdownMinute;
        private static int CountdownSecond;
        string CountdownTime = "";
        private static int CountSecondUp;
        private Stopwatch SignOutTime = new Stopwatch();
        private static double TimerCount = 0;

        private void User_Program_Sign_Off_Load(object sender, EventArgs e)
        {
            Timer.Start();
            SignOutTime.Start();
            CountdownMinute = 4;
            CountdownSecond = 60;
            CountSecondUp = 1;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerCount = SignOutTime.Elapsed.Minutes;
            CountdownSecond = CountdownSecond - 1;
            CountSecondUp = CountSecondUp + 1;
            if (CountSecondUp == 300 || TimerCount >= 5)
            {
                Timer.Stop();
                LogOff_Button_Click(sender, e);
            }
            if(CountdownSecond == 0)
            {
                CountdownMinute = CountdownMinute - 1;
                //CountdownSecond = 60;
                CountdownTime = CountdownMinute.ToString() + ":0" + CountdownSecond.ToString();
                CountdownSecond = 59;
                Countdown_TextBox.Text = "You Will Be Signed Off In: " + CountdownTime;
            }
            else if(CountdownSecond <= 9 && CountdownSecond >= 1)
            {
                CountdownTime = CountdownMinute.ToString() + ":0" + CountdownSecond.ToString();
                Countdown_TextBox.Text = "You Will Be Signed Off In: " + CountdownTime;
            }
            else if(CountdownSecond >= 10 && CountdownSecond < 60)
            {
                CountdownTime = CountdownMinute + ":" + CountdownSecond;
                Countdown_TextBox.Text = "You Will Be Signed Off In: " + CountdownTime;
            }
            
            //CountdownTime = CountdownMinute + ":" + CountdownSecond;
            //Countdown_TextBox.Text = CountdownTime;
        }

        private void LogOff_Button_Click(object sender, EventArgs e)
        {
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void StaySignedIn_Button_Click(object sender, EventArgs e)
        {
            User_Program__ControlLogix_System_.UserInterface.Enabled = true;
            this.Close();
        }
    }
}
