﻿namespace Clicktastic
{
    partial class Clicktastic
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Clicktastic));
            this.tcClicktastic = new System.Windows.Forms.TabControl();
            this.tbAutoclicker = new System.Windows.Forms.TabPage();
            this.axMedia = new AxWMPLib.AxWindowsMediaPlayer();
            this.pbAutoclickerRunning = new System.Windows.Forms.PictureBox();
            this.pbAutoclickerEnabled = new System.Windows.Forms.PictureBox();
            this.lblAutoclickerRunning = new System.Windows.Forms.Label();
            this.lblAutoclickerEnabled = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.tbPreferences = new System.Windows.Forms.TabPage();
            this.cbLoadSound = new System.Windows.Forms.CheckBox();
            this.cbAlwaysPlay = new System.Windows.Forms.CheckBox();
            this.cbMute = new System.Windows.Forms.CheckBox();
            this.cbSuppressHotkeys = new System.Windows.Forms.CheckBox();
            this.cbEnter = new System.Windows.Forms.CheckBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.lblTurboMode = new System.Windows.Forms.Label();
            this.ddbTurboMode = new System.Windows.Forms.ComboBox();
            this.btnAutoclickButton = new System.Windows.Forms.Button();
            this.btnDeactivationButton = new System.Windows.Forms.Button();
            this.btnActivationButton = new System.Windows.Forms.Button();
            this.btnManageProfiles = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ddbActivationMode = new System.Windows.Forms.ComboBox();
            this.lblActivationMode = new System.Windows.Forms.Label();
            this.lblMaxDelay = new System.Windows.Forms.Label();
            this.lblMaxCPS = new System.Windows.Forms.Label();
            this.numMaxDelay = new System.Windows.Forms.NumericUpDown();
            this.lblMinDelay = new System.Windows.Forms.Label();
            this.ddbSpeedMode = new System.Windows.Forms.ComboBox();
            this.lblMinCPS = new System.Windows.Forms.Label();
            this.numMinDelay = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.tbAutoclickButton = new System.Windows.Forms.TextBox();
            this.lblAutoclickButton = new System.Windows.Forms.Label();
            this.tbDeactivationButton = new System.Windows.Forms.TextBox();
            this.lblDeactivationButton = new System.Windows.Forms.Label();
            this.cbUseDeactivationButton = new System.Windows.Forms.CheckBox();
            this.tbActivationButton = new System.Windows.Forms.TextBox();
            this.lblActivationButton = new System.Windows.Forms.Label();
            this.lblSelectProfile = new System.Windows.Forms.Label();
            this.ddbProfile = new System.Windows.Forms.ComboBox();
            this.AutoClicker = new System.ComponentModel.BackgroundWorker();
            this.tcClicktastic.SuspendLayout();
            this.tbAutoclicker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMedia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerRunning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerEnabled)).BeginInit();
            this.tbPreferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // tcClicktastic
            // 
            this.tcClicktastic.Controls.Add(this.tbAutoclicker);
            this.tcClicktastic.Controls.Add(this.tbPreferences);
            this.tcClicktastic.Location = new System.Drawing.Point(-1, 0);
            this.tcClicktastic.Name = "tcClicktastic";
            this.tcClicktastic.SelectedIndex = 0;
            this.tcClicktastic.Size = new System.Drawing.Size(529, 341);
            this.tcClicktastic.TabIndex = 0;
            this.tcClicktastic.SelectedIndexChanged += new System.EventHandler(this.tcClicktastic_SelectedIndexChanged);
            // 
            // tbAutoclicker
            // 
            this.tbAutoclicker.BackColor = System.Drawing.Color.Black;
            this.tbAutoclicker.Controls.Add(this.axMedia);
            this.tbAutoclicker.Controls.Add(this.pbAutoclickerRunning);
            this.tbAutoclicker.Controls.Add(this.pbAutoclickerEnabled);
            this.tbAutoclicker.Controls.Add(this.lblAutoclickerRunning);
            this.tbAutoclicker.Controls.Add(this.lblAutoclickerEnabled);
            this.tbAutoclicker.Controls.Add(this.lblInstructions);
            this.tbAutoclicker.Location = new System.Drawing.Point(4, 25);
            this.tbAutoclicker.Name = "tbAutoclicker";
            this.tbAutoclicker.Padding = new System.Windows.Forms.Padding(3);
            this.tbAutoclicker.Size = new System.Drawing.Size(521, 312);
            this.tbAutoclicker.TabIndex = 0;
            this.tbAutoclicker.Text = "Autoclicker";
            // 
            // axMedia
            // 
            this.axMedia.Enabled = true;
            this.axMedia.Location = new System.Drawing.Point(514, 301);
            this.axMedia.Name = "axMedia";
            this.axMedia.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMedia.OcxState")));
            this.axMedia.Size = new System.Drawing.Size(10, 10);
            this.axMedia.TabIndex = 8;
            this.axMedia.Visible = false;
            this.axMedia.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.axMedia_PlayStateChange);
            // 
            // pbAutoclickerRunning
            // 
            this.pbAutoclickerRunning.Image = global::Clicktastic.Properties.Resources.RedCircle;
            this.pbAutoclickerRunning.ImageLocation = "";
            this.pbAutoclickerRunning.Location = new System.Drawing.Point(203, 239);
            this.pbAutoclickerRunning.Name = "pbAutoclickerRunning";
            this.pbAutoclickerRunning.Size = new System.Drawing.Size(29, 29);
            this.pbAutoclickerRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAutoclickerRunning.TabIndex = 6;
            this.pbAutoclickerRunning.TabStop = false;
            // 
            // pbAutoclickerEnabled
            // 
            this.pbAutoclickerEnabled.Image = global::Clicktastic.Properties.Resources.RedCircle;
            this.pbAutoclickerEnabled.ImageLocation = "";
            this.pbAutoclickerEnabled.Location = new System.Drawing.Point(203, 204);
            this.pbAutoclickerEnabled.Name = "pbAutoclickerEnabled";
            this.pbAutoclickerEnabled.Size = new System.Drawing.Size(29, 29);
            this.pbAutoclickerEnabled.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAutoclickerEnabled.TabIndex = 5;
            this.pbAutoclickerEnabled.TabStop = false;
            // 
            // lblAutoclickerRunning
            // 
            this.lblAutoclickerRunning.AutoSize = true;
            this.lblAutoclickerRunning.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoclickerRunning.ForeColor = System.Drawing.Color.Red;
            this.lblAutoclickerRunning.Location = new System.Drawing.Point(238, 239);
            this.lblAutoclickerRunning.Name = "lblAutoclickerRunning";
            this.lblAutoclickerRunning.Size = new System.Drawing.Size(98, 29);
            this.lblAutoclickerRunning.TabIndex = 4;
            this.lblAutoclickerRunning.Text = "Waiting";
            // 
            // lblAutoclickerEnabled
            // 
            this.lblAutoclickerEnabled.AutoSize = true;
            this.lblAutoclickerEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoclickerEnabled.ForeColor = System.Drawing.Color.Red;
            this.lblAutoclickerEnabled.Location = new System.Drawing.Point(238, 204);
            this.lblAutoclickerEnabled.Name = "lblAutoclickerEnabled";
            this.lblAutoclickerEnabled.Size = new System.Drawing.Size(112, 29);
            this.lblAutoclickerEnabled.TabIndex = 3;
            this.lblAutoclickerEnabled.Text = "Disabled";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.ForeColor = System.Drawing.Color.White;
            this.lblInstructions.Location = new System.Drawing.Point(9, 3);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(499, 198);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "Press ~ to activate Autoclicker on Left Click";
            this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbPreferences
            // 
            this.tbPreferences.BackColor = System.Drawing.Color.Black;
            this.tbPreferences.Controls.Add(this.cbLoadSound);
            this.tbPreferences.Controls.Add(this.cbAlwaysPlay);
            this.tbPreferences.Controls.Add(this.cbMute);
            this.tbPreferences.Controls.Add(this.cbSuppressHotkeys);
            this.tbPreferences.Controls.Add(this.cbEnter);
            this.tbPreferences.Controls.Add(this.btnAbout);
            this.tbPreferences.Controls.Add(this.lblTurboMode);
            this.tbPreferences.Controls.Add(this.ddbTurboMode);
            this.tbPreferences.Controls.Add(this.btnAutoclickButton);
            this.tbPreferences.Controls.Add(this.btnDeactivationButton);
            this.tbPreferences.Controls.Add(this.btnActivationButton);
            this.tbPreferences.Controls.Add(this.btnManageProfiles);
            this.tbPreferences.Controls.Add(this.btnSave);
            this.tbPreferences.Controls.Add(this.ddbActivationMode);
            this.tbPreferences.Controls.Add(this.lblActivationMode);
            this.tbPreferences.Controls.Add(this.lblMaxDelay);
            this.tbPreferences.Controls.Add(this.lblMaxCPS);
            this.tbPreferences.Controls.Add(this.numMaxDelay);
            this.tbPreferences.Controls.Add(this.lblMinDelay);
            this.tbPreferences.Controls.Add(this.ddbSpeedMode);
            this.tbPreferences.Controls.Add(this.lblMinCPS);
            this.tbPreferences.Controls.Add(this.numMinDelay);
            this.tbPreferences.Controls.Add(this.label5);
            this.tbPreferences.Controls.Add(this.tbAutoclickButton);
            this.tbPreferences.Controls.Add(this.lblAutoclickButton);
            this.tbPreferences.Controls.Add(this.tbDeactivationButton);
            this.tbPreferences.Controls.Add(this.lblDeactivationButton);
            this.tbPreferences.Controls.Add(this.cbUseDeactivationButton);
            this.tbPreferences.Controls.Add(this.tbActivationButton);
            this.tbPreferences.Controls.Add(this.lblActivationButton);
            this.tbPreferences.Controls.Add(this.lblSelectProfile);
            this.tbPreferences.Controls.Add(this.ddbProfile);
            this.tbPreferences.Location = new System.Drawing.Point(4, 25);
            this.tbPreferences.Name = "tbPreferences";
            this.tbPreferences.Padding = new System.Windows.Forms.Padding(3);
            this.tbPreferences.Size = new System.Drawing.Size(521, 312);
            this.tbPreferences.TabIndex = 1;
            this.tbPreferences.Text = "Preferences";
            // 
            // cbLoadSound
            // 
            this.cbLoadSound.AutoSize = true;
            this.cbLoadSound.ForeColor = System.Drawing.Color.White;
            this.cbLoadSound.Location = new System.Drawing.Point(354, 248);
            this.cbLoadSound.Name = "cbLoadSound";
            this.cbLoadSound.Size = new System.Drawing.Size(134, 21);
            this.cbLoadSound.TabIndex = 33;
            this.cbLoadSound.Text = "Sounds on Load";
            this.cbLoadSound.UseVisualStyleBackColor = true;
            this.cbLoadSound.CheckedChanged += new System.EventHandler(this.cbLoadSound_CheckedChanged);
            // 
            // cbAlwaysPlay
            // 
            this.cbAlwaysPlay.AutoSize = true;
            this.cbAlwaysPlay.Enabled = false;
            this.cbAlwaysPlay.ForeColor = System.Drawing.Color.White;
            this.cbAlwaysPlay.Location = new System.Drawing.Point(354, 275);
            this.cbAlwaysPlay.Name = "cbAlwaysPlay";
            this.cbAlwaysPlay.Size = new System.Drawing.Size(156, 21);
            this.cbAlwaysPlay.TabIndex = 32;
            this.cbAlwaysPlay.Text = "Always Play Sounds";
            this.cbAlwaysPlay.UseVisualStyleBackColor = true;
            this.cbAlwaysPlay.Visible = false;
            this.cbAlwaysPlay.CheckedChanged += new System.EventHandler(this.cbAlwaysPlay_CheckedChanged);
            // 
            // cbMute
            // 
            this.cbMute.AutoSize = true;
            this.cbMute.ForeColor = System.Drawing.Color.White;
            this.cbMute.Location = new System.Drawing.Point(354, 221);
            this.cbMute.Name = "cbMute";
            this.cbMute.Size = new System.Drawing.Size(153, 21);
            this.cbMute.TabIndex = 31;
            this.cbMute.Text = "Mute Sound Effects";
            this.cbMute.UseVisualStyleBackColor = true;
            this.cbMute.CheckedChanged += new System.EventHandler(this.cbMute_CheckedChanged);
            // 
            // cbSuppressHotkeys
            // 
            this.cbSuppressHotkeys.AutoSize = true;
            this.cbSuppressHotkeys.ForeColor = System.Drawing.Color.White;
            this.cbSuppressHotkeys.Location = new System.Drawing.Point(245, 67);
            this.cbSuppressHotkeys.Name = "cbSuppressHotkeys";
            this.cbSuppressHotkeys.Size = new System.Drawing.Size(145, 21);
            this.cbSuppressHotkeys.TabIndex = 30;
            this.cbSuppressHotkeys.Text = "Suppress Hotkeys";
            this.cbSuppressHotkeys.UseVisualStyleBackColor = true;
            this.cbSuppressHotkeys.CheckedChanged += new System.EventHandler(this.cbSuppressHotkeys_CheckedChanged);
            // 
            // cbEnter
            // 
            this.cbEnter.AutoSize = true;
            this.cbEnter.Enabled = false;
            this.cbEnter.ForeColor = System.Drawing.Color.White;
            this.cbEnter.Location = new System.Drawing.Point(10, 184);
            this.cbEnter.Name = "cbEnter";
            this.cbEnter.Size = new System.Drawing.Size(230, 21);
            this.cbEnter.TabIndex = 29;
            this.cbEnter.Text = "Press enter after each autoclick";
            this.cbEnter.UseVisualStyleBackColor = true;
            this.cbEnter.Visible = false;
            this.cbEnter.CheckedChanged += new System.EventHandler(this.cbEnter_CheckedChanged);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(355, 8);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(154, 57);
            this.btnAbout.TabIndex = 28;
            this.btnAbout.Text = "About Clicktastic";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // lblTurboMode
            // 
            this.lblTurboMode.AutoSize = true;
            this.lblTurboMode.ForeColor = System.Drawing.Color.White;
            this.lblTurboMode.Location = new System.Drawing.Point(179, 208);
            this.lblTurboMode.Name = "lblTurboMode";
            this.lblTurboMode.Size = new System.Drawing.Size(89, 17);
            this.lblTurboMode.TabIndex = 27;
            this.lblTurboMode.Text = "Turbo Mode:";
            // 
            // ddbTurboMode
            // 
            this.ddbTurboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddbTurboMode.FormattingEnabled = true;
            this.ddbTurboMode.Items.AddRange(new object[] {
            "(1x) None",
            "(3x) Mini Boost",
            "(6x) Small Boost",
            "(9x) Large Boost",
            "(12x) Thrusters",
            "(15x) Afterburners",
            "(18x) Impulse",
            "(21x) Light Speed",
            "(24x) Hyperdrive",
            "(27x) Warp 10",
            "(30x) Ludicrous Speed!"});
            this.ddbTurboMode.Location = new System.Drawing.Point(182, 228);
            this.ddbTurboMode.MaxDropDownItems = 2;
            this.ddbTurboMode.Name = "ddbTurboMode";
            this.ddbTurboMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddbTurboMode.Size = new System.Drawing.Size(166, 24);
            this.ddbTurboMode.TabIndex = 26;
            this.ddbTurboMode.SelectedIndexChanged += new System.EventHandler(this.ddbTurboMode_SelectedIndexChanged);
            // 
            // btnAutoclickButton
            // 
            this.btnAutoclickButton.Location = new System.Drawing.Point(182, 156);
            this.btnAutoclickButton.Name = "btnAutoclickButton";
            this.btnAutoclickButton.Size = new System.Drawing.Size(42, 23);
            this.btnAutoclickButton.TabIndex = 24;
            this.btnAutoclickButton.Text = "Set";
            this.btnAutoclickButton.UseVisualStyleBackColor = true;
            this.btnAutoclickButton.Click += new System.EventHandler(this.AutoclickButton_Click);
            // 
            // btnDeactivationButton
            // 
            this.btnDeactivationButton.Enabled = false;
            this.btnDeactivationButton.Location = new System.Drawing.Point(417, 182);
            this.btnDeactivationButton.Name = "btnDeactivationButton";
            this.btnDeactivationButton.Size = new System.Drawing.Size(42, 23);
            this.btnDeactivationButton.TabIndex = 23;
            this.btnDeactivationButton.Text = "Set";
            this.btnDeactivationButton.UseVisualStyleBackColor = true;
            this.btnDeactivationButton.Visible = false;
            this.btnDeactivationButton.Click += new System.EventHandler(this.DeactivationButton_Click);
            // 
            // btnActivationButton
            // 
            this.btnActivationButton.Location = new System.Drawing.Point(417, 112);
            this.btnActivationButton.Name = "btnActivationButton";
            this.btnActivationButton.Size = new System.Drawing.Size(42, 23);
            this.btnActivationButton.TabIndex = 22;
            this.btnActivationButton.Text = "Set";
            this.btnActivationButton.UseVisualStyleBackColor = true;
            this.btnActivationButton.Click += new System.EventHandler(this.ActivationButton_Click);
            // 
            // btnManageProfiles
            // 
            this.btnManageProfiles.Location = new System.Drawing.Point(96, 57);
            this.btnManageProfiles.Name = "btnManageProfiles";
            this.btnManageProfiles.Size = new System.Drawing.Size(144, 30);
            this.btnManageProfiles.TabIndex = 21;
            this.btnManageProfiles.Text = "Manage Profiles";
            this.btnManageProfiles.UseVisualStyleBackColor = true;
            this.btnManageProfiles.Click += new System.EventHandler(this.btnManageProfiles_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(10, 58);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ddbActivationMode
            // 
            this.ddbActivationMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddbActivationMode.FormattingEnabled = true;
            this.ddbActivationMode.Items.AddRange(new object[] {
            "Toggle",
            "Hold"});
            this.ddbActivationMode.Location = new System.Drawing.Point(9, 111);
            this.ddbActivationMode.MaxDropDownItems = 2;
            this.ddbActivationMode.Name = "ddbActivationMode";
            this.ddbActivationMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddbActivationMode.Size = new System.Drawing.Size(167, 24);
            this.ddbActivationMode.TabIndex = 19;
            this.ddbActivationMode.SelectedIndexChanged += new System.EventHandler(this.ddbActivationMode_SelectedIndexChanged);
            // 
            // lblActivationMode
            // 
            this.lblActivationMode.AutoSize = true;
            this.lblActivationMode.ForeColor = System.Drawing.Color.White;
            this.lblActivationMode.Location = new System.Drawing.Point(7, 91);
            this.lblActivationMode.Name = "lblActivationMode";
            this.lblActivationMode.Size = new System.Drawing.Size(112, 17);
            this.lblActivationMode.TabIndex = 18;
            this.lblActivationMode.Text = "Activation Mode:";
            // 
            // lblMaxDelay
            // 
            this.lblMaxDelay.AutoSize = true;
            this.lblMaxDelay.ForeColor = System.Drawing.Color.White;
            this.lblMaxDelay.Location = new System.Drawing.Point(179, 255);
            this.lblMaxDelay.Name = "lblMaxDelay";
            this.lblMaxDelay.Size = new System.Drawing.Size(145, 17);
            this.lblMaxDelay.TabIndex = 17;
            this.lblMaxDelay.Text = "Maximum Delay Time:";
            this.lblMaxDelay.Visible = false;
            // 
            // lblMaxCPS
            // 
            this.lblMaxCPS.AutoSize = true;
            this.lblMaxCPS.ForeColor = System.Drawing.Color.White;
            this.lblMaxCPS.Location = new System.Drawing.Point(322, 282);
            this.lblMaxCPS.Name = "lblMaxCPS";
            this.lblMaxCPS.Size = new System.Drawing.Size(26, 17);
            this.lblMaxCPS.TabIndex = 16;
            this.lblMaxCPS.Text = "ms";
            this.lblMaxCPS.Visible = false;
            // 
            // numMaxDelay
            // 
            this.numMaxDelay.Location = new System.Drawing.Point(182, 277);
            this.numMaxDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMaxDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxDelay.Name = "numMaxDelay";
            this.numMaxDelay.Size = new System.Drawing.Size(134, 22);
            this.numMaxDelay.TabIndex = 15;
            this.numMaxDelay.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMaxDelay.Visible = false;
            this.numMaxDelay.ValueChanged += new System.EventHandler(this.numMaxDelay_ValueChanged);
            // 
            // lblMinDelay
            // 
            this.lblMinDelay.AutoSize = true;
            this.lblMinDelay.ForeColor = System.Drawing.Color.White;
            this.lblMinDelay.Location = new System.Drawing.Point(7, 255);
            this.lblMinDelay.Name = "lblMinDelay";
            this.lblMinDelay.Size = new System.Drawing.Size(142, 17);
            this.lblMinDelay.TabIndex = 14;
            this.lblMinDelay.Text = "Minimum Delay Time:";
            // 
            // ddbSpeedMode
            // 
            this.ddbSpeedMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddbSpeedMode.FormattingEnabled = true;
            this.ddbSpeedMode.Items.AddRange(new object[] {
            "Constant",
            "Random"});
            this.ddbSpeedMode.Location = new System.Drawing.Point(10, 228);
            this.ddbSpeedMode.MaxDropDownItems = 2;
            this.ddbSpeedMode.Name = "ddbSpeedMode";
            this.ddbSpeedMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddbSpeedMode.Size = new System.Drawing.Size(166, 24);
            this.ddbSpeedMode.TabIndex = 13;
            this.ddbSpeedMode.SelectedIndexChanged += new System.EventHandler(this.ddbSpeedMode_SelectedIndexChanged);
            // 
            // lblMinCPS
            // 
            this.lblMinCPS.AutoSize = true;
            this.lblMinCPS.ForeColor = System.Drawing.Color.White;
            this.lblMinCPS.Location = new System.Drawing.Point(150, 279);
            this.lblMinCPS.Name = "lblMinCPS";
            this.lblMinCPS.Size = new System.Drawing.Size(26, 17);
            this.lblMinCPS.TabIndex = 12;
            this.lblMinCPS.Text = "ms";
            // 
            // numMinDelay
            // 
            this.numMinDelay.Location = new System.Drawing.Point(10, 275);
            this.numMinDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMinDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinDelay.Name = "numMinDelay";
            this.numMinDelay.Size = new System.Drawing.Size(134, 22);
            this.numMinDelay.TabIndex = 11;
            this.numMinDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinDelay.ValueChanged += new System.EventHandler(this.numMinDelay_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(7, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Speed Mode:";
            // 
            // tbAutoclickButton
            // 
            this.tbAutoclickButton.Location = new System.Drawing.Point(10, 157);
            this.tbAutoclickButton.Name = "tbAutoclickButton";
            this.tbAutoclickButton.ReadOnly = true;
            this.tbAutoclickButton.Size = new System.Drawing.Size(166, 22);
            this.tbAutoclickButton.TabIndex = 9;
            this.tbAutoclickButton.Text = "LeftClick";
            this.tbAutoclickButton.Click += new System.EventHandler(this.AutoclickButton_Click);
            this.tbAutoclickButton.TextChanged += new System.EventHandler(this.tbAutoclickButton_TextChanged);
            // 
            // lblAutoclickButton
            // 
            this.lblAutoclickButton.AutoSize = true;
            this.lblAutoclickButton.ForeColor = System.Drawing.Color.White;
            this.lblAutoclickButton.Location = new System.Drawing.Point(7, 138);
            this.lblAutoclickButton.Name = "lblAutoclickButton";
            this.lblAutoclickButton.Size = new System.Drawing.Size(129, 17);
            this.lblAutoclickButton.TabIndex = 8;
            this.lblAutoclickButton.Text = "Button to Autoclick:";
            // 
            // tbDeactivationButton
            // 
            this.tbDeactivationButton.Enabled = false;
            this.tbDeactivationButton.Location = new System.Drawing.Point(245, 183);
            this.tbDeactivationButton.Name = "tbDeactivationButton";
            this.tbDeactivationButton.ReadOnly = true;
            this.tbDeactivationButton.Size = new System.Drawing.Size(166, 22);
            this.tbDeactivationButton.TabIndex = 6;
            this.tbDeactivationButton.Text = "` (~)";
            this.tbDeactivationButton.Visible = false;
            this.tbDeactivationButton.Click += new System.EventHandler(this.DeactivationButton_Click);
            this.tbDeactivationButton.TextChanged += new System.EventHandler(this.tbDeactivationButton_TextChanged);
            // 
            // lblDeactivationButton
            // 
            this.lblDeactivationButton.AutoSize = true;
            this.lblDeactivationButton.Enabled = false;
            this.lblDeactivationButton.ForeColor = System.Drawing.Color.White;
            this.lblDeactivationButton.Location = new System.Drawing.Point(242, 162);
            this.lblDeactivationButton.Name = "lblDeactivationButton";
            this.lblDeactivationButton.Size = new System.Drawing.Size(132, 17);
            this.lblDeactivationButton.TabIndex = 5;
            this.lblDeactivationButton.Text = "Deactivator Hotkey:";
            this.lblDeactivationButton.Visible = false;
            // 
            // cbUseDeactivationButton
            // 
            this.cbUseDeactivationButton.AutoSize = true;
            this.cbUseDeactivationButton.ForeColor = System.Drawing.Color.White;
            this.cbUseDeactivationButton.Location = new System.Drawing.Point(245, 139);
            this.cbUseDeactivationButton.Name = "cbUseDeactivationButton";
            this.cbUseDeactivationButton.Size = new System.Drawing.Size(264, 21);
            this.cbUseDeactivationButton.TabIndex = 4;
            this.cbUseDeactivationButton.Text = "Use Different Hotkey for Deactivation";
            this.cbUseDeactivationButton.UseVisualStyleBackColor = true;
            this.cbUseDeactivationButton.CheckedChanged += new System.EventHandler(this.cbUseDeactivationButton_CheckedChanged);
            // 
            // tbActivationButton
            // 
            this.tbActivationButton.Location = new System.Drawing.Point(245, 111);
            this.tbActivationButton.Name = "tbActivationButton";
            this.tbActivationButton.ReadOnly = true;
            this.tbActivationButton.Size = new System.Drawing.Size(166, 22);
            this.tbActivationButton.TabIndex = 3;
            this.tbActivationButton.Text = "` (~)";
            this.tbActivationButton.Click += new System.EventHandler(this.ActivationButton_Click);
            this.tbActivationButton.TextChanged += new System.EventHandler(this.tbActivationButton_TextChanged);
            // 
            // lblActivationButton
            // 
            this.lblActivationButton.AutoSize = true;
            this.lblActivationButton.ForeColor = System.Drawing.Color.White;
            this.lblActivationButton.Location = new System.Drawing.Point(242, 91);
            this.lblActivationButton.Name = "lblActivationButton";
            this.lblActivationButton.Size = new System.Drawing.Size(115, 17);
            this.lblActivationButton.TabIndex = 2;
            this.lblActivationButton.Text = "Activator Hotkey:";
            // 
            // lblSelectProfile
            // 
            this.lblSelectProfile.AutoSize = true;
            this.lblSelectProfile.ForeColor = System.Drawing.Color.White;
            this.lblSelectProfile.Location = new System.Drawing.Point(7, 8);
            this.lblSelectProfile.Name = "lblSelectProfile";
            this.lblSelectProfile.Size = new System.Drawing.Size(95, 17);
            this.lblSelectProfile.TabIndex = 1;
            this.lblSelectProfile.Text = "Select Profile:";
            // 
            // ddbProfile
            // 
            this.ddbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddbProfile.FormattingEnabled = true;
            this.ddbProfile.Location = new System.Drawing.Point(9, 28);
            this.ddbProfile.MaxDropDownItems = 100;
            this.ddbProfile.Name = "ddbProfile";
            this.ddbProfile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddbProfile.Size = new System.Drawing.Size(231, 24);
            this.ddbProfile.TabIndex = 0;
            this.ddbProfile.SelectedIndexChanged += new System.EventHandler(this.ddbProfile_SelectedIndexChanged);
            // 
            // AutoClicker
            // 
            this.AutoClicker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AutoClicker_DoWork);
            // 
            // Clicktastic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(523, 334);
            this.Controls.Add(this.tcClicktastic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Clicktastic";
            this.Text = "Clicktastic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Clicktastic_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Clicktastic_KeyDown);
            this.tcClicktastic.ResumeLayout(false);
            this.tbAutoclicker.ResumeLayout(false);
            this.tbAutoclicker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMedia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerRunning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerEnabled)).EndInit();
            this.tbPreferences.ResumeLayout(false);
            this.tbPreferences.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcClicktastic;
        private System.Windows.Forms.TabPage tbAutoclicker;
        private System.Windows.Forms.TabPage tbPreferences;
        private System.Windows.Forms.ComboBox ddbProfile;
        private System.Windows.Forms.ComboBox ddbSpeedMode;
        private System.Windows.Forms.Label lblMinCPS;
        private System.Windows.Forms.NumericUpDown numMinDelay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbAutoclickButton;
        private System.Windows.Forms.Label lblAutoclickButton;
        private System.Windows.Forms.TextBox tbDeactivationButton;
        private System.Windows.Forms.Label lblDeactivationButton;
        private System.Windows.Forms.CheckBox cbUseDeactivationButton;
        private System.Windows.Forms.TextBox tbActivationButton;
        private System.Windows.Forms.Label lblActivationButton;
        private System.Windows.Forms.Label lblSelectProfile;
        private System.Windows.Forms.ComboBox ddbActivationMode;
        private System.Windows.Forms.Label lblActivationMode;
        private System.Windows.Forms.Label lblMaxDelay;
        private System.Windows.Forms.Label lblMaxCPS;
        private System.Windows.Forms.NumericUpDown numMaxDelay;
        private System.Windows.Forms.Label lblMinDelay;
        private System.Windows.Forms.Button btnManageProfiles;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label lblAutoclickerEnabled;
        private System.Windows.Forms.Label lblAutoclickerRunning;
        private System.Windows.Forms.PictureBox pbAutoclickerRunning;
        private System.Windows.Forms.PictureBox pbAutoclickerEnabled;
        private System.Windows.Forms.Button btnActivationButton;
        private System.Windows.Forms.Button btnAutoclickButton;
        private System.Windows.Forms.Button btnDeactivationButton;
        private System.Windows.Forms.Label lblTurboMode;
        private System.Windows.Forms.ComboBox ddbTurboMode;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.CheckBox cbEnter;
        private System.Windows.Forms.CheckBox cbSuppressHotkeys;
        private System.Windows.Forms.CheckBox cbMute;
        private AxWMPLib.AxWindowsMediaPlayer axMedia;
        private System.ComponentModel.BackgroundWorker AutoClicker;
        private System.Windows.Forms.CheckBox cbLoadSound;
        private System.Windows.Forms.CheckBox cbAlwaysPlay;
    }
}

