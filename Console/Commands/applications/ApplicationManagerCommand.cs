using Bugtracker.Attributes;
using Bugtracker.GlobalsInformation;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;
using System;

namespace Bugtracker.Console
{
    [Command("applications", "apps", "Utility to manage applications")]
    [Arguments(new[] { "subcommand"}, new[] { "optionalParams"})]
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

            ApplicationManager appX = RunningConfiguration.GetInstance().ApplicationManager;

            foreach(Application app in appX.GetApplications())
            {
                foreach(Log log in app.LogFiles)
                {
                    retString += log.ToString() + Environment.NewLine;
                }
            }

            return retString;
        }
    }
}
