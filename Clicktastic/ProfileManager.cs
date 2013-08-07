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
        Profile profile = new Profile();

        public ProfileManager()
        {
            InitializeComponent();
        }

        private string GetName(string text, string oldName)
        {
            string name = null;
            Form nameForm = new Form() { FormBorderStyle = FormBorderStyle.FixedSingle, MinimizeBox = false, MaximizeBox = false };
            nameForm.StartPosition = FormStartPosition.CenterParent;
            nameForm.Width = 200;
            nameForm.Height = 110;
            nameForm.Text = "Clicktastic";
            nameForm.Icon = Properties.Resources.clicktastic;

            Label lblName = new Label()
            {
                Width = 190,
                Height = 20,
                Location = new Point(5, 2),
                Text = text
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
                Text = "OK"
            };
            btnOK.Click += (btnOKSender, btnOKe) =>
            {
                name = tbName.Text;
                nameForm.Close();
            };

            Button btnCancel = new Button()
            {
                Width = 92,
                Height = 25,
                Location = new Point(98, 48),
                ImageAlign = ContentAlignment.MiddleCenter,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Cancel"
            };
            btnCancel.Click += (btnCancelSender, btnCancele) =>
            {
                nameForm.Close();
            };

            nameForm.AcceptButton = btnOK;
            nameForm.CancelButton = btnCancel;
            nameForm.Controls.Add(lblName);
            nameForm.Controls.Add(tbName);
            nameForm.Controls.Add(btnOK);
            nameForm.Controls.Add(btnCancel);
            nameForm.ShowDialog();
            btnCancel.Dispose();
            btnOK.Dispose();
            tbName.Dispose();
            lblName.Dispose();
            nameForm.Dispose();

            return name;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string name = GetName("Enter a name:", "");
            if (name != null)
            {
                //create file here
                lbProfiles.Items.Add(name);
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            int selected = lbProfiles.SelectedItems.Count;
            string[] names = new string[selected];
            int i = 0;
            for (i = 0; i < selected; i++)
            {
                names[i] = lbProfiles.GetItemText(lbProfiles.SelectedItems[i]);
            }
            if (selected > 0)
            {
                foreach (string name in names)
                {
                    try
                    {
                        string newName = GetName("Enter a new name:", name);
                        if (newName != null)
                        {
                            //rename file here
                            lbProfiles.Items.Remove(name);
                            lbProfiles.Items.Add(newName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        MessageBox.Show("Unable to rename " + name + "!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        i++;
                    }
                }
            }
            else
            {
                MessageBox.Show("No profile is selected!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selected = lbProfiles.SelectedItems.Count;
            string[] names = new string[selected];
            int i = 0;
            for (i = 0; i < selected; i++)
            {
                names[i] = lbProfiles.GetItemText(lbProfiles.SelectedItems[i]);
            }
            if (selected > 0)
            {
                DialogResult result = DialogResult.No;
                if (selected == 1)
                    result = MessageBox.Show("Are you sure you want to delete " + names[0] + "?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                else
                    result = MessageBox.Show("Are you sure you want to delete the selected profiles?", "Clicktastic", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    i = 0;
                    foreach (string name in names)
                    {
                        try
                        {
                            //delete file here
                            //File.SetAttributes(name, FileAttributes.Normal);
                            //File.Delete(name);
                            lbProfiles.Items.Remove(name);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            MessageBox.Show("Unable to delete " + name + "!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            i++;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No profile is selected!", "Clicktastic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
