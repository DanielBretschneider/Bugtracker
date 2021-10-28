using Bugtracker.Globals_and_Information;
using Bugtracker.Attributes;

namespace Bugtracker.Console.Commands.applications
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
