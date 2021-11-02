using Bugtracker.Attributes;
using Bugtracker.Console;
using Bugtracker.GlobalsInformation;

namespace bugracker.Console.Commands
{
    [Command("target", "Utility to manage targets.")]
    [Arguments(new[] { "arguments"})]
    class TargetCommand : Command
    {
        public override string Execute()
        {
            return base.Execute();
        }
    }

    [Command("list", "Lists all Targets of the running configuration.", typeof(TargetCommand))]
    class TargetListCommand : Command
    {
        public override string Execute()
        {
            return RunningConfiguration.GetInstance().TargetManager.ToString();
        }
    }
}
