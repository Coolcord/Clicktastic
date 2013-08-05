using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clicktastic
{
    class KeyStringConverter
    {
        KeysConverter converter = new KeysConverter();

        public String KeyToString(Keys key)
        {
            switch (key)
            {
                case Keys.ControlKey:
                    return "Ctrl";
                case Keys.Control:
                    return "Ctrl";
                case Keys.LControlKey:
                    return "Ctrl";
                case Keys.RControlKey:
                    return "Ctrl";
                case Keys.Menu:
                    return "Alt";
                case Keys.LMenu:
                    return "Alt";
                case Keys.RMenu:
                    return "Alt";
                case Keys.Shift:
                    return "Shift";
                case Keys.ShiftKey:
                    return "Shift";
                case Keys.LShiftKey:
                    return "Shift";
                case Keys.RShiftKey:
                    return "Shift";
                case Keys.Add:
                    return "NumPad+";
                case Keys.Decimal:
                    return ".";
                case Keys.Divide:
                    return "/";
                case Keys.Multiply:
                    return "*";
                case Keys.OemBackslash:
                    return "\\";
                case Keys.OemCloseBrackets:
                    return "]";
                case Keys.OemMinus:
                    return "-";
                case Keys.OemOpenBrackets:
                    return "[";
                case Keys.OemPeriod:
                    return ".";
                case Keys.OemPipe:
                    return "|";
                case Keys.OemQuestion:
                    return "/";
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
                case Keys.Separator:
                    return "-";
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
                    {
                        return converter.ConvertToString(key);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return null;
                    }
            }
        }

        public Keys StringToKey(string key)
        {
            switch (key)
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
                    {
                        return (Keys)converter.ConvertFromString(key);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Keys.None;
                    }
            }
        }
    }
}
