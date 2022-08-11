using System;
using Bugtracker.Attributes;
using Bugtracker.Configuration;
using Bugtracker.InternalApplication;
using Bugtracker.Logging;

namespace Bugtracker.Console.Commands.application
{
    [Command("application", "app", "Lists information for given application")]
    [Arguments(new[] { "subcommand/application" })]
    class ApplicationCommand : Command
    {
        public override string Execute()
        {
            return base.Execute();
        }
    }

    [Command("log", "-l", "Shows Log Info for given application", typeof(ApplicationCommand))]
    [Arguments(new[] { "application-name" })]
    class ApplicationLogCommand : Command
    {
        public override string Execute()
        {
            string retString = "";

            Application appX = RunningConfiguration.GetInstance().Applications.GetApplicationByName(GivenArguments[0]);

            foreach (Log l in appX.LogFiles)
            {
                retString += l.ToString() + Environment.NewLine;
            }

            return retString;
        }
    }
}
