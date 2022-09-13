using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using Bugtracker.Problem_Descriptors;
using Bugtracker.Targeting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Bugtracker.Utils;
using Bugtracker.Variables;
using Timer = System.Windows.Forms.Timer;
using Bugtracker.Plugin;
using System.Threading.Tasks;

namespace Bugtracker.Configuration
{
    /// <summary>
    /// The current status of the Server of the running instance server connection
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// Server is available.
        /// </summary>
        Available,
        /// <summary>
        /// Server is not available.
        /// </summary>
        NotAvailable
    }

    /// <summary>
    /// Type of Config Source
    /// </summary>
    public enum ConfigSource
    {
        Server,
        Client
    }

    public class RunningConfiguration : Singleton<RunningConfiguration>
    {

        //public Task GetServerStatus
        //{
        //    return Task.Run(() => ServerStatus)
        //}

        public static event EventHandler InitiliazedRunningConfiguration;

        /// <summary>
        /// The Manager Object for modifying Applications
        /// </summary>
        public ApplicationManager Applications { get; set; }

        /// <summary>
        /// The Manager Object for modifying Targets
        /// </summary>
        public TargetManager Targets { get; set; }

        /// <summary>
        /// The Manager Object for modifying Problems and their categories
        /// </summary>
        public ProblemManager ProblemCategories { get; set; }

        /// <summary>
        /// The Manager Object for storing variables that can be used in configuration
        /// </summary>
        public VariableManager Variables { get; set; }

        /// <summary>
        /// The Manager Object reading and writing config-file data to and from running Configuration
        /// </summary>
        public ConfigurationManager Configurations { get; protected set; }

        [Key("version")]
        public string Version
        {
            get
            {
                return null;
            }
            //get
            //{
            //    if (ApplicationDeployment.IsNetworkDeployed)
            //        myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            //}
        }


        /// <summary>
        /// For Folder 
        /// </summary>
        [Key("idString")]
        public String IdentificationString
        { 
            get
            {
                if(PCInfo.IsRemoteSession)
                {
                    return PCInfo.Clientname + "-on-" + PCInfo.Hostname;
                }
                else
                {
                    return PCInfo.Clientname;
                }
            }
        } 

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
        public DateTime ServerLastConnectionTime 
        { 
            get; set; 
        }
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

        /// <summary>
        /// The Main Server Object containing ServerStatus and other networking information
        /// </summary>
        public Server MainServer { get; set; }

        /// <summary>
        /// LoggerEnabled
        /// </summary>
        public bool LoggerEnabled { get; set; }

        /// <summary>
        /// Returns the currently selected problem category, either selcted via the console or gui
        /// </summary>
        public ProblemCategory SelectedProblemCategory { get; set; }

        /// <summary>
        /// Abrreviation of Selected Problem Category used for variable replacement in configuration
        /// </summary>
        [Key("abbrev", true)]
        public string SelectedProblemCategoryAbbrev
        {
            get
            {
                if (SelectedProblemCategory != null)
                    return SelectedProblemCategory.TicketAbbreviation;
                else
                    return "non-selected";
            }
        }

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

        [Key("firststartup")]
        public bool FirstStartup { get; set; } = (bool) ConfigurationManager.GetStartupValue("firstStartup");

        [Key("configurationFolderPath")]
        public string ConfigurationFolderPath { get; protected set; }

        /// <summary>
        /// The startup time of the application
        /// </summary>
        [Key("startupTime")]
        public DateTime StartupTime { get; protected set; }

        private List<DirectoryInfo> _bugtrackerFolders;

        /// <summary>
        /// All Bugtrack Folders of current Session
        /// </summary>
        public List<DirectoryInfo> BugtrackerFolders
        {
            get => BugtrackerUtils.GetAllExisitingDirectories(_bugtrackerFolders);

            set => _bugtrackerFolders = value;
        }

        /// <summary>
        /// List of all current loaded Plugins
        /// </summary>
        public List<IPlugin> LoadedPlugins = new();

        public string dateString;

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
        /// The most recently created bugtrack folder
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
        /// Returns the name of the most recent bugtrack folder
        /// </summary>
        public string BugtrackerFolderName => NewestBugtrackerFolder.Name;


        /// <summary>
        /// Default constructor of Running Configuration, use the InitStartupProcedure Method to begin
        /// </summary>
        public RunningConfiguration()
        {
            //first, initialize pcinfo object - collects usefull pc info
            PcInfo                      = new PCInfo();
            //second, initialize variable manager - load in all variables that could be used in the configuration
            Variables             = new VariableManager(this, PcInfo);
            //after initializing data that can be used in the configuration files, load in all config files and replace all placeholders with values stored in variables
            Configurations        = new ConfigurationManager(this);
        }

        public void InitStartupProcedure()
        {

            //sets server address according to configuration file
            ServerAddress               = Configurations.GetMainServerAddress();
            //set and initializes new server object with server address loaded in from configuration
            MainServer                  = new Server(ServerAddress);

            //variable manager loads in all variables for the first time
            Variables.FullRefresh();
            //sets configuration folder path according to configuration file
            ConfigurationFolderPath = ConfigurationManager.GetStartupValue("loadConfigsFrom");
            //after setting the configuraion folder path, refreshes all variables
            Variables.FullRefresh();
            //sets server path according to configuration file
            ServerPath = ConfigurationManager.GetStartupValue("mainserver");
            //after setting the server path, refreshes all variables
            Variables.FullRefresh();

            //Init new List for BugtrackerFolders
            BugtrackerFolders     = new List<DirectoryInfo>();
            //Initializes all other Manager Objects
            Applications          = new ApplicationManager();
            Targets               = new TargetManager();
            ProblemCategories     = new ProblemManager();

            StartupTime = DateTime.Now;

            Load();
            SetupConnectionStatusTimer();

            InitiliazedRunningConfiguration?.Invoke(null, null);
        }

        private Timer _serverConnectionStatusTimer;
        private readonly ProblemCategory _selectedProblemCategory;

        /// <summary>
        /// 
        /// </summary>
        public void SetupConnectionStatusTimer()
        {
            _serverConnectionStatusTimer             = new Timer();
            _serverConnectionStatusTimer.Tick        += new EventHandler(CheckServerConnectionStatusAsync);
            _serverConnectionStatusTimer.Interval    = 2000;
            _serverConnectionStatusTimer.Start();
        }

        public event CheckServerConnectionCompletedEventHandler CheckServerConnectionCompleted;

        public delegate void CheckServerConnectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

        private void CheckServerConnectionStatusAsync(object sender, EventArgs e)
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

        private void Load()
        {
            string[] configPaths;

            if (Directory.Exists(ConfigurationFolderPath))
            {
                configPaths     = Directory.GetFiles(ConfigurationFolderPath, "*.xml");
                ConfigSource    = ConfigSource.Server;
            }
            else
            {
                configPaths     = Directory.GetFiles(Globals.INTERNAL_CONFIG_FOLDER_PATH, "*.xml");
                ConfigSource    = ConfigSource.Client;
            }
            
            //copy files to internal config folder
            foreach (var filePath in configPaths)
            {
                if(ConfigSource == ConfigSource.Server)
                {
                    File.Copy(Path.Join(ConfigurationFolderPath, Path.GetFileName(filePath)), Path.Join(Globals.INTERNAL_CONFIG_FOLDER_PATH, Path.GetFileName(filePath)), true);
                }

                LoggerEnabled   = ConfigurationManager.IsLoggingEnabled(filePath);
                LogSeverity     = Configurations.GetLoggingSeverity(filePath);

                Applications.Applications.AddRange(Configurations.GetSpecifiedApplications(filePath));
                Targets.Targets.AddRange(Configurations.GetSpecifiedTargets(filePath));
                ProblemCategories.ProblemCategories.AddRange(Configurations.GetSpecifiedProblemCategories(filePath));
            }
        }

        public override string ToString()
        {
            var returnString = "";

            returnString += "PcInfo: \n \n";
            returnString += PCInfo.Summary() + Environment.NewLine;
            returnString += "Current Bugtracker Folder Name: " + NewestBugtrackerFolder + Environment.NewLine;
            returnString += "Logger Enabled: " + LoggerEnabled + Environment.NewLine;
            returnString += "Log Severity: " + Enum.GetName(typeof(LoggingSeverity), LogSeverity) + Environment.NewLine;
            returnString += "Target Path: " + TargetPath;

            return returnString;
        }
    }
}
