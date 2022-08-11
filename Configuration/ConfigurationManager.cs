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
    public class ConfigurationManager
    {
        private readonly RunningConfiguration rc;

        /// <summary>
        /// Default Constructor,
        /// does nothing
        /// </summary>
        public ConfigurationManager(RunningConfiguration runningConfiguration)
        {
            rc = runningConfiguration;
        }

        public string GetMainServerAddress(
            string customConfigPath = null)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();


            VariableManager vm = rc.Variables;

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

        public static dynamic GetStartupValue(string attribute, string customConfigPath = null)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();

            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("startup"))
                        {
                            //try parsing into boolean
                            if (Boolean.TryParse(reader.GetAttribute(attribute), out bool startupValue))
                            {
                                return startupValue;
                            } // if it fails, return string
                            else
                            {
                                return reader.GetAttribute(attribute);
                            }
                        }
                    }
                }
            }

            throw new Exception("Didn't find attribute in startup configuration.");
        }

        internal static void OverwriteStartupConfig(string customConfigPath = null, params (string attribute, string value)[] settings)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();

            XmlDocument xmlDocument = new();

            xmlDocument.Load(customConfigPath);

            if (xmlDocument.SelectSingleNode("configuration/startup") is XmlElement node)
            {
                foreach (var (attribute, value) in settings)
                {
                    node.SetAttribute(attribute, value);
                }
            }

            xmlDocument.Save(customConfigPath);
        }

        /// <summary>
        /// Returns a List of applications specified in the logfile
        /// Parameters are loglocation type, path, filename (regex), find (per timeperiod)
        /// </summary>
        /// <returns></returns>
        public List<Application> GetSpecifiedApplications(
            string customConfigPath = null)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();

            VariableManager vm = rc.Variables;

            List<Application> applications = new();

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
                            Application appToAdd = new();

                            appToAdd.Name = vm.ReplaceKeywords(reader.GetAttribute("name"));
                            appToAdd.ExecutableLocation = vm.ReplaceKeywords(reader.GetAttribute("executable"));
                            appToAdd.IsStandard = Convert.ToBoolean(vm.ReplaceKeywords(reader.GetAttribute("standard")));

                            Enum.TryParse(vm.ReplaceKeywords(reader.GetAttribute("show")), out Application.ShowAppSpecifier show);

                            appToAdd.ShowSpecifier = show;

                            currentApplication = appToAdd;

                            applications.Add(appToAdd);
                        }


                        if (reader.Name.Equals("log"))
                        {
                            Log logToAppend = new();

                            if (Enum.TryParse<LogLocationType>(vm.ReplaceKeywords(reader.GetAttribute("location")), out LogLocationType type))
                                logToAppend.LocationType = type;

                            logToAppend.Path = vm.ReplaceKeywords(reader.GetAttribute("path"));
                            logToAppend.Filename = vm.ReplaceKeywords(reader.GetAttribute("filename"));


                            if (Enum.TryParse<LogFindSpecifier>(vm.ReplaceKeywords(reader.GetAttribute("find")), out LogFindSpecifier findSpec))
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

        public List<ProblemCategory> GetSpecifiedProblemCategories(
            string customConfigPath = null)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();

            VariableManager vm = rc.Variables;

            List<ProblemCategory> problemCategories = new();

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
                            ProblemCategory categoryToAdd = new();
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

                            string configurationPath = Globals.GetFittingConfigFilesPath();

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


                                        if (Directory.Exists(GetStartupValue(Globals_and_Information.Globals.LOCAL_STARTUP_CONFIG_FILE_PATH)))
                                            configurationPath = GetStartupValue(Globals_and_Information.Globals.LOCAL_STARTUP_CONFIG_FILE_PATH);

                                        foreach (string path in Directory.GetFiles(configurationPath, "*.xml"))
                                        {
                                            foreach (Application a in this.GetSpecifiedApplications(path))
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

        public List<Target> GetSpecifiedTargets(
            string customConfigPath = null)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();

            VariableManager vm = rc.Variables;

            List<Target> targets = new();

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
                            Target targetToAdd = new();

                            targetToAdd.Name = vm.ReplaceKeywords(reader.GetAttribute("name"));


                            if (Enum.TryParse<TargetType>(vm.ReplaceKeywords(reader.GetAttribute("type")), out TargetType type))
                                targetToAdd.TargetType = type;

                            bool defaultT = false;

                            if (reader.GetAttribute("default") != null)
                                bool.TryParse(vm.ReplaceKeywords(reader.GetAttribute("default")), out defaultT);

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

        public LoggingSeverity GetLoggingSeverity(
            string customConfigPath = null)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();

            VariableManager vm = rc.Variables;

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
        public static bool IsLoggingEnabled(
            string customConfigPath = null)
        {
            if (customConfigPath == null)
                customConfigPath = Globals_and_Information.Globals.GetFittingStartupConfigPath();

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
