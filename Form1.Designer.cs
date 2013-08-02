namespace Clicktastic
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblHoldInstructions = new System.Windows.Forms.Label();
            this.pbAutoclickerRunning = new System.Windows.Forms.PictureBox();
            this.pbAutoclickerEnabled = new System.Windows.Forms.PictureBox();
            this.lblAutoclickerRunning = new System.Windows.Forms.Label();
            this.lblAutoclickerEnabled = new System.Windows.Forms.Label();
            this.lblSpeedInstructions = new System.Windows.Forms.Label();
            this.lblDeactivationInstructions = new System.Windows.Forms.Label();
            this.lblActivationInstructions = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblTurboMode = new System.Windows.Forms.Label();
            this.ddbTurboMode = new System.Windows.Forms.ComboBox();
            this.btnAutoclickButton = new System.Windows.Forms.Button();
            this.btnDeactivationButton = new System.Windows.Forms.Button();
            this.btnActivationButton = new System.Windows.Forms.Button();
            this.btnManageProfiles = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ddbActivationMode = new System.Windows.Forms.ComboBox();
            this.lblActivationMode = new System.Windows.Forms.Label();
            this.lblMaxSpeed = new System.Windows.Forms.Label();
            this.lblMaxCPS = new System.Windows.Forms.Label();
            this.numMaxSpeed = new System.Windows.Forms.NumericUpDown();
            this.lblMinSpeed = new System.Windows.Forms.Label();
            this.ddbSpeedMode = new System.Windows.Forms.ComboBox();
            this.lblMinCPS = new System.Windows.Forms.Label();
            this.numMinSpeed = new System.Windows.Forms.NumericUpDown();
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
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerRunning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerEnabled)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(529, 341);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblHoldInstructions);
            this.tabPage1.Controls.Add(this.pbAutoclickerRunning);
            this.tabPage1.Controls.Add(this.pbAutoclickerEnabled);
            this.tabPage1.Controls.Add(this.lblAutoclickerRunning);
            this.tabPage1.Controls.Add(this.lblAutoclickerEnabled);
            this.tabPage1.Controls.Add(this.lblSpeedInstructions);
            this.tabPage1.Controls.Add(this.lblDeactivationInstructions);
            this.tabPage1.Controls.Add(this.lblActivationInstructions);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(521, 312);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Autoclicker";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblHoldInstructions
            // 
            this.lblHoldInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoldInstructions.Location = new System.Drawing.Point(9, 55);
            this.lblHoldInstructions.Name = "lblHoldInstructions";
            this.lblHoldInstructions.Size = new System.Drawing.Size(499, 25);
            this.lblHoldInstructions.TabIndex = 7;
            this.lblHoldInstructions.Text = "Hold Left Click to autoclick";
            this.lblHoldInstructions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbAutoclickerRunning
            // 
            this.pbAutoclickerRunning.Image = global::Clicktastic.Properties.Resources.red_circle;
            this.pbAutoclickerRunning.ImageLocation = "";
            this.pbAutoclickerRunning.Location = new System.Drawing.Point(203, 225);
            this.pbAutoclickerRunning.Name = "pbAutoclickerRunning";
            this.pbAutoclickerRunning.Size = new System.Drawing.Size(29, 29);
            this.pbAutoclickerRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAutoclickerRunning.TabIndex = 6;
            this.pbAutoclickerRunning.TabStop = false;
            this.pbAutoclickerRunning.Click += new System.EventHandler(this.pbAutoclickerRunning_Click);
            // 
            // pbAutoclickerEnabled
            // 
            this.pbAutoclickerEnabled.Image = global::Clicktastic.Properties.Resources.green_circle;
            this.pbAutoclickerEnabled.ImageLocation = "";
            this.pbAutoclickerEnabled.Location = new System.Drawing.Point(203, 190);
            this.pbAutoclickerEnabled.Name = "pbAutoclickerEnabled";
            this.pbAutoclickerEnabled.Size = new System.Drawing.Size(29, 29);
            this.pbAutoclickerEnabled.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAutoclickerEnabled.TabIndex = 5;
            this.pbAutoclickerEnabled.TabStop = false;
            this.pbAutoclickerEnabled.Click += new System.EventHandler(this.pbAutoclickerEnabled_Click);
            // 
            // lblAutoclickerRunning
            // 
            this.lblAutoclickerRunning.AutoSize = true;
            this.lblAutoclickerRunning.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoclickerRunning.ForeColor = System.Drawing.Color.Red;
            this.lblAutoclickerRunning.Location = new System.Drawing.Point(238, 225);
            this.lblAutoclickerRunning.Name = "lblAutoclickerRunning";
            this.lblAutoclickerRunning.Size = new System.Drawing.Size(98, 29);
            this.lblAutoclickerRunning.TabIndex = 4;
            this.lblAutoclickerRunning.Text = "Waiting";
            // 
            // lblAutoclickerEnabled
            // 
            this.lblAutoclickerEnabled.AutoSize = true;
            this.lblAutoclickerEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoclickerEnabled.ForeColor = System.Drawing.Color.Lime;
            this.lblAutoclickerEnabled.Location = new System.Drawing.Point(238, 190);
            this.lblAutoclickerEnabled.Name = "lblAutoclickerEnabled";
            this.lblAutoclickerEnabled.Size = new System.Drawing.Size(106, 29);
            this.lblAutoclickerEnabled.TabIndex = 3;
            this.lblAutoclickerEnabled.Text = "Enabled";
            // 
            // lblSpeedInstructions
            // 
            this.lblSpeedInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeedInstructions.Location = new System.Drawing.Point(9, 141);
            this.lblSpeedInstructions.Name = "lblSpeedInstructions";
            this.lblSpeedInstructions.Size = new System.Drawing.Size(499, 25);
            this.lblSpeedInstructions.TabIndex = 2;
            this.lblSpeedInstructions.Text = "Autoclicker will run between 999 and 1000 clicks per second";
            this.lblSpeedInstructions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDeactivationInstructions
            // 
            this.lblDeactivationInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeactivationInstructions.Location = new System.Drawing.Point(9, 93);
            this.lblDeactivationInstructions.Name = "lblDeactivationInstructions";
            this.lblDeactivationInstructions.Size = new System.Drawing.Size(499, 25);
            this.lblDeactivationInstructions.TabIndex = 1;
            this.lblDeactivationInstructions.Text = "Press ~ to deactivate Autoclicker on Left Click";
            this.lblDeactivationInstructions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblActivationInstructions
            // 
            this.lblActivationInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivationInstructions.Location = new System.Drawing.Point(9, 38);
            this.lblActivationInstructions.Name = "lblActivationInstructions";
            this.lblActivationInstructions.Size = new System.Drawing.Size(499, 25);
            this.lblActivationInstructions.TabIndex = 0;
            this.lblActivationInstructions.Text = "Press ~ to activate Autoclicker on Left Click";
            this.lblActivationInstructions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblTurboMode);
            this.tabPage2.Controls.Add(this.ddbTurboMode);
            this.tabPage2.Controls.Add(this.btnAutoclickButton);
            this.tabPage2.Controls.Add(this.btnDeactivationButton);
            this.tabPage2.Controls.Add(this.btnActivationButton);
            this.tabPage2.Controls.Add(this.btnManageProfiles);
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.ddbActivationMode);
            this.tabPage2.Controls.Add(this.lblActivationMode);
            this.tabPage2.Controls.Add(this.lblMaxSpeed);
            this.tabPage2.Controls.Add(this.lblMaxCPS);
            this.tabPage2.Controls.Add(this.numMaxSpeed);
            this.tabPage2.Controls.Add(this.lblMinSpeed);
            this.tabPage2.Controls.Add(this.ddbSpeedMode);
            this.tabPage2.Controls.Add(this.lblMinCPS);
            this.tabPage2.Controls.Add(this.numMinSpeed);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.tbAutoclickButton);
            this.tabPage2.Controls.Add(this.lblAutoclickButton);
            this.tabPage2.Controls.Add(this.tbDeactivationButton);
            this.tabPage2.Controls.Add(this.lblDeactivationButton);
            this.tabPage2.Controls.Add(this.cbUseDeactivationButton);
            this.tabPage2.Controls.Add(this.tbActivationButton);
            this.tabPage2.Controls.Add(this.lblActivationButton);
            this.tabPage2.Controls.Add(this.lblSelectProfile);
            this.tabPage2.Controls.Add(this.ddbProfile);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(521, 312);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Preferences";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblTurboMode
            // 
            this.lblTurboMode.AutoSize = true;
            this.lblTurboMode.Location = new System.Drawing.Point(163, 208);
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
            "None",
            "Afterburners",
            "Hyperdrive",
            "Warp 10",
            "Ludicrous Speed"});
            this.ddbTurboMode.Location = new System.Drawing.Point(166, 228);
            this.ddbTurboMode.MaxDropDownItems = 2;
            this.ddbTurboMode.Name = "ddbTurboMode";
            this.ddbTurboMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddbTurboMode.Size = new System.Drawing.Size(135, 24);
            this.ddbTurboMode.TabIndex = 26;
            this.ddbTurboMode.SelectedIndexChanged += new System.EventHandler(this.ddbTurboMode_SelectedIndexChanged);
            // 
            // btnAutoclickButton
            // 
            this.btnAutoclickButton.Location = new System.Drawing.Point(108, 183);
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
            this.btnDeactivationButton.Location = new System.Drawing.Point(264, 182);
            this.btnDeactivationButton.Name = "btnDeactivationButton";
            this.btnDeactivationButton.Size = new System.Drawing.Size(42, 23);
            this.btnDeactivationButton.TabIndex = 23;
            this.btnDeactivationButton.Text = "Set";
            this.btnDeactivationButton.UseVisualStyleBackColor = true;
            this.btnDeactivationButton.Click += new System.EventHandler(this.DeactivationButton_Click);
            // 
            // btnActivationButton
            // 
            this.btnActivationButton.Location = new System.Drawing.Point(264, 111);
            this.btnActivationButton.Name = "btnActivationButton";
            this.btnActivationButton.Size = new System.Drawing.Size(42, 23);
            this.btnActivationButton.TabIndex = 22;
            this.btnActivationButton.Text = "Set";
            this.btnActivationButton.UseVisualStyleBackColor = true;
            this.btnActivationButton.Click += new System.EventHandler(this.ActivationButton_Click);
            // 
            // btnManageProfiles
            // 
            this.btnManageProfiles.Location = new System.Drawing.Point(91, 58);
            this.btnManageProfiles.Name = "btnManageProfiles";
            this.btnManageProfiles.Size = new System.Drawing.Size(133, 30);
            this.btnManageProfiles.TabIndex = 21;
            this.btnManageProfiles.Text = "Manage Profiles";
            this.btnManageProfiles.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(10, 58);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
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
            this.ddbActivationMode.Size = new System.Drawing.Size(136, 24);
            this.ddbActivationMode.TabIndex = 19;
            this.ddbActivationMode.SelectedIndexChanged += new System.EventHandler(this.ddbActivationMode_SelectedIndexChanged);
            // 
            // lblActivationMode
            // 
            this.lblActivationMode.AutoSize = true;
            this.lblActivationMode.Location = new System.Drawing.Point(7, 91);
            this.lblActivationMode.Name = "lblActivationMode";
            this.lblActivationMode.Size = new System.Drawing.Size(112, 17);
            this.lblActivationMode.TabIndex = 18;
            this.lblActivationMode.Text = "Activation Mode:";
            // 
            // lblMaxSpeed
            // 
            this.lblMaxSpeed.AutoSize = true;
            this.lblMaxSpeed.Location = new System.Drawing.Point(168, 255);
            this.lblMaxSpeed.Name = "lblMaxSpeed";
            this.lblMaxSpeed.Size = new System.Drawing.Size(145, 17);
            this.lblMaxSpeed.TabIndex = 17;
            this.lblMaxSpeed.Text = "Maximum Sleep Time:";
            // 
            // lblMaxCPS
            // 
            this.lblMaxCPS.AutoSize = true;
            this.lblMaxCPS.Location = new System.Drawing.Point(297, 279);
            this.lblMaxCPS.Name = "lblMaxCPS";
            this.lblMaxCPS.Size = new System.Drawing.Size(26, 17);
            this.lblMaxCPS.TabIndex = 16;
            this.lblMaxCPS.Text = "ms";
            // 
            // numMaxSpeed
            // 
            this.numMaxSpeed.Location = new System.Drawing.Point(171, 278);
            this.numMaxSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMaxSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxSpeed.Name = "numMaxSpeed";
            this.numMaxSpeed.Size = new System.Drawing.Size(120, 22);
            this.numMaxSpeed.TabIndex = 15;
            this.numMaxSpeed.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMaxSpeed.ValueChanged += new System.EventHandler(this.numMaxSpeed_ValueChanged);
            // 
            // lblMinSpeed
            // 
            this.lblMinSpeed.AutoSize = true;
            this.lblMinSpeed.Location = new System.Drawing.Point(7, 255);
            this.lblMinSpeed.Name = "lblMinSpeed";
            this.lblMinSpeed.Size = new System.Drawing.Size(142, 17);
            this.lblMinSpeed.TabIndex = 14;
            this.lblMinSpeed.Text = "Minimum Sleep Time:";
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
            this.ddbSpeedMode.Size = new System.Drawing.Size(135, 24);
            this.ddbSpeedMode.TabIndex = 13;
            this.ddbSpeedMode.SelectedIndexChanged += new System.EventHandler(this.ddbSpeedMode_SelectedIndexChanged);
            // 
            // lblMinCPS
            // 
            this.lblMinCPS.AutoSize = true;
            this.lblMinCPS.Location = new System.Drawing.Point(136, 279);
            this.lblMinCPS.Name = "lblMinCPS";
            this.lblMinCPS.Size = new System.Drawing.Size(26, 17);
            this.lblMinCPS.TabIndex = 12;
            this.lblMinCPS.Text = "ms";
            // 
            // numMinSpeed
            // 
            this.numMinSpeed.Location = new System.Drawing.Point(10, 275);
            this.numMinSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMinSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinSpeed.Name = "numMinSpeed";
            this.numMinSpeed.Size = new System.Drawing.Size(120, 22);
            this.numMinSpeed.TabIndex = 11;
            this.numMinSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinSpeed.ValueChanged += new System.EventHandler(this.numMinSpeed_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Speed Mode:";
            // 
            // tbAutoclickButton
            // 
            this.tbAutoclickButton.Location = new System.Drawing.Point(10, 183);
            this.tbAutoclickButton.Name = "tbAutoclickButton";
            this.tbAutoclickButton.ReadOnly = true;
            this.tbAutoclickButton.Size = new System.Drawing.Size(92, 22);
            this.tbAutoclickButton.TabIndex = 9;
            this.tbAutoclickButton.Text = "LeftClick";
            this.tbAutoclickButton.Click += new System.EventHandler(this.AutoclickButton_Click);
            this.tbAutoclickButton.TextChanged += new System.EventHandler(this.tbAutoclickButton_TextChanged);
            // 
            // lblAutoclickButton
            // 
            this.lblAutoclickButton.AutoSize = true;
            this.lblAutoclickButton.Location = new System.Drawing.Point(7, 163);
            this.lblAutoclickButton.Name = "lblAutoclickButton";
            this.lblAutoclickButton.Size = new System.Drawing.Size(129, 17);
            this.lblAutoclickButton.TabIndex = 8;
            this.lblAutoclickButton.Text = "Button to Autoclick:";
            // 
            // tbDeactivationButton
            // 
            this.tbDeactivationButton.Enabled = false;
            this.tbDeactivationButton.Location = new System.Drawing.Point(166, 183);
            this.tbDeactivationButton.Name = "tbDeactivationButton";
            this.tbDeactivationButton.ReadOnly = true;
            this.tbDeactivationButton.Size = new System.Drawing.Size(92, 22);
            this.tbDeactivationButton.TabIndex = 6;
            this.tbDeactivationButton.Text = "~";
            this.tbDeactivationButton.Click += new System.EventHandler(this.DeactivationButton_Click);
            this.tbDeactivationButton.TextChanged += new System.EventHandler(this.tbDeactivationButton_TextChanged);
            // 
            // lblDeactivationButton
            // 
            this.lblDeactivationButton.AutoSize = true;
            this.lblDeactivationButton.Enabled = false;
            this.lblDeactivationButton.Location = new System.Drawing.Point(163, 162);
            this.lblDeactivationButton.Name = "lblDeactivationButton";
            this.lblDeactivationButton.Size = new System.Drawing.Size(135, 17);
            this.lblDeactivationButton.TabIndex = 5;
            this.lblDeactivationButton.Text = "Deactivation Button:";
            // 
            // cbUseDeactivationButton
            // 
            this.cbUseDeactivationButton.AutoSize = true;
            this.cbUseDeactivationButton.Location = new System.Drawing.Point(166, 139);
            this.cbUseDeactivationButton.Name = "cbUseDeactivationButton";
            this.cbUseDeactivationButton.Size = new System.Drawing.Size(261, 21);
            this.cbUseDeactivationButton.TabIndex = 4;
            this.cbUseDeactivationButton.Text = "Use Different Button for Deactivation";
            this.cbUseDeactivationButton.UseVisualStyleBackColor = true;
            this.cbUseDeactivationButton.CheckedChanged += new System.EventHandler(this.cbUseDeactivationButton_CheckedChanged);
            // 
            // tbActivationButton
            // 
            this.tbActivationButton.Location = new System.Drawing.Point(166, 111);
            this.tbActivationButton.Name = "tbActivationButton";
            this.tbActivationButton.ReadOnly = true;
            this.tbActivationButton.Size = new System.Drawing.Size(92, 22);
            this.tbActivationButton.TabIndex = 3;
            this.tbActivationButton.Text = "~";
            this.tbActivationButton.Click += new System.EventHandler(this.ActivationButton_Click);
            this.tbActivationButton.TextChanged += new System.EventHandler(this.tbActivationButton_TextChanged);
            // 
            // lblActivationButton
            // 
            this.lblActivationButton.AutoSize = true;
            this.lblActivationButton.Location = new System.Drawing.Point(163, 91);
            this.lblActivationButton.Name = "lblActivationButton";
            this.lblActivationButton.Size = new System.Drawing.Size(118, 17);
            this.lblActivationButton.TabIndex = 2;
            this.lblActivationButton.Text = "Activation Button:";
            // 
            // lblSelectProfile
            // 
            this.lblSelectProfile.AutoSize = true;
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
            this.ddbProfile.Items.AddRange(new object[] {
            "Default"});
            this.ddbProfile.Location = new System.Drawing.Point(9, 28);
            this.ddbProfile.MaxDropDownItems = 100;
            this.ddbProfile.Name = "ddbProfile";
            this.ddbProfile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddbProfile.Size = new System.Drawing.Size(215, 24);
            this.ddbProfile.TabIndex = 0;
            this.ddbProfile.SelectedIndexChanged += new System.EventHandler(this.ddbProfile_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(523, 334);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Clicktastic";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerRunning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAutoclickerEnabled)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox ddbProfile;
        private System.Windows.Forms.ComboBox ddbSpeedMode;
        private System.Windows.Forms.Label lblMinCPS;
        private System.Windows.Forms.NumericUpDown numMinSpeed;
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
        private System.Windows.Forms.Label lblMaxSpeed;
        private System.Windows.Forms.Label lblMaxCPS;
        private System.Windows.Forms.NumericUpDown numMaxSpeed;
        private System.Windows.Forms.Label lblMinSpeed;
        private System.Windows.Forms.Button btnManageProfiles;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblActivationInstructions;
        private System.Windows.Forms.Label lblDeactivationInstructions;
        private System.Windows.Forms.Label lblSpeedInstructions;
        private System.Windows.Forms.Label lblAutoclickerEnabled;
        private System.Windows.Forms.Label lblAutoclickerRunning;
        private System.Windows.Forms.PictureBox pbAutoclickerRunning;
        private System.Windows.Forms.PictureBox pbAutoclickerEnabled;
        private System.Windows.Forms.Label lblHoldInstructions;
        private System.Windows.Forms.Button btnActivationButton;
        private System.Windows.Forms.Button btnAutoclickButton;
        private System.Windows.Forms.Button btnDeactivationButton;
        private System.Windows.Forms.Label lblTurboMode;
        private System.Windows.Forms.ComboBox ddbTurboMode;
    }
}

