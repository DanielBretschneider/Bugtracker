using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Bugtracker.Configuration;
using Bugtracker.Console;
using Bugtracker.Globals_and_Information;
using Bugtracker.Logging;

namespace Bugtracker.Capture.Screen
{
    /// <summary>
    /// This class handles everthing related to 
    /// taking screenshots
    /// 
    /// TODO: Test with multiple screens
    /// </summary>
    public class ScreenCaptureHandler
    {
        private int CurrentNumberInSequence = 1;
        /// <summary>
        /// public constructor
        /// </summary>
        public ScreenCaptureHandler()
        {
            Logger.Log("ScreenCaptureHandler objet was created.", (LoggingSeverity)2);
        }


        /// <summary>
        /// Assamble screenshot image file name
        /// 
        /// PCName_data_time.jpg
        /// f.e. PC01_0102021_081243.jpg
        /// for PC01 01.02.2021 08:12:43
        /// </summary>
        private string BuildScreenShotFileName()
        {
            // log info
            Logger.Log("Building filename for screenshot file(s).", (LoggingSeverity)2);

            // PcInfo object
            PCInfo pcinfo = RunningConfiguration.GetInstance().PcInfo;

            // start with hostname
            string filename = "screenshot_" + pcinfo.GetHostname();

            // build date format
            DateTime dt = DateTime.Now; // Or whatever
            string date = dt.ToString("_dd-MM-yy_HH-mm-ss");

            // finished filename
            filename += date + Globals.SCREENSHOT_FILE_FORMAT;
            Logger.Log("Finished filename: " + filename, (LoggingSeverity)2);

            // return filename
            return filename;
        }

        public void GenerateScreenshotInSequence()
        {
            CurrentNumberInSequence++;
            BugtrackConsole.Print("Screenshot Number" + CurrentNumberInSequence + "captured.");
            GenerateScreenshots(RunningConfiguration.GetInstance().BugtrackerFolderName, true);
        }


        /// <summary>
        /// This method is responsible for
        /// generating the screenshots for
        /// the current bugtrack
        /// </summary>
        /// <param name="bugtrackFolderName"></param>
        /// <returns>Screenshot Path</returns>
        public string GenerateScreenshots(string bugtrackFolderName, bool sequence = false)
        {
            CurrentNumberInSequence++;
            // log info
            Logger.Log("Generating screenshot now", (LoggingSeverity)2);

            // get file name for screenshot file
            string screenShotFileName = BuildScreenShotFileName();

            try
            {
                // Determine the size of the "virtual screen", which includes all monitors.
                int screenLeft = SystemInformation.VirtualScreen.Left;
                int screenTop = SystemInformation.VirtualScreen.Top;
                int screenWidth = SystemInformation.VirtualScreen.Width;
                int screenHeight = SystemInformation.VirtualScreen.Height;
                Logger.Log("Detected eges (left, top, width, height) = (" + screenLeft + ", " + screenTop + ", " + screenWidth + ", " + screenHeight, (LoggingSeverity)2);

                // Create a bitmap of the appropriate size to receive the screenshot.
                using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
                {
                    // Draw the screenshot into our bitmap.
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                    }

                    if (sequence)
                        bmp.Save(bugtrackFolderName + @"\" + screenShotFileName + "sequence_" + CurrentNumberInSequence, ImageFormat.Jpeg);
                    else
                        // Do something with the Bitmap here, like save it to a file:
                        bmp.Save(bugtrackFolderName + @"\" + screenShotFileName, ImageFormat.Jpeg);
                }

                Logger.Log("Screenshot generated and saved at '" + screenShotFileName + "'", (LoggingSeverity)2);
            }
            catch (Exception e)
            {
                Logger.Log("There was an error taking screenshots of the clients monitors.", 0);
                Logger.Log(e.Message, 0);
            }

            return screenShotFileName;

        }

    }
}
