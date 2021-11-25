using Bugtracker.Console;
using Bugtracker.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bugtracker.Configuration;
using Bugtracker.Globals_and_Information;
using Bugtracker.Plugin;

namespace Bugtracker
{

    /// <summary>
    /// This is the starting point of 
    /// bugtracker programming project
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        private static void SetupApplication(IEnumerable<string> args)
        {
            // log start of application
            Logger.Log("Bugtracker version 2 was started", (LoggingSeverity)2);

            // create tmp folder
            CreateTempFolder();

            if(!args.Contains("-sp"))
                PluginManager.Load();
        }

        /// <summary>
        /// Creates directory in bugtrack folder where the 
        /// different logfiles and screenshots will be gathered,
        /// zipped and sent out to management server 
        /// After successfully sending the file it gets deleted
        /// </summary>
        private static void CreateTempFolder()
        {
            // check if tmp path exits
            if (!File.Exists(Globals.TMP_DIRECTORY))
            {
                Logger.Log("Creating tmp directory at '" + Globals.TMP_DIRECTORY + "'", (LoggingSeverity)2);
                DirectoryInfo tmpfolder = new DirectoryInfo(Globals.TMP_DIRECTORY);
                tmpfolder.Create();
                Logger.Log("Tmp folder created", (LoggingSeverity)2);
            }
        }


        /// <summary>
        /// This application is responsible for displaying
        /// the Bugtracker GUI. As the GUI will be developed in 
        /// Version 2.2, this method will not be called in this Version.
        /// <br />
        /// This method is only called when no command line arguments
        /// have been passed
        /// </summary>


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
        static void StartCommandLineApplication(string[] args)
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
        /// This is the main method.
        /// </summary>
        [STAThread]
        private static void Main(String[] args)
        {
            //Initialize Running Configuration Instance, before everything else
            RunningConfiguration runningConfiguration = RunningConfiguration.GetInstance();

            //sends all unhandled exception to console handler unhandled exception trapper
            System.AppDomain.CurrentDomain.UnhandledException += ConsoleHandler.UnhandledExceptionTrapper;

            // set up basic application directory 
            // and initialize logging
            SetupApplication(args);

            if (args.Contains("-sp"))
                args = new[] {""};

            // decide which Bugtracker version should be
            // executed.

            if (!runningConfiguration.HideConsole)
                StartCommandLineApplication(args);
        }
    }
}
