using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clicktastic
{
    public partial class Form1 : Form
    {
        Boolean AutoclickerEnabled = false;
        Boolean AutoclickerActivated = false;

        public Form1()
        {
            InitializeComponent();
            ddbProfile.SelectedIndex = 0;
            ddbActivationMode.SelectedIndex = 0;
            ddbSpeedMode.SelectedIndex = 0;
            setInstructions();
        }

        private void setInstructions()
        {
            if (cbUseDeactivationButton.Checked && ddbActivationMode.SelectedIndex == 0)
            {
                lblActivationInstructions.Text = "Press " + tbActivationButton.Text + " to activate Autoclicker on " + tbAutoclickButton.Text;
                lblHoldInstructions.Text = "Hold " + tbAutoclickButton.Text + " to autoclick";
                lblDeactivationInstructions.Text = "Press " + tbDeactivationButton.Text + " to deactivate Autoclicker";
                pbAutoclickerEnabled.ImageLocation = "C:\\Users\\Cord\\Desktop\\green_circle_small.png";
                lblAutoclickerEnabled.Text = "Enabled";
                lblAutoclickerEnabled.ForeColor = Color.Lime;
                lblHoldInstructions.Enabled = false;
                lblHoldInstructions.Visible = false;
                lblDeactivationInstructions.Enabled = true;
                lblDeactivationInstructions.Visible = true;
                lblAutoclickerEnabled.Enabled = false;
                lblAutoclickerEnabled.Visible = false;
                lblAutoclickerEnabled.Enabled = false;
                lblAutoclickerEnabled.Visible = false;
                pbAutoclickerEnabled.Enabled = false;
                pbAutoclickerEnabled.Visible = false;
            }
            else if (!cbUseDeactivationButton.Checked && ddbActivationMode.SelectedIndex == 0)
            {
                lblActivationInstructions.Text = "Press " + tbActivationButton.Text + " to toggle Autoclicker on " + tbAutoclickButton.Text;
                lblHoldInstructions.Text = "Hold " + tbAutoclickButton.Text + " to autoclick";
                lblDeactivationInstructions.Text = "Press " + tbDeactivationButton.Text + " to toggle Autoclicker";
                pbAutoclickerEnabled.ImageLocation = "C:\\Users\\Cord\\Desktop\\green_circle_small.png";
                lblAutoclickerEnabled.Text = "Enabled";
                lblAutoclickerEnabled.ForeColor = Color.Lime;
                lblHoldInstructions.Enabled = false;
                lblHoldInstructions.Visible = false;
                lblDeactivationInstructions.Enabled = false;
                lblDeactivationInstructions.Visible = false;
                lblAutoclickerEnabled.Enabled = false;
                lblAutoclickerEnabled.Visible = false;
                pbAutoclickerEnabled.Enabled = false;
                pbAutoclickerEnabled.Visible = false;
            }
            else if (cbUseDeactivationButton.Checked && ddbActivationMode.SelectedIndex == 1)
            {
                lblActivationInstructions.Text = "Press " + tbActivationButton.Text + " to enable Autoclicker on " + tbAutoclickButton.Text;
                lblHoldInstructions.Text = "Hold " + tbAutoclickButton.Text + " to autoclick";
                lblDeactivationInstructions.Text = "Press " + tbDeactivationButton.Text + " to disable Autoclicker";
                pbAutoclickerEnabled.ImageLocation = "C:\\Users\\Cord\\Desktop\\green_circle_small.png";
                lblAutoclickerEnabled.Text = "Enabled";
                lblAutoclickerEnabled.ForeColor = Color.Lime;
                lblHoldInstructions.Enabled = true;
                lblHoldInstructions.Visible = true;
                lblDeactivationInstructions.Enabled = true;
                lblDeactivationInstructions.Visible = true;
                lblAutoclickerEnabled.Enabled = true;
                lblAutoclickerEnabled.Visible = true;
                pbAutoclickerEnabled.Enabled = true;
                pbAutoclickerEnabled.Visible = true;
            }
            else
            {
                lblActivationInstructions.Text = "Press " + tbActivationButton.Text + " to enable Autoclicker on " + tbAutoclickButton.Text;
                lblHoldInstructions.Text = "Hold " + tbAutoclickButton.Text + " to autoclick";
                lblDeactivationInstructions.Text = "Press " + tbDeactivationButton.Text + " to disable Autoclicker";
                pbAutoclickerEnabled.ImageLocation = "C:\\Users\\Cord\\Desktop\\green_circle_small.png";
                lblAutoclickerEnabled.Text = "Enabled";
                lblAutoclickerEnabled.ForeColor = Color.Lime;
                lblHoldInstructions.Enabled = true;
                lblHoldInstructions.Visible = true;
                lblDeactivationInstructions.Enabled = false;
                lblDeactivationInstructions.Visible = false;
                lblAutoclickerEnabled.Enabled = true;
                lblAutoclickerEnabled.Visible = true;
                pbAutoclickerEnabled.Enabled = true;
                pbAutoclickerEnabled.Visible = true;
            }
            if (ddbSpeedMode.SelectedIndex == 0)
            {
                lblSpeedInstructions.Text = "Autoclicker will run at " + pluralCPS(numMinSpeed.Value);
            }
            else
            {
                if (numMinSpeed.Value == numMaxSpeed.Value)
                    lblSpeedInstructions.Text = "Autoclicker will run at " + pluralCPS(numMinSpeed.Value);
                else
                    lblSpeedInstructions.Text = "Autoclicker will run between " + numMinSpeed.Value + " and " + pluralCPS(numMaxSpeed.Value);
            }
        }

        private string pluralCPS(decimal num)
        {
            if (num == 1)
                return "1 click per second";
            else
                return num + " clicks per second";
        }

        private void cbUseDeactivationButton_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseDeactivationButton.Checked)
            {
                lblDeactivationButton.Enabled = true;
                tbDeactivationButton.Enabled = true;
            }
            else
            {
                lblDeactivationButton.Enabled = false;
                tbDeactivationButton.Text = tbActivationButton.Text;
                tbDeactivationButton.Enabled = false;
            }
            setInstructions();
        }

        private void tbActivationButton_TextChanged(object sender, EventArgs e)
        {
            if (!cbUseDeactivationButton.Checked)
            {
                tbDeactivationButton.Text = tbActivationButton.Text;
            }
            setInstructions();
        }

        private void ddbSpeedMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddbSpeedMode.SelectedIndex == 0) //constant speed
            {
                lblMaxCPS.Enabled = false;
                lblMaxSpeed.Enabled = false;
                numMaxSpeed.Enabled = false;
                lblMaxCPS.Visible = false;
                lblMaxSpeed.Visible = false;
                numMaxSpeed.Visible = false;
                numMaxSpeed.Value = numMinSpeed.Value;
                lblMinSpeed.Text = "Speed:";
            }
            else //random speed
            {
                lblMaxCPS.Enabled = true;
                lblMaxSpeed.Enabled = true;
                numMaxSpeed.Enabled = true;
                lblMaxCPS.Visible = true;
                lblMaxSpeed.Visible = true;
                numMaxSpeed.Visible = true;
                numMaxSpeed.Value = numMinSpeed.Value;
                lblMinSpeed.Text = "Minimum Speed:";
            }
            setInstructions();
        }

        private void numMinSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (numMaxSpeed.Value < numMinSpeed.Value)
                numMaxSpeed.Value = numMinSpeed.Value;
            setInstructions();
        }

        private void numMaxSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (numMinSpeed.Value > numMaxSpeed.Value)
                numMinSpeed.Value = numMaxSpeed.Value;
            setInstructions();
        }

        private void tbDeactivationButton_TextChanged(object sender, EventArgs e)
        {
            setInstructions();
        }

        private void tbAutoclickButton_TextChanged(object sender, EventArgs e)
        {
            setInstructions();
        }

        private void ddbActivationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            setInstructions();
        }

        private void ddbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            setInstructions();
        }

        private void pbAutoclickerEnabled_Click(object sender, EventArgs e)
        {
            if (AutoclickerEnabled == true)
            {
                AutoclickerEnabled = false;
                pbAutoclickerEnabled.ImageLocation = "C:\\Users\\Cord\\Desktop\\red_circle_small.png";
                lblAutoclickerEnabled.Text = "Disabled";
                lblAutoclickerEnabled.ForeColor = Color.Red;
            }
            else
            {
                AutoclickerEnabled = true;
                pbAutoclickerEnabled.ImageLocation = "C:\\Users\\Cord\\Desktop\\green_circle_small.png";
                lblAutoclickerEnabled.Text = "Enabled";
                lblAutoclickerEnabled.ForeColor = Color.Lime;
            }
        }

        private void pbAutoclickerRunning_Click(object sender, EventArgs e)
        {
            if (AutoclickerActivated == true)
            {
                AutoclickerActivated = false;
                pbAutoclickerRunning.ImageLocation = "C:\\Users\\Cord\\Desktop\\red_circle_small.png";
                lblAutoclickerRunning.Text = "Waiting";
                lblAutoclickerRunning.ForeColor = Color.Red;
            }
            else
            {
                AutoclickerActivated = true;
                pbAutoclickerRunning.ImageLocation = "C:\\Users\\Cord\\Desktop\\green_circle_small.png";
                lblAutoclickerRunning.Text = "Running";
                lblAutoclickerRunning.ForeColor = Color.Lime;
            }
        }
    }
}
