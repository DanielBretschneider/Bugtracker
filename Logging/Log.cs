using System;

namespace Bugtracker.Logging
{
    /// <summary>
    /// This Class represents the specified log in the config.xml
    /// it has a location type, a path, a filename and timespan specifier
    /// </summary>
    public class Log
    {
        #region properties
        public enum LogLocationType
        {
            client,
            host,
            server
        }

        public enum LogFindSpecifier
        {
            NEW,
            ALL
        }
        /// <summary>
        /// The Log Location Type specifies if the log file is stored on the server or on the client
        /// </summary>
        public LogLocationType LocationType { get; set; }

        public LogFindSpecifier Find { get; internal set; }

        /// <summary>
        /// The Path specifies the location of the log file
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The Filename specifies the name of the logfile
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// The TimeSpan specifies from which point in time (x) to which other point in time (x) 
        /// the log lines will be fetched
        /// </summary>
        public TimeSpan TimeSpan { get; set; }

        public string? Lines { get; set; }

        #endregion

        public override string ToString()
        {
            string retString = "";

            retString += "Log Information" + Environment.NewLine;
            retString += "Path: " + Path + Environment.NewLine;
            retString += "Location Type: " + Enum.GetName(typeof(LogLocationType), LocationType) + Environment.NewLine;
            retString += "Log FIND: " + Enum.GetName(typeof(LogFindSpecifier), Find) + Environment.NewLine;
            retString += "Filename: " + Filename + Environment.NewLine;

            return retString;
        }
    }
}
