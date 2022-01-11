using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using Bugtracker.Problem_Descriptors;
using Bugtracker.Targeting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;
using Bugtracker.Utils;
using Bugtracker.Variables;
using static Bugtracker.Configuration.ConfigHandler;
using Timer = System.Windows.Forms.Timer;

namespace Bugtracker.Configuration
{

    /// <summary>
    /// The current status of the Server of the running instance server connection
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Available,
        /// <summary>
        /// 
        /// </summary>
        NotAvailable
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConfigSource
    {
        Server,
        Client
    }

    public class RunningConfiguration : Singleton<RunningConfiguration>
    {

        public static event EventHandler InitiliazedRunningConfiguration;

        /// <summary>
        /// The Manager Object for modifying Applications
        /// </summary>
        public ApplicationManager ApplicationManager { get; set; }

        /// <summary>
        /// The Manager Object for modifying Targets
        /// </summary>
        public TargetManager TargetManager { get; set; }

        /// <summary>
        /// The Manager Object for modifying Problems and their categories
        /// </summary>
        public ProblemManager ProblemManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VariableManager VariableManager { get; set; }

        /// <summary>
        /// The current status of the Server, where configurations are loaded from and Captures are sent
        /// </summary>
        [Key("serverstatus")]
        public ServerStatus ServerStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key("mainserver")]
        public string ServerAddress { get; set; }

        /// <summary>
        /// The last successful connection time to the main server
        /// </summary>
        [Key("serverLastConnectionTime")]
        public DateTime ServerLastConnectionTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Key("serverPath")]
        public string ServerPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Key("configSourceType")]
        public ConfigSource ConfigSource { get; set; }

        /// <summary>
        /// PC Info Object containing useful information about the host PC
        /// </summary>
        public PCInfo PcInfo { get; protected set; }

        public ConfigHandler ConfigHandler { get; protected set; }

        /// <summary>
        /// The Main Server Object containing the ServerStatus
        /// </summary>
        public Server MainServer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool LoggerEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public LoggingSeverity LogSeverity { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string TargetPath { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public Form MainGui { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HideConsole { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [Key("configurationFolderPath")]
        public string ConfigurationFolderPath { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartupTime { get; protected set; }

        private List<DirectoryInfo> bugtrackerFolders;

        /// <summary>
        /// 
        /// </summary>
        public List<DirectoryInfo> BugtrackerFolders
        {
            get => GetAllDirectoriesStillExisting(bugtrackerFolders);

            set => bugtrackerFolders = value;
        }

        public string dateString;


        private bool isRemoteSession;

        [Key("remoteSession")]
        public bool IsRemoteSession
        {
            set => isRemoteSession = value;
            get => System.Windows.Forms.SystemInformation.TerminalServerSession;
        }

        [Key("date", true)]
        public string DateString 
        { 
            set => dateString = value;
            get => DateTime.Now.ToString("MM-dd-yyyy"); 
        }

        public string timeString;

        [Key("time", true)]
        public string TimeString 
        {
            set => timeString = value;
            get => DateTime.Now.ToString("HH-mm-ss"); 
        }

        /// <summary>
        /// 
        /// </summary>
        public DirectoryInfo NewestBugtrackerFolder
        {
            get
            {
                if (BugtrackerFolders.Count > 0)
                    return BugtrackerFolders[^1];

                BugtrackerFolders.Add(BugtrackerUtils.CreateBugtrackFolder());
                return NewestBugtrackerFolder;

            }

            set => BugtrackerFolders.Add(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public string BugtrackerFolderName => NewestBugtrackerFolder.Name;


        /// <summary>
        /// 
        /// </summary>
        public RunningConfiguration()
        {

            PcInfo = new PCInfo();
            VariableManager = new VariableManager(this);

            ConfigHandler = new ConfigHandler(this);

        }

        public void InitStartupProcedure()
        {
            OverwriteStartupConfigIfFirstStartup();

            ServerAddress = ConfigHandler.GetMainServerAddress(Globals.LOCAL_STARTUP_CONFIG_FILE_PATH);
            MainServer = new Server(ServerAddress);

            VariableManager.FullRefresh();
            ConfigurationFolderPath = ConfigHandler.GetConfigurationFolderPath();
            VariableManager.FullRefresh();
            ServerPath = ConfigHandler.GetConfigurationFolderPath();
            VariableManager.FullRefresh();

            BugtrackerFolders = new List<DirectoryInfo>();

            ApplicationManager = new ApplicationManager();
            TargetManager = new TargetManager();
            ProblemManager = new ProblemManager();

            StartupTime = DateTime.Now;

            InitParametersAccordingToConfigurationFiles();
            InitServerConnectionStatusTimer();

            InitiliazedRunningConfiguration?.Invoke(null, null);
        }

        private Timer serverConnectionStatusTimer;
        private ProblemCategory _selectedProblemCategory;

        /// <summary>
        /// 
        /// </summary>
        public void InitServerConnectionStatusTimer()
        {
            serverConnectionStatusTimer = new Timer();
            serverConnectionStatusTimer.Tick += new EventHandler(CheckServerConnectionStatus);
            serverConnectionStatusTimer.Interval = 2000;
            serverConnectionStatusTimer.Start();
        }
        
        public void OverwriteStartupConfigIfFirstStartup()
        {
            if(VariableManager.VariableDictionary["firststartup"].value == "TRUE")
            {
                ConfigHandler.OverwriteStartupConfiguration(
                    VariableManager.VariableDictionary["serverdest"].value, VariableManager.VariableDictionary["configdest"].value);

                VariableManager.ToggleFirstStartup();
            }
        }

        private void CheckServerConnectionStatus(object sender, EventArgs e)
        {
            switch (MainServer.ServerStatus)
            {
                case ServerStatus.NotAvailable:
                    ServerStatus = ServerStatus.NotAvailable;
                    break;
                default:
                    ServerLastConnectionTime = DateTime.Now;
                    ServerStatus = ServerStatus.Available;
                    break;
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
            foreach (var di in toCheck.Where(di => di.Exists == false))
            {
                toCheck.Remove(di);
            }

            return toCheck;
        }
        public override string ToString()
        {
            var returnString = "";

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
