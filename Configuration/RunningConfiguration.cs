using Bugtracker.Globals_and_Information;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using Bugtracker.Problem_Descriptors;
using Bugtracker.Targeting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Bugtracker.Configuration
{

    public enum ServerStatus
    {
        Available,
        NotAvailable
    }

    public enum ConfigSource
    {
        Server,
        Client
    }

    class RunningConfiguration : Singleton<RunningConfiguration>
    {
        public ApplicationManager ApplicationManager { get; set; }
        public TargetManager TargetManager { get; set; }
        public ProblemManager ProblemManager { get; set; }

        public ServerStatus ServerStatus { get; set; }

        public DateTime ServerLastConnectionTime { get; set; }
        public string ServerPath { get; set; }

        public ConfigSource ConfigSource { get; set; }

        /// <summary>
        /// PC Info Object
        /// </summary>
        public PCInfo PcInfo { get; protected set; }
        public string BugtrackerFolderName { get; set; }
        public bool LoggerEnabled { get; set; }
        public LoggingSeverity LogSeverity { get; protected set; }
        public string TargetPath { get; protected set; }

        public string ConfigurationFolderPath { get; protected set; }
        public DateTime StartupTime { get; protected set; }

        private List<DirectoryInfo> bugtrackerFolders;
        public List<DirectoryInfo> BugtrackerFolders
        {
            get
            {
                return GetAllDirectoriesStillExisting(bugtrackerFolders);
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
            ConfigurationFolderPath = ConfigHandler.GetConfigurationFolderPath(Globals.LOCAL_CONFIG_FILE_PATH);
            ServerPath = ConfigHandler.GetConfigurationFolderPath(Globals.LOCAL_CONFIG_FILE_PATH);

            BugtrackerFolders = new List<DirectoryInfo>();
            BugtrackerFolderName = "not set yet";
            TargetPath = "not set yet";

            PcInfo = new PCInfo();
            ApplicationManager = new ApplicationManager();
            TargetManager = new TargetManager();
            ProblemManager = new ProblemManager();

            System.Diagnostics.Debug.WriteLine("config folder path: " + ConfigurationFolderPath);
            Console.BugtrackConsole.Print("config folder path: " + ConfigurationFolderPath);

            StartupTime = DateTime.Now; ;

            InitParametersAccordingToConfigurationFiles();
            InitServerConnectionStatusTimer();
        }

        private Timer serverConnectionStatusTimer;

        public void InitServerConnectionStatusTimer()
        {
            serverConnectionStatusTimer = new Timer();
            serverConnectionStatusTimer.Tick += new EventHandler(checkServerConnectionStatus);
            serverConnectionStatusTimer.Interval = 2000;
            serverConnectionStatusTimer.Start();
        }
        
        private void checkServerConnectionStatus(object sender, EventArgs e)
        {
            if (ServerUtils.GetServerStatus() == ServerStatus.NotAvailable)
                ServerStatus = ServerStatus.NotAvailable;
            else
            {
                ServerLastConnectionTime = DateTime.Now;
                ServerStatus = ServerStatus.Available;
            }
        }

        private void InitParametersAccordingToConfigurationFiles()
        {
            string[] configPaths;

            if (Directory.Exists(ConfigurationFolderPath))
            {
                configPaths = Directory.GetFiles(ConfigurationFolderPath, "*.xml");
                ConfigSource = ConfigSource.Server;
            }
            else
            {
                configPaths = Directory.GetFiles(Globals.LOCAL_CONFIG_FILES_PATH, "*.xml");
                ConfigSource = ConfigSource.Client;
            }
                

            foreach (var filePath in configPaths)
            {
                if(ConfigSource == ConfigSource.Server)
                {
                    File.Copy(filePath, Globals.LOCAL_CONFIG_FILES_PATH + "\\" + Path.GetFileName(filePath), true);
                }

                LoggerEnabled = ConfigHandler.IsLoggingEnabled(filePath);
                LogSeverity = ConfigHandler.GetLoggingSeverity(filePath);
                ApplicationManager.Applications.AddRange(ConfigHandler.GetSpecifiedApplications(filePath));
                TargetManager.Targets.AddRange(ConfigHandler.GetSpecifiedTargets(filePath));
                ProblemManager.ProblemCategories.AddRange(ConfigHandler.GetSpecifiedProblemCategories(filePath));
            }
        }

        private static List<DirectoryInfo> GetAllDirectoriesStillExisting(List<DirectoryInfo> toCheck)
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

            returnString += "PcInfo: \n \n";
            returnString += PcInfo.GetPCInformationSummary() + Environment.NewLine;
            returnString += "Current Bugtracker Folder Name: " + NewestBugtrackerFolder + Environment.NewLine;
            returnString += "Logger Enabled: " + LoggerEnabled + Environment.NewLine;
            returnString += "Log Severity: " + Enum.GetName(typeof(LoggingSeverity), LogSeverity) + Environment.NewLine;
            returnString += "Target Path: " + TargetPath;

            return returnString;
        }
    }
}
