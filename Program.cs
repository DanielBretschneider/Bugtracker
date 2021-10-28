using System;
using System.IO;
using Bugtracker.Console;
using Bugtracker.GlobalsInformation;
using Bugtracker.GUI;
using Bugtracker.Logging;

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
        private static void SetupApplication()
        {
            // log start of application
            Logger.Log("Bugtracker version 2 was started", (LoggingSeverity)2);

            // create tmp folder
            CreateTempFolder();

            // start application lookup
            // find installed applications

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
        static void StartGraphicalInterfaceApplication()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Form1());
        }


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
        static void Main(String[] args)
        {
            //Initialize Running Configuration Instance, before everything else
            RunningConfiguration runningConfiguration = RunningConfiguration.GetInstance();

            //sends all unhandled exception to console handler unhandled exception trapper
            System.AppDomain.CurrentDomain.UnhandledException += ConsoleHandler.UnhandledExceptionTrapper;

            // set up basic application directory 
            // and initialize logging
            SetupApplication();

            // decide which Bugtracker version should be
            // executed.

            if(args.Length > 0  && args[0].Contains("gui"))
                //    // As the gui command line argument has been
                //    // passed Bugtracker will be executed as 
                //    // GUI Application
                StartGraphicalInterfaceApplication();
            else
                //    // Command line Arguments have been entered
                //    // so the application will be executed on terminal
                //    StartCommandLineApplication();
                StartCommandLineApplication(args);
        }
    }
}
