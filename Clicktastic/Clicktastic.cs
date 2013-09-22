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

/* -===========================- About Clicktastic -===========================-
 *  
 *  Clicktastic is an autoclicker that I designed for fun. The goal of this 
 *  project was to make an autoclicker that provided more options and allowed
 *  for much customization compared to what's currently available.
 *  
 *  Like usual for me when I design an application, I look around and try to see
 *  what's available. With autoclickers, I couldn't find any that were up to my
 *  liking. Consequently, this project started.
 *  
 *  Most autoclickers I found were rather slow, in my opinion, and I was determined
 *  that I could make one that was faster. This is where the turbo option comes 
 *  into play. Normally the application will request a click every 1 millisecond.
 *  However, with turbo, this can be multiplied, allowing for more clicks at each
 *  request. This speeds up the autoclicking process significantly.
 *  
 *  However, turbo mode may not work on every machine. If you experience any kind
 *  of lag, whether it'd be input lag, mouse lag, or lag within the application 
 *  that you are autoclicking, it may be a good idea to turn the turbo mode down.
 *  Ludicrous speed can be very taxing, even for very high end machines. This is 
 *  mostly a limitation of the current operating systems available, due to the
 *  way that they handle taking input via message input queues.
 * 
 *  Another problem most autoclickers have is that their speed is constant -- too
 *  constant. This is part of the reason why autoclickers can often be detected.
 *  A human can't press a button with that precise of timing for that long.
 *  Random speed mode solves this by allowing the user to input a minimum and 
 *  maximum delay time, allowing the autoclicker to appear more human like.
 *  
 *  I decided to take the autoclicker a step further and add sounds. This enhances
 *  the autoclicking experience much more. By default, the sounds only play when
 *  max turbo (Ludicrous speed) is enabled, but they can be turned to always on or
 *  muted.
 *  
 *  By making this application open source, I hope to make it easier for others
 *  to build an autoclicker that meets their own personal needs, using this
 *  source code as a base.
 *  
 *  If you wish to contact me about the application, or anything of the like,
 *  feel free to send me an email at coolcord24@gmail.com
 */

/* -================================- Credits -================================-
 *  
 *  The following files included with Clicktastic (though some modified)
 *  were not originally created by me. Credit shall be given where it is due!
 * 
 *  Loop.wav:
 *  MaxTurbo.wav:
 *  Start1.wav:
 *  Start2.wav:
 *  Stop.wav:
 *      Original sound effect from Spaceballs
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

using AxWMPLib;

namespace Clicktastic
{
    public partial class Clicktastic : Form
    {
        #region Initialization, Construction, and Startup
        //===========================================================================
        //
        // Initialization, Construction, and Startup
        //
        //===========================================================================

        //Define Structures
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct ProfileData
        {
            public KEYCOMBO ActivationKey;
            public KEYCOMBO DeactivationKey;
            public KEYCOMBO AutoclickKey;
            public Boolean random;
            public Boolean hold;
            public Boolean pressEnter;
            public Boolean useDeactivationKey;
            public Boolean suppressHotkeys;
            public Boolean mute;
            public Boolean loadSound;
            public Boolean alwaysPlay;
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

        //Define Global Variables
        Boolean AutoclickerActivated = false;
        Boolean AutoclickerEnabled = true;
        Boolean AutoclickerCharged = false;
        Boolean AutoclickerCharging = false;
        Boolean AutoclickerWaiting = true;
        Boolean Loading = false;
        Boolean SimulatingClicksOnHold = false;
        Boolean Startup = true;
        Boolean Stopped = true;
        int RetryAttempts = 0;
        KeyStringConverter keyStringConverter = new KeyStringConverter();
        Profile profile = new Profile();
        public ProfileData profileData = new ProfileData();
        Semaphore mediaSemaphore = new Semaphore(1, 1);
        Semaphore soundSemaphore = new Semaphore(1, 1);
        System.Media.SoundPlayer sound = new System.Media.SoundPlayer("C:\\Users\\Cord\\Desktop\\Start1.wav");
        SoundEffects soundEffects = null;
        static string currentDirectory = Directory.GetCurrentDirectory() + "\\Profiles";
        string previousProfile = "Default";

        //Constructor
        public Clicktastic()
        {
            InitializeComponent();
            soundEffects = new SoundEffects(ref axMedia, ref soundSemaphore, ref mediaSemaphore, ref Stopped);

            _procKey = HookCallbackKey;
            _procMouse = HookCallbackMouse;
            _hookIDKey = SetHookKey(_procKey);
            _hookIDMouse = SetHookMouse(_procMouse);

            if (!Directory.Exists(currentDirectory))
            {
                try
                {
                    Directory.CreateDirectory(currentDirectory);
                }
                catch { }
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
            catch { }
            Boolean loaded = false;
            Loading = true;
            while (!loaded)
            {
                loaded = AttemptLoad();
            }
            Startup = false;
            SetInstructions();
        }
        #endregion

        #region Interface Event Handlers
        //===========================================================================
        //
        // Interface Event Handlers
        //
        //===========================================================================

        private void ActivationButton_Click(object sender, EventArgs e)
        {
            KEYCOMBO key = GetKeyDialog("Press any key");
            if (isActivationSettingsValid(key))
            { //make sure key is valid before accepting it
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

        private void AutoclickButton_Click(object sender, EventArgs e)
        {
            KEYCOMBO key = GetKeyDialog("Press any key or click here");
            if (isAutoclickKeySettingsValid(key))
            { //make sure key is valid before accepting it
                if (key.keyString == tbActivationButton.Text || key.keyString == tbDeactivationButton.Text)
                {
                    MessageBox.Show("Autoclick button and Activator Hotkeys cannot be the same!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                profileData.AutoclickKey = key;
                tbAutoclickButton.Text = key.keyString;
            }
        }

        private void axMedia_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axMedia.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                try
                {
                    mediaSemaphore.Release();
                }
                catch { }
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
            aboutForm.BackColor = Color.Black;

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
            aboutText.ForeColor = Color.White;
            Button btnOk = new Button() { Width = 100, Height = 30, Text = "OK", Location = new Point(150, 130), ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter };
            btnOk.Click += (btnSender, btnE) => aboutForm.Close(); //click ok to close
            btnOk.BackColor = SystemColors.ButtonFace;
            btnOk.UseVisualStyleBackColor = true;
            aboutForm.AcceptButton = btnOk;
            aboutForm.Controls.Add(aboutText);
            aboutForm.Controls.Add(btnOk);

            //Easter Egg =D
            aboutForm.KeyPreview = true;
            CheatCode cheatCode = new CheatCode();
            aboutForm.KeyDown += new KeyEventHandler(cheatCode.GetCheatCode);

            //All done with the about form
            aboutForm.ShowDialog();
            aboutForm.Dispose();
            btnOk.Dispose();
            aboutText.Dispose();
            aboutFont.Dispose();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (profile.Save(ddbProfile.Text, ref profileData))
                MessageBox.Show(ddbProfile.Text + " saved successfully!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else //save failed
                MessageBox.Show("Unable to save " + ddbProfile.Text + "!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Properties.Settings.Default.DefaultProfile = ddbProfile.Text;
            Properties.Settings.Default.Save(); //load this profile the next time the application starts
        }

        private void cbAlwaysPlay_CheckedChanged(object sender, EventArgs e)
        {
            profileData.alwaysPlay = cbAlwaysPlay.Checked;
        }

        private void cbEnter_CheckedChanged(object sender, EventArgs e)
        {
            profileData.pressEnter = cbEnter.Checked;
            //Fix the stored autoclick key command
            profileData.AutoclickKey.cmd = keyStringConverter.KeyToCmd(profileData.AutoclickKey.key, profileData.AutoclickKey.modifierKeys, profileData.pressEnter, profileData.turbo);
        }

        private void cbLoadSound_CheckedChanged(object sender, EventArgs e)
        {
            profileData.loadSound = cbLoadSound.Checked;
            if (cbLoadSound.Checked)
            {
                cbAlwaysPlay.Enabled = true;
                cbAlwaysPlay.Visible = true;
            }
            else
            {
                cbAlwaysPlay.Checked = false;
                cbAlwaysPlay.Enabled = false;
                cbAlwaysPlay.Visible = false;
            }
        }

        private void cbMute_CheckedChanged(object sender, EventArgs e)
        {
            profileData.mute = cbMute.Checked;
            if (cbMute.Checked)
            {
                cbLoadSound.Checked = false;
                cbAlwaysPlay.Checked = false;
                cbLoadSound.Enabled = false;
                cbAlwaysPlay.Enabled = false;
                cbLoadSound.Visible = false;
                cbAlwaysPlay.Visible = false;
            }
            else
            {
                cbLoadSound.Enabled = true;
                cbLoadSound.Visible = true;
            }
        }

        private void cbSuppressHotkeys_CheckedChanged(object sender, EventArgs e)
        {
            profileData.suppressHotkeys = cbSuppressHotkeys.Checked;
        }

        private void cbUseDeactivationButton_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseDeactivationButton.Checked)
            {
                lblDeactivationButton.Enabled = true;
                tbDeactivationButton.Enabled = true;
                btnDeactivationButton.Enabled = true;
                lblDeactivationButton.Visible = true;
                tbDeactivationButton.Visible = true;
                btnDeactivationButton.Visible = true;
                profileData.useDeactivationKey = true;
            }
            else //no deactivation hotkey
            {
                lblDeactivationButton.Enabled = false;
                lblDeactivationButton.Visible = false;
                profileData.DeactivationKey = profileData.ActivationKey;
                tbDeactivationButton.Text = tbActivationButton.Text;
                tbDeactivationButton.Enabled = false;
                btnDeactivationButton.Enabled = false;
                tbDeactivationButton.Visible = false;
                btnDeactivationButton.Visible = false;
                profileData.useDeactivationKey = false;
            }
            SetInstructions();
        }

        private void Clicktastic_FormClosing(object sender, FormClosingEventArgs e)
        {
            Shutdown();
        }

        public void Clicktastic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers.Equals(Keys.Alt))
            {
                e.Handled = true; //don't open the menu with alt
            }
        }

        private void ddbActivationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckActivationModeSettings();
            if (ddbActivationMode.SelectedIndex == 0)
            {
                profileData.hold = false;
                AutoclickerEnabled = true;
            }
            else
            {
                profileData.hold = true;
                AutoclickerEnabled = false;
            }
            SetInstructions();
        }

        private void ddbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loading)
            {
                Boolean loaded = false;
                while (!loaded)
                { //keep trying to load a file until one succeeds
                    loaded = AttemptLoad();
                }
            }
            else
                Loading = false;
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
                profileData.random = false;
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
                profileData.random = true;
            }
            SetInstructions();
        }

        private void ddbTurboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTurbo();
            if (ddbTurboMode.SelectedIndex == 10 && (!Startup || profileData.loadSound))
            {
                if (!profileData.mute)
                { //play the max turbo sound effect
                    soundEffects.PlayEffect();
                }
            }
            SetInstructions();
        }

        private void DeactivationButton_Click(object sender, EventArgs e)
        {
            KEYCOMBO key = GetKeyDialog("Press any key");
            if (isActivationSettingsValid(key))
            { //make sure key is valid before accepting it
                if (key.keyString == tbAutoclickButton.Text)
                {
                    MessageBox.Show("Autoclick button and Activator Hotkeys cannot be the same!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                profileData.DeactivationKey = key;
                tbDeactivationButton.Text = key.keyString;
            }
        }

        private void numMaxDelay_ValueChanged(object sender, EventArgs e)
        {
            if (numMinDelay.Value > numMaxDelay.Value)
                numMinDelay.Value = numMaxDelay.Value;
            profileData.MinDelay = (int)numMinDelay.Value;
            profileData.MaxDelay = (int)numMaxDelay.Value;
            SetInstructions();
        }

        private void numMinDelay_ValueChanged(object sender, EventArgs e)
        {
            if (numMaxDelay.Value < numMinDelay.Value)
                numMaxDelay.Value = numMinDelay.Value;
            profileData.MinDelay = (int)numMinDelay.Value;
            profileData.MaxDelay = (int)numMaxDelay.Value;
            SetInstructions();
        }

        private void tbActivationButton_TextChanged(object sender, EventArgs e)
        {
            if (!cbUseDeactivationButton.Checked)
            {
                tbDeactivationButton.Text = tbActivationButton.Text;
            }
            SetInstructions();
        }

        private void tbAutoclickButton_TextChanged(object sender, EventArgs e)
        {
            if (profileData.AutoclickKey.isKeyboard)
            {
                lblActivationMode.Enabled = false;
                ddbActivationMode.Enabled = false;
                lblActivationMode.Visible = false;
                ddbActivationMode.Visible = false;
            }
            else //mouse
            {
                lblActivationMode.Enabled = true;
                ddbActivationMode.Enabled = true;
                lblActivationMode.Visible = true;
                ddbActivationMode.Visible = true;
                if (cbEnter.Enabled)
                {
                    cbEnter.Checked = false;
                    profileData.pressEnter = false; //make sure it saves
                }
                cbEnter.Enabled = false;
                cbEnter.Visible = false;
            }
            SetTurbo();
            SetInstructions();
        }

        private void tbDeactivationButton_TextChanged(object sender, EventArgs e)
        {
            SetInstructions();
        }

        private void tcClicktastic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcClicktastic.SelectedIndex != 0)
            {
                //Stop the Autoclicker
                if (AutoclickerActivated && !profileData.mute &&
                    (profileData.turbo >= 30 || profileData.alwaysPlay))
                    soundEffects.Stop();
                AutoclickerEnabled = false;
                AutoclickerActivated = false;
                AutoclickerCharged = false;
                AutoclickerWaiting = true;
                AutoclickerCharging = false;
                this.Invoke(new MethodInvoker(() =>
                { //mark status as disabled
                    pbAutoclickerEnabled.Image = Properties.Resources.RedCircle;
                    lblAutoclickerEnabled.Text = "Disabled";
                    lblAutoclickerEnabled.ForeColor = Color.Red;
                    pbAutoclickerRunning.Image = Properties.Resources.RedCircle;
                    lblAutoclickerRunning.Text = "Waiting";
                    lblAutoclickerRunning.ForeColor = Color.Red;
                }));
            }
        }
        #endregion

        #region Background Worker Handlers
        //===========================================================================
        //
        // Background Worker Handlers
        //
        //===========================================================================

        //
        // AutoClicker_DoWork(object sender, DoWorkEventArgs e)
        // Background worker for preparing, calling, and running the autoclicker
        //
        private void AutoClicker_DoWork(object sender, DoWorkEventArgs e)
        {
            soundSemaphore.WaitOne();
            Boolean noChange = true;
            if ((profileData.turbo >= 30 || profileData.alwaysPlay) &&
                        !profileData.mute)
            {
                if (profileData.hold)
                {
                    if (AutoclickerActivated)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            if (tcClicktastic.SelectedIndex == 0)
                            {
                                pbAutoclickerEnabled.Image = Properties.Resources.GreenCircle;
                                lblAutoclickerEnabled.Text = "Enabled";
                                lblAutoclickerEnabled.ForeColor = Color.Lime;
                            }
                            else
                                noChange = false;
                        }));
                        Boolean ButtonHeld = ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left);
                        if (ButtonHeld && (AutoclickerWaiting || AutoclickerCharging))
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                if (tcClicktastic.SelectedIndex == 0)
                                {
                                    pbAutoclickerRunning.Image = Properties.Resources.GreenCircle;
                                    lblAutoclickerRunning.Text = "Running";
                                    lblAutoclickerRunning.ForeColor = Color.Lime;
                                    AutoclickerWaiting = false;
                                }
                                else
                                    noChange = false;
                            }));
                        }
                        else
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                if (tcClicktastic.SelectedIndex == 0)
                                {
                                    pbAutoclickerRunning.Image = Properties.Resources.RedCircle;
                                    lblAutoclickerRunning.Text = "Waiting";
                                    lblAutoclickerRunning.ForeColor = Color.Red;
                                    AutoclickerWaiting = true;
                                }
                                else
                                    noChange = false;
                            }));
                        }
                        if (noChange)
                        {
                            AutoclickerCharging = false;
                            AutoclickerCharged = true;
                        }
                    }
                }
                else
                {
                    if (AutoclickerActivated)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            if (tcClicktastic.SelectedIndex == 0)
                            {
                                pbAutoclickerRunning.Image = Properties.Resources.GreenCircle;
                                lblAutoclickerRunning.Text = "Running";
                                lblAutoclickerRunning.ForeColor = Color.Lime;
                                AutoclickerWaiting = false;
                            }
                            else
                                noChange = false;
                        }));
                        if (noChange)
                        {
                            AutoclickerCharging = false;
                            AutoclickerCharged = true;
                        }
                    }
                }
            }
            AutoClick();
        }
        #endregion

        #region Functions
        //===========================================================================
        //
        // Functions
        //
        //===========================================================================

        //
        // AttemptLoad()
        // Attempts to load a profile and will try various things to achieve success
        //
        private Boolean AttemptLoad()
        {
            ProfileData loadProfileData = new ProfileData();
            if (profile.Load(ddbProfile.Text, ref loadProfileData))
            { //load from file successful
                ProfileData previousProfileData = profileData;
                profileData = loadProfileData;
                if (!UpdatePreferences())
                {
                    profileData = previousProfileData;
                    return false; //preferences could not be updated, so the loading failed
                }
                previousProfile = ddbProfile.Text;
                RetryAttempts = 0;
                Properties.Settings.Default.DefaultProfile = ddbProfile.Text;
                Properties.Settings.Default.Save();
                SetInstructions();
                if (Startup && profileData.loadSound && (profileData.alwaysPlay || profileData.turbo >= 30))
                { //play the prepare ship sound effect at startup
                    soundEffects.PlayEffect();
                }
                return true;
            }
            else //load from file failed
            {
                if (!Directory.Exists(currentDirectory)) //make sure the profile folder exists
                {
                    try
                    {
                        Directory.CreateDirectory(currentDirectory); //if not, create it
                    } catch { }
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

        //
        // AutoClick()
        // Prepares the timers for autoclicking and performs the clicks
        //
        private void AutoClick()
        {
            if (profileData.random)
            { //add random delay
                Random randomGen = new Random();
                int randomNumber = randomGen.Next(profileData.MinDelay, profileData.MaxDelay);
                System.Timers.Timer timer1 = new System.Timers.Timer(randomNumber);
                timer1.AutoReset = true;
                timer1.Enabled = true;
                timer1.Elapsed += (sender, e) => PerformClick(timer1, randomGen);
            }
            else //delay is constant
            {
                Random randomGen = null;
                System.Timers.Timer timer1 = new System.Timers.Timer(profileData.MinDelay);
                timer1.AutoReset = true;
                timer1.Enabled = true;
                timer1.Elapsed += (sender, e) => PerformClick(timer1, randomGen);
            }
        }

        //
        // CheckActivationModeSettings()
        // Checks activation mode settings and throws errors if they are not valid
        //
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

        //
        // CreateDefaultProfile()
        // Creates the default profile and saves it to a file
        //
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
            catch { }

            //Default settings
            profileData.ActivationKey = ParseKEYCOMBO("` (~)", Keys.Oemtilde);
            profileData.DeactivationKey = ParseKEYCOMBO("` (~)", Keys.Oemtilde);
            profileData.AutoclickKey = ParseKEYCOMBO("LeftClick", Keys.None);
            profileData.random = false;
            profileData.hold = false;
            profileData.pressEnter = false;
            profileData.useDeactivationKey = false;
            profileData.suppressHotkeys = false;
            profileData.mute = false;
            profileData.loadSound = false;
            profileData.alwaysPlay = false;
            ddbSpeedMode.SelectedIndex = 0;
            ddbTurboMode.SelectedIndex = 0;
            profileData.turbo = 1;
            profileData.MinDelay = 1;
            profileData.MaxDelay = 1000;
            ddbActivationMode.SelectedIndex = 0;

            profile.Save("Default", ref profileData); //save the file
            try
            {
                ddbProfile.Items.Clear();
                foreach (string file in Directory.GetFiles(currentDirectory, "*.clk"))
                { //update the profiles list
                    ddbProfile.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
                ddbProfile.SelectedItem = "Default";
            }
            catch { }
        }

        //
        // GetKeyDialog(string message)
        // Asks the user to press a key and returns the key that the user pressed
        //
        private KEYCOMBO GetKeyDialog(string message)
        {
            //Construct the key dialog
            Form keyPrompt = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
            keyPrompt.StartPosition = FormStartPosition.CenterParent;
            keyPrompt.Width = 250;
            keyPrompt.Height = 100;
            keyPrompt.Text = "Clicktastic";
            keyPrompt.KeyPreview = true;
            keyPrompt.Icon = Properties.Resources.clicktastic;
            keyPrompt.BackColor = Color.Black;
            Label lblKey = new Label() { Width = 250, Height = 65, ImageAlign = ContentAlignment.MiddleCenter, TextAlign = ContentAlignment.MiddleCenter, Text = message, ForeColor = Color.White };
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
                        { //add dots to the end of the message as time passes
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
                catch { }
            };
            KEYCOMBO key = new KEYCOMBO();
            Keys lastKey = Keys.None;
            key.valid = false; //assume invalid unless otherwise stated
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

            //All done with the dialog
            keyPrompt.Dispose();
            lblKey.Dispose();
            key = ParseKEYCOMBO(strKey, lastKey);
            return key;
        }

        //
        // isActivationSettingsValid(KEYCOMBO key)
        // Checks if the activation hotkey settings are valid. It throws errors and returns false if not
        //
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
            { //unknown combination of settings
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

        //
        // isAutoclickKeySettingsValid(KEYCOMBO key)
        // Checks if the activation hotkey settings are valid. It throws errors and returns false if not
        //
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
            { //unknown combination of settings
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
                if (profileData.hold) //hold is on
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
            else if (profileData.hold && key.wheel != 0)
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

        //
        // isKeyAcceptable(Keys key)
        // Checks the list of unnacceptable hotkeys and returns false if the key is unnacceptable
        //
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
                case Keys.LWin:
                case Keys.RWin:
                case Keys.None:
                    return false; //these are not accepted
                default:
                    return true; //anything else is accepted
            }
        }

        //
        // ParseKEYCOMBO(string strKey, Keys lastKeyCode)
        // Creates a KEYCOMBO structure out of a string and a keycode
        //
        private KEYCOMBO ParseKEYCOMBO(string strKey, Keys lastKeyCode)
        {
            KEYCOMBO key = new KEYCOMBO();
            if (strKey == null)
            {
                key.valid = false;
                return key; //key is invalid, so give up
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
            foreach (string button in buttons)
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
                        key.key = Keys.None; //allow only Ctrl, Shift, or Alt by themselves
                    }
                    else
                        key.key = lastKeyCode; //store the key code
                }
                previous = button;
            }

            //Add modifier keys
            if (ctrl)
                key.modifierKeys = key.modifierKeys | Keys.Control;
            if (shift)
                key.modifierKeys = key.modifierKeys | Keys.Shift;
            if (alt)
                key.modifierKeys = key.modifierKeys | Keys.Alt;
            key.cmd = keyStringConverter.KeyToCmd(key.key, key.modifierKeys, profileData.pressEnter, profileData.turbo);
            if (key.isKeyboard && key.cmd == null)
                key.valid = false; //key command could not be constructed, so key is invalid
            else
                key.valid = true;
            return key;
        }

        //
        // PerformClick(System.Timers.Timer timer, Random randomNumber)
        // Performs the clicking from the autoclicker
        //
        private void PerformClick(System.Timers.Timer timer, Random randomNumber)
        {
            try
            {
                Boolean KeyHeld = ((Control.MouseButtons & MouseButtons.Left) != 0);
                if ((!profileData.hold || (AutoclickerEnabled && KeyHeld)))
                {
                    if (!AutoclickerActivated) //stop the autoclicker
                    {
                        try
                        {
                            soundSemaphore.Release();
                        } catch { }
                        timer.Stop();
                        timer.Dispose();
                        return;
                    }
                    SimulatingClicksOnHold = true;
                    PlayKeyCombo(profileData.AutoclickKey); //autoclick!
                    SimulatingClicksOnHold = false;
                    if (profileData.random && randomNumber != null)
                        timer.Interval = randomNumber.Next(profileData.MinDelay, profileData.MaxDelay);
                }
            }
            catch { }
            if (!AutoclickerActivated) //stop the autoclicker
            {
                try
                {
                    soundSemaphore.Release();
                }
                catch { }
                timer.Stop();
                timer.Dispose();
                return;
            }
        }

        //
        // PlayKeyCombo(KEYCOMBO key)
        // Simulates a key press or click from a KEYCOMBO structure
        //
        private void PlayKeyCombo(KEYCOMBO key)
        {
            if (!key.valid)
                return; //key combo is invalid, so don't try running it
            if (profileData.hold)
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
                    catch { }
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

        //
        // SetInstructions()
        // Updates the instructions on the front tab of the application
        //
        private void SetInstructions()
        {
            string instructions = "";
            if (ddbActivationMode.SelectedIndex == 0)
            {
                instructions = instructions + "Press " + tbActivationButton.Text + " to activate Autoclicker on " + tbAutoclickButton.Text;
                if (tbActivationButton.Text != tbDeactivationButton.Text)
                    instructions = instructions + "\nPress " + tbDeactivationButton.Text + " to deactivate Autoclicker";
                pbAutoclickerEnabled.Image = Properties.Resources.GreenCircle;
                lblAutoclickerEnabled.Text = "Enabled";
                lblAutoclickerEnabled.ForeColor = Color.Lime;
                lblAutoclickerEnabled.Enabled = false;
                lblAutoclickerEnabled.Visible = false;
                lblAutoclickerEnabled.Enabled = false;
                lblAutoclickerEnabled.Visible = false;
                pbAutoclickerEnabled.Enabled = false;
                pbAutoclickerEnabled.Visible = false;
            }
            else
            {
                instructions = instructions + "Press " + tbActivationButton.Text + " to enable Autoclicker on " + tbAutoclickButton.Text;
                instructions = instructions + "\nHold " + tbAutoclickButton.Text + " to autoclick";
                if (tbActivationButton.Text != tbDeactivationButton.Text)
                    instructions = instructions + "\nPress " + tbDeactivationButton.Text + " to disable Autoclicker";
                pbAutoclickerEnabled.Image = Properties.Resources.RedCircle;
                lblAutoclickerEnabled.Text = "Disabled";
                lblAutoclickerEnabled.ForeColor = Color.Red;
                lblAutoclickerEnabled.Enabled = true;
                lblAutoclickerEnabled.Visible = true;
                pbAutoclickerEnabled.Enabled = true;
                pbAutoclickerEnabled.Visible = true;
            }
            instructions = instructions + "\n\n";
            if (ddbSpeedMode.SelectedIndex == 0)
            {
                instructions = instructions + "\nAutoclicker will run with " + numMinDelay.Value + " ms delay";
            }
            else
            {
                if (numMinDelay.Value == numMaxDelay.Value)
                    instructions = instructions + "\nAutoclicker will run with " + numMinDelay.Value + " ms delay";
                else
                    instructions = instructions + "\nDelay on Autoclicker will be between " + numMinDelay.Value + " ms and " + numMaxDelay.Value + " ms";
            }
            if (ddbTurboMode.SelectedIndex != 0)
            {
                instructions = instructions + "\n\nTurbo mode set to " + profileData.turbo + "x speed";
                if (ddbActivationMode.SelectedIndex != 0)
                    instructions = instructions + "\nTurbo and Hold mode together may not always be responsive!";
            }
            lblInstructions.Text = instructions;
        }

        //
        // SetTurbo()
        // Checks if the turbo mode is valid and calculates it
        //
        private void SetTurbo()
        {
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
                { //calculate turbo mode speed
                    cbEnter.Checked = false;
                    cbEnter.Enabled = false;
                    cbEnter.Visible = false;
                    profileData.pressEnter = false; //make sure that it is saved
                    profileData.turbo = (profileData.turbo - 1) * 3;
                }
                else
                {
                    cbEnter.Enabled = true;
                    cbEnter.Visible = true;
                }
            }
            else
            {
                if (profileData.turbo > 1)
                { //calculate turbo mode speed
                    profileData.turbo = (profileData.turbo - 1) * 3;
                }
            }
            //Fix the stored autoclick key command
            profileData.AutoclickKey.cmd = keyStringConverter.KeyToCmd(profileData.AutoclickKey.key, profileData.AutoclickKey.modifierKeys, profileData.pressEnter, profileData.turbo);
        }

        //
        // Shutdown()
        // Called when the application closes. This prevents the autoclicker from running after close
        //
        private void Shutdown()
        {
            axMedia.Ctlcontrols.stop();
            Stopped = true;
            AutoclickerActivated = false;
            AutoclickerEnabled = false;
            AutoclickerCharged = false;
            AutoclickerCharging = false;

            //Release all low level hooks
            UnhookWindowsHookEx(_hookIDKey);
            UnhookWindowsHookEx(_hookIDMouse);

            //Try to release semaphores before disposing and closing
            try
            {
                mediaSemaphore.Release();
            }
            catch { }
            try
            {
                soundSemaphore.Release();
            }
            catch { }
            try
            {
                mediaSemaphore.Dispose();
            }
            catch { }
            try
            {
                soundSemaphore.Dispose();
            }
            catch { }
        }

        //
        // ToggleAutoClicker(Keys key)
        // Turns the autoclicker on and off
        //
        private void ToggleAutoClicker(Keys key)
        {
            if (profileData.hold)
            {
                if (AutoclickerEnabled && key == profileData.DeactivationKey.key)
                {
                    AutoclickerEnabled = false;
                    AutoclickerActivated = false;
                    this.Invoke(new MethodInvoker(() =>
                    { //mark status as disabled
                        pbAutoclickerEnabled.Image = Properties.Resources.RedCircle;
                        lblAutoclickerEnabled.Text = "Disabled";
                        lblAutoclickerEnabled.ForeColor = Color.Red;
                        pbAutoclickerRunning.Image = Properties.Resources.RedCircle;
                        lblAutoclickerRunning.Text = "Waiting";
                        lblAutoclickerRunning.ForeColor = Color.Red;
                    }));
                    AutoclickerWaiting = true;
                    AutoclickerCharging = false;
                    AutoclickerCharged = false;
                    if (profileData.turbo >= 30 || profileData.alwaysPlay)
                    {
                        if (!profileData.mute)
                        { //play stop sound
                            soundEffects.Stop();
                        }
                    }
                }
                else if (!AutoclickerEnabled && key == profileData.ActivationKey.key)
                {
                    AutoclickerEnabled = true;
                    AutoclickerActivated = true;
                    if ((profileData.turbo >= 30 || profileData.alwaysPlay) &&
                        !profileData.mute)
                    { //use sounds
                        this.Invoke(new MethodInvoker(() =>
                        { //mark status as charging
                            pbAutoclickerEnabled.Image = Properties.Resources.YellowCircle;
                            lblAutoclickerEnabled.Text = "Charging";
                            lblAutoclickerEnabled.ForeColor = Color.Yellow;
                        }));
                        Boolean ButtonHeld = ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left);
                        if (ButtonHeld && AutoclickerWaiting)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            { //mark status as charging
                                pbAutoclickerRunning.Image = Properties.Resources.YellowCircle;
                                lblAutoclickerRunning.Text = "Charging";
                                lblAutoclickerRunning.ForeColor = Color.Yellow;
                            }));
                            AutoclickerWaiting = false;
                            AutoclickerCharging = true;
                        }
                        else
                        {
                            this.Invoke(new MethodInvoker(() =>
                            { //mark status as waiting
                                pbAutoclickerRunning.Image = Properties.Resources.RedCircle;
                                lblAutoclickerRunning.Text = "Waiting";
                                lblAutoclickerRunning.ForeColor = Color.Red;
                            }));
                            AutoclickerWaiting = true;
                            AutoclickerCharging = false;
                        }
                        soundSemaphore.WaitOne();
                        soundEffects.PlayLoop();
                    }
                    else //no sounds
                    {
                        this.Invoke(new MethodInvoker(() =>
                        { //mark status as enabled
                            pbAutoclickerEnabled.Image = Properties.Resources.GreenCircle;
                            lblAutoclickerEnabled.Text = "Enabled";
                            lblAutoclickerEnabled.ForeColor = Color.Lime;
                        }));
                        AutoclickerCharged = true;
                        Boolean ButtonHeld = ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left);
                        if (ButtonHeld && AutoclickerWaiting)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            { //mark status as running
                                pbAutoclickerRunning.Image = Properties.Resources.GreenCircle;
                                lblAutoclickerRunning.Text = "Running";
                                lblAutoclickerRunning.ForeColor = Color.Lime;
                            }));
                            AutoclickerWaiting = false;
                            AutoclickerCharging = false;
                        }
                        else
                        {
                            this.Invoke(new MethodInvoker(() =>
                            { //mark status as waiting
                                pbAutoclickerRunning.Image = Properties.Resources.RedCircle;
                                lblAutoclickerRunning.Text = "Waiting";
                                lblAutoclickerRunning.ForeColor = Color.Red;
                            }));
                            AutoclickerWaiting = true;
                            AutoclickerCharging = false;
                        }
                    }
                    //Run the autoclicker as a background thread
                    AutoClicker.RunWorkerAsync();
                }
            }
            else //toggle mode
            {
                if (AutoclickerActivated && key == profileData.DeactivationKey.key)
                {
                    AutoclickerActivated = false;
                    if (!AutoclickerWaiting || AutoclickerCharging)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        { //mark status as waiting
                            pbAutoclickerRunning.Image = Properties.Resources.RedCircle;
                            lblAutoclickerRunning.Text = "Waiting";
                            lblAutoclickerRunning.ForeColor = Color.Red;
                        }));
                        AutoclickerWaiting = true;
                        AutoclickerCharging = false;
                        AutoclickerCharged = false;
                        if (profileData.turbo >= 30 || profileData.alwaysPlay)
                        {
                            if (!profileData.mute)
                            { //play stop sound
                                soundEffects.Stop();
                            }
                        }
                    }
                }
                else if (!AutoclickerActivated && key == profileData.ActivationKey.key)
                {
                    if ((profileData.turbo >= 30 || profileData.alwaysPlay) &&
                        !profileData.mute)
                    { //using sounds
                        AutoclickerActivated = true;
                        if (AutoclickerWaiting)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            { //mark status as charging
                                pbAutoclickerRunning.Image = Properties.Resources.YellowCircle;
                                lblAutoclickerRunning.Text = "Charging";
                                lblAutoclickerRunning.ForeColor = Color.Yellow;
                            }));
                            AutoclickerWaiting = false;
                            AutoclickerCharging = true;
                        }
                        soundSemaphore.WaitOne();
                        soundEffects.PlayLoop();
                    }
                    else //no sounds
                    {
                        AutoclickerActivated = true;
                        if (AutoclickerWaiting)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            { //mark status as running
                                pbAutoclickerRunning.Image = Properties.Resources.GreenCircle;
                                lblAutoclickerRunning.Text = "Running";
                                lblAutoclickerRunning.ForeColor = Color.Lime;
                            }));
                            AutoclickerWaiting = false;
                            AutoclickerCharging = false;
                        }
                    }
                    //Run the autoclicker as a background thread
                    AutoClicker.RunWorkerAsync();
                }
            }
        }

        //
        // UpdatePreferences()
        // Updates the settings after loading a new profile
        //
        private Boolean UpdatePreferences()
        {
            try
            {
                if (profileData.hold) //hold mode
                    ddbActivationMode.SelectedIndex = 1;
                else //trigger mode
                    ddbActivationMode.SelectedIndex = 0;
                int MinDelay = profileData.MinDelay; //store the min delay to prevent losing the value
                int MaxDelay = profileData.MaxDelay; //store the max delay to prevent losing the value
                if (profileData.random) //random speed
                    ddbSpeedMode.SelectedIndex = 1;
                else //constant speed
                    ddbSpeedMode.SelectedIndex = 0;

                //Calculate turbo
                if (profileData.turbo > 1)
                    ddbTurboMode.SelectedIndex = (profileData.turbo / 3);
                else //no turbo
                    ddbTurboMode.SelectedIndex = profileData.turbo - 1;

                //Number Boxes
                numMinDelay.Value = MinDelay;
                numMaxDelay.Value = MaxDelay;

                //Checkboxes
                cbUseDeactivationButton.Checked = profileData.useDeactivationKey;
                cbEnter.Checked = profileData.pressEnter;
                cbSuppressHotkeys.Checked = profileData.suppressHotkeys;
                cbMute.Checked = profileData.mute;
                cbLoadSound.Checked = profileData.loadSound;
                cbAlwaysPlay.Checked = profileData.alwaysPlay;

                //Textboxes
                tbActivationButton.Text = profileData.ActivationKey.keyString;
                tbDeactivationButton.Text = profileData.DeactivationKey.keyString;
                tbAutoclickButton.Text = profileData.AutoclickKey.keyString;
            }
            catch
            { //preferences could not be updated -- assume that the loaded file is corrupted
                return false;
            }
            return true;
        }
        #endregion

        #region Low Level Keyboard Hook
        //===========================================================================
        //
        // Low Level Keyboard Hook
        //
        //===========================================================================

        //Define constants
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        //Define Global Variables
        private LowLevelKeyboardProc _procKey = null;
        private static IntPtr _hookIDKey = IntPtr.Zero;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Import DLLs
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

        //
        // HookCallbackKey(int nCode, IntPtr wParam, IntPtr lParam)
        // Callback for when a keyboard event occurs
        //
        private IntPtr HookCallbackKey(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (tcClicktastic.SelectedIndex == 0 &&
                    ((profileData.ActivationKey.modifierKeys == Control.ModifierKeys &&
                    profileData.ActivationKey.key == Keys.None) ||
                    (profileData.DeactivationKey.modifierKeys == Control.ModifierKeys &&
                    profileData.DeactivationKey.key == Keys.None)))
                { //Ctrl, Shift, or Alt only
                    int vkCode = Marshal.ReadInt32(lParam);
                    ToggleAutoClicker(Keys.None);
                    if (cbSuppressHotkeys.Checked)
                    { //suppress the key
                        return (IntPtr)1; //dummy value
                    }
                }
                else if ((wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN) && tcClicktastic.SelectedIndex == 0)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    if (((Keys)vkCode == profileData.ActivationKey.key && profileData.ActivationKey.modifierKeys == Control.ModifierKeys) ||
                        ((Keys)vkCode == profileData.DeactivationKey.key && profileData.DeactivationKey.modifierKeys == Control.ModifierKeys))
                    { //hotkey pressed
                        ToggleAutoClicker((Keys)vkCode);
                        if (cbSuppressHotkeys.Checked)
                        { //suppress the key
                            return (IntPtr)1; //dummy value
                        }
                    }
                }
            }

            return CallNextHookEx(_hookIDKey, nCode, wParam, lParam);
        }

        //
        // SetHookKey(LowLevelKeyboardProc proc)
        // Hooks the low level hook to the process
        //
        private static IntPtr SetHookKey(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        #endregion

        #region Low Level Mouse Hook
        //===========================================================================
        //
        // Low Level Mouse Hook
        //
        //===========================================================================

        //Define input constants
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
        private struct MSLLHOOKSTRUCT
        {
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        //Define Global Variables
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelMouseProc _procMouse = null;
        private static IntPtr _hookIDMouse = IntPtr.Zero;

        //Define output constants
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        private const UInt32 MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const UInt32 MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const UInt32 MOUSEEVENTF_RIGHTUP = 0x0010;
        private const UInt32 MOUSEEVENTF_WHEEL = 0x0800;

        //Import DLLs
        [DllImport("user32.dll")] //Mouse Output
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, uint dwExtraInf);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)] //Mouse Hook
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        //
        // HookCallbackMouse(int nCode, IntPtr wParam, IntPtr lParam)
        // Callback for when a mouse event occurs
        //
        private IntPtr HookCallbackMouse(
        int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 &&
                MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                if (profileData.hold && AutoclickerEnabled && !SimulatingClicksOnHold)
                {
                    if (AutoclickerWaiting && !AutoclickerCharged)
                    { //mark status as charging
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.YellowCircle;
                            lblAutoclickerRunning.Text = "Charging";
                            lblAutoclickerRunning.ForeColor = Color.Yellow;
                        }));
                        AutoclickerWaiting = false;
                        AutoclickerCharging = true;
                    }
                    else if (AutoclickerWaiting && AutoclickerCharged)
                    { //mark status as running
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.GreenCircle;
                            lblAutoclickerRunning.Text = "Running";
                            lblAutoclickerRunning.ForeColor = Color.Lime;
                        }));
                        AutoclickerWaiting = false;
                        AutoclickerCharging = false;
                    }
                }
            }
            if (nCode >= 0 &&
                MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                if (profileData.hold && AutoclickerEnabled && !SimulatingClicksOnHold)
                {
                    if (!AutoclickerWaiting || AutoclickerCharging)
                    { //mark status as waiting
                        this.Invoke(new MethodInvoker(() =>
                        {
                            pbAutoclickerRunning.Image = Properties.Resources.RedCircle;
                            lblAutoclickerRunning.Text = "Waiting";
                            lblAutoclickerRunning.ForeColor = Color.Red;
                        }));
                        AutoclickerWaiting = true;
                        AutoclickerCharging = false;
                    }
                }
            }
            return CallNextHookEx(_hookIDMouse, nCode, wParam, lParam);
        }

        //
        // SetHookMouse(LowLevelMouseProc proc)
        // Hooks the low level hook to the process
        //
        private static IntPtr SetHookMouse(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        #endregion
    }
}
