using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Bugtracker.Exceptions;
using Bugtracker.Globals_and_Information;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using Bugtracker.Problem_Descriptors;
using Bugtracker.Targeting;
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

        public static bool IsGUIEnabledOnStartup(string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            bool GUIEnabled = false;

            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("startup"))
                            Boolean.TryParse(reader.GetAttribute("startGUI"), out GUIEnabled);
                    }
                }
            }

            return GUIEnabled;
        }

        public static string GetConfigurationFolderPath(string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            string path = "";

            using (XmlReader reader = XmlReader.Create(Globals.LOCAL_CONFIG_FILE_PATH))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName.Equals("startup"))
                            path = reader.GetAttribute("loadConfigsFrom");
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
        public static List<Application> GetSpecifiedApplications(string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
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

                            appToAdd.Name = reader.GetAttribute("name");
                            appToAdd.ExecutableLocation = reader.GetAttribute("executable");
                            appToAdd.Enabled = Convert.ToBoolean(reader.GetAttribute("enable"));
                            appToAdd.IsStandard = Convert.ToBoolean(reader.GetAttribute("standard"));
                            appToAdd.ShowSpecifier = ConvertStringToShowSpecifier(reader.GetAttribute("show"), appToAdd.Name, lineInfo.LineNumber, lineInfo.LinePosition);

                            currentApplication = appToAdd;

                            applications.Add(appToAdd);
                        }


                        if (reader.Name.Equals("log"))
                        {
                            Log logToAppend = new Log();
                            LogLocationType type = LogLocationType.client;

                            if (Enum.TryParse<LogLocationType>(reader.GetAttribute("location"), out type))
                                logToAppend.LocationType = type;

                            logToAppend.Path = reader.GetAttribute("path");
                            logToAppend.Filename = reader.GetAttribute("filename");

                            LogFindSpecifier findSpec = LogFindSpecifier.NEW;

                            if (Enum.TryParse<LogFindSpecifier>(reader.GetAttribute("find"), out findSpec))
                                logToAppend.Find = findSpec;

                            if (currentApplication != null)
                                currentApplication.LogFiles.Add(logToAppend);
                        }
                    }
                }
            }

            return applications;
        }

        public static List<ProblemCategory> GetSpecifiedProblemCategories(string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
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
                            categoryToAdd.Name = reader.GetAttribute("name");
                            currentProblemCategory = categoryToAdd;
                            problemCategories.Add(categoryToAdd);
                        }

                        if (reader.Name.Equals("description"))
                        {
                            string descriptorText = reader.GetAttribute("text");

                            if (currentProblemCategory != null)
                                currentProblemCategory.Descriptions.Add(descriptorText);
                        }

                        if(reader.Name.Equals("app-selection"))
                        {
                            string selection = reader.ReadElementContentAsString();
                            System.Diagnostics.Debug.WriteLine("selection text: " + selection);
                            string[] splitSelect = selection.Split(',');

                            

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
                                        //currentProblemCategory.SelectedApplications.Add(RunningConfiguration.GetInstance().ApplicationManager.GetApplicationByName(s));

                                        foreach(string path in Directory.GetFiles(GetConfigurationFolderPath(), "*.xml"))
                                        {
                                            foreach (Application a in GetSpecifiedApplications(path))
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

        public static List<Target> GetSpecifiedTargets(string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
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

                            targetToAdd.Name = reader.GetAttribute("name");

                            TargetType type = TargetType.folder;

                            if (Enum.TryParse<TargetType>(reader.GetAttribute("type"), out type))
                                targetToAdd.TargetType = type;

                            bool defaultT = false;

                            if (reader.GetAttribute("default") != null)
                                Boolean.TryParse(reader.GetAttribute("default"), out defaultT);

                            targetToAdd.Default = defaultT;

                            targetToAdd.Path = reader.GetAttribute("path");
                            targetToAdd.Address = reader.GetAttribute("address");

                            targets.Add(targetToAdd);
                        }
                    }
                }
            }

            return targets;
        }

        public static LoggingSeverity GetLoggingSeverity(string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
            using (XmlReader reader = XmlReader.Create(customConfigPath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name.Equals("logger"))
                        {
                            switch (reader.GetAttribute("severity"))
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
        public static bool IsLoggingEnabled(string customConfigPath = Globals.LOCAL_CONFIG_FILE_PATH)
        {
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

        public static Application.ShowAppSpecifier ConvertStringToShowSpecifier(string content, string appname, int line, int linePosition)
        {
            if (content.Contains("onExist"))
                return Application.ShowAppSpecifier.onExist;
            else if (content.Contains("show"))
                return Application.ShowAppSpecifier.show;
            else if (content.Contains("hide"))
                return Application.ShowAppSpecifier.hide;
            else
            {
                string showSpecifierList = "";

                int i = 0;
                int l = Enum.GetNames(typeof(Application.ShowAppSpecifier)).Count();
                foreach (string showSpecifier in Enum.GetNames(typeof(Application.ShowAppSpecifier)))
                {
                    i++;
                    if (i < l - 1)
                        showSpecifierList += showSpecifier + ", ";
                    else
                        showSpecifierList += showSpecifier;
                }

                throw new ConfigFileParseException(appname + " at line " + line + "postion: " + linePosition +
                    ".show=\"\" has to be one of the following: " + showSpecifierList);
            }

        }

    }

}
