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
        public static string GenerateScreenCapture(bool inSequence = false)
        {
            // Handler class for screenshots
            var screenCaptureHandler = new ScreenCaptureHandler();

            // do it
            return screenCaptureHandler.GenerateScreenshot(RunningConfiguration.GetInstance().NewestBugtrackerFolder.FullName, inSequence);
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


        /// <summary>
        /// Executes Scripts or small programs
        /// can execute ps1, bat and exe files.
        /// </summary>
        /// <param name="path"></param>

        public static void ExecuteScript(string path)
        {
            if (path.Contains(".ps1"))
            {
                var ps1File = path;
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -file \"{ps1File}\"",
                    UseShellExecute = false
                };
                Process.Start(startInfo);
            }

            if (path.Contains(".bat") || path.Contains(".exe"))
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        public static string LoadFileContentAsString(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }

        public static void DeleteContentOfDirectory(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
