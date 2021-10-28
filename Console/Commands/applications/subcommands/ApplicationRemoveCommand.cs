using Bugtracker.Globals_and_Information;
using Bugtracker.Attributes;

namespace Bugtracker.Console.Commands.applications
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
