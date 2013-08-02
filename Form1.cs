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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Clicktastic
{
    public partial class Form1 : Form
    {
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);

        Boolean AutoclickerEnabled = false;
        Boolean AutoclickerActivated = false;
        Boolean Random = false;
        string ActivationKey = "~";
        string DeactivationKey = "~";
        string AutoclickKey = "a";
        int turbo = 1;
        int MinSpeed = 1;
        int MaxSpeed = 1000;

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
                else
                    key = e.Button.ToString();
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
            ddbTurboMode.SelectedIndex = 0;
            MinSpeed = (int)numMinSpeed.Value;
            MaxSpeed = (int)numMaxSpeed.Value;
            setInstructions();
        }

        private void PerformClick(System.Timers.Timer timer, Random randomNumber)
        {
            try
            {
                if (AutoclickerActivated == false) //stop the autoclicker
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }
                for (int i = 0; i < turbo; i++)
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);//make left button down
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);//make left button up
                    //SendKeys.SendWait("{ENTER}"); //press key or click
                }
                if (Random)
                    timer.Interval = randomNumber.Next(MinSpeed, MaxSpeed);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void AutoClick()
        {
            if (Random)
            {
                Random randomGen = new Random();
                int randomNumber = randomGen.Next(MinSpeed, MaxSpeed);
                System.Timers.Timer timer1 = new System.Timers.Timer(randomNumber);
                timer1.AutoReset = true;
                timer1.Enabled = true;
                timer1.Elapsed += (sender, e) => PerformClick(timer1, randomGen);
            }
            else
            {
                Random randomGen = null;
                System.Timers.Timer timer1 = new System.Timers.Timer(MinSpeed);
                timer1.AutoReset = true;
                timer1.Enabled = true;
                timer1.Elapsed += (sender, e) => PerformClick(timer1, randomGen);
            }

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
                lblMinSpeed.Text = "Sleep Time:";
                Random = false;
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
                lblMinSpeed.Text = "Minimum Sleep Time:";
                Random = true;
            }
            setInstructions();
        }

        private void numMinSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (numMaxSpeed.Value < numMinSpeed.Value)
                numMaxSpeed.Value = numMinSpeed.Value;
            MinSpeed = (int)numMinSpeed.Value;
            MaxSpeed = (int)numMaxSpeed.Value;
            setInstructions();
        }

        private void numMaxSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (numMinSpeed.Value > numMaxSpeed.Value)
                numMinSpeed.Value = numMaxSpeed.Value;
            MinSpeed = (int)numMinSpeed.Value;
            MaxSpeed = (int)numMaxSpeed.Value;
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
                Thread.Sleep(1000); //debug code
                AutoClick();
            }
        }

        private void ddbTurboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            turbo = ddbTurboMode.SelectedIndex + 1;
        }
    }
}
