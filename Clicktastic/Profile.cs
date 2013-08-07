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
        public static string currentDirectory = Directory.GetCurrentDirectory() + "\\Profiles";

        private static byte[] GetBytes(Keys key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] bytes = new byte[sizeof(Keys)];
            binaryFormatter.Serialize(ms, key);
            return ms.ToArray();
        }

        private static Keys GetKey(byte[] bytes)
        {
            /*
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            return (Keys)binaryFormatter.Deserialize(ms);
             * */

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Console.WriteLine(memStream.ToString());
            Object obj = (Object)binForm.Deserialize(memStream);
            return (Keys)obj;
        }

        private Boolean SaveKEYCOMBO(BinaryWriter b, Clicktastic.KEYCOMBO key)
        {
            try
            {
                if (!key.valid)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        private static string GetChecksum(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }

        public Boolean Save(string name, ref Clicktastic.ProfileData profile)
        {
            Boolean success = false;
            BinaryWriter b = null;
            try
            {
                if (!Directory.Exists(currentDirectory))
                    Directory.CreateDirectory(currentDirectory);
                b = new BinaryWriter(File.Open(currentDirectory + "\\" + name + ".clk", FileMode.OpenOrCreate));

                b.Write("Clicktastic Profile");

                if (!SaveKEYCOMBO(b, profile.ActivationKey))
                    throw new Exception("ActivationKey could not be saved!");
                if (!SaveKEYCOMBO(b, profile.DeactivationKey))
                    throw new Exception("DeactivationKey could not be saved!");
                if (!SaveKEYCOMBO(b, profile.AutoclickKey))
                    throw new Exception("AutoclickKey could not be saved!");

                b.Write(profile.Random);
                b.Write(profile.Hold);
                b.Write(profile.pressEnter);
                b.Write(profile.useDeactivationKey);
                b.Write(profile.turbo);
                b.Write(profile.MinDelay);
                b.Write(profile.MaxDelay);

                b.Close();

                /*
                string checksum = GetChecksum(currentDirectory + "\\" + name + ".clk");

                //Append checksum
                using (var fileStream = new FileStream(currentDirectory + "\\" + name + ".clk", FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new BinaryWriter(fileStream))
                {
                    bw.Write(checksum);
                }
                 * */

                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (b != null)
                {
                    b.Close();
                    b.Dispose();
                }
            }
            return success;
        }

        private Boolean LoadKEYCOMBO(BinaryReader b, ref Clicktastic.KEYCOMBO key)
        {
            try
            {
                key.valid = b.ReadBoolean();
                if (!key.valid)
                    throw new Exception("KEYCOMBO is not valid!");
                key.isKeyboard = b.ReadBoolean();
                const int SERIALIZATIONSPACE = 160;
                key.modifierKeys = GetKey(b.ReadBytes(sizeof(Keys) + SERIALIZATIONSPACE));
                key.key = GetKey(b.ReadBytes(sizeof(Keys) + SERIALIZATIONSPACE));
                key.keyString = b.ReadString();
                key.cmd = b.ReadString();
                key.mouseButton = b.ReadUInt32();
                key.wheel = b.ReadInt32();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        public Boolean Load(string name, ref Clicktastic.ProfileData profile)
        {
            Boolean success = false;
            BinaryReader b = null;
            try
            {
                b = new BinaryReader(File.Open(currentDirectory + "\\" + name + ".clk", FileMode.Open));

                if (b.ReadString() != "Clicktastic Profile")
                    throw new Exception("File is not a Clicktastic profile!");
                if (!LoadKEYCOMBO(b, ref profile.ActivationKey))
                    throw new Exception("ActivationKey could not be loaded!");
                if (!LoadKEYCOMBO(b, ref profile.DeactivationKey))
                    throw new Exception("DeactivationKey could not be loaded!");
                if (!LoadKEYCOMBO(b, ref profile.AutoclickKey))
                    throw new Exception("AutoclickKey could not be loaded!");

                profile.Random = b.ReadBoolean();
                profile.Hold = b.ReadBoolean();
                profile.pressEnter = b.ReadBoolean();
                profile.useDeactivationKey = b.ReadBoolean();
                profile.turbo = b.ReadInt32();
                profile.MinDelay = b.ReadInt32();
                profile.MaxDelay = b.ReadInt32();

                b.Close();
                 
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (b != null)
                {
                    b.Close();
                    b.Dispose();
                }
            }
            return success;
        }
    }
}
