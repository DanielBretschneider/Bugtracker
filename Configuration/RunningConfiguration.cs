using System;
using System.Collections.Generic;
using System.IO;
using Bugtracker.Globals_and_Information;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using Bugtracker.Targeting;

namespace Bugtracker.Configuration
{

    class RunningConfiguration : Singleton<RunningConfiguration>
    {
        public ApplicationManager ApplicationManager { get; set; }
        public TargetManager TargetManager { get; set; }
        /// <summary>
        /// PC Info Object
        /// </summary>
        public PCInfo PCInfo { get; protected set; }
        public string BugtrackerFolderName { get; set; }
        public bool LoggerEnabled { get; set; }
        public LoggingSeverity LogSeverity { get; protected set; }
        public string TargetPath { get; protected set; }

        private List<DirectoryInfo> bugtrackerFolders;
        public List<DirectoryInfo> BugtrackerFolders
        {
            get
            {
                return GetAllDirectorysStillExisting(bugtrackerFolders);
            }

            set
            {
                bugtrackerFolders = value;
            }
        }

        public DirectoryInfo NewestBugtrackerFolder
        {
            get
            {
                if (BugtrackerFolders.Count != 0)
                    return BugtrackerFolders[BugtrackerFolders.Count - 1];
                else
                {
                    BugtrackerFolders.Add(BugtrackerUtils.CreateBugtrackFolder());
                    return NewestBugtrackerFolder;
                }

            }

            set
            {
                BugtrackerFolders.Add(value);
            }
        }

        public RunningConfiguration()
        {
            BugtrackerFolders = new List<DirectoryInfo>();
            BugtrackerFolderName = "not set yet";
            TargetPath = "not set yet";

            LoggerEnabled = ConfigHandler.IsLoggingEnabled();
            LogSeverity = ConfigHandler.GetLoggingSeverity();
            PCInfo = new PCInfo();
            ApplicationManager = new ApplicationManager();
            TargetManager = new TargetManager();
        }

        private List<DirectoryInfo> GetAllDirectorysStillExisting(List<DirectoryInfo> toCheck)
        {
            foreach (DirectoryInfo di in toCheck)
            {
                if (di.Exists == false)
                    toCheck.Remove(di);
            }

            return toCheck;
        }
        public override string ToString()
        {
            string returnString = "";

            returnString += "PCInfo: \n \n";
            returnString += PCInfo.GetPCInformationSummary() + Environment.NewLine;
            returnString += "Current Bugtracker Folder Name: " + NewestBugtrackerFolder + Environment.NewLine;
            returnString += "Logger Enabled: " + LoggerEnabled + Environment.NewLine;
            returnString += "Log Severity: " + Enum.GetName(typeof(LoggingSeverity), LogSeverity) + Environment.NewLine;
            returnString += "Target Path: " + TargetPath;

            return returnString;
        }
    }
}
