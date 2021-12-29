using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Bugtracker.Globals_and_Information;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using Bugtracker.Problem_Descriptors;
using Bugtracker.Targeting;
using Bugtracker.Variables;
using static Bugtracker.Logging.Log;

namespace Bugtracker.Configuration
{
    /// <summary>
    /// This class is only here to handle all the XML-Magic
    /// </summary>
    class ConfigHandler
    {
        /// <summary>
        /// Default Constructor,
        /// does nothing
        /// </summary>
        public ConfigHandler()
        {

        }

        /// <summary>
        /// Returns number of applications specified in bugtracker Config
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfApplications()
        {
            // value of target aka site/ip to be pinged
            int appCount = 0;

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(Globals.LOCAL_CONFIG_FILE_PATH))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName == "application")
                        {
                            appCount++;
                        }
                    }
                }
            }

            return appCount;
        }

        public static string GetMainServerAddress(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILES_PATH)
        {
            VariableManager vm = rc.VariableManager;

            string serverAddress = "";

            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("startup"))
                            serverAddress = vm.ReplaceKeywords(reader.GetAttribute("mainserver"));
                    }
                }
            }

            return serverAddress;
        }
        public static bool IsGUIEnabledOnStartup(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            VariableManager vm = rc.VariableManager;

            bool GUIEnabled = false;

            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("startup"))
                            Boolean.TryParse(vm.ReplaceKeywords(reader.GetAttribute("startGUI")), out GUIEnabled);
                    }
                }
            }

            return GUIEnabled;
        }

        public static string GetConfigurationFolderPath(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            VariableManager vm = rc.VariableManager;

            string path = "";

            using (XmlReader reader = XmlReader.Create(Globals.LOCAL_CONFIG_FILE_PATH))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("startup"))
                            path = vm.ReplaceKeywords(reader.GetAttribute("loadConfigsFrom"));
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Returns a List of applications specified in the logfile
        /// Parameters are loglocation type, path, filename (regex), find (per timeperiod)
        /// </summary>
        /// <returns></returns>
        public static List<Application> GetSpecifiedApplications(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            VariableManager vm = rc.VariableManager;

            List<Application> applications = new List<Application>();

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                Application currentApplication = null;

                IXmlLineInfo lineInfo = (IXmlLineInfo)reader;
                int line = lineInfo.LineNumber;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("application"))
                        {
                            Application appToAdd = new Application();

                            appToAdd.Name = vm.ReplaceKeywords(reader.GetAttribute("name"));
                            appToAdd.ExecutableLocation = vm.ReplaceKeywords(reader.GetAttribute("executable"));
                            appToAdd.IsStandard = Convert.ToBoolean(vm.ReplaceKeywords(reader.GetAttribute("standard")));

                            Application.ShowAppSpecifier show;

                            Enum.TryParse<Application.ShowAppSpecifier>(vm.ReplaceKeywords(reader.GetAttribute("show")), out show);

                            appToAdd.ShowSpecifier = show;

                            currentApplication = appToAdd;

                            applications.Add(appToAdd);
                        }


                        if (reader.Name.Equals("log"))
                        {
                            Log logToAppend = new Log();
                            LogLocationType type = LogLocationType.client;

                            if (Enum.TryParse<LogLocationType>(vm.ReplaceKeywords(reader.GetAttribute("location")), out type))
                                logToAppend.LocationType = type;

                            logToAppend.Path = vm.ReplaceKeywords(reader.GetAttribute("path"));
                            logToAppend.Filename = vm.ReplaceKeywords(reader.GetAttribute("filename"));

                            LogFindSpecifier findSpec = LogFindSpecifier.NEW;

                            if (Enum.TryParse<LogFindSpecifier>(vm.ReplaceKeywords(reader.GetAttribute("find")), out findSpec))
                                logToAppend.Find = findSpec;

                            logToAppend.Lines = reader.GetAttribute("lines");

                            if (currentApplication != null)
                                currentApplication.LogFiles.Add(logToAppend);
                        }

                        if (reader.Name.Equals("pre-fetch"))
                        {
                            if (currentApplication != null)
                                currentApplication.PreFetchExecutionPath = vm.ReplaceKeywords(reader.GetAttribute("path"));
                        }

                        if (reader.Name.Equals("post-fetch"))
                        {
                            if (currentApplication != null)
                                currentApplication.PostFetchExecutionPath = vm.ReplaceKeywords(reader.GetAttribute("path"));
                        }
                    }
                }
            }

            return applications;
        }

        public static List<ProblemCategory> GetSpecifiedProblemCategories(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            VariableManager vm = rc.VariableManager;

            List<ProblemCategory> problemCategories = new List<ProblemCategory>();

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                ProblemCategory currentProblemCategory = null;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("problem-category"))
                        {
                            ProblemCategory categoryToAdd = new ProblemCategory();
                            categoryToAdd.Name = vm.ReplaceKeywords(reader.GetAttribute("name"));
                            categoryToAdd.TicketAbbreviation = vm.ReplaceKeywords(reader.GetAttribute("ticket"));
                            currentProblemCategory = categoryToAdd;
                            problemCategories.Add(categoryToAdd);
                        }

                        if (reader.Name.Equals("description"))
                        {
                            string descriptorText = vm.ReplaceKeywords(reader.GetAttribute("text"));

                            if (currentProblemCategory != null)
                                currentProblemCategory.Descriptions.Add(descriptorText);
                        }

                        if(reader.Name.Equals("app-selection"))
                        {
                            string selection = reader.ReadElementContentAsString();
                            System.Diagnostics.Debug.WriteLine("selection text: " + selection);
                            string[] splitSelect = selection.Split(',');

                            string configurationPath = Globals.LOCAL_CONFIG_FILES_PATH;

                            if (currentProblemCategory != null)
                            {
                                foreach (string s in splitSelect)
                                {
                                    Regex.Replace(s, @"\s+", "");

                                    if (s.Equals("All"))
                                        currentProblemCategory.SelectAllApplications = true;

                                    if (s.Equals("Screen"))
                                        currentProblemCategory.SelectScreenshot = true;

                                    if (!s.Equals("All") && !s.Equals("Screen") && !s.Equals(""))
                                    {

                                        if (Directory.Exists(GetConfigurationFolderPath(rc)))
                                            configurationPath = GetConfigurationFolderPath(rc);

                                        foreach (string path in Directory.GetFiles(configurationPath, "*.xml"))
                                        {
                                            foreach (Application a in GetSpecifiedApplications(rc, path))
                                            {
                                                if (a.Name == s)
                                                {
                                                    currentProblemCategory.SelectedApplications.Add(a);
                                                }
                                            }
                                        }

                                    }
                                        
                                }

                                Logger.Log("Content of " + currentProblemCategory.Name + " : " + currentProblemCategory.SelectedApplications.ToString(), LoggingSeverity.Info);
                                Logger.Log("Screenshot selection: " + currentProblemCategory.SelectScreenshot, LoggingSeverity.Info);
                                Logger.Log("Alle apps selected " + currentProblemCategory.SelectScreenshot, LoggingSeverity.Info);
                            }
                        }
                    }
                }
            }

            return problemCategories;
        }

        public static List<Target> GetSpecifiedTargets(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            VariableManager vm = rc.VariableManager;

            List<Target> targets = new List<Target>();

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {

                IXmlLineInfo lineInfo = (IXmlLineInfo)reader;
                int line = lineInfo.LineNumber;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("target"))
                        {
                            Target targetToAdd = new Target();

                            targetToAdd.Name = vm.ReplaceKeywords(reader.GetAttribute("name"));

                            TargetType type = TargetType.folder;

                            if (Enum.TryParse<TargetType>(vm.ReplaceKeywords(reader.GetAttribute("type")), out type))
                                targetToAdd.TargetType = type;

                            bool defaultT = false;

                            if (reader.GetAttribute("default") != null)
                                Boolean.TryParse(vm.ReplaceKeywords(reader.GetAttribute("default")), out defaultT);

                            targetToAdd.Default = defaultT;

                            targetToAdd.Path = vm.ReplaceKeywords(reader.GetAttribute("path"));
                            targetToAdd.Address = vm.ReplaceKeywords(reader.GetAttribute("address"));

                            targetToAdd.CustomBugtrackerFolderName = reader.GetAttribute("foldername");

                            targets.Add(targetToAdd);
                        }
                    }
                }
            }

            return targets;
        }

        public static LoggingSeverity GetLoggingSeverity(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            VariableManager vm = rc.VariableManager;

            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name.Equals("logger"))
                        {
                            switch (vm.ReplaceKeywords(reader.GetAttribute("severity")))
                            {
                                case "1":
                                    return LoggingSeverity.Error;

                                case "2:":
                                    return LoggingSeverity.Warning;

                                case "3":
                                    return LoggingSeverity.Info;
                                default:
                                    //TODO: Write Exception for error in logging severity.
                                    System.Diagnostics.Debug.Write("Severity in config not valid.");
                                    throw new Exception("Logging Severity not correctly defined!");
                            }
                        }
                    }
                }
            }

            return LoggingSeverity.Null;
        }

        /// <summary>
        /// To log, or not to log.
        /// Checks if logging is enabled via the logger - enabled xml tag and attribute
        /// </summary>
        /// <returns></returns>
        public static bool IsLoggingEnabled(RunningConfiguration rc,
            string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            VariableManager vm = rc.VariableManager;
            // value of target aka site/ip to be pinged
            bool log = false;

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name.Equals("logger"))
                        {

                            if (reader.GetAttribute("enabled").Equals("true"))
                            {
                                System.Diagnostics.Debug.Write("Logger is enabled!");
                                log = true;
                                return log;
                            }
                        }
                    }
                }
            }

            return log;
        }

    }

}
