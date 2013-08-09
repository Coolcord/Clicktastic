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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO.Compression;

namespace Clicktastic
{
    class Profile
    {
        //Define Global Variables
        public static string currentDirectory = Directory.GetCurrentDirectory() + "\\Profiles";
        public static int KeySize = Properties.Settings.Default.KeySize;

        #region Public Functions
        //===========================================================================
        //
        // Public Functions
        //
        //===========================================================================

        //
        // Load(string name, ref Clicktastic.ProfileData profile)
        // Attempts to load a profile from a file into a ProfileData structure
        //
        public Boolean Load(string name, ref Clicktastic.ProfileData profile)
        {
            Boolean success = false;
            BinaryReader b = null;
            try
            {
                //Open the file
                b = new BinaryReader(File.Open(currentDirectory + "\\" + name + ".clk", FileMode.Open));

                //Check the header
                if (b.ReadString() != "Clicktastic Profile")
                {
                    success = false; //header was invalid
                    throw new Exception("File is not a Clicktastic profile!");
                }

                //Load the KEYCOMBO structures
                if (!LoadKEYCOMBO(b, ref profile.ActivationKey))
                {
                    success = false;
                    throw new Exception("ActivationKey could not be loaded!");
                }
                if (!LoadKEYCOMBO(b, ref profile.DeactivationKey))
                {
                    success = false;
                    throw new Exception("DeactivationKey could not be loaded!");
                }
                if (!LoadKEYCOMBO(b, ref profile.AutoclickKey))
                {
                    success = false;
                    throw new Exception("AutoclickKey could not be loaded!");
                }

                //Load all other profile data
                profile.Random = b.ReadBoolean();
                profile.Hold = b.ReadBoolean();
                profile.pressEnter = b.ReadBoolean();
                profile.useDeactivationKey = b.ReadBoolean();
                profile.suppressHotkeys = b.ReadBoolean();
                profile.mute = b.ReadBoolean();
                profile.loadSound = b.ReadBoolean();
                profile.alwaysPlay = b.ReadBoolean();
                profile.turbo = b.ReadInt32();
                profile.MinDelay = b.ReadInt32();
                profile.MaxDelay = b.ReadInt32();

                success = true;
            }
            catch
            { //something went wrong
                success = false;
            }
            finally
            {
                if (b != null)
                {
                    b.Close(); //all done with the file
                    b.Dispose();
                }
            }
            return success;
        }

        //
        // Save(string name, ref Clicktastic.ProfileData profile)
        // Attempts to save a profile to a file from a ProfileData structure
        //
        public Boolean Save(string name, ref Clicktastic.ProfileData profile)
        {
            Boolean success = false;
            BinaryWriter b = null;
            try
            {
                if (!Directory.Exists(currentDirectory)) //make sure the profile directory exists
                    Directory.CreateDirectory(currentDirectory); //if not, create it

                //Open the file
                b = new BinaryWriter(File.Open(currentDirectory + "\\" + name + ".clk", FileMode.OpenOrCreate));

                //Write the header
                b.Write("Clicktastic Profile");

                //Save the KEYCOMBO structures
                if (!SaveKEYCOMBO(b, profile.ActivationKey))
                    throw new Exception("ActivationKey could not be saved!");
                if (!SaveKEYCOMBO(b, profile.DeactivationKey))
                    throw new Exception("DeactivationKey could not be saved!");
                if (!SaveKEYCOMBO(b, profile.AutoclickKey))
                    throw new Exception("AutoclickKey could not be saved!");

                //Save all other profile data
                b.Write(profile.Random);
                b.Write(profile.Hold);
                b.Write(profile.pressEnter);
                b.Write(profile.useDeactivationKey);
                b.Write(profile.suppressHotkeys);
                b.Write(profile.mute);
                b.Write(profile.loadSound);
                b.Write(profile.alwaysPlay);
                b.Write(profile.turbo);
                b.Write(profile.MinDelay);
                b.Write(profile.MaxDelay);

                success = true;
            }
            catch
            { //something went wrong
                success = false;
            }
            finally
            {
                if (b != null)
                {
                    b.Close(); //all done with the file
                    b.Dispose();
                }
            }
            return success;
        }
        #endregion

        #region Private Functions
        //===========================================================================
        //
        // Private Functions
        //
        //===========================================================================

        //
        // GetBytes(Keys key)
        // Converts a key to a byte array
        //
        private static byte[] GetBytes(Keys key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            binaryFormatter.Serialize(ms, key);
            KeySize = ms.ToArray().Count(); //get the key size
            Properties.Settings.Default.KeySize = KeySize;
            Properties.Settings.Default.Save(); //save it for when the next load occurs
            return ms.ToArray();
        }

        //
        // GetKey(byte[] bytes)
        // Converts a byte array to a key
        //
        private static Keys GetKey(byte[] bytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return (Keys)obj;
        }

        //
        // LoadKEYCOMBO(BinaryReader b, ref Clicktastic.KEYCOMBO key)
        // Attempts to load a KEYCOMBO structure from a file
        //
        private Boolean LoadKEYCOMBO(BinaryReader b, ref Clicktastic.KEYCOMBO key)
        {
            try
            {
                key.valid = b.ReadBoolean();
                if (!key.valid) //key must be valid
                    throw new Exception("KEYCOMBO is not valid!");
                key.isKeyboard = b.ReadBoolean();
                key.modifierKeys = GetKey(b.ReadBytes(KeySize));
                key.key = GetKey(b.ReadBytes(KeySize));
                key.keyString = b.ReadString();
                key.cmd = b.ReadString();
                key.mouseButton = b.ReadUInt32();
                key.wheel = b.ReadInt32();
            }
            catch
            {
                return false;
            }
            return true;
        }

        //
        // SaveKEYCOMBO(BinaryWriter b, Clicktastic.KEYCOMBO key)
        // Attempts to save a KEYCOMBO structure to a file
        //
        private Boolean SaveKEYCOMBO(BinaryWriter b, Clicktastic.KEYCOMBO key)
        {
            try
            {
                if (!key.valid) //key must be valid
                    throw new Exception("KEYCOMBO is not valid!");
                b.Write(key.valid);
                b.Write(key.isKeyboard);
                b.Write(GetBytes(key.modifierKeys));
                b.Write(GetBytes(key.key));
                b.Write(key.keyString);
                b.Write(key.cmd);
                b.Write(key.mouseButton);
                b.Write(key.wheel);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
