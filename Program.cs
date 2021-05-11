using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bugtracker
{

    /// <summary>
    /// This is the starting point of 
    /// bugtracker programming project
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Logger 
        /// </summary>
        private static Logger logger = new Logger();


        /// <summary>
        /// 
        /// </summary>
        private static void SetupApplication()
        {
            // log start of application
            logger.log("Bugtracker version 2 was started", 2);

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
                logger.log("Creating tmp directory at '" + Globals.TMP_DIRECTORY + "'", 2);
                DirectoryInfo tmpfolder = new DirectoryInfo(Globals.TMP_DIRECTORY);
                tmpfolder.Create();
                logger.log("Tmp folder created", 2);
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
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
        static void StartCommandLineApplication()
        {
            // create console window
            ConsoleHandler.Create();

            // create new bugtrackerConsole object
            BugtrackConsole console = new BugtrackConsole();

            // start console version of bugtracker
            console.StartBugtrackerConsoleLogic();

            // close console session
            ConsoleHandler.Destroy();
        }


        /// <summary>
        /// This is the main method.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            // set up basic application directory 
            // and initialize logging
            SetupApplication();

            // decide which Bugtracker version should be
            // executed.
            if (args.Length == 0)
            {
                // As no command line arguments have been
                // passed Bugtracker will be executed as 
                // GUI Application
                StartGraphicalInterfaceApplication();
            } 
            else
            {
                // Command line Arguments have been entered
                // so the application will be executed on terminal
                StartCommandLineApplication();
            }
        }
    }
}
