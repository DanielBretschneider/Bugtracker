using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bugtracker
{
    /// <summary>
    /// This class handles everthing related to 
    /// taking screenshots
    /// 
    /// TODO: test with multiple screens
    /// </summary>
    class ScreenCaptureHandler
    {
        // logging
        Logger logger = new Logger();

        /// <summary>
        /// public constructor
        /// </summary>
        public ScreenCaptureHandler()
        {
            logger.log("ScreenCaptureHandler objet was created.", 2);
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
            logger.log("Building filename for screenshot file(s).", 2);

            // PCInfo object
            PCInfo pcinfo = new PCInfo();

            // start with hostname
            string filename = "screenshot_" + pcinfo.GetHostname();

            // build date format
            DateTime dt = DateTime.Now; // Or whatever
            string date = dt.ToString("_dd-MM-yy_HH-mm-ss");

            // finished filename
            filename += date + Globals.SCREENSHOT_FILE_FORMAT;
            logger.log("Finished filename: " + filename, 2);

            // return filename
            return filename;
        }


        /// <summary>
        /// This method is responsible for
        /// generating the screenshots for
        /// the current bugtrack
        /// </summary>
        public void GenerateScreenshots(string bugtrackFolderName)
        {
            // log info
            logger.log("Generating screenshot now", 2);

            // get file name for screenshot file
            string screenShotFileName = BuildScreenShotFileName();

            try
            {
                // Determine the size of the "virtual screen", which includes all monitors.
                int screenLeft = SystemInformation.VirtualScreen.Left;
                int screenTop = SystemInformation.VirtualScreen.Top;
                int screenWidth = SystemInformation.VirtualScreen.Width;
                int screenHeight = SystemInformation.VirtualScreen.Height;
                logger.log("Detected eges (left, top, width, height) = (" + screenLeft + ", " + screenTop + ", " + screenWidth + ", " + screenHeight, 2);

                // Create a bitmap of the appropriate size to receive the screenshot.
                using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
                {
                    // Draw the screenshot into our bitmap.
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                    }

                    // Do something with the Bitmap here, like save it to a file:
                    bmp.Save(bugtrackFolderName + @"\" + screenShotFileName, ImageFormat.Jpeg);
                }

                logger.log("Screenshot generated and saved at '" + screenShotFileName + "'", 2);
            } 
            catch (Exception e)
            {
                logger.log("There was an error taking screenshots of the clients monitors.", 0);
                logger.log(e.Message, 0);
            }
            
        }

    }
}
