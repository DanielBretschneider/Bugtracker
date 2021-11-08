using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Console.Commands.applications.subcommands
{
    [Command("remove", "Removes an Application from the running configuration.", typeof(ApplicationManagerCommand))]
    [Arguments(new[] { "appname" })]
    class ApplicationRemoveCommand : Command
    {
        public override string Execute()
        {
            return GlobalMessages.NOT_IMPLEMENTED;
        }
    }
}
