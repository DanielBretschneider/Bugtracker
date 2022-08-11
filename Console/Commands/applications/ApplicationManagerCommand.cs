using System;
using Bugtracker.Attributes;
using Bugtracker.Configuration;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;

namespace Bugtracker.Console.Commands.applications
{
    [Command("applications", "apps", "Utility to manage applications")]
    [Arguments(new[] { "subcommand" }, new[] { "optionalParams" })]
    class ApplicationManagerCommand : Command
    {
        public override string Execute()
        {
            return base.Execute();
        }
    }

    [Command("logs", "-l", "Shows Logs Info for all applications", typeof(ApplicationManagerCommand))]
    class ApplicationsLogsCommand : Command
    {
        public override string Execute()
        {
            string retString = "";

            ApplicationManager appX = RunningConfiguration.GetInstance().Applications;

            foreach (Application app in appX.GetApplications())
            {
                retString += app.Name + Environment.NewLine;

                foreach (Log log in app.LogFiles)
                {
                    retString += log.ToString() + Environment.NewLine;
                }
            }

            return retString;
        }
    }

    [Command("installed", "-i", "Shows Log info of all installed applications", typeof(ApplicationsLogsCommand))]
    class ApplicationsLogsInstalledCommand : Command
    {
        public override string Execute()
        {
            string retString = "";

            ApplicationManager appX = RunningConfiguration.GetInstance().Applications;

            foreach (Application app in appX.GetApplications())
            {
                retString += app.Name + Environment.NewLine;

                foreach (Log log in app.LogFiles)
                {
                    if(app.IsInstalled)
                        retString += log.ToString() + Environment.NewLine;
                }
            }

            return retString;
        }
    }
}
