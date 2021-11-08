using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Bugtracker.Exceptions;
using Bugtracker.Globals_and_Information;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
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
            //nop
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
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
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


        /// <summary>
        /// Returns a List of applications specified in the logfile
        /// Parameters are loglocation type, path, filename (regex), find (per timeperiod)
        /// </summary>
        /// <returns></returns>
        public static List<Application> GetSpecifiedApplications()
        {
            List<Application> applications = new List<Application>();

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
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

        public static List<Target> GetSpecifiedTargets()
        {
            List<Target> targets = new List<Target>();

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
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

        public static LoggingSeverity GetLoggingSeverity()
        {
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
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
        public static bool IsLoggingEnabled()
        {
            // value of target aka site/ip to be pinged
            bool log = false;

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
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
