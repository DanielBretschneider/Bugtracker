using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Console.Commands.applications.subcommands
{
    [Command("add", "Adds a new Application to the running - configuration.", typeof(ApplicationManagerCommand))]
    [Arguments(new[] { "name", "exe-path", "standard", "showspec" })]
    class ApplicationAddCommand : Command
    {
        public override string Execute()
        {
            return GlobalMessages.NOT_IMPLEMENTED;
        }
    }
}
