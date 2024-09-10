using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace Kilolani_WebIO_Power_Controller
{

    public partial class KilolaniPowerManagementForm : Form
    {
        private ASCOM.DriverAccess.Switch driver;

        public KilolaniPowerManagementForm()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.DriverId))
            {
                try
                {
                    driver = new ASCOM.DriverAccess.Switch(Properties.Settings.Default.DriverId);
                    driver.Connected = true;
                }
                catch
                { }
                if (IsConnected)
                {
                    ButtonColorOff(StandbyPowerButton);
                    ButtonColorOff(FullPowerButton);
                    ButtonColorOff(CloseButton);
                    ButtonColorOn(PowerCheckButton);
                    try { PowerCheck(); }
                    catch { return; }
                    ButtonColorOff(PowerCheckButton);
                }
            }
            SetUIState();
        }

        private void KilolaniPowerManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        private void ChooseButton_Click(object sender, EventArgs e)
        {
            string driverId = ASCOM.DriverAccess.Switch.Choose(Properties.Settings.Default.DriverId);
            Properties.Settings.Default.DriverId = driverId;
            Properties.Settings.Default.Save();
            DeviceIdLabel.Text = Properties.Settings.Default.DriverId;
            SetUIState();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
            }
            else
            {
                driver = new ASCOM.DriverAccess.Switch(Properties.Settings.Default.DriverId);
                driver.Connected = true;
                if (IsConnected)
                {
                    ButtonColorOff(StandbyPowerButton);
                    ButtonColorOff(FullPowerButton);
                    ButtonColorOn(PowerCheckButton);
                    PowerCheck();
                    ButtonColorOff(PowerCheckButton);
                }
                SetUIState();
            }
        }

        private bool SetUIState()
        { //returns true if ASCOM has a device, false if not
          // boolean only used for start up
            DeviceIdLabel.Text = Properties.Settings.Default.DriverId ?? "No Device Selected";
            ChooseButton.Enabled = !IsConnected;
            ConnectButton.Text = IsConnected ? "Disconnect" : "Connect";
            return (IsConnected);
        }

        private bool IsConnected
        {
            get
            {
                ASCOM.DriverAccess.Switch dvr;
                if (this.driver != null)
                {
                    dvr = this.driver;
                    bool cState = driver.Connected;
                }
                if (this.driver == null)
                    return false;
                if (driver.Connected != true)
                    return false;
                return true;
            }
        }

        private void PowerCheckButton_Click(object sender, EventArgs e)
        //Checks the current state of the power control relays -- vicariously
        {
            if (IsConnected)
            {
                ButtonColorOn(PowerCheckButton);
                PowerCheck();
                ButtonColorOff(PowerCheckButton);
            }
            return;
        }

        private void FullPowerbutton_Click(object sender, EventArgs e)
        //Turns on all power control relays
        {
            ButtonColorOn(FullPowerButton);
            if (IsConnected)
                PowerKilolaniOn();
            ButtonColorOff(FullPowerButton);
            ButtonColorOn(PowerCheckButton);
            PowerCheck();
            ButtonColorOff(PowerCheckButton);
            return;
        }

        public void PowerKilolaniOn()
        {
            //Highlight and turn on all channels
            driver.SetSwitch(IOCfg.PowerButton1Channel, IOCfg.PowerButton1ChannelOn);
            driver.SetSwitch(IOCfg.PowerButton2Channel, IOCfg.PowerButton2ChannelOn);
            driver.SetSwitch(IOCfg.PowerButton5Channel, IOCfg.PowerButton5ChannelOn);
            driver.SetSwitch(IOCfg.PowerButton6Channel, IOCfg.PowerButton6ChannelOn);
            driver.SetSwitch(IOCfg.PowerButton7Channel, IOCfg.PowerButton7ChannelOn);
            driver.SetSwitch(IOCfg.PowerButton8Channel, IOCfg.PowerButton8ChannelOn);
            driver.SetSwitch(IOCfg.PowerButton3Channel, IOCfg.PowerButton3ChannelOn);
            driver.SetSwitch(IOCfg.PowerButton4Channel, IOCfg.PowerButton4ChannelOn);
            System.Threading.Thread.Sleep(1000);
            return;
        }

        private void StandbyPowerButton_Click(object sender, EventArgs e)
        {
            ButtonColorOn(StandbyPowerButton);
            if (IsConnected)
                PowerKilolaniStandby();
            ButtonColorOff(StandbyPowerButton);
            ButtonColorOn(PowerCheckButton);
            PowerCheck();
            ButtonColorOff(PowerCheckButton);
            return;
        }

        public void PowerKilolaniStandby()
        {
            //Highlight and turn off all channels
            driver.SetSwitch(IOCfg.PowerButton1Channel, IOCfg.PowerButton1ChannelStby);
            driver.SetSwitch(IOCfg.PowerButton2Channel, IOCfg.PowerButton2ChannelStby);
            driver.SetSwitch(IOCfg.PowerButton5Channel, IOCfg.PowerButton5ChannelStby);
            driver.SetSwitch(IOCfg.PowerButton6Channel, IOCfg.PowerButton6ChannelStby);
            driver.SetSwitch(IOCfg.PowerButton7Channel, IOCfg.PowerButton7ChannelStby);
            driver.SetSwitch(IOCfg.PowerButton8Channel, IOCfg.PowerButton8ChannelStby);
            driver.SetSwitch(IOCfg.PowerButton3Channel, IOCfg.PowerButton3ChannelStby);
            driver.SetSwitch(IOCfg.PowerButton4Channel, IOCfg.PowerButton4ChannelStby);
            System.Threading.Thread.Sleep(1000);
            return;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        //Close app, but leave I/O as set
        {
            if (IsConnected)
            {
                driver.Connected = false;
            }
            //the settings are saved automatically when this application is closed.
            Close();
            return;
        }

        #region power buttons

        private void PowerButton1_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 1
            if (IsConnected)
            {
                PowerButton1.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton1Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton1Channel, false);
                    PowerButton1.BackColor = System.Drawing.Color.LightGray;
                    PowerButton1.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton1Channel, true);
                    PowerButton1.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton1.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerButton2_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 2
            if (IsConnected)
            {
                PowerButton2.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton2Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton2Channel, false);
                    PowerButton2.BackColor = System.Drawing.Color.LightGray;
                    PowerButton2.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton2Channel, true);
                    PowerButton2.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton2.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerButton3_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 1
            if (IsConnected)
            {
                PowerButton3.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton3Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton3Channel, false);
                    PowerButton3.BackColor = System.Drawing.Color.LightGray;
                    PowerButton3.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton3Channel, true);
                    PowerButton3.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton3.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerButton4_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 1
            if (IsConnected)
            {
                PowerButton4.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton4Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton4Channel, false);
                    PowerButton4.BackColor = System.Drawing.Color.LightGray;
                    PowerButton4.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton4Channel, true);
                    PowerButton4.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton4.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerButton5_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 1
            if (IsConnected)
            {
                PowerButton5.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton5Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton5Channel, false);
                    PowerButton5.BackColor = System.Drawing.Color.LightGray;
                    PowerButton5.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton5Channel, true);
                    PowerButton5.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton5.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerButton6_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 1
            if (IsConnected)
            {
                PowerButton6.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton6Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton6Channel, false);
                    PowerButton6.BackColor = System.Drawing.Color.LightGray;
                    PowerButton6.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton6Channel, true);
                    PowerButton6.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton6.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerButton7_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 1
            if (IsConnected)
            {
                PowerButton7.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton7Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton7Channel, false);
                    PowerButton7.BackColor = System.Drawing.Color.LightGray;
                    PowerButton7.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton7Channel, true);
                    PowerButton7.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton7.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerButton8_Click(object sender, EventArgs e)
        {
            //Toggles the power control relay 1
            if (IsConnected)
            {
                PowerButton8.BackColor = System.Drawing.Color.LightSalmon;
                Show();
                System.Windows.Forms.Application.DoEvents();

                if (driver.GetSwitch(IOCfg.PowerButton8Channel))
                {
                    driver.SetSwitch(IOCfg.PowerButton8Channel, false);
                    PowerButton8.BackColor = System.Drawing.Color.LightGray;
                    PowerButton8.ForeColor = System.Drawing.Color.LightCoral;
                }
                else
                {
                    driver.SetSwitch(IOCfg.PowerButton8Channel, true);
                    PowerButton8.BackColor = System.Drawing.Color.LightCoral;
                    PowerButton8.ForeColor = System.Drawing.Color.Black;
                }
                return;
            }
        }

        private void PowerCheck()
        {
            //Determines the current state of the power control relays
            if (driver.GetSwitch(IOCfg.PowerButton1Channel))
            {
                PowerButton1.BackColor = System.Drawing.Color.LightCoral;
                PowerButton1.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton1.BackColor = System.Drawing.Color.LightGray;
                PowerButton1.ForeColor = System.Drawing.Color.LightCoral;  
            }
            if (driver.GetSwitch(IOCfg.PowerButton2Channel))
            {
                PowerButton2.BackColor = System.Drawing.Color.LightCoral;
                PowerButton2.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton2.BackColor = System.Drawing.Color.LightGray;
                PowerButton2.ForeColor = System.Drawing.Color.LightCoral;
            }
            if (driver.GetSwitch(IOCfg.PowerButton3Channel))
            {
                PowerButton3.BackColor = System.Drawing.Color.LightCoral;
                PowerButton3.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton3.BackColor = System.Drawing.Color.LightGray;
                PowerButton3.ForeColor = System.Drawing.Color.LightCoral;

            }
            if (driver.GetSwitch(IOCfg.PowerButton4Channel))
            {
                PowerButton4.BackColor = System.Drawing.Color.LightCoral;
                PowerButton4.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton4.BackColor = System.Drawing.Color.LightGray;
                PowerButton4.ForeColor = System.Drawing.Color.LightCoral;
            }

            if (driver.GetSwitch(IOCfg.PowerButton5Channel))
            {
                PowerButton5.BackColor = System.Drawing.Color.LightCoral;
                PowerButton5.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton5.BackColor = System.Drawing.Color.LightGray;
                PowerButton5.ForeColor = System.Drawing.Color.LightCoral;
            }
            if (driver.GetSwitch(IOCfg.PowerButton6Channel))
            {
                PowerButton6.BackColor = System.Drawing.Color.LightCoral;
                PowerButton6.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton6.BackColor = System.Drawing.Color.LightGray;
                PowerButton6.ForeColor = System.Drawing.Color.LightCoral;
            }
            if (driver.GetSwitch(IOCfg.PowerButton7Channel))
            {
                PowerButton7.BackColor = System.Drawing.Color.LightCoral;
                PowerButton7.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton7.BackColor = System.Drawing.Color.LightGray;
                PowerButton7.ForeColor = System.Drawing.Color.LightCoral;
            }
            if (driver.GetSwitch(IOCfg.PowerButton8Channel))
            {
                PowerButton8.BackColor = System.Drawing.Color.LightCoral;
                PowerButton8.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                PowerButton8.BackColor = System.Drawing.Color.LightGray;
                PowerButton8.ForeColor = System.Drawing.Color.LightCoral;
            }
            return;
        }

        #endregion

        private void ButtonColorOn(Button butt)
        {
            butt.BackColor = System.Drawing.Color.LightSalmon;
            Show();
            System.Windows.Forms.Application.DoEvents();
            return;
        }

        private void ButtonColorOff(Button butt)
        {
            butt.BackColor = System.Drawing.Color.LightGreen;
            Show();
            System.Windows.Forms.Application.DoEvents();
            return;
        }

        private void ButtonColorBlank(Button butt)
        {
            butt.BackColor = System.Drawing.Color.Gray;
            Show();
            System.Windows.Forms.Application.DoEvents();
            return;
        }
    }

}
