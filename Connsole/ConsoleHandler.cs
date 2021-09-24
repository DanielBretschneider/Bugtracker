using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.IO;

namespace Bugtracker
{

    /// <summary>
    /// Does the console windows creation 
    /// and redirecting the text to console 
    /// window
    /// </summary>
    class ConsoleHandler
    {
        #region constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ConsoleHandler()
        {
            
        }

        #endregion // constructor


        #region public static Methods
        /// <summary>
        /// Creates a console output window, if one doesn't already exist.
        /// This window will receive all outputs from System.Console.Write()
        /// </summary>
        /// <returns>
        /// 0 if successful, else the Windows API error code from Marshal.GetLastWin32Error()
        /// </returns>
        /// <remarks>See the AllocConsole() function in the Windows API for full details.</remarks>
        public static int Create()
        {
            if (AllocConsole())
            {
                return 0;
            }
            else
            {
                return Marshal.GetLastWin32Error();
            }
        }

        /// <summary>
        /// Destroys the console window, if it exists.
        /// </summary>
        /// <returns>
        /// 0 if successful, else the Windows API error code from Marshal.GetLastWin32Error()
        /// </returns>
        /// <remarks>See the FreeConsole() function in the Windows API for full details.</remarks>
        public static int Destroy()
        {
            if (FreeConsole())
            {
                return 0;
            }
            else
            {
                return Marshal.GetLastWin32Error();
            }
        }

        #endregion  // public static Methods


        #region Private PInvokes

        /// <summary>
        /// AllocConsole() - open new console window
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// Close previously opened console window and free memory
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        internal static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion  // Private PInvokes
    }
    
    
    /// <summary>
    /// Implements the command line logic
    /// </summary>
    class BugtrackConsole
    {
        /// <summary>
        /// Global log Variable
        /// </summary>
        Logger logger = new Logger();

        /// <summary>
        /// PC Info Object
        /// </summary>
        PCInfo pcinfo = new PCInfo();

        /// <summary>
        /// PCname and date will be added later
        /// name of folder / zip file
        /// </summary>
        string bugtrackFolderName = "";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BugtrackConsole()
        {
            //nop
        }


        /// <summary>
        /// Prints text to the console
        /// </summary>
        /// <param name="msg"></param>
        public static void Print(String msg)
        {
            System.Console.Write(msg);
        }


        /// <summary>
        /// Used to fetch a new command from the user
        /// </summary>
        /// <returns></returns>
        public string GetCommand()
        {
            // define empty string
            string command = "";

            // wrap readline in try/catch to prevent
            // crashede
            try
            {
                // get next command
                Print("bugtracker > ");
                command = System.Console.ReadLine();

                // log input
                logger.log("GetCommand() - User entered command: " + command, 2);
            }
            catch (Exception e)
            {
                logger.log("GetCommand() - A problem while reading user input happend. This incident will be logged:", 0);
                logger.log(e.Message, 0);
            }
            
            // return inputed command
            return command;
        }


        /// <summary>
        /// Works like the cmd command "pause"
        /// </summary>
        public void Pause()
        {
            // empty readline
            System.Console.WriteLine("Press any key...");
            System.Console.ReadLine();

            // log
            logger.log("Pause() - activated, waiting for user to press any key.", 2);
        }


        /// <summary>
        /// Print Help Message and show + describe
        /// available commands
        /// </summary>
        private void PrintHelpMessage()
        {
            Print("Bugtracker v2.1" + Globals.EOL_CHARACTER + Globals.EOL_CHARACTER);
            PrintHelpMessageCommandDescription("help", "Show this message");
            PrintHelpMessageCommandDescription("clear", "Clear console screen");
            PrintHelpMessageCommandDescription("pcinfo", "Shows Information on PC (ip, mac, hostname, domain and user)");
            PrintHelpMessageCommandDescription("fullcapture", "Does a full capture (all logfiles and screens)");
            PrintHelpMessageCommandDescription("capture", "Has arguments -f for logfiles and -s for monitor screen shots");
            PrintHelpMessageCommandDescription("exit", "Terminate application");
            Print(Globals.EOL_CHARACTER);
        }


        /// <summary>
        /// Print formatted command
        /// number of tabs depends on command length
        /// </summary>
        private void PrintHelpMessageCommandDescription(string commandName, string commandDescription)
        {
            // print formatted command
            if (commandName.Length < 8)
                Print(commandName + "\t\t\t\t" + commandDescription + Globals.EOL_CHARACTER);
            else
                Print(commandName + "\t\t\t" + commandDescription + Globals.EOL_CHARACTER);
        }


        /// <summary>
        /// Foes a full capture
        /// equivalent to 
        /// capture -f -s
        /// </summary>
        private void FullCapture()
        {

        }


        /// <summary>
        /// Captures logfiles, screens or both
        /// </summary>
        /// <param name="argument"></param>
        private void Capture(string argument)
        {
            // create current bugtrack folder
            CreateBugtrackFolder();

            // switch 
            switch(argument)
            {
                // do screenshots
                case "-s":
                    GenerateScreenCapture();
                    Pause();
                    break;

                // default case
                default:
                    break;

            }
        }

        /// <summary>
        /// Create current bugtrack folder
        /// </summary>
        private void CreateBugtrackFolder()
        {
            // build name
            string folderName = CreateBugtrackFolderName();

            // full path 
            string fullPath = Globals.TMP_DIRECTORY + folderName;
            bugtrackFolderName = fullPath;

            // create folder
            logger.log("Creating new bugtrack folder at '" + fullPath + "'", 2);
            DirectoryInfo currentBugtrackFolder = new DirectoryInfo(fullPath);
            currentBugtrackFolder.Create();
            logger.log("Tmp folder created", 2);
        }


        /// <summary>
        /// Create Bugtracker folder name
        /// </summary>
        private string CreateBugtrackFolderName()
        {
            // first part of folder name
            string bugtrackFolderName = "Bugtracker_";

            // add pc name
            bugtrackFolderName += pcinfo.GetHostname();

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
        private void GenerateScreenCapture()
        {
            // Handler class for screenshots
            ScreenCaptureHandler screenCaptureHandler = new ScreenCaptureHandler();

            // do it
            screenCaptureHandler.GenerateScreenshots(bugtrackFolderName);
        }


        /// <summary>
        /// Print information of PC 
        /// </summary>
        private void PrintPCInfo()
        {
            // new PCInfo object
            PCInfo pcinfo = new PCInfo();

            // get pc info summary as string
            string pcInfoString = pcinfo.GetPCInformationSummary();

            // print to terminal 
            Print(pcInfoString + Globals.EOL_CHARACTER);
        }


        /// <summary>
        /// Clear Terminal screen
        /// </summary>
        private void ClearScreen()
        {
            // Works like "clear" in terminal
            // oder "cls" in cmd
            System.Console.Clear();
        }


        /// <summary>
        /// Process entered command 
        /// </summary>
        /// <param name="command"></param>
        private void ProcessCommand(string fullCommand)
        {
            // command
            string command = fullCommand;

            // arguments are f.e. -f, -s and so on
            string argument = "";

            // if fullCommand contains space " " 
            // then split command. This is needed for 
            // commands like capture -s etc.
            if (fullCommand.Contains(Globals.SPACE_CHARACTER))
            {
                string[] splittedCommand = fullCommand.Split(Globals.SPACE_CHARACTER);

                // change command and arguement to given command
                command = splittedCommand[0];
                argument = splittedCommand[1];
            }
                

            // Switch evaluates one command each time
            switch(command)
            {
                // Print help message
                case "help":
                    PrintHelpMessage();
                    break;

                // Do a full capture
                case "fullcapture":
                    FullCapture();
                    break;

                // capture logfiles, screen or both
                case "capture":
                    Capture(argument);
                    break;

                // print pc info
                case "pcinfo":
                    PrintPCInfo();
                    break;

                // clear screen
                case "clear":
                    ClearScreen();
                    break;

                // no input should be ignored and not be interpreted as command
                case "":
                    break;

                // command not found exception
                default:
                    Print("'" + command + "'" + " was not found. Write 'help' for further information." + Globals.EOL_CHARACTER);
                    break;

            }
        }

        /// <summary>
        /// Here is a while(true) loop 
        /// that lets you enter commands util 
        /// the command "exit" ist entered
        /// </summary>
        public void StartBugtrackerConsoleLogic()
        {
            // exit will stop the while true loop
            while(true)
            {
                // fetch new command
                string cmd  = GetCommand();

                // process command
                ProcessCommand(cmd);

                // always check if user wants to quit
                if (cmd.Equals("exit"))
                    return;
            }
        }

        /// <summary>
        /// Handles all exceptions and errors in program
        /// Output all noteable information to console
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            //switch(e.ExceptionObject.GetType())
            //{
            //    case System.NotImplementedException:
            //}
            Exception ex = (Exception)e.ExceptionObject;
                

            Print(e.ExceptionObject.ToString());
            Print("Press Enter to continue");
        }

    }
}

