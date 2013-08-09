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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using AxWMPLib;

namespace Clicktastic
{
    class SoundEffects
    {
        //Import DLLs
        [DllImport("winmm.dll")]
        private static extern uint mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);

        //Define Global Variables
        private AxWindowsMediaPlayer media = null;
        private Boolean stopped = true;
        private int LoopLength = 0;
        private int MaxTurboLength = 0;
        private int Start1Length = 0;
        private int Start2Length = 0;
        private int StopLength = 0;
        private Semaphore soundSemaphore = null;
        private Semaphore mediaSemaphore = null;
        private System.Media.SoundPlayer sound = null;
        private static string currentDirectory = Directory.GetCurrentDirectory() + "\\Sounds";
        private Thread soundThread = null;

        //Constructor
        public SoundEffects(ref AxWindowsMediaPlayer axMedia, ref Semaphore soundSem, ref Semaphore mediaSem, ref Boolean stop)
        {
            //Assign variables
            soundSemaphore = soundSem;
            mediaSemaphore = mediaSem;
            media = axMedia;
            media.settings.setMode("loop", false);
            stopped = stop;

            //Determine if sound folder already exists
            if (!Directory.Exists(currentDirectory))
            {
                try
                {
                    Directory.CreateDirectory(currentDirectory); //if not, create it
                } catch { }
            }
            //Get the length of every sound file
            MaxTurboLength = GetSoundLength(currentDirectory + "\\MaxTurbo.wav") + 1000;
            Start1Length = GetSoundLength(currentDirectory + "\\Start1.wav") + 1000;
            Start2Length = GetSoundLength(currentDirectory + "\\Start2.wav") + 1000;
            LoopLength = GetSoundLength(currentDirectory + "\\Loop.wav") + 1000;
            StopLength = GetSoundLength(currentDirectory + "\\Stop.wav") + 1000;
            sound = new System.Media.SoundPlayer(currentDirectory + "\\MaxTurbo.wav");
        }

        #region Public Functions
        //===========================================================================
        //
        // Public Functions
        //
        //===========================================================================

        //
        // PlayEffect()
        // Plays the MaxTurbo.wav sound effect
        //
        public void PlayEffect()
        {
            sound.SoundLocation = currentDirectory + "\\MaxTurbo.wav";
            try
            { //Only play the effect if it exists
                if (!File.Exists(currentDirectory + "\\MaxTurbo.wav"))
                    throw new Exception("MaxTurbo.wav not found!");
                sound.Play();
            } catch { }
        }

        //
        // PlayLoop()
        // Starts playing the sound effect loop
        //
        public void PlayLoop()
        {
            stopped = false; //signal that the loop has started
            if (soundThread == null)
                soundThread = new Thread(() => RunLoop());
            else
            {
                try
                {
                    soundThread.Abort(); //try to abort the thread if it already exists
                } catch { }
                soundThread = new Thread(() => RunLoop());
            }
            try
            { //start running the new thread
                soundThread.Start();
            } catch { }
        }

        //
        // Stop()
        // Stops playing the sound effect loop
        //
        public void Stop()
        {
            stopped = true; //signal that loop is being stopped
            try
            {
                mediaSemaphore.Release();
            } catch { }
            media.Ctlcontrols.stop();
            sound.SoundLocation = currentDirectory + "\\Stop.wav";
            if (!stopped) //another thread started playing before the stop sound could be played
                return; //don't play the sound
            try
            { //Only play the effect if it exists
                if (!File.Exists(currentDirectory + "\\Stop.wav"))
                    throw new Exception("Stop.wav not found!");
                sound.Play();
            } catch { }
        }
        #endregion

        #region Private Functions
        //===========================================================================
        //
        // Private Functions
        //
        //===========================================================================

        //
        // GetSoundLength(string fileName)
        // Returns the sound length of a wav file
        //
        private static int GetSoundLength(string fileName)
        {
            StringBuilder lengthBuf = new StringBuilder(32);

            mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
            mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
            mciSendString("close wave", null, 0, IntPtr.Zero);

            int length = 0;
            int.TryParse(lengthBuf.ToString(), out length);

            return length; //length in milliseconds
        }

        //
        // RunLoop()
        // Runs the sound effect loop
        //
        private void RunLoop()
        {
            try
            { //Try to play the sound effect loop
                mediaSemaphore.WaitOne(100);
                media.URL = currentDirectory + "\\Start1.wav";
                try
                { //Only play the effect if it exists
                    if (!File.Exists(currentDirectory + "\\Start1.wav"))
                        throw new Exception("Start1.wav not found!");
                    media.Ctlcontrols.play();
                }
                catch
                {
                    mediaSemaphore.Release();
                }
                if (stopped)
                {
                    try
                    {
                        soundSemaphore.Release();
                    }
                    catch { }
                    try
                    {
                        mediaSemaphore.Release(); //make sure that the media semaphore gets released as well in case of a race condition
                    }
                    catch { }
                    media.Ctlcontrols.stop();
                    return;
                }

                mediaSemaphore.WaitOne(Start1Length); //wait for the intro sound to finish
                try
                {
                    soundSemaphore.Release(); //let the autoclicker start
                }
                catch { }
                media.URL = currentDirectory + "\\Start2.wav";
                try
                { //Only play the effect if it exists
                    if (!File.Exists(currentDirectory + "\\Start2.wav"))
                        throw new Exception("Start2.wav not found!");
                    media.Ctlcontrols.play();
                }
                catch
                {
                    mediaSemaphore.Release();
                }
                if (stopped)
                {
                    media.Ctlcontrols.stop();
                    return;
                }

                mediaSemaphore.WaitOne(Start2Length); //wait for the intro to the loop to finish
                sound.SoundLocation = currentDirectory + "\\Loop.wav";
                try
                { //Only play the effect if it exists
                    if (!File.Exists(currentDirectory + "\\Loop.wav"))
                        throw new Exception("Loop.wav not found!");
                    sound.PlayLooping(); //start playing the looping sound
                }
                catch { }
            }
            catch
            {
                //Make sure the semaphores get released
                try
                {
                    soundSemaphore.Release();
                }
                catch { }
                try
                {
                    mediaSemaphore.Release();
                }
                catch { }
            }
        }
        #endregion
    }
}
