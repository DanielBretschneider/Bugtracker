using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;
using Bugtracker.Logging;
using System;
using Bugtracker.Configuration;

namespace Bugtracker.Console.Commands.logger
{
    [Command("logger", "log", "Logger command utility")]
    [Arguments(new[] { "subcommand" })]
    class LoggerCommand : Command
    {
        public override string Execute()
        {
            return GlobalMessages.NOT_IMPLEMENTED;
        }
    }

    [Command("log", "-l", "Saves log message", typeof(LoggerCommand))]
    [Arguments(new[] { "log message" }, new[] { "loggin severity" })]
    class LoggerLogCommand : Command
    {
        public override string Execute()
        {
            string logMessage = "";

            foreach (string argument in GivenArguments)
            {
                logMessage += argument + " ";
            }

            Logger.Log(logMessage, LoggingSeverity.Info);

            return "Saved: " + "\"" + logMessage + "\" to log." + Environment.NewLine;
        }
    }

    [Command("enabled", "-e", "Enables\\Disables Logger for current Session", typeof(LoggerCommand))]
    [Arguments(new[] { "true\\false" })]
    class LoggerEnableCommand : Command
    {
        public override string Execute()
        {
            bool x;

            Boolean.TryParse(GivenArguments[0], out x);

            if (x == true)
            {
                RunningConfiguration.GetInstance().LoggerEnabled = true;
                return "Enabled Logger";
            }

            if (x == false)
            {
                RunningConfiguration.GetInstance().LoggerEnabled = false;
                return "Disabled Logger";
            }
            return "";

        }
    }

    [Command("path", "-p", "Shows current path of log file", typeof(LoggerCommand))]
    class LoggerPathCommand : Command
    {
        public override string Execute()
        {
            return "Current Logging-Path ist: " + Globals.LOG_FILE_PATH;
        }
    }

    [Command("status", "-s", "Shows current status of logger", typeof(LoggerCommand))]
    class LoggerStatusCommand : Command
    {
        public override string Execute()
        {
            return "Logger is enabled: " + RunningConfiguration.GetInstance().LoggerEnabled;
        }
    }
}
