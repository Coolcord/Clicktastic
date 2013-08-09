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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clicktastic
{
    public partial class ProfileManager : Form
    {
        //Define Global Variables
        public Clicktastic.ProfileData data;
        Profile profile = new Profile();
        public static string currentDirectory = Directory.GetCurrentDirectory() + "\\Profiles";

        //Constructor
        public ProfileManager(ref Clicktastic.ProfileData profileData)
        {
            data = profileData;
            InitializeComponent();
            foreach (string file in Directory.GetFiles(currentDirectory, "*.clk"))
            {
                lbProfiles.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        #region Interface Event Handlers
        //===========================================================================
        //
        // Interface Event Handlers
        //
        //===========================================================================

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); //close the profile manager form
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selected = lbProfiles.SelectedItems.Count;
            string[] names = new string[selected];
            int i = 0;
            for (i = 0; i < selected; i++)
            { //get selected files
                names[i] = lbProfiles.GetItemText(lbProfiles.SelectedItems[i]);
            }
            if (selected > 0)
            {
                DialogResult result = DialogResult.No;
                if (selected == 1)
                    result = MessageBox.Show("Are you sure you want to delete " + names[0] + "?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                else //more than one item was selected
                    result = MessageBox.Show("Are you sure you want to delete the selected profiles?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    foreach (string name in names) //delete all selected files
                    {
                        try
                        {
                            //Delete the file
                            File.SetAttributes(currentDirectory + "\\" + name + ".clk", FileAttributes.Normal);
                            File.Delete(currentDirectory + "\\" + name + ".clk");
                            lbProfiles.Items.Remove(name);
                        }
                        catch
                        { //unable to delete for some reason
                            MessageBox.Show("Unable to delete " + name + "!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else //the user did not select anything
            {
                MessageBox.Show("No profile is selected!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string name = null;
            while (true) //keep requesting a name until the user enters a valid one or cancels
            {
                name = GetName("Enter a new name:", "");
                if (name != null && lbProfiles.Items.Contains(name))
                    MessageBox.Show(name + " already exists!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    break; //user entered a valid name
            }
            if (name != null)
            {
                if (profile.Save(name, ref data))
                    lbProfiles.Items.Add(name);
                else //unable to save for some reason
                    MessageBox.Show("Unable to save " + name + "!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            int selected = lbProfiles.SelectedItems.Count;
            string[] names = new string[selected];
            int i = 0;
            for (i = 0; i < selected; i++)
            { //get selected files
                names[i] = lbProfiles.GetItemText(lbProfiles.SelectedItems[i]);
            }
            if (selected > 0)
            {
                foreach (string name in names) //rename all selected files
                {
                    try
                    {
                        string newName = null;
                        while (true) //keep renaming files until the user inputs a valid name or cancels
                        {
                            newName = GetName("Enter a new name:", name);
                            if (newName != null && lbProfiles.Items.Contains(newName))
                                MessageBox.Show(newName + " already exists!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                break; //valid name entered
                        }
                        if (newName != null)
                        {
                            //Rename the file
                            File.Move(currentDirectory + "\\" + name + ".clk", currentDirectory + "\\" + newName + ".clk");
                            lbProfiles.Items.Remove(name);
                            lbProfiles.Items.Add(newName);
                        }
                    }
                    catch
                    { //unable to rename for some reason
                        MessageBox.Show("Unable to rename " + name + "!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else //the user did not select anything
            {
                MessageBox.Show("No profile is selected!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Functions
        //===========================================================================
        //
        // Functions
        //
        //===========================================================================

        //
        // GetName(string text, string oldName)
        // Opens the get name form, allowing the user to input a name
        //
        private string GetName(string text, string oldName)
        {
            //Construct the form
            string name = null;
            Form nameForm = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
            nameForm.StartPosition = FormStartPosition.CenterParent;
            nameForm.Width = 200;
            nameForm.Height = 110;
            nameForm.Text = "Clicktastic";
            nameForm.Icon = Properties.Resources.clicktastic;
            nameForm.BackColor = Color.Black;

            Label lblName = new Label()
            {
                Width = 190,
                Height = 20,
                Location = new Point(5, 2),
                Text = text,
                ForeColor = Color.White
            };

            TextBox tbName = new TextBox()
            {
                Width = 185,
                Height = 20,
                Location = new Point(5, 23),
                Text = oldName
            };

            Button btnOK = new Button()
            {
                Width = 92,
                Height = 25,
                Location = new Point(5, 48),
                ImageAlign = ContentAlignment.MiddleCenter,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "OK",
                BackColor = SystemColors.ButtonFace,
                UseVisualStyleBackColor = true
            };
            btnOK.Click += (btnOKSender, btnOKe) =>
            {
                name = tbName.Text; //save the name
                nameForm.Close(); //close the form
            };

            Button btnCancel = new Button()
            {
                Width = 92,
                Height = 25,
                Location = new Point(98, 48),
                ImageAlign = ContentAlignment.MiddleCenter,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Cancel",
                BackColor = SystemColors.ButtonFace,
                UseVisualStyleBackColor = true
            };
            btnCancel.Click += (btnCancelSender, btnCancele) =>
            {
                nameForm.Close(); //close the form without saving
            };

            //Build the form and show it
            nameForm.AcceptButton = btnOK;
            nameForm.CancelButton = btnCancel;
            nameForm.Controls.Add(lblName);
            nameForm.Controls.Add(tbName);
            nameForm.Controls.Add(btnOK);
            nameForm.Controls.Add(btnCancel);
            nameForm.ShowDialog();

            //Dispose the form's elements
            btnCancel.Dispose();
            btnOK.Dispose();
            tbName.Dispose();
            lblName.Dispose();
            nameForm.Dispose();

            return name;
        }
        #endregion
    }
}
