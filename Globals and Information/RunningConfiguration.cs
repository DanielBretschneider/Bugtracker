using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker.GlobalsInformation
{
    public enum LoggingSeverity
    {
        //TODO: Remove Null
        Null = 99,
        Notification = 3,
        Warning = 2,
        Error = 1
    }

    class RunningConfiguration : Singleton<RunningConfiguration>
    {
        public bool                 LoggerEnabled { get; set; }
        public  LoggingSeverity     LogSeverity { get; set; }
        public string               TargetPath { get; set; }

        public RunningConfiguration()
        {
            LogSeverity = ConfigHandler.GetLoggingSeverity();
            LoggerEnabled = ConfigHandler.IsLoggingEnabled();
        }
    }
}
