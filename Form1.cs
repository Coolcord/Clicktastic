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
using System.Diagnostics;
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
        private const UInt32 MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const UInt32 MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const UInt32 MOUSEEVENTF_RIGHTUP = 0x0010;

        /*
        private const UInt32 MOD_ALT = 0x0001;
        private const UInt32 MOD_CONTROL = 0x0002;
        private const UInt32 MOD_NOREPEAT = 0x4000;
        private const UInt32 MOD_SHIFT = 0x0004;
        private const UInt32 MOD_WIN = 0x0008;
         */

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelMouseProc _proc2 = null;
        private static IntPtr _hookID2 = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);


        private static IntPtr SetHookMouse(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallbackMouse(
        int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 &&
                MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                if (Hold && AutoclickerEnabled && !SimulatingClicksOnHold)
                {
                    if (AutoclickerWaiting)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.green_circle;
                            lblAutoclickerRunning.Text = "Running";
                            lblAutoclickerRunning.ForeColor = Color.Lime;
                        }));
                        AutoclickerWaiting = false;
                    }
                }
            }
            if (nCode >= 0 &&
                MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                if (Hold && AutoclickerEnabled && !SimulatingClicksOnHold)
                {
                    if (!AutoclickerWaiting)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                            lblAutoclickerRunning.Text = "Waiting";
                            lblAutoclickerRunning.ForeColor = Color.Red;
                        }));
                        AutoclickerWaiting = true;
                    }
                }
            }
            return CallNextHookEx(_hookID2, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }



















        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private LowLevelKeyboardProc _proc = null;
        private static IntPtr _hookID = IntPtr.Zero;



        private static IntPtr SetHookKey(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallbackKey(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if ((Keys)vkCode == Keys.Oemtilde)
                {
                    if (Hold)
                    {
                        if (AutoclickerEnabled)
                        {
                            AutoclickerEnabled = false;
                            AutoclickerActivated = false;
                            pbAutoclickerEnabled.Image = Properties.Resources.red_circle;
                            lblAutoclickerEnabled.Text = "Disabled";
                            lblAutoclickerEnabled.ForeColor = Color.Red;
                            pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                            lblAutoclickerRunning.Text = "Waiting";
                            lblAutoclickerRunning.ForeColor = Color.Red;
                        }
                        else
                        {
                            AutoclickerEnabled = true;
                            AutoclickerActivated = true;
                            pbAutoclickerEnabled.Image = Properties.Resources.green_circle;
                            lblAutoclickerEnabled.Text = "Enabled";
                            lblAutoclickerEnabled.ForeColor = Color.Lime;
                            pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                            lblAutoclickerRunning.Text = "Waiting";
                            lblAutoclickerRunning.ForeColor = Color.Red;
                            AutoClick();
                        }
                    }
                    else
                    {
                        if (AutoclickerActivated)
                        {
                            AutoclickerActivated = false;
                            if (!AutoclickerWaiting)
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                                    lblAutoclickerRunning.Text = "Waiting";
                                    lblAutoclickerRunning.ForeColor = Color.Red;
                                }));
                                AutoclickerWaiting = true;
                            }
                        }
                        else
                        {
                            AutoclickerActivated = true;
                            if (AutoclickerWaiting)
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    pbAutoclickerRunning.Image = Properties.Resources.green_circle;
                                    lblAutoclickerRunning.Text = "Running";
                                    lblAutoclickerRunning.ForeColor = Color.Lime;
                                }));
                                AutoclickerWaiting = false;
                            }
                            AutoClick();
                        }
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }















        Boolean AutoclickerEnabled = true;
        Boolean AutoclickerActivated = false;
        Boolean AutoclickerWaiting = true;
        Boolean SimulatingClicksOnHold = false; //this might need to be a mutex fo some kind
        Boolean Random = false;
        Boolean Hold = false;
        string ActivationKey = "~";
        string DeactivationKey = "~";
        string AutoclickKey = "a";
        int turbo = 1;
        int MinDelay = 1;
        int MaxDelay = 1000;

        private string GetKeyDialog()
        {
            Form keyPrompt = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
            keyPrompt.StartPosition = FormStartPosition.CenterParent;
            keyPrompt.Width = 250;
            keyPrompt.Height = 100;
            keyPrompt.Text = "Clicktastic";
            keyPrompt.KeyPreview = true;
            Label lblKey = new Label() { Width = 250, Height = 65, ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter, Text = "Press any key or click here" };
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
                                lblKey.Text = "Press any key or click here";
                                break;
                            case 1:
                                lblKey.Text = "Press any key or click here.";
                                break;
                            case 2:
                                lblKey.Text = "Press any key or click here..";
                                break;
                            case 3:
                                lblKey.Text = "Press any key or click here...";
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
            _proc = HookCallbackKey;
            _proc2 = HookCallbackMouse;

            InitializeComponent();
            ddbProfile.SelectedIndex = 0;
            ddbActivationMode.SelectedIndex = 0;
            ddbSpeedMode.SelectedIndex = 0;
            ddbTurboMode.SelectedIndex = 0;
            MinDelay = (int)numMinDelay.Value;
            MaxDelay = (int)numMaxDelay.Value;

            _hookID = SetHookKey(_proc);
            _hookID2 = SetHookMouse(_proc2);

            setInstructions();
        }

        ~Form1()
        {
            UnhookWindowsHookEx(_hookID);
            UnhookWindowsHookEx(_hookID2);
        }

        private void PerformClick(System.Timers.Timer timer, Random randomNumber)
        {
            try
            {
                Boolean KeyHeld = ((Control.MouseButtons & MouseButtons.Left) != 0);
                if (!Hold || (AutoclickerEnabled && KeyHeld))
                {
                    if (!AutoclickerActivated) //stop the autoclicker
                    {
                        /*
                        if (!AutoclickerWaiting)
                        {
                            AutoclickerWaiting = true;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                                lblAutoclickerRunning.Text = "Waiting";
                                lblAutoclickerRunning.ForeColor = Color.Red;
                            }));
                        }
                         */
                        timer.Stop();
                        timer.Dispose();
                        return;
                    }
                    /*
                    if (AutoclickerWaiting)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.green_circle;
                            lblAutoclickerRunning.Text = "Running";
                            lblAutoclickerRunning.ForeColor = Color.Lime;
                        }));
                        AutoclickerWaiting = false;
                    }
                     */
                    SimulatingClicksOnHold = true;
                    if (Hold)
                    {
                        for (int i = 0; i < turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);//make left button up
                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);//make left button down
                            //SendKeys.SendWait("{ENTER}"); //press key or click
                        }
                    }
                    else //trigger
                    {
                        for (int i = 0; i < turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);//make left button down
                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);//make left button up
                            //SendKeys.SendWait("{ENTER}"); //press key or click
                        }
                    }
                    SimulatingClicksOnHold = false;
                    if (Random)
                        timer.Interval = randomNumber.Next(MinDelay, MaxDelay);
                }
                else
                {
                    /*
                    if (!AutoclickerWaiting && !SimulatingClicksOnHold)
                    {
                        AutoclickerWaiting = true;
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                            lblAutoclickerRunning.Text = "Waiting";
                            lblAutoclickerRunning.ForeColor = Color.Red;
                        }));
                    }
                     */
                }
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
                int randomNumber = randomGen.Next(MinDelay, MaxDelay);
                System.Timers.Timer timer1 = new System.Timers.Timer(randomNumber);
                timer1.AutoReset = true;
                timer1.Enabled = true;
                timer1.Elapsed += (sender, e) => PerformClick(timer1, randomGen);
            }
            else
            {
                Random randomGen = null;
                System.Timers.Timer timer1 = new System.Timers.Timer(MinDelay);
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
                pbAutoclickerEnabled.Image = Properties.Resources.red_circle;
                lblAutoclickerEnabled.Text = "Disabled";
                lblAutoclickerEnabled.ForeColor = Color.Red;
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
                pbAutoclickerEnabled.Image = Properties.Resources.red_circle;
                lblAutoclickerEnabled.Text = "Disabled";
                lblAutoclickerEnabled.ForeColor = Color.Red;
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
                lblSpeedInstructions.Text = "Autoclicker will run with " + numMinDelay.Value + " ms delay";
            }
            else
            {
                if (numMinDelay.Value == numMaxDelay.Value)
                    lblSpeedInstructions.Text = "Autoclicker will run with " + numMinDelay.Value + " ms delay";
                else
                    lblSpeedInstructions.Text = "Delay on Autoclicker will be between " + numMinDelay.Value + " ms and " + numMaxDelay.Value + " ms";
            }
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
                lblMaxDelay.Enabled = false;
                numMaxDelay.Enabled = false;
                lblMaxCPS.Visible = false;
                lblMaxDelay.Visible = false;
                numMaxDelay.Visible = false;
                numMaxDelay.Value = numMinDelay.Value;
                lblMinDelay.Text = "Delay Time:";
                Random = false;
            }
            else //random speed
            {
                lblMaxCPS.Enabled = true;
                lblMaxDelay.Enabled = true;
                numMaxDelay.Enabled = true;
                lblMaxCPS.Visible = true;
                lblMaxDelay.Visible = true;
                numMaxDelay.Visible = true;
                numMaxDelay.Value = numMinDelay.Value;
                lblMinDelay.Text = "Minimum Delay Time:";
                Random = true;
            }
            setInstructions();
        }

        private void numMinDelay_ValueChanged(object sender, EventArgs e)
        {
            if (numMaxDelay.Value < numMinDelay.Value)
                numMaxDelay.Value = numMinDelay.Value;
            MinDelay = (int)numMinDelay.Value;
            MaxDelay = (int)numMaxDelay.Value;
            setInstructions();
        }

        private void numMaxDelay_ValueChanged(object sender, EventArgs e)
        {
            if (numMinDelay.Value > numMaxDelay.Value)
                numMinDelay.Value = numMaxDelay.Value;
            MinDelay = (int)numMinDelay.Value;
            MaxDelay = (int)numMaxDelay.Value;
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
            if (ddbActivationMode.SelectedIndex == 0)
            {
                Hold = false;
                AutoclickerEnabled = true;
            }
            else
            {
                Hold = true;
                AutoclickerEnabled = false;
            }
            setInstructions();
        }

        private void ddbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            setInstructions();
        }

        private void ddbTurboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            turbo = ddbTurboMode.SelectedIndex + 1;
        }

        private void PlayMarioTheme()
        {
            Console.Beep(659, 125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(523, 125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(784, 125);
            Thread.Sleep(375);
            Console.Beep(392, 125);
        }
    }
}
