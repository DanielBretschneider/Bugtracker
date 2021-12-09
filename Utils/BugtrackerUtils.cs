using System;
using System.IO;
 using Bugtracker.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
 using Bugtracker.Capture.Screen;
using Bugtracker.Configuration;
using Bugtracker.Console;
using Bugtracker.Globals_and_Information;
using Bugtracker.Logging;

namespace Bugtracker.Utils
{
    public static class BugtrackerUtils
    {
        /// <summary>
        /// Run bugtracker as CMDlet using
        /// ConsoleHandler. 
        /// 
        /// As this project is define as windows 
        /// forms application system.console.writeline (..)
        /// wont work, so we have to manually include
        /// kernel32.dll.
        /// 
        /// For more info look into ConsoleHandler.cs
        /// </summary>
        public static void StartCommandLineApplication(string[] args)
        {
            // create console window
            ConsoleHandler.Create();

            // create new bugtrackerConsole object
            //BugtrackConsole console = new BugtrackConsole();

            // start console version of bugtracker
            BugtrackConsole.StartBugtrackerConsoleLogic(args);


            // close console session
            ConsoleHandler.Destroy();
        }

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
            var bugtrackFolderName = "Bugtracker_";

            // add pc name
            bugtrackFolderName += RunningConfiguration.GetInstance().PcInfo.GetHostname();

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
            var screenCaptureHandler = new ScreenCaptureHandler();

            // do it
            return screenCaptureHandler.GenerateScreenshots(RunningConfiguration.GetInstance().BugtrackerFolderName);
        }

        /// <summary>
        /// Copys full direcotry
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);

                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (!copySubDirs) return;
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

    }
}
