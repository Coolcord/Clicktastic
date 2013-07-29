/* -========================- License and Distribution -========================-
 * 
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Clicktastic
{
    public partial class Form1 : Form
    {
        Boolean AutoclickerEnabled = false;
        Boolean AutoclickerActivated = false;

        public static class Prompt
        {
            public static int ShowDialog(string text, string caption)
            {
                Form prompt = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.Width = 250;
                prompt.Height = 100;
                prompt.Text = caption;
                Label textLabel = new Label() { Width = 250, Height = 65, ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter, Text = text };
                textLabel.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                prompt.Controls.Add(textLabel);
                prompt.ShowDialog();
                return 0;
            }
        }

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
                pbAutoclickerEnabled.Image = Properties.Resources.green_circle;
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
                pbAutoclickerEnabled.Image = Properties.Resources.green_circle;
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
                pbAutoclickerEnabled.Image = Properties.Resources.green_circle;
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
                pbAutoclickerEnabled.Image = Properties.Resources.green_circle;
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
                btnDeactivationButton.Enabled = true;
            }
            else
            {
                lblDeactivationButton.Enabled = false;
                tbDeactivationButton.Text = tbActivationButton.Text;
                tbDeactivationButton.Enabled = false;
                btnDeactivationButton.Enabled = false;
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

        private void tbActivationButton_Click(object sender, EventArgs e)
        {
            int promptValue = Prompt.ShowDialog("Press any key...", "Clicktastic");
        }

        private void tbDeactivationButton_Click(object sender, EventArgs e)
        {
            int promptValue = Prompt.ShowDialog("Press any key...", "Clicktastic");
        }

        private void tbAutoclickButton_Click(object sender, EventArgs e)
        {
            int promptValue = Prompt.ShowDialog("Press any key...", "Clicktastic");
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
                pbAutoclickerEnabled.Image = Properties.Resources.red_circle;
                lblAutoclickerEnabled.Text = "Disabled";
                lblAutoclickerEnabled.ForeColor = Color.Red;
            }
            else
            {
                AutoclickerEnabled = true;
                pbAutoclickerEnabled.Image = Properties.Resources.green_circle;
                lblAutoclickerEnabled.Text = "Enabled";
                lblAutoclickerEnabled.ForeColor = Color.Lime;
            }
        }

        private void pbAutoclickerRunning_Click(object sender, EventArgs e)
        {
            if (AutoclickerActivated == true)
            {
                AutoclickerActivated = false;
                pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                lblAutoclickerRunning.Text = "Waiting";
                lblAutoclickerRunning.ForeColor = Color.Red;
            }
            else
            {
                AutoclickerActivated = true;
                pbAutoclickerRunning.Image = Properties.Resources.green_circle;
                lblAutoclickerRunning.Text = "Running";
                lblAutoclickerRunning.ForeColor = Color.Lime;
            }
        }

        private void btnActivationButton_Click(object sender, EventArgs e)
        {
            int promptValue = Prompt.ShowDialog("Press any key...", "Clicktastic");
        }

        private void btnDeactivationButton_Click(object sender, EventArgs e)
        {
            int promptValue = Prompt.ShowDialog("Press any key...", "Clicktastic");
        }

        private void btnAutoclickButton_Click(object sender, EventArgs e)
        {
            int promptValue = Prompt.ShowDialog("Press any key...", "Clicktastic");
        }
    }
}
