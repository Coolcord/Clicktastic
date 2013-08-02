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
        string ActivationKey = "~";
        string DeactivationKey = "~";
        string AutoclickKey = "a";

        /*
        public static class Prompt
        {
            public static int ShowDialog(string text, string caption)
            {
                Form prompt = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
                keyPrompt.StartPosition = FormStartPosition.CenterParent;
                keyPrompt.Width = 250;
                keyPrompt.Height = 100;
                keyPrompt.Text = caption;
                //keyPrompt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GetKeyPress);
                Label lblKey = new Label() { Width = 250, Height = 65, ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter, Text = text };
                lblKey.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                keyPrompt.Controls.Add(lblKey);
                keyPrompt.ShowDialog();
                return 0;
            }
        }
        */

        public string GetKeyDialog()
        {
            Form keyPrompt = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
            keyPrompt.StartPosition = FormStartPosition.CenterParent;
            keyPrompt.Width = 250;
            keyPrompt.Height = 100;
            keyPrompt.Text = "Clicktastic";
            keyPrompt.KeyPreview = true;
            Label lblKey = new Label() { Width = 250, Height = 65, ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter, Text = "Press any key" };
            lblKey.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            keyPrompt.Controls.Add(lblKey);
            System.Timers.Timer timer = new System.Timers.Timer(750);
            int textState = 1;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += (sender, e) =>
            {
                try
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        switch (textState)
                        {
                            case 0:
                                lblKey.Text = "Press any key";
                                break;
                            case 1:
                                lblKey.Text = "Press any key.";
                                break;
                            case 2:
                                lblKey.Text = "Press any key..";
                                break;
                            case 3:
                                lblKey.Text = "Press any key...";
                                break;
                        }
                    }));
                    textState = (textState + 1) % 4;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            };
            string key = null;
            keyPrompt.PreviewKeyDown += (sender, e) =>
            {
                timer.Stop();

                //Determine the key pressed
                if (e.KeyCode == Keys.Menu)
                    key = "Alt";
                else if (e.KeyCode == Keys.ShiftKey)
                    key = "Shift";
                else if (e.KeyCode == Keys.ControlKey)
                    key = "Ctrl";
                else
                    key = e.KeyCode.ToString();

                //Determine key modifiers
                if (e.Alt && e.KeyCode != Keys.Menu)
                    key = "Alt + " + key;
                if (e.Shift && e.KeyCode != Keys.ShiftKey)
                    key = "Shift + " + key;
                if (e.Control && e.KeyCode != Keys.ControlKey)
                    key = "Ctrl + " + key;

                lblKey.Text = key;
            };
            keyPrompt.KeyDown += (sender, e) =>
            {
                if (e.Modifiers.Equals(Keys.Alt))
                {
                    e.Handled = true; //don't open the menu with alt
                }
            };
            keyPrompt.KeyUp += (sender, e) =>
            {
                keyPrompt.Close();
            };
            lblKey.MouseClick += (sender, e) =>
            {
                timer.Stop();
                if (e.Button == MouseButtons.Left)
                    key = "LeftClick";
                else if (e.Button == MouseButtons.Right)
                    key = "RightClick";
                else if (e.Button == MouseButtons.Middle)
                    key = "MiddleClick";
                string lastKey = lblKey.Text.Split(' ').Last();
                if (lastKey == "Ctrl" || lastKey == "Shift" || lastKey == "Alt")
                    key = lblKey.Text + " + " + key;
                lblKey.Text = key;
                keyPrompt.Close();
            };
            keyPrompt.MouseWheel += (sender, e) =>
            {
                timer.Stop();
                if (e.Delta < 0) //negative is down
                    key = "MouseWheelDown";
                else //positive is up
                    key = "MouseWheelUp";
                string lastKey = lblKey.Text.Split(' ').Last();
                if (lastKey == "Ctrl" || lastKey == "Shift" || lastKey == "Alt")
                    key = lblKey.Text + " + " + key;
                lblKey.Text = key;
                keyPrompt.Close();
            };
            keyPrompt.ShowDialog();
            return key;
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
                if (tbActivationButton.Text == tbDeactivationButton.Text)
                {
                    lblDeactivationInstructions.Enabled = false;
                    lblDeactivationInstructions.Visible = false;
                }
                else
                {
                    lblDeactivationInstructions.Enabled = true;
                    lblDeactivationInstructions.Visible = true;
                }
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
                if (tbActivationButton.Text == tbDeactivationButton.Text)
                {
                    lblDeactivationInstructions.Enabled = false;
                    lblDeactivationInstructions.Visible = false;
                }
                else
                {
                    lblDeactivationInstructions.Enabled = true;
                    lblDeactivationInstructions.Visible = true;
                }
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

        private void ActivationButton_Click(object sender, EventArgs e)
        {
            string key = GetKeyDialog();
            if (key != null)
            {
                ActivationKey = key;
                tbActivationButton.Text = key.ToString();
            }
        }

        private void DeactivationButton_Click(object sender, EventArgs e)
        {
            string key = GetKeyDialog();
            if (key != null)
            {
                DeactivationKey = key;
                tbDeactivationButton.Text = key.ToString();
            }
        }

        private void AutoclickButton_Click(object sender, EventArgs e)
        {
            string key = GetKeyDialog();
            if (key != null)
            {
                AutoclickKey = key;
                tbAutoclickButton.Text = key.ToString();
            }
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
    }
}
