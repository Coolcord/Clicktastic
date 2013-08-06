namespace Clicktastic
{
    partial class ProfileManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileManager));
            this.lbProfiles = new System.Windows.Forms.ListBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbProfiles
            // 
            this.lbProfiles.FormattingEnabled = true;
            this.lbProfiles.HorizontalScrollbar = true;
            this.lbProfiles.ItemHeight = 16;
            this.lbProfiles.Items.AddRange(new object[] {
            "Mass Paste",
            "Speedy Click"});
            this.lbProfiles.Location = new System.Drawing.Point(13, 12);
            this.lbProfiles.Name = "lbProfiles";
            this.lbProfiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbProfiles.Size = new System.Drawing.Size(292, 212);
            this.lbProfiles.Sorted = true;
            this.lbProfiles.TabIndex = 0;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(13, 230);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(93, 35);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(112, 230);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(94, 35);
            this.btnRename.TabIndex = 2;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(212, 230);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(93, 35);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 277);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.lbProfiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProfileManager";
            this.Text = "Profile Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbProfiles;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDelete;
    }
}