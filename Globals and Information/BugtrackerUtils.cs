using Bugtracker.GlobalsInformation;
using Bugtracker.Logging;
using System;
using System.IO;

namespace Bugtracker.Globals_and_Information
{
    static class BugtrackerUtils
    { 

        /// <summary>
        /// PCname and date will be added later
        /// name of folder / zip file
        /// </summary>


        /// <summary>
        /// Create current bugtrack folder
        /// </summary>
        public static DirectoryInfo CreateBugtrackFolder()
        {
            // build name
            string folderName = CreateBugtrackFolderName();

            // full path 
            string fullPath = Globals.TMP_DIRECTORY + folderName;
            RunningConfiguration.GetInstance().BugtrackerFolderName = fullPath;

            // create folder
            Logger.Log("Creating new bugtrack folder at '" + fullPath + "'", (LoggingSeverity)2);
            DirectoryInfo currentBugtrackFolder = new DirectoryInfo(fullPath);
            currentBugtrackFolder.Create();
            Logger.Log("Tmp folder created", (LoggingSeverity)2);

            return currentBugtrackFolder;
        }


        /// <summary>
        /// Create Bugtracker folder name
        /// </summary>
        public static string CreateBugtrackFolderName()
        {
            // first part of folder name
            string bugtrackFolderName = "Bugtracker_";

            // add pc name
            bugtrackFolderName += RunningConfiguration.GetInstance().PCInfo.GetHostname();

            // build date format
            DateTime dt = DateTime.Now; // Or whatever
            string date = dt.ToString("_dd-MM-yy_HH-mm-ss");

            // finish folder name
            bugtrackFolderName += date;

            // return
            return bugtrackFolderName;
        }

        /// <summary>
        /// Generates screenshots of all
        /// monitors
        /// </summary>
        /// <returns>Screen capture file path</returns>
        public static string GenerateScreenCapture()
        {
            // Handler class for screenshots
            ScreenCaptureHandler screenCaptureHandler = new ScreenCaptureHandler();

            // do it
            return screenCaptureHandler.GenerateScreenshots(RunningConfiguration.GetInstance().BugtrackerFolderName);
        }
    }
}
