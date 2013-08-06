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
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Clicktastic
{
    class CheatCode
    {
        int cheatCode = 0;

        public void GetCheatCode(object sender, KeyEventArgs e)
        {
            switch (cheatCode)
            {
                case 0:
                    if (e.KeyCode == Keys.A) cheatCode++;
                    else cheatCode = 0;
                    break;
                case 1:
                    if (e.KeyCode == Keys.B) cheatCode++;
                    else cheatCode = 0;
                    break;
                case 2:
                    if (e.KeyCode == Keys.B) cheatCode++;
                    else cheatCode = 0;
                    break;
                case 3:
                    if (e.KeyCode == Keys.A)
                    {
                        //Play the Mario theme in a separate thread
                        MarioTheme mario = new MarioTheme();
                        Thread marioTheme = new Thread(new ThreadStart(mario.Play));
                        marioTheme.Start();
                        cheatCode = 0;
                    }
                    else
                        cheatCode = 0;
                    break;
            }
        }
    }

    public class MarioTheme
    {
        public void Play()
        {
            Console.Beep(659, 125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(523, 125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(784, 125);
            Thread.Sleep(375);
            Console.Beep(392, 125);
        }
    }
}
