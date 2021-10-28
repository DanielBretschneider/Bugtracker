using bugracker.Target;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using System;
using System.IO;

namespace Bugtracker.GlobalsInformation
{

    class RunningConfiguration : Singleton<RunningConfiguration>
    {
        public ApplicationManager   ApplicationManager { get; set; } 
        public TargetManager        TargetManager { get; set; }
        /// <summary>
        /// PC Info Object
        /// </summary>
        public  PCInfo              PCInfo { get; protected set;  }
        public  string              BugtrackerFolderName { get; set; }

        public bool                 LoggerEnabled { get; set; }
        public  LoggingSeverity     LogSeverity { get; protected set; }
        public string               TargetPath { get; protected set; }
        public DirectoryInfo BugtrackerFolder { get; internal set; }

        public RunningConfiguration()
        {
            BugtrackerFolderName    = "not set yet";
            TargetPath              = "not set yet";

            LoggerEnabled = ConfigHandler.IsLoggingEnabled();
            LogSeverity = ConfigHandler.GetLoggingSeverity();
            PCInfo = new PCInfo();
            ApplicationManager = new ApplicationManager();

        }

        public override string ToString()
        {
            string returnString = "";

            returnString += "PCInfo: \n \n";
            returnString += PCInfo.GetPCInformationSummary() + Environment.NewLine;
            returnString += "Bugtracker Folder Name: " + BugtrackerFolderName + Environment.NewLine;
            returnString += "Logger Enabled: " + LoggerEnabled + Environment.NewLine;
            returnString += "Log Severity: " + Enum.GetName(typeof(LoggingSeverity), LogSeverity) + Environment.NewLine;
            returnString += "Target Path: " + TargetPath;

            return returnString;
        }
    }
}
