using Bugtracker.Attributes;
using Bugtracker.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using Bugtracker.Console.Commands;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Console
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

            return Marshal.GetLastWin32Error();
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

            return Marshal.GetLastWin32Error();
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
        /// <summary>
        /// Handles all exceptions and errors in program
        /// Output all noteable information to console
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO: Implement out of bounds for too few arguments, ex: applications add <arg1>
            Exception ex = (Exception)e.ExceptionObject;

            //if(ex.GetType() == typeof(System.ArgumentOutOfRangeException))
            //{
            //    BugtrackConsole.Print("Too few Arguments, write <command> help for a list of all arguments");

            //}

            //BugtrackConsole.Print(ex.ToString());
            //BugtrackConsole.Print("Press Enter to continue");
        }

        #endregion  // Private PInvokes
    }


    /// <summary>
    /// Implements the command line logic
    /// </summary>
    static class BugtrackConsole
    {
        /// <summary>
        /// A Dictionairy containing a string[] with command name, and alias - and A Command Object
        /// </summary>
        public static Dictionary<string[], Command> commandRegestry = FillCommandRegistry();

        public static bool ConsoleRunning { get; set; }

        /// <summary>
        /// Prints text to the console
        /// </summary>
        /// <param name="msg"></param>
        public static void Print(String msg)
        {
            System.Console.Write(msg);
        }

        public static string GetReverseLookUpForCommand(Type commandType)
        {
            foreach (Command con in commandRegestry.Values)
            {
                if (con.GetType() == commandType)
                    return con.CommandReverse;
            }

            return null;
        }

        /// <summary>
        /// Used to fetch a new command from the user
        /// </summary>
        /// <returns></returns>
        public static string GetCommand()
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
                Logger.Log("GetCommand() - User entered command: " + command, (LoggingSeverity)2);
            }
            catch (Exception e)
            {
                Logger.Log("GetCommand() - A problem while reading user input happend. This incident will be logged:", 0);
                Logger.Log(e.Message, 0);
            }

            // return inputed command
            return command;
        }

        public static string GetCommand(string[] args)
        {
            // define empty string
            string command = "";

            for (int i = 0; i <= args.Length - 1; i++)
            {
                if (i != args.Length - 1)
                    command += args[i] + " ";
                else
                    command += args[i];

            }
            // wrap readline in try/catch to prevent
            // crashede
            try
            {
                // log input
                Logger.Log("GetCommand() - User entered command: " + command, (LoggingSeverity)2);
            }
            catch (Exception e)
            {
                Logger.Log("GetCommand() - A problem while reading user input happend. This incident will be logged:", 0);
                Logger.Log(e.Message, 0);
            }

            // return inputed command
            return command;
        }


        /// <summary>
        /// Works like the cmd command "pause"
        /// </summary>
        public static void Pause()
        {
            // empty readline
            System.Console.WriteLine("Press any key...");
            System.Console.ReadLine();

            // log
            Logger.Log("Pause() - activated, waiting for user to press any key.", (LoggingSeverity)2);
        }


        private static Dictionary<string[], Command> FillCommandRegistry()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetCustomAttribute(typeof(CommandAttribute), true) != null);
            Dictionary<string[], Command> commandReg = new Dictionary<string[], Command>();

            foreach (Type t in types)
            {
                CommandAttribute comAtr = (CommandAttribute)t.GetCustomAttribute(typeof(CommandAttribute), true);

                if (comAtr.ParentCommand == null)
                {
                    commandReg.Add(new[] { comAtr.CommandName, comAtr.CommandAlias, comAtr.CommandHelpMessage }, Command.Initialize(t));
                }
            }

            return commandReg;
        }

        private static Command InterpretCommand(string command)
        {
            foreach (var item in commandRegestry)
            {
                if (item.Key[0] == command || item.Key[1] == command)
                {
                    return item.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Process entered command 
        /// </summary>
        /// <param name="command"></param>
        private static void ProcessCommand(string fullCommand)
        {
            // command
            string command = fullCommand;

            //using arguments list for multiple arguments
            List<string> arguments = new List<string>();

            // if fullCommand contains space " " 
            // then split command. This is needed for 
            // commands like capture -s etc.
            if (fullCommand.Contains(Globals.SPACE_CHARACTER))
            {
                string[] splittedCommand = fullCommand.Split(Globals.SPACE_CHARACTER);

                // change command and arguement to given command
                command = splittedCommand[0];

                //setting arguments list from split[1] to end of array
                arguments = new List<string>(splittedCommand);
                arguments.RemoveAt(0);
            }


            //get command info from command regestry
            Command baseCommand = InterpretCommand(command);

            //check for "" input
            if (baseCommand != null)
                Print(Environment.NewLine + baseCommand.Run(arguments) + Environment.NewLine);
            else if (command != "")
                Print(Environment.NewLine + "Command \"" + command + "\" not found. Write help for a list of all available commands" + Environment.NewLine);
        }

        /// <summary>
        /// Here is a while(true) loop 
        /// that lets you enter commands util 
        /// the command "exit" ist entered
        /// </summary>
        public static void StartBugtrackerConsoleLogic(string[] args)
        {
            if (args.Length > 0)
            {
                ProcessCommand(GetCommand(args));
            }
            else
            {
                ConsoleRunning = true;
                // exit will stop the while true loop
                while (true)
                {
                    // fetch new command
                    string cmd = GetCommand();

                    // process command
                    ProcessCommand(cmd);

                    // always check if user wants to quit
                    if (!ConsoleRunning)
                        return;
                }
            }


        }

        public static class ConsoleUtilites
        {
            /// <summary>
            /// Print formatted command
            /// number of tabs depends on command length
            /// </summary>
            public static string GettHelpMessageCommandDescription(string commandName, string commandDescription)
            {
                string returnString = "";

                // print formatted command
                if (commandName.Length < 8)
                    returnString += commandName + "\t\t\t\t" + commandDescription + Globals.EOL_CHARACTER;
                else
                    returnString += commandName + "\t\t\t" + commandDescription + Globals.EOL_CHARACTER;

                return returnString;
            }
        }
    }
}

