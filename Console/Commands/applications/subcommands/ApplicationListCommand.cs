using System;
using Bugtracker.Attributes;
using Bugtracker.Configuration;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Console.Commands.applications.subcommands
{
    [Command("list", "Lists all Applications of the running configuration.", typeof(ApplicationManagerCommand))]
    [Arguments(null, new[] { "all / specific" })]
    class ApplicationListCommand : Command
    {
        public override string Execute()
        {
            return RunningConfiguration.GetInstance().Applications.ToString();
        }
    }

    [Command("all", "Lists all Applications of the running configuration", typeof(ApplicationListCommand))]
    class ApplicationListAllCommand : Command
    {
        public override string Execute()
        {
            return GlobalMessages.NOT_IMPLEMENTED + Environment.NewLine;
        }
    }
}
