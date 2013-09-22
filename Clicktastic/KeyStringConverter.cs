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
using System.Text;
using System.Windows.Forms;

namespace Clicktastic
{
    class KeyStringConverter
    {
        KeysConverter converter = new KeysConverter();

        //
        // KeyToCmd(Keys key, Keys modifiers, Boolean enter, int turbo)
        // Converts a key to proper SendWait command syntax
        //
        public string KeyToCmd(Keys key, Keys modifiers, Boolean enter, int turbo)
        {
            string cmd = KeyToString(key); //convert the key to a string first
            if (cmd == null)
                return null; //unable to get a string from the key
            if (cmd.Length > 1)
                cmd = cmd.ToUpper(); //make the string all uppercase for the command

            switch (cmd) //attempt to parse the string to a command
            {
                case "CTRL":
                    cmd = "^";
                    break;
                case "SHIFT":
                    cmd = "+";
                    break;
                case "ALT":
                    cmd = "%";
                    break;
                case "PAGEUP":
                    cmd = "PGUP";
                    break;
                case "PAGEDOWN":
                    cmd = "PGDN";
                    break;
                case "` (~)":
                    cmd = "`";
                    break;
            }

            //Add turbo as specified
            if (cmd == "NONE")
            {
                if (turbo > 1)
                    cmd = turbo.ToString(); //build the command with turbo
                else
                    cmd = ""; //build the command
            }
            else if (cmd == "^" || cmd == "+" || cmd == "%")
            {
                if (turbo > 1)
                    cmd = cmd + " " + turbo; //build the command with turbo
            }
            else
            {
                if (turbo > 1)
                    cmd = "{" + cmd + " " + turbo + "}"; //build the command with turbo
                else
                    cmd = "{" + cmd + "}"; //build the command
            }

            //Add any necessary modifiers
            if ((modifiers & Keys.Control) > 0)
            {
                if (cmd.Length == 0)
                    cmd = "^";
                else
                    cmd = "^(" + cmd + ")";
            }
            if ((modifiers & Keys.Shift) > 0)
            {
                if (cmd.Length == 0)
                    cmd = "+";
                else
                    cmd = "+(" + cmd + ")";
            }
            if ((modifiers & Keys.Alt) > 0)
            {
                if (cmd.Length == 0)
                    cmd = "%";
                else
                    cmd = "%(" + cmd + ")";
            }

            if (enter) //add enter to the command
                cmd = cmd + "{ENTER}";

            return cmd;
        }

        //
        // KeyToString(Keys key)
        // Converts a key to a string
        //
        public String KeyToString(Keys key)
        {
            switch (key)
            {
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                    return "Ctrl";
                case Keys.Alt:
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                    return "Alt";
                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    return "Shift";
                case Keys.Add:
                    return "NumPad+";
                case Keys.Decimal:
                case Keys.OemPeriod:
                    return ".";
                case Keys.Divide:
                case Keys.OemQuestion:
                    return "/";
                case Keys.Multiply:
                    return "*";
                case Keys.OemBackslash:
                    return "\\";
                case Keys.OemCloseBrackets:
                    return "]";
                case Keys.OemMinus:
                case Keys.Separator:
                    return "-";
                case Keys.OemOpenBrackets:
                    return "[";
                case Keys.OemPipe:
                    return "|";
                case Keys.OemQuotes:
                    return "\"";
                case Keys.OemSemicolon:
                    return ";";
                case Keys.Oemcomma:
                    return ",";
                case Keys.Oemplus:
                    return "=";
                case Keys.Oemtilde:
                    return "` (~)";
                case Keys.Subtract:
                    return "NumPad-";
                case Keys.D0:
                    return "0";
                case Keys.D1:
                    return "1";
                case Keys.D2:
                    return "2";
                case Keys.D3:
                    return "3";
                case Keys.D4:
                    return "4";
                case Keys.D5:
                    return "5";
                case Keys.D6:
                    return "6";
                case Keys.D7:
                    return "7";
                case Keys.D8:
                    return "8";
                case Keys.D9:
                    return "9";
                case Keys.Space:
                    return " ";
                case Keys.Back:
                    return "Backspace";
                case Keys.CapsLock:
                    return "CapsLock";
                case Keys.Insert:
                    return "Insert";
                case Keys.Delete:
                    return "Delete";
                case Keys.PageUp:
                    return "PageUp";
                case Keys.PageDown:
                    return "PageDown";
                default:
                    try
                    { //string was not one of the defined methods, so try using the key converter
                        string value = converter.ConvertToString(key);
                        if (value.Length == 1) //value is a single letter
                            value = value.ToLower(); //make it lowercase
                        return value;
                    }
                    catch
                    { //key converter failed, so return null
                        return null;
                    }
            }
        }

        //
        // StringToKey(string key)
        // Converts a string to a key
        //
        public Keys StringToKey(string key)
        {
            switch (key) //attempt to parse the string to a key
            {
                case "Ctrl":
                    return Keys.ControlKey;
                case "Shift":
                    return Keys.ShiftKey;
                case "Alt":
                    return Keys.Menu;
                case "/":
                    return Keys.Divide;
                case "*":
                    return Keys.Multiply;
                case "\\":
                    return Keys.OemBackslash;
                case "]":
                    return Keys.OemCloseBrackets;
                case "-":
                    return Keys.OemMinus;
                case "[":
                    return Keys.OemOpenBrackets;
                case ".":
                    return Keys.OemPeriod;
                case "|":
                    return Keys.OemPipe;
                case "\"":
                    return Keys.OemQuotes;
                case ";":
                    return Keys.OemSemicolon;
                case ",":
                    return Keys.Oemcomma;
                case "NumPad-":
                    return Keys.Subtract;
                case "NumPad+":
                    return Keys.Add;
                case "+":
                    return Keys.Oemplus;
                case "` (~)":
                    return Keys.Oemtilde;
                case "`":
                    return Keys.Oemtilde;
                case "0":
                    return Keys.D0;
                case "1":
                    return Keys.D1;
                case "2":
                    return Keys.D2;
                case "3":
                    return Keys.D3;
                case "4":
                    return Keys.D4;
                case "5":
                    return Keys.D5;
                case "6":
                    return Keys.D6;
                case "7":
                    return Keys.D7;
                case "8":
                    return Keys.D8;
                case "9":
                    return Keys.D9;
                case " ":
                    return Keys.Space;
                case "Backspace":
                    return Keys.Back;
                case "CapsLock":
                    return Keys.CapsLock;
                case "Insert":
                    return Keys.Insert;
                case "Delete":
                    return Keys.Delete;
                case "PageUp":
                    return Keys.PageUp;
                case "PageDown":
                    return Keys.PageDown;
                default:
                    try
                    { //string was not one of the defined methods, so try using the key converter
                        return (Keys)converter.ConvertFromString(key);
                    }
                    catch
                    { //key converter failed, so return no keys
                        return Keys.None;
                    }
            }
        }
    }
}
