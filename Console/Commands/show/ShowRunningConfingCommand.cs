using Bugtracker.Attributes;
using Bugtracker.Configuration;
using Bugtracker.Console.Commands.util;

namespace Bugtracker.Console.Commands.show
{
    [Command("show", "shw", "Utility to show current running configuration parameters.")]
    [Arguments(new[] { "parameters to show" })]
    class ShowCommand : Command
    {
        public override string Execute()
        {
            return "";
        }
    }

    [Command("config", "conf", "Shows all running config parameters", typeof(ShowCommand))]
    class ShowRunningConfigCommand : Command
    {
        public override string Execute()
        {
            return RunningConfiguration.GetInstance().ToString();
        }
    }

    [Command("path", "path", "Shows current bugtracker path", typeof(ShowCommand))]
    class ShowRunningConfigBugtrackerPathCommand : Command
    {
        public override string Execute()
        {
            if (RunningConfiguration.GetInstance().NewestBugtrackerFolder != null)
                return RunningConfiguration.GetInstance().NewestBugtrackerFolder.FullName;
            else
                return "Not created yet use: " + BugtrackConsole.GetReverseLookUpForCommand(typeof(UtilInitCommand)) + "to initialize.";
        }
    }
}
