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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Clicktastic
{
    public partial class Clicktastic : Form
    {
        public static string currentDirectory = Directory.GetCurrentDirectory() + "\\Profiles";

        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        private const UInt32 MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const UInt32 MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const UInt32 MOUSEEVENTF_RIGHTUP = 0x0010;
        private const UInt32 MOUSEEVENTF_WHEEL = 0x0800;

        /*
        private const UInt32 MOD_ALT = 0x0001;
        private const UInt32 MOD_CONTROL = 0x0002;
        private const UInt32 MOD_NOREPEAT = 0x4000;
        private const UInt32 MOD_SHIFT = 0x0004;
        private const UInt32 MOD_WIN = 0x0008;
         */

        Boolean AutoclickerEnabled = true;
        Boolean AutoclickerActivated = false;
        Boolean AutoclickerWaiting = true;
        Boolean SimulatingClicksOnHold = false;
        Boolean Startup = true;
        Boolean Loading = false;
        int RetryAttempts = 0;
        KeyStringConverter keyStringConverter = new KeyStringConverter();
        public ProfileData profileData = new ProfileData();
        Profile profile = new Profile();
        string previousProfile = "Default";

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct ProfileData
        {
            public KEYCOMBO ActivationKey;
            public KEYCOMBO DeactivationKey;
            public KEYCOMBO AutoclickKey;
            public Boolean Random;
            public Boolean Hold;
            public Boolean pressEnter;
            public Boolean useDeactivationKey;
            public Boolean suppressHotkeys;
            public int turbo;
            public int MinDelay;
            public int MaxDelay;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYCOMBO
        {
            public Boolean valid;
            public Boolean isKeyboard;
            public Keys modifierKeys;
            public Keys key;
            public string keyString;
            public string cmd;
            public uint mouseButton;
            public int wheel;
        }

        //Mouse Output
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, uint dwExtraInf);

        //Keyboard Hook
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

        //Mouse Hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

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
        private struct MSLLHOOKSTRUCT
        {
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x0100;
        private LowLevelKeyboardProc _procKey = null;
        private static IntPtr _hookIDKey = IntPtr.Zero;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelMouseProc _procMouse = null;
        private static IntPtr _hookIDMouse = IntPtr.Zero;

        private static IntPtr SetHookKey(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                //return SetWindowsHookEx(WH_KEYBOARD_LL, hookProcDelegate,
                    //GetModuleHandle(curModule.ModuleName), 0);
                //return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    //GetModuleHandle(curModule.ModuleName), 0);
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private const Int32 dummy = 0x0000;

        private IntPtr HookCallbackKey(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN && tcClicktastic.SelectedIndex == 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (((Keys)vkCode == profileData.ActivationKey.key && profileData.ActivationKey.modifierKeys == Control.ModifierKeys) ||
                    ((Keys)vkCode == profileData.DeactivationKey.key && profileData.DeactivationKey.modifierKeys == Control.ModifierKeys))
                {
                    ToggleAutoClicker((Keys)vkCode);
                    if (cbSuppressHotkeys.Checked)
                    {
                        return (IntPtr)1; //dummy value
                    }
                }
            }
            else if (nCode >= 0 && tcClicktastic.SelectedIndex == 0 &&
                ((profileData.ActivationKey.modifierKeys == Control.ModifierKeys &&
                profileData.ActivationKey.key == Keys.None) ||
                (profileData.DeactivationKey.modifierKeys == Control.ModifierKeys &&
                profileData.DeactivationKey.key == Keys.None))) //Ctrl, Shift, or Alt only
            {
                int vkCode = Marshal.ReadInt32(lParam);
                ToggleAutoClicker(Keys.None);
                if (cbSuppressHotkeys.Checked)
                {
                    return (IntPtr)1; //dummy value
                }
            }

            return CallNextHookEx(_hookIDKey, nCode, wParam, lParam);
        }

        private void ToggleAutoClicker(Keys key)
        {
            if (profileData.Hold)
            {
                if (AutoclickerEnabled && key == profileData.DeactivationKey.key)
                {
                    AutoclickerEnabled = false;
                    AutoclickerActivated = false;
                    this.Invoke(new MethodInvoker(() =>
                    {
                        pbAutoclickerEnabled.Image = Properties.Resources.red_circle;
                        lblAutoclickerEnabled.Text = "Disabled";
                        lblAutoclickerEnabled.ForeColor = Color.Red;
                        pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                        lblAutoclickerRunning.Text = "Waiting";
                        lblAutoclickerRunning.ForeColor = Color.Red;
                    }));
                    AutoclickerWaiting = true;
                    if (ddbTurboMode.SelectedIndex == 10)
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Stop.wav");
                        sound.Play();  //Plays the sound in a new thread
                    }
                }
                else if (!AutoclickerEnabled && key == profileData.ActivationKey.key)
                {
                    AutoclickerEnabled = true;
                    AutoclickerActivated = true;
                    this.Invoke(new MethodInvoker(() =>
                    {
                        pbAutoclickerEnabled.Image = Properties.Resources.green_circle;
                        lblAutoclickerEnabled.Text = "Enabled";
                        lblAutoclickerEnabled.ForeColor = Color.Lime;
                    }));
                    Boolean ButtonHeld = ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left);
                    if (ButtonHeld && AutoclickerWaiting)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.green_circle;
                            lblAutoclickerRunning.Text = "Running";
                            lblAutoclickerRunning.ForeColor = Color.Lime;
                        }));
                        AutoclickerWaiting = false;
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                            lblAutoclickerRunning.Text = "Waiting";
                            lblAutoclickerRunning.ForeColor = Color.Red;
                        }));
                        AutoclickerWaiting = true;
                    }
                    if (ddbTurboMode.SelectedIndex == 10)
                    {
                        //System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Start.wav");
                        //sound.Play();  //Plays the sound in a new thread
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Loop.wav");
                        sound.PlayLooping();
                    }
                    AutoClick();
                }
            }
            else
            {
                if (AutoclickerActivated && key == profileData.DeactivationKey.key)
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
                        if (ddbTurboMode.SelectedIndex == 10)
                        {
                            System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Stop.wav");
                            sound.Play();  //Plays the sound in a new thread
                        }
                    }
                }
                else if (!AutoclickerActivated && key == profileData.ActivationKey.key)
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
                    if (ddbTurboMode.SelectedIndex == 10)
                    {
                        //System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Start.wav");
                        //sound.Play();  //Plays the sound in a new thread
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Loop.wav");
                        sound.PlayLooping();
                    }
                    AutoClick();
                }
            }
        }

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
                if (profileData.Hold && AutoclickerEnabled && !SimulatingClicksOnHold)
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
                if (profileData.Hold && AutoclickerEnabled && !SimulatingClicksOnHold)
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
            return CallNextHookEx(_hookIDMouse, nCode, wParam, lParam);
        }

        private KEYCOMBO GetKeyDialog(string message)
        {
            Form keyPrompt = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
            keyPrompt.StartPosition = FormStartPosition.CenterParent;
            keyPrompt.Width = 250;
            keyPrompt.Height = 100;
            keyPrompt.Text = "Clicktastic";
            keyPrompt.KeyPreview = true;
            Label lblKey = new Label() { Width = 250, Height = 65, ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter, Text = message };
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
                                lblKey.Text = message;
                                break;
                            case 1:
                                lblKey.Text = message + ".";
                                break;
                            case 2:
                                lblKey.Text = message + "..";
                                break;
                            case 3:
                                lblKey.Text = message + "...";
                                break;
                        }
                    }));
                    textState = (textState + 1) % 4;
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            };
            KEYCOMBO key = new KEYCOMBO();
            Keys lastKey = Keys.None;
            key.valid = false;
            string strKey = null;
            keyPrompt.PreviewKeyDown += (sender, e) =>
            {
                timer.Stop();

                //Determine the key pressed
                 strKey = keyStringConverter.KeyToString(e.KeyCode);

                //Determine key modifiers
                if (e.Alt && e.KeyCode != Keys.Menu)
                    strKey = "Alt + " + strKey;
                if (e.Shift && e.KeyCode != Keys.ShiftKey)
                    strKey = "Shift + " + strKey;
                if (e.Control && e.KeyCode != Keys.ControlKey)
                    strKey = "Ctrl + " + strKey;

                lblKey.Text = strKey;
                lastKey = e.KeyCode;
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
                    strKey = "LeftClick";
                else if (e.Button == MouseButtons.Right)
                    strKey = "RightClick";
                else if (e.Button == MouseButtons.Middle)
                    strKey = "MiddleClick";
                else
                    return; //button not recognized
                string strLastKey = lblKey.Text.Split(' ').Last();
                if (strLastKey == "Ctrl" || strLastKey == "Shift" || strLastKey == "Alt")
                    strKey = lblKey.Text + " + " + strKey;
                lblKey.Text = strKey;
                lastKey = Keys.None;
                keyPrompt.Close();
            };
            keyPrompt.MouseWheel += (sender, e) =>
            {
                timer.Stop();
                if (e.Delta < 0) //negative is down
                    strKey = "MouseWheelDown";
                else //positive is up
                    strKey = "MouseWheelUp";
                string strLastKey = lblKey.Text.Split(' ').Last();
                if (strLastKey == "Ctrl" || strLastKey == "Shift" || strLastKey == "Alt")
                    strKey = lblKey.Text + " + " + strKey;
                lblKey.Text = strKey;
                lastKey = Keys.None;
                keyPrompt.Close();
            };
            keyPrompt.ShowDialog();
            keyPrompt.Dispose();
            lblKey.Dispose();
            key = ParseKEYCOMBO(strKey, lastKey);
            return key;
        }

        private KEYCOMBO ParseKEYCOMBO(string strKey, Keys lastKeyCode)
        {
            KEYCOMBO key = new KEYCOMBO();
            if (strKey == null)
            {
                key.valid = false;
                return key;
            }
            key.keyString = strKey;
            Boolean ctrl = false;
            Boolean shift = false;
            Boolean alt = false;
            key.modifierKeys = Keys.None;
            key.isKeyboard = false;
            key.mouseButton = 0;
            key.wheel = 0;
            key.key = Keys.None;
            string[] buttons = strKey.Split(' ');
            string previous = null;
            string lastKey = buttons.Last();
            foreach(string button in buttons)
            {
                if (button == "(~)" || (button == "+" && (button == previous || lastKey != button)))
                {
                    previous = button;
                    continue;
                }
                if (button != lastKey)
                {
                    if (button == "Ctrl")
                    {
                        ctrl = true;
                        previous = button;
                        continue;
                    }
                    else if (button == "Shift")
                    {
                        shift = true;
                        previous = button;
                        continue;
                    }
                    else if (button == "Alt")
                    {
                        alt = true;
                        previous = button;
                        continue;
                    }
                }
                if (button == "LeftClick") key.mouseButton = MOUSEEVENTF_LEFTDOWN;
                else if (button == "RightClick") key.mouseButton = MOUSEEVENTF_RIGHTDOWN;
                else if (button == "MiddleClick") key.mouseButton = MOUSEEVENTF_MIDDLEDOWN;
                else if (button == "MouseWheelDown") key.wheel = -120;
                else if (button == "MouseWheelUp") key.wheel = 120;
                else
                {
                    key.isKeyboard = true;

                    if (button == "Ctrl") ctrl = true;
                    else if (button == "Shift") shift = true;
                    else if (button == "Alt") alt = true;

                    if (!isKeyAcceptable(lastKeyCode))
                    {
                        Console.WriteLine("Triggered");
                        key.key = Keys.None; //allow only Ctrl, Shift, or Alt by themselves
                    }
                    else
                        key.key = lastKeyCode; //store the key code
                }
                previous = button;
            }
            if (ctrl)
                key.modifierKeys = key.modifierKeys | Keys.Control;
            if (shift)
                key.modifierKeys = key.modifierKeys | Keys.Shift;
            if (alt)
                key.modifierKeys = key.modifierKeys | Keys.Alt;
            key.cmd = keyStringConverter.KeyToCmd(key.key, key.modifierKeys, profileData.pressEnter, profileData.turbo);
            key.valid = true;
            return key;
        }

        public Clicktastic()
        {
            InitializeComponent();

            _procKey = HookCallbackKey;
            _procMouse = HookCallbackMouse;
            _hookIDKey = SetHookKey(_procKey);
            _hookIDMouse = SetHookMouse(_procMouse);

            //CreateDefaultProfile();

            if (!Directory.Exists(currentDirectory))
            {
                try
                {
                    Directory.CreateDirectory(currentDirectory);
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            previousProfile = Properties.Settings.Default.DefaultProfile;

            RetryAttempts = 0;
            try
            {
                foreach (string file in Directory.GetFiles(currentDirectory, "*.clk"))
                {
                    ddbProfile.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
                ddbProfile.SelectedItem = previousProfile;
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            Boolean loaded = false;
            Loading = true;
            while (!loaded)
            {
                loaded = AttemptLoad();
                //CreateDefaultProfile();
                //loaded = true;
            }
            Startup = false;
            setInstructions();
        }

        ~Clicktastic()
        {
            UnhookWindowsHookEx(_hookIDKey);
            UnhookWindowsHookEx(_hookIDMouse);
        }

        private void PlayKeyCombo(KEYCOMBO key)
        {
            if (!key.valid)
                return; //key combo is invalid, so don't try running it
            if (profileData.Hold)
            {
                if (key.isKeyboard) //keyboard key
                {
                    return; //keyboard keys are not supported in hold mode
                }
                else //is mouse
                {
                    if (key.mouseButton == MOUSEEVENTF_LEFTDOWN) //left mouse click
                    {
                        for (int i = 0; i < profileData.turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                        }
                    }
                    else if (key.mouseButton == MOUSEEVENTF_RIGHTDOWN) //right mouse click
                    {
                        for (int i = 0; i < profileData.turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                        }
                    }
                    else if (key.mouseButton == MOUSEEVENTF_MIDDLEDOWN) //middle mouse click
                    {
                        for (int i = 0; i < profileData.turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
                        }
                    }
                    else //scroll
                    {
                        return; //mouse scroll is not supported in hold mode
                    }
                }
            }
            else //toggle
            {
                if (key.isKeyboard) //keyboard key
                {
                    try
                    {
                        SendKeys.SendWait(profileData.AutoclickKey.cmd);
                    }
                    catch (Exception ex) { Console.WriteLine(ex); }
                }
                else //is mouse
                {
                    if (key.mouseButton == MOUSEEVENTF_LEFTDOWN) //left mouse click
                    {
                        for (int i = 0; i < profileData.turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        }
                    }
                    else if (key.mouseButton == MOUSEEVENTF_RIGHTDOWN) //right mouse click
                    {
                        for (int i = 0; i < profileData.turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                        }
                    }
                    else if (key.mouseButton == MOUSEEVENTF_MIDDLEDOWN) //middle mouse click
                    {
                        for (int i = 0; i < profileData.turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
                        }
                    }
                    else //scroll
                    {
                        for (int i = 0; i < profileData.turbo; i++)
                        {
                            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, key.wheel, 0);
                        }
                    }
                }
            }
        }

        private void PerformClick(System.Timers.Timer timer, Random randomNumber)
        {
            try
            {
                Boolean KeyHeld = ((Control.MouseButtons & MouseButtons.Left) != 0);
                if (!profileData.Hold || (AutoclickerEnabled && KeyHeld))
                {
                    if (!AutoclickerActivated) //stop the autoclicker
                    {
                        timer.Stop();
                        timer.Dispose();
                        return;
                    }
                    SimulatingClicksOnHold = true;
                    PlayKeyCombo(profileData.AutoclickKey);
                    SimulatingClicksOnHold = false;
                    if (profileData.Random && randomNumber != null)
                        timer.Interval = randomNumber.Next(profileData.MinDelay, profileData.MaxDelay);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            if (!AutoclickerActivated) //stop the autoclicker
            {
                timer.Stop();
                timer.Dispose();
                return;
            }
        }

        private void AutoClick()
        {
            if (profileData.Random)
            {
                Random randomGen = new Random();
                int randomNumber = randomGen.Next(profileData.MinDelay, profileData.MaxDelay);
                System.Timers.Timer timer1 = new System.Timers.Timer(randomNumber);
                timer1.AutoReset = true;
                timer1.Enabled = true;
                timer1.Elapsed += (sender, e) => PerformClick(timer1, randomGen);
            }
            else
            {
                Random randomGen = null;
                System.Timers.Timer timer1 = new System.Timers.Timer(profileData.MinDelay);
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
                profileData.useDeactivationKey = true;
            }
            else
            {
                lblDeactivationButton.Enabled = false;
                profileData.DeactivationKey = profileData.ActivationKey;
                tbDeactivationButton.Text = tbActivationButton.Text;
                tbDeactivationButton.Enabled = false;
                btnDeactivationButton.Enabled = false;
                profileData.useDeactivationKey = false;
            }
            setInstructions();
        }

        private Boolean isKeyAcceptable(Keys key)
        {
            switch (key)
            {
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.Alt:
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.None:
                    return false; //these are not accepted
                default:
                    return true; //anything else is accepted
            }
        }

        private Boolean isActivationSettingsValid(KEYCOMBO key)
        {
            if (!key.valid)
                return false;
            else if (key.isKeyboard && key.key == Keys.None && key.modifierKeys == Keys.None)
            {
                MessageBox.Show("That key is not supported!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!key.isKeyboard && (key.mouseButton != MOUSEEVENTF_LEFTDOWN &&
                key.mouseButton != MOUSEEVENTF_RIGHTDOWN &&
                key.mouseButton != MOUSEEVENTF_MIDDLEDOWN &&
                key.wheel == 0))
            {
                MessageBox.Show("That button is not supported!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!key.isKeyboard)
            {
                MessageBox.Show("Mouse buttons cannot be used as activator hotkeys!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                return true;
        }

        private Boolean isAutoclickKeySettingsValid(KEYCOMBO key)
        {
            if (!key.valid)
                return false;
            else if (key.isKeyboard && !isKeyAcceptable(key.key))
            {
                MessageBox.Show("That key is not supported!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!key.isKeyboard && (key.mouseButton != MOUSEEVENTF_LEFTDOWN &&
                key.mouseButton != MOUSEEVENTF_RIGHTDOWN &&
                key.mouseButton != MOUSEEVENTF_MIDDLEDOWN &&
                key.wheel == 0))
            {
                MessageBox.Show("That button is not supported!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!key.isKeyboard && key.modifierKeys != Keys.None)
            {
                MessageBox.Show("Keyboard combos are not supported with the mouse!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (key.isKeyboard)
            {
                /*
                if (profileData.Hold && ddbTurboMode.SelectedIndex != 0) //hold and turbo are on
                {
                    DialogResult result = DialogResult.Yes;
                    result = MessageBox.Show("Keyboard keys are not supported in hold mode and turbo mode! Would you like to switch back to toggle mode and turn off turbo mode?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (result == DialogResult.Yes)
                    {
                        ddbActivationMode.SelectedIndex = 0;
                        ddbTurboMode.SelectedIndex = 0;
                    }
                    else
                        return false;
                }
                else if (ddbTurboMode.SelectedIndex != 0) //turbo is on
                {
                    DialogResult result = DialogResult.Yes;
                    result = MessageBox.Show("Keyboard keys are not supported in turbo mode!\nWould you like to turn off turbo mode?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (result == DialogResult.Yes)
                    {
                        ddbTurboMode.SelectedIndex = 0;
                    }
                    else
                        return false;
                }
                 * */
                if (profileData.Hold) //hold is on
                {
                    DialogResult result = DialogResult.Yes;
                    result = MessageBox.Show("Keyboard keys are not supported in hold mode!\nWould you like to switch back to toggle mode?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (result == DialogResult.Yes)
                    {
                        ddbActivationMode.SelectedIndex = 0;
                    }
                    else
                        return false;
                }
            }
            else if (profileData.Hold && key.wheel != 0)
            {
                DialogResult result = DialogResult.Yes;
                result = MessageBox.Show("Mouse wheel is not supported in hold mode!\nWould you like to switch back to toggle mode?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    ddbActivationMode.SelectedIndex = 0;
                }
                else
                    return false;
            }
            return true;
        }

        private void SetTurbo()
        {
            /*
            if (ddbTurboMode.SelectedIndex == 0)
                return; //turbo is off which is always fine
            if (profileData.AutoclickKey.isKeyboard)
            {
                MessageBox.Show("Keyboard keys are not supported in turbo mode!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddbTurboMode.SelectedIndex = 0; //fall back to trigger mode
            }
             * */
            if (profileData.pressEnter && (ddbTurboMode.SelectedIndex + 1) > 1)
            {
                DialogResult result = DialogResult.Yes;
                result = MessageBox.Show("Pressing enter after autoclick is not supported in turbo mode! Would you like disable pressing enter after autoclick?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result != DialogResult.Yes)
                {
                    ddbTurboMode.SelectedIndex = 0;
                    return;
                }
            }
            profileData.turbo = ddbTurboMode.SelectedIndex + 1;
            if (profileData.AutoclickKey.isKeyboard)
            {
                if (profileData.turbo > 1)
                {
                    cbEnter.Checked = false;
                    cbEnter.Enabled = false;
                    profileData.pressEnter = false; //make sure that it is saved
                    profileData.turbo = (profileData.turbo - 1) * 6;
                }
                else
                    cbEnter.Enabled = true;
            }
            else
            {
                if (profileData.turbo > 1)
                {
                    //cbEnter.Checked = false;
                    //cbEnter.Enabled = false;
                    //profileData.pressEnter = false; //make sure that it is saved
                    profileData.turbo = (profileData.turbo - 1) * 3;
                }
                //else if (profileData.AutoclickKey.isKeyboard)
                    //cbEnter.Enabled = true;
            }
            //Fix the stored autoclick key command
            profileData.AutoclickKey.cmd = keyStringConverter.KeyToCmd(profileData.AutoclickKey.key, profileData.AutoclickKey.modifierKeys, profileData.pressEnter, profileData.turbo);
        }

        private void CheckActivationModeSettings()
        {
            if (ddbActivationMode.SelectedIndex == 0)
                return; //trigger mode is always fine
            if (profileData.AutoclickKey.isKeyboard)
            {
                MessageBox.Show("Keyboard keys are not supported in hold mode!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddbActivationMode.SelectedIndex = 0; //fall back to trigger mode
            }
            if (profileData.AutoclickKey.wheel != 0)
            {
                MessageBox.Show("Mouse wheel is not supported in hold mode!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddbActivationMode.SelectedIndex = 0; //fall back to trigger mode
            }
            return;
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
            KEYCOMBO key = GetKeyDialog("Press any key");
            if (isActivationSettingsValid(key))
            {
                if (key.keyString == tbAutoclickButton.Text)
                {
                    MessageBox.Show("Autoclick button and Activator Hotkeys cannot be the same!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                profileData.ActivationKey = key;
                if (!cbUseDeactivationButton.Checked)
                    profileData.DeactivationKey = key;
                tbActivationButton.Text = key.keyString;
            }
        }

        private void DeactivationButton_Click(object sender, EventArgs e)
        {
            KEYCOMBO key = GetKeyDialog("Press any key");
            if (isActivationSettingsValid(key))
            {
                if (key.keyString == tbAutoclickButton.Text)
                {
                    MessageBox.Show("Autoclick button and Activator Hotkeys cannot be the same!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                profileData.DeactivationKey = key;
                tbDeactivationButton.Text = key.keyString;
            }
        }

        private void AutoclickButton_Click(object sender, EventArgs e)
        {
            KEYCOMBO key = GetKeyDialog("Press any key or click here");
            if (isAutoclickKeySettingsValid(key))
            {
                if (key.keyString == tbActivationButton.Text || key.keyString == tbDeactivationButton.Text)
                {
                    MessageBox.Show("Autoclick button and Activator Hotkeys cannot be the same!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                profileData.AutoclickKey = key;
                tbAutoclickButton.Text = key.keyString;
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
                profileData.MaxDelay = (int)numMaxDelay.Value;
                lblMinDelay.Text = "Delay Time:";
                profileData.Random = false;
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
                profileData.MaxDelay = (int)numMaxDelay.Value;
                lblMinDelay.Text = "Minimum Delay Time:";
                profileData.Random = true;
            }
            setInstructions();
        }

        private void numMinDelay_ValueChanged(object sender, EventArgs e)
        {
            if (numMaxDelay.Value < numMinDelay.Value)
                numMaxDelay.Value = numMinDelay.Value;
            profileData.MinDelay = (int)numMinDelay.Value;
            profileData.MaxDelay = (int)numMaxDelay.Value;
            setInstructions();
        }

        private void numMaxDelay_ValueChanged(object sender, EventArgs e)
        {
            if (numMinDelay.Value > numMaxDelay.Value)
                numMinDelay.Value = numMaxDelay.Value;
            profileData.MinDelay = (int)numMinDelay.Value;
            profileData.MaxDelay = (int)numMaxDelay.Value;
            setInstructions();
        }

        private void tbDeactivationButton_TextChanged(object sender, EventArgs e)
        {
            setInstructions();
        }

        private void tbAutoclickButton_TextChanged(object sender, EventArgs e)
        {
            if (profileData.AutoclickKey.isKeyboard)
            {
                lblActivationMode.Enabled = false;
                ddbActivationMode.Enabled = false;
            }
            else
            {
                lblActivationMode.Enabled = true;
                ddbActivationMode.Enabled = true;
                if (cbEnter.Enabled)
                {
                    cbEnter.Checked = false;
                    profileData.pressEnter = false; //make sure it saves
                }
                cbEnter.Enabled = false;
            }
            SetTurbo();
            setInstructions();
        }

        private void ddbActivationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckActivationModeSettings();
            if (ddbActivationMode.SelectedIndex == 0)
            {
                profileData.Hold = false;
                AutoclickerEnabled = true;
            }
            else
            {
                profileData.Hold = true;
                AutoclickerEnabled = false;
            }
            setInstructions();
        }

        private void UpdatePreferences()
        {
            if (profileData.Hold)
                ddbActivationMode.SelectedIndex = 1;
            else
                ddbActivationMode.SelectedIndex = 0;
            int MinDelay = profileData.MinDelay; //store the min delay to prevent losing the value
            int MaxDelay = profileData.MaxDelay; //store the max delay to prevent losing the value
            if (profileData.Random)
                ddbSpeedMode.SelectedIndex = 1;
            else
                ddbSpeedMode.SelectedIndex = 0;

            if (profileData.AutoclickKey.isKeyboard && profileData.turbo > 1)
                ddbTurboMode.SelectedIndex = (profileData.turbo / 6);
            else if (!profileData.AutoclickKey.isKeyboard && profileData.turbo > 1)
                ddbTurboMode.SelectedIndex = (profileData.turbo / 3);
            else
                ddbTurboMode.SelectedIndex = profileData.turbo - 1;

            numMinDelay.Value = MinDelay;
            numMaxDelay.Value = MaxDelay;

            cbUseDeactivationButton.Checked = profileData.useDeactivationKey;
            cbEnter.Checked = profileData.pressEnter;
            cbSuppressHotkeys.Checked = profileData.suppressHotkeys;

            tbActivationButton.Text = profileData.ActivationKey.keyString;
            tbDeactivationButton.Text = profileData.DeactivationKey.keyString;
            tbAutoclickButton.Text = profileData.AutoclickKey.keyString;
        }

        private void ddbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loading)
            {
                Boolean loaded = false;
                while (!loaded)
                {
                    loaded = AttemptLoad();
                }
            }
            else
                Loading = false;
        }

        private Boolean AttemptLoad()
        {
            ProfileData loadProfileData = new ProfileData();
            if (profile.Load(ddbProfile.Text, ref loadProfileData))
            {
                profileData = loadProfileData;
                UpdatePreferences();
                previousProfile = ddbProfile.Text;
                RetryAttempts = 0;
                Properties.Settings.Default.DefaultProfile = ddbProfile.Text;
                Properties.Settings.Default.Save();
                setInstructions();
                return true;
            }
            else
            {
                if (!Directory.Exists(currentDirectory)) //make sure the profile folder exists
                {
                    try
                    {
                        Directory.CreateDirectory(currentDirectory);
                    }
                    catch (Exception ex) { Console.WriteLine(ex); }
                }
                if (!Startup && RetryAttempts == 0) //only show the error message one time
                    MessageBox.Show("Unable to load profile!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (RetryAttempts == 0 && ddbProfile.Items.Contains(previousProfile))
                {
                    RetryAttempts++;
                    ddbProfile.SelectedItem = previousProfile; //revert back to previous profile
                }
                else if (RetryAttempts < 2 && ddbProfile.Items.Count > 0)
                {
                    RetryAttempts++;
                    ddbProfile.SelectedIndex = 0; //attempt to revert to the first profile in the list
                }
                else if (RetryAttempts < 3 && ddbProfile.Items.Contains("Default"))
                {
                    RetryAttempts++;
                    ddbProfile.SelectedItem = "Default"; //attempt to fall back to the default profile
                }
                else //give up
                {
                    RetryAttempts = 0;
                    CreateDefaultProfile(); //recreate default profile
                    return true;
                }
                return false;
            }
        }

        private void ddbTurboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTurbo();
            if (ddbTurboMode.SelectedIndex == 10 && !Startup)
            {
                System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Prepare Ship.wav");
                sound.Play();  //Plays the sound in a new thread
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            Form aboutForm = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
            aboutForm.StartPosition = FormStartPosition.CenterParent;
            aboutForm.Width = 400;
            aboutForm.Height = 200;
            aboutForm.Text = "About Clicktastic";
            aboutForm.Icon = Properties.Resources.clicktastic;

            //Get the version number
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Label aboutText = new Label()
            {
                Width = 400,
                Height = 130,
                Location = new Point(0, 0),
                ImageAlign = ContentAlignment.MiddleCenter,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Clicktastic v" + fileVersionInfo.ProductMajorPart + "." + fileVersionInfo.ProductMinorPart + "." + fileVersionInfo.ProductBuildPart + "\n\n" +
                    "Mass Click Mouse Buttons or\n" + "Mass Press Keyboard Keys\n\n" +
                    "Programmed and Designed by Coolcord"
            };
            Font aboutFont = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            aboutText.Font = aboutFont;
            Button btnOk = new Button() { Width = 100, Height = 30, Text = "OK", Location = new Point(150, 130), ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter };
            btnOk.Click += (btnSender, btnE) => aboutForm.Close(); //click ok to close
            aboutForm.AcceptButton = btnOk;
            aboutForm.Controls.Add(aboutText);
            aboutForm.Controls.Add(btnOk);

            //Easter Egg =D
            aboutForm.KeyPreview = true;
            CheatCode cheatCode = new CheatCode();
            aboutForm.KeyDown += new KeyEventHandler(cheatCode.GetCheatCode);

            aboutForm.ShowDialog();
            aboutForm.Dispose();
            btnOk.Dispose();
            aboutText.Dispose();
            aboutFont.Dispose();
        }

        private void tcClicktastic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcClicktastic.SelectedIndex != 0)
            {
                //Stop the Autoclicker
                AutoclickerEnabled = false;
                AutoclickerActivated = false;
                this.Invoke(new MethodInvoker(() =>
                {
                    pbAutoclickerEnabled.Image = Properties.Resources.red_circle;
                    lblAutoclickerEnabled.Text = "Disabled";
                    lblAutoclickerEnabled.ForeColor = Color.Red;
                    pbAutoclickerRunning.Image = Properties.Resources.red_circle;
                    lblAutoclickerRunning.Text = "Waiting";
                    lblAutoclickerRunning.ForeColor = Color.Red;
                }));
                AutoclickerWaiting = true;
            }
        }

        private void cbEnter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnter.Checked)
            {
                profileData.pressEnter = true;
            }
            else
            {
                profileData.pressEnter = false;
            }
            //Fix the stored autoclick key command
            profileData.AutoclickKey.cmd = keyStringConverter.KeyToCmd(profileData.AutoclickKey.key, profileData.AutoclickKey.modifierKeys, profileData.pressEnter, profileData.turbo);
        }

        private void btnManageProfiles_Click(object sender, EventArgs e)
        {
            ProfileManager profileManager = new ProfileManager(ref profileData);
            profileManager.StartPosition = FormStartPosition.CenterParent;
            profileManager.ShowDialog();

            ddbProfile.Items.Clear();
            foreach (string file in Directory.GetFiles(currentDirectory, "*.clk"))
            {
                ddbProfile.Items.Add(Path.GetFileNameWithoutExtension(file));
            }

            if (previousProfile != null && ddbProfile.Items.Contains(previousProfile))
                ddbProfile.SelectedItem = previousProfile; //select the proper profile again
            else if (ddbProfile.Items.Count > 0)
                ddbProfile.SelectedIndex = 0; //profile was deleted, so use the top one
            else //the user deleted every profile
            {
                CreateDefaultProfile();
            }

            profileManager.Dispose();
        }

        private void CreateDefaultProfile()
        {
            try
            {
                if (!Directory.Exists(currentDirectory)) //make sure the profile folder exists
                {
                    Directory.CreateDirectory(currentDirectory);
                }
                if (File.Exists(currentDirectory + "\\Default.clk")) //the file exists already
                {
                    File.SetAttributes(currentDirectory + "\\Default.clk", FileAttributes.Normal);
                    File.Delete(currentDirectory + "\\Default.clk"); //delete it
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            profileData.ActivationKey = ParseKEYCOMBO("` (~)", Keys.Oemtilde);
            profileData.DeactivationKey = ParseKEYCOMBO("` (~)", Keys.Oemtilde);
            profileData.AutoclickKey = ParseKEYCOMBO("LeftClick", Keys.None);
            profileData.Random = false;
            profileData.Hold = false;
            profileData.pressEnter = false;
            profileData.useDeactivationKey = false;
            profileData.suppressHotkeys = false;
            ddbSpeedMode.SelectedIndex = 0;
            ddbTurboMode.SelectedIndex = 0;
            profileData.turbo = 1;
            profileData.MinDelay = 1;
            profileData.MaxDelay = 1000;
            ddbActivationMode.SelectedIndex = 0;
            profile.Save("Default", ref profileData); //create a new one
            try
            {
                ddbProfile.Items.Clear();
                foreach (string file in Directory.GetFiles(currentDirectory, "*.clk"))
                {
                    ddbProfile.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
                ddbProfile.SelectedItem = "Default";
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (profile.Save(ddbProfile.Text, ref profileData))
                MessageBox.Show(ddbProfile.Text + " saved successfully!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Unable to save " + ddbProfile.Text + "!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Properties.Settings.Default.DefaultProfile = ddbProfile.Text;
            Properties.Settings.Default.Save();
        }

        private void cbSuppressHotkeys_CheckedChanged(object sender, EventArgs e)
        {
            profileData.suppressHotkeys = cbSuppressHotkeys.Checked;
        }
    }
}
